using Prana.BusinessObjects.AppConstants;
using System;

namespace Prana.Interfaces
{
    public interface IMultiTradingTicket
    {
        System.Windows.Forms.Form Reference();
        void SetMTTFromNirvanaMain(Prana.BusinessObjects.OrderBindingList orderList, bool isEdit);
        //void UpdateOrderListWithSMData(Prana.BusinessObjects.OrderBindingList orderList);
        //void SetPreferences();
        //void ApplyPreferences(string moduleName);
        Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }

        ISecurityMasterServices SecurityMaster
        {
            set;
        }
        TradingTicketParent TradingTicketParent { get; set; }
        event EventHandler FormClosedHandler;

        void ResetMultiTradingTicket(bool isFormLoadRequired);
    }
}
