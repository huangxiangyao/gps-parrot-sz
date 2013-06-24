using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Parrot;

namespace Parrot.Models.Db44
{
    public class Db44WrapperHelper
    {
        /// <summary>
        /// 打包。步骤：填充协议长度，加密协议号和协议内容，计算CRC，转义，Base64编码。
        /// </summary>
        /// <param name="mdtModel"></param>
        /// <param name="data"></param>
        /// <returns>字符串。BASE64编码，不含帧头(0x7e)和帧尾(0x7f)。</returns>
        public static string Pack(int mdtModel, Db44Packet data)
        {
            try
            {
                //填充协议长度
                int dataLength = 2;
                if (data.Body != null)
                {
                    dataLength += data.Body.Length;
                }

                byte[] body = new byte[dataLength];
                body[0] = (byte)(data.FunctionCode[0]);
                body[1] = byte.Parse(data.FunctionCode.Substring(1, 2));

                if (data.Body != null)
                {
                    data.Body.CopyTo(body, 2);
                }

                //加密协议号和协议内容
                byte[] dataAfterEncryption = Db44Encryption.Encrypt(body, 0, dataLength, 251);
                byte[] t = new byte[dataAfterEncryption.Length + 20];
                t[0] = Db44Packet.Mark;
                t[1] = data.SequenceNumber;

                t[2] = (byte)((data.MdtManufacturerCode & 0xff00)>>8);
                t[3] = (byte)((data.MdtManufacturerCode & 0xff));

                t[4] = (byte)((data.MdtId & 0xff000000) >> 24);
                t[5] = (byte)((data.MdtId & 0xff0000) >> 16);
                t[6] = (byte)((data.MdtId & 0xff00) >> 8);
                t[7] = (byte)((data.MdtId & 0xff));
                
                t[8] = (byte)((data.OmcId & 0xff000000) >> 24);
                t[9] = (byte)((data.OmcId & 0xff0000) >> 16);
                t[10] = (byte)((data.OmcId & 0xff00) >> 8);
                t[11] = (byte)((data.OmcId & 0xff));
                
                t[12] = (byte)((data.Pin & 0xff000000) >> 24);
                t[13] = (byte)((data.Pin & 0xff0000) >> 16);
                t[14] = (byte)((data.Pin & 0xff00) >> 8);
                t[15] = (byte)((data.Pin & 0xff));

                t[16] = (byte)((dataAfterEncryption.Length & 0xff00) >> 8);
                t[17] = (byte)((dataAfterEncryption.Length & 0xff));

                dataAfterEncryption.CopyTo(t, 18);

                //计算CRC
                ushort crc = Db44CrcCalculator.CalculateCrc(t, 0, t.Length - 2);
                t[t.Length - 2] = (byte)((crc & 0xff00) >> 8);
                t[t.Length - 1] = (byte)((crc & 0xff));

                //转义
                t = Db44EscapeHelper.Escape(t);

                //Base64编码
                return Convert.ToBase64String(t);
            }
            catch (Exception ex)
            {
                throw new ArithmeticException("无法将指定数据打包成DB44协议数据。",ex);
            }
        }

        /// <summary>
        /// 打包。步骤：填充协议长度，加密协议号和协议内容，计算CRC，转义，Base64编码。  
        /// </summary>
        /// <param name="mdtModel">GPS终端类型，可选值为：251,252,253。</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Pack(int mdtModel, byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");

            byte[] bytes = new byte[] { data[0x11], data[0x12] };
            int num = int.Parse(NumberConverter.ConvertForInt32(Util.BytesToHex(bytes), 0x10, 10));

            Db44Encryption.Encrypt(data, 0x13, 0x13 + num, mdtModel).CopyTo(data, 0x13);

            uint crc = Db44CrcCalculator.CalculateCrc(data, 1, 0x13 + num);
            data[0x13 + num] = (byte)(crc / 0x100);
            data[20 + num] = (byte)(crc % 0x100);
            byte[] destinationArray = new byte[20 + num];
            Array.Copy(data, 1, destinationArray, 0, destinationArray.Length);

            data = Db44EscapeHelper.Escape(destinationArray);

            string s = Convert.ToBase64String(destinationArray);
            byte[] buffer4 = Encoding.Default.GetBytes(s);
            byte[] array = new byte[buffer4.Length + 2];
            buffer4.CopyTo(array, 1);

            array[0] = 0x7e;
            array[array.Length - 1] = 0x7f;

            return array;
        }
    }
}