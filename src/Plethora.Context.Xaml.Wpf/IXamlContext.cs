using JetBrains.Annotations;

namespace Plethora.Context
{
    public interface IXamlContext
    {
        string ContextName { get; }

        int Rank { get; }

        [CanBeNull]
        object Data { get; }
    }
}
