namespace Prana.Import.Controls
{
    partial class CtrlHistoricalUpload
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_frmReport != null)
                {
                    _frmReport.Dispose();
                }
                if (_frmUploadedFile != null)
                {
                    _frmUploadedFile.Dispose();
                }
                if (ctrlReport != null)
                {
                    ctrlReport.Dispose();
                }
                if (_grdReport != null)
                {
                    _grdReport.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.dtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnExport = new Infragistics.Win.Misc.UltraButton();
            this.btnGetData = new Infragistics.Win.Misc.UltraButton();
            this.lblToDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblFromDate = new Infragistics.Win.Misc.UltraLabel();
            this.grdHistoricalUpload = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistoricalUpload)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ultraPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdHistoricalUpload);
            this.splitContainer1.Size = new System.Drawing.Size(661, 361);
            this.splitContainer1.SplitterDistance = 63;
            this.splitContainer1.TabIndex = 0;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.dtFromDate);
            this.ultraPanel1.ClientArea.Controls.Add(this.dtToDate);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnExport);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnGetData);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblToDate);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblFromDate);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(661, 63);
            this.ultraPanel1.TabIndex = 0;
            // 
            // dtFromDate
            // 
            this.dtFromDate.Location = new System.Drawing.Point(62, 17);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(123, 21);
            this.dtFromDate.TabIndex = 11;
            // 
            // dtToDate
            // 
            this.dtToDate.Location = new System.Drawing.Point(236, 17);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(123, 21);
            this.dtToDate.TabIndex = 10;
            this.dtToDate.AfterCloseUp += new System.EventHandler(this.dtToDate_AfterCloseUp);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(518, 17);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(106, 28);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnGetData
            // 
            this.btnGetData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetData.Location = new System.Drawing.Point(400, 17);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(106, 28);
            this.btnGetData.TabIndex = 8;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // lblToDate
            // 
            this.lblToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToDate.Location = new System.Drawing.Point(203, 17);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(53, 28);
            this.lblToDate.TabIndex = 7;
            this.lblToDate.Text = "To";
            // 
            // lblFromDate
            // 
            this.lblFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromDate.Location = new System.Drawing.Point(15, 17);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(53, 28);
            this.lblFromDate.TabIndex = 6;
            this.lblFromDate.Text = "From";
            // 
            // grdHistoricalUpload
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdHistoricalUpload.DisplayLayout.Appearance = appearance1;
            this.grdHistoricalUpload.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdHistoricalUpload.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdHistoricalUpload.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdHistoricalUpload.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdHistoricalUpload.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdHistoricalUpload.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdHistoricalUpload.DisplayLayout.MaxColScrollRegions = 1;
            this.grdHistoricalUpload.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdHistoricalUpload.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdHistoricalUpload.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdHistoricalUpload.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdHistoricalUpload.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdHistoricalUpload.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdHistoricalUpload.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdHistoricalUpload.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdHistoricalUpload.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdHistoricalUpload.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdHistoricalUpload.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdHistoricalUpload.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdHistoricalUpload.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdHistoricalUpload.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdHistoricalUpload.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdHistoricalUpload.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdHistoricalUpload.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdHistoricalUpload.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdHistoricalUpload.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdHistoricalUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdHistoricalUpload.Location = new System.Drawing.Point(0, 0);
            this.grdHistoricalUpload.Name = "grdHistoricalUpload";
            this.grdHistoricalUpload.Size = new System.Drawing.Size(661, 294);
            this.grdHistoricalUpload.TabIndex = 0;
            this.grdHistoricalUpload.Text = "grdHistoricalUpload";
            this.grdHistoricalUpload.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdHistoricalUpload_InitializeLayout);
            this.grdHistoricalUpload.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdHistoricalUpload_ClickCellButton);
            // 
            // CtrlHistoricalUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CtrlHistoricalUpload";
            this.Size = new System.Drawing.Size(661, 361);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistoricalUpload)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton btnExport;
        private Infragistics.Win.Misc.UltraButton btnGetData;
        private Infragistics.Win.Misc.UltraLabel lblToDate;
        private Infragistics.Win.Misc.UltraLabel lblFromDate;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdHistoricalUpload;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDate;

    }
}
