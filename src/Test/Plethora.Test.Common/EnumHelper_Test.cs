using System;
using NUnit.Framework;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Plethora.Test
{
    [TestFixture]
    public class EnumHelper_Test
    {
        #region NoFlags

        [Test]
        public void NoFlags_Zero()
        {
            //Setup
            var enumValue = NoFlagsEnum.Zero;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("Zero", descrip);
        }

        [Test]
        public void NoFlags_KnownElement()
        {
            //Setup
            var enumValue = NoFlagsEnum.Four;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("Four", descrip);
        }

        [Test]
        public void NoFlags_KnownElementIsNotFlagged()
        {
            //Setup
            var enumValue = NoFlagsEnum.Five;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("Five", descrip);
        }

        [Test]
        public void NoFlags_UnknownElement()
        {
            //Setup
            var enumValue = (NoFlagsEnum)87;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("87", descrip);
        }
        
        #endregion
        
        #region NoFlagsWithDescription

        [Test]
        public void NoFlagsWithDescription_Zero()
        {
            //Setup
            var enumValue = NoFlagsWithDescriptionEnum.Zero;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("Zéro", descrip);
        }

        [Test]
        public void NoFlagsWithDescription_KnownElement()
        {
            //Setup
            var enumValue = NoFlagsWithDescriptionEnum.Four;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("Quatre", descrip);
        }

        [Test]
        public void NoFlagsWithDescription_KnownElementIsNotFlagged()
        {
            //Setup
            var enumValue = NoFlagsWithDescriptionEnum.Five;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("Cinq", descrip);
        }

        [Test]
        public void NoFlagsWithDescription_UnknownElement()
        {
            //Setup
            var enumValue = (NoFlagsWithDescriptionEnum)87;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("87", descrip);
        }

        #endregion

        #region Flags

        [Test]
        public void Flags_Zero()
        {
            //Setup
            var enumValue = FlagsEnum.Nothing;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("Nothing", descrip);
        }

        [Test]
        public void Flags_SingleFlag()
        {
            //Setup
            var enumValue = FlagsEnum.Setting1;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("Setting1", descrip);
        }

        [Test]
        public void Flags_MultipleFlags()
        {
            //Setup
            var enumValue = FlagsEnum.Setting1 | FlagsEnum.Setting2;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("Setting1, Setting2", descrip);
        }

        [Test]
        public void Flags_CoveringFlags()
        {
            //Setup
            var enumValue = FlagsEnum.AllSettings;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("AllSettings", descrip);
        }

        [Test]
        public void Flags_Unknown()
        {
            //Setup
            var enumValue = ((FlagsEnum) 16);

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("16", descrip);
        }

        #endregion

        #region FlagsWithDescription

        [Test]
        public void FlagsWithDescription_Zero()
        {
            //Setup
            var enumValue = FlagsWithDescriptionEnum.Nothing;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("All Off", descrip);
        }

        [Test]
        public void FlagsWithDescription_SingleFlag()
        {
            //Setup
            var enumValue = FlagsWithDescriptionEnum.Setting1;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("First On", descrip);
        }

        [Test]
        public void FlagsWithDescription_MultipleFlags()
        {
            //Setup
            var enumValue = FlagsWithDescriptionEnum.Setting1 | FlagsWithDescriptionEnum.Setting2;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("First On, Second On", descrip);
        }

        [Test]
        public void FlagsWithDescription_CoveringFlags()
        {
            //Setup
            var enumValue = FlagsWithDescriptionEnum.AllSettings;

            //Exec
            string descrip = enumValue.Description();

            //Test
            Assert.AreEqual("All On", descrip);
        }

        [Test]
        public void FlagsWithDescription_Unknown()
        {
            //Setup
            var enumValue = ((FlagsWithDescriptionEnum)16);

            //Exec
            string descrip = enumValue.Description();

            //Test
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
