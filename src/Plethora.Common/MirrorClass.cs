using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Plethora.Logging;

namespace Plethora
{
    //TODO: Allow MirrorClass to accept 'out' and 'ref' arguments

    /// <summary>
    /// Helper class which reflects on a class, exposing non-public members.
    /// </summary>
    /// <remarks>
    ///  <para>
    ///   This class relies on calling method signatures, and should NOT be used for production
    ///   code. Compiler or JIT optimisation may cause unexpected behaviour.
    ///  </para>
    ///  <para>
    ///   This class intended for use in unit tests. It allows for internal classes, and non-public
    ///   methods to be accessed through an extension class.
    ///  </para>
    /// </remarks>
    /// <example>
    /// In this example 'LambdaKeyer' is an internal class, but it's GetKey method is made
    /// easily accessable using the <see cref="MirrorClass{T}"/> to reflect on the class.
    /// <code>
    /// <![CDATA[
    ///    class LambdaKeyerEx
    ///    {
    ///        private static readonly MirrorClass mirrorClass;
    ///
    ///        static LambdaKeyerEx()
    ///        {
    ///            Type cachedExecutorType = typeof(CachedExecutor);
    ///            string lambdaKeyerName = cachedExecutorType.Namespace + ".LambdaKeyer";
    ///
    ///            mirrorClass = MirrorClass.Create(cachedExecutorType.Assembly, lambdaKeyerName);
    ///        }
    ///
    ///        public static string GetKey(Expression expression)
    ///        {
    ///            //Call the Exec method to execute the underlying method which
    ///            // matches the signature of the containing class.
    ///            //Notice that GetKey is not generic, and so generic arguments aren't required.
    ///            return (string)mirrorClass.Exec(new Type[0], expression);
    ///        }
    ///    }
    /// ]]>
    /// </code>
    /// </example>
    public class MirrorClass
    {
        public static readonly object StaticOnlyContext = new object();

        #region Constants

        private const BindingFlags AllInstanceBindings =
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Instance;

        private const BindingFlags AllStaticBindings =
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Static;
        #endregion

        #region Fields

        private static readonly Dictionary<RuntimeMethodHandle, MethodInfo> methodPool =
            new Dictionary<RuntimeMethodHandle, MethodInfo>();

        private readonly Type reflectedType;
        private readonly object innerInstance;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="MirrorClass"/> with the underlying
        /// reflected type.
        /// The reflected type's constructor called will be matched using the arguments provided.
        /// </summary>
        /// <param name="reflectedType">The type to be reflected for this instance.</param>
        /// <param name="args">The constructor arguments.</param>
        /// <remarks>
        /// If <paramref name="args"/> is specified as <see cref="MirrorClass.StaticOnlyContext"/>,
        /// an instance of the underlying type is not created. Only static calls will be possible
        /// via this <see cref="MirrorClass"/>.
        /// </remarks>
        public MirrorClass(Type reflectedType, params object[] args)
        {
            //Validation
            if (reflectedType == null)
                throw new ArgumentNullException("reflectedType");

            if (args == null)
                throw new ArgumentNullException("args");


            this.reflectedType = reflectedType;

            if ((args.Length == 1) && (args[0] == StaticOnlyContext))
            {
                this.innerInstance = null;
                return;
            }

            this.innerInstance = Activator.CreateInstance(
                reflectedType,
                AllInstanceBindings,
                null,
                args,
                null);
        }
        #endregion

        #region Public Static Factory Methods

        public static MirrorClass Create(Assembly assembly, string fullName, params object[] args)
        {
            //Validation
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            if (fullName == null)
                throw new ArgumentNullException("fullName");


            Type reflectType = assembly.GetType(fullName);
            if (reflectType == null)
                throw new ArgumentException(ResourceProvider.TypeNotFoundInAssembly(fullName, assembly.FullName));

            return new MirrorClass(reflectType, args);
        }

