#region Author Summary
///////////////////////////////////////////////////////////////////////////////
// AUTHOR   		 : Bharat Jangir
// CREATION DATE	 : 20 September 2013 
// PURPOSE	    	 : Apply Dynamic Themes on Controls
// FILE NAME	     : $Workfile: CustomThemeHelper.cs $	
///////////////////////////////////////////////////////////////////////////////
#endregion

#region Using NameSpaces
using Infragistics.Win.AppStyling;
using Infragistics.Win.UltraWinForm;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Appearance = Infragistics.Win.Appearance;

#endregion

namespace Prana.Utilities.UI.UIUtilities
{
    public static class CustomThemeHelper
    {
        #region Theme Constants
        /// <summary>
        /// The file name in which the theme is defined. The file name is decided to be DefaultTheme.isl for any theme which Nirvana will support
        /// </summary>
        public const string THEME_ISL_NAME = "DefaultTheme.isl";
        /// <summary>
        /// The styleLibraryName we are using is a constant and is DefaultTheme for all cases
        /// </summary>
        public const string THEME_STYLELIBRARYNAME = "DefaultTheme";
        #endregion
        /// <summary>
        /// The private variable apply_theme is set at the start of application from WhiteLabelTheme class
        /// </summary>
        private static bool _applyTheme;

        public static bool ApplyTheme
        {
            get { return _applyTheme; }
            set
            {
                _applyTheme = value;
                if (value)
                    LoadTheme();
            }
        }

        /// <summary>
        /// The private variable _dynamicThemeName is set at the start of application from WhiteLabelTheme class
        /// </summary>
        private static string _dynamicThemeName = THEME_ISL_NAME;

        /// <summary>
        /// Dynamic Theme Name
        /// </summary>
        public static string DynamicThemeName
        {
            get { return CustomThemeHelper._dynamicThemeName; }
            set { CustomThemeHelper._dynamicThemeName = value; }
        }

        /// <summary>
        /// The theme name to be applied, is set at the start from the WhiteLabelTheme class in Prana project
        /// </summary>
        public static string WHITELABELTHEME;
        public const string PRODUCT_COMPANY_NAME = "Nirvāna";
        public const string REJECT_POPUP = "Order Rejected";
        public const string PENDINGNEW_POPUP = "Order Pending New";

