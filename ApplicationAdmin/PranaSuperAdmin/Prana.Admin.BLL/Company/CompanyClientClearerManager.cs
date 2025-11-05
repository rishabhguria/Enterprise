using Prana.LogManager;
using System;
using System.Data;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyClientClearerManager.
    /// </summary>
    public class CompanyClientClearerManager
    {
        public CompanyClientClearerManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static int SaveCompanyClientClearer(CompanyClientClearer CompanyClientClearerDetalis)
        {
            //TODO: Write SP
            int result = int.MinValue;
            object[] parameter = new object[4];
            try
            {
                parameter[0] = CompanyClientClearerDetalis.CompanyClientID;
                parameter[1] = CompanyClientClearerDetalis.CompanyClientClearerName;
                parameter[2] = CompanyClientClearerDetalis.CompanyClientClearerShortName;
                parameter[3] = CompanyClientClearerDetalis.ClearingFirmBrokerID;
                result = int.Parse(DatabaseManager.DatabaseManager.ExecuteScalar("P_SaveCompanyClientClearer", parameter).ToString());
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
            return result;
        }


        public static CompanyClientClearer GetCompanyClientClearer(int intCompanyClientID)
        {
            CompanyClientClearer companyClientClearer = new CompanyClientClearer();
            Object[] parameter = new object[1];
            parameter[0] = intCompanyClientID;
            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyClientClearer", parameter))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        companyClientClearer = FillClientClearers(row, 0);
                    }
                }
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
            return companyClientClearer;

        }

        public static CompanyClientClearer FillClientClearers(object[] row, int offSet)
        {
            int CompanyClientID = 0 + offSet;
            int CompanyClientClearerName = 1 + offSet;
            int CompanyClientClearerShortName = 2 + offSet;
            int ClearingFirmBrokerID = 3 + offSet;


            CompanyClientClearer companyClientClearer = new CompanyClientClearer();
            if (row[CompanyClientID] != null)
            {
                companyClientClearer.CompanyClientID = int.Parse(row[CompanyClientID].ToString());
            }
            if (row[CompanyClientClearerName] != null)
            {
                companyClientClearer.CompanyClientClearerName = row[CompanyClientClearerName].ToString();
            }
            if (row[CompanyClientClearerShortName] != null)
            {
                companyClientClearer.CompanyClientClearerShortName = row[CompanyClientClearerShortName].ToString();
            }

            if (row[ClearingFirmBrokerID] != null)
            {
                companyClientClearer.ClearingFirmBrokerID = row[ClearingFirmBrokerID].ToString();

            }


            return companyClientClearer;
        }

    }
}
