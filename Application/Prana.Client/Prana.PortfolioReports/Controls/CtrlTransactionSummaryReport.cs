using Microsoft.Reporting.WinForms;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace Prana.PortfolioReports
{
    public partial class CtrlTransactionSummaryReport : UserControl
    {
        const string GROUPBY_MASTERFUND = "MasterFundName";
        const string GROUPBY_FUND = "AccountName";
        const string GROUPBY_TICKER = "Ticker";
        const string GROUPBY_TYPE = "Type";

        public CtrlTransactionSummaryReport()
        {
            InitializeComponent();
            this.reportViewerTransactionSummary.LocalReport.ReportPath = Application.StartupPath + "\\Reports\\TransactionSummary.rdlc";
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

        //public string GetAppPath()
        //{
        //    return System.IO.Directory.GetCurrentDirectory();
        //}

        bool _isMasterFundAssociationSaved = false;
        public void SetupControl()
        {
            _buttonGenerateClicked = false;
            _passThrough = false;
            dtStartDate.Value = DateTime.Now;
            //dtStartDate.MaxDate = DateTime.Now; 21st Jan
            //dtMonth.MaskInput = "{LOC}mm/yyyy";

            //_isMasterFundAssociationSaved = CachedDataManager.GetInstance.IsMasterFundAssociationSaved;
            _isMasterFundAssociationSaved = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("MasterFundAssociation").ToString());

            dtEndDate.Value = DateTime.Now;
            //dtEndDate.MaxDate = DateTime.Now; 21st Jan

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
            //cmb1stGroup.Value = int.MinValue; On 15th Jan
            //cmb1stGroup.Text = "MasterFund";
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
            //lstGroupFilters = EnumHelper.ConvertEnumForBindingWithSelect(typeof(GroupFilters));
            //cmb2ndGroup.DataSource = null;
            //cmb2ndGroup.DataSource = lstGroupFilters;
            Utils.UltraComboFilter(cmb2ndGroup, "DisplayText");
            //cmb1stGroup.Value = int.MinValue; On 15th Jan
            //cmb2ndGroup.Text = "Account";

            cmb3rdGroup.DisplayMember = "DisplayText";
            cmb3rdGroup.ValueMember = "Value";
            // ------------------------ On 15th Jan ----------------
            //cmb2ndGroup.DataSource = null;
            //cmb2ndGroup.DataSource = lstGroupFilters;
            Utils.UltraComboFilter(cmb3rdGroup, "DisplayText");
            //cmb2ndGroup.Value = int.MinValue;
            //cmb2ndGroup.Enabled = false; 

            cmb4thGroup.DisplayMember = "DisplayText";
            cmb4thGroup.ValueMember = "Value";
            //cmb3rdGroup.DataSource = null;
            //cmb3rdGroup.DataSource = lstGroupFilters;
            //Utils.UltraComboFilter(cmb3rdGroup, "DisplayText");
            //cmb3rdGroup.Value = int.MinValue;
            //cmb3rdGroup.Enabled = false;
        }

        DataTable _dtTransactionStatement = new DataTable();
        List<string> _requestedSymbolList = new List<string>();
        DataSetTransactionSummary _dataSetTransactionSummaryUpdated = new DataSetTransactionSummary();
        bool _passThrough = false;
        private string _companyBaseCurrencySymbol = string.Empty;
        private bool _buttonGenerateClicked = false;
        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            _buttonGenerateClicked = true;
            this.reportViewerTransactionSummary.Clear();
            GenerateReports();
        }

        public void GenerateReports()
        {
            try
            {
                System.Windows.Forms.Cursor currentCursor = this.Cursor;
                this.Cursor = System.Windows.Forms.Cursors.AppStarting;

                //_passThrough = false; //15th Jan
                _requestedSymbolList.Clear();
                dataSetTransactionSummary.EnforceConstraints = false;
                this.reportViewerTransactionSummary.LocalReport.EnableExternalImages = true;

                //LocalReport localReportNew = reportViewerTransactionSummary.LocalReport;
                //ReportParameter param = new ReportParameter("Ticker");
                //List<ReportParameter> listReportParameter = new List<ReportParameter>();
                //localReportNew.SetParameters(listReportParameter);


                DateTime startDate = (DateTime)dtStartDate.Value;
                DateTime endDate = (DateTime)dtEndDate.Value;
                string errMessage = " ";
                int? errNumber = 0;
                if (this.dataSetTransactionSummary.PMGetTransactionSummaryReportData == null)
                {
                }
                else
                {

                    this.pMGetTransactionSummaryReportDataTableAdapter.Fill(this.dataSetTransactionSummary.PMGetTransactionSummaryReportData, _loginUser.CompanyID, _loginUser.CompanyUserID, startDate, TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(startDate), endDate, TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(endDate), ref errMessage, ref errNumber);
                }

                if (this.dataSetCompanyLogo.PMGetCompanyLogo != null)
                {
                    this.pMGetCompanyLogoTableAdapter.Fill(this.dataSetCompanyLogo.PMGetCompanyLogo, _loginUser.CompanyID, _loginUser.CompanyUserID, ref errMessage, ref errNumber);
                }

                _dtTransactionStatement = this.pMGetTransactionSummaryReportDataTableAdapter.GetData(_loginUser.CompanyID, _loginUser.CompanyUserID, startDate, TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(startDate), endDate, TimeZoneHelper.GetInstance().GetSameDateForAllAUEC(endDate), ref errMessage, ref errNumber);

                int companyBaseCurrencyID = int.MinValue;
                if (_dtTransactionStatement.Rows.Count > 0)
                {
                    companyBaseCurrencyID = int.Parse(_dtTransactionStatement.Rows[0]["BaseCurrencyID"].ToString());
                    Prana.Admin.BLL.Currency currency = Prana.Admin.BLL.AUECManager.GetCurrency(companyBaseCurrencyID);
                    _companyBaseCurrencySymbol = currency.CurrencySymbol;

                    //GroupTable();
                    ReportParameter[] param = new ReportParameter[3];
                    param[0] = new ReportParameter("parNoData", "False");
                    param[1] = new ReportParameter("parReportDate", startDate.ToString("MM/dd/yyyy"));
                    param[2] = new ReportParameter("parReportEndDate", endDate.ToString("MM/dd/yyyy"));
                    //param[2] = new ReportParameter("parReportEndDate", endDate.ToString("MM/dd/yyyy"));
                    reportViewerTransactionSummary.LocalReport.SetParameters(param);
                    UpdateReportAfterCurrencyLangSet();
                }
                else
                {
                    //this.reportViewerTransactionSummary.RefreshReport();
                    ReportParameter[] param = new ReportParameter[3];
                    param[0] = new ReportParameter("parNoData", "True");
                    param[1] = new ReportParameter("parReportDate", startDate.ToString("MM/dd/yyyy"));
                    param[2] = new ReportParameter("parReportEndDate", endDate.ToString("MM/dd/yyyy"));
                    //param[2] = new ReportParameter("parReportEndDate", endDate.ToString("MM/dd/yyyy"));
                    reportViewerTransactionSummary.LocalReport.SetParameters(param);

                    LocalReport localReport = reportViewerTransactionSummary.LocalReport;
                    ReportDataSource repDataSource = new ReportDataSource();
                    repDataSource.Name = "DataSource";
                    repDataSource.Value = _dtTransactionStatement;//dataSetTransactionSummary.PMGetTransactionSummaryReportData;

                    localReport.DataSources.Add(repDataSource);

                    this.reportViewerTransactionSummary.RefreshReport();
                    //return;

                }
                this.Cursor = currentCursor;
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
            //    this.reportViewerTransactionSummary.RefreshReport();

            //    LocalReport localReport = reportViewerTransactionSummary.LocalReport;
            //    //DataSetTransactionSummary dataSetTransactionSummaryNew = new DataSetTransactionSummary();
            //    ReportDataSource repDataSource = new ReportDataSource();
            //    repDataSource.Name = "DataSource";
            //    repDataSource.Value = _dtTransactionStatement;//dataSetTransactionSummary.PMGetTransactionSummaryReportData;
            //    DataTable dt1 = dataSetTransactionSummary.Tables[0];
            //    localReport.DataSources.Add(repDataSource);

            //    this.reportViewerTransactionSummary.RefreshReport();



            //    return;
            //}
            //string symbol = string.Empty;
            //double currentOrMarketPrice = 0;
            //if (!_liveFeedCacheInstance.IsDataManagerConnected())
            //{
            //    this.reportViewerTransactionSummary.RefreshReport();

            //    LocalReport localReport = reportViewerTransactionSummary.LocalReport;
            //    ReportDataSource repDataSource = new ReportDataSource();
            //    repDataSource.Name = "DataSource";
            //    repDataSource.Value = dataSetTransactionSummary.PMGetTransactionSummaryReportData;
            //    DataTable dt1 = dataSetTransactionSummary.Tables[0];
            //    localReport.DataSources.Add(repDataSource);

            //    this.reportViewerTransactionSummary.RefreshReport();
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

        private void UpdateReportAfterCurrencyLangSet()
        {
            try
            {
                //if (_passThrough.Equals(false))
                //{
                string symbol = string.Empty;
                string currencyLanguageName = string.Empty;
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

                    DataColumn colLogoPath = new DataColumn();
                    colLogoPath = (DataColumn)_dtTransactionStatement.Columns["LogoPath"];
                    colLogoPath.ReadOnly = false;

                    DataColumn colAggregateOption = new DataColumn();
                    colAggregateOption = (DataColumn)_dtTransactionStatement.Columns["AggregateOption"];
                    colAggregateOption.ReadOnly = false;

                    currencyLanguageName = GetCurrencyLanguageName(row["CurrencySymbol"].ToString());
                    row["LanguageName"] = currencyLanguageName;//"ja-JP"; //currencyLanguageName.ToString(); Commented on 21st May, 08.

                    //row["BaseCurrencyLanguageName"] = baseCompanyCurrencyLanguageName; //Commented on 21st May, 08.
                    row["BaseCurrencyLanguageName"] = _companyBaseCurrencySymbol;

                    symbol = row["Ticker"].ToString();
                    //if (_passThrough.Equals(false)/* && _3rdGroup.Equals(string.Empty) && _2ndGroup.Equals(string.Empty) && _1stGroup.Equals(string.Empty)*/)
                    //{
                    if (String.IsNullOrEmpty(symbol))
                    {
                        continue;
                    }
                    //row["TotalCost"] = 0;
                    //row["TotalCostInBaseCurrency"] = 0;
                    //row["ShortTermCapitalGains"] = 0;
                    //row["LongTermCapitalGains"] = 0;

                    if (_buttonGenerateClicked.Equals(true))
                    {
                        if (symbol.IndexOf(" ") > 0)
                        {
                            //futureMultiplier = float.Parse(CachedDataManager.GetInstance.GetContractMultiplierBySymbol(symbol.Substring(0, symbol.IndexOf(" "))).ToString());
                            //if (futureMultiplier > 1)
                            //{
                            //    row["TotalCost"] = (double.Parse(row["TotalCost"].ToString()) * futureMultiplier) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                            //    //row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString()) * futureMultiplier) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString())); //25th Jan
                            //    row["TotalCostInBaseCurrency"] = double.Parse(row["TotalCost"].ToString()) * double.Parse(row["ConversionFactor"].ToString());

                            //    //As per requirement for now shortterm gain is storing the values for both short term & long term gain. 21-05-2008
                            //    row["ShortTermCapitalGains"] = ((double.Parse(row["ShortTermCapitalGains"].ToString()) * futureMultiplier) - double.Parse(row["RealizedPNLExpenses"].ToString()));
                            //}
                            //else
                            //{
                            //    row["TotalCost"] = (double.Parse(row["TotalCost"].ToString())) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                            //    //row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString())) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString())); //25th Jan
                            //    row["TotalCostInBaseCurrency"] = double.Parse(row["TotalCost"].ToString()) * double.Parse(row["ConversionFactor"].ToString());

                            //    //As per requirement for now shortterm gain is storing the values for both short term & long term gain. 21-05-2008
                            //    row["ShortTermCapitalGains"] = (double.Parse(row["ShortTermCapitalGains"].ToString()) - double.Parse(row["RealizedPNLExpenses"].ToString()));
                            //}
                        }
                        else
                        {
                            row["TotalCost"] = (double.Parse(row["TotalCost"].ToString())) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString()));
                            //row["TotalCostInBaseCurrency"] = (double.Parse(row["TotalCostInBaseCurrency"].ToString())) + (double.Parse(row["Commission"].ToString()) + double.Parse(row["Fees"].ToString())); //25th Jan
                            row["TotalCostInBaseCurrency"] = double.Parse(row["TotalCost"].ToString()) * double.Parse(row["ConversionFactor"].ToString());

                            //DateTime dtSettleDate = DateTime.Parse(row["SettlementDate"].ToString());
                            //DateTime dtTradeDate = DateTime.Parse(row["TradeDate"].ToString());
                            ////DateTime day = DateTime.Parse(row["TradeDate"].ToString());
                            //TimeSpan finalDate = dtSettleDate.Subtract(dtTradeDate);

                            //As per requirement for now shortterm gain is storing the values for both short term & long term gain. 21-05-2008
                            row["ShortTermCapitalGains"] = (double.Parse(row["ShortTermCapitalGains"].ToString()) - double.Parse(row["RealizedPNLExpenses"].ToString()));

                        }
                    }
                    //}


                    if (!_4thGroup.Equals(string.Empty))
                    {
                        row["GroupParamaeter4"] = row[_4thGroup];
                    }
                    else
                    {
                        row["GroupParamaeter4"] = string.Empty;
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
                        //if (_1stGroup.Equals("AccountName"))
                        //{
                        //    _dtTransactionStatement.Columns["AccountName"].MaxLength = 0;
                        //    _dataSetTransactionSummaryUpdated.PMGetTransactionSummaryReportData.Columns["AccountName"].MaxLength = 0;
                        //}
                        //else
                        //{
                        //    _dtTransactionStatement.Columns["AccountName"].MaxLength = 50;
                        //    _dataSetTransactionSummaryUpdated.PMGetTransactionSummaryReportData.Columns["AccountName"].MaxLength = 50;
                        //}
                    }
                    else
                    {
                        row["GroupParamaeter1"] = string.Empty;
                    }
                    row["LogoPath"] = Application.StartupPath + "\\Reports\\Logo.jpg";
                    row["AggregateOption"] = _valueOptionAggregate;

                }

                if (this.dataSetTransactionSummary.Tables["PMGetTransactionSummaryReportData"] != null)
                {
                    this.dataSetTransactionSummary.Tables.Remove("PMGetTransactionSummaryReportData");
                }
                if (!this.dataSetTransactionSummary.Tables.Contains("Table1"))
                {
                    this.dataSetTransactionSummary.Tables.Add(_dtTransactionStatement);
                }

                //this.reportViewerTransactionSummary.RefreshReport();

                LocalReport localReport = reportViewerTransactionSummary.LocalReport;
                //localReport.
                ReportDataSource repDataSource = new ReportDataSource();
                repDataSource.Name = "DataSource";
                repDataSource.Value = _dataSetTransactionSummaryUpdated.PMGetTransactionSummaryReportData;
                //repDataSource.Value = dataSetTransactionSummary.Tables[0];
                localReport.DataSources.Add(repDataSource);



                this.reportViewerTransactionSummary.RefreshReport();
                _passThrough = true;
                _buttonGenerateClicked = false;

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
        //            this.dataSetTransactionSummary.Tables.Remove("PMGetTransactionSummaryReportData");
        //            this.dataSetTransactionSummary.Tables.Add(_dtTransactionStatement);

        //            this.reportViewerTransactionSummary.RefreshReport();

        //            LocalReport localReport = reportViewerTransactionSummary.LocalReport;
        //            ReportDataSource repDataSource = new ReportDataSource();
        //            repDataSource.Name = "DataSource";
        //            repDataSource.Value = _dataSetTransactionSummaryUpdated.PMGetTransactionSummaryReportData;
        //            localReport.DataSources.Add(repDataSource);

        //            this.reportViewerTransactionSummary.RefreshReport();
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
            MasterFund = 3
        }

        private string _1stGroup = string.Empty;
        private string _2ndGroup = string.Empty;
        private string _3rdGroup = string.Empty;
        private string _4thGroup = string.Empty;
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
                    //lstGroupParametersTemp = (EnumerationValueList)lstGroupFiltersFinal.Clone();
                    lstGroupParametersTemp = (EnumerationValueList)lstGroupFilters2nd.Clone();
                    lstGroupParametersTemp.Remove(enumVal);
                    cmb3rdGroup.DataSource = null;
                    cmb3rdGroup.DataSource = lstGroupParametersTemp;
                    Utils.UltraComboFilter(cmb3rdGroup, "DisplayText");


                    lstGroupFilters3rd = lstGroupParametersTemp;
                    //cmb4thGroup.DataSource = null;
                    if (_isMasterFundAssociationSaved.Equals(false))
                    {
                        //cmb4thGroup.DataSource = null;
                    }
                    else
                    {
                        cmb4thGroup.DataSource = lstGroupFilters2nd;
                    }

                    Utils.UltraComboFilter(cmb4thGroup, "DisplayText");

                    cmb3rdGroup.Value = int.MinValue;
                    cmb4thGroup.Value = int.MinValue;

                    cmb3rdGroup.Enabled = true;

                    //15th Jan
                    if (_passThrough.Equals(false))
                    {
                        if (_isMasterFundAssociationSaved.Equals(false))
                        {
                            cmb3rdGroup.Text = "PositionType";
                        }
                        else
                        {
                            cmb3rdGroup.Text = "Symbol";
                        }
                        //cmb3rdGroup.Text = "Symbol";
                    }
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
                        if (this.LoginUser != null) // On 15th Jan
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "Account":
                        //GroupTable(GROUPBY_FUND);
                        _2ndGroup = GROUPBY_FUND;
                        if (this.LoginUser != null) // On 15th Jan
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TICKER);
                        _2ndGroup = GROUPBY_TICKER;
                        if (this.LoginUser != null) // On 15th Jan
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _2ndGroup = GROUPBY_TYPE;
                        if (this.LoginUser != null) // On 15th Jan
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    default:
                        _2ndGroup = "";
                        if (this.LoginUser != null && _passThrough.Equals(true))
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
                    if (_isMasterFundAssociationSaved.Equals(false))
                    {
                        //cmb4thGroup.DataSource = null;
                    }
                    else
                    {
                        lstGroupParametersTemp = (EnumerationValueList)lstGroupFilters3rd.Clone();
                        lstGroupParametersTemp.Remove(enumVal);
                        cmb4thGroup.DataSource = null;
                        cmb4thGroup.DataSource = lstGroupParametersTemp;
                        Utils.UltraComboFilter(cmb4thGroup, "DisplayText");
                    }

                    cmb4thGroup.Value = int.MinValue;
                    cmb4thGroup.Enabled = true;

                    if (_passThrough.Equals(false))
                    {
                        if (_isMasterFundAssociationSaved.Equals(false))
                        {
                            //cmb4thGroup.Value = int.MinValue;
                        }
                        else
                        {
                            //cmb4thGroup.Text = "";
                            cmb4thGroup.Text = "PositionType";
                        }
                        //cmb4thGroup.Text = "PositionType";
                    }
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
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "Account":
                        //GroupTable(GROUPBY_TICKER);
                        _3rdGroup = GROUPBY_FUND;
                        if (this.LoginUser != null)
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TICKER);
                        _3rdGroup = GROUPBY_TICKER;
                        if (this.LoginUser != null)
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _3rdGroup = GROUPBY_TYPE;
                        if (this.LoginUser != null)
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    default:
                        _3rdGroup = "";
                        if (this.LoginUser != null && _passThrough.Equals(true))
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                }
            }
        }

        private void cmb4thGroup_ValueChanged(object sender, EventArgs e)
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
                        if (this.LoginUser != null)
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "Account":
                        //GroupTable(GROUPBY_TICKER);
                        _4thGroup = GROUPBY_FUND;
                        if (this.LoginUser != null)
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TICKER);
                        _4thGroup = GROUPBY_TICKER;
                        if (this.LoginUser != null)
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _4thGroup = GROUPBY_TYPE;
                        if (this.LoginUser != null)
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    default:
                        _4thGroup = "";
                        if (this.LoginUser != null && _passThrough.Equals(true))
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

        private void reportViewerTransactionSummary_ReportRefresh(object sender, CancelEventArgs e)
        {
            _buttonGenerateClicked = true;
            this.reportViewerTransactionSummary.Clear();
            GenerateReports();
        }

        private void reportViewerTransactionSummary_ReportError(object sender, ReportErrorEventArgs e)
        {
            //
            e.Handled = true;
        }

        private void reportViewerTransactionSummary_ReportExport(object sender, ReportExportEventArgs e)
        {
            //
        }

        bool _optionAggregatge = true;
        private int _valueOptionAggregate = 1;
        private void btnAggregagte_Click(object sender, EventArgs e)
        {
            UpdateReportAggregateOption();
        }

        private void UpdateReportAggregateOption()
        {
            //Column basecurrency id is used as a check parameter for aggreagate.
            //TODO: remove dependency upon basecurrency parameter and to add some new parameter for this purpose only.
            DataColumn colBaseCurrencyID = new DataColumn();
            colBaseCurrencyID = (DataColumn)_dtTransactionStatement.Columns["BaseCurrencyID"];
            colBaseCurrencyID.ReadOnly = false;
            if (_optionAggregatge.Equals(true))
            {
                foreach (DataRow row in _dtTransactionStatement.Rows)
                {
                    row["AggregateOption"] = 0; //0 here means that the option aggreagte is used.
                }
                _valueOptionAggregate = 0;

                if (dataSetTransactionSummary.Tables.Contains("PMGetTransactionSummaryReportData"))
                {
                    this.dataSetTransactionSummary.Tables.Remove("PMGetTransactionSummaryReportData");
                }


                if (!this.dataSetTransactionSummary.Tables.Contains("Table1"))
                {
                    this.dataSetTransactionSummary.Tables.Add(_dtTransactionStatement);
                }

                LocalReport localReport = reportViewerTransactionSummary.LocalReport;
                ReportDataSource repDataSource = new ReportDataSource();
                repDataSource.Name = "DataSource";
                repDataSource.Value = _dataSetTransactionSummaryUpdated.PMGetTransactionSummaryReportData;
                localReport.DataSources.Add(repDataSource);

                this.reportViewerTransactionSummary.RefreshReport();

                btnAggregagte.Text = "Show Details";
                _optionAggregatge = false;
            }
            else
            {
                //Back to the normal view without aggregate.
                foreach (DataRow row in _dtTransactionStatement.Rows)
                {
                    row["AggregateOption"] = 1; //1 here means that the option aggreagte is not used.
                }
                _valueOptionAggregate = 1;

                if (dataSetTransactionSummary.Tables.Contains("PMGetTransactionSummaryReportData"))
                {
                    this.dataSetTransactionSummary.Tables.Remove("PMGetTransactionSummaryReportData");
                }


                if (!this.dataSetTransactionSummary.Tables.Contains("Table1"))
                {
                    this.dataSetTransactionSummary.Tables.Add(_dtTransactionStatement);
                }

                LocalReport localReport = reportViewerTransactionSummary.LocalReport;
                ReportDataSource repDataSource = new ReportDataSource();
                repDataSource.Name = "DataSource";
                repDataSource.Value = _dataSetTransactionSummaryUpdated.PMGetTransactionSummaryReportData;
                localReport.DataSources.Add(repDataSource);

                this.reportViewerTransactionSummary.RefreshReport();

                btnAggregagte.Text = "Hide Details";
                _optionAggregatge = true;
            }

        }

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
                    Utils.UltraComboFilter(cmb2ndGroup, "DisplayText");

                    lstGroupFilters2nd = lstGroupParametersTemp;
                    cmb3rdGroup.DataSource = null;
                    cmb3rdGroup.DataSource = lstGroupFilters2nd;
                    Utils.UltraComboFilter(cmb3rdGroup, "DisplayText");


                    lstGroupFilters2nd = lstGroupParametersTemp;
                    cmb4thGroup.DataSource = null;
                    cmb4thGroup.DataSource = lstGroupFilters2nd;
                    Utils.UltraComboFilter(cmb4thGroup, "DisplayText");

                    cmb2ndGroup.Value = int.MinValue;
                    cmb3rdGroup.Value = int.MinValue;
                    cmb4thGroup.Value = int.MinValue;

                    cmb2ndGroup.Enabled = true;
                    //cmb3rdGroup.Enabled = true;

                    //15th Jan
                    if (_passThrough.Equals(false))
                    {
                        if (_isMasterFundAssociationSaved.Equals(false))
                        {
                            cmb2ndGroup.Text = "Symbol";
                        }
                        else
                        {
                            cmb2ndGroup.Text = "Account";
                        }
                    }
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
                        if (this.LoginUser != null) // On 15th Jan
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "Account":
                        //GroupTable(GROUPBY_TICKER);
                        _1stGroup = GROUPBY_FUND;
                        if (this.LoginUser != null) // On 15th Jan
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "Symbol":
                        //GroupTable(GROUPBY_TYPE);
                        _1stGroup = GROUPBY_TICKER;
                        if (this.LoginUser != null) // On 15th Jan
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    case "PositionType":
                        //GroupTable(GROUPBY_TYPE);
                        _1stGroup = GROUPBY_TYPE;
                        if (this.LoginUser != null) // On 15th Jan
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                    default:
                        _1stGroup = "";
                        if (this.LoginUser != null && _passThrough.Equals(true))
                        {
                            UpdateReportAfterCurrencyLangSet();
                        }
                        break;
                }
            }
        }

    }
}