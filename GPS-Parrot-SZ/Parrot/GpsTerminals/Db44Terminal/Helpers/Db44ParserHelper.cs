using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot;

namespace Parrot.Models.Db44
{
    public class Db44ParserHelper
    {

        /// <summary>
        /// 解包。步骤：Base64解码(输入本函数前已完成)，反转义，校验CRC，解密协议号和协议内容，截取协议长度。
        /// </summary>
        /// <param name="mdtModel"></param>
        /// <param name="pdu">字符串。BASE64编码，但可能含帧头(0x7e)和帧尾(0x7f)。</param>
        /// <param name="hasHeaderAndTail">含不含帧头(0x7e)和帧尾(0x7f)？</param>
        /// <returns></returns>
        /// <exception cref=""/>
        public static Db44Packet Unpack(int mdtModel, string pdu, bool hasHeaderAndTail = true)
        {
            if (hasHeaderAndTail)
            {
                if (pdu[0] == 0x7e) pdu = pdu.Substring(1, pdu.Length - 1);
                if (pdu[pdu.Length - 1] == 0x7f) pdu = pdu.Substring(0, pdu.Length - 1);
            }
            //BASE64解码，不含帧头(0x7e)和帧尾(0x7f)。
            byte[] data = Convert.FromBase64String(pdu);

            //反转义
            data = Db44EscapeHelper.ReverseEscape(data);

            //校验CRC
            ushort crc = Db44CrcCalculator.CalculateCrc(data, 0, data.Length - 2);
            if (crc != ((data[data.Length - 2] << 8) | data[data.Length - 1]))
            {
                throw new ArithmeticException("CRC校验失败。");
            }

            //解密协议号和协议内容
            byte sequenceNumber = data[1];
            ushort mdtManufacturerCode = NumberConverter.GetUInt16(data, 2);
            uint mdtId = NumberConverter.GetUInt32(data, 4);
            uint omcId = NumberConverter.GetUInt32(data, 8);
            uint pin = NumberConverter.GetUInt32(data, 12);

            int dataLength = ((data[16] << 8) | data[17]);
            byte[] bodyWithFunctionCode = Db44Encryption.Encrypt(data, 18, 18 + dataLength, mdtModel);
            string functionCode = string.Format("{0}{1:x2}", (char)bodyWithFunctionCode[0], bodyWithFunctionCode[1]);
            byte[] body = new byte[dataLength - 2];
            Array.Copy(bodyWithFunctionCode,2,body,0,body.Length);

            return new Db44Packet(sequenceNumber, mdtManufacturerCode, mdtId, omcId, pin, functionCode, body);
        }

        /// <summary>
        /// 解包。步骤：Base64解码(输入本函数前已完成)，反转义，校验CRC，解密协议号和协议内容，截取协议长度。
        /// </summary>
        /// <param name="data">传入数据：7E (47...) 7F 括号内的数据。</param>
        /// <param name="mdtModel">GPS终端类型，可选值为：251,252,253。</param>
        /// <returns>输出数据：7E 47... 7F。</returns>
        public static byte[] Unpack(byte[] data, int mdtModel)
        {
            try
            {
                //反转义
                byte[] dataAfterReverse = Db44EscapeHelper.ReverseEscape(data);


                byte[] array = new byte[dataAfterReverse.Length + 2];
                array[0] = 0x7e;
                array[array.Length - 1] = 0x7f;
                dataAfterReverse.CopyTo(array, 1);

                ushort num = Db44CrcCalculator.CalculateCrc(array, 1, array.Length - 3);

                Db44Encryption.Encrypt(array, 0x13, array.Length - 3, mdtModel).CopyTo(array, 0x13);

                return array;
            }
            catch
            {
                return null;
            }
        }
    }
}
