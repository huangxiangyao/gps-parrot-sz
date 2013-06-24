using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Timers;
using System.Diagnostics;
using Parrot.Models;

namespace Parrot.Models.TianheGprs
{
    /// <summary>
    /// *HQ
    /// </summary>
    public class TianheGprsIn
    {
        internal class PrivateImplementationDetails
        {
            internal static Hashtable hashtable1;
        }

        // Fields
        private int HistryDataId = 0;
        private ImgBody Img_Body;
        private int MsgID;
        private string nDirection = "";
        private byte nMsgType;
        private string nSpeed = "";
        public static Hashtable RecvImage_Hash;
        private byte[] tempbody = new byte[0x20];
        private Timer Timer_Img = new Timer();
        private string wDate = "";
        private string wTime = "";

        // Events
        public event GpsAlarmReceivedEventHandler GpsAlarmReceived;

        public event PlainMessageReceivedEventHandler PlainMessageReceived;

        public event PlainGpsDataReceivedEventHandler PlainGpsDataReceived;

        // Methods
        public TianheGprsIn()
        {
            this.Timer_Img.Elapsed += new ElapsedEventHandler(this.OnTimedEvent_Img);
            this.Timer_Img.Interval = 10000.0;
            this.Timer_Img.Enabled = true;
            RecvImage_Hash = new Hashtable();
        }


