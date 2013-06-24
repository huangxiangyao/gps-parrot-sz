using System;
using System.Collections;
using System.Net.Sockets;
using Parrot.Models;

namespace Parrot.Models
{
    public class MdtWrapper
    {
        // Fields
        private string _AlarmInfoStr;
        private bool _AutoCall;
        private bool _AutoSendAlarmMsgToUser;
        private string _AutoSendAlarmMsgToUserNo;
        private int _DB44_CompanyCode = 0;
        private string _DB44_EnterpriseCode = "0";
        private string _DB44_MDT_Type = "";
        private string _DB44_VehicleGroupCode = "";
        private string _DB44_VehicleGroupCYZGZ = "";
        private string _DB44_VehicleGroupName = "";
        private string _DB44_VehicleRegistration_Type = "";
        /// <summary>
        /// 按DB44
        /// </summary>
        private byte _DB44_VehicleRegistrationColor = 0;
        private string _DB44_VehicleType = "";
        /// <summary>
        /// 按DB44
        /// </summary>
        private byte _DB44_VehicleUseType = 0;
        private bool _Gps_Tag;
        private string _GpsInfoStr;
        private int _GpsRecivInterval;
        private bool _H1_H_Tag;
        private bool _H1_L_Tag;
        private bool _H2_H_Tag;
        private bool _H2_L_Tag;
        private bool _IsCheckRunTime;
        private bool _IsCheckStopTime;
        private bool _IsOldSmpp;
        private int _IsXMode;
        private bool _L1_H_Tag;
        private bool _L1_L_Tag;
        private bool _L2_H_Tag;
        private bool _L2_L_Tag;
        private string _LastAlarmShieldStr;
        private DateTime _LastAlarmShieldTime;
        private string _LastAlarmStr;
        private int _LastAlarmTimes;
        private string _LastAV;
        private string _LastCallAlarmStr;
        private DateTime _LastCallAlarmTime;
        private string _LastCmdReTurn;
        private double _LastF;
        private DateTime _LastKeepPrivacyTime;
        private DateTime _LastLinkTime;
        private string _LastSendAlarmStr;
        private DateTime _LastSendAlarmTime;
        private string _LastST;
        private DateTime _LastStopTime;
        private string _LastUT;
        private double _LastV;
        private double _LastX;
        private double _LastY;
        private double _Mileages;
        private string _Mobile_Group_Name = "";
        private int _Mobile_IntervalSms;
        private int _Mobile_IntervalTcp;
        private int _Mobile_IntervalUdp;
        private string _Mobile_Sensor_H1_H;
        private string _Mobile_Sensor_H1_L;
        private string _Mobile_Sensor_H2_H;
        private string _Mobile_Sensor_H2_L;
        private string _Mobile_Sensor_L1_H;
        private string _Mobile_Sensor_L1_L;
        private string _Mobile_Sensor_L2_H;
        private string _Mobile_Sensor_L2_L;
        private string _Mobile_Tel1;
        private string _Mobile_Tel2;
        private string _Mobile_Tel3;
        private string _Mobile_VehicleColor;
        private string _Mobile_VehicleName;
        private string _Mobile_VehicleRegistration;
        private string _MobileID;
        private int _MobileType;
        private string _NetParameter;
        private double _OilCapacity;
        private bool _Power_Tag;
        private int _ProtocolType;
        private bool _RegionSpeed;
        private ArrayList _RegisterList;
        private string _Sim_ID;
        private string _SmsComInfo;
        private string _SmsRemoteInfo;
        private int _SysID;
        private string _TcpRemoteInfo;
        private double _Temperature;
        private string _UdpRemoteInfo;
        private byte rectAlarmType;


        // Methods
        public int CheckLastStopTime()
        {
            int num = -1;
            if (this._LastV < 5.0)
            {
                if (this._LastStopTime.AddSeconds(180.0) < DateTime.Now)
                {
                    if (!this._IsCheckStopTime)
                    {
                        num = 0;
                    }
                    else
                    {
                        num = -1;
                    }
                    this._IsCheckStopTime = true;
                    this._IsCheckRunTime = false;
                }
                return num;
            }
            if (this._LastV >= 5.0)
            {
                if (!this._IsCheckRunTime)
                {
                    num = 1;
                }
                else
                {
                    num = -1;
                }
                this._IsCheckRunTime = true;
                this._IsCheckStopTime = false;
            }
            return num;
        }

