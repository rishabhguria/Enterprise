using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.UI;
namespace Nirvana.TestAutomation.Steps.PranaClient
{
    public partial class PranaClientUIMap : UIMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PranaClientUIMap));
            this.PranaServerApplication = new TestAutomationFX.UI.UIApplication();
            this.Server = new TestAutomationFX.UI.UIWindow();
            this.PranaApplication = new TestAutomationFX.UI.UIApplication();
            this.PranaMain = new TestAutomationFX.UI.UIWindow();
            this.PranaMain_UltraFormManager_Dock_Area_Top = new TestAutomationFX.UI.UIWindow();
            this.Close = new TestAutomationFX.UI.UIMsaa();
            this.Minimize = new TestAutomationFX.UI.UIMsaa();
            this.SaveLayout = new TestAutomationFX.UI.UIWindow();
            this.ButtonNo = new TestAutomationFX.UI.UIWindow();
            this.ButtonYes = new TestAutomationFX.UI.UIWindow();
            this.Login = new TestAutomationFX.UI.UIWindow();
            this.TxtLoginID = new TestAutomationFX.UI.UIUltraTextEditor();
            this.TxtPassword = new TestAutomationFX.UI.UIUltraTextEditor();
            this.BtnLogin = new TestAutomationFX.UI.UIWindow();
            this.BtnClose = new TestAutomationFX.UI.UIWindow();
            this.uiMainWindow1 = new TestAutomationFX.UI.UIMainWindow();
            this.PranaMainWindow = new TestAutomationFX.UI.UIMainWindow();
            this.uiMainWindow2 = new TestAutomationFX.UI.UIMainWindow();
            this.PranaMainWindow1 = new TestAutomationFX.UI.UIMainWindow();
            this.CustomMessageBox = new TestAutomationFX.UI.UIWindow();
            this.TableLayoutPanel1 = new TestAutomationFX.UI.UIWindow();
            this.UltraPanelBottom = new TestAutomationFX.UI.UIWindow();
            this.UltraOkButton = new TestAutomationFX.UI.UIWindow();
            this.uiMainWindow3 = new TestAutomationFX.UI.UIMainWindow();
            this.PranaMainWindow2 = new TestAutomationFX.UI.UIMainWindow();
            this.ControlPartOfPranaMain = new TestAutomationFX.UI.UIControlPart();
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top = new TestAutomationFX.UI.UIControlPart();
            this.uiMainWindow4 = new TestAutomationFX.UI.UIMainWindow();
            this.PranaMainWindow3 = new TestAutomationFX.UI.UIMainWindow();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PranaServerApplication
            // 
            this.PranaServerApplication.Comment = null;
            this.PranaServerApplication.ImagePath = ApplicationArguments.ClientReleasePath + "\\Prana.exe";
            this.PranaServerApplication.Name = "PranaServerApplication";
            this.PranaServerApplication.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PranaServerApplication.ObjectImage")));
            this.PranaServerApplication.Parent = null;
            this.PranaServerApplication.ProcessName = "Prana.Server";
            this.PranaServerApplication.TimeOut = 1000;
            this.PranaServerApplication.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Application;
            this.PranaServerApplication.UseCoordinatesOnClick = false;
            this.PranaServerApplication.UsedMatchedProperties = ((TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties)((TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties.ProcessName | TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties.CommandLineArguments)));
            this.PranaServerApplication.WorkingDirectory = ApplicationArguments.ClientReleasePath;
            // 
            // Server
            // 
            this.Server.Comment = null;
            this.Server.InstanceName = "Server";
            this.Server.MatchedIndex = 0;
            this.Server.MsaaName = "alberta: Prana Trade Server, v1.9.0.0";
            this.Server.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.Server.Name = "Server";
            this.Server.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Server.ObjectImage")));
            this.Server.OwnedWindow = true;
            this.Server.Parent = this.PranaServerApplication;
            this.Server.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.Server.UseCoordinatesOnClick = true;
            this.Server.WindowClass = "";
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
            this.PranaMain.IsOptional = true;
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
            // PranaMain_UltraFormManager_Dock_Area_Top
            // 
            this.PranaMain_UltraFormManager_Dock_Area_Top.Comment = null;
            this.PranaMain_UltraFormManager_Dock_Area_Top.Index = 5;
            this.PranaMain_UltraFormManager_Dock_Area_Top.InstanceName = "_PranaMain_UltraFormManager_Dock_Area_Top";
            this.PranaMain_UltraFormManager_Dock_Area_Top.MatchedIndex = 0;
            this.PranaMain_UltraFormManager_Dock_Area_Top.MsaaName = "DockTop";
            this.PranaMain_UltraFormManager_Dock_Area_Top.MsaaRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.PranaMain_UltraFormManager_Dock_Area_Top.Name = "PranaMain_UltraFormManager_Dock_Area_Top";
            this.PranaMain_UltraFormManager_Dock_Area_Top.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PranaMain_UltraFormManager_Dock_Area_Top.ObjectImage")));
            this.PranaMain_UltraFormManager_Dock_Area_Top.Parent = this.PranaMain;
            this.PranaMain_UltraFormManager_Dock_Area_Top.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PranaMain_UltraFormManager_Dock_Area_Top.UseCoordinatesOnClick = true;
            this.PranaMain_UltraFormManager_Dock_Area_Top.WindowClass = "";
            // 
            // Close
            // 
            this.Close.Comment = null;
            this.Close.MsaaName = "Close";
            this.Close.Name = "Close";
            this.Close.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Close.ObjectImage")));
            this.Close.Parent = this.PranaMain_UltraFormManager_Dock_Area_Top;
            this.Close.Role = System.Windows.Forms.AccessibleRole.PushButton;
            this.Close.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.Close.UseCoordinatesOnClick = false;
            // 
            // Minimize
            // 
            this.Minimize.Comment = null;
            this.Minimize.MsaaName = "Minimize";
            this.Minimize.Name = "Minimize";
            this.Minimize.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Minimize.ObjectImage")));
            this.Minimize.Parent = this.PranaMain_UltraFormManager_Dock_Area_Top;
            this.Minimize.Role = System.Windows.Forms.AccessibleRole.PushButton;
            this.Minimize.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.Minimize.UseCoordinatesOnClick = false;
            // 
            // SaveLayout
            // 
            this.SaveLayout.Comment = null;
            this.SaveLayout.Index = 0;
            this.SaveLayout.IsOptional = true;
            this.SaveLayout.MatchedIndex = 0;
            this.SaveLayout.MsaaName = "Save Layout";
            this.SaveLayout.MsaaRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.SaveLayout.Name = "SaveLayout";
            this.SaveLayout.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("SaveLayout.ObjectImage")));
            this.SaveLayout.OwnedWindow = true;
            this.SaveLayout.Parent = this.PranaMain;
            this.SaveLayout.TimeOut = 3000;
            this.SaveLayout.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.SaveLayout.UseCoordinatesOnClick = true;
            this.SaveLayout.WindowClass = "#32770";
            this.SaveLayout.WindowText = "Save Layout";
            // 
            // ButtonNo
            // 
            this.ButtonNo.Comment = null;
            this.ButtonNo.Index = 1;
            this.ButtonNo.IsOptional = true;
            this.ButtonNo.MatchedIndex = 0;
            this.ButtonNo.MsaaName = "No";
            this.ButtonNo.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ButtonNo.Name = "ButtonNo";
            this.ButtonNo.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("ButtonNo.ObjectImage")));
            this.ButtonNo.Parent = this.SaveLayout;
            this.ButtonNo.TimeOut = 3000;
            this.ButtonNo.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.ButtonNo.UseCoordinatesOnClick = false;
            this.ButtonNo.WindowClass = "Button";
            this.ButtonNo.WindowText = "&No";
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
            this.ButtonYes.Parent = this.SaveLayout;
            this.ButtonYes.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.ButtonYes.UseCoordinatesOnClick = false;
            this.ButtonYes.WindowClass = "Button";
            this.ButtonYes.WindowText = "&Yes";
            // 
            // Login
            // 
            this.Login.Comment = null;
            this.Login.InstanceName = "Login";
            this.Login.IsOptional = true;
            this.Login.MatchedIndex = 0;
            this.Login.MsaaName = "Nirvana: User Login";
            this.Login.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.Login.Name = "Login";
            this.Login.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Login.ObjectImage")));
            this.Login.OwnedWindow = true;
            this.Login.Parent = this.PranaApplication;
            this.Login.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.Login.UseCoordinatesOnClick = true;
            this.Login.WindowClass = "";
            this.Login.WindowText = "Nirvana: User Login";
            // 
            // TxtLoginID
            // 
            this.TxtLoginID.AllowChildren = false;
            this.TxtLoginID.Comment = null;
            this.TxtLoginID.Index = 5;
            this.TxtLoginID.InstanceName = "txtLoginID";
            this.TxtLoginID.MatchedIndex = 0;
            this.TxtLoginID.MsaaName = null;
            this.TxtLoginID.Name = "TxtLoginID";
            this.TxtLoginID.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("TxtLoginID.ObjectImage")));
            this.TxtLoginID.Parent = this.Login;
            this.TxtLoginID.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.TxtLoginID.UseCoordinatesOnClick = true;
            this.TxtLoginID.WindowClass = "";
            // 
            // TxtPassword
            // 
            this.TxtPassword.AllowChildren = false;
            this.TxtPassword.Comment = null;
            this.TxtPassword.Index = 2;
            this.TxtPassword.InstanceName = "txtPassword";
            this.TxtPassword.MatchedIndex = 0;
            this.TxtPassword.MsaaName = null;
            this.TxtPassword.Name = "TxtPassword";
            this.TxtPassword.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("TxtPassword.ObjectImage")));
            this.TxtPassword.Parent = this.Login;
            this.TxtPassword.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.TxtPassword.UseCoordinatesOnClick = true;
            this.TxtPassword.WindowClass = "";
            // 
            // BtnLogin
            // 
            this.BtnLogin.Comment = null;
            this.BtnLogin.Index = 3;
            this.BtnLogin.InstanceName = "btnLogin";
            this.BtnLogin.MatchedIndex = 0;
            this.BtnLogin.MsaaName = null;
            this.BtnLogin.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BtnLogin.ObjectImage")));
            this.BtnLogin.Parent = this.Login;
            this.BtnLogin.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.BtnLogin.UseCoordinatesOnClick = false;
            this.BtnLogin.WindowClass = "";
            // 
            // BtnClose
            // 
            this.BtnClose.Comment = null;
            this.BtnClose.Index = 4;
            this.BtnClose.InstanceName = "btnClose";
            this.BtnClose.MatchedIndex = 0;
            this.BtnClose.MsaaName = null;
            this.BtnClose.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BtnClose.ObjectImage")));
            this.BtnClose.Parent = this.Login;
            this.BtnClose.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.BtnClose.UseCoordinatesOnClick = false;
            this.BtnClose.WindowClass = "";
            // 
            // uiMainWindow1
            // 
            this.uiMainWindow1.Comment = null;
            this.uiMainWindow1.Name = "uiMainWindow1";
            this.uiMainWindow1.ObjectImage = null;
            this.uiMainWindow1.Parent = this.PranaServerApplication;
            this.uiMainWindow1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MainWindow;
            this.uiMainWindow1.UseCoordinatesOnClick = false;
            this.uiMainWindow1.WindowClass = "";
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
            // uiMainWindow2
            // 
            this.uiMainWindow2.Comment = null;
            this.uiMainWindow2.Name = "uiMainWindow2";
            this.uiMainWindow2.ObjectImage = null;
            this.uiMainWindow2.Parent = this.PranaServerApplication;
            this.uiMainWindow2.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MainWindow;
            this.uiMainWindow2.UseCoordinatesOnClick = false;
            this.uiMainWindow2.WindowClass = "";
            // 
            // PranaMainWindow1
            // 
            this.PranaMainWindow1.Comment = null;
            this.PranaMainWindow1.Name = "PranaMainWindow1";
            this.PranaMainWindow1.ObjectImage = null;
            this.PranaMainWindow1.Parent = this.PranaApplication;
            this.PranaMainWindow1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MainWindow;
            this.PranaMainWindow1.UseCoordinatesOnClick = false;
            this.PranaMainWindow1.WindowClass = "";
            // 
            // CustomMessageBox
            // 
            this.CustomMessageBox.Comment = null;
            this.CustomMessageBox.InstanceName = "CustomMessageBox";
            this.CustomMessageBox.IsOptional = true;
            this.CustomMessageBox.MatchedIndex = 0;
            this.CustomMessageBox.MsaaName = null;
            this.CustomMessageBox.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.CustomMessageBox.Name = "CustomMessageBox";
            this.CustomMessageBox.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("CustomMessageBox.ObjectImage")));
            this.CustomMessageBox.OwnedWindow = true;
            this.CustomMessageBox.Parent = this.Login;
            this.CustomMessageBox.TimeOut = 3500;
            this.CustomMessageBox.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CustomMessageBox.UseCoordinatesOnClick = true;
            this.CustomMessageBox.WindowClass = "";
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
            this.TableLayoutPanel1.Parent = this.CustomMessageBox;
            this.TableLayoutPanel1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.TableLayoutPanel1.UseCoordinatesOnClick = true;
            this.TableLayoutPanel1.WindowClass = "";
            // 
            // UltraPanelBottom
            // 
            this.UltraPanelBottom.Comment = null;
            this.UltraPanelBottom.Index = 1;
            this.UltraPanelBottom.InstanceName = "ultraPanelBottom";
            this.UltraPanelBottom.MatchedIndex = 0;
            this.UltraPanelBottom.MsaaName = null;
            this.UltraPanelBottom.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanelBottom.Name = "UltraPanelBottom";
            this.UltraPanelBottom.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanelBottom.ObjectImage")));
            this.UltraPanelBottom.Parent = this.TableLayoutPanel1;
            this.UltraPanelBottom.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraPanelBottom.UseCoordinatesOnClick = true;
            this.UltraPanelBottom.WindowClass = "";
            // 
            // UltraOkButton
            // 
            this.UltraOkButton.Comment = null;
            this.UltraOkButton.Index = 0;
            this.UltraOkButton.InstanceName = "ultraOkButton";
            this.UltraOkButton.MatchedIndex = 0;
            this.UltraOkButton.MsaaName = "Proceed";
            this.UltraOkButton.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.UltraOkButton.Name = "UltraOkButton";
            this.UltraOkButton.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraOkButton.ObjectImage")));
            this.UltraOkButton.Parent = this.UltraPanelBottom;
            this.UltraOkButton.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.UltraOkButton.UseCoordinatesOnClick = false;
            this.UltraOkButton.WindowClass = "";
            this.UltraOkButton.WindowText = "Proceed";
            // 
            // uiMainWindow3
            // 
            this.uiMainWindow3.Comment = null;
            this.uiMainWindow3.Name = "uiMainWindow3";
            this.uiMainWindow3.ObjectImage = null;
            this.uiMainWindow3.Parent = this.PranaServerApplication;
            this.uiMainWindow3.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MainWindow;
            this.uiMainWindow3.UseCoordinatesOnClick = false;
            this.uiMainWindow3.WindowClass = "";
            // 
            // PranaMainWindow2
            // 
            this.PranaMainWindow2.Comment = null;
            this.PranaMainWindow2.Name = "PranaMainWindow2";
            this.PranaMainWindow2.ObjectImage = null;
            this.PranaMainWindow2.Parent = this.PranaApplication;
            this.PranaMainWindow2.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MainWindow;
            this.PranaMainWindow2.UseCoordinatesOnClick = false;
            this.PranaMainWindow2.WindowClass = "";
            // 
            // ControlPartOfPranaMain
            // 
            this.ControlPartOfPranaMain.BoundsInParent = new System.Drawing.Rectangle(0, 0, 50, 50);
            this.ControlPartOfPranaMain.Comment = null;
            this.ControlPartOfPranaMain.ControlPartProvider = null;
            this.ControlPartOfPranaMain.Name = "ControlPartOfPranaMain";
            this.ControlPartOfPranaMain.ObjectImage = null;
            this.ControlPartOfPranaMain.Parent = this.PranaMain;
            this.ControlPartOfPranaMain.Path = null;
            this.ControlPartOfPranaMain.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
            this.ControlPartOfPranaMain.UseCoordinatesOnClick = false;
            // 
            // ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top
            // 
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.BoundsInParent = new System.Drawing.Rectangle(1070, 0, 25, 25);
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.Comment = null;
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.ControlPartProvider = null;
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.IsOptional = true;
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.Name = "ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top";
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.ObjectImage = null;
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.Parent = this.PranaMain_UltraFormManager_Dock_Area_Top;
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.Path = null;
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.TimeOut = 3000;
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.UIObjectType = TestAutomationFX.UI.UIObjectTypes.ControlPart;
            this.ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.UseCoordinatesOnClick = false;
            // 
            // uiMainWindow4
            // 
            this.uiMainWindow4.Comment = null;
            this.uiMainWindow4.Name = "uiMainWindow4";
            this.uiMainWindow4.ObjectImage = null;
            this.uiMainWindow4.Parent = this.PranaServerApplication;
            this.uiMainWindow4.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MainWindow;
            this.uiMainWindow4.UseCoordinatesOnClick = false;
            this.uiMainWindow4.WindowClass = "";
            // 
            // PranaMainWindow3
            // 
            this.PranaMainWindow3.Comment = null;
            this.PranaMainWindow3.Name = "PranaMainWindow3";
            this.PranaMainWindow3.ObjectImage = null;
            this.PranaMainWindow3.Parent = this.PranaApplication;
            this.PranaMainWindow3.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MainWindow;
            this.PranaMainWindow3.UseCoordinatesOnClick = false;
            this.PranaMainWindow3.WindowClass = "";
            // 
            // PranaClientUIMap
            // 
            this.Settings.PlaybackSettings.AutoStartTests = true;
            this.Settings.PlaybackSettings.CloseTestFormAfterTests = true;
            this.Settings.PlaybackSettings.SaveScreenShotOnTestFailure = false;
            this.Settings.PlaybackSettings.StopOnFailedTest = false;
            this.UIMapObjectApplications.Add(this.PranaApplication);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected TestAutomationFX.UI.UIApplication PranaServerApplication;
        protected TestAutomationFX.UI.UIWindow Server;
        protected TestAutomationFX.UI.UIApplication PranaApplication;
        protected TestAutomationFX.UI.UIWindow PranaMain;
        protected TestAutomationFX.UI.UIWindow PranaMain_UltraFormManager_Dock_Area_Top;
        protected TestAutomationFX.UI.UIMsaa Close;
        protected TestAutomationFX.UI.UIMsaa Minimize;
        protected TestAutomationFX.UI.UIWindow SaveLayout;
        protected TestAutomationFX.UI.UIWindow ButtonNo;
        protected TestAutomationFX.UI.UIWindow ButtonYes;
        protected TestAutomationFX.UI.UIWindow Login;
        public TestAutomationFX.UI.UIUltraTextEditor TxtLoginID;
        public TestAutomationFX.UI.UIUltraTextEditor TxtPassword;
        public TestAutomationFX.UI.UIWindow BtnLogin;
        protected TestAutomationFX.UI.UIWindow BtnClose;
        private UIMainWindow uiMainWindow1;
        private UIMainWindow PranaMainWindow;
        private UIMainWindow uiMainWindow2;
        private UIMainWindow PranaMainWindow1;
        public UIWindow CustomMessageBox;
        public UIWindow TableLayoutPanel1;
        public UIWindow UltraPanelBottom;
        public UIWindow UltraOkButton;
        private UIMainWindow uiMainWindow3;
        private UIMainWindow PranaMainWindow2;
        private UIControlPart ControlPartOfPranaMain;
        protected UIControlPart ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top;
        private UIMainWindow uiMainWindow4;
        private UIMainWindow PranaMainWindow3;

    }
}
