using Microsoft.Reporting.WinForms;
using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace Prana.PortfolioReports.Controls
{
    public partial class CtrlValuationSummaryReport : UserControl
    {
        const string GROUPBY_FUND = "AccountName";
        const string GROUPBY_TICKER = "Symbol";
        const string GROUPBY_TYPE = "Position Type";
        const string GROUPBY_MASTERFUND = "MasterFundName";
        const int MODECOSTBASIS = 0;
        const int MODEDAYBASIS = 1;
        const int REPORTPROCESSINGSTATUS = 0;
        const int REPORTPROCESSDSTATUS = 1;

        public CtrlValuationSummaryReport()
        {
            InitializeComponent();
            this.reportViewerValuationSummary.LocalReport.ReportPath = Application.StartupPath + "\\Reports\\ValuationSummary.rdlc";
            //reportModeValueStatic = cmbReportMode.Value.ToString();
        }

        //ILiveFeedPublisher _liveFeedCacheInstance = null;
        private CompanyUser _loginUser;
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
            }
        }

        bool _isMasterFundAssociationSaved = false;
        public void SetupControl(CompanyUser loginUser)
        {
            _loginUser = loginUser;
            if (_loginUser == null)
            {
                //throw new Exception("User could not be found while initializing valuation summary Reports");
            }
            dtSelectDate.Value = DateTime.Now;
            //dtMonth.Value = DateTime.Now.Date.AddMonths(-1);
            //dtSelectDate.MaxDate = DateTime.Now; 21st Jan
            //dtMonth.MaskInput = "{LOC}mm/yyyy";
            //BindAccountList();

            //_isMasterFundAssociationSaved = CachedDataManager.GetInstance.IsMasterFundAssociationSaved;
            _isMasterFundAssociationSaved = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("MasterFundAssociation").ToString());

            BindComboGroupFilters();
            BindReportModeCombo();
            BindReportTypeCombo();
            _reportMode = 0;
            //_reportViewType = 0;
            //GenerateReports();
        }

        EnumerationValueList lstReportMode = new EnumerationValueList();
        private void BindReportModeCombo()
        {
            cmbReportMode.DisplayMember = "DisplayText";
            cmbReportMode.ValueMember = "Value";
            lstReportMode = EnumHelper.ConvertEnumForBindingWitouthSelect(typeof(ReportMode));
            cmbReportMode.DataSource = null;
            cmbReportMode.DataSource = lstReportMode;
            Utils.UltraComboFilter(cmbReportMode, "DisplayText");
            cmbReportMode.Text = "CostBasis";
        }

        EnumerationValueList lstReportType = new EnumerationValueList();
        private void BindReportTypeCombo()
        {
            cmbReportViewType.DisplayMember = "DisplayText";
            cmbReportViewType.ValueMember = "Value";
            lstReportType = EnumHelper.ConvertEnumForBindingWitouthSelect(typeof(ReportViewType));
            cmbReportViewType.DataSource = null;
            cmbReportViewType.DataSource = lstReportType;
            Utils.UltraComboFilter(cmbReportViewType, "DisplayText");
            cmbReportViewType.Text = "Current";
        }

        EnumerationValueList lstGroupFilters = new EnumerationValueList();
        EnumerationValueList lstGroupFilters2nd = new EnumerationValueList();
        EnumerationValueList lstGroupFilters3rd = new EnumerationValueList();
        private void BindComboGroupFilters()
        {
            lstGroupFilters = EnumHelper.ConvertEnumForBindingWithSelect(typeof(GroupFilters));
            if (_isMasterFundAssociationSaved.Equals(false))
            {
                lstGroupFilters.RemoveAt(4);
                label3.Visible = false;
                cmb4thGroup.Visible = false;
            }

            cmb1stGroup.DisplayMember = "DisplayText";
            cmb1stGroup.ValueMember = "Value";
            cmb1stGroup.DataSource = null;
            cmb1stGroup.DataSource = lstGroupFilters;
            Utils.UltraComboFilter(cmb1stGroup, "DisplayText");
            //cmb1stGroup.Value = int.MinValue;  On 15th Jan
            if (_isMasterFundAssociationSaved.Equals(false))
            {
                cmb1stGroup.Text = "Account";
                //cmb3rdGroup.Enabled = false;
            }
            else
            {
                cmb1stGroup.Text = "MasterFund";
            }

            cmb2ndGroup.DisplayMember = "DisplayText";
            cmb2ndGroup.ValueMember = "Value";
            //cmb2ndGroup.DataSource = null;
            //cmb2ndGroup.DataSource = lstGroupFilters;
            Utils.UltraComboFilter(cmb2ndGroup, "DisplayText");
            //cmb2ndGroup.Value = int.MinValue;
            //cmb2ndGroup.Enabled = false;
            //cmb2ndGroup.Text = "PositionType";
            cmb2ndGroup.Value = int.MinValue;

            // ------------------------ On 15th Jan ----------------
            cmb3rdGroup.DisplayMember = "DisplayText";
            cmb3rdGroup.ValueMember = "Value";
            //cmb2ndGroup.DataSource = null;
            //cmb2ndGroup.DataSource = lstGroupFilters;
            Utils.UltraComboFilter(cmb3rdGroup, "DisplayText");
            //cmb2ndGroup.Value = int.MinValue;
            //cmb2ndGroup.Enabled = false;
            //cmb2ndGroup.Text = "PositionType";
            cmb3rdGroup.Enabled = false;

            cmb4thGroup.DisplayMember = "DisplayText";
            cmb4thGroup.ValueMember = "Value";
            cmb4thGroup.DataSource = null;
            cmb4thGroup.DataSource = lstGroupFilters;
            Utils.UltraComboFilter(cmb4thGroup, "DisplayText");
            cmb4thGroup.Value = int.MinValue;
            cmb4thGroup.Enabled = false;
        }


        DataTable _dtValuationStatement = new DataTable();
        List<string> _requestedSymbolList = new List<string>();
        DataSetValuationStatement _dataSetValuationStatementUpdated = new DataSetValuationStatement();
        bool _passThrough = false;
        private string _companyBaseCurrencySymbol = string.Empty;
        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor currentCursor = this.Cursor;
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.reportViewerValuationSummary.Clear();
            GenerateReports();
            this.Cursor = currentCursor;
        }

        private int _reportMode = 0;
        //private int _reportViewType = 0;
        DateTime _date = DateTime.UtcNow;
        public void GenerateReports()
        {
            _passThrough = false;
            dataSetValuationStatement.EnforceConstraints = false;


            _requestedSymbolList.Clear(); //Added on 6th Nov, 2007
            //StringBuilder sbAccountString = new StringBuilder();
            //int accountID = 0;
            //for (int i = 0; i < checkedListAccounts.CheckedItems.Count; i++)
            //{
            //    accountID = ((Prana.BusinessObjects.PositionManagement.Account)checkedListAccounts.CheckedItems[i]).ID;
            //    sbAccountString.Append("'");
            //    sbAccountString.Append(accountID);
            //    sbAccountString.Append("',");
            //}
            //int len = sbAccountString.Length;
            //if (sbAccountString.Length > 0)
            //{
            //    sbAccountString.Remove((len - 1), 1);
            //}


            //pMGetMonthlySummaryValuesTableAdapter.Connection.ConnectionString = "Data Source=VS20052K3E;Initial Catalog=QADBPRODj;Persist Security Info=True;User ID=sa;Password=Prana2@@6";// .Connection.Database = "QADBPRODj";
            //string connectionString = System.ConfigurationManager.ConnectionStrings["PranaConnectionString"].ConnectionString.ToString();

            string reportModeValue = cmbReportMode.Value.ToString();
            if (reportModeValue == ReportMode.CostBasis.ToString())
            {
                _reportMode = 0;
            }
            else
            {
                _reportMode = 1;
            }

            _date = (DateTime)dtSelectDate.Value;
            string errMessage = " ";
            int? errNumber = 0;
            string allAUECDatesString = string.Empty;
            allAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(_date);
            //if (_reportViewType.Equals(0))
            //{
            //    date = DateTime.UtcNow;
            //    allAUECDatesString = TimeZoneHelper.GetAllAUECLocalDatesFromUTCStr(date);
            //}
            //else
            //{
            //    allAUECDatesString = TimeZoneHelper.GetSameDateForAllAUEC(date);
            //}


            ReportParameter[] param = new ReportParameter[4];
            param[0] = new ReportParameter("parNoData", "False");
            param[1] = new ReportParameter("parReportDate", _date.ToString("MM/dd/yyyy"));
            param[2] = new ReportParameter("paramCostBasis", _reportMode.ToString());
            param[3] = new ReportParameter("paramReportProcessingStatusBit", REPORTPROCESSINGSTATUS.ToString());
            reportViewerValuationSummary.LocalReport.SetParameters(param);

            if (this.dataSetValuationStatement.PMGetValuationSummaryReportData == null)
            {
                //this.dataSetValuationStatement.PMGetValuationSummaryReportData = new DataSetValuationStatement.PMGetValuationSummaryReportDataDataTable();
            }
            else
            {
                this.pMGetValuationSummaryReportDataTableAdapter.Fill(this.dataSetValuationStatement.PMGetValuationSummaryReportData, _loginUser.CompanyID, _loginUser.CompanyUserID, _date, allAUECDatesString, ref errMessage, ref errNumber, _reportMode);
                //this.pMGetValuationSummaryReportDataTableAdapter.Fill(this.dataSetValuationStatement.PMGetValuationSummaryReportData, _loginUser.CompanyID, _loginUser.CompanyUserID, date, sbAccountString.ToString(), ref errMessage, ref errNumber);
            }

            if (this.dataSetCompanyLogo.PMGetCompanyLogo != null)
            {
                this.pMGetCompanyLogoTableAdapter.Fill(this.dataSetCompanyLogo.PMGetCompanyLogo, _loginUser.CompanyID, _loginUser.CompanyUserID, ref errMessage, ref errNumber);
            }


            _dtValuationStatement = this.pMGetValuationSummaryReportDataTableAdapter.GetData(_loginUser.CompanyID, _loginUser.CompanyUserID, _date, allAUECDatesString, ref errMessage, ref errNumber, _reportMode);
            //_dtValuationStatement = this.pMGetValuationSummaryReportDataTableAdapter.GetData(_loginUser.CompanyID, _loginUser.CompanyUserID, date, sbAccountString.ToString(), ref errMessage, ref errNumber);



            int companyBaseCurrencyID = int.MinValue;
            if (_dtValuationStatement.Rows.Count > 0)
            {
                companyBaseCurrencyID = int.Parse(_dtValuationStatement.Rows[0]["BaseCurrencyID"].ToString());
                Prana.Admin.BLL.Currency currency = Prana.Admin.BLL.AUECManager.GetCurrency(companyBaseCurrencyID);
                _companyBaseCurrencySymbol = currency.CurrencySymbol;

                //--BB--LiveFeed
                //_liveFeedCacheInstance = LiveFeedInstanceCreator.Instance;
                //if (_liveFeedCacheInstance != null)
                //{
                //    _liveFeedCacheInstance.PublishLevel1SnapshotResponse += new EventHandler(_liveFeedCacheInstance_PublishLevel1SnapshotResponse);
                //}


            }
            else
            {
                //this.reportViewerValuationSummary.RefreshReport();

                //LocalReport localReport = reportViewerValuationSummary.LocalReport;
                ////DataSetValuationStatement dataSetValuationStatementNew = new DataSetValuationStatement();
                //ReportDataSource repDataSource = new ReportDataSource();
                //repDataSource.Name = "DataSource";
                //repDataSource.Value = dataSetValuationStatement.PMGetValuationSummaryReportData;
                //DataTable dt1 = dataSetValuationStatement.Tables[0];
                //localReport.DataSources.Add(repDataSource);

                //this.reportViewerValuationSummary.RefreshReport();

                param[0] = new ReportParameter("parNoData", "True");
                param[1] = new ReportParameter("parReportDate", _date.ToString("MM/dd/yyyy"));
                param[2] = new ReportParameter("paramCostBasis", _reportMode.ToString());
                param[3] = new ReportParameter("paramReportProcessingStatusBit", REPORTPROCESSDSTATUS.ToString());
                reportViewerValuationSummary.LocalReport.SetParameters(param);

                //this.reportViewerValuationSummary.RefreshReport();
                return;
            }

            //--BB--LiveFeed
            //if (!_liveFeedCacheInstance.IsDataManagerConnected())
            //{
            //    this.reportViewerValuationSummary.RefreshReport();

            //    LocalReport localReport = reportViewerValuationSummary.LocalReport;
            //    //DataSetValuationStatement dataSetValuationStatementNew = new DataSetValuationStatement();
            //    ReportDataSource repDataSource = new ReportDataSource();
            //    repDataSource.Name = "DataSource";
            //    repDataSource.Value = dataSetValuationStatement.PMGetValuationSummaryReportData;
            //    DataTable dt1 = dataSetValuationStatement.Tables[0];
            //    localReport.DataSources.Add(repDataSource);

            //    this.reportViewerValuationSummary.RefreshReport();
            //}
            //else
            //{
            //    foreach (DataRow row in _dtValuationStatement.Rows)
            //    {
            //        DataRow rowNew = row;
            //        //DataColumn colMarketOrCurrentPrice = new DataColumn();
            //        //colMarketOrCurrentPrice = (DataColumn)_dtValuationStatement.Columns["MarketOrCurrentPrice"];
            //        //colMarketOrCurrentPrice.ReadOnly = false;
            //        //DataColumn colMARKETVALUE = new DataColumn();
            //        //colMARKETVALUE = (DataColumn)_dtValuationStatement.Columns["MARKETVALUE"];
            //        //colMARKETVALUE.ReadOnly = false;

            //        if (_liveFeedCacheInstance.IsDataManagerConnected())
            //        {
            //            symbol = row["Symbol"].ToString();

            //            if (!_requestedSymbolList.Contains(symbol))
            //            {
            //                _requestedSymbolList.Add(symbol);
            //                _liveFeedCacheInstance.RequestLevel1Snapshot(symbol);
            //            }


            //            //currentOrMarketPrice = _l1data.Last;
            //            //if (currentOrMarketPrice <= 0)
            //            //{
            //            //    currentOrMarketPrice = 1;
            //            //}
            //            //row["MarketOrCurrentPrice"] = currentOrMarketPrice;
            //            //row["MARKETVALUE"] = currentOrMarketPrice * Convert.ToInt32(row["Quantity"]);
            //        }
            //    }
            //}

            UpdateReportWithCurrentMarketPrice();






            //this.dataSetValuationStatement.Tables.Remove("PMGetValuationSummaryReportData");
            //this.dataSetValuationStatement.Tables.Add(_dtValuationStatement);

            //this.reportViewerValuationSummary.RefreshReport();

            //LocalReport localReport = reportViewerValuationSummary.LocalReport;
            ////DataSetValuationStatement dataSetValuationStatementNew = new DataSetValuationStatement();
            //ReportDataSource repDataSource = new ReportDataSource();
            //repDataSource.Name = "DataSource";
            //repDataSource.Value = dataSetValuationStatementNew.PMGetValuationSummaryReportData;
            //DataTable dt1 = dataSetValuationStatementNew.Tables[0];
            //localReport.DataSources.Add(repDataSource);

            //this.reportViewerValuationSummary.RefreshReport();
        }

        private void UpdateReportWithCurrentMarketPrice()
        {
            string baseCompanyCurrencyLanguageName = string.Empty;
            //string symbol = string.Empty;
            string currencyLanguageName = string.Empty;
            bool isTradeDateTransaction = false;
            double conversionRateOnTradeDate = 1;
            double conversionRateOnYesterdayDate = 1;
            double conversionRateOnMarketDate = 1;
            DateTime tradeDate = DateTime.Now.Date;
            DateTime dateBeforeTradeDate = DateTime.Now.Date;
            DateTime dateOnMarketDate = DateTime.Now.Date;
            int auecID = 0;
            AssetCategory assetCategory = AssetCategory.None;
            int currencyID = 0;
            int tradedCurrencyID = 0;
            //int vsCurrencyID = 0;
            int baseCurrencyID = 0;
            ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData(_date);

            foreach (DataRow row in _dtValuationStatement.Rows)
            {
                isTradeDateTransaction = false;
                DataColumn colMARKETVALUE = new DataColumn();
                colMARKETVALUE = (DataColumn)_dtValuationStatement.Columns["MARKETVALUE"];
                colMARKETVALUE.ReadOnly = false;

                DataColumn colGain = new DataColumn();
                colGain = (DataColumn)_dtValuationStatement.Columns["Gain"];
                colGain.ReadOnly = false;

                DataColumn colLanguageName = new DataColumn();
                colLanguageName = (DataColumn)_dtValuationStatement.Columns["LanguageName"];

                DataColumn colBaseCurrencyLanguageName = new DataColumn();
                colBaseCurrencyLanguageName = (DataColumn)_dtValuationStatement.Columns["BaseCurrencyLanguageName"];
                colBaseCurrencyLanguageName.ReadOnly = false;
                colLanguageName.ReadOnly = false;

                DataColumn colTotalCost = new DataColumn();
                colTotalCost = (DataColumn)_dtValuationStatement.Columns["TotalCost"];
                colTotalCost.ReadOnly = false;

                DataColumn colTotalCostInBaseCurrency = new DataColumn();
                colTotalCostInBaseCurrency = (DataColumn)_dtValuationStatement.Columns["TotalCostInBaseCurrency"];
                colTotalCostInBaseCurrency.ReadOnly = false;

                DataColumn colTotalCostPerShare = new DataColumn();
                colTotalCostPerShare = (DataColumn)_dtValuationStatement.Columns["TotalCostPerShare"];
                colTotalCostPerShare.ReadOnly = false;

                DataColumn colMarketValueInBaseCurrency = new DataColumn();
                colMarketValueInBaseCurrency = (DataColumn)_dtValuationStatement.Columns["MarketValueInBaseCurrency"];
                colMarketValueInBaseCurrency.ReadOnly = false;

                DataColumn colGainInBaseCurrency = new DataColumn();
                colGainInBaseCurrency = (DataColumn)_dtValuationStatement.Columns["GainInBaseCurrency"];
                colGainInBaseCurrency.ReadOnly = false;

                DataColumn colGroupParamaeter1 = new DataColumn();
                colGroupParamaeter1 = (DataColumn)_dtValuationStatement.Columns["GroupParamaeter1"];
                colGroupParamaeter1.ReadOnly = false;

                DataColumn colGroupParamaeter2 = new DataColumn();
                colGroupParamaeter2 = (DataColumn)_dtValuationStatement.Columns["GroupParamaeter2"];
                colGroupParamaeter2.ReadOnly = false;

                DataColumn colGroupParamaeter3 = new DataColumn();
                colGroupParamaeter3 = (DataColumn)_dtValuationStatement.Columns["GroupParamaeter3"];
                colGroupParamaeter3.ReadOnly = false;

                DataColumn colLogoPath = new DataColumn();
                colLogoPath = (DataColumn)_dtValuationStatement.Columns["LogoPath"];
                colLogoPath.ReadOnly = false;

                DataColumn colAggregateOption = new DataColumn();
                colAggregateOption = (DataColumn)_dtValuationStatement.Columns["AggregateOption"];
                colAggregateOption.ReadOnly = false;

                //symbol = row["Symbol"].ToString();
                auecID = Convert.ToInt32(row["AUECID"]);
                assetCategory = (AssetCategory)Convert.ToInt32(row["AssetID"]);
                currencyID = Convert.ToInt32(row["CurrencyID"]);
                tradedCurrencyID = Convert.ToInt32(row["TradedCurrencyID"]);
                //vsCurrencyID = Convert.ToInt32(row["VsCurrencyID"]);
                baseCurrencyID = Convert.ToInt32(row["BaseCurrencyID"]);

                //double futureMultiplier = 1;

                currencyLanguageName = GetCurrencyLanguageName(row["CurrencySymbol"].ToString());
                row["LanguageName"] = currencyLanguageName;

                baseCompanyCurrencyLanguageName = GetCurrencyLanguageName(_companyBaseCurrencySymbol);
                row["BaseCurrencyLanguageName"] = baseCompanyCurrencyLanguageName;

                tradeDate = DateTime.Parse(row["TradeDate"].ToString());
                dateOnMarketDate = _date;
                if (_reportMode.Equals(MODEDAYBASIS))
                {
                    tradeDate = _date;
                    //dateBeforeTradeDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(_date, -1, auecID);
                }

                if (_reportMode.Equals(MODEDAYBASIS) && Convert.ToDateTime(row["TradeDate"]).Date.Equals(_date.Date))
                {
                    dateBeforeTradeDate = tradeDate;
                }
                else if (_reportMode.Equals(MODEDAYBASIS) && Convert.ToDateTime(row["TradeDate"]).Date != _date.Date)
                {
                    //dateBeforeTradeDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(tradeDate, -1, auecID);
                    if (assetCategory.Equals(AssetCategory.FX))
                    {
                        dateBeforeTradeDate = _date.AddDays(-1);
                    }
                    else
                    {
                        dateBeforeTradeDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(_date, -1, auecID);
                    }
                }
                ConversionRate conversionRateOnTradeDt = null;
                ConversionRate conversionRateOnYestDt = null;
                ConversionRate conversionRateOnMarketDt = null;

                if (assetCategory.Equals(AssetCategory.FX))
                {
                    //conversionRateOnTradeDt = ForexConverter.GetInstance().GetFXRateForFXSymbolAndDate(symbol, tradeDate);
                    //conversionRateOnTradeDt = ForexConverter.GetInstance(_loginUser.CompanyID).GetConversionRateFromCurrencies(tradedCurrencyID, baseCurrencyID, tradeDate);
                    if (tradedCurrencyID.Equals(baseCurrencyID))
                    {
                        conversionRateOnTradeDate = 1;
                        conversionRateOnYesterdayDate = 1;
                        conversionRateOnMarketDate = 1;
                    }
                    else
                    {
                        //ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData(tradeDate);
                        //conversionRateOnTradeDt = ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetConversionRateFromCurrenciesForGivenDate(tradedCurrencyID, baseCurrencyID, tradeDate);
                        //ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData(tradeDate); BB
                        conversionRateOnTradeDt = ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetConversionRateFromCurrenciesForGivenDate(tradedCurrencyID, baseCurrencyID, tradeDate, 0);
                        if (conversionRateOnTradeDt.ConversionMethod.ToString().Equals("Divide"))
                        {
                            if (conversionRateOnTradeDt.RateValue.Equals(0.0))
                            {
                                conversionRateOnTradeDate = 0;
                            }
                            else
                            {
                                conversionRateOnTradeDate = 1 / conversionRateOnTradeDt.RateValue;
                            }
                        }
                        else
                        {
                            conversionRateOnTradeDate = conversionRateOnTradeDt.RateValue;
                        }

                        if (_reportMode.Equals(MODECOSTBASIS))
                        {
                            //ForexConverter.GetInstance(_loginUser.CompanyID, dateOnMarketDate).GetForexData(dateOnMarketDate);BB
                            conversionRateOnMarketDt = ForexConverter.GetInstance(_loginUser.CompanyID, dateOnMarketDate).GetConversionRateFromCurrenciesForGivenDate(tradedCurrencyID, baseCurrencyID, dateOnMarketDate, 0);
                            if (conversionRateOnMarketDt.ConversionMethod.ToString().Equals("Divide"))
                            {
                                if (conversionRateOnMarketDt.RateValue.Equals(0.0))
                                {
                                    conversionRateOnMarketDate = 0;
                                }
                                else
                                {
                                    conversionRateOnMarketDate = 1 / conversionRateOnMarketDt.RateValue;
                                }
                            }
                            else
                            {
                                conversionRateOnMarketDate = conversionRateOnMarketDt.RateValue;
                            }
                        }

                        if (_reportMode.Equals(MODEDAYBASIS))
                        {
                            //ForexConverter.GetInstance(_loginUser.CompanyID, dateBeforeTradeDate).GetForexData(dateBeforeTradeDate);BB
                            conversionRateOnYestDt = ForexConverter.GetInstance(_loginUser.CompanyID, dateBeforeTradeDate).GetConversionRateFromCurrenciesForGivenDate(tradedCurrencyID, baseCurrencyID, dateBeforeTradeDate, 0);
                            if (conversionRateOnYestDt.ConversionMethod.ToString().Equals("Divide"))
                            {
                                if (conversionRateOnYestDt.RateValue.Equals(0.0))
                                {
                                    conversionRateOnYesterdayDate = 0;
                                }
                                else
                                {
                                    conversionRateOnYesterdayDate = 1 / conversionRateOnYestDt.RateValue;
                                }
                            }
                            else
                            {
                                conversionRateOnYesterdayDate = conversionRateOnYestDt.RateValue;
                            }
                        }
                    }
                }
                else
                {
                    //conversionRateOnTradeDt = ForexConverter.GetInstance(_loginUser.CompanyID).GetConversionRateForCurrencyToBaseCurrency(currencyID, tradeDate);
                    //Prana.CommonDataCache.ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData();
                    //ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData(tradeDate);
                    //conversionRateOnTradeDt = ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetConversionRateForCurrencyToBaseCurrencyForGivenDate(currencyID, tradeDate);
                    //ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData(tradeDate);BB
                    conversionRateOnTradeDt = ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetConversionRateForCurrencyToBaseCurrencyForGivenDate(currencyID, tradeDate, 0);
                    if (conversionRateOnTradeDt.ConversionMethod.ToString().Equals("Divide"))
                    {
                        if (conversionRateOnTradeDt.RateValue.Equals(0.0))
                        {
                            conversionRateOnTradeDate = 0;
                        }
                        else
                        {
                            conversionRateOnTradeDate = 1 / conversionRateOnTradeDt.RateValue;
                        }
                    }
                    else
                    {
                        conversionRateOnTradeDate = conversionRateOnTradeDt.RateValue;
                    }
                    //conversionRateOnTradeDate = conversionRateOnTradeDt.RateValue;

                    if (_reportMode.Equals(MODECOSTBASIS))
                    {
                        //ForexConverter.GetInstance(_loginUser.CompanyID, dateOnMarketDate).GetForexData(dateOnMarketDate);BB
                        conversionRateOnMarketDt = ForexConverter.GetInstance(_loginUser.CompanyID, dateOnMarketDate).GetConversionRateForCurrencyToBaseCurrencyForGivenDate(currencyID, dateOnMarketDate, 0);
                        if (conversionRateOnMarketDt.ConversionMethod.ToString().Equals("Divide"))
                        {
                            if (conversionRateOnMarketDt.RateValue.Equals(0.0))
                            {
                                conversionRateOnMarketDate = 0;
                            }
                            else
                            {
                                conversionRateOnMarketDate = 1 / conversionRateOnMarketDt.RateValue;
                            }
                        }
                        else
                        {
                            conversionRateOnMarketDate = conversionRateOnMarketDt.RateValue;
                        }
                    }

                    if (_reportMode.Equals(MODEDAYBASIS))
                    {
                        //ForexConverter.GetInstance(_loginUser.CompanyID, dateBeforeTradeDate).GetForexData(dateBeforeTradeDate);BB
                        conversionRateOnYestDt = ForexConverter.GetInstance(_loginUser.CompanyID, dateBeforeTradeDate).GetConversionRateForCurrencyToBaseCurrencyForGivenDate(currencyID, dateBeforeTradeDate, 0);
                        if (conversionRateOnYestDt.ConversionMethod.ToString().Equals("Divide"))
                        {
                            if (conversionRateOnYestDt.RateValue.Equals(0.0))
                            {
                                conversionRateOnYesterdayDate = 0;
                            }
                            else
                            {
                                conversionRateOnYesterdayDate = 1 / conversionRateOnYestDt.RateValue;
                            }
                        }
                        else
                        {
                            conversionRateOnYesterdayDate = conversionRateOnYestDt.RateValue;
                        }
                    }
                }

                //if (true)
                //{

                //}
                //if (CachedDataManager.GetInstance.GetFXConversionRateBySymbolAndDate(symbol, tradeDate) != null)
                //{
                //    conversionRateOnTradeDate = double.Parse(CachedDataManager.GetInstance.GetFXConversionRateBySymbolAndDate(symbol, tradeDate).RateValue.ToString());
                //}

                double convFactor = 1;
                bool isNullconversionFactor = double.TryParse(row["ConversionFactor"].ToString(), out convFactor);
                if (isNullconversionFactor.Equals(false))
                {
                    convFactor = 1;
                }

                if (_passThrough.Equals(false))
                {
                    if (double.Parse(row["TotalCost"].ToString()) < 0)
                    {
                        row["MARKETVALUE"] = double.Parse(row["MARKETVALUE"].ToString()) * -1; //For short.
                        row["YesterdayMarketValue"] = double.Parse(row["YesterdayMarketValue"].ToString()) * -1; //For short.
                    }


                    //if (symbol.IndexOf(" ") > 0)
                    //{
                    //    futureMultiplier = float.Parse(CachedDataManager.GetInstance.GetContractMultiplierBySymbol(symbol.Substring(0, symbol.IndexOf(" "))).ToString());
                    //    if (futureMultiplier > 1)
                    //    {
                    //        row["TotalCost"] = (double.Parse(row["TotalCost"].ToString()) * futureMultiplier) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                    //        //row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString()) * futureMultiplier) + ((double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()))); Jan 25th
                    //        row["TotalCostInBaseCurrency"] = double.Parse(row["TotalCost"].ToString()) * conversionRateOnTradeDate;

                    //        row["MARKETVALUE"] = double.Parse(row["MARKETVALUE"].ToString()) * futureMultiplier;

                    //        if (_reportMode.Equals(MODECOSTBASIS))
                    //        {
                    //            row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnMarketDate;
                    //        }
                    //        else
                    //        {
                    //            row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnTradeDate;
                    //        }

                    //        //row["GainInBaseCurrency"] = double.Parse(row["Gain"].ToString()) * double.Parse(row["ConversionFactor"].ToString());

                    //        row["TotalCostPerShare"] = double.Parse(row["TotalCostPerShare"].ToString()) * (double.Parse(row["TotalCost"].ToString()) / double.Parse(row["Quantity"].ToString()));

                    //        if (DateTime.Parse(row["TradeDate"].ToString()).Date.Equals(_date.Date) && _reportMode.Equals(1) )
                    //        {
                    //            if (row["CurrencySymbol"].ToString().Equals("MUL"))
                    //            {
                    //                row["YesterdayMarketValue"] = double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                    //            }
                    //            else
                    //            {
                    //                row["YesterdayMarketValue"] = double.Parse(row["TotalCostPerShare"].ToString()) * double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                    //            }
                    //            isTradeDateTransaction = true;
                    //        }
                    //        row["YesterdayMarketValue"] = double.Parse(row["YesterdayMarketValue"].ToString()) * futureMultiplier;
                    //        row["YesterdayMarketValueInBaseCurrency"] = double.Parse(row["YesterdayMarketValue"].ToString()) * conversionRateOnYesterdayDate;
                    //    }
                    //    else
                    //    {
                    //        row["TotalCost"] = double.Parse(row["TotalCost"].ToString()) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                    //        row["TotalCostPerShare"] = double.Parse(row["TotalCostPerShare"].ToString()) * (double.Parse(row["TotalCost"].ToString()) / double.Parse(row["Quantity"].ToString()));
                    //        //row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString())) + ((double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()))); 25th Jan
                    //        row["TotalCostInBaseCurrency"] = double.Parse(row["TotalCost"].ToString()) * conversionRateOnTradeDate;

                    //        if (_reportMode.Equals(MODECOSTBASIS))
                    //        {
                    //            row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnMarketDate;
                    //        }
                    //        else
                    //        {
                    //            row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnTradeDate;
                    //        }
                    //        //row["GainInBaseCurrency"] = double.Parse(row["Gain"].ToString()) * double.Parse(row["ConversionFactor"].ToString());

                    //        if (DateTime.Parse(row["TradeDate"].ToString()).Date.Equals(_date.Date) && _reportMode.Equals(1))
                    //        {
                    //            if (row["CurrencySymbol"].ToString().Equals("MUL"))
                    //            {
                    //                row["YesterdayMarketValue"] = double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                    //            }
                    //            else
                    //            {
                    //                row["YesterdayMarketValue"] = double.Parse(row["TotalCostPerShare"].ToString()) * double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                    //            }
                    //            isTradeDateTransaction = true;
                    //        }
                    //        row["YesterdayMarketValueInBaseCurrency"] = double.Parse(row["YesterdayMarketValue"].ToString()) * conversionRateOnYesterdayDate;
                    //    }
                    //}
                    //else
                    //{
                    //    row["TotalCost"] = double.Parse(row["TotalCost"].ToString()) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                    //    row["TotalCostPerShare"] = double.Parse(row["TotalCostPerShare"].ToString()) * (double.Parse(row["TotalCost"].ToString()) / double.Parse(row["Quantity"].ToString()));
                    //    //row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString())) + ((double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()))); 25th Jan
                    //    row["TotalCostInBaseCurrency"] = double.Parse(row["TotalCost"].ToString()) * conversionRateOnTradeDate;//* conversionRateOnMarketDate;//

                    //    if (_reportMode.Equals(MODECOSTBASIS))
                    //    {
                    //        row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnMarketDate;
                    //    }
                    //    else
                    //    {
                    //        row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnTradeDate;
                    //    }
                    //    //row["GainInBaseCurrency"] = double.Parse(row["Gain"].ToString()) * double.Parse(row["ConversionFactor"].ToString());

                    //    if (DateTime.Parse(row["TradeDate"].ToString()).Date.Equals(_date.Date) && _reportMode.Equals(1))
                    //    {
                    //        if (row["CurrencySymbol"].ToString().Equals("MUL"))
                    //        {
                    //            row["YesterdayMarketValue"] = double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                    //        }
                    //        else
                    //        {
                    //            row["YesterdayMarketValue"] = double.Parse(row["TotalCostPerShare"].ToString()) * double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                    //        }
                    //        isTradeDateTransaction = true;
                    //    }
                    //    row["YesterdayMarketValueInBaseCurrency"] = double.Parse(row["YesterdayMarketValue"].ToString()) * conversionRateOnYesterdayDate;
                    //}
                }

                if (_reportMode.Equals(MODECOSTBASIS))
                {
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0)
                    {
                        row["Gain"] = double.Parse(row["MARKETVALUE"].ToString()) - double.Parse(row["TotalCost"].ToString());
                    }
                    else
                    {
                        row["Gain"] = 0;
                    }
                }
                else
                {
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && double.Parse(row["YesterdayMarkPrice"].ToString()) > 0 && isTradeDateTransaction.Equals(false))
                    {
                        row["Gain"] = double.Parse(row["MARKETVALUE"].ToString()) - double.Parse(row["YesterdayMarketValue"].ToString());
                    }
                    else if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && isTradeDateTransaction.Equals(true))
                    {
                        row["Gain"] = double.Parse(row["MARKETVALUE"].ToString()) - double.Parse(row["YesterdayMarketValue"].ToString());
                    }
                    else
                        row["Gain"] = 0;
                }
                //row["GainInBaseCurrency"] = double.Parse(row["Gain"].ToString()) * convFactor;

                if (_reportMode.Equals(MODEDAYBASIS) && row["CurrencySymbol"].ToString().Equals("MUL") && isTradeDateTransaction.Equals(true))
                {
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) <= 0.0)
                    {
                        row["GainInBaseCurrency"] = 0;
                    }
                    else
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    }
                }
                else if (_reportMode.Equals(MODEDAYBASIS) && row["CurrencySymbol"].ToString().Equals("MUL") && isTradeDateTransaction.Equals(false))
                {
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) <= 0.0
                        || double.Parse(row["YesterdayMarkPrice"].ToString()) <= 0.0)
                    {
                        row["GainInBaseCurrency"] = 0;
                    }
                    else
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    }
                }
                else if (_reportMode.Equals(MODECOSTBASIS) && row["CurrencySymbol"].ToString().Equals("MUL") && conversionRateOnMarketDate > 0 && conversionRateOnTradeDate > 0) //have to check this case.
                {
                    //row["GainInBaseCurrency"] = double.Parse(row["YesterdayMarketValue"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && conversionRateOnMarketDate > 0)
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["TotalCostInBaseCurrency"].ToString());
                    }
                    else
                    {
                        row["GainInBaseCurrency"] = 0;
                    }
                }
                else if (_reportMode.Equals(MODECOSTBASIS) && row["CurrencySymbol"].ToString() != "MUL")
                {
                    //row["GainInBaseCurrency"] = double.Parse(row["YesterdayMarketValue"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    //if (double.Parse(row["MarketValueInBaseCurrency"].ToString()) > 0)
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && conversionRateOnMarketDate > 0 && conversionRateOnTradeDate > 0)
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["TotalCostInBaseCurrency"].ToString());
                    }
                    else
                    {
                        row["GainInBaseCurrency"] = 0;
                    }
                }
                else
                {
                    //row["GainInBaseCurrency"] = double.Parse(row["Gain"].ToString()) * convFactor;
                    //if (double.Parse(row["MarketValueInBaseCurrency"].ToString()) > 0)
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && conversionRateOnMarketDate > 0 && conversionRateOnYesterdayDate > 0 && isTradeDateTransaction.Equals(true))
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    }
                    else if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && conversionRateOnMarketDate > 0 && conversionRateOnYesterdayDate > 0 && isTradeDateTransaction.Equals(false) && double.Parse(row["YesterdayMarkPrice"].ToString()) > 0)
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    }
                    else
                    {
                        row["GainInBaseCurrency"] = 0;
                    }
                }

                if (!_1stGroup.Equals(string.Empty))
                {
                    row["GroupParamaeter1"] = row[_1stGroup];
                }
                else
                {
                    row["GroupParamaeter1"] = string.Empty;
                }
                if (!_2ndGroup.Equals(string.Empty))
                {
                    row["GroupParamaeter2"] = row[_2ndGroup];
                }
                else
                {
                    row["GroupParamaeter2"] = string.Empty;
                }
                if (!_3rdGroup.Equals(string.Empty))
                {
                    row["GroupParamaeter3"] = row[_3rdGroup];
                }
                else
                {
                    row["GroupParamaeter3"] = string.Empty;
                }
                if (!_4thGroup.Equals(string.Empty))
                {
                    row["GroupParamaeter4"] = row[_4thGroup];
                }
                else
                {
                    row["GroupParamaeter4"] = string.Empty;
                }

                row["LogoPath"] = Application.StartupPath + "\\Reports\\Logo.jpg";
                row["AggregateOption"] = _valueOptionAggregate;
            }
            ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).ClearForexConversionHashForGivenDateCache();

            ReportParameter[] param = new ReportParameter[4];
            param[0] = new ReportParameter("parNoData", "False");
            param[1] = new ReportParameter("parReportDate", _date.ToString("MM/dd/yyyy"));
            param[2] = new ReportParameter("paramCostBasis", _reportMode.ToString());
            param[3] = new ReportParameter("paramReportProcessingStatusBit", REPORTPROCESSDSTATUS.ToString());
            reportViewerValuationSummary.LocalReport.SetParameters(param);

            // ------------------------ On 15th Jan ----------------
            if (dataSetValuationStatement.Tables.Contains("PMGetValuationSummaryReportData"))
            {
                this.dataSetValuationStatement.Tables.Remove("PMGetValuationSummaryReportData");
            }
            this.dataSetValuationStatement.Tables.Add(_dtValuationStatement);

            _dddd = this.dataSetValuationStatement;

            //this.reportViewerValuationSummary.RefreshReport();

            LocalReport localReport = reportViewerValuationSummary.LocalReport;
            //DataSetValuationStatement dataSetValuationStatementNew = new DataSetValuationStatement();
            ReportDataSource repDataSource = new ReportDataSource();
            repDataSource.Name = "DataSource";
            repDataSource.Value = _dataSetValuationStatementUpdated.PMGetValuationSummaryReportData;
            //DataTable dt1 = dataSetValuationStatementNew.Tables[0];
            localReport.DataSources.Add(repDataSource);

            this.reportViewerValuationSummary.RefreshReport();
            _passThrough = true;



            #region Old_Code_For_LiveFeed
            //if (_requestedSymbolList.Contains(_l1data.Symbol))
            //{
            //    string baseCompanyCurrencyLanguageName = string.Empty;
            //    //if(_l1data.Symbol.Equals(
            //    string symbol = string.Empty;
            //    double currentOrMarketPrice = 0;
            //    string currencyLanguageName = string.Empty;
            //    foreach (DataRow row in _dtValuationStatement.Rows)
            //    {
            //        DataRow rowNew = row;
            //        DataColumn colMarketOrCurrentPrice = new DataColumn();
            //        colMarketOrCurrentPrice = (DataColumn)_dtValuationStatement.Columns["MarketOrCurrentPrice"];
            //        colMarketOrCurrentPrice.ReadOnly = false;
            //        DataColumn colMARKETVALUE = new DataColumn();
            //        colMARKETVALUE = (DataColumn)_dtValuationStatement.Columns["MARKETVALUE"];
            //        colMARKETVALUE.ReadOnly = false;
            //        DataColumn colGain = new DataColumn();
            //        colGain = (DataColumn)_dtValuationStatement.Columns["Gain"];
            //        colGain.ReadOnly = false;

            //        DataColumn colLanguageName = new DataColumn();
            //        colLanguageName = (DataColumn)_dtValuationStatement.Columns["LanguageName"];

            //        DataColumn colBaseCurrencyLanguageName = new DataColumn();
            //        colBaseCurrencyLanguageName = (DataColumn)_dtValuationStatement.Columns["BaseCurrencyLanguageName"];
            //        colBaseCurrencyLanguageName.ReadOnly = false;
            //        colLanguageName.ReadOnly = false;

            //        DataColumn colTotalCost = new DataColumn();
            //        colTotalCost = (DataColumn)_dtValuationStatement.Columns["TotalCost"];
            //        colTotalCost.ReadOnly = false;

            //        DataColumn colTotalCostInBaseCurrency = new DataColumn();
            //        colTotalCostInBaseCurrency = (DataColumn)_dtValuationStatement.Columns["TotalCostInBaseCurrency"];
            //        colTotalCostInBaseCurrency.ReadOnly = false;

            //        DataColumn colTotalCostPerShare = new DataColumn();
            //        colTotalCostPerShare = (DataColumn)_dtValuationStatement.Columns["TotalCostPerShare"];
            //        colTotalCostPerShare.ReadOnly = false;

            //        symbol = row["Symbol"].ToString();
            //        currentOrMarketPrice = _l1data.Last;

            //        float futureMultiplier = 1;
            //        if (symbol.Equals(_l1data.Symbol))
            //        {
            //            if (currentOrMarketPrice <= 0)
            //            {
            //                currentOrMarketPrice = 1;
            //            }
            //            currencyLanguageName = GetCurrencyLanguageName(row["CurrencySymbol"].ToString());
            //            row["LanguageName"] = currencyLanguageName;

            //            baseCompanyCurrencyLanguageName = GetCurrencyLanguageName(_companyBaseCurrencySymbol);
            //            row["BaseCurrencyLanguageName"] = baseCompanyCurrencyLanguageName;

            //            row["MarketOrCurrentPrice"] = currentOrMarketPrice;
            //            if (double.Parse(row["TotalCost"].ToString()) < 0)
            //            {
            //                row["MARKETVALUE"] = currentOrMarketPrice * Convert.ToInt32(row["Quantity"]) * -1; //For short.
            //            }
            //            else
            //            {
            //                row["MARKETVALUE"] = currentOrMarketPrice * Convert.ToInt32(row["Quantity"]);
            //            }

            //            symbol = row["Symbol"].ToString();
            //            if (symbol.IndexOf(" ") > 0 && _passThrough.Equals(false))
            //            {
            //                futureMultiplier = float.Parse(CachedDataManager.GetInstance.GetContractMultiplierBySymbol(symbol.Substring(0, symbol.IndexOf(" "))).ToString());
            //                row["TotalCost"] = (double.Parse(row["TotalCost"].ToString()) * futureMultiplier) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString())) ;
            //                row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString()) * futureMultiplier) + ((double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString())));

            //                row["MARKETVALUE"] = double.Parse(row["MARKETVALUE"].ToString()) * futureMultiplier;
            //                row["TotalCostPerShare"] = double.Parse(row["TotalCostPerShare"].ToString()) * (double.Parse(row["TotalCost"].ToString()) / double.Parse(row["Quantity"].ToString()));
            //            }
            //            else
            //            {
            //                row["TotalCost"] = double.Parse(row["TotalCost"].ToString()) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
            //                row["TotalCostPerShare"] = double.Parse(row["TotalCostPerShare"].ToString()) * (double.Parse(row["TotalCost"].ToString()) / double.Parse(row["Quantity"].ToString()));
            //                row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString())) + ((double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString())));
            //            }

            //            row["Gain"] = double.Parse(row["MARKETVALUE"].ToString()) - double.Parse(row["TotalCost"].ToString());

            //            if (!_1stGroup.Equals(string.Empty))
            //            {
            //                row["GroupParamaeter1"] = row[_1stGroup];
            //            }
            //            else
            //            {
            //                row["GroupParamaeter1"] = string.Empty;
            //            }
            //            if (!_2ndGroup.Equals(string.Empty))
            //            {
            //                row["GroupParamaeter2"] = row[_2ndGroup];
            //            }
            //            else
            //            {
            //                row["GroupParamaeter2"] = string.Empty;
            //            }
            //            if (!_3rdGroup.Equals(string.Empty))
            //            {
            //                row["GroupParamaeter3"] = row[_3rdGroup];
            //            }
            //            else
            //            {
            //                row["GroupParamaeter3"] = string.Empty;
            //            }
            //        }
            //    }
            //    _requestedSymbolList.Remove(_l1data.Symbol);
            //}
            //if (_requestedSymbolList.Count.Equals(0))
            //{
            //    if (_passThrough.Equals(false))
            //    {
            //        this.dataSetValuationStatement.Tables.Remove("PMGetValuationSummaryReportData");
            //        this.dataSetValuationStatement.Tables.Add(_dtValuationStatement);

            //        this.reportViewerValuationSummary.RefreshReport();

            //        LocalReport localReport = reportViewerValuationSummary.LocalReport;
            //        //DataSetValuationStatement dataSetValuationStatementNew = new DataSetValuationStatement();
            //        ReportDataSource repDataSource = new ReportDataSource();
            //        repDataSource.Name = "DataSource";
            //        repDataSource.Value = _dataSetValuationStatementUpdated.PMGetValuationSummaryReportData;
            //        //DataTable dt1 = dataSetValuationStatementNew.Tables[0];
            //        localReport.DataSources.Add(repDataSource);

            //        this.reportViewerValuationSummary.RefreshReport();
            //        _requestedSymbolList.Clear();
            //        _passThrough = true;
            //    }
            //}

            #endregion
        }

        private void UpdateReportWithGroupFilters()
        {
            foreach (DataRow row in _dtValuationStatement.Rows)
            {
                //DataColumn colGroupParamaeter1 = new DataColumn();
                //colGroupParamaeter1 = (DataColumn)_dtTransactionStatement.Columns["GroupParamaeter1"];
                //colGroupParamaeter1.ReadOnly = false;
                //DataColumn colGroupParamaeter2 = new DataColumn();
                //colGroupParamaeter2 = (DataColumn)_dtTransactionStatement.Columns["GroupParamaeter2"];
                //colGroupParamaeter2.ReadOnly = false;
                //DataColumn colGroupParamaeter3 = new DataColumn();
                //colGroupParamaeter3 = (DataColumn)_dtTransactionStatement.Columns["GroupParamaeter3"];
                //colGroupParamaeter3.ReadOnly = false;

                if (!_1stGroup.Equals(string.Empty))
                {
                    row["GroupParamaeter1"] = row[_1stGroup];
                }
                else
                {
                    row["GroupParamaeter1"] = string.Empty;
                }
                if (!_2ndGroup.Equals(string.Empty))
                {
                    row["GroupParamaeter2"] = row[_2ndGroup];
                }
                else
                {
                    row["GroupParamaeter2"] = string.Empty;
                }
                if (!_3rdGroup.Equals(string.Empty))
                {
                    row["GroupParamaeter3"] = row[_3rdGroup];
                }
                else
                {
                    row["GroupParamaeter3"] = string.Empty;
                }
                if (!_4thGroup.Equals(string.Empty))
                {
                    row["GroupParamaeter4"] = row[_4thGroup];
                }
                else
                {
                    row["GroupParamaeter4"] = string.Empty;
                }
            }

            if (dataSetValuationStatement.Tables.Contains("PMGetValuationSummaryReportData"))
            {
                this.dataSetValuationStatement.Tables.Remove("PMGetValuationSummaryReportData");
            }


            if (!this.dataSetValuationStatement.Tables.Contains("Table1"))
            {
                this.dataSetValuationStatement.Tables.Add(_dtValuationStatement);
            }

            LocalReport localReport = reportViewerValuationSummary.LocalReport;
            ReportDataSource repDataSource = new ReportDataSource();
            repDataSource.Name = "DataSource";
            repDataSource.Value = _dataSetValuationStatementUpdated.PMGetValuationSummaryReportData;
            localReport.DataSources.Add(repDataSource);

            this.reportViewerValuationSummary.RefreshReport();


        }

        private string GetCurrencyLanguageName(string currencySymbol)
        {
            CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            string languageName = string.Empty;
            foreach (CultureInfo culture in allCultures)
            {
                //Build the regionInfo object from the cultureInfo object
                int lcid = culture.LCID;
                RegionInfo ri = new RegionInfo(lcid);
                string currencyCode = ri.ISOCurrencySymbol;
                if (currencySymbol.Equals(currencyCode))
                {
                    //languageName = culture.EnglishName;
                    languageName = culture.Name;
                    return languageName;
                }
            }
            return languageName;
        }

        private enum GroupFilters
        {
            Account = 0,
            Symbol = 1,
            PositionType = 2,
            MasterFund = 3
        }

        private enum ReportMode
        {
            CostBasis = 0,
            Day = 1
        }

        private enum ReportViewType
        {
            Current = 0,
            Historical = 1
        }

        private string _1stGroup = string.Empty;
        private string _2ndGroup = string.Empty;
        private string _3rdGroup = string.Empty;
        private string _4thGroup = string.Empty;
        private void cmb1stGroup_ValueChanged(object sender, EventArgs e)
        {
            if (cmb2ndGroup.Value != null)
            {
                EnumerationValueList lstGroupParametersTemp = new EnumerationValueList();
                EnumerationValue enumVal = (EnumerationValue)(cmb2ndGroup.SelectedRow.ListObject);
                int output = int.MinValue;
                bool result = false;
                if (int.TryParse(cmb2ndGroup.Value.ToString(), out output))
                {
                    result = true;
                }
                if (result == false)
                {
                    //lstGroupParametersTemp = (EnumerationValueList)lstGroupFiltersFinal.Clone();
                    lstGroupParametersTemp = (EnumerationValueList)lstGroupFilters2nd.Clone();
                    lstGroupParametersTemp.Remove(enumVal);
                    cmb3rdGroup.DataSource = null;
                    cmb3rdGroup.DataSource = lstGroupParametersTemp;
                    Utils.UltraComboFilter(cmb3rdGroup, "DisplayText");


                    lstGroupFilters3rd = lstGroupParametersTemp;
                    cmb4thGroup.DataSource = null;
                    cmb4thGroup.DataSource = lstGroupFilters2nd;
                    Utils.UltraComboFilter(cmb4thGroup, "DisplayText");

                    cmb3rdGroup.Value = int.MinValue;
                    //cmb2ndGroup.Text = "PositionType";

                    cmb4thGroup.Value = int.MinValue;

                    cmb3rdGroup.Enabled = true;
                }
                else
                {
                    cmb3rdGroup.Value = int.MinValue;
                    cmb3rdGroup.Enabled = false;
                }
                string grpByField = cmb2ndGroup.Value.ToString();
                switch (grpByField)
                {
                    case "MasterFund":
                        //GroupTable(GROUPBY_FUND);
                        _2ndGroup = GROUPBY_MASTERFUND;
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                    case "Account":
                        //GroupTable(GROUPBY_TICKER);
                        _2ndGroup = GROUPBY_FUND;
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TICKER);
                        _2ndGroup = GROUPBY_TICKER;
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _2ndGroup = GROUPBY_TYPE;
                        UpdateReportWithGroupFilters();
                        break;
                    default:
                        _2ndGroup = "";
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                }



            }
        }

        private void cmb2ndGroup_ValueChanged(object sender, EventArgs e)
        {
            if (cmb3rdGroup.Value != null)
            {
                EnumerationValueList lstGroupParametersTemp = new EnumerationValueList();
                EnumerationValue enumVal = (EnumerationValue)(cmb3rdGroup.SelectedRow.ListObject);
                int output = int.MinValue;
                bool result = false;
                if (int.TryParse(cmb3rdGroup.Value.ToString(), out output))
                {
                    result = true;
                }
                if (result == false)
                {
                    lstGroupParametersTemp = (EnumerationValueList)lstGroupFilters3rd.Clone();
                    lstGroupParametersTemp.Remove(enumVal);
                    cmb4thGroup.DataSource = null;
                    cmb4thGroup.DataSource = lstGroupParametersTemp;
                    Utils.UltraComboFilter(cmb4thGroup, "DisplayText");

                    cmb4thGroup.Value = int.MinValue;
                    cmb4thGroup.Enabled = true;
                }
                else
                {
                    cmb4thGroup.Value = int.MinValue;
                    cmb4thGroup.Enabled = false;
                }

                string grpByField = cmb3rdGroup.Value.ToString();
                switch (grpByField)
                {
                    case "MasterFund":
                        //GroupTable(GROUPBY_FUND);
                        _3rdGroup = GROUPBY_MASTERFUND;
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                    case "Account":
                        //GroupTable(GROUPBY_TICKER);
                        _3rdGroup = GROUPBY_FUND;
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TICKER);
                        _3rdGroup = GROUPBY_TICKER;
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _3rdGroup = GROUPBY_TYPE;
                        UpdateReportWithGroupFilters();
                        break;
                    default:
                        _3rdGroup = "";
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                }
            }
        }

        private void cmb3rdGroup_ValueChanged(object sender, EventArgs e)
        {
            if (cmb4thGroup.Value != null)
            {
                //EnumerationValueList lstGroupParametersTemp = new EnumerationValueList();
                //object cmb3rdGroupvalue = cmb4thGroup.Value;
                //EnumerationValue enumVal = (EnumerationValue)(cmb4thGroup.SelectedRow.ListObject);
                //int output = int.MinValue;
                //bool result = false;
                //if (int.TryParse(cmb3rdGroup.Value.ToString(), out output))
                //{
                //    result = true;
                //}
                //if (result == false)
                //{
                //    lstGroupParametersTemp = (EnumerationValueList)lstGroupFilters2nd.Clone();
                //    lstGroupParametersTemp.Remove(enumVal);
                //    cmb3rdGroup.DataSource = lstGroupParametersTemp;

                //}

                string grpByField = cmb4thGroup.Value.ToString();
                switch (grpByField)
                {
                    case "MasterFund":
                        //GroupTable(GROUPBY_FUND);
                        _4thGroup = GROUPBY_MASTERFUND;
                        UpdateReportWithGroupFilters();
                        break;
                    case "Account":
                        //GroupTable(GROUPBY_TICKER);
                        _4thGroup = GROUPBY_FUND;
                        UpdateReportWithGroupFilters();
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TICKER);
                        _4thGroup = GROUPBY_TICKER;
                        UpdateReportWithGroupFilters();
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _4thGroup = GROUPBY_TYPE;
                        UpdateReportWithGroupFilters();
                        break;
                    default:
                        _4thGroup = "";
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                }
            }
        }


        private void UpdateReportAggregateOption()
        {
            //Column basecurrency id is used as a check parameter for aggreagate.
            //TODO: remove dependency upon basecurrency parameter and to add some new parameter for this purpose only.
            DataColumn colBaseCurrencyID = new DataColumn();
            colBaseCurrencyID = (DataColumn)_dtValuationStatement.Columns["BaseCurrencyID"];
            colBaseCurrencyID.ReadOnly = false;
            if (_optionAggregatge.Equals(true))
            {
                foreach (DataRow row in _dtValuationStatement.Rows)
                {
                    row["AggregateOption"] = 0; //0 here means that the option aggreagte is used.
                }
                _valueOptionAggregate = 0;

                if (dataSetValuationStatement.Tables.Contains("PMGetValuationSummaryReportData"))
                {
                    this.dataSetValuationStatement.Tables.Remove("PMGetValuationSummaryReportData");
                }


                if (!this.dataSetValuationStatement.Tables.Contains("Table1"))
                {
                    this.dataSetValuationStatement.Tables.Add(_dtValuationStatement);
                }

                LocalReport localReport = reportViewerValuationSummary.LocalReport;
                ReportDataSource repDataSource = new ReportDataSource();
                repDataSource.Name = "DataSource";
                repDataSource.Value = _dataSetValuationStatementUpdated.PMGetValuationSummaryReportData;
                localReport.DataSources.Add(repDataSource);

                this.reportViewerValuationSummary.RefreshReport();

                btnAggregagte.Text = "Show Details";
                _optionAggregatge = false;
            }
            else
            {
                //Back to the normal view without aggregate.
                foreach (DataRow row in _dtValuationStatement.Rows)
                {
                    row["AggregateOption"] = 1; //1 here means that the option aggreagte is not used.
                }
                _valueOptionAggregate = 1;

                if (dataSetValuationStatement.Tables.Contains("PMGetValuationSummaryReportData"))
                {
                    this.dataSetValuationStatement.Tables.Remove("PMGetValuationSummaryReportData");
                }


                if (!this.dataSetValuationStatement.Tables.Contains("Table1"))
                {
                    this.dataSetValuationStatement.Tables.Add(_dtValuationStatement);
                }

                LocalReport localReport = reportViewerValuationSummary.LocalReport;
                ReportDataSource repDataSource = new ReportDataSource();
                repDataSource.Name = "DataSource";
                repDataSource.Value = _dataSetValuationStatementUpdated.PMGetValuationSummaryReportData;
                localReport.DataSources.Add(repDataSource);

                this.reportViewerValuationSummary.RefreshReport();

                btnAggregagte.Text = "Hide Details";
                _optionAggregatge = true;
            }

        }

        bool _optionAggregatge = true;
        private int _valueOptionAggregate = 1;
        private void btnAggregagte_Click(object sender, EventArgs e)
        {
            UpdateReportAggregateOption();
        }

        private void reportViewerValuationSummary_ReportRefresh(object sender, CancelEventArgs e)
        {
            this.reportViewerValuationSummary.Clear();
            GenerateReports();
        }

        private void cmbReportViewType_ValueChanged(object sender, EventArgs e)
        {
            //if (cmbReportViewType.Text.ToString().Equals("Current"))
            //{
            //    dtSelectDate.Enabled = false;
            //    _reportViewType = 0;
            //}
            //else
            //{
            //    dtSelectDate.Enabled = true;
            //    _reportViewType = 1;
            //}
        }

        public static DataSetValuationStatement ProcessAndReturnDataSet(DataSetValuationStatement dataSetValuationStatement)
        {
            //System.Windows.Forms.Cursor currentCursor = Cursor;
            //Cursor = System.Windows.Forms.Cursors.AppStarting;
            //reportViewerValuationSummary.Clear();
            dataSetValuationStatement = _dddd;
            dataSetValuationStatement = GenerateServerReports(dataSetValuationStatement);
            //this.Cursor = currentCursor;

            return dataSetValuationStatement;
        }

        public static int ProcessAndReturnInteger()
        {
            int val = 5;
            return val;
        }


        static bool _passThroughStatic = false;
        static int _reportModeStatic = 0;
        static string reportModeValueStatic = string.Empty;
        static DateTime _dateStatic = DateTime.Now.Date;
        static string _companyBaseCurrencySymbolStatic = string.Empty;
        static DataSetValuationStatement _dddd = new DataSetValuationStatement();
        public static DataSetValuationStatement GenerateServerReports(DataSetValuationStatement dataSetValuationStatement)
        {
            _passThroughStatic = false;
            dataSetValuationStatement.EnforceConstraints = false;

            //string reportModeValue = cmbReportMode.Value.ToString(); To check

            //---------07/07/08---------
            if (reportModeValueStatic == ReportMode.CostBasis.ToString())
            {
                _reportModeStatic = 0;
            }
            else
            {
                _reportModeStatic = 1;
            }
            //---------07/07/08---------

            //---------07/07/08---------
            //_dateStatic = (DateTime)dtSelectDate.Value;
            //string errMessage = " ";
            //int? errNumber = 0;
            //string allAUECDatesString = string.Empty;
            //allAUECDatesString = TimeZoneHelper.GetSameDateForAllAUEC(_date);


            //ReportParameter[] param = new ReportParameter[4];
            //param[0] = new ReportParameter("parNoData", "False");
            //param[1] = new ReportParameter("parReportDate", _date.ToString("MM/dd/yyyy"));
            //param[2] = new ReportParameter("paramCostBasis", _reportMode.ToString());
            //param[3] = new ReportParameter("paramReportProcessingStatusBit", REPORTPROCESSINGSTATUS.ToString());
            //reportViewerValuationSummary.LocalReport.SetParameters(param);

            //if (this.dataSetValuationStatement.PMGetValuationSummaryReportData == null)
            //{
            //    //this.dataSetValuationStatement.PMGetValuationSummaryReportData = new DataSetValuationStatement.PMGetValuationSummaryReportDataDataTable();
            //}
            //else
            //{
            //    this.pMGetValuationSummaryReportDataTableAdapter.Fill(dataSetValuationStatement.PMGetValuationSummaryReportData, 5, 17, _dateStatic, allAUECDatesString, ref errMessage, ref errNumber, _reportModeStatic);
            //    //this.pMGetValuationSummaryReportDataTableAdapter.Fill(this.dataSetValuationStatement.PMGetValuationSummaryReportData, _loginUser.CompanyID, _loginUser.CompanyUserID, date, sbAccountString.ToString(), ref errMessage, ref errNumber);
            //}

            //if (this.dataSetCompanyLogo.PMGetCompanyLogo != null)
            //{
            //    this.pMGetCompanyLogoTableAdapter.Fill(this.dataSetCompanyLogo.PMGetCompanyLogo, 5, 17, ref errMessage, ref errNumber);
            //}


            //_dtValuationStatement = this.pMGetValuationSummaryReportDataTableAdapter.GetData(5, 17, _date, allAUECDatesString, ref errMessage, ref errNumber, _reportModeStatic);
            //---------07/07/08---------

            int companyBaseCurrencyID = int.MinValue;
            if (dataSetValuationStatement.Tables[0].Rows.Count > 0)
            {
                companyBaseCurrencyID = int.Parse(dataSetValuationStatement.Tables[0].Rows[0]["BaseCurrencyID"].ToString());
                Prana.Admin.BLL.Currency currency = Prana.Admin.BLL.AUECManager.GetCurrency(companyBaseCurrencyID);
                _companyBaseCurrencySymbolStatic = currency.CurrencySymbol;

            }
            else
            {
                //---------07/07/08---------
                //param[0] = new ReportParameter("parNoData", "True");
                //param[1] = new ReportParameter("parReportDate", _date.ToString("MM/dd/yyyy"));
                //param[2] = new ReportParameter("paramCostBasis", _reportMode.ToString());
                //param[3] = new ReportParameter("paramReportProcessingStatusBit", REPORTPROCESSDSTATUS.ToString());
                //reportViewerValuationSummary.LocalReport.SetParameters(param);
                //---------07/07/08---------

                //this.reportViewerValuationSummary.RefreshReport();
                return dataSetValuationStatement;
            }
            dataSetValuationStatement = UpdateReportWithCurrentMarketPriceStatic(dataSetValuationStatement);

            return dataSetValuationStatement;
        }

        private static DataSetValuationStatement UpdateReportWithCurrentMarketPriceStatic(DataSetValuationStatement dataSetValuationStatement)
        {
            string baseCompanyCurrencyLanguageName = string.Empty;
            string symbol = string.Empty;
            string currencyLanguageName = string.Empty;
            bool isTradeDateTransaction = false;
            double conversionRateOnTradeDate = 1;
            double conversionRateOnYesterdayDate = 1;
            double conversionRateOnMarketDate = 1;
            DateTime tradeDate = DateTime.Now.Date;
            DateTime dateBeforeTradeDate = DateTime.Now.Date;
            DateTime dateOnMarketDate = DateTime.Now.Date;
            int auecID = 0;
            AssetCategory assetCategory = AssetCategory.None;
            int currencyID = 0;
            int tradedCurrencyID = 0;
            //int vsCurrencyID = 0;
            int baseCurrencyID = 0;
            ForexConverter.GetInstance(5, tradeDate).GetForexData(_dateStatic);

            DataTable dtValuationStatement = dataSetValuationStatement.Tables[0];
            foreach (DataRow row in dtValuationStatement.Rows/* _dtValuationStatement.Rows*/)
            {
                isTradeDateTransaction = false;
                DataColumn colMARKETVALUE = new DataColumn();
                colMARKETVALUE = (DataColumn)dtValuationStatement.Columns["MARKETVALUE"];
                colMARKETVALUE.ReadOnly = false;

                DataColumn colGain = new DataColumn();
                colGain = (DataColumn)dtValuationStatement.Columns["Gain"];
                colGain.ReadOnly = false;

                DataColumn colLanguageName = new DataColumn();
                colLanguageName = (DataColumn)dtValuationStatement.Columns["LanguageName"];

                DataColumn colBaseCurrencyLanguageName = new DataColumn();
                colBaseCurrencyLanguageName = (DataColumn)dtValuationStatement.Columns["BaseCurrencyLanguageName"];
                colBaseCurrencyLanguageName.ReadOnly = false;
                colLanguageName.ReadOnly = false;

                DataColumn colTotalCost = new DataColumn();
                colTotalCost = (DataColumn)dtValuationStatement.Columns["TotalCost"];
                colTotalCost.ReadOnly = false;

                DataColumn colTotalCostInBaseCurrency = new DataColumn();
                colTotalCostInBaseCurrency = (DataColumn)dtValuationStatement.Columns["TotalCostInBaseCurrency"];
                colTotalCostInBaseCurrency.ReadOnly = false;

                DataColumn colTotalCostPerShare = new DataColumn();
                colTotalCostPerShare = (DataColumn)dtValuationStatement.Columns["TotalCostPerShare"];
                colTotalCostPerShare.ReadOnly = false;

                DataColumn colMarketValueInBaseCurrency = new DataColumn();
                colMarketValueInBaseCurrency = (DataColumn)dtValuationStatement.Columns["MarketValueInBaseCurrency"];
                colMarketValueInBaseCurrency.ReadOnly = false;

                DataColumn colGainInBaseCurrency = new DataColumn();
                colGainInBaseCurrency = (DataColumn)dtValuationStatement.Columns["GainInBaseCurrency"];
                colGainInBaseCurrency.ReadOnly = false;

                DataColumn colGroupParamaeter1 = new DataColumn();
                colGroupParamaeter1 = (DataColumn)dtValuationStatement.Columns["GroupParamaeter1"];
                colGroupParamaeter1.ReadOnly = false;

                DataColumn colGroupParamaeter2 = new DataColumn();
                colGroupParamaeter2 = (DataColumn)dtValuationStatement.Columns["GroupParamaeter2"];
                colGroupParamaeter2.ReadOnly = false;

                DataColumn colGroupParamaeter3 = new DataColumn();
                colGroupParamaeter3 = (DataColumn)dtValuationStatement.Columns["GroupParamaeter3"];
                colGroupParamaeter3.ReadOnly = false;

                DataColumn colLogoPath = new DataColumn();
                colLogoPath = (DataColumn)dtValuationStatement.Columns["LogoPath"];
                colLogoPath.ReadOnly = false;

                DataColumn colAggregateOption = new DataColumn();
                colAggregateOption = (DataColumn)dtValuationStatement.Columns["AggregateOption"];
                colAggregateOption.ReadOnly = false;

                symbol = row["Symbol"].ToString();
                auecID = Convert.ToInt32(row["AUECID"]);
                assetCategory = (AssetCategory)Convert.ToInt32(row["AssetID"]);
                currencyID = Convert.ToInt32(row["CurrencyID"]);
                tradedCurrencyID = Convert.ToInt32(row["TradedCurrencyID"]);
                //vsCurrencyID = Convert.ToInt32(row["VsCurrencyID"]);
                baseCurrencyID = Convert.ToInt32(row["BaseCurrencyID"]);

                //double futureMultiplier = 1;

                currencyLanguageName = GetCurrencyLanguageNameStatic(row["CurrencySymbol"].ToString());
                row["LanguageName"] = currencyLanguageName;

                baseCompanyCurrencyLanguageName = GetCurrencyLanguageNameStatic(_companyBaseCurrencySymbolStatic);
                row["BaseCurrencyLanguageName"] = baseCompanyCurrencyLanguageName;

                tradeDate = DateTime.Parse(row["TradeDate"].ToString());
                dateOnMarketDate = _dateStatic;
                if (_reportModeStatic.Equals(MODEDAYBASIS))
                {
                    tradeDate = _dateStatic;
                    //dateBeforeTradeDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(_date, -1, auecID);
                }

                if (_reportModeStatic.Equals(MODEDAYBASIS) && Convert.ToDateTime(row["TradeDate"]).Date.Equals(_dateStatic.Date))
                {
                    dateBeforeTradeDate = tradeDate;
                }
                else if (_reportModeStatic.Equals(MODEDAYBASIS) && Convert.ToDateTime(row["TradeDate"]).Date != _dateStatic.Date)
                {
                    //dateBeforeTradeDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(tradeDate, -1, auecID);
                    if (assetCategory.Equals(AssetCategory.FX))
                    {
                        dateBeforeTradeDate = _dateStatic.AddDays(-1);
                    }
                    else
                    {
                        dateBeforeTradeDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(_dateStatic, -1, auecID);
                    }
                }
                ConversionRate conversionRateOnTradeDt = null;
                ConversionRate conversionRateOnYestDt = null;
                ConversionRate conversionRateOnMarketDt = null;

                if (assetCategory.Equals(AssetCategory.FX))
                {
                    //conversionRateOnTradeDt = ForexConverter.GetInstance().GetFXRateForFXSymbolAndDate(symbol, tradeDate);
                    //conversionRateOnTradeDt = ForexConverter.GetInstance(_loginUser.CompanyID).GetConversionRateFromCurrencies(tradedCurrencyID, baseCurrencyID, tradeDate);
                    if (tradedCurrencyID.Equals(baseCurrencyID))
                    {
                        conversionRateOnTradeDate = 1;
                        conversionRateOnYesterdayDate = 1;
                        conversionRateOnMarketDate = 1;
                    }
                    else
                    {
                        //ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData(tradeDate);
                        //conversionRateOnTradeDt = ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetConversionRateFromCurrenciesForGivenDate(tradedCurrencyID, baseCurrencyID, tradeDate);
                        //ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData(tradeDate); BB
                        conversionRateOnTradeDt = ForexConverter.GetInstance(5, tradeDate).GetConversionRateFromCurrenciesForGivenDate(tradedCurrencyID, baseCurrencyID, tradeDate, 0);
                        if (conversionRateOnTradeDt.ConversionMethod.ToString().Equals("Divide"))
                        {
                            if (conversionRateOnTradeDt.RateValue.Equals(0.0))
                            {
                                conversionRateOnTradeDate = 0;
                            }
                            else
                            {
                                conversionRateOnTradeDate = 1 / conversionRateOnTradeDt.RateValue;
                            }
                        }
                        else
                        {
                            conversionRateOnTradeDate = conversionRateOnTradeDt.RateValue;
                        }

                        if (_reportModeStatic.Equals(MODECOSTBASIS))
                        {
                            //ForexConverter.GetInstance(_loginUser.CompanyID, dateOnMarketDate).GetForexData(dateOnMarketDate);BB
                            conversionRateOnMarketDt = ForexConverter.GetInstance(5, dateOnMarketDate).GetConversionRateFromCurrenciesForGivenDate(tradedCurrencyID, baseCurrencyID, dateOnMarketDate, 0);
                            if (conversionRateOnMarketDt.ConversionMethod.ToString().Equals("Divide"))
                            {
                                if (conversionRateOnMarketDt.RateValue.Equals(0.0))
                                {
                                    conversionRateOnMarketDate = 0;
                                }
                                else
                                {
                                    conversionRateOnMarketDate = 1 / conversionRateOnMarketDt.RateValue;
                                }
                            }
                            else
                            {
                                conversionRateOnMarketDate = conversionRateOnMarketDt.RateValue;
                            }
                        }

                        if (_reportModeStatic.Equals(MODEDAYBASIS))
                        {
                            //ForexConverter.GetInstance(_loginUser.CompanyID, dateBeforeTradeDate).GetForexData(dateBeforeTradeDate);BB
                            conversionRateOnYestDt = ForexConverter.GetInstance(5, dateBeforeTradeDate).GetConversionRateFromCurrenciesForGivenDate(tradedCurrencyID, baseCurrencyID, dateBeforeTradeDate, 0);
                            if (conversionRateOnYestDt.ConversionMethod.ToString().Equals("Divide"))
                            {
                                if (conversionRateOnYestDt.RateValue.Equals(0.0))
                                {
                                    conversionRateOnYesterdayDate = 0;
                                }
                                else
                                {
                                    conversionRateOnYesterdayDate = 1 / conversionRateOnYestDt.RateValue;
                                }
                            }
                            else
                            {
                                conversionRateOnYesterdayDate = conversionRateOnYestDt.RateValue;
                            }
                        }
                    }
                }
                else
                {
                    //conversionRateOnTradeDt = ForexConverter.GetInstance(_loginUser.CompanyID).GetConversionRateForCurrencyToBaseCurrency(currencyID, tradeDate);
                    //Prana.CommonDataCache.ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData();
                    //ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData(tradeDate);
                    //conversionRateOnTradeDt = ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetConversionRateForCurrencyToBaseCurrencyForGivenDate(currencyID, tradeDate);
                    //ForexConverter.GetInstance(_loginUser.CompanyID, tradeDate).GetForexData(tradeDate);BB
                    conversionRateOnTradeDt = ForexConverter.GetInstance(5, tradeDate).GetConversionRateForCurrencyToBaseCurrencyForGivenDate(currencyID, tradeDate, 0);
                    if (conversionRateOnTradeDt.ConversionMethod.ToString().Equals("Divide"))
                    {
                        if (conversionRateOnTradeDt.RateValue.Equals(0.0))
                        {
                            conversionRateOnTradeDate = 0;
                        }
                        else
                        {
                            conversionRateOnTradeDate = 1 / conversionRateOnTradeDt.RateValue;
                        }
                    }
                    else
                    {
                        conversionRateOnTradeDate = conversionRateOnTradeDt.RateValue;
                    }
                    //conversionRateOnTradeDate = conversionRateOnTradeDt.RateValue;

                    if (_reportModeStatic.Equals(MODECOSTBASIS))
                    {
                        //ForexConverter.GetInstance(_loginUser.CompanyID, dateOnMarketDate).GetForexData(dateOnMarketDate);BB
                        conversionRateOnMarketDt = ForexConverter.GetInstance(5, dateOnMarketDate).GetConversionRateForCurrencyToBaseCurrencyForGivenDate(currencyID, dateOnMarketDate, 0);
                        if (conversionRateOnMarketDt.ConversionMethod.ToString().Equals("Divide"))
                        {
                            if (conversionRateOnMarketDt.RateValue.Equals(0.0))
                            {
                                conversionRateOnMarketDate = 0;
                            }
                            else
                            {
                                conversionRateOnMarketDate = 1 / conversionRateOnMarketDt.RateValue;
                            }
                        }
                        else
                        {
                            conversionRateOnMarketDate = conversionRateOnMarketDt.RateValue;
                        }
                    }

                    if (_reportModeStatic.Equals(MODEDAYBASIS))
                    {
                        //ForexConverter.GetInstance(_loginUser.CompanyID, dateBeforeTradeDate).GetForexData(dateBeforeTradeDate);BB
                        conversionRateOnYestDt = ForexConverter.GetInstance(5, dateBeforeTradeDate).GetConversionRateForCurrencyToBaseCurrencyForGivenDate(currencyID, dateBeforeTradeDate, 0);
                        if (conversionRateOnYestDt.ConversionMethod.ToString().Equals("Divide"))
                        {
                            if (conversionRateOnYestDt.RateValue.Equals(0.0))
                            {
                                conversionRateOnYesterdayDate = 0;
                            }
                            else
                            {
                                conversionRateOnYesterdayDate = 1 / conversionRateOnYestDt.RateValue;
                            }
                        }
                        else
                        {
                            conversionRateOnYesterdayDate = conversionRateOnYestDt.RateValue;
                        }
                    }
                }

                //if (true)
                //{

                //}
                //if (CachedDataManager.GetInstance.GetFXConversionRateBySymbolAndDate(symbol, tradeDate) != null)
                //{
                //    conversionRateOnTradeDate = double.Parse(CachedDataManager.GetInstance.GetFXConversionRateBySymbolAndDate(symbol, tradeDate).RateValue.ToString());
                //}

                double convFactor = 1;
                bool isNullconversionFactor = double.TryParse(row["ConversionFactor"].ToString(), out convFactor);
                if (isNullconversionFactor.Equals(false))
                {
                    convFactor = 1;
                }

                if (_passThroughStatic.Equals(false))
                {
                    if (double.Parse(row["TotalCost"].ToString()) < 0)
                    {
                        row["MARKETVALUE"] = double.Parse(row["MARKETVALUE"].ToString()) * -1; //For short.
                        row["YesterdayMarketValue"] = double.Parse(row["YesterdayMarketValue"].ToString()) * -1; //For short.
                    }


                    if (symbol.IndexOf(" ") > 0)
                    {
                        //futureMultiplier = float.Parse(CachedDataManager.GetInstance.GetContractMultiplierBySymbol(symbol.Substring(0, symbol.IndexOf(" "))).ToString());
                        //if (futureMultiplier > 1)
                        //{
                        //    row["TotalCost"] = (double.Parse(row["TotalCost"].ToString()) * futureMultiplier) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                        //    //row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString()) * futureMultiplier) + ((double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()))); Jan 25th
                        //    row["TotalCostInBaseCurrency"] = double.Parse(row["TotalCost"].ToString()) * conversionRateOnMarketDate;//* conversionRateOnTradeDate;

                        //    row["MARKETVALUE"] = double.Parse(row["MARKETVALUE"].ToString()) * futureMultiplier;

                        //    if (_reportModeStatic.Equals(MODECOSTBASIS))
                        //    {
                        //        row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnMarketDate;
                        //    }
                        //    else
                        //    {
                        //        row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnTradeDate;
                        //    }

                        //    //row["GainInBaseCurrency"] = double.Parse(row["Gain"].ToString()) * double.Parse(row["ConversionFactor"].ToString());

                        //    row["TotalCostPerShare"] = double.Parse(row["TotalCostPerShare"].ToString()) * (double.Parse(row["TotalCost"].ToString()) / double.Parse(row["Quantity"].ToString()));

                        //    if (DateTime.Parse(row["TradeDate"].ToString()).Date.Equals(_dateStatic.Date) && _reportModeStatic.Equals(1))
                        //    {
                        //        if (row["CurrencySymbol"].ToString().Equals("MUL"))
                        //        {
                        //            row["YesterdayMarketValue"] = double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                        //        }
                        //        else
                        //        {
                        //            row["YesterdayMarketValue"] = double.Parse(row["TotalCostPerShare"].ToString()) * double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                        //        }
                        //        isTradeDateTransaction = true;
                        //    }
                        //    row["YesterdayMarketValue"] = double.Parse(row["YesterdayMarketValue"].ToString()) * futureMultiplier;
                        //    row["YesterdayMarketValueInBaseCurrency"] = double.Parse(row["YesterdayMarketValue"].ToString()) * conversionRateOnYesterdayDate;
                        //}
                        //else
                        //{
                        //    row["TotalCost"] = double.Parse(row["TotalCost"].ToString()) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                        //    row["TotalCostPerShare"] = double.Parse(row["TotalCostPerShare"].ToString()) * (double.Parse(row["TotalCost"].ToString()) / double.Parse(row["Quantity"].ToString()));
                        //    //row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString())) + ((double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()))); 25th Jan
                        //    row["TotalCostInBaseCurrency"] = double.Parse(row["TotalCost"].ToString()) * conversionRateOnMarketDate;//* conversionRateOnTradeDate;

                        //    if (_reportModeStatic.Equals(MODECOSTBASIS))
                        //    {
                        //        row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnMarketDate;
                        //    }
                        //    else
                        //    {
                        //        row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnTradeDate;
                        //    }
                        //    //row["GainInBaseCurrency"] = double.Parse(row["Gain"].ToString()) * double.Parse(row["ConversionFactor"].ToString());

                        //    if (DateTime.Parse(row["TradeDate"].ToString()).Date.Equals(_dateStatic.Date) && _reportModeStatic.Equals(1))
                        //    {
                        //        if (row["CurrencySymbol"].ToString().Equals("MUL"))
                        //        {
                        //            row["YesterdayMarketValue"] = double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                        //        }
                        //        else
                        //        {
                        //            row["YesterdayMarketValue"] = double.Parse(row["TotalCostPerShare"].ToString()) * double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                        //        }
                        //        isTradeDateTransaction = true;
                        //    }
                        //    row["YesterdayMarketValueInBaseCurrency"] = double.Parse(row["YesterdayMarketValue"].ToString()) * conversionRateOnYesterdayDate;
                        //}
                    }
                    else
                    {
                        row["TotalCost"] = double.Parse(row["TotalCost"].ToString()) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                        row["TotalCostPerShare"] = double.Parse(row["TotalCostPerShare"].ToString()) * (double.Parse(row["TotalCost"].ToString()) / double.Parse(row["Quantity"].ToString()));
                        //row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString())) + ((double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()))); 25th Jan
                        row["TotalCostInBaseCurrency"] = double.Parse(row["TotalCost"].ToString()) * conversionRateOnTradeDate;//* conversionRateOnMarketDate;//

                        if (_reportModeStatic.Equals(MODECOSTBASIS))
                        {
                            row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnMarketDate;
                        }
                        else
                        {
                            row["MarketValueInBaseCurrency"] = double.Parse(row["MARKETVALUE"].ToString()) * conversionRateOnTradeDate;
                        }
                        //row["GainInBaseCurrency"] = double.Parse(row["Gain"].ToString()) * double.Parse(row["ConversionFactor"].ToString());

                        if (DateTime.Parse(row["TradeDate"].ToString()).Date.Equals(_dateStatic.Date) && _reportModeStatic.Equals(1))
                        {
                            if (row["CurrencySymbol"].ToString().Equals("MUL"))
                            {
                                row["YesterdayMarketValue"] = double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                            }
                            else
                            {
                                row["YesterdayMarketValue"] = double.Parse(row["TotalCostPerShare"].ToString()) * double.Parse(row["Quantity"].ToString()) * double.Parse(row["Multiplier"].ToString());
                            }
                            isTradeDateTransaction = true;
                        }
                        row["YesterdayMarketValueInBaseCurrency"] = double.Parse(row["YesterdayMarketValue"].ToString()) * conversionRateOnYesterdayDate;
                    }
                }

                if (_reportModeStatic.Equals(MODECOSTBASIS))
                {
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0)
                    {
                        row["Gain"] = double.Parse(row["MARKETVALUE"].ToString()) - double.Parse(row["TotalCost"].ToString());
                    }
                    else
                    {
                        row["Gain"] = 0;
                    }
                }
                else
                {
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && double.Parse(row["YesterdayMarkPrice"].ToString()) > 0 && isTradeDateTransaction.Equals(false))
                    {
                        row["Gain"] = double.Parse(row["MARKETVALUE"].ToString()) - double.Parse(row["YesterdayMarketValue"].ToString());
                    }
                    else if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && isTradeDateTransaction.Equals(true))
                    {
                        row["Gain"] = double.Parse(row["MARKETVALUE"].ToString()) - double.Parse(row["YesterdayMarketValue"].ToString());
                    }
                    else
                        row["Gain"] = 0;
                }
                //row["GainInBaseCurrency"] = double.Parse(row["Gain"].ToString()) * convFactor;

                if (_reportModeStatic.Equals(MODEDAYBASIS) && row["CurrencySymbol"].ToString().Equals("MUL") && isTradeDateTransaction.Equals(true))
                {
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) <= 0.0)
                    {
                        row["GainInBaseCurrency"] = 0;
                    }
                    else
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    }
                }
                else if (_reportModeStatic.Equals(MODEDAYBASIS) && row["CurrencySymbol"].ToString().Equals("MUL") && isTradeDateTransaction.Equals(false))
                {
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) <= 0.0
                        || double.Parse(row["YesterdayMarkPrice"].ToString()) <= 0.0)
                    {
                        row["GainInBaseCurrency"] = 0;
                    }
                    else
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    }
                }
                else if (_reportModeStatic.Equals(MODECOSTBASIS) && row["CurrencySymbol"].ToString().Equals("MUL")) //have to check this case.
                {
                    //row["GainInBaseCurrency"] = double.Parse(row["YesterdayMarketValue"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && conversionRateOnMarketDate > 0)
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["TotalCostInBaseCurrency"].ToString());
                    }
                    else
                    {
                        row["GainInBaseCurrency"] = 0;
                    }
                }
                else if (_reportModeStatic.Equals(MODECOSTBASIS) && row["CurrencySymbol"].ToString() != "MUL")
                {
                    //row["GainInBaseCurrency"] = double.Parse(row["YesterdayMarketValue"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    //if (double.Parse(row["MarketValueInBaseCurrency"].ToString()) > 0)
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && conversionRateOnMarketDate > 0)
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["TotalCostInBaseCurrency"].ToString());
                    }
                    else
                    {
                        row["GainInBaseCurrency"] = 0;
                    }
                }
                else
                {
                    //row["GainInBaseCurrency"] = double.Parse(row["Gain"].ToString()) * convFactor;
                    //if (double.Parse(row["MarketValueInBaseCurrency"].ToString()) > 0)
                    if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && conversionRateOnMarketDate > 0 && conversionRateOnYesterdayDate > 0 && isTradeDateTransaction.Equals(true))
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    }
                    else if (double.Parse(row["MarketOrCurrentPrice"].ToString()) > 0 && conversionRateOnMarketDate > 0 && conversionRateOnYesterdayDate > 0 && isTradeDateTransaction.Equals(false) && double.Parse(row["YesterdayMarkPrice"].ToString()) > 0)
                    {
                        row["GainInBaseCurrency"] = double.Parse(row["MarketValueInBaseCurrency"].ToString()) - double.Parse(row["YesterdayMarketValueInBaseCurrency"].ToString());
                    }
                    else
                    {
                        row["GainInBaseCurrency"] = 0;
                    }
                }

                //---------07/07/08---------
                //if (!_1stGroup.Equals(string.Empty))
                //{
                //    row["GroupParamaeter1"] = row[_1stGroup];
                //}
                //else
                //{
                //    row["GroupParamaeter1"] = string.Empty;
                //}
                //if (!_2ndGroup.Equals(string.Empty))
                //{
                //    row["GroupParamaeter2"] = row[_2ndGroup];
                //}
                //else
                //{
                //    row["GroupParamaeter2"] = string.Empty;
                //}
                //if (!_3rdGroup.Equals(string.Empty))
                //{
                //    row["GroupParamaeter3"] = row[_3rdGroup];
                //}
                //else
                //{
                //    row["GroupParamaeter3"] = string.Empty;
                //}
                //---------07/07/08---------

                row["LogoPath"] = Application.StartupPath + "\\Reports\\Logo.jpg";
                //row["AggregateOption"] = _valueOptionAggregate; //---------07/07/08---------
            }
            ForexConverter.GetInstance(5, tradeDate).ClearForexConversionHashForGivenDateCache();

            //---------07/07/08---------
            //ReportParameter[] param = new ReportParameter[4];
            //param[0] = new ReportParameter("parNoData", "False");
            //param[1] = new ReportParameter("parReportDate", _date.ToString("MM/dd/yyyy"));
            //param[2] = new ReportParameter("paramCostBasis", _reportMode.ToString());
            //param[3] = new ReportParameter("paramReportProcessingStatusBit", REPORTPROCESSDSTATUS.ToString());
            //reportViewerValuationSummary.LocalReport.SetParameters(param);

            //// ------------------------ On 15th Jan ----------------
            //if (dataSetValuationStatement.Tables.Contains("PMGetValuationSummaryReportData"))
            //{
            //    this.dataSetValuationStatement.Tables.Remove("PMGetValuationSummaryReportData");
            //}
            //this.dataSetValuationStatement.Tables.Add(_dtValuationStatement);

            ////this.reportViewerValuationSummary.RefreshReport();

            //LocalReport localReport = reportViewerValuationSummary.LocalReport;
            ////DataSetValuationStatement dataSetValuationStatementNew = new DataSetValuationStatement();
            //ReportDataSource repDataSource = new ReportDataSource();
            //repDataSource.Name = "DataSource";
            //repDataSource.Value = _dataSetValuationStatementUpdated.PMGetValuationSummaryReportData;
            ////DataTable dt1 = dataSetValuationStatementNew.Tables[0];
            //localReport.DataSources.Add(repDataSource);

            //this.reportViewerValuationSummary.RefreshReport();
            //---------07/07/08---------
            _passThroughStatic = true;

            return dataSetValuationStatement;
        }

        public static string GetCurrencyLanguageNameStatic(string currencySymbol)
        {
            CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            string languageName = string.Empty;
            foreach (CultureInfo culture in allCultures)
            {
                //Build the regionInfo object from the cultureInfo object
                int lcid = culture.LCID;
                RegionInfo ri = new RegionInfo(lcid);
                string currencyCode = ri.ISOCurrencySymbol;
                if (currencySymbol.Equals(currencyCode))
                {
                    //languageName = culture.EnglishName;
                    languageName = culture.Name;
                    return languageName;
                }
            }
            return languageName;
        }

        private void btnStaticRun_Click(object sender, EventArgs e)
        {
            reportModeValueStatic = cmbReportMode.Value.ToString();

            _date = (DateTime)dtSelectDate.Value;
            string errMessage = " ";
            int? errNumber = 0;
            string allAUECDatesString = string.Empty;
            allAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(_date);
            //if (_reportViewType.Equals(0))
            //{
            //    date = DateTime.UtcNow;
            //    allAUECDatesString = TimeZoneHelper.GetAllAUECLocalDatesFromUTCStr(date);
            //}
            //else
            //{
            //    allAUECDatesString = TimeZoneHelper.GetSameDateForAllAUEC(date);
            //}


            ReportParameter[] param = new ReportParameter[4];
            param[0] = new ReportParameter("parNoData", "False");
            param[1] = new ReportParameter("parReportDate", _date.ToString("MM/dd/yyyy"));
            param[2] = new ReportParameter("paramCostBasis", _reportMode.ToString());
            param[3] = new ReportParameter("paramReportProcessingStatusBit", REPORTPROCESSINGSTATUS.ToString());
            reportViewerValuationSummary.LocalReport.SetParameters(param);

            if (this.dataSetValuationStatement.PMGetValuationSummaryReportData == null)
            {
                //this.dataSetValuationStatement.PMGetValuationSummaryReportData = new DataSetValuationStatement.PMGetValuationSummaryReportDataDataTable();
            }
            else
            {
                this.pMGetValuationSummaryReportDataTableAdapter.Fill(this.dataSetValuationStatement.PMGetValuationSummaryReportData, _loginUser.CompanyID, _loginUser.CompanyUserID, _date, allAUECDatesString, ref errMessage, ref errNumber, _reportMode);
                //this.pMGetValuationSummaryReportDataTableAdapter.Fill(this.dataSetValuationStatement.PMGetValuationSummaryReportData, _loginUser.CompanyID, _loginUser.CompanyUserID, date, sbAccountString.ToString(), ref errMessage, ref errNumber);
            }

            if (this.dataSetCompanyLogo.PMGetCompanyLogo != null)
            {
                this.pMGetCompanyLogoTableAdapter.Fill(this.dataSetCompanyLogo.PMGetCompanyLogo, _loginUser.CompanyID, _loginUser.CompanyUserID, ref errMessage, ref errNumber);
            }

            _dtValuationStatement = this.pMGetValuationSummaryReportDataTableAdapter.GetData(_loginUser.CompanyID, _loginUser.CompanyUserID, _date, allAUECDatesString, ref errMessage, ref errNumber, _reportMode);

            int companyBaseCurrencyID = int.MinValue;
            if (_dtValuationStatement.Rows.Count > 0)
            {
                companyBaseCurrencyID = int.Parse(_dtValuationStatement.Rows[0]["BaseCurrencyID"].ToString());
                Prana.Admin.BLL.Currency currency = Prana.Admin.BLL.AUECManager.GetCurrency(companyBaseCurrencyID);
                _companyBaseCurrencySymbol = currency.CurrencySymbol;
            }











            //dataSetValuationStatement = _dddd;
            dataSetValuationStatement = GenerateServerReports(dataSetValuationStatement);
        }

        private void cmb1stGroup_ValueChanged_1(object sender, EventArgs e)
        {
            if (cmb1stGroup.Value != null)
            {
                EnumerationValueList lstGroupParametersTemp = new EnumerationValueList();
                EnumerationValue enumVal = (EnumerationValue)(cmb1stGroup.SelectedRow.ListObject);
                int output = int.MinValue;
                bool result = false;
                if (int.TryParse(cmb1stGroup.Value.ToString(), out output))
                {
                    result = true;
                }
                if (result == false)
                {
                    //lstGroupParametersTemp = (EnumerationValueList)lstGroupFiltersFinal.Clone();
                    lstGroupParametersTemp = (EnumerationValueList)lstGroupFilters.Clone();
                    lstGroupParametersTemp.Remove(enumVal);
                    cmb2ndGroup.DataSource = null;
                    cmb2ndGroup.DataSource = lstGroupParametersTemp;
                    Utils.UltraComboFilter(cmb2ndGroup, "DisplayText");

                    //lstGroupParametersTemp = (EnumerationValueList)lstGroupFiltersFinal.Clone();
                    lstGroupParametersTemp = (EnumerationValueList)lstGroupFilters.Clone();
                    lstGroupParametersTemp.Remove(enumVal);
                    cmb3rdGroup.DataSource = null;
                    cmb3rdGroup.DataSource = lstGroupParametersTemp;
                    Utils.UltraComboFilter(cmb3rdGroup, "DisplayText");


                    lstGroupFilters2nd = lstGroupParametersTemp;
                    cmb4thGroup.DataSource = null;
                    cmb4thGroup.DataSource = lstGroupFilters2nd;
                    Utils.UltraComboFilter(cmb4thGroup, "DisplayText");

                    cmb2ndGroup.Value = int.MinValue;

                    cmb3rdGroup.Value = int.MinValue;
                    //cmb2ndGroup.Text = "PositionType";

                    cmb4thGroup.Value = int.MinValue;

                    cmb2ndGroup.Enabled = true;
                }
                else
                {
                    cmb2ndGroup.Value = int.MinValue;
                    cmb2ndGroup.Enabled = false;
                }
                string grpByField = cmb1stGroup.Value.ToString();
                switch (grpByField)
                {
                    case "MasterFund":
                        //GroupTable(GROUPBY_FUND);
                        _1stGroup = GROUPBY_MASTERFUND;
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                    case "Account":
                        //GroupTable(GROUPBY_TICKER);
                        _1stGroup = GROUPBY_FUND;
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TICKER);
                        _1stGroup = GROUPBY_TICKER;
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _1stGroup = GROUPBY_TYPE;
                        UpdateReportWithGroupFilters();
                        break;
                    default:
                        _1stGroup = "";
                        if (this.LoginUser != null)
                        {
                            UpdateReportWithGroupFilters();
                        }
                        break;
                }
            }
        }

    }
}
