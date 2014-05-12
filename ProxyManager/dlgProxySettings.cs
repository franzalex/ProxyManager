using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProxyManager
{
    public partial class dlgProxySettings : Form
    {
        private ProxySettings manualProxy;
        private ConnectionSettings netConfig;
        private static dlgProxySettings _instance;

        public dlgProxySettings()
        {
            InitializeComponent();

            this.Icon = Program.Icon;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            txtProxyAddress.Enter += ProxyAddressAndPortTextBox_Enter;
            txtProxyAddress.Validating += ProxyAddressTextBox_Validating;
            txtProxyPort.Enter += ProxyAddressAndPortTextBox_Enter;
            txtProxyPort.Validating += ProxyPortTextBox_Validating;

            var types = new[]{Tuple.Create(ConnectionType.NoProxy, "No Proxy"),
                              Tuple.Create(ConnectionType.AutoDetect, "Auto Detect Settings"),
                              Tuple.Create(ConnectionType.AutoConfigUrl, "Automatic Configuration Script"),
                              Tuple.Create(ConnectionType.ManualProxy, "Manual Proxy Settings")};

            cboType.ValueMember = "Item1";
            cboType.DisplayMember = "Item2";
            cboType.DataSource = types;
            cboType.SelectedValue = ConnectionType.NoProxy;

            this.ConnectionSettings = ConnectionSettings.GetCurrentConfig();
        }

        public ConnectionSettings ConnectionSettings
        {
            get
            {
                return netConfig;
            }
            set
            {
                if (value != null)
                {
                    netConfig = value;
                    netConfig.ManualProxy.With((m) =>
                              manualProxy = new ProxySettings(m.Http, m.Https, m.Ftp, m.Socks,
                                                                   m.ProxyOverrides.Split(';')));

                    UpdateControls();
                }
            }
        }

        public static dlgProxySettings Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                    _instance = new dlgProxySettings();

                return _instance;
            }
        }

        internal static void ProxyAddressAndPortTextBox_Enter(object sender, EventArgs e)
        {
            var t = (TextBox)sender;
            t.Tag = t.Text;
        }

        internal static void ProxyAddressTextBox_Validating(object sender, CancelEventArgs e)
        {
            var tb = (WatermarkTextBox)sender;
            var invalid = !ProxyAddress.IsValidAddress(tb.Text);

            if (invalid)
                if (MessageBox.Show("Invalid address entered.", "Manual Proxy Configuration",
                                   MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    tb.Text = (string)tb.Tag;
                }
                else
                {
                    // cancel when address is invalid and user wants to retry
                    ((TextBox)sender).SelectAll();
                    e.Cancel = true;
                }
        }

        internal static void ProxyPortTextBox_Validating(object sender, CancelEventArgs e)
        {
            var tb = (TextBox)sender;
            var t = tb.Text.Trim();
            UInt16 u;
            e.Cancel = !(string.IsNullOrEmpty(t) || UInt16.TryParse(t, out u));

            if (e.Cancel)
                if (MessageBox.Show("Invalid port entered.", "Manual Proxy Configuration",
                                   MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    tb.Text = (string)tb.Tag;
                }
                else
                {
                    // cancel when address is invalid and user wants to retry
                    tb.SelectAll();
                    e.Cancel = true;
                }
        }

        private void btnAdvancedProxy_Click(object sender, EventArgs e)
        {
            using (var d = new dlgAdvancedProxy() { ProxyConfiguration = manualProxy })
                if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    manualProxy = d.ProxyConfiguration;
                    UpdateProxyControls();
                }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtProxyAddress.Enabled)
            {
                var proxy = txtProxyAddress.Text.Trim() + ":" + txtProxyPort.Text.Trim();
                manualProxy = new ProxySettings(new ProxyAddress(proxy), manualProxy.BypassList.ToArray());
            }

            netConfig.ConnectionType = (ConnectionType)cboType.SelectedValue;
            netConfig.AutoConfigUrl = txtAutoConfigUrl.Text.Trim();
            netConfig.ManualProxy.With((m) => {
                m.BypassList.Clear();
                m.BypassList.AddRange(manualProxy.BypassList);
                m.BypassForLocal = chkNoLocalProxy.Checked;
                m.Ftp = manualProxy.Ftp;
                m.Http = manualProxy.Http;
                m.Https = manualProxy.Https;
                m.Socks = manualProxy.Socks;
            });

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void cboType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cboType.SelectedValue == null || cboType.SelectedValue.GetType() != typeof(ConnectionType))
                return;

            var selectedItem = (ConnectionType)cboType.SelectedValue;
            var disabledControls = new List<Control>();
            var visibleRows = new List<int>(new[] { 0, tlpMain.RowCount - 1 });

            if (selectedItem == ConnectionType.AutoConfigUrl)
                visibleRows.Add(tlpMain.GetRow(txtAutoConfigUrl));
            else if (selectedItem == ConnectionType.ManualProxy)
                foreach (var c in new Control[] { txtProxyAddress, txtProxyPort, btnAdvancedProxy, chkNoLocalProxy })
                    visibleRows.Add(tlpMain.GetRow(c));

            tlpMain.SuspendLayout();
            // disable all controls not on the first or last row of the main table layout panel
            foreach (Control ctl in tlpMain.Controls)
                ctl.Visible = visibleRows.Contains(tlpMain.GetRow(ctl));
            tlpMain.ResumeLayout(true);
        }

        private void dlgProxySettings_Shown(object sender, EventArgs e)
        {
            // prompt the user if connection type has both manual and auto config url
            if (netConfig.ConnectionType.GetAllFlagsPresent().Length > 1)
            {
                MessageBox.Show("Select the connection type to use for this network.", "Network Configuration",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboType.DroppedDown = true;
            }
        }

        private void UpdateControls()
        {
            cboType.SelectedValue = ConnectionType.NoProxy;
            foreach (var f in netConfig.ConnectionType.GetAllFlagsPresent())
                if (f != ConnectionType.NoProxy)
                {
                    cboType.SelectedValue = f;
                    break;
                }

            txtAutoConfigUrl.Text = netConfig.AutoConfigUrl;
            chkNoLocalProxy.Checked = manualProxy.BypassForLocal;

            UpdateProxyControls();
        }

        private void UpdateProxyControls()
        {
            var singleProxy = netConfig.ManualProxy.SingleProxy;
            var setProxyText = manualProxy != null &&
                               manualProxy.Http != null && manualProxy.Http.Address != null;

            txtProxyAddress.Enabled = singleProxy;
            txtProxyPort.Enabled = singleProxy;
            txtProxyAddress.Text = singleProxy && setProxyText ? manualProxy.Http.Address : "";
            txtProxyPort.Text = singleProxy && setProxyText ? manualProxy.Http.Port.ToString() : "";
        }
    }
}