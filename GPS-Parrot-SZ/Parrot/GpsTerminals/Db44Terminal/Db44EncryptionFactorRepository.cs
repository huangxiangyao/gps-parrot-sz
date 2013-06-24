using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Db44
{
    /// <summary>
    /// DB44加密算法因子(<see cref="Db44EncryptionFactor"/>)库。
    /// </summary>
    public class Db44EncryptionFactorRepository
    {
        #region Singleton
        class Nested
        {
            internal static readonly Db44EncryptionFactorRepository singleton = new Db44EncryptionFactorRepository();
            static Nested() { }
        }
        private Db44EncryptionFactorRepository() { Factors = new Dictionary<int, Db44EncryptionFactor>(); }
        public static Db44EncryptionFactorRepository Default { get { return Nested.singleton; } }
        #endregion

        private Dictionary<int, Db44EncryptionFactor> Factors = null;

        private bool Exists(int mdtModel)
        {
            return Factors.Any(q=>q.Key==mdtModel);
        }
        /// <summary>
        /// 添加或更新一个加密算法因子(<see cref="Db44EncryptionFactor"/>)。
        /// </summary>
        /// <param name="mdtModel">GPS终端类型。</param>
        /// <param name="factor">DB44加密算法因子。</param>
        public void AddOrUpdate(int mdtModel, Db44EncryptionFactor factor)
        {
            if (Exists(mdtModel))
            {
                Factors[mdtModel] = factor;
            }
            else
            {
                Factors.Add(mdtModel, factor);
            }
        }

        /// <summary>
        /// 取出一个加密算法因子(<see cref="Db44EncryptionFactor"/>)。
        /// </summary>
        /// <param name="mdtModel">GPS终端类型。</param>
        /// <returns>null，如果在仓库中没有找到。</returns>
        public Db44EncryptionFactor GetFactor(int mdtModel)
        {
            if (Exists(mdtModel)) return Factors[mdtModel];
            return null;
        }
    }
}
