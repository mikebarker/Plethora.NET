using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Data;
using System;
using System.Collections.Generic;

namespace Plethora.Test.Data
{
    [TestClass]
    public class DataReader_Test
    {
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
    }
}
