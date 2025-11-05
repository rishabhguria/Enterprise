using Nirvana.TestAutomation.Utilities;

namespace Nirvana.TestAutomation.Steps.CreateTransaction
{
    partial class CreateTransactionUIMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateTransactionUIMap));
            this.PranaApplication = new TestAutomationFX.UI.UIApplication();
            this.PranaMain = new TestAutomationFX.UI.UIWindow();
            this.UltraPanel1 = new TestAutomationFX.UI.UIWindow();
            this.UltraPanel1ClientArea = new TestAutomationFX.UI.UIWindow();
            this.MenuPrana = new TestAutomationFX.UI.UIWindow();
            this.PortfolioManagement = new TestAutomationFX.UI.UIMenuItem();
            this.CreateTransaction1 = new TestAutomationFX.UI.UIMenuItem();
            this.PopupMenuPortfolioManagementDropDown = new TestAutomationFX.UI.UIWindow();
            this.CreatePosition = new TestAutomationFX.UI.UIWindow();
            this.TcCreateandImportPositions = new TestAutomationFX.UI.UIWindow();
            this.UltraTabPageControl3 = new TestAutomationFX.UI.UIWindow();
            this.BtnAddToCloseTrade = new TestAutomationFX.UI.UIWindow();
            this.BtnSave = new TestAutomationFX.UI.UIWindow();
            this.CtrlCreateAndImportPosition1 = new TestAutomationFX.UI.UIWindow();
            this.GrdCreatePosition = new TestAutomationFX.UI.UIUltraGrid();
            this.CreateTransactionSave = new TestAutomationFX.UI.UIWindow();
            this.ButtonYes = new TestAutomationFX.UI.UIWindow();
            this.CreateTransactionsSave = new TestAutomationFX.UI.UIWindow();
            this.TitleBar = new TestAutomationFX.UI.UIMsaa();
            this.ButtonSaveYes = new TestAutomationFX.UI.UIWindow();
            this.CreatePosition_UltraFormManager_Dock_Area_Top = new TestAutomationFX.UI.UIWindow();
            this.ButtonNo = new TestAutomationFX.UI.UIWindow();
            this.NAVLock = new TestAutomationFX.UI.UIWindow();
            this.TitleBar1 = new TestAutomationFX.UI.UIMsaa();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PranaApplication
            // 
            this.PranaApplication.Comment = null;
            this.PranaApplication.ImagePath = ApplicationArguments.ClientReleasePath + "\\Prana.exe";
            this.PranaApplication.Name = "PranaApplication";
            this.PranaApplication.ObjectImage = null;
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
            this.PranaMain.ObjectImage = null;
            this.PranaMain.OwnedWindow = true;
            this.PranaMain.Parent = this.PranaApplication;
            this.PranaMain.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PranaMain.UseCoordinatesOnClick = true;
            this.PranaMain.WindowClass = "";
            // 
            // UltraPanel1
            // 
            this.UltraPanel1.Comment = null;
            this.UltraPanel1.Index = 1;
            this.UltraPanel1.InstanceName = "ultraPanel1";
            this.UltraPanel1.MatchedIndex = 0;
            this.UltraPanel1.MsaaName = null;
            this.UltraPanel1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanel1.Name = "UltraPanel1";
            this.UltraPanel1.ObjectImage = null;
            this.UltraPanel1.Parent = this.PranaMain;
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
            this.UltraPanel1ClientArea.ObjectImage = null;
            this.UltraPanel1ClientArea.Parent = this.UltraPanel1;
            this.UltraPanel1ClientArea.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraPanel1ClientArea.UseCoordinatesOnClick = true;
            this.UltraPanel1ClientArea.WindowClass = "";
            // 
            // MenuPrana
            // 
            this.MenuPrana.Comment = null;
            this.MenuPrana.Index = 0;
            this.MenuPrana.InstanceName = "menuPrana";
            this.MenuPrana.MatchedIndex = 0;
            this.MenuPrana.MsaaName = null;
            this.MenuPrana.MsaaRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.MenuPrana.Name = "MenuPrana";
            this.MenuPrana.ObjectImage = null;
            this.MenuPrana.Parent = this.UltraPanel1ClientArea;
            this.MenuPrana.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.MenuPrana.UseCoordinatesOnClick = true;
            this.MenuPrana.WindowClass = "";
            // 
            // PortfolioManagement
            // 
            this.PortfolioManagement.Comment = null;
            this.PortfolioManagement.MsaaName = "Portfolio Management";
            this.PortfolioManagement.Name = "PortfolioManagement";
            this.PortfolioManagement.ObjectImage = null;
            this.PortfolioManagement.Parent = this.MenuPrana;
            this.PortfolioManagement.Role = System.Windows.Forms.AccessibleRole.MenuItem;
            this.PortfolioManagement.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MenuItem;
            this.PortfolioManagement.UseCoordinatesOnClick = false;
            // 
            // CreateTransaction1
            // 
            this.CreateTransaction1.Comment = null;
            this.CreateTransaction1.MousePath = TestAutomationFX.Core.UI.MousePath.HorizontalVertical;
            this.CreateTransaction1.MsaaName = "Create Transaction";
            this.CreateTransaction1.Name = "CreateTransaction1";
            this.CreateTransaction1.ObjectImage = null;
            this.CreateTransaction1.Parent = this.MenuPrana;
            this.CreateTransaction1.Role = System.Windows.Forms.AccessibleRole.MenuItem;
            this.CreateTransaction1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MenuItem;
            this.CreateTransaction1.UseCoordinatesOnClick = false;
            // 
            // PopupMenuPortfolioManagementDropDown
            // 
            this.PopupMenuPortfolioManagementDropDown.Comment = null;
            this.PopupMenuPortfolioManagementDropDown.Index = 0;
            this.PopupMenuPortfolioManagementDropDown.MatchedIndex = 0;
            this.PopupMenuPortfolioManagementDropDown.MsaaName = "Portfolio ManagementDropDown";
            this.PopupMenuPortfolioManagementDropDown.MsaaRole = System.Windows.Forms.AccessibleRole.MenuPopup;
            this.PopupMenuPortfolioManagementDropDown.Name = "PopupMenuPortfolioManagementDropDown";
            this.PopupMenuPortfolioManagementDropDown.ObjectImage = null;
            this.PopupMenuPortfolioManagementDropDown.OwnedWindow = true;
            this.PopupMenuPortfolioManagementDropDown.Parent = this.PranaMain;
            this.PopupMenuPortfolioManagementDropDown.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PopupMenuPortfolioManagementDropDown.UseCoordinatesOnClick = true;
            this.PopupMenuPortfolioManagementDropDown.WindowClass = "";
            // 
            // CreatePosition
            // 
            this.CreatePosition.Comment = null;
            this.CreatePosition.InstanceName = "CreatePosition";
            this.CreatePosition.MatchedIndex = 0;
            this.CreatePosition.MsaaName = "Create Transaction";
            this.CreatePosition.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.CreatePosition.Name = "CreatePosition";
            this.CreatePosition.ObjectImage = null;
            this.CreatePosition.OwnedWindow = true;
            this.CreatePosition.Parent = this.PranaMain;
            this.CreatePosition.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CreatePosition.UseCoordinatesOnClick = true;
            this.CreatePosition.WindowClass = "";
            this.CreatePosition.WindowText = "Create Transaction";
            // 
            // TcCreateandImportPositions
            // 
            this.TcCreateandImportPositions.Comment = null;
            this.TcCreateandImportPositions.Index = 0;
            this.TcCreateandImportPositions.InstanceName = "tcCreateandImportPositions";
            this.TcCreateandImportPositions.MatchedIndex = 0;
            this.TcCreateandImportPositions.MsaaName = null;
            this.TcCreateandImportPositions.MsaaRole = System.Windows.Forms.AccessibleRole.PageTabList;
            this.TcCreateandImportPositions.Name = "TcCreateandImportPositions";
            this.TcCreateandImportPositions.ObjectImage = null;
            this.TcCreateandImportPositions.Parent = this.CreatePosition;
            this.TcCreateandImportPositions.UIObjectType = TestAutomationFX.UI.UIObjectTypes.TabControl;
            this.TcCreateandImportPositions.UseCoordinatesOnClick = true;
            this.TcCreateandImportPositions.WindowClass = "";
            // 
            // UltraTabPageControl3
            // 
            this.UltraTabPageControl3.Comment = null;
            this.UltraTabPageControl3.Index = 1;
            this.UltraTabPageControl3.InstanceName = "ultraTabPageControl3";
            this.UltraTabPageControl3.MatchedIndex = 0;
            this.UltraTabPageControl3.MsaaName = null;
            this.UltraTabPageControl3.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraTabPageControl3.Name = "UltraTabPageControl3";
            this.UltraTabPageControl3.ObjectImage = null;
            this.UltraTabPageControl3.Parent = this.TcCreateandImportPositions;
            this.UltraTabPageControl3.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraTabPageControl3.UseCoordinatesOnClick = true;
            this.UltraTabPageControl3.WindowClass = "";
            // 
            // BtnAddToCloseTrade
            // 
            this.BtnAddToCloseTrade.Comment = null;
            this.BtnAddToCloseTrade.Index = 0;
            this.BtnAddToCloseTrade.InstanceName = "btnAddToCloseTrade";
            this.BtnAddToCloseTrade.MatchedIndex = 0;
            this.BtnAddToCloseTrade.MsaaName = "Add New";
            this.BtnAddToCloseTrade.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnAddToCloseTrade.Name = "BtnAddToCloseTrade";
            this.BtnAddToCloseTrade.ObjectImage = null;
            this.BtnAddToCloseTrade.Parent = this.UltraTabPageControl3;
            this.BtnAddToCloseTrade.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.BtnAddToCloseTrade.UseCoordinatesOnClick = false;
            this.BtnAddToCloseTrade.WindowClass = "";
            this.BtnAddToCloseTrade.WindowText = "Add New";
            // 
            // BtnSave
            // 
            this.BtnSave.Comment = null;
            this.BtnSave.Index = 2;
            this.BtnSave.InstanceName = "btnSave";
            this.BtnSave.MatchedIndex = 0;
            this.BtnSave.MsaaName = "Save";
            this.BtnSave.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.ObjectImage = null;
            this.BtnSave.Parent = this.UltraTabPageControl3;
            this.BtnSave.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.BtnSave.UseCoordinatesOnClick = false;
            this.BtnSave.WindowClass = "";
            this.BtnSave.WindowText = "Save";
            // 
            // CtrlCreateAndImportPosition1
            // 
            this.CtrlCreateAndImportPosition1.Comment = null;
            this.CtrlCreateAndImportPosition1.Index = 4;
            this.CtrlCreateAndImportPosition1.InstanceName = "ctrlCreateAndImportPosition1";
            this.CtrlCreateAndImportPosition1.MatchedIndex = 0;
            this.CtrlCreateAndImportPosition1.MsaaName = null;
            this.CtrlCreateAndImportPosition1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.CtrlCreateAndImportPosition1.Name = "CtrlCreateAndImportPosition1";
            this.CtrlCreateAndImportPosition1.ObjectImage = null;
            this.CtrlCreateAndImportPosition1.Parent = this.UltraTabPageControl3;
            this.CtrlCreateAndImportPosition1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CtrlCreateAndImportPosition1.UseCoordinatesOnClick = true;
            this.CtrlCreateAndImportPosition1.WindowClass = "";
            // 
            // GrdCreatePosition
            // 
            this.GrdCreatePosition.Comment = null;
            this.GrdCreatePosition.Index = 0;
            this.GrdCreatePosition.InstanceName = "grdCreatePosition";
            this.GrdCreatePosition.MatchedIndex = 0;
            this.GrdCreatePosition.MsaaName = "grdCreatePosition";
            this.GrdCreatePosition.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.GrdCreatePosition.Name = "GrdCreatePosition";
            this.GrdCreatePosition.ObjectImage = null;
            this.GrdCreatePosition.Parent = this.CtrlCreateAndImportPosition1;
            this.GrdCreatePosition.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Grid;
            this.GrdCreatePosition.UseCoordinatesOnClick = true;
            this.GrdCreatePosition.WindowClass = "";
            this.GrdCreatePosition.WindowText = "grdCreatePosition";
            // 
            // CreateTransactionSave
            // 
            this.CreateTransactionSave.Comment = null;
            this.CreateTransactionSave.Index = 0;
            this.CreateTransactionSave.MatchedIndex = 0;
            this.CreateTransactionSave.MsaaName = "Create Transaction Save";
            this.CreateTransactionSave.MsaaRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.CreateTransactionSave.Name = "CreateTransactionSave";
            this.CreateTransactionSave.ObjectImage = null;
            this.CreateTransactionSave.OwnedWindow = true;
            this.CreateTransactionSave.Parent = this.CreatePosition;
            this.CreateTransactionSave.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CreateTransactionSave.UseCoordinatesOnClick = true;
            this.CreateTransactionSave.WindowClass = "#32770";
            this.CreateTransactionSave.WindowText = "Create Transaction Save";
            // 
            // ButtonYes
            // 
            this.ButtonYes.Comment = null;
            this.ButtonYes.Index = 0;
            this.ButtonYes.MatchedIndex = 0;
            this.ButtonYes.MsaaName = "Yes";
            this.ButtonYes.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ButtonYes.Name = "ButtonYes";
            this.ButtonYes.ObjectImage = null;
            this.ButtonYes.Parent = this.CreateTransactionSave;
            this.ButtonYes.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.ButtonYes.UseCoordinatesOnClick = false;
            this.ButtonYes.WindowClass = "Button";
            this.ButtonYes.WindowText = "&Yes";
            // 
            // CreateTransactionsSave
            // 
            this.CreateTransactionsSave.Comment = null;
            this.CreateTransactionsSave.Index = 0;
            this.CreateTransactionsSave.IsOptional = true;
            this.CreateTransactionsSave.MatchedIndex = 0;
            this.CreateTransactionsSave.MsaaName = "Create Transactions Save";
            this.CreateTransactionsSave.MsaaRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.CreateTransactionsSave.Name = "CreateTransactionsSave";
            this.CreateTransactionsSave.ObjectImage = null;
            this.CreateTransactionsSave.OwnedWindow = true;
            this.CreateTransactionsSave.Parent = this.CreatePosition;
            this.CreateTransactionsSave.TimeOut = 2000;
            this.CreateTransactionsSave.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CreateTransactionsSave.UseCoordinatesOnClick = true;
            this.CreateTransactionsSave.WindowClass = "#32770";
            this.CreateTransactionsSave.WindowText = "Create Transactions Save";
            // 
            // TitleBar
            // 
            this.TitleBar.Comment = null;
            this.TitleBar.Index = 1;
            this.TitleBar.MsaaName = null;
            this.TitleBar.Name = "TitleBar";
            this.TitleBar.ObjectImage = null;
            this.TitleBar.Parent = this.CreateTransactionsSave;
            this.TitleBar.Role = System.Windows.Forms.AccessibleRole.TitleBar;
            this.TitleBar.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.TitleBar.UseCoordinatesOnClick = true;
            // 
            // ButtonSaveYes
            // 
            this.ButtonSaveYes.Comment = null;
            this.ButtonSaveYes.Index = 0;
            this.ButtonSaveYes.IsOptional = true;
            this.ButtonSaveYes.MatchedIndex = 0;
            this.ButtonSaveYes.MsaaName = "Yes";
            this.ButtonSaveYes.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ButtonSaveYes.Name = "ButtonSaveYes";
            this.ButtonSaveYes.ObjectImage = null;
            this.ButtonSaveYes.Parent = this.CreateTransactionsSave;
            this.ButtonSaveYes.TimeOut = 1000;
            this.ButtonSaveYes.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.ButtonSaveYes.UseCoordinatesOnClick = false;
            this.ButtonSaveYes.WindowClass = "Button";
            this.ButtonSaveYes.WindowText = "&Yes";
            // 
            // CreatePosition_UltraFormManager_Dock_Area_Top
            // 
            this.CreatePosition_UltraFormManager_Dock_Area_Top.Comment = null;
            this.CreatePosition_UltraFormManager_Dock_Area_Top.Index = 3;
            this.CreatePosition_UltraFormManager_Dock_Area_Top.InstanceName = "_CreatePosition_UltraFormManager_Dock_Area_Top";
            this.CreatePosition_UltraFormManager_Dock_Area_Top.MatchedIndex = 0;
            this.CreatePosition_UltraFormManager_Dock_Area_Top.MsaaName = "DockTop";
            this.CreatePosition_UltraFormManager_Dock_Area_Top.MsaaRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.CreatePosition_UltraFormManager_Dock_Area_Top.Name = "CreatePosition_UltraFormManager_Dock_Area_Top";
            this.CreatePosition_UltraFormManager_Dock_Area_Top.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("CreatePosition_UltraFormManager_Dock_Area_Top.ObjectImage")));
            this.CreatePosition_UltraFormManager_Dock_Area_Top.Parent = this.CreatePosition;
            this.CreatePosition_UltraFormManager_Dock_Area_Top.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CreatePosition_UltraFormManager_Dock_Area_Top.UseCoordinatesOnClick = true;
            this.CreatePosition_UltraFormManager_Dock_Area_Top.WindowClass = "";
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
            this.ButtonNo.Parent = this.CreateTransactionsSave;
            this.ButtonNo.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.ButtonNo.UseCoordinatesOnClick = false;
            this.ButtonNo.WindowClass = "Button";
            this.ButtonNo.WindowText = "&No";
            // 
            // NAVLock
            // 
            this.NAVLock.Comment = null;
            this.NAVLock.Index = 0;
            this.NAVLock.IsOptional = true;
            this.NAVLock.MatchedIndex = 0;
            this.NAVLock.MsaaName = "NAV Lock";
            this.NAVLock.MsaaRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.NAVLock.Name = "NAVLock";
            this.NAVLock.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("NAVLock.ObjectImage")));
            this.NAVLock.OwnedWindow = true;
            this.NAVLock.Parent = this.CreatePosition;
            this.NAVLock.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.NAVLock.UseCoordinatesOnClick = true;
            this.NAVLock.WindowClass = "#32770";
            this.NAVLock.WindowText = "NAV Lock";
            // 
            // TitleBar1
            // 
            this.TitleBar1.Comment = null;
            this.TitleBar1.Index = 1;
            this.TitleBar1.MsaaName = null;
            this.TitleBar1.Name = "TitleBar1";
            this.TitleBar1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("TitleBar1.ObjectImage")));
            this.TitleBar1.Parent = this.NAVLock;
            this.TitleBar1.Role = System.Windows.Forms.AccessibleRole.TitleBar;
            this.TitleBar1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.TitleBar1.UseCoordinatesOnClick = true;
            // 
            // CreateTransactionUIMap
            // 
            this.Settings.PlaybackSettings.AutoStartTests = true;
            this.Settings.PlaybackSettings.CloseTestFormAfterTests = true;
            this.Settings.PlaybackSettings.SaveScreenShotOnTestFailure = false;
            this.Settings.PlaybackSettings.StopOnFailedTest = false;
            this.UIMapObjectApplications.Add(this.PranaApplication);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

     
        protected TestAutomationFX.UI.UIApplication PranaApplication;
        protected TestAutomationFX.UI.UIWindow PranaMain;
        protected TestAutomationFX.UI.UIWindow UltraPanel1;
        protected TestAutomationFX.UI.UIWindow UltraPanel1ClientArea;
        protected TestAutomationFX.UI.UIWindow MenuPrana;
        protected TestAutomationFX.UI.UIMenuItem PortfolioManagement;
        protected TestAutomationFX.UI.UIMenuItem CreateTransaction1;
        protected TestAutomationFX.UI.UIWindow PopupMenuPortfolioManagementDropDown;
        protected TestAutomationFX.UI.UIWindow CreatePosition;
        protected TestAutomationFX.UI.UIWindow TcCreateandImportPositions;
        protected TestAutomationFX.UI.UIWindow UltraTabPageControl3;
        protected TestAutomationFX.UI.UIWindow BtnAddToCloseTrade;
        protected TestAutomationFX.UI.UIWindow BtnSave;
        protected TestAutomationFX.UI.UIWindow CtrlCreateAndImportPosition1;
        protected TestAutomationFX.UI.UIUltraGrid GrdCreatePosition;
        protected TestAutomationFX.UI.UIWindow CreateTransactionSave;
        protected TestAutomationFX.UI.UIWindow ButtonYes;
        protected TestAutomationFX.UI.UIWindow CreateTransactionsSave;
        protected TestAutomationFX.UI.UIMsaa TitleBar;
        protected TestAutomationFX.UI.UIWindow ButtonSaveYes;
        protected TestAutomationFX.UI.UIWindow CreatePosition_UltraFormManager_Dock_Area_Top;
        protected TestAutomationFX.UI.UIWindow ButtonNo;
        protected TestAutomationFX.UI.UIWindow NAVLock;
        protected TestAutomationFX.UI.UIMsaa TitleBar1;
    }
}
