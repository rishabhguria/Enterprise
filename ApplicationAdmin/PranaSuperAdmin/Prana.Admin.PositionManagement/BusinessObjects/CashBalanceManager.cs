using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class CashBalanceManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CashBalanceManager"/> class.
        /// </summary>
        private CashBalanceManager()
        {

        }


        /// <summary>
        /// Gets the cash balance data for data source.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        /// <returns></returns>
        public static CashBalanceManagement GetCashBalanceDataForDataSource(DataSourceNameID dataSourceNameID, DateTime date)
        {
            CashBalanceManagement cashBalanceData = new CashBalanceManagement();

            cashBalanceData.DataSourceNameID = dataSourceNameID;
            cashBalanceData.Date = date;

            SortableSearchableList<CashBalanceEntry> cashBalanceManagementGridData = new SortableSearchableList<CashBalanceEntry>();
           
            if (int.Equals(dataSourceNameID.ID, 1) || int.Equals(dataSourceNameID.ID, 0))
            {
                CashBalanceEntry row1 = new CashBalanceEntry();
                row1.DataSourceNameID.ID = 1;
                row1.DataSourceNameID.FullName = "GoldMan Sachs";
                row1.DataSourceNameID.ShortName = "GS";
                row1.Fund.Name = "Long";
                row1.Currency.Name = "USD";
                row1.OpeningCash = 1000000.0;
                row1.TradingCashSpent = 100000.0;
                row1.TradingCashReceived = 20000.0;
                row1.Transactions = "MISC";
                row1.NetAmount = 300.0;
                row1.ProjectedClosingCash = 919700.0;
                cashBalanceManagementGridData.Add(row1);

                CashBalanceEntry row2 = new CashBalanceEntry();
                row2.DataSourceNameID.ID = 1;
                row2.DataSourceNameID.FullName = "GoldMan Sachs";
                row2.DataSourceNameID.ShortName = "GS";
                row2.Fund.Name = "Short";
                row2.Currency.Name = "Euro";
                row2.OpeningCash = 2000000.0;
                row2.TradingCashSpent = 250000.0;
                row2.TradingCashReceived = 0.0;
                row2.Transactions = "MISC";
                row2.NetAmount = 20000.0;
                row2.ProjectedClosingCash = 1770000.0;
                cashBalanceManagementGridData.Add(row2);
            }
            if((int.Equals(dataSourceNameID.ID, 32)) || int.Equals(dataSourceNameID.ID, 0))
            {
                CashBalanceEntry row1 = new CashBalanceEntry();
                row1.DataSourceNameID.ID = 32;
                row1.DataSourceNameID.FullName = "Merrill Lynch";
                row1.DataSourceNameID.ShortName = "ML";
                row1.Fund.Name = "Debt";
                row1.Currency.Name = "Rupee";
                row1.OpeningCash = 1000000.0;
                row1.TradingCashSpent = 100000.0;
                row1.TradingCashReceived = 20000.0;
                row1.Transactions = "MISC";
                row1.NetAmount = 300.0;
                row1.ProjectedClosingCash = 919700.0;
                cashBalanceManagementGridData.Add(row1);

                CashBalanceEntry row2 = new CashBalanceEntry();
                row2.DataSourceNameID.ID = 1;
                row2.DataSourceNameID.FullName = "Merrill Lynch";
                row2.DataSourceNameID.ShortName = "ML";
                row2.Fund.Name = "Equity";
                row2.Currency.Name = "Pound";
                row2.OpeningCash = 2000000.0;
                row2.TradingCashSpent = 250000.0;
                row2.TradingCashReceived = 0.0;
                row2.Transactions = "MISC";
                row2.NetAmount = 20000.0;
                row2.ProjectedClosingCash = 1770000.0;
                cashBalanceManagementGridData.Add(row2);
            }
            if ((int.Equals(dataSourceNameID.ID, 35)) || int.Equals(dataSourceNameID.ID, 0))
            {
                CashBalanceEntry row1 = new CashBalanceEntry();
                row1.DataSourceNameID.ID = 35;
                row1.DataSourceNameID.FullName = "Morgan Stanley";
                row1.DataSourceNameID.ShortName = "MS";
                row1.Fund.Name = "MidCap";
                row1.Currency.Name = "Euro";
                row1.OpeningCash = 1000000.0;
                row1.TradingCashSpent = 100000.0;
                row1.TradingCashReceived = 20000.0;
                row1.Transactions = "MISC";
                row1.NetAmount = 300.0;
                row1.ProjectedClosingCash = 919700.0;
                cashBalanceManagementGridData.Add(row1);

                CashBalanceEntry row2 = new CashBalanceEntry();
                row2.DataSourceNameID.ID = 1;
                row2.DataSourceNameID.FullName = "Morgan Stanley";
                row2.DataSourceNameID.ShortName = "MS";
                row2.Fund.Name = "WealthMultiplier";
                row2.Currency.Name = "Yen";
                row2.OpeningCash = 2000000.0;
                row2.TradingCashSpent = 250000.0;
                row2.TradingCashReceived = 0.0;
                row2.Transactions = "MISC";
                row2.NetAmount = 20000.0;
                row2.ProjectedClosingCash = 1770000.0;
                cashBalanceManagementGridData.Add(row2);
            }

            cashBalanceData.CashBalanceManagementDataListItems = cashBalanceManagementGridData;
            return cashBalanceData;
        }
    }
}
