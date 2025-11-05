using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.SqlClient;
using Prana.Global;
namespace Prana.BasketTrading
{
    public class TemplateDataManager
    {

        //public static void SaveaExchangeMappingList(TemplateExchange templateExchage, string templateID)
        //{
        //    //Database db = DatabaseFactory.CreateDatabase();

        //    //object[] parameter = new object[3];

        //    //parameter[0] = templateExchage.ExchangeIDlist;
        //    //parameter[1] = templateExchage.ExchangeMappinglist;
        //    //parameter[2] = templateID;


        //    //#region try
        //    //try
        //    //{
        //    //    db.ExecuteScalar("[P_BTSaveExchangeMapping]", parameter);
        //    //}
        //    //# endregion

        //    //#region Catch
        //    //catch (Exception ex)
        //    //{
        //    //    // Invoke our policy that is responsible for making sure no secure information
        //    //    // gets out of our layer.
        //    //    bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);


        //    //    if (rethrow)
        //    //    {
        //    //        throw;
        //    //    }
        //    //}
        //    //#endregion


        //}

        public static void SaveOrderSideMappingList(TemplateOrderSide templateorderSide, string templateID)
        {
            Database db = DatabaseFactory.CreateDatabase();

            object[] parameter = new object[2];

            parameter[0] = templateID;
            parameter[1] = templateorderSide.SideMappingString;

            #region try
            try
            {

                db.ExecuteScalar("[P_SaveTemplateOrderSideMapping]", parameter);

            }
            # endregion

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


        }

        public static void SaveConventionMappingList(TemplateConvention templateconvention)
        {
            Database db = DatabaseFactory.CreateDatabase();

            object[] parameter = new object[3];

            parameter[0] = templateconvention.ConventionMappingID;
            parameter[1] = templateconvention.Percentage;
            parameter[2] = templateconvention.RoundLot;

            #region try
            try
            {

                db.ExecuteScalar("[P_SaveTemplateConventionMapping]", parameter);

            }
            # endregion

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


        }

        public static DataTable GetTemplatesByAUID()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TemplateID");
            dt.Columns.Add("TemplateName");

            Database db = DatabaseFactory.CreateDatabase();
            try
            {

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_GetTemplateByAUID"))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            dt.Rows.Add(row);

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
            return dt;
        }

        public static List<string> GetTemplateColumns(string templateID)
        {
            List<string> templatecolumnList = new List<string>();
            
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = templateID;


                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_GetTemplateColumnsByTemplateID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        string[] templateColumnCommastring  = row[0].ToString().Split(',');
                        foreach (string column in templateColumnCommastring)
                        {
                            templatecolumnList.Add(column);
                        }


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

            return templatecolumnList;

        }

        public static DataTable GetCurrencies()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CURRENCYID");
            dt.Columns.Add("CURRENCYNAME");
            dt.Columns.Add("CURRENCYSYMBOL");
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_GetCurrencies"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dt.Rows.Add(row);

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
            return dt;
        }

        public static DataTable GetTemplatesFromDB()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TemplateID");
            dt.Columns.Add("TemplateName");

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "BTGetTemplates"))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dt.Rows.Add(row);
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
            return dt;
        }

        public static BasketTemplate GetTemplateDetails(string templateID)
        {
            BasketTemplate template = new BasketTemplate();
            template.TemplateOrderSide = new TemplateOrderSide();
            template.TemplateExchange = new TemplateExchange();
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = templateID;


                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("BTGetTemplateDetails", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        template.TemplateName = row[0].ToString();
                        template.SetColumns(row[1].ToString());
                        //template.AssetID = int.Parse(row[2].ToString());
                        //template.UnderLyingID = int.Parse(row[3].ToString());
                        template.TemplateOrderSide.SideMappingString = row[2].ToString();
                        template.IsDefaultTemplate = Convert.ToBoolean(row[3].ToString());
                        template.TemplateExchange.ExchangeMappinglist = row[4].ToString();

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

            return template;
        }

        //public static string GetSideMappingString(string templateID)
        //{
        //    string sideMappingString = string.Empty;
        //    Database db = DatabaseFactory.CreateDatabase();
        //    try
        //    {
        //        object[] parameter = new object[1];
        //        parameter[0] = templateID;


        //        using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetSideMappingString", parameter))
        //        {
        //            while (reader.Read())
        //            {
        //                object[] row = new object[reader.FieldCount];
        //                reader.GetValues(row);
        //                sideMappingString = row[0].ToString();

        //            }
        //        }
        //    }
        //    #region Catch
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    #endregion
        //    return sideMappingString;

        //}

        public static string  GetDefaultTemplate( int userID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            string  templateID = string.Empty;
            try
            {
                object[] parameter = new object[1];
              
                parameter[0] = userID;


                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetDefaultTemplate", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        templateID = row[0].ToString();
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

            return templateID;
        }
    }
}

