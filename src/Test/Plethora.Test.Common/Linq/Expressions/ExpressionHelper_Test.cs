using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Linq.Expressions;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.Linq.Expressions
{
    [TestClass]
    public class ExpressionHelper_Test
    {
        [TestMethod]
        public void GetPropertyName()
        {
            // Arrange
            Person p = default;

            // Action
            var result = ExpressionHelper.GetPropertyName(() => p.FamilyName);

            // Assert
            Assert.AreEqual("FamilyName", result);
        }

        [TestMethod]
        public void GetPropertyName_NestedProperty()
        {
            // Arrange
            Person p = default;

            // Action
            var result = ExpressionHelper.GetPropertyName(() => p.DateOfBirth.Year);

            // Assert
            Assert.AreEqual("Year", result);
        }

        [TestMethod]
        public void GetPropertyName_IndirectType()
        {
            // Arrange

            // Action
            var result = ExpressionHelper.GetPropertyName<Person, string>(person => person.FamilyName);

            // Assert
            Assert.AreEqual("FamilyName", result);
        }


        [TestMethod]
        public void GetFieldName()
        {
            // Arrange
            Record r = default;

            // Action
            var result = ExpressionHelper.GetFieldName(() => r.Name);

            // Assert
            Assert.AreEqual("Name", result);
        }


        [TestMethod]
        public void GetPropertyOrFieldName_Property()
        {
            // Arrange
            Person p = default;

            // Action
            var result = ExpressionHelper.GetPropertyOrFieldName(() => p.FamilyName);

            // Assert
            Assert.AreEqual("FamilyName", result);
        }

        [TestMethod]
        public void GetPropertyOrFieldName_Field()
        {
            // Arrange
            Record r = default;

            // Action
            var result = ExpressionHelper.GetPropertyOrFieldName(() => r.Name);

            // Assert
            Assert.AreEqual("Name", result);
        }

        [TestMethod]
        public void GetPropertyOrFieldName_Property_NestedProperty()
        {
            // Arrange
            Person p = default;

            // Action
            var result = ExpressionHelper.GetPropertyOrFieldName(() => p.DateOfBirth.Year);

            // Assert
            Assert.AreEqual("Year", result);
        }

        [TestMethod]
        public void GetPropertyOrFieldName_Property_IndirectType()
        {
            // Arrange

            // Action
            var result = ExpressionHelper.GetPropertyOrFieldName<Person, string>(person => person.FamilyName);

            // Assert
            Assert.AreEqual("FamilyName", result);
        }


        private class Record
        {
            public int Name;
        }
    }
}
