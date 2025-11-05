using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.RebalancerBL.Cache;
using System;
using System.Data;

namespace Prana.RebalancerBL.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class RebalancerDBManager
    {
        #region Singleton Constructor Implementation.
        private static RebalancerDBManager _instance;
        private int _maxPortfolioId;
        private int _maxTradeListId;
        private static readonly object lockObject = new object();
        private RebalancerDBManager()
        {
            _maxPortfolioId = Int32.MinValue;
            _maxTradeListId = Int32.MinValue;
        }

        public static RebalancerDBManager GetInstance()
        {
            //ensure if Thread Safe
            if (_instance == null)
            {
                _instance = new RebalancerDBManager();
            }
            return _instance;
        }
        #endregion

        #region Model Portfolio tab

        public DataSet GetModelPortfolios()
        {
            DataSet result = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetModelPortfolios";

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return result;
        }

        private int GetMaxModelPortfolioId()
        {
            int result = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMaxModelPortfolioId";

                DataSet resultDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (resultDataSet != null && resultDataSet.Tables.Count > 0 &&
                    resultDataSet.Tables[0].Rows.Count > 0)
                {
                    Int32.TryParse(resultDataSet.Tables[0].Rows[0]["MaxModelPortfolioId"].ToString(), out result);
                }

                return result;
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
            return result;
        }

        public ModelPortfolioDto SaveEditModelPortfolio(ModelPortfolioDto modelPortfolioDto, bool isEdit)
        {
            try
            {
                lock (lockObject)
                {
                    if (_maxPortfolioId == Int32.MinValue)
                    {
                        _maxPortfolioId = GetMaxModelPortfolioId() + 1;
                    }

                    int modelPortfolioId = isEdit ? modelPortfolioDto.Id : _maxPortfolioId;

                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_SaveEditModelPortfolio";

                    queryData.DictionaryDatabaseParameter.Add("@ModelPortfolioId", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ModelPortfolioId",
                        ParameterType = DbType.Int32,
                        ParameterValue = modelPortfolioId
                    });
                    queryData.DictionaryDatabaseParameter.Add("@ModelPortfolioName", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ModelPortfolioName",
                        ParameterType = DbType.String,
                        ParameterValue = modelPortfolioDto.ModelPortfolioName
                    });
                    queryData.DictionaryDatabaseParameter.Add("@ModelPortfolioTypeId", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ModelPortfolioTypeId",
                        ParameterType = DbType.Int32,
                        ParameterValue = (int)modelPortfolioDto.ModelPortfolioType
                    });
                    queryData.DictionaryDatabaseParameter.Add("@ReferenceId", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ReferenceId",
                        ParameterType = DbType.Int32,
                        ParameterValue = modelPortfolioDto.ReferenceId
                    });
                    queryData.DictionaryDatabaseParameter.Add("@ModelPortfolioData", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ModelPortfolioData",
                        ParameterType = DbType.String,
                        ParameterValue = modelPortfolioDto.ModelPortfolioData
                    });
                    queryData.DictionaryDatabaseParameter.Add("@PositionsTypeId", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@PositionsTypeId",
                        ParameterType = DbType.Int32,
                        ParameterValue = modelPortfolioDto.PositionsType.HasValue ? (int)modelPortfolioDto.PositionsType.Value : (int?)null
                    });
                    queryData.DictionaryDatabaseParameter.Add("@UseToleranceId", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@UseToleranceId",
                        ParameterType = DbType.Int32,
                        ParameterValue = modelPortfolioDto.UseTolerance.HasValue ? (int)modelPortfolioDto.UseTolerance.Value : (int?)null
                    });
                    queryData.DictionaryDatabaseParameter.Add("@ToleranceFactorId", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@ToleranceFactorId",
                        ParameterType = DbType.Int32,
                        ParameterValue = modelPortfolioDto.ToleranceFactor.HasValue ? (int)modelPortfolioDto.ToleranceFactor.Value : (int?)null
                    });
                    queryData.DictionaryDatabaseParameter.Add("@TargetPercentTypeId", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@TargetPercentTypeId",
                        ParameterType = DbType.Int32,
                        ParameterValue = modelPortfolioDto.TargetPercentType.HasValue ? (int)modelPortfolioDto.TargetPercentType.Value : (int?)null
                    });

                    modelPortfolioDto.Id = modelPortfolioId;
                    int result = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                    if (result > 0)
                    {
                        if (!isEdit) _maxPortfolioId++;
                        return modelPortfolioDto;
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
            return modelPortfolioDto;
        }

        public bool DeleteModelPortfolio(int modelPortfolioId)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_DeleteModelPortfolio";
                queryData.DictionaryDatabaseParameter.Add("@ModelPortfolioId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ModelPortfolioId",
                    ParameterType = DbType.Int32,
                    ParameterValue = modelPortfolioId
                });

                int result = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                if (result > 0)
                {
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

        #endregion

        #region Custom Groups

        public bool DeleteCustomGroupMapping(int customGroupId)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_DeleteCustomFundGroupsMapping";
                queryData.DictionaryDatabaseParameter.Add("@customGroupId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@customGroupId",
                    ParameterType = DbType.Int32,
                    ParameterValue = customGroupId
                });

                int result = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                if (result > 0)
                {
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

        public bool SaveCustomGroupMapping(string xmlDataTable, ref int groupID, string groupName)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveFundGroupMapping";

                queryData.DictionaryDatabaseParameter.Add("@XMLDoc", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@XMLDoc",
                    ParameterType = DbType.String,
                    ParameterValue = xmlDataTable
                });
                queryData.DictionaryDatabaseParameter.Add("@groupID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@groupID",
                    ParameterType = DbType.Int32,
                    ParameterValue = groupID
                });
                queryData.DictionaryDatabaseParameter.Add("@groupName", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@groupName",
                    ParameterType = DbType.String,
                    ParameterValue = groupName
                });
                queryData.DictionaryDatabaseParameter.Add("@SavedFundId", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@SavedFundId",
                    ParameterType = DbType.Int32,
                    OutParameterSize = sizeof(Int32)
                });

                int result = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                if (result > 0)
                {
                    if ((int)queryData.DictionaryDatabaseParameter["@SavedFundId"].ParameterValue != 0)
                        groupID = (int)queryData.DictionaryDatabaseParameter["@SavedFundId"].ParameterValue;
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

        public DataSet GetAllCustomFundGroupsMapping()
        {
            DataSet result = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllCustomFundGroupsMapping";
                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return result;
        }

        public DataSet GetCustomFundGroups()
        {
            DataSet result = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllFundGroups";
                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

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
            return result;
        }

        public DataSet GetRebalPreferences()
        {
            DataSet result = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetRebalPreferences";
                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

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
            return result;
        }

        public bool UpdateRebalPreferences(RebalPreferencesDto rebalPreferences)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_UpdateRebalPreferences";
                queryData.DictionaryDatabaseParameter.Add("@rebalPreferenceKey", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@rebalPreferenceKey",
                    ParameterType = DbType.String,
                    ParameterValue = rebalPreferences.PreferenceKey
                });
                queryData.DictionaryDatabaseParameter.Add("@accountId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@accountId",
                    ParameterType = DbType.Int16,
                    ParameterValue = rebalPreferences.AccountId
                });
                queryData.DictionaryDatabaseParameter.Add("@rebalPreferenceValue", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@rebalPreferenceValue",
                    ParameterType = DbType.String,
                    ParameterValue = rebalPreferences.PreferenceValue
                });

                int result = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                return (result > 0);
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

        public bool UpdateRebalPreferencesForAllAccounts(string preferenceKey, string xmlDoc)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_UpdateRebalPreferencesForAllAccounts";
                queryData.DictionaryDatabaseParameter.Add("@rebalPreferenceKey", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@rebalPreferenceKey",
                    ParameterType = DbType.String,
                    ParameterValue = preferenceKey
                });
                queryData.DictionaryDatabaseParameter.Add("@rebalPreferenceValueXml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@rebalPreferenceValueXml",
                    ParameterType = DbType.String,
                    ParameterValue = xmlDoc
                });

                int result = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                return (result > 0);
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

        #endregion


        internal bool SaveRebalancerTradeList(DateTime selectedDate, string smartName, int id, string strtradeList)
        {
            try
            {
                if (_maxTradeListId == Int32.MinValue)
                {
                    _maxTradeListId = GetMaxRebalancerTradeListId() + 1;
                }
                id = id < 0 ? _maxTradeListId : id;
                if (RebalancerCache.GetInstance().TradeListDataDict.ContainsKey(id))
                {
                    RebalancerCache.GetInstance().TradeListDataDict[id] = strtradeList;
                }
                else
                {
                    RebalancerCache.GetInstance().TradeListDataDict.Add(id, strtradeList);
                }

                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveTradeListData";
                queryData.DictionaryDatabaseParameter.Add("@TradeListId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TradeListId",
                    ParameterType = DbType.Int32,
                    ParameterValue = id
                });
                queryData.DictionaryDatabaseParameter.Add("@TradeListDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TradeListDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = selectedDate
                });
                queryData.DictionaryDatabaseParameter.Add("@TradeListName", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TradeListName",
                    ParameterType = DbType.String,
                    ParameterValue = smartName
                });
                queryData.DictionaryDatabaseParameter.Add("@TradeListData", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TradeListData",
                    ParameterType = DbType.String,
                    ParameterValue = strtradeList
                });

                int result = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                if (result > 0)
                {
                    if (id == _maxTradeListId)
                        _maxTradeListId++;
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

        private int GetMaxRebalancerTradeListId()
        {
            int result = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMaxRebalancerTradeListId";
                DataSet resultDataSet = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (resultDataSet != null && resultDataSet.Tables.Count > 0 &&
                    resultDataSet.Tables[0].Rows.Count > 0)
                {
                    Int32.TryParse(resultDataSet.Tables[0].Rows[0]["MaxRebalancerTradeListId"].ToString(), out result);
                }

                return result;
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
            return result;
        }

        internal DataSet GetRebalancerTradeListNames(DateTime selectedDate)
        {
            DataSet result = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllTradeListsForADate";
                queryData.DictionaryDatabaseParameter.Add("@TradeListDate", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TradeListDate",
                    ParameterType = DbType.DateTime,
                    ParameterValue = selectedDate.Date
                });

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return result;
        }

        internal DataSet GetRebalancerTradeListNames(int tradeListId)
        {
            DataSet result = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetTradeListData";
                queryData.DictionaryDatabaseParameter.Add("@TradeListId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TradeListId",
                    ParameterType = DbType.Int32,
                    ParameterValue = tradeListId
                });

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return result;
        }

    }
}