        public void _Deleted(string _ID, int _Type, ref byte[] body, int ByteLen, ref MdtWrapper mobileInfo)
        {
            // TODO: ERROR?
        }
        //收到天禾车载机上传的定位数据
        private void _GPSDataIn(string _ID, int _Type, byte[] _Body, ref MdtWrapper mobileInfo)
        {
            string str3;
            string str4;
            string str11;
            //if (<PrivateImplementationDetails>.$$method0x6000035-1 == null)
            if (PrivateImplementationDetails.hashtable1 == null)
            {
                Hashtable hashtable1 = new Hashtable(0x54, 0.5f);
                hashtable1.Add("S17", 0);
                hashtable1.Add("S12", 1);
                hashtable1.Add("S2", 2);
                hashtable1.Add("R7", 3);
                hashtable1.Add("R1", 4);
                hashtable1.Add("S13", 5);
                hashtable1.Add("S14", 6);
                hashtable1.Add("S18", 7);
                hashtable1.Add("S19", 8);
                hashtable1.Add("S20", 9);
                hashtable1.Add("S21", 10);
                hashtable1.Add("S22", 11);
                hashtable1.Add("C1", 12);
                hashtable1.Add("A1", 13);
                hashtable1.Add("R8", 14);
                hashtable1.Add("S23", 15);
                hashtable1.Add("S24", 0x10);
                hashtable1.Add("S25", 0x11);
                hashtable1.Add("S26", 0x12);
                hashtable1.Add("S1", 0x13);
                hashtable1.Add("S5", 20);
                hashtable1.Add("S27", 0x15);
                hashtable1.Add("S28", 0x16);
                hashtable1.Add("S30", 0x17);
                hashtable1.Add("S31", 0x18);
                hashtable1.Add("S32", 0x19);
                hashtable1.Add("S33", 0x1a);
                hashtable1.Add("S34", 0x1b);
                hashtable1.Add("S37", 0x1c);
                hashtable1.Add("S38", 0x1d);
                hashtable1.Add("S35", 30);
                hashtable1.Add("S36", 0x1f);
                hashtable1.Add("I1", 0x20);
                hashtable1.Add("I5", 0x21);
                hashtable1.Add("S15", 0x22);
                hashtable1.Add("S4", 0x23);
                hashtable1.Add("S3", 0x24);
                hashtable1.Add("S39", 0x25);
                hashtable1.Add("S6", 0x26);
                hashtable1.Add("S40", 0x27);
                hashtable1.Add("S9", 40);
                //<PrivateImplementationDetails>.$$method0x6000035-1 = hashtable1;
                PrivateImplementationDetails.hashtable1 = hashtable1;
            }
            string str = Encoding.Default.GetString(_Body, 0, _Body.Length);
            string[] tokens = str.Split(new char[] { ',' });
            string str2 = tokens[2];
            switch (str2)
            {
                case "V1":
                    this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                    return;

                case "I1":
                    {
                        int num = int.Parse(tokens[6]);
                        if (num != 2)
                        {
                            if (num > 2)
                            {
                            }
                        }
                        else
                        {
                            if (tokens[7] == "18#")
                            {
                                tokens[7] = "OK";
                            }
                            if (tokens[7] == "11#")
                            {
                                tokens[7] = "CLR";
                            }
                            FirePlainMessageReceived(_ID, 2, "控制回复", "用户按下了操作按键：" + tokens[7], mobileInfo);
                        }
                        Debug.WriteLine("I1:----------" + _Body);
                        return;
                    }
                case "I2":
                    Debug.WriteLine("I2:----------" + _Body);
                    return;

                case "I3":
                    Debug.WriteLine("I3:----------" + _Body);
                    return;

                case "I4":
                    {
                        int num2 = int.Parse(tokens[6]);
                        return;
                    }
                case "I5":
                    Debug.WriteLine("I5:----------" + _Body);
                    return;

                case "I6":
                    Debug.WriteLine("I6:----------" + _Body);
                    return;

                case "I7":
                    Debug.WriteLine("I7:----------" + _Body);
                    return;

                case "I8":
                    return;

                case "V5":
                    this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                    this.RoadCheck(_ID, tokens[3], tokens[4], tokens[5], ref mobileInfo);
                    return;

                case "V6":
                    this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                    this.GpsAlarmReceived(_ID, 1, "行驶状态", "驾驶员疲劳驾驶中", mobileInfo.LastAV, mobileInfo.LastX, mobileInfo.LastY, mobileInfo.LastV, mobileInfo.LastF.ToString(), DateTime.Now.ToString(),  mobileInfo);
                    return;

                case "V4":
                    object obj3;
                    str2 = tokens[3];
                    str3 = "";
                    //if (((obj3 = str2) != null) && ((obj3 = <PrivateImplementationDetails>.$$method0x6000035-1[obj3]) != null))
                    if ((obj3 = str2) != null && ((obj3 = PrivateImplementationDetails.hashtable1[obj3]) != null))
                    {
                        switch (((int)obj3))
                        {
                            case 0:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "自动监控设置命已完成";
                                goto Label_187C;

                            case 1:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "设置自动监控开关已完成";
                                goto Label_187C;

                            case 2:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "设置监控中心短信号码已完成";
                                goto Label_187C;

                            case 3:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "清除报警指令已完成";
                                goto Label_187C;

                            case 4:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "冷启动指令已完成";
                                goto Label_187C;

                            case 5:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "设置通话设置指令已完成";
                                goto Label_187C;

                            case 6:
                                {
                                    this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                    string[] strArray2 = new string[] { "设置速度限制已完成；最高限速：", (int.Parse(tokens[4]) * 1.852).ToString("0"), "；最低限速：", (int.Parse(tokens[5]) * 1.852).ToString("0"), "；报警检测时间：", tokens[7], "秒" };
                                    str3 = string.Concat(strArray2);
                                    goto Label_187C;
                                }
                            case 7:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                switch (tokens[5])
                                {
                                    case "0":
                                        str3 = "虚拟设置，用于查询现有设置";
                                        goto Label_0892;

                                    case "9":
                                        str3 = "已取消越界报警，改为防爆功能";
                                        goto Label_0892;

                                    case "1":
                                        str3 = "取消防爆功能，改为越界报警";
                                        break;

                                    case "2":
                                        str3 = "取消防爆功能，改为越界报警";
                                        break;
                                }
                                goto Label_0892;

                            case 8:
                                str3 = "设置自定义报警已完成：" + tokens[6] + "-" + tokens[7] + "-" + tokens[8] + "-" + tokens[9] + "；" + tokens[10] + "-" + tokens[11] + "-" + tokens[12] + "-" + tokens[13] + "；" + tokens[14] + "-" + tokens[15] + "-" + tokens[0x10] + "-" + tokens[0x11] + "；" + tokens[0x12] + "-" + tokens[0x13] + "-" + tokens[20] + "-" + tokens[0x15] + "；" + tokens[0x16] + "-" + tokens[0x17] + "-" + tokens[0x18] + "-" + tokens[0x19] + "；" + tokens[0x1a] + "-" + tokens[0x1b] + "-" + tokens[0x1c] + "-" + tokens[0x1d] + "；" + tokens[30] + "-" + tokens[0x1f] + "-" + tokens[0x20] + "-" + tokens[0x21] + "；" + tokens[0x22] + "-" + tokens[0x23] + "-" + tokens[0x24] + "-" + tokens[0x25].Substring(0, tokens[0x25].Length - 1);
                                goto Label_187C;

                            case 9:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                if (!(tokens[4] == "ERROR"))
                                {
                                    if (tokens[4] == "DONE")
                                    {
                                        str3 = "启动断油电已完成";
                                    }
                                    else if (tokens[4] == "OK")
                                    {
                                        str3 = "已恢复油电的供应";
                                    }
                                }
                                else
                                {
                                    str3 = "启动断油电失败";
                                }
                                goto Label_187C;

                            case 10:
                                {
                                    str4 = "";
                                    if (!(tokens[4] == "0"))
                                    {
                                        str4 = "第" + tokens[4] + "号";
                                    }
                                    else
                                    {
                                        str4 = "所有的";
                                    }
                                    string str10 = tokens[5];
                                    if (str10 == null)
                                    {
                                        goto Label_0CDA;
                                    }
                                    str10 = string.IsInterned(str10);
                                    if (str10 == "0")
                                    {
                                        str3 = "被禁止";
                                        goto Label_0CDA;
                                    }
                                    if (str10 == "1")
                                    {
                                        str3 = "在GPS定位数据有效时启用，禁止车辆驶出围栏";
                                    }
                                    else if (str10 == "2")
                                    {
                                        str3 = "不论GPS数据是否有效都启用，禁止车辆驶出围栏";
                                    }
                                    else if (str10 == "3")
                                    {
                                        str3 = "在GPS定位数据有效时启用，禁止车辆驶入围栏";
                                    }
                                    else
                                    {
                                        if (str10 != "4")
                                        {
                                            if (str10 == "5")
                                            {
                                                goto Label_0C53;
                                            }
                                            goto Label_0CDA;
                                        }
                                        str3 = "不论GPS数据是否有效都启用（A或V），禁止车辆驶入围栏；";
                                    }
                                    goto Label_0C53;
                                }
                            case 11:
                                str3 = "点名指令已完成";
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                goto Label_187C;

                            case 12:
                                str3 = "呼叫命令已完成";
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                goto Label_187C;

                            case 13:
                                str3 = "确认命令(报警确认)已完成";
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                goto Label_187C;

                            case 14:
                                str3 = "监听命令R8已完成";
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                goto Label_187C;

                            case 15:
                                str3 = "设置监控中心GPRS服务器IP地址、监听端口号、报警设置S23已完成";
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                goto Label_187C;

                            case 0x10:
                                str3 = "已设置接入点名称APN为：" + tokens[4];
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                goto Label_187C;

                            case 0x11:
                                str3 = "恢复出厂设置已完成";
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                goto Label_187C;

                            case 0x12:
                                {
                                    string[] strArray5 = new string[] { "分组号：", tokens[8], "；自动监控间隔时间：", tokens[7], "；速度上限：", (int.Parse(tokens[9]) * 1.852).ToString("0"), "；速度下限：", (int.Parse(tokens[10]) * 1.852).ToString("0"), "；限速报警预设持续时间：", tokens[11], "；越界报警预设持续时间：", tokens[12], "；", this.Control_stat(tokens[13]), "；打开的用户自定义报警：", this.User_def_flag(tokens[14]) };
                                    str3 = string.Concat(strArray5);
                                    goto Label_187C;
                                }
                            case 0x13:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "点名回应";
                                goto Label_187C;

                            case 20:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "设置条件打入、打出电话号段S5已完成。控制状态：" + this.Control_stat(tokens[4]) + "；存储位置：" + tokens[5] + "；号码：" + tokens[6];
                                goto Label_187C;

                            case 0x15:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "总发送字节数：" + Convert.ToInt64(tokens[4], 0x10).ToString() + "；UDP发送字节数：" + Convert.ToInt64(tokens[5], 0x10).ToString() + "；总接收字节数：" + Convert.ToInt64(tokens[6], 0x10).ToString();
                                goto Label_187C;

                            case 0x16:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "设置辅助(下行)监控中心短信号码S28已完成";
                                goto Label_187C;

                            case 0x17:
                                str3 = "增加设置自定义信息S30已完成：" + tokens[6] + "-" + tokens[7] + "-" + tokens[8] + "-" + tokens[9] + "；" + tokens[10] + "-" + tokens[11] + "-" + tokens[12] + "-" + tokens[13] + "；" + tokens[14] + "-" + tokens[15] + "-" + tokens[0x10] + "-" + tokens[0x11] + "；" + tokens[0x12] + "-" + tokens[0x13] + "-" + tokens[20] + "-" + tokens[0x15] + "；" + tokens[0x16] + "-" + tokens[0x17] + "-" + tokens[0x18] + "-" + tokens[0x19] + "；" + tokens[0x1a] + "-" + tokens[0x1b] + "-" + tokens[0x1c] + "-" + tokens[0x1d] + "；" + tokens[30] + "-" + tokens[0x1f] + "-" + tokens[0x20] + "-" + tokens[0x21] + "；" + tokens[0x22] + "-" + tokens[0x23] + "-" + tokens[0x24] + "-" + tokens[0x25].Substring(0, tokens[0x25].Length - 1);
                                goto Label_187C;

                            case 0x18:
                                if (!(tokens[4] == "ERROR"))
                                {
                                    str3 = "设置温度报警已完成，高温门限：T" + tokens[6] + "，低温门限：T" + tokens[7] + "，当前检测温度：" + tokens[0x11].Replace("#", "");
                                }
                                else
                                {
                                    str3 = "设置温度报警S31失败，因为传感器没有被安装";
                                }
                                goto Label_187C;

                            case 0x19:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "当前终端行驶里程=" + ((double.Parse(tokens[4]) * 0.51444)).ToString("0.00") + "m";
                                goto Label_187C;

                            case 0x1a:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "设置车上超速提示报警命令已完成，高速门限：" + ((int.Parse(tokens[4]) * 1.852)).ToString("0");
                                goto Label_187C;

                            case 0x1b:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "终端上传数据模式已更改";
                                goto Label_187C;

                            case 0x1c:
                                this.MsgID++;
                                if (!(tokens[4] != "0"))
                                {
                                    if (tokens[4] != "1")
                                    {
                                        string str5 = Convert.ToBase64String(_Body, 0x18, 0x100);
                                        FirePlainMessageReceived(_ID, 100, "已读出道路检测数据", str5, mobileInfo);
                                        byte[] buffer = Convert.FromBase64String(str5);
                                    }
                                }
                                else
                                {
                                    str3 = "对道路检测数据操作已完成";
                                }
                                goto Label_187C;

                            case 0x1d:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "设置道路参数：道路号=" + tokens[4] + "、偏离报警时间=" + tokens[5] + "秒、道路检测超速报警时间" + tokens[6] + "秒、通过时间大于" + tokens[8] + "秒且小于" + tokens[7] + "秒";
                                goto Label_187C;

                            case 30:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "查询或设置行车记录仪记录间隔已完成：记录间隔=" + tokens[4] + "秒、当前记录指针=" + tokens[5] + "、总记录数=" + tokens[6] + "条";
                                goto Label_187C;

                            case 0x1f:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "终端正在准备向中心传送黑匣子数据";
                                return;

                            case 0x20:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "用户已收到交互信息";
                                goto Label_187C;

                            case 0x21:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "用户已收到透传信息";
                                goto Label_187C;

                            case 0x22:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "传输参数设置已完成：" + this.vehicle_S_Key_S17_Stat_S17(_ID, tokens[4], tokens[5], tokens[6]);
                                goto Label_187C;

                            case 0x23:
                                {
                                    this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                    byte num3 = Convert.ToByte(tokens[4], 0x10);
                                    if ((num3 & 0x80) != 0x80)
                                    {
                                        str3 = str3 + "打开安防报警控制、";
                                    }
                                    else
                                    {
                                        str3 = str3 + "关闭安防报警控制、";
                                    }
                                    if ((num3 & 0x40) == 0x40)
                                    {
                                        str3 = str3 + "打开短信发送、";
                                    }
                                    else
                                    {
                                        str3 = str3 + "关闭短信发送的物理通道、";
                                    }
                                    if ((num3 & 0x20) == 0x20)
                                    {
                                        str3 = str3 + "、关闭GPRS传输短信";
                                    }
                                    else
                                    {
                                        str3 = str3 + "打开GPRS传输短信、";
                                    }
                                    if ((num3 & 0x10) == 0x10)
                                    {
                                        str3 = str3 + "关闭GPS信息输出、";
                                    }
                                    else
                                    {
                                        str3 = str3 + "打开GPS信息输出、";
                                    }
                                    if ((num3 & 8) == 8)
                                    {
                                        str3 = str3 + "关闭GPS休眠控制、";
                                    }
                                    else
                                    {
                                        str3 = str3 + "打开GPS休眠控制、";
                                    }
                                    goto Label_187C;
                                }
                            case 0x24:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "设置临时热线电话号码已完成：" + tokens[4];
                                goto Label_187C;

                            case 0x25:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "终端已收到回传图片请求";
                                goto Label_187C;

                            case 0x26:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                if (!(tokens[4] == "ERROR"))
                                {
                                    if (tokens[4] == "DONE")
                                    {
                                        str3 = "终端已完成对中控锁的操作";
                                    }
                                }
                                else
                                {
                                    str3 = "终端对中控锁操作不成功";
                                }
                                goto Label_187C;

                            case 0x27:
                                this.EP_Pack_GSM_New(_ID, tokens, _Type, 8, ref mobileInfo);
                                str3 = "终端已完成对疲劳驾驶监控功能的设置";
                                goto Label_187C;

                            case 40:
                                {
                                    str3 = "第" + tokens[4] + "号码设置";
                                    int num4 = tokens[5].Length / 2;
                                    byte[] bytes = new byte[num4];
                                    for (int i = 0; i < num4; i++)
                                    {
                                        string str6 = tokens[5].Substring(i * 2, 2);
                                        bytes[i] = Convert.ToByte(str6, 0x10);
                                    }
                                    str3 = str3 + "车辆标识：" + Encoding.BigEndianUnicode.GetString(bytes);
                                    byte[] buffer3 = new byte[3];
                                    for (int j = 0; j < 3; j++)
                                    {
                                        string str7 = tokens[6].Substring(j * 2, 2);
                                        buffer3[j] = Convert.ToByte(str7, 0x10);
                                    }
                                    string str8 = "";
                                    if ((buffer3[0] & 0x80) == 0)
                                    {
                                        str8 = "温度报警、";
                                    }
                                    if ((buffer3[0] & 0x40) == 0)
                                    {
                                        str8 = str8 + "紧急报警、";
                                    }
                                    if ((buffer3[0] & 0x20) == 0)
                                    {
                                        str8 = str8 + "电瓶拆除报警、";
                                    }
                                    if ((buffer3[0] & 0x10) == 0)
                                    {
                                        str8 = str8 + "密码错误报警、";
                                    }
                                    if ((buffer3[0] & 8) == 0)
                                    {
                                        str8 = str8 + "盗警、";
                                    }
                                    if ((buffer3[0] & 4) == 0)
                                    {
                                        str8 = str8 + "GPS故障报警、";
                                    }
                                    if ((buffer3[0] & 2) == 0)
                                    {
                                        str8 = str8 + "GPS天线短路报警、";
                                    }
                                    if ((buffer3[0] & 1) == 0)
                                    {
                                        str8 = str8 + "GPS天线开路报警、";
                                    }
                                    if ((buffer3[1] & 0x80) == 0)
                                    {
                                        str8 = str8 + "非法点火报警、";
                                    }
                                    if ((buffer3[1] & 0x40) == 0)
                                    {
                                        str8 = str8 + "超速报警、";
                                    }
                                    if ((buffer3[1] & 0x20) == 0)
                                    {
                                        str8 = str8 + "禁止驶出越界报警、";
                                    }
                                    if ((buffer3[1] & 0x10) == 0)
                                    {
                                        str8 = str8 + "禁止驶入越界报警、";
                                    }
                                    if ((buffer3[1] & 8) == 0)
                                    {
                                        str8 = str8 + "GPRS阻塞报警、";
                                    }
                                    if ((buffer3[1] & 4) == 0)
                                    {
                                        str8 = str8 + "电压异常报警、";
                                    }
                                    if ((buffer3[1] & 2) == 0)
                                    {
                                        str8 = str8 + "自定义报警";
                                    }
                                    str3 = str3 + "打开的自动报警：" + str8;
                                    goto Label_187C;
                                }
                        }
                    }
                    Debug.WriteLine("A收到不能识别数据：" + str);
                    return;
            }
            Debug.WriteLine("B收到不能识别数据：" + str);
            return;
        Label_0892:
            str3 = "已设置越界报警时间为：" + tokens[4] + "，功能被设定为：" + str3;
            goto Label_187C;
        Label_0C53:
            str11 = str3;
            str3 = str11 + "围栏定义：" + tokens[8] + "、" + tokens[9] + "、" + tokens[10] + "、" + tokens[11] + "、" + tokens[12] + "、" + tokens[13];
        Label_0CDA:
            str3 = str4 + "电子围栏设置已完成，" + str3;
        Label_187C:
            this.MsgID++;
            FirePlainMessageReceived(_ID, 2, "控制回复", str3, mobileInfo);
        }

