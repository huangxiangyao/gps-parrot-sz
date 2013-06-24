using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Parrot.Models.Longhan
{
    public class LH12_GPRS_PE_Out
    {
        // Fields
        private byte[] CmdByte = new byte[0x800];

        // Methods
        public LH12_GPRS_PE_Out()
        {
            this.CmdByte[0] = 0x29;
            this.CmdByte[1] = 0x29;
        }

        public string Order(string _ID, int MobileType, string[] P)
        {
            byte num9;
            byte num10;
            byte num11;
            byte num12;
            string carID = _ID.Substring(1);
            int num = 0;
            switch (int.Parse(P[0]))
            {
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
                    goto Label_0B68;

                case 0x10:
                    {
                        this.CmdByte[2] = 0x3e;
                        num = (byte)(P[1].Length / 2);
                        string str4 = "";
                        for (byte i = 0; i < num; i = (byte)(i + 1))
                        {
                            str4 = P[1].Substring(i * 2, 2);
                            this.CmdByte[9 + i] = Convert.ToByte(str4, 0x10);
                        }
                        str4 = P[1].Substring(num * 2);
                        if (str4.Length == 0)
                        {
                            this.CmdByte[9 + num] = 0xff;
                        }
                        else if (str4.Length < 2)
                        {
                            this.CmdByte[9 + num] = Convert.ToByte(str4 + "F", 0x10);
                        }
                        this.CmdByte[4] = (byte)((6 + num) + 1);
                        this.CmdByte[3] = 0;
                        goto Label_0B68;
                    }
                case 0:
                    this.CmdByte[2] = 0x30;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_0B68;

                case 7:
                    this.CmdByte[2] = 50;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_0B68;

                case 20:
                    this.CmdByte[2] = 0x31;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_0B68;

                case 0x1a:
                    this.CmdByte[2] = 0x3f;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse((((double)byte.Parse(P[1])) / 1.852).ToString("0"));
                    goto Label_0B68;

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
                        goto Label_0B68;
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
                    goto Label_0B68;

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
                            string str2 = int.Parse(strArray[0]).ToString("000") + "." + int.Parse(strArray[1]).ToString("000") + "." + int.Parse(strArray[2]).ToString("000") + "." + int.Parse(strArray[3]).ToString("000");
                            str2 = "\"" + str2 + "\"," + int.Parse(P[3]).ToString("0000");
                            byte[] buffer = Encoding.Default.GetBytes(str2);
                            this.CmdByte[2] = 0x69;
                            this.CmdByte[3] = 0;
                            this.CmdByte[4] = (byte)(6 + buffer.Length);
                            buffer.CopyTo(this.CmdByte, 9);
                        }
                        goto Label_0B68;
                    }
                case 0x31:
                    this.CmdByte[2] = 0x70;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_0B68;

                case 50:
                    this.CmdByte[2] = 0x7a;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_0B68;

                case 0x33:
                    this.CmdByte[2] = 0x3d;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_0B68;

                case 0x3f:
                    {
                        byte[] buffer3 = Convert.FromBase64String(P[1]);
                        this.CmdByte[2] = 0x3a;
                        this.CmdByte[3] = 0;
                        this.CmdByte[4] = (byte)(6 + buffer3.Length);
                        buffer3.CopyTo(this.CmdByte, 9);
                        goto Label_0B68;
                    }
                case 0x43:
                    return "";

                case 70:
                    {
                        byte[] buffer4 = new byte[int.Parse(P[1]) * 0x12];
                        for (int j = 0; j < int.Parse(P[1]); j++)
                        {
                            string str5 = "";
                            string str6 = "";
                            double num6 = 0.0;
                            int num7 = 0;
                            string str8 = P[(j * 6) + 3];
                            string str9 = P[(j * 6) + 2];
                            string str10 = P[(j * 6) + 5];
                            string str11 = P[(j * 6) + 4];
                            str5 = str8.Substring(0, str8.IndexOf('.'));
                            num6 = double.Parse(str8.Substring(str8.IndexOf('.'))) * 60.0;
                            str6 = num6.ToString("00.000").Replace(".", "");
                            num7 = int.Parse(str5 + str6, NumberStyles.HexNumber);
                            buffer4[j * 0x12] = (byte)(num7 / 0x1000000);
                            num7 = num7 % 0x1000000;
                            buffer4[(j * 0x12) + 1] = (byte)(num7 / 0x10000);
                            num7 = num7 % 0x10000;
                            buffer4[(j * 0x12) + 2] = (byte)(num7 / 0x100);
                            num7 = num7 % 0x100;
                            buffer4[(j * 0x12) + 3] = (byte)num7;
                            str5 = str9.Substring(0, str9.IndexOf('.'));
                            num6 = double.Parse(str9.Substring(str9.IndexOf('.'))) * 60.0;
                            str6 = num6.ToString("00.000").Replace(".", "");
                            num7 = int.Parse(str5 + str6, NumberStyles.HexNumber);
                            buffer4[(j * 0x12) + 4] = (byte)(num7 / 0x1000000);
                            num7 = num7 % 0x1000000;
                            buffer4[(j * 0x12) + 5] = (byte)(num7 / 0x10000);
                            num7 = num7 % 0x10000;
                            buffer4[(j * 0x12) + 6] = (byte)(num7 / 0x100);
                            num7 = num7 % 0x100;
                            buffer4[(j * 0x12) + 7] = (byte)num7;
                            str5 = str10.Substring(0, str10.IndexOf('.'));
                            num6 = double.Parse(str10.Substring(str10.IndexOf('.'))) * 60.0;
                            str6 = num6.ToString("00.000").Replace(".", "");
                            num7 = int.Parse(str5 + str6, NumberStyles.HexNumber);
                            buffer4[(j * 0x12) + 8] = (byte)(num7 / 0x1000000);
                            num7 = num7 % 0x1000000;
                            buffer4[(j * 0x12) + 9] = (byte)(num7 / 0x10000);
                            num7 = num7 % 0x10000;
                            buffer4[(j * 0x12) + 10] = (byte)(num7 / 0x100);
                            num7 = num7 % 0x100;
                            buffer4[(j * 0x12) + 11] = (byte)num7;
                            str5 = str11.Substring(0, str11.IndexOf('.'));
                            num6 = double.Parse(str11.Substring(str11.IndexOf('.'))) * 60.0;
                            num7 = int.Parse(str5 + num6.ToString("00.000").Replace(".", ""), NumberStyles.HexNumber);
                            buffer4[(j * 0x12) + 12] = (byte)(num7 / 0x1000000);
                            num7 = num7 % 0x1000000;
                            buffer4[(j * 0x12) + 13] = (byte)(num7 / 0x10000);
                            num7 = num7 % 0x10000;
                            buffer4[(j * 0x12) + 14] = (byte)(num7 / 0x100);
                            num7 = num7 % 0x100;
                            buffer4[(j * 0x12) + 15] = (byte)num7;
                            buffer4[(j * 0x12) + 0x10] = byte.Parse(P[(j * 6) + 6]);
                            buffer4[(j * 0x12) + 0x11] = byte.Parse(P[(j * 6) + 7]);
                        }
                        int num8 = 7 + buffer4.Length;
                        this.CmdByte[2] = 70;
                        this.CmdByte[3] = (byte)(num8 / 0x100);
                        this.CmdByte[4] = (byte)(num8 % 0x100);
                        this.CmdByte[9] = byte.Parse(P[1]);
                        buffer4.CopyTo(this.CmdByte, 10);
                        goto Label_0B68;
                    }
                case 0x48:
                    {
                        string str12 = "<C006,FFF>";
                        byte[] buffer5 = Encoding.Default.GetBytes(str12);
                        this.CmdByte[2] = 0xb0;
                        this.CmdByte[3] = 0;
                        this.CmdByte[4] = (byte)(6 + buffer5.Length);
                        buffer5.CopyTo(this.CmdByte, 9);
                        goto Label_0B68;
                    }
                case 0x49:
                    this.CmdByte[2] = 0x47;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_0B68;

                case 0x4a:
                    this.CmdByte[2] = 0x48;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_0B68;

                default:
                    return "Err";
            }
            this.CmdByte[3] = 0;
            this.CmdByte[4] = 6;
        Label_0B68:
            LonghanWrapper.Get_IP_From_CarID(carID, out num9, out num10, out num11, out num12);
            this.CmdByte[5] = num9;
            this.CmdByte[6] = num10;
            this.CmdByte[7] = num11;
            this.CmdByte[8] = num12;
            num = (this.CmdByte[3] * 0x100) + this.CmdByte[4];
            this.CmdByte[(5 + num) - 2] = LonghanWrapper.Get_CheckXor(ref this.CmdByte, (5 + num) - 2);
            this.CmdByte[(5 + num) - 1] = 13;
            return Convert.ToBase64String(this.CmdByte, 0, 5 + num);
        }
    }
}