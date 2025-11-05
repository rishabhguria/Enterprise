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
    public class FilterDataManager
    {
        /// <summary>
        /// Returns All Filter Types Like Volume,Value,
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFilterTypes()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FilterTypeID");
            dt.Columns.Add("FilterTypeName");
            DataRow row = dt.NewRow();
            row[0] = int.MinValue;
            row[1] = ApplicationConstants.C_COMBO_SELECT;
            dt.Rows.Add(row);
            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_GetFilterTypes"))
                {
                    while (reader.Read())
                    {
                        object[] rows = new object[reader.FieldCount];
                        reader.GetValues(rows);
                        dt.Rows.Add(rows);
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
        //TBD
        /// <summary>
        /// Get Benchmark like ADV for given Filter
        /// </summary>
        /// <param name="filterTypeID"></param>
        /// <returns></returns>
        public static DataTable GetBenchmarksByFilterTypeID(int filterTypeID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BenchmarkID");
            dt.Columns.Add("BenchmarkName");

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                object[] parameter = new object[1];
                parameter[0] = filterTypeID;

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetBenchmarkByFilterTypeID", parameter))
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
        /// <summary>
        /// Get List Of Operators like > =>=
        /// </summary>
        /// <returns></returns>
        public static DataTable GetOperatorList()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OperatorID");
            dt.Columns.Add("Symbol");

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_BTGetOperatorList"))
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

        public static void SaveFilterName(string filterID, string filterName)
        {
            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[2];
            parameter[0] = filterID;
            parameter[1] = filterName;
           
            #region try
            try
            {                
                db.ExecuteScalar("[P_BTSaveFilterName]", parameter);
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

        public static void DeleteFilterDetailsByFilterID(string filterID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[1];
            parameter[0] = filterID;           

            #region try
            try
            {
                db.ExecuteScalar("[P_BTDeleteFilterDetailsByFilterID]", parameter);
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

        /// <summary>
        /// To Save Filter Details
        /// </summary>
        /// <param name="filterdetails"></param>
        public static void SaveFilterDetails(BasketFilter filterdetails)
        {
            Database db = DatabaseFactory.CreateDatabase();

            object[] parameter = new object[5];

            parameter[0] = filterdetails.FilterTypeID;
            parameter[1] = filterdetails.FilterID;
            parameter[2] = filterdetails.BenchmarkID;
            parameter[3] = filterdetails.OperatorID;
            parameter[4] = filterdetails.Percentage;

            #region try
            try
            {                
                
                db.ExecuteScalar("[P_BTSaveFilterDetails]", parameter);

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
        /// <summary>
        /// Returns all Filter Names and Its IDS
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllFilterNames()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FilterID");
            dt.Columns.Add("FilterName");

            Database db = DatabaseFactory.CreateDatabase();
            try
            {
                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader(CommandType.StoredProcedure, "P_BTGetFilterNameList"))
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

        /// <summary>
        /// Returns Filter Name
        /// </summary>
        /// <param name="filterID"></param>
        /// <returns></returns>
        public static string GetFilterNameByID(string filterID)
        {
            string filterName = string.Empty;

            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[1];

            parameter[0] = filterID;

            #region try
            try
            {

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetFilterNameByID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        filterName = row[0].ToString();

                    }
                }
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

            return filterName;

        }

        public static void DeleteFilterByFilterID(string filterID)
        {
            DeleteFilterDetailsByFilterID(filterID);
            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[1];
            parameter[0] = filterID;
         
             #region try
            try
            {
                db.ExecuteScalar("P_BTDeleteFilterByFilterID", parameter);
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

        /// <summary>
        /// Returns all Filters associated with a Filter Name
        /// </summary>
        /// <param name="filterID"></param>
        /// <returns></returns>
        public static BasketFilterCollection GetFilterCollectionByID(string filterID)
        {
            BasketFilterCollection _basketFilterCollection = new BasketFilterCollection();

            Database db = DatabaseFactory.CreateDatabase();
            object[] parameter = new object[1];
            parameter[0] = filterID;
            try
            {

                using (SqlDataReader reader = (SqlDataReader)db.ExecuteReader("P_BTGetFilterDetailsByID", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        BasketFilter basketFilter = new BasketFilter();
                        basketFilter.FilterID = filterID;
                        basketFilter.FilterTypeID = int.Parse(row[0].ToString());
                        basketFilter.BenchmarkID = int.Parse(row[1].ToString());
                        basketFilter.OperatorID = int.Parse(row[2].ToString());
                        basketFilter.Percentage = float.Parse(row[3].ToString());
                        _basketFilterCollection.Add(basketFilter);
                        //filterTypeName = row[0].ToString();

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

            return _basketFilterCollection;
        }

        /// <summary>
        /// Returns Filter from List of Basket Filter based on filtertypeID
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="filterTypeID"></param>
        /// <returns></returns>
        public static BasketFilter GetFilter(BasketFilterCollection filters,int  filterTypeID)
        {
            BasketFilter filter = null;
            foreach (BasketFilter filter1 in filters)
            {
               
                if (filter1.FilterTypeID == filterTypeID )
                {
                    filter = filter1;
                    break;
                }
            }
            return filter;
        }

    }
}
