using Prana.BusinessObjects;
using Prana.Global;
using System;

namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for Blotter module.
    /// </summary>
    public interface IBlotter
    {
        event EventHandler BlotterClosed;
        event EventHandler LaunchPreferences;
        event EventHandler LaunchSecurityMasterForm;
        System.Windows.Forms.Form Reference();
        event EventHandler TradeClick;
        event EventHandler ReplaceOrEditOrderClicked;
        event EventHandler<EventArgs<string>> HighlightSymbolFromBlotter;
        void InitControl();
        void WireEvents();
        Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }

        event EventHandler<EventArgs<string, DateTime, DateTime>> GoToAllocationClicked;

        ISecurityMasterServices SecurityMaster
        {
            set;
        }

        void HighlightSymbol(string symbol);
        void HighlightSymbolFromQTT(QTTBlotterLinkingData linkingData);
        void DeHighlightSymbolFromQTT(QTTBlotterLinkingData linkingData);
    }
}