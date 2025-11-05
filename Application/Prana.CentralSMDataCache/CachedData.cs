using Prana.CentralSMDataCache.DAL;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Prana.CentralSMDataCache
{
    class CachedData
    {
        #region Singleton

        private static volatile CachedData instance;
        private static object syncRoot = new Object();

        private CachedData() { }

        public static CachedData Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CachedData();
                    }
                }

                return instance;
            }
        }
        #endregion

        ConcurrentDictionary<string, string> _pranaPreferences = new ConcurrentDictionary<string, string>();

        public ConcurrentDictionary<string, string> PranaPreferences
        {
            get { return _pranaPreferences; }
            set { _pranaPreferences = value; }
        }

        ConcurrentDictionary<int, string> _currencies = new ConcurrentDictionary<int, string>();

        ConcurrentDictionary<int, int> _auecIdToAssetDict = new ConcurrentDictionary<int, int>();

        public ConcurrentDictionary<int, int> AuecIdToAssetDict
        {
            get { return _auecIdToAssetDict; }
            set { _auecIdToAssetDict = value; }
        }
        ConcurrentDictionary<string, int> _exchangeIdentifiers = new ConcurrentDictionary<string, int>();

        public ConcurrentDictionary<string, int> ExchangeIdentifiers
        {
            get { return _exchangeIdentifiers; }
            set { _exchangeIdentifiers = value; }
        }

        ConcurrentDictionary<int, string> _auecs = new ConcurrentDictionary<int, string>();

        public ConcurrentDictionary<int, string> Auecs
        {
            get { return _auecs; }
            set { _auecs = value; }
        }

        public ConcurrentDictionary<int, string> Currencies
        {
            get { return _currencies; }
            set { _currencies = value; }
        }

        public void GetPranaPreferences()
        {
            try
            {
                DataSet pranaPref = CentralSMMiscDBRequestManager.Instance.GetPranaPreference();
                foreach (DataRow row in pranaPref.Tables[0].Rows)
                {
                    string PreferenceKey = row["PreferenceKey"].ToString();
                    string PreferenceValue = row["PreferenceValue"].ToString();
                    if (_pranaPreferences == null)
                        _pranaPreferences = new ConcurrentDictionary<string, string>();
                    if (!_pranaPreferences.ContainsKey(PreferenceKey))
                        _pranaPreferences.TryAdd(PreferenceKey, PreferenceValue);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        

        public void GetPranaKeyValuePairsSM()
        {
            try
            {
                DataSet ds = CentralSMMiscDBRequestManager.Instance.GetPranaKeyValuePairsSM();
                if (ds != null && ds.Tables.Count > 2)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        _currencies.AddOrUpdate(Convert.ToInt32(dr[0].ToString()), dr[1].ToString(), (key, oldValue) => dr[1].ToString());
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        _auecIdToAssetDict.AddOrUpdate(Convert.ToInt32(dr[0].ToString()), Convert.ToInt32(dr[1].ToString()), (key, oldValue) => Convert.ToInt32(dr[1].ToString()));
                    }
                    int AUECID = 0;
                    int AssetID = 1;
                    int UnderlyingID = 2;
                    int ExchangeID = 3;
                    int CurrencyID = 4;
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        _auecs.AddOrUpdate(Convert.ToInt32(dr[AUECID].ToString()), dr[AssetID].ToString() + "," + dr[UnderlyingID].ToString() + "," + dr[ExchangeID].ToString() + "," + dr[CurrencyID].ToString(), (key, value) => dr[AssetID].ToString() + "," + dr[UnderlyingID].ToString() + "," + dr[ExchangeID].ToString() + "," + dr[CurrencyID].ToString());
                    }
                    foreach (DataRow dr in ds.Tables[3].Rows)
                    {
                        _exchangeIdentifiers.AddOrUpdate(dr[1].ToString(), Convert.ToInt32(dr[0].ToString()), (key, oldValue) => Convert.ToInt32(dr[0].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
