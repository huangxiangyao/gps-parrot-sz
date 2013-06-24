using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot;

namespace Parrot.Models.Db44
{
    public class Db44Parser
    {
        #region 必选协议（00-06）
        /// <summary>
        /// 自动上传之卫星定位数据包。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsDataList">N个基本卫星定位数据包(<see cref="Db44GpsData"/>)。</param>
        /// <returns></returns>
        public static bool U00(Db44Packet packet, out List<Db44GpsData> gpsDataList)
        {
            gpsDataList = null;

            if (packet.FunctionCode != "U00") return false;

            try
            {
                int len = packet.Body.Length;

                if (len % Db44GpsData.FixDataLength != 0) return false;

                gpsDataList = new List<Db44GpsData>();
                for (int i = 0; i < len; i += Db44GpsData.FixDataLength)
                {
                    Db44GpsData gpsData = new Db44GpsData(packet.Body, i);
                    gpsDataList.Add( gpsData);
                }
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 当前位置的返回数据。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整卫星定位数据包(<see cref="Db44GpsData"/>)。</param>
        /// <returns></returns>
        public static bool U01(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U01") return false;

            try {
                gpsData = new Db44GpsData(packet.Body);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 定时监控设置的响应数据。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <param name="timeInterval">时间间隔。</param>
        /// <param name="times">发送次数。</param>
        /// <param name="numberOfPackets">每次发送的数据包数。</param>
        /// <returns></returns>
        public static bool U02(Db44Packet packet, out Db44GpsData gpsData, out byte timeInterval, out byte times, out ushort numberOfPackets)
        {
            gpsData = null;
            timeInterval = 0;
            times = 0;
            numberOfPackets = 0;

            if (packet.FunctionCode != "U02") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body,pos);
                pos += Db44GpsData.FixDataLength;

                timeInterval = packet.Body[pos];
                pos += 1;

                times = packet.Body[pos];
                pos += 1;

                numberOfPackets = (ushort)((packet.Body[pos] << 8) | (packet.Body[pos + 1]));
                return true;
            }
            catch { }
            return false;
        }


        /// <summary>
        /// 查看定时监控参数的响应数据。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <param name="timeInterval">时间间隔。</param>
        /// <param name="times">发送次数。</param>
        /// <param name="numberOfPackets">每次发送的数据包数。</param>
        /// <returns></returns>
        public static bool U03(Db44Packet packet, out Db44GpsData gpsData, out byte timeInterval, out byte times, out ushort numberOfPackets)
        {
            gpsData = null;
            timeInterval = 0;
            times = 0;
            numberOfPackets = 0;

            if (packet.FunctionCode != "U03") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body, pos);
                pos += Db44GpsData.FixDataLength;

                timeInterval = packet.Body[pos];
                pos += 1;

                times = packet.Body[pos];
                pos += 1;

                numberOfPackets = (ushort)((packet.Body[pos] << 8) | (packet.Body[pos + 1]));
                return true;
            }
            catch { }
            return false;
        }
        
        /// <summary>
        /// 定距监控设置的响应数据。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <param name="timeInterval">间距。</param>
        /// <param name="times">发送次数。</param>
        /// <param name="numberOfPackets">每次发送的数据包数。</param>
        /// <returns></returns>
        public static bool U04(Db44Packet packet, out Db44GpsData gpsData, out ushort distanceInterval, out byte times, out ushort numberOfPackets)
        {
            gpsData = null;
            distanceInterval = 0;
            times = 0;
            numberOfPackets = 0;

            if (packet.FunctionCode != "U04") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body, pos);
                pos += Db44GpsData.FixDataLength;

                distanceInterval = (ushort)((packet.Body[pos] << 8) | (packet.Body[pos + 1]));
                pos += 2;

                times = packet.Body[pos];
                pos += 1;

