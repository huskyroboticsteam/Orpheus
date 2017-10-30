using System;
using System.Linq;
using System.Text;

namespace Scarlet.Utilities
{
    public static class UtilData
    {
        internal static byte[] EnsureBigEndian(byte[] Input)
        {
            if (BitConverter.IsLittleEndian) { Array.Reverse(Input); }
            return Input;
        }

        public static byte[] ToBytes(bool Input) { return EnsureBigEndian(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(char Input) { return EnsureBigEndian(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(double Input) { return EnsureBigEndian(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(float Input) { return EnsureBigEndian(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(int Input) { return EnsureBigEndian(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(long Input) { return EnsureBigEndian(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(short Input) { return EnsureBigEndian(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(uint Input) { return EnsureBigEndian(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(ulong Input) { return EnsureBigEndian(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(ushort Input) { return EnsureBigEndian(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(string Input)
        {
            if(Input == null || Input.Length == 0) { return new byte[0]; }
            char[] Characters = Input.ToCharArray();
            byte[] Output = new byte[Characters.Length * 2];
            for(int i = 0; i < Characters.Length; i++)
            {
                Output[i * 2] = (byte)(Characters[i] >> 8);
                Output[(i * 2) + 1] = (byte)(Characters[i]);
            }
            return Output;
        }

        public static bool ToBool(byte[] Input)
        {
            if (Input.Length != 1) { throw new FormatException("Given byte[] does not convert to bool."); }
            return BitConverter.ToBoolean(EnsureBigEndian(Input), 0);
        }

        public static char ToChar(byte[] Input)
        {
            if (Input.Length != 2) { throw new FormatException("Given byte[] does not convert to char."); }
            return BitConverter.ToChar(EnsureBigEndian(Input), 0);
        }

        public static double ToDouble(byte[] Input)
        {
            if (Input.Length != 8) { throw new FormatException("Given byte[] does not convert to double."); }
            return BitConverter.ToDouble(EnsureBigEndian(Input), 0);
        }

        public static float ToFloat(byte[] Input)
        {
            if (Input.Length != 4) { throw new FormatException("Given byte[] does not convert to float."); }
            return BitConverter.ToSingle(EnsureBigEndian(Input), 0);
        }

        public static int ToInt(byte[] Input)
        {
            if (Input.Length != 4) { throw new FormatException("Given byte[] does not convert to int."); }
            return BitConverter.ToInt32(EnsureBigEndian(Input), 0);
        }

        public static long ToLong(byte[] Input)
        {
            if (Input.Length != 8) { throw new FormatException("Given byte[] does not convert to long."); }
            return BitConverter.ToInt64(EnsureBigEndian(Input), 0);
        }

        public static short ToShort(byte[] Input)
        {
            if (Input.Length != 2) { throw new FormatException("Given byte[] does not convert to short."); }
            return BitConverter.ToInt16(EnsureBigEndian(Input), 0);
        }

        public static uint ToUInt(byte[] Input)
        {
            if (Input.Length != 4) { throw new FormatException("Given byte[] does not convert to uint."); }
            return BitConverter.ToUInt32(EnsureBigEndian(Input), 0);
        }

        public static ulong ToULong(byte[] Input)
        {
            if (Input.Length != 8) { throw new FormatException("Given byte[] does not convert to ulong."); }
            return BitConverter.ToUInt64(EnsureBigEndian(Input), 0);
        }

        public static ushort ToUShort(byte[] Input)
        {
            if (Input.Length != 2) { throw new FormatException("Given byte[] does not convert to ushort."); }
            return BitConverter.ToUInt16(EnsureBigEndian(Input), 0);
        }

        /// <summary>
        /// Returns a string from the byte
        /// representation of the string in
        /// unicode
        /// </summary>
        /// <param name="Input">Byte represensation of the unicode string</param>
        /// <returns>String representation of the bytes</returns>
        public static string ToString(byte[] Input)
        {
            if (Input == null || Input.Length == 0 || Input.Length % 2 == 1) { throw new FormatException("Given byte[] does not convert to string."); }
            StringBuilder Output = new StringBuilder(Input.Length / 2);
            for(int i = 0; i < Input.Length; i += 2)
            {
                Output.Append((char)(Input[i] << 8 | Input[i + 1]));
            }
            return Output.ToString();
        }

        /// <summary>
        /// Tries to convert a byte input (unicode) to a string
        /// </summary>
        /// <param name="Input">Bytes to convert to string</param>
        /// <param name="Output">Output of the conversion. (Null if failed)</param>
        /// <returns>True if string conversion succeeds</returns>
        public static bool TryToString(byte[] Input, out string Output)
        {
            Output = null; 
            try { Output = ToString(Input); }
            catch { return false; }
            return true;
        }

        /// <summary>
        /// <c>IsNumericType(Type Type)</c>
        /// Determines if the given type
        /// is numeric. 
        /// </summary>
        /// <param name="Type">Type to determine whether or not it is a numeric</param>
        /// <returns>
        /// Returns <c>true</c> if 
        /// param is a numeric; 
        /// otherwise returns <c>false</c>.</returns>
        public static bool IsNumericType(Type Type)
        {
            switch (Type.GetTypeCode(Type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

    }
}
