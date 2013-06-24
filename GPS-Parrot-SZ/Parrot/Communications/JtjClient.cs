using System;
using System.Text;
using Parrot.Models;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Timer = System.Timers.Timer;
using System.Timers;
using Parrot.Models.Db44;
using Parrot.Protocols.Jtj;

namespace Parrot
{
    public class JtjClient
    {
        #region Fields
        /// <summary>
        /// 属主。
        /// </summary>
        protected SmppAgent SmppAgent;
        /// <summary>
        /// 网络连接配置。
        /// </summary>
        private JtjClientAccount JtjClientAccount = null;
        private string Title = "与交通局通讯";

        private TcpClient TcpClient = null;
        private Thread ThreadForReceiving = null;
        private bool IsThreadRunning = false;
        private Timer TimerForKeepingAlive = new Timer();
        #endregion
        #region Events
        public event LoggingEventHandler Logging;

        /// <summary>
        /// 上报给交通局的数据
        /// </summary>
        public event DeliveredToJtjEventHandler DeliveredToJtj;
        /// <summary>
        /// 收到来自交通局的D04指令
        /// </summary>
        public event D04ReceivedFromJtjEventHandler D04ReceivedFromJtj;

        protected void FireLoggingEvent(Level level, object message)
        {
            if (Logging != null)
            {
                Logging(this, level, message);
            }
        }
        protected void FireD04ReceivedFromJtjEvent(string plateNumber, byte plateColor)
        {
            if (D04ReceivedFromJtj != null)
            {
                D04ReceivedFromJtj(this, new D04ReceivedFromJtjEventArgs(plateNumber, plateColor, DateTime.Now));
            }
        }

        protected void FireDeliveredToJtjEvent(string plateNumber, string functionCode)
        {
            if (DeliveredToJtj != null)
            {
                DeliveredToJtj(this, new DeliveredToJtjEventArgs(plateNumber, functionCode, DateTime.Now));
            }
        }
        #endregion
        #region Properties
        public bool IsRunning { get { return IsThreadRunning; } }
        /// <summary>
        /// 是否输出调试信息。
        /// </summary>
        public bool IsDebugLoggingEnable { get; set; }
        /// <summary>
        /// 将要输出调试信息的GPS终端的ID（列表）。
        /// </summary>
        public string DebugMobileID { get; set; }
        #endregion

        /// <summary>
        /// 构造函数。
        /// </summary>
        public JtjClient(SmppAgent smppAgent, JtjClientAccount jtjClientAccount)
        {
            this.SmppAgent = smppAgent;
            this.JtjClientAccount = jtjClientAccount;

            this.DebugMobileID = "N";
            this.IsDebugLoggingEnable = true;

            this.TimerForKeepingAlive.Elapsed += new ElapsedEventHandler(TimerForKeepingAlive_Elapsed);
            this.TimerForKeepingAlive.Interval = 60000;
            this.TimerForKeepingAlive.Enabled = false;
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
                this.ThreadForReceiving.Name = "ThreadForReceivingFromJtj";
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
                {
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, this.JtjClientAccount.LocalPort);
                    try
                    {
                        this.TcpClient = new TcpClient(ep);
                    }
                    catch (Exception ex)
                    {
                        FireLoggingEvent(Level.Info, string.Format("连接交通局失败：本地端口{0}已被其他程序占用。", this.JtjClientAccount.LocalPort));
                        FireLoggingEvent(Level.Advanced, ex);
                    }
                }
                try
                {
                    TcpClient.Connect(this.JtjClientAccount.ServerIp, this.JtjClientAccount.ListeningPort);
                }
                catch (SocketException se)
                {
                    FireLoggingEvent(Level.Info, string.Format("连接交通局失败：请确认{0}:{1}是交通局服务地址，且未被其他程序占用。", this.JtjClientAccount.ServerIp, this.JtjClientAccount.ListeningPort));
                    FireLoggingEvent(Level.Advanced, se);
                }

                if (TcpClient.Connected)
                {
                    FireLoggingEvent(Level.Info, this.Title + "：连接成功。");
                }
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, this.Title + ":连接失败，网络连接发生异常。详情请查阅系统日志。");
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

        private void TimerForKeepingAlive_Elapsed(object source, ElapsedEventArgs e)
        {
            if (IsThreadRunning) SendT01();
        }



