#region Using

using Prana.LogManager;
using System;
using System.Data;

#endregion

namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for TraderManager.
    /// </summary>
    public class TraderManager
    {
        private TraderManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private static Trader FillTrader(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }

            Trader trader = null;

            try
            {
                if (row != null)
                {
                    trader = new Trader();

                    int TRADERID = offset + 0;
                    int FIRSTNAME = offset + 1;
                    int LASTNAME = offset + 2;
                    int SHORTNAME = offset + 3;
                    int TITLE = offset + 4;
                    int EMAIL = offset + 5;
                    int TELEPHONEWORK = offset + 6;
                    int TELEPHONECELL = offset + 7;
                    int PAGER = offset + 8;
                    int TELEPHONEHOME = offset + 9;
                    int FAX = offset + 10;
                    int COMPANYID = offset + 11;

                    trader.TraderID = Convert.ToInt32(row[TRADERID]);
                    trader.FirstName = Convert.ToString(row[FIRSTNAME]);
                    if (DBNull.Value != row[LASTNAME])
                    {
                        trader.LastName = Convert.ToString(row[LASTNAME]);
                    }

                    if (DBNull.Value != row[SHORTNAME])
                    {
                        trader.ShortName = Convert.ToString(row[SHORTNAME]);
                    }

                    if (DBNull.Value != row[TITLE])
                    {
                        trader.Title = Convert.ToString(row[TITLE]);
                    }

                    trader.EMail = Convert.ToString(row[EMAIL]);
                    trader.TelephoneWork = Convert.ToString(row[TELEPHONEWORK]);
                    if (DBNull.Value != row[TELEPHONECELL])
                    {
                        trader.TelephoneCell = Convert.ToString(row[TELEPHONECELL]);
                    }

                    if (DBNull.Value != row[PAGER])
                    {
                        trader.Pager = Convert.ToString(row[PAGER]);
                    }

                    if (DBNull.Value != row[TELEPHONEHOME])
                    {
                        trader.TelephoneHome = Convert.ToString(row[TELEPHONEHOME]);
                    }

                    if (DBNull.Value != row[FAX])
                    {
                        trader.Fax = Convert.ToString(row[FAX]);
                    }
                    trader.CompanyID = Convert.ToInt32(row[COMPANYID]);
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
            return trader;
        }

        public static int SaveTrader(int companyClientID, Traders traders)
        {

            int result = int.MinValue;

            object[] parameter = new object[1];
            parameter[0] = companyClientID;
            try
            {
                //db.ExecuteNonQuery("P_DeleteCompanyClientTraders", parameter).ToString(); 
                parameter = new object[12];
                foreach (Trader trader in traders)
                {
                    parameter[0] = trader.TraderID;
                    parameter[1] = trader.EMail;
                    parameter[2] = trader.Fax;
                    parameter[3] = trader.FirstName;
                    parameter[4] = trader.LastName;
                    parameter[5] = trader.Pager;
                    parameter[6] = trader.ShortName;
                    parameter[7] = trader.TelephoneCell;
                    parameter[8] = trader.TelephoneHome;
                    parameter[9] = trader.TelephoneWork;
                    parameter[10] = trader.Title;
                    parameter[11] = companyClientID;

                    result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveTrader", parameter).ToString());
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

        public static Traders GetTraders(int companyClientID)
        {
            Traders traders = new Traders();

            object[] parameter = new object[1];
            parameter[0] = companyClientID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClientTraders", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        traders.Add(FillTrader(row, 0));
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
            return traders;
        }

        public static bool DeleteTrader(int traderID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = traderID;

            try
            {
                //if(db.ExecuteNonQuery("P_DeleteClientTrader", parameter) > 0)
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyClientTraderByID", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
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

        public static bool DeleteClientTrader(int traderID)
        {
            bool result = false;
            Object[] parameter = new object[1];
            parameter[0] = traderID;

            try
            {
                //if(db.ExecuteNonQuery("P_DeleteClientTrader", parameter) > 0)
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteCompanyClientTraderByID", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
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

        public static Traders GetTraders(int companyID, int traderID)
        {
            Traders traders = new Traders();

            object[] parameter = new object[2];
            parameter[0] = companyID;
            parameter[1] = traderID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetTradersBoth", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        traders.Add(FillTrader(row, 0));
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
            return traders;
        }
    }
}
