using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Parrot.Models;

namespace Parrot.Models.Longhan
{
    public class LH101G_GPRS_PE_In
    {
        // Fields
        private int LogID;
        private int nDirection;
        private double nSpeed;
        private byte[] ReturnCmdByte = new byte[10];
        private string v = "";

        // Events
        public event PlainMessageReceivedEventHandler RecivEven;

        public event PlainGpsDataReceivedEventHandler RecivGpsData;

        public event GpsDataReturnEventHandler RecivGpsDataReturn;

        // Methods
        public LH101G_GPRS_PE_In()
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
            this.v = "";
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
            this.RecivEven(_ID, 2, "控制回复", "执行" + str, mobileInfo);
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
            this.EP_Pack_Nol(_ID, ref body, ref mobileInfo);
        }

        private bool EP_Pack_Nol(string _ID, ref byte[] body, ref MdtWrapper mobileInfo)
        {
            if (body[2] == 0x8e)
            {
                return false;
            }
            if ((body[0x1b] & 0x80) == 0x80)
            {
                this.v = "1";
            }
            else
            {
                this.v = "0";
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
            this.nSpeed *= 1.852;
            this.nSpeed = Math.Round(this.nSpeed, 1);
            array[14] = (byte)this.nSpeed;
            this.nDirection = (((body[0x19] % 0x10) * 100) + ((body[0x1a] / 0x10) * 10)) + (body[0x1a] % 0x10);
            array[15] = (byte)(this.nDirection / 2);
            array[0x10] = 0;
            array[0x11] = 0;
            array[0x12] = 0;
            array[0x13] = 0;
            array[20] = 0;
            array[0x15] = 0;
            string statusStr = "";
            byte[] buffer2 = null;
            if (body[2] == 130)
            {
                buffer2 = this.GetAlarmStatus_12(_ID, body[30], body[0x1f], out statusStr, ref mobileInfo);
            }
            else if (body[2] == 0x80)
            {
                buffer2 = this.Vehicle_Status_Nol(_ID, body[0x1c], body[0x1d], out statusStr, ref mobileInfo);
            }
            if (this.v == "1")
            {
                buffer2[2] = (byte)(buffer2[2] + 1);
            }
            buffer2[0] = (byte)((buffer2[0] + 1) + 2);
            buffer2.CopyTo(array, 0x16);
            if (this.RecivGpsData != null)
            {
                this.RecivGpsData(_ID,  array,   mobileInfo);
            }
            return true;
        }

        private byte[] GetAlarmStatus_12(string _ID, byte A, byte B, out string StatusStr, ref MdtWrapper mobileInfo)
        {
            string str = "";
            byte[] buffer = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            if ((A & 2) == 2)
            {
                str = str + "入区域报警、";
                buffer[2] = (byte)(buffer[2] + 0x40);
            }
            if ((A & 4) == 4)
            {
                str = str + "出区域报警、";
                buffer[2] = (byte)(buffer[2] + 0x20);
            }
            if ((A & 0x20) == 0x20)
            {
                str = str + "GPS接收机故障、";
                buffer[2] = (byte)(buffer[2] + 8);
            }
            if ((B & 0x40) == 0x40)
            {
            }
            if ((B & 0x20) == 0x20)
            {
                str = str + "振动报警、";
                buffer[1] = (byte)(buffer[1] + 0x20);
            }
            if ((B & 8) == 8)
            {
                str = str + "主电源断开、";
                buffer[1] = (byte)(buffer[1] + 0x40);
            }
            if ((B & 2) == 2)
            {
                str = str + "用户车辆超速行驶中";
                buffer[1] = (byte)(buffer[1] + 0x10);
            }
            if ((B & 1) == 1)
            {
                str = str + "用户遇劫警或紧急求助、";
                buffer[1] = (byte)(buffer[1] + 4);
            }
            StatusStr = str + "@";
            str = str.Replace("、@", "").Replace("@", "");
            return buffer;
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
            if (body[0x17] == 0)
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
            this.RecivEven(_ID, 2, "终端设置状态", str,  mobileInfo);
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
                case 0xb1:
                case 0xff:
                    goto Label_0132;

                case 0xb2:
                    {
                        string str = Encoding.Default.GetString(body, 9, body[4] - 6);
                        this.RecivEven(_ID, 2, "主机版本", str,  mobileInfo);
                        goto Label_0132;
                    }
                case 0x80:
                case 0x81:
                    break;

                case 130:
                    this.EP_Pack(_ID, ref body, ref mobileInfo);
                    goto Label_0132;

                case 0x83:
                    this.GetStatus(_ID, body, ref mobileInfo);
                    goto Label_0132;

                case 0x85:
                    if (body[4] != 40)
                    {
                        if (!((body[4] == 0x2c) | (body[4] == 11)))
                        {
                            return;
                        }
                        this.CmdReport(_ID, body[body.Length - 7], 0, 0, ref mobileInfo);
                        break;
                    }
                    this.CmdReport(_ID, body[body.Length - 3], 0, 0, ref mobileInfo);
                    break;

                case 0x8e:
                    this.EP_Pack(_ID, ref body, ref mobileInfo);
                    goto Label_0132;

                default:
                    {
                        string str2 = "";
                        for (int i = 0; i < body.Length; i++)
                        {
                            str2 = str2 + " " + body[i].ToString("X2");
                        }
                        return;
                    }
            }
            this.EP_Pack(_ID, ref body, ref mobileInfo);
        Label_0132:
            this.ReturnCmdByte[5] = body[body.Length - 2];
            this.ReturnCmdByte[6] = body[2];
            this.ReturnCmdByte[7] = body[9];
            this.ReturnCmdByte[8] = LonghanWrapper.Get_CheckXor(ref this.ReturnCmdByte, 8);
            this.RecivGpsDataReturn(_ID, Convert.ToBase64String(this.ReturnCmdByte, 0, 10),  mobileInfo, 1);
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

        private void ReturnEnclosure_12(string _ID, byte[] data, ref MdtWrapper mobileInfo)
        {
            string str2 = string.Empty;
            string str = str2 + "总共围栏个数：" + data[9].ToString() + "、所查询的围栏序号为：" + data[10].ToString() + "围栏内容：";
            double num = 0.0;
            double num2 = 0.0;
            num = double.Parse(data[12].ToString("X2").Remove(0, 1) + data[13].ToString("X2") + data[14].ToString("X2")) / 60000.0;
            num2 = double.Parse(data[11].ToString("X2").Remove(0, 1) + data[12].ToString("X2").Substring(0, 1));
            str = str + " 小纬=" + ((num + num2)).ToString("00.00000");
            num = double.Parse(data[0x10].ToString("X2").Remove(0, 1) + data[0x11].ToString("X2") + data[0x12].ToString("X2")) / 60000.0;
            num2 = double.Parse(data[15].ToString("X2") + data[0x10].ToString("X2").Substring(0, 1));
            str = str + " 小经=" + ((num + num2)).ToString("00.00000");
            num = double.Parse(data[20].ToString("X2").Remove(0, 1) + data[0x15].ToString("X2") + data[0x16].ToString("X2")) / 60000.0;
            num2 = double.Parse(data[0x13].ToString("X2").Remove(0, 1) + data[20].ToString("X2").Substring(0, 1));
            str = str + " 大纬=" + ((num + num2)).ToString("00.00000");
            num = double.Parse(data[0x18].ToString("X2").Remove(0, 1) + data[0x19].ToString("X2") + data[0x1a].ToString("X2")) / 60000.0;
            num2 = double.Parse(data[0x17].ToString("X2") + data[0x18].ToString("X2").Substring(0, 1));
            str = (str + " 大经=" + ((num + num2)).ToString("00.00000")) + "、围栏号：" + data[0x1b].ToString();
            switch (data[0x1c])
            {
                case 0:
                    str = str + "、围栏报警方式：表示驶出围栏报警";
                    break;

                case 1:
                    str = str + "、围栏报警方式：表示驶入围栏报警";
                    break;

                case 2:
                    str = str + "、围栏报警方式：表示驶入驶出围栏报警";
                    break;

                case 3:
                    str = str + "、围栏报警方式：表示禁止围栏报警";
                    break;
            }
            this.RecivEven(_ID, 2, "终端返回电子围栏信息", str,  mobileInfo);
        }

        private byte[] Vehicle_Status_Nol(string _ID, byte A, byte B, out string StatusStr, ref MdtWrapper mobileInfo)
        {
            StatusStr = "";
            byte[] buffer = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            if ((A & 0x80) == 0x80)
            {
                StatusStr = "ACC开、";
                buffer[1] = (byte)(buffer[1] + 0x20);
            }
            else
            {
                StatusStr = "ACC关、";
            }
            if ((A & 0x40) == 0x40)
            {
                StatusStr = StatusStr + "车门开、";
                buffer[1] = 2;
            }
            else
            {
                StatusStr = StatusStr + "车门关、";
            }
            if ((A & 0x20) == 0x20)
            {
                StatusStr = StatusStr + "油路已断、";
                buffer[0] = (byte)(buffer[0] + 8);
            }
            if ((A & 8) == 8)
            {
                StatusStr = StatusStr + "设防、";
            }
            else
            {
                StatusStr = StatusStr + "撤防、";
            }
            if ((A & 2) == 2)
            {
                StatusStr = StatusStr + "盗警开启、";
            }
            if ((A & 1) == 1)
            {
                StatusStr = StatusStr + "劫警开启、";
            }
            StatusStr = StatusStr + "@";
            StatusStr = StatusStr.Replace("、@", "");
            StatusStr = StatusStr.Replace("@", "");
            return buffer;
        }
    }

}