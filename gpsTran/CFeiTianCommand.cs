using System;
using System.Collections.Generic;
namespace gpsTran
{
	internal class CFeiTianCommand
	{
		public static byte[] MakeLogin(string strUserNo, string strMd5Pass, DateTime dt)
		{
			byte[] result;
			try
			{
				string text = "1,{0},{1},{2}";
				text = string.Format(text, strUserNo, dt.ToString("yyyyMMddHHmmss"), CFeiTianPacket.md5(strMd5Pass));
				byte[] array = CFeiTianPacket.CombinPacket(text);
				result = array;
			}
			catch (Exception ex)
			{
				CFeiTianCommand.dbgPrint(ex);
				result = null;
			}
			return result;
		}
		public static byte[] MakePosition(string strDeviceNo, DateTime dt, bool blnLocate, double Longitude, double Latitude, int Velocity, int Angle, string strAlarmInfo, out string cmdText)
		{
			byte[] result;
			try
			{
				string text = "3,{0},{1},{2},{3},{4},{5},{6},{7}";
				text = string.Format(text, new object[]
				{
					strDeviceNo,
					dt.ToString("yyyyMMddHHmmss"),
					blnLocate ? "1" : "0",
					Longitude.ToString("0.000000"),
					Latitude.ToString("0.000000"),
					Velocity.ToString(),
					Angle.ToString(),
					strAlarmInfo
				});
				cmdText = text;
				byte[] array = CFeiTianPacket.CombinPacket(text);
				result = array;
			}
			catch (Exception ex)
			{
				cmdText = "";
				CFeiTianCommand.dbgPrint(ex);
				result = null;
			}
			return result;
		}
		public static byte[] MakeVehicleInfo(List<FeiTanVehInfo> lstVehInfo)
		{
			byte[] result;
			try
			{
				string text = "6,";
				string[] array = new string[lstVehInfo.Count];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = lstVehInfo[i].ToString();
				}
				text += string.Join(",", array);
				byte[] array2 = CFeiTianPacket.CombinPacket(text);
				result = array2;
			}
			catch (Exception ex)
			{
				CFeiTianCommand.dbgPrint(ex);
				result = null;
			}
			return result;
		}
		private static void dbgPrint(Exception ex)
		{
		}
	}
}
