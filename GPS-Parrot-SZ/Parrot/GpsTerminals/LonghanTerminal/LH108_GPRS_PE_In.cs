using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot.Models;
using System.Diagnostics;

namespace Parrot.Models.Longhan
{
    public class LH108_GPRS_PE_In
    {
        // Fields
        private int LogID;
        private int nDirection;
        private double nSpeed;
        private byte[] ReturnCmdByte = new byte[10];

        // Events
        public event PlainMessageReceivedEventHandler RecivEven;

        public event PlainGpsDataReceivedEventHandler RecivGpsData;

        public event GpsDataReturnEventHandler RecivGpsDataReturn;
        
        // Methods
        public LH108_GPRS_PE_In()
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
                case 0x70:
                    str = "ACC关定时回传间隔";
                    break;

                case 0x7a:
                    str = "TCP心跳间隔";
                    break;

                case 0x30:
                    str = "单次呼叫";
                    break;

                case 0x31:
                    str = "状态查询";
                    break;

                case 50:
                    str = "终端关机复位";
                    break;

                case 0x34:
                    str = "ACC开时定时回传间隔";
                    break;

                case 0x37:
                    str = "取消报警";
                    break;

                case 0x38:
                    str = "恢复油路";
                    break;

                case 0x39:
                    str = "关闭油路";
                    break;

                case 0x3a:
                    str = "设置省电或输出控制方式已完成";
                    break;

                case 0x3d:
                    str = "查询软件版本";
                    break;

                case 0x3f:
                    str = "设置超速报警";
                    break;

                case 0x40:
                    str = "设置停车超时报警";
                    break;

                case 70:
                    str = "下载电子围栏";
                    break;

                case 0x47:
                    str = "取消电子围栏";
                    break;

                case 0x48:
                    str = "查询电子围栏";
                    break;

                case 0x66:
                    str = "清除里程";
                    break;
            }
            str = str + "指令成功";
            mobileInfo.LastCmdReTurn = str;
            this.RecivEven(_ID, 2, "控制回复", "执行" + str,  mobileInfo);
        }

        private void EP_Pack(string _ID, ref byte[] body, ref MdtWrapper mobileInfo)
        {
            this.EP_Pack_Pro(_ID, ref body, ref mobileInfo);
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
            double num = (((body[0x1c] * 0x100) * 0x100) + (body[0x1d] * 0x100)) + body[30];
            num /= 100.0;
            string str = num.ToString("0").PadLeft(8, '0');
            for (int i = 0; i < 4; i++)
            {
                array[0x12 + i] = Convert.ToByte(str.Substring(i * 2, 2), 0x10);
            }
            string statusStr = "";
            byte[] buffer2 = null;
            buffer2 = this.Vehicle_St1St2St3St4(_ID, body[0x1b], body[0x1f], body[0x20], body[0x21], body[0x22], out statusStr, ref mobileInfo);
            buffer2[0] = (byte)((buffer2[0] + 1) + 2);
            buffer2.CopyTo(array, 0x16);
            if (this.RecivGpsData != null)
            {
                this.RecivGpsData(_ID,  array,   mobileInfo);
            }
        }

        public void GprsDataIn_(string _ID, int Car_Type, byte[] body, ref MdtWrapper mobileInfo)
        {
            this.LogID++;
            if (this.LogID > 0xf4240)
            {
                this.LogID = 0;
            }
            switch (body[2])
            {
                case 0xb2:
                    {
                        string s = Encoding.Default.GetString(body, 9, ((body[3] * 0x100) + body[4]) - 6);
                        s = Convert.ToBase64String(Encoding.Default.GetBytes(s));
                        this.RecivEven(_ID, 11, "主机版本", s,  mobileInfo);
                        goto Label_018E;
                    }
                case 0xb3:
                    {
                        string str = Encoding.Default.GetString(body, 9, ((body[3] * 0x100) + body[4]) - 6);
                        str = Convert.ToBase64String(Encoding.Default.GetBytes(str));
                        this.RecivEven(_ID, 11, "主机工作状态", str,  mobileInfo);
                        goto Label_018E;
                    }
                case 0xff:
                    goto Label_018E;

                case 0x9a:
                    this.ReturnEnclosure_16(_ID, body, ref mobileInfo);
                    goto Label_018E;

                case 0x80:
                    break;

                case 0x81:
                    this.RecivEven(_ID, 2, "控制回复", "终端点名回应",  mobileInfo);
                    break;

                case 0x85:
                    {
                        if (body.Length > 0x10)
                        {
                            this.CmdReport(_ID, body[body.Length - 3], 0, 0, ref mobileInfo);
                            break;
                        }
                        byte num = body[9];
                        byte num2 = body[10];
                        byte num3 = body[11];
                        this.CmdReport(_ID, num, num2, num3, ref mobileInfo);
                        return;
                    }
                default:
                    {
                        string str3 = "";
                        for (int i = 0; i < body.Length; i++)
                        {
                            str3 = str3 + " " + body[i].ToString("X2");
                        }
                        return;
                    }
            }
            this.EP_Pack(_ID, ref body, ref mobileInfo);
        Label_018E:
            this.ReturnCmdByte[5] = body[body.Length - 2];
            this.ReturnCmdByte[6] = body[2];
            this.ReturnCmdByte[7] = body[9];
            this.ReturnCmdByte[8] = LonghanWrapper.Get_CheckXor(ref this.ReturnCmdByte, 8);
            this.RecivGpsDataReturn(_ID, Convert.ToBase64String(this.ReturnCmdByte, 0, 10),  mobileInfo, 1);
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
            this.RecivEven(_ID, 2, "终端返回电子围栏信息", str,  mobileInfo);
        }

        private byte[] Vehicle_St1St2St3St4(string _ID, byte ST, byte A, byte B, byte C, byte D, out string StatusStr, ref MdtWrapper mobileInfo)
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
            if (((ST & 0x18) != 0x18) && ((ST & 0x10) == 0x10))
            {
                StatusStr = StatusStr + "主电源断开、";
                buffer[0] = (byte)(buffer[0] + 0x40);
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
            if ((A & 0x40) != 0x40)
            {
            }
            if ((A & 0x10) == 0x10)
            {
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
            if ((B & 0x20) == 0)
            {
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