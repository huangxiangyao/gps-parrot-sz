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
    public class VehiclePurposeHelper
    {
        private static VehiclePurposeHelper defaultInstance = new VehiclePurposeHelper();

        public static VehiclePurposeHelper Default { get { return defaultInstance; } }

        private Dictionary<byte, string> items = null;

        public VehiclePurposeHelper()
        {
            items = new Dictionary<byte, string>();
            items.Add(1, "省际班车");
            items.Add(2, "市际班车");
            items.Add(3, "县际班车");
            items.Add(4, "县内班车");
            items.Add(5, "省际包车");
            items.Add(6, "市际包车");
            items.Add(7, "县际包车");
            items.Add(8, "县内包车");
            items.Add(9, "危险货物运输");
            items.Add(10, "出租的士");
            items.Add(11, "公共的士");
            items.Add(12, "普通货运");
            items.Add(13, "测试车辆");
            items.Add(14, "执法车");
            items.Add(15, "驾驶员教练车");
            items.Add(16, "校车");
            items.Add(17, "散装物料车");
        }

        public Dictionary<byte, string> Items { get { return items; } }
    }
}
