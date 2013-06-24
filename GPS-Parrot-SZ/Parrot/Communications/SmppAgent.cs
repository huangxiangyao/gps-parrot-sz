using System;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Timer = System.Timers.Timer;
using System.IO;
using System.Security.Cryptography;
using Parrot.Models.TianheGprs;
using Parrot.Models.Yelang;
using System.Runtime.InteropServices;
using System.Timers;
using Parrot.Models.Longhan;
using Parrot.Models;
using Parrot.Models.Db44;
using System.Collections.Generic;

namespace Parrot
{
    public class SmppAgent
    {
        /// <summary>
        /// 流水号生成器。
        /// </summary>
        private ITraceGenerator TraceGenerator;

        private Db44In Db44In;
        private Db44Out Db44Out;
        private LonghanIn LonghanIn;
        private static LonghanOut LonghanOut;
        private YelangIn YelangIn;
        private static YelangOut YelangOut;
        private TianheGprsIn TianheGprsIn;
        private static TianheGprsOut TianheGprsOut;

        private Timer TimerForUpdateLastGpsLocation;
        private Timer TimerForKeepingAlive;

        /// <summary>
        /// 是否输出调试信息。
        /// </summary>
        public bool IsDebugLoggingEnable { get; set; }
        public event LoggingEventHandler Logging;

        /// <summary>
        /// 上报给交通局的数据
        /// </summary>
        public event DeliveredToJtjEventHandler DeliveredToJtj;
        /// <summary>
        /// 收到来自交通局的D04指令
        /// </summary>
        public event D04ReceivedFromJtjEventHandler D04ReceivedFromJtj;

        /// <summary>
        /// 更新地理位置
        /// </summary>
        public event GpsLocationUpdatedEventHandler GpsLocationUpdated;
        /// <summary>
        /// 收到图像
        /// </summary>
        public event ImageReceivedEventHandler ImageReceived;
        public event CommuStatusUpdatedEventHandler CommuStatusUpdated;

        private int TimerCountOfReduceProcessWorkingSetSize = 0;
        /// <summary>
        /// 将要输出调试信息的GPS终端的ID（列表）。
        /// </summary>
        public string DebugMobileID { get; set; }
        public static ArrayList MdtConnectedList;
        /// <summary>
        /// 接收到来自交通局的消息条数。
        /// </summary>
        public int CountOfReceivingFromJtj { get; set; }
        /// <summary>
        /// 发送给交通局的消息条数。
        /// </summary>
        public int CountOfSendingToJtj { get; set; }
        /// <summary>
        /// 发送给交通局的速度。
        /// </summary>
        public int SpeedOfSendingToJtj { get; set; }
        /// <summary>
        /// 发送给交通局的失败次数。
        /// </summary>
        public int ErrorCountOfSendingToJtj { get; set; }
        /// <summary>
        /// 接收来自GPS终端的条数。
        /// </summary>
        public int CountOfReceivingFromMdt { get; set; }
        /// <summary>
        /// 接收来自GPS终端的速度。
        /// </summary>
        public int SpeedOfReceivingFromMdt { get; set; }

        public static Hashtable RemoteInfo_Hash;
        public static Hashtable MobileInfo_Hash;
        public static SortedList OldSmppClientHash;
        public static SortedList SmppClientHash;
        private JtjClient JtjClient;
        public static Hashtable LastGpsInfo_Hash;

        public string PCmdHead { get; set; }
        public string PCmdTotal { get; set; }
        public string PSmppHead { get; set; }

        private string SysPath;

