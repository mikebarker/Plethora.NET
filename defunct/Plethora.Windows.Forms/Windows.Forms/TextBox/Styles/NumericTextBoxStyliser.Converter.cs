using System.Security.Permissions;
using Plethora.ComponentModel;

namespace Plethora.Windows.Forms.Styles
{
    public partial class NumericTextBoxStyliser
    {
        [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
        public new class Converter : DefaultReferenceConverter
        {
            public Converter()
                : base(typeof(NumericTextBoxStyliser), "Default", true)
            {
            }
        }
    }
}
