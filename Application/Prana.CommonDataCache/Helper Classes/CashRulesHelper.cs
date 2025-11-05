using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.CommonDataCache
{
    public class CashRulesHelper
    {

        public static string getSubAccountName(int subAccountID)
        {
            string subAccountName = CachedDataManager.GetSubAccountName(subAccountID);
            return subAccountName;
        }

        public static string getSymbolByTaxlotID()
        {
            string symbol = ""; //CachedDataManager.GetSubAccountName(TaxlotID);
            return symbol;
        }



        public static string GetAccountType(int subAccountID)
        {
            try
            {
                return CachedDataManager.GetAccountTypeAcronymBySubAccountID(subAccountID);

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
            return string.Empty;
        }

        public static List<TransactionEntry> GetCashTransactionEntries(Transaction transaction)
        {
            List<TransactionEntry> lsCashTransacionEntries = new List<TransactionEntry>();
            try
            {
                if (transaction.TransactionEntries != null && transaction.TransactionEntries.Count > 0)
                {
                    foreach (TransactionEntry transactionEntry in transaction.TransactionEntries)
                    {
                        string accountType = GetAccountType(transactionEntry.SubAcID);
                        if (accountType == TransactionType.Cash.ToString())
                        {
                            lsCashTransacionEntries.Add(transactionEntry);
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
            return lsCashTransacionEntries;
        }


        public static decimal getCashImpact(TransactionEntry transactionEntry)
        {
            try
            {
                Dictionary<string, int> accountSideMultiplier = CachedDataManager.GetTransactionEntrySideMultiplierBySubAccountID(transactionEntry.SubAcID);
                if (transactionEntry.DR > 0)
                {
                    return transactionEntry.DR * accountSideMultiplier["DR"];
                }
                else if (transactionEntry.CR > 0)
                {
                    return transactionEntry.CR * accountSideMultiplier["CR"];
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
            return 0;
        }

        public static List<int> getRecurringAssetIDS()
        {
            List<int> lsRecurringIDs = new List<int>();
            lsRecurringIDs.Add((int)AssetCategory.Future);
            lsRecurringIDs.Add((int)AssetCategory.FixedIncome);
            lsRecurringIDs.Add((int)AssetCategory.ConvertibleBond);
            return lsRecurringIDs;
        }

        public static List<Transaction> SetCashDataNameValues(List<Transaction> transactions)
        {
            try
            {
                if (transactions != null)
                {
                    foreach (Transaction t in transactions)
                    {
                        foreach (TransactionEntry data in t.TransactionEntries)
                        {
                            data.BaseCurrencyID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(data.AccountID);
                            data.AccountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(data.AccountID);
                            data.CurrencyName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(data.CurrencyID);
                            data.BaseCurrencyName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(data.BaseCurrencyID);
                            data.SubAcName = CashRulesHelper.getSubAccountName(data.SubAcID);
                            //PRANA-9776
                            data.UserName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetUserText(data.UserId);

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
            return transactions;

        }

        public static GenericBindingList<CompanyAccountCashCurrencyValue> SetDayEndCash_NameValues(GenericBindingList<CompanyAccountCashCurrencyValue> dayEndDataListWithIds)
        {
            try
            {
                foreach (CompanyAccountCashCurrencyValue data in dayEndDataListWithIds)
                {
                    data.AccountName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAccountText(data.AccountID);
                    data.LocalCurrencyName = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyText(data.LocalCurrencyID);
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
            return dayEndDataListWithIds;
        }

        public static List<Transaction> getTransactionsFromTransactionEntries(List<TransactionEntry> data)
        {
            try
            {
                if (data != null && data.Count > 0)
                {
                    Transaction tr = new Transaction(data[0].TransactionDate);
                    tr.TransactionID = data[0].TransactionID;
                    List<Transaction> lsTransaction = new List<Transaction>();
                    lsTransaction.Add(tr);
                    foreach (TransactionEntry trEntry in data)
                    {
                        tr = lsTransaction.FirstOrDefault(t => t.TransactionID.Equals(trEntry.TransactionID));
                        if (tr != null)
                            tr.Add(trEntry);
                        else
                        {
                            tr = new Transaction(trEntry.TransactionDate);
                            tr.TransactionID = trEntry.TransactionID;
                            tr.Add(trEntry);
                            lsTransaction.Add(tr);
                        }
                    }
                    return lsTransaction;
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
            return null;
        }

        public static List<CashActivity> SetActivityDataNameValues(List<CashActivity> activities)
        {
            try
            {
                if (activities != null)
                {
                    foreach (CashActivity t in activities)
                    {
                        t.ActivityType = Prana.CommonDataCache.CachedDataManager.GetActivityText(t.ActivityTypeId);
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
            return activities;

        }


        /// <summary>
        /// Raturi: remove the cash journals when publishing
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-5967
        /// Get Transaction entries that should be removed from the grid when publishing the data
        /// </summary>
        /// <param name="CashImpactToBind"></param>
        /// <param name="trEntry"></param>
        /// <returns></returns>
        public static TransactionEntry GetTransactionEntryToRemove(GenericBindingList<TransactionEntry> CashImpactToBind, TransactionEntry trEntry)
        {
            try
            {
                if (CashImpactToBind != null && CashImpactToBind.Count > 0 && trEntry != null && !string.IsNullOrEmpty(trEntry.ActivityId_FK))
                {
                    return CashImpactToBind.FirstOrDefault(tr => tr.ActivityId_FK != null && tr.ActivityId_FK.Equals(trEntry.ActivityId_FK));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
    }
}
