using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CentralSMDataCache.DAL;
using Prana.Global;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.CentralSMDataCache
{
    public sealed class CentralSMCacheManager
    {
        private static volatile CentralSMCacheManager instance;
        private static object syncRoot = new Object();

        private CentralSMCacheManager()
        {
            try
            {
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static CentralSMCacheManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CentralSMCacheManager();
                    }
                }
                return instance;
            }
        }

        public void FillCachedData()
        {
            try
            {
                CachedData.Instance.GetPranaPreferences();
                CachedData.Instance.GetPranaKeyValuePairsSM();

                // Set UDA attributes in UDA data cache
                UDADataCache.GetInstance.SetCommonUDAData();
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

        public int GetCurrencyID(string currencyCode)
        {
            int id = int.MinValue;
            try
            {
                id = CachedData.Instance.Currencies.Where(pair => currencyCode.Equals(pair.Value, StringComparison.InvariantCultureIgnoreCase))
                                      .Select(pair => pair.Key).FirstOrDefault();
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
            return id;
        }

        public int GetAssetIdByAUECId(int auecID)
        {
            int value = 1;
            try
            {
                if (CachedData.Instance.AuecIdToAssetDict.TryGetValue(auecID, out value))
                {
                    return value;
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
            return 1;
        }

        public int GetCurrencyIdByAUECID(int auecID)
        {
            try
            {
                foreach (KeyValuePair<int, string> de in CachedData.Instance.Auecs)
                {
                    string[] auecInfo = de.Value.Split(',');
                    int tempCurrencyID = Convert.ToInt32(auecInfo[3]);
                    if (auecID == de.Key)
                    {
                        return tempCurrencyID;
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
            return int.MinValue;
        }

        public int GetExchangeIdFromAUECId(int auecID)
        {
            try
            {
                foreach (KeyValuePair<int, string> de in CachedData.Instance.Auecs)
                {
                    string[] auecInfo = de.Value.Split(',');
                    int tempExchangeId = Convert.ToInt32(auecInfo[2]);
                    if (auecID == de.Key)
                    {
                        return tempExchangeId;
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
            return int.MinValue;
        }

        public int GetUnderlyingID(int AUECID)
        {
            try
            {
                foreach (KeyValuePair<int, string> de in CachedData.Instance.Auecs)
                {
                    string[] auecInfo = de.Value.Split(',');
                    int tempUnderlyingId = Convert.ToInt32(auecInfo[1]);
                    if (AUECID == de.Key)
                    {
                        return tempUnderlyingId;
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
            return int.MinValue;
        }

        public int GetAUECIdByExchangeIdentifier(string exchangeIdentifier)
        {
            int ret = int.MinValue;
            try
            {
                if (CachedData.Instance.ExchangeIdentifiers.TryGetValue(exchangeIdentifier, out ret))
                {
                    return ret;
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
            return int.MinValue;
        }

        public string GetPranaPreferenceByKey(string key)
        {
            try
            {
                if (CachedData.Instance.PranaPreferences.ContainsKey(key))
                {
                    return CachedData.Instance.PranaPreferences[key];
                }
                else
                    return string.Empty;
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
                return string.Empty;
            }
        }

        public void GetUnderlyingExchangeIDFromAUECID(int AUECID, ref Underlying UnderlyingId, ref int ExchangeID)
        {
            ConcurrentDictionary<int, string> auecDict = CachedData.Instance.Auecs;
            if (auecDict.ContainsKey(AUECID))
            {
                string[] auecInfo = auecDict[AUECID].Split(',');
                UnderlyingId = (Underlying)Convert.ToInt32(auecInfo[1]);
                ExchangeID = Convert.ToInt32(auecInfo[2]);
                //CurrencyID = Convert.ToInt32(auecInfo[3]);
            }
            else
            {
                UnderlyingId = Underlying.None;// (Underlying)Convert.ToInt32(auecInfo[1]);
                ExchangeID = int.MinValue;
                //CurrencyID = int.MinValue;
            }
        }

        public string GetCurrencyText(int ID)
        {
            string retValue;
            try
            {
                if (CachedData.Instance.Currencies.TryGetValue(ID, out retValue))
                    return retValue;
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
                return "";
        }



        /// <summary>
        /// Get UDA ID From UDA Text 
        /// created by omshiv, May 30
        /// </summary>
        /// <param name="UDAText"></param>
        /// <param name="UDAType"></param>
        /// <returns></returns>
        public int GetUDAIDFromText(string UDAText, string UDAType)
        {
            int UDAID= 0;
            try
            {
               UDAID= UDADataCache.GetInstance.GetUDAIDFromText(UDAText, UDAType);
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
            return UDAID;
        }

        /// <summary>
        /// Save Attribute in DB and Cache
        /// </summary>
        /// <param name="udaDataCol"></param>
        public void SaveUDAAttributesData(Dictionary<string, Dictionary<string, object>> udaDataCol)
        {
            try
            {
                foreach (KeyValuePair<String, Dictionary<String, object>> udaTypeItem in udaDataCol)
                {
                    foreach (KeyValuePair<String, object> udaItem in udaTypeItem.Value)
                    {
                        if (udaItem.Key.Contains("Delete"))
                        {
                            List<int> udaIdsToDelete = udaItem.Value as List<int>;
                            if (udaIdsToDelete != null)
                            {
                                UDADataCache.GetInstance.RemoveDeletedUDAFrmCache(udaTypeItem.Key, udaIdsToDelete);
                                UDADataManager.DeleteInformation(udaItem.Key, udaIdsToDelete);
                            }
                        }
                        else if (udaItem.Key.Contains("Insert"))
                        {
                            UDACollection udaColToInsert = udaItem.Value as UDACollection;
                            if (udaColToInsert != null)
                            {
                                UDADataCache.GetInstance.AddUDAinCache(udaTypeItem.Key, udaColToInsert);
                                UDADataManager.SaveInformation(udaItem.Key, udaColToInsert);
                            }
                        }
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
