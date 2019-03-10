using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityDriven.Inferno;
using System.Collections.Generic;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestExtendedShort
    {
        #region ------------------------ long -----------------------------------------------
        /// <summary>
        /// Test adding all max and min values that can fit in 2 -> 32 bits 
        /// in sequence and seperated by an extra bit and then getting them
        /// from the same BVector
        /// </summary>
        [TestMethod]
        public void Ext_Short_AddGet_BitsMinMax_Loopback()
        {
            int maxBits = 16;
            BVector d = new BVector();
            short value;
            short val;
            short expected;
            short maxVal = short.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = (short)(maxVal >> i);
                // add max
                d.Add(value, bits);

                // add min
                d.Add(-value, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = (short)(maxVal >> i);
                // add max
                d.Add(value, bits);
                d.Add1(false);

                // add min
                d.Add(-value, bits);
                d.Add1(false);
            }

            BVector d2 = d;

            // get min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = (short)(maxVal >> i);
                val = d2.GetShort(bits);
                Assert.AreEqual(expected, val);

                // get min
                expected = (short)-expected;
                val = d2.GetShort(bits);
                Assert.AreEqual(expected, val);
            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = (short)(maxVal >> i);
                val = d2.GetShort(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());

                // get min
                expected = (short)-expected;
                val = d2.GetShort(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());
            }
        }


        /// <summary>
        /// Test adding all max and min values that can fit in 2 -> 32 bits 
        /// in sequence and seperated by an extra bit and then getting them
        /// from a new BVector intantiated with the byte[] from the first BVector
        /// </summary>
        [TestMethod]
        public void Ext_Short_AddGet_BitsMinMax_InOut()
        {
            int maxBits = 16;
            BVector d = new BVector();
            short value;
            short val;
            short expected;
            short maxVal = short.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = (short)(maxVal >> i);
                // add max
                d.Add(value, bits);

                // add min
                d.Add(-value, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = (short)(maxVal >> i);
                // add max
                d.Add(value, bits);
                d.Add1(false);

                // add min
                d.Add(-value, bits);
                d.Add1(false);
            }

            BVector d2 = new BVector(d.ToBytes());

            // get min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = (short)(maxVal >> i);
                val = d2.GetShort(bits);
                Assert.AreEqual(expected, val);

                // get min
                expected = (short)-expected;
                val = d2.GetShort(bits);
                Assert.AreEqual(expected, val);
            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = (short)(maxVal >> i);
                val = d2.GetShort(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());

                // get min
                expected = (short)-expected;
                val = d2.GetShort(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());
            }
        }

        /// <summary>
        /// Test adding random values that can fit in 2 -> 32 bits 
        /// in sequence and seperated by an extra bit and then getting them
        /// from a new BVector intantiated with the byte[] from the first BVector
        /// </summary>
        [TestMethod]
        public void Ext_Short_AddGet_Rnd_InOut()
        {
            BVector d = new BVector();
            var rnd = new CryptoRandom();
            List<short> values = new List<short>();
            int maxBits = 16;
            int index = 0;
            int itemsPerBit = 100;
            short value;
            short val;
            short expected;
            short maxVal = short.MaxValue;
            short tmp;
            byte bits;


            // add random values for 2-32 bits
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);
                    tmp = (short)(maxVal >> (i + 1));

                    // add random value
                    value = (short)rnd.Next(-tmp, tmp);
                    values.Add(value);
                    d.Add(value, bits);
                }
            }

            // add random values for 2-32 bits seperated by 1 bit 
            for (int j = 0; j < itemsPerBit; j++)
            {
                d.Add1(false);
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);
                    tmp = (short)(maxVal >> (i + 1));

                    // add random value
                    value = (short)rnd.NextLong(-tmp, tmp);
                    values.Add(value);
                    d.Add(value, bits);
                    d.Add1(false);
                }
            }

            BVector d2 = new BVector(d.ToBytes());

            // get values values for 2-32 bits
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);

                    // get max
                    expected = values[index];
                    val = d2.GetShort(bits);
                    Assert.AreEqual(expected, val);
                    index++;
                }
            }

            // get values for 2-32 bits seperated by 1 bit
            for (int j = 0; j < itemsPerBit; j++)
            {
                Assert.AreEqual(false, d2.Get1());
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);

                    // get max
                    expected = values[index];
                    val = d2.GetShort(bits);
                    Assert.AreEqual(expected, val);
                    Assert.AreEqual(false, d2.Get1());
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
        public void Ext_Short_Add_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((short)0, 1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is greater than 64
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_Short_Add_Exc_MoreThan16Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((short)0, 17);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetInt is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_Short_Get_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((short)0, 32);
            d.GetShort(1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetInt is greater than 32
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_Short_Get_Exc_MoreThan16Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((short)0, 16);
            d.GetShort(17);
        }
        #endregion

        #region ------------------------ long? ----------------------------------------------
        /// <summary>
        /// Test adding all max, min and null values that can fit in 2 -> 32 bits 
        /// in sequence and seperated by an extra bit and then getting them
        /// from the same BVector
        /// </summary>
        [TestMethod]
        public void Ext_ShortN_AddGet_BitsMinMax_Loopback()
        {
            int maxBits = 16;
            BVector d = new BVector();
            short? value;
            short? val;
            short? expected;
            short? nullVal = (short?)null;
            const short maxVal = short.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = (short)(maxVal >> i);
                // add max
                d.Add(value, bits);

                // add min
                d.Add(-value, bits);

                // add null
                d.Add(nullVal, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = (short)(maxVal >> i);
                // add max
                d.Add(value, bits);
                d.Add1(false);

                // add min
                d.Add(-value, bits);
                d.Add1(false);

                // add null
                d.Add(nullVal, bits);
                d.Add1(false);
            }

            BVector d2 = d;

            // get min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = (short)(maxVal >> i);
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);

                // get min
                expected = (short)-expected;
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);

                // get null
                expected = nullVal;
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);

            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = (short)(maxVal >> i);
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());

                // get min
                expected = (short)-expected;
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());

                // get null
                expected = nullVal;
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());
            }
        }

        /// <summary>
        /// Test adding all max, min and null values that can fit in 2 -> 32 bits 
        /// in sequence and seperated by an extra bit and then getting them
        /// from a new BVector intantiated with the byte[] from the first BVector
        /// </summary>
        [TestMethod]
        public void Ext_ShortN_AddGet_BitsMinMax_InOut()
        {
            int maxBits = 16;
            BVector d = new BVector();
            short? value;
            short? val;
            short? expected;
            short? nullVal = (short?)null;
            const short maxVal = short.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = (short)(maxVal >> i);
                // add max
                d.Add(value, bits);

                // add min
                d.Add(-value, bits);

                // add null
                d.Add(nullVal, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = (short)(maxVal >> i);
                // add max
                d.Add(value, bits);
                d.Add1(false);

                // add min
                d.Add(-value, bits);
                d.Add1(false);

                // add null
                d.Add(nullVal, bits);
                d.Add1(false);
            }

            BVector d2 = new BVector(d.ToBytes());

            // get min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = (short)(maxVal >> i);
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);

                // get min
                expected = (short)-expected;
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);

                // get null
                expected = nullVal;
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);

            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = (short)(maxVal >> i);
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());

                // get min
                expected = (short)-expected;
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());

                // get null
                expected = nullVal;
                val = d2.GetShortN(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());
            }
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_ShortN_Add_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((short?)0, 1);
        }


        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is greater than 32
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_ShortN_Add_Exc_MoreThan16Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((short?)0, 17);
        }


        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Get is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_ShortN_Get_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((short?)0, 16);
            d.GetShortN(1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Get is greater than 32
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_ShortN_Get_Exc_MoreThan16Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((short?)0, 16);
            d.GetShortN(17);
        }

        #endregion
    }
}
