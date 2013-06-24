using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using Parrot;

namespace Parrot.Models.Db44
{
    public class Db44Out
    {
        public static SimpleCycleCodeGenerator CycleCode = new SimpleCycleCodeGenerator();

        public Db44Out()
        {
        }

        // Methods
        private byte[] getSetParameterByte(byte T, string ParameterBase64)
        {
            byte[] bytes = Convert.FromBase64String(ParameterBase64);
            switch (T)
            {
                case 1:
                    {
                        string s = Encoding.Default.GetString(bytes).PadLeft(0x11, ' ');
                        return Encoding.Default.GetBytes(s);
                    }
                case 2:
                    {
                        string str2 = Encoding.Default.GetString(bytes).PadLeft(12, '\0');
                        return Encoding.Default.GetBytes(str2);
                    }
                case 3:
                    {
                        string str3 = Encoding.Default.GetString(bytes).PadLeft(12, '\0');
                        return Encoding.Default.GetBytes(str3);
                    }
                case 4:
                    {
                        string hexString = Encoding.Default.GetString(bytes).PadLeft(12, '0');
                        return Util.HexToBytes(hexString);
                    }
                case 5:
                    {
                        string str5 = Encoding.Default.GetString(bytes).PadLeft(0x12, '\0');
                        return Encoding.Default.GetBytes(str5);
                    }
                case 6:
                case 7:
                case 10:
                case 11:
                    return bytes;

                case 8:
                    {
                        string str6 = Encoding.Default.GetString(bytes).PadLeft(6, '0');
                        return Util.HexToBytes(str6);
                    }
                case 9:
                    {
                        string str7 = Encoding.Default.GetString(bytes).PadLeft(12, '0');
                        return Util.HexToBytes(str7);
                    }
                case 12:
                case 13:
                case 14:
                    {
                        string str8 = Encoding.Default.GetString(bytes).PadLeft(15, '\0');
                        return Encoding.Default.GetBytes(str8);
                    }
            }
            return bytes;
        }

