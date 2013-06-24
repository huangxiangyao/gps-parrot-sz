using System.Windows.Forms;
namespace Parrot
{
    public partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.SaveButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Label20 = new System.Windows.Forms.Label();
            this.OperatorPassword = new System.Windows.Forms.TextBox();
            this.OperatorName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.DbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DbPassword = new System.Windows.Forms.TextBox();
            this.DbUsername = new System.Windows.Forms.TextBox();
            this.NetId = new System.Windows.Forms.TextBox();
            this.DbServer = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.OldSmppIP = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SmppIP = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.UseOldSmpp = new System.Windows.Forms.CheckBox();
            this.UseNewSmpp = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.JtjLocalPort = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.JtjClientId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.JtjServerIp = new System.Windows.Forms.TextBox();
            this.JtjUsername = new System.Windows.Forms.TextBox();
            this.JtjPassword = new System.Windows.Forms.TextBox();
            this.JtjListeningPort = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label31 = new System.Windows.Forms.Label();
            this.CenterId_253 = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.Db44Password_253 = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.ManufacturerCode_253 = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.Key_253 = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.IC1_253 = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.M1_253 = new System.Windows.Forms.TextBox();
            this.IA1_253 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label21 = new System.Windows.Forms.Label();
            this.CenterId_252 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.Db44Password_252 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.ManufacturerCode_252 = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.Key_252 = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.IC1_252 = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.M1_252 = new System.Windows.Forms.TextBox();
            this.IA1_252 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.CenterId_251 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.Db44Password_251 = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.ManufacturerCode_251 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.Key_251 = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.IC1_251 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.M1_251 = new System.Windows.Forms.TextBox();
            this.IA1_251 = new System.Windows.Forms.TextBox();
            this.DbUseWindowsAuthenticationMode = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.SaveButton, "SaveButton");
            this.SaveButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.UseVisualStyleBackColor = false;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DbUseWindowsAuthenticationMode);
            this.tabPage1.Controls.Add(this.Label20);
            this.tabPage1.Controls.Add(this.OperatorPassword);
            this.tabPage1.Controls.Add(this.OperatorName);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.label24);
            this.tabPage1.Controls.Add(this.DbName);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.DbPassword);
            this.tabPage1.Controls.Add(this.DbUsername);
            this.tabPage1.Controls.Add(this.NetId);
            this.tabPage1.Controls.Add(this.DbServer);
            this.tabPage1.Controls.Add(this.label25);
            this.tabPage1.Controls.Add(this.label26);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Label20
            // 
            this.Label20.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.Label20, "Label20");
            this.Label20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label20.Name = "Label20";
            // 
            // OperatorPassword
            // 
            this.OperatorPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OperatorPassword.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.OperatorPassword, "OperatorPassword");
            this.OperatorPassword.Name = "OperatorPassword";
            // 
            // OperatorName
            // 
            this.OperatorName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OperatorName.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.OperatorName, "OperatorName");
            this.OperatorName.Name = "OperatorName";
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label13, "label13");
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Name = "label13";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label10, "label10");
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Name = "label10";
            // 
            // label24
            // 
            this.label24.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label24, "label24");
            this.label24.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label24.Name = "label24";
            // 
            // DbName
            // 
            this.DbName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DbName.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.DbName, "DbName");
            this.DbName.Name = "DbName";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Name = "label1";
            // 
            // DbPassword
            // 
            this.DbPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DbPassword.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.DbPassword, "DbPassword");
            this.DbPassword.Name = "DbPassword";
            // 
            // DbUsername
            // 
            this.DbUsername.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DbUsername.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.DbUsername, "DbUsername");
            this.DbUsername.Name = "DbUsername";
            // 
            // NetId
            // 
            this.NetId.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NetId.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.NetId, "NetId");
            this.NetId.Name = "NetId";
            // 
            // DbServer
            // 
            this.DbServer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DbServer.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.DbServer, "DbServer");
            this.DbServer.Name = "DbServer";
            // 
            // label25
            // 
            this.label25.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label25, "label25");
            this.label25.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label25.Name = "label25";
            // 
            // label26
            // 
            this.label26.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label26, "label26");
            this.label26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label26.Name = "label26";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.OldSmppIP);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.SmppIP);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.UseOldSmpp);
            this.tabPage4.Controls.Add(this.UseNewSmpp);
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Name = "label2";
            // 
            // OldSmppIP
            // 
            this.OldSmppIP.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.OldSmppIP, "OldSmppIP");
            this.OldSmppIP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.OldSmppIP.Name = "OldSmppIP";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Name = "label5";
            // 
            // SmppIP
            // 
            this.SmppIP.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SmppIP.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.SmppIP, "SmppIP");
            this.SmppIP.Name = "SmppIP";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label7, "label7");
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Name = "label7";
            // 
            // UseOldSmpp
            // 
            resources.ApplyResources(this.UseOldSmpp, "UseOldSmpp");
            this.UseOldSmpp.Name = "UseOldSmpp";
            this.UseOldSmpp.UseVisualStyleBackColor = true;
            this.UseOldSmpp.CheckedChanged += new System.EventHandler(this.UseOldSmpp_CheckedChanged);
            // 
            // UseNewSmpp
            // 
            resources.ApplyResources(this.UseNewSmpp, "UseNewSmpp");
            this.UseNewSmpp.Checked = true;
            this.UseNewSmpp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseNewSmpp.Name = "UseNewSmpp";
            this.UseNewSmpp.UseVisualStyleBackColor = true;
            this.UseNewSmpp.CheckedChanged += new System.EventHandler(this.UseNewSmpp_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.JtjLocalPort);
            this.tabPage2.Controls.Add(this.checkBox1);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.JtjClientId);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.JtjServerIp);
            this.tabPage2.Controls.Add(this.JtjUsername);
            this.tabPage2.Controls.Add(this.JtjPassword);
            this.tabPage2.Controls.Add(this.JtjListeningPort);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label11, "label11");
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Name = "label11";
            // 
            // JtjLocalPort
            // 
            this.JtjLocalPort.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.JtjLocalPort, "JtjLocalPort");
            this.JtjLocalPort.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JtjLocalPort.Name = "JtjLocalPort";
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Name = "label3";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Name = "label4";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label6, "label6");
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Name = "label6";
            // 
            // JtjClientId
            // 
            this.JtjClientId.Cursor = System.Windows.Forms.Cursors.Hand;
            this.JtjClientId.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.JtjClientId, "JtjClientId");
            this.JtjClientId.Name = "JtjClientId";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label8, "label8");
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Name = "label8";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label9, "label9");
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Name = "label9";
            // 
            // JtjServerIp
            // 
            this.JtjServerIp.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.JtjServerIp, "JtjServerIp");
            this.JtjServerIp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JtjServerIp.Name = "JtjServerIp";
            // 
            // JtjUsername
            // 
            this.JtjUsername.Cursor = System.Windows.Forms.Cursors.Hand;
            this.JtjUsername.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.JtjUsername, "JtjUsername");
            this.JtjUsername.Name = "JtjUsername";
            // 
            // JtjPassword
            // 
            this.JtjPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.JtjPassword.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.JtjPassword, "JtjPassword");
            this.JtjPassword.Name = "JtjPassword";
            // 
            // JtjListeningPort
            // 
            this.JtjListeningPort.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.JtjListeningPort, "JtjListeningPort");
            this.JtjListeningPort.ForeColor = System.Drawing.SystemColors.ControlText;
            this.JtjListeningPort.Name = "JtjListeningPort";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox3);
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label31);
            this.groupBox3.Controls.Add(this.CenterId_253);
            this.groupBox3.Controls.Add(this.label32);
            this.groupBox3.Controls.Add(this.Db44Password_253);
            this.groupBox3.Controls.Add(this.label33);
            this.groupBox3.Controls.Add(this.ManufacturerCode_253);
            this.groupBox3.Controls.Add(this.label34);
            this.groupBox3.Controls.Add(this.Key_253);
            this.groupBox3.Controls.Add(this.label35);
            this.groupBox3.Controls.Add(this.IC1_253);
            this.groupBox3.Controls.Add(this.label36);
            this.groupBox3.Controls.Add(this.label37);
            this.groupBox3.Controls.Add(this.M1_253);
            this.groupBox3.Controls.Add(this.IA1_253);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label31
            // 
            this.label31.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label31, "label31");
            this.label31.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label31.Name = "label31";
            // 
            // CenterId_253
            // 
            this.CenterId_253.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CenterId_253.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.CenterId_253, "CenterId_253");
            this.CenterId_253.Name = "CenterId_253";
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label32, "label32");
            this.label32.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label32.Name = "label32";
            // 
            // Db44Password_253
            // 
            this.Db44Password_253.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Db44Password_253.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.Db44Password_253, "Db44Password_253");
            this.Db44Password_253.Name = "Db44Password_253";
            // 
            // label33
            // 
            this.label33.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label33, "label33");
            this.label33.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label33.Name = "label33";
            // 
            // ManufacturerCode_253
            // 
            this.ManufacturerCode_253.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ManufacturerCode_253.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.ManufacturerCode_253, "ManufacturerCode_253");
            this.ManufacturerCode_253.Name = "ManufacturerCode_253";
            // 
            // label34
            // 
            this.label34.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label34, "label34");
            this.label34.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label34.Name = "label34";
            // 
            // Key_253
            // 
            this.Key_253.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Key_253.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.Key_253, "Key_253");
            this.Key_253.Name = "Key_253";
            // 
            // label35
            // 
            this.label35.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label35, "label35");
            this.label35.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label35.Name = "label35";
            // 
            // IC1_253
            // 
            this.IC1_253.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IC1_253.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.IC1_253, "IC1_253");
            this.IC1_253.Name = "IC1_253";
            // 
            // label36
            // 
            this.label36.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label36, "label36");
            this.label36.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label36.Name = "label36";
            // 
            // label37
            // 
            this.label37.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label37, "label37");
            this.label37.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label37.Name = "label37";
            // 
            // M1_253
            // 
            this.M1_253.Cursor = System.Windows.Forms.Cursors.Hand;
            this.M1_253.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.M1_253, "M1_253");
            this.M1_253.Name = "M1_253";
            // 
            // IA1_253
            // 
            this.IA1_253.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IA1_253.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.IA1_253, "IA1_253");
            this.IA1_253.Name = "IA1_253";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.CenterId_252);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.Db44Password_252);
            this.groupBox2.Controls.Add(this.label23);
            this.groupBox2.Controls.Add(this.ManufacturerCode_252);
            this.groupBox2.Controls.Add(this.label27);
            this.groupBox2.Controls.Add(this.Key_252);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.IC1_252);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.label30);
            this.groupBox2.Controls.Add(this.M1_252);
            this.groupBox2.Controls.Add(this.IA1_252);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label21, "label21");
            this.label21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label21.Name = "label21";
            // 
            // CenterId_252
            // 
            this.CenterId_252.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CenterId_252.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.CenterId_252, "CenterId_252");
            this.CenterId_252.Name = "CenterId_252";
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label22, "label22");
            this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label22.Name = "label22";
            // 
            // Db44Password_252
            // 
            this.Db44Password_252.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Db44Password_252.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.Db44Password_252, "Db44Password_252");
            this.Db44Password_252.Name = "Db44Password_252";
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label23, "label23");
            this.label23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label23.Name = "label23";
            // 
            // ManufacturerCode_252
            // 
            this.ManufacturerCode_252.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ManufacturerCode_252.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.ManufacturerCode_252, "ManufacturerCode_252");
            this.ManufacturerCode_252.Name = "ManufacturerCode_252";
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label27, "label27");
            this.label27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label27.Name = "label27";
            // 
            // Key_252
            // 
            this.Key_252.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Key_252.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.Key_252, "Key_252");
            this.Key_252.Name = "Key_252";
            // 
            // label28
            // 
            this.label28.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label28, "label28");
            this.label28.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label28.Name = "label28";
            // 
            // IC1_252
            // 
            this.IC1_252.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IC1_252.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.IC1_252, "IC1_252");
            this.IC1_252.Name = "IC1_252";
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label29, "label29");
            this.label29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label29.Name = "label29";
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label30, "label30");
            this.label30.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label30.Name = "label30";
            // 
            // M1_252
            // 
            this.M1_252.Cursor = System.Windows.Forms.Cursors.Hand;
            this.M1_252.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.M1_252, "M1_252");
            this.M1_252.Name = "M1_252";
            // 
            // IA1_252
            // 
            this.IA1_252.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IA1_252.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.IA1_252, "IA1_252");
            this.IA1_252.Name = "IA1_252";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.CenterId_251);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.Db44Password_251);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.ManufacturerCode_251);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.Key_251);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.IC1_251);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.M1_251);
            this.groupBox1.Controls.Add(this.IA1_251);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label17, "label17");
            this.label17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label17.Name = "label17";
            // 
            // CenterId_251
            // 
            this.CenterId_251.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CenterId_251.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.CenterId_251, "CenterId_251");
            this.CenterId_251.Name = "CenterId_251";
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label18, "label18");
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Name = "label18";
            // 
            // Db44Password_251
            // 
            this.Db44Password_251.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Db44Password_251.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.Db44Password_251, "Db44Password_251");
            this.Db44Password_251.Name = "Db44Password_251";
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label19, "label19");
            this.label19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label19.Name = "label19";
            // 
            // ManufacturerCode_251
            // 
            this.ManufacturerCode_251.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ManufacturerCode_251.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.ManufacturerCode_251, "ManufacturerCode_251");
            this.ManufacturerCode_251.Name = "ManufacturerCode_251";
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label16, "label16");
            this.label16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label16.Name = "label16";
            // 
            // Key_251
            // 
            this.Key_251.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Key_251.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.Key_251, "Key_251");
            this.Key_251.Name = "Key_251";
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label15, "label15");
            this.label15.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label15.Name = "label15";
            // 
            // IC1_251
            // 
            this.IC1_251.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IC1_251.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.IC1_251, "IC1_251");
            this.IC1_251.Name = "IC1_251";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label12, "label12");
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Name = "label12";
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.label14, "label14");
            this.label14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label14.Name = "label14";
            // 
            // M1_251
            // 
            this.M1_251.Cursor = System.Windows.Forms.Cursors.Hand;
            this.M1_251.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.M1_251, "M1_251");
            this.M1_251.Name = "M1_251";
            // 
            // IA1_251
            // 
            this.IA1_251.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IA1_251.ForeColor = System.Drawing.SystemColors.ControlText;
            resources.ApplyResources(this.IA1_251, "IA1_251");
            this.IA1_251.Name = "IA1_251";
            // 
            // DbUseWindowsAuthenticationMode
            // 
            resources.ApplyResources(this.DbUseWindowsAuthenticationMode, "DbUseWindowsAuthenticationMode");
            this.DbUseWindowsAuthenticationMode.Name = "DbUseWindowsAuthenticationMode";
            this.DbUseWindowsAuthenticationMode.UseVisualStyleBackColor = true;
            this.DbUseWindowsAuthenticationMode.CheckedChanged += new System.EventHandler(this.DbUseWindowsAuthenticationMode_CheckedChanged);
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.SaveButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private Button SaveButton;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Label label10;
        private Label label1;
        private Label label24;
        private TextBox NetId;
        private Label label25;
        private Label label26;
        private TextBox DbServer;
        private TextBox DbUsername;
        private TextBox DbPassword;
        private TextBox DbName;
        private TabPage tabPage4;
        private TabPage tabPage2;
        private CheckBox checkBox1;
        private Label label3;
        private Label label4;
        private Label label6;
        private TextBox JtjClientId;
        private Label label8;
        private Label label9;
        private TextBox JtjServerIp;
        private TextBox JtjUsername;
        private TextBox JtjPassword;
        private TextBox JtjListeningPort;
        private Label label11;
        private TextBox JtjLocalPort;
        private Label Label20;
        private TextBox OperatorPassword;
        private TextBox OperatorName;
        private Label label13;
        private Label label2;
        private TextBox OldSmppIP;
        private Label label5;
        private TextBox SmppIP;
        private Label label7;
        private CheckBox UseOldSmpp;
        private CheckBox UseNewSmpp;
        private TabPage tabPage3;
        private GroupBox groupBox3;
        private Label label31;
        private TextBox CenterId_253;
        private Label label32;
        private TextBox Db44Password_253;
        private Label label33;
        private TextBox ManufacturerCode_253;
        private Label label34;
        private TextBox Key_253;
        private Label label35;
        private TextBox IC1_253;
        private Label label36;
        private Label label37;
        private TextBox M1_253;
        private TextBox IA1_253;
        private GroupBox groupBox2;
        private Label label21;
        private TextBox CenterId_252;
        private Label label22;
        private TextBox Db44Password_252;
        private Label label23;
        private TextBox ManufacturerCode_252;
        private Label label27;
        private TextBox Key_252;
        private Label label28;
        private TextBox IC1_252;
        private Label label29;
        private Label label30;
        private TextBox M1_252;
        private TextBox IA1_252;
        private GroupBox groupBox1;
        private Label label17;
        private TextBox CenterId_251;
        private Label label18;
        private TextBox Db44Password_251;
        private Label label19;
        private TextBox ManufacturerCode_251;
        private Label label16;
        private TextBox Key_251;
        private Label label15;
        private TextBox IC1_251;
        private Label label12;
        private Label label14;
        private TextBox M1_251;
        private TextBox IA1_251;
        private CheckBox DbUseWindowsAuthenticationMode;
    }
}
