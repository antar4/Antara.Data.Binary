using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityDriven.Inferno;
using System.Collections.Generic;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestExtendedULong
    {
        #region ------------------------ ulong ----------------------------------------------
        /// <summary>
        /// Test adding all max and min values that can fit in 2 -> 32 bits 
        /// in sequence and seperated by an extra bit and then getting them
        /// from the same BVector
        /// </summary>
        [TestMethod]
        public void Ext_ULong_AddGet_BitsMinMax_Loopback()
        {
            int maxBits = 64;
            BVector d = new BVector();
            ulong value;
            ulong val;
            ulong expected;
            ulong maxVal = ulong.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);
                d.Add1(false);
            }

            BVector d2 = d;

            // get min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetULong(bits);
                Assert.AreEqual(expected, val);
            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetULong(bits);
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
        public void Ext_ULong_AddGet_BitsMinMax_InOut()
        {
            int maxBits = 64;
            BVector d = new BVector();
            ulong value;
            ulong val;
            ulong expected;
            ulong maxVal = ulong.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);
                d.Add1(false);
            }

            BVector d2 = new BVector(d.ToBytes());

            // get min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetULong(bits);
                Assert.AreEqual(expected, val);
            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetULong(bits);
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
        public void Ext_ULong_AddGet_Rnd_InOut()
        {
            BVector d = new BVector();
            var rnd = new CryptoRandom();
            List<ulong> values = new List<ulong>();
            int maxBits = 64;
            int index = 0;
            int itemsPerBit = 100;
            ulong value;
            ulong val;
            ulong expected;
            long maxVal = long.MaxValue;
            long tmp;
            byte bits;


            // add random values for 2-32 bits
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);
                    tmp = maxVal >> (i + 1);

                    // add random value
                    value = (ulong)rnd.NextLong(0, tmp) + (ulong)rnd.NextLong(0, tmp);
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
                    tmp = maxVal >> (i + 1);

                    // add random value
                    value = (ulong)rnd.NextLong(0, tmp) + (ulong)rnd.NextLong(0, tmp);
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
                    val = d2.GetULong(bits);
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
                    val = d2.GetULong(bits);
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
        public void Ext_ULong_Add_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((ulong)0, 1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is greater than 64
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_ULong_Add_Exc_MoreThan64Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((ulong)0, 65);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetInt is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_ULong_Get_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add(0, 32);
            d.GetULong(1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetInt is greater than 32
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_ULong_Get_Exc_MoreThan32Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add(0, 32);
            d.GetULong(65);
        }
        #endregion

        #region ------------------------ ulong? ---------------------------------------------
        /// <summary>
        /// Test adding all max, min and null values that can fit in 2 -> 32 bits 
        /// in sequence and seperated by an extra bit and then getting them
        /// from the same BVector
        /// </summary>
        [TestMethod]
        public void Ext_ULongN_AddGet_BitsMinMax_Loopback()
        {
            int maxBits = 64;
            BVector d = new BVector();
            ulong? value;
            ulong? val;
            ulong? expected;
            ulong? nullVal = (ulong?)null;
            const ulong maxVal = ulong.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);

                // add null
                d.Add(nullVal, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
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
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetULongN(bits);
                Assert.AreEqual(expected, val);

                // get null
                expected = nullVal;
                val = d2.GetULongN(bits);
                Assert.AreEqual(expected, val);

            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetULongN(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());

                // get null
                expected = nullVal;
                val = d2.GetULongN(bits);
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
        public void Ext_ULongN_AddGet_BitsMinMax_InOut()
        {
            int maxBits = 64;
            BVector d = new BVector();
            ulong? value;
            ulong? val;
            ulong? expected;
            ulong? nullVal = (ulong?)null;
            const ulong maxVal = ulong.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                value = maxVal >> i;
                // add max
                d.Add(value, bits);

                // add null
                d.Add(nullVal, bits);
            }

            // add min and max values for 2-32 bits seperated by 1 bit 
            d.Add1(false);
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
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
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetULongN(bits);
                Assert.AreEqual(expected, val);

                // get null
                expected = nullVal;
                val = d2.GetULongN(bits);
                Assert.AreEqual(expected, val);

            }

            // get min and max values for 2-32 bits seperated by 1 bit
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get max
                expected = maxVal >> i;
                val = d2.GetULongN(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());

                // get null
                expected = nullVal;
                val = d2.GetULongN(bits);
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
        public void Ext_ULongN_Add_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((ulong?)0, 1);
        }


        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is greater than 32
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_ULongN_Add_Exc_MoreThan64Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((ulong?)0, 65);
        }


        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Get is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_ULongN_Get_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((ulong?)0, 32);
            d.GetULongN(1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Get is greater than 32
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Ext_ULongN_Get_Exc_MoreThan32Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((ulong?)0, 32);
            d.GetULongN(65);
        }

        #endregion
    }
}
