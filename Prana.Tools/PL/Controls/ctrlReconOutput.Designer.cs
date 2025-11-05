using Prana.Utilities.UI.UIUtilities;
namespace Prana.Tools
{
    partial class ctrlReconOutput
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
                if (_pricingServicesProxy != null && !CustomThemeHelper.IsDesignMode())
                {
                    _pricingServicesProxy.Dispose();
                }
                if (components != null)
                {
                    components.Dispose();
                }
                if (dtMarkPrice != null)
                {
                    dtMarkPrice.Dispose();
                }
                if (_reconTemplate != null)
                {
                    _reconTemplate.Dispose();
                }
                if (grdReport != null)
                {
                    grdReport.Dispose();
                }
                if (_frmViewFile != null)
                {
                    _frmViewFile.Dispose();
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
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grdReconOutput = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lockedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unlockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearGridFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewInFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnCopyFailedValues = new Infragistics.Win.Misc.UltraButton();
            this.btnCopyAll = new Infragistics.Win.Misc.UltraButton();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.txtUserComments = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraStatusBarReconStatus = new Infragistics.Win.UltraWinStatusBar.UltraStatusBar();
            this.bgSaveClickWorker = new System.ComponentModel.BackgroundWorker();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdReconOutput)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBarReconStatus)).BeginInit();
            this.SuspendLayout();
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
            this.ultraPanel1.Size = new System.Drawing.Size(900, 363);
            this.ultraPanel1.TabIndex = 0;
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
            this.splitContainer1.Panel1.Controls.Add(this.grdReconOutput);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.ultraStatusBarReconStatus);
            this.splitContainer1.Size = new System.Drawing.Size(900, 363);
            this.splitContainer1.SplitterDistance = 263;
            this.splitContainer1.TabIndex = 7;
            // 
            // grdReconOutput
            // 
            this.grdReconOutput.ContextMenuStrip = this.contextMenuStrip1;
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdReconOutput.DisplayLayout.Appearance = appearance1;
            this.grdReconOutput.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdReconOutput.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdReconOutput.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdReconOutput.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdReconOutput.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdReconOutput.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdReconOutput.DisplayLayout.MaxColScrollRegions = 1;
            this.grdReconOutput.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdReconOutput.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdReconOutput.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdReconOutput.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdReconOutput.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdReconOutput.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdReconOutput.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdReconOutput.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdReconOutput.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdReconOutput.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdReconOutput.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdReconOutput.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdReconOutput.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdReconOutput.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdReconOutput.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdReconOutput.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdReconOutput.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdReconOutput.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdReconOutput.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdReconOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdReconOutput.Location = new System.Drawing.Point(0, 0);
            this.grdReconOutput.Name = "grdReconOutput";
            this.grdReconOutput.Size = new System.Drawing.Size(900, 263);
            this.grdReconOutput.TabIndex = 0;
            this.grdReconOutput.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdReconOutput.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReconOutput_AfterCellUpdate);
            this.grdReconOutput.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdReconOutput_InitializeLayout);
            this.grdReconOutput.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdReconOutput_InitializeRow);
            this.grdReconOutput.BeforeRowActivate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdReconOutput_BeforeRowActivate);
            this.grdReconOutput.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReconOutput_CellChange);
            this.grdReconOutput.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdReconOutput_ClickCellButton);
            this.grdReconOutput.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.grdReconOutput_BeforeExitEditMode);
            this.grdReconOutput.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdReconOutput_BeforeCellUpdate);
            this.grdReconOutput.BeforeHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.BeforeHeaderCheckStateChangedEventHandler(this.grdReconOutput_BeforeHeaderCheckStateChanged);
            this.grdReconOutput.AfterHeaderCheckStateChanged += new Infragistics.Win.UltraWinGrid.AfterHeaderCheckStateChangedEventHandler(this.grdReconOutput_AfterHeaderCheckStateChanged);
            this.grdReconOutput.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdReconOutput_MouseDown);
            this.grdReconOutput.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdReconOutput_BeforeColumnChooserDisplayed);
            this.grdReconOutput.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdReconOutput_BeforeCustomRowFilterDialog);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lockedToolStripMenuItem,
            this.unlockToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.viewInFileToolStripMenuItem,
            this.saveLayoutToTemplate,
            this.clearGridFiltersToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(206, 114);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // lockedToolStripMenuItem
            // 
            this.lockedToolStripMenuItem.Name = "lockedToolStripMenuItem";
            this.lockedToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.lockedToolStripMenuItem.Text = "Lock";
            this.lockedToolStripMenuItem.Click += new System.EventHandler(this.lockedToolStripMenuItem_Click);
            // 
            // unlockToolStripMenuItem
            // 
            this.unlockToolStripMenuItem.Name = "unlockToolStripMenuItem";
            this.unlockToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.unlockToolStripMenuItem.Text = "Unlock";
            this.unlockToolStripMenuItem.Click += new System.EventHandler(this.unlockToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);

            // 
            // clearGridFiltersToolStripMenuItem
            // 
            this.clearGridFiltersToolStripMenuItem.Name = "clearGridFiltersToolStripMenuItem";
            this.clearGridFiltersToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.clearGridFiltersToolStripMenuItem.Text = "Default layout";
            this.clearGridFiltersToolStripMenuItem.Click += new System.EventHandler(this.clearGridFiltersToolStripMenuItem_Click);
            // 
            // viewInFileToolStripMenuItem
            // 
            this.viewInFileToolStripMenuItem.Name = "viewInFileToolStripMenuItem";
            this.viewInFileToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.viewInFileToolStripMenuItem.Text = "View In File";
            this.viewInFileToolStripMenuItem.Click += new System.EventHandler(this.viewInFileToolStripMenuItem_Click);
            // 
            // saveLayoutToTemplate
            // 
            this.saveLayoutToTemplate.Name = "saveLayoutToTemplate";
            this.saveLayoutToTemplate.Size = new System.Drawing.Size(205, 22);
            this.saveLayoutToTemplate.Text = "Save Layout for template";
            this.saveLayoutToTemplate.Click += new System.EventHandler(this.saveLayoutToTemplate_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnCopyFailedValues);
            this.splitContainer2.Panel1.Controls.Add(this.btnCopyAll);
            this.splitContainer2.Panel1.Controls.Add(this.btnDelete);
            this.splitContainer2.Panel1.Controls.Add(this.btnSave);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ultraGroupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(900, 73);
            this.splitContainer2.SplitterDistance = 28;
            this.splitContainer2.TabIndex = 6;
            // 
            // btnCopyFailedValues
            // 
            this.btnCopyFailedValues.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCopyFailedValues.Location = new System.Drawing.Point(268, 4);
            this.btnCopyFailedValues.Name = "btnCopyFailedValues";
            this.btnCopyFailedValues.Size = new System.Drawing.Size(122, 25);
            this.btnCopyFailedValues.TabIndex = 1;
            this.btnCopyFailedValues.Text = "Copy Failed Values";
            this.btnCopyFailedValues.Click += new System.EventHandler(this.btnCopyFailedValues_Click);
            // 
            // btnCopyAll
            // 
            this.btnCopyAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCopyAll.Location = new System.Drawing.Point(411, 4);
            this.btnCopyAll.Name = "btnCopyAll";
            this.btnCopyAll.Size = new System.Drawing.Size(75, 25);
            this.btnCopyAll.TabIndex = 2;
            this.btnCopyAll.Text = "Copy All";
            this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDelete.Location = new System.Drawing.Point(603, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 25);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.Location = new System.Drawing.Point(507, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.txtUserComments);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(900, 41);
            this.ultraGroupBox1.TabIndex = 0;
            this.ultraGroupBox1.Text = "Comments";
            // 
            // txtUserComments
            // 
            this.txtUserComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserComments.Location = new System.Drawing.Point(3, 16);
            this.txtUserComments.Name = "txtUserComments";
            this.txtUserComments.Size = new System.Drawing.Size(894, 21);
            this.txtUserComments.TabIndex = 0;
            // 
            // ultraStatusBarReconStatus
            // 
            this.ultraStatusBarReconStatus.Location = new System.Drawing.Point(0, 73);
            this.ultraStatusBarReconStatus.Name = "ultraStatusBarReconStatus";
            this.ultraStatusBarReconStatus.Size = new System.Drawing.Size(900, 23);
            this.ultraStatusBarReconStatus.TabIndex = 5;
            // 
            // bgSaveClickWorker
            // 
            this.bgSaveClickWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgSaveClickWorker_DoWork);
            this.bgSaveClickWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgSaveClickWorker_RunWorkerCompleted);
            // 
            // ctrlReconOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlReconOutput";
            this.Size = new System.Drawing.Size(900, 363);
            this.Load += new System.EventHandler(this.ctrlReconOutput_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdReconOutput)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraStatusBarReconStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.Misc.UltraButton btnCopyAll;
        private Infragistics.Win.Misc.UltraButton btnCopyFailedValues;
        public Infragistics.Win.UltraWinGrid.UltraGrid grdReconOutput;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem lockedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unlockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearGridFiltersToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        internal Infragistics.Win.UltraWinStatusBar.UltraStatusBar ultraStatusBarReconStatus;
        private System.ComponentModel.BackgroundWorker bgSaveClickWorker;
        private System.Windows.Forms.ToolStripMenuItem viewInFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToTemplate;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtUserComments;
    }
}
