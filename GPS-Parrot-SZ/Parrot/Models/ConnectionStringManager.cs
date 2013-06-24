using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.EntityClient;

namespace Parrot.Models
{
    public class ConnectionStringManager
    {
        private static ConnectionStringManager defaultInstance = new ConnectionStringManager();

        public static ConnectionStringManager Default { get { return defaultInstance; } }

        private bool useWindowsAuthenticationMode;
        private string server, db, username, password;
        public ConnectionStringManager()
        {
            useWindowsAuthenticationMode = false;
            server = "(local)";
            db = "SysInfoCenter";
            username = "sa";
            password = "";
        }
        private SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
        {
            var sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = server;
            sqlBuilder.InitialCatalog = db;
            if (useWindowsAuthenticationMode)
            {
                sqlBuilder.IntegratedSecurity = true;
            }
            else
            {
                sqlBuilder.UserID = username;
                sqlBuilder.Password = password;
            }
            sqlBuilder.MultipleActiveResultSets = true;
            return sqlBuilder;
        }

        private EntityConnectionStringBuilder GetEntityConnectionStringBuilder()
        {
            var eBuilder = new EntityConnectionStringBuilder();

            eBuilder.Provider = "System.Data.SqlClient";
            eBuilder.Metadata = string.Format("res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", "Models.ParrotModel");
            eBuilder.ProviderConnectionString = GetSqlConnectionStringBuilder().ToString();
            return eBuilder;
        }

        public string EntityConnectionString { get { return GetEntityConnectionStringBuilder().ToString(); } }
        public string SqlConnecitonString { get { return GetSqlConnectionStringBuilder().ToString(); } }

        /// <summary>
        /// 采用Windows验证方式。
        /// </summary>
        /// <param name="server"></param>
        /// <param name="db"></param>
        public void SetProperties(string server, string db)
        {
            this.useWindowsAuthenticationMode = true;
            this.server = server;
            this.db = db;
        }
        /// <summary>
        /// 采用SQL验证方式
        /// </summary>
        /// <param name="server"></param>
        /// <param name="db"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void SetProperties(string server, string db, string username, string password)
        {
            this.useWindowsAuthenticationMode = false;
            this.server = server;
            this.db = db;
            this.username = username;
            this.password = password;
        }
    }
}
