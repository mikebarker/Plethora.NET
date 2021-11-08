using System;
using System.Globalization;
using System.Text;

using JetBrains.Annotations;

using Plethora.Properties;

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
        [NotNull]
        public static string AlreadyDisposed()
        {
            return Resources.AlreadyDisposed;
        }

        /// <summary>
        /// Returns the resource string 'ArgAddingDuplicate'.
        /// </summary>
        [NotNull]
        public static string ArgAddingDuplicate()
        {
            return Resources.ArgAddingDuplicate;
        }

        /// <summary>
        /// Returns the resource string 'ArgArrayInvalidType'.
        /// </summary>
        [NotNull]
        public static string ArgArrayInvalidType()
        {
            return Resources.ArgArrayInvalidType;
        }

        /// <summary>
        /// Returns the resource string 'ArgArrayMultiDimensionNotSupported'.
        /// </summary>
        [NotNull]
        public static string ArgArrayMultiDimensionNotSupported()
        {
            return Resources.ArgArrayMultiDimensionNotSupported;
        }

        /// <summary>
        /// Returns the resource string 'ArgArrayNonZeroLowerBound'.
        /// </summary>
        [NotNull]
        public static string ArgArrayNonZeroLowerBound()
        {
            return Resources.ArgArrayNonZeroLowerBound;
        }

        /// <summary>
        /// Returns the resource string 'ArgInvalid' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the invalid argument.</param>
        [NotNull]
        public static string ArgInvalid([InvokerParameterName, NotNull] string arg)
        {
            return StringFormat(Resources.ArgInvalid, arg);
        }

        /// <summary>
        /// Returns the resource string 'ArgInvalidOffsetLength' with substitutions made.
        /// </summary>
        /// <param name="offsetArg">The name of the offset argument.</param>
        /// <param name="lengthArg">The name of the length argument.</param>
        [NotNull]
        public static string ArgInvalidOffsetLength([InvokerParameterName, NotNull] string offsetArg, [InvokerParameterName, NotNull] string lengthArg)
        {
            return StringFormat(Resources.ArgInvalidOffsetLength, offsetArg, lengthArg);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="value">The value which the argument must be greater than.</param>
        [NotNull]
        public static string ArgMustBeGreaterThan([InvokerParameterName, NotNull] string arg, int value)
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
        [NotNull]
        public static string ArgMustBeGreaterThan([InvokerParameterName, NotNull] string arg, double value)
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
        [NotNull]
        public static string ArgMustBeGreaterThan([InvokerParameterName, NotNull] string arg, decimal value)
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
        [NotNull]
        public static string ArgMustBeGreaterThan([InvokerParameterName, NotNull] string arg, [NotNull] string value)
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
        [NotNull]
        public static string ArgMustNotBe([InvokerParameterName, NotNull] string arg, [NotNull] string value)
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
        [NotNull]
        public static string ArgMustBeGreaterThanEqualTo([InvokerParameterName, NotNull] string arg, int value)
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
        [NotNull]
        public static string ArgMustBeGreaterThanEqualTo([InvokerParameterName, NotNull] string arg, double value)
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
        [NotNull]
        public static string ArgMustBeGreaterThanEqualTo([InvokerParameterName, NotNull] string arg, decimal value)
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
        [NotNull]
        public static string ArgMustBeGreaterThanEqualTo([InvokerParameterName, NotNull] string arg, [NotNull] string value)
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
        [NotNull]
        public static string ArgMustBeLessThan([InvokerParameterName, NotNull] string arg, int value)
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
        [NotNull]
        public static string ArgMustBeLessThan([InvokerParameterName, NotNull] string arg, double value)
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
        [NotNull]
        public static string ArgMustBeLessThan([InvokerParameterName, NotNull] string arg, decimal value)
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
        [NotNull]
        public static string ArgMustBeLessThan([InvokerParameterName, NotNull] string arg, [NotNull] string value)
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
        [NotNull]
        public static string ArgMustBeLessThanEqualTo([InvokerParameterName, NotNull] string arg, int value)
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
        [NotNull]
        public static string ArgMustBeLessThanEqualTo([InvokerParameterName, NotNull] string arg, double value)
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
        [NotNull]
        public static string ArgMustBeLessThanEqualTo([InvokerParameterName, NotNull] string arg, decimal value)
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
        [NotNull]
        public static string ArgMustBeLessThanEqualTo([InvokerParameterName, NotNull] string arg, [NotNull] string value)
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
        [NotNull]
        public static string ArgMustBeBetween([InvokerParameterName, NotNull] string arg, int min, int max)
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
        [NotNull]
        public static string ArgMustBeBetween([InvokerParameterName, NotNull] string arg, double min, double max)
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
        [NotNull]
        public static string ArgMustBeBetween([InvokerParameterName, NotNull] string arg, decimal min, decimal max)
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
        [NotNull]
        public static string ArgMustBeBetween([InvokerParameterName, NotNull] string arg, [NotNull] string min, [NotNull] string max)
        {
            return StringFormat(Resources.ArgMustBeBetween, arg, min, max);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        [NotNull]
        public static string ArgMustNotBeZero([InvokerParameterName, NotNull] string arg)
        {
            return ArgMustNotBe(arg, Resources.Zero);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        [NotNull]
        public static string ArgMustBeGreaterThanZero([InvokerParameterName, NotNull] string arg)
        {
            return ArgMustBeGreaterThan(arg, Resources.Zero);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        [NotNull]
        public static string ArgMustBeGreaterThanEqualToZero([InvokerParameterName, NotNull] string arg)
        {
            return ArgMustBeGreaterThanEqualTo(arg, Resources.Zero);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeOfType' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="type">The required type of the argument.</param>
        [NotNull]
        public static string ArgMustBeOfType([InvokerParameterName, NotNull] string arg, [NotNull] Type type)
        {
            return StringFormat(Resources.ArgMustBeOfType, arg, type.Name);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeOneOf' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        /// <param name="values">The valid values allowed for the argument.</param>
        [NotNull]
        public static string ArgMustBeOneOf([InvokerParameterName, NotNull] string arg, [NotNull] params object[] values)
        {
            StringBuilder sb = new StringBuilder();
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
        [NotNull]
        public static string ArgPropertyInvalid([InvokerParameterName, NotNull] string arg, [NotNull] string property)
        {
            return StringFormat(Resources.ArgPropertyInvalid, arg, property);
        }

        /// <summary>
        /// Returns the resource string 'ArgStringEmpty' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        [NotNull]
        public static string ArgStringEmpty([InvokerParameterName, NotNull] string arg)
        {
            return StringFormat(Resources.ArgStringEmpty, arg);
        }

        /// <summary>
        /// Returns the resource string 'ArgTimeout' with substitutions made.
        /// </summary>
        /// <param name="arg">The name of the argument.</param>
        [NotNull]
        public static string ArgTimeout([InvokerParameterName, NotNull] string arg)
        {
            return StringFormat(Resources.ArgTimeout, arg, Resources.Zero, "Timeout.Infinite");
        }

        /// <summary>
        /// Returns the resource string 'AtLeastOneDayOfWeek' with substitutions made.
        /// </summary>
        [NotNull]
        public static string AtLeastOneDayOfWeek()
        {
            return Resources.AtLeastOneDayOfWeek;
        }

        /// <summary>
        /// Returns the resource string 'AtLeastOneDateOrEom' with substitutions made.
        /// </summary>
        [NotNull]
        public static string AtLeastOneDateOrEom()
        {
            return Resources.AtLeastOneDateOrEom;
        }

        /// <summary>
        /// Returns the resource string 'BadEnum' with substitutions made.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The invalid value for the enum.</param>
        [NotNull]
        public static string BadEnum<T>(T value)
        {
            return StringFormat(Resources.BadEnum, typeof(T).FullName, value);
        }

        /// <summary>
        /// Returns the resource string 'CollectionReadonly' with substitutions made.
        /// </summary>
        [NotNull]
        public static string CollectionReadonly()
        {
            return StringFormat(Resources.CollectionReadonly);
        }

        /// <summary>
        /// Returns the resource string 'GenericArgumentsMismatch' with substitutions made.
        /// </summary>
        [NotNull]
        public static string GenericArgumentsMismatch()
        {
            return Resources.GenericArgumentsMismatch;
        }

        /// <summary>
        /// Returns the resource string 'GenericArgMustBeDelegate' with substitutions made.
        /// </summary>
        [NotNull]
        public static string GenericArgMustBeDelegate()
        {
            return Resources.GenericArgMustBeDelegate;
        }

        /// <summary>
        /// Returns the resource string 'GenericArgMustBeIComparable' with substitutions made.
        /// </summary>
        [NotNull]
        public static string GenericArgMustBeIComparable()
        {
            return Resources.GenericArgMustBeIComparable;
        }

        /// <summary>
        /// Returns the resource string 'InvalidCast' with substitutions made.
        /// </summary>
        [NotNull]
        public static string InvalidCast()
        {
            return Resources.InvalidCast;
        }

        /// <summary>
        /// Returns the resource string 'InvalidState'.
        /// </summary>
        [NotNull]
        public static string InvalidState()
        {
            return Resources.InvalidState;
        }

        /// <summary>
        /// Returns the resource string 'MethodNotFound'.
        /// </summary>
        [NotNull]
        public static string MethodNotFound()
        {
            return Resources.MethodNotFound;
        }

        /// <summary>
        /// Returns the resource string 'ParameterTypeNotInGenericList'.
        /// </summary>
        [NotNull]
        public static string ParameterTypeNotInGenericList()
        {
            return Resources.ParameterTypeNotInGenericList;
        }

        /// <summary>
        /// Returns the resource string 'StaticOnlyMirror'.
        /// </summary>
        [NotNull]
        public static string StaticOnlyMirror()
        {
            return Resources.StaticOnlyMirror;
        }

        /// <summary>
        /// Returns the resource string 'TimeoutInvalid'.
        /// </summary>
        [NotNull]
        public static string TimeoutInvalid()
        {
            return Resources.TimeoutInvalid;
        }

        /// <summary>
        /// Returns the resource string 'TypeNotFoundInAssembly' with substitutions made.
        /// </summary>
        /// <param name="type">The type name not found.</param>
        /// <param name="assembly">The assembly name in which the type was not found.</param>
        [NotNull]
        public static string TypeNotFoundInAssembly([NotNull] string type, [NotNull] string assembly)
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
        [StringFormatMethod("format")]
        [NotNull]
        private static string StringFormat([NotNull] string format, [NotNull] params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }
        #endregion
    }
}