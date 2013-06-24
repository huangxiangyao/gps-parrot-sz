using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot.Models.Db44;

namespace Parrot
{
    /// <summary>
    /// 违法信息
    /// </summary>
    public class JtjD02
    {
        /// <summary>
        /// 违法信息ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string PlateNumber { get; set; }
        /// <summary>
        /// 车牌颜色
        /// </summary>
        public byte PlateColor { get; set; }
        /// <summary>
        /// 违法时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 违法内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 违法发生地（城市名）
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 违法发生道路（城市道路名）
        /// </summary>
        public string Street { get; set; }

        public override string ToString()
        {
            return string.Format("违法信息ID：{0}，车牌号：{1}，车牌颜色：{2}，违法时间：{3:yyyy-MM-dd HH:mm:ss}，违法内容：{4}，违法发生地（城市名）：{5}，违法发生道路（城市道路名）：{6}。",
                Id, PlateNumber,
                VehiclePlateColorHelper.Default.Items[(byte)(PlateColor)],
                CreateDate, Text, City, Street);
        }
    }
}