        private void AutoReciveImage(bool _Now)
        {
        }

        private string Control_stat(string x)
        {
            byte num = Convert.ToByte(x, 0x10);
            string str = "";
            if ((num & 1) == 1)
            {
                str = "禁止打入电话、";
            }
            else
            {
                str = "允许打入电话、";
            }
            if ((num & 2) == 2)
            {
                str = str + "禁止打出电话、";
            }
            else
            {
                str = str + "允许打出电话、";
            }
            if ((num & 4) == 0)
            {
                str = str + "速度限制是否定位都有效、";
            }
            else
            {
                str = str + "速度限制只在定位时有效、";
            }
            if ((num & 8) == 8)
            {
                str = str + "越界报警触发自动监控、";
            }
            else
            {
                str = str + "越界报警不触发自动监控、";
            }
            if ((num & 0x10) == 0x10)
            {
                str = str + "静态断油电、";
            }
            else
            {
                str = str + "动态断油电、";
            }
            if ((num & 0x20) == 0x20)
            {
                str = str + "打开条件打入打出";
            }
            else
            {
                str = str + "关闭条件打入打出";
            }
            if ((num & 0x40) == 0x40)
            {
                return (str + "打开GPS信息输出");
            }
            return (str + "关闭GPS信息输出");
        }
        /// <summary>
        /// 处理天禾车载机的上行数据
        /// </summary>
        /// <param name="mobileID"></param>
        /// <param name="mobileType"></param>
        /// <param name="body"></param>
        /// <param name="mobileInfo"></param>
        public void ProcessIncomingData(string mobileID, int mobileType, byte[] body, MdtWrapper mobileInfo)
        {
            try
            {
                this.MsgID++;
                if (this.MsgID > 0xffff)
                {
                    this.MsgID = 0;
                }
                this.nMsgType = body[0];
                switch (this.nMsgType)
                {
                    case 0x58://'X'
                        this.EP_Pack(mobileID, mobileType, body, 7, mobileInfo);
                        return;

                    case 0x59://'Y'
                        this.HistryDataId++;
                        this.EP_Pack(mobileID, mobileType, body, 1, mobileInfo);
                        return;

                    case 0x50://'P'
                        if (body.Length == 0x220)
                        {
                            string str = "";
                            for (int i = 0; i < 0x220; i++)
                            {
                                str = str + " " + body[i].ToString("X2");
                            }
                            Debug.WriteLine("ImageBody:--------------" + str);
                            this.MsgID++;
                            int num2 = body[0x10] / 0x80;
                            int num4 = body[0x1f];
                            Debug.WriteLine(string.Concat(new object[] { DateTime.Now.ToString(), "接收到图片数据-------- MointerID=", num2, " BlockNo=", num4 }));
                            int num6 = num4 + 1;
                            FirePlainMessageReceived(mobileID, 120, "图片传输状态", "接收到第：" + num6.ToString() + "包数据", mobileInfo);
                            if (RecvImage_Hash.ContainsKey(mobileID))
                            {
                                this.Img_Body = (ImgBody)RecvImage_Hash[mobileID];
                                this.Img_Body.LastReciveTime = DateTime.Now;
                                this.Img_Body.AddNewImgPack(num4, Convert.ToBase64String(body, 0x20, 0x200));
                                Debug.WriteLine("--------添加图片数据：" + num4);
                                if ((((body[0x21f] == 0xff) & (body[0x21e] == 0xff)) & (body[0x21d] == 0xff)) & (body[540] == 0xff))
                                {
                                    object[] objArray2 = new object[] { "--------收到了图片的最后一个：", num4, ",总包数据：", (num4 + 1).ToString() };
                                    Debug.WriteLine(string.Concat(objArray2));
                                    this.Img_Body.PackLen = num4 + 1;
                                    this.Img_Body.ReciveEnd = true;
                                }
                            }
                            else
                            {
                                ImgBody body2 = new ImgBody();
                                body2.NewImg();
                                body2.VcdID = num2;
                                body2.CarID = mobileID;
                                body2.ImgNo = DateTime.UtcNow.Ticks.ToString();
                                body2.AddNewImgPack(num4, Convert.ToBase64String(body, 0x20, 0x200));
                                RecvImage_Hash.Add(mobileID, body2);
                            }
                        }
                        return;

                    case 0x24://'$'
                        break;

                    case 0x2a://'*'
                        this._GPSDataIn(mobileID, mobileType, body, ref mobileInfo);
                        return;

                    default:
                        return;
                }
                this.EP_Pack(mobileID, mobileType, body, 1, mobileInfo);
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Error:----------" + exception.ToString());
            }
        }

