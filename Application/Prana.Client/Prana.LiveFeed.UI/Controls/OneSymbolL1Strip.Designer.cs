namespace Prana.LiveFeed.UI.Controls
{
    partial class OneSymbolL1Strip
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
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ultraPanel = new Infragistics.Win.Misc.UltraPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblLast = new System.Windows.Forms.Label();
            this.lblChange = new System.Windows.Forms.Label();
            this.lblBid = new System.Windows.Forms.Label();
            this.lblAsk = new System.Windows.Forms.Label();
            this.lblHigh = new System.Windows.Forms.Label();
            this.lblLow = new System.Windows.Forms.Label();
            this.lblVolumn = new System.Windows.Forms.Label();
            this.lblNotation = new System.Windows.Forms.Label();
            this.lblVWAP = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblPosition = new System.Windows.Forms.Label();
            this.lblExposure = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblRoundLot = new System.Windows.Forms.Label();
            this.toggleSwitchRoundLot = new Prana.Utilities.UI.ToggleSwitch();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.ultraPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(334, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label8, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label8.TabIndex = 43;
            this.label8.Text = "Volume";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(458, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label12, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label12.TabIndex = 56;
            this.label12.Text = "Exposure";
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraPanel.AutoSize = true;
            this.ultraPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ultraPanel.ClientArea.Controls.Add(this.lblExposure);
            this.ultraPanel.ClientArea.Controls.Add(this.label12);
            this.ultraPanel.ClientArea.Controls.Add(this.lblPosition);
            this.ultraPanel.ClientArea.Controls.Add(this.label11);
            this.ultraPanel.ClientArea.Controls.Add(this.lblVWAP);
            this.ultraPanel.ClientArea.Controls.Add(this.lblNotation);
            this.ultraPanel.ClientArea.Controls.Add(this.lblVolumn);
            this.ultraPanel.ClientArea.Controls.Add(this.lblLow);
            this.ultraPanel.ClientArea.Controls.Add(this.lblHigh);
            this.ultraPanel.ClientArea.Controls.Add(this.lblAsk);
            this.ultraPanel.ClientArea.Controls.Add(this.lblBid);
            this.ultraPanel.ClientArea.Controls.Add(this.lblChange);
            this.ultraPanel.ClientArea.Controls.Add(this.lblLast);
            this.ultraPanel.ClientArea.Controls.Add(this.label9);
            this.ultraPanel.ClientArea.Controls.Add(this.label8);
            this.ultraPanel.ClientArea.Controls.Add(this.label7);
            this.ultraPanel.ClientArea.Controls.Add(this.label6);
            this.ultraPanel.ClientArea.Controls.Add(this.label5);
            this.ultraPanel.ClientArea.Controls.Add(this.label4);
            this.ultraPanel.ClientArea.Controls.Add(this.label3);
            this.ultraPanel.ClientArea.Controls.Add(this.label2);
            this.ultraPanel.ClientArea.Controls.Add(this.label1);
            this.ultraPanel.ClientArea.Controls.Add(this.toggleSwitchRoundLot);
            this.ultraPanel.ClientArea.Controls.Add(this.lblRoundLot);
            this.ultraPanel.ClientArea.Controls.Add(this.label10);
            this.ultraPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel.Name = "ultraPanel";
            this.ultraPanel.Size = new System.Drawing.Size(671, 39);
            this.ultraPanel.TabIndex = 0;
            
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label1.TabIndex = 36;
            this.label1.Text = "Last";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label2.TabIndex = 37;
            this.label2.Text = "Change";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label3, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label3.TabIndex = 38;
            this.label3.Text = "Bid";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(168, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label4, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label4.TabIndex = 39;
            this.label4.Text = "Ask";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(227, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label5, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label5.TabIndex = 40;
            this.label5.Text = "High";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(287, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label6, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label6.TabIndex = 41;
            this.label6.Text = "Low";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(524, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label7, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label7.TabIndex = 42;
            this.label7.Text = "Gross Notional";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(606, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label9, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label9.TabIndex = 44;
            this.label9.Text = "VWAP";
            // 
            // lblLast
            // 
            this.lblLast.AutoSize = true;
            this.lblLast.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblLast.Location = new System.Drawing.Point(4, 21);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblLast, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblLast.TabIndex = 45;
            this.lblLast.Text = "";
            // 
            // lblChange
            // 
            this.lblChange.AutoSize = true;
            this.lblChange.ForeColor = System.Drawing.Color.White;
            this.lblChange.Location = new System.Drawing.Point(52, 21);
            this.lblChange.Name = "lblChange";
            this.lblChange.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblChange, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblChange.TabIndex = 46;
            this.lblChange.Text = "";
            // 
            // lblBid
            // 
            this.lblBid.AutoSize = true;
            this.lblBid.ForeColor = System.Drawing.Color.White;
            this.lblBid.Location = new System.Drawing.Point(121, 21);
            this.lblBid.Name = "lblBid";
            this.lblBid.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblBid, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblBid.TabIndex = 47;
            this.lblBid.Text = "";
            // 
            // lblAsk
            // 
            this.lblAsk.AutoSize = true;
            this.lblAsk.ForeColor = System.Drawing.Color.White;
            this.lblAsk.Location = new System.Drawing.Point(168, 21);
            this.lblAsk.Name = "lblAsk";
            this.lblAsk.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblAsk, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblAsk.TabIndex = 48;
            this.lblAsk.Text = "";
            // 
            // lblHigh
            // 
            this.lblHigh.AutoSize = true;
            this.lblHigh.ForeColor = System.Drawing.Color.White;
            this.lblHigh.Location = new System.Drawing.Point(227, 21);
            this.lblHigh.Name = "lblHigh";
            this.lblHigh.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblHigh, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblHigh.TabIndex = 49;
            this.lblHigh.Text = "";
            // 
            // lblLow
            // 
            this.lblLow.AutoSize = true;
            this.lblLow.ForeColor = System.Drawing.Color.White;
            this.lblLow.Location = new System.Drawing.Point(287, 21);
            this.lblLow.Name = "lblLow";
            this.lblLow.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblLow, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblLow.TabIndex = 50;
            this.lblLow.Text = "";
            // 
            // lblVolumn
            // 
            this.lblVolumn.AutoSize = true;
            this.lblVolumn.ForeColor = System.Drawing.Color.White;
            this.lblVolumn.Location = new System.Drawing.Point(334, 21);
            this.lblVolumn.Name = "lblVolumn";
            this.lblVolumn.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblVolumn, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblVolumn.TabIndex = 51;
            this.lblVolumn.Text = "";
            // 
            // lblNotation
            // 
            this.lblNotation.AutoSize = true;
            this.lblNotation.ForeColor = System.Drawing.Color.White;
            this.lblNotation.Location = new System.Drawing.Point(524, 21);
            this.lblNotation.Name = "lblNotation";
            this.lblNotation.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblNotation, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblNotation.TabIndex = 52;
            this.lblNotation.Text = "";
            // 
            // lblVWAP
            // 
            this.lblVWAP.AutoSize = true;
            this.lblVWAP.ForeColor = System.Drawing.Color.White;
            this.lblVWAP.Location = new System.Drawing.Point(606, 21);
            this.lblVWAP.Name = "lblVWAP";
            this.lblVWAP.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblVWAP, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblVWAP.TabIndex = 53;
            this.lblVWAP.Text = "";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(398, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label11, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label11.TabIndex = 54;
            this.label11.Text = "Position";
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.ForeColor = System.Drawing.Color.White;
            this.lblPosition.Location = new System.Drawing.Point(398, 21);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblPosition, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblPosition.TabIndex = 55;
            this.lblPosition.Text = "";
            // 
            // lblExposure
            // 
            this.lblExposure.AutoSize = true;
            this.lblExposure.ForeColor = System.Drawing.Color.White;
            this.lblExposure.Location = new System.Drawing.Point(458, 21);
            this.lblExposure.Name = "lblExposure";
            this.lblExposure.Size = new System.Drawing.Size(25, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblExposure, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblExposure.TabIndex = 57;
            this.lblExposure.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(654, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 13);
            this.inboxControlStyler1.SetStyleSettings(this.label10, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label10.TabIndex = 58;
            this.label10.Text = "Round Lot";
            // 
            // lblRoundLot
            // 
            this.lblRoundLot.AutoSize = true;
            this.lblRoundLot.Location = new System.Drawing.Point(655, 21);
            this.lblRoundLot.Name = "lblRoundLot";
            this.lblRoundLot.Size = new System.Drawing.Size(0, 13);
            this.inboxControlStyler1.SetStyleSettings(this.lblRoundLot, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblRoundLot.TabIndex = 59;
            // 
            // toggleSwitchRoundLot
            // 
            this.toggleSwitchRoundLot.AllowCheckChangedDuringLoad = false;
            this.toggleSwitchRoundLot.Location = new System.Drawing.Point(710, 8);
            this.toggleSwitchRoundLot.Name = "toggleSwitchRoundLot";
            this.toggleSwitchRoundLot.OffFont = new System.Drawing.Font("Microsoft Sans Serif", 6.95F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleSwitchRoundLot.OffText = "Off";
            this.toggleSwitchRoundLot.OnFont = new System.Drawing.Font("Microsoft Sans Serif", 6.95F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleSwitchRoundLot.OnText = "On";
            this.toggleSwitchRoundLot.Size = new System.Drawing.Size(35, 14);
            this.toggleSwitchRoundLot.Style = Prana.Utilities.UI.ToggleSwitch.ToggleSwitchStyle.IOS5;
            this.toggleSwitchRoundLot.TabIndex = 60;
            this.toggleSwitchRoundLot.Click += new System.EventHandler(this.toggleSwitchRoundLot_Click);
            // 
            // OneSymbolL1Strip
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.Controls.Add(this.ultraPanel);
            this.Name = "OneSymbolL1Strip";
            this.Size = new System.Drawing.Size(748, 39);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Load += new System.EventHandler(this.OneSymbolL1Strip_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ultraPanel.ResumeLayout(false);
            this.ultraPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        public Infragistics.Win.Misc.UltraPanel ultraPanel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblExposure;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblVWAP;
        private System.Windows.Forms.Label lblNotation;
        private System.Windows.Forms.Label lblVolumn;
        private System.Windows.Forms.Label lblLow;
        private System.Windows.Forms.Label lblHigh;
        private System.Windows.Forms.Label lblAsk;
        private System.Windows.Forms.Label lblBid;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.Label lblLast;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblRoundLot;
        private System.Windows.Forms.Label label10;
        private Utilities.UI.ToggleSwitch toggleSwitchRoundLot;
    }
}
