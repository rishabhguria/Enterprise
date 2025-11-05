using System;

namespace Prana.Interfaces
{
    public interface IPluggableTools
    {
        void SetUP();
        System.Windows.Forms.Form Reference();
        event EventHandler PluggableToolsClosed;
        ISecurityMasterServices SecurityMaster
        {
            set;
        }
        IPostTradeServices PostTradeServices
        {
            set;
        }
        IPricingAnalysis PricingAnalysis
        {
            set;
        }
    }
}