        private int EP_Pack(string _ID, int _Type, byte[] Body, int DataType, MdtWrapper mobileInfo)
        {
            int index = 0;
            int num2 = 0;
            for (int i = 0; i < (Body.Length / 0x20); i++)
            {
                if (Body[index] == 0x24)//$
                {
                    num2 = 8;
                }
                else if (Body[index] == 0x59)//Y
                {
                    num2 = 9;
                }
                else if (Body[index] == 0x58)//X
                {
                    num2 = 7;
                }
                else
                {
                    Debug.WriteLine("-------------" + Body[index].ToString("00"));
                }
                byte[] array = new byte[30];
                this.wDate = "20" + Body[index + 11].ToString("X2") + "-" + Body[index + 10].ToString("X2") + "-" + Body[index + 9].ToString("X2");
                this.wTime = Body[index + 6].ToString("X2") + ":" + Body[index + 7].ToString("X2") + ":" + Body[index + 8].ToString("X2");
                this.wDate = this.wDate + " " + this.wTime;
                try
                {
                    this.wDate = DateTime.Parse(this.wDate).AddHours(8.0).ToString("yy:MM:dd:HH:mm:ss");
                }
                catch
                {
                    this.wDate = DateTime.Now.ToString("yy:MM:dd:HH:mm:ss");
                }
                string[] strArray = this.wDate.Split(new char[] { ':' });
                //时间
                array[0] = Convert.ToByte(strArray[0], 0x10);
                array[1] = Convert.ToByte(strArray[1], 0x10);
                array[2] = Convert.ToByte(strArray[2], 0x10);
                array[3] = Convert.ToByte(strArray[3], 0x10);
                array[4] = Convert.ToByte(strArray[4], 0x10);
                array[5] = Convert.ToByte(strArray[5], 0x10);
                //经度
                array[6] = Body[index + 0x11];
                array[7] = Body[index + 0x12];
                array[8] = Body[index + 0x13];
                array[9] = Body[index + 0x14];
                //纬度
                array[10] = (byte)(Body[index + 0x0c]>>4);//0x22128745 -> (DB44)0x02212874
                array[11] = (byte)(((Body[index + 0x0c] & 0x0f)<<4)|(Body[index+0x0d] >>4));
                array[12] = (byte)(((Body[index + 0x0d] & 0x0f) <<4)|(Body[index+0x0e]>>4));
                array[13] = (byte)(((Body[index + 0x0e] & 0x0f) <<4)|(Body[index+0x0f]>>4));
                //速度
                this.nSpeed = (double.Parse(Body[index + 0x16].ToString("X2") + Body[index + 0x17].ToString("X2").Substring(0, 1)) * 1.852).ToString("0");
                array[14] = byte.Parse(this.nSpeed);
                //方向
                this.nDirection = Body[index + 0x17].ToString("X2").Substring(1, 1) + Body[index + 0x18].ToString("X2");
                int num4 = int.Parse(this.nDirection);
                array[15] = (byte)(num4 / 2);
                //高度
                array[16] = 0;
                array[17] = 0;
                //里程
                array[18] = 0;
                array[19] = 0;
                array[20] = 0;
                array[21] = 0;
                if (num2 == 7)
                {
                    mobileInfo.IsXMode = 1;
                    double num5 = (double.Parse(Body[1].ToString("X2") + Body[2].ToString("X2") + Body[3].ToString("X2") + Body[4].ToString("X2") + Body[5].ToString("X2")) * 0.51444) / 100.0;
                    string str2 = num5.ToString("0").PadLeft(8, '0');
                    for (int j = 0; j < 4; j++)
                    {
                        array[18 + j] = Convert.ToByte(str2.Substring(j * 2, 2), 0x10);
                    }
                }
                //状态
                string statusStr = "";
                byte[] buffer2 = this.vehicle_status(_ID, _Type, Body[index + 25], Body[index + 26], Body[index + 27], Body[index + 28], out statusStr, ref mobileInfo);
                if ((Body[index + 21] & 2) == 2)
                {
                    buffer2[2] = (byte)(buffer2[2] + 1);
                }
                buffer2[0] = (byte)((buffer2[0] + 1) + 2);
                buffer2.CopyTo(array, 22);

                //Db44VehicleState vehicleState = ConvertFromTHVehicleState(Body[index + 25], Body[index + 26], Body[index + 27], Body[index + 28],
                //    //  Body[index+0x10]=='A', Body[index + 0x10] == 'N', Body[index + 0x10] == 'E');
                //false, true, true);
                //vehicleState.Data.CopyTo(array,22);

                if (this.PlainGpsDataReceived != null)
                {
                    this.PlainGpsDataReceived(_ID,  array,   mobileInfo);
                }
            }
            return 1;
        }

