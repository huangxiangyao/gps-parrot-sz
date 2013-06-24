using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Parrot
{
    /// <summary>
    /// Provides data for the DriverSignInOrOut event.
    /// </summary>
    public class DriverSignedInOrOutEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverSignedInOrOutEventArgs"/> class.
        /// </summary>
        /// <param name="plateNumber">车牌号。</param>
        /// <param name="plateColor">车牌颜色。</param>
        /// <param name="cameraNumber">登入(true)/登出(false)。</param>
        /// <param name="captureTime">驾驶证号码。</param>
        /// <param name="imageData">驾驶员姓名。</param>
        public DriverSignedInOrOutEventArgs(string plateNumber, byte plateColor, bool signedIn, string driverLicenseNumber, string driverName)
        {
            this.PlateNumber = plateNumber;
            this.PlateColor = PlateColor;
            this.SignedIn = signedIn;
            this.DriverLicenseNumber = driverLicenseNumber;
            this.DriverName = driverName;
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
        /// 登入(true)/登出(false)。
        /// </summary>
        public bool SignedIn { get; private set; }
        /// <summary>
        /// 驾驶证号码。
        /// </summary>
        public string DriverLicenseNumber { get; private set; }
        /// <summary>
        /// 驾驶员姓名。
        /// </summary>
        public string DriverName { get; private set; }
    }
}
