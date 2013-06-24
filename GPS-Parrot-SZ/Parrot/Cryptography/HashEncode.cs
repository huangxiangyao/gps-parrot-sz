using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Parrot.Cryptography
{
    public class HashEncode
    {
        // Methods
        public static string GetRandomValue()
        {
            Random random = new Random();
            return random.Next(1, 0x7fffffff).ToString();
        }

        public static string GetSecurity()
        {
            return HashEncoding(GetRandomValue());
        }

        public static string HashEncoding(string Security)
        {
            byte[] bytes = new UnicodeEncoding().GetBytes(Security);
            byte[] buffer = new SHA512Managed().ComputeHash(bytes);
            Security = "";
            foreach (byte num in buffer)
            {
                Security = Security + ((int)num) + "O";
            }
            return Security;
        }
    }

}