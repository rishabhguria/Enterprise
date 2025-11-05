namespace Prana.MonitoringServices
{
    partial class ErrorDisplayCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstbxMessages = new System.Windows.Forms.ListBox();
            this.serverStatusCtrl1 = new Prana.MonitoringServices.ServerStatusCtrl();
            this.SuspendLayout();
            // 
            // lstbxMessages
            // 
            this.lstbxMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstbxMessages.FormattingEnabled = true;
            this.lstbxMessages.Items.AddRange(new object[] {
            ""});
            this.lstbxMessages.Location = new System.Drawing.Point(3, 68);
            this.lstbxMessages.Name = "lstbxMessages";
            this.lstbxMessages.Size = new System.Drawing.Size(595, 212);
            this.lstbxMessages.TabIndex = 0;
            // 
            // serverStatusCtrl1
            // 
            this.serverStatusCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.serverStatusCtrl1.Location = new System.Drawing.Point(0, 3);
            this.serverStatusCtrl1.Name = "serverStatusCtrl1";
            this.serverStatusCtrl1.Size = new System.Drawing.Size(598, 66);
            this.serverStatusCtrl1.TabIndex = 1;
            // 
            // ErrorDisplayCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.serverStatusCtrl1);
            this.Controls.Add(this.lstbxMessages);
            this.Name = "ErrorDisplayCtrl";
            this.Size = new System.Drawing.Size(601, 280);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstbxMessages;
        private ServerStatusCtrl serverStatusCtrl1;
    }
}
