using Prana.LogManager;
using System;
using System.Data;

namespace Prana.Preferences
{
    public static class GeneralDatabaseManager
    {
        public static GeneralPreferenceData GetPMPrefDataFromDB(int userID)
        {
            GeneralPreferenceData PrefereneceData = new GeneralPreferenceData();
            object[] parameter = new object[1];
            parameter[0] = userID;

            try
            {
                using (IDataReader reader = DatabaseManager.DatabaseManager.ExecuteReader("P_GetGeneralPreferences", new object[] { userID }))
                {
                    while (reader.Read())
                    {
                        object[] row = new object[reader.FieldCount];
                        reader.GetValues(row);
                        PrefereneceData.IsShowServiceIcons = Convert.ToBoolean(row[0].ToString());
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

        internal static void SavePMPrefDatainDB(int UserId, bool isShowServiceIcons)
        {
            object[] parameters = new object[2];

            parameters[0] = UserId;
            parameters[1] = isShowServiceIcons;

            try
            {
                DatabaseManager.DatabaseManager.ExecuteNonQuery("P_SaveGeneralPreferences", parameters).ToString();

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

    }
}
