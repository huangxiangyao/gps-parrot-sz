using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Db44.Security.Cryptography.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Db44SymmetricCryptoProvider provider = EnterpriseLibraryContainer.Current.GetInstance<Db44SymmetricCryptoProvider>("Terminal251");

            string plaintext = "01 02 03 04 05";
            Console.WriteLine(plaintext);
            
            byte[] data1 = Util.HexToBytes(plaintext);
            byte[] data2 = provider.Encrypt(data1);
            Console.WriteLine(Util.BytesToHex(data2,true));

            data2 = provider.Encrypt(data2);
            Console.WriteLine(Util.BytesToHex(data2, true));

            data2 = provider.Encrypt(data2);
            Console.WriteLine(Util.BytesToHex(data2, true));

            data2 = provider.Encrypt(data2);
            Console.WriteLine(Util.BytesToHex(data2, true));
        }
    }
}
