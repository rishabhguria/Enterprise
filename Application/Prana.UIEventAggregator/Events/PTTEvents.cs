using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using System.Collections.Generic;

namespace Prana.UIEventAggregator.Events
{

    /// <summary>
    /// Sends close PTT signal to Nirvana Main on close click
    /// </summary>
    public class ClosePTTUI
    {
    }

    /// <summary>
    /// Sends Expnl service status from expnl service connector to view model
    /// </summary>
    public class PTTExpnlStatus
    {
        public bool IsExpnlServiceConnected { get; set; }
    }
}