        static SmppAgent()
        {
            RemoteInfo_Hash = Hashtable.Synchronized(new Hashtable());
            MobileInfo_Hash = Hashtable.Synchronized(new Hashtable());
            LastGpsInfo_Hash = Hashtable.Synchronized(new Hashtable());
            OldSmppClientHash = SortedList.Synchronized(new SortedList());
            SmppClientHash = SortedList.Synchronized(new SortedList());
        }
        public SmppAgent(JtjClientAccount jtjClientAccount)
        {
            this.DebugMobileID = "";
            this.IsDebugLoggingEnable = true;
            this.TraceGenerator = new SimpleTraceGenerator((int)((DateTime.Now.Ticks % 999999) + 1));

            this.JtjClient = new JtjClient(this, jtjClientAccount);
            this.JtjClient.Logging += new LoggingEventHandler(JtjClient_Logging);
            this.JtjClient.D04ReceivedFromJtj += new D04ReceivedFromJtjEventHandler(JtjClient_D04ReceivedFromJtj);
            this.JtjClient.DeliveredToJtj += new DeliveredToJtjEventHandler(JtjClient_DeliveredToJtj);

            SysPath = Environment.CurrentDirectory;
            if (!Directory.Exists(SysPath + @"\Area_LimitingSpeedInfo"))
            {
                Directory.CreateDirectory(SysPath + @"\Area_LimitingSpeedInfo");
            }
            if (!Directory.Exists(SysPath + @"\Stop_OvertimeInfo"))
            {
                Directory.CreateDirectory(SysPath + @"\Stop_OvertimeInfo");
            }
            if (!Directory.Exists(SysPath + @"\Area_Limiting_FJZZ_Info"))
            {
                Directory.CreateDirectory(SysPath + @"\Area_Limiting_FJZZ_Info");
            }
            if (!Directory.Exists(SysPath + @"\temp"))
            {
                Directory.CreateDirectory(SysPath + @"\temp");
            }

            FireLoggingEvent(Level.Info, "开始初始化通讯服务。");

            this.IsDebugLoggingEnable = true;
            PCmdHead = "";
            PCmdTotal = "\r\n";
            PSmppHead = "##";
            MdtConnectedList = new ArrayList();

            TianheGprsOut = new TianheGprsOut();
            this.TianheGprsIn = new TianheGprsIn();
            this.TianheGprsIn.PlainGpsDataReceived += new PlainGpsDataReceivedEventHandler(this.OnPlainGpsDataReceived);
            this.TianheGprsIn.PlainMessageReceived += new PlainMessageReceivedEventHandler(this.OnPlainMessageReceived);

            LonghanOut = new LonghanOut();
            this.LonghanIn = new LonghanIn();
            this.LonghanIn.PlainGpsDataReceived += new PlainGpsDataReceivedEventHandler(this.OnPlainGpsDataReceived);
            this.LonghanIn.PlainMessageReceived += new PlainMessageReceivedEventHandler(this.OnPlainMessageReceived);
            this.LonghanIn.GpsDataReturn += new GpsDataReturnEventHandler(this.AutoSendCmdToMobile);
            this.LonghanIn.ImageReceived += new ImageReceivedEventHandler(this.OnImageReceived);

            Db44Out = new Db44Out();
            this.Db44In = new Db44In();
            this.Db44In.GpsDataReceived += new EventHandler<GpsDataReceivedEventArgs>(Db44In_GpsDataReceived);
            this.Db44In.DriverSignedInOrOut += new EventHandler<DriverSignedInOrOutEventArgs>(Db44In_DriverSignedInOrOut);
            this.Db44In.PossibleAccidentDataReporting += new EventHandler<PossibleAccidentDataReportingEventArgs>(Db44In_PossibleAccidentDataReporting);
            this.Db44In.CameraCapturing += new EventHandler<CameraCapturingEventArgs>(Db44In_CameraCapturing);

            this.Db44In.PlainMessageReceived += new PlainMessageReceivedEventHandler(this.OnPlainMessageReceived);
            this.Db44In.GpsDataReturn += new GpsDataReturnEventHandler(this.AutoSendCmdToMobile);

            YelangOut = new YelangOut();
            this.YelangIn = new YelangIn();
            this.YelangIn.PlainGpsDataReceived += new PlainGpsDataReceivedEventHandler(this.OnPlainGpsDataReceived);
            this.YelangIn.PlainMessageReceived += new PlainMessageReceivedEventHandler(this.OnPlainMessageReceived);
            this.YelangIn.RecivGpsDataReturn += new GpsDataReturnEventHandler(this.AutoSendCmdToMobile);

            this.TimerForUpdateLastGpsLocation = new Timer();
            this.TimerForUpdateLastGpsLocation.Elapsed += new ElapsedEventHandler(TimerForRefillingLastGpsLocation_Elapsed);
            this.TimerForUpdateLastGpsLocation.Interval = 300000.0;
            this.TimerForUpdateLastGpsLocation.Start();

            this.TimerForKeepingAlive = new Timer();
            this.TimerForKeepingAlive.Elapsed += new ElapsedEventHandler(TimerForKeepingAlive_Elapsed);
            this.TimerForKeepingAlive.Interval = 20000.0;
            this.TimerForKeepingAlive.Start();

            FireLoggingEvent(Level.Info, "通讯服务初始化完成。");
        }

        void JtjClient_DeliveredToJtj(object sender, DeliveredToJtjEventArgs e)
        {
            FireDeliveredToJtjEvent(e);
        }


        void JtjClient_D04ReceivedFromJtj(object sender, D04ReceivedFromJtjEventArgs e)
        {
            if (D04ReceivedFromJtj != null)
                D04ReceivedFromJtj(this, e);
        }

        private void FireDeliveredToJtjEvent(DeliveredToJtjEventArgs e)
        {
            if (DeliveredToJtj != null)
                DeliveredToJtj(this, e);
        }

        void JtjClient_Logging(object sender, Level level, object message)
        {
            FireLoggingEvent(level, message);
        }

