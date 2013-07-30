using System.Drawing;

namespace Plethora.Context
{
    public interface IUiContextActionTemplate : IContextActionTemplate
    {
        string GetActionDescription(ContextInfo context);

        Image GetImage(ContextInfo context);

        string GetGroup(ContextInfo context);

        int GetRank(ContextInfo context);
    }

    public interface IUiMultiContextActionTemplate : IMultiContextActionTemplate
    {
        string GetActionDescription(ContextInfo[] context);

        Image GetImage(ContextInfo[] context);

        string GetGroup(ContextInfo[] context);

        int GetRank(ContextInfo[] context);
    }
}
