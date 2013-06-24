using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Longhan
{
    public class LonghanWrapper
    {
        // Methods
        public static string Get_CarID_From_IP(byte A, byte B, byte C, byte D)
        {
            byte num = 0;
            if (A >= 0x80)
            {
                num = 8;
                A = (byte)(A - 0x80);
            }
            if (B >= 0x80)
            {
                num = (byte)(num + 4);
                B = (byte)(B - 0x80);
            }
            if (C >= 0x80)
            {
                num = (byte)(num + 2);
                C = (byte)(C - 0x80);
            }
            if (D >= 0x80)
            {
                num = (byte)(num + 1);
                D = (byte)(D - 0x80);
            }
            if (num > 9)
            {
                num = (byte)(num + 0x10);
            }
            string[] strArray = new string[] { (num + 130).ToString(), A.ToString("00"), B.ToString("00"), C.ToString("00"), D.ToString("00") };
            return string.Concat(strArray);
        }

        public static byte Get_CheckXor(ref byte[] temp, int len)
        {
            byte num = (byte)(temp[0] ^ temp[1]);
            for (int i = 2; i < len; i++)
            {
                num = (byte)(num ^ temp[i]);
            }
            return num;
        }

        public static void Get_IP_From_CarID(string CarID, out byte A, out byte B, out byte C, out byte D)
        {
            byte num = (byte)(byte.Parse(CarID.Substring(0, 2)) - 30);
            A = byte.Parse(CarID.Substring(2, 2));
            if ((num & 8) == 8)
            {
                A = (byte)(A + 0x80);
            }
            B = byte.Parse(CarID.Substring(4, 2));
            if ((num & 4) == 4)
            {
                B = (byte)(B + 0x80);
            }
            C = byte.Parse(CarID.Substring(6, 2));
            if ((num & 2) == 2)
            {
                C = (byte)(C + 0x80);
            }
            D = byte.Parse(CarID.Substring(8, 2));
            if ((num & 1) == 1)
            {
                D = (byte)(D + 0x80);
            }
        }

        public static string Get_TableName(byte i)
        {
            string str = "";
            switch (i)
            {
                case 0x80:
                    return ("DynData" + DateTime.Now.Day.ToString());

                case 0x81:
                    return ("DynData" + DateTime.Now.Day.ToString());

                case 130:
                    return ("DynData" + DateTime.Now.Day.ToString());

                case 0x89:
                    return "PassPoint";

                case 0x8a:
                    return "VehLogin";

                case 0x8e:
                    return ("DynData" + DateTime.Now.Day.ToString());

                case 0x8f:
                    return str;

                case 0x90:
                    return ("WorkData" + DateTime.Now.Day.ToString());
            }
            return str;
        }
    }
}