using ExportGridsData;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects.AppConstants;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prana.TradeManager.Forms
{
    public enum TradingRuleType
    {
        OverBuyOverSellRule,
        FatFingerRule,
        SharesOutstandingRule
    }

    public partial class TradingRulesViolatedPopUp : Form, IExportGridData
    {
        public TradingRulesViolatedPopUp()
        {
            try
            {
                InitializeComponent();

                if (CustomThemeHelper.ApplyTheme)
                {
                    CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PRANA_SHORTLOCATE);
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, "Trading Rules Violated", CustomThemeHelper.UsedFont);
                    SetButtonsColor();
                }

                ultraExpandableGroupBoxOverBuyOverSell.Visible = false;
                ultraExpandableGroupBoxFatFinger.Visible = false;
                ultraExpandableGroupBoxSharesOutstanding.Visible = false;
                ultraExpandableGroupBoxOverBuyOverSell.Expanded = false;
                ultraExpandableGroupBoxFatFinger.Expanded = false;
                ultraExpandableGroupBoxSharesOutstanding.Expanded = false;
                InstanceManager.RegisterInstance(this);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Bind the data for Trading Rules
        /// </summary>
        private Dictionary<TradingRuleType, TradingRuleViolatedData> violatedRules;
        internal void DataBind(Dictionary<TradingRuleType, TradingRuleViolatedData> dictTradingRuleViolatedData)
        {
            try
            {
                violatedRules = dictTradingRuleViolatedData;
                if (dictTradingRuleViolatedData != null)
                {
                    if (dictTradingRuleViolatedData.ContainsKey(TradingRuleType.OverBuyOverSellRule))
                    {
                        ultraExpandableGroupBoxOverBuyOverSell.Text = dictTradingRuleViolatedData[TradingRuleType.OverBuyOverSellRule].TitleMessage;

                        ultraExpandableGroupBoxOverBuyOverSell.Visible = true;
                        ultraGridOverBuyOverSellRuleViolated.DataSource = dictTradingRuleViolatedData[TradingRuleType.OverBuyOverSellRule].TradingRuleViolatedParameter;
                    }
                    if (dictTradingRuleViolatedData.ContainsKey(TradingRuleType.FatFingerRule))
                    {

                        ultraExpandableGroupBoxFatFinger.Text = dictTradingRuleViolatedData[TradingRuleType.FatFingerRule].TitleMessage;

                        ultraExpandableGroupBoxFatFinger.Visible = true;
                        ultraGridFatFingerRuleViolated.DataSource = dictTradingRuleViolatedData[TradingRuleType.FatFingerRule].TradingRuleViolatedParameter;
                        if ((int)TradingTktPrefs.TradingTicketRulesPrefs.IsAbsoluteAmountOrDefinePercent == (int)AbsoluteAmountOrDefinePercent.AbsoluteAmount)
                        {
                            ultraGridFatFingerRuleViolated.DisplayLayout.Bands[0].Columns["NavPercent"].Header.Caption = "Notional";
                        }
                        if ((int)TradingTktPrefs.TradingTicketRulesPrefs.FatFingerAccountOrMasterFund == (int)FundSelectionType.Account)
                        {
                            ultraGridFatFingerRuleViolated.DisplayLayout.Bands[0].Columns["Masterfund"].Hidden = true;
                        }
                        if (!dictTradingRuleViolatedData[TradingRuleType.FatFingerRule].AllowExpand)
                        {
                            ultraExpandableGroupBoxFatFinger.Expanded = dictTradingRuleViolatedData[TradingRuleType.FatFingerRule].AllowExpand;
                            ultraExpandableGroupBoxFatFinger.Enabled = dictTradingRuleViolatedData[TradingRuleType.FatFingerRule].AllowExpand;
                        }
                        if (!dictTradingRuleViolatedData[TradingRuleType.FatFingerRule].AllowExpand)
                        {
                            ultraExpandableGroupBoxFatFinger.Expanded = dictTradingRuleViolatedData[TradingRuleType.FatFingerRule].AllowExpand;
                            ultraExpandableGroupBoxFatFinger.Enabled = dictTradingRuleViolatedData[TradingRuleType.FatFingerRule].AllowExpand;
                        }
                        if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                           && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        {
                            ultraGridFatFingerRuleViolated.DisplayLayout.Bands[0].Columns[ApplicationConstants.CONST_NavPercent].NullText = ApplicationConstants.CONST_CensorValue;
                        }
                    }
                    if (dictTradingRuleViolatedData.ContainsKey(TradingRuleType.SharesOutstandingRule))
                    {
                        ultraExpandableGroupBoxSharesOutstanding.Text = dictTradingRuleViolatedData[TradingRuleType.SharesOutstandingRule].TitleMessage;
                        ultraExpandableGroupBoxSharesOutstanding.Visible = true;
                        ultraGridSharesOutstandingRuleViolated.DataSource = dictTradingRuleViolatedData[TradingRuleType.SharesOutstandingRule].TradingRuleViolatedParameter;
                        if ((int)TradingTktPrefs.TradingTicketRulesPrefs.SharesOutstandingAccOrMF == (int)FundSelectionType.Portfolio)
                        {
                            ultraGridSharesOutstandingRuleViolated.DisplayLayout.Bands[0].Columns["Masterfund"].Hidden = true;
                            ultraGridSharesOutstandingRuleViolated.DisplayLayout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].Hidden = true;
                        }
                        if ((int)TradingTktPrefs.TradingTicketRulesPrefs.SharesOutstandingAccOrMF == (int)FundSelectionType.Account)
                        {
                            ultraGridSharesOutstandingRuleViolated.DisplayLayout.Bands[0].Columns["Masterfund"].Hidden = true;
                        }
                        ultraExpandableGroupBoxSharesOutstanding.Enabled = dictTradingRuleViolatedData[TradingRuleType.SharesOutstandingRule].AllowExpand;
                        if (CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                            && !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                        {
                            ultraGridSharesOutstandingRuleViolated.DisplayLayout.Bands[0].Columns[ApplicationConstants.CONST_SharesOutstandingPercent].NullText = ApplicationConstants.CONST_CensorValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void TradingRulesViolatedPopUp_Load(object sender, EventArgs e)
        {
            try
            {
                if (violatedRules != null)
                {
                    if (violatedRules.ContainsKey(TradingRuleType.OverBuyOverSellRule))
                    {
                        ultraExpandableGroupBoxOverBuyOverSell.Expanded = true;
                    }
                    if (violatedRules.ContainsKey(TradingRuleType.FatFingerRule))
                    {
                        if (!ultraExpandableGroupBoxOverBuyOverSell.Expanded)
                        {
                            ultraExpandableGroupBoxFatFinger.Expanded = violatedRules[TradingRuleType.FatFingerRule].AllowExpand;
                        }
                    }
                    if (violatedRules.ContainsKey(TradingRuleType.SharesOutstandingRule))
                    {
                        if (!ultraExpandableGroupBoxOverBuyOverSell.Expanded && !ultraExpandableGroupBoxFatFinger.Expanded)
                        {
                            ultraExpandableGroupBoxSharesOutstanding.Expanded = violatedRules[TradingRuleType.SharesOutstandingRule].AllowExpand;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraGridOverBuyOverSellRuleViolated_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].Header.Caption = "Symbol";
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].Header.VisiblePosition = 2;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].Width = 100;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].CellActivation = Activation.NoEdit;

                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].Header.Caption = "Account Name";
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].Header.VisiblePosition = 3;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].Width = 105;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].CellActivation = Activation.NoEdit;

                e.Layout.Bands[0].Columns["Masterfund"].Header.Caption = "Masterfund";
                e.Layout.Bands[0].Columns["Masterfund"].Header.VisiblePosition = 4;
                e.Layout.Bands[0].Columns["Masterfund"].Width = 90;
                e.Layout.Bands[0].Columns["Masterfund"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["Masterfund"].Hidden = true;

                e.Layout.Bands[0].Columns["TradeQuantity"].Header.Caption = "Trade Quantity";
                e.Layout.Bands[0].Columns["TradeQuantity"].Header.VisiblePosition = 5;
                e.Layout.Bands[0].Columns["TradeQuantity"].Width = 115;
                e.Layout.Bands[0].Columns["TradeQuantity"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["TradeQuantity"].Format = "#,####0.#########";


                e.Layout.Bands[0].Columns["CurrentPosition"].Header.Caption = "Current Position";
                e.Layout.Bands[0].Columns["CurrentPosition"].Header.VisiblePosition = 6;
                e.Layout.Bands[0].Columns["CurrentPosition"].Width = 90;
                e.Layout.Bands[0].Columns["CurrentPosition"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["CurrentPosition"].Format = "#,####0.###########";

                e.Layout.Bands[0].Columns["NavPercent"].Header.Caption = "% of NAV";
                e.Layout.Bands[0].Columns["NavPercent"].Header.VisiblePosition = 7;
                e.Layout.Bands[0].Columns["NavPercent"].Width = 90;
                e.Layout.Bands[0].Columns["NavPercent"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["NavPercent"].Format = "#,####0.##";
                e.Layout.Bands[0].Columns["NavPercent"].Hidden = true;

                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Header.Caption = "% of Shares Outstanding";
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Header.VisiblePosition = 7;
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Width = 90;
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Format = "#,####0.##";
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Hidden = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraGridFatFingerRuleViolated_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].Header.Caption = "Symbol";
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].Header.VisiblePosition = 2;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].Width = 100;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].CellActivation = Activation.NoEdit;

                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].Header.Caption = "Account Name";
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].Header.VisiblePosition = 3;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].Width = 105;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].CellActivation = Activation.NoEdit;

                e.Layout.Bands[0].Columns["Masterfund"].Header.Caption = "Masterfund";
                e.Layout.Bands[0].Columns["Masterfund"].Header.VisiblePosition = 4;
                e.Layout.Bands[0].Columns["Masterfund"].Width = 90;
                e.Layout.Bands[0].Columns["Masterfund"].CellActivation = Activation.NoEdit;

                e.Layout.Bands[0].Columns["TradeQuantity"].Header.Caption = "Trade Quantity";
                e.Layout.Bands[0].Columns["TradeQuantity"].Header.VisiblePosition = 5;
                e.Layout.Bands[0].Columns["TradeQuantity"].Width = 115;
                e.Layout.Bands[0].Columns["TradeQuantity"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["TradeQuantity"].Format = "#,####0.###########";


                e.Layout.Bands[0].Columns["CurrentPosition"].Header.Caption = "Current Position";
                e.Layout.Bands[0].Columns["CurrentPosition"].Header.VisiblePosition = 6;
                e.Layout.Bands[0].Columns["CurrentPosition"].Width = 90;
                e.Layout.Bands[0].Columns["CurrentPosition"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["CurrentPosition"].Format = "#,####0.###########";
                e.Layout.Bands[0].Columns["CurrentPosition"].Hidden = true;

                e.Layout.Bands[0].Columns["NavPercent"].Header.Caption = "% of NAV";
                e.Layout.Bands[0].Columns["NavPercent"].Header.VisiblePosition = 7;
                e.Layout.Bands[0].Columns["NavPercent"].Width = 90;
                e.Layout.Bands[0].Columns["NavPercent"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["NavPercent"].Format = "#,####0.##";

                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Header.Caption = "% of Shares Outstanding";
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Header.VisiblePosition = 7;
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Width = 90;
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Format = "#,####0.##";
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Hidden = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        private void ultraGridSharesOutstandingRuleViolated_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].Header.Caption = "Symbol";
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].Header.VisiblePosition = 2;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].Width = 100;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_SYMBOL].CellActivation = Activation.NoEdit;

                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].Header.Caption = "Account Name";
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].Header.VisiblePosition = 3;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].Width = 105;
                e.Layout.Bands[0].Columns[Prana.Global.ApplicationConstants.CONST_FUNDNAME].CellActivation = Activation.NoEdit;


                e.Layout.Bands[0].Columns["Masterfund"].Header.Caption = "Masterfund";
                e.Layout.Bands[0].Columns["Masterfund"].Header.VisiblePosition = 4;
                e.Layout.Bands[0].Columns["Masterfund"].Width = 90;
                e.Layout.Bands[0].Columns["Masterfund"].CellActivation = Activation.NoEdit;

                e.Layout.Bands[0].Columns["TradeQuantity"].Header.Caption = "Trade Quantity";
                e.Layout.Bands[0].Columns["TradeQuantity"].Header.VisiblePosition = 5;
                e.Layout.Bands[0].Columns["TradeQuantity"].Width = 115;
                e.Layout.Bands[0].Columns["TradeQuantity"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["TradeQuantity"].Format = "#,####0.###########";


                e.Layout.Bands[0].Columns["CurrentPosition"].Header.Caption = "Current Position";
                e.Layout.Bands[0].Columns["CurrentPosition"].Header.VisiblePosition = 6;
                e.Layout.Bands[0].Columns["CurrentPosition"].Width = 90;
                e.Layout.Bands[0].Columns["CurrentPosition"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["CurrentPosition"].Format = "#,####0.###########";

                e.Layout.Bands[0].Columns["NavPercent"].Header.Caption = "% of NAV";
                e.Layout.Bands[0].Columns["NavPercent"].Header.VisiblePosition = 7;
                e.Layout.Bands[0].Columns["NavPercent"].Width = 90;
                e.Layout.Bands[0].Columns["NavPercent"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["NavPercent"].Format = "#,####0.##";
                e.Layout.Bands[0].Columns["NavPercent"].Hidden = true;

                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Header.Caption = "% of Shares Outstanding";
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Header.VisiblePosition = 7;
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Width = 90;
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].CellActivation = Activation.NoEdit;
                e.Layout.Bands[0].Columns["SharesOutstandingPercent"].Format = "#,####0.##";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool _isTradeAllowed = false;

        internal Infragistics.Win.Misc.UltraButton BtnContinue
        {
            get { return ultraButtonYes; }
        }

        internal Infragistics.Win.Misc.UltraButton BtnEdit
        {
            get { return ultraButtonNo; }
        }

        internal bool ShouldTrade
        {
            get { return _isTradeAllowed; }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            _isTradeAllowed = true;
            this.Close();
            InstanceManager.ReleaseInstance(typeof(TradingRulesViolatedPopUp));
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            _isTradeAllowed = false;
            this.Close();
            InstanceManager.ReleaseInstance(typeof(TradingRulesViolatedPopUp));
        }

        /// <summary>
        /// Set the color of the Button
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                ultraButtonNo.ButtonStyle = UIElementButtonStyle.Button3D;
                ultraButtonNo.BackColor = Color.FromArgb(140, 5, 5);
                ultraButtonNo.ForeColor = Color.White;
                ultraButtonNo.UseAppStyling = false;
                ultraButtonNo.UseOsThemes = DefaultableBoolean.False;

                ultraButtonYes.ButtonStyle = UIElementButtonStyle.Button3D;
                ultraButtonYes.BackColor = Color.FromArgb(104, 156, 46);
                ultraButtonYes.ForeColor = Color.White;
                ultraButtonYes.UseAppStyling = false;
                ultraButtonYes.UseOsThemes = DefaultableBoolean.False;

                ultraGridFatFingerRuleViolated.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.False;
                ultraGridOverBuyOverSellRuleViolated.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.False;
                ultraGridSharesOutstandingRuleViolated.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///  used To export data for automation.
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            try
            {
                // Create a new instance of the exporter
                UltraGridExcelExporter exporter = new UltraGridExcelExporter();
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!System.IO.Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                // Perform the export
                if (gridName == "ultraGridOverBuyOverSellRuleViolated")
                    exporter.Export(ultraGridOverBuyOverSellRuleViolated, filePath);
                else if (gridName == "ultraGridFatFingerRuleViolated")
                    exporter.Export(ultraGridFatFingerRuleViolated, filePath);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
