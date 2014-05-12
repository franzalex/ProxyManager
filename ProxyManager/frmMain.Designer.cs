namespace ProxyManager
{
    partial class frmMain
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
            this.button1 = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.ctxNotification = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmnuProxySettings = new System.Windows.Forms.ToolStripMenuItem();
            this.cmnuProxySettingsChange = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxNotifSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuNotifAutoStart = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxNotifSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmnuNotifExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxNotification.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(53, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.ctxNotification;
            this.notifyIcon.Visible = true;
            this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.notifyIcon_BalloonTipClicked);
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // ctxNotification
            // 
            this.ctxNotification.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuProxySettings,
            this.ctxNotifSep1,
            this.cmnuNotifAutoStart,
            this.ctxNotifSep2,
            this.cmnuNotifExit});
            this.ctxNotification.Name = "ctxNotification";
            this.ctxNotification.ShowCheckMargin = true;
            this.ctxNotification.ShowImageMargin = false;
            this.ctxNotification.Size = new System.Drawing.Size(149, 82);
            this.ctxNotification.Opening += new System.ComponentModel.CancelEventHandler(this.ctxNotification_Opening);
            // 
            // cmnuProxySettings
            // 
            this.cmnuProxySettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmnuProxySettingsChange});
            this.cmnuProxySettings.Name = "cmnuProxySettings";
            this.cmnuProxySettings.Size = new System.Drawing.Size(148, 22);
            this.cmnuProxySettings.Text = "Proxy Settings";
            this.cmnuProxySettings.DropDownOpening += new System.EventHandler(this.cmnuProxySettings_DropDownOpening);
            this.cmnuProxySettings.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmnuProxySettings_DropDownItemClicked);
            // 
            // cmnuProxySettingsChange
            // 
            this.cmnuProxySettingsChange.Name = "cmnuProxySettingsChange";
            this.cmnuProxySettingsChange.Size = new System.Drawing.Size(169, 22);
            this.cmnuProxySettingsChange.Text = "&Change Settings...";
            // 
            // ctxNotifSep1
            // 
            this.ctxNotifSep1.Name = "ctxNotifSep1";
            this.ctxNotifSep1.Size = new System.Drawing.Size(145, 6);
            this.ctxNotifSep1.Visible = false;
            // 
            // cmnuNotifAutoStart
            // 
            this.cmnuNotifAutoStart.CheckOnClick = true;
            this.cmnuNotifAutoStart.Name = "cmnuNotifAutoStart";
            this.cmnuNotifAutoStart.Size = new System.Drawing.Size(148, 22);
            this.cmnuNotifAutoStart.Text = "&Auto Start";
            this.cmnuNotifAutoStart.CheckedChanged += new System.EventHandler(this.cmnuNotifAutoStart_CheckedChanged);
            // 
            // ctxNotifSep2
            // 
            this.ctxNotifSep2.Name = "ctxNotifSep2";
            this.ctxNotifSep2.Size = new System.Drawing.Size(145, 6);
            // 
            // cmnuNotifExit
            // 
            this.cmnuNotifExit.Name = "cmnuNotifExit";
            this.cmnuNotifExit.Size = new System.Drawing.Size(148, 22);
            this.cmnuNotifExit.Text = "E&xit";
            this.cmnuNotifExit.Click += new System.EventHandler(this.cmnuNotifExit_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(176, 112);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ctxNotification.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip ctxNotification;
        private System.Windows.Forms.ToolStripSeparator ctxNotifSep1;
        private System.Windows.Forms.ToolStripMenuItem cmnuNotifAutoStart;
        private System.Windows.Forms.ToolStripSeparator ctxNotifSep2;
        private System.Windows.Forms.ToolStripMenuItem cmnuNotifExit;
        private System.Windows.Forms.ToolStripMenuItem cmnuProxySettings;
        private System.Windows.Forms.ToolStripMenuItem cmnuProxySettingsChange;

    }
}

