using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

#pragma warning disable 618  // Obselete members

namespace Plethora.Test
{
    [TestClass]
    public class EnumHelper_Test
    {
        #region NoFlags

        [TestMethod]
        public void NoFlags_Zero()
        {
            // Arrange
            var enumValue = NoFlagsEnum.Zero;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("Zero", descrip);
        }

        [TestMethod]
        public void NoFlags_KnownElement()
        {
            // Arrange
            var enumValue = NoFlagsEnum.Four;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("Four", descrip);
        }

        [TestMethod]
        public void NoFlags_KnownElementIsNotFlagged()
        {
            // Arrange
            var enumValue = NoFlagsEnum.Five;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("Five", descrip);
        }

        [TestMethod]
        public void NoFlags_UnknownElement()
        {
            // Arrange
            var enumValue = (NoFlagsEnum)87;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("87", descrip);
        }
        
        #endregion
        
        #region NoFlagsWithDescription

        [TestMethod]
        public void NoFlagsWithDescription_Zero()
        {
            // Arrange
            var enumValue = NoFlagsWithDescriptionEnum.Zero;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("Zéro", descrip);
        }

        [TestMethod]
        public void NoFlagsWithDescription_KnownElement()
        {
            // Arrange
            var enumValue = NoFlagsWithDescriptionEnum.Four;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("Quatre", descrip);
        }

        [TestMethod]
        public void NoFlagsWithDescription_KnownElementIsNotFlagged()
        {
            // Arrange
            var enumValue = NoFlagsWithDescriptionEnum.Five;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("Cinq", descrip);
        }

        [TestMethod]
        public void NoFlagsWithDescription_UnknownElement()
        {
            // Arrange
            var enumValue = (NoFlagsWithDescriptionEnum)87;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("87", descrip);
        }

        #endregion

        #region Flags

        [TestMethod]
        public void Flags_Zero()
        {
            // Arrange
            var enumValue = FlagsEnum.Nothing;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("Nothing", descrip);
        }

        [TestMethod]
        public void Flags_SingleFlag()
        {
            // Arrange
            var enumValue = FlagsEnum.Setting1;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("Setting1", descrip);
        }

        [TestMethod]
        public void Flags_MultipleFlags()
        {
            // Arrange
            var enumValue = FlagsEnum.Setting1 | FlagsEnum.Setting2;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("Setting1, Setting2", descrip);
        }

        [TestMethod]
        public void Flags_CoveringFlags()
        {
            // Arrange
            var enumValue = FlagsEnum.AllSettings;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("AllSettings", descrip);
        }

        [TestMethod]
        public void Flags_Unknown()
        {
            // Arrange
            var enumValue = ((FlagsEnum) 16);

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("16", descrip);
        }

        #endregion

        #region FlagsWithDescription

        [TestMethod]
        public void FlagsWithDescription_Zero()
        {
            // Arrange
            var enumValue = FlagsWithDescriptionEnum.Nothing;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("All Off", descrip);
        }

        [TestMethod]
        public void FlagsWithDescription_SingleFlag()
        {
            // Arrange
            var enumValue = FlagsWithDescriptionEnum.Setting1;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("First On", descrip);
        }

        [TestMethod]
        public void FlagsWithDescription_MultipleFlags()
        {
            // Arrange
            var enumValue = FlagsWithDescriptionEnum.Setting1 | FlagsWithDescriptionEnum.Setting2;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("First On, Second On", descrip);
        }

        [TestMethod]
        public void FlagsWithDescription_CoveringFlags()
        {
            // Arrange
            var enumValue = FlagsWithDescriptionEnum.AllSettings;

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("All On", descrip);
        }

        [TestMethod]
        public void FlagsWithDescription_Unknown()
        {
            // Arrange
            var enumValue = ((FlagsWithDescriptionEnum)16);

            //Exec
            string descrip = enumValue.Description();

            // Assert
            Assert.AreEqual("16", descrip);
        }

        #endregion




        public enum NoFlagsEnum
        {
            Zero = 0,
            One = 1,
            Two = 2,
            Three= 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10
        }

        public enum NoFlagsWithDescriptionEnum
        {
            [Description("Zéro")]
            Zero = 0,
            [Description("Un")]
            One = 1,
            [Description("Deux")]
            Two = 2,
            [Description("Trois")]
            Three = 3,
            [Description("Quatre")]
            Four = 4,
            [Description("Cinq")]
            Five = 5,
            [Description("Six")]
            Six = 6,
            [Description("Sept")]
            Seven = 7,
            [Description("Huit")]
            Eight = 8,
            [Description("Neuf")]
            Nine = 9,
            [Description("Dix")]
            Ten = 10
        }

        [Flags]
        public enum FlagsEnum
        {
            Nothing = 0,

            Setting1 = 0x01,
            Setting2 = 0x02,
            Setting3 = 0x04,

            AllSettings = Setting1 | Setting2 | Setting3
        }

        [Flags]
        public enum FlagsWithDescriptionEnum
        {
            [Description("All Off")]
            Nothing = 0,

            [Description("First On")]
            Setting1 = 0x01,
            [Description("Second On")]
            Setting2 = 0x02,
            [Description("Third On")]
            Setting3 = 0x04,

            [Description("All On")]
            AllSettings = Setting1 | Setting2 | Setting3
        }

    }
}
