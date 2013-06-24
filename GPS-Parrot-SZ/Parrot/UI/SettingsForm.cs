using System;
using System.Windows.Forms;

namespace Parrot
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            this.InitializeComponent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DbServer = DbServer.Text;
            Properties.Settings.Default.DbName = DbName.Text;
            Properties.Settings.Default.DbUseWindowsAuthenticationMode = DbUseWindowsAuthenticationMode.Checked;
            Properties.Settings.Default.DbUsername = DbUsername.Text;
            Properties.Settings.Default.DbPassword = DbPassword.Text;
            Properties.Settings.Default.NetId = NetId.Text;
            Properties.Settings.Default.OperatorName = OperatorName.Text;
            Properties.Settings.Default.OperatorPassword = OperatorPassword.Text;

            Properties.Settings.Default.SmppIp = SmppIP.Text;
            Properties.Settings.Default.UseNewSmpp = UseNewSmpp.Checked;
            Properties.Settings.Default.OldSmppIp = OldSmppIP.Text;
            Properties.Settings.Default.UseOldSmpp = UseOldSmpp.Checked;

            Properties.Settings.Default.JtjServerIp = JtjServerIp.Text;
            Properties.Settings.Default.JtjListeningPort = int.Parse(JtjListeningPort.Text);
            Properties.Settings.Default.JtjLocalPort = int.Parse(JtjLocalPort.Text);
            Properties.Settings.Default.JtjClientId = int.Parse(JtjClientId.Text);
            Properties.Settings.Default.JtjUsername = JtjUsername.Text;
            Properties.Settings.Default.JtjPassword = JtjPassword.Text;

            Properties.Settings.Default.M1_251 = uint.Parse(M1_251.Text);
            Properties.Settings.Default.IA1_251 = uint.Parse(IA1_251.Text);
            Properties.Settings.Default.IC1_251 = uint.Parse(IC1_251.Text);
            Properties.Settings.Default.KEY_251 = uint.Parse(Key_251.Text);
            Properties.Settings.Default.CompanyCode_251 = ushort.Parse(ManufacturerCode_251.Text);
            Properties.Settings.Default.Password_251 = uint.Parse(Db44Password_251.Text);
            Properties.Settings.Default.CenterCode_251 = uint.Parse(CenterId_251.Text);

            Properties.Settings.Default.M1_252 = uint.Parse(M1_252.Text);
            Properties.Settings.Default.IA1_252 = uint.Parse(IA1_252.Text);
            Properties.Settings.Default.IC1_252 = uint.Parse(IC1_252.Text);
            Properties.Settings.Default.KEY_252 = uint.Parse(Key_252.Text);
            Properties.Settings.Default.CompanyCode_252 = ushort.Parse(ManufacturerCode_252.Text);
            Properties.Settings.Default.Password_252 = uint.Parse(Db44Password_252.Text);
            Properties.Settings.Default.CenterCode_252 = uint.Parse(CenterId_252.Text);

            Properties.Settings.Default.M1_253 = uint.Parse(M1_253.Text);
            Properties.Settings.Default.IA1_253 = uint.Parse(IA1_253.Text);
            Properties.Settings.Default.IC1_253 = uint.Parse(IC1_253.Text);
            Properties.Settings.Default.KEY_253 = uint.Parse(Key_253.Text);
            Properties.Settings.Default.CompanyCode_253 = ushort.Parse(ManufacturerCode_253.Text);
            Properties.Settings.Default.Password_253 = uint.Parse(Db44Password_253.Text);
            Properties.Settings.Default.CenterCode_253 = uint.Parse(CenterId_253.Text);

            Properties.Settings.Default.Save();

            MessageBox.Show("系统配置参数已保存，请重新启动程序。");
            Application.Exit();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            DbServer.Text = Properties.Settings.Default.DbServer;
            DbName.Text = Properties.Settings.Default.DbName;
            DbUseWindowsAuthenticationMode.Checked = Properties.Settings.Default.DbUseWindowsAuthenticationMode;
            DbUsername.Text = Properties.Settings.Default.DbUsername;
            DbPassword.Text = Properties.Settings.Default.DbPassword;
            NetId.Text = Properties.Settings.Default.NetId;
            OperatorName.Text = Properties.Settings.Default.OperatorName;
            OperatorPassword.Text = Properties.Settings.Default.OperatorPassword;

            SmppIP.Text = Properties.Settings.Default.SmppIp;
            UseNewSmpp.Checked = Properties.Settings.Default.UseNewSmpp;
            OldSmppIP.Text = Properties.Settings.Default.OldSmppIp;
            UseOldSmpp.Checked = Properties.Settings.Default.UseOldSmpp;

            JtjServerIp.Text = Properties.Settings.Default.JtjServerIp;
            JtjListeningPort.Text = Properties.Settings.Default.JtjListeningPort.ToString();
            JtjLocalPort.Text = Properties.Settings.Default.JtjLocalPort.ToString();
            JtjClientId.Text = Properties.Settings.Default.JtjClientId.ToString();
            JtjUsername.Text = Properties.Settings.Default.JtjUsername;
            JtjPassword.Text = Properties.Settings.Default.JtjPassword;

            M1_251.Text = Properties.Settings.Default.M1_251.ToString();
            IA1_251.Text = Properties.Settings.Default.IA1_251.ToString();
            IC1_251.Text = Properties.Settings.Default.IC1_251.ToString();
            Key_251.Text = Properties.Settings.Default.KEY_251.ToString();
            ManufacturerCode_251.Text = Properties.Settings.Default.CompanyCode_251.ToString();
            Db44Password_251.Text = Properties.Settings.Default.Password_251.ToString();
            CenterId_251.Text = Properties.Settings.Default.CenterCode_251.ToString();

            M1_252.Text = Properties.Settings.Default.M1_252.ToString();
            IA1_252.Text = Properties.Settings.Default.IA1_252.ToString();
            IC1_252.Text = Properties.Settings.Default.IC1_252.ToString();
            Key_252.Text = Properties.Settings.Default.KEY_252.ToString();
            ManufacturerCode_252.Text = Properties.Settings.Default.CompanyCode_252.ToString();
            Db44Password_252.Text = Properties.Settings.Default.Password_252.ToString();
            CenterId_252.Text = Properties.Settings.Default.CenterCode_252.ToString();

            M1_253.Text = Properties.Settings.Default.M1_253.ToString();
            IA1_253.Text = Properties.Settings.Default.IA1_253.ToString();
            IC1_253.Text = Properties.Settings.Default.IC1_253.ToString();
            Key_253.Text = Properties.Settings.Default.KEY_253.ToString();
            ManufacturerCode_253.Text = Properties.Settings.Default.CompanyCode_253.ToString();
            Db44Password_253.Text = Properties.Settings.Default.Password_253.ToString();
            CenterId_253.Text = Properties.Settings.Default.CenterCode_253.ToString();
        }

        private void UseNewSmpp_CheckedChanged(object sender, EventArgs e)
        {
            this.SmppIP.Enabled = this.UseNewSmpp.Checked;
        }

        private void UseOldSmpp_CheckedChanged(object sender, EventArgs e)
        {
            this.OldSmppIP.Enabled = this.UseOldSmpp.Checked;
        }
        
        private void DbUseWindowsAuthenticationMode_CheckedChanged(object sender, EventArgs e)
        {
            DbUsername.Enabled = !DbUseWindowsAuthenticationMode.Checked;
            DbPassword.Enabled = !DbUseWindowsAuthenticationMode.Checked;
        }
    }
}