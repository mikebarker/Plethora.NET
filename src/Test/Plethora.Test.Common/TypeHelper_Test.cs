using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Plethora.Test
{
    [TestClass]
    public class TypeHelper_Test
    {
        [TestMethod]
        public void GetAs_Int32_FromInt32()
        {
            //setup
            int value = 134;

            //exec
            var result = TypeHelper.As<int>(value);

            //test
            Assert.AreEqual(134, result);
        }

        [TestMethod]
        public void GetAs_Int32_FromInt64()
        {
            //setup
            long value = 134;

            //exec
            var result = TypeHelper.As<int>(value);

            //test
            Assert.AreEqual(134, result);
        }

        [TestMethod]
        public void GetAs_Int32_Error_IsNull()
        {
            object value = null;

            //exec
            try
            {
                var result = TypeHelper.As<int>(value);

                Assert.Fail();
            }
            catch (InvalidCastException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod]
        public void GetAs_Int32_Error_IsString()
        {
            //setup
            string value = "Hello";

            //exec
            try
            {
                var result = TypeHelper.As<int>(value);

                Assert.Fail();
            }
            catch (InvalidCastException ex)
            {
                Assert.IsNotNull(ex);
            }
        }


        [TestMethod]
        public void GetAs_Decimal_FromDecimal()
        {
            //setup
            decimal value = 134.5m;

            //exec
            var result = TypeHelper.As<decimal>(value);

            //test
            Assert.AreEqual(134.5m, result);
        }

        [TestMethod]
        public void GetAs_Decimal_FromDouble()
        {
            //setup
            double value = 134.5;

            //exec
            var result = TypeHelper.As<decimal>(value);

            //test
            Assert.AreEqual(134.5m, result);
        }


        [TestMethod]
        public void GetAs_String_FromString()
        {
            //setup
            string value = "Hello";

            //exec
            var result = TypeHelper.As<string>(value);

            //test
            Assert.AreEqual("Hello", result);
        }

        [TestMethod]
        public void GetAs_String_Null()
        {
            //setup
            object value = null;

            //exec
            var result = TypeHelper.As<string>(value);

            //test
            Assert.AreEqual(null, result);
        }


        [TestMethod]
        public void GetAs_NullableInt32_FromInt32()
        {
            //setup
            int value = 134;

            //exec
            var result = TypeHelper.As<int?>(value);

            //test
            Assert.AreEqual(134, result);
        }

        [TestMethod]
        public void GetAs_NullableInt32_FromInt64()
        {
            //setup
            long value = 134;

            //exec
            var result = TypeHelper.As<int?>(value);

            //test
            Assert.AreEqual(134, result);
        }

        [TestMethod]
        public void GetAs_NullableInt32_IsNull()
        {
            //setup
            object value = null;

            //exec
            var result = TypeHelper.As<int?>(value);

            //test
            Assert.IsFalse(result.HasValue);
        }

        [TestMethod]
        public void GetAs_NullableInt32_Error_IsString()
        {
            //setup
            string value = "Hello";

            //exec
            try
            {
                var result = TypeHelper.As<int?>(value);

                Assert.Fail();
            }
            catch (InvalidCastException ex)
            {
                Assert.IsNotNull(ex);
            }
        }


        [TestMethod]
        public void GetAs_NullableEnum_FromString()
        {
            //setup
            string value = "Tuesday";

            //exec
            var result = TypeHelper.As<DayOfWeek?>(value);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }

        [TestMethod]
        public void GetAs_NullableEnum_Error_NotValidEnum()
        {
            //setup
            string value = "Hello";

            //exec
            try
            {
                var result = TypeHelper.As<DayOfWeek?>(value);

                Assert.Fail();
            }
            catch (InvalidCastException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod]
        public void GetAs_NullableEnum_FromInt32()
        {
            //setup
            int value = (int)DayOfWeek.Tuesday;

            //exec
            var result = TypeHelper.As<DayOfWeek?>(value);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }

        [TestMethod]
        public void GetAs_NullableEnum_FromInt64()
        {
            //setup
            long value = (long)DayOfWeek.Tuesday;

            //exec
            var result = TypeHelper.As<DayOfWeek?>(value);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }


        [TestMethod]
        public void GetAs_Enum_FromString()
        {
            //setup
            string value = "Tuesday";

            //exec
            var result = TypeHelper.As<DayOfWeek>(value);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }

        [TestMethod]
        public void GetAs_Enum_Error_NotValidEnum()
        {
            //setup
            string value = "Hello";

            //exec
            try
            {
                var result = TypeHelper.As<DayOfWeek>(value);

                Assert.Fail();
            }
            catch (InvalidCastException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod]
        public void GetAs_Enum_FromInt32()
        {
            //setup
            int value = (int)DayOfWeek.Tuesday;

            //exec
            var result = TypeHelper.As<DayOfWeek>(value);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }

        [TestMethod]
        public void GetAs_Enum_FromInt64()
        {
            //setup
            long value = (long)DayOfWeek.Tuesday;

            //exec
            var result = TypeHelper.As<DayOfWeek>(value);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }

        [TestMethod]
        public void GetAs_Guid_FromString()
        {
            //setup
            string value = "4947C8BD-E0A9-4580-8109-23BC1A4602C3";

            //exec
            var result = TypeHelper.As<Guid>(value);

            //test
            Assert.AreEqual(new Guid("4947C8BD-E0A9-4580-8109-23BC1A4602C3"), result);
        }
    }
}
