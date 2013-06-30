using BSJProtocol;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
namespace gpsTran
{
	internal class CFeiTanClient
	{
		public delegate void OnDataPacketArrivalsEx(string strCmdPacket);
		public delegate void OnClientStatusChangeEx(bool blnChange);
		public delegate void ShowMessageEx(string strMsg);
		public delegate void NetCountEx(int nCount);
		private WaitCallback _WaitCallback;
		private bool m_blnExit;
		private Thread m_NetRecvThread;
		private Socket m_sckClient;
		private string m_strServer;
		private int m_nPort = 0;
		private string m_strUser;
		private string m_strPass;
		private int _SendCount = 0;
		private int _RecvCount = 0;
		private AsyncCallback _SendAsyncCallback;
		public event CFeiTanClient.NetCountEx SendNetCount;
        public event CFeiTanClient.NetCountEx RecvNetCount;
		public event CFeiTanClient.OnClientStatusChangeEx OnClientStatusChange;
		public event CFeiTanClient.OnDataPacketArrivalsEx OnDataPacketArrivals;
		public event CFeiTanClient.ShowMessageEx ShowMessage;
		public void InitConnect(string strServer, int nPort, string strUser, string strPass)
		{
			try
			{
				this._WaitCallback = new WaitCallback(this.SendDataEx);
				this._SendAsyncCallback = new AsyncCallback(this.SendComplete);
				this._SendCount = 0;
				this._RecvCount = 0;
				this.m_strServer = strServer;
				this.m_nPort = nPort;
				this.m_strUser = strUser;
				this.m_strPass = strPass;
				this.Close();
				this.m_NetRecvThread = new Thread(new ThreadStart(this.RecvThread));
				this.m_NetRecvThread.Name = "飞田服务器连接线程";
				this.m_NetRecvThread.IsBackground = true;
				this.m_NetRecvThread.Start();
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		public void Close()
		{
			try
			{
				this.m_blnExit = true;
				if (this.m_sckClient != null)
				{
					this.m_sckClient.Close();
				}
				Thread.Sleep(100);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		public void SendData(SendGPSDataPacket gpsData)
		{
			try
			{
				ThreadPool.QueueUserWorkItem(this._WaitCallback, gpsData);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void SendDataEx(object obj)
		{
			try
			{
				SendGPSDataPacket sendGPSDataPacket = (SendGPSDataPacket)obj;
				byte b = sendGPSDataPacket.gpsData[2];
				if (b <= 150)
				{
					switch (b)
					{
					case 128:
						this.ParsePositionPacket(sendGPSDataPacket);
						break;
					case 129:
						this.ParsePositionPacket(sendGPSDataPacket);
						break;
					default:
						if (b != 150)
						{
						}
						break;
					}
				}
				else
				{
					if (b != 165)
					{
						if (b == 170)
						{
							if (sendGPSDataPacket.gpsData[9] == 5)
							{
								int num = sendGPSDataPacket.gpsData.Length;
								num -= 12;
								if (num > 0)
								{
									byte[] array = new byte[num];
									Buffer.BlockCopy(sendGPSDataPacket.gpsData, 10, array, 0, num);
									byte[] value = new byte[]
									{
										array[2],
										array[1]
									};
									int num2 = (int)BitConverter.ToUInt16(value, 0);
									if (num2 == 512)
									{
										this.ParseGBPosition(sendGPSDataPacket, array);
									}
								}
							}
						}
					}
					else
					{
						this.ExplainDB44Byte(sendGPSDataPacket);
					}
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void ParseGBPosition(SendGPSDataPacket gpsData, byte[] data)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			byte[] array = new byte[4];
			byte[] array2 = new byte[8];
			uint dWord = this.GetDWord(data, 13);
			uint dWord2 = this.GetDWord(data, 17);
			uint dWord3 = this.GetDWord(data, 21);
			uint dWord4 = this.GetDWord(data, 25);
			ushort word = this.GetWord(data, 29);
			ushort word2 = this.GetWord(data, 31);
			ushort word3 = this.GetWord(data, 33);
			bool flag = false;
			bool flag2 = false;
			this.GetComponentStatus(dWord2, out flag, out flag2, ref num, ref num2, ref num3, ref num4);
			string s;
			if (flag && flag2)
			{
				s = string.Concat(new string[]
				{
					"20",
					data[35].ToString("X2"),
					"-",
					data[36].ToString("X2"),
					"-",
					data[37].ToString("X2"),
					" ",
					data[38].ToString("X2"),
					":",
					data[39].ToString("X2"),
					":",
					data[40].ToString("X2")
				});
			}
			else
			{
				s = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			}
			DateTime dt;
			if (DateTime.TryParse(s, out dt))
			{
				double latitude = dWord3 / 1000000.0;
				double longitude = dWord4 / 1000000.0;
				int velocity = Convert.ToInt32((double)word2 / 10.0);
				this.GetAlarmStatus(dWord, ref num, ref num2, ref num3, ref num4);
				string str = "";
				byte[] buff = CFeiTianCommand.MakePosition(gpsData.DeviceNo, dt, true, longitude, latitude, velocity, (int)word3, "0", out str);
				this.PrintStatus("\r\n己转发定位数据   ->  [" + gpsData.DeviceNo + "] \r\n" + str);
				this.SendData(buff);
			}
		}
		private void GetComponentStatus(uint status, out bool blnLocate, out bool blnAcc, ref int iS1, ref int iS2, ref int iS3, ref int iS4)
		{
			StringBuilder stringBuilder = new StringBuilder();
			blnLocate = ((status & 2u) != 0u);
			blnAcc = ((status & 1u) != 0u);
			iS1++;
			iS1 += 2;
			if ((status & 1u) == 1u)
			{
				iS2 += 32;
			}
			if ((status & 2u) != 0u)
			{
				iS3++;
			}
			if ((status & 8192u) != 0u)
			{
				iS2 += 32;
			}
			if ((status & 32768u) != 0u)
			{
				iS2 += 2;
			}
			if ((status & 262144u) != 0u)
			{
				iS2 += 4;
			}
			if ((status & 524288u) != 0u)
			{
				iS2 += 8;
			}
			if ((status & 1048576u) != 0u)
			{
				iS2 += 16;
			}
		}
		private void GetAlarmStatus(uint status, ref int iS1, ref int iS2, ref int iS3, ref int iS4)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if ((status & 1u) != 0u)
			{
				iS1 += 4;
			}
			if ((status & 2u) != 0u)
			{
				iS1 += 16;
			}
			if ((status & 4u) != 0u)
			{
				iS4 += 8;
			}
			if ((status & 16u) != 0u)
			{
				iS3 += 4;
			}
			if ((status & 32u) != 0u)
			{
				iS3 += 2;
			}
			if ((status & 64u) != 0u)
			{
				iS4++;
			}
			if ((status & 128u) != 0u)
			{
				iS1 += 64;
			}
			if ((status & 1048576u) != 0u)
			{
				stringBuilder.Append("驶出区域报警 驶入区域报警 ");
				iS3 += 32;
				iS3 += 64;
			}
			if ((status & 8388608u) != 0u)
			{
				stringBuilder.Append("偏离线路报警 ");
			}
			string text = stringBuilder.ToString();
		}
		public ushort GetWord(byte[] data, int Index)
		{
			byte[] value = new byte[]
			{
				data[Index + 1],
				data[Index]
			};
			return BitConverter.ToUInt16(value, 0);
		}
		public uint GetDWord(byte[] data, int Index)
		{
			byte[] value = new byte[]
			{
				data[Index + 3],
				data[Index + 2],
				data[Index + 1],
				data[Index]
			};
			return BitConverter.ToUInt32(value, 0);
		}
		public string GetByteBCD(byte[] data, int iIndex, int iLength)
		{
			string text = "";
			for (int i = iIndex; i < iIndex + 4; i++)
			{
				text += data[i].ToString("X2");
			}
			return text;
		}
		public void ExplainDB44Byte(SendGPSDataPacket gpsData)
		{
			byte[] gpsData2 = gpsData.gpsData;
			if (gpsData2[29] == 0 | gpsData2[29] == 1 | gpsData2[29] == 2 | gpsData2[29] == 3 | gpsData2[29] == 4 | gpsData2[29] == 5 | gpsData2[29] == 6 | gpsData2[29] == 15 | gpsData2[29] == 19 | gpsData2[29] == 21 | gpsData2[29] == 22 | gpsData2[29] == 23 | gpsData2[29] == 24 | gpsData2[29] == 25 | gpsData2[29] == 26 | gpsData2[29] == 27 | gpsData2[29] == 28 | gpsData2[29] == 29 | gpsData2[29] == 225)
			{
				this.ParseDB44Position(gpsData);
			}
		}
		public string GetIndexTime(byte[] pPacketData, int iStartIndex)
		{
			string result;
			try
			{
				string text = "20" + pPacketData[iStartIndex].ToString("X2");
				string text2 = pPacketData[iStartIndex + 1].ToString("X2");
				string text3 = pPacketData[iStartIndex + 2].ToString("X2");
				string text4 = pPacketData[iStartIndex + 3].ToString("X2");
				string text5 = pPacketData[iStartIndex + 4].ToString("X2");
				string text6 = pPacketData[iStartIndex + 5].ToString("X2");
				result = string.Concat(new string[]
				{
					text,
					"-",
					text2,
					"-",
					text3,
					" ",
					text4,
					":",
					text5,
					":",
					text6
				});
			}
			catch
			{
				result = "";
			}
			return result;
		}
		public void ParseDB44Position(SendGPSDataPacket gpsData)
		{
			byte[] gpsData2 = gpsData.gpsData;
			string ipAddress = this.GetIpAddress(gpsData2, 5);
			DateTime dt = Convert.ToDateTime(this.GetIndexTime(gpsData2, 30));
			bool flag = false;
			double num = this.GetLongitude(gpsData2, 36, ref flag);
			if (flag)
			{
				double num2 = this.GetLatitude(gpsData2, 40, ref flag);
				if (flag)
				{
					int velocity = (int)gpsData2[44];
					int angle = (int)(gpsData2[45] * 2);
					double num3 = (double)Convert.ToInt64(gpsData2[48].ToString("X2") + gpsData2[49].ToString("X2") + gpsData2[50].ToString("X2") + gpsData2[51].ToString("X2"), 10);
					num3 *= 100.0;
					string text = "";
					int num4 = 0;
					int num5 = 0;
					int num6 = 1;
					int num7 = 1;
					int[] array = new int[]
					{
						1,
						1,
						1,
						1
					};
					string text2 = this.PositionDb44Status(gpsData2, 52, ref text, ref num4, ref num5, ref num6, ref num7);
					num *= (double)num6;
					num2 *= (double)num7;
					string str = "";
					byte[] buff = CFeiTianCommand.MakePosition(gpsData.DeviceNo, dt, true, num, num2, velocity, angle, "0", out str);
					this.PrintStatus("\r\n己转发定位数据   ->  [" + gpsData.DeviceNo + "] \r\n" + str);
					this.SendData(buff);
				}
			}
		}
		public string PositionDb44Status(byte[] pPacketData, int Statusindex, ref string sAlarm, ref int iAccStatus, ref int iLocalFlag, ref int iLongFlag, ref int iLatiFlag)
		{
			string text = "";
			string text2 = "";
			if ((pPacketData[Statusindex] & 128) == 128)
			{
			}
			if ((pPacketData[Statusindex] & 64) == 64)
			{
				text2 += "主电源断电报警 ";
			}
			if ((pPacketData[Statusindex] & 32) == 32)
			{
				text2 += "振动报警 ";
			}
			if ((pPacketData[Statusindex] & 16) == 16)
			{
				text2 += "超速报警 ";
			}
			if ((pPacketData[Statusindex] & 8) == 8)
			{
				text += "断油 ";
			}
			if ((pPacketData[Statusindex] & 4) == 4)
			{
				text2 += "紧急报警 ";
			}
			if ((pPacketData[Statusindex] & 2) == 2)
			{
				iLatiFlag = 1;
			}
			else
			{
				iLatiFlag = -1;
			}
			if ((pPacketData[Statusindex] & 1) == 1)
			{
				iLongFlag = 1;
			}
			else
			{
				iLongFlag = -1;
			}
			if ((pPacketData[Statusindex + 1] & 128) == 128)
			{
			}
			if ((pPacketData[Statusindex + 1] & 64) == 64)
			{
			}
			if ((pPacketData[Statusindex + 1] & 32) == 32)
			{
				iAccStatus = 1;
				text += "ACC开 ";
			}
			else
			{
				iAccStatus = 0;
				text += "ACC关 ";
			}
			if ((pPacketData[Statusindex + 1] & 16) == 16)
			{
				text += "远光灯开 ";
			}
			if ((pPacketData[Statusindex + 1] & 8) == 8)
			{
				text += "右转向灯开 ";
			}
			if ((pPacketData[Statusindex + 1] & 4) == 4)
			{
				text += "左转向灯开 ";
			}
			if ((pPacketData[Statusindex + 1] & 2) == 2)
			{
				text += "开门 ";
			}
			if ((pPacketData[Statusindex + 1] & 1) == 1)
			{
				text += "刹车 ";
			}
			if ((pPacketData[Statusindex + 2] & 128) == 128)
			{
			}
			if ((pPacketData[Statusindex + 2] & 64) == 64)
			{
				text2 += "入区域越界 ";
			}
			if ((pPacketData[Statusindex + 2] & 32) == 32)
			{
				text2 += "出区域越界 ";
			}
			if ((pPacketData[Statusindex + 2] & 16) == 16)
			{
				text += "通信模块异常 ";
			}
			if ((pPacketData[Statusindex + 2] & 8) == 8)
			{
				text += "定位模块异常 ";
			}
			if ((pPacketData[Statusindex + 2] & 4) == 4)
			{
				text2 += "天线开路 ";
			}
			if ((pPacketData[Statusindex + 2] & 2) == 2)
			{
				text2 += "天线短路 ";
			}
			if ((pPacketData[Statusindex + 2] & 1) == 1)
			{
				iLocalFlag = 1;
				text += "卫星锁定 ";
			}
			else
			{
				iLocalFlag = 0;
				text += "卫星未锁定 ";
			}
			if ((pPacketData[Statusindex + 3] & 128) == 128)
			{
			}
			if ((pPacketData[Statusindex + 3] & 64) == 64)
			{
			}
			if ((pPacketData[Statusindex + 3] & 32) == 32)
			{
			}
			if ((pPacketData[Statusindex + 3] & 16) == 16)
			{
			}
			if ((pPacketData[Statusindex + 3] & 8) == 8)
			{
				text2 += "疲劳驾驶报警 ";
			}
			if ((pPacketData[Statusindex + 3] & 4) == 4)
			{
				text += "发动机开 ";
			}
			else
			{
				text += "发动机关 ";
			}
			if ((pPacketData[Statusindex + 3] & 2) == 2)
			{
				text2 += "越界 ";
			}
			if ((pPacketData[Statusindex + 3] & 1) == 1)
			{
				text += "备用电池异常 ";
			}
			if ((pPacketData[Statusindex + 4] & 128) == 128)
			{
			}
			if ((pPacketData[Statusindex + 4] & 64) == 64)
			{
				text += "摄像头连接状态断开 ";
			}
			else
			{
				text += "摄像头连接状态连上 ";
			}
			if ((pPacketData[Statusindex + 4] & 32) == 32)
			{
				text += "打印机连接状态断开 ";
			}
			else
			{
				text += "打印机连接状态连上 ";
			}
			if ((pPacketData[Statusindex + 4] & 16) == 16)
			{
				text += "SD卡状态异常 ";
			}
			if ((pPacketData[Statusindex + 4] & 8) == 8)
			{
				text += "显示屏状态异常 ";
			}
			if ((pPacketData[Statusindex + 4] & 4) == 4)
			{
				text += "FLASH状态异常 ";
			}
			if ((pPacketData[Statusindex + 4] & 2) == 2)
			{
				text += "内存状态异常 ";
			}
			if ((pPacketData[Statusindex + 4] & 1) == 1)
			{
				text += "CPU状态异常 ";
			}
			if ((pPacketData[Statusindex + 5] & 128) == 128)
			{
			}
			if ((pPacketData[Statusindex + 5] & 64) == 64)
			{
			}
			if ((pPacketData[Statusindex + 5] & 32) == 32)
			{
			}
			if ((pPacketData[Statusindex + 5] & 16) == 16)
			{
			}
			if ((pPacketData[Statusindex + 5] & 8) == 8)
			{
			}
			if ((pPacketData[Statusindex + 5] & 4) == 4)
			{
			}
			if ((pPacketData[Statusindex + 5] & 2) == 2)
			{
				text += "定时 ";
			}
			if ((pPacketData[Statusindex + 5] & 1) == 1)
			{
				text += "定距 ";
			}
			sAlarm = text2;
			return text;
		}
		public string GetIpAddress(byte[] pPacketData, int iStartIndex)
		{
			return string.Concat(new object[]
			{
				pPacketData[iStartIndex],
				".",
				pPacketData[iStartIndex + 1],
				".",
				pPacketData[iStartIndex + 2],
				".",
				pPacketData[iStartIndex + 3]
			});
		}
		public string GetTime(byte[] pPacketData, int iStartIndex)
		{
			string result;
			try
			{
				string text = "20" + pPacketData[iStartIndex].ToString("X2");
				string text2 = pPacketData[iStartIndex + 1].ToString("X2");
				string text3 = pPacketData[iStartIndex + 2].ToString("X2");
				string text4 = pPacketData[iStartIndex + 3].ToString("X2");
				string text5 = pPacketData[iStartIndex + 4].ToString("X2");
				string text6 = pPacketData[iStartIndex + 5].ToString("X2");
				result = string.Concat(new string[]
				{
					text,
					"-",
					text2,
					"-",
					text3,
					" ",
					text4,
					":",
					text5,
					":",
					text6
				});
			}
			catch
			{
				result = "";
			}
			return result;
		}
		public string GetTime(byte[] pPacketData, int iStartIndex, int intFlag)
		{
			string result;
			try
			{
				string text = "";
				string text2 = "";
				string text3 = "";
				string text4 = "";
				string text5 = "";
				string text6 = "";
				switch (intFlag)
				{
				case 0:
					text = "20" + pPacketData[iStartIndex].ToString("X2");
					text2 = pPacketData[iStartIndex + 1].ToString("X2");
					text3 = pPacketData[iStartIndex + 2].ToString("X2");
					text4 = pPacketData[iStartIndex + 3].ToString("X2");
					text5 = pPacketData[iStartIndex + 4].ToString("X2");
					text6 = pPacketData[iStartIndex + 5].ToString("X2");
					break;
				case 1:
					text = DateTime.Now.Year.ToString();
					text2 = DateTime.Now.Month.ToString();
					text3 = DateTime.Now.Day.ToString();
					text4 = pPacketData[iStartIndex].ToString("X2");
					text5 = pPacketData[iStartIndex + 1].ToString("X2");
					text6 = pPacketData[iStartIndex + 2].ToString("X2");
					break;
				case 2:
					text = DateTime.Now.Year.ToString();
					text2 = pPacketData[iStartIndex].ToString("X2");
					text3 = pPacketData[iStartIndex + 1].ToString("X2");
					text4 = pPacketData[iStartIndex + 2].ToString("X2");
					text5 = pPacketData[iStartIndex + 3].ToString("X2");
					text6 = pPacketData[iStartIndex + 4].ToString("X2");
					break;
				case 3:
					text = DateTime.Now.Year.ToString();
					text2 = DateTime.Now.Month.ToString();
					text3 = DateTime.Now.Day.ToString();
					text4 = pPacketData[iStartIndex].ToString("X2");
					text5 = pPacketData[iStartIndex + 1].ToString("X2");
					text6 = pPacketData[iStartIndex + 2].ToString("X2");
					break;
				}
				result = string.Concat(new string[]
				{
					text,
					"-",
					text2,
					"-",
					text3,
					" ",
					text4,
					":",
					text5,
					":",
					text6
				});
			}
			catch
			{
				result = "";
			}
			return result;
		}
		public double GetLatitude(byte[] pPacketData, int iStartIndex, ref bool bUse)
		{
			double result;
			try
			{
				string text = "";
				for (int i = iStartIndex; i < iStartIndex + 4; i++)
				{
					text += pPacketData[i].ToString("X2");
				}
				double num = Convert.ToDouble(text.Substring(3, 2) + "." + text.Substring(5)) / 60.0;
				double num2 = num + (double)Convert.ToInt16(text.Substring(0, 3), 10);
				bUse = true;
				result = num2;
			}
			catch
			{
				bUse = false;
				result = 0.0;
			}
			return result;
		}
		public double GetLongitude(byte[] pPacketData, int iStartIndex, ref bool bUse)
		{
			double result;
			try
			{
				string text = "";
				for (int i = iStartIndex; i < iStartIndex + 4; i++)
				{
					text += pPacketData[i].ToString("X2");
				}
				double num = Convert.ToDouble(text.Substring(3, 2) + "." + text.Substring(5)) / 60.0;
				double num2 = num + (double)Convert.ToInt16(text.Substring(0, 3), 10);
				bUse = true;
				result = num2;
			}
			catch
			{
				bUse = false;
				result = 0.0;
			}
			return result;
		}
		public int GetVelocity(byte[] pPacketData, int iStartIndex)
		{
			int result;
			try
			{
				int num;
				if (pPacketData.Length == 35)
				{
					string value = pPacketData[iStartIndex].ToString("X2") + pPacketData[iStartIndex + 1].ToString("X2");
					num = (int)Convert.ToInt16((double)Convert.ToInt16(value, 10) * 1.8432);
				}
				else
				{
					if (pPacketData.Length == 32)
					{
						if (pPacketData[28] == 0)
						{
							string value = pPacketData[iStartIndex].ToString("X2") + pPacketData[iStartIndex + 1].ToString("X2");
							num = (int)Convert.ToInt16(value, 10);
						}
						else
						{
							string value = pPacketData[iStartIndex].ToString("X2") + pPacketData[iStartIndex + 1].ToString("X2");
							num = (int)Convert.ToInt16((double)Convert.ToInt16(value, 10) * 1.8432);
						}
					}
					else
					{
						string value = pPacketData[iStartIndex].ToString("X2") + pPacketData[iStartIndex + 1].ToString("X2");
						num = (int)Convert.ToInt16(value, 10);
					}
				}
				result = num;
			}
			catch
			{
				result = 0;
			}
			return result;
		}
		public int GetAngle(byte[] pPacketData, int iStartIndex)
		{
			int result;
			try
			{
				string value = pPacketData[iStartIndex].ToString("X2") + pPacketData[iStartIndex + 1].ToString("X2");
				result = (int)Convert.ToInt16(value, 10);
			}
			catch
			{
				result = 0;
			}
			return result;
		}
		public int GetLocalFlag(byte[] pPacketData, int iStartIndex)
		{
			string text = pPacketData[iStartIndex].ToString("X2");
			string text2 = text.Substring(0, 1);
			int result;
			switch (text2)
			{
			case "8":
			case "9":
			case "A":
			case "B":
			case "C":
			case "D":
			case "E":
			case "F":
				result = 1;
				return result;
			}
			result = 0;
			return result;
		}
		private void ParsePositionPacket(SendGPSDataPacket cmdPack)
		{
			try
			{
				int iPAddress = CBsjProtocol.GetIPAddress(ref cmdPack.gpsData);
				byte[] array;
				CBsjProtocol.TryParse(cmdPack.gpsData, out array);
				string s = string.Concat(new string[]
				{
					"20",
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
				});
				string text = array[6].ToString("X2") + array[7].ToString("X2") + array[8].ToString("X2") + array[9].ToString("X2");
				double latitude = Math.Round(double.Parse(text.Substring(0, 3)) + double.Parse(text.Substring(3, 2) + "." + text.Substring(5, 2)) / 60.0, 5);
				text = array[10].ToString("X2") + array[11].ToString("X2") + array[12].ToString("X2") + array[13].ToString("X2");
				double longitude = Math.Round(double.Parse(text.Substring(0, 3)) + double.Parse(text.Substring(3, 2) + "." + text.Substring(5, 2)) / 60.0, 5);
				text = array[14].ToString("X2") + array[15].ToString("X2");
				double num = double.Parse(text);
				text = array[16].ToString("X2") + array[17].ToString("X2");
				short angle = short.Parse(text);
				DateTime now;
				if (!DateTime.TryParse(s, out now))
				{
					now = DateTime.Now;
				}
				string str = "";
				byte[] buff = CFeiTianCommand.MakePosition(cmdPack.DeviceNo, now, true, longitude, latitude, (int)num, (int)angle, "0", out str);
				this.PrintStatus("\r\n己转发定位数据   ->  [" + cmdPack.DeviceNo + "] \r\n" + str);
				this.SendData(buff);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		public void SendData(byte[] buff)
		{
			try
			{
				if (this.m_sckClient.Connected)
				{
					this.m_sckClient.BeginSend(buff, 0, buff.Length, SocketFlags.None, this._SendAsyncCallback, null);
					Interlocked.Increment(ref this._SendCount);
					if (this.SendNetCount != null)
					{
						this.SendNetCount(this._SendCount);
					}
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
				try
				{
					this.m_sckClient.Close();
				}
				catch (Exception)
				{
				}
			}
		}
		private void SendComplete(IAsyncResult ia)
		{
			try
			{
				this.m_sckClient.EndSend(ia);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
				try
				{
					this.m_sckClient.Close();
				}
				catch (Exception)
				{
				}
			}
		}
		private void RecvThread()
		{
			try
			{
				this.m_blnExit = false;
				while (!this.m_blnExit)
				{
					try
					{
						this.m_sckClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
						this.m_sckClient.Connect(this.m_strServer, this.m_nPort);
						if (this.OnClientStatusChange != null)
						{
							this.OnClientStatusChange(this.m_sckClient.Connected);
						}
						if (!this.m_sckClient.Connected)
						{
							this.PrintStatus("连接交委服务器失败");
							Thread.Sleep(2000);
						}
						else
						{
							byte[] array = new byte[8192];
							CFeiTianPacket cFeiTianPacket = new CFeiTianPacket();
							this.SendLoginRequest();
							while (this.m_sckClient.Connected)
							{
								int num = this.m_sckClient.Receive(array, 0, array.Length, SocketFlags.None);
								if (num == 0)
								{
									break;
								}
								cFeiTianPacket.Append(array, num);
								byte[] cmdPack;
								while (cFeiTianPacket.SplitPacket(out cmdPack))
								{
									Interlocked.Increment(ref this._RecvCount);
									if (this.RecvNetCount != null)
									{
										this.RecvNetCount(this._RecvCount);
									}
									this.ParsePacket(cmdPack);
								}
							}
						}
					}
					catch (SocketException ex)
					{
						this.PrintStatus(ex.Message);
					}
					catch (Exception ex2)
					{
						this.dbgPrint(ex2);
					}
				}
			}
			catch (Exception ex3)
			{
				this.dbgPrint(ex3);
			}
			finally
			{
				if (this.OnClientStatusChange != null)
				{
					this.OnClientStatusChange(false);
				}
			}
		}
		private void ParsePacket(byte[] cmdPack)
		{
			try
			{
				string text;
				if (CFeiTianPacket.TryParsePacket(cmdPack, out text))
				{
					string[] array = text.Split(new char[]
					{
						','
					});
					string text2 = array[0];
					if (text2 != null)
					{
						if (!(text2 == "1"))
						{
							if (text2 == "4")
							{
								this.PrintStatus("交委服务器要求车辆资料上传。。。");
								if (this.OnDataPacketArrivals != null)
								{
									this.OnDataPacketArrivals(text);
								}
							}
						}
						else
						{
							if (array[1] == "1")
							{
								this.PrintStatus("登录交委服务器成功！");
								if (this.OnDataPacketArrivals != null)
								{
									this.OnDataPacketArrivals(text);
								}
							}
							else
							{
								this.PrintStatus("登录交委服务器失败！");
								this.SendLoginRequest();
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void SendLoginRequest()
		{
			try
			{
				byte[] buff = CFeiTianCommand.MakeLogin(this.m_strUser, this.m_strPass, DateTime.Now);
				this.SendData(buff);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void PrintStatus(string strStatus)
		{
			try
			{
				if (this.ShowMessage != null)
				{
					this.ShowMessage(strStatus);
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void dbgPrint(Exception ex)
		{
			this.PrintStatus(ex.Message);
		}
	}
}
