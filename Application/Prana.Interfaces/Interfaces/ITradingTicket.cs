using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;

namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for Trading Ticket module.
    /// </summary>
    public interface ITradingTicket
    {
        System.Windows.Forms.Form Reference();
        void SetTradingTicketFromNirvanaMain(Prana.BusinessObjects.OrderSingle or, int allocationPrefID = 0, Dictionary<int, double> accountWithPostions = null);
        void TradingFormSetting(string symbol, string watchlistColumn);
        //void SetPreferences();
        //void ApplyPreferences(string moduleName);
        Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }
        //ICommunicationManager SetCommunicationManager { set;}  
        //event EventHandler LaunchPreferences;
        event EventHandler LaunchSymbolLookup;
        ISecurityMasterServices SecurityMaster
        {
            set;
        }

        // Added this to reset TT to its default value when it is opened from any other module
        void ResetTicket();

        event EventHandler FormClosedHandler;
        TradingTicketParent TradingTicketParent { get; set; }
    }
}
