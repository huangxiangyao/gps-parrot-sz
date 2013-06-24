using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot;

namespace Parrot.Protocols.Jtj
{
    /// <summary>
    /// 解析交通局下行数据包。
    /// </summary>
    public static class DownloadDataParser
    {
        /// <summary>
        /// 解析D06(下发“图片请求数据包”给GPS终端)。
        /// </summary>
        /// <param name="pdu"></param>
        /// <param name="plateNumber"></param>
        /// <param name="plateColor"></param>
        /// <param name="pictureRequestType"></param>
        /// <param name="cameraNumber"></param>
        /// <returns></returns>
        public static bool D06(byte[] pdu, out string plateNumber, out byte plateColor, out byte pictureRequestType, out byte cameraNumber)
        {
            plateNumber = null;
            plateColor = 0;
            pictureRequestType = 0;
            cameraNumber = 0;

            if (!Validate(pdu, "D06")) return false;

            try
            {
                int bodyLength = ParseBodyLength(pdu);

                int startPos = 15;
                int endPos = 0;

                endPos = Array.IndexOf<byte>(pdu, (byte)'|', startPos);
                plateNumber = Encoding.Default.GetString(pdu, startPos, endPos - startPos);

                endPos += 1;
                plateColor = pdu[endPos];

                endPos += 1;
                pictureRequestType = pdu[endPos];
                
                endPos += 1;
                cameraNumber = pdu[endPos];

                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 解析D05(下发“事故疑点信息请求数据包”给GPS终端)。
        /// </summary>
        /// <param name="pdu"></param>
        /// <param name="plateNumber"></param>
        /// <param name="plateColor"></param>
        /// <param name="trafficPacketIndex"></param>
        /// <returns></returns>
        public static bool D05(byte[] pdu, out string plateNumber, out byte plateColor, out byte trafficPacketIndex)
        {
            plateNumber = null;
            plateColor = 0;
            trafficPacketIndex = 0;

            if (!Validate(pdu, "D05")) return false;

            try
            {
                int bodyLength = ParseBodyLength(pdu);

                int startPos = 15;
                int endPos = 0;

                endPos = Array.IndexOf<byte>(pdu, (byte)'|', startPos);
                plateNumber = Encoding.Default.GetString(pdu, startPos, endPos - startPos);

                endPos += 1;
                plateColor = pdu[endPos];

                endPos += 1;
                trafficPacketIndex = pdu[endPos];

                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 解析D04(下发“车辆静态信息请求数据包”给GPS终端)。
        /// </summary>
        /// <param name="pdu"></param>
        /// <param name="plateNumber"></param>
        /// <param name="plateColor"></param>
        /// <returns></returns>
        public static bool D04(byte[] pdu, out string plateNumber, out byte plateColor)
        {
            plateNumber = null;
            plateColor = 0;
            
            if (!Validate(pdu, "D04")) return false;

            try
            {
                int bodyLength = ParseBodyLength(pdu);

                int startPos = 15;
                int endPos = 0;

                endPos = Array.IndexOf<byte>(pdu, (byte)'|', startPos);
                plateNumber = Encoding.Default.GetString(pdu, startPos, endPos - startPos);

                plateColor = pdu[endPos + 1];

                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 解析D03(下发“提示信息数据包”给GPS终端)。
        /// </summary>
        /// <param name="pdu"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool D03(byte[] pdu, out JtjD03 result)
        {
            result = null;

            if (!Validate(pdu, "D03")) return false;

            try
            {
                result = new JtjD03();

                #region parse...

                int bodyLength = ParseBodyLength(pdu);

                int startPos = 15;
                int endPos = 0;
                int nFieldIndex = 0;

                for (int i = 0; i < bodyLength; i++)
                {
                    if (pdu[i + 15] != 0x7c)
                    {
                        continue;
                    }
                    nFieldIndex++;
                    endPos = i + 15;
                    switch (nFieldIndex)
                    {
                        case 1:
                            try
                            {
                                result.Id = int.Parse(ASCIIEncoding.ASCII.GetString(pdu, startPos, endPos - startPos));
                            }
                            catch { }
                            continue;

                        case 2:
                            try
                            {
                                result.PlateNumber = Encoding.Default.GetString(pdu, startPos, endPos - startPos);
                            }
                            catch { }
                            continue;

                        case 3:
                            result.PlateColor = pdu[startPos];
                            continue;

                        case 4:
                            try
                            {
                                result.CreateDate = DateTime.Parse(
                                    string.Format("20{0:X2}-{1:X2}-{2:X2} {3:X2}:{4:X2}:{5:X2}",
                                    pdu[startPos],
                                    pdu[startPos + 1],
                                    pdu[startPos + 2],
                                    pdu[startPos + 3],
                                    pdu[startPos + 4],
                                    pdu[startPos + 5]));
                            }
                            catch { }
                            continue;

                        case 5:
                            try
                            {
                                result.Text = Base64ToGbk(pdu, startPos, endPos - startPos);
                            }
                            catch { }

                            startPos = endPos + 1;
                            try
                            {
                                result.Sender = Base64ToGbk(pdu, startPos, endPos - startPos);
                            }
                            catch { }

                            break;
                    }
                }
                #endregion

                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 解析D02(下发“违法信息数据包”给GPS终端)。
        /// </summary>
        /// <param name="pdu"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool D02(byte[] pdu, out JtjD02 result)
        {
            result = null;

            if (!Validate(pdu, "D02")) return false;

            try
            {
                result = new JtjD02();

                #region parse...

                int bodyLength = ParseBodyLength(pdu);

                int startPos = 15;
                int endPos = 0;
                int nFieldIndex = 0;

                for (int i = 0; i < bodyLength; i++)
                {
                    if (pdu[i + 15] != 0x7c)
                    {
                        continue;
                    }
                    nFieldIndex++;
                    endPos = i + 15;
                    switch (nFieldIndex)
                    {
                        case 1:
                            try
                            {
                                result.Id = int.Parse(ASCIIEncoding.ASCII.GetString(pdu, startPos, endPos - startPos));
                            }
                            catch { }
                            continue;

                        case 2:
                            try
                            {
                                result.PlateNumber = Encoding.Default.GetString(pdu, startPos, endPos - startPos);
                            }
                            catch { }
                            continue;

                        case 3:
                            result.PlateColor = pdu[startPos];
                            continue;

                        case 4:
                            try
                            {
                                result.CreateDate = DateTime.Parse(
                                    string.Format("20{0:X2}-{1:X2}-{2:X2} {3:X2}:{4:X2}:{5:X2}",
                                    pdu[startPos],
                                    pdu[startPos + 1],
                                    pdu[startPos + 2],
                                    pdu[startPos + 3],
                                    pdu[startPos + 4],
                                    pdu[startPos + 5]));
                            }
                            catch { }
                            continue;

                        case 5:
                            try
                            {
                                result.Text = Base64ToGbk(pdu, startPos, endPos - startPos);
                            }
                            catch { }
                            continue;
                        case 6:
                            try
                            {
                                result.City = Base64ToGbk(pdu, startPos, endPos - startPos);
                            }
                            catch { }

                            startPos = endPos + 1;
                            try
                            {
                                result.Street = Base64ToGbk(pdu, startPos, endPos - startPos);
                            }
                            catch { }
                            break;
                    }
                }
                #endregion

                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 解析D01(下发“警告信息数据包”给GPS终端)。
        /// </summary>
        /// <param name="pdu"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool D01(byte[] pdu, out JtjD01 result)
        {
            result = null;

            if (!Validate(pdu, "D01")) return false;

            try
            {
                result = new JtjD01();

                #region parse...
                int bodyLength = ParseBodyLength(pdu);

                int startPos = 15;
                int endPos = 0;
                int nFieldIndex = 0;

                for (int i = 0; i < bodyLength; i++)
                {
                    if (pdu[i + 15] != (byte)'|')//0x7c
                    {
                        continue;
                    }
                    nFieldIndex++;
                    endPos = i + 15;
                    switch (nFieldIndex)
                    {
                        case 1:
                            try
                            {
                                result.Id = int.Parse(ASCIIEncoding.ASCII.GetString(pdu, startPos, endPos - startPos));
                            }
                            catch { }
                            continue;

                        case 2:
                            try
                            {
                                result.PlateNumber = Encoding.Default.GetString(pdu, startPos, endPos - startPos);
                            }
                            catch { }
                            continue;

                        case 3:
                            result.PlateColor = pdu[startPos];
                            continue;

                        case 4:
                            try
                            {
                                result.CreateDate = DateTime.Parse(
                                    string.Format("20{0:X2}-{1:X2}-{2:X2} {3:X2}:{4:X2}:{5:X2}",
                                    pdu[startPos],
                                    pdu[startPos + 1],
                                    pdu[startPos + 2],
                                    pdu[startPos + 3],
                                    pdu[startPos + 4],
                                    pdu[startPos + 5]));
                            }
                            catch { }
                            continue;

                        case 5:
                            try
                            {
                                result.Text = Base64ToGbk(pdu, startPos, endPos - startPos);
                            }
                            catch { }
                            continue;
                        case 6:
                            try
                            {
                                result.City = Base64ToGbk(pdu, startPos, endPos - startPos);
                            }
                            catch { }
                            continue;
                        case 7:
                            try
                            {
                                result.Street = Base64ToGbk(pdu, startPos, endPos - startPos);
                            }
                            catch { }

                            startPos = endPos + 1;
                            try
                            {
                                result.WarningType = pdu[startPos];
                            }
                            catch { }
                            break;
                    }
                }
                #endregion

                return true;
            }
            catch { }

            return false;
        }

        public static bool T02(byte[] pdu)
        {
            if (!Validate(pdu, "T02")) return false;

            return true;
        }

        public static bool L02(byte[] pdu, out int result)
        {
            result = 0;

            if (!Validate(pdu, "L02")) return false;

            try
            {
                result = (((((pdu[15] * 0x100) + pdu[16]) * 0x100) + pdu[17]) * 0x100) + pdu[18];
                return true;
            }
            catch { }
            return false;
        }

        public static bool L00(byte[] pdu, out string salt)
        {
            salt = null;

            if (!Validate(pdu, "L00")) return false;

            try
            {
                salt = ASCIIEncoding.ASCII.GetString(pdu, 15, 10);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 检查数据是否符合交通局通讯协议中的数据包规定。
        /// </summary>
        /// <param name="pdu">协议包</param>
        /// <param name="functionCode">功能关键字，如"T02"</param>
        /// <returns></returns>
        private static bool Validate(byte[] pdu, string functionCode)
        {
            if (pdu == null) return false;

            string pduHeader = ASCIIEncoding.ASCII.GetString(pdu, 0, 4);
            if (!pduHeader.StartsWith("~")) return false;

            if (pduHeader.Substring(1, 3) != functionCode) return false;

            return true;
        }

        private static string Base64ToGbk(byte[] buffer, int offset, int size)
        {
            try
            {
                byte[] data = Convert.FromBase64String(ASCIIEncoding.ASCII.GetString(buffer, offset, size));
                return Encoding.Default.GetString(data);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("无法将BASE64格式的指定数据转换为GBK字符串。", ex);
            }
        }

        /// <summary>
        /// 从交通局消息中取出“数据长度”。
        /// </summary>
        /// <param name="pdu"></param>
        /// <returns></returns>
        private static int ParseBodyLength(byte[] pdu)
        {
            return (((((pdu[10] * 0x100) + pdu[11]) * 0x100) + pdu[12]) * 0x100) + pdu[13];
        }
    }
}
