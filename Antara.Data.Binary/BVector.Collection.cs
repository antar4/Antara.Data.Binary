using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antara.Data.Binary
{
    public sealed partial class BVector
    {
        #region ------------------------------ Collection - Generic --------------------------
        public void Add<T>(List<T> list)
        {
            var type = typeof(T);
            if (!SupportedTypes.Contains(type))
            {
                throw new ArgumentException($"Type {typeof(T)} is not supported by BVector collections.");
            }
            var count = list.Count;
			if (count == 0)
            {
                Add1(false);
                return;
            }
            Add1(true);
            var code = Type.GetTypeCode(type);
            var itemSize = GetSize(code);
            _AddObjectLen((uint)list.Count, itemSize);
            switch (code)
            {
                case TypeCode.Boolean:
                    foreach (var item in list)
                    {
                        Add1((bool)(Convert.ChangeType(item, typeof(bool))));
                    }
                    break;

                case TypeCode.Byte:
                    foreach (var item in list)
                    {
                        Add((byte)(Convert.ChangeType(item, typeof(byte))));
                    }
                    break;

                case TypeCode.Char:
                    foreach (var item in list)
                    {
                        Add((char)(Convert.ChangeType(item, typeof(char))));
                    }
                    break;

                case TypeCode.DateTime:
                    foreach (var item in list)
                    {
                        Add((DateTime)(Convert.ChangeType(item, typeof(DateTime))));
                    }
                    break;


                case TypeCode.Decimal:
                    foreach (var item in list)
                    {
                        Add((decimal)(Convert.ChangeType(item, typeof(decimal))));
                    }
                    break;

                case TypeCode.Double:
                    foreach (var item in list)
                    {
                        Add((double)(Convert.ChangeType(item, typeof(double))));
                    }
                    break;

                case TypeCode.Int16:
                    foreach (var item in list)
                    {
                        Add((short)(Convert.ChangeType(item, typeof(short))));
                    }
                    break;

                case TypeCode.Int32:
                    foreach (var item in list)
                    {
                        Add((int)(Convert.ChangeType(item, typeof(int))));
                    }
                    break;

                case TypeCode.Int64:
                    foreach (var item in list)
                    {
                        Add((long)(Convert.ChangeType(item, typeof(long))));
                    }
                    break;

                case TypeCode.SByte:
                    foreach (var item in list)
                    {
                        Add((sbyte)(Convert.ChangeType(item, typeof(sbyte))));
                    }
                    break;

                case TypeCode.Single:
                    foreach (var item in list)
                    {
                        Add((float)(Convert.ChangeType(item, typeof(float))));
                    }
                    break;

                case TypeCode.String:
                    foreach (var item in list)
                    {
                        Add((string)(Convert.ChangeType(item, typeof(string))));
                    }
                    break;

                case TypeCode.UInt16:
                    foreach (var item in list)
                    {
                        Add((ushort)(Convert.ChangeType(item, typeof(ushort))));
                    }
                    break;

                case TypeCode.UInt32:
                    foreach (var item in list)
                    {
                        Add((uint)(Convert.ChangeType(item, typeof(uint))));
                    }
                    break;

                case TypeCode.UInt64:
                    foreach (var item in list)
                    {
                        Add((ulong)(Convert.ChangeType(item, typeof(ulong))));
                    }
                    break;
            }
            
        }
        public List<T> GetList<T>()
        {
            var type = typeof(T);
            if (!SupportedTypes.Contains(type))
            {
                throw new ArgumentException($"Type {typeof(T)} is not supported by BVector collections.");
            }
            
            if (!Get1()) return new List<T>();
            var code = Type.GetTypeCode(type);
            var len = _GetObjectLen();
            var result = new List<T>(len);
            
            switch (code)
            {
                case TypeCode.Boolean:
                    for (var i = 0;i < len;i++)
                    {
                        result.Add((T)Convert.ChangeType(Get1(), type));
                    }
                    break;

                case TypeCode.Byte:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetByte(), type));
                    }
                    break;

                case TypeCode.Char:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetChar(), type));
                    }
                    break;

                case TypeCode.DateTime:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetDateTime(), type));
                    }
                    break;


                case TypeCode.Decimal:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetDecimal(), type));
                    }
                    break;

                case TypeCode.Double:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetDouble(), type));
                    }
                    break;

                case TypeCode.Int16:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetShort(), type));
                    }
                    break;

                case TypeCode.Int32:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetInt(), type));
                    }
                    break;

                case TypeCode.Int64:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetLong(), type));
                    }
                    break;

                case TypeCode.SByte:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetSByte(), type));
                    }
                    break;

                case TypeCode.Single:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetFloat(), type));
                    }
                    break;

                case TypeCode.String:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetString(), type));
                    }
                    break;

                case TypeCode.UInt16:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetUShort(), type));
                    }
                    break;

                case TypeCode.UInt32:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetUInt(), type));
                    }
                    break;

                case TypeCode.UInt64:
                    for (var i = 0; i < len; i++)
                    {
                        result.Add((T)Convert.ChangeType(GetULong(), type));
                    }
                    break;
            }


            return result;
        }
        #endregion


        #region ------------------------------ Helper ----------------------------------------
        private static List<Type> _SupportedTypes;
        private static List<Type> SupportedTypes
        {
            get
            {
                if (_SupportedTypes == null)
                {
                    var typeCodes = typeof(TypeCode).GetEnumValues();
                    _SupportedTypes = new List<Type>();
                    foreach (TypeCode t in typeCodes)
                    {
                        var _t = ToType(t);
                        if (_t != null)
                        {
                            _SupportedTypes.Add(_t);
                        }

                    }
                }
                return _SupportedTypes;
            }
        }

        private static Type ToType(TypeCode code)
        {
            switch (code)
            {
                case TypeCode.Boolean:
                    return typeof(bool);

                case TypeCode.Byte:
                    return typeof(byte);

                case TypeCode.Char:
                    return typeof(char);

                case TypeCode.DateTime:
                    return typeof(DateTime);

                case TypeCode.Decimal:
                    return typeof(decimal);

                case TypeCode.Double:
                    return typeof(double);

                case TypeCode.Int16:
                    return typeof(short);

                case TypeCode.Int32:
                    return typeof(int);

                case TypeCode.Int64:
                    return typeof(long);

                case TypeCode.SByte:
                    return typeof(sbyte);

                case TypeCode.Single:
                    return typeof(Single);

                case TypeCode.String:
                    return typeof(string);

                case TypeCode.UInt16:
                    return typeof(UInt16);

                case TypeCode.UInt32:
                    return typeof(UInt32);

                case TypeCode.UInt64:
                    return typeof(UInt64);
            }

            return null;
        }
		private static byte GetSize(TypeCode code)
        {

            switch (code)
            {
                case TypeCode.Boolean:
                    return 1;

                case TypeCode.Byte:
                    return 8;

                case TypeCode.Char:
                    return 8;

                case TypeCode.DateTime:
                    return 64;

                case TypeCode.Decimal:
                    return 128;

                case TypeCode.Double:
                    return 64;

                case TypeCode.Int16:
                    return 16;

                case TypeCode.Int32:
                    return 32;

                case TypeCode.Int64:
                    return 64;

                case TypeCode.SByte:
                    return 8;

                case TypeCode.Single:
                    return 16;

                case TypeCode.String:
                    return 16;

                case TypeCode.UInt16:
                    return 16;

                case TypeCode.UInt32:
                    return 32;

                case TypeCode.UInt64:
                    return 64;
            }
            return 0;
        }
        #endregion

    }
}
