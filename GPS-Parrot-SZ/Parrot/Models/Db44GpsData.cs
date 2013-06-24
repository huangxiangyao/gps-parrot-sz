using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Globalization;

namespace Parrot
{
    /// <summary>
    /// 基本卫星定位数据包。
    /// </summary>
    /// <remarks>本结构体长度固定为<see cref="FixDataLength"/>。</remarks>
    /// <seealso cref="Db44VehicleState"/>
    public class Db44GpsData
    {
        /// <summary>
        /// 数据体长度。
        /// </summary>
        public const int FixDataLength = 30;
        /// <summary>
        /// 数据体。
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public Db44GpsData()
        {
            Data = new byte[FixDataLength];
        }

        /// <summary>
        /// 拷贝构造函数。
        /// </summary>
        /// <param name="data"></param>
        public Db44GpsData(byte[] data) : this(data, 0) { }

        /// <summary>
        /// 拷贝构造函数。
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset">偏移位置。</param>
        public Db44GpsData(byte[] data, int offset)
        {
            if (data == null) throw new ArgumentNullException();
            if (data.Length < (offset + FixDataLength)) throw new ArgumentException(string.Format("指定偏移位置之后的数据不足{0}字节。", FixDataLength));

            this.Data = new byte[FixDataLength];
            Array.Copy(data, offset, this.Data, 0, FixDataLength);
        }

