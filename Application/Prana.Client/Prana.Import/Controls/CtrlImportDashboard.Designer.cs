namespace Prana.Import.Controls
{
    partial class CtrlImportDashboard
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
                if (_timerRefresh != null)
                {
                    _timerRefresh.Dispose();
                }
                if (_grdReport != null)
                {
                    _grdReport.Dispose();
                }
                if (_frmImportStatusReport != null)
                {
                    _frmImportStatusReport.Dispose();
                }
                if (_frmSymbolManagement != null)
                {
                    _frmSymbolManagement.Dispose();
                }
                if (_frmReport != null)
                {
                    _frmReport.Dispose();
                }
                if (_frmImportTag != null)
                {
                    _frmImportTag.Dispose();
                }
                if (_frmUploadedFile != null)
                {
                    _frmUploadedFile.Dispose();
                }
                if (_frmUploadNow != null)
                {
                    _frmUploadNow.Dispose();
                }
                if (_ctrlImportTag != null)
                {
                    _ctrlImportTag.Dispose();
                }
                if (_ctrlsymbolManagement != null)
                {
                    _ctrlsymbolManagement.Dispose();
                }
                if (_ctrlImportStatusReport != null)
                {
                    _ctrlImportStatusReport.Dispose();
                }
                if (_ctrlReport != null)
                {
                    _ctrlReport.Dispose();
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
            this.grdImportDashboard = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllGridFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDataMapping = new Infragistics.Win.Misc.UltraButton();
            this.txtLastRunDate = new System.Windows.Forms.TextBox();
            this.txtLastRunTime = new System.Windows.Forms.TextBox();
            this.btnValidate = new Infragistics.Win.Misc.UltraButton();
            this.btnUpload = new Infragistics.Win.Misc.UltraButton();
            this.lblLastRunTime = new Infragistics.Win.Misc.UltraLabel();
            this.lblLastRunDate = new Infragistics.Win.Misc.UltraLabel();
            this.btnPurge = new Infragistics.Win.Misc.UltraButton();
            this.btnImport = new Infragistics.Win.Misc.UltraButton();
            this.btnArchive = new Infragistics.Win.Misc.UltraButton();
            this.bgImportIntoApp = new System.ComponentModel.BackgroundWorker();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.grdImportDashboard)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdImportDashboard
            // 
            this.grdImportDashboard.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdImportDashboard.DisplayLayout.Appearance = appearance1;
            this.grdImportDashboard.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdImportDashboard.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdImportDashboard.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdImportDashboard.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdImportDashboard.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdImportDashboard.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdImportDashboard.DisplayLayout.MaxColScrollRegions = 1;
            this.grdImportDashboard.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdImportDashboard.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdImportDashboard.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdImportDashboard.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdImportDashboard.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdImportDashboard.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdImportDashboard.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdImportDashboard.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdImportDashboard.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdImportDashboard.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdImportDashboard.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdImportDashboard.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdImportDashboard.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            appearance11.FontData.BoldAsString = "False";
            this.grdImportDashboard.DisplayLayout.Override.RowAppearance = appearance11;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdImportDashboard.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdImportDashboard.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdImportDashboard.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdImportDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdImportDashboard.Location = new System.Drawing.Point(0, 0);
            this.grdImportDashboard.Name = "grdImportDashboard";
            this.grdImportDashboard.Size = new System.Drawing.Size(750, 241);
            this.grdImportDashboard.TabIndex = 29;
            this.grdImportDashboard.Text = "ultraGrid1";
            this.grdImportDashboard.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdImportDashboard_InitializeLayout);
            this.grdImportDashboard.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdImportDashboard_InitializeRow);
            this.grdImportDashboard.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdImportDashboard_ClickCellButton);
            this.grdImportDashboard.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdImportDashboard_BeforeCustomRowFilterDialog);
            this.grdImportDashboard.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdImportDashboard_BeforeColumnChooserDisplayed);
            this.grdImportDashboard.BeforeHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventHandler(this.grdImportDashboard_BeforeHeaderCheckStateChanged);
            this.grdImportDashboard.AfterHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventHandler(this.grdImportDashboard_AfterHeaderCheckStateChanged);
            this.grdImportDashboard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdImportDashboard_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem,this.clearAllGridFiltersToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 26);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // clearAllGridFiltersToolStripMenuItem
            // 
            this.clearAllGridFiltersToolStripMenuItem.Name = "clearAllGridFiltersToolStripMenuItem";
            this.clearAllGridFiltersToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.clearAllGridFiltersToolStripMenuItem.Text = "Default layout";
            this.clearAllGridFiltersToolStripMenuItem.Click += new System.EventHandler(this.clearAllGridFiltersToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnPurge);
            this.splitContainer1.Panel2.Controls.Add(this.btnImport);
            this.splitContainer1.Panel2.Controls.Add(this.btnArchive);
            this.splitContainer1.Size = new System.Drawing.Size(750, 328);
            this.splitContainer1.SplitterDistance = 288;
            this.splitContainer1.TabIndex = 30;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.grdImportDashboard);
            this.splitContainer2.Size = new System.Drawing.Size(750, 288);
            this.splitContainer2.SplitterDistance = 43;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDataMapping);
            this.groupBox1.Controls.Add(this.txtLastRunDate);
            this.groupBox1.Controls.Add(this.txtLastRunTime);
            this.groupBox1.Controls.Add(this.btnValidate);
            this.groupBox1.Controls.Add(this.btnUpload);
            this.groupBox1.Controls.Add(this.lblLastRunTime);
            this.groupBox1.Controls.Add(this.lblLastRunDate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(750, 43);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnDataMapping
            // 
            this.btnDataMapping.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDataMapping.Location = new System.Drawing.Point(650, 13);
            this.btnDataMapping.Name = "btnDataMapping";
            this.btnDataMapping.Size = new System.Drawing.Size(87, 28);
            this.btnDataMapping.TabIndex = 8;
            this.btnDataMapping.Text = "Data Mapping";
            this.btnDataMapping.Click += new System.EventHandler(this.btnDataMapping_Click);
            // 
            // txtLastRunDate
            // 
            this.txtLastRunDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastRunDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastRunDate.Location = new System.Drawing.Point(87, 18);
            this.txtLastRunDate.Name = "txtLastRunDate";
            this.txtLastRunDate.ReadOnly = true;
            this.txtLastRunDate.Size = new System.Drawing.Size(100, 20);
            this.txtLastRunDate.TabIndex = 7;
            // 
            // txtLastRunTime
            // 
            this.txtLastRunTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastRunTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastRunTime.Location = new System.Drawing.Point(275, 18);
            this.txtLastRunTime.Name = "txtLastRunTime";
            this.txtLastRunTime.ReadOnly = true;
            this.txtLastRunTime.Size = new System.Drawing.Size(100, 20);
            this.txtLastRunTime.TabIndex = 6;
            // 
            // btnValidate
            // 
            this.btnValidate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValidate.Location = new System.Drawing.Point(509, 13);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(128, 28);
            this.btnValidate.TabIndex = 5;
            this.btnValidate.Text = "Proceed To Validation";
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.Location = new System.Drawing.Point(391, 13);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(106, 28);
            this.btnUpload.TabIndex = 4;
            this.btnUpload.Text = "Upload File Now";
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lblLastRunTime
            // 
            this.lblLastRunTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastRunTime.Location = new System.Drawing.Point(194, 18);
            this.lblLastRunTime.Name = "lblLastRunTime";
            this.lblLastRunTime.Size = new System.Drawing.Size(100, 13);
            this.lblLastRunTime.TabIndex = 3;
            this.lblLastRunTime.Text = "Last Run Time";
            // 
            // lblLastRunDate
            // 
            this.lblLastRunDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastRunDate.Location = new System.Drawing.Point(6, 18);
            this.lblLastRunDate.Name = "lblLastRunDate";
            this.lblLastRunDate.Size = new System.Drawing.Size(100, 13);
            this.lblLastRunDate.TabIndex = 1;
            this.lblLastRunDate.Text = "Last Run Date";
            // 
            // btnPurge
            // 
            this.btnPurge.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPurge.Location = new System.Drawing.Point(419, 6);
            this.btnPurge.Name = "btnPurge";
            this.btnPurge.Size = new System.Drawing.Size(75, 25);
            this.btnPurge.TabIndex = 2;
            this.btnPurge.Text = "Purge";
            this.btnPurge.Click += new System.EventHandler(this.btnPurge_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnImport.Location = new System.Drawing.Point(326, 6);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 25);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnArchive
            // 
            this.btnArchive.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnArchive.Location = new System.Drawing.Point(230, 6);
            this.btnArchive.Name = "btnArchive";
            this.btnArchive.Size = new System.Drawing.Size(75, 25);
            this.btnArchive.TabIndex = 0;
            this.btnArchive.Text = "Archive";
            this.btnArchive.Click += new System.EventHandler(this.btnArchive_Click);
            // 
            // bgImportIntoApp
            // 
            this.bgImportIntoApp.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgImportIntoApp_DoWork);
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer1);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(750, 328);
            this.ultraPanel1.TabIndex = 31;
            // 
            // CtrlImportDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "CtrlImportDashboard";
            this.Size = new System.Drawing.Size(750, 328);
            ((System.ComponentModel.ISupportInitialize)(this.grdImportDashboard)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdImportDashboard;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Infragistics.Win.Misc.UltraButton btnArchive;
        private Infragistics.Win.Misc.UltraButton btnPurge;
        private Infragistics.Win.Misc.UltraButton btnImport;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraLabel lblLastRunTime;
        private Infragistics.Win.Misc.UltraLabel lblLastRunDate;
        private Infragistics.Win.Misc.UltraButton btnValidate;
        private Infragistics.Win.Misc.UltraButton btnUpload;
        private System.Windows.Forms.TextBox txtLastRunDate;
        private System.Windows.Forms.TextBox txtLastRunTime;
        private Infragistics.Win.Misc.UltraButton btnDataMapping;
        private System.ComponentModel.BackgroundWorker bgImportIntoApp;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllGridFiltersToolStripMenuItem;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
    }
}
