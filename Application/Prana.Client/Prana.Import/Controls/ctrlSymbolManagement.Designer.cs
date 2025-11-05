namespace Prana.Import.Controls
{
    partial class ctrlSymbolManagement
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
                if (_frmSymbolMismatch != null)
                {
                    _frmSymbolMismatch.Dispose();
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
            this.btnValidate = new Infragistics.Win.Misc.UltraButton();
            this.btnSymbolLookUp = new Infragistics.Win.Misc.UltraButton();
            this.txtFailedValidation = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtValidationAPI = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtValidationSecMaster = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTotalSymbol = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.dtEndDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtStartDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblFailedValidation = new Infragistics.Win.Misc.UltraLabel();
            this.lblValidationAPI = new Infragistics.Win.Misc.UltraLabel();
            this.lblValidationSecMaster = new Infragistics.Win.Misc.UltraLabel();
            this.lblEndDate = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.lblStartDate = new Infragistics.Win.Misc.UltraLabel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.grdReport = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnExport = new Infragistics.Win.Misc.UltraButton();
            this.btnSaveLayout = new Infragistics.Win.Misc.UltraButton();
            this.btnApprove = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraStatusBarSymbolMgt = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtFailedValidation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValidationAPI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValidationSecMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalSymbol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdReport)).BeginInit();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBarSymbolMgt)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnValidate);
            this.splitContainer1.Panel1.Controls.Add(this.btnSymbolLookUp);
            this.splitContainer1.Panel1.Controls.Add(this.txtFailedValidation);
            this.splitContainer1.Panel1.Controls.Add(this.txtValidationAPI);
            this.splitContainer1.Panel1.Controls.Add(this.txtValidationSecMaster);
            this.splitContainer1.Panel1.Controls.Add(this.txtTotalSymbol);
            this.splitContainer1.Panel1.Controls.Add(this.dtEndDate);
            this.splitContainer1.Panel1.Controls.Add(this.dtStartDate);
            this.splitContainer1.Panel1.Controls.Add(this.lblFailedValidation);
            this.splitContainer1.Panel1.Controls.Add(this.lblValidationAPI);
            this.splitContainer1.Panel1.Controls.Add(this.lblValidationSecMaster);
            this.splitContainer1.Panel1.Controls.Add(this.lblEndDate);
            this.splitContainer1.Panel1.Controls.Add(this.lblTotalSymbol);
            this.splitContainer1.Panel1.Controls.Add(this.lblStartDate);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(832, 492);
            this.splitContainer1.SplitterDistance = 146;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(486, 104);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(137, 23);
            this.btnValidate.TabIndex = 2;
            this.btnValidate.Text = "Validate Security";
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnSymbolLookUp
            // 
            this.btnSymbolLookUp.AutoSize = true;
            this.btnSymbolLookUp.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSymbolLookUp.Location = new System.Drawing.Point(326, 104);
            this.btnSymbolLookUp.Name = "btnSymbolLookUp";
            this.btnSymbolLookUp.Size = new System.Drawing.Size(97, 25);
            this.btnSymbolLookUp.TabIndex = 24;
            this.btnSymbolLookUp.Text = "Symbol Look Up";
            this.btnSymbolLookUp.Click += new System.EventHandler(this.btnSymbolLookUp_Click);
            // 
            // txtFailedValidation
            // 
            this.txtFailedValidation.Enabled = false;
            this.txtFailedValidation.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFailedValidation.Location = new System.Drawing.Point(85, 104);
            this.txtFailedValidation.Name = "txtFailedValidation";
            this.txtFailedValidation.Size = new System.Drawing.Size(95, 22);
            this.txtFailedValidation.TabIndex = 23;
            // 
            // txtValidationAPI
            // 
            this.txtValidationAPI.Enabled = false;
            this.txtValidationAPI.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtValidationAPI.Location = new System.Drawing.Point(549, 60);
            this.txtValidationAPI.Name = "txtValidationAPI";
            this.txtValidationAPI.Size = new System.Drawing.Size(95, 22);
            this.txtValidationAPI.TabIndex = 22;
            // 
            // txtValidationSecMaster
            // 
            this.txtValidationSecMaster.Enabled = false;
            this.txtValidationSecMaster.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtValidationSecMaster.Location = new System.Drawing.Point(328, 62);
            this.txtValidationSecMaster.Name = "txtValidationSecMaster";
            this.txtValidationSecMaster.Size = new System.Drawing.Size(95, 22);
            this.txtValidationSecMaster.TabIndex = 21;
            // 
            // txtTotalSymbol
            // 
            this.txtTotalSymbol.Enabled = false;
            this.txtTotalSymbol.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTotalSymbol.Location = new System.Drawing.Point(85, 62);
            this.txtTotalSymbol.Name = "txtTotalSymbol";
            this.txtTotalSymbol.Size = new System.Drawing.Size(95, 22);
            this.txtTotalSymbol.TabIndex = 20;
            // 
            // dtEndDate
            // 
            this.dtEndDate.Enabled = false;
            this.dtEndDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dtEndDate.Location = new System.Drawing.Point(328, 20);
            this.dtEndDate.Name = "dtEndDate";
            this.dtEndDate.Size = new System.Drawing.Size(95, 22);
            this.dtEndDate.TabIndex = 19;
            // 
            // dtStartDate
            // 
            this.dtStartDate.Enabled = false;
            this.dtStartDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dtStartDate.Location = new System.Drawing.Point(85, 20);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Size = new System.Drawing.Size(95, 22);
            this.dtStartDate.TabIndex = 18;
            // 
            // lblFailedValidation
            // 
            this.lblFailedValidation.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFailedValidation.Location = new System.Drawing.Point(14, 102);
            this.lblFailedValidation.Name = "lblFailedValidation";
            this.lblFailedValidation.Size = new System.Drawing.Size(79, 29);
            this.lblFailedValidation.TabIndex = 16;
            this.lblFailedValidation.Text = "#Failed Validation";
            // 
            // lblValidationAPI
            // 
            this.lblValidationAPI.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblValidationAPI.Location = new System.Drawing.Point(484, 60);
            this.lblValidationAPI.Name = "lblValidationAPI";
            this.lblValidationAPI.Size = new System.Drawing.Size(69, 29);
            this.lblValidationAPI.TabIndex = 15;
            this.lblValidationAPI.Text = "Validation API";
            // 
            // lblValidationSecMaster
            // 
            this.lblValidationSecMaster.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblValidationSecMaster.Location = new System.Drawing.Point(253, 60);
            this.lblValidationSecMaster.Name = "lblValidationSecMaster";
            this.lblValidationSecMaster.Size = new System.Drawing.Size(79, 29);
            this.lblValidationSecMaster.TabIndex = 14;
            this.lblValidationSecMaster.Text = "Validation - Sec Master";
            // 
            // lblEndDate
            // 
            this.lblEndDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblEndDate.Location = new System.Drawing.Point(253, 24);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(100, 23);
            this.lblEndDate.TabIndex = 13;
            this.lblEndDate.Text = "End Date";
            // 
            // lblTotalSymbol
            // 
            this.lblTotalSymbol.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTotalSymbol.Location = new System.Drawing.Point(14, 60);
            this.lblTotalSymbol.Name = "lblTotalSymbol";
            this.lblTotalSymbol.Size = new System.Drawing.Size(79, 29);
            this.lblTotalSymbol.TabIndex = 2;
            this.lblTotalSymbol.Text = "Total # Symbols";
            // 
            // lblStartDate
            // 
            this.lblStartDate.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblStartDate.Location = new System.Drawing.Point(14, 24);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(68, 23);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "Start Date";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.grdReport);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnExport);
            this.splitContainer2.Panel2.Controls.Add(this.btnSaveLayout);
            this.splitContainer2.Panel2.Controls.Add(this.btnApprove);
            this.splitContainer2.Size = new System.Drawing.Size(832, 342);
            this.splitContainer2.SplitterDistance = 293;
            this.splitContainer2.TabIndex = 1;
            // 
            // grdReport
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdReport.DisplayLayout.Appearance = appearance1;
            this.grdReport.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdReport.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdReport.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdReport.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdReport.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdReport.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdReport.DisplayLayout.MaxColScrollRegions = 1;
            this.grdReport.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdReport.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdReport.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdReport.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdReport.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdReport.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdReport.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdReport.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdReport.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdReport.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdReport.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdReport.DisplayLayout.Override.HeaderCheckBoxSynchronization = Infragistics.Win.UltraWinGrid.HeaderCheckBoxSynchronization.None;
            this.grdReport.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdReport.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdReport.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdReport.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdReport.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdReport.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdReport.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdReport.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdReport.Location = new System.Drawing.Point(0, 0);
            this.grdReport.Name = "grdReport";
            this.grdReport.Size = new System.Drawing.Size(832, 293);
            this.grdReport.TabIndex = 0;
            this.grdReport.Text = "ultraGrid1";
            this.grdReport.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdReport_InitializeLayout);
            this.grdReport.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdReport_InitializeRow);
            this.grdReport.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReport_ClickCellButton);
            this.grdReport.BeforeHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventHandler(this.grdReport_BeforeHeaderCheckStateChanged);
            this.grdReport.AfterHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventHandler(this.grdReport_AfterHeaderCheckStateChanged);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnExport.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnExport.Location = new System.Drawing.Point(486, 10);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(86, 23);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSaveLayout
            // 
            this.btnSaveLayout.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSaveLayout.AutoSize = true;
            this.btnSaveLayout.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSaveLayout.Location = new System.Drawing.Point(372, 8);
            this.btnSaveLayout.Name = "btnSaveLayout";
            this.btnSaveLayout.Size = new System.Drawing.Size(77, 25);
            this.btnSaveLayout.TabIndex = 2;
            this.btnSaveLayout.Text = "Save Layout";
            this.btnSaveLayout.Click += new System.EventHandler(this.btnSaveLayout_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnApprove.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnApprove.Location = new System.Drawing.Point(258, 10);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(86, 23);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "Approve";
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(832, 492);
            this.ultraPanel1.TabIndex = 1;
            // 
            // ultraStatusBarSymbolMgt
            // 
            this.ultraStatusBarSymbolMgt.Location = new System.Drawing.Point(0, 492);
            this.ultraStatusBarSymbolMgt.Name = "ultraStatusBarSymbolMgt";
            this.ultraStatusBarSymbolMgt.Size = new System.Drawing.Size(832, 23);
            this.ultraStatusBarSymbolMgt.TabIndex = 4;
            // 
            // ctrlSymbolManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this.ultraStatusBarSymbolMgt);
            this.Name = "ctrlSymbolManagement";
            this.Size = new System.Drawing.Size(832, 515);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtFailedValidation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValidationAPI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValidationSecMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalSymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEndDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdReport)).EndInit();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBarSymbolMgt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdReport;
        private Infragistics.Win.Misc.UltraLabel lblTotalSymbol;
        private Infragistics.Win.Misc.UltraLabel lblStartDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtStartDate;
        private Infragistics.Win.Misc.UltraLabel lblFailedValidation;
        private Infragistics.Win.Misc.UltraLabel lblValidationAPI;
        private Infragistics.Win.Misc.UltraLabel lblValidationSecMaster;
        private Infragistics.Win.Misc.UltraLabel lblEndDate;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFailedValidation;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtValidationAPI;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtValidationSecMaster;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtTotalSymbol;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtEndDate;
        private Infragistics.Win.Misc.UltraButton btnSymbolLookUp;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Infragistics.Win.Misc.UltraButton btnExport;
        private Infragistics.Win.Misc.UltraButton btnSaveLayout;
        private Infragistics.Win.Misc.UltraButton btnApprove;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton btnValidate;
        internal Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBarSymbolMgt;
    }
}
