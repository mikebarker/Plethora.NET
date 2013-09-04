using System.Drawing;

namespace Plethora.Context
{
    /// <summary>
    /// Extends the <see cref="IAction"/> interface to include visual elements.
    /// </summary>
    public interface IUiAction : IAction
    {
        string ActionDescription { get; }

        Image Image { get; }

        string Group { get; }

        int Rank { get; }
    }
}
