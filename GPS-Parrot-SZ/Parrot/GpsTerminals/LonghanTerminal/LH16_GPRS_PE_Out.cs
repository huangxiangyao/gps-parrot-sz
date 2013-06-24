using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Collections;

namespace Parrot.Models.Longhan
{
    public class LH16_GPRS_PE_Out
    {
        // Fields
        private byte[] CmdByte = new byte[0x800];
        private string filePath;

        // Methods
        public LH16_GPRS_PE_Out()
        {
            this.CmdByte[0] = 0x29;
            this.CmdByte[1] = 0x29;
            this.filePath = Environment.CurrentDirectory;
        }

        private FileInfo[] ListFile(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            return info.GetFiles("*.ini");
        }

        public string Order(string _ID, int MobileType, string[] P)
        {
            byte num32;
            byte num33;
            byte num34;
            byte num35;
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
                    goto Label_1910;

                case 0x10:
                    {
                        this.CmdByte[2] = 0x3e;
                        num = (byte)(P[1].Length / 2);
                        string str11 = "";
                        for (byte i = 0; i < num; i = (byte)(i + 1))
                        {
                            str11 = P[1].Substring(i * 2, 2);
                            this.CmdByte[9 + i] = Convert.ToByte(str11, 0x10);
                        }
                        str11 = P[1].Substring(num * 2);
                        if (str11.Length == 0)
                        {
                            this.CmdByte[9 + num] = 0xff;
                        }
                        else if (str11.Length < 2)
                        {
                            this.CmdByte[9 + num] = Convert.ToByte(str11 + "F", 0x10);
                        }
                        this.CmdByte[4] = (byte)((6 + num) + 1);
                        this.CmdByte[3] = 0;
                        goto Label_1910;
                    }
                case 6:
                    this.CmdByte[2] = 0x37;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_1910;

                case 7:
                    this.CmdByte[2] = 50;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_1910;

                case 0:
                    this.CmdByte[2] = 0x30;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_1910;

                case 20:
                    this.CmdByte[2] = 0x31;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_1910;

                case 0x17:
                    {
                        byte[] buffer3 = Convert.FromBase64String(P[1]);
                        this.CmdByte[2] = 0x77;
                        this.CmdByte[3] = 0;
                        this.CmdByte[4] = (byte)(6 + buffer3.Length);
                        buffer3.CopyTo(this.CmdByte, 9);
                        goto Label_1910;
                    }
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
                        goto Label_1910;
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
                    goto Label_1910;

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
                        goto Label_1910;
                    }
                case 0x31:
                    this.CmdByte[2] = 0x70;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_1910;

                case 0x33:
                    this.CmdByte[2] = 0x3d;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_1910;

                case 0x39:
                    this.CmdByte[2] = 0x65;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 8;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    this.CmdByte[10] = Convert.ToByte(P[2], 2);
                    goto Label_1910;

                case 0x3a:
                    this.CmdByte[2] = 40;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 8;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    this.CmdByte[10] = byte.Parse(P[2]);
                    goto Label_1910;

                case 0x3b:
                    this.CmdByte[2] = 0x26;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_1910;

                case 60:
                    this.CmdByte[2] = 0x29;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 9;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    this.CmdByte[10] = byte.Parse(P[2]);
                    this.CmdByte[11] = byte.Parse(P[3]);
                    goto Label_1910;

                case 0x3d:
                    this.CmdByte[2] = 0x25;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_1910;

                case 0x3e:
                    this.CmdByte[2] = 0x27;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_1910;

