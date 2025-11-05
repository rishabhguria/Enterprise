namespace Prana.PricingService2UI.MarketDataMonitoring
{
    partial class SAPIProperties
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraGroupBoxClientDetails = new Infragistics.Win.Misc.UltraGroupBox();
            this.ErrorMessageBar = new System.Windows.Forms.Label();
            this.tableLayoutPanelClient = new System.Windows.Forms.TableLayoutPanel();
            this.lblPortNumber = new System.Windows.Forms.Label();
            this.txtBoxPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblServerAddress = new System.Windows.Forms.Label();
            this.txtBoxServerAddress = new System.Windows.Forms.TextBox();
            this.txtBoxPortNumber = new System.Windows.Forms.TextBox();
            this.ultraPanelBottom = new Infragistics.Win.Misc.UltraPanel();
            this.tableLayoutPanelBottom = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBoxClientDetails)).BeginInit();
            this.ultraGroupBoxClientDetails.SuspendLayout();
            this.tableLayoutPanelClient.SuspendLayout();
            this.ultraPanelBottom.ClientArea.SuspendLayout();
            this.ultraPanelBottom.SuspendLayout();
            this.tableLayoutPanelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraGroupBoxClientDetails
            // 
            appearance1.BorderColor = System.Drawing.Color.Black;
            this.ultraGroupBoxClientDetails.ContentAreaAppearance = appearance1;
            this.ultraGroupBoxClientDetails.Controls.Add(this.ErrorMessageBar);
            this.ultraGroupBoxClientDetails.Controls.Add(this.tableLayoutPanelClient);
            this.ultraGroupBoxClientDetails.Location = new System.Drawing.Point(12, 12);
            this.ultraGroupBoxClientDetails.Name = "ultraGroupBoxClientDetails";
            this.ultraGroupBoxClientDetails.Size = new System.Drawing.Size(406, 139);
            this.ultraGroupBoxClientDetails.TabIndex = 7;
            this.ultraGroupBoxClientDetails.Text = "Credentials";
            // 
            // ErrorMessageBar
            // 
            this.ErrorMessageBar.AutoSize = true;
            this.ErrorMessageBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.ErrorMessageBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ErrorMessageBar.Location = new System.Drawing.Point(9, 261);
            this.ErrorMessageBar.Name = "ErrorMessageBar";
            this.ErrorMessageBar.Size = new System.Drawing.Size(0, 15);
            this.ErrorMessageBar.TabIndex = 8;
            // 
            // tableLayoutPanelClient
            // 
            this.tableLayoutPanelClient.ColumnCount = 2;
            this.tableLayoutPanelClient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelClient.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelClient.Controls.Add(this.lblPortNumber, 0, 2);
            this.tableLayoutPanelClient.Controls.Add(this.txtBoxPassword, 1, 1);
            this.tableLayoutPanelClient.Controls.Add(this.lblPassword, 0, 1);
            this.tableLayoutPanelClient.Controls.Add(this.lblServerAddress, 0, 0);
            this.tableLayoutPanelClient.Controls.Add(this.txtBoxServerAddress, 1, 0);
            this.tableLayoutPanelClient.Controls.Add(this.txtBoxPortNumber, 1, 2);
            this.tableLayoutPanelClient.Location = new System.Drawing.Point(11, 23);
            this.tableLayoutPanelClient.Name = "tableLayoutPanelClient";
            this.tableLayoutPanelClient.RowCount = 3;
            this.tableLayoutPanelClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelClient.Size = new System.Drawing.Size(385, 90);
            this.tableLayoutPanelClient.TabIndex = 6;
            // 
            // txtBoxPassword
            // 
            this.txtBoxPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxPassword.Location = new System.Drawing.Point(195, 41);
            this.txtBoxPassword.Name = "txtBoxPassword";
            this.txtBoxPassword.Size = new System.Drawing.Size(187, 20);
            this.txtBoxPassword.TabIndex = 5;
            this.txtBoxPassword.UseSystemPasswordChar = true;
            //
            // lblPortNumber
            // 
            this.lblPortNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPortNumber.AutoSize = true;
            this.lblPortNumber.Location = new System.Drawing.Point(30, 68);
            this.lblPortNumber.Margin = new System.Windows.Forms.Padding(30, 0, 3, 0);
            this.lblPortNumber.Name = "lblPortNumber";
            this.lblPortNumber.Size = new System.Drawing.Size(70, 13);
            this.lblPortNumber.TabIndex = 6;
            this.lblPortNumber.Text = "Port Number*";
            // 
            // lblPassword
            // 
            this.lblPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(30, 45);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(30, 0, 3, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password*";
            // 
            // lblServerAddress
            // 
            this.lblServerAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblServerAddress.AutoSize = true;
            this.lblServerAddress.Location = new System.Drawing.Point(30, 10);
            this.lblServerAddress.Margin = new System.Windows.Forms.Padding(30, 0, 3, 0);
            this.lblServerAddress.Name = "lblServerAddress";
            this.lblServerAddress.Size = new System.Drawing.Size(59, 13);
            this.lblServerAddress.TabIndex = 0;
            this.lblServerAddress.Text = "Server Address*";
            // 
            // txtBoxlblServerAddress
            // 
            this.txtBoxServerAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxServerAddress.Location = new System.Drawing.Point(195, 7);
            this.txtBoxServerAddress.Name = "txtBoxlblServerAddress";
            this.txtBoxServerAddress.Size = new System.Drawing.Size(187, 20);
            this.txtBoxServerAddress.TabIndex = 4;
            // 
            // txtBoxPortNumber
            // 
            this.txtBoxPortNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxPortNumber.Location = new System.Drawing.Point(195, 65);
            this.txtBoxPortNumber.Name = "textBoxPortNumber";
            this.txtBoxPortNumber.Size = new System.Drawing.Size(187, 20);
            this.txtBoxPortNumber.TabIndex = 7;
            //
            // ultraPanelBottom
            // 
            // 
            // ultraPanelBottom.ClientArea
            // 
            this.ultraPanelBottom.ClientArea.Controls.Add(this.tableLayoutPanelBottom);
            this.ultraPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanelBottom.Location = new System.Drawing.Point(0, 136);
            this.ultraPanelBottom.Name = "ultraPanelBottom";
            this.ultraPanelBottom.Size = new System.Drawing.Size(431, 36);
            this.ultraPanelBottom.TabIndex = 13;
            // 
            // tableLayoutPanelBottom
            // 
            this.tableLayoutPanelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelBottom.ColumnCount = 2;
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.Controls.Add(this.buttonCancel, 1, 0);
            this.tableLayoutPanelBottom.Controls.Add(this.buttonOk, 0, 0);
            this.tableLayoutPanelBottom.Location = new System.Drawing.Point(91, 3);
            this.tableLayoutPanelBottom.Name = "tableLayoutPanelBottom";
            this.tableLayoutPanelBottom.RowCount = 1;
            this.tableLayoutPanelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelBottom.Size = new System.Drawing.Size(255, 30);
            this.tableLayoutPanelBottom.TabIndex = 7;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCancel.Location = new System.Drawing.Point(150, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonOk.Location = new System.Drawing.Point(25, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // ActivProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 227);
            this.Controls.Add(this.ultraPanelBottom);
            this.Controls.Add(this.ultraGroupBoxClientDetails);
            this.MaximizeBox = false;
            this.Name = "BloombergProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bloomberg Properties";
            this.Load += new System.EventHandler(this.SAPIProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBoxClientDetails)).EndInit();
            this.ultraGroupBoxClientDetails.ResumeLayout(false);
            this.ultraGroupBoxClientDetails.PerformLayout();
            this.tableLayoutPanelClient.ResumeLayout(false);
            this.tableLayoutPanelClient.PerformLayout();
            this.ultraPanelBottom.ClientArea.ResumeLayout(false);
            this.ultraPanelBottom.ResumeLayout(false);
            this.tableLayoutPanelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBoxClientDetails;
        private System.Windows.Forms.Label ErrorMessageBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelClient;
        private System.Windows.Forms.TextBox txtBoxPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblServerAddress;
        private System.Windows.Forms.TextBox txtBoxServerAddress;
        private Infragistics.Win.Misc.UltraPanel ultraPanelBottom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBottom;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label lblPortNumber;
        private System.Windows.Forms.TextBox txtBoxPortNumber;
    }
}