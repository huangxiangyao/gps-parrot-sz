using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Db44
{

    /// <summary>
    /// GPS封闭区域。
    /// </summary>
    public class GpsClosedArea
    {
        /// <summary>
        /// 区域编号。
        /// </summary>
        public byte Index { get; set; }
        /// <summary>
        /// 本区域点数（16～255）。
        /// </summary>
        public byte Amount { get { return (byte)GpsPositions.Count; } }
        /// <summary>
        /// 每点的经纬度。
        /// </summary>
        public readonly List<GpsPosition> GpsPositions = new List<GpsPosition>();
    }
}