                case 0x3f:
                    {
                        byte[] buffer6 = Convert.FromBase64String(P[1]);
                        this.CmdByte[2] = 0x3a;
                        this.CmdByte[3] = 0;
                        this.CmdByte[4] = (byte)(6 + buffer6.Length);
                        buffer6.CopyTo(this.CmdByte, 9);
                        goto Label_1910;
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
                    goto Label_1910;

                case 0x41:
                    {
                        byte[] buffer5 = Convert.FromBase64String(P[1]);
                        num = (6 + buffer5.Length) + 1;
                        this.CmdByte[2] = 0x43;
                        this.CmdByte[3] = (byte)(num / 0x100);
                        this.CmdByte[4] = (byte)(num % 0x100);
                        this.CmdByte[9] = 0xff;
                        buffer5.CopyTo(this.CmdByte, 10);
                        goto Label_1910;
                    }
                case 70:
                    {
                        byte[] buffer4 = new byte[int.Parse(P[1]) * 0x12];
                        for (int j = 0; j < int.Parse(P[1]); j++)
                        {
                            string str4 = "";
                            string str5 = "";
                            double num5 = 0.0;
                            int num6 = 0;
                            string str7 = P[(j * 6) + 3];
                            string str8 = P[(j * 6) + 2];
                            string str9 = P[(j * 6) + 5];
                            string str10 = P[(j * 6) + 4];
                            str4 = str7.Substring(0, str7.IndexOf('.'));
                            num5 = double.Parse(str7.Substring(str7.IndexOf('.'))) * 60.0;
                            str5 = num5.ToString("00.000").Replace(".", "");
                            num6 = int.Parse(str4 + str5, NumberStyles.HexNumber);
                            buffer4[j * 0x12] = (byte)(num6 / 0x1000000);
                            num6 = num6 % 0x1000000;
                            buffer4[(j * 0x12) + 1] = (byte)(num6 / 0x10000);
                            num6 = num6 % 0x10000;
                            buffer4[(j * 0x12) + 2] = (byte)(num6 / 0x100);
                            num6 = num6 % 0x100;
                            buffer4[(j * 0x12) + 3] = (byte)num6;
                            str4 = str8.Substring(0, str8.IndexOf('.'));
                            num5 = double.Parse(str8.Substring(str8.IndexOf('.'))) * 60.0;
                            str5 = num5.ToString("00.000").Replace(".", "");
                            num6 = int.Parse(str4 + str5, NumberStyles.HexNumber);
                            buffer4[(j * 0x12) + 4] = (byte)(num6 / 0x1000000);
                            num6 = num6 % 0x1000000;
                            buffer4[(j * 0x12) + 5] = (byte)(num6 / 0x10000);
                            num6 = num6 % 0x10000;
                            buffer4[(j * 0x12) + 6] = (byte)(num6 / 0x100);
                            num6 = num6 % 0x100;
                            buffer4[(j * 0x12) + 7] = (byte)num6;
                            str4 = str9.Substring(0, str9.IndexOf('.'));
                            num5 = double.Parse(str9.Substring(str9.IndexOf('.'))) * 60.0;
                            str5 = num5.ToString("00.000").Replace(".", "");
                            num6 = int.Parse(str4 + str5, NumberStyles.HexNumber);
                            buffer4[(j * 0x12) + 8] = (byte)(num6 / 0x1000000);
                            num6 = num6 % 0x1000000;
                            buffer4[(j * 0x12) + 9] = (byte)(num6 / 0x10000);
                            num6 = num6 % 0x10000;
                            buffer4[(j * 0x12) + 10] = (byte)(num6 / 0x100);
                            num6 = num6 % 0x100;
                            buffer4[(j * 0x12) + 11] = (byte)num6;
                            str4 = str10.Substring(0, str10.IndexOf('.'));
                            num5 = double.Parse(str10.Substring(str10.IndexOf('.'))) * 60.0;
                            num6 = int.Parse(str4 + num5.ToString("00.000").Replace(".", ""), NumberStyles.HexNumber);
                            buffer4[(j * 0x12) + 12] = (byte)(num6 / 0x1000000);
                            num6 = num6 % 0x1000000;
                            buffer4[(j * 0x12) + 13] = (byte)(num6 / 0x10000);
                            num6 = num6 % 0x10000;
                            buffer4[(j * 0x12) + 14] = (byte)(num6 / 0x100);
                            num6 = num6 % 0x100;
                            buffer4[(j * 0x12) + 15] = (byte)num6;
                            buffer4[(j * 0x12) + 0x10] = byte.Parse(P[(j * 6) + 6]);
                            buffer4[(j * 0x12) + 0x11] = byte.Parse(P[(j * 6) + 7]);
                        }
                        int num7 = 7 + buffer4.Length;
                        this.CmdByte[2] = 70;
                        this.CmdByte[3] = (byte)(num7 / 0x100);
                        this.CmdByte[4] = (byte)(num7 % 0x100);
                        this.CmdByte[9] = byte.Parse(P[1]);
                        buffer4.CopyTo(this.CmdByte, 10);
                        goto Label_1910;
                    }
                case 0x47:
                    {
                        byte[] buffer7 = Convert.FromBase64String(P[1]);
                        num = (6 + buffer7.Length) + 1;
                        this.CmdByte[2] = 120;
                        this.CmdByte[3] = (byte)(num / 0x100);
                        this.CmdByte[4] = (byte)(num % 0x100);
                        this.CmdByte[9] = 1;
                        buffer7.CopyTo(this.CmdByte, 10);
                        goto Label_1910;
                    }
                case 0x49:
                    this.CmdByte[2] = 0x47;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_1910;

                case 0x4a:
                    this.CmdByte[2] = 0x48;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 6;
                    goto Label_1910;

                case 0x4b:
                    {
                        byte[] array = new byte[0x1c];
                        array[0] = 0xff;
                        array[1] = 0xff;
                        string str12 = P[2];
                        string str13 = P[1];
                        string str14 = P[3];
                        string str15 = P[4];
                        string str16 = P[5];
                        string str17 = str12.Substring(0, str12.IndexOf('.'));
                        double num9 = double.Parse(str12.Substring(str12.IndexOf('.'))) * 60.0;
                        int num10 = int.Parse(str17 + num9.ToString("00.000").Replace(".", ""), NumberStyles.HexNumber);
                        array[2] = (byte)(num10 / 0x1000000);
                        num10 = num10 % 0x1000000;
                        array[3] = (byte)(num10 / 0x10000);
                        num10 = num10 % 0x10000;
                        array[4] = (byte)(num10 / 0x100);
                        num10 = num10 % 0x100;
                        array[5] = (byte)num10;
                        str17 = str13.Substring(0, str13.IndexOf('.'));
                        num9 = double.Parse(str13.Substring(str13.IndexOf('.'))) * 60.0;
                        num10 = int.Parse(str17 + num9.ToString("00.000").Replace(".", ""), NumberStyles.HexNumber);
                        array[6] = (byte)(num10 / 0x1000000);
                        num10 = num10 % 0x1000000;
                        array[7] = (byte)(num10 / 0x10000);
                        num10 = num10 % 0x10000;
                        array[8] = (byte)(num10 / 0x100);
                        num10 = num10 % 0x100;
                        array[9] = (byte)num10;
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
                        Convert.FromBase64String(str15).CopyTo(array, 12);
                        byte[] buffer10 = Convert.FromBase64String(str16);
                        int num11 = 0x22 + buffer10.Length;
                        this.CmdByte[2] = 0x3a;
                        this.CmdByte[3] = (byte)(num11 / 0x100);
                        this.CmdByte[4] = (byte)(num11 % 0x100);
                        array.CopyTo(this.CmdByte, 9);
                        buffer10.CopyTo(this.CmdByte, 0x25);
                        goto Label_1910;
                    }
                case 90:
                    {
                        ArrayList list = new ArrayList();
                        for (int k = 0; k < int.Parse(P[1]); k++)
                        {
                            double num13 = double.Parse(P[(k * 7) + 2]);
                            double num14 = double.Parse(P[(k * 7) + 3]);
                            double num15 = double.Parse(P[(k * 7) + 4]);
                            double num16 = double.Parse(P[(k * 7) + 5]);
                            byte num17 = byte.Parse(P[(k * 7) + 6]);
                            byte num18 = byte.Parse(P[(k * 7) + 7]);
                            byte num19 = byte.Parse(P[(k * 7) + 8]);
                            Area_LimitingSpeedInfoClass class2 = new Area_LimitingSpeedInfoClass();
                            class2.Inti(num13, num14, num15, num16, num17, num18, num19);
                            list.Add(class2);
                        }
                        this.Save_Area_LimitingSpeedInfo(_ID, list);
                        return "";
                    }
                case 0x5b:
                    {
                        string str20 = "";
                        if (!(P[1] == "1"))
                        {
                            str20 = "$GPRMC,000000.000,E,00000.0000,N,00000.0000,E,0.00,000.00";
                        }
                        else
                        {
                            str20 = "$GPRMC,000000.000,D,00000.0000,N,00000.0000,E,0.00,000.00";
                        }
                        byte[] buffer11 = Encoding.Default.GetBytes(str20);
                        num = (6 + buffer11.Length) + 1;
                        this.CmdByte[2] = 120;
                        this.CmdByte[3] = (byte)(num / 0x100);
                        this.CmdByte[4] = (byte)(num % 0x100);
                        this.CmdByte[9] = 1;
                        buffer11.CopyTo(this.CmdByte, 10);
                        goto Label_1910;
                    }
                case 0x5c:
                    {
                        byte[] buffer12 = new byte[int.Parse(P[3]) * 0x12];
                        for (int m = 0; m < int.Parse(P[3]); m++)
                        {
                            string str21 = "";
                            string str22 = "";
                            double num21 = 0.0;
                            int num22 = 0;
                            string str24 = P[(m * 6) + 5];
                            string str25 = P[(m * 6) + 4];
                            string str26 = P[(m * 6) + 7];
                            string str27 = P[(m * 6) + 6];
                            str21 = str24.Substring(0, str24.IndexOf('.'));
                            num21 = double.Parse(str24.Substring(str24.IndexOf('.'))) * 60.0;
                            str22 = num21.ToString("00.000").Replace(".", "");
                            num22 = int.Parse(str21 + str22, NumberStyles.HexNumber);
                            buffer12[m * 0x12] = (byte)(num22 / 0x1000000);
                            num22 = num22 % 0x1000000;
                            buffer12[(m * 0x12) + 1] = (byte)(num22 / 0x10000);
                            num22 = num22 % 0x10000;
                            buffer12[(m * 0x12) + 2] = (byte)(num22 / 0x100);
                            num22 = num22 % 0x100;
                            buffer12[(m * 0x12) + 3] = (byte)num22;
                            str21 = str25.Substring(0, str25.IndexOf('.'));
                            num21 = double.Parse(str25.Substring(str25.IndexOf('.'))) * 60.0;
                            str22 = num21.ToString("00.000").Replace(".", "");
                            num22 = int.Parse(str21 + str22, NumberStyles.HexNumber);
                            buffer12[(m * 0x12) + 4] = (byte)(num22 / 0x1000000);
                            num22 = num22 % 0x1000000;
                            buffer12[(m * 0x12) + 5] = (byte)(num22 / 0x10000);
                            num22 = num22 % 0x10000;
                            buffer12[(m * 0x12) + 6] = (byte)(num22 / 0x100);
                            num22 = num22 % 0x100;
                            buffer12[(m * 0x12) + 7] = (byte)num22;
                            str21 = str26.Substring(0, str26.IndexOf('.'));
                            num21 = double.Parse(str26.Substring(str26.IndexOf('.'))) * 60.0;
                            str22 = num21.ToString("00.000").Replace(".", "");
                            num22 = int.Parse(str21 + str22, NumberStyles.HexNumber);
                            buffer12[(m * 0x12) + 8] = (byte)(num22 / 0x1000000);
                            num22 = num22 % 0x1000000;
                            buffer12[(m * 0x12) + 9] = (byte)(num22 / 0x10000);
                            num22 = num22 % 0x10000;
                            buffer12[(m * 0x12) + 10] = (byte)(num22 / 0x100);
                            num22 = num22 % 0x100;
                            buffer12[(m * 0x12) + 11] = (byte)num22;
                            str21 = str27.Substring(0, str27.IndexOf('.'));
                            num21 = double.Parse(str27.Substring(str27.IndexOf('.'))) * 60.0;
                            num22 = int.Parse(str21 + num21.ToString("00.000").Replace(".", ""), NumberStyles.HexNumber);
                            buffer12[(m * 0x12) + 12] = (byte)(num22 / 0x1000000);
                            num22 = num22 % 0x1000000;
                            buffer12[(m * 0x12) + 13] = (byte)(num22 / 0x10000);
                            num22 = num22 % 0x10000;
                            buffer12[(m * 0x12) + 14] = (byte)(num22 / 0x100);
                            num22 = num22 % 0x100;
                            buffer12[(m * 0x12) + 15] = (byte)num22;
                            buffer12[(m * 0x12) + 0x10] = byte.Parse(P[(m * 6) + 8]);
                            buffer12[(m * 0x12) + 0x11] = byte.Parse(P[(m * 6) + 9]);
                        }
                        int num23 = 9 + buffer12.Length;
                        this.CmdByte[2] = 0x7d;
                        this.CmdByte[3] = (byte)(num23 / 0x100);
                        this.CmdByte[4] = (byte)(num23 % 0x100);
                        this.CmdByte[9] = byte.Parse(P[1]);
                        this.CmdByte[10] = byte.Parse(P[2]);
                        this.CmdByte[11] = byte.Parse(P[3]);
                        buffer12.CopyTo(this.CmdByte, 12);
                        goto Label_1910;
                    }
                case 0x5d:
                    this.CmdByte[2] = 0x7c;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_1910;

                case 0x5e:
                    this.CmdByte[2] = 0x7f;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 8;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    this.CmdByte[10] = byte.Parse(P[2]);
                    goto Label_1910;

                case 0x5f:
                    this.CmdByte[2] = 0x7e;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_1910;

                case 0x62:
                    {
                        P[4] = Encoding.Default.GetString(Convert.FromBase64String(P[4]));
                        string str28 = "";
                        if (!(P[1] == "1"))
                        {
                            str28 = P[4];
                        }
                        else
                        {
                            str28 = "&" + P[3] + "&" + P[4];
                        }
                        str28 = P[2].PadLeft(13, '0') + str28;
                        byte[] buffer13 = Encoding.Default.GetBytes(str28);
                        num = (6 + buffer13.Length) + 1;
                        this.CmdByte[2] = 0x98;
                        this.CmdByte[3] = (byte)(num / 0x100);
                        this.CmdByte[4] = (byte)(num % 0x100);
                        this.CmdByte[9] = byte.Parse(P[1]);
                        buffer13.CopyTo(this.CmdByte, 10);
                        goto Label_1910;
                    }
                case 0x63:
                    {
                        ArrayList list2 = new ArrayList();
                        int num24 = 0;
                        double num25 = double.Parse(P[(num24 * 7) + 2]);
                        double num26 = double.Parse(P[(num24 * 7) + 3]);
                        double num27 = double.Parse(P[(num24 * 7) + 4]);
                        double num28 = double.Parse(P[(num24 * 7) + 5]);
                        byte num29 = byte.Parse(P[(num24 * 7) + 6]);
                        byte num30 = byte.Parse(P[(num24 * 7) + 7]);
                        byte num31 = byte.Parse(P[(num24 * 7) + 8]);
                        Area_LimitingSpeedInfoClass class3 = new Area_LimitingSpeedInfoClass();
                        class3.Inti(num25, num26, num27, num28, num29, num30, num31);
                        list2.Add(class3);
                        this.Save_Area_Limiting_FJZZ_Info(_ID, list2);
                        return "";
                    }
                case 0x1a:
                    this.CmdByte[2] = 0x3f;
                    this.CmdByte[3] = 0;
                    this.CmdByte[4] = 7;
                    this.CmdByte[9] = byte.Parse(P[1]);
                    goto Label_1910;

                default:
                    return "Err";
            }
            this.CmdByte[3] = 0;
            this.CmdByte[4] = 6;
        Label_1910:
            LonghanWrapper.Get_IP_From_CarID(carID, out num32, out num33, out num34, out num35);
            this.CmdByte[5] = num32;
            this.CmdByte[6] = num33;
            this.CmdByte[7] = num34;
            this.CmdByte[8] = num35;
            num = (this.CmdByte[3] * 0x100) + this.CmdByte[4];
            this.CmdByte[(5 + num) - 2] = LonghanWrapper.Get_CheckXor(ref this.CmdByte, (5 + num) - 2);
            this.CmdByte[(5 + num) - 1] = 13;
            return Convert.ToBase64String(this.CmdByte, 0, 5 + num);
        }

