using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Prana.Interfaces;
using Infragistics.Win.UltraWinGrid;
using System.Xml.Serialization ;
using System.IO;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Utilities.Win32Utilities;
using Prana.Utilities.DateTimeUtilities;
using Prana.Allocation.BLL;
using Prana.Utilities.UIUtilities;
using Prana.CommonDataCache;

namespace Prana.Allocation
{
	/// <summary>
	/// Summary description for Allocations.
	/// </summary>
	public class AllocationMain : System.Windows.Forms.Form,IAllocation
	{
        

        AllocationReport _allocationReport ;
		
		#region Windows Form Designer generated code

        private System.Windows.Forms.Panel AllocationMain_Fill_Panel;
		private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
		private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Timer timerUpdate;
        private HelpProvider helpProviderAllocation;
        private Button btGetAllocation;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationMain_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationMain_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationMain_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _AllocationMain_Toolbars_Dock_Area_Bottom;
        private RadioButton rbHistorical;
        private RadioButton rbCurrent;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtPickerAllocation;
        private Timer timerGetData;

		private System.ComponentModel.IContainer components;
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AllocationMain));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Commission");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Preferences");
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Report");
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Commission");
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Get Data");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Auto Group");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool1 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerTool1");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool3 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("ControlContainerTool2");
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.AllocationMain_Fill_Panel = new System.Windows.Forms.Panel();
            this.dtPickerAllocation = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.btn_AtoGroup = new System.Windows.Forms.Button();
            this.rbHistorical = new System.Windows.Forms.RadioButton();
            this.rbCurrent = new System.Windows.Forms.RadioButton();
            this.btGetAllocation = new System.Windows.Forms.Button();
            this.tabCtrlAllocation = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.helpProviderAllocation = new System.Windows.Forms.HelpProvider();
            this.timerGetData = new System.Windows.Forms.Timer(this.components);
            this.fundUserControl = new Prana.Allocation.FundUserControl();
            this.StrategyUserControl = new Prana.Allocation.FundUserControl();
            this._AllocationMain_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._AllocationMain_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._AllocationMain_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._AllocationMain_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.AllocationMain_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerAllocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlAllocation)).BeginInit();
            this.tabCtrlAllocation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this.fundUserControl);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(912, 473);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.StrategyUserControl);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(912, 498);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "");
            this.imgList.Images.SetKeyName(1, "");
            this.imgList.Images.SetKeyName(2, "");
            this.imgList.Images.SetKeyName(3, "");
            this.imgList.Images.SetKeyName(4, "");
            this.imgList.Images.SetKeyName(5, "service.png");
            this.imgList.Images.SetKeyName(6, "CA32TBL9.jpg");
            // 
            // AllocationMain_Fill_Panel
            // 
            this.AllocationMain_Fill_Panel.Controls.Add(this.dtPickerAllocation);
            this.AllocationMain_Fill_Panel.Controls.Add(this.btn_AtoGroup);
            this.AllocationMain_Fill_Panel.Controls.Add(this.rbHistorical);
            this.AllocationMain_Fill_Panel.Controls.Add(this.rbCurrent);
            this.AllocationMain_Fill_Panel.Controls.Add(this.btGetAllocation);
            this.AllocationMain_Fill_Panel.Controls.Add(this.tabCtrlAllocation);
            this.AllocationMain_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AllocationMain_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AllocationMain_Fill_Panel.Location = new System.Drawing.Point(0, 23);
            this.AllocationMain_Fill_Panel.Name = "AllocationMain_Fill_Panel";
            this.AllocationMain_Fill_Panel.Size = new System.Drawing.Size(914, 550);
            this.AllocationMain_Fill_Panel.TabIndex = 0;
            // 
            // dtPickerAllocation
            // 
            this.dtPickerAllocation.Location = new System.Drawing.Point(150, 7);
            this.dtPickerAllocation.MaxDate = new System.DateTime(2008, 6, 25, 0, 0, 0, 0);
            this.dtPickerAllocation.Name = "dtPickerAllocation";
            this.dtPickerAllocation.Size = new System.Drawing.Size(80, 22);
            this.dtPickerAllocation.TabIndex = 14;
            this.dtPickerAllocation.ValueChanged += new System.EventHandler(this.dtPickerAllocationMain_ValueChanged);
            // 
            // btn_AtoGroup
            // 
            this.btn_AtoGroup.Location = new System.Drawing.Point(350, 7);
            this.btn_AtoGroup.Name = "btn_AtoGroup";
            this.btn_AtoGroup.Size = new System.Drawing.Size(80, 23);
            this.btn_AtoGroup.TabIndex = 6;
            this.btn_AtoGroup.Text = "Auto Group";
            this.btn_AtoGroup.Click += new System.EventHandler(this.btn_AtoGroup_Click);
            // 
            // rbHistorical
            // 
            this.rbHistorical.AutoSize = true;
            this.rbHistorical.Location = new System.Drawing.Point(73, 9);
            this.rbHistorical.Name = "rbHistorical";
            this.rbHistorical.Size = new System.Drawing.Size(71, 17);
            this.rbHistorical.TabIndex = 13;
            this.rbHistorical.TabStop = true;
            this.rbHistorical.Text = "Historical ";
            this.rbHistorical.UseVisualStyleBackColor = true;
            this.rbHistorical.CheckedChanged += new System.EventHandler(this.rbHistorical_CheckedChanged);
            // 
            // rbCurrent
            // 
            this.rbCurrent.AutoSize = true;
            this.rbCurrent.Location = new System.Drawing.Point(13, 9);
            this.rbCurrent.Name = "rbCurrent";
            this.rbCurrent.Size = new System.Drawing.Size(65, 17);
            this.rbCurrent.TabIndex = 12;
            this.rbCurrent.TabStop = true;
            this.rbCurrent.Text = "Current ";
            this.rbCurrent.UseVisualStyleBackColor = true;
            // 
            // btGetAllocation
            // 
            this.btGetAllocation.Location = new System.Drawing.Point(250, 7);
            this.btGetAllocation.Name = "btGetAllocation";
            this.btGetAllocation.Size = new System.Drawing.Size(80, 23);
            this.btGetAllocation.TabIndex = 11;
            this.btGetAllocation.Text = "Get Data";
            this.btGetAllocation.Click += new System.EventHandler(this.btGetAllocation_Click);
            // 
            // tabCtrlAllocation
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance1.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.tabCtrlAllocation.ActiveTabAppearance = appearance1;
            this.tabCtrlAllocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlAllocation.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.tabCtrlAllocation.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabCtrlAllocation.Controls.Add(this.ultraTabPageControl1);
            this.tabCtrlAllocation.Controls.Add(this.ultraTabPageControl2);
            this.tabCtrlAllocation.Location = new System.Drawing.Point(3, 35);
            this.tabCtrlAllocation.Name = "tabCtrlAllocation";
            this.tabCtrlAllocation.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabCtrlAllocation.Size = new System.Drawing.Size(914, 494);
            this.tabCtrlAllocation.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this.tabCtrlAllocation.TabIndex = 0;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(10)))));
            ultraTab1.ActiveAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            ultraTab1.Appearance = appearance3;
            ultraTab1.FixedWidth = 200;
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Fund Allocation";
            this.tabCtrlAllocation.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1});
            this.tabCtrlAllocation.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.VisualStudio2005;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(912, 473);
            // 
            // timerUpdate
            // 
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // helpProviderAllocation
            // 
            this.helpProviderAllocation.HelpNamespace = "Prana Help.chm";
            // 
            // timerGetData
            // 
            this.timerGetData.Tick += new System.EventHandler(this.timerGetData_Tick);
            // 
            // fundUserControl
            // 
            this.fundUserControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.fundUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fundUserControl.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.fundUserControl.FormType = Prana.BusinessObjects.PranaInternalConstants.TYPE_OF_ALLOCATION.FUND;
            this.fundUserControl.IsCurrentDate = true;
            this.fundUserControl.Location = new System.Drawing.Point(0, 0);
            this.fundUserControl.Name = "fundUserControl";
            this.fundUserControl.Read_Write = 0;
            this.fundUserControl.Size = new System.Drawing.Size(912, 473);
            this.fundUserControl.TabIndex = 0;
            // 
            // StrategyUserControl
            // 
            this.StrategyUserControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.StrategyUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StrategyUserControl.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.StrategyUserControl.FormType = Prana.BusinessObjects.PranaInternalConstants.TYPE_OF_ALLOCATION.FUND;
            this.StrategyUserControl.IsCurrentDate = true;
            this.StrategyUserControl.Location = new System.Drawing.Point(0, 0);
            this.StrategyUserControl.Name = "StrategyUserControl";
            this.StrategyUserControl.Read_Write = 0;
            this.StrategyUserControl.Size = new System.Drawing.Size(912, 498);
            this.StrategyUserControl.TabIndex = 0;
            // 
            // _AllocationMain_Toolbars_Dock_Area_Left
            // 
            this._AllocationMain_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationMain_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationMain_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._AllocationMain_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationMain_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 23);
            this._AllocationMain_Toolbars_Dock_Area_Left.Name = "_AllocationMain_Toolbars_Dock_Area_Left";
            this._AllocationMain_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 550);
            this._AllocationMain_Toolbars_Dock_Area_Left.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // ultraToolbarsManager1
            // 
            this.ultraToolbarsManager1.DesignerFlags = 1;
            this.ultraToolbarsManager1.DockWithinContainer = this;
            this.ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.ultraToolbarsManager1.ImageListSmall = this.imgList;
            this.ultraToolbarsManager1.ImageTransparentColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ultraToolbarsManager1.LockToolbars = true;
            this.ultraToolbarsManager1.MenuSettings.IsSideStripVisible = Infragistics.Win.DefaultableBoolean.False;
            this.ultraToolbarsManager1.ShowFullMenusDelay = 500;
            this.ultraToolbarsManager1.ShowShortcutsInToolTips = true;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool8,
            buttonTool10,
            buttonTool12});
            ultraToolbar1.Text = "UltraToolbar1";
            this.ultraToolbarsManager1.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            this.ultraToolbarsManager1.ToolbarSettings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
            appearance29.Image = 0;
            buttonTool13.SharedProps.AppearancesSmall.Appearance = appearance29;
            buttonTool13.SharedProps.Caption = "Preferences";
            buttonTool13.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance30.Image = 2;
            buttonTool14.SharedProps.AppearancesSmall.Appearance = appearance30;
            buttonTool14.SharedProps.Caption = "Report";
            buttonTool14.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance31.Image = "service.png";
            buttonTool15.SharedProps.AppearancesSmall.Appearance = appearance31;
            buttonTool15.SharedProps.Caption = "Commission";
            buttonTool15.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool16.SharedProps.Caption = "ButtonTool1";
            buttonTool17.SharedProps.Caption = "Get Data";
            buttonTool17.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool18.SharedProps.Caption = "Auto Group";
            buttonTool18.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            controlContainerTool1.SharedProps.Caption = "ControlContainerTool1";
            controlContainerTool1.SharedProps.Width = 80;
            controlContainerTool3.SharedProps.Caption = "ControlContainerTool2";
            this.ultraToolbarsManager1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool13,
            buttonTool14,
            buttonTool15,
            buttonTool16,
            buttonTool17,
            buttonTool18,
            controlContainerTool1,
            controlContainerTool3});
            this.ultraToolbarsManager1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.ultraToolbarsManager1.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager1_ToolClick_1);
            // 
            // _AllocationMain_Toolbars_Dock_Area_Right
            // 
            this._AllocationMain_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationMain_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationMain_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._AllocationMain_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationMain_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(914, 23);
            this._AllocationMain_Toolbars_Dock_Area_Right.Name = "_AllocationMain_Toolbars_Dock_Area_Right";
            this._AllocationMain_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 550);
            this._AllocationMain_Toolbars_Dock_Area_Right.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _AllocationMain_Toolbars_Dock_Area_Top
            // 
            this._AllocationMain_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationMain_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationMain_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._AllocationMain_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationMain_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AllocationMain_Toolbars_Dock_Area_Top.Name = "_AllocationMain_Toolbars_Dock_Area_Top";
            this._AllocationMain_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(914, 23);
            this._AllocationMain_Toolbars_Dock_Area_Top.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // _AllocationMain_Toolbars_Dock_Area_Bottom
            // 
            this._AllocationMain_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AllocationMain_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this._AllocationMain_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._AllocationMain_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AllocationMain_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 573);
            this._AllocationMain_Toolbars_Dock_Area_Bottom.Name = "_AllocationMain_Toolbars_Dock_Area_Bottom";
            this._AllocationMain_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(914, 0);
            this._AllocationMain_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.ultraToolbarsManager1;
            // 
            // AllocationMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(914, 573);
            this.Controls.Add(this.AllocationMain_Fill_Panel);
            this.Controls.Add(this._AllocationMain_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._AllocationMain_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._AllocationMain_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._AllocationMain_Toolbars_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.helpProviderAllocation.SetHelpKeyword(this, "LaunchingAllocationModule.html");
            this.helpProviderAllocation.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.Topic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(790, 500);
            this.Name = "AllocationMain";
            this.helpProviderAllocation.SetShowHelp(this, true);
            this.Text = "Allocations";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.AllocationMain_Closing);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.AllocationMain_Fill_Panel.ResumeLayout(false);
            this.AllocationMain_Fill_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerAllocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabCtrlAllocation)).EndInit();
            this.tabCtrlAllocation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraToolbarsManager1)).EndInit();
            this.ResumeLayout(false);

		}

      
		#endregion

		#region private Member
		
		
		private Infragistics.Win.UltraWinTabControl.UltraTabControl tabCtrlAllocation;
		private System.Windows.Forms.ImageList imgList;		
		//private AllocationPreferences  _allocationPreferences;
		private Prana.BusinessObjects.CompanyUser  _loginUser;
		private Prana.Allocation.FundUserControl fundUserControl;
        private Prana.Allocation.FundUserControl StrategyUserControl;
		private System.Windows.Forms.Button btn_AtoGroup;
        //private string FORM_NAME="AllocationMain";
		private bool bBundlingEventNotSet=true;
        private int _read_write = 0;

		#endregion		

		public event EventHandler AllocationClosed;

        public event EventHandler LaunchCommissionCalculation;
        //public event EventHandler CalculateCommission;
        //public DateTime TimeStamp = DateTime.UtcNow;
		#region Constructor And SetUp
		public AllocationMain()
		{
            try
            {
                InitializeComponent();
                UIThreadMarshaller.AddFormForMarshalling(UIThreadMarshaller.ALLOCATION_FORM, this);
                //TimeStamp = DateTime.UtcNow;
                fundUserControl.currentDateTime = DateTime.UtcNow;
                StrategyUserControl.currentDateTime = DateTime.UtcNow;
                rbCurrent.Checked = true;
                dtPickerAllocation.Enabled = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
		
		}
		public void SetUp()
		{
			try
			{
                btGetAllocation.Enabled = false;
                rbHistorical.Enabled = false;
                rbCurrent.Enabled = false;
                BackgroundWorker backGroundWorker = new BackgroundWorker();
                backGroundWorker.DoWork += new DoWorkEventHandler(backGroundWorker_DoWork);
                backGroundWorker.RunWorkerAsync();
                backGroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backGroundWorker_RunWorkerCompleted);
                fundUserControl.Read_Write = _read_write;
                StrategyUserControl.Read_Write = _read_write;

                //if (!dtPickerAllocation.DateTime.Date.Equals(DateTime.Now.Date))
                //{
                //fundUserControl.AUECLocalDate = dtPickerAllocation.DateTime.Date;
                //StrategyUserControl.AUECLocalDate = dtPickerAllocation.DateTime.Date;
                //}
                //else
                //{
                //    fundUserControl.AUECLocalDate = DateTimeConstants.MinValue;
                //    StrategyUserControl.AUECLocalDate = DateTimeConstants.MinValue;
                //}
                fundUserControl.SetUp(_loginUser,PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
                StrategyUserControl.SetUp( _loginUser, PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);

				//in case of AllocationFund Strategy Bundling 
				if( AllocationPreferencesManager.AllocationPreferences.GeneralRules.IntegrateFundAndStrategyBundling)
				{
					//fundUserControl.FundStrategyBundling +=new Prana.Allocation.FundUserControl.FundStrategyBundlingEventDelegate(fundUserControl_FundStrategyBundling);
					bBundlingEventNotSet=false;
				}


                if (AllocationPreferencesManager.AllocationPreferences.GeneralRules.IsActiveTimer)
                {
                    timerUpdate.Start();
                    timerUpdate.Interval = AllocationPreferencesManager.AllocationPreferences.GeneralRules.UpdateInterval;
                }
                else
                {
                    timerUpdate.Start();
                    timerUpdate.Interval = 600000;
                }
				
			}
			catch(Exception ex)
			{
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                //if (rethrow)
                //{
                //    throw;
                //}
			}

		}
        public void UserPermissionSetUp(int read_write)
        {
            try
            {
                _read_write = read_write;
                if (read_write == 1)
                {
                    btn_AtoGroup.Enabled = true;      
                   
                }
                else
                {
                    btn_AtoGroup.Enabled = false;                  
                }
                  

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                //if (rethrow)
                //{
                //    throw;
                //}
            }

        }
       

        void backGroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Db Operation
            string AllAUECDatesString = string.Empty;

            if (rbHistorical.Checked.Equals(false))
            {
                AllAUECDatesString = TimeZoneHelper.GetAllAUECLocalDatesFromUTCStr(DateTime.UtcNow);
            }
            else
            {
                AllAUECDatesString = TimeZoneHelper.GetSameDateForAllAUEC(dtPickerAllocation.DateTime.Date);
            }

            OrderAllocationManager.Initialise(_loginUser, AllAUECDatesString, false);
        }

        void backGroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            StrategyUserControl.BindGrids(false );
            fundUserControl.BindGrids(false);
            btGetAllocation.Enabled = true;
            rbHistorical.Enabled = true;
            rbCurrent.Enabled = true;
        }
	
		#endregion
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
                    fundUserControl.Dispose();
                    StrategyUserControl.Dispose();
                    fundUserControl = null;
                    StrategyUserControl = null;
					components.Dispose();					
				}
			}
			base.Dispose( disposing );
		}
	
		#region Events
        

        /// <summary>
        /// Allocation Form Closing
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllocationMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //System.Text.StringBuilder errorMessage = new System.Text.StringBuilder();
            //errorMessage.AppendLine("Please make sure that you have saved Commission before exiting").AppendLine("Are you sure you want to close Allocation?");
            //if (MessageBox.Show(errorMessage.ToString(),"Confirm Close", MessageBoxButtons.YesNo) == DialogResult.No)
            //{
            //    e.Cancel=true ;

            //}
            //else
            //{
            // Close Alloaction Report Form 
            if (_allocationReport != null)
            {
                _allocationReport.Close();
                _allocationReport = null;
            }

            // Dispose All Components of Tool Manager
            ultraToolbarsManager1.Dispose();
            fundUserControl.InitializeUI = false;
            // Inform Prana Main of Closing
            UIThreadMarshaller.RemoveFormForMarshalling(UIThreadMarshaller.ALLOCATION_FORM);
            if (AllocationClosed != null)
            {
                AllocationClosed(this, e);
            }
        }

        //}
        /// <summary>
        /// On Update Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btn_Update_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        BackgroundWorker backGroundWorkerUpdate = new BackgroundWorker();
        //        backGroundWorkerUpdate.DoWork += new DoWorkEventHandler(backGroundWorkerUpdate_DoWork);
        //        backGroundWorkerUpdate.RunWorkerAsync();
        //        backGroundWorkerUpdate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backGroundWorkerUpdate_RunWorkerCompleted);
        //        //CalculateCommission += new EventHandler(CalculateCommission_UpdatedOrders);
        //        OrderAllocationManager.SettimeStamp(TimeStamp);
        //        TimeStamp = DateTime.UtcNow;
               
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }

        //    }
        //}
        //void CalculateCommission_UpdatedOrders(object sender,  e)
        //{
           
        //}
        //void backGroundWorkerUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (!e.Cancelled)
        //    {
        //        //AllocationOrderCollection updatedOrders = (AllocationOrderCollection)e.Result;
        //        //if (updatedOrders.Count > 0)
        //        //{
        //        //    OrderFundAllocationManager.GetInstance.CheckUpdatedFundOrdersLocation(updatedOrders);
        //        //    // For Getting Strategy Orders Clone Fund Orders Received from dataBase
        //        //    OrderStrategyAllocationManager.GetInstance.CheckUpdatedStrategyOrdersLocation(updatedOrders.Clone());
        //        //}
        //        fundUserControl.UpdateDataInGrids(null);
        //        StrategyUserControl.UpdateDataInGrids(null);
        //    }
        //}

        //void backGroundWorkerUpdate_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        //AllocationOrderCollection updatedOrders = 
        //         OrderAllocationManager.UpdateAllocation(DateTime.Now.ToUniversalTime());
        //        //e.Result = updatedOrders;
        //    }
        //    catch (Exception ex)
        //    {
        //        e.Cancel=true ;
        //        MessageBox.Show("Problem while Updating Orders . Exception ="+ex.Message );
                
        //    }
        //}
        /// <summary>
