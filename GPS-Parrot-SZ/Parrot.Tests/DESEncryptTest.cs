using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Parrot.Cryptography;

namespace Omc.Tests
{
    
    
    /// <summary>
    ///This is a test class for DESEncryptTest and is intended
    ///to contain all DESEncryptTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DESEncryptTest
    {

        /// <summary>
        ///A test for Encrypt
        ///</summary>
        [TestMethod()]
        public void Encrypt()
        {
            string Text = "dgchkt";
            string expected = "01F53C224577323B";//"6147C272E4D2255C";
            string actual;
            actual = DESEncrypt.Encrypt(Text);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void Decrypt()
        {
            Assert.AreEqual("dgchkt",DESEncrypt.Decrypt("01F53C224577323B"));
            Assert.AreEqual("jxgps",DESEncrypt.Decrypt("E1A0797778296090"));
        }

        [TestMethod()]
        public void TestAppendCode()
        {
            Assert.AreEqual("++", System.Text.Encoding.Default.GetString(Convert.FromBase64String("Kys=")));
            Assert.AreEqual("200", System.Text.Encoding.Default.GetString(Convert.FromBase64String("MjAw")));
        }
    }
}
