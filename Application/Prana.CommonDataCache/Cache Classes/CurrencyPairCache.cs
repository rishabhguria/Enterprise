using Prana.LogManager;
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;

namespace Prana.CommonDataCache.Cache_Classes
{
    public class CurrencyPairCache
    {
        private ConcurrentBag<Tuple<int, int>> _currencyPairConcurrentBag;

        // Lock synchronization object
        private static readonly object _syncLock = new object();

        private static CurrencyPairCache _instance;

        public static CurrencyPairCache GetInstance()
        {
            // Support multithreaded applications through
            // 'Double checked locking' pattern which (once
            // the instance exists) avoids locking each
            // time the method is invoked
            if (_instance == null)
            {
                lock (_syncLock)
                {
                    if (_instance == null)
                    {
                        _instance = new CurrencyPairCache();
                    }
                }
            }

            return _instance;
        }

        public bool IsPairAvailable(int firstCurrency, int secondCurrency)
        {
            return _currencyPairConcurrentBag.Any(tuple =>
                (tuple.Item1 == firstCurrency && tuple.Item2 == secondCurrency) ||
                (tuple.Item1 == secondCurrency && tuple.Item2 == firstCurrency));
        }

        public void CreateOrUpdateCacheFromDataTable(DataTable dTwithDuplicate)
        {
            try
            {
                if (dTwithDuplicate.Columns.Contains("FromCurrencyID") && dTwithDuplicate.Columns.Contains("ToCurrencyID"))
                {
                    string[] tobeDistinct = { "FromCurrencyID", "ToCurrencyID" };
                    DataTable dtDistinct = GetDistinctRecords(dTwithDuplicate, tobeDistinct);
                    _currencyPairConcurrentBag = new ConcurrentBag<Tuple<int, int>>();
                    foreach (DataRow row in dtDistinct.Rows)
                    {
                        int firstCurrencyID = Convert.ToInt32(row["FromCurrencyID"].ToString());
                        int secondCurrencyID = Convert.ToInt32(row["ToCurrencyID"].ToString());
                        AddCurrencyPair(firstCurrencyID, secondCurrencyID);
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

        public bool AddCurrencyPair(int firstCurrencyID, int secondCurrencyID)
        {
            try
            {
                if (!IsPairAvailable(firstCurrencyID, secondCurrencyID))
                {
                    _currencyPairConcurrentBag.Add(new Tuple<int, int>(firstCurrencyID, secondCurrencyID));
                    _currencyPairConcurrentBag.Add(new Tuple<int, int>(secondCurrencyID, firstCurrencyID));
                    return true;
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
            return false;
        }

        private DataTable GetDistinctRecords(DataTable dTwithDuplicate, string[] tobeDistinct)
        {
            DataTable dtUniqRecords = new DataTable();
            try
            {
                dtUniqRecords = dTwithDuplicate.DefaultView.ToTable(true, tobeDistinct);
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
            return dtUniqRecords;
        }
    }
}