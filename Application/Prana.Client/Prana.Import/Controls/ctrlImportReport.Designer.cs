namespace Prana.Import.Controls
{
    partial class ctrlImportReport
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
                if (crlSymbolMismatch != null)
                {
                    crlSymbolMismatch.Dispose();
                }
                if (_runUpload != null)
                {
                    _runUpload.Dispose();
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
            this.txtSymPending = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtSymFail = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtSymValid = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txttotalSymbol = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtFileType = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtAccount = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtThirdParty = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtStartDate = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblSymbolPendingApproval = new Infragistics.Win.Misc.UltraLabel();
            this.lblSymbolValidnImported = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccount = new Infragistics.Win.Misc.UltraLabel();
            this.lblSymbolFailValidate = new Infragistics.Win.Misc.UltraLabel();
            this.lblThirdParty = new Infragistics.Win.Misc.UltraLabel();
            this.lblTotalSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.lblFileType = new Infragistics.Win.Misc.UltraLabel();
            this.lblStartDate = new Infragistics.Win.Misc.UltraLabel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.grdReport = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuExport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImport = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.ultraStatusBar1 = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymPending)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymFail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymValid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txttotalSymbol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtThirdParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdReport)).BeginInit();
            this.mnuExport.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtSymPending);
            this.splitContainer1.Panel1.Controls.Add(this.txtSymFail);
            this.splitContainer1.Panel1.Controls.Add(this.txtSymValid);
            this.splitContainer1.Panel1.Controls.Add(this.txttotalSymbol);
            this.splitContainer1.Panel1.Controls.Add(this.txtFileType);
            this.splitContainer1.Panel1.Controls.Add(this.txtAccount);
            this.splitContainer1.Panel1.Controls.Add(this.txtThirdParty);
            this.splitContainer1.Panel1.Controls.Add(this.txtStartDate);
            this.splitContainer1.Panel1.Controls.Add(this.lblSymbolPendingApproval);
            this.splitContainer1.Panel1.Controls.Add(this.lblSymbolValidnImported);
            this.splitContainer1.Panel1.Controls.Add(this.lblAccount);
            this.splitContainer1.Panel1.Controls.Add(this.lblSymbolFailValidate);
            this.splitContainer1.Panel1.Controls.Add(this.lblThirdParty);
            this.splitContainer1.Panel1.Controls.Add(this.lblTotalSymbol);
            this.splitContainer1.Panel1.Controls.Add(this.lblFileType);
            this.splitContainer1.Panel1.Controls.Add(this.lblStartDate);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(719, 448);
            this.splitContainer1.SplitterDistance = 172;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtSymPending
            // 
            this.txtSymPending.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSymPending.Location = new System.Drawing.Point(514, 112);
            this.txtSymPending.Name = "txtSymPending";
            this.txtSymPending.ReadOnly = true;
            this.txtSymPending.Size = new System.Drawing.Size(106, 22);
            this.txtSymPending.TabIndex = 15;
            // 
            // txtSymFail
            // 
            this.txtSymFail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSymFail.Location = new System.Drawing.Point(514, 65);
            this.txtSymFail.Name = "txtSymFail";
            this.txtSymFail.ReadOnly = true;
            this.txtSymFail.Size = new System.Drawing.Size(106, 22);
            this.txtSymFail.TabIndex = 14;
            // 
            // txtSymValid
            // 
            this.txtSymValid.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSymValid.Location = new System.Drawing.Point(292, 112);
            this.txtSymValid.Name = "txtSymValid";
            this.txtSymValid.ReadOnly = true;
            this.txtSymValid.Size = new System.Drawing.Size(106, 22);
            this.txtSymValid.TabIndex = 13;
            // 
            // txttotalSymbol
            // 
            this.txttotalSymbol.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttotalSymbol.Location = new System.Drawing.Point(292, 65);
            this.txttotalSymbol.Name = "txttotalSymbol";
            this.txttotalSymbol.ReadOnly = true;
            this.txttotalSymbol.Size = new System.Drawing.Size(106, 22);
            this.txttotalSymbol.TabIndex = 12;
            // 
            // txtFileType
            // 
            this.txtFileType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileType.Location = new System.Drawing.Point(292, 18);
            this.txtFileType.Name = "txtFileType";
            this.txtFileType.ReadOnly = true;
            this.txtFileType.Size = new System.Drawing.Size(106, 22);
            this.txtFileType.TabIndex = 11;
            // 
            // txtAccount
            // 
            this.txtAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccount.Location = new System.Drawing.Point(99, 112);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.ReadOnly = true;
            this.txtAccount.Size = new System.Drawing.Size(106, 22);
            this.txtAccount.TabIndex = 10;
            // 
            // txtThirdParty
            // 
            this.txtThirdParty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtThirdParty.Location = new System.Drawing.Point(99, 65);
            this.txtThirdParty.Name = "txtThirdParty";
            this.txtThirdParty.ReadOnly = true;
            this.txtThirdParty.Size = new System.Drawing.Size(106, 22);
            this.txtThirdParty.TabIndex = 9;
            // 
            // txtStartDate
            // 
            this.txtStartDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStartDate.Location = new System.Drawing.Point(99, 18);
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.ReadOnly = true;
            this.txtStartDate.Size = new System.Drawing.Size(106, 22);
            this.txtStartDate.TabIndex = 8;
            // 
            // lblSymbolPendingApproval
            // 
            this.lblSymbolPendingApproval.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbolPendingApproval.Location = new System.Drawing.Point(404, 112);
            this.lblSymbolPendingApproval.Name = "lblSymbolPendingApproval";
            this.lblSymbolPendingApproval.Size = new System.Drawing.Size(104, 48);
            this.lblSymbolPendingApproval.TabIndex = 7;
            this.lblSymbolPendingApproval.Text = "# Symbols Pending Approval";
            // 
            // lblSymbolValidnImported
            // 
            this.lblSymbolValidnImported.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbolValidnImported.Location = new System.Drawing.Point(213, 112);
            this.lblSymbolValidnImported.Name = "lblSymbolValidnImported";
            this.lblSymbolValidnImported.Size = new System.Drawing.Size(73, 48);
            this.lblSymbolValidnImported.TabIndex = 6;
            this.lblSymbolValidnImported.Text = "# of Symbols Validated and Imported";
            // 
            // lblAccount
            // 
            this.lblAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccount.Location = new System.Drawing.Point(16, 112);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(73, 22);
            this.lblAccount.TabIndex = 5;
            this.lblAccount.Text = "# Accounts";
            // 
            // lblSymbolFailValidate
            // 
            this.lblSymbolFailValidate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbolFailValidate.Location = new System.Drawing.Point(408, 65);
            this.lblSymbolFailValidate.Name = "lblSymbolFailValidate";
            this.lblSymbolFailValidate.Size = new System.Drawing.Size(89, 48);
            this.lblSymbolFailValidate.TabIndex = 4;
            this.lblSymbolFailValidate.Text = "# Symbol Failed Validation";
            // 
            // lblThirdParty
            // 
            this.lblThirdParty.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThirdParty.Location = new System.Drawing.Point(16, 65);
            this.lblThirdParty.Name = "lblThirdParty";
            this.lblThirdParty.Size = new System.Drawing.Size(73, 22);
            this.lblThirdParty.TabIndex = 3;
            this.lblThirdParty.Text = "Third Party";
            // 
            // lblTotalSymbol
            // 
            this.lblTotalSymbol.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalSymbol.Location = new System.Drawing.Point(213, 65);
            this.lblTotalSymbol.Name = "lblTotalSymbol";
            this.lblTotalSymbol.Size = new System.Drawing.Size(73, 41);
            this.lblTotalSymbol.TabIndex = 2;
            this.lblTotalSymbol.Text = "Total # Symbols";
            // 
            // lblFileType
            // 
            this.lblFileType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFileType.Location = new System.Drawing.Point(213, 18);
            this.lblFileType.Name = "lblFileType";
            this.lblFileType.Size = new System.Drawing.Size(73, 22);
            this.lblFileType.TabIndex = 1;
            this.lblFileType.Text = "File Type";
            // 
            // lblStartDate
            // 
            this.lblStartDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.Location = new System.Drawing.Point(16, 22);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(73, 18);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "Start Date";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
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
            this.splitContainer2.Panel2.Controls.Add(this.btnImport);
            this.splitContainer2.Size = new System.Drawing.Size(719, 272);
            this.splitContainer2.SplitterDistance = 240;
            this.splitContainer2.TabIndex = 1;
            // 
            // grdReport
            // 
            this.grdReport.ContextMenuStrip = this.mnuExport;
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
            this.grdReport.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdReport.Location = new System.Drawing.Point(0, 0);
            this.grdReport.Name = "grdReport";
            this.grdReport.Size = new System.Drawing.Size(719, 240);
            this.grdReport.TabIndex = 0;
            this.grdReport.Text = "ultraGrid1";
            this.grdReport.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReport_AfterCellUpdate);
            this.grdReport.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdReport_InitializeLayout);
            this.grdReport.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdReport_InitializeRow);
            this.grdReport.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReport_ClickCellButton);
            this.grdReport.BeforeHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventHandler(this.grdReport_BeforeHeaderCheckStateChanged);
            this.grdReport.AfterHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventHandler(this.grdReport_AfterHeaderCheckStateChanged);
            // 
            // mnuExport
            // 
            this.mnuExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.saveLayoutMenuItem});
            this.mnuExport.Name = "mnuExport";
            this.mnuExport.Size = new System.Drawing.Size(138, 48);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // saveLayoutMenuItem
            // 
            this.saveLayoutMenuItem.Name = "saveLayoutMenuItem";
            this.saveLayoutMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayoutMenuItem.Text = "Save Layout";
            this.saveLayoutMenuItem.Click += new System.EventHandler(this.saveLayoutMenuItem_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnImport.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnImport.Location = new System.Drawing.Point(322, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer3);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(719, 477);
            this.ultraPanel1.TabIndex = 1;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.ultraStatusBar1);
            this.splitContainer3.Size = new System.Drawing.Size(719, 477);
            this.splitContainer3.SplitterDistance = 448;
            this.splitContainer3.TabIndex = 1;
            // 
            // ultraStatusBar1
            // 
            this.ultraStatusBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraStatusBar1.Location = new System.Drawing.Point(0, 0);
            this.ultraStatusBar1.Name = "ultraStatusBar1";
            this.ultraStatusBar1.Size = new System.Drawing.Size(719, 25);
            this.ultraStatusBar1.TabIndex = 0;
            this.ultraStatusBar1.Text = "ultraStatusBar1";
            // 
            // ctrlImportReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlImportReport";
            this.Size = new System.Drawing.Size(719, 477);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSymPending)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymFail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymValid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txttotalSymbol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFileType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtThirdParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartDate)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdReport)).EndInit();
            this.mnuExport.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBar1)).EndInit();
            this.ResumeLayout(false);

        }

        
        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdReport;
        private Infragistics.Win.Misc.UltraLabel lblSymbolPendingApproval;
        private Infragistics.Win.Misc.UltraLabel lblSymbolValidnImported;
        private Infragistics.Win.Misc.UltraLabel lblAccount;
        private Infragistics.Win.Misc.UltraLabel lblSymbolFailValidate;
        private Infragistics.Win.Misc.UltraLabel lblThirdParty;
        private Infragistics.Win.Misc.UltraLabel lblTotalSymbol;
        private Infragistics.Win.Misc.UltraLabel lblFileType;
        private Infragistics.Win.Misc.UltraLabel lblStartDate;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStartDate;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSymPending;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSymFail;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSymValid;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txttotalSymbol;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFileType;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAccount;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtThirdParty;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private System.Windows.Forms.ContextMenuStrip mnuExport;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Infragistics.Win.Misc.UltraButton btnImport;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBar1;
    }
}
