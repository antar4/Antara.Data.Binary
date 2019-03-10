using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antara.Data.Binary.Test
{

	[TestClass]
	public class TestExtendedDateTime
	{

        /// <summary>
        /// Tests adding and getting the current datetime
        /// </summary>
		[TestMethod]
		public void DateTime_Current()
		{
			DateTime dt = DateTime.Now;
			BVector d = new BVector();
			d.Add(dt);

			Assert.AreEqual(dt, d.GetDateTime());
		}

        /// <summary>
        /// Tests adding and getting min and max DateTime values
        /// </summary>
        [TestMethod]
        public void DateTime_MinMax()
        {
            DateTime max = DateTime.MaxValue;
            DateTime min = DateTime.MinValue;
            BVector d = new BVector();
            d.Add(max);
            d.Add1(false);
            d.Add(min);

            Assert.AreEqual(max, d.GetDateTime());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(min, d.GetDateTime());
        }

        /// <summary>
        /// Tests adding and getting the current datetime and a null datetime
        /// </summary>
		[TestMethod]
        public void DateTimeN_Current()
        {
            DateTime? dt = DateTime.Now;
            DateTime? @null = null;
            BVector d = new BVector();

            d.Add(@null);
            d.Add1(false);
            d.Add(dt);


            Assert.AreEqual(@null, d.GetDateTimeN());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(dt, d.GetDateTimeN());
        }

        /// <summary>
        /// Tests adding and getting min, max and null DateTime values
        /// </summary>
        [TestMethod]
        public void DateTimeN_MinMax()
        {
            DateTime? max = DateTime.MaxValue;
            DateTime? min = DateTime.MinValue;
            DateTime? @null = null;
            BVector d = new BVector();
            d.Add(max);
            d.Add1(false);
            d.Add(@null);
            d.Add1(false);
            d.Add(min);

            Assert.AreEqual(max, d.GetDateTimeN());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(@null, d.GetDateTimeN());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(min, d.GetDateTimeN());
        }
    }
}
