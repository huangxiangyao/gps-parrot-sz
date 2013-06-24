using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Yelang
{
    public class YelangOut
    {
        // Fields
        private byte[] CmdByte = new byte[0x800];
        private string filePath;

        // Methods
        public YelangOut()
        {
            this.CmdByte[0] = 0x59;
            this.CmdByte[1] = 0x47;
            this.filePath = Environment.CurrentDirectory;
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

        public string Order(string _ID, int MobileType, string[] P)
        {
            byte[] buffer6;
            string str = _ID.Substring(1);
            switch (int.Parse(P[0]))
            {
                case 0x17:
                    {
                        byte[] bytes = Encoding.Default.GetBytes(P[1]);
                        this.CmdByte[9] = 0x12;
                        this.CmdByte[10] = 3;
                        this.CmdByte[11] = 0;
                        this.CmdByte[12] = (byte)bytes.Length;
                        bytes.CopyTo(this.CmdByte, 13);
                        goto Label_07CC;
                    }
                case 0x1a:
                    this.CmdByte[9] = 20;
                    this.CmdByte[10] = 9;
                    this.CmdByte[11] = 0;
                    this.CmdByte[12] = 1;
                    this.CmdByte[13] = byte.Parse(P[1]);
                    goto Label_07CC;

                case 0:
                    this.CmdByte[9] = 0x13;
                    this.CmdByte[10] = 1;
                    this.CmdByte[11] = 0;
                    this.CmdByte[12] = 0;
                    goto Label_07CC;

                case 1:
                    this.CmdByte[9] = 20;
                    this.CmdByte[10] = 2;
                    this.CmdByte[11] = 0;
                    this.CmdByte[12] = 4;
                    this.CmdByte[13] = 0;
                    this.CmdByte[14] = byte.Parse(P[1]);
                    this.CmdByte[15] = 0;
                    this.CmdByte[0x10] = byte.Parse(P[2]);
                    if (this.CmdByte[14] < 3)
                    {
                        this.CmdByte[14] = 3;
                    }
                    goto Label_07CC;

                case 6:
                    this.CmdByte[9] = 20;
                    this.CmdByte[10] = 0x10;
                    this.CmdByte[11] = 0;
                    this.CmdByte[12] = 0;
                    goto Label_07CC;

                case 7:
                    this.CmdByte[9] = 20;
                    this.CmdByte[10] = 0x20;
                    this.CmdByte[11] = 0;
                    this.CmdByte[12] = 0;
                    goto Label_07CC;

                case 11:
                    this.CmdByte[9] = 20;
                    this.CmdByte[10] = 5;
                    this.CmdByte[11] = 0;
                    this.CmdByte[12] = 1;
                    this.CmdByte[13] = byte.Parse(P[1]);
                    goto Label_07CC;

                case 0x10:
                    {
                        byte[] buffer4 = Encoding.Default.GetBytes(P[1]);
                        this.CmdByte[9] = 20;
                        this.CmdByte[10] = 4;
                        this.CmdByte[11] = 0;
                        this.CmdByte[12] = (byte)buffer4.Length;
                        buffer4.CopyTo(this.CmdByte, 13);
                        goto Label_07CC;
                    }
                case 0x11:
                    {
                        string[] strArray = P[2].Split(new char[] { '.' });
                        string s = "";
                        s = "\"" + s + "\"," + int.Parse(P[3]).ToString("0000");
                        byte[] buffer = Encoding.Default.GetBytes(s);
                        this.CmdByte[9] = 0x12;
                        this.CmdByte[10] = 1;
                        this.CmdByte[11] = 0;
                        this.CmdByte[12] = 6;
                        this.CmdByte[13] = byte.Parse(strArray[0]);
                        this.CmdByte[14] = byte.Parse(strArray[1]);
                        this.CmdByte[15] = byte.Parse(strArray[2]);
                        this.CmdByte[0x10] = byte.Parse(strArray[3]);
                        int num3 = int.Parse(P[3]);
                        this.CmdByte[0x11] = (byte)(num3 / 0x100);
                        this.CmdByte[0x12] = (byte)(num3 % 0x100);
                        goto Label_07CC;
                    }
                case 0x12:
                    {
                        byte[] buffer5 = Convert.FromBase64String(P[1]);
                        this.CmdByte[9] = 0x12;
                        this.CmdByte[10] = 0xf1;
                        this.CmdByte[11] = 0;
                        this.CmdByte[12] = (byte)buffer5.Length;
                        buffer5.CopyTo(this.CmdByte, 13);
                        goto Label_07CC;
                    }
                case 0x13:
                    this.CmdByte[9] = 20;
                    this.CmdByte[10] = 0x21;
                    this.CmdByte[11] = 0;
                    this.CmdByte[12] = 0;
                    goto Label_07CC;

                case 0x2d:
                    this.CmdByte[9] = 0x12;
                    this.CmdByte[10] = 8;
                    this.CmdByte[11] = 0;
                    this.CmdByte[12] = 2;
                    this.CmdByte[13] = 0;
                    this.CmdByte[14] = byte.Parse(P[1]);
                    goto Label_07CC;

                case 0x2f:
                    {
                        int num5 = int.Parse(P[1]);
                        this.CmdByte[9] = 20;
                        this.CmdByte[10] = 8;
                        this.CmdByte[11] = 0;
                        this.CmdByte[12] = 2;
                        this.CmdByte[13] = (byte)(num5 / 0x100);
                        this.CmdByte[14] = (byte)(num5 % 0x100);
                        goto Label_07CC;
                    }
                case 0x31:
                    {
                        this.CmdByte[9] = 0x12;
                        this.CmdByte[10] = 9;
                        this.CmdByte[11] = 0;
                        this.CmdByte[12] = 2;
                        this.CmdByte[13] = 0;
                        int num4 = byte.Parse(P[1]) * 60;
                        if (num4 > 0xff)
                        {
                            num4 = 0xff;
                        }
                        this.CmdByte[14] = (byte)num4;
                        goto Label_07CC;
                    }
                case 50:
                    this.CmdByte[9] = 0x12;
                    this.CmdByte[10] = 7;
                    this.CmdByte[11] = 0;
                    this.CmdByte[12] = 2;
                    this.CmdByte[13] = 0;
                    this.CmdByte[14] = 0x19;
                    goto Label_07CC;

                case 0x4e:
                    {
                        byte[] buffer3 = Encoding.Default.GetBytes(P[1]);
                        this.CmdByte[9] = 0x12;
                        this.CmdByte[10] = 6;
                        this.CmdByte[11] = 0;
                        this.CmdByte[12] = (byte)buffer3.Length;
                        buffer3.CopyTo(this.CmdByte, 13);
                        goto Label_07CC;
                    }
                case 0x5d:
                    buffer6 = Encoding.Default.GetBytes(P[2]);
                    this.CmdByte[9] = 0x12;
                    if (!(P[1] == "0"))
                    {
                        if (P[1] == "1")
                        {
                            this.CmdByte[10] = 13;
                        }
                        else if (P[1] == "2")
                        {
                            this.CmdByte[10] = 14;
                        }
                        else if (P[1] == "3")
                        {
                            this.CmdByte[10] = 15;
                        }
                        else if (P[1] == "4")
                        {
                            this.CmdByte[10] = 240;
                        }
                        break;
                    }
                    this.CmdByte[10] = 5;
                    break;

                case 0x5e:
                    if (byte.Parse(P[1]) >= 2)
                    {
                        this.CmdByte[9] = 20;
                        this.CmdByte[10] = 0x12;
                        this.CmdByte[11] = 0;
                        this.CmdByte[12] = 1;
                        this.CmdByte[13] = (byte)(byte.Parse(P[1]) - 2);
                    }
                    else
                    {
                        this.CmdByte[9] = 20;
                        this.CmdByte[10] = 0x11;
                        this.CmdByte[11] = 0;
                        this.CmdByte[12] = 1;
                        this.CmdByte[13] = byte.Parse(P[1]);
                    }
                    goto Label_07CC;

                case 0x84:
                    this.CmdByte[9] = 20;
                    this.CmdByte[10] = 0x12;
                    this.CmdByte[11] = 0;
                    this.CmdByte[12] = 1;
                    this.CmdByte[13] = byte.Parse(P[1]);
                    goto Label_07CC;

                default:
                    goto Label_07CC;
            }
            this.CmdByte[11] = 0;
            this.CmdByte[12] = (byte)buffer6.Length;
            buffer6.CopyTo(this.CmdByte, 13);
        Label_07CC:
            this.CmdByte[2] = 0x30;
            string str3 = _ID;
            this.CmdByte[3] = Convert.ToByte(str3.Substring(0, 2), 0x10);
            this.CmdByte[4] = Convert.ToByte(str3.Substring(2, 2), 0x10);
            this.CmdByte[5] = Convert.ToByte(str3.Substring(4, 2), 0x10);
            this.CmdByte[6] = Convert.ToByte(str3.Substring(6, 2), 0x10);
            this.CmdByte[7] = Convert.ToByte(str3.Substring(8, 2), 0x10);
            this.CmdByte[8] = Convert.ToByte(str3.Substring(10, 1) + "F", 0x10);
            this.CmdByte[((this.CmdByte[11] * 0x100) + this.CmdByte[12]) + 13] = this.Get_CheckXor(ref this.CmdByte, ((this.CmdByte[11] * 0x100) + this.CmdByte[12]) + 13);
            this.CmdByte[((this.CmdByte[11] * 0x100) + this.CmdByte[12]) + 14] = 13;
            return Convert.ToBase64String(this.CmdByte, 0, ((this.CmdByte[11] * 0x100) + this.CmdByte[12]) + 15);
        }
    }
}