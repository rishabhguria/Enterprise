using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PostTradeServices
{
    class DataBaseProcessor
    {
        // COMMON VARIABLE
        static DataTable _dtPM = null;
        static List<int> _allAccounts = null;
        static DataTable _dtClosing = null;

        // VARIABLE FOR 'PM OPEN TAXLOT' TABLE
        static DataTable _dtOpen = null;
        static Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>> _symbolsLastStateInAccounts = new Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>>();

        // VARIABLE FOR 'MTM PNL' TABLE 
        static int _nextURPNLDate = -1;
        static DataTable _dtMTMPNL = null;
        static Dictionary<string, PositionDetail> _taxlotsURPNLLastState = new Dictionary<string, PositionDetail>();

        // VARIABLE FOR 'MONTHLY IRR' TABLE
        static DataTable _dtIRR = null;
        static int _nextSpecialDate = -1;
        static List<int> _specialDates = null;
        static Dictionary<int, Dictionary<int, double>> _dictTimeKeyAccountCashValue = new Dictionary<int, Dictionary<int, double>>();
        static Dictionary<int, List<DataRow>> _openPositionOnDateKey = new Dictionary<int, List<DataRow>>();
        static Dictionary<int, Dictionary<int, MTM>> _monthlyAccountWisePNL = new Dictionary<int, Dictionary<int, MTM>>();

        // PUBLIC FUNCTIONS
        public static void SetUp()
        {
            try
            {
                ClearAllDictionaries();
                MarkPriceHelper.CreateDictionary();
                HolidayHelper.CreateDictionary();
                CurrencyRateHelper.CreateDictionary();
                SMHelper.CreateDictionary();
                _dtClosing = DataBaseManager.GetPMClosingData();
                _dtPM = DataBaseManager.GetPMTaxlotsData();
                _allAccounts = DataBaseManager.GetAllComapanyAccounts();

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
        public static void ClearAllDictionaries()
        {
            _symbolsLastStateInAccounts.Clear();
            _taxlotsURPNLLastState.Clear();
            _monthlyAccountWisePNL.Clear();
            _openPositionOnDateKey.Clear();
            _dictTimeKeyAccountCashValue.Clear();
        }
        public static void MigrateDataFormTransactionDB()
        {
            try
            {
                DataBaseManager.MigrateDataFormTransactionDB();
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
        public static void ProcessPMTaxlotToPMOpenPostionSnapShot()
        {
            // COMMENTWED FOR SPECIAL DATE
            //_nextSpecialDate = -1;
            //SetNextSpecialDateIndex();
            //_symbolsLastStateInAccounts.Clear();
            _dtOpen = CreatePMOpenPostionTableSchema();
            CreatePMOpenPositionSnapShotData();
            DataBaseManager.SavePMOpenPositionSnapShot(_dtOpen);
        }
        public static void ProcessPMTaxlotClosingToMarkToMarketPNL()
        {
            try
            {
                //_taxlotsURPNLLastState.Clear();
                _dtMTMPNL = CreateMTMPNLTableSchema();
                CreateURPNLTableData();
                CreateRPNLTableData();
                DataBaseManager.SaveURPNLData(_dtMTMPNL);
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
        public static void ProcessPMTaxlotsToMonthlyIRR()
        {
            try
            {
                _specialDates = DataBaseManager.GetAllSpecialDatesOfCurrentYear();
                _nextSpecialDate = 0;
                //_monthlyAccountWisePNL.Clear();
                //_openPositionOnDateKey.Clear();
                //_dictTimeKeyAccountCashValue.Clear();
                CreateTimeKeyAccountCashDictionary();
                CreateOpenPositionOnDateKeyDictionary();
                CalculateMarketValueAndMonthURPNL();
                CalculateMonthlyRPNL();
                CalculateIRR();
                DataBaseManager.SaveIRRTableData(_dtIRR);
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

        // FUNCTIONS FOR 'MONTHLY IRR' TABLE
        private static void CreateTimeKeyAccountCashDictionary()
        {
            try
            {
                _dictTimeKeyAccountCashValue.Clear();
                DataTable dtCash = DataBaseManager.GetCashForSpecialDates();

                foreach (int spDate in _specialDates)
                {
                    Dictionary<int, double> accountsCash = new Dictionary<int, double>();
                    foreach (int accountID in _allAccounts)
                    {
                        accountsCash.Add(accountID, 0);
                    }
                    _dictTimeKeyAccountCashValue.Add(spDate, accountsCash);
                }

                foreach (DataRow row in dtCash.Rows)
                {
                    int timeKey = Convert.ToInt32(row["TimeKey"].ToString());
                    int accountID = Convert.ToInt32(row["FundID"].ToString());
                    double cashValue = Convert.ToDouble(row["CashValue"].ToString());

                    _dictTimeKeyAccountCashValue[timeKey][accountID] = cashValue;
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
        private static void CreateOpenPositionOnDateKeyDictionary()
        {
            try
            {
                if (_dtPM.Rows.Count > 0)
                {
                    _nextSpecialDate = 0;
                    Dictionary<string, DataRow> dictTaxlotsCurrentState = new Dictionary<string, DataRow>();

                    foreach (DataRow row in _dtPM.Rows)
                    {
                        if (_nextSpecialDate != -1)
                        {
                            int currentTimeKey = Convert.ToInt32(row["TimeKey"].ToString());
                            string taxlotID = row["TaxlotID"].ToString();

                            while (_nextSpecialDate != -1 && currentTimeKey > _specialDates[_nextSpecialDate])
                            {
                                CreateOpenSnapShotForDictionary(dictTaxlotsCurrentState, _specialDates[_nextSpecialDate]);
                                SetNextSpecialDateIndex();
                            }

                            if (!dictTaxlotsCurrentState.ContainsKey(taxlotID))
                            {
                                dictTaxlotsCurrentState.Add(taxlotID, row);
                            }
                            else
                            {
                                dictTaxlotsCurrentState[taxlotID] = row;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (_specialDates.Count > _nextSpecialDate)
                    {
                        int count = _specialDates.Count - _nextSpecialDate;
                        for (int i = 0; i < count; i++)
                        {
                            if (_nextSpecialDate != -1)
                            {
                                CreateOpenSnapShotForDictionary(dictTaxlotsCurrentState, _specialDates[_nextSpecialDate]);
                                SetNextSpecialDateIndex();
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
        private static void CreateOpenSnapShotForDictionary(Dictionary<string, DataRow> dictTaxlotsCurrentState, int timeKey)
        {
            try
            {
                List<DataRow> listPos = new List<DataRow>();
                _openPositionOnDateKey.Add(timeKey, listPos);

                foreach (KeyValuePair<string, DataRow> item in dictTaxlotsCurrentState)
                {
                    DataRow row = item.Value;
                    _openPositionOnDateKey[timeKey].Add(row);
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

        private static void CalculateMarketValueAndMonthURPNL()
        {
            try
            {
                foreach (KeyValuePair<int, List<DataRow>> item in _openPositionOnDateKey)
                {
                    int timeKey = item.Key;
                    List<DataRow> listPos = item.Value;

                    _monthlyAccountWisePNL.Add(timeKey, new Dictionary<int, MTM>());
                    foreach (int accountID in _allAccounts)
                    {
                        _monthlyAccountWisePNL[timeKey].Add(accountID, new MTM());
                    }

                    foreach (DataRow row in listPos)
                    {
                        string symbol = row["Symbol"].ToString();
                        string taxlotID = row["TaxlotID"].ToString();
                        int accountID = Convert.ToInt32(row["FundID"].ToString());
                        double avgPx = Convert.ToDouble(row["AvgPrice"].ToString());
                        int subAccountID = Convert.ToInt32(row["SubAccountID"].ToString());
                        int tradeDate = Convert.ToInt32(row["TradeDate"].ToString());
                        double commission = (row["Commission"].ToString() == String.Empty) ? 0 : Convert.ToDouble(row["Commission"].ToString());
                        double quantity = Convert.ToDouble(row["TaxlotOpenQty"].ToString());
                        quantity = (subAccountID == 2000) ? quantity : -quantity;

                        if (quantity != 0)
                        {
                            MTM mtm = _monthlyAccountWisePNL[timeKey][accountID];
                            double uRPNL = 0.0;
                            SMData smData = SMHelper.GetSMDataForTickerSymbol(symbol);
                            int lastMonthTimeKey = 0;
                            int lastMonthIndex = _specialDates.IndexOf(timeKey) - 1;
                            if (lastMonthIndex != -1)
                            {
                                lastMonthTimeKey = _specialDates[lastMonthIndex];
                            }

                            if (tradeDate == timeKey || tradeDate > lastMonthTimeKey)
                            {
                                double tradeFxRate = CurrencyRateHelper.GetTradeFxRate(taxlotID);
                                tradeFxRate = (tradeFxRate != 1.0) ? tradeFxRate : CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey);

                                uRPNL = quantity * smData.Multiplier * MarkPriceHelper.GetSymbolMarkPrice(timeKey, symbol) * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey)
                                       - quantity * smData.Multiplier * avgPx * tradeFxRate - commission * tradeFxRate;
                            }
                            else
                            {
                                uRPNL = quantity * smData.Multiplier * MarkPriceHelper.GetSymbolMarkPrice(timeKey, symbol) * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey)
                                      - quantity * smData.Multiplier * MarkPriceHelper.GetSymbolMarkPrice(lastMonthTimeKey, symbol) * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, lastMonthTimeKey);
                            }

                            mtm.Unrealized += uRPNL;
                            mtm.MarketValue += quantity * SMHelper.GetMultiplierForTickerSymbol(symbol) * MarkPriceHelper.GetSymbolMarkPrice(timeKey, symbol) * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey);
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
        private static void CalculateMonthlyRPNL()
        {
            try
            {

                int startDate = 0;
                int endDate = 0;
                if (_specialDates.Count > 1)
                {
                    _nextSpecialDate = 0;
                    startDate = _specialDates[_nextSpecialDate];
                    endDate = _specialDates[_nextSpecialDate + 1];
                }
                else
                {
                    return;
                }

                foreach (DataRow row in _dtClosing.Rows)
                {
                    int timeKey = Convert.ToInt32(row["TimeKey"].ToString());
                    if (timeKey <= startDate)
                    {
                        continue; // Skip all closing trade before the first specail date
                    }
                    while (timeKey > endDate)
                    {
                        startDate = endDate;
                        SetNextSpecialDateIndex();
                        if ((_nextSpecialDate != -1) && ((_nextSpecialDate + 1) != _specialDates.Count))
                        {
                            endDate = _specialDates[_nextSpecialDate + 1];
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (timeKey > startDate)
                    {
                        string symbol = row["Symbol"].ToString();
                        string taxlotID = row["TaxlotID"].ToString();
                        int accountID = Convert.ToInt32(row["FundID"].ToString());
                        double closedQty = Convert.ToDouble(row["ClosedQty"].ToString());
                        double closingPx = Convert.ToDouble(row["ClosingPx"].ToString());
                        double avgPx = Convert.ToDouble(row["AvgPx"].ToString());
                        int subAccountID = Convert.ToInt32(row["SubAccountID"].ToString());
                        int tradeDate = Convert.ToInt32(row["TradeDate"].ToString());
                        double commission = (row["Commission"].ToString() == String.Empty) ? 0 : Convert.ToDouble(row["Commission"].ToString());
                        closedQty = (subAccountID == 2000) ? closedQty : closedQty * -1;

                        double rPNL = 0;
                        SMData smData = SMHelper.GetSMDataForTickerSymbol(symbol);
                        if (tradeDate == timeKey || tradeDate > startDate)
                        {
                            double tradeFxRate = CurrencyRateHelper.GetTradeFxRate(taxlotID);
                            tradeFxRate = (tradeFxRate != 1.0) ? tradeFxRate : CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey);

                            rPNL = closedQty * smData.Multiplier * closingPx * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey)
                                   - closedQty * smData.Multiplier * avgPx * tradeFxRate - commission * tradeFxRate;
                        }
                        else
                        {
                            rPNL = closedQty * smData.Multiplier * closingPx * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey)
                                   - closedQty * smData.Multiplier * MarkPriceHelper.GetSymbolMarkPrice(startDate, symbol) * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, startDate);
                        }

                        if (rPNL != 0)
                        {
                            if (!_monthlyAccountWisePNL[endDate].ContainsKey(accountID))
                            {
                                MTM mtm = new MTM();
                                mtm.Realized = rPNL;
                                _monthlyAccountWisePNL[endDate].Add(accountID, mtm);
                            }
                            else
                            {
                                _monthlyAccountWisePNL[endDate][accountID].Realized += rPNL;
                            }
                        }
                    }
                }
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
        private static void CalculateIRR()
        {
            try
            {
                _dtIRR = CreateIRRTableSchema();
                int timekey = _specialDates[0]; ;
                int lastTimeKey = 0;

                foreach (KeyValuePair<int, Dictionary<int, MTM>> itemAccountMTM in _monthlyAccountWisePNL)
                {
                    lastTimeKey = timekey;
                    timekey = itemAccountMTM.Key;
                    Dictionary<int, MTM> dictAccountCash = itemAccountMTM.Value;

                    if (timekey == _specialDates[0])
                    {
                        continue;
                    }

                    foreach (KeyValuePair<int, MTM> item in dictAccountCash)
                    {
                        int accountID = item.Key;
                        MTM mtm = item.Value;
                        MTM mtmLast = _monthlyAccountWisePNL[lastTimeKey][accountID];
                        double cashValue = _dictTimeKeyAccountCashValue[lastTimeKey][accountID];
                        double irr = 0.0;
                        if ((mtmLast.MarketValue + cashValue) != 0.0)
                        {
                            irr = (mtm.Realized + mtm.Unrealized) / (mtmLast.MarketValue + cashValue);
                        }

                        if (irr != 0)
                        {
                            DataRow newRow = _dtIRR.NewRow();
                            newRow["TimeKey"] = timekey;
                            newRow["FundID"] = accountID;
                            newRow["IRR"] = irr;
                            newRow["Realized"] = mtm.Realized;
                            newRow["UnRealized"] = mtm.Unrealized;
                            newRow["MarketValue"] = mtm.MarketValue;
                            newRow["CashValue"] = _dictTimeKeyAccountCashValue[timekey][accountID]; ;
                            _dtIRR.Rows.Add(newRow);
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
        private static DataTable CreateIRRTableSchema()
        {
            DataTable dt = new DataTable("IRR");

            dt.Columns.Add("TimeKey", typeof(Int32));
            dt.Columns.Add("FundID", typeof(Int32));
            dt.Columns.Add("IRR", typeof(Double));
            dt.Columns.Add("Realized", typeof(Double));
            dt.Columns.Add("UnRealized", typeof(Double));
            dt.Columns.Add("MarketValue", typeof(Double));
            dt.Columns.Add("CashValue", typeof(Double));

            return dt;
        }
        private static void SetNextSpecialDateIndex()
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


        // FUNCTIONS FOR 'MTM PNL' TABLE
        private static void CreateURPNLTableData()
        {
            try
            {
                if (_dtPM.Rows.Count > 0)
                {
                    List<DataRow> currentDateTaxlots = new List<DataRow>();
                    int timeKey = Convert.ToInt32(_dtPM.Rows[0]["TimeKey"].ToString());
                    _nextURPNLDate = timeKey;

                    foreach (DataRow row in _dtPM.Rows)
                    {
                        int currentTimeKey = Convert.ToInt32(row["TimeKey"].ToString());

                        int assetID = SMHelper.GetAssetIDForTickerSymbol(row["Symbol"].ToString());
                        AssetCategory asset = Mapper.GetBaseAssetCategory((AssetCategory)assetID);
                        if (asset != AssetCategory.Future)
                        {
                            continue;
                        }

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

                    // CREATE ROWS UPTO CURRENT DATES
                    int currentDate = DataBaseManager.GetTimeKeyForTheDate();
                    while (_nextURPNLDate <= currentDate)
                    {
                        GenerateURPNLTableRows(_nextURPNLDate, _taxlotsURPNLLastState);
                        _nextURPNLDate++;
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
        private static void CreateRPNLTableData()
        {
            try
            {
                foreach (DataRow row in _dtClosing.Rows)
                {
                    int assetID = SMHelper.GetAssetIDForTickerSymbol(row["Symbol"].ToString());
                    AssetCategory asset = Mapper.GetBaseAssetCategory((AssetCategory)assetID);
                    if (asset != AssetCategory.Future)
                    {
                        continue;
                    }

                    string taxlotID = row["TaxlotID"].ToString();
                    int timeKey = Convert.ToInt32(row["TimeKey"].ToString());
                    string symbol = row["Symbol"].ToString();
                    double closedQty = Convert.ToDouble(row["ClosedQty"].ToString());
                    double closingPx = Convert.ToDouble(row["ClosingPx"].ToString());
                    double avgPx = Convert.ToDouble(row["AvgPx"].ToString());
                    int subAccountID = Convert.ToInt32(row["SubAccountID"].ToString());
                    int tradeDate = Convert.ToInt32(row["TradeDate"].ToString());
                    double commission = (row["Commission"].ToString() == String.Empty) ? 0 : Convert.ToDouble(row["Commission"].ToString());
                    closedQty = (subAccountID == 2000) ? closedQty : closedQty * -1;

                    double rPNL = 0;
                    SMData smData = SMHelper.GetSMDataForTickerSymbol(symbol);
                    if (timeKey == tradeDate)
                    {
                        double tradeFxRate = CurrencyRateHelper.GetTradeFxRate(taxlotID);
                        tradeFxRate = (tradeFxRate != 1.0) ? tradeFxRate : CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey);

                        rPNL = closedQty * smData.Multiplier * closingPx * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey)
                               - closedQty * smData.Multiplier * avgPx * tradeFxRate - commission * tradeFxRate;
                    }
                    else
                    {
                        rPNL = closedQty * smData.Multiplier * closingPx * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey)
                              - closedQty * smData.Multiplier * MarkPriceHelper.GetSymbolMarkPrice(timeKey - 1, symbol) * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey - 1);
                    }

                    if (rPNL != 0)
                    {
                        DataRow newRow = _dtMTMPNL.NewRow();
                        newRow["TimeKey"] = timeKey;
                        newRow["TaxlotID"] = row["TaxlotID"].ToString(); ;
                        newRow["Symbol"] = row["Symbol"].ToString();
                        newRow["FundID"] = Convert.ToInt32(row["FundID"].ToString());
                        newRow["PNL"] = rPNL;
                        newRow["SubAccountID"] = subAccountID;
                        _dtMTMPNL.Rows.Add(newRow);
                    }
                }
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
        private static void ProcessCurrentDateTaxlotsURPNL(List<DataRow> currentDateTaxlots)
        {
            try
            {
                Dictionary<string, PositionDetail> taxlotsURPNLCurrentState = new Dictionary<string, PositionDetail>();

                int timeKey = 0;
                string taxlotID = String.Empty;
                string symbol = String.Empty;
                int accountID = 0;
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
                    accountID = Convert.ToInt32(row["FundID"].ToString());
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
                        posDetail.AccountID = accountID;
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        private static void UpdateCurrentURPNLStateFromLastURPNLState(Dictionary<string, PositionDetail> taxlotsURPNLCurrentState)
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
        private static void GenerateURPNLTableRows(int timeKey, Dictionary<string, PositionDetail> taxlotsURPNLState)
        {
            foreach (KeyValuePair<string, PositionDetail> item in taxlotsURPNLState)
            {
                string taxlotID = item.Key;
                PositionDetail posDetail = item.Value;

                int auecID = SMHelper.GetAUECIDForTickerSymbol(posDetail.Symbol);
                if (HolidayHelper.IsHoliday(auecID, timeKey))
                {
                    continue;
                }

                if (posDetail.Quantity != 0)
                {
                    double uRPNL = 0.0;
                    SMData smData = SMHelper.GetSMDataForTickerSymbol(posDetail.Symbol);

                    if (posDetail.TradeDate == timeKey && posDetail.Symbol != String.Empty)
                    {
                        double tradeFxRate = CurrencyRateHelper.GetTradeFxRate(taxlotID);
                        tradeFxRate = (tradeFxRate != 1.0) ? tradeFxRate : CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey);
                        uRPNL = posDetail.Quantity * smData.Multiplier * MarkPriceHelper.GetSymbolMarkPrice(timeKey, posDetail.Symbol) * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey)
                                - posDetail.Quantity * smData.Multiplier * posDetail.AvgPrice * tradeFxRate - posDetail.Commission * tradeFxRate;
                    }
                    else
                    {
                        uRPNL = posDetail.Quantity * smData.Multiplier * MarkPriceHelper.GetSymbolMarkPrice(timeKey, posDetail.Symbol) * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey)
                                - posDetail.Quantity * smData.Multiplier * MarkPriceHelper.GetSymbolMarkPrice(timeKey - 1, posDetail.Symbol) * CurrencyRateHelper.GetToBaseCurrencyConversionRate(smData, timeKey - 1);
                    }
                    if (uRPNL != 0)
                    {
                        DataRow newRow = _dtMTMPNL.NewRow();
                        newRow["TimeKey"] = timeKey;
                        newRow["TaxlotID"] = taxlotID;
                        newRow["Symbol"] = posDetail.Symbol;
                        newRow["FundID"] = posDetail.AccountID;
                        newRow["PNL"] = uRPNL;
                        newRow["SubAccountID"] = posDetail.SubAccountID;
                        _dtMTMPNL.Rows.Add(newRow);
                    }
                }
            }
        }
        private static DataTable CreateMTMPNLTableSchema()
        {
            DataTable dt = new DataTable("MTMPNL");

            dt.Columns.Add("TimeKey", typeof(Int32));
            dt.Columns.Add("TaxlotID", typeof(String));
            dt.Columns.Add("Symbol", typeof(String));
            dt.Columns.Add("FundID", typeof(Int32));
            dt.Columns.Add("PNL", typeof(Double));
            dt.Columns.Add("SubAccountID", typeof(Double));
            return dt;
        }

        // FUNCTIONS FOR 'PM OPEN TAXLOT' TABLE
        private static void CreatePMOpenPositionSnapShotData()
        {
            try
            {
                if (_dtPM.Rows.Count > 0)
                {
                    List<DataRow> currentDateTaxlots = new List<DataRow>();
                    int timeKey = Convert.ToInt32(_dtPM.Rows[0]["TimeKey"].ToString());

                    foreach (DataRow row in _dtPM.Rows)
                    {
                        #region Commented Code PM_Taxlots Additional Rows
                        //long currentPkID = Convert.ToInt64(row["Taxlot_PK"].ToString());
                        //if (_seedPMTaxlots <= currentPkID)
                        //{
                        //    _seedPMTaxlots = currentPkID+1;
                        //} 
                        #endregion
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
                    #region COMMENTED FOR SPECIAL DATES
                    //if (_specialDates.Count > _nextSpecialDate)
                    //{
                    //    int count = _specialDates.Count - _nextSpecialDate;
                    //    for (int i = 0; i < count; i++)
                    //    {
                    //        if (_nextSpecialDate != -1)
                    //        {
                    //            GeneratePMOpenPositionSnapShot(_symbolsLastStateInAccounts, _specialDates[_nextSpecialDate]);
                    //            SetNextSpecialDateIndex();
                    //        }
                    //    }
                    //} 
                    #endregion
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
        private static void ProcessCurrentDateTaxlots(List<DataRow> currentDateTaxlots)
        {
            // WE HAVE ALL TAXLOTS OF SAME DATE
            try
            {
                Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>> symbolsCurrentDateStateInAccounts = new Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>>();
                List<string> currentSymbols = new List<string>(); // List of Current Symbols needed for looping as if collection modified at teh runtime looping halts

                string symbol = String.Empty;
                string taxlotID = String.Empty;
                int timeKey = 0;
                int accountID = 0;
                double quantity = 0;
                double avgPrice = 0;
                int subAccountID = 0;

                #region FILL CURRENT STATE DICTIONARY FROM CURRENT ROW LIST
                foreach (DataRow row in currentDateTaxlots)
                {
                    symbol = row["Symbol"].ToString();
                    taxlotID = row["TaxlotID"].ToString();
                    timeKey = Convert.ToInt32(row["TimeKey"].ToString());
                    accountID = Convert.ToInt32(row["FundID"].ToString());
                    quantity = Convert.ToDouble(row["TaxlotOpenQty"].ToString());
                    avgPrice = Convert.ToDouble(row["AvgPrice"].ToString());
                    subAccountID = Convert.ToInt32(row["SubAccountID"].ToString());

                    quantity = (subAccountID == 2000) ? quantity : quantity * -1;
                    if (!currentSymbols.Contains(symbol))
                    {
                        currentSymbols.Add(symbol);
                    }


                    if (!symbolsCurrentDateStateInAccounts.ContainsKey(symbol))
                    {
                        Dictionary<int, Dictionary<string, PositionDetail>> symbolStateInAccountsDict = new Dictionary<int, Dictionary<string, PositionDetail>>();
                        foreach (int account in _allAccounts)
                        {
                            Dictionary<string, PositionDetail> taxlotsInAccountsDict = new Dictionary<string, PositionDetail>();
                            if (account == accountID)
                            {
                                PositionDetail posDetail = new PositionDetail();
                                posDetail.AvgPrice = avgPrice;
                                posDetail.Quantity = quantity;
                                posDetail.SubAccountID = subAccountID;
                                taxlotsInAccountsDict.Add(taxlotID, posDetail);
                            }
                            symbolStateInAccountsDict.Add(account, taxlotsInAccountsDict);
                        }
                        symbolsCurrentDateStateInAccounts.Add(symbol, symbolStateInAccountsDict);
                    }
                    else // If Symbol exists then add the taxlot to the appropriate FUND
                    {
                        Dictionary<string, PositionDetail> taxlotsInAccountsDict = symbolsCurrentDateStateInAccounts[symbol][accountID];

                        PositionDetail posDetail = new PositionDetail();
                        posDetail.AvgPrice = avgPrice;
                        posDetail.Quantity = quantity;
                        posDetail.SubAccountID = subAccountID;
                        taxlotsInAccountsDict.Add(taxlotID, posDetail);
                    }
                }
                #endregion

                #region COMMENTED FOR SPECIAL DATES
                //if ((_nextSpecialDate != -1) && (timeKey == _specialDates[_nextSpecialDate]))
                //{
                //    UpdateCurrentSymbolStatesFromLastStates(symbolsCurrentDateStateInAccounts, currentSymbols);
                //    GeneratePMOpenPositionSnapShot(_symbolsLastStateInAccounts, _specialDates[_nextSpecialDate]);
                //    SetNextSpecialDateIndex();
                //    return;
                //}
                //while ((_nextSpecialDate != -1) && (timeKey > _specialDates[_nextSpecialDate]))
                //{
                //    GeneratePMOpenPositionSnapShot(_symbolsLastStateInAccounts, _specialDates[_nextSpecialDate]);
                //    SetNextSpecialDateIndex();
                //}
                //if ((_nextSpecialDate != -1) && (timeKey == _specialDates[_nextSpecialDate]))
                //{
                //    UpdateCurrentSymbolStatesFromLastStates(symbolsCurrentDateStateInAccounts, currentSymbols);
                //    GeneratePMOpenPositionSnapShot(_symbolsLastStateInAccounts, _specialDates[_nextSpecialDate]);
                //    SetNextSpecialDateIndex();
                //    return;
                //} 
                #endregion

                UpdateCurrentSymbolStatesFromLastStates(symbolsCurrentDateStateInAccounts, currentSymbols);
                GeneratePMOpenPositionSnapShot(symbolsCurrentDateStateInAccounts, timeKey);
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
        private static void UpdateCurrentSymbolStatesFromLastStates(Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>> symbolsCurrentDateStateInAccounts, List<string> currentSymbols)
        {
            // UPDATE THIS SYMBOL STATE FROM LAST SYMBOL STATE
            foreach (string symbolListItem in currentSymbols)
            {
                if (_symbolsLastStateInAccounts.ContainsKey(symbolListItem))
                {
                    foreach (int accountListItem in _allAccounts)
                    {
                        if (_symbolsLastStateInAccounts[symbolListItem][accountListItem].Count > 0)
                        {
                            foreach (KeyValuePair<string, PositionDetail> item in _symbolsLastStateInAccounts[symbolListItem][accountListItem])
                            {
                                if (!symbolsCurrentDateStateInAccounts[symbolListItem][accountListItem].ContainsKey(item.Key))
                                {
                                    symbolsCurrentDateStateInAccounts[symbolListItem][accountListItem].Add(item.Key, item.Value);
                                }
                            }
                        }
                    }
                }
                // mark this current state of symbol as last state of symbol
                _symbolsLastStateInAccounts[symbolListItem] = symbolsCurrentDateStateInAccounts[symbolListItem];
            }
        }
        private static void GeneratePMOpenPositionSnapShot(Dictionary<string, Dictionary<int, Dictionary<string, PositionDetail>>> symbolsCurrentDateStateInAccounts, int timeKey)
        {
            // GENERATE PM OPEN POSTION SNAPSHOT 
            foreach (KeyValuePair<string, Dictionary<int, Dictionary<string, PositionDetail>>> symbolInAccounts in symbolsCurrentDateStateInAccounts)
            {
                foreach (KeyValuePair<int, Dictionary<string, PositionDetail>> accountState in symbolInAccounts.Value)
                {
                    if (accountState.Value.Count > 0)
                    {
                        PositionDetail longPositionDetail = null;
                        PositionDetail shortPositionDetail = null;
                        foreach (KeyValuePair<string, PositionDetail> accountTaxlots in accountState.Value)
                        {
                            PositionDetail pos = accountTaxlots.Value;

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
                            openRow["Symbol"] = symbolInAccounts.Key;
                            openRow["FundID"] = accountState.Key;
                            openRow["Quantity"] = longPositionDetail.Quantity;
                            openRow["SubAccountID"] = longPositionDetail.SubAccountID;
                            openRow["AvgPrice"] = longPositionDetail.AvgPrice;
                            _dtOpen.Rows.Add(openRow);
                        }
                        if (shortPositionDetail != null)
                        {
                            DataRow openRow = _dtOpen.NewRow();
                            openRow["TimeKey"] = timeKey;
                            openRow["Symbol"] = symbolInAccounts.Key;
                            openRow["FundID"] = accountState.Key;
                            openRow["Quantity"] = shortPositionDetail.Quantity;
                            openRow["SubAccountID"] = shortPositionDetail.SubAccountID;
                            openRow["AvgPrice"] = shortPositionDetail.AvgPrice;
                            _dtOpen.Rows.Add(openRow);
                        }
                    }
                }
            }
        }
        private static DataTable CreatePMOpenPostionTableSchema()
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

        #region Commented Code PM_Taxlots Additional Rows
        //static DataTable _dtPM = null;
        //static long _seedPMTaxlots = 0;
        //static Dictionary<string, DataRow> _PMTaxlotsLastState = new Dictionary<string, DataRow>();
        //public static void ProcessAdditionalPMTaxlots()
        //{
        //    _dtPM = _dt.Clone();
        //    _dtPM.TableName = "PMTaxlots";
        //    _PMTaxlotsLastState.Clear();
        //    _nextSpecialDate = -1;
        //    SetNextSpecialDateIndex();
        //    CreateAdditionalPMTaxlotsData();
        //    DataBaseManager.SaveAdditionalPMTaxlotsSnap(_dtPM);
        //}
        //private static void CreateAdditionalPMTaxlotsData()
        //{
        //    try
        //    {
        //        if (_dt.Rows.Count > 0)
        //        {
        //            //_seedPMTaxlots = Convert.ToInt64(_dt.Rows[_dt.Rows.Count - 1]["Taxlot_PK"].ToString()) + 1;

        //            List<DataRow> currentDateTaxlots = new List<DataRow>();
        //            int timeKey = Convert.ToInt32(_dt.Rows[0]["TimeKey"].ToString());

        //            foreach (DataRow row in _dt.Rows)
        //            {
        //                int currentTimeKey = Convert.ToInt32(row["TimeKey"].ToString());

        //                if (timeKey == currentTimeKey) // Fill List with Same Date Taxlots
        //                {
        //                    currentDateTaxlots.Add(row);
        //                }
        //                else // Clear and Refill the List for new Date
        //                {
        //                    ProcessCurrentDatePMTaxlots(currentDateTaxlots, timeKey);
        //                    timeKey = currentTimeKey;
        //                    currentDateTaxlots.Clear();
        //                    currentDateTaxlots.Add(row);
        //                }
        //            }

        //            ProcessCurrentDatePMTaxlots(currentDateTaxlots, timeKey);
        //            if (_specialDates.Count > _nextSpecialDate)
        //            {
        //                int count = _specialDates.Count - _nextSpecialDate;
        //                for (int i = 0; i < count; i++)
        //                {
        //                    if (_nextSpecialDate != -1)
        //                    {
        //                        GenerateAdditionalPMTaxlots(_specialDates[_nextSpecialDate]);
        //                        SetNextSpecialDateIndex();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //private static void ProcessCurrentDatePMTaxlots(List<DataRow> currentDateTaxlots, int timeKey)
        //{
        //    if ((_nextSpecialDate != -1) && (timeKey == _specialDates[_nextSpecialDate]))
        //    { // There are trades on special Date
        //        UpdateLastDatePMTaxlots(currentDateTaxlots);
        //        GenerateAdditionalPMTaxlots(_specialDates[_nextSpecialDate]);
        //        SetNextSpecialDateIndex();
        //        return;
        //    }
        //    while ((_nextSpecialDate != -1) && (timeKey > _specialDates[_nextSpecialDate]))
        //    {  // Holiday or No Trades on special Date
        //        GenerateAdditionalPMTaxlots(_specialDates[_nextSpecialDate]);
        //        SetNextSpecialDateIndex();
        //    }
        //    if ((_nextSpecialDate != -1) && (timeKey == _specialDates[_nextSpecialDate]))
        //    { // if loop makes timKey equals to special date
        //        UpdateLastDatePMTaxlots(currentDateTaxlots);
        //        GenerateAdditionalPMTaxlots(_specialDates[_nextSpecialDate]);
        //        SetNextSpecialDateIndex();
        //        return;
        //    }
        //    UpdateLastDatePMTaxlots(currentDateTaxlots); // Non-Special Date
        //}
        //private static void UpdateLastDatePMTaxlots(List<DataRow> currentDateTaxlots)
        //{
        //    string taxlotID = String.Empty;

        //    foreach (DataRow row in currentDateTaxlots)
        //    {
        //        taxlotID = row["TaxlotID"].ToString();

        //        if (!_PMTaxlotsLastState.ContainsKey(taxlotID))
        //        {
        //            _PMTaxlotsLastState.Add(taxlotID, row);
        //        }
        //        else
        //        {
        //            _PMTaxlotsLastState[taxlotID] = row;
        //        }
        //    }
        //}
        //private static void GenerateAdditionalPMTaxlots(int timeKey)
        //{
        //    try
        //    {
        //        foreach (KeyValuePair<string, DataRow> item in _PMTaxlotsLastState)
        //        {
        //            DataRow row = item.Value;
        //            DataRow newRow = _dtPM.Rows.Add(row.ItemArray);
        //            newRow["Taxlot_PK"] = _seedPMTaxlots++;
        //            newRow["TimeKey"] = timeKey;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //} 
        #endregion
    }

    class PositionDetail
    {
        private string symbol = String.Empty;
        private double _quantity = 0.0;
        private double _avgPrice = 0.0;
        private int _subAccountID = 0;
        private int _tradeDate = 0;
        private int _accountID = 0;
        private double _commission = 0;

        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }
        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public int SubAccountID
        {
            get { return _subAccountID; }
            set { _subAccountID = value; }
        }
        public int TradeDate
        {
            get { return _tradeDate; }
            set { _tradeDate = value; }
        }
        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
        public double Commission
        {
            get { return _commission; }
            set { _commission = value; }
        }
    }
    class MTM
    {
        private double _unrealized = 0;
        private double _realized = 0;
        private double _marketValue = 0;

        public double Unrealized
        {
            get { return _unrealized; }
            set { _unrealized = value; }
        }
        public double Realized
        {
            get { return _realized; }
            set { _realized = value; }
        }
        public double MarketValue
        {
            get { return _marketValue; }
            set { _marketValue = value; }
        }
    }
}