        private int EP_Pack_GSM_New(string _ID, string[] tokens, int _Type, int T, ref MdtWrapper mobileInfo)
        {
            return 0;
        }

        private void OnTimedEvent_Img(object source, ElapsedEventArgs e)
        {
        }

        private void RoadCheck(string CarId, string RoadCheckStr, string CheckPoint, string Station, ref MdtWrapper mobileInfo)
        {
            RoadCheckStr = Convert.ToString(Convert.ToInt32(RoadCheckStr, 0x10), 2).PadLeft(8, '0');
            bool flag = false;
            string str = "";
            for (int i = 0; i < 8; i++)
            {
                int num2 = int.Parse(RoadCheckStr.Substring(i, 1));
                switch (i)
                {
                    case 0:
                        if (num2 == 0)
                        {
                            flag = true;
                            str = @"道路偏离报警\";
                        }
                        break;

                    case 1:
                        if (num2 == 0)
                        {
                            flag = true;
                            str = str + @"道路超速报警\";
                        }
                        break;

                    case 2:
                        if (num2 == 0)
                        {
                            flag = true;
                            str = str + @"通过时间超过上限报警\";
                        }
                        break;

                    case 3:
                        if (num2 == 0)
                        {
                            flag = true;
                            str = str + @"通过时间低于下限报警\";
                        }
                        break;
                }
            }
            string str2 = "";
            string str3 = Convert.ToByte(RoadCheckStr.Substring(4, 4), 2).ToString();
            if (flag)
            {
                this.MsgID++;
                str2 = "在第" + str3 + "条道路的检测点：" + CheckPoint + "发生了" + str;
                FirePlainMessageReceived(CarId, 1, "道路检测", str2, mobileInfo);
            }
            else
            {
                str2 = "进入第" + str3 + "条道路的检测点：" + CheckPoint;
                FirePlainMessageReceived(CarId, 0x67, "道路检测", str2, mobileInfo);
            }
        }

