using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using RoboticsLibrary.Errors;

namespace RoboticsLibrary.Utilities
{
    public static class UtilMain
    {
                 
         /// <summary>
         /// <c>IsNumericType(Type Type)</c>
         /// Determines if the given type
         /// is numeric. 
         /// </summary>
         /// <returns>
         /// Returns <c>true</c> if 
         /// param <param> Type </param> is a numeric, 
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
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        /// <summary>
        /// Converts a byte array into a 32 bit integer.
        /// * * * Caution integer overflow.
        /// </summary>
        /// <param name="Array">Byte array given for int conversion.</param>
        /// <param name="StartIndex">
        /// Defaults to 0. The start index in the bytearray for integer conversion.</param>
        /// <returns>
        /// 32-bit int representation of the given byte array.
        /// Throws <c>OverflowException</c> if integer overflow occurs.
        /// </returns>
        public static int ByteArrayToInt(byte[] Array, int StartIndex = 0)
        {
            if ((Array.Length * 8 - StartIndex) <= 32)
            {
                return BitConverter.ToInt32(Array, StartIndex);
            }
            throw new OverflowException();
        }

    }

}
