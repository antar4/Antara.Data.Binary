using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SecurityDriven.Inferno;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestDynamicUnsigned
    {
        /// <summary>
        /// Tests adding and getting 1.000.000 random ulong values
        /// and ulong min (0) and max values
        /// by using <see cref="BVector.DynamicAdd(ulong)"/> and <see cref="BVector.DynamicGetULong"/>
        /// </summary>
        [TestMethod]
        public void DynamicULong()
        {
            var rnd = new CryptoRandom();
            ulong itemCount = 1_000_000;
            ulong value = 0;
            ulong[] values = new ulong[itemCount];
            BVector d = new BVector();
            for (ulong i = 0; i < itemCount; i++)
            {
                value = (ulong)rnd.NextLong(0, long.MaxValue);
                d.DynamicAdd(value);
                d.Add1();
                values[i] = value;
            }
            d.DynamicAdd(ulong.MinValue);
            d.Add1();
            d.DynamicAdd(ulong.MaxValue);

            for (uint i = 0; i < itemCount; i++)
            {
                Assert.AreEqual(values[i], d.DynamicGetULong());
                Assert.AreEqual(true, d.Get1());
            }
            Assert.AreEqual(ulong.MinValue, d.DynamicGetULong());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(ulong.MaxValue, d.DynamicGetULong());
        }

        /// <summary>
        /// Tests adding and getting 1.000.000 random uint values
        /// and uint min (0) and max values
        /// by using <see cref="BVector.DynamicAdd(uint)"/> and <see cref="BVector.DynamicGetUInt"/>
        /// </summary>
        [TestMethod]
        public void DynamicUInt()
        {
            var rnd = new CryptoRandom();
            int itemCount = 1_000_000;
            uint value = 0;
            uint[] values = new uint[itemCount];
            BVector d = new BVector();
            for (uint i = 0; i < itemCount; i++)
            {
                value = (uint)rnd.NextLong(0, uint.MaxValue);
                d.DynamicAdd(value);
                d.Add1();
                values[i] = value;
            }
            d.DynamicAdd(uint.MinValue);
            d.Add1();
            d.DynamicAdd(uint.MaxValue);

            for (uint i = 0; i < itemCount; i++)
            {
                Assert.AreEqual(values[i], d.DynamicGetUInt());
                Assert.AreEqual(true, d.Get1());
            }
            Assert.AreEqual(uint.MinValue, d.DynamicGetUInt());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(uint.MaxValue, d.DynamicGetUInt());
        }


        /// <summary>
        /// Tests adding and getting all possible ushort values
        /// and ushort min (0) and max values
        /// by using <see cref="BVector.DynamicAdd(ushort)"/> and <see cref="BVector.DynamicGetUShort"/>
        /// </summary>
        [TestMethod]
        public void DynamicUShort()
        {
            var rnd = new CryptoRandom();
            int itemCount = ushort.MaxValue;
            ushort value = 0;
            ushort[] values = new ushort[itemCount];
            BVector d = new BVector();
            for (uint i = 0; i < itemCount; i++)
            {
                value = (ushort)rnd.NextLong(0, ushort.MaxValue);
                d.DynamicAdd(value);
                d.Add1();
                values[i] = value;
            }
            d.DynamicAdd(ushort.MinValue);
            d.Add1();
            d.DynamicAdd(ushort.MaxValue);

            for (uint i = 0; i < itemCount; i++)
            {
                Assert.AreEqual(values[i], d.DynamicGetUShort());
                Assert.AreEqual(true, d.Get1());
            }
            Assert.AreEqual(ushort.MinValue, d.DynamicGetUShort());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(ushort.MaxValue, d.DynamicGetUShort());
        }

        /// <summary>
        /// Tests adding and getting all possible byte values
        /// and byte min (0) and max values
        /// by using <see cref="BVector.DynamicAdd(byte)"/> and <see cref="BVector.DynamicGetByte"/>
        /// </summary>
        [TestMethod]
        public void DynamicByte()
        {
            var rnd = new CryptoRandom();
            int itemCount = byte.MaxValue;
            byte value = 0;
            byte[] values = new byte[itemCount];
            BVector d = new BVector();
            for (uint i = 0; i < itemCount; i++)
            {
                value = (byte)rnd.NextLong(0, byte.MaxValue);
                d.DynamicAdd(value);
                d.Add1();
                values[i] = value;
            }
            d.DynamicAdd(byte.MinValue);
            d.Add1();
            d.DynamicAdd(byte.MaxValue);

            for (uint i = 0; i < itemCount; i++)
            {
                Assert.AreEqual(values[i], d.DynamicGetByte());
                Assert.AreEqual(true, d.Get1());
            }
            Assert.AreEqual(byte.MinValue, d.DynamicGetByte());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(byte.MaxValue, d.DynamicGetByte());
        }
    }
}
