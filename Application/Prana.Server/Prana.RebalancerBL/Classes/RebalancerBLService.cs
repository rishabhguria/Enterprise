using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.RebalancerBL.Cache;
using Prana.RebalancerBL.DAL;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Prana.RebalancerBL
{
    /// <summary>
    /// BL service for Rebalancer
    /// </summary>
    /// <seealso cref="Prana.Interfaces.IRebalancerBLService" />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class RebalancerBLService : IRebalancerBLService
    {

        #region Custom Group

        public bool DeleteCustomGroupMapping(int customGroupId)
        {
            bool result = RebalancerDBManager.GetInstance().DeleteCustomGroupMapping(customGroupId);
            if (result)
            {
                List<int> deletedCustomGroupMapping = new List<int>();
                deletedCustomGroupMapping.Add(customGroupId);
                PublishSavedData(deletedCustomGroupMapping, Topics.Topic_Rebalancer_CustomGroup);
            }
            return result;
        }

        public Dictionary<int, List<int>> GetAllCustomFundGroupsMapping()
        {
            Dictionary<int, List<int>> dictCustomFundGroupsMapping = new Dictionary<int, List<int>>();
            DataSet customFundGroupsMapping = RebalancerDBManager.GetInstance().GetAllCustomFundGroupsMapping();
            if (customFundGroupsMapping != null && customFundGroupsMapping.Tables.Count > 0 && customFundGroupsMapping.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in customFundGroupsMapping.Tables[0].Rows)
                {
                    int fundGroupId;
                    int fundId;
                    if (int.TryParse(row["FundGroupId"].ToString(), out fundGroupId) &&
                        int.TryParse(row["FundId"].ToString(), out fundId))
                    {
                        if (dictCustomFundGroupsMapping.ContainsKey(fundGroupId))
                            dictCustomFundGroupsMapping[fundGroupId].Add(fundId);
                        else
                            dictCustomFundGroupsMapping.Add(fundGroupId, new List<int> { fundId });
                    }
                }
            }
            return dictCustomFundGroupsMapping;
        }

        public Dictionary<int, string> GetAllCustomFundGroups()
        {
            Dictionary<int, string> customFundGroups = new Dictionary<int, string>();
            DataSet customFundGroupsDataSet = RebalancerDBManager.GetInstance().GetCustomFundGroups();
            if (customFundGroupsDataSet != null && customFundGroupsDataSet.Tables.Count > 0 && customFundGroupsDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in customFundGroupsDataSet.Tables[0].Rows)
                {
                    int fundGroupId;
                    if (int.TryParse(row["FundGroupId"].ToString(), out fundGroupId))
                    {
                        if (customFundGroups.ContainsKey(fundGroupId))
                            customFundGroups[fundGroupId] = row["GroupName"].ToString();
                        else
                            customFundGroups.Add(fundGroupId, row["GroupName"].ToString());
                    }
                }
            }
            return customFundGroups;
        }

        public bool SaveCustomGroupMapping(CustomGroupDto customGroupDto)
        {
            DataSet dsMapping = GetDataSetFromList(customGroupDto.GroupID, customGroupDto.FundGroupMapping);
            String xmlDoc = dsMapping.GetXml();
            int groupId = customGroupDto.GroupID;
            if (RebalancerDBManager.GetInstance().SaveCustomGroupMapping(xmlDoc, ref groupId, customGroupDto.GroupName))
            {
                customGroupDto.GroupID = groupId;
                List<CustomGroupDto> customGroupMappingPublishingData = new List<CustomGroupDto> { customGroupDto };
                PublishSavedData(customGroupMappingPublishingData, Topics.Topic_Rebalancer_CustomGroup);
                return true;
            }

            return false;
        }

        #endregion

        #region Model Portfolio

        public List<ModelPortfolioDto> GetModelPortfolios()
        {
            List<ModelPortfolioDto> lstModelPortfolios = null;
            DataSet dsModelPortfolios = RebalancerDBManager.GetInstance().GetModelPortfolios();
            if (dsModelPortfolios != null && dsModelPortfolios.Tables.Count > 0 && dsModelPortfolios.Tables[0].Rows.Count > 0)
            {
                lstModelPortfolios = new List<ModelPortfolioDto>();
                foreach (DataRow row in dsModelPortfolios.Tables[0].Rows)
                {
                    ModelPortfolioDto modelPortfolioDto = new ModelPortfolioDto()
                    {
                        Id = int.Parse(row["ModelPortfolioId"].ToString()),
                        ModelPortfolioName = row["ModelPortfolioName"].ToString(),
                        ModelPortfolioType =
                            (RebalancerEnums.ModelPortfolioType)
                            (int.Parse(row["ModelPortfolioTypeId"].ToString())),
                        ReferenceId = row["ReferenceId"].Equals(DBNull.Value)
                            ? 0
                            : int.Parse(row["ReferenceId"].ToString()),
                        ModelPortfolioData = row["ModelPortfolioData"].ToString(),
                        PositionsType = row["PositionsTypeId"].Equals(DBNull.Value)
                            ? RebalancerEnums.RebalancerPositionsType.RealTimePositions
                            : (RebalancerEnums.RebalancerPositionsType)(int.Parse(row["PositionsTypeId"].ToString())),
                        UseTolerance = row["UseToleranceId"].Equals(DBNull.Value)
                            ? RebalancerEnums.UseTolerance.No
                            : (RebalancerEnums.UseTolerance)(int.Parse(row["UseToleranceId"].ToString())),
                        ToleranceFactor = row["ToleranceFactorId"].Equals(DBNull.Value)
                            ? RebalancerEnums.ToleranceFactor.InPercentage
                            : (RebalancerEnums.ToleranceFactor)(int.Parse(row["ToleranceFactorId"].ToString())),
                        TargetPercentType = row["TargetPercentTypeId"].Equals(DBNull.Value)
                            ? RebalancerEnums.TargetPercentType.ModelTargetPercent
                            : (RebalancerEnums.TargetPercentType)(int.Parse(row["TargetPercentTypeId"].ToString()))
                    };
                    lstModelPortfolios.Add(modelPortfolioDto);
                }
            }
            return lstModelPortfolios;
        }

        public bool SaveEditModelPortfolio(ModelPortfolioDto modelPortfolioDto, bool isEdit)
        {
            ModelPortfolioDto savedModelPortfolioDto = RebalancerDBManager.GetInstance().SaveEditModelPortfolio(modelPortfolioDto, isEdit);
            List<ModelPortfolioDto> savedModelPortfolioDtos = new List<ModelPortfolioDto>();
            savedModelPortfolioDtos.Add(savedModelPortfolioDto);
            PublishSavedData(savedModelPortfolioDtos, Topics.Topic_Rebalancer_ModelPortfolio);
            return true;
        }

        public bool DeleteModelPortfolio(int modelPortfolioId)
        {
            bool result = RebalancerDBManager.GetInstance().DeleteModelPortfolio(modelPortfolioId);
            if (result)
            {
                List<int> deletedModelPortfolios = new List<int>();
                deletedModelPortfolios.Add(modelPortfolioId);
                PublishSavedData(deletedModelPortfolios, Topics.Topic_Rebalancer_ModelPortfolio);
            }
            return result;
        }

        #endregion

        private DataSet GetDataSetFromList(int fundGroupId, List<int> fundGroupMapping)
        {
            DataTable dt = new DataTable("TABFundGroupMapping");
            dt.Columns.Add("GroupId", typeof(int));
            dt.Columns.Add("FundId", typeof(int));
            DataSet ds = new DataSet("DSFundGroupMapping");
            try
            {
                foreach (int fId in fundGroupMapping)
                {
                    dt.Rows.Add(fundGroupId, fId);
                }
                ds.Tables.Add(dt);
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
            return ds;
        }

        private DataSet GetDataSetFromPreferenceDictionary(Dictionary<int, string> preferenceDictionary)
        {
            DataTable dt = new DataTable("TABPreferenceMapping");
            dt.Columns.Add("AccountId", typeof(int));
            dt.Columns.Add("PreferenceValue", typeof(string));
            DataSet ds = new DataSet("DSPreferenceMapping");
            try
            {
                foreach (KeyValuePair<int, string> preferenceKeyValuePair in preferenceDictionary)
                {
                    dt.Rows.Add(preferenceKeyValuePair.Key, preferenceKeyValuePair.Value);
                }
                ds.Tables.Add(dt);
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
            return ds;
        }

        #region publishing

        private void PublishSavedData<T>(List<T> dataToSend, string topic)
        {
            CreatePublishingProxy();
            MessageData data = new MessageData { EventData = dataToSend, TopicName = topic };
            _proxy.InnerChannel.Publish(data, data.TopicName);
        }

        static ProxyBase<IPublishing> _proxy;
        private void CreatePublishingProxy()
        {
            try
            {
                if (_proxy == null)
                {
                    _proxy = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
                }
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion

        #region Rebal Preferences

        public bool UpdateRebalPreferences(RebalPreferencesDto rebalPreferences)
        {
            return RebalancerDBManager.GetInstance().UpdateRebalPreferences(rebalPreferences);
        }

        public bool UpdateRebalPreferencesForAllAccounts(string preferenceKey, Dictionary<int, string> preferenceDictionary)
        {
            DataSet dsMapping = GetDataSetFromPreferenceDictionary(preferenceDictionary);
            String xmlDoc = dsMapping.GetXml();
            return RebalancerDBManager.GetInstance()
                .UpdateRebalPreferencesForAllAccounts(preferenceKey, xmlDoc);
        }

        public Dictionary<Tuple<int, string>, string> GetRebalPreferences()
        {
            Dictionary<Tuple<int, string>, string> rebalPreferences = new Dictionary<Tuple<int, string>, string>();
            try
            {
                DataSet rebalPreferencesDataSet = RebalancerDBManager.GetInstance().GetRebalPreferences();
                if (rebalPreferencesDataSet != null && rebalPreferencesDataSet.Tables.Count > 0 && rebalPreferencesDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in rebalPreferencesDataSet.Tables[0].Rows)
                    {
                        var key = Tuple.Create(int.Parse(row["AccountId"].ToString()), row["PreferenceKey"].ToString());
                        if (rebalPreferences.ContainsKey(key))
                            rebalPreferences[key] = row["PreferenceValue"].ToString();
                        else
                            rebalPreferences.Add(key, row["PreferenceValue"].ToString());
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
            return rebalPreferences;
        }

        public bool SaveRebalancerTradeList(DateTime selectedDate, string smartName, int id, string strtradeList)
        {
            return RebalancerDBManager.GetInstance().SaveRebalancerTradeList(selectedDate, smartName, id, strtradeList);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetRebalancerTradeListNames(DateTime selectedDate)
        {
            Dictionary<string, int> smartFileNameDict = new Dictionary<string, int>();
            DataSet ds = RebalancerDBManager.GetInstance().GetRebalancerTradeListNames(selectedDate);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int tradeListId;
                    if (int.TryParse(row["TradeListId"].ToString(), out tradeListId) && !string.IsNullOrWhiteSpace((row["TradeListName"].ToString())))
                    {
                        if (!smartFileNameDict.ContainsKey(row["TradeListName"].ToString()))
                        {
                            smartFileNameDict.Add(row["TradeListName"].ToString(), tradeListId);
                        }
                    }
                }
            if (RebalancerCache.GetInstance().SmartNameCount < 0)
            {
                RebalancerCache.GetInstance().SmartNameCount = smartFileNameDict.Count;
            }
            return smartFileNameDict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tradeListId"></param>
        /// <returns></returns>
        public string GetTradeList(int tradeListId)
        {
            if (RebalancerCache.GetInstance().TradeListDataDict.ContainsKey(tradeListId))
            {
                return RebalancerCache.GetInstance().TradeListDataDict[tradeListId];
            }
            string tradeListData = string.Empty;
            DataSet ds = RebalancerDBManager.GetInstance().GetRebalancerTradeListNames(tradeListId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                if (!string.IsNullOrWhiteSpace((row["TradeListName"].ToString())))
                {
                    tradeListData = row["TradeListData"].ToString();
                }
            }
            return tradeListData;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSmartName()
        {
            String name = string.Empty;
            Dictionary<string, int> dict = GetRebalancerTradeListNames(DateTime.Now.Date);
            if (RebalancerCache.GetInstance().SmartNameCount < 0)
            {
                RebalancerCache.GetInstance().SmartNameCount = dict.Count;
            }
            while (true)
            {
                name = String.Format("Rebal_{0}_{1}", DateTime.Now.Date.ToString("MM/dd/yyyy"),
                    ++RebalancerCache.GetInstance().SmartNameCount);
                if (!dict.ContainsKey(name.ToString()))
                {
                    return name.ToString();
                }
            }
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion
    }
}
