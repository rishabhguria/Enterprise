using System;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.PNL.UI.Controls
{
    partial class MainPnlGrid
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
            try
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }

                _isInitialized = false;
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPnlGrid));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
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
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tradeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionChainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addNewPNLViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandCollapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grdPNL = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPNL)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tradeToolStripMenuItem,
            this.depthToolStripMenuItem,
            this.chartToolStripMenuItem,
            this.optionChainToolStripMenuItem,
            this.toolStripSeparator2,
            this.addNewPNLViewToolStripMenuItem,
            this.expandCollapseToolStripMenuItem,
            this.renameViewToolStripMenuItem,
            this.deleteViewToolStripMenuItem,
            this.saveLayoutToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip2";
            this.contextMenuStrip.Size = new System.Drawing.Size(182, 208);
            // 
            // tradeToolStripMenuItem
            // 
            this.tradeToolStripMenuItem.Image = global::Prana.PNL.UI.Properties.Resources.trade;
            this.tradeToolStripMenuItem.Name = "tradeToolStripMenuItem";
            this.tradeToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.tradeToolStripMenuItem.Text = "Open Trading Ticket";
            this.tradeToolStripMenuItem.Click += new System.EventHandler(this.tradeToolStripMenuItem_Click);
            // 
            // depthToolStripMenuItem
            // 
            this.depthToolStripMenuItem.Image = global::Prana.PNL.UI.Properties.Resources.stage;
            this.depthToolStripMenuItem.Name = "depthToolStripMenuItem";
            this.depthToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.depthToolStripMenuItem.Text = "Depth";
            this.depthToolStripMenuItem.Click += new System.EventHandler(this.depthToolStripMenuItem_Click);
            // 
            // chartToolStripMenuItem
            // 
            this.chartToolStripMenuItem.Image = global::Prana.PNL.UI.Properties.Resources.chart_icon;
            this.chartToolStripMenuItem.Name = "chartToolStripMenuItem";
            this.chartToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.chartToolStripMenuItem.Text = "Chart";
            this.chartToolStripMenuItem.Click += new System.EventHandler(this.chartToolStripMenuItem_Click);
            // 
            // optionChainToolStripMenuItem
            // 
            this.optionChainToolStripMenuItem.Image = global::Prana.PNL.UI.Properties.Resources.chain;
            this.optionChainToolStripMenuItem.Name = "optionChainToolStripMenuItem";
            this.optionChainToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.optionChainToolStripMenuItem.Text = "Option Chain";
            this.optionChainToolStripMenuItem.Click += new System.EventHandler(this.optionChainToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(178, 6);
            // 
            // addNewPNLViewToolStripMenuItem
            // 
            this.addNewPNLViewToolStripMenuItem.Image = global::Prana.PNL.UI.Properties.Resources.AddSign;
            this.addNewPNLViewToolStripMenuItem.Name = "addNewPNLViewToolStripMenuItem";
            this.addNewPNLViewToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.addNewPNLViewToolStripMenuItem.Text = "Add New PNL View";
            this.addNewPNLViewToolStripMenuItem.Click += new System.EventHandler(this.addNewPNLViewToolStripMenuItem_Click);
            // 
            // expandCollapseToolStripMenuItem
            // 
            this.expandCollapseToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("expandCollapseToolStripMenuItem.Image")));
            this.expandCollapseToolStripMenuItem.Name = "expandCollapseToolStripMenuItem";
            this.expandCollapseToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.expandCollapseToolStripMenuItem.Text = "Expand/Collapse";
            this.expandCollapseToolStripMenuItem.Click += new System.EventHandler(this.expandCollapseToolStripMenuItem_Click);
            // 
            // renameViewToolStripMenuItem
            // 
            this.renameViewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("renameViewToolStripMenuItem.Image")));
            this.renameViewToolStripMenuItem.Name = "renameViewToolStripMenuItem";
            this.renameViewToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.renameViewToolStripMenuItem.Text = "Rename View";
            this.renameViewToolStripMenuItem.Click += new System.EventHandler(this.renameViewToolStripMenuItem_Click);
            // 
            // deleteViewToolStripMenuItem
            // 
            this.deleteViewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteViewToolStripMenuItem.Image")));
            this.deleteViewToolStripMenuItem.Name = "deleteViewToolStripMenuItem";
            this.deleteViewToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.deleteViewToolStripMenuItem.Text = "Delete View";
            this.deleteViewToolStripMenuItem.Click += new System.EventHandler(this.deleteViewToolStripMenuItem_Click);
            // 
            // saveLayoutToolStripMenuItem
            // 
            this.saveLayoutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveLayoutToolStripMenuItem.Image")));
            this.saveLayoutToolStripMenuItem.Name = "saveLayoutToolStripMenuItem";
            this.saveLayoutToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.saveLayoutToolStripMenuItem.Text = "Save Layout";
            this.saveLayoutToolStripMenuItem.Click += new System.EventHandler(this.saveLayoutToolStripMenuItem_Click);
            // 
            // grdPNL
            // 
            this.grdPNL.ContextMenuStrip = this.contextMenuStrip;
            appearance1.BackColor = System.Drawing.Color.Black;
            this.grdPNL.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.Indentation = 0;
            this.grdPNL.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdPNL.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdPNL.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            appearance2.BackColor = System.Drawing.Color.Silver;
            appearance2.BackColor2 = System.Drawing.Color.DimGray;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            this.grdPNL.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.LightSteelBlue;
            appearance3.BackColor2 = System.Drawing.Color.SteelBlue;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance3.ForeColor = System.Drawing.Color.White;
            appearance3.ForeColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.grdPNL.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdPNL.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdPNL.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.Black;
            this.grdPNL.DisplayLayout.GroupByBox.ButtonConnectorStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.grdPNL.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdPNL.DisplayLayout.MaxColScrollRegions = 1;
            this.grdPNL.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.Color.SlateGray;
            appearance5.BackColor2 = System.Drawing.Color.DarkSlateGray;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance5.BorderColor = System.Drawing.Color.DimGray;
            appearance5.FontData.BoldAsString = "True";
            appearance5.ForeColor = System.Drawing.Color.White;
            this.grdPNL.DisplayLayout.Override.ActiveRowAppearance = appearance5;
            this.grdPNL.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.SingleSummary;
            this.grdPNL.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdPNL.DisplayLayout.Override.CellPadding = 0;
            this.grdPNL.DisplayLayout.Override.CellSpacing = 0;
            this.grdPNL.DisplayLayout.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.CheckOnDisplay;
            appearance6.BackColor = System.Drawing.Color.Gray;
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.VerticalBump;
            appearance6.BorderColor = System.Drawing.Color.Black;
            appearance6.FontData.BoldAsString = "True";
            appearance6.ForeColor = System.Drawing.Color.White;
            this.grdPNL.DisplayLayout.Override.GroupByRowAppearance = appearance6;
            this.grdPNL.DisplayLayout.Override.GroupByRowDescriptionMask = "[value]";
            this.grdPNL.DisplayLayout.Override.GroupByRowInitialExpansionState = Infragistics.Win.UltraWinGrid.GroupByRowInitialExpansionState.Collapsed;
            this.grdPNL.DisplayLayout.Override.GroupByRowPadding = 0;
            this.grdPNL.DisplayLayout.Override.GroupByRowSpacingAfter = 0;
            this.grdPNL.DisplayLayout.Override.GroupByRowSpacingBefore = 0;
            this.grdPNL.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.SummaryCells;
            appearance7.BorderColor = System.Drawing.Color.Transparent;
            appearance7.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance7.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.grdPNL.DisplayLayout.Override.GroupBySummaryValueAppearance = appearance7;
            appearance8.FontData.SizeInPoints = 8F;
            appearance8.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance8.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.grdPNL.DisplayLayout.Override.HeaderAppearance = appearance8;
            this.grdPNL.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle;
            this.grdPNL.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdPNL.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            appearance9.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance9.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.grdPNL.DisplayLayout.Override.RowAlternateAppearance = appearance9;
            appearance10.BackColor = System.Drawing.Color.Black;
            appearance10.TextHAlign = Infragistics.Win.HAlign.Center;
            appearance10.TextVAlign = Infragistics.Win.VAlign.Middle;
            this.grdPNL.DisplayLayout.Override.RowAppearance = appearance10;
            this.grdPNL.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this.grdPNL.DisplayLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.XPThemed;
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.BorderColor = System.Drawing.Color.Transparent;
            appearance11.FontData.BoldAsString = "True";
            this.grdPNL.DisplayLayout.Override.SelectedRowAppearance = appearance11;
            this.grdPNL.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdPNL.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdPNL.DisplayLayout.Override.SelectTypeGroupByRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdPNL.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdPNL.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.None;
            this.grdPNL.DisplayLayout.Override.SpecialRowSeparatorHeight = 0;
            this.grdPNL.DisplayLayout.Override.SummaryDisplayArea = Infragistics.Win.UltraWinGrid.SummaryDisplayAreas.InGroupByRows;
            appearance12.BorderColor = System.Drawing.Color.Transparent;
            this.grdPNL.DisplayLayout.Override.SummaryFooterAppearance = appearance12;
            this.grdPNL.DisplayLayout.Override.SummaryFooterSpacingAfter = 0;
            this.grdPNL.DisplayLayout.Override.SummaryFooterSpacingBefore = 0;
            appearance13.BackColor = System.Drawing.Color.Transparent;
            appearance13.BorderColor = System.Drawing.Color.Transparent;
            appearance13.FontData.BoldAsString = "False";
            appearance13.ForeColor = System.Drawing.Color.White;
            this.grdPNL.DisplayLayout.Override.SummaryValueAppearance = appearance13;
            this.grdPNL.DisplayLayout.PriorityScrolling = true;
            this.grdPNL.DisplayLayout.RowConnectorColor = System.Drawing.Color.Transparent;
            this.grdPNL.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdPNL.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdPNL.DisplayLayout.SummaryButtonImage = global::Prana.PNL.UI.Properties.Resources.pixcel;
            this.grdPNL.DisplayLayout.UseFixedHeaders = true;
            this.grdPNL.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdPNL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPNL.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdPNL.Location = new System.Drawing.Point(0, 0);
            this.grdPNL.Name = "grdPNL";
            this.grdPNL.Size = new System.Drawing.Size(946, 354);
            this.grdPNL.TabIndex = 53;
            this.grdPNL.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdPNL_InitializeRow);
            this.grdPNL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdPNL_MouseDown);
            this.grdPNL.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdPNL_InitializeLayout);
            this.grdPNL.InitializeGroupByRow += new Infragistics.Win.UltraWinGrid.InitializeGroupByRowEventHandler(this.grdPNL_InitializeGroupByRow);
            // 
            // MainPnlGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.grdPNL);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MainPnlGrid";
            this.Size = new System.Drawing.Size(946, 354);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdPNL_MouseUp);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPNL)).EndInit();
            this.ResumeLayout(false);

        }

        

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tradeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionChainToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem saveLayoutToolStripMenuItem;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdPNL;
        private System.Windows.Forms.ToolStripMenuItem addNewPNLViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandCollapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameViewToolStripMenuItem;
    }
}
