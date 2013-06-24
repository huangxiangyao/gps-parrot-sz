using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot.Models;
using System.Diagnostics;

namespace Parrot.Models.CxGprs
{
    public class CxGprsIn
    {
        // Fields
        private string car_statu = "";
        private double dwLatitude;
        private double dwLongitude;
        private int LogID;
        private int nDirection;
        private double nSpeed;
        private string wDate = "";
        private string wTime = "";

        // Events
        public event GpsAlarmReceivedEventHandler RecivAlarmEven;

        public event PlainMessageReceivedEventHandler RecivEven;

        public event Eve_RecivGpsData2 RecivGpsData;

        public event GpsDataReturnEventHandler RecivGpsDataReturn;

        // Methods
        public CxGprsIn()
        {
            this.wDate = "";
            this.wTime = "";
            this.dwLatitude = 0.0;
            this.dwLongitude = 0.0;
            this.nSpeed = 0.0;
            this.nDirection = 0;
            this.car_statu = "";
        }

        private void Parse( string _ID, int _Type,  byte[] body,  MdtWrapper mobileInfo)
        {
            this.LogID++;
            if (this.LogID > 0xf4240)
            {
                this.LogID = 0;
            }
            byte num12 = body[1];
            switch (num12)
            {
                case 0x4c:
                case 0x54:
                    break;

                default:
                    {
                        if (num12 != 0x69)
                        {
                            this.CmdReport(_ID, body[1], ref mobileInfo);
                            break;
                        }
                        string str = body[2].ToString("0") + body[3].ToString("000") + body[4].ToString("000");
                        byte[] inArray = new byte[] { 0x80, 0x49, 0xff, 13 };
                        this.RecivGpsDataReturn(_ID, Convert.ToBase64String(inArray),  mobileInfo, 0);
                        return;
                    }
            }
            byte[] buffer2 = new byte[0x400];
            string message = "";
            byte[] buffer3 = new byte[body.Length + 8];
            for (int i = 0; i < body.Length; i++)
            {
                message = message + body[i].ToString("X2") + " ";
            }
            Debug.WriteLine(message);
            byte num2 = (byte)((body[0x1c] ^ 0x55) ^ 0x36);
            this.wDate = "";
            this.wTime = ((body[0x18] ^ 0x47) ^ num2).ToString("X2");
            this.wTime = this.wTime + ":" + (((body[0x19] ^ 0x47) ^ num2)).ToString("X2");
            this.wTime = this.wTime + ":" + (((body[0x1a] ^ 0x47) ^ num2)).ToString("X2");
            this.wDate = DateTime.Now.ToString("yyyy-MM-dd") + " " + this.wTime;
            this.wDate = DateTime.Parse(this.wDate).AddHours(8.0).ToString("yyyy-MM-dd HH:mm:ss");
            byte num3 = (byte)(body[5] ^ num2);
            string s = ((body[14] ^ num2)).ToString("X2") + ((body[15] ^ num2)).ToString("X2");
            this.nSpeed = double.Parse(s) * 0.1852;
            string str4 = ((body[0x10] ^ num2)).ToString("X2") + ((body[0x11] ^ num2)).ToString("X2");
            this.nDirection = int.Parse((double.Parse(str4) * 0.1).ToString("0"));
            byte num4 = (byte)((num2 & 240) | (((body[0x19] ^ 0x47) ^ num2) / 15));
            string str5 = (((body[6] ^ num4)).ToString("X2") + ((body[7] ^ num4)).ToString("X2")) + ((body[8] ^ num4)).ToString("X2") + ((body[9] ^ num4)).ToString("X2");
            string str6 = "";
            byte num5 = (byte)(((num2 % 0x10) * 0x10) | (((body[0x19] ^ 0x47) ^ num2) & 15));
            str6 = (((body[10] ^ num5)).ToString("X2") + ((body[11] ^ num5)).ToString("X2")) + ((body[12] ^ num5)).ToString("X2") + ((body[13] ^ num5)).ToString("X2");
            double num6 = 0.0;
            num6 = double.Parse(str5.Substring(4, 4)) / 10000.0;
            num6 = (num6 + double.Parse(str5.Substring(2, 2))) / 60.0;
            num6 += double.Parse(str5.Substring(0, 2));
            this.dwLatitude = num6;
            byte a = (byte)(body[0x1d] ^ num2);
            byte b = (byte)(body[30] ^ num2);
            byte c = (byte)(body[0x1f] ^ num2);
            byte d = (byte)(body[0x20] ^ num2);
            string str7 = "0";
            if ((b & 0x80) == 0x80)
            {
                str7 = "1";
            }
            double num11 = 0.0;
            num11 = double.Parse(str6.Substring(4, 4)) / 10000.0;
            num11 = double.Parse(str7 + str6.Substring(0, 2)) + ((num11 + double.Parse(str6.Substring(2, 2))) / 60.0);
            this.dwLongitude = num11;
            string str8 = this.Vehicle_St1St2St3St4(_ID, 0, a, b, c, d, this.dwLatitude, this.dwLongitude, this.nSpeed, this.nDirection, ref mobileInfo);
            string str9 = Convert.ToString(a, 2).PadLeft(8, '0') + "  " + Convert.ToString(b, 2).PadLeft(8, '0') + "  " + Convert.ToString(c, 2).PadLeft(8, '0') + "  " + Convert.ToString(d, 2).PadLeft(8, '0');
            string v = "1";
            string uST = "";
            if (this.RecivGpsData != null)
            {
                this.RecivGpsData(_ID, ref v, this.dwLatitude, this.dwLongitude, this.nSpeed, this.nDirection, ref this.wDate, ref this.car_statu, ref uST, ref mobileInfo, 0);
            }
            Debug.WriteLine(string.Concat(new object[] { "wDate=", this.wDate, "\r\nStatuByte=", num3.ToString("X2"), "\r\nnSpeed=", this.nSpeed, "\r\nnDirection=", this.nDirection, "\r\ndwLatitude=", this.dwLatitude, "\r\ndwLongitude=", this.dwLongitude, "\r\nStatuStr=", str9, "\r\n" }));
        }

