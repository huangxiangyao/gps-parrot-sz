using System;
using System.Text;
using System.Net.Sockets;
using Timer = System.Timers.Timer;
using System.Threading;
using System.Diagnostics;
using System.Timers;
using Parrot.Models;
using System.Net;

namespace Parrot
{
    public class SmppClient : SmppClientBase
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="smppAgent"></param>
        /// <param name="netParameter"></param>
        /// <param name="remoteEP"></param>
        public SmppClient(SmppAgent smppAgent, string netParameter, IPEndPoint remoteEP) : base(smppAgent, netParameter, remoteEP) { }

        protected override void SendKeepAlivePacket()
        {
            string s = "##,2,1000000,\r\n";
            byte[] bytes = Encoding.Default.GetBytes(s);
            try
            {
                this.Send(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, this.Title + "：检测连接失败。");
                FireLoggingEvent(Level.Advanced, ex);
            }
        }

        /// <summary>
        /// 解析PDU。
        /// 格式：##,messageType,sequenceNumber,protocolType,localPort,remoteEndpoint,pdu(base64),\r\n
        /// </summary>
        /// <param name="pdu"></param>
        protected override void Parse(string pdu)
        {
            string[] pduParts = pdu.Split(new char[] { ',' });

            if (pduParts[1] == "3")
            {
                return;
            }
            else if (pduParts[1] == "2")
            {
                this.SmppLastLinkTestDateTime = DateTime.Now;
                return;
            }
            else if (pduParts[1] == "1")
            {
                try
                {
                    long num3 = long.Parse(pduParts[3]);
                }
                catch (Exception ex)
                {
                    FireLoggingEvent(Level.Info, "处理来自GPS终端的消息时发生异常。错误：" + pduParts[3] + ":" + pduParts[4] + ":" + pduParts[5] + "。详情请查阅系统日志。");
                    FireLoggingEvent(Level.Advanced, ex);
                }
                return;
            }


            if (pduParts[1] != "0")
            {
                return;
            }

            byte protocolType = byte.Parse(pduParts[3]);
            string localInfo = pduParts[4];
            string remoteInfo = pduParts[5];
            byte[] body = Convert.FromBase64String(pduParts[6]);
            string mdtId = "";
            if (body.Length < 4)
            {
                FireLoggingEvent(Level.Debug, "收到无法识别的消息：" + pdu);
                return;
            }

            string tempNewRemoteInfoKey = string.Format("{0},{1},{2}", protocolType, localInfo, remoteInfo);
            if (protocolType > 1)
            {
                mdtId = localInfo;
                if (mdtId.StartsWith("+"))
                {
                    mdtId = mdtId.Substring(0);
                }
                if (mdtId.StartsWith("86"))
                {
                    mdtId = mdtId.Substring(2);
                }
            }
            else
            {
                byte mobileType;
                SmppAgent.GetMobileBaseInfo(ref body, out mdtId, out mobileType, protocolType);
            }
            MdtWrapper mdt = null;
            if (mdtId != "")
            {
                mdt = (MdtWrapper)SmppAgent.MobileInfo_Hash[mdtId];
                if ((mdt != null) & (protocolType < 2))
                {
                    SmppAgent.ReRegisterRemoteInfo(protocolType, ref tempNewRemoteInfoKey, ref mdt);
                }
            }
            else if (protocolType < 2)
            {
                mdt = (MdtWrapper)SmppAgent.RemoteInfo_Hash[tempNewRemoteInfoKey];
            }
            if (mdt != null)
            {
                SmppAgent.CountOfReceivingFromMdt++;
                SmppAgent.SpeedOfReceivingFromMdt++;
                FireMdtDataReceivedEvent(body, body.Length, mdt);
            }
        }
    }
}