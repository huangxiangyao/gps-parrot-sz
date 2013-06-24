using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Timers;
using System.Diagnostics;
using Parrot.Models;

namespace Parrot.Models.Yelang
{
   /// <summary>
   /// *HQ
   /// </summary>
    public class YelangIn
    {
        // Fields
        private byte[] CmdByte = new byte[0x12];
        private int MsgID;
        private int nDirection;
        private int nMsgType;
        private double nSpeed;
        public static Hashtable RecvImage_Hash;
        private string wDate = "";

        // Events
        public event PlainMessageReceivedEventHandler PlainMessageReceived;

        public event PlainGpsDataReceivedEventHandler PlainGpsDataReceived;

        public event GpsDataReturnEventHandler RecivGpsDataReturn;

        // Methods
        public YelangIn()
        {
            this.CmdByte[0] = 0x59;
            this.CmdByte[1] = 0x47;
            this.CmdByte[2] = 0x30;
            this.CmdByte[0x11] = 13;
        }

        public void _DataIn(ref string _ID, int _Type, ref byte[] body, ref MdtWrapper mobileInfo)
        {
            try
            {
                this.MsgID++;
                if (this.MsgID > 0xffff)
                {
                    this.MsgID = 0;
                }
                this.nMsgType = (body[9] * 0x100) + body[10];
                switch (this.nMsgType)
                {
                    case 0xff:
                    case 0:
                        goto Label_01A6;

                    case 0x100:
                        this.CmdReport(_ID, (body[13] * 0x100) + body[14], body[15], 0, ref mobileInfo);
                        return;

                    case 0x201:
                        this.EP_Pack(_ID, _Type, ref body, 0, 13, ref mobileInfo);
                        goto Label_01A6;

                    case 0x202:
                        this.EP_Pack(_ID, _Type, ref body, 0, 15, ref mobileInfo);
                        goto Label_01A6;

                    case 0x203:
                        this.EP_Pack(_ID, _Type, ref body, 0, 15, ref mobileInfo);
                        goto Label_01A6;

                    case 0x204:
                    case 0x303:
                    case 0x304:
                        return;

                    case 0x205:
                        if (body[13] != 0)
                        {
                            break;
                        }
                        this.PlainMessageReceived(_ID, 2, "控制回复", "终端已恢复油电",  mobileInfo);
                        goto Label_01A6;

                    case 0x301:
                        this.EP_Pack(_ID, _Type, ref body, 0, 13, ref mobileInfo);
                        goto Label_01A6;

                    case 770:
                        this.EP_Pack(_ID, _Type, ref body, 0, 13, ref mobileInfo);
                        goto Label_01A6;

                    case 0x305:
                        this.EP_Pack(_ID, _Type, ref body, 0, 15, ref mobileInfo);
                        goto Label_01A6;

                    case 0x801:
                        this.EP_Pack(_ID, _Type, ref body, 0, 0x11, ref mobileInfo);
                        goto Label_01A6;

                    default:
                        return;
                }
                this.PlainMessageReceived(_ID, 2, "控制回复", "终端已断开油电",  mobileInfo);
            Label_01A6:
                this.CmdByte[3] = body[3];
                this.CmdByte[4] = body[4];
                this.CmdByte[5] = body[5];
                this.CmdByte[6] = body[6];
                this.CmdByte[7] = body[7];
                this.CmdByte[8] = body[8];
                this.CmdByte[9] = 0x11;
                this.CmdByte[10] = 0;
                this.CmdByte[11] = 0;
                this.CmdByte[12] = 3;
                this.CmdByte[13] = body[9];
                this.CmdByte[14] = body[10];
                this.CmdByte[15] = 0;
                this.CmdByte[0x10] = this.Get_CheckXor(ref this.CmdByte, 0x10);
                this.CmdByte[0x11] = 13;
                this.RecivGpsDataReturn(_ID, Convert.ToBase64String(this.CmdByte, 0, 0x12),  mobileInfo, 1);
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Error:----------" + exception.ToString());
            }
        }

        private void AutoReciveImage(bool _Now)
        {
        }

