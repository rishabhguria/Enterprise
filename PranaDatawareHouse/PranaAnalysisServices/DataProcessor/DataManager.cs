using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Data.SqlClient;
using System.IO;

namespace DataProcessor
{
    class DataManager
    {

        internal static DataTable GetPMTaxlotsData()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DataTable dt = null;

            try
            {
                dt = db.ExecuteDataSet("P_GetAllPMTaxlots").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }

        internal static DataTable GetMarkPriceData()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DataTable dt = null;

            try
            {
                dt = db.ExecuteDataSet("P_GetMarkPrices").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }

        internal static List<int> GetAllComapanyFunds()
        {
            Database db = DatabaseFactory.CreateDatabase();

            List<int> comapnyFunds = new List<int>();

            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetAllComapanyFunds"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        comapnyFunds.Add(Convert.ToInt32(row[0].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return comapnyFunds;

        }

        internal static void SavePMOpenPositionSnapShot(DataTable dtOpen)
        {
            Database db = DatabaseFactory.CreateDatabase();
            object[] parameters = new object[1];

            MemoryStream stream = new MemoryStream();
            dtOpen.WriteXml(stream);
            
            byte[] bytes = stream.ToArray();
            string xml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            try
            {
                parameters[0] = xml;
                db.ExecuteNonQuery("P_SaveOpenPositionSnapShot", parameters);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal static List<int> GetAllSpecialDatesOfCurrentYear()
        {
            Database db = DatabaseFactory.CreateDatabase();

            List<int> specialDates = new List<int>();

            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetAllSpecialDates"))
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
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return specialDates;
        }

        internal static void SaveAdditionalPMTaxlots(DataTable dtPM)
        {
            Database db = DatabaseFactory.CreateDatabase();
            object[] parameters = new object[1];

            MemoryStream stream = new MemoryStream();
            dtPM.WriteXml(stream);

            byte[] bytes = stream.ToArray();
            string xml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            try
            {
                parameters[0] = xml;
                db.ExecuteNonQuery("P_SaveAdditionalPMTalots", parameters);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        internal static void SaveURPNLData(DataTable dtURPNL)
        {
            Database db = DatabaseFactory.CreateDatabase();
            object[] parameters = new object[1];

            MemoryStream stream = new MemoryStream();
            dtURPNL.WriteXml(stream);

            byte[] bytes = stream.ToArray();
            string xml = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
            try
            {
                parameters[0] = xml;
                db.ExecuteNonQuery("P_SaveMarkToMarketPNL", parameters);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        internal static DataTable GetPMClosingData()
        {
            Database db = DatabaseFactory.CreateDatabase();

            DataTable dt = null;

            try
            {
                dt = db.ExecuteDataSet("P_GetAllPMClosingTaxlots").Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            return dt;
        }
    }
}
