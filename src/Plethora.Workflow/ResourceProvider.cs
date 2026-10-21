using System.Globalization;

using Plethora.Workflow.Properties;

namespace Plethora.Workflow;

/// <summary>
/// Class which provides access to the standard resources with substitutions made.
/// </summary>
public static class ResourceProvider
{
    #region Public Static Methods

    /// <summary>
    /// Returns the resource string 'LockNotHeld' with substitutions made.
    /// </summary>
    public static string LockNotHeld()
    {
        return Resources.LockNotHeld;
    }

    /// <summary>
    /// Returns the resource string 'LockAlreadyAcquired'.
    /// </summary>
    public static string LockAlreadyAcquired()
    {
        return Resources.LockAlreadyAcquired;
    }

    /// <summary>
    /// Returns the resource string 'AcquiredTimeMustNotBeInFuture'.
    /// </summary>
    public static string AcquiredTimeMustNotBeInFuture()
    {
        return Resources.AcquiredTimeMustNotBeInFuture;
    }

    /// <summary>
    /// Returns the resource string 'TimeoutMustBeAfterAcquired'.
    /// </summary>
    public static string TimeoutMustBeAfterAcquired()
    {
        return Resources.TimeoutMustBeAfterAcquired;
    }

    /// <summary>
    /// Returns the resource string 'NoProcessors'.
    /// </summary>
    public static string NoProcessors()
    {
        return Resources.NoProcessors;
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
    private static string StringFormat(string format, params object[] args)
    {
        return string.Format(CultureInfo.CurrentCulture, format, args);
    }
    #endregion
}