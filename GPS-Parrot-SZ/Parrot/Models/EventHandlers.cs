using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Parrot.Models
{
    public enum Level
    {
        Info = 0,
        Debug,
        Advanced
    }


    public class DeliveredToJtjEventArgs : EventArgs
    {
        /// <summary>
        /// 终端ID
        /// </summary>
        //public string MdtId { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNumber { get; set; }
        /// <summary>
        /// 车牌颜色，如1
        /// </summary>
        //public byte PlateColor { get; set; }
        /// <summary>
        /// 功能关键字，如U01
        /// </summary>
        public string FunctionCode { get; set; }
        /// <summary>
        /// 投递时间
        /// </summary>
        public DateTime DeliveredDateTime { get; set; }

        public DeliveredToJtjEventArgs(string plateNumber, string functionCode, DateTime deliveredDateTime)
        {
            this.PlateNumber = plateNumber;
            this.FunctionCode = functionCode;
            this.DeliveredDateTime = deliveredDateTime;
        }
    }

    public class D04ReceivedFromJtjEventArgs : EventArgs
    {
        /// <summary>
        /// 车牌号码
        /// </summary>
        public string PlateNumber { get; set; }
        /// <summary>
        /// 车牌颜色，如1
        /// </summary>
        public byte PlateColor { get; set; }
        /// <summary>
        /// 投递时间
        /// </summary>
        public DateTime ReceivedDateTime { get; set; }

        public D04ReceivedFromJtjEventArgs(string plateNumber, byte plateColor, DateTime receivedDateTime)
        {
            this.PlateNumber = plateNumber;
            this.PlateColor = plateColor;
            this.ReceivedDateTime = receivedDateTime;
        }
    }

    public delegate void D04ReceivedFromJtjEventHandler(object sender, D04ReceivedFromJtjEventArgs e);
    public delegate void DeliveredToJtjEventHandler(object sender, DeliveredToJtjEventArgs e);

    //0
    public delegate void ConnectedEventHandler();
    public delegate void DisconnectedEventHandler();

    //1
    public delegate void LoggingEventHandler(object sender, Level level, object message);
    public delegate void OtherDataReceivedEventHandler(string RecvData);
    public delegate void MobleInfoReceivedEventHandler(TMobleUnitInfo MobleUnitInfo);
    public delegate void DelegateName(Hashtable m_aryMobiles);

    //2
    public delegate void CommuStatusUpdatedEventHandler(string id, string Statu);
    public delegate void DriverLoggedInEventHandler(string _LoginID, string Logined);
    public delegate void TableResultUpdatedEventHandler(int UpdateResult, string LogStr);
    public delegate void OtherClientDisconnectedEventHandler(int Sequence, string ClientId);
    public delegate void AlarmHistoryResultReceivedEventHandler(string Target, int Total);
    public delegate void GpsSendResultReceivedEventHandler(string Target, int iSendResp);
    public delegate void GpsHistoryResultReceivedEventHandler(string Target, int Total);
    public delegate void GetAlarmHistryDataEventHandler(string Target, TAlarmData AlarmData);
    public delegate void OtherClientInfoReceivedEventHandler(int Sequence, TGpsClientInfo OtherClientInfo);
    public delegate void MdtListUpdatedEventHandler(object sender, MdtWrapper mdt);
    public delegate void GpsLocationUpdatedEventHandler(object sender, MdtWrapper mdt, Db44GpsData gpsData);

    //3
    public delegate void DownloadSoftwareResultReceivedEventHandler(string Target, int ChildCmdType, int Address);
    public delegate void ClientCommandReturnEventHandler(long tempCmd_ID, string R, string LogStr);
    public delegate void PlainGpsDataReceivedEventHandler(string id, byte[] GpsPackByte, MdtWrapper mdt);

    //4
    public delegate void GpsTransmitReceivedEventHandler(string Target, string NewSequence, int ResultCode, string TransmitResult);
    public delegate void GpsDataReturnEventHandler(string id, string ReCmd, MdtWrapper mobileInfo, byte ProtocolType);
    public delegate void LineNodeResultReceivedEventHandler(string Target, int ChildCmdType, int NodeID, int Tag);
    public delegate void OperationReceivedEventHandler(string Target, int OperateType, string OperateContent, string OperateScribe);
    public delegate void MdtDataReceivedEventHandler(byte[] body, int len, MdtWrapper mobileInfo, string _NetParameter);

    //5   
    public delegate void PlainMessageReceivedEventHandler(string id, int Msg_type, string Msg_str, string Msg_Describe, MdtWrapper mobileInfo);
    public delegate void TrafficAccidentRelativeDataReceivedEventHandler(string id, int Msg_type, byte Q, byte[] QT, byte[] QD, MdtWrapper mobileInfo);
    public delegate void LogResultReceivedEventHandler(bool LogResult, int Purview, int Total, string ResultStr, long _Purview);
    public delegate void HandleReplyDirective(string mobileID, byte cmdType, int mobileType, int cycleCode, string[] parameter);
    public delegate void ClientConnectedEventHandler(string IP, string CName, string UserType, string WorkStatu, string UserName);

    //6
    public delegate void OtherOperationReceivedEventHandler(int Sequence, string FormClientId, string Target, string Msg_Type, string Msg_str, string Msg_Describe);
    public delegate void ImageReturnEventHandler(string id, int MointerID, long ImgNo, int BlockNo, string OperationDescribe, string DataID);

    //7+
    public delegate void ImageInfoReceivedEventHandler(string Target, string Mointer, string ImageID, string ImageLen, int ImageSeq, string ImageData, string ImgStatu);
    public delegate void AlarmOperationReceivedEventHandler(string Target, string sOperateContent, string sOperateDescribe, string _dwLatitude, string _dwLongitude, string _nSpeed, string _nDirectionn);
    public delegate void ImageReceivedEventHandler(string id, int MointerID, long ImageID, int ImageLen, int ImageSeq, string ImageBody, string ImgStatu, DateTime FilmDateTime, MdtWrapper mobileInfo);
    public delegate void TaxiOprationInfoReceivedEventHandler(string id, string PayType, string TaxiID, string GetInDateTime, string GetOutDateTime, string WaitTime, string Mileage, string AllroundPrice, string PriceOfGetOut, string FreeMileage);
    public delegate void Eve_RecivGpsData2(string id, ref string v, double Y, double X, double VV, int FF, ref string Stime, ref string ST, ref string UST, ref MdtWrapper mobileInfo, byte DateType);
    public delegate void MileageReceivedEventHandler(string id, double X, double Y, double V, string nDirection_, string Mileages, string Temperature, string OilCapacity, ref MdtWrapper mobileInfo);
    public delegate void GpsAlarmReceivedEventHandler(string id, int Msg_type, string Msg_str, string Msg_Describe, string v, double X, double Y, double V, string nDirection_, string wDate, MdtWrapper mobileInfo);
}
