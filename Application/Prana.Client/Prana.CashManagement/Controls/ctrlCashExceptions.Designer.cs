namespace Prana.CashManagement.Controls
{
    partial class ctrlCashExceptions
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
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            this.pnluEGboxExceptions = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.grdCashExceptions = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.uEGboxExceptions = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.pnlGetData = new System.Windows.Forms.Panel();
            this.btnOverriding = new Infragistics.Win.Misc.UltraButton();
            this.lblToDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblFromDate = new Infragistics.Win.Misc.UltraLabel();
            this.dtFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnGetEx = new Infragistics.Win.Misc.UltraButton();
            this.pnluEGboxExceptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCashExceptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uEGboxExceptions)).BeginInit();
            this.uEGboxExceptions.SuspendLayout();
            this.pnlGetData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).BeginInit();
            this.SuspendLayout();
            // 
            // pnluEGboxExceptions
            // 
            this.pnluEGboxExceptions.Controls.Add(this.grdCashExceptions);
            this.pnluEGboxExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnluEGboxExceptions.Location = new System.Drawing.Point(3, 19);
            this.pnluEGboxExceptions.Name = "pnluEGboxExceptions";
            this.pnluEGboxExceptions.Size = new System.Drawing.Size(537, 320);
            this.pnluEGboxExceptions.TabIndex = 0;
            // 
            // grdCashExceptions
            // 
            appearance29.BackColor = System.Drawing.Color.Black;
            this.grdCashExceptions.DisplayLayout.Appearance = appearance29;
            this.grdCashExceptions.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdCashExceptions.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashExceptions.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashExceptions.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCashExceptions.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCashExceptions.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.grdCashExceptions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashExceptions.DisplayLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashExceptions.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashExceptions.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashExceptions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
            this.grdCashExceptions.DisplayLayout.Override.CellPadding = 0;
            this.grdCashExceptions.DisplayLayout.Override.CellSpacing = 0;
            appearance32.FontData.Name = "Tahoma";
            appearance32.FontData.SizeInPoints = 8F;
            appearance32.TextHAlignAsString = "Center";
            this.grdCashExceptions.DisplayLayout.Override.HeaderAppearance = appearance32;
            this.grdCashExceptions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdCashExceptions.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdCashExceptions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdCashExceptions.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            appearance12.FontData.BoldAsString = "True";
            this.grdCashExceptions.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.grdCashExceptions.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCashExceptions.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCashExceptions.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdCashExceptions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCashExceptions.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdCashExceptions.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdCashExceptions.DisplayLayout.Override.SupportDataErrorInfo = Infragistics.Win.UltraWinGrid.SupportDataErrorInfo.RowsAndCells;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdCashExceptions.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.grdCashExceptions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCashExceptions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCashExceptions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCashExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCashExceptions.ExitEditModeOnLeave = false;
            this.grdCashExceptions.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdCashExceptions.Location = new System.Drawing.Point(0, 0);
            this.grdCashExceptions.Name = "grdCashExceptions";
            this.grdCashExceptions.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.grdCashExceptions.Size = new System.Drawing.Size(537, 320);
            this.grdCashExceptions.TabIndex = 2;
            this.grdCashExceptions.Text = "Cash Journal";
            this.grdCashExceptions.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnCellChangeOrLostFocus;
            this.grdCashExceptions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCashExceptions.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCashExceptions_InitializeLayout);
            this.grdCashExceptions.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdCashExceptions_InitializeGroupByRow);
            this.grdCashExceptions.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdCashExceptions_InitializeRow);
            this.grdCashExceptions.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.grdCashExceptions_AfterSortChange);
            // 
            // uEGboxExceptions
            // 
            this.uEGboxExceptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.uEGboxExceptions.Controls.Add(this.pnluEGboxExceptions);
            this.uEGboxExceptions.ExpandedSize = new System.Drawing.Size(543, 342);
            this.uEGboxExceptions.Location = new System.Drawing.Point(3, 45);
            this.uEGboxExceptions.Name = "uEGboxExceptions";
            this.uEGboxExceptions.Size = new System.Drawing.Size(543, 342);
            this.uEGboxExceptions.TabIndex = 2;
            this.uEGboxExceptions.Text = "Cash Exceptions";
            // 
            // pnlGetData
            // 
            this.pnlGetData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGetData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGetData.Controls.Add(this.btnOverriding);
            this.pnlGetData.Controls.Add(this.lblToDate);
            this.pnlGetData.Controls.Add(this.dtToDate);
            this.pnlGetData.Controls.Add(this.lblFromDate);
            this.pnlGetData.Controls.Add(this.dtFromDate);
            this.pnlGetData.Controls.Add(this.btnSave);
            this.pnlGetData.Controls.Add(this.btnGetEx);
            this.pnlGetData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pnlGetData.Location = new System.Drawing.Point(6, 4);
            this.pnlGetData.Name = "pnlGetData";
            this.pnlGetData.Size = new System.Drawing.Size(537, 35);
            this.pnlGetData.TabIndex = 1;
            // 
            // btnOverriding
            // 
            this.btnOverriding.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnOverriding.Location = new System.Drawing.Point(350, 4);
            this.btnOverriding.Name = "btnOverriding";
            this.btnOverriding.Size = new System.Drawing.Size(115, 23);
            this.btnOverriding.TabIndex = 10;
            this.btnOverriding.Text = "Get Overriding Data";
            this.btnOverriding.Click += new System.EventHandler(this.btnOverriding_Click);
            // 
            // lblToDate
            // 
            this.lblToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblToDate.Location = new System.Drawing.Point(137, 8);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(18, 15);
            this.lblToDate.TabIndex = 8;
            this.lblToDate.Text = "To";
            // 
            // dtToDate
            // 
            this.dtToDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtToDate.Location = new System.Drawing.Point(163, 4);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(86, 22);
            this.dtToDate.TabIndex = 2;
            // 
            // lblFromDate
            // 
            this.lblFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblFromDate.Location = new System.Drawing.Point(3, 8);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(32, 15);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "From";
            // 
            // dtFromDate
            // 
            this.dtFromDate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtFromDate.Location = new System.Drawing.Point(43, 4);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(86, 22);
            this.dtFromDate.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(469, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGetEx
            // 
            this.btnGetEx.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnGetEx.Location = new System.Drawing.Point(256, 4);
            this.btnGetEx.Name = "btnGetEx";
            this.btnGetEx.Size = new System.Drawing.Size(91, 23);
            this.btnGetEx.TabIndex = 3;
            this.btnGetEx.Text = "Get Exceptions";
            this.btnGetEx.Click += new System.EventHandler(this.btnGetEx_Click);
            // 
            // ctrlCashExceptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.pnlGetData);
            this.Controls.Add(this.uEGboxExceptions);
            this.Name = "ctrlCashExceptions";
            this.Size = new System.Drawing.Size(549, 390);
            this.pnluEGboxExceptions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCashExceptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uEGboxExceptions)).EndInit();
            this.uEGboxExceptions.ResumeLayout(false);
            this.pnlGetData.ResumeLayout(false);
            this.pnlGetData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel pnluEGboxExceptions;
        private Infragistics.Win.Misc.UltraExpandableGroupBox uEGboxExceptions;
        private System.Windows.Forms.Panel pnlGetData;
        private Infragistics.Win.Misc.UltraLabel lblFromDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtFromDate;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnGetEx;
        private Infragistics.Win.Misc.UltraLabel lblToDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtToDate;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCashExceptions;
        private Infragistics.Win.Misc.UltraButton btnOverriding;



    }
}
