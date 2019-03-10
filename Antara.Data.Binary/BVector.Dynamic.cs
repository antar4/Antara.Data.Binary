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
        #region ------------------------------ Serial ----------------------------------------
        /// <summary>
        /// Stores a short value by setting bits equal to value-1 to 0
        /// and the next bit to 1
        /// Adding value with this method requires significantly higher storage space than usual. 
        /// Although this method allows for an extensible length.
        /// </summary>
        /// <param name="value"></param>
        public void SerialAdd(ushort value)
        {
            ResizeToFit(value + 1);
            fBitsLength += value;
            Add1(true);
        }

        /// <summary>
        /// Returns a value added with <see cref="SerialAdd(ushort)"/>
        /// </summary>
        /// <returns></returns>
        public ushort SerialGet()
        {
            ushort val = 0;
            while (!Get1()) val++;
            return val;
        }
        #endregion


        #region ------------------------------ Internal --------------------------------------
        /// <summary>
        /// Adds a ulong and dynamically calculates the bits to occupy
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _DynamicAddUInteger(ulong value)
        {
            if (value < U_HALF_BYTE)
            {
                SerialAdd(0);
                Add((uint)value, 4);
            }
            else if (value < byte.MaxValue)
            {
                SerialAdd(1);
                Add((uint)value, 8);
            }
            else if (value < ushort.MaxValue)
            {
                SerialAdd(2);
                Add((uint)value, 16);
            }
            else if (value < BITS_U24_MAXVALUE)
            {
                SerialAdd(3);
                Add((uint)value, 24);
            }
            else if (value < uint.MaxValue)
            {
                SerialAdd(4);
                Add((uint)value, 32);
            } else
            {
                SerialAdd(5);
                Add(value, 64);
            }
        }

        /// <summary>
        /// Returns a ulong value added with <see cref="_DynamicAddUInteger(ulong)"/>
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ulong _DynamicGetUInteger()
        {
            int lenIndex = SerialGet();
            switch (lenIndex)
            {
                case 0: return GetUInt(4);
                case 1: return GetUInt(8);
                case 2: return GetUInt(16);
                case 3: return GetUInt(24);
                case 4: return GetUInt();
                case 5: return GetULong();
            }
            throw new Exception("Unrecognized lenIndex in _DynamicGetUInteger");
        }

        /// <summary>
        /// Adds a long and dynamically calculates the bits to occupy
        /// </summary>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void _DynamicAddInteger(long value)
        {
            var ovalue = value;
            if (value < 0) value = ~value;
            if (value < HALF_BYTE)
            {
                SerialAdd(0);
                Add((int)ovalue, 4);
            }
            else if (value < sbyte.MaxValue)
            {
                SerialAdd(1);
                Add((int)ovalue, 8);
            }
            else if (value < short.MaxValue)
            {
                SerialAdd(2);
                Add((int)ovalue, 16);
            }
            else if (value < BITS_24_MAXVALUE)
            {
                SerialAdd(3);
                Add((int)ovalue, 24);
            }
            else if (value < int.MaxValue)
            {
                SerialAdd(4);
                Add((int)ovalue, 32);
            }
            else
            {
                SerialAdd(5);
                Add(ovalue);
            }
        }

        /// <summary>
        /// Returns a long value added with <see cref="_DynamicAddInteger(long)"/>
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private long _DynamicGetInteger()
        {
            int lenIndex = SerialGet();
            switch (lenIndex)
            {
                case 0: return GetInt(4);
                case 1: return GetInt(8);
                case 2: return GetInt(16);
                case 3: return GetInt(24);
                case 4: return GetInt();
                case 5: return GetLong();
            }
            throw new Exception("Unrecognized lenIndex in _DynamicGetInteger");
        }

        private void _DynamicAddDecimal(decimal value)
        {
            
        }

        #endregion

        #region ------------------------------ Unsigned --------------------------------------        
        /// <summary>
        /// Adds a ulong and dynamically calculates the bits it will occupy 
        /// at the expense of speed
        /// </summary>
        /// <param name="value"></param>
        public void DynamicAdd(ulong value)
        {
            _DynamicAddUInteger(value);
        }

        /// <summary>
        /// Adds a uint and dynamically calculates the bits it will occupy 
        /// at the expense of speed
        /// </summary>
        /// <param name="value"></param>
        public void DynamicAdd(uint value)
        {
            _DynamicAddUInteger(value);
        }

        /// <summary>
        /// Adds a ushort and dynamically calculates the bits it will occupy 
        /// at the expense of speed
        /// </summary>
        /// <param name="value"></param>
        public void DynamicAdd(ushort value)
        {
            _DynamicAddUInteger((uint)value);
        }

        /// <summary>
        /// Adds a byte and dynamically calculates the bits it will occupy 
        /// at the expense of speed
        /// </summary>
        /// <param name="value"></param>
        public void DynamicAdd(byte value)
        {
            _DynamicAddUInteger((uint)value);
        }

        /// <summary>
        /// Gets a ulong value that was added with <see cref="DynamicAdd(uint)"/>
        /// </summary>
        /// <returns></returns>
        public uint DynamicGetUInt()
        {
            return (uint)_DynamicGetUInteger();
        }

        /// <summary>
        /// Gets a ulong value that was added with <see cref="DynamicAdd(ulong)"/>
        /// </summary>
        /// <returns></returns>
        public ulong DynamicGetULong()
        {
            return _DynamicGetUInteger();
        }

        /// <summary>
        /// Gets a ushort that was added with <see cref="DynamicAdd(ushort)"/>
        /// </summary>
        /// <returns></returns>
        public ushort DynamicGetUShort()
        {
            return (ushort)_DynamicGetUInteger();
        }

        /// <summary>
        /// Gets a ushort that was added with <see cref="DynamicAdd(byte)"/>
        /// </summary>
        /// <returns></returns>
        public byte DynamicGetByte()
        {
            return (byte)_DynamicGetUInteger();
        }
        #endregion

        #region ------------------------------ Signed ----------------------------------------
        /// <summary>
        /// Adds a long and dynamically calculates the bits it will occupy 
        /// at the expense of speed
        /// </summary>
        /// <param name="value"></param>
        public void DynamicAdd(long value)
        {
            _DynamicAddInteger(value);
        }

        /// <summary>
        /// Adds a int and dynamically calculates the bits it will occupy 
        /// at the expense of speed
        /// </summary>
        /// <param name="value"></param>
        public void DynamicAdd(int value)
        {
            _DynamicAddInteger(value);
        }

        /// <summary>
        /// Adds a short and dynamically calculates the bits it will occupy 
        /// at the expense of speed
        /// </summary>
        /// <param name="value"></param>
        public void DynamicAdd(short value)
        {
            _DynamicAddInteger(value);
        }

        /// <summary>
        /// Adds a sbyte and dynamically calculates the bits it will occupy 
        /// at the expense of speed
        /// </summary>
        /// <param name="value"></param>
        public void DynamicAdd(sbyte value)
        {
            _DynamicAddInteger(value);
        }




        /// <summary>
        /// Gets a long value that was added with <see cref="DynamicAdd(long)"/>
        /// </summary>
        /// <returns></returns>
        public long DynamicGetLong()
        {
            return _DynamicGetInteger();
        }

        /// <summary>
        /// Gets a int value that was added with <see cref="DynamicAdd(int)"/>
        /// </summary>
        /// <returns></returns>
        public int DynamicGetInt()
        {
            return (int)_DynamicGetInteger();
        }

        /// <summary>
        /// Gets a short value that was added with <see cref="DynamicAdd(short)"/>
        /// </summary>
        /// <returns></returns>
        public short DynamicGetShort()
        {
            return (short)_DynamicGetInteger();
        }

        /// <summary>
        /// Gets a sbyte value that was added with <see cref="DynamicAdd(sbyte)"/>
        /// </summary>
        /// <returns></returns>
        public sbyte DynamicGetSByte()
        {
            return (sbyte)_DynamicGetInteger();
        }



        #endregion

        #region ------------------------------ Decimal ---------------------------------------

        #endregion

        #region ------------------------------ Const -----------------------------------------
        const long U_HALF_BYTE = 16;
        static readonly ulong BITS_U24_MAXVALUE = (ulong)Math.Pow(2, 24);

        const long HALF_BYTE = 8;
        static readonly long BITS_24_MAXVALUE = (long)Math.Pow(2, 23);
        #endregion
    }
}
