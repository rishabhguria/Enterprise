using Prana.BusinessLogic;
using Prana.DatabaseManager;
using Prana.LogManager;
using Prana.PM.BLL;
using Prana.Utilities.XMLUtilities;
using System;
using System.Data;
namespace Prana.PM.DAL
{
    public class TradeReconManager
    {
        #region GetAppReconciliedColumnList

        public static Prana.PM.BLL.AppReconciliedColumnList GetAppReconciliedColumnList(int thirdPartyID)
        {
            AppReconciliedColumnList appReconciliedColumnList = null;
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            QueryData queryData = new QueryData();
            queryData.StoredProcedureName = "PMGetAppReconciliedColumnsByID";
            queryData.DictionaryDatabaseParameter.Add("@ThirdPartyID", new DatabaseParameter()
            {
                IsOutParameter = false,
                ParameterName = "@ThirdPartyID",
                ParameterType = DbType.Int32,
                ParameterValue = thirdPartyID
            });
            queryData.DictionaryDatabaseParameter.Add("@ErrorMessage", new DatabaseParameter()
            {
                IsOutParameter = true,
                ParameterName = "@ErrorMessage",
                ParameterType = DbType.String,
                OutParameterSize = -1
            });

            // DataSet that will hold the returned results		
            DataSet dsAppReconciliedColumnList = null;
            try
            {

                dsAppReconciliedColumnList = DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);

                // Note: connection was closed by ExecuteDataSet method call 

                appReconciliedColumnList = FillAppReconciliedColumnList(dsAppReconciliedColumnList);

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

            return appReconciliedColumnList;
        }

        public static int SaveSetupTradeRecon(DataSourceReconColumnsInfo reconColumnsInfo)
        {
            int rowsAffected = 0;
            try
            {

                string xml = XMLUtilities.SerializeToXML(reconColumnsInfo);

                rowsAffected = XMLSaveManager.SaveThroughXML("PMAddUpdateSetupTradeRecon", xml);

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

        private static Prana.PM.BLL.AppReconciliedColumnList FillAppReconciliedColumnList(System.Data.DataSet ds)
        {
            Prana.PM.BLL.AppReconciliedColumnList appReconciliedColumnList = new Prana.PM.BLL.AppReconciliedColumnList();

            int dataSourceColumnID = 0;
            int name = 1; // +offSet;
            int description = 2; // +offSet;
            int includeAsCash = 3; // +offSet;
            int type = 4; // +offSet;
            int acceptableDeviation = 5; // +offSet;
            int deviationSign = 6;
            int companyReconColumnID = 7;
            int acceptDataFrom = 8;



            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                Prana.PM.BLL.AppReconciliedColumn appReconciliedColumn = null;

                try
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        appReconciliedColumn = new Prana.PM.BLL.AppReconciliedColumn();
                        if (!(row[dataSourceColumnID] is System.DBNull))
                        {
                            appReconciliedColumn.DataSourceColumnID = Convert.ToInt32(row[dataSourceColumnID]);
                        }
                        if (!(row[name] is System.DBNull))
                        {
                            appReconciliedColumn.AppReconciliedColumnName = row[name].ToString();
                        }
                        if (!(row[description] is System.DBNull))
                        {
                            appReconciliedColumn.Description = row[description].ToString();
                        }
                        if (!(row[includeAsCash] is System.DBNull))
                        {
                            appReconciliedColumn.IsIncludedAsCash = bool.Parse(row[includeAsCash].ToString());
                        }
                        if (!(row[type] is System.DBNull))
                        {
                            appReconciliedColumn.Type = (Prana.PM.BLL.EntryType)Convert.ToInt32(row[type]);
                        }
                        if (!(row[acceptableDeviation] is System.DBNull))
                        {
                            appReconciliedColumn.AcceptableDeviation = int.Parse(row[acceptableDeviation].ToString());
                        }

                        if (!(row[deviationSign] is System.DBNull))
                        {
                            appReconciliedColumn.DeviationSign = (Prana.PM.BLL.DeviationSignList)int.Parse(row[deviationSign].ToString());
                        }
                        if (!(row[companyReconColumnID] is System.DBNull))
                        {
                            appReconciliedColumn.ID = Convert.ToInt32(row[companyReconColumnID]);
                        }

                        if (!(row[acceptDataFrom] is System.DBNull))
                        {
                            appReconciliedColumn.AcceptDataFrom = (Prana.PM.BLL.AcceptDataFrom)Convert.ToInt16(row[acceptDataFrom]);
                        }
                        if (row != null)
                        {
                            appReconciliedColumnList.Add(appReconciliedColumn);
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



            return appReconciliedColumnList;
        }
        #endregion
    }
}
