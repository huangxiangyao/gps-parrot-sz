using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Parrot;

namespace VehicleMis.Tests
{
    
    
    /// <summary>
    ///This is a test class for Db44VehicleStateTest and is intended
    ///to contain all Db44VehicleStateTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Db44VehicleStateTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Db44VehicleState Constructor
        ///</summary>
        [TestMethod()]
        public void TestDb44VehicleStateConstructor()
        {
            byte[] data = null; // TODO: Initialize to an appropriate value
            Db44VehicleState target = new Db44VehicleState(data);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Db44VehicleState Constructor
        ///</summary>
        [TestMethod()]
        public void TestDb44VehicleStateConstructor1()
        {
            Db44VehicleState target = new Db44VehicleState();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for GetValue
        ///</summary>
        [TestMethod()]
        public void TestGetValue()
        {
            Db44VehicleState target = new Db44VehicleState(); // TODO: Initialize to an appropriate value
            int byteIndex = 0; // TODO: Initialize to an appropriate value
            int bitIndex = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.GetValue(byteIndex, bitIndex);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SetValue
        ///</summary>
        [TestMethod()]
        public void TestSetValue()
        {
            Db44VehicleState target = new Db44VehicleState(); // TODO: Initialize to an appropriate value
            int byteIndex = 0; // TODO: Initialize to an appropriate value
            int bitIndex = 0; // TODO: Initialize to an appropriate value
            bool value = false; // TODO: Initialize to an appropriate value
            target.SetValue(byteIndex, bitIndex, value);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ToReadableStatusText
        ///</summary>
        [TestMethod()]
        public void TestToReadableStatusText()
        {
            Db44VehicleState target = new Db44VehicleState(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToReadableStatusText();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Data
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Parrot.exe")]
        public void TestData()
        {
            Db44VehicleState target = new Db44VehicleState(); // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            target.Data = expected;
            actual = target.Data;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
