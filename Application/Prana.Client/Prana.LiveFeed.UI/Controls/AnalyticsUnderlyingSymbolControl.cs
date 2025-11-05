//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Text;
//using System.Windows.Forms;
//using Prana.LiveFeedProvider;
//using Prana.BusinessObjects;
//using Prana.Global;
//using Prana.Logging;
//using Prana.CommonDataCache;

//namespace Prana.LiveFeed.UI
//{
//    public class AnalyticsUnderlyingSymbolControl : UserControl
//    {
//        private int _companyUserID;

//        //CompanyUser _companyUser;
//        PricingDataSubscriber _pricingDataSubscriber;


//        public int CompanyUserID
//        {
//            get { return _companyUserID; }
//            set { _companyUserID = value; }
//        }
//        public string CurrentMonth
//        {
//            get { return _currentMonth; }
//            set { _currentMonth = value; }
//        }
//        public string UnderlyingSymbol
//        {
//            get { return _underlyingSymbol; }
//            set
//            {
//                _underlyingSymbol = value;
//                UpdateSymbolOnChart(_underlyingSymbol);
//            }
//        }
//        private List<string> RequestedSymbols
//        {
//            get { return _requestedSymbols; }
//            set { _requestedSymbols = value; }
//        }

//        #region Private Variables
//        Prana.BusinessObjects.LiveFeed.MonthEnum _monthEnum = new Prana.BusinessObjects.LiveFeed.MonthEnum();

//        Dictionary<int, List<int>> monthToExpirationDaysLeftDictForUnderlying = null;
//        Dictionary<string, Dictionary<string, VolatilityData>> StrikeVolatilityForParticularMonth = new Dictionary<string, Dictionary<string, VolatilityData>>();

//        private string _currentMonth;
//        private string _underlyingSymbol = "N.A.";
//        List<string> _requestedSymbols = new List<string>();
//        private DataTable _tblVolatility = new DataTable();
//        private DataTable _tblDividend = new DataTable();
//        private DataTable _tblInterest = new DataTable();
//        private DataTable _tblMonths = new DataTable();
//        BindingSource _viewVolatility = new BindingSource();
//        private DataTable _tblView = new DataTable();
//        #endregion


//        public AnalyticsUnderlyingSymbolControl(CompanyUser user, string underlyingSymbol)
//        {
//            InitializeComponent();
//            CompanyUserID = user.CompanyUserID;
//            UnderlyingSymbol = underlyingSymbol;
//            _pricingDataSubscriber = PricingDataSubscriber.GetInstance();
//            _pricingDataSubscriber.PricingDataSnapShotReceived += new PricingDataSnapShotHandler(_pricingDataSubscriber_PricingDataSnapShotReceived);
//            monthToExpirationDaysLeftDictForUnderlying = OptionDataCacheManager.GetMonthToExpirationDaysLeftDictForUnderlying(_underlyingSymbol);
//            InitControl();
//        }

//        private void InitControl()
//        {
//            _viewVolatility.DataSource = _tblVolatility;
//            SetDataGridColumns();            
//            PopulateDataTables();
//            SetDataSources();
//            WrapChartEvents();
//            PopulateComboView();

//        }

//        #region Initial Grids SetUps
//        /// <summary>
//        /// Populate view (Put, Call or Both) dropdown
//        /// </summary>
//        private void PopulateComboView()
//        {
//            DataColumn dcviewType = new DataColumn("viewType");
//            DataColumn dcviewName = new DataColumn("viewName");
//            _tblView.Columns.Add(dcviewType);
//            _tblView.Columns.Add(dcviewName);
//            DataRow viewRowCall = _tblView.NewRow();
//            viewRowCall[0] = "Call";
//            viewRowCall[1] = "Call";
//            _tblView.Rows.Add(viewRowCall);

//            DataRow viewRowPut = _tblView.NewRow();
//            viewRowPut[0] = "Put";
//            viewRowPut[1] = "Put";
//            _tblView.Rows.Add(viewRowPut);

//            DataRow viewRowBoth = _tblView.NewRow();
//            viewRowBoth[0] = "Both";
//            viewRowBoth[1] = "Both";
//            _tblView.Rows.Add(viewRowBoth);
//            cmbView.DataSource = null;
//            cmbView.DataSource = _tblView;
//            cmbView.ValueMember = "viewType";
//            cmbView.DisplayMember = "viewName";
//            cmbView.DisplayLayout.Bands[0].Columns["viewType"].Hidden = true;
//            cmbView.DataBind();
//            cmbView.SelectedRow = cmbView.Rows[0];
//        }

//        /// <summary>
//        /// Set Columns for the Grids
//        /// </summary>
//        private void SetDataGridColumns()
//        {
//            //Volatility Datagrid Columns
//            DataColumn dc00 = new DataColumn("SymbolCall", typeof(string), null, System.Data.MappingType.Element);
//            DataColumn dc01 = new DataColumn("User Call Vol.", typeof(double), null, System.Data.MappingType.Element);
//            DataColumn dc02 = new DataColumn("Implied Call Vol.", typeof(double), null, System.Data.MappingType.Element);
//            DataColumn dc03 = new DataColumn("Strike", typeof(double), null, System.Data.MappingType.Element);
//            DataColumn dc04 = new DataColumn("Implied Put Vol.", typeof(double), null, System.Data.MappingType.Element);
//            DataColumn dc05 = new DataColumn("User Put Vol.", typeof(double), null, System.Data.MappingType.Element);
//            DataColumn dc06 = new DataColumn("SymbolPut", typeof(string), null, System.Data.MappingType.Element);

//            _tblVolatility.Columns.Add(dc00);
//            _tblVolatility.Columns.Add(dc01);
//            _tblVolatility.Columns.Add(dc02);
//            _tblVolatility.Columns.Add(dc03);
//            _tblVolatility.Columns.Add(dc04);
//            _tblVolatility.Columns.Add(dc05);
//            _tblVolatility.Columns.Add(dc06);

//            //Dividend Datagrid Columns
//            DataColumn dc5 = new DataColumn("Ex-Dividend Rate", typeof(double), null, System.Data.MappingType.Element);
//            DataColumn dc6 = new DataColumn("Dividend Amount", typeof(double), null, System.Data.MappingType.Element);
//            _tblDividend.Columns.Add(dc5);
//            _tblDividend.Columns.Add(dc6);

//            //Interest Datagrid Columns
//            DataColumn dc7 = new DataColumn("Date", typeof(string), null, System.Data.MappingType.Element);
//            DataColumn dc8 = new DataColumn("Auto", typeof(double), null, System.Data.MappingType.Element);
//            DataColumn dc9 = new DataColumn("Manual", typeof(double), null, System.Data.MappingType.Element);
//            _tblInterest.Columns.Add(dc7);
//            _tblInterest.Columns.Add(dc8);
//            _tblInterest.Columns.Add(dc9);

//            //Expiration Months Datagrid Columns
//            DataColumn dc10 = new DataColumn("Symbol-Expiry", typeof(string), null, System.Data.MappingType.Element);
//            DataColumn dc11 = new DataColumn("Days to Expiration", typeof(string), null, System.Data.MappingType.Element);
//            _tblMonths.Columns.Add(dc10);
//            _tblMonths.Columns.Add(dc11);
//        }

//        #region Populate Individual Grids
//        private void PopulateDataTables()
//        {
//            try
//            {
//                PopulateDataGridMonths();
//                PopulateDataGridDividend();
//                PopulateDataGridInterest();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Error populating data in grids! Please Retry.", "Warning!");
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }
//        private void PopulateDataGridMonths()
//        {
//            try
//            {
//                _tblMonths.Rows.Clear();
//                if (monthToExpirationDaysLeftDictForUnderlying != null)
//                {
//                    foreach (KeyValuePair<int, List<int>> keyvalPair in monthToExpirationDaysLeftDictForUnderlying)
//                    {
//                        // for some options : QQQQ for eg. , we have two dates for expiration in a particular months
//                        foreach (int daystoExpiry in keyvalPair.Value)
//                        {
//                            DataRow row = _tblMonths.NewRow();
//                            row[0] = _underlyingSymbol + keyvalPair.Key.ToString();
//                            row[1] = daystoExpiry.ToString() + " days";
//                            _tblMonths.Rows.Add(row);
//                        }
//                    }
//                }
//            }
//            catch (Exception )
//            {
//                MessageBox.Show("Value not in correct format. Please reopen the form.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);