        private const int MaxPduSize = 2048 + 16;
        /// <summary>
        /// 接收来自交通局的消息，包括交通局对本地发起的消息的回复。
        /// </summary>
        private void ReceivingProc()
        {
            int nReceivedBytes = 0;
            byte[] buffer = new byte[MaxPduSize]; //接收缓冲区。协议中规定的协议体的最大长度

            int nTodoBytes = 0; //待处理数据的长度，即当前TempStr的位置。
            byte[] todo = null; // 用于将被分割的协议体连结起来。

            byte[] pdu = null; //一条完整的协议体。


            this.IsThreadRunning = true;
            while (this.IsThreadRunning)
            {
                try
                {
                    nReceivedBytes = this.TcpClient.GetStream().Read(buffer, 0, buffer.Length);
                    if (nReceivedBytes == 0)
                    {
                        this.IsThreadRunning = false;
                        FireLoggingEvent(Level.Info, this.Title + "：交通局关闭连接。");
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
                    buffer = null;
                    Thread.Sleep(60 * 1000);
                    this.Reset();
                    break;
                }

                try
                {
                    if (nTodoBytes > MaxPduSize)
                    {
                        nTodoBytes = 0; //丢弃缓冲区中的数据
                        todo = null;
                    }
                    Array.Resize(ref todo, nTodoBytes + nReceivedBytes);
                    //FireLoggingEvent(Level.Debug, string.Format("1:{0},{1}", nTodoBytes, nReceivedBytes));
                    Array.Copy(buffer, 0, todo, nTodoBytes, nReceivedBytes);
                    nTodoBytes += nReceivedBytes;

                    int endPos = 0;
                    for (endPos = FindEndOfPdu(todo); endPos >= 0; endPos = FindEndOfPdu(todo))
                    {
                        Array.Resize(ref pdu, endPos + 1);
                        //FireLoggingEvent(Level.Debug, string.Format("2:{0},{1}", endPos+1, endPos+1));
                        Array.Copy(todo, pdu, endPos + 1);
                        nTodoBytes -= (endPos + 1);

                        Parse(pdu);

                        if (nTodoBytes == 0) break;
                        //FireLoggingEvent(Level.Debug, string.Format("3:{0},{1}", endPos + 1, nTodoBytes));

                        Array.Copy(todo, endPos + 1, todo, 0, nTodoBytes);
                        Array.Resize(ref todo, nTodoBytes);
                    }
                }
                catch (Exception ex)
                {
                    FireLoggingEvent(Level.Info, "处理来自交通局的消息时发生异常，详情请查阅系统日志。");
                    FireLoggingEvent(Level.Advanced, ex);
                    Thread.Sleep(60 * 1000);
                }
            }
        }
        private int FindEndOfPdu(byte[] pdu)
        {
            return Array.IndexOf(pdu, (byte)'#');
        }

        #region 解析来自交通局的消息
        /// <summary>
        /// 解析PDU。
        /// 格式：##,messageType,sequenceNumber,protocolType,localPort,remoteEndpoint,pdu(base64),\r\n
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="pdu"></param>
        protected void Parse(byte[] pdu)
        {
            string pduHeader = ASCIIEncoding.ASCII.GetString(pdu, 0, 4);
            if (!pduHeader.StartsWith("~")) return;

            SmppAgent.CountOfReceivingFromJtj++;

            try
            {
                switch (pduHeader)
                {
                    case "~L00":
                        {
                            string passwordSalt;
                            if (DownloadDataParser.L00(pdu, out passwordSalt))
                            {
                                SendL01(this.JtjClientAccount.Username, this.JtjClientAccount.Password, passwordSalt);
                            }
                            return;
                        }
                    case "~L02":
                        {
                            int retCode;
                            if (DownloadDataParser.L02(pdu, out retCode))
                            {
                                switch (retCode)
                                {
                                    case 0: FireLoggingEvent(Level.Info, "交通局登录结果-成功。\r\n"); break;
                                    case 1: FireLoggingEvent(Level.Info, "交通局登录结果-无效数据包！\r\n"); break;
                                    case 2: FireLoggingEvent(Level.Info, "交通局登录结果-无效数据包类型！\r\n"); break;
                                    case 3: FireLoggingEvent(Level.Info, "交通局登录结果-无效用户名！\r\n"); break;
                                    case 4: FireLoggingEvent(Level.Info, "交通局登录结果-密码错误！\r\n"); break;
                                    case 5: FireLoggingEvent(Level.Info, "交通局登录结果-申请拒绝，随机序列错！\r\n"); break;
                                    case 6: FireLoggingEvent(Level.Info, "交通局登录结果-登录拒绝，IP错，运营商错！\r\n"); break;
                                }
                            }
                            return;
                        }
                    case "~T02":
                        {
                            FireLoggingEvent(Level.Info, "交通局链路检测：OK");
                            return;
                        }
                    case "~D01":
                        {
                            JtjD01 result = null;
                            if (DownloadDataParser.D01(pdu, out result))
                            {
                                FireLoggingEvent(Level.Debug, "~D01---" + result.ToString());
                            }
                            break;
                        }
                    case "~D02":
                        {
                            JtjD02 result = null;
                            if (DownloadDataParser.D02(pdu, out result))
                            {
                                FireLoggingEvent(Level.Debug, "~D02---" + result.ToString());
                            }
                            break;
                        }
                    case "~D03":
                        {
                            JtjD03 result = null;
                            if (DownloadDataParser.D03(pdu, out result))
                            {
                                FireLoggingEvent(Level.Debug, "~D03---" + result.ToString());
                            }
                            break;
                        }
                    case "~D04":
                        ParseAsD04(pdu);
                        break;
                    case "~D05":
                        ParseAsD05(pdu);
                        break;
                    case "~D06":
                        ParseAsD06(pdu);
                        break;
                }
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, this.Title + "：接收到格式错误的消息：" + Util.BytesToHex(pdu, true));
                FireLoggingEvent(Level.Advanced, ex);
            }
        }

