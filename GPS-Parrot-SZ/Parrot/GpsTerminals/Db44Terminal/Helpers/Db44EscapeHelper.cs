using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Db44
{
    public static class Db44EscapeHelper
    {
        /// <summary>
        /// 转义。若遇0x7d, 0x7e, 或0x7f，则将该字节与0x20异或，变成0x5d, 0x5e, 或0x5f，然后在前面插入一个字节0x7d。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Escape(byte[] data)
        {
            int num2;
            string str = "";
            byte[] buffer = null;
            for (num2 = 0; num2 < data.Length; num2++)
            {
                byte num = data[num2];
                if (((num != 0x7d) && (num != 0x7e)) && (num != 0x7f))
                {
                    str = str + " " + num.ToString("X2");
                }
                else
                {
                    str = str + " 7D";
                    str = str + " " + ((byte)(num ^ 0x20)).ToString("X2");
                }
            }
            string[] strArray = str.Trim().Split(new char[] { ' ' });
            buffer = new byte[strArray.Length];
            for (num2 = 0; num2 < strArray.Length; num2++)
            {
                buffer[num2] = Convert.ToByte(strArray[num2], 0x10);
            }
            return buffer;
        }

        /// <summary>
        /// 反转义。若遇0x7d，则后续字节是经过转义后的字节，可能是0x5d, 0x5e, 0x5f三者之一，这个字节应当与0x20进行"或"运算，还原为0x7d, 0x7e, 0x7f。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ReverseEscape(byte[] data)
        {
            string str = "";

            int i;
            for (i = 0; i < data.Length; i++)
            {
                if (data[i] != 0x7d)
                {
                    str = str + " " + data[i].ToString("X2");
                }
                else
                {
                    str = str + " " + ((byte)(data[i + 1] ^ 0x20)).ToString("X2");
                    i++;
                }
            }
            string[] strArray = str.Trim().Split(new char[] { ' ' });

            byte[] result = new byte[strArray.Length];
            for (i = 0; i < strArray.Length; i++)
            {
                result[i] = Convert.ToByte(strArray[i].Trim(), 16);
            }
            return result;
        }
    }
}
