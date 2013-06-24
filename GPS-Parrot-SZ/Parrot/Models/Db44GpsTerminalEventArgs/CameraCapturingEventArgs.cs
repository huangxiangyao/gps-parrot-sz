using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Parrot
{
    /// <summary>
    /// Provides data for the CameraCapturing event.
    /// </summary>
    public class CameraCapturingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverSignedInOrOutEventArgs"/> class.
        /// </summary>
        /// <param name="plateNumber">车牌号。</param>
        /// <param name="plateColor">车牌颜色。</param>
        /// <param name="cameraNumber">数据包编号。</param>
        /// <param name="captureTime">发生时间。</param>
        /// <param name="imageData">疑点数据。数据长度为200个字节。</param>
        public CameraCapturingEventArgs(string plateNumber, byte plateColor, byte cameraNumber, DateTime captureTime, string imageFormatName, int packetTotal, byte packetIndex, byte[] imageData)
        {
            this.PlateNumber = plateNumber;
            this.PlateColor = PlateColor;
            this.CameraNumber = cameraNumber;
            this.CaptureTime = captureTime;
            this.ImageFormatName = imageFormatName;
            this.PacketTotal = packetTotal;
            this.PacketIndex = packetIndex;
            this.ImageData = imageData;
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
        /// 摄像头编号。
        /// </summary>
        public byte CameraNumber { get; private set; }
        /// <summary>
        /// 拍摄时间。
        /// </summary>
        public DateTime CaptureTime { get; private set; }
        /// <summary>
        /// 图片格式。
        /// </summary>
        /// <remarks>如"jpg", "gif", "tiff"。</remarks>
        public string ImageFormatName { get; private set; }
        /// <summary>
        /// 本图片数据包总数。
        /// </summary>
        public int PacketTotal { get; private set; }
        /// <summary>
        /// 数据包序号。
        /// </summary>
        /// <remarks>从1开始。当<see cref="PacketIndex"/>与<see cref="PacketTotal"/>相等时表示最后一包。图片数据包必须按顺序传输。</remarks>
        public byte PacketIndex { get; private set; }
        /// <summary>
        /// 图片数据。
        /// </summary>
        /// <remarks>不定长，每包图片数据不超过1024字节。</remarks>
        public byte[] ImageData { get; private set; }
    }
}
