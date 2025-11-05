using System;

namespace Prana.Interfaces
{
    public interface IMarkPriceUI
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosed;
        Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }
        IPricingAnalysis PricingAnalysis { set; }
    }
}
