using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityDriven.Inferno;
using System.Collections.Generic;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestExtendedDecimal
    {
        /// <summary>
        /// Tests adding min, max and zero (0) decimal values
        /// </summary>
		[TestMethod]
        public void Decimal_MinMax()
        {
            BVector d = new BVector();
            decimal max = decimal.MaxValue;
            decimal min = decimal.MinValue;
            decimal zero = (decimal)0;

            d.Add(max);
            d.Add1(false);
            d.Add(zero);
            d.Add1(true);
            d.Add(min);

            Assert.AreEqual(max, d.GetDecimal());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(zero, d.GetDecimal());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(min, d.GetDecimal());

        }

        /// <summary>
        /// Tests adding min, max, zero (0) and null decimal values
        /// </summary>
        [TestMethod]
        public void DecimalN_MinMax()
        {
            BVector d = new BVector();
            decimal? max = decimal.MaxValue;
            decimal? min = decimal.MinValue;
            decimal? zero = (decimal)0;
            decimal? @null = null;


            d.Add(max);
            d.Add1(false);
            d.Add(@null);
            d.Add1(true);
            d.Add(zero);
            d.Add1(false);
            d.Add(min);

            Assert.AreEqual(max, d.GetDecimalN());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(@null, d.GetDecimalN());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(zero, d.GetDecimalN());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(min, d.GetDecimalN());
        }


        /// <summary>
        /// Tests adding and getting 2000 random (positive and negative) decimal values
        /// </summary>
        [TestMethod]
        public void Decimal_Rnd()
        {
            BVector d = new BVector();
            decimal value;
            const decimal maxValue = decimal.MaxValue;
            const decimal minValue = decimal.MinValue;
            var rnd = new CryptoRandom();
            List<decimal> values = new List<decimal>();

            // number of items to add per bit
            int itemsCount = 1_000;

            for (int i = 0; i < itemsCount; i++)
            {
                // add positive
                value = (decimal)rnd.NextDouble() * maxValue;
                d.Add(value);
                values.Add(value);
                d.Add1(false);

                // add negative
                value = (decimal)rnd.NextDouble() * minValue;
                d.Add(value);
                values.Add(value);
                d.Add1(true);
            }

            for (int i = 0; i < itemsCount; i++)
            {
                value = d.GetDecimal();
                Assert.AreEqual(values[i], value);
                Assert.AreEqual(i % 2 != 0, d.Get1());
            }
        }
    }
}
