using System.Security.Permissions;
using Plethora.ComponentModel;

namespace Plethora.Windows.Forms.Styles
{
    public partial class NullableNumericTextBoxStyliser
    {
        [HostProtection(SecurityAction.LinkDemand, SharedState = true)]
        public new class Converter : DefaultReferenceConverter
        {
            public Converter()
                : base(typeof(NullableNumericTextBoxStyliser), "Default", true)
            {
            }
        }
    }
}