                numberOfPackets = (ushort)((packet.Body[pos] << 8) | (packet.Body[pos + 1]));
                return true;
            }
            catch { }
            return false;
        }


        /// <summary>
        /// 查看定距监控参数的响应数据。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <param name="timeInterval">间距。</param>
        /// <param name="times">发送次数。</param>
        /// <param name="numberOfPackets">每次发送的数据包数。</param>
        /// <returns></returns>
        public static bool U05(Db44Packet packet, out Db44GpsData gpsData, out ushort distanceInterval, out byte times, out ushort numberOfPackets)
        {
            gpsData = null;
            distanceInterval = 0;
            times = 0;
            numberOfPackets = 0;

            if (packet.FunctionCode != "U05") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body, pos);
                pos += Db44GpsData.FixDataLength;

                distanceInterval = (ushort)((packet.Body[pos] << 8) | (packet.Body[pos + 1]));
                pos += 2;

                times = packet.Body[pos];
                pos += 1;

                numberOfPackets = (ushort)((packet.Body[pos] << 8) | (packet.Body[pos + 1]));
                return true;
            }
            catch { }
            return false;
        }


        /// <summary>
        /// 设置速度监控的响应数据。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整卫星定位数据包(<see cref="Db44GpsData"/>)。</param>
        /// <returns></returns>
        public static bool U06(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U06") return false;

            try
            {
                gpsData = new Db44GpsData(packet.Body);
                return true;
            }
            catch { }
            return false;
        }
        #endregion
        #region 可选协议（07-0e），仅部分支持。
        public static bool U07(Db44Packet packet, out List<byte> areaNumberList)
        {
            areaNumberList = null;

            if (packet.FunctionCode != "U07") return false;

            try
            {
                byte amount = packet.Body[0];
                if (packet.Body.Length != (amount + 1)) return false;

                areaNumberList = new List<byte>();
                for (byte i = 0; i < amount; i++)
                {
                    areaNumberList.Add(packet.Body[i + 1]);
                }
            }
            catch { }
            return false;
        }
        public static bool U07(Db44Packet packet, out List<GpsPosition> gpsPositionList)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 必选协议（0F-1D）
        /// <summary>
        /// 返回完整卫星定位数据包。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <returns></returns>
        public static bool U0f(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U0f") return false;

            try
            {
                gpsData = new Db44GpsData(packet.Body);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 返回事故疑点数据。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <returns></returns>
        public static bool U10(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U10") return false;

            try
            {
                int pos = 0;
                pos += 1;

                gpsData = new Db44GpsData(packet.Body,pos);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 历史轨迹。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsDataList">N个基本卫星定位数据包(<see cref="Db44GpsData"/>)。</param>
        /// <returns></returns>
        public static bool U11(Db44Packet packet, out List<Db44GpsData> gpsDataList)
        {
            gpsDataList = null;

            if (packet.FunctionCode != "U11") return false;

            try
            {
                int len = packet.Body.Length;

                if (len % Db44GpsData.FixDataLength != 0) return false;

                gpsDataList = new List<Db44GpsData>();
                for (int i = 0; i < len; i += Db44GpsData.FixDataLength)
                {
                    Db44GpsData gpsData = new Db44GpsData(packet.Body, i);
                    gpsDataList.Add(gpsData);
                }
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 上行驾驶员身份。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="driverNumber">驾驶员身份信息（ID）。</param>
        /// <param name="drivingDuration">驾驶时间。单位：分钟。</param>
        /// <returns></returns>
        public static bool U12(Db44Packet packet, out uint driverNumber, out ushort drivingDuration)
        {
            driverNumber = 0;
            drivingDuration = 0;
            if (packet.FunctionCode != "U12") return false;

            try
            {
                int pos = 0;
                driverNumber = uint.Parse(string.Format("{0:X2}{1:X2}{2:X2}{3:X2}",
                    packet.Body[pos],packet.Body[pos + 1],packet.Body[pos + 2],packet.Body[pos + 3]));
                pos += 4;

                drivingDuration = ushort.Parse(string.Format("{0:X2}{1:X2}",
                    packet.Body[pos], packet.Body[pos + 1]));

                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 打印前上报。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <returns></returns>
        public static bool U13(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U13") return false;

            try
            {
                gpsData = new Db44GpsData(packet.Body);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 上行连续驾驶时间。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="drivingDuration">连续驾驶时间。</param>
        /// <returns></returns>
        public static bool U14(Db44Packet packet, out ushort drivingDuration)
        {
            drivingDuration = 0;
            if (packet.FunctionCode != "U14") return false;

            try
            {
                int pos = 0;
                drivingDuration = ushort.Parse(string.Format("{0:X2}{1:X2}",
                    packet.Body[pos], packet.Body[pos + 1]));

                return true;
            }
            catch { }
            return false;
        }


        /// <summary>
        /// 下发文本信息的返回数据。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <param name="text">文本信息。</param>
        /// <returns></returns>
        public static bool U15(Db44Packet packet, out Db44GpsData gpsData, out string text)
        {
            gpsData = null;
            text = null;
            if (packet.FunctionCode != "U15") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body, pos);
                pos += Db44GpsData.FixDataLength;

                int textLength = packet.Body.Length-pos;
                text = Encoding.Default.GetString(packet.Body, pos, textLength);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 上行的文本信息。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <param name="text">文本信息。</param>
        /// <returns></returns>
        public static bool U16(Db44Packet packet, out Db44GpsData gpsData, out string text)
        {
            gpsData = null;
            text = null;
            if (packet.FunctionCode != "U16") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body, pos);
                pos += Db44GpsData.FixDataLength;

                int textLength = packet.Body.Length - pos;
                text = Encoding.Default.GetString(packet.Body, pos, textLength);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 上行的紧急报警。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <returns></returns>
        public static bool U17(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U17") return false;

            try
            {
                gpsData = new Db44GpsData(packet.Body);
                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 解除紧急报警的返回数据。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <returns></returns>
        public static bool U18(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U18") return false;

            try
            {
                gpsData = new Db44GpsData(packet.Body);
                return true;
            }
            catch { }
            return false;
        }


        /// <summary>
        /// 远程断油（或恢复）控制的返回数据。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <param name="result">执行结果。</param>
        /// <returns></returns>
        public static bool U19(Db44Packet packet, out Db44GpsData gpsData, out byte result)
        {
            gpsData = null;
            result = 0;
            if (packet.FunctionCode != "U19") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body);
                pos += Db44GpsData.FixDataLength;

                result = packet.Body[pos];

                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 查看远程参数的返回。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <returns></returns>
        public static bool U1a(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U1a") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body);
                pos += Db44GpsData.FixDataLength;
                //...

                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 设置远程参数的返回。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <returns></returns>
        public static bool U1b(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U1b") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body);
                pos += Db44GpsData.FixDataLength;
                //...

                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 复位指令的返回。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <returns></returns>
        public static bool U1c(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U1c") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body);
                pos += Db44GpsData.FixDataLength;
                //...

                return true;
            }
            catch { }
            return false;
        }

        /// <summary>
        /// 查询密钥的返回。
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="gpsData">1个完整的卫星定位数据包。</param>
        /// <returns></returns>
        public static bool U1d(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "U1d") return false;

            try
            {
                int pos = 0;
                gpsData = new Db44GpsData(packet.Body);
                pos += Db44GpsData.FixDataLength;
                //...
                return true;
            }
            catch { }
            return false;
        }
        #endregion
        #region 自定义协议
        public static bool Uf8(Db44Packet packet, out Db44GpsData gpsData)
        {
            gpsData = null;
            if (packet.FunctionCode != "Uf8") return false;

            try
            {
                int pos = 0;
                pos += 3;
                //...
                gpsData = new Db44GpsData(packet.Body,pos);
                return true;
            }
            catch { }
            return false;
        }
        #endregion
    }
}