        private void FirePlainMessageReceived(string id, int Msg_type, string Msg_str, string Msg_Describe, MdtWrapper mobileInfo)
        {
            if (PlainMessageReceived != null)
            {
                PlainMessageReceived(id, Msg_type, Msg_str, Msg_Describe, mobileInfo);
            }
        }

        private string User_Alarm_flag(string _ID, byte A, bool SendAlarm, ref MdtWrapper mobileInfo)
        {
            bool flag = false;
            string str = "自定义状态：";
            if ((A & 1) == 0)
            {
                flag = true;
                str = "车门开、";
            }
            if ((A & 2) == 0)
            {
                flag = true;
                str = str + "发动机运转、";
            }
            if ((A & 4) == 0)
            {
                flag = true;
                str = str + "ACC开、";
            }
            if ((A & 8) == 0)
            {
                flag = true;
                str = str + "主机由后备电池供电、";
            }
            if ((A & 0x10) == 0)
            {
                flag = true;
                if ((mobileInfo.Mobile_Sensor_H1_H != null) & (mobileInfo.Mobile_Sensor_H1_H != ""))
                {
                    str = str + mobileInfo.Mobile_Sensor_H1_H + "、";
                }
            }
            if (((A & 0x20) == 0) && ((mobileInfo.Mobile_Sensor_H2_H != null) & (mobileInfo.Mobile_Sensor_H2_H != "")))
            {
                flag = true;
                str = str + mobileInfo.Mobile_Sensor_H2_H + "、";
            }
            if (((A & 0x40) == 0) && ((mobileInfo.Mobile_Sensor_L1_L != null) & (mobileInfo.Mobile_Sensor_L1_L != "")))
            {
                flag = true;
                str = str + mobileInfo.Mobile_Sensor_L1_L + "、";
            }
            if (((A & 0x80) == 0) && ((mobileInfo.Mobile_Sensor_L2_L != null) & (mobileInfo.Mobile_Sensor_L2_L != "")))
            {
                flag = true;
                str = str + mobileInfo.Mobile_Sensor_L2_L + "、";
            }
            str = (str + "@").Replace("、@", "").Replace("@", "");
            if (flag & SendAlarm)
            {
                this.MsgID++;
                if (this.GpsAlarmReceived != null)
                {
                    this.GpsAlarmReceived(_ID, 1, "自定义报警", str, mobileInfo.LastAV, mobileInfo.LastX, mobileInfo.LastY, mobileInfo.LastV, mobileInfo.LastF.ToString(), DateTime.Now.ToString(),  mobileInfo);
                }
            }
            if (SendAlarm)
            {
                return str;
            }
            return "";
        }

