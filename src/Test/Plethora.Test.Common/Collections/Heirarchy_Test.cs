using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Test.ExtensionClasses;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Collections
{
    [TestClass]
    public class Heirarchy_Test
    {
        private readonly Style styleA = new Style("Times New Roman", null, FontProperty.None);
        private readonly Style styleB = new Style("Arial", 9, FontProperty.None);
        private readonly Style styleC = new Style(null, 16, FontProperty.Bold | FontProperty.Italic);
        private HierarchyEx hierarchy;

        public Heirarchy_Test()
        {
            hierarchy = new HierarchyEx(styleA, styleB, styleC);
        }

        [TestMethod]
        public void ClassBasedLookup()
        {
            // Action
            string fontName = hierarchy.FontName;

            // Assert
            Assert.AreEqual(styleA.FontName, fontName);
        }

        [TestMethod]
        public void ClassBasedLookup_AllEmpty()
        {
            // Arrange
            hierarchy = new HierarchyEx(styleC, styleC, styleC);

            // Action
            string fontName = hierarchy.FontName;

            // Assert
            Assert.IsNull(fontName);
        }

        [TestMethod]
        public void NullableStructBasedLookup()
        {
            // Action
            int? fontSize = hierarchy.FontSize;

            // Assert
            Assert.AreEqual(styleB.FontSize, fontSize);
        }

        [TestMethod]
        public void NullableStructBasedLookup_AllEmpty()
        {
            // Arrange
            hierarchy = new HierarchyEx(styleA, styleA, styleA);

            // Action
            int? fontSize = hierarchy.FontSize;

            // Assert
            Assert.IsTrue(fontSize == null);
        }

        [TestMethod]
        public void StructBasedLookup()
        {
            // Action
            FontProperty fontProperty = hierarchy.FontProperty;

            // Assert
            Assert.AreEqual(styleC.FontProperty, fontProperty);
        }

        [TestMethod]
        public void StructBasedLookup_AllEmpty()
        {
            // Arrange
            hierarchy = new HierarchyEx(styleA, styleB, styleA);

            // Action
            FontProperty fontProperty = hierarchy.FontProperty;

            // Assert
            Assert.AreEqual(FontProperty.None, fontProperty);
        }


    }
}
