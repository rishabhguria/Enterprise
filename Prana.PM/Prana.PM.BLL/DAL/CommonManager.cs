using Prana.BusinessLogic;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Data;

namespace Prana.PM.DAL
{
    /// <summary>
    /// Common Manager will contain all those reusable functions
    /// </summary>
    public class CommonManager
    {
        private static int _errorNumber = 0;

        private static string _errorMessage = string.Empty;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal static int SaveThroughXML(string spName, string xmlDoc, int thirdPartyID)
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
                queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ThirdPartyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = thirdPartyID
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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
        /// Saves the through XML.
        /// </summary>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="xmlDoc">The XML doc.[ Assumed parameter name : @Xml ]</param>
        /// <param name="thirdPartyID">The third party ID. [ Assumed parameter name : @thirdPartyID ]</param>
        /// <param name="companyID">The company ID. [ Assumed parameter name : @CompanyID ]</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        internal static int SaveThroughXML(string spName, string xmlDoc, int thirdPartyID, int companyID)
        {
            int rowsAffected = 0;
            //string dbErrorMessage = string.Empty;
            //int dbErrorNumber = 0;
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
                queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@ThirdPartyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = thirdPartyID
                });
                queryData.DictionaryDatabaseParameter.Add("@CompanyID", new DatabaseParameter()
                {
                    IsOutParameter = false,
                    ParameterName = "@CompanyID",
                    ParameterType = DbType.Int32,
                    ParameterValue = companyID
                });

                XMLSaveManager.AddOutErrorParameters(queryData);

                rowsAffected = DatabaseManager.DatabaseManager.ExecuteNonQuery(queryData);

                XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, queryData.DictionaryDatabaseParameter);
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
    }
}
