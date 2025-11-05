using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;
using System.Reflection;
using BusinessObjects;
using System.Collections;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace LiveFeedUtility
{
    /// <summary>
    /// Main Application UI
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class ConsoleUI : Form
    {


        #region Fields and Properties
        /// <summary>
        /// The third party FTP
        /// </summary>
        private static ThirdPartyFtp _thirdPartyFtp = null;
        
        /// <summary>
        /// The file pricing timer(Responsible for fetching data from API and uploading to FTP)
        /// </summary>
        private static System.Timers.Timer filePricingTimer = new System.Timers.Timer();

        /// <summary>
        /// The live prices file read interval
        /// </summary>
        private static long _livePricesFileReadInterval = Convert.ToInt64(ConfigurationManager.AppSettings["LivePricesFileReadInterval"]);
        
        /// <summary>
        /// The pricing folder
        /// </summary>
        private static string _pricingFolder = Application.StartupPath + "\\" + ConfigurationManager.AppSettings["PricingFolder"] + "\\";

        private static bool _isBBGSymbology = Convert.ToBoolean(ConfigurationManager.AppSettings["IsBBGSymbology"]);

        private static string _sftpFolder = "/" + ConfigurationManager.AppSettings["PricingFolder"] + "/";
        /// <summary>
        /// The add on symbols
        /// </summary>
        private static NameValueCollection addOnSymbols = ((NameValueCollection)ConfigurationManager.GetSection("AddOnSymbols"));

        private static ArrayList _messagesList = new ArrayList();

        private static readonly object _locker = new object();

        private static DataTable _manualInputTable = null;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleUI"/> class.
        /// </summary>
        public ConsoleUI()
        {
            InitializeComponent();
        }

        #region methods
        /// <summary>
        /// Handles the Click event of the StartButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.messageLogged+=Logger_MessageLogged;

                Logger.LogMessage("Initialization Started");
                HostService();
                _thirdPartyFtp = ThirdPartyFtp.GetFtpFromConfig();
                MarketDataProviderManager.LoadMarketDataProviderPreferences();
                LiveFeedProviderFactory.LoadProviderDetails();

                //Create pricing folder if doesn't already exist
                if (!Directory.Exists(_pricingFolder))
                    Directory.CreateDirectory(_pricingFolder);
                else
                {
                    FileInfo[] files = new DirectoryInfo(_pricingFolder).GetFiles();
                    foreach (FileInfo file in files)
                    {
                        if (file.LastWriteTime.Date < DateTime.Now.Date)
                            file.Delete();
                    }
                }
                if (File.Exists("ManualOverride.csv"))
                {
                    _manualInputTable = CsvHelper.GetDataTableFromCSVWithHeader("ManualOverride.csv");

                    DataColumn dc = _manualInputTable.Columns[0];
                    _manualInputTable.PrimaryKey = new DataColumn[] { dc };
                }


                #region Start File Pricing Timer
                filePricingTimer.Interval = _livePricesFileReadInterval;
                filePricingTimer.Elapsed += new System.Timers.ElapsedEventHandler(filePricingTimer_TimerTickHandler);
                filePricingTimer.Start();
                #endregion
                
                filePricingTimer_TimerTickHandler(null, null);
                Logger.LogMessage("Initialization Successfull");
                StartButton.Enabled = false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        delegate void TestDelegate();
        ServiceHost host = null;
        private void HostService()
        {

            //Create ServiceHost
            host = new ServiceHost(typeof(MarketDataService));

            //Start the Service
            host.Open();
        }

        /// <summary>
        /// Loggers the data manager connected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void Logger_MessageLogged(object sender, string e)
        {
            try
            {
                _messagesList.Insert(0, e);
                if (messageList.IsHandleCreated)
                {
                    if (messageList.InvokeRequired)
                    {
                        TestDelegate mi = delegate()
                        {
                            messageList.DataSource = null;
                            messageList.DataSource = _messagesList;
                        };
                        this.BeginInvoke(mi);
                    }
                    else
                    {
                        messageList.DataSource = null;
                        messageList.DataSource = _messagesList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Handles the KeyDown event of the messageList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void messageList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control == true && e.KeyCode == Keys.C && messageList.SelectedItem != null)
                {
                    string s = messageList.SelectedItem.ToString();
                    Clipboard.SetData(DataFormats.StringFormat, s);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        /// <summary>
        /// Handles the TimerTickHandler event of the filePricingTimer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void filePricingTimer_TimerTickHandler(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Logger.LogMessage("Request response process started");

                DataTable openSymbolTable = DataManager.GetOpenSymbolsData();

                //Adding currency and exchange from config
                foreach (string symbol in addOnSymbols.Keys)
                {
                    string asset = addOnSymbols[symbol];
                    DataRow row = openSymbolTable.NewRow();
                    row["Symbol"] = symbol;
                    row["BloombergSymbol"] = symbol;
                    row["Asset"] = asset;
                    openSymbolTable.Rows.Add(row);
                }
                DataTable openSymbolInfoWithDataProvider = MarketDataProviderManager.MapDataProviderWithSymbolInfo(openSymbolTable);
                CsvHelper.DataTableToCSV(openSymbolInfoWithDataProvider, Path.Combine(Environment.CurrentDirectory, "OpenSymbolsWithProvidersLog.csv"));

                DataTable liveFeedDataTable = LiveFeedProviderFactory.GetLiveFeedData(openSymbolInfoWithDataProvider);

                //Replace ticker with Bloomberg Symbol if BBg symbolgy is used(Dumping ticker if BBG symbol is blank)
                if (_isBBGSymbology)
                {
                    Dictionary<string, string> tickerBBGMapping = new Dictionary<string, string>();
                    foreach (DataRow row in openSymbolTable.Rows)
                    {
                        tickerBBGMapping[row["Symbol"].ToString()] = row["BloombergSymbol"].ToString();
                    }
                    foreach (DataRow row in liveFeedDataTable.Rows)
                    {
                        string ticker = row["Symbol"].ToString();
                        if(tickerBBGMapping.ContainsKey(ticker) && !string.IsNullOrWhiteSpace(tickerBBGMapping[ticker]))
                            row["Symbol"] = tickerBBGMapping[ticker];
                    }
                }
                if (_manualInputTable != null)
                {
                    DataColumn dc = liveFeedDataTable.Columns[0];
                    liveFeedDataTable.PrimaryKey = new DataColumn[] { dc };
                    liveFeedDataTable.Merge(_manualInputTable, false, MissingSchemaAction.Ignore);
                }
                SendPricestoFTP(sender, liveFeedDataTable);
                Logger.LogMessage("Request Response process complete");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        private void SendPricestoFTP(object sender, DataTable dt)
        {
            try
            {

                if (dt != null && dt.Rows.Count > 0)
                {
                    Logger.LogMessage("Data fetched from API");

                    string path = string.Empty;
                    // Pricing server picks Prices_ file on startup so creating prices_ file based on null check which is passed in first call
                    if (sender == null)
                    {
                        path = "Prices_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv";
                    }
                    else
                    {
                        path = "LivePrices_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".csv";
                    }
                    CsvHelper.DataTableToCSV(dt, _pricingFolder + path);
                    NirvanaWinSCPUtility util = new NirvanaWinSCPUtility(_thirdPartyFtp);
                    util.DeleteFiles(_sftpFolder + "*.csv");
                    Logger.LogMessage(util.SendFile(_pricingFolder + path, _sftpFolder + path));
                    Logger.LogMessage("File written successfully.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }
        
        #endregion
    }
}
