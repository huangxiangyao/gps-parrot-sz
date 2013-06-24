using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot.Models;

namespace Parrot.Models.FortyGprs
{
    public class FortyGprsIn
    {
        // Fields
        private string dwLatitude;
        private string dwLongitude;
        private string nDirection;
        private string nSpeed;
        private string v = "0";
        private string wDate = "";

        // Events
        public event GpsAlarmReceivedEventHandler RecivAlarmEven;

        public event PlainMessageReceivedEventHandler RecivEven;

        public event Eve_RecivGpsData2 RecivGpsData;

        // Methods
        public void DataIn(string _ID, int _Type, ref byte[] _body, int ByteLen, ref MdtWrapper mobileInfo)
        {
            byte[] buffer = null;
            if (_body[3] < 0x30)
            {
                int index = 0;
                buffer = new byte[_body.Length];
                for (int i = 0; i < _body.Length; i++)
                {
                    if (_body[i] == 0x24)
                    {
                        buffer[index] = (byte)(_body[i] + _body[i + 1]);
                        i++;
                    }
                    else
                    {
                        buffer[index] = _body[i];
                    }
                    index++;
                }
                if ((buffer[10] == 0x40) & (buffer[11] == 1))
                {
                    string str = "正常";
                    this.wDate = "20" + buffer[12].ToString("00") + "-" + buffer[13].ToString("00") + "-" + buffer[14].ToString("00") + " " + buffer[15].ToString("00") + ":" + buffer[0x10].ToString("00") + ":" + buffer[0x11].ToString("00");
                    this.dwLongitude = buffer[0x12].ToString() + "." + buffer[0x13].ToString("00") + buffer[20].ToString("00") + buffer[0x15].ToString("00");
                    this.dwLatitude = buffer[0x16].ToString() + "." + buffer[0x17].ToString("00") + buffer[0x18].ToString("00") + buffer[0x19].ToString("00");
                    this.nSpeed = buffer[0x1a].ToString();
                    this.nDirection = (buffer[0x1b] * 2).ToString();
                    if (buffer[30] == 0x10)
                    {
                        this.v = "1";
                    }
                    if (buffer[0x1f] == 1)
                    {
                        str = "紧急报警";
                        this.RecivAlarmEven(_ID, 1, "报警", str, this.v, double.Parse(this.dwLongitude), double.Parse(this.dwLatitude), double.Parse(this.nSpeed), this.nDirection, this.wDate, mobileInfo);
                    }
                    if (this.RecivGpsData != null)
                    {
                        string uST = "";
                        this.RecivGpsData(_ID, ref this.v, double.Parse(this.dwLatitude), double.Parse(this.dwLongitude), double.Parse(this.nSpeed), int.Parse(this.nDirection), ref this.wDate, ref str, ref uST, ref mobileInfo, 0);
                    }
                }
            }
            else
            {
                string str3 = Encoding.Default.GetString(_body);
                byte[] buffer2 = new byte[str3.Length / 2];
                for (int j = 0; j < (str3.Length / 2); j++)
                {
                    string str4 = str3.Substring(j * 2, 2);
                    buffer2[j] = Convert.ToByte(str4, 0x10);
                }
                buffer = new byte[buffer2.Length];
                int num4 = 0;
                for (int k = 0; k < buffer2.Length; k++)
                {
                    if (buffer2[k] == 0x24)
                    {
                        buffer[num4] = (byte)(buffer2[k] + buffer2[k + 1]);
                        k++;
                    }
                    else
                    {
                        buffer[num4] = buffer2[k];
                    }
                    num4++;
                }
            }
            if ((buffer[4] == 0x40) & (buffer[5] == 1))
            {
                string str5 = "正常";
                this.wDate = "20" + buffer[6].ToString("00") + "-" + buffer[7].ToString("00") + "-" + buffer[8].ToString("00") + " " + buffer[9].ToString("00") + ":" + buffer[10].ToString("00") + ":" + buffer[11].ToString("00");
                this.dwLongitude = buffer[12].ToString() + "." + buffer[13].ToString("00") + buffer[14].ToString("00") + buffer[15].ToString("00");
                this.dwLatitude = buffer[0x10].ToString() + "." + buffer[0x11].ToString("00") + buffer[0x12].ToString("00") + buffer[0x13].ToString("00");
                this.nSpeed = buffer[20].ToString();
                this.nDirection = (buffer[0x15] * 2).ToString();
                if (buffer[0x18] == 0x10)
                {
                    this.v = "1";
                }
                if (buffer[0x19] == 1)
                {
                    str5 = "紧急报警";
                    this.RecivAlarmEven(_ID, 1, "报警", str5, this.v, double.Parse(this.dwLongitude), double.Parse(this.dwLatitude), double.Parse(this.nSpeed), this.nDirection, this.wDate, mobileInfo);
                }
                if (this.RecivGpsData != null)
                {
                    string str6 = "";
                    this.RecivGpsData(_ID, ref this.v, double.Parse(this.dwLatitude), double.Parse(this.dwLongitude), double.Parse(this.nSpeed), int.Parse(this.nDirection), ref this.wDate, ref str5, ref str6, ref mobileInfo, 0);
                }
            }
            else if ((buffer[4] == 0x41) & (buffer[5] == 1))
            {
                string str7 = "";
                if ((buffer[6] == 0) & (buffer[7] == 1))
                {
                    str7 = "设置终端ID";
                }
                else if ((buffer[6] == 0) & (buffer[7] == 2))
                {
                    str7 = "设置短信中心号码";
                }
                else if ((buffer[6] == 0) & (buffer[7] == 3))
                {
                    str7 = "设置IP端口";
                }
                else if ((buffer[6] == 0) & (buffer[7] == 4))
                {
                    str7 = "设置GPRS定时定位间隔";
                }
                else if ((buffer[6] == 0) & (buffer[7] == 5))
                {
                    str7 = "设置监控中心求助号码";
                }
                else if ((buffer[6] == 1) & (buffer[7] == 1))
                {
                    str7 = "点名";
                }
                else if ((buffer[6] == 1) & (buffer[7] == 2))
                {
                    str7 = "实时跟踪";
                }
                else if ((buffer[6] == 1) & (buffer[7] == 0x12))
                {
                    str7 = "停止跟踪";
                }
                else if ((buffer[6] == 1) & (buffer[7] == 3))
                {
                    str7 = "查询版本信息";
                }
                else if ((buffer[6] == 1) & (buffer[7] == 4))
                {
                    str7 = "停止报警";
                }
                else if ((buffer[6] == 2) & (buffer[7] == 1))
                {
                    str7 = "位置汇报确认";
                }
                if (buffer[8] == 1)
                {
                    str7 = str7 + "成功";
                }
                else
                {
                    str7 = str7 + "失败";
                }
                this.RecivEven(_ID, 2, "指令回复", str7, mobileInfo);
            }
            else if (!((buffer[4] == 0x41) & (buffer[5] == 2)) && ((buffer[4] == 0x40) & (buffer[5] == 2)))
            {
            }
        }
    }


}