        #region StyleSetName Constants
        //public const string THEME_STYLESETNAME_ALLOCATION = "AllocationStyleSet"; same as blotter currently
        public const string THEME_STYLESETNAME_BLOTTER = "BlotterStyleSet";
        //public const string THEME_STYLESETNAME_EDIT_TRADES = "EditTradesStyleSet"; same as blotter currently
        public const string THEME_STYLESETNAME_TRADING_TICKET = "TradingTicketStyleSet";
        public const string THEME_STYLESETNAME_SMALL_TRADING_TICKET = "SmallTradingTicketStyleSet";
        public const string THEME_STYLESETNAME_PM = "PMStyleSet";
        public const string THEME_STYLESETNAME_RISK_MANAGEMENT = "RiskStyleSet";
        public const string THEME_STYLESETNAME_MULTI_TRADING_TICKET = "MultiTTStyleSheet";
        public const string THEME_STYLESETNAME_NIRVANA_MAIN = "MainUIStyleSet";
        public const string THEME_STYLESETNAME_CASH_MANAGEMENT = "CashMgmtStyleSet";
        public const string THEME_STYLESETNAME_CASH_ACCOUNTS = "CashAccountsStyleSet";
        public const string THEME_STYLESETNAME_BLOTTER_NEW = "PranaBlotterStyleSet";
        public const string THEME_STYLESETNAME_WATCHLIST = "PranaWatchlistStyleSet";
        public const string THEME_STYLESETNAME_COMPLIANCE_POPUP = "PranaCompliancePopUpStyleSet";
        public const string THEME_STYLESETNAME_OPTIONCHAIN = "OptionChainStyleSet";
        public const string THEME_STYLESETNAME_OPTIONCHAIN_GRID = "OptionChainGridStyleSet";
        public const string THEME_STYLESETNAME_AUDIT_TRAIL = "AuditTrailStyleSet";
        public const string THEME_STYLESETNAME_PRICING_INPUTS = "PricingInputsStyleSet";
        public const string THEME_STYLESETNAME_DAILY_PM_CLIENTUI = "DailyValuationStyleSet";
        public const string THEME_STYLESETNAME_SYMBOL_LOOKUP = "SymbolLookupStyleSet";
        public const string THEME_STYLESETNAME_IMPORT_DATA = "ImportDataStyleSet";
        public const string THEME_STYLESETNAME_CORP_ACTION = "CorpActionStyleSet";
        public const string THEME_STYLESETNAME_RECONCILATION = "ReconcilationStyleSet";
        public const string THEME_STYLESETNAME_CREATE_TRANSACTION = "CreateTransactionStyleSet";
        public const string THEME_STYLESETNAME_CLOSE_TRADE = "CloseTradeStyleSet";
        public const string THEME_STYLESETNAME_ALLOCATION_MAIN = "AllocationStyleSet";
        public const string THEME_STYLESETNAME_ALLOCATION_FUND_ONLY_CTRL = "FundOnlyStyleSet";
        public const string THEME_STYLESETNAME_ALLOCATION_FUND_STRATEGY_CTRL = "FundStrategyStyleSet";
        public const string THEME_STYLESETNAME_PREFERENCES = "PreferencesStyleSet";
        public const string THEME_STYLE_NAME_MISSING_TRADES = "MissingTradesStyleSet";
        public const string THEME_STYLE_NAME_WATCHLIST = "WatchListStyleSet";
        public const string THEME_STYLE_NAME_THIRD_PARTY = "ThirdPartyStyleSet";
        public const string THEME_STYLE_NAME_THIRD_PARTY_CUSTOM = "ThirdPartyStyleSetCustom";
        public const string THEME_STYLESETNAME_COMPLIANCE_ENGINE = "ComplianceEngineStyleSet";
        public const string THEME_STYLESETNAME_ZERO_POSITION_ALERT = "ZeroPositionAlertStyleSet";
        public const string THEME_STYLESETNAME_MIDDLEWARE_MANAGER = "MiddlewareManagerStyleSet";
        public const string THEME_STYLESETNAME_CTRL_CLOSE_TRADE_ALLOCATION = "CtrlCloseTradeFromAllocationStyleSet";
        public const string THEME_STYLESETNAME_CONSOLIDATION_PANEL = "ConsolidationPanelStyleSet";
        public const string THEME_STYLESETNAME_HEAT_MAP = "HeatMapStyleSet";
        public const string THEME_STYLESETNAME_JOURNAL_PANAL = "JournalPanelStyleSet";
        public const string THEME_STYLESETNAME_PRANA_TRADING_TICKET = "PranaTradingTicketStyleSet";
        public const string THEME_STYLESETNAME_PRANA_QUICK_TRADING_TICKET = "PranaQuickTradingTicketStyleSet";
        public const string THEME_STYLESETNAME_CLOSING_WIZARD = "ClosingWizardStyleSet";
        public const string THEME_STYLESETNAME_CLOSE_TRADE_GRID = "CloseTradeGridStyleSet";
        public const string THEME_STYLESETNAME_IMPORT_DASHBOARD = "ImportDashboardStyleSet";
        public const string THEME_STYLESETNAME_BLOTTER_SUMMARY_GRID = "BlotterSummaryGridStyleSet";
        public const string THEME_STYLESETNAME_THIRD_PARTY_FILE_TYPE = "ThirdPartyFileTypeStyleSet";
        public const string THEME_STYLESETNAME_WORK_AREA = "WorkAreaStyleSet";
        public const string THEME_STYLESETNAME_RISK_MANAGEMENT_WRAP_HEADER = "RiskStyleSetWrapHeader";
        public const string THEME_STYLESETNAME_BLOTTER_OVERRIDE = "PranaBlotterOverride";
        public const string THEME_STYLESETNAME_BLOTTER_SUMMARY_GRID_OVERRIDE = "BlotterSummaryGridOverrideStyleSet";
        public const string THEME_STYLESETNAME_RECON_NEW = "ReconNewStyleSet";
        public const string THEME_STYLESETNAME_CUSTOM_FILTER = "CustomFilterStyleSet";
        public const string THEME_STYLESETNAME_PRANA_SHORTLOCATE = "PranaShortLocateStyleSet";
        public const string THEME_STYLESETNAME_LEFT_ALIGN_CONTROLS = "LeftAlignControlsStyleSet";
        public const string THEME_STYLESETNAME_PRANA_MULTILEG_IMPORT = "PranaMultiLegImportStyleSet";
        public const string THEME_STYLESETNAME_PRANA_SHORTCUTS = "PranaModuleShortcuts";
        public const string THEME_MESSAGE_WITH_GRID = "MessageWithGridTheme";

