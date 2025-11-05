using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Prana.Import
{
    public class ImportHelper
    {

        private static ConcurrentDictionary<string, int> _dictCurrencies = null;


        /// <summary>
        /// To set object property type
        /// </summary>
        /// <param name="typeToLoad"></param>
        /// <param name="ds"></param>
        /// <param name="importObj"></param>
        /// <param name="irow"></param>
        internal static void SetProperty(Type typeToLoad, DataSet ds, object importObj, int irow)
        {
            try
            {
                for (int icol = 0; icol < ds.Tables[0].Columns.Count; icol++)
                {
                    string colName = ds.Tables[0].Columns[icol].ColumnName.ToString();

                    // assign into property
                    PropertyInfo propInfo = typeToLoad.GetProperty(colName);
                    if (propInfo != null)
                    {
                        Type dataType = propInfo.PropertyType;

                        if (dataType.FullName.Equals("System.String"))
                        {
                            if (String.IsNullOrEmpty(ds.Tables[0].Rows[irow][icol].ToString()))
                            {
                                propInfo.SetValue(importObj, string.Empty, null);
                            }
                            else
                            {
                                propInfo.SetValue(importObj, ds.Tables[0].Rows[irow][icol].ToString().Trim(), null);

                            }
                        }
                        else if (dataType.FullName.Equals("System.Double"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(importObj, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                double result;
                                blnIsTrue = double.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(importObj, Convert.ToDouble(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(importObj, double.MinValue, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Int32"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(importObj, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                int result;
                                blnIsTrue = int.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(importObj, Convert.ToInt32(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(importObj, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Int64"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(importObj, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                Int64 result;
                                blnIsTrue = Int64.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(importObj, Convert.ToInt64(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(importObj, 0, null);
                                }
                            }
                        }
                        else if (dataType.FullName.Equals("System.Boolean"))
                        {
                            if (ds.Tables[0].Rows[irow][icol].Equals(string.Empty) || ds.Tables[0].Rows[irow][icol].Equals(System.DBNull.Value))
                            {
                                propInfo.SetValue(importObj, 0, null);
                            }
                            else
                            {
                                bool blnIsTrue;
                                bool result;
                                blnIsTrue = bool.TryParse(ds.Tables[0].Rows[irow][icol].ToString(), out result);
                                if (blnIsTrue)
                                {
                                    propInfo.SetValue(importObj, Convert.ToBoolean(ds.Tables[0].Rows[irow][icol]), null);
                                }
                                else
                                {
                                    propInfo.SetValue(importObj, 0, null);
                                }
                            }
                        }
                        else if (dataType.BaseType.Equals(typeof(System.Enum)))//dataType.FullName.Equals("Prana.BusinessObjects.AppConstants.Operator"))
                        {
                            //Enum handling on generic basis since we are also dealing with another column 
                            //CommissionSource now.
                            string colValue = ds.Tables[0].Rows[irow][icol].ToString();
                            object value = null;
                            if (!string.IsNullOrEmpty(colValue))
                            {
                                value = Enum.Parse(dataType, colValue, true);
                                propInfo.SetValue(importObj, value, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To set directory path for xml files of import
        /// </summary>
        /// <param name="currentTask"></param>
        /// <param name="validatedSymbolsXmlFilePath"></param>
        /// <param name="totalImportDataXmlFilePath"></param>
        internal static void SetDirectoryPath(TaskResult currentTask, ref string validatedSymbolsXmlFilePath, ref string totalImportDataXmlFilePath)
        {
            try
            {
                string executionName = string.Empty;
                string dashboardXmlDirectoryPath = string.Empty;
                string refDataDirectoryPath = string.Empty;

                if (currentTask != null && currentTask.ExecutionInfo.TaskInfo != null)
                {
                    executionName = Path.GetFileName(currentTask.GetDashBoardXmlPath());
                    dashboardXmlDirectoryPath = Path.GetDirectoryName(currentTask.GetDashBoardXmlPath());
                }
                if (!Directory.Exists(Application.StartupPath + dashboardXmlDirectoryPath))
                {
                    Directory.CreateDirectory(Application.StartupPath + dashboardXmlDirectoryPath);
                }

                refDataDirectoryPath = dashboardXmlDirectoryPath + @"\RefData";
                if (!Directory.Exists(Application.StartupPath + refDataDirectoryPath))
                {
                    Directory.CreateDirectory(Application.StartupPath + refDataDirectoryPath);
                }

                validatedSymbolsXmlFilePath = refDataDirectoryPath + @"\" + executionName + "_ValidatedSymbols" + ".xml";
                totalImportDataXmlFilePath = refDataDirectoryPath + @"\" + executionName + "_ImportData" + ".xml";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Set Property New
        /// </summary>
        /// <param name="row"></param>
        /// <param name="positionMaster"></param>
        internal static void SetPropertyNew(DataRow row, PositionMaster positionMaster)
        {
            try
            {
                #region Fill String
                FillString(row, positionMaster);
                #endregion
                #region Fill Integers
                FillIntegers(positionMaster, row);
                #endregion
                #region Fill Double
                FillDouble(positionMaster, row);
                #endregion
                #region Fill Bool
                positionMaster.IsZero = row.GetBool("IsZero", positionMaster.IsZero);
                positionMaster.IsSecApproved = row.GetBool("IsSecApproved", positionMaster.IsSecApproved);
                #endregion
                #region Fill Date
                positionMaster.NirvanaProcessDate = row.GetDate("NirvanaProcessDate", positionMaster.NirvanaProcessDate);
                positionMaster.FirstCouponDate = row.GetDate("FirstCouponDate", positionMaster.FirstCouponDate);
                positionMaster.MaturityDate = row.GetDate("MaturityDate", positionMaster.MaturityDate);
                #endregion
                #region Fill Enum
                FillEnum(positionMaster, row);
                #endregion
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
        /// Fill String in positionMaster
        /// </summary>
        /// <param name="row"></param>
        /// <param name="positionMaster"></param>
        private static void FillString(DataRow row, PositionMaster positionMaster)
        {
            try
            {
                positionMaster.Symbol = row.GetString("Symbol", positionMaster.Symbol);
                positionMaster.Bloomberg = row.GetString("Bloomberg", positionMaster.Bloomberg);
                positionMaster.AccountName = row.GetString("AccountName", positionMaster.AccountName);
                positionMaster.ImportTag = row.GetString("ImportTag", positionMaster.ImportTag);
                positionMaster.ExecutingBroker = row.GetString("ExecutingBroker", positionMaster.ExecutingBroker);
                positionMaster.Description = row.GetString("Description", positionMaster.Description);
                positionMaster.Symbology = row.GetString("Symbology", positionMaster.Symbology);
                positionMaster.PositionStartDate = row.GetString("PositionStartDate", positionMaster.PositionStartDate);
                positionMaster.PBSymbol = row.GetString("PBSymbol", positionMaster.PBSymbol);
                positionMaster.CUSIP = row.GetString("CUSIP", positionMaster.CUSIP);
                positionMaster.SEDOL = row.GetString("SEDOL", positionMaster.SEDOL);
                positionMaster.ISIN = row.GetString("ISIN", positionMaster.ISIN);
                positionMaster.RIC = row.GetString("RIC", positionMaster.RIC);
                positionMaster.OSIOptionSymbol = row.GetString("OSIOptionSymbol", positionMaster.OSIOptionSymbol);
                positionMaster.IDCOOptionSymbol = row.GetString("IDCOOptionSymbol", positionMaster.IDCOOptionSymbol);
                positionMaster.OpraOptionSymbol = row.GetString("OpraOptionSymbol", positionMaster.OpraOptionSymbol);
                positionMaster.AUECLocalDate = row.GetString("AUECLocalDate", positionMaster.AUECLocalDate);
                positionMaster.Strategy = row.GetString("Strategy", positionMaster.Strategy);
                positionMaster.SecApprovalStatus = row.GetString("SecApprovalStatus", positionMaster.SecApprovalStatus);
                positionMaster.ValidationStatus = row.GetString("ValidationStatus", positionMaster.ValidationStatus);
                positionMaster.MismatchType = row.GetString("MismatchType", positionMaster.MismatchType);
                positionMaster.DerivativeRootSymbol = row.GetString("DerivativeRootSymbol", positionMaster.DerivativeRootSymbol);
                positionMaster.DerivativeUnderlyingSymbol = row.GetString("DerivativeUnderlyingSymbol", positionMaster.DerivativeUnderlyingSymbol);
                positionMaster.PositionType = row.GetString("PositionType", positionMaster.PositionType);
                positionMaster.PBAssetType = row.GetString("PBAssetType", positionMaster.PBAssetType);
                positionMaster.ProcessDate = row.GetString("ProcessDate", positionMaster.ProcessDate);
                positionMaster.OriginalPurchaseDate = row.GetString("OriginalPurchaseDate", positionMaster.OriginalPurchaseDate);
                positionMaster.PositionSettlementDate = row.GetString("PositionSettlementDate", positionMaster.PositionSettlementDate);
                positionMaster.PositionExpirationDate = row.GetString("PositionExpirationDate", positionMaster.PositionExpirationDate);
                positionMaster.ExpirationDate = row.GetString("ExpirationDate", positionMaster.ExpirationDate);
                positionMaster.GroupID = row.GetString("GroupID", positionMaster.GroupID);
                positionMaster.LotId = row.GetString("LotId", positionMaster.LotId);
                positionMaster.ExternalTransId = row.GetString("ExternalTransId", positionMaster.ExternalTransId);
                positionMaster.TransactionType = row.GetString("TransactionType", positionMaster.TransactionType);
                positionMaster.ExpiredTaxlotID = row.GetString("ExpiredTaxlotID", positionMaster.ExpiredTaxlotID);
                positionMaster.TradeAttribute1 = row.GetString("TradeAttribute1", positionMaster.TradeAttribute1);
                positionMaster.TradeAttribute2 = row.GetString("TradeAttribute2", positionMaster.TradeAttribute2);
                positionMaster.TradeAttribute3 = row.GetString("TradeAttribute3", positionMaster.TradeAttribute3);
                positionMaster.TradeAttribute4 = row.GetString("TradeAttribute4", positionMaster.TradeAttribute4);
                positionMaster.TradeAttribute5 = row.GetString("TradeAttribute5", positionMaster.TradeAttribute5);
                positionMaster.SideTagValue = row.GetString("SideTagValue", positionMaster.SideTagValue);
                positionMaster.CreatedByID = row.GetString("CreatedByID", positionMaster.CreatedByID);
                positionMaster.ImportStatus = row.GetString("ImportStatus", positionMaster.ImportStatus);
                positionMaster.ExternalOrderID = row.GetString("ExternalOrderID", positionMaster.ExternalOrderID);
                positionMaster.TaxLotClosingId = row.GetString("TaxLotClosingId", positionMaster.TaxLotClosingId);

                #region Settlement Currency
                string currency = row.GetString(OrderFields.PROPERTY_SETTLEMENTCURRENCYNAME, positionMaster.SettlementCurrencyID.ToString());
                positionMaster.SettlementCurrencyID = positionMaster.CurrencyID;
                if (_dictCurrencies.ContainsKey(currency))
                {
                    int currecnyID = 0;
                    if (_dictCurrencies.TryGetValue(currency, out currecnyID))
                    {
                        positionMaster.SettlCurrencyName = currency;
                        positionMaster.SettlementCurrencyID = currecnyID;
                    }
                }
                #endregion
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
        ///  Fill Enum in positionMaster
        /// </summary>
        /// <param name="positionMaster"></param>
        /// <param name="row"></param>
        private static void FillEnum(BusinessObjects.PositionMaster positionMaster, DataRow row)
        {
            try
            {
                positionMaster.AccrualBasis = row.GetEnum<AccrualBasis>("AccrualBasis", positionMaster.AccrualBasis);
                positionMaster.BondType = row.GetEnum<SecurityType>("BondType", positionMaster.BondType);
                positionMaster.Freq = row.GetEnum<CouponFrequency>("Freq", positionMaster.Freq);
                positionMaster.AssetType = row.GetEnum<AssetCategory>("AssetType", positionMaster.AssetType);
                positionMaster.TransactionSource = row.GetEnum<TransactionSource>("TransactionSource", positionMaster.TransactionSource);
                positionMaster.WorkflowState = row.GetEnum<NirvanaWorkFlowsStats>("WorkflowState", positionMaster.WorkflowState);

                //Added By : Manvendra Prajapati
                //Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-10380
                positionMaster.FXConversionMethodOperator = row.GetEnum<Operator>("FxConversionMethodOperator", positionMaster.FXConversionMethodOperator);
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
        ///  Fill Double in positionMaster
        /// </summary>
        /// <param name="positionMaster"></param>
        /// <param name="row"></param>
        private static void FillDouble(BusinessObjects.PositionMaster positionMaster, DataRow row)
        {
            try
            {
                positionMaster.StampDuty = row.GetDouble("StampDuty", positionMaster.StampDuty);
                positionMaster.SoftCommission = row.GetDouble("SoftCommission", positionMaster.SoftCommission);
                positionMaster.TransactionLevy = row.GetDouble("TransactionLevy", positionMaster.TransactionLevy);
                positionMaster.NetPosition = row.GetDouble("NetPosition", positionMaster.NetPosition);
                positionMaster.CostBasis = row.GetDouble("CostBasis", positionMaster.CostBasis);
                positionMaster.FXRate = row.GetDouble("FXRate", positionMaster.FXRate);
                positionMaster.Commission = row.GetDouble("Commission", positionMaster.Commission);
                positionMaster.Fees = row.GetDouble("Fees", positionMaster.Fees);
                positionMaster.ClearingBrokerFee = row.GetDouble("ClearingBrokerFee", positionMaster.ClearingBrokerFee);
                positionMaster.ClearingFee = row.GetDouble("ClearingFee", positionMaster.ClearingFee);
                positionMaster.TaxOnCommissions = row.GetDouble("TaxOnCommissions", positionMaster.TaxOnCommissions);
                positionMaster.MiscFees = row.GetDouble("MiscFees", positionMaster.MiscFees);
                positionMaster.SecFee = row.GetDouble("SecFee", positionMaster.SecFee);
                positionMaster.OccFee = row.GetDouble("OccFee", positionMaster.OccFee);
                positionMaster.OrfFee = row.GetDouble("OrfFee", positionMaster.OrfFee);
                positionMaster.ExpiredQty = row.GetDouble("ExpiredQty", positionMaster.ExpiredQty);
                positionMaster.Coupon = row.GetDouble("Coupon", positionMaster.Coupon);
                positionMaster.Multiplier = row.GetDouble("Multiplier", positionMaster.Multiplier);
                positionMaster.AccruedInterest = row.GetDouble("AccruedInterest", positionMaster.AccruedInterest);
                positionMaster.OptionPremiumAdjustment = row.GetDouble(OrderFields.PROPERTY_OPTIONPREMIUMADJUSTMENT, positionMaster.OptionPremiumAdjustment);
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
        ///  Fill Integers in positionMaster
        /// </summary>
        /// <param name="positionMaster"></param>
        /// <param name="row"></param>
        private static void FillIntegers(BusinessObjects.PositionMaster positionMaster, DataRow row)
        {
            try
            {
                positionMaster.RowIndex = row.GetInteger("RowIndex", positionMaster.RowIndex);
                positionMaster.CounterPartyID = row.GetInteger("CounterPartyID", positionMaster.CounterPartyID);
                positionMaster.AUECID = row.GetInteger("AUECID", positionMaster.AUECID);
                positionMaster.AssetID = row.GetInteger("AssetID", positionMaster.AssetID);
                positionMaster.Call_Put = row.GetInteger("Call_Put", positionMaster.Call_Put);
                positionMaster.UnderlyingID = row.GetInteger("UnderlyingID", positionMaster.UnderlyingID);
                positionMaster.StrategyID = row.GetInteger("StrategyID", positionMaster.StrategyID);
                positionMaster.UserID = row.GetInteger("UserID", positionMaster.UserID);
                positionMaster.TradingAccountID = row.GetInteger("TradingAccountID", positionMaster.TradingAccountID);
                positionMaster.VenueID = row.GetInteger("VenueID", positionMaster.VenueID);
                positionMaster.ExchangeID = row.GetInteger("ExchangeID", positionMaster.ExchangeID);
                positionMaster.LeadCurrencyID = row.GetInteger("LeadCurrencyID", positionMaster.LeadCurrencyID);
                positionMaster.TaxLotID = row.GetInteger("TaxLotID", positionMaster.TaxLotID);
                positionMaster.SecFees = row.GetInteger("SecFees", positionMaster.SecFees);
                positionMaster.AccountID = row.GetInteger("AccountID", positionMaster.AccountID);
                positionMaster.UploadID = row.GetInteger("UploadID", positionMaster.UploadID);
                positionMaster.CurrencyID = row.GetInteger("CurrencyID", positionMaster.CurrencyID);
                //PRANA-9628	[Import] - Settlement currency field comes out none instead of Trade Currency while importing.
                if (positionMaster.SettlementCurrencyID > 0 && CachedDataManager.GetInstance.GetAllCurrencies().ContainsKey(positionMaster.SettlementCurrencyID)
                    && positionMaster.SettlCurrencyName != CachedDataManager.GetInstance.GetAllCurrencies()[positionMaster.SettlementCurrencyID])
                {
                    positionMaster.SettlCurrencyName = CachedDataManager.GetInstance.GetAllCurrencies()[positionMaster.SettlementCurrencyID];
                }
                positionMaster.CompanyID = row.GetInteger("CompanyID", positionMaster.CompanyID);
                positionMaster.StateIDForStrategy = row.GetInteger("StateIDForStrategy", positionMaster.StateIDForStrategy);
                positionMaster.ImportFileID = row.GetInteger("ImportFileID", positionMaster.ImportFileID);
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
        /// fill Currency dictionary to fill position master 
        /// </summary>
        internal static void FillCurrenciesDictionary()
        {
            try
            {
                _dictCurrencies = new ConcurrentDictionary<string, int>();
                foreach (KeyValuePair<int, string> kvp in CachedDataManager.GetInstance.GetAllCurrencies())
                {
                    _dictCurrencies.TryAdd(kvp.Value, kvp.Key);
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
    }
}
