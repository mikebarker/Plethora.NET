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
    }
}
