using System;
using System.Collections.Generic;
using System.Text;

namespace Scarlet.Utilities
{
    public static class UtilMain
    {

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

        /// <summary>
        /// Returns subarray of given array.
        /// </summary>
        /// <typeparam name="T">
        /// Datatype of array
        /// </typeparam>
        /// <param name="data">Array to manipulate</param>
        /// <param name="index">Starting index of subarray.</param>
        /// <param name="length">Length of wanted subarray.</param>
        /// <returns>
        /// Sub array of data[index:index+length-1] (inclusive)
        /// </returns>
        public static T[] SubArray<T>(T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Gives a user-readable representation of a byte array.
        /// </summary>
        /// <param name="Data">The array to format.</param>
        /// <param name="Spaces">Whether to add spaces between every byte in the output</param>
        /// <returns>A string formatted as such: "4D 3A 20 8C", or "4D3A208C", depending on the Spaces parameter.</returns>
        public static string BytesToNiceString(byte[] Data, bool Spaces)
        {
            if(Data == null) { return string.Empty; }
            StringBuilder Output = new StringBuilder();
            for (int i = 0; i < Data.Length; i++)
            {
                Output.Append(Data[i].ToString("X2"));
                if (Spaces) { Output.Append(' '); }
            }
            if (Spaces) { Output.Remove(Output.Length - 1, 1); }
            return Output.ToString();
        }
        
        /// <summary>
        /// Takes in a string,
        /// converts the string
        /// into its byte representation.
        /// </summary>
        /// <param name="Data">
        /// String to convert into bytes</param>
        /// <returns>
        /// Byte array that represents the given string.</returns>
        public static byte[] StringToBytes(string Data)
        {
            List<byte> Output = new List<byte>();
            Data = Data.Replace(" ", "");
            for (int Chunk = 0; Chunk < Math.Ceiling(Data.Length / 2.000); Chunk++)
            {
                int Start = Data.Length - ((Chunk + 1) * 2);
                string Section;
                if (Start >= 0) { Section = Data.Substring(Start, 2); }
                else { Section = Data.Substring(0, 1); }
                Output.Add(Convert.ToByte(Section, 16));
            }
            return Output.ToArray();
        }

    }

}
