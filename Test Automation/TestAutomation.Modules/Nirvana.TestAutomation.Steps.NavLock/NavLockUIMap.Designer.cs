using Nirvana.TestAutomation.Utilities;
namespace Nirvana.TestAutomation.Steps.NavLock
{
    partial class NavLockUIMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NavLockUIMap));
            this.PranaApplication = new TestAutomationFX.UI.UIApplication();
            this.PranaMain = new TestAutomationFX.UI.UIWindow();
            this.FrmNavLock = new TestAutomationFX.UI.UIWindow();
            this.TableLayoutPanel1 = new TestAutomationFX.UI.UIWindow();
            this.FlowLayoutPanel1 = new TestAutomationFX.UI.UIWindow();
            this.BtnAddLock = new TestAutomationFX.UI.UIWindow();
            this.DtLockDate = new TestAutomationFX.UI.UIWindow();
            this.Asingletextcharacter = new TestAutomationFX.UI.UIMsaa();
            this.PranaUltraGrid1 = new TestAutomationFX.UI.UIWindow();
            this.ControlPartOfPranaMain = new TestAutomationFX.UI.UIControlPart();
            this.AboutPrana_UltraFormManager_Dock_Area_Top = new TestAutomationFX.UI.UIWindow();
            this.PromptWindow = new TestAutomationFX.UI.UIWindow();
            this.PromptWindow_Fill_Panel = new TestAutomationFX.UI.UIWindow();
            this.PromptWindow_Fill_PanelClientArea = new TestAutomationFX.UI.UIWindow();
            this.BtnPlace = new TestAutomationFX.UI.UIWindow();
            this.BtnEdit = new TestAutomationFX.UI.UIWindow();
            this.NavLockDelete = new TestAutomationFX.UI.UIMsaa();
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
            // FrmNavLock
            // 
            this.FrmNavLock.Comment = null;
            this.FrmNavLock.InstanceName = "frmNavLock";
            this.FrmNavLock.MatchedIndex = 0;
            this.FrmNavLock.MsaaName = "NAV Lock";
            this.FrmNavLock.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.FrmNavLock.Name = "FrmNavLock";
            this.FrmNavLock.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("FrmNavLock.ObjectImage")));
            this.FrmNavLock.OwnedWindow = true;
            this.FrmNavLock.Parent = this.PranaMain;
            this.FrmNavLock.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.FrmNavLock.UseCoordinatesOnClick = true;
            this.FrmNavLock.WindowClass = "";
            this.FrmNavLock.WindowText = "NAV Lock";
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.Comment = null;
            this.TableLayoutPanel1.Index = 0;
            this.TableLayoutPanel1.InstanceName = "tableLayoutPanel1";
            this.TableLayoutPanel1.MatchedIndex = 0;
            this.TableLayoutPanel1.MsaaName = null;
            this.TableLayoutPanel1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("TableLayoutPanel1.ObjectImage")));
            this.TableLayoutPanel1.Parent = this.FrmNavLock;
            this.TableLayoutPanel1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.TableLayoutPanel1.UseCoordinatesOnClick = true;
            this.TableLayoutPanel1.WindowClass = "";
            // 
            // FlowLayoutPanel1
            // 
            this.FlowLayoutPanel1.Comment = null;
            this.FlowLayoutPanel1.Index = 1;
            this.FlowLayoutPanel1.InstanceName = "flowLayoutPanel1";
            this.FlowLayoutPanel1.MatchedIndex = 0;
            this.FlowLayoutPanel1.MsaaName = null;
            this.FlowLayoutPanel1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.FlowLayoutPanel1.Name = "FlowLayoutPanel1";
            this.FlowLayoutPanel1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("FlowLayoutPanel1.ObjectImage")));
            this.FlowLayoutPanel1.Parent = this.TableLayoutPanel1;
            this.FlowLayoutPanel1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.FlowLayoutPanel1.UseCoordinatesOnClick = true;
            this.FlowLayoutPanel1.WindowClass = "";
            // 
            // BtnAddLock
            // 
            this.BtnAddLock.Comment = null;
            this.BtnAddLock.Index = 2;
            this.BtnAddLock.InstanceName = "btnAddLock";
            this.BtnAddLock.MatchedIndex = 0;
            this.BtnAddLock.MsaaName = "Add Lock";
            this.BtnAddLock.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnAddLock.Name = "BtnAddLock";
            this.BtnAddLock.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BtnAddLock.ObjectImage")));
            this.BtnAddLock.Parent = this.FlowLayoutPanel1;
            this.BtnAddLock.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.BtnAddLock.UseCoordinatesOnClick = false;
            this.BtnAddLock.WindowClass = "";
            this.BtnAddLock.WindowText = "Add Lock";
            // 
            // DtLockDate
            // 
            this.DtLockDate.Comment = null;
            this.DtLockDate.Index = 1;
            this.DtLockDate.InstanceName = "dtLockDate";
            this.DtLockDate.MatchedIndex = 0;
            this.DtLockDate.MsaaName = "__/__/____";
            this.DtLockDate.MsaaRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.DtLockDate.Name = "DtLockDate";
            this.DtLockDate.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("DtLockDate.ObjectImage")));
            this.DtLockDate.Parent = this.FlowLayoutPanel1;
            this.DtLockDate.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.DtLockDate.UseCoordinatesOnClick = true;
            this.DtLockDate.WindowClass = "";
            this.DtLockDate.WindowText = "__/__/____";
            // 
            // Asingletextcharacter
            // 
            this.Asingletextcharacter.Comment = null;
            this.Asingletextcharacter.MsaaName = "A single text character";
            this.Asingletextcharacter.Name = "Asingletextcharacter";
            this.Asingletextcharacter.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Asingletextcharacter.ObjectImage")));
            this.Asingletextcharacter.Parent = this.DtLockDate;
            this.Asingletextcharacter.Role = System.Windows.Forms.AccessibleRole.Text;
            this.Asingletextcharacter.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Label;
            this.Asingletextcharacter.UseCoordinatesOnClick = true;
            // 
            // PranaUltraGrid1
            // 
            this.PranaUltraGrid1.Comment = null;
            this.PranaUltraGrid1.Index = 0;
            this.PranaUltraGrid1.InstanceName = "pranaUltraGrid1";
            this.PranaUltraGrid1.MatchedIndex = 0;
            this.PranaUltraGrid1.MsaaName = "pranaUltraGrid1";
            this.PranaUltraGrid1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.PranaUltraGrid1.Name = "PranaUltraGrid1";
            this.PranaUltraGrid1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PranaUltraGrid1.ObjectImage")));
            this.PranaUltraGrid1.Parent = this.TableLayoutPanel1;
            this.PranaUltraGrid1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PranaUltraGrid1.UseCoordinatesOnClick = true;
            this.PranaUltraGrid1.WindowClass = "";
            this.PranaUltraGrid1.WindowText = "pranaUltraGrid1";
            // 
            // ControlPartOfPranaMain
            // 
            this.ControlPartOfPranaMain.BoundsInParent = new System.Drawing.Rectangle(625, 50, 35, 35);
            this.ControlPartOfPranaMain.Comment = null;
            this.ControlPartOfPranaMain.ControlPartProvider = null;
            this.ControlPartOfPranaMain.Name = "ControlPartOfPranaMain";
            this.ControlPartOfPranaMain.ObjectImage = null;
            this.ControlPartOfPranaMain.Parent = this.PranaMain;
            this.ControlPartOfPranaMain.Path = null;
            this.ControlPartOfPranaMain.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
            this.ControlPartOfPranaMain.UseCoordinatesOnClick = false;
            // 
            // AboutPrana_UltraFormManager_Dock_Area_Top
            // 
            this.AboutPrana_UltraFormManager_Dock_Area_Top.Comment = null;
            this.AboutPrana_UltraFormManager_Dock_Area_Top.Index = 3;
            this.AboutPrana_UltraFormManager_Dock_Area_Top.InstanceName = "_AboutPrana_UltraFormManager_Dock_Area_Top";
            this.AboutPrana_UltraFormManager_Dock_Area_Top.MatchedIndex = 0;
            this.AboutPrana_UltraFormManager_Dock_Area_Top.MsaaName = "DockTop";
            this.AboutPrana_UltraFormManager_Dock_Area_Top.MsaaRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.AboutPrana_UltraFormManager_Dock_Area_Top.Name = "AboutPrana_UltraFormManager_Dock_Area_Top";
            this.AboutPrana_UltraFormManager_Dock_Area_Top.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("AboutPrana_UltraFormManager_Dock_Area_Top.ObjectImage")));
            this.AboutPrana_UltraFormManager_Dock_Area_Top.Parent = this.FrmNavLock;
            this.AboutPrana_UltraFormManager_Dock_Area_Top.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.AboutPrana_UltraFormManager_Dock_Area_Top.UseCoordinatesOnClick = true;
            this.AboutPrana_UltraFormManager_Dock_Area_Top.WindowClass = "";
            // 
            // PromptWindow
            // 
            this.PromptWindow.Comment = null;
            this.PromptWindow.InstanceName = "PromptWindow";
            this.PromptWindow.MatchedIndex = 0;
            this.PromptWindow.MsaaName = "NIRVANA";
            this.PromptWindow.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.PromptWindow.Name = "PromptWindow";
            this.PromptWindow.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PromptWindow.ObjectImage")));
            this.PromptWindow.OwnedWindow = true;
            this.PromptWindow.Parent = this.FrmNavLock;
            this.PromptWindow.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PromptWindow.UseCoordinatesOnClick = true;
            this.PromptWindow.WindowClass = "";
            this.PromptWindow.WindowText = "NIRVANA";
            // 
            // PromptWindow_Fill_Panel
            // 
            this.PromptWindow_Fill_Panel.Comment = null;
            this.PromptWindow_Fill_Panel.Index = 0;
            this.PromptWindow_Fill_Panel.InstanceName = "PromptWindow_Fill_Panel";
            this.PromptWindow_Fill_Panel.MatchedIndex = 0;
            this.PromptWindow_Fill_Panel.MsaaName = null;
            this.PromptWindow_Fill_Panel.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.PromptWindow_Fill_Panel.Name = "PromptWindow_Fill_Panel";
            this.PromptWindow_Fill_Panel.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PromptWindow_Fill_Panel.ObjectImage")));
            this.PromptWindow_Fill_Panel.Parent = this.PromptWindow;
            this.PromptWindow_Fill_Panel.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PromptWindow_Fill_Panel.UseCoordinatesOnClick = true;
            this.PromptWindow_Fill_Panel.WindowClass = "";
            // 
            // PromptWindow_Fill_PanelClientArea
            // 
            this.PromptWindow_Fill_PanelClientArea.Comment = null;
            this.PromptWindow_Fill_PanelClientArea.Index = 0;
            this.PromptWindow_Fill_PanelClientArea.InstanceName = "PromptWindow_Fill_Panel.ClientArea";
            this.PromptWindow_Fill_PanelClientArea.MatchedIndex = 0;
            this.PromptWindow_Fill_PanelClientArea.MsaaName = null;
            this.PromptWindow_Fill_PanelClientArea.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.PromptWindow_Fill_PanelClientArea.Name = "PromptWindow_Fill_PanelClientArea";
            this.PromptWindow_Fill_PanelClientArea.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PromptWindow_Fill_PanelClientArea.ObjectImage")));
            this.PromptWindow_Fill_PanelClientArea.Parent = this.PromptWindow_Fill_Panel;
            this.PromptWindow_Fill_PanelClientArea.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PromptWindow_Fill_PanelClientArea.UseCoordinatesOnClick = true;
            this.PromptWindow_Fill_PanelClientArea.WindowClass = "";
            // 
            // BtnPlace
            // 
            this.BtnPlace.Comment = null;
            this.BtnPlace.Index = 1;
            this.BtnPlace.InstanceName = "btnPlace";
            this.BtnPlace.MatchedIndex = 0;
            this.BtnPlace.MsaaName = "Confirm";
            this.BtnPlace.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnPlace.Name = "BtnPlace";
            this.BtnPlace.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BtnPlace.ObjectImage")));
            this.BtnPlace.Parent = this.PromptWindow_Fill_PanelClientArea;
            this.BtnPlace.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.BtnPlace.UseCoordinatesOnClick = false;
            this.BtnPlace.WindowClass = "";
            this.BtnPlace.WindowText = "Confirm";
            // 
            // BtnEdit
            // 
            this.BtnEdit.Comment = null;
            this.BtnEdit.Index = 0;
            this.BtnEdit.InstanceName = "btnEdit";
            this.BtnEdit.MatchedIndex = 0;
            this.BtnEdit.MsaaName = "Cancel";
            this.BtnEdit.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BtnEdit.ObjectImage")));
            this.BtnEdit.Parent = this.PromptWindow_Fill_PanelClientArea;
            this.BtnEdit.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.BtnEdit.UseCoordinatesOnClick = false;
            this.BtnEdit.WindowClass = "";
            this.BtnEdit.WindowText = "Cancel";
            // 
            // NavLockDelete
            // 
            this.NavLockDelete.Comment = null;
            this.NavLockDelete.Index = 5;
            this.NavLockDelete.Name = "NavLockDelete";
            this.NavLockDelete.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("NavLockDelete.ObjectImage")));
            this.NavLockDelete.Parent = this.PranaUltraGrid1;
            this.NavLockDelete.Role = System.Windows.Forms.AccessibleRole.Cell;
            this.NavLockDelete.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.NavLockDelete.UseCoordinatesOnClick = true;
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
            // NavLockUIMap
            // 
            this.UIMapObjectApplications.Add(this.PranaApplication);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private TestAutomationFX.UI.UIApplication PranaApplication;
        private TestAutomationFX.UI.UIWindow PranaMain;
        protected TestAutomationFX.UI.UIWindow FrmNavLock;
        protected TestAutomationFX.UI.UIWindow TableLayoutPanel1;
        protected TestAutomationFX.UI.UIWindow FlowLayoutPanel1;
        protected TestAutomationFX.UI.UIWindow BtnAddLock;
        protected TestAutomationFX.UI.UIWindow DtLockDate;
        protected TestAutomationFX.UI.UIMsaa Asingletextcharacter;
        protected TestAutomationFX.UI.UIWindow PranaUltraGrid1;
        protected TestAutomationFX.UI.UIControlPart ControlPartOfPranaMain;
        protected TestAutomationFX.UI.UIWindow AboutPrana_UltraFormManager_Dock_Area_Top;
        protected TestAutomationFX.UI.UIWindow PromptWindow;
        protected TestAutomationFX.UI.UIWindow PromptWindow_Fill_Panel;
        protected TestAutomationFX.UI.UIWindow PromptWindow_Fill_PanelClientArea;
        protected TestAutomationFX.UI.UIWindow BtnPlace;
        protected TestAutomationFX.UI.UIWindow BtnEdit;
        protected TestAutomationFX.UI.UIMsaa NavLockDelete;
        private TestAutomationFX.UI.UIMainWindow PranaMainWindow;
    }
}
