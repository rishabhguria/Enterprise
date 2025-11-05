
#region Using namespaces
using Prana.LogManager;
using System;
using System.Data;
#endregion


namespace Prana.ClientCommon
{
    /// <summary>
    /// Loading and Saving Layout Settings from and to the database.
    /// </summary>
    public class LayoutManager
    {
        public LayoutManager()
        {
        }
        /// <summary>
        /// Save Layout Settings to the Db
        /// </summary>
        /// <param name="dt">The datatable which contains Layout Settings</param>
        public static int SaveLayout(DataTable dt, int LayoutID)
        {
            int result = int.MinValue;
            try
            {
                object[] para = new object[1];

                para[0] = LayoutID;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_DeleteOldLayoutDetails", para);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] parameter = new object[10];
                    parameter[0] = int.Parse(dt.Rows[i]["LeftX"].ToString());
                    parameter[1] = int.Parse(dt.Rows[i]["RightY"].ToString());
                    parameter[2] = int.Parse(dt.Rows[i]["Height"].ToString());
                    parameter[3] = int.Parse(dt.Rows[i]["Width"].ToString());
                    parameter[4] = dt.Rows[i]["WindowState"];
                    parameter[5] = int.Parse(dt.Rows[i]["IsInUse"].ToString());
                    parameter[6] = int.Parse(dt.Rows[i]["LayoutID"].ToString());
                    parameter[7] = int.Parse(dt.Rows[i]["ModuleID"].ToString());
                    parameter[8] = int.Parse(dt.Rows[i]["UserID"].ToString());
                    if (!dt.Rows[i]["QTTIndex"].Equals(DBNull.Value))
                        parameter[9] = int.Parse(dt.Rows[i]["QTTIndex"].ToString());

                    result = DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveLayout", parameter);
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
            return result;
        }

        /// <summary>
        /// Get module ID when the module Name is known
        /// </summary>
        /// <param name="moduleName">The module whose ID is to be fetched from the db</param>
        /// <returns>the module ID</returns>
        public static int GetModuleID(string moduleName)
        {
            int _moduleID = int.MinValue;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = moduleName;

                _moduleID = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_GetComponentIDByName", parameter).ToString());

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


            return _moduleID;

        }
        /// <summary>
        /// To fetch the Layout Details corresponding to a particular Layout ID
        /// </summary>
        /// <param name="dt">The datatable which will contain the layout settings</param>
        /// <param name="LayoutID">the id corresponding to which the details are to be fetched</param>
        /// <returns>the datatable which contains the settings</returns>
        public static DataTable GetLayoutDetails(int LayoutID, int userID)
        {
            DataSet ds = null;
            try
            {
                object[] parameter = new object[2];
                parameter[0] = LayoutID;
                parameter[1] = userID;
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetLayoutDetailsByID", parameter);
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

            return ds.Tables[0];
        }

        /// <summary>
        /// To fetch all the Layouts corresponding to a particular user
        /// </summary>
        /// <param name="dtLayout">The datatable which will contain the layouts</param>
        /// <param name="userID">the id corresponding to which the layouts are to be fetched</param>
        /// <returns>the datatable which contains the layouts</returns>
        public static DataTable GetAllLayoutsForUser(int userID)
        {
            DataSet ds = null;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = userID;
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetLayoutsForUser", parameter);
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

            return ds.Tables[0];
        }

        /// <summary>
        /// Save the Layout Name and set a unique ID corresponding to it
        /// </summary>
        /// <param name="str">Layout File Name</param>
        /// <returns>Layout ID corresponding to which the file name has been saved</returns>
        public static int SaveLayoutName(string str, int userID)
        {
            int _layoutID = int.MinValue;
            try
            {
                object[] para = new object[2];
                para[0] = str;
                para[1] = userID;
                _layoutID = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveLayoutName", para).ToString());
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


            return _layoutID;

        }
        /// <summary>
        /// Save the Layout Name and set a unique ID corresponding to it
        /// </summary>
        /// <param name="str">Layout File Name</param>
        /// <returns>Layout ID corresponding to which the file name has been saved</returns>
        public static void SaveLastUsedTime(int LayoutID)
        {
            try
            {
                object[] parameters = new object[1];
                parameters[0] = LayoutID;
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveLastUsedTime", parameters);
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

        //SP_OBSOLETE: GetLayoutID
        /// <summary>
        /// Get the Layout ID corresponding to the name of the Layout
        /// </summary>
        /// <param name="str">The name of the Layout</param>
        /// <returns>Layout ID corresponding to the Layout Name</returns>
        public static int GetLayoutID(string str)
        {
            int _layoutID = int.MinValue;
            try
            {
                object[] para = new object[1];
                para[0] = str;
                _layoutID = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_GetLayoutID", para).ToString());
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


            return _layoutID;


        }

        public static DataTable GetUserMainFormLayout(int userID)
        {
            DataSet ds = null;
            try
            {
                object[] parameter = new object[1];
                parameter[0] = userID;
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetMainFormLayoutForUser", parameter);
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
            return ds.Tables[0];
        }

        public static DataTable GetModuleDetailsForLayout(int layoutID, string moduleName)
        {
            DataSet ds = null;
            try
            {
                object[] parameter = new object[2];
                parameter[0] = layoutID;
                parameter[1] = moduleName;
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetModuleDetailsForLayout", parameter);
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

            return ds.Tables[0];
        }

        /// <summary>
        /// Gets the layout details by module identifier.
        /// </summary>
        /// <param name="LayoutID">The layout identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="moduleId">The module identifier.</param>
        /// <returns></returns>
        public static DataTable GetLayoutDetailsByModuleId(int LayoutID, int userID, int moduleId)
        {
            DataTable dataTable = null;
            try
            {
                object[] parameter = new object[3];
                parameter[0] = LayoutID;
                parameter[1] = userID;
                parameter[2] = moduleId;
                dataTable = DatabaseManager.DatabaseManager.ExecuteDataSet("P_GetLayoutDetailsByModuleID", parameter).Tables[0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return dataTable;
        }
    }
}
