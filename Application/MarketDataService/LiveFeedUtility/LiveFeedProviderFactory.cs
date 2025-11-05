using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BusinessObjects;
using System.Reflection;
using System.Collections.Specialized;
using System.Configuration;
using System.Windows.Forms;

namespace LiveFeedUtility
{
    static class LiveFeedProviderFactory
    {
        private static Dictionary<string, DynamicClass> _liveFeedProviders = new Dictionary<string, DynamicClass>();

        /// <summary>
        /// symbol, Asset, Exchange, MarketDataProvider
        /// </summary>
        /// <param name="openSymbolsTable"></param>
        /// <returns></returns>
        internal static DataTable GetLiveFeedData(DataTable openSymbolsTable)
        {
            DataTable liveFeedDataTable = null;
            try
            {
                //Find out distinct MarketDataProviders
                openSymbolsTable.DefaultView.RowFilter = "Isnull(MarketDataProvider, '') <>''";
                DataTable distinctMarketDataProvider = openSymbolsTable.DefaultView.ToTable(true, "MarketDataProvider");

                // For each distinct DataProvider, start fetching the livefeeddata for all the rows
                foreach (DataRow dataProviderRow in distinctMarketDataProvider.Rows)
                {
                    string provider = dataProviderRow["MarketDataProvider"].ToString();
                    openSymbolsTable.DefaultView.RowFilter = "Isnull(MarketDataProvider, '') ='" + provider + "'";

                    // Get the data for specific provider
                    DataTable symbolLiveData = GetProvider(provider).GetLiveFeedData(openSymbolsTable.DefaultView.ToTable());
                    symbolLiveData.PrimaryKey = new DataColumn[] { symbolLiveData.Columns[0] };
                    if (liveFeedDataTable == null)
                        liveFeedDataTable = symbolLiveData;
                    else
                        liveFeedDataTable.Merge(symbolLiveData, false, MissingSchemaAction.Ignore);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
            return liveFeedDataTable;
        }

        static NameValueCollection liveFeedProviders = ((NameValueCollection)ConfigurationManager.GetSection("LiveFeedProviders"));

        /// <summary>
        /// Loads the provider details.
        /// </summary>
        /// <exception cref="System.Exception">Error in accessing the modules configuration</exception>
        internal static void LoadProviderDetails()
        {
            try
            {

                string assemblyFile = string.Empty;
                string providerKey = string.Empty;
                string className = string.Empty;
                string sValue = string.Empty;
                string sPrefControlType = string.Empty;

                if (liveFeedProviders == null)
                {
                    throw new Exception("Error in accessing the provider configuration");
                }
                for (int iIndex = 0, count = liveFeedProviders.Count - 1; iIndex <= count; iIndex++)
                {
                    providerKey = liveFeedProviders.GetKey(iIndex);
                    sValue = liveFeedProviders[providerKey];

                    string[] moduleDetailsBreakUp = sValue.Split('~');
                    assemblyFile = Application.StartupPath + "\\" + moduleDetailsBreakUp[0];
                    className = moduleDetailsBreakUp[1];
                    _liveFeedProviders.Add(providerKey, new DynamicClass(assemblyFile, className, sPrefControlType, providerKey));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private static Dictionary<string, ILiveFeedProvider> _providersDict = new Dictionary<string, ILiveFeedProvider>();

        private static ILiveFeedProvider GetProvider(string providerType)
        {
            ILiveFeedProvider liveFeedProvider = null;
            try
            {
                if (_providersDict.ContainsKey(providerType))
                    return _providersDict[providerType];
                DynamicClass providerToLoad;
                if (!_liveFeedProviders.ContainsKey(providerType))
                    throw new Exception("Provider type not found: " + providerType);
                providerToLoad = _liveFeedProviders[providerType];
                Assembly providerAssembly = Assembly.LoadFrom(providerToLoad.Location);
                Type typeToLoad = providerAssembly.GetType(providerToLoad.Type);
                liveFeedProvider = (ILiveFeedProvider)Activator.CreateInstance(typeToLoad);
                _providersDict.Add(providerType, liveFeedProvider);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
            return liveFeedProvider;
        }

        /// <summary>
        /// Returns a Dictionary&lt;string, string&gt; with Provider Name
        /// </summary>
        /// <returns></returns>
        internal static Dictionary<string,string> GetProviderName()
        {
            Dictionary<string, string> providersName = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string name in liveFeedProviders.AllKeys)
                providersName.Add(name, name);
            return providersName;
        }

        /// <summary>
        /// Gets the sm data provider.
        /// </summary>
        /// <returns></returns>
        internal static ILiveFeedProvider GetSMDataProvider()
        {
            try
            {
                if (!_providersDict.ContainsKey("Yahoo"))
                    return GetProvider("Yahoo");
                else
                    return _providersDict["Yahoo"];
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return null;
        }
    }
}
