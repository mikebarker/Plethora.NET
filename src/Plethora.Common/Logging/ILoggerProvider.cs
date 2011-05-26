using System;

namespace Plethora.Logging
{
    /// <summary>
    /// Interface for providing an <see cref="ILogger"/>.
    /// </summary>
    public interface ILoggerProvider
    {
        ILogger GetLogger(string name);

        ILogger GetLogger(Type type);
    }
}
