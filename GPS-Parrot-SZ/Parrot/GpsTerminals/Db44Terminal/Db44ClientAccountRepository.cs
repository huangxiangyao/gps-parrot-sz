using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Db44
{
    /// <summary>
    /// DB44终端通信配置(<see cref="Db44ClientAccount"/>)仓库。
    /// </summary>
    public class Db44ClientAccountRepository
    {
        #region Singleton
        class Nested
        {
            internal static readonly Db44ClientAccountRepository singleton = new Db44ClientAccountRepository();
            static Nested() { }
        }
        private Db44ClientAccountRepository() { Accounts = new Dictionary<int, Db44ClientAccount>(); }
        public static Db44ClientAccountRepository Default { get { return Nested.singleton; } }
        #endregion

        private Dictionary<int, Db44ClientAccount> Accounts = null;

        private bool Exists(int mdtModel)
        {
            return Accounts.Any(q => q.Key == mdtModel);
        }
        /// <summary>
        /// 添加或更新一个终端通信配置(<see cref="Db44ClientAccount"/>)。
        /// </summary>
        /// <seealso cref="Db44EncryptionFactor"/>
        /// 
        /// <param name="mdtModel">GPS终端类型。</param>
        /// <param name="account">DB44通信帐户</param>
        public void AddOrUpdate(int mdtModel, Db44ClientAccount account)
        {
            if (Exists(mdtModel))
            {
                Accounts[mdtModel] = account;
            }
            else
            {
                Accounts.Add(mdtModel, account);
            }
        }

        /// <summary>
        /// 取出一个终端通信配置。
        /// </summary>
        /// <seealso cref="Db44ClientAccount"/>
        /// <param name="mdtModel">GPS终端类型。</param>
        /// <returns>null，如果在仓库中没有找到。</returns>
        public Db44ClientAccount GetAccount(int mdtModel)
        {
            if (Exists(mdtModel)) return Accounts[mdtModel];
            return null;
        }
    }
}
