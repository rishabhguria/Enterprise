namespace Prana.Tools
{
    partial class ctrlSMBatchSetup
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
                if (frmBatch != null)
                {
                    frmBatch.Dispose();
                }
                if (ctrl != null)
                {
                    ctrl.Dispose();
                }
                if (ctrlTaskScheduler != null)
                {
                    ctrlTaskScheduler.Dispose();
                }
                if (_vlRunTimeType != null)
                {
                    _vlRunTimeType.Dispose();
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.rbSetBatchType = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.btnDelete = new Infragistics.Win.Misc.UltraButton();
            this.ultraDateTimeEditor1 = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblStartDate = new Infragistics.Win.Misc.UltraLabel();
            this.btnEdit = new Infragistics.Win.Misc.UltraButton();
            this.btnRun = new Infragistics.Win.Misc.UltraButton();
            this.btnCreate = new Infragistics.Win.Misc.UltraButton();
            this.grdSMBatch = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rbSetBatchType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSMBatch)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.grdSMBatch);
            this.splitContainer1.Size = new System.Drawing.Size(658, 408);
            this.splitContainer1.SplitterDistance = 70;
            this.splitContainer1.TabIndex = 0;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.rbSetBatchType);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnDelete);
            this.ultraPanel1.ClientArea.Controls.Add(this.ultraDateTimeEditor1);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblStartDate);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnEdit);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnRun);
            this.ultraPanel1.ClientArea.Controls.Add(this.btnCreate);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(658, 70);
            this.ultraPanel1.TabIndex = 0;
            // 
            // rbSetBatchType
            // 
            this.rbSetBatchType.CheckedIndex = 0;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = 0;
            valueListItem1.DisplayText = "Historical Batch";
            valueListItem2.DataValue = 1;
            valueListItem2.DisplayText = "Current Batch";
            this.rbSetBatchType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.rbSetBatchType.Location = new System.Drawing.Point(202, 10);
            this.rbSetBatchType.Name = "rbSetBatchType";
            this.rbSetBatchType.Size = new System.Drawing.Size(103, 39);
            this.rbSetBatchType.TabIndex = 14;
            this.rbSetBatchType.Text = "Historical Batch";
            this.rbSetBatchType.ValueChanged += new System.EventHandler(this.rbSet_ValueChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(421, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 23);
            this.btnDelete.TabIndex = 13;
            this.btnDelete.Text = "Delete Batch";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ultraDateTimeEditor1
            // 
            this.ultraDateTimeEditor1.Location = new System.Drawing.Point(49, 16);
            this.ultraDateTimeEditor1.Name = "ultraDateTimeEditor1";
            this.ultraDateTimeEditor1.Size = new System.Drawing.Size(150, 21);
            this.ultraDateTimeEditor1.TabIndex = 12;
            // 
            // lblStartDate
            // 
            appearance17.TextHAlignAsString = "Center";
            appearance17.TextVAlignAsString = "Middle";
            this.lblStartDate.Appearance = appearance17;
            this.lblStartDate.Location = new System.Drawing.Point(8, 15);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(38, 23);
            this.lblStartDate.TabIndex = 11;
            this.lblStartDate.Text = "Date";
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(601, 15);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(53, 23);
            this.btnEdit.TabIndex = 10;
            this.btnEdit.Text = "Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(514, 15);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(84, 23);
            this.btnRun.TabIndex = 10;
            this.btnRun.Text = "Run Batch";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(308, 15);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(110, 23);
            this.btnCreate.TabIndex = 9;
            this.btnCreate.Text = "Create New Batch";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // grdSMBatch
            // 
            this.grdSMBatch.ContextMenuStrip = this.contextMenuStrip1;
            this.grdSMBatch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdSMBatch.DisplayLayout.Appearance = appearance2;
            this.grdSMBatch.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdSMBatch.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.grdSMBatch.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdSMBatch.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.grdSMBatch.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdSMBatch.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.grdSMBatch.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSMBatch.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            appearance14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grdSMBatch.DisplayLayout.Override.RowSelectorAppearance = appearance14;
            this.grdSMBatch.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdSMBatch.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdSMBatch.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdSMBatch.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdSMBatch.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.grdSMBatch.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdSMBatch.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.grdSMBatch.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdSMBatch.DisplayLayout.Override.CellAppearance = appearance9;
            this.grdSMBatch.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdSMBatch.DisplayLayout.Override.CellPadding = 0;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.BorderColor = System.Drawing.SystemColors.Window;
            this.grdSMBatch.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.TextHAlignAsString = "Left";
            this.grdSMBatch.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.grdSMBatch.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdSMBatch.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.grdSMBatch.DisplayLayout.Override.RowAppearance = appearance12;
            this.grdSMBatch.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdSMBatch.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.grdSMBatch.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdSMBatch.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdSMBatch.Location = new System.Drawing.Point(13, 12);
            this.grdSMBatch.Name = "grdSMBatch";
            this.grdSMBatch.Size = new System.Drawing.Size(632, 309);
            this.grdSMBatch.TabIndex = 0;
            this.grdSMBatch.Text = "ultraGrid1";
            this.grdSMBatch.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdSMBatch_InitializeLayout);
            this.grdSMBatch.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdSMBatch_InitializeRow);
            this.grdSMBatch.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSMBatch_CellChange);
            this.grdSMBatch.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdSMBatch_ClickCellButton);
            this.grdSMBatch.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.grdSMBatch_BeforeCellUpdate);
            this.grdSMBatch.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdSMBatch_BeforeCustomRowFilterDialog);
            this.grdSMBatch.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdSMBatch_BeforeColumnChooserDisplayed);

            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLayoutToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 26);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // ctrlSMBatchSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ctrlSMBatchSetup";
            this.Size = new System.Drawing.Size(658, 408);
            this.Load += new System.EventHandler(this.ctrlSMBatchSetup_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ClientArea.PerformLayout();
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rbSetBatchType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDateTimeEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSMBatch)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSMBatch;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraButton btnDelete;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor ultraDateTimeEditor1;
        private Infragistics.Win.Misc.UltraLabel lblStartDate;
        private Infragistics.Win.Misc.UltraButton btnRun;
        private Infragistics.Win.Misc.UltraButton btnCreate;
        private Infragistics.Win.Misc.UltraButton btnEdit;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet rbSetBatchType;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;

    }
}