        private void Read_Area_Limiting_FJZZ_Info()
        {
            FileInfo[] infoArray = this.ListFile(this.filePath + @"\Area_Limiting_FJZZ_Info");
            foreach (FileInfo info in infoArray)
            {
                string key = info.Name.Replace(".ini", "");
                string message = this.filePath + @"\Area_Limiting_FJZZ_Info\" + info.Name;
                Debug.WriteLine(message);
                StreamReader reader = new StreamReader(message);
                if (reader != null)
                {
                    string str3 = null;
                    ArrayList list = new ArrayList();
                    do
                    {
                        try
                        {
                            str3 = reader.ReadLine();
                            if (str3 == null)
                            {
                                break;
                            }
                            string[] strArray = str3.Split(new char[] { ',' });
                            if (strArray.Length == 7)
                            {
                                double num2 = double.Parse(strArray[0]);
                                double num3 = double.Parse(strArray[1]);
                                double num4 = double.Parse(strArray[2]);
                                double num5 = double.Parse(strArray[3]);
                                byte num6 = byte.Parse(strArray[4]);
                                byte num7 = byte.Parse(strArray[5]);
                                byte num8 = byte.Parse(strArray[6]);
                                Area_LimitingSpeedInfoClass class2 = new Area_LimitingSpeedInfoClass();
                                class2.Inti(num2, num3, num4, num5, num6, num7, num8);
                                list.Add(class2);
                            }
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine("Error---" + exception.ToString());
                        }
                    }
                    while (str3 != null);
                    if (Area_LimitingSpeedInfoClass.Area_Limiting_FJZZ_HashList != null)
                    {
                        Area_LimitingSpeedInfoClass.Area_Limiting_FJZZ_HashList.Remove(key);
                        Area_LimitingSpeedInfoClass.Area_Limiting_FJZZ_HashList.Add(key, list);
                    }
                }
                reader.Close();
            }
        }

