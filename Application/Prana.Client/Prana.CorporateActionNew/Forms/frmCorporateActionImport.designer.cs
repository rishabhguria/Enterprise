namespace Prana.CorporateActionNew.Forms
{
    partial class frmCorporateActionImport
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
                if (headerCheckBox != null)
                {
                    headerCheckBox.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
                if (_selectedRowsTable != null)
                {
                    _selectedRowsTable.Dispose();
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
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            this.grdData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.txtbxSourceFile = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtbxXSLTFile = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.btnSourceFile = new Infragistics.Win.Misc.UltraButton();
            this.btnXSLTFile = new Infragistics.Win.Misc.UltraButton();
            this.btnView = new Infragistics.Win.Misc.UltraButton();
            this.btnImport = new Infragistics.Win.Misc.UltraButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.frmCorporateActionImport_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            //this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtbxSourceFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtbxXSLTFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.frmCorporateActionImport_Fill_Panel.ClientArea.SuspendLayout();
            this.frmCorporateActionImport_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdData
            // 
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.BackColor2 = System.Drawing.Color.Black;
            appearance1.BorderColor = System.Drawing.Color.Black;
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "Tahoma";
            appearance1.FontData.SizeInPoints = 8.25F;
            this.grdData.DisplayLayout.Appearance = appearance1;
            this.grdData.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdData.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdData.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdData.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdData.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdData.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdData.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdData.DisplayLayout.MaxColScrollRegions = 1;
            this.grdData.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdData.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.LightSlateGray;
            appearance6.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance6.BorderColor = System.Drawing.Color.DimGray;
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            this.grdData.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdData.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdData.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdData.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdData.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdData.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdData.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdData.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdData.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.FontData.Name = "Tahoma";
            appearance10.FontData.SizeInPoints = 8F;
            appearance10.TextHAlignAsString = "Center";
            this.grdData.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdData.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.DimGray;
            appearance11.ForeColor = System.Drawing.Color.White;
            appearance11.TextHAlignAsString = "Right";
            appearance11.TextVAlignAsString = "Middle";
            this.grdData.DisplayLayout.Override.RowAlternateAppearance = appearance11;
            appearance12.BackColor = System.Drawing.Color.Black;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            appearance12.ForeColor = System.Drawing.Color.White;
            appearance12.TextHAlignAsString = "Right";
            appearance12.TextVAlignAsString = "Middle";
            this.grdData.DisplayLayout.Override.RowAppearance = appearance12;
            this.grdData.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance13.BackColor = System.Drawing.Color.Transparent;
            appearance13.BorderColor = System.Drawing.Color.Transparent;
            appearance13.FontData.BoldAsString = "True";
            this.grdData.DisplayLayout.Override.SelectedRowAppearance = appearance13;
            this.grdData.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdData.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdData.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdData.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdData.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdData.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdData.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance14.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdData.DisplayLayout.Override.TemplateAddRowAppearance = appearance14;
            this.grdData.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdData.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdData.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdData.Location = new System.Drawing.Point(0, 37);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(715, 308);
            this.grdData.TabIndex = 0;
            this.grdData.Text = "ultraGrid1";
            this.grdData.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.grdData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdData_InitializeLayout);
            this.grdData.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdData_CellChange);
            // 
            // txtbxSourceFile
            // 
            this.txtbxSourceFile.Location = new System.Drawing.Point(235, 10);
            this.txtbxSourceFile.Name = "txtbxSourceFile";
            this.txtbxSourceFile.Size = new System.Drawing.Size(136, 21);
            this.txtbxSourceFile.TabIndex = 1;
            this.txtbxSourceFile.TextChanged += new System.EventHandler(this.txtbxSourceFile_TextChanged);
            // 
            // txtbxXSLTFile
            // 
            this.txtbxXSLTFile.Location = new System.Drawing.Point(12, 10);
            this.txtbxXSLTFile.Name = "txtbxXSLTFile";
            this.txtbxXSLTFile.Size = new System.Drawing.Size(136, 21);
            this.txtbxXSLTFile.TabIndex = 2;
            this.txtbxXSLTFile.TextChanged += new System.EventHandler(this.txtbxXSLTFile_TextChanged);
            // 
            // btnSourceFile
            // 
            this.btnSourceFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSourceFile.Location = new System.Drawing.Point(377, 8);
            this.btnSourceFile.Name = "btnSourceFile";
            this.btnSourceFile.Size = new System.Drawing.Size(75, 23);
            this.btnSourceFile.TabIndex = 3;
            this.btnSourceFile.Text = "Source File";
            this.btnSourceFile.Click += new System.EventHandler(this.btnSourceFile_Click);
            // 
            // btnXSLTFile
            // 
            this.btnXSLTFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXSLTFile.Location = new System.Drawing.Point(154, 8);
            this.btnXSLTFile.Name = "btnXSLTFile";
            this.btnXSLTFile.Size = new System.Drawing.Size(75, 23);
            this.btnXSLTFile.TabIndex = 4;
            this.btnXSLTFile.Text = "XSLT File";
            this.btnXSLTFile.Click += new System.EventHandler(this.btnXSLTFile_Click);
            // 
            // btnView
            // 
            this.btnView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnView.Location = new System.Drawing.Point(471, 8);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "View";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnImport
            // 
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnImport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(602, 8);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(89, 23);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Import Now";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // frmCorporateActionImport_Fill_Panel
            // 
            // 
            // frmCorporateActionImport_Fill_Panel.ClientArea
            // 
            this.frmCorporateActionImport_Fill_Panel.ClientArea.Controls.Add(this.btnImport);
            this.frmCorporateActionImport_Fill_Panel.ClientArea.Controls.Add(this.btnView);
            this.frmCorporateActionImport_Fill_Panel.ClientArea.Controls.Add(this.btnXSLTFile);
            this.frmCorporateActionImport_Fill_Panel.ClientArea.Controls.Add(this.btnSourceFile);
            this.frmCorporateActionImport_Fill_Panel.ClientArea.Controls.Add(this.txtbxXSLTFile);
            this.frmCorporateActionImport_Fill_Panel.ClientArea.Controls.Add(this.txtbxSourceFile);
            this.frmCorporateActionImport_Fill_Panel.ClientArea.Controls.Add(this.grdData);
            this.frmCorporateActionImport_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.frmCorporateActionImport_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frmCorporateActionImport_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.frmCorporateActionImport_Fill_Panel.Name = "frmCorporateActionImport_Fill_Panel";
            this.frmCorporateActionImport_Fill_Panel.Size = new System.Drawing.Size(715, 345);
            this.frmCorporateActionImport_Fill_Panel.TabIndex = 0;
            // 
            // _frmCorporateActionImport_UltraFormManager_Dock_Area_Left
            // 
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left.Name = "_frmCorporateActionImport_UltraFormManager_Dock_Area_Left";
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 345);
            // 
            // _frmCorporateActionImport_UltraFormManager_Dock_Area_Right
            // 
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(719, 27);
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right.Name = "_frmCorporateActionImport_UltraFormManager_Dock_Area_Right";
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 345);
            // 
            // _frmCorporateActionImport_UltraFormManager_Dock_Area_Top
            // 
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Top.Name = "_frmCorporateActionImport_UltraFormManager_Dock_Area_Top";
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(723, 27);
            // 
            // _frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom
            // 
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 372);
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom.Name = "_frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom";
            this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(723, 4);
            // 
            // frmCorporateActionImport
            // 
            this.AcceptButton = this.btnImport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 376);
            this.Controls.Add(this.frmCorporateActionImport_Fill_Panel);
            this.Controls.Add(this._frmCorporateActionImport_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._frmCorporateActionImport_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._frmCorporateActionImport_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom);
            this.Name = "frmCorporateActionImport";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Corporate Action Import";
            this.Load += new System.EventHandler(this.frmCorporateActionImport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtbxSourceFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtbxXSLTFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.frmCorporateActionImport_Fill_Panel.ClientArea.ResumeLayout(false);
            this.frmCorporateActionImport_Fill_Panel.ClientArea.PerformLayout();
            this.frmCorporateActionImport_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdData;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtbxSourceFile;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtbxXSLTFile;
        private Infragistics.Win.Misc.UltraButton btnSourceFile;
        private Infragistics.Win.Misc.UltraButton btnXSLTFile;
        private Infragistics.Win.Misc.UltraButton btnView;
        private Infragistics.Win.Misc.UltraButton btnImport;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel frmCorporateActionImport_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCorporateActionImport_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCorporateActionImport_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCorporateActionImport_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _frmCorporateActionImport_UltraFormManager_Dock_Area_Bottom;
        //private Utilities.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}