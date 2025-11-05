using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ActivFinancial.ContentPlatform.ContentGatewayApi;
using ActivFinancial.Middleware.Application;
using ActivFinancial.Middleware.Service;
using ActivFinancial.Samples.Common.BusObj;
using System.Threading;
using System.Reflection;

namespace ActivAdapter
{
    public class ActivProvider : ILiveFeedProvider
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
        //private Dictionary<string, Tuple<string, bool, double>> _currencyMapping = new Dictionary<string, Tuple<string, bool, double>>();

        /// <summary>
        /// The exchange mapping
        /// </summary>
        //private Dictionary<string, string> _exchangeMapping = new Dictionary<string, string>();

        /// <summary>
        /// The symbol mapping
        /// </summary>
        private Dictionary<string, string> _symbolMapping = new Dictionary<string, string>();

        /// <summary>
        /// The mapped symbols
        /// </summary>
        private Dictionary<string, string> _mappedSymbols = new Dictionary<string, string>();

        /// <summary>
        /// Activ Credentials
        /// </summary>
        private Dictionary<string, string> _credentials = new Dictionary<string, string>();

        public static Dictionary<string, string> result = new Dictionary<string, string>();

        public ActivProvider()
        {
            Initialize();
        }

        private void Initialize()
        {
            #region Configs
            //Read yahoo specific config file
            config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);

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

            //Process Activ Credentials from config
            AppSettingsSection credentials = (AppSettingsSection)config.GetSection("APIDetails");
            foreach (KeyValueConfigurationElement element in credentials.Settings)
                _credentials.Add(element.Key, element.Value);

            //Process Currency to Update from config
            //AppSettingsSection currencySection = (AppSettingsSection)config.GetSection("CurrenciesToUpdate");
            //foreach (KeyValueConfigurationElement element in exchangeSection.Settings)
            //{
            //    if (element != null)
            //    {
            //        string[] values = element.Value.Split(',');
            //        if (values.Length == 3)
            //        {
            //            string newCurr = values[0];
            //            bool isMul = values[1].Equals("M");
            //            double num = Convert.ToDouble(values[2]);

            //            _currencyMapping.Add(element.Key, new Tuple<string, bool, double>(newCurr, isMul, num));
            //        }
            //    }
            //}

            //Process Exchange Mapping from config
            //AppSettingsSection exchangeSection = (AppSettingsSection)config.GetSection("ExchangeMapping");
            //foreach (KeyValueConfigurationElement element in exchangeSection.Settings)
            //{
            //    if (element != null)
            //        _exchangeMapping.Add(element.Key, element.Value);
            //}

            #endregion
        }

        public Dictionary<string, string> GetMappedSymbolsForProvider(DataTable openSymbols)
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

                    //Uncomment below code when Exchange Mapping gets added for ACTIV API in App.Config
                    //{
                    //    foreach (string s in _exchangeMapping.Keys)
                    //    {
                    //        if (symbol.EndsWith(s))
                    //            _mappedSymbols.Add(symbol, symbol.Replace(s, _exchangeMapping[s]));
                    //    }
                    //}

                    if (!_mappedSymbols.ContainsKey(symbol))
                    {
                        switch (asset)
                        {
                            case "EquityOption":
                                //Make Changes As Per the Equity Options Symbology in ACTIV API
                                break;
                            case "Future":
                                //Make Changes As Per the Future Symbology in ACTIV API
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

        private DataTable GetLiveFeedData(Dictionary<string, string> symbols)
        {
            // construct application settings
            Settings setting = new Settings();

            // construct new application.
            ActivApplication application = new ActivApplication(setting);

            // start a thread to run the application. Async callbacks will be processed by this thread
            application.StartThread();

            ContentGatewayClient client = new ActivConnectionAndDataProvider(application, symbols, _credentials);

            DataTable dt = new DataTable();
            while (true)
            {
                if (_symbolData.Count == 0)
                    Thread.Sleep(5000);
                else
                {
                    dt = CreateDataTable(_symbolData);
                    return dt;
                }
            }   
        }

        private DataTable CreateDataTable(Dictionary<string, Dictionary<string, string>> dic)
        {
            if (resultDataTable == null)
            {
                resultDataTable = new DataTable();
                resultDataTable.Columns.Add(new DataColumn("Ticker"));
                foreach (var symbol in dic)
                {
                    foreach (var fields in symbol.Value)
                    {
                        if (resultDataTable.Columns.Contains(fields.Key))
                            break;
                        if (_fieldMapping.ContainsKey(fields.Key))
                            resultDataTable.Columns.Add(new DataColumn(_fieldMapping[fields.Key].Trim()));
                        else
                            resultDataTable.Columns.Add(new DataColumn(fields.Key));
                    }
                }
                DataColumn pricingSourceCol = new DataColumn("PRICINGSOURCE", typeof(System.String));
                pricingSourceCol.DefaultValue = "Activ";
                resultDataTable.Columns.Add(pricingSourceCol);
            }
            else
            {
                resultDataTable.Rows.Clear();
            }

            DataRow workRow;
            int i = 1;

            foreach (var v in dic)
            {
                string t = "";
                workRow = resultDataTable.NewRow();
                if (v.Key.Contains(". [0]"))
                    t = v.Key.Replace(". [0]", "");
                workRow["Ticker"] = t;
                foreach (var c in v.Value)
                {
                    while (i < resultDataTable.Columns.Count - 1)
                    {
                        string temp = "";
                        if (resultDataTable.Columns[i].ColumnName.ToString() == c.Key || resultDataTable.Columns[i].ColumnName.ToString() == _fieldMapping[c.Key])
                        {
                            temp = c.Value;
                            if (c.Value.Contains("<+  >"))
                                temp = c.Value.Replace("<+  >", "");
                            if (c.Value.Contains("<-  >"))
                                temp = c.Value.Replace("<-  >", "");
                            workRow[i] = temp;
                            break;
                        }
                    }
                    i++;
                }
                i = 1;
                resultDataTable.Rows.Add(workRow);
            }
            return resultDataTable;
        }

        private static Dictionary<string, Dictionary<string, string>> _symbolData = new Dictionary<string, Dictionary<string, string>>();

        //Callback method receives symbol data from ActiveConnectionAndDataProvider class
        internal static void SetDic(Dictionary<string, Dictionary<string, string>> symbolData)
        {
            _symbolData = symbolData;
        }

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