        private void CmdReport(string _ID, byte M_Cmd_Id, ref MdtWrapper mobileInfo)
        {
            string str = "";
            switch (M_Cmd_Id)
            {
                case 0x41:
                    str = "监听";
                    break;

                case 0x42:
                    str = "枪毙车胎";
                    break;

                case 0x44:
                    str = "复活车胎";
                    break;

                case 0x4b:
                    str = "停止行驶(锁车)";
                    break;

                case 0x4d:
                    str = "发送短信息";
                    break;

                case 80:
                    str = "远程设防";
                    break;

                case 0x52:
                    str = "图像传输";
                    break;

                case 0x53:
                    str = "停止跟踪";
                    break;

                case 0x55:
                    str = "恢复行驶（解锁）";
                    break;

                case 0x58:
                    str = "设置电子围栏";
                    break;

                case 0x61:
                    str = "设置速度限制";
                    break;

                case 0x62:
                    str = "取消速度限制";
                    break;

                case 0x63:
                    str = "定时回传间隔";
                    break;

                case 0x68:
                    str = "取消电子围栏";
                    break;

                case 0x70:
                    str = "远程撤防";
                    break;

                case 0x72:
                    str = "取消图像传输";
                    break;
            }
            str = str + "指令成功";
            this.RecivEven(_ID, 2, "控制回复", "执行" + str,  mobileInfo);
        }

        public void Parse(string _ID, int _Type, byte[] body, int ByteLen, MdtWrapper mobileInfo)
        {
            this.Parse( _ID, _Type,  body,  mobileInfo);
        }

        private string Vehicle_St1St2St3St4(string _ID, byte ST, byte A, byte B, byte C, byte D, double _dwLatitude, double _dwLongitude, double _nSpeed, int _nDirectionn, ref MdtWrapper mobileInfo)
        {
            string str = "";
            bool flag = false;
            string str2 = "";
            string str3 = "";
            if ((A & 0x80) == 0x80)
            {
                str2 = str2 + "ACC开、";
            }
            else
            {
                str2 = str2 + "ACC关、";
            }
            if ((A & 0x40) == 0x40)
            {
                str2 = str2 + "紧急报警、";
                flag = true;
                str = str + "用户遇劫警或紧急求助、";
                str3 = str3 + "紧急报警、";
            }
            if ((A & 4) == 4)
            {
                str2 = str2 + "车右灯亮、";
            }
            if ((A & 2) == 2)
            {
                str2 = str2 + "车左灯亮、";
            }
            if ((A & 1) == 1)
            {
                str2 = str2 + "车大灯亮、";
            }
            if ((B & 0x40) == 0x40)
            {
                str2 = str2 + "状态检测屏蔽、";
            }
            if ((B & 0x20) == 0x20)
            {
                str2 = str2 + "GPS天线故障、";
            }
            if ((B & 0x10) == 0x10)
            {
                str2 = str2 + "车门开、";
            }
            else
            {
                str2 = str2 + "车门关、";
            }
            if ((B & 4) == 4)
            {
                str2 = str2 + "主电源掉电、";
                flag = true;
                str = str + "主电源掉电、";
                str3 = str3 + "故障报警、";
            }
            if ((B & 2) == 2)
            {
                str2 = str2 + "已断油电、";
            }
            if ((B & 1) == 1)
            {
                str2 = str2 + "报警器报警、";
                flag = true;
                str = str + "报警器报警、";
                str3 = str3 + "安防报警、";
            }
            if ((C & 1) == 1)
            {
                str2 = str2 + "远程设防、";
            }
            else
            {
                str2 = str2 + "远程辙防、";
            }
            if ((C & 2) == 2)
            {
                str2 = str2 + "本地设防";
            }
            str2 = str2 + "本地辙防";
            if (flag)
            {
                this.LogID++;
                this.RecivAlarmEven(_ID, 1, str3, str, mobileInfo.LastAV, mobileInfo.LastX, mobileInfo.LastY, mobileInfo.LastV, mobileInfo.LastF.ToString(), DateTime.Now.ToString(),  mobileInfo);
            }
            return str2;
        }
    }

}