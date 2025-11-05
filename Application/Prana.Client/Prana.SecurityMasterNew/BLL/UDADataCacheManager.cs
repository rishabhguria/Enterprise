using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.LogManager;
using System;
namespace Prana.SecurityMasterNew.BLL
{
    internal class UDADataCacheManager
    {
        /// <summary>
        /// 
        /// </summary>
        //internal static void InitAccountWiseUDADataInCache()
        //{
        //    try
        //    {
        //        ConcurrentDictionary<string, UDAData> accountSymbolUDADict = new ConcurrentDictionary<string, UDAData>();
        //        DataSet udaDataDS = UDADataManager.GetAccountWiseUDAData();
        //        if (udaDataDS != null && udaDataDS.Tables.Count > 0)
        //        {
        //            udaDataDS.Tables[0].AsEnumerable().AsParallel().ForAll(row =>
        //            {

        //                UDAData udaData = new UDAData();
        //                bool isAproved = row["IsApproved"] == DBNull.Value ? Convert.ToBoolean(row["UDAAssetClassID"].ToString()) : false;
        //                if (isAproved)
        //                {
        //                    udaData.FillSymbolUDAData(row);
        //                    UpdateUDADataWithName(udaData);

        //                    String key = GetKey(udaData.AccountID, udaData.Symbol);
        //                    if (!String.IsNullOrWhiteSpace(key))
        //                        accountSymbolUDADict.TryAdd(key, udaData);
        //                }
        //            });
        //        }
        //        UDADataCache.GetInstance.AccountSymbolUDADataDict = accountSymbolUDADict;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private static string GetKey(int accountID, string symbol)
        {
            try
            {
                return accountID + symbol.Trim().ToUpper();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;

        }

        /// <summary>
        /// Updating Uda data with name object by UDA data IDs 
        /// </summary>
        /// <param name="secMasterobj"></param>
        internal static void UpdateUDADataWithName(UDAData udaSymbolData)
        {

            try
            {
                if (udaSymbolData != null)
                {

                    udaSymbolData.UDAAsset = UDADataCache.GetInstance.GetUDATextFromID(udaSymbolData.AssetID, SecMasterConstants.CONST_UDAAsset);
                    udaSymbolData.UDACountry = UDADataCache.GetInstance.GetUDATextFromID(udaSymbolData.CountryID, SecMasterConstants.CONST_UDACountry);
                    udaSymbolData.UDASector = UDADataCache.GetInstance.GetUDATextFromID(udaSymbolData.SectorID, SecMasterConstants.CONST_UDASector);
                    udaSymbolData.UDASecurityType = UDADataCache.GetInstance.GetUDATextFromID(udaSymbolData.SecurityTypeID, SecMasterConstants.CONST_UDASecurityType);
                    udaSymbolData.UDASubSector = UDADataCache.GetInstance.GetUDATextFromID(udaSymbolData.SubSectorID, SecMasterConstants.CONST_UDASubSector);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountWiseUDAData"></param>
        /// <returns></returns>
        //internal static void UpdateAccountWiseUDADataInCache(DataSet accountWiseUDAData)
        //{

        //    try
        //    {
        //        if (accountWiseUDAData != null && accountWiseUDAData.Tables.Count > 0)
        //        {
        //            accountWiseUDAData.Tables[0].AsEnumerable().AsParallel().ForAll(row =>
        //            {

        //                UDAData udaData = new UDAData();
        //                bool isAproved = row["IsApproved"] == DBNull.Value ? Convert.ToBoolean(row["UDAAssetClassID"].ToString()) : false;
        //                if (isAproved)
        //                {
        //                    udaData.FillSymbolUDAData(row);
        //                    UpdateUDADataWithName(udaData);

        //                    String key = GetKey(udaData.AccountID, udaData.Symbol);
        //                    if (UDADataCache.GetInstance.AccountSymbolUDADataDict.ContainsKey(key))
        //                        UDADataCache.GetInstance.AccountSymbolUDADataDict[key] = udaData;
        //                    else
        //                    {
        //                        UDADataCache.GetInstance.AccountSymbolUDADataDict.TryAdd(key, udaData);
        //                    }
        //                }
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}

        /// <summary>
        /// Check for account wise UDA available, if available then merge to secmasterbaseObject
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="secMasterobj"></param>
        /// <returns></returns>
        //internal static UDAData CheckUDAAvailableForAccount_Update(int accountID, String Symbol)
        //{
        //    try
        //    {
        //        String key = GetKey(accountID, Symbol);
        //        if (UDADataCache.GetInstance.AccountSymbolUDADataDict.ContainsKey(key))
        //        {
        //            return UDADataCache.GetInstance.AccountSymbolUDADataDict[key];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return null;

        //}

    }
}