        void Db44In_CameraCapturing(object sender, CameraCapturingEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Db44In_PossibleAccidentDataReporting(object sender, PossibleAccidentDataReportingEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Db44In_DriverSignedInOrOut(object sender, DriverSignedInOrOutEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static Dictionary<string, DateTime> MonitorForAcc = new Dictionary<string, DateTime>();
        private static int MonitorForAccInterval = 30;//minutes
        void Db44In_GpsDataReceived(object sender, GpsDataReceivedEventArgs e)
        {
            try
            {
                bool acc = e.GpsData.GetState().Acc;
                if (acc)
                {
                    JtjClient.ForwardU01(e.PlateNumber, e.PlateColor, e.GpsData);
                    SpeedOfSendingToJtj++;
                    CountOfSendingToJtj++;
                    FireCommuStatusUpdatedEvent(ParrotModelWrapper.GetMobileSnByPlateNumber(e.PlateNumber), "U01");
                    //FireLoggingEvent(Level.Info, "成功将GPS终端的卫星定位数据包转发给交通局。");
                    //FireGpsLocationUpdatedEvent(mobileInfo, gpsData);
                }
                else
                {
                    DateTime lastAccOff = DateTime.MinValue;
                    MonitorForAcc.TryGetValue(e.PlateNumber, out  lastAccOff);
                    DateTime thisAccOff = DateTime.Now;
                    if ((thisAccOff - lastAccOff).TotalMinutes > MonitorForAccInterval)
                    {
                        if (MonitorForAcc.ContainsKey(e.PlateNumber))
                            MonitorForAcc[e.PlateNumber] = thisAccOff;
                        else
                            MonitorForAcc.Add(e.PlateNumber, thisAccOff);

                        JtjClient.ForwardU01(e.PlateNumber, e.PlateColor, e.GpsData);
                        SpeedOfSendingToJtj++;
                        CountOfSendingToJtj++;
                        try
                        {
                            FireCommuStatusUpdatedEvent(ParrotModelWrapper.GetMobileSnByPlateNumber(e.PlateNumber), "U01");
                        }
                        catch { }
                        //FireLoggingEvent(Level.Info, "成功将GPS终端的卫星定位数据包转发给交通局。");
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                ErrorCountOfSendingToJtj++;
                try
                {
                    FireCommuStatusUpdatedEvent(ParrotModelWrapper.GetMobileSnByPlateNumber(e.PlateNumber), "失败");
                }
                catch { }
                FireLoggingEvent(Level.Info, "将GPS终端的卫星定位数据包转发给交通局时发生异常。详情请查阅系统日志。");
                FireLoggingEvent(Level.Advanced, ex);
            }
        }

        private void OnPlainGpsDataReceivedX(string _ID, byte[] GpsPackByte, MdtWrapper mobileInfo)
        {
            try
            {
                string plateNumber = mobileInfo.Mobile_VehicleRegistration;
                byte plateColor = (byte)(mobileInfo.DB44_VehicleRegistrationColor);
                Db44GpsData gpsData = new Db44GpsData(GpsPackByte);
                JtjClient.ForwardU01(plateNumber, plateColor, gpsData);

                SpeedOfSendingToJtj++;
                CountOfSendingToJtj++;
                FireCommuStatusUpdatedEvent(mobileInfo.MobileID, "U01");
                FireGpsLocationUpdatedEvent(mobileInfo, gpsData);
                return;
            }
            catch
            {
                ErrorCountOfSendingToJtj++;
                FireCommuStatusUpdatedEvent(_ID, "失败");
            }
        }
        private void OnPlainGpsDataReceived(string _ID, byte[] gpsPackBytes, MdtWrapper mobileInfo)
        {
            try
            {
                string plateNumber = mobileInfo.Mobile_VehicleRegistration;
                byte plateColor = (byte)(mobileInfo.DB44_VehicleRegistrationColor);
                Db44GpsData gpsData = new Db44GpsData(gpsPackBytes);

                bool acc = gpsData.GetState().Acc;
                if (acc)
                {
                    JtjClient.ForwardU01(plateNumber, plateColor, gpsData);
                    SpeedOfSendingToJtj++;
                    CountOfSendingToJtj++;
                    FireCommuStatusUpdatedEvent(mobileInfo.MobileID, "U01");
                    FireGpsLocationUpdatedEvent(mobileInfo, gpsData);
                    return;
                }
                else
                {
                    DateTime lastAccOff = DateTime.MinValue;
                    MonitorForAcc.TryGetValue(plateNumber, out  lastAccOff);
                    DateTime thisAccOff = DateTime.Now;
                    if ((thisAccOff - lastAccOff).TotalMinutes > MonitorForAccInterval)
                    {
                        if (MonitorForAcc.ContainsKey(plateNumber))
                            MonitorForAcc[plateNumber] = thisAccOff;
                        else
                            MonitorForAcc.Add(plateNumber, thisAccOff);

                        JtjClient.ForwardU01(plateNumber, plateColor, gpsData);
                        SpeedOfSendingToJtj++;
                        CountOfSendingToJtj++;
                        FireCommuStatusUpdatedEvent(mobileInfo.MobileID, "U01");
                        FireGpsLocationUpdatedEvent(mobileInfo, gpsData);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorCountOfSendingToJtj++;
                FireCommuStatusUpdatedEvent(_ID, "失败");

                FireLoggingEvent(Level.Info, "将GPS终端的卫星定位数据包转发给交通局时发生异常。详情请查阅系统日志。");
                FireLoggingEvent(Level.Advanced, ex);
            }
        }

        void TimerForRefillingLastGpsLocation_Elapsed(object sender, ElapsedEventArgs e)
        {
            FireLoggingEvent(Level.Info, "开始更新车辆的最新位置。");
            UpdateLastGpsLocation();
            FireLoggingEvent(Level.Info, "更新车辆的最新位置完成。");
        }


        private void FireGpsLocationUpdatedEvent(MdtWrapper mdt, Db44GpsData gpsData)
        {
            if (GpsLocationUpdated != null)
            {
                GpsLocationUpdated(this, mdt, gpsData);
            }

        }
        private void FireCommuStatusUpdatedEvent(string mobileSN, string status)
        {
            if (CommuStatusUpdated != null)
            {
                CommuStatusUpdated(mobileSN, status);
            }
        }
        private void FireLoggingEvent(Level level, object message)
        {
            if (Logging != null)
                Logging(this, level, message);
        }
        protected void FireD04ReceivedFromJtjEvent(string plateNumber, byte plateColor, DateTime receivedDateTime)
        {
            if (D04ReceivedFromJtj != null)
            {
                D04ReceivedFromJtj(this, new D04ReceivedFromJtjEventArgs(plateNumber, plateColor, receivedDateTime));
            }
        }

        protected void FireDeliveredToJtjEvent(string plateNumber, string functionCode, DateTime deliveredDateTime)
        {
            if (DeliveredToJtj != null)
            {
                DeliveredToJtj(this, new DeliveredToJtjEventArgs(plateNumber, functionCode, deliveredDateTime));
            }
        }

        /// <summary>
        /// 添加一个SmppServer，格式：网络链路(NetParameter),IP地址,端口。
        /// </summary>
        /// <param name="value"></param>
        public void AddSmppClient(string value)
        {
            string[] arr = value.Split(new char[] { ',' });
            if (arr.Length != 3) return;

            string netParameter = arr[0];
            string ip = arr[1];
            int port = int.Parse(arr[2]);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
            SmppClient client = new SmppClient(this, netParameter, remoteEP);

            client.Logging += new LoggingEventHandler(SmppServer_Logging);
            client.MdtDataReceived += new MdtDataReceivedEventHandler(this.SmppServer_MdtDataReceived);

            SmppClientHash.Add(netParameter, client);
        }

        /// <summary>
        /// 添加一个OldSmppServer，格式：网络链路(NetParameter),IP地址,端口。
        /// </summary>
        /// <param name="value"></param>
        public void AddOldSmppClient(string value)
        {
            string[] arr = value.Split(new char[] { ',' });
            if (arr.Length != 3) return;

            string netParameter = arr[0];
            string ip = arr[1];
            int port = int.Parse(arr[2]);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
            OldSmppClient client = new OldSmppClient(this, netParameter, remoteEP);

            client.Logging += new LoggingEventHandler(SmppServer_Logging);
            client.MdtDataReceived += new MdtDataReceivedEventHandler(SmppServer_MdtDataReceived);

            OldSmppClientHash.Add(netParameter, client);
        }

        void SmppServer_Logging(object sender, Level level, object message)
        {
            FireLoggingEvent(level, message);
        }

        protected void AutoSendCmdToMobile(string _ID, string ReCmd, MdtWrapper mobileInfo, byte ProtocolType)
        {
        }

        private string CmdOut(string mobileId, int mdtModel, string[] args)
        {
            string str = "";
            switch (mdtModel)
            {
                case 251:
                case 252://DB44,华宝
                case 253:
                    return Db44Out.Order(mobileId, mdtModel, args);

                case 101:
                case 102://60型GPRS物流终端
                case 103://50型GSM私家车
                case 701:
                    return TianheGprsOut.Order(mobileId, mdtModel, args);

                case 201://LH-GPRS安防12型(XLD-B2)
                case 202:
                case 203:

                case 205:
                case 206:
                case 207:
                case 208:
                case 209:

                case 210:
                case 211:
                case 212://LH-101G-SMS
                case 213:
                case 215://龙翰
                case 219:
                    return LonghanOut.Order(mobileId, mdtModel, args);
                case 1003:
                    return YelangOut.Order(mobileId, mdtModel, args);
                case 901://SiYuang
                case 902:
                case 801:

                case 104:
                case 105:
                case 106:

                case 204://LH-GSM安防型(XLD-G)
                case 214:
                case 216:
                case 217:
                case 218:

                case 301:
                case 302:
                case 303:
                case 401:

                case 501:
                case 502:
                case 602:
                default:
                    return str;
            }
        }

        private void SmppServer_MdtDataReceived(byte[] sDataBodyByte, int Len, MdtWrapper mobileInfo, string _NetParameter)
        {
            if ((this.IsDebugLoggingEnable) && ((this.DebugMobileID == "") | (mobileInfo.MobileID.IndexOf(this.DebugMobileID) >= 0)))
            {
                string str = "";
                for (int i = 0; i < sDataBodyByte.Length; i++)
                {
                    str = str + " " + sDataBodyByte[i].ToString("X2");
                }
                string str2 = string.Format("MobileID：{0}，Data：{1} - {2}", mobileInfo.MobileID, str, Encoding.Default.GetString(sDataBodyByte));
                FireLoggingEvent(Level.Debug, _NetParameter + ":" + str2);
            }
            switch (mobileInfo.MobileType)
            {
                case 101:
                case 102:
                case 103:
                    this.TianheGprsIn.ProcessIncomingData(mobileInfo.MobileID, mobileInfo.MobileType, sDataBodyByte, mobileInfo);
                    break;

                case 201:
                case 202:
                case 203:
                case 205:
                case 206:
                case 207:
                case 208:
                case 209:
                    this.LonghanIn.DataIn(mobileInfo.MobileID, mobileInfo.MobileType, sDataBodyByte, Len, mobileInfo);
                    break;

                case 210:
                case 211:
                case 212:
                case 213:
                case 215:
                case 219:
                    this.LonghanIn.DataIn(mobileInfo.MobileID, mobileInfo.MobileType, sDataBodyByte, Len, mobileInfo);
                    break;

                case 251:
                case 252:
                case 253:
                    this.Db44In.ProcessIncomingData(mobileInfo.MobileID, mobileInfo.MobileType, sDataBodyByte, mobileInfo);
                    break;

                case 0x2bd:
                    this.TianheGprsIn.ProcessIncomingData(mobileInfo.MobileID, mobileInfo.MobileType, sDataBodyByte, mobileInfo);
                    break;

                case 0x3eb:
                    this.YelangIn.DataIn(mobileInfo.MobileID, mobileInfo.MobileType, sDataBodyByte, Len, mobileInfo);
                    break;
            }
        }


        public void GetMobileBaseInfo(ref byte[] sDataBodyByte, out string MobileID, out byte MobileType, byte netType)
        {
            MobileID = "";
            MobileType = 0;
            string s = "";
            if (sDataBodyByte.Length > 5)
            {
                try
                {
                    if ((sDataBodyByte[0] == 0x7e) & (sDataBodyByte[sDataBodyByte.Length - 1] == 0x7f))
                    {
                        s = Encoding.Default.GetString(sDataBodyByte, 1, sDataBodyByte.Length - 2);
                        byte[] buffer = Convert.FromBase64String(s);
                        if (netType == 1)
                        {
                            MobileID = Db44Util.GetSimNoFromIP(buffer[4], buffer[5], buffer[6], buffer[7]);
                        }
                        else
                        {
                            MobileID = buffer[4].ToString("000") + buffer[5].ToString("000") + (((buffer[6] * 0x100) + buffer[7])).ToString("00000");
                        }
                        sDataBodyByte = buffer;
                    }
                    else if (((sDataBodyByte[0] == 0x2a) & (sDataBodyByte[1] == 0x48)) & (sDataBodyByte[2] == 0x51))
                    {
                        MobileType = 1;
                        MobileID = Encoding.Default.GetString(sDataBodyByte, 4, 10);
                    }
                    else if (((sDataBodyByte[0] == 0x2a) & (sDataBodyByte[1] == 0x44)) & (sDataBodyByte[2] == 0x47))
                    {
                        MobileType = 1;
                        MobileID = Encoding.Default.GetString(sDataBodyByte, 4, 10);
                    }
                    else if (((sDataBodyByte[0] == 0x29) & (sDataBodyByte[1] == 0x29)) & (sDataBodyByte[sDataBodyByte.Length - 1] == 13))
                    {
                        MobileType = 2;
                        MobileID = LonghanWrapper.Get_CarID_From_IP(sDataBodyByte[5], sDataBodyByte[6], sDataBodyByte[7], sDataBodyByte[8]);
                    }
                    else if ((sDataBodyByte[0] == 0x24) | (sDataBodyByte[0] == 80))
                    {
                        MobileType = 1;
                        MobileID = sDataBodyByte[1].ToString("X2") + sDataBodyByte[2].ToString("X2") + sDataBodyByte[3].ToString("X2") + sDataBodyByte[4].ToString("X2") + sDataBodyByte[5].ToString("X2");
                    }
                    else if (((sDataBodyByte[0] == 0x90) & (sDataBodyByte[sDataBodyByte.Length - 3] == 0xff)) & (sDataBodyByte[sDataBodyByte.Length - 1] == 10))
                    {
                        MobileType = 3;
                        MobileID = sDataBodyByte[2].ToString("0") + sDataBodyByte[3].ToString("000") + sDataBodyByte[4].ToString("000");
                    }
                    else if (((sDataBodyByte[0] == 0x5b) & (sDataBodyByte[1] == 0x80)) & (sDataBodyByte[sDataBodyByte.Length - 1] == 0x5d))
                    {
                        MobileType = 4;
                        MobileID = Encoding.Default.GetString(sDataBodyByte, 13, 11);
                    }
                    else if (((((sDataBodyByte[0] == 0x40) & (sDataBodyByte[1] == 0x40)) & (sDataBodyByte[3] == 0x65)) & (sDataBodyByte[sDataBodyByte.Length - 2] == 13)) & (sDataBodyByte[sDataBodyByte.Length - 1] == 10))
                    {
                        MobileType = 5;
                        MobileID = "1" + (((((((sDataBodyByte[6] * 0x100) * 0x100) * 0x100) + ((sDataBodyByte[7] * 0x100) * 0x100)) + (sDataBodyByte[8] * 0x100)) + sDataBodyByte[9])).ToString();
                    }
                    else if ((sDataBodyByte[0] == 60) & (sDataBodyByte[sDataBodyByte.Length - 1] == 0x3e))
                    {
                        MobileType = 5;
                        MobileID = Encoding.Default.GetString(sDataBodyByte, 1, 11);
                    }
                    else if ((sDataBodyByte[0] == 0x26) & (sDataBodyByte[1] == 0x26))
                    {
                        MobileType = 5;
                        MobileID = "1";
                        int num = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            if (sDataBodyByte[(i + num) + 5] != 0x24)
                            {
                                MobileID = MobileID + sDataBodyByte[(i + num) + 5].ToString("X2");
                            }
                            else
                            {
                                MobileID = MobileID + ((sDataBodyByte[(i + num) + 5] + sDataBodyByte[((i + num) + 5) + 1])).ToString("X2");
                                num++;
                            }
                        }
                    }
                    else if (((sDataBodyByte[0] == 0x59) & (sDataBodyByte[1] == 0x47)) & (sDataBodyByte[sDataBodyByte.Length - 1] == 13))
                    {
                        MobileType = 5;
                        string[] strArray2 = new string[] { sDataBodyByte[3].ToString("X2"), sDataBodyByte[4].ToString("X2"), sDataBodyByte[5].ToString("X2"), sDataBodyByte[6].ToString("X2"), sDataBodyByte[7].ToString("X2"), (sDataBodyByte[8] / 0x10).ToString("X") };
                        MobileID = string.Concat(strArray2);
                    }
                }
                catch (Exception ex)
                {
                    FireLoggingEvent(Level.Debug, "GetMobileBaseInfo Error   " + s + "。详情请查阅系统日志。");
                    FireLoggingEvent(Level.Advanced, ex);
                }
            }
        }

        private void WakeupAllClients()
        {
            try
            {
                for (int i = 0; i < OldSmppClientHash.Count; i++)
                {
                    SmppClientBase client = (SmppClientBase)OldSmppClientHash.GetByIndex(i);
                    if (!client.IsRunning)
                    {
                        client.Reset();
                    }
                }
                for (int j = 0; j < SmppClientHash.Count; j++)
                {
                    SmppClientBase client = (SmppClientBase)SmppClientHash.GetByIndex(j);
                    if (!client.IsRunning)
                    {
                        client.Reset();
                    }
                }
                if (!JtjClient.IsRunning)
                {
                    JtjClient.Reset();
                }
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Advanced, ex);
            }
        }
        public void TimerForKeepingAlive_Elapsed(object source, ElapsedEventArgs e)
        {
            WakeupAllClients();

            if (this.TimerCountOfReduceProcessWorkingSetSize < 14)//不足15次计数，即5分钟
            {
                this.TimerCountOfReduceProcessWorkingSetSize++;
            }
            else
            {
                //try
                //{
                //    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                //}
                //catch { }
                // For reasons to comment, see: http://hi.baidu.com/taobaoshoping/blog/item/a1f6baf52d523a21bd3109f5.html
                FireLoggingEvent(Level.Info, "执行定时任务：清理内存。");
                this.TimerCountOfReduceProcessWorkingSetSize = 0;
            }
        }



        private void OnPlainMessageReceived(string _ID, int OperationType, string Operation, string OperationDescribe, MdtWrapper mobileInfo)
        {
            int num = OperationType;
            if (OperationType > 100)
            {
                num = 2;
            }
            try
            {
                byte num2 = 5;
                byte num3 = 1;
                string s = _ID + "," + num.ToString() + "," + Operation.ToString() + "," + OperationDescribe.ToString();
                s = Convert.ToBase64String(Encoding.Default.GetBytes(s));
                string str2 = PCmdHead + "," + num2.ToString() + "," + num3.ToString() + "," + s.ToString() + ",\r\n";
            }
            catch
            {
            }
        }


        public void OnImageReceived(string _ID, int MointerID, long ImageID, int ImageLen, int ImageSeq, string ImageBody, string ImgStatu, DateTime FilmDateTime, MdtWrapper mobileInfo)
        {
            if (this.IsDebugLoggingEnable && (this.DebugMobileID == _ID))
            {
                this.ImageReceived(_ID, MointerID, ImageID, ImageLen, ImageSeq, ImageBody, ImgStatu, FilmDateTime, mobileInfo);
            }

            try
            {
                string plateNumber = mobileInfo.Mobile_VehicleRegistration;
                byte plateColor = (byte)(mobileInfo.DB44_VehicleRegistrationColor + 1);
                byte[] imageData = Convert.FromBase64String(ImageBody);
                JtjClient.SendU02(plateNumber, plateColor, FilmDateTime, (byte)MointerID, "jpg", (byte)ImageLen, (byte)ImageSeq, imageData);

                SpeedOfSendingToJtj++;
                CountOfSendingToJtj++;
                FireCommuStatusUpdatedEvent(mobileInfo.MobileID, "U02");
                return;
            }
            catch
            {
                ErrorCountOfSendingToJtj++;
                FireCommuStatusUpdatedEvent(_ID, "失败");
            }
        }

        /// <summary>
        /// 上传事故终点信息数据包给交通局。
        /// </summary>
        /// <param name="mobileId"></param>
        /// <param name="Msg_type"></param>
        /// <param name="cameraNumber"></param>
        /// <param name="QT"></param>
        /// <param name="trafficData"></param>
        /// <param name="mdt"></param>
        private void OnTrafficAccidentRelativeDataReceived(string _ID, int Msg_type, byte packetIndex, byte[] occurTimeBytes, byte[] trafficData, MdtWrapper mobileInfo)
        {
            try
            {
                string plateNumber = mobileInfo.Mobile_VehicleRegistration;
                byte plateColor = (byte)(mobileInfo.DB44_VehicleRegistrationColor + 1);
                JtjClient.SendU04(plateNumber, plateColor, packetIndex, occurTimeBytes, trafficData);

                SpeedOfSendingToJtj++;
                CountOfSendingToJtj++;
                FireCommuStatusUpdatedEvent(mobileInfo.MobileID, "U04");
                return;
            }
            catch
            {
                ErrorCountOfSendingToJtj++;
                FireCommuStatusUpdatedEvent(_ID, "失败");
            }
        }

        public static void ReRegisterRemoteInfo(byte Protocoltype, ref string tempNewRemoteInfoKey, ref MdtWrapper mobileInfo)
        {
            string key = "";
            bool flag = false;
            if (Protocoltype == 0)
            {
                key = mobileInfo.TcpRemoteInfo;
                if (key != tempNewRemoteInfoKey)
                {
                    mobileInfo.TcpRemoteInfo = tempNewRemoteInfoKey;
                    flag = true;
                }
            }
            else if (Protocoltype == 1)
            {
                key = mobileInfo.UdpRemoteInfo;
                if (key != tempNewRemoteInfoKey)
                {
                    mobileInfo.UdpRemoteInfo = tempNewRemoteInfoKey;
                    flag = true;
                }
            }
            else if (Protocoltype == 2)
            {
                mobileInfo.SmsComInfo = tempNewRemoteInfoKey;
            }
            else if (Protocoltype == 3)
            {
                mobileInfo.SmsRemoteInfo = tempNewRemoteInfoKey;
            }
            if (flag)
            {
                lock (RemoteInfo_Hash)
                {
                    RemoteInfo_Hash.Remove(key);
                    RemoteInfo_Hash.Add(tempNewRemoteInfoKey, mobileInfo);
                }
            }
        }

        public void UpdateLastGpsLocation()
        {
            int num = 0;
            StreamWriter writer = null;
            bool flag = false;
            try
            {
                try
                {
                    File.Delete(SysPath + @"\LastGpsInfo.ini");
                }
                catch
                {
                }
                lock (LastGpsInfo_Hash)
                {
                    foreach (string str in LastGpsInfo_Hash.Values)
                    {
                        try
                        {
                            if (flag)
                            {
                                writer.Write(str + "\r\n");
                            }
                            else
                            {
                                writer = new StreamWriter(new FileStream(SysPath + @"\LastGpsInfo.ini", FileMode.OpenOrCreate, FileAccess.Write));
                                writer.AutoFlush = true;
                                writer.Write(str + "\r\n");
                                num++;
                                flag = true;
                            }
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                FireLoggingEvent(Level.Advanced, exception);
            }
        }

        public int SendCmdToMobile(string _ID, string[] P)
        {
            int num;
            lock (MobileInfo_Hash)
            {
                try
                {
                    if (MobileInfo_Hash.ContainsKey(_ID))
                    {
                        string str4;
                        string mobileID;
                        MdtWrapper mobileInfo = (MdtWrapper)MobileInfo_Hash[_ID];
                        if ((P[0] == "66") && (mobileInfo.Mobile_IntervalUdp > int.Parse(P[1])))
                        {
                            P[1] = mobileInfo.Mobile_IntervalUdp.ToString();
                        }
                        if (P[0] == "76")
                        {
                            if (mobileInfo.LastKeepPrivacyTime > DateTime.Now)
                            {
                                return 10;
                            }
                            return 6;
                        }
                        string tcpRemoteInfo = "";
                        string s = CmdOut(_ID, mobileInfo.MobileType, P);
                        switch (s)
                        {
                            case "Err":
                                return 1;

                            case "":
                                return 6;

                            default:
                                if (!mobileInfo.IsOldSmpp)
                                {
                                    if (mobileInfo.ProtocolType == 0)
                                    {
                                        tcpRemoteInfo = mobileInfo.TcpRemoteInfo;
                                    }
                                    else if (mobileInfo.ProtocolType == 1)
                                    {
                                        tcpRemoteInfo = mobileInfo.UdpRemoteInfo;
                                    }
                                    else if (mobileInfo.ProtocolType == 2)
                                    {
                                        tcpRemoteInfo = mobileInfo.SmsComInfo;
                                    }
                                    else if (mobileInfo.ProtocolType == 3)
                                    {
                                        tcpRemoteInfo = mobileInfo.SmsRemoteInfo;
                                    }
                                    if (tcpRemoteInfo == "")
                                    {
                                        return 9;
                                    }
                                    string str3 = string.Format(
                                        "{0},1,{1},{2},{3},0,\r\n",
                                        PSmppHead, TraceGenerator.NextTrace(), tcpRemoteInfo, s);
                                    //return SendCmdToMobileFromSmpp(ref Encoding.Default.GetBytes(str3), ref mdt);
                                    return SendCmdToMobileFromSmpp(Encoding.Default.GetBytes(str3), mobileInfo.NetParameter);
                                }
                                if (!mobileInfo.IsOldSmpp)
                                {
                                    return 0;
                                }
                                str4 = "";
                                mobileID = "";
                                switch (mobileInfo.MobileType)
                                {
                                    case 0x191:
                                    case 0x65:
                                    case 0x66:
                                    case 0xcb:
                                    case 0xcd:
                                    case 0x2bd:
                                        str4 = "203";
                                        mobileID = mobileInfo.TcpRemoteInfo;
                                        break;

                                    case 0x1f5:
                                    case 0x67:
                                    case 210:
                                    case 0xd4:
                                    case 0xd5:
                                    case 0xd7:
                                    case 0x385:
                                    case 0x386:
                                        str4 = "23";
                                        mobileID = mobileInfo.MobileID;
                                        break;

                                    case 0xc9:
                                    case 0xca:
                                    case 0xce:
                                    case 0xcf:
                                    case 0xd3:
                                    case 220:
                                    case 0x321:
                                    case 0x3eb:
                                        str4 = "211";
                                        mobileID = mobileInfo.TcpRemoteInfo;
                                        break;

                                    case 0xdb:
                                        str4 = "23";
                                        mobileID = mobileInfo.MobileID;
                                        s = PSmppHead + "1," +
                TraceGenerator.NextTrace().ToString() + "," + str4 + ":" + mobileID + ",00," + s + "\r\n";
                                        s = Convert.ToBase64String(Encoding.Default.GetBytes(s));
                                        str4 = "203";
                                        mobileID = "13900000000";
                                        break;
                                }
                                break;
                        }
                        if (mobileID == "")
                        {
                            return 9;
                        }
                        string str6 = s;
                        str6 = PSmppHead + "1," +
                TraceGenerator.NextTrace().ToString() + "," + str4 + ":" + mobileID + ",00," + str6 + "\r\n";
                        //return SendCmdToMobileFromOldSmpp(ref Encoding.Default.GetBytes(str6), ref mdt);
                        return SendCmdToMobileFromOldSmpp(Encoding.Default.GetBytes(str6), mobileInfo.NetParameter);
                    }
                    num = 3;
                }
                catch
                {
                    num = 100;
                }
            }
            return num;
        }

        private static int SendCmdToMobileFromOldSmpp(byte[] buffer, string netParameter)
        {
            try
            {
                SmppClientBase client = (SmppClientBase)OldSmppClientHash[netParameter];
                if (client.IsRunning)
                {
                    client.Send(buffer, 0, buffer.Length);
                }
                return 0;
            }
            catch
            {
                return 8;
            }
        }

        private static int SendCmdToMobileFromSmpp(byte[] buffer, string netParameter)
        {
            try
            {
                SmppClientBase client = (SmppClientBase)SmppClientHash[netParameter];
                if (client.IsRunning)
                {
                    client.Send(buffer, 0, buffer.Length);
                }
                return 0;
            }
            catch
            {
                return 8;
            }
        }

        [DllImport("Kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

        //public void testDataIn(string MobileID, string sDataBodyStr, string _NetParameter)
        //{
        //    if (MobileInfo_Hash.ContainsKey(MobileID))
        //    {
        //        MdtManager mdt = (MdtManager)MobileInfo_Hash[MobileID];
        //        byte[] bytes = Encoding.Default.GetBytes(sDataBodyStr);
        //        this.OldSmpp_MdtDataReceived(bytes, bytes.Length, mdt, _NetParameter);
        //    }
        //}

        internal static MdtWrapper GetMdtStaticInfo(string plateNumber)
        {
            throw new NotImplementedException();
        }
    }
}