        public static MirrorClass CreateStaticOnly(Assembly assembly, string fullName)
        {
            //Validation
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            if (fullName == null)
                throw new ArgumentNullException("fullName");


            Type reflectType = assembly.GetType(fullName);
            if (reflectType == null)
                throw new ArgumentException(ResourceProvider.TypeNotFoundInAssembly(fullName, assembly.FullName));

            return new MirrorClass(reflectType, StaticOnlyContext);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Executes an instance method on the reflected class. The method executed
        /// matches the calling method's signature.
        /// </summary>
        /// <param name="genericArguments">The types of the closed method's generic arguments.</param>
        /// <param name="args">The arguments to be passed to the method.</param>
        /// <returns>
        /// The result of the underlying method.
        /// </returns>
        public object Exec(Type[] genericArguments, params object[] args)
        {
            MethodBase callingMethod = new StackFrame(1, false).GetMethod();

            //Ensure a static method is not called in a non-static context
            if ((!callingMethod.IsStatic) && (this.innerInstance == null))
                throw new InvalidOperationException(ResourceProvider.StaticOnlyMirror());

            MethodInfo mirroredMethod = GetMirroredMethod(reflectedType, callingMethod, genericArguments);


            object instance = callingMethod.IsStatic
                           ? null
                           : this.innerInstance;

            return mirroredMethod.Invoke(instance, args);
        }
        #endregion

        #region Non-Public Methods

        protected static MethodInfo GetMirroredMethod(Type type, MethodBase callingMethod, Type[] genericArguments)
        {
            //Validation
            if ((callingMethod.IsGenericMethodDefinition) &&
                (callingMethod.GetGenericArguments().Length != genericArguments.Length))
                throw new ArgumentException(ResourceProvider.GenericArgumentsMismatch());


            MethodInfo mirroredMethod = GetCachedMirroredMethodGenericDefinition(type, callingMethod);

            //Create closed method
            if (mirroredMethod.IsGenericMethodDefinition)
                mirroredMethod = mirroredMethod.MakeGenericMethod(genericArguments);

            return mirroredMethod;
        }

        // The calling and returned method are the open method definitions, in the case of generics.
        private static MethodInfo GetCachedMirroredMethodGenericDefinition(Type type, MethodBase callingMethod)
        {
            RuntimeMethodHandle hCallingMethod = callingMethod.MethodHandle;
            MethodInfo method;
            lock (methodPool)
            {
                if (!methodPool.TryGetValue(hCallingMethod, out method))
                {
                    method = GetMirroredMethodGenericDefinition(type, callingMethod);
                    if (method == null)
                        throw new MethodNotFoundException(ResourceProvider.MethodNotFound());

                    methodPool.Add(hCallingMethod, method);
                }
            }

            return method;
        }

        // The calling and returned method are the open method definitions, in the case of generics.
        private static MethodInfo GetMirroredMethodGenericDefinition(Type type, MethodBase callingMethod)
        {
            //Can't just use type.GetMethod(...) because of generic arguments.

            BindingFlags bindings = callingMethod.IsStatic
                ? AllStaticBindings
                : AllInstanceBindings;

            ParameterInfo[] callingParameters = callingMethod.GetParameters();
            Type[] callingGenericArgs = callingMethod.GetGenericArguments();

            var callingParameterTypes = callingParameters.Select(param => param.ParameterType).ToList();

            //Get a list of potential methods (matching by name, no. of arguments and no. of generic parameters)
            var potentialMethods = type.GetMethods(bindings)
                .Where(method =>
                    string.Equals(method.Name, callingMethod.Name) &&
                    (method.GetParameters().Length == callingParameters.Length) &&
                    (method.GetGenericArguments().Length == callingGenericArgs.Length));

            MethodInfo matchedMethod = null;
            foreach (var potentialMethod in potentialMethods)
            {
                MethodInfo typedPotentialMethod;
                try
                {
                    typedPotentialMethod = potentialMethod.IsGenericMethodDefinition
                        ? potentialMethod.MakeGenericMethod(callingGenericArgs)
                        : potentialMethod;
                }
                catch (ArgumentException)
                {
                    continue;
                }

                var typedParameterTypes = typedPotentialMethod.GetParameters().Select(param => param.ParameterType);
                if (Enumerable.SequenceEqual(callingParameterTypes, typedParameterTypes))
                {
                    matchedMethod = potentialMethod;
                    break;
                }
            }

            return matchedMethod;            
        }

        protected static object InvokeMethod(MethodInfo method, object obj, params object[] args)
        {
            return method.Invoke(obj, args);
        }
        #endregion
    }

    /// <summary>
    /// Inheritor from <see cref="MirrorClass"/> where the reflected type is
    /// specified by the generic type parameter.
    /// </summary>
    /// <typeparam name="T">The reflected type.</typeparam>
    /// <remarks>
    /// Provides a static method to call <see cref="Exec"/>.
    /// </remarks>
    public class MirrorClass<T> : MirrorClass
    {
        #region Constructor

        /// <summary>
        /// Creates a new instance of the <see cref="MirrorClass"/> with the underlying
        /// reflected type.
        /// The reflected type's constructor called will be matched using the arguments provided.
        /// </summary>
        /// <param name="args">The constructor arguments.</param>
        public MirrorClass(params object[] args)
            : base(typeof(T), args)
        {
        }
        #endregion

        #region Public Static Methods

        /// <summary>
        /// Executes a static method on the reflected class. The method executed
        /// matches the calling method's signature.
        /// </summary>
        /// <param name="genericArguments">The types of any generic arguments.</param>
        /// <param name="args">The arguments to be passed to the method.</param>
        /// <returns>
        /// The result of the underlying method.
        /// </returns>
        public new static object Exec(Type[] genericArguments, params object[] args)
        {
            MethodBase callingMethod = new StackFrame(1, false).GetMethod();

            //Ensure a static method is not called in a non-static context
            if (!callingMethod.IsStatic)
                throw new InvalidOperationException(ResourceProvider.StaticOnlyMirror());


            MethodInfo mirroredMethod = GetMirroredMethod(typeof(T), callingMethod, genericArguments);

            return InvokeMethod(mirroredMethod, null, args);
        }
        #endregion
    }

    [Serializable]
    public class MethodNotFoundException : IsLoggedException
    {
        #region Constructors

        public MethodNotFoundException(string message)
            : base(message)
        {
        }

        public MethodNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected MethodNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        #endregion
    }
}
