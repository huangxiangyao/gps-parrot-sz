using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Db44
{
    public class Db44Encryption
    {
        /// <summary>
        /// 加密协议头和协议内容。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="mdtModel">GPS终端类型。</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, int startPos, int endPos, int mdtModel)
        {
            return Encrypt(data, startPos, endPos, Db44EncryptionFactorRepository.Default.GetFactor(mdtModel));
        }

        /// <summary>
        /// 加密协议头和协议内容。算法请参见相关《协议》。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        public static byte[] Encrypt(byte[] data, int startPos, int endPos, Db44EncryptionFactor factor)
        {
            if (factor == null) throw new ArgumentNullException("factor");

            return Encrypt(data, startPos, endPos, factor.Key, factor.M1, factor.IA1, factor.IC1);
        }

        /// <summary>
        /// 加密协议头和协议内容。算法请参见相关《协议》。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="key"></param>
        /// <param name="m1"></param>
        /// <param name="ia1"></param>
        /// <param name="ic1"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] data, int startPos, int endPos,
            uint key, uint m1, uint ia1, uint ic1)
        {
            byte[] buffer = new byte[endPos - startPos];

            int i = 0;
            int idx = 0;

            if (key == 0)
            {
                key = 1;
            }
            for (i = startPos; i < endPos; i++)
            {
                key = (ia1 * (key % m1)) + ic1;
                byte num7 = (byte)((key >> 20) & 0xff);
                buffer[idx] = (byte)(data[i] ^ num7);
                idx++;
            }
            return buffer;
        }
    }
}
