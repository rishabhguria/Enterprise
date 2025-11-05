namespace Prana.PricingService2UI.SecondaryMarketDataMonitoring
{
    partial class BloombergProperties
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
            this.txtBoxPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtBoxUsername = new System.Windows.Forms.TextBox();
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
            this.ultraGroupBoxClientDetails.Location = new System.Drawing.Point(12, 6);
            this.ultraGroupBoxClientDetails.Name = "ultraGroupBoxClientDetails";
            this.ultraGroupBoxClientDetails.Size = new System.Drawing.Size(406, 105);
            this.ultraGroupBoxClientDetails.TabIndex = 14;
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
            this.tableLayoutPanelClient.Controls.Add(this.txtBoxPassword, 1, 1);
            this.tableLayoutPanelClient.Controls.Add(this.lblPassword, 0, 1);
            this.tableLayoutPanelClient.Controls.Add(this.lblUsername, 0, 0);
            this.tableLayoutPanelClient.Controls.Add(this.txtBoxUsername, 1, 0);
            this.tableLayoutPanelClient.Location = new System.Drawing.Point(11, 23);
            this.tableLayoutPanelClient.Name = "tableLayoutPanelClient";
            this.tableLayoutPanelClient.RowCount = 2;
            this.tableLayoutPanelClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelClient.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelClient.Size = new System.Drawing.Size(385, 69);
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
            // lblUsername
            // 
            this.lblUsername.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(30, 10);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(30, 0, 3, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(59, 13);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username*";
            // 
            // txtBoxUsername
            // 
            this.txtBoxUsername.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtBoxUsername.Location = new System.Drawing.Point(195, 7);
            this.txtBoxUsername.Name = "txtBoxUsername";
            this.txtBoxUsername.Size = new System.Drawing.Size(187, 20);
            this.txtBoxUsername.TabIndex = 4;
            // 
            // ultraPanelBottom
            // 
            // 
            // ultraPanelBottom.ClientArea
            // 
            this.ultraPanelBottom.ClientArea.Controls.Add(this.tableLayoutPanelBottom);
            this.ultraPanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanelBottom.Location = new System.Drawing.Point(0, 137);
            this.ultraPanelBottom.Name = "ultraPanelBottom";
            this.ultraPanelBottom.Size = new System.Drawing.Size(427, 36);
            this.ultraPanelBottom.TabIndex = 15;
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
            this.tableLayoutPanelBottom.Size = new System.Drawing.Size(246, 30);
            this.tableLayoutPanelBottom.TabIndex = 7;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCancel.Location = new System.Drawing.Point(147, 3);
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
            this.buttonOk.Location = new System.Drawing.Point(24, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // BloombergProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 173);
            this.Controls.Add(this.ultraGroupBoxClientDetails);
            this.Controls.Add(this.ultraPanelBottom);
            this.Name = "BloombergProperties";
            this.Text = "Bloomberg Connection Properties";
            this.Load += new System.EventHandler(this.BloombergProperties_Load);
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
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtBoxUsername;
        private Infragistics.Win.Misc.UltraPanel ultraPanelBottom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelBottom;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
    }
}
