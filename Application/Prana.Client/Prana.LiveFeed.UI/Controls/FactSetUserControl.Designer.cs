namespace Prana.LiveFeed.UI.Controls
{
    partial class FactSetUserControl
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
            this.factSetWebsiteView = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.factSetWebsiteView)).BeginInit();
            this.SuspendLayout();
            // 
            // factSetWebsiteView
            // 
            this.factSetWebsiteView.AllowExternalDrop = true;
            this.factSetWebsiteView.CreationProperties = null;
            this.factSetWebsiteView.DefaultBackgroundColor = System.Drawing.Color.White;
            this.factSetWebsiteView.Location = new System.Drawing.Point(14, 15);
            this.factSetWebsiteView.Name = "factSetWebsiteView";
            this.factSetWebsiteView.Size = new System.Drawing.Size(815, 517);
            this.factSetWebsiteView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.factSetWebsiteView.TabIndex = 1;
            this.factSetWebsiteView.ZoomFactor = 1D;
            this.factSetWebsiteView.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(FactSetWebsiteView_NavigationCompleted);   
            // 
            // FactSetUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.factSetWebsiteView);
            this.Name = "FactSetUserControl";
            this.Size = new System.Drawing.Size(846, 545);
            ((System.ComponentModel.ISupportInitialize)(this.factSetWebsiteView)).EndInit();
            this.ResumeLayout(false);

        }


        private Microsoft.Web.WebView2.WinForms.WebView2 factSetWebsiteView;
        #endregion
    }
}
