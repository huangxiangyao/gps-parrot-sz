using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Parrot
{
    /// <summary>
    /// Provides data for the GpsDataReceived event.
    /// </summary>
    public class GpsDataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverSignedInOrOutEventArgs"/> class.
        /// </summary>
        /// <param name="plateNumber">车牌号。</param>
        /// <param name="plateColor">车牌颜色。</param>
        /// <param name="cameraNumber">登入(true)/登出(false)。</param>
        /// <param name="captureTime">驾驶证号码。</param>
        /// <param name="imageData">驾驶员姓名。</param>
        public GpsDataReceivedEventArgs(string plateNumber, byte plateColor, Db44GpsData gpsData)
        {
            this.PlateNumber = plateNumber;
            this.PlateColor = PlateColor;
            this.GpsData = gpsData;
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
        /// 卫星定位数据。
        /// </summary>
        public Db44GpsData GpsData { get; private set; }
    }
}
