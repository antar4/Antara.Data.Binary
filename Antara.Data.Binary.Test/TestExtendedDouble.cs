using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityDriven.Inferno;
using System.Collections.Generic;

namespace Antara.Data.Binary.Test
{
	[TestClass]
	public class TestExtendedDouble
	{

        /// <summary>
        /// Tests adding min, max and zero (0) double values
        /// </summary>
		[TestMethod]
		public void Double_MinMax()
		{
			BVector d = new BVector();
			double max = double.MaxValue;
			double min = double.MinValue;
            double zero = (double)0;
			d.Add(max);
            d.Add1(false);
            d.Add(zero);
            d.Add1(true);
			d.Add(min);

			Assert.AreEqual(max, d.GetDouble());
            Assert.AreEqual(false, d.Get1());
			Assert.AreEqual(zero, d.GetDouble());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(min, d.GetDouble());
        }

        /// <summary>
        /// Tests adding min, max, zero (0) and null double values
        /// </summary>
        [TestMethod]
        public void DoubleN_MinMax()
        {
            BVector d = new BVector();
            double? max = double.MaxValue;
            double? min = double.MinValue;
            double? zero = (double)0;
            double? @null = null;


            d.Add(max);
            d.Add1(false);
            d.Add(@null);
            d.Add1(true);
            d.Add(zero);
            d.Add1(false);
            d.Add(min);

            Assert.AreEqual(max, d.GetDoubleN());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(@null, d.GetDoubleN());
            Assert.AreEqual(true, d.Get1());
            Assert.AreEqual(zero, d.GetDoubleN());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(min, d.GetDoubleN());
        }

        /// <summary>
        /// Tests adding and getting 2000 random (positive and negative) double values
        /// </summary>
        [TestMethod]
        public void Double_Rnd()
        {
            BVector d = new BVector();
            double value;
            const double maxValue = double.MaxValue;
            const double minValue = double.MinValue;
            var rnd = new CryptoRandom();
            List<double> values = new List<double>();

            // number of items to add per bit
            int itemsCount = 1_000;

            for (int i = 0; i < itemsCount; i++)
            {
                // add positive
                value = (double)rnd.NextDouble() * maxValue;
                d.Add(value);
                values.Add(value);
                d.Add1(false);

                // add negative
                value = (double)rnd.NextDouble() * minValue;
                d.Add(value);
                values.Add(value);
                d.Add1(true);
            }

            for (int i = 0; i < itemsCount; i++)
            {
                value = d.GetDouble();
                Assert.AreEqual(values[i], value);
                Assert.AreEqual(i % 2 != 0, d.Get1());
            }
        }

    }
}
