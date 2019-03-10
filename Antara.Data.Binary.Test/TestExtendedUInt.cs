using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SecurityDriven.Inferno;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestExtendedUInt
    {
        #region ------------------------ uint -----------------------------------------------
        /// <summary>
        /// Test adding all max and min values that can fit in 2 -> 32 bits 
        /// in sequence and seperated by an extra bit and then getting them
        /// from the same BVector
        /// </summary>
        [TestMethod]
        public void Ext_UInt_AddGet_BitsMax_Loopback()
        {
            BVector d = new BVector();
            uint value;
            uint val;
            uint expected;
            uint maxVal = uint.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);
                d.Add1(false);
            }

            BVector d2 = d;

            // get min and max values for 2-32 bits
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetUInt(bits);
                Assert.AreEqual(expected, val);
            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetUInt(bits);
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
        public void Ext_UInt_AddGet_BitsMax_InOut()
        {
            BVector d = new BVector();
            uint value;
            uint val;
            uint expected;
            uint maxVal = uint.MaxValue;
            byte bits;


            // add max values for 2-32 bits
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);
            }

            // add max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);
                d.Add1(false);
            }

            BVector d2 = new BVector(d.ToBytes());

            // get max values for 2-32 bits
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetUInt(bits);
                Assert.AreEqual(expected, val);
            }

            // get max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetUInt(bits);
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
        public void Ext_UInt_AddGet_Rnd_InOut()
        {
            int maxBits = 32;
            int index = 0;
            byte bits;
            uint value;
            uint expected;
            const uint maxValue = uint.MaxValue;
            uint rngMax;
            var rnd = new CryptoRandom();
            List<uint> values = new List<uint>();

            // number of items to add per bit
            int itemsCount = 1_000;
            BVector d = new BVector();

            for (int j = 2; j <= maxBits; j++)
            {
                rngMax = maxValue >> (maxBits - j);
                bits = (byte)j;
                for (int i = 0; i < itemsCount; i++)
                {
                    // add positive
                    value = (uint)rnd.NextLong(rngMax);
                    d.Add(value, bits);
                    values.Add(value);
                    d.Add1(false);
                }
            }

            BVector d2 = d;

            for (int j = 2; j < maxBits; j++)
            {
                bits = (byte)j;
                for (int i = 0; i < itemsCount; i++)
                {
                    // read positive
                    value = d2.GetUInt(bits);
                    expected = values[index];
                    Assert.AreEqual(expected, value);
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
        public void Ext_UInt_Add_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((uint)0, 1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is greater than 32
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_UInt_Add_Exc_MoreThan32Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((uint)0, 33);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetInt is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_UInt_Get_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add(0, 32);
            d.GetUInt(1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetInt is greater than 32
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_UInt_Get_Exc_MoreThan32Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add(0, 32);
            d.GetUInt(33);
        }
        #endregion

        #region ------------------------ uint? ----------------------------------------------
        /// <summary>
        /// Test adding all max, min and null values that can fit in 2 -> 32 bits 
        /// in sequence and seperated by an extra bit and then getting them
        /// from the same BVector
        /// </summary>
        [TestMethod]
        public void Ext_UIntN_AddGet_BitsMax_Loopback()
        {
            BVector d = new BVector();
            uint? value;
            uint? val;
            uint? expected;
            uint? maxVal = int.MaxValue;
            uint? nullVal = null;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);

                // add null
                d.Add(nullVal, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);
                d.Add1(false);

                // add null
                d.Add(nullVal, bits);
                d.Add1(false);
            }

            BVector d2 = d;

            // get min and max values for 2-32 bits
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetUIntN(bits);
                Assert.AreEqual(expected, val);

                // get null
                expected = nullVal;
                val = d2.GetUIntN(bits);
                Assert.AreEqual(expected, val);
            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetUIntN(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());

                // get min
                expected = nullVal;
                val = d2.GetUIntN(bits);
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
        public void Ext_UIntN_AddGet_BitsMax_InOut()
        {
            BVector d = new BVector();
            uint? value;
            uint? val;
            uint? expected;
            uint? maxVal = int.MaxValue;
            uint? nullVal = null;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);

                // add null
                d.Add(nullVal, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);
                d.Add1(false);

                // add null
                d.Add(nullVal, bits);
                d.Add1(false);

            }

            BVector d2 = new BVector(d.ToBytes());

            // get min and max values for 2-32 bits
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetUIntN(bits);
                Assert.AreEqual(expected, val);

                // get null
                expected = nullVal;
                val = d2.GetUIntN(bits);
                Assert.AreEqual(expected, val);
            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < 31; i++)
            {
                bits = (byte)(32 - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetUIntN(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());

                // get min
                expected = nullVal;
                val = d2.GetUIntN(bits);
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
        public void Ext_UIntN_Add_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((uint?)0, 1);
        }


        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is greater than 32
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_UIntN_Add_Exc_MoreThan32Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((uint?)0, 33);
        }


        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Get is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_UIntN_Get_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((uint?)0, 32);
            d.GetUIntN(1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Get is greater than 32
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_UIntN_Get_Exc_MoreThan32Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((uint?)0, 32);
            d.GetUIntN(33);
        }

        #endregion
    }
}
