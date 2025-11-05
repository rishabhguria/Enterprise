using ExportGridsData;
using Infragistics.Win;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WatchList.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace Prana.WatchList.Forms
{
    partial class WatchListMain
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
            //System.GC.SuppressFinalize(_symbolLiveFeedManager);
            if (disposing && (components != null))
            {
                components.Dispose();
                if (_symbolLiveFeedManager != null)
                {
                    _symbolLiveFeedManager.LinkedSymbolSelected -= SendLinkedSymbolToMain;
                    _symbolLiveFeedManager.Dispose();
                    _symbolLiveFeedManager = null;
                }
                InstanceManager.ReleaseInstance(typeof(WatchListMain));
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
            dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedTop);
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("watchlistToolBar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddTab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DelTab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RenameTab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AddTab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("DelTab");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("RenameTab");
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.userCtrl = new System.Windows.Forms.ContainerControl();
            this.ultraDockManager1 = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._Form1UnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._Form1UnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._Form1UnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._Form1UnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._WatchList_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._WatchList_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._WatchList_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._WatchList_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.MainTable = new System.Windows.Forms.TableLayoutPanel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ClientArea_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._ClientArea_Toolbars_Dock_Area_1_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._ClientArea_Toolbars_Dock_Area_1_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_1_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_1_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblErrorMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this._ClientArea_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._ClientArea_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).BeginInit();
            this.MainTable.SuspendLayout();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ClientArea_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _WatchList_UltraFormManager_Dock_Area_Left
            // 
            this._WatchList_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WatchList_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WatchList_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._WatchList_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WatchList_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._WatchList_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._WatchList_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._WatchList_UltraFormManager_Dock_Area_Left.Name = "_WatchList_UltraFormManager_Dock_Area_Left";
            this._WatchList_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 444);
            // 
            // _WatchList_UltraFormManager_Dock_Area_Right
            // 
            this._WatchList_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WatchList_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WatchList_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._WatchList_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WatchList_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._WatchList_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._WatchList_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1011, 32);
            this._WatchList_UltraFormManager_Dock_Area_Right.Name = "_WatchList_UltraFormManager_Dock_Area_Right";
            this._WatchList_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 444);
            // 
            // _WatchList_UltraFormManager_Dock_Area_Top
            // 
            this._WatchList_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WatchList_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WatchList_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._WatchList_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WatchList_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._WatchList_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._WatchList_UltraFormManager_Dock_Area_Top.Name = "_WatchList_UltraFormManager_Dock_Area_Top";
            this._WatchList_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1019, 32);
            // 
            // _WatchList_UltraFormManager_Dock_Area_Bottom
            // 
            this._WatchList_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._WatchList_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._WatchList_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._WatchList_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._WatchList_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._WatchList_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._WatchList_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 476);
            this._WatchList_UltraFormManager_Dock_Area_Bottom.Name = "_WatchList_UltraFormManager_Dock_Area_Bottom";
            this._WatchList_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1019, 8);
            // 
            // ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;            
            // 
            // MainTable
            // 
            this.MainTable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this.MainTable.ColumnCount = 1;
            this.MainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTable.Controls.Add(this.userCtrl, 0, 1);
            this.MainTable.Controls.Add(this.ultraPanel1, 0, 0);
            this.MainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTable.Location = new System.Drawing.Point(8, 32);
            this.MainTable.Margin = new System.Windows.Forms.Padding(0);
            this.MainTable.Name = "MainTable";
            this.MainTable.RowCount = 1;
            this.MainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTable.Size = new System.Drawing.Size(1003, 444);
            this.MainTable.TabIndex = 0;
            //
            //dockAreaPane1
            //
            dockAreaPane1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            dockAreaPane1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            dockAreaPane1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            dockAreaPane1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            dockAreaPane1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            dockAreaPane1.Settings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            dockAreaPane1.Settings.AllowPin = Infragistics.Win.DefaultableBoolean.False;
            dockAreaPane1.Settings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            dockAreaPane1.Settings.DoubleClickAction = PaneDoubleClickAction.None;
            dockAreaPane1.ChildPaneStyle = ChildPaneStyle.TabGroup;
            //dockAreaPane1.Settings.TabWidth += 100;
            AppearanceBase tabAppearance = new Infragistics.Win.Appearance();
            tabAppearance.BackColor = Color.Gainsboro;
            tabAppearance.ForeColor = Color.Black;
            tabAppearance.FontData.Bold = DefaultableBoolean.True;
            AppearanceBase activeTabAppearance = new Infragistics.Win.Appearance();
            activeTabAppearance.BackColor = Color.White;
            activeTabAppearance.ForeColor = Color.Black;
            activeTabAppearance.FontData.Bold = DefaultableBoolean.True;
            dockAreaPane1.DefaultPaneSettings.TabAppearance = tabAppearance;
            dockAreaPane1.DefaultPaneSettings.SelectedTabAppearance = activeTabAppearance;
            //dockAreaPane1.DefaultPaneSettings.TabWidth = 150;
            dockAreaPane1.DefaultPaneSettings.TabAppearance.FontData.SizeInPoints = 10;
            dockAreaPane1.DefaultPaneSettings.TabAppearance.BorderColor3DBase = Color.Black;
            //dockAreaPane1.
            //
            //ultraDockManager1
            //
            this.ultraDockManager1.HostControl = userCtrl;
            this.ultraDockManager1.AllowDrop = false;
            this.ultraDockManager1.ShowPinButton = false;
            this.ultraDockManager1.ShowCloseButton = false;
            this.ultraDockManager1.UseAppStyling = false;
            this.ultraDockManager1.UseOsThemes = DefaultableBoolean.False;
            this.ultraDockManager1.DefaultGroupSettings.TabAreaAppearance.BackColor = Color.Transparent;
            ultraDockManager1.DefaultGroupSettings.TabLocation = Infragistics.Win.UltraWinDock.Location.Top;
            
            this.ultraDockManager1.LayoutStyle = Infragistics.Win.UltraWinDock.DockAreaLayoutStyle.FillContainer;
            ultraDockManager1.DefaultPaneSettings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            ultraDockManager1.DefaultPaneSettings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            ultraDockManager1.DefaultPaneSettings.AllowDockRight = Infragistics.Win.DefaultableBoolean.False;
            ultraDockManager1.DefaultPaneSettings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            ultraDockManager1.DefaultPaneSettings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            ultraDockManager1.DefaultPaneSettings.AllowMaximize = Infragistics.Win.DefaultableBoolean.False;
            ultraDockManager1.DefaultPaneSettings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            ultraDockManager1.DefaultPaneSettings.ShowCaption = DefaultableBoolean.False;
            ultraDockManager1.AfterToggleDockState += ultraDockManager1_AfterToggleDockState;

            #region tabareas
            // 
            // _Form1UnpinnedTabAreaLeft
            // 
            this._Form1UnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._Form1UnpinnedTabAreaLeft.Name = "_Form1UnpinnedTabAreaLeft";
            this._Form1UnpinnedTabAreaLeft.Owner = this.ultraDockManager1;
            // 
            // _Form1UnpinnedTabAreaRight
            // 
            this._Form1UnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._Form1UnpinnedTabAreaRight.Name = "_Form1UnpinnedTabAreaRight";
            this._Form1UnpinnedTabAreaRight.Owner = this.ultraDockManager1;
            // 
            // _Form1UnpinnedTabAreaTop
            // 
            this._Form1UnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._Form1UnpinnedTabAreaTop.Name = "_Form1UnpinnedTabAreaTop";
            this._Form1UnpinnedTabAreaTop.Owner = this.ultraDockManager1;
            // 
            // _Form1UnpinnedTabAreaBottom
            // 
            this._Form1UnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._Form1UnpinnedTabAreaBottom.Name = "_Form1UnpinnedTabAreaBottom";
            this._Form1UnpinnedTabAreaBottom.Owner = this.ultraDockManager1;
            #endregion

            // 
            // userCtrl
            // 
            this.userCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userCtrl.Name = "userCtrl";
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.ClientArea_Fill_Panel);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_1_Left);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_1_Right);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_1_Bottom);
            this.ultraPanel1.ClientArea.Controls.Add(this._ClientArea_Toolbars_Dock_Area_1_Top);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1003, 30);
            this.ultraPanel1.TabIndex = 2;
            // 
            // ClientArea_Fill_Panel
            // 
            this.ClientArea_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.ClientArea_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClientArea_Fill_Panel.Location = new System.Drawing.Point(0, 25);
            this.ClientArea_Fill_Panel.Name = "ClientArea_Fill_Panel";
            this.ClientArea_Fill_Panel.Size = new System.Drawing.Size(1003, 5);
            this.ClientArea_Fill_Panel.TabIndex = 0;
            // 
            // _ClientArea_Toolbars_Dock_Area_1_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_1_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_1_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this._ClientArea_Toolbars_Dock_Area_1_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._ClientArea_Toolbars_Dock_Area_1_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_1_Left.Location = new System.Drawing.Point(0, 25);
            this._ClientArea_Toolbars_Dock_Area_1_Left.Name = "_ClientArea_Toolbars_Dock_Area_1_Left";
            this._ClientArea_Toolbars_Dock_Area_1_Left.Size = new System.Drawing.Size(0, 5);
            this._ClientArea_Toolbars_Dock_Area_1_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this.ultraPanel1.ClientArea;
            this.ultraToolbarsManager1.LockToolbars = true;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            this.ultraToolbarsManager1.ShowQuickCustomizeButton = false;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool4,
            buttonTool5});
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            this.ultraToolbarsManager1.ToolbarSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.ToolbarSettings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
            buttonTool2.SharedPropsInternal.Caption = "Add Tab";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool2.SharedPropsInternal.ToolTipText = "Add Tab";
            buttonTool3.SharedPropsInternal.Caption = "Delete Tab";
            buttonTool3.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool3.SharedPropsInternal.ToolTipText = "Delete Tab";
            buttonTool6.SharedPropsInternal.Caption = "Rename Tab";
            buttonTool6.SharedPropsInternal.CustomizerCaption = "RenameTab";
            buttonTool6.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2,
            buttonTool3,
            buttonTool6});
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick);
            // 
            // _ClientArea_Toolbars_Dock_Area_1_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_1_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_1_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this._ClientArea_Toolbars_Dock_Area_1_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._ClientArea_Toolbars_Dock_Area_1_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_1_Right.Location = new System.Drawing.Point(1003, 25);
            this._ClientArea_Toolbars_Dock_Area_1_Right.Name = "_ClientArea_Toolbars_Dock_Area_1_Right";
            this._ClientArea_Toolbars_Dock_Area_1_Right.Size = new System.Drawing.Size(0, 5);
            this._ClientArea_Toolbars_Dock_Area_1_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ClientArea_Toolbars_Dock_Area_1_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_1_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_1_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this._ClientArea_Toolbars_Dock_Area_1_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._ClientArea_Toolbars_Dock_Area_1_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_1_Bottom.Location = new System.Drawing.Point(0, 30);
            this._ClientArea_Toolbars_Dock_Area_1_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_1_Bottom";
            this._ClientArea_Toolbars_Dock_Area_1_Bottom.Size = new System.Drawing.Size(1003, 0);
            this._ClientArea_Toolbars_Dock_Area_1_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _ClientArea_Toolbars_Dock_Area_1_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_1_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_1_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(210)))), ((int)(((byte)(212)))));
            this._ClientArea_Toolbars_Dock_Area_1_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._ClientArea_Toolbars_Dock_Area_1_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ClientArea_Toolbars_Dock_Area_1_Top.Location = new System.Drawing.Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_1_Top.Name = "_ClientArea_Toolbars_Dock_Area_1_Top";
            this._ClientArea_Toolbars_Dock_Area_1_Top.Size = new System.Drawing.Size(1003, 25);
            this._ClientArea_Toolbars_Dock_Area_1_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblErrorMessage});
            this.statusStrip1.BackColor = Color.FromArgb(88, 88, 90);
            this.lblErrorMessage.BackColor = Color.FromArgb(88, 88, 90);
            this.statusStrip1.ForeColor = Color.FromArgb(236, 240, 241);
            this.lblErrorMessage.ForeColor = Color.FromArgb(236, 240, 241);
            this.statusStrip1.Location = new System.Drawing.Point(0, 424);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1003, 20);
            this.statusStrip1.TabIndex = 23;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblErrorMessage.Margin = new System.Windows.Forms.Padding(0);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(0, 0);
            // 
            // _ClientArea_Toolbars_Dock_Area_Left
            // 
            this._ClientArea_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Floating;
            this._ClientArea_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 52);
            this._ClientArea_Toolbars_Dock_Area_Left.Name = "_ClientArea_Toolbars_Dock_Area_Left";
            this._ClientArea_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 0);
            // 
            // _ClientArea_Toolbars_Dock_Area_Right
            // 
            this._ClientArea_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Floating;
            this._ClientArea_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1003, 52);
            this._ClientArea_Toolbars_Dock_Area_Right.Name = "_ClientArea_Toolbars_Dock_Area_Right";
            this._ClientArea_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 0);
            // 
            // _ClientArea_Toolbars_Dock_Area_Top
            // 
            this._ClientArea_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Floating;
            this._ClientArea_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ClientArea_Toolbars_Dock_Area_Top.Name = "_ClientArea_Toolbars_Dock_Area_Top";
            this._ClientArea_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1003, 52);
            // 
            // _ClientArea_Toolbars_Dock_Area_Bottom
            // 
            this._ClientArea_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ClientArea_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Floating;
            this._ClientArea_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 30);
            this._ClientArea_Toolbars_Dock_Area_Bottom.Name = "_ClientArea_Toolbars_Dock_Area_Bottom";
            this._ClientArea_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1003, 0);
            // 
            // WatchListMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(1019, 484);
            this.Controls.Add(this.MainTable);
            this.Controls.Add(this._WatchList_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._WatchList_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._WatchList_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._WatchList_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(87, 100);
            this.Name = "WatchListMain";
            this.Text = "Watchlist";
            this.BackColor = Color.Gainsboro;
            this.Load += new System.EventHandler(this.WatchListMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.MainTable.ResumeLayout(false);
            this.MainTable.PerformLayout();
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ClientArea_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainTable;
        //private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WatchList_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WatchList_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WatchList_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _WatchList_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager ultraToolTipManager1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ClientArea_Fill_Panel;
        private UltraToolbarsManager ultraToolbarsManager1;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_1_Left;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_1_Right;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_1_Bottom;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_1_Top;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Left;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Right;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Top;
        private UltraToolbarsDockArea _ClientArea_Toolbars_Dock_Area_Bottom;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblErrorMessage;
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager1;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _Form1UnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _Form1UnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _Form1UnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _Form1UnpinnedTabAreaBottom;
        private System.Windows.Forms.ContainerControl userCtrl;
        Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1;

    }
}

