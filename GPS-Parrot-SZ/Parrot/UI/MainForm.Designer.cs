using System.Resources;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.ComponentModel;
namespace Parrot
{
    public partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.EverySecondTimer = new System.Windows.Forms.Timer(this.components);
            this.InfoDateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.InfoColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.InfoIdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OmcCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MdtType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ManufacturerCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VehiclePlateNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VehiclePlateColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VehiclePlateType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ToTerminalListView = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.InfoListView = new System.Windows.Forms.ListView();
            this.button6 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.MdtListView = new System.Windows.Forms.ListView();
            this.CommuStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MdtCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VehicleType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SupervisorName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SupervisorOrgCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VehiclePurpose = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.VehicleQualificationNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MdtListViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.FocusThisMdtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.JtjListView = new System.Windows.Forms.ListView();
            this.JtjPlateNumberColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.JtjPlateColorColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UploadPduColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DeliveredDateTimeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DownloadPduColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ReceivedDateTimeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.LastReceivedImagePictureBox = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.DebugListView = new System.Windows.Forms.ListView();
            this.DebugIdColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DebugDateColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DebugColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DebugListViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ClearDebugListViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.DebugMobileID = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.DebugDetailsTextBox = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SettingsButton = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.CountOfReceivingFromJtj = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.TxErrorLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SpeedOfSendingToJtj = new System.Windows.Forms.Label();
            this.CountOfSendingToJtj = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SpeedOfReceivingFromMdt = new System.Windows.Forms.Label();
            this.CountOfReceivingFromMdt = new System.Windows.Forms.Label();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ToTerminalListView.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.MdtListViewContextMenuStrip.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LastReceivedImagePictureBox)).BeginInit();
            this.panel5.SuspendLayout();
            this.DebugListViewContextMenuStrip.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // EverySecondTimer
            // 
            this.EverySecondTimer.Interval = 1000;
            this.EverySecondTimer.Tick += new System.EventHandler(this.EverySecondTimer_Tick);
            // 
            // InfoDateColumnHeader
            // 
            resources.ApplyResources(this.InfoDateColumnHeader, "InfoDateColumnHeader");
            // 
            // InfoColumnHeader
            // 
            resources.ApplyResources(this.InfoColumnHeader, "InfoColumnHeader");
            // 
            // InfoIdColumnHeader
            // 
            resources.ApplyResources(this.InfoIdColumnHeader, "InfoIdColumnHeader");
            // 
            // OmcCode
            // 
            resources.ApplyResources(this.OmcCode, "OmcCode");
            // 
            // MdtType
            // 
            resources.ApplyResources(this.MdtType, "MdtType");
            // 
            // ManufacturerCode
            // 
            resources.ApplyResources(this.ManufacturerCode, "ManufacturerCode");
            // 
            // VehiclePlateNumber
            // 
            resources.ApplyResources(this.VehiclePlateNumber, "VehiclePlateNumber");
            // 
            // VehiclePlateColor
            // 
            resources.ApplyResources(this.VehiclePlateColor, "VehiclePlateColor");
            // 
            // VehiclePlateType
            // 
            resources.ApplyResources(this.VehiclePlateType, "VehiclePlateType");
            // 
            // ToTerminalListView
            // 
            this.ToTerminalListView.Controls.Add(this.tabPage1);
            this.ToTerminalListView.Controls.Add(this.tabPage2);
            this.ToTerminalListView.Controls.Add(this.tabPage3);
            this.ToTerminalListView.Controls.Add(this.tabPage4);
            resources.ApplyResources(this.ToTerminalListView, "ToTerminalListView");
            this.ToTerminalListView.Name = "ToTerminalListView";
            this.ToTerminalListView.SelectedIndex = 0;
            this.ToTerminalListView.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.InfoListView);
            this.tabPage1.Controls.Add(this.button6);
            this.tabPage1.Controls.Add(this.button2);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Tag = "0";
            // 
            // InfoListView
            // 
            this.InfoListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.InfoListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoListView.CausesValidation = false;
            this.InfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.InfoIdColumnHeader,
            this.InfoDateColumnHeader,
            this.InfoColumnHeader});
            resources.ApplyResources(this.InfoListView, "InfoListView");
            this.InfoListView.ForeColor = System.Drawing.Color.Navy;
            this.InfoListView.FullRowSelect = true;
            this.InfoListView.GridLines = true;
            this.InfoListView.MultiSelect = false;
            this.InfoListView.Name = "InfoListView";
            this.InfoListView.UseCompatibleStateImageBehavior = false;
            this.InfoListView.View = System.Windows.Forms.View.Details;
            // 
            // button6
            // 
            resources.ApplyResources(this.button6, "button6");
            this.button6.Name = "button6";
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.MdtListView);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Tag = "1";
            // 
            // MdtListView
            // 
            this.MdtListView.AllowColumnReorder = true;
            this.MdtListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.MdtListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CommuStatus,
            this.MdtCode,
            this.MdtType,
            this.VehicleType,
            this.VehiclePlateNumber,
            this.VehiclePlateColor,
            this.VehiclePlateType,
            this.SupervisorName,
            this.SupervisorOrgCode,
            this.OmcCode,
            this.ManufacturerCode,
            this.VehiclePurpose,
            this.VehicleQualificationNumber});
            this.MdtListView.ContextMenuStrip = this.MdtListViewContextMenuStrip;
            resources.ApplyResources(this.MdtListView, "MdtListView");
            this.MdtListView.ForeColor = System.Drawing.Color.Navy;
            this.MdtListView.FullRowSelect = true;
            this.MdtListView.GridLines = true;
            this.MdtListView.Name = "MdtListView";
            this.MdtListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.MdtListView.TabStop = false;
            this.MdtListView.UseCompatibleStateImageBehavior = false;
            this.MdtListView.View = System.Windows.Forms.View.Details;
            this.MdtListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.MdtListView_ColumnClick);
            // 
            // CommuStatus
            // 
            resources.ApplyResources(this.CommuStatus, "CommuStatus");
            // 
            // MdtCode
            // 
            resources.ApplyResources(this.MdtCode, "MdtCode");
            // 
            // VehicleType
            // 
            resources.ApplyResources(this.VehicleType, "VehicleType");
            // 
            // SupervisorName
            // 
            resources.ApplyResources(this.SupervisorName, "SupervisorName");
            // 
            // SupervisorOrgCode
            // 
            resources.ApplyResources(this.SupervisorOrgCode, "SupervisorOrgCode");
            // 
            // VehiclePurpose
            // 
            resources.ApplyResources(this.VehiclePurpose, "VehiclePurpose");
            // 
            // VehicleQualificationNumber
            // 
            resources.ApplyResources(this.VehicleQualificationNumber, "VehicleQualificationNumber");
            // 
            // MdtListViewContextMenuStrip
            // 
            this.MdtListViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FocusThisMdtToolStripMenuItem});
            this.MdtListViewContextMenuStrip.Name = "MdtListViewContextMenuStrip";
            resources.ApplyResources(this.MdtListViewContextMenuStrip, "MdtListViewContextMenuStrip");
            // 
            // FocusThisMdtToolStripMenuItem
            // 
            this.FocusThisMdtToolStripMenuItem.Name = "FocusThisMdtToolStripMenuItem";
            resources.ApplyResources(this.FocusThisMdtToolStripMenuItem, "FocusThisMdtToolStripMenuItem");
            this.FocusThisMdtToolStripMenuItem.Click += new System.EventHandler(this.FocusThisMdtToolStripMenuItem_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.JtjListView);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Tag = "2";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // JtjListView
            // 
            this.JtjListView.AllowColumnReorder = true;
            this.JtjListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.JtjListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.JtjPlateNumberColumn,
            this.JtjPlateColorColumn,
            this.UploadPduColumn,
            this.DeliveredDateTimeColumn,
            this.DownloadPduColumn,
            this.ReceivedDateTimeColumn});
            this.JtjListView.ContextMenuStrip = this.MdtListViewContextMenuStrip;
            resources.ApplyResources(this.JtjListView, "JtjListView");
            this.JtjListView.ForeColor = System.Drawing.Color.Navy;
            this.JtjListView.FullRowSelect = true;
            this.JtjListView.GridLines = true;
            this.JtjListView.Name = "JtjListView";
            this.JtjListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.JtjListView.TabStop = false;
            this.JtjListView.UseCompatibleStateImageBehavior = false;
            this.JtjListView.View = System.Windows.Forms.View.Details;
            this.JtjListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.JtjListView_ColumnClick);
            // 
            // JtjPlateNumberColumn
            // 
            resources.ApplyResources(this.JtjPlateNumberColumn, "JtjPlateNumberColumn");
            // 
            // JtjPlateColorColumn
            // 
            resources.ApplyResources(this.JtjPlateColorColumn, "JtjPlateColorColumn");
            // 
            // UploadPduColumn
            // 
            resources.ApplyResources(this.UploadPduColumn, "UploadPduColumn");
            // 
            // DeliveredDateTimeColumn
            // 
            resources.ApplyResources(this.DeliveredDateTimeColumn, "DeliveredDateTimeColumn");
            // 
            // DownloadPduColumn
            // 
            resources.ApplyResources(this.DownloadPduColumn, "DownloadPduColumn");
            // 
            // ReceivedDateTimeColumn
            // 
            resources.ApplyResources(this.ReceivedDateTimeColumn, "ReceivedDateTimeColumn");
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.panel4);
            this.tabPage4.Controls.Add(this.panel3);
            this.tabPage4.Controls.Add(this.label28);
            resources.ApplyResources(this.tabPage4, "tabPage4");
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Tag = "4";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panel5);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.LastReceivedImagePictureBox);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // LastReceivedImagePictureBox
            // 
            this.LastReceivedImagePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.LastReceivedImagePictureBox, "LastReceivedImagePictureBox");
            this.LastReceivedImagePictureBox.Image = global::Parrot.Properties.Resources.black;
            this.LastReceivedImagePictureBox.Name = "LastReceivedImagePictureBox";
            this.LastReceivedImagePictureBox.TabStop = false;
            // 
            // panel5
            // 
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.DebugListView);
            this.panel5.Name = "panel5";
            // 
            // DebugListView
            // 
            this.DebugListView.BackColor = System.Drawing.Color.WhiteSmoke;
            this.DebugListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DebugListView.CausesValidation = false;
            this.DebugListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DebugIdColumnHeader,
            this.DebugDateColumnHeader,
            this.DebugColumnHeader});
            this.DebugListView.ContextMenuStrip = this.DebugListViewContextMenuStrip;
            resources.ApplyResources(this.DebugListView, "DebugListView");
            this.DebugListView.ForeColor = System.Drawing.Color.Navy;
            this.DebugListView.FullRowSelect = true;
            this.DebugListView.GridLines = true;
            this.DebugListView.MultiSelect = false;
            this.DebugListView.Name = "DebugListView";
            this.DebugListView.UseCompatibleStateImageBehavior = false;
            this.DebugListView.View = System.Windows.Forms.View.Details;
            this.DebugListView.DoubleClick += new System.EventHandler(this.DebugInfoListView_DoubleClick);
            // 
            // DebugIdColumnHeader
            // 
            resources.ApplyResources(this.DebugIdColumnHeader, "DebugIdColumnHeader");
            // 
            // DebugDateColumnHeader
            // 
            resources.ApplyResources(this.DebugDateColumnHeader, "DebugDateColumnHeader");
            // 
            // DebugColumnHeader
            // 
            resources.ApplyResources(this.DebugColumnHeader, "DebugColumnHeader");
            // 
            // DebugListViewContextMenuStrip
            // 
            this.DebugListViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ClearDebugListViewToolStripMenuItem});
            this.DebugListViewContextMenuStrip.Name = "contextMenuStripForDebugListView";
            resources.ApplyResources(this.DebugListViewContextMenuStrip, "DebugListViewContextMenuStrip");
            // 
            // ClearDebugListViewToolStripMenuItem
            // 
            this.ClearDebugListViewToolStripMenuItem.Name = "ClearDebugListViewToolStripMenuItem";
            resources.ApplyResources(this.ClearDebugListViewToolStripMenuItem, "ClearDebugListViewToolStripMenuItem");
            this.ClearDebugListViewToolStripMenuItem.Click += new System.EventHandler(this.ClearDebugListViewToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.DebugMobileID);
            this.panel3.Controls.Add(this.label17);
            this.panel3.Controls.Add(this.DebugDetailsTextBox);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // DebugMobileID
            // 
            resources.ApplyResources(this.DebugMobileID, "DebugMobileID");
            this.DebugMobileID.Name = "DebugMobileID";
            this.DebugMobileID.TextChanged += new System.EventHandler(this.DebugMobileID_TextChanged);
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // DebugDetailsTextBox
            // 
            resources.ApplyResources(this.DebugDetailsTextBox, "DebugDetailsTextBox");
            this.DebugDetailsTextBox.Name = "DebugDetailsTextBox";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SettingsButton);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.CountOfReceivingFromJtj);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.TxErrorLabel);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.SpeedOfSendingToJtj);
            this.panel1.Controls.Add(this.CountOfSendingToJtj);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.SpeedOfReceivingFromMdt);
            this.panel1.Controls.Add(this.CountOfReceivingFromMdt);
            this.panel1.Controls.Add(this.MessageLabel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // SettingsButton
            // 
            resources.ApplyResources(this.SettingsButton, "SettingsButton");
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.UseVisualStyleBackColor = true;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Name = "label12";
            // 
            // CountOfReceivingFromJtj
            // 
            resources.ApplyResources(this.CountOfReceivingFromJtj, "CountOfReceivingFromJtj");
            this.CountOfReceivingFromJtj.BackColor = System.Drawing.Color.Transparent;
            this.CountOfReceivingFromJtj.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CountOfReceivingFromJtj.ForeColor = System.Drawing.Color.Black;
            this.CountOfReceivingFromJtj.Name = "CountOfReceivingFromJtj";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Name = "label10";
            // 
            // TxErrorLabel
            // 
            resources.ApplyResources(this.TxErrorLabel, "TxErrorLabel");
            this.TxErrorLabel.BackColor = System.Drawing.Color.Transparent;
            this.TxErrorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TxErrorLabel.ForeColor = System.Drawing.Color.Black;
            this.TxErrorLabel.Name = "TxErrorLabel";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Name = "label7";
            // 
            // SpeedOfSendingToJtj
            // 
            resources.ApplyResources(this.SpeedOfSendingToJtj, "SpeedOfSendingToJtj");
            this.SpeedOfSendingToJtj.BackColor = System.Drawing.Color.Transparent;
            this.SpeedOfSendingToJtj.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SpeedOfSendingToJtj.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SpeedOfSendingToJtj.ForeColor = System.Drawing.Color.Black;
            this.SpeedOfSendingToJtj.Name = "SpeedOfSendingToJtj";
            // 
            // CountOfSendingToJtj
            // 
            resources.ApplyResources(this.CountOfSendingToJtj, "CountOfSendingToJtj");
            this.CountOfSendingToJtj.BackColor = System.Drawing.Color.Transparent;
            this.CountOfSendingToJtj.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CountOfSendingToJtj.ForeColor = System.Drawing.Color.Black;
            this.CountOfSendingToJtj.Name = "CountOfSendingToJtj";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Name = "label6";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // SpeedOfReceivingFromMdt
            // 
            resources.ApplyResources(this.SpeedOfReceivingFromMdt, "SpeedOfReceivingFromMdt");
            this.SpeedOfReceivingFromMdt.BackColor = System.Drawing.Color.Transparent;
            this.SpeedOfReceivingFromMdt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SpeedOfReceivingFromMdt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SpeedOfReceivingFromMdt.ForeColor = System.Drawing.Color.Black;
            this.SpeedOfReceivingFromMdt.Name = "SpeedOfReceivingFromMdt";
            // 
            // CountOfReceivingFromMdt
            // 
            resources.ApplyResources(this.CountOfReceivingFromMdt, "CountOfReceivingFromMdt");
            this.CountOfReceivingFromMdt.BackColor = System.Drawing.Color.Transparent;
            this.CountOfReceivingFromMdt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CountOfReceivingFromMdt.ForeColor = System.Drawing.Color.Black;
            this.CountOfReceivingFromMdt.Name = "CountOfReceivingFromMdt";
            // 
            // MessageLabel
            // 
            resources.ApplyResources(this.MessageLabel, "MessageLabel");
            this.MessageLabel.BackColor = System.Drawing.Color.Transparent;
            this.MessageLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MessageLabel.ForeColor = System.Drawing.Color.Black;
            this.MessageLabel.Name = "MessageLabel";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ToTerminalListView);
            this.panel2.Controls.Add(this.panel1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.panel2);
            this.Name = "MainForm";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ToTerminalListView.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.MdtListViewContextMenuStrip.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LastReceivedImagePictureBox)).EndInit();
            this.panel5.ResumeLayout(false);
            this.DebugListViewContextMenuStrip.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Button SettingsButton;
        private Button button2;
        private Button button6;
        private ColumnHeader ManufacturerCode;
        private ColumnHeader OmcCode;
        private ColumnHeader MdtType;
        private ColumnHeader SupervisorOrgCode;
        private ColumnHeader VehicleQualificationNumber;
        private ColumnHeader SupervisorName;
        private ColumnHeader VehiclePlateNumber;
        private ColumnHeader VehiclePlateType;
        private ColumnHeader VehiclePlateColor;
        private ColumnHeader VehicleType;
        private ColumnHeader VehiclePurpose;
        private ColumnHeader InfoIdColumnHeader;
        private ColumnHeader DebugIdColumnHeader;
        private ColumnHeader DebugDateColumnHeader;
        private ColumnHeader DebugColumnHeader;
        private ColumnHeader InfoDateColumnHeader;
        private ColumnHeader InfoColumnHeader;
        private ColumnHeader MdtCode;
        private ColumnHeader CommuStatus;
        private Label MessageLabel;
        private Label label10;
        private Label TxErrorLabel;
        private Label label12;
        private Label CountOfReceivingFromJtj;
        private Label label14;
        private Label label17;
        private Label label2;
        private Label label28;
        private Label label3;
        private Label CountOfReceivingFromMdt;
        private Label SpeedOfReceivingFromMdt;
        private Label label6;
        private Label label7;
        private Label SpeedOfSendingToJtj;
        private Label CountOfSendingToJtj;
        private ListView DebugListView;
        private ListView InfoListView;
        private ListView MdtListView;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private PictureBox LastReceivedImagePictureBox;
        private TabControl ToTerminalListView;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage4;
        private TextBox DebugMobileID;
        private TextBox DebugDetailsTextBox;
        private System.Windows.Forms.Timer EverySecondTimer;
        private ContextMenuStrip DebugListViewContextMenuStrip;
        private ToolStripMenuItem ClearDebugListViewToolStripMenuItem;
        private ContextMenuStrip MdtListViewContextMenuStrip;
        private ToolStripMenuItem FocusThisMdtToolStripMenuItem;
        private TabPage tabPage3;
        private ListView JtjListView;
        private ColumnHeader JtjPlateNumberColumn;
        private ColumnHeader JtjPlateColorColumn;
        private ColumnHeader DeliveredDateTimeColumn;
        private ColumnHeader DownloadPduColumn;
        private ColumnHeader UploadPduColumn;
        private ColumnHeader ReceivedDateTimeColumn;
    }
}

