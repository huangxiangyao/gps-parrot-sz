using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models
{
    public static class MdtIdHelper
    {

        public static string GetMdtSn(int mdtModel, uint mdtId)
        {
            if (mdtModel == 252) return GetMdtSnFor252(mdtId);

            return mdtId.ToString("00000000000");
        }
        public static string GetMdtSnFor252(uint mdtId)
        {
            return string.Format("{0:000}{1:000}{2:00000}",
                (byte)((mdtId & 0xff000000)>>24),
                (byte)((mdtId & 0xff0000) >>16),
                (ushort)((mdtId & 0xffff)));
        }


        /// <summary>
        /// 将CarList.Mobile_SN的值转换为符合DB44的四字节的MDT代码。
        /// </summary>
        /// <param name="mobileSN">终端序列号。如：01002010356</param>
        /// <returns>终端ID，如：0169093138</returns>
        public static uint ParseMdtCode(string mobileSN)
        {
            try
            {
                if (mobileSN.Length==3+3+5) return ParseMdtCodeForDb44(mobileSN);

                return ParseMdtCodeForOthers(mobileSN);
            }
            catch {                
            }
            return 0;
        }

        /// <summary>
        /// 将终端序列号(Mobile_SN)换算为终端ID(MdtId)。
        /// </summary>
        /// <param name="mdtModel"></param>
        /// <param name="mobileId">终端序列号。如：01002010258</param>
        /// <returns>终端代码，如：0x0A142812</returns>
        public static uint GetMdtCode(int mdtModel, string mobileId)
        {
            if (mdtModel == 252) return ParseMdtCodeForDb44(mobileId);

            return ParseMdtCodeForOthers(mobileId);
        }

        public static uint ParseMdtCodeForDb44(string mobileId)
        {
            byte a = Util.HexToBytes(NumberConverter.ConvertForInt32(mobileId.Substring(0, 3), 10, 0x10).PadLeft(2, '0'))[0];
            byte b = Util.HexToBytes(NumberConverter.ConvertForInt32(mobileId.Substring(3, 3), 10, 0x10).PadLeft(2, '0'))[0];
            byte c = Util.HexToBytes(NumberConverter.ConvertForInt32(mobileId.Substring(6, 5), 10, 0x10).PadLeft(4, '0'))[0];
            byte d = Util.HexToBytes(NumberConverter.ConvertForInt32(mobileId.Substring(6, 5), 10, 0x10).PadLeft(4, '0'))[1];
            return (uint)((a << 24) | (b << 16) | (c << 8) | (d));
        }

        public static uint ParseMdtCodeForOthers(string simNo)
        {
            simNo = simNo.Substring(1);
            byte num = (byte)(byte.Parse(simNo.Substring(0, 2)) - 30);
            byte A = byte.Parse(simNo.Substring(2, 2));
            if ((num & 8) == 8)
            {
                A = (byte)(A + 0x80);
            }
            byte B = byte.Parse(simNo.Substring(4, 2));
            if ((num & 4) == 4)
            {
                B = (byte)(B + 0x80);
            }
            byte C = byte.Parse(simNo.Substring(6, 2));
            if ((num & 2) == 2)
            {
                C = (byte)(C + 0x80);
            }
            byte D = byte.Parse(simNo.Substring(8, 2));
            if ((num & 1) == 1)
            {
                D = (byte)(D + 0x80);
            }

            return (uint)((A << 24) | (B << 16) | (C << 8) | (D));
        }
    }
}