        private void CmdReport(string _ID, int M_Cmd_Id, byte C_Cmd_Id, byte S_Fa, ref MdtWrapper mobileInfo)
        {
            string str = "";
            switch (M_Cmd_Id)
            {
                case 0x1201:
                    str = "设置服务器IP和端口号";
                    break;

                case 0x1203:
                    str = "设置服务中心号码";
                    break;

                case 0x1205:
                    str = "设置本机号码";
                    break;

                case 0x1206:
                    str = "设置预解警密码";
                    break;

                case 0x1207:
                    str = "设置网络握手的时间间隔";
                    break;

                case 0x1208:
                    str = "设置点火状态下定位数据上传的时间间隔";
                    break;

                case 0x1209:
                    str = "设置熄火状态下定位数据上传的时间间隔";
                    break;

                case 0x120a:
                    str = "设置ACC省电时间";
                    break;

                case 0x120d:
                    str = "设置一键通电话号码";
                    break;

                case 0x120e:
                    str = "设置第一设防电话号码";
                    break;

                case 0x120f:
                    str = "设置第二设防电话号码";
                    break;

                case 0x12f0:
                    str = "设置特权服务中心号";
                    break;

                case 0x12f1:
                    str = "设置APN参数";
                    break;

                case 0x1404:
                    str = "监听指令";
                    break;

                case 0x1405:
                    if (C_Cmd_Id != 0)
                    {
                        str = "断开油电指令已收到";
                    }
                    else
                    {
                        str = "恢复油电指令已收到";
                    }
                    goto Label_01CD;

                case 0x1408:
                    str = "停车超时报警设置";
                    break;

                case 0x1409:
                    str = "超速报警设置";
                    break;

                case 0x1410:
                    str = "报警清除指令";
                    break;

                case 0x1411:
                    if (C_Cmd_Id != 0)
                    {
                        str = "进入预警指令已收到";
                    }
                    else
                    {
                        str = "解除预警指令已收到";
                    }
                    goto Label_01CD;

                case 0x1412:
                    if (C_Cmd_Id != 0)
                    {
                        str = "进入自动预警指令已收到";
                    }
                    else
                    {
                        str = "进入人工预警指令已收到";
                    }
                    goto Label_01CD;

                case 0x1420:
                    str = "重新启动";
                    break;

                case 0x1421:
                    str = "恢复初始设置";
                    break;

                case 0xffff:
                    break;

                default:
                    goto Label_01CD;
            }
            if (C_Cmd_Id == 0)
            {
                str = str + "指令成功";
            }
            else
            {
                str = str + "指令失败";
            }
        Label_01CD:
            mobileInfo.LastCmdReTurn = str;
            this.PlainMessageReceived(_ID, 2, "控制回复", "执行" + str,  mobileInfo);
        }

        public void DataIn(string _ID, int _Type,  byte[] body, int ByteLen,  MdtWrapper mobileInfo)
        {
            this._DataIn(ref _ID, _Type, ref body, ref mobileInfo);
        }

        private int EP_Pack(string _ID, int _Type, ref byte[] Body, int DataType, int StarIndex, ref MdtWrapper mobileInfo)
        {
            byte[] array = new byte[30];
            this.wDate = "20" + Body[StarIndex].ToString("X2") + "-" + Body[StarIndex + 1].ToString("X2") + "-" + Body[StarIndex + 2].ToString("X2") + " " + Body[StarIndex + 3].ToString("X2") + ":" + Body[StarIndex + 4].ToString("X2") + ":" + Body[StarIndex + 5].ToString("X2");
            try
            {
                this.wDate = DateTime.Parse(this.wDate).AddHours(8.0).ToString("yy:MM:dd:HH:mm:ss");
            }
            catch
            {
                this.wDate = DateTime.Now.ToString("yy:MM:dd:HH:mm:ss");
            }
            string[] strArray = this.wDate.Split(new char[] { ':' });
            array[0] = Convert.ToByte(strArray[0], 0x10);
            array[1] = Convert.ToByte(strArray[1], 0x10);
            array[2] = Convert.ToByte(strArray[2], 0x10);
            array[3] = Convert.ToByte(strArray[3], 0x10);
            array[4] = Convert.ToByte(strArray[4], 0x10);
            array[5] = Convert.ToByte(strArray[5], 0x10);
            array[6] = Body[StarIndex + 10];
            array[7] = Body[StarIndex + 11];
            array[8] = Body[StarIndex + 12];
            array[9] = Body[StarIndex + 13];
            array[10] = Body[StarIndex + 6];
            array[11] = Body[StarIndex + 7];
            array[12] = Body[StarIndex + 8];
            array[13] = Body[StarIndex + 9];
            this.nSpeed = Body[StarIndex + 15];
            array[14] = (byte)this.nSpeed;
            this.nDirection = Body[StarIndex + 0x10];
            array[15] = (byte)this.nDirection;
            array[0x10] = 0;
            array[0x11] = 0;
            double num = (((((Body[StarIndex + 0x15] * 0x100) * 0x100) * 0x100) + ((Body[StarIndex + 0x16] * 0x100) * 0x100)) + (Body[StarIndex + 0x17] * 0x100)) + Body[StarIndex + 0x18];
            num /= 100.0;
            string str = num.ToString("0").PadLeft(8, '0');
            for (int i = 0; i < 4; i++)
            {
                array[0x12 + i] = Convert.ToByte(str.Substring(i * 2, 2), 0x10);
            }
            string statusStr = "";
            byte[] buffer2 = this.Vehicle_St1St2St3St4(_ID, Body[StarIndex + 0x11], Body[StarIndex + 0x12], Body[StarIndex + 0x13], Body[StarIndex + 20], out statusStr, ref mobileInfo);
            if ((Body[StarIndex + 14] & 1) == 0)
            {
                buffer2[2] = (byte)(buffer2[2] + 1);
            }
            buffer2[0] = (byte)((buffer2[0] + 1) + 2);
            buffer2.CopyTo(array, 0x16);
            if (this.PlainGpsDataReceived != null)
            {
                this.PlainGpsDataReceived(_ID,  array,   mobileInfo);
            }
            return 1;
        }

