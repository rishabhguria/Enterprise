using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Prana.CashManagement
{
    public class CashHelperClass
    {
        #region Table columns
        private const string COLUMN_ACTIVITYTYPEID_FK = "ActivityTypeId_FK";
        private const string COLUMN_AMOUNTTYPEID_FK = "AmountTypeId_FK";
        private const string COLUMN_DEBITACCOUNT = "DebitAccount";
        private const string COLUMN_CREDITACCOUNT = "CreditAccount";
        private const string COLUMN_CASHVALUETYPE = "CashValueType";
        private const string COLUMN_ACTIVITYDATETYPE = "ActivityDateType";
        private const string COLUMN_STAMPDUTY = "StampDuty";
        private const string COLUMN_COMMISSION = "Commission";
        private const string COLUMN_PNL = "PnL";
        private const string COLUMN_FXPNL = "FXPnL";
        private const string COLUMN_OTHERBROKERFEES = "OtherBrokerFees";
        private const string COLUMN_TRANSACTIONLAVY = "TransactionLevy";
        private const string COLUMN_TAXONCOMMISSIONS = "TaxOnCommissions";
        private const string COLUMN_MISCFEES = "MiscFees";
        private const string COLUMN_OPTIONPREMIUMADJUSTMENT = "OptionPremiumAdjustment";
        private const string COLUMN_CLEARINGFEE = "ClearingFee";
        private const string COLUMN_AMOUNT = "Amount";
        private const string COLUMN_CLOSEDQTY = "ClosedQty";
        private const string COLUMN_SECFEE = "SecFee";
        private const string COLUMN_OCCFEE = "OccFee";
        private const string COLUMN_ORFFEE = "OrfFee";
        private const string COLUMN_CLEARINGBROKERFEE = "ClearingBrokerFee";
        private const string COLUMN_SOFTCOMMISSION = "SoftCommission";
        #endregion

        /// <summary>
        /// Assets the i ds for daily calculation.
        /// </summary>
        /// <param name="allaccounts">The allaccounts.</param>
        /// <param name="Conditions">The conditions.</param>
        /// <returns></returns>
        public static string AssetIDsForDailyCalculation(int[] allaccounts, out StringBuilder Conditions)
        {
            string _assetIDsForDailyCalculation = String.Empty;
            Conditions = new StringBuilder(String.Empty);
            try
            {
                List<int> accounts = new List<int>();
                if (CashDataManager.GetCashPreferences() != null)
                {
                    accounts = allaccounts.Where(f => CashDataManager.GetCashPreferences().Keys.Contains(f)).ToList();
                }

                bool IsCalculateBondAccurals = false;
                bool IsCalculatePnL = false;

                StringBuilder CommaSapratedAssetIDs = new StringBuilder();
                foreach (int accountID in accounts)
                {
                    if ((CashDataManager.GetCashPreferences()[accountID].IsCalculateBondAccurals == true || CashDataManager.GetCashPreferences()[accountID].IsCalculatePnL == true))
                    {
                        if (Conditions.Length > 0)
                            Conditions.Append(" or ");
                        else
                            Conditions.Append(" and (");

                        if (CashDataManager.GetCashPreferences()[accountID].IsCalculateBondAccurals == true && CashDataManager.GetCashPreferences()[accountID].IsCalculatePnL == true)
                        {
                            IsCalculateBondAccurals = true;
                            IsCalculatePnL = true;
                            Conditions.Append("(fundid =" + accountID + " and ( (AssetID=" + (int)AssetCategory.Future + ") Or (AssetID=" + (int)AssetCategory.FixedIncome + ")  Or (AssetID=" + (int)AssetCategory.ConvertibleBond + ") ))");
                        }
                        else if (CashDataManager.GetCashPreferences()[accountID].IsCalculateBondAccurals == true)
                        {
                            IsCalculateBondAccurals = true;
                            Conditions.Append("(fundid =" + accountID + " and ( (AssetID=" + (int)AssetCategory.FixedIncome + ")  Or (AssetID=" + (int)AssetCategory.ConvertibleBond + ") ))");
                        }

                        else if (CashDataManager.GetCashPreferences()[accountID].IsCalculatePnL == true)
                        {
                            IsCalculatePnL = true;
                            Conditions.Append(" (fundid =" + accountID + " and ( AssetID=" + (int)AssetCategory.Future + " )) ");
                        }
                    }
                }
                if (IsCalculateBondAccurals)
                {
                    CommaSapratedAssetIDs.Append(((int)AssetCategory.FixedIncome).ToString());
                    CommaSapratedAssetIDs.Append(",");
                    CommaSapratedAssetIDs.Append(((int)AssetCategory.ConvertibleBond).ToString());
                }
                if (IsCalculatePnL)
                {
                    CommaSapratedAssetIDs.Append(",");
                    CommaSapratedAssetIDs.Append(((int)AssetCategory.Future).ToString());
                }
                Conditions.Append(")");
                _assetIDsForDailyCalculation = CommaSapratedAssetIDs.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return _assetIDsForDailyCalculation;
        }

        /// <summary>
        /// Sets the state of the transactions taxlot.
        /// </summary>
        /// <param name="lsTransactions">The ls transactions.</param>
        /// <param name="taxlotState">State of the taxlot.</param>
        public static void SetTransactionsTaxlotState(List<Transaction> lsTransactions, ApplicationConstants.TaxLotState taxlotState)
        {
            try
            {
                if (lsTransactions != null)
                {
                    foreach (Transaction t in lsTransactions)
                    {
                        foreach (TransactionEntry data in t.TransactionEntries)
                        {
                            data.TaxLotState = taxlotState;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
        public static List<Transaction> GetTransactionsFromDSForMultileg(DataSet ds, CashTransactionType transactionSource, int userId)
        {
            List<Transaction> transactionList = new List<Transaction>();
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    bool isTaxlotIdExists = false; string taxlotID = string.Empty;
                    bool isSymbolExists = false; string symbol = string.Empty;
                    bool isDescriptionExists = false; string description = string.Empty;
                    bool isAccountExists = false; int accountID = 0;
                    bool isCurrencyIDExists = false;
                    bool isCurrencyExists = false; int currencyID = 0;
                    bool isCRExists = false; double CRAmount = 0.0;
                    bool isDRExists = false; double DRAmount = 0.0;
                    bool isCashSubAccountExists = false; string CashSubAccount = string.Empty;
                    bool isDateExists = false; DateTime transactionDate = DateTime.Now;
                    bool isFXRateExists = false; double FXRate = 0.0;
                    bool isFXConversionMethodOperatorExists = false; string FXConversionMethodOperator = "M";
                    bool isNonTradingTransactionExists = false; bool isThisNonTradeImport = false;

                    if (ds.Tables[0].Columns.Contains("TaxlotID"))
                        isTaxlotIdExists = true;
                    if (ds.Tables[0].Columns.Contains("FundID"))
                        isAccountExists = true;
                    if (ds.Tables[0].Columns.Contains("Symbol"))
                        isSymbolExists = true;
                    if (ds.Tables[0].Columns.Contains("Date"))
                        isDateExists = true;
                    if (ds.Tables[0].Columns.Contains("Description"))
                        isDescriptionExists = true;
                    if (ds.Tables[0].Columns.Contains("CurrencyID"))
                        isCurrencyIDExists = true;
                    if (ds.Tables[0].Columns.Contains("CurrencyName"))
                        isCurrencyExists = true;
                    if (ds.Tables[0].Columns.Contains("DR"))
                        isDRExists = true;
                    if (ds.Tables[0].Columns.Contains("CR"))
                        isCRExists = true;
                    if (ds.Tables[0].Columns.Contains("Cash-SubAccount"))
                        isCashSubAccountExists = true;
                    if (ds.Tables[0].Columns.Contains("FXRate"))
                        isFXRateExists = true;
                    if (ds.Tables[0].Columns.Contains("FXConversionMethodOperator"))
                        isFXConversionMethodOperatorExists = true;
                    if (ds.Tables[0].Columns.Contains("IsNonTradingTransaction"))
                        isNonTradingTransactionExists = true;

                    int EntryId = 0;
                    if (ds.Tables[0].Columns.Contains("EntryID"))
                    {
                        EntryId = Convert.ToInt32(ds.Tables[0].Rows[0]["EntryID"]);
                        if (isDateExists && ds.Tables[0].Rows[0]["Date"] != DBNull.Value)
                            transactionDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["Date"]);
                    }
                    Transaction newTransaction = new Transaction(transactionDate);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["Validated"].ToString() != "Validated")
                            continue;

                        if (isTaxlotIdExists && dr["TaxlotID"] != DBNull.Value)
                            taxlotID = dr["TaxlotID"].ToString();

                        if (isAccountExists && dr["FundID"] != DBNull.Value)
                            accountID = Convert.ToInt32(dr["FundID"]);

                        if (isCurrencyIDExists && dr["CurrencyID"] != DBNull.Value)
                            currencyID = Convert.ToInt32(dr["CurrencyID"]);

                        if (isCurrencyExists && dr["CurrencyName"] != DBNull.Value)
                            currencyID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyID(dr["CurrencyName"].ToString());

                        if (isFXRateExists && dr["FXRate"] != DBNull.Value)
                            FXRate = Convert.ToDouble(dr["FXRate"]);

                        if (isCashSubAccountExists && dr["Cash-SubAccount"] != DBNull.Value)
                            CashSubAccount = dr["Cash-SubAccount"].ToString();

                        if (isDRExists && dr["DR"] != DBNull.Value)
                            DRAmount = Convert.ToDouble(dr["DR"]);

                        if (isCRExists && dr["CR"] != DBNull.Value)
                            CRAmount = Convert.ToDouble(dr["CR"]);

                        if (isSymbolExists && dr["Symbol"] != DBNull.Value)
                            symbol = dr["Symbol"].ToString();

                        if (isDescriptionExists && dr["Description"] != DBNull.Value)
                            description = dr["Description"].ToString();

                        if (isFXConversionMethodOperatorExists && dr["FXConversionMethodOperator"] != DBNull.Value)
                            FXConversionMethodOperator = dr["FXConversionMethodOperator"].ToString();

                        if (isNonTradingTransactionExists && dr["IsNonTradingTransaction"] != DBNull.Value)
                            isThisNonTradeImport = Convert.ToBoolean(Convert.ToInt32(dr["IsNonTradingTransaction"]));

                        if (EntryId != Convert.ToInt32(dr["EntryID"]))
                        {
                            EntryId = Convert.ToInt32(dr["EntryID"]);
                            transactionList.Add(newTransaction);
                            newTransaction = new Transaction(transactionDate);
                        }

                        if (DRAmount != 0)
                        {
                            TransactionEntry newTrEntry = new TransactionEntry();
                            newTrEntry.SubAcID = CachedDataManager.GetSubAccountIDByAcronym(CashSubAccount);
                            newTrEntry.DR = Math.Abs(Convert.ToDecimal(DRAmount));
                            newTrEntry.TaxLotId = taxlotID;
                            newTrEntry.TaxLotState = ApplicationConstants.TaxLotState.New;
                            newTrEntry.TransactionEntryID = uIDGenerator.GenerateID();
                            newTrEntry.AccountID = accountID;
                            newTrEntry.CurrencyID = currencyID;
                            newTrEntry.Symbol = symbol;
                            newTrEntry.Description = description;
                            newTrEntry.TransactionSource = transactionSource;
                            newTrEntry.ActivitySource = isThisNonTradeImport ? ActivitySource.NonTrading : ActivitySource.OpeningBalance;
                            newTrEntry.FXConversionMethodOperator = FXConversionMethodOperator;
                            newTrEntry.EntryAccountSide = AccountSide.DR;
                            newTrEntry.FxRate = FXRate;
                            newTrEntry.TransactionID = newTrEntry.TransactionEntryID;
                            newTrEntry.UserId = userId;
                            newTransaction.Add(newTrEntry);
                        }
                        if (CRAmount != 0)
                        {
                            TransactionEntry newTrEntry = new TransactionEntry();
                            newTrEntry.SubAcID = CachedDataManager.GetSubAccountIDByAcronym(CashSubAccount);
                            newTrEntry.CR = Math.Abs(Convert.ToDecimal(CRAmount));
                            newTrEntry.TaxLotId = taxlotID;
                            newTrEntry.TaxLotState = ApplicationConstants.TaxLotState.New;
                            newTrEntry.TransactionEntryID = uIDGenerator.GenerateID();
                            newTrEntry.AccountID = accountID;
                            newTrEntry.CurrencyID = currencyID;
                            newTrEntry.Symbol = symbol;
                            newTrEntry.TransactionSource = transactionSource;
                            newTrEntry.ActivitySource = isThisNonTradeImport ? ActivitySource.NonTrading : ActivitySource.OpeningBalance;
                            newTrEntry.EntryAccountSide = AccountSide.CR;
                            newTrEntry.Description = description;
                            newTrEntry.FXConversionMethodOperator = FXConversionMethodOperator;
                            newTrEntry.FxRate = FXRate;
                            newTrEntry.TransactionID = newTrEntry.TransactionEntryID;
                            newTrEntry.UserId = userId;
                            newTransaction.Add(newTrEntry);
                        }
                    }
                    transactionList.Add(newTransaction);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return transactionList;

        }
        /// <summary>
        /// Gets the transactions from ds.
        /// PRANA-9776 New Parameter UserId  
        /// </summary>
        /// <param name="ds">The ds.</param>
        /// <param name="transactionSource">The transaction source.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public static List<Transaction> GetTransactionsFromDS(DataSet ds, CashTransactionType transactionSource, int userId)
        {
            List<Transaction> transactionList = new List<Transaction>();
            try
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    bool isTaxlotIdExists = false; string taxlotID = string.Empty;
                    bool isSymbolExists = false; string symbol = string.Empty;
                    bool isDescriptionExists = false; string description = string.Empty;
                    bool isAccountExists = false; int accountID = 0;
                    bool isCurrencyIDExists = false;
                    bool isCurrencyExists = false; int currencyID = 0;
                    bool isCRCurrencyExists = false; int CRcurrencyID = 0;
                    bool isDRCurrencyExists = false; int DRcurrencyID = 0;
                    bool isFXRateExists = false; double FXrate = 0;
                    bool isCRFXRate = false; bool isDRFXRate = false; double CRFXRate = 0; double DRFXRate = 0;
                    bool isDateExists = false; DateTime transactionDate = DateTime.Now;
                    bool isFXConversionMethodOperatorExists = false; string FXConversionMethodOperator = "M";

                    if (!ds.Tables[0].Columns.Contains("JournalEntries"))
                        return null;

                    if (ds.Tables[0].Columns.Contains("TaxlotID"))
                        isTaxlotIdExists = true;
                    if (ds.Tables[0].Columns.Contains("FundID"))
                        isAccountExists = true;
                    if (ds.Tables[0].Columns.Contains("Symbol"))
                        isSymbolExists = true;
                    if (ds.Tables[0].Columns.Contains("Date"))
                        isDateExists = true;
                    if (ds.Tables[0].Columns.Contains("Description"))
                        isDescriptionExists = true;

                    if (ds.Tables[0].Columns.Contains("CurrencyID"))
                        isCurrencyIDExists = true;
                    if (ds.Tables[0].Columns.Contains("CurrencyName"))
                        isCurrencyExists = true;

                    if (ds.Tables[0].Columns.Contains("DRCurrencyName"))
                        isDRCurrencyExists = true;
                    if (ds.Tables[0].Columns.Contains("CRCurrencyName"))
                        isCRCurrencyExists = true;

                    if (ds.Tables[0].Columns.Contains("FXRate"))
                        isFXRateExists = true;
                    if (ds.Tables[0].Columns.Contains("DRFXRate"))
                        isDRFXRate = true;
                    if (ds.Tables[0].Columns.Contains("CRFXRate"))
                        isCRFXRate = true;

                    if (ds.Tables[0].Columns.Contains("FXConversionMethodOperator"))
                        isFXConversionMethodOperatorExists = true;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["Validated"].ToString() != "Validated")
                            continue;

                        if (isTaxlotIdExists && dr["TaxlotID"] != DBNull.Value)
                            taxlotID = dr["TaxlotID"].ToString();

                        if (isAccountExists && dr["FundID"] != DBNull.Value)
                            accountID = Convert.ToInt32(dr["FundID"]);

                        if (isCurrencyIDExists && dr["CurrencyID"] != DBNull.Value)
                            currencyID = Convert.ToInt32(dr["CurrencyID"]);

                        if (isCurrencyExists && dr["CurrencyName"] != DBNull.Value)
                            currencyID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyID(dr["CurrencyName"].ToString());

                        if (isCRCurrencyExists && dr["CRCurrencyName"] != DBNull.Value)
                            CRcurrencyID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyID(dr["CRCurrencyName"].ToString());

                        if (isDRCurrencyExists && dr["DRCurrencyName"] != DBNull.Value)
                            DRcurrencyID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyID(dr["DRCurrencyName"].ToString());

                        if (isCRFXRate && dr["CRFXRate"] != DBNull.Value)
                            CRFXRate = Convert.ToDouble(dr["CRFXRate"]);
                        else
                            CRFXRate = (currencyID == Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompanyBaseCurrencyID()) ? 1 : 0;

                        if (isFXRateExists && dr["FXRate"] != DBNull.Value)
                            FXrate = Convert.ToDouble(dr["FXRate"]);

                        if (isDRFXRate && dr["DRFXRate"] != DBNull.Value)
                            DRFXRate = Convert.ToDouble(dr["DRFXRate"]);
                        else
                            DRFXRate = (currencyID == Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompanyBaseCurrencyID()) ? 1 : 0;

                        if (isSymbolExists && dr["Symbol"] != DBNull.Value)
                            symbol = dr["Symbol"].ToString();

                        if (isDateExists && dr["Date"] != DBNull.Value)
                            transactionDate = Convert.ToDateTime(dr["Date"]);

                        if (isDescriptionExists && dr["Description"] != DBNull.Value)
                            description = dr["Description"].ToString();

                        if (isFXConversionMethodOperatorExists && dr["FXConversionMethodOperator"] != DBNull.Value)
                            FXConversionMethodOperator = dr["FXConversionMethodOperator"].ToString();

                        string[] arrStrDRCR = dr["JournalEntries"].ToString().Split('|');
                        string[] arrStrDREntries = arrStrDRCR[0].Split(';');
                        string[] arrStrCREntries = arrStrDRCR[1].Split(';');

                        Transaction newTransaction = new Transaction(transactionDate);
                        foreach (string strDREntry in arrStrDREntries)
                        {
                            string[] DREntryWithAmount = strDREntry.Split(':');
                            TransactionEntry newTrEntry = new TransactionEntry();
                            newTrEntry.SubAcID = CachedDataManager.GetSubAccountIDByAcronym(DREntryWithAmount[0].ToString());
                            newTrEntry.DR = Math.Abs(Convert.ToDecimal(DREntryWithAmount[1]));
                            newTrEntry.TaxLotId = taxlotID;
                            newTrEntry.TaxLotState = ApplicationConstants.TaxLotState.New;
                            newTrEntry.TransactionEntryID = uIDGenerator.GenerateID();
                            newTrEntry.AccountID = accountID;
                            newTrEntry.CurrencyID = isDRCurrencyExists ? DRcurrencyID : currencyID;
                            newTrEntry.Symbol = symbol;
                            newTrEntry.Description = description;
                            newTrEntry.TransactionSource = transactionSource;
                            newTrEntry.FXConversionMethodOperator = FXConversionMethodOperator;
                            newTrEntry.EntryAccountSide = AccountSide.DR;
                            newTrEntry.FxRate = isFXRateExists ? FXrate : DRFXRate;
                            //as discussed with Rajat Transaction Id Will be first TransactionEntryId                            
                            newTrEntry.TransactionID = newTrEntry.TransactionEntryID;
                            newTrEntry.UserId = userId;//PRANA-9776
                            newTransaction.Add(newTrEntry);
                        }
                        foreach (string strCREntry in arrStrCREntries)
                        {
                            string[] CREntryWithAmount = strCREntry.Split(':');
                            TransactionEntry newTrEntry = new TransactionEntry();
                            newTrEntry.SubAcID = CachedDataManager.GetSubAccountIDByAcronym(CREntryWithAmount[0].ToString());
                            newTrEntry.CR = Math.Abs(Convert.ToDecimal(CREntryWithAmount[1]));
                            newTrEntry.TaxLotId = taxlotID;
                            newTrEntry.TaxLotState = ApplicationConstants.TaxLotState.New;
                            newTrEntry.TransactionEntryID = uIDGenerator.GenerateID();
                            newTrEntry.AccountID = accountID;
                            newTrEntry.CurrencyID = isCRCurrencyExists ? CRcurrencyID : currencyID;
                            newTrEntry.Symbol = symbol;
                            newTrEntry.TransactionSource = transactionSource;
                            newTrEntry.EntryAccountSide = AccountSide.CR;
                            newTrEntry.Description = description;
                            newTrEntry.FXConversionMethodOperator = FXConversionMethodOperator;
                            newTrEntry.FxRate = isFXRateExists ? FXrate : CRFXRate;
                            //as discussed with Rajat Transaction Id Will be first TransactionEntryId                            
                            newTrEntry.TransactionID = newTrEntry.TransactionEntryID;
                            newTrEntry.UserId = userId; //PRANA-9776
                            newTransaction.Add(newTrEntry);
                        }
                        transactionList.Add(newTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return transactionList;
        }

        /// <summary>
        /// Gets the new transaction entry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        private static TransactionEntry GetNewTransactionEntry<T>(T source)
        {
            TransactionEntry trEntry = new TransactionEntry();
            try
            {
                CashActivity cashActivity = source as CashActivity;
                if (cashActivity != null)
                {
                    if (cashActivity.ActivityState != ApplicationConstants.TaxLotState.Deleted)
                        trEntry.TransactionEntryID = uIDGenerator.GenerateID();
                    trEntry.TransactionEntryID = uIDGenerator.GenerateID();
                    trEntry.TaxLotId = cashActivity.FKID;
                    trEntry.TaxLotState = cashActivity.ActivityState;
                    trEntry.AccountID = cashActivity.AccountID;
                    trEntry.CurrencyID = cashActivity.CurrencyID;
                    trEntry.Symbol = cashActivity.Symbol;
                    trEntry.TransactionSource = cashActivity.TransactionSource;
                    trEntry.TransactionID = trEntry.TransactionEntryID;
                    trEntry.ActivityId_FK = cashActivity.ActivityId;
                    trEntry.Description = cashActivity.Description;
                    trEntry.FxRate = cashActivity.FXRate;
                    trEntry.FXConversionMethodOperator = cashActivity.FXConversionMethodOperator;
                    trEntry.ActivitySource = cashActivity.ActivitySource;
                    trEntry.EntryDate = cashActivity.EntryDate;
                    trEntry.UserId = cashActivity.UserId;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return trEntry;
        }

        /// <summary>
        /// Narendra Kumar Jangir May 09 2013
        /// Create Journal Entries for the activities on the basis of Activity to Journal Mapping
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<Transaction> CreateJournalEntry<T>(T data)
        {
            List<Transaction> transactionList = new List<Transaction>();
            try
            {
                //Narendra Kumar Jangir
                //Date:May 05 2013
                //Make journal entries on the basis of journal mapping 
                //get activity journal mapping which contains activity to journal mapping
                DataTable dtActivityJournalMapping = CachedDataManager.GetActivityJournalMapping();
                Dictionary<int, string> dictAmountType = CachedDataManager.GetActivityAmountType();
                List<CashActivity> lsCashActivity = data as List<CashActivity>;
                if (lsCashActivity != null)
                {
                    foreach (CashActivity cashActivity in lsCashActivity)
                    {
                        if (!CachedDataManager.GetInstance.GetCashPreferenceFundsDict().ContainsKey(cashActivity.AccountID)) // check of cashmgmt startdate is put now in CreateTransactions()
                        {
                            continue;
                        }
                        CreateTransactions(transactionList, dtActivityJournalMapping, dictAmountType, cashActivity);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return transactionList;
        }

        /// <summary>
        ///  [Foreign Positions Settling in Base Currency][Cash Management] Add settlement currency fields in cash management
        /// </summary>
        /// <param name="newTransaction"></param>
        /// <param name="cashActivity"></param>
        private static void SettlementTransactionEntries(Transaction newTransaction, CashActivity cashActivity)
        {
            try
            {
                List<TransactionEntry> newEntries = new List<TransactionEntry>();
                foreach (TransactionEntry trEntry in newTransaction.TransactionEntries)
                {
                    //PRANA-6796 [Cash Management] Incorrect Trading Journals
                    if (cashActivity.SettlCurrencyID > 0 && cashActivity.CurrencyID != cashActivity.SettlCurrencyID && cashActivity.FXRate > 0)
                    {
                        string subAccountName = CachedDataManager.GetSubAccountName(trEntry.SubAcID);
                        if (!string.IsNullOrWhiteSpace(subAccountName) && IsSettlementCurrencySubAccount(subAccountName))
                        {
                            TransactionEntry newEntryInTradeCurrency = GetNewTransactionEntry(cashActivity);
                            TransactionEntry newEntryInSettlCurrency = GetNewTransactionEntry(cashActivity);
                            newEntryInTradeCurrency.TransactionID = trEntry.TransactionID;
                            newEntryInSettlCurrency.TransactionID = trEntry.TransactionID;
                            newEntryInTradeCurrency.SubAcID = trEntry.SubAcID;
                            newEntryInSettlCurrency.SubAcID = trEntry.SubAcID;
                            newEntryInTradeCurrency.TransactionDate = cashActivity.Date;
                            newEntryInSettlCurrency.TransactionDate = cashActivity.Date;
                            //CHMW-3141	[Foreign Positions Settling in Base Currency] Add preferences in cash management module to show/hide settlement journal entries
                            newEntryInTradeCurrency.TransactionSource = CashTransactionType.SettlementTransaction;
                            trEntry.TransactionSource = CashTransactionType.SettlementTransaction;

                            if (trEntry.EntryAccountSide == AccountSide.CR)
                            {
                                newEntryInTradeCurrency.EntryAccountSide = AccountSide.DR;
                                newEntryInSettlCurrency.EntryAccountSide = AccountSide.CR;
                                newEntryInTradeCurrency.DR = trEntry.CR;
                                if (cashActivity.FXConversionMethodOperator.Equals(Operator.D.ToString()) && cashActivity.FXRate > 0)
                                {
                                    newEntryInSettlCurrency.CR = trEntry.CR / (decimal)cashActivity.FXRate;
                                }
                                else if (cashActivity.FXConversionMethodOperator.Equals(Operator.M.ToString()) && cashActivity.FXRate > 0)
                                {
                                    newEntryInSettlCurrency.CR = trEntry.CR * (decimal)cashActivity.FXRate;
                                }
                            }
                            else if (trEntry.EntryAccountSide == AccountSide.DR)
                            {
                                newEntryInTradeCurrency.EntryAccountSide = AccountSide.CR;
                                newEntryInSettlCurrency.EntryAccountSide = AccountSide.DR;
                                newEntryInTradeCurrency.CR = trEntry.DR;
                                if (cashActivity.FXConversionMethodOperator.Equals(Operator.D.ToString()) && cashActivity.FXRate > 0)
                                {
                                    newEntryInSettlCurrency.DR = trEntry.DR / (decimal)cashActivity.FXRate;
                                }
                                else if (cashActivity.FXConversionMethodOperator.Equals(Operator.M.ToString()) && cashActivity.FXRate > 0)
                                {
                                    newEntryInSettlCurrency.DR = trEntry.DR * (decimal)cashActivity.FXRate;
                                }
                            }
                            newEntryInTradeCurrency.CurrencyID = cashActivity.CurrencyID;
                            newEntryInSettlCurrency.CurrencyID = cashActivity.SettlCurrencyID;
                            newEntryInSettlCurrency.FxRate = 1;
                            newEntries.Add(newEntryInTradeCurrency);
                            newEntries.Add(newEntryInSettlCurrency);
                        }
                    }
                }
                //Add new Entries created in transaction
                foreach (TransactionEntry trEntry in newEntries)
                {
                    newTransaction.TransactionEntries.Add(trEntry);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Merges the journal entries.
        /// </summary>
        /// <param name="newTransaction">The new transaction.</param>
        private static void MergeJournalEntries(Transaction newTransaction)
        {
            Dictionary<string, TransactionEntry> dictGroupedTransactionEntry = new Dictionary<string, TransactionEntry>();
            Dictionary<string, TransactionEntry> dictGroupedDeletedTransactionEntry = new Dictionary<string, TransactionEntry>();
            try
            {
                MergeJournalEntries(newTransaction.TransactionEntries, dictGroupedTransactionEntry);
                MergeJournalEntries(newTransaction.DeletedTransactionEntries, dictGroupedDeletedTransactionEntry);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Merges the journal entries.
        /// </summary>
        /// <param name="newTransaction">The new transaction.</param>
        private static void MergeFXAndFxForwardJournalEntries(Transaction newTransaction)
        {
            Dictionary<string, TransactionEntry> dictGroupedTransactionEntry = new Dictionary<string, TransactionEntry>();
            Dictionary<string, TransactionEntry> dictGroupedDeletedTransactionEntry = new Dictionary<string, TransactionEntry>();
            try
            {
                MergeFXAndFxForwardJournalEntries(newTransaction.TransactionEntries, dictGroupedTransactionEntry);
                MergeFXAndFxForwardJournalEntries(newTransaction.DeletedTransactionEntries, dictGroupedDeletedTransactionEntry);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Creates the transactions.
        /// </summary>
        /// <param name="transactionList">The transaction list.</param>
        /// <param name="dtActivityJournalMapping">The dt activity journal mapping.</param>
        /// <param name="dictAmountType">Type of the dictionary amount.</param>
        /// <param name="cashActivity">The cash activity.</param>
        private static void CreateTransactions(List<Transaction> transactionList, DataTable dtActivityJournalMapping, Dictionary<int, string> dictAmountType, CashActivity cashActivity)
        {
            try
            {
                int AmountTypeId;

                #region TransactionEntry According to new structure
                DataRow[] matchingJournalEntries = dtActivityJournalMapping.Select(COLUMN_ACTIVITYTYPEID_FK + " = " + cashActivity.ActivityTypeId);
                List<int> dateTypes = matchingJournalEntries.Where(x => !string.IsNullOrWhiteSpace(x[COLUMN_ACTIVITYDATETYPE].ToString())).Select(x => Convert.ToInt32(x[COLUMN_ACTIVITYDATETYPE].ToString())).Distinct().ToList();
                int TransactionCounter = 0;
                //Iterate for each date type, it does not matter it is dividend or non trade
                foreach (int dateType in dateTypes)
                {
                    DataRow[] matchingEntries = matchingJournalEntries.Where(x => !string.IsNullOrWhiteSpace(x[COLUMN_ACTIVITYDATETYPE].ToString()) && Convert.ToInt32(x[COLUMN_ACTIVITYDATETYPE]) == dateType).ToArray();
                    DateTime date = GetDateForJournalMapping(dateType, cashActivity, out TransactionCounter);
                    Transaction newTransaction = new Transaction(date);
                    if (!((cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.DividendIncome.ToString())) || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.DividendExpense.ToString())) || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.WithholdingTax.ToString())))
                        && !CachedDataManager.GetInstance.GetCashPreferenceFundsDict()[cashActivity.AccountID].Item2))
                    {
                        //Change by Nishant Jain [2015-02-23]
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6321 
                        if (CachedDataManager.GetInstance.GetCashPreferenceFundsDict()[cashActivity.AccountID].Item1.Date < date.Date)
                        {
                            foreach (DataRow rowJournalEntry in matchingEntries)
                            {
                                //get AmountTypeId from journal mapping
                                if ((Int32.TryParse((rowJournalEntry[COLUMN_AMOUNTTYPEID_FK].ToString()), out AmountTypeId)) && dictAmountType.ContainsKey(AmountTypeId))
                                {
                                    //get Amount Type on the basis of AmountTypeId
                                    string AmountType = dictAmountType[AmountTypeId].ToString();
                                    //on the basis of AmountTypeId get the amount
                                    decimal Amount = GetAmountForAmountType(AmountType, cashActivity);

                                    bool IsDebitAccountEmpty = string.IsNullOrEmpty(rowJournalEntry[COLUMN_DEBITACCOUNT].ToString());
                                    bool IsCreditAccountEmpty = string.IsNullOrEmpty(rowJournalEntry[COLUMN_CREDITACCOUNT].ToString());

                                    if ((Amount != 0 || CheckActivityType(cashActivity)) && !date.Equals(DateTime.MinValue))
                                    {
                                        if (!IsDebitAccountEmpty && !IsCreditAccountEmpty)
                                        {
                                            AddNewEntries(cashActivity, TransactionCounter, date, newTransaction, rowJournalEntry, AmountType, Amount, false);
                                        }
                                        else if (IsDebitAccountEmpty || IsCreditAccountEmpty)
                                        {
                                            AddNewEntry(cashActivity, TransactionCounter, date, newTransaction, rowJournalEntry, Amount, false);
                                        }
                                    }
                                    else if ((Amount == 0 || CheckActivityType(cashActivity)) && !date.Equals(DateTime.MinValue))
                                    {
                                        if (!IsDebitAccountEmpty && !IsCreditAccountEmpty)
                                        {
                                            AddNewEntries(cashActivity, TransactionCounter, date, newTransaction, rowJournalEntry, AmountType, Amount, true);
                                        }
                                        else if (IsDebitAccountEmpty || IsCreditAccountEmpty)
                                        {
                                            AddNewEntry(cashActivity, TransactionCounter, date, newTransaction, rowJournalEntry, Amount, true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // If total amount of any existing trade is changed to 0, then for that activity, journal should be deleted .
                    if (newTransaction.TransactionEntries.Count == 0)
                    {
                        if (dateType == (int)ActivityDateType.ExDate)
                        {
                            newTransaction.Add(new TransactionEntry()
                            {
                                ActivityId_FK = cashActivity.ActivityId,
                                TaxLotState = ApplicationConstants.TaxLotState.Deleted,
                                TransactionSource = cashActivity.TransactionSource,
                                ActivitySource = cashActivity.ActivitySource,
                                AccountID = cashActivity.AccountID,
                                TransactionNumber = 1
                            });
                        }
                        else
                        {
                            newTransaction.Add(new TransactionEntry()
                            {
                                ActivityId_FK = cashActivity.ActivityId,
                                TaxLotState = ApplicationConstants.TaxLotState.Deleted,
                                TransactionSource = cashActivity.TransactionSource,
                                ActivitySource = cashActivity.ActivitySource,
                                AccountID = cashActivity.AccountID,
                                TransactionNumber = 2
                            });
                        }
                    }
                    if (cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.FxForward_Settled.ToString()))
                       || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.FxForwardLongLT_Settled.ToString()))
                       || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.FxForwardLongST_Settled.ToString()))
                      || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.FxForwardShortST_Settled.ToString()))
                        || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.FX_Settled.ToString())))
                    {
                        MergeFXAndFxForwardJournalEntries(newTransaction);
                    }
                    else
                        MergeJournalEntries(newTransaction);
                    SettlementTransactionEntries(newTransaction, cashActivity);
                    transactionList.Add(newTransaction);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Gets the date for journal mapping.
        /// </summary>
        /// <param name="dateType">Type of the date.</param>
        /// <param name="cashActivity">The cash activity.</param>
        /// <param name="TransactionCounter">The transaction counter.</param>
        /// <returns></returns>
        private static DateTime GetDateForJournalMapping(int dateType, CashActivity cashActivity, out int TransactionCounter)
        {
            DateTime date = DateTime.MinValue;
            TransactionCounter = 0;
            try
            {
                ActivityDateType actDate = (ActivityDateType)dateType;
                switch (actDate)
                {
                    case ActivityDateType.ExDate:
                        date = cashActivity.Date;
                        TransactionCounter = 1;
                        break;
                    case ActivityDateType.PayoutDate:
                        if (cashActivity.SettlementDate != null)
                            date = DateTime.Parse(cashActivity.SettlementDate.ToString());
                        TransactionCounter = 2;
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return date;
        }

        /// <summary>
        /// Narendra Kumar Jangir May 09 2013
        /// Get amount for the given amount type
        /// </summary>
        /// <param name="AmountType"></param>
        /// <returns></returns>
        private static decimal GetAmountForAmountType(string AmountType, CashActivity cashActivity)
        {
            decimal amount = 0;
            try
            {
                switch (AmountType)
                {
                    case COLUMN_COMMISSION:
                        amount = Convert.ToDecimal(cashActivity.Commission);
                        break;
                    case COLUMN_MISCFEES:
                        amount = Convert.ToDecimal(cashActivity.MiscFees);
                        break;
                    case COLUMN_TRANSACTIONLAVY:
                        amount = Convert.ToDecimal(cashActivity.TransactionLevy);
                        break;
                    case COLUMN_STAMPDUTY:
                        amount = Convert.ToDecimal(cashActivity.StampDuty);
                        break;
                    case COLUMN_OTHERBROKERFEES:
                        amount = Convert.ToDecimal(cashActivity.OtherBrokerFees);
                        break;
                    case COLUMN_TAXONCOMMISSIONS:
                        amount = Convert.ToDecimal(cashActivity.TaxOnCommissions);
                        break;
                    case COLUMN_CLEARINGFEE:
                        amount = Convert.ToDecimal(cashActivity.ClearingFee);
                        break;
                    case COLUMN_AMOUNT:
                        amount = Convert.ToDecimal(cashActivity.Amount);
                        break;
                    case COLUMN_PNL:
                        amount = Convert.ToDecimal(cashActivity.PnL);
                        break;
                    case COLUMN_FXPNL:
                        amount = Convert.ToDecimal(cashActivity.FXPnL);
                        break;
                    case COLUMN_CLOSEDQTY:
                        amount = Convert.ToDecimal(cashActivity.ClosedQty);
                        break;
                    case COLUMN_SECFEE:
                        amount = Convert.ToDecimal(cashActivity.SecFee);
                        break;
                    case COLUMN_OCCFEE:
                        amount = Convert.ToDecimal(cashActivity.OccFee);
                        break;
                    case COLUMN_ORFFEE:
                        amount = Convert.ToDecimal(cashActivity.OrfFee);
                        break;
                    case COLUMN_CLEARINGBROKERFEE:
                        amount = Convert.ToDecimal(cashActivity.ClearingBrokerFee);
                        break;
                    case COLUMN_SOFTCOMMISSION:
                        amount = Convert.ToDecimal(cashActivity.SoftCommission);
                        break;
                    case COLUMN_OPTIONPREMIUMADJUSTMENT:
                        amount = Convert.ToDecimal(cashActivity.OptionPremiumAdjustment);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return amount;
        }

        /// <summary>
        /// This method will return true is sub account is to be impacted in the settlement currency
        /// </summary>
        /// <param name="subAccountName"></param>
        /// <returns></returns>
        private static bool IsSettlementCurrencySubAccount(string subAccountName)
        {
            try
            {
                if (subAccountName.Contains("[Cash]"))
                    return true;
                else
                {
                    string settlementCurrencySubAccounts = ConfigurationHelper.Instance.GetAppSettingValueByKey(ConfigurationHelper.CONFIG_APPSETTING_SettlementCurrencySubAccounts);
                    List<string> lstSettlementCurrencySubAccounts = new List<string>(settlementCurrencySubAccounts.Split(Seperators.SEPERATOR_14).Select(s => s.Trim()));
                    int startIndex = subAccountName.IndexOf('[');
                    int endIndex = subAccountName.IndexOf(']');
                    string trimmedSubAccount = subAccountName;
                    if (startIndex > 0 && endIndex > 0)
                        trimmedSubAccount = (subAccountName.Substring(0, startIndex) + subAccountName.Substring(endIndex + 1)).Trim();
                    if (lstSettlementCurrencySubAccounts.Contains(trimmedSubAccount, StringComparer.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Check Activity Type
        /// </summary>
        /// <param name="cashActivity"></param>
        /// <returns></returns>
        private static bool CheckActivityType(CashActivity cashActivity)
        {
            try
            {
                return ((cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Expense.ToString()))
                                                           || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Trading_Interest.ToString()))
                                                           || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Received.ToString()))
                                                           || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Interest_Receivable.ToString()))
                                                           || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.Bond_Trading_Payable.ToString())))
                                                           && !cashActivity.TransactionSource.Equals(CashTransactionType.DailyCalculation));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Add New Entries
        /// </summary>
        /// <param name="cashActivity"></param>
        /// <param name="TransactionCounter"></param>
        /// <param name="date"></param>
        /// <param name="newTransaction"></param>
        /// <param name="rowJournalEntry"></param>
        /// <param name="AmountType"></param>
        /// <param name="Amount"></param>
        /// <param name="AddDeletedEntry"></param>
        private static void AddNewEntries(CashActivity cashActivity, int TransactionCounter, DateTime date, Transaction newTransaction, DataRow rowJournalEntry, string AmountType, decimal Amount, bool AddDeletedEntry)
        {
            try
            {
                CashValueType cashValueType = (CashValueType)(Convert.ToInt32(rowJournalEntry[COLUMN_CASHVALUETYPE].ToString()));

                #region Debit entry
                TransactionEntry trDebitEntry = GetNewTransactionEntry(cashActivity);
                trDebitEntry.TransactionDate = date;

                //PRANA-9776
                trDebitEntry.UserId = cashActivity.UserId;

                //swap debit and credit sub accounts whenever cash value type does not match with amount sign
                //modified by: Bharat raturi, 03 oct 2014
                //Purpose: Use amount instead of side multiplier to decide entry side 
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-4905
                //if ((cashValueType.Equals(CashValueType.Positive) && cashActivity.SideMultiplier >= 0) || (cashValueType.Equals(CashValueType.Negative) && cashActivity.SideMultiplier < 0))
                if ((cashValueType.Equals(CashValueType.Positive) && Amount >= 0) || (cashValueType.Equals(CashValueType.Negative) && Amount < 0))
                    trDebitEntry.SubAcID = int.Parse(rowJournalEntry[COLUMN_DEBITACCOUNT].ToString());
                else
                    trDebitEntry.SubAcID = int.Parse(rowJournalEntry[COLUMN_CREDITACCOUNT].ToString());
                trDebitEntry.DR = Math.Abs(Amount);
                trDebitEntry.EntryAccountSide = AccountSide.DR;
                trDebitEntry.TransactionNumber = TransactionCounter;

                #endregion

                #region Credit entry
                TransactionEntry trCreditEntry = GetNewTransactionEntry(cashActivity);
                trCreditEntry.TransactionDate = date;
                //PRANA-9776
                trCreditEntry.UserId = cashActivity.UserId;
                //modified by: Bharat raturi, 03 oct 2014
                //Purpose: Use amount instead of side multiplier to decide entry side 
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-4905
                //swap debit and credit sub accounts whenever cash value type does not match with amount sign
                if ((cashValueType.Equals(CashValueType.Positive) && Amount >= 0) || (cashValueType.Equals(CashValueType.Negative) && Amount < 0))
                    trCreditEntry.SubAcID = int.Parse(rowJournalEntry[COLUMN_CREDITACCOUNT].ToString());
                else
                    trCreditEntry.SubAcID = int.Parse(rowJournalEntry[COLUMN_DEBITACCOUNT].ToString());
                //since for each credit entry there is equal amount debit entry so amount for debit and credit account will be same
                trCreditEntry.CR = Math.Abs(Amount);
                trCreditEntry.EntryAccountSide = AccountSide.CR;
                trCreditEntry.TransactionNumber = TransactionCounter;

                #endregion

                Activities activityType = CachedDataManager.GetActivityTypeFromAcronym(cashActivity.ActivityType);

                switch (activityType)
                {
                    case Activities.CashTransfer:
                        #region Cash Transfer Activity
                        trDebitEntry.CurrencyID = cashActivity.LeadCurrencyID;
                        trCreditEntry.CurrencyID = cashActivity.VsCurrencyID;
                        if (trCreditEntry.FXConversionMethodOperator.ToString().Equals(Operator.D.ToString()) && trCreditEntry.FxRate > 0)
                        {
                            trCreditEntry.CR = trCreditEntry.CR / (decimal)trCreditEntry.FxRate;
                            trCreditEntry.FxRate = 1;
                        }
                        else
                        {
                            trCreditEntry.CR = trCreditEntry.CR * (decimal)trCreditEntry.FxRate;
                            trCreditEntry.FxRate = 1;
                        }
                        #endregion
                        break;

                    case Activities.AccrualsRevaluation:
                        #region Accruals Revaluation Activity
                        //for accruals revaluation debit/credit sub-account should be same as that of actual sub-account
                        //e.g. for dividend revaluation sub-account may be dividend receivable/payable
                        if (trDebitEntry.SubAcID.Equals(Convert.ToInt32(CachedDataManager.GetSubAccountIDByAcronym(SubAccounts.Cash.ToString()))))
                        {
                            trDebitEntry.SubAcID = cashActivity.SubAccountID;
                        }
                        else if (trCreditEntry.SubAcID.Equals(Convert.ToInt32(CachedDataManager.GetSubAccountIDByAcronym(SubAccounts.Cash.ToString()))))
                        {
                            trCreditEntry.SubAcID = cashActivity.SubAccountID;
                        }
                        #endregion
                        break;

                    case Activities.FXL:
                    case Activities.FXS:
                    case Activities.FXOptionL:
                    case Activities.FXOptionS:
                        #region Handling for FX
                        if (cashActivity.SideMultiplier > 0)
                        {
                            trDebitEntry.DR = cashActivity.ClosedQty;
                            trDebitEntry.CurrencyID = cashActivity.CurrencyID;
                            trCreditEntry.CurrencyID = cashActivity.CurrencyID == cashActivity.LeadCurrencyID ? cashActivity.VsCurrencyID : cashActivity.LeadCurrencyID;
                        }
                        else
                        {
                            trCreditEntry.CR = cashActivity.ClosedQty;
                            trCreditEntry.CurrencyID = cashActivity.CurrencyID;
                            trDebitEntry.CurrencyID = cashActivity.CurrencyID == cashActivity.LeadCurrencyID ? cashActivity.VsCurrencyID : cashActivity.LeadCurrencyID;

                            trDebitEntry.SubAcID = int.Parse(rowJournalEntry[COLUMN_DEBITACCOUNT].ToString());
                            trCreditEntry.SubAcID = int.Parse(rowJournalEntry[COLUMN_CREDITACCOUNT].ToString());
                        }
                        SetTransactionFxRate(cashActivity, trDebitEntry);
                        SetTransactionFxRate(cashActivity, trCreditEntry);

                        if (cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.FXS.ToString())) || cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.FXForwardS.ToString())))
                        {
                            int currency = trDebitEntry.CurrencyID;
                            trDebitEntry.CurrencyID = trCreditEntry.CurrencyID;
                            trCreditEntry.CurrencyID = currency;

                            double fxRate = 0;
                            fxRate = trDebitEntry.FxRate;
                            trDebitEntry.FxRate = trCreditEntry.FxRate;
                            trCreditEntry.FxRate = fxRate;

                            decimal amount = 0;
                            if (trDebitEntry.CR > 0)
                            {
                                amount = trDebitEntry.CR;
                                trDebitEntry.CR = trCreditEntry.DR;
                                trCreditEntry.DR = amount;
                            }
                            else if (trDebitEntry.DR > 0)
                            {
                                amount = trDebitEntry.DR;
                                trDebitEntry.DR = trCreditEntry.CR;
                                trCreditEntry.CR = amount;
                            }
                        }
                        #endregion
                        break;

                    case Activities.FX_Settled:
                    case Activities.FXL_CurrencySettled:
                    case Activities.FXS_CurrencySettled:
                    case Activities.FxForward_Settled:
                    case Activities.FxForwardLongLT_Settled:
                    case Activities.FxForwardLongST_Settled:
                    case Activities.FxForwardShortST_Settled:
                        #region Handling for FX Settled
                        if (cashActivity.LeadCurrencyID.Equals(CachedDataManager.GetInstance.GetCompanyBaseCurrencyID()) || cashActivity.VsCurrencyID.Equals(CachedDataManager.GetInstance.GetCompanyBaseCurrencyID()))
                        {
                            trDebitEntry.FxRate = trCreditEntry.FxRate = 1;
                            if (cashActivity.SideMultiplier > 0)
                            {
                                if (AmountType.Equals(COLUMN_CLOSEDQTY))
                                {
                                    trDebitEntry.CurrencyID = cashActivity.CurrencyID == cashActivity.LeadCurrencyID ? cashActivity.VsCurrencyID : cashActivity.LeadCurrencyID;
                                    trCreditEntry.CurrencyID = cashActivity.CurrencyID;
                                    trDebitEntry.DR = Math.Abs(cashActivity.Amount);
                                }

                                if (AmountType.Equals(COLUMN_AMOUNT))
                                {
                                    trDebitEntry.CurrencyID = cashActivity.CurrencyID;
                                    trCreditEntry.CurrencyID = cashActivity.CurrencyID == cashActivity.LeadCurrencyID ? cashActivity.VsCurrencyID : cashActivity.LeadCurrencyID;


                                    if (cashActivity.avgPrice != 0)
                                    {
                                        if (cashActivity.CurrencyID == cashActivity.LeadCurrencyID)
                                            trDebitEntry.DR = Math.Abs(Amount / Convert.ToDecimal(cashActivity.avgPrice));
                                        else
                                        {
                                            trDebitEntry.DR = Math.Abs(Amount * Convert.ToDecimal(cashActivity.avgPrice));
                                        }
                                    }
                                    else
                                        trDebitEntry.DR = Math.Abs(Amount);

                                    trCreditEntry.CR = Math.Abs(Amount);
                                }
                            }
                            else
                            {
                                if (AmountType.Equals(COLUMN_CLOSEDQTY))
                                {
                                    trDebitEntry.CurrencyID = cashActivity.CurrencyID;
                                    trCreditEntry.CurrencyID = cashActivity.CurrencyID == cashActivity.LeadCurrencyID ? cashActivity.VsCurrencyID : cashActivity.LeadCurrencyID;
                                    trCreditEntry.CR = Math.Abs(cashActivity.Amount);
                                }

                                if (AmountType.Equals(COLUMN_AMOUNT))
                                {
                                    trCreditEntry.CurrencyID = cashActivity.CurrencyID;
                                    trDebitEntry.CurrencyID = cashActivity.CurrencyID == cashActivity.LeadCurrencyID ? cashActivity.VsCurrencyID : cashActivity.LeadCurrencyID;

                                    if (cashActivity.avgPrice != 0)
                                    {
                                        if (cashActivity.CurrencyID == cashActivity.LeadCurrencyID)
                                        {
                                            trCreditEntry.CR = Math.Abs(Amount / Convert.ToDecimal(cashActivity.avgPrice));
                                        }
                                        else
                                        {
                                            trCreditEntry.CR = Math.Abs(Amount * Convert.ToDecimal(cashActivity.avgPrice));
                                        }
                                    }
                                    else
                                        trCreditEntry.CR = Math.Abs(Amount);
                                    trDebitEntry.DR = Math.Abs(Amount);
                                }
                            }

                            if (AmountType.Equals(COLUMN_PNL) || AmountType.Equals(COLUMN_FXPNL))
                            {
                                if (trDebitEntry.SubAcID.Equals(int.Parse(rowJournalEntry[COLUMN_DEBITACCOUNT].ToString())))
                                {
                                    if (cashActivity.avgPrice != 0)
                                    {
                                        if (cashActivity.CurrencyID == cashActivity.LeadCurrencyID)
                                        {
                                            trDebitEntry.DR = Math.Abs(Amount / Convert.ToDecimal(cashActivity.avgPrice));
                                        }
                                        else
                                        {
                                            trDebitEntry.DR = Math.Abs(Amount * Convert.ToDecimal(cashActivity.avgPrice));
                                        }
                                    }
                                    else
                                        trDebitEntry.DR = Math.Abs(Amount);
                                    trCreditEntry.CR = Math.Abs(Amount);

                                    trDebitEntry.CurrencyID = cashActivity.CurrencyID;
                                    trCreditEntry.CurrencyID = cashActivity.CurrencyID == cashActivity.LeadCurrencyID ? cashActivity.VsCurrencyID : cashActivity.LeadCurrencyID;
                                }
                                else
                                {
                                    trCreditEntry.CurrencyID = cashActivity.CurrencyID;
                                    trDebitEntry.CurrencyID = cashActivity.CurrencyID == cashActivity.LeadCurrencyID ? cashActivity.VsCurrencyID : cashActivity.LeadCurrencyID;
                                    trDebitEntry.DR = Math.Abs(Amount);
                                    if (cashActivity.avgPrice != 0)
                                    {
                                        if (cashActivity.CurrencyID == cashActivity.LeadCurrencyID)
                                        {
                                            trCreditEntry.CR = Math.Abs(Amount / Convert.ToDecimal(cashActivity.avgPrice));
                                        }
                                        else
                                        {
                                            trCreditEntry.CR = Math.Abs(Amount * Convert.ToDecimal(cashActivity.avgPrice));
                                        }
                                    }
                                    else
                                        trCreditEntry.CR = Math.Abs(Amount);
                                }
                            }

                            SetTransactionFxRate(cashActivity, trCreditEntry);
                            SetTransactionFxRate(cashActivity, trDebitEntry);
                        }
                        #endregion
                        break;
                }

                if (!AddDeletedEntry)
                {
                    newTransaction.Add(trDebitEntry);
                    newTransaction.Add(trCreditEntry);
                }
                else
                {
                    newTransaction.DeletedTransactionEntries.Add(trDebitEntry);
                    newTransaction.DeletedTransactionEntries.Add(trCreditEntry);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the transaction fx rate.
        /// </summary>
        /// <param name="cashActivity">The cash activity.</param>
        /// <param name="tranEntry">The tran entry.</param>
        private static void SetTransactionFxRate(CashActivity cashActivity, TransactionEntry tranEntry)
        {
            try
            {
                tranEntry.FXConversionMethodOperator = Operator.M.ToString();
                if (tranEntry.CurrencyID.Equals(CachedDataManager.GetInstance.GetCompanyBaseCurrencyID()))
                    tranEntry.FxRate = 1;
                else
                {
                    if (tranEntry.CurrencyID.Equals(cashActivity.LeadCurrencyID))
                        tranEntry.FxRate = cashActivity.avgPrice;
                    else
                        tranEntry.FxRate = cashActivity.FXRate;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Add New Entry
        /// </summary>
        /// <param name="cashActivity"></param>
        /// <param name="TransactionCounter"></param>
        /// <param name="date"></param>
        /// <param name="newTransaction"></param>
        /// <param name="rowJournalEntry"></param>
        /// <param name="Amount"></param>
        /// <param name="addDeletedEntry"></param>
        private static void AddNewEntry(CashActivity cashActivity, int TransactionCounter, DateTime date, Transaction newTransaction, DataRow rowJournalEntry, decimal Amount, bool addDeletedEntry)
        {
            try
            {
                CashValueType cashValueType = (CashValueType)(Convert.ToInt32(rowJournalEntry[COLUMN_CASHVALUETYPE].ToString()));
                if (!string.IsNullOrEmpty(rowJournalEntry[COLUMN_DEBITACCOUNT].ToString()))
                {
                    TransactionEntry trDebitEntry = GetNewTransactionEntry(cashActivity);
                    trDebitEntry.TransactionDate = date;
                    if ((cashValueType.Equals(CashValueType.Positive) && Amount >= 0) || (cashValueType.Equals(CashValueType.Negative) && Amount < 0))
                    {
                        trDebitEntry.DR = Math.Abs(Amount);
                        trDebitEntry.EntryAccountSide = AccountSide.DR;
                    }
                    else
                    {
                        trDebitEntry.CR = Math.Abs(Amount);
                        trDebitEntry.EntryAccountSide = AccountSide.CR;
                    }
                    trDebitEntry.SubAcID = int.Parse(rowJournalEntry[COLUMN_DEBITACCOUNT].ToString());
                    trDebitEntry.TransactionNumber = TransactionCounter;
                    if (!addDeletedEntry)
                    {
                        newTransaction.Add(trDebitEntry);
                    }
                    else
                    {
                        newTransaction.DeletedTransactionEntries.Add(trDebitEntry);
                    }

                }

                else if (!string.IsNullOrEmpty(rowJournalEntry[COLUMN_CREDITACCOUNT].ToString()))
                {
                    TransactionEntry trCreditEntry = GetNewTransactionEntry(cashActivity);
                    trCreditEntry.TransactionDate = date;
                    if ((cashValueType.Equals(CashValueType.Positive) && Amount >= 0) || (cashValueType.Equals(CashValueType.Negative) && Amount < 0))
                    {
                        trCreditEntry.CR = Math.Abs(Amount);
                        trCreditEntry.EntryAccountSide = AccountSide.CR;
                    }
                    else
                    {
                        trCreditEntry.DR = Math.Abs(Amount);
                        trCreditEntry.EntryAccountSide = AccountSide.DR;
                    }
                    trCreditEntry.SubAcID = int.Parse(rowJournalEntry[COLUMN_CREDITACCOUNT].ToString());
                    trCreditEntry.TransactionNumber = TransactionCounter;
                    if (!addDeletedEntry)
                    {
                        newTransaction.Add(trCreditEntry);
                    }
                    else
                    {
                        newTransaction.DeletedTransactionEntries.Add(trCreditEntry);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Merge Journal Entries
        /// </summary>
        /// <param name="newTransactionEntries"></param>
        /// <param name="dictGroupedTransactionEntry"></param>
        private static void MergeFXAndFxForwardJournalEntries(IList<TransactionEntry> newTransactionEntries, Dictionary<string, TransactionEntry> dictGroupedTransactionEntry)
        {
            try
            {
                foreach (TransactionEntry trEntry in newTransactionEntries)
                {
                    //group by transaction-id, sub-account-id, currency
                    string keyToGroup = trEntry.TransactionID + trEntry.SubAcID + trEntry.CurrencyID;
                    if (dictGroupedTransactionEntry.ContainsKey(keyToGroup))
                    {
                        dictGroupedTransactionEntry[keyToGroup].CR += trEntry.CR;
                        dictGroupedTransactionEntry[keyToGroup].DR += trEntry.DR;
                        if (dictGroupedTransactionEntry[keyToGroup].CR != 0 && dictGroupedTransactionEntry[keyToGroup].DR != 0)
                        {
                            if (dictGroupedTransactionEntry[keyToGroup].CR > dictGroupedTransactionEntry[keyToGroup].DR)
                            {
                                dictGroupedTransactionEntry[keyToGroup].CR = dictGroupedTransactionEntry[keyToGroup].CR - dictGroupedTransactionEntry[keyToGroup].DR;
                                dictGroupedTransactionEntry[keyToGroup].EntryAccountSide = AccountSide.CR;
                                dictGroupedTransactionEntry[keyToGroup].DR = 0;
                            }
                            else
                            {
                                dictGroupedTransactionEntry[keyToGroup].DR = dictGroupedTransactionEntry[keyToGroup].DR - dictGroupedTransactionEntry[keyToGroup].CR;
                                dictGroupedTransactionEntry[keyToGroup].EntryAccountSide = AccountSide.DR;
                                dictGroupedTransactionEntry[keyToGroup].CR = 0;
                            }
                        }
                    }
                    else
                    {
                        dictGroupedTransactionEntry.Add(keyToGroup, trEntry);
                    }
                }
                newTransactionEntries.Clear();
                //bind Transaction Entries to transaction
                foreach (KeyValuePair<string, TransactionEntry> kvpTransactionEntry in dictGroupedTransactionEntry)
                {
                    if (!newTransactionEntries.Contains(kvpTransactionEntry.Value))
                        newTransactionEntries.Add(kvpTransactionEntry.Value);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        private static void MergeJournalEntries(IList<TransactionEntry> newTransactionEntries, Dictionary<string, TransactionEntry> dictGroupedTransactionEntry)
        {
            try
            {
                foreach (TransactionEntry trEntry in newTransactionEntries)
                {
                    //group by transaction-id, sub-account-id, currency
                    string keyToGroup = trEntry.TransactionID + trEntry.SubAcID + trEntry.CurrencyID + trEntry.EntryAccountSide;
                    if (dictGroupedTransactionEntry.ContainsKey(keyToGroup))
                    {
                        dictGroupedTransactionEntry[keyToGroup].CR += trEntry.CR;
                        dictGroupedTransactionEntry[keyToGroup].DR += trEntry.DR;
                    }
                    else
                    {
                        dictGroupedTransactionEntry.Add(keyToGroup, trEntry);
                    }
                }
                newTransactionEntries.Clear();
                //bind Transaction Entries to transaction
                foreach (KeyValuePair<string, TransactionEntry> kvpTransactionEntry in dictGroupedTransactionEntry)
                {
                    if (!newTransactionEntries.Contains(kvpTransactionEntry.Value))
                        newTransactionEntries.Add(kvpTransactionEntry.Value);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
