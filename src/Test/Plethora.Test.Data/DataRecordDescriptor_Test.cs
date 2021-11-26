using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plethora.Data;
using System;

namespace Plethora.Test.Data
{
    [TestClass]
    public class DataRecordDescriptor_Test
    {
        [TestMethod]
        public void AddField()
        {
            // Arrange
            DataRecordDescriptor<DateTime> descriptor = new DataRecordDescriptor<DateTime>();

            // Aciton
            descriptor.AddField(dt => dt.Year);

            // Assert
            Assert.AreEqual(1, descriptor.FieldCount);
            Assert.AreEqual(0, descriptor.GetOrdinal("Year"));
            Assert.AreEqual("Year", descriptor.GetName(0));
        }

        [TestMethod]
        public void AddField_SpecifyName()
        {
            // Arrange
            DataRecordDescriptor<DateTime> descriptor = new DataRecordDescriptor<DateTime>();

            // Aciton
            descriptor.AddField("Tomorrow", dt => dt.AddDays(1));

            // Assert
            Assert.AreEqual(1, descriptor.FieldCount);
            Assert.AreEqual(0, descriptor.GetOrdinal("Tomorrow"));
            Assert.AreEqual("Tomorrow", descriptor.GetName(0));
        }

        [TestMethod]
        public void AddField_NameExists_Throw()
        {
            // Arrange
            DataRecordDescriptor<DateTime> descriptor = new DataRecordDescriptor<DateTime>();
            descriptor.AddField(dt => dt.Year);

            // Aciton
            try
            {
                descriptor.AddField(dt => dt.Year);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            // Assert
            Assert.AreEqual(1, descriptor.FieldCount);
            Assert.AreEqual(0, descriptor.GetOrdinal("Year"));
            Assert.AreEqual("Year", descriptor.GetName(0));
        }

        [TestMethod]
        public void FieldCount()
        {
            // Arrange
            DataRecordDescriptor<DateTime> descriptor = CreateDescriptor();

            // Aciton
            var result = descriptor.FieldCount;

            // Assert
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void GetName_Exists()
        {
            // Arrange
            DataRecordDescriptor<DateTime> descriptor = CreateDescriptor();

            // Aciton
            var result = descriptor.GetName(2);

            // Assert
            Assert.AreEqual("Day", result);
        }

        [TestMethod]
        public void GetOrdinal_Exists()
        {
            // Arrange
            DataRecordDescriptor<DateTime> descriptor = CreateDescriptor();

            // Aciton
            var result = descriptor.GetOrdinal("Day");

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void GetValue()
        {
            // Arrange
            DataRecordDescriptor<DateTime> descriptor = CreateDescriptor();

            DateTime dateTime = new DateTime(2000, 01, 02, 12, 34, 56);

            // Aciton
            var result = descriptor.GetValue(dateTime, 4); // Hour

            // Assert
            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void GetType_()
        {
            // Arrange
            DataRecordDescriptor<DateTime> descriptor = CreateDescriptor();

            // Aciton
            var result = descriptor.GetType(4); // Hour

            // Assert
            Assert.AreEqual(typeof(int), result);
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
