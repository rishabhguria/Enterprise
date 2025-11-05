using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessLogic
{
    public class XMLSaveManager
    {
        private static int _errorNumber = 0;

        private static string _errorMessage = string.Empty;
        private static string _oldValues = string.Empty;

        public static string OldValues
        {
            get
            {
                return _oldValues;
            }
        }

        /// <summary>
        /// Saves the through XML.
        /// </summary>
        /// <param name="spName">Name of the sp. This method assumes that Xml parameter name is @Xml</param>
        /// <param name="xmlDoc">The XML doc.</param>
        /// <returns></returns>
        //internal static int SaveThroughXML(string spName, string xmlDoc)
        public static int SaveThroughXML(string spName, string xmlDoc)
        {
            int rowsAffected = 0;
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = spName;
                queryData.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@Xml",
                    ParameterType = DbType.String,
                    ParameterValue = xmlDoc
                });

                AddOutErrorParameters(queryData);
                if (spName.Equals("[PMSaveForexRate_Import]"))
                {
                    queryData.DictionaryDatabaseParameter.Add("@OldValues", new DatabaseParameter()
                    {
                        IsOutParameter = true,
                        ParameterName = "@OldValues",
                        ParameterType = DbType.Xml,
                        OutParameterSize = -1
                    });
                }

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);
                GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
                if (spName.Equals("[PMSaveForexRate_Import]"))
                {
                    _oldValues = Convert.ToString(queryData.DictionaryDatabaseParameter["@OldValues"].ParameterValue);
                }

                xmlDoc = null;
            }
            catch (Exception ex)
            {
                //done changes to get error returned from failure of query run in database and log into server log, PRANA-3596
                Logger.HandleException(new Exception("Error Message=" + _errorMessage + "Error in Saving Xml:=" + xmlDoc), LoggingConstants.POLICY_LOGANDSHOW);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return rowsAffected;
        }

        public static int SaveThroughXML(string spName, string xmlDoc, string connString)
        {
            int rowsAffected = 0;

            try
            {
                #region dbfactory

                object[] parameter = new object[3];

                parameter[0] = xmlDoc;
                parameter[1] = "";
                parameter[2] = 0;
                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(spName, parameter, connString);
                #endregion
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
            return rowsAffected;
        }

        /// <summary>
        /// Sets the out parameter values.
        /// </summary>
        /// <param name="dbErrorMessage">The db message.</param>
        /// <param name="dbErrorNumber">The db error number.</param>
        /// <param name="parameters">Parameters of QueryData object.</param>
        public static void GetErrorParameterValues(ref string dbErrorMessage, ref int dbErrorNumber, Dictionary<string, DatabaseParameter> parameters)
        {
            try
            {
                dbErrorNumber = 0;
                dbErrorMessage = string.Empty;
                dbErrorNumber = Convert.ToInt32(parameters["@ErrorNumber"].ParameterValue);
                dbErrorMessage = Convert.ToString(parameters["@ErrorMessage"].ParameterValue);

                if (!int.Equals(dbErrorNumber, 0))
                {
                    dbErrorMessage = GetErrorMessage(dbErrorNumber, dbErrorMessage);

                    throw new Exception(dbErrorMessage);
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

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <param name="dbErrorNumber">The db error number.</param>
        /// <returns></returns>
        private static string GetErrorMessage(int dbErrorNumber, string dbErrorMessage)
        {
            string message = string.Empty;
            string actionFailed = "Your action failed.";
            switch (dbErrorNumber)
            {
                //Error raised in SQL using RAISEERROR(), is given error number 50000.
                // I raised it only at one place for catching if the datasource file contains an unmapped
                // accountname.
                case 50000:
                    message = "The File Contains AccountNames which are not mapped.So, the data in the file is incorrect and cannot be uploaded.";
                    break;
                //Cannot insert duplicate key row in object '%.*ls' with unique index '%.*ls'.
                case 2601:
                //Violation of %ls constraint '%.*ls'. Cannot insert duplicate key in object '%.*ls'.
                case 2627:
                //message = "Cannot make duplicate entry. " + actionFailed;
                //break;
                //The %ls statement conflicted with the %ls constraint "%.*ls". The conflict occurred in database "%.*ls", table "%.*ls"%ls%.*ls%ls.
                case 547:
                //message = "Foreign Key Constraint violation. " + actionFailed;
                //break;
                //Cannot insert the value NULL into column '%.*ls', table '%.*ls'; column does not allow nulls. %ls fails.
                case 515:
                //message = "Value for some mandatory field missing. " + actionFailed;
                //break;
                //Conversion failed when converting the %ls value '%.*ls' to data type %ls.
                case 245:
                //message = "Error in conversion of value. " + actionFailed;
                // break;
                //Arithmetic overflow error for data type %ls, value = %ld.
                case 220:
                //message = "Value of some integer over flown. " + actionFailed;
                //break;
                //Arithmetic overflow error converting %ls to data type %ls.
                case 8115:
                //message = "Arithmetic OverFlow Error in Data. " + actionFailed;
                //break;
                //Conversion failed when converting datetime from character string.
                case 241:
                //message = "Conversion failed when converting datetime from character string. Database Problem. " + actionFailed;
                // break;
                //Incorrect syntax near the keyword '%.*ls'.
                case 156:
                //message = "Incorrect syntax. " + actionFailed;
                //break;
                //The select list for the INSERT statement contains fewer items than the insert list. 
                //The number of SELECT values must match the number of INSERT columns.
                case 120:
                //The select list for the INSERT statement contains more items than the insert list. 
                //The number of SELECT values must match the number of INSERT columns.
                case 121:
                // Syntax Error
                case 102:
                //The conversion of a char data type to a datetime data type resulted in an out-of-range datetime value.
                case 242:
                // Invalid object name 
                case 208:
                //Column, parameter, or variable #1: Cannot find data type UniqueIndentifier.
                case 2715:
                //There are more columns in the INSERT statement than values specified in the VALUES clause. 
                //The number of values in the VALUES clause must match the number of columns specified in the INSERT statement.
                case 109:
                //There are fewer columns in the INSERT statement than values specified in the VALUES clause. 
                //The number of values in the VALUES clause must match the number of columns specified in the INSERT statement
                case 110:
                //Subquery returned more than 1 value. This is not permitted when the subquery follows 
                // =, !=, <, <= , >, >= or when the subquery is used as an expression.
                case 512:

                //case 8179 : 
                //    http://www.devnewsgroups.net/group/microsoft.public.dotnet.framework.adonet/topic3306.aspx
                //    http://www.google.co.in/search?hl=en&q=8179+sql+server+error+code&meta=

                case 8144:
                    message = dbErrorMessage;
                    break;
                default:
                    message = "DataBase Error. Please report it to Prana Development Team." + actionFailed + " Technical Details : " + dbErrorMessage;
                    break;
            }

            return message;
        }

        /// <summary>
        /// Adds the out parameters.
        /// </summary>
        /// <param name="queryData">QueryData object on which out parameters have to be added.</param>
        public static void AddOutErrorParameters(QueryData queryData)
        {
            queryData.DictionaryDatabaseParameter.Add("@ErrorMessage", new DatabaseParameter()
            {
                IsOutParameter = true,
                ParameterName = "@ErrorMessage",
                ParameterType = DbType.String,
                OutParameterSize = -1
            });

            queryData.DictionaryDatabaseParameter.Add("@ErrorNumber", new DatabaseParameter()
            {
                IsOutParameter = true,
                ParameterName = "@ErrorNumber",
                ParameterType = DbType.Int32,
                ParameterValue = 0,
                OutParameterSize = sizeof(Int32)
            });
        }
    }
}