        public static Font UsedFont = new Font("Century Gothic", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
        public static Font TitleFont = new Font("Mulish", 11F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
        public static Appearance Appearance1 = new Appearance();
        #endregion

        static FileInfo _styleFileName;

        public static FileInfo StyleFileName
        {
            get { return _styleFileName; }
            set { _styleFileName = value; }
        }

        #region Method:LoadTheme

        /// <summary>
        /// This method is used for applying theme on controls
        /// </summary>
        /// <returns></returns>
        public static void LoadTheme()
        {
            try
            {
                //_styleFileName = new FileInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Themes\" + WHITELABELTHEME + @"\" + THEME_ISL_NAME);
                _styleFileName = new FileInfo(Application.StartupPath + @"\Themes\" + WHITELABELTHEME + @"\StyleLibrary\" + CustomThemeHelper.DynamicThemeName);
                if (WHITELABELTHEME != null && _styleFileName != null)
                    StyleManager.Load(_styleFileName.FullName);
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
        #endregion

        #region Method:SetThemeProperties

        /// <summary>
        /// This method is used for applying theme on controls
        /// </summary>
        /// <param name="control">Takes the control on which theme is to be applied</param>
        /// <param name="styleLibraryName">Name of the property style library</param>
        /// <param name="styleSetName">Name of the property style set</param>
        /// <returns></returns>
        public static void SetThemeProperties(Control control, String styleLibraryName, String styleSetName)
        {
            try
            {
                string tempStyleSetName = string.Empty;

                if (control != null)
                {
                    if (control is UltraCombo || control is UltraDropDown)
                    {
                        tempStyleSetName = THEME_STYLESETNAME_LEFT_ALIGN_CONTROLS;
                    }
                    else
                    {
                        tempStyleSetName = styleSetName;
                    }

                    //typeof(control).is
                    PropertyDescriptorCollection tempObjectPropertyCollection = TypeDescriptor.GetProperties(control);
                    PropertyDescriptor tempObjectStyleLibraryNameProperty = tempObjectPropertyCollection.Find("StyleLibraryName", true);
                    PropertyDescriptor tempObjectStyleSetNameProperty = tempObjectPropertyCollection.Find("StyleSetName", true);
                    PropertyDescriptor tempObjectDefaultStyleLibraryNameProperty = tempObjectPropertyCollection.Find("DefaultStyleLibraryName", true);
                    PropertyDescriptor tempObjectDefaultStyleSetName = tempObjectPropertyCollection.Find("DefaultStyleSetName", true);

                    if (!ApplyTheme)
                    {
                        PropertyDescriptor formDisplayStyle = tempObjectPropertyCollection.Find("FormDisplayStyle", true);
                        PropertyDescriptor isGlassSupported = tempObjectPropertyCollection.Find("IsGlassSupported", true);
                        if (tempObjectPropertyCollection != null && formDisplayStyle != null && isGlassSupported != null)
                        {
                            formDisplayStyle.SetValue(control, FormDisplayStyle.Standard);
                            isGlassSupported.SetValue(control, true);
                        }
                    }
                    else
                    {
                        if (tempObjectPropertyCollection != null)
                        {
                            //if (control is Form)
                            //{
                            //    if (fontTypeProperty != null)
                            //    {
                            //        fontTypeProperty.SetValue(control, usedFont);
                            //    }
                            //}
                            if (tempObjectStyleLibraryNameProperty != null && tempObjectStyleSetNameProperty != null)
                            {
                                //tempObjectStyleLibraryNameProperty.SetValue(control, styleLibraryName);
                                if (tempObjectStyleSetNameProperty.GetValue(control) != null)
                                {
                                    tempObjectStyleSetNameProperty.SetValue(control, tempStyleSetName);
                                }
                            }
                            if (tempObjectDefaultStyleLibraryNameProperty != null && tempObjectDefaultStyleSetName != null)
                            {
                                //tempObjectDefaultStyleLibraryNameProperty.SetValue(control, styleLibraryName);
                                if (tempObjectDefaultStyleSetName.GetValue(control) != null)
                                {
                                    tempObjectDefaultStyleSetName.SetValue(control, tempStyleSetName);
                                }
                            }
                            PropertyDescriptor formDisplayStyle = tempObjectPropertyCollection.Find("FormDisplayStyle", true);
                            PropertyDescriptor isGlassSupported = tempObjectPropertyCollection.Find("IsGlassSupported", true);
                            if (formDisplayStyle != null && isGlassSupported != null)
                            {
                                formDisplayStyle.SetValue(control, FormDisplayStyle.RoundedSizable);
                                isGlassSupported.SetValue(control, false);
                            }
                        }
                    }
                    if (control.Controls != null)
                    {
                        foreach (Control child in control.Controls)
                        {
                            SetThemeProperties(child, styleLibraryName, styleSetName);
                        }
                    }

                    GetComponentsAndSetTheme(control, styleSetName);
                }
                IconSettingsForForm(control, styleSetName);
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

        private static void IconSettingsForForm(Control control, String styleSetName)
        {
            try
            {
                bool showIconsBool;
                if (Boolean.TryParse(ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ThemeAndWhiteLabeling, "ShowIcons"), out showIconsBool))
                {
                    if (showIconsBool)
                    {
                        SetFormIcon(control, styleSetName);
                    }
                    else
                    {
                        HideFormIcon(control, styleSetName);
                    }
                }
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

        static void GetComponentsAndSetTheme(Control control, String styleSetName)
        {
            try
            {
                Type t = control.GetType();
                object frm = (Object)control;//Activator.CreateInstance(t);
                FieldInfo field = t.GetField("components", BindingFlags.Instance | BindingFlags.NonPublic);
                if (field != null)
                {
                    IContainer componentContainer = field.GetValue(frm) as IContainer;
                    if (componentContainer != null)
                    {
                        for (int i = 0; i < componentContainer.Components.Count; i++)
                        {
                            SetThemePropertiesComponent(componentContainer.Components[i], styleSetName);
                        }
                    }
                }
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

        public static void SetThemePropertiesComponent(IComponent component, String styleSetName)
        {
            try
            {
                string tempStyleSetName = string.Empty;

                if (component != null)
                {
                    if (component is UltraCombo || component is UltraDropDown)
                    {
                        tempStyleSetName = THEME_STYLESETNAME_LEFT_ALIGN_CONTROLS;
                    }
                    else
                    {
                        tempStyleSetName = styleSetName;
                    }

                    PropertyDescriptorCollection tempObjectPropertyCollection = TypeDescriptor.GetProperties(component);
                    if (tempObjectPropertyCollection != null)
                    {
                        PropertyDescriptor tempObjectStyleLibraryNameProperty = tempObjectPropertyCollection.Find("StyleLibraryName", true);
                        PropertyDescriptor tempObjectStyleSetNameProperty = tempObjectPropertyCollection.Find("StyleSetName", true);
                        PropertyDescriptor tempObjectDefaultStyleLibraryNameProperty = tempObjectPropertyCollection.Find("DefaultStyleLibraryName", true);
                        PropertyDescriptor tempObjectDefaultStyleSetName = tempObjectPropertyCollection.Find("DefaultStyleSetName", true);
                        if (!ApplyTheme)
                        {
                            PropertyDescriptor formStyleSettings = tempObjectPropertyCollection.Find("FormStyleSettings", true);
                            PropertyDescriptor formDisplayStyle = tempObjectPropertyCollection.Find("FormDisplayStyle", true);
                            PropertyDescriptor isGlassSupported = tempObjectPropertyCollection.Find("IsGlassSupported", true);
                            if (formDisplayStyle != null)
                            {
                                formDisplayStyle.SetValue(component, FormDisplayStyle.Standard);
                            }
                            if (isGlassSupported != null)
                            {
                                isGlassSupported.SetValue(component, true);
                            }
                            if (formStyleSettings != null)
                            {
                                PropertyDescriptorCollection tempFormStyleChildSetting = formStyleSettings.GetChildProperties();
                                if (tempFormStyleChildSetting != null)
                                {
                                    formDisplayStyle = tempFormStyleChildSetting.Find("FormDisplayStyle", true);
                                    if (formDisplayStyle != null)
                                    {
                                        FormStyleSettings tempFormStyleSettings = (FormStyleSettings)formStyleSettings.GetValue(component);
                                        formDisplayStyle.SetValue(tempFormStyleSettings, FormDisplayStyle.Standard);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //if (component is Form)
                            //{
                            //    if (fontTypeProperty != null)
                            //    {
                            //        fontTypeProperty.SetValue(component, usedFont);
                            //    }
                            //}
                            if (tempObjectStyleLibraryNameProperty != null && tempObjectStyleSetNameProperty != null)
                            {
                                //tempObjectStyleLibraryNameProperty.SetValue(component, styleLibraryName);
                                if (tempObjectStyleSetNameProperty.GetValue(component) != null)
                                {
                                    tempObjectStyleSetNameProperty.SetValue(component, tempStyleSetName);
                                }
                            }
                            if (tempObjectDefaultStyleLibraryNameProperty != null && tempObjectDefaultStyleSetName != null)
                            {
                                //tempObjectDefaultStyleLibraryNameProperty.SetValue(component, styleLibraryName);
                                if (tempObjectDefaultStyleSetName.GetValue(component) != null)
                                {
                                    tempObjectDefaultStyleSetName.SetValue(component, tempStyleSetName);
                                }
                            }
                            PropertyDescriptor formStyleSettings = tempObjectPropertyCollection.Find("FormStyleSettings", true);
                            PropertyDescriptor formDisplayStyle = tempObjectPropertyCollection.Find("FormDisplayStyle", true);
                            PropertyDescriptor isGlassSupported = tempObjectPropertyCollection.Find("IsGlassSupported", true);
                            if (formDisplayStyle != null)
                            {
                                formDisplayStyle.SetValue(component, FormDisplayStyle.RoundedSizable);
                            }
                            if (isGlassSupported != null)
                            {
                                isGlassSupported.SetValue(component, false);
                            }
                            if (formStyleSettings != null)
                            {
                                PropertyDescriptorCollection tempFormStyleChildSetting = formStyleSettings.GetChildProperties();
                                if (tempFormStyleChildSetting != null)
                                {
                                    formDisplayStyle = tempFormStyleChildSetting.Find("FormDisplayStyle", true);
                                    if (formDisplayStyle != null)
                                    {
                                        FormStyleSettings tempFormStyleSettings = (FormStyleSettings)formStyleSettings.GetValue(component);
                                        formDisplayStyle.SetValue(tempFormStyleSettings, FormDisplayStyle.RoundedSizable);
                                    }
                                }
                            }
                        }
                    }
                }
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
        public static bool IsDesignMode()
        {
            try
            {
                bool bProcCheck = false;
                using (Process process = Process.GetCurrentProcess())
                {
                    bProcCheck = process.ProcessName.ToLower().Trim() == "devenv";
                }

                bool bModeCheck = (LicenseManager.UsageMode == LicenseUsageMode.Designtime);

                return bProcCheck || bModeCheck;
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
            return false;
        }
        public static void SetFormIcon(Control control, String styleSetName)
        {
            try
            {
                string styleIconFile = String.Format("{0}\\Themes\\{1}\\FormIcons\\{2}.ico", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), WHITELABELTHEME, styleSetName.Replace("StyleSet", ""));
                if (File.Exists(styleIconFile))
                {
                    if (control is Form)
                    {
                        (control as Form).Icon = new Icon(styleIconFile);
                    }
                }
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

        private static void HideFormIcon(Control control, string styleSetName)
        {
            try
            {
                if ((control is Form) && !styleSetName.Equals(THEME_STYLESETNAME_NIRVANA_MAIN))
                {
                    (control as Form).ShowIcon = false;
                }
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

        /// <summary>
        /// sets theme at the form if the `
        /// </summary>
        /// <param name="dynamicForm"></param>
        /// <param name="control"></param>
        public static void SetThemeAtDynamicForm(Form dynamicForm, Object control)
        {
            try
            {
                System.ComponentModel.IContainer dynamicComponents = new System.ComponentModel.Container();
                Infragistics.Win.UltraWinToolbars.UltraToolbarsManager ultraToolbarsManager1;
                Infragistics.Win.Misc.UltraPanel dynamicForm_Fill_Panel;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Left;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Right;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Bottom;
                Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea dynamicForm_Toolbars_Dock_Area_Top;
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1 = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(dynamicComponents);
                dynamicForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
                dynamicForm_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                dynamicForm_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).BeginInit();
                dynamicForm_Fill_Panel.SuspendLayout();
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-1691
                //SuspendLayout();
                // 
                // ultraToolbarsManager1
                // 
                ultraToolbarsManager1.DesignerFlags = 1;
                ultraToolbarsManager1.DockWithinContainer = dynamicForm;
                ultraToolbarsManager1.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
                ultraToolbarsManager1.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
                ultraToolbarsManager1.IsGlassSupported = false;
                // 
                // frmReconCancelAmend_Fill_Panel
                // 
                dynamicForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
                dynamicForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
                dynamicForm_Fill_Panel.Location = new System.Drawing.Point(4, 52);
                dynamicForm_Fill_Panel.Name = "dynamicForm_Fill_Panel";
                dynamicForm_Fill_Panel.Size = new System.Drawing.Size(576, 261);
                dynamicForm_Fill_Panel.TabIndex = 0;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Left
                // 
                dynamicForm_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
                dynamicForm_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 52);
                dynamicForm_Toolbars_Dock_Area_Left.Name = "dynamicForm_Toolbars_Dock_Area_Left";
                dynamicForm_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 261);
                dynamicForm_Toolbars_Dock_Area_Left.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Right
                // 
                dynamicForm_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
                dynamicForm_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(580, 52);
                dynamicForm_Toolbars_Dock_Area_Right.Name = "dynamicForm_Toolbars_Dock_Area_Right";
                dynamicForm_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 261);
                dynamicForm_Toolbars_Dock_Area_Right.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Top
                // 
                dynamicForm_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
                dynamicForm_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
                dynamicForm_Toolbars_Dock_Area_Top.Name = "dynamicForm_Toolbars_Dock_Area_Top";
                dynamicForm_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(584, 52);
                dynamicForm_Toolbars_Dock_Area_Top.ToolbarsManager = ultraToolbarsManager1;
                // 
                // _frmReconCancelAmend_Toolbars_Dock_Area_Bottom
                // 
                dynamicForm_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
                dynamicForm_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
                dynamicForm_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
                dynamicForm_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
                dynamicForm_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
                dynamicForm_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 313);
                dynamicForm_Toolbars_Dock_Area_Bottom.Name = "dynamicForm_Toolbars_Dock_Area_Bottom";
                dynamicForm_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(584, 4);
                dynamicForm_Toolbars_Dock_Area_Bottom.ToolbarsManager = ultraToolbarsManager1;
                // 
                // frm
                //    
                if (control as UserControl == null)
                {
                    UltraGrid grid = control as UltraGrid;
                    grid.Dock = DockStyle.Fill;
                    dynamicForm.Controls.Add(grid);
                }
                else
                {
                    UserControl userControl = control as UserControl;
                    userControl.Dock = DockStyle.Fill;
                    dynamicForm.Controls.Add(userControl);
                }

                dynamicForm.ShowInTaskbar = false;
                //dynamicForm.Size = new System.Drawing.Size(1107, 630);
                dynamicForm.Controls.Add(dynamicForm_Fill_Panel);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Left);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Right);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Bottom);
                dynamicForm.Controls.Add(dynamicForm_Toolbars_Dock_Area_Top);
                ((System.ComponentModel.ISupportInitialize)(ultraToolbarsManager1)).EndInit();
                dynamicForm_Fill_Panel.ClientArea.ResumeLayout(false);
                dynamicForm_Fill_Panel.ClientArea.PerformLayout();
                dynamicForm_Fill_Panel.ResumeLayout(false);
                dynamicForm.ResumeLayout(false);
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
        #endregion

        public static void AddUltraFormManagerToDynamicForm(Form frm)
        {
            try
            {
                UltraFormManager ultraFormManager1 = new UltraFormManager();
                ultraFormManager1.Form = frm;
                if (ApplyTheme)
                {
                    ultraFormManager1.FormStyleSettings.IsGlassSupported = false;
                    ultraFormManager1.FormStyleSettings.FormDisplayStyle = FormDisplayStyle.RoundedSizable;
                    if (WHITELABELTHEME.Equals("Nirvana"))
                    {
                        ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                        ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, frm.Text, CustomThemeHelper.UsedFont);
                    }
                    IconSettingsForForm(frm, string.Empty);
                }
                else
                {
                    ultraFormManager1.FormStyleSettings.IsGlassSupported = true;
                    ultraFormManager1.FormStyleSettings.FormDisplayStyle = FormDisplayStyle.Standard;
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

        public static void SetThemeStyleBasedOnRange(Control control, int lowerRange, int upperRange, String styleSetName)
        {
            try
            {
                double formHeight = control.FindForm().Height;
                if (control != null)
                {
                    if (control.Parent != null)
                    {
                        Point locationOnForm = control.FindForm().PointToClient(control.Parent.PointToScreen(control.Location));
                        double controlEndingY = locationOnForm.Y + control.Size.Height;
                        double endingRange = controlEndingY / formHeight * 100;
                        if (endingRange > lowerRange && endingRange <= upperRange)
                        {
                            //apply style
                            PropertyDescriptorCollection tempObjectPropertyCollection = TypeDescriptor.GetProperties(control);
                            PropertyDescriptor tempObjectStyleLibraryNameProperty = tempObjectPropertyCollection.Find("StyleLibraryName", true);
                            PropertyDescriptor tempObjectStyleSetNameProperty = tempObjectPropertyCollection.Find("StyleSetName", true);
                            PropertyDescriptor tempObjectDefaultStyleLibraryNameProperty = tempObjectPropertyCollection.Find("DefaultStyleLibraryName", true);
                            PropertyDescriptor tempObjectDefaultStyleSetName = tempObjectPropertyCollection.Find("DefaultStyleSetName", true);
                            if (tempObjectPropertyCollection != null)
                            {
                                if (tempObjectStyleLibraryNameProperty != null && tempObjectStyleSetNameProperty != null)
                                {
                                    //tempObjectStyleLibraryNameProperty.SetValue(control, styleLibraryName);
                                    if (tempObjectStyleSetNameProperty.GetValue(control) != null)
                                    {
                                        tempObjectStyleSetNameProperty.SetValue(control, styleSetName);
                                    }
                                }
                                if (tempObjectDefaultStyleLibraryNameProperty != null && tempObjectDefaultStyleSetName != null)
                                {
                                    //tempObjectDefaultStyleLibraryNameProperty.SetValue(control, styleLibraryName);
                                    if (tempObjectDefaultStyleSetName.GetValue(control) != null)
                                    {
                                        tempObjectDefaultStyleSetName.SetValue(control, styleSetName);
                                    }
                                }
                                PropertyDescriptor formDisplayStyle = tempObjectPropertyCollection.Find("FormDisplayStyle", true);
                                PropertyDescriptor isGlassSupported = tempObjectPropertyCollection.Find("IsGlassSupported", true);
                                if (formDisplayStyle != null && isGlassSupported != null)
                                {
                                    formDisplayStyle.SetValue(control, FormDisplayStyle.RoundedSizable);
                                    isGlassSupported.SetValue(control, false);
                                }
                            }
                        }

                    }
                    if (control.Controls.Count > 0)
                    {
                        foreach (Control child in control.Controls)
                        {
                            SetThemeStyleBasedOnRange(child, lowerRange, upperRange, styleSetName);
                        }
                    }
                }
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
    }
}