//            }
//        }
//        private void PopulateDataGridDividend()
//        {
//            _tblDividend.Rows.Clear();
//            for (int i = 0; i < 15; i++)
//            {
//                DataRow row = _tblDividend.NewRow();
//                row[0] = i;
//                row[1] = ((2 * i) + 1);
//                _tblDividend.Rows.Add(row);
//            }
//        }
//        private void PopulateDataGridInterest()
//        {
//            try
//            {
//                AnalyticsDataManager.GetInstance().GetInterestRates(CompanyUserID, _tblInterest);
//                if (monthToExpirationDaysLeftDictForUnderlying != null)
//                {
//                    foreach (KeyValuePair<int, List<int>> keyvalPair in monthToExpirationDaysLeftDictForUnderlying)
//                    {
//                        bool monthInterestRateExists = false;
//                        string month = keyvalPair.Key.ToString();
//                        for (int i = 0; i < _tblInterest.Rows.Count; i++)
//                        {
//                            if ((_tblInterest.Rows[i]["Date"].ToString() == month))
//                            {
//                                monthInterestRateExists = true;
//                                break;
//                            }
//                        }
//                        if (!monthInterestRateExists)
//                        {
//                            DataRow newrow = _tblInterest.NewRow();
//                            newrow["Date"] = month;
//                            //if the interest rate not already available, get it from IDC
//                            //newrow["Auto"] = GetInterestRate(month);
//                            newrow["Manual"] = 0.0;
//                            _tblInterest.Rows.Add(newrow);
//                        }
//                    }
//                }
//            }
//            catch (Exception )
//            {
//                MessageBox.Show("Value not in correct format. Please reopen the form.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);

//            }
//        }
//        private void PopulateGridVolatility(List<string> symbolForMonths, Dictionary<string, VolatilityData> volDataDict)
//        {
//            try
//            {
//                _tblVolatility.Rows.Clear();

//                // remove repeated symbols from vol
//                foreach (KeyValuePair<string, VolatilityData> data in volDataDict)
//                {
//                    bool symbolExists = false;

//                    for (int i = 0; i < _tblVolatility.Rows.Count; i++)
//                    {
//                        if (_tblVolatility.Rows[i]["SymbolCall"].ToString() == data.Key
//                            || _tblVolatility.Rows[i]["SymbolPut"].ToString() == data.Key)
//                        {
//                            symbolExists = true;
//                            break;
//                        }
//                    }

//                    if (!symbolExists)
//                    {

//                        DataRow row = _tblVolatility.NewRow();
//                        row["SymbolCall"] = data.Value.CallSymbol;
//                        row["SymbolPut"] = data.Value.PutSymbol;
//                        row["Strike"] = data.Value.StrikePrice;
//                        row["User Call Vol."] = data.Value.CallUserVol;
//                        row["User Put Vol."] = data.Value.PutUserVol;
//                        row["Implied Call Vol."] = data.Value.CallImpVol;
//                        row["Implied Put Vol."] = data.Value.PutImpVol;
//                        _tblVolatility.Rows.Add(row);
//                    }
//                }
//                _tblVolatility.AcceptChanges();
//                RefreshBindingSource();
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        //refresh UI on UI Thread
//        private void RefreshBindingSource()
//        {
//            try
//            {
//                MethodInvoker mi = new MethodInvoker(this.ResetBindings);
//                if (this.InvokeRequired)
//                    Invoke(mi);
//                else
//                    this.ResetBindings();
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }
//        private new void ResetBindings()
//        {
//            _viewVolatility.ResetBindings(false);
//        }
//        #endregion

//        private void SetDataSources()
//        {
//            try
//            {
//                dataGridViewMonths.DataSource = _tblMonths;
//                dataGridViewMonths.ColumnHeadersVisible = true;

//                dataGridViewMonths.Columns["Symbol-Expiry"].ReadOnly = true;
//                dataGridViewMonths.Columns["Days to Expiration"].ReadOnly = true;
//                //dataGridViewMonths.Columns["Interest Rate (%)"].ReadOnly = true;
//                dataGridViewMonths.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//                dataGridViewMonths.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//                //dataGridViewMonths.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);

//                dataGridViewVolatility.DataSource = _viewVolatility;

//                dataGridViewVolatility.Columns["SymbolCall"].ReadOnly = true;
//                dataGridViewVolatility.Columns["User Call Vol."].ReadOnly = true;
//                dataGridViewVolatility.Columns["Implied Call Vol."].ReadOnly = true;
//                dataGridViewVolatility.Columns["Strike"].ReadOnly = true;
//                dataGridViewVolatility.Columns["Implied Put Vol."].ReadOnly = true;
//                dataGridViewVolatility.Columns["User Put Vol."].ReadOnly = true;
//                dataGridViewVolatility.Columns["SymbolPut"].ReadOnly = true;

//                dataGridViewVolatility.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//                dataGridViewVolatility.ColumnHeadersVisible = true;

//                dataGridViewDividend.DataSource = _tblDividend;
//                dataGridViewDividend.ColumnHeadersVisible = true;
//                dataGridViewDividend.Columns["Ex-Dividend Rate"].ReadOnly = true;
//                dataGridViewDividend.Columns["Dividend Amount"].ReadOnly = true;
//                dataGridViewDividend.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

//                dataGridViewInterest.DataSource = _tblInterest;
//                dataGridViewInterest.ColumnHeadersVisible = true;
//                dataGridViewInterest.Columns["Date"].ReadOnly = true;
//                dataGridViewInterest.Columns["Auto"].ReadOnly = true;
//                dataGridViewInterest.Columns["Manual"].ReadOnly = true;
//                dataGridViewInterest.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

//                //dataGridViewInterest.Columns["Manual"].DefaultCellStyle.BackColor = System.Drawing.Color.White;
//                //dataGridViewInterest.Columns["Manual"].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        } 
//        #endregion

//        #region Chart Events
//        private void WrapChartEvents()
//        {
//            analyticsChart1.PointDoubleClicked += new EventHandler<AnalyticsChart.PointEventArgs>(analyticsChart1_PointDoubleClicked);
//        }
//        public event EventHandler<TradeParametersArgs> TradeClick = null;

//        void analyticsChart1_PointDoubleClicked(object sender, AnalyticsChart.PointEventArgs e)
//        {
//            Prana.BusinessObjects.LiveFeed.FullOptionData optionData = OptionDataCacheManager.GetOptSymbolData(e.SelectedSymbol);

//            TradeParametersArgs t = new TradeParametersArgs();
//            t.AssetId = Prana.BusinessObjects.AppConstants.AssetCategory.EquityOption;
//            t.UnderlyingId = Prana.BusinessObjects.AppConstants.Underlying.US;
//            t.Symbol = e.SelectedSymbol;
//            t.StrikePrice = e.PointStrike;

//            t.OrderSide = "Buy";
//            //t.OpenClose = "O";
//            t.UnderlyingSymbol = optionData.UnderlyingSymbol;
//            t.ExpirationMonth = int.Parse(CurrentMonth); //current month property stores the currently selected row month
//            t.CurrencyId = CachedDataManager.GetInstance.GetCurrencyID("USD");
//            int auecID = CachedDataManager.GetInstance.GetAUECIdByExchangeIdentifier(optionData.CExchangeListed +"-"+ Prana.BusinessObjects.AppConstants.AssetCategory.EquityOption.ToString());
//            if (auecID != int.MinValue)
//            {
//                t.AUECId = auecID;
//                t.ListedExchangeId = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(auecID);
//            }

