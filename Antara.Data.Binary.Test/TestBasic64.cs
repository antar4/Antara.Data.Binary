using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityDriven.Inferno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestBasic64
    {
        /// <summary>
        /// Tests adding 1000 random values by using <see cref="BVector.AddU32(uint, byte)"/>
        /// and retrieving them from the same <see cref="BVector"/> 
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_64_AddGet_Safe_Random_Loopback()
        {
            int maxBits = 64;
            int index = 0;
            byte bits;
            const long maxValue = long.MaxValue;
            long rngMax;
            ulong value;
            ulong expected;
            var rnd = new CryptoRandom();
            List<ulong> values = new List<ulong>();

            // number of items to add per bit
            int itemsCount = 1_000;
            BVector d = new BVector();

            for (int j = 2; j <= maxBits; j++)
            {
                rngMax = (maxValue >> (maxBits - j));
                bits = (byte)j;
                for (int i = 0; i < itemsCount; i++)
                {
                    value = (ulong)rnd.NextLong(rngMax) + (ulong)rnd.NextLong(rngMax) + (ulong)rnd.Next(2);
                    d.AddU64(value, bits);
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
                    value = d2.GetU64(bits);
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
        public void Basic_64_AddGet_UintMinMaxZero_Loopback()
        {
            List<uint> values = new List<uint>();
            BVector d = new BVector();
            d.AddU64(ulong.MinValue);
            d.AddU64(ulong.MaxValue);
            d.AddU64(0);

            // add 1 bit to test byte split
            d.Add1(false);
            d.AddU64(ulong.MinValue);
            d.AddU64(ulong.MaxValue);
            d.AddU64(0);

            Assert.AreEqual(ulong.MinValue, d.GetU64());
            Assert.AreEqual(ulong.MaxValue, d.GetU64());
            Assert.AreEqual((ulong)0, d.GetU64());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(ulong.MinValue, d.GetU64());
            Assert.AreEqual(ulong.MaxValue, d.GetU64());
            Assert.AreEqual((ulong)0, d.GetU64());
        }

        /// <summary>
        /// Tests adding the max value per number of bits by using <see cref="BVector.AddU32(uint, byte)"/>
        /// and retrieving them from the same <see cref="BVector"/> 
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_64_AddGet_Safe_BitMaxValues_Loopback()
        {
            List<uint> values = new List<uint>();
            BVector d = new BVector();
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(64 - i);
                d.AddU64(ulong.MaxValue >> i, bits);
            }

            // add 1 bit to test byte split
            d.Add1(false);
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(64 - i);
                d.AddU64(ulong.MaxValue >> i, bits);
                d.Add1(false);
            }

            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(64 - i);
                ulong expected = ulong.MaxValue >> i;
                ulong val = d.GetU64(bits);
                Assert.AreEqual(expected, val);
            }

            Assert.AreEqual(false, d.Get1());
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(64 - i);
                ulong expected = ulong.MaxValue >> i;
                ulong val = d.GetU64(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d.Get1());
            }
        }

        /// <summary>
        /// Tests adding 1000 random uint values by using <see cref="BVector.AddU64(uint, byte)"/>
        /// and retrieving them from a newly created <see cref="BVector"/> using the result of <see cref="BVector.ToBytes"/>
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_64_AddGet_Safe_Random_InOut()
        {
            var rnd = new CryptoRandom();
            List<ulong> values = new List<ulong>();
            ulong value;
            int valueCount = 1_000;
            BVector d = new BVector();
            for (int i = 0; i < valueCount; i++)
            {
                value = (ulong)rnd.NextLong(int.MaxValue, long.MaxValue);
                d.AddU64(value);
                values.Add(value);
                // add 1 bit to split bytes
                d.Add1(false);
            }

            BVector d2 = new BVector(d.ToBytes());

            for (int i = 0; i < valueCount; i++)
            {
                Assert.AreEqual(values[i], d2.GetU64());
                Assert.AreEqual(false, d2.Get1());
            }
        }

        /// <summary>
        /// Tests adding uint Min, Max and 0 values by using <see cref="BVector.AddU32(uint, byte)"/>
        /// and retrieving them from a newly created <see cref="BVector"/> by importing using the result of <see cref="BVector.ToBytes"/>
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_64_AddGet_UintMinMaxZero_InOut()
        {
            List<uint> values = new List<uint>();
            BVector d = new BVector();
            d.AddU64(ulong.MinValue);
            d.AddU64(ulong.MaxValue);
            d.AddU64(0);

            // add 1 bit to test byte split
            d.Add1(false);
            d.AddU64(ulong.MinValue);
            d.AddU64(ulong.MaxValue);
            d.AddU64(0);

            BVector d2 = new BVector(d.ToBytes());

            Assert.AreEqual(ulong.MinValue, d2.GetU64());
            Assert.AreEqual(ulong.MaxValue, d2.GetU64());
            Assert.AreEqual((ulong)0, d2.GetU64());
            Assert.AreEqual(false, d2.Get1());
            Assert.AreEqual(ulong.MinValue, d2.GetU64());
            Assert.AreEqual(ulong.MaxValue, d2.GetU64());
            Assert.AreEqual((ulong)0, d2.GetU64());
        }

        /// <summary>
        /// Tests adding the max value per number of bits by using <see cref="BVector.AddU32(uint, byte)"/>
        /// and retrieving them from the same <see cref="BVector"/> 
        /// by using <see cref="BVector.GetU32(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_64_AddGet_Safe_BitMaxValues_InOut()
        {
            List<uint> values = new List<uint>();
            BVector d = new BVector();
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(64 - i);
                d.AddU64(ulong.MaxValue >> i, bits);
            }

            // add 1 bit to test byte split
            d.Add1(false);
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(64 - i);
                d.AddU64(ulong.MaxValue >> i, bits);
                d.Add1(false);
            }

            BVector d2 = new BVector(d.ToBytes());

            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(64 - i);
                ulong expected = ulong.MaxValue >> i;
                ulong val = d2.GetU64(bits);
                Assert.AreEqual(expected, val);
            }

            Assert.AreEqual(false, d2.Get1());
            for (int i = 0; i < 32; i++)
            {
                byte bits = (byte)(64 - i);
                ulong expected = ulong.MaxValue >> i;
                ulong val = d2.GetU64(bits);
                Assert.AreEqual(expected, val);
                Assert.AreEqual(false, d2.Get1());
            }
        }


        /// <summary>
        /// Tests the internal redirect to AddU32 when the bits specified are equal or less to 32
        /// </summary>
        [TestMethod]
        public void Basic_64_AddGet_Safe_RedirectTo32_Loopback()
        {
            BVector d = new BVector();
            d.AddU64((ulong)1, 32);
            Assert.AreEqual((ulong)1, d.GetU64(32));
        }
    }
}
