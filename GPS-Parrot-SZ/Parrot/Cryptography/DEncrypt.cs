using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Parrot.Cryptography
{
    public class DEncrypt
    {
        // Methods
        public static string Decrypt(string original)
        {
            return Decrypt(original, "LITIANPING", Encoding.Default);
        }

        public static byte[] Decrypt(byte[] encrypted)
        {
            byte[] bytes = Encoding.Default.GetBytes("LITIANPING");
            return Decrypt(encrypted, bytes);
        }

        public static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = MakeMD5(key);
            provider.Mode = CipherMode.ECB;
            return provider.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        public static string Decrypt(string original, string key)
        {
            return Decrypt(original, key, Encoding.Default);
        }

        public static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buffer = Convert.FromBase64String(encrypted);
            byte[] bytes = Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buffer, bytes));
        }

        public static string Encrypt(string original)
        {
            return Encrypt(original, "LITIANPING");
        }

        public static byte[] Encrypt(byte[] original)
        {
            byte[] bytes = Encoding.Default.GetBytes("LITIANPING");
            return Encrypt(original, bytes);
        }

        public static string Encrypt(string original, string key)
        {
            byte[] bytes = Encoding.Default.GetBytes(original);
            byte[] buffer2 = Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(bytes, buffer2));
        }

        public static byte[] Encrypt(byte[] original, byte[] key)
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = MakeMD5(key);
            provider.Mode = CipherMode.ECB;
            return provider.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        public static byte[] MakeMD5(byte[] original)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(original);
            return buffer;
        }
    }
}