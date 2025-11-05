using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;


namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for OrderManager.
    /// </summary>
    public class OrderManager
    {
        public OrderManager()
        {
        }

        #region Basec methods like Add/Update/Delete/Get for Side

        private static Side FillSide(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            Side side = new Side();

            try
            {
                if (row != null)
                {
                    int SIDEID = offset + 0;
                    int SIDE = offset + 1;
                    int SIDETAGVALUE = offset + 2;

                    side.SideID = Convert.ToInt32(row[SIDEID]);
                    if (DBNull.Value != row[SIDE])
                    {

                        side.Name = Convert.ToString(row[SIDE]);
                    }
                    if (DBNull.Value != row[SIDETAGVALUE])
                    {

                        side.TagValue = Convert.ToString(row[SIDETAGVALUE]);
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
            return side;
        }


        public static Sides GetSides()
        {
            Sides sides = new Sides();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllSides";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        sides.Add(FillSide(row, 0));
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
            return sides;
        }
        public static Sides GetOrderSidesByCVAUEC(int assetID, int UnderLyingID, int counterPartyID, int venueID)
        {
            //TODO: Write SP to get OrderSides for a user according to his permission.
            Sides orderSides = new Sides();

            try
            {
                object[] parameter = new object[4];
                parameter[0] = assetID;
                parameter[1] = UnderLyingID;
                parameter[2] = counterPartyID;
                parameter[3] = venueID;
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOrderSidesByAUCVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderSides.Add(FillSide(row, 0));
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
            return orderSides;
        }
        public static Side GetSide(int sideID)
        {
            Side side = new Side();

            object[] parameter = new object[1];
            parameter[0] = sideID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetSide", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        side = FillSide(row, 0);
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
            return side;
        }

        public static int SaveSide(Side side)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];
            parameter[0] = side.SideID;
            parameter[1] = side.Name;
            parameter[2] = side.TagValue;
            parameter[3] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveSide", parameter).ToString());
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
            return result;
        }

        public static bool DeleteSide(int sideID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = sideID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteSide", parameter) > 0)
                {
                    result = true;
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
            return result;
        }


        #endregion

        #region Basec methods like Add/Update/Delete/Get for OrderType

        private static OrderType FillOrderType(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            OrderType orderType = new OrderType();
            try
            {
                if (row != null)
                {
                    int ORDERTYPESID = offset + 0;
                    int ORDERTYPES = offset + 1;
                    int ORDERTYPESTAGVALUE = offset + 2;
                    orderType.OrderTypesID = Convert.ToInt32(row[ORDERTYPESID]);
                    if (DBNull.Value != row[ORDERTYPES])
                    {

                        orderType.Type = Convert.ToString(row[ORDERTYPES]);
                    }
                    orderType.TagValue = Convert.ToString(row[ORDERTYPESTAGVALUE]);
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
            return orderType;
        }

        public static OrderTypes GetOrderTypes()
        {
            OrderTypes orderTypes = new OrderTypes();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllOrderTypes";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderTypes.Add(FillOrderType(row, 0));
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
            return orderTypes;
        }


        public static OrderType GetOrderType(int orderTypesID)
        {
            OrderType orderType = new OrderType();

            object[] parameter = new object[1];
            parameter[0] = orderTypesID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOrderType", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderType = FillOrderType(row, 0);
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
            return orderType;
        }

        public static int SaveOrderType(OrderType orderType)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];
            parameter[0] = orderType.OrderTypesID;
            parameter[1] = orderType.Type;
            parameter[2] = orderType.TagValue;
            parameter[3] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveOrderType", parameter).ToString());
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
            return result;
        }

        public static OrderTypes GetOrderTypesByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            OrderTypes orderTypes = new OrderTypes();
            object[] parameter = new object[4];
            parameter[0] = assetID;
            parameter[1] = underlyingID;
            parameter[2] = counterPartyID;
            parameter[3] = venueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetOrderTypesByAUCVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        orderTypes.Add(FillOrderType(row, 0));
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
            return orderTypes;
        }

        public static bool DeleteOrderType(int orderTypeID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = orderTypeID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteOrderType", parameter) > 0)
                {
                    result = true;
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
            return result;
        }

        #endregion

        #region Basec methods like Add/Update/Delete/Get for TimeInForce
        private static TimeInForce FillTimeInForce(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            TimeInForce timeInForce = new TimeInForce();
            try
            {
                if (row != null)
                {
                    int TIMEINFORCEID = offset + 0;
                    int TIMEINFORCE = offset + 1;
                    int TIMEINFORCETAGVALUE = offset + 2;



                    timeInForce.TimeInForceID = Convert.ToInt32(row[TIMEINFORCEID]);
                    if (DBNull.Value != row[TIMEINFORCE])
                    {

                        timeInForce.Name = Convert.ToString(row[TIMEINFORCE]);
                    }
                    if (DBNull.Value != row[TIMEINFORCETAGVALUE])
                    {

                        timeInForce.TagValue = Convert.ToString(row[TIMEINFORCETAGVALUE]);
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
            return timeInForce;
        }


        public static TimeInForces GetTimeInForces()
        {
            TimeInForces timeInForces = new TimeInForces();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllTimeInForce";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        timeInForces.Add(FillTimeInForce(row, 0));
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
            return timeInForces;
        }

        public static TimeInForce GetTimeInForce(int timeInForceID)
        {
            TimeInForce timeInForce = new TimeInForce();

            object[] parameter = new object[1];
            parameter[0] = timeInForceID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTimeInForce", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        timeInForce = FillTimeInForce(row, 0);
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
            return timeInForce;
        }

        public static TimeInForces GetTIFByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            TimeInForces timeInForces = new TimeInForces();

            object[] parameter = new object[4];
            parameter[0] = assetID;
            parameter[1] = underlyingID;
            parameter[2] = counterPartyID;
            parameter[3] = venueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTIFByAUCVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        timeInForces.Add(FillTimeInForce(row, 0));
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
            return timeInForces;
        }
        public static int SaveTimeInForce(TimeInForce timeInForce)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];
            parameter[0] = timeInForce.TimeInForceID;
            parameter[1] = timeInForce.Name;
            parameter[2] = timeInForce.TagValue;
            parameter[3] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveTimeInForce", parameter).ToString());
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
            return result;
        }

        public static bool DeleteTimeInForce(int timeInForceID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = timeInForceID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteTimeInForce", parameter) > 0)
                {
                    result = true;
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
            return result;
        }

        #endregion

        #region Basec methods like Add/Update/Delete/Get for Handling Instruction
        private static HandlingInstruction FillHandlingInstruction(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            HandlingInstruction handlingInstruction = new HandlingInstruction();
            try
            {
                if (row != null)
                {
                    int HANDLINGINSTRUCTIONSID = offset + 0;
                    int HANDLINGINSTRUCTIONS = offset + 1;
                    int HANDLINGINSTRUCTIONSTAGVALUE = offset + 2;



                    handlingInstruction.HandlingInstructionID = Convert.ToInt32(row[HANDLINGINSTRUCTIONSID]);
                    if (DBNull.Value != row[HANDLINGINSTRUCTIONS])
                    {

                        handlingInstruction.Name = Convert.ToString(row[HANDLINGINSTRUCTIONS]);
                    }
                    if (DBNull.Value != row[HANDLINGINSTRUCTIONSTAGVALUE])
                    {

                        handlingInstruction.TagValue = Convert.ToString(row[HANDLINGINSTRUCTIONSTAGVALUE]);
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
            return handlingInstruction;


        }

        public static HandlingInstructions GetHandlingInstructions()
        {
            HandlingInstructions handlingInstructions = new HandlingInstructions();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllHandlingInstructions";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        handlingInstructions.Add(FillHandlingInstruction(row, 0));
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
            return handlingInstructions;
        }

        public static HandlingInstructions GetHandlingInstructionByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            HandlingInstructions handlingInstructions = new HandlingInstructions();
            object[] parameter = new object[4];
            parameter[0] = assetID;
            parameter[1] = underlyingID;
            parameter[2] = counterPartyID;
            parameter[3] = venueID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetHandlingInstructionByAUCVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        handlingInstructions.Add(FillHandlingInstruction(row, 0));
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
            return handlingInstructions;
        }

        public static HandlingInstruction GetHandlingInstruction(string handlingInstructionID)
        {
            HandlingInstruction handlingInstruction = new HandlingInstruction();

            object[] parameter = new object[1];
            parameter[0] = handlingInstructionID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetHandlingInstruction", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        handlingInstruction = FillHandlingInstruction(row, 0);
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
            return handlingInstruction;
        }


        public static int SaveHandlingInstruction(HandlingInstruction handlingInstruction)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];
            parameter[0] = handlingInstruction.HandlingInstructionID;
            parameter[1] = handlingInstruction.Name;
            parameter[2] = handlingInstruction.TagValue;
            parameter[3] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveHandlingInstruction", parameter).ToString());
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
            return result;
        }

        public static bool DeleteHandlingInstruction(int handlingInstructionID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = handlingInstructionID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteHandlingInstructions", parameter) > 0)
                {
                    result = true;
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
            return result;
        }


        #endregion

        #region Basec methods like Add/Update/Delete/Get for Execution Instruction
        private static ExecutionInstruction FillExecutionInstruction(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            ExecutionInstruction executionInstruction = new ExecutionInstruction();
            try
            {
                if (row != null)
                {
                    int T_EXECUTIONINSTRUCTIONSID = offset + 0;
                    int EXECUTIONINSTRUCTIONS = offset + 1;
                    int EXECUTIONINSTRUCTIONSTAGVALUE = offset + 2;



                    executionInstruction.ExecutionInstructionsID = Convert.ToInt32(row[T_EXECUTIONINSTRUCTIONSID]);
                    if (DBNull.Value != row[EXECUTIONINSTRUCTIONS])
                    {

                        executionInstruction.ExecutionInstructions = Convert.ToString(row[EXECUTIONINSTRUCTIONS]);
                    }
                    executionInstruction.TagValue = Convert.ToString(row[EXECUTIONINSTRUCTIONSTAGVALUE]);


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
            return executionInstruction;
        }

        public static ExecutionInstructions GetExecutionInstructions()
        {
            ExecutionInstructions executionInstructions = new ExecutionInstructions();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllExecutionInstructions";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        executionInstructions.Add(FillExecutionInstruction(row, 0));
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
            return executionInstructions;
        }

        public static ExecutionInstruction GetExecutionInstruction(string executionInstructionID)
        {
            ExecutionInstruction executionInstruction = new ExecutionInstruction();

            object[] parameter = new object[1];
            parameter[0] = executionInstructionID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetExecutionInstruction", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        executionInstruction = FillExecutionInstruction(row, 0);
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
            return executionInstruction;
        }

        public static int SaveExecutionInstruction(ExecutionInstruction executionInstruction)
        {
            int result = int.MinValue;

            object[] parameter = new object[4];
            parameter[0] = executionInstruction.ExecutionInstructionsID;
            parameter[1] = executionInstruction.ExecutionInstructions;
            parameter[2] = executionInstruction.TagValue;
            parameter[3] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveExecutionInstruction", parameter).ToString());
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
            return result;
        }

        public static ExecutionInstructions GetExecutionInstructionByAUCVID(int assetID, int underlyingID, int counterPartyID, int venueID)
        {
            ExecutionInstructions executionInstructions = new ExecutionInstructions();

            object[] parameter = new object[4];
            parameter[0] = assetID;
            parameter[1] = underlyingID;
            parameter[2] = counterPartyID;
            parameter[3] = venueID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetExecutionInstructionByAUCVID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        executionInstructions.Add(FillExecutionInstruction(row, 0));
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
            return executionInstructions;
        }

        public static bool DeleteExecutionInstruction(int executionInstructionID)
        {
            bool result = false;

            try
            {
                object[] parameter = new object[1];
                parameter[0] = executionInstructionID;
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteExecutionInstruction", parameter) > 0)
                {
                    result = true;
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
            return result;
        }


        #endregion

        #region Basec methods like Add/Update/Delete/Get for Advanced Order
        private static AdvancedOrder FillAdvancedOrder(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            AdvancedOrder advancedOrder = new AdvancedOrder();
            try
            {

                if (row != null)
                {
                    int ADVANCEDORDERSID = offset + 0;
                    int ADVANCEDORDERS = offset + 1;




                    advancedOrder.AdvancedOrdersID = Convert.ToInt32(row[ADVANCEDORDERSID]);
                    if (DBNull.Value != row[ADVANCEDORDERS])
                    {

                        advancedOrder.AdvancedOrders = Convert.ToString(row[ADVANCEDORDERS]);
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
            return advancedOrder;
        }

        public static AdvancedOrders GetAdvancedOrders()
        {
            AdvancedOrders advancedOrders = new AdvancedOrders();

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "P_GetAllAdvancedOrders";

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        advancedOrders.Add(FillAdvancedOrder(row, 0));
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
            return advancedOrders;
        }

        public static AdvancedOrder GetAdvancedOrder(int advancedOrderID)
        {
            AdvancedOrder advancedOrder = new AdvancedOrder();

            object[] parameter = new object[1];
            parameter[0] = advancedOrderID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetAdvancedOrder", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        advancedOrder = FillAdvancedOrder(row, 0);
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
            return advancedOrder;
        }

        public static int SaveAdvancedOrder(AdvancedOrder advancedOrder)
        {
            int result = int.MinValue;

            object[] parameter = new object[3];
            parameter[0] = advancedOrder.AdvancedOrdersID;
            parameter[1] = advancedOrder.AdvancedOrders;
            parameter[2] = int.MinValue;
            try
            {
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveAdvancedOrder", parameter).ToString());
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
            return result;
        }


        #endregion

    }

}
