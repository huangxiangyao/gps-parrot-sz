using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.IO;

namespace Parrot.Models.TianheGprs
{
    /// <summary>
    /// 天禾终端。
    /// </summary>
    public class TianheGprsOut
    {
        // Fields
        private string CmdHeard = "*HQ";
        private string CmdStr;
        private string CmdTimeNow;
        private string filePath;
        public int MsgID = 0;

        // Methods
        public TianheGprsOut()
        {
            this.MsgID = 0;
            this.CmdTimeNow = "";
            this.CmdStr = "";
            this.filePath = Environment.CurrentDirectory;
            this.Read_Stop_OvertimeInfo();
        }

        private FileInfo[] ListFile(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            return info.GetFiles("*.ini");
        }

        public string Order(string _ID, int MobileType, string[] P)
        {
            if (MobileType == 0x2bd)
            {
                this.CmdHeard = "*DG";
            }
            else
            {
                this.CmdHeard = "*HQ";
            }
            this.CmdTimeNow = DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");
            switch (int.Parse(P[0]))
            {
                case 0:
                    this.CmdStr = this.CmdHeard + ",0000,S1," + this.CmdTimeNow + "#";
                    goto Label_1B85;

                case 1:
                    this.CmdStr = this.CmdHeard + ",0000,D1," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "#";
                    goto Label_1B85;

                case 2:
                    this.CmdStr = this.CmdHeard + ",0000,S17," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 3:
                    {
                        int num3 = Convert.ToInt32(P[1] + P[2], 2);
                        this.CmdStr = this.CmdHeard + ",0000,S12," + this.CmdTimeNow + "," + num3.ToString("X2") + "#";
                        goto Label_1B85;
                    }
                case 4:
                    this.CmdStr = this.CmdHeard + ",0000,S2," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 5:
                    if (P[1].ToString() == "1")
                    {
                        P[1] = "00";
                    }
                    if (P[1].ToString() == "2")
                    {
                        P[1] = "02";
                    }
                    if (P[1].ToString() == "3")
                    {
                        P[1] = "01";
                    }
                    if (P[1].ToString() == "4")
                    {
                        P[1] = "03";
                    }
                    this.CmdStr = this.CmdHeard + ",0000,S13," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 6:
                    this.CmdStr = this.CmdHeard + ",0000,R7," + this.CmdTimeNow + "#";
                    goto Label_1B85;

                case 7:
                    this.CmdStr = this.CmdHeard + ",0000,R1," + this.CmdTimeNow + "#";
                    goto Label_1B85;

                case 8:
                    P[1] = int.Parse(P[1]).ToString("0");
                    P[2] = int.Parse(P[2]).ToString("0");
                    this.CmdStr = this.CmdHeard + ",0000,S14," + this.CmdTimeNow + "," + P[1] + "," + P[2] + ",1," + P[4] + "#";
                    goto Label_1B85;

                case 9:
                    {
                        string str = P[2];
                        if (str != null)
                        {
                            str = string.IsInterned(str);
                            if (str == "0")
                            {
                                P[2] = "0";
                            }
                            else
                            {
                                if (str == "1")
                                {
                                    P[2] = "9";
                                    break;
                                }
                                if (str == "2")
                                {
                                    P[2] = "1";
                                    break;
                                }
                                if (str == "3")
                                {
                                    P[2] = "2";
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case 10:
                    P[1] = "A" + P[1];
                    this.CmdStr = this.CmdHeard + ",0000,S19," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "," + P[3] + ",0#";
                    goto Label_1B85;

                case 11:
                    if (!(P[1] == "0"))
                    {
                        if (P[1] == "1")
                        {
                            this.CmdStr = this.CmdHeard + ",0000,S20," + this.CmdTimeNow + ",1,1#";
                        }
                        else if (P[1] == "2")
                        {
                            this.CmdStr = this.CmdHeard + ",0000,R7," + this.CmdTimeNow + "#";
                        }
                    }
                    else
                    {
                        this.CmdStr = this.CmdHeard + ",0000,S20," + this.CmdTimeNow + ",1,0#";
                    }
                    goto Label_1B85;

                case 12:
                    if (!((P[2] == "0") | (P[2] == "5")))
                    {
                        double num4 = 0.0;
                        num4 = double.Parse(P[4].Substring(P[4].IndexOf("."))) * 60.0;
                        P[4] = P[4].Substring(0, P[4].IndexOf(".")) + num4.ToString("00.000");
                        num4 = double.Parse(P[5].Substring(P[5].IndexOf("."))) * 60.0;
                        P[5] = P[5].Substring(0, P[5].IndexOf(".")) + num4.ToString("00.000");
                        num4 = double.Parse(P[6].Substring(P[6].IndexOf("."))) * 60.0;
                        P[6] = P[6].Substring(0, P[6].IndexOf(".")) + num4.ToString("00.000");
                        P[3] = P[3].Substring(0, P[3].IndexOf(".")) + ((double.Parse(P[3].Substring(P[3].IndexOf("."))) * 60.0)).ToString("00.000");
                    }
                    else
                    {
                        P[3] = "0.0";
                        P[4] = "0.0";
                        P[5] = "0.0";
                        P[6] = "0.0";
                    }
                    this.CmdStr = this.CmdHeard + ",0000,S21," + this.CmdTimeNow + "," + P[1] + "," + P[2] + ",N," + P[4] + "," + P[6] + ",E," + P[3] + "," + P[5] + "#";
                    goto Label_1B85;

                case 13:
                    this.CmdStr = this.CmdHeard + ",0000,S22," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 14:
                    this.CmdStr = this.CmdHeard + ",0000,S21," + this.CmdTimeNow + ",0,A" + P[1] + "," + P[2] + ",N," + P[3] + "," + P[4] + ",E," + P[5] + "," + P[6] + "#";
                    goto Label_1B85;

                case 15:
                    this.CmdStr = this.CmdHeard + ",0000,A1," + this.CmdTimeNow + "#";
                    goto Label_1B85;

                case 0x10:
                    this.CmdStr = this.CmdHeard + ",0000,R8," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 0x11:
                    P[1] = P[1].Replace(".", ",");
                    this.CmdStr = this.CmdHeard + ",0000,S23," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "," + P[3] + "#";
                    goto Label_1B85;

                case 0x12:
                    this.CmdStr = this.CmdHeard + ",0000,S24," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "#";
                    goto Label_1B85;

                case 0x13:
                    this.CmdStr = this.CmdHeard + ",0000,S25," + this.CmdTimeNow + "#";
                    goto Label_1B85;

                case 20:
                    this.CmdStr = this.CmdHeard + ",0000,S26," + this.CmdTimeNow + "#";
                    goto Label_1B85;

                case 0x15:
                    this.CmdStr = this.CmdHeard + ",0000,S1," + this.CmdTimeNow + "#";
                    goto Label_1B85;

                case 0x16:
                    this.CmdStr = this.CmdHeard + ",0000,S5," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "," + P[3] + "#";
                    goto Label_1B85;

                case 0x17:
                    this.CmdStr = this.CmdHeard + ",0000,S28," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 0x18:
                    this.CmdStr = this.CmdHeard + ",0000,S31," + this.CmdTimeNow + "," + P[1] + ",0," + P[2] + ",0#";
                    goto Label_1B85;

                case 0x19:
                    this.CmdStr = this.CmdHeard + ",0000,S32," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 0x1a:
                    P[1] = (((double)int.Parse(P[1])) / 1.852).ToString("0");
                    this.CmdStr = this.CmdHeard + ",0000,S33," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 0x1b:
                    P[2] = "000000" + P[2];
                    P[2] = Convert.ToInt32(P[2], 2).ToString("X2");
                    P[3] = Convert.ToInt32(P[3], 2).ToString("X2");
                    P[4] = Convert.ToInt32(P[4], 2).ToString("X2");
                    this.CmdStr = this.CmdHeard + ",0000,S15," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "," + P[3] + "," + P[4] + "#";
                    goto Label_1B85;

                case 0x1c:
                    this.CmdStr = this.CmdHeard + ",0000,S3," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 0x1d:
                    P[2] = "FF";
                    P[1] = Convert.ToInt32(P[1], 2).ToString("X2");
                    this.CmdStr = this.CmdHeard + ",0000,S4," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "#";
                    goto Label_1B85;

                case 30:
                    this.CmdStr = this.CmdHeard + ",0000,S16," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 0x1f:
                    this.CmdStr = this.CmdHeard + ",0000,S40," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "," + P[3] + "," + P[4] + "," + P[5] + ",FF#";
                    goto Label_1B85;

                case 0x20:
                    this.CmdStr = this.CmdHeard + ",0000,D1," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "#";
                    goto Label_1B85;

                case 0x21:
                    this.CmdStr = this.CmdHeard + ",0000,S17," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "#";
                    goto Label_1B85;

                case 0x22:
                    this.CmdStr = this.CmdHeard + ",0000,S30," + this.CmdTimeNow + ",A" + P[1] + "#";
                    goto Label_1B85;

                case 0x23:
                    this.CmdStr = this.CmdHeard + ",0000,S27," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 0x24:
                    this.CmdStr = this.CmdHeard + ",0000,S34," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 0x25:
                    {
                        int len = 0;
                        P[1] = Encoding.Default.GetString(Convert.FromBase64String(P[1]));
                        P[1] = this.TxtCode(P[1], out len);
                        this.CmdStr = string.Concat(new object[] { this.CmdHeard, ",0000,I1,", this.CmdTimeNow, ",", P[2], ",0,", len, ",", P[1] });
                        goto Label_1B85;
                    }
                case 0x26:
                    {
                        this.CmdStr = this.CmdHeard + ",0000,S37," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "," + P[3] + ",";
                        if (!(P[3] == "2"))
                        {
                            this.CmdStr = this.CmdStr + "#";
                            goto Label_1B85;
                        }
                        byte[] array = new byte[this.CmdStr.Length + 0x101];
                        byte[] bytes = Encoding.Default.GetBytes(this.CmdStr);
                        bytes.CopyTo(array, 0);
                        Convert.FromBase64String(P[4]).CopyTo(array, bytes.Length);
                        array[array.Length - 1] = 0x23;
                        bytes = null;
                        this.CmdStr = Convert.ToBase64String(array);
                        array = null;
                        return this.CmdStr;
                    }
                case 0x27:
                    this.CmdStr = this.CmdHeard + ",0000,S38," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "," + P[3] + "," + P[4] + "," + P[5] + "," + P[6] + "#";
                    goto Label_1B85;

                case 40:
                    this.CmdStr = this.CmdHeard + ",0000,S35," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "#";
                    goto Label_1B85;

                case 0x29:
                    this.CmdStr = this.CmdHeard + ",0000,S36," + this.CmdTimeNow + ",1," + P[1] + "," + P[2] + "#";
                    goto Label_1B85;

                case 0x2a:
                    {
                        this.CmdStr = this.CmdHeard + ",0000,S39," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "," + P[3] + "," + P[4] + "#";
                        TianheGprsIn.RecvImage_Hash.Remove(_ID);
                        ImgBody body = new ImgBody();
                        body.NewImg();
                        body.VcdID = int.Parse(P[2]);
                        body.CarID = _ID;
                        body.ImgNo = DateTime.UtcNow.Ticks.ToString();
                        TianheGprsIn.RecvImage_Hash.Add("1" + _ID, body);
                        goto Label_1B85;
                    }
                case 0x2b:
                    this.CmdStr = this.CmdHeard + ",0000,S6," + this.CmdTimeNow + "," + P[1] + "#";
                    goto Label_1B85;

                case 0x2c:
                    return "";

                case 0x2e:
                    if (!(P[1] == "0"))
                    {
                        this.CmdStr = this.CmdHeard + ",0000,S6," + this.CmdTimeNow + ",0#";
                    }
                    else
                    {
                        this.CmdStr = this.CmdHeard + ",0000,S6," + this.CmdTimeNow + ",1#";
                    }
                    goto Label_1B85;

                case 0x2f:
                    {
                        Stop_OvertimeInfoClass class2 = new Stop_OvertimeInfoClass();
                        class2.Inti(int.Parse(P[1]));
                        if (Stop_OvertimeInfoClass.Stop_OvertimeInfo_HashList.ContainsKey(_ID))
                        {
                            Stop_OvertimeInfoClass.Stop_OvertimeInfo_HashList.Remove(_ID);
                        }
                        Stop_OvertimeInfoClass.Stop_OvertimeInfo_HashList.Add(_ID, class2);
                        this.Save_Stop_OvertimeInfo(_ID, int.Parse(P[1]));
                        return "";
                    }
                case 0x42:
                    this.CmdStr = this.CmdHeard + ",0000,S17," + this.CmdTimeNow + "," + P[1] + ",0#";
                    goto Label_1B85;

                case 0x43:
                    return "";

                case 0x45:
                    P[1] = (double.Parse(P[1]) / 1.852).ToString("0");
                    this.CmdStr = this.CmdHeard + ",0000,S14," + this.CmdTimeNow + "," + P[1] + ",0,1,10#";
                    goto Label_1B85;

                case 0x47:
                    {
                        byte[] buffer4 = Convert.FromBase64String(P[1]);
                        byte[] buffer5 = new byte[0x1c + buffer4.Length];
                        buffer5[0] = 0x2a;
                        buffer5[1] = 0x48;
                        buffer5[2] = 0x51;
                        buffer5[3] = 0x2c;
                        buffer5[4] = 0x30;
                        buffer5[5] = 0x30;
                        buffer5[6] = 0x30;
                        buffer5[7] = 0x30;
                        buffer5[8] = 0x2c;
                        buffer5[9] = 0x49;
                        buffer5[10] = 0x34;
                        buffer5[11] = 0x2c;
                        buffer5[12] = 0x30;
                        buffer5[13] = 0x30;
                        buffer5[14] = 0x30;
                        buffer5[15] = 0x30;
                        buffer5[0x10] = 0x30;
                        buffer5[0x11] = 0x30;
                        buffer5[0x12] = 0x2c;
                        buffer5[0x13] = 0x36;
                        buffer5[20] = 0x30;
                        buffer5[0x15] = 0x2c;
                        buffer5[0x16] = 0x30;
                        buffer5[0x17] = 0x2c;
                        buffer5[0x18] = (byte)(0x30 + (buffer4.Length / 100));
                        buffer5[0x19] = (byte)(0x30 + ((buffer4.Length % 100) / 10));
                        buffer5[0x1a] = (byte)(0x30 + ((buffer4.Length % 100) % 10));
                        buffer5[0x1b] = 0x2c;
                        buffer4.CopyTo(buffer5, 0x1c);
                        this.CmdStr = Convert.ToBase64String(buffer5, 0, buffer5.Length);
                        return this.CmdStr;
                    }
                case 0x58:
                    {
                        int num2 = int.Parse(P[1]) * 0xe10;
                        if (num2 > 0xfd20)
                        {
                            num2 = 0xfd20;
                        }
                        this.CmdStr = this.CmdHeard + ",0000,S17," + this.CmdTimeNow + "," + num2.ToString() + "#";
                        goto Label_1B85;
                    }
                case 90:
                    {
                        ArrayList list = new ArrayList();
                        for (int i = 0; i < int.Parse(P[1]); i++)
                        {
                            double num7 = double.Parse(P[(i * 7) + 2]);
                            double num8 = double.Parse(P[(i * 7) + 3]);
                            double num9 = double.Parse(P[(i * 7) + 4]);
                            double num10 = double.Parse(P[(i * 7) + 5]);
                            byte num11 = byte.Parse(P[(i * 7) + 6]);
                            byte num12 = byte.Parse(P[(i * 7) + 7]);
                            byte num13 = byte.Parse(P[(i * 7) + 8]);
                        }
                        return "";
                    }
                case 0x5c:
                    {
                        string str2 = P[4];
                        string str3 = "";
                        byte[] buffer6 = Encoding.BigEndianUnicode.GetBytes(P[5]);
                        for (int j = 0; j < buffer6.Length; j++)
                        {
                            str3 = str3 + buffer6[j].ToString("X2");
                        }
                        string str4 = Convert.ToInt32(P[1], 2).ToString("X2");
                        string str5 = Convert.ToInt32(P[2], 2).ToString("X2");
                        string str6 = Convert.ToInt32(P[3], 2).ToString("X2");
                        string str7 = str4 + str5 + str6;
                        string str8 = P[6];
                        this.CmdStr = this.CmdHeard + ",0000,S9," + this.CmdTimeNow + "," + str2 + "," + str3 + "," + str7 + "," + str8 + "#";
                        goto Label_1B85;
                    }
                default:
                    return "Err";
            }
            this.CmdStr = this.CmdHeard + ",0000,S18," + this.CmdTimeNow + "," + P[1] + "," + P[2] + "#";
        Label_1B85:
            this.MsgID++;
            if (this.MsgID > 0xff)
            {
                this.MsgID = 0;
            }
            Debug.WriteLine(this.CmdStr);
            this.CmdStr = Convert.ToBase64String(Encoding.Default.GetBytes(this.CmdStr));
            return this.CmdStr;
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

        private void Read_Stop_OvertimeInfo()
        {
            FileInfo[] infoArray = this.ListFile(this.filePath + @"\Stop_OvertimeInfo");
            foreach (FileInfo info in infoArray)
            {
                string key = info.Name.Replace(".ini", "");
                string message = this.filePath + @"\Stop_OvertimeInfo\" + info.Name;
                Debug.WriteLine(message);
                StreamReader reader = new StreamReader(message);
                if (reader != null)
                {
                    string str3 = null;
                    do
                    {
                        try
                        {
                            str3 = reader.ReadLine();
                            if (str3 == null)
                            {
                                break;
                            }
                            string s = str3;
                            if (s != "0")
                            {
                                Stop_OvertimeInfoClass class2 = new Stop_OvertimeInfoClass();
                                class2.Inti(int.Parse(s));
                                Stop_OvertimeInfoClass.Stop_OvertimeInfo_HashList.Add(key, class2);
                            }
                        }
                        catch (Exception exception)
                        {
                            Debug.WriteLine("Error---" + exception.ToString());
                        }
                    }
                    while (str3 != null);
                }
                reader.Close();
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
                    writer.WriteLine(string.Concat(new object[] { class2.MinLong, ",", class2.MinLat, ",", class2.MaxLong, ",", class2.MaxLat, ",", class2.AreaID, ",", class2.AlarmType, ",", class2.MaxSpeed }));
                }
                writer.Close();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
        }

        private void Save_Stop_OvertimeInfo(string _ID, int P)
        {
            try
            {
                new FileInfo(this.filePath + @"\Stop_OvertimeInfo\" + _ID + ".ini").Delete();
                StreamWriter writer = new StreamWriter(new FileStream(this.filePath + @"\Stop_OvertimeInfo\" + _ID + ".ini", FileMode.OpenOrCreate, FileAccess.Write));
                writer.AutoFlush = true;
                writer.WriteLine(P.ToString());
                writer.Close();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
        }

        private string TxtCode(string Temp, out int Len)
        {
            int num = 0;
            string str = "";
            for (int i = 0; i < Temp.Length; i++)
            {
                if (Encoding.Default.GetBytes(Temp.Substring(i, 1)).Length > 1)
                {
                    str = str + Temp.Substring(i, 1);
                }
                else
                {
                    str = str + '\0' + Temp.Substring(i, 1);
                }
                num += 2;
            }
            Len = num;
            return str;
        }
    }
}