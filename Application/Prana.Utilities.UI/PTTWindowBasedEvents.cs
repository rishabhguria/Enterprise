using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using System.Collections.Generic;
using System.Windows;

namespace Prana.Utilities.UI
{
    /// <summary>
    /// Sends information on trade clicked in PTT
    /// </summary>
    public class PTTTradeClicked
    {
        public OrderSingle Order { get; set; }
        public Window ParentWindow { get; set; }
    }

    /// <summary>
    /// Sends information for compliance alerts to Nirvana Main from PTT
    /// </summary>
    public class PTTComplianceAlerts
    {
        public List<Alert> Alerts { get; set; }
        public Window ParentWindow { get; set; }
    }

    /// <summary>
    /// Sends symbol information from PTT to NirvanaMain in order to open Symbol Lookup
    /// </summary>
    public class PTTSymbolLookUpClicked
    {
        public ListEventAargs Args { get; set; }
        public Window ParentWindow { get; set; }
    }

    public class PTTPreferenceClicked
    {
        public Window ParentWindow { get; set; }
    }

    public class OpenMultiTTFromPTT
    {
        public OrderBindingList OrderList { get; set; }
        public Window ParentWindow { get; set; }
    }
}
