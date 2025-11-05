using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.Collections.Generic;

namespace Prana.Interfaces
{
    public interface IPositionManagement
    {
        event EventHandler FormClosedHandler;

        event EventHandler<EventArgs<OrderSingle, Dictionary<int, double>>> TradeClick;

        event EventHandler PricingInputClick;



        event EventHandler ClosePositionClick;

        event EventHandler MarkPriceClick;

        event EventHandler CorporateActionClick;

        event EventHandler<EventArgs<string, PTTMasterFundOrAccount, List<int>, string>> PercentTradingToolClick;

        System.Windows.Forms.Form Reference();

        void InitializePM();
        void RequestAccountData();

        ICommunicationManager CommunicationManagerInstance { get; set; }
        ICommunicationManager ExPNLCommMgrInstance { get; set; }

        CompanyUser LoginUser
        {
            get;
            set;
        }

        string StatusMessage
        { set; }

        void ApplyPreferences(object sender, EventArgs<string, IPreferenceData> e);

        void HighlightSymbol(string symbol);
    }
}
