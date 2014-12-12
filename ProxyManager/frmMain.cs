using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace ProxyManager
{
    public partial class frmMain : Form
    {
        #region NetworkMatch

        private class NetworkMatch
        {
            public int Index;
            public double MatchScore;
            public NetworkSettings NetworkSetting;
        }

        #endregion NetworkMatch

        private bool CurrentConnectionIsKnown;
        private IEnumerable<NetworkInformation> transmittingInterfaces;
        private ConnectionSettings CurrentConnectionSetting;
        private DateTime SettingsApplied;
        private Timer tmrBalloon;

        public frmMain()
        {
            InitializeComponent();
            this.Icon = Program.Icon;
            this.WindowState = FormWindowState.Minimized;
            notifyIcon.Icon = this.Icon;
            notifyIcon.Text = Program.Name;
            tmrBalloon = new Timer(this.components) { Interval = 2500 };
            tmrBalloon.Tick += tmrBalloon_Tick;
            cmnuNotifAutoStart.Enabled = !Program.Arguments.Contains(Program.ValidArgs.NoAutorun);

            Program.RegistryWatcher.RegChanged += RegistryWatcher_RegChanged;
            Program.NetworkWatcher.NetworkConnect += NetworkWatcher_NetworkEvent;
            Program.NetworkWatcher.NetworkDisconnect += NetworkWatcher_NetworkEvent;
            Program.NetworkWatcher.NetworkAddressChanged += NetworkWatcher_NetworkEvent;

            transmittingInterfaces = Enumerable.Empty<NetworkInformation>();

            // LOW: render Windows 7 style tray window (e.g. volume control & action center)
        }

        private delegate void UpdateProxyDropDownItemsDelegate();

        /// <summary>Finds the networks that match the specified network information.</summary>
        /// <returns></returns>
        private static IEnumerable<NetworkMatch> FindNetworkMatches(IEnumerable<NetworkInformation> ni,
                                                                    double minMatchScore)
        {
            var yields = new HashSet<ConnectionSettings>();

            var matches = Program.NetworkSettings.Select((s, i) => new NetworkMatch() {
                NetworkSetting = s,
                Index = i,
                MatchScore = s.NetworkMatchScore(ni)
            }).Where(m => m.MatchScore >= minMatchScore).OrderByDescending(n => n.MatchScore);

            foreach (var m in matches)
                if (yields.Add(m.NetworkSetting.Configuration))
                    yield return m;
        }

        /// <summary>Saves the specified network configuration.</summary>
        /// <param name="connSetting">The connection setting.</param>
        /// <param name="netInfo">The network information associated with the connection setting.</param>
        private static void SaveConfiguration(ConnectionSettings connSetting,
                                              IEnumerable<NetworkInformation> netInfo)
        {
            var netConfig = Program.NetworkSettings.Select((s, i) => new {
                Setting = s,
                MatchScore = s.NetworkMatchScore(netInfo),
                Index = i
            }).OrderByDescending(i => i.MatchScore).ToList();

            var c1 = netConfig.FirstOrDefault();

            // save new setting if exact network found and configuration different
            // otherwise add new network and configuration to network settings list
            if (netConfig.Any() && c1.MatchScore == 1)
            {
                if (!c1.Setting.Configuration.Equals(connSetting))
                    Program.NetworkSettings[c1.Index].Configuration = connSetting;
            }
            else
                Program.NetworkSettings.Add(new NetworkSettings(netInfo, connSetting));
        }

        /// <summary>Applies the network setting.</summary>
        /// <param name="setting">The setting.</param>
        private void ApplyNetworkSetting(ConnectionSettings setting)
        {
            // apply new configuration only if it is different from current one
            if (!setting.Equals(ConnectionSettings.GetCurrentConfig()))
                ConnectionSettings.SetConfig(setting);

            CurrentConnectionSetting = ConnectionSettings.GetCurrentConfig();
            SettingsApplied = DateTime.Now;
            ShowConnectionSettingBalloon();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var nc = ConnectionSettings.GetCurrentConfig();

            //using (var dlg = new dlgProxySettings() { ConnectionSettings = nc })
            //    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //        nc = dlg.ConnectionSettings;

            //var jsonFile = @"C:\Users\Alex\Desktop\proxy.json.txt";
            //var jset = new JsonSerializerSettings();
            //jset.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            //jset.Converters.Add(new NetworkInformation.JsonConverter());
            //var jsonOutput = JsonConvert.SerializeObject(nc, Formatting.Indented, jset);
            //System.IO.File.WriteAllText(jsonFile, jsonOutput);
            //var nc1 = JsonConvert.DeserializeObject<ConnectionSettings>(jsonOutput);

            //var nic = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
            //                .Where(n => n.OperationalStatus ==
            //                            System.Net.NetworkInformation.OperationalStatus.Up)
            //                .Select(n => new NetworkInformation(n)).First();

            //var nicJson = @"C:\Users\Alex\Desktop\WiFi.json.txt";
            //var nicJsonOutput = JsonConvert.SerializeObject(nic, Formatting.Indented, jset);
            //System.IO.File.WriteAllText(nicJson, nicJsonOutput);
            //var ni = JsonConvert.DeserializeObject<NetworkInformation>(nicJsonOutput, jset);
        }

        private void cmnuNotifAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            Program.AutoStart = cmnuNotifAutoStart.Checked;
        }

        private void cmnuNotifExit_Click(object sender, EventArgs e)
        {
            Program.Exit();
        }

        private void cmnuProxySettings_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == cmnuProxySettingsChange)
            {   // change proxy settings
                var dlg = dlgProxySettings.Instance;

                if (!dlg.Visible)
                {
                    dlg.ConnectionSettings = CurrentConnectionSetting;
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ConnectionSettings.SetConfig(dlg.ConnectionSettings);
                        SaveConfiguration(dlg.ConnectionSettings, transmittingInterfaces);
                    }
                }
                else
                {
                    dlg.BringToFront();
                }
            }
            else if (e.ClickedItem.Tag != null)
            {   // any other item
                var config = (ConnectionSettings)e.ClickedItem.Tag;
                ConnectionSettings.SetConfig(config);
                SaveConfiguration(config, transmittingInterfaces);
            }
        }

        private void cmnuProxySettings_DropDownOpening(object sender, EventArgs e)
        {
            // set the checkbox on the current configuration if it exists in there
            var c = CurrentConnectionSetting;

            foreach (var i in cmnuProxySettings.DropDownItems.OfType<ToolStripMenuItem>())
            {
                i.Checked = i.Tag != null && i.Tag.As<ConnectionSettings>().Equals(c);
            }
        }

        private void ctxNotification_Opening(object sender, CancelEventArgs e)
        {
            cmnuNotifAutoStart.Checked = Program.AutoStart;
            UpdateProxyDropDownItems();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Visible = false;

            // trigger a network event and find an exact match for the network we are on
            NetworkWatcher_NetworkEvent(null, EventArgs.Empty);
            var m = FindNetworkMatches(transmittingInterfaces, 1);
            if (m.Any())    // exact match found. use it as the current setting
                ApplyNetworkSetting(m.First().NetworkSetting.Configuration);

            // trigger registry watcher event to update current setting
            SettingsApplied = DateTime.Now.AddHours(-1); // spoof an older time to ensure the method doesn't short-circuit
            RegistryWatcher_RegChanged(null, null);
            SettingsApplied = DateTime.Now;

            ShowConnectionSettingBalloon();

            Program.RegistryWatcher.Start();
            Program.NetworkWatcher.Start();
        }

        private void NetworkWatcher_NetworkEvent(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler((o, e1) => NetworkWatcher_NetworkEvent(sender, e)));
                return;
            }

            //var eType = e.GetType();
            //if (eType == typeof(NetworkDisconnectEventArgs))
            //{
            //    var ne = e.As<NetworkDisconnectEventArgs>();
            //    //ShowBalloonTipDelayed(2500, Program.Name, "Disconnected from " + ne.NetworkInterface.Name,
            //    //                      ToolTipIcon.Info);
            //}
            //else if (eType == typeof(NetworkConnectEventArgs))
            //{
            //    var ne = e.As<NetworkConnectEventArgs>();
            //    //ShowBalloonTipDelayed(2500, Program.Name, "Connected to " + ne.NetworkInterface.Name,
            //    //                      ToolTipIcon.Info);
            //}
            //else if (eType == typeof(NetworkAddressChangedEventArgs))
            //{
            //    var ne = e.As<NetworkAddressChangedEventArgs>();
            //    //if (ne.NetworkInfo.NetworkInterface.Status != System.Net.NetworkInformation.OperationalStatus.Up)
            //    //    return;

            //    //ShowBalloonTipDelayed(2500, Program.Name, string.Format("IP Address on {0}\r\nchanged to ",
            //    //                      ne.NetworkInfo.NetworkInterface.Name,
            //    //                      ne.NetworkInfo.IPv4Address),
            //    //                      ToolTipIcon.Info);
            //}

            // update proxy dropdown items and apply default on all network events
            var ni = NetworkInterfaceInternal.GetTransmittingInterfaces()
                                             .Select(n => new NetworkInformation(n))
                                             .Where(i => i.InitializedSuccessfully)
                                             .ToArray();

            // check if the elements in the two sets are different before applying setting
            // prevents unnecessary updates to settings and hence balloon pop ups.
            if (!ni.ElementsAreSame(transmittingInterfaces))
            {
                transmittingInterfaces = ni;
                var matches = FindNetworkMatches(ni, 0.5);
                if (matches.Any())
                    ApplyNetworkSetting(matches.First().NetworkSetting.Configuration);
            }
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            if (!CurrentConnectionIsKnown)
            {
                // prevent showing the dialog box twice
                if (dlgProxySettings.Instance.Visible) return;

                dlgProxySettings.Instance.ConnectionSettings = CurrentConnectionSetting;
                if (dlgProxySettings.Instance.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ApplyNetworkSetting(dlgProxySettings.Instance.ConnectionSettings);
                    SaveConfiguration(dlgProxySettings.Instance.ConnectionSettings, transmittingInterfaces);
                    RegistryWatcher_RegChanged(null, null);
                    ShowConnectionSettingBalloon();
                }
            }
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (dlgProxySettings.Instance.Visible)
            {
                dlgProxySettings.Instance.BringToFront();
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ShowConnectionSettingBalloon();
                return;
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                cmnuProxySettingsChange.PerformClick();
        }

        private void RegistryWatcher_RegChanged(object sender, EventArgs e)
        {
            if ((DateTime.Now - SettingsApplied).TotalSeconds < 5)
                return; // short-circuit if we applied a setting within the last 5 seconds

            CurrentConnectionSetting = ConnectionSettings.GetCurrentConfig();
            CurrentConnectionIsKnown = FindNetworkMatches(transmittingInterfaces, 0.5)
                                       .Any(n => n.NetworkSetting.Configuration.Equals(CurrentConnectionSetting));
        }

        /// <summary>Shows the notification icon's balloon tip after the specified delay.</summary>
        /// <param name="duration">The time period, in milliseconds, the balloon tip should display.</param>
        /// <param name="title">The title to display on the balloon tip.</param>
        /// <param name="text">The text to display on the balloon tip.</param>
        /// <param name="icon">One of the <see cref="ToolTipIcon" /> values.</param>
        /// <param name="delay">The time period, in milliseconds, after which the balloon tip will be displayed.</param>
        internal void ShowBalloonTipDelayed(int duration, string title, string text,
                                           ToolTipIcon icon, int delay = 2000)
        {
            tmrBalloon.Stop();
            // ensure we have at least two lines of text.
            // split lines, skip first line check if there are any more lines
            text = text.Split('\r', '\n').Where(s => !s.IsNullOrEmpty()).Skip(1).Any() ? text : "\r\n " + text;
            tmrBalloon.Tag = string.Join("|", new[] { duration.ToString(), title, text, ((int)icon).ToString() });
            tmrBalloon.Interval = Math.Max(1, delay);
            tmrBalloon.Start();
        }

        /// <summary>Shows the connection setting in a notification icon balloon.</summary>
        private void ShowConnectionSettingBalloon()
        {
            var c = CurrentConnectionSetting;
            var tniNames = string.Join("\r\n",
                transmittingInterfaces.Select(ti => "\t\t" + ti.NetworkInterface.Name).ToArray());

            var msgLn1 = "Connected On:\t" + (tniNames.Any() ? tniNames.TrimStart() : "Disconnected");
            var msgLn2 = "Current Setting:\t" + c.ToString();

            ShowBalloonTipDelayed(10, Program.Name, string.Join("\r\n", new[] { msgLn1, msgLn2 }) +
                                  (!CurrentConnectionIsKnown ? "\r\n\r\nClick on this balloon to edit." : ""),
                                  ToolTipIcon.Info, 500);
        }

        private void tmrBalloon_Tick(object sender, EventArgs e)
        {
            tmrBalloon.Stop();
            if (tmrBalloon.Tag != null)
            {
                var p = tmrBalloon.Tag.ToString().Split('|');
                notifyIcon.ShowBalloonTip(int.Parse(p[0]), p[1], p[2], (ToolTipIcon)int.Parse(p[3]));
                tmrBalloon.Tag = null;
            }
        }

        /// <summary>Updates the proxy settings context menu's drop down items.</summary>
        private void UpdateProxyDropDownItems()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdateProxyDropDownItemsDelegate(() => UpdateProxyDropDownItems()));
                return;
            }

            // find the saved network with highest match score and apply its configuration

            var netConfig = FindNetworkMatches(transmittingInterfaces, 0.5).ToArray();

            // add top 3 configurations to the proxies list
            cmnuProxySettings.DropDownItems.Clear();
            if (netConfig.Any())
            {
                cmnuProxySettings.DropDownItems.AddRange(
                    netConfig.Take(3).Select(c => new ToolStripMenuItem() {
                        Text = string.Format("{0}", c.NetworkSetting.Configuration, c.MatchScore),
                        ToolTipText = string.Format("Saved network configuration.\r\n" +
                                                    "{0:p2} match to current network", c.MatchScore),
                        AutoToolTip = true,
                        Checked = CurrentConnectionSetting.Equals(c.NetworkSetting.Configuration),
                        Tag = c.NetworkSetting.Configuration
                    }).ToArray());
                ((ToolStripMenuItem)cmnuProxySettings.DropDownItems[0]).Checked = true;

                cmnuProxySettings.DropDownItems.Add(new ToolStripSeparator());

                // add the current setting if it is unsaved
                if (!CurrentConnectionIsKnown)
                {
                    cmnuProxySettings.DropDownItems.Add(new ToolStripMenuItem() {
                        Text = string.Format("*{0}", CurrentConnectionSetting),
                        ToolTipText = "Current network configuration.\r\nClick to save.",
                        AutoToolTip = true,
                        Checked = true,
                        Tag = CurrentConnectionSetting
                    });
                    cmnuProxySettings.DropDownItems.Add(new ToolStripSeparator());
                }
            }

            cmnuProxySettings.DropDownItems.Add(cmnuProxySettingsChange);
        }
    }
}