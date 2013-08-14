using BSJProtocol;
using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
namespace gpsTran
{
	internal class CBsjClient
	{
		public delegate void OnDataPacketArrivalsEx(byte[] packData);
		public delegate void OnClientStatusChangeEx(bool blnChange);
		public delegate void ShowMessageEx(string strMsg);
		
        private Thread m_ClientThread;
		private Socket m_sckClient;
		private bool m_blnExit;
		private string m_strTcpHost;
		private int m_nTcpPort;
		private string m_strUser;
		private string m_strPass;
		private long _PacketCount = 0L;

		public event CBsjClient.OnClientStatusChangeEx OnClientStatusChange;
		public event CBsjClient.OnDataPacketArrivalsEx OnDataPacketArrivals;
		public event CBsjClient.ShowMessageEx ShowMessage;
		public long PacketCount
		{
			get
			{
				return this._PacketCount;
			}
		}
		public void InitClient(string strBSJServer, int nBSJPort, string strUser, string strPass)
		{
			this.m_nTcpPort = nBSJPort;
			this.m_strTcpHost = strBSJServer;
			this.m_strUser = strUser;
			this.m_strPass = strPass;
			this.m_ClientThread = new Thread(new ThreadStart(this.ClientThread));
			this.m_ClientThread.IsBackground = true;
			this.m_ClientThread.Name = "BSJ中心服务器连接线程";
			this.m_ClientThread.Start();
			this.m_blnExit = false;
		}

		private void ClientThread()
		{
			try
			{
				while (!this.m_blnExit)
				{
					try
					{
						if (this.m_sckClient != null)
						{
							this.m_sckClient.Close();
							this.m_sckClient = null;
						}
						this.m_sckClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
						this.m_sckClient.Connect(this.m_strTcpHost, this.m_nTcpPort);
						if (this.m_sckClient.Connected)
						{
							if (this.OnClientStatusChange != null)
							{
								this.OnClientStatusChange(true);
							}
							this.PrintStatus("连接BSJ中心服务器成功！");
							CBsjProtocol cBsjProtocol = new CBsjProtocol();
							byte[] array = new byte[8192];
							Thread.Sleep(500);
							this.SendLoginRequest(this.m_strUser, this.m_strPass);
							while (!this.m_blnExit)
							{
								try
								{
									int num = this.m_sckClient.Receive(array, array.Length, SocketFlags.None);
									if (num == 0)
									{
										this.m_sckClient.Close();
										break;
									}
									cBsjProtocol.Append(array, num);
									byte[] packet;
									for (AnalysisResutl analysisResutl = cBsjProtocol.SplitPack(out packet); analysisResutl == AnalysisResutl.AnalysisOK; analysisResutl = cBsjProtocol.SplitPack(out packet))
									{
										this.ParsePacket(packet);
									}
								}
								catch (SocketException ex)
								{
									this.dbgPrint(ex);
									break;
								}
							}
						}
						else
						{
							if (this.OnClientStatusChange != null)
							{
								this.OnClientStatusChange(false);
							}
							this.PrintStatus("连接BSJ中心服务器失败！" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
							Thread.Sleep(1000);
						}
					}
					catch (ThreadAbortException ex2)
					{
						this.m_blnExit = true;
						this.dbgPrint(ex2);
						break;
					}
					catch (SocketException ex3)
					{
						this.dbgPrint(ex3);
						Thread.Sleep(2000);
					}
					catch (Exception ex4)
					{
						this.dbgPrint(ex4);
						break;
					}
				}
			}
			catch (Exception ex4)
			{
				this.dbgPrint(ex4);
			}
			finally
			{
				if (this.m_sckClient != null)
				{
					this.m_sckClient.Close();
					this.m_sckClient = null;
				}
				if (this.OnClientStatusChange != null)
				{
					this.OnClientStatusChange(false);
				}
				this.PrintStatus("与BSJ中心服务器连接退出");
			}
		}
		private void SendLoginRequest(string strUser, string strPass)
		{
			try
			{
				byte[] buffer = CBsjProtocol.MakeLoginPacket(strUser, strPass);
				this.PrintStatus("发送登录请求！");
				this.m_sckClient.Send(buffer);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void ParsePacket(byte[] packet)
		{
			try
			{
				this._PacketCount += 1L;
				byte b = packet[2];
				if (b <= 150)
				{
					switch (b)
					{
					case 128:
						if (this.OnDataPacketArrivals != null)
						{
							this.OnDataPacketArrivals(packet);
						}
						break;
					case 129:
						if (this.OnDataPacketArrivals != null)
						{
							this.OnDataPacketArrivals(packet);
						}
						break;
					default:
						if (b == 150)
						{
							if (this.OnDataPacketArrivals != null)
							{
								this.OnDataPacketArrivals(packet);
							}
						}
						break;
					}
				}
				else
				{
					if (b != 165)
					{
						if (b != 170)
						{
							if (b == 227)
							{
								if (packet[5] == 1)
								{
									this.PrintStatus("登录中心服务器成功！");
								}
								else
								{
									this.PrintStatus("登录中心服务器失败，2秒后再次尝试登录！");
									Thread.Sleep(2000);
									this.SendLoginRequest(this.m_strUser, this.m_strPass);
								}
							}
						}
						else
						{
							if (this.OnDataPacketArrivals != null)
							{
								this.OnDataPacketArrivals(packet);
							}
						}
					}
					else
					{
						if (this.OnDataPacketArrivals != null)
						{
							this.OnDataPacketArrivals(packet);
						}
					}
				}
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
				Thread.Sleep(100);
				if (this.m_sckClient != null)
				{
					this.m_sckClient.Shutdown(SocketShutdown.Both);
					Thread.Sleep(50);
					this.m_sckClient.Close();
					this.m_sckClient = null;
				}
				Thread.Sleep(100);
				if (this.m_ClientThread != null)
				{
					this.m_ClientThread.Abort();
					this.m_ClientThread = null;
				}
				Thread.Sleep(200);
				this._PacketCount = 0L;
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		public void SendPacketEx(byte[] data)
		{
			try
			{
				this.m_sckClient.Send(data);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}

		private void dbgPrint(Exception ex)
		{
			if (this.ShowMessage != null)
			{
				this.ShowMessage(ex.Message);
			}
		}
		private void PrintStatus(string strStatus)
		{
			if (this.ShowMessage != null)
			{
				this.ShowMessage(strStatus);
			}
		}
	}
}
