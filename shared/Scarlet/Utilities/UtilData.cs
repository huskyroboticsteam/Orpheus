using System;
using System.Linq;
using System.Text;

namespace Scarlet.Utilities
{
    public static class UtilData
    {
        public static byte[] Reverse(byte[] Input)
        {
            if (BitConverter.IsLittleEndian) { Array.Reverse(Input); }
            return Input;
        }

        public static byte[] ToBytes(bool Input) { return Reverse(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(char Input) { return Reverse(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(double Input) { return Reverse(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(float Input) { return Reverse(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(int Input) { return Reverse(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(long Input) { return Reverse(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(short Input) { return Reverse(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(uint Input) { return Reverse(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(ulong Input) { return Reverse(BitConverter.GetBytes(Input)); }
        public static byte[] ToBytes(ushort Input) { return Reverse(BitConverter.GetBytes(Input)); }
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
            return BitConverter.ToBoolean(Reverse(Input), 0);
        }

        public static char ToChar(byte[] Input)
        {
            if (Input.Length != 2) { throw new FormatException("Given byte[] does not convert to char."); }
            return BitConverter.ToChar(Reverse(Input), 0);
        }

        public static double ToDouble(byte[] Input)
        {
            if (Input.Length != 8) { throw new FormatException("Given byte[] does not convert to double."); }
            return BitConverter.ToDouble(Reverse(Input), 0);
        }

        public static float ToFloat(byte[] Input)
        {
            if (Input.Length != 4) { throw new FormatException("Given byte[] does not convert to float."); }
            return BitConverter.ToSingle(Reverse(Input), 0);
        }

        public static int ToInt(byte[] Input)
        {
            if (Input.Length != 4) { throw new FormatException("Given byte[] does not convert to int."); }
            return BitConverter.ToInt32(Reverse(Input), 0);
        }

        public static long ToLong(byte[] Input)
        {
            if (Input.Length != 8) { throw new FormatException("Given byte[] does not convert to long."); }
            return BitConverter.ToInt64(Reverse(Input), 0);
        }

        public static short ToShort(byte[] Input)
        {
            if (Input.Length != 2) { throw new FormatException("Given byte[] does not convert to short."); }
            return BitConverter.ToInt16(Reverse(Input), 0);
        }

        public static uint ToUInt(byte[] Input)
        {
            if (Input.Length != 4) { throw new FormatException("Given byte[] does not convert to uint."); }
            return BitConverter.ToUInt32(Reverse(Input), 0);
        }

        public static ulong ToULong(byte[] Input)
        {
            if (Input.Length != 8) { throw new FormatException("Given byte[] does not convert to ulong."); }
            return BitConverter.ToUInt64(Reverse(Input), 0);
        }

        public static ushort ToUShort(byte[] Input)
        {
            if (Input.Length != 2) { throw new FormatException("Given byte[] does not convert to ushort."); }
            return BitConverter.ToUInt16(Reverse(Input), 0);
        }

        public static string ToString(byte[] Input)
        {
            if (Input.Length == 0 || Input.Length % 2 == 1) { throw new FormatException("Given byte[] does not convert to string."); }
            StringBuilder Output = new StringBuilder(Input.Length / 2);
            for(int i = 0; i < Input.Length / 2; i += 2)
            {
                Output.Append((char)(Input[i] << 8 | Input[i + 1]));
            }
            return Output.ToString();
        }
    }
}
