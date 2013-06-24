using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using Parrot.Models;
using System.Diagnostics;
using System.Globalization;

namespace Parrot.Models.Longhan
{
    public class LH14_GPRS_PE_In
    {
        // Fields
        private static Hashtable GpsLiCheng_Hash = new Hashtable();
        private static Hashtable ImgHashList = new Hashtable();
        private int LogID;
        private int nDirection;
        private double nSpeed;
        private byte[] ReturnCmdByte = new byte[10];

        // Events
        public event GpsAlarmReceivedEventHandler Alarm;

        public event PlainMessageReceivedEventHandler PlainMessageReceived;

        public event PlainGpsDataReceivedEventHandler PlainGpsDataReceived;

        public event GpsDataReturnEventHandler GpsDataReturn;

        public event ImageReceivedEventHandler ImageReceived;
        
        public event TaxiOprationInfoReceivedEventHandler TaxiOprationInfoReceived;

        // Methods
        public LH14_GPRS_PE_In()
        {
            this.ReturnCmdByte[0] = 0x29;
            this.ReturnCmdByte[1] = 0x29;
            this.ReturnCmdByte[2] = 0x21;
            this.ReturnCmdByte[3] = 0;
            this.ReturnCmdByte[4] = 5;
            this.ReturnCmdByte[7] = 0;
            this.ReturnCmdByte[9] = 13;
            this.nSpeed = 0.0;
            this.nDirection = 0;
        }

        private void CmdReport(string _ID, byte M_Cmd_Id, byte C_Cmd_Id, byte S_Fa, ref MdtWrapper mobileInfo)
        {
            string str = "";
            switch (M_Cmd_Id)
            {
                case 0x25:
                    str = "图像采集器恢复出厂设置";
                    goto Label_02A8;

                case 0x26:
                    str = "设置报警触发方式";
                    goto Label_02A8;

                case 0x27:
                    str = "查询图像采集器设置状态信息";
                    goto Label_02A8;

                case 40:
                    str = "发送即时图像回传";
                    goto Label_02A8;

                case 0x29:
                    str = "设置摄像头图像参数";
                    goto Label_02A8;

                case 0x2a:
                case 0x2b:
                case 0x2c:
                case 0x2d:
                case 0x2e:
                case 0x2f:
                case 0x33:
                case 0x35:
                case 0x36:
                case 0x3b:
                case 60:
                case 0x41:
                case 0x42:
                case 0x63:
                case 100:
                case 0x6a:
                case 0x6b:
                case 0x6c:
                case 0x6d:
                case 110:
                case 0x6f:
                case 0x73:
                case 0x74:
                case 0x75:
                case 0x79:
                    goto Label_02A8;

                case 0x30:
                    str = "单次呼叫";
                    goto Label_02A8;

                case 0x31:
                    str = "状态查询";
                    goto Label_02A8;

                case 50:
                    str = "终端关机复位";
                    goto Label_02A8;

                case 0x34:
                    str = "定时回传间隔";
                    goto Label_02A8;

                case 0x37:
                    str = "取消报警";
                    goto Label_02A8;

                case 0x38:
                    str = "恢复油路";
                    goto Label_02A8;

                case 0x39:
                    str = "关闭油路";
                    goto Label_02A8;

                case 0x3a:
                    str = "调度短信";
                    goto Label_02A8;

                case 0x3d:
                    str = "查询软件版本";
                    goto Label_02A8;

                case 0x3e:
                    str = "单向电话监听";
                    goto Label_02A8;

                case 0x3f:
                    str = "设置超速报警";
                    goto Label_02A8;

                case 0x40:
                    str = "设置停车报警";
                    goto Label_02A8;

                case 0x43:
                    str = "下载集团电话号码";
                    goto Label_02A8;

                case 0x44:
                    str = "取消下载集团电话号码";
                    goto Label_02A8;

                case 0x45:
                    str = "下载车辆行驶线路节点";
                    goto Label_02A8;

                case 70:
                    str = "下载电子围栏";
                    goto Label_02A8;

                case 0x47:
                    str = "取消电子围栏";
                    goto Label_02A8;

                case 0x48:
                    str = "查询电子围栏";
                    goto Label_02A8;

                case 0x62:
                    str = "远程软件更新";
                    goto Label_02A8;

                case 0x65:
                    str = "图象定时采集";
                    goto Label_02A8;

                case 0x66:
                    str = "清除里程";
                    goto Label_02A8;

                case 0x67:
                    str = "远程开车门";
                    goto Label_02A8;

                case 0x68:
                    str = "远程关车门";
                    goto Label_02A8;

                case 0x69:
                    str = "远程修改UDP（IP号和端口号）";
                    goto Label_02A8;

                case 0x70:
                    str = "ACC关定时发送间隔";
                    goto Label_02A8;

                case 0x71:
                    str = "设置GPRS心跳发送时间";
                    goto Label_02A8;

                case 0x72:
                    str = "查询GPRS主机设置状态2";
                    goto Label_02A8;

                case 0x76:
                    str = "远程修改TCP（IP号和端口号）";
                    goto Label_02A8;

                case 0x77:
                    str = "远程修改SMS中心号码";
                    goto Label_02A8;

                case 120:
                    str = "透明传输";
                    goto Label_02A8;

                case 0x7a:
                    str = "GPRS连接检测间隔";
                    goto Label_02A8;

                case 0x98:
                    str = "发送出租车调度";
                    break;

                case 0xb0:
                    str = "解除报警";
                    break;
            }
        Label_02A8:
            str = str + "指令成功";
            mobileInfo.LastCmdReTurn = str;
            this.PlainMessageReceived(_ID, 2, "控制回复", "执行" + str,  mobileInfo);
        }

