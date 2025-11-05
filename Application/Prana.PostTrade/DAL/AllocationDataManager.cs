using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Prana.CommonDataCache
{
    public class AllocationDataManager
    {
        // private static int _errorNumber;

        private static readonly int _miscellanousTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["MiscellanousTimeout"]);
        // private static int _errorNumber;

        public static TaxlotBaseCollection GetAllAccountOpenPositionsForDateAndSymbol(DateTime selectedDate, string symbol, string commaSeparatedAccountIds)
        {
            TaxlotBaseCollection accountTaxLotCollection = new TaxlotBaseCollection();

            try
            {
                object[] parameter = new object[3];
                parameter[0] = selectedDate;
                parameter[1] = symbol;
                parameter[2] = commaSeparatedAccountIds;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("PMGetFundOpenPositionsForSymbol", parameter, string.Empty, _miscellanousTimeout))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accountTaxLotCollection.Add(FillTaxLot(row));
                    }
                }

            }
            #region Catch
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
            #endregion
            return accountTaxLotCollection;
        }

        public static List<string> GetFKIDForDivUndo(string caIds)
        {
            List<string> fkIDs = new List<string>();

            try
            {

                object[] parameter = new object[1];
                parameter[0] = caIds;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetFKIDForDivUndo", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        fkIDs.Add(row[0].ToString());
                    }
                }

            }
            #region Catch
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
            #endregion

            return fkIDs;
        }

        /// <summary>
        /// Returns the allocated and unallocated taxlots for the given utcdate.
        /// </summary>
        /// <param name="utcDate"></param>
        /// <returns></returns>
        public static TaxlotBaseCollection GetAllTaxLotsForDateAndSymbol(DateTime utcDate, string symbol)
        {
            TaxlotBaseCollection accountTaxLotCollection = new TaxlotBaseCollection();
            string auecString = TimeZoneHelper.GetInstance().GetAllAUECLocalDatesFromUTCStr(utcDate);

            try
            {

                object[] parameter = new object[2];
                parameter[0] = auecString;
                parameter[1] = symbol;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAllTaxlotsForDateAndSymbol", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accountTaxLotCollection.Add(FillTaxLot(row));
                    }
                }

            }
            #region Catch
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
            #endregion
            return accountTaxLotCollection;
        }

        private static TaxlotBase FillTaxLotForUndo(object[] row, int offset)
        {
            TaxlotBase f = null;
            try
            {
                int i = 0;
                int exDivDate = offset + i;
                int payoutDate = offset + i;
                f = FillTaxLot(row);
                if (row[exDivDate] != DBNull.Value)
                {
                    f.ExDivDate = row[exDivDate].ToString();
                }
                if (row[payoutDate] != DBNull.Value)
                {
                    f.DivPayoutDate = row[payoutDate].ToString();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return f;
        }

        private static TaxlotBase FillTaxLot(object[] row)
        {
            int i = 0;
            TaxlotBase f = new TaxlotBase();
            try
            {


                //int taxlotID = offset + i++;
                //int orderSideTagValue = offset + i++;
                //int symbol = offset + i++;
                //int groupID = offset + i++;
                //int taxlotOpenQty = offset + i++;
                //int avgPrice = offset + i++;
                //int accountID = offset + i++;
                //int openTotalCommissionandFees = offset + i++;
                //int l2ID = offset + i++;
                //int auecID = offset + i++;
                //int taxLotPK = offset + i++;
                //int AUECLocalDate = offset + i++;
                //int AssetID = offset + i++;
                //int taxlot_PK = offset + i++;
                //int currencyID = offset + i++;
                //int closedTotalCommissionandFees = offset + i++;
                //int OrderTypeTagValue = offset + i++;
                //int CounterPartyID = offset + i++;
                //int VenueID = offset + i++;
                //int TradingAccountID = offset + i++;
                //int CumQty = offset + i++;
                //int AllocatedQty = offset + i++;
                //int ListID = offset + i++;
                //int UserID = offset + i++;
                //int ISProrataActive = offset + i++;
                //int AutoGrouped = offset + i++;
                //int StateID = offset + i++;
                //int IsBasketGroup = offset + i++;
                //int BasketGroupID = offset + i++;
                //int IsManualGroup = offset + i++;
                //int UnderLyingID = offset + i++;
                //int ExchangeID = offset + i++;
                //int Description = offset + i++;
                //int FXRate = offset + i++;
                //int FXConversionMethodOperator = offset + i++;

                if (row[i] != DBNull.Value)
                {
                    f.L2TaxlotID = row[i].ToString();
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.OrderSideTagValue = row[i].ToString().Trim();
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.Symbol = row[i].ToString();
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.GroupID = row[i].ToString();
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.OpenQty = Convert.ToDouble(row[i]);
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.AvgPrice = Convert.ToDouble(row[i]);
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.Level1ID = Convert.ToInt32(row[i]);
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.Level2ID = Convert.ToInt32(row[i]);
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.AUECID = Convert.ToInt32(row[i]);
                    //f.AUECDate = (TimeZoneHelper.GetAUECLocalDateFromUTC(Convert.ToInt32(row[auecID]), DateTime.Now.ToUniversalTime())).ToString();
                }
                i++;

                if (CachedDataManager.GetInstance.IsLongSide(f.OrderSideTagValue))
                {
                    f.PositionType = PositionType.Long;
                    f.PositionTag = PositionTag.Long;
                }
                else
                {
                    f.PositionType = PositionType.Short;
                    f.PositionTag = PositionTag.Short;
                }


                f.UTCDate = (DateTime.Now.ToUniversalTime()).ToString();

                if (row[i] != DBNull.Value)
                {
                    f.AUECLocalDate = Convert.ToDateTime(row[i]);
                    f.AUECDate = f.AUECLocalDate.ToString();
                }
                i++;

                if (row[i] != DBNull.Value)
                {
                    f.ParentTaxlot_PK = Convert.ToInt64(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.AssetCategory = (AssetCategory)Convert.ToInt32(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.Underlying = (Underlying)Convert.ToInt32(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.ExchangeID = Convert.ToInt32(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.CurrencyID = Convert.ToInt32(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.OpenTotalCommissionandFees = Convert.ToDouble(row[i]);
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.ClosedTotalCommissionandFees = Convert.ToDouble(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.OrderTypeTagValue = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.CounterPartyID = Convert.ToInt32(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.VenueID = Convert.ToInt32(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.TradingAccountID = Convert.ToInt32(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.CumQty = Convert.ToDouble(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.AllocatedQty = Convert.ToDouble(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.ListID = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.ISProrataActive = Convert.ToBoolean(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.AutoGrouped = Convert.ToBoolean(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.IsManualGroup = Convert.ToBoolean(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.Description = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.FXRate = Convert.ToDouble(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.FXConversionMethodOperator = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.ProcessDate = Convert.ToDateTime(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.OriginalPurchaseDate = Convert.ToDateTime(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.UserID = Convert.ToInt32(row[i]);
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.TradeAttribute1 = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.TradeAttribute2 = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.TradeAttribute3 = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.TradeAttribute4 = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.TradeAttribute5 = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.TradeAttribute6 = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.LotId = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.ExternalTransId = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.AssetMultiplier = Convert.ToDouble(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.TransactionType = row[i].ToString();
                }
                //Added Company Name of The Symbol 
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.NewCompanyName = row[i].ToString();
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.NotionalValue = Convert.ToDouble(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.NotionalValueBase = Convert.ToDouble(row[i].ToString());
                }
                i++;
                if (row[i] != DBNull.Value)
                {
                    f.SettlementDate = Convert.ToDateTime(row[i].ToString());
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return f;
        }

        /// <summary>
        /// It will save the modified taxlot post corporate action. Used in split
        /// </summary>
        /// <param name="modifiedTaxlots"></param>
        public static int SaveAllTaxLotsPostCorporateAction(TaxlotBaseCollection modifiedTaxlots)
        {
            string taxlotsXML = XMLUtilities.SerializeToXML(modifiedTaxlots);
            string spName = "P_SaveTaxlotsPostSplit";
            return XMLSaveManager.SaveThroughXML(spName, taxlotsXML);
        }

        /// <summary>
        /// It will save the modified taxlot post corporate action. Used in stock dividend
        /// </summary>
        /// <param name="modifiedTaxlots"></param>
        /// 
        public static DataSet SavStockDividendForTaxlots(TaxlotBaseCollection modifiedTaxlots)
        {
            string taxlotsXML = XMLUtilities.SerializeToXML(modifiedTaxlots);
            int _errorNumber = 0;
            string _errorMessage = string.Empty;

            DataSet ds = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveTaxlotsPostStockDividend";
                queryData.DictionaryDatabaseParameter.Add("@xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@xml",
                    ParameterType = DbType.Xml,
                    ParameterValue = taxlotsXML
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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

        /// <summary>
        /// It will save the modified taxlot post corporate action. Used in Cash Dividend
        /// </summary>
        /// <param name="modifiedTaxlots"></param>
        /// 
        public static DataSet SaveCashDividendForTaxlots(TaxlotBaseCollection modifiedTaxlots)
        {
            string taxlotsXML = XMLUtilities.SerializeToXML(modifiedTaxlots);
            //string spName = "P_SaveCashDividendForTaxlots";
            //return XMLSaveManager.SaveThroughXML(spName, taxlotsXML);
            int _errorNumber = 0;
            string _errorMessage = string.Empty;

            //int rowsAffected = 0;
            DataSet ds = null;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SaveCashDividendForTaxlots";
                queryData.DictionaryDatabaseParameter.Add("@xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@xml",
                    ParameterType = DbType.Xml,
                    ParameterValue = taxlotsXML
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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

        /// <summary>
        /// It will save the modified taxlot post corporate action.
        /// </summary>
        /// <param name="modifiedTaxlots"></param>
        //public static int SaveAllTaxLotsPostCorporateActionNameChange(TaxlotBaseCollection modifiedTaxlots)
        //{
        //    string taxlotsXML = XMLUtilities.SerializeToXML(modifiedTaxlots);
        //    string spName = "P_SaveTaxlotsPostCorporateActionNameChange";
        //    return XMLSaveManager.SaveThroughXML(spName, taxlotsXML);
        //}

        public static int SaveCAWiseCloseData(CACloseData closeData)
        {
            string caWiseCloseXML = XMLUtilities.SerializeToXML(closeData);
            string spName = "P_SaveCAWiseCloseDataPostNameChange";
            return XMLSaveManager.SaveThroughXML(spName, caWiseCloseXML);
        }

        public static int UndoSplitCA(string caIDs)
        {
            int rowsAffected = 0;
            try
            {
                object[] parameter = new object[3];

                parameter[0] = caIDs;
                parameter[1] = string.Empty;
                parameter[2] = 0;

                string spName = "P_UndoSplit";

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameter);

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

            return rowsAffected;
        }

        public static DataSet UndoCashDividend(string caIDs, string taxlots)
        {
            DataSet ds = null;
            int _errorNumber = 0;
            string _errorMessage = string.Empty;

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_UndoCashDividend_New";
                queryData.DictionaryDatabaseParameter.Add("@corpactionIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@corpactionIDs",
                    ParameterType = DbType.String,
                    ParameterValue = caIDs
                });
                queryData.DictionaryDatabaseParameter.Add("@taxlotIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@taxlotIDs",
                    ParameterType = DbType.String,
                    ParameterValue = taxlots
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

            return ds;
        }

        /// <summary>
        /// Undo Stock dividend Corporate Action
        /// </summary>
        /// <param name="caIDs"></param>
        /// <returns></returns>
        public static DataSet UndoStockDividendCA(string caIDs)
        {
            DataSet ds = null;
            int _errorNumber = 0;
            string _errorMessage = string.Empty;

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_UndoStockDividend";
                queryData.DictionaryDatabaseParameter.Add("@corpactionIDs", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@corpactionIDs",
                    ParameterType = DbType.String,
                    ParameterValue = caIDs
                });

                XMLSaveManager.AddOutErrorParameters(queryData);
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion

            return ds;
        }

        public static List<string> GetNameChangeTaxlotIDs()
        {
            List<string> taxlotIDs = new List<string>();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetNameChangeTaxlotIDs";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        taxlotIDs.Add(row[0].ToString());
                    }
                }

            }
            #region Catch
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
            #endregion

            return taxlotIDs;
        }

        public static Dictionary<string, List<string>> GetTaxlotIdsBeforeUndoPreview(string corpActionIDs)
        {
            Dictionary<string, List<string>> caTaxlotIDDict = new Dictionary<string, List<string>>();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = corpActionIDs;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("PMGetTaxlotIdsBeforeUndoPreview", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);

                        string ca = string.Empty;
                        string taxlotid = string.Empty;

                        if (row[0] != DBNull.Value)
                        {
                            ca = row[0].ToString();
                        }
                        if (row[1] != DBNull.Value)
                        {
                            taxlotid = row[1].ToString();
                        }

                        if (caTaxlotIDDict.ContainsKey(ca))
                        {
                            caTaxlotIDDict[ca].Add(taxlotid);
                        }
                        else
                        {
                            List<string> taxlots = new List<string>();
                            taxlots.Add(taxlotid);
                            caTaxlotIDDict.Add(ca, taxlots);
                        }

                    }
                }
            }
            #region Catch
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
            #endregion
            return caTaxlotIDDict;
        }

        public static List<string> GetPositionalCorpActionTaxlotId(DateTime date, string caIDs)
        {
            List<string> positionalTaxlotId = new List<string>();

            try
            {
                object[] parameter = new object[2];
                parameter[0] = date;
                parameter[1] = caIDs;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("PM_GetNameChangeTaxlotIdForDate", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value)
                        {
                            positionalTaxlotId.Add(row[0].ToString());
                        }
                    }
                }

            }
            #region Catch
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
            #endregion
            return positionalTaxlotId;

        }

        public static List<string> GetFuturePositionalCorpActionTaxlotId(DateTime date)
        {
            List<string> positionalTaxlotId = new List<string>();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = date;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("PM_GetFutureNameChangeTaxlotIdForDate", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value)
                        {
                            positionalTaxlotId.Add(row[0].ToString());
                        }
                    }
                }

            }
            #region Catch
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
            #endregion
            return positionalTaxlotId;

        }

        public static TaxlotBaseCollection GetOpenPositionforUndoPreview(string corpActionIDs)
        {
            TaxlotBaseCollection accountTaxLotCollection = new TaxlotBaseCollection();

            try
            {
                object[] parameter = new object[1];
                parameter[0] = corpActionIDs;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("PMGetOpenPositionforUndoPreview", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        accountTaxLotCollection.Add(FillTaxLotForUndo(row, 44));
                    }
                }

            }
            #region Catch
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
            #endregion
            return accountTaxLotCollection;
        }

        public static int CopyOrigSymbolUDAInfo(string origSymbol, string newSymbol)
        {
            int affectedRows = 0;
            try
            {
                object[] parameter = new object[2];

                parameter[0] = origSymbol;
                parameter[1] = newSymbol;

                string spName = "P_CopyOrigSymbolUDAInfo";

                affectedRows = DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameter);
            }
            #region Catch
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

            #endregion
            return affectedRows;
        }

        /// <summary>
        /// Gets data for cost adjustment taxlots
        /// </summary>
        /// <returns>List of data for cost adjustment taxlots</returns>
        public static List<CostAdjustmentTaxlotsForSave> GetCostAdjustmentData()
        {
            List<CostAdjustmentTaxlotsForSave> costAdjustmentTaxlots = new List<CostAdjustmentTaxlotsForSave>();
            DataSet costAdjustmentPositions = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_AL_GetCostAdjustmentTaxlots";
                queryData.CommandTimeout = 200;

                costAdjustmentPositions = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                if (costAdjustmentPositions != null)
                {
                    DataTable dt = costAdjustmentPositions.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        costAdjustmentTaxlots.Add(GetCostAdjustmentTaxlotsData(row));
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
            return costAdjustmentTaxlots;
        }

        /// <summary>
        /// Gets CostAdjustmentTaxlotsForSave data from DataRow
        /// </summary>
        /// <param name="row">The DataRow</param>
        /// <returns>CostAdjustmentTaxlotsForSave data</returns>
        private static CostAdjustmentTaxlotsForSave GetCostAdjustmentTaxlotsData(DataRow row)
        {
            CostAdjustmentTaxlotsForSave costAdjustmentData = new CostAdjustmentTaxlotsForSave();
            try
            {
                costAdjustmentData.CAID = row["CostAdjustmentID"].ToString();
                costAdjustmentData.TaxlotID = row["TaxlotID"].ToString();
                costAdjustmentData.GroupID = row["GroupID"].ToString();
                costAdjustmentData.ClosingTaxlotID = row["ClosingTaxlotID"] == DBNull.Value ? null : row["ClosingTaxlotID"].ToString();
                costAdjustmentData.ClosingUniqueID = row["ClosingID"] == DBNull.Value ? null : row["ClosingID"].ToString();
                costAdjustmentData.ParentTaxlot_PK = row["ParentRow_Pk"] == DBNull.Value ? 0 : Convert.ToInt64(row["ParentRow_Pk"]);
                costAdjustmentData.Taxlot_PK = Convert.ToInt64(row["Taxlot_PK"]);
                costAdjustmentData.ClosingDate = Convert.ToDateTime(row["UTCInsertionTime"]);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return costAdjustmentData;
        }
    }
}
