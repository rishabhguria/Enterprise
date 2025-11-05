using BusinessObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Globalization;

namespace YahooAdapter
{
    public class YahooProvider : ILiveFeedProvider
    {

        public YahooProvider()
        {
            Initialize();
        }

        /// <summary>
        /// The URL
        /// </summary>
        private string URL = "https://query1.finance.yahoo.com/v7/finance/quote?symbols=";

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

        private static int BATCH_SYMBOL_REQUEST_LIMIT;

        /// <summary>
        /// The properties
        /// </summary>
        private PropertyInfo[] properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="YahooProvider"/> class.
        /// </summary>
        private void Initialize()
        {
            #region Configs
            try
            {
                //Read yahoo specific config file
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

                AppSettingsSection apiKey = (AppSettingsSection)config.GetSection("APIDetails");
                foreach (KeyValueConfigurationElement element in apiKey.Settings)
                    SetApiDetails(apiKey);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

            #endregion

            Type type = typeof(MarketDataParamter);
            properties = type.GetProperties();
            //Removed processing of additional columns other than config
            properties = properties.Where(x => _fieldMapping.ContainsKey(x.Name)).ToArray<PropertyInfo>();

            resultDataTable.Columns.Add("Symbol");

            for (int i = 0; i < properties.Count(); i++)
            {
                resultDataTable.Columns.Add(_fieldMapping[properties[i].Name]);
            }

            DataColumn pricingSourceCol = new DataColumn("PRICINGSOURCE", typeof(System.String));
            pricingSourceCol.DefaultValue = "Yahoo";
            resultDataTable.Columns.Add(pricingSourceCol);
        }

        /// <summary>
        /// SetApiDetails
        /// </summary>
        /// <param name="apiKey"></param>
        private void SetApiDetails(AppSettingsSection apiKey)
        {
            try
            {
                foreach (KeyValueConfigurationElement element in apiKey.Settings)
                {
                    if (element.Key.Equals("URL"))
                        URL = element.Value;
                    else
                        BATCH_SYMBOL_REQUEST_LIMIT = int.Parse(element.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

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
                foreach (DataRow symbolRow in openSymbols.Rows)
                {

                    string symbol = symbolRow["symbol"].ToString();
                    string asset = symbolRow["Asset"].ToString();

                    if (!mappedSymbols.ContainsKey(symbol))
                    {
                        mappedSymbols.Add(symbol, GetproviderSymbol(symbol, asset));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return mappedSymbols;
        }

        private string GetproviderSymbol(string symbol, string asset)
        {
            try
            {
                if (_symbolMapping.ContainsKey(symbol))
                    return _symbolMapping[symbol];
                else
                {
                    foreach (string s in _exchangeMapping.Keys)
                    {
                        if (symbol.EndsWith(s))
                            return symbol.Replace(s, _exchangeMapping[s]);
                    }
                }
                switch (asset)
                {
                    case "Future":
                        return symbol.Replace(" ", string.Empty) + ".CME";
                    case "EquityOption":
                        {
                            string[] arr = symbol.Split(' ');
                            int mon = 1;
                            char cp = 'C';
                            if (arr.Length == 2)
                            {
                                char month = arr[1][2];
                                if ("ABCDEFGHIJKL".Contains(month))
                                {
                                    mon = month - 'A' + 1;
                                }
                                else
                                {
                                    mon = month - 'L';
                                    cp = 'P';
                                }
                            }

                            if (arr[1].LastIndexOf('D') > 0)
                            {
                                string temp = arr[1].Substring(3);
                                string undsym = arr[0].Substring(2);
                                string date = arr[1].Substring(arr[1].LastIndexOf('D') + 1);
                                int strPrice = (int)(Convert.ToDouble(temp.Substring(0, temp.IndexOf('D'))) * 1000);
                                string ysym = undsym + arr[1].Substring(0, 2) + ((mon.ToString().Length < 2) ? "0" + mon.ToString() : mon.ToString()) + (date.Length < 2 ? "0" + date : date) + cp + strPrice.ToString("D8");
                                return ysym;
                            }
                            break;
                        }
                    case "FXForward":
                        {
                            string[] arr = symbol.Split(' ');
                            if (arr.Length > 1)
                                return arr[0].Replace("/", "") + "=X";
                            break;
                        }
                    case "FX":
                        {
                            if (symbol.Contains("/"))
                                return symbol.Replace("/", "") + "=X";
                            else if (symbol.Contains(' '))
                            {
                                string[] arr = symbol.Split(' ');
                                string curr = arr[0];
                                switch (curr)
                                {
                                    case "EUR":
                                    case "AUD":
                                    case "GBP":
                                    case "NZD":
                                        return curr + "USD=X";
                                    default:
                                        return "USD" + curr + "=X";
                                }
                            }
                            break;
                        }
                }
                if (symbol.Contains("/"))
                    symbol = symbol.Replace("/", "");
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Error Occured for Symbol: " + symbol + " " + ex.Message);
            }
            return symbol;
        }

        /// <summary>
        /// Gets the live feed data.
        /// </summary>
        /// <param name="openSymbols">The open symbols.</param>
        /// <returns></returns>
        private DataTable GetLiveFeedData(Dictionary<string, string> openSymbols)
        {
            Dictionary<string, MarketDataParamter> results = new Dictionary<string, MarketDataParamter>();
            try
            {
                StringBuilder symbolsToRequestInBatch = new StringBuilder();
                int count = 0;
                List<MarketDataParamter> symbolData = new List<MarketDataParamter>();
                foreach (var symbol in openSymbols)
                {
                    if (openSymbols[symbol.Key].Equals(openSymbols.Last().Value) || count >= BATCH_SYMBOL_REQUEST_LIMIT)
                    {
                        symbolsToRequestInBatch.AppendFormat(symbol.Value);
                        try
                        {
                            string yahooSymbolRequestURL = URL + symbolsToRequestInBatch;
                            Logger.LogMessage("Sending request of Symbols to Yahoo - " + Environment.NewLine + symbolsToRequestInBatch + Environment.NewLine + " On Yahoo URL : " + Environment.NewLine + yahooSymbolRequestURL);

                            var json = new WebClient().DownloadString(yahooSymbolRequestURL);
                            symbolData.AddRange(JsonConvert.DeserializeObject<MarketDataList>(json).quoteResponse.result.ToList());
                            Logger.LogMessage("Reponse received for Yahoo. ");
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex);
                        }
                        symbolsToRequestInBatch.Clear();
                        count = 0;
                    }
                    else
                        symbolsToRequestInBatch.AppendFormat(symbol.Value + ",");
                    count++;
                }

                if (symbolData.Select(symbolValue => symbolValue.symbol).Equals("") || symbolData.Select(symbolValue => symbolValue.symbol) == null)
                    Logger.LogMessage("Symbol is not present for : " + symbolData.Select(symbolValue => symbolValue.symbol) + " while fetching from Yahoo");
                else
                {
                    foreach (var a in symbolData)
                    {
                        ResponseModification(a);
                        if (openSymbols.ContainsValue(a.symbol))
                        {
                            MarketDataParamter res = a;
                            var keys = openSymbols.Where(x => x.Value == a.symbol).Select(x => x.Key);
                            foreach (string symKey in keys)
                                results[symKey] = res;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            DataTable dt = CreateDataTable(results);
            return dt;
        }

        /// <summary>
        /// Responses the modification.
        /// </summary>
        /// <param name="res">The resource.</param>
        private void ResponseModification(MarketDataParamter res)
        {
            try
            {
                if (res.regularMarketTime.HasValue)
                {
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(res.regularMarketTime.Value);
                    res.updateTime = dateTimeOffset.DateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                }
                if (res.currency != null)
                {

                    if (_currencyMapping.ContainsKey(res.currency))
                    {
                        double value =  _currencyMapping[res.currency].Item3;
                        if (!_currencyMapping[res.currency].Item2)
                        {
                            value = 1d / value;
                        }
                        res.currency = _currencyMapping[res.currency].Item1;
                        res.ask *= value;
                        res.bid *= value;
                        res.regularMarketChange *= value;
                        res.regularMarketDayHigh *= value;
                        res.regularMarketPrice *= value;
                        res.regularMarketDayLow *= value;
                        res.regularMarketOpen *= value;
                        res.regularMarketPreviousClose *= value;
                    }
                }
                else
                {
                    Logger.LogMessage("Currency not found for symbol: " + res.symbol);
                }
            }
            catch (Exception Ex)
            {
                Logger.LogError(Ex);
            }
        }

        /// <summary>
        /// Creates the data table.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <returns></returns>
        private DataTable CreateDataTable(Dictionary<string, MarketDataParamter> dict)
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


        /// <summary>
        /// Validates the symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns></returns>
        public Dictionary<string, string> ValidateSymbol(string symbol, string asset)
        {
            try
            {
                string providerSymbol = GetproviderSymbol(symbol, asset);
                var json = new WebClient().DownloadString(URL + providerSymbol);
                List<MarketDataParamter> data = JsonConvert.DeserializeObject<MarketDataList>(json).quoteResponse.result;
                if (data != null && data.Count > 0)
                {
                    Dictionary<string, string> output = new Dictionary<string, string>();

                    output["Symbol"] = symbol;
                    for (int i = 0; i < properties.Length; i++)
                    {
                        PropertyInfo inf = properties[i];
                        string propName = inf.Name;
                        if(_fieldMapping.ContainsKey(propName))
                            propName = _fieldMapping[propName];
                        if (!propName.Equals("Symbol"))
                        {
                            Object x = inf.GetValue(data[0]);
                            if (x != null)
                                output[propName] = x.ToString();
                        }
                    }
                    return output;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return null;
            
        }
    }
}
