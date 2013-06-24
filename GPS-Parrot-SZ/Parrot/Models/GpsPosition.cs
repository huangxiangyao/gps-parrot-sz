using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Db44
{
    /// <summary>
    /// GPS经纬度。
    /// </summary>
    public class GpsPosition
    {
        /// <summary>
        /// 经度（DDDFF.FFF）。
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度（0DDFF.FFF）。
        /// </summary>
        public double Latitude { get; set; }
    }
}