        private void ComOperation(string _ID, string body, ref MdtWrapper mobileInfo)
        {
            string str = "";
            if (body.StartsWith("E759"))
            {
                string s = int.Parse(body.Substring(4, 8), NumberStyles.HexNumber).ToString();
                string str3 = int.Parse(body.Substring(12, 8), NumberStyles.HexNumber).ToString();
                string str4 = int.Parse(body.Substring(20, 2), NumberStyles.HexNumber).ToString() + "." + int.Parse(body.Substring(0x16, 2), NumberStyles.HexNumber).ToString();
                string str5 = int.Parse(body.Substring(0x18, 4), NumberStyles.HexNumber).ToString();
                str = string.Concat(new object[] { "里程=", s, "公里；油量=", 100 - int.Parse(str3), "%；温度=", str4, "摄氏度" });
                mobileInfo.Mileages = double.Parse(s);
                mobileInfo.Temperature = double.Parse(str4);
                mobileInfo.OilCapacity = double.Parse(str3);
                lock (GpsLiCheng_Hash)
                {
                    if (GpsLiCheng_Hash.ContainsKey(_ID))
                    {
                        GpsLiCheng_Hash[_ID] = str;
                    }
                    else
                    {
                        GpsLiCheng_Hash.Add(_ID, str);
                    }
                }
            }
            else if (body.StartsWith("25"))
            {
                int num = int.Parse(body.Substring(4, 4), NumberStyles.HexNumber);
                string str6 = "电压=" + ((((double)num) / 100.0)).ToString("00.00") + "V";
                string str7 = body.Substring(8, 2);
                str7 = "水温" + int.Parse(str7, NumberStyles.HexNumber).ToString("") + "℃";
                string str8 = body.Substring(10, 2);
                str8 = "油位" + ((double.Parse(str8, NumberStyles.HexNumber) * 10.0)).ToString("00.0") + "%";
                string str9 = body.Substring(12, 2);
                str9 = "油温" + int.Parse(str9, NumberStyles.HexNumber).ToString() + "℃";
                string str10 = body.Substring(14, 2);
                str10 = "油压" + double.Parse(str10, NumberStyles.HexNumber).ToString() + "度量MPa";
                int num2 = int.Parse(body.Substring(0x10, 8), NumberStyles.HexNumber);
                string str11 = "工作时间=" + ((((double)num2) / 10.0)).ToString("0000000000.0") + "小时";
                string str12 = body.Substring(0x18, 4);
                str12 = "转速值=" + int.Parse(str12, NumberStyles.HexNumber).ToString("") + "r/min";
                str = str6 + "、" + str7 + "、" + str8 + "、" + str9 + "、" + str10 + "、" + str11 + "、" + str12;
                byte num3 = byte.Parse(body.Substring(0x1c, 2), NumberStyles.HexNumber);
                string str13 = "";
                if ((num3 & 0x80) == 0x80)
                {
                    str13 = "油水分离报警";
                }
                if ((num3 & 0x40) == 0x40)
                {
                    str13 = str13 + "电压低报警、";
                }
                if ((num3 & 0x20) == 0x20)
                {
                    str13 = "蜂鸣响、";
                }
                if ((num3 & 0x10) == 0x10)
                {
                    str13 = str13 + "油压低报警、";
                }
                if ((num3 & 8) == 8)
                {
                    str13 = str13 + "水温高报警、";
                }
                if ((num3 & 4) == 4)
                {
                    str13 = str13 + "油温高报警、";
                }
                if ((num3 & 2) == 2)
                {
                    str13 = str13 + "油滤堵塞报警、";
                }
                if ((num3 & 1) == 1)
                {
                    str13 = str13 + "空滤堵塞报警、";
                }
                if (str13 != "")
                {
                    this.Alarm(_ID, 1, "车辆检测仪表报警", str13, mobileInfo.LastAV, mobileInfo.LastX, mobileInfo.LastY, mobileInfo.LastV, mobileInfo.LastF.ToString(), DateTime.Now.ToString(),  mobileInfo);
                }
                lock (GpsLiCheng_Hash)
                {
                    if (GpsLiCheng_Hash.ContainsKey(_ID))
                    {
                        GpsLiCheng_Hash[_ID] = str;
                    }
                    else
                    {
                        GpsLiCheng_Hash.Add(_ID, str);
                    }
                }
            }
        }

        public void DataIn(string _ID, int _Type, ref byte[] body, int ByteLen, ref MdtWrapper mobileInfo)
        {
            for (int i = 0; i < ByteLen; i++)
            {
                try
                {
                    if ((body[i] == 0x29) & (body[i + 1] == 0x29))
                    {
                        int num2 = (body[i + 3] * 0x100) + body[i + 4];
                        byte[] buffer = Convert.FromBase64String(Convert.ToBase64String(body, i, 5 + num2));
                        this.GPSDataIn_(_ID, _Type, buffer, buffer.Length, ref mobileInfo);
                        i = (i + num2) + 4;
                    }
                }
                catch
                {
                }
            }
        }

        private void EP_Pack(string _ID, ref byte[] body, ref MdtWrapper mobileInfo)
        {
            if (body.Length >= 0x29)
            {
                this.EP_Pack_Pro(_ID, ref body, ref mobileInfo);
            }
        }

