using Prana.BusinessObjects.TextSearch.SymbolSearch;
using Prana.Interfaces.TextSearch.SymbolSearch;
using Prana.LogManager;
using Prana.SecuritySearch.Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.SecuritySearch.Search
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class SymbolSearch : ISymbolSearch
    {
        SymbolSearchBase symbolSearchBase = null;

        //private static readonly object lockObject = new object();

        /// <summary>
        /// Constructor for the SymbolSearch class.
        /// It initializes the SymbolSearchBase class object and also sets the caching directory for the particular class.
        /// One particular search class will have one specified directory only.
        /// 
        /// </summary>
        public SymbolSearch()
        {
            if (symbolSearchBase == null)
            {
                symbolSearchBase = new SymbolSearchBase();
                setDirectory();
            }
        }

        /// <summary>
        /// This method just return the SymbolSearchBase class object, which should not be null.
        /// </summary>
        /// <returns></returns>
        private SymbolSearchBase getSymbolSearchBaseInstance()
        {
            /*
            lock (lockObject)
            {
                if (symbolSearchBase == null)
                {
                    symbolSearchBase = new SymbolSearchBase();
                    setDirectory();
                }
            }

            return symbolSearchBase;
            */

            return symbolSearchBase;
        }

        /// <summary>
        /// This method is used to set the cache directory for the SymbolSearchBase class.
        /// This method should be called only once from the Constructor of this class.
        /// Once the cache directory is set, it should not be change again.
        /// 
        /// </summary>
        public void setDirectory()
        {
            getSymbolSearchBaseInstance().IndexingDir = CommonSearchConstants.SYMBOL_INDEX_DIRECTORY;
        }

        /// <summary>
        /// This method is used to add SymbolSearchDataModel list into the specified cache directory.
        /// It calls the addUpdateLuceneIndex method of SymbolSearchBase class.
        /// 
        /// </summary>
        /// <param name="symbolDataList"></param>
        public void addSymbolList(List<SymbolSearchDataModel> symbolDataList)
        {
            try
            {
                getSymbolSearchBaseInstance().addUpdateLuceneIndex(symbolDataList, false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is used to add SymbolSearchDataModel Object into the specified cache directory.
        /// It calls the addUpdateLuceneIndex method of SymbolSearchBase class.
        /// 
        /// </summary>
        /// <param name="symbolData"></param>
        public void addSymbol(SymbolSearchDataModel symbolData)
        {
            try
            {
                getSymbolSearchBaseInstance().addUpdateLuceneIndex(symbolData, true);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// This method is used to search for symbol from the specified cache directory, starting with the given text.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="limit"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public IEnumerable<SymbolSearchDataModel> searchSymbol(string searchText, int limit, string fieldName)
        {
            IEnumerable<SymbolSearchDataModel> resultSampleData = new List<SymbolSearchDataModel>();
            try
            {
                resultSampleData = getSymbolSearchBaseInstance().symbolSearch(searchText, limit, fieldName);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return resultSampleData;
        }

    }
}
