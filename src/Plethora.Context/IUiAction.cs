using System.Drawing;

namespace Plethora.Context
{
    public interface IUiAction : IAction
    {
        string ActionDescription { get; }

        Image Image { get; }

        string Group { get; }

        int Rank { get; }
    }
}
