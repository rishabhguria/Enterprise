namespace Prana.LiveFeed.UI.Controls
{
    partial class QTTL1Strip
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
            CleanUp();
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_marketDataHelperInstance != null)
                {
                    _marketDataHelperInstance.OnResponse -= LevelOne_OnResponse;
                    _marketDataHelperInstance.Dispose();
                    _marketDataHelperInstance = null;
                }
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
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.titleDayPL = new System.Windows.Forms.Label();
            this.titleExposure = new System.Windows.Forms.Label();
            this.titlePosition = new System.Windows.Forms.Label();
            this.titleAsk = new System.Windows.Forms.Label();
            this.titleBid = new System.Windows.Forms.Label();
            this.titleChange = new System.Windows.Forms.Label();
            this.titleLast = new System.Windows.Forms.Label();
            this.lblDayPL = new System.Windows.Forms.Label();
            this.lblExposure = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblAsk = new System.Windows.Forms.Label();
            this.lblBid = new System.Windows.Forms.Label();
            this.lblChange = new System.Windows.Forms.Label();
            this.lblLast = new System.Windows.Forms.Label();
            this.lblSplitter1 = new System.Windows.Forms.Label();
            this.lblSplitter2 = new System.Windows.Forms.Label();
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.mainLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 9;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.mainLayoutPanel.Controls.Add(this.titleDayPL, 8, 0);
            this.mainLayoutPanel.Controls.Add(this.lblSplitter2, 7, 0);
            this.mainLayoutPanel.Controls.Add(this.titleExposure, 6, 0);
            this.mainLayoutPanel.Controls.Add(this.titlePosition, 5, 0);
            this.mainLayoutPanel.Controls.Add(this.lblSplitter1, 4, 0);
            this.mainLayoutPanel.Controls.Add(this.titleAsk, 3, 0);
            this.mainLayoutPanel.Controls.Add(this.titleBid, 2, 0);
            this.mainLayoutPanel.Controls.Add(this.titleChange, 1, 0);
            this.mainLayoutPanel.Controls.Add(this.titleLast, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.lblDayPL, 8, 1);
            this.mainLayoutPanel.Controls.Add(this.lblExposure, 6, 1);
            this.mainLayoutPanel.Controls.Add(this.lblPosition, 5, 1);
            this.mainLayoutPanel.Controls.Add(this.lblAsk, 3, 1);
            this.mainLayoutPanel.Controls.Add(this.lblBid, 2, 1);
            this.mainLayoutPanel.Controls.Add(this.lblChange, 1, 1);
            this.mainLayoutPanel.Controls.Add(this.lblLast, 0, 1);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(526, 48);
            this.inboxControlStyler1.SetStyleSettings(this.mainLayoutPanel, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // lblSplitter1
            // 
            this.lblSplitter1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSplitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSplitter1.Location = new System.Drawing.Point(146, 1);
            this.lblSplitter1.Name = "lblSplitter1";
            this.mainLayoutPanel.SetRowSpan(this.lblSplitter1, 2);
            this.lblSplitter1.Size = new System.Drawing.Size(1, 50);
            this.lblSplitter1.TabStop = false;
            // 
            // lblSplitter2
            // 
            this.lblSplitter2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSplitter2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSplitter2.Location = new System.Drawing.Point(146, 1);
            this.lblSplitter2.Name = "lblSplitter2";
            this.mainLayoutPanel.SetRowSpan(this.lblSplitter2, 2);
            this.lblSplitter2.Size = new System.Drawing.Size(1, 50);
            this.lblSplitter2.TabStop = false;
            // 
            // titleDayPL
            // 
            this.titleDayPL.AutoSize = true;
            this.titleDayPL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleDayPL.Location = new System.Drawing.Point(453, 24);
            this.titleDayPL.Name = "titleDayPL";
            this.titleDayPL.Size = new System.Drawing.Size(70, 24);
            this.inboxControlStyler1.SetStyleSettings(this.titleDayPL, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.titleDayPL.TabIndex = 7;
            this.titleDayPL.Text = "Day P&&L";
            this.titleDayPL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleExposure
            // 
            this.titleExposure.AutoSize = true;
            this.titleExposure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleExposure.Location = new System.Drawing.Point(354, 24);
            this.titleExposure.Name = "titleExposure";
            this.titleExposure.Size = new System.Drawing.Size(88, 24);
            this.inboxControlStyler1.SetStyleSettings(this.titleExposure, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.titleExposure.TabIndex = 6;
            this.titleExposure.Text = "Exposure";
            this.titleExposure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titlePosition
            // 
            this.titlePosition.AutoSize = true;
            this.titlePosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titlePosition.Location = new System.Drawing.Point(260, 24);
            this.titlePosition.Name = "titlePosition";
            this.titlePosition.Size = new System.Drawing.Size(88, 24);
            this.inboxControlStyler1.SetStyleSettings(this.titlePosition, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.titlePosition.TabIndex = 5;
            this.titlePosition.Text = "Position";
            this.titlePosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleAsk
            // 
            this.titleAsk.AutoSize = true;
            this.titleAsk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleAsk.Location = new System.Drawing.Point(192, 24);
            this.titleAsk.Name = "titleAsk";
            this.titleAsk.Size = new System.Drawing.Size(57, 24);
            this.inboxControlStyler1.SetStyleSettings(this.titleAsk, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.titleAsk.TabIndex = 4;
            this.titleAsk.Text = "Ask";
            this.titleAsk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleBid
            // 
            this.titleBid.AutoSize = true;
            this.titleBid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBid.Location = new System.Drawing.Point(129, 24);
            this.titleBid.Name = "titleBid";
            this.titleBid.Size = new System.Drawing.Size(57, 24);
            this.inboxControlStyler1.SetStyleSettings(this.titleBid, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.titleBid.TabIndex = 3;
            this.titleBid.Text = "Bid";
            this.titleBid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleChange
            // 
            this.titleChange.AutoSize = true;
            this.titleChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleChange.Location = new System.Drawing.Point(66, 24);
            this.titleChange.Name = "titleChange";
            this.titleChange.Size = new System.Drawing.Size(57, 24);
            this.inboxControlStyler1.SetStyleSettings(this.titleChange, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.titleChange.TabIndex = 2;
            this.titleChange.Text = "Change";
            this.titleChange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // titleLast
            // 
            this.titleLast.AutoSize = true;
            this.titleLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLast.Location = new System.Drawing.Point(3, 24);
            this.titleLast.Name = "titleLast";
            this.titleLast.Size = new System.Drawing.Size(57, 24);
            this.inboxControlStyler1.SetStyleSettings(this.titleLast, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.titleLast.TabIndex = 1;
            this.titleLast.Text = "Last";
            this.titleLast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDayPL
            // 
            this.lblDayPL.AutoSize = true;
            this.lblDayPL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDayPL.Location = new System.Drawing.Point(453, 24);
            this.lblDayPL.Name = "lblDayPL";
            this.lblDayPL.Size = new System.Drawing.Size(70, 24);
            this.inboxControlStyler1.SetStyleSettings(this.lblDayPL, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblDayPL.TabIndex = 7;
            this.lblDayPL.Text = "0.0";
            this.lblDayPL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDayPL.MouseHover += new System.EventHandler(this.lblDayPL_MouseHover);
            // 
            // lblExposure
            // 
            this.lblExposure.AutoSize = true;
            this.lblExposure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExposure.Location = new System.Drawing.Point(354, 24);
            this.lblExposure.Name = "lblExposure";
            this.lblExposure.Size = new System.Drawing.Size(88, 24);
            this.inboxControlStyler1.SetStyleSettings(this.lblExposure, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblExposure.TabIndex = 6;
            this.lblExposure.Text = "0.0";
            this.lblExposure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblExposure.MouseHover += new System.EventHandler(this.lblExposure_MouseHover);
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPosition.Location = new System.Drawing.Point(260, 24);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(88, 24);
            this.inboxControlStyler1.SetStyleSettings(this.lblPosition, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblPosition.TabIndex = 5;
            this.lblPosition.Text = "0.0";
            this.lblPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPosition.MouseHover += new System.EventHandler(this.lblPosition_MouseHover);
            // 
            // lblAsk
            // 
            this.lblAsk.AutoSize = true;
            this.lblAsk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAsk.Location = new System.Drawing.Point(192, 24);
            this.lblAsk.Name = "lblAsk";
            this.lblAsk.Size = new System.Drawing.Size(57, 24);
            this.inboxControlStyler1.SetStyleSettings(this.lblAsk, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblAsk.TabIndex = 4;
            this.lblAsk.Text = "0.0";
            this.lblAsk.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBid
            // 
            this.lblBid.AutoSize = true;
            this.lblBid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBid.Location = new System.Drawing.Point(129, 24);
            this.lblBid.Name = "lblBid";
            this.lblBid.Size = new System.Drawing.Size(57, 24);
            this.inboxControlStyler1.SetStyleSettings(this.lblBid, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblBid.TabIndex = 3;
            this.lblBid.Text = "0.0";
            this.lblBid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblChange
            // 
            this.lblChange.AutoSize = true;
            this.lblChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChange.Location = new System.Drawing.Point(66, 24);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(57, 24);
            this.inboxControlStyler1.SetStyleSettings(this.lblChange, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblChange.TabIndex = 2;
            this.lblChange.Text = "0.0";
            this.lblChange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLast
            // 
            this.lblLast.AutoSize = true;
            this.lblLast.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLast.Location = new System.Drawing.Point(3, 24);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(57, 24);
            this.inboxControlStyler1.SetStyleSettings(this.lblLast, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblLast.TabIndex = 1;
            this.lblLast.Text = "0.0";
            this.lblLast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;
            this.ultraToolTipManager1.DisplayStyle = Infragistics.Win.ToolTipDisplayStyle.Default;
            // 
            // QTTL1Strip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayoutPanel);
            this.Name = "QTTL1Strip";
            this.Size = new System.Drawing.Size(526, 48);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.OneSymbolL1Strip_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.mainLayoutPanel.ResumeLayout(false);
            this.mainLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.Label lblDayPL;
        private System.Windows.Forms.Label lblExposure;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label lblAsk;
        private System.Windows.Forms.Label lblBid;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Label lblLast;
        private System.Windows.Forms.Label titleDayPL;
        private System.Windows.Forms.Label titleExposure;
        private System.Windows.Forms.Label titlePosition;
        private System.Windows.Forms.Label titleAsk;
        private System.Windows.Forms.Label titleBid;
        private System.Windows.Forms.Label titleChange;
        private System.Windows.Forms.Label titleLast;
        private System.Windows.Forms.Label lblSplitter1;
        private System.Windows.Forms.Label lblSplitter2;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
    }
}