        /// <summary>
        /// 时间。BCD码，yyMMddHHmmss
        /// </summary>
        public DateTime GpsTime
        {
            get
            {
                return DateTime.Parse(
                    string.Format("20{0:X2}-{1:X2}-{2:X2} {3:X2}:{4:X2}:{5:X2}",
                    Data[0], Data[1], Data[2], Data[3], Data[4], Data[5]));
            }
            set
            {
                Data[0] = byte.Parse((value.Year - 2000).ToString(), NumberStyles.HexNumber);
                Data[1] = byte.Parse((value.Month).ToString(), NumberStyles.HexNumber);
                Data[2] = byte.Parse((value.Day).ToString(), NumberStyles.HexNumber);
                Data[3] = byte.Parse((value.Hour).ToString(), NumberStyles.HexNumber);
                Data[4] = byte.Parse((value.Minute).ToString(), NumberStyles.HexNumber);
                Data[5] = byte.Parse((value.Second).ToString(), NumberStyles.HexNumber);
            }
        }
        /// <summary>
        /// 经度。BCD码，DDDFF.FFF
        /// </summary>
        public double Longitude
        {
            get
            {
                int startPos = 6;
                return double.Parse(
                    string.Format("{0:X2}{1:X2}{2:X1}.{3:X1}{4:X2}",
                    Data[startPos + 0], Data[startPos + 1],
                    (byte)((Data[startPos + 2] & 0xf0) >> 4),
                    (byte)((Data[startPos + 2] & 0x0f)),
                    Data[startPos + 3]));
            }
            set
            {
                int startPos = 6;
                string s = value.ToString("00000.000");
                s = s.Replace(".", "");
                Data[startPos + 0] = byte.Parse(s.Substring(0, 2), NumberStyles.HexNumber);
                Data[startPos + 1] = byte.Parse(s.Substring(2, 2), NumberStyles.HexNumber);
                Data[startPos + 2] = byte.Parse(s.Substring(4, 2), NumberStyles.HexNumber);
                Data[startPos + 3] = byte.Parse(s.Substring(6, 2), NumberStyles.HexNumber);
            }
        }
        /// <summary>
        /// 纬度。BCD码，0DDFF.FFF
        /// </summary>
        public double Latitude
        {
            get
            {
                int startPos = 10;
                return double.Parse(
                    string.Format("{0:X2}{1:X2}{2:X1}.{3:X1}{4:X2}",
                    Data[startPos + 0], Data[startPos + 1],
                    (byte)((Data[startPos + 2] & 0xf0) >> 4),
                    (byte)((Data[startPos + 2] & 0x0f)),
                    Data[startPos + 3]));
            }
            set
            {
                int startPos = 10;
                string s = value.ToString("00000.000");
                s = s.Replace(".", "");
                Data[startPos + 0] = byte.Parse(s.Substring(0, 2), NumberStyles.HexNumber);
                Data[startPos + 1] = byte.Parse(s.Substring(2, 2), NumberStyles.HexNumber);
                Data[startPos + 2] = byte.Parse(s.Substring(4, 2), NumberStyles.HexNumber);
                Data[startPos + 3] = byte.Parse(s.Substring(6, 2), NumberStyles.HexNumber);

            }
        }
        /// <summary>
        /// 速度。单位：km/h
        /// </summary>
        public byte Speed
        {
            get
            {
                int startPos = 14;
                return Data[startPos];
            }
            set
            {
                int startPos = 14;
                Data[startPos] = value;
            }
        }
        /// <summary>
        /// 方向。单位：dec。
        /// </summary>
        public short Direction
        {
            get
            {
                //单位：2 dec
                int startPos = 15;
                return (short)(Data[startPos] * 2); 
            }
            set
            {
                int startPos = 15;
                Data[startPos] = (byte)(value / 2);
            }
        }
        /// <summary>
        /// 高度。单位：m
        /// </summary>
        public ushort Altitude
        {
            get
            {
                int startPos = 16;
                return (ushort)((Data[startPos] << 8) | Data[startPos + 1]);
            }
            set
            {
                int startPos = 16;
                Data[startPos] = (byte)((value & 0xff00) >> 8);
                Data[startPos + 1] = (byte)((value & 0xff));
            }
        }
        /// <summary>
        /// 里程表读数。单位：km
        /// </summary>
        public double Odometer
        {
            get
            {
                //单位：0.1km，BCD码，9999999.9
                int startPos = 18;
                return double.Parse(
                    string.Format("{0:X2}{1:X2}{2:X2}{3:X1}.{4:X1}",
                    Data[startPos + 0], Data[startPos + 1], Data[startPos + 2],
                    (byte)((Data[startPos + 3] & 0xf0) >> 4),
                    (byte)((Data[startPos + 3] & 0x0f))));
            }
            set
            {
                int startPos = 18;
                string s = value.ToString("0000000.0");
                s = s.Replace(".", "");
                Data[startPos + 0] = byte.Parse(s.Substring(0, 2), NumberStyles.HexNumber);
                Data[startPos + 1] = byte.Parse(s.Substring(2, 2), NumberStyles.HexNumber);
                Data[startPos + 2] = byte.Parse(s.Substring(4, 2), NumberStyles.HexNumber);
                Data[startPos + 3] = byte.Parse(s.Substring(6, 2), NumberStyles.HexNumber);

            }
        }

        public Db44VehicleState GetState()
        {
            int startPos = 22;
            byte[] t = new byte[8];
            Array.Copy(this.Data, startPos, t, 0, 8);
            return new Db44VehicleState(t);
        }
        public void SetState(Db44VehicleState value)
        {
            int startPos = 22;
            value.Data.CopyTo(this.Data, startPos);
        }

        public string ToReadableString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("时间：{0:yyyy-MM-dd HH:mm:ss}" + Environment.NewLine, this.GpsTime);
            sb.AppendFormat("经度：{0}" + Environment.NewLine, this.Longitude);
            sb.AppendFormat("纬度：{0}" + Environment.NewLine, this.Latitude);
            sb.AppendFormat("速度：{0} km/h" + Environment.NewLine, this.Speed);
            sb.AppendFormat("方向：{0} dec" + Environment.NewLine, this.Direction);
            sb.AppendFormat("高度：{0} m" + Environment.NewLine, this.Altitude);
            sb.AppendFormat("里程：{0} km" + Environment.NewLine, this.Odometer);
            sb.AppendFormat("状态：{0}", GetState().ToReadableStatusText());
            return sb.ToString();
        }
    }
}
