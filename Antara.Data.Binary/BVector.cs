using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Antara.Data.Binary
{
    /// <summary>
    /// Manages a compact array of values that are being stored in form of bits.
    /// Provides high performance and precision in serializing values.
    /// </summary>
	public  sealed partial class BVector : ICloneable
    {
        #region ------------------------------ Constructors ----------------------------------

        /// <summary>
        /// Initializes a new instance of the BVector class with 
        /// the smallest storage size.
        /// </summary>
        public BVector() : this(4) { }

        /// <summary>
        /// Initializes a new instance of the BVector class with 
        /// room to fit 32 * <paramref name="initialSize"/> bits
        /// </summary>
        /// <param name="initialSize"></param>
        public BVector(int initialSize)
        {
            fStorage = new uint[initialSize];
        }

        /// <summary>
        /// Initializes a new instance of the BVector class that contains
        /// bit values from the specified byte[]
        /// </summary>
        /// <param name="bytes">An array of bytes containing values from a previously exported BVector</param>
		public BVector(byte[] bytes)
        {
            BytesToStorage(bytes);
        }
        #endregion

        #region ------------------------------ Index -----------------------------------------
        /// <summary>
        /// Advances the <see cref="Index"/> <paramref name="bits"/> bits
        /// </summary>
        /// <param name="bits"></param>
        public void AdvanceIndex(int bits)
        {
            fIndex += bits;
        }

        /// <summary>
        /// Advances the <see cref="Index"/> to the begining of the next full byte
        /// </summary>
        public void AdvanceToNextByte()
        {
            fIndex = ((fIndex >> 3) + 1) << 3;
        }
        #endregion

        #region ------------------------------ Basic - Add/Get 1 -----------------------------
        /// <summary>
        /// Adds 1 bit at the current position of the BVector and advances the position by 1
        /// and ensures the BVector has adequete size for the value to be added
        /// </summary>
        /// <param name="value">The boolean value that will be added to the BVector</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add1(bool value = true)
        {
            ResizeToFit(1);
            UnsafeAdd1(value);
        }

        /// <summary>
        /// Gets the bit value at the current position
        /// </summary>
        /// <returns>The value of the bit at the current position</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Get1()
        {
            bool r = ((fStorage[fIndex >> 5] << (fIndex & 31)) >> 31) == 1;
            fIndex++;
            return r;
        }

        /// <summary>
        /// Adds 1 bit at the current position of the BVector and advances the position by 1
        /// NOTE: This method does NOT resize the BVector
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeAdd1(bool value = true)
        {
            if (value)
            {
                fStorage[fBitsLength >> 5] |= (uint)(1 << (31 - (fBitsLength & 31)));
            }
            fBitsLength++;
        }

        /// <summary>
        /// Gets the value of the bit at a specific position in the BVector
        /// </summary>
        /// <param name="index">The zero-based index of the value to get</param>
        /// <returns>The value of the bit at position index</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public bool this[int index]
        {
            get => ((fStorage[index >> 5] << (index & 31)) >> 31) == 1;
        }
        #endregion

        #region ------------------------------ Basic - Add/Get 8 -----------------------------
        /// <summary>
        /// Adds a byte (8 bits) at the current position of the BVector and advances the position by 8
        /// and ensures the BVector has adequete size for the value to be added
        /// </summary>
        /// <param name="value">The byte value that will be added to the BVector</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add8(byte value)
        {
            ResizeToFit(8);
            UnsafeAdd8(value);
        }

        /// <summary>
        /// Gets a byte (8 bits) value at the current position
        /// </summary>
        /// <returns>The value of the byte at the current position</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte Get8()
        {
            int cursor = fIndex & C31;
            int lbits = (C32 - cursor);
            int index = fIndex;
            fIndex += 8;
            return 8 > lbits
                ? (byte)(((fStorage[index >> C5] << cursor) | (fStorage[1 + (index >> C5)] >> lbits)) >> C24)
                : (byte)((fStorage[index >> C5] << cursor) >> C24);
        }

        /// <summary>
        /// Adds a byte (8 bits) at the current position of the BVector and advances the position by 8
        /// NOTE: This method does NOT resize the BVector
        /// </summary>
        /// <param name="value">The byte value that will be added to the BVector</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeAdd8(byte value)
        {
            int lbits = 24 - (fBitsLength & 31);
            int lmod = fBitsLength >> 5;
            if (lbits >= 0)
            {
                uint v = (uint)(value << lbits);
                fStorage[lmod] |= v;
            }
            else
            {
                uint lvalue = (uint)(value >> -lbits);
                fStorage[lmod] |= lvalue;
                uint rvalue = (uint)(value << (32 + lbits));
                fStorage[1 + lmod] = rvalue;
            }
            fBitsLength += 8;
        }
        #endregion

        #region ------------------------------ Basic - Add/Get 32 ----------------------------
        /// <summary>
        /// Adds an unsigned integer (2 - 32 bits) at the current position of the BVector and advances the position by the number of bits defined
        /// and ensures the BVector has adequete size for the value to be added
        /// </summary>
        /// <param name="value">The unsigned integer value that will be added to the BVector</param>
        /// <param name="bits">The number of bits used to store the unsigned integer</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddU32(uint value, byte bits = 32)
        {
            ResizeToFit(bits);
            UnsafeAddU32(value, bits);
        }

        /// <summary>
        /// Adds an unsigned integer (2 - 32 bits) at the current position of the BVector and advances the position by the number of bits defined
        /// NOTE: This method does NOT resize the BVector
        /// </summary>
        /// <param name="value">The unsigned integer value that will be added to the BVector</param>
        /// <param name="bits">The number of bits used to store the unsigned integer</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeAddU32(uint value, byte bits = 32)
        {
            int lbits = C32 - bits - (fBitsLength & C31);

            if (lbits < 0)
            {
                int lmod = fBitsLength >> C5;
                fStorage[lmod] |= (uint)(value >> -lbits);
                fStorage[1 + lmod] = (uint)(value << (C32 + lbits));
            }
            else
            {
                fStorage[fBitsLength >> C5] |= (uint)(value << lbits);
            }
            fBitsLength += bits;
        }

        /// <summary>
        /// Gets an unsigned integer (2-32 bits) value at the current position
        /// </summary>
        /// <returns>The value of the unsigned interger at the current position</returns>
        /// <param name="bits">The number of bits used to retrieve the unsigned integer</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint GetU32(byte bits = 32)
        {
            int cursor = fIndex & C31;
            int lbits = (C32 - cursor);
            int index = fIndex;
            fIndex += bits;
            return bits > lbits
                ? (uint)(((fStorage[index >> C5] << cursor) | (fStorage[1 + (index >> C5)] >> lbits)) >> (C32 - bits))
                : (uint)((fStorage[index >> C5] << cursor) >> (C32 - bits));
        }
        #endregion

        #region ------------------------------ Basic - Add/Get 64 ----------------------------
        /// <summary>
        /// Adds a unsigned long integer (2 - 64 bits) at the current position of the BVector and advances the position by the number of bits defined
        /// and ensures the BVector has adequete size for the value to be added
        /// </summary>
        /// <param name="value">The byte value that will be added to the BVector</param>
        /// <param name="bits">The number of bits used to store the unsigned long integer</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddU64(ulong value, byte bits = 64)
        {
            if (bits <= 32)
            {
                AddU32((uint)value, bits);
                return;
            }
            ResizeToFit(bits);
            UnsafeAddU64(value, bits);
        }

        /// <summary>
        /// Adds a unsigned long integer (2 - 64 bits) at the current position of the BVector and advances the position by the number of bits defined
        /// NOTE: This method does NOT resize the BVector
        /// </summary>
        /// <param name="value">The byte value that will be added to the BVector</param>
        /// <param name="bits">The number of bits used to store the unsigned long integer</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsafeAddU64(ulong value, byte bits = 64)
        {
            if (bits <= 32)
            {
                UnsafeAddU32((uint)value, bits);
                return;
            }
            uint a1 = (uint)(value & uint.MaxValue);
            uint a2 = (uint)(value >> 32);
            UnsafeAddU32(a1, 32);
            UnsafeAddU32(a2, (byte)(bits - 32));
        }

        /// <summary>
        /// Gets an unsigned long integer (2 - 64 bits) value at the current position
        /// </summary>
        /// <returns>The value of the unsigned long interger at the current position</returns>
        /// <param name="bits">The number of bits used to retrieve the unsigned long integer</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong GetU64(byte bits = 64)
        {
            if (bits <= 32) return GetU32(bits);
            return (ulong)GetU32(32) | (ulong)GetU32((byte)(bits - 32)) << 32;
        }
        #endregion

        #region ------------------------------ Basic - Object Length -------------------------
        /// <summary>
        /// Stores the length of an object by using the least possible space
        /// and resizes the BVector to fit the length indicator and the object itself
        /// </summary>
        /// <param name="len">The length of object</param>
        /// <param name="itemSize">The size of the item's bits</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _AddObjectLen(uint len, byte itemSize)
        {
            // 0 -> empty item => 0
            // 10 -> 8 bits length (tiny)
            // 110 -> 16 bits length (small)
            // 111 -> 24 bits length (large)
            if (len == 0)
            {
                ResizeToFit(1);
                fBitsLength++;
                return;
            }
            ResizeToFit(((int)len * itemSize) + 48);

            Add1();
            if (len < 256)
            {
                fBitsLength++;
                UnsafeAdd8((byte)len);
            }
            else
            {
                Add1();
                if (len < 65536)
                {
                    fBitsLength++;
                    UnsafeAddU32(len, 16);
                }
                else
                {
                    if (len >= 16777216) throw new ArgumentOutOfRangeException("Objects may not exceed items 16777215 in length");
                    Add1();
                    UnsafeAddU32(len, 24);
                }
            }
        }

        /// <summary>
        /// Returns the length of an object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int _GetObjectLen()
        {
            if (!Get1()) return 0;
            if (!Get1()) return Get8();
            if (!Get1()) return (int)GetU32(16);
            return (int)GetU32(24);
        }
		#endregion

		#region ------------------------------ Storage Manipulation --------------------------
		/// <summary>
		/// Reads the bytes in the provided byte[] and stores the in the <see cref="fStorage"/> uint array
		/// </summary>
		/// <param name="b"></param>
		private void BytesToStorage(byte[] b)
        {
            int i;
            int l = b.Length / 4;
            int sBytes = b.Length % 4;
            if (sBytes > 0)
            {
                fStorage = new uint[l + 1];
            }
            else
            {
                fStorage = new uint[l];
            }
            uint bt;
            int bi;
            for (i = 0; i < l; i++)
            {
                bi = i * 4;
                bt = (uint)((b[bi] << 24) | (b[bi + 1] << 16) | (b[bi + 2] << 8) | (b[bi + 3]));
                fStorage[i] = bt;
            }
            int lastByte = i;

            for (i = 0; i < sBytes; i++)
            {
                fStorage[lastByte] |= (uint)(((uint)(((uint)(((sbyte)b[(lastByte * 4) + i]) << 24)) >> 24)) << ((3 - i) * 8));
            }

            fBitsLength = b.Length * 8;
        }

        /// <summary>
        /// Resizes the storage if necessary in order to fit the specified number of bits
        /// </summary>
        /// <param name="bits"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ResizeToFit(int bits)
        {
            var newSize = ((fBitsLength + bits) >> 5);
            if (fStorage.Length <= newSize)
            {
                var newArraySize = fStorage.Length * 2;
                while (newArraySize < newSize) newArraySize *= 2;
                Array.Resize<uint>(ref fStorage, newArraySize);
            }
        }

        /// <summary>
        /// Resets the read index so the BVector can be read again
        /// </summary>
        public void ResetIndex()
        {
            fIndex = 0;
        }

        /// <summary>
        /// Returns the data stored in the BVector in a byte[]
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            int l = (int)Math.Floor((double)((double)fBitsLength / 8));
            int remain = (int)Math.Ceiling((double)(fBitsLength % 8) / 8);

            byte[] b = new byte[l + remain];
            uint bt;
            int index = -4;
            l /= 4;
            for (int i = 0; i < l; i++)
            {
                index = 0;
                bt = fStorage[i];
                index = i * 4;
                b[index] = (byte)(bt >> 24);
                b[index + 1] = (byte)(bt >> 16);
                b[index + 2] = (byte)(bt >> 8);
                b[index + 3] = (byte)(bt);
            }
            index += 4;
            var _l = Math.Min(4, b.Length - index);
            for (int i = 0; i < _l; i++)
            {
                bt = fStorage[l];
                b[index + i] = (byte)(bt >> 8 * (3 - i));
            }
            return b;
        }
        #endregion

        #region ------------------------------ Fields ----------------------------------------
        /// <summary>
        /// Stores the binary data
        /// </summary>
        private uint[] fStorage;

        /// <summary>
        /// Used when ADDING data to the BVector
        /// </summary>
        private int fBitsLength;

        /// <summary>
        /// Used when READING data from the BVector
        /// </summary>
        private int fIndex;

		/// <summary>
		/// Returns the current index when reading traversing the <see cref="BVector"/>
		/// </summary>
        public int Index => fIndex;

		/// <summary>
		/// Gets the total length in bits 
		/// NOTE: Applicable only when creating a <see cref="BVector"/>
		/// </summary>
        public int BitsLen => fBitsLength;

        const int C32 = 32;
        const int C31 = 31;
        const int C24 = 24;
        const int C5 = 5;

        const int INT_MAX = int.MaxValue;
        const int INT_MIN = int.MinValue;
        #endregion

        #region ------------------------------ Helper ----------------------------------------
        /// <summary>
        /// Converts the specified floating point number to a 32-bit signed integer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static unsafe int SingleToInt32Bits(float value)
        {
            return *(int*)(&value);
        }

        /// <summary>
        /// Converts the specified 32-bit signed integer to a floating point number.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static unsafe float Int32BitsToSingle(int value)
        {
            return *(float*)(&value);
        }


        #endregion

        #region ------------------------------ ICloneable ------------------------------------
		/// <summary>
		/// Creates a copy of the <see cref="BVector"/>
		/// </summary>
		/// <returns></returns>
        public object Clone()
        {
            return new BVector(ToBytes());
        }

        #endregion

        #region ------------------------------ File Operations -------------------------------
		/// <summary>
		/// Writes a byte array to the specified file in a manner that can be read by <see cref="ReadFromFile(string)"/>
		/// </summary>
		/// <param name="b">The byte[] to store in the file</param>
		/// <param name="filename">The filename where the data will be stored</param>
        public static void WriteToFile(byte[] b, string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {

                    int bcount = b.Length;
                    writer.Write(bcount);
                    writer.Write(b);
                    writer.Close();
                }
            }
        }

		/// <summary>
		/// Reads a byte array from the specified file that was previously written by <see cref="WriteToFile(byte[], string)"/>
		/// </summary>
		/// <param name="filename">The full path where the file is stored</param>
		/// <returns></returns>
        public static byte[] ReadFromFile(string filename)
        {
            byte[] b = null;
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int bcount = reader.ReadInt32();
                    b = reader.ReadBytes(bcount);
                    reader.Close();
                }
            }
            return b;
        }
        #endregion
    }
}
