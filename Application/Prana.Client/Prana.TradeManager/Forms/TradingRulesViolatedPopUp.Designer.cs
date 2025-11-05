using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Infragistics.Win;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;

namespace Prana.TradeManager.Forms
{
    partial class TradingRulesViolatedPopUp
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TradingRulesViolatedPopUp));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.ultraPanelTradingRulesViolatedFill = new Infragistics.Win.Misc.UltraPanel();
            this.ultraExpandableGroupBoxSharesOutstanding = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanelSharesOutStanding = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraGridSharesOutstandingRuleViolated = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraPanelPaddingSharesOutstanding = new Infragistics.Win.Misc.UltraPanel();
            this.ultraExpandableGroupBoxFatFinger = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanelFatFinger = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraGridFatFingerRuleViolated = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraPanelPaddingFatFinger = new Infragistics.Win.Misc.UltraPanel();
            this.ultraExpandableGroupBoxOverBuyOverSell = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanelOverBuyOverSell = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraGridOverBuyOverSellRuleViolated = new PranaUltraGrid();
            this.ultraPanelPaddingOverBuyOverSell = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabelInformation = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButtonNo = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonYes = new Infragistics.Win.Misc.UltraButton();
            this.ultraPanelTradingRulesViolatedBottom = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraPanelTradingRulesViolatedTop = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanelTradingRulesViolatedFill.ClientArea.SuspendLayout();
            this.ultraPanelTradingRulesViolatedFill.SuspendLayout();
            this.Load += new System.EventHandler(this.TradingRulesViolatedPopUp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBoxSharesOutstanding)).BeginInit();
            this.ultraExpandableGroupBoxSharesOutstanding.SuspendLayout();
            this.ultraExpandableGroupBoxPanelSharesOutStanding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridSharesOutstandingRuleViolated)).BeginInit();
            this.ultraPanelPaddingSharesOutstanding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBoxFatFinger)).BeginInit();
            this.ultraExpandableGroupBoxFatFinger.SuspendLayout();
            this.ultraExpandableGroupBoxPanelFatFinger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridFatFingerRuleViolated)).BeginInit();
            this.ultraPanelPaddingFatFinger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBoxOverBuyOverSell)).BeginInit();
            this.ultraExpandableGroupBoxOverBuyOverSell.SuspendLayout();
            this.ultraExpandableGroupBoxPanelOverBuyOverSell.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridOverBuyOverSellRuleViolated)).BeginInit();
            this.ultraPanelPaddingOverBuyOverSell.SuspendLayout();
            this.ultraPanelTradingRulesViolatedBottom.ClientArea.SuspendLayout();
            this.ultraPanelTradingRulesViolatedBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.ultraPanelTradingRulesViolatedTop.ClientArea.SuspendLayout();
            this.ultraPanelTradingRulesViolatedTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraPanelTradingRulesViolatedFill
            // 
            this.ultraPanelTradingRulesViolatedFill.AutoSize = true;
            this.ultraPanelTradingRulesViolatedFill.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // 
            // ultraPanelTradingRulesViolatedFill.ClientArea
            // 
            this.ultraPanelTradingRulesViolatedFill.ClientArea.Controls.Add(this.ultraExpandableGroupBoxSharesOutstanding);
            this.ultraPanelTradingRulesViolatedFill.ClientArea.Controls.Add(this.ultraExpandableGroupBoxFatFinger);
            this.ultraPanelTradingRulesViolatedFill.ClientArea.Controls.Add(this.ultraExpandableGroupBoxOverBuyOverSell);
            this.ultraPanelTradingRulesViolatedFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanelTradingRulesViolatedFill.Location = new System.Drawing.Point(9, 88);
            this.ultraPanelTradingRulesViolatedFill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraPanelTradingRulesViolatedFill.MinimumSize = new System.Drawing.Size(979, 0);
            this.ultraPanelTradingRulesViolatedFill.Name = "ultraPanelTradingRulesViolatedFill";
            this.ultraPanelTradingRulesViolatedFill.Size = new System.Drawing.Size(1097, 49);
            this.ultraPanelTradingRulesViolatedFill.TabIndex = 0;
            // 
            // ultraExpandableGroupBoxSharesOutstanding
            // 
            this.ultraExpandableGroupBoxSharesOutstanding.Controls.Add(this.ultraExpandableGroupBoxPanelSharesOutStanding);
            this.ultraExpandableGroupBoxSharesOutstanding.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ultraExpandableGroupBoxSharesOutstanding.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGroupBoxSharesOutstanding.Expanded = false;
            this.ultraExpandableGroupBoxSharesOutstanding.ExpandedSize = new System.Drawing.Size(1097, 150);
            this.ultraExpandableGroupBoxSharesOutstanding.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.Near;
            this.ultraExpandableGroupBoxSharesOutstanding.ExpansionIndicatorCollapsed = ((System.Drawing.Image)(resources.GetObject("ultraExpandableGroupBoxSharesOutstanding.ExpansionIndicatorCollapsed")));
            this.ultraExpandableGroupBoxSharesOutstanding.ExpansionIndicatorExpanded = ((System.Drawing.Image)(resources.GetObject("ultraExpandableGroupBoxSharesOutstanding.ExpansionIndicatorExpanded")));
            this.ultraExpandableGroupBoxSharesOutstanding.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance1.BackColor = System.Drawing.Color.DimGray;
            appearance1.FontData.BoldAsString = "False";
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.ForeColorDisabled = System.Drawing.Color.White;
            this.ultraExpandableGroupBoxSharesOutstanding.HeaderAppearance = appearance1;
            this.ultraExpandableGroupBoxSharesOutstanding.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraExpandableGroupBoxSharesOutstanding.Location = new System.Drawing.Point(0, 344);
            this.ultraExpandableGroupBoxSharesOutstanding.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraExpandableGroupBoxSharesOutstanding.Name = "ultraExpandableGroupBoxSharesOutstanding";
            this.ultraExpandableGroupBoxSharesOutstanding.Size = new System.Drawing.Size(1097, 150);
            this.ultraExpandableGroupBoxSharesOutstanding.TabIndex = 2;
            this.ultraExpandableGroupBoxSharesOutstanding.Text = "SharesOutstanding Rule";
            this.ultraExpandableGroupBoxSharesOutstanding.UseAppStyling = false;
            this.ultraExpandableGroupBoxSharesOutstanding.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraExpandableGroupBoxPanelSharesOutStanding
            // 
            this.ultraExpandableGroupBoxPanelSharesOutStanding.Controls.Add(this.ultraGridSharesOutstandingRuleViolated);
            this.ultraExpandableGroupBoxPanelSharesOutStanding.Controls.Add(this.ultraPanelPaddingSharesOutstanding);
            this.ultraExpandableGroupBoxPanelSharesOutStanding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanelSharesOutStanding.Location = new System.Drawing.Point(3, 23);
            this.ultraExpandableGroupBoxPanelSharesOutStanding.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraExpandableGroupBoxPanelSharesOutStanding.Name = "ultraExpandableGroupBoxPanelSharesOutStanding";
            this.ultraExpandableGroupBoxPanelSharesOutStanding.Size = new System.Drawing.Size(1091, 124);
            this.ultraExpandableGroupBoxPanelSharesOutStanding.TabIndex = 0;
            // 
            // ultraGridSharesOutstandingRuleViolated
            // 
            this.ultraGridSharesOutstandingRuleViolated.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGridSharesOutstandingRuleViolated.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridSharesOutstandingRuleViolated.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGridSharesOutstandingRuleViolated.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridSharesOutstandingRuleViolated.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridSharesOutstandingRuleViolated.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridSharesOutstandingRuleViolated.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridSharesOutstandingRuleViolated.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridSharesOutstandingRuleViolated.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridSharesOutstandingRuleViolated.DisplayLayout.UseFixedHeaders = true;
            this.ultraGridSharesOutstandingRuleViolated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridSharesOutstandingRuleViolated.Location = new System.Drawing.Point(0, 0);
            this.ultraGridSharesOutstandingRuleViolated.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraGridSharesOutstandingRuleViolated.Name = "ultraGridSharesOutstandingRuleViolated";
            this.ultraGridSharesOutstandingRuleViolated.Size = new System.Drawing.Size(1091, 114);
            this.ultraGridSharesOutstandingRuleViolated.TabIndex = 1;
            this.ultraGridSharesOutstandingRuleViolated.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridSharesOutstandingRuleViolated_InitializeLayout);
            // 
            // ultraPanelPaddingSharesOutstanding
            // 
            this.ultraPanelPaddingSharesOutstanding.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanelPaddingSharesOutstanding.Location = new System.Drawing.Point(0, 114);
            this.ultraPanelPaddingSharesOutstanding.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ultraPanelPaddingSharesOutstanding.Name = "ultraPanelPaddingSharesOutstanding";
            this.ultraPanelPaddingSharesOutstanding.Size = new System.Drawing.Size(1091, 10);
            this.ultraPanelPaddingSharesOutstanding.TabIndex = 2;
            // 
            // ultraExpandableGroupBoxFatFinger
            // 
            this.ultraExpandableGroupBoxFatFinger.Controls.Add(this.ultraExpandableGroupBoxPanelFatFinger);
            this.ultraExpandableGroupBoxFatFinger.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ultraExpandableGroupBoxFatFinger.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGroupBoxFatFinger.Expanded = false;
            this.ultraExpandableGroupBoxFatFinger.ExpandedSize = new System.Drawing.Size(1097, 172);
            this.ultraExpandableGroupBoxFatFinger.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.Near;
            this.ultraExpandableGroupBoxFatFinger.ExpansionIndicatorCollapsed = ((System.Drawing.Image)(resources.GetObject("ultraExpandableGroupBoxFatFinger.ExpansionIndicatorCollapsed")));
            this.ultraExpandableGroupBoxFatFinger.ExpansionIndicatorExpanded = ((System.Drawing.Image)(resources.GetObject("ultraExpandableGroupBoxFatFinger.ExpansionIndicatorExpanded")));
            this.ultraExpandableGroupBoxFatFinger.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance2.BackColor = System.Drawing.Color.DimGray;
            appearance2.FontData.BoldAsString = "False";
            appearance2.ForeColor = System.Drawing.Color.White;
            appearance2.ForeColorDisabled = System.Drawing.Color.White;
            this.ultraExpandableGroupBoxFatFinger.HeaderAppearance = appearance2;
            this.ultraExpandableGroupBoxFatFinger.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraExpandableGroupBoxFatFinger.Location = new System.Drawing.Point(0, 172);
            this.ultraExpandableGroupBoxFatFinger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraExpandableGroupBoxFatFinger.Name = "ultraExpandableGroupBoxFatFinger";
            this.ultraExpandableGroupBoxFatFinger.Size = new System.Drawing.Size(1097, 172);
            this.ultraExpandableGroupBoxFatFinger.TabIndex = 1;
            this.ultraExpandableGroupBoxFatFinger.Text = "FatFinger Rule";
            this.ultraExpandableGroupBoxFatFinger.UseAppStyling = false;
            this.ultraExpandableGroupBoxFatFinger.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraExpandableGroupBoxPanelFatFinger
            // 
            this.ultraExpandableGroupBoxPanelFatFinger.Controls.Add(this.ultraGridFatFingerRuleViolated);
            this.ultraExpandableGroupBoxPanelFatFinger.Controls.Add(this.ultraPanelPaddingFatFinger);
            this.ultraExpandableGroupBoxPanelFatFinger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanelFatFinger.Location = new System.Drawing.Point(3, 23);
            this.ultraExpandableGroupBoxPanelFatFinger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraExpandableGroupBoxPanelFatFinger.Name = "ultraExpandableGroupBoxPanelFatFinger";
            this.ultraExpandableGroupBoxPanelFatFinger.Size = new System.Drawing.Size(1091, 146);
            this.ultraExpandableGroupBoxPanelFatFinger.TabIndex = 0;
            // 
            // ultraGridFatFingerRuleViolated
            // 
            this.ultraGridFatFingerRuleViolated.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGridFatFingerRuleViolated.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridFatFingerRuleViolated.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGridFatFingerRuleViolated.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridFatFingerRuleViolated.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridFatFingerRuleViolated.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridFatFingerRuleViolated.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridFatFingerRuleViolated.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridFatFingerRuleViolated.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridFatFingerRuleViolated.DisplayLayout.UseFixedHeaders = true;
            this.ultraGridFatFingerRuleViolated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridFatFingerRuleViolated.Location = new System.Drawing.Point(0, 0);
            this.ultraGridFatFingerRuleViolated.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraGridFatFingerRuleViolated.Name = "ultraGridFatFingerRuleViolated";
            this.ultraGridFatFingerRuleViolated.Size = new System.Drawing.Size(1091, 136);
            this.ultraGridFatFingerRuleViolated.TabIndex = 1;
            this.ultraGridFatFingerRuleViolated.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridFatFingerRuleViolated_InitializeLayout);
            // 
            // ultraPanelPaddingFatFinger
            // 
            this.ultraPanelPaddingFatFinger.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanelPaddingFatFinger.Location = new System.Drawing.Point(0, 136);
            this.ultraPanelPaddingFatFinger.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ultraPanelPaddingFatFinger.Name = "ultraPanelPaddingFatFinger";
            this.ultraPanelPaddingFatFinger.Size = new System.Drawing.Size(1091, 10);
            this.ultraPanelPaddingFatFinger.TabIndex = 2;
            // 
            // ultraExpandableGroupBoxOverBuyOverSell
            // 
            this.ultraExpandableGroupBoxOverBuyOverSell.Controls.Add(this.ultraExpandableGroupBoxPanelOverBuyOverSell);
            this.ultraExpandableGroupBoxOverBuyOverSell.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ultraExpandableGroupBoxOverBuyOverSell.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGroupBoxOverBuyOverSell.Expanded = false;
            this.ultraExpandableGroupBoxOverBuyOverSell.ExpandedSize = new System.Drawing.Size(1097, 172);
            this.ultraExpandableGroupBoxOverBuyOverSell.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.Near;
            this.ultraExpandableGroupBoxOverBuyOverSell.ExpansionIndicatorCollapsed = ((System.Drawing.Image)(resources.GetObject("ultraExpandableGroupBoxOverBuyOverSell.ExpansionIndicatorCollapsed")));
            this.ultraExpandableGroupBoxOverBuyOverSell.ExpansionIndicatorExpanded = ((System.Drawing.Image)(resources.GetObject("ultraExpandableGroupBoxOverBuyOverSell.ExpansionIndicatorExpanded")));
            this.ultraExpandableGroupBoxOverBuyOverSell.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            appearance3.BackColor = System.Drawing.Color.DimGray;
            appearance3.FontData.BoldAsString = "False";
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraExpandableGroupBoxOverBuyOverSell.HeaderAppearance = appearance3;
            this.ultraExpandableGroupBoxOverBuyOverSell.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraExpandableGroupBoxOverBuyOverSell.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxOverBuyOverSell.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraExpandableGroupBoxOverBuyOverSell.Name = "ultraExpandableGroupBoxOverBuyOverSell";
            this.ultraExpandableGroupBoxOverBuyOverSell.Size = new System.Drawing.Size(1097, 172);
            this.ultraExpandableGroupBoxOverBuyOverSell.TabIndex = 0;
            this.ultraExpandableGroupBoxOverBuyOverSell.Text = "OverBuyOverSell Rule";
            this.ultraExpandableGroupBoxOverBuyOverSell.UseAppStyling = false;
            this.ultraExpandableGroupBoxOverBuyOverSell.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraExpandableGroupBoxPanelOverBuyOverSell
            // 
            this.ultraExpandableGroupBoxPanelOverBuyOverSell.Controls.Add(this.ultraGridOverBuyOverSellRuleViolated);
            this.ultraExpandableGroupBoxPanelOverBuyOverSell.Controls.Add(this.ultraPanelPaddingOverBuyOverSell);
            this.ultraExpandableGroupBoxPanelOverBuyOverSell.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanelOverBuyOverSell.Location = new System.Drawing.Point(3, 23);
            this.ultraExpandableGroupBoxPanelOverBuyOverSell.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraExpandableGroupBoxPanelOverBuyOverSell.Name = "ultraExpandableGroupBoxPanelOverBuyOverSell";
            this.ultraExpandableGroupBoxPanelOverBuyOverSell.Size = new System.Drawing.Size(1091, 146);
            this.ultraExpandableGroupBoxPanelOverBuyOverSell.TabIndex = 0;
            // 
            // ultraGridOverBuyOverSellRuleViolated
            // 
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraGridOverBuyOverSellRuleViolated.DisplayLayout.UseFixedHeaders = true;
            this.ultraGridOverBuyOverSellRuleViolated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGridOverBuyOverSellRuleViolated.Location = new System.Drawing.Point(0, 0);
            this.ultraGridOverBuyOverSellRuleViolated.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraGridOverBuyOverSellRuleViolated.Name = "ultraGridOverBuyOverSellRuleViolated";
            this.ultraGridOverBuyOverSellRuleViolated.Size = new System.Drawing.Size(1091, 136);
            this.ultraGridOverBuyOverSellRuleViolated.TabIndex = 1;
            this.ultraGridOverBuyOverSellRuleViolated.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGridOverBuyOverSellRuleViolated_InitializeLayout);
            // 
            // ultraPanelPaddingOverBuyOverSell
            // 
            this.ultraPanelPaddingOverBuyOverSell.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanelPaddingOverBuyOverSell.Location = new System.Drawing.Point(0, 136);
            this.ultraPanelPaddingOverBuyOverSell.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ultraPanelPaddingOverBuyOverSell.Name = "ultraPanelPaddingOverBuyOverSell";
            this.ultraPanelPaddingOverBuyOverSell.Size = new System.Drawing.Size(1091, 10);
            this.ultraPanelPaddingOverBuyOverSell.TabIndex = 1;
            // 
            // ultraLabelInformation
            // 
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Top";
            this.ultraLabelInformation.Appearance = appearance4;
            this.ultraLabelInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraLabelInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold);
            this.ultraLabelInformation.Location = new System.Drawing.Point(0, 0);
            this.ultraLabelInformation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraLabelInformation.Name = "ultraLabelInformation";
            this.ultraLabelInformation.Padding = new System.Drawing.Size(2, 2);
            this.ultraLabelInformation.Size = new System.Drawing.Size(1097, 49);
            this.ultraLabelInformation.TabIndex = 0;
            this.ultraLabelInformation.Text = "The following trading rules are violated. Do you want to continue?";
            this.ultraLabelInformation.UseAppStyling = false;
            // 
            // ultraButtonNo
            // 
            this.ultraButtonNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButtonNo.Location = new System.Drawing.Point(955, 5);
            this.ultraButtonNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraButtonNo.Name = "ultraButtonNo";
            this.ultraButtonNo.Size = new System.Drawing.Size(133, 33);
            this.ultraButtonNo.TabIndex = 1;
            this.ultraButtonNo.Text = "No";
            this.ultraButtonNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // ultraButtonYes
            // 
            this.ultraButtonYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButtonYes.Location = new System.Drawing.Point(815, 5);
            this.ultraButtonYes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraButtonYes.Name = "ultraButtonYes";
            this.ultraButtonYes.Size = new System.Drawing.Size(133, 33);
            this.ultraButtonYes.TabIndex = 0;
            this.ultraButtonYes.Text = "Yes";
            this.ultraButtonYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // ultraPanelTradingRulesViolatedBottom
            // 
            // 
            // ultraPanelTradingRulesViolatedBottom.ClientArea
            // 
            this.ultraPanelTradingRulesViolatedBottom.ClientArea.Controls.Add(this.ultraButtonNo);
            this.ultraPanelTradingRulesViolatedBottom.ClientArea.Controls.Add(this.ultraButtonYes);
            this.ultraPanelTradingRulesViolatedBottom.Cursor = System.Windows.Forms.Cursors.Default;
            this.ultraPanelTradingRulesViolatedBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraPanelTradingRulesViolatedBottom.Location = new System.Drawing.Point(9, 137);
            this.ultraPanelTradingRulesViolatedBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ultraPanelTradingRulesViolatedBottom.Name = "ultraPanelTradingRulesViolatedBottom";
            this.ultraPanelTradingRulesViolatedBottom.Size = new System.Drawing.Size(1097, 43);
            this.ultraPanelTradingRulesViolatedBottom.TabIndex = 0;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left
            // 
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 9;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 39);
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left.Name = "_TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left";
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(9, 141);
            // 
            // _TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right
            // 
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 9;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1106, 39);
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right.Name = "_TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right";
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(9, 141);
            // 
            // _TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top
            // 
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top.Name = "_TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top";
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1115, 39);
            // 
            // _TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom
            // 
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 9;
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 180);
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom.Name = "_TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom";
            this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1115, 9);
            // 
            // ultraPanelTradingRulesViolatedTop
            // 
            // 
            // ultraPanelTradingRulesViolatedTop.ClientArea
            // 
            this.ultraPanelTradingRulesViolatedTop.ClientArea.Controls.Add(this.ultraLabelInformation);
            this.ultraPanelTradingRulesViolatedTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanelTradingRulesViolatedTop.Location = new System.Drawing.Point(9, 39);
            this.ultraPanelTradingRulesViolatedTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ultraPanelTradingRulesViolatedTop.Name = "ultraPanelTradingRulesViolatedTop";
            this.ultraPanelTradingRulesViolatedTop.Size = new System.Drawing.Size(1097, 49);
            this.ultraPanelTradingRulesViolatedTop.TabIndex = 6;
            // 
            // TradingRulesViolatedPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1115, 189);
            this.Controls.Add(this.ultraPanelTradingRulesViolatedFill);
            this.Controls.Add(this.ultraPanelTradingRulesViolatedBottom);
            this.Controls.Add(this.ultraPanelTradingRulesViolatedTop);
            this.Controls.Add(this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(994, 88);
            this.Name = "TradingRulesViolatedPopUp";
            this.ShowIcon = false;
            this.Text = "Trading Rules Violated";
            this.ultraPanelTradingRulesViolatedFill.ClientArea.ResumeLayout(false);
            this.ultraPanelTradingRulesViolatedFill.ResumeLayout(false);
            this.ultraPanelTradingRulesViolatedFill.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBoxSharesOutstanding)).EndInit();
            this.ultraExpandableGroupBoxSharesOutstanding.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanelSharesOutStanding.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridSharesOutstandingRuleViolated)).EndInit();
            this.ultraPanelPaddingSharesOutstanding.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBoxFatFinger)).EndInit();
            this.ultraExpandableGroupBoxFatFinger.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanelFatFinger.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridFatFingerRuleViolated)).EndInit();
            this.ultraPanelPaddingFatFinger.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBoxOverBuyOverSell)).EndInit();
            this.ultraExpandableGroupBoxOverBuyOverSell.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanelOverBuyOverSell.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridOverBuyOverSellRuleViolated)).EndInit();
            this.ultraPanelPaddingOverBuyOverSell.ResumeLayout(false);
            this.ultraPanelTradingRulesViolatedBottom.ClientArea.ResumeLayout(false);
            this.ultraPanelTradingRulesViolatedBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ultraPanelTradingRulesViolatedTop.ClientArea.ResumeLayout(false);
            this.ultraPanelTradingRulesViolatedTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanelTradingRulesViolatedFill;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBoxSharesOutstanding;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanelSharesOutStanding;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridSharesOutstandingRuleViolated;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBoxFatFinger;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanelFatFinger;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridFatFingerRuleViolated;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBoxOverBuyOverSell;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanelOverBuyOverSell;
        private PranaUltraGrid ultraGridOverBuyOverSellRuleViolated;
        private Infragistics.Win.Misc.UltraButton ultraButtonNo;
        private Infragistics.Win.Misc.UltraButton ultraButtonYes;
        private Infragistics.Win.Misc.UltraLabel ultraLabelInformation;
        private Infragistics.Win.Misc.UltraPanel ultraPanelTradingRulesViolatedBottom;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _TradingRulesViolatedPopUp_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.Misc.UltraPanel ultraPanelTradingRulesViolatedTop;
        private Infragistics.Win.Misc.UltraPanel ultraPanelPaddingSharesOutstanding;
        private Infragistics.Win.Misc.UltraPanel ultraPanelPaddingFatFinger;
        private Infragistics.Win.Misc.UltraPanel ultraPanelPaddingOverBuyOverSell;
    }
}