using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot
{
    /// <summary>
    /// 车辆状态。
    /// </summary>
    /// <remarks>本结构体长度固定为<see cref="DataLength"/>。</remarks>
    public class Db44VehicleState
    {
        /// <summary>
        /// 数据体长度。
        /// </summary>
        public const int DataLength = 8;
        /// <summary>
        /// 数据体。
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public Db44VehicleState()
        {
            Data = new byte[DataLength];
        }
        /// <summary>
        /// 拷贝构造函数。
        /// </summary>
        /// <param name="data"></param>
        public Db44VehicleState(byte[] data)
        {
            if (data == null) throw new ArgumentNullException();

            this.Data = new byte[DataLength];
            int len = data.Length;
            if (len > DataLength) len = DataLength;
            Array.Clear(this.Data, 0, DataLength);
            Array.Copy(data, this.Data, data.Length);
        }
        
        /// <summary>
        /// 设置车辆状态字的值。
        /// </summary>
        /// <param name="byteIndex">第n状态字。n: 0~7</param>
        /// <param name="bitIndex">第n位。n: 0~7</param>
        /// <param name="value">true(1)置位 or false(0)复位</param>
        public void SetValue(int byteIndex, int bitIndex, bool value)
        {
            if (value)
            {
                Data[byteIndex] |= (byte)(1 << bitIndex);
            }
            else
            {
                Data[byteIndex] &= ((byte)~(1 << bitIndex));
            }
        }

        /// <summary>
        /// 获取车辆指定状态位的值。
        /// </summary>
        /// <param name="byteIndex">第n状态字。n: 0~7</param>
        /// <param name="bitIndex">第n位。n: 0~7</param>
        /// <returns>true(1)置位 or false(0)复位</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public bool GetValue(int byteIndex, int bitIndex)
        {
            return ((Data[byteIndex] & (1 << bitIndex)) != 0);
        }

        public bool Acc
        {
            get { return GetValue(1, 5);}
        }

        public string ToReadableStatusText()
        {
            StringBuilder sb = new StringBuilder();

            int byteIndex = 0;
            int bitIndex = 0;
            if (!GetValue(byteIndex, bitIndex++))
                sb.Append("西经，");
            if (!GetValue(byteIndex, bitIndex++))
                sb.Append("南纬，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("紧急报警，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("断油，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("超速报警，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("振动报警，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("主电源断电，");

            byteIndex++;
            bitIndex = 0;
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("刹车制动，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("开门，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("左转向灯 ON，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("右转向灯 ON，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("远光灯 ON，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("ACC ON，");

            byteIndex++;
            bitIndex = 0;
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("GPS定位锁定，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("GPS天线短路，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("GPS天线开路，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("GPS模块异常，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("通信模块异常，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("出区域越界，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("入区域越界，");

            byteIndex++;
            bitIndex = 0;
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("备用电池异常，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("地理栅栏越界，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("发动机 ON，");
            else
                sb.Append("发动机 OFF，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("疲劳驾驶，");

            byteIndex++;
            bitIndex = 0;
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("CPU状态异常，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("内存状态异常，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("FLASH状态异常，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("显示屏状态异常，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("SD卡状态异常，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("打印机连接断开，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("摄像头连接断开，");

            byteIndex++;
            bitIndex = 0;
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("定时上传，");
            if (GetValue(byteIndex, bitIndex++))
                sb.Append("定距上传，");
            //...
            if (GetValue(byteIndex, 7))
                sb.Append("GPS测速，");
            else
                sb.Append("传感器测速，");

            sb.Append("其他状态正常");

            return sb.ToString();
        }
    }
}