        private bool EP_Pack_Nol(string _ID, ref byte[] body, ref MdtWrapper mobileInfo)
        {
            //if ((body[0x1b] & 0x80) == 0x80)
            //{
            //    //this.v = "1";
            //}
            //else
            //{
            //    //this.v = "0";
            //}
            if (body[2] == 0x8e)
            {
                return false;
            }
            byte[] array = new byte[30];
            array[0] = body[9];
            array[1] = body[10];
            array[2] = body[11];
            array[3] = body[12];
            array[4] = body[13];
            array[5] = body[14];
            array[6] = body[0x13];
            array[7] = body[20];
            array[8] = body[0x15];
            array[9] = body[0x16];
            array[10] = body[15];
            array[11] = body[0x10];
            array[12] = body[0x11];
            array[13] = body[0x12];
            this.nSpeed = (((body[0x17] % 0x10) * 100) + ((body[0x18] / 0x10) * 10)) + (body[0x18] % 0x10);
            array[14] = (byte)this.nSpeed;
            this.nDirection = (((body[0x19] % 0x10) * 100) + ((body[0x1a] / 0x10) * 10)) + (body[0x1a] % 0x10);
            array[15] = (byte)(this.nDirection / 2);
            array[0x10] = 0;
            array[0x11] = 0;
            array[0x12] = 0;
            array[0x13] = 0;
            array[20] = 0;
            array[0x15] = 0;
            if (body[2] == 130)
            {
                string statusStr = "";
                byte[] buffer2 = this.GetAlarmStatus_16(_ID, ref body, out statusStr, ref mobileInfo);
                buffer2[0] = (byte)((buffer2[0] + 1) + 2);
                buffer2.CopyTo(array, 0x16);
                if (this.PlainGpsDataReceived != null)
                {
                    this.PlainGpsDataReceived(_ID,  array,   mobileInfo);
                }
            }
            return true;
        }

        private void EP_Pack_Pro(string _ID, ref byte[] body, ref MdtWrapper mobileInfo)
        {
            byte[] array = new byte[30];
            array[0] = body[9];
            array[1] = body[10];
            array[2] = body[11];
            array[3] = body[12];
            array[4] = body[13];
            array[5] = body[14];
            array[6] = body[0x13];
            array[7] = body[20];
            array[8] = body[0x15];
            array[9] = body[0x16];
            array[10] = body[15];
            array[11] = body[0x10];
            array[12] = body[0x11];
            array[13] = body[0x12];
            this.nSpeed = (((body[0x17] % 0x10) * 100) + ((body[0x18] / 0x10) * 10)) + (body[0x18] % 0x10);
            array[14] = (byte)this.nSpeed;
            this.nDirection = (((body[0x19] % 0x10) * 100) + ((body[0x1a] / 0x10) * 10)) + (body[0x1a] % 0x10);
            array[15] = (byte)(this.nDirection / 2);
            array[0x10] = 0;
            array[0x11] = 0;
            int num = (((body[0x1c] * 0x100) * 0x100) + (body[0x1d] * 0x100)) + body[30];
            num /= 100;
            string str = num.ToString("0").PadLeft(8, '0');
            for (int i = 0; i < 4; i++)
            {
                array[0x12 + i] = Convert.ToByte(str.Substring(i * 2, 2), 0x10);
            }
            string statusStr = "";
            byte[] buffer2 = this.Vehicle_St1St2St3St4(_ID, body[0x1b], body[0x1f], body[0x20], body[0x21], body[0x22], body[40], out statusStr, ref mobileInfo);
            buffer2[0] = (byte)((buffer2[0] + 1) + 2);
            buffer2.CopyTo(array, 0x16);
            if (this.PlainGpsDataReceived != null)
            {
                this.PlainGpsDataReceived(_ID,  array,   mobileInfo);
            }
        }

        private byte[] GetAlarmStatus_16(string _ID, ref byte[] body, out string StatusStr, ref MdtWrapper mobileInfo)
        {
            StatusStr = "";
            byte[] buffer = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte num = body[30];
            byte num2 = body[0x1f];
            string str = "";
            string str2 = "";
            if ((num & 0x80) == 0x80)
            {
                str = "入区域报警、";
                buffer[2] = (byte)(buffer[2] + 0x40);
            }
            if ((num & 0x40) == 0x40)
            {
                str = "出区域报警、";
                buffer[2] = (byte)(buffer[2] + 0x20);
            }
            if ((num2 & 0x80) == 0x80)
            {
                str = str + "非法开门、";
                str2 = str2 + "安防报警";
            }
            if ((num & 0x40) == 0x40)
            {
                str = str + "拖车报警、";
                str2 = str2 + "异常移动";
            }
            if ((num2 & 0x20) == 0x20)
            {
                str = str + "振动报警、";
                buffer[1] = (byte)(buffer[1] + 0x20);
            }
            if ((num2 & 8) == 8)
            {
                str = str + "主电源断开、";
                buffer[1] = (byte)(buffer[1] + 0x40);
            }
            if ((num2 & 2) == 2)
            {
                str = str + "用户车辆超速行驶中";
                buffer[1] = (byte)(buffer[1] + 0x10);
            }
            if ((num2 & 1) == 1)
            {
                str = str + "用户遇劫警或紧急求助、";
                buffer[1] = (byte)(buffer[1] + 4);
            }
            StatusStr = str + "@";
            str = str.Replace("、@", "").Replace("@", "");
            return buffer;
        }

