using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Data;
namespace Prana.TradeManager
{
    /// <summary>
    /// Summary description for TicketManager.
    /// </summary>
    public class TicketManager
    {
        public enum TicketType
        {
            OPTradingTicket = 1,
            OGTradingTicket = 2
        }

        private static Ticket FillTicket(object[] row, int offSet)
        {
            int TICKET_ID = 0 + offSet;
            int TICKET_NAME = 1 + offSet;
            int DISPLAY_NAME = 2 + offSet;
            int TICKET_TYPE = 3 + offSet;
            //int USER_ID = 4 + offSet;

            Ticket ticket = new Ticket();

            try
            {
                if (row[TICKET_ID] != System.DBNull.Value)
                {
                    ticket.TicketID = int.Parse(row[TICKET_ID].ToString());
                }
                if (row[TICKET_NAME] != System.DBNull.Value)
                {
                    ticket.TicketName = row[TICKET_NAME].ToString();
                }
                if (row[DISPLAY_NAME] != System.DBNull.Value)
                {
                    ticket.DisplayName = row[DISPLAY_NAME].ToString();
                }
                if (row[TICKET_TYPE] != System.DBNull.Value)
                {
                    ticket.TicketType = (EnumTicketType)int.Parse(row[TICKET_TYPE].ToString());
                }
                //				if(row[USER_ID] != System.DBNull.Value)
                //				{			
                //					ticket.TicketType = int.Parse(row[USER_ID].ToString());
                //				}
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, "General Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            return ticket;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static TicketCollection GetTicket(int userID)//, TicketType type)
        {
            TicketCollection ticketCollection = new TicketCollection();

            Object[] parameter = new object[1];
            parameter[0] = (int)userID;
            //parameter[1] = (int)type;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTicketByUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        ticketCollection.Add(FillTicket(row, 0));
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
            return ticketCollection;
        }

        public static PriceSymbolValidation GetPriceSymbolSettings(int UserID)
        {
            PriceSymbolValidation riskValidation = new PriceSymbolValidation();

            object[] parameter = new object[1];
            parameter[0] = UserID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTTRiskValidateSettings", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        riskValidation.RiskCtrlCheck = Convert.ToBoolean(row[0]);
                        riskValidation.ValidateSymbolCheck = Convert.ToBoolean(row[1]);
                        riskValidation.RiskValue = double.Parse(row[2].ToString());
                        riskValidation.CompanyUserID = UserID;
                        riskValidation.LimitPriceCheck = Convert.ToBoolean(row[3]);
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
            return riskValidation;


        }
    }
}
