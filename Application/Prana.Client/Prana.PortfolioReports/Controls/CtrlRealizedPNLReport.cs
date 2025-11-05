using Microsoft.Reporting.WinForms;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace Prana.PortfolioReports.Controls
{
    public partial class CtrlRealizedPNLReport : UserControl
    {
        const string GROUPBY_FUND = "AccountName";
        const string GROUPBY_TICKER = "Ticker";
        const string GROUPBY_TYPE = "Type";

        public CtrlRealizedPNLReport()
        {
            InitializeComponent();
            this.reportViewerRealizedPNL.LocalReport.ReportPath = Application.StartupPath + "\\Reports\\TransactionSummary.rdlc";
        }

        //ILiveFeedPublisher _liveFeedCacheInstance = null;
        private CompanyUser _loginUser;
        public CompanyUser LoginUser
        {
            get { return _loginUser; }
            set
            {
                _loginUser = value;
                if (_loginUser == null)
                {
                    //throw new Exception("User could not be found while initializing valuation summary Reports");
                }

            }
        }

        public void SetupControl()
        {
            dtStartDate.Value = DateTime.Now;
            dtStartDate.MaxDate = DateTime.Now;
            //dtMonth.MaskInput = "{LOC}mm/yyyy";

            dtEndDate.Value = DateTime.Now;
            dtEndDate.MaxDate = DateTime.Now;

            //BindAccountList();
            //BindDataSourceList();
            BindComboGroupFilters();

            //try
            //{
            //    GenerateReports();
            //}
            //catch (Exception ex)
            //{

            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, Common.POLICY_LOGANDTHROW);

            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
        }

        EnumerationValueList lstGroupFilters = new EnumerationValueList();
        EnumerationValueList lstGroupFilters2nd = new EnumerationValueList();
        private void BindComboGroupFilters()
        {
            cmb1stGroup.DisplayMember = "DisplayText";
            cmb1stGroup.ValueMember = "Value";
            lstGroupFilters = EnumHelper.ConvertEnumForBindingWithSelect(typeof(GroupFilters));
            cmb1stGroup.DataSource = null;
            cmb1stGroup.DataSource = lstGroupFilters;
            Utils.UltraComboFilter(cmb1stGroup, "DisplayText");
            cmb1stGroup.Value = int.MinValue;

            cmb2ndGroup.DisplayMember = "DisplayText";
            cmb2ndGroup.ValueMember = "Value";
            cmb2ndGroup.DataSource = null;
            cmb2ndGroup.DataSource = lstGroupFilters;
            Utils.UltraComboFilter(cmb2ndGroup, "DisplayText");
            cmb2ndGroup.Value = int.MinValue;
            cmb2ndGroup.Enabled = false;

            cmb3rdGroup.DisplayMember = "DisplayText";
            cmb3rdGroup.ValueMember = "Value";
            cmb3rdGroup.DataSource = null;
            cmb3rdGroup.DataSource = lstGroupFilters;
            Utils.UltraComboFilter(cmb3rdGroup, "DisplayText");
            cmb3rdGroup.Value = int.MinValue;
            cmb3rdGroup.Enabled = false;
        }

        DataTable _dtTransactionStatement = new DataTable();
        List<string> _requestedSymbolList = new List<string>();
        DataSetRealizedPNL _dataSetRealizedPNLUpdated = new DataSetRealizedPNL();
        bool _passThrough = false;
        private string _companyBaseCurrencySymbol = string.Empty;
        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            this.reportViewerRealizedPNL.Clear();
            GenerateReports();
        }

        public void GenerateReports()
        {
            try
            {
                _passThrough = false;
                _requestedSymbolList.Clear();
                dataSetRealizedPNL.EnforceConstraints = false;

                //LocalReport localReportNew = reportViewerRealizedPNL.LocalReport;
                //ReportParameter param = new ReportParameter("Ticker");
                //List<ReportParameter> listReportParameter = new List<ReportParameter>();
                //localReportNew.SetParameters(listReportParameter);


                DateTime date = (DateTime)dtStartDate.Value;
                string errMessage = " ";
                int? errNumber = 0;
                if (this.dataSetRealizedPNL.PMGetRealizedPNLReport == null)
                {
                }
                else
                {
                    this.pMGetRealizedPNLReportTableAdapter.Fill(this.dataSetRealizedPNL.PMGetRealizedPNLReport, _loginUser.CompanyID, _loginUser.CompanyUserID, date.ToUniversalTime(), TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(date), ref errMessage, ref errNumber);
                }

                _dtTransactionStatement = this.pMGetRealizedPNLReportTableAdapter.GetData(_loginUser.CompanyID, _loginUser.CompanyUserID, date.ToUniversalTime(), TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(date), ref errMessage, ref errNumber);

                int companyBaseCurrencyID = int.MinValue;
                if (_dtTransactionStatement.Rows.Count > 0)
                {
                    companyBaseCurrencyID = int.Parse(_dtTransactionStatement.Rows[0]["BaseCurrencyID"].ToString());
                    Prana.Admin.BLL.Currency currency = Prana.Admin.BLL.AUECManager.GetCurrency(companyBaseCurrencyID);
                    _companyBaseCurrencySymbol = currency.CurrencySymbol;

                    //GroupTable();
                    UpdateReportAfterCurrencyLangSet();
                }
                else
                {
                    //this.reportViewerRealizedPNL.RefreshReport();

                    LocalReport localReport = reportViewerRealizedPNL.LocalReport;
                    //DataSetTransactionSummary dataSetTransactionSummaryNew = new DataSetTransactionSummary();
                    ReportDataSource repDataSource = new ReportDataSource();
                    repDataSource.Name = "DataSource";
                    repDataSource.Value = _dtTransactionStatement;//dataSetRealizedPNL.PMGetTransactionSummaryReportData;
                    localReport.DataSources.Add(repDataSource);

                    //this.reportViewerRealizedPNL.RefreshReport();

                    //return;

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



            #region LiveFeed_Interaction_Code
            //if (_dtTransactionStatement.Rows.Count > 0)
            //{
            //    _liveFeedCacheInstance = LiveFeedInstanceCreator.Instance;
            //    if (_liveFeedCacheInstance != null)
            //    {
            //        _liveFeedCacheInstance.PublishLevel1SnapshotResponse += new EventHandler(_liveFeedCacheInstance_PublishLevel1SnapshotResponse);
            //    }
            //}
            //else
            //{
            //    this.reportViewerRealizedPNL.RefreshReport();

            //    LocalReport localReport = reportViewerRealizedPNL.LocalReport;
            //    //DataSetTransactionSummary dataSetTransactionSummaryNew = new DataSetTransactionSummary();
            //    ReportDataSource repDataSource = new ReportDataSource();
            //    repDataSource.Name = "DataSource";
            //    repDataSource.Value = _dtTransactionStatement;//dataSetRealizedPNL.PMGetTransactionSummaryReportData;
            //    DataTable dt1 = dataSetRealizedPNL.Tables[0];
            //    localReport.DataSources.Add(repDataSource);

            //    this.reportViewerRealizedPNL.RefreshReport();



            //    return;
            //}
            //string symbol = string.Empty;
            //double currentOrMarketPrice = 0;
            //if (!_liveFeedCacheInstance.IsDataManagerConnected())
            //{
            //    this.reportViewerRealizedPNL.RefreshReport();

            //    LocalReport localReport = reportViewerRealizedPNL.LocalReport;
            //    ReportDataSource repDataSource = new ReportDataSource();
            //    repDataSource.Name = "DataSource";
            //    repDataSource.Value = dataSetRealizedPNL.PMGetTransactionSummaryReportData;
            //    DataTable dt1 = dataSetRealizedPNL.Tables[0];
            //    localReport.DataSources.Add(repDataSource);

            //    this.reportViewerRealizedPNL.RefreshReport();
            //}
            //else
            //{

            //    foreach (DataRow row in _dtTransactionStatement.Rows)
            //    {
            //        DataRow rowNew = row;
            //        if (_liveFeedCacheInstance.IsDataManagerConnected())
            //        {
            //            symbol = row["Ticker"].ToString();

            //            if (!_requestedSymbolList.Contains(symbol))
            //            {
            //                _requestedSymbolList.Add(symbol);
            //                _liveFeedCacheInstance.RequestLevel1Snapshot(symbol);
            //            }
            //        }
            //    }

            //}
            #endregion
        }

        //private DataSet _grpDataSetTransactionSummary = new DataSet();
        //DataSetHelper _dsHelper = new DataSetHelper();
        //DataTable _dtCreateGroupByTableDest = new DataTable();

        private void UpdateReportAfterCurrencyLangSet()
        {
            try
            {
                //if (_passThrough.Equals(false))
                //{
                string symbol = string.Empty;
                string currencyLanguageName = string.Empty;
                string baseCompanyCurrencyLanguageName = string.Empty;
                //double futureMultiplier = 1;
                foreach (DataRow row in _dtTransactionStatement.Rows)
                {
                    DataColumn colLanguageName = new DataColumn();
                    colLanguageName = (DataColumn)_dtTransactionStatement.Columns["LanguageName"];
                    colLanguageName.ReadOnly = false;

                    DataColumn colGroupParamaeter1 = new DataColumn();
                    colGroupParamaeter1 = (DataColumn)_dtTransactionStatement.Columns["GroupParamaeter1"];
                    colGroupParamaeter1.ReadOnly = false;
                    DataColumn colGroupParamaeter2 = new DataColumn();
                    colGroupParamaeter2 = (DataColumn)_dtTransactionStatement.Columns["GroupParamaeter2"];
                    colGroupParamaeter2.ReadOnly = false;
                    DataColumn colGroupParamaeter3 = new DataColumn();
                    colGroupParamaeter3 = (DataColumn)_dtTransactionStatement.Columns["GroupParamaeter3"];
                    colGroupParamaeter3.ReadOnly = false;

                    DataColumn colBaseCurrencyLanguageName = new DataColumn();
                    colBaseCurrencyLanguageName = (DataColumn)_dtTransactionStatement.Columns["BaseCurrencyLanguageName"];
                    colBaseCurrencyLanguageName.ReadOnly = false;

                    DataColumn colTotalCost = new DataColumn();
                    colTotalCost = (DataColumn)_dtTransactionStatement.Columns["TotalCost"];
                    colTotalCost.ReadOnly = false;

                    DataColumn colTotalCostInBaseCurrency = new DataColumn();
                    colTotalCostInBaseCurrency = (DataColumn)_dtTransactionStatement.Columns["TotalCostInBaseCurrency"];
                    colTotalCostInBaseCurrency.ReadOnly = false;

                    DataColumn colLongTermCapitalGains = new DataColumn();
                    colLongTermCapitalGains = (DataColumn)_dtTransactionStatement.Columns["LongTermCapitalGains"];
                    colLongTermCapitalGains.ReadOnly = false;

                    DataColumn colShortTermCapitalGains = new DataColumn();
                    colShortTermCapitalGains = (DataColumn)_dtTransactionStatement.Columns["ShortTermCapitalGains"];
                    colShortTermCapitalGains.ReadOnly = false;

                    currencyLanguageName = GetCurrencyLanguageName(row["CurrencySymbol"].ToString());
                    row["LanguageName"] = currencyLanguageName;//"ja-JP"; //currencyLanguageName.ToString();

                    baseCompanyCurrencyLanguageName = GetCurrencyLanguageName(_companyBaseCurrencySymbol);
                    row["BaseCurrencyLanguageName"] = baseCompanyCurrencyLanguageName;

                    symbol = row["Ticker"].ToString();
                    if (_passThrough.Equals(false)/* && _3rdGroup.Equals(string.Empty) && _2ndGroup.Equals(string.Empty) && _1stGroup.Equals(string.Empty)*/)
                    {
                        if (String.IsNullOrEmpty(symbol))
                        {
                            continue;
                        }
                        if (symbol.IndexOf(" ") > 0)
                        {
                            //futureMultiplier = float.Parse(CachedDataManager.GetInstance.GetContractMultiplierBySymbol(symbol.Substring(0, symbol.IndexOf(" "))).ToString());
                            //if (futureMultiplier > 1)
                            //{
                            //    row["TotalCost"] = (double.Parse(row["TotalCost"].ToString()) * futureMultiplier) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                            //    row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString()) * futureMultiplier) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));

                            //    if ((DateTime.Parse(row["SettlementDate"].ToString()).AddYears(1).AddYears(-(DateTime.Parse(row["TradeDate"].ToString()).Year))).Year > 1)
                            //    {
                            //        //Long Term gains
                            //        row["LongTermCapitalGains"] = (double.Parse(row["LongTermCapitalGains"].ToString()) * futureMultiplier) - double.Parse(row["RealizedPNLExpenses"].ToString());
                            //    }
                            //    else
                            //    {
                            //        //Short Term gains
                            //        row["ShortTermCapitalGains"] = ((double.Parse(row["ShortTermCapitalGains"].ToString()) * futureMultiplier) - double.Parse(row["RealizedPNLExpenses"].ToString()));
                            //    }
                            //}
                            //else
                            //{
                            //    row["TotalCost"] = (double.Parse(row["TotalCost"].ToString())) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                            //    row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString())) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));

                            //    if ((DateTime.Parse(row["SettlementDate"].ToString()).AddYears(1).AddYears(-(DateTime.Parse(row["TradeDate"].ToString()).Year)).Year > 1))
                            //    {
                            //        //Long Term gains
                            //        row["LongTermCapitalGains"] = double.Parse(row["LongTermCapitalGains"].ToString()) - double.Parse(row["RealizedPNLExpenses"].ToString());
                            //    }
                            //    else
                            //    {
                            //        //Short Term gains
                            //        row["ShortTermCapitalGains"] = (double.Parse(row["ShortTermCapitalGains"].ToString()) - double.Parse(row["RealizedPNLExpenses"].ToString()));
                            //    }
                            //}
                        }
                        else
                        {
                            row["TotalCost"] = (double.Parse(row["TotalCost"].ToString())) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                            row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString())) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));

                            if ((DateTime.Parse(row["SettlementDate"].ToString()).AddYears(1).AddYears(-(DateTime.Parse(row["TradeDate"].ToString()).Year)).Year > 1))
                            {
                                //Long Term gains
                                row["LongTermCapitalGains"] = double.Parse(row["LongTermCapitalGains"].ToString()) - double.Parse(row["RealizedPNLExpenses"].ToString());
                            }
                            else
                            {
                                //Short Term gains
                                row["ShortTermCapitalGains"] = (double.Parse(row["ShortTermCapitalGains"].ToString()) - double.Parse(row["RealizedPNLExpenses"].ToString()));
                            }
                        }
                    }

                    if (!_3rdGroup.Equals(string.Empty))
                    {
                        row["GroupParamaeter3"] = row[_3rdGroup];
                    }
                    else
                    {
                        row["GroupParamaeter3"] = string.Empty;
                    }
                    if (!_2ndGroup.Equals(string.Empty))
                    {
                        row["GroupParamaeter2"] = row[_2ndGroup];
                    }
                    else
                    {
                        row["GroupParamaeter2"] = string.Empty;
                    }

                    if (!_1stGroup.Equals(string.Empty))
                    {
                        row["GroupParamaeter1"] = row[_1stGroup];
                    }
                    else
                    {
                        row["GroupParamaeter1"] = string.Empty;
                    }

                }

                if (this.dataSetRealizedPNL.Tables["PMGetRealizedPNLReport"] != null)
                {
                    this.dataSetRealizedPNL.Tables.Remove("PMGetRealizedPNLReport");
                }
                if (!this.dataSetRealizedPNL.Tables.Contains("Table1"))
                {
                    this.dataSetRealizedPNL.Tables.Add(_dtTransactionStatement);
                }

                //this.reportViewerRealizedPNL.RefreshReport();

                LocalReport localReport = reportViewerRealizedPNL.LocalReport;
                //localReport.

                ReportDataSource repDataSource = new ReportDataSource();
                repDataSource.Name = "DataSource";
                repDataSource.Value = _dataSetRealizedPNLUpdated.PMGetRealizedPNLReport;
                localReport.DataSources.Add(repDataSource);

                this.reportViewerRealizedPNL.RefreshReport();
                _passThrough = true;



                //}
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



        #region LiveFeed_Interaction_Code_Extended
        //Level1Data _l1data = new Level1Data();
        //void _liveFeedCacheInstance_PublishLevel1SnapshotResponse(object sender, EventArgs e)
        //{
        //    LiveFeedEventArgs liveFeedArg = (LiveFeedEventArgs)e;
        //    _l1data = liveFeedArg.SnapshotLevel1Data;

        //    UpdateReportWithCurrentMarketPrice();
        //}

        //private void UpdateReportWithCurrentMarketPrice()
        //{
        //    if (_requestedSymbolList.Contains(_l1data.Symbol))
        //    {
        //        string symbol = string.Empty;
        //        double currentOrMarketPrice = 0;
        //        string currencyLanguageName = string.Empty;
        //        foreach (DataRow row in _dtTransactionStatement.Rows)
        //        {
        //            DataRow rowNew = row;
        //            DataColumn colMarketOrCurrentPrice = new DataColumn();
        //            colMarketOrCurrentPrice = (DataColumn)_dtTransactionStatement.Columns["MarketOrCurrentPrice"];
        //            colMarketOrCurrentPrice.ReadOnly = false;

        //            DataColumn colShortTermCapitalGains = new DataColumn();
        //            colShortTermCapitalGains = (DataColumn)_dtTransactionStatement.Columns["ShortTermCapitalGains"];
        //            colShortTermCapitalGains.ReadOnly = false;

        //            DataColumn colLanguageName = new DataColumn();
        //            colLanguageName = (DataColumn)_dtTransactionStatement.Columns["LanguageName"];
        //            colLanguageName.ReadOnly = false;


        //            //DataColumn colMARKETVALUE = new DataColumn();
        //            //colMARKETVALUE = (DataColumn)_dtValuationStatement.Columns["MARKETVALUE"];
        //            //colMARKETVALUE.ReadOnly = false;
        //            //DataColumn colGain = new DataColumn();
        //            //colGain = (DataColumn)_dtValuationStatement.Columns["Gain"];
        //            //colGain.ReadOnly = false;

        //            symbol = row["Ticker"].ToString();
        //            currentOrMarketPrice = _l1data.Last;
        //            if (symbol.Equals(_l1data.Symbol))
        //            {
        //                if (currentOrMarketPrice <= 0)
        //                {
        //                    currentOrMarketPrice = 1;
        //                }
        //                row["MarketOrCurrentPrice"] = currentOrMarketPrice;
        //                //row["Price"] = currentOrMarketPrice;
        //                //row["ShortTermCapitalGains"] = row["ShortTermCapitalGains"].ToString();//"Y " + row["ShortTermCapitalGains"].ToString();

        //                //row["MARKETVALUE"] = currentOrMarketPrice * Convert.ToInt32(row["Quantity"]);
        //                //row["Gain"] = double.Parse(row["MARKETVALUE"].ToString()) - double.Parse(row["TotalCost"].ToString());

        //                currencyLanguageName = GetCurrencyLanguageName(row["CurrencySymbol"].ToString());
        //                row["LanguageName"] = currencyLanguageName;
        //            }
        //        }
        //        _requestedSymbolList.Remove(_l1data.Symbol);
        //    }
        //    if (_requestedSymbolList.Count.Equals(0))
        //    {
        //        if (_passThrough.Equals(false))
        //        {
        //            this.dataSetRealizedPNL.Tables.Remove("PMGetTransactionSummaryReportData");
        //            this.dataSetRealizedPNL.Tables.Add(_dtTransactionStatement);

        //            this.reportViewerRealizedPNL.RefreshReport();

        //            LocalReport localReport = reportViewerRealizedPNL.LocalReport;
        //            ReportDataSource repDataSource = new ReportDataSource();
        //            repDataSource.Name = "DataSource";
        //            repDataSource.Value = _dataSetTransactionSummaryUpdated.PMGetTransactionSummaryReportData;
        //            localReport.DataSources.Add(repDataSource);

        //            this.reportViewerRealizedPNL.RefreshReport();
        //            _requestedSymbolList.Clear();
        //            _passThrough = true;
        //        }
        //    }
        //}
        #endregion

        private enum GroupFilters
        {
            Account = 0,
            Symbol = 1,
            PositionType = 2,
        }

        private string _1stGroup = string.Empty;
        private string _2ndGroup = string.Empty;
        private string _3rdGroup = string.Empty;
        private void cmb1stGroup_ValueChanged(object sender, EventArgs e)
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


                    lstGroupFilters2nd = lstGroupParametersTemp;
                    cmb3rdGroup.DataSource = lstGroupFilters2nd;

                    cmb2ndGroup.Value = int.MinValue;
                    cmb3rdGroup.Value = int.MinValue;

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
                    case "Account":
                        //GroupTable(GROUPBY_FUND);
                        _1stGroup = GROUPBY_FUND;
                        UpdateReportAfterCurrencyLangSet();
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TICKER);
                        _1stGroup = GROUPBY_TICKER;
                        UpdateReportAfterCurrencyLangSet();
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _1stGroup = GROUPBY_TYPE;
                        UpdateReportAfterCurrencyLangSet();
                        break;
                    default:
                        _1stGroup = "";
                        if (this.LoginUser != null)
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                }



            }
        }

        private void cmb2ndGroup_ValueChanged(object sender, EventArgs e)
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
                    lstGroupParametersTemp = (EnumerationValueList)lstGroupFilters2nd.Clone();
                    lstGroupParametersTemp.Remove(enumVal);
                    cmb3rdGroup.DataSource = null;
                    cmb3rdGroup.DataSource = lstGroupParametersTemp;

                    cmb3rdGroup.Value = int.MinValue;
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
                    case "Account":
                        //GroupTable(GROUPBY_FUND);
                        _2ndGroup = GROUPBY_FUND;
                        UpdateReportAfterCurrencyLangSet();
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TICKER);
                        _2ndGroup = GROUPBY_TICKER;
                        UpdateReportAfterCurrencyLangSet();
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _2ndGroup = GROUPBY_TYPE;
                        UpdateReportAfterCurrencyLangSet();
                        break;
                    default:
                        _2ndGroup = "";
                        if (this.LoginUser != null)
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                }
            }
        }

        private void cmb3rdGroup_ValueChanged(object sender, EventArgs e)
        {
            if (cmb3rdGroup.Value != null)
            {
                //EnumerationValueList lstGroupParametersTemp = new EnumerationValueList();
                //object cmb3rdGroupvalue = cmb3rdGroup.Value;
                //EnumerationValue enumVal = (EnumerationValue)(cmb3rdGroup.SelectedRow.ListObject);
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

                string grpByField = cmb3rdGroup.Value.ToString();
                switch (grpByField)
                {
                    case "Account":
                        //GroupTable(GROUPBY_FUND);
                        _3rdGroup = GROUPBY_FUND;
                        UpdateReportAfterCurrencyLangSet();
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TICKER);
                        _3rdGroup = GROUPBY_TICKER;
                        UpdateReportAfterCurrencyLangSet();
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _3rdGroup = GROUPBY_TYPE;
                        UpdateReportAfterCurrencyLangSet();
                        break;
                    default:
                        _3rdGroup = "";
                        if (this.LoginUser != null)
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                }
            }
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



    }
}
