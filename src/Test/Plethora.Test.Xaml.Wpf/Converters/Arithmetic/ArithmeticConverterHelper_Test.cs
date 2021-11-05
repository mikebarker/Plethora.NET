using System;
using System.Globalization;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Plethora.Xaml.Wpf.Converters;

namespace Plethora.Test.Xaml.Converters.Arithmetic
{
    [TestClass]
    public class ArithmeticConverterHelper_Test
    {
        #region ConvertForOperation

        [TestMethod]
        public void ConvertForOperation_Byte_Byte()
        {
            //setup
            Byte a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Byte_SByte()
        {
            //setup
            Byte a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Byte_Int16()
        {
            //setup
            Byte a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Byte_UInt16()
        {
            //setup
            Byte a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Byte_Int32()
        {
            //setup
            Byte a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Byte_UInt32()
        {
            //setup
            Byte a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Byte_Int64()
        {
            //setup
            Byte a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Byte_UInt64()
        {
            //setup
            Byte a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Byte_Single()
        {
            //setup
            Byte a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Byte_Double()
        {
            //setup
            Byte a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Byte_Decimal()
        {
            //setup
            Byte a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_SByte_Byte()
        {
            //setup
            SByte a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_SByte_SByte()
        {
            //setup
            SByte a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_SByte_Int16()
        {
            //setup
            SByte a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_SByte_UInt16()
        {
            //setup
            SByte a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_SByte_Int32()
        {
            //setup
            SByte a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_SByte_UInt32()
        {
            //setup
            SByte a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_SByte_Int64()
        {
            //setup
            SByte a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_SByte_UInt64_Fail()
        {
            //setup
            SByte a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_SByte_Single()
        {
            //setup
            SByte a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_SByte_Double()
        {
            //setup
            SByte a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_SByte_Decimal()
        {
            //setup
            SByte a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int16_Byte()
        {
            //setup
            Int16 a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int16_SByte()
        {
            //setup
            Int16 a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int16_Int16()
        {
            //setup
            Int16 a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int16_UInt16()
        {
            //setup
            Int16 a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int16_Int32()
        {
            //setup
            Int16 a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int16_UInt32()
        {
            //setup
            Int16 a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int16_Int64()
        {
            //setup
            Int16 a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int16_UInt64_Fail()
        {
            //setup
            Int16 a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_Int16_Single()
        {
            //setup
            Int16 a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int16_Double()
        {
            //setup
            Int16 a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int16_Decimal()
        {
            //setup
            Int16 a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_Byte()
        {
            //setup
            UInt16 a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_SByte()
        {
            //setup
            UInt16 a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_Int16()
        {
            //setup
            UInt16 a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_UInt16()
        {
            //setup
            UInt16 a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_Int32()
        {
            //setup
            UInt16 a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_UInt32()
        {
            //setup
            UInt16 a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_Int64()
        {
            //setup
            UInt16 a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_UInt64()
        {
            //setup
            UInt16 a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_Single()
        {
            //setup
            UInt16 a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_Double()
        {
            //setup
            UInt16 a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt16_Decimal()
        {
            //setup
            UInt16 a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int32_Byte()
        {
            //setup
            Int32 a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int32_SByte()
        {
            //setup
            Int32 a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int32_Int16()
        {
            //setup
            Int32 a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int32_UInt16()
        {
            //setup
            Int32 a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int32_Int32()
        {
            //setup
            Int32 a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int32_UInt32()
        {
            //setup
            Int32 a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int32_Int64()
        {
            //setup
            Int32 a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int32_UInt64_Fail()
        {
            //setup
            Int32 a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_Int32_Single()
        {
            //setup
            Int32 a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int32_Double()
        {
            //setup
            Int32 a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int32_Decimal()
        {
            //setup
            Int32 a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_Byte()
        {
            //setup
            UInt32 a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_SByte()
        {
            //setup
            UInt32 a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_Int16()
        {
            //setup
            UInt32 a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_UInt16()
        {
            //setup
            UInt32 a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_Int32()
        {
            //setup
            UInt32 a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_UInt32()
        {
            //setup
            UInt32 a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_Int64()
        {
            //setup
            UInt32 a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_UInt64()
        {
            //setup
            UInt32 a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_Single()
        {
            //setup
            UInt32 a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_Double()
        {
            //setup
            UInt32 a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt32_Decimal()
        {
            //setup
            UInt32 a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int64_Byte()
        {
            //setup
            Int64 a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int64_SByte()
        {
            //setup
            Int64 a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int64_Int16()
        {
            //setup
            Int64 a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int64_UInt16()
        {
            //setup
            Int64 a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int64_Int32()
        {
            //setup
            Int64 a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int64_UInt32()
        {
            //setup
            Int64 a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int64_Int64()
        {
            //setup
            Int64 a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int64_UInt64_Fail()
        {
            //setup
            Int64 a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_Int64_Single()
        {
            //setup
            Int64 a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int64_Double()
        {
            //setup
            Int64 a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Int64_Decimal()
        {
            //setup
            Int64 a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_Byte()
        {
            //setup
            UInt64 a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_SByte_Fail()
        {
            //setup
            UInt64 a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_Int16_Fail()
        {
            //setup
            UInt64 a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_UInt16()
        {
            //setup
            UInt64 a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_Int32_Fail()
        {
            //setup
            UInt64 a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_UInt32()
        {
            //setup
            UInt64 a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_Int64_Fail()
        {
            //setup
            UInt64 a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_UInt64()
        {
            //setup
            UInt64 a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_Single()
        {
            //setup
            UInt64 a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_Double()
        {
            //setup
            UInt64 a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_UInt64_Decimal()
        {
            //setup
            UInt64 a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_Byte()
        {
            //setup
            Single a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_SByte()
        {
            //setup
            Single a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_Int16()
        {
            //setup
            Single a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_UInt16()
        {
            //setup
            Single a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_Int32()
        {
            //setup
            Single a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_UInt32()
        {
            //setup
            Single a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_Int64()
        {
            //setup
            Single a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_UInt64()
        {
            //setup
            Single a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_Single()
        {
            //setup
            Single a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_Double()
        {
            //setup
            Single a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Single_Decimal_Fail()
        {
            //setup
            Single a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_Double_Byte()
        {
            //setup
            Double a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Double_SByte()
        {
            //setup
            Double a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Double_Int16()
        {
            //setup
            Double a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Double_UInt16()
        {
            //setup
            Double a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Double_Int32()
        {
            //setup
            Double a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Double_UInt32()
        {
            //setup
            Double a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Double_Int64()
        {
            //setup
            Double a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Double_UInt64()
        {
            //setup
            Double a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Double_Single()
        {
            //setup
            Double a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Double_Double()
        {
            //setup
            Double a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Double_Decimal_Fail()
        {
            //setup
            Double a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_Byte()
        {
            //setup
            Decimal a = 1;
            Byte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_SByte()
        {
            //setup
            Decimal a = 1;
            SByte b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_Int16()
        {
            //setup
            Decimal a = 1;
            Int16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_UInt16()
        {
            //setup
            Decimal a = 1;
            UInt16 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_Int32()
        {
            //setup
            Decimal a = 1;
            Int32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_UInt32()
        {
            //setup
            Decimal a = 1;
            UInt32 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_Int64()
        {
            //setup
            Decimal a = 1;
            Int64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_UInt64()
        {
            //setup
            Decimal a = 1;
            UInt64 b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_Single_Fail()
        {
            //setup
            Decimal a = 1;
            Single b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_Double_Fail()
        {
            //setup
            Decimal a = 1;
            Double b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            try
            {
                ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

                Assert.Fail();
            }
            catch (InvalidCastException)
            {
            }
        }

        [TestMethod]
        public void ConvertForOperation_Decimal_Decimal()
        {
            //setup
            Decimal a = 1;
            Decimal b = 2;
            object aObj = a;
            object bObj = b;

            //exec
            var result = a + b;
            ArithmeticConverterHelper_ConvertForOperation(ref aObj, ref bObj);

            //test
            Type resultType = result.GetType();
            Type aObjType = aObj.GetType();
            Type bObjType = bObj.GetType();

            Assert.AreEqual(resultType, aObjType);
            Assert.AreEqual(resultType, bObjType);
        }


        #endregion

        #region ConvertType

        [TestMethod]
        public void ConvertType_SameType()
        {
            //setup
            Int32 value = 1;
            Type targetType = typeof(Int32);

            //exec
            object result = ArithmeticConverterHelper_ConvertType(value, targetType, CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(1, result);
            Assert.AreEqual(targetType, result.GetType());
        }

        [TestMethod]
        public void ConvertType_NullableNull()
        {
            //setup
            object value = null;
            Type targetType = typeof(int?);

            //exec
            object result = ArithmeticConverterHelper_ConvertType(value, targetType, CultureInfo.InvariantCulture);

            //test
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ConvertType_NullableValue()
        {
            //setup
            Int32 value = 1;
            Type targetType = typeof(int?);

            //exec
            object result = ArithmeticConverterHelper_ConvertType(value, targetType, CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(1, result);
            Assert.AreEqual(typeof(int), result.GetType());
        }

        [TestMethod]
        public void ConvertType_Enum()
        {
            //setup
            Int32 value = 1;
            Type targetType = typeof(DayOfWeek);

            //exec
            object result = ArithmeticConverterHelper_ConvertType(value, targetType, CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(DayOfWeek.Monday, result);
            Assert.AreEqual(targetType, result.GetType());
        }

        [TestMethod]
        public void ConvertType_NullToClass()
        {
            //setup
            object value = null;
            Type targetType = typeof(Attribute);

            //exec
            object result = ArithmeticConverterHelper_ConvertType(value, targetType, CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(null, result);
        }

        [TestMethod]
        public void ConvertType_InherantTypeConvertion()
        {
            //setup
            Int32 value = 1;
            Type targetType = typeof(Int64);

            //exec
            object result = ArithmeticConverterHelper_ConvertType(value, targetType, CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(1L, result);
            Assert.AreEqual(targetType, result.GetType());
        }

        [TestMethod]
        public void ConvertType_CastMethod()
        {
            //setup
            Rational value = new Rational(1, 2);
            Type targetType = typeof(decimal);

            //exec
            object result = ArithmeticConverterHelper_ConvertType(value, targetType, CultureInfo.InvariantCulture);

            //test
            Assert.AreEqual(0.5m, result);
            Assert.AreEqual(targetType, result.GetType());
        }

        #endregion


        private static void ArithmeticConverterHelper_ConvertForOperation(ref object a, ref object b)
        {
            var args = new[] { a, b };

            try
            {
                Assembly assembly = typeof(AdditionConverter).Assembly;
                Type type = assembly.GetType("Plethora.Xaml.Wpf.Converters.ArithmeticConverterHelper");
                MethodInfo method = type.GetMethod("ConvertForOperation", BindingFlags.Static | BindingFlags.Public);
                method.Invoke(null, args);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }

            a = args[0];
            b = args[1];
        }

        private static object ArithmeticConverterHelper_ConvertType(object value, Type targetType, IFormatProvider provider)
        {
            object[] args = { value, targetType, provider };

            try
            {
                Assembly assembly = typeof(AdditionConverter).Assembly;
                Type type = assembly.GetType("Plethora.Xaml.Wpf.Converters.ArithmeticConverterHelper");
                MethodInfo method = type.GetMethod("ConvertType", BindingFlags.Static | BindingFlags.Public);
                object result = method.Invoke(null, args);
                return result;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