        public int GetGpsRecivInterval()
        {
            if (this._LastLinkTime.AddSeconds(180.0) < DateTime.Now)
            {
                this._GpsRecivInterval = 0;
                return 2;
            }
            this._GpsRecivInterval++;
            if (this._GpsRecivInterval >= 4)
            {
                this._GpsRecivInterval = 0;
                return 1;
            }
            return 0;
        }

        public void Init(string mobileID, string sim_ID, int sysID)
        {
            this._RegisterList = ArrayList.Synchronized(new ArrayList());
            this._MobileID = mobileID;
            this._Sim_ID = sim_ID;
            this._SysID = sysID;
            this._TcpRemoteInfo = "";
            this._UdpRemoteInfo = "";
            this._SmsRemoteInfo = "";
            this._SmsComInfo = "";
            this._LastStopTime = DateTime.Now;
            this._GpsRecivInterval = 0;
            this._IsCheckStopTime = false;
            this._IsCheckRunTime = false;
            this._Mileages = 0.0;
            this._Temperature = 255.0;
            this._OilCapacity = -1.0;
            this._LastCmdReTurn = "";
            this._Mobile_IntervalTcp = 30;
            this._Mobile_IntervalUdp = 30;
            this._Mobile_IntervalSms = 30;
            this._AutoSendAlarmMsgToUser = false;
            this._AutoSendAlarmMsgToUserNo = "";
            this._H1_H_Tag = false;
            this._H1_L_Tag = false;
            this._H2_H_Tag = false;
            this._H2_L_Tag = false;
            this._L1_H_Tag = false;
            this._L1_L_Tag = false;
            this._L2_H_Tag = false;
            this._L2_L_Tag = false;
            this._Power_Tag = false;
            this._Gps_Tag = false;
            this._IsOldSmpp = false;
            this._IsXMode = -1;
            this._RegionSpeed = false;
            this._LastSendAlarmStr = "";
            this._LastCallAlarmStr = "";
            this._LastSendAlarmTime = new DateTime();
            this._LastCallAlarmTime = new DateTime();
            this._Mobile_VehicleRegistration = "";
            this._Mobile_VehicleName = "";
            this._LastAV = "";
            this._AutoCall = false;
            this._Mobile_Tel1 = "";
            this._Mobile_Tel2 = "";
            this._Mobile_Tel3 = "";
            this._NetParameter = "";
        }

