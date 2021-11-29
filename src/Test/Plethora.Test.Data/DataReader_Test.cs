using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Plethora.Test.Data
{
    [TestClass]
    public class DataReader_Test
    {
        [TestMethod]
        public void Depth()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            var result = ((IDataReader)dataReader).Depth;

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void FieldCount()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            var result = dataReader.FieldCount;

            // Assert
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void FieldCount_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            try
            {
                var result = dataReader.FieldCount;

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void Dispose_ClosesDataReader()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            ((IDisposable)dataReader).Dispose();

            // Assert
            Assert.IsTrue(dataReader.IsClosed);
        }


        [TestMethod]
        public void Close()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Assert
            Assert.IsFalse(dataReader.IsClosed);

            // Action
            dataReader.Close();

            // Assert
            Assert.IsTrue(dataReader.IsClosed);
        }

        [TestMethod]
        public void Close_RepeatCall_DoesNotThrow()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            dataReader.Close();

            // Assert
        }


        [TestMethod]
        public void GetOrdinal()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            var result = dataReader.GetOrdinal("Hour");

            // Assert
            Assert.AreEqual(4, result);
        }

        [TestMethod]
        public void GetOrdinal_NotExistant_Thrws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            try
            {
                var result = dataReader.GetOrdinal("Moon");

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void GetOrdinal_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            try
            {
                var result = dataReader.GetOrdinal("Hour");

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void GetName()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            var result = dataReader.GetName(4); // Hour

            // Assert
            Assert.AreEqual("Hour", result);
        }

        [TestMethod]
        public void GetName_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            try
            {
                var result = dataReader.GetName(0);

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void Read()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            // Assert
            Assert.IsTrue(dataReader.Read());
            Assert.IsTrue(dataReader.Read());
            Assert.IsTrue(dataReader.Read());
            Assert.IsFalse(dataReader.Read());  // Three elements in the list
        }

        [TestMethod]
        public void Read_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            try
            {
                var result = dataReader.Read();

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void NextResult_ReturnsFalse()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            var result = ((IDataReader)dataReader).NextResult();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NextResult_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            try
            {
                var result = ((IDataReader)dataReader).NextResult();

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void Indexer_Ordinal()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Read();

            // Action - 1st item
            // Assert
            Assert.AreEqual(2000, dataReader[0]); // Year
            Assert.AreEqual(01, dataReader[1]); // Month
            Assert.AreEqual(01, dataReader[2]); // Day
            Assert.AreEqual("Saturday", dataReader[3]); // DayOfWeek
            Assert.AreEqual(12, dataReader[4]); // Hour
            Assert.AreEqual(34, dataReader[5]); // Minute
            Assert.AreEqual(56, dataReader[6]); // Second
        }
        [TestMethod]
        public void Indexer_Ordinal_ReadNotStarted_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            try
            {
                var result = dataReader[4]; // Hour

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void Indexer_Ordinal_InvalidOrdinal_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Read();

            // Action
            try
            {
                var result = dataReader[8]; // Invalid

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void Indexer_Ordinal_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            try
            {
                var result = dataReader[0];

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void Indexer_Name()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Read();

            // Action - 1st item
            // Assert
            Assert.AreEqual(2000, dataReader["Year"]);
            Assert.AreEqual(01, dataReader["Month"]);
            Assert.AreEqual(01, dataReader["Day"]);
            Assert.AreEqual("Saturday", dataReader["DayOfWeek"]);
            Assert.AreEqual(12, dataReader["Hour"]);
            Assert.AreEqual(34, dataReader["Minute"]);
            Assert.AreEqual(56, dataReader["Second"]);
        }
        [TestMethod]
        public void Indexer_Name_ReadNotStarted_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            try
            {
                var result = dataReader["Hour"];

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void Indexer_Name_InvalidOrdinal_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Read();

            // Action
            try
            {
                var result = dataReader["Moon"]; // Invalid

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void Indexer_Name_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            try
            {
                var result = dataReader["Year"];

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void GetValue()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action - 1st item
            // Assert
            dataReader.Read();
            Assert.AreEqual(2000, dataReader.GetValue(0)); // Year
            Assert.AreEqual(01, dataReader.GetValue(1)); // Month
            Assert.AreEqual(01, dataReader.GetValue(2)); // Day
            Assert.AreEqual("Saturday", dataReader.GetValue(3)); // DayOfWeek
            Assert.AreEqual(12, dataReader.GetValue(4)); // Hour
            Assert.AreEqual(34, dataReader.GetValue(5)); // Minute
            Assert.AreEqual(56, dataReader.GetValue(6)); // Second

            // Action - 2nd item
            // Assert
            dataReader.Read();
            Assert.AreEqual(2000, dataReader.GetValue(0)); // Year
            Assert.AreEqual(02, dataReader.GetValue(1)); // Month
            Assert.AreEqual(02, dataReader.GetValue(2)); // Day
            Assert.AreEqual("Wednesday", dataReader.GetValue(3)); // DayOfWeek
            Assert.AreEqual(01, dataReader.GetValue(4)); // Hour
            Assert.AreEqual(02, dataReader.GetValue(5)); // Minute
            Assert.AreEqual(03, dataReader.GetValue(6)); // Second

            // Action - 3rd item
            // Assert
            dataReader.Read();
            Assert.AreEqual(2000, dataReader.GetValue(0)); // Year
            Assert.AreEqual(03, dataReader.GetValue(1)); // Month
            Assert.AreEqual(03, dataReader.GetValue(2)); // Day
            Assert.AreEqual("Friday", dataReader.GetValue(3)); // DayOfWeek
            Assert.AreEqual(06, dataReader.GetValue(4)); // Hour
            Assert.AreEqual(15, dataReader.GetValue(5)); // Minute
            Assert.AreEqual(42, dataReader.GetValue(6)); // Second
        }

        [TestMethod]
        public void GetValue_ReadNotStarted_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            try
            {
                var result = dataReader.GetValue(4); // Hour

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void GetValue_InvalidOrdinal_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Read();

            // Action
            try
            {
                var result = dataReader.GetValue(8); // Invalid

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void GetValue_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            try
            {
                var result = dataReader.GetValue(0);

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void GetValues()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Read();

            object[] values = new object[7];

            // Action
            int count = ((IDataReader)dataReader).GetValues(values);

            // Assert
            Assert.AreEqual(7, count);
            Assert.AreEqual(dataReader[0], values[0]);
            Assert.AreEqual(dataReader[1], values[1]);
            Assert.AreEqual(dataReader[2], values[2]);
            Assert.AreEqual(dataReader[3], values[3]);
            Assert.AreEqual(dataReader[4], values[4]);
            Assert.AreEqual(dataReader[5], values[5]);
            Assert.AreEqual(dataReader[6], values[6]);
        }

        [TestMethod]
        public void GetValues_ArrayTooSmall_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            object[] values = new object[1];

            // Action
            try
            {
                var result = ((IDataReader)dataReader).GetValues(values);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public void GetValues_ReadNotStarted_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            object[] values = new object[8];

            // Action
            try
            {
                var result = ((IDataReader)dataReader).GetValues(values);

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void GetValues_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            object[] values = new object[8];

            // Action
            try
            {
                var result = ((IDataReader)dataReader).GetValues(values);

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void GetFieldType()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            var result = ((IDataReader)dataReader).GetFieldType(4); // Hour

            // Assert
            Assert.AreEqual(typeof(int), result);
        }

        [TestMethod]
        public void GetFieldType_Invalid_Thorws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            try
            {
                var result = ((IDataReader)dataReader).GetFieldType(8); // Invalid

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void GetFieldType_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            try
            {
                var result = ((IDataReader)dataReader).GetFieldType(4); // Hour

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }


        [TestMethod]
        public void GetDataTypeName()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            var result = ((IDataReader)dataReader).GetDataTypeName(4); // Hour

            // Assert
            Assert.AreEqual("Int32", result);
        }

        [TestMethod]
        public void GetDataTypeName_Invalid_Thorws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();

            // Action
            try
            {
                var result = ((IDataReader)dataReader).GetDataTypeName(8); // Invalid

                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        [TestMethod]
        public void GetDataTypeName_IsClosed_Throws()
        {
            // Arrange
            DataReader<DateTime> dataReader = CreateDataReader();
            dataReader.Close();

            // Action
            try
            {
                var result = ((IDataReader)dataReader).GetDataTypeName(4); // Hour

                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
        }

        [TestMethod]
        public void GetBoolean()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetBoolean(0);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void GetByte()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetByte(1);

            // Assert
            Assert.AreEqual((byte)1, result);
        }

        [TestMethod]
        public void GetChar()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetChar(2);

            // Assert
            Assert.AreEqual('a', result);
        }

        [TestMethod]
        public void GetDateTime()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetDateTime(3);

            // Assert
            Assert.AreEqual(new DateTime(2000, 01, 01), result);
        }

        [TestMethod]
        public void GetDecimal()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetDecimal(4);

            // Assert
            Assert.AreEqual(1m, result);
        }

        [TestMethod]
        public void GetDouble()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetDouble(5);

            // Assert
            Assert.AreEqual(1.0, result);
        }

        [TestMethod]
        public void GetFloat()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetFloat(6);

            // Assert
            Assert.AreEqual(1.0f, result);
        }

        [TestMethod]
        public void GetGuid()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetGuid(7);

            // Assert
            Assert.AreEqual(new Guid(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1), result);
        }

        [TestMethod]
        public void GetInt16()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetInt16(8);

            // Assert
            Assert.AreEqual((short)1, result);
        }

        [TestMethod]
        public void GetInt32()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetInt32(9);

            // Assert
            Assert.AreEqual((int)1, result);
        }

        [TestMethod]
        public void GetInt64()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetInt64(10);

            // Assert
            Assert.AreEqual((long)1, result);
        }

        [TestMethod]
        public void GetString()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).GetString(11);

            // Assert
            Assert.AreEqual("String", result);
        }

        [TestMethod]
        public void IsDBNull_NotNull_False()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).IsDBNull(11); // String

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDBNull_Null_True()
        {
            // Arrange
            var collection = new[] { new AllTypesProvider() };
            var descriptor = CreateAllTypesProviderDescriptor();
            DataReader<AllTypesProvider> dataReader = new DataReader<AllTypesProvider>(collection, descriptor);

            dataReader.Read();

            // Action
            var result = ((IDataReader)dataReader).IsDBNull(12); // Nullable

            // Assert
            Assert.IsTrue(result);
        }


        #region private members

        private static DataReader<DateTime> CreateDataReader(
            IEnumerable<DateTime> elements = null,
            DataRecordDescriptor<DateTime> descriptor = null)
        {
            if (elements == null)
                elements = CreateDateList();

            if (descriptor == null)
                descriptor = CreateDescriptor();

            DataReader<DateTime> dataReader = new(elements, descriptor);
            return dataReader;
        }

        private static List<DateTime> CreateDateList()
        {
            List<DateTime> list = new()
            {
                new DateTime(2000, 01, 01, 12, 34, 56),
                new DateTime(2000, 02, 02, 01, 02, 03),
                new DateTime(2000, 03, 03, 06, 15, 42),
            };

            return list;
        }

        private static DataRecordDescriptor<DateTime> CreateDescriptor()
        {
            DataRecordDescriptor<DateTime> descriptor = new();
            descriptor.AddField(dt => dt.Year);
            descriptor.AddField(dt => dt.Month);
            descriptor.AddField(dt => dt.Day);
            descriptor.AddField("DayOfWeek", dt => dt.DayOfWeek.ToString());
            descriptor.AddField(dt => dt.Hour);
            descriptor.AddField(dt => dt.Minute);
            descriptor.AddField(dt => dt.Second);

            return descriptor;
        }


        private static DataRecordDescriptor<AllTypesProvider> CreateAllTypesProviderDescriptor()
        {
            DataRecordDescriptor<AllTypesProvider> descriptor = new();
            descriptor.AddField(p => p.Boolean);
            descriptor.AddField(p => p.Byte);
            descriptor.AddField(p => p.Char);
            descriptor.AddField(p => p.DateTime);
            descriptor.AddField(p => p.Decimal);
            descriptor.AddField(p => p.Double);
            descriptor.AddField(p => p.Float);
            descriptor.AddField(p => p.Guid);
            descriptor.AddField(p => p.Int16);
            descriptor.AddField(p => p.Int32);
            descriptor.AddField(p => p.Int64);
            descriptor.AddField(p => p.String);
            descriptor.AddField(p => p.Nullable);

            return descriptor;
        }



        class AllTypesProvider
        {
            public bool Boolean { get; } = true;
            public byte Byte { get; } = 1;
            public char Char { get; } = 'a';
            public DateTime DateTime { get; } = new DateTime(2000, 01, 01);
            public decimal Decimal { get; } = 1m;
            public double Double { get; } = 1.0;
            public float Float { get; } = 1.0f;
            public Guid Guid { get; } = new Guid(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
            public short Int16 { get; } = 1;
            public int Int32 { get; } = 1;
            public long Int64 { get; } = 1;
            public string String { get; } = "String";
            public int? Nullable { get; } = null;
        }

        #endregion
    }
}