//            if (e.SelectedSymbol == optionData.CSymbol)
//            {                
//                t.Price = optionData.CLast;
//                t.UnderlyingPutOrCall = OptionType.Call.ToString(); //FIXConstants.Underlying_Call;                
//            }
//            else
//            {
//                t.Price = optionData.PLast;
//                t.UnderlyingPutOrCall = OptionType.Put.ToString(); 
//            }

//            if (TradeClick != null)
//            {
//                TradeClick(this, t);
//            }
//        }

//        private void cmbView_ValueChanged(object sender, EventArgs e)
//        {
//            //view can be changed to call, put or both
//            switch (cmbView.Value.ToString())
//            {
//                case "Call":
//                    dataGridViewVolatility.Columns["SymbolCall"].Visible = true;
//                    dataGridViewVolatility.Columns["User Call Vol."].Visible = true;
//                    dataGridViewVolatility.Columns["Implied Call Vol."].Visible = true;
//                    dataGridViewVolatility.Columns["Implied Put Vol."].Visible = false;
//                    dataGridViewVolatility.Columns["User Put Vol."].Visible = false;
//                    dataGridViewVolatility.Columns["SymbolPut"].Visible = false;
//                    break;

//                case "Put":
//                    dataGridViewVolatility.Columns["SymbolCall"].Visible = false;
//                    dataGridViewVolatility.Columns["User Call Vol."].Visible = false;
//                    dataGridViewVolatility.Columns["Implied Call Vol."].Visible = false;
//                    dataGridViewVolatility.Columns["Implied Put Vol."].Visible = true;
//                    dataGridViewVolatility.Columns["User Put Vol."].Visible = true;
//                    dataGridViewVolatility.Columns["SymbolPut"].Visible = true;
//                    break;

//                case "Both":
//                    dataGridViewVolatility.Columns["SymbolCall"].Visible = true;
//                    dataGridViewVolatility.Columns["User Call Vol."].Visible = true;
//                    dataGridViewVolatility.Columns["Implied Call Vol."].Visible = true;
//                    dataGridViewVolatility.Columns["Implied Put Vol."].Visible = true;
//                    dataGridViewVolatility.Columns["User Put Vol."].Visible = true;
//                    dataGridViewVolatility.Columns["SymbolPut"].Visible = true;
//                    break;
//                default:
//                    break;
//            }

//        }

//        #endregion

//        #region GridEvents
//        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
//        {
//            MessageBox.Show("Please enter a valid decimal value");
//            //((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;

//        }

//        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
//        {
//            try
//            {
//                double d = Convert.ToDouble((((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()));//.ItemArray.GetValue(e.ColumnIndex));
//                //double d1 = Convert.ToDouble(_datagridVolatility.Rows[e.RowIndex].ItemArray.GetValue(e.ColumnIndex));
//                //dataGridViewVolatility.CommitEdit(DataGridViewDataErrorContexts.Commit);
//            }

//            catch (FormatException)
//            {
//                MessageBox.Show("Enter a valid decimal value");

//                //DataRow errorRow = _datagridVolatility.Rows[e.RowIndex];
//                //string errorstr = errorRow.GetColumnError(e.ColumnIndex);
//            }
//            catch (NullReferenceException )
//            {
//                MessageBox.Show("Enter a valid decimal value");
//            }
//            catch (InvalidCastException )
//            {
//                MessageBox.Show("Enter a valid decimal value");
//                //DataRow errorRow = _datagridVolatility.Rows[e.RowIndex];
//                //string errorstr = errorRow.GetColumnError(e.ColumnIndex);
//            }


//            catch (OverflowException )
//            {
//                MessageBox.Show("Value too big for a decimal");
//            }
//            catch (Exception )
//            {

//            }
//        }

//        private void dataGridViewMonths_RowEnter(object sender, DataGridViewCellEventArgs e)
//        {
//            try
//            {
//                dataGridViewMonths.Rows[e.RowIndex].Selected = true;
//                DataGridViewRow selectedRow = dataGridViewMonths.Rows[e.RowIndex];

//                string symbolExpiry;
//                if (selectedRow.Cells["Symbol-Expiry"].Value != null)
//                {
//                    symbolExpiry = selectedRow.Cells["Symbol-Expiry"].Value.ToString();

//                    string selectedMonth = symbolExpiry.Substring(symbolExpiry.Length - 6);
//                    int expirationMonthInteger;
//                    int.TryParse(selectedMonth, out expirationMonthInteger);
//                    CurrentMonth = expirationMonthInteger.ToString();

//                    string strremainingDays = selectedRow.Cells["Days to Expiration"].Value.ToString();
//                    int remainingDays;
//                    int.TryParse(strremainingDays.Substring(0, (strremainingDays.Length - 5)), out remainingDays);

//                    UpDateVolatilityData(expirationMonthInteger, remainingDays);
//                }
//            }
//            catch (NullReferenceException ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }
//        private void dataGridViewVolatility_DataError(object sender, DataGridViewDataErrorEventArgs e)
//        {
//            string errorMessage = "Index " + e.RowIndex + " does not have a value.";
//            if (e.Exception.Message == errorMessage)
//            {
//                //dataGridViewVolatility.Rows[e.RowIndex].Visible = false;
//                //_tblVolatility.Rows[e.RowIndex].Delete();
//            }
//        }
//        #endregion

//        #region Chk changes for AutoManual

//        private void rdbtnVolatility_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((System.Windows.Forms.RadioButton)grpbxVolatility.Controls[0]).Checked == true)
//            {
//                // if manual checked, readonly = false
//                dataGridViewVolatility.Columns["User Call Vol."].ReadOnly = false;
//                dataGridViewVolatility.Columns["User Put Vol."].ReadOnly = false;
//            }
//            else
//            {
//                dataGridViewVolatility.Columns["User Call Vol."].ReadOnly = true;
//                dataGridViewVolatility.Columns["User Put Vol."].ReadOnly = true;
//            }
//        }

//        void rdbtnDividend_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((System.Windows.Forms.RadioButton)grpbxDividend.Controls[0]).Checked == true)
//            {
//                // if manual checked, readonly = false
//                dataGridViewDividend.Columns["Ex-Dividend Rate"].ReadOnly = false;
//                dataGridViewDividend.Columns["Dividend Amount"].ReadOnly = false;

//            }
//            else
//            {
//                dataGridViewDividend.Columns["Ex-Dividend Rate"].ReadOnly = true;
//                dataGridViewDividend.Columns["Dividend Amount"].ReadOnly = true;
//            }
//        }

//        void rdbtnInterest_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((System.Windows.Forms.RadioButton)grpbxInterestRate.Controls[0]).Checked == false)
//            {
//                // if auto checked, readonly = true
//                dataGridViewInterest.Columns["Manual"].ReadOnly = true;
//            }
//            else
//            {
//                dataGridViewInterest.Columns["Manual"].ReadOnly = false;
//            }
//        }
//        #endregion

//        #region Form Events
//        /// <summary>
//        /// Refresh Chart with currently Populated data in volatility grid
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        void btnRefresh_Click(object sender, EventArgs e)
//        {
//            bool manual = false;
//            // if manual == true, set manual true
//            if (((System.Windows.Forms.RadioButton)grpbxVolatility.Controls[0]).Checked == true)
//            {
//                manual = true;
//            }
//            else // set manual false
//            {
//                manual = false;
//            }
//            analyticsChart1.SetData(_tblVolatility, manual);
//        }

//        #endregion


