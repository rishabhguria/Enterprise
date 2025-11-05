namespace Prana.Tools
{
    partial class ctrlMasterColumns
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
                if (dtNirvanaGridDisplayColumns != null)
                {
                    dtNirvanaGridDisplayColumns.Dispose();
                }
                if (dtPBGridDisplayColumns != null)
                {
                    dtPBGridDisplayColumns.Dispose();
                }
                if (_reconTemplate != null)
                {
                    _reconTemplate.Dispose();
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grdMasterColumnsNirvana = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grdMasterColumnsPB = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuAddCustomColumnsNirvana = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblNirvana = new Infragistics.Win.Misc.UltraLabel();
            this.lblPrimeBroker = new Infragistics.Win.Misc.UltraLabel();
            this.btn_addRow_Nirvana = new Infragistics.Win.Misc.UltraButton();
            this.mnuAddCustomColumnsPB = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMasterColumnsNirvana)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMasterColumnsPB)).BeginInit();
            this.mnuAddCustomColumnsNirvana.SuspendLayout();
            this.mnuAddCustomColumnsPB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grdMasterColumnsNirvana);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdMasterColumnsPB);
            this.splitContainer1.Size = new System.Drawing.Size(908, 374);
            this.splitContainer1.SplitterDistance = 454;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 5;
            // 
            // grdMasterColumnsNirvana
            // 
            this.grdMasterColumnsNirvana.AllowDrop = true;
            this.grdMasterColumnsNirvana.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grdMasterColumnsNirvana.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdMasterColumnsNirvana.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMasterColumnsNirvana.DisplayLayout.GroupByBox.Hidden = true;
            this.grdMasterColumnsNirvana.DisplayLayout.MaxColScrollRegions = 1;
            this.grdMasterColumnsNirvana.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.CellPadding = 0;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.Never;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdMasterColumnsNirvana.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.grdMasterColumnsNirvana.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdMasterColumnsNirvana.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdMasterColumnsNirvana.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdMasterColumnsNirvana.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMasterColumnsNirvana.Location = new System.Drawing.Point(0, 0);
            this.grdMasterColumnsNirvana.Name = "grdMasterColumnsNirvana";
            this.grdMasterColumnsNirvana.Size = new System.Drawing.Size(454, 374);
            this.grdMasterColumnsNirvana.TabIndex = 4;
            this.grdMasterColumnsNirvana.Text = "ultraGrid1";
            this.grdMasterColumnsNirvana.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grdMasterColumnsNirvana.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMasterColumnsNirvana_AfterCellUpdate);
            this.grdMasterColumnsNirvana.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdMasterColumnsNirvana_InitializeLayout);
            this.grdMasterColumnsNirvana.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdMasterColumnsNirvana_InitializeRow);
            this.grdMasterColumnsNirvana.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMasterColumnsNirvana_CellChange);
            this.grdMasterColumnsNirvana.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdMasterColumnsNirvana_MouseDown);
            this.grdMasterColumnsNirvana.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdMasterColumnsNirvana_BeforeColumnChooserDisplayed);
            this.grdMasterColumnsNirvana.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdMasterColumnsNirvana_BeforeCustomRowFilterDialog);
            // 
            // grdMasterColumnsPB
            // 
            this.grdMasterColumnsPB.AllowDrop = true;
            this.grdMasterColumnsPB.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.grdMasterColumnsPB.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdMasterColumnsPB.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMasterColumnsPB.DisplayLayout.GroupByBox.Hidden = true;
            this.grdMasterColumnsPB.DisplayLayout.MaxColScrollRegions = 1;
            this.grdMasterColumnsPB.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdMasterColumnsPB.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdMasterColumnsPB.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdMasterColumnsPB.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMasterColumnsPB.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdMasterColumnsPB.DisplayLayout.Override.CellPadding = 0;
            this.grdMasterColumnsPB.DisplayLayout.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.Never;
            this.grdMasterColumnsPB.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
            this.grdMasterColumnsPB.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.grdMasterColumnsPB.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdMasterColumnsPB.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdMasterColumnsPB.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed;
            this.grdMasterColumnsPB.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdMasterColumnsPB.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdMasterColumnsPB.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdMasterColumnsPB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMasterColumnsPB.Location = new System.Drawing.Point(0, 0);
            this.grdMasterColumnsPB.Name = "grdMasterColumnsPB";
            this.grdMasterColumnsPB.Size = new System.Drawing.Size(452, 374);
            this.grdMasterColumnsPB.TabIndex = 3;
            this.grdMasterColumnsPB.Text = "ultraGrid1";
            this.grdMasterColumnsPB.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChange;
            this.grdMasterColumnsPB.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMasterColumnsPB_AfterCellUpdate);
            this.grdMasterColumnsPB.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdMasterColumnsPB_InitializeLayout);
            this.grdMasterColumnsPB.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdMasterColumnsPB_InitializeRow);
            this.grdMasterColumnsPB.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdMasterColumnsPB_CellChange);
            this.grdMasterColumnsPB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdMasterColumnsPB_MouseDown);
            this.grdMasterColumnsPB.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdMasterColumnsPB_BeforeColumnChooserDisplayed);
            this.grdMasterColumnsPB.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdMasterColumnsPB_BeforeCustomRowFilterDialog);
            // 
            // mnuAddCustomColumnsNirvana
            // 
            this.mnuAddCustomColumnsNirvana.AllowDrop = true;
            this.mnuAddCustomColumnsNirvana.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.mnuAddCustomColumnsNirvana.Name = "contextMenuStrip1";
            this.mnuAddCustomColumnsNirvana.Size = new System.Drawing.Size(108, 48);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItemNirvana_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItemNirvana_Click);
            // 
            // lblNirvana
            // 
            this.lblNirvana.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblNirvana.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblNirvana.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNirvana.Location = new System.Drawing.Point(196, 6);
            this.lblNirvana.Name = "lblNirvana";
            this.lblNirvana.Size = new System.Drawing.Size(63, 20);
            this.lblNirvana.TabIndex = 8;
            this.lblNirvana.Text = "NIRVANA";
            // 
            // lblPrimeBroker
            // 
            this.lblPrimeBroker.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblPrimeBroker.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.lblPrimeBroker.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblPrimeBroker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrimeBroker.Location = new System.Drawing.Point(173, 6);
            this.lblPrimeBroker.Name = "lblPrimeBroker";
            this.lblPrimeBroker.Size = new System.Drawing.Size(105, 20);
            this.lblPrimeBroker.TabIndex = 9;
            this.lblPrimeBroker.Text = "PRIME BROKER";
            // 
            // btn_addRow_Nirvana
            // 
            this.btn_addRow_Nirvana.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_addRow_Nirvana.Location = new System.Drawing.Point(361, 5);
            this.btn_addRow_Nirvana.Name = "btn_addRow_Nirvana";
            this.btn_addRow_Nirvana.Size = new System.Drawing.Size(186, 23);
            this.btn_addRow_Nirvana.TabIndex = 10;
            this.btn_addRow_Nirvana.Text = "Add Custom Column";
            this.btn_addRow_Nirvana.Click += new System.EventHandler(this.btn_addCustomColumn_Click);
            // 
            // mnuAddCustomColumnsPB
            // 
            this.mnuAddCustomColumnsPB.AllowDrop = true;
            this.mnuAddCustomColumnsPB.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.mnuAddCustomColumnsPB.Name = "contextMenuStrip1";
            this.mnuAddCustomColumnsPB.Size = new System.Drawing.Size(95, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem1.Text = "Edit";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.editToolStripMenuItemPB_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lblNirvana);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lblPrimeBroker);
            this.splitContainer2.Size = new System.Drawing.Size(908, 27);
            this.splitContainer2.SplitterDistance = 454;
            this.splitContainer2.SplitterWidth = 2;
            this.splitContainer2.TabIndex = 11;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer3.Size = new System.Drawing.Size(908, 405);
            this.splitContainer3.SplitterDistance = 27;
            this.splitContainer3.TabIndex = 12;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.btn_addRow_Nirvana);
            this.splitContainer4.Size = new System.Drawing.Size(908, 441);
            this.splitContainer4.SplitterDistance = 405;
            this.splitContainer4.TabIndex = 13;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.splitContainer4);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(908, 441);
            this.ultraPanel1.TabIndex = 14;
            // 
            // ctrlMasterColumns
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraPanel1);
            this.Name = "ctrlMasterColumns";
            this.Size = new System.Drawing.Size(908, 441);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdMasterColumnsNirvana)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdMasterColumnsPB)).EndInit();
            this.mnuAddCustomColumnsNirvana.ResumeLayout(false);
            this.mnuAddCustomColumnsPB.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
  
        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdMasterColumnsPB;
        private Infragistics.Win.Misc.UltraLabel lblNirvana;
        private Infragistics.Win.Misc.UltraLabel lblPrimeBroker;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdMasterColumnsNirvana;
        private System.Windows.Forms.ContextMenuStrip mnuAddCustomColumnsNirvana;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private Infragistics.Win.Misc.UltraButton btn_addRow_Nirvana;
        private System.Windows.Forms.ContextMenuStrip mnuAddCustomColumnsPB;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;

    }
}
