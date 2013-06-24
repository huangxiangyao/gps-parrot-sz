using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Db44
{
    /// <summary>
    /// 严格遵循DB44。
    /// 注意：在数据库中因为原程序员错，只能将就采用0-3。-1表示“不会用到”，null表示“未知”。
    /// </summary>
    public class VehiclePlateColorHelper
    {
        public enum VehiclePlateColor
        {
            Blue=1, //协议中规定，交通局实际下发的数据为1-4。
            yellow,
            White,
            Black
        }

        private static VehiclePlateColorHelper defaultInstance = new VehiclePlateColorHelper();

        public static VehiclePlateColorHelper Default { get { return defaultInstance; } }

        private Dictionary<byte, string> items = null;

        public VehiclePlateColorHelper()
        {
            items = new Dictionary<byte, string>();
            items.Add(1, "蓝");
            items.Add(2, "黄");
            items.Add(3, "白");
            items.Add(4, "黑");
        }

        public Dictionary<byte, string> Items { get { return items; } }
    }
}
