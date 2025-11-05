using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    public partial class MarketDataDetails : UserControl
    {
        private int _companyID = int.MinValue;

        public MarketDataDetails()
        {
            InitializeComponent();
        }

        public void LoadControl(int companyID)
        {
            try
            {
                if (_companyID != companyID)
                {
                    _companyID = companyID;
                    string[] marketDataProviders = Enum.GetNames(typeof(MarketDataProvider));
                    cmbFeedProvider.DataSource = marketDataProviders;
                    cmbSecondaryMarketDataProvider.DataSource = (from e in EnumHelper.ConvertEnumForBindingWithActualAssignedValuesWithCaption(typeof(SecondaryMarketDataProvider)) select e.DisplayText).ToList();

                    if (_companyID != int.MinValue)
                    {
                        QueryData queryData = new QueryData();
                        queryData.StoredProcedureName = "P_GetCompanyMarketDataProvider";
                        queryData.DictionaryDatabaseParameter = new Dictionary<string, DatabaseParameter>();
                        queryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                        {
                            ParameterName = "@CompanyId",
                            ParameterType = System.Data.DbType.Int32,
                            ParameterValue = CachedDataManager.GetInstance.LoggedInUser.CompanyID,
                            IsOutParameter = false
                        });
                        queryData.DictionaryDatabaseParameter.Add("@MarketDataProvider", new DatabaseParameter()
                        {
                            ParameterName = "@MarketDataProvider",
                            ParameterType = System.Data.DbType.Int32,
                            IsOutParameter = true
                        });

                        DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                        if (queryData.DictionaryDatabaseParameter["@MarketDataProvider"].ParameterValue != DBNull.Value)
                        {
                            int providerID = Convert.ToInt32(queryData.DictionaryDatabaseParameter["@MarketDataProvider"].ParameterValue);
                            string providerName = ((MarketDataProvider)providerID).ToString();
                            cmbFeedProvider.SelectedRow = cmbFeedProvider.Rows[Array.IndexOf(marketDataProviders, providerName)];
                        }
                        else
                            cmbFeedProvider.SelectedRow = cmbFeedProvider.Rows[0];

                        QueryData marketDataBlockedQueryData = new QueryData();
                        marketDataBlockedQueryData.StoredProcedureName = "P_GetMarketDataBlockedInformation";
                        marketDataBlockedQueryData.DictionaryDatabaseParameter = new Dictionary<string, DatabaseParameter>();
                        marketDataBlockedQueryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                        {
                            ParameterName = "@CompanyId",
                            ParameterType = System.Data.DbType.Int32,
                            ParameterValue = CachedDataManager.GetInstance.LoggedInUser.CompanyID,
                            IsOutParameter = false
                        });
                        marketDataBlockedQueryData.DictionaryDatabaseParameter.Add("@IsMarketDataBlocked", new DatabaseParameter()
                        {
                            ParameterName = "@IsMarketDataBlocked",
                            ParameterType = System.Data.DbType.Boolean,
                            IsOutParameter = true
                        });

                        DatabaseManager.DatabaseManager.ExecuteNonQuery(marketDataBlockedQueryData);

                        if (marketDataBlockedQueryData.DictionaryDatabaseParameter["@IsMarketDataBlocked"].ParameterValue != DBNull.Value)
                            chkBlockDataFromFeedProvider.Checked = Convert.ToBoolean(marketDataBlockedQueryData.DictionaryDatabaseParameter["@IsMarketDataBlocked"].ParameterValue);

                        QueryData factSetContractTypeQueryData = new QueryData();
                        factSetContractTypeQueryData.StoredProcedureName = "P_GetFactSetContractTypeQueryDataInformation";
                        factSetContractTypeQueryData.DictionaryDatabaseParameter = new Dictionary<string, DatabaseParameter>();
                        factSetContractTypeQueryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                        {
                            ParameterName = "@CompanyId",
                            ParameterType = System.Data.DbType.Int32,
                            ParameterValue = CachedDataManager.GetInstance.LoggedInUser.CompanyID,
                            IsOutParameter = false
                        });
                        factSetContractTypeQueryData.DictionaryDatabaseParameter.Add("@FactSetContractType", new DatabaseParameter()
                        {
                            ParameterName = "@FactSetContractType",
                            ParameterType = System.Data.DbType.Int32,
                            IsOutParameter = true
                        });

                        DatabaseManager.DatabaseManager.ExecuteNonQuery(factSetContractTypeQueryData);

                        if (factSetContractTypeQueryData.DictionaryDatabaseParameter["@FactSetContractType"].ParameterValue != DBNull.Value)
                        {
                            SetFactSetContractType(Convert.ToInt32(factSetContractTypeQueryData.DictionaryDatabaseParameter["@FactSetContractType"].ParameterValue));
                        }

                        QueryData secondaryQueryData = new QueryData();
                        secondaryQueryData.StoredProcedureName = "P_GetSecondaryCompanyMarketDataProvider";
                        secondaryQueryData.DictionaryDatabaseParameter = new Dictionary<string, DatabaseParameter>();
                        secondaryQueryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                        {
                            ParameterName = "@CompanyId",
                            ParameterType = System.Data.DbType.Int32,
                            ParameterValue = CachedDataManager.GetInstance.LoggedInUser.CompanyID,
                            IsOutParameter = false
                        });
                        secondaryQueryData.DictionaryDatabaseParameter.Add("@SecondaryMarketDataProvider", new DatabaseParameter()
                        {
                            ParameterName = "@SecondaryMarketDataProvider",
                            ParameterType = System.Data.DbType.Int32,
                            IsOutParameter = true
                        });

                        DatabaseManager.DatabaseManager.ExecuteNonQuery(secondaryQueryData);

                        if (secondaryQueryData.DictionaryDatabaseParameter["@SecondaryMarketDataProvider"].ParameterValue != DBNull.Value)
                            cmbSecondaryMarketDataProvider.SelectedRow = cmbSecondaryMarketDataProvider.Rows[Convert.ToInt32(secondaryQueryData.DictionaryDatabaseParameter["@SecondaryMarketDataProvider"].ParameterValue)];
                        else
                            cmbSecondaryMarketDataProvider.SelectedRow = cmbSecondaryMarketDataProvider.Rows[0];
                    }
                    else
                    {
                        cmbFeedProvider.SelectedRow = cmbFeedProvider.Rows[0];
                        cmbSecondaryMarketDataProvider.SelectedRow = cmbSecondaryMarketDataProvider.Rows[0];
                    }
                }
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
        }

        public void SaveControl()
        {
            try
            {
                if (cmbFeedProvider.SelectedRow != null)
                {
                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_SetCompanyMarketDataProvider";
                    queryData.DictionaryDatabaseParameter = new Dictionary<string, DatabaseParameter>();
                    queryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                    {
                        ParameterName = "@CompanyId",
                        ParameterType = System.Data.DbType.Int32,
                        ParameterValue = CachedDataManager.GetInstance.LoggedInUser.CompanyID,
                        IsOutParameter = false
                    });
                    queryData.DictionaryDatabaseParameter.Add("@MarketDataProvider", new DatabaseParameter()
                    {
                        ParameterName = "@MarketDataProvider",
                        ParameterType = System.Data.DbType.Int32,
                        ParameterValue = (int)Enum.Parse(typeof(MarketDataProvider), cmbFeedProvider.SelectedRow.Cells[0].Value.ToString()),
                        IsOutParameter = false
                    });
                    queryData.DictionaryDatabaseParameter.Add("@IsMarketDataBlocked", new DatabaseParameter()
                    {
                        ParameterName = "@IsMarketDataBlocked",
                        ParameterType = System.Data.DbType.Boolean,
                        ParameterValue = chkBlockDataFromFeedProvider.Checked,
                        IsOutParameter = false
                    });
                    queryData.DictionaryDatabaseParameter.Add("@FactSetContractType", new DatabaseParameter()
                    {
                        ParameterName = "@FactSetContractType",
                        ParameterType = System.Data.DbType.Int32,
                        ParameterValue = GetFactSetContractType(),
                        IsOutParameter = false
                    });

                    DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                    CommonDataCache.CachedDataManager.FetchCompanyMarketDataProvider();
                    CommonDataCache.CachedDataManager.FetchMarketDataBlockedInformation();
                    CommonDataCache.CachedDataManager.FetchFactSetContractType();
                }

                if (cmbSecondaryMarketDataProvider.SelectedRow != null)
                {
                    var selectedSecondaryMarketData = EnumHelper.GetValueFromEnumDescription<SecondaryMarketDataProvider>(cmbSecondaryMarketDataProvider.SelectedRow.Cells[0].Value.ToString());
                    QueryData secondaryQueryData = new QueryData();
                    secondaryQueryData.StoredProcedureName = "P_SetSecondaryCompanyMarketDataProvider";
                    secondaryQueryData.DictionaryDatabaseParameter = new Dictionary<string, DatabaseParameter>();
                    secondaryQueryData.DictionaryDatabaseParameter.Add("@CompanyId", new DatabaseParameter()
                    {
                        ParameterName = "@CompanyId",
                        ParameterType = System.Data.DbType.Int32,
                        ParameterValue = CachedDataManager.GetInstance.LoggedInUser.CompanyID,
                        IsOutParameter = false
                    });
                    secondaryQueryData.DictionaryDatabaseParameter.Add("@SecondaryMarketDataProvider", new DatabaseParameter()
                    {
                        ParameterName = "@SecondaryMarketDataProvider",
                        ParameterType = System.Data.DbType.Int32,
                        ParameterValue = (int)selectedSecondaryMarketData,
                        IsOutParameter = false
                    });

                    DatabaseManager.DatabaseManager.ExecuteNonQuery(secondaryQueryData);
                    CommonDataCache.CachedDataManager.FetchSecondaryCompanyMarketDataProvider();
                }

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
        }

        private void cmbFeedProvider_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                chkBlockDataFromFeedProvider.Checked = true;
                if (cmbFeedProvider.Value.Equals("SAPI"))
                {
                    this.chkBlockDataFromFeedProvider.Visible = true;
                    this.groupBoxFactSetContractType.Visible = false;
                }
                else if (cmbFeedProvider.Value.Equals("FactSet"))
                {
                    this.groupBoxFactSetContractType.Visible = true;
                    this.chkBlockDataFromFeedProvider.Visible = false;
                }
                else
                {
                    this.chkBlockDataFromFeedProvider.Visible = false;
                    this.groupBoxFactSetContractType.Visible = false;

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetFactSetContractType(int factSetContractTypeInt)
        {
            try
            {
                FactSetContractType factSetContractType = (FactSetContractType)factSetContractTypeInt;
                if (factSetContractType.Equals(FactSetContractType.ChannelPartner))
                    radioButtonChannelPartner.Checked = true;
                else
                    radioButtonReseller.Checked = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private int GetFactSetContractType()
        {
            int factSetContractTypeInt = 0;
            try
            {
                FactSetContractType factSetContractType = FactSetContractType.Reseller;
                if (radioButtonChannelPartner.Checked)
                    factSetContractType = FactSetContractType.ChannelPartner;
                factSetContractTypeInt = (int)factSetContractType;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return factSetContractTypeInt;
        }
    }
}
