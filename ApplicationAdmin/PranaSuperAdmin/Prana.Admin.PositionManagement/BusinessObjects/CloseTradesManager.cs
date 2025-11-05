using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CloseTradesManager
    {
        public static CloseTradeInterface GetCloseTradeInterfaceData(DateTime date, bool isInternal)
        {
            CloseTradeInterface _closeTradeInterface = new CloseTradeInterface();

            //putting common in both cases, data is dummy anyways... 
            SortableSearchableList<Fund> funds = new SortableSearchableList<Fund>();
            SortableSearchableList<Exchange> exchanges = new SortableSearchableList<Exchange>();
            SortableSearchableList<Underlying> underlyings = new SortableSearchableList<Underlying>();
            _closeTradeInterface.Preferences.Funds = funds;
            _closeTradeInterface.Preferences.Exchanges = exchanges;
            _closeTradeInterface.Preferences.Underlyings = underlyings;

            if (isInternal)
            {
                _closeTradeInterface.Date = date;
                _closeTradeInterface.Preferences.Asset.ID = 1;
                _closeTradeInterface.Preferences.Asset.Name = "Equity";        

                _closeTradeInterface.Preferences.Algorithm = CloseTradeAlogrithm.None;
                _closeTradeInterface.Preferences.DataSourceNameID.ID = 1;
                _closeTradeInterface.Preferences.DataSourceNameID.FullName = "GoldMan Sachs";
                _closeTradeInterface.Preferences.DataSourceNameID.ShortName = "GS";
                _closeTradeInterface.Preferences.DefaultMethodology = CloseTradeMethodology.Automatic;

                SortableSearchableList<AllocatedTrades> allocatedTrades = new SortableSearchableList<AllocatedTrades>();

                AllocatedTrades row1 = new AllocatedTrades();
                row1.ID = 1;
                row1.Quantity = 10000;
                row1.Side = "Sell";
                row1.Symbol = "MSFT";
                row1.FundValue.ID = 1;
                row1.FundValue.Name = "Fund X";
                row1.AveragePrice = 23.23;
                allocatedTrades.Add(row1);

                AllocatedTrades row2 = new AllocatedTrades();
                row2.ID = 1;
                row2.Quantity = 3000;
                row2.Side = "Sell";
                row2.Symbol = "DELL";
                row2.FundValue.ID = 1;
                row2.FundValue.Name = "Fund X";
                row2.AveragePrice = 30.23;
                allocatedTrades.Add(row2);

                _closeTradeInterface.AllocatedTradesData = allocatedTrades;

                SortableSearchableList<PositionEligibleForClosing> positionsEligibleForClosing = new SortableSearchableList<PositionEligibleForClosing>();

                PositionEligibleForClosing position1 = new PositionEligibleForClosing();
                position1.ID = 1234;
                position1.Quantity = 300000;
                position1.Symbol = "Dell";
                position1.FundValue.ID = 1;
                position1.FundValue.Name = "Fund X";
                position1.ClosingQuantity = 3000;
                position1.AveragePrice = 32.45;
                position1.LastActivityDate = DateTime.Now.AddDays(-4.0);
                position1.PositionType = PositionType.Long;
                positionsEligibleForClosing.Add(position1);

                PositionEligibleForClosing position2 = new PositionEligibleForClosing();
                position2.ID = 0045;
                position2.Quantity = 500000;
                position2.Symbol = "MSFT";
                position2.FundValue.ID = 1;
                position2.FundValue.Name = "Fund X";
                position2.ClosingQuantity = -10000;
                position2.AveragePrice = 24.45;
                position2.LastActivityDate = DateTime.Now.AddDays(-2.0);
                position2.PositionType = PositionType.Short;
                positionsEligibleForClosing.Add(position2);

                _closeTradeInterface.PositionsEligibleForClosing = positionsEligibleForClosing;
            }
            else
            {
                _closeTradeInterface.Date = date;
                _closeTradeInterface.Preferences.Asset.ID = 1;
                _closeTradeInterface.Preferences.Asset.Name = "Equity";
                
                _closeTradeInterface.Preferences.Algorithm = CloseTradeAlogrithm.None;
                _closeTradeInterface.Preferences.DataSourceNameID.ID = 1;
                _closeTradeInterface.Preferences.DataSourceNameID.FullName = "GoldMan Sachs";
                _closeTradeInterface.Preferences.DataSourceNameID.ShortName = "GS";
                _closeTradeInterface.Preferences.DefaultMethodology = CloseTradeMethodology.Automatic;

                SortableSearchableList<AllocatedTrades> allocatedTrades = new SortableSearchableList<AllocatedTrades>();

                AllocatedTrades row1 = new AllocatedTrades();
                row1.ID = 1;
                row1.Quantity = 10000;
                row1.Side = "Sell";
                row1.Symbol = "GOOG";
                row1.FundValue.ID = 1;
                row1.FundValue.Name = "Fund Y";
                row1.AveragePrice = 23.23;
                allocatedTrades.Add(row1);

                AllocatedTrades row2 = new AllocatedTrades();
                row2.ID = 1;
                row2.Quantity = 3000;
                row2.Side = "Sell";
                row2.Symbol = "INFY";
                row2.FundValue.ID = 1;
                row2.FundValue.Name = "Fund Y";
                row2.AveragePrice = 30.23;
                allocatedTrades.Add(row2);

                _closeTradeInterface.AllocatedTradesData = allocatedTrades;

                SortableSearchableList<PositionEligibleForClosing> positionsEligibleForClosing = new SortableSearchableList<PositionEligibleForClosing>();

                PositionEligibleForClosing position1 = new PositionEligibleForClosing();
                position1.ID = 1234;
                position1.Quantity = 300000;
                position1.Symbol = "INFY";
                position1.FundValue.ID = 1;
                position1.FundValue.Name = "Fund Y";
                position1.ClosingQuantity = 3000;
                position1.AveragePrice = 32.45;
                position1.LastActivityDate = DateTime.Now.AddDays(-4.0);
                position1.PositionType = PositionType.Long;
                positionsEligibleForClosing.Add(position1);

                PositionEligibleForClosing position2 = new PositionEligibleForClosing();
                position2.ID = 0045;
                position2.Quantity = 500000;
                position2.Symbol = "GOOG";
                position2.FundValue.ID = 1;
                position2.FundValue.Name = "Fund Y";
                position2.ClosingQuantity = -10000;
                position2.AveragePrice = 24.45;
                position2.LastActivityDate = DateTime.Now.AddDays(-2.0);
                position2.PositionType = PositionType.Short;
                positionsEligibleForClosing.Add(position2);

                _closeTradeInterface.PositionsEligibleForClosing = positionsEligibleForClosing;
            }
            return _closeTradeInterface;
        }
    }
}
