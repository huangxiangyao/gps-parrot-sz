using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parrot.Models;
using System.Linq;

namespace Omc.Tests
{
    /// <summary>
    /// Summary description for EFTests
    /// </summary>
    [TestClass]
    public class EFTest
    {
        public EFTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        [TestMethod]
        public void DefaultConnectionStringTest()
        {
            //VehicleEntities ctx = new VehicleEntities();
            //int id = 322;
            //CarList result = ctx.CarLists.FirstOrDefault(q => q.id == id);
            //Assert.IsNotNull(result);
            //Assert.AreEqual(id, result.id);
        }

        [TestMethod]
        public void ManualConnectionStringTest()
        {
            //ConnectionStringManager.Default.SetProperties("Omc","SysInfoCenter","sa","4848285");
            ConnectionStringManager.Default.SetProperties("Omc", "SysInfoCenter");
            string cs = ConnectionStringManager.Default.EntityConnectionString;
            ParrotEntities ctx = new ParrotEntities(cs);
            int id = 322;
            UsrLogRec result = ctx.UsrLogRecs.FirstOrDefault(q => q.LogId == id);
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.LogId);
        }

    }
}