        private void GetMobile_V(string _ID, byte[] body, ref MdtWrapper mobileInfo)
        {
            string str = "";
            str = "型号:LH-" + body[9].ToString() + "，软件版本:V:" + body[10].ToString() + "，日期:V:20" + body[11].ToString("X2") + "-" + body[12].ToString("X2") + "-" + body[13].ToString("X2") + "　" + body[14].ToString("X2") + ":00，";
            if ((body[15] & 0x80) == 0x80)
            {
                str = str + "支持摄像功能，";
            }
            if ((body[15] & 0x40) == 0x40)
            {
                str = str + "支持软件远程下载更新功能，";
            }
            if ((body[15] & 0x20) == 0x20)
            {
                str = str + "支持看车功能，";
            }
            if ((body[15] & 0x10) == 0x10)
            {
                str = str + "支持防盗功能，";
            }
            if ((body[15] & 8) == 8)
            {
                str = str + "支持应急报警功能，";
            }
            if ((body[15] & 4) == 4)
            {
                str = str + "支持手柄电话功能，";
            }
            if ((body[15] & 2) == 2)
            {
                str = str + "支持显示屏，";
            }
            if ((body[15] & 1) == 1)
            {
                str = str + "可接计价器";
            }
            else
            {
                str = str + "不可接计价器";
            }
            this.PlainMessageReceived(_ID, 2, "车载设备版本", str,  mobileInfo);
        }

