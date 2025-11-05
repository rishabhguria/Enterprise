using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.TradingTicket.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Prana.TradingTicket.TTView
{
    /// <summary>
    ///  Multi Trading Ticket View 
    /// </summary>
    public interface IMultiTradingTicketView
    {
        /// <summary>
        /// Gets or Sets whether request came for Edit Orders
        /// </summary>
        bool IsEditOrders { get; set; }

        /// <summary>
        /// Gets the checked ultra grid rows.
        /// </summary>
        /// <value>
        /// The checked ultra grid rows.
        /// </value>
        List<UltraGridRow> CheckedUltraGridRows { get; }

        /// <summary>
        /// Gets or sets the price symbol settings.
        /// </summary>
        /// <value>
        /// The price symbol settings.
        /// </value>
        PriceSymbolValidation PriceSymbolSettings { get; set; }

        /// <summary>
        /// Gets the index of the row from.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        UltraGridRow GetRowFromIndex(int index);

        /// <summary>
        /// Gets the Bulk Account Quantity, if any.
        /// </summary>
        AccountQty BulkAccountQty { get; set; }
        /// <summary>
        /// Enable or Disable MTT Components.
        /// </summary>
        /// <param name="isMTTEnabled">Shows if MTT is enabled or disabled</param>
        /// <returns></returns>
        void EnableOrDisableMTT(bool isMTTEnabled);
        
        void DeleteRowsFromGrid();

        void MarkTradeForDeletion(int index);

        bool IsMTTSettingUp { get; set; }
        /// <summary>
        /// Gets or sets the list orders binded.
        /// </summary>
        /// <value>
        /// The list orders binded.
        /// </value>
        OrderBindingList ListOrdersBinded { get; set; }
        Dictionary<string, List<UltraGridRow>> DictOrdersBindedSymbolwise { get; }
        /// <summary>
        /// Gets the login user.
        /// </summary>
        /// <value>
        /// The login user.
        /// </value>
        CompanyUser LoginUser { get; }
        /// <summary>
        /// Assign combo to the column.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="name">The name.</param>
        void AddDropDownForGivenColumn(ValueList valueList, string name);
        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <param name="result">The result.</param>
        void UpdateCell(int index, string columnName, string result);
        /// <summary>
        /// Update column of Combo at given index.
        /// </summary>
        /// <param name="dropDown">The drop down.</param>
        /// <param name="index">The index.</param>
        /// <param name="name">The name.</param>
        void AddDropDownToGivenIndexForColumn(ValueList valueList, int index, string name);
        /// <summary>
        /// Update Venue Column of MTT grid at given index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="VenueID">The Venue ID.</param>
        void UpgradeGridAfterVenueChange(int index, int VenueID);

        void UpgradeGridAfterBrokerChange(int index, int BrokerID);
        /// <summary>
        /// Update column of Algo Type of given row.
        /// </summary>       
        /// <param name="row">The Row.</param>
        void UpdateAlgoType(UltraGridRow row);
        /// <summary>
        /// Sets the status bar message.
        /// </summary>
        /// <param name="message">The message.</param>
        void SetStatusBarMessage(string message);
        /// <summary>
        /// Sets the trade attribute captions.
        /// </summary>
        /// <param name="lblTA1">The trade attribute label a1.</param>
        /// <param name="lblTA2">The trade attribute t a2.</param>
        /// <param name="lblTA3">The trade attribute t a3.</param>
        /// <param name="lblTA4">The trade attribute t a4.</param>
        /// <param name="lblTA5">The trade attribute t a5.</param>
        /// <param name="lblTA6">The trade attribute t a6.</param>
        void SetTradeAttributeCaptions(string lblTA1, string lblTA2, string lblTA3, string lblTA4, string lblTA5, string lblTA6);
        /// <summary>
        /// Sets the Limit increment from Trade preferences, if any.
        /// </summary>
        /// <param name="LimitIncrement">The limit incrment value.</param>
        void SetLimitIncrement(decimal LimitIncrement);
        /// <summary>
        /// Sets the Stop increment from Trade preferences, if any.
        /// </summary>
        /// <param name="StopIncrement">The Stop increment value.</param>
        void SetStopIncrement(decimal StopIncrement);
        /// <summary>
        /// Sets the Quantity increment from Trade preferences, if any.
        /// </summary>
        /// <param name="QuantityIncrement">The Quantity increment value.</param>
        void SetQuantityIncrement(decimal QuantityIncrement);
        /// <summary>
        /// Occurs when [tag database manager work].
        /// </summary>
        event EventHandler<EventArgs> TagDatabaseManagerWork;
        /// <summary>
        /// Occurs when [bind all drop downs].
        /// </summary>
        event EventHandler<EventArgs> BindAllDropDowns;
        /// <summary>
        /// Occurs when [trade click].
        /// </summary>
        event EventHandler<EventArgs<TradingTicketType>> TradeClick;
        /// <summary>
        /// Snapshots the response.
        /// </summary>
        /// <param name="data">The data.</param>
        void SnapshotResponse(SymbolData data);

        /// <summary>
        /// Gets or sets the trading ticket parent.
        /// </summary>
        /// <value>
        /// The trading ticket parent.
        /// </value>
        TradingTicketParent TradingTicketParent { get; set; }
        /// Occurs when [update venue on broker change].
        /// </summary>
        event EventHandler<EventArgs<int>> UpdateVenueOnBrokerChange;

        event EventHandler<EventArgs<string>> UpdateVenueOnBrokerChangeBulk;
        /// Occurs when [update broker on Account change].
        /// </summary>
        event EventHandler<EventArgs<int>> UpdateBrokerOnAccountChange;
        /// <summary>
        /// Gets the index of the order single based on.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        UltraGridRow GetOrderSingleBasedOnIndex(int index);
        event EventHandler<EventArgs> GetPrice;

        void BindBulkCombo(StrategyCollection values, string columnName);
        void BindBulkCombo(ValueList values, string columnName);
        void FillBulkCommissionBasis(IList list);
        void SetBulkTradeAttributeValueList(BindableValueList[] vls);
        void BindBulkCombo(DataTable values, string columnName);

        event EventHandler<EventArgs> UnwireEventsInPresenter;

        /// <summary>
        /// Occurs when [update venue on broker change].
        /// </summary>
        event EventHandler<EventArgs<int>> UpdateVenueForFirstRowOnBrokerChange;

		/// <summary>
        /// Returns Counter Parties valuelist for the row at given index
        /// </summary>
        ValueList GetCounterParties(int index);

        /// <summary>
        /// Set Error In Broker Column when default broker is not defined for unmapped accounts
        /// </summary>
        void SetUnmappedBrokerError(int index, Dictionary<int, int> accountBrokerMapping);
    }
}
