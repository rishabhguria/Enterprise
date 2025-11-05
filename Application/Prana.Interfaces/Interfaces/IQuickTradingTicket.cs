using Prana.BusinessObjects;
using Prana.Global;
using System;

namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for Quick Trading Ticket module.
    /// </summary>
    public interface IQuickTradingTicket
    {
        System.Windows.Forms.Form Reference();
        CompanyUser LoginUser
        {
            get;
            set;
        }

        event EventHandler LaunchSymbolLookup;

        event EventHandler<QTTBlotterLinkingData> HighlightSymbolOnBlotter;

        event EventHandler<QTTBlotterLinkingData> DeHighlightSymbolOnBlotter;

        event EventHandler<EventArgs<string>> SendInstanceName;

        ISecurityMasterServices SecurityMaster
        {
            set;
        }

        event EventHandler FormClosedHandler;
        int QTTIndex { get; set; }
    }
}
