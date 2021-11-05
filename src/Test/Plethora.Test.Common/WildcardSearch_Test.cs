using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Plethora.Test
{
    [TestClass]
    public class WildcardSearch_Test
    {
        [TestMethod]
        public void IsMatch_Empty_Match()
        {
            // Arrange
            string pattern = "";
            string text = "";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern);

            // Assert
            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatch_PatternEmpty_NoMatch()
        {
            // Arrange
            string pattern = "";
            string text = "Sample search input text";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern);

            // Assert
            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatch_NoWildcard_Match()
        {
            // Arrange
            string pattern = "Sample search input text";
            string text = "Sample search input text";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern);

            // Assert
            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatch_NoWildcard_CaseNoMatch()
        {
            // Arrange
            string pattern = "sample search input text";
            string text = "Sample search input text";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern);

            // Assert
            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatch_NoWildcard_IgnoreCase()
        {
            // Arrange
            string pattern = "sample search input text";
            string text = "Sample search input text";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern, StringComparison.OrdinalIgnoreCase);

            // Assert
            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatch_NoWildcard_NoMatch()
        {
            // Arrange
            string pattern = "mismatch";
            string text = "Sample search input text";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern);

            // Assert
            Assert.IsFalse(isMatch);
        }

        [TestMethod]
        public void IsMatch_LeadingWildcard_Match()
        {
            // Arrange
            string pattern = "*text";
            string text = "Sample search input text";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern);

            // Assert
            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatch_TrailingWildcard_Match()
        {
            // Arrange
            string pattern = "Sample*";
            string text = "Sample search input text";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern);

            // Assert
            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatch_LeadingAndTrailingWildcard_Match()
        {
            // Arrange
            string pattern = "*search*";
            string text = "Sample search input text";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern);

            // Assert
            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatch_MidWildcard_Match()
        {
            // Arrange
            string pattern = "Sample*text";
            string text = "Sample search input text";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern);

            // Assert
            Assert.IsTrue(isMatch);
        }

        [TestMethod]
        public void IsMatch_SuccessiveWildcard_Match()
        {
            // Arrange
            string pattern = "*search**text";
            string text = "Sample search input text";

            // Action
            bool isMatch = WildcardSearch.IsMatch(text, pattern);

            // Assert
            Assert.IsTrue(isMatch);
        }
    }
}
