using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.TradingTicket.TTView
{
    /// <summary>
    /// this View Interface is used by TradingTicketPresenter.cs to interact with TradingTicket.cs Form
    /// </summary>
    public interface ITradingTicketView : ITicketView
    {
        /// <summary>
        /// Gets the deal in.
        /// </summary>
        /// <value>
        /// The deal in.
        /// </value>
        string DealIn { get; }

        TradingTicketParent TradingTicketParent { set; get; }

        /// <summary>
        /// Adds the ps tto account combo.
        /// </summary>
        void RemovePTTFromAccountCombo();

        /// <summary>
        /// Calls the symbol control text entered event.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        void CallSymbolControlTextEnteredEvent(string symbol);

        /// <summary>
        /// Closes the ticket.
        /// </summary>
        void CloseTicket();

        /// <summary>
        /// Sets the is swap.
        /// </summary>
        /// <param name="isSwap">if set to <c>true</c> [is swap].</param>
        void SetIsSwap(bool isSwap);

        /// <summary>
        /// Enables the prana symbol control.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        void EnablePranaSymbolControl(bool value);

        /// <summary>
        /// Fills the execution instructions.
        /// </summary>
        /// <param name="executionInstrucitons">The execution instrucitons.</param>
        void FillExecutionInstructions(DataTable executionInstrucitons);

        /// <summary>
        /// Fills the handling instruction.
        /// </summary>
        /// <param name="handlingInstrucitons">The handling instrucitons.</param>
        void FillHandlingInstruction(DataTable handlingInstrucitons);

        /// <summary>
        /// Fills the settlement currency.
        /// </summary>
        /// <param name="currencies">The currencies.</param>
        void FillSettlementCurrency(ValueList currencies);

        /// <summary>
        /// Fills the startegy combo.
        /// </summary>
        /// <param name="strategies">The strategies.</param>
        void FillStartegyCombo(StrategyCollection strategies);

        /// <summary>
        /// Fills the trading account combo.
        /// </summary>
        /// <param name="newDt">The new dt.</param>
        void FillTradingAccountCombo(System.Data.DataTable newDt);

        /// <summary>
        /// Sets the combo side value.
        /// </summary>
        /// <param name="order">The order.</param>
        void SetComboSideValue(OrderSingle order);

        /// <summary>
        /// Sets the dealIn combo value.
        /// </summary>
        /// <param name="currencyList">The currency list.</param>
        void SetDealInComboValue(Dictionary<int, string> currencyList);

        /// <summary>
        /// Fills the settlement fx operator combo.
        /// </summary>
        /// <param name="SettlementFxOperatorEnumList">The settlement fx operator enum list.</param>
        void FillFxOperatorCombo(ValueList FxOperatorEnumList);

        /// <summary>
        /// Sets the message box text and get dialog result.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="messageBoxButtons">The message box buttons.</param>
        /// <returns></returns>
        DialogResult SetMessageBoxTextAndGetDialogResult(string errorMessage, string caption, MessageBoxButtons messageBoxButtons);

        /// <summary>
        /// Sets the prana symbol control text.
        /// </summary>
        /// <param name="text">The text.</param>
        void SetPranaSymbolControlText(string text);

        /// <summary>
        /// Sets the prana Fund default value text.
        /// </summary>
        /// <param name="text">The text.</param>
        void SetPranaFundDefaultValue(OrderSingle order);

        /// <summary>
        /// Sets the trade attribute labels.
        /// </summary>
        /// <param name="lblTA1">The label t a1.</param>
        /// <param name="lblTA2">The label t a2.</param>
        /// <param name="lblTA3">The label t a3.</param>
        /// <param name="lblTA4">The label t a4.</param>
        /// <param name="lblTA5">The label t a5.</param>
        /// <param name="lblTA6">The label t a6.</param>
        void SetTradeAttributeLabels(string lblTA1, string lblTA2, string lblTA3, string lblTA4, string lblTA5, string lblTA6);

        /// <summary>
        /// Sets the trade attribute value list.
        /// </summary>
        /// <param name="vls">The VLS.</param>
        void SetTradeAttributeValueList(BindableValueList[] vls);

        /// <summary>
        /// Wires the side change event.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        void WireSideChangeEvent(bool value);

        /// <summary>
        /// Getting preferred Symbol based on selected Symbolbology from Ticker
        /// </summary>
        string GetPreferredSymbolFromTicker(string tickerSymbol);

        /// <summary>
        /// Level1s the snapshot response.
        /// </summary>
        /// <param name="l1data">The l1data.</param>
        void Level1SnapshotResponse(SymbolData l1data);

    }
}