using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Plethora.Synchronized.Change
{
    public sealed class ChangeDescriptorApplier : IChangeApplier
    {
        #region Fields

        private readonly object topLevelObject;

        #endregion
        
        #region Constructor

        public ChangeDescriptorApplier(object topLevelObject)
        {
            //Validation
            if (topLevelObject == null)
                throw new ArgumentNullException(nameof(topLevelObject));


            this.topLevelObject = topLevelObject;
        }

        #endregion

        #region Implementation of IChangeDescriptor

        public void Apply(ChangeDescriptor change)
        {
            //Validation
            if (change == null)
                throw new ArgumentNullException(nameof(change));


            Type objType = this.topLevelObject.GetType();
            MemberInfo member = FindMember(objType, change.MemberName, change.Arguments);

            if (member is FieldInfo)
            {
                var field = (FieldInfo)member;

                if (change.Value is ChangeDescriptor)
                {
                    var nextChange = (ChangeDescriptor)change.Value;

                    object fieldValue = field.GetValue(this.topLevelObject);

                    ChangeDescriptorApplier applier = new ChangeDescriptorApplier(fieldValue);
                    applier.Apply(nextChange);
                }
                else
                {
                    field.SetValue(this.topLevelObject, change.Value);
                }
            }
            else if (member is PropertyInfo)
            {
                var property = (PropertyInfo)member;

                if (change.Value is ChangeDescriptor)
                {
                    var nextChange = (ChangeDescriptor)change.Value;

                    object propertyValue = property.GetValue(this.topLevelObject, change.Arguments);

                    ChangeDescriptorApplier applier = new ChangeDescriptorApplier(propertyValue);
                    applier.Apply(nextChange);
                }
                else
                {
                    property.SetValue(this.topLevelObject, change.Value, change.Arguments);
                }
            }
            else if (member is MethodInfo)
            {
                var method = (MethodInfo)member;

                if (change.Value != null)
                    throw new ArgumentException("ChangeDescriptor.Value is not null for expected method call.");

                object result = method.Invoke(this.topLevelObject, change.Arguments);
            }
        }

        #endregion

        #region Private Methods

        private static MemberInfo FindMember(Type type, string propertyName, object[] arguments)
        {
            List<MemberInfo> candidateMembers = new List<MemberInfo>();

            var allMembers = type.GetMembers(BindingFlags.Public | BindingFlags.Instance);
            var namedMembers = allMembers.Where(member => string.Equals(member.Name, propertyName));
            foreach (var member in namedMembers)
            {
                if (member is MethodInfo)
                {
                    var method = (MethodInfo)member;
                    var parameters = method.GetParameters();
                    var parameterTypes = parameters.Select(param => param.ParameterType).ToArray();

                    if (AreArgumentsOfType(arguments, parameterTypes))
                        candidateMembers.Add(member);
                }
                else if (member is PropertyInfo)
                {
                    var property = (PropertyInfo)member;
                    var getMethod = property.GetGetMethod();
                    var parameters = getMethod.GetParameters();
                    var parameterTypes = parameters.Select(param => param.ParameterType).ToArray();

                    if (AreArgumentsOfType(arguments, parameterTypes))
                        candidateMembers.Add(member);
                }
                else if (member is FieldInfo)
                {
                    if ((arguments == null) || (arguments.Length == 0))
                        candidateMembers.Add(member);
                }
            }


            if (candidateMembers.Count == 0)
                throw new InvalidOperationException(string.Format("No properties of type {0} met the criteria for name={1}; arguments=({2})",
                    type.Name,
                    propertyName,
                    FormatArgumentTypes(arguments)));

            if (candidateMembers.Count > 1)
                throw new InvalidOperationException(string.Format("Too many candidates for properties of type {0} with the criteria for name={1}; arguments=({2})",
                    type.Name,
                    propertyName,
                    FormatArgumentTypes(arguments)));


            return candidateMembers[0];
        }

        private static bool AreArgumentsOfType(object[] arguments, Type[] types)
        {
            if ((arguments == null))
                return (types.Length == 0);

            if (arguments.Length != types.Length)
                return false;

            for (int i = 0; i < arguments.Length; i++)
            {
                object argument = arguments[i];
                Type type = types[i];


                if (!IsArgumentOfType(argument, type))
                    return false;
            }
            return true;
        }

        private static bool IsArgumentOfType(object argument, Type type)
        {
            if (argument == null)
                return (type.IsClass);

            //TODO: consider looking for implicit converts
            return type.IsInstanceOfType(argument);
        }

        private static string FormatArgumentTypes(object[] arguments)
        {
            string[] argumentNames = arguments
                .Select(arg => (arg == null) ? "<null>" : arg.GetType().Name)
                .ToArray();

            return string.Join(", ", argumentNames);
        }

        #endregion
    }

}
