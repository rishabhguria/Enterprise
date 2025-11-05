using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using System;
using Prana.Utilities.UI.UIUtilities;
using Infragistics.Win;
using System.Drawing;

namespace Prana.ThirdPartyReport.Controls
{
    partial class ThirdPartyGrid
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
                if (HeaderCheckBoxUnallocated != null)
                {
                    HeaderCheckBoxUnallocated.Dispose();
                }
                if (_dsForXMLFile != null)
                {
                    _dsForXMLFile.Dispose();
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
            Infragistics.Win.UltraWinScrollBar.ScrollBarLook scrollBarLook1 = new Infragistics.Win.UltraWinScrollBar.ScrollBarLook();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            this.grdThirdParty = new PranaUltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.grdThirdParty)).BeginInit();
            this.SuspendLayout();
            // 
            // grdThirdParty
            // 
            this.grdThirdParty.DisplayLayout.GroupByBox.Hidden = true;
            this.grdThirdParty.DisplayLayout.MaxColScrollRegions = 1;
            this.grdThirdParty.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdThirdParty.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.grdThirdParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdThirdParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdThirdParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdThirdParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdThirdParty.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.OncePerGroupedRowIsland;
            this.grdThirdParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdThirdParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this.grdThirdParty.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdThirdParty.DisplayLayout.Override.RowSelectorHeaderStyle = RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdThirdParty.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdThirdParty.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.grdThirdParty.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            scrollBarLook1.Appearance = appearance9;
            this.grdThirdParty.DisplayLayout.ScrollBarLook = scrollBarLook1;
            this.grdThirdParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdThirdParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdThirdParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdThirdParty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdThirdParty.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdThirdParty.Location = new System.Drawing.Point(0, 0);
            this.grdThirdParty.Name = "grdThirdParty";
            this.grdThirdParty.Size = new System.Drawing.Size(1057, 773);
            this.grdThirdParty.TabIndex = 1;
            this.grdThirdParty.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdThirdParty.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdThirdParty_InitializeLayout);
            this.grdThirdParty.Error += new Infragistics.Win.UltraWinGrid.ErrorEventHandler(this.grdThirdParty_Error);
            this.grdThirdParty.AfterRowExpanded += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdThirdParty_AfterRowExpanded);
            this.grdThirdParty.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.grdThirdParty_ClickCell);
            this.grdThirdParty.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdThirdParty_InitializeRow);
            // 
            // ThirdPartyGrid
            // 
            this.Controls.Add(this.grdThirdParty);
            this.Name = "ThirdPartyGrid";
            this.Size = new System.Drawing.Size(1057, 773);
            this.Load += new System.EventHandler(this.ThirdPartyGrid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdThirdParty)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PranaUltraGrid grdThirdParty;

    }
}
