namespace Prana.Tools
{
    partial class MissingTradesUI
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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingTradesUI));
            this.grdMessages = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMnuMissing = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addSymbolManuallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendAlertForTradesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBoxMissingTrades = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraMessageBoxManager1 = new Infragistics.Win.UltraMessageBox.UltraMessageBoxManager(this.components);
            this.btnMissingTrades = new Infragistics.Win.Misc.UltraButton();
            this.grdExclExporter = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.MissingTradesUI_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._MissingTradesUI_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MissingTradesUI_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MissingTradesUI_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdMessages)).BeginInit();
            this.contextMnuMissing.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxMissingTrades)).BeginInit();
            this.grpBoxMissingTrades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.MissingTradesUI_Fill_Panel.ClientArea.SuspendLayout();
            this.MissingTradesUI_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdMessages
            // 
            this.grdMessages.ContextMenuStrip = this.contextMnuMissing;
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "Tahoma";
            appearance1.FontData.SizeInPoints = 8.25F;
            this.grdMessages.DisplayLayout.Appearance = appearance1;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.grdMessages.DisplayLayout.CaptionAppearance = appearance2;
            this.grdMessages.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdMessages.DisplayLayout.GroupByBox.Hidden = true;
            this.grdMessages.DisplayLayout.MaxColScrollRegions = 1;
            this.grdMessages.DisplayLayout.MaxRowScrollRegions = 1;
            appearance3.BackColor = System.Drawing.Color.LightSlateGray;
            appearance3.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.Color.DimGray;
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.SizeInPoints = 8F;
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.TextHAlignAsString = "Center";
            this.grdMessages.DisplayLayout.Override.ActiveRowAppearance = appearance3;
            this.grdMessages.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinGroup;
            this.grdMessages.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdMessages.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdMessages.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdMessages.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdMessages.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdMessages.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdMessages.DisplayLayout.Override.DefaultColWidth = 50;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance4.Cursor = System.Windows.Forms.Cursors.Hand;
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Tahoma";
            appearance4.FontData.SizeInPoints = 8F;
            appearance4.TextHAlignAsString = "Center";
            this.grdMessages.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.grdMessages.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdMessages.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdMessages.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance5.FontData.SizeInPoints = 8F;
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.grdMessages.DisplayLayout.Override.RowAlternateAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.DimGray;
            appearance6.FontData.SizeInPoints = 8F;
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextHAlignAsString = "Center";
            this.grdMessages.DisplayLayout.Override.RowAppearance = appearance6;
            this.grdMessages.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.ForeColor = System.Drawing.Color.White;
            this.grdMessages.DisplayLayout.Override.SelectedRowAppearance = appearance7;
            this.grdMessages.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdMessages.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdMessages.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdMessages.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMessages.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grdMessages.Location = new System.Drawing.Point(3, 16);
            this.grdMessages.Name = "grdMessages";
            this.grdMessages.Size = new System.Drawing.Size(1008, 327);
            this.grdMessages.TabIndex = 95;
            this.grdMessages.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdMessages.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdMessages_BeforeCustomRowFilterDialog);
            this.grdMessages.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdMessages_BeforeColumnChooserDisplayed);
            this.grdMessages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdMessages_MouseDown);
            // 
            // contextMnuMissing
            // 
            this.contextMnuMissing.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSymbolManuallyToolStripMenuItem,
            this.refreshDataToolStripMenuItem,
            this.saveLayoutToolStripMenuItem,
            this.sendAlertForTradesToolStripMenuItem,
            this.exportDataToolStripMenuItem});
            this.contextMnuMissing.Name = "contextMnuMissing";
            this.contextMnuMissing.Size = new System.Drawing.Size(192, 114);
            this.inboxControlStyler1.SetStyleSettings(this.contextMnuMissing, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // addSymbolManuallyToolStripMenuItem
            // 
            this.addSymbolManuallyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addSymbolManuallyToolStripMenuItem.Image")));
            this.addSymbolManuallyToolStripMenuItem.Name = "addSymbolManuallyToolStripMenuItem";
            this.addSymbolManuallyToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.addSymbolManuallyToolStripMenuItem.Text = "Add Symbol Manually";
            this.addSymbolManuallyToolStripMenuItem.Click += new System.EventHandler(this.addSymbolManuallyToolStripMenuItem_Click);
            // 
            // refreshDataToolStripMenuItem
            // 
            this.refreshDataToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("refreshDataToolStripMenuItem.Image")));
            this.refreshDataToolStripMenuItem.Name = "refreshDataToolStripMenuItem";
            this.refreshDataToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.refreshDataToolStripMenuItem.Text = "Refresh All Data";
            this.refreshDataToolStripMenuItem.Click += new System.EventHandler(this.btnMissingTrades_Click);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveLayoutToolStripMenuItem.Image")));
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // sendAlertForTradesToolStripMenuItem
            // 
            this.sendAlertForTradesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("sendAlertForTradesToolStripMenuItem.Image")));
            this.sendAlertForTradesToolStripMenuItem.Name = "sendAlertForTradesToolStripMenuItem";
            this.sendAlertForTradesToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.sendAlertForTradesToolStripMenuItem.Text = "Send Mail Alert";
            this.sendAlertForTradesToolStripMenuItem.Visible = false;
            this.sendAlertForTradesToolStripMenuItem.Click += new System.EventHandler(this.sendAlertForTradesToolStripMenuItem_Click);
            // 
            // exportDataToolStripMenuItem
            // 
            this.exportDataToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportDataToolStripMenuItem.Image")));
            this.exportDataToolStripMenuItem.Name = "exportDataToolStripMenuItem";
            this.exportDataToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exportDataToolStripMenuItem.Text = "Export Data";
            this.exportDataToolStripMenuItem.Click += new System.EventHandler(this.exportDataToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(4, 411);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1006, 22);
            this.inboxControlStyler1.SetStyleSettings(this.statusStrip1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.statusStrip1.TabIndex = 96;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(263, 17);
            this.toolStripStatusLabel1.Text = "Click on \"Get Missing trades\" to get stuck trades.";
            // 
            // grpBoxMissingTrades
            // 
            this.grpBoxMissingTrades.Controls.Add(this.grdMessages);
            this.grpBoxMissingTrades.Location = new System.Drawing.Point(0, 35);
            this.grpBoxMissingTrades.Name = "grpBoxMissingTrades";
            this.grpBoxMissingTrades.Size = new System.Drawing.Size(1014, 346);
            this.grpBoxMissingTrades.TabIndex = 97;
            this.grpBoxMissingTrades.Text = "Missing Trades";
            // 
            // ultraMessageBoxManager1
            // 
            this.ultraMessageBoxManager1.ContainingControl = this;
            // 
            // btnMissingTrades
            // 
            this.btnMissingTrades.AutoSize = true;
            this.btnMissingTrades.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMissingTrades.Location = new System.Drawing.Point(8, 1);
            this.btnMissingTrades.Name = "btnMissingTrades";
            this.btnMissingTrades.Size = new System.Drawing.Size(118, 28);
            this.btnMissingTrades.TabIndex = 98;
            this.btnMissingTrades.Text = "Get Missing Trades";
            this.btnMissingTrades.Click += new System.EventHandler(this.btnMissingTrades_Click);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // MissingTradesUI_Fill_Panel
            // 
            // 
            // MissingTradesUI_Fill_Panel.ClientArea
            // 
            this.MissingTradesUI_Fill_Panel.ClientArea.Controls.Add(this.btnMissingTrades);
            this.MissingTradesUI_Fill_Panel.ClientArea.Controls.Add(this.grpBoxMissingTrades);
            this.MissingTradesUI_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.MissingTradesUI_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MissingTradesUI_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.MissingTradesUI_Fill_Panel.Name = "MissingTradesUI_Fill_Panel";
            this.MissingTradesUI_Fill_Panel.Size = new System.Drawing.Size(1006, 384);
            this.MissingTradesUI_Fill_Panel.TabIndex = 97;
            // 
            // _MissingTradesUI_UltraFormManager_Dock_Area_Left
            // 
            this._MissingTradesUI_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MissingTradesUI_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._MissingTradesUI_UltraFormManager_Dock_Area_Left.Name = "_MissingTradesUI_UltraFormManager_Dock_Area_Left";
            this._MissingTradesUI_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 406);
            // 
            // _MissingTradesUI_UltraFormManager_Dock_Area_Right
            // 
            this._MissingTradesUI_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MissingTradesUI_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1010, 27);
            this._MissingTradesUI_UltraFormManager_Dock_Area_Right.Name = "_MissingTradesUI_UltraFormManager_Dock_Area_Right";
            this._MissingTradesUI_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 406);
            // 
            // _MissingTradesUI_UltraFormManager_Dock_Area_Top
            // 
            this._MissingTradesUI_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MissingTradesUI_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._MissingTradesUI_UltraFormManager_Dock_Area_Top.Name = "_MissingTradesUI_UltraFormManager_Dock_Area_Top";
            this._MissingTradesUI_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1014, 27);
            // 
            // _MissingTradesUI_UltraFormManager_Dock_Area_Bottom
            // 
            this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 433);
            this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom.Name = "_MissingTradesUI_UltraFormManager_Dock_Area_Bottom";
            this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1014, 4);
            // 
            // MissingTradesUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 437);
            this.Controls.Add(this.MissingTradesUI_Fill_Panel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._MissingTradesUI_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._MissingTradesUI_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._MissingTradesUI_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._MissingTradesUI_UltraFormManager_Dock_Area_Bottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MissingTradesUI";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Missing Trades";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MissingTradesUI_FormClosed);
            this.Load += new System.EventHandler(this.MissingTradesUI_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdMessages)).EndInit();
            this.contextMnuMissing.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpBoxMissingTrades)).EndInit();
            this.grpBoxMissingTrades.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.MissingTradesUI_Fill_Panel.ClientArea.ResumeLayout(false);
            this.MissingTradesUI_Fill_Panel.ClientArea.PerformLayout();
            this.MissingTradesUI_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdMessages;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Infragistics.Win.Misc.UltraGroupBox grpBoxMissingTrades;
        private Infragistics.Win.UltraMessageBox.UltraMessageBoxManager ultraMessageBoxManager1;
        private Infragistics.Win.Misc.UltraButton btnMissingTrades;
        private System.Windows.Forms.ContextMenuStrip contextMnuMissing;
        private System.Windows.Forms.ToolStripMenuItem addSymbolManuallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendAlertForTradesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter grdExclExporter;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel MissingTradesUI_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MissingTradesUI_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MissingTradesUI_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MissingTradesUI_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MissingTradesUI_UltraFormManager_Dock_Area_Bottom;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}