namespace ProxyManager
{
    partial class dlgProxySettings
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
            this.txtAutoConfigUrl = new System.Windows.Forms.TextBox();
            this.chkNoLocalProxy = new System.Windows.Forms.CheckBox();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.txtProxyAddress = new System.Windows.Forms.WatermarkTextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.lblAutoConfigUrl = new System.Windows.Forms.Label();
            this.lblHttpProxy = new System.Windows.Forms.Label();
            this.txtProxyPort = new System.Windows.Forms.WatermarkTextBox();
            this.btnAdvancedProxy = new System.Windows.Forms.Button();
            this.tlpButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tlpMain.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAutoConfigUrl
            // 
            this.tlpMain.SetColumnSpan(this.txtAutoConfigUrl, 3);
            this.txtAutoConfigUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAutoConfigUrl.Location = new System.Drawing.Point(117, 34);
            this.txtAutoConfigUrl.Name = "txtAutoConfigUrl";
            this.txtAutoConfigUrl.Size = new System.Drawing.Size(214, 25);
            this.txtAutoConfigUrl.TabIndex = 3;
            // 
            // chkNoLocalProxy
            // 
            this.chkNoLocalProxy.AutoSize = true;
            this.tlpMain.SetColumnSpan(this.chkNoLocalProxy, 3);
            this.chkNoLocalProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkNoLocalProxy.Location = new System.Drawing.Point(117, 96);
            this.chkNoLocalProxy.Name = "chkNoLocalProxy";
            this.chkNoLocalProxy.Size = new System.Drawing.Size(214, 21);
            this.chkNoLocalProxy.TabIndex = 8;
            this.chkNoLocalProxy.Text = "&No proxy for local addresses";
            this.chkNoLocalProxy.UseVisualStyleBackColor = true;
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 4;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.Controls.Add(this.chkNoLocalProxy, 1, 3);
            this.tlpMain.Controls.Add(this.cboType, 1, 0);
            this.tlpMain.Controls.Add(this.txtProxyAddress, 1, 2);
            this.tlpMain.Controls.Add(this.lblType, 0, 0);
            this.tlpMain.Controls.Add(this.txtAutoConfigUrl, 1, 1);
            this.tlpMain.Controls.Add(this.lblAutoConfigUrl, 0, 1);
            this.tlpMain.Controls.Add(this.lblHttpProxy, 0, 2);
            this.tlpMain.Controls.Add(this.txtProxyPort, 2, 2);
            this.tlpMain.Controls.Add(this.btnAdvancedProxy, 3, 2);
            this.tlpMain.Controls.Add(this.tlpButtons, 1, 4);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 5;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(334, 137);
            this.tlpMain.TabIndex = 0;
            // 
            // cboType
            // 
            this.tlpMain.SetColumnSpan(this.cboType, 3);
            this.cboType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Location = new System.Drawing.Point(117, 3);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(214, 25);
            this.cboType.TabIndex = 1;
            this.cboType.SelectedValueChanged += new System.EventHandler(this.cboType_SelectedValueChanged);
            // 
            // txtProxyAddress
            // 
            this.txtProxyAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyAddress.Location = new System.Drawing.Point(117, 65);
            this.txtProxyAddress.Name = "txtProxyAddress";
            this.txtProxyAddress.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProxyAddress.PromptText = "Address";
            this.txtProxyAddress.Size = new System.Drawing.Size(125, 25);
            this.txtProxyAddress.TabIndex = 5;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblType.Location = new System.Drawing.Point(3, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(108, 31);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Connection Type:";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAutoConfigUrl
            // 
            this.lblAutoConfigUrl.AutoSize = true;
            this.lblAutoConfigUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAutoConfigUrl.Location = new System.Drawing.Point(3, 31);
            this.lblAutoConfigUrl.Name = "lblAutoConfigUrl";
            this.lblAutoConfigUrl.Size = new System.Drawing.Size(108, 31);
            this.lblAutoConfigUrl.TabIndex = 2;
            this.lblAutoConfigUrl.Text = "Auto Config URL:";
            this.lblAutoConfigUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHttpProxy
            // 
            this.lblHttpProxy.AutoSize = true;
            this.lblHttpProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHttpProxy.Location = new System.Drawing.Point(3, 62);
            this.lblHttpProxy.Name = "lblHttpProxy";
            this.lblHttpProxy.Size = new System.Drawing.Size(108, 31);
            this.lblHttpProxy.TabIndex = 4;
            this.lblHttpProxy.Text = "&Proxy Server:";
            this.lblHttpProxy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtProxyPort
            // 
            this.txtProxyPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProxyPort.Location = new System.Drawing.Point(248, 65);
            this.txtProxyPort.Name = "txtProxyPort";
            this.txtProxyPort.PromptFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProxyPort.PromptText = "Port";
            this.txtProxyPort.Size = new System.Drawing.Size(50, 25);
            this.txtProxyPort.TabIndex = 6;
            // 
            // btnAdvancedProxy
            // 
            this.btnAdvancedProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdvancedProxy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdvancedProxy.Location = new System.Drawing.Point(304, 65);
            this.btnAdvancedProxy.Name = "btnAdvancedProxy";
            this.btnAdvancedProxy.Size = new System.Drawing.Size(27, 25);
            this.btnAdvancedProxy.TabIndex = 7;
            this.btnAdvancedProxy.Text = "...";
            this.btnAdvancedProxy.UseVisualStyleBackColor = true;
            this.btnAdvancedProxy.Click += new System.EventHandler(this.btnAdvancedProxy_Click);
            // 
            // tlpButtons
            // 
            this.tlpButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpButtons.AutoSize = true;
            this.tlpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpButtons.ColumnCount = 2;
            this.tlpMain.SetColumnSpan(this.tlpButtons, 3);
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButtons.Controls.Add(this.btnOk, 0, 0);
            this.tlpButtons.Controls.Add(this.btnCancel, 1, 0);
            this.tlpButtons.Location = new System.Drawing.Point(169, 123);
            this.tlpButtons.Name = "tlpButtons";
            this.tlpButtons.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.tlpButtons.RowCount = 1;
            this.tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButtons.Size = new System.Drawing.Size(162, 39);
            this.tlpButtons.TabIndex = 9;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(3, 11);
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
            this.btnCancel.Location = new System.Drawing.Point(84, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // dlgProxySettings
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(334, 137);
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgProxySettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connection Settings";
            this.Shown += new System.EventHandler(this.dlgProxySettings_Shown);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkNoLocalProxy;
        private System.Windows.Forms.TextBox txtAutoConfigUrl;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblAutoConfigUrl;
        private System.Windows.Forms.WatermarkTextBox txtProxyAddress;
        private System.Windows.Forms.WatermarkTextBox txtProxyPort;
        private System.Windows.Forms.Label lblHttpProxy;
        private System.Windows.Forms.Button btnAdvancedProxy;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpButtons;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;

    }
}