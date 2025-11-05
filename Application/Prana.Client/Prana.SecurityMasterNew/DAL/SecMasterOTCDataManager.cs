using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.SecurityMasterNew
{
    public class SecMasterOTCDataManager
    {
        private const string CONST_SMConnStringName = "SMConnectionString";

        /// <summary>
        /// Get OTC Templates
        /// </summary>
        /// <returns></returns>
        public static List<SecMasterOTCData> GetOTCTemplates(int instrumentTypeId = 0)
        {
            var otcTemplates = new List<SecMasterOTCData>();
            DataSet result = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_OTC_GetAllTemplates";
                queryData.DictionaryDatabaseParameter.Add("@InstrumentType", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@InstrumentType",
                    ParameterType = DbType.Int32,
                    ParameterValue = instrumentTypeId
                });

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, CONST_SMConnStringName);
                otcTemplates = GetOTCTemplatesFromDataset(result);
                return otcTemplates;
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
            return otcTemplates;

        }

        /// <summary>
        /// delete OTC Templates
        /// </summary>
        /// <returns></returns>
        public static int DeleteOTCTemplates(int templateID = 0)
        {
            int result = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_OTC_DeleteTemplate";
                queryData.DictionaryDatabaseParameter.Add("@TemplateID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@TemplateID",
                    ParameterType = DbType.Int32,
                    ParameterValue = templateID
                });

                var res = DatabaseManager.DatabaseManager.ExecuteScalar(queryData, CONST_SMConnStringName);
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

        /// <summary>
        /// Get OTC Templates Details
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        public static SecMasterOTCData GetOTCTemplatesDetails(int templateID)
        {

            var otcTemplates = new SecMasterOTCData();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_OTC_GetTemplateDetailedData";
                queryData.DictionaryDatabaseParameter.Add("@InstrumentTypeID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@InstrumentTypeID",
                    ParameterType = DbType.Int32,
                    ParameterValue = 1
                });
                queryData.DictionaryDatabaseParameter.Add("@OTCTemplateID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@OTCTemplateID",
                    ParameterType = DbType.Int32,
                    ParameterValue = templateID
                });

                DataSet result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, CONST_SMConnStringName);
                otcTemplates = GetOTCTemplateDetailFromDataset(result).FirstOrDefault();
                return otcTemplates;
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
            return otcTemplates;

        }

        private static List<SecMasterOTCData> GetOTCTemplateDetailFromDataset(DataSet result)
        {
            List<SecMasterOTCData> otcTemplates = new List<SecMasterOTCData>();

            if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in result.Tables[0].Rows)
                {
                    if (int.Parse(row["InstrumentType"].ToString()) == 1)
                    {
                        SecMasterOTCData euitySwapOTCTemplate = new SecMasterEquitySwap()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            Name = row["Name"].ToString(),
                            CreatedBy = int.Parse(row["CreatedBy"].ToString()),
                            CreationDate = Convert.ToDateTime(row["CreationDate"].ToString()),
                            Description = row["Description"].ToString(),
                            DaysToSettle = int.Parse(row["DaysToSettle"].ToString()),
                            InstrumentType = row["InstrumentType"].ToString(),
                            ISDAContract = row["ISDAContract"].ToString(),
                            ISDACounterParty = int.Parse(row["ISDACounterParty"].ToString()),
                            LastModifieDate = Convert.ToDateTime(row["LastModifieDate"].ToString()),
                            LastModifiedBy = int.Parse(row["LastModifiedBy"].ToString()),
                            UnderlyingAssetID = int.Parse(row["UnderlyingAssetID"].ToString()),
                            EquityLeg_Frequency = row["EquityLeg_Frequency"].ToString(),
                            EquityLeg_BulletSwap = Convert.ToBoolean(row["EquityLeg_BulletSwap"].ToString()),
                            EquityLeg_ExcludeDividends = Convert.ToBoolean(row["EquityLeg_ExcludeDividends"].ToString()),
                            EquityLeg_ImpliedCommission = Convert.ToBoolean(row["EquityLeg_ImpliedCommission"].ToString()),
                            CommissionBasis = row["Commission_Basis"].ToString(),
                            HardCommissionRate = float.Parse(row["Commission_HardCommissionRate"].ToString()),
                            SoftCommissionRate = float.Parse(row["Commission_SoftCommissionRate"].ToString()),
                            FinanceLeg_InterestRate = int.Parse(row["FinanceLeg_InterestRate"].ToString()),
                            FinanceLeg_SpreadBasisPoint = float.Parse(row["FinanceLeg_SpreadBasisPoint"].ToString()),
                            FinanceLeg_DayCount = int.Parse(row["FinanceLeg_DayCount"].ToString()),
                            FinanceLeg_Frequency = row["FinanceLeg_Frequency"].ToString(),
                            FinanceLeg_FixedRate = float.Parse(row["FinanceLeg_FixedRate"].ToString()),
                            CustomFields = row["CustomFields"] != DBNull.Value ? JsonHelper.DeserializeToList<OTCCustomFields>(row["CustomFields"].ToString()) : new List<OTCCustomFields>(),

                        };
                        otcTemplates.Add(euitySwapOTCTemplate);
                    }
                    else if (int.Parse(row["InstrumentType"].ToString()) == 2)
                    {
                        SecMasterOTCData euitySwapOTCTemplate = new SecMasterCFDData()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            Name = row["Name"].ToString(),
                            CreatedBy = int.Parse(row["CreatedBy"].ToString()),
                            CreationDate = Convert.ToDateTime(row["CreationDate"].ToString()),
                            Description = row["Description"].ToString(),
                            DaysToSettle = int.Parse(row["DaysToSettle"].ToString()),
                            InstrumentType = row["InstrumentType"].ToString(),
                            ISDAContract = row["ISDAContract"].ToString(),
                            ISDACounterParty = int.Parse(row["ISDACounterParty"].ToString()),
                            LastModifieDate = Convert.ToDateTime(row["LastModifieDate"].ToString()),
                            LastModifiedBy = int.Parse(row["LastModifiedBy"].ToString()),
                            UnderlyingAssetID = int.Parse(row["UnderlyingAssetID"].ToString()),
                            Collateral_DayCount = int.Parse(row["Collateral_DayCount"].ToString()),
                            Collateral_Margin = float.Parse(row["Collateral_Margin"].ToString()),
                            Collateral_Rate = float.Parse(row["Collateral_Rate"].ToString()),
                            Finance_DayCount = int.Parse(row["FinanceLeg_DayCount"].ToString()),
                            Finance_Fixedrate = float.Parse(row["FinanceLeg_FixedRate"].ToString()),
                            Finance_InteresrRatebenchmark = int.Parse(row["FinanceLeg_InterestRate"].ToString()),
                            Finance_ScriptlendingFee = float.Parse(row["FinanceLeg_ScriptLeadingFee"].ToString()),
                            Finance_SpreadBP = float.Parse(row["FinanceLeg_SpreadBasisPoint"].ToString()),
                            CFD_HardCommRate = float.Parse(row["Commission_HardCommissionRate"].ToString()),
                            CFD_SoftCommRate = float.Parse(row["Commission_SoftCommissionRate"].ToString()),
                            CFD_Commissionbasis = int.Parse(row["Commission_Basis"].ToString()),
                            CustomFields = row["CustomFields"] != DBNull.Value ? JsonHelper.DeserializeToList<OTCCustomFields>(row["CustomFields"].ToString()) : new List<OTCCustomFields>(),
                        };
                        otcTemplates.Add(euitySwapOTCTemplate);


                    }
                    else if (int.Parse(row["InstrumentType"].ToString()) == 3)
                    {
                        SecMasterOTCData OTCTempDataTemplate = new SecMasterConvertibleBondData()
                        {
                            Id = int.Parse(row["Id"].ToString()),
                            Name = row["Name"].ToString(),
                            CreatedBy = int.Parse(row["CreatedBy"].ToString()),
                            CreationDate = Convert.ToDateTime(row["CreationDate"].ToString()),
                            Description = row["Description"].ToString(),
                            InstrumentType = row["InstrumentType"].ToString(),
                            ISDACounterParty = int.Parse(row["ISDACounterParty"].ToString()),
                            LastModifieDate = Convert.ToDateTime(row["LastModifieDate"].ToString()),
                            LastModifiedBy = int.Parse(row["LastModifiedBy"].ToString()),
                            UnderlyingAssetID = int.Parse(row["UnderlyingAssetID"].ToString()),
                            FinanceLeg_IRBenchMark = int.Parse(row["FinanceLeg_IRBenchMark"].ToString()),
                            EquityLeg_ConversionRatio = float.Parse(row["EquityLeg_ConversionRatio"].ToString()),
                            FinanceLeg_DayCount = int.Parse(row["FinanceLeg_DayCount"].ToString()),
                            FinanceLeg_FXRate = float.Parse(row["FinanceLeg_FXRate"].ToString()),
                            FinanceLeg_CouponFreq = int.Parse(row["FinanceLeg_CouponFreq"].ToString()),
                            FinanceLeg_SBPoint = float.Parse(row["FinanceLeg_SBPoint"].ToString()),
                            Commission_HardCommRate = float.Parse(row["Commission_HardCommissionRate"].ToString()),
                            Commission_SoftCommRate = float.Parse(row["Commission_SoftCommissionRate"].ToString()),
                            Commission_Basis = int.Parse(row["Commission_Basis"].ToString()),
                            CustomFields = row["CustomFields"] != DBNull.Value ? JsonHelper.DeserializeToList<OTCCustomFields>(row["CustomFields"].ToString()) : new List<OTCCustomFields>(),
                        };
                        otcTemplates.Add(OTCTempDataTemplate);


                    }

                }
            }
            return otcTemplates;
        }



        private static List<SecMasterOTCData> GetOTCTemplatesFromDataset(DataSet result)
        {
            List<SecMasterOTCData> otcTemplates = new List<SecMasterOTCData>();
            if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow row in result.Tables[0].Rows)
                {
                    SecMasterOTCData otcTemplate = new SecMasterOTCData()
                    {
                        Id = int.Parse(row["Id"].ToString()),
                        Name = row["Name"].ToString(),
                        CreatedBy = int.Parse(row["CreatedBy"].ToString()),
                        CreationDate = Convert.ToDateTime(row["CreationDate"].ToString()),
                        Description = row["Description"].ToString(),
                        InstrumentType = row["InstrumentType"].ToString(),
                        ISDAContract = row["ISDAContract"].ToString(),
                        ISDACounterParty = int.Parse(row["ISDACounterParty"].ToString()),
                        LastModifieDate = Convert.ToDateTime(row["LastModifieDate"].ToString()),
                        LastModifiedBy = int.Parse(row["LastModifiedBy"].ToString()),
                        UnderlyingAssetID = int.Parse(row["UnderlyingAssetID"].ToString()),
                        DaysToSettle = int.Parse(row["DaysToSettle"].ToString()),


                    };
                    otcTemplates.Add(otcTemplate);
                }
            }
            return otcTemplates;
        }

        /// <summary>
        /// Get OTC Custom Fields
        /// </summary>
        /// <returns></returns>
        public static List<OTCCustomFields> GetOTCCustomFields(int instrumentTypeId = 0, int customFieldId = -1)
        {
            List<OTCCustomFields> customFields = new List<OTCCustomFields>();
            DataSet result = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_OTC_GetCustomFields";
                queryData.DictionaryDatabaseParameter.Add("@InstrumentType", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@InstrumentType",
                    ParameterType = DbType.Int32,
                    ParameterValue = instrumentTypeId
                });
                queryData.DictionaryDatabaseParameter.Add("@OTCTemplateID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CustomFieldId",
                    ParameterType = DbType.Int32,
                    ParameterValue = customFieldId
                });

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, CONST_SMConnStringName);
                if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        OTCCustomFields customField = new OTCCustomFields()
                        {
                            ID = int.Parse(row["Id"].ToString()),
                            Name = row["Name"].ToString(),
                            DataType = row["DataType"].ToString(),
                            DefaultValue = row["DefaultValue"].ToString(),
                            UIOrder = int.Parse(row["UIOrder"].ToString()),
                            InstrumentType = row["InstrumentType"].ToString(),

                        };
                        customFields.Add(customField);
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
            return customFields;

        }

        /// <summary>
        /// Save OTC Templates
        /// </summary>
        /// <param name="OTCTemplates"></param>
        /// <returns></returns>
        public static int SaveOTCTemplates(SecMasterOTCData OTCTemplates)
        {
            string SaveSpName = string.Empty;
            if (OTCTemplates.InstrumentType == "1")
            {
                OTCTemplates.CustomFieldsString = JsonHelper.SerializeObject<OTCCustomFields>(((SecMasterEquitySwap)OTCTemplates).CustomFields);
                SaveSpName = "P_OTC_SaveTemplateData";
            }
            else if (OTCTemplates.InstrumentType == "2")
            {
                OTCTemplates.CustomFieldsString = JsonHelper.SerializeObject<OTCCustomFields>(((SecMasterCFDData)OTCTemplates).CustomFields);
                SaveSpName = "P_OTC_SaveCFDTemplateData";
            }
            else if (OTCTemplates.InstrumentType == "3")
            {
                OTCTemplates.CustomFieldsString = JsonHelper.SerializeObject<OTCCustomFields>(((SecMasterConvertibleBondData)OTCTemplates).CustomFields);
                SaveSpName = "P_OTC_SaveOTCTemplateData";
            }
            string OTCTemplatesXML = XMLUtilities.SerializeToXML(OTCTemplates);

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = SaveSpName;
                queryData.DictionaryDatabaseParameter.Add("@xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@xml",
                    ParameterType = DbType.String,
                    ParameterValue = OTCTemplatesXML
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, CONST_SMConnStringName);
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

        /// <summary>
        /// Save OTC Custom Fields
        /// </summary>
        /// <param name="customField"></param>
        /// <returns></returns>
        public static int SaveOTCCustomFields(OTCCustomFields customField)
        {
            string customFieldXML = XMLUtilities.SerializeToXML(customField);

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_OTC_SaveCustomFields";
                queryData.DictionaryDatabaseParameter.Add("@xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@xml",
                    ParameterType = DbType.String,
                    ParameterValue = customFieldXML
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, CONST_SMConnStringName);
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


        /// <summary>
        /// Delete OTC Custom Fields
        /// </summary>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        ///  
        public static int DeleteOTCCustomFields(int customFieldId)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_OTC_DeleteCustomFields";
                queryData.DictionaryDatabaseParameter.Add("@CustomFieldId", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CustomFieldId",
                    ParameterType = DbType.Int32,
                    ParameterValue = customFieldId
                });
                queryData.DictionaryDatabaseParameter.Add("@ErrorMessage", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@ErrorMessage",
                    ParameterType = DbType.String,
                    ParameterValue = "",
                    OutParameterSize = 100

                });
                queryData.DictionaryDatabaseParameter.Add("@ErrorNumber", new DatabaseParameter()
                {
                    IsOutParameter = true,
                    ParameterName = "@ErrorNumber",
                    ParameterType = DbType.Int32
                });

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, CONST_SMConnStringName);
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


        internal static List<OTCTradeData> GetOTCTradeData(List<string> groupIds)
        {
            List<OTCTradeData> otcTradeData = new List<OTCTradeData>();
            DataSet result = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_OTC_GetTradeData";
                queryData.DictionaryDatabaseParameter.Add("@GroupIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@GroupIDs",
                    ParameterType = DbType.String,
                    ParameterValue = string.Join(",", groupIds)
                });

                result = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        var instrumentType = int.Parse(row["InstrumentType"].ToString());
                        if (instrumentType == 1)
                        {
                            OTCTradeData equitySwap = GetEquitySwapTradeData(row);
                            otcTradeData.Add(equitySwap);
                        }
                    }
                }
                if (result != null && result.Tables.Count > 1 && result.Tables[1].Rows.Count > 0)
                {

                    foreach (DataRow row in result.Tables[1].Rows)
                    {
                        var instrumentType = int.Parse(row["InstrumentType"].ToString());
                        if (instrumentType == 2)
                        {
                            CFDTradeData equitySwap = GetCFDTradeData(row);
                            otcTradeData.Add(equitySwap);
                        }
                    }
                }

                if (result != null && result.Tables.Count > 2 && result.Tables[2].Rows.Count > 0)
                {

                    foreach (DataRow row in result.Tables[2].Rows)
                    {
                        var instrumentType = int.Parse(row["InstrumentType"].ToString());
                        if (instrumentType == 3)
                        {
                            ConvertibleBondTradeData convertibleBond = GetConvertibleBondTradeData(row);
                            otcTradeData.Add(convertibleBond);
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
            return otcTradeData;
        }

        private static CFDTradeData GetCFDTradeData(DataRow row)
        {
            try
            {
                return new CFDTradeData()
                {
                    GroupID = row["GroupId"].ToString(),
                    DaysToSettle = int.Parse(row["DaysToSettle"].ToString()),
                    InstrumentType = row["InstrumentType"].ToString(),
                    TradeDate = Convert.ToDateTime(row["TradeDate"].ToString()),
                    EffectiveDate = Convert.ToDateTime(row["EffectiveDate"].ToString()),
                    ISDACounterParty = int.Parse(row["ISDA_CounterParty"].ToString()),
                    CommissionBasis = row["Commission_Basis"].ToString(),
                    HardCommissionRate = float.Parse(row["Commission_HardCommissionRate"].ToString()),
                    SoftCommissionRate = float.Parse(row["Commission_SoftCommissionRate"].ToString()),
                    FinanceLeg_InterestRate = int.Parse(row["FinanceLeg_InterestRate"].ToString()),
                    FinanceLeg_SpreadBasisPoint = float.Parse(row["FinanceLeg_SpreadBasisPoint"].ToString()),
                    FinanceLeg_DayCount = int.Parse(row["FinanceLeg_DayCount"].ToString()),
                    FinanceLeg_FixedRate = float.Parse(row["FinanceLeg_FixedRate"].ToString()),
                    Collateral_Margin = float.Parse(row["Collateral_Margin"].ToString()),
                    Collateral_Rate = float.Parse(row["Collateral_Rate"].ToString()),
                    Collateral_DayCount = int.Parse(row["Collateral_DayCount"].ToString()),
                    CustomFields = (row["CustomFields"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["CustomFields"].ToString())) ? JsonHelper.DeserializeToList<OTCCustomFields>(row["CustomFields"].ToString()) : new List<OTCCustomFields>(),
                    UniqueIdentifier = (row["UniqueIdentifier"].ToString())

                };
            }
            catch (Exception)
            {

                throw;
            }
        }


        private static ConvertibleBondTradeData GetConvertibleBondTradeData(DataRow row)
        {
            try
            {
                return new ConvertibleBondTradeData()
                {
                    GroupID = row["GroupId"].ToString(),
                    DaysToSettle = int.Parse(row["DaysToSettle"].ToString()),
                    InstrumentType = row["InstrumentType"].ToString(),
                    TradeDate = Convert.ToDateTime(row["TradeDate"].ToString()),
                    EffectiveDate = Convert.ToDateTime(row["EffectiveDate"].ToString()),
                    ISDACounterParty = int.Parse(row["ISDA_CounterParty"].ToString()),
                    EquityLeg_ConversionRatio = float.Parse(row["EquityLeg_ConversionRatio"].ToString()),
                    EquityLeg_ConversionPrice = float.Parse(row["EquityLeg_ConversionPrice"].ToString()),
                    EquityLeg_ConversionDate = Convert.ToDateTime(row["EquityLeg_ConversionDate"].ToString()),

                    FinanceLeg_DayCount = int.Parse(row["FinanceLeg_DayCount"].ToString()),
                    FinanceLeg_FXRate = float.Parse(row["FinanceLeg_FXRate"].ToString()),
                    FinanceLeg_CouponFreq = int.Parse(row["FinanceLeg_CouponFreq"].ToString()),
                    FinanceLeg_SBPoint = float.Parse(row["FinanceLeg_SBPoint"].ToString()),
                    FinanceLeg_IRBenchMark = int.Parse(row["FinanceLeg_IRBenchMark"].ToString()),
                    FinanceLeg_ZeroCoupon = Boolean.Parse(row["FinanceLeg_ZeroCoupon"].ToString()),
                    FinanceLeg_FirstResetDate = Convert.ToDateTime(row["FinanceLeg_FirstResetDate"].ToString()),
                    FinanceLeg_FirstPaymentDate = Convert.ToDateTime(row["FinanceLeg_FirstPaymentDate"].ToString()),
                    Sedol = (row["Sedol"].ToString()),
                    Isin = (row["ISIN"].ToString()),
                    Cusip = (row["Cusip"].ToString()),
                    Currency = (row["Currency"].ToString()),
                    FinanceLeg_ParValue = (row["FinanceLeg_ParValue"].ToString()),
                    Commission_HardCommRate = float.Parse(row["Commission_HardCommissionRate"].ToString()),
                    Commission_SoftCommRate = float.Parse(row["Commission_SoftCommissionRate"].ToString()),
                    Commission_Basis = int.Parse(row["Commission_Basis"].ToString()),
                    CustomFields = (row["CustomFields"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["CustomFields"].ToString())) ? JsonHelper.DeserializeToList<OTCCustomFields>(row["CustomFields"].ToString()) : new List<OTCCustomFields>(),
                    UniqueIdentifier = (row["UniqueIdentifier"].ToString())

                };
            }
            catch (Exception)
            {

                throw;
            }
        }


        private static OTCTradeData GetEquitySwapTradeData(DataRow row)
        {

            try
            {
                return new EquitySwapTradeData()

                {
                    GroupID = row["GroupId"].ToString(),
                    DaysToSettle = int.Parse(row["DaysToSettle"].ToString()),
                    InstrumentType = row["InstrumentType"].ToString(),
                    TradeDate = Convert.ToDateTime(row["TradeDate"].ToString()),
                    EffectiveDate = Convert.ToDateTime(row["EffectiveDate"].ToString()),
                    ISDACounterParty = int.Parse(row["ISDA_CounterParty"].ToString()),
                    EquityLeg_Frequency = row["EquityLeg_Frequency"].ToString(),
                    EquityLeg_BulletSwap = Convert.ToBoolean(row["EquityLeg_BulletSwap"].ToString()),
                    EquityLeg_FirstPaymentDate = Convert.ToDateTime(row["EquityLeg_FirstPaymentDate"].ToString()),
                    EquityLeg_ExpirationDate = Convert.ToDateTime(row["EquityLeg_ExpirationDate"].ToString()),
                    EquityLeg_ImpliedCommission = Convert.ToBoolean(row["EquityLeg_ImpliedCommission"].ToString()),
                    CommissionBasis = row["Commission_Basis"].ToString(),
                    HardCommissionRate = float.Parse(row["Commission_HardCommissionRate"].ToString()),
                    SoftCommissionRate = float.Parse(row["Commission_SoftCommissionRate"].ToString()),
                    FinanceLeg_InterestRate = int.Parse(row["FinanceLeg_InterestRate"].ToString()),
                    FinanceLeg_SpreadBasisPoint = float.Parse(row["FinanceLeg_SpreadBasisPoint"].ToString()),
                    FinanceLeg_DayCount = int.Parse(row["FinanceLeg_DayCount"].ToString()),
                    FinanceLeg_Frequency = row["FinanceLeg_Frequency"].ToString(),
                    FinanceLeg_FixedRate = float.Parse(row["FinanceLeg_FixedRate"].ToString()),
                    FinanceLeg_FirstResetDate = Convert.ToDateTime(row["FinanceLeg_FirstResetDate"].ToString()),
                    FinanceLeg_FirstPaymentDate = Convert.ToDateTime(row["FinanceLeg_FirstPaymentDate"].ToString()),
                    CustomFields = !string.IsNullOrWhiteSpace(row["CustomFields"].ToString()) ? JsonHelper.DeserializeToList<OTCCustomFields>(row["CustomFields"].ToString()) : new List<OTCCustomFields>(),
                    UniqueIdentifier = (row["UniqueIdentifier"].ToString()),
                    EquityLeg_ExcludeDividends = Convert.ToBoolean(row["EquityLeg_ExcludeDividends"].ToString()),
                };

            }
            catch (Exception)
            {

                throw;
            }
        }

        internal static System.Threading.Tasks.Task GetOTCTempDataTemplatesDetails(int templateID)
        {
            throw new NotImplementedException();
        }
    }
}
