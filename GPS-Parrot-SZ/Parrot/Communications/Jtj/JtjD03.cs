using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Parrot.Models.Db44;

namespace Parrot
{
    /// <summary>
    /// 提示信息
    /// </summary>
    public class JtjD03
    {
        /// <summary>
        /// 提示信息ID
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
        /// 发送时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 提示内容
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 发送者
        /// </summary>
        public string Sender { get; set; }

        public override string ToString()
        {
            return string.Format("提示信息ID：{0}，车牌号：{1}，车牌颜色：{2}，发送时间：{3:yyyy-MM-dd HH:mm:ss}，提示内容：{4}，发送者：{5}。",
                Id, PlateNumber,
                VehiclePlateColorHelper.Default.Items[(byte)(PlateColor)],
                CreateDate, Text, Sender);
        }
    }
}
