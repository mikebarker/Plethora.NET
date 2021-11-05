using System.Security.Permissions;
using Plethora.ComponentModel;

namespace Plethora.Windows.Forms.Styles
{
    public partial class TextBoxStyliser
    {
        [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
        public class Converter : DefaultReferenceConverter
        {
            public Converter()
                : base(typeof(TextBoxStyliser), "Default", false)
            {
            }
        }
    }
}
