using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.CxGprs
{
    public class CxGprsOut
    {
        // Fields
        private byte[] CmdByte = new byte[0x100];

        // Methods
        public string Order(string _ID, int MobileType, string[] P)
        {
            byte length = 0;
            byte num2 = byte.Parse(_ID.Substring(0, 1));
            byte num3 = byte.Parse(_ID.Substring(1, 3));
            byte num4 = byte.Parse(_ID.Substring(4, 3));
            switch (int.Parse(P[0]))
            {
                case 0x2c:
                    this.CmdByte[0] = 0x80;
                    this.CmdByte[1] = 0x53;
                    this.CmdByte[2] = num2;
                    this.CmdByte[3] = num3;
                    this.CmdByte[4] = num4;
                    this.CmdByte[5] = 0xff;
                    this.CmdByte[6] = 13;
                    length = 7;
                    break;

                case 0x2d:
                    {
                        this.CmdByte[0] = 0x80;
                        this.CmdByte[1] = 0x54;
                        this.CmdByte[2] = num2;
                        this.CmdByte[3] = num3;
                        this.CmdByte[4] = num4;
                        this.CmdByte[5] = 0x30;
                        this.CmdByte[6] = 0x30;
                        int num6 = int.Parse(P[1]);
                        if (num6 > 100)
                        {
                            num6 = num6 % 100;
                        }
                        this.CmdByte[7] = (byte)(0x30 + (num6 / 10));
                        this.CmdByte[8] = (byte)(0x30 + (num6 % 10));
                        this.CmdByte[9] = 0x39;
                        this.CmdByte[10] = 0x39;
                        this.CmdByte[11] = 0x39;
                        this.CmdByte[12] = 0x39;
                        this.CmdByte[13] = 0xff;
                        this.CmdByte[14] = 13;
                        length = 15;
                        break;
                    }
                case 0x1a:
                    this.CmdByte[0] = 0x80;
                    this.CmdByte[2] = num2;
                    this.CmdByte[3] = num3;
                    this.CmdByte[4] = num4;
                    if (P[1] == "1")
                    {
                        this.CmdByte[1] = 0x61;
                        P[2] = P[2].PadLeft(3, '0');
                        this.CmdByte[5] = 0;
                        this.CmdByte[6] = (byte)(0x30 + byte.Parse(P[2].Substring(0, 1)));
                        this.CmdByte[7] = (byte)(0x30 + byte.Parse(P[2].Substring(1, 1)));
                        this.CmdByte[8] = (byte)(0x30 + byte.Parse(P[2].Substring(2, 1)));
                        this.CmdByte[9] = 0xff;
                        this.CmdByte[10] = 13;
                        length = 11;
                    }
                    else
                    {
                        this.CmdByte[1] = 0x62;
                        this.CmdByte[5] = 0xff;
                        this.CmdByte[6] = 13;
                        length = 7;
                    }
                    break;

                case 11:
                    this.CmdByte[0] = 0x80;
                    if (P[1] == "0")
                    {
                        this.CmdByte[1] = 0x55;
                    }
                    else if (P[1] == "1")
                    {
                        this.CmdByte[1] = 0x4b;
                    }
                    else if (P[1] == "2")
                    {
                        this.CmdByte[1] = 0x53;
                    }
                    this.CmdByte[2] = num2;
                    this.CmdByte[3] = num3;
                    this.CmdByte[4] = num4;
                    this.CmdByte[5] = 0xff;
                    this.CmdByte[6] = 13;
                    length = 7;
                    break;

                case 0:
                    this.CmdByte[0] = 0x80;
                    this.CmdByte[1] = 0x4c;
                    this.CmdByte[2] = num2;
                    this.CmdByte[3] = num3;
                    this.CmdByte[4] = num4;
                    this.CmdByte[5] = 0xff;
                    this.CmdByte[6] = 13;
                    length = 7;
                    break;

                case 5:
                    this.CmdByte[0] = 0x80;
                    if (P[1] == "0")
                    {
                        this.CmdByte[1] = 80;
                    }
                    else if (P[1] == "1")
                    {
                        this.CmdByte[1] = 0x70;
                    }
                    this.CmdByte[2] = num2;
                    this.CmdByte[3] = num3;
                    this.CmdByte[4] = num4;
                    this.CmdByte[5] = 0xff;
                    this.CmdByte[6] = 13;
                    length = 7;
                    break;
            }
            return Convert.ToBase64String(this.CmdByte, 0, length);
        }
    }
}