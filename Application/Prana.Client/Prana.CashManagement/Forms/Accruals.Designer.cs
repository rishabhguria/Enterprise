namespace Prana.CashManagement
{
	partial class Accruals
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
            if (currencyValList != null)
            {
                currencyValList.Dispose();
                currencyValList = null;
            }
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
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            this.grdAccruals = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.menuStripAccruals = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGet = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.lblTo = new Infragistics.Win.Misc.UltraLabel();
            this.lblFrom = new Infragistics.Win.Misc.UltraLabel();
            this.dtTo = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dtFrom = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.btnExport = new Infragistics.Win.Misc.UltraButton();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.Accruals_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._Accruals_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._Accruals_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._Accruals_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._Accruals_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccruals)).BeginInit();
            this.menuStripAccruals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.Accruals_Fill_Panel.ClientArea.SuspendLayout();
            this.Accruals_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // grdAccruals
            // 
            this.grdAccruals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAccruals.ContextMenuStrip = this.menuStripAccruals;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdAccruals.DisplayLayout.Appearance = appearance1;
            this.grdAccruals.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdAccruals.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccruals.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAccruals.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.LightSlateGray;
            appearance2.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance2.BorderColor = System.Drawing.Color.DimGray;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.grdAccruals.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdAccruals.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdAccruals.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccruals.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccruals.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccruals.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdAccruals.DisplayLayout.Override.CellPadding = 0;
            this.grdAccruals.DisplayLayout.Override.CellSpacing = 0;
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.grdAccruals.DisplayLayout.Override.GroupByRowAppearance = appearance3;
            appearance4.FontData.Name = "Segoe UI";
            appearance4.FontData.SizeInPoints = 9F;
            appearance4.TextHAlignAsString = "Center";
            this.grdAccruals.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.grdAccruals.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdAccruals.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Middle";
            this.grdAccruals.DisplayLayout.Override.RowAlternateAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.Black;
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextHAlignAsString = "Right";
            appearance6.TextVAlignAsString = "Middle";
            this.grdAccruals.DisplayLayout.Override.RowAppearance = appearance6;
            this.grdAccruals.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdAccruals.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Transparent;
            appearance7.FontData.BoldAsString = "True";
            this.grdAccruals.DisplayLayout.Override.SelectedRowAppearance = appearance7;
            this.grdAccruals.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccruals.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccruals.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdAccruals.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAccruals.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdAccruals.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdAccruals.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            this.grdAccruals.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance8.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdAccruals.DisplayLayout.Override.TemplateAddRowAppearance = appearance8;
            this.grdAccruals.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAccruals.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAccruals.DisplayLayout.UseFixedHeaders = true;
            this.grdAccruals.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdAccruals.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAccruals.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grdAccruals.Location = new System.Drawing.Point(0, 47);
            this.grdAccruals.Margin = new System.Windows.Forms.Padding(4);
            this.grdAccruals.Name = "grdAccruals";
            this.grdAccruals.Size = new System.Drawing.Size(570, 500);
            this.grdAccruals.TabIndex = 1;
            this.grdAccruals.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdAccruals.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdAccruals_InitializeRow);
            this.grdAccruals.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdAccruals_CellChange);
            // 
            // menuStripAccruals
            // 
            this.menuStripAccruals.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRowToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.menuStripAccruals.Name = "menuStripCashValue";
            this.menuStripAccruals.Size = new System.Drawing.Size(108, 48);
            this.inboxControlStyler1.SetStyleSettings(this.menuStripAccruals, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            // 
            // addRowToolStripMenuItem
            // 
            this.addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            this.addRowToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.addRowToolStripMenuItem.Text = "Add";
            this.addRowToolStripMenuItem.Click += new System.EventHandler(this.addRowToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // btnGet
            // 
            this.btnGet.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGet.Location = new System.Drawing.Point(304, 13);
            this.btnGet.Margin = new System.Windows.Forms.Padding(4);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(80, 23);
            this.btnGet.TabIndex = 2;
            this.btnGet.Text = "Get";
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(392, 13);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTo
            // 
            appearance9.TextHAlignAsString = "Left";
            appearance9.TextVAlignAsString = "Middle";
            this.lblTo.Appearance = appearance9;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(161, 13);
            this.lblTo.Margin = new System.Windows.Forms.Padding(4);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(18, 23);
            this.lblTo.TabIndex = 100;
            this.lblTo.Text = "to";
            // 
            // lblFrom
            // 
            appearance10.TextHAlignAsString = "Left";
            appearance10.TextVAlignAsString = "Middle";
            this.lblFrom.Appearance = appearance10;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(5, 13);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(4);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(34, 23);
            this.lblFrom.TabIndex = 99;
            this.lblFrom.Text = "From";
            // 
            // dtTo
            // 
            this.dtTo.AutoSize = false;
            this.dtTo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtTo.Location = new System.Drawing.Point(187, 13);
            this.dtTo.Margin = new System.Windows.Forms.Padding(4);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(109, 23);
            this.dtTo.TabIndex = 98;
            // 
            // dtFrom
            // 
            this.dtFrom.AutoSize = false;
            this.dtFrom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFrom.Location = new System.Drawing.Point(44, 13);
            this.dtFrom.Margin = new System.Windows.Forms.Padding(4);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(109, 23);
            this.dtFrom.TabIndex = 97;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Location = new System.Drawing.Point(480, 13);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // Accruals_Fill_Panel
            // 
            // 
            // Accruals_Fill_Panel.ClientArea
            // 
            this.Accruals_Fill_Panel.ClientArea.Controls.Add(this.dtTo);
            this.Accruals_Fill_Panel.ClientArea.Controls.Add(this.lblTo);
            this.Accruals_Fill_Panel.ClientArea.Controls.Add(this.lblFrom);
            this.Accruals_Fill_Panel.ClientArea.Controls.Add(this.dtFrom);
            this.Accruals_Fill_Panel.ClientArea.Controls.Add(this.btnSave);
            this.Accruals_Fill_Panel.ClientArea.Controls.Add(this.btnGet);
            this.Accruals_Fill_Panel.ClientArea.Controls.Add(this.btnExport);
            this.Accruals_Fill_Panel.ClientArea.Controls.Add(this.grdAccruals);
            this.Accruals_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.Accruals_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Accruals_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.Accruals_Fill_Panel.Margin = new System.Windows.Forms.Padding(4);
            this.Accruals_Fill_Panel.Name = "Accruals_Fill_Panel";
            this.Accruals_Fill_Panel.Size = new System.Drawing.Size(570, 547);
            this.Accruals_Fill_Panel.TabIndex = 1;
            // 
            // _Accruals_UltraFormManager_Dock_Area_Left
            // 
            this._Accruals_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Accruals_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Accruals_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._Accruals_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Accruals_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._Accruals_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._Accruals_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._Accruals_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(4);
            this._Accruals_UltraFormManager_Dock_Area_Left.Name = "_Accruals_UltraFormManager_Dock_Area_Left";
            this._Accruals_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 547);
            // 
            // _Accruals_UltraFormManager_Dock_Area_Right
            // 
            this._Accruals_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Accruals_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Accruals_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._Accruals_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Accruals_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._Accruals_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._Accruals_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(574, 27);
            this._Accruals_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(4);
            this._Accruals_UltraFormManager_Dock_Area_Right.Name = "_Accruals_UltraFormManager_Dock_Area_Right";
            this._Accruals_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 547);
            // 
            // _Accruals_UltraFormManager_Dock_Area_Top
            // 
            this._Accruals_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Accruals_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Accruals_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._Accruals_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Accruals_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._Accruals_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._Accruals_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(4);
            this._Accruals_UltraFormManager_Dock_Area_Top.Name = "_Accruals_UltraFormManager_Dock_Area_Top";
            this._Accruals_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(578, 27);
            // 
            // _Accruals_UltraFormManager_Dock_Area_Bottom
            // 
            this._Accruals_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Accruals_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Accruals_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._Accruals_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Accruals_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._Accruals_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._Accruals_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 574);
            this._Accruals_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(4);
            this._Accruals_UltraFormManager_Dock_Area_Bottom.Name = "_Accruals_UltraFormManager_Dock_Area_Bottom";
            this._Accruals_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(578, 4);
            // 
            // Accruals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 578);
            this.Controls.Add(this.Accruals_Fill_Panel);
            this.Controls.Add(this._Accruals_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._Accruals_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._Accruals_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._Accruals_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Accruals";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Accruals";
            this.Load += new System.EventHandler(this.Accruals_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdAccruals)).EndInit();
            this.menuStripAccruals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.Accruals_Fill_Panel.ClientArea.ResumeLayout(false);
            this.Accruals_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdAccruals;
        private Infragistics.Win.Misc.UltraButton btnGet;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraLabel lblTo;
        private Infragistics.Win.Misc.UltraLabel lblFrom;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtTo;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFrom;
        private System.Windows.Forms.ContextMenuStrip menuStripAccruals;
        private System.Windows.Forms.ToolStripMenuItem addRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private Infragistics.Win.Misc.UltraButton btnExport;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel Accruals_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Accruals_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Accruals_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Accruals_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _Accruals_UltraFormManager_Dock_Area_Bottom;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
	}
}