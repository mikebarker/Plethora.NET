using System;
using NUnit.Framework;

namespace Plethora.StringFormat.Test
{
    [TestFixture]
    public class StringFormatHelper_Test
    {
        [Test]
        public void ReplaceNamedFormat_Fail_NullFormat()
        {
            try
            {
                //exec
                StringFormatHelper.ReplaceNamedFormat(null, "", 0);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ReplaceNamedFormat_Fail_NullName()
        {
            try
            {
                //exec
                StringFormatHelper.ReplaceNamedFormat("", null, 0);

                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ReplaceNamedFormat_Fail_IndexLessThanZero()
        {
            try
            {
                //exec
                StringFormatHelper.ReplaceNamedFormat("", "", -1);

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ReplaceNamedFormat_NameNotFound()
        {
            //setup
            const string format = "Value: {Value}";

            //exec
            string result = format
                .ReplaceNamedFormat("Undefined", 0);

            //test
            Assert.IsNotNull(result);
            Assert.AreEqual("Value: {Value}", result);
        }

        [Test]
        public void ReplaceNamedFormat_BraceOnly()
        {
            //setup
            const string format = "Value: {Value}";

            //exec
            string result = format
                .ReplaceNamedFormat("Value", 0);

            //test
            Assert.IsNotNull(result);
            Assert.AreEqual("Value: {0}", result);
        }

        [Test]
        public void ReplaceNamedFormat_Padding()
        {
            //setup
            const string format = "Value: {Value,25}";

            //exec
            string result = format
                .ReplaceNamedFormat("Value", 0);

            //test
            Assert.IsNotNull(result);
            Assert.AreEqual("Value: {0,25}", result);
        }

        [Test]
        public void ReplaceNamedFormat_Format()
        {
            //setup
            const string format = "Value: {Value:N8}";
            //exec
            string result = format
                .ReplaceNamedFormat("Value", 0);

            //test
            Assert.IsNotNull(result);
            Assert.AreEqual("Value: {0:N8}", result);
        }

        [Test]
        public void ReplaceNamedFormat_Multiple()
        {
            //setup
            const string format = "Name: {Name}; Surname: {Surname,25}; Age: {Age:N3}";

            //exec
            string result = format
                .ReplaceNamedFormat("Name", 0)
                .ReplaceNamedFormat("Surname", 1)
                .ReplaceNamedFormat("Age", 2);

            //test
            Assert.IsNotNull(result);
            Assert.AreEqual("Name: {0}; Surname: {1,25}; Age: {2:N3}", result);
        }

        [Test]
        [Ignore("Known bug.")] // TODO: Known bug
        public void ReplaceNamedFormat_WhitespaceInFormat()
        {
            //setup
            const string format = "{Name }";

            //exec
            string result = format
                .ReplaceNamedFormat("Name", 0);

            //test
            Assert.IsNotNull(result);
            Assert.AreEqual("{0 }", result);
        }

        [Test]
        [Ignore("Known bug.")] // TODO: Known bug
        public void ReplaceNamedFormat_NamedItemInDoubleBrace()
        {
            //setup
            const string format = "{{Name}}";

            //exec
            string result = format
                .ReplaceNamedFormat("Name", 0);

            //test
            Assert.IsNotNull(result);
            Assert.AreEqual("{{Name}}", result);
        }
    }
}