//        #region Private Methods
//        /// <summary>
//        /// converts numerical month to text month eg. 0102 to Jan02
//        /// </summary>
//        /// <param name="shortMonth"></param>
//        /// <returns></returns>
//        private string ConvertToLongMonthFormat(string shortMonth)
//        {
//            try
//            {
//                if (shortMonth.Length == 6)
//                {
//                    int integerMonth;
//                    int.TryParse(shortMonth.Substring(4, 2), out integerMonth);
//                    if (integerMonth != 0)
//                    {
//                        return (Enum.GetName(_monthEnum.GetType(), integerMonth) + shortMonth.Substring(2, 2));
//                    }
//                }
//                return string.Empty;
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
//                return string.Empty;
//            }
//        }

//        /// <summary>
//        /// Sends snapshot request to Pricing Server
//        /// </summary>
//        /// <param name="expirationMonthInteger"></param>
//        /// <param name="remainingDays"></param>
//        private void UpDateVolatilityData(int expirationMonthInteger, int remainingDays)
//        {
//            try
//            {
//                if (expirationMonthInteger != 0)
//                {
//                    List<string> symbolForMonths = OptionDataCacheManager.GetOptSymbolsForUnderlyingMonthRemainingDays(_underlyingSymbol, expirationMonthInteger, remainingDays);

//                    string monthName = ConvertToLongMonthFormat(expirationMonthInteger.ToString());
//                    string key = UnderlyingSymbol + monthName;


//                    bool _isDividendManual = false;
//                    if (((System.Windows.Forms.RadioButton)grpbxDividend.Controls[0]).Checked == true) // manual checked
//                    {
//                        _isDividendManual = true;
//                    }

//                    bool _isInterestRateManual = false;
//                    if (((System.Windows.Forms.RadioButton)grpbxInterestRate.Controls[0]).Checked == true)
//                    {
//                        _isInterestRateManual = true;
//                    }

//                    if (_pricingDataSubscriber != null)
//                    {
//                        _pricingDataSubscriber.RequestForPricingDataSnapShot(CreateInputParameterCollection(symbolForMonths, _isDividendManual, _isInterestRateManual, expirationMonthInteger));
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        /// <summary>
//        /// Creates InputParametersForGreeks for Pricing Server Snapshot and updates requested Symbol List
//        /// </summary>
//        /// <param name="symbolForMonths"></param>
//        /// <param name="isDividendManual"></param>
//        /// <param name="isInterestManual"></param>
//        /// <param name="expirationMonth"></param>
//        /// <returns></returns>
//        private InputParametersCollection CreateInputParameterCollection(List<string> symbolForMonths, bool isDividendManual, bool isInterestManual, int expirationMonth)
//        {
//            try
//            {
//                RequestedSymbols.Clear();
//                double closestInterestRate = 0;
//                if (isInterestManual)
//                {
//                    closestInterestRate = FindInterestRate(expirationMonth);
//                }
//                if (isDividendManual)
//                {
//                    //pick dividend information when available
//                }

//                InputParametersCollection inputParamList = new InputParametersCollection();
//                inputParamList.UserID = CompanyUserID.ToString();

//                foreach (string symbol in symbolForMonths)
//                {
//                    InputParametersForGreeks inputparam = new InputParametersForGreeks();

//                    inputparam.Auto = true;
//                    inputparam.Symbol = symbol;
//                    inputparam.UnderLysymbol = _underlyingSymbol;
//                    //Update currently requested symbol list
//                    RequestedSymbols.Add(symbol);
//                    if (closestInterestRate != double.MinValue && closestInterestRate != 0.0)
//                    {
//                        inputparam.InterestRate = closestInterestRate;
//                    }
//                    // no field to incorporate dividend information
//                    // add when available
//                    inputparam.PutOrCall = Convert.ToChar("p");
//                    inputParamList.Add(inputparam);
//                }
//                return inputParamList;
//            }
//            catch (Exception )
//            {
//                MessageBox.Show("Error creating greeks request.Please retry!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                throw;
//            }
//        }

//        /// <summary>
//        /// Returns Interest rate for requested month from DataGridViewVolatility
//        /// </summary>
//        /// <param name="expirationMonth"></param>
//        /// <returns></returns>
//        private double FindInterestRate(int expirationMonth)
//        {
//            double closestInterestRate = 0.0;
//            //try
//            //{                
//            for (int i = 0; i < dataGridViewInterest.Rows.Count; i++)
//            {
//                if (dataGridViewInterest.Rows[i].Cells[0].Value != null)
//                {

//                    if (dataGridViewInterest.Rows[i].Cells[0].Value.ToString() == expirationMonth.ToString())
//                    {
//                        if (((System.Windows.Forms.RadioButton)grpbxInterestRate.Controls[0]).Checked == false)
//                        {
//                            //i.e. not manual...pick auto values
//                            return double.Parse(dataGridViewInterest.Rows[i].Cells["Auto"].Value.ToString());
//                        }
//                        else
//                        {
//                            //pick manual values
//                            return double.Parse(dataGridViewInterest.Rows[i].Cells["Manual"].Value.ToString());
//                        }
//                    }
//                }
//            }
//            return closestInterestRate;
//            //}
//            //catch (Exception ex)
//            //{
//            //    bool rethrow = Logger.HandleException(ex, Common.POLICY_LOGONLY);


//            //}
//            //finally
//            //{

//            //}
//        }

//        /// <summary>
//        /// Refresh DataGridViewVolatility after response from Pricing Server has been updated in the DataTable
//        /// </summary>

//        private void RefreshGridOnUIThread()
//        {
//            try
//            {
//                //dataGridViewVolatility.DataSource = null;
//                MethodInvoker mi = new MethodInvoker(dataGridViewVolatility.Refresh);
//                if (this.InvokeRequired)
//                    Invoke(mi);
//                else
//                    dataGridViewVolatility.Refresh();
//            }
//            catch (Exception ex)
//            {
//                // Invoke our policy that is responsible for making sure no secure information
//                // gets out of our layer.
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }
//        #endregion


//        void _pricingDataSubscriber_PricingDataSnapShotReceived(Dictionary<string, OptionGreeks> pricingDataDict)
//        {
//            try
//            {
//                string tempSymbol = string.Empty;

//                Dictionary<string, VolatilityData> newVolData = new Dictionary<string, VolatilityData>();
//                foreach (KeyValuePair<string, OptionGreeks> keyValPair in pricingDataDict)
//                {
//                    if (RequestedSymbols.Contains(keyValPair.Value.Symbol))
//                    {
//                        if (keyValPair.Value.Symbol != string.Empty)
//                        {
//                            string incomingsymbol = keyValPair.Value.Symbol;
//                            Prana.BusinessObjects.LiveFeed.FullOptionData optionData = OptionDataCacheManager.GetOptSymbolData(incomingsymbol);

//                            // the following try catch block added since option data mayb null for some symbols
//                            // In this case we want the execution to continue for other elements of the loop 
//                            try
//                            {
//                                string strikeVolDataKey = UnderlyingSymbol + optionData.ExpirationMonth.ToString();
//                                tempSymbol = incomingsymbol;

//                                if (StrikeVolatilityForParticularMonth.ContainsKey(strikeVolDataKey))
//                                {
//                                    if (StrikeVolatilityForParticularMonth[strikeVolDataKey].ContainsKey(incomingsymbol))
//                                    {
//                                        if (incomingsymbol == optionData.CSymbol)
//                                        {
//                                            StrikeVolatilityForParticularMonth[strikeVolDataKey][incomingsymbol].CallImpVol = keyValPair.Value.ImpliedVol;

//                                        }
//                                        else if (incomingsymbol == optionData.PSymbol)
//                                        {
//                                            StrikeVolatilityForParticularMonth[strikeVolDataKey][incomingsymbol].PutImpVol = keyValPair.Value.ImpliedVol;
//                                        }
//                                    }
//                                    else
//                                    {
//                                        VolatilityData incomingData = new VolatilityData();
//                                        incomingData.CallSymbol = optionData.CSymbol;
//                                        incomingData.PutSymbol = optionData.PSymbol;
//                                        incomingData.StrikePrice = optionData.StrikePrice;
//                                        incomingData.CallUserVol = AnalyticsDataManager.GetInstance().GetUserVolatilityData(CompanyUserID, incomingData.CallSymbol);
//                                        incomingData.PutUserVol = AnalyticsDataManager.GetInstance().GetUserVolatilityData(CompanyUserID, incomingData.PutSymbol);
//                                        if (incomingsymbol == optionData.CSymbol)
//                                        {
//                                            incomingData.CallImpVol = keyValPair.Value.ImpliedVol;
//                                        }
//                                        else if (incomingsymbol == optionData.PSymbol)
//                                        {
//                                            incomingData.PutImpVol = keyValPair.Value.ImpliedVol;
//                                        }

//                                        StrikeVolatilityForParticularMonth[strikeVolDataKey].Add(keyValPair.Key, incomingData);
//                                    }
//                                }
//                                else
//                                {
//                                    if (!newVolData.ContainsKey(optionData.CSymbol))
//                                    {
//                                        VolatilityData incomingData = new VolatilityData();
//                                        incomingData.CallSymbol = optionData.CSymbol;
//                                        incomingData.PutSymbol = optionData.PSymbol;
//                                        incomingData.StrikePrice = optionData.StrikePrice;
//                                        double userVol = AnalyticsDataManager.GetInstance().GetUserVolatilityData(CompanyUserID, incomingData.CallSymbol);
//                                        if (incomingsymbol == optionData.CSymbol)
//                                        {
//                                            incomingData.CallImpVol = keyValPair.Value.ImpliedVol;
//                                            incomingData.CallUserVol = userVol;
//                                        }
//                                        else if (incomingsymbol == optionData.PSymbol)
//                                        {
//                                            incomingData.PutImpVol = keyValPair.Value.ImpliedVol;
//                                            incomingData.PutUserVol = userVol;
//                                        }
//                                        newVolData.Add(optionData.CSymbol, incomingData);
//                                        tempSymbol = optionData.CSymbol;
//                                    }
//                                    else
//                                    {
//                                        if (incomingsymbol == optionData.CSymbol)
//                                        {
//                                            newVolData[optionData.CSymbol].CallImpVol = keyValPair.Value.ImpliedVol;

//                                        }
//                                        else if (incomingsymbol == optionData.PSymbol)
//                                        {
//                                            newVolData[optionData.CSymbol].PutImpVol = keyValPair.Value.ImpliedVol;
//                                        }
//                                    }
//                                }
//                            }
//                            catch (NullReferenceException )
//                            {

//                            }                            
//                        }
//                    }
//                }

//                Prana.BusinessObjects.LiveFeed.FullOptionData optionDatatemp = OptionDataCacheManager.GetOptSymbolData(tempSymbol);
//                //CurrentMonth = optionDatatemp.SortingKey.Substring(0,6);
//                if (optionDatatemp != null)
//                {
//                    if (optionDatatemp.UnderlyingSymbol == this.UnderlyingSymbol)
//                    {
//                        string strikeVolDatatoUpdateKey = UnderlyingSymbol + optionDatatemp.ExpirationMonth.ToString();
//                        if (newVolData.Count > 0)
//                        {
//                            StrikeVolatilityForParticularMonth.Add(strikeVolDatatoUpdateKey, newVolData);
//                        }
//                        PopulateGridVolatility(RequestedSymbols, StrikeVolatilityForParticularMonth[strikeVolDatatoUpdateKey]);
//                        RefreshGridOnUIThread();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
//                if (rethrow)
//                {
//                    throw;
//                }
//            }
//        }

//        /// <summary>
//        /// Updates symbol on chart when symbol is replaced on Option Chain
//        /// </summary>
//        /// <param name="newSymbol"></param>
//        private void UpdateSymbolOnChart(string newSymbol)
//        {
//            analyticsChart1.UpdateSymbolOnChart(newSymbol);
//        }


//        #region Saving in DB

//        /// <summary>
//        /// Saves strike,volatilities for symbols and Interest Rates in DB
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void btnSaveLocally_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                AnalyticsDataManager.GetInstance().SaveInterestRates(CompanyUserID, _tblInterest);
//                AnalyticsDataManager.GetInstance().SaveSymbolVolatilityData(CompanyUserID, _tblVolatility, UnderlyingSymbol);

//            }
//            catch (Exception ex)
//            {
//                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

//                if (rethrow)
//                {
//                    throw;
//                }
//            }


//        }

//        private void btnSaveUniversally_Click(object sender, EventArgs e)
//        {
//            AnalyticsDataManager.GetInstance().SaveUniversalInterestRates(_tblInterest);
//            //AnalyticsDataManager.GetInstance().SaveSymbolVolatilityData(CompanyUserID, _tblVolatility, UnderlyingSymbol);
//        }
//        #endregion

