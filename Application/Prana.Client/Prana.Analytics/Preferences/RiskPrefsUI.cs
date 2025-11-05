using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Analytics
{
    public partial class RiskPrefsUI : Form
    {
        private DataSet dsIR = null;
        private RiskPrefernece _riskPrefs;
        private bool _isSetupRiskParams = false;
        private bool _isSetupInterestRate = false;
        private bool _isSetupGrouping = false;
        private bool _isSetupAppearence = false;
        private bool _isSetupExportFileFormat = false;
        public event EventHandler PrefsUpdated;
        public RiskPrefsUI()
        {
            try
            {
                _riskPrefs = RiskPreferenceManager.RiskPrefernece;
                InitializeComponent();
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

        private void SetupRiskParams()
        {
            try
            {
                if (!_isSetupRiskParams)
                {
                    BindCombos();
                    SetPreferences();
                    _isSetupRiskParams = true;
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

        private void SetupInterestRate()
        {
            try
            {
                if (!_isSetupInterestRate)
                {
                    grdInterestRate.DataSource = null;
                    dsIR = _riskPrefs.InterestRateTable;
                    grdInterestRate.DataSource = dsIR;

                    grdInterestRate.Refresh();
                    UltraGridBand band = grdInterestRate.DisplayLayout.Bands[0];
                    band.Columns["Period"].Header.Caption = "Period(in Months)";
                    band.Columns["Rate"].Header.Caption = "Rate(%)";
                    _isSetupInterestRate = true;
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

        private void SetupGroupingCriteria()
        {
            try
            {
                //TODO:Need refactoring, can replace with a grid
                if (!_isSetupGrouping)
                {
                    // Grouping
                    List<string> listGroup = _riskPrefs.Grouping;

                    if (listGroup.Contains(chkBoxSide.Text))
                    {
                        chkBoxSide.Checked = true;
                    }
                    else
                    {
                        chkBoxSide.Checked = false;
                    }

                    if (listGroup.Contains(chkBoxAccount.Text))
                    {
                        chkBoxAccount.Checked = true;
                    }
                    else
                    {
                        chkBoxAccount.Checked = false;
                    }
                    if (listGroup.Contains(chkBoxStrategy.Text))
                    {
                        chkBoxStrategy.Checked = true;
                    }
                    else
                    {
                        chkBoxStrategy.Checked = false;
                    }
                    _isSetupGrouping = true;
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

        private void SetupAppearence()
        {
            try
            {
                if (!_isSetupAppearence)
                {
                    // Group Band Colors
                    cpBackLevel1.Color = Color.FromArgb(_riskPrefs.BackColorLevel1);
                    cpBackLevel2.Color = Color.FromArgb(_riskPrefs.BackColorLevel2);
                    cpBackLevel3.Color = Color.FromArgb(_riskPrefs.BackColorLevel3);
                    cpForeLevel1.Color = Color.FromArgb(_riskPrefs.ForeColorLevel1);
                    cpForeLevel2.Color = Color.FromArgb(_riskPrefs.ForeColorLevel2);
                    cpForeLevel3.Color = Color.FromArgb(_riskPrefs.ForeColorLevel3);
                    chkboxWrapHeader.Checked = Convert.ToBoolean(_riskPrefs.WrapHeader);
                    numericUpDownFontSize.Value = _riskPrefs.FontSize;
                    cpSummaryColor.Color = Color.FromArgb(_riskPrefs.ColorSummaryText);
                    _isSetupAppearence = true;

                    if (CustomThemeHelper.ApplyTheme)
                    {
                        groupBox2.Visible = false;
                    }
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

        private void SetPreferences()
        {
            try
            {
                cmbVolatilityType.Value = _riskPrefs.VolatilityType;
                cmbVARModel.Value = _riskPrefs.Method;
                cmbUnderlyingPriceType.Value = _riskPrefs.UnderlyingPriceType;
                numericUpDownLambda.Value = Convert.ToDecimal(_riskPrefs.Lambda);
                cmbRiskCalculationBasedOn.Value = _riskPrefs.RiskCalculationBasedOn;
                cmbRiskCalculationCurrency.Value = _riskPrefs.RiskCalculationCurrency;
                numericUpDownDaysBwDataPoints.Value = _riskPrefs.DaysBwDataPoints;
                numericUpDownConfidenceLevel.Value = _riskPrefs.ConfidenceLevelPercent;
                checkBoxAutoLoadDataOnStartup.Checked = _riskPrefs.IsAutoLoadDataOnStartup;
                ultraNumericStressTest.Value = _riskPrefs._stressTestDateRange;
                ultraNumericRiskSimulation.Value = _riskPrefs._riskSimulationDateRange;
                ultraNumericRiskReport.Value = _riskPrefs._riskReportDateRange;

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

        private void BindCombos()
        {
            try
            {
                BindVARModelCombo();
                //BindConfidenceLevelCombo();
                BindVolatilityTypeCombo();
                BindUnderlyingPriceTypeCombo();
                BindColumnChooserListBox();
                BindCalculationBasedOnCombo();
                BindCurrencyCombo();
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

        private void BindDateFormatCombo()
        {
            try
            {
                var us = new CultureInfo("en-US");
                List<string> dateTimeFormats = new List<string>();
                dateTimeFormats.Add("yyy.MM.dd.HH.mm");
                Action<string> addItem = x => { if (!String.IsNullOrEmpty(x)) dateTimeFormats.Add(x); };
                foreach (string dateTime in DateTime.Now.GetDateTimeFormats().ToList())
                {
                    addItem(GuessPattern(dateTime, us));
                }
                dateTimeFormats = dateTimeFormats.Select<string, string>(s => s.Contains('/') ? s.Replace('/', '.') : s).ToList();
                ucmbDateFormatSelector.DataSource = null;
                ucmbDateFormatSelector.DataSource = dateTimeFormats;
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

        static string GuessPattern(string text, CultureInfo culture)
        {
            try
            {
                foreach (var pattern in GetDateTimePatterns(culture))
                {
                    DateTime ignored;
                    if (DateTime.TryParseExact(text, pattern, culture,
                                               DateTimeStyles.None, out ignored))
                    {
                        return pattern;
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
            return String.Empty;
        }

        static IList<string> GetDateTimePatterns(CultureInfo culture)
        {
            try
            {
                var info = culture.DateTimeFormat;
                return new string[]
                {
                    info.FullDateTimePattern,
                    info.LongDatePattern,
                    info.LongTimePattern,
                    info.ShortDatePattern,
                    info.ShortTimePattern,
                    info.MonthDayPattern,
                    info.ShortDatePattern + " " + info.LongTimePattern,
                    info.ShortDatePattern + " " + info.ShortTimePattern,
                    info.YearMonthPattern
                };
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
            return null;
        }

        private void BindColumnChooserListBox()
        {
            try
            {
                SerializableDictionary<string, bool> exportColumnList = new SerializableDictionary<string, bool>();
                exportColumnList = RiskPreferenceManager.GetExportColumns();

                if (exportColumnList.Count > 0)
                {
                    SortedList<string, bool> sortedColumnList = new SortedList<string, bool>();
                    foreach (KeyValuePair<string, bool> kvp in exportColumnList)
                    {
                        // IF condition added on 02/18/2015   (Leave its XML file Untouched)
                        if (kvp.Key.Equals("Implied Vol") || kvp.Key.Equals("Implied Volatility"))
                        {
                            sortedColumnList.Add("Volatility (%)", kvp.Value);
                        }
                        else
                        {
                            sortedColumnList.Add(kvp.Key, kvp.Value);
                        }
                    }
                    checkedMultipleItems.Items.Add("All", false);
                    foreach (KeyValuePair<string, bool> kvp in sortedColumnList)
                    {
                        checkedMultipleItems.Items.Add(kvp.Key, kvp.Value);
                    }
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

        //private List<EnumerationValue> GetAllowedGroupByColumns()
        //{
        //    List<EnumerationValue> allowedGroupByColumns = new List<EnumerationValue>();
        //    try
        //    {
        //        allowedGroupByColumns.Add(new EnumerationValue("None", "None"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Account", "Level1Name"));
        //        allowedGroupByColumns.Add(new EnumerationValue("MasterFund", "MasterFund"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Strategy", "Level2Name"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Symbol", "Symbol"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Sector Name", "SectorName"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Country Name", "CountryName"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Security Type Name", "SecurityTypeName"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Trade Attribute 1", "TradeAttribute1"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Trade Attribute 2", "TradeAttribute2"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Trade Attribute 3", "TradeAttribute3"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Trade Attribute 4", "TradeAttribute4"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Trade Attribute 5", "TradeAttribute5"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Trade Attribute 6", "TradeAttribute6"));
        //        allowedGroupByColumns.Add(new EnumerationValue("Underlying Symbol", "UnderlyingSymbol"));
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return allowedGroupByColumns;
        //}

        private void BindVARModelCombo()
        {
            List<EnumerationValue> varMethods = ConvertEnumForBindingWithAssignedValues(typeof(RiskConstants.Method));
            try
            {
                cmbVARModel.DataSource = null;
                cmbVARModel.DataSource = varMethods;
                cmbVARModel.DisplayMember = "DisplayText";
                cmbVARModel.ValueMember = "Value";
                cmbVARModel.DropDownWidth = 345;
                cmbVARModel.DisplayLayout.Bands[0].Columns["DisplayText"].Width = 345;
                cmbVARModel.DisplayLayout.Bands[0].ColHeadersVisible = false;
                cmbVARModel.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                cmbVARModel.Value = 1;
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

        //private void BindConfidenceLevelCombo()
        //{
        //    try
        //    {
        //        List<EnumerationValue> confidenceLevels = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(RiskConstants.ConfidenceLevel));
        //        cmbConfidenceLevel.DataSource = null;
        //        cmbConfidenceLevel.DataSource = confidenceLevels;
        //        cmbConfidenceLevel.DisplayMember = "Value";
        //        cmbConfidenceLevel.ValueMember = "Value";
        //        cmbConfidenceLevel.DisplayLayout.Bands[0].Columns["DisplayText"].Hidden = true;
        //        cmbConfidenceLevel.Value = 95;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void BindVolatilityTypeCombo()
        {
            try
            {
                List<EnumerationValue> volatilityTypes = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(RiskConstants.VolType));
                cmbVolatilityType.DataSource = null;
                cmbVolatilityType.DataSource = volatilityTypes;
                cmbVolatilityType.DisplayMember = "DisplayText";
                cmbVolatilityType.ValueMember = "Value";
                cmbVolatilityType.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbVolatilityType.Value = 1;
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

        private void BindUnderlyingPriceTypeCombo()
        {
            try
            {
                List<EnumerationValue> underlyingPriceTypes = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(PriceUsedType));
                cmbUnderlyingPriceType.DataSource = null;
                cmbUnderlyingPriceType.DataSource = underlyingPriceTypes;
                cmbUnderlyingPriceType.DisplayMember = "DisplayText";
                cmbUnderlyingPriceType.ValueMember = "Value";
                cmbUnderlyingPriceType.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;
                cmbUnderlyingPriceType.Value = Convert.ToInt32(PriceUsedType.Last);
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

        private void BindCalculationBasedOnCombo()
        {
            try
            {
                List<EnumerationValue> varCalculationBasedOnValues = ConvertEnumForBindingWithAssignedValues(typeof(RiskConstants.RiskCalculationBasedOn));
                cmbRiskCalculationBasedOn.DataSource = null;
                cmbRiskCalculationBasedOn.DataSource = varCalculationBasedOnValues;
                cmbRiskCalculationBasedOn.DisplayMember = "DisplayText";
                cmbRiskCalculationBasedOn.ValueMember = "Value";
                cmbRiskCalculationBasedOn.DropDownWidth = 200;
                cmbRiskCalculationBasedOn.DisplayLayout.Bands[0].Columns["DisplayText"].Width = 200;
                cmbRiskCalculationBasedOn.DisplayLayout.Bands[0].ColHeadersVisible = false;
                cmbRiskCalculationBasedOn.DisplayLayout.Bands[0].Columns["Value"].Hidden = true;

                cmbRiskCalculationBasedOn.Value = 1;
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

        private void BindCurrencyCombo()
        {
            try
            {
                List<string> lstCurrency = CommonDataCache.CachedDataManager.GetInstance.GetAllCurrencies().OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Values.ToList();
                lstCurrency = lstCurrency.ConvertAll(currency => currency.ToUpper());

                cmbRiskCalculationCurrency.DataSource = null;
                cmbRiskCalculationCurrency.DataSource = lstCurrency;
                cmbRiskCalculationCurrency.DisplayMember = "Value";
                cmbRiskCalculationCurrency.ValueMember = "Value";
                cmbRiskCalculationCurrency.DropDownWidth = 200;
                cmbRiskCalculationCurrency.DisplayLayout.Bands[0].Columns["Value"].Width = 200;
                cmbRiskCalculationCurrency.DisplayLayout.Bands[0].ColHeadersVisible = false;
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

        private void UpdateGroupingList(List<string> listGrouping)
        {
            try
            {
                listGrouping.Clear();
                listGrouping.Add(chkBoxSymbol.Text);

                if (chkBoxSide.Checked)
                {
                    listGrouping.Add(chkBoxSide.Text);
                }

                if (chkBoxAccount.Checked)
                {
                    listGrouping.Add(chkBoxAccount.Text);
                }
                if (chkBoxStrategy.Checked)
                {
                    listGrouping.Add(chkBoxStrategy.Text);
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

        private void tabRiskPrefs_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                if (_riskPrefs == null)
                {
                    _riskPrefs = RiskPreferenceManager.RiskPrefernece;
                }

                SetupRiskParams();
                SetupInterestRate();
                SetupGroupingCriteria();
                SetupAppearence();
                SetupExportFileFormat();
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

        private void SetupExportFileFormat()
        {
            if (!_isSetupExportFileFormat)
            {
                BindDateFormatCombo();
                utxtExportFileFormat.Text = _riskPrefs.RiskExportFileName;
                ucmbDateFormatSelector.Value = _riskPrefs.RiskExportDateFormat;
                if (CommonDataCache.CachedDataManager.CompanyMarketDataProvider == BusinessObjects.AppConstants.MarketDataProvider.SAPI && CommonDataCache.CachedDataManager.IsMarketDataBlocked)
                {
                    chkUseExportFileName.Checked = false;
                    chkUseExportFileName.Enabled = false;
                }
                else
                    chkUseExportFileName.Checked = _riskPrefs.UseExportFileFormat;
                _isSetupExportFileFormat = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_riskPrefs != null)
            {
                try
                {
                    _riskPrefs.InterestRateTable = dsIR;
                    _riskPrefs.VolatilityType = (RiskConstants.VolType)cmbVolatilityType.Value;
                    _riskPrefs.Method = (RiskConstants.Method)cmbVARModel.Value;
                    _riskPrefs.UnderlyingPriceType = (PriceUsedType)cmbUnderlyingPriceType.Value;
                    _riskPrefs.ConfidenceLevelPercent = Convert.ToInt32(numericUpDownConfidenceLevel.Value);
                    _riskPrefs.Lambda = Convert.ToDouble(numericUpDownLambda.Value);
                    _riskPrefs.BackColorLevel1 = cpBackLevel1.Color.ToArgb();
                    _riskPrefs.BackColorLevel2 = cpBackLevel2.Color.ToArgb();
                    _riskPrefs.BackColorLevel3 = cpBackLevel3.Color.ToArgb();
                    _riskPrefs.ForeColorLevel1 = cpForeLevel1.Color.ToArgb();
                    _riskPrefs.ForeColorLevel2 = cpForeLevel2.Color.ToArgb();
                    _riskPrefs.ForeColorLevel3 = cpForeLevel3.Color.ToArgb();
                    _riskPrefs.WrapHeader = chkboxWrapHeader.Checked;
                    Decimal.TryParse(numericUpDownFontSize.Value.ToString(), out _riskPrefs.FontSize);
                    _riskPrefs.RiskCalculationBasedOn = (RiskConstants.RiskCalculationBasedOn)cmbRiskCalculationBasedOn.Value;
                    _riskPrefs.RiskCalculationCurrency = cmbRiskCalculationCurrency.Value.ToString();
                    _riskPrefs.IsAutoLoadDataOnStartup = checkBoxAutoLoadDataOnStartup.Checked;
                    _riskPrefs.DaysBwDataPoints = Convert.ToInt32(numericUpDownDaysBwDataPoints.Value);
                    _riskPrefs.ColorSummaryText = cpSummaryColor.Color.ToArgb();
                    UpdateGroupingList(_riskPrefs.Grouping);
                    grdInterestRate.Refresh();
                    _riskPrefs.UseExportFileFormat = chkUseExportFileName.Checked;

                    _riskPrefs.RiskExportFileName = utxtExportFileFormat.Text;
                    if (ucmbDateFormatSelector.Value == null)
                        _riskPrefs.RiskExportDateFormat = string.Empty;
                    else
                        _riskPrefs.RiskExportDateFormat = ucmbDateFormatSelector.Value.ToString();

                    SerializableDictionary<string, bool> exportColumnList = new SerializableDictionary<string, bool>();
                    for (int i = 1; i < checkedMultipleItems.Items.Count; i++)
                    {
                        if (!exportColumnList.ContainsKey(checkedMultipleItems.Items[i].ToString()))
                            exportColumnList.Add(checkedMultipleItems.Items[i].ToString(), checkedMultipleItems.GetItemChecked(i));

                    }
                    int.TryParse(ultraNumericStressTest.Value.ToString(), out _riskPrefs._stressTestDateRange);
                    int.TryParse(ultraNumericRiskReport.Value.ToString(), out _riskPrefs._riskReportDateRange);
                    int.TryParse(ultraNumericRiskSimulation.Value.ToString(), out _riskPrefs._riskSimulationDateRange);
                    _riskPrefs.UpdateExportColumnList(exportColumnList);
                    RiskPreferenceManager.SavePreferences(_riskPrefs);
                    RiskPreferenceManager.RefreshInterestRate();
                    if (PrefsUpdated != null)
                    {
                        PrefsUpdated(null, null);
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
        }

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                grdInterestRate.DisplayLayout.Bands[0].AddNew();
                grdInterestRate.Rows.AddRowModifiedByUser = true;
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdInterestRate.ActiveRow != null)
                {
                    string period = grdInterestRate.ActiveRow.Cells["Period"].Value.ToString();
                    if (period != String.Empty)
                    {
                        int ID = Convert.ToInt32(period);
                        WindsorContainerManager.DeleteInterestRateFromDB(ID);
                    }
                    grdInterestRate.ActiveRow.Delete(true);
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

        private void btnDefault_Click(object sender, EventArgs e)
        {
            try
            {
                RiskPreferenceManager.RestoreDefaults();
                _riskPrefs = RiskPreferenceManager.RiskPrefernece;
                _isSetupRiskParams = false;
                _isSetupInterestRate = false;
                _isSetupGrouping = false;
                _isSetupAppearence = false;
                _isSetupExportFileFormat = false;
                SetupRiskParams();
                SetupInterestRate();
                SetupGroupingCriteria();
                SetupAppearence();
                SetupExportFileFormat();
                chkBoxAccount.Checked = true;
                chkBoxStrategy.Checked = true;
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

        private List<EnumerationValue> ConvertEnumForBindingWithAssignedValues(Type enumType)
        {
            List<EnumerationValue> results = new List<EnumerationValue>();

            // Use reflection to see what values the enum provides
            string[] members = Enum.GetNames(enumType);

            foreach (string member in members)
            {
                string name = member;
                int i = Convert.ToInt32(Enum.Parse(enumType, name));
                results.Add(new EnumerationValue(Wordify(name), i));
            }

            return results;
        }

        public string Wordify(string pascalCaseString)
        {
            Regex r = new Regex("(?<=[a-z])(?<x>[A-Z])|(?<=.)(?<x>[A-Z])(?=[a-z])");
            return r.Replace(pascalCaseString, " ${x}");
        }

        private void grdInterestRate_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key.Equals("Period"))
                {
                    UltraGridRow Activerow = e.Cell.Row;
                    foreach (UltraGridRow row in grdInterestRate.Rows)
                    {
                        if (!row.Equals(Activerow))
                        {
                            if (row.Cells["Period"].Value.ToString().Equals(Activerow.Cells["Period"].Value.ToString()))
                            {
                                MessageBox.Show("The month period you have entered already exists,please enter a different value", "Risk Preferences", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
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

        private void checkedMultipleItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                //select deselect all the accounts on the basis of select all checkbox
                if (e.Index == 0)
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);

                    for (int i = 1; i < checkedMultipleItems.Items.Count; i++)
                    {
                        checkedMultipleItems.SetItemCheckState(i, e.NewValue);
                    }

                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                }
                else if (e.NewValue == CheckState.Checked)
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);

                    if (checkedMultipleItems.CheckedItems.Count == checkedMultipleItems.Items.Count - 2)
                    {
                        checkedMultipleItems.SetItemCheckState(0, e.NewValue);
                    }

                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    this.checkedMultipleItems.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);

                    if (checkedMultipleItems.CheckedItems.Count == checkedMultipleItems.Items.Count)
                    {
                        checkedMultipleItems.SetItemCheckState(0, e.NewValue);
                    }

                    this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
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

        private void RiskPrefsUI_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_RISK_MANAGEMENT);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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
        /// Used for changing the color of buttons. The indices and their colors are as follows:
        /// 0 & 3: For the Green Shade
        /// 1 & 4: For the Neutral Shade
        /// 2 & 5: For the Red Shade 
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDefault.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDefault.ForeColor = System.Drawing.Color.White;
                btnDefault.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDefault.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDefault.UseAppStyling = false;
                btnDefault.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        private void chkUseExportFileName_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkUseExportFileName.Checked)
                {
                    ugbxExportFileNameSettings.Enabled = true;
                }
                else
                {
                    ugbxExportFileNameSettings.Enabled = false;
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
    }
}