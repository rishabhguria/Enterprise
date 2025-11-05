using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveFeedUtility
{
    public class MarketDataProviderManager
    {
        private const string MARKETDATA_PROVIDER_PREFERENCES_FILE = "marketDataProviderPreferences.csv";

        private static DataTable _marketDataProviderPreferences = null;

        /// <summary>
        /// It returns the market data provider preferences for any of the given asset, exchange, symbol combination.
        /// </summary>
        public static void LoadMarketDataProviderPreferences()
        {
            string prefFileFullPath = Path.Combine(Application.StartupPath, MARKETDATA_PROVIDER_PREFERENCES_FILE);
            DataTable providerPrefs = CsvHelper.GetDataTableFromCSVWithHeader(prefFileFullPath);
            providerPrefs.DefaultView.Sort = "Symbols,Exchange,Asset desc";
            _marketDataProviderPreferences = providerPrefs;
        }

        /// <summary>
        /// // It creates a provider wise datatable, so that the symbols can directly be sent to DataProviders for fetching relevant data.
        // e.g.  Yahoo - symbol, asset, exchange table
        //       FMP   - symbol, asset, exchange table
        // TODO :   1) While comparing strings, need to take care - Ignoring case, extra spaces etc
        //          2) Put validation while reading csv so that specific format can be ensured
        /// </summary>
        /// <param name="openSymbolsTable"></param>
        /// <returns></returns>
        public static DataTable MapDataProviderWithSymbolInfo(DataTable openSymbolsTable)
        {
            Dictionary<string, string> providersName = LiveFeedProviderFactory.GetProviderName();

            //DataTable openSymbolsTableWithDataProvider = 
            openSymbolsTable.Columns.Add("MarketDataProvider");

            _marketDataProviderPreferences.DefaultView.RowFilter = "";

            // Priority of assigning data providers if provider column is blank = Symbol -> (Asset + Exchange) -> Generic Asset
            // Symbol wise - First it searches for non blank symbols and start assigning data providers from that
            _marketDataProviderPreferences.DefaultView.RowFilter = "Isnull(Symbols,'') <> ''";
            foreach (DataRowView rowView in _marketDataProviderPreferences.DefaultView)
            {
                openSymbolsTable.DefaultView.RowFilter = "";

                DataRow row = rowView.Row;
                string provider = row["MarketDataProvider"].ToString().Replace(" ", String.Empty);

                //Check for Case InSensitive Provider Name Coming From MarketDataProviderPreferences
                if (providersName.ContainsKey(provider))
                    provider = providersName[provider];

                string[] prefSymbols = row["Symbols"].ToString().Split(new char[] { '|' });

                // TODO : Need to correct for symbols like CG=X, as its creating issue. CG=X symbol will not work here.
                string commaSeparatedPrefSymbols = string.Join(",", prefSymbols.Select(r => "'" + r + "'"));
                openSymbolsTable.DefaultView.RowFilter = "symbol IN (" + commaSeparatedPrefSymbols + ") AND Isnull(MarketDataProvider,'') = ''";
                UpdateProviderOnFilteredRows(openSymbolsTable, provider);
            }

            // (Asset + Exchange) - Exchanges separated by Pipe
            _marketDataProviderPreferences.DefaultView.RowFilter = "Isnull(Asset,'') <> '' AND Isnull(Exchange,'') <> '' ";
            foreach (DataRowView rowView in _marketDataProviderPreferences.DefaultView)
            {
                openSymbolsTable.DefaultView.RowFilter = "";

                DataRow row = rowView.Row;
                string provider = row["MarketDataProvider"].ToString().Replace(" ", String.Empty);

                //Check for Case InSensitive Provider Name Coming From MarketDataProviderPreferences
                if (providersName.ContainsKey(provider))
                    provider = providersName[provider];

                string asset = row["Asset"].ToString().Replace(" ", String.Empty);
                string[] prefExchanges = row["Exchange"].ToString().Split(new char[] { '|' });

                // TODO : Need to correct for symbols like CG=X, as its creating issue. CG=X symbol will not work here.
                string commaSeparatedPrefExchanges = string.Join(",", prefExchanges.Select(r => "'" + r + "'"));
                openSymbolsTable.DefaultView.RowFilter = "asset = '" + asset + "' AND Exchange IN (" + commaSeparatedPrefExchanges + ") AND Isnull(MarketDataProvider,'') = ''";
                UpdateProviderOnFilteredRows(openSymbolsTable, provider);
            }

            // Asset wise - After assigning providers based on specific needs; we now assign the providers in a generic way based on asset class
            _marketDataProviderPreferences.DefaultView.RowFilter = "Isnull(Asset,'') <> '' AND Isnull(Exchange,'') = '' ";
            foreach (DataRowView rowView in _marketDataProviderPreferences.DefaultView)
            {
                openSymbolsTable.DefaultView.RowFilter = "";

                DataRow row = rowView.Row;
                string provider = row["MarketDataProvider"].ToString().Replace(" ", String.Empty);

                //Check for Case InSensitive Provider Name Coming From MarketDataProviderPreferences
                if (providersName.ContainsKey(provider))
                    provider = providersName[provider];

                string asset = row["Asset"].ToString().Replace(" ", String.Empty);
                //string[] prefExchanges = row["Exchange"].ToString().Split(new char[] { '|' });

                // TODO : Need to correct for symbols like CG=X, as its creating issue. CG=X symbol will not work here.
                //AND Exchange='' AND symbol=''
                openSymbolsTable.DefaultView.RowFilter = "asset = '" + asset + "' AND Isnull(MarketDataProvider,'') = ''";
                UpdateProviderOnFilteredRows(openSymbolsTable, provider);
            }
            // TODO : For every call its getting assiged, while it should have only assigned on when the file got changed from the time of last assignment
            // Or the new positions coming up - Think about it.

            return openSymbolsTable;
        }

        private static void UpdateProviderOnFilteredRows(DataTable openSymbolsTable, string provider)
        {
            foreach (DataRowView openRowView in openSymbolsTable.DefaultView)
            {
                openRowView.BeginEdit();
                openRowView["MarketDataProvider"] = provider;
                openRowView.EndEdit();
            }
        }

    }
}
