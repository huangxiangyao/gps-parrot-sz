using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot.Models.Db44;

namespace Parrot
{
    /// <summary>
    /// 警告信息
    /// </summary>
    public class JtjD01
    {
        /// <summary>
        /// 警告ID
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
        /// 警告时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 警告内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 警告发生地（城市名）
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 警告发生道路（城市道路名）
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 警告类型
        /// </summary>
        public byte WarningType { get; set; }

        public override string ToString()
        {
            return string.Format("警告ID：{0}，车牌号：{1}，车牌颜色：{2}，警告时间：{3:yyyy-MM-dd HH:mm:ss}，警告内容：{4}，警告发生地（城市名）：{5}，警告发生道路（城市道路名）：{6}，警告类型：{7}。",
                Id, PlateNumber,
                VehiclePlateColorHelper.Default.Items[(byte)(PlateColor)],
                CreateDate, Text, City, Street, WarningType);
        }
    }
}
