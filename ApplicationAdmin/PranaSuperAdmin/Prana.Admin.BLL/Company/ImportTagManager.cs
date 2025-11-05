using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Admin.BLL
{
    public class ImportTagManager
    {
        public const string colImportTagName = "ImportTagName";
        public const string colAcronym = "Acronym";

        public static bool SaveImportTagData(DataTable dt)
        {
            try
            {
                String importTagXML = CreateReleaseXML(dt.Copy());
                ImportTagDAL.SaveImportTagData(importTagXML);
                return true;
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
            return false;
        }
        /// <summary>
        /// Create the XML representation of the DataTable holding the details
        /// </summary>
        /// <param name="dsPricing">Dataset holding the details </param>
        /// <returns>XML representation of the data</returns>
        public static string CreateReleaseXML(DataTable dt)
        {
            string releaseXML = null;
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                releaseXML = ds.GetXml();
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
        /// <summary>       
        /// Create the Dataset of the client details
        /// </summary>
        /// <returns>Dataset holding the client details</returns>
        public static DataTable GetImportTagDataTableFromDB()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = ImportTagDAL.GetImportTagDataTableFromDB();
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
            return dt;
        }
    }
}