        public int RegisterClient(Socket clientsock, int I)
        {
            try
            {
                if (I > 0)
                {
                    if (this._RegisterList.IndexOf(clientsock) < 0)
                    {
                        this._RegisterList.Add(clientsock);
                    }
                }
                else if (I == 0)
                {
                    int index = this._RegisterList.IndexOf(clientsock);
                    if (index >= 0)
                    {
                        this._RegisterList.RemoveAt(index);
                    }
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        // Properties
        public string AlarmInfoStr
        {
            get
            {
                return this._AlarmInfoStr;
            }
            set
            {
                this._AlarmInfoStr = value;
            }
        }

        public bool AutoCall
        {
            get
            {
                return this._AutoCall;
            }
            set
            {
                this._AutoCall = value;
            }
        }

        public bool AutoSendAlarmMsgToUser
        {
            get
            {
                return this._AutoSendAlarmMsgToUser;
            }
            set
            {
                this._AutoSendAlarmMsgToUser = value;
            }
        }

        public string AutoSendAlarmMsgToUserNo
        {
            get
            {
                return this._AutoSendAlarmMsgToUserNo;
            }
            set
            {
                this._AutoSendAlarmMsgToUserNo = value;
            }
        }

        public int DB44_CompanyCode
        {
            get
            {
                return this._DB44_CompanyCode;
            }
            set
            {
                this._DB44_CompanyCode = value;
            }
        }

        public string DB44_EnterpriseCode
        {
            get
            {
                return this._DB44_EnterpriseCode;
            }
            set
            {
                this._DB44_EnterpriseCode = value;
            }
        }

        public string DB44_MDT_Type
        {
            get
            {
                return this._DB44_MDT_Type;
            }
            set
            {
                this._DB44_MDT_Type = value;
            }
        }

        public string DB44_VehicleGroupCode
        {
            get
            {
                return this._DB44_VehicleGroupCode;
            }
            set
            {
                this._DB44_VehicleGroupCode = value;
            }
        }

        public string DB44_VehicleGroupCYZGZ
        {
            get
            {
                return this._DB44_VehicleGroupCYZGZ;
            }
            set
            {
                this._DB44_VehicleGroupCYZGZ = value;
            }
        }

        public string DB44_VehicleGroupName
        {
            get
            {
                return this._DB44_VehicleGroupName;
            }
            set
            {
                this._DB44_VehicleGroupName = value;
            }
        }

        public string DB44_VehicleRegistration_Type
        {
            get
            {
                return this._DB44_VehicleRegistration_Type;
            }
            set
            {
                this._DB44_VehicleRegistration_Type = value;
            }
        }

        /// <summary>
        /// 车牌颜色。严格遵循DB44。
        /// </summary>
        public byte DB44_VehicleRegistrationColor
        {
            get
            {
                return this._DB44_VehicleRegistrationColor;
            }
            set
            {
                this._DB44_VehicleRegistrationColor = value;
            }
        }

        public string DB44_VehicleType
        {
            get
            {
                return this._DB44_VehicleType;
            }
            set
            {
                this._DB44_VehicleType = value;
            }
        }

        public byte DB44_VehicleUseType
        {
            get
            {
                return this._DB44_VehicleUseType;
            }
            set
            {
                this._DB44_VehicleUseType = value;
            }
        }

        public bool Gps_Tag
        {
            get
            {
                return this._Gps_Tag;
            }
            set
            {
                this._Gps_Tag = value;
            }
        }

        public string GpsInfoStr
        {
            get
            {
                return this._GpsInfoStr;
            }
            set
            {
                this._GpsInfoStr = value;
            }
        }

        public int GpsRecivInterval
        {
            get
            {
                return this._GpsRecivInterval;
            }
            set
            {
                this._GpsRecivInterval = value;
            }
        }

        public bool H1_H_Tag
        {
            get
            {
                return this._H1_H_Tag;
            }
            set
            {
                this._H1_H_Tag = value;
            }
        }

        public bool H1_L_Tag
        {
            get
            {
                return this._H1_L_Tag;
            }
            set
            {
                this._H1_L_Tag = value;
            }
        }

        public bool H2_H_Tag
        {
            get
            {
                return this._H2_H_Tag;
            }
            set
            {
                this._H2_H_Tag = value;
            }
        }

        public bool H2_L_Tag
        {
            get
            {
                return this._H2_L_Tag;
            }
            set
            {
                this._H2_L_Tag = value;
            }
        }

        public bool IsOldSmpp
        {
            get
            {
                return this._IsOldSmpp;
            }
            set
            {
                this._IsOldSmpp = value;
            }
        }

        public int IsXMode
        {
            get
            {
                return this._IsXMode;
            }
            set
            {
                this._IsXMode = value;
            }
        }

        public bool L1_H_Tag
        {
            get
            {
                return this._L1_H_Tag;
            }
            set
            {
                this._L1_H_Tag = value;
            }
        }

        public bool L1_L_Tag
        {
            get
            {
                return this._L1_L_Tag;
            }
            set
            {
                this._L1_L_Tag = value;
            }
        }

        public bool L2_H_Tag
        {
            get
            {
                return this._L2_H_Tag;
            }
            set
            {
                this._L2_H_Tag = value;
            }
        }

        public bool L2_L_Tag
        {
            get
            {
                return this._L2_L_Tag;
            }
            set
            {
                this._L2_L_Tag = value;
            }
        }

        public string LastAlarmShieldStr
        {
            get
            {
                return this._LastAlarmShieldStr;
            }
            set
            {
                this._LastAlarmShieldStr = value;
            }
        }

        public DateTime LastAlarmShieldTime
        {
            get
            {
                return this._LastAlarmShieldTime;
            }
            set
            {
                this._LastAlarmShieldTime = value;
            }
        }

        public string LastAlarmStr
        {
            get
            {
                return this._LastAlarmStr;
            }
            set
            {
                this._LastAlarmStr = value;
            }
        }

        public int LastAlarmTimes
        {
            get
            {
                return this._LastAlarmTimes;
            }
            set
            {
                this._LastAlarmTimes = value;
            }
        }

        public string LastAV
        {
            get
            {
                return this._LastAV;
            }
            set
            {
                this._LastAV = value;
            }
        }

        public string LastCallAlarmStr
        {
            get
            {
                return this._LastCallAlarmStr;
            }
            set
            {
                this._LastCallAlarmStr = value;
            }
        }

        public DateTime LastCallAlarmTime
        {
            get
            {
                return this._LastCallAlarmTime;
            }
            set
            {
                this._LastCallAlarmTime = value;
            }
        }

        public string LastCmdReTurn
        {
            get
            {
                return this._LastCmdReTurn;
            }
            set
            {
                this._LastCmdReTurn = value;
            }
        }

        public double LastF
        {
            get
            {
                return this._LastF;
            }
            set
            {
                this._LastF = value;
            }
        }

        public DateTime LastKeepPrivacyTime
        {
            get
            {
                return this._LastKeepPrivacyTime;
            }
            set
            {
                this._LastKeepPrivacyTime = value;
            }
        }

        public DateTime LastLinkTime
        {
            get
            {
                return this._LastLinkTime;
            }
            set
            {
                this._LastLinkTime = value;
                if (this._LastV >= 5.0)
                {
                    this._LastStopTime = this._LastLinkTime;
                }
            }
        }

        public string LastSendAlarmStr
        {
            get
            {
                return this._LastSendAlarmStr;
            }
            set
            {
                this._LastSendAlarmStr = value;
            }
        }

        public DateTime LastSendAlarmTime
        {
            get
            {
                return this._LastSendAlarmTime;
            }
            set
            {
                this._LastSendAlarmTime = value;
            }
        }

        public string LastST
        {
            get
            {
                return this._LastST;
            }
            set
            {
                this._LastST = value;
            }
        }

        public string LastUT
        {
            get
            {
                return this._LastUT;
            }
            set
            {
                this._LastUT = value;
            }
        }

        public double LastV
        {
            get
            {
                return this._LastV;
            }
            set
            {
                this._LastV = value;
            }
        }

        public double LastX
        {
            get
            {
                return this._LastX;
            }
            set
            {
                this._LastX = value;
            }
        }

        public double LastY
        {
            get
            {
                return this._LastY;
            }
            set
            {
                this._LastY = value;
            }
        }

        public double Mileages
        {
            get
            {
                return this._Mileages;
            }
            set
            {
                this._Mileages = value;
            }
        }

        public string Mobile_Group_Name
        {
            get
            {
                return this._Mobile_Group_Name;
            }
            set
            {
                this._Mobile_Group_Name = value;
            }
        }

        public int Mobile_IntervalSms
        {
            get
            {
                return this._Mobile_IntervalSms;
            }
            set
            {
                this._Mobile_IntervalSms = value;
            }
        }

        public int Mobile_IntervalTcp
        {
            get
            {
                return this._Mobile_IntervalTcp;
            }
            set
            {
                this._Mobile_IntervalTcp = value;
            }
        }

        public int Mobile_IntervalUdp
        {
            get
            {
                return this._Mobile_IntervalUdp;
            }
            set
            {
                this._Mobile_IntervalUdp = value;
            }
        }

        public string Mobile_Sensor_H1_H
        {
            get
            {
                return this._Mobile_Sensor_H1_H;
            }
            set
            {
                this._Mobile_Sensor_H1_H = value;
            }
        }

        public string Mobile_Sensor_H1_L
        {
            get
            {
                return this._Mobile_Sensor_H1_L;
            }
            set
            {
                this._Mobile_Sensor_H1_L = value;
            }
        }

        public string Mobile_Sensor_H2_H
        {
            get
            {
                return this._Mobile_Sensor_H2_H;
            }
            set
            {
                this._Mobile_Sensor_H2_H = value;
            }
        }

        public string Mobile_Sensor_H2_L
        {
            get
            {
                return this._Mobile_Sensor_H2_L;
            }
            set
            {
                this._Mobile_Sensor_H2_L = value;
            }
        }

        public string Mobile_Sensor_L1_H
        {
            get
            {
                return this._Mobile_Sensor_L1_H;
            }
            set
            {
                this._Mobile_Sensor_L1_H = value;
            }
        }

        public string Mobile_Sensor_L1_L
        {
            get
            {
                return this._Mobile_Sensor_L1_L;
            }
            set
            {
                this._Mobile_Sensor_L1_L = value;
            }
        }

        public string Mobile_Sensor_L2_H
        {
            get
            {
                return this._Mobile_Sensor_L2_H;
            }
            set
            {
                this._Mobile_Sensor_L2_H = value;
            }
        }

        public string Mobile_Sensor_L2_L
        {
            get
            {
                return this._Mobile_Sensor_L2_L;
            }
            set
            {
                this._Mobile_Sensor_L2_L = value;
            }
        }

        public string Mobile_Tel1
        {
            get
            {
                return this._Mobile_Tel1;
            }
            set
            {
                this._Mobile_Tel1 = value;
            }
        }

        public string Mobile_Tel2
        {
            get
            {
                return this._Mobile_Tel2;
            }
            set
            {
                this._Mobile_Tel2 = value;
            }
        }

        public string Mobile_Tel3
        {
            get
            {
                return this._Mobile_Tel3;
            }
            set
            {
                this._Mobile_Tel3 = value;
            }
        }

        public string Mobile_VehicleColor
        {
            get
            {
                return this._Mobile_VehicleColor;
            }
            set
            {
                this._Mobile_VehicleColor = value;
            }
        }

        public string Mobile_VehicleName
        {
            get
            {
                return this._Mobile_VehicleName;
            }
            set
            {
                this._Mobile_VehicleName = value;
            }
        }

        public string Mobile_VehicleRegistration
        {
            get
            {
                return this._Mobile_VehicleRegistration;
            }
            set
            {
                this._Mobile_VehicleRegistration = value;
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        public string MobileID
        {
            get
            {
                return this._MobileID;
            }
            set
            {
                this._MobileID = value;
            }
        }

        /// <summary>
        /// MDT代码（DB44）。 
        /// </summary>
        public string MdtCode { get; set; }

        /// <summary>
        /// GPS终端类型。其中，251,252,253支持DB44协议。
        /// </summary>
        public int MobileType
        {
            get
            {
                return this._MobileType;
            }
            set
            {
                this._MobileType = value;
            }
        }

        public string NetParameter
        {
            get
            {
                return this._NetParameter;
            }
            set
            {
                this._NetParameter = value;
            }
        }

        public double OilCapacity
        {
            get
            {
                return this._OilCapacity;
            }
            set
            {
                this._OilCapacity = value;
            }
        }

        public bool Power_Tag
        {
            get
            {
                return this._Power_Tag;
            }
            set
            {
                this._Power_Tag = value;
            }
        }

        public int ProtocolType
        {
            get
            {
                return this._ProtocolType;
            }
            set
            {
                this._ProtocolType = value;
            }
        }

        public byte RectAlarmType
        {
            get
            {
                return this.rectAlarmType;
            }
            set
            {
                this.rectAlarmType = value;
            }
        }

        public bool RegionSpeed
        {
            get
            {
                return this._RegionSpeed;
            }
            set
            {
                this._RegionSpeed = value;
            }
        }

        public ArrayList RegisterList
        {
            get
            {
                return this._RegisterList;
            }
        }

        public string SimID
        {
            get
            {
                return this._Sim_ID;
            }
            set
            {
                this._Sim_ID = value;
            }
        }

        public string SmsComInfo
        {
            get
            {
                return this._SmsComInfo;
            }
            set
            {
                this._SmsComInfo = value;
            }
        }

        public string SmsRemoteInfo
        {
            get
            {
                return this._SmsRemoteInfo;
            }
            set
            {
                this._SmsRemoteInfo = value;
            }
        }

        public int SysID
        {
            get
            {
                return this._SysID;
            }
            set
            {
                this._SysID = value;
            }
        }

        public string TcpRemoteInfo
        {
            get
            {
                return this._TcpRemoteInfo;
            }
            set
            {
                this._TcpRemoteInfo = value;
            }
        }

        public double Temperature
        {
            get
            {
                return this._Temperature;
            }
            set
            {
                this._Temperature = value;
            }
        }

        public string UdpRemoteInfo
        {
            get
            {
                return this._UdpRemoteInfo;
            }
            set
            {
                this._UdpRemoteInfo = value;
            }
        }

        public void UpdateByCarListTable()
        {
            CarList mobile = ParrotModelWrapper.GetCarListByMobileID(this.MobileID);
            if (mobile == null) return;

            this.MdtCode = mobile.Mobile_SN;

            this.Mobile_VehicleRegistration = mobile.Mobile_VehicleRegistration;
            this.DB44_EnterpriseCode = mobile.DB44_EnterpriseCode;
            this.DB44_MDT_Type = mobile.DB44_MDT_Type;
            this.DB44_VehicleGroupCode = mobile.DB44_VehicleGroupCode;
            try
            {
                this.DB44_CompanyCode = int.Parse(mobile.DB44_CompanyCode);
            }
            catch { }
            try
            {
                this.DB44_VehicleRegistrationColor = (byte)(byte.Parse(mobile.DB44_VehicleRegistrationColor) + 1);
            }
            catch { }
            this.DB44_VehicleRegistration_Type = mobile.DB44_VehicleRegistration_Type;
            this.DB44_VehicleType = mobile.DB44_VehicleType;
            try
            {
                this.DB44_VehicleUseType = (byte)(byte.Parse(mobile.DB44_VehicleUseType) + 1);
            }
            catch { }
            this.DB44_VehicleGroupName = mobile.DB44_VehicleGroupName;

            this.DB44_VehicleGroupCYZGZ = mobile.DB44_VehicleGroupCYZGZ;

            this.NetParameter = mobile.NetParameter;
            if (!string.IsNullOrEmpty(mobile.NetParameter))
            {
                this.IsOldSmpp = (this.NetParameter.Length == 11);
            }
            if (this.ProtocolType == 2)
            {
                try
                {
                    this.SmsComInfo = string.Format("{0},{1},{2}",
                        ProtocolType,
                        mobile.Mobile_Sim,
                        this.NetParameter);
                }
                catch
                {
                }
            }
            else if (this.ProtocolType == 3)
            {
                try
                {
                    this.SmsRemoteInfo = string.Format("{0},{1},{2}",
                       ProtocolType,
                        mobile.Mobile_Sim,
                        this.NetParameter);
                }
                catch
                {
                }
            }
            this.MobileType = mobile.Mobile_Type;
        }
        /// <summary>
        /// 取出CarList表中没有的三个字段：
        /// Mobile_MostlyProtocol
        /// Mobile_IntervalSms
        /// Mobile_VehicleColor
        /// </summary>
        public void UpdateByMobileInfoList()
        {
            MobileInfoList mobile = ParrotModelWrapper.GetMobileInfoListByMobileID(this.MobileID);
            if (mobile == null) return;

            try
            {
                if (mobile.Mobile_MostlyProtocol.HasValue)
                {
                    this.ProtocolType = (int)mobile.Mobile_MostlyProtocol.Value;
                }
            }
            catch { }

            if (mobile.Mobile_IntervalSms.HasValue)
            {
                this.Mobile_IntervalSms = mobile.Mobile_IntervalSms.Value;
            }
            this.Mobile_VehicleName = mobile.Mobile_VehicleName;
            if (!string.IsNullOrEmpty(mobile.Mobile_VehicleColor))
            {
                this.Mobile_VehicleColor = mobile.Mobile_VehicleColor.Replace("色", "");
            }
        }

    }
}