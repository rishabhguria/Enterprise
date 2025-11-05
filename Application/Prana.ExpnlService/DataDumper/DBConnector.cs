using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.Utilities.XMLUtilities;
using System;
using System.Configuration;
using System.Data;

namespace Prana.ExpnlService.DataDumper
{
    internal static class DBConnector
    {
        private const string CONST_ClientConnStringName = "PranaConnectionString";
        static object _isSaving = new object();

        /// <summary>
        /// Store data in T_PMDataDump after Remove old data
        /// </summary>
        /// <param name="dataToSave"></param>
        internal static void SaveRealTimeData(ExposurePnlCacheItemList dataToSave, DataTable dtIndicesReturn)
        {
            Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();
            String XmlString = _xml.WriteString(dataToSave);

            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtIndicesReturn.Copy());
                String XmlIndicesString = ds.GetXml();

                #region remove old reatimedata from T_PMDataDump

                bool isDeleteHistoricalData = true;
                isDeleteHistoricalData = Convert.ToBoolean(ConfigurationManager.AppSettings["RemoveOldData"]);
                int iHours = Convert.ToInt32(ConfigurationManager.AppSettings["Hours"]);
                if (isDeleteHistoricalData)
                {
                    if (iHours <= 0)
                    {
                        iHours = 0;
                    }

                    QueryData deleteQuery = new QueryData();
                    deleteQuery.StoredProcedureName = "P_WC_RemoveHistoricalData";
                    deleteQuery.CommandTimeout = 300;
                    deleteQuery.DictionaryDatabaseParameter.Add("@hrs", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@hrs",
                        ParameterType = DbType.Int32,
                        ParameterValue = iHours.ToString()
                    });

                    Prana.DatabaseManager.DatabaseManager.ExecuteNonQuery(deleteQuery, CONST_ClientConnStringName);
                }

                #endregion

                #region save realtime data in T_PMDataDump Table
                lock (_isSaving)
                {
                    string _errorMessage = string.Empty;
                    int _errorNumber = 0;
                    QueryData query = new QueryData();
                    query.StoredProcedureName = "P_WC_SaveRealtimeData";
                    query.CommandTimeout = 300;
                    query.DictionaryDatabaseParameter.Add("@Xml", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@Xml",
                        ParameterType = DbType.String,
                        ParameterValue = XmlString
                    });
                    query.DictionaryDatabaseParameter.Add("@XmlIndices", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@XmlIndices",
                        ParameterType = DbType.String,
                        ParameterValue = XmlIndicesString
                    });

                    XMLSaveManager.AddOutErrorParameters(query);
                    Prana.DatabaseManager.DatabaseManager.ExecuteNonQuery(query, CONST_ClientConnStringName);
                    XMLSaveManager.GetErrorParameterValues(ref _errorMessage, ref _errorNumber, query.DictionaryDatabaseParameter);
                }
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
        }
    }
}
