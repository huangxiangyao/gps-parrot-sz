using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Db44
{
    /// <summary>
    /// DB44通信帐户。
    /// </summary>
    public class Db44ClientAccount
    {
        public Db44ClientAccount(ushort mdtManufacturerCode, uint omcId,  uint pin)
        {
            this.MdtManufacturerCode = mdtManufacturerCode;
            this.OmcId = omcId;
            this.Pin = pin;
        }
        
        /// <summary>
        /// 终端厂商代码。
        /// </summary>
        public ushort MdtManufacturerCode { get; private set; }
        /// <summary>
        /// 运营管理中心识别号。
        /// </summary>
        public uint OmcId { get; private set; }
        /// <summary>
        /// 密码。
        /// </summary>
        public uint Pin { get; private set; }
    }
}
