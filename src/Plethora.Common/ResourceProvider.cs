using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Reflection;
using Plethora.Properties;

namespace Plethora
{
    /// <summary>
    /// Class which provides access to the standard resources with correct
    /// substitutions made.
    /// </summary>
    public static class ResourceProvider
    {
        #region Public Static Methods

        /// <summary>
        /// Returns the resource string 'ArgDimensionsMustMatch' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg0">The first argument of the string.</param>
        /// <param name="arg1">The second argument of the string.</param>
        /// <returns>
        /// The 'ArgDimensionsMustMatch' string with substitutions made.
        /// </returns>
        public static string ArgDimensionsMustMatch(string arg0, string arg1)
        {
            return StringFormat(Resources.ArgDimensionsMustMatch, arg0, arg1);
        }

        /// <summary>
        /// Returns the resource string 'ArgInvalid' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg">The argument of the string.</param>
        /// <returns>
        /// The 'ArgInvalid' string with substitutions made.
        /// </returns>
        public static string ArgInvalid(string arg)
        {
            return StringFormat(Resources.ArgInvalid, arg);
        }

        /// <summary>
        /// Returns the resource string 'ArgIsNotPublicProperty' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg0">The first argument of the string.</param>
        /// <param name="arg1">The second argument of the string.</param>
        /// <returns>
        /// The 'ArgIsNotPublicProperty' string with substitutions made.
        /// </returns>
        public static string ArgIsNotPublicProperty(string arg0, string arg1)
        {
            return StringFormat(Resources.ArgIsNotPublicProperty, arg0, arg1);
        }

        /// <summary>
        /// Returns the resource string 'ArgIsNotSubclass' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg0">The first argument of the string.</param>
        /// <param name="arg1">The second argument of the string.</param>
        /// <returns>
        /// The 'ArgIsNotSubclass' string with substitutions made.
        /// </returns>
        public static string ArgIsNotSubclass(string arg0, string arg1)
        {
            return StringFormat(Resources.ArgIsNotSubclass, arg0, arg1);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg">The argument of the string.</param>
        /// <param name="value">The value of the string.</param>
        /// <returns>
        /// The 'ArgMustBeGreaterThan' string with substitutions made.
        /// </returns>
        public static string ArgMustBeGreaterThan(string arg, string value)
        {
            return StringFormat(Resources.ArgMustBeGreaterThan,
                                arg,
                                value);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeLessThan' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg">The argument of the string.</param>
        /// <param name="value">The value of the string.</param>
        /// <returns>
        /// The 'ArgMustBeLessThan' string with substitutions made.
        /// </returns>
        public static string ArgMustBeLessThan(string arg, string value)
        {
            return StringFormat(Resources.ArgMustBeLessThan,
                                arg,
                                value);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeBetween' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg">The argument of the string.</param>
        /// <param name="min">The minimum value of the argument.</param>
        /// <param name="max">The maximum value of the argument.</param>
        /// <returns>
        /// The 'ArgMustBeBetween' string with substitutions made.
        /// </returns>
        public static string ArgMustBeBetween(string arg, int min, int max)
        {
            return ArgMustBeBetween(arg,
                                    min.ToString(CultureInfo.CurrentCulture),
                                    max.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeBetween' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg">The argument of the string.</param>
        /// <param name="min">The minimum value of the argument.</param>
        /// <param name="max">The maximum value of the argument.</param>
        /// <returns>
        /// The 'ArgMustBeBetween' string with substitutions made.
        /// </returns>
        public static string ArgMustBeBetween(string arg, double min, double max)
        {
            return ArgMustBeBetween(arg,
                                    min.ToString(CultureInfo.CurrentCulture),
                                    max.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeBetween' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg">The argument of the string.</param>
        /// <param name="min">The minimum value of the argument.</param>
        /// <param name="max">The maximum value of the argument.</param>
        /// <returns>
        /// The 'ArgMustBeBetween' string with substitutions made.
        /// </returns>
        public static string ArgMustBeBetween(string arg, string min, string max)
        {
            return StringFormat(Resources.ArgMustBeBetween, arg, min, max);
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg">The argument of the string.</param>
        /// <returns>
        /// The 'ArgMustBeGreaterThan' string with substitutions made.
        /// </returns>
        public static string ArgMustBeGreaterThanZero(string arg)
        {
            return ArgMustBeGreaterThan(arg, Resources.Zero);
        }

        /// <summary>
        /// Returns the resource string 'ArgPropertyInvalid' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg">The argument of the string.</param>
        /// <param name="property">
        /// The property of the argument which was invalid.
        /// </param>
        /// <returns>
        /// The 'ArgPropertyInvalid' string with substitutions made.
        /// </returns>
        public static string ArgPropertyInvalid(string arg, string property)
        {
            return StringFormat(Resources.ArgPropertyInvalid, arg, property);
        }

        /// <summary>
        /// Returns the resource string 'ArgStringEmpty' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg">The argument of the string.</param>
        /// <returns>
        /// The 'ArgStringEmpty' string with substitutions made.
        /// </returns>
        public static string ArgStringEmpty(string arg)
        {
            return StringFormat(Resources.ArgStringEmpty, arg);
        }

        /// <summary>
        /// Returns the resource string 'BadEnum' with
        /// substitutions made.
        /// </summary>
        /// <typeparam name="T">The type of the enum.</typeparam>
        /// <param name="value">The invalid value for the enum.</param>
        /// <returns>
        /// The 'BadEnum' string with substitutions made.
        /// </returns>
        public static string BadEnum<T>(T value)
        {
            return StringFormat(Resources.BadEnum, typeof(T).FullName, value);
        }

        /// <summary>
        /// Returns the resource string 'InvalidState' with
        /// substitutions made.
        /// </summary>
        /// <returns>
        /// The 'InvalidState' string with substitutions made.
        /// </returns>
        public static string InvalidState()
        {
            return Resources.InvalidState;
        }

        /// <summary>
        /// Returns the resource string 'IsNotSupported' with
        /// substitutions made.
        /// </summary>
        /// <param name="arg0">The argument of the string.</param>
        /// <returns>
        /// The 'IsNotSupported' string with substitutions made.
        /// </returns>
        public static string MethodIsNotSupported(string arg0)
        {
            return StringFormat(Resources.NotSupported,
              arg0);
        }

        /// <summary>
        /// Returns the resource string 'IsNotSupported' with
        /// substitutions made.
        /// </summary>
        /// <returns>
        /// The 'IsNotSupported' string with substitutions made.
        /// </returns>
        public static string MethodIsNotSupported()
        {
            StackTrace stackTrace = new StackTrace(1, false);
            StackFrame frame = stackTrace.GetFrame(0);

            return MethodIsNotSupported(MethodFormat(frame.GetMethod()));
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Formats a MethodBase a human readble string.
        /// </summary>
        /// <param name="method">The method to be formatted.</param>
        /// <returns>
        /// A string containing the human readble string representation of the
        /// method.
        /// </returns>
        private static string MethodFormat(MethodBase method)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(method.Name);
            sb.Append("(");

            bool firstParameter = true;
            ParameterInfo[] parameters = method.GetParameters();
            foreach (ParameterInfo parameter in parameters)
            {
                if (firstParameter)
                    firstParameter = false;
                else
                    sb.Append(", ");

                sb.Append(parameter.ParameterType.Name);
            }
            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// Returns the resource string 'ArgMustBeGreaterThan' with
        /// substitutions made, according to the current UI culture.
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
        private static string StringFormat(string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentUICulture, format, args);
        }
        #endregion
    }
}