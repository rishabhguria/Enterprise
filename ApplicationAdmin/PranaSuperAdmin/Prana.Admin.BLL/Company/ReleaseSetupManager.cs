using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Admin.BLL
{
    public class ReleaseSetupManager
    {
        /// <summary>
        /// Create the Dataset of the client details
        /// </summary>
        /// <returns>Dataset holding the client details</returns>
        public static DataSet GetClientFromDB()
        {
            DataSet dsClient = new DataSet("dsClient");
            DataTable dtClient = new DataTable("dtClient");
            dtClient.Columns.Add("CompanyID", typeof(int));
            dtClient.Columns.Add("CompanyName", typeof(string));

            try
            {
                Dictionary<int, string> dicClients = ReleaseSetupDAL.GetClientsFromDB();
                foreach (int clientID in dicClients.Keys)
                {
                    dtClient.Rows.Add(clientID, dicClients[clientID]);
                }
                dsClient.Tables.Add(dtClient);
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
            return dsClient;
        }

        /// <summary>
        /// Create the datasource for the ultracombo 
        /// </summary>
        /// <returns>The Datatable of accounts</returns>
        public static DataTable GetAccounts(DataSet dsClient)
        {
            DataTable dtAccounts = new DataTable();
            dtAccounts.Columns.Add("FundID", typeof(int));
            dtAccounts.Columns.Add("FundName", typeof(string));
            String xmlClient = dsClient.GetXml();

            try
            {
                Dictionary<int, string> dicAccount = ReleaseSetupDAL.GetAccountsFromDB(xmlClient);
                foreach (int accountID in dicAccount.Keys)
                {
                    dtAccounts.Rows.Add(accountID, dicAccount[accountID]);
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
            return dtAccounts;
        }

        /// <summary>
        /// Create the datasource for the Ultragrid 
        /// </summary>
        /// <returns>Datatable holding the data</returns>
        public static DataTable GetReleaseDetails()
        {
            DataTable dtReleaseDetail = new DataTable();
            dtReleaseDetail.Columns.Add("ReleaseID", typeof(int));
            dtReleaseDetail.Columns.Add("ReleaseName", typeof(string));
            dtReleaseDetail.Columns.Add("Company", typeof(object));
            dtReleaseDetail.Columns.Add("Account", typeof(object));
            dtReleaseDetail.Columns.Add("IP", typeof(string));
            dtReleaseDetail.Columns.Add("ReleasePath", typeof(string));
            dtReleaseDetail.Columns.Add("ClientDB_Name", typeof(string));
            dtReleaseDetail.Columns.Add("SMDB_Name", typeof(string));
            dtReleaseDetail.Columns.Add("InUse", typeof(int));

            try
            {
                Dictionary<int, ReleaseDetails> dicRelease = ReleaseSetupDAL.GetReleaseDetailsFromDB();
                foreach (int releaseID in dicRelease.Keys)
                {
                    dtReleaseDetail.Rows.Add(releaseID, dicRelease[releaseID].ReleaseName, dicRelease[releaseID].clientID, dicRelease[releaseID].accountID,
                                            dicRelease[releaseID].IP, dicRelease[releaseID].ReleasePath, dicRelease[releaseID].ClientDB_Name, dicRelease[releaseID].SMDB_Name, dicRelease[releaseID].InUse);
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
            return dtReleaseDetail;
        }

        /// <summary>
        /// Save the release details to the database
        /// </summary>
        /// <param name="releaseDetailsList">List holding the lists of releases</param>
        /// <returns></returns>
        public static bool SaveReleaseData(List<List<string>> releaseDetailsList)
        {
            try
            {
                DataSet dsRelease = CreateReleaseDataSet(releaseDetailsList);
                String releaseXML = CreateReleaseXML(dsRelease);
                ReleaseSetupDAL.SaveReleaseDetailsInDB(releaseXML);
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
            return true;
        }

        /// <summary>
        /// Create the dataset of the Release details
        /// </summary>
        /// <param name="releaseList"></param>
        /// <returns>The dataset holding the Release details</returns>
        public static DataSet CreateReleaseDataSet(List<List<string>> releaseList)
        {
            DataSet dsReleaseDetail = new DataSet("dsReleaseDetail");
            DataTable dtReleaseDetail = new DataTable("dtReleaseDetail");
            dtReleaseDetail.Columns.Add("ReleaseID", typeof(int));
            dtReleaseDetail.Columns.Add("ReleaseName", typeof(string));
            dtReleaseDetail.Columns.Add("CompanyID", typeof(object));
            dtReleaseDetail.Columns.Add("FundID", typeof(object));
            dtReleaseDetail.Columns.Add("IP", typeof(string));
            dtReleaseDetail.Columns.Add("ReleasePath", typeof(string));
            dtReleaseDetail.Columns.Add("ClientDB_Name", typeof(string));
            dtReleaseDetail.Columns.Add("SMDB_Name", typeof(string));
            try
            {
                foreach (List<string> release in releaseList)
                {
                    dtReleaseDetail.Rows.Add(release[0], release[1], release[2],
                        release[3], release[4], release[5], release[6], release[7]);
                }
                dsReleaseDetail.Tables.Add(dtReleaseDetail);
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
            return dsReleaseDetail;
        }

        /// <summary>
        /// Create the XML representation of the dataset holding the details
        /// </summary>
        /// <param name="dsPricing">Dataset holding the details </param>
        /// <returns>XML representation of the data</returns>
        public static string CreateReleaseXML(DataSet dsRelease)
        {
            string releaseXML = null;
            try
            {
                releaseXML = dsRelease.GetXml();
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
            return releaseXML;
        }
    }
}
