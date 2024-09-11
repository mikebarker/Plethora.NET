using Plethora.Properties;
using System;
using System.Globalization;
using System.Text;

namespace Plethora
{
    /// <summary>
    /// Class which provides access to the standard resources with substitutions made.
    /// </summary>
    public static class ResourceProvider
    {
        #region Public Static Methods

        /// <summary>
        /// Returns the resource string 'AlreadyDisposed'.
        /// </summary>
        public static string AlreadyDisposed()
        {
            return Resources.AlreadyDisposed;
        }

        /// <summary>
        /// Returns the resource string 'ArgAddingDuplicate'.
        /// </summary>
        public static string ArgAddingDuplicate()
        {
            return Resources.ArgAddingDuplicate;
        }

        /// <summary>
        /// Returns the resource string 'ArgArrayInvalidType'.
        /// </summary>
        public static string ArgArrayInvalidType()
        {
            return Resources.ArgArrayInvalidType;
        }

        /// <summary>
        /// Returns the resource string 'ArgArrayMultiDimensionNotSupported'.
        /// </summary>
        public static string ArgArrayMultiDimensionNotSupported()
        {
            return Resources.ArgArrayMultiDimensionNotSupported;
        }

        /// <summary>
        /// Returns the resource string 'ArgArrayNonZeroLowerBound'.
        /// </summary>
        public static string ArgArrayNonZeroLowerBound()
        {
            return Resources.ArgArrayNonZeroLowerBound;
        }

        /// <summary>
        /// Returns the resource string 'ArgInvalid' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the invalid argument.</param>
        public static string ArgInvalid(string arg)
        {
            return StringFormat(Resources.ArgInvalid, arg);
        }

