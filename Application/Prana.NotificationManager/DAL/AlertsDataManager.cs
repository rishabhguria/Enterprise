using Prana.BusinessObjects.Compliance.Alerting;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.NotificationManager.DAL
{
    internal class AlertsDataManager
    {
        /// <summary>
        /// SAves alert in DB
        /// </summary>
        /// <param name="alert"></param>
        internal static void SaveAlertsInDB(Alert alert)
        {
            try
            {
                object[] parameters = new object[18];

                parameters[0] = alert.RuleId;
                // parameters[1] = dr["Name"];
                parameters[1] = alert.UserId;
                parameters[2] = alert.PackageName;
                parameters[3] = alert.Summary;
                parameters[4] = alert.CompressionLevel;
                parameters[5] = alert.Parameters;
                parameters[6] = alert.ValidationTime;
                parameters[7] = alert.OrderId;
                parameters[8] = alert.Status;
                parameters[9] = alert.Description;
                parameters[10] = alert.Dimension;
                parameters[11] = alert.PreTradeType;
                parameters[12] = alert.ActionUser;
                parameters[13] = alert.PreTradeActionType;
                parameters[14] = alert.ComplianceOfficerNotes;
                parameters[15] = alert.UserNotes;
                parameters[16] = alert.TradeDetails;
                parameters[17] = alert.AlertPopUpResponse;

                Prana.DatabaseManager.DatabaseManager.ExecuteNonQuery("P_CA_SaveAlertHistory", parameters);
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

        /// <summary>
        /// Gets the post trade one batch alert his.
        /// </summary>
        /// <returns></returns>
        internal static DataTable GetPostTradeScheduleAlertHis()
        {
            DataSet ds = new DataSet();
            try
            {
                QueryData queryData = new QueryData();
                queryData.StoredProcedureName = "P_CA_GetScheduleAlertHis";

                ds = Prana.DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
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
            return ds.Tables[0];
        }
    }
}
