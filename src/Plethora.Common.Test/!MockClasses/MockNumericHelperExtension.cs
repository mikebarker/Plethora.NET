using System.Globalization;
using Plethora.Globalization;

namespace Plethora.Test.MockClasses
{
    class MockCultureExtension : CultureExtensionBase
    {
        #region Constructors

        public MockCultureExtension(CultureInfo culture)
            : base(culture)
        {
        }

        public MockCultureExtension()
            : base(CultureInfo.InvariantCulture)
        {
        }
        #endregion

        #region ICultureExtension Members

        public override string GetOrdinalSuffix(int number)
        {
            return "a";
        }
        #endregion
    }
}
