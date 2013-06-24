using Parrot.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Parrot.Tests
{
    
    
    /// <summary>
    ///This is a test class for ParrotModelWrapperTest and is intended
    ///to contain all ParrotModelWrapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ParrotModelWrapperTest
    {
        /// <summary>
        ///A test for GetUserInfo
        ///</summary>
        [TestMethod()]
        public void GetUserInfoTest()
        {
            string username = "123456";
            string password = "123456";
            ConnectionStringManager.Default.SetProperties("Omc", "SysInfoCenter");
            UserInfo actual;
            actual = ParrotModelWrapper.GetUserInfo(username, password);
            Assert.AreEqual(username, actual.User_Name);
        }

        /// <summary>
        ///A test for GetAllMdts
        ///</summary>
        [TestMethod()]
        public void GetAllMdtsTest()
        {
            ConnectionStringManager.Default.SetProperties("Omc", "SysInfoCenter");

            List<MobileInfoList> actual;
            actual = ParrotModelWrapper.GetAllMdts();
            Assert.IsTrue(actual.Count > 0);
        }
    }
}
