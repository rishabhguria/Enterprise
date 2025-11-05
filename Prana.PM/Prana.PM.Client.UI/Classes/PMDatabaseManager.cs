using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.PM.Client.UI
{
    public static class PMDatabaseManager
    {
        internal static PMPreferenceData GetPMPrefDataFromDB(int userID)
        {
            PMPreferenceData PrefereneceData = new PMPreferenceData();
            object[] parameter = new object[1];
            parameter[0] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetPMPreferences", new object[] { userID }))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        PrefereneceData.UseClosingMark = bool.Parse(row[0].ToString());
                        PrefereneceData.XPercentofAvgVolume = double.Parse(row[1].ToString());
                        PrefereneceData.IsShowPMToolbar = Convert.ToBoolean(row[2].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return PrefereneceData;
        }

        internal static void SavePMPrefDatainDB(int UserId, bool useClosingMark, double XPercentofAvgVolume, bool PMToolStatus)
        {
            object[] parameters = new object[4];

            parameters[0] = UserId;
            parameters[1] = useClosingMark;
            parameters[2] = XPercentofAvgVolume;
            parameters[3] = PMToolStatus;
            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SavePMPreferences", parameters).ToString();

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        internal static PMUIPrefs GetCompanyPMPreferences(int companyID)
        {
            PMUIPrefs uiPrefs = new PMUIPrefs();

            try
            {

                object[] parameter = new object[1];
                parameter[0] = companyID;
                try
                {
                    using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetCompanyPMUIPrefsNew", parameter))
                    {
                        while (reader.Read())
                        {
                            object[] row = new object[reader.FieldCount];
                            reader.GetValues(row);
                            if (row[0] != DBNull.Value)
                            {
                                uiPrefs.NumberOfCustomViewsAllowed = Convert.ToInt32(row[0].ToString());
                            }
                            if (row[1] != DBNull.Value)
                            {
                                uiPrefs.NumberOfVisibleColumnsAllowed = Convert.ToInt32(row[1].ToString());
                            }
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
            return uiPrefs;
        }
    }
}
