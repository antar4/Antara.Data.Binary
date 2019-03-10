using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityDriven.Inferno;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestDynamicSigned
    {
        /// <summary>
        /// Tests adding and getting 1.000.000 random long values
        /// and long min, max and zero(0) values
        /// by using <see cref="BVector.DynamicAdd(long)"/> and <see cref="BVector.DynamicGetLong"/>
        /// </summary>
        [TestMethod]
        public void DynamicLong()
        {
            var rnd = new CryptoRandom();
            long itemCount = 1_000_000;
            long value = 0;
            long[] values = new long[itemCount];
            BVector d = new BVector();
            for (long i = 0; i < itemCount; i++)
            {
                value = (long)rnd.NextLong(long.MinValue, long.MaxValue);
                d.DynamicAdd(value);
                d.Add1();
                values[i] = value;
            }
            d.DynamicAdd(long.MinValue);
            d.Add1();
            d.DynamicAdd(long.MaxValue);
            d.Add1();
            d.DynamicAdd((long)0);


            for (uint i = 0; i < itemCount; i++)
            {
                Assert.AreEqual(values[i], d.DynamicGetLong());
                Assert.AreEqual(true, d.Get1());
            }
            Assert.AreEqual(long.MinValue, d.DynamicGetLong());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(long.MaxValue, d.DynamicGetLong());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual((long)0, d.DynamicGetLong());

        }

        /// <summary>
        /// Tests adding and getting 1.000.000 random int values
        /// and int min, max and zero(0) values
        /// by using <see cref="BVector.DynamicAdd(int)"/> and <see cref="BVector.DynamicGetInt"/>
        /// </summary>
        [TestMethod]
        public void DynamicInt()
        {
            var rnd = new CryptoRandom();
            int itemCount = 1_000_000;
            int value = 0;
            int[] values = new int[itemCount];
            BVector d = new BVector();
            for (int i = 0; i < itemCount; i++)
            {
                value = (int)rnd.NextLong(int.MinValue, int.MaxValue);
                d.DynamicAdd(value);
                d.Add1();
                values[i] = value;
            }
            d.DynamicAdd(int.MinValue);
            d.Add1();
            d.DynamicAdd(int.MaxValue);
            d.Add1();
            d.DynamicAdd((int)0);


            for (uint i = 0; i < itemCount; i++)
            {
                Assert.AreEqual(values[i], d.DynamicGetInt());
                Assert.AreEqual(true, d.Get1());
            }
            Assert.AreEqual(int.MinValue, d.DynamicGetInt());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(int.MaxValue, d.DynamicGetInt());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual((int)0, d.DynamicGetInt());

        }

        /// <summary>
        /// Tests adding and getting all possible short values
        /// and short min, max and zero(0) values
        /// by using <see cref="BVector.DynamicAdd(short)"/> and <see cref="BVector.DynamicGetShort"/>
        /// </summary>
        [TestMethod]
        public void DynamicShort()
        {
            var rnd = new CryptoRandom();
            short value = 0;
            short[] values = new short[ushort.MaxValue];
            int index = 0;
            BVector d = new BVector();
            for (int i = short.MinValue; i < short.MaxValue; i++)
            {
                value = (short)rnd.NextLong(short.MinValue, short.MaxValue);
                d.DynamicAdd(value);
                d.Add1();
                values[index++] = (short)value;
            }
            d.DynamicAdd(short.MinValue);
            d.Add1();
            d.DynamicAdd(short.MaxValue);
            d.Add1();
            d.DynamicAdd((short)0);

            index = 0;
            for (int i = 0; i < values.Length; i++)
            {
                Assert.AreEqual(values[i], d.DynamicGetShort());
                Assert.AreEqual(true, d.Get1());
            }
            Assert.AreEqual(short.MinValue, d.DynamicGetShort());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(short.MaxValue, d.DynamicGetShort());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual((short)0, d.DynamicGetShort());

        }

        /// <summary>
        /// Tests adding and getting all possible sbyte values
        /// and ushort min (0) and max values
        /// by using <see cref="BVector.DynamicAdd(Sbyte)"/> and <see cref="BVector.DynamicGetSByte"/>
        /// </summary>
        [TestMethod]
        public void DynamicSByte()
        {
            var rnd = new CryptoRandom();
            int itemCount = byte.MaxValue;
            sbyte value = 0;
            sbyte[] values = new sbyte[itemCount];
            BVector d = new BVector();
            for (uint i = 0; i < itemCount; i++)
            {
                value = (sbyte)rnd.NextLong(sbyte.MinValue, sbyte.MaxValue);
                d.DynamicAdd(value);
                d.Add1();
                values[i] = value;
            }
            d.DynamicAdd(sbyte.MinValue);
            d.Add1();
            d.DynamicAdd(sbyte.MaxValue);
            d.Add1();
            d.DynamicAdd((sbyte)0);

            for (uint i = 0; i < itemCount; i++)
            {
                Assert.AreEqual(values[i], d.DynamicGetSByte());
                Assert.AreEqual(true, d.Get1());
            }
            Assert.AreEqual(sbyte.MinValue, d.DynamicGetSByte());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(sbyte.MaxValue, d.DynamicGetSByte());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual((sbyte)0, d.DynamicGetSByte());
        }

    }
}
