using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
namespace gpsTran
{
	public partial class frmTest : Form
	{
		private delegate void PrintStatusEx(string strText, int nType);
		private Socket m_socket;
		private IContainer components = null;
		private Button button1;
		private TextBox txtServer;
		private Label label1;
		private TextBox txtPort;
		private Label label2;
		private GroupBox groupBox1;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private DateTimePicker dateTimePicker1;
		private TextBox txtUserID;
		private Button btnLogin;
		private Label label5;
		private Label label4;
		private TextBox txtPassword;
		private Label label3;
		private TabPage tabPage2;
		private CheckBox chkLocate;
		private TextBox txtLocateTime;
		private Label label8;
		private Label label7;
		private TextBox txtSpeed;
		private Label label11;
		private TextBox txtAlarm;
		private Label label13;
		private TextBox txtLatitude;
		private TextBox txtAngle;
		private Label label10;
		private Label label12;
		private TextBox txtLongitude;
		private Label label9;
		private TextBox txtDeviceNo;
		private Label label6;
		private Button btnPosition;
		private TabPage tabPage3;
		private Label label14;
		private Label label15;
		private TextBox txtVehOwnerTel;
		private Label label16;
		private Label label17;
		private TextBox txtDeviceTel;
		private Label label18;
		private TextBox txtVehNo;
		private Label label19;
		private Label label31;
		private TextBox txtVehOwner2Name;
		private Label label30;
		private TextBox txtStartTime;
		private TextBox txtVehOwnerID;
		private TextBox txtVehOwner1Tel;
		private TextBox txtVehOwner2Tel;
		private Label label29;
		private Label label28;
		private Label label22;
		private TextBox txtVehOwner1Name;
		private Label label27;
		private TextBox txtVehOwnerName;
		private Label label26;
		private TextBox txtVehOwnerPost;
		private Label label21;
		private Label label25;
		private TextBox txtVehOwnerAddress;
		private TextBox txtEndTime;
		private TextBox txtVehOwnerWork;
		private Label label24;
		private TextBox txtVehName;
		private Label label32;
		private TextBox txtVehOwnerMail;
		private Label label20;
		private Label label23;
		private ComboBox cboVehOwnerSex;
		private CheckBox chkCamera;
		private ComboBox cboVehType;
		private RichTextBox rtDisplay;
		private Button btnUpDate;
		private TabPage tabPage4;
		private Button button2;
		private TextBox textBox1;
		public frmTest()
		{
			this.InitializeComponent();
		}
		private void button1_Click(object sender, EventArgs e)
		{
			this.m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.m_socket.Connect(this.txtServer.Text.Trim(), int.Parse(this.txtPort.Text.Trim()));
			if (!this.m_socket.Connected)
			{
				this.PrintStatus("连接失败！", 0);
			}
			else
			{
				this.PrintStatus("连接成功!", 0);
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.RecvThread), this.m_socket);
			}
		}
		private void RecvThread(object obj)
		{
			try
			{
				Socket socket = (Socket)obj;
				CFeiTianPacket cFeiTianPacket = new CFeiTianPacket();
				byte[] array = new byte[8192];
				while (true)
				{
					int num = socket.Receive(array, 0, array.Length, SocketFlags.None);
					if (num <= 0)
					{
						break;
					}
					cFeiTianPacket.Append(array, num);
					byte[] packData;
					while (cFeiTianPacket.SplitPacket(out packData))
					{
						this.OnPacketArrivals(packData);
					}
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
			finally
			{
				this.PrintStatus("接收线程退出", 0);
			}
		}
		private void OnPacketArrivals(byte[] packData)
		{
			string text;
			if (CFeiTianPacket.TryParsePacket(packData, out text))
			{
				this.PrintStatus(text, 2);
				string[] array = text.Split(new char[]
				{
					','
				});
				string text2 = array[0];
				if (text2 != null)
				{
					if (text2 == "1")
					{
						if (array[1] == "1")
						{
							this.PrintStatus("登录应答:登录成功！", 0);
						}
						else
						{
							this.PrintStatus("登录应答:登录失败！", 0);
						}
						goto IL_148;
					}
					if (text2 == "4")
					{
						if (array[1] == "0")
						{
							this.PrintStatus("服务器要求上传车辆资料:要求上传所有车辆资料！", 0);
						}
						else
						{
							string[] array2 = array[1].Split(new char[]
							{
								','
							});
							switch (array2.Length)
							{
							case 0:
								this.PrintStatus("Error：要求上传所有车辆资料的指令错误，无附加参数!", 0);
								break;
							case 1:
								this.PrintStatus("要求上传终端编号为[" + array2[0] + "]的车辆资料", 0);
								break;
							default:
								this.PrintStatus("要求以下终端编号列表[" + array[1] + "]的车辆资料", 0);
								break;
							}
						}
						goto IL_148;
					}
				}
				this.PrintStatus("其它指令", 0);
				IL_148:;
			}
		}
		private void PrintStatus(string strText, int nType)
		{
			if (base.InvokeRequired)
			{
				base.Invoke(new frmTest.PrintStatusEx(this.PrintStatus), new object[]
				{
					strText,
					nType
				});
			}
			else
			{
				string str;
				switch (nType)
				{
				case 0:
					this.rtDisplay.SelectionColor = Color.White;
					str = "系统消息：\r\n";
					break;
				case 1:
					this.rtDisplay.SelectionColor = Color.Green;
					str = "发送：\r\n";
					break;
				case 2:
					this.rtDisplay.SelectionColor = Color.Red;
					str = "接收：\r\n";
					break;
				default:
					str = "其它：\r\n";
					break;
				}
				this.rtDisplay.Focus();
				this.rtDisplay.AppendText(str + strText + "\r\n");
			}
		}
		private void dbgPrint(Exception ex)
		{
			this.PrintStatus(ex.Message, 0);
		}
		private void btnLogin_Click(object sender, EventArgs e)
		{
			try
			{
				string strUserNo = this.txtUserID.Text.Trim();
				string text = this.txtPassword.Text;
				byte[] array = CFeiTianCommand.MakeLogin(strUserNo, CFeiTianPacket.md5(text), this.dateTimePicker1.Value);
				this.m_socket.Send(array);
				this.PrintStatus(CFeiTianPacket.FormatArray(array), 1);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void btnPosition_Click(object sender, EventArgs e)
		{
			try
			{
				string text = "";
				byte[] array = CFeiTianCommand.MakePosition(this.txtDeviceNo.Text.Trim(), DateTime.Now, this.chkLocate.Checked, double.Parse(this.txtLongitude.Text), double.Parse(this.txtLatitude.Text), int.Parse(this.txtSpeed.Text), int.Parse(this.txtAngle.Text), this.txtAlarm.Text.Trim(), out text);
				this.m_socket.Send(array);
				this.PrintStatus(CFeiTianPacket.FormatArray(array), 1);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void rtDisplay_DoubleClick(object sender, EventArgs e)
		{
			this.rtDisplay.Clear();
		}
		private void btnUpdate_Click(object sender, EventArgs e)
		{
			try
			{
				FeiTanVehInfo item = new FeiTanVehInfo(false);
				item.DeviceNo = this.txtVehNo.Text;
				item.VehName = this.txtVehName.Text;
				item.VehTypeNo = this.cboVehType.Text;
				item.VehPhoneNo = this.txtDeviceTel.Text;
				item.SuportCamare = (this.chkCamera.Checked ? "1" : "0");
				item.VehOwnerName = this.txtVehOwnerName.Text;
				item.VehOwnerTel = this.txtVehOwnerTel.Text;
				item.VehOwnerSex = this.cboVehOwnerSex.Text;
				item.VehOwnerID = this.txtVehOwnerID.Text;
				item.VehOwnerEmail = this.txtVehOwnerMail.Text;
				item.VehOwnerWorkUnits = this.txtVehOwnerWork.Text;
				item.VehOwenrAddress = this.txtVehOwnerAddress.Text;
				item.VehOwenrPostCode = this.txtVehOwnerPost.Text;
				item.VehOwenrContactTel1 = this.txtVehOwner1Tel.Text;
				item.VehOwenrContactName1 = this.txtVehOwner1Name.Text;
				item.VehOwenrContactTel2 = this.txtVehOwner2Tel.Text;
				item.VehOwenrContactName2 = this.txtVehOwner2Name.Text;
				item.ServerStartTime = this.txtStartTime.Text;
				item.ServerEndTime = this.txtEndTime.Text;
				byte[] array = CFeiTianCommand.MakeVehicleInfo(new List<FeiTanVehInfo>
				{
					item
				});
				this.m_socket.Send(array);
				this.PrintStatus(CFeiTianPacket.FormatArray(array), 1);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
		}
		private void frmMain_Load(object sender, EventArgs e)
		{
		}
		private void button2_Click(object sender, EventArgs e)
		{
		}
		private void button2_Click_1(object sender, EventArgs e)
		{
			try
			{
				string strText = this.textBox1.Text.Trim();
				byte[] buffer = CFeiTianPacket.CombinPacket(strText);
				this.m_socket.Send(buffer);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkLocate = new System.Windows.Forms.CheckBox();
            this.txtLocateTime = new System.Windows.Forms.TextBox();
            this.btnPosition = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAlarm = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtLatitude = new System.Windows.Forms.TextBox();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtLongitude = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDeviceNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cboVehType = new System.Windows.Forms.ComboBox();
            this.cboVehOwnerSex = new System.Windows.Forms.ComboBox();
            this.btnUpDate = new System.Windows.Forms.Button();
            this.chkCamera = new System.Windows.Forms.CheckBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtVehOwner2Name = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtStartTime = new System.Windows.Forms.TextBox();
            this.txtVehOwnerID = new System.Windows.Forms.TextBox();
            this.txtVehOwner1Tel = new System.Windows.Forms.TextBox();
            this.txtVehOwner2Tel = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtVehOwnerTel = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.txtVehOwner1Name = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtVehOwnerName = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtVehOwnerPost = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.txtDeviceTel = new System.Windows.Forms.TextBox();
            this.txtVehOwnerAddress = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtEndTime = new System.Windows.Forms.TextBox();
            this.txtVehOwnerWork = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtVehName = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.txtVehOwnerMail = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.txtVehNo = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.rtDisplay = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(467, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(88, 20);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(138, 21);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = "192.168.1.67";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "服务器地址：";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(311, 20);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(138, 21);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "6669";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(232, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "服务器端口：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtPort);
            this.groupBox1.Location = new System.Drawing.Point(7, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(650, 55);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(7, 61);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(650, 193);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dateTimePicker1);
            this.tabPage1.Controls.Add(this.txtUserID);
            this.tabPage1.Controls.Add(this.btnLogin);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(642, 167);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "登录";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(99, 92);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(138, 21);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(99, 38);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(138, 21);
            this.txtUserID.TabIndex = 1;
            this.txtUserID.Text = "123";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(258, 38);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(78, 75);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "校验码：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "校验码：";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(99, 65);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(138, 21);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "123";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "营运商ID：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkLocate);
            this.tabPage2.Controls.Add(this.txtLocateTime);
            this.tabPage2.Controls.Add(this.btnPosition);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.txtSpeed);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.txtAlarm);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.txtLatitude);
            this.tabPage2.Controls.Add(this.txtAngle);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.txtLongitude);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.txtDeviceNo);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(642, 167);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "定位数据";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkLocate
            // 
            this.chkLocate.AutoSize = true;
            this.chkLocate.Location = new System.Drawing.Point(397, 16);
            this.chkLocate.Name = "chkLocate";
            this.chkLocate.Size = new System.Drawing.Size(60, 16);
            this.chkLocate.TabIndex = 3;
            this.chkLocate.Text = "己定位";
            this.chkLocate.UseVisualStyleBackColor = true;
            // 
            // txtLocateTime
            // 
            this.txtLocateTime.Location = new System.Drawing.Point(230, 13);
            this.txtLocateTime.Name = "txtLocateTime";
            this.txtLocateTime.Size = new System.Drawing.Size(95, 21);
            this.txtLocateTime.TabIndex = 1;
            this.txtLocateTime.Text = "20110119100125";
            // 
            // btnPosition
            // 
            this.btnPosition.Location = new System.Drawing.Point(512, 7);
            this.btnPosition.Name = "btnPosition";
            this.btnPosition.Size = new System.Drawing.Size(100, 75);
            this.btnPosition.TabIndex = 0;
            this.btnPosition.Text = "发送";
            this.btnPosition.UseVisualStyleBackColor = true;
            this.btnPosition.Click += new System.EventHandler(this.btnPosition_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(331, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "定位标记：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(169, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "定位时间：";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(397, 40);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(95, 21);
            this.txtSpeed.TabIndex = 1;
            this.txtSpeed.Text = "70";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(331, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "速度：";
            // 
            // txtAlarm
            // 
            this.txtAlarm.Location = new System.Drawing.Point(230, 65);
            this.txtAlarm.Name = "txtAlarm";
            this.txtAlarm.Size = new System.Drawing.Size(95, 21);
            this.txtAlarm.TabIndex = 1;
            this.txtAlarm.Text = "1536";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(169, 70);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 2;
            this.label13.Text = "报警类型：";
            // 
            // txtLatitude
            // 
            this.txtLatitude.Location = new System.Drawing.Point(230, 40);
            this.txtLatitude.Name = "txtLatitude";
            this.txtLatitude.Size = new System.Drawing.Size(95, 21);
            this.txtLatitude.TabIndex = 1;
            this.txtLatitude.Text = "22.59587";
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(67, 65);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(95, 21);
            this.txtAngle.TabIndex = 1;
            this.txtAngle.Text = "90";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(169, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 2;
            this.label10.Text = "纬度：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 2;
            this.label12.Text = "方向：";
            // 
            // txtLongitude
            // 
            this.txtLongitude.Location = new System.Drawing.Point(67, 40);
            this.txtLongitude.Name = "txtLongitude";
            this.txtLongitude.Size = new System.Drawing.Size(95, 21);
            this.txtLongitude.TabIndex = 1;
            this.txtLongitude.Text = "113.35383";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "经度：";
            // 
            // txtDeviceNo
            // 
            this.txtDeviceNo.Location = new System.Drawing.Point(67, 13);
            this.txtDeviceNo.Name = "txtDeviceNo";
            this.txtDeviceNo.Size = new System.Drawing.Size(95, 21);
            this.txtDeviceNo.TabIndex = 1;
            this.txtDeviceNo.Text = "3838438";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "终端编号：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cboVehType);
            this.tabPage3.Controls.Add(this.cboVehOwnerSex);
            this.tabPage3.Controls.Add(this.btnUpDate);
            this.tabPage3.Controls.Add(this.chkCamera);
            this.tabPage3.Controls.Add(this.label31);
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.txtVehOwner2Name);
            this.tabPage3.Controls.Add(this.label30);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.txtStartTime);
            this.tabPage3.Controls.Add(this.txtVehOwnerID);
            this.tabPage3.Controls.Add(this.txtVehOwner1Tel);
            this.tabPage3.Controls.Add(this.txtVehOwner2Tel);
            this.tabPage3.Controls.Add(this.label29);
            this.tabPage3.Controls.Add(this.txtVehOwnerTel);
            this.tabPage3.Controls.Add(this.label28);
            this.tabPage3.Controls.Add(this.label22);
            this.tabPage3.Controls.Add(this.txtVehOwner1Name);
            this.tabPage3.Controls.Add(this.label16);
            this.tabPage3.Controls.Add(this.label27);
            this.tabPage3.Controls.Add(this.txtVehOwnerName);
            this.tabPage3.Controls.Add(this.label26);
            this.tabPage3.Controls.Add(this.label17);
            this.tabPage3.Controls.Add(this.txtVehOwnerPost);
            this.tabPage3.Controls.Add(this.label21);
            this.tabPage3.Controls.Add(this.label25);
            this.tabPage3.Controls.Add(this.txtDeviceTel);
            this.tabPage3.Controls.Add(this.txtVehOwnerAddress);
            this.tabPage3.Controls.Add(this.label18);
            this.tabPage3.Controls.Add(this.txtEndTime);
            this.tabPage3.Controls.Add(this.txtVehOwnerWork);
            this.tabPage3.Controls.Add(this.label24);
            this.tabPage3.Controls.Add(this.txtVehName);
            this.tabPage3.Controls.Add(this.label32);
            this.tabPage3.Controls.Add(this.txtVehOwnerMail);
            this.tabPage3.Controls.Add(this.label20);
            this.tabPage3.Controls.Add(this.label23);
            this.tabPage3.Controls.Add(this.txtVehNo);
            this.tabPage3.Controls.Add(this.label19);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(642, 167);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "静态数据";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cboVehType
            // 
            this.cboVehType.FormattingEnabled = true;
            this.cboVehType.Items.AddRange(new object[] {
            "省际客运班车",
            "市际客运班车",
            "旅游客运车辆",
            "县际客运班车",
            "危险货物运输车辆",
            "重型载货汽车",
            "半挂牵引车",
            "出租汽车",
            "教练车",
            "公交车",
            "其他"});
            this.cboVehType.Location = new System.Drawing.Point(381, 6);
            this.cboVehType.Name = "cboVehType";
            this.cboVehType.Size = new System.Drawing.Size(99, 20);
            this.cboVehType.TabIndex = 16;
            // 
            // cboVehOwnerSex
            // 
            this.cboVehOwnerSex.FormattingEnabled = true;
            this.cboVehOwnerSex.Items.AddRange(new object[] {
            "男",
            "女"});
            this.cboVehOwnerSex.Location = new System.Drawing.Point(225, 53);
            this.cboVehOwnerSex.Name = "cboVehOwnerSex";
            this.cboVehOwnerSex.Size = new System.Drawing.Size(84, 20);
            this.cboVehOwnerSex.TabIndex = 16;
            this.cboVehOwnerSex.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnUpDate
            // 
            this.btnUpDate.Location = new System.Drawing.Point(493, 41);
            this.btnUpDate.Name = "btnUpDate";
            this.btnUpDate.Size = new System.Drawing.Size(134, 101);
            this.btnUpDate.TabIndex = 0;
            this.btnUpDate.Text = "上传";
            this.btnUpDate.UseVisualStyleBackColor = true;
            this.btnUpDate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // chkCamera
            // 
            this.chkCamera.AutoSize = true;
            this.chkCamera.Location = new System.Drawing.Point(230, 34);
            this.chkCamera.Name = "chkCamera";
            this.chkCamera.Size = new System.Drawing.Size(48, 16);
            this.chkCamera.TabIndex = 15;
            this.chkCamera.Text = "支持";
            this.chkCamera.UseVisualStyleBackColor = true;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(156, 87);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(65, 12);
            this.label31.TabIndex = 11;
            this.label31.Text = "工作单位：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(156, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 11;
            this.label14.Text = "车牌号码：";
            // 
            // txtVehOwner2Name
            // 
            this.txtVehOwner2Name.Location = new System.Drawing.Point(225, 134);
            this.txtVehOwner2Name.Name = "txtVehOwner2Name";
            this.txtVehOwner2Name.Size = new System.Drawing.Size(84, 21);
            this.txtVehOwner2Name.TabIndex = 8;
            this.txtVehOwner2Name.Text = "1536";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(156, 139);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(71, 12);
            this.label30.TabIndex = 12;
            this.label30.Text = "联系2Name：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(156, 61);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 12;
            this.label15.Text = "车主性别：";
            // 
            // txtStartTime
            // 
            this.txtStartTime.Location = new System.Drawing.Point(381, 135);
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Size = new System.Drawing.Size(99, 21);
            this.txtStartTime.TabIndex = 3;
            this.txtStartTime.Text = "20101010";
            // 
            // txtVehOwnerID
            // 
            this.txtVehOwnerID.Location = new System.Drawing.Point(381, 57);
            this.txtVehOwnerID.Name = "txtVehOwnerID";
            this.txtVehOwnerID.Size = new System.Drawing.Size(99, 21);
            this.txtVehOwnerID.TabIndex = 3;
            this.txtVehOwnerID.Text = "90";
            // 
            // txtVehOwner1Tel
            // 
            this.txtVehOwner1Tel.Location = new System.Drawing.Point(225, 109);
            this.txtVehOwner1Tel.Name = "txtVehOwner1Tel";
            this.txtVehOwner1Tel.Size = new System.Drawing.Size(84, 21);
            this.txtVehOwner1Tel.TabIndex = 4;
            this.txtVehOwner1Tel.Text = "2259587";
            // 
            // txtVehOwner2Tel
            // 
            this.txtVehOwner2Tel.Location = new System.Drawing.Point(67, 137);
            this.txtVehOwner2Tel.Name = "txtVehOwner2Tel";
            this.txtVehOwner2Tel.Size = new System.Drawing.Size(86, 21);
            this.txtVehOwner2Tel.TabIndex = 3;
            this.txtVehOwner2Tel.Text = "1383838";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(311, 139);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(65, 12);
            this.label29.TabIndex = 14;
            this.label29.Text = "开始时间：";
            // 
            // txtVehOwnerTel
            // 
            this.txtVehOwnerTel.Location = new System.Drawing.Point(67, 59);
            this.txtVehOwnerTel.Name = "txtVehOwnerTel";
            this.txtVehOwnerTel.Size = new System.Drawing.Size(86, 21);
            this.txtVehOwnerTel.TabIndex = 3;
            this.txtVehOwnerTel.Text = "5438538";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(156, 114);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(65, 12);
            this.label28.TabIndex = 13;
            this.label28.Text = "联系1Tel：";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(311, 61);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(65, 12);
            this.label22.TabIndex = 14;
            this.label22.Text = "身份证号：";
            // 
            // txtVehOwner1Name
            // 
            this.txtVehOwner1Name.Location = new System.Drawing.Point(381, 110);
            this.txtVehOwner1Name.Name = "txtVehOwner1Name";
            this.txtVehOwner1Name.Size = new System.Drawing.Size(99, 21);
            this.txtVehOwner1Name.TabIndex = 6;
            this.txtVehOwner1Name.Text = "1135383";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(156, 36);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 13;
            this.label16.Text = "摄像头支持：";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 143);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(65, 12);
            this.label27.TabIndex = 14;
            this.label27.Text = "联系2Tel：";
            // 
            // txtVehOwnerName
            // 
            this.txtVehOwnerName.Location = new System.Drawing.Point(381, 32);
            this.txtVehOwnerName.Name = "txtVehOwnerName";
            this.txtVehOwnerName.Size = new System.Drawing.Size(99, 21);
            this.txtVehOwnerName.TabIndex = 6;
            this.txtVehOwnerName.Text = "11335383";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(311, 114);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(71, 12);
            this.label26.TabIndex = 9;
            this.label26.Text = "联系1Name：";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 65);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 14;
            this.label17.Text = "车主电话：";
            // 
            // txtVehOwnerPost
            // 
            this.txtVehOwnerPost.Location = new System.Drawing.Point(67, 112);
            this.txtVehOwnerPost.Name = "txtVehOwnerPost";
            this.txtVehOwnerPost.Size = new System.Drawing.Size(86, 21);
            this.txtVehOwnerPost.TabIndex = 6;
            this.txtVehOwnerPost.Text = "383838";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(311, 36);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(65, 12);
            this.label21.TabIndex = 9;
            this.label21.Text = "车主姓名：";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(6, 118);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(65, 12);
            this.label25.TabIndex = 9;
            this.label25.Text = "邮政编码：";
            // 
            // txtDeviceTel
            // 
            this.txtDeviceTel.Location = new System.Drawing.Point(67, 34);
            this.txtDeviceTel.Name = "txtDeviceTel";
            this.txtDeviceTel.Size = new System.Drawing.Size(86, 21);
            this.txtDeviceTel.TabIndex = 6;
            this.txtDeviceTel.Text = "3838438";
            // 
            // txtVehOwnerAddress
            // 
            this.txtVehOwnerAddress.Location = new System.Drawing.Point(381, 83);
            this.txtVehOwnerAddress.Name = "txtVehOwnerAddress";
            this.txtVehOwnerAddress.Size = new System.Drawing.Size(99, 21);
            this.txtVehOwnerAddress.TabIndex = 5;
            this.txtVehOwnerAddress.Text = "3838438";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 40);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 9;
            this.label18.Text = "终端电话：";
            // 
            // txtEndTime
            // 
            this.txtEndTime.Location = new System.Drawing.Point(552, 4);
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.Size = new System.Drawing.Size(75, 21);
            this.txtEndTime.TabIndex = 5;
            this.txtEndTime.Text = "20101212";
            // 
            // txtVehOwnerWork
            // 
            this.txtVehOwnerWork.Location = new System.Drawing.Point(226, 82);
            this.txtVehOwnerWork.Name = "txtVehOwnerWork";
            this.txtVehOwnerWork.Size = new System.Drawing.Size(83, 21);
            this.txtVehOwnerWork.TabIndex = 5;
            this.txtVehOwnerWork.Text = "3838438";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(311, 87);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(65, 12);
            this.label24.TabIndex = 10;
            this.label24.Text = "通讯地址：";
            // 
            // txtVehName
            // 
            this.txtVehName.Location = new System.Drawing.Point(226, 6);
            this.txtVehName.Name = "txtVehName";
            this.txtVehName.Size = new System.Drawing.Size(83, 21);
            this.txtVehName.TabIndex = 5;
            this.txtVehName.Text = "京sb250";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(491, 9);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(65, 12);
            this.label32.TabIndex = 10;
            this.label32.Text = "到期时间：";
            // 
            // txtVehOwnerMail
            // 
            this.txtVehOwnerMail.Location = new System.Drawing.Point(67, 85);
            this.txtVehOwnerMail.Name = "txtVehOwnerMail";
            this.txtVehOwnerMail.Size = new System.Drawing.Size(86, 21);
            this.txtVehOwnerMail.TabIndex = 5;
            this.txtVehOwnerMail.Text = "3838@38.com";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(311, 9);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(65, 12);
            this.label20.TabIndex = 10;
            this.label20.Text = "车辆类型：";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 91);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(65, 12);
            this.label23.TabIndex = 10;
            this.label23.Text = "车主mail：";
            // 
            // txtVehNo
            // 
            this.txtVehNo.Location = new System.Drawing.Point(67, 7);
            this.txtVehNo.Name = "txtVehNo";
            this.txtVehNo.Size = new System.Drawing.Size(86, 21);
            this.txtVehNo.TabIndex = 5;
            this.txtVehNo.Text = "3838438";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(6, 13);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(65, 12);
            this.label19.TabIndex = 10;
            this.label19.Text = "终端编号：";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button2);
            this.tabPage4.Controls.Add(this.textBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(642, 167);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Test";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(420, 122);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(171, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "Send";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 18);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(620, 87);
            this.textBox1.TabIndex = 0;
            // 
            // rtDisplay
            // 
            this.rtDisplay.BackColor = System.Drawing.Color.Black;
            this.rtDisplay.ForeColor = System.Drawing.Color.White;
            this.rtDisplay.Location = new System.Drawing.Point(7, 256);
            this.rtDisplay.Name = "rtDisplay";
            this.rtDisplay.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtDisplay.Size = new System.Drawing.Size(650, 198);
            this.rtDisplay.TabIndex = 6;
            this.rtDisplay.Text = "";
            this.rtDisplay.DoubleClick += new System.EventHandler(this.rtDisplay_DoubleClick);
            // 
            // frmTest
            // 
            this.ClientSize = new System.Drawing.Size(665, 466);
            this.Controls.Add(this.rtDisplay);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTest";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

		}
	}
}