        private string User_def_flag(string x)
        {
            byte num = Convert.ToByte(x, 0x10);
            string str = "";
            if ((num & 1) == 0)
            {
                str = @"A1\";
            }
            if ((num & 2) == 0)
            {
                str = str + @"A2\";
            }
            if ((num & 4) == 0)
            {
                str = str + @"A3\";
            }
            if ((num & 8) == 0)
            {
                str = str + @"A4\";
            }
            if ((num & 0x10) == 0)
            {
                str = str + @"A5\";
            }
            if ((num & 0x20) == 0)
            {
                str = str + @"A6\";
            }
            if ((num & 0x40) == 0)
            {
                str = str + @"A7\";
            }
            if ((num & 0x80) == 0)
            {
                str = str + @"A8\";
            }
            return str;
        }

        private string vehicle_S_Key_S17_Stat_S17(string _ID, string S, string Key_S17, string Stat_S17)
        {
            try
            {
                string str = "";
                string str2 = Convert.ToString(Convert.ToInt32(S, 0x10), 2).PadLeft(8, '0');
                if (str2.Substring(7, 1) == "1")
                {
                    str = str + "压缩传输方式、";
                }
                else
                {
                    str = str + "正常传输方式、";
                }
                if (str2.Substring(6, 1) == "1")
                {
                    str = str + "自定义报警产生后报警1次、";
                }
                else
                {
                    str = str + "自定义报警产生后报警3次、";
                }
                str2 = Convert.ToString(Convert.ToInt32(Key_S17, 0x10), 2).PadLeft(8, '0');
                for (int i = 0; i < 8; i++)
                {
                    int num2 = int.Parse(str2.Substring(7 - i, 1));
                    switch (i)
                    {
                        case 0:
                            {
                                if (num2 != 0)
                                {
                                    break;
                                }
                                str = str + "自动上传与ACC无关、";
                                continue;
                            }
                        case 1:
                            {
                                if (num2 != 0)
                                {
                                    goto Label_0109;
                                }
                                str = str + "高电平传感器1控制位，与传感器无关、";
                                continue;
                            }
                        case 2:
                            {
                                if (num2 != 0)
                                {
                                    goto Label_012B;
                                }
                                str = str + "发动机控制位，与发动机无关、";
                                continue;
                            }
                        case 3:
                            {
                                if (num2 != 0)
                                {
                                    goto Label_014D;
                                }
                                str = str + "高电平传感器2控制位，与传感器无关、";
                                continue;
                            }
                        case 4:
                            {
                                if (num2 != 0)
                                {
                                    goto Label_016C;
                                }
                                str = str + "低电平传感器1控制位，与传感器无关、";
                                continue;
                            }
                        case 5:
                            {
                                if (num2 != 0)
                                {
                                    goto Label_018B;
                                }
                                str = str + "低电平传感器2控制位，与传感器无关、";
                                continue;
                            }
                        case 6:
                            {
                                if (num2 != 0)
                                {
                                    goto Label_01AA;
                                }
                                str = str + "门开关控制位，与门开关无关、";
                                continue;
                            }
                        case 7:
                            {
                                if (num2 != 0)
                                {
                                    goto Label_01C9;
                                }
                                str = str + "后备电源控制位，与后备电源无关、";
                                continue;
                            }
                        default:
                            {
                                continue;
                            }
                    }
                    str = str + "自动上传受控于ACC状态、";
                    continue;
                Label_0109:
                    str = str + "高电平传感器1控制位，受控于高电平传感器1状态、";
                    continue;
                Label_012B:
                    str = str + "发动机控制位，自动上传受控于发动机状态、";
                    continue;
                Label_014D:
                    str = str + "高电平传感器2控制位，受控于高电平传感器2状态、";
                    continue;
                Label_016C:
                    str = str + "低电平传感器1控制位，受控于低电平传感器1状态、";
                    continue;
                Label_018B:
                    str = str + "低电平传感器2控制位，受控于低电平传感器2状态、";
                    continue;
                Label_01AA:
                    str = str + "门开关控制位，受控于门开关状态状态、";
                    continue;
                Label_01C9:
                    str = str + "后备电源控制位，受控于后备电源状态状态、";
                }
                str2 = Convert.ToString(Convert.ToInt32(Stat_S17, 0x10), 2).PadLeft(8, '0');
                for (int j = 0; j < 8; j++)
                {
                    int num4 = int.Parse(str2.Substring(7 - j, 1));
                    switch (j)
                    {
                        case 0:
                            {
                                if (num4 != 0)
                                {
                                    break;
                                }
                                str = str + "ACC开时自动上传、";
                                continue;
                            }
                        case 1:
                            {
                                if (num4 != 0)
                                {
                                    goto Label_0279;
                                }
                                str = str + "高电平传感器1，传感器高自动上传、";
                                continue;
                            }
                        case 2:
                            {
                                if (num4 != 0)
                                {
                                    goto Label_029C;
                                }
                                str = str + "发动机，发动机运转自动上传、";
                                continue;
                            }
                        case 3:
                            {
                                if (num4 != 0)
                                {
                                    goto Label_02BF;
                                }
                                str = str + "高电平传感器2，传感器高自动上传、";
                                continue;
                            }
                        case 4:
                            {
                                if (num4 != 0)
                                {
                                    goto Label_02E2;
                                }
                                str = str + "低电平传感器1，传感器搭铁自动上传、";
                                continue;
                            }
                        case 5:
                            {
                                if (num4 != 0)
                                {
                                    goto Label_0302;
                                }
                                str = str + "低电平传感器1，传感器搭铁自动上传、";
                                continue;
                            }
                        case 6:
                            {
                                if (num4 != 0)
                                {
                                    goto Label_0322;
                                }
                                str = str + "门开关，开门时自动上传、";
                                continue;
                            }
                        case 7:
                            {
                                if (num4 != 0)
                                {
                                    goto Label_0342;
                                }
                                str = str + "后备电源，主机由后备电源供电自动上传、";
                                continue;
                            }
                        default:
                            {
                                continue;
                            }
                    }
                    str = str + "ACC关时自动上传、";
                    continue;
                Label_0279:
                    str = str + "高电平传感器1，传感器低或悬空自动上传、";
                    continue;
                Label_029C:
                    str = str + "发动机，发动机停止自动上传、";
                    continue;
                Label_02BF:
                    str = str + "高电平传感器2，传感器低或悬空自动上传、";
                    continue;
                Label_02E2:
                    str = str + "低电平传感器1，传感器高或悬空自动上传、";
                    continue;
                Label_0302:
                    str = str + "低电平传感器1，传感器高或悬空自动上传、";
                    continue;
                Label_0322:
                    str = str + "门开关，关门时自动上传、";
                    continue;
                Label_0342:
                    str = str + "后备电源，主机由电瓶供电自动上传、";
                }
                return str;
            }
            catch
            {
                return "Error";
            }
        }