        /// <summary>
        /// Returns the resource string 'ArgInvalidOffsetLength' with substitutions made.
        /// </summary>
        /// <param name="offsetArg">The name of the offset argument.</param>
        /// <param name="lengthArg">The name of the length argument.</param>
        public static string ArgInvalidOffsetLength(string offsetArg, string lengthArg)
        {
            return StringFormat(Resources.ArgInvalidOffsetLength, offsetArg, lengthArg);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be greater than.</param>
        public static string ArgMustBeGreaterThan(string arg, int value)
        {
            return ArgMustBeGreaterThan(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be greater than.</param>
        public static string ArgMustBeGreaterThan(string arg, double value)
        {
            return ArgMustBeGreaterThan(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be greater than.</param>
        public static string ArgMustBeGreaterThan(string arg, decimal value)
        {
            return ArgMustBeGreaterThan(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be greater than.</param>
        public static string ArgMustBeGreaterThan(string arg, string value)
        {
            return StringFormat(Resources.ArgMustBeGreaterThan,
                                arg,
                                value);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustNotBe' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be greater than.</param>
        public static string ArgMustNotBe(string arg, string value)
        {
            return StringFormat(Resources.ArgMustNotBe,
                                arg,
                                value);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThanEqualTo' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be greater than, or equal to.</param>
        public static string ArgMustBeGreaterThanEqualTo(string arg, int value)
        {
            return ArgMustBeGreaterThanEqualTo(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThanEqualTo' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be greater than, or equal to.</param>
        public static string ArgMustBeGreaterThanEqualTo(string arg, double value)
        {
            return ArgMustBeGreaterThanEqualTo(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThanEqualTo' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be greater than, or equal to.</param>
        public static string ArgMustBeGreaterThanEqualTo(string arg, decimal value)
        {
            return ArgMustBeGreaterThanEqualTo(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThanEqualTo' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be greater than, or equal to.</param>
        public static string ArgMustBeGreaterThanEqualTo(string arg, string value)
        {
            return StringFormat(Resources.ArgMustBeGreaterThanEqualTo,
                                arg,
                                value);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeLessThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be less than.</param>
        public static string ArgMustBeLessThan(string arg, int value)
        {
            return ArgMustBeLessThan(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeLessThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be less than.</param>
        public static string ArgMustBeLessThan(string arg, double value)
        {
            return ArgMustBeLessThan(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeLessThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be less than.</param>
        public static string ArgMustBeLessThan(string arg, decimal value)
        {
            return ArgMustBeLessThan(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeLessThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be less than.</param>
        public static string ArgMustBeLessThan(string arg, string value)
        {
            return StringFormat(Resources.ArgMustBeLessThan,
                                arg,
                                value);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeLessThanEqualTo' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be less than, or equal to.</param>
        public static string ArgMustBeLessThanEqualTo(string arg, int value)
        {
            return ArgMustBeLessThanEqualTo(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeLessThanEqualTo' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be less than, or equal to.</param>
        public static string ArgMustBeLessThanEqualTo(string arg, double value)
        {
            return ArgMustBeLessThanEqualTo(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeLessThanEqualTo' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be less than, or equal to.</param>
        public static string ArgMustBeLessThanEqualTo(string arg, decimal value)
        {
            return ArgMustBeLessThanEqualTo(
                arg,
                value.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeLessThanEqualTo' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be less than, or equal to.</param>
        public static string ArgMustBeLessThanEqualTo(string arg, string value)
        {
            return StringFormat(Resources.ArgMustBeLessThanEqualTo,
                                arg,
                                value);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeBetween' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="min">The minimum value of the argument.</param>
        /// <param name="max">The maximum value of the argument.</param>
        public static string ArgMustBeBetween(string arg, int min, int max)
        {
            return ArgMustBeBetween(arg,
                                    min.ToString(CultureInfo.CurrentCulture),
                                    max.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeBetween' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="min">The minimum value of the argument.</param>
        /// <param name="max">The maximum value of the argument.</param>
        public static string ArgMustBeBetween(string arg, double min, double max)
        {
            return ArgMustBeBetween(arg,
                                    min.ToString(CultureInfo.CurrentCulture),
                                    max.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeBetween' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="min">The minimum value of the argument.</param>
        /// <param name="max">The maximum value of the argument.</param>
        public static string ArgMustBeBetween(string arg, decimal min, decimal max)
        {
            return ArgMustBeBetween(arg,
                                    min.ToString(CultureInfo.CurrentCulture),
                                    max.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeBetween' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="min">The minimum value of the argument.</param>
        /// <param name="max">The maximum value of the argument.</param>
        public static string ArgMustBeBetween(string arg, string min, string max)
        {
            return StringFormat(Resources.ArgMustBeBetween, arg, min, max);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        public static string ArgMustNotBeZero(string arg)
        {
            return ArgMustNotBe(arg, Resources.Zero);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        public static string ArgMustBeGreaterThanZero(string arg)
        {
            return ArgMustBeGreaterThan(arg, Resources.Zero);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        public static string ArgMustBeGreaterThanEqualToZero(string arg)
        {
            return ArgMustBeGreaterThanEqualTo(arg, Resources.Zero);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeOfType' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="type">The required type of the argument.</param>
        public static string ArgMustBeOfType(string arg, Type type)
        {
            return StringFormat(Resources.ArgMustBeOfType, arg, type.Name);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeOneOf' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="values">The valid values allowed for the argument.</param>
        public static string ArgMustBeOneOf(string arg, params object[] values)
        {
            StringBuilder sb = new();
            bool isFirst = true;
            foreach (var value in values)
            {
                if (isFirst)
                    isFirst = false;
                else
                    sb.Append(", ");

                sb.Append(value);
            }

            return StringFormat(Resources.ArgMustBeOneOf, arg, sb.ToString());
        }

        /// <summary>
        /// Returns the resource string 'ArgPropertyInvalid' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="property">
        /// The name of the property of the argument which was invalid.
        /// </param>
        public static string ArgPropertyInvalid(string arg, string property)
        {
            return StringFormat(Resources.ArgPropertyInvalid, arg, property);
        }

        /// <summary>
        /// Returns the resource string 'ArgStringEmpty' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        public static string ArgStringEmpty(string arg)
        {
            return StringFormat(Resources.ArgStringEmpty, arg);
        }

        /// <summary>
        /// Returns the resource string 'ArgTimeout' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        public static string ArgTimeout(string arg)
        {
            return StringFormat(Resources.ArgTimeout, arg, Resources.Zero, "Timeout.Infinite");
        }

        /// <summary>
        /// Returns the resource string 'AtLeastOneDayOfWeek' with substitutions made.
        /// </summary>
        public static string AtLeastOneDayOfWeek()
        {
            return Resources.AtLeastOneDayOfWeek;
        }

        /// <summary>
        /// Returns the resource string 'AtLeastOneDateOrEom' with substitutions made.
        /// </summary>
        public static string AtLeastOneDateOrEom()
        {
            return Resources.AtLeastOneDateOrEom;
        }

        /// <summary>
        /// Returns the resource string 'BadEnum' with substitutions made.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The invalid value for the enum.</param>
        public static string BadEnum<T>(T value)
        {
            return StringFormat(Resources.BadEnum, typeof(T).FullName, value);
        }

        /// <summary>
        /// Returns the resource string 'CollectionReadOnly' with substitutions made.
        /// </summary>
        public static string CollectionReadOnly()
        {
            return StringFormat(Resources.CollectionReadOnly);
        }

        /// <summary>
        /// Returns the resource string 'EnumeratorOpCantHappen'.
        /// </summary>
        public static string EnumeratorOpCantHappen()
        {
            return Resources.EnumeratorOpCantHappen;
        }

        /// <summary>
        /// Returns the resource string 'ElementWithSameKeyExists' with substitutions made.
        /// </summary>
        /// <param name="type">The type of the collection.</param>
        public static string ElementWithSameKeyExists(Type type)
        {
            return StringFormat(Resources.ElementWithSameKeyExists, type.Name);
        }

        /// <summary>
        /// Returns the resource string 'ExpressionMustBeField'.
        /// </summary>
        public static string ExpressionMustBeField()
        {
            return Resources.ExpressionMustBeField;
        }

        /// <summary>
        /// Returns the resource string 'ExpressionMustBeProperty'.
        /// </summary>
        public static string ExpressionMustBeProperty()
        {
            return Resources.ExpressionMustBeProperty;
        }

        /// <summary>
        /// Returns the resource string 'ExpressionMustBePropertyOrField'.
        /// </summary>
        public static string ExpressionMustBePropertyOrField()
        {
            return Resources.ExpressionMustBePropertyOrField;
        }

        /// <summary>
        /// Returns the resource string 'GenericArgumentsMismatch' with substitutions made.
        /// </summary>
        public static string GenericArgumentsMismatch()
        {
            return Resources.GenericArgumentsMismatch;
        }

        /// <summary>
        /// Returns the resource string 'GenericArgMustBeDelegate' with substitutions made.
        /// </summary>
        public static string GenericArgMustBeDelegate()
        {
            return Resources.GenericArgMustBeDelegate;
        }

        /// <summary>
        /// Returns the resource string 'GenericArgMustBeIComparable' with substitutions made.
        /// </summary>
        public static string GenericArgMustBeIComparable()
        {
            return Resources.GenericArgMustBeIComparable;
        }

        /// <summary>
        /// Returns the resource string 'InvalidCast' with substitutions made.
        /// </summary>
        public static string InvalidCast()
        {
            return Resources.InvalidCast;
        }

        /// <summary>
        /// Returns the resource string 'InvalidState'.
        /// </summary>
        public static string InvalidState()
        {
            return Resources.InvalidState;
        }

        /// <summary>
        /// Returns the resource string 'KeysDoNotMatch' with substitutions made.
        /// </summary>
        /// <param name="type">The type of the collection.</param>
        public static string KeysDoNotMatch(Type type)
        {
            return StringFormat(Resources.KeysDoNotMatch, type.Name);
        }

        /// <summary>
        /// Returns the resource string 'MethodNotFound'.
        /// </summary>
        public static string MethodNotFound()
        {
            return Resources.MethodNotFound;
        }

        /// <summary>
        /// Returns the resource string 'ParameterTypeNotInGenericList'.
        /// </summary>
        public static string ParameterTypeNotInGenericList()
        {
            return Resources.ParameterTypeNotInGenericList;
        }

        /// <summary>
        /// Returns the resource string 'StaticOnlyMirror'.
        /// </summary>
        public static string StaticOnlyMirror()
        {
            return Resources.StaticOnlyMirror;
        }

        /// <summary>
        /// Returns the resource string 'TimeoutInvalid'.
        /// </summary>
        public static string TimeoutInvalid()
        {
            return Resources.TimeoutInvalid;
        }

        /// <summary>
        /// Returns the resource string 'TypeNotFoundInAssembly' with substitutions made.
        /// </summary>
        /// <param name="type">The type name not found.</param>
        /// <param name="assembly">The assembly name in which the type was not found.</param>
        public static string TypeNotFoundInAssembly(string type, string assembly)
        {
            return StringFormat(Resources.TypeNotFoundInAssembly, type, assembly);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the format string with substitutions made, according to the current culture.
        /// </summary>
        /// <param name="format">
        /// A string containing zero or more format items.
        /// </param>
        /// <param name="args">
        /// An object array containing zero or more objects to format.
        /// </param>
        /// <returns>
        /// The format string with substitutions made, according to the current UI
        /// culture.
        /// </returns>
        private static string StringFormat(string format, params object?[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }
        #endregion
    }
}