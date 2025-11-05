#region Using
using Prana.Admin.BLL;
using Prana.AdminForms;
using Prana.AdminTools;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Authorization;
using Prana.BusinessObjects.Enums;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.Admin.UI.Forms;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
#endregion

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class SuperAdminMain : System.Windows.Forms.Form
    {
        #region Constant Definitions
        const int MNU_FILE = 0;

        const int MNU_FILE_NEW = 0;
        const int MNU_FILE_NEW_SLSU = 2;

        const int MNU_FILE_OPEN = 1;
        const int MNU_FILE_OPEN_AUEC = 0;
        const int MNU_FILE_OPEN_COUNTERPARTYVENUE = 5;
        const int MNU_FILE_OPEN_COMPANY = 7;

        const int MNU_TOOLS = 1;
        const int MNU_TOOLS_AUECMASTER = 0;
        const int MNU_TOOLS_AUECMASTER_AUEC = 1;

        const int MNU_TOOLS_COMPANY = 3;

        const int MNU_TOOLS_COUNTERPARTYVENUEMASTER = 4;
        const int MNU_TOOLS_COUNTERPARTYVENUE = 1;

        const int MNU_TOOLS_SLSU = 5;

        const int MNU_TOOLS_COMMISSION_RULES = 2;
        const int MNU_TOOLS_THIRD_PARTIES = 8;

        const int MNU_TOOLS_VENDOR = 9;
        #endregion

        #region Private and Protected Members
        private System.Windows.Forms.MainMenu mnuAdmin;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.MenuItem mnuFile;
        private System.Windows.Forms.MenuItem mnuFileOpen;
        private System.Windows.Forms.MenuItem mnuFileExit;
        private System.Windows.Forms.MenuItem mnuTools;
        private System.Windows.Forms.MenuItem mnuWindow;
        private System.Windows.Forms.MenuItem mnuHelp;
        private System.Windows.Forms.MenuItem mnuHelpAboutPrana;
        private System.Windows.Forms.MenuItem mnuFileOpenAUEC;

        private AUEC frmAUEC = null;
        private ExchangeForm frmExchange = null;
        private AssetDetails frmAssetDetails = null;
        private UnderlyingDetails frmUnderlyingDetails = null;
        private GlobalPreferences frmGlobalPreferences = null;

        //Sugandh Adding the Form for Import Setup Main.
        private Prana.PM.Admin.UI.Forms.PMMain frmPM = null;




        private Side frmSide = null;
        private Prana.Admin.OrderType frmOrderType = null;
        private Prana.Admin.ExecutionInstruction frmExecutionInstruction = null;
        private Prana.Admin.HandlingInstruction frmHandlingInstruction = null;
        private Prana.Admin.TimeInForce frmTimeInForce = null;
        private Prana.Admin.Fix frmFix = null;
        private Prana.Admin.FixCapability frmFixCapability = null;

        private Prana.Admin.Unit frmUnit = null;
        private Prana.Admin.Identifier frmIdentifier = null;
        private Prana.Admin.Module frmModule = null;
        private Prana.Admin.VenueType frmVenueType = null;
        private Prana.Admin.CounterPartyType frmCounterPartyType = null;
        private Prana.Admin.SymbolConvention frmSymbolConvention = null;
        private Prana.Admin.ClearingFirmsPrimeBrokers frmClearingFirmsPrimeBrokers = null;

        private Prana.Admin.CurrencyType frmCurrencyType = null;
        private Prana.Admin.ContractListingType frmContractListingType = null;
        private Prana.Admin.FutureMonthCode frmFutureMonthCode = null;
        private Prana.Admin.HolidayForm frmHoliday = null;
        private Prana.Admin.PranaHelp frmPranaHelp = null;


        private Prana.Admin.RiskManagement.RM frmRiskManagement = null;
        //		private Prana.Admin.RiskManagement.RMAdmin frmRiskManagement = null;
        //private Prana.Admin.CommissionRules.CommissionRules frmCommissionRules = null;
        private Prana.AdminForms.CommissionRulesNew frmCommissionRulesNew = null;
        private Prana.AdminForms.ThirdParty.ThirdPartyVendor frm_Vendor = null;

        private System.Windows.Forms.MenuItem mnuFileNewAdminUserDetail;
        private System.Windows.Forms.MenuItem mnuFileOpenAsset;
        private System.Windows.Forms.MenuItem mnuFileOpenUnderlying;
        private System.Windows.Forms.MenuItem mnuFileOpenExchange;
        private System.Windows.Forms.MenuItem mnuFileOpenCurrency;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager toolbarsManager;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _SuperAdminMain_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _SuperAdminMain_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _SuperAdminMain_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _SuperAdminMain_Toolbars_Dock_Area_Bottom;
        private System.Windows.Forms.MenuItem mnuFileOpenCounterParty;
        private System.Windows.Forms.MenuItem mnuFileOpenVenue;
        private System.Windows.Forms.MenuItem mnuFileOpenCompany;
        private System.Windows.Forms.MenuItem mnuWindowCascade;
        private System.Windows.Forms.MenuItem mnuThirdParty;
        private System.Windows.Forms.MenuItem mnuFileNewUnit;
        private System.Windows.Forms.MenuItem mnuFileNewIdentifier;
        private System.Windows.Forms.MenuItem mnuFileNewModule;
        private System.Windows.Forms.MenuItem mnuFileNewVenueType;
        private System.Windows.Forms.MenuItem mnuFileNewCounterPartyType;
        private System.Windows.Forms.MenuItem mnuFileNewClearingFirmPrimeBroker;
        private System.Windows.Forms.MenuItem mnuFileNewSymbolConvention;
        private System.Windows.Forms.MenuItem mnuFileNewContractListingType;
        private System.Windows.Forms.MenuItem mnuFileNewFutureMonthCodes;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem mnuFileNewHoliday;
        private System.Windows.Forms.MenuItem mnuFileOpenCommissionRules;
        private System.Windows.Forms.MenuItem mnuFileOpenRiskManagement;
        private MenuItem mnuVendor;
        private MenuItem mnuFilePositionManagement;
        private System.ComponentModel.IContainer components;
        private MenuItem mnuReconXSLTSetup;

        private EMSImportExport frmEMSImportExport = null;
        private MenuItem mnuEMSImportExport;
        private ReconXSLTSetup frmReconXSLTSetup = null;
        private MenuItem mnuCalendar;
        private MenuItem mnuFileOpenCountry;
        private MenuItem mnuFileOpenState;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Hashtable availableTools;
        private List<String> activeModulesAndTools = new List<String>();
        #endregion

        /// <summary>
        /// Super Admin Main
        /// </summary>
        public SuperAdminMain()
        {
            InitializeComponent();
            ApplyTheme();
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// Modified By Omshiv, Apply theme for CH Release
        /// </summary>
        private void ApplyTheme()
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    toolbarsManager.IsGlassSupported = false;
                    toolbarsManager.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
                    toolbarsManager.LockToolbars = true;
                    toolbarsManager.ShowQuickCustomizeButton = false;
                    this.DesktopLocation = new Point(100, 100);
                    this.Icon = WhiteLabelTheme.AppIcon;

                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
                    this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                    this.ForeColor = System.Drawing.Color.White;

                    this.toolbarsManager.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                    this.toolbarsManager.Appearance.ForeColor = System.Drawing.Color.White;

                    //this.toolbarsManager.ActiveMdiChildManager.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
                    //this.toolbarsManager.ActiveMdiChildManager.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// SetUp  authorizedPrincipal
        /// </summary>
        /// <param name="authorizedPrincipal"></param>
        public void SetUp(NirvanaPrincipal authorizedPrincipal)
        {
            this.authorizedPrincipal = authorizedPrincipal;
            availableTools = new Hashtable();
            NameValueCollection toolConfigDetails = ConfigurationHelper.Instance.LoadSectionBySectionName("AvailableTools");

            for (int iIndex = 0; iIndex <= toolConfigDetails.Count - 1; iIndex++)
            {
                string key = toolConfigDetails.GetKey(iIndex);
                string value = toolConfigDetails[key];
                string[] toolDetailsBreakUp = value.Split('~');
                int iPos = int.MinValue;
                iPos = value.IndexOf("~");
                string location = Application.StartupPath + "\\" + toolDetailsBreakUp[0];
                string type = toolDetailsBreakUp[1];
                availableTools.Add(key, new DynamicClass(location, type, null, key));
                MenuItem toolMnuItem = new MenuItem(key);
                toolMnuItem.Click += new EventHandler(toolMnuItem_Click);
                mnuTools.MenuItems.Add(toolMnuItem);
            }

            SetUpMenuPermissions();
        }

        void toolMnuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MenuItem toolMnuItem = (MenuItem)sender;

                ModuleResources module = (ModuleResources)Enum.Parse(typeof(ModuleResources), toolMnuItem.Text);
                AuthAction action = AuthAction.Read;
                var hasAccess = AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, action);

                if (!hasAccess)
                {
                    MessageBox.Show("You don't have permission to access this Action/Resource.", "Prana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                LaunchPluggableTool(toolMnuItem.Text);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void LaunchPluggableTool(string toolName)
        {
            try
            {
                Form pluggableForm = null;
                IPluggableTools pluggaleToolLaunch = null;
                if (availableTools.ContainsKey(toolName))
                {

                    DynamicClass formToLoad;
                    formToLoad = (DynamicClass)availableTools[toolName];

                    Assembly asmAssemblyContainingForm = Assembly.LoadFrom(formToLoad.Location);

                    Type typeToLoad = asmAssemblyContainingForm.GetType(formToLoad.Type);
                    pluggaleToolLaunch = (IPluggableTools)Activator.CreateInstance(typeToLoad);
                    pluggableForm = pluggaleToolLaunch.Reference();
                    pluggaleToolLaunch.PluggableToolsClosed += new EventHandler(PluggableToolClosed);

                    pluggableForm.Owner = this;
                    pluggableForm.ShowInTaskbar = false;
                    pluggableForm.Show();

                    // BringFormToFront(pluggableForm);
                }
                else
                {
                    ShowModuleUnavailableMessage();
                }
            }
            catch (Exception ex)
            {
                InformationMessageBox.Display(ex.StackTrace);
            }
        }
        void PluggableToolClosed(object sender, EventArgs e)
        {
            Form pluggaleTuner = (Form)sender;
            pluggaleTuner = null;
            //pluggaleToolLaunch = null;
            //menuItemUDATool.Enabled = true;
        }
        private void ShowModuleUnavailableMessage()
        {
            string DisplayMessage = "Requested module is not available."
                                    + "Please contact Prana Financial Solutions LLC. or local administrator to request access to this module.";
            InformationMessageBox.Display(DisplayMessage, "Prana");
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (frmVendor != null)
                {
                    frmVendor.Dispose();
                }
                if (frmUser != null)
                {
                    frmUser.Dispose();
                }
                if (mnuAdmin != null)
                {
                    mnuAdmin.Dispose();
                }
                if (menuItem13 != null)
                {
                    menuItem13.Dispose();
                }
                if (mnuFile != null)
                {
                    mnuFile.Dispose();
                }
                if (mnuFileOpen != null)
                {
                    mnuFileOpen.Dispose();
                }
                if (mnuFileExit != null)
                {
                    mnuFileExit.Dispose();
                }
                if (mnuTools != null)
                {
                    mnuTools.Dispose();
                }
                if (mnuWindow != null)
                {
                    mnuWindow.Dispose();
                }
                if (mnuHelp != null)
                {
                    mnuHelp.Dispose();
                }
                if (mnuHelpAboutPrana != null)
                {
                    mnuHelpAboutPrana.Dispose();
                }
                if (mnuFileOpenAUEC != null)
                {
                    mnuFileOpenAUEC.Dispose();
                }
                if (frmAUEC != null)
                {
                    frmAUEC.Dispose();
                }
                if (frmExchange != null)
                {
                    frmExchange.Dispose();
                }
                if (frmAssetDetails != null)
                {
                    frmAssetDetails.Dispose();
                }
                if (frmUnderlyingDetails != null)
                {
                    frmUnderlyingDetails.Dispose();
                }
                if (frmGlobalPreferences != null)
                {
                    frmGlobalPreferences.Dispose();
                }
                if (frmPM != null)
                {
                    frmPM.Dispose();
                }
                if (frmSide != null)
                {
                    frmSide.Dispose();
                }
                if (frmOrderType != null)
                {
                    frmOrderType.Dispose();
                }
                if (frmExecutionInstruction != null)
                {
                    frmExecutionInstruction.Dispose();
                }
                if (frmHandlingInstruction != null)
                {
                    frmHandlingInstruction.Dispose();
                }
                if (frmTimeInForce != null)
                {
                    frmTimeInForce.Dispose();
                }
                if (frmFix != null)
                {
                    frmFix.Dispose();
                }
                if (frmFixCapability != null)
                {
                    frmFixCapability.Dispose();
                }
                if (frmUnit != null)
                {
                    frmUnit.Dispose();
                }
                if (frmIdentifier != null)
                {
                    frmIdentifier.Dispose();
                }
                if (frmModule != null)
                {
                    frmModule.Dispose();
                }
                if (frmVenueType != null)
                {
                    frmVenueType.Dispose();
                }
                if (frmCounterPartyType != null)
                {
                    frmCounterPartyType.Dispose();
                }
                if (frmSymbolConvention != null)
                {
                    frmSymbolConvention.Dispose();
                }
                if (frmClearingFirmsPrimeBrokers != null)
                {
                    frmClearingFirmsPrimeBrokers.Dispose();
                }
                if (frmCurrencyType != null)
                {
                    frmCurrencyType.Dispose();
                }
                if (frmContractListingType != null)
                {
                    frmContractListingType.Dispose();
                }
                if (frmFutureMonthCode != null)
                {
                    frmFutureMonthCode.Dispose();
                }
                if (frmHoliday != null)
                {
                    frmHoliday.Dispose();
                }
                if (frmPranaHelp != null)
                {
                    frmPranaHelp.Dispose();
                }
                if (frmRiskManagement != null)
                {
                    frmRiskManagement.Dispose();
                }
                if (frmCommissionRulesNew != null)
                {
                    frmCommissionRulesNew.Dispose();
                }
                if (frm_Vendor != null)
                {
                    frm_Vendor.Dispose();
                }
                if (mnuFileNewAdminUserDetail != null)
                {
                    mnuFileNewAdminUserDetail.Dispose();
                }
                if (mnuFileOpenAsset != null)
                {
                    mnuFileOpenAsset.Dispose();
                }
                if (mnuFileOpenUnderlying != null)
                {
                    mnuFileOpenUnderlying.Dispose();
                }
                if (mnuFileOpenExchange != null)
                {
                    mnuFileOpenExchange.Dispose();
                }
                if (mnuFileOpenCurrency != null)
                {
                    mnuFileOpenCurrency.Dispose();
                }
                if (toolbarsManager != null)
                {
                    toolbarsManager.Dispose();
                }
                if (_SuperAdminMain_Toolbars_Dock_Area_Bottom != null)
                {
                    _SuperAdminMain_Toolbars_Dock_Area_Bottom.Dispose();
                }
                if (_SuperAdminMain_Toolbars_Dock_Area_Left != null)
                {
                    _SuperAdminMain_Toolbars_Dock_Area_Left.Dispose();
                }
                if (_SuperAdminMain_Toolbars_Dock_Area_Right != null)
                {
                    _SuperAdminMain_Toolbars_Dock_Area_Right.Dispose();
                }
                if (_SuperAdminMain_Toolbars_Dock_Area_Top != null)
                {
                    _SuperAdminMain_Toolbars_Dock_Area_Top.Dispose();
                }
                if (mnuFileOpenCounterParty != null)
                {
                    mnuFileOpenCounterParty.Dispose();
                }
                if (mnuFileOpenVenue != null)
                {
                    mnuFileOpenVenue.Dispose();
                }
                if (mnuFileOpenCompany != null)
                {
                    mnuFileOpenCompany.Dispose();
                }
                if (mnuWindowCascade != null)
                {
                    mnuWindowCascade.Dispose();
                }
                if (mnuThirdParty != null)
                {
                    mnuThirdParty.Dispose();
                }
                if (mnuFileNewUnit != null)
                {
                    mnuFileNewUnit.Dispose();
                }
                if (mnuFileNewIdentifier != null)
                {
                    mnuFileNewIdentifier.Dispose();
                }
                if (mnuFileNewModule != null)
                {
                    mnuFileNewModule.Dispose();
                }
                if (mnuFileNewVenueType != null)
                {
                    mnuFileNewVenueType.Dispose();
                }
                if (mnuFileNewCounterPartyType != null)
                {
                    mnuFileNewCounterPartyType.Dispose();
                }
                if (mnuFileNewClearingFirmPrimeBroker != null)
                {
                    mnuFileNewClearingFirmPrimeBroker.Dispose();
                }
                if (mnuFileNewSymbolConvention != null)
                {
                    mnuFileNewSymbolConvention.Dispose();
                }
                if (mnuFileNewContractListingType != null)
                {
                    mnuFileNewContractListingType.Dispose();
                }
                if (mnuFileNewFutureMonthCodes != null)
                {
                    mnuFileNewFutureMonthCodes.Dispose();
                }
                if (menuItem1 != null)
                {
                    menuItem1.Dispose();
                }
                if (menuItem2 != null)
                {
                    menuItem2.Dispose();
                }
                if (mnuFileNewHoliday != null)
                {
                    mnuFileNewHoliday.Dispose();
                }
                if (mnuFileOpenCommissionRules != null)
                {
                    mnuFileOpenCommissionRules.Dispose();
                }
                if (mnuFileOpenRiskManagement != null)
                {
                    mnuFileOpenRiskManagement.Dispose();
                }
                if (mnuVendor != null)
                {
                    mnuVendor.Dispose();
                }
                if (mnuFilePositionManagement != null)
                {
                    mnuFilePositionManagement.Dispose();
                }
                if (mnuReconXSLTSetup != null)
                {
                    mnuReconXSLTSetup.Dispose();
                }
                if (frmEMSImportExport != null)
                {
                    frmEMSImportExport.Dispose();
                }
                if (mnuEMSImportExport != null)
                {
                    mnuEMSImportExport.Dispose();
                }
                if (frmReconXSLTSetup != null)
                {
                    frmReconXSLTSetup.Dispose();
                }
                if (mnuCalendar != null)
                {
                    mnuCalendar.Dispose();
                }
                if (mnuFileOpenCountry != null)
                {
                    mnuFileOpenCountry.Dispose();
                }
                if (mnuFileOpenState != null)
                {
                    mnuFileOpenState.Dispose();
                }
                if (ultraPanel1 != null)
                {
                    ultraPanel1.Dispose();
                }
                if (frmThirdPartyVendor != null)
                {
                    frmThirdPartyVendor.Dispose();
                }
                if (frmCounterPartyVenueMaster != null)
                {
                    frmCounterPartyVenueMaster.Dispose();
                }
                if (frmCompanyMaster != null)
                {
                    frmCompanyMaster.Dispose();
                }
                if (frmThirdPartyForm != null)
                {
                    frmThirdPartyForm.Dispose();
                }
                if (frmCurrencyForm != null)
                {
                    frmCurrencyForm.Dispose();
                }
                if (frmTestUserForm != null)
                {
                    frmTestUserForm.Dispose();
                }
            }
            base.Dispose(disposing);
            Application.Exit();
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("tlbPranaMain");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AUEC");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CV_Master");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Company_Master");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Asset");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Underlying");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exchange");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Currency");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Third_Party");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SLSU");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Position_Management");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Master AUEC");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Global_Preferences");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool34 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Reload_Settings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("AUEC");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Underlying");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Currency");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exchange");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SLSU");
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Company_Master");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Asset");
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CV_Master");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Position_Management");
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Third_Party");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Global_Preferences");
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Global Preferences");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Master AUEC");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Assets");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exchanges");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Underlyings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Currencies");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Assets");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exchanges");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Underlyings");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool33 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Currencies");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool35 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Reload_Settings");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuperAdminMain));
            this.mnuAdmin = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuFileOpen = new System.Windows.Forms.MenuItem();
            this.mnuFileNewContractListingType = new System.Windows.Forms.MenuItem();
            this.mnuFileNewFutureMonthCodes = new System.Windows.Forms.MenuItem();
            this.mnuFileNewIdentifier = new System.Windows.Forms.MenuItem();
            this.mnuFileNewModule = new System.Windows.Forms.MenuItem();
            this.mnuEMSImportExport = new System.Windows.Forms.MenuItem();
            this.mnuReconXSLTSetup = new System.Windows.Forms.MenuItem();
            this.mnuFileNewSymbolConvention = new System.Windows.Forms.MenuItem();
            this.mnuFileNewUnit = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenVenue = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.mnuFileExit = new System.Windows.Forms.MenuItem();
            this.mnuTools = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenAsset = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenAUEC = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenCurrency = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenExchange = new System.Windows.Forms.MenuItem();
            this.mnuFileNewHoliday = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenUnderlying = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenCountry = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenState = new System.Windows.Forms.MenuItem();
            this.mnuFileNewClearingFirmPrimeBroker = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenCommissionRules = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenCompany = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.mnuFileNewCounterPartyType = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenCounterParty = new System.Windows.Forms.MenuItem();
            this.mnuFileNewVenueType = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenRiskManagement = new System.Windows.Forms.MenuItem();
            this.mnuFileNewAdminUserDetail = new System.Windows.Forms.MenuItem();
            this.mnuThirdParty = new System.Windows.Forms.MenuItem();
            this.mnuVendor = new System.Windows.Forms.MenuItem();
            this.mnuFilePositionManagement = new System.Windows.Forms.MenuItem();
            this.mnuCalendar = new System.Windows.Forms.MenuItem();
            this.mnuWindow = new System.Windows.Forms.MenuItem();
            this.mnuWindowCascade = new System.Windows.Forms.MenuItem();
            this.mnuHelp = new System.Windows.Forms.MenuItem();
            this.mnuHelpAboutPrana = new System.Windows.Forms.MenuItem();
            this._SuperAdminMain_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.toolbarsManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._SuperAdminMain_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._SuperAdminMain_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._SuperAdminMain_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            ((System.ComponentModel.ISupportInitialize)(this.toolbarsManager)).BeginInit();
            this.ultraPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuAdmin
            // 
            this.mnuAdmin.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuTools,
            this.mnuWindow,
            this.mnuHelp});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MdiList = true;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFileOpen,
            this.menuItem13,
            this.mnuFileExit});
            this.mnuFile.Text = "&File";
            // 
            // mnuFileOpen
            // 
            this.mnuFileOpen.Index = 0;
            this.mnuFileOpen.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFileNewContractListingType,
            this.mnuFileNewFutureMonthCodes,
            this.mnuFileNewIdentifier,
            this.mnuFileNewModule,
            this.mnuEMSImportExport,
            this.mnuReconXSLTSetup,
            this.mnuFileNewSymbolConvention,
            this.mnuFileNewUnit,
            this.mnuFileOpenVenue});
            this.mnuFileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mnuFileOpen.Text = "&Open";
            // 
            // mnuFileNewContractListingType
            // 
            this.mnuFileNewContractListingType.Enabled = false;
            this.mnuFileNewContractListingType.Index = 0;
            this.mnuFileNewContractListingType.Text = "Contract Listing Type";
            this.mnuFileNewContractListingType.Click += new System.EventHandler(this.mnuFileNewContractListingType_Click);
            // 
            // mnuFileNewFutureMonthCodes
            // 
            this.mnuFileNewFutureMonthCodes.Enabled = false;
            this.mnuFileNewFutureMonthCodes.Index = 1;
            this.mnuFileNewFutureMonthCodes.Text = "Future Month Codes";
            this.mnuFileNewFutureMonthCodes.Click += new System.EventHandler(this.mnuFileNewFutureMonthCodes_Click);
            // 
            // mnuFileNewIdentifier
            // 
            this.mnuFileNewIdentifier.Index = 2;
            this.mnuFileNewIdentifier.Text = "Identifier";
            this.mnuFileNewIdentifier.Click += new System.EventHandler(this.mnuFileNewIdentifier_Click);
            // 
            // mnuFileNewModule
            // 
            this.mnuFileNewModule.Index = 3;
            this.mnuFileNewModule.Text = "Module";
            this.mnuFileNewModule.Click += new System.EventHandler(this.mnuFileNewModule_Click);
            // 
            // mnuEMSImportExport
            // 
            this.mnuEMSImportExport.Index = 4;
            this.mnuEMSImportExport.Text = "EMS Import XSLT Setup";
            this.mnuEMSImportExport.Click += new System.EventHandler(this.mnuEMSImportExport_Click);
            // 
            // mnuReconXSLTSetup
            // 
            this.mnuReconXSLTSetup.Index = 5;
            this.mnuReconXSLTSetup.Text = "Recon XSLT Setup";
            this.mnuReconXSLTSetup.Click += new System.EventHandler(this.mnuReconXSLTSetup_Click);
            // 
            // mnuFileNewSymbolConvention
            // 
            this.mnuFileNewSymbolConvention.Index = 6;
            this.mnuFileNewSymbolConvention.Text = "Symbol Convention";
            this.mnuFileNewSymbolConvention.Click += new System.EventHandler(this.mnuFileNewSymbolConvention_Click);
            // 
            // mnuFileNewUnit
            // 
            this.mnuFileNewUnit.Index = 7;
            this.mnuFileNewUnit.Text = "Unit";
            this.mnuFileNewUnit.Click += new System.EventHandler(this.mnuFileNewUnit_Click);
            // 
            // mnuFileOpenVenue
            // 
            this.mnuFileOpenVenue.Enabled = false;
            this.mnuFileOpenVenue.Index = 8;
            this.mnuFileOpenVenue.Text = "&Venue";
            this.mnuFileOpenVenue.Visible = false;
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 1;
            this.menuItem13.Text = "-";
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Index = 2;
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.Index = 1;
            this.mnuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.mnuFileNewClearingFirmPrimeBroker,
            this.mnuFileOpenCommissionRules,
            this.mnuFileOpenCompany,
            this.menuItem2,
            this.mnuFileOpenRiskManagement,
            this.mnuFileNewAdminUserDetail,
            this.mnuThirdParty,
            this.mnuVendor,
            this.mnuFilePositionManagement,
            this.mnuCalendar});
            this.mnuTools.Text = "&Tools";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFileOpenAsset,
            this.mnuFileOpenAUEC,
            this.mnuFileOpenCurrency,
            this.mnuFileOpenExchange,
            this.mnuFileNewHoliday,
            this.mnuFileOpenUnderlying,
            this.mnuFileOpenCountry,
            this.mnuFileOpenState});
            this.menuItem1.Text = "AUEC Master";
            // 
            // mnuFileOpenAsset
            // 
            this.mnuFileOpenAsset.Index = 0;
            this.mnuFileOpenAsset.Text = "&Asset";
            this.mnuFileOpenAsset.Click += new System.EventHandler(this.mnuFileOpenAsset_Click);
            // 
            // mnuFileOpenAUEC
            // 
            this.mnuFileOpenAUEC.Index = 1;
            this.mnuFileOpenAUEC.Text = "AUEC";
            this.mnuFileOpenAUEC.Click += new System.EventHandler(this.mnuFileOpenAUEC_Click);
            // 
            // mnuFileOpenCurrency
            // 
            this.mnuFileOpenCurrency.Index = 2;
            this.mnuFileOpenCurrency.Text = "&Currency";
            this.mnuFileOpenCurrency.Click += new System.EventHandler(this.mnuFileOpenCurrency_Click);
            // 
            // mnuFileOpenExchange
            // 
            this.mnuFileOpenExchange.Index = 3;
            this.mnuFileOpenExchange.Text = "&Exchange";
            this.mnuFileOpenExchange.Click += new System.EventHandler(this.mnuFileOpenExchange_Click);
            // 
            // mnuFileNewHoliday
            // 
            this.mnuFileNewHoliday.Index = 4;
            this.mnuFileNewHoliday.Text = "Holiday";
            this.mnuFileNewHoliday.Click += new System.EventHandler(this.mnuFileNewHoliday_Click);
            // 
            // mnuFileOpenUnderlying
            // 
            this.mnuFileOpenUnderlying.Index = 5;
            this.mnuFileOpenUnderlying.Text = "&Underlying";
            this.mnuFileOpenUnderlying.Click += new System.EventHandler(this.mnuFileOpenUnderlying_Click);
            // 
            // mnuFileOpenCountry
            // 
            this.mnuFileOpenCountry.Index = 6;
            this.mnuFileOpenCountry.Text = "Country";
            this.mnuFileOpenCountry.Click += new System.EventHandler(this.mnuFileOpenCountry_Click);
            // 
            // mnuFileOpenState
            // 
            this.mnuFileOpenState.Index = 7;
            this.mnuFileOpenState.Text = "State";
            this.mnuFileOpenState.Click += new System.EventHandler(this.mnuFileOpenState_Click);
            // 
            // mnuFileNewClearingFirmPrimeBroker
            // 
            this.mnuFileNewClearingFirmPrimeBroker.Enabled = false;
            this.mnuFileNewClearingFirmPrimeBroker.Index = 1;
            this.mnuFileNewClearingFirmPrimeBroker.Text = "Clearing Firm Prime Broker";
            this.mnuFileNewClearingFirmPrimeBroker.Click += new System.EventHandler(this.mnuFileNewClearingFirmPrimeBroker_Click);
            // 
            // mnuFileOpenCommissionRules
            // 
            this.mnuFileOpenCommissionRules.Index = 2;
            this.mnuFileOpenCommissionRules.Text = "Commission Rules";
            this.mnuFileOpenCommissionRules.Click += new System.EventHandler(this.mnuFileOpenCommissionRules_Click);
            // 
            // mnuFileOpenCompany
            // 
            this.mnuFileOpenCompany.Index = 3;
            this.mnuFileOpenCompany.Text = "Client";
            this.mnuFileOpenCompany.Click += new System.EventHandler(this.mnuFileOpenCompany_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 4;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFileNewCounterPartyType,
            this.mnuFileOpenCounterParty,
            this.mnuFileNewVenueType});
            this.menuItem2.Text = "Broker Venue Master";
            // 
            // mnuFileNewCounterPartyType
            // 
            this.mnuFileNewCounterPartyType.Index = 0;
            this.mnuFileNewCounterPartyType.Text = "Broker Type";
            this.mnuFileNewCounterPartyType.Click += new System.EventHandler(this.mnuFileNewCounterPartyType_Click);
            // 
            // mnuFileOpenCounterParty
            // 
            this.mnuFileOpenCounterParty.Index = 1;
            this.mnuFileOpenCounterParty.Text = "Broker Venue";
            this.mnuFileOpenCounterParty.Click += new System.EventHandler(this.mnuFileOpenCounterParty_Click);
            // 
            // mnuFileNewVenueType
            // 
            this.mnuFileNewVenueType.Index = 2;
            this.mnuFileNewVenueType.Text = "Venue Type";
            this.mnuFileNewVenueType.Click += new System.EventHandler(this.mnuFileNewVenueType_Click);
            // 
            // mnuFileOpenRiskManagement
            // 
            this.mnuFileOpenRiskManagement.Index = 5;
            this.mnuFileOpenRiskManagement.Text = "Risk Management";
            this.mnuFileOpenRiskManagement.Click += new System.EventHandler(this.mnuFileOpenRiskManagement_Click);
            // 
            // mnuFileNewAdminUserDetail
            // 
            this.mnuFileNewAdminUserDetail.Index = 6;
            this.mnuFileNewAdminUserDetail.Text = "SLSU";
            this.mnuFileNewAdminUserDetail.Click += new System.EventHandler(this.mnuFileNewAdminUserDetail_Click);
            // 
            // mnuThirdParty
            // 
            this.mnuThirdParty.Index = 7;
            this.mnuThirdParty.Text = "&Third Party";
            this.mnuThirdParty.Click += new System.EventHandler(this.mnuThirdParty_Click);
            // 
            // mnuVendor
            // 
            this.mnuVendor.Index = 8;
            this.mnuVendor.Text = "&Vendor";
            this.mnuVendor.Click += new System.EventHandler(this.mnuVendor_Click);
            // 
            // mnuFilePositionManagement
            // 
            this.mnuFilePositionManagement.Index = 9;
            //CHMW-994 [Admin] Cleanup of datasource
            this.mnuFilePositionManagement.Text = "&Import Setup";
            this.mnuFilePositionManagement.Click += new System.EventHandler(this.mnuFilePositionManagement_Click);
            // 
            // mnuCalendar
            // 
            this.mnuCalendar.Index = 10;
            this.mnuCalendar.Text = "Holiday Calendar";
            this.mnuCalendar.Click += new System.EventHandler(this.mnuCalendar_Click);
            // 
            // mnuWindow
            // 
            this.mnuWindow.Index = 2;
            this.mnuWindow.MdiList = true;
            this.mnuWindow.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuWindowCascade});
            this.mnuWindow.Text = "&Window";
            // 
            // mnuWindowCascade
            // 
            this.mnuWindowCascade.Index = 0;
            this.mnuWindowCascade.Text = "Cascade";
            this.mnuWindowCascade.Click += new System.EventHandler(this.mnuWindowCascade_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Index = 3;
            this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuHelpAboutPrana});
            this.mnuHelp.Text = "&Help";
            // 
            // mnuHelpAboutPrana
            // 
            this.mnuHelpAboutPrana.Index = 0;
            this.mnuHelpAboutPrana.Text = "&About Prana";
            this.mnuHelpAboutPrana.Click += new System.EventHandler(this.mnuHelpAboutPrana_Click);
            // 
            // _SuperAdminMain_Toolbars_Dock_Area_Left
            // 
            this._SuperAdminMain_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SuperAdminMain_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.Azure;
            this._SuperAdminMain_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._SuperAdminMain_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SuperAdminMain_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 25);
            this._SuperAdminMain_Toolbars_Dock_Area_Left.Name = "_SuperAdminMain_Toolbars_Dock_Area_Left";
            this._SuperAdminMain_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 5);
            this._SuperAdminMain_Toolbars_Dock_Area_Left.ToolbarsManager = this.toolbarsManager;
            // 
            // toolbarsManager
            // 
            this.toolbarsManager.DesignerFlags = 1;
            this.toolbarsManager.DockWithinContainer = this;
            this.toolbarsManager.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.toolbarsManager.RightAlignedMenus = Infragistics.Win.DefaultableBoolean.True;
            this.toolbarsManager.ShowFullMenusDelay = 500;
            this.toolbarsManager.ShowQuickCustomizeButton = false;
            this.toolbarsManager.ShowShortcutsInToolTips = true;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            buttonTool3.InstanceProps.AppearancesSmall.Appearance = appearance1;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4,
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool21,
            buttonTool8,
            buttonTool9,
            popupMenuTool1,
            buttonTool26,
            buttonTool10,
            buttonTool34});
            ultraToolbar1.Text = "Main Toolbaar";
            this.toolbarsManager.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            buttonTool11.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
            buttonTool11.SharedPropsInternal.Caption = "AUEC";
            buttonTool11.SharedPropsInternal.CustomizerCaption = "AUEC";
            buttonTool11.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool11.SharedPropsInternal.ToolTipText = "AUEC";
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            buttonTool12.SharedPropsInternal.AppearancesSmall.Appearance = appearance3;
            buttonTool12.SharedPropsInternal.Caption = "Underlying";
            buttonTool12.SharedPropsInternal.CustomizerCaption = "Underlying";
            buttonTool12.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool12.SharedPropsInternal.ToolTipText = "Underlying";
            buttonTool12.SharedPropsInternal.Visible = false;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            buttonTool13.SharedPropsInternal.AppearancesSmall.Appearance = appearance4;
            buttonTool13.SharedPropsInternal.Caption = "Currency";
            buttonTool13.SharedPropsInternal.CustomizerCaption = "Currency";
            buttonTool13.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool13.SharedPropsInternal.ToolTipText = "Currency";
            buttonTool13.SharedPropsInternal.Visible = false;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            buttonTool14.SharedPropsInternal.AppearancesSmall.Appearance = appearance5;
            buttonTool14.SharedPropsInternal.Caption = "Exchange";
            buttonTool14.SharedPropsInternal.CustomizerCaption = "Exchange";
            buttonTool14.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool14.SharedPropsInternal.ToolTipText = "Exchange";
            buttonTool14.SharedPropsInternal.Visible = false;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            buttonTool15.SharedPropsInternal.AppearancesSmall.Appearance = appearance6;
            buttonTool15.SharedPropsInternal.Caption = "SLSU";
            buttonTool15.SharedPropsInternal.CustomizerCaption = "SLSU";
            buttonTool15.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool15.SharedPropsInternal.ToolTipText = "Users";
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            buttonTool16.SharedPropsInternal.AppearancesSmall.Appearance = appearance7;
            buttonTool16.SharedPropsInternal.Caption = "Exit";
            buttonTool16.SharedPropsInternal.CustomizerCaption = "Exit";
            buttonTool16.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool16.SharedPropsInternal.ToolTipText = "Exit";
            buttonTool17.SharedPropsInternal.Caption = "Client";
            buttonTool17.SharedPropsInternal.CustomizerCaption = "Client";
            buttonTool17.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool17.SharedPropsInternal.ToolTipText = "Client";
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            buttonTool18.SharedPropsInternal.AppearancesSmall.Appearance = appearance8;
            buttonTool18.SharedPropsInternal.Caption = "Asset";
            buttonTool18.SharedPropsInternal.CustomizerCaption = "Asset";
            buttonTool18.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            buttonTool18.SharedPropsInternal.ToolTipText = "Asset";
            buttonTool18.SharedPropsInternal.Visible = false;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            buttonTool19.SharedPropsInternal.AppearancesSmall.Appearance = appearance9;
            buttonTool19.SharedPropsInternal.Caption = "Broker Venue";
            buttonTool19.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance10.Image = global::Prana.Admin.Properties.Resources.PM;
            buttonTool20.SharedPropsInternal.AppearancesSmall.Appearance = appearance10;
            //CHMW-994 [Admin] Cleanup of datasource
            buttonTool20.SharedPropsInternal.Caption = "Import Setup";
            buttonTool20.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            buttonTool22.SharedPropsInternal.AppearancesSmall.Appearance = appearance11;
            buttonTool22.SharedPropsInternal.Caption = "Third Party";
            buttonTool22.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
            buttonTool25.SharedPropsInternal.AppearancesSmall.Appearance = appearance12;
            buttonTool25.SharedPropsInternal.Caption = "Global Preferences";
            buttonTool25.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool2.SharedPropsInternal.Caption = "Master AUEC";
            popupMenuTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool23,
            buttonTool27,
            buttonTool28,
            buttonTool29});
            buttonTool30.SharedPropsInternal.Caption = "Assets";
            buttonTool31.SharedPropsInternal.Caption = "Exchanges";
            buttonTool32.SharedPropsInternal.Caption = "Underlyings";
            buttonTool33.SharedPropsInternal.Caption = "Currencies";
            buttonTool35.SharedPropsInternal.Caption = "Reload Settings";
            buttonTool35.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            this.toolbarsManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool11,
            buttonTool12,
            buttonTool13,
            buttonTool14,
            buttonTool15,
            buttonTool16,
            buttonTool17,
            buttonTool18,
            buttonTool19,
            buttonTool20,
            buttonTool22,
            buttonTool25,
            buttonTool24,
            popupMenuTool2,
            buttonTool30,
            buttonTool31,
            buttonTool32,
            buttonTool33,
            buttonTool35});
            this.toolbarsManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.toolbarsManager_ToolClick);
            // 
            // _SuperAdminMain_Toolbars_Dock_Area_Right
            // 
            this._SuperAdminMain_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SuperAdminMain_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.Azure;
            this._SuperAdminMain_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._SuperAdminMain_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SuperAdminMain_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1020, 25);
            this._SuperAdminMain_Toolbars_Dock_Area_Right.Name = "_SuperAdminMain_Toolbars_Dock_Area_Right";
            this._SuperAdminMain_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 5);
            this._SuperAdminMain_Toolbars_Dock_Area_Right.ToolbarsManager = this.toolbarsManager;
            // 
            // _SuperAdminMain_Toolbars_Dock_Area_Top
            // 
            this._SuperAdminMain_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SuperAdminMain_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.Azure;
            this._SuperAdminMain_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._SuperAdminMain_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SuperAdminMain_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._SuperAdminMain_Toolbars_Dock_Area_Top.Name = "_SuperAdminMain_Toolbars_Dock_Area_Top";
            this._SuperAdminMain_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1020, 25);
            this._SuperAdminMain_Toolbars_Dock_Area_Top.ToolbarsManager = this.toolbarsManager;
            // 
            // _SuperAdminMain_Toolbars_Dock_Area_Bottom
            // 
            this._SuperAdminMain_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._SuperAdminMain_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.Azure;
            this._SuperAdminMain_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._SuperAdminMain_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._SuperAdminMain_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 30);
            this._SuperAdminMain_Toolbars_Dock_Area_Bottom.Name = "_SuperAdminMain_Toolbars_Dock_Area_Bottom";
            this._SuperAdminMain_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1020, 0);
            this._SuperAdminMain_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.toolbarsManager;
            // 
            // ultraPanel1
            // 
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel1.Location = new System.Drawing.Point(0, 25);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(1020, 5);
            this.ultraPanel1.TabIndex = 5;
            // 
            // SuperAdminMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(1020, 30);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this._SuperAdminMain_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._SuperAdminMain_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._SuperAdminMain_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._SuperAdminMain_Toolbars_Dock_Area_Top);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.Menu = this.mnuAdmin;
            this.Name = "SuperAdminMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Prana: Admin";
            ((System.ComponentModel.ISupportInitialize)(this.toolbarsManager)).EndInit();
            this.ultraPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void mnuFileExit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void mnuFileOpenAUEC_Click(object sender, System.EventArgs e)
        {
            if (frmAUEC == null)
            {
                frmAUEC = new AUEC();
                frmAUEC.Owner = this;
                frmAUEC.ShowInTaskbar = false;
            }
            //Modified by : Pranay Deep
            //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

            if ((frmAUEC.WindowState) == (FormWindowState.Minimized))
            {
                frmAUEC.WindowState = FormWindowState.Normal;
            }

            frmAUEC.BringToFront();
            frmAUEC.Show();
            frmAUEC.Activate();
            frmAUEC.Disposed += new EventHandler(frmAUEC_Disposed);
        }

        AddUser frmUser = null;
        private void mnuFileNewUser_Click(object sender, System.EventArgs e)
        {
            if (frmUser == null)
            {
                frmUser = new AddUser();
                frmUser.Owner = this;
                frmUser.ShowInTaskbar = false;
            }
            frmUser.Show();
            frmUser.Activate();
            frmUser.Disposed += new EventHandler(frmUser_Disposed);

        }
        AddVendor frmVendor = null;
        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            if (frmVendor == null)
            {
                frmVendor = new AddVendor();
                frmVendor.Owner = this;
                frmVendor.ShowInTaskbar = false;
            }
            //Modified by : Pranay Deep
            //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

            if ((frmVendor.WindowState) == (FormWindowState.Minimized))
            {
                frmVendor.WindowState = FormWindowState.Normal;
            }

            frmVendor.BringToFront();
            frmVendor.Show();
            frmVendor.Activate();
            frmVendor.Disposed += new EventHandler(frmVendor_Disposed);
        }

        private void mnuFileClose_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        ThirdPartyVendor frmThirdPartyVendor = null;
        private void mnuFileNewAdminUserDetail_Click(object sender, System.EventArgs e)
        {
            if (frmThirdPartyVendor == null)
            {
                frmThirdPartyVendor = new ThirdPartyVendor();
                frmThirdPartyVendor.Owner = this;
                frmThirdPartyVendor.ShowInTaskbar = false;
            }
            frmThirdPartyVendor.Show();
            frmThirdPartyVendor.Activate();
            frmThirdPartyVendor.Disposed += new EventHandler(frmThirdPartyVendor_Disposed);
        }

        private void mnuFileOpenAsset_Click(object sender, System.EventArgs e)
        {
            if (frmAssetDetails == null)
            {
                frmAssetDetails = new AssetDetails();
                frmAssetDetails.Owner = this;
                frmAssetDetails.ShowInTaskbar = false;
            }
            frmAssetDetails.Show();
            frmAssetDetails.Activate();
            frmAssetDetails.Disposed += new EventHandler(frmAssetDetails_Disposed);
        }

        //modified by amit on 11.03.2015 to add reload settings for updating cache.
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-2895
        private void toolbarsManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                if (!e.Tool.Key.Equals("Exit") && !e.Tool.Key.Equals("Reload_Settings"))
                {
                    ModuleResources module = (ModuleResources)Enum.Parse(typeof(ModuleResources), e.Tool.Key);
                    AuthAction action = AuthAction.Read;
                    var hasAccess = AuthorizationManager.GetInstance().CheckAccesibilityForMoldule(module, action);
                    if (!hasAccess)
                    {
                        MessageBox.Show("You don't have permission to access this Action/Resource.", "Prana Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                switch (e.Tool.Key)
                {
                    case "Company_Master":

                        if (frmCompanyMaster == null)
                        {
                            frmCompanyMaster = new CompanyMaster();
                            frmCompanyMaster.Owner = this;
                            frmCompanyMaster.ShowInTaskbar = false;
                        }

                        #region added by: Bharat Raturi, 07 july 2014, bring the form to normal state if already open and minimized
                        else if (frmCompanyMaster.WindowState == FormWindowState.Minimized)
                        {
                            frmCompanyMaster.WindowState = FormWindowState.Normal;
                        }
                        #endregion
                        frmCompanyMaster.Show();
                        frmCompanyMaster.Activate();
                        activeModulesAndTools.Add("Company_Master");
                        frmCompanyMaster.Disposed += new EventHandler(frmCompanyMaster_Disposed);
                        break;

                    case "AUEC":
                        if (frmAUEC == null)
                        {
                            frmAUEC = new AUEC();
                            frmAUEC.Owner = this;
                            frmAUEC.ShowInTaskbar = false;
                        }
                        //Modified by : Pranay Deep
                        //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

                        if ((frmAUEC.WindowState) == (FormWindowState.Minimized))
                        {
                            frmAUEC.WindowState = FormWindowState.Normal;
                        }

                        frmAUEC.BringToFront();
                        frmAUEC.Show();
                        frmAUEC.Activate();
                        activeModulesAndTools.Add("AUEC");
                        frmAUEC.Disposed += new EventHandler(frmAUEC_Disposed);
                        break;

                    case "ASSET":
                    case "Assets":
                        if (frmAssetDetails == null)
                        {
                            frmAssetDetails = new AssetDetails();
                            frmAssetDetails.Owner = this;
                            frmAssetDetails.ShowInTaskbar = false;
                        }
                        //Modified by : Pranay Deep
                        //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

                        if ((frmAssetDetails.WindowState) == (FormWindowState.Minimized))
                        {
                            frmAssetDetails.WindowState = FormWindowState.Normal;
                        }

                        frmAssetDetails.BringToFront();
                        frmAssetDetails.Show();
                        frmAssetDetails.Activate();
                        activeModulesAndTools.Add("Assets");
                        frmAssetDetails.Disposed += new EventHandler(frmAssetDetails_Disposed);
                        break;

                    case "UNDERLYING":
                    case "Underlyings":
                        if (frmUnderlyingDetails == null)
                        {
                            frmUnderlyingDetails = new UnderlyingDetails();
                            frmUnderlyingDetails.Owner = this;
                            frmUnderlyingDetails.ShowInTaskbar = false;
                        }
                        //Modified by : Pranay Deep
                        //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

                        if ((frmUnderlyingDetails.WindowState) == (FormWindowState.Minimized))
                        {
                            frmUnderlyingDetails.WindowState = FormWindowState.Normal;
                        }

                        frmUnderlyingDetails.BringToFront();
                        frmUnderlyingDetails.Show();
                        frmUnderlyingDetails.Activate();
                        activeModulesAndTools.Add("Underlyings");
                        frmUnderlyingDetails.Disposed += new EventHandler(frmUnderlyingDetails_Disposed);
                        break;

                    case "EXCHANGE":
                    case "Exchanges":
                        if (frmExchange == null)
                        {
                            frmExchange = new ExchangeForm();
                            frmExchange.Owner = this;
                            frmExchange.ShowInTaskbar = false;
                        }

                        //Modified by : Pranay Deep
                        //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

                        if ((frmExchange.WindowState) == (FormWindowState.Minimized))
                        {
                            frmExchange.WindowState = FormWindowState.Normal;
                        }

                        frmExchange.BringToFront();
                        frmExchange.Show();
                        frmExchange.Activate();
                        activeModulesAndTools.Add("Exchanges");
                        frmExchange.Disposed += new EventHandler(frmExchange_Disposed);
                        break;

                    case "CURRENCY":
                    case "Currencies":
                        if (frmCurrencyForm == null)
                        {
                            frmCurrencyForm = new CurrencyForm();
                            frmCurrencyForm.Owner = this;
                            frmCurrencyForm.ShowInTaskbar = false;
                        }
                        //Modified by : Pranay Deep
                        //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

                        if ((frmCurrencyForm.WindowState) == (FormWindowState.Minimized))
                        {
                            frmCurrencyForm.WindowState = FormWindowState.Normal;
                        }

                        frmCurrencyForm.BringToFront();
                        frmCurrencyForm.Show();
                        frmCurrencyForm.Activate();
                        activeModulesAndTools.Add("Currencies");
                        frmCurrencyForm.Disposed += new EventHandler(frmCurrencyForm_Disposed);
                        break;

                    case "SLSU":
                        if (frmThirdPartyVendor == null)
                        {
                            frmThirdPartyVendor = new ThirdPartyVendor();
                            frmThirdPartyVendor.Owner = this;
                            frmThirdPartyVendor.ShowInTaskbar = false;
                        }
                        //Modified by : Pranay Deep
                        //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

                        if ((frmThirdPartyVendor.WindowState) == (FormWindowState.Minimized))
                        {
                            frmThirdPartyVendor.WindowState = FormWindowState.Normal;
                        }

                        frmThirdPartyVendor.BringToFront();
                        frmThirdPartyVendor.Show();
                        frmThirdPartyVendor.Activate();
                        activeModulesAndTools.Add("SLSU");
                        frmThirdPartyVendor.Disposed += new EventHandler(frmThirdPartyVendor_Disposed);
                        break;

                    case "CV_Master":

                        if (frmCounterPartyVenueMaster == null)
                        {
                            frmCounterPartyVenueMaster = new CounterPartyVenueMaster();
                            frmCounterPartyVenueMaster.Owner = this;
                            frmCounterPartyVenueMaster.ShowInTaskbar = false;
                        }
                        //Modified by : Pranay Deep
                        //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

                        if ((frmCounterPartyVenueMaster.WindowState) == (FormWindowState.Minimized))
                        {
                            frmCounterPartyVenueMaster.WindowState = FormWindowState.Normal;
                        }

                        frmCounterPartyVenueMaster.BringToFront();

                        frmCounterPartyVenueMaster.Show();
                        frmCounterPartyVenueMaster.Activate();
                        activeModulesAndTools.Add("CV_Master");
                        frmCounterPartyVenueMaster.Disposed += new EventHandler(frmCounterPartyVenueMaster_Disposed);

                        break;

                    // Sugandh - Added for the PM module
                    case "Position_Management":

                        if (frmPM == null)
                        {
                            frmPM = new Prana.PM.Admin.UI.Forms.PMMain();
                            frmPM.Owner = this;
                            frmPM.ShowInTaskbar = false;
                        }
                        //Modified by : Pranay Deep
                        //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

                        if ((frmPM.WindowState) == (FormWindowState.Minimized))
                        {
                            frmPM.WindowState = FormWindowState.Normal;
                        }

                        frmPM.BringToFront();
                        frmPM.Show();
                        frmPM.Activate();
                        activeModulesAndTools.Add("Position_Management");
                        frmPM.Disposed += new EventHandler(frmPM_Disposed);

                        break;


                    case "Exit":
                        Application.Exit();
                        break;

                    case "Third_Party":
                        mnuThirdParty_Click(this, null);
                        frmThirdPartyForm.Show();
                        frmThirdPartyForm.Activate();
                        activeModulesAndTools.Add("Third_Party");
                        break;
                    case "Global_Preferences":
                        //if (frmGlobalPreferences == null)
                        // {
                        LaunchGlobalPreferencesForm_Click(this, null);
                        // }
                        activeModulesAndTools.Add("Global_Preferences");
                        break;

                    //Added by amit 11.03.2015 for updating cache.
                    case "Reload_Settings":
                        RefreshFrequentlyUsedData();
                        break;

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        //Added by amit on 11.03.2015 for updating cache.
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-2895
        /// <summary>
        /// Refresh Frequently Used Data
        /// </summary>
        private void RefreshFrequentlyUsedData()
        {
            try
            {
                if (activeModulesAndTools.Count == 0)
                {
                    CachedDataManager.GetInstance.RefreshFrequentlyUsedData();
                    MessageBox.Show("Settings reloaded.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Please close all open windows before reloading settings.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void LaunchGlobalPreferencesForm_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (frmGlobalPreferences == null)
                {
                    frmGlobalPreferences = new GlobalPreferences();
                    frmGlobalPreferences.Owner = this;
                    frmGlobalPreferences.ShowInTaskbar = false;

                }

                //Modified by : Pranay Deep
                //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

                if ((frmGlobalPreferences.WindowState) == (FormWindowState.Minimized))
                {
                    frmGlobalPreferences.WindowState = FormWindowState.Normal;
                }

                frmGlobalPreferences.BringToFront();
                frmGlobalPreferences.Show();
                frmGlobalPreferences.Activate();
                frmGlobalPreferences.Disposed += new EventHandler(frmGlobalPreferences_Disposed);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void mnuFileOpenUnderlying_Click(object sender, System.EventArgs e)
        {
            if (frmUnderlyingDetails == null)
            {
                frmUnderlyingDetails = new UnderlyingDetails();
                frmUnderlyingDetails.Owner = this;
                frmUnderlyingDetails.ShowInTaskbar = false;
            }
            frmUnderlyingDetails.Show();
            frmUnderlyingDetails.Activate();
            frmUnderlyingDetails.Disposed += new EventHandler(frmUnderlyingDetails_Disposed);
        }

        private CounterPartyVenueMaster frmCounterPartyVenueMaster = null;
        private void mnuFileOpenCounterParty_Click(object sender, System.EventArgs e)
        {
            if (frmCounterPartyVenueMaster == null)
            {
                frmCounterPartyVenueMaster = new CounterPartyVenueMaster();
                frmCounterPartyVenueMaster.Owner = this;
                frmCounterPartyVenueMaster.ShowInTaskbar = false;
            }
            frmCounterPartyVenueMaster.Show();
            frmCounterPartyVenueMaster.Activate();
            frmCounterPartyVenueMaster.Disposed += new EventHandler(frmCounterPartyVenueMaster_Disposed);
        }

        CompanyMaster frmCompanyMaster = null;
        private void mnuFileOpenCompany_Click(object sender, System.EventArgs e)
        {
            if (frmCompanyMaster == null)
            {
                frmCompanyMaster = new CompanyMaster();
                frmCompanyMaster.Owner = this;
                frmCompanyMaster.ShowInTaskbar = false;
            }
            frmCompanyMaster.Show();
            frmCompanyMaster.Activate();
            frmCompanyMaster.Disposed += new EventHandler(frmCompanyMaster_Disposed);
        }

        Permissions _userPermissions = null;
        /// <summary>
        /// User Permissions
        /// </summary>
        public Permissions UserPermissions
        {
            get { return _userPermissions; }
            set
            {
                _userPermissions = value;
                SavePreferences();
                SetUpMenuPermissions();

            }
        }

        /// <summary>
        /// set UI menu according to CH/ nirvana Release.
        /// Modified by - Omshiv, May 2014
        /// </summary>
        private void SetUpMenuPermissions()
        {
            try
            {

                bool chkViewCompanyMaster = true;
                bool chkViewCounterParties = true;
                bool chkViewAUEC = true;
                bool chkViewThirdParties = false;
                bool chkViewCommissionRules = false;
                bool chkViewSLSU = false;
                bool chkViewVendor = false;
                bool chkViewPosition_Management = false;

                chkViewCommissionRules = true;
                chkViewSLSU = true;
                chkViewVendor = true;
                chkViewPosition_Management = true;
                chkViewThirdParties = true;
                toolbarsManager.Toolbars["tlbPranaMain"].Tools["Reload_Settings"].SharedProps.Visible = false;

                if (chkViewAUEC == false)
                {
                    toolbarsManager.Toolbars["tlbPranaMain"].Tools["AUEC"].SharedProps.Enabled = false;
                    mnuTools.MenuItems[MNU_TOOLS_AUECMASTER].MenuItems[MNU_TOOLS_AUECMASTER_AUEC].Enabled = false;
                }
                if (chkViewCounterParties == false)
                {
                    mnuTools.MenuItems[MNU_TOOLS_COUNTERPARTYVENUEMASTER].MenuItems[MNU_TOOLS_COUNTERPARTYVENUE].Enabled = false;
                    toolbarsManager.Toolbars["tlbPranaMain"].Tools["CV_Master"].SharedProps.Enabled = false;
                }
                if (chkViewCompanyMaster == false)
                {
                    mnuTools.MenuItems[MNU_TOOLS_COMPANY].Enabled = false;
                    toolbarsManager.Toolbars["tlbPranaMain"].Tools["Company_Master"].SharedProps.Enabled = false;
                }


                if (chkViewThirdParties == false)
                {
                    mnuTools.MenuItems[MNU_TOOLS_THIRD_PARTIES].Enabled = false;
                }

                if (chkViewPosition_Management == false)
                {

                    toolbarsManager.Toolbars["tlbPranaMain"].Tools["Position_Management"].SharedProps.Visible = false;
                }


                if (chkViewCommissionRules == false)
                {
                    mnuTools.MenuItems[MNU_TOOLS_COMMISSION_RULES].Enabled = false;
                }
                if (chkViewSLSU == false)
                {
                    mnuTools.MenuItems[MNU_TOOLS_SLSU].Enabled = false;
                    toolbarsManager.Toolbars["tlbPranaMain"].Tools["SLSU"].SharedProps.Visible = false;
                }

                if (chkViewVendor == false)
                {
                    mnuTools.MenuItems[MNU_TOOLS_VENDOR].Enabled = false;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SavePreferences()
        {
            Prana.Admin.BLL.Preferences preferences = Prana.Admin.BLL.Preferences.Instance;
            preferences.UserPermissions = _userPermissions;
            preferences.UserID = _userPermissions.UserID; //Newlly added code line on 14th Aprip 2006.
            return preferences.Reset();
        }

        private void mnuWindowCascade_Click(object sender, System.EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void mnuFileOpenExchange_Click(object sender, System.EventArgs e)
        {
            if (frmExchange == null)
            {
                frmExchange = new ExchangeForm();
                frmExchange.Owner = this;
                frmExchange.ShowInTaskbar = false;
            }
            frmExchange.Show();
            frmExchange.Activate();
            frmExchange.Disposed += new EventHandler(frmExchange_Disposed);
        }

        ThirdPartyForm frmThirdPartyForm = null;
        private void mnuThirdParty_Click(object sender, System.EventArgs e)
        {


            if (frmThirdPartyForm == null)
            {
                frmThirdPartyForm = new ThirdPartyForm();
                frmThirdPartyForm.Owner = this;
                frmThirdPartyForm.ShowInTaskbar = false;

            }
            //Modified by : Pranay Deep
            //JIRA : http://jira.nirvanasolutions.com:8080/browse/PRANA-10419

            if ((frmThirdPartyForm.WindowState) == (FormWindowState.Minimized))
            {
                frmThirdPartyForm.WindowState = FormWindowState.Normal;
            }

            frmThirdPartyForm.BringToFront();
            frmThirdPartyForm.Show();
            frmThirdPartyForm.Activate();
            frmThirdPartyForm.Disposed += new EventHandler(frmThirdPartyForm_Disposed);
        }

        CurrencyForm frmCurrencyForm = null;
        private void mnuFileOpenCurrency_Click(object sender, System.EventArgs e)
        {
            if (frmCurrencyForm == null)
            {
                frmCurrencyForm = new CurrencyForm();
                frmCurrencyForm.Owner = this;
                frmCurrencyForm.ShowInTaskbar = false;
            }
            frmCurrencyForm.Show();
            frmCurrencyForm.Activate();
            frmCurrencyForm.Disposed += new EventHandler(frmCurrencyForm_Disposed);
        }

        TestUserForm frmTestUserForm = null;
        private void mnuTestUser_Click(object sender, System.EventArgs e)
        {
            if (frmTestUserForm == null)
            {
                frmTestUserForm = new TestUserForm();
                frmTestUserForm.Owner = this;
                frmTestUserForm.ShowInTaskbar = false;
            }
            frmTestUserForm.Show();
            frmTestUserForm.Disposed += new EventHandler(frmTestUserForm_Disposed);
        }

        //added by amit on 11.03.2015  for removing open window from list on dispose.
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-2895
        private void frmGlobalPreferences_Disposed(object sender, EventArgs e)
        {
            frmGlobalPreferences = null;
            activeModulesAndTools.Remove("Global_Preferences");
        }
        private void frmTestUserForm_Disposed(object sender, EventArgs e)
        {
            frmTestUserForm = null;
        }
        private void frmThirdPartyForm_Disposed(object sender, EventArgs e)
        {
            frmThirdPartyForm = null;
            activeModulesAndTools.Remove("Third_Party");
        }
        private void frmCurrencyForm_Disposed(object sender, EventArgs e)
        {
            frmCurrencyForm = null;
            activeModulesAndTools.Remove("Currencies");
        }
        private void frmExchange_Disposed(object sender, EventArgs e)
        {
            frmExchange = null;
            activeModulesAndTools.Remove("Exchanges");
        }

        private void frmAUEC_Disposed(object sender, EventArgs e)
        {
            frmAUEC = null;
            activeModulesAndTools.Remove("AUEC");
        }

        private void frmUser_Disposed(object sender, EventArgs e)
        {
            frmUser = null;
        }
        private void frmVendor_Disposed(object sender, EventArgs e)
        {
            frmVendor = null;
        }

        private void frmThirdPartyVendor_Disposed(object sender, EventArgs e)
        {
            frmThirdPartyVendor = null;
            activeModulesAndTools.Remove("SLSU");
        }

        private void frmAssetDetails_Disposed(object sender, EventArgs e)
        {
            frmAssetDetails = null;
            activeModulesAndTools.Remove("Assets");
        }

        private void frmUnderlyingDetails_Disposed(object sender, EventArgs e)
        {
            frmUnderlyingDetails = null;
            activeModulesAndTools.Remove("Underlyings");
        }

        private void frmCounterPartyVenueMaster_Disposed(object sender, EventArgs e)
        {
            frmCounterPartyVenueMaster = null;
            activeModulesAndTools.Remove("CV_Master");
        }


        private void frmPM_Disposed(object sender, EventArgs e)
        {
            frmPM = null;
            activeModulesAndTools.Remove("Position_Management");
        }

        private void frmCompanyMaster_Disposed(object sender, EventArgs e)
        {
            frmCompanyMaster = null;
            activeModulesAndTools.Remove("Company_Master");
        }

        private void frmSide_Disposed(object sender, EventArgs e)
        {
            frmSide = null;
        }

        private void frmOrderType_Disposed(object sender, EventArgs e)
        {
            frmOrderType = null;
        }

        private void frmExecutionInstruction_Disposed(object sender, EventArgs e)
        {
            frmExecutionInstruction = null;
        }

        private void frmTimeInForce_Disposed(object sender, EventArgs e)
        {
            frmTimeInForce = null;
        }

        private void frmHandlingInstruction_Disposed(object sender, EventArgs e)
        {
            frmHandlingInstruction = null;
        }

        private void frmUnit_Disposed(object sender, EventArgs e)
        {
            frmUnit = null;
        }

        private void frmIdentifier_Disposed(object sender, EventArgs e)
        {
            frmIdentifier = null;
        }

        private void frmFix_Disposed(object sender, EventArgs e)
        {
            frmFix = null;
        }

        private void frmFixCapability_Disposed(object sender, EventArgs e)
        {
            frmFixCapability = null;
        }

        private void frmModule_Disposed(object sender, EventArgs e)
        {
            frmModule = null;
        }

        private void frmVenueType_Disposed(object sender, EventArgs e)
        {
            frmVenueType = null;
        }

        private void frmCounterPartyType_Disposed(object sender, EventArgs e)
        {
            frmCounterPartyType = null;
        }

        private void frmSymbolConvention_Disposed(object sender, EventArgs e)
        {
            frmSymbolConvention = null;
        }

        private void frmClearingFirmsPrimeBrokers_Disposed(object sender, EventArgs e)
        {
            frmClearingFirmsPrimeBrokers = null;
        }

        private void frmCurrencyType_Disposed(object sender, EventArgs e)
        {
            frmCurrencyType = null;
        }

        private void frmContractListingType_Disposed(object sender, EventArgs e)
        {
            frmContractListingType = null;
        }

        private void frmFutureMonthCode_Disposed(object sender, EventArgs e)
        {
            frmFutureMonthCode = null;
        }

        private void frmHoliday_Disposed(object sender, EventArgs e)
        {
            frmHoliday = null;
        }

        private void frmRiskManagement_Disposed(object sender, EventArgs e)
        {
            frmRiskManagement = null;
        }

        private void frmCommissionRulesNew_Disposed(object sender, EventArgs e)
        {
            frmCommissionRulesNew = null;
        }

        private void frmPranaHelp_Disposed(object sender, EventArgs e)
        {
            frmPranaHelp = null;
        }

        private void frm_Vendor_Disposed(object sender, EventArgs e)
        {
            frm_Vendor = null;
        }

        private void mnuNewFIXTAGSSIDE_Click(object sender, System.EventArgs e)
        {
            if (frmSide == null)
            {
                frmSide = new Side();
                frmSide.Owner = this;
                frmSide.ShowInTaskbar = false;
            }
            frmSide.Show();
            frmSide.Activate();
            frmSide.Disposed += new EventHandler(frmSide_Disposed);
        }

        private void mnuNewFIXTAGSOrderType_Click(object sender, System.EventArgs e)
        {
            if (frmOrderType == null)
            {
                frmOrderType = new OrderType();
                frmOrderType.Owner = this;
                frmOrderType.ShowInTaskbar = false;
            }
            frmOrderType.Show();
            frmOrderType.Activate();
            frmOrderType.Disposed += new EventHandler(frmOrderType_Disposed);
        }

        private void mnuNewFIXTAGSExecutionInstruction_Click(object sender, System.EventArgs e)
        {
            if (frmExecutionInstruction == null)
            {
                frmExecutionInstruction = new ExecutionInstruction();
                frmExecutionInstruction.Owner = this;
                frmExecutionInstruction.ShowInTaskbar = false;
            }
            frmExecutionInstruction.Show();
            frmExecutionInstruction.Activate();
            frmExecutionInstruction.Disposed += new EventHandler(frmExecutionInstruction_Disposed);
        }

        private void mnuNewFIXTAGSHandlingInstructions_Click(object sender, System.EventArgs e)
        {
            if (frmHandlingInstruction == null)
            {
                frmHandlingInstruction = new HandlingInstruction();
                frmHandlingInstruction.Owner = this;
                frmHandlingInstruction.ShowInTaskbar = false;
            }
            frmHandlingInstruction.Show();
            frmHandlingInstruction.Activate();
            frmHandlingInstruction.Disposed += new EventHandler(frmHandlingInstruction_Disposed);
        }

        private void mnuNewFIXTAGSTimeInForce_Click(object sender, System.EventArgs e)
        {
            if (frmTimeInForce == null)
            {
                frmTimeInForce = new TimeInForce();
                frmTimeInForce.Owner = this;
                frmTimeInForce.ShowInTaskbar = false;
            }
            frmTimeInForce.Show();
            frmTimeInForce.Activate();
            frmTimeInForce.Disposed += new EventHandler(frmTimeInForce_Disposed);
        }

        private void mnuFileNewUnit_Click(object sender, System.EventArgs e)
        {
            if (frmUnit == null)
            {
                frmUnit = new Unit();
                frmUnit.Owner = this;
                frmUnit.ShowInTaskbar = false;
            }
            frmUnit.Show();
            frmUnit.Activate();
            frmUnit.Disposed += new EventHandler(frmUnit_Disposed);
        }

        private void mnuFileNewIdentifier_Click(object sender, System.EventArgs e)
        {
            if (frmIdentifier == null)
            {
                frmIdentifier = new Identifier();
                frmIdentifier.Owner = this;
                frmIdentifier.ShowInTaskbar = false;
            }
            frmIdentifier.Show();
            frmIdentifier.Activate();
            frmIdentifier.Disposed += new EventHandler(frmIdentifier_Disposed);
        }

        private void mnuNewFIXTAGSFix_Click(object sender, System.EventArgs e)
        {
            if (frmFix == null)
            {
                frmFix = new Fix();
                frmFix.Owner = this;
                frmFix.ShowInTaskbar = false;
            }
            frmFix.Show();
            frmFix.Activate();
            frmFix.Disposed += new EventHandler(frmFix_Disposed);
        }

        private void mnuNewFIXTAGSFixCapability_Click(object sender, System.EventArgs e)
        {
            if (frmFixCapability == null)
            {
                frmFixCapability = new FixCapability();
                frmFixCapability.Owner = this;
                frmFixCapability.ShowInTaskbar = false;
            }
            frmFixCapability.Show();
            frmFixCapability.Activate();
            frmFixCapability.Disposed += new EventHandler(frmFixCapability_Disposed);
        }

        private void mnuFileNewModule_Click(object sender, System.EventArgs e)
        {
            if (frmModule == null)
            {
                frmModule = new Module();
                frmModule.Owner = this;
                frmModule.ShowInTaskbar = false;
            }
            frmModule.Show();
            frmModule.Activate();
            frmModule.Disposed += new EventHandler(frmModule_Disposed);
        }

        private void mnuFileNewVenueType_Click(object sender, System.EventArgs e)
        {
            if (frmVenueType == null)
            {
                frmVenueType = new VenueType();
                frmVenueType.Owner = this;
                frmVenueType.ShowInTaskbar = false;
            }
            frmVenueType.Show();
            frmVenueType.Activate();
            frmVenueType.Disposed += new EventHandler(frmVenueType_Disposed);
        }

        private void mnuFileNewCounterPartyType_Click(object sender, System.EventArgs e)
        {
            if (frmCounterPartyType == null)
            {
                frmCounterPartyType = new CounterPartyType();
                frmCounterPartyType.Owner = this;
                frmCounterPartyType.ShowInTaskbar = false;
            }
            frmCounterPartyType.Show();
            frmCounterPartyType.Activate();
            frmCounterPartyType.Disposed += new EventHandler(frmCounterPartyType_Disposed);
        }

        //		private void mnuFileNewSymbolIdentifier_Click(object sender, System.EventArgs e)
        //		{
        //			if(frmSymbolConvention == null)
        //			{
        //				frmSymbolConvention = new SymbolConvention();
        //				frmSymbolConvention.Owner = this;			
        //				frmSymbolConvention.ShowInTaskbar = false;
        //			}
        //			frmSymbolConvention.Show();
        //			frmSymbolConvention.Disposed +=new EventHandler(frmSymbolConvention_Disposed);
        //		}

        private void mnuFileNewClearingFirmPrimeBroker_Click(object sender, System.EventArgs e)
        {
            if (frmClearingFirmsPrimeBrokers == null)
            {
                frmClearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();
                frmClearingFirmsPrimeBrokers.Owner = this;
                frmClearingFirmsPrimeBrokers.ShowInTaskbar = false;
            }
            frmClearingFirmsPrimeBrokers.Show();
            frmClearingFirmsPrimeBrokers.Disposed += new EventHandler(frmClearingFirmsPrimeBrokers_Disposed);
        }

        private void mnuFileNewSymbolConvention_Click(object sender, System.EventArgs e)
        {
            if (frmSymbolConvention == null)
            {
                frmSymbolConvention = new SymbolConvention();
                frmSymbolConvention.Owner = this;
                frmSymbolConvention.ShowInTaskbar = false;
            }
            frmSymbolConvention.Show();
            frmSymbolConvention.Activate();
            frmSymbolConvention.Disposed += new EventHandler(frmSymbolConvention_Disposed);
        }

        private void mnuFileNewCurrencyType_Click(object sender, System.EventArgs e)
        {
            if (frmCurrencyType == null)
            {
                frmCurrencyType = new CurrencyType();
                frmCurrencyType.Owner = this;
                frmCurrencyType.ShowInTaskbar = false;
            }
            frmCurrencyType.Show();
            frmCurrencyType.Activate();
            frmCurrencyType.Disposed += new EventHandler(frmCurrencyType_Disposed);
        }

        private void mnuFileNewContractListingType_Click(object sender, System.EventArgs e)
        {
            if (frmContractListingType == null)
            {
                frmContractListingType = new ContractListingType();
                frmContractListingType.Owner = this;
                frmContractListingType.ShowInTaskbar = false;
            }
            frmContractListingType.Show();
            frmContractListingType.Activate();
            frmContractListingType.Disposed += new EventHandler(frmContractListingType_Disposed);
        }

        private void mnuFileNewFutureMonthCodes_Click(object sender, System.EventArgs e)
        {
            if (frmFutureMonthCode == null)
            {
                frmFutureMonthCode = new FutureMonthCode();
                frmFutureMonthCode.Owner = this;
                frmFutureMonthCode.ShowInTaskbar = false;
            }
            frmFutureMonthCode.Show();
            frmFutureMonthCode.Activate();
            frmFutureMonthCode.Disposed += new EventHandler(frmFutureMonthCode_Disposed);
        }

        private void mnuFileNewHoliday_Click(object sender, System.EventArgs e)
        {
            if (frmHoliday == null)
            {
                frmHoliday = new HolidayForm();
                frmHoliday.Owner = this;
                frmHoliday.ShowInTaskbar = false;
            }
            frmHoliday.Show();
            frmHoliday.Activate();
            frmHoliday.Disposed += new EventHandler(frmHoliday_Disposed);
        }

        private void mnuFileOpenRiskManagement_Click(object sender, System.EventArgs e)
        {
            if (frmRiskManagement == null)
            {
                frmRiskManagement = new Prana.Admin.RiskManagement.RM();
                frmRiskManagement.Owner = this;
                frmRiskManagement.ShowInTaskbar = false;
            }
            frmRiskManagement.Show();
            frmRiskManagement.Activate();
            frmRiskManagement.Disposed += new EventHandler(frmRiskManagement_Disposed);
        }

        private void mnuFileOpenCommissionRules_Click(object sender, System.EventArgs e)
        {
            if (frmCommissionRulesNew == null)
            {
                //frmCommissionRulesNew = new Prana.Admin.CommissionRules.CommissionRules();
                frmCommissionRulesNew = new Prana.AdminForms.CommissionRulesNew();
                frmCommissionRulesNew.Owner = this;
                frmCommissionRulesNew.ShowInTaskbar = false;
            }
            frmCommissionRulesNew.Show();
            frmCommissionRulesNew.Activate();
            frmCommissionRulesNew.Disposed += new EventHandler(frmCommissionRulesNew_Disposed);
        }


        private void mnuHelpAboutPrana_Click(object sender, System.EventArgs e)
        {
            if (frmPranaHelp == null)
            {
                frmPranaHelp = new Prana.Admin.PranaHelp();
                frmPranaHelp.Owner = this;
                frmPranaHelp.ShowInTaskbar = false;
            }
            frmPranaHelp.Show();
            frmPranaHelp.Activate();
            frmPranaHelp.Disposed += new EventHandler(frmPranaHelp_Disposed);
        }

        private void mnuVendor_Click(object sender, EventArgs e)
        {
            if (frm_Vendor == null)
            {
                frm_Vendor = new Prana.AdminForms.ThirdParty.ThirdPartyVendor();
                frm_Vendor.Owner = this;
            }
            frm_Vendor.Show();
            frm_Vendor.Activate();
            frm_Vendor.Disposed += new EventHandler(frm_Vendor_Disposed);
        }

        private void mnuFilePositionManagement_Click(object sender, EventArgs e)
        {
            if (frmPM == null)
            {
                frmPM = new PMMain();
                frmPM.Owner = this;
            }
            frmPM.Show();
            frmPM.Activate();
            frmPM.Disposed += new EventHandler(frmPM_Disposed);
        }

        private void mnuReconXSLTSetup_Click(object sender, EventArgs e)
        {
            if (frmReconXSLTSetup == null)
            {
                frmReconXSLTSetup = new ReconXSLTSetup();
                frmReconXSLTSetup.Owner = this;
            }
            frmReconXSLTSetup.Show();
            frmReconXSLTSetup.Activate();
            frmReconXSLTSetup.Disposed += new EventHandler(frmReconXSLTSetup_Disposed);
        }

        private void frmReconXSLTSetup_Disposed(object sender, EventArgs e)
        {
            frmReconXSLTSetup = null;
        }

        private void mnuEMSImportExport_Click(object sender, EventArgs e)
        {
            if (frmEMSImportExport == null)
            {
                frmEMSImportExport = new EMSImportExport();
                frmEMSImportExport.Owner = this;
            }
            frmEMSImportExport.Show();
            frmEMSImportExport.Activate();
            frmEMSImportExport.Disposed += new EventHandler(frmEMSImportExport_Disposed);

        }

        private void frmEMSImportExport_Disposed(object sender, EventArgs e)
        {
            frmEMSImportExport = null;
        }

        private void mnuCalendar_Click(object sender, EventArgs e)
        {

            Prana.AdminForms.CalendarHolidays cholidays = Prana.AdminForms.CalendarHolidays.GetInstance();
            cholidays.Show();


        }

        /// <summary>
        /// Rahul 20120316 Details:http://jira.nirvanasolutions.com:8080/browse/PRANA-1713
        /// Open the Country form to add and edit countries.
        /// </summary>
        private void mnuFileOpenCountry_Click(object sender, EventArgs e)
        {
            CountryForm frmCountry = CountryForm.GetInstance();
            frmCountry.Show();

        }

        /// <summary>
        /// Rahul 20120316 
        /// Open the State Form to add and edit States.
        /// </summary>
        private void mnuFileOpenState_Click(object sender, EventArgs e)
        {
            StateForm frmState = StateForm.GetInstance();
            frmState.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        public NirvanaPrincipal authorizedPrincipal { get; set; }
    }
}
