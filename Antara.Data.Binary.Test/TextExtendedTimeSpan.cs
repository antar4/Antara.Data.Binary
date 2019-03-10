using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Antara.Data.Binary.Test
{
	[TestClass]
	public class TextExtendedTimeSpan
	{
        /// <summary>
        /// Tests adding and getting TimeSpan
        /// </summary>
		[TestMethod]
		public void TimeSpan_Basic()
		{
			TimeSpan ts = new TimeSpan(DateTime.Now.Ticks);
			BVector d = new BVector();
			d.Add(ts);

			Assert.AreEqual(ts, d.GetTimeSpan());
		}

        /// <summary>
        /// Tests adding and getting min and max TimeSpan values
        /// </summary>
        [TestMethod]
        public void TimeSpan_MinMax()
        {
            TimeSpan max = TimeSpan.MaxValue;
            TimeSpan min = TimeSpan.MinValue;
            BVector d = new BVector();
            d.Add(max);
            d.Add1(false);
            d.Add(min);

            Assert.AreEqual(max, d.GetTimeSpan());
            Assert.AreEqual(false, d.Get1());
            Assert.AreEqual(min, d.GetTimeSpan());
        }
    }
}
