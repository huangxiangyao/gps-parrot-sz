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
    public abstract class SmppClientBase
    {
        #region Fields
        /// <summary>
        /// 属主。
        /// </summary>
        protected SmppAgent SmppAgent;
        /// <summary>
        /// 网络链路。
        /// </summary>
        protected string Title;
        /// <summary>
        /// 服务器地址。
        /// </summary>
        private IPEndPoint RemoteEP;

        private TcpClient TcpClient = null;
        private Thread ThreadForReceiving = null;
        private bool IsThreadRunning = false;
        private Timer TimerForKeepingAlive = new Timer();
        protected DateTime SmppLastLinkTestDateTime = DateTime.MinValue;
        #endregion

        #region Events
        public event ClientCommandReturnEventHandler ClientCommandReturn;
        public event MdtDataReceivedEventHandler MdtDataReceived;
        public event LoggingEventHandler Logging;    
        
        protected void FireClientCommandReturnEvent(long cmdId,string r,string message)
        {
            if (ClientCommandReturn != null)
            {
                ClientCommandReturn(cmdId,r,message);
            }
        }
        protected void FireMdtDataReceivedEvent(byte[] body, int len, MdtWrapper mobileInfo)
        {
            if (MdtDataReceived != null)
            {
                MdtDataReceived(body, len, mobileInfo, Title);
            }
        }
        protected void FireLoggingEvent(Level level, object message)
        {
            if (Logging != null)
            {
                Logging(this, level, message);
            }
        }
        #endregion

        #region Properties
        public bool IsRunning { get { return IsThreadRunning; } }
        #endregion

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="smppAgent"></param>
        /// <param name="netParameter"></param>
        /// <param name="remoteEP"></param>
        public SmppClientBase(SmppAgent smppAgent, string netParameter, IPEndPoint remoteEP)
        {
            this.SmppAgent = smppAgent;
            this.Title = netParameter;
            this.RemoteEP = remoteEP;

            this.TimerForKeepingAlive.Elapsed += new ElapsedEventHandler(this.TimerForKeepingAlive_Elapsed);
            this.TimerForKeepingAlive.Interval = 20000.0;
            this.TimerForKeepingAlive.Enabled = false;
        }

        /// <summary>
        /// 发送消息给服务器。
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public void Send(byte[] buffer, int offset, int size)
        {
            this.TcpClient.GetStream().Write(buffer, offset, size);
        }
        /// <summary>
        /// 复位。先停止，再启动。
        /// </summary>
        public void Reset()
        {
            Stop();
            Start();
        }
        /// <summary>
        /// 启动。启动网络连接，启动接收线程，启动心跳定时器。
        /// </summary>
        private void Start()
        {
            Connect();
            if (!TcpClient.Connected) return;

            StartReceivingThread();

            if (!this.TimerForKeepingAlive.Enabled)
            {
                this.TimerForKeepingAlive.Start();
                this.SmppLastLinkTestDateTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 启动接收线程。
        /// </summary>
        private void StartReceivingThread()
        {
            FireLoggingEvent(Level.Info, this.Title + "：开始启动接收线程...");
            try
            {
                if (this.ThreadForReceiving == null)
                    this.ThreadForReceiving = new Thread(new ThreadStart(this.ReceivingProc));
                this.ThreadForReceiving.IsBackground = true;
                this.ThreadForReceiving.Name = "ThreadForReceiving";
                this.ThreadForReceiving.Start();
                FireLoggingEvent(Level.Info, this.Title + "：成功启动接收线程。");
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, this.Title + ":启动接收线程失败。详情请查阅系统日志。");
                FireLoggingEvent(Level.Advanced, ex);
            }
        }
        /// <summary>
        /// 连接到服务器。
        /// </summary>
        private void Connect()
        {
            try
            {
                FireLoggingEvent(Level.Info, this.Title + "：开始连接...");
                if (this.TcpClient == null)
                    this.TcpClient = new TcpClient();
                TcpClient.Connect(this.RemoteEP);
                if (TcpClient.Connected)
                {
                    FireLoggingEvent(Level.Info, this.Title + "：连接成功。");
                }
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, this.Title + ":连接失败。详情请查阅系统日志。");
                FireLoggingEvent(Level.Advanced, ex);
            }
        }
        /// <summary>
        /// 停止。停止心跳定时器，停止接收线程，停止网络连接。
        /// </summary>
        private void Stop()
        {
            try
            {
                if (this.TimerForKeepingAlive.Enabled)
                {
                    this.TimerForKeepingAlive.Stop();
                }
                if (this.ThreadForReceiving != null)
                {
                    try
                    {
                        this.ThreadForReceiving.Abort();
                    }
                    catch { }
                    finally
                    {
                        this.ThreadForReceiving = null;
                    }
                }
                if (this.TcpClient != null)
                {
                    try
                    {
                        this.TcpClient.Close();
                    }
                    catch { }
                    finally
                    {
                        this.TcpClient = null;
                    }
                }
            }
            catch
            {
            }
        }

        protected abstract void SendKeepAlivePacket();
        //{
        //    string s = "##,2,1000000,\r\n";
        //    byte[] bytes = Encoding.Default.GetBytes(s);
        //    try
        //    {
        //        this.Send(bytes, 0, bytes.Length);
        //    }
        //    catch (Exception ex)
        //    {
        //        FireLoggingEvent(Level.Info, this.netParameter + "：检测连接失败。");
        //        FireLoggingEvent(Level.Advanced, ex);
        //    }
        //}
        private void TimerForKeepingAlive_Elapsed(object source, ElapsedEventArgs e)
        {
            if (this.SmppLastLinkTestDateTime.AddMinutes(1) < DateTime.Now)
            {
                FireLoggingEvent(Level.Info, this.Title + "：检测连接超时。");
                this.IsThreadRunning = false;
            }
            else
            {
                SendKeepAlivePacket();
            }
        }

        private void ReceivingProc()
        {
            string tempStr = "";
            string pdu = "";
            string sTryParse = "";
            int nReceivedBytes = 0;
            byte[] buffer = new byte[1024];

            this.IsThreadRunning = true;
            while (this.IsThreadRunning)
            {
                try
                {
                    nReceivedBytes = this.TcpClient.GetStream().Read(buffer, 0, buffer.Length);
                    if (nReceivedBytes == 0)
                    {
                        this.IsThreadRunning = false;
                        FireLoggingEvent(Level.Info, this.Title + "：GPS终端关闭连接。");
                        this.Stop();
                        buffer = null;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    this.IsThreadRunning = false;
                    FireLoggingEvent(Level.Info, this.Title + "：失去联系。");
                    FireLoggingEvent(Level.Advanced, ex);
                    this.Stop();
                    buffer = null;
                    break;
                }
                try
                {
                    sTryParse = tempStr + Encoding.Default.GetString(buffer, 0, nReceivedBytes);
                    int endPos = 0;
                    for (endPos = sTryParse.IndexOf("\r\n"); endPos >= 0; endPos = sTryParse.IndexOf("\r\n"))
                    {
                        pdu = sTryParse.Substring(0, endPos + 2);
                        this.Parse(pdu);
                        pdu = "";
                        sTryParse = sTryParse.Substring(endPos + 2);
                    }
                    tempStr = sTryParse;
                    sTryParse = "";
                    continue;
                }
                catch (Exception exception)
                {
                    FireLoggingEvent(Level.Advanced,
                        string.Format("[SmppClient] Temp={1}\r\nTry={2}\r\nPDU={3}\r\nException:{0}\r\n",
                        exception.ToString(), tempStr, sTryParse, pdu));
                    tempStr = "";
                    sTryParse = "";
                    continue;
                }
            }
            FireLoggingEvent(Level.Info, this.Title + "：停止接收线程。");
            this.IsThreadRunning = false;
        }

        /// <summary>
        /// 解析PDU。
        /// 格式：##,messageType,sequenceNumber,protocolType,localPort,remoteEndpoint,pdu(base64),\r\n
        /// </summary>
        /// <param name="pdu"></param>
        protected abstract void Parse(string pdu);
        //{
        //    string[] pduParts = pdu.Split(new char[] { ',' });

        //    if (pduParts[1] == "3")
        //    {
        //        return;
        //    }
        //    else if (pduParts[1] == "2")
        //    {
        //        this.SmppLastLinkTestDateTime = DateTime.Now;
        //        return;
        //    }
        //    else if (pduParts[1] == "1")
        //    {
        //        try
        //        {
        //            long num3 = long.Parse(pduParts[3]);
        //        }
        //        catch (Exception ex)
        //        {
        //            FireLoggingEvent(Level.Info, "处理来自GPS终端的消息时发生异常。错误：" + pduParts[3] + ":" + pduParts[4] + ":" + pduParts[5] + "。详情请查阅系统日志。");
        //            FireLoggingEvent(Level.Advanced, ex);
        //        }
        //        return;
        //    }


        //    if (pduParts[1] != "0")
        //    {
        //        return;
        //    }

        //    byte protocolType = byte.Parse(pduParts[3]);
        //    string localInfo = pduParts[4];
        //    string remoteInfo = pduParts[5];
        //    byte[] pdu = Convert.FromBase64String(pduParts[6]);
        //    string mdtId = "";
        //    if (pdu.Length < 4)
        //    {
        //        FireLoggingEvent(Level.Debug, "收到无法识别的消息：" + pdu);
        //        return;
        //    }

        //    string tempNewRemoteInfoKey = string.Format("{0},{1},{2}", protocolType, localInfo, remoteInfo);
        //    if (protocolType > 1)
        //    {
        //        mdtId = localInfo;
        //        if (mdtId.StartsWith("+"))
        //        {
        //            mdtId = mdtId.Substring(0);
        //        }
        //        if (mdtId.StartsWith("86"))
        //        {
        //            mdtId = mdtId.Substring(2);
        //        }
        //    }
        //    else
        //    {
        //        byte mdtModel;
        //        SmppAgent.GetMobileBaseInfo(ref pdu, out mdtId, out mdtModel, protocolType);
        //    }
        //    MdtWrapper mdt = null;
        //    if (mdtId != "")
        //    {
        //        mdt = (MdtWrapper)SmppAgent.MobileInfo_Hash[mdtId];
        //        if ((mdt != null) & (protocolType < 2))
        //        {
        //            SmppAgent.ReRegisterRemoteInfo(protocolType, ref tempNewRemoteInfoKey, ref mdt);
        //        }
        //    }
        //    else if (protocolType < 2)
        //    {
        //        mdt = (MdtWrapper)SmppAgent.RemoteInfo_Hash[tempNewRemoteInfoKey];
        //    }
        //    if (mdt != null)
        //    {
        //        SmppAgent.CountOfReceivingFromMdt++;
        //        SmppAgent.SpeedOfReceivingFromMdt++;
        //        if (MdtDataReceived != null)
        //        {
        //            MdtDataReceived(pdu, pdu.Length, mdt, this.netParameter);
        //        }
        //    }
        //}
    }
}