/// Auto Group of Orders
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
		private void btn_AtoGroup_Click(object sender, System.EventArgs e)
		{
            if (tabCtrlAllocation.SelectedTab.Index == 0)
            {
                
                fundUserControl.AutoGroup();
            }
            else
            {
                if (!AllocationPreferencesManager.AllocationPreferences.GeneralRules.IntegrateFundAndStrategyBundling)
                {
                    StrategyUserControl.AutoGroup();
                }
                else
                {
                    MessageBox.Show("User has selected for FundStrategy Bundling ");
                }
            }

		
		}
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (AllocationPreferencesManager.AllocationPreferences.GeneralRules.IsActiveTimer)
            {
                timerUpdate.Interval = AllocationPreferencesManager.AllocationPreferences.GeneralRules.UpdateInterval;
                SetUp();
                //btn_Update_Click(null, null);
            }
        }
        /// <summary>
        /// Toolbar on top of allocation Form ... 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraToolbarsManager1_ToolClick_1(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            if (e.Tool.Key.Equals("preferences", StringComparison.OrdinalIgnoreCase))
            {
                fundUserControl.SetDefaultValuesToDefaultCombo();
                StrategyUserControl.SetDefaultValuesToDefaultCombo();
                if (LaunchPreferences != null)
                    LaunchPreferences(sender, e);
            }
            if (e.Tool.Key.Equals("report", StringComparison.OrdinalIgnoreCase))
            {
                _allocationReport = AllocationReport.GetInstance;
                WinUtilities.SetForegroundWindow(_allocationReport.Handle);
                if(!_allocationReport.Visible)
                _allocationReport.Show();
            }
            if (e.Tool.Key.Equals("commission", StringComparison.OrdinalIgnoreCase))
            {
                if (LaunchCommissionCalculation != null)
                {
                   
                     LaunchCommissionCalculation(this,e);
                }
            }
        }

		#endregion	

		#region Preferences
		public void UpdatePreferences(AllocationPreferences   prefs)
		{
		
			
			try
			{
				
				if(fundUserControl ==null)
					return ;
				if(StrategyUserControl ==null)
					return ;

                fundUserControl.SetAllocationPreferences(prefs);
                StrategyUserControl.SetAllocationPreferences(prefs);

                MethodInvoker methodInvoker = new MethodInvoker(fundUserControl.UpdatePreferences);
                methodInvoker.BeginInvoke(null, null);
                MethodInvoker methodInvoker1 = new MethodInvoker(StrategyUserControl.UpdatePreferences);
                methodInvoker1.BeginInvoke(null, null);
               
               
                
              
			}
			catch(Exception ex)
			{
//				string formattedInfo = ex.Message + "\n Stack Trace:" + ex.StackTrace.ToString() + "\n Inner Exception:"  + ex.InnerException;
//				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
//					FORM_NAME);
//				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
//				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
				throw ex;

			}
			
		
		}
		
		#region IAllocation Members
		public void ApplyPreferences(string moduleName, IPreferenceData prefs)
		{
			try
			{
				if(!moduleName.Equals(ApplicationConstants.ALLOCATION_MODULE)) return;
			{
                AllocationPreferences allocationPreferences = (AllocationPreferences)prefs;
                if (allocationPreferences.GeneralRules.IntegrateFundAndStrategyBundling)
				{
					if(bBundlingEventNotSet)
					{
						if(fundUserControl !=null)
						{
							//fundUserControl.FundStrategyBundling +=new Prana.Allocation.FundUserControl.FundStrategyBundlingEventDelegate(fundUserControl_FundStrategyBundling);
							bBundlingEventNotSet=false;
						}
						else
						{
							return;
						}
					}
				}
                UpdatePreferences(allocationPreferences);	
					
			}
			}
			catch(Exception ex)
			{
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
			}
			
		}

		public event System.EventHandler LaunchPreferences;

	
		#endregion
		#endregion
		
		#region Properties
		public Prana.BusinessObjects.CompanyUser loginUser
		{
			get{return _loginUser;}
			set{_loginUser=value ;}
		
		}

        private DateTime  _selectedDate = DateTime.UtcNow;

       public DateTime  SelectedDate
        {
            get { return _selectedDate; }
            
        }

        private bool _isCurrentDate = true;

        public bool IsCurrentDate
        {
            get { return _isCurrentDate; }
            set { _isCurrentDate = value; }
        }
        
		
		public Form Reference()
		{
			return this;
		}
		
		#endregion

        //private void btGetAllocation_Click(object sender, EventArgs e)
        //{
           
        //    //fundUserControl.GetDataForDate(PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
        //    //fundUserControl.GetDataForDate(PranaInternalConstants.TYPE_OF_ALLOCATION.STRATEGY);
        //    //BackgroundWorker backGroundWorker = new BackgroundWorker();
        //    //backGroundWorker.DoWork += new DoWorkEventHandler(backGroundWorker_DoWork);
        //    //backGroundWorker.RunWorkerAsync();
           
        //    //backGroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backGroundWorker_RunWorkerCompleted);
        //    //fundUserControl.GetDataForDate();
        //    //StrategyUserControl.GetDataForDate();
        //    //fundUserControl.ClearOrderCollection();
        //    //OrderFundAllocationManager.GetInstance.Orders.Clear();
        //    SetUp();
           
            
        //   //OrderFundAllocationManager.GetInstance.Orders.Clear();
           
        //    //fundUserControl.SetUp(_loginUser, PranaInternalConstants.TYPE_OF_ALLOCATION.FUND);
        //}

        private void dtPickerAllocationMain_ValueChanged(object sender, EventArgs e)
        {
                timerGetData.Stop();
                timerGetData.Interval = 1000;
                timerGetData.Start();

                //raise a flag to fetch data again.  

                fundUserControl.currentDateTime = dtPickerAllocation.DateTime.Date;
                StrategyUserControl.currentDateTime = dtPickerAllocation.DateTime.Date;

                //if (!dtPickerAllocationMain.DateTime.Date.Equals(DateTime.Now.Date))
                //{
                fundUserControl.AUECLocalDate = dtPickerAllocation.DateTime.Date;
                StrategyUserControl.AUECLocalDate = dtPickerAllocation.DateTime.Date;
                _selectedDate = dtPickerAllocation.DateTime.Date;

                //}
                //else
                //{
                //    fundUserControl.AUECLocalDate = DateTimeConstants.MinValue;
                //    StrategyUserControl.AUECLocalDate = DateTimeConstants.MinValue;
                //}
          }
      
        void timerGetData_Tick(object sender, EventArgs e)
        {
            //raise a flag not to fetch the data again - once this method has been called. 
            timerGetData.Stop();

            SetUp();
          
        }

        private void btGetAllocation_Click(object sender, EventArgs e)
        {
           
            SetUp();
            
        }

        //private void rbCurrent_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (rbCurrent.Checked.Equals(true))
        //    {
        //        dtPickerAllocation.Enabled = false;
        //        fundUserControl.AUECLocalDate = DateTimeConstants.MinValue;
        //        StrategyUserControl.AUECLocalDate = DateTimeConstants.MinValue;
                
        //    }
        //    else
        //    {
        //        dtPickerAllocation.Enabled = true;
        //    }
        //}

        private void rbHistorical_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHistorical.Checked.Equals(true))
            {
                fundUserControl.AUECLocalDate = dtPickerAllocation.DateTime.Date;
                StrategyUserControl.AUECLocalDate = dtPickerAllocation.DateTime.Date;
                SetUp();
            }
            else
            {
                dtPickerAllocation.DateTime = dtPickerAllocation.MaxDate;
                fundUserControl.AUECLocalDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;
                StrategyUserControl.AUECLocalDate = Prana.Utilities.DateTimeUtilities.DateTimeConstants.MinValue;
                SetUp();
            }
            _selectedDate = dtPickerAllocation.DateTime.Date;
            dtPickerAllocation.Enabled = rbHistorical.Checked;
            fundUserControl.IsCurrentDate = !rbHistorical.Checked;
            StrategyUserControl.IsCurrentDate = !rbHistorical.Checked;
            IsCurrentDate = !rbHistorical.Checked;
        }
    }
}
