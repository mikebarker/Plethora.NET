using System;
using System.Data;

namespace Plethora.Data
{
    /// <summary>
    /// Provides a test method to determine whether a <see cref="IDbCommand"/> should be re-tried.
    /// </summary>
    public interface IExceptionTester
    {
        /// <summary>
        /// Determines whether a <see cref="IDbCommand"/> should be re-tried.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> which has caused the retry.</param>
        /// <param name="waitTime">Output. The time to wait before re-executing the command.</param>
        /// <returns>True to re-try the command, else false.</returns>
        bool TestForRetry(Exception exception, out TimeSpan waitTime);
    }
}
