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

namespace BarchartAdapter
{
    public class BarchartProvider : ILiveFeedProvider
    {
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
        /// The mapped symbols
        /// </summary>
        private Dictionary<string, string> _mappedSymbols = new Dictionary<string, string>();

        /// <summary>
        /// The API Key
        /// </summary>
        private static string _APIKEY;

        /// <summary>
        /// The properties
        /// </summary>
        private PropertyInfo[] properties;

        public BarchartProvider()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BarchartProvider"/> class.
        /// </summary>
        private void Initialize()
        {
            #region Configs
            //Read Barchart specific config file
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
            pricingSourceCol.DefaultValue = "Barchart";
            resultDataTable.Columns.Add(pricingSourceCol);
        }

        /// <summary>
        /// Process API Key from config
        /// </summary>
        /// <param name="apiKey"></param>
        private void SetApiKey(AppSettingsSection apiKey)
        {
            string key = null;
            try
            {
                foreach (KeyValueConfigurationElement element in apiKey.Settings)
                    key = element.Value;
                _APIKEY = key;
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
            try
            {
                Dictionary<string, string> replaceMapping = new Dictionary<string, string>();
                foreach (string s in _symbolMapping.Keys)
                    replaceMapping.Add(s, _symbolMapping[s]);

                foreach (DataRow symbolRow in openSymbols.Rows)
                {
                    string symbol = symbolRow["symbol"].ToString();
                    string asset = symbolRow["Asset"].ToString();
                    if (!_mappedSymbols.ContainsKey(symbol))
                    {
                        if (replaceMapping.ContainsKey(symbol))
                            _mappedSymbols.Add(symbol, replaceMapping[symbol]);
                        else
                        {
                            foreach (string s in _exchangeMapping.Keys)
                            {
                                if (symbol.EndsWith(s))
                                    _mappedSymbols.Add(symbol, symbol.Replace(s, _exchangeMapping[s]));
                            }
                        }
                        if (!_mappedSymbols.ContainsKey(symbol))
                        {
                            switch (asset)
                            {
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
                                            int strPrice = (int)(Convert.ToDouble(temp.Substring(0, temp.IndexOf('D'))) * 1000);
                                            string ysym = undsym + arr[1].Substring(0, 2) + mon + arr[1].Substring(arr[1].LastIndexOf('D') + 1) + cp + strPrice.ToString("D8");
                                            _mappedSymbols.Add(symbol, ysym);
                                            if (!_mappedSymbols.ContainsKey(undsym))
                                                _mappedSymbols.Add(undsym, undsym);
                                        }
                                    }
                                    break;
                                case "Future":
                                    string sym = symbol.Replace(" ", string.Empty) + ".CME";
                                    _mappedSymbols.Add(symbol, sym);
                                    break;
                                default:
                                    _mappedSymbols.Add(symbol, symbol);
                                    break;
                            }
                        }
                    }
                }
                return _mappedSymbols;
            }
            catch (Exception)
            {
                return _mappedSymbols;
            }
        }

        /// <summary>
        /// Gets the live feed data.
        /// </summary>
        /// <param name="openSymbols">The open symbols.</param>
        /// <returns></returns>
        private DataTable GetLiveFeedData(Dictionary<string, string> openSymbols)
        {
            Dictionary<string, MarketData> results = new Dictionary<string, MarketData>();
            foreach (KeyValuePair<string, string> kvp in openSymbols)
            {
                try
                {
                    string json = ConvertCsvFileToJsonObject(kvp.Value);
                    json = json.Trim('[', ']');
                    var ab = JsonConvert.DeserializeObject<MarketData>(json);

                    if (ab == null)
                    {
                        MarketData res = new MarketData();
                        res.symbol = null;
                        res.exchange = null;
                        res.name = null;
                        res.dayCode = null;
                        res.serverTimestamp = null;
                        res.mode = null;
                        res.lastPrice = null;
                        res.tradeTimestamp = null;
                        res.netChange = null;
                        res.percentChange = null;
                        res.unitCode = null;
                        res.open = null;
                        res.high = null;
                        res.low = null;
                        res.close = null;
                        res.flag = null;
                        res.volume = null;

                        //ResponseModification(res);
                        results.Add(kvp.Key, res);
                    }
                    else
                    {
                        MarketData res = ab;
                        //ResponseModification(res);
                        results.Add(kvp.Key, res);
                    }
                }
                catch (Exception)
                {
                    MarketData res = new MarketData();
                    res.symbol = null;
                    res.exchange = null;
                    res.name = null;
                    res.dayCode = null;
                    res.serverTimestamp = null;
                    res.mode = null;
                    res.lastPrice = null;
                    res.tradeTimestamp = null;
                    res.netChange = null;
                    res.percentChange = null;
                    res.unitCode = null;
                    res.open = null;
                    res.high = null;
                    res.low = null;
                    res.close = null;
                    res.flag = null;
                    res.volume = null;

                    //ResponseModification(res);
                    results.Add(kvp.Key, res);
                }
            }
            DataTable dt = CreateDataTable(results);
            return dt;
        }

        private string GetURL()
        {
            string URL = "https://marketdata.websol.barchart.com/getQuote.csv?apikey=" + GetApiKey() + "&symbols=";
            return URL;
        }

        /// <summary>
        /// Responses the modification.
        /// </summary>
        /// <param name="res">The resource.</param>
        private void ResponseModification(MarketData res)
        {
            //if (_currencyMapping.ContainsKey(res.currency))
            //{
            //    double value = _currencyMapping[res.currency].Item3;
            //    if (!_currencyMapping[res.currency].Item2)
            //    {
            //        value = 1d / value;
            //    }
            //    res.currency = _currencyMapping[res.currency].Item1;
            //    res.ask *= value;
            //    res.bid *= value;
            //    res.regularMarketChange *= value;
            //    res.regularMarketDayHigh *= value;
            //    res.regularMarketPrice *= value;
            //    res.regularMarketDayLow *= value;
            //    res.regularMarketOpen *= value;
            //    res.regularMarketPreviousClose *= value;
            //} 
        }

        /// <summary>
        /// Creates the data table.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        /// <returns></returns>
        private DataTable CreateDataTable(Dictionary<string, MarketData> dict)
        {
            try
            {
                resultDataTable.Rows.Clear();

                foreach (string key in dict.Keys)
                {
                    object[] values = new object[properties.Length + 1];
                    values[0] = key;
                    for (int i = 0; i < properties.Length; i++)
                    {
                        PropertyInfo inf = properties[i];
                        values[i + 1] = properties[i].GetValue(dict[key]);
                    }

                    resultDataTable.Rows.Add(values);
                }

                return resultDataTable;
            }
            catch (Exception)
            {
                return resultDataTable;
            }
        }

        /// <summary>
        /// Converts the CSV file to json object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string ConvertCsvFileToJsonObject(string value)
        {
            var csv = new List<string[]>();
            string URL = GetURL();
            var lines = new WebClient().DownloadString(URL + value).Split('\n');

            foreach (string line in lines)
                csv.Add(line.Split(','));

            var properties = lines[0].Split(',');

            var listObjResult = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Length - 1; i++)
            {
                var objResult = new Dictionary<string, string>();
                for (int j = 0; j < properties.Length; j++)
                    objResult.Add(properties[j], csv[i][j]);

                listObjResult.Add(objResult);
            }

            return JsonConvert.SerializeObject(listObjResult);
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
