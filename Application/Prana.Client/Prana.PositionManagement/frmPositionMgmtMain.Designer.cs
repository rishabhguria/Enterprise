namespace Prana.PositionManagement
{
    partial class frmPositionMgmtMain
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

                if (_proxy != null)
                {
                    _proxy.InnerChannel.UnSubscribe(Prana.BusinessObjects.Constants.Topics.Topic_Allocation);
                    _proxy.InnerChannel.UnSubscribe(Prana.BusinessObjects.Constants.Topics.Topic_Closing);
                    _proxy.Dispose();
                }
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
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btn_Reprocess = new System.Windows.Forms.Button();
            this.uegbMain = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.grdOpenPositions = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraButtonPreferenecs = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.uegbMain)).BeginInit();
            this.uegbMain.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOpenPositions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(772, 37);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(88, 22);
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btn_Reprocess
            // 
            this.btn_Reprocess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Reprocess.Location = new System.Drawing.Point(869, 37);
            this.btn_Reprocess.Name = "btn_Reprocess";
            this.btn_Reprocess.Size = new System.Drawing.Size(93, 22);
            this.btn_Reprocess.TabIndex = 11;
            this.btn_Reprocess.Text = "ReProcess";
            this.btn_Reprocess.UseVisualStyleBackColor = true;
            // 
            // uegbMain
            // 
            this.uegbMain.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.uegbMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uegbMain.ExpandedSize = new System.Drawing.Size(960, 368);
            this.uegbMain.ForeColor = System.Drawing.SystemColors.ControlText;
            this.uegbMain.Location = new System.Drawing.Point(8, 65);
            this.uegbMain.Name = "uegbMain";
            this.uegbMain.Size = new System.Drawing.Size(960, 368);
            this.uegbMain.TabIndex = 5;
            this.uegbMain.Text = "Open Positions";
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.grdOpenPositions);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 19);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(954, 346);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // grdOpenPositions
            // 
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdOpenPositions.DisplayLayout.Appearance = appearance1;
            this.grdOpenPositions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdOpenPositions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdOpenPositions.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdOpenPositions.DisplayLayout.EmptyRowSettings.Style = Infragistics.Win.UltraWinGrid.EmptyRowStyle.HideRowSelector;
            this.grdOpenPositions.DisplayLayout.MaxColScrollRegions = 1;
            this.grdOpenPositions.DisplayLayout.MaxRowScrollRegions = 1;
            appearance2.BackColor = System.Drawing.Color.LightSlateGray;
            appearance2.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance2.BorderColor = System.Drawing.Color.DimGray;
            appearance2.FontData.BoldAsString = "True";
            appearance2.ForeColor = System.Drawing.Color.White;
            this.grdOpenPositions.DisplayLayout.Override.ActiveRowAppearance = appearance2;
            this.grdOpenPositions.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdOpenPositions.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdOpenPositions.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            this.grdOpenPositions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdOpenPositions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdOpenPositions.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdOpenPositions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            this.grdOpenPositions.DisplayLayout.Override.CellPadding = 0;
            this.grdOpenPositions.DisplayLayout.Override.CellSpacing = 0;
            this.grdOpenPositions.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.EntireColumn;
            appearance3.BorderColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.grdOpenPositions.DisplayLayout.Override.GroupByRowAppearance = appearance3;
            appearance4.FontData.Name = "Tahoma";
            appearance4.FontData.SizeInPoints = 8F;
            appearance4.TextHAlignAsString = "Center";
            this.grdOpenPositions.DisplayLayout.Override.HeaderAppearance = appearance4;
            this.grdOpenPositions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdOpenPositions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.Standard;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance5.ForeColor = System.Drawing.Color.White;
            appearance5.TextHAlignAsString = "Right";
            appearance5.TextVAlignAsString = "Middle";
            this.grdOpenPositions.DisplayLayout.Override.RowAlternateAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.Black;
            appearance6.ForeColor = System.Drawing.Color.White;
            appearance6.TextHAlignAsString = "Right";
            appearance6.TextVAlignAsString = "Middle";
            this.grdOpenPositions.DisplayLayout.Override.RowAppearance = appearance6;
            this.grdOpenPositions.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdOpenPositions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdOpenPositions.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderColor = System.Drawing.Color.Transparent;
            appearance7.FontData.BoldAsString = "True";
            this.grdOpenPositions.DisplayLayout.Override.SelectedRowAppearance = appearance7;
            this.grdOpenPositions.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdOpenPositions.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdOpenPositions.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdOpenPositions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdOpenPositions.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdOpenPositions.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdOpenPositions.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            this.grdOpenPositions.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance8.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdOpenPositions.DisplayLayout.Override.TemplateAddRowAppearance = appearance8;
            this.grdOpenPositions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdOpenPositions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdOpenPositions.DisplayLayout.UseFixedHeaders = true;
            this.grdOpenPositions.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdOpenPositions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdOpenPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOpenPositions.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdOpenPositions.Location = new System.Drawing.Point(0, 0);
            this.grdOpenPositions.Name = "grdOpenPositions";
            this.grdOpenPositions.Size = new System.Drawing.Size(954, 346);
            this.grdOpenPositions.TabIndex = 108;
            this.grdOpenPositions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _frmPositionMgmtMain_UltraFormManager_Dock_Area_Left
            // 
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left.Name = "_frmPositionMgmtMain_UltraFormManager_Dock_Area_Left";
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 402);
            // 
            // _frmPositionMgmtMain_UltraFormManager_Dock_Area_Right
            // 
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(968, 31);
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right.Name = "_frmPositionMgmtMain_UltraFormManager_Dock_Area_Right";
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 402);
            // 
            // _frmPositionMgmtMain_UltraFormManager_Dock_Area_Top
            // 
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Top.Name = "_frmPositionMgmtMain_UltraFormManager_Dock_Area_Top";
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(976, 31);
            // 
            // _frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom
            // 
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 433);
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom.Name = "_frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom";
            this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(976, 8);
            // 
            // ultraButtonPreferenecs
            // 
            this.ultraButtonPreferenecs.Location = new System.Drawing.Point(14, 36);
            this.ultraButtonPreferenecs.Name = "ultraButtonPreferenecs";
            this.ultraButtonPreferenecs.Size = new System.Drawing.Size(119, 23);
            this.ultraButtonPreferenecs.TabIndex = 13;
            this.ultraButtonPreferenecs.Text = "Preferenecs";
            this.ultraButtonPreferenecs.Click += new System.EventHandler(this.ultraButtonPreferenecs_Click);
            // 
            // frmPositionMgmtMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 441);
            this.Controls.Add(this.ultraButtonPreferenecs);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btn_Reprocess);
            this.Controls.Add(this.uegbMain);
            this.Controls.Add(this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom);
            this.Name = "frmPositionMgmtMain";
            this.Text = "Position Management";
            ((System.ComponentModel.ISupportInitialize)(this.uegbMain)).EndInit();
            this.uegbMain.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdOpenPositions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraExpandableGroupBox uegbMain;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdOpenPositions;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btn_Reprocess;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmPositionMgmtMain_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmPositionMgmtMain_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmPositionMgmtMain_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmPositionMgmtMain_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraButton ultraButtonPreferenecs;
    }
}

