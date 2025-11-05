namespace DataProcessor
{
    partial class frmProcessTradeData
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.btnProcesssOpenSnapShot = new System.Windows.Forms.Button();
            this.grdData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnGetPMData = new System.Windows.Forms.Button();
            this.btnSaveOpenPosition = new System.Windows.Forms.Button();
            this.btnProcessPMTaxlotSnapShot = new System.Windows.Forms.Button();
            this.btnSavePMTaxlots = new System.Windows.Forms.Button();
            this.btnURPNLTable = new System.Windows.Forms.Button();
            this.btnSaveURPNL = new System.Windows.Forms.Button();
            this.btnProcessClosingData = new System.Windows.Forms.Button();
            this.btnPMTaxlotClosing = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProcesssOpenSnapShot
            // 
            this.btnProcesssOpenSnapShot.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnProcesssOpenSnapShot.Location = new System.Drawing.Point(235, 324);
            this.btnProcesssOpenSnapShot.Name = "btnProcesssOpenSnapShot";
            this.btnProcesssOpenSnapShot.Size = new System.Drawing.Size(133, 23);
            this.btnProcesssOpenSnapShot.TabIndex = 0;
            this.btnProcesssOpenSnapShot.Text = "CreateOpenSnapShot";
            this.btnProcesssOpenSnapShot.UseVisualStyleBackColor = true;
            this.btnProcesssOpenSnapShot.Click += new System.EventHandler(this.btnProcesssPMTaxlots_Click);
            // 
            // grdData
            // 
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.BackColor = System.Drawing.SystemColors.Window;
            appearance4.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdData.DisplayLayout.Appearance = appearance4;
            ultraGridBand1.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance1.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.BorderColor = System.Drawing.SystemColors.Window;
            this.grdData.DisplayLayout.GroupByBox.Appearance = appearance1;
            appearance2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdData.DisplayLayout.GroupByBox.BandLabelAppearance = appearance2;
            this.grdData.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColor2 = System.Drawing.SystemColors.Control;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdData.DisplayLayout.GroupByBox.PromptAppearance = appearance3;
            this.grdData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdData.DisplayLayout.MaxRowScrollRegions = 1;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdData.DisplayLayout.Override.ActiveCellAppearance = appearance12;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdData.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.grdData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            this.grdData.DisplayLayout.Override.CardAreaAppearance = appearance6;
            appearance5.BorderColor = System.Drawing.Color.Silver;
            appearance5.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdData.DisplayLayout.Override.CellAppearance = appearance5;
            this.grdData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdData.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdData.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance11.TextHAlignAsString = "Left";
            this.grdData.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.grdData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance10.BackColor = System.Drawing.SystemColors.Window;
            appearance10.BorderColor = System.Drawing.Color.Silver;
            this.grdData.DisplayLayout.Override.RowAppearance = appearance10;
            this.grdData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance8.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdData.DisplayLayout.Override.TemplateAddRowAppearance = appearance8;
            this.grdData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdData.Location = new System.Drawing.Point(0, 0);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(732, 294);
            this.grdData.TabIndex = 1;
            this.grdData.Text = "ultraGrid1";
            // 
            // btnGetPMData
            // 
            this.btnGetPMData.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGetPMData.Location = new System.Drawing.Point(330, 295);
            this.btnGetPMData.Name = "btnGetPMData";
            this.btnGetPMData.Size = new System.Drawing.Size(88, 23);
            this.btnGetPMData.TabIndex = 0;
            this.btnGetPMData.Text = "Get PM Data";
            this.btnGetPMData.UseVisualStyleBackColor = true;
            this.btnGetPMData.Click += new System.EventHandler(this.btnGetPMData_Click);
            // 
            // btnSaveOpenPosition
            // 
            this.btnSaveOpenPosition.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSaveOpenPosition.Location = new System.Drawing.Point(376, 324);
            this.btnSaveOpenPosition.Name = "btnSaveOpenPosition";
            this.btnSaveOpenPosition.Size = new System.Drawing.Size(117, 23);
            this.btnSaveOpenPosition.TabIndex = 0;
            this.btnSaveOpenPosition.Text = "SaveOpenSnapShot";
            this.btnSaveOpenPosition.UseVisualStyleBackColor = true;
            this.btnSaveOpenPosition.Click += new System.EventHandler(this.btnSaveOpenPosition_Click);
            // 
            // btnProcessPMTaxlotSnapShot
            // 
            this.btnProcessPMTaxlotSnapShot.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnProcessPMTaxlotSnapShot.Location = new System.Drawing.Point(404, 255);
            this.btnProcessPMTaxlotSnapShot.Name = "btnProcessPMTaxlotSnapShot";
            this.btnProcessPMTaxlotSnapShot.Size = new System.Drawing.Size(103, 23);
            this.btnProcessPMTaxlotSnapShot.TabIndex = 2;
            this.btnProcessPMTaxlotSnapShot.Text = "ProcessPMTaxlots";
            this.btnProcessPMTaxlotSnapShot.UseVisualStyleBackColor = true;
            this.btnProcessPMTaxlotSnapShot.Visible = false;
            this.btnProcessPMTaxlotSnapShot.Click += new System.EventHandler(this.btnProcessPMTaxlotSnapShot_Click);
            // 
            // btnSavePMTaxlots
            // 
            this.btnSavePMTaxlots.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSavePMTaxlots.Location = new System.Drawing.Point(513, 255);
            this.btnSavePMTaxlots.Name = "btnSavePMTaxlots";
            this.btnSavePMTaxlots.Size = new System.Drawing.Size(93, 23);
            this.btnSavePMTaxlots.TabIndex = 3;
            this.btnSavePMTaxlots.Text = "SavePMTaxlots";
            this.btnSavePMTaxlots.UseVisualStyleBackColor = true;
            this.btnSavePMTaxlots.Visible = false;
            this.btnSavePMTaxlots.Click += new System.EventHandler(this.btnSavePMTaxlots_Click);
            // 
            // btnURPNLTable
            // 
            this.btnURPNLTable.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnURPNLTable.Location = new System.Drawing.Point(164, 353);
            this.btnURPNLTable.Name = "btnURPNLTable";
            this.btnURPNLTable.Size = new System.Drawing.Size(100, 23);
            this.btnURPNLTable.TabIndex = 4;
            this.btnURPNLTable.Text = "Create URPNL Table";
            this.btnURPNLTable.UseVisualStyleBackColor = true;
            this.btnURPNLTable.Click += new System.EventHandler(this.btnURPNLTable_Click);
            // 
            // btnSaveURPNL
            // 
            this.btnSaveURPNL.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSaveURPNL.Location = new System.Drawing.Point(470, 353);
            this.btnSaveURPNL.Name = "btnSaveURPNL";
            this.btnSaveURPNL.Size = new System.Drawing.Size(80, 23);
            this.btnSaveURPNL.TabIndex = 5;
            this.btnSaveURPNL.Text = "SaveURPNL";
            this.btnSaveURPNL.UseVisualStyleBackColor = true;
            this.btnSaveURPNL.Click += new System.EventHandler(this.btnSaveURPNL_Click);
            // 
            // btnProcessClosingData
            // 
            this.btnProcessClosingData.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnProcessClosingData.Location = new System.Drawing.Point(372, 353);
            this.btnProcessClosingData.Name = "btnProcessClosingData";
            this.btnProcessClosingData.Size = new System.Drawing.Size(92, 23);
            this.btnProcessClosingData.TabIndex = 6;
            this.btnProcessClosingData.Text = "Create RPNL";
            this.btnProcessClosingData.UseVisualStyleBackColor = true;
            this.btnProcessClosingData.Click += new System.EventHandler(this.btnProcessClosingData_Click);
            // 
            // btnPMTaxlotClosing
            // 
            this.btnPMTaxlotClosing.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPMTaxlotClosing.Location = new System.Drawing.Point(269, 353);
            this.btnPMTaxlotClosing.Name = "btnPMTaxlotClosing";
            this.btnPMTaxlotClosing.Size = new System.Drawing.Size(98, 23);
            this.btnPMTaxlotClosing.TabIndex = 6;
            this.btnPMTaxlotClosing.Text = "Get Closing Data";
            this.btnPMTaxlotClosing.UseVisualStyleBackColor = true;
            this.btnPMTaxlotClosing.Click += new System.EventHandler(this.btnPMTaxlotClosing_Click);
            // 
            // frmProcessTradeData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 384);
            this.Controls.Add(this.btnProcessClosingData);
            this.Controls.Add(this.btnPMTaxlotClosing);
            this.Controls.Add(this.btnSaveURPNL);
            this.Controls.Add(this.btnURPNLTable);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.btnSaveOpenPosition);
            this.Controls.Add(this.btnGetPMData);
            this.Controls.Add(this.btnProcesssOpenSnapShot);
            this.Controls.Add(this.btnProcessPMTaxlotSnapShot);
            this.Controls.Add(this.btnSavePMTaxlots);
            this.Name = "frmProcessTradeData";
            this.Text = "Process Trade Data";
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProcesssOpenSnapShot;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdData;
        private System.Windows.Forms.Button btnGetPMData;
        private System.Windows.Forms.Button btnSaveOpenPosition;
        private System.Windows.Forms.Button btnProcessPMTaxlotSnapShot;
        private System.Windows.Forms.Button btnSavePMTaxlots;
        private System.Windows.Forms.Button btnURPNLTable;
        private System.Windows.Forms.Button btnSaveURPNL;
        private System.Windows.Forms.Button btnProcessClosingData;
        private System.Windows.Forms.Button btnPMTaxlotClosing;
    }
}