        private void ParseAsD04(byte[] pdu)
        {
            string plateNumber;
            byte plateColor;

            if (!DownloadDataParser.D04(pdu, out plateNumber, out plateColor))
            {
                FireLoggingEvent(Level.Debug, this.Title + "：接收到格式错误的消息：" + Util.BytesToHex(pdu, true));
                return;
            }
            else
            {
                FireD04ReceivedFromJtjEvent(plateNumber, plateColor);

                try
                {
                    string s = string.Format("~D04(车辆静态信息请求)---车牌号：{0}，车牌颜色：{1}",
                        plateNumber, VehiclePlateColorHelper.Default.Items[(byte)(plateColor)]);
                    FireLoggingEvent(Level.Debug, s);
                }
                catch
                {
                    string s = string.Format("~D04(车辆静态信息请求)---车牌号：{0}，车牌颜色：（{1}）",
                        plateNumber, plateColor);
                    FireLoggingEvent(Level.Debug, s);
                }
            }

            try
            {
                SendU03(plateNumber);

                FireDeliveredToJtjEvent(plateNumber, "U03");

                FireLoggingEvent(Level.Info, string.Format("已成功上报车辆静态信息（车牌号：{0}）。", plateNumber));
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, string.Format("上报车辆静态信息时发生异常（车牌号：{0}）。详情请查阅系统日志。", plateNumber));
                FireLoggingEvent(Level.Advanced, ex);
            }
        }


        private void ParseAsD05(byte[] pdu)
        {
            string plateNumber;
            byte plateColor;
            byte trafficPacketIndex;

            if (!DownloadDataParser.D05(pdu, out plateNumber, out plateColor, out trafficPacketIndex))
            {
                FireLoggingEvent(Level.Debug, this.Title + "：接收到格式错误的消息：" + Util.BytesToHex(pdu, true));
                return;
            }
            else
            {
                string s = string.Format("车牌号：{0}，车牌颜色：{1}，疑点数据包编号：{2}",
                    plateNumber, plateColor, trafficPacketIndex);
                FireLoggingEvent(Level.Debug, "~D05(事故疑点信息请求)---" + s);
            }

            try
            {
                RelayD05(plateNumber, trafficPacketIndex);
                FireLoggingEvent(Level.Info,
                    string.Format("已成功转发事故疑点请求给GPS终端。车牌号：{0}，车牌颜色：{1}，疑点数据包编号：{2}",
                    plateNumber, plateColor, trafficPacketIndex));
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info,
                    string.Format("转发交通局请求上传事故疑点数据包时发生异常（车牌号：{0}，车牌颜色：{1}，疑点数据包编号：{2}）。详情请查阅系统日志。",
                    plateNumber, plateColor, trafficPacketIndex));
                FireLoggingEvent(Level.Advanced, ex);
            }
        }

        private void RelayD05(string plateNumber, byte trafficPacketIndex)
        {
            CarList info = ParrotModelWrapper.GetMdtByPlateNumber(plateNumber);
            if (info == null)
            {
                FireLoggingEvent(Level.Info, string.Format("交通局请求上传事故疑点数据包（指定数据包编号），但是数据库中未找到指定车辆。（车牌号：{0}）", plateNumber));
                return;
            }

            //int mdtModel = info.Mobile_Type;
            //Db44Wrapper.D10(MdtIdHelper.GetMdtId(mdtModel, info.Mobile_ID), mdtModel, trafficPacketIndex, DateTime.Now);
            SmppAgent.SendCmdToMobile(info.Mobile_ID, new string[] { "96", 
                    trafficPacketIndex.ToString(), 
                    DateTime.Now.ToString("yy:MM:dd:HH:mm:ss") });

        }
        private void ParseAsD06(byte[] pdu)
        {
            string plateNumber;
            byte plateColor;
            byte pictureRequestType;
            byte cameraNumber;

            if (!DownloadDataParser.D06(pdu, out plateNumber, out plateColor, out pictureRequestType, out cameraNumber))
            {
                FireLoggingEvent(Level.Debug, this.Title + "：接收到格式错误的消息：" + Util.BytesToHex(pdu, true));
                return;
            }
            else
            {
                string s = string.Format("车牌号：{0}，车牌颜色：{1}，图片请求类型：{2}，摄像头序号：{3}",
                    plateNumber, plateColor, pictureRequestType, cameraNumber);
                FireLoggingEvent(Level.Debug, "~D06(图片请求)---" + s);
            }

            try
            {
                RelayD06(plateNumber, pictureRequestType, cameraNumber);
                FireLoggingEvent(Level.Info,
                    string.Format("已成功转发图片请求给GPS终端。车牌号：{0}，车牌颜色：{1}，图片请求类型：{2}，摄像头序号：{3}",
                    plateNumber, plateColor, pictureRequestType, cameraNumber));
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info,
                    string.Format("转发交通局请求图片数据包时发生异常（车牌号：{0}，车牌颜色：{1}，图片请求类型：{2}，摄像头序号：{3}。详情请查阅系统日志。",
                    plateNumber, plateColor, pictureRequestType, cameraNumber));
                FireLoggingEvent(Level.Advanced, ex);
            }
        }
        private void RelayD06(string plateNumber, byte pictureRequestType, byte cameraNumber)
        {
            CarList info = ParrotModelWrapper.GetMdtByPlateNumber(plateNumber);
            if (info == null)
            {
                FireLoggingEvent(Level.Info, string.Format("交通局请求上传图片，但是数据库中未找到指定车辆。（车牌号：{0}）", plateNumber));
                return;
            }

            string msg = String.Empty;
            string cmd = "";
            switch (pictureRequestType)
            {
                case 2://自动上传
                    cmd = "57";
                    msg = "指令禁止，终端不支持";
                    break;

                case 3://取消自动上传
                    cmd = "57";
                    msg = "指令禁止，终端不支持";
                    break;

                case 1://立即拍照一张并上传
                    {
                        cmd = "58";
                        if (cameraNumber == 0)
                        {
                            cameraNumber = 1;
                        }
                        SmppAgent.SendCmdToMobile(info.Mobile_ID, new string[] { cmd, cameraNumber.ToString(), "0" });
                        msg = "指令许可";
                        break;
                    }
            }
            //if ((info.Mobile_Type != 252) && (info.Mobile_Type == 251))
            //{
            //    info.Mobile_Type = 206;

            //    info.Mobile_Type = 251;
            //}
        }
        #endregion

        #region 发送消息给交通局
        /// <summary>
        /// 发送消息给服务器。
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        private void Send(byte[] buffer)
        {
            this.TcpClient.GetStream().Write(buffer, 0, buffer.Length);
        }

        private void SendT01()
        {
            try
            {
                this.Send(UploadDataWrapper.T01(this.JtjClientAccount.ClientId));
                FireLoggingEvent(Level.Info, this.Title + "：发送链路检测。");
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, this.Title + "：发送链路检测时发生异常。详情请查阅系统日志。");
                FireLoggingEvent(Level.Advanced, ex);
            }
        }
        private void SendL01(string username, string password, string salt)
        {
            this.Send(UploadDataWrapper.L01(JtjClientAccount.ClientId, username, password, salt));
        }
        /// <summary>
        /// 将卫星定位数据包上报给交通局。
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public void ForwardU01(string plateNumber, byte plateColor, Db44GpsData gpsData)
        {
            if (string.IsNullOrEmpty(plateNumber))
                throw new ArgumentException("车牌号不能为空。", "plateNumber");
            if (gpsData == null)
                throw new ArgumentNullException("gpsData", "卫星定位数据包不能为空。");

            plateColor = ParrotModelWrapper.GetPlateColorValueByPlateNumber(plateNumber);
            byte[] pdu = UploadDataWrapper.U01(JtjClientAccount.ClientId, plateNumber, plateColor, gpsData);
            this.Send(pdu);
            FireDeliveredToJtjEvent(plateNumber, "U01");
            //FireLoggingEvent(Level.Info, "将卫星定位数据包上报给交通局。");
            //FireLoggingEvent(Level.Debug, string.Format("将卫星定位数据包上报给交通局。牌：{0}。色：{1}。数据：{2}",plateNumber,plateColor,Util.BytesToHex(pdu,true)));
        }

        /// <summary>
        /// 将图片数据包上报给交通局。
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public void SendU02(string plateNumber, byte plateColor, DateTime captureTime, byte cameraNumber, string fileExtensionName, byte packetTotal, byte packetIndex, byte[] imageData)
        {
            if (string.IsNullOrEmpty(plateNumber))
                throw new ArgumentException("车牌号不能为空。", "plateNumber");
            if (string.IsNullOrEmpty(fileExtensionName))
                throw new ArgumentException("图片格式不能为空。", "ImageFormatName");
            if (imageData == null)
                throw new ArgumentNullException("imageData", "图片数据包不能为空。");
            
            plateColor = ParrotModelWrapper.GetPlateColorValueByPlateNumber(plateNumber);
            byte[] pdu = UploadDataWrapper.U02(JtjClientAccount.ClientId, plateNumber, plateColor, captureTime, cameraNumber, fileExtensionName, packetTotal, packetIndex, imageData);
            this.Send(pdu);

            FireDeliveredToJtjEvent(plateNumber, "U02");
            FireLoggingEvent(Level.Info, "将图片数据包上报给交通局。");
        }

        /// <summary>
        /// 将指定车辆的静态信息上报给交通局。
        /// </summary>
        /// <param name="plateNumber">车牌号</param>
        public void SendU03(string plateNumber)
        {
            CarList info = ParrotModelWrapper.GetMdtByPlateNumber(plateNumber);
            if (info == null)
            {
                FireLoggingEvent(Level.Info, string.Format("交通局请求上传车辆静态信息，但是数据库中未找到指定车辆。（车牌号：{0}）", plateNumber));
                return;
            }

            byte[] pdu = UploadDataWrapper.U03(JtjClientAccount.ClientId, info);
            this.Send(pdu);
            try
            {
                FireDeliveredToJtjEvent(plateNumber, "U03");
                FireLoggingEvent(Level.Info, string.Format("已成功上报车辆静态信息（车牌号：{0}）。数据：{1}", plateNumber, Util.BytesToHex(pdu, true)));
            }
            catch { }
        }

        /// <summary>
        /// 将指定车辆的事故疑点信息上报给交通局。
        /// </summary>
        /// <param name="plateNumber">车牌号</param>
        public void SendU04(string plateNumber, byte plateColor, byte packetIndex, byte[] occurTimeBytes, byte[] trafficData)
        {
            CarList info = ParrotModelWrapper.GetMdtByPlateNumber(plateNumber);
            if (info == null)
            {
                FireLoggingEvent(Level.Info, string.Format("交通局请求上传事故疑点信息，但是数据库中未找到指定车辆。（车牌号：{0}）", plateNumber));
                return;
            }

            byte[] pdu = UploadDataWrapper.U04(JtjClientAccount.ClientId, plateNumber, plateColor, packetIndex, occurTimeBytes, trafficData);
            this.Send(pdu);

            FireDeliveredToJtjEvent(plateNumber, "U04");
            FireLoggingEvent(Level.Info, "将指定车辆的事故疑点信息上报给交通局。");
        }
        #endregion
    }
}