        private byte Get_CheckXor(ref byte[] temp, int len)
        {
            byte num = (byte)(temp[0] ^ temp[1]);
            for (int i = 2; i < len; i++)
            {
                num = (byte)(num ^ temp[i]);
            }
            return num;
        }

        private void OnTimedEvent_Img(object source, ElapsedEventArgs e)
        {
        }

        private byte[] Vehicle_St1St2St3St4(string _ID, byte A, byte B, byte C, byte D, out string StatusStr, ref MdtWrapper mobileInfo)
        {
            StatusStr = "";
            byte[] buffer = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            if ((A & 1) == 1)
            {
                StatusStr = StatusStr + "车门开、";
                buffer[1] = (byte)(buffer[1] + 2);
            }
            else
            {
                StatusStr = StatusStr + "车门关、";
            }
            if ((A & 2) == 2)
            {
                StatusStr = StatusStr + "设防、";
            }
            else
            {
                StatusStr = StatusStr + "撤防、";
            }
            if ((A & 4) == 4)
            {
                StatusStr = StatusStr + "ACC开、";
                buffer[1] = (byte)(buffer[1] + 0x20);
            }
            else
            {
                StatusStr = StatusStr + "ACC关、";
            }
            if ((A & 8) == 8)
            {
            }
            if ((A & 0x10) == 0x10)
            {
                StatusStr = StatusStr + "低电压、";
            }
            if ((A & 0x20) == 0x20)
            {
                StatusStr = StatusStr + "已设置超速报警、";
            }
            if ((A & 0x40) == 0x40)
            {
                StatusStr = StatusStr + "已设置区域报警、";
            }
            if ((A & 0x80) == 0x80)
            {
                StatusStr = StatusStr + "已断油电、";
                buffer[0] = (byte)(buffer[0] + 8);
            }
            if ((B & 1) == 1)
            {
                StatusStr = StatusStr + "屏蔽上传报警、";
            }
            if ((B & 2) == 2)
            {
                StatusStr = StatusStr + "长时间不定位、";
                buffer[2] = (byte)(buffer[2] + 8);
            }
            if ((C & 1) == 1)
            {
                StatusStr = StatusStr + "盗警、";
            }
            if ((C & 2) == 2)
            {
                StatusStr = StatusStr + "紧急报警、";
                buffer[0] = (byte)(buffer[0] + 4);
            }
            if ((C & 4) == 4)
            {
                StatusStr = StatusStr + "超速、";
                buffer[0] = (byte)(buffer[0] + 0x10);
            }
            if ((C & 8) == 8)
            {
                StatusStr = StatusStr + "车门非法打开、";
            }
            if ((C & 0x10) == 0x10)
            {
                StatusStr = StatusStr + "主电源断开、";
                buffer[0] = (byte)(buffer[0] + 0x40);
            }
            if ((C & 0x20) == 0x20)
            {
                StatusStr = StatusStr + "GPS天线开路、";
                buffer[2] = (byte)(buffer[2] + 4);
            }
            if ((C & 0x40) == 0x40)
            {
                StatusStr = StatusStr + "非法点火、";
            }
            if ((C & 0x80) == 0x80)
            {
                StatusStr = StatusStr + "密码错误、";
            }
            if ((D & 1) == 1)
            {
                StatusStr = StatusStr + "进区报警、";
                buffer[2] = (byte)(buffer[2] + 0x40);
            }
            if ((D & 2) == 2)
            {
                StatusStr = StatusStr + "出区报警、";
                buffer[2] = (byte)(buffer[2] + 0x20);
            }
            if ((D & 4) == 4)
            {
                StatusStr = StatusStr + "停车时间超长、";
            }
            if ((D & 8) == 8)
            {
                StatusStr = StatusStr + "未设防报警、";
            }
            StatusStr = StatusStr + "@";
            StatusStr = StatusStr.Replace("、@", "");
            StatusStr = StatusStr.Replace("@", "");
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