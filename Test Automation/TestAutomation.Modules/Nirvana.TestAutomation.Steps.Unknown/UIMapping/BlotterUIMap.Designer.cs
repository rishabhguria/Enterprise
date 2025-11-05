using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Unknown
{
    partial class BlotterUIMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlotterUIMap));
            this.PranaApplication = new TestAutomationFX.UI.UIApplication();
            this.PranaMain = new TestAutomationFX.UI.UIWindow();
            this.BlotterMain = new TestAutomationFX.UI.UIWindow();
            this.UltraPanel1 = new TestAutomationFX.UI.UIWindow();
            this.UltraPanel1ClientArea = new TestAutomationFX.UI.UIWindow();
            this.CtrlBlotterMain = new TestAutomationFX.UI.UIWindow();
            this.BlotterTabControl = new TestAutomationFX.UI.UIWindow();
            this.OrdersTabPageControl = new TestAutomationFX.UI.UIWindow();
            this.OrderBlotterGrid = new TestAutomationFX.UI.UIWindow();
            this.DgBlotter = new TestAutomationFX.UI.UIWindow();
            this.UltraPanelSubOrdersBlotter = new TestAutomationFX.UI.UIWindow();
            this.UltraPanelSubOrdersBlotterClientArea = new TestAutomationFX.UI.UIWindow();
            this.SubOrderBlotterGrid = new TestAutomationFX.UI.UIWindow();
            this.DgBlotter1 = new TestAutomationFX.UI.UIWindow();
            this.Orders = new TestAutomationFX.UI.UIMsaa();
            this.WorkingSubs = new TestAutomationFX.UI.UIMsaa();
            this.WorkingSubsTabPageControl = new TestAutomationFX.UI.UIWindow();
            this.WorkingSubBlotterGrid = new TestAutomationFX.UI.UIWindow();
            this.DgBlotter2 = new TestAutomationFX.UI.UIWindow();
            this.PopupMenuContext = new TestAutomationFX.UI.UIWindow();
            this.BlotterMain_UltraFormManager_Dock_Area_Top = new TestAutomationFX.UI.UIWindow();
            this.NirvanaBlotter = new TestAutomationFX.UI.UIWindow();
            this.ButtonYes = new TestAutomationFX.UI.UIWindow();
            this.Warning = new TestAutomationFX.UI.UIWindow();
            this.ButtonOK = new TestAutomationFX.UI.UIWindow();
            this.Summary = new TestAutomationFX.UI.UIMsaa();
            this.SummaryTabPageControl = new TestAutomationFX.UI.UIWindow();
            this.SummaryBlotterGrid = new TestAutomationFX.UI.UIWindow();
            this.DgBlotter3 = new TestAutomationFX.UI.UIWindow();
            this.ButtonNo = new TestAutomationFX.UI.UIWindow();
            this.ultraPanel11 = new TestAutomationFX.UI.UIWindow();
            this.PopupMenuTransfertoUser = new TestAutomationFX.UI.UIWindow();
            this.TransfertoUser = new TestAutomationFX.UI.UIMenuItem();
            this.PranaMainWindow = new TestAutomationFX.UI.UIMainWindow();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PranaApplication
            // 
            this.PranaApplication.Comment = null;
            this.PranaApplication.ImagePath = ApplicationArguments.ClientReleasePath + "\\Prana.exe";
            this.PranaApplication.Name = "PranaApplication";
            this.PranaApplication.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PranaApplication.ObjectImage")));
            this.PranaApplication.Parent = null;
            this.PranaApplication.ProcessName = "Prana";
            this.PranaApplication.TimeOut = 1000;
            this.PranaApplication.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Application;
            this.PranaApplication.UseCoordinatesOnClick = false;
            this.PranaApplication.UsedMatchedProperties = ((TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties)((TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties.ProcessName | TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties.CommandLineArguments)));
            this.PranaApplication.WorkingDirectory = ApplicationArguments.ClientReleasePath;
            // 
            // PranaMain
            // 
            this.PranaMain.Comment = null;
            this.PranaMain.InstanceName = "PranaMain";
            this.PranaMain.MatchedIndex = 0;
            this.PranaMain.MsaaName = "Nirvana";
            this.PranaMain.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.PranaMain.Name = "PranaMain";
            this.PranaMain.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PranaMain.ObjectImage")));
            this.PranaMain.OwnedWindow = true;
            this.PranaMain.Parent = this.PranaApplication;
            this.PranaMain.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PranaMain.UseCoordinatesOnClick = true;
            this.PranaMain.WindowClass = "";
            // 
            // BlotterMain
            // 
            this.BlotterMain.Comment = null;
            this.BlotterMain.InstanceName = "BlotterMain";
            this.BlotterMain.MatchedIndex = 0;
            this.BlotterMain.MsaaName = "Blotter";
            this.BlotterMain.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.BlotterMain.Name = "BlotterMain";
            this.BlotterMain.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BlotterMain.ObjectImage")));
            this.BlotterMain.OwnedWindow = true;
            this.BlotterMain.Parent = this.PranaMain;
            this.BlotterMain.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.BlotterMain.UseCoordinatesOnClick = true;
            this.BlotterMain.WindowClass = "";
            this.BlotterMain.WindowText = "Blotter";
            // 
            // UltraPanel1
            // 
            this.UltraPanel1.Comment = null;
            this.UltraPanel1.Index = 0;
            this.UltraPanel1.InstanceName = "ultraPanel1";
            this.UltraPanel1.MatchedIndex = 0;
            this.UltraPanel1.MsaaName = null;
            this.UltraPanel1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanel1.Name = "UltraPanel1";
            this.UltraPanel1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanel1.ObjectImage")));
            this.UltraPanel1.Parent = this.BlotterMain;
            this.UltraPanel1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraPanel1.UseCoordinatesOnClick = true;
            this.UltraPanel1.WindowClass = "";
            // 
            // UltraPanel1ClientArea
            // 
            this.UltraPanel1ClientArea.Comment = null;
            this.UltraPanel1ClientArea.Index = 0;
            this.UltraPanel1ClientArea.InstanceName = "ultraPanel1.ClientArea";
            this.UltraPanel1ClientArea.MatchedIndex = 0;
            this.UltraPanel1ClientArea.MsaaName = null;
            this.UltraPanel1ClientArea.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanel1ClientArea.Name = "UltraPanel1ClientArea";
            this.UltraPanel1ClientArea.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanel1ClientArea.ObjectImage")));
            this.UltraPanel1ClientArea.Parent = this.UltraPanel1;
            this.UltraPanel1ClientArea.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraPanel1ClientArea.UseCoordinatesOnClick = true;
            this.UltraPanel1ClientArea.WindowClass = "";
            // 
            // CtrlBlotterMain
            // 
            this.CtrlBlotterMain.Comment = null;
            this.CtrlBlotterMain.Index = 0;
            this.CtrlBlotterMain.InstanceName = "ctrlBlotterMain";
            this.CtrlBlotterMain.MatchedIndex = 0;
            this.CtrlBlotterMain.MsaaName = null;
            this.CtrlBlotterMain.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.CtrlBlotterMain.Name = "CtrlBlotterMain";
            this.CtrlBlotterMain.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("CtrlBlotterMain.ObjectImage")));
            this.CtrlBlotterMain.Parent = this.UltraPanel1ClientArea;
            this.CtrlBlotterMain.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CtrlBlotterMain.UseCoordinatesOnClick = true;
            this.CtrlBlotterMain.WindowClass = "";
            // 
            // BlotterTabControl
            // 
            this.BlotterTabControl.Comment = null;
            this.BlotterTabControl.Index = 0;
            this.BlotterTabControl.InstanceName = "BlotterTabControl";
            this.BlotterTabControl.MatchedIndex = 0;
            this.BlotterTabControl.MsaaName = null;
            this.BlotterTabControl.MsaaRole = System.Windows.Forms.AccessibleRole.PageTabList;
            this.BlotterTabControl.Name = "BlotterTabControl";
            this.BlotterTabControl.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BlotterTabControl.ObjectImage")));
            this.BlotterTabControl.Parent = this.CtrlBlotterMain;
            this.BlotterTabControl.UIObjectType = TestAutomationFX.UI.UIObjectTypes.TabControl;
            this.BlotterTabControl.UseCoordinatesOnClick = true;
            this.BlotterTabControl.WindowClass = "";
            // 
            // OrdersTabPageControl
            // 
            this.OrdersTabPageControl.Comment = null;
            this.OrdersTabPageControl.Index = 1;
            this.OrdersTabPageControl.InstanceName = "OrdersTabPageControl";
            this.OrdersTabPageControl.MatchedIndex = 0;
            this.OrdersTabPageControl.MsaaName = null;
            this.OrdersTabPageControl.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.OrdersTabPageControl.Name = "OrdersTabPageControl";
            this.OrdersTabPageControl.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("OrdersTabPageControl.ObjectImage")));
            this.OrdersTabPageControl.Parent = this.BlotterTabControl;
            this.OrdersTabPageControl.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.OrdersTabPageControl.UseCoordinatesOnClick = true;
            this.OrdersTabPageControl.WindowClass = "";
            // 
            // OrderBlotterGrid
            // 
            this.OrderBlotterGrid.Comment = null;
            this.OrderBlotterGrid.Index = 0;
            this.OrderBlotterGrid.InstanceName = "OrderBlotterGrid";
            this.OrderBlotterGrid.MatchedIndex = 0;
            this.OrderBlotterGrid.MsaaName = null;
            this.OrderBlotterGrid.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.OrderBlotterGrid.Name = "OrderBlotterGrid";
            this.OrderBlotterGrid.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("OrderBlotterGrid.ObjectImage")));
            this.OrderBlotterGrid.Parent = this.OrdersTabPageControl;
            this.OrderBlotterGrid.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.OrderBlotterGrid.UseCoordinatesOnClick = true;
            this.OrderBlotterGrid.WindowClass = "";
            // 
            // DgBlotter
            // 
            this.DgBlotter.Comment = null;
            this.DgBlotter.Index = 0;
            this.DgBlotter.InstanceName = "dgBlotter";
            this.DgBlotter.MatchedIndex = 0;
            this.DgBlotter.MsaaName = "ultraGrid1";
            this.DgBlotter.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.DgBlotter.Name = "DgBlotter";
            this.DgBlotter.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("DgBlotter.ObjectImage")));
            this.DgBlotter.Parent = this.OrderBlotterGrid;
            this.DgBlotter.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.DgBlotter.UseCoordinatesOnClick = true;
            this.DgBlotter.WindowClass = "";
            this.DgBlotter.WindowText = "ultraGrid1";
            // 
            // UltraPanelSubOrdersBlotter
            // 
            this.UltraPanelSubOrdersBlotter.Comment = null;
            this.UltraPanelSubOrdersBlotter.Index = 2;
            this.UltraPanelSubOrdersBlotter.InstanceName = "ultraPanelSubOrdersBlotter";
            this.UltraPanelSubOrdersBlotter.MatchedIndex = 0;
            this.UltraPanelSubOrdersBlotter.MsaaName = "Collapse All";
            this.UltraPanelSubOrdersBlotter.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanelSubOrdersBlotter.Name = "UltraPanelSubOrdersBlotter";
            this.UltraPanelSubOrdersBlotter.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanelSubOrdersBlotter.ObjectImage")));
            this.UltraPanelSubOrdersBlotter.Parent = this.OrderBlotterGrid;
            this.UltraPanelSubOrdersBlotter.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraPanelSubOrdersBlotter.UseCoordinatesOnClick = true;
            this.UltraPanelSubOrdersBlotter.WindowClass = "";
            // 
            // UltraPanelSubOrdersBlotterClientArea
            // 
            this.UltraPanelSubOrdersBlotterClientArea.Comment = null;
            this.UltraPanelSubOrdersBlotterClientArea.Index = 0;
            this.UltraPanelSubOrdersBlotterClientArea.InstanceName = "ultraPanelSubOrdersBlotter.ClientArea";
            this.UltraPanelSubOrdersBlotterClientArea.MatchedIndex = 0;
            this.UltraPanelSubOrdersBlotterClientArea.MsaaName = null;
            this.UltraPanelSubOrdersBlotterClientArea.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanelSubOrdersBlotterClientArea.Name = "UltraPanelSubOrdersBlotterClientArea";
            this.UltraPanelSubOrdersBlotterClientArea.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanelSubOrdersBlotterClientArea.ObjectImage")));
            this.UltraPanelSubOrdersBlotterClientArea.Parent = this.UltraPanelSubOrdersBlotter;
            this.UltraPanelSubOrdersBlotterClientArea.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraPanelSubOrdersBlotterClientArea.UseCoordinatesOnClick = true;
            this.UltraPanelSubOrdersBlotterClientArea.WindowClass = "";
            // 
            // SubOrderBlotterGrid
            // 
            this.SubOrderBlotterGrid.Comment = null;
            this.SubOrderBlotterGrid.Index = 0;
            this.SubOrderBlotterGrid.InstanceName = "SubOrderBlotterGrid";
            this.SubOrderBlotterGrid.MatchedIndex = 0;
            this.SubOrderBlotterGrid.MsaaName = null;
            this.SubOrderBlotterGrid.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.SubOrderBlotterGrid.Name = "SubOrderBlotterGrid";
            this.SubOrderBlotterGrid.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("SubOrderBlotterGrid.ObjectImage")));
            this.SubOrderBlotterGrid.Parent = this.UltraPanelSubOrdersBlotterClientArea;
            this.SubOrderBlotterGrid.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.SubOrderBlotterGrid.UseCoordinatesOnClick = true;
            this.SubOrderBlotterGrid.WindowClass = "";
            // 
            // DgBlotter1
            // 
            this.DgBlotter1.Comment = null;
            this.DgBlotter1.Index = 0;
            this.DgBlotter1.InstanceName = "dgBlotter";
            this.DgBlotter1.MatchedIndex = 0;
            this.DgBlotter1.MsaaName = "ultraGrid1";
            this.DgBlotter1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.DgBlotter1.Name = "DgBlotter1";
            this.DgBlotter1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("DgBlotter1.ObjectImage")));
            this.DgBlotter1.Parent = this.SubOrderBlotterGrid;
            this.DgBlotter1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.DgBlotter1.UseCoordinatesOnClick = true;
            this.DgBlotter1.WindowClass = "";
            this.DgBlotter1.WindowText = "ultraGrid1";
            // 
            // Orders
            // 
            this.Orders.Comment = null;
            this.Orders.MsaaName = "Orders";
            this.Orders.Name = "Orders";
            this.Orders.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Orders.ObjectImage")));
            this.Orders.Parent = this.BlotterTabControl;
            this.Orders.Role = System.Windows.Forms.AccessibleRole.PageTab;
            this.Orders.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.Orders.UseCoordinatesOnClick = true;
            // 
            // WorkingSubs
            // 
            this.WorkingSubs.Comment = null;
            this.WorkingSubs.MsaaName = "Working Subs";
            this.WorkingSubs.Name = "WorkingSubs";
            this.WorkingSubs.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("WorkingSubs.ObjectImage")));
            this.WorkingSubs.Parent = this.BlotterTabControl;
            this.WorkingSubs.Role = System.Windows.Forms.AccessibleRole.PageTab;
            this.WorkingSubs.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.WorkingSubs.UseCoordinatesOnClick = true;
            // 
            // WorkingSubsTabPageControl
            // 
            this.WorkingSubsTabPageControl.Comment = null;
            this.WorkingSubsTabPageControl.Index = 2;
            this.WorkingSubsTabPageControl.InstanceName = "WorkingSubsTabPageControl";
            this.WorkingSubsTabPageControl.MatchedIndex = 0;
            this.WorkingSubsTabPageControl.MsaaName = null;
            this.WorkingSubsTabPageControl.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.WorkingSubsTabPageControl.Name = "WorkingSubsTabPageControl";
            this.WorkingSubsTabPageControl.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("WorkingSubsTabPageControl.ObjectImage")));
            this.WorkingSubsTabPageControl.Parent = this.BlotterTabControl;
            this.WorkingSubsTabPageControl.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.WorkingSubsTabPageControl.UseCoordinatesOnClick = true;
            this.WorkingSubsTabPageControl.WindowClass = "";
            // 
            // WorkingSubBlotterGrid
            // 
            this.WorkingSubBlotterGrid.Comment = null;
            this.WorkingSubBlotterGrid.Index = 0;
            this.WorkingSubBlotterGrid.InstanceName = "WorkingSubBlotterGrid";
            this.WorkingSubBlotterGrid.MatchedIndex = 0;
            this.WorkingSubBlotterGrid.MsaaName = null;
            this.WorkingSubBlotterGrid.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.WorkingSubBlotterGrid.Name = "WorkingSubBlotterGrid";
            this.WorkingSubBlotterGrid.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("WorkingSubBlotterGrid.ObjectImage")));
            this.WorkingSubBlotterGrid.Parent = this.WorkingSubsTabPageControl;
            this.WorkingSubBlotterGrid.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.WorkingSubBlotterGrid.UseCoordinatesOnClick = true;
            this.WorkingSubBlotterGrid.WindowClass = "";
            // 
            // DgBlotter2
            // 
            this.DgBlotter2.Comment = null;
            this.DgBlotter2.Index = 0;
            this.DgBlotter2.InstanceName = "dgBlotter";
            this.DgBlotter2.MatchedIndex = 0;
            this.DgBlotter2.MsaaName = "ultraGrid1";
            this.DgBlotter2.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.DgBlotter2.Name = "DgBlotter2";
            this.DgBlotter2.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("DgBlotter2.ObjectImage")));
            this.DgBlotter2.Parent = this.WorkingSubBlotterGrid;
            this.DgBlotter2.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.DgBlotter2.UseCoordinatesOnClick = true;
            this.DgBlotter2.WindowClass = "";
            this.DgBlotter2.WindowText = "ultraGrid1";
            // 
            // PopupMenuContext
            // 
            this.PopupMenuContext.Comment = null;
            this.PopupMenuContext.MatchedIndex = 0;
            this.PopupMenuContext.MsaaName = "Context";
            this.PopupMenuContext.MsaaRole = System.Windows.Forms.AccessibleRole.MenuPopup;
            this.PopupMenuContext.Name = "PopupMenuContext";
            this.PopupMenuContext.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PopupMenuContext.ObjectImage")));
            this.PopupMenuContext.OwnedWindow = true;
            this.PopupMenuContext.Parent = this.PranaApplication;
            this.PopupMenuContext.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PopupMenuContext.UseCoordinatesOnClick = true;
            this.PopupMenuContext.WindowClass = "#32768";
            // 
            // BlotterMain_UltraFormManager_Dock_Area_Top
            // 
            this.BlotterMain_UltraFormManager_Dock_Area_Top.Comment = null;
            this.BlotterMain_UltraFormManager_Dock_Area_Top.Index = 3;
            this.BlotterMain_UltraFormManager_Dock_Area_Top.InstanceName = "_BlotterMain_UltraFormManager_Dock_Area_Top";
            this.BlotterMain_UltraFormManager_Dock_Area_Top.MatchedIndex = 0;
            this.BlotterMain_UltraFormManager_Dock_Area_Top.MsaaName = "DockTop";
            this.BlotterMain_UltraFormManager_Dock_Area_Top.MsaaRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.BlotterMain_UltraFormManager_Dock_Area_Top.Name = "BlotterMain_UltraFormManager_Dock_Area_Top";
            this.BlotterMain_UltraFormManager_Dock_Area_Top.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BlotterMain_UltraFormManager_Dock_Area_Top.ObjectImage")));
            this.BlotterMain_UltraFormManager_Dock_Area_Top.Parent = this.BlotterMain;
            this.BlotterMain_UltraFormManager_Dock_Area_Top.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.BlotterMain_UltraFormManager_Dock_Area_Top.UseCoordinatesOnClick = true;
            this.BlotterMain_UltraFormManager_Dock_Area_Top.WindowClass = "";
            // 
            // NirvanaBlotter
            // 
            this.NirvanaBlotter.Comment = null;
            this.NirvanaBlotter.Index = 0;
            this.NirvanaBlotter.IsOptional = true;
            this.NirvanaBlotter.MatchedIndex = 0;
            this.NirvanaBlotter.MsaaName = "Nirvana Blotter";
            this.NirvanaBlotter.MsaaRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.NirvanaBlotter.Name = "NirvanaBlotter";
            this.NirvanaBlotter.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("NirvanaBlotter.ObjectImage")));
            this.NirvanaBlotter.OwnedWindow = true;
            this.NirvanaBlotter.Parent = this.BlotterMain;
            this.NirvanaBlotter.TimeOut = 3000;
            this.NirvanaBlotter.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.NirvanaBlotter.UseCoordinatesOnClick = true;
            this.NirvanaBlotter.WindowClass = "#32770";
            this.NirvanaBlotter.WindowText = "Nirvana Blotter";
            // 
            // ButtonYes
            // 
            this.ButtonYes.Comment = null;
            this.ButtonYes.Index = 0;
            this.ButtonYes.MatchedIndex = 0;
            this.ButtonYes.MsaaName = "Yes";
            this.ButtonYes.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ButtonYes.Name = "ButtonYes";
            this.ButtonYes.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("ButtonYes.ObjectImage")));
            this.ButtonYes.Parent = this.NirvanaBlotter;
            this.ButtonYes.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.ButtonYes.UseCoordinatesOnClick = false;
            this.ButtonYes.WindowClass = "Button";
            this.ButtonYes.WindowText = "&Yes";
            // 
            // Warning
            // 
            this.Warning.Comment = null;
            this.Warning.Index = 0;
            this.Warning.IsOptional = true;
            this.Warning.MatchedIndex = 0;
            this.Warning.MsaaName = "Warning!";
            this.Warning.MsaaRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.Warning.Name = "Warning";
            this.Warning.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Warning.ObjectImage")));
            this.Warning.OwnedWindow = true;
            this.Warning.Parent = this.BlotterMain;
            this.Warning.TimeOut = 3000;
            this.Warning.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.Warning.UseCoordinatesOnClick = true;
            this.Warning.WindowClass = "#32770";
            this.Warning.WindowText = "Warning!";
            // 
            // ButtonOK
            // 
            this.ButtonOK.Comment = null;
            this.ButtonOK.Index = 0;
            this.ButtonOK.IsOptional = true;
            this.ButtonOK.TimeOut = 3500;
            this.ButtonOK.MatchedIndex = 0;
            this.ButtonOK.MsaaName = "OK";
            this.ButtonOK.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("ButtonOK.ObjectImage")));
            this.ButtonOK.Parent = this.Warning;
            this.ButtonOK.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.ButtonOK.UseCoordinatesOnClick = false;
            this.ButtonOK.WindowClass = "Button";
            this.ButtonOK.WindowText = "OK";
            // 
            // Summary
            // 
            this.Summary.Comment = null;
            this.Summary.MsaaName = "Summary";
            this.Summary.Name = "Summary";
            this.Summary.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Summary.ObjectImage")));
            this.Summary.Parent = this.BlotterTabControl;
            this.Summary.Role = System.Windows.Forms.AccessibleRole.PageTab;
            this.Summary.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.Summary.UseCoordinatesOnClick = true;
            // 
            // SummaryTabPageControl
            // 
            this.SummaryTabPageControl.Comment = null;
            this.SummaryTabPageControl.Index = 3;
            this.SummaryTabPageControl.InstanceName = "SummaryTabPageControl";
            this.SummaryTabPageControl.MatchedIndex = 0;
            this.SummaryTabPageControl.MsaaName = null;
            this.SummaryTabPageControl.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.SummaryTabPageControl.Name = "SummaryTabPageControl";
            this.SummaryTabPageControl.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("SummaryTabPageControl.ObjectImage")));
            this.SummaryTabPageControl.Parent = this.BlotterTabControl;
            this.SummaryTabPageControl.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.SummaryTabPageControl.UseCoordinatesOnClick = true;
            this.SummaryTabPageControl.WindowClass = "";
            // 
            // SummaryBlotterGrid
            // 
            this.SummaryBlotterGrid.Comment = null;
            this.SummaryBlotterGrid.Index = 0;
            this.SummaryBlotterGrid.InstanceName = "SummaryBlotterGrid";
            this.SummaryBlotterGrid.MatchedIndex = 0;
            this.SummaryBlotterGrid.MsaaName = null;
            this.SummaryBlotterGrid.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.SummaryBlotterGrid.Name = "SummaryBlotterGrid";
            this.SummaryBlotterGrid.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("SummaryBlotterGrid.ObjectImage")));
            this.SummaryBlotterGrid.Parent = this.SummaryTabPageControl;
            this.SummaryBlotterGrid.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.SummaryBlotterGrid.UseCoordinatesOnClick = true;
            this.SummaryBlotterGrid.WindowClass = "";
            // 
            // DgBlotter3
            // 
            this.DgBlotter3.Comment = null;
            this.DgBlotter3.Index = 1;
            this.DgBlotter3.InstanceName = "dgBlotter";
            this.DgBlotter3.MatchedIndex = 0;
            this.DgBlotter3.MsaaName = "ultraGrid1";
            this.DgBlotter3.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.DgBlotter3.Name = "DgBlotter3";
            this.DgBlotter3.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("DgBlotter3.ObjectImage")));
            this.DgBlotter3.Parent = this.SummaryBlotterGrid;
            this.DgBlotter3.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.DgBlotter3.UseCoordinatesOnClick = true;
            this.DgBlotter3.WindowClass = "";
            this.DgBlotter3.WindowText = "ultraGrid1";
            // 
            // ButtonNo
            // 
            this.ButtonNo.Comment = null;
            this.ButtonNo.Index = 1;
            this.ButtonNo.MatchedIndex = 0;
            this.ButtonNo.MsaaName = "No";
            this.ButtonNo.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ButtonNo.Name = "ButtonNo";
            this.ButtonNo.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("ButtonNo.ObjectImage")));
            this.ButtonNo.Parent = this.Warning;
            this.ButtonNo.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.ButtonNo.UseCoordinatesOnClick = false;
            this.ButtonNo.WindowClass = "Button";
            this.ButtonNo.WindowText = "&No";
            // 
            // ultraPanel11
            // 
            this.ultraPanel11.Comment = null;
            this.ultraPanel11.Name = "ultraPanel11";
            this.ultraPanel11.ObjectImage = null;
            this.ultraPanel11.Parent = this.BlotterMain;
            this.ultraPanel11.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.ultraPanel11.UseCoordinatesOnClick = true;
            this.ultraPanel11.WindowClass = "";
            // 
            // PopupMenuTransfertoUser
            // 
            this.PopupMenuTransfertoUser.Comment = null;
            this.PopupMenuTransfertoUser.Index = 0;
            this.PopupMenuTransfertoUser.MatchedIndex = 0;
            this.PopupMenuTransfertoUser.MsaaName = "Transfer to User";
            this.PopupMenuTransfertoUser.MsaaRole = System.Windows.Forms.AccessibleRole.MenuPopup;
            this.PopupMenuTransfertoUser.Name = "PopupMenuTransfertoUser";
            this.PopupMenuTransfertoUser.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PopupMenuTransfertoUser.ObjectImage")));
            this.PopupMenuTransfertoUser.OwnedWindow = true;
            this.PopupMenuTransfertoUser.Parent = this.BlotterMain;
            this.PopupMenuTransfertoUser.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PopupMenuTransfertoUser.UseCoordinatesOnClick = true;
            this.PopupMenuTransfertoUser.WindowClass = "#32768";
            // 
            // TransfertoUser
            // 
            this.TransfertoUser.Comment = null;
            this.TransfertoUser.MsaaName = "Transfer to User";
            this.TransfertoUser.Name = "TransfertoUser";
            this.TransfertoUser.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("TransfertoUser.ObjectImage")));
            this.TransfertoUser.Parent = this.PopupMenuContext;
            this.TransfertoUser.Role = System.Windows.Forms.AccessibleRole.MenuItem;
            this.TransfertoUser.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MenuItem;
            this.TransfertoUser.UseCoordinatesOnClick = false;
            // 
            // PranaMainWindow
            // 
            this.PranaMainWindow.Comment = null;
            this.PranaMainWindow.Name = "PranaMainWindow";
            this.PranaMainWindow.ObjectImage = null;
            this.PranaMainWindow.Parent = this.PranaApplication;
            this.PranaMainWindow.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MainWindow;
            this.PranaMainWindow.UseCoordinatesOnClick = false;
            this.PranaMainWindow.WindowClass = "";
            // 
            // BlotterUIMap
            // 
            this.UIMapObjectApplications.Add(this.PranaApplication);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private UIApplication PranaApplication;
        protected UIWindow PranaMain;
        protected UIWindow BlotterMain;
        private UIWindow UltraPanel1;
        private UIWindow UltraPanel1ClientArea;
        private UIWindow CtrlBlotterMain;
        private UIWindow BlotterTabControl;
        private UIWindow OrdersTabPageControl;
        private UIWindow OrderBlotterGrid;
        protected UIWindow DgBlotter;
        private UIWindow UltraPanelSubOrdersBlotter;
        private UIWindow UltraPanelSubOrdersBlotterClientArea;
        private UIWindow SubOrderBlotterGrid;
        protected UIWindow DgBlotter1;
        protected UIMsaa Orders;
        protected UIMsaa WorkingSubs;
        private UIWindow WorkingSubsTabPageControl;
        private UIWindow WorkingSubBlotterGrid;
        protected UIWindow DgBlotter2;
        protected UIWindow PopupMenuContext;
        protected UIWindow BlotterMain_UltraFormManager_Dock_Area_Top;
        protected UIWindow NirvanaBlotter;
        protected UIWindow ButtonYes;
        protected UIWindow Warning;
        protected UIWindow ButtonOK;
        protected UIMsaa Summary;
        private UIWindow SummaryTabPageControl;
        private UIWindow SummaryBlotterGrid;
        protected UIWindow DgBlotter3;
        private UIWindow ButtonNo;
        private UIWindow ultraPanel11;
        protected UIWindow PopupMenuTransfertoUser;
        private UIMenuItem TransfertoUser;
        private UIMainWindow PranaMainWindow;
    }
}
