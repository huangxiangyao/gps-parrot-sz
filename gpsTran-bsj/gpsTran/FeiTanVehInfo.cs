using System;
using System.Text;
using System.Web;
namespace gpsTran
{
	public struct FeiTanVehInfo
	{
		public string DeviceNo;
		public string VehName;
		public string VehTypeNo;
		public string VehPhoneNo;
		public string SuportCamare;
		public string VehOwnerName;
		public string VehOwnerTel;
		public string VehOwnerSex;
		public string VehOwnerID;
		public string VehOwnerEmail;
		public string VehOwnerWorkUnits;
		public string VehOwenrAddress;
		public string VehOwenrPostCode;
		public string VehOwenrContactName1;
		public string VehOwenrContactTel1;
		public string VehOwenrContactName2;
		public string VehOwenrContactTel2;
		public string ServerStartTime;
		public string ServerEndTime;
		public string VehBurden;
		public string License;
		public string PathName;
		public string ParamInfo;
		public string DB44Info;
		public FeiTanVehInfo(bool blnTaaaa)
		{
			this.DeviceNo = " ";
			this.VehName = " ";
			this.VehTypeNo = " ";
			this.VehPhoneNo = " ";
			this.SuportCamare = " ";
			this.VehOwnerName = " ";
			this.VehOwnerTel = " ";
			this.VehOwnerSex = " ";
			this.VehOwnerID = " ";
			this.VehOwnerEmail = " ";
			this.VehOwnerWorkUnits = " ";
			this.VehOwenrAddress = " ";
			this.VehOwenrPostCode = " ";
			this.VehOwenrContactName1 = " ";
			this.VehOwenrContactTel1 = " ";
			this.VehOwenrContactName2 = " ";
			this.VehOwenrContactTel2 = " ";
			this.ServerStartTime = " ";
			this.ServerEndTime = " ";
			this.VehBurden = "0";
			this.License = " ";
			this.PathName = " ";
			this.ParamInfo = " ";
			this.DB44Info = " ";
		}
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.DeviceNo)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehName)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.GetCarType(this.VehTypeNo))) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehPhoneNo)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.SuportCamare)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwnerName)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwnerTel)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwnerSex)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwnerID)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwnerEmail)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwnerWorkUnits)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwenrAddress)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwenrPostCode)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwenrContactName1)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwenrContactTel1)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwenrContactName2)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehOwenrContactTel2)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.ServerStartTime)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.ServerEndTime)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.VehBurden)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.License)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.PathName)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.ParamInfo)) + "$");
			stringBuilder.Append(HttpUtility.UrlEncode(Encoding.GetEncoding("gb2312").GetBytes(this.DB44Info)));
			return stringBuilder.ToString();
		}
		public string GetCarType(string enter)
		{
			string result = "99";
			switch (enter)
			{
			case "省际客运车辆":
				result = "11";
				break;
			case "市际客运车辆":
				result = "12";
				break;
			case "旅游客运车辆":
				result = "13";
				break;
			case "县际客运车辆":
				result = "14";
				break;
			case "危险货物运输车辆":
				result = "20";
				break;
			case "重型货车":
				result = "31";
				break;
			case "牵引列车":
				result = "32";
				break;
			case "重型自卸车":
				result = "33";
				break;
			case "普通货车":
				result = "34";
				break;
			case "出租汽车":
				result = "41";
				break;
			case "教练车":
				result = "42";
				break;
			case "公交车":
				result = "43";
				break;
			case "其他":
				result = "99";
				break;
			}
			return result;
		}
	}
}