        /// <summary>
        /// 将TH车载机协议中定义的状态字转换为DB44协议中定义的状态字。（未完全转换）
        /// </summary>
        /// <param name="b1">第一字节</param>
        /// <param name="b2">第二字节</param>
        /// <param name="b3">第三字节</param>
        /// <param name="b4">第四字节</param>
        /// <param name="locationLocked">定位标记A则锁定</param>
        /// <param name="NorthEarth">北纬</param>
        /// <param name="EastEarth">东经</param>
        /// <returns></returns>
        private Db44VehicleState ConvertFromTHVehicleState(byte b1, byte b2, byte b3, byte b4,bool locationLocked,bool NorthEarth, bool EastEarth)
        {
            Db44VehicleState data = new Db44VehicleState();
            //初始化为正常状态
            data.SetValue(0, 0, EastEarth);
            data.SetValue(0, 1, NorthEarth);
            data.SetValue(2, 0, locationLocked);

            //状态位转换
            data.SetValue(0, 3, (b1 & (1 << 3)) == 0);//车辆处于断油电状态
            //data.SetValue(0, 0, (b1 & (1 << 4)) == 0);//电瓶拆除报警
            data.SetValue(2, 3, (b2 & (1 << 0)) == 0);//GPS接收机故障
            data.SetValue(3, 0, (b2 & (1 << 3)) == 0);//主机由后备电池供电
            data.SetValue(2, 2, (b2 & (1 << 5)) == 0);//GPS天线开路
            data.SetValue(2, 1, (b2 & (1 << 6)) == 0);//GPS天线短路
            data.SetValue(1, 1, (b3 & (1 << 0)) == 0);//车门开
            data.SetValue(1, 5, (b3 & (1 << 2)) > 0);//ACC关
            data.SetValue(3, 2, (b3 & (1 << 5)) == 0);//发动机启动
            data.SetValue(0, 2, (b4 & (1 << 1)) == 0);//紧急报警
            data.SetValue(0, 4, (b4 & (1 << 2)) == 0);//超速
            data.SetValue(2, 6, (b4 & (1 << 4)) == 0);//禁止驶入越界报警
            data.SetValue(2, 5, (b4 & (1 << 7)) == 0);//禁止驶出越界报警

            return data;
        }
        private byte[] vehicle_status(string _ID, int _Type, byte A, byte B, byte C, byte D, out string StatusStr, ref MdtWrapper mobileInfo)
        {
            StatusStr = "";
            byte[] buffer = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            string str = "";
            if ((A & 8) == 0)
            {
                str = str + "车辆处于断油电状态、";
                buffer[0] = (byte)(buffer[0] + 8);
            }
            if ((A & 0x10) == 0)
            {
                str = str + "电瓶拆除报警、";
                buffer[0] = (byte)(buffer[0] + 0x40);
            }
            if ((B & 1) == 0)
            {
                str = str + "GPS接收机故障、";
                buffer[2] = (byte)(buffer[2] + 8);
            }
            if ((B & 8) == 0)
            {
                str = str + "主机由后备电池供电、";
            }
            if ((B & 0x20) == 0)
            {
                str = str + "GPS天线开路、";
                buffer[2] = (byte)(buffer[2] + 4);
            }
            if ((B & 0x40) == 0)
            {
                str = str + "GPS天线短路、";
                buffer[2] = (byte)(buffer[2] + 2);
            }
            if ((C & 1) == 0)
            {
                str = str + "车门开、";
                buffer[1] = (byte)(buffer[1] + 2);
            }
            if ((C & 4) == 0)
            {
                str = str + "ACC关、";
            }
            else
            {
                str = str + "ACC开、";
                buffer[1] = (byte)(buffer[1] + 0x20);
            }
            if ((C & 0x20) == 0)
            {
                str = str + "发动机启动、";
                buffer[3] = (byte)(buffer[3] + 4);
            }
            if ((D & 2) == 0)
            {
                str = str + "紧急报警、";
                buffer[0] = (byte)(buffer[0] + 4);
            }
            if ((D & 4) == 0)
            {
                str = str + "超速、";
                buffer[0] = (byte)(buffer[0] + 0x10);
            }
            if ((D & 0x10) == 0)
            {
                str = str + "禁止驶入越界报警、";
                buffer[2] = (byte)(buffer[2] + 0x40);
            }
            if ((D & 0x80) == 0)
            {
                str = str + "禁止驶出越界报警、";
                buffer[2] = (byte)(buffer[2] + 0x20);
            }
            StatusStr = str + "@";
            str = str.Replace("、@", "").Replace("@", "");
            return buffer;
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        public struct CarRunInfo
        {
            public int carMaxV;
            public int carMinV;
            public int carStopTime;
        }
    }
}