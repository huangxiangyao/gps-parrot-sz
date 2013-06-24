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
    public class OldSmppClient : SmppClientBase
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="smppAgent"></param>
        /// <param name="netParameter"></param>
        /// <param name="remoteEP"></param>
        public OldSmppClient(SmppAgent smppAgent, string netParameter, IPEndPoint remoteEP) : base(smppAgent, netParameter, remoteEP) { }

        protected override void SendKeepAlivePacket()
        {
            string s = "SysLinkTest";
            s = Convert.ToBase64String(Encoding.Default.GetBytes(s));
            s = "##1,1000000,203:13178890033,00," + s + "\r\n";
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

        protected override void Parse(string pdu)
        {
            string key = "";
            string mobileID = "";
            byte mobileType = 0;
            MdtWrapper mobileInfo = null;
            byte[] sDataBodyByte = null;
            string str3 = "";
            string s = "";
            try
            {
                string[] strArray = pdu.Split(new char[] { ',' });
                s = pdu;
                if (s.StartsWith("##1"))
                {
                    str3 = strArray[2];
                    if (str3 == "203:13900000000")
                    {
                        string str5 = strArray[4].ToString();
                        byte[] buffer2 = Convert.FromBase64String(str5.Substring(0, str5.IndexOf("\r\n")));
                        str5 = Encoding.Default.GetString(buffer2);
                        byte[] buffer3 = new byte[str5.Length / 2];
                        for (int i = 0; i < (str5.Length / 2); i++)
                        {
                            string str6 = str5.Substring(i * 2, 2);
                            buffer3[i] = Convert.ToByte(str6, 0x10);
                        }
                        strArray = Encoding.Default.GetString(buffer3).Split(new char[] { ',' });
                        if (strArray[2] == "FE")
                        {
                            return;
                        }
                        str5 = strArray[3].ToString();
                        buffer2 = Convert.FromBase64String(str5.Substring(0, str5.IndexOf("\r\n")));
                        strArray = Encoding.Default.GetString(buffer2).Split(new char[] { ',' });
                        str3 = strArray[2];
                    }
                    int num3 = int.Parse(str3.Substring(0, str3.IndexOf(":")));
                    str3 = str3.Substring(str3.IndexOf(":") + 1);
                    s = strArray[4].ToString();
                    s = s.Substring(0, s.IndexOf("\r\n"));
                    byte[] bytes = Convert.FromBase64String(s);
                    s = Encoding.Default.GetString(bytes);
                    if (str3.StartsWith("86"))
                    {
                        mobileID = str3.Substring(2);
                        sDataBodyByte = Encoding.Default.GetBytes(s);
                        key = mobileID;
                    }
                    else
                    {
                        sDataBodyByte = new byte[s.Length / 2];
                        for (int j = 0; j < (s.Length / 2); j++)
                        {
                            string str7 = s.Substring(j * 2, 2);
                            sDataBodyByte[j] = Convert.ToByte(str7, 0x10);
                        }
                        key = str3;
                        SmppAgent.GetMobileBaseInfo(ref sDataBodyByte, out mobileID, out mobileType, 0xff);
                    }
                    if (mobileID == "")
                    {
                        if (SmppAgent.RemoteInfo_Hash.ContainsKey(key))
                        {
                            mobileID = (string)SmppAgent.RemoteInfo_Hash[key];
                        }
                    }
                    else
                    {
                        lock (SmppAgent.RemoteInfo_Hash)
                        {
                            if (!SmppAgent.RemoteInfo_Hash.ContainsKey(key))
                            {
                                SmppAgent.RemoteInfo_Hash.Add(key, mobileID);
                            }
                        }
                    }
                    mobileInfo = (MdtWrapper)SmppAgent.MobileInfo_Hash[mobileID];
                }
                else if (s.StartsWith("##0"))
                {
                    if (int.Parse(strArray[1]) == 0xf4240)
                    {
                        this.SmppLastLinkTestDateTime = DateTime.Now;
                    }
                    FireClientCommandReturnEvent(long.Parse(strArray[1]), strArray[2], "");
                    s = "";
                }
            }
            catch (Exception exception)
            {
                FireLoggingEvent(Level.Info, "解析来自GPS终端的消息时发生异常，详情请查阅系统日志。");
                FireLoggingEvent(Level.Advanced, "DOSmpp_BError----" + ("DOSmpp_B--" + key + "--" + mobileID + "--DataBody=" + s + "\r\n" + exception.ToString() + "\r\n"));
            }
            try
            {
                if (mobileInfo != null)
                {
                    SmppAgent.CountOfReceivingFromMdt++;
                    SmppAgent.SpeedOfReceivingFromMdt++;
                    lock (mobileInfo)
                    {
                        if (mobileInfo.ProtocolType < 2)
                        {
                            mobileInfo.IsOldSmpp = true;
                            mobileInfo.TcpRemoteInfo = key;
                        }
                        FireMdtDataReceivedEvent(sDataBodyByte, sDataBodyByte.Length, mobileInfo);
                    }
                }
            }
            catch (Exception exception2)
            {
                FireLoggingEvent(Level.Info, "解析来自GPS终端的消息时发生异常，详情请查阅系统日志。");
                string str9 = "DOSmpp_D---" + key + "--" + mobileID + "--DataBody=" + s + "\r\n" + exception2.ToString() + "\r\n";
                FireLoggingEvent(Level.Advanced, str9);

            }
        }
    }
}