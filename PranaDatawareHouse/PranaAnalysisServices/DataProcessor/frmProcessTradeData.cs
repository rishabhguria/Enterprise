using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace DataProcessor
{
    public partial class frmProcessTradeData : Form
    {
        List<int> _allFunds = null;
        List<int> _specialDates = null;
        int _nextSpecialDate = -1;
        long _seedPMTaxlots = 0;
        DataTable _dt = null;
        DataTable _dtOpen = null;
        DataTable _dtPM = null;
        DataTable _dtMTMPNL = null;
        Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>> _symbolsLastStateInFunds = new Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>>();
        Dictionary<string, DataRow> _PMTaxlotsLastState = new Dictionary<string, DataRow>();


        public frmProcessTradeData()
        {
            InitializeComponent();
        }

        private void btnGetPMData_Click(object sender, EventArgs e)
        {
            try
            {
                _dt = DataManager.GetPMTaxlotsData();
                _allFunds = DataManager.GetAllComapanyFunds();
                _specialDates = DataManager.GetAllSpecialDatesOfCurrentYear();
                _seedPMTaxlots = Convert.ToInt64(_dt.Rows[_dt.Rows.Count-1]["Taxlot_PK"].ToString())+1;
                SetNextSpecialDateIndex();
                BindGridData(_dt);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void btnProcesssPMTaxlots_Click(object sender, EventArgs e)
        {
            try
            {
                _symbolsLastStateInFunds.Clear();
                CreatePMOpenPositionSnapShotData();
                BindGridData(_dtOpen);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void btnSaveOpenPosition_Click(object sender, EventArgs e)
        {
            try
            {
                DataManager.SavePMOpenPositionSnapShot(_dtOpen);
                MessageBox.Show("Saved to PMOpenTaxlotSnapShot Table !","Analysis", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CreatePMOpenPositionSnapShotData()
        {
            try
            {
                if (_dt.Rows.Count > 0)
                {
                    _dtOpen = CreatePMOpenPostionTable();
                    List<DataRow> currentDateTaxlots = new List<DataRow>();
                    int timeKey = Convert.ToInt32(_dt.Rows[0]["TimeKey"].ToString());

                    foreach (DataRow row in _dt.Rows)
                    {
                        int currentTimeKey = Convert.ToInt32(row["TimeKey"].ToString());

                        if (timeKey == currentTimeKey) // Fill List with Same Date Taxlots
                        {
                            currentDateTaxlots.Add(row);
                        }
                        else // Clear and Refill the List for new Date
                        {
                            timeKey = currentTimeKey;
                            ProcessCurrentDateTaxlots(currentDateTaxlots);
                            currentDateTaxlots.Clear();
                            currentDateTaxlots.Add(row);
                        }
                    }

                    ProcessCurrentDateTaxlots(currentDateTaxlots);
                    if (_specialDates.Count > _nextSpecialDate)
                    {
                        for (int i = 0; i < (_specialDates.Count - _nextSpecialDate); i++)
                        {
                            if (_nextSpecialDate != -1)
                            {
                                GeneratePMOpenPositionSnapShot(_symbolsLastStateInFunds, _specialDates[_nextSpecialDate]);
                                SetNextSpecialDateIndex();                            
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void ProcessCurrentDateTaxlots(List<DataRow> currentDateTaxlots)
        {
            // WE HAVE ALL TAXLOTS OF SAME DATE
            try
            {
                Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>> symbolsCurrentDateStateInFunds = new Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>>();
                List<string> currentSymbols = new List<string>(); // List of Current Symbols needed for looping as if collection modified at teh runtime looping halts

                string symbol = String.Empty;
                string taxlotID = String.Empty;
                int timeKey = 0;
                int fundID = 0;
                double quantity = 0;
                double avgPrice = 0;
                int subAccountID = 0;

                #region FILL CURRENT STATE DICTIONARY FROM CURRENT ROW LIST
                foreach (DataRow row in currentDateTaxlots)
                {
                    symbol = row["Symbol"].ToString();
                    taxlotID = row["TaxlotID"].ToString();
                    timeKey = Convert.ToInt32(row["TimeKey"].ToString());
                    fundID = Convert.ToInt32(row["FundID"].ToString());
                    quantity = Convert.ToDouble(row["TaxlotOpenQty"].ToString());
                    avgPrice = Convert.ToDouble(row["AvgPrice"].ToString());
                    subAccountID = Convert.ToInt32(row["SubAccountID"].ToString());

                    quantity = (subAccountID == 2000) ? quantity : quantity * -1;
                    if (!currentSymbols.Contains(symbol))
                    {
                        currentSymbols.Add(symbol);
                    }


                    if (!symbolsCurrentDateStateInFunds.ContainsKey(symbol))
                    {
                        Dictionary<int, Dictionary<string, PositionDetail>> symbolStateInFundsDict = new Dictionary<int, Dictionary<string, PositionDetail>>();
                        foreach (int fund in _allFunds)
                        {
                            Dictionary<string, PositionDetail> taxlotsInFundsDict = new Dictionary<string, PositionDetail>();
                            if (fund == fundID)
                            {
                                PositionDetail posDetail = new PositionDetail();
                                posDetail.AvgPrice = avgPrice;
                                posDetail.Quantity = quantity;
                                posDetail.SubAccountID = subAccountID;
                                taxlotsInFundsDict.Add(taxlotID,posDetail);
                            }
                            symbolStateInFundsDict.Add(fund, taxlotsInFundsDict);
                        }
                        symbolsCurrentDateStateInFunds.Add(symbol, symbolStateInFundsDict);
                    }
                    else // If Symbol exists then add the taxlot to the appropriate FUND
                    {
                        Dictionary<string, PositionDetail> taxlotsInFundsDict = symbolsCurrentDateStateInFunds[symbol][fundID];
                        
                        PositionDetail posDetail = new PositionDetail();
                        posDetail.AvgPrice = avgPrice;
                        posDetail.Quantity = quantity;
                        posDetail.SubAccountID = subAccountID;
                        taxlotsInFundsDict.Add(taxlotID, posDetail);
                    }
                }
                #endregion


                if ((_nextSpecialDate != -1) && (timeKey == _specialDates[_nextSpecialDate]))
                {
                    UpdateCurrentSymbolStatesFromLastStates(symbolsCurrentDateStateInFunds, currentSymbols);
                    GeneratePMOpenPositionSnapShot(_symbolsLastStateInFunds, _specialDates[_nextSpecialDate]);
                    SetNextSpecialDateIndex();
                    return;
                }
                if ((_nextSpecialDate != -1) && (timeKey > _specialDates[_nextSpecialDate]))
                {
                    GeneratePMOpenPositionSnapShot(_symbolsLastStateInFunds, _specialDates[_nextSpecialDate]);
                    SetNextSpecialDateIndex();
                }
                UpdateCurrentSymbolStatesFromLastStates(symbolsCurrentDateStateInFunds, currentSymbols);
                GeneratePMOpenPositionSnapShot(symbolsCurrentDateStateInFunds, timeKey);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }            
        }

        private void SetNextSpecialDateIndex()
        {
            if (_specialDates.Count > (_nextSpecialDate + 1))
            {
                _nextSpecialDate++;
            }
            else
            {
                _nextSpecialDate = -1;
            }
        }

        private void GeneratePMOpenPositionSnapShot(Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>> symbolsCurrentDateStateInFunds, int timeKey)
        {
            // GENERATE PM OPEN POSTION SNAPSHOT 
            foreach (KeyValuePair<string, Dictionary<int, Dictionary<string, PositionDetail>>> symbolInFunds in symbolsCurrentDateStateInFunds)
            {
                foreach (KeyValuePair<int, Dictionary<string, PositionDetail>> fundState in symbolInFunds.Value)
                {
                    if (fundState.Value.Count > 0)
                    {
                        PositionDetail longPositionDetail = null;
                        PositionDetail shortPositionDetail = null;
                        foreach (KeyValuePair<string, PositionDetail> fundTaxlots in fundState.Value)
                        {
                            PositionDetail pos = fundTaxlots.Value;

                            if (pos.SubAccountID == 2000)
                            {
                                if (longPositionDetail == null)
                                {
                                    longPositionDetail = new PositionDetail();
                                }
                                // Avg Price
                                if ((longPositionDetail.Quantity + pos.Quantity) != 0.0)
                                {
                                    longPositionDetail.AvgPrice = (longPositionDetail.AvgPrice * longPositionDetail.Quantity + pos.AvgPrice * pos.Quantity) / (longPositionDetail.Quantity + pos.Quantity);
                                }
                                else
                                {
                                    longPositionDetail.AvgPrice = 0.0;
                                }

                                // Quantity
                                longPositionDetail.Quantity += pos.Quantity;

                                // SubAccount ID
                                longPositionDetail.SubAccountID = 2000;
                            }
                            else
                            {
                                if (shortPositionDetail == null)
                                {
                                    shortPositionDetail = new PositionDetail();
                                }
                                // Avg Price
                                if ((shortPositionDetail.Quantity + pos.Quantity) != 0.0)
                                {
                                    shortPositionDetail.AvgPrice = (shortPositionDetail.AvgPrice * shortPositionDetail.Quantity + pos.AvgPrice * pos.Quantity) / (shortPositionDetail.Quantity + pos.Quantity);
                                }
                                else
                                {
                                    shortPositionDetail.AvgPrice = 0.0;
                                }

                                // Quantity
                                shortPositionDetail.Quantity += pos.Quantity;

                                // SubAccount ID
                                shortPositionDetail.SubAccountID = 2001;
                            }
                        }

                        if (longPositionDetail != null)
                        {
                            DataRow openRow = _dtOpen.NewRow();
                            openRow["TimeKey"] = timeKey;
                            openRow["Symbol"] = symbolInFunds.Key;
                            openRow["FundID"] = fundState.Key;
                            openRow["Quantity"] = longPositionDetail.Quantity;
                            openRow["SubAccountID"] = longPositionDetail.SubAccountID;
                            openRow["AvgPrice"] = longPositionDetail.AvgPrice;
                            _dtOpen.Rows.Add(openRow);
                        }
                        if (shortPositionDetail != null)
                        {
                            DataRow openRow = _dtOpen.NewRow();
                            openRow["TimeKey"] = timeKey;
                            openRow["Symbol"] = symbolInFunds.Key;
                            openRow["FundID"] = fundState.Key;
                            openRow["Quantity"] = shortPositionDetail.Quantity;
                            openRow["SubAccountID"] = shortPositionDetail.SubAccountID;
                            openRow["AvgPrice"] = shortPositionDetail.AvgPrice;
                            _dtOpen.Rows.Add(openRow);
                        }
                    }
                }
            }
        }
        private void UpdateCurrentSymbolStatesFromLastStates(Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>> symbolsCurrentDateStateInFunds, List<string> currentSymbols)
        {
            // UPDATE THIS SYMBOL STATE FROM LAST SYMBOL STATE
            foreach (string symbolListItem in currentSymbols)
            {
                if (_symbolsLastStateInFunds.ContainsKey(symbolListItem))
                {
                    foreach (int fundListItem in _allFunds)
                    {
                        if (_symbolsLastStateInFunds[symbolListItem][fundListItem].Count > 0)
                        {
                            foreach (KeyValuePair<string, PositionDetail> item in _symbolsLastStateInFunds[symbolListItem][fundListItem])
                            {
                                if (!symbolsCurrentDateStateInFunds[symbolListItem][fundListItem].ContainsKey(item.Key))
                                {
                                    symbolsCurrentDateStateInFunds[symbolListItem][fundListItem].Add(item.Key, item.Value);
                                }
                            }
                        }
                    }
                }

                // mark this current state of symbol as last state of symbol
                _symbolsLastStateInFunds[symbolListItem] = symbolsCurrentDateStateInFunds[symbolListItem];

            }
        }
        private DataTable CreatePMOpenPostionTable()
        {
            DataTable dt = new DataTable("OpenTaxlots");

            dt.Columns.Add("Symbol", typeof(String));
            dt.Columns.Add("FundID", typeof(Int32));
            dt.Columns.Add("Quantity", typeof(Double));
            dt.Columns.Add("TimeKey", typeof(Int32));
            dt.Columns.Add("SubAccountID", typeof(Int32));
            dt.Columns.Add("AvgPrice", typeof(Double));

            return dt;
        }
        private void BindGridData(DataTable dt)
        {
            grdData.DataSource = null;
            grdData.DataSource = dt;
        }

        private void btnSavePMTaxlots_Click(object sender, EventArgs e)
        {
            try
            {
                DataManager.SaveAdditionalPMTaxlots(_dtPM);
                MessageBox.Show("Saved to PM_Taxlots Table !", "Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnProcessPMTaxlotSnapShot_Click(object sender, EventArgs e)
        {
            _PMTaxlotsLastState.Clear();
            SetNextSpecialDateIndex();
            CreatePMTaxlotsData();
            BindGridData(_dtPM);
        }
        private void CreatePMTaxlotsData()
        {

            try
            {
                if (_dt.Rows.Count > 0)
                {
                    _dtPM = _dt.Clone();
                    _dtPM.TableName = "PMTaxlots";

                    List<DataRow> currentDateTaxlots = new List<DataRow>();
                    int timeKey = Convert.ToInt32(_dt.Rows[0]["TimeKey"].ToString());

                    foreach (DataRow row in _dt.Rows)
                    {
                        int currentTimeKey = Convert.ToInt32(row["TimeKey"].ToString());

                        if (timeKey == currentTimeKey) // Fill List with Same Date Taxlots
                        {
                            currentDateTaxlots.Add(row);
                        }
                        else // Clear and Refill the List for new Date
                        {
                            timeKey = currentTimeKey;
                            ProcessCurrentDatePMTaxlots(currentDateTaxlots, timeKey);
                            currentDateTaxlots.Clear();
                            currentDateTaxlots.Add(row);
                        }
                    }

                    ProcessCurrentDatePMTaxlots(currentDateTaxlots, timeKey);
                    if (_specialDates.Count > _nextSpecialDate)
                    {
                        for (int i = 0; i < (_specialDates.Count - _nextSpecialDate); i++)
                        {
                            if (_nextSpecialDate != -1)
                            {
                                GenerateAdditionalPMTaxlots(_specialDates[_nextSpecialDate]);
                                SetNextSpecialDateIndex();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }
        private void ProcessCurrentDatePMTaxlots(List<DataRow> currentDateTaxlots, int timeKey)
        {
            if ((_nextSpecialDate != -1) && (timeKey == _specialDates[_nextSpecialDate]))
            { // There are trades on special Date
                UpdateLastDatePMTaxlots(currentDateTaxlots);
                GenerateAdditionalPMTaxlots(_specialDates[_nextSpecialDate]);
                SetNextSpecialDateIndex();
                return;
            }
            if ((_nextSpecialDate != -1) && (timeKey > _specialDates[_nextSpecialDate]))
            {  // Holiday or No Trades on special Date
                GenerateAdditionalPMTaxlots(_specialDates[_nextSpecialDate]);
                SetNextSpecialDateIndex();
            }
            UpdateLastDatePMTaxlots(currentDateTaxlots); // Non-Special Date
        }
        private void UpdateLastDatePMTaxlots(List<DataRow> currentDateTaxlots)
        {
            string taxlotID = String.Empty;

            foreach (DataRow row in currentDateTaxlots)
            {
                taxlotID = row["TaxlotID"].ToString();

                if (!_PMTaxlotsLastState.ContainsKey(taxlotID))
                {
                    _PMTaxlotsLastState.Add(taxlotID, row);
                }
                else
                {
                    _PMTaxlotsLastState[taxlotID] = row;
                }
            }

        }
        private void GenerateAdditionalPMTaxlots(int timeKey)
        {
            try
            {
                foreach (KeyValuePair<string, DataRow> item in _PMTaxlotsLastState)
                {
                    DataRow row = item.Value;
                    DataRow newRow = _dtPM.Rows.Add(row.ItemArray);
                    newRow["Taxlot_PK"] = _seedPMTaxlots++;
                    newRow["TimeKey"] = timeKey;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }



        Dictionary<string, PositionDetail> _taxlotsURPNLLastState = new Dictionary<string, PositionDetail>();
        int _nextURPNLDate = -1;
        private void btnURPNLTable_Click(object sender, EventArgs e)
        {
            MarkPriceHelper.CreateDictionary(DataManager.GetMarkPriceData());
            _taxlotsURPNLLastState.Clear();
            CreateURPNLTableData();
            BindGridData(_dtMTMPNL);
        }
        private void CreateURPNLTableData()
        {
            try
            {
                if (_dt.Rows.Count > 0)
                {
                    _dtMTMPNL = CreateRPNLTableSchema();
                    List<DataRow> currentDateTaxlots = new List<DataRow>();
                    int timeKey = Convert.ToInt32(_dt.Rows[0]["TimeKey"].ToString());
                    _nextURPNLDate = timeKey;

                    foreach (DataRow row in _dt.Rows)
                    {
                        int currentTimeKey = Convert.ToInt32(row["TimeKey"].ToString());

                        if (timeKey == currentTimeKey) // Fill List with Same Date Taxlots
                        {
                            currentDateTaxlots.Add(row);
                        }
                        else // Clear and Refill the List for new Date
                        {
                            timeKey = currentTimeKey;
                            ProcessCurrentDateTaxlotsURPNL(currentDateTaxlots);
                            currentDateTaxlots.Clear();
                            currentDateTaxlots.Add(row);
                        }
                    }

                    ProcessCurrentDateTaxlotsURPNL(currentDateTaxlots);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void ProcessCurrentDateTaxlotsURPNL(List<DataRow> currentDateTaxlots)
        {
            try
            {
                Dictionary<string, PositionDetail> taxlotsURPNLCurrentState = new Dictionary<string, PositionDetail>();

                int timeKey = 0;
                string taxlotID = String.Empty;
                string symbol = String.Empty;
                int fundID = 0;
                double quantity = 0;
                double avgPrice = 0;
                int subAccountID = 0;
                int tradeDate = 0;
                double commission = 0;

                // PREPARE CURRENT DATE STATE
                foreach (DataRow row in currentDateTaxlots)
                {
                    taxlotID = row["TaxlotID"].ToString();
                    symbol = row["Symbol"].ToString();
                    fundID = Convert.ToInt32(row["FundID"].ToString());
                    timeKey = Convert.ToInt32(row["TimeKey"].ToString());
                    quantity = Convert.ToDouble(row["TaxlotOpenQty"].ToString());
                    avgPrice = Convert.ToDouble(row["AvgPrice"].ToString());
                    subAccountID = Convert.ToInt32(row["SubAccountID"].ToString());
                    tradeDate = Convert.ToInt32(row["TradeDate"].ToString());
                    commission = (row["Commission"].ToString() == String.Empty) ? 0 : Convert.ToDouble(row["Commission"].ToString());
                    quantity = (subAccountID == 2000) ? quantity : quantity * -1;

                    if (!taxlotsURPNLCurrentState.ContainsKey(taxlotID))
                    {
                        PositionDetail posDetail = new PositionDetail();
                        posDetail.Symbol = symbol;
                        posDetail.FundID = fundID;
                        posDetail.AvgPrice = avgPrice;
                        posDetail.Quantity = quantity;
                        posDetail.SubAccountID = subAccountID;
                        posDetail.TradeDate = tradeDate;
                        posDetail.Commission = commission;
                        taxlotsURPNLCurrentState.Add(taxlotID, posDetail);
                    }
                    else
                    {
                        PositionDetail posDetail = taxlotsURPNLCurrentState[taxlotID];
                        posDetail.Quantity = quantity;
                    }
                }

                // CREATE ROWS FOR INTERMEDIALE DATES
                while (_nextURPNLDate < timeKey)
                {
                    GenerateURPNLTableRows(_nextURPNLDate, _taxlotsURPNLLastState);
                    _nextURPNLDate++;
                }

                // NOW NEXTDATE IS EQUAL TO CURRENT DATE
                UpdateCurrentURPNLStateFromLastURPNLState(taxlotsURPNLCurrentState);
                GenerateURPNLTableRows(_nextURPNLDate, taxlotsURPNLCurrentState);
                _nextURPNLDate++;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void GenerateURPNLTableRows(int timeKey, Dictionary<string, PositionDetail> taxlotsURPNLState)
        {
            foreach (KeyValuePair<string, PositionDetail> item in taxlotsURPNLState)
            {
                string taxlotID = item.Key;
                PositionDetail posDetail = item.Value;

                if (posDetail.Quantity != 0)
                {
                    double uRPNL = 0.0;
                    if (posDetail.TradeDate == timeKey && posDetail.Symbol != String.Empty)
                    {
                        uRPNL = posDetail.Quantity * (MarkPriceHelper.GetSymbolMarkPrice(timeKey, posDetail.Symbol) - posDetail.AvgPrice) - posDetail.Commission;
                    }
                    else
                    {
                        uRPNL = posDetail.Quantity * (MarkPriceHelper.GetSymbolMarkPrice(timeKey, posDetail.Symbol) - MarkPriceHelper.GetSymbolMarkPrice(timeKey-1, posDetail.Symbol));
                    }

                    DataRow newRow = _dtMTMPNL.NewRow();
                    newRow["TimeKey"] = timeKey;
                    newRow["TaxlotID"] = taxlotID;
                    newRow["Symbol"] = posDetail.Symbol;
                    newRow["FundID"] = posDetail.FundID;
                    newRow["PNL"] = uRPNL;
                    _dtMTMPNL.Rows.Add(newRow);
                } 
            }
        }
        private void UpdateCurrentURPNLStateFromLastURPNLState(Dictionary<string, PositionDetail> taxlotsURPNLCurrentState)
        {
            foreach (KeyValuePair<string, PositionDetail> item in _taxlotsURPNLLastState)
            {
                string taxlotID = item.Key;
                PositionDetail posDetail = item.Value;

                if (!taxlotsURPNLCurrentState.ContainsKey(taxlotID) && posDetail.Quantity != 0)
                {
                    taxlotsURPNLCurrentState.Add(taxlotID, posDetail);
                }
            }
            _taxlotsURPNLLastState = taxlotsURPNLCurrentState;
        }
        private DataTable CreateRPNLTableSchema()
        {
            DataTable dt = new DataTable("MTMPNL");

            dt.Columns.Add("TimeKey", typeof(Int32));
            dt.Columns.Add("TaxlotID", typeof(String));
            dt.Columns.Add("Symbol", typeof(String));
            dt.Columns.Add("FundID", typeof(Int32));
            dt.Columns.Add("PNL", typeof(Double));            

            return dt;
        }


        DataTable _dtClosing = null;
        private void btnPMTaxlotClosing_Click(object sender, EventArgs e)
        {
            try
            {
                _dtClosing = DataManager.GetPMClosingData();
                BindGridData(_dtClosing);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void btnProcessClosingData_Click(object sender, EventArgs e)
        {
            try
            {
                CreateRPNLTableData();
                BindGridData(_dtMTMPNL);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void CreateRPNLTableData()
        {
            try
            {
                int timeKey = 0;
                string symbol = String.Empty;
                double closedQty = 0;
                double closingPx = 0;
                double avgPx = 0;
                int subAccountID = 0;
                int tradeDate = 0;
                double commission = 0;

                foreach (DataRow row in _dtClosing.Rows)
                {
                    timeKey = Convert.ToInt32(row["TimeKey"].ToString());
                    symbol = row["Symbol"].ToString();
                    closedQty = Convert.ToDouble(row["ClosedQty"].ToString());
                    closingPx = Convert.ToDouble(row["ClosingPx"].ToString());
                    avgPx = Convert.ToDouble(row["AvgPx"].ToString());
                    subAccountID = Convert.ToInt32(row["SubAccountID"].ToString());
                    tradeDate = Convert.ToInt32(row["TradeDate"].ToString());
                    commission = (row["Commission"].ToString() == String.Empty) ? 0 : Convert.ToDouble(row["Commission"].ToString());
                    closedQty = (subAccountID == 2000) ? closedQty : closedQty * -1;

                    double rPNL = 0;
                    if (timeKey == tradeDate)
                    {
                        rPNL = closedQty * (closingPx - avgPx) - commission;
                    }
                    else
                    {
                        rPNL = closedQty * (closingPx - MarkPriceHelper.GetSymbolMarkPrice(timeKey-1, symbol));
                    }

                    if (closedQty != 0)
                    {
                        DataRow newRow = _dtMTMPNL.NewRow();
                        newRow["TimeKey"] = timeKey;
                        newRow["TaxlotID"] = row["TaxlotID"].ToString(); ;
                        newRow["Symbol"] = symbol;
                        newRow["FundID"] = Convert.ToInt32(row["FundID"].ToString());
                        newRow["PNL"] = rPNL;
                        _dtMTMPNL.Rows.Add(newRow);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnSaveURPNL_Click(object sender, EventArgs e)
        {
            try
            {
                DataManager.SaveURPNLData(_dtMTMPNL);
                MessageBox.Show("Saved to URPNL !", "Analysis", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}