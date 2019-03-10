using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Antara.Data.Binary;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using SecurityDriven.Inferno;

namespace Antara.Data.Binary.Test
{
    /// <summary>
    /// Tests basic Add1/Get1 operations
    /// 1. Loopback => using the same object
    /// 2. InOut => by exporting to byte[] and reimporting
    /// </summary>
    [TestClass]
    public class TestBasic1
    {
        /// <summary>
        /// Tests adding 1000 true values by using <see cref="BVector.Add1(bool)"/>
        /// and retrieving them from the same <see cref="BVector"/> by using <see cref="BVector.Get1"/>
        /// </summary>
        [TestMethod]
        public void Basic_1_AddGet_Safe_TrueSequence_Loopback()
        {
            int bitCount = 1_000;
            BVector d = new BVector();
            for (int i=0;i<bitCount;i++)
            {
                d.Add1(true);
            }

            List<bool> result = new List<bool>(bitCount);
            for (int i = 0; i < bitCount; i++)
            {
                Assert.AreEqual(true, d.Get1());
            }
        }

        /// <summary>
        /// Tests adding 1000 false values by using <see cref="BVector.Add1(bool)"/>
        /// and retrieving them from the same <see cref="BVector"/> by using <see cref="BVector.Get1"/>
        /// </summary>
        [TestMethod]
        public void Basic_1_AddGet_FalseSequence_Loopback()
        {
            int bitCount = 1_000;
            BVector d = new BVector();
            for (int i = 0; i < bitCount; i++)
            {
                d.Add1(false);
            }

            List<bool> result = new List<bool>(bitCount);
            for (int i = 0; i < bitCount; i++)
            {
                Assert.AreEqual(false, d.Get1());
            }
        }

        /// <summary>
        /// Tests adding 1000 alternating true/false values by using <see cref="BVector.Add1(bool)"/>
        /// and retrieving them from the same <see cref="BVector"/> by using <see cref="BVector.Get1"/>
        /// </summary>
        [TestMethod]
        public void Basic_1_AddGet_AlternatingSequence_Loopback()
        {
            bool value = false;
            int bitCount = 1_000;
            BVector d = new BVector();
            for (int i = 0; i < bitCount; i++)
            {
                d.Add1(!value);
            }


            value = false;
            List<bool> result = new List<bool>(bitCount);
            for (int i = 0; i < bitCount; i++)
            {
                Assert.AreEqual(!value, d.Get1());
            }
        }

        /// <summary>
        /// Tests adding 1000 random true/false values by using <see cref="BVector.Add1(bool)"/>
        /// and retrieving them from the same <see cref="BVector"/> by using <see cref="BVector.Get1"/>
        /// </summary>
        [TestMethod]
        public void Basic_1_AddGet_RandomSequence_Loopback()
        {
            var rnd = new CryptoRandom();
            bool value;
            List<bool> values = new List<bool>();
            int bitCount = 1_000;

            BVector d = new BVector();
            // create sequenece and add
            for (int i = 0; i < bitCount; i++)
            {
                value = rnd.Next(100) > 50;
                values.Add(value);
                d.Add1(value);
            }

            // verify
            for (int i = 0; i < bitCount; i++)
            {
                Assert.AreEqual(values[i], d.Get1());
            }
        }

        /// <summary>
        /// Tests adding 1000 true values by using <see cref="BVector.Add1(bool)"/>
        /// and retrieving them from a new BVector created using the ToBytes() value of the <see cref="BVector"/> 
        /// using <see cref="BVector.Get1"/>
        /// </summary>
        [TestMethod]
        public void Basic_1_AddGet_Safe_TrueSequence_InOut()
        {
            int bitCount = 1_000;
            BVector d = new BVector();
            for (int i = 0; i < bitCount; i++)
            {
                d.Add1(true);
            }
            
            BVector d2 = new BVector(d.ToBytes());
            Assert.AreEqual(d.ToBytes().SequenceEqual(d2.ToBytes()), true);

            List<bool> result = new List<bool>(bitCount);
            for (int i = 0; i < bitCount; i++)
            {
                Assert.AreEqual(true, d2.Get1());
            }
        }

        /// <summary>
        /// Tests adding 1000 false values by using <see cref="BVector.Add1(bool)"/>
        /// and retrieving them from a new BVector created using the ToBytes() value of the <see cref="BVector"/> 
        /// using <see cref="BVector.Get1"/>
        /// </summary>
        [TestMethod]
        public void Basic_1_AddGet_FalseSequence_InOut()
        {
            int bitCount = 1_000;
            BVector d = new BVector();
            for (int i = 0; i < bitCount; i++)
            {
                d.Add1(false);
            }

            BVector d2 = new BVector(d.ToBytes());
            Assert.AreEqual(d.ToBytes().SequenceEqual(d2.ToBytes()), true);


            List<bool> result = new List<bool>(bitCount);
            for (int i = 0; i < bitCount; i++)
            {
                Assert.AreEqual(false, d2.Get1());
            }
        }

        /// <summary>
        /// Tests adding 1000 alternating true/false values by using <see cref="BVector.Add1(bool)"/>
        /// and retrieving them from a new BVector created using the ToBytes() value of the <see cref="BVector"/> 
        /// using <see cref="BVector.Get1"/>
        /// </summary>
        [TestMethod]
        public void Basic_1_AddGet_AlternatingSequence_InOut()
        {
            bool value = false;
            int bitCount = 1_000;
            BVector d = new BVector();
            for (int i = 0; i < bitCount; i++)
            {
                d.Add1(!value);
            }

            BVector d2 = new BVector(d.ToBytes());
            Assert.AreEqual(d.ToBytes().SequenceEqual(d2.ToBytes()), true);


            value = false;
            List<bool> result = new List<bool>(bitCount);
            for (int i = 0; i < bitCount; i++)
            {
                Assert.AreEqual(!value, d2.Get1());
            }
        }

        /// <summary>
        /// Tests adding 1000 random true/false values by using <see cref="BVector.Add1(bool)"/>
        /// and retrieving them from a new BVector created using the ToBytes() value of the <see cref="BVector"/> 
        /// using <see cref="BVector.Get1"/>
        /// </summary>
        [TestMethod]
        public void Basic_1_AddGet_RandomSequence_InOut()
        {
            var rnd = new CryptoRandom();
            bool value;
            List<bool> values = new List<bool>();
            int bitCount = 1_000;

            BVector d = new BVector();
            // create sequenece and add
            for (int i = 0; i < bitCount; i++)
            {
                value = rnd.Next(100) > 50;
                values.Add(value);
                d.Add1(value);
            }

            BVector d2 = new BVector(d.ToBytes());
            Assert.AreEqual(d.ToBytes().SequenceEqual(d2.ToBytes()), true);


            // verify
            for (int i = 0; i < bitCount; i++)
            {
                Assert.AreEqual(values[i], d2.Get1());
            }
        }
    }

}