        private void GetMobileImg(string _ID, byte[] body, ref MdtWrapper mobileInfo)
        {
            try
            {
                int num = (body[3] * 0x100) + body[4];
                string s = Convert.ToBase64String(body, 13, 0x3e8);
                byte[] buffer = Convert.FromBase64String(s);
                buffer[0] = buffer[1];
                byte num2 = (byte)(body[10] % 0x10);
                byte num3 = (byte)(body[10] / 0x10);
                byte num4 = body[9];
                byte num5 = body[11];
                byte num6 = body[12];
                this.PlainMessageReceived(_ID, 0x65, "接收图片数据", "第：" + num2.ToString() + "路摄像头，图片序号：" + num3.ToString() + "，总数据包：" + num6.ToString() + "，数据包序号：" + num4.ToString(),  mobileInfo);
                lock (ImgHashList)
                {
                    ImageTemp temp = (ImageTemp)ImgHashList[_ID];
                    if (temp == null)
                    {
                        temp = new ImageTemp();
                        temp.Inti(_ID, num3, num6, num2);
                        ImgHashList.Add(_ID, temp);
                    }
                    else if (temp.ImageID != num3)
                    {
                        temp.ImageTempList.Clear();
                        temp.ImageID = num3;
                        temp.PackAll = num6;
                        temp.Inti(_ID, num3, num6, num2);
                    }
                    temp.ImageTempList[num4 - 1] = s;
                    string imgStatu = "";
                    bool flag = true;
                    for (int i = 0; i < temp.ImageTempList.Count; i++)
                    {
                        if (temp.ImageTempList[i].ToString() == "")
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        switch (num5)
                        {
                            case 1:
                                imgStatu = "用户报劫警时回传现场图像";
                                break;

                            case 2:
                                imgStatu = "自定义传感器为高时触发回传现场图像";
                                break;

                            case 3:
                                imgStatu = "自定义传感器为低时触发回传现场图像";
                                break;

                            case 4:
                                imgStatu = "自动定时回传图像";
                                break;

                            case 5:
                                imgStatu = "即时现场图像回传";
                                break;
                        }
                        this.PlainMessageReceived(_ID, 2, "接收图片数据", "接收第：" + num2.ToString() + "路摄像头，图片序号：" + num3.ToString() + "，总共：" + num6.ToString() + "包数据已接收完成！",  mobileInfo);
                        if (this.ImageReceived != null)
                        {
                            long ticks = DateTime.UtcNow.Ticks;
                            for (int j = 0; j < temp.ImageTempList.Count; j++)
                            {
                                this.ImageReceived(_ID, temp.CCD_ID, ticks, num6, j, temp.ImageTempList[j].ToString(), imgStatu, temp.FilmDateTime,  mobileInfo);
                                if (j >= (num6 - 1))
                                {
                                    temp.ImageTempList.Clear();
                                    temp.ImageID = 0xff;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Image Error--" + exception.ToString());
            }
        }

        private void GetStatus(string _ID, byte[] body, ref MdtWrapper mobileInfo)
        {
            string str = "";
            if (body[0x12] == 1)
            {
                str = str + "定时采样";
            }
            else
            {
                str = str + "定距采样";
            }
            str = str + "、采样值=" + (((body[0x13] * 0x100) + body[20])).ToString();
            if (body[0x15] == 1)
            {
                str = str + "、单点发送";
            }
            else
            {
                str = str + "、静默";
            }
            if (body[0x16] == 0)
            {
                str = str + "、停车设置=无";
            }
            else
            {
                str = str + "、停车设置=" + body[0x16].ToString() + "分钟";
            }
            if (((mobileInfo.MobileType == 0xca) | (mobileInfo.MobileType == 0xcb)) | (mobileInfo.MobileType == 0xcd))
            {
                if (body[0x17] == 0)
                {
                    str = str + "、超速设置=无";
                }
                else
                {
                    str = str + "、超速门限=" + body[0x17].ToString("0") + "公里";
                }
            }
            else if (body[0x17] == 0)
            {
                str = str + "、超速设置=无";
            }
            else
            {
                str = str + "、超速门限=" + ((body[0x17] * 1.852)).ToString("0") + "公里";
            }
            if (body[0x18] == 0)
            {
                str = str + "、电话限制=无";
            }
            else
            {
                str = str + "、集团电话限制";
            }
            if (body[0x19] == 0)
            {
                str = str + "、节点限制=无";
            }
            else
            {
                str = str + "、线路节点限制";
            }
            this.PlainMessageReceived(_ID, 2, "终端状态", str,  mobileInfo);
        }

        public void GprsDataIn_(string _ID, int Car_Type, byte[] body, ref MdtWrapper mobileInfo)
        {
            string str4;
            string str5;
            this.LogID++;
            if (this.LogID > 0xf4240)
            {
                this.LogID = 0;
            }
            switch (body[2])
            {
                case 0x80:
                    break;

                case 0x81:
                    this.PlainMessageReceived(_ID, 2, "控制回复", "终端点名回应",  mobileInfo);
                    break;

                case 130:
                    this.EP_Pack(_ID, ref body, ref mobileInfo);
                    goto Label_0912;

                case 0x83:
                    this.Vehicle_SetStatu(_ID, body[0x23], body[0x24], body[0x25], body[0x26], body[0x27], body[40], body[0x29], body[0x2a], ref mobileInfo);
                    break;

                case 0x84:
                    {
                        string str = "";
                        if (body[9] != 0xe7)
                        {
                            str = Encoding.Default.GetString(body, 9, body[4] - 6);
                            this.PlainMessageReceived(_ID, 3, "终端信息", str,  mobileInfo);
                        }
                        else
                        {
                            if (body[10] != 0x59)
                            {
                                if (body[10] == 0x70)
                                {
                                    string str2 = "油量变化提示";
                                    string str3 = "";
                                    if (body[11] == 0)
                                    {
                                        str3 = "油量降低，下降幅度为：" + body[12].ToString() + "%";
                                    }
                                    else
                                    {
                                        str3 = "油量上升，上升幅度为：" + body[12].ToString() + "%";
                                    }
                                    this.Alarm(_ID, 1, str2, str3, mobileInfo.LastAV, mobileInfo.LastX, mobileInfo.LastY, mobileInfo.LastV, mobileInfo.LastF.ToString(), DateTime.Now.ToString(),  mobileInfo);
                                }
                            }
                            else
                            {
                                for (int j = 0; j < 15; j++)
                                {
                                    str = str + body[9 + j].ToString("X2");
                                }
                                this.ComOperation(_ID, str, ref mobileInfo);
                            }
                            str = "";
                            for (int i = 9; i < (body.Length - 2); i++)
                            {
                                str = str + body[i].ToString("X2");
                            }
                            this.PlainMessageReceived(_ID, 200, "终端信息", str,  mobileInfo);
                        }
                        goto Label_0912;
                    }
                case 0x85:
                    if (body[4] == 40)
                    {
                        this.CmdReport(_ID, body[body.Length - 3], 0, 0, ref mobileInfo);
                    }
                    else
                    {
                        if (!((body[4] == 0x2c) | (body[4] == 11)))
                        {
                            return;
                        }
                        this.CmdReport(_ID, body[body.Length - 7], 0, 0, ref mobileInfo);
                    }
                    break;

                case 0x86:
                    this.GetMobile_V(_ID, body, ref mobileInfo);
                    return;

                case 0x87:
                    this.UpdateMobile_V(_ID, body, ref mobileInfo);
                    goto Label_0912;

                case 0x88:
                    this.UpdateLine_V(_ID, body, ref mobileInfo);
                    return;

                case 0x89:
                    this.ReturnCmdByte[5] = body[body.Length - 2];
                    this.EP_Pack(_ID, ref body, ref mobileInfo);
                    this.PassPoint(_ID, body, ref mobileInfo);
                    return;

                case 140:
                    str4 = "";
                    switch (body[9])
                    {
                        case 1:
                            str4 = "取消下载电话号码成功";
                            goto Label_0393;

                        case 2:
                            str4 = "手柄下载电话号码成功";
                            goto Label_0393;

                        case 3:
                            str4 = "手柄下载电话号码失败";
                            goto Label_0393;

                        case 4:
                            str4 = "取消下载电话号码失败";
                            goto Label_0393;
                    }
                    goto Label_0393;

                case 0x8d:
                    this.GetMobileImg(_ID, body, ref mobileInfo);
                    goto Label_0912;

                case 0x8e:
                    this.EP_Pack(_ID, ref body, ref mobileInfo);
                    goto Label_0912;

                case 0x90:
                    {
                        str5 = "";
                        if ((body[10] & 8) == 8)
                        {
                            str5 = str5 + "一";
                        }
                        if ((body[10] & 4) == 4)
                        {
                            if (str5 != "")
                            {
                                str5 = str5 + "、";
                            }
                            str5 = str5 + "二";
                        }
                        if ((body[10] & 2) == 2)
                        {
                            if (str5 != "")
                            {
                                str5 = str5 + "、";
                            }
                            str5 = str5 + "三";
                        }
                        if ((body[10] & 1) == 1)
                        {
                            if (str5 != "")
                            {
                                str5 = str5 + "、";
                            }
                            str5 = str5 + "四";
                        }
                        str5 = "循环拍摄第（" + str5 + "）路摄像头；";
                        str5 = "回传间隔：" + ((body[9] * 30)).ToString() + "秒；" + str5;
                        string str6 = "第一路摄像头参数（";
                        string str11 = str6;
                        str6 = str11 + "亮度=" + body[11].ToString() + "、对比度=" + body[12].ToString() + "、色饱和度=" + body[13].ToString() + "、色调=" + body[14].ToString() + "）；";
                        str5 = str5 + str6;
                        str6 = "第二路摄像头参数（";
                        string str12 = str6;
                        str6 = str12 + "亮度=" + body[15].ToString() + "、对比度=" + body[0x10].ToString() + "、色饱和度=" + body[0x11].ToString() + "、色调=" + body[0x12].ToString() + "）；";
                        str5 = str5 + str6;
                        str6 = "第三路摄像头参数（";
                        string str13 = str6;
                        str6 = str13 + "亮度=" + body[0x13].ToString() + "、对比度=" + body[20].ToString() + "、色饱和度=" + body[0x15].ToString() + "、色调=" + body[0x16].ToString() + "）；";
                        str5 = str5 + str6;
                        str6 = "第四路摄像头参数（";
                        string str14 = str6;
                        str6 = str14 + "亮度=" + body[0x17].ToString() + "、对比度=" + body[0x18].ToString() + "、色饱和度=" + body[0x19].ToString() + "、色调=" + body[0x1a].ToString() + "）；";
                        str5 = str5 + str6;
                        this.PlainMessageReceived(_ID, 2, "GPRS主机设置状态", str5,  mobileInfo);
                        goto Label_0912;
                    }
                case 0x91:
                    str5 = "";
                    if (!(((body[9] == 0xff) & (body[10] == 2)) & (body[12] == 0x30)))
                    {
                        if (body[9] == 4)
                        {
                            string str7 = Encoding.Default.GetString(body, 10, 13);
                            this.PlainMessageReceived(_ID, 600, "抢答订单", str7,  mobileInfo);
                        }
                        if (body[9] == 5)
                        {
                            string str8 = Encoding.Default.GetString(body, 10, 13);
                            this.PlainMessageReceived(_ID, 600, "取消订单", str8,  mobileInfo);
                        }
                        if (body[9] == 6)
                        {
                            string str9 = Encoding.Default.GetString(body, 10, 13);
                            this.PlainMessageReceived(_ID, 600, "完成订单", str9,  mobileInfo);
                        }
                    }
                    else
                    {
                        this.Taxi_Run_Check(_ID, body, ref mobileInfo);
                    }
                    goto Label_0912;

                case 150:
                    str5 = "心跳时间：" + body[9].ToString() + "秒、当ACC关时发送间隔：" + body[10].ToString();
                    this.PlainMessageReceived(_ID, 2, "GPRS主机设置状态", str5,  mobileInfo);
                    return;

                case 0x9a:
                    this.ReturnEnclosure_16(_ID, body, ref mobileInfo);
                    return;

                case 0xb1:
                case 0xff:
                    goto Label_0912;

                case 0xb2:
                    str5 = Encoding.Default.GetString(body, 9, ((body[3] * 0x100) + body[4]) - 6);
                    str5 = Convert.ToBase64String(Encoding.Default.GetBytes(str5));
                    this.PlainMessageReceived(_ID, 11, "主机版本", str5,  mobileInfo);
                    goto Label_0912;

                case 0xb3:
                    str5 = Encoding.Default.GetString(body, 9, ((body[3] * 0x100) + body[4]) - 6);
                    str5 = Convert.ToBase64String(Encoding.Default.GetBytes(str5));
                    this.PlainMessageReceived(_ID, 11, "主机工作状态", str5,  mobileInfo);
                    goto Label_0912;

                default:
                    {
                        string str10 = "";
                        for (int k = 0; k < body.Length; k++)
                        {
                            str10 = str10 + " " + body[k].ToString("X2");
                        }
                        return;
                    }
            }
            this.EP_Pack(_ID, ref body, ref mobileInfo);
            goto Label_0912;
        Label_0393:
            this.PlainMessageReceived(_ID, 2, "集团电话号码数据到手柄", str4,  mobileInfo);
            return;
        Label_0912:
            this.ReturnCmdByte[5] = body[body.Length - 2];
            this.ReturnCmdByte[6] = body[2];
            this.ReturnCmdByte[7] = body[9];
            this.ReturnCmdByte[8] = LonghanWrapper.Get_CheckXor(ref this.ReturnCmdByte, 8);
            this.GpsDataReturn(_ID, Convert.ToBase64String(this.ReturnCmdByte, 0, 10),  mobileInfo, 1);
        }

        private void GPSDataIn_(string _ID, int _Type, byte[] body, int ByteLen, ref MdtWrapper mobileInfo)
        {
            for (int i = 0; i < ByteLen; i++)
            {
                if ((body[i] == 0x29) & (body[i + 1] == 0x29))
                {
                    int num2 = (body[i + 3] * 0x100) + body[i + 4];
                    string s = Convert.ToBase64String(body, i, 5 + num2);
                    this.GprsDataIn_(_ID, _Type, Convert.FromBase64String(s), ref mobileInfo);
                    i = (i + num2) + 4;
                }
            }
        }

        private void PassPoint(string _ID, byte[] body, ref MdtWrapper mobileInfo)
        {
            string str = "司机工号:" + body[30].ToString("2X") + body[0x1f].ToString("2X") + body[0x20].ToString("2X");
            if (body[0x21] == 1)
            {
                str = "入区域报警，区域号：";
            }
            else if (body[0x21] == 0)
            {
                str = "出区域报警，区域号：";
            }
            str = str + body[0x22].ToString();
            this.Alarm(_ID, 1, "区域报警", str, mobileInfo.LastAV, mobileInfo.LastX, mobileInfo.LastY, mobileInfo.LastV, mobileInfo.LastF.ToString(), DateTime.Now.ToString(),  mobileInfo);
        }

        private void ReturnEnclosure_16(string _ID, byte[] data, ref MdtWrapper mobileInfo)
        {
            string str = string.Empty + "总共围栏个数：" + data[9].ToString() + "、所查询的围栏内容：";
            double num = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            try
            {
                for (int i = 0; i < data[9]; i++)
                {
                    num = double.Parse(data[(i * 0x12) + 11].ToString("X2").Remove(0, 1) + data[(i * 0x12) + 12].ToString("X2") + data[(i * 0x12) + 13].ToString("X2")) / 60000.0;
                    num2 = double.Parse(data[(i * 0x12) + 10].ToString("X2").Remove(0, 1) + data[(i * 0x12) + 11].ToString("X2").Substring(0, 1));
                    num3 = num + num2;
                    str = str + " 小纬=" + num3.ToString("00.00000");
                    num = double.Parse(data[(i * 0x12) + 15].ToString("X2").Remove(0, 1) + data[(i * 0x12) + 0x10].ToString("X2") + data[(i * 0x12) + 0x11].ToString("X2")) / 60000.0;
                    num2 = double.Parse(data[(i * 0x12) + 14].ToString("X2") + data[(i * 0x12) + 15].ToString("X2").Substring(0, 1));
                    num3 = num + num2;
                    str = str + " 小经=" + num3.ToString("00.00000");
                    num = double.Parse(data[(i * 0x12) + 0x13].ToString("X2").Remove(0, 1) + data[(i * 0x12) + 20].ToString("X2") + data[(i * 0x12) + 0x15].ToString("X2")) / 60000.0;
                    num2 = double.Parse(data[(i * 0x12) + 0x12].ToString("X2").Remove(0, 1) + data[(i * 0x12) + 0x13].ToString("X2").Substring(0, 1));
                    num3 = num + num2;
                    str = str + " 大纬=" + num3.ToString("00.00000");
                    num = double.Parse(data[(i * 0x12) + 0x17].ToString("X2").Remove(0, 1) + data[(i * 0x12) + 0x18].ToString("X2") + data[(i * 0x12) + 0x19].ToString("X2")) / 60000.0;
                    num2 = double.Parse(data[(i * 0x12) + 0x16].ToString("X2") + data[(i * 0x12) + 0x17].ToString("X2").Substring(0, 1));
                    str = str + " 大经=" + ((num + num2)).ToString("00.00000");
                    str = str + "、围栏号：" + data[(i * 0x12) + 0x1a].ToString();
                    switch (data[(i * 0x12) + 0x1b])
                    {
                        case 0:
                            str = str + "、围栏报警方式：表示驶入围栏报警";
                            break;

                        case 1:
                            str = str + "、围栏报警方式：表示驶出围栏报警";
                            break;

                        case 2:
                            str = str + "、围栏报警方式：表示驶入驶出围栏报警";
                            break;

                        case 3:
                            str = str + "、围栏报警方式：表示禁止围栏报警";
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
            this.PlainMessageReceived(_ID, 2, "终端返回电子围栏信息", str,  mobileInfo);
        }

        private void Taxi_Run_Check(string _ID, byte[] _body, ref MdtWrapper mobileInfo)
        {
            int count = _body[4] - 9;
            byte[] bytes = new byte[count];
            string str = Encoding.Default.GetString(_body, 11, count);
            for (int i = 0; i < (str.Length / 2); i++)
            {
                string str2 = str.Substring(i * 2, 2);
                bytes[i] = Convert.ToByte(str2, 0x10);
            }
            string str3 = "";
            string payType = "";
            if (bytes[2] == 0)
            {
                payType = "0";
                str3 = "现金交易、";
            }
            else if (bytes[2] == 1)
            {
                payType = "1";
                str3 = "IC卡交易、";
            }
            string taxiID = Encoding.Default.GetString(bytes, 3, 6);
            str3 = str3 + "车号：" + taxiID + "、";
            string str6 = bytes[9].ToString("X2") + bytes[10].ToString("X2") + "-" + bytes[11].ToString("X2") + "-" + bytes[12].ToString("X2");
            string str7 = bytes[13].ToString("X2") + ":" + bytes[14].ToString("X2");
            string getInDateTime = str6 + " " + str7;
            str3 = str3 + "上车时间：" + getInDateTime + "、";
            string str9 = bytes[15].ToString("X2") + ":" + bytes[0x10].ToString("X2");
            string getOutDateTime = str6 + " " + str9;
            str3 = str3 + "下车时间：" + getOutDateTime + "、";
            string waitTime = (((bytes[0x11] * 0xe10) + (bytes[0x12] * 60)) + bytes[0x13]).ToString();
            str3 = str3 + "等候时间：" + waitTime + "秒、";
            string mileage = string.Concat(new object[] { bytes[20].ToString("X2"), bytes[0x15] / 0x10, ".", bytes[0x20] % 0x10 });
            str3 = str3 + "行驶里程：" + mileage + "公里、";
            string allroundPrice = bytes[0x16].ToString("X2") + bytes[0x17].ToString("X2") + "." + bytes[0x18].ToString("X2");
            str3 = str3 + "营运金额：" + allroundPrice + "元、";
            string priceOfGetOut = bytes[0x19].ToString("X2") + "." + bytes[0x1a].ToString("X2");
            str3 = str3 + "下车时单价：" + priceOfGetOut + "元、";
            string freeMileage = string.Concat(new object[] { bytes[0x1b].ToString("X2"), bytes[0x1c] / 0x10, ".", bytes[0x1c] % 0x10 });
            str3 = str3 + "空驶里程：" + freeMileage + "公里";
            this.PlainMessageReceived(_ID, 2, "出租车营运报告", str3,  mobileInfo);
            if (this.TaxiOprationInfoReceived != null)
            {
                this.TaxiOprationInfoReceived(mobileInfo.SysID.ToString(), payType, taxiID, getInDateTime, getOutDateTime, waitTime, mileage, allroundPrice, priceOfGetOut, freeMileage);
            }
            if (payType == "1")
            {
            }
        }

        private void UpdateLine_V(string _ID, byte[] body, ref MdtWrapper mobileInfo)
        {
            string str = "";
            if (body[9] == 1)
            {
                str = "终端请求下载第：" + body[10].ToString() + "包数据";
            }
            else if (body[9] == 2)
            {
                str = "终端下载节点数据已完成";
            }
            else if (body[9] == 3)
            {
                str = "终端下载节点数据发生错误！";
            }
            this.PlainMessageReceived(_ID, 2, "下载节点数据", str,  mobileInfo);
        }

        private void UpdateMobile_V(string _ID, byte[] body, ref MdtWrapper mobileInfo)
        {
            string str = "";
            if (body[9] == 1)
            {
                byte num;
                byte num2;
                byte num3;
                byte num4;
                str = "终端请求更新第：" + body[10].ToString() + "包数据";
                byte[] temp = new byte[0x10d];
                temp[0] = 0x29;
                temp[1] = 0x29;
                temp[2] = 0x63;
                temp[3] = 1;
                temp[4] = 8;
                LonghanWrapper.Get_IP_From_CarID(_ID, out num, out num2, out num3, out num4);
                temp[5] = num;
                temp[6] = num2;
                temp[7] = num3;
                temp[8] = num4;
                temp[9] = body[10];
                temp[10] = 0;
                int num5 = (temp[3] * 0x100) + temp[4];
                temp[(5 + num5) - 2] = LonghanWrapper.Get_CheckXor(ref temp, (5 + num5) - 2);
                temp[(5 + num5) - 1] = 13;
                this.GpsDataReturn(_ID, Convert.ToBase64String(this.ReturnCmdByte, 0, 10),  mobileInfo, 0);
            }
        }

        private string Vehicle_SetStatu(string _ID, byte A, byte B, byte C, byte D, byte E, byte F, byte G, byte H, ref MdtWrapper mobileInfo)
        {
            string str = "";
            string[] strArray = new string[] { "定时发送间隔：", ((A * 0x100) + B).ToString(), "秒、停车报警检测时间：", C.ToString(), "分钟、" };
            str = string.Concat(strArray);
            if (((mobileInfo.MobileType == 0xca) | (mobileInfo.MobileType == 0xcb)) | (mobileInfo.MobileType == 0xcd))
            {
                str = str + "超速门限：" + D.ToString("0") + "公里、";
            }
            else
            {
                str = str + "超速门限：" + ((D * 1.852)).ToString("0") + "公里、";
            }
            string str3 = str;
            string[] strArray2 = new string[6];
            strArray2[0] = str3;
            strArray2[1] = "登签：";
            strArray2[2] = F.ToString();
            strArray2[3] = "、定时发送图片的时间：";
            strArray2[4] = (G * 30).ToString();
            strArray2[5] = "秒";
            str = string.Concat(strArray2);
            this.PlainMessageReceived(_ID, 2, "终端状态", str,  mobileInfo);
            return str;
        }

        private byte[] Vehicle_St1St2St3St4(string _ID, byte ST, byte A, byte B, byte C, byte D, byte Did, out string StatusStr, ref MdtWrapper mobileInfo)
        {
            StatusStr = "";
            byte[] buffer = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            if ((ST & 0x80) == 0x80)
            {
                buffer[2] = 1;
            }
            if ((ST & 0x60) == 0x60)
            {
                StatusStr = "GPS正常、";
            }
            else if ((ST & 0x40) == 0x40)
            {
                if (mobileInfo.MobileType != 0xcd)
                {
                    StatusStr = "GPS天线短路、";
                    buffer[2] = (byte)(buffer[2] + 2);
                }
            }
            else if ((ST & 0x20) == 0x20)
            {
                if (mobileInfo.MobileType != 0xcd)
                {
                    StatusStr = StatusStr + "GPS天线开路、";
                    buffer[2] = (byte)(buffer[2] + 4);
                }
            }
            else if ((ST & 0x60) == 0)
            {
                StatusStr = StatusStr + "GPS天线故障、";
                buffer[2] = (byte)(buffer[2] + 8);
            }
            if ((ST & 0x18) != 0x18)
            {
                if ((ST & 0x10) == 0x10)
                {
                    StatusStr = StatusStr + "主电源断开、";
                    buffer[0] = (byte)(buffer[0] + 0x40);
                }
                else if ((ST & 8) == 8)
                {
                }
            }
            if ((A & 0x80) == 0x80)
            {
                StatusStr = StatusStr + "汽车熄火、";
            }
            else
            {
                StatusStr = StatusStr + "汽车点火、";
                buffer[1] = (byte)(buffer[1] + 0x20);
            }
            if ((A & 4) == 4)
            {
                StatusStr = StatusStr + "油路正常、";
            }
            else
            {
                StatusStr = StatusStr + "油路断开、";
                buffer[0] = (byte)(buffer[0] + 8);
            }
            if ((B & 0x80) == 0)
            {
                StatusStr = StatusStr + "紧急报警、";
                buffer[0] = (byte)(buffer[0] + 4);
            }
            if ((B & 0x40) == 0)
            {
                StatusStr = StatusStr + "超速、";
                buffer[0] = (byte)(buffer[0] + 0x10);
            }
            if ((B & 0x10) == 0)
            {
                StatusStr = StatusStr + "禁止使出区域、";
                buffer[2] = (byte)(buffer[2] + 0x20);
            }
            if ((B & 8) == 0)
            {
                StatusStr = StatusStr + "禁止使入区域、";
                buffer[2] = (byte)(buffer[2] + 0x40);
            }
            StatusStr = StatusStr + "@";
            StatusStr = StatusStr.Replace("、@", "");
            StatusStr = StatusStr.Replace("@", "");
            return buffer;
        }
    }
}