using BusinessObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
namespace FinancialModelingAdpater
{
    public class FinancialModelingProvider : ILiveFeedProvider
    {
       /// <summary>
        /// The URL
        /// </summary>
        private const string URLSTART = "https://financialmodelingprep.com/api/v3/quote/";

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

         //<summary>
         //The currency mapping
         //</summary>
        private Dictionary<string, Tuple<bool, double>> _currencyConversionMapping = new Dictionary<string, Tuple<bool, double>>();

        /// <summary>
        /// Exclude Symbols From currency mapping
        /// </summary>
        private string[] _excludeSymbolsFromConversion;

        /// <summary>
        /// The exchange mapping
        /// </summary>
        private Dictionary<string, string> _exchangeMapping = new Dictionary<string, string>();

        /// <summary>
        /// The symbol mapping
        /// </summary>
        private Dictionary<string, string> _symbolMapping = new Dictionary<string, string>();

        private Dictionary<string, string> _symbolCurrencyMapping = new Dictionary<string, string>();

        private static int BATCH_SYMBOL_REQUEST_LIMIT;

        /// <summary>
        /// The API Key
        /// </summary>
        private static string _APIKEY;

        /// <summary>
        /// The properties
        /// </summary>
        private PropertyInfo[] properties;


        public FinancialModelingProvider()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialModelingProvider"/> class.
        /// </summary>
        private void Initialize()
        {
            #region Configs
            try
            {
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
                        if (values.Length == 2)
                        {
                            //string newCurr = values[0];
                            bool isMul = values[0].Equals("M");
                            double num = Convert.ToDouble(values[1]);

                            _currencyConversionMapping.Add(element.Key, new Tuple<bool, double>(isMul, num));
                        }
                    }
                }


                AppSettingsSection apiKey = (AppSettingsSection)config.GetSection("APIDetails");
                SetApiDetails(apiKey);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

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
            DataColumn currencyCol = new DataColumn("CURENCYCODE", typeof(System.String));
            resultDataTable.Columns.Add(currencyCol);
            DataColumn pricingSourceCol = new DataColumn("PRICINGSOURCE", typeof(System.String));
            pricingSourceCol.DefaultValue = "FMP";
            resultDataTable.Columns.Add(pricingSourceCol);
        }

        /// <summary>
        /// Process API Key from config
        /// </summary>
        /// <param name="apiKey"></param>
        private void SetApiDetails(AppSettingsSection apiKey)
        {
            try
            {
                foreach (KeyValueConfigurationElement element in apiKey.Settings)
                {
                    if (element.Key.Equals("FMPAPI"))
                        _APIKEY = element.Value;
                    else if (element.Key.Equals("ExcludeSymbolsFromCurrencyConversion"))
                        _excludeSymbolsFromConversion = element.Value.Split(',');
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
                bool currencyColExists = false;
                if (openSymbols.Columns.Contains("Currency"))
                {
                    currencyColExists = true;
                }
                foreach (DataRow symbolRow in openSymbols.Rows)
                {

                    string symbol = symbolRow["symbol"].ToString();
                    string asset = symbolRow["Asset"].ToString();
                    string currency = string.Empty;
                    if (currencyColExists)
                    {
                        currency = symbolRow["Currency"].ToString();
                    }
                    try
                    {
                        if (!mappedSymbols.ContainsKey(symbol))
                        {
                            if (replaceMapping.ContainsKey(symbol))
                                mappedSymbols.Add(symbol, replaceMapping[symbol]);
                            else
                            {
                                foreach (string s in _exchangeMapping.Keys)
                                {
                                    if (symbol.Contains(s))
                                    {
                                        mappedSymbols.Add(symbol, symbol.Replace(s, _exchangeMapping[s]));
                                    }
                                    else
                                        continue;
                                }
                            }

                            // Important to map "/ and \" for the below symbols
                            if (!mappedSymbols.ContainsKey(symbol))
                            {
                                switch (asset)
                                {
                                    case "FXForward":
                                        {
                                            string[] arr = symbol.Split(' ');
                                            if (arr.Length > 1)
                                                mappedSymbols.Add(symbol, arr[0].Replace("/", ""));
                                            break;
                                        }
                                    case "FX":
                                        {
                                            if (symbol.Contains("/"))
                                                mappedSymbols.Add(symbol, symbol.Replace("/", ""));
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
                                                        mappedSymbols.Add(symbol, curr + "USD");
                                                        break;
                                                    default:
                                                        mappedSymbols.Add(symbol, "USD" + curr);
                                                        break;
                                                }
                                            }
                                            else
                                                mappedSymbols.Add(symbol, symbol);
                                            break;
                                        }
                                    default:
                                        mappedSymbols.Add(symbol, symbol);
                                        break;
                                }
                            }
                        }
                        if (!_symbolCurrencyMapping.ContainsKey(symbol))
                        {
                            _symbolCurrencyMapping.Add(symbol, currency);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogWarning("Error Occured for Symbol: " + symbol + " " + ex.Message);
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
        public DataTable GetLiveFeedData(Dictionary<string, string> openSymbols)
        {
            Dictionary<string, MarketData> results = new Dictionary<string, MarketData>();
            string URLEND = GetURLEnd();
            try
            {
                List<MarketData> symbolData = new List<MarketData>();
                StringBuilder symbolsToRequestInBatch = new StringBuilder();
                int count = 0;
                foreach (var symbol in openSymbols)
                {
                    string reqSymbol = symbol.Value;
                    if (Regex.IsMatch(reqSymbol, @"[/\\]"))
                    {
                        reqSymbol = Regex.Replace(reqSymbol, @"[/\\]", "-");
                    }
                    //Check if the symbol is last in the openSymbols dicitonary or count has reached BATCH_SYMBOL_REQUEST_LIMIT
                    if (openSymbols[symbol.Key].Equals(openSymbols.Last().Value) || count >= BATCH_SYMBOL_REQUEST_LIMIT)
                    {
                        symbolsToRequestInBatch.AppendFormat(reqSymbol);
                        try
                        {
                            string fmpSymbolRequestURL = URLSTART + symbolsToRequestInBatch + URLEND;
                            Logger.LogMessage("Sending request of Symbols to FMP - " + Environment.NewLine + symbolsToRequestInBatch + Environment.NewLine + " On FMP URL : " + Environment.NewLine + fmpSymbolRequestURL);

                            symbolData.AddRange(JsonConvert.DeserializeObject<List<MarketData>>(new WebClient().DownloadString(fmpSymbolRequestURL)));
                            Logger.LogMessage("Reponse received for FMP. ");
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex);
                        }
                        symbolsToRequestInBatch.Clear();
                        count = 0;
                    }
                    else
                        symbolsToRequestInBatch.AppendFormat(reqSymbol + ",");
                    count++;
                }
                if (symbolData.Select(value => value.symbol) == null || symbolData.Select(value => value.symbol).Equals(""))
                    Logger.LogMessage("Symbol is not present for : " + symbolData.Select(value => value.symbol) + " while fetching from FMP");
                else
                {
                    foreach (var data in symbolData)
                    {
                        if (data.symbol.Contains("^"))
                            data.symbol = data.symbol.Replace("^", "%5E");
                        if (openSymbols.ContainsValue(data.symbol))
                        {
                            ResponseModification(data);
                            var keys = openSymbols.Where(x => x.Value == data.symbol).Select(x => x.Key);
                            foreach(string symKey in keys)
                                results[symKey] = data;
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

        private string GetURLEnd()
        {
            string URLEND = "?apikey=" + GetApiKey();
            return URLEND;
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
                if (res.exchange != null)
                {

                    if (_currencyConversionMapping.ContainsKey(res.exchange) && !_excludeSymbolsFromConversion.Contains(res.symbol))
                    {
                        double value = _currencyConversionMapping[res.exchange].Item2;
                        if (!_currencyConversionMapping[res.exchange].Item1)
                        {
                            value = 1d / value;
                        }
                        //res.currency = _currencyMapping[res.currency].Item1;
                        res.ask *= value;
                        res.bid *= value;
                        res.previousClose *= value;
                        res.price *= value;
                        res.open *= value;
                        res.dayHigh *= value;
                        res.dayLow *= value;
                        res.change *= value;
                    }
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
                foreach (DataRow row in dt.Rows)
                {
                    if (_symbolCurrencyMapping.ContainsKey(row.Field<string>("Symbol")))
                    {
                        row.SetField("CURENCYCODE", _symbolCurrencyMapping[row.Field<string>("Symbol")]);
                    }

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
