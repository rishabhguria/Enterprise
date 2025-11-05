using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinForm;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinTabs;
using Infragistics.Win.UltraWinToolbars;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.PM.Client.UI.Controls;
using DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition;

namespace Prana.PM.Client.UI.Forms
{
    partial class PM
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
                if(_exInstance != null)
                {
                    _exInstance.Dispose();
                }
                if(_pmAppearance != null)
                {
                    _pmAppearance.Dispose();
                }
                if(frmEquityBaseValue != null)
                {
                    frmEquityBaseValue.Dispose();
                }
            }
            //if (_fundBindableView != null)
            //    _fundBindableView.Dispose();

            ClearData();
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("toolbarPMButtons");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool169 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Admin");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool170 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RefreshData");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool172 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Reconciliation");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool173 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CashManagement");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool174 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Stage");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool175 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Start Writing Data");
            
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool177 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CorporateAction");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool179 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ClosePositions");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool180 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool181 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MarkPrice");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool182 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool183 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UnrealizedPNL");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool184 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Options Simulator");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool185 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Step Analysis");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool186 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Daily Reports");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool187 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Stage");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool190 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool191 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AmendCorrect");
            
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool193 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Admin");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool194 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool195 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Reconciliation");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool196 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CashManagement");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool197 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Stage");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool198 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ClosePositions");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool199 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AmendCorrect");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool200 = new Infragistics.Win.UltraWinToolbars.ButtonTool("MarkPrice");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool201 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CreatePosition");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool202 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool204 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Options Simulator");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool205 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Step Analysis");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool206 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Daily Reports");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool207 = new Infragistics.Win.UltraWinToolbars.ButtonTool("UnrealizedPNL");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool208 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RefreshData");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool209 = new Infragistics.Win.UltraWinToolbars.ButtonTool("BASEEQUITYVALUE");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool210 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ACCRUALS");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool211 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DESK");
            
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool213 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool214 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CorporateAction");
            
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool216 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Start Writing Data");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool8 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cmbFilter");
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool218 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ScreenShot");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("lblGotoTab");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool2 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cmbGotoTab");
            Infragistics.Win.ValueList valueList2 = new Infragistics.Win.ValueList(0);
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.defaultPmUserControl = new Prana.PM.Client.UI.Controls.PMUserControl();
            this.ultraTabSharedControlsPage16 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ultraTabSharedControlsPage17 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.PM_Fill_Panel = new System.Windows.Forms.Panel();
            this.PMTabView = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this._PM_Fill_Panel_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._PM_Fill_Panel_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._PM_Fill_Panel_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._PM_Fill_Panel_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addCustomViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._PM_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PM_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PM_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PM_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraTabPageControl1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.PM_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PMTabView)).BeginInit();
            this.PMTabView.SuspendLayout();
            this.ultraTabSharedControlsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.defaultPmUserControl);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(1277, 508);
            // 
            // defaultPmUserControl
            // 
            this.defaultPmUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defaultPmUserControl.Location = new System.Drawing.Point(0, 0);
            this.defaultPmUserControl.Name = "defaultPmUserControl";
            this.defaultPmUserControl.Size = new System.Drawing.Size(1277, 508);
            this.defaultPmUserControl.TabIndex = 0;
            // 
            // ultraTabSharedControlsPage16
            // 
            this.ultraTabSharedControlsPage16.Location = new System.Drawing.Point(1, 1);
            this.ultraTabSharedControlsPage16.Name = "ultraTabSharedControlsPage16";
            this.ultraTabSharedControlsPage16.Size = new System.Drawing.Size(1065, 332);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(8, 610);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1279, 22);
            this.statusStrip1.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoToolTip = true;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // ultraTabSharedControlsPage17
            // 
            this.ultraTabSharedControlsPage17.Location = new System.Drawing.Point(1, 1);
            this.ultraTabSharedControlsPage17.Name = "ultraTabSharedControlsPage17";
            this.ultraTabSharedControlsPage17.Size = new System.Drawing.Size(996, 359);
            // 
            // PM_Fill_Panel
            // 
            this.PM_Fill_Panel.Controls.Add(this.PMTabView);
            this.PM_Fill_Panel.Controls.Add(this._PM_Fill_Panel_Toolbars_Dock_Area_Left);
            this.PM_Fill_Panel.Controls.Add(this._PM_Fill_Panel_Toolbars_Dock_Area_Right);
            this.PM_Fill_Panel.Controls.Add(this._PM_Fill_Panel_Toolbars_Dock_Area_Bottom);
            this.PM_Fill_Panel.Controls.Add(this._PM_Fill_Panel_Toolbars_Dock_Area_Top);
            this.PM_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.PM_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PM_Fill_Panel.Location = new System.Drawing.Point(8, 31);
            this.PM_Fill_Panel.Name = "PM_Fill_Panel";
            this.PM_Fill_Panel.Size = new System.Drawing.Size(1279, 579);
            this.PM_Fill_Panel.TabIndex = 0;
            // 
            // PMTabView
            // 
            this.PMTabView.AllowTabMoving = true;
            this.PMTabView.Controls.Add(this.ultraTabSharedControlsPage);
            this.PMTabView.Controls.Add(this.ultraTabPageControl1);
            this.PMTabView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PMTabView.Location = new System.Drawing.Point(0, 50);
            this.PMTabView.Name = "PMTabView";
            appearance1.BackColor2 = System.Drawing.Color.White;
            this.PMTabView.SelectedTabAppearance = appearance1;
            this.PMTabView.SharedControls.AddRange(new System.Windows.Forms.Control[] {
            this.defaultPmUserControl});
            this.PMTabView.SharedControlsPage = this.ultraTabSharedControlsPage;
            this.PMTabView.Size = new System.Drawing.Size(1279, 529);
            this.PMTabView.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.PMTabView.TabIndex = 5;
            ultraTab1.Key = "Main";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Main";
            this.PMTabView.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            this.PMTabView.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.PMTabView_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage
            // 
            this.ultraTabSharedControlsPage.Controls.Add(this.defaultPmUserControl);
            this.ultraTabSharedControlsPage.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage.Name = "ultraTabSharedControlsPage";
            this.ultraTabSharedControlsPage.Size = new System.Drawing.Size(1277, 508);
            // 
            // _PM_Fill_Panel_Toolbars_Dock_Area_Left
            // 
            this._PM_Fill_Panel_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 50);
            this._PM_Fill_Panel_Toolbars_Dock_Area_Left.Name = "_PM_Fill_Panel_Toolbars_Dock_Area_Left";
            this._PM_Fill_Panel_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 529);
            this._PM_Fill_Panel_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this.PM_Fill_Panel;
            this.ultraToolbarsManager1.LockToolbars = true;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.FloatingLocation = new System.Drawing.Point(139, 194);
            ultraToolbar1.FloatingSize = new System.Drawing.Size(348, 42);
            buttonTool191.InstanceProps.Visible = Infragistics.Win.DefaultableBoolean.False;
            
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool169,
            buttonTool170,
            buttonTool172,
            buttonTool173,
            buttonTool174,
            buttonTool175,

            buttonTool177,
            buttonTool179,
            buttonTool180,
            buttonTool181,
            buttonTool182,
            buttonTool183,
            buttonTool184,
            buttonTool185,
            buttonTool186,
            buttonTool187,
            buttonTool190,
            buttonTool191
            });
            ultraToolbar1.Text = "toolbarPMButtons";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            buttonTool193.SharedPropsInternal.Caption = "Admin";
            buttonTool193.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool193.SharedPropsInternal.Enabled = false;
            buttonTool193.SharedPropsInternal.Visible = false;
            buttonTool194.SharedPropsInternal.Caption = "Exit";
            buttonTool194.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool194.SharedPropsInternal.ToolTipText = "Exit";
            buttonTool194.SharedPropsInternal.Visible = false;
            buttonTool195.SharedPropsInternal.Caption = "Reconciliation";
            buttonTool195.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool195.SharedPropsInternal.ToolTipText = "Trade Reconciliation";
            buttonTool195.SharedPropsInternal.Visible = false;
            buttonTool196.SharedPropsInternal.Caption = "Cash Mgt.";
            buttonTool196.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool196.SharedPropsInternal.ToolTipText = "Cash  Management";
            buttonTool196.SharedPropsInternal.Visible = false;
            buttonTool197.SharedPropsInternal.Caption = "Stage";
            buttonTool197.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool197.SharedPropsInternal.ToolTipText = "Stage";
            buttonTool197.SharedPropsInternal.Visible = false;
            buttonTool198.SharedPropsInternal.Caption = "Close Positions";
            buttonTool198.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool198.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
            buttonTool198.SharedPropsInternal.ToolTipText = "Close Positions";
            buttonTool199.SharedPropsInternal.Caption = "Amend & Correct";
            buttonTool199.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            buttonTool199.SharedPropsInternal.ToolTipText = "Amend Correct";
            buttonTool199.SharedPropsInternal.Visible = false;
            buttonTool200.SharedPropsInternal.Caption = "Daily Valuation";
            buttonTool200.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool200.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            buttonTool200.SharedPropsInternal.ToolTipText = "Daily Valuation";
            buttonTool201.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
            buttonTool201.SharedPropsInternal.ToolTipText = "Create Transaction";
            buttonTool202.SharedPropsInternal.Caption = "Preferences";
            buttonTool202.SharedPropsInternal.ToolTipText = "Preferences";
            buttonTool202.SharedPropsInternal.Visible = false;
            buttonTool204.SharedPropsInternal.Caption = "Options Simulator";
            buttonTool204.SharedPropsInternal.ToolTipText = "Options Simulator";
            buttonTool204.SharedPropsInternal.Visible = false;
            buttonTool205.SharedPropsInternal.Caption = "Step Analysis";
            buttonTool205.SharedPropsInternal.CustomizerCaption = "Step Analysis";
            buttonTool205.SharedPropsInternal.Visible = false;
            buttonTool206.SharedPropsInternal.Caption = "Daily Reports";
            buttonTool206.SharedPropsInternal.CustomizerCaption = "Daily Reports";
            buttonTool206.SharedPropsInternal.ToolTipText = "Daily Reports";
            buttonTool206.SharedPropsInternal.Visible = false;
            buttonTool207.SharedPropsInternal.Caption = "Save Unrealized PNL";
            buttonTool207.SharedPropsInternal.ToolTipText = "Save Unrealized PNL";
            buttonTool207.SharedPropsInternal.Visible = false;
            buttonTool208.SharedPropsInternal.Caption = "Refresh/Reconnect";
            buttonTool208.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            buttonTool208.SharedPropsInternal.ToolTipText = "Refresh Allocation & Closing";
            buttonTool208.SharedPropsInternal.Visible = false;
            buttonTool209.SharedPropsInternal.Caption = "Base Equity Value";
            buttonTool209.SharedPropsInternal.ToolTipText = "Base Equity Value";
            buttonTool210.SharedPropsInternal.Caption = "Accruals";
            buttonTool210.SharedPropsInternal.ToolTipText = "Accruals";
            buttonTool211.SharedPropsInternal.Caption = "Desk";
            buttonTool211.SharedPropsInternal.ToolTipText = "Desk";
            buttonTool211.SharedPropsInternal.Visible = false;
            
            buttonTool213.SharedPropsInternal.Caption = "ButtonTool1";
            buttonTool214.SharedPropsInternal.Caption = "Corporate Actions";
            buttonTool214.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            
            buttonTool216.SharedPropsInternal.Caption = "Start Writing Data";
            buttonTool216.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            comboBoxTool8.SharedPropsInternal.Caption = "Go to Account";
            comboBoxTool8.ValueList = valueList1;
            buttonTool218.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            labelTool2.SharedPropsInternal.Caption = "Go to Tab";
            comboBoxTool2.SharedPropsInternal.Caption = "Go to Tab Combo";
            comboBoxTool2.ValueList = valueList2;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool193,
            buttonTool194,
            buttonTool195,
            buttonTool196,
            buttonTool197,
            buttonTool198,
            buttonTool199,
            buttonTool200,
            buttonTool202,
            buttonTool204,
            buttonTool205,
            buttonTool206,
            buttonTool207,
            buttonTool208,
            buttonTool209,
            buttonTool210,
            buttonTool211,
            buttonTool213,
            buttonTool214,
            
            buttonTool216,
            comboBoxTool8,
            buttonTool218,
            labelTool2,
            comboBoxTool2});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _PM_Fill_Panel_Toolbars_Dock_Area_Right
            // 
            this._PM_Fill_Panel_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1279, 50);
            this._PM_Fill_Panel_Toolbars_Dock_Area_Right.Name = "_PM_Fill_Panel_Toolbars_Dock_Area_Right";
            this._PM_Fill_Panel_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 529);
            this._PM_Fill_Panel_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _PM_Fill_Panel_Toolbars_Dock_Area_Bottom
            // 
            this._PM_Fill_Panel_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 579);
            this._PM_Fill_Panel_Toolbars_Dock_Area_Bottom.Name = "_PM_Fill_Panel_Toolbars_Dock_Area_Bottom";
            this._PM_Fill_Panel_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1279, 0);
            this._PM_Fill_Panel_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _PM_Fill_Panel_Toolbars_Dock_Area_Top
            // 
            this._PM_Fill_Panel_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PM_Fill_Panel_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._PM_Fill_Panel_Toolbars_Dock_Area_Top.Name = "_PM_Fill_Panel_Toolbars_Dock_Area_Top";
            this._PM_Fill_Panel_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1279, 50);
            this._PM_Fill_Panel_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // addCustomViewToolStripMenuItem
            // 
            this.addCustomViewToolStripMenuItem.Name = "addCustomViewToolStripMenuItem";
            this.addCustomViewToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "Nirvana Help.chm";
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(1067, 356);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _PM_UltraFormManager_Dock_Area_Left
            // 
            this._PM_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PM_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PM_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._PM_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PM_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._PM_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._PM_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._PM_UltraFormManager_Dock_Area_Left.Name = "_PM_UltraFormManager_Dock_Area_Left";
            this._PM_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 601);
            // 
            // _PM_UltraFormManager_Dock_Area_Right
            // 
            this._PM_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PM_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PM_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._PM_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PM_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._PM_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._PM_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1287, 31);
            this._PM_UltraFormManager_Dock_Area_Right.Name = "_PM_UltraFormManager_Dock_Area_Right";
            this._PM_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 601);
            // 
            // _PM_UltraFormManager_Dock_Area_Top
            // 
            this._PM_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PM_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PM_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._PM_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PM_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._PM_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._PM_UltraFormManager_Dock_Area_Top.Name = "_PM_UltraFormManager_Dock_Area_Top";
            this._PM_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1295, 31);
            // 
            // _PM_UltraFormManager_Dock_Area_Bottom
            // 
            this._PM_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PM_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PM_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._PM_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PM_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._PM_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._PM_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 632);
            this._PM_UltraFormManager_Dock_Area_Bottom.Name = "_PM_UltraFormManager_Dock_Area_Bottom";
            this._PM_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1295, 8);
            // 
            // PM
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1295, 640);
            this.Controls.Add(this.PM_Fill_Panel);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._PM_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._PM_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._PM_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._PM_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.helpProvider1.SetHelpKeyword(this, "ConsolidationView.html");
            this.helpProvider1.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Name = "PM";
            this.helpProvider1.SetShowHelp(this, true);
            this.Text = "Portfolio Management";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PM_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PM_FormClosed);
            this.Load += new System.EventHandler(this.PM_Load);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.PM_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PMTabView)).EndInit();
            this.PMTabView.ResumeLayout(false);
            this.ultraTabSharedControlsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private Panel PM_Fill_Panel;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem addCustomViewToolStripMenuItem;
        private UltraTabSharedControlsPage ultraTabSharedControlsPage17;
        private HelpProvider helpProvider1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;
        private UltraTabSharedControlsPage ultraTabSharedControlsPage16;
        private UltraTabPageControl ultraTabPageControl2;
        private UltraFormManager ultraFormManager1;
        private UltraFormDockArea _PM_UltraFormManager_Dock_Area_Left;
        private UltraFormDockArea _PM_UltraFormManager_Dock_Area_Right;
        private UltraFormDockArea _PM_UltraFormManager_Dock_Area_Top;
        private UltraFormDockArea _PM_UltraFormManager_Dock_Area_Bottom;
        private PMUserControl defaultPmUserControl;
        private UltraToolbarsDockArea _PM_Fill_Panel_Toolbars_Dock_Area_Left;
        private UltraToolbarsManager ultraToolbarsManager1;
        private UltraToolbarsDockArea _PM_Fill_Panel_Toolbars_Dock_Area_Right;
        private UltraToolbarsDockArea _PM_Fill_Panel_Toolbars_Dock_Area_Bottom;
        private UltraToolbarsDockArea _PM_Fill_Panel_Toolbars_Dock_Area_Top;
        internal UltraTabControl PMTabView;
        private UltraTabSharedControlsPage ultraTabSharedControlsPage;
        private UltraTabPageControl ultraTabPageControl1;
    }
}