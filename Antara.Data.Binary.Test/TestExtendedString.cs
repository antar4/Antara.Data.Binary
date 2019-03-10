using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antara.Data.Binary.Test
{
	[TestClass]
	public class TestExtendedString
	{

        /// <summary>
        /// Tests adding chars 0-255 using <see cref="BVector.Add(string)"/> and <see cref="BVector.GetString"/>
        /// </summary>
        [TestMethod]
		public void String_Basic256()
		{
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 256; i++)
            {
                sb.Append((char)i);
            }
            string s = sb.ToString();
			BVector d = new BVector();
			d.Add(s);
			Assert.AreEqual(s, d.GetString());
		}

        /// <summary>
        /// Tests adding chars 0-255 using <see cref="BVector.AddAscii(string)(string)"/> and <see cref="BVector.GetAscii"/>
        /// </summary>
        [TestMethod]
        public void Ascii_Basic256()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 256; i++)
            {
                sb.Append((char)i);
            }
            string s = sb.ToString();
            BVector d = new BVector();
            d.AddAscii(s);
            Assert.AreEqual(s, d.GetAscii());
        }


        /// <summary>
        /// Tests adding and reading all unicode characters
        /// </summary>
        [TestMethod]
        public void String_Unicode()
        {
            string unicodeChars = BHelper.GetUnicodeCharacters();
            BVector d = new BVector();
            d.Add(unicodeChars);
            string stored = d.GetString();
            Assert.AreEqual(unicodeChars, stored);
        }


    }
}
