using Prana.Utilities.UI.UIUtilities;

namespace Prana.PM.Common
{
    partial class CtrlValidAUEC
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
                _dictAuec.Clear();
                _dictAuec = null;
            }
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.btnGetData = new Infragistics.Win.Misc.UltraButton();
            this.btnSkip = new Infragistics.Win.Misc.UltraButton();
            this.txtSearch = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblSearch = new Infragistics.Win.Misc.UltraLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.grdAuec = new PranaUltraGrid();
           // this.ctrlImageListButtons1 = new Prana.Utilities.UI.UIUtilities.CtrlImageListButtons(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAuec)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(235, 12);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(75, 23);
            this.btnGetData.TabIndex = 8;
            this.btnGetData.Text = "Get Data";
            this.toolTip1.SetToolTip(this.btnGetData, "Reload All Auecs");
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSkip.Location = new System.Drawing.Point(328, 13);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(75, 23);
            this.btnSkip.TabIndex = 9;
            this.btnSkip.Text = "Skip";
            this.toolTip1.SetToolTip(this.btnSkip, "Skip this Step and add AUEC manually");
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(105, 15);
            this.txtSearch.MaxLength = 30;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(102, 21);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(37, 18);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(40, 14);
            this.lblSearch.TabIndex = 7;
            this.lblSearch.Text = "Search";
            // 
            // grdAuec
            // 
            this.grdAuec.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdAuec.DisplayLayout.Appearance = appearance1;
            this.grdAuec.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            appearance2.FontData.BoldAsString = "True";
            ultraGridBand1.Header.Appearance = appearance2;
            this.grdAuec.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            appearance3.BackColor = System.Drawing.Color.SlateGray;
            this.grdAuec.DisplayLayout.Override.ActiveRowAppearance = appearance3;
            this.grdAuec.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            appearance4.BackColor = System.Drawing.Color.Black;
            this.grdAuec.DisplayLayout.Override.FilterRowAppearance = appearance4;
            this.grdAuec.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.grdAuec.DisplayLayout.Override.HeaderAppearance = appearance5;
            this.grdAuec.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            appearance6.BackColor = System.Drawing.Color.Black;
            appearance6.ForeColor = System.Drawing.Color.White;
            this.grdAuec.DisplayLayout.Override.RowAppearance = appearance6;
            this.grdAuec.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAuec.Location = new System.Drawing.Point(3, 41);
            this.grdAuec.Name = "grdAuec";
            this.grdAuec.Size = new System.Drawing.Size(753, 387);
            this.grdAuec.TabIndex = 10;
            this.toolTip1.SetToolTip(this.grdAuec, "Search Auec");
            this.grdAuec.BeforeCustomRowFilterDialog += new Infragistics.Win.UltraWinGrid.BeforeCustomRowFilterDialogEventHandler(this.grdAuec_BeforeCustomRowFilterDialog);
            this.grdAuec.BeforeColumnChooserDisplayed += new Infragistics.Win.UltraWinGrid.BeforeColumnChooserDisplayedEventHandler(this.grdAuec_BeforeColumnChooserDisplayed);
            this.grdAuec.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdAuec_KeyDown);
            this.grdAuec.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdAuec_MouseClick);
            // 
            // CtrlValidAUEC
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.Controls.Add(this.grdAuec);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            this.Name = "CtrlValidAUEC";
            this.Size = new System.Drawing.Size(759, 431);
            this.Load += new System.EventHandler(this.CtrlValidAUEC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAuec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnGetData;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor  txtSearch;
        private Infragistics.Win.Misc.UltraLabel lblSearch;
        private System.Windows.Forms.ToolTip toolTip1;
        protected PranaUltraGrid grdAuec;
        protected Infragistics.Win.Misc.UltraButton btnSkip;
        //private Utilities.UI.UIUtilities.CtrlImageListButtons ctrlImageListButtons1;
    }
}
