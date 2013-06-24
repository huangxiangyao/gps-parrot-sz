using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Longhan
{
    public class LonghanOut
    {
        // Fields
        private byte[] CmdByte = new byte[0x800];
        private string filePath;
        private LH101G_GPRS_PE_Out LH101G_Out = new LH101G_GPRS_PE_Out();
        private LH108_GPRS_PE_Out LH108_Out = new LH108_GPRS_PE_Out();
        private LH12_GPRS_PE_Out LH12_Out = new LH12_GPRS_PE_Out();
        private LH14_GPRS_PE_Out LH14_Out = new LH14_GPRS_PE_Out();
        private LH16_GPRS_PE_Out LH16_Out = new LH16_GPRS_PE_Out();
        private LH208_GPRS_PE_Out LH208_Out = new LH208_GPRS_PE_Out();
        private LHA6_GPRS_PE_Out LHA6GS_Out = new LHA6_GPRS_PE_Out();

        // Methods
        public LonghanOut()
        {
            this.CmdByte[0] = 0x29;
            this.CmdByte[1] = 0x29;
            this.filePath = Environment.CurrentDirectory;
        }

        public string Order(string _ID, int MobileType, string[] P)
        {
            int num = int.Parse(P[0]);
            string str = "";
            switch (MobileType)
            {
                case 0xc9:
                    return this.LH12_Out.Order(_ID, MobileType, P);

                case 0xca:
                    return this.LH14_Out.Order(_ID, MobileType, P);

                case 0xcb:
                    return this.LH16_Out.Order(_ID, MobileType, P);

                case 0xcc:
                case 210:
                    return str;

                case 0xcd:
                case 0xcf:
                    return this.LH108_Out.Order(_ID, MobileType, P);

                case 0xce:
                    return this.LHA6GS_Out.Order(_ID, MobileType, P);

                case 0xd0:
                    return this.LH16_Out.Order(_ID, MobileType, P);

                case 0xd1:
                    return this.LH208_Out.Order(_ID, MobileType, P);

                case 0xd3:
                    return this.LH101G_Out.Order(_ID, MobileType, P);

                case 230:
                    return this.LH12_Out.Order(_ID, MobileType, P);
            }
            return str;
        }
    }
}