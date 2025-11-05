
using Prana.WatchListData;
using System.Collections.Generic;

namespace Prana.WatchlistDataService
{
    internal class WatchlistDataManager
    {
        public int UserId
        {
            get
            {
                return _userId;
            }
        }

        private SymbolsAddressBook _watchlistData;
        private int _userId;

        public WatchlistDataManager(int userId)
        {
            _userId = userId;
            _watchlistData = new SymbolsAddressBook(DataBaseOperationHelper.Instance().GetAllTabsDataForUser(userId));
        }
        internal Dictionary<string, int> GetAllTabNames()
        {
            return _watchlistData.TabNamesDict;
        }

        internal Dictionary<string, HashSet<string>> GetTabWiseSymbols()
        {
            return _watchlistData.TabWiseSymbols;
        }

        internal void AddTab(string tabName)
        {
            _watchlistData.AddNewTab(tabName);
        }

        internal void RenameTab(string oldTabName, string newTabName)
        { 
            _watchlistData.RenameTab(newTabName, oldTabName);
        }

        internal void DeleteTab(string tabName)
        {
            _watchlistData.RemoveTabWithSymbols(tabName);
        }

        internal void AddSymbolInTab(string symbol, string tabName)
        {
            _watchlistData.AddSymbolToTab(symbol, tabName);
        }

        internal void RemoveSymbolFromTab(string symbol, string tabName)
        {
            _watchlistData.RemoveSymbolFromTab(symbol, tabName);
        }
    }
}
