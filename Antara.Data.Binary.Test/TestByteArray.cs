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
    public class TestByteArray
    {
        /// <summary>
        /// Tests adding a 1000 bytes byte[] and getting it
        /// </summary>
        [TestMethod]
        public void ByteArray_Basic()
        {
            var rnd = new CryptoRandom();
            int itemsCount = 1_000;
            BVector d = new BVector();
            byte[] values = rnd.NextBytes(itemsCount);

            d.Add(values);
            Assert.IsTrue(BHelper.CompareByteArrays(values, d.GetByteArray()));
        }
    }
}
