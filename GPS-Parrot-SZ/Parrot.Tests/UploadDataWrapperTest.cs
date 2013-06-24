using Parrot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Parrot.Models;

namespace Parrot.Tests
{
    
    
    /// <summary>
    ///This is a test class for UploadDataWrapperTest and is intended
    ///to contain all UploadDataWrapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UploadDataWrapperTest
    {

        /// <summary>
        ///A test for U03
        ///</summary>
        [TestMethod()]
        public void U03Test()
        {
            int clientId = 0x1C;
            ConnectionStringManager.Default.SetProperties("Omc", "SysInfoCenter");
            CarList mdt = ParrotModelWrapper.GetMdtByPlateNumber("粤S79903");
            //string expected = "7E 55 30 33 26 00 00 00 1C 26 00 00 00 44 26 00 00 00 1C 7C 4E 52 32 30 31 30 2D 41 7C 16 11 0B 0E 3A 81 7C D4 C1 53 37 39 39 30 33 7C 02 7C 7C 7C 00 7C 7C B6 AB DD B8 CA D0 B3 A3 C6 BD D6 D2 C0 FB CD C1 CA AF B7 BD B9 A4 B3 CC B7 FE CE F1 B2 BF 7C 23 ";
            string expected = "7E 55 30 33 26 00 00 00 1C 26 00 00 00 44 26 00 00 00 1C 7C 4E 52 32 30 31 30 2D 41 7C 16 11 0B 0E 3A 81 7C D4 C1 53 37 39 39 30 33 7C 02 7C 7C 7C 0C 7C 7C B6 AB DD B8 CA D0 B3 A3 C6 BD D6 D2 C0 FB CD C1 CA AF B7 BD B9 A4 B3 CC B7 FE CE F1 B2 BF 7C 23 ";
            byte[] actual;
            actual = UploadDataWrapper.U03(clientId, mdt);
            Console.WriteLine(Util.BytesToHex(actual, true));
            Assert.AreEqual(expected,Util.BytesToHex(actual,true));
        }
    }
}
