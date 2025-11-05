using Microsoft.Practices.EnterpriseLibrary.Data;
using Prana.Logging;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.NotificationManager.DAL
{
    internal class SaveAlertsInDb
    {
        /// <summary>
        /// SAves alert in DB
        /// </summary>
        /// <param name="alert"></param>
        internal static void SaveAlertsInDB(Alert alert)
        {
            try
            {
                String procedureName = "P_CA_SaveAlertHistory";
                Database db = DatabaseFactory.CreateDatabase();
                object[] parameters = new object[14];

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
                db.ExecuteNonQuery(procedureName, parameters);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionLogger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }        
    }
}
