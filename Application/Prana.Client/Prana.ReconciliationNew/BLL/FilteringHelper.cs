using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ReconciliationNew
{
    public static class FilteringLogic
    {

        #region commented
        /// <summary>
        /// Input: Filter parameters and datatable
        /// Output: datatable
        /// </summary>
        /// <param name="filterParameters"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        //        public static DataTable GetFilteredData(Dictionary<Prana.NewRecon.Enums.ColumnName, List<string>> filterParameters, DataTable dt)
        //        {
        //            #region commented
        //            //if (dictFilters.Count > 0)
        //            //{
        //            //    try
        //            //    {
        //            //        DataTable dtFiltered = new DataTable();
        //            //        dtFiltered = dt.Clone();

        //            //        List<string> listAccountFilter = new List<string>();
        //            //        List<string> listAssetFilter = new List<string>();
        //            //        List<string> listCounterPartyFilter = new List<string>();
        //            //        List<string> listAUECFilter = new List<string>();

        //            //        List<int> listSubAccountIDs = new List<int>();
        //            //        foreach (KeyValuePair<ReconFilterType, Dictionary<int, string>> kp in dictFilters)
        //            //        {
        //            //            switch (kp.Key)
        //            //            {
        //            //                case ReconFilterType.Account:

        //            //                    List<string> listAccountNames = new List<string>();
        //            //                    listAccountNames.AddRange(kp.Value.Values);

        //            //                    if (listAccountFilter.Count >= listAccountNames.Count)
        //            //                    {
        //            //                        listAccountFilter.Clear();

        //            //                    }
        //            //                    if (listAccountFilter.Count == 0)
        //            //                    {
        //            //                        listAccountFilter.AddRange(listAccountNames);
        //            //                    }
        //            //                    break;

        //            //                case ReconFilterType.PrimeBroker:
        //            //                    listSubAccountIDs = new List<int>();
        //            //                    // StringBuilder accountIDs = new StringBuilder();
        //            //                    Dictionary<int, List<int>> dictDataSourceSubAccountAssociation = CachedDataManager.GetInstance.GetDataSourceSubAccountAssociation();
        //            //                    List<int> thirdPartyIDs = new List<int>(dictFilters[ReconFilterType.PrimeBroker].Keys);
        //            //                    foreach (int thirdPartyID in thirdPartyIDs)
        //            //                    {
        //            //                        if (dictDataSourceSubAccountAssociation.ContainsKey(thirdPartyID))
        //            //                        {
        //            //                            listSubAccountIDs.AddRange(dictDataSourceSubAccountAssociation[thirdPartyID]);
        //            //                        }
        //            //                    }
        //            //                    if (listAccountFilter.Count >= listSubAccountIDs.Count)
        //            //                    {
        //            //                        listAccountFilter.Clear();
        //            //                        //foreach (int accountID in listSubAccountIDs)
        //            //                        //{
        //            //                        //    string accountName = CachedDataManager.GetInstance.GetAccountText(accountID);
        //            //                        //    listAccountFilter.Add(accountName);
        //            //                        //}
        //            //                    }
        //            //                    if (listAccountFilter.Count == 0)
        //            //                    {
        //            //                        foreach (int accountID in listSubAccountIDs)
        //            //                        {
        //            //                            string accountName = CachedDataManager.GetInstance.GetAccountText(accountID);
        //            //                            listAccountFilter.Add(accountName);

        //            //                        }
        //            //                    }

        //            //                    break;
        //            //                case ReconFilterType.MasterFund:

        //            //                    listSubAccountIDs = new List<int>();
        //            //                    Dictionary<int, List<int>> dictMasterFundSubAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
        //            //                    List<int> listMasterFundIDs = new List<int>(dictFilters[ReconFilterType.MasterFund].Keys);
        //            //                    foreach (int masterFundID in listMasterFundIDs)
        //            //                    {
        //            //                        if (dictMasterFundSubAccountAssociation.ContainsKey(masterFundID))
        //            //                        {
        //            //                            listSubAccountIDs.AddRange(dictMasterFundSubAccountAssociation[masterFundID]);
        //            //                        }
        //            //                    }
        //            //                    if (listAccountFilter.Count >= listSubAccountIDs.Count)
        //            //                    {
        //            //                        listAccountFilter.Clear();

        //            //                        //foreach (int accountID in listSubAccountIDs)
        //            //                        //{
        //            //                        //    string accountName = CachedDataManager.GetInstance.GetAccountText(accountID);
        //            //                        //    listAccountFilter.Add(accountName);

        //            //                        //}

        //            //                    }
        //            //                    if (listAccountFilter.Count == 0)
        //            //                    {
        //            //                        foreach (int accountID in listSubAccountIDs)
        //            //                        {
        //            //                            string accountName = CachedDataManager.GetInstance.GetAccountText(accountID);
        //            //                            listAccountFilter.Add(accountName);

        //            //                        }
        //            //                    }
        //            //                    break;

        //            //                case ReconFilterType.Asset:
        //            //                    listAssetFilter.AddRange(kp.Value.Values);
        //            //                    break;
        //            //                case ReconFilterType.AUEC:
        //            //                    listAUECFilter.AddRange(kp.Value.Values);

        //            //                    break;
        //            //                case ReconFilterType.CounterParty:
        //            //                    listCounterPartyFilter.AddRange(kp.Value.Values);

        //            //                    break;
        //            //                default:
        //            //                    break;
        //            //            }

        //            //        }
        //            //        foreach (DataRow dr in dt.Rows)
        //            //        {

        //            //            string PbAssetName = string.Empty;
        //            //            string AccountName = string.Empty;
        //            //            string CounterPartyName = string.Empty;

        //            //            if (dr.Table.Columns.Contains("PBAssetName"))
        //            //            {
        //            //                PbAssetName = (dr["PbAssetName"].ToString());
        //            //            }
        //            //            if (dr.Table.Columns.Contains("AccountName"))
        //            //            {
        //            //                AccountName = (dr["AccountName"].ToString());
        //            //            }
        //            //            if (dr.Table.Columns.Contains("CounterParty"))
        //            //            {
        //            //                CounterPartyName = (dr["CounterParty"].ToString());
        //            //            }
        //            //            if ((listAssetFilter.Contains(PbAssetName) || listAssetFilter.Count == 0 || PbAssetName == string.Empty) && (listAccountFilter.Contains(AccountName) || listAccountFilter.Count == 0 || AccountName == string.Empty)
        //            //                && (listCounterPartyFilter.Contains(CounterPartyName) || listCounterPartyFilter.Count == 0 || CounterPartyName == string.Empty))
        //            //            {
        //            //                dtFiltered.Rows.Add(dr.ItemArray);
        //            //            }
        //            //        }
        //            //        return dtFiltered;
        //            //    }
        //            //    catch (Exception ex)
        //            //    {

        //            //        // Invoke our policy that is responsible for making sure no secure information
        //            //        // gets out of our layer.
        //            //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
        //            //        if (rethrow)
        //            //        {
        //            //            throw;
        //            //        }
        //            //    }

        //            //}
        //#endregion

        //            try
        //            {
        //                DataTable dtFiltered = new DataTable();
        //                dtFiltered = dt.Clone();

        //                foreach (DataRow dr in dt.Rows)
        //                {
        //                    bool isMatched = true;
        //                    foreach (KeyValuePair<Prana.NewRecon.Enums.ColumnName, List<string>> item in filterParameters)
        //                    {
        //                        if (!(dr.Table.Columns.Contains(item.Key.ToString()) && item.Value.Contains(item.Key.ToString())))
        //                        {
        //                            isMatched = false;
        //                            continue;
        //                        }
        //                        if (isMatched)
        //                        {
        //                            dtFiltered.Rows.Add(dr.ItemArray);
        //                        }
        //                    }

        //                    return dtFiltered;
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //                // Invoke our policy that is responsible for making sure no secure information
        //                // gets out of our layer.
        //                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

        //                if (rethrow)
        //                {
        //                    throw;
        //                }
        //            }


        //            return dt;

        //        }
        #endregion

        internal static DataTable GetFilteredData(Dictionary<ReconFilterType, Dictionary<int, string>> dictFilters, DataTable dt)
        {
            if (dictFilters.Count > 0)
            {
                try
                {
                    DataTable dtFiltered = new DataTable();
                    dtFiltered = dt.Clone();

                    List<string> listAccountFilter = new List<string>();
                    List<string> listAssetFilter = new List<string>();
                    List<string> listCounterPartyFilter = new List<string>();
                    List<string> listAUECFilter = new List<string>();

                    List<int> listAccountIDs = new List<int>();
                    foreach (KeyValuePair<ReconFilterType, Dictionary<int, string>> kp in dictFilters)
                    {
                        switch (kp.Key)
                        {
                            case ReconFilterType.Account:

                                List<string> listAccountNames = new List<string>();
                                listAccountNames.AddRange(kp.Value.Values);

                                if (listAccountFilter.Count >= listAccountNames.Count)
                                {
                                    listAccountFilter.Clear();

                                }
                                if (listAccountFilter.Count == 0)
                                {
                                    listAccountFilter.AddRange(listAccountNames);
                                }
                                break;

                            case ReconFilterType.PrimeBroker:
                                //listAccountIDs = new List<int>();
                                //// StringBuilder accountIDs = new StringBuilder();
                                //Dictionary<int, List<int>> dictDataSourceSubAccountAssociation = CachedDataManagerRecon.GetInstance.;
                                //List<int> thirdPartyIDs = new List<int>(dictFilters[ReconFilterType.PrimeBroker].Keys);
                                //foreach (int thirdPartyID in thirdPartyIDs)
                                //{
                                //    if (dictDataSourceSubAccountAssociation.ContainsKey(thirdPartyID))
                                //    {
                                //        listAccountIDs.AddRange(dictDataSourceSubAccountAssociation[thirdPartyID]);
                                //    }
                                //}
                                //if (listAccountFilter.Count >= listAccountIDs.Count)
                                //{
                                //    listAccountFilter.Clear();
                                //    //foreach (int accountID in listSubAccountIDs)
                                //    //{
                                //    //    string accountName = CachedDataManager.GetInstance.GetAccountText(accountID);
                                //    //    listAccountFilter.Add(accountName);
                                //    //}
                                //}
                                //if (listAccountFilter.Count == 0)
                                //{
                                //    foreach (int accountID in listAccountIDs)
                                //    {
                                //        string accountName = CachedDataManager.GetInstance.GetAccountText(accountID);
                                //        listAccountFilter.Add(accountName);

                                //    }
                                //}

                                break;
                            case ReconFilterType.MasterFund:

                                listAccountIDs = new List<int>();
                                Dictionary<int, List<int>> dictMasterFundSubAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                                List<int> listMasterFundIDs = new List<int>(dictFilters[ReconFilterType.MasterFund].Keys);
                                foreach (int masterFundID in listMasterFundIDs)
                                {
                                    if (dictMasterFundSubAccountAssociation.ContainsKey(masterFundID))
                                    {
                                        listAccountIDs.AddRange(dictMasterFundSubAccountAssociation[masterFundID]);
                                    }
                                }
                                if (listAccountFilter.Count >= listAccountIDs.Count)
                                {
                                    listAccountFilter.Clear();

                                    //foreach (int accountID in listSubAccountIDs)
                                    //{
                                    //    string accountName = CachedDataManager.GetInstance.GetAccountText(accountID);
                                    //    listAccountFilter.Add(accountName);

                                    //}

                                }
                                if (listAccountFilter.Count == 0)
                                {
                                    foreach (int accountID in listAccountIDs)
                                    {
                                        string accountName = CachedDataManager.GetInstance.GetAccountText(accountID);
                                        listAccountFilter.Add(accountName);

                                    }
                                }
                                break;

                            case ReconFilterType.Asset:
                                listAssetFilter.AddRange(kp.Value.Values);
                                break;
                            case ReconFilterType.AUEC:
                                listAUECFilter.AddRange(kp.Value.Values);

                                break;
                            case ReconFilterType.CounterParty:
                                listCounterPartyFilter.AddRange(kp.Value.Values);

                                break;
                            default:
                                break;
                        }

                    }
                    foreach (DataRow dr in dt.Rows)
                    {

                        string PbAssetName = string.Empty;
                        string AccountName = string.Empty;
                        string CounterPartyName = string.Empty;

                        if (dr.Table.Columns.Contains("Asset"))
                        {
                            PbAssetName = (dr["Asset"].ToString());
                        }
                        if (dr.Table.Columns.Contains("AccountName"))
                        {
                            AccountName = (dr["AccountName"].ToString());
                        }
                        if (dr.Table.Columns.Contains("CounterParty"))
                        {
                            CounterPartyName = (dr["CounterParty"].ToString());
                        }
                        if ((listAssetFilter.Contains(PbAssetName) || listAssetFilter.Count == 0 || string.IsNullOrWhiteSpace(PbAssetName))
                            && (listAccountFilter.Contains(AccountName) || listAccountFilter.Count == 0 || string.IsNullOrWhiteSpace(AccountName))
                            && (listCounterPartyFilter.Contains(CounterPartyName) || listCounterPartyFilter.Count == 0 || string.IsNullOrWhiteSpace(CounterPartyName)))
                        {
                            dtFiltered.Rows.Add(dr.ItemArray);
                        }
                    }
                    return dtFiltered;
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

            return dt;

        }
    }
}