        private void Read_Area_LimitingSpeedInfo()
        {
            FileInfo[] infoArray = this.ListFile(this.filePath + @"\Area_LimitingSpeedInfo");
            foreach (FileInfo info in infoArray)
            {
                string key = info.Name.Replace(".ini", "");
                string message = this.filePath + @"\Area_LimitingSpeedInfo\" + info.Name;
                Debug.WriteLine(message);
                StreamReader reader = new StreamReader(message);
                if (reader != null)
                {
                    string str3 = null;
                    ArrayList list = new ArrayList();
                    do
                    {
                        try
                        {
                            str3 = reader.ReadLine();
                            if (str3 == null)
                            {
                                break;
                            }
                            string[] strArray = str3.Split(new char[] { ',' });
                            if (strArray.Length == 7)
                            {
                                double num2 = double.Parse(strArray[0]);
                                double num3 = double.Parse(strArray[1]);
                                double num4 = double.Parse(strArray[2]);
                                double num5 = double.Parse(strArray[3]);
                                byte num6 = byte.Parse(strArray[4]);
                                byte num7 = byte.Parse(strArray[5]);
                                byte num8 = byte.Parse(strArray[6]);
                                Area_LimitingSpeedInfoClass class2 = new Area_LimitingSpeedInfoClass();
                                class2.Inti(num2, num3, num4, num5, num6, num7, num8);
                                list.Add(class2);
                            }
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine("Error---" + exception.ToString());
                        }
                    }
                    while (str3 != null);
                    if (Area_LimitingSpeedInfoClass.Area_LimitingSpeed_HashList != null)
                    {
                        Area_LimitingSpeedInfoClass.Area_LimitingSpeed_HashList.Remove(key);
                        Area_LimitingSpeedInfoClass.Area_LimitingSpeed_HashList.Add(key, list);
                    }
                }
                reader.Close();
            }
        }

