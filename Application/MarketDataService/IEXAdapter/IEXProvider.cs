using BusinessObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace IEXAdapter
{
    public class IEXProvider : ILiveFeedProvider
    {
        /// <summary>
        /// The URL
        /// </summary>
        private string URLSTART = string.Empty;

        /// <summary>
        /// The result data table
        /// </summary>
        private DataTable resultDataTable = new DataTable();

        /// <summary>
        /// The configuration
        /// </summary>
        private Configuration config = null;

        /// <summary>
        /// The field mapping
        /// </summary>
        private Dictionary<string, string> _fieldMapping = new Dictionary<string, string>();

        /// <summary>
        /// The currency mapping
        /// </summary>
        private Dictionary<string, Tuple<string, bool, double>> _currencyMapping = new Dictionary<string, Tuple<string, bool, double>>();

        /// <summary>
        /// The exchange mapping
        /// </summary>
        private Dictionary<string, string> _exchangeMapping = new Dictionary<string, string>();

        /// <summary>
        /// The symbol mapping
        /// </summary>
        private Dictionary<string, string> _symbolMapping = new Dictionary<string, string>();

        /// <summary>
        /// The API Key
        /// </summary>
        private static string _APIKEY;

        /// <summary>
        /// The properties
        /// </summary>
        private PropertyInfo[] properties;

        public IEXProvider()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IEXProvider"/> class.
        /// </summary>
        private void Initialize()
        {
            #region Configs
            //Read IEX specific config file
            config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

            //Process Exchange Mapping from config
            AppSettingsSection exchangeSection = (AppSettingsSection)config.GetSection("ExchangeMapping");
            foreach (KeyValueConfigurationElement element in exchangeSection.Settings)
            {
                if (element != null)
                    _exchangeMapping.Add(element.Key, element.Value);
            }

            //Process Column Mapping from config
            AppSettingsSection columnSection = (AppSettingsSection)config.GetSection("ColumnMapping");
            foreach (KeyValueConfigurationElement element in columnSection.Settings)
            {
                if (element != null)
                    _fieldMapping.Add(element.Key, element.Value);
            }

            //Process Symbol Mapping from config
            AppSettingsSection symbolSection = (AppSettingsSection)config.GetSection("SymbolMapping");
            foreach (KeyValueConfigurationElement element in symbolSection.Settings)
            {
                if (element != null)
                    _symbolMapping.Add(element.Key, element.Value);
            }

            //Process Currency to Update from config
            AppSettingsSection currencySection = (AppSettingsSection)config.GetSection("CurrenciesToUpdate");
            foreach (KeyValueConfigurationElement element in currencySection.Settings)
            {
                if (element != null)
                {
                    string[] values = element.Value.Split(',');
                    if (values.Length == 3)
                    {
                        string newCurr = values[0];
                        bool isMul = values[1].Equals("M");
                        double num = Convert.ToDouble(values[2]);

                        _currencyMapping.Add(element.Key, new Tuple<string, bool, double>(newCurr, isMul, num));
                    }
                }
            }

            AppSettingsSection apiKey = (AppSettingsSection)config.GetSection("APIKey");
            SetApiKey(apiKey);

            #endregion

            Type type = typeof(MarketData);
            properties = type.GetProperties();
            //Removed processing of additional columns other than config
            properties = properties.Where(x => _fieldMapping.ContainsKey(x.Name)).ToArray<PropertyInfo>();

            resultDataTable.Columns.Add(new DataColumn("Symbol"));
            foreach (PropertyInfo info in properties)
            {
                resultDataTable.Columns.Add(new DataColumn(_fieldMapping[info.Name].Trim()));
            }
            DataColumn pricingSourceCol = new DataColumn("PRICINGSOURCE", typeof(System.String));
            pricingSourceCol.DefaultValue = "IEX";
            resultDataTable.Columns.Add(pricingSourceCol);
        }

        /// <summary>
        /// Process API Key from config
        /// </summary>
        /// <param name="apiKey"></param>
        private void SetApiKey(AppSettingsSection apiKey)
        {
            try
            {
                foreach (KeyValueConfigurationElement element in apiKey.Settings)
                {
                    if (element.Key.Equals("IEXAPI"))
                        _APIKEY = element.Value;
                    else if (element.Key.Equals("APIURL"))
                        URLSTART = "https://" + element.Value + "/stable/stock/";
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

        }

        /// <summary>
        /// Get API Key
        /// </summary>
        /// <returns></returns>
        private static string GetApiKey()
        {
            return _APIKEY;
        }

        /// <summary>
        /// Gets the open symbols.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetMappedSymbolsForProvider(DataTable openSymbols)
        {
            Dictionary<string, string> mappedSymbols = new Dictionary<string, string>();
            try
            {
                Dictionary<string, string> replaceMapping = new Dictionary<string, string>();
                foreach (string s in _symbolMapping.Keys)
                    replaceMapping.Add(s, _symbolMapping[s]);

                //Dictionary<string, string> openSymbols = DataManager.GetOpenSymbolsList();
                foreach (DataRow symbolRow in openSymbols.Rows)
                {
                    string symbol = symbolRow["symbol"].ToString();
                    string asset = symbolRow["Asset"].ToString();
                    if (!mappedSymbols.ContainsKey(symbol))
                    {
                        if (replaceMapping.ContainsKey(symbol))
                            mappedSymbols.Add(symbol, replaceMapping[symbol]);
                        else
                        {
                            foreach (string s in _exchangeMapping.Keys)
                            {
                                if (symbol.EndsWith(s))
                                    mappedSymbols.Add(symbol, symbol.Replace(s, _exchangeMapping[s]));
                            }
                        }
                        if (!mappedSymbols.ContainsKey(symbol))
                            mappedSymbols.Add(symbol, symbol);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return mappedSymbols;
        }

        /// <summary>
        /// Gets the live feed data.
        /// </summary>
        /// <param name="openSymbols">The open symbols.</param>
        /// <returns></returns>
        private DataTable GetLiveFeedData(Dictionary<string, string> openSymbols)
        {

            Dictionary<string, MarketData> results = new Dictionary<string, MarketData>();
            string URLEND = GetURLEnd();
            foreach (KeyValuePair<string, string> kvp in openSymbols)
            {
                try
                {
                    if (kvp.Key.Contains("Curncy"))
                    {
                        URLSTART = URLSTART.Replace("stock/", "fx/latest?symbols=");
                        URLEND = URLEND.Replace("/quote?", "&");
                    }

                    string iexSymbolRequestURL = URLSTART + kvp.Value + URLEND;
                    Logger.LogMessage("Sending request of Symbols to IEX - " + Environment.NewLine + kvp.Value + Environment.NewLine + " On IEX URL : " + Environment.NewLine + iexSymbolRequestURL);

                    string json = new WebClient().DownloadString(iexSymbolRequestURL);
                    Logger.LogMessage("Reponse received for IEX. ");
                    json = json.Trim('[', ']');
                    var res = JsonConvert.DeserializeObject<MarketData>(json);
                    if (res == null)
                    {
                        Logger.LogMessage("Symbol is not present for : " + kvp.Key + " while fetching from IEX");
                    }
                    else
                    {
                        if (res.rate != null)
                        {
                            res.latestPrice = res.rate;
                        }
                        ResponseModification(res);
                        results.Add(kvp.Key, res);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogMessage("Data not coming for : " + kvp.Key + " while fetching from IEX" + Environment.NewLine + ex.ToString());
                }
            }
            DataTable dt = CreateDataTable(results);
            return dt;
        }

        private string GetURLEnd()
        {
            string urlEnd = "/quote?token=" + GetApiKey();
            return urlEnd;
        }

        /// <summary>
        /// Responses the modification.
        /// </summary>
        /// <param name="res">The resource.</param>
        private void ResponseModification(MarketData res)
        {
            try
            {
                if (res.timestamp.HasValue)
                {
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(res.timestamp.Value);
                    res.updateTime = dateTimeOffset.DateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Creates the data table.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <returns></returns>
        private DataTable CreateDataTable(Dictionary<string, MarketData> dict)
        {
            DataTable dt = resultDataTable.Clone();
            try
            {
                foreach (string key in dict.Keys)
                {
                    object[] values = new object[properties.Length + 1];
                    values[0] = key;
                    for (int i = 0; i < properties.Length; i++)
                    {
                        PropertyInfo inf = properties[i];
                        values[i + 1] = properties[i].GetValue(dict[key]);
                    }

                    dt.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return dt;
        }


        //Datatable with columns - symbol, Asset, Exchange, MarketDataProvider
        public DataTable GetLiveFeedData(DataTable symbolInfo)
        {
            Dictionary<string, string> mappedSymbols = GetMappedSymbolsForProvider(symbolInfo);
            return GetLiveFeedData(mappedSymbols);
        }


        public Dictionary<string, string> ValidateSymbol(string symbol, string asset)
        {
            throw new NotImplementedException();
        }
    }
}
