using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antara.Data.Binary.Test
{
    [TestClass]
    public class TestCombinations
    {

        /// <summary>
        /// Tests adding 1000 random values of various types and getting them
        /// </summary>
        [TestMethod]
        public void Combo1()
        {
            object val = null;
            object testValue = null;
            BVector d = new BVector();
            int itemCount = 1000;
            object[] values = new object[itemCount];

            for (int i = 0; i < itemCount; i++)
            {
                val = BHelper.WriteRandomValue(d);
                values[i] = val;
            }

            for (int i = 0; i < itemCount; i++)
            {
                val = values[i];
                testValue = BHelper.ReadNextRandomValue(d);
                var valueType = val.GetType();

                // Assert.AreEqual cannot compare byte[] since it is using reference equality
                if (valueType.IsArray && valueType.GetElementType() == typeof(byte))
                {
                    Assert.IsTrue(BHelper.CompareByteArrays((byte[])val, (byte[])testValue));
                }
                else
                {
                    Assert.AreEqual(val, testValue);
                }
            }
        }
    }
}
