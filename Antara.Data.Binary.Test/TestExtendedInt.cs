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
    public class TestExtendedInt
    {
        #region ------------------------ int ------------------------------------------------
        /// <summary>
        /// Test adding all max and min values that can fit in 2 to 32 bits 
        /// in sequence and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void Int_AddGet_BitsMinMax_Sequence_Loopback()
        {
            int maxBits = 32;
            List<int> values = new List<int>();
            BVector d = new BVector();
            int positiveValue;
            int negativeValue;
            int actualValue;
            int index;
            int expected;
            int maxVal = int.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                positiveValue = maxVal >> i;
                negativeValue = -positiveValue - 1;

                // add positive
                d.Add(positiveValue, bits);
                values.Add(positiveValue);

                // add negative
                d.Add(negativeValue, bits);
                values.Add(negativeValue);
            }


            // get min and max values for 2-32 bits
            index = 0;
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get positive
                expected = values[index++];
                actualValue = d.GetInt(bits);
                Assert.AreEqual(expected, actualValue);

                // get min
                expected = values[index++];
                actualValue = d.GetInt(bits);
                Assert.AreEqual(expected, actualValue);
            }
        }

        /// <summary>
        /// Test adding all max and min values that can fit in 2 to 32 bits 
        /// separated by 1 bit values and and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void Int_AddGet_BitsMinMax_Separated_Loopback()
        {
            int maxBits = 32;
            List<int> values = new List<int>();
            BVector d = new BVector();
            int positiveValue;
            int negativeValue;
            int actualValue;
            int index;
            int expected;
            int maxVal = int.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                positiveValue = maxVal >> i;
                negativeValue = -positiveValue - 1;

                // add positive
                d.Add(positiveValue, bits);
                values.Add(positiveValue);

                // true separator
                d.Add1();

                // add negative
                d.Add(negativeValue, bits);
                values.Add(negativeValue);

                // false separator
                d.Add1(false);
            }


            // get min and max values for 2-32 bits
            index = 0;
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get positive
                expected = values[index++];
                actualValue = d.GetInt(bits);
                Assert.AreEqual(expected, actualValue);

                // true separator
                Assert.AreEqual(true, d.Get1());

                // get min
                expected = values[index++];
                actualValue = d.GetInt(bits);
                Assert.AreEqual(expected, actualValue);

                // false separator
                Assert.AreEqual(false, d.Get1());
            }
        }


        /// <summary>
        /// Test adding 10000 random values  
        /// in sequence and and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void Int_AddGet_Rnd_Sequence_Loopback()
        {
            int maxBits = 32;
            List<int> values = new List<int>();
            BVector d = new BVector();
            int actualValue;
            int index;
            int expected;
            int maxVal = int.MaxValue;
            byte bits;
            var rnd = new CryptoRandom();
            int itemsPerBit = 10000;
            int value;
            int tmp;


            // add random values for 2-32 bits
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);
                    tmp = maxVal >> (i + 1);

                    // add random value
                    value = rnd.Next(-tmp - 1, tmp);
                    values.Add(value);
                    d.Add(value, bits);
                }
            }

            // get values for 2-32 bits
            index = 0;
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);

                    // get value
                    expected = values[index];
                    actualValue = d.GetInt(bits);
                    Assert.AreEqual(expected, actualValue);
                    index++;
                }
            }
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Int_Add_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((int)0, 1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is greater than 32
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Int_Add_Exc_MoreThan32Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((int)0, 33);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetInt is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Int_Get_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add(0, 32);
            d.GetInt(1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetInt is greater than 32
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Int_Get_Exc_MoreThan32Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add(0, 32);
            d.GetInt(33);
        }
        #endregion

        #region ------------------------ long? ----------------------------------------------
        /// <summary>
        /// Test adding all max and min values that can fit in 2 to 32 bits 
        /// in sequence and and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void IntN_AddGet_BitsMinMax_Sequence_Loopback()
        {
            int maxBits = 32;
            List<int?> values = new List<int?>();
            BVector d = new BVector();
            int? positiveValue;
            int? negativeValue;
            int? actualValue;
            int? nullValue = null;
            int index;
            int? expected;
            int? maxVal = int.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                positiveValue = maxVal >> i;
                negativeValue = -positiveValue - 1;

                // add positive
                d.Add(positiveValue, bits);
                values.Add(positiveValue);

                // add null
                d.Add(nullValue, bits);
                values.Add(nullValue);

                // add negative
                d.Add(negativeValue, bits);
                values.Add(negativeValue);
            }


            // get min and max values for 2-32 bits
            index = 0;
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get positive
                expected = values[index++];
                actualValue = d.GetIntN(bits);
                Assert.AreEqual(expected, actualValue);

                // get null
                expected = values[index++];
                actualValue = d.GetIntN(bits);
                Assert.AreEqual(expected, actualValue);

                // get negative
                expected = values[index++];
                actualValue = d.GetIntN(bits);
                Assert.AreEqual(expected, actualValue);
            }
        }

        /// <summary>
        /// Test adding all max and min values that can fit in 2 to 32 bits 
        /// separated by 1 bit values and and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void IntN_AddGet_BitsMinMax_Separated_Loopback()
        {
            int maxBits = 32;
            List<int?> values = new List<int?>();
            BVector d = new BVector();
            int? positiveValue;
            int? negativeValue;
            int? actualValue;
            int? nullValue = null;
            int index;
            int? expected;
            int? maxVal = int.MaxValue;
            byte bits;


            // add min and max values for 2-32 bits
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);
                positiveValue = maxVal >> i;
                negativeValue = -positiveValue - 1;

                // add positive
                d.Add(positiveValue, bits);
                values.Add(positiveValue);

                // true separator
                d.Add1(true);

                // add null
                d.Add(nullValue, bits);
                values.Add(nullValue);

                // false separator
                d.Add1(false);

                // add negative
                d.Add(negativeValue, bits);
                values.Add(negativeValue);

                // true separator
                d.Add1(true);

            }


            // get min and max values for 2-32 bits
            index = 0;
            for (int i = 0; i < (maxBits - 1); i++)
            {
                bits = (byte)(maxBits - i);

                // get positive
                expected = values[index++];
                actualValue = d.GetIntN(bits);
                Assert.AreEqual(expected, actualValue);

                // true separator
                Assert.AreEqual(true, d.Get1());

                // get null
                expected = values[index++];
                actualValue = d.GetIntN(bits);
                Assert.AreEqual(expected, actualValue);

                // false separator
                Assert.AreEqual(false, d.Get1());

                // get negative
                expected = values[index++];
                actualValue = d.GetIntN(bits);
                Assert.AreEqual(expected, actualValue);

                // true separator
                Assert.AreEqual(true, d.Get1());
            }
        }


        /// <summary>
        /// Test adding 10000 random values  
        /// in sequence and and then getting them from the same BVector
        /// </summary>
        [TestMethod]
        public void IntN_AddGet_Rnd_Sequence_Loopback()
        {
            int maxBits = 32;
            List<int?> values = new List<int?>();
            BVector d = new BVector();
            int? actualValue;
            int index;
            int? expected;
            int? maxVal = int.MaxValue;
            byte bits;
            var rnd = new CryptoRandom();
            int itemsPerBit = 10000;
            int? value;
            int tmp;


            // add random values for 2-32 bits
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);
                    tmp = maxVal.Value >> (i + 1);

                    // add random value
                    value = rnd.Next(-tmp - 1, tmp);
                    values.Add(value);
                    d.Add(value, bits);
                }
            }

            // get values for 2-32 bits
            index = 0;
            for (int j = 0; j < itemsPerBit; j++)
            {
                for (int i = 0; i < (maxBits - 2); i++)
                {
                    bits = (byte)(maxBits - i);

                    // get value
                    expected = values[index];
                    actualValue = d.GetIntN(bits);
                    Assert.AreEqual(expected, actualValue);
                    index++;
                }
            }
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IntN_Add_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((int?)0, 1);
        }


        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to Add is greater than 32
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IntN_Add_Exc_MoreThan32Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((int?)0, 33);
        }


        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetIntN is less than 2
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IntN_Get_Exc_LessThan2Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((int?)0, 32);
            d.GetIntN(1);
        }

        /// <summary>
        /// Test to ensure an exception is thrown when the bits 
        /// given to GetIntN is greater than 32
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IntN_Get_Exc_MoreThan32Bits_Loopback()
        {
            BVector d = new BVector();
            d.Add((int?)0, 32);
            d.GetIntN(33);
        }

        #endregion

    }
}
