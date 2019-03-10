using SecurityDriven.Inferno;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antara.Data.Binary.Test
{
    public static class BHelper
    {
        /// <summary>
        /// Compares two byte arrays
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static bool CompareByteArrays(byte[] b1, byte[] b2)
        {
            return b1.SequenceEqual(b2);
        }


        /// <summary>
        /// Returns all unicode characters
        /// </summary>
        /// <returns></returns>
        public static string GetUnicodeCharacters()
        {
            // file from http://unicode.org/Public/UNIDATA/UnicodeData.txt
            string definedCodePoints = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UnicodeData.txt"));
            System.IO.StringReader reader = new System.IO.StringReader(definedCodePoints);
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();

            StringBuilder sb = new StringBuilder();

            while (true)
            {
                string line = reader.ReadLine();
                if (line == null) break;
                int codePoint = Convert.ToInt32(line.Substring(0, line.IndexOf(";")), 16);
                if (codePoint >= 0xD800 && codePoint <= 0xDFFF)
                {
                    //surrogate boundary; not valid codePoint, but listed in the document
                }
                else
                {
                    string utf16 = char.ConvertFromUtf32(codePoint);
                    sb.Append(utf16);
                    byte[] utf8 = encoder.GetBytes(utf16);
                    //TODO: something with the UTF-8-encoded character
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Adds a random value to the specified BVector
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static object WriteRandomValue(BVector d)
        {

            int stringLen = 0;
            StringBuilder sb;
            string unicodeChars;

            object result = null;
            var rnd = new CryptoRandom();
            int valueType = rnd.Next(12);
            bool nullable = rnd.Next(2) == 1;

            d.Add1(nullable);
            d.Add8((byte)valueType);

            switch (valueType)
            {
                case 0: // int
                    result = rnd.Next(int.MinValue, int.MaxValue);
                    if (nullable) d.Add((int?)result);
                    else d.Add((int)result);
                    break;
                case 1: // long
                    result = rnd.NextLong(long.MinValue, long.MaxValue);
                    if (nullable) d.Add((long?)result);
                    else d.Add((long)result);
                    break;
                case 2: // short
                    result = (short)rnd.Next(short.MinValue, short.MaxValue);
                    if (nullable) d.Add((short?)result);
                    else d.Add((short)result);
                    break;
                case 3: // byte
                    result = (byte)rnd.Next(byte.MinValue, byte.MaxValue);
                    if (nullable) d.Add((byte?)result);
                    else d.Add((byte)result);
                    break;
                case 4: // string (unicode)
                    stringLen = rnd.Next(0, 1000);
                    sb = new StringBuilder();
                    unicodeChars = GetUnicodeCharacters();
                    for (int i = 0; i < stringLen; i++)
                    {
                        int index = rnd.Next(unicodeChars.Length);
                        sb.Append(unicodeChars[index]);
                    }
                    result = sb.ToString();
                    d.Add((string)result);
                    break;
                case 5: // ascii
                    stringLen = rnd.Next(0, 1000);
                    sb = new StringBuilder();
                    for (int i = 0; i < stringLen; i++)
                    {
                        int ascii = rnd.Next(255);
                        sb.Append((char)ascii);
                    }
                    result = sb.ToString();
                    d.AddAscii((string)result);
                    break;
                case 6: // DateTime
                    result = new DateTime(rnd.NextLong(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks));
                    if (nullable) d.Add((DateTime?)result);
                    else d.Add((DateTime)result);
                    break;
                case 7: // decimal
                    result = (decimal)rnd.NextDouble() * decimal.MaxValue;
                    if (nullable) d.Add((decimal?)result);
                    else d.Add((decimal)result);
                    break;
                case 8: // double
                    result = rnd.NextDouble() * double.MaxValue;
                    if (nullable) d.Add((double?)result);
                    else d.Add((double)result);
                    break;
                case 9: // bool
                    result = rnd.Next(2) == 1;
                    d.Add1((bool)result);
                    break;
                case 10: // TimeSpan
                    result = new TimeSpan(rnd.NextLong(TimeSpan.MinValue.Ticks, TimeSpan.MaxValue.Ticks));
                    d.Add((TimeSpan)result);
                    break;
                case 11: // byte[]
                    result = rnd.NextBytes(rnd.Next(1000));
                    d.Add((byte[])result);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Reads the next random value added by <see cref="WriteRandomValue(BVector)"/>
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static object ReadNextRandomValue(BVector d)
        {
            bool nullable = d.Get1();
            byte valueType = d.Get8();
            switch (valueType)
            {
                case 0:
                    if (nullable) return d.GetIntN();
                    return d.GetInt();
                case 1:
                    if (nullable) return d.GetLongN();
                    return d.GetLong();
                case 2:
                    if (nullable) return d.GetShortN();
                    return d.GetShort();
                case 3:
                    if (nullable) return d.GetByteN();
                    return d.GetByte();
                case 4:
                    return d.GetString();
                case 5:
                    return d.GetAscii();
                case 6:
                    if (nullable) return d.GetDateTimeN();
                    return d.GetDateTime();
                case 7:
                    if (nullable) return d.GetDecimalN();
                    return d.GetDecimal();
                case 8:
                    if (nullable) return d.GetDoubleN();
                    return d.GetDouble();
                case 9:
                    return d.Get1();
                case 10:
                    return d.GetTimeSpan();
                case 11:
                    return d.GetByteArray();
            }
            return null;
        }

        public static string GetIntBinaryString(int n)
        {
            // Displays bits.
            char[] b = new char[32];
            int pos = 31;
            int i = 0;

            while (i < 32)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }

        /// <summary>
        /// Calculate the amount of bytes required for the mantissa
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static int GetBytesRequiredForMantissa(decimal v)
        {
            var bytes = decimal.GetBits(v);
            return 0;


        }

        /// <summary>
        /// returns the index of the first bit set in the integer starting from the rightmost side
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static int IndexOfFirstSetBitRight(int v)
        {
            int i = 0;

            while (i < 32)
            {
                if ((v & (1 << i)) != 0)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }


        public static int IndexOfFirstSetBitLeft(int v)
        {
            int i = 31;
            while (i >= 0)
            {
                if ((v & (1 << i)) != 0)
                {
                    return 31-i;
                }
                i--;
            }
            return -1;
        }
    }
}
