using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Fix.FixDictionary;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Prana.PostTradeServices
{
    class DataBaseManager
    {
        static int _errorNumber = 0;
        static string _errorMessage = string.Empty;

        public static void MigrateDataFormTransactionDB()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_MigrateDataFromTransactionDB";

                XMLSaveManager.AddOutErrorParameters(queryData);

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData, "PranaDWConnectionString");

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static DataTable GetPMTaxlotsData()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllPMTaxlots";

            DataTable dt = null;

            try
            {
                dt = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "PranaDWConnectionString").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }
        public static DataTable GetPMClosingData()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllPMClosingTaxlots";

            DataTable dt = null;

            try
            {
                dt = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "PranaDWConnectionString").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }
        public static List<int> GetAllComapanyAccounts()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllCompanyFunds";

            List<int> comapnyAccounts = new List<int>();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData, "PranaDWConnectionString"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        comapnyAccounts.Add(Convert.ToInt32(row[0].ToString()));
                    }
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

            return comapnyAccounts;
        }

        public static List<int> GetAllSpecialDatesOfCurrentYear()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllSpecialDates";

            List<int> specialDates = new List<int>();

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData, "PranaDWConnectionString"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        specialDates.Add(Convert.ToInt32(row[0].ToString()));
                    }
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
            return specialDates;
        }
        public static DataTable GetMarkPriceData()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetMarkPrices";

            DataTable dt = null;

            try
            {
                dt = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "PranaDWConnectionString").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }
        public static void SavePMOpenPositionSnapShot(DataTable dtOpen)
        {
            object[] parameters = new object[1];

            MemoryStream stream = new MemoryStream();
            dtOpen.WriteXml(stream);

            byte[] bytes = stream.ToArray();
            string xml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            try
            {
                parameters[0] = xml;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveOpenPositionSnapShot", parameters, "PranaDWConnectionString");
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
        public static void SaveURPNLData(DataTable dtMTMPNL)
        {
            object[] parameters = new object[1];


            try
            {
                int rowLimit = 99;
                int count = 0;
                List<DataTable> dataTables = new List<DataTable>();
                DataTable dt = new DataTable("MTMPNL");
                foreach (DataRow row in dtMTMPNL.Rows)
                {
                    if (count == 0)
                    {
                        dt = dtMTMPNL.Clone();
                        //foreach (DataColumn column in dtMTMPNL.Columns)
                        //{
                        //    dt.Columns.Add(column.ColumnName);
                        //}
                        dataTables.Add(dt);
                    }
                    else if (count == rowLimit)
                    {
                        count = -1;
                    }

                    count++;

                    dt.Rows.Add(row.ItemArray);
                }

                foreach (DataTable dtChunk in dataTables)
                {
                    MemoryStream stream = new MemoryStream();
                    dtChunk.WriteXml(stream);

                    byte[] bytes = stream.ToArray();
                    string xml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                    parameters[0] = xml;
                    DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveMarkToMarketPNL", parameters, "PranaDWConnectionString");
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

        public static int GetTimeKeyForTheDate()
        {
            object[] parameters = new object[1];
            int timeKey = 0;

            try
            {
                parameters[0] = DateTime.Now.Date.ToString();
                timeKey = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar("P_GetTimeKeyForDate", parameters, "PranaDWConnectionString"));
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
            return timeKey;
        }
        public static DataTable GetAUECSpecificHolidays()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllAUECSpecificHolidays";

            DataTable dt = null;

            try
            {
                dt = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "PranaDWConnectionString").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }
        public static DataSet GetAUECWeekendHolidays()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllWeekendHolidays";

            DataSet ds = null;

            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "PranaDWConnectionString");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return ds;
        }
        public static DataTable GetAUECWeekends()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllAUECWeeklyHolidays";

            DataTable dt = null;

            try
            {
                dt = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "PranaDWConnectionString").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }
        public static DataTable GetCurrentSymbolSMData()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCurrentSymbolSMData";

            DataTable dt = null;

            try
            {
                dt = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "PranaDWConnectionString").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }
        public static DataTable GetCashForSpecialDates()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCashForSpecialDates";

            DataTable dt = null;

            try
            {
                dt = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "PranaDWConnectionString").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }
        public static void SaveIRRTableData(DataTable dtIRR)
        {
            object[] parameters = new object[1];

            MemoryStream stream = new MemoryStream();
            dtIRR.WriteXml(stream);

            byte[] bytes = stream.ToArray();
            string xml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            try
            {
                parameters[0] = xml;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveIRRTableData", parameters, "PranaDWConnectionString");
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
        public static DataTable GetCurrencyConversionRateData()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCurrencyConversionRates";

            DataTable dt = null;

            try
            {
                dt = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "PranaDWConnectionString").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }
        public static int GetCompanyBaseCurrency()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetCompanyBaseCurrencyID";
            int baseCurrencyID = 1;

            try
            {
                baseCurrencyID = Convert.ToInt32(DatabaseManager.DatabaseManager.ExecuteScalar(queryData, "PranaDWConnectionString"));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return baseCurrencyID;
        }

        public static DataTable GetFxRateOnTradeDateData()
        {
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllFxRateOnTradeDate";

            DataTable dt = null;

            try
            {
                dt = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData, "PranaDWConnectionString").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }

        /// <summary>
        /// Gets the manual order details.
        /// </summary>
        /// <param name="auecId">The auec identifier.</param>
        /// <param name="lastRunTime">The last run time.</param>
        /// <returns></returns>
        public static List<PranaMessage> GetManualOrderDetails(int auecId, DateTime lastRunTime)
        {
            List<PranaMessage> messages = new List<PranaMessage>();
            try
            {
                object[] parameter = new object[2];
                parameter[0] = lastRunTime;
                parameter[1] = auecId;
                DataSet dataSet = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetLastManualOrderTradesData", parameter, "PranaConnectionString");
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    PranaMessage msg = Transformer.CreatePranaMessageThroughReflection(row);
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagSide, row["OrderSidetagValue"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagSenderSubID, row["SenderSubID"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagAccount, row["TradingAccountID"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagExecInst, row["ExecutionInst"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagHandlInst, row["HandlingInst"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagOrigClOrdID, row["OrigClOrderID"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagTimeInForce, row["TimeInForce"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagTransactTime, row["AUECLocalDate"].ToString());
                    if (row["ExchangeID"] != DBNull.Value)
                    {
                        int exchangeId = Convert.ToInt32(row["ExchangeID"].ToString());
                        msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagExecBroker, CachedDataManager.GetInstance.GetExchangeText(exchangeId));
                    }
                    if (row["CurrencyID"] != DBNull.Value)
                    {
                        int currencyId = Convert.ToInt32(row["CurrencyID"].ToString());
                        msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagCurrency, CachedDataManager.GetInstance.GetCurrencyText(currencyId));
                    }
                    msg.MessageType = CustomFIXConstants.MSG_CounterPartyUp;
                    messages.Add(msg);
                }
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    PranaMessage msg = Transformer.CreatePranaMessageThroughReflection(row);
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagSide, row["OrderSidetagValue"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagSenderSubID, row["SenderSubID"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagAccount, row["TradingAccountID"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagExecInst, row["ExecutionInst"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagHandlInst, row["HandlingInst"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagOrigClOrdID, row["OrigClOrderID"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagTimeInForce, row["TimeInForce"].ToString());
                    msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagTransactTime, row["AUECLocalDate"].ToString());
                    if (row["ExchangeID"] != DBNull.Value)
                    {
                        int exchangeId = Convert.ToInt32(row["ExchangeID"].ToString());
                        msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagExecBroker, CachedDataManager.GetInstance.GetExchangeText(exchangeId));
                    }
                    if (row["CurrencyID"] != DBNull.Value)
                    {
                        int currencyId = Convert.ToInt32(row["CurrencyID"].ToString());
                        msg.FIXMessage.CustomInformation.AddField(FIXConstants.TagCurrency, CachedDataManager.GetInstance.GetCurrencyText(currencyId));
                    }
                    msg.MessageType = CustomFIXConstants.MSG_CounterPartyUp;
                    messages.Add(msg);
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

            return messages;
        }

        /// <summary>
        /// Gets the manual order send schedular data list.
        /// </summary>
        /// <param name="companyId">The company identifier.</param>
        /// <returns></returns>
        public static List<ManualOrderSendSchedularData> GetManualOrderSendSchedularDataList(int companyId)
        {
            List<ManualOrderSendSchedularData> manualOrderDataList = new List<ManualOrderSendSchedularData>();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = companyId;
                DataTable dt = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetManualOrderTriggerDetailsForCompany", parameter, "PranaConnectionString").Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    ManualOrderSendSchedularData manualOrder = new ManualOrderSendSchedularData(row);
                    if (manualOrder.PermitManualOrderSend)
                        manualOrderDataList.Add(manualOrder);
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
            return manualOrderDataList;
        }

        /// <summary>
        /// Sets the last manual order trigger time.
        /// </summary>
        /// <param name="auecId">The auec identifier.</param>
        /// <param name="lastManualOrderRunTriggerTime">The last manual order run trigger time.</param>
        /// <param name="companyId">The company identifier.</param>
        public static void SetLastManualOrderTriggerTime(int auecId, int companyId)
        {

            try
            {
                object[] parameters = new object[3];
                parameters[0] = auecId;
                parameters[1] = DateTime.UtcNow;
                parameters[2] = companyId;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SetLastManualOrderTriggerTime", parameters, "PranaConnectionString");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Gets Blotter Data
        /// <summary>
        /// Gets all order from database.
        /// </summary>
        /// <param name="isAllowMutlidayStagedOrders"></param>
        /// <returns></returns>
        public static List<OrderSingle> GetBlotterLaunchData(bool isAllowMutlidayStagedOrders, List<int> listImpactedAUECID = null)
        {
            List<OrderSingle> orderCollection = new List<OrderSingle>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetBlotterLaunchDataNew";
                queryData.CommandTimeout = 200;
                queryData.DictionaryDatabaseParameter.Add("@AllAUECDatesString", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@AllAUECDatesString",
                    ParameterType = DbType.String,
                    ParameterValue = listImpactedAUECID == null ? TimeZoneHelper.GetInstance().GetAllAUECDateInUseAUECStr(DateTime.UtcNow) : TimeZoneHelper.GetInstance().GetAUECDateBasedOnAUECList(DateTime.UtcNow, listImpactedAUECID)
                });
                queryData.DictionaryDatabaseParameter.Add("@userID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@userID",
                    ParameterType = DbType.Int32,
                    ParameterValue = null
                });
                queryData.DictionaryDatabaseParameter.Add("@isAllowMultidayStagedOrders", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@isAllowMultidayStagedOrders",
                    ParameterType = DbType.Boolean,
                    ParameterValue = isAllowMutlidayStagedOrders
                });

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        OrderSingle order = FillOrderDetails(row, 0);
                        orderCollection.Add(order);
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
            return orderCollection;

        }

        /// <summary>
        /// Create order from row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static OrderSingle FillOrderDetails(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            OrderSingle order = null;
            if (row != null)
            {
                order = new OrderSingle();
                int CLORDERID = offset + 0;
                int PARENTCLORDERID = offset + 1;
                int LAST_PRICE = offset + 2;
                int AVGPRICE = offset + 3;
                int LEAVESQTY = offset + 4;
                int CUMQTY = offset + 5;
                int ORDERSTATUS = offset + 6;
                int LASTSHARES = offset + 7;
                int QUANTITY = offset + 8;
                int SYMBOL = offset + 9;
                int ORDERSIDE = offset + 10;
                int ORDERTYPE = offset + 11;
                int PRICE = offset + 12;
                int ORIGCLORDERID = offset + 13;
                int EXECUTION_ID = offset + 14;
                int CLIENTTIME = offset + 15;
                int COUNTERPARTYID = offset + 16;
                int VENUEID = offset + 17;
                int AUECID = offset + 18;
                int ASSETID = offset + 19;
                int UNDERLYINGID = offset + 20;
                //21: For Country Flag Image
                int STAGEDORDERID = offset + 22;
                int TRADINGACCOUNTID = offset + 23;
                int USERID = offset + 24;
                int Prana_MSG_TYPE = offset + 25;
                int DISCR_OFFSET = offset + 26;
                int PEG_DIFF = offset + 27;
                int STOP_PRICE = offset + 28;
                //29: Clearance Time
                int MATURITY_YEARMONTH = offset + 30;
                int STRIKE_PRICE = offset + 31;
                int PUT_CALL = offset + 32;
                int SECURITY_TYPE = offset + 33;
                //34: OpenClose
                int EXEC_INST = offset + 35;
                int TIMEINFORCE = offset + 36;
                int HANDLINGINST = offset + 37;
                int MESSAGETYPE = offset + 38;
                int CMTA = offset + 39;
                int GIVEUPID = offset + 40;
                int UNDERLYINGSYMBOL = offset + 41;
                int ORDER_ID = offset + 42;
                int ALGOSTRATEGYID = offset + 43;
                int ALGOSTRATEGYPARAMETERS = offset + 44;
                int OriginatorTypeID = offset + 45;
                int clientOrderID = offset + 46;
                int AUECLocalDate = offset + 47;
                int SettlementDate = offset + 48;
                int SenderSubID = offset + 49;
                int CurrencyID = offset + 50;
                int AvgFxRateForTrade = offset + 51;
                int Multiplier = offset + 52;
                int ProcessDate = offset + 53;
                int AccountID = offset + 54;
                int StategyId = offset + 55;
                Int64 OrderSeqNumber = offset + 56;
                int Calcbasis = offset + 57;
                int CommissionRate = offset + 58;
                int CommissionAmt = offset + 59;
                int ImportFileName = offset + 60;
                int ImportFileID = offset + 61;
                int BloombergSymbol = offset + 62;
                int SoftCommissionCalcBasis = offset + 63;
                int SoftCommissionRate = offset + 64;
                int SoftCommissionAmt = offset + 65;
                int TradeAttribute1 = offset + 66;
                int TradeAttribute2 = offset + 67;
                int TradeAttribute3 = offset + 68;
                int TradeAttribute4 = offset + 69;
                int TradeAttribute5 = offset + 70;
                int TradeAttribute6 = offset + 71;
                int InternalComments = offset + 72;
                int settlementCurrency = offset + 73;
                int FxRateCalc = offset + 75;
                #region Swap Parameters
                int IsSwapped = offset + 76;
                int NotionalValue = offset + 77;
                int BenchMarkRate = offset + 78;
                int Differential = offset + 79;
                int OrigCostBasis = offset + 80;
                int DayCount = offset + 81;
                int SwapDescription = offset + 82;
                int FirstResetDate = offset + 83;
                int OrigTransDate = offset + 84;
                int ResetFrequency = offset + 85;
                int ClosingPrice = offset + 86;
                int ClosingDate = offset + 87;
                int TransDate = offset + 88;
                #endregion

                //89: Should Override Notional
                //90: Should Override CostBasis

                // Added change type field
                int ChangeType = offset + 91;
                int text = offset + 92;
                int OriginalAllocationPreferenceID = offset + 93;
                int TransactionSourcetag = offset + 94;
                int LeadCurrencyID = offset + 95;
                int VsCurrencyID = offset + 96;
                int allocationState = offset + 97;
                int accountIDs = offset + 98;
                int strategyIDs = offset + 99;
                int allocationSchemeName = offset + 100;
                int rebalancerFileName = offset + 101;
                int sedolSymbol = offset + 102;
                int companyName = offset + 103;
                int actualCompanyUserID = offset + 104;
                int BorrowerID = offset + 105;
                int BorrowBroker = offset + 106;
                int ShortRebate = offset + 107;
                int NirvanaLocateID = offset + 108;
                int IsManualOrder = offset + 109;
                int activSymbol = offset + 110;
                int factsetSymbol = offset + 111;
                int executionTimeLastFill = offset + 112;
                int expireTime = offset + 113;
                int isUseCustodianBroker = offset + 116;
                int BloombergExchangeCode = offset + 117;
                int originalPurchaseDate = offset + 118;
                int tradeAttributes = offset + 119;

                order.CalcBasis = (CalculationBasis)int.Parse(row[Calcbasis].ToString());
                if (!row[CommissionAmt].ToString().Equals(string.Empty))
                {
                    order.CommissionAmt = double.Parse(row[CommissionAmt].ToString(), System.Globalization.NumberStyles.Float);
                }
                else
                {
                    order.CommissionAmt = 0.0;
                }
                order.CommissionRate = double.Parse(row[CommissionRate].ToString(), System.Globalization.NumberStyles.Float);

                order.SoftCommissionCalcBasis = (CalculationBasis)int.Parse(row[SoftCommissionCalcBasis].ToString());
                if (!row[SoftCommissionAmt].ToString().Equals(string.Empty))
                {
                    order.SoftCommissionAmt = double.Parse(row[SoftCommissionAmt].ToString(), System.Globalization.NumberStyles.Float);
                }
                else
                {
                    order.SoftCommissionAmt = 0.0;
                }
                order.SoftCommissionRate = double.Parse(row[SoftCommissionRate].ToString(), System.Globalization.NumberStyles.Float);

                order.ClOrderID = row[CLORDERID].ToString();
                order.ParentClOrderID = row[PARENTCLORDERID].ToString();
                order.Price = double.Parse(row[PRICE].ToString(), System.Globalization.NumberStyles.Float);
                order.LastPrice = double.Parse(row[LAST_PRICE].ToString(), System.Globalization.NumberStyles.Float);
                order.LeavesQty = double.Parse(row[LEAVESQTY].ToString(), System.Globalization.NumberStyles.Float);
                order.CumQty = double.Parse(row[CUMQTY].ToString(), System.Globalization.NumberStyles.Float);
                order.LastShares = double.Parse(row[LASTSHARES].ToString(), System.Globalization.NumberStyles.Float);
                order.OrderStatusTagValue = row[ORDERSTATUS].ToString().Trim();
                order.AvgPrice = Double.Parse(row[AVGPRICE].ToString(), System.Globalization.NumberStyles.Float);
                order.Quantity = Double.Parse(row[QUANTITY].ToString(), System.Globalization.NumberStyles.Float);
                order.Symbol = row[SYMBOL].ToString();
                order.ExecID = row[EXECUTION_ID].ToString();
                order.OrderSideTagValue = row[ORDERSIDE].ToString().Trim();
                order.OrderTypeTagValue = row[ORDERTYPE].ToString().Trim();
                order.OrigClOrderID = row[ORIGCLORDERID].ToString();
                order.CounterPartyID = int.Parse(row[COUNTERPARTYID].ToString(), System.Globalization.NumberStyles.Integer);
                order.VenueID = int.Parse(row[VENUEID].ToString(), System.Globalization.NumberStyles.Integer);
                order.AUECID = int.Parse(row[AUECID].ToString(), System.Globalization.NumberStyles.Integer);
                order.ClientTime = row[CLIENTTIME].ToString();
                DateTime transTime = DateTime.Parse(row[AUECLocalDate].ToString());
                order.TransactionTime = transTime;
                order.ExecutionTimeLastFill = row[executionTimeLastFill].ToString();
                if (order.ExecutionTimeLastFill == string.Empty)
                    order.ExecutionTimeLastFill = row[executionTimeLastFill].ToString();
                else
                {
                    DateTime dtExecutionTimeLastFill;
                    if (!(order.ExecutionTimeLastFill.ToString().Contains("/")))
                    {
                        dtExecutionTimeLastFill = DateTime.ParseExact(order.ExecutionTimeLastFill, DateTimeConstants.NirvanaDateTimeFormat, null);
                    }
                    else
                    {
                        dtExecutionTimeLastFill = DateTime.Parse(order.ExecutionTimeLastFill);
                    }
                    order.ExecutionTimeLastFill = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(dtExecutionTimeLastFill, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID)).ToString(DateTimeConstants.NirvanaDateTimeFormat);
                }
                order.AssetID = int.Parse(row[ASSETID].ToString(), System.Globalization.NumberStyles.Integer);
                order.UnderlyingID = int.Parse(row[UNDERLYINGID].ToString(), System.Globalization.NumberStyles.Integer);
                order.StagedOrderID = row[STAGEDORDERID].ToString();
                order.TradingAccountID = Int32.Parse(row[TRADINGACCOUNTID].ToString());
                order.CompanyUserID = Int32.Parse(row[USERID].ToString());
                order.PranaMsgType = Int32.Parse(row[Prana_MSG_TYPE].ToString());
                order.DiscretionOffset = Double.Parse(row[DISCR_OFFSET].ToString());
                order.PegDifference = Double.Parse(row[PEG_DIFF].ToString());
                order.StopPrice = Double.Parse(row[STOP_PRICE].ToString());
                order.MaturityMonthYear = row[MATURITY_YEARMONTH].ToString();
                order.StrikePrice = Double.Parse(row[STRIKE_PRICE].ToString());
                order.SecurityType = row[SECURITY_TYPE].ToString();
                order.PutOrCalls = row[PUT_CALL].ToString();
                order.ExecutionInstruction = row[EXEC_INST].ToString().Trim();
                order.TIF = row[TIMEINFORCE].ToString().Trim();
                order.HandlingInstruction = row[HANDLINGINST].ToString().Trim();
                order.MsgType = row[MESSAGETYPE].ToString();
                order.Level1ID = int.Parse(row[AccountID].ToString(), System.Globalization.NumberStyles.Integer);
                if (row[StategyId] != DBNull.Value)
                {
                    order.Level2ID = int.Parse(row[StategyId].ToString(), System.Globalization.NumberStyles.Integer);
                }
                if (row[OrderSeqNumber] != DBNull.Value)
                {
                    order.OrderSeqNumber = Int64.Parse(row[OrderSeqNumber].ToString());
                }
                if (row[CMTA].ToString() != string.Empty)
                {
                    order.CMTAID = int.Parse(row[CMTA].ToString());
                }
                if (row[GIVEUPID].ToString() != string.Empty)
                {
                    order.GiveUpID = int.Parse(row[GIVEUPID].ToString());
                }
                if (row[UNDERLYINGSYMBOL] != DBNull.Value)
                {
                    order.UnderlyingSymbol = row[UNDERLYINGSYMBOL].ToString();
                }
                if (row[ORDER_ID] != DBNull.Value)
                {
                    order.OrderID = row[ORDER_ID].ToString();
                }
                if (row[ALGOSTRATEGYID] != DBNull.Value && row[ALGOSTRATEGYID].ToString() != string.Empty)
                {
                    order.AlgoStrategyID = row[ALGOSTRATEGYID].ToString();
                    order.AlgoStrategyName = AlgoStrategyNamesDetails.GetAlgoStrategyText(order.AlgoStrategyID, order.CounterPartyID);
                }
                if (row[ALGOSTRATEGYPARAMETERS] != DBNull.Value)
                {
                    order.AlgoProperties = new OrderAlgoStartegyParameters(row[ALGOSTRATEGYPARAMETERS].ToString());
                }
                if (row[OriginatorTypeID] != DBNull.Value)
                {
                    order.OriginatorType = int.Parse(row[OriginatorTypeID].ToString());
                }
                if (row[clientOrderID] != DBNull.Value)
                {
                    order.ClientOrderID = row[clientOrderID].ToString();
                }
                if (row[AUECLocalDate] != DBNull.Value)
                {
                    order.AUECLocalDate = Convert.ToDateTime(row[AUECLocalDate]);
                }
                if (row[SettlementDate] != DBNull.Value)
                {
                    order.SettlementDate = Convert.ToDateTime(row[SettlementDate]);
                }
                if (row[SenderSubID] != DBNull.Value)
                {
                    order.SenderSubID = row[SenderSubID].ToString();
                }
                if (row[CurrencyID] != DBNull.Value)
                {
                    order.CurrencyID = int.Parse(row[CurrencyID].ToString());
                }
                if (row[AvgFxRateForTrade] != DBNull.Value)
                {
                    order.FXRate = double.Parse(row[AvgFxRateForTrade].ToString());
                }
                if (row[Multiplier] != DBNull.Value)
                {
                    order.ContractMultiplier = double.Parse(row[Multiplier].ToString());
                }
                if (row[ProcessDate] != DBNull.Value)
                {
                    order.ProcessDate = Convert.ToDateTime(row[ProcessDate].ToString());
                }
                if (row[ImportFileName] != DBNull.Value)
                {
                    order.ImportFileName = row[ImportFileName].ToString();
                }
                if (row[ImportFileID] != DBNull.Value)
                {
                    order.ImportFileID = int.Parse(row[ImportFileID].ToString());
                }
                if (row[BloombergSymbol] != DBNull.Value)
                {
                    order.BloombergSymbol = row[BloombergSymbol].ToString();
                }
                if (row[BloombergExchangeCode] != DBNull.Value)
                {
                    order.BloombergSymbolWithExchangeCode = row[BloombergExchangeCode].ToString();
                }
                if (row[activSymbol] != DBNull.Value)
                {
                    order.ActivSymbol = row[activSymbol].ToString();
                }
                if (row[factsetSymbol] != DBNull.Value)
                {
                    order.FactSetSymbol = row[factsetSymbol].ToString();
                }
                if (row[TradeAttribute1] != DBNull.Value)
                {
                    order.TradeAttribute1 = row[TradeAttribute1].ToString();
                }
                if (row[TradeAttribute2] != DBNull.Value)
                {
                    order.TradeAttribute2 = row[TradeAttribute2].ToString();
                }
                if (row[TradeAttribute3] != DBNull.Value)
                {
                    order.TradeAttribute3 = row[TradeAttribute3].ToString();
                }
                if (row[TradeAttribute4] != DBNull.Value)
                {
                    order.TradeAttribute4 = row[TradeAttribute4].ToString();
                }
                if (row[TradeAttribute5] != DBNull.Value)
                {
                    order.TradeAttribute5 = row[TradeAttribute5].ToString();
                }
                if (row[TradeAttribute6] != DBNull.Value)
                {
                    order.TradeAttribute6 = row[TradeAttribute6].ToString();
                }
                if (row[InternalComments] != DBNull.Value)
                {
                    order.InternalComments = row[InternalComments].ToString();
                }
                if (row[settlementCurrency] != DBNull.Value)
                {
                    order.SettlementCurrencyID = Int32.Parse(row[settlementCurrency].ToString());
                }
                if (row[FxRateCalc] != DBNull.Value)
                {
                    order.FXConversionMethodOperator = row[FxRateCalc].ToString();
                }

                #region Swap Parameters
                if (row[IsSwapped] != DBNull.Value)
                {
                    bool IsSwap = bool.Parse(row[IsSwapped].ToString());
                    if (IsSwap)
                    {
                        SwapParameters swapParameters = new SwapParameters();
                        if (row[NotionalValue] != DBNull.Value)
                        {
                            swapParameters.NotionalValue = double.Parse(row[NotionalValue].ToString());
                        }
                        if (row[BenchMarkRate] != DBNull.Value)
                        {
                            swapParameters.BenchMarkRate = double.Parse(row[BenchMarkRate].ToString());
                        }
                        if (row[Differential] != DBNull.Value)
                        {
                            swapParameters.Differential = double.Parse(row[Differential].ToString());
                        }
                        if (row[OrigCostBasis] != DBNull.Value)
                        {
                            swapParameters.OrigCostBasis = double.Parse(row[OrigCostBasis].ToString());
                        }
                        if (row[DayCount] != DBNull.Value)
                        {
                            swapParameters.DayCount = int.Parse(row[DayCount].ToString());
                        }
                        if (row[SwapDescription] != DBNull.Value)
                        {
                            swapParameters.SwapDescription = row[SwapDescription].ToString();
                        }
                        if (row[FirstResetDate] != DBNull.Value)
                        {
                            swapParameters.FirstResetDate = DateTime.Parse(row[FirstResetDate].ToString());
                        }
                        if (row[OrigTransDate] != DBNull.Value)
                        {
                            swapParameters.OrigTransDate = DateTime.Parse(row[OrigTransDate].ToString());
                        }
                        if (row[ResetFrequency] != DBNull.Value)
                        {
                            swapParameters.ResetFrequency = row[ResetFrequency].ToString();
                        }
                        if (row[ClosingPrice] != DBNull.Value)
                        {
                            swapParameters.ClosingPrice = double.Parse(row[ClosingPrice].ToString());
                        }
                        if (row[ClosingDate] != DBNull.Value)
                        {
                            swapParameters.ClosingDate = DateTime.Parse(row[ClosingDate].ToString());
                        }
                        if (row[TransDate] != DBNull.Value)
                        {
                            swapParameters.TransDate = DateTime.Parse(row[TransDate].ToString());
                        }
                        order.SwapParameters = swapParameters;
                    }
                }
                #endregion

                if (row[ChangeType] != DBNull.Value)
                {
                    order.ChangeType = Int32.Parse(row[ChangeType].ToString());
                }
                if (row[text] != DBNull.Value)
                {
                    order.Text = row[text].ToString();
                }
                if (row[OriginalAllocationPreferenceID] != DBNull.Value)
                {
                    order.OriginalAllocationPreferenceID = int.Parse(row[OriginalAllocationPreferenceID].ToString());
                }
                if (row[TransactionSourcetag] != DBNull.Value)
                {
                    order.TransactionSourceTag = int.Parse(row[TransactionSourcetag].ToString());
                    order.TransactionSource = ((TransactionSource)order.TransactionSourceTag);
                }
                if (row[LeadCurrencyID] != DBNull.Value)
                {
                    order.LeadCurrencyID = Int32.Parse(row[LeadCurrencyID].ToString());
                }
                if (row[VsCurrencyID] != DBNull.Value)
                {
                    order.VsCurrencyID = Int32.Parse(row[VsCurrencyID].ToString());
                }
                if (row[IsManualOrder] != DBNull.Value)
                {
                    order.IsManualOrder = Boolean.Parse(row[IsManualOrder].ToString());
                }

                //Forcefully updated value of Account, Master fund, Strategy and Allocation Status value to Dash (-) in case of Order status is Pending New or Rejected. 
                //Because in case of Pending new and Rejected case, These groups are not visible in Allocation.
                if (order.OrderStatusTagValue != FIXConstants.ORDSTATUS_PendingNew && order.OrderStatusTagValue != FIXConstants.ORDSTATUS_Rejected)
                {
                    if (row[allocationState] != DBNull.Value)
                        order.AllocationStatus = row[allocationState].ToString();

                    if (row[allocationSchemeName] != DBNull.Value)
                        order.AllocationSchemeName = row[allocationSchemeName].ToString();

                    if (row[accountIDs] != DBNull.Value)
                    {
                        //Account
                        List<string> accounts = row[accountIDs].ToString().Split(',').ToList();
                        order.Account = accounts.Distinct().Count() > 1 ? OrderFields.PROPERTY_MULTIPLE : CachedDataManager.GetInstance.GetAccountText(Int32.Parse(accounts[0].ToString()));

                        //MasterFund
                        List<int> masterFunds = new List<int>();
                        accounts.ForEach(accountID =>
                        {
                            int masterFundID = CachedDataManager.GetInstance.GetMasterFundIDFromAccountID(Int32.Parse(accountID));
                            if (!masterFunds.Contains(masterFundID))
                                masterFunds.Add(masterFundID);
                        });

                        if (masterFunds.Distinct().Count() > 1)
                            order.MasterFund = OrderFields.PROPERTY_MULTIPLE;
                        else
                        {
                            string masterFundName = CachedDataManager.GetInstance.GetMasterFund(masterFunds[0]);
                            order.MasterFund = String.IsNullOrEmpty(masterFundName) ? OrderFields.PROPERTY_DASH : masterFundName;
                        }
                    }
                    else
                    {
                        order.Account = OrderFields.PROPERTY_DASH;
                        order.MasterFund = CachedDataManager.GetInstance.IsShowMasterFundonTT() && !string.IsNullOrEmpty(order.TradeAttribute6) ? order.TradeAttribute6 : OrderFields.PROPERTY_DASH;
                        order.AllocationSchemeName = OrderFields.PROPERTY_DASH;
                    }

                    //Strategy
                    if (row[strategyIDs] != DBNull.Value)
                    {
                        List<string> strategyValues = row[strategyIDs].ToString().Split(',').Distinct().ToList();
                        order.Strategy = strategyValues.Count > 1 ? OrderFields.PROPERTY_MULTIPLE : CachedDataManager.GetInstance.GetStrategyText(Convert.ToInt32(strategyValues[0]));
                    }
                    else
                        order.Strategy = OrderFields.PROPERTY_DASH;
                }
                else
                {
                    order.AllocationSchemeName = order.AllocationStatus = order.Strategy = order.Account = order.MasterFund = OrderFields.PROPERTY_DASH;
                }

                if (row[rebalancerFileName] != DBNull.Value)
                {
                    order.RebalancerFileName = row[rebalancerFileName].ToString();
                }
                if (row[sedolSymbol] != DBNull.Value)
                {
                    order.SEDOLSymbol = row[sedolSymbol].ToString();
                }
                if (row[companyName] != DBNull.Value)
                {
                    order.CompanyName = row[companyName].ToString();
                }
                if (row[actualCompanyUserID] != DBNull.Value)
                {
                    order.ActualCompanyUserID = Convert.ToInt32(row[actualCompanyUserID].ToString());
                }
                if (row[BorrowerID] != DBNull.Value)
                {
                    order.BorrowerID = row[BorrowerID].ToString();
                }
                if (row[BorrowBroker] != DBNull.Value)
                {
                    order.BorrowerBroker = row[BorrowBroker].ToString();
                }
                if (row[ShortRebate] != DBNull.Value)
                {
                    order.ShortRebate = Convert.ToDouble(row[ShortRebate].ToString());
                }
                if (row[NirvanaLocateID] != DBNull.Value)
                {
                    order.NirvanaLocateID = Convert.ToInt32(row[NirvanaLocateID].ToString());
                }
                if (row[expireTime] != DBNull.Value && !string.IsNullOrEmpty(row[expireTime].ToString()))
                {
                    DateTime _expireTime = DateTime.ParseExact(row[expireTime].ToString(), DateTimeConstants.NirvanaDateTimeFormat, null);
                    order.ExpireTime = _expireTime.ToString();
                }
                order.IsUseCustodianBroker = bool.Parse(row[isUseCustodianBroker].ToString());
                if (order.IsUseCustodianBroker && order.Account == OrderFields.PROPERTY_MULTIPLE && order.CounterPartyID == int.MinValue)
                {
                    order.CounterPartyName = OrderFields.PROPERTY_MULTIPLE;
                }
                else if (order.CounterPartyID != int.MinValue)
                {
                    order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(order.CounterPartyID);
                }
                if (row[originalPurchaseDate] != DBNull.Value)
                {
                    DateTime dt = DateTimeConstants.MinValue;
                    order.OriginalPurchaseDate = dt;
                    if (DateTime.TryParse(row[originalPurchaseDate].ToString(), out dt))
                    {
                        order.OriginalPurchaseDate = dt;
                    }

                }
                if (row[tradeAttributes] != DBNull.Value)
                {
                    string json = row[tradeAttributes].ToString();
                    order.SetTradeAttribute(json);
                }
            }
            return order;
        }
        #endregion

        #region SetIsHiddenFieldInT_SubTableToTrueForGivenCLOrderID
        /// <summary>
        /// Set is Hidden Field in T_Sub Table To True For Given CLOrderIDs
        /// </summary>
        /// <param name="commaSepratedCLOrderID"></param>
        public static void SetIsHiddenTrueInSub(string commaSepratedCLOrderID)
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SetIsHiddenInSubTable";
                queryData.DictionaryDatabaseParameter.Add("@ParentCLOrderID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ParentCLOrderID",
                    ParameterType = DbType.String,
                    ParameterValue = commaSepratedCLOrderID
                });

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