        private void Save_Area_Limiting_FJZZ_Info(string _ID, ArrayList Area_LimitingSpeedInfoList)
        {
            if (Area_LimitingSpeedInfoClass.Area_Limiting_FJZZ_HashList.ContainsKey(_ID))
            {
                Area_LimitingSpeedInfoClass.Area_Limiting_FJZZ_HashList.Remove(_ID);
            }
            Area_LimitingSpeedInfoClass.Area_Limiting_FJZZ_HashList.Add(_ID, Area_LimitingSpeedInfoList);
            try
            {
                new FileInfo(this.filePath + @"\Area_Limiting_FJZZ_Info\" + _ID + ".ini").Delete();
                StreamWriter writer = new StreamWriter(new FileStream(this.filePath + @"\Area_Limiting_FJZZ_Info\" + _ID + ".ini", FileMode.OpenOrCreate, FileAccess.Write));
                writer.AutoFlush = true;
                foreach (Area_LimitingSpeedInfoClass class2 in Area_LimitingSpeedInfoList)
                {
                    writer.WriteLine(string.Concat(new object[] { class2.MinLong, ",", class2.MinLat, ",", class2.MaxLong, ",", class2.MaxLat, ",", class2.AreaID, ",", class2.AreaAlarmType, ",", class2.MaxSpeed }));
                }
                writer.Close();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
        }

        private void Save_Area_LimitingSpeedInfo(string _ID, ArrayList Area_LimitingSpeedInfoList)
        {
            if (Area_LimitingSpeedInfoClass.Area_LimitingSpeed_HashList.ContainsKey(_ID))
            {
                Area_LimitingSpeedInfoClass.Area_LimitingSpeed_HashList.Remove(_ID);
            }
            Area_LimitingSpeedInfoClass.Area_LimitingSpeed_HashList.Add(_ID, Area_LimitingSpeedInfoList);
            try
            {
                new FileInfo(this.filePath + @"\Area_LimitingSpeedInfo\" + _ID + ".ini").Delete();
                StreamWriter writer = new StreamWriter(new FileStream(this.filePath + @"\Area_LimitingSpeedInfo\" + _ID + ".ini", FileMode.OpenOrCreate, FileAccess.Write));
                writer.AutoFlush = true;
                foreach (Area_LimitingSpeedInfoClass class2 in Area_LimitingSpeedInfoList)
                {
                    writer.WriteLine(string.Concat(new object[] { class2.MinLong, ",", class2.MinLat, ",", class2.MaxLong, ",", class2.MaxLat, ",", class2.AreaID, ",", class2.AreaAlarmType, ",", class2.MaxSpeed }));
                }
                writer.Close();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
        }
    }
}