using Parrot.Models;
using System.Diagnostics;
using System.Text;
using System;
using Parrot;

namespace Parrot.Models.Db44
{
    public class Db44In
    {
        private Db44Out DataOut;

        public Db44In()
        {
            this.DataOut = new Db44Out();
        }

        #region Events
        public event EventHandler<GpsDataReceivedEventArgs> GpsDataReceived;
        private void FireGpsDataReceivedEvent(GpsDataReceivedEventArgs e)
        {
            if (GpsDataReceived != null) GpsDataReceived(this, e);
        }
        public event EventHandler<DriverSignedInOrOutEventArgs> DriverSignedInOrOut;
        private void FireDriverSignedInOrOutEvent(DriverSignedInOrOutEventArgs e)
        {
            if (DriverSignedInOrOut != null) DriverSignedInOrOut(this, e);
        }
        public event EventHandler<PossibleAccidentDataReportingEventArgs> PossibleAccidentDataReporting;
        private void FirePossibleAccidentDataReportingEvent(PossibleAccidentDataReportingEventArgs e)
        {
            if (PossibleAccidentDataReporting != null) PossibleAccidentDataReporting(this, e);
        }
        public event EventHandler<CameraCapturingEventArgs> CameraCapturing;
        private void FireCameraCapturingEvent(CameraCapturingEventArgs e)
        {
            if (CameraCapturing != null) CameraCapturing(this, e);
        }
        #endregion
        
        /// <summary>
        /// 收到报警
        /// </summary>
        public event GpsAlarmReceivedEventHandler Alarm;
        public event GpsDataReturnEventHandler GpsDataReturn;
        public event PlainMessageReceivedEventHandler PlainMessageReceived;

        /// <summary>
        /// 处理流入OMC的数据。
        /// </summary>
        /// <param name="mobileID"></param>
        /// <param name="mdtModel"></param>
        /// <param name="bodyWithFunctionCode"></param>
        /// <param name="ByteLen"></param>
        /// <param name="mdt"></param>
        public void ProcessIncomingData(string mobileID, int mdtModel, byte[] body, MdtWrapper mobileInfo)
        {
            if (body[0] == 0x47)
            {
                //Debug.WriteLine("源始数据 7E" + Util.BytesToHex(body) + " 7F");

                byte[] buffer = Db44ParserHelper.Unpack(body, mdtModel);
                if (buffer != null)
                {
                    this.GprsDataIn_(mobileID, buffer, mdtModel, ref mobileInfo, "");
                }
            }
        }
        private void GprsDataIn_(string mobileID, byte[] body, int mobileType, ref MdtWrapper mobileInfo, string X)
        {
            string str = Util.BytesToHex(body);
            string bodyStr = str.Substring(0x2a, (str.Length - 6) - 0x2a);

            byte protocolNumber = body[20];
            byte num2 = body[2];
            //Debug.WriteLine("解密数据" + Util.BytesToHex(body,true) + "");
            //Debug.WriteLine("解密数据" + protocolNumber.ToString("X2") + "\r\n");

            switch (protocolNumber)
            {

                case 0://自动上传
                case 4://定距监控设置响应
                case 5://定距监控查看参数响应
                    break;

                case 1://位置返回
                    mobileInfo.LastCmdReTurn = "当前位置返回";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "当前位置返回", mobileInfo);
                    }
                    break;

                case 2://定时监控设置响应
                    mobileInfo.LastCmdReTurn = "定时监控设置";
                    if (this.PlainMessageReceived != null)
                    {
                        string[] strArray = new string[] { "时间间隔:", body[0x33].ToString(), "秒，发送条数:", body[0x34].ToString(), "条，发送包数:", ((body[0x35] * 0x100) + body[0x36]).ToString(), "包" };
                        this.PlainMessageReceived(mobileID, 2, "控制回复", string.Concat(strArray), mobileInfo);
                    }
                    break;

