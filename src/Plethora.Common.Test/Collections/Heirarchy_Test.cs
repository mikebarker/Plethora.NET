using NUnit.Framework;
using Plethora.Test.ExtensionClasses;
using Plethora.Test.UtilityClasses;

namespace Plethora.Collections.Test
{
    [TestFixture]
    public class Heirarchy_Test
    {
        private HierarchyEx hierarchy;
        private readonly Style styleA = new Style("Times New Roman", null, FontProperty.None);
        private readonly Style styleB = new Style("Arial", 9, FontProperty.None);
        private readonly Style styleC = new Style(null, 16, FontProperty.Bold | FontProperty.Italic);

        [SetUp]
        public void SetUp()
        {
            hierarchy = new HierarchyEx(styleA, styleB, styleC);
        }

        [Test]
        public void ClassBasedLookup()
        {
            //exec
            string fontName = hierarchy.FontName;

            //test
            Assert.AreEqual(styleA.FontName, fontName);
        }

        [Test]
        public void ClassBasedLookup_AllEmpty()
        {
            //setup
            hierarchy = new HierarchyEx(styleC, styleC, styleC);

            //exec
            string fontName = hierarchy.FontName;

            //test
            Assert.IsNull(fontName);
        }

        [Test]
        public void NullableStructBasedLookup()
        {
            //exec
            int? fontSize = hierarchy.FontSize;

            //test
            Assert.AreEqual(styleB.FontSize, fontSize);
        }

        [Test]
        public void NullableStructBasedLookup_AllEmpty()
        {
            //setup
            hierarchy = new HierarchyEx(styleA, styleA, styleA);

            //exec
            int? fontSize = hierarchy.FontSize;

            //test
            Assert.IsTrue(fontSize == null);
        }

        [Test]
        public void StructBasedLookup()
        {
            //exec
            FontProperty fontProperty = hierarchy.FontProperty;

            //test
            Assert.AreEqual(styleC.FontProperty, fontProperty);
        }

        [Test]
        public void StructBasedLookup_AllEmpty()
        {
            //setup
            hierarchy = new HierarchyEx(styleA, styleB, styleA);

            //exec
            FontProperty fontProperty = hierarchy.FontProperty;

            //test
            Assert.AreEqual(FontProperty.None, fontProperty);
        }


    }
}
