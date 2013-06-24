using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Parrot.Models.Longhan
{
    public class LH108_GPRS_PE_Out
    {
        // Fields
        private byte[] CmdByte = new byte[0x800];

        // Methods
        public LH108_GPRS_PE_Out()
        {
            this.CmdByte[0] = 0x29;
            this.CmdByte[1] = 0x29;
        }

        public string Order(string _ID, int MobileType, string[] P)
        {
            byte num13;
            byte num14;
            byte num15;
            byte num16;
            string carID = _ID.Substring(1);
            int num = 0;
            switch (int.Parse(P[0]))
            {
                case 0x10:
                    {
                        this.CmdByte[2] = 0x3e;
                        num = (byte)(P[1].Length / 2);
                        string str9 = "";
                        for (byte i = 0; i < num; i = (byte)(i + 1))
                        {
                            str9 = P[1].Substring(i * 2, 2);
                            this.CmdByte[9 + i] = Convert.ToByte(str9, 0x10);
                        }
                        str9 = P[1].Substring(num * 2);
                        if (str9.Length == 0)
                        {
                            this.CmdByte[9 + num] = 0xff;
                        }
                        else if (str9.Length < 2)
                        {
                            this.CmdByte[9 + num] = Convert.ToByte(str9 + "F", 0x10);
                        }
                        this.CmdByte[4] = (byte)((6 + num) + 1);
                        this.CmdByte[3] = 0;
                        goto Label_12A9;
                    }
                case 20:
                    this.CmdByte[2] = 0x31;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_12A9;

                case 0x17:
                    {
                        byte[] buffer3 = Convert.FromBase64String(P[1]);
                        this.CmdByte[2] = 0x77;
                        this.CmdByte[3] = 0;
                        this.CmdByte[4] = (byte)(6 + buffer3.Length);
                        buffer3.CopyTo(this.CmdByte, 9);
                        goto Label_12A9;
                    }
                case 0x1a:
                    this.CmdByte[2] = 0x3f;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_12A9;

                case 0x2c:
                    return "";

                case 0x2d:
                    {
                        this.CmdByte[2] = 0x34;
                        this.CmdByte[3] = 0;
                        this.CmdByte[4] = 8;
                        int num3 = int.Parse(P[1]);
                        this.CmdByte[9] = (byte)(num3 / 0x100);
                        this.CmdByte[10] = (byte)(num3 % 0x100);
                        goto Label_12A9;
                    }
                case 0x2e:
                    if (!(P[1] == "0"))
                    {
                        this.CmdByte[2] = 0x67;
                        break;
                    }
                    this.CmdByte[2] = 0x68;
                    break;

                case 0x2f:
                    this.CmdByte[2] = 0x40;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_12A9;

                case 0x30:
                    {
                        string[] strArray = P[2].Split(new char[] { '.' });
                        if (!(P[1] == "1"))
                        {
                            if (P[1] == "2")
                            {
                                string s = int.Parse(strArray[0]).ToString("000") + "." + int.Parse(strArray[1]).ToString("000") + "." + int.Parse(strArray[2]).ToString("000") + "." + int.Parse(strArray[3]).ToString("000");
                                s = "\"" + s + "\"," + int.Parse(P[3]).ToString("0000");
                                byte[] bytes = Encoding.Default.GetBytes(s);
                                this.CmdByte[2] = 0x76;
                                this.CmdByte[3] = 0;
                                this.CmdByte[4] = (byte)(6 + bytes.Length);
                                bytes.CopyTo(this.CmdByte, 9);
                            }
                        }
                        else
                        {
                            string str10 = int.Parse(strArray[0]).ToString("000") + "." + int.Parse(strArray[1]).ToString("000") + "." + int.Parse(strArray[2]).ToString("000") + "." + int.Parse(strArray[3]).ToString("000");
                            str10 = "\"" + str10 + "\"," + int.Parse(P[3]).ToString("0000");
                            byte[] buffer4 = Encoding.Default.GetBytes(str10);
                            this.CmdByte[2] = 0x69;
                            this.CmdByte[3] = 0;
                            this.CmdByte[4] = (byte)(6 + buffer4.Length);
                            buffer4.CopyTo(this.CmdByte, 9);
                        }
                        goto Label_12A9;
                    }
                case 0x31:
                    this.CmdByte[2] = 0x70;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_12A9;

                case 50:
                    this.CmdByte[2] = 0x7a;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_12A9;

                case 0x33:
                    this.CmdByte[2] = 0x3d;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_12A9;

                case 0x34:
                    this.CmdByte[2] = 0x72;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_12A9;

                case 0x38:
                    {
                        byte[] buffer6 = Convert.FromBase64String(P[2]);
                        num = (6 + buffer6.Length) + 2;
                        this.CmdByte[2] = 70;
                        this.CmdByte[3] = (byte)(num / 0x100);
                        this.CmdByte[4] = (byte)(num % 0x100);
                        this.CmdByte[9] = byte.Parse(P[1]);
                        buffer6.CopyTo(this.CmdByte, 10);
                        goto Label_12A9;
                    }
                case 0x39:
                    this.CmdByte[2] = 0x65;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 8;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    this.CmdByte[10] = Convert.ToByte(P[2], 2);
                    goto Label_12A9;

                case 0x3a:
                    this.CmdByte[2] = 40;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 8;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    this.CmdByte[10] = byte.Parse(P[2]);
                    goto Label_12A9;

                case 0x3b:
                    this.CmdByte[2] = 0x26;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_12A9;

                case 60:
                    this.CmdByte[2] = 0x29;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 9;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    this.CmdByte[10] = byte.Parse(P[2]);
                    this.CmdByte[11] = byte.Parse(P[3]);
                    goto Label_12A9;

                case 0x3d:
                    this.CmdByte[2] = 0x25;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_12A9;

                case 0x3e:
                    this.CmdByte[2] = 0x27;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_12A9;

                case 0x3f:
                    {
                        byte[] buffer = Convert.FromBase64String(P[1]);
                        this.CmdByte[2] = 0x3a;
                        this.CmdByte[3] = 0;
                        this.CmdByte[4] = (byte)(6 + buffer.Length);
                        buffer.CopyTo(this.CmdByte, 9);
                        goto Label_12A9;
                    }
                case 0x40:
                    this.CmdByte[2] = 0x66;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 12;
                    this.CmdByte[9] = (byte)(DateTime.Now.Year - 0x7d0);
                    this.CmdByte[10] = (byte)DateTime.Now.Month;
                    this.CmdByte[11] = (byte)DateTime.Now.Day;
                    this.CmdByte[12] = (byte)DateTime.Now.Hour;
                    this.CmdByte[13] = (byte)DateTime.Now.Minute;
                    this.CmdByte[14] = (byte)DateTime.Now.Second;
                    goto Label_12A9;

                case 0x41:
                    {
                        byte[] buffer7 = Convert.FromBase64String(P[1]);
                        num = (6 + buffer7.Length) + 1;
                        this.CmdByte[2] = 0x43;
                        this.CmdByte[3] = (byte)(num / 0x100);
                        this.CmdByte[4] = (byte)(num % 0x100);
                        this.CmdByte[9] = 0xff;
                        buffer7.CopyTo(this.CmdByte, 10);
                        goto Label_12A9;
                    }
                case 0x42:
                    {
                        this.CmdByte[2] = 0x34;
                        this.CmdByte[3] = 0;
                        this.CmdByte[4] = 8;
                        int num9 = int.Parse(P[1]);
                        this.CmdByte[9] = (byte)(num9 / 0x100);
                        this.CmdByte[10] = (byte)(num9 % 0x100);
                        goto Label_12A9;
                    }
                case 0x43:
                    return "";

                case 70:
                    {
                        byte[] buffer2 = new byte[int.Parse(P[1]) * 0x12];
                        for (int j = 0; j < int.Parse(P[1]); j++)
                        {
                            string str2 = "";
                            string str3 = "";
                            double num5 = 0.0;
                            int num6 = 0;
                            string str5 = P[(j * 6) + 3];
                            string str6 = P[(j * 6) + 2];
                            string str7 = P[(j * 6) + 5];
                            string str8 = P[(j * 6) + 4];
                            str2 = str5.Substring(0, str5.IndexOf('.'));
                            num5 = double.Parse(str5.Substring(str5.IndexOf('.'))) * 60.0;
                            str3 = num5.ToString("00.000").Replace(".", "");
                            num6 = int.Parse(str2 + str3, NumberStyles.HexNumber);
                            buffer2[j * 0x12] = (byte)(num6 / 0x1000000);
                            num6 = num6 % 0x1000000;
                            buffer2[(j * 0x12) + 1] = (byte)(num6 / 0x10000);
                            num6 = num6 % 0x10000;
                            buffer2[(j * 0x12) + 2] = (byte)(num6 / 0x100);
                            num6 = num6 % 0x100;
                            buffer2[(j * 0x12) + 3] = (byte)num6;
                            str2 = str6.Substring(0, str6.IndexOf('.'));
                            num5 = double.Parse(str6.Substring(str6.IndexOf('.'))) * 60.0;
                            str3 = num5.ToString("00.000").Replace(".", "");
                            num6 = int.Parse(str2 + str3, NumberStyles.HexNumber);
                            buffer2[(j * 0x12) + 4] = (byte)(num6 / 0x1000000);
                            num6 = num6 % 0x1000000;
                            buffer2[(j * 0x12) + 5] = (byte)(num6 / 0x10000);
                            num6 = num6 % 0x10000;
                            buffer2[(j * 0x12) + 6] = (byte)(num6 / 0x100);
                            num6 = num6 % 0x100;
                            buffer2[(j * 0x12) + 7] = (byte)num6;
                            str2 = str7.Substring(0, str7.IndexOf('.'));
                            num5 = double.Parse(str7.Substring(str7.IndexOf('.'))) * 60.0;
                            str3 = num5.ToString("00.000").Replace(".", "");
                            num6 = int.Parse(str2 + str3, NumberStyles.HexNumber);
                            buffer2[(j * 0x12) + 8] = (byte)(num6 / 0x1000000);
                            num6 = num6 % 0x1000000;
                            buffer2[(j * 0x12) + 9] = (byte)(num6 / 0x10000);
                            num6 = num6 % 0x10000;
                            buffer2[(j * 0x12) + 10] = (byte)(num6 / 0x100);
                            num6 = num6 % 0x100;
                            buffer2[(j * 0x12) + 11] = (byte)num6;
                            str2 = str8.Substring(0, str8.IndexOf('.'));
                            num5 = double.Parse(str8.Substring(str8.IndexOf('.'))) * 60.0;
                            num6 = int.Parse(str2 + num5.ToString("00.000").Replace(".", ""), NumberStyles.HexNumber);
                            buffer2[(j * 0x12) + 12] = (byte)(num6 / 0x1000000);
                            num6 = num6 % 0x1000000;
                            buffer2[(j * 0x12) + 13] = (byte)(num6 / 0x10000);
                            num6 = num6 % 0x10000;
                            buffer2[(j * 0x12) + 14] = (byte)(num6 / 0x100);
                            num6 = num6 % 0x100;
                            buffer2[(j * 0x12) + 15] = (byte)num6;
                            buffer2[(j * 0x12) + 0x10] = byte.Parse(P[(j * 6) + 6]);
                            buffer2[(j * 0x12) + 0x11] = byte.Parse(P[(j * 6) + 7]);
                        }
                        int num7 = 7 + buffer2.Length;
                        this.CmdByte[2] = 70;
                        this.CmdByte[3] = (byte)(num7 / 0x100);
                        this.CmdByte[4] = (byte)(num7 % 0x100);
                        this.CmdByte[9] = byte.Parse(P[1]);
                        buffer2.CopyTo(this.CmdByte, 10);
                        goto Label_12A9;
                    }
                case 0x47:
                    if (MobileType != 0xca)
                    {
                        byte[] buffer9 = Convert.FromBase64String(P[1]);
                        num = (6 + buffer9.Length) + 1;
                        this.CmdByte[2] = 120;
                        this.CmdByte[3] = (byte)(num / 0x100);
                        this.CmdByte[4] = (byte)(num % 0x100);
                        this.CmdByte[9] = 1;
                        buffer9.CopyTo(this.CmdByte, 10);
                    }
                    else
                    {
                        byte[] buffer8 = Convert.FromBase64String(P[1]);
                        this.CmdByte[2] = 0x3a;
                        this.CmdByte[3] = 0;
                        this.CmdByte[4] = (byte)(6 + buffer8.Length);
                        buffer8.CopyTo(this.CmdByte, 9);
                    }
                    goto Label_12A9;

                case 0x48:
                    {
                        string str12 = "<C006,FFF>";
                        byte[] buffer10 = Encoding.Default.GetBytes(str12);
                        this.CmdByte[2] = 0xb0;
                        this.CmdByte[3] = 0;
                        this.CmdByte[4] = (byte)(6 + buffer10.Length);
                        buffer10.CopyTo(this.CmdByte, 9);
                        goto Label_12A9;
                    }
                case 0x49:
                    this.CmdByte[2] = 0x47;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_12A9;

                case 0x4a:
                    this.CmdByte[2] = 0x48;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_12A9;

                case 0x4b:
                    {
                        byte[] array = new byte[0x1c];
                        array[0] = 0xff;
                        array[1] = 0xff;
                        string str13 = P[2];
                        string str14 = P[1];
                        string str15 = P[3];
                        string str16 = P[4];
                        string str17 = P[5];
                        string str18 = str13.Substring(0, str13.IndexOf('.'));
                        double num10 = double.Parse(str13.Substring(str13.IndexOf('.'))) * 60.0;
                        int num11 = int.Parse(str18 + num10.ToString("00.000").Replace(".", ""), NumberStyles.HexNumber);
                        array[2] = (byte)(num11 / 0x1000000);
                        num11 = num11 % 0x1000000;
                        array[3] = (byte)(num11 / 0x10000);
                        num11 = num11 % 0x10000;
                        array[4] = (byte)(num11 / 0x100);
                        num11 = num11 % 0x100;
                        array[5] = (byte)num11;
                        str18 = str14.Substring(0, str14.IndexOf('.'));
                        num10 = double.Parse(str14.Substring(str14.IndexOf('.'))) * 60.0;
                        num11 = int.Parse(str18 + num10.ToString("00.000").Replace(".", ""), NumberStyles.HexNumber);
                        array[6] = (byte)(num11 / 0x1000000);
                        num11 = num11 % 0x1000000;
                        array[7] = (byte)(num11 / 0x10000);
                        num11 = num11 % 0x10000;
                        array[8] = (byte)(num11 / 0x100);
                        num11 = num11 % 0x100;
                        array[9] = (byte)num11;
                        array[10] = 0x20;
                        array[11] = 0x20;
                        array[12] = 0x20;
                        array[13] = 0x20;
                        array[14] = 0x20;
                        array[15] = 0x20;
                        array[0x10] = 0x20;
                        array[0x11] = 0x20;
                        array[0x12] = 0x20;
                        array[0x13] = 0x20;
                        array[20] = 0x20;
                        array[0x15] = 0x20;
                        array[0x16] = 0x20;
                        array[0x17] = 0x20;
                        array[0x18] = 0x20;
                        array[0x19] = 0x20;
                        array[0x1a] = 0x20;
                        array[0x1b] = 0x20;
                        Convert.FromBase64String(str16).CopyTo(array, 12);
                        byte[] buffer13 = Convert.FromBase64String(str17);
                        int num12 = 0x22 + buffer13.Length;
                        this.CmdByte[2] = 0x3a;
                        this.CmdByte[3] = (byte)(num12 / 0x100);
                        this.CmdByte[4] = (byte)(num12 % 0x100);
                        array.CopyTo(this.CmdByte, 9);
                        buffer13.CopyTo(this.CmdByte, 0x25);
                        goto Label_12A9;
                    }
                case 11:
                    if (P[1] == "1")
                    {
                        this.CmdByte[2] = 0x39;
                    }
                    else if (P[1] == "0")
                    {
                        this.CmdByte[2] = 0x38;
                    }
                    else if (P[1] == "2")
                    {
                        this.CmdByte[2] = 0x37;
                    }
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_12A9;

                case 6:
                    this.CmdByte[2] = 0x37;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_12A9;

                case 7:
                    this.CmdByte[2] = 50;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_12A9;

                case 0:
                    this.CmdByte[2] = 0x30;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_12A9;

                default:
                    return "Err";
            }
            this.CmdByte[3] = 0;
            this.CmdByte[4] = 6;
        Label_12A9:
            LonghanWrapper.Get_IP_From_CarID(carID, out num13, out num14, out num15, out num16);
            this.CmdByte[5] = num13;
            this.CmdByte[6] = num14;
            this.CmdByte[7] = num15;
            this.CmdByte[8] = num16;
            num = (this.CmdByte[3] * 0x100) + this.CmdByte[4];
            this.CmdByte[(5 + num) - 2] = LonghanWrapper.Get_CheckXor(ref this.CmdByte, (5 + num) - 2);
            this.CmdByte[(5 + num) - 1] = 13;
            return Convert.ToBase64String(this.CmdByte, 0, 5 + num);
        }
    }
}