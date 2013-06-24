using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot;

namespace Parrot.Models.Db44
{
    public class Db44Packet
    {
        /// <summary>
        /// DB44通信帐户。
        /// </summary>
        public Db44ClientAccount ClientAccount { get; private set; }
        public uint OmcId { get { return ClientAccount.OmcId; } }
        public uint Pin { get { return ClientAccount.Pin; } }
        public ushort MdtManufacturerCode { get { return ClientAccount.MdtManufacturerCode; } }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="account">DB44通信帐户(<see cref="Db44ClientAccount"/>)。</param>
        /// <param name="sequenceNumber">流水号（指令循环码）。</param>
        /// <param name="mdtId">终端识别号。</param>
        /// <param name="functionCode">协议号（功能码）。</param>
        /// <param name="bodyWithFunctionCode">协议内容。不含协议号（功能码）！</param>
        public Db44Packet(Db44ClientAccount account,
            byte sequenceNumber, uint mdtId, string functionCode,
            byte[] body)
        {
            this.ClientAccount = account;

            this.SequenceNumber = sequenceNumber;
            this.MdtId = mdtId;
            this.FunctionCode = functionCode;
            this.Body = body;
        }

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="mdtManufacturerCode">终端厂商代码。</param>
        /// <param name="omcId">运营管理中心识别号。</param>
        /// <param name="pin">密码。</param>
        /// <param name="sequenceNumber">流水号（指令循环码）。</param>
        /// <param name="mdtId">终端识别号。</param>
        /// <param name="functionCode">协议号（功能码）。</param>
        /// <param name="bodyWithFunctionCode">协议内容。不含协议号（功能码）！</param>
        public Db44Packet(
            byte sequenceNumber, 
            ushort mdtManufacturerCode, uint mdtId, uint omcId, uint pin, string functionCode,
            byte[] body)
            : this(new Db44ClientAccount(mdtManufacturerCode, omcId, pin), sequenceNumber, mdtId, functionCode, body) { }

        /// <summary>
        /// 帧头。
        /// </summary>
        public const byte Header = 0x7e;
        /// <summary>
        /// 帧尾。
        /// </summary>
        public const byte Tail = 0x7f;
        /// <summary>
        /// 标志码。
        /// </summary>
        public const byte Mark = 0x47;
        /// <summary>
        /// 流水号（指令循环码）。
        /// </summary>
        public byte SequenceNumber { get; private set; }
        /// <summary>
        /// 终端识别号。
        /// </summary>
        public uint MdtId { get; private set; }
        /// <summary>
        /// 功能码（协议号）。
        /// </summary>
        public string FunctionCode { get; private set; }
        /// <summary>
        /// 协议内容。
        /// </summary>
        /// <remarks>不含协议号（功能码）！</remarks>
        public byte[] Body { get; private set; }


        public string ToReadableString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("流水号：{0}" + Environment.NewLine, this.SequenceNumber);
            sb.AppendFormat("功能码：{0}" + Environment.NewLine, this.FunctionCode);
            sb.AppendFormat("终端ID：{0}" + Environment.NewLine, this.MdtId);
            sb.AppendFormat("运营管理中心ID：{0}" + Environment.NewLine, this.OmcId);
            sb.AppendFormat("终端厂商代码：{0}" + Environment.NewLine, this.MdtManufacturerCode);
            sb.AppendFormat("密码：{0}" + Environment.NewLine, this.Pin);
            sb.AppendFormat("协议内容：{0}", Util.BytesToHex(this.Body, true));
            return sb.ToString();
        }
    }
}
