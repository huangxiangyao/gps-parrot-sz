using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parrot.Models;

namespace Omc.Tests
{
    
    
    /// <summary>
    ///This is a test class for ConnectionStringManagerTest and is intended
    ///to contain all ConnectionStringManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConnectionStringManagerTest
    {





        /// <summary>
        ///A test for EntityConnectionString
        ///</summary>
        [TestMethod()]
        public void EntityConnectionStringTest()
        {
            //ConnectionStringManager.Default.SetProperties("Omc", "SysInfoCenter", "sa", "4848285");
            ConnectionStringManager.Default.SetProperties("Omc", "SysInfoCenter");
            string result = ConnectionStringManager.Default.EntityConnectionString;
            
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreEqual("metadata=res://*/Models.ParrotModel.csdl|res://*/Models.ParrotModel.ssdl|res://*/Models.ParrotModel.msl;provider=System.Data.SqlClient;provider connection string=\"Data Source=Omc;Initial Catalog=SysInfoCenter;Integrated Security=True;MultipleActiveResultSets=True\"", result);
        }
    }
}
