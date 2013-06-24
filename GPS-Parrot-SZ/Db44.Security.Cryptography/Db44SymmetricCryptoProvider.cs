using System;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Db44.Security.Cryptography
{
    /// <summary>
    /// DB44协议书中定义的加密算法。
    /// </summary>
    [ConfigurationElementType(typeof(CustomSymmetricCryptoProviderData))]
    public class Db44SymmetricCryptoProvider : ISymmetricCryptoProvider
    {
        private readonly uint IA1;
        private readonly uint IC1;
        private readonly uint M1;
        private readonly uint Key;

        public Db44SymmetricCryptoProvider(NameValueCollection attributes)
        {
            IA1 = uint.Parse(attributes["IA1"]);
            IC1 = uint.Parse(attributes["IC1"]);
            M1 = uint.Parse(attributes["M1"]);
            Key = uint.Parse(attributes["Key"]);
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            return Encrypt(ciphertext, 0, ciphertext.Length, Key, M1, IA1, IC1);
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            return Encrypt(plaintext, 0, plaintext.Length, Key, M1, IA1, IC1);
        }


        /// <summary>
        /// 加密协议头和协议内容。算法请参见相关《DB44协议》。
        /// 这是一种循环对称加密/解密算法，即：加密算法与解密算法完全一样。
        /// 该算法因子有：IA1,IC1,M1,Key四个。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="key"></param>
        /// <param name="m1"></param>
        /// <param name="ia1"></param>
        /// <param name="ic1"></param>
        /// <returns></returns>
        private static byte[] Encrypt(byte[] data, int startPos, int endPos,
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
