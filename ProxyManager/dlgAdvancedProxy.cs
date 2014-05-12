using System;
using System.Windows.Forms;

namespace ProxyManager
{
    public partial class dlgAdvancedProxy : Form
    {
        private ProxySettings _proxy;

        public dlgAdvancedProxy()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.ProxyConfiguration = new ProxySettings();
            this.Icon = null;

            foreach (var tb in new[] { txtHttpAddress, txtHttpsAddress, txtFtpAddress, txtSocksAddress })
            {
                tb.Validating += dlgProxySettings.ProxyAddressTextBox_Validating;
                tb.Enter += dlgProxySettings.ProxyAddressAndPortTextBox_Enter;
            }
            foreach (var tb in new[] { txtHttpPort, txtHttpsPort, txtFtpPort, txtSocksPort })
            {
                tb.Validating += dlgProxySettings.ProxyPortTextBox_Validating;
                tb.Enter += dlgProxySettings.ProxyAddressAndPortTextBox_Enter;
            }
        }

        public ProxySettings ProxyConfiguration
        {
            get { return _proxy; }
            set { _proxy = value; UpdateControls(); }
        }

        private void AddressAndPortTextBox_Enter(object sender, EventArgs e)
        {
            // back up the address entered
            var t = (TextBox)sender;
            t.Tag = t.Text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var sep = new[] { ';', '\r', '\n' };
            var exceptions = txtExceptions.Text.Trim().Split(sep, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < exceptions.Length; i++)
                exceptions[i] = exceptions[i].Trim();

            var ht = new ProxyAddress(txtHttpAddress.Text.Trim() + ":" + txtHttpPort.Text.Trim());
            var hs = new ProxyAddress(txtHttpsAddress.Text.Trim() + ":" + txtHttpsPort.Text.Trim());
            var ft = new ProxyAddress(txtFtpAddress.Text.Trim() + ":" + txtFtpPort.Text.Trim());
            var sk = new ProxyAddress(txtSocksAddress.Text.Trim() + ":" + txtSocksPort.Text.Trim());

            _proxy = new ProxySettings(ht, hs, ft, sk, exceptions);
            _proxy.BypassForLocal = ((bool)txtExceptions.Tag);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void chkSingleProxy_CheckedChanged(object sender, EventArgs e)
        {
            var enable = !chkSingleProxy.Checked;
            foreach (var tb in new[]{txtHttpsAddress, txtHttpsPort, txtFtpAddress, txtFtpPort,
                                     txtSocksAddress, txtSocksPort   })
                tb.Enabled = enable;
        }

        private void txtExceptions_EnterLeave(object sender, EventArgs e)
        {
            this.AcceptButton = txtExceptions.Focused ? null : btnOk;
        }

        private void UpdateControls()
        {
            txtHttpAddress.Text = _proxy.Http.Address;
            txtHttpsAddress.Text = _proxy.Https.Address;
            txtFtpAddress.Text = _proxy.Ftp.Address;
            txtSocksAddress.Text = _proxy.Socks.Address;
            txtHttpPort.Text = _proxy.Http.HasAddress ? _proxy.Http.Port.ToString() : "";
            txtHttpsPort.Text = _proxy.Https.HasAddress ? _proxy.Https.Port.ToString() : "";
            txtFtpPort.Text = _proxy.Ftp.HasAddress ? _proxy.Ftp.Port.ToString() : "";
            txtSocksPort.Text = _proxy.Socks.HasAddress ? _proxy.Socks.Port.ToString() : "";
            txtExceptions.Tag = _proxy.BypassForLocal;
            txtExceptions.Text = string.Join(";", _proxy.BypassList.ToArray());
            chkSingleProxy.Checked = _proxy.SingleProxy;
        }
    }
}