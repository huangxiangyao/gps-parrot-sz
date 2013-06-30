using BSJProtocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Properties;
namespace gpsTran
{
	public partial class frmMain : Form
	{
		private delegate void OnBsjClientStatusChangeEx(bool blnConnect);
		private delegate void ShowBsjRecvCountEx(long lngCount);
		private delegate void ShowFTNetCountEx(int nCount);
		private delegate void PrintStatusEx(string strStatus);
		private IContainer components = null;
		private GroupBox groupBox1;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private TabPage tabPage3;
		private TextBox txtGpsPort;
		private TextBox txtGpsServer;
		private Label label2;
		private Label label1;
		private TabPage tabPage4;
		private Button btnDisconnGPS;
		private Button btnConnectGPS;
		private Label lblBSJCount;
		private Label lblBsjStatus;
		private Label label4;
		private Label label3;
		private TextBox txtGpsPass;
		private TextBox txtGpsUser;
		private Label label16;
		private Label label15;
		private Button btnFTDisConnect;
		private Button btnFTConnect;
		private TextBox txtFTPass;
		private TextBox txtFTUser;
		private TextBox txtFTPort;
		private TextBox txtFTServer;
		private Label lblFTRecvCount;
		private Label lblFTSendCount;
		private Label lblFTStatus;
		private Label label17;
		private Label label9;
		private Label label10;
		private Label label11;
		private Label label12;
		private Label label13;
		private Label label14;
		private Button btnLoadVehicle;
		private Button btnSQLConnect;
		private TextBox txtSQLUser;
		private TextBox txtDBname;
		private TextBox txtSQLPort;
		private TextBox txtSQLServer;
		private Label label24;
		private Label label26;
		private Label label27;
		private Label label28;
		private Button btnSaveConfig;
		private TextBox txtDisplay;
		private TextBox txtSQLPass;
        private Label label19;
		private DataGridView dataGridView1;
		private DataGridViewCheckBoxColumn Column1;
		private Button btnLoadLocal;
		private Button btnSaveDB;
		private Button btnSelect;
		private Button button2;
		private Button button1;
		private TextBox txtCompany;
		private Label label5;
		private TextBox txtDB44Code;
		private Label label6;
		private Label label7;
		private Button button3;
		private CBsjClient m_BsjClient;
		private CFeiTanClient m_FeiTanClient;
		private frmMain.PrintStatusEx _PrintStatusEx;
		private frmMain.OnBsjClientStatusChangeEx _OnBsjClientStatusChangeEx;
		private frmMain.ShowBsjRecvCountEx _ShowBsjRecvCountEx;
		private frmMain.ShowFTNetCountEx _FTSendCount;
		private frmMain.ShowFTNetCountEx _FTRecvCount;
		private SqlConnection m_sqlConnect;
		private bool _blnTrans = false;
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDB44Code = new System.Windows.Forms.TextBox();
            this.btnDisconnGPS = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnConnectGPS = new System.Windows.Forms.Button();
            this.txtGpsPass = new System.Windows.Forms.TextBox();
            this.txtGpsUser = new System.Windows.Forms.TextBox();
            this.txtGpsPort = new System.Windows.Forms.TextBox();
            this.txtGpsServer = new System.Windows.Forms.TextBox();
            this.lblBSJCount = new System.Windows.Forms.Label();
            this.lblBsjStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnFTDisConnect = new System.Windows.Forms.Button();
            this.btnFTConnect = new System.Windows.Forms.Button();
            this.txtFTPass = new System.Windows.Forms.TextBox();
            this.txtFTUser = new System.Windows.Forms.TextBox();
            this.txtFTPort = new System.Windows.Forms.TextBox();
            this.txtFTServer = new System.Windows.Forms.TextBox();
            this.lblFTRecvCount = new System.Windows.Forms.Label();
            this.lblFTSendCount = new System.Windows.Forms.Label();
            this.lblFTStatus = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnLoadLocal = new System.Windows.Forms.Button();
            this.btnSaveDB = new System.Windows.Forms.Button();
            this.btnLoadVehicle = new System.Windows.Forms.Button();
            this.btnSQLConnect = new System.Windows.Forms.Button();
            this.txtSQLPass = new System.Windows.Forms.TextBox();
            this.txtSQLUser = new System.Windows.Forms.TextBox();
            this.txtDBname = new System.Windows.Forms.TextBox();
            this.txtSQLPort = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtSQLServer = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtDisplay = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnSaveConfig);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(773, 64);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(365, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(95, 33);
            this.button3.TabIndex = 33;
            this.button3.Text = "上传所有车辆";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(97, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 33);
            this.button2.TabIndex = 32;
            this.button2.Text = "停止转发";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 33);
            this.button1.TabIndex = 32;
            this.button1.Text = "开始转发";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(228, 19);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(87, 33);
            this.btnSaveConfig.TabIndex = 31;
            this.btnSaveConfig.Text = "保存当前设置";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_click);
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(166, 99);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Size = new System.Drawing.Size(180, 21);
            this.txtCompany.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(98, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 33;
            this.label5.Text = "公司名称：";
            this.label5.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(3, 73);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(777, 298);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.txtDB44Code);
            this.tabPage1.Controls.Add(this.txtCompany);
            this.tabPage1.Controls.Add(this.btnDisconnGPS);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.btnConnectGPS);
            this.tabPage1.Controls.Add(this.txtGpsPass);
            this.tabPage1.Controls.Add(this.txtGpsUser);
            this.tabPage1.Controls.Add(this.txtGpsPort);
            this.tabPage1.Controls.Add(this.txtGpsServer);
            this.tabPage1.Controls.Add(this.lblBSJCount);
            this.tabPage1.Controls.Add(this.lblBsjStatus);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(769, 272);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "GPS服务器";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(352, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 35;
            this.label7.Text = "十进制表示";
            // 
            // txtDB44Code
            // 
            this.txtDB44Code.Location = new System.Drawing.Point(166, 134);
            this.txtDB44Code.Name = "txtDB44Code";
            this.txtDB44Code.Size = new System.Drawing.Size(180, 21);
            this.txtDB44Code.TabIndex = 34;
            this.txtDB44Code.Text = "5642";
            // 
            // btnDisconnGPS
            // 
            this.btnDisconnGPS.Location = new System.Drawing.Point(605, 204);
            this.btnDisconnGPS.Name = "btnDisconnGPS";
            this.btnDisconnGPS.Size = new System.Drawing.Size(98, 33);
            this.btnDisconnGPS.TabIndex = 2;
            this.btnDisconnGPS.Text = "断开";
            this.btnDisconnGPS.UseVisualStyleBackColor = true;
            this.btnDisconnGPS.Click += new System.EventHandler(this.btnDisconnGPS_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(85, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 33;
            this.label6.Text = "DB44授权码：";
            // 
            // btnConnectGPS
            // 
            this.btnConnectGPS.Location = new System.Drawing.Point(475, 204);
            this.btnConnectGPS.Name = "btnConnectGPS";
            this.btnConnectGPS.Size = new System.Drawing.Size(98, 33);
            this.btnConnectGPS.TabIndex = 2;
            this.btnConnectGPS.Text = "连接";
            this.btnConnectGPS.UseVisualStyleBackColor = true;
            this.btnConnectGPS.Click += new System.EventHandler(this.btnConnectGPS_Click);
            // 
            // txtGpsPass
            // 
            this.txtGpsPass.Location = new System.Drawing.Point(475, 57);
            this.txtGpsPass.Name = "txtGpsPass";
            this.txtGpsPass.Size = new System.Drawing.Size(180, 21);
            this.txtGpsPass.TabIndex = 1;
            // 
            // txtGpsUser
            // 
            this.txtGpsUser.Location = new System.Drawing.Point(475, 29);
            this.txtGpsUser.Name = "txtGpsUser";
            this.txtGpsUser.Size = new System.Drawing.Size(180, 21);
            this.txtGpsUser.TabIndex = 1;
            // 
            // txtGpsPort
            // 
            this.txtGpsPort.Location = new System.Drawing.Point(166, 55);
            this.txtGpsPort.Name = "txtGpsPort";
            this.txtGpsPort.Size = new System.Drawing.Size(180, 21);
            this.txtGpsPort.TabIndex = 1;
            // 
            // txtGpsServer
            // 
            this.txtGpsServer.Location = new System.Drawing.Point(166, 27);
            this.txtGpsServer.Name = "txtGpsServer";
            this.txtGpsServer.Size = new System.Drawing.Size(180, 21);
            this.txtGpsServer.TabIndex = 1;
            // 
            // lblBSJCount
            // 
            this.lblBSJCount.AutoSize = true;
            this.lblBSJCount.Location = new System.Drawing.Point(164, 243);
            this.lblBSJCount.Name = "lblBSJCount";
            this.lblBSJCount.Size = new System.Drawing.Size(11, 12);
            this.lblBSJCount.TabIndex = 0;
            this.lblBSJCount.Text = "0";
            // 
            // lblBsjStatus
            // 
            this.lblBsjStatus.AutoSize = true;
            this.lblBsjStatus.Location = new System.Drawing.Point(164, 209);
            this.lblBsjStatus.Name = "lblBsjStatus";
            this.lblBsjStatus.Size = new System.Drawing.Size(29, 12);
            this.lblBsjStatus.TabIndex = 0;
            this.lblBsjStatus.Text = "断开";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 243);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "接收GPS数据包：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(395, 62);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 12);
            this.label16.TabIndex = 0;
            this.label16.Text = "登录密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(97, 209);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "网络状态：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(395, 34);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "登录帐号：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "服务器端口：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(86, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器地址：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnFTDisConnect);
            this.tabPage2.Controls.Add(this.btnFTConnect);
            this.tabPage2.Controls.Add(this.txtFTPass);
            this.tabPage2.Controls.Add(this.txtFTUser);
            this.tabPage2.Controls.Add(this.txtFTPort);
            this.tabPage2.Controls.Add(this.txtFTServer);
            this.tabPage2.Controls.Add(this.lblFTRecvCount);
            this.tabPage2.Controls.Add(this.lblFTSendCount);
            this.tabPage2.Controls.Add(this.lblFTStatus);
            this.tabPage2.Controls.Add(this.label17);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(769, 272);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "交委服务器";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnFTDisConnect
            // 
            this.btnFTDisConnect.Location = new System.Drawing.Point(604, 199);
            this.btnFTDisConnect.Name = "btnFTDisConnect";
            this.btnFTDisConnect.Size = new System.Drawing.Size(98, 51);
            this.btnFTDisConnect.TabIndex = 15;
            this.btnFTDisConnect.Text = "断开";
            this.btnFTDisConnect.UseVisualStyleBackColor = true;
            this.btnFTDisConnect.Click += new System.EventHandler(this.btnFTDisConnect_Click);
            // 
            // btnFTConnect
            // 
            this.btnFTConnect.Location = new System.Drawing.Point(474, 199);
            this.btnFTConnect.Name = "btnFTConnect";
            this.btnFTConnect.Size = new System.Drawing.Size(98, 51);
            this.btnFTConnect.TabIndex = 16;
            this.btnFTConnect.Text = "连接";
            this.btnFTConnect.UseVisualStyleBackColor = true;
            this.btnFTConnect.Click += new System.EventHandler(this.btnFTConnect_Click);
            // 
            // txtFTPass
            // 
            this.txtFTPass.Location = new System.Drawing.Point(474, 52);
            this.txtFTPass.Name = "txtFTPass";
            this.txtFTPass.Size = new System.Drawing.Size(180, 21);
            this.txtFTPass.TabIndex = 11;
            // 
            // txtFTUser
            // 
            this.txtFTUser.Location = new System.Drawing.Point(474, 24);
            this.txtFTUser.Name = "txtFTUser";
            this.txtFTUser.Size = new System.Drawing.Size(180, 21);
            this.txtFTUser.TabIndex = 12;
            // 
            // txtFTPort
            // 
            this.txtFTPort.Location = new System.Drawing.Point(165, 50);
            this.txtFTPort.Name = "txtFTPort";
            this.txtFTPort.Size = new System.Drawing.Size(180, 21);
            this.txtFTPort.TabIndex = 14;
            // 
            // txtFTServer
            // 
            this.txtFTServer.Location = new System.Drawing.Point(165, 22);
            this.txtFTServer.Name = "txtFTServer";
            this.txtFTServer.Size = new System.Drawing.Size(180, 21);
            this.txtFTServer.TabIndex = 13;
            // 
            // lblFTRecvCount
            // 
            this.lblFTRecvCount.AutoSize = true;
            this.lblFTRecvCount.Location = new System.Drawing.Point(164, 227);
            this.lblFTRecvCount.Name = "lblFTRecvCount";
            this.lblFTRecvCount.Size = new System.Drawing.Size(11, 12);
            this.lblFTRecvCount.TabIndex = 10;
            this.lblFTRecvCount.Text = "0";
            // 
            // lblFTSendCount
            // 
            this.lblFTSendCount.AutoSize = true;
            this.lblFTSendCount.Location = new System.Drawing.Point(164, 248);
            this.lblFTSendCount.Name = "lblFTSendCount";
            this.lblFTSendCount.Size = new System.Drawing.Size(11, 12);
            this.lblFTSendCount.TabIndex = 10;
            this.lblFTSendCount.Text = "0";
            // 
            // lblFTStatus
            // 
            this.lblFTStatus.AutoSize = true;
            this.lblFTStatus.Location = new System.Drawing.Point(163, 204);
            this.lblFTStatus.Name = "lblFTStatus";
            this.lblFTStatus.Size = new System.Drawing.Size(29, 12);
            this.lblFTStatus.TabIndex = 5;
            this.lblFTStatus.Text = "断开";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(84, 227);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(77, 12);
            this.label17.TabIndex = 4;
            this.label17.Text = "接收指令包：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(84, 248);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 4;
            this.label9.Text = "发送指令包：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(394, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 3;
            this.label10.Text = "登录密码：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(96, 204);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 8;
            this.label11.Text = "网络状态：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(394, 29);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 7;
            this.label12.Text = "登录帐号：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(85, 55);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 12);
            this.label13.TabIndex = 6;
            this.label13.Text = "服务器端口：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(85, 27);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 9;
            this.label14.Text = "服务器地址：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView1);
            this.tabPage3.Controls.Add(this.btnSelect);
            this.tabPage3.Controls.Add(this.btnLoadLocal);
            this.tabPage3.Controls.Add(this.btnSaveDB);
            this.tabPage3.Controls.Add(this.btnLoadVehicle);
            this.tabPage3.Controls.Add(this.btnSQLConnect);
            this.tabPage3.Controls.Add(this.txtSQLPass);
            this.tabPage3.Controls.Add(this.txtSQLUser);
            this.tabPage3.Controls.Add(this.txtDBname);
            this.tabPage3.Controls.Add(this.txtSQLPort);
            this.tabPage3.Controls.Add(this.label19);
            this.tabPage3.Controls.Add(this.txtSQLServer);
            this.tabPage3.Controls.Add(this.label24);
            this.tabPage3.Controls.Add(this.label26);
            this.tabPage3.Controls.Add(this.label27);
            this.tabPage3.Controls.Add(this.label28);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(769, 272);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "数据库服务器";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dataGridView1.Location = new System.Drawing.Point(228, 14);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowCellToolTips = false;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(530, 256);
            this.dataGridView1.TabIndex = 33;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "1";
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.Width = 30;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(121, 231);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(91, 29);
            this.btnSelect.TabIndex = 31;
            this.btnSelect.Text = "反选";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnLoadLocal
            // 
            this.btnLoadLocal.Location = new System.Drawing.Point(121, 196);
            this.btnLoadLocal.Name = "btnLoadLocal";
            this.btnLoadLocal.Size = new System.Drawing.Size(91, 29);
            this.btnLoadLocal.TabIndex = 31;
            this.btnLoadLocal.Text = "本地加载";
            this.btnLoadLocal.UseVisualStyleBackColor = true;
            this.btnLoadLocal.Visible = false;
            this.btnLoadLocal.Click += new System.EventHandler(this.btnLoadLocal_Click);
            // 
            // btnSaveDB
            // 
            this.btnSaveDB.Location = new System.Drawing.Point(11, 197);
            this.btnSaveDB.Name = "btnSaveDB";
            this.btnSaveDB.Size = new System.Drawing.Size(91, 29);
            this.btnSaveDB.TabIndex = 32;
            this.btnSaveDB.Text = "保存";
            this.btnSaveDB.UseVisualStyleBackColor = true;
            this.btnSaveDB.Visible = false;
            this.btnSaveDB.Click += new System.EventHandler(this.btnSaveDB_Click);
            // 
            // btnLoadVehicle
            // 
            this.btnLoadVehicle.Location = new System.Drawing.Point(121, 161);
            this.btnLoadVehicle.Name = "btnLoadVehicle";
            this.btnLoadVehicle.Size = new System.Drawing.Size(91, 29);
            this.btnLoadVehicle.TabIndex = 31;
            this.btnLoadVehicle.Text = "数据库加载";
            this.btnLoadVehicle.UseVisualStyleBackColor = true;
            this.btnLoadVehicle.Click += new System.EventHandler(this.btnLoadVehicle_Click);
            // 
            // btnSQLConnect
            // 
            this.btnSQLConnect.Location = new System.Drawing.Point(11, 162);
            this.btnSQLConnect.Name = "btnSQLConnect";
            this.btnSQLConnect.Size = new System.Drawing.Size(91, 29);
            this.btnSQLConnect.TabIndex = 32;
            this.btnSQLConnect.Text = "连接";
            this.btnSQLConnect.UseVisualStyleBackColor = true;
            this.btnSQLConnect.Click += new System.EventHandler(this.btnSQLConnect_Click);
            // 
            // txtSQLPass
            // 
            this.txtSQLPass.Location = new System.Drawing.Point(92, 134);
            this.txtSQLPass.Name = "txtSQLPass";
            this.txtSQLPass.Size = new System.Drawing.Size(130, 21);
            this.txtSQLPass.TabIndex = 27;
            // 
            // txtSQLUser
            // 
            this.txtSQLUser.Location = new System.Drawing.Point(92, 107);
            this.txtSQLUser.Name = "txtSQLUser";
            this.txtSQLUser.Size = new System.Drawing.Size(130, 21);
            this.txtSQLUser.TabIndex = 27;
            // 
            // txtDBname
            // 
            this.txtDBname.Location = new System.Drawing.Point(92, 79);
            this.txtDBname.Name = "txtDBname";
            this.txtDBname.Size = new System.Drawing.Size(130, 21);
            this.txtDBname.TabIndex = 28;
            // 
            // txtSQLPort
            // 
            this.txtSQLPort.Location = new System.Drawing.Point(92, 52);
            this.txtSQLPort.Name = "txtSQLPort";
            this.txtSQLPort.Size = new System.Drawing.Size(130, 21);
            this.txtSQLPort.TabIndex = 30;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(21, 137);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(65, 12);
            this.label19.TabIndex = 17;
            this.label19.Text = "登录密码：";
            // 
            // txtSQLServer
            // 
            this.txtSQLServer.Location = new System.Drawing.Point(92, 24);
            this.txtSQLServer.Name = "txtSQLServer";
            this.txtSQLServer.Size = new System.Drawing.Size(130, 21);
            this.txtSQLServer.TabIndex = 29;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(21, 110);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(65, 12);
            this.label24.TabIndex = 17;
            this.label24.Text = "登录密码：";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(12, 82);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(77, 12);
            this.label26.TabIndex = 22;
            this.label26.Text = "数据库名称：";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(12, 57);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(77, 12);
            this.label27.TabIndex = 21;
            this.label27.Text = "服务器端口：";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(12, 29);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(77, 12);
            this.label28.TabIndex = 24;
            this.label28.Text = "服务器地址：";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtDisplay);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(769, 272);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "转发状态";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtDisplay
            // 
            this.txtDisplay.Location = new System.Drawing.Point(3, 6);
            this.txtDisplay.Multiline = true;
            this.txtDisplay.Name = "txtDisplay";
            this.txtDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDisplay.Size = new System.Drawing.Size(763, 261);
            this.txtDisplay.TabIndex = 0;
            // 
            // frmMain
            // 
            this.ClientSize = new System.Drawing.Size(783, 379);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "深圳交委通用转发(传统、DB44、国标)";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

		}
		public frmMain()
		{
			this.InitializeComponent();
			this._PrintStatusEx = new frmMain.PrintStatusEx(this.PrintStatus);
			this._OnBsjClientStatusChangeEx = new frmMain.OnBsjClientStatusChangeEx(this.OnBsjClientStatusChange);
			this._ShowBsjRecvCountEx = new frmMain.ShowBsjRecvCountEx(this.ShowBsjRecvCount);
			this.m_BsjClient = new CBsjClient();
			this.m_BsjClient.ShowMessage += new CBsjClient.ShowMessageEx(this.PrintStatus);
			this.m_BsjClient.OnClientStatusChange += new CBsjClient.OnClientStatusChangeEx(this.OnBsjClientStatusChange);
			this.m_BsjClient.OnDataPacketArrivals += new CBsjClient.OnDataPacketArrivalsEx(this.OnBsjGpsData);
			this.m_FeiTanClient = new CFeiTanClient();
			this.m_FeiTanClient.ShowMessage += new CFeiTanClient.ShowMessageEx(this.PrintStatus);
			this.m_FeiTanClient.OnClientStatusChange += new CFeiTanClient.OnClientStatusChangeEx(this.OnFeiTanClientStatusChange);
			this.m_FeiTanClient.OnDataPacketArrivals += new CFeiTanClient.OnDataPacketArrivalsEx(this.OnFeiTanPacket);
			this.m_FeiTanClient.SendNetCount += new CFeiTanClient.NetCountEx(this.ShowFTSendCount);
			this.m_FeiTanClient.RecvNetCount += new CFeiTanClient.NetCountEx(this.ShowFTRecvCount);
			this._FTSendCount = new frmMain.ShowFTNetCountEx(this.ShowFTSendCount);
			this._FTRecvCount = new frmMain.ShowFTNetCountEx(this.ShowFTRecvCount);
		}
		private void btnConnectGPS_Click(object sender, EventArgs e)
		{
			try
			{
				this.m_BsjClient.Close();
				this.m_BsjClient.InitClient(this.txtGpsServer.Text.Trim(), int.Parse(this.txtGpsPort.Text), this.txtGpsUser.Text.Trim(), this.txtGpsPass.Text);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void dbgPrint(Exception ex)
		{
			try
			{
				this.PrintStatus(ex.ToString());
			}
			catch (Exception value)
			{
				Debug.Write(value);
			}
		}
		private void PrintStatus(string strStatus)
		{
			try
			{
				if (base.InvokeRequired)
				{
					base.Invoke(this._PrintStatusEx, new object[]
					{
						strStatus
					});
				}
				else
				{
					if (this.txtDisplay.TextLength > 20480)
					{
						this.txtDisplay.Clear();
					}
					this.txtDisplay.AppendText(strStatus + "\r\n\r\n");
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void OnBsjGpsData(byte[] gpsData)
		{
			try
			{
				this.ShowBsjRecvCount(this.m_BsjClient.PacketCount);
				if (gpsData[2] == 150)
				{
					this.LoadVehInfoFormDataBase(this.m_sqlConnect);
					this.button3_Click(null, null);
				}
				else
				{
					if (this._blnTrans)
					{
						int iPAddress = CBsjProtocol.GetIPAddress(ref gpsData);
						string b = CBsjProtocol.IPToString(iPAddress);
						for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
						{
							if (this.dataGridView1.Rows[i].Cells[3].Value.ToString().Trim() == b)
							{
								string text = "14411{0}{1}";
								string arg = this.dataGridView1.Rows[i].Cells[1].Value.ToString();
								text = string.Format(text, Settings.Default.FTUser, arg);
								SendGPSDataPacket gpsData2 = default(SendGPSDataPacket);
								gpsData2.DeviceNo = text;
								gpsData2.gpsData = gpsData;
								this.m_FeiTanClient.SendData(gpsData2);
								break;
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
		private void OnBsjClientStatusChange(bool blnConnect)
		{
			try
			{
				if (base.InvokeRequired)
				{
					base.Invoke(this._OnBsjClientStatusChangeEx, new object[]
					{
						blnConnect
					});
				}
				else
				{
					this.lblBsjStatus.Text = (blnConnect ? "连接" : "断开");
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void OnFeiTanClientStatusChange(bool blnConnect)
		{
			try
			{
				if (base.InvokeRequired)
				{
					base.Invoke(new frmMain.OnBsjClientStatusChangeEx(this.OnFeiTanClientStatusChange), new object[]
					{
						blnConnect
					});
				}
				else
				{
					this.lblFTStatus.Text = (blnConnect ? "连接" : "断开");
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void OnFeiTanPacket(string cmdPack)
		{
			try
			{
				string[] array = cmdPack.Split(new char[]
				{
					','
				});
				string text = array[0];
				if (text != null)
				{
					if (text == "1" || text == "4")
					{
						this.PrintStatus("服务器请求上传车辆：[" + cmdPack + "]");
						string[] array2 = array[1].Split(new char[]
						{
							','
						});
						List<FeiTanVehInfo> list = new List<FeiTanVehInfo>();
						string[] array3 = cmdPack.Split(",".ToCharArray());
						for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
						{
							FeiTanVehInfo item = new FeiTanVehInfo(false);
							string text2 = "14411{0}{1}";
							string arg = this.dataGridView1.Rows[i].Cells[1].Value.ToString();
							text2 = string.Format(text2, Settings.Default.FTUser, arg);
							if (array3[1] == text2)
							{
								item.DeviceNo = text2;
								item.VehName = this.dataGridView1.Rows[i].Cells[4].Value.ToString();
								item.SuportCamare = "0";
								item.VehTypeNo = this.dataGridView1.Rows[i].Cells[19].Value.ToString();
								item.VehPhoneNo = this.dataGridView1.Rows[i].Cells[2].Value.ToString();
								item.VehOwnerTel = this.dataGridView1.Rows[i].Cells[2].Value.ToString();
								item.VehOwenrContactTel1 = this.dataGridView1.Rows[i].Cells[2].Value.ToString();
								item.VehOwnerName = this.dataGridView1.Rows[i].Cells[6].Value.ToString();
								item.ServerStartTime = "2011-04-20 12:00:00";
								item.ServerEndTime = "2013-04-20 12:00:00";
								item.VehOwenrAddress = "深圳市";
								item.VehOwenrContactName1 = item.VehOwnerName;
								item.VehOwenrContactName2 = item.VehOwnerName;
								item.VehOwenrContactTel1 = item.VehOwnerTel;
								item.VehOwenrContactTel2 = item.VehOwnerTel;
								item.VehOwenrPostCode = "530031";
								item.VehOwnerEmail = "2258773@163.com";
								item.VehOwnerID = "452525197010011254";
								item.VehOwnerSex = "男";
								item.VehOwnerWorkUnits = this.dataGridView1.Rows[i].Cells[24].Value.ToString();
								item.DB44Info = this.txtDB44Code.Text.Trim();
								list.Add(item);
								this.m_FeiTanClient.SendData(CFeiTianCommand.MakeVehicleInfo(list));
								list.RemoveAt(0);
								this.PrintStatus("上传车辆资料：[" + cmdPack + " ]  " + item.ToString());
								break;
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
		private void ShowBsjRecvCount(long lngCount)
		{
			try
			{
				if (base.InvokeRequired)
				{
					base.Invoke(this._ShowBsjRecvCountEx, new object[]
					{
						lngCount
					});
				}
				else
				{
					this.lblBSJCount.Text = lngCount.ToString();
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void btnDisconnGPS_Click(object sender, EventArgs e)
		{
			try
			{
				this.m_BsjClient.Close();
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void btnFTConnect_Click(object sender, EventArgs e)
		{
			try
			{
				this.m_FeiTanClient.InitConnect(this.txtFTServer.Text.Trim(), int.Parse(this.txtFTPort.Text), this.txtFTUser.Text.Trim(), this.txtFTPass.Text);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void btnFTDisConnect_Click(object sender, EventArgs e)
		{
			try
			{
				this.m_FeiTanClient.Close();
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void btnSQLConnect_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.m_sqlConnect != null)
				{
					this.m_sqlConnect.Dispose();
					this.m_sqlConnect = null;
				}
				this.m_sqlConnect = new SqlConnection();
				string connectionString = string.Concat(new string[]
				{
					"Server = ",
					this.txtSQLServer.Text.Trim(),
					",",
					this.txtSQLPort.Text.Trim(),
					"; Database =",
					this.txtDBname.Text.Trim(),
					"; User ID = ",
					this.txtSQLUser.Text.Trim(),
					"; Password=",
					this.txtSQLPass.Text,
					"; Integrated Security=false;Connect Timeout = 5"
				});
				this.m_sqlConnect.ConnectionString = connectionString;
				this.m_sqlConnect.Open();
				if (this.m_sqlConnect.State == ConnectionState.Open)
				{
					Settings.Default.SQLServer = this.txtSQLServer.Text.Trim();
					Settings.Default.SQLPOrt = this.txtSQLPort.Text.Trim();
					Settings.Default.SQLDBName = this.txtDBname.Text.Trim();
					Settings.Default.SQLUser = this.txtSQLUser.Text.Trim();
					Settings.Default.SQLPass = this.txtSQLPass.Text.Trim();
					Settings.Default.Save();
					MessageBox.Show("连接数据库成功！");
				}
				else
				{
					MessageBox.Show("连接数据库失败！");
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void btnLoadVehicle_Click(object sender, EventArgs e)
		{
			try
			{
				this.LoadVehInfoFormDataBase(this.m_sqlConnect);
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private bool LoadVehInfoFormDataBase(SqlConnection cn)
		{
			bool flag = false;
			bool result;
			try
			{
				string cmdText = " declare @UserID int  set @UserID=(select UserID from UserMain where UserName='" + Settings.Default.BsjUser + "')  drop table #GroupIDList  create table #GroupIDList(theid int)  insert into #GroupIDList(theid) exec GetAllVehGroupByUserID @UserID  select vehicle.*  from vehicle left join vehicledetail on vehicle.id=vehicledetail.vehid left join vehgroupmain vg on vehicledetail.vehgroupid=vg.vehgroupid where vg.vehgroupid in (select theid from #GroupIDList)";
				using (SqlCommand sqlCommand = new SqlCommand(cmdText, cn))
				{
					sqlCommand.CommandType = CommandType.Text;
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
					{
						using (DataSet dataSet = new DataSet())
						{
							sqlDataAdapter.Fill(dataSet);
							this.dataGridView1.DataSource = dataSet.Tables[0];
							flag = true;
						}
					}
				}
				result = flag;
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
				result = false;
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
			return result;
		}
		private void frmMain_Load(object sender, EventArgs e)
		{
			try
			{
				this.txtGpsServer.Text = Settings.Default.BsjServer;
				this.txtGpsPort.Text = Settings.Default.BsjPort;
				this.txtGpsUser.Text = Settings.Default.BsjUser;
				this.txtGpsPass.Text = Settings.Default.BsjPass;
				this.txtFTServer.Text = Settings.Default.FTServer;
				this.txtFTPort.Text = Settings.Default.FTPort;
				this.txtFTUser.Text = Settings.Default.FTUser;
				this.txtFTPass.Text = Settings.Default.FTPass;
				this.txtSQLServer.Text = Settings.Default.SQLServer;
				this.txtSQLPort.Text = Settings.Default.SQLPOrt;
				this.txtDBname.Text = Settings.Default.SQLDBName;
				this.txtSQLUser.Text = Settings.Default.SQLUser;
				this.txtSQLPass.Text = Settings.Default.SQLPass;
				this.txtCompany.Text = Settings.Default.Company;
				this.txtDB44Code.Text = Settings.Default.DB44Code;
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void btnSaveConfig_click(object sender, EventArgs e)
		{
			try
			{
				Settings.Default.BsjServer = this.txtGpsServer.Text.Trim();
				Settings.Default.BsjPort = this.txtGpsPort.Text.Trim();
				Settings.Default.BsjUser = this.txtGpsUser.Text.Trim();
				Settings.Default.BsjPass = this.txtGpsPass.Text.Trim();
				Settings.Default.FTServer = this.txtFTServer.Text.Trim();
				Settings.Default.FTPort = this.txtFTPort.Text.Trim();
				Settings.Default.FTUser = this.txtFTUser.Text.Trim();
				Settings.Default.FTPass = this.txtFTPass.Text.Trim();
				Settings.Default.SQLServer = this.txtSQLServer.Text.Trim();
				Settings.Default.SQLPOrt = this.txtSQLPort.Text.Trim();
				Settings.Default.SQLDBName = this.txtDBname.Text.Trim();
				Settings.Default.SQLUser = this.txtSQLUser.Text.Trim();
				Settings.Default.SQLPass = this.txtSQLPass.Text.Trim();
				Settings.Default.Company = this.txtCompany.Text.Trim();
				Settings.Default.DB44Code = this.txtDB44Code.Text.Trim();
				Settings.Default.Save();
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
				MessageBox.Show("保存失败！\r\n" + ex.Message);
			}
		}
		private void btnSaveDB_Click(object sender, EventArgs e)
		{
			try
			{
				string path = Application.StartupPath + "\\dbConfig.dat";
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				using (FileStream fileStream = new FileStream(path, FileMode.Append))
				{
					string[] array = new string[this.dataGridView1.Columns.Count];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = this.dataGridView1.Columns[i].HeaderText;
					}
					string str = string.Join("\t", array);
					byte[] bytes = Encoding.GetEncoding("gb2312").GetBytes(str + "\r");
					fileStream.Write(bytes, 0, bytes.Length);
					for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
					{
						if ((bool)this.dataGridView1.Rows[i].Cells[0].Value)
						{
							for (int j = 0; j < array.Length; j++)
							{
								array[j] = this.dataGridView1.Rows[i].Cells[j].Value.ToString();
							}
							str = string.Join("\t", array);
							bytes = Encoding.GetEncoding("gb2312").GetBytes(str + "\r");
							fileStream.Write(bytes, 0, bytes.Length);
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void btnLoadLocal_Click(object sender, EventArgs e)
		{
			try
			{
				string path = Application.StartupPath + "\\dbConfig.dat";
				this.dataGridView1.DataSource = null;
				this.dataGridView1.Rows.Clear();
				this.dataGridView1.Columns.Clear();
				if (File.Exists(path))
				{
					using (FileStream fileStream = new FileStream(path, FileMode.Open))
					{
						byte[] array = new byte[fileStream.Length];
						fileStream.Read(array, 0, array.Length);
						string @string = Encoding.GetEncoding("gb2312").GetString(array);
						string[] array2 = @string.Split(new char[]
						{
							'\r'
						});
						string[] array3 = array2[0].Split(new char[]
						{
							'\t'
						});
						for (int i = 0; i < array3.Length; i++)
						{
							if (i == 0)
							{
								DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
								dataGridViewCheckBoxColumn.HeaderText = array3[i];
								this.dataGridView1.Columns.Add(dataGridViewCheckBoxColumn);
							}
							else
							{
								this.dataGridView1.Columns.Add("Name_" + array3[i], array3[i]);
							}
						}
						for (int i = 1; i < array2.Length; i++)
						{
							if (array2[i].Length > 10)
							{
								array3 = array2[i].Split(new char[]
								{
									'\t'
								});
								this.dataGridView1.Rows.Add(array3);
							}
						}
					}
				}
				else
				{
					MessageBox.Show("本地没有数据！");
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void btnSelect_Click(object sender, EventArgs e)
		{
			try
			{
				for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
				{
					if (this.dataGridView1.Rows[i].Cells[0].Value == null)
					{
						this.dataGridView1.Rows[i].Cells[0].Value = true;
					}
					else
					{
						bool flag = (bool)this.dataGridView1.Rows[i].Cells[0].Value;
						this.dataGridView1.Rows[i].Cells[0].Value = !flag;
					}
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void ShowFTSendCount(int nCount)
		{
			try
			{
				if (base.InvokeRequired)
				{
					base.Invoke(this._FTSendCount, new object[]
					{
						nCount
					});
				}
				else
				{
					this.lblFTSendCount.Text = nCount.ToString();
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void ShowFTRecvCount(int nCount)
		{
			try
			{
				if (base.InvokeRequired)
				{
					base.Invoke(this._FTRecvCount, new object[]
					{
						nCount
					});
				}
				else
				{
					this.lblFTRecvCount.Text = nCount.ToString();
				}
			}
			catch (Exception ex)
			{
				this.dbgPrint(ex);
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			this._blnTrans = true;
			this.button1.Enabled = false;
			this.button2.Enabled = true;
		}
		private void button2_Click(object sender, EventArgs e)
		{
			this._blnTrans = false;
			this.button1.Enabled = true;
			this.button2.Enabled = false;
		}
		private void button3_Click(object sender, EventArgs e)
		{
			List<FeiTanVehInfo> list = new List<FeiTanVehInfo>();
			for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
			{
				FeiTanVehInfo item = new FeiTanVehInfo(false);
				string text = "14411{0}{1}";
				string arg = this.dataGridView1.Rows[i].Cells[1].Value.ToString();
				text = string.Format(text, Settings.Default.FTUser, arg);
				item.DeviceNo = text;
				item.VehName = this.dataGridView1.Rows[i].Cells[4].Value.ToString();
				item.SuportCamare = "0";
				item.VehTypeNo = this.dataGridView1.Rows[i].Cells[19].Value.ToString();
				item.VehPhoneNo = this.dataGridView1.Rows[i].Cells[2].Value.ToString();
				item.VehOwnerTel = this.dataGridView1.Rows[i].Cells[2].Value.ToString();
				item.VehOwenrContactTel1 = this.dataGridView1.Rows[i].Cells[2].Value.ToString();
				item.VehOwnerName = this.dataGridView1.Rows[i].Cells[6].Value.ToString();
				item.ServerStartTime = "2011-04-20 12:00:00";
				item.ServerEndTime = "2013-04-20 12:00:00";
				item.VehOwenrAddress = "深圳市";
				item.VehOwenrContactName1 = item.VehOwnerName;
				item.VehOwenrContactName2 = item.VehOwnerName;
				item.VehOwenrContactTel1 = item.VehOwnerTel;
				item.VehOwenrContactTel2 = item.VehOwnerTel;
				item.VehOwenrPostCode = "530031";
				item.VehOwnerEmail = "2258773@163.com";
				item.VehOwnerID = "452525197010011254";
				item.VehOwnerSex = "男";
				item.VehOwnerWorkUnits = this.dataGridView1.Rows[i].Cells[24].Value.ToString();
				item.DB44Info = this.txtDB44Code.Text.Trim();
				list.Add(item);
				this.m_FeiTanClient.SendData(CFeiTianCommand.MakeVehicleInfo(list));
				list.RemoveAt(0);
				this.PrintStatus("上传车辆资料：[" + item.VehName + " ]  " + item.ToString());
			}
		}
	}
}
