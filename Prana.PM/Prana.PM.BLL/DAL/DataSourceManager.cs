using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.BLL;
//
using Prana.Utilities.XMLUtilities;
using System;
using System.Data;
namespace Prana.PM.DAL
{
    public class DataSourceManager
    {
        private static int _errorNumber = 0;

        private static string _errorMessage = string.Empty;

        public static SortableSearchableList<Prana.BusinessObjects.PositionManagement.ThirdPartyNameID> RetrieveDataSourceNames(bool selectItemRequired, bool isAllDataSourceAvailable)
        {
            SortableSearchableList<Prana.BusinessObjects.PositionManagement.ThirdPartyNameID> dataSourceNameIDList = GetAllDataSourceNames();
            if (selectItemRequired)
            {
                dataSourceNameIDList.Insert(0, new Prana.BusinessObjects.PositionManagement.ThirdPartyNameID(-1, ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
            }
            if (isAllDataSourceAvailable)
            {
                dataSourceNameIDList.Insert(0, new Prana.BusinessObjects.PositionManagement.ThirdPartyNameID(0, ApplicationConstants.C_COMBO_ALL));
            }

            return dataSourceNameIDList;
        }

        /// <summary>
        /// Gets all data source names.
        /// </summary>
        /// <returns></returns>
        public static SortableSearchableList<Prana.BusinessObjects.PositionManagement.ThirdPartyNameID> GetAllDataSourceNames()
        {
            SortableSearchableList<Prana.BusinessObjects.PositionManagement.ThirdPartyNameID> dataSourceList = new SortableSearchableList<Prana.BusinessObjects.PositionManagement.ThirdPartyNameID>();

            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "PMGetAllDataSourceNames";

                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader(queryData))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        dataSourceList.Add(FillDataSourcesNameID(row, 0));

                    }
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

            return dataSourceList;
        }

        /// <summary>
        /// Fills the data sources.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        private static Prana.BusinessObjects.PositionManagement.ThirdPartyNameID FillDataSourcesNameID(object[] row, int offset)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            Prana.BusinessObjects.PositionManagement.ThirdPartyNameID thirdParty = null;

            if (row != null)
            {
                thirdParty = new Prana.BusinessObjects.PositionManagement.ThirdPartyNameID();
                int THIRDPARTYID = offset + 0;
                int THIRDPARTYNAME = offset + 1;
                int THIRDPARTYSHORTNAME = offset + 2;

                try
                {
                    thirdParty.ID = Convert.ToInt32(row[THIRDPARTYID].ToString());
                    thirdParty.FullName = Convert.ToString(row[THIRDPARTYNAME]);
                    thirdParty.ShortName = Convert.ToString(row[THIRDPARTYSHORTNAME]);
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

            return thirdParty;
        }
        #region DataSourceTables
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private static TableTypeList FillTableTypeList(DataSet ds)
        {
            TableTypeList tableTypeList = new TableTypeList();
            int TABLETYPEID = 0;
            int TABLETYPENAME = 1;
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                foreach (DataRow row in dt.Rows)
                {
                    TableType tableType = new TableType();


                    try
                    {
                        //tableType.TableTypeID = Convert.ToInt32(row["TableTypeID"]);
                        //tableType.TableTypeName = Convert.ToString(row["TableTypeName"]);
                        tableType.TableTypeID = Convert.ToInt32(row[TABLETYPEID].ToString());
                        tableType.TableTypeName = Convert.ToString(row[TABLETYPENAME].ToString());
                        tableTypeList.Add(tableType);
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
            return tableTypeList;
        }

        public static TableTypeList GetTableTypes(bool isSelectItemRequired)
        {
            TableTypeList tableTypeList = new TableTypeList();

            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetTableTypes";

            XMLSaveManager.AddOutErrorParameters(queryData);

            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

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
            tableTypeList = FillTableTypeList(ds);

            if (isSelectItemRequired)
            {
                tableTypeList.Insert(0, new TableType(-1, ApplicationConstants.C_COMBO_SELECT));
            }


            return tableTypeList;
        }
        private static DataSourceTable FillDataSourceTable(DataSet ds)
        {
            DataSourceTable dataSourceTable = new DataSourceTable();

            int thirdPartyID = 0;
            int tableName = 1;
            int tableTypeID = 2;

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                try
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!(row[thirdPartyID] is System.DBNull))
                        {
                            dataSourceTable.ThirdPartyID = int.Parse(row[thirdPartyID].ToString());
                        }
                        if (!(row[tableName] is System.DBNull))
                        {
                            dataSourceTable.TableName = row[tableName].ToString();
                        }
                        if (!(row[tableTypeID] is System.DBNull))
                        {
                            dataSourceTable.TableTypeID = int.Parse(row[tableTypeID].ToString());
                        }

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

            return dataSourceTable;
        }

        public static DataSourceTable GetDataSourceTable(int thirdPartyID, int tableTypeID)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "[PMGetDataSourceTableValue]";
            queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ThirdPartyID",
                ParameterType = DbType.Int32,
                ParameterValue = thirdPartyID
            });
            queryData.DictionaryDatabaseParameter.Add("@TableTypeID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@TableTypeID",
                ParameterType = DbType.Int32,
                ParameterValue = tableTypeID
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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


            return FillDataSourceTable(ds);

        }

        public static DataSourceTable GetDataSourceTable(int thirdPartyID)
        {
            DataSet ds = null;

            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "[PMGetDataSourceTable]";
            queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ThirdPartyID",
                ParameterType = DbType.Int32,
                ParameterValue = thirdPartyID
            });

            XMLSaveManager.AddOutErrorParameters(queryData);
            try
            {
                ds = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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


            return FillDataSourceTable(ds);

        }
        #endregion
    }
}