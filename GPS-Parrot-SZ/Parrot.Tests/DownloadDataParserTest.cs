using Parrot.Protocols.Jtj;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Parrot.Tests
{
    
    
    /// <summary>
    ///This is a test class for DownloadDataParserTest and is intended
    ///to contain all DownloadDataParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DownloadDataParserTest
    {
        /// <summary>
        ///A test for D04
        ///</summary>
        [TestMethod()]
        public void D04Test_2()
        {
            string pduStr = "7E 44 30 34 26 00 00 00 1C 26 00 00 00 0A 26 D4 C1 53 33 39 32 39 35 7C 04 23";
            byte[] pdu = Util.HexToBytes(pduStr);
            string plateNumber;
            string plateNumberExpected = "粤S39295";
            byte plateColor;
            byte plateColorExpected = 4;
            bool expected = true;
            bool actual;
            actual = DownloadDataParser.D04(pdu, out plateNumber, out plateColor);
            Assert.AreEqual(plateNumberExpected, plateNumber);
            Assert.AreEqual(plateColorExpected, plateColor);
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        ///A test for D04
        ///</summary>
        [TestMethod()]
        public void D04Test()
        {
            string pduStr ="7E 44 30 34 26 00 00 00 1C 26 00 00 00 0B 26 D4 C1 53 34 34 37 33 D1 A7 7C 00 23";
            byte[] pdu = Util.HexToBytes(pduStr);
            string plateNumber;
            string plateNumberExpected = "粤S4473学";
            byte plateColor;
            byte plateColorExpected = 0;
            bool expected = true;
            bool actual;
            actual = DownloadDataParser.D04(pdu, out plateNumber, out plateColor);
            Assert.AreEqual(plateNumberExpected, plateNumber);
            Assert.AreEqual(plateColorExpected, plateColor);
            Assert.AreEqual(expected, actual);
        }
    }
}
