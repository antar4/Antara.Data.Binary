using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using SecurityDriven.Inferno;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestExtendedLong
    {
        #region ------------------------ long -----------------------------------------------
        /// <summary>
        /// Test adding all max and min values that can fit in 2 to 64 bits 
        /// in sequence and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void Long_AddGet_BitsMinMax_Sequence_Loopback()
        {
            int maxBits = 64;
            List<long> values = new List<long>();
            BVector d = new BVector();
            long positiveValue;
            long negativeValue;
            long actualValue;
            int index;
            long expected;
            long maxVal = long.MaxValue;
            byte bits;


            // add min and max values for 2-64 bits
            for (int i = 0; i < (maxBits-1); i++)
            {
                bits = (byte)(maxBits - i);
                positiveValue = maxVal >> i;
                negativeValue = -positiveValue - 1;
                
                // add positive
                d.Add(positiveValue, bits);
                values.Add(positiveValue);
                
                // add negative
                d.Add(negativeValue, bits);
                values.Add(negativeValue);
            }


            // get min and max values for 2-64 bits
            index = 0;
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get positive
                expected = values[index++];
                actualValue = d.GetLong(bits);
                Assert.AreEqual(expected, actualValue);

                // get min
                expected = values[index++];
                actualValue = d.GetLong(bits);
                Assert.AreEqual(expected, actualValue);
            }
        }

        /// <summary>
        /// Test adding all max and min values that can fit in 2 to 64 bits 
        /// separated by 1 bit values and and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void Long_AddGet_BitsMinMax_Separated_Loopback()
        {
            int maxBits = 64;
            List<long> values = new List<long>();
            BVector d = new BVector();
            long positiveValue;
            long negativeValue;
            long actualValue;
            int index;
            long expected;
            long maxVal = long.MaxValue;
            byte bits;


            // add min and max values for 2-64 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                positiveValue = maxVal >> i;
                negativeValue = -positiveValue - 1;

                // add positive
                d.Add(positiveValue, bits);
                values.Add(positiveValue);

                // true separator
                d.Add1();

                // add negative
                d.Add(negativeValue, bits);
                values.Add(negativeValue);

                // false separator
                d.Add1(false);
            }


            // get min and max values for 2-64 bits
            index = 0;
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get positive
                expected = values[index++];
                actualValue = d.GetLong(bits);
                Assert.AreEqual(expected, actualValue);

                // true separator
                Assert.AreEqual(true, d.Get1());

                // get min
                expected = values[index++];
                actualValue = d.GetLong(bits);
                Assert.AreEqual(expected, actualValue);

                // false separator
                Assert.AreEqual(false, d.Get1());
            }
        }


        /// <summary>
        /// Test adding 10000 random values  
        /// in sequence and and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void Long_AddGet_Rnd_Sequence_Loopback()
        {
            int maxBits = 64;
            List<long> values = new List<long>();
            BVector d = new BVector();
            long actualValue;
            int index;
            long expected;
            long maxVal = long.MaxValue;
            byte bits;
            var rnd = new CryptoRandom();
            int itemsPerBit = 10000;
            long value;
            long tmp;


            // add random values for 2-64 bits
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);
                    tmp = maxVal >> (i + 1) ;

                    // add random value
                    value = rnd.NextLong(-tmp-1, tmp);
                    values.Add(value);
                    d.Add(value, bits);
                }
            }

            // get values for 2-64 bits
            index = 0;
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);

                    // get value
                    expected = values[index];
                    actualValue = d.GetLong(bits);
                    Assert.AreEqual(expected, actualValue);
                    index++;
                }
            }
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Long_Add_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((long)0, 1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is greater than 64
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Long_Add_Exc_MoreThan64Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((long)0, 65);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetLong is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Long_Get_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add(0, 32);
            d.GetLong(1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetLong is greater than 64
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Long_Get_Exc_MoreThan64Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add(0, 32);
            d.GetLong(65);
        }
        #endregion

        #region ------------------------ long? ----------------------------------------------
        /// <summary>
        /// Test adding all max and min values that can fit in 2 to 64 bits 
        /// in sequence and and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void LongN_AddGet_BitsMinMax_Sequence_Loopback()
        {
            int maxBits = 64;
            List<long?> values = new List<long?>();
            BVector d = new BVector();
            long? positiveValue;
            long? negativeValue;
            long? actualValue;
            long? nullValue = null;
            int index;
            long? expected;
            long? maxVal = long.MaxValue;
            byte bits;


            // add min and max values for 2-64 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                positiveValue = maxVal >> i;
                negativeValue = -positiveValue - 1;

                // add positive
                d.Add(positiveValue, bits);
                values.Add(positiveValue);

                // add null
                d.Add(nullValue, bits);
                values.Add(nullValue);

                // add negative
                d.Add(negativeValue, bits);
                values.Add(negativeValue);
            }


            // get min and max values for 2-64 bits
            index = 0;
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get positive
                expected = values[index++];
                actualValue = d.GetLongN(bits);
                Assert.AreEqual(expected, actualValue);

                // get null
                expected = values[index++];
                actualValue = d.GetLongN(bits);
                Assert.AreEqual(expected, actualValue);

                // get negative
                expected = values[index++];
                actualValue = d.GetLongN(bits);
                Assert.AreEqual(expected, actualValue);
            }
        }

        /// <summary>
        /// Test adding all max and min values that can fit in 2 to 64 bits 
        /// separated by 1 bit values and and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void LongN_AddGet_BitsMinMax_Separated_Loopback()
        {
            int maxBits = 64;
            List<long?> values = new List<long?>();
            BVector d = new BVector();
            long? positiveValue;
            long? negativeValue;
            long? actualValue;
            long? nullValue = null;
            int index;
            long? expected;
            long? maxVal = long.MaxValue;
            byte bits;


            // add min and max values for 2-64 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                positiveValue = maxVal >> i;
                negativeValue = -positiveValue - 1;

                // add positive
                d.Add(positiveValue, bits);
                values.Add(positiveValue);

                // true separator
                d.Add1(true);

                // add null
                d.Add(nullValue, bits);
                values.Add(nullValue);

                // false separator
                d.Add1(false);

                // add negative
                d.Add(negativeValue, bits);
                values.Add(negativeValue);

                // true separator
                d.Add1(true);

            }


            // get min and max values for 2-64 bits
            index = 0;
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get positive
                expected = values[index++];
                actualValue = d.GetLongN(bits);
                Assert.AreEqual(expected, actualValue);

                // true separator
                Assert.AreEqual(true, d.Get1());

                // get null
                expected = values[index++];
                actualValue = d.GetLongN(bits);
                Assert.AreEqual(expected, actualValue);

                // false separator
                Assert.AreEqual(false, d.Get1());

                // get negative
                expected = values[index++];
                actualValue = d.GetLongN(bits);
                Assert.AreEqual(expected, actualValue);

                // true separator
                Assert.AreEqual(true, d.Get1());
            }
        }


        /// <summary>
        /// Test adding 10000 random values  
        /// in sequence and and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void LongN_AddGet_Rnd_Sequence_Loopback()
        {
            int maxBits = 64;
            List<long?> values = new List<long?>();
            BVector d = new BVector();
            long? actualValue;
            int index;
            long? expected;
            long? maxVal = long.MaxValue;
            byte bits;
            var rnd = new CryptoRandom();
            int itemsPerBit = 10000;
            long? value;
            long tmp;


            // add random values for 2-64 bits
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);
                    tmp = maxVal.Value >> (i + 1);

                    // add random value
                    value = rnd.NextLong(-tmp - 1, tmp);
                    values.Add(value);
                    d.Add(value, bits);
                }
            }

            // get values for 2-64 bits
            index = 0;
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);

                    // get value
                    expected = values[index];
                    actualValue = d.GetLongN(bits);
                    Assert.AreEqual(expected, actualValue);
                    index++;
                }
            }
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LongN_Add_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((long?)0, 1);
        }


        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is greater than 64
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LongN_Add_Exc_MoreThan64Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((long?)0, 65);
        }


        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetLongN is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LongN_Get_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((long?)0, 32);
            d.GetLongN(1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetLongN is greater than 64
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LongN_Get_Exc_MoreThan64Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((long?)0, 32);
            d.GetLongN(65);
        }

        #endregion
    }
}