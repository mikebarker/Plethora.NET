using System.Drawing;
using System.Globalization;

namespace Plethora.Context.Windows.Forms
{
    public interface IImageKeyConverter
    {
        Image Convert(object imageKey, CultureInfo cultureInfo);
    }
}
