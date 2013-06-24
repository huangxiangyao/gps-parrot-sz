using Parrot.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Parrot.Tests
{
    
    
    /// <summary>
    ///This is a test class for MdtIdHelperTest and is intended
    ///to contain all MdtIdHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MdtIdHelperTest
    {


        /// <summary>
        ///A test for GetMdtId
        ///</summary>
        [TestMethod()]
        public void GetMdtIdTest()
        {
            string mobileId = "01002010258";
            uint expected = 0x0A142812;
            uint actual;
            actual = MdtIdHelper.ParseMdtCode(mobileId);
            Assert.AreEqual(expected, actual);
        }
    }
}
