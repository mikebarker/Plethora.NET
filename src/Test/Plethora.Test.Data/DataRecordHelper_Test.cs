using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Data;
using Plethora.Test._MockClasses;

namespace Plethora.Test.Data
{
    [TestClass]
    public class DataRecordHelper_Test
    {
        [TestMethod]
        public void GetAs_Int32_FromInt32()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, 134);

            //exec
            var result = dataRecord.GetAs<int>(1);

            //test
            Assert.AreEqual(134, result);
        }

        [TestMethod]
        public void GetAs_Int32_FromInt64()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, (long)134);

            //exec
            var result = dataRecord.GetAs<int>(1);

            //test
            Assert.AreEqual(134, result);
        }

        [TestMethod]
        public void GetAs_Int32_Error_IsNull()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, null);

            //exec
            try
            {
                var result = dataRecord.GetAs<int>(1);

                Assert.Fail();
            }
            catch (InvalidCastException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod]
        public void GetAs_Int32_Error_IsDBNull()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, DBNull.Value);

            //exec
            try
            {
                var result = dataRecord.GetAs<int>(1);

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
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, "Hello");

            //exec
            try
            {
                var result = dataRecord.GetAs<int>(1);

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
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, 134.5m);

            //exec
            var result = dataRecord.GetAs<decimal>(1);

            //test
            Assert.AreEqual(134.5m, result);
        }

        [TestMethod]
        public void GetAs_Decimal_FromDouble()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, 134.5);

            //exec
            var result = dataRecord.GetAs<decimal>(1);

            //test
            Assert.AreEqual(134.5m, result);
        }


        [TestMethod]
        public void GetAs_String_FromString()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, "Hello");

            //exec
            var result = dataRecord.GetAs<string>(1);

            //test
            Assert.AreEqual("Hello", result);
        }

        [TestMethod]
        public void GetAs_String_Null()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, null);

            //exec
            var result = dataRecord.GetAs<string>(1);

            //test
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void GetAs_String_DBNull()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, DBNull.Value);

            //exec
            var result = dataRecord.GetAs<string>(1);

            //test
            Assert.AreEqual(null, result);
        }



        [TestMethod]
        public void GetAs_NullableInt32_FromInt32()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, 134);

            //exec
            var result = dataRecord.GetAs<int?>(1);

            //test
            Assert.AreEqual(134, result);
        }

        [TestMethod]
        public void GetAs_NullableInt32_FromInt64()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, (long)134);

            //exec
            var result = dataRecord.GetAs<int?>(1);

            //test
            Assert.AreEqual(134, result);
        }

        [TestMethod]
        public void GetAs_NullableInt32_IsNull()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, null);

            //exec
            var result = dataRecord.GetAs<int?>(1);

            //test
            Assert.IsFalse(result.HasValue);
        }

        [TestMethod]
        public void GetAs_NullableInt32_IsDBNull()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, DBNull.Value);

            //exec
            var result = dataRecord.GetAs<int?>(1);

            //test
            Assert.IsFalse(result.HasValue);
        }

        [TestMethod]
        public void GetAs_NullableInt32_Error_IsString()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, "Hello");

            //exec
            try
            {
                var result = dataRecord.GetAs<int?>(1);

                Assert.Fail();
            }
            catch (InvalidCastException ex)
            {
                Assert.IsNotNull(ex);
            }
        }


        [TestMethod]
        public void GetAs_NullableEnum_Null()
        {
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, DBNull.Value);

            var result = dataRecord.GetAs<DayOfWeek?>(1);

            //test
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void GetAs_NullableEnum_FromString()
        {
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, "Tuesday");

            var result = dataRecord.GetAs<DayOfWeek?>(1);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }

        [TestMethod]
        public void GetAs_NullableEnum_Error_NotValidEnum()
        {
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, "Hello");

            //exec
            try
            {
                var result = dataRecord.GetAs<DayOfWeek?>(1);

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
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, (int)DayOfWeek.Tuesday);

            var result = dataRecord.GetAs<DayOfWeek?>(1);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }

        [TestMethod]
        public void GetAs_NullableEnum_FromInt64()
        {
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, (long)DayOfWeek.Tuesday);

            var result = dataRecord.GetAs<DayOfWeek?>(1);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }


        [TestMethod]
        public void GetAs_Enum_FromString()
        {
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, "Tuesday");

            var result = dataRecord.GetAs<DayOfWeek>(1);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }

        [TestMethod]
        public void GetAs_Enum_Error_NotValidEnum()
        {
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, "Hello");

            //exec
            try
            {
                var result = dataRecord.GetAs<DayOfWeek>(1);

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
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, (int)DayOfWeek.Tuesday);

            var result = dataRecord.GetAs<DayOfWeek>(1);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }

        [TestMethod]
        public void GetAs_Enum_FromInt64()
        {
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, (long)DayOfWeek.Tuesday);

            var result = dataRecord.GetAs<DayOfWeek>(1);

            //test
            Assert.AreEqual(DayOfWeek.Tuesday, result);
        }

        [TestMethod]
        public void GetAs_Guid_FromString()
        {
            //setup
            MockDataRecord dataRecord = new MockDataRecord();
            dataRecord.SetValue(1, "4947C8BD-E0A9-4580-8109-23BC1A4602C3");

            //exec
            var result = dataRecord.GetAs<Guid>(1);

            //test
            Assert.AreEqual(new Guid("4947C8BD-E0A9-4580-8109-23BC1A4602C3"), result);
        }
    }
}
