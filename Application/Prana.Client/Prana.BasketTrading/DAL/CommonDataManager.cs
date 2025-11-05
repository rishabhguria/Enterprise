using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.SqlClient;
using Prana.Global;
using Prana.BusinessObjects;
using System.Data;

namespace Prana.BasketTrading
{
    public class CommonDataManager
    {
        /// <summary>
        /// Returns Traded bakset  by tradedbasketID 
        /// </summary>
        /// <param name="tradedBasketID"></param>
        /// <returns></returns>
        /// 
        public static VenueCollection GetVenuesByAUIDCounterPartyAndUserID(int userID, int counterPartyID, int assetID, int underlyingID)
        {
            //TODO: Write SP to get CounterParty for a user according to his permission.
            VenueCollection venues = new VenueCollection();

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[4];
                parameter[0] = userID;
                parameter[1] = counterPartyID;
                parameter[2] = assetID;
                parameter[3] = underlyingID;
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetVenuesByAUIDCounterPartyAndUserID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        venues.Add(FillVenues(row, 0));
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return venues;
        }

        private static Venue FillVenues(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Venue venue = null;
            try
            {
                if (row != null)
                {
                    venue = new Venue();
                    int VENUE_ID = offset + 0;
                    int VENUE_NAME = offset + 1;
                    //int VENUETYPEID = offset + 2;
                    //int ROUTE = offset + 3;

                    venue.VenueID = Convert.ToInt32(row[VENUE_ID]);
                    venue.Name = Convert.ToString(row[VENUE_NAME]);
                }
            }
            #region Catch
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return venue;
        }

        public static DataTable GetTradingAccountsOfUser(int userID)
        {
            Prana.BusinessObjects.TradingAccountCollection  tradingAccs = Prana.CommonDataCache.ClientsCommonDataManager.GetTradingAccounts(userID);
            //TradingAccounts tradinga  ccounts = CompanyManager.GetTradingAccountsForUser

            DataTable dt = new DataTable();
            dt.Columns.Add("TradingAccountID");
            dt.Columns.Add("Name");

            foreach (Prana.BusinessObjects.TradingAccount account in tradingAccs)
            {
                DataRow rowAccount = dt.NewRow();
                rowAccount[0] = account.TradingAccountID.ToString();
                rowAccount[1] = account.Name.ToString();
                dt.Rows.Add(rowAccount);
            }
            return dt;
        }

    }
}