                case 3://定时监控查看参数响应
                    mobileInfo.LastCmdReTurn = "定时监控查看";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "时间间隔:" + body[30].ToString() + "秒，发送条数:" + body[0x1f].ToString() + "条，剩余发送包数:" + body[0x20].ToString() + "包", mobileInfo);
                    }
                    break;

                case 6://速度监控设置响应
                    mobileInfo.LastCmdReTurn = "超速报警设置";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "超速报警设置成功", mobileInfo);
                    }
                    break;

                case 7://区域设定（封闭区域）响应
                    mobileInfo.LastCmdReTurn = "区域设定";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "已设置区域:" + body[0x15].ToString() + "个", mobileInfo);
                    }
                    return;

                case 8://查看区域设定（封闭区域）响应
                    mobileInfo.LastCmdReTurn = "查看区域设定";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "已设置区域:" + body[0x15].ToString() + "个----" + bodyStr, mobileInfo);
                    }
                    return;

                case 9://区域监控设置响应
                    mobileInfo.LastCmdReTurn = "区域监控";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "设置区域监控成功，编号:" + body[0x15].ToString(), mobileInfo);
                    }
                    return;

                case 0x0a://区域监控查看响应
                    {
                        string str8 = body[0x16].ToString();
                        string str9 = body[0x17].ToString();
                        string str10 = body[0x18].ToString();
                        string str11 = body[0x19].ToString();
                        string str12 = body[0x1a].ToString();
                        mobileInfo.LastCmdReTurn = "区域监控查看";
                        if (this.PlainMessageReceived != null)
                        {
                            this.PlainMessageReceived(mobileID, 2, "控制回复", "区域监控查看，编号：" + str8 + "限制速度：" + str9 + "持续时间：" + str10 + "禁入禁出：" + str11 + "持续时间：" + str12, mobileInfo);
                        }
                        if (this.GpsDataReturn != null)
                        {
                            this.GpsDataReturn(mobileID, this.DataOut.Order(mobileID, mobileType, new string[] { "F2", str8 }), mobileInfo, 1);
                        }
                        return;
                    }
                case 0x0b://道路设定响应
                    mobileInfo.LastCmdReTurn = "线路设定";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "已设置线路:" + body[0x15].ToString() + "个", mobileInfo);
                    }
                    return;

                case 0x0c://道路设定查看响应
                    mobileInfo.LastCmdReTurn = "查看线路设定";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "已设置线路:" + body[0x15].ToString() + "个----" + bodyStr, mobileInfo);
                    }
                    return;

                case 0x0d://道路监控设置响应
                    mobileInfo.LastCmdReTurn = "道路监控";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "设置道路监控成功，编号" + body[0x15].ToString(), mobileInfo);
                    }
                    if (this.GpsDataReturn != null)
                    {
                        this.GpsDataReturn(mobileID, this.DataOut.Order(mobileID, mobileType, new string[] { "F1", body[0x15].ToString() }), mobileInfo, 1);
                    }
                    return;

                case 0x0e://道路监控查看响应
                    {
                        string str13 = NumberConverter.ConvertForInt32(bodyStr.Substring(0, 2), 0x10, 10);
                        string str14 = body[0x16].ToString();
                        string str15 = body[0x17].ToString();
                        string str16 = body[0x18].ToString();
                        string str17 = ((body[0x19] * 0x100) + body[0x1a]).ToString();
                        string str18 = ((body[0x1b] * 0x100) + body[0x1c]).ToString();
                        mobileInfo.LastCmdReTurn = "道路监控查看";
                        if (this.PlainMessageReceived != null)
                        {
                            this.PlainMessageReceived(mobileID, 2, "控制回复", "道路监控查看，总数:" + str13 + "编号:" + str14 + "限制速度:" + str15 + "禁入禁出:" + str16 + "起止时间:" + str17 + "偏离道路:" + str18, mobileInfo);
                        }
                        if (this.GpsDataReturn != null)
                        {
                            this.GpsDataReturn(mobileID, this.DataOut.Order(mobileID, mobileType, new string[] { "F3", str14 }), mobileInfo, 1);
                        }
                        return;
                    }
                case 0x0f://下达设备自检指令的响应
                    mobileInfo.LastCmdReTurn = "设备自检功能";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "设备自检设置成功", mobileInfo);
                    }
                    break;

                case 0x10://事故疑点数据（上行）
                    {
                        byte packetIndex = body[0x15];
                        DateTime stoppedTime = DateTime.Parse(
                            string.Format(
                            "20{0:X2}-{1:X2}-{2:X2} {3:X2}:{4:X2}:{5:X2}",
                            body[22], body[23], body[24], body[25], body[26], body[27]));
                        byte[] brakeData = new byte[200];
                        Array.Copy(body, 28, brakeData, 0, 200);

                        PossibleAccidentDataReportingEventArgs e = new PossibleAccidentDataReportingEventArgs(mobileInfo.Mobile_VehicleRegistration, mobileInfo.DB44_VehicleRegistrationColor,
                            packetIndex, stoppedTime, brakeData);
                        FirePossibleAccidentDataReportingEvent(e);
                        return;
                    }
                case 0x11://查询历史轨迹的响应
                    if (!((this.PlainMessageReceived != null) & (body[0x12] > 2)))
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "无历史轨迹", mobileInfo);
                        return;
                    }
                    this.EP_Pack(ref body, ref mobileInfo);
                    return;

                case 0x12://查询驾驶员身份的响应
                    {
                        string str2 = body[0x15].ToString("X2") + body[0x16].ToString("X2") + body[0x17].ToString("X2") + body[0x18].ToString("X2");
                        string str3 = body[0x19].ToString("X2") + body[0x1a].ToString("X2");
                        if (this.PlainMessageReceived != null)
                        {
                            this.PlainMessageReceived(mobileID, 2, "查询驾驶员身份", "驾驶员代码:" + str2 + "，驾驶时间:" + str3 + "分钟", mobileInfo);
                        }
                        return;
                    }
                case 0x13://完整卫星定位数据包，打印前上报
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "打印前上报", "设备准备打印", mobileInfo);
                    }
                    break;

                case 0x14://实际连续驾驶时间
                    {
                        string str4 = body[0x15].ToString("X2") + body[0x16].ToString("X2");
                        if (this.PlainMessageReceived != null)
                        {
                            this.PlainMessageReceived(mobileID, 2, "控制回复", "疲劳驾驶设置已完成，实际连续驾驶时间:" + str4 + "分钟", mobileInfo);
                        }
                        return;
                    }
                case 0x15://下发国标汉字信息的响应
                    mobileInfo.LastCmdReTurn = "发送中文信息";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "终端已收到下发的信息", mobileInfo);
                    }
                    break;

                case 0x16://上发国标汉字信息
                    {
                        string str5 = "";
                        str5 = Encoding.Unicode.GetString(body, 0x33, ((body[0x11] * 0x100) + body[0x12]) - 0x33);
                        if (this.PlainMessageReceived != null)
                        {
                            this.PlainMessageReceived(mobileID, 3, "文字信息", str5, mobileInfo);
                        }
                        if (this.GpsDataReturn != null)
                        {
                            this.GpsDataReturn(mobileID, this.DataOut.Order(mobileID, mobileType, new string[] { "65535" }), mobileInfo, 1);
                        }
                        break;
                    }
                case 0x17://紧急报警
                    mobileInfo.LastCmdReTurn = "紧急报警";
                    this.DataOut.cycleCode = num2;
                    if (this.GpsDataReturn != null)
                    {
                        this.GpsDataReturn(mobileID, this.DataOut.Order(mobileID, mobileType, new string[] { "65534" }), mobileInfo, 1);
                    }
                    break;

                case 0x18://解除紧急报警警情的响应
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "紧急报警已解除", mobileInfo);
                    }
                    break;

                case 0x19://远程断油控制的响应
                    {
                        string str6 = ((body[0x33] & 1) == 1) ? "中断油路" : "恢复油路";
                        str6 = str6 + (((body[0x33] & 0x80) == 0x80) ? "成功" : "失败");
                        if (this.PlainMessageReceived != null)
                        {
                            this.PlainMessageReceived(mobileID, 2, "控制回复", str6, mobileInfo);
                        }
                        break;
                    }
                case 0x1a://查看远程参数的响应
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "远程参数查询结果", this.getSetParameterStr(ref body, 0x33), mobileInfo);
                    }
                    break;

                case 0x1b://设置远程参数的响应
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", this.getSetParameterStr(ref body, 0x33), mobileInfo);
                    }
                    break;

                case 0x1c://复位指令的响应
                    mobileInfo.LastCmdReTurn = "复位";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "复位", mobileInfo);
                    }
                    break;

                case 0x1d://MDT返回密钥
                    {
                        string str7 = body[0x33].ToString("X2") + body[0x34].ToString("X2") + body[0x35].ToString("X2") + body[0x36].ToString("X2");
                        if (this.PlainMessageReceived != null)
                        {
                            this.PlainMessageReceived(mobileID, 2, "控制回复", "获取密钥：" + str7, mobileInfo);
                        }
                        break;
                    }

                case 0xe0:
                    mobileInfo.LastCmdReTurn = "UDP握手";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "UDP握手", mobileInfo);
                    }
                    if (this.GpsDataReturn != null)
                    {
                        this.DataOut.cycleCode = num2;
                        this.GpsDataReturn(mobileID, this.DataOut.Order(mobileID, mobileType, new string[] { "65533" }), mobileInfo, 1);
                    }
                    return;

                case 0xe1:
                    {
                        string str19 = bodyStr.Substring(60, 30);
                        if (this.PlainMessageReceived != null)
                        {
                            this.PlainMessageReceived(mobileID, 2, "控制回复", str19 + "请求登录", mobileInfo);
                        }
                        if (this.GpsDataReturn != null)
                        {
                            this.GpsDataReturn(mobileID, this.DataOut.Order(mobileID, mobileType, new string[] { "65532" }), mobileInfo, 1);
                        }
                        break;
                    }
                case 0xe2:
                    {
                        string str20 = "";
                        for (int m = 0; m < 0x12; m++)
                        {
                            str20 = str20 + body[m + 0x15].ToString("X2");
                        }
                        str20 = "驾驶证号:" + str20 + ((body[0x27] == 1) ? "拔卡" : "插卡");
                        if (this.PlainMessageReceived != null)
                        {
                            this.PlainMessageReceived(mobileID, 2, "IC卡插拔卡反馈", str20, mobileInfo);
                        }
                        if (this.GpsDataReturn != null)
                        {
                            this.GpsDataReturn(mobileID, this.DataOut.Order(mobileID, mobileType, new string[] { "65531", body[0x27].ToString() }), mobileInfo, 1);
                        }
                        break;
                    }
                case 0xf0:
                    mobileInfo.LastCmdReTurn = "UDP握手";
                    if (this.PlainMessageReceived != null)
                    {
                        this.PlainMessageReceived(mobileID, 2, "控制回复", "UDP握手", mobileInfo);
                    }
                    if (this.GpsDataReturn != null)
                    {
                        this.DataOut.cycleCode = num2;
                        this.GpsDataReturn(mobileID, this.DataOut.Order(mobileID, mobileType, new string[] { "65536" }), mobileInfo, 1);
                    }
                    return;

                case 0xf7:
                    {
                        string str21 = "";
                        for (int j = 0; j < 0x12; j++)
                        {
                            str21 = str21 + body[j + 0x15].ToString("X2");
                        }
                        str21 = "驾驶证号:" + str21 + ((body[0x27] == 1) ? "拔卡" : "插卡");
                        if (this.PlainMessageReceived != null)
                        {
                            this.PlainMessageReceived(mobileID, 2, "IC卡插拔卡反馈", str21, mobileInfo);
                        }
                        if (this.GpsDataReturn != null)
                        {
                            this.GpsDataReturn(mobileID, this.DataOut.Order(mobileID, mobileType, new string[] { "65530", body[0x27].ToString() }), mobileInfo, 1);
                        }
                        break;
                    }
                default:
                    return;
            }
            this.EP_Pack(ref body, ref mobileInfo);
        }


        private void EP_Pack(ref byte[] body, ref MdtWrapper mobileInfo)
        {
            int num = (body.Length - 0x18) / 30;//多个GPS数据包
            for (int i = 0; i < num; i++)
            {
                try
                {
                    byte[] gpsPackByte = new byte[30];
                    for (int j = 0; j < 30; j++)
                    {
                        gpsPackByte[j] = body[((i * 30) + 0x15) + j];
                    }

                    GpsDataReceivedEventArgs e = new GpsDataReceivedEventArgs(mobileInfo.Mobile_VehicleRegistration, mobileInfo.DB44_VehicleRegistrationColor, new Db44GpsData(gpsPackByte));
                    FireGpsDataReceivedEvent(e);
                }
                catch(Exception ex)
                {
                    ;
                }
            }
        }

        /// <summary>
        /// MDT基本参数。
        /// </summary>
        /// <param name="bodys">bodys[0x33]：远程参数类型编号。</param>
        /// <param name="IndexL"></param>
        /// <returns></returns>
        private string getSetParameterStr(ref byte[] bodys, int IndexL)
        {
            int num = (bodys[0x11] * 0x100) + bodys[0x12];
            IndexL++;
            string str = Encoding.Default.GetString(bodys, IndexL, (num - 30) - 3);
            switch (bodys[0x33])
            {
                case 1:
                    return ("车辆识别代号：" + str.Trim());

                case 2:
                    return ("车牌号码：" + str.Trim(new char[1]));

                case 3:
                    return ("车牌分类：" + str.Trim(new char[1]));

                case 4:
                    return ("驾驶员代码：" + str.Trim(new char[] { '0' }));

                case 5:
                    return ("驾驶证证号：" + str.Trim(new char[1]));

                case 6:
                    return ("MDT主机ID：" + str.Trim(new char[1]));

                case 7:
                    return ("固件版本号：" + str.Trim(new char[1]));

                case 8:
                    return ("初次安装日期：20" + bodys[IndexL].ToString("X2") + "-" + bodys[IndexL + 1].ToString("X2") + "-" + bodys[IndexL + 2].ToString("X2"));

                case 9:
                    return ("初次安装日期：20" + bodys[IndexL].ToString("X2") + "-" + bodys[IndexL + 1].ToString("X2") + "-" + bodys[IndexL + 2].ToString("X2") + " " + bodys[IndexL + 3].ToString("X2") + "：" + bodys[IndexL + 4].ToString("X2") + "：" + bodys[IndexL + 5].ToString("X2"));

                case 10:
                    {
                        string[] strArray3 = new string[] { "UDP地址：", bodys[IndexL].ToString(), ".", bodys[IndexL + 1].ToString(), ".", bodys[IndexL + 2].ToString(), ".", bodys[IndexL + 3].ToString(""), "：", ((bodys[IndexL + 4] * 0x100) + bodys[IndexL + 5]).ToString("X2") };
                        return string.Concat(strArray3);
                    }
                case 11:
                    {
                        string[] strArray4 = new string[] { "TCP地址：", bodys[IndexL].ToString(), ".", bodys[IndexL + 1].ToString(), ".", bodys[IndexL + 2].ToString(), ".", bodys[IndexL + 3].ToString(""), "：", ((bodys[IndexL + 4] * 0x100) + bodys[IndexL + 5]).ToString("X2") };
                        return string.Concat(strArray4);
                    }
                case 12:
                    return ("短消息服务中心号码：" + str.Trim(new char[1]));

                case 13:
                    return ("运营管理中心短消息服务号码一：" + str.Trim(new char[1]));

                case 14:
                    return ("运营管理中心短消息服务号码二：" + str.Trim(new char[1]));
            }
            return "";
        }


        /// <summary>
        /// GPS扩展状态。
        /// </summary>
        /// <param name="mobileID"></param>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="E"></param>
        /// <param name="F"></param>
        /// <param name="G"></param>
        /// <param name="H"></param>
        /// <param name="mdt"></param>
        /// <returns></returns>
        private string GPSStatue(string _ID, byte A, byte B, byte C, byte D, byte E, byte F, byte G, byte H, ref MdtWrapper mobileInfo)
        {
            string str = "";
            bool flag = false;
            string str2 = "";
            string str3 = "";
            if ((A & 4) == 4)
            {
                str2 = str2 + "紧急报警、";
                flag = true;
                str = str + "用户遇劫警或紧急求助、";
                str3 = str3 + "紧急报警、";
            }
            if ((A & 8) == 8)
            {
                str2 = str2 + "已断油电、";
            }
            if ((A & 0x10) == 0x10)
            {
                str2 = str2 + "超速、";
                flag = true;
                str = str + "用户车辆超速行驶中，当前速度：" + mobileInfo.LastV.ToString() + "公里、";
                str3 = str3 + "超速、";
            }
            if ((A & 0x20) == 0x20)
            {
                str2 = str2 + "振动报警、";
                flag = true;
                str = "振动报警、";
                str3 = "安防报警、";
            }
            if ((A & 0x40) == 0x40)
            {
                str2 = str2 + "主电源断开、";
                if (mobileInfo.Power_Tag)
                {
                    flag = true;
                    str = str + "主电源断开、";
                    str3 = str3 + "设备故障、";
                }
            }
            if ((B & 1) == 1)
            {
                str2 = str2 + "刹车、";
            }
            if ((B & 2) == 2)
            {
                str2 = str2 + "开门、";
            }
            if ((B & 4) == 4)
            {
                str2 = str2 + "左转向灯开、";
            }
            else
            {
                str2 = str2 + "左转向灯关、";
            }
            if ((B & 8) == 8)
            {
                str2 = str2 + "右转向灯开、";
            }
            else
            {
                str2 = str2 + "右转向灯关、";
            }
            if ((B & 0x10) == 0x10)
            {
                str2 = str2 + "远光灯开、";
            }
            else
            {
                str2 = str2 + "远光灯关、";
            }
            if ((B & 0x20) == 0x20)
            {
                str2 = str2 + "ACC开、";
            }
            else
            {
                str2 = str2 + "ACC关、";
            }
            if ((C & 2) == 2)
            {
                str2 = "GPS天线短路、";
                if (mobileInfo.Gps_Tag)
                {
                    flag = true;
                    str = "GPS天线短路、";
                    str3 = "设备故障、";
                }
            }
            if ((C & 4) == 4)
            {
                str2 = str2 + "GPS天线开路、";
                if (mobileInfo.Gps_Tag)
                {
                    flag = true;
                    str = str + "GPS天线开路、";
                    str3 = str3 + "设备故障、";
                }
            }
            if ((C & 8) == 8)
            {
                str2 = str2 + "定位模块异常、";
                if (mobileInfo.Gps_Tag)
                {
                    flag = true;
                    str = str + "定位模块异常、";
                    str3 = str3 + "设备故障、";
                }
            }
            if ((C & 0x10) == 0x10)
            {
                str2 = str2 + "通信模块异常、";
                flag = true;
                str = str + "通信模块异常、";
                str3 = str3 + "设备故障、";
            }
            if ((C & 0x20) == 0x20)
            {
                str2 = str2 + "禁止使出区域、";
                flag = true;
                str = str + "禁止使出区域、";
                str3 = str3 + "越界报警、";
            }
            if ((C & 0x40) == 0x40)
            {
                str2 = str2 + "禁止使入区域、";
                flag = true;
                str = str + "禁止使入区域、";
                str3 = str3 + "越界报警";
            }
            if ((D & 1) == 1)
            {
                str2 = str2 + "/备电异常";
                flag = true;
                str = str + "/备电异常";
                str3 = str3 + "设备故障、";
            }
            if ((D & 2) == 2)
            {
                str2 = str2 + "地理栅栏越界、";
                flag = true;
                str = str + "地理栅栏越界、";
                str3 = str3 + "越界报警、";
            }
            if ((D & 4) == 4)
            {
                str2 = str2 + "汽车点火、";
            }
            else
            {
                str2 = str2 + "汽车熄火、";
            }
            str2 = (str2 + "@").Replace("、@", "").Replace("@", "");
            str3 = (str3 + "@").Replace("、@", "").Replace("@", "");
            str = (str + "@").Replace("、@", "").Replace("@", "");
            if (flag)
            {
                this.Alarm(_ID, 1, str3, str, mobileInfo.LastAV, mobileInfo.LastX, mobileInfo.LastY, mobileInfo.LastV, mobileInfo.LastF.ToString(), DateTime.Now.ToString(),  mobileInfo);
            }
            return str2;
        }

    }
}