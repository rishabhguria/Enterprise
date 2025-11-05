using Nirvana.TestAutomation.Utilities;
namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    partial class DayEndCashUIMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DayEndCashUIMap));
            this.PranaApplication = new TestAutomationFX.UI.UIApplication();
            this.PranaMain = new TestAutomationFX.UI.UIWindow();
            this.UltraPanel1 = new TestAutomationFX.UI.UIWindow();
            this.UltraPanel1ClientArea = new TestAutomationFX.UI.UIWindow();
            this.MenuPrana = new TestAutomationFX.UI.UIWindow();
            this.GeneralLedger = new TestAutomationFX.UI.UIMenuItem();
            this.FrmCashManagementMain = new TestAutomationFX.UI.UIWindow();
            this.FrmCashManagementMain_Fill_Panel = new TestAutomationFX.UI.UIWindow();
            this.FrmCashManagementMain_Fill_PanelClientArea = new TestAutomationFX.UI.UIWindow();
            this.TbCtrlMain = new TestAutomationFX.UI.UIWindow();
            this.DayEndCash = new TestAutomationFX.UI.UIMsaa();
            this.UltraTabPageControl2 = new TestAutomationFX.UI.UIWindow();
            this.CtrlCashForm = new TestAutomationFX.UI.UIWindow();
            this.UltraPanel11 = new TestAutomationFX.UI.UIWindow();
            this.UltraPanel1ClientArea1 = new TestAutomationFX.UI.UIWindow();
            this.UgbxDayEndParams = new TestAutomationFX.UI.UIWindow();
            this.CtrlMasterFundAndAccountsDropdown1 = new TestAutomationFX.UI.UIWindow();
            this.CmbMasterFund = new TestAutomationFX.UI.UIWindow();
            this.MultiSelectEditor = new TestAutomationFX.UI.UIUltraTextEditor();
            this.CmbMultiAccounts = new TestAutomationFX.UI.UIWindow();
            this.MultiSelectEditor1 = new TestAutomationFX.UI.UIUltraTextEditor();
            this.DtFromDate = new TestAutomationFX.UI.UIWindow();
            this.Textarea = new TestAutomationFX.UI.UIMsaa();
            this.Open = new TestAutomationFX.UI.UIMsaa();
            this.DtToDate = new TestAutomationFX.UI.UIWindow();
            this.Textarea1 = new TestAutomationFX.UI.UIMsaa();
            this.BtnGet = new TestAutomationFX.UI.UIWindow();
            this.BtnRunBatch = new TestAutomationFX.UI.UIWindow();
            this.BtnSave = new TestAutomationFX.UI.UIWindow();
            this.Window = new TestAutomationFX.UI.UIWindow();
            this.MonthDropDown = new TestAutomationFX.UI.UIWindow();
            this.DayEndCash1 = new TestAutomationFX.UI.UIWindow();
            this.ButtonYes = new TestAutomationFX.UI.UIWindow();
            this.SplitContainer2 = new TestAutomationFX.UI.UIWindow();
            this.Panel1 = new TestAutomationFX.UI.UIWindow();
            this.SplitContainer1 = new TestAutomationFX.UI.UIWindow();
            this.Panel2 = new TestAutomationFX.UI.UIWindow();
            this.PnltabDayEnd = new TestAutomationFX.UI.UIWindow();
            this.TabCntlDayEndData = new TestAutomationFX.UI.UIWindow();
            this.DayEndCash2 = new TestAutomationFX.UI.UIMsaa();
            this.uiWindow1 = new TestAutomationFX.UI.UIWindow();
            this.UltraGrid = new TestAutomationFX.UI.UIUltraGrid();
            this.ColumnHeaders = new TestAutomationFX.UI.UIMsaa();
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top = new TestAutomationFX.UI.UIWindow();
            this.UltraPanel2 = new TestAutomationFX.UI.UIWindow();
            this.UltraPanel2ClientArea = new TestAutomationFX.UI.UIWindow();
            this.ClientArea_Toolbars_Dock_Area_Top = new TestAutomationFX.UI.UIWindow();
            this.GeneralLedger1 = new TestAutomationFX.UI.UIMsaa();
            this.NAVLock = new TestAutomationFX.UI.UIWindow();
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
            // UltraPanel1
            // 
            this.UltraPanel1.Comment = null;
            this.UltraPanel1.Index = 1;
            this.UltraPanel1.InstanceName = "ultraPanel1";
            this.UltraPanel1.MatchedIndex = 0;
            this.UltraPanel1.MsaaName = null;
            this.UltraPanel1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanel1.Name = "UltraPanel1";
            this.UltraPanel1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanel1.ObjectImage")));
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
            this.UltraPanel1ClientArea.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanel1ClientArea.ObjectImage")));
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
            this.MenuPrana.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("MenuPrana.ObjectImage")));
            this.MenuPrana.Parent = this.UltraPanel1ClientArea;
            this.MenuPrana.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.MenuPrana.UseCoordinatesOnClick = true;
            this.MenuPrana.WindowClass = "";
            // 
            // GeneralLedger
            // 
            this.GeneralLedger.Comment = null;
            this.GeneralLedger.MsaaName = "General Ledger";
            this.GeneralLedger.Name = "GeneralLedger";
            this.GeneralLedger.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("GeneralLedger.ObjectImage")));
            this.GeneralLedger.Parent = this.MenuPrana;
            this.GeneralLedger.Role = System.Windows.Forms.AccessibleRole.MenuItem;
            this.GeneralLedger.UIObjectType = TestAutomationFX.UI.UIObjectTypes.MenuItem;
            this.GeneralLedger.UseCoordinatesOnClick = false;
            // 
            // FrmCashManagementMain
            // 
            this.FrmCashManagementMain.Comment = null;
            this.FrmCashManagementMain.InstanceName = "frmCashManagementMain";
            this.FrmCashManagementMain.MatchedIndex = 0;
            this.FrmCashManagementMain.MsaaName = "General Ledger";
            this.FrmCashManagementMain.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.FrmCashManagementMain.Name = "FrmCashManagementMain";
            this.FrmCashManagementMain.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("FrmCashManagementMain.ObjectImage")));
            this.FrmCashManagementMain.OwnedWindow = true;
            this.FrmCashManagementMain.Parent = this.PranaMain;
            this.FrmCashManagementMain.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.FrmCashManagementMain.UseCoordinatesOnClick = true;
            this.FrmCashManagementMain.WindowClass = "";
            this.FrmCashManagementMain.WindowText = "General Ledger";
            // 
            // FrmCashManagementMain_Fill_Panel
            // 
            this.FrmCashManagementMain_Fill_Panel.Comment = null;
            this.FrmCashManagementMain_Fill_Panel.Index = 0;
            this.FrmCashManagementMain_Fill_Panel.InstanceName = "frmCashManagementMain_Fill_Panel";
            this.FrmCashManagementMain_Fill_Panel.MatchedIndex = 0;
            this.FrmCashManagementMain_Fill_Panel.MsaaName = null;
            this.FrmCashManagementMain_Fill_Panel.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.FrmCashManagementMain_Fill_Panel.Name = "FrmCashManagementMain_Fill_Panel";
            this.FrmCashManagementMain_Fill_Panel.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("FrmCashManagementMain_Fill_Panel.ObjectImage")));
            this.FrmCashManagementMain_Fill_Panel.Parent = this.FrmCashManagementMain;
            this.FrmCashManagementMain_Fill_Panel.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.FrmCashManagementMain_Fill_Panel.UseCoordinatesOnClick = true;
            this.FrmCashManagementMain_Fill_Panel.WindowClass = "";
            // 
            // FrmCashManagementMain_Fill_PanelClientArea
            // 
            this.FrmCashManagementMain_Fill_PanelClientArea.Comment = null;
            this.FrmCashManagementMain_Fill_PanelClientArea.Index = 0;
            this.FrmCashManagementMain_Fill_PanelClientArea.InstanceName = "frmCashManagementMain_Fill_Panel.ClientArea";
            this.FrmCashManagementMain_Fill_PanelClientArea.MatchedIndex = 0;
            this.FrmCashManagementMain_Fill_PanelClientArea.MsaaName = null;
            this.FrmCashManagementMain_Fill_PanelClientArea.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.FrmCashManagementMain_Fill_PanelClientArea.Name = "FrmCashManagementMain_Fill_PanelClientArea";
            this.FrmCashManagementMain_Fill_PanelClientArea.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("FrmCashManagementMain_Fill_PanelClientArea.ObjectImage")));
            this.FrmCashManagementMain_Fill_PanelClientArea.Parent = this.FrmCashManagementMain_Fill_Panel;
            this.FrmCashManagementMain_Fill_PanelClientArea.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.FrmCashManagementMain_Fill_PanelClientArea.UseCoordinatesOnClick = true;
            this.FrmCashManagementMain_Fill_PanelClientArea.WindowClass = "";
            // 
            // TbCtrlMain
            // 
            this.TbCtrlMain.Comment = null;
            this.TbCtrlMain.Index = 0;
            this.TbCtrlMain.InstanceName = "tbCtrlMain";
            this.TbCtrlMain.MatchedIndex = 0;
            this.TbCtrlMain.MsaaName = null;
            this.TbCtrlMain.MsaaRole = System.Windows.Forms.AccessibleRole.PageTabList;
            this.TbCtrlMain.Name = "TbCtrlMain";
            this.TbCtrlMain.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("TbCtrlMain.ObjectImage")));
            this.TbCtrlMain.Parent = this.FrmCashManagementMain_Fill_PanelClientArea;
            this.TbCtrlMain.UIObjectType = TestAutomationFX.UI.UIObjectTypes.TabControl;
            this.TbCtrlMain.UseCoordinatesOnClick = true;
            this.TbCtrlMain.WindowClass = "";
            // 
            // DayEndCash
            // 
            this.DayEndCash.Comment = null;
            this.DayEndCash.MsaaName = "Day End Cash";
            this.DayEndCash.Name = "DayEndCash";
            this.DayEndCash.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("DayEndCash.ObjectImage")));
            this.DayEndCash.Parent = this.TbCtrlMain;
            this.DayEndCash.Role = System.Windows.Forms.AccessibleRole.PageTab;
            this.DayEndCash.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.DayEndCash.UseCoordinatesOnClick = true;
            // 
            // UltraTabPageControl2
            // 
            this.UltraTabPageControl2.Comment = null;
            this.UltraTabPageControl2.Index = 2;
            this.UltraTabPageControl2.InstanceName = "ultraTabPageControl2";
            this.UltraTabPageControl2.MatchedIndex = 0;
            this.UltraTabPageControl2.MsaaName = null;
            this.UltraTabPageControl2.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraTabPageControl2.Name = "UltraTabPageControl2";
            this.UltraTabPageControl2.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraTabPageControl2.ObjectImage")));
            this.UltraTabPageControl2.Parent = this.TbCtrlMain;
            this.UltraTabPageControl2.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraTabPageControl2.UseCoordinatesOnClick = true;
            this.UltraTabPageControl2.WindowClass = "";
            // 
            // CtrlCashForm
            // 
            this.CtrlCashForm.Comment = null;
            this.CtrlCashForm.Index = 0;
            this.CtrlCashForm.InstanceName = "ctrlCashForm";
            this.CtrlCashForm.MatchedIndex = 0;
            this.CtrlCashForm.MsaaName = null;
            this.CtrlCashForm.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.CtrlCashForm.Name = "CtrlCashForm";
            this.CtrlCashForm.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("CtrlCashForm.ObjectImage")));
            this.CtrlCashForm.Parent = this.UltraTabPageControl2;
            this.CtrlCashForm.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CtrlCashForm.UseCoordinatesOnClick = true;
            this.CtrlCashForm.WindowClass = "";
            // 
            // UltraPanel11
            // 
            this.UltraPanel11.Comment = null;
            this.UltraPanel11.Index = 0;
            this.UltraPanel11.InstanceName = "ultraPanel1";
            this.UltraPanel11.MatchedIndex = 0;
            this.UltraPanel11.MsaaName = null;
            this.UltraPanel11.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanel11.Name = "UltraPanel11";
            this.UltraPanel11.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanel11.ObjectImage")));
            this.UltraPanel11.Parent = this.CtrlCashForm;
            this.UltraPanel11.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraPanel11.UseCoordinatesOnClick = true;
            this.UltraPanel11.WindowClass = "";
            // 
            // UltraPanel1ClientArea1
            // 
            this.UltraPanel1ClientArea1.Comment = null;
            this.UltraPanel1ClientArea1.Index = 0;
            this.UltraPanel1ClientArea1.InstanceName = "ultraPanel1.ClientArea";
            this.UltraPanel1ClientArea1.MatchedIndex = 0;
            this.UltraPanel1ClientArea1.MsaaName = null;
            this.UltraPanel1ClientArea1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanel1ClientArea1.Name = "UltraPanel1ClientArea1";
            this.UltraPanel1ClientArea1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanel1ClientArea1.ObjectImage")));
            this.UltraPanel1ClientArea1.Parent = this.UltraPanel11;
            this.UltraPanel1ClientArea1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraPanel1ClientArea1.UseCoordinatesOnClick = true;
            this.UltraPanel1ClientArea1.WindowClass = "";
            // 
            // UgbxDayEndParams
            // 
            this.UgbxDayEndParams.Comment = null;
            this.UgbxDayEndParams.Index = 2;
            this.UgbxDayEndParams.InstanceName = "ugbxDayEndParams";
            this.UgbxDayEndParams.MatchedIndex = 0;
            this.UgbxDayEndParams.MsaaRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.UgbxDayEndParams.Name = "UgbxDayEndParams";
            this.UgbxDayEndParams.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UgbxDayEndParams.ObjectImage")));
            this.UgbxDayEndParams.Parent = this.UltraPanel1ClientArea1;
            this.UgbxDayEndParams.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UgbxDayEndParams.UseCoordinatesOnClick = true;
            this.UgbxDayEndParams.WindowClass = "";
            // 
            // CtrlMasterFundAndAccountsDropdown1
            // 
            this.CtrlMasterFundAndAccountsDropdown1.Comment = null;
            this.CtrlMasterFundAndAccountsDropdown1.Index = 0;
            this.CtrlMasterFundAndAccountsDropdown1.InstanceName = "ctrlMasterFundAndAccountsDropdown1";
            this.CtrlMasterFundAndAccountsDropdown1.MatchedIndex = 0;
            this.CtrlMasterFundAndAccountsDropdown1.MsaaName = null;
            this.CtrlMasterFundAndAccountsDropdown1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.CtrlMasterFundAndAccountsDropdown1.Name = "CtrlMasterFundAndAccountsDropdown1";
            this.CtrlMasterFundAndAccountsDropdown1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("CtrlMasterFundAndAccountsDropdown1.ObjectImage")));
            this.CtrlMasterFundAndAccountsDropdown1.Parent = this.UgbxDayEndParams;
            this.CtrlMasterFundAndAccountsDropdown1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CtrlMasterFundAndAccountsDropdown1.UseCoordinatesOnClick = true;
            this.CtrlMasterFundAndAccountsDropdown1.WindowClass = "";
            // 
            // CmbMasterFund
            // 
            this.CmbMasterFund.Comment = null;
            this.CmbMasterFund.Index = 1;
            this.CmbMasterFund.InstanceName = "cmbMasterFund";
            this.CmbMasterFund.MatchedIndex = 0;
            this.CmbMasterFund.MsaaName = null;
            this.CmbMasterFund.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.CmbMasterFund.Name = "CmbMasterFund";
            this.CmbMasterFund.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("CmbMasterFund.ObjectImage")));
            this.CmbMasterFund.Parent = this.CtrlMasterFundAndAccountsDropdown1;
            this.CmbMasterFund.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CmbMasterFund.UseCoordinatesOnClick = true;
            this.CmbMasterFund.WindowClass = "";
            // 
            // MultiSelectEditor
            // 
            this.MultiSelectEditor.AllowChildren = false;
            this.MultiSelectEditor.Comment = null;
            this.MultiSelectEditor.Index = 0;
            this.MultiSelectEditor.InstanceName = "MultiSelectEditor";
            this.MultiSelectEditor.MatchedIndex = 0;
            this.MultiSelectEditor.MsaaName = null;
            this.MultiSelectEditor.Name = "MultiSelectEditor";
            this.MultiSelectEditor.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("MultiSelectEditor.ObjectImage")));
            this.MultiSelectEditor.Parent = this.CmbMasterFund;
            this.MultiSelectEditor.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.MultiSelectEditor.UseCoordinatesOnClick = true;
            this.MultiSelectEditor.WindowClass = "";
            // 
            // CmbMultiAccounts
            // 
            this.CmbMultiAccounts.Comment = null;
            this.CmbMultiAccounts.Index = 3;
            this.CmbMultiAccounts.InstanceName = "cmbMultiAccounts";
            this.CmbMultiAccounts.MatchedIndex = 0;
            this.CmbMultiAccounts.MsaaName = null;
            this.CmbMultiAccounts.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.CmbMultiAccounts.Name = "CmbMultiAccounts";
            this.CmbMultiAccounts.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("CmbMultiAccounts.ObjectImage")));
            this.CmbMultiAccounts.Parent = this.CtrlMasterFundAndAccountsDropdown1;
            this.CmbMultiAccounts.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.CmbMultiAccounts.UseCoordinatesOnClick = true;
            this.CmbMultiAccounts.WindowClass = "";
            // 
            // MultiSelectEditor1
            // 
            this.MultiSelectEditor1.AllowChildren = false;
            this.MultiSelectEditor1.Comment = null;
            this.MultiSelectEditor1.Index = 0;
            this.MultiSelectEditor1.InstanceName = "MultiSelectEditor";
            this.MultiSelectEditor1.MatchedIndex = 0;
            this.MultiSelectEditor1.MsaaName = null;
            this.MultiSelectEditor1.Name = "MultiSelectEditor1";
            this.MultiSelectEditor1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("MultiSelectEditor1.ObjectImage")));
            this.MultiSelectEditor1.Parent = this.CmbMultiAccounts;
            this.MultiSelectEditor1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.MultiSelectEditor1.UseCoordinatesOnClick = true;
            this.MultiSelectEditor1.WindowClass = "";
            // 
            // DtFromDate
            // 
            this.DtFromDate.Comment = null;
            this.DtFromDate.Index = 1;
            this.DtFromDate.InstanceName = "dtFromDate";
            this.DtFromDate.MatchedIndex = 0;
            this.DtFromDate.MsaaName = "02/03/2017";
            this.DtFromDate.MsaaRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.DtFromDate.Name = "DtFromDate";
            this.DtFromDate.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("DtFromDate.ObjectImage")));
            this.DtFromDate.Parent = this.UgbxDayEndParams;
            this.DtFromDate.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.DtFromDate.UseCoordinatesOnClick = true;
            this.DtFromDate.WindowClass = "";
            // 
            // Textarea
            // 
            this.Textarea.Comment = null;
            this.Textarea.MsaaName = "Text area";
            this.Textarea.Name = "Textarea";
            this.Textarea.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Textarea.ObjectImage")));
            this.Textarea.Parent = this.DtFromDate;
            this.Textarea.Role = System.Windows.Forms.AccessibleRole.Text;
            this.Textarea.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Label;
            this.Textarea.UseCoordinatesOnClick = true;
            // 
            // Open
            // 
            this.Open.Comment = null;
            this.Open.MsaaName = "Open";
            this.Open.Name = "Open";
            this.Open.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Open.ObjectImage")));
            this.Open.Parent = this.DtFromDate;
            this.Open.Role = System.Windows.Forms.AccessibleRole.ButtonDropDown;
            this.Open.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.Open.UseCoordinatesOnClick = true;
            // 
            // DtToDate
            // 
            this.DtToDate.Comment = null;
            this.DtToDate.Index = 4;
            this.DtToDate.InstanceName = "dtToDate";
            this.DtToDate.MatchedIndex = 0;
            this.DtToDate.MsaaName = "02/03/2017";
            this.DtToDate.MsaaRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.DtToDate.Name = "DtToDate";
            this.DtToDate.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("DtToDate.ObjectImage")));
            this.DtToDate.Parent = this.UgbxDayEndParams;
            this.DtToDate.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.DtToDate.UseCoordinatesOnClick = true;
            this.DtToDate.WindowClass = "";
            // 
            // Textarea1
            // 
            this.Textarea1.Comment = null;
            this.Textarea1.MsaaName = "Text area";
            this.Textarea1.Name = "Textarea1";
            this.Textarea1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Textarea1.ObjectImage")));
            this.Textarea1.Parent = this.DtToDate;
            this.Textarea1.Role = System.Windows.Forms.AccessibleRole.Text;
            this.Textarea1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Label;
            this.Textarea1.UseCoordinatesOnClick = true;
            // 
            // BtnGet
            // 
            this.BtnGet.Comment = null;
            this.BtnGet.Index = 7;
            this.BtnGet.InstanceName = "btnGet";
            this.BtnGet.MatchedIndex = 0;
            this.BtnGet.MsaaName = "Get Data";
            this.BtnGet.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnGet.Name = "BtnGet";
            this.BtnGet.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BtnGet.ObjectImage")));
            this.BtnGet.Parent = this.UgbxDayEndParams;
            this.BtnGet.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.BtnGet.UseCoordinatesOnClick = false;
            this.BtnGet.WindowClass = "";
            this.BtnGet.WindowText = "Get Data";
            // 
            // BtnRunBatch
            // 
            this.BtnRunBatch.Comment = null;
            this.BtnRunBatch.Index = 6;
            this.BtnRunBatch.InstanceName = "btnRunBatch";
            this.BtnRunBatch.MatchedIndex = 0;
            this.BtnRunBatch.MsaaName = "Calculate";
            this.BtnRunBatch.MsaaRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BtnRunBatch.Name = "BtnRunBatch";
            this.BtnRunBatch.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BtnRunBatch.ObjectImage")));
            this.BtnRunBatch.Parent = this.UgbxDayEndParams;
            this.BtnRunBatch.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.BtnRunBatch.UseCoordinatesOnClick = false;
            this.BtnRunBatch.WindowClass = "";
            this.BtnRunBatch.WindowText = "Calculate";
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
            this.BtnSave.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("BtnSave.ObjectImage")));
            this.BtnSave.Parent = this.UgbxDayEndParams;
            this.BtnSave.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.BtnSave.UseCoordinatesOnClick = false;
            this.BtnSave.WindowClass = "";
            this.BtnSave.WindowText = "Save";
            // 
            // Window
            // 
            this.Window.Comment = null;
            this.Window.MatchedIndex = 1;
            this.Window.MsaaName = null;
            this.Window.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.Window.Name = "Window";
            this.Window.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Window.ObjectImage")));
            this.Window.OwnedWindow = true;
            this.Window.Parent = this.FrmCashManagementMain;
            this.Window.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.Window.UseCoordinatesOnClick = true;
            this.Window.WindowClass = "";
            // 
            // MonthDropDown
            // 
            this.MonthDropDown.Comment = null;
            this.MonthDropDown.Index = 0;
            this.MonthDropDown.InstanceName = "MonthDropDown";
            this.MonthDropDown.MatchedIndex = 0;
            this.MonthDropDown.MsaaName = null;
            this.MonthDropDown.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.MonthDropDown.Name = "MonthDropDown";
            this.MonthDropDown.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("MonthDropDown.ObjectImage")));
            this.MonthDropDown.Parent = this.Window;
            this.MonthDropDown.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.MonthDropDown.UseCoordinatesOnClick = true;
            this.MonthDropDown.WindowClass = "";
            // 
            // DayEndCash1
            // 
            this.DayEndCash1.Comment = null;
            this.DayEndCash1.Index = 0;
            this.DayEndCash1.IsOptional = true;
            this.DayEndCash1.MatchedIndex = 0;
            this.DayEndCash1.MsaaName = "DayEndCash";
            this.DayEndCash1.MsaaRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.DayEndCash1.Name = "DayEndCash1";
            this.DayEndCash1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("DayEndCash1.ObjectImage")));
            this.DayEndCash1.OwnedWindow = true;
            this.DayEndCash1.Parent = this.FrmCashManagementMain;
            this.DayEndCash1.TimeOut = 500;
            this.DayEndCash1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.DayEndCash1.UseCoordinatesOnClick = true;
            this.DayEndCash1.WindowClass = "#32770";
            this.DayEndCash1.WindowText = "DayEndCash";
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
            this.ButtonYes.Parent = this.DayEndCash1;
            this.ButtonYes.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.ButtonYes.UseCoordinatesOnClick = false;
            this.ButtonYes.WindowClass = "Button";
            this.ButtonYes.WindowText = "&Yes";
            // 
            // SplitContainer2
            // 
            this.SplitContainer2.Comment = null;
            this.SplitContainer2.Index = 0;
            this.SplitContainer2.InstanceName = "splitContainer2";
            this.SplitContainer2.MatchedIndex = 0;
            this.SplitContainer2.MsaaName = null;
            this.SplitContainer2.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.SplitContainer2.Name = "SplitContainer2";
            this.SplitContainer2.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("SplitContainer2.ObjectImage")));
            this.SplitContainer2.Parent = this.UltraPanel1ClientArea1;
            this.SplitContainer2.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.SplitContainer2.UseCoordinatesOnClick = true;
            this.SplitContainer2.WindowClass = "";
            // 
            // Panel1
            // 
            this.Panel1.Comment = null;
            this.Panel1.Index = 0;
            this.Panel1.InstanceName = "panel1";
            this.Panel1.MatchedIndex = 0;
            this.Panel1.MsaaName = null;
            this.Panel1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.Panel1.Name = "Panel1";
            this.Panel1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Panel1.ObjectImage")));
            this.Panel1.Parent = this.SplitContainer2;
            this.Panel1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.Panel1.UseCoordinatesOnClick = true;
            this.Panel1.WindowClass = "";
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Comment = null;
            this.SplitContainer1.Index = 0;
            this.SplitContainer1.InstanceName = "splitContainer1";
            this.SplitContainer1.MatchedIndex = 0;
            this.SplitContainer1.MsaaName = null;
            this.SplitContainer1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.SplitContainer1.Name = "SplitContainer1";
            this.SplitContainer1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("SplitContainer1.ObjectImage")));
            this.SplitContainer1.Parent = this.Panel1;
            this.SplitContainer1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.SplitContainer1.UseCoordinatesOnClick = true;
            this.SplitContainer1.WindowClass = "";
            // 
            // Panel2
            // 
            this.Panel2.Comment = null;
            this.Panel2.Index = 1;
            this.Panel2.InstanceName = "panel2";
            this.Panel2.MatchedIndex = 0;
            this.Panel2.MsaaName = null;
            this.Panel2.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.Panel2.Name = "Panel2";
            this.Panel2.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("Panel2.ObjectImage")));
            this.Panel2.Parent = this.SplitContainer1;
            this.Panel2.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.Panel2.UseCoordinatesOnClick = true;
            this.Panel2.WindowClass = "";
            // 
            // PnltabDayEnd
            // 
            this.PnltabDayEnd.Comment = null;
            this.PnltabDayEnd.Index = 0;
            this.PnltabDayEnd.InstanceName = "pnltabDayEnd";
            this.PnltabDayEnd.MatchedIndex = 0;
            this.PnltabDayEnd.MsaaName = null;
            this.PnltabDayEnd.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.PnltabDayEnd.Name = "PnltabDayEnd";
            this.PnltabDayEnd.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("PnltabDayEnd.ObjectImage")));
            this.PnltabDayEnd.Parent = this.Panel2;
            this.PnltabDayEnd.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.PnltabDayEnd.UseCoordinatesOnClick = true;
            this.PnltabDayEnd.WindowClass = "";
            // 
            // TabCntlDayEndData
            // 
            this.TabCntlDayEndData.Comment = null;
            this.TabCntlDayEndData.Index = 0;
            this.TabCntlDayEndData.InstanceName = "tabCntlDayEndData";
            this.TabCntlDayEndData.MatchedIndex = 0;
            this.TabCntlDayEndData.MsaaName = "Today Day End";
            this.TabCntlDayEndData.MsaaRole = System.Windows.Forms.AccessibleRole.PageTabList;
            this.TabCntlDayEndData.Name = "TabCntlDayEndData";
            this.TabCntlDayEndData.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("TabCntlDayEndData.ObjectImage")));
            this.TabCntlDayEndData.Parent = this.PnltabDayEnd;
            this.TabCntlDayEndData.UIObjectType = TestAutomationFX.UI.UIObjectTypes.TabControl;
            this.TabCntlDayEndData.UseCoordinatesOnClick = true;
            this.TabCntlDayEndData.WindowClass = "";
            // 
            // DayEndCash2
            // 
            this.DayEndCash2.Comment = null;
            this.DayEndCash2.MsaaName = "Day End Cash";
            this.DayEndCash2.Name = "DayEndCash2";
            this.DayEndCash2.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("DayEndCash2.ObjectImage")));
            this.DayEndCash2.Parent = this.TabCntlDayEndData;
            this.DayEndCash2.Role = System.Windows.Forms.AccessibleRole.PageTab;
            this.DayEndCash2.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.DayEndCash2.UseCoordinatesOnClick = true;
            // 
            // uiWindow1
            // 
            this.uiWindow1.Comment = null;
            this.uiWindow1.Index = 1;
            this.uiWindow1.MatchedIndex = 1;
            this.uiWindow1.MsaaName = null;
            this.uiWindow1.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.uiWindow1.Name = "uiWindow1";
            this.uiWindow1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("uiWindow1.ObjectImage")));
            this.uiWindow1.Parent = this.TabCntlDayEndData;
            this.uiWindow1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.uiWindow1.UseCoordinatesOnClick = true;
            this.uiWindow1.WindowClass = "";
            // 
            // UltraGrid
            // 
            this.UltraGrid.Comment = null;
            this.UltraGrid.Index = 0;
            this.UltraGrid.InstanceName = "ultraGrid";
            this.UltraGrid.MatchedIndex = 0;
            this.UltraGrid.MsaaName = null;
            this.UltraGrid.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraGrid.Name = "UltraGrid";
            this.UltraGrid.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraGrid.ObjectImage")));
            this.UltraGrid.Parent = this.uiWindow1;
            this.UltraGrid.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Grid;
            this.UltraGrid.UseCoordinatesOnClick = true;
            this.UltraGrid.WindowClass = "";
            // 
            // ColumnHeaders
            // 
            this.ColumnHeaders.Comment = null;
            this.ColumnHeaders.MsaaName = "Column Headers";
            this.ColumnHeaders.Name = "ColumnHeaders";
            this.ColumnHeaders.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("ColumnHeaders.ObjectImage")));
            this.ColumnHeaders.Parent = this.UltraGrid;
            this.ColumnHeaders.Role = System.Windows.Forms.AccessibleRole.Grouping;
            this.ColumnHeaders.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
            this.ColumnHeaders.UseCoordinatesOnClick = true;
            // 
            // FrmCashManagementMain_UltraFormManager_Dock_Area_Top
            // 
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.Comment = null;
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.Index = 3;
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.InstanceName = "_frmCashManagementMain_UltraFormManager_Dock_Area_Top";
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.MatchedIndex = 0;
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.MsaaName = "DockTop";
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.MsaaRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.Name = "FrmCashManagementMain_UltraFormManager_Dock_Area_Top";
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("FrmCashManagementMain_UltraFormManager_Dock_Area_Top.ObjectImage")));
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.Parent = this.FrmCashManagementMain;
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.UseCoordinatesOnClick = true;
            this.FrmCashManagementMain_UltraFormManager_Dock_Area_Top.WindowClass = "";
            // 
            // UltraPanel2
            // 
            this.UltraPanel2.Comment = null;
            this.UltraPanel2.Index = 0;
            this.UltraPanel2.InstanceName = "ultraPanel2";
            this.UltraPanel2.MatchedIndex = 0;
            this.UltraPanel2.MsaaName = null;
            this.UltraPanel2.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanel2.Name = "UltraPanel2";
            this.UltraPanel2.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanel2.ObjectImage")));
            this.UltraPanel2.Parent = this.PranaMain;
            this.UltraPanel2.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraPanel2.UseCoordinatesOnClick = true;
            this.UltraPanel2.WindowClass = "";
            // 
            // UltraPanel2ClientArea
            // 
            this.UltraPanel2ClientArea.Comment = null;
            this.UltraPanel2ClientArea.Index = 0;
            this.UltraPanel2ClientArea.InstanceName = "ultraPanel2.ClientArea";
            this.UltraPanel2ClientArea.MatchedIndex = 0;
            this.UltraPanel2ClientArea.MsaaName = null;
            this.UltraPanel2ClientArea.MsaaRole = System.Windows.Forms.AccessibleRole.Client;
            this.UltraPanel2ClientArea.Name = "UltraPanel2ClientArea";
            this.UltraPanel2ClientArea.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("UltraPanel2ClientArea.ObjectImage")));
            this.UltraPanel2ClientArea.Parent = this.UltraPanel2;
            this.UltraPanel2ClientArea.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.UltraPanel2ClientArea.UseCoordinatesOnClick = true;
            this.UltraPanel2ClientArea.WindowClass = "";
            // 
            // ClientArea_Toolbars_Dock_Area_Top
            // 
            this.ClientArea_Toolbars_Dock_Area_Top.Comment = null;
            this.ClientArea_Toolbars_Dock_Area_Top.Index = 6;
            this.ClientArea_Toolbars_Dock_Area_Top.InstanceName = "_ClientArea_Toolbars_Dock_Area_Top";
            this.ClientArea_Toolbars_Dock_Area_Top.MatchedIndex = 0;
            this.ClientArea_Toolbars_Dock_Area_Top.MsaaName = "DockTop";
            this.ClientArea_Toolbars_Dock_Area_Top.MsaaRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.ClientArea_Toolbars_Dock_Area_Top.Name = "ClientArea_Toolbars_Dock_Area_Top";
            this.ClientArea_Toolbars_Dock_Area_Top.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("ClientArea_Toolbars_Dock_Area_Top.ObjectImage")));
            this.ClientArea_Toolbars_Dock_Area_Top.Parent = this.UltraPanel2ClientArea;
            this.ClientArea_Toolbars_Dock_Area_Top.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.ClientArea_Toolbars_Dock_Area_Top.UseCoordinatesOnClick = true;
            this.ClientArea_Toolbars_Dock_Area_Top.WindowClass = "";
            // 
            // GeneralLedger1
            // 
            this.GeneralLedger1.Comment = null;
            this.GeneralLedger1.MsaaName = "General Ledger";
            this.GeneralLedger1.Name = "GeneralLedger1";
            this.GeneralLedger1.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("GeneralLedger1.ObjectImage")));
            this.GeneralLedger1.Parent = this.ClientArea_Toolbars_Dock_Area_Top;
            this.GeneralLedger1.Role = System.Windows.Forms.AccessibleRole.PushButton;
            this.GeneralLedger1.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Button;
            this.GeneralLedger1.UseCoordinatesOnClick = false;
            // 
            // NAVLock
            // 
            this.NAVLock.Comment = null;
            this.NAVLock.Index = 0;
            this.NAVLock.MatchedIndex = 0;
            this.NAVLock.MsaaName = "NAV Lock";
            this.NAVLock.MsaaRole = System.Windows.Forms.AccessibleRole.Dialog;
            this.NAVLock.Name = "NAVLock";
            this.NAVLock.ObjectImage = ((System.Drawing.Bitmap)(resources.GetObject("NAVLock.ObjectImage")));
            this.NAVLock.OwnedWindow = true;
            this.NAVLock.Parent = this.FrmCashManagementMain;
            this.NAVLock.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Window;
            this.NAVLock.UseCoordinatesOnClick = true;
            this.NAVLock.WindowClass = "#32770";
            this.NAVLock.WindowText = "NAV Lock";
            this.NAVLock.IsOptional = true;
            // 
            // DayEndCashUIMap
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
        protected TestAutomationFX.UI.UIMenuItem GeneralLedger;
        protected TestAutomationFX.UI.UIWindow FrmCashManagementMain;
        protected TestAutomationFX.UI.UIWindow FrmCashManagementMain_Fill_Panel;
        protected TestAutomationFX.UI.UIWindow FrmCashManagementMain_Fill_PanelClientArea;
        protected TestAutomationFX.UI.UIWindow TbCtrlMain;
        protected TestAutomationFX.UI.UIMsaa DayEndCash;
        protected TestAutomationFX.UI.UIWindow UltraTabPageControl2;
        protected TestAutomationFX.UI.UIWindow CtrlCashForm;
        protected TestAutomationFX.UI.UIWindow UltraPanel11;
        protected TestAutomationFX.UI.UIWindow UltraPanel1ClientArea1;
        protected TestAutomationFX.UI.UIWindow UgbxDayEndParams;
        protected TestAutomationFX.UI.UIWindow CtrlMasterFundAndAccountsDropdown1;
        protected TestAutomationFX.UI.UIWindow CmbMasterFund;
        protected TestAutomationFX.UI.UIUltraTextEditor MultiSelectEditor;
        protected TestAutomationFX.UI.UIWindow CmbMultiAccounts;
        protected TestAutomationFX.UI.UIUltraTextEditor MultiSelectEditor1;
        protected TestAutomationFX.UI.UIWindow DtFromDate;
        protected TestAutomationFX.UI.UIMsaa Textarea;
        protected TestAutomationFX.UI.UIMsaa Open;
        protected TestAutomationFX.UI.UIWindow DtToDate;
        protected TestAutomationFX.UI.UIMsaa Textarea1;
        protected TestAutomationFX.UI.UIWindow BtnGet;
        protected TestAutomationFX.UI.UIWindow BtnRunBatch;
        protected TestAutomationFX.UI.UIWindow BtnSave;
        protected TestAutomationFX.UI.UIWindow Window;
        protected TestAutomationFX.UI.UIWindow MonthDropDown;
        protected TestAutomationFX.UI.UIWindow DayEndCash1;
        protected TestAutomationFX.UI.UIWindow ButtonYes;
        protected TestAutomationFX.UI.UIWindow SplitContainer2;
        protected TestAutomationFX.UI.UIWindow Panel1;
        protected TestAutomationFX.UI.UIWindow SplitContainer1;
        protected TestAutomationFX.UI.UIWindow Panel2;
        protected TestAutomationFX.UI.UIWindow PnltabDayEnd;
        protected TestAutomationFX.UI.UIWindow TabCntlDayEndData;
        protected TestAutomationFX.UI.UIMsaa DayEndCash2;
        protected TestAutomationFX.UI.UIWindow uiWindow1;
        protected TestAutomationFX.UI.UIUltraGrid UltraGrid;
        protected TestAutomationFX.UI.UIMsaa ColumnHeaders;
        protected TestAutomationFX.UI.UIWindow FrmCashManagementMain_UltraFormManager_Dock_Area_Top;
        private TestAutomationFX.UI.UIWindow UltraPanel2;
        private TestAutomationFX.UI.UIWindow UltraPanel2ClientArea;
        private TestAutomationFX.UI.UIWindow ClientArea_Toolbars_Dock_Area_Top;
        protected TestAutomationFX.UI.UIMsaa GeneralLedger1;
        protected TestAutomationFX.UI.UIWindow NAVLock;
    }
}
