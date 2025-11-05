namespace Prana.RuleEngine
{
    partial class OpenWebkitWebControl
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
            this.webBrowser = new WebKit.WebKitBrowser();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.AllowDrop = true;
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.BackColor = System.Drawing.Color.White;
            this.webBrowser.Location = new System.Drawing.Point(0, 3);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.PrivateBrowsing = false;
            this.webBrowser.Size = new System.Drawing.Size(659, 354);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.Url = null;
            // 
            // OpenWebkitControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.webBrowser);
            this.Name = "OpenWebkitControl";
            this.Size = new System.Drawing.Size(662, 360);
            this.ResumeLayout(false);

        }

        #endregion

        internal WebKit.WebKitBrowser webBrowser;
    }
}
