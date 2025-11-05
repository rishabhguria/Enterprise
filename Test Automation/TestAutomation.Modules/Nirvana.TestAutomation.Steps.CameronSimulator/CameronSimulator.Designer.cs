using Nirvana.TestAutomation.Utilities;
namespace Nirvana.TestAutomation.Steps.Simulator
{
    partial class CameronSimulator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameronSimulator));
            this.StartFixApplication = new TestAutomationFX.UI.UIApplication();
            this.Form1 = new TestAutomationFX.UI.UIWindow();
            this.Config_TT = new TestAutomationFX.UI.UIWindow();
            this.JavaApplication = new TestAutomationFX.UI.UIApplication();
            this.SellSideLog = new TestAutomationFX.UI.UIWindow();
            this.TitleBar = new TestAutomationFX.UI.UIMsaa();
            this.StartFixMainWindow = new TestAutomationFX.UI.UIMainWindow();
            this.JavaMainWindow = new TestAutomationFX.UI.UIMainWindow();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // StartFixApplication
            // 
            this.StartFixApplication.Comment = null;
            this.StartFixApplication.ImagePath = ApplicationArguments.CameronSimulatorPath + "\\StartFix.exe";
            this.StartFixApplication.Name = "StartFixApplication";
            this.StartFixApplication.ObjectImage = null;
            this.StartFixApplication.Parent = null;
            this.StartFixApplication.ProcessName = "StartFix";
            this.StartFixApplication.TimeOut = 1000;
            this.StartFixApplication.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Application;
            this.StartFixApplication.UseCoordinatesOnClick = false;
            this.StartFixApplication.UsedMatchedProperties = ((TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties)((TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties.ProcessName | TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties.CommandLineArguments)));
            this.StartFixApplication.WorkingDirectory = ApplicationArguments.CameronSimulatorPath;
            // 
            // Form1
            // 
            this.Form1.Comment = null;
            this.Form1.InstanceName = "Form1";
            this.Form1.MatchedIndex = 0;
            this.Form1.MsaaName = "Form1";
            this.Form1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.Form1.Name = "Form1";
            this.Form1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Form1.ObjectImage")));
            this.Form1.OwnedWindow = true;
            this.Form1.Parent = this.StartFixApplication;
            this.Form1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.Form1.UseCoordinatesOnClick = true;
            this.Form1.WindowClass = "";
            // 
            // Config_TT
            // 
            this.Config_TT.Comment = null;
            this.Config_TT.Index = 1;
            this.Config_TT.InstanceName = "config_TT";
            this.Config_TT.MatchedIndex = 0;
            this.Config_TT.MsaaName = "Live TT";
            this.Config_TT.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Config_TT.Name = "Config_TT";
            this.Config_TT.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Config_TT.ObjectImage")));
            this.Config_TT.Parent = this.Form1;
            this.Config_TT.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.Config_TT.UseCoordinatesOnClick = false;
            this.Config_TT.WindowClass = "";
            this.Config_TT.WindowText = "Live TT";
            // 
            // JavaApplication
            // 
            this.JavaApplication.CommandLineArguments = resources.GetString("JavaApplication.CommandLineArguments");
            this.JavaApplication.Comment = null;
            this.JavaApplication.ImagePath = "C:\\Program Files\\Java\\jdk1.8.0_201\\bin\\java.exe";
            this.JavaApplication.Name = "JavaApplication";
            this.JavaApplication.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("JavaApplication.ObjectImage")));
            this.JavaApplication.Parent = null;
            this.JavaApplication.ProcessName = "Java";
            this.JavaApplication.TimeOut = 1000;
            this.JavaApplication.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Application;
            this.JavaApplication.UseCoordinatesOnClick = false;
            this.JavaApplication.UsedMatchedProperties = ((TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties)((TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties.ProcessName | TestAutomationFX.UI.UIApplication.UIApplicationMatchedProperties.CommandLineArguments)));
            this.JavaApplication.WorkingDirectory = "C:\\Program Files\\Java\\jdk1.8.0_201\\bin";
            // 
            // SellSideLog
            // 
            this.SellSideLog.Comment = null;
            this.SellSideLog.IsOptional = true;
            this.SellSideLog.MatchedIndex = 0;
            this.SellSideLog.MsaaName = "Sell Side Log";
            this.SellSideLog.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.SellSideLog.Name = "SellSideLog";
            this.SellSideLog.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("SellSideLog.ObjectImage")));
            this.SellSideLog.OwnedWindow = true;
            this.SellSideLog.Parent = this.JavaApplication;
            this.SellSideLog.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.SellSideLog.UseCoordinatesOnClick = true;
            this.SellSideLog.WindowClass = "SunAwtFrame";
            // 
            // TitleBar
            // 
            this.TitleBar.Comment = null;
            this.TitleBar.IsOptional = true;
            this.TitleBar.Index = 1;
            this.TitleBar.MsaaName = null;
            this.TitleBar.Name = "TitleBar";
            this.TitleBar.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("TitleBar.ObjectImage")));
            this.TitleBar.Parent = this.SellSideLog;
            this.TitleBar.Role = System.Windows.Forms.AccessibleRole.TitleBar;
            this.TitleBar.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.TitleBar.UseCoordinatesOnClick = true;
            // 
            // StartFixMainWindow
            // 
            this.StartFixMainWindow.Comment = null;
            this.StartFixMainWindow.Name = "StartFixMainWindow";
            this.StartFixMainWindow.ObjectImage = null;
            this.StartFixMainWindow.Parent = this.StartFixApplication;
            this.StartFixMainWindow.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MainWindow;
            this.StartFixMainWindow.UseCoordinatesOnClick = false;
            this.StartFixMainWindow.WindowClass = "";
            // 
            // JavaMainWindow
            // 
            this.JavaMainWindow.Comment = null;
            this.JavaMainWindow.Name = "JavaMainWindow";
            this.JavaMainWindow.ObjectImage = null;
            this.JavaMainWindow.Parent = this.JavaApplication;
            this.JavaMainWindow.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MainWindow;
            this.JavaMainWindow.UseCoordinatesOnClick = false;
            this.JavaMainWindow.WindowClass = "";
            // 
            // CameronSimulator
            // 
            this.UIMapObjectApplications.Add(this.StartFixApplication);
            this.UIMapObjectApplications.Add(this.JavaApplication);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        public TestAutomationFX.UI.UIApplication StartFixApplication;
        public TestAutomationFX.UI.UIWindow Form1;
        public TestAutomationFX.UI.UIWindow Config_TT;
        protected TestAutomationFX.UI.UIApplication JavaApplication;
        protected TestAutomationFX.UI.UIWindow SellSideLog;
        protected TestAutomationFX.UI.UIMsaa TitleBar;
        private TestAutomationFX.UI.UIMainWindow StartFixMainWindow;
        private TestAutomationFX.UI.UIMainWindow JavaMainWindow;
    }
}
