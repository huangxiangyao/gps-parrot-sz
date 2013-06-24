using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Db44
{
    public static class Db44Util
    {
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

        public static void GetIPFromSimNo(string simNo, out byte A, out byte B, out byte C, out byte D)
        {
            simNo = simNo.Substring(1);
            byte num = (byte)(byte.Parse(simNo.Substring(0, 2)) - 30);
            A = byte.Parse(simNo.Substring(2, 2));
            if ((num & 8) == 8)
            {
                A = (byte)(A + 0x80);
            }
            B = byte.Parse(simNo.Substring(4, 2));
            if ((num & 4) == 4)
            {
                B = (byte)(B + 0x80);
            }
            C = byte.Parse(simNo.Substring(6, 2));
            if ((num & 2) == 2)
            {
                C = (byte)(C + 0x80);
            }
            D = byte.Parse(simNo.Substring(8, 2));
            if ((num & 1) == 1)
            {
                D = (byte)(D + 0x80);
            }
        }

        public static string GetSimNoFromIP(byte A, byte B, byte C, byte D)
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
                
    }
}
