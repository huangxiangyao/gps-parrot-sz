using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot
{
    public static class NumberConverter
    {
        public static ushort GetUInt16(byte[] buffer, int offset)
        {
            return (ushort)((buffer[offset] << 8) | buffer[offset + 1]);
        }

        public static uint GetUInt32(byte[] buffer, int offset)
        {
            return (uint)((buffer[offset] << 24) | (buffer[offset + 1] << 16) | (buffer[offset + 2] << 8) | (buffer[offset + 3]));
        }

        /// <summary>
        /// 将一个字符串从某进制转换为另一进制。无论以哪种进制表示，该字符串最大能表达一个4字节整型数值。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fromBase"></param>
        /// <param name="toBase"></param>
        /// <returns></returns>
        public static string ConvertForInt32(string value, int fromBase, int toBase)
        {
            return Convert.ToString(Convert.ToInt32(value, fromBase), toBase);
        }

        /// <summary>
        /// 将一个2位正整数转换为BCD码。
        /// </summary>
        /// <param name="value">2位正整数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">输入的参数大于99。</exception>
        public static byte ToBcd(byte value)
        {
            if (value > 99) throw new ArgumentOutOfRangeException();

            return (byte)(((value/10)<<4)|(value%10));
        }
    }
}
