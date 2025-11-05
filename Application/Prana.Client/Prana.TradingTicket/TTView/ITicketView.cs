using Prana.AlgoStrategyControls;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Prana.TradingTicket.TTView
{
    public interface ITicketView
    {

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        string Account { get; }

        /// <summary>
        /// Gets the account Text.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        string AccountText { get; }

        /// <summary>
        /// Gets or sets the settlement currency.
        /// </summary>
        /// <value>
        /// The settlement currency.
        /// </value>
        int SettlementCurrency { get; }

        /// <summary>
        /// Gets the algo strategy control property.
        /// </summary>
        /// <value>
        /// The algo strategy control property.
        /// </value>
        AlgoStrategyControl AlgoStrategyControlProperty { get; }

        /// <summary>
        /// Gets the broker.
        /// </summary>
        /// <value>
        /// The broker.
        /// </value>
        string Broker { get; }

        /// <summary>
        /// Gets or sets the brokerid.
        /// </summary>
        /// <value>
        /// The brokerid.
        /// </value>
        string Brokerid { get; }

        /// <summary>
        /// Gets or sets the broker notes.
        /// </summary>
        /// <value>
        /// The broker notes.
        /// </value>
        string BrokerNotes { get; }

        /// <summary>
        /// Gets or sets the commission basis.
        /// </summary>
        /// <value>
        /// The commission basis.
        /// </value>
        CalculationBasis CommissionBasis { get; }

        /// <summary>
        /// Gets or sets the commission rate.
        /// </summary>
        /// <value>
        /// The commission rate.
        /// </value>
        decimal CommissionRate { get; }

        string FxOperator { get; }

        /// <summary>
        /// Gets or sets the execution instructions.
        /// </summary>
        /// <value>
        /// The execution instructions.
        /// </value>
        string ExecutionInstructions { get; }

        /// <summary>
        /// Gets or sets the fx rate.
        /// </summary>
        /// <value>
        /// The fx rate.
        /// </value>
        decimal FxRate { get; }

        /// <summary>
        /// Gets or sets the handling instruction.
        /// </summary>
        /// <value>
        /// The handling instruction.
        /// </value>
        string HandlingInstruction { get; }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        /// <value>
        /// The limit.
        /// </value>
        decimal Limit { get; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        string Notes { get; }

        /// <summary>
        /// Gets or sets the order side.
        /// </summary>
        /// <value>
        /// The order side.
        /// </value>
        string OrderSide { get; }
        /// <summary>
        /// Gets or sets the type of the order.
        /// </summary>
        /// <value>
        /// The type of the order.
        /// </value>
        string OrderType { get; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        decimal Price { get; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        decimal Quantity { get; }

        /// <summary>
        /// Gets or sets the soft commission basis.
        /// </summary>
        /// <value>
        /// The soft commission basis.
        /// </value>
        CalculationBasis SoftCommissionBasis { get; }

        /// <summary>
        /// Gets or sets the tagValueDictionary
        /// </summary>
        /// <value>
        /// The tagValueDictionary
        /// </value>
        Dictionary<string, string> TagValueDictionary { get; }


        /// <summary>
        /// Gets or sets the soft commission rate.
        /// </summary>
        /// <value>
        /// The soft commission rate.
        /// </value>
        decimal SoftCommissionRate { get; }

        /// <summary>
        /// Gets or sets the stop.
        /// </summary>
        /// <value>
        /// The stop.
        /// </value>
        decimal Stop { get; }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>
        /// The strategy.
        /// </value>
        int? Strategy { get; }

        /// <summary>
        /// Gets the symbol text.
        /// </summary>
        /// <value>
        /// The symbol text.
        /// </value>
        string SymbolText { get; }

        /// <summary>
        /// Gets or sets the target quantity.
        /// </summary>
        /// <value>
        /// The target quantity.
        /// </value>
        decimal TargetQuantity { get; }
        /// <summary>
        /// Gets or sets the tif.
        /// </summary>
        /// <value>
        /// The tif.
        /// </value>
        string TIF { get; }

        /// <summary>
        /// Gets or sets the ExpireDate
        /// </summary>
        string ExpireTime { get; }
        /// <summary>
        /// Gets or sets the trade attribute1.
        /// </summary>
        /// <value>
        /// The trade attribute1.
        /// </value>
        string TradeAttribute1 { get; }

        /// <summary>
        /// Gets or sets the trade attribute2.
        /// </summary>
        /// <value>
        /// The trade attribute2.
        /// </value>
        string TradeAttribute2 { get; }

        /// <summary>
        /// Gets or sets the trade attribute3.
        /// </summary>
        /// <value>
        /// The trade attribute3.
        /// </value>
        string TradeAttribute3 { get; }

        /// <summary>
        /// Gets or sets the trade attribute4.
        /// </summary>
        /// <value>
        /// The trade attribute4.
        /// </value>
        string TradeAttribute4 { get; }

        /// <summary>
        /// Gets or sets the trade attribute5.
        /// </summary>
        /// <value>
        /// The trade attribute5.
        /// </value>
        string TradeAttribute5 { get; }

        /// <summary>
        /// Gets or sets the trade attribute6.
        /// </summary>
        /// <value>
        /// The trade attribute6.
        /// </value>
        string TradeAttribute6 { get; }

        /// <summary>
        /// Gets or sets the trade date.
        /// </summary>
        /// <value>
        /// The trade date.
        /// </value>
        DateTime TradeDate { get; }
        /// <summary>
        /// Gets or sets the trading account.
        /// </summary>
        /// <value>
        /// The trading account.
        /// </value>
        string TradingAccount { get; }

        /// <summary>
        /// Gets the venue.
        /// </summary>
        /// <value>
        /// The venue.
        /// </value>
        string Venue { get; }

        /// <summary>
        /// Gets the venue identifier.
        /// </summary>
        /// <value>
        /// The venue identifier.
        /// </value>
        string VenueId { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is swap.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is swap; otherwise, <c>false</c>.
        /// </value>
        bool IsSwap { get; }

        /// <summary>
        /// Gets the control swap parameter.
        /// </summary>
        /// <value>
        /// The control swap parameter.
        /// </value>
        CtrlSwapParameters CtrlSwapParameter { get; }

        /// <summary>
        /// Gets and Set OTC swap parameter.
        /// </summary>
        OTCTradeData OTCParameters { get; }

        bool IsUseCustodianAsExecutingBroker { get; }

		/// <summary>
        /// The dictionary for account broker mapping for selected fund
        /// </summary>
        Dictionary<int, int> AccountBrokerMapping { get; }

        /// <summary>
        /// Draws the control.
        /// </summary>
        /// <param name="secMasterObj">The sec master object.</param>
        void DrawControl(SecMasterBaseObj secMasterObj);

        /// <summary>
        /// Fills the account combo.
        /// </summary>
        /// <param name="sortTable">The sort table.</param>
        void FillAccountCombo(DataTable sortTable);

        /// <summary>
        /// Fills the broker combo value.
        /// </summary>
        /// <param name="dt">The dt.</param>
        void FillBrokerComboValue(DataTable dt);

        /// <summary>
        /// Fills the type of the order.
        /// </summary>
        /// <param name="types">The types.</param>
        void FillOrderType(DataTable types);

        /// <summary>
        /// Fills the order side.
        /// </summary>
        /// <param name="sides">The sides.</param>
        void FillOrderSide(DataTable sides);

        /// <summary>
        /// Fills the tif.
        /// </summary>
        /// <param name="tifs">The tifs.</param>
        void FillTIF(DataTable tifs);

        /// <summary>
        /// Fills the venue combo.
        /// </summary>
        /// <param name="dt">The dt.</param>
        void FillVenueCombo(DataTable dt);

        /// <summary>
        /// Launches the symbol lookup method.
        /// </summary>
        /// <param name="args">The arguments.</param>
        void LaunchSymbolLookupMethod(ListEventAargs args);

        /// <summary>
        /// Refreshes the control.
        /// </summary>
        void RefreshControl(bool refreshOption);

        /// <summary>
        /// Sets the algo details.
        /// </summary>
        /// <param name="auecID">The auec identifier.</param>
        void SetAlgoDetails(int auecID);

        /// <summary>
        /// Sets the error message.
        /// </summary>
        /// <param name="errMessage">The error message.</param>
        void SetLabelMessage(string errMessage);

        /// <summary>
        /// Awaits the symbol validation.
        /// </summary>
        void AwaitSymbolValidation();

        /// <summary>
        /// Sets the message box text.
        /// </summary>
        /// <param name="message">The message.</param>
        void SetMessageBoxText(string message);

        /// <summary>
        /// Sets the symbol l1 strip.
        /// </summary>
        /// <param name="_symbol">The _symbol.</param>
        void SetSymbolL1Strip(string _symbol);

        /// <summary>
        /// Stops the live feed.
        /// </summary>
        void StopLiveFeed();

        /// <summary>
        /// Updates the caption.
        /// </summary>
        void UpdateCaption();

        /// <summary>
        /// Updates the drop down.
        /// </summary>
        /// <param name="startwith">The startwith.</param>
        /// <param name="items">The items.</param>
        void UpdateDropDown(string startwith, IList<string> items);

        /// <summary>
        /// Updates the symbol position and expose.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="exposure">The exposure.</param>
        void UpdateSymbolPositionExposeAndPNL(double position, double exposure, double pnl);

        /// <summary>
        /// Adds the custom preference to account combo.
        /// </summary>
        /// <param name="p">The p.</param>
        void AddCustomPreferenceToAccountCombo(int prefID);

        /// <summary>
        /// Resets the ticket.
        /// </summary>
        void ResetTicket();

        /// <summary>
        /// Sets the ticket preferences.
        /// </summary>
        /// <param name="tradingTicketuserUiPrefs">The trading ticket user UI prefs.</param>
        /// <param name="companyTradingTicketUiPrefs">The company trading ticket UI prefs.</param>
        void SetTicketPreferences(TradingTicketUIPrefs tradingTicketuserUiPrefs, TradingTicketUIPrefs companyTradingTicketUiPrefs);

        void SetAUECDateInTicket(DateTime dateTime);

        void FillCommissionBasis(IList list);

        /// <summary>
        /// Sets the Custom Message Box message and header.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="message"></param>
        void SetCustomMessageBoxHeaderAndText(string header, string message);

    }
}
