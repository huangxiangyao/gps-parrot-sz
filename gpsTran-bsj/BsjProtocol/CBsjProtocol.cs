using System;
using System.Globalization;
using System.IO;
using System.Text;
namespace BSJProtocol
{
	public class CBsjProtocol
	{
		public delegate void ShowImageEx(string strPath);
		private byte[] m_abtBuffer;
		public void Append(byte[] buff)
		{
			if (this.m_abtBuffer == null)
			{
				this.m_abtBuffer = new byte[buff.Length];
				Buffer.BlockCopy(buff, 0, this.m_abtBuffer, 0, this.m_abtBuffer.Length);
				return;
			}
			byte[] abtBuffer = this.m_abtBuffer;
			this.m_abtBuffer = new byte[abtBuffer.Length + buff.Length];
			Buffer.BlockCopy(abtBuffer, 0, this.m_abtBuffer, 0, abtBuffer.Length);
			Buffer.BlockCopy(buff, 0, this.m_abtBuffer, abtBuffer.Length, buff.Length);
		}
		public void Append(byte[] buff, int nSize)
		{
			if (this.m_abtBuffer == null)
			{
				this.m_abtBuffer = new byte[nSize];
				Buffer.BlockCopy(buff, 0, this.m_abtBuffer, 0, nSize);
				return;
			}
			byte[] abtBuffer = this.m_abtBuffer;
			this.m_abtBuffer = new byte[abtBuffer.Length + nSize];
			Buffer.BlockCopy(abtBuffer, 0, this.m_abtBuffer, 0, abtBuffer.Length);
			Buffer.BlockCopy(buff, 0, this.m_abtBuffer, abtBuffer.Length, nSize);
		}
		public void Clear()
		{
			this.m_abtBuffer = null;
		}
		public AnalysisResutl SplitPack(out byte[] PackData)
		{
			PackData = null;
			AnalysisResutl result;
			try
			{
				AnalysisResutl analysisResutl = AnalysisResutl.AnalysisFail;
				int num = 0;
				for (int i = 0; i < this.m_abtBuffer.Length - 5; i++)
				{
					if (this.m_abtBuffer[i] == 41 && this.m_abtBuffer[i + 1] == 41)
					{
						num = i;
						int num2 = (int)this.m_abtBuffer[i + 3] << 8;
						num2 += (int)this.m_abtBuffer[i + 4];
						if (5 + num2 > this.m_abtBuffer.Length)
						{
							break;
						}
						if (i + num2 + 4 > this.m_abtBuffer.Length)
						{
							num = 0;
							break;
						}
						int num3 = i + num2 + 4;
						if (this.m_abtBuffer[num3] == 13)
						{
							byte b = this.m_abtBuffer[num3 - 1];
							if (b == CBsjProtocol.GetXorValue(this.m_abtBuffer, i, num2 + 3))
							{
								PackData = new byte[num2 + 5];
								Buffer.BlockCopy(this.m_abtBuffer, i, PackData, 0, PackData.Length);
								int num4 = PackData.Length;
								byte[] array = new byte[this.m_abtBuffer.Length - (i + num4)];
								Buffer.BlockCopy(this.m_abtBuffer, i + num4, array, 0, array.Length);
								this.m_abtBuffer = array;
								num = 0;
								analysisResutl = AnalysisResutl.AnalysisOK;
								break;
							}
						}
					}
				}
				if (analysisResutl != AnalysisResutl.AnalysisOK && num > 0)
				{
					byte[] array2 = new byte[this.m_abtBuffer.Length - 1];
					Buffer.BlockCopy(this.m_abtBuffer, 1, array2, 0, array2.Length);
					this.m_abtBuffer = array2;
				}
				result = analysisResutl;
			}
			catch (Exception)
			{
				result = AnalysisResutl.AnalysisError;
			}
			return result;
		}
		public static byte GetXorValue(byte[] abtData, int iStartPos, int nLength)
		{
			byte b = abtData[iStartPos];
			nLength += iStartPos;
			iStartPos++;
			for (int i = iStartPos; i < nLength; i++)
			{
				b ^= abtData[i];
			}
			return b;
		}
		public static int GetIPAddress(ref byte[] buff)
		{
			DWORDIPAddress dWORDIPAddress = default(DWORDIPAddress);
			dWORDIPAddress.Byte1 = buff[5];
			dWORDIPAddress.Byte2 = buff[6];
			dWORDIPAddress.Byte3 = buff[7];
			dWORDIPAddress.Byte4 = buff[8];
			return dWORDIPAddress.Address;
		}
		public static string FormatArray(byte[] buff)
		{
			string[] array = new string[buff.Length];
			for (int i = 0; i < buff.Length; i++)
			{
				array[i] = buff[i].ToString("X2");
			}
			return string.Join(" ", array);
		}
		public static byte[] CombinePacket(byte nCmdID, int nIPAddress, byte[] Content)
		{
			byte[] result;
			try
			{
				byte[] array = new byte[Content.Length + 11];
				array[0] = 41;
				array[1] = 41;
				array[2] = nCmdID;
				array[3] = (byte)((array.Length - 5 & 65280) >> 8);
				array[4] = (byte)(array.Length - 5 & 255);
				DWORDIPAddress dWORDIPAddress = default(DWORDIPAddress);
				dWORDIPAddress.Address = nIPAddress;
				array[5] = dWORDIPAddress.Byte1;
				array[6] = dWORDIPAddress.Byte2;
				array[7] = dWORDIPAddress.Byte3;
				array[8] = dWORDIPAddress.Byte4;
				Buffer.BlockCopy(Content, 0, array, 9, Content.Length);
				array[array.Length - 2] = CBsjProtocol.GetXorValue(array, 0, array.Length - 2);
				array[array.Length - 1] = 13;
				result = array;
			}
			catch (Exception ex)
			{
				CBsjProtocol.dbgPrint(ex);
				result = null;
			}
			return result;
		}
		public static byte[] CombinePacket(byte nCmdID, int nIPAddress)
		{
			byte[] result;
			try
			{
				byte[] array = new byte[11];
				array[0] = 41;
				array[1] = 41;
				array[2] = nCmdID;
				array[3] = (byte)((array.Length - 5 & 65280) >> 8);
				array[4] = (byte)(array.Length - 5 & 255);
				DWORDIPAddress dWORDIPAddress = default(DWORDIPAddress);
				dWORDIPAddress.Address = nIPAddress;
				array[5] = dWORDIPAddress.Byte1;
				array[6] = dWORDIPAddress.Byte2;
				array[7] = dWORDIPAddress.Byte3;
				array[8] = dWORDIPAddress.Byte4;
				array[array.Length - 2] = CBsjProtocol.GetXorValue(array, 0, array.Length - 2);
				array[array.Length - 1] = 13;
				result = array;
			}
			catch (Exception ex)
			{
				CBsjProtocol.dbgPrint(ex);
				result = null;
			}
			return result;
		}
		public static int IPToInteger(string strIP)
		{
			if (strIP.Length <= 0)
			{
				return 0;
			}
			DWORDIPAddress dWORDIPAddress = default(DWORDIPAddress);
			string[] array = strIP.Split(new char[]
			{
				'.'
			});
			dWORDIPAddress.Byte1 = byte.Parse(array[0]);
			dWORDIPAddress.Byte2 = byte.Parse(array[1]);
			dWORDIPAddress.Byte3 = byte.Parse(array[2]);
			dWORDIPAddress.Byte4 = byte.Parse(array[3]);
			return dWORDIPAddress.Address;
		}
		public static string IPToString(int iIP)
		{
			DWORDIPAddress dWORDIPAddress = default(DWORDIPAddress);
			dWORDIPAddress.Address = iIP;
			return string.Concat(new string[]
			{
				dWORDIPAddress.Byte1.ToString(),
				".",
				dWORDIPAddress.Byte2.ToString(),
				".",
				dWORDIPAddress.Byte3.ToString(),
				".",
				dWORDIPAddress.Byte4.ToString()
			});
		}
		public static byte[] MakeLoginPacket(string strUser, string strPass)
		{
			byte[] result;
			try
			{
				byte[] array = new byte[40];
				byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(strUser);
				for (int i = 0; i < 20; i++)
				{
					if (i < bytes.Length)
					{
						array[i] = bytes[i];
					}
					else
					{
						array[i] = 32;
					}
				}
				bytes = Encoding.GetEncoding("gb2312").GetBytes(strPass);
				for (int j = 0; j < 20; j++)
				{
					if (j < bytes.Length)
					{
						array[20 + j] = bytes[j];
					}
					else
					{
						array[20 + j] = 32;
					}
				}
				result = CBsjProtocol.CombinePacket(163, 0, array);
			}
			catch (Exception ex)
			{
				CBsjProtocol.dbgPrint(ex);
				result = null;
			}
			return result;
		}
		public static bool TryParse(byte[] PackData, out byte[] ContextData)
		{
			ContextData = null;
			bool result;
			try
			{
				if (PackData[0] == 41 && PackData[1] == 41 && PackData[PackData.Length - 1] == 13 && PackData.Length > 11)
				{
					ContextData = new byte[PackData.Length - 11];
					Buffer.BlockCopy(PackData, 9, ContextData, 0, ContextData.Length);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				CBsjProtocol.dbgPrint(ex);
				result = false;
			}
			return result;
		}
		public static string ConvertMobileIPAddress(string sSim)
		{
			int[] array = new int[4];
			string[] array2 = new string[4];
			string result;
			try
			{
				int num;
				if (sSim.Length == 11)
				{
					array[0] = int.Parse(sSim.Substring(3, 2));
					array[1] = int.Parse(sSim.Substring(5, 2));
					array[2] = int.Parse(sSim.Substring(7, 2));
					array[3] = int.Parse(sSim.Substring(9, 2));
					num = int.Parse(sSim.Substring(1, 2)) - 30;
				}
				else
				{
					if (sSim.Length == 10)
					{
						array[0] = int.Parse(sSim.Substring(2, 2));
						array[1] = int.Parse(sSim.Substring(4, 2));
						array[2] = int.Parse(sSim.Substring(6, 2));
						array[3] = int.Parse(sSim.Substring(8, 2));
						num = int.Parse(sSim.Substring(0, 2)) - 30;
					}
					else
					{
						if (sSim.Length == 9)
						{
							array[0] = int.Parse(sSim.Substring(1, 2));
							array[1] = int.Parse(sSim.Substring(3, 2));
							array[2] = int.Parse(sSim.Substring(5, 2));
							array[3] = int.Parse(sSim.Substring(7, 2));
							num = int.Parse(sSim.Substring(0, 1));
						}
						else
						{
							if (sSim.Length >= 9)
							{
								result = "";
								return result;
							}
							switch (sSim.Length)
							{
							case 1:
								sSim = "1400000000" + sSim;
								break;
							case 2:
								sSim = "140000000" + sSim;
								break;
							case 3:
								sSim = "14000000" + sSim;
								break;
							case 4:
								sSim = "1400000" + sSim;
								break;
							case 5:
								sSim = "140000" + sSim;
								break;
							case 6:
								sSim = "14000" + sSim;
								break;
							case 7:
								sSim = "1400" + sSim;
								break;
							case 8:
								sSim = "140" + sSim;
								break;
							}
							array[0] = int.Parse(sSim.Substring(3, 2));
							array[1] = int.Parse(sSim.Substring(5, 2));
							array[2] = int.Parse(sSim.Substring(7, 2));
							array[3] = int.Parse(sSim.Substring(9, 2));
							num = int.Parse(sSim.Substring(1, 2)) - 30;
						}
					}
				}
				if ((num & 8) != 0)
				{
					array2[0] = (array[0] | 128).ToString();
				}
				else
				{
					array2[0] = array[0].ToString();
				}
				if ((num & 4) != 0)
				{
					array2[1] = (array[1] | 128).ToString();
				}
				else
				{
					array2[1] = array[1].ToString();
				}
				if ((num & 2) != 0)
				{
					array2[2] = (array[2] | 128).ToString();
				}
				else
				{
					array2[2] = array[2].ToString();
				}
				if ((num & 1) != 0)
				{
					array2[3] = (array[3] | 128).ToString();
				}
				else
				{
					array2[3] = array[3].ToString();
				}
				result = string.Concat(new string[]
				{
					array2[0],
					".",
					array2[1],
					".",
					array2[2],
					".",
					array2[3]
				});
			}
			catch (Exception ex)
			{
				CBsjProtocol.dbgPrint(ex);
				result = "";
			}
			return result;
		}
		public static string Analyze(byte[] buff, CBsjProtocol.ShowImageEx showImage)
		{
			string result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("\r\n[协议解析]\r\n协议类型：\t\tBSJ协议");
				int num = (int)buff[2];
				int num2 = num;
				if (num2 <= 133)
				{
					if (num2 <= 82)
					{
						if (num2 == 10)
						{
							stringBuilder.AppendLine("指令类型：\t\t设置特定时间段 定时拍照并上传");
							goto IL_693;
						}
						switch (num2)
						{
						case 21:
							stringBuilder.AppendLine("指令类型：\t\t设置识别器的遥控编码");
							goto IL_693;
						case 22:
							stringBuilder.AppendLine("指令类型：\t\t设置识别器的控制状态");
							goto IL_693;
						case 23:
							stringBuilder.AppendLine("指令类型：\t\t控制内置继电器状态");
							goto IL_693;
						case 33:
							stringBuilder.AppendLine("指令类型：\t\t中心确认包");
							goto IL_693;
						case 38:
							stringBuilder.AppendLine("指令类型：\t\t设置报警上传图象方式");
							goto IL_693;
						case 39:
							stringBuilder.AppendLine("指令类型：\t\t图像名称查询");
							goto IL_693;
						case 40:
							stringBuilder.AppendLine("指令类型：\t\t图像采集");
							goto IL_693;
						case 41:
							stringBuilder.AppendLine("指令类型：\t\t提取指定名称图片");
							goto IL_693;
						case 48:
							stringBuilder.AppendLine("指令类型：\t\t下发点名");
							goto IL_693;
						case 49:
							stringBuilder.AppendLine("指令类型：\t\t查看状态");
							goto IL_693;
						case 50:
							stringBuilder.AppendLine("指令类型：\t\t复位重启");
							goto IL_693;
						case 52:
							stringBuilder.AppendLine("指令类型：\t\t点火时(ACC开)定时上传位置信息时间间隔");
							goto IL_693;
						case 55:
							stringBuilder.AppendLine("指令类型：\t\t清除报警");
							goto IL_693;
						case 56:
							stringBuilder.AppendLine("指令类型：\t\t恢复油路");
							goto IL_693;
						case 57:
							stringBuilder.AppendLine("指令类型：\t\t控制断油");
							goto IL_693;
						case 58:
							stringBuilder.AppendLine("指令类型：\t\t向指定车载设备下发调度文本信息");
							CBsjProtocol.ParseTextInfo(buff, ref stringBuilder);
							goto IL_693;
						case 61:
							stringBuilder.AppendLine("指令类型：\t\t查看版本");
							goto IL_693;
						case 62:
							stringBuilder.AppendLine("指令类型：\t\t单向电话监听");
							goto IL_693;
						case 63:
							stringBuilder.AppendLine("指令类型：\t\t设置超速报警门限");
							goto IL_693;
						case 64:
							stringBuilder.AppendLine("指令类型：\t\t设置停车超时报警门限");
							goto IL_693;
						case 70:
							stringBuilder.AppendLine("指令类型：\t\t下载矩形电子围栏");
							goto IL_693;
						case 71:
							stringBuilder.AppendLine("指令类型：\t\t取消矩形电子围栏");
							goto IL_693;
						case 72:
							stringBuilder.AppendLine("指令类型：\t\t查询矩形电子围栏");
							goto IL_693;
						case 73:
							stringBuilder.AppendLine("指令类型：\t\t带超速限制的电子围栏");
							goto IL_693;
						case 74:
							stringBuilder.AppendLine("指令类型：\t\t取消带超速限制电子围栏");
							goto IL_693;
						case 75:
							stringBuilder.AppendLine("指令类型：\t\t查询带超速限制的电子围栏");
							goto IL_693;
						case 76:
							stringBuilder.AppendLine("指令类型：\t\t下载线路节点");
							goto IL_693;
						case 77:
							stringBuilder.AppendLine("指令类型：\t\t取消线路节点");
							goto IL_693;
						case 78:
							stringBuilder.AppendLine("指令类型：\t\t查询线路节点");
							goto IL_693;
						case 80:
							stringBuilder.AppendLine("指令类型：\t\t设置多边形区域");
							goto IL_693;
						case 81:
							stringBuilder.AppendLine("指令类型：\t\t取消多边形区域");
							goto IL_693;
						case 82:
							stringBuilder.AppendLine("指令类型：\t\t查询多边形区域");
							goto IL_693;
						}
					}
					else
					{
						switch (num2)
						{
						case 101:
							stringBuilder.AppendLine("指令类型：\t\t设置定时拍照并上传");
							goto IL_693;
						case 102:
							stringBuilder.AppendLine("指令类型：\t\t行驶里程数初始化");
							goto IL_693;
						case 103:
						case 104:
							break;
						case 105:
							stringBuilder.AppendLine("指令类型：\t\t修改UDP IP及端口");
							goto IL_693;
						default:
							switch (num2)
							{
							case 112:
								stringBuilder.AppendLine("指令类型：\t\t熄火时定时上传位置信息时间间隔");
								goto IL_693;
							case 118:
								stringBuilder.AppendLine("指令类型：\t\t修改TCP IP及端口");
								goto IL_693;
							case 119:
								stringBuilder.AppendLine("指令类型：\t\t修改短信中心号码");
								goto IL_693;
							case 120:
								stringBuilder.AppendLine("指令类型：\t\t中心下发透传指令");
								goto IL_693;
							case 122:
								stringBuilder.AppendLine("指令类型：\t\tTCP心跳时间间隔");
								goto IL_693;
							case 123:
								stringBuilder.AppendLine("指令类型：\t\t定时定次回传");
								goto IL_693;
							case 125:
								stringBuilder.AppendLine("指令类型：\t\t设置CDMA终端定时上线时间间隔");
								goto IL_693;
							case 126:
								stringBuilder.AppendLine("指令类型：\t\t设置CDMA终端发送短信劫警信息开关");
								goto IL_693;
							case 128:
								stringBuilder.AppendLine("指令类型：\t\t定位数据");
								CBsjProtocol.ParsePosition(buff, ref stringBuilder);
								goto IL_693;
							case 129:
								stringBuilder.AppendLine("指令类型：\t\t点名答应");
								CBsjProtocol.ParsePosition(buff, ref stringBuilder);
								goto IL_693;
							case 130:
								stringBuilder.AppendLine("指令类型：\t\t特殊报警数据");
								goto IL_693;
							case 132:
								stringBuilder.AppendLine("指令类型：\t\t上发文本或调度信息");
								CBsjProtocol.ParseTextInfo(buff, ref stringBuilder);
								goto IL_693;
							case 133:
								stringBuilder.AppendLine("指令类型：\t\t车载终端确认包");
								CBsjProtocol.ParseAnswerCommand(buff, ref stringBuilder);
								goto IL_693;
							}
							break;
						}
					}
				}
				else
				{
					if (num2 <= 159)
					{
						switch (num2)
						{
						case 141:
							stringBuilder.AppendLine("指令类型：\t\t图像数据");
							CBsjProtocol.ParseImageData_8D(buff, ref stringBuilder, showImage);
							goto IL_693;
						case 142:
							stringBuilder.AppendLine("指令类型：\t\t盲区补传数据");
							CBsjProtocol.ParseRecoup(buff, ref stringBuilder);
							goto IL_693;
						case 143:
						case 144:
							break;
						case 145:
							stringBuilder.AppendLine("指令类型：\t\t透传协议");
							goto IL_693;
						default:
							switch (num2)
							{
							case 154:
								stringBuilder.AppendLine("指令类型：\t\t上发电子围栏数据");
								goto IL_693;
							case 155:
								stringBuilder.AppendLine("指令类型：\t\t上发带限制速度的电子围栏数据");
								goto IL_693;
							case 156:
								stringBuilder.AppendLine("指令类型：\t\t上发线路节点数据");
								goto IL_693;
							case 157:
								stringBuilder.AppendLine("指令类型：\t\t上传图片名称");
								CBsjProtocol.ParseTextInfo(buff, ref stringBuilder);
								goto IL_693;
							case 158:
								stringBuilder.AppendLine("指令类型：\t\t上传带图片名称的图片数据");
								CBsjProtocol.ParseImageData_9E(buff, ref stringBuilder, showImage);
								goto IL_693;
							case 159:
								stringBuilder.AppendLine("指令类型：\t\t上发多边形区域节点数据");
								goto IL_693;
							}
							break;
						}
					}
					else
					{
						if (num2 == 179)
						{
							stringBuilder.AppendLine("指令类型：\t\t上发版本或状态信息");
							CBsjProtocol.ParseTextInfo(buff, ref stringBuilder);
							goto IL_693;
						}
						switch (num2)
						{
						case 240:
							stringBuilder.AppendLine("指令类型：\t\t上发日记文件指令");
							goto IL_693;
						case 241:
							stringBuilder.AppendLine("指令类型：\t\t识别器上传数据");
							goto IL_693;
						}
					}
				}
				stringBuilder.AppendLine("指令类型：\t\t其它未知指令");
				IL_693:
				stringBuilder.Append("\r\n");
				result = stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				CBsjProtocol.dbgPrint(ex);
				result = "解析错误！" + ex.Message;
			}
			return result;
		}
		private static void ParseAnswerCommand(byte[] buff, ref StringBuilder sb)
		{
			try
			{
				byte[] array;
				CBsjProtocol.TryParse(buff, out array);
				sb.Append("源指令：\t\t0x" + array[array.Length - 1].ToString("X2") + "\t");
				byte b = array[array.Length - 1];
				if (b <= 133)
				{
					if (b <= 82)
					{
						if (b == 10)
						{
							sb.AppendLine("指令类型->设置特定时间段 定时拍照并上传");
							goto IL_6A1;
						}
						switch (b)
						{
						case 21:
							sb.AppendLine(" 指令类型->设置识别器的遥控编码");
							goto IL_6A1;
						case 22:
							sb.AppendLine(" 指令类型->设置识别器的控制状态");
							goto IL_6A1;
						case 23:
							sb.AppendLine(" 指令类型->控制内置继电器状态");
							goto IL_6A1;
						case 33:
							sb.AppendLine(" 指令类型->中心确认包");
							goto IL_6A1;
						case 38:
							sb.AppendLine("指令类型->设置报警上传图象方式");
							goto IL_6A1;
						case 39:
							sb.AppendLine("指令类型->图像名称查询");
							goto IL_6A1;
						case 40:
							sb.AppendLine("指令类型->图像采集");
							goto IL_6A1;
						case 41:
							sb.AppendLine("指令类型->提取指定名称图片");
							goto IL_6A1;
						case 48:
							sb.AppendLine("指令类型->下发点名");
							goto IL_6A1;
						case 49:
							sb.AppendLine(" 指令类型->查看状态");
							goto IL_6A1;
						case 50:
							sb.AppendLine("指令类型->复位重启");
							goto IL_6A1;
						case 52:
							sb.AppendLine(" 指令类型->点火时(ACC开)定时上传位置信息时间间隔");
							goto IL_6A1;
						case 55:
							sb.AppendLine("指令类型->清除报警");
							goto IL_6A1;
						case 56:
							sb.AppendLine("指令类型->恢复油路");
							goto IL_6A1;
						case 57:
							sb.AppendLine("指令类型->控制断油");
							goto IL_6A1;
						case 58:
							sb.AppendLine("指令类型->扩展指令设置");
							goto IL_6A1;
						case 61:
							sb.AppendLine(" 指令类型->查看版本");
							goto IL_6A1;
						case 62:
							sb.AppendLine("指令类型->单向电话监听");
							goto IL_6A1;
						case 63:
							sb.AppendLine("指令类型->设置超速报警门限");
							goto IL_6A1;
						case 64:
							sb.AppendLine("指令类型->设置停车超时报警门限");
							goto IL_6A1;
						case 70:
							sb.AppendLine("指令类型->下载矩形电子围栏");
							goto IL_6A1;
						case 71:
							sb.AppendLine("指令类型->取消矩形电子围栏");
							goto IL_6A1;
						case 72:
							sb.AppendLine("指令类型->查询矩形电子围栏");
							goto IL_6A1;
						case 73:
							sb.AppendLine("指令类型->带超速限制的电子围栏");
							goto IL_6A1;
						case 74:
							sb.AppendLine("指令类型->取消带超速限制电子围栏");
							goto IL_6A1;
						case 75:
							sb.AppendLine("指令类型->查询带超速限制的电子围栏");
							goto IL_6A1;
						case 76:
							sb.AppendLine("指令类型->下载线路节点");
							goto IL_6A1;
						case 77:
							sb.AppendLine("指令类型->取消线路节点");
							goto IL_6A1;
						case 78:
							sb.AppendLine("指令类型->查询线路节点");
							goto IL_6A1;
						case 80:
							sb.AppendLine("指令类型->设置多边形区域");
							goto IL_6A1;
						case 81:
							sb.AppendLine("指令类型->取消多边形区域");
							goto IL_6A1;
						case 82:
							sb.AppendLine("指令类型->查询多边形区域");
							goto IL_6A1;
						}
					}
					else
					{
						switch (b)
						{
						case 101:
							sb.AppendLine("指令类型->设置定时拍照并上传");
							goto IL_6A1;
						case 102:
							sb.AppendLine("指令类型->行驶里程数初始化");
							goto IL_6A1;
						case 103:
						case 104:
							break;
						case 105:
							sb.AppendLine("指令类型->修改UDP IP及端口");
							goto IL_6A1;
						default:
							switch (b)
							{
							case 112:
								sb.AppendLine("指令类型->熄火时定时上传位置信息时间间隔");
								goto IL_6A1;
							case 118:
								sb.AppendLine("指令类型->修改TCP IP及端口");
								goto IL_6A1;
							case 119:
								sb.AppendLine("指令类型->修改短信中心号码");
								goto IL_6A1;
							case 120:
								sb.AppendLine("指令类型->中心下发透传指令");
								goto IL_6A1;
							case 122:
								sb.AppendLine("指令类型->TCP心跳时间间隔");
								goto IL_6A1;
							case 123:
								sb.AppendLine(" 指令类型->定时定次回传");
								goto IL_6A1;
							case 125:
								sb.AppendLine("指令类型->设置CDMA终端定时上线时间间隔");
								goto IL_6A1;
							case 126:
								sb.AppendLine("指令类型->设置CDMA终端发送短信劫警信息开关");
								goto IL_6A1;
							case 128:
								sb.AppendLine("指令类型->定位数据");
								goto IL_6A1;
							case 129:
								sb.AppendLine("指令类型->点名答应");
								goto IL_6A1;
							case 130:
								sb.AppendLine("指令类型->特殊报警数据");
								goto IL_6A1;
							case 132:
								sb.AppendLine("指令类型：\t\t上发文本或调度信息");
								goto IL_6A1;
							case 133:
								sb.AppendLine("指令类型->车载终端确认包");
								goto IL_6A1;
							}
							break;
						}
					}
				}
				else
				{
					if (b <= 159)
					{
						switch (b)
						{
						case 141:
							sb.AppendLine("指令类型->图像数据");
							goto IL_6A1;
						case 142:
							sb.AppendLine("指令类型->盲区补传数据");
							goto IL_6A1;
						case 143:
						case 144:
							break;
						case 145:
							sb.AppendLine("指令类型->透传协议");
							goto IL_6A1;
						default:
							switch (b)
							{
							case 154:
								sb.AppendLine("指令类型->上发电子围栏数据");
								goto IL_6A1;
							case 155:
								sb.AppendLine("指令类型->上发带限制速度的电子围栏数据");
								goto IL_6A1;
							case 156:
								sb.AppendLine("指令类型->上发线路节点数据");
								goto IL_6A1;
							case 157:
								sb.AppendLine("指令类型->上传图片名称");
								goto IL_6A1;
							case 158:
								sb.AppendLine("指令类型->上传带图片名称的图片数据");
								goto IL_6A1;
							case 159:
								sb.AppendLine("指令类型->上发多边形区域节点数据");
								goto IL_6A1;
							}
							break;
						}
					}
					else
					{
						if (b == 179)
						{
							sb.AppendLine("指令类型->上发版本或状态信息");
							goto IL_6A1;
						}
						switch (b)
						{
						case 240:
							sb.AppendLine("指令类型->上发日记文件指令");
							goto IL_6A1;
						case 241:
							sb.AppendLine("指令类型->识别器上传数据");
							goto IL_6A1;
						}
					}
				}
				sb.AppendLine("指令类型->其它未知指令");
				IL_6A1:
				sb.AppendLine(string.Concat(new string[]
				{
					"日期时间：\t\t20",
					array[0].ToString("X2"),
					"-",
					array[1].ToString("X2"),
					"-",
					array[2].ToString("X2"),
					" ",
					array[3].ToString("X2"),
					":",
					array[4].ToString("X2"),
					":",
					array[5].ToString("X2")
				}));
				string text = array[6].ToString("X2") + array[7].ToString("X2") + array[8].ToString("X2") + array[9].ToString("X2");
				sb.AppendLine(string.Concat(new string[]
				{
					"纬度：\t\t\t",
					text.Substring(0, 3),
					"度",
					text.Substring(3, 2),
					"分",
					text.Substring(5, 2),
					"秒"
				}));
				text = array[10].ToString("X2") + array[11].ToString("X2") + array[12].ToString("X2") + array[13].ToString("X2");
				sb.AppendLine(string.Concat(new string[]
				{
					"经度：\t\t\t",
					text.Substring(0, 3),
					"度",
					text.Substring(3, 2),
					"分",
					text.Substring(5, 2),
					"秒"
				}));
				text = array[14].ToString("X2") + array[15].ToString("X2");
				sb.AppendLine("速度：\t\t\t" + int.Parse(text).ToString() + " 公里/小时");
				text = array[16].ToString("X2") + array[17].ToString("X2");
				sb.AppendLine("方向：\t\t\t" + int.Parse(text).ToString() + "度角");
				sb.Append("T状态位：\t\t");
				if ((array[18] & 128) != 0)
				{
					sb.Append("GPS己定位，");
				}
				else
				{
					sb.Append("GPS未定位，");
				}
				if ((array[18] & 40) != 0 && (array[18] & 20) != 0)
				{
					sb.Append("GPS天线正常，");
				}
				else
				{
					sb.Append("GPS天线开路，");
				}
				if ((array[18] & 10) != 0 && (array[18] & 8) != 0)
				{
					sb.Append("电源正常");
				}
				else
				{
					if ((array[18] & 10) != 0 && (array[18] & 8) == 0)
					{
						sb.Append("主电源掉电");
					}
					else
					{
						if ((array[18] & 40) == 0 && (array[18] & 20) != 0)
						{
							sb.Append("主电源电压过低");
						}
					}
				}
				text = array[19].ToString("X2") + array[20].ToString("X2") + array[21].ToString("X2");
				double num = (double)int.Parse(text, NumberStyles.HexNumber);
				num /= 1000.0;
				sb.AppendLine("\r\n里程：\t\t\t" + num.ToString("0.000") + " 公里");
			}
			catch (Exception ex)
			{
				CBsjProtocol.dbgPrint(ex);
				sb.AppendLine("解析错误！" + ex.Message);
			}
		}
		private static void ParsePosition(byte[] buff, ref StringBuilder sb)
		{
			try
			{
				byte[] array;
				CBsjProtocol.TryParse(buff, out array);
				sb.AppendLine(string.Concat(new string[]
				{
					"日期时间：\t\t20",
					array[0].ToString("X2"),
					"-",
					array[1].ToString("X2"),
					"-",
					array[2].ToString("X2"),
					" ",
					array[3].ToString("X2"),
					":",
					array[4].ToString("X2"),
					":",
					array[5].ToString("X2")
				}));
				string text = array[6].ToString("X2") + array[7].ToString("X2") + array[8].ToString("X2") + array[9].ToString("X2");
				sb.AppendLine(string.Concat(new string[]
				{
					"纬度：\t\t\t",
					text.Substring(0, 3),
					"度",
					text.Substring(3, 2),
					"分",
					text.Substring(5, 2),
					"秒"
				}));
				text = array[10].ToString("X2") + array[11].ToString("X2") + array[12].ToString("X2") + array[13].ToString("X2");
				sb.AppendLine(string.Concat(new string[]
				{
					"经度：\t\t\t",
					text.Substring(0, 3),
					"度",
					text.Substring(3, 2),
					"分",
					text.Substring(5, 2),
					"秒"
				}));
				text = array[14].ToString("X2") + array[15].ToString("X2");
				sb.AppendLine("速度：\t\t\t" + int.Parse(text).ToString() + " 公里/小时");
				text = array[16].ToString("X2") + array[17].ToString("X2");
				sb.AppendLine("方向：\t\t\t" + int.Parse(text).ToString() + "度角");
				sb.Append("T状态位：\t\t");
				if ((array[18] & 128) != 0)
				{
					sb.Append("GPS己定位，");
				}
				else
				{
					sb.Append("GPS未定位，");
				}
				if ((array[18] & 40) != 0 && (array[18] & 20) != 0)
				{
					sb.Append("GPS天线正常，");
				}
				else
				{
					sb.Append("GPS天线开路，");
				}
				if ((array[18] & 10) != 0 && (array[18] & 8) != 0)
				{
					sb.Append("电源正常");
				}
				else
				{
					if ((array[18] & 10) != 0 && (array[18] & 8) == 0)
					{
						sb.Append("主电源掉电");
					}
					else
					{
						if ((array[18] & 40) == 0 && (array[18] & 20) != 0)
						{
							sb.Append("主电源电压过低");
						}
					}
				}
				text = array[19].ToString("X2") + array[20].ToString("X2") + array[21].ToString("X2");
				double num = (double)int.Parse(text, NumberStyles.HexNumber);
				num /= 1000.0;
				sb.AppendLine("\r\n里程：\t\t\t" + num.ToString("0.000") + " 公里");
			}
			catch (Exception ex)
			{
				CBsjProtocol.dbgPrint(ex);
				sb.AppendLine("解析错误！" + ex.Message);
			}
		}
		private static void ParseImageName(byte[] buff, ref StringBuilder sb)
		{
			try
			{
				byte[] array;
				if (CBsjProtocol.TryParse(buff, out array))
				{
					if (array == null)
					{
						sb.AppendLine("图片名称：\t\t无图片名称！");
					}
					else
					{
						sb.Append("\t\t\t\t");
						for (int i = 0; i < array.Length; i++)
						{
							if (i % 7 == 0)
							{
								sb.Append("\r\n\t\t\t\t");
							}
							sb.Append(array[i].ToString("X2"));
						}
						sb.Append("\r\n");
					}
				}
				else
				{
					sb.AppendLine("图片名称：\t\t无图片名称！");
				}
			}
			catch (Exception ex)
			{
				sb.AppendLine("解析失败！" + ex.Message);
				CBsjProtocol.dbgPrint(ex);
			}
		}
		private static void ParseRecoup(byte[] buff, ref StringBuilder sb)
		{
			try
			{
				byte[] array;
				CBsjProtocol.TryParse(buff, out array);
				sb.AppendLine(string.Concat(new string[]
				{
					"日期时间：\t\t20",
					array[0].ToString("X2"),
					"-",
					array[1].ToString("X2"),
					"-",
					array[2].ToString("X2"),
					" ",
					array[3].ToString("X2"),
					":",
					array[4].ToString("X2"),
					":",
					array[5].ToString("X2")
				}));
				string text = array[6].ToString("X2") + array[7].ToString("X2") + array[8].ToString("X2") + array[9].ToString("X2");
				sb.AppendLine(string.Concat(new string[]
				{
					"纬度：\t\t\t",
					text.Substring(0, 3),
					"度",
					text.Substring(3, 2),
					"分",
					text.Substring(5, 2),
					"秒"
				}));
				text = array[10].ToString("X2") + array[11].ToString("X2") + array[12].ToString("X2") + array[13].ToString("X2");
				sb.AppendLine(string.Concat(new string[]
				{
					"经度：\t\t\t",
					text.Substring(0, 3),
					"度",
					text.Substring(3, 2),
					"分",
					text.Substring(5, 2),
					"秒"
				}));
				text = array[14].ToString("X2") + array[15].ToString("X2");
				sb.AppendLine("速度：\t\t\t" + int.Parse(text).ToString() + " 公里/小时");
				text = array[16].ToString("X2") + array[17].ToString("X2");
				sb.AppendLine("方向：\t\t\t" + int.Parse(text).ToString() + "度角");
				sb.Append("T状态位：\t\t");
				if ((array[18] & 128) != 0)
				{
					sb.Append("GPS己定位，");
				}
				else
				{
					sb.Append("GPS未定位，");
				}
				if ((array[18] & 40) != 0)
				{
					sb.Append("GPS已差分定位");
				}
				else
				{
					sb.Append("GPS未差分定位");
				}
			}
			catch (Exception ex)
			{
				sb.AppendLine("解析错误！" + ex.Message);
				CBsjProtocol.dbgPrint(ex);
			}
		}
		private static void ParseImageData_9E(byte[] buff, ref StringBuilder sb, CBsjProtocol.ShowImageEx showImage)
		{
			try
			{
				string text = "";
				string text2 = CBsjProtocol.GetIPAddress(ref buff).ToString("X8");
				string text3 = "BSJ_9E_" + text2;
				byte[] array;
				if (CBsjProtocol.TryParse(buff, out array))
				{
					string text4 = string.Concat(new string[]
					{
						array[1].ToString("X2"),
						array[2].ToString("X2"),
						array[3].ToString("X2"),
						array[4].ToString("X2"),
						array[5].ToString("X2"),
						array[6].ToString("X2")
					});
					text3 = text3 + "_" + text4 + "_";
					byte b = array[7];
					byte b2 = (byte)(array[8] & 0x0f);
					byte b3 = array[9];
					byte b4 = array[10];
					sb.AppendLine("当前包序号：\t\t" + b.ToString());
					sb.AppendLine("当前包总数：\t\t" + b4.ToString());
					b2 = (byte)(b2 >> 4);
					byte b5 = b2;
					switch (b5)
					{
					case 1:
						sb.AppendLine("摄像头路数：\t\t第1路");
						break;
					case 2:
						sb.AppendLine("摄像头路数：\t\t第2路");
						break;
					case 3:
						break;
					case 4:
						sb.AppendLine("摄像头路数：\t\t第3路");
						break;
					default:
						if (b5 == 8)
						{
							sb.AppendLine("摄像头路数：\t\t第4路");
						}
						break;
					}
					switch (b3)
					{
					case 1:
						sb.AppendLine("主机状态：\t\t劫警");
						break;
					case 2:
						sb.AppendLine("主机状态：\t\t自定义高");
						break;
					case 3:
						sb.AppendLine("主机状态：\t\t自定义低");
						break;
					case 4:
						sb.AppendLine("主机状态：\t\t定时回传图像");
						break;
					case 5:
						sb.AppendLine("主机状态：\t\t捕捉图像回传");
						break;
					}
					string text5 = text3;
					text3 = string.Concat(new string[]
					{
						text5,
						"_",
						b.ToString("X2"),
						"_",
						b2.ToString("X2")
					});
					text3 = text + "\\temp\\" + text3;
					using (FileStream fileStream = new FileStream(text3, FileMode.Create))
					{
						fileStream.Write(array, 11, array.Length - 11);
					}
					int num = 0;
					for (int i = 1; i <= (int)b4; i++)
					{
						string text6 = string.Concat(new string[]
						{
							text,
							"\\temp\\BSJ_9E_",
							text2,
							"_",
							text4,
							"_"
						});
						string text7 = text6;
						text6 = string.Concat(new string[]
						{
							text7,
							"_",
							i.ToString("X2"),
							"_",
							b2.ToString("X2")
						});
						if (File.Exists(text6))
						{
							num++;
						}
					}
					sb.AppendLine("己接收包数：\t\t" + num.ToString() + "/" + b4.ToString());
					if (num != (int)b4)
					{
						goto IL_4AF;
					}
					string text8 = text + "\\Photo\\" + text2;
					if (!Directory.Exists(text8))
					{
						Directory.CreateDirectory(text8);
					}
					text8 = text8 + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
					using (FileStream fileStream2 = new FileStream(text8, FileMode.Create))
					{
						for (int j = 1; j <= (int)b4; j++)
						{
							string text9 = string.Concat(new string[]
							{
								text,
								"\\temp\\BSJ_9E_",
								text2,
								"_",
								text4,
								"_"
							});
							string text10 = text9;
							text9 = string.Concat(new string[]
							{
								text10,
								"_",
								j.ToString("X2"),
								"_",
								b2.ToString("X2")
							});
							using (FileStream fileStream3 = new FileStream(text9, FileMode.Open))
							{
								byte[] array2 = new byte[fileStream3.Length];
								fileStream3.Read(array2, 0, array2.Length);
								fileStream2.Write(array2, 0, array2.Length);
							}
							File.Delete(text9);
						}
						fileStream2.Flush();
						goto IL_4AF;
					}
				}
				sb.AppendLine("解析错误！图像数据包无内容");
				IL_4AF:;
			}
			catch (Exception ex)
			{
				CBsjProtocol.dbgPrint(ex);
				sb.AppendLine("解析错误！" + ex.Message);
			}
		}
		private static void ParseImageData_8D(byte[] buff, ref StringBuilder sb, CBsjProtocol.ShowImageEx showImage)
		{
			try
			{
				string str = "";
				string text = CBsjProtocol.GetIPAddress(ref buff).ToString("X8");
				string text2 = "BSJ_8D_" + text;
				byte[] array;
				if (CBsjProtocol.TryParse(buff, out array))
				{
					byte b = array[0];
					byte b2 = (byte)(array[1] & 0x0f);
					byte b3 = array[2];
					byte b4 = array[3];
					sb.AppendLine("当前包序号：\t\t" + b.ToString());
					sb.AppendLine("当前包总数：\t\t" + b4.ToString());
					b2 = (byte)(b2 >> 4);
					byte b5 = b2;
					switch (b5)
					{
					case 1:
						sb.AppendLine("摄像头路数：\t\t第1路");
						break;
					case 2:
						sb.AppendLine("摄像头路数：\t\t第2路");
						break;
					case 3:
						break;
					case 4:
						sb.AppendLine("摄像头路数：\t\t第3路");
						break;
					default:
						if (b5 == 8)
						{
							sb.AppendLine("摄像头路数：\t\t第4路");
						}
						break;
					}
					switch (b3)
					{
					case 1:
						sb.AppendLine("主机状态：\t\t劫警");
						break;
					case 2:
						sb.AppendLine("主机状态：\t\t自定义高");
						break;
					case 3:
						sb.AppendLine("主机状态：\t\t自定义低");
						break;
					case 4:
						sb.AppendLine("主机状态：\t\t定时回传图像");
						break;
					case 5:
						sb.AppendLine("主机状态：\t\t捕捉图像回传");
						break;
					}
					string text3 = text2;
					text2 = string.Concat(new string[]
					{
						text3,
						"_",
						b.ToString("X2"),
						"_",
						b2.ToString("X2")
					});
					text2 = str + "\\temp\\" + text2;
					using (FileStream fileStream = new FileStream(text2, FileMode.Create))
					{
						fileStream.Write(array, 4, array.Length - 4);
					}
					int num = 0;
					for (int i = 1; i <= (int)b4; i++)
					{
						string text4 = str + "\\temp\\BSJ_8D_" + text;
						string text5 = text4;
						text4 = string.Concat(new string[]
						{
							text5,
							"_",
							i.ToString("X2"),
							"_",
							b2.ToString("X2")
						});
						if (File.Exists(text4))
						{
							num++;
						}
					}
					sb.AppendLine("己接收包数：\t\t" + b.ToString() + "/" + num.ToString());
					if (num != (int)b4)
					{
						goto IL_3A7;
					}
					string text6 = str + "\\Photo\\" + text;
					if (!Directory.Exists(text6))
					{
						Directory.CreateDirectory(text6);
					}
					text6 = text6 + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
					using (FileStream fileStream2 = new FileStream(text6, FileMode.Create))
					{
						for (int j = 1; j <= (int)b4; j++)
						{
							string text7 = str + "\\temp\\BSJ_8D_" + text;
							string text8 = text7;
							text7 = string.Concat(new string[]
							{
								text8,
								"_",
								j.ToString("X2"),
								"_",
								b2.ToString("X2")
							});
							using (FileStream fileStream3 = new FileStream(text7, FileMode.Open))
							{
								byte[] array2 = new byte[fileStream3.Length];
								fileStream3.Read(array2, 0, array2.Length);
								fileStream2.Write(array2, 0, array2.Length);
							}
							File.Delete(text7);
						}
						fileStream2.Flush();
						goto IL_3A7;
					}
				}
				sb.AppendLine("解析错误！图像数据包无内容");
				IL_3A7:;
			}
			catch (Exception ex)
			{
				CBsjProtocol.dbgPrint(ex);
				sb.AppendLine("解析错误！" + ex.Message);
			}
		}
		private static void ParseTextInfo(byte[] buff, ref StringBuilder sb)
		{
			try
			{
				byte[] array;
				if (CBsjProtocol.TryParse(buff, out array))
				{
					if (array != null)
					{
						string @string = Encoding.GetEncoding("gb2312").GetString(array);
						sb.AppendLine("文本内容：\t\t" + @string);
					}
					else
					{
						sb.AppendLine("无文本内容 ");
					}
				}
				else
				{
					sb.AppendLine("抽取文字信息失败！");
				}
			}
			catch (Exception ex)
			{
				sb.AppendLine("解析失败！" + ex.Message);
				CBsjProtocol.dbgPrint(ex);
			}
		}
		private static void dbgPrint(Exception ex)
		{
		}
	}
}
