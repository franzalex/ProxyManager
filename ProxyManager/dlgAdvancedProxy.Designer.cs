namespace ProxyManager
{
    partial class dlgAdvancedProxy
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
            System.Windows.Forms.TableLayoutPanel tlpMain;
            System.Windows.Forms.Label lblHttpProxy;
            System.Windows.Forms.Label lblHttpsProxy;
            System.Windows.Forms.Label lblSocksProxy;
            System.Windows.Forms.Label lblFtpProxy;
            System.Windows.Forms.GroupBox grpExceptions;
            System.Windows.Forms.TableLayoutPanel tlpButtons;
            this.txtSocksAddress = new System.Windows.Forms.WatermarkTextBox();
            this.txtHttpAddress = new System.Windows.Forms.WatermarkTextBox();
            this.txtHttpPort = new System.Windows.Forms.WatermarkTextBox();
            this.txtHttpsPort = new System.Windows.Forms.WatermarkTextBox();
            this.txtFtpPort = new System.Windows.Forms.WatermarkTextBox();
            this.txtHttpsAddress = new System.Windows.Forms.WatermarkTextBox();
            this.txtSocksPort = new System.Windows.Forms.WatermarkTextBox();
            this.txtFtpAddress = new System.Windows.Forms.WatermarkTextBox();
            this.txtExceptions = new System.Windows.Forms.WatermarkTextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkSingleProxy = new System.Windows.Forms.CheckBox();
            tlpMain = new System.Windows.Forms.TableLayoutPanel();
            lblHttpProxy = new System.Windows.Forms.Label();
            lblHttpsProxy = new System.Windows.Forms.Label();
            lblSocksProxy = new System.Windows.Forms.Label();
            lblFtpProxy = new System.Windows.Forms.Label();
            grpExceptions = new System.Windows.Forms.GroupBox();
            tlpButtons = new System.Windows.Forms.TableLayoutPanel();
            tlpMain.SuspendLayout();
            grpExceptions.SuspendLayout();
            tlpButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 3;
            tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            tlpMain.Controls.Add(this.txtSocksAddress, 1, 4);
            tlpMain.Controls.Add(this.txtHttpAddress, 1, 0);
            tlpMain.Controls.Add(lblHttpProxy, 0, 0);
            tlpMain.Controls.Add(this.txtHttpPort, 2, 0);
            tlpMain.Controls.Add(this.txtHttpsPort, 1, 2);
            tlpMain.Controls.Add(this.txtFtpPort, 1, 3);
            tlpMain.Controls.Add(this.txtHttpsAddress, 1, 2);
            tlpMain.Controls.Add(lblHttpsProxy, 0, 2);
            tlpMain.Controls.Add(this.txtSocksPort, 1, 4);
            tlpMain.Controls.Add(lblSocksProxy, 0, 4);
            tlpMain.Controls.Add(this.txtFtpAddress, 1, 3);
            tlpMain.Controls.Add(lblFtpProxy, 0, 3);
            tlpMain.Controls.Add(grpExceptions, 1, 5);
            tlpMain.Controls.Add(tlpButtons, 1, 6);
            tlpMain.Controls.Add(this.chkSingleProxy, 1, 1);
            tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpMain.Location = new System.Drawing.Point(0, 0);
            tlpMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 7;
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpMain.Size = new System.Drawing.Size(300, 300);
            tlpMain.TabIndex = 0;
            // 
            // txtSocksAddress
            // 
            this.txtSocksAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSocksAddress.Location = new System.Drawing.Point(57, 132);
            this.txtSocksAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSocksAddress.Name = "txtSocksAddress";
            this.txtSocksAddress.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSocksAddress.PromptText = "Address";
            this.txtSocksAddress.Size = new System.Drawing.Size(172, 25);
            this.txtSocksAddress.TabIndex = 11;
            // 
            // txtHttpAddress
            // 
            this.txtHttpAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHttpAddress.Location = new System.Drawing.Point(57, 4);
            this.txtHttpAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHttpAddress.Name = "txtHttpAddress";
            this.txtHttpAddress.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHttpAddress.PromptText = "Address";
            this.txtHttpAddress.Size = new System.Drawing.Size(172, 25);
            this.txtHttpAddress.TabIndex = 1;
            // 
            // lblHttpProxy
            // 
            lblHttpProxy.AutoSize = true;
            lblHttpProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            lblHttpProxy.Location = new System.Drawing.Point(3, 0);
            lblHttpProxy.Name = "lblHttpProxy";
            lblHttpProxy.Size = new System.Drawing.Size(48, 33);
            lblHttpProxy.TabIndex = 0;
            lblHttpProxy.Text = "&HTTP:";
            lblHttpProxy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHttpPort
            // 
            this.txtHttpPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHttpPort.Location = new System.Drawing.Point(235, 4);
            this.txtHttpPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHttpPort.Name = "txtHttpPort";
            this.txtHttpPort.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHttpPort.PromptText = "Port";
            this.txtHttpPort.Size = new System.Drawing.Size(62, 25);
            this.txtHttpPort.TabIndex = 2;
            // 
            // txtHttpsPort
            // 
            this.txtHttpsPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHttpsPort.Location = new System.Drawing.Point(235, 66);
            this.txtHttpsPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHttpsPort.Name = "txtHttpsPort";
            this.txtHttpsPort.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHttpsPort.PromptText = "Port";
            this.txtHttpsPort.Size = new System.Drawing.Size(62, 25);
            this.txtHttpsPort.TabIndex = 6;
            // 
            // txtFtpPort
            // 
            this.txtFtpPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFtpPort.Location = new System.Drawing.Point(235, 99);
            this.txtFtpPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFtpPort.Name = "txtFtpPort";
            this.txtFtpPort.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFtpPort.PromptText = "Port";
            this.txtFtpPort.Size = new System.Drawing.Size(62, 25);
            this.txtFtpPort.TabIndex = 9;
            // 
            // txtHttpsAddress
            // 
            this.txtHttpsAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHttpsAddress.Location = new System.Drawing.Point(57, 66);
            this.txtHttpsAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtHttpsAddress.Name = "txtHttpsAddress";
            this.txtHttpsAddress.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHttpsAddress.PromptText = "Address";
            this.txtHttpsAddress.Size = new System.Drawing.Size(172, 25);
            this.txtHttpsAddress.TabIndex = 5;
            // 
            // lblHttpsProxy
            // 
            lblHttpsProxy.AutoSize = true;
            lblHttpsProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            lblHttpsProxy.Location = new System.Drawing.Point(3, 62);
            lblHttpsProxy.Name = "lblHttpsProxy";
            lblHttpsProxy.Size = new System.Drawing.Size(48, 33);
            lblHttpsProxy.TabIndex = 4;
            lblHttpsProxy.Text = "HTTP&S:";
            lblHttpsProxy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSocksPort
            // 
            this.txtSocksPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSocksPort.Location = new System.Drawing.Point(235, 132);
            this.txtSocksPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSocksPort.Name = "txtSocksPort";
            this.txtSocksPort.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSocksPort.PromptText = "Port";
            this.txtSocksPort.Size = new System.Drawing.Size(62, 25);
            this.txtSocksPort.TabIndex = 12;
            // 
            // lblSocksProxy
            // 
            lblSocksProxy.AutoSize = true;
            lblSocksProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSocksProxy.Location = new System.Drawing.Point(3, 128);
            lblSocksProxy.Name = "lblSocksProxy";
            lblSocksProxy.Size = new System.Drawing.Size(48, 33);
            lblSocksProxy.TabIndex = 10;
            lblSocksProxy.Text = "So&cks:";
            lblSocksProxy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFtpAddress
            // 
            this.txtFtpAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFtpAddress.Location = new System.Drawing.Point(57, 99);
            this.txtFtpAddress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFtpAddress.Name = "txtFtpAddress";
            this.txtFtpAddress.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFtpAddress.PromptText = "Address";
            this.txtFtpAddress.Size = new System.Drawing.Size(172, 25);
            this.txtFtpAddress.TabIndex = 8;
            // 
            // lblFtpProxy
            // 
            lblFtpProxy.AutoSize = true;
            lblFtpProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            lblFtpProxy.Location = new System.Drawing.Point(3, 95);
            lblFtpProxy.Name = "lblFtpProxy";
            lblFtpProxy.Size = new System.Drawing.Size(48, 33);
            lblFtpProxy.TabIndex = 7;
            lblFtpProxy.Text = "&FTP:";
            lblFtpProxy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpExceptions
            // 
            tlpMain.SetColumnSpan(grpExceptions, 2);
            grpExceptions.Controls.Add(this.txtExceptions);
            grpExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            grpExceptions.Location = new System.Drawing.Point(57, 164);
            grpExceptions.Name = "grpExceptions";
            grpExceptions.Size = new System.Drawing.Size(240, 100);
            grpExceptions.TabIndex = 13;
            grpExceptions.TabStop = false;
            grpExceptions.Text = "&Don\'t use proxy for these addresses";
            // 
            // txtExceptions
            // 
            this.txtExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExceptions.Location = new System.Drawing.Point(3, 21);
            this.txtExceptions.Multiline = true;
            this.txtExceptions.Name = "txtExceptions";
            this.txtExceptions.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExceptions.PromptText = "Use a semi colon ( ; ) to separate entries.";
            this.txtExceptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtExceptions.Size = new System.Drawing.Size(234, 76);
            this.txtExceptions.TabIndex = 0;
            this.txtExceptions.Enter += new System.EventHandler(this.txtExceptions_EnterLeave);
            this.txtExceptions.Leave += new System.EventHandler(this.txtExceptions_EnterLeave);
            // 
            // tlpButtons
            // 
            tlpButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            tlpButtons.AutoSize = true;
            tlpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tlpButtons.ColumnCount = 2;
            tlpMain.SetColumnSpan(tlpButtons, 2);
            tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tlpButtons.Controls.Add(this.btnOk, 0, 0);
            tlpButtons.Controls.Add(this.btnCancel, 1, 0);
            tlpButtons.Location = new System.Drawing.Point(135, 270);
            tlpButtons.Name = "tlpButtons";
            tlpButtons.RowCount = 1;
            tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tlpButtons.Size = new System.Drawing.Size(162, 31);
            tlpButtons.TabIndex = 14;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(3, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(84, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkSingleProxy
            // 
            this.chkSingleProxy.AutoSize = true;
            tlpMain.SetColumnSpan(this.chkSingleProxy, 2);
            this.chkSingleProxy.Location = new System.Drawing.Point(57, 37);
            this.chkSingleProxy.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkSingleProxy.Name = "chkSingleProxy";
            this.chkSingleProxy.Size = new System.Drawing.Size(208, 21);
            this.chkSingleProxy.TabIndex = 3;
            this.chkSingleProxy.Text = "&Use this proxy for all protocols";
            this.chkSingleProxy.UseVisualStyleBackColor = true;
            this.chkSingleProxy.CheckedChanged += new System.EventHandler(this.chkSingleProxy_CheckedChanged);
            // 
            // dlgAdvancedProxy
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(tlpMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgAdvancedProxy";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Proxy Settings - Advanced";
            tlpMain.ResumeLayout(false);
            tlpMain.PerformLayout();
            grpExceptions.ResumeLayout(false);
            grpExceptions.PerformLayout();
            tlpButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WatermarkTextBox txtSocksAddress;
        private System.Windows.Forms.CheckBox chkSingleProxy;
        private System.Windows.Forms.WatermarkTextBox txtHttpAddress;
        private System.Windows.Forms.WatermarkTextBox txtHttpPort;
        private System.Windows.Forms.WatermarkTextBox txtHttpsPort;
        private System.Windows.Forms.WatermarkTextBox txtFtpPort;
        private System.Windows.Forms.WatermarkTextBox txtHttpsAddress;
        private System.Windows.Forms.WatermarkTextBox txtSocksPort;
        private System.Windows.Forms.WatermarkTextBox txtFtpAddress;
        private System.Windows.Forms.WatermarkTextBox txtExceptions;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}