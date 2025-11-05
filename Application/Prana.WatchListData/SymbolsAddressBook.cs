using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.WatchListData
{
    public class SymbolsAddressBook
    {

        /// <summary>
        ///This dictionary is stores key as symbol and value is total no of tab contains that symbol. 
        /// </summary>
        Dictionary<string, int> _symbolsCountDict = new Dictionary<string, int>();
        public Dictionary<string, int> SymbolsCountDict
        {
            get { return _symbolsCountDict; }
        }

        /// <summary>
        /// This dictionary is store the tab name as key and set of symbols for the tab as value.      
        /// </summary>
        Dictionary<string, HashSet<string>> _tabWiseSymbols = new Dictionary<string, HashSet<string>>();
        public Dictionary<string, HashSet<string>> TabWiseSymbols
        {
            get { return _tabWiseSymbols; }
        }


        /// <summary>
        ///This dictionary is use to contains the tab names as key and index no as value.       
        /// </summary>
        Dictionary<string, int> _tabNamesDict = new Dictionary<string, int>();

        public Dictionary<string, int> TabNamesDict
        {
            get { return _tabNamesDict; }
        }

        /// <summary>
        /// Name of the tab that is permanent and will not be deleted
        /// </summary>
        public string PermanentTabName { get; set; }

        /// <summary>
        /// This property is use to return all symbols from all tabs.    
        /// </summary>
        /// <value>
        /// All symbols.
        /// </value>
        public List<string> AllSymbols
        {
            get
            {
                List<string> allSymbols = new List<string>();
                foreach (string tab in TabWiseSymbols.Keys)
                {
                    allSymbols.AddRange(TabWiseSymbols[tab]);
                }
                return allSymbols;
            }
        }

        /// <summary>
        /// this property is use to return all the tab's name.    
        /// </summary>
        /// <value>
        /// The tab names.
        /// </value>
        public List<string> TabNames
        {
            get { return _tabNamesDict.Keys.ToList(); }
        }

        /// <summary>
        /// This constructor is use to insert all the data of data set into its member.
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        public SymbolsAddressBook(DataSet dataSet)
        {
            try
            {
                DataTable dataTable = dataSet.Tables[0];
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string tabName = dataTable.Rows[i]["TabName"].ToString();
                    if (Convert.ToBoolean(dataTable.Rows[i]["IsPermanent"]))
                        PermanentTabName = tabName;
                    if (!_tabNamesDict.ContainsKey(tabName))
                    {
                        _tabNamesDict.Add(tabName, i);
                        _tabWiseSymbols.Add(tabName, new HashSet<string>());
                    }
                }

                dataTable = dataSet.Tables[1];
                foreach (DataRow row in dataTable.Rows)
                {
                    string symbol = row["Symbol"].ToString();
                    string tabName = row["TabName"].ToString();

                    if (!_tabWiseSymbols.ContainsKey(tabName))
                    {
                        HashSet<string> symbolSet = new HashSet<string>();
                        symbolSet.Add(symbol);
                        _tabWiseSymbols.Add(tabName, symbolSet);
                    }
                    else if (_tabWiseSymbols.ContainsKey(tabName) && !_tabWiseSymbols[tabName].Contains(symbol))
                    {
                        _tabWiseSymbols[tabName].Add(symbol);
                    }
                    if (_symbolsCountDict.ContainsKey(symbol))
                        _symbolsCountDict[symbol] = _symbolsCountDict[symbol] + 1;
                    else
                        _symbolsCountDict.Add(symbol, 1);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is use to add symbol details into the tab.
        /// </summary>
        /// <param name="symbol">string</param>
        /// <param name="tabName">string</param>
        public void AddSymbolToTab(string symbol, string tabName)
        {
            try
            {
                if (!_tabWiseSymbols.ContainsKey(tabName))
                {
                    HashSet<string> symbolSet = new HashSet<string>();
                    symbolSet.Add(symbol);
                    _tabWiseSymbols.Add(tabName, symbolSet);
                }
                else
                    _tabWiseSymbols[tabName].Add(symbol);

                if (_symbolsCountDict.ContainsKey(symbol))
                    _symbolsCountDict[symbol] = _symbolsCountDict[symbol] + 1;
                else
                    _symbolsCountDict.Add(symbol, 1);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is use to  check if symbol is in tab.
        /// </summary>
        /// <param name="tabNumber">int</param>
        /// <returns>HashSet<string></returns>
        public bool IsSymbolInTab(string tabName, string symbol)
        {
            try
            {
                if (_tabWiseSymbols.ContainsKey(tabName))
                    return _tabWiseSymbols[tabName].Contains(symbol);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// This method is use to delete symbol from the tab.
        /// </summary>
        /// <param name="symbol"></param>
        public void RemoveSymbolFromTab(string symbol, string tabName)
        {
            try
            {
                _tabWiseSymbols[tabName].Remove(symbol);

                if (_symbolsCountDict.ContainsKey(symbol))
                    _symbolsCountDict[symbol] = _symbolsCountDict[symbol] - 1;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is use to delete all the symbols and the tab too from the all member of this class.
        /// </summary>
        /// <param name="tabName"></param>
        public List<string> RemoveTabWithSymbols(string tabName)
        {
            List<string> symbolsToRemove = new List<string>();
            try
            {
                if (_tabNamesDict.ContainsKey(tabName) && _tabWiseSymbols.ContainsKey(tabName))
                {
                    int tabNumber = _tabNamesDict[tabName];

                    foreach (string symbol in _tabWiseSymbols[tabName])
                    {
                        if (_symbolsCountDict.ContainsKey(symbol))
                        {
                            if (_symbolsCountDict[symbol] <= 1)
                            {
                                _symbolsCountDict.Remove(symbol);
                                symbolsToRemove.Add(symbol);
                            }
                            else
                                _symbolsCountDict[symbol] = _symbolsCountDict[symbol] - 1;
                        }
                    }

                    DeleteTab(tabName);

                    foreach (string key in TabNames)
                    {
                        int ithTabName = _tabNamesDict[key];
                        if (ithTabName > tabNumber)
                        {
                            _tabNamesDict[key] = _tabNamesDict[key] - 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return symbolsToRemove;
        }

        /// <summary>
        /// Deletes the tab.
        /// </summary>
        /// <param name="tabName">Name of the tab.</param>
        private void DeleteTab(string tabName)
        {
            try
            {
                _tabNamesDict.Remove(tabName);
                _tabWiseSymbols.Remove(tabName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is use to add a tab details.
        /// </summary>
        /// <param name="tabName"></param>
        public void AddNewTab(string tabName)
        {
            try
            {
                if (!_tabNamesDict.ContainsKey(tabName))
                {
                    _tabNamesDict.Add(tabName, _tabNamesDict.Count);
                    _tabWiseSymbols[tabName] = new HashSet<string>();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Renames the tab.
        /// </summary>
        /// <param name="NewName">The new name.</param>
        /// <param name="oldName">The old name.</param>
        public void RenameTab(string NewName, string oldName)
        {
            try
            {
                if (_tabNamesDict.ContainsKey(oldName))
                {
                    int tabNumber = _tabNamesDict[oldName];
                    _tabNamesDict.Remove(oldName);
                    _tabNamesDict.Add(NewName, tabNumber);
                }//_symbolListDict
                if (_tabWiseSymbols.ContainsKey(oldName))
                {
                    var symbols = _tabWiseSymbols[oldName];
                    _tabWiseSymbols.Remove(oldName);
                    _tabWiseSymbols.Add(NewName, symbols);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
