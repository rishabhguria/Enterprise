namespace Prana.LiveFeedEngine
{
	partial class LivefeedConnection
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.pbLiveFeedConnectionStatus = new Infragistics.Win.UltraWinEditors.UltraPictureBox();
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.SuspendLayout();
            // 
            // pbLiveFeedConnectionStatus
            // 
            appearance1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbLiveFeedConnectionStatus.Appearance = appearance1;
            this.pbLiveFeedConnectionStatus.BorderShadowColor = System.Drawing.Color.Empty;
            this.pbLiveFeedConnectionStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbLiveFeedConnectionStatus.Location = new System.Drawing.Point(0, 0);
            this.pbLiveFeedConnectionStatus.Name = "pbLiveFeedConnectionStatus";
            this.pbLiveFeedConnectionStatus.Size = new System.Drawing.Size(19, 21);
            this.pbLiveFeedConnectionStatus.TabIndex = 35;
            // 
            // ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;
            // 
            // LivefeedConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pbLiveFeedConnectionStatus);
            this.Name = "LivefeedConnection";
            this.Size = new System.Drawing.Size(19, 22);
            this.ResumeLayout(false);

		}

		#endregion

        private Infragistics.Win.UltraWinEditors.UltraPictureBox pbLiveFeedConnectionStatus;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
	}
}
