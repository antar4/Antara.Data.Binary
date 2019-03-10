using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecurityDriven.Inferno;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestSerial
    {
        /// <summary>
        /// Tests adding and retrieve all ushort values using <see cref="BVector.SerialAdd(short)"/> and <see cref="BVector.SerialGet"/>
        /// </summary>
        [TestMethod]
        public void SerialAddGetAll()
        {
            var rnd = new CryptoRandom();
            // using 2 BVectors cause of the extensive length required
            BVector d = new BVector();
            BVector d2 = new BVector();
            for (ushort i = 0;i<ushort.MaxValue;i++)
            {
                if (i % 2 == 0)
                {
                    d.SerialAdd(i);
                    d.Add1(rnd.Next(100) < 50);
                } else
                {
                    d2.SerialAdd(i);
                    d2.Add1(rnd.Next(100) < 50);
                }
            }

            for (ushort i = 0; i < ushort.MaxValue; i++)
            {
                if (i % 2 == 0)
                {

                    Assert.AreEqual(d.SerialGet(), i);
                    d.Get1();
                } else
                {
                    Assert.AreEqual(d2.SerialGet(), i);
                    d2.Get1();
                }
            }
        }
    }
}
