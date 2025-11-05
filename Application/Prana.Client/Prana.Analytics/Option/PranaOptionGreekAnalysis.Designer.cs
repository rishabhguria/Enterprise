namespace Prana.Analytics
{
    partial class PranaOptionGreekAnalysis
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
            UnwireEvents();
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.btnRefreshPositions = new System.Windows.Forms.Button();
            this.grdData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnCalculateGreeks = new System.Windows.Forms.Button();
            this.chkbxVol = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblVolatility = new System.Windows.Forms.Label();
            this.btnSimulation = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.chkbxUnderLyingPrice = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.label4 = new System.Windows.Forms.Label();
            this.chkbxInterestRate = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.numericUpDownIntRate = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownVol = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownUnderLyingPrice = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ckhbxExpiration = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.txtbxDaysToExpiration = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUnderLyingPrice)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRefreshPositions
            // 
            this.btnRefreshPositions.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRefreshPositions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnRefreshPositions.Location = new System.Drawing.Point(369, 409);
            this.btnRefreshPositions.Name = "btnRefreshPositions";
            this.btnRefreshPositions.Size = new System.Drawing.Size(112, 23);
            this.btnRefreshPositions.TabIndex = 130;
            this.btnRefreshPositions.Text = "Refresh Positions";
            this.btnRefreshPositions.UseVisualStyleBackColor = false;
            this.btnRefreshPositions.Click += new System.EventHandler(this.btnRefreshPositions_Click);
            // 
            // grdData
            // 
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "Tahoma";
            appearance1.FontData.SizeInPoints = 8.25F;
            this.grdData.DisplayLayout.Appearance = appearance1;
            this.grdData.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance2.BackColor = System.Drawing.Color.White;
            this.grdData.DisplayLayout.CaptionAppearance = appearance2;
            this.grdData.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.GroupByBox.Hidden = true;
            this.grdData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdData.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdData.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdData.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdData.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdData.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdData.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.FontData.BoldAsString = "False";
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 8.25F;
            this.grdData.DisplayLayout.Override.HeaderAppearance = appearance3;
            this.grdData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdData.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.OncePerGroupedRowIsland;
            this.grdData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance4.BackHatchStyle = Infragistics.Win.BackHatchStyle.None;
            this.grdData.DisplayLayout.Override.RowAlternateAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.Black;
            appearance5.BackColor2 = System.Drawing.Color.Black;
            appearance5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.grdData.DisplayLayout.Override.RowAppearance = appearance5;
            this.grdData.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdData.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.Bottom;
            this.grdData.DisplayLayout.Override.SummaryFooterCaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdData.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdData.Location = new System.Drawing.Point(3, 53);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(931, 350);
            this.grdData.TabIndex = 131;
            this.grdData.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdData.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.FilterCellValueChanged += new Infragistics.Win.UltraWinGrid.FilterCellValueChangedEventHandler(this.grdData_FilterCellValueChanged);
            // 
            // btnCalculateGreeks
            // 
            this.btnCalculateGreeks.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCalculateGreeks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnCalculateGreeks.Location = new System.Drawing.Point(487, 409);
            this.btnCalculateGreeks.Name = "btnCalculateGreeks";
            this.btnCalculateGreeks.Size = new System.Drawing.Size(112, 23);
            this.btnCalculateGreeks.TabIndex = 132;
            this.btnCalculateGreeks.Text = "CalculateGreeks";
            this.btnCalculateGreeks.UseVisualStyleBackColor = false;
            this.btnCalculateGreeks.Click += new System.EventHandler(this.btnCalculateGreeks_Click);
            // 
            // chkbxVol
            // 
            this.chkbxVol.Location = new System.Drawing.Point(11, 11);
            this.chkbxVol.Name = "chkbxVol";
            this.chkbxVol.Size = new System.Drawing.Size(16, 15);
            this.chkbxVol.TabIndex = 133;
            // 
            // lblVolatility
            // 
            this.lblVolatility.AutoSize = true;
            this.lblVolatility.Location = new System.Drawing.Point(28, 12);
            this.lblVolatility.Name = "lblVolatility";
            this.lblVolatility.Size = new System.Drawing.Size(56, 13);
            this.lblVolatility.TabIndex = 134;
            this.lblVolatility.Text = "Volatility %";
            // 
            // btnSimulation
            // 
            this.btnSimulation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnSimulation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnSimulation.Location = new System.Drawing.Point(786, 10);
            this.btnSimulation.Name = "btnSimulation";
            this.btnSimulation.Size = new System.Drawing.Size(105, 23);
            this.btnSimulation.TabIndex = 137;
            this.btnSimulation.Text = "Simulate";
            this.btnSimulation.UseVisualStyleBackColor = false;
            this.btnSimulation.Click += new System.EventHandler(this.btnSimulation_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 139;
            this.label2.Text = "UnderLyingPrice%";
            // 
            // chkbxUnderLyingPrice
            // 
            this.chkbxUnderLyingPrice.Location = new System.Drawing.Point(10, 11);
            this.chkbxUnderLyingPrice.Name = "chkbxUnderLyingPrice";
            this.chkbxUnderLyingPrice.Size = new System.Drawing.Size(16, 17);
            this.chkbxUnderLyingPrice.TabIndex = 138;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 142;
            this.label4.Text = "InterestRate%";
            // 
            // chkbxInterestRate
            // 
            this.chkbxInterestRate.Location = new System.Drawing.Point(9, 10);
            this.chkbxInterestRate.Name = "chkbxInterestRate";
            this.chkbxInterestRate.Size = new System.Drawing.Size(16, 17);
            this.chkbxInterestRate.TabIndex = 141;
            // 
            // numericUpDownIntRate
            // 
            this.numericUpDownIntRate.Location = new System.Drawing.Point(103, 9);
            this.numericUpDownIntRate.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownIntRate.Name = "numericUpDownIntRate";
            this.numericUpDownIntRate.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownIntRate.TabIndex = 143;
            this.numericUpDownIntRate.ValueChanged += new System.EventHandler(this.numericUpDownIntRate_ValueChanged);
            // 
            // numericUpDownVol
            // 
            this.numericUpDownVol.Location = new System.Drawing.Point(86, 9);
            this.numericUpDownVol.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownVol.Name = "numericUpDownVol";
            this.numericUpDownVol.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownVol.TabIndex = 144;
            this.numericUpDownVol.ValueChanged += new System.EventHandler(this.numericUpDownVol_ValueChanged);
            // 
            // numericUpDownUnderLyingPrice
            // 
            this.numericUpDownUnderLyingPrice.Location = new System.Drawing.Point(122, 9);
            this.numericUpDownUnderLyingPrice.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericUpDownUnderLyingPrice.Name = "numericUpDownUnderLyingPrice";
            this.numericUpDownUnderLyingPrice.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownUnderLyingPrice.TabIndex = 145;
            this.numericUpDownUnderLyingPrice.ValueChanged += new System.EventHandler(this.numericUpDownUnderLyingPrice_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 147;
            this.label1.Text = "Days To Expiration";
            // 
            // ckhbxExpiration
            // 
            this.ckhbxExpiration.Location = new System.Drawing.Point(10, 12);
            this.ckhbxExpiration.Name = "ckhbxExpiration";
            this.ckhbxExpiration.Size = new System.Drawing.Size(16, 15);
            this.ckhbxExpiration.TabIndex = 146;
            // 
            // txtbxDaysToExpiration
            // 
            this.txtbxDaysToExpiration.Location = new System.Drawing.Point(131, 9);
            this.txtbxDaysToExpiration.Name = "txtbxDaysToExpiration";
            this.txtbxDaysToExpiration.Size = new System.Drawing.Size(45, 20);
            this.txtbxDaysToExpiration.TabIndex = 148;
            this.txtbxDaysToExpiration.TextChanged += new System.EventHandler(this.txtbxDaysToExpiration_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownVol);
            this.groupBox1.Controls.Add(this.chkbxVol);
            this.groupBox1.Controls.Add(this.lblVolatility);
            this.groupBox1.Location = new System.Drawing.Point(10, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(146, 32);
            this.groupBox1.TabIndex = 149;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.chkbxUnderLyingPrice);
            this.groupBox2.Controls.Add(this.numericUpDownUnderLyingPrice);
            this.groupBox2.Location = new System.Drawing.Point(162, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(183, 32);
            this.groupBox2.TabIndex = 150;
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.chkbxInterestRate);
            this.groupBox3.Controls.Add(this.numericUpDownIntRate);
            this.groupBox3.Location = new System.Drawing.Point(351, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(163, 32);
            this.groupBox3.TabIndex = 151;
            this.groupBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.ckhbxExpiration);
            this.groupBox4.Controls.Add(this.txtbxDaysToExpiration);
            this.groupBox4.Location = new System.Drawing.Point(520, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(185, 32);
            this.groupBox4.TabIndex = 151;
            this.groupBox4.TabStop = false;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 438);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(937, 22);
            this.statusStrip1.TabIndex = 152;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox1.Controls.Add(this.groupBox1);
            this.ultraGroupBox1.Controls.Add(this.groupBox2);
            this.ultraGroupBox1.Controls.Add(this.btnSimulation);
            this.ultraGroupBox1.Controls.Add(this.groupBox4);
            this.ultraGroupBox1.Controls.Add(this.groupBox3);
            this.ultraGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(931, 44);
            this.ultraGroupBox1.TabIndex = 153;
            // 
            // PranaOptionGreekAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCalculateGreeks);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.btnRefreshPositions);
            this.Name = "PranaOptionGreekAnalysis";
            this.Size = new System.Drawing.Size(937, 460);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownIntRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownUnderLyingPrice)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRefreshPositions;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdData;
        private System.Windows.Forms.Button btnCalculateGreeks;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxVol;
        private System.Windows.Forms.Label lblVolatility;
        private System.Windows.Forms.Button btnSimulation;
        private System.Windows.Forms.Label label2;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxUnderLyingPrice;
        private System.Windows.Forms.Label label4;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkbxInterestRate;
        private System.Windows.Forms.NumericUpDown numericUpDownIntRate;
        private System.Windows.Forms.NumericUpDown numericUpDownVol;
        private System.Windows.Forms.NumericUpDown numericUpDownUnderLyingPrice;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor ckhbxExpiration;
        private System.Windows.Forms.TextBox txtbxDaysToExpiration;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
    }
}