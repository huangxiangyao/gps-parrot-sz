using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Parrot;
using Parrot.Models;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Net;
using Parrot.Models.Db44;

namespace Parrot
{
    public partial class MainForm : Form
    {
        private DatabaseProcessor DatabaseProcessor;
        private SmppAgent SmppAgent;

        /// <summary>
        /// 当前系统消息序号。
        /// </summary>
        private int CurrentInfoIndex = 1;
        /// <summary>
        /// 当前“上报交通局的消息”的序号。
        /// </summary>
        private int CurrentDeliveredToJtjIndex = 1;
        /// <summary>
        /// 当前“来自交通局的指令”的序号。
        /// </summary>
        private int CurrentReceivedFromJtjIndex = 1;
        /// <summary>
        /// 当前调试信息序号。
        /// </summary>
        private int CurrentDebugListViewIndex = 1;
        /// <summary>
        /// 最近收到的由GPS终端抓拍上传的监控图像。
        /// </summary>
        private byte[] LastReceivedImage;
        /// <summary>
        /// 最近收到的由GPS终端抓拍上传的监控图像的数据包序号。
        /// </summary>
        private int LastReceivedImageIndex = 0;

        /// <summary>
        /// 检查在列表中是否存在指定ID。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ID"></param>
        /// <returns>-1, if not found</returns>
        private int FindMdtListViewItem(ListView list, string ID)
        {
            for (int i = 0; i < list.Items.Count; i++)
            {
                if (list.Items[i].Text == ID)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 检查在列表中是否存在指定车牌号码的条目。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ID"></param>
        /// <returns>-1, if not found</returns>
        private int FindListViewItem(ListView list, string plateNumber)
        {
            for (int i = 0; i < list.Items.Count; i++)
            {
                if (list.Items[i].Text == plateNumber)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 更新“测试信息”窗口。
        /// </summary>
        /// <param name="s"></param>
        private void AddDebugListViewItem(string s)
        {
            ListView view;
            Monitor.Enter(view = this.DebugListView);
            try
            {
                if (this.DebugListView.Items.Count > 1000)
                {
                    this.DebugListView.Items.Clear();
                    this.CurrentDebugListViewIndex = 0;
                }

                ListViewItem item = new ListViewItem();
                item.SubItems[0].Text = this.CurrentDebugListViewIndex.ToString("000");
                item.SubItems.Add(DateTime.Now.ToString());
                item.SubItems.Add(s);
                this.DebugListView.Items.Add(item);

                this.CurrentDebugListViewIndex++;
            }
            catch (Exception ex)
            {
                Program.Log.Error(ex);
            }
            finally
            {
                Monitor.Exit(view);
            }
        }

        /// <summary>
        /// 更新“程序日志”窗口。
        /// </summary>
        /// <param name="s"></param>
        private void AddInfoListViewItem(string s)
        {
            ListView view;
            Monitor.Enter(view = this.InfoListView);
            try
            {
                if (view.Items.Count > 1000)
                {
                    this.CurrentInfoIndex = 0;
                    view.Items.Clear();
                }

                //this.label1.Text = DateTime.Now.ToString() + "  " + message;

                ListViewItem item = new ListViewItem();
                item.SubItems[0].Text = this.CurrentInfoIndex.ToString("000");
                item.SubItems.Add(DateTime.Now.ToString());
                item.SubItems.Add(s);
                view.Items.Add(item);

                //自动滚动到最新一行
                item.EnsureVisible();

                this.CurrentInfoIndex++;
            }
            catch (Exception ex)
            {
                Program.Log.Error(ex);
            }
            finally
            {
                Monitor.Exit(view);
            }
        }

        public MainForm()
        {
            this.InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FireLoggingEvent(Level.Info, "启动主程序。");

            try
            {
                if (Properties.Settings.Default.DbUseWindowsAuthenticationMode)
                {
                    ConnectionStringManager.Default.SetProperties(Properties.Settings.Default.DbServer,
                        Properties.Settings.Default.DbName);
                }
                else
                {
                    ConnectionStringManager.Default.SetProperties(Properties.Settings.Default.DbServer,
                        Properties.Settings.Default.DbName,
                        Properties.Settings.Default.DbUsername,
                        Properties.Settings.Default.DbPassword);
                }
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, "主程序启动时发生异常：未配置数据库。");
                FireLoggingEvent(Level.Advanced, ex);
                return;
            }
            try
            {
                if (ParrotModelWrapper.GetAllUsers().Count == 0)
                {
                    FireLoggingEvent(Level.Info, "数据库缺失或配置不正确，请联系系统管理员。");
                    return;
                }
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, "主程序启动时发生异常：数据库配置不正确。详情请查阅系统日志。");
                FireLoggingEvent(Level.Advanced, ex);
                return;
            }
            try
            {
                this.DatabaseProcessor = new DatabaseProcessor(
                    Properties.Settings.Default.OperatorName,
                    Properties.Settings.Default.OperatorPassword);
                this.DatabaseProcessor.Logging += new LoggingEventHandler(OnLogging);
                this.DatabaseProcessor.MdtListUpdated += new MdtListUpdatedEventHandler(DatabaseProcessor_MdtListUpdated);
                this.DatabaseProcessor.Start();
            }
            catch (Exception ex)
            {
                FireLoggingEvent(Level.Info, "主程序启动时发生异常：操作员配置错误，或数据库异常，导致无法获取GPS终端列表。");
                FireLoggingEvent(Level.Advanced, ex);
                return;
            }

            try
            {
                JtjClientAccount JtjClientAccount = new JtjClientAccount(
                    IPAddress.Parse(Properties.Settings.Default.JtjServerIp),
                    Properties.Settings.Default.JtjListeningPort,
                    Properties.Settings.Default.JtjClientId,
                    Properties.Settings.Default.JtjUsername,
                    Properties.Settings.Default.JtjPassword,
                    Properties.Settings.Default.JtjLocalPort);

                Db44EncryptionFactor factor251 = new Db44EncryptionFactor(
                    Properties.Settings.Default.IA1_251,
                    Properties.Settings.Default.IC1_251,
                    Properties.Settings.Default.M1_251,
                    Properties.Settings.Default.KEY_251);
                Db44EncryptionFactorRepository.Default.AddOrUpdate(251, factor251);
                Db44EncryptionFactor factor252 = new Db44EncryptionFactor(
                    Properties.Settings.Default.IA1_252,
                    Properties.Settings.Default.IC1_252,
                    Properties.Settings.Default.M1_252,
                    Properties.Settings.Default.KEY_252);
                Db44EncryptionFactorRepository.Default.AddOrUpdate(252, factor252);
                Db44EncryptionFactor factor253 = new Db44EncryptionFactor(
                    Properties.Settings.Default.IA1_253,
                    Properties.Settings.Default.IC1_253,
                    Properties.Settings.Default.M1_253,
                    Properties.Settings.Default.KEY_253);
                Db44EncryptionFactorRepository.Default.AddOrUpdate(253, factor253);

                Db44ClientAccount account251 = new Db44ClientAccount(
                    Properties.Settings.Default.CompanyCode_251,
                    Properties.Settings.Default.CenterCode_251,
                    Properties.Settings.Default.Password_251);
                Db44ClientAccountRepository.Default.AddOrUpdate(251, account251);
                Db44ClientAccount account252 = new Db44ClientAccount(
                    Properties.Settings.Default.CompanyCode_252,
                    Properties.Settings.Default.CenterCode_252,
                    Properties.Settings.Default.Password_252);
                Db44ClientAccountRepository.Default.AddOrUpdate(252, account252);
                Db44ClientAccount account253 = new Db44ClientAccount(
                    Properties.Settings.Default.CompanyCode_253,
                    Properties.Settings.Default.CenterCode_253,
                    Properties.Settings.Default.Password_253);
                Db44ClientAccountRepository.Default.AddOrUpdate(253, account253);

                SmppAgent = new SmppAgent(JtjClientAccount);

                SmppAgent.PCmdHead = "++";
                SmppAgent.DebugMobileID = "A";

                SmppAgent.Logging += new LoggingEventHandler(OnLogging);
                SmppAgent.D04ReceivedFromJtj += new D04ReceivedFromJtjEventHandler(SmppAgent_D04ReceivedFromJtj);
                SmppAgent.DeliveredToJtj += new DeliveredToJtjEventHandler(SmppAgent_DeliveredToJtj);
                SmppAgent.CommuStatusUpdated += new CommuStatusUpdatedEventHandler(SmppAgent_CommuStatusUpdated);
                SmppAgent.GpsLocationUpdated += new GpsLocationUpdatedEventHandler(SmppAgent_GpsLocationUpdated);
                SmppAgent.ImageReceived += new ImageReceivedEventHandler(SmppAgent_ImageReceived);

                if (Properties.Settings.Default.UseNewSmpp)
                {
                    string[] smppIps = Properties.Settings.Default.SmppIp.Split(new char[] { ';' });
                    foreach (string s in smppIps)
                    {
                        SmppAgent.AddSmppClient(s);
                    }
                }

                if (Properties.Settings.Default.UseOldSmpp)
                {
                    string[] oldSmppIps = Properties.Settings.Default.OldSmppIp.Split(new char[] { ';' });
                    foreach (string s in oldSmppIps)
                    {
                        SmppAgent.AddOldSmppClient(s);
                    }
                }
            }
            catch (Exception exception)
            {
                FireLoggingEvent(Level.Info, "主程序启动时发生异常，详情请查阅系统日志。");
                FireLoggingEvent(Level.Advanced, exception);
                return;
            }


            EverySecondTimer.Start();

            FireLoggingEvent(Level.Info, "主程序启动完成。");
        }

        void SmppAgent_DeliveredToJtj(object sender, DeliveredToJtjEventArgs e)
        {
            AddOrUpdateJtjListViewItem(e);
        }

        void SmppAgent_D04ReceivedFromJtj(object sender, D04ReceivedFromJtjEventArgs e)
        {
            AddOrUpdateJtjListViewItem(e);
        }

        private void FireLoggingEvent(Level level, object message)
        {
            OnLogging(this, level, message);
        }
        private void OnLogging(object sender, Level level, object message)
        {
            switch (level)
            {
                case Level.Info:
                    Program.Log.Info(message);
                    AddInfoListViewItem(message.ToString());
                    break;
                case Level.Debug:
                    Program.Log.Debug(message);
                    AddDebugListViewItem(message.ToString());
                    break;
                default:
                    Program.Log.Debug(message);
                    break;
            }
        }
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            new SettingsForm().Show();
        }
        private void AddOrUpdateJtjListViewItem(DeliveredToJtjEventArgs e)
        {
            ListView view;
            Monitor.Enter(view = this.JtjListView);
            try
            {
                int itemIndex = view.Items.IndexOfKey(e.PlateNumber);
                ListViewItem item;
                if (itemIndex == -1)
                {
                    item = new ListViewItem();
                    item.Name = e.PlateNumber;
                    for (int i = 0; i < view.Columns.Count; i++)
                    {
                        item.SubItems.Add("");
                    }
                    item.SubItems[this.JtjPlateNumberColumn.Index].Text = e.PlateNumber;
                    item.SubItems[this.JtjPlateColorColumn.Index].Text = ParrotModelWrapper.GetPlateColorByPlateNumber(e.PlateNumber);
                }
                else
                {
                    item = view.Items[itemIndex];
                }

                item.SubItems[this.UploadPduColumn.Index].Text = e.FunctionCode;
                item.SubItems[this.DeliveredDateTimeColumn.Index].Text = e.DeliveredDateTime.ToString();

                if (itemIndex == -1)
                {
                    view.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error(ex);
            }
            finally
            {
                Monitor.Exit(view);
            }
        }
        private void AddOrUpdateMdtListViewItem(MdtWrapper e)
        {
            ListView view;
            Monitor.Enter(view = this.MdtListView);
            try
            {

                int itemIndex = view.Items.IndexOfKey(e.MobileID);
                ListViewItem item;
                if (itemIndex == -1)
                {
                    item = new ListViewItem();
                    item.Name = e.MobileID;
                    for (int i = 0; i < view.Columns.Count; i++)
                    {
                        item.SubItems.Add("");
                    }
                    item.SubItems[this.CommuStatus.Index].Text = "无数据";
                }
                else
                {
                    item = view.Items[itemIndex];
                }

                item.SubItems[this.MdtCode.Index].Text = e.MdtCode;
                item.SubItems[this.MdtType.Index].Text = e.DB44_MDT_Type;
                item.SubItems[this.VehicleType.Index].Text = e.DB44_VehicleType;
                item.SubItems[this.VehiclePlateNumber.Index].Text = e.Mobile_VehicleRegistration;
                try
                {
                    item.SubItems[this.VehiclePlateColor.Index].Text = VehiclePlateColorHelper.Default.Items[e.DB44_VehicleRegistrationColor];
                }
                catch
                {
                    item.SubItems[this.VehiclePlateColor.Index].Text = string.Format("({0})", e.DB44_VehicleRegistrationColor);
                }
                item.SubItems[this.VehiclePlateType.Index].Text = e.DB44_VehicleRegistration_Type;
                item.SubItems[this.SupervisorName.Index].Text = e.DB44_VehicleGroupName;
                item.SubItems[this.SupervisorOrgCode.Index].Text = e.DB44_VehicleGroupCode;
                item.SubItems[this.OmcCode.Index].Text = e.DB44_EnterpriseCode;
                item.SubItems[this.ManufacturerCode.Index].Text = e.DB44_CompanyCode.ToString();
                try
                {
                    item.SubItems[this.VehiclePurpose.Index].Text = VehiclePurposeHelper.Default.Items[(byte)(e.DB44_VehicleUseType)];
                }
                catch
                {
                    item.SubItems[this.VehiclePurpose.Index].Text = string.Format("({0})", e.DB44_VehicleUseType);
                }

                item.SubItems[this.VehicleQualificationNumber.Index].Text = e.DB44_VehicleGroupCYZGZ;

                if (itemIndex == -1)
                {
                    view.Items.Add(item);

                }
            }
            catch (Exception ex)
            {
                Program.Log.Error(ex);
            }
            finally
            {
                Monitor.Exit(view);
            }
        }
        private void UpdateMdtListViewItem(string mobileSN, string commuStatus)
        {
            ListView view;
            Monitor.Enter(view = this.MdtListView);
            try
            {

                int itemIndex = view.Items.IndexOfKey(mobileSN);
                ListViewItem item;
                if (itemIndex != -1)
                {
                    item = view.Items[itemIndex];
                    item.SubItems[this.CommuStatus.Index].Text = commuStatus;
                }

            }
            catch (Exception ex)
            {
                Program.Log.Error(ex);
            }
            finally
            {
                Monitor.Exit(view);
            }
        }

        private void AddOrUpdateJtjListViewItem(D04ReceivedFromJtjEventArgs e)
        {
            ListView view;
            Monitor.Enter(view = this.JtjListView);
            try
            {

                int itemIndex = view.Items.IndexOfKey(e.PlateNumber);
                ListViewItem item;
                if (itemIndex == -1)
                {
                    item = new ListViewItem();
                    item.Name = e.PlateNumber;
                    for (int i = 0; i < view.Columns.Count; i++)
                    {
                        item.SubItems.Add("");
                    }
                    item.SubItems[this.JtjPlateNumberColumn.Index].Text = e.PlateNumber;
                    item.SubItems[this.JtjPlateColorColumn.Index].Text = ParrotModelWrapper.GetPlateColorByPlateNumber(e.PlateNumber);
                }
                else
                {
                    item = view.Items[itemIndex];
                }

                item.SubItems[this.DownloadPduColumn.Index].Text = string.Format("D04({0})", e.PlateColor);
                item.SubItems[this.ReceivedDateTimeColumn.Index].Text = e.ReceivedDateTime.ToString();

                if (itemIndex == -1)
                {
                    view.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error(ex);
            }
            finally
            {
                Monitor.Exit(view);
            }
        }

        private void DatabaseProcessor_MdtListUpdated(object sender, MdtWrapper mobileInfo)
        {
            AddOrUpdateMdtListViewItem(mobileInfo);
        }

        /// <summary>
        /// 更新MDT的位置。
        /// </summary>
        /// <param name="mdt"></param>
        /// <param name="gpsData"></param>
        private void SmppAgent_GpsLocationUpdated(object sender, MdtWrapper mdt, Db44GpsData gpsData)
        {
            //AddDeliveredToJtjListViewItem(mdt, gpsData);
        }

        /// <summary>
        /// 更新通讯状态。
        /// </summary>
        /// <param name="mdtId"></param>
        /// <param name="status">通讯状态</param>
        private void SmppAgent_CommuStatusUpdated(string mobileSN, string commuStatus)
        {
            UpdateMdtListViewItem(mobileSN, commuStatus);
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            if (MessageBox.Show(this, "是否确定要退出系统？？", "注意", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                //SmppAgent.UpdateLastGpsLocation();
                Environment.Exit(0);
            }
        }
        private void DebugInfoListView_DoubleClick(object sender, EventArgs e)
        {
            int index = this.DebugListView.FocusedItem.Index;
            this.DebugDetailsTextBox.Text = this.DebugListView.Items[index].SubItems[2].Text;
        }
        private void MdtListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (this.MdtListView.Tag == null)
            {
                this.MdtListView.Tag = true;
            }
            if ((bool)this.MdtListView.Tag)
            {
                this.MdtListView.Tag = false;
            }
            else
            {
                this.MdtListView.Tag = true;
            }
            this.MdtListView.ListViewItemSorter = new ListViewSort(e.Column, (bool)MdtListView.Tag);
            this.MdtListView.Sort();
        }
        private void SmppAgent_ImageReceived(string _ID, int MointerID, long ImageID, int ImageLen, int ImageSeq, string ImageBody, string ImgStatu, DateTime FilmDateTime, MdtWrapper mobileInfo)
        {
            byte[] buffer = Convert.FromBase64String(ImageBody);
            if (ImageSeq == 0)
            {
                this.LastReceivedImage = new byte[buffer.Length * ImageLen];
                this.LastReceivedImageIndex = 0;
            }
            buffer.CopyTo(this.LastReceivedImage, this.LastReceivedImageIndex);
            this.LastReceivedImageIndex = buffer.Length;
            if (ImageSeq == (ImageLen - 1))
            {
                Image image = Image.FromStream(new MemoryStream(this.LastReceivedImage));
                this.LastReceivedImagePictureBox.Image = image;
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                TabControl control = (TabControl)sender;
                this.SmppAgent.IsDebugLoggingEnable = (control.SelectedIndex == 2);
            }
            catch
            {
            }
        }
        private void DebugMobileID_TextChanged(object sender, EventArgs e)
        {
            this.SmppAgent.DebugMobileID = this.DebugMobileID.Text;
        }
        private void EverySecondTimer_Tick(object sender, EventArgs e)
        {
            this.CountOfReceivingFromMdt.Text = SmppAgent.CountOfReceivingFromMdt.ToString();
            this.SpeedOfReceivingFromMdt.Text = SmppAgent.SpeedOfReceivingFromMdt.ToString();
            SmppAgent.SpeedOfReceivingFromMdt = 0;
            this.SpeedOfSendingToJtj.Text = SmppAgent.SpeedOfSendingToJtj.ToString();
            SmppAgent.SpeedOfSendingToJtj = 0;
            this.CountOfSendingToJtj.Text = SmppAgent.CountOfSendingToJtj.ToString();
            this.TxErrorLabel.Text = SmppAgent.ErrorCountOfSendingToJtj.ToString();
            this.CountOfReceivingFromJtj.Text = SmppAgent.CountOfReceivingFromJtj.ToString();
        }

        private void ClearDebugListViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DebugListView.Items.Clear();
            this.CurrentDebugListViewIndex = 0;
        }

        private void FocusThisMdtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.DebugMobileID.Text = this.MdtListView.FocusedItem.SubItems[this.MdtCode.Index].Text;
                this.SmppAgent.DebugMobileID = this.DebugMobileID.Text;
                this.ToTerminalListView.SelectedIndex = 2;
            }
            catch { }

        }

        private void JtjListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView view = this.JtjListView;
            if (view.Tag == null)
            {
                view.Tag = true;
            }
            if ((bool)view.Tag)
            {
                view.Tag = false;
            }
            else
            {
                view.Tag = true;
            }
            view.ListViewItemSorter = new ListViewSort(e.Column, (bool)view.Tag);
            view.Sort();
        }
    }
}