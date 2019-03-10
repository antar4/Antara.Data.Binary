using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityDriven.Inferno;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antara.Data.Binary.Test
{
    /// <summary>
    /// Tests basic AddU32/GetU32 operations
    /// 1. Loopback => using the same object
    /// 2. InOut => by exporting to byte[] and reimporting
    /// </summary>
    [TestClass]
    public class TestBasic32
    {
        /// <summary>
        /// Tests adding 1000 random values by using <see cref="BVector.AddU32(uint, byte)"/>
        /// foreach bit between 2 - 32
        /// and retrieving them from the same <see cref="BVector"/> 
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_32_AddGet_Safe_Random_Loopback()
        {
            int maxBits = 32;
            int index = 0;
            byte bits;
            uint maxValue = 0;
            uint value;
            uint expected;
            var rnd = new CryptoRandom();
            List<uint> values = new List<uint>();
            
            // number of items to add per bit
            int itemsCount = 1_000;
            BVector d = new BVector();

            for (int j = 2; j <= maxBits; j++)
            {
                maxValue = (uint)Math.Pow(2, j);
                bits = (byte)j;
                for (int i = 0; i < itemsCount; i++)
                {
                    value = (uint)rnd.NextLong(maxValue);
                    d.AddU32(value, bits);
                    values.Add(value);
                    // add 1 bit to split bytes
                    d.Add1(false);
                }
            }

            BVector d2 = d;

            for (int j = 2; j <= maxBits; j++)
            {
                bits = (byte)j;
                for (int i = 0; i < itemsCount; i++)
                {
                    value = d2.GetU32(bits);
                    expected = values[index];
                    Assert.AreEqual(expected, value);
                    Assert.AreEqual(false, d2.Get1());
                    index++;
                }
            }
        }

        /// <summary>
        /// Tests adding uint Min, Max and 0 values by using <see cref="BVector.AddU32(uint, byte)"/>
        /// and retrieving them from the same <see cref="BVector"/> 
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_32_AddGet_UintMinMaxZero_Loopback()
        {
            List<uint> values = new List<uint>();
            BVector d = new BVector();
            d.AddU32(uint.MinValue);
            d.AddU32(uint.MaxValue);
            d.AddU32(0);

            // add 1 bit to test byte split
            d.Add1(false);
            d.AddU32(uint.MinValue);
            d.AddU32(uint.MaxValue);
            d.AddU32(0);

            Assert.AreEqual(uint.MinValue, d.GetU32());
            Assert.AreEqual(uint.MaxValue, d.GetU32());
            Assert.AreEqual((uint)0, d.GetU32());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(uint.MinValue, d.GetU32());
            Assert.AreEqual(uint.MaxValue, d.GetU32());
            Assert.AreEqual((uint)0, d.GetU32());
        }

        /// <summary>
        /// Tests adding the max value per number of bits by using <see cref="BVector.AddU32(uint, byte)"/>
        /// and retrieving them from the same <see cref="BVector"/> 
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_32_AddGet_Safe_BitMaxValues_Loopback()
        {
            List<uint> values = new List<uint>();
            BVector d = new BVector();
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(32 - i);
                d.AddU32(uint.MaxValue >> i, bits);
            }

            // add 1 bit to test byte split
            d.Add1(false);
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(32 - i);
                d.AddU32(uint.MaxValue >> i, bits);
                d.Add1(false);
            }

            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(32 - i);
                uint expected = uint.MaxValue >> i;
                uint val = d.GetU32(bits);
                Assert.AreEqual(expected, val);
            }

            Assert.AreEqual(false, d.Get1());
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(32 - i);
                uint expected = uint.MaxValue >> i;
                uint val = d.GetU32(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d.Get1());
            }
        }

        /// <summary>
        /// Tests adding 1000 random uint values by using <see cref="BVector.AddU32(uint, byte)"/>
        /// and retrieving them from a newly created <see cref="BVector"/> using the result of <see cref="BVector.ToBytes"/>
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_32_AddGet_Safe_Random_InOut()
        {
            int maxBits = 32;
            int index = 0;
            byte bits;
            uint maxValue = 0;
            uint value;
            uint expected;
            var rnd = new CryptoRandom();
            List<uint> values = new List<uint>();

            // number of items to add per bit
            int valueCount = 1_000;
            BVector d = new BVector();

            for (int j = 2; j < maxBits; j++)
            {
                maxValue = (uint)Math.Pow(2, j);
                bits = (byte)j;
                for (int i = 0; i < valueCount; i++)
                {
                    value = (uint)rnd.NextLong(maxValue);
                    d.AddU32(value, bits);
                    values.Add(value);
                    // add 1 bit to split bytes
                    d.Add1(false);
                }
            }

            BVector d2 = new BVector(d.ToBytes());

            for (int j = 2; j < maxBits; j++)
            {
                bits = (byte)j;
                for (int i = 0; i < valueCount; i++)
                {
                    value = d2.GetU32(bits);
                    expected = values[index];
                    Assert.AreEqual(expected, value);
                    Assert.AreEqual(false, d2.Get1());
                    index++;
                }
            }
        }

        /// <summary>
        /// Tests adding uint Min, Max and 0 values by using <see cref="BVector.AddU32(uint, byte)"/>
        /// and retrieving them from a newly created <see cref="BVector"/> by importing using the result of <see cref="BVector.ToBytes"/>
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_32_AddGet_UintMinMaxZero_InOut()
        {
            List<uint> values = new List<uint>();
            BVector d = new BVector();
            d.AddU32(uint.MinValue);
            d.AddU32(uint.MaxValue);
            d.AddU32(0);

            // add 1 bit to test byte split
            d.Add1(false);
            d.AddU32(uint.MinValue);
            d.AddU32(uint.MaxValue);
            d.AddU32(0);

            BVector d2 = new BVector(d.ToBytes());

            Assert.AreEqual(uint.MinValue, d2.GetU32());
            Assert.AreEqual(uint.MaxValue, d2.GetU32());
            Assert.AreEqual((uint)0, d2.GetU32());
            Assert.AreEqual(false, d2.Get1());
            Assert.AreEqual(uint.MinValue, d2.GetU32());
            Assert.AreEqual(uint.MaxValue, d2.GetU32());
            Assert.AreEqual((uint)0, d2.GetU32());
        }

        /// <summary>
        /// Tests adding the max value per number of bits by using <see cref="BVector.AddU32(uint, byte)"/>
        /// and retrieving them from the same <see cref="BVector"/> 
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_32_AddGet_Safe_BitMaxValues_InOut()
        {
            List<uint> values = new List<uint>();
            BVector d = new BVector();
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(32 - i);
                d.AddU32(uint.MaxValue >> i, bits);
            }

            // add 1 bit to test byte split - false to ensure there are 0 bits
            d.Add1(false);
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(32 - i);
                d.AddU32(uint.MaxValue >> i, bits);
                d.Add1(false);
            }

            BVector d2 = new BVector(d.ToBytes());

            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(32 - i);
                uint expected = uint.MaxValue >> i;
                uint val = d2.GetU32(bits);
                Assert.AreEqual(expected, val);
            }
            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(32 - i);
                uint expected = uint.MaxValue >> i;
                uint val = d2.GetU32(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());
            }
        }
    }
}
