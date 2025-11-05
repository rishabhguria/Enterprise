using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.TextSearch.SymbolSearch;
using Prana.Global;
using Prana.SecuritySearch.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.SecurityMasterNew.BLL
{
    public class SecMasterSymbolCache
    {
        SymbolSearch symbolSearchObj = null;
        //private static readonly object lockObject = new object();

        //private ConcurrentDictionary<string, object> _searchRoot = new ConcurrentDictionary<string, object>();

        private SymbolSearch getSymbolSearchInstance()
        {
            /*
            lock (lockObject)
            {
                if (symbolSearchObj == null)
                    symbolSearchObj = new SymbolSearch();
            }

            return symbolSearchObj;
            */

            if (symbolSearchObj == null)
                symbolSearchObj = new SymbolSearch();

            return symbolSearchObj;
        }
        /*
        protected IList<string> symbolSearch(ConcurrentDictionary<string, object> dir, string startwith, int limit)
        {
            IList<string> rtn = null;
            if (startwith.Length == 0)
            {
                int left = limit;
                List<string> results = new List<string>();
                if (dir.ContainsKey(""))
                {
                    results.Add("");
                    left--;
                }
                foreach (string key in dir.Keys)
                {
                    if (key.Length == 1)
                    {
                        IList<string> subResults = symbolSearch((ConcurrentDictionary<string, object>)dir[key], "", left);
                        foreach (string sub in subResults)
                        {
                            results.Add(key + sub);
                        }
                        left -= subResults.Count;

                        if (left <= 0)
                            break;
                    }
                }
                return results;
            }
            string s = startwith.Substring(0, 1);
            if (!dir.ContainsKey(s))
            {
                return new List<string>();
            }
            ConcurrentDictionary<string, object> subDir = (ConcurrentDictionary<string, object>)dir[s];
            rtn = symbolSearch(subDir, startwith.Substring(1), limit);
            for (int i = 0; i < rtn.Count; i++)
            {
                rtn[i] = s + rtn[i];
            }
            return rtn;
        }
        */
        /*
        protected void fillData(ConcurrentDictionary<string, object> dir, string symbol)
        {
            if (symbol.Length == 0)
            {
                dir[symbol] = symbol;
                return;
            }
            string s = symbol.Substring(0, 1);
            ConcurrentDictionary<string, object> subDir = null;
            if (dir.ContainsKey(s))
            {
                subDir = (ConcurrentDictionary<string, object>)dir[s];
            }
            else
            {
                subDir = new ConcurrentDictionary<string, object>();
                dir[s] = subDir;
            }
            fillData(subDir, symbol.Substring(1));
        }

        public void fillData(string symbol)
        {
            fillData(_searchRoot, symbol);
        }
        */

        public void symbolFillData(SecMasterBaseObj secMasterBaseObj)
        {
            SymbolSearchDataModel symbolDataModel = new SymbolSearchDataModel();
            if (!string.IsNullOrEmpty(secMasterBaseObj.TickerSymbol))
                symbolDataModel.TickerSymbol = secMasterBaseObj.TickerSymbol;
            else
                symbolDataModel.TickerSymbol = "";

            if (!string.IsNullOrEmpty(secMasterBaseObj.BloombergSymbol))
                symbolDataModel.BloombergSymbol = secMasterBaseObj.BloombergSymbol;
            else
                symbolDataModel.BloombergSymbol = "";

            if (!string.IsNullOrEmpty(secMasterBaseObj.FactSetSymbol))
                symbolDataModel.FactSetSymbol = secMasterBaseObj.FactSetSymbol;
            else
                symbolDataModel.FactSetSymbol = "";

            if (!string.IsNullOrEmpty(secMasterBaseObj.ActivSymbol))
                symbolDataModel.ActivSymbol = secMasterBaseObj.ActivSymbol;
            else
                symbolDataModel.ActivSymbol = "";

            if (!string.IsNullOrEmpty(symbolDataModel.TickerSymbol) || !string.IsNullOrEmpty(symbolDataModel.BloombergSymbol) ||
                 !string.IsNullOrEmpty(symbolDataModel.FactSetSymbol) || !string.IsNullOrEmpty(symbolDataModel.ActivSymbol))
                getSymbolSearchInstance().addSymbol(symbolDataModel);
        }

        public void symbolFillDataList(List<SymbolSearchDataModel> symbolDataList)
        {
            getSymbolSearchInstance().addSymbolList(symbolDataList);
        }

        public IList<string> symbolSearch(string startwith, int limit, ApplicationConstants.SymbologyCodes symbology)
        {
            IList<string> textSearchList = new List<string>();
            IEnumerable<SymbolSearchDataModel> textSearchEnumerable = null;

            switch (symbology)
            {
                case ApplicationConstants.SymbologyCodes.TickerSymbol:
                    textSearchEnumerable = getSymbolSearchInstance().searchSymbol(startwith, limit, "TickerSymbol");
                    foreach (SymbolSearchDataModel symbolDataModel in textSearchEnumerable)
                    {
                        if (!textSearchList.Contains(symbolDataModel.TickerSymbol, StringComparer.OrdinalIgnoreCase))
                            textSearchList.Add(symbolDataModel.TickerSymbol.ToUpper());
                    }
                    break;
                case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                    textSearchEnumerable = getSymbolSearchInstance().searchSymbol(startwith, limit, "BloombergSymbol");
                    foreach (SymbolSearchDataModel symbolDataModel in textSearchEnumerable)
                    {
                        if (!textSearchList.Contains(symbolDataModel.BloombergSymbol, StringComparer.OrdinalIgnoreCase))
                            textSearchList.Add(symbolDataModel.BloombergSymbol.ToUpper());
                    }
                    break;
                case ApplicationConstants.SymbologyCodes.FactSetSymbol:
                    textSearchEnumerable = getSymbolSearchInstance().searchSymbol(startwith, limit, "FactSetSymbol");
                    foreach (SymbolSearchDataModel symbolDataModel in textSearchEnumerable)
                    {
                        if (!textSearchList.Contains(symbolDataModel.FactSetSymbol, StringComparer.OrdinalIgnoreCase))
                            textSearchList.Add(symbolDataModel.FactSetSymbol.ToUpper());
                    }
                    break;
                case ApplicationConstants.SymbologyCodes.ActivSymbol:
                    textSearchEnumerable = getSymbolSearchInstance().searchSymbol(startwith, limit, "ActivSymbol");
                    foreach (SymbolSearchDataModel symbolDataModel in textSearchEnumerable)
                    {
                        if (!textSearchList.Contains(symbolDataModel.ActivSymbol, StringComparer.OrdinalIgnoreCase))
                            textSearchList.Add(symbolDataModel.ActivSymbol.ToUpper());
                    }
                    break;
            }

            textSearchList = textSearchList.Distinct().ToList();
            return textSearchList;
        }

    }
}
