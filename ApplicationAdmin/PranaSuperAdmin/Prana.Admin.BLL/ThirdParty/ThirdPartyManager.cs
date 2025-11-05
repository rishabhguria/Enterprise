using Prana.BusinessObjects;
using Prana.DatabaseManager;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Data;
using System.IO;
namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ThirdPartyManager.
    /// </summary>
    public class ThirdPartyManager
    {
        private ThirdPartyManager()
        {
        }


        #region ThirdPartyModules

        private static AUEC FillCompanyAUECDetails(object[] row, int offSet)
        {
            int AUECID = 0 + offSet;
            int AUECDISPLAYNAME = 1 + offSet;

            AUEC companyAUEC = new AUEC();
            try
            {
                if (!(row[AUECID] is System.DBNull))
                {
                    companyAUEC.AUECID = int.Parse(row[AUECID].ToString());
                }
                if (!(row[AUECDISPLAYNAME] is System.DBNull))
                {
                    companyAUEC.DisplayName = row[AUECDISPLAYNAME].ToString();
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
            return companyAUEC;
        }

        public static AUECs GetCompanyWorkingAUECs(int companyID)
        {
            AUECs companyAUECs = new AUECs();

            object[] parameter = new object[1];
            parameter[0] = companyID;

            using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("GetWorkingAUECForCompany", parameter))
            {
                while (reader.Read())
                {
                    object[] row = new object[reader.FieldCount];
                    reader.GetValues(row);
                    companyAUECs.Add(FillCompanyAUECDetails(row, 0));

                }
            }
            return companyAUECs;
        }
        #endregion
    }
}
