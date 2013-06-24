using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Parrot
{
    /// <summary>
    /// Provides data for the AccidentRelativeDataReporting event.
    /// </summary>
    public class PossibleAccidentDataReportingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverSignedInOrOutEventArgs"/> class.
        /// </summary>
        /// <param name="plateNumber">车牌号。</param>
        /// <param name="plateColor">车牌颜色。</param>
        /// <param name="cameraNumber">数据包编号。</param>
        /// <param name="captureTime">发生时间。</param>
        /// <param name="imageData">疑点数据。数据长度为200个字节。</param>
        public PossibleAccidentDataReportingEventArgs(string plateNumber, byte plateColor, byte packetIndex, DateTime stoppedTime, byte[] brakeData)
        {
            this.PlateNumber = plateNumber;
            this.PlateColor = PlateColor;
            this.PacketIndex = packetIndex;
            this.StoppedTime = stoppedTime;
            this.BrakeData = brakeData;
        }

        /// <summary>
        /// 车牌号。
        /// </summary>
        public string PlateNumber { get; private set; }
        /// <summary>
        /// 车牌颜色。
        /// </summary>
        public byte PlateColor { get; private set; }
        /// <summary>
        /// 疑点数据包序号。
        /// </summary>
        /// <remarks>有效值为：1~10。</remarks>
        public byte PacketIndex { get; private set; }
        /// <summary>
        /// 疑点停车时间。
        /// </summary>
        public DateTime StoppedTime { get; private set; }
        /// <summary>
        /// 停车时疑点数据。
        /// </summary>
        /// <remarks>100组速度、开关量，数据之间无分隔符。每个速度和开关量的长度均为1个字节。速度的单位：km/h；开关量最高位表示制动信号。</remarks>
        public byte[] BrakeData { get; private set; }
    }
}
