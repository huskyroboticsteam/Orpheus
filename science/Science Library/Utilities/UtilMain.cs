using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

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

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static int ByteArrayToInt(byte[] Array, int StartIndex = 0)
        {
            return BitConverter.ToInt32(Array, StartIndex);
        }

    }

}
