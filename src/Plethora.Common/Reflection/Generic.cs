using System;
using System.Linq;
using System.Reflection;

namespace Plethora.Reflection
{
    /// <summary>
    /// Class which assists with reflecting on generic members.
    /// </summary>
    public static class Generic
    {
        #region Assistance Classes

        // ReSharper disable ConvertToStaticClass

        /// <summary>
        /// Used by <see cref="GetGenericMethod(Type, string, BindingFlags, Binder, Type[], ParameterModifier[])"/> to specify the first generic argument.
        /// </summary>
        public sealed class Arg1 { private Arg1() { } }

        /// <summary>
        /// Used by <see cref="GetGenericMethod(Type, string, BindingFlags, Binder, Type[], ParameterModifier[])"/> to specify the second generic argument.
        /// </summary>
        public sealed class Arg2 { private Arg2() { } }

        /// <summary>
        /// Used by <see cref="GetGenericMethod(Type, string, BindingFlags, Binder, Type[], ParameterModifier[])"/> to specify the third generic argument.
        /// </summary>
        public sealed class Arg3 { private Arg3() { } }

        /// <summary>
        /// Used by <see cref="GetGenericMethod(Type, string, BindingFlags, Binder, Type[], ParameterModifier[])"/> to specify the forth generic argument.
        /// </summary>
        public sealed class Arg4 { private Arg4() { } }

        // ReSharper restore ConvertToStaticClass

        #endregion

        #region GetGenericMethod

        /// <summary>
        /// Searches for the specified public method whose parameters match the specified argument types.
        /// </summary>
        /// <param name="type">
        /// The reflected <see cref="Type"/> in which to find the method.
        /// </param>
        /// <param name="name">
        /// The string containing the name of the public method to get.
        /// </param>
        /// <param name="parameterTypes">
        /// An array of <see cref="Type"/> objects representing the number, order, and type of the parameters
        /// for the method to get.
        /// </param>
        /// <returns>
        /// An object representing the public method whose parameters match the specified argument types, if found;
        /// otherwise, null.
        /// The <see cref="MethodInfo"/> returned represents the generic method declaration.
        /// </returns>
        /// <remarks>
        /// Generic argument types are specified by substituting the types <see cref="Arg1"/>,
        /// <see cref="Generic.Arg2"/>,  <see cref="Generic.Arg3"/> and <see cref="Generic.Arg4"/>,
        /// where Arg1 represents the first generic argument, Arg2 the second, Arg3 the third, etc.
        ///  <example>
        ///   For example, one can obtain the generic method
        ///   using the following call:
        ///   <code>
        ///   <![CDATA[
        ///       // Returns the method definition for
        ///       //  Queryable.Where<TSource>(IQueryable<TSource>, Expression<Func<TSource, bool>>)
        ///       MethodInfo whereMethod = typeof(Queryable).GetGenericMethod(
        ///           "Where",
        ///           new Type[]
        ///               {
        ///                   typeof (IQueryable<Generic.Arg1>),
        ///                   typeof (Expression<Func<Generic.Arg1, bool>>)
        ///               });
        ///   ]]>
        ///   </code>
        ///  </example>
        /// </remarks>
        public static MethodInfo GetGenericMethod(this Type type, string name, Type[] parameterTypes)
        {
            return GetGenericMethod(
                type,
                name,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance,
                null,
                parameterTypes,
                null);
        }

        /// <summary>
        /// Searches for the specified public method whose parameters match the specified argument types.
        /// </summary>
        /// <param name="type">
        /// The reflected <see cref="Type"/> in which to find the method.
        /// </param>
        /// <param name="name">
        /// The string containing the name of the public method to get.
        /// </param>
        /// <param name="bindingFlags">
        /// A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.
        /// </param>
        /// <param name="binder">Unused.</param>
        /// <param name="parameterTypes">
        /// An array of <see cref="Type"/> objects representing the number, order, and type of the parameters
        /// for the method to get.
        /// </param>
        /// <param name="modifiers">Unused.</param>
        /// <returns>
        /// An object representing the public method whose parameters match the specified argument types, if found;
        /// otherwise, null.
        /// The <see cref="MethodInfo"/> returned represents the generic method declaration.
        /// </returns>
        /// <remarks>
        /// Generic argument types are specified by substituting the types <see cref="Arg1"/>,
        /// <see cref="Generic.Arg2"/>,  <see cref="Generic.Arg3"/> and <see cref="Generic.Arg4"/>,
        /// where Arg1 represents the first generic argument, Arg2 the second, Arg3 the third, etc.
        ///  <example>
        ///   For example, one can obtain the generic method
        ///   using the following call:
        ///   <code>
        ///   <![CDATA[
        ///       // Returns the method definition for
        ///       //  Queryable.Where<TSource>(IQueryable<TSource>, Expression<Func<TSource, bool>>)
        ///       MethodInfo whereMethod = typeof(Queryable).GetGenericMethod(
        ///           "Where",
        ///           new Type[]
        ///               {
        ///                   typeof (IQueryable<Generic.Arg1>),
        ///                   typeof (Expression<Func<Generic.Arg1, bool>>)
        ///               });
        ///   ]]>
        ///   </code>
        ///  </example>
        /// </remarks>
        public static MethodInfo GetGenericMethod(this Type type,
            string name,
            BindingFlags bindingFlags,
            Binder binder,
            Type[] parameterTypes,
            ParameterModifier[] modifiers)
        {
            var methods = type.GetMethods(bindingFlags)
                .Where(m => string.Equals(m.Name, name));

            foreach (MethodInfo method in methods)
            {
                Type[] genericArguments = method.GetGenericArguments();

                var replacedTypes = parameterTypes
                    .Select(t => ReplaceWithGenerics(t, genericArguments))
                    .ToList();


                bool doParametersMatch = method.GetParameters()
                    .Select(p => p.ParameterType)
                    .SequenceEqual(replacedTypes);

                if (doParametersMatch)
                    return method;
            }

            return null;
        }

        #endregion

        #region Private Methods

        private static Type ReplaceWithGenerics(Type t, Type[] genericReplacementTypes)
        {
            if (t == typeof(Arg1))
                return genericReplacementTypes[0];

            if (t == typeof(Arg2))
                return genericReplacementTypes[1];

            if (t == typeof(Arg3))
                return genericReplacementTypes[2];

            if (t == typeof(Arg4))
                return genericReplacementTypes[3];

            if (!t.IsGenericType)
                return t;

            Type[] genericParameters = t.GetGenericArguments()
                .Select(ga => ReplaceWithGenerics(ga, genericReplacementTypes))
                .ToArray();

            return t
                .GetGenericTypeDefinition()
                .MakeGenericType(genericParameters);
        }

        #endregion
    }
}
