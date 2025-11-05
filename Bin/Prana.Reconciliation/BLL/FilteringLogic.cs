using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System.Data;
using Prana.Global;
using Prana.CommonDataCache;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
namespace Prana.Reconciliation
{
    public static class FilteringLogic
    {
        public static DataTable GetFilteredData(Dictionary<ReconFilterType, Dictionary<int, string>> dictFilters, DataTable dt)
        {
            if (dictFilters.Count > 0)
            {

                DataTable dtFiltered = new DataTable();
                try
                {
                dtFiltered = dt.Clone();

                List<int> listFundFilter = new List<int>();
                List<int> listAssetFilter = new List<int>();
                List<int> listCounterPartyFilter = new List<int>();
                List<int> listAUECFilter = new List<int>();

                foreach (KeyValuePair<ReconFilterType, Dictionary<int, string>> kp in dictFilters)
                {
                    switch (kp.Key)
                    {
                        //case ReconFilterType.PrimeBroker:
                        //case ReconFilterType.MasterFund:
                        //case ReconFilterType.Fund:

                        //    if (listFundFilter.Count == 0)
                        //    {
                        //        listFundFilter.AddRange(kp.Value.Keys);
                        //    }
                        //    else
                        //    {
                        //        if (listFundFilter.Count > kp.Value.Keys.Count)
                        //        {
                        //            listFundFilter.Clear();
                        //            listFundFilter.AddRange(kp.Value.Keys);
                        //        }
                        //    }
                        //    break;

                        case ReconFilterType.Asset:
                            listAssetFilter.AddRange(kp.Value.Keys);
                            break;
                        case ReconFilterType.AUEC:
                            listAUECFilter.AddRange(kp.Value.Keys);

                            break;
                        case ReconFilterType.CounterParty:
                            listCounterPartyFilter.AddRange(kp.Value.Keys);

                            break;
                        default:
                            break;
                    }

                }
                foreach (DataRow dr in dt.Rows)
                {
                    int AuecID = int.MinValue;
                    int AssetID = int.MinValue;
                    int fundID = int.MinValue;
                    int CounterPartyID = int.MinValue;


                    if (dr.Table.Columns.Contains("AUECID"))
                    {
                        AuecID = Convert.ToInt32(dr["AUECID"].ToString());
                    }

                    if (dr.Table.Columns.Contains("AssetID"))
                    {
                        AssetID = Convert.ToInt32(dr["AssetID"].ToString());
                    }

                    if (dr.Table.Columns.Contains("FundID"))
                    {
                        fundID = Convert.ToInt32(dr["FundID"].ToString());
                    }

                    if (dr.Table.Columns.Contains("CounterPartyID"))
                    {
                        CounterPartyID = Convert.ToInt32(dr["CounterPartyID"].ToString());
                    }

                    if ((listCounterPartyFilter.Contains(CounterPartyID) || listCounterPartyFilter.Count == 0 || CounterPartyID == int.MinValue) && (listAssetFilter.Contains(AssetID) || listAssetFilter.Count == 0 || AssetID == int.MinValue) && (listFundFilter.Contains(fundID) || listFundFilter.Count == 0 || fundID == int.MinValue) && (listAUECFilter.Contains(AuecID) || listAUECFilter.Count == 0 || AuecID == int.MinValue))
                    {
                        dtFiltered.Rows.Add(dr.ItemArray);
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
                return dtFiltered;
            }
            return dt;
        }


        public static DataTable GetFilteredPBData(Dictionary<ReconFilterType, Dictionary<int, string>> dictFilters, DataTable dt)
        {
            if (dictFilters.Count > 0)
            {
                try
                {
                DataTable dtFiltered = new DataTable();
                dtFiltered = dt.Clone();

                List<string> listFundFilter = new List<string>();
                List<string> listAssetFilter = new List<string>();
                List<string> listCounterPartyFilter = new List<string>();
                List<string> listAUECFilter = new List<string>();

                List<int> listSubAccountIDs = new List<int>();
                foreach (KeyValuePair<ReconFilterType, Dictionary<int, string>> kp in dictFilters)
                {
                    switch (kp.Key)
                    {
                        case ReconFilterType.Fund:

                            List<string> listFundNames = new List<string>();
                            listFundNames.AddRange(kp.Value.Values);

                            if (listFundFilter.Count >= listFundNames.Count)
                            {
                                listFundFilter.Clear();
                               
                            }
                            if (listFundFilter.Count == 0)
                            {
                                listFundFilter.AddRange(listFundNames);
                            }
                            break;

                        case ReconFilterType.PrimeBroker:
                            listSubAccountIDs = new List<int>();
                            // StringBuilder fundIDs = new StringBuilder();
                            Dictionary<int, List<int>> dictDataSourceSubAccountAssociation = CachedDataManager.GetInstance.GetDataSourceSubAccountAssociation();
                            List<int> dataSourceIds = new List<int>(dictFilters[ReconFilterType.PrimeBroker].Keys);
                            foreach (int dataSourceID in dataSourceIds)
                            {
                                if (dictDataSourceSubAccountAssociation.ContainsKey(dataSourceID))
                                {
                                    listSubAccountIDs.AddRange(dictDataSourceSubAccountAssociation[dataSourceID]);
                                }
                            }
                            if (listFundFilter.Count >= listSubAccountIDs.Count)
                            {
                                listFundFilter.Clear();
                                //foreach (int fundID in listSubAccountIDs)
                                //{
                                //    string fundName = CachedDataManager.GetInstance.GetFundText(fundID);
                                //    listFundFilter.Add(fundName);
                                //}
                            }
                           if (listFundFilter.Count == 0)
                            {
                            foreach (int fundID in listSubAccountIDs)
                            {
                                string fundName = CachedDataManager.GetInstance.GetFundText(fundID);
                                listFundFilter.Add(fundName);

                            }
                            }

                            break;
                        case ReconFilterType.MasterFund:

                            listSubAccountIDs = new List<int>();
                            Dictionary<int, List<int>> dictMasterFundSubAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                            List<int> listMasterFundIDs = new List<int>(dictFilters[ReconFilterType.MasterFund].Keys);
                            foreach (int masterFundID in listMasterFundIDs)
                            {
                                if (dictMasterFundSubAccountAssociation.ContainsKey(masterFundID))
                                {
                                    listSubAccountIDs.AddRange(dictMasterFundSubAccountAssociation[masterFundID]);
                                }
                            }
                            if (listFundFilter.Count >= listSubAccountIDs.Count)
                            {
                                listFundFilter.Clear();

                                //foreach (int fundID in listSubAccountIDs)
                                //{
                                //    string fundName = CachedDataManager.GetInstance.GetFundText(fundID);
                                //    listFundFilter.Add(fundName);

                                //}

                            }
                            if (listFundFilter.Count == 0)
                            {
                                foreach (int fundID in listSubAccountIDs)
                                {
                                    string fundName = CachedDataManager.GetInstance.GetFundText(fundID);
                                    listFundFilter.Add(fundName);

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
                    string FundName = string.Empty;
                    string CounterPartyName = string.Empty;

                    if (dr.Table.Columns.Contains("PBAssetName"))
                    {
                        PbAssetName = (dr["PbAssetName"].ToString());
                    }
                    if (dr.Table.Columns.Contains("FundName"))
                    {
                        FundName = (dr["FundName"].ToString());
                    }
                    if (dr.Table.Columns.Contains("CounterParty"))
                    {
                        CounterPartyName = (dr["CounterParty"].ToString());
                    }
                    if ((listAssetFilter.Contains(PbAssetName) || listAssetFilter.Count == 0 || PbAssetName == string.Empty) && (listFundFilter.Contains(FundName) || listFundFilter.Count == 0 || FundName == string.Empty)
                        && (listCounterPartyFilter.Contains(CounterPartyName) || listCounterPartyFilter.Count == 0 || CounterPartyName == string.Empty))
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
                    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
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
