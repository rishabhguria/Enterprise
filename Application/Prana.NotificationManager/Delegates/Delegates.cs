using Prana.BusinessObjects.Compliance.Alerting;
using System.Data;

namespace Prana.NotificationManager.Delegates
{
    /// <summary>
    /// Deletgate for alerting alert and rule save settings.
    /// </summary>
    /// <param name="alert"></param>
    public delegate void AlertDelegate(Alert alert,bool isCancel,string replaceAlertType);
    public delegate void RuleSavedHandler(DataSet data);
    public delegate void PreTradeApprovalInfoEvent(DataSet preTradeApprovalInfoEvent);
}
