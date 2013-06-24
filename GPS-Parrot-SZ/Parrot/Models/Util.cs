using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot
{
    public static class Util
    {
        /// <summary>
        /// 将字节数组转换为一个Hex字符串。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="splitBySpace">输出时是否用空格分隔每个字节。</param>
        /// <returns></returns>
        public static string BytesToHex(byte[] data, bool splitBySpace = false)
        {
            if(data==null) throw new ArgumentNullException("data");

            StringBuilder hex = new StringBuilder(data.Length * 2);
            if (splitBySpace)
            {
                foreach (byte b in data)
                    hex.AppendFormat("{0:X2} ", b);
            }
            else
            {
                foreach (byte b in data)
                    hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
        }

        /// <summary>
        /// 将Hex字符串转换为字节数组。
        /// </summary>
        /// <param name="hex">Hex字符串。</param>
        /// <param name="hasSpaceSplit">指示Hex字符串中是否用了空格来分隔每个字节。</param>
        /// <returns></returns>
        public static byte[] HexToBytes(String hex, bool hasSpaceSplit = true)
        {
            if (hasSpaceSplit) hex = hex.Replace(" ", "");

            int nChars = hex.Length;
            byte[] bytes = new byte[nChars / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i*2, 2), 16);
            return bytes;
        }
    }
}