        public int cycleCode = 0;
        public string Order(string mobileID, int mdtModel, string[] P)
        {
            int companyCode = 14;
            int password = 9012;
            int centerCode = 8;

            int arg0 = int.Parse(P[0]);
            byte[] array = new byte[2048];
            array[0] = (byte)'~';// 0x7e;
            array[1] = 0x47;
            array[2] = (byte)this.cycleCode;
            byte[] buffer2 = Util.HexToBytes(Convert.ToString(companyCode, 0x10).PadLeft(4, '0'));
            array[3] = buffer2[0];
            array[4] = buffer2[1];
            byte a = 0;
            byte b = 0;
            byte c = 0;
            byte d = 0;
            Db44Util.GetIPFromSimNo(mobileID, out a, out b, out c, out d);
            if (mdtModel == 0xfc)//252
            {
                a = Util.HexToBytes(NumberConverter.ConvertForInt32(mobileID.Substring(0, 3), 10, 0x10).PadLeft(2, '0'))[0];
                b = Util.HexToBytes(NumberConverter.ConvertForInt32(mobileID.Substring(3, 3), 10, 0x10).PadLeft(2, '0'))[0];
                c = Util.HexToBytes(NumberConverter.ConvertForInt32(mobileID.Substring(6, 3), 10, 0x10).PadLeft(4, '0'))[0];
                d = Util.HexToBytes(NumberConverter.ConvertForInt32(mobileID.Substring(6, 3), 10, 0x10).PadLeft(4, '0'))[1];
            }
            else
            {
                Db44Util.GetIPFromSimNo(mobileID, out a, out b, out c, out d);
            }
            array[5] = a;
            array[6] = b;
            array[7] = c;
            array[8] = d;
            byte[] buffer3 = Util.HexToBytes(Convert.ToString(centerCode, 0x10).PadLeft(8, '0'));
            array[9] = buffer3[0];
            array[10] = buffer3[1];
            array[11] = buffer3[2];
            array[12] = buffer3[3];
            byte[] buffer4 = Util.HexToBytes(Convert.ToString(password, 0x10).PadLeft(8, '0'));
            array[13] = buffer4[0];
            array[14] = buffer4[1];
            array[15] = buffer4[2];
            array[0x10] = buffer4[3];
            array[0x13] = 0x44;
            switch (arg0)
            {

                case 0:
                    array[0x11] = 0;
                    array[0x12] = 2;
                    array[20] = 1;//01
                    break;
                case 0x2d:
                    array[0x11] = 0;
                    array[0x12] = 6;
                    array[20] = 2; //02
                    array[0x15] = byte.Parse(P[1]);
                    array[0x16] = 1;
                    array[0x17] = 0xff;
                    array[0x18] = 0xff;
                    break;

                case 0x1a:
                    array[0x11] = 0;
                    array[0x12] = 4;
                    array[20] = 6;//06
                    array[0x15] = byte.Parse(P[0]);
                    array[0x15] = 20;
                    break;   
                case 0x73:
                    {
                        int num6 = int.Parse(P[1]);
                        byte[] buffer5 = new byte[((int.Parse(P[2]) * 8) + (num6 * 2)) + 1];
                        buffer5[0] = (byte)num6;
                        int num8 = 1;
                        string[] strArray = Encoding.Default.GetString(Convert.FromBase64String(P[3])).Split(new char[] { ';' });
                        for (int i = 0; i < num6; i++)
                        {
                            string[] strArray2 = strArray[i].Split(new char[] { ',' });
                            byte num10 = byte.Parse(strArray2[0]);
                            byte num11 = byte.Parse(strArray2[1]);
                            buffer5[num8++] = num10;
                            buffer5[num8++] = num11;
                            for (int j = 0; j < num11; j++)
                            {
                                string str6 = "";
                                string str7 = "";
                                double num13 = 0.0;
                                int num14 = 0;
                                string str9 = strArray2[2 + (j * 2)];
                                str6 = str9.Substring(0, str9.IndexOf('.'));
                                num13 = double.Parse(str9.Substring(str9.IndexOf('.'))) * 60.0;
                                str7 = num13.ToString("00.000").Replace(".", "");
                                num14 = int.Parse(str6 + str7, NumberStyles.HexNumber);
                                buffer5[num8++] = (byte)(num14 / 0x1000000);
                                num14 = num14 % 0x1000000;
                                buffer5[num8++] = (byte)(num14 / 0x10000);
                                num14 = num14 % 0x10000;
                                buffer5[num8++] = (byte)(num14 / 0x100);
                                num14 = num14 % 0x100;
                                buffer5[num8++] = (byte)num14;
                                string str10 = strArray2[3 + (j * 2)];
                                str6 = str10.Substring(0, str10.IndexOf('.'));
                                num13 = double.Parse(str10.Substring(str10.IndexOf('.'))) * 60.0;
                                num14 = int.Parse(str6 + num13.ToString("00.000").Replace(".", ""), NumberStyles.HexNumber);
                                buffer5[num8++] = (byte)(num14 / 0x1000000);
                                num14 = num14 % 0x1000000;
                                buffer5[num8++] = (byte)(num14 / 0x10000);
                                num14 = num14 % 0x10000;
                                buffer5[num8++] = (byte)(num14 / 0x100);
                                num14 = num14 % 0x100;
                                buffer5[num8++] = (byte)num14;
                            }
                        }
                        array[0x11] = 0;
                        array[0x12] = (byte)(2 + buffer5.Length);
                        array[20] = 7;//07
                        buffer5.CopyTo(array, 0x15);
                        break;
                    }
                case 0x74:
                    {
                        byte[] buffer6 = new byte[int.Parse(P[1]) + 1];
                        buffer6[0] = byte.Parse(P[1]);
                        for (int k = 1; k < buffer6.Length; k++)
                        {
                            buffer6[k] = (byte)k;
                        }
                        array[0x11] = 0;
                        array[0x12] = (byte)(2 + buffer6.Length);
                        array[20] = 8;//08
                        buffer6.CopyTo(array, 0x15);
                        break;
                    }
                case 0x75:
                    array[0x11] = 0;
                    array[0x12] = 7;
                    array[20] = 9;//09
                    array[0x15] = byte.Parse(P[1]);
                    array[0x16] = byte.Parse(P[2]);
                    array[0x17] = byte.Parse(P[3]);
                    array[0x18] = byte.Parse(P[4]);
                    array[0x19] = byte.Parse(P[5]);
                    break;
                case 0xf0:
                    array[0x11] = 0;
                    array[0x12] = 3;
                    array[20] = 9;//09
                    array[0x15] = byte.Parse(P[1]);
                    break;


                case 0xf2:
                    array[20] = 10;//0a
                    array[0x15] = byte.Parse(P[1]);
                    array[0x11] = 0;
                    array[0x12] = 3;
                    break;

                case 0x76:
                    array[0x11] = 0;
                    array[0x12] = 2;
                    array[20] = 10;//0a
                    break;

                case 0x77:
                    {
                        int num16 = int.Parse(P[1]);
                        byte[] buffer7 = new byte[((int.Parse(P[2]) * 8) + (num16 * 2)) + 1];
                        buffer7[0] = (byte)num16;
                        int num18 = 1;
                        string[] strArray3 = Encoding.Default.GetString(Convert.FromBase64String(P[3])).Split(new char[] { ';' });
                        for (int m = 0; m < num16; m++)
                        {
                            string[] strArray4 = strArray3[m].Split(new char[] { ',' });
                            byte num20 = byte.Parse(strArray4[0]);
                            byte num21 = byte.Parse(strArray4[1]);
                            buffer7[num18++] = num20;
                            buffer7[num18++] = num21;
                            for (int n = 0; n < num21; n++)
                            {
                                string str13 = "";
                                string str14 = "";
                                double num23 = 0.0;
                                int num24 = 0;
                                string str16 = strArray4[2 + (n * 2)];
                                str13 = str16.Substring(0, str16.IndexOf('.'));
                                num23 = double.Parse(str16.Substring(str16.IndexOf('.'))) * 60.0;
                                str14 = num23.ToString("00.000").Replace(".", "");
                                num24 = int.Parse(str13 + str14, NumberStyles.HexNumber);
                                buffer7[num18++] = (byte)(num24 / 0x1000000);
                                num24 = num24 % 0x1000000;
                                buffer7[num18++] = (byte)(num24 / 0x10000);
                                num24 = num24 % 0x10000;
                                buffer7[num18++] = (byte)(num24 / 0x100);
                                num24 = num24 % 0x100;
                                buffer7[num18++] = (byte)num24;
                                string str17 = strArray4[3 + (n * 2)];
                                str13 = str17.Substring(0, str17.IndexOf('.'));
                                num23 = double.Parse(str17.Substring(str17.IndexOf('.'))) * 60.0;
                                num24 = int.Parse(str13 + num23.ToString("00.000").Replace(".", ""), NumberStyles.HexNumber);
                                buffer7[num18++] = (byte)(num24 / 0x1000000);
                                num24 = num24 % 0x1000000;
                                buffer7[num18++] = (byte)(num24 / 0x10000);
                                num24 = num24 % 0x10000;
                                buffer7[num18++] = (byte)(num24 / 0x100);
                                num24 = num24 % 0x100;
                                buffer7[num18++] = (byte)num24;
                            }
                        }
                        array[0x11] = 0;
                        array[0x12] = (byte)(2 + buffer7.Length);
                        array[20] = 11;//0b
                        buffer7.CopyTo(array, 0x15);
                        break;
                    }
                case 0x78:
                    {
                        byte[] buffer8 = new byte[int.Parse(P[1]) + 1];
                        buffer8[0] = byte.Parse(P[1]);
                        for (int num25 = 1; num25 < buffer8.Length; num25++)
                        {
                            buffer8[num25] = (byte)num25;
                        }
                        array[0x11] = 0;
                        array[0x12] = (byte)(2 + buffer8.Length);
                        array[20] = 12;//0c
                        buffer8.CopyTo(array, 0x15);
                        break;
                    }
                case 0xf1:
                    array[20] = 13;//0d
                    array[0x15] = byte.Parse(P[1]);
                    array[0x11] = 0;
                    array[0x12] = 3;
                    break;
                case 0x79:
                    array[0x11] = 0;
                    array[0x12] = 9;
                    array[20] = 13;//0d
                    array[0x15] = byte.Parse(P[1]);
                    array[0x16] = byte.Parse(P[2]);
                    array[0x17] = byte.Parse(P[3]);
                    array[0x18] = (byte)(int.Parse(P[4]) / 0x100);
                    array[0x19] = (byte)(int.Parse(P[4]) % 0x100);
                    array[0x1a] = (byte)(int.Parse(P[5]) / 0x100);
                    array[0x1b] = (byte)(int.Parse(P[5]) % 0x100);
                    break;

                case 0x7a:
                    array[0x11] = 0;
                    array[0x12] = 2;
                    array[20] = 14;//0e
                    break;
                case 0xf3:
                    array[20] = 14;//0e
                    array[0x15] = byte.Parse(P[1]);
                    array[0x11] = 0;
                    array[0x12] = 3;
                    break;
                case 0x5f:
                    array[0x11] = 0;
                    array[0x12] = 3;
                    array[20] = 15;//0F
                    if (!(P[1] == "0"))
                    {
                        if (P[1] == "1")
                        {
                            array[0x15] = 0x66;
                        }
                        else if (P[1] == "2")
                        {
                            array[0x15] = 0x77;
                        }
                        break;
                    }
                    array[0x15] = 0x55;
                    break;

                case 0x60:
                    {
                        array[0x11] = 0;
                        array[0x12] = 6;
                        array[20] = 0x10;//10
                        array[0x15] = byte.Parse(P[1]);
                        string[] strArray5 = P[2].Split(new char[] { ':' });
                        array[0x16] = Convert.ToByte(strArray5[2], 0x10);
                        array[0x17] = Convert.ToByte(strArray5[3], 0x10);
                        array[0x18] = Convert.ToByte(strArray5[4], 0x10);
                        break;
                    }
                case 0x61:
                    {
                        array[0x11] = 0;
                        array[0x12] = 7;
                        array[20] = 0x11;//11
                        string[] strArray6 = P[1].Split(new char[] { ':' });
                        array[0x15] = Convert.ToByte(strArray6[1], 0x10);
                        array[0x16] = Convert.ToByte(strArray6[2], 0x10);
                        array[0x17] = Convert.ToByte(strArray6[3], 0x10);
                        array[0x18] = Convert.ToByte(strArray6[4], 0x10);
                        array[0x19] = byte.Parse(P[2]);
                        break;
                    }
                case 0x62:
                    array[0x11] = 0;
                    array[0x12] = 2;
                    array[20] = 0x12;//12
                    break;

                case 0x6e:
                    array[0x11] = 0;
                    array[0x12] = 4;
                    array[20] = 20;//14
                    array[0x15] = (byte)(int.Parse(P[1]) / 5);
                    array[0x16] = byte.Parse(P[2]);
                    break;

                case 0x3f:
                    {
                        byte[] buffer9 = Convert.FromBase64String(P[1]);
                        array[0x11] = 0;
                        array[0x12] = (byte)(2 + buffer9.Length);
                        array[20] = 0x15;//15
                        buffer9.CopyTo(array, 0x15);
                        break;
                    }

                case 0xffff://65535
                    array[20] = 0x16; //16
                    array[0x11] = 0;
                    array[0x12] = 2;
                    break;
                case 0xfffe://65534
                    array[20] = 0x17; //17
                    array[0x11] = 0;
                    array[0x12] = 2;
                    break;

                case 6:
                    array[20] = 0x18;//18
                    array[0x11] = 0;
                    array[0x12] = 2;
                    break;
                case 0x0b:
                    {
                        array[0x11] = 0;
                        array[0x12] = 9;
                        array[20] = 0x19;//19
                        array[0x15] = byte.Parse(P[1]);
                        string str18 = DateTime.Now.ToString("yyyyMMddHHmmss");
                        array[0x16] = Convert.ToByte(str18.Substring(2, 2), 0x10);
                        array[0x17] = Convert.ToByte(str18.Substring(4, 2), 0x10);
                        array[0x18] = Convert.ToByte(str18.Substring(6, 2), 0x10);
                        array[0x19] = Convert.ToByte(str18.Substring(8, 2), 0x10);
                        array[0x1a] = Convert.ToByte(str18.Substring(10, 2), 0x10);
                        array[0x1b] = Convert.ToByte(str18.Substring(12, 2), 0x10);
                        break;
                    }

                case 0x6f:
                    {
                        byte num26 = byte.Parse(P[1]);
                        if (num26 != 1)
                        {
                            if (num26 == 2)
                            {
                                byte[] buffer10 = this.getSetParameterByte(byte.Parse(P[2]), P[3]);
                                array[0x11] = (byte)((buffer10.Length + 2) / 0x100);
                                array[0x12] = (byte)((buffer10.Length + 2) % 0x100);
                                array[20] = 0x1b;
                                array[0x15] = byte.Parse(P[2]);
                                buffer10.CopyTo(array, 0x16);
                            }
                            break;
                        }
                        array[0x11] = 0;
                        array[0x12] = 3;
                        array[20] = 0x1a;//1a
                        array[0x15] = byte.Parse(P[2]);
                        break;
                    }
                case 0x30:
                    {
                        array[0x11] = 0;
                        array[0x12] = 9;
                        array[20] = 0x1b;//1b
                        string[] strArray7 = P[2].Split(new char[] { '.' });
                        if (P[1] == "1")
                        {
                            array[0x15] = 10;//
                        }
                        else if (P[1] == "2")
                        {
                            array[0x15] = 11;//
                        }
                        array[0x16] = byte.Parse(strArray7[0]);
                        array[0x17] = byte.Parse(strArray7[1]);
                        array[0x18] = byte.Parse(strArray7[2]);
                        array[0x19] = byte.Parse(strArray7[3]);
                        array[0x1a] = (byte)(int.Parse(P[3]) / 0x100);
                        array[0x1b] = (byte)(int.Parse(P[3]) % 0x100);
                        break;
                    }
                case 7:
                    array[0x11] = 0;
                    array[0x12] = 3;
                    array[20] = 0x1c;//1C
                    array[0x15] = byte.Parse(P[1]);
                    break;
                case 0x70:
                    array[20] = 0x1d;//1d
                    array[0x11] = 0;
                    array[0x12] = 2;
                    break;

                case 0xfffd://65533
                    array[0x11] = 0;
                    array[0x12] = 2;
                    array[20] = 0xe0;//e0
                    break;
                case 0xfffc://65532
                    {
                        array[0x11] = 0;
                        array[0x12] = 8;
                        array[20] = 0xe1;//e1
                        byte[] buffer11 = Util.HexToBytes(DateTime.Now.ToString("yyMMddHHmmss"));
                        array[0x15] = buffer11[0];
                        array[0x16] = buffer11[1];
                        array[0x17] = buffer11[2];
                        array[0x18] = buffer11[3];
                        array[0x19] = buffer11[4];
                        array[0x1a] = buffer11[5];
                        break;
                    }
                case 0xfffb://65531
                    array[0x11] = 0;
                    array[0x12] = 3;
                    array[20] = 0xe2; //e2
                    array[0x15] = byte.Parse(P[1]);
                    break;


                case 0x10000://65536
                    array[0x11] = 0;
                    array[0x12] = 2;
                    array[20] = 0xf0; //f0
                    break;
                case 0x71:
                    array[0x11] = 0;
                    array[0x12] = 3;
                    array[20] = 0xf3;//f3
                    array[0x15] = byte.Parse(P[1]);
                    break;
                case 0xfffa://65530
                    array[0x11] = 0;
                    array[0x12] = 3;
                    array[20] = 0xf7;//f7
                    array[0x15] = byte.Parse(P[1]);
                    break;
            }
            byte[] inArray = Db44WrapperHelper.Pack(mdtModel, array);
            return Convert.ToBase64String(inArray, 0, inArray.Length);
        }
    }
}