using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Antara.Data.Binary
{
    public sealed partial class BVector
    {
        #region ------------------------------ Extended - Numeric\int ------------------------
        /// <summary>
        /// Adds a signed integer value that will occupy the specified number of bits
        /// and advances the cursor by the same amount of bits
        /// If the signed integer value does not fit in the bits specified the value will not be stored properly
        /// NOTE: This method does NOT resize the BVector
        /// </summary>
        /// <param name="value">The signed integer value that will be added to the BVector</param>
        /// <param name="bits">The number of bits the signed integer will occupy</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _AddInt(int value, byte bits)
        {
            if (value < 0)
            {
                Add1(true);
                UnsafeAddU32((uint)~value, (byte)(bits - 1));
            }
            else
            {
                fBitsLength++;
                UnsafeAddU32((uint)value, (byte)(bits - 1));
            }
        }

        /// <summary>
        /// Gets the signed integer value that resides from the current position 
        /// to the next specified number of bits
        /// and advances the cursor
        /// </summary>
        /// <param name="bits">The number of bits ahead to read to construct the signed integer</param>
        /// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int _GetInt(byte bits)
        {
            return Get1() ? -(int)GetU32((byte)(bits - 1)) - 1 : (int)GetU32((byte)(bits - 1));
        }

		/// <summary>
		/// Verifies the specified int can fit by using the specified bits. If the number does not fit, an exception is thrown
		/// </summary>
		/// <param name="value">The signed integer that needs to fit</param>
		/// <param name="bits">The number of bits the integer is expected to fit in</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void _VerifyIntFits(int value, byte bits)
        {
            if (value > 0)
            {
                int maxV = INT_MAX >> (32 - bits);
                if (value > maxV) throw new ArgumentOutOfRangeException($"Max value for adding a int with {bits} bits is {maxV}, value provided={value}");
            }
            else if (value < 0)
            {
                int minV = INT_MIN >> (32 - bits);
                if (value < minV) throw new ArgumentOutOfRangeException($"Min value for adding a int with {bits} bits is {minV}, value provided={value}");
            }
        }

        /// <summary>
        /// Adds a signed integer value that will occupy the specified number of bits
        /// and advances the cursor
        /// NOTE: If the signed integer value does not fit in the bits specified the value is not stored properly
        /// </summary>
        /// <param name="value">The signed integer value that will be added to the BVector</param>
        /// <param name="bits">The number of bits the signed integer will occupy</param>
		public void Add(int value, byte bits = 32)
        {
            if (bits < 2 || bits > 32) throw new ArgumentException("Bits for ADDing int should be between 2 and 32");
            ResizeToFit(bits);
            _AddInt(value, bits);
        }

        /// <summary>
        /// Adds a signed integer value that will occupy the specified number of bits
        /// and advances the cursor
        /// If the signed integer value does not fit in the bits specified an exception is thrown
        /// </summary>
        /// <param name="value">The signed integer value that will be added to the BVector</param>
        /// <param name="bits">The number of bits the signed integer will occupy</param>
        public void SafeAdd(int value, byte bits = 32)
        {
            if (bits < 2 || bits > 32) throw new ArgumentException("Bits for ADDing int should be between 2 and 32");
            _VerifyIntFits(value, bits);
            ResizeToFit(bits);
            _AddInt(value, bits);
        }

        /// <summary>
        /// Gets the signed integer value that resides from the current position 
        /// to the next specified number of bits and advances the cursor
        /// </summary>
        /// <param name="bits">The number of bits ahead to read to construct the signed integer. Should be the same number as used when the number was added.</param>
        /// <returns></returns>
        public int GetInt(byte bits = 32)
        {
            if (bits < 2 || bits > 32) throw new ArgumentException("Bits for GETting int should be between 2 and 32");
            return _GetInt(bits);
        }

		/// <summary>
		/// Adds a nullable signed integer value that will occupy the specified number of bits + 1
		/// and advances the cursor
		/// NOTE: If the signed integer value does not fit in the bits specified the value is not stored properly
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The signed integer value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the signed integer will occupy</param>
		public void Add(int? value, byte bits = 32)
        {
            if (bits < 2 || bits > 32) throw new ArgumentException("Bits for ADDing int should be between 2 and 32");
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(bits);
            _AddInt(value.Value, bits);

        }

        /// <summary>
        /// Adds a signed integer value that will occupy the specified number of bits 
        /// and advances the cursor
        /// If the signed integer value does not fit in the bits specified an exception is thrown
        /// </summary>
        /// <param name="value">The signed integer value that will be added to the BVector</param>
        /// <param name="bits">The number of bits the signed integer will occupy</param>
        public void SafeAdd(int? value, byte bits = 32)
        {
            if (bits < 2 || bits > 32) throw new ArgumentException("Bits for ADDing int should be between 2 and 32");
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }
            _VerifyIntFits(value.Value, bits);
            ResizeToFit(bits);
            _AddInt(value.Value, bits);
        }

        /// <summary>
        /// Gets the nullable signed integer value that resides from the current position 
        /// and the next specified number of bits
        /// </summary>
        /// <param name="bits">The number of bits ahead to read to construct the signed integer. Should be the same number as used when the number was added.</param>
        /// <returns></returns>
        public int? GetIntN(byte bits = 32)
        {
            if (bits < 2 || bits > 32) throw new ArgumentException("Bits for GETting int should be between 2 and 32");
            if (!Get1())
            {
                return null;
            }
            return _GetInt(bits);
        }

		#endregion

		#region ------------------------------ Extended - Numeric\long -----------------------
		/// <summary>
		/// Adds a signed long value that will occupy the specified number of bits
		/// and advances the cursor by the same amount of bits
		/// If the signed long value does not fit in the bits specified the value will not be stored properly
		/// NOTE: This method does NOT resize the BVector
		/// </summary>
		/// <param name="value">The signed long value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the signed long will occupy</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _AddLong(long value, byte bits)
        {
            if (value < 0)
            {
                Add1(true); // is negative
                UnsafeAddU64((ulong)~value, (byte)(bits - 1));
            }
            else
            {
                fBitsLength++;
                UnsafeAddU64((ulong)value, (byte)(bits - 1));
            }
        }

		/// <summary>
		/// Gets the signed long value that resides from the current position 
		/// to the next specified number of bits
		/// and advances the cursor
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the signed long value. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long _GetLong(byte bits)
        {
            var isNegative = Get1();
            return isNegative ? -(long)GetU64((byte)(bits - 1)) - 1 : (long)GetU64((byte)(bits - 1));
        }

		/// <summary>
		/// Adds a signed long value that will occupy the specified number of bits
		/// and advances the cursor
		/// NOTE: If the signed long value does not fit in the bits specified the value is not stored properly
		/// </summary>
		/// <param name="value">The signed long value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the signed long will occupy</param>
		public void Add(long value, byte bits = 64)
        {
            if (bits < 2 || bits > 64) throw new ArgumentException("Bits for ADDing long should be between 2 and 64");
            ResizeToFit(bits);
            _AddLong(value, bits);
        }

		/// <summary>
		/// Gets the signed long value that resides from the current position 
		/// to the next specified number of bits and advances the cursor
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the signed long. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public long GetLong(byte bits = 64)
        {
            if (bits < 2 || bits > 64) throw new ArgumentException("Bits for GETting long should be between 2 and 64");
            return _GetLong(bits);
        }

		/// <summary>
		/// Adds a nullable signed long value that will occupy the specified number of bits + 1
		/// and advances the cursor
		/// NOTE: If the signed long value does not fit in the bits specified the value is not stored properly
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The signed long value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the signed long will occupy</param>
		public void Add(long? value, byte bits = 64)
        {
            if (bits < 2 || bits > 64) throw new ArgumentException("Bits for ADDing long should be between 2 and 64");
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(bits);
            _AddLong(value.Value, bits);

        }

		/// <summary>
		/// Gets the nullable signed long value that resides from the current position 
		/// and the next specified number of bits
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the signed long. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public long? GetLongN(byte bits = 64)
        {
            if (bits < 2 || bits > 64) throw new ArgumentException("Bits for GETting long should be between 2 and 64");
            if (!Get1())
            {
                return null;
            }
            return _GetLong(bits);
        }
		#endregion

		#region ------------------------------ Extended - Numeric\uint -----------------------
		/// <summary>
		/// Adds an unsigned integer value that will occupy the specified number of bits
		/// and advances the cursor
		/// NOTE: If the unsigned integer value does not fit in the bits specified the value is not stored properly
		/// </summary>
		/// <param name="value">The unsigned integer value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the unsigned integer will occupy</param>
		public void Add(uint value, byte bits = 32)
        {
            if (bits < 2 || bits > 32) throw new ArgumentException("Bits for ADDing int should be between 2 and 32");
            ResizeToFit(bits);
            UnsafeAddU32(value, bits);
        }

		/// <summary>
		/// Gets the unsigned integer value that resides from the current position 
		/// to the next specified number of bits and advances the cursor
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the unsigned integer. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public uint GetUInt(byte bits = 32)
        {
            if (bits < 2 || bits > 32) throw new ArgumentException("Bits for GETting int should be between 2 and 32");
            return GetU32(bits);
        }

		/// <summary>
		/// Adds a nullable unsigned integer value that will occupy the specified number of bits + 1
		/// and advances the cursor
		/// NOTE: If the unsigned integer value does not fit in the bits specified the value is not stored properly
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The unsigned integer value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the unsigned integer will occupy</param>
		public void Add(uint? value, byte bits = 32)
        {
            if (bits < 2 || bits > 32) throw new ArgumentException("Bits for ADDing int should be between 2 and 32");
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(bits);
            UnsafeAddU32(value.Value, bits);

        }

		/// <summary>
		/// Gets the nullable unsigned integer value that resides from the current position 
		/// and the next specified number of bits
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the unsigned integer. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public uint? GetUIntN(byte bits = 32)
        {
            if (bits < 2 || bits > 32) throw new ArgumentException("Bits for GETting int should be between 2 and 32");
            if (!Get1())
            {
                return null;
            }
            return GetU32(bits);
        }
		#endregion

		#region ------------------------------ Extended - Numeric\ulong ----------------------
		/// <summary>
		/// Adds an unsigned long value that will occupy the specified number of bits
		/// and advances the cursor
		/// NOTE: If the unsigned long value does not fit in the bits specified the value is not stored properly
		/// </summary>
		/// <param name="value">The unsigned long value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the unsigned long will occupy</param>
		public void Add(ulong value, byte bits = 64)
        {
            if (bits < 2 || bits > 64) throw new ArgumentException("Bits for ADDing ulong should be between 2 and 64");
            ResizeToFit(bits);
            UnsafeAddU64(value, bits);
        }

		/// <summary>
		/// Gets the unsigned long value that resides from the current position 
		/// to the next specified number of bits and advances the cursor
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the unsigned long. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public ulong GetULong(byte bits = 64)
        {
            if (bits < 2 || bits > 64) throw new ArgumentException("Bits for GETting ulong should be between 2 and 64");
            return GetU64(bits);
        }

		/// <summary>
		/// Adds a nullable unsigned long value that will occupy the specified number of bits + 1
		/// and advances the cursor
		/// NOTE: If the unsigned long value does not fit in the bits specified the value is not stored properly
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The unsigned long value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the unsigned long will occupy</param>
		public void Add(ulong? value, byte bits = 64)
        {
            if (bits < 2 || bits > 64) throw new ArgumentException("Bits for ADDing ulong should be between 2 and 64");
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(bits);
            UnsafeAddU64(value.Value, bits);

        }

		/// <summary>
		/// Gets the nullable unsigned long value that resides from the current position 
		/// and the next specified number of bits
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the unsigned long. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public ulong? GetULongN(byte bits = 64)
        {
            if (bits < 2 || bits > 64) throw new ArgumentException("Bits for GETting ulong should be between 2 and 64");
            if (!Get1())
            {
                return null;
            }
            return GetU64(bits);
        }
		#endregion

		#region ------------------------------ Extended - Numeric\short ----------------------
		/// <summary>
		/// Adds an signed short value that will occupy the specified number of bits
		/// and advances the cursor
		/// NOTE: If the signed short value does not fit in the bits specified the value is not stored properly
		/// </summary>
		/// <param name="value">The signed short value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the signed short will occupy</param>
		public void Add(short value, byte bits = 16)
        {
            if (bits < 2 || bits > 16) throw new ArgumentException("Bits for ADDing short should be between 2 and 16");
            ResizeToFit(bits);
            _AddInt(value, bits);
        }

		/// <summary>
		/// Gets the signed short value that resides from the current position 
		/// to the next specified number of bits and advances the cursor
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the signed short. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public short GetShort(byte bits = 16)
        {
            if (bits < 2 || bits > 16) throw new ArgumentException("Bits for GETting short should be between 2 and 16");
            return (short)_GetInt(bits);
        }

		/// <summary>
		/// Adds a nullable signed short value that will occupy the specified number of bits + 1
		/// and advances the cursor
		/// NOTE: If the signed short value does not fit in the bits specified the value is not stored properly
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The signed short value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the signed short will occupy</param>
		public void Add(short? value, byte bits = 16)
        {
            if (bits < 2 || bits > 16) throw new ArgumentException("Bits for ADDing short should be between 2 and 16");
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(bits);
            _AddInt(value.Value, bits);

        }

		/// <summary>
		/// Gets the nullable signed short value that resides from the current position 
		/// and the next specified number of bits
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the signed short. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public short? GetShortN(byte bits = 16)
        {
            if (bits < 2 || bits > 16) throw new ArgumentException("Bits for GETting short should be between 2 and 16");
            if (!Get1())
            {
                return null;
            }
            return (short?)_GetInt(bits);
        }
		#endregion

		#region ------------------------------ Extended - Numeric\ushort ---------------------
		/// <summary>
		/// Adds an unsigned short value that will occupy the specified number of bits
		/// and advances the cursor
		/// NOTE: If the unsigned short value does not fit in the bits specified the value is not stored properly
		/// </summary>
		/// <param name="value">The unsigned short value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the unsigned short will occupy</param>
		public void Add(ushort value, byte bits = 16)
        {
            if (bits < 2 || bits > 16) throw new ArgumentException("Bits for ADDing ushort should be between 2 and 16");
            ResizeToFit(bits);
            UnsafeAddU32(value, bits);
        }

		/// <summary>
		/// Gets the unsigned short value that resides from the current position 
		/// to the next specified number of bits and advances the cursor
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the unsigned short. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public ushort GetUShort(byte bits = 16)
        {
            if (bits < 2 || bits > 16) throw new ArgumentException("Bits for GETting ushort should be between 2 and 16");
            return (ushort)GetU32(bits);
        }

		/// <summary>
		/// Adds a nullable unsigned short value that will occupy the specified number of bits + 1
		/// and advances the cursor
		/// NOTE: If the unsigned short value does not fit in the bits specified the value is not stored properly
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The unsigned short value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the unsigned short will occupy</param>
		public void Add(ushort? value, byte bits = 16)
        {
            if (bits < 2 || bits > 16) throw new ArgumentException("Bits for ADDing ushort should be between 2 and 16");
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(bits);
            UnsafeAddU32(value.Value, bits);

        }

		/// <summary>
		/// Gets the nullable unsigned short value that resides from the current position 
		/// and the next specified number of bits
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the unsigned short. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public ushort? GetUShortN(byte bits = 16)
        {
            if (bits < 2 || bits > 16) throw new ArgumentException("Bits for GETting ushort should be between 2 and 16");
            var hasValue = Get1();
            if (!hasValue)
            {
                return null;
            }
            return (ushort?)GetU32(bits);
        }
		#endregion

		#region ------------------------------ Extended - Numeric\byte -----------------------
		/// <summary>
		/// Adds an unsigned byte value that will occupy the specified number of bits
		/// and advances the cursor
		/// NOTE: If the unsigned byte value does not fit in the bits specified the value is not stored properly
		/// </summary>
		/// <param name="value">The unsigned byte value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the unsigned byte will occupy</param>
		public void Add(byte value, byte bits = 8)
        {
            if (bits < 2 || bits > 8) throw new ArgumentException("Bits for ADDing byte should be between 2 and 8");
            ResizeToFit(bits);
            UnsafeAddU32(value, bits);
        }

		/// <summary>
		/// Gets the unsigned byte value that resides from the current position 
		/// to the next specified number of bits and advances the cursor
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the unsigned byte. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public byte GetByte(byte bits = 8)
        {
            if (bits < 2 || bits > 8) throw new ArgumentException("Bits for GETting byte should be between 2 and 8");
            return (byte)GetU32(bits);
        }

		/// <summary>
		/// Adds a nullable unsigned byte value that will occupy the specified number of bits + 1
		/// and advances the cursor
		/// NOTE: If the unsigned byte value does not fit in the bits specified the value is not stored properly
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The unsigned byte value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the unsigned byte will occupy</param>
		public void Add(byte? value, byte bits = 8)
        {
            if (bits < 2 || bits > 8) throw new ArgumentException("Bits for ADDing byte should be between 2 and 8");
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(bits);
            UnsafeAddU32(value.Value, bits);
        }

		/// <summary>
		/// Gets the nullable unsigned byte value that resides from the current position 
		/// and the next specified number of bits
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the unsigned byte. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public byte? GetByteN(byte bits = 8)
        {
            if (bits < 2 || bits > 8) throw new ArgumentException("Bits for GETting byte should be between 2 and 8");
            if (!Get1())
            {
                return null;
            }
            return (byte?)GetU32(bits);
        }
		#endregion

		#region ------------------------------ Extended - Numeric\sbyte ----------------------
		/// <summary>
		/// Adds an signed byte value that will occupy the specified number of bits
		/// and advances the cursor
		/// NOTE: If the signed byte value does not fit in the bits specified the value is not stored properly
		/// </summary>
		/// <param name="value">The signed byte value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the signed byte will occupy</param>
		public void Add(sbyte value, byte bits = 8)
        {
            if (bits < 2 || bits > 8) throw new ArgumentException("Bits for ADDing sbyte should be between 2 and 8");
            ResizeToFit(bits);
            _AddInt(value, bits);
        }

		/// <summary>
		/// Gets the signed byte value that resides from the current position 
		/// to the next specified number of bits and advances the cursor
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the signed byte. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public sbyte GetSByte(byte bits = 8)
        {
            if (bits < 2 || bits > 8) throw new ArgumentException("Bits for GETting sbyte should be between 2 and 8");
            return (sbyte)_GetInt(bits);
        }

		/// <summary>
		/// Adds a nullable signed byte value that will occupy the specified number of bits + 1
		/// and advances the cursor
		/// NOTE: If the signed byte value does not fit in the bits specified the value is not stored properly
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The signed byte value that will be added to the BVector</param>
		/// <param name="bits">The number of bits the signed byte will occupy</param>
		public void Add(sbyte? value, byte bits = 8)
        {
            if (bits < 2 || bits > 8) throw new ArgumentException("Bits for ADDing sbyte should be between 2 and 8");
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(bits);
            _AddInt(value.Value, bits);

        }

		/// <summary>
		/// Gets the nullable signed byte value that resides from the current position 
		/// and the next specified number of bits
		/// </summary>
		/// <param name="bits">The number of bits ahead to read to construct the signed byte. Should be the same number as used when the number was added.</param>
		/// <returns></returns>
		public sbyte? GetSByteN(byte bits = 8)
        {
            if (bits < 2 || bits > 8) throw new ArgumentException("Bits for GETting sbyte should be between 2 and 8");
            if (!Get1())
            {
                return null;
            }
            return (sbyte?)_GetInt(bits);
        }
		#endregion

		#region ------------------------------ Extended - Numeric\decimal --------------------
		/// <summary>
		/// Adds a decimal value that will occupy 128 bits
		/// and advances the cursor 
		/// NOTE: This method does NOT resize the BVector
		/// </summary>
		/// <param name="value">The decimal value that will be added to the BVector</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _AddDecimal(decimal value)
        {
            int[] bits = Decimal.GetBits(value);
            _AddInt(bits[0], 32);
            _AddInt(bits[1], 32);
            _AddInt(bits[2], 32);
            _AddInt(bits[3], 32);
        }

		/// <summary>
		/// Gets the decimal value that resides from the current position 
		/// to the next 128 bits
		/// and advances the cursor
		/// </summary>
		/// <returns>The reconstructed decimal value</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private decimal _GetDecimal()
        {
            return new decimal(new int[4] {
                _GetInt(32),
                _GetInt(32),
                _GetInt(32),
                _GetInt(32)
            });
        }

		/// <summary>
		/// Adds a decimal value that will occupy 128 bits
		/// and advances the cursor
		/// </summary>
		/// <param name="value">The decimal value that will be added to the BVector</param>
		public void Add(decimal value)
        {
            ResizeToFit(128);
            _AddDecimal(value);
        }

		/// <summary>
		/// Gets the decimal value that resides from the current position 
		/// to the next 128 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed decimal value</returns>
		public decimal GetDecimal()
        {
            return _GetDecimal();
        }

		/// <summary>
		/// Adds a nullable decimal value that will occupy 128 bits + 1 bit for null specification
		/// and advances the cursor
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The decimal value that will be added to the BVector</param>
		public void Add(decimal? value)
        {
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(128);
            _AddDecimal(value.Value);
        }

		/// <summary>
		/// Gets the nullable decimal value that resides from the current position 
		/// to the next 128 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed decimal value or null</returns>
		public decimal? GetDecimalN()
        {
            if (!Get1())
            {
                return null;
            }
            return _GetDecimal();
        }
		#endregion

		#region ------------------------------ Extended - Numeric\double----------------------
		/// <summary>
		/// Adds a double value that will occupy 64 bits
		/// and advances the cursor
		/// </summary>
		/// <param name="value">The double value that will be added to the BVector</param>
		public void Add(double value)
        {
            ResizeToFit(64);
            _AddLong(BitConverter.DoubleToInt64Bits(value), 64);
        }

		/// <summary>
		/// Gets the double value that resides from the current position 
		/// to the next 64 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed double value</returns>
		public double GetDouble()
        {
            return BitConverter.Int64BitsToDouble(_GetLong(64));
        }

		/// <summary>
		/// Adds a nullable double value that will occupy 64 bits + 1 bit for null specification
		/// and advances the cursor
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The double value that will be added to the BVector</param>
		public void Add(double? value)
        {
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(64);
            _AddLong(BitConverter.DoubleToInt64Bits(value.Value), 64);
        }

		/// <summary>
		/// Gets the nullable double value that resides from the current position 
		/// to the next 64 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed double value or null</returns>
		public double? GetDoubleN()
        {
            var hasValue = Get1();
            if (!hasValue)
            {
                return null;
            }
            return BitConverter.Int64BitsToDouble(_GetLong(64));
        }
		#endregion

		#region ------------------------------ Extended - Numeric\float ---------------------
		/// <summary>
		/// Adds a float value that will occupy 32 bits
		/// and advances the cursor
		/// </summary>
		/// <param name="value">The float value that will be added to the BVector</param>
		public void Add(float value)
        {
            ResizeToFit(32);
            _AddInt(SingleToInt32Bits(value), 32);
        }

		/// <summary>
		/// Gets the float value that resides from the current position 
		/// to the next 32 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed float value</returns>
		public float GetFloat()
        {
            return Int32BitsToSingle(_GetInt(32));
        }

		/// <summary>
		/// Adds a nullable float value that will occupy 32 bits + 1 bit for null specification
		/// and advances the cursor
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The float value that will be added to the BVector</param>
		public void Add(float? value)
        {
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(32);
            _AddInt(SingleToInt32Bits(value.Value), 32);
        }

		/// <summary>
		/// Gets the nullable float value that resides from the current position 
		/// to the next 32 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed float value or null</returns>
		public float? GetFloatN()
        {
            var hasValue = Get1();
            if (!hasValue)
            {
                return null;
            }
            return Int32BitsToSingle(_GetInt(32));
        }
		#endregion

		#region ------------------------------ Extended - Date\Datetime ----------------------
		/// <summary>
		/// Adds a DateTime value that will occupy 64 bits
		/// and advances the cursor
		/// </summary>
		/// <param name="value">The DateTime value that will be added to the BVector</param>
		public void Add(DateTime value)
        {
            ResizeToFit(64);
            _AddLong(value.Ticks, 64);
        }

		/// <summary>
		/// Gets the DateTime value that resides from the current position 
		/// to the next 64 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed DateTime value</returns>
		public DateTime GetDateTime()
        {
            return new DateTime(_GetLong(64));
        }

		/// <summary>
		/// Adds a nullable DateTime value that will occupy 64 bits + 1 bit for null specification
		/// and advances the cursor
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The DateTime value that will be added to the BVector</param>
		public void Add(DateTime? value)
        {
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);

            ResizeToFit(64);
            _AddLong(value.Value.Ticks, 64);
        }

		/// <summary>
		/// Gets the nullable DateTime value that resides from the current position 
		/// to the next 64 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed DateTime value or null</returns>
		public DateTime? GetDateTimeN()
        {
            if (!Get1())
            {
                return null;
            }
            return new DateTime(_GetLong(64));
        }
		#endregion

		#region ------------------------------ Extended - Date\Timespan ----------------------
		/// <summary>
		/// Adds a TimeSpan value that will occupy 64 bits
		/// and advances the cursor
		/// </summary>
		/// <param name="value">The TimeSpan value that will be added to the BVector</param>
		public void Add(TimeSpan value)
        {
            ResizeToFit(64);
            _AddLong(value.Ticks, 64);
        }

		/// <summary>
		/// Gets the TimeSpan value that resides from the current position 
		/// to the next 64 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed TimeSpan value</returns>
		public TimeSpan GetTimeSpan()
        {
            return new TimeSpan(_GetLong(64));
        }

		/// <summary>
		/// Adds a nullable TimeSpan value that will occupy 64 bits + 1 bit for null specification
		/// and advances the cursor
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The TimeSpan value that will be added to the BVector</param>
		public void Add(TimeSpan? value)
        {
            if (!value.HasValue)
            {
                fBitsLength++;
                return;
            }

            Add1(true);
            ResizeToFit(64);
            _AddLong(value.Value.Ticks, 64);
        }

		/// <summary>
		/// Gets the nullable TimeSpan value that resides from the current position 
		/// to the next 64 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed TimeSpan value or null</returns>
		public TimeSpan? GetTimeSpanN()
        {
            if (!Get1())
            {
                return null;
            }
            return new TimeSpan(_GetLong(64));
        }
		#endregion

		#region ------------------------------ Extended - Text\string ------------------------
		/// <summary>
		/// Adds a unicode string value and advances the cursor
		/// Each character requires 16 bits + a few bits depending on the length of the string to specify its length
		/// </summary>
		/// <param name="value">The string value that will be added to the BVector</param>
		public void Add(string value)
        {
            if (value == null)
            {
                fBitsLength++;
                return;
            }
            Add1();
            _AddObjectLen((uint)value.Length, 16);
            value.Select(a => (uint)a).ToList().ForEach(a => UnsafeAddU32(a, 16));
        }

		/// <summary>
		/// Gets the string value that resides from the current position as it was originally added
		/// </summary>
		/// <returns>The reconstructed string value</returns>
		public string GetString()
        {
            if (!Get1()) return null;
            int len = _GetObjectLen();
            if (len == 0) return string.Empty;
            StringBuilder sb = new StringBuilder(len, len);
            for (int i = 0; i < len; i++)
            {
                sb.Append((char)GetU32(16));
            }
            return sb.ToString();
        }

		/// <summary>
		/// Adds an ascii string value and advances the cursor
		/// Each character requires 8 bits + a few bits depending on the length of the string to specify its length
		/// </summary>
		/// <param name="value">The string value that will be added to the BVector</param>
		public void AddAscii(string value)
        {
            if (value == null)
            {
                fBitsLength++;
                return;
            }
            Add1();
            _AddObjectLen((uint)value.Length, 8);

            value.Select(a => (short)a).ToList().ForEach(a => UnsafeAddU32((uint)a, 8));
        }

		/// <summary>
		/// Gets the ascii string value that resides from the current position as it was originally added
		/// </summary>
		/// <returns>The reconstructed string value</returns>
		public string GetAscii()
        {
            if (!Get1()) return null;
            int len = _GetObjectLen();
            if (len == 0) return string.Empty;
            StringBuilder sb = new StringBuilder(len, len);
            for (int i = 0; i < len; i++)
            {
                sb.Append((char)GetU32(8));
            }
            return sb.ToString();
        }
		#endregion

		#region ------------------------------ Extended - Text\char --------------------------
		/// <summary>
		/// Adds a char value that will occupy 16 bits
		/// and advances the cursor
		/// </summary>
		/// <param name="value">The char value that will be added to the BVector</param>
		public void Add(char value)
        {
            ResizeToFit(16);
            UnsafeAddU32(value, 16);
        }

		/// <summary>
		/// Gets the char value that resides from the current position 
		/// to the next 16 bits and advances the cursor
		/// </summary>
		/// <returns>The reconstructed char value</returns>
		public char GetChar()
        {
            return (char)GetU32(16);
        }
		#endregion

		#region ------------------------------ Extended - Binary\bytearray -------------------
		/// <summary>
		/// Adds a byte[] value that will occupy it's length in bytes + an additional few bits to specify its length
		/// and advances the cursor
		/// NOTE: If the value is null, only 1 bit is occupied
		/// </summary>
		/// <param name="value">The byte[] value that will be added to the BVector</param>
		public void Add(byte[] value)
        {
            if (value == null)
            {
                fBitsLength++;
                return;
            }
            Add1();
            _AddObjectLen((uint)value.Length, 8);
            for (int i = 0; i < value.Length; i++)
            {
                UnsafeAdd8(value[i]);
            }
        }

		/// <summary>
		/// Gets the byte[] value that resides from the current position as originally added
		/// </summary>
		/// <returns>The reconstructed char value</returns>
		public byte[] GetByteArray()
        {
            if (!Get1()) return null;
            int len = _GetObjectLen();
            if (len == 0) return new byte[] { };
            byte[] result = new byte[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = Get8();
            }
            return result;
        }
		#endregion
	}
}
