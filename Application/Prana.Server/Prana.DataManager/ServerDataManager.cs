using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
namespace Prana.DataManager
{
    public class ServerDataManager
    {
        /// <summary>
        /// for getting All rading Accounts of a Company
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllTradingAccounts()
        {
            List<string> tradingAccountIDs = new List<string>();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetAllTradingAccounts";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        tradingAccountIDs.Add(row[0].ToString());
                    }
                }
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
            return tradingAccountIDs;
            #endregion Catch
        }

        public static OrderCollection GetPossibleUnSentOrders()
        {
            OrderCollection orders = new OrderCollection();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetPossibleUnSentOrders";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orders.Add(FillOrder(row, 0));
                    }
                }
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
            return orders;
            #endregion Catch
        }
        private static Order FillOrder(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            Order order = new Order();

            if (row != null)
            {
                int ParentClOrderID = offset + 0;
                int ClOrderID = offset + 1;
                int OrderSidetagValue = offset + 2;
                int Symbol = offset + 3;
                int OrderTypeTagvalue = offset + 4;
                int UserID = offset + 5;
                int TradingAccountID = offset + 6;
                int AUECID = offset + 7;
                int CounterPartyID = offset + 8;
                int VenueID = offset + 9;
                int AccountID = offset + 10;
                int StrategyID = offset + 11;
                int MsgType = offset + 12;
                int PranaMsgType = offset + 13;
                int Quantity = offset + 14;
                int Price = offset + 15;
                int ExecutionInst = offset + 16;
                int TimeInForce = offset + 17;
                int HandlingInst = offset + 18;
                int InsertionTime = offset + 19;

                try
                {
                    order.ParentClOrderID = row[ParentClOrderID].ToString();
                    order.ClOrderID = row[ClOrderID].ToString();
                    order.OrderSideTagValue = row[OrderSidetagValue].ToString();
                    order.Symbol = row[Symbol].ToString();
                    order.OrderTypeTagValue = row[OrderTypeTagvalue].ToString();
                    order.CompanyUserID = int.Parse(row[UserID].ToString());
                    order.TradingAccountID = int.Parse(row[TradingAccountID].ToString());
                    order.AUECID = int.Parse(row[AUECID].ToString());
                    order.CounterPartyID = int.Parse(row[CounterPartyID].ToString());
                    order.VenueID = int.Parse(row[VenueID].ToString());
                    order.Level1ID = int.Parse(row[AccountID].ToString());
                    order.Level2ID = int.Parse(row[StrategyID].ToString());
                    order.MsgType = row[MsgType].ToString();
                    order.PranaMsgType = int.Parse(row[PranaMsgType].ToString());
                    order.Quantity = double.Parse(row[Quantity].ToString());
                    order.Price = double.Parse(row[Price].ToString());
                    order.ExecutionInstruction = row[ExecutionInst].ToString();
                    order.TIF = row[TimeInForce].ToString();
                    order.HandlingInstruction = row[HandlingInst].ToString();
                    order.SendingTime = row[InsertionTime].ToString();
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
            return order;
        }

        /// <summary>
        /// Gets last sequence number received from a counterParty .
        /// used for Troubleshooting the Server in abnormal shutdown
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static Int64 GetCounterPartyLastMsgSeqNumber(string identifier, DateTime resetTime)
        {
            object[] parameter = new object[2];
            Int64 lastMsgSeqNumberReceived = Int64.MinValue;
            parameter[0] = identifier;
            parameter[1] = resetTime;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetLastSeqNumberReceived", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0].ToString() != string.Empty)
                            lastMsgSeqNumberReceived = Int64.Parse(row[0].ToString());
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
            return lastMsgSeqNumberReceived;
        }
        /// <summary>
        /// Sets that Server is ShutDown Normally
        /// </summary>
        public static void SetServerShutDownStatus()
        {
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_SetServerShutDownStatus";

                DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
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
        /// Get Last ShutDown Status
        /// </summary>
        public static bool GetServerShutDownStatus()
        {
            bool ServerShutDownStatus = false;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetServerShutDownStatus";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0].ToString() == "1")
                            ServerShutDownStatus = true;
                    }
                }
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

            #endregion Catch

            return ServerShutDownStatus;
        }

        public static long GetMaxSeqNumber()
        {
            Int64 orderSeqNumber = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMaxSeqNumber";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0].ToString() != string.Empty)
                        {
                            orderSeqNumber = Int64.Parse(row[0].ToString());
                        }
                    }
                }
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
            #endregion Catch

            return orderSeqNumber;
        }

        /// <summary>
        /// Picks up the max id from T_journal. This id is further used to generate the new distinct ids.
        /// </summary>
        /// <returns></returns>
        public static Int64 GetMaxGeneratedIDFromDB()
        {
            Int64 maxGeneratedID = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMaxGeneratedNumber";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value && row[0].ToString() != string.Empty)
                        {
                            maxGeneratedID = Int64.Parse(row[0].ToString());
                        }
                    }
                }
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
            #endregion Catch
            return maxGeneratedID;
        }

        /// <summary>
        /// Picks up the max id for allocation. This id is further used to generate the new distinct ids.
        /// </summary>
        /// <returns></returns>
        public static string GetMaxGeneratedIDFromDBForGroup()
        {
            string maxGeneratedID = string.Empty;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMaxGeneratedNumberForGroup";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value && row[0].ToString() != string.Empty)
                        {
                            maxGeneratedID = row[0].ToString();
                        }
                    }
                }
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
            return maxGeneratedID;
        }

        /// <summary>
        /// Picks up the max id for Orders. This id is further used to generate the new distinct ids.
        /// </summary>
        /// <returns></returns>
        public static Int64 GetMaxGeneratedIDFromDBForOrders()
        {
            Int64 maxGeneratedID = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMaxGeneratedNumberForOrders";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value && row[0].ToString() != string.Empty)
                        {
                            maxGeneratedID = Int64.Parse(row[0].ToString());
                        }
                    }
                }
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
            #endregion Catch
            return maxGeneratedID;
        }

        public static long GetMaxSymbolPKIDFromDB()
        {
            Int64 maxGeneratedID = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_GetMaxSymbolPKNumber";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData, "SMConnectionString"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        if (row[0] != DBNull.Value && row[0].ToString() != string.Empty)
                        {
                            maxGeneratedID = Int64.Parse(row[0].ToString());
                        }
                    }
                }
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

            #endregion Catch

            return maxGeneratedID;
        }

        /// <summary>
        /// Saves PTT allocation mapping to DB 
        /// </summary>
        /// <param name="originalAllocationPrefId"></param>
        /// <param name="allocationPrefId"></param>
        public static void SavePTTAllocationMapping(int originalAllocationPrefId, string allocationPrefId)
        {
            try
            {
                object[] parameter = new object[2];
                parameter[0] = originalAllocationPrefId;
                parameter[1] = allocationPrefId;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SavePTTAllocationMapping", parameter);
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
    }
}