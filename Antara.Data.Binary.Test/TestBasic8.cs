using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityDriven.Inferno;
using System.Collections.Generic;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestBasic8
    {

        /// <summary>
        /// Tests adding 1000 random values by using <see cref="BVector.Add8(byte)"/>
        /// and retrieving them from the same <see cref="BVector"/> 
        /// by using <see cref="BVector.Get8(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_8_AddGet_Safe_Random_Loopback()
        {
            int index = 0;
            byte maxValue = byte.MaxValue;
            byte value;
            byte expected;
            var rnd = new CryptoRandom();
            List<byte> values = new List<byte>();

            // number of items to add per bit
            int itemsCount = 1_000;
            BVector d = new BVector();

            for (int i = 0; i < itemsCount; i++)
            {
                value = (byte)rnd.NextLong(maxValue);
                d.Add8(value);
                values.Add(value);
                // add 1 bit to split bytes
                d.Add1(false);
            }

            BVector d2 = d;

            for (int i = 0; i < itemsCount; i++)
            {
                value = d2.Get8();
                expected = values[index];
                Assert.AreEqual(expected, value);
                Assert.AreEqual(false, d2.Get1());
                index++;
            }
        }

        /// <summary>
        /// Tests adding 1000 random uint values by using <see cref="BVector.Add8(byte)"/>
        /// and retrieving them from a newly created <see cref="BVector"/> using the result of <see cref="BVector.ToBytes"/>
        /// by using <see cref="BVector.Get8(byte)"/>
        /// </summary>
        [TestMethod]
        public void Basic_8_AddGet_Safe_Random_InOut()
        {
            int index = 0;
            byte maxValue = byte.MaxValue;
            byte value;
            byte expected;
            var rnd = new CryptoRandom();
            List<byte> values = new List<byte>();

            // number of items to add per bit
            int itemsCount = 1_000;
            BVector d = new BVector();

            for (int i = 0; i < itemsCount; i++)
            {
                value = (byte)rnd.NextLong(maxValue);
                d.Add8(value);
                values.Add(value);
                // add 1 bit to split bytes
                d.Add1(false);
            }

            BVector d2 = new BVector(d.ToBytes());

            for (int i = 0; i < itemsCount; i++)
            {
                value = d2.Get8();
                expected = values[index];
                Assert.AreEqual(expected, value);
                Assert.AreEqual(false, d2.Get1());
                index++;
            }
        }
    }
}