//        #region Designer Code
//        /// <summary> 
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
//            this.rdbtnManualInterest = new System.Windows.Forms.RadioButton();
//            this.rdbtnAutoInterest = new System.Windows.Forms.RadioButton();
//            this.dataGridViewInterest = new System.Windows.Forms.DataGridView();
//            this.lblInterest = new Infragistics.Win.Misc.UltraLabel();
//            this.grpbxInterestRate = new System.Windows.Forms.GroupBox();
//            this.dataGridViewMonths = new System.Windows.Forms.DataGridView();
//            this.pnlInterest = new System.Windows.Forms.Panel();
//            this.pnlDividend = new System.Windows.Forms.Panel();
//            this.dataGridViewDividend = new System.Windows.Forms.DataGridView();
//            this.lblDividend = new Infragistics.Win.Misc.UltraLabel();
//            this.grpbxDividend = new System.Windows.Forms.GroupBox();
//            this.rdbtnManualDividend = new System.Windows.Forms.RadioButton();
//            this.rdbtnAutoDividend = new System.Windows.Forms.RadioButton();
//            this.pnlStrikePrice = new System.Windows.Forms.Panel();
//            this.cmbView = new Infragistics.Win.UltraWinGrid.UltraCombo();
//            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
//            this.dataGridViewVolatility = new System.Windows.Forms.DataGridView();
//            this.grpbxVolatility = new System.Windows.Forms.GroupBox();
//            this.rdbtnManualVolatility = new System.Windows.Forms.RadioButton();
//            this.rdbtnAutoVolatility = new System.Windows.Forms.RadioButton();
//            this.lblVolatility = new Infragistics.Win.Misc.UltraLabel();
//            this.btnSaveLocally = new System.Windows.Forms.Button();
//            this.btnSaveUniversally = new System.Windows.Forms.Button();
//            this.btnRefresh = new System.Windows.Forms.Button();
//            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
//            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
//            this.analyticsChart1 = new Prana.LiveFeed.UI.AnalyticsChart();
//            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInterest)).BeginInit();
//            this.grpbxInterestRate.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMonths)).BeginInit();
//            this.pnlInterest.SuspendLayout();
//            this.pnlDividend.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDividend)).BeginInit();
//            this.grpbxDividend.SuspendLayout();
//            this.pnlStrikePrice.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.cmbView)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVolatility)).BeginInit();
//            this.grpbxVolatility.SuspendLayout();
//            this.splitContainer1.Panel1.SuspendLayout();
//            this.splitContainer1.Panel2.SuspendLayout();
//            this.splitContainer1.SuspendLayout();
//            this.splitContainer2.Panel1.SuspendLayout();
//            this.splitContainer2.Panel2.SuspendLayout();
//            this.splitContainer2.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // rdbtnManualInterest
//            // 
//            this.rdbtnManualInterest.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
//            this.rdbtnManualInterest.Location = new System.Drawing.Point(69, 7);
//            this.rdbtnManualInterest.Name = "rdbtnManualInterest";
//            this.rdbtnManualInterest.Size = new System.Drawing.Size(60, 16);
//            this.rdbtnManualInterest.TabIndex = 23;
//            this.rdbtnManualInterest.Text = "Manual";
//            this.rdbtnManualInterest.CheckedChanged += new System.EventHandler(this.rdbtnInterest_CheckedChanged);
//            // 
//            // rdbtnAutoInterest
//            // 
//            this.rdbtnAutoInterest.Checked = true;
//            this.rdbtnAutoInterest.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
//            this.rdbtnAutoInterest.Location = new System.Drawing.Point(7, 7);
//            this.rdbtnAutoInterest.Name = "rdbtnAutoInterest";
//            this.rdbtnAutoInterest.Size = new System.Drawing.Size(56, 16);
//            this.rdbtnAutoInterest.TabIndex = 22;
//            this.rdbtnAutoInterest.TabStop = true;
//            this.rdbtnAutoInterest.Text = "Auto";
//            this.rdbtnAutoInterest.CheckedChanged += new System.EventHandler(this.rdbtnInterest_CheckedChanged);
//            // 
//            // dataGridViewInterest
//            // 
//            this.dataGridViewInterest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.dataGridViewInterest.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.dataGridViewInterest.Location = new System.Drawing.Point(0, 17);
//            this.dataGridViewInterest.Margin = new System.Windows.Forms.Padding(0);
//            this.dataGridViewInterest.Name = "dataGridViewInterest";
//            this.dataGridViewInterest.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
//            this.dataGridViewInterest.RowHeadersVisible = false;
//            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
//            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
//            this.dataGridViewInterest.RowsDefaultCellStyle = dataGridViewCellStyle1;
//            this.dataGridViewInterest.RowTemplate.Height = 18;
//            this.dataGridViewInterest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
//            this.dataGridViewInterest.Size = new System.Drawing.Size(162, 72);
//            this.dataGridViewInterest.TabIndex = 11;
//            this.dataGridViewInterest.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
//            this.dataGridViewInterest.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
//            // 
//            // lblInterest
//            // 
//            this.lblInterest.Dock = System.Windows.Forms.DockStyle.Top;
//            this.lblInterest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.lblInterest.Location = new System.Drawing.Point(0, 0);
//            this.lblInterest.Name = "lblInterest";
//            this.lblInterest.Size = new System.Drawing.Size(162, 17);
//            this.lblInterest.TabIndex = 9;
//            this.lblInterest.Text = "Interest Rate(%)";
//            // 
//            // grpbxInterestRate
//            // 
//            this.grpbxInterestRate.Controls.Add(this.rdbtnManualInterest);
//            this.grpbxInterestRate.Controls.Add(this.rdbtnAutoInterest);
//            this.grpbxInterestRate.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.grpbxInterestRate.Location = new System.Drawing.Point(0, 89);
//            this.grpbxInterestRate.Name = "grpbxInterestRate";
//            this.grpbxInterestRate.Size = new System.Drawing.Size(162, 25);
//            this.grpbxInterestRate.TabIndex = 6;
//            this.grpbxInterestRate.TabStop = false;
//            // 
//            // dataGridViewMonths
//            // 
//            this.dataGridViewMonths.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
//                        | System.Windows.Forms.AnchorStyles.Left)
//                        | System.Windows.Forms.AnchorStyles.Right)));
//            this.dataGridViewMonths.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.dataGridViewMonths.Location = new System.Drawing.Point(0, 0);
//            this.dataGridViewMonths.Margin = new System.Windows.Forms.Padding(0);
//            this.dataGridViewMonths.MultiSelect = false;
//            this.dataGridViewMonths.Name = "dataGridViewMonths";
//            this.dataGridViewMonths.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
//            this.dataGridViewMonths.RowHeadersVisible = false;
//            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
//            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
//            this.dataGridViewMonths.RowsDefaultCellStyle = dataGridViewCellStyle2;
//            this.dataGridViewMonths.RowTemplate.Height = 18;
//            this.dataGridViewMonths.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
//            this.dataGridViewMonths.Size = new System.Drawing.Size(544, 104);
//            this.dataGridViewMonths.TabIndex = 20;
//            this.dataGridViewMonths.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMonths_RowEnter);
//            // 
//            // pnlInterest
//            // 
//            this.pnlInterest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
//                        | System.Windows.Forms.AnchorStyles.Right)));
//            this.pnlInterest.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.pnlInterest.Controls.Add(this.dataGridViewInterest);
//            this.pnlInterest.Controls.Add(this.lblInterest);
//            this.pnlInterest.Controls.Add(this.grpbxInterestRate);
//            this.pnlInterest.Location = new System.Drawing.Point(0, 351);
//            this.pnlInterest.Name = "pnlInterest";
//            this.pnlInterest.Size = new System.Drawing.Size(166, 118);
//            this.pnlInterest.TabIndex = 17;
//            // 
//            // pnlDividend
//            // 
//            this.pnlDividend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
//                        | System.Windows.Forms.AnchorStyles.Right)));
//            this.pnlDividend.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.pnlDividend.Controls.Add(this.dataGridViewDividend);
//            this.pnlDividend.Controls.Add(this.lblDividend);
//            this.pnlDividend.Controls.Add(this.grpbxDividend);
//            this.pnlDividend.Location = new System.Drawing.Point(0, 177);
//            this.pnlDividend.Name = "pnlDividend";
//            this.pnlDividend.Size = new System.Drawing.Size(166, 174);
//            this.pnlDividend.TabIndex = 18;
//            // 
//            // dataGridViewDividend
//            // 
//            this.dataGridViewDividend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.dataGridViewDividend.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.dataGridViewDividend.Location = new System.Drawing.Point(0, 17);
//            this.dataGridViewDividend.Margin = new System.Windows.Forms.Padding(0);
//            this.dataGridViewDividend.Name = "dataGridViewDividend";
//            this.dataGridViewDividend.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
//            this.dataGridViewDividend.RowHeadersVisible = false;
//            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Black;
//            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
//            this.dataGridViewDividend.RowsDefaultCellStyle = dataGridViewCellStyle3;
//            this.dataGridViewDividend.RowTemplate.Height = 18;
//            this.dataGridViewDividend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
//            this.dataGridViewDividend.Size = new System.Drawing.Size(162, 128);
//            this.dataGridViewDividend.TabIndex = 10;
//            this.dataGridViewDividend.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
//            this.dataGridViewDividend.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
//            // 
//            // lblDividend
//            // 
//            this.lblDividend.Dock = System.Windows.Forms.DockStyle.Top;
//            this.lblDividend.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.lblDividend.Location = new System.Drawing.Point(0, 0);
//            this.lblDividend.Name = "lblDividend";
//            this.lblDividend.Size = new System.Drawing.Size(162, 17);
//            this.lblDividend.TabIndex = 10;
//            this.lblDividend.Text = "Dividend Schedule";
//            // 
//            // grpbxDividend
//            // 
//            this.grpbxDividend.Controls.Add(this.rdbtnManualDividend);
//            this.grpbxDividend.Controls.Add(this.rdbtnAutoDividend);
//            this.grpbxDividend.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.grpbxDividend.Location = new System.Drawing.Point(0, 145);
//            this.grpbxDividend.Name = "grpbxDividend";
//            this.grpbxDividend.Size = new System.Drawing.Size(162, 25);
//            this.grpbxDividend.TabIndex = 6;
//            this.grpbxDividend.TabStop = false;
//            // 
//            // rdbtnManualDividend
//            // 
//            this.rdbtnManualDividend.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
//            this.rdbtnManualDividend.Location = new System.Drawing.Point(69, 7);
//            this.rdbtnManualDividend.Name = "rdbtnManualDividend";
//            this.rdbtnManualDividend.Size = new System.Drawing.Size(60, 16);
//            this.rdbtnManualDividend.TabIndex = 23;
//            this.rdbtnManualDividend.Text = "Manual";
//            this.rdbtnManualDividend.CheckedChanged += new System.EventHandler(this.rdbtnDividend_CheckedChanged);
//            // 
//            // rdbtnAutoDividend
//            // 
//            this.rdbtnAutoDividend.Checked = true;
//            this.rdbtnAutoDividend.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
//            this.rdbtnAutoDividend.Location = new System.Drawing.Point(7, 7);
//            this.rdbtnAutoDividend.Name = "rdbtnAutoDividend";
//            this.rdbtnAutoDividend.Size = new System.Drawing.Size(56, 16);
//            this.rdbtnAutoDividend.TabIndex = 22;
//            this.rdbtnAutoDividend.TabStop = true;
//            this.rdbtnAutoDividend.Text = "Auto";
//            this.rdbtnAutoDividend.CheckedChanged += new System.EventHandler(this.rdbtnDividend_CheckedChanged);
//            // 
//            // pnlStrikePrice
//            // 
//            this.pnlStrikePrice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
//                        | System.Windows.Forms.AnchorStyles.Left)
//                        | System.Windows.Forms.AnchorStyles.Right)));
//            this.pnlStrikePrice.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.pnlStrikePrice.Controls.Add(this.cmbView);
//            this.pnlStrikePrice.Controls.Add(this.ultraLabel1);
//            this.pnlStrikePrice.Controls.Add(this.dataGridViewVolatility);
//            this.pnlStrikePrice.Controls.Add(this.grpbxVolatility);
//            this.pnlStrikePrice.Controls.Add(this.lblVolatility);
//            this.pnlStrikePrice.Location = new System.Drawing.Point(0, 0);
//            this.pnlStrikePrice.Name = "pnlStrikePrice";
//            this.pnlStrikePrice.Size = new System.Drawing.Size(166, 177);
//            this.pnlStrikePrice.TabIndex = 16;
//            // 
//            // cmbView
//            // 
//            this.cmbView.AutoEdit = false;
//            this.cmbView.AutoSize = false;
//            this.cmbView.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
//            appearance1.BackColor = System.Drawing.SystemColors.Window;
//            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
//            this.cmbView.DisplayLayout.Appearance = appearance1;
//            this.cmbView.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
//            this.cmbView.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
//            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
//            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
//            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
//            appearance2.BorderColor = System.Drawing.SystemColors.Window;
//            this.cmbView.DisplayLayout.GroupByBox.Appearance = appearance2;
//            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
//            this.cmbView.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
//            this.cmbView.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
//            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
//            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
//            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
//            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
//            this.cmbView.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
//            this.cmbView.DisplayLayout.MaxColScrollRegions = 1;
//            this.cmbView.DisplayLayout.MaxRowScrollRegions = 1;
//            appearance5.BackColor = System.Drawing.SystemColors.Window;
//            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
//            this.cmbView.DisplayLayout.Override.ActiveCellAppearance = appearance5;
//            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
//            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
//            this.cmbView.DisplayLayout.Override.ActiveRowAppearance = appearance6;
//            this.cmbView.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
//            this.cmbView.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
//            appearance7.BackColor = System.Drawing.SystemColors.Window;
//            this.cmbView.DisplayLayout.Override.CardAreaAppearance = appearance7;
//            appearance8.BorderColor = System.Drawing.Color.Silver;
//            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
//            this.cmbView.DisplayLayout.Override.CellAppearance = appearance8;
//            this.cmbView.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
//            this.cmbView.DisplayLayout.Override.CellPadding = 0;
//            appearance9.BackColor = System.Drawing.SystemColors.Control;
//            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
//            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
//            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
//            appearance9.BorderColor = System.Drawing.SystemColors.Window;
//            this.cmbView.DisplayLayout.Override.GroupByRowAppearance = appearance9;
//            appearance10.TextHAlign = Infragistics.Win.HAlign.Left;
//            this.cmbView.DisplayLayout.Override.HeaderAppearance = appearance10;
//            this.cmbView.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
//            this.cmbView.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
//            appearance11.BackColor = System.Drawing.SystemColors.Window;
//            appearance11.BorderColor = System.Drawing.Color.Silver;
//            this.cmbView.DisplayLayout.Override.RowAppearance = appearance11;
//            this.cmbView.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
//            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
//            this.cmbView.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
//            this.cmbView.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
//            this.cmbView.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
//            this.cmbView.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
//            this.cmbView.DisplayMember = "";
//            this.cmbView.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
//            this.cmbView.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
//            this.cmbView.Location = new System.Drawing.Point(50, 14);
//            this.cmbView.Name = "cmbView";
//            this.cmbView.Size = new System.Drawing.Size(67, 17);
//            this.cmbView.TabIndex = 24;
//            this.cmbView.ValueMember = "";
//            this.cmbView.ValueChanged += new System.EventHandler(this.cmbView_ValueChanged);
//            // 
//            // ultraLabel1
//            // 
//            this.ultraLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
//            this.ultraLabel1.Location = new System.Drawing.Point(10, 17);
//            this.ultraLabel1.Name = "ultraLabel1";
//            this.ultraLabel1.Size = new System.Drawing.Size(34, 12);
//            this.ultraLabel1.TabIndex = 11;
//            this.ultraLabel1.Text = "View";
//            // 
//            // dataGridViewVolatility
//            // 
//            this.dataGridViewVolatility.AllowUserToOrderColumns = true;
//            this.dataGridViewVolatility.AllowUserToResizeRows = false;
//            this.dataGridViewVolatility.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
//                        | System.Windows.Forms.AnchorStyles.Left)
//                        | System.Windows.Forms.AnchorStyles.Right)));
//            this.dataGridViewVolatility.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.dataGridViewVolatility.Location = new System.Drawing.Point(0, 34);
//            this.dataGridViewVolatility.Margin = new System.Windows.Forms.Padding(0);
//            this.dataGridViewVolatility.Name = "dataGridViewVolatility";
//            this.dataGridViewVolatility.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
//            this.dataGridViewVolatility.RowHeadersVisible = false;
//            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Black;
//            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
//            this.dataGridViewVolatility.RowsDefaultCellStyle = dataGridViewCellStyle4;
//            this.dataGridViewVolatility.RowTemplate.Height = 20;
//            this.dataGridViewVolatility.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
//            this.dataGridViewVolatility.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
//            this.dataGridViewVolatility.ShowCellErrors = false;
//            this.dataGridViewVolatility.Size = new System.Drawing.Size(162, 114);
//            this.dataGridViewVolatility.TabIndex = 9;
//            this.dataGridViewVolatility.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
//            this.dataGridViewVolatility.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridViewVolatility_DataError);
//            // 
//            // grpbxVolatility
//            // 
//            this.grpbxVolatility.Controls.Add(this.rdbtnManualVolatility);
//            this.grpbxVolatility.Controls.Add(this.rdbtnAutoVolatility);
//            this.grpbxVolatility.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.grpbxVolatility.Location = new System.Drawing.Point(0, 148);
//            this.grpbxVolatility.Name = "grpbxVolatility";
//            this.grpbxVolatility.Size = new System.Drawing.Size(162, 25);
//            this.grpbxVolatility.TabIndex = 10;
//            this.grpbxVolatility.TabStop = false;
//            // 
//            // rdbtnManualVolatility
//            // 
//            this.rdbtnManualVolatility.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
//            this.rdbtnManualVolatility.Location = new System.Drawing.Point(69, 7);
//            this.rdbtnManualVolatility.Name = "rdbtnManualVolatility";
//            this.rdbtnManualVolatility.Size = new System.Drawing.Size(60, 16);
//            this.rdbtnManualVolatility.TabIndex = 23;
//            this.rdbtnManualVolatility.Text = "Manual";
//            this.rdbtnManualVolatility.CheckedChanged += new System.EventHandler(this.rdbtnVolatility_CheckedChanged);
//            // 
//            // rdbtnAutoVolatility
//            // 
//            this.rdbtnAutoVolatility.Checked = true;
//            this.rdbtnAutoVolatility.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
//            this.rdbtnAutoVolatility.Location = new System.Drawing.Point(7, 7);
//            this.rdbtnAutoVolatility.Name = "rdbtnAutoVolatility";
//            this.rdbtnAutoVolatility.Size = new System.Drawing.Size(56, 16);
//            this.rdbtnAutoVolatility.TabIndex = 22;
//            this.rdbtnAutoVolatility.TabStop = true;
//            this.rdbtnAutoVolatility.Text = "Auto";
//            this.rdbtnAutoVolatility.CheckedChanged += new System.EventHandler(this.rdbtnVolatility_CheckedChanged);
//            // 
//            // lblVolatility
//            // 
//            this.lblVolatility.Dock = System.Windows.Forms.DockStyle.Top;
//            this.lblVolatility.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.lblVolatility.Location = new System.Drawing.Point(0, 0);
//            this.lblVolatility.Name = "lblVolatility";
//            this.lblVolatility.Size = new System.Drawing.Size(162, 12);
//            this.lblVolatility.TabIndex = 8;
//            this.lblVolatility.Text = "Volatility Function";
//            // 
//            // btnSaveLocally
//            // 
//            this.btnSaveLocally.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
//            this.btnSaveLocally.Location = new System.Drawing.Point(225, 107);
//            this.btnSaveLocally.Name = "btnSaveLocally";
//            this.btnSaveLocally.Size = new System.Drawing.Size(103, 23);
//            this.btnSaveLocally.TabIndex = 15;
//            this.btnSaveLocally.Text = "Save Locally";
//            this.btnSaveLocally.UseVisualStyleBackColor = true;
//            this.btnSaveLocally.Click += new System.EventHandler(this.btnSaveLocally_Click);
//            // 
//            // btnSaveUniversally
//            // 
//            this.btnSaveUniversally.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
//            this.btnSaveUniversally.Location = new System.Drawing.Point(334, 107);
//            this.btnSaveUniversally.Name = "btnSaveUniversally";
//            this.btnSaveUniversally.Size = new System.Drawing.Size(103, 23);
//            this.btnSaveUniversally.TabIndex = 14;
//            this.btnSaveUniversally.Text = "Save Universally";
//            this.btnSaveUniversally.UseVisualStyleBackColor = true;
//            this.btnSaveUniversally.Click += new System.EventHandler(this.btnSaveUniversally_Click);
//            // 
//            // btnRefresh
//            // 
//            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
//            this.btnRefresh.Location = new System.Drawing.Point(116, 107);
//            this.btnRefresh.Name = "btnRefresh";
//            this.btnRefresh.Size = new System.Drawing.Size(103, 23);
//            this.btnRefresh.TabIndex = 13;
//            this.btnRefresh.Text = "Refresh Chart";
//            this.btnRefresh.UseVisualStyleBackColor = true;
//            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
//            // 
//            // splitContainer1
//            // 
//            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
//            this.splitContainer1.Name = "splitContainer1";
//            // 
//            // splitContainer1.Panel1
//            // 
//            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
//            // 
//            // splitContainer1.Panel2
//            // 
//            this.splitContainer1.Panel2.Controls.Add(this.pnlDividend);
//            this.splitContainer1.Panel2.Controls.Add(this.pnlStrikePrice);
//            this.splitContainer1.Panel2.Controls.Add(this.pnlInterest);
//            this.splitContainer1.Size = new System.Drawing.Size(714, 469);
//            this.splitContainer1.SplitterDistance = 544;
//            this.splitContainer1.TabIndex = 21;
//            // 
//            // splitContainer2
//            // 
//            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
//            this.splitContainer2.Name = "splitContainer2";
//            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
//            // 
//            // splitContainer2.Panel1
//            // 
//            this.splitContainer2.Panel1.Controls.Add(this.analyticsChart1);
//            // 
//            // splitContainer2.Panel2
//            // 
//            this.splitContainer2.Panel2.Controls.Add(this.dataGridViewMonths);
//            this.splitContainer2.Panel2.Controls.Add(this.btnRefresh);
//            this.splitContainer2.Panel2.Controls.Add(this.btnSaveLocally);
//            this.splitContainer2.Panel2.Controls.Add(this.btnSaveUniversally);
//            this.splitContainer2.Size = new System.Drawing.Size(544, 469);
//            this.splitContainer2.SplitterDistance = 328;
//            this.splitContainer2.TabIndex = 21;
//            // 
//            // analyticsChart1
//            // 
//            this.analyticsChart1.AutoScroll = true;
//            this.analyticsChart1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.analyticsChart1.Location = new System.Drawing.Point(0, 0);
//            this.analyticsChart1.Name = "analyticsChart1";
//            this.analyticsChart1.Size = new System.Drawing.Size(544, 328);
//            this.analyticsChart1.TabIndex = 12;
//            // 
//            // AnalyticsUnderlyingSymbolControl
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.Controls.Add(this.splitContainer1);
//            this.Name = "AnalyticsUnderlyingSymbolControl";
//            this.Size = new System.Drawing.Size(714, 469);
//            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInterest)).EndInit();
//            this.grpbxInterestRate.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMonths)).EndInit();
//            this.pnlInterest.ResumeLayout(false);
//            this.pnlDividend.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDividend)).EndInit();
//            this.grpbxDividend.ResumeLayout(false);
//            this.pnlStrikePrice.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.cmbView)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVolatility)).EndInit();
//            this.grpbxVolatility.ResumeLayout(false);
//            this.splitContainer1.Panel1.ResumeLayout(false);
//            this.splitContainer1.Panel2.ResumeLayout(false);
//            this.splitContainer1.ResumeLayout(false);
//            this.splitContainer2.Panel1.ResumeLayout(false);
//            this.splitContainer2.Panel2.ResumeLayout(false);
//            this.splitContainer2.ResumeLayout(false);
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private System.Windows.Forms.RadioButton rdbtnManualInterest;
//        private System.Windows.Forms.RadioButton rdbtnAutoInterest;
//        private Infragistics.Win.Misc.UltraLabel lblInterest;
//        private System.Windows.Forms.GroupBox grpbxInterestRate;
//        private System.Windows.Forms.Panel pnlInterest;
//        private System.Windows.Forms.Panel pnlDividend;
//        private Infragistics.Win.Misc.UltraLabel lblDividend;
//        private System.Windows.Forms.GroupBox grpbxDividend;
//        private System.Windows.Forms.RadioButton rdbtnManualDividend;
//        private System.Windows.Forms.RadioButton rdbtnAutoDividend;
//        private System.Windows.Forms.Panel pnlStrikePrice;
//        private Infragistics.Win.Misc.UltraLabel lblVolatility;
//        private System.Windows.Forms.Button btnSaveLocally;
//        private System.Windows.Forms.Button btnSaveUniversally;
//        private System.Windows.Forms.Button btnRefresh;
//        private AnalyticsChart analyticsChart1;
//        private System.Windows.Forms.GroupBox grpbxVolatility;
//        private System.Windows.Forms.RadioButton rdbtnManualVolatility;
//        private System.Windows.Forms.RadioButton rdbtnAutoVolatility;
//        private System.Windows.Forms.SplitContainer splitContainer1;
//        private System.Windows.Forms.SplitContainer splitContainer2;
//        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
//        private Infragistics.Win.UltraWinGrid.UltraCombo cmbView;


//        private System.Windows.Forms.DataGridView dataGridViewDividend;
//        private System.Windows.Forms.DataGridView dataGridViewVolatility;
//        private System.Windows.Forms.DataGridView dataGridViewInterest;
//        private System.Windows.Forms.DataGridView dataGridViewMonths;
//        #endregion

//    }

//}