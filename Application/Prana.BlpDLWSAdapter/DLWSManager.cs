using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Prana.BlpDLWSAdapter.BusinessObject.Mappings;
using Prana.BlpDLWSAdapter.PerSecurityWSDL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
//using Prana.CommonDataCache;
using Prana.Global;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Prana.CentralSMDataCache;
using Prana.Utilities.MiscUtilities;
using Prana.BlpDLWSAdapter.BLL;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Prana.Utilities.XMLUtilities;

namespace Prana.BlpDLWSAdapter
{
    public sealed class DLWSManager : IDisposable
    {
        PerSecurityWSDL.PerSecurityWS _perSecurityRef;

        #region singleton
        private static volatile DLWSManager instance;
        private static object syncRoot = new Object();
        private static object _logLocker = new object();

        public static DLWSManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new DLWSManager();
                    }
                }

                return instance;
            }
        }
        #endregion

        private DLWSManager()
        {
            try
            {
                Connect();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //static ConcurrentDictionary<string, SymbolDataInfo> _assetMapping = new ConcurrentDictionary<string, SymbolDataInfo>();


        //private void AddAssetMapping(string[] SecType, AssetCategory asset)
        //{
        //    try
        //    {
        //        foreach (string sectyp in SecType)
        //        {
        //            switch (asset)
        //            {
        //                case AssetCategory.Equity:
        //                case AssetCategory.PrivateEquity:
        //                    _assetMapping.TryAdd(sectyp, new SymbolDataInfo(asset, typeof(EquitySymbolData)));
        //                    break;
        //                case AssetCategory.EquityOption:
        //                case AssetCategory.Option:
        //                case AssetCategory.FXOption:
        //                    _assetMapping.TryAdd(sectyp, new SymbolDataInfo(asset, typeof(OptionSymbolData)));
        //                    break;
        //                case AssetCategory.FixedIncome:
        //                    _assetMapping.TryAdd(sectyp, new SymbolDataInfo(asset, typeof(FixedIncomeSymbolData)));
        //                    break;
        //                case AssetCategory.Forex:
        //                case AssetCategory.FX:
        //                    _assetMapping.TryAdd(sectyp, new SymbolDataInfo(asset, typeof(FxContractSymbolData)));
        //                    break;
        //                case AssetCategory.Future:
        //                    _assetMapping.TryAdd(sectyp, new SymbolDataInfo(asset, typeof(FutureSymbolData)));
        //                    break;
        //                case AssetCategory.FXForward:
        //                    _assetMapping.TryAdd(sectyp, new SymbolDataInfo(asset, typeof(FxForwardContractSymbolData)));
        //                    break;
        //                case AssetCategory.FutureOption:
        //                    _assetMapping.TryAdd(sectyp, new SymbolDataInfo(asset, typeof(FutureOptionSymbolData)));
        //                    break;
        //                case AssetCategory.Indices:
        //                    _assetMapping.TryAdd(sectyp, new SymbolDataInfo(asset, typeof(IndexSymbolData)));
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}


        ///// <summary>
        ///// Fills the asset and bloomberg security type mapping dictionary with static data. TODO:PKE Move the mapping to some xml or DB where it can be changed and loaded in runtime.
        ///// </summary>
        //private void FillAssetMapping()
        //{
        //    try
        //    {
        //        #region equity

        //        AddAssetMapping(new string[] {  "Common Stock", 
        //                                        "Preferred",
        //                                        "Preference",
        //                                        "PRIVATE",
        //                                        "PUBLIC",
        //                                        "Equity",
        //                                        "Pfd"}, AssetCategory.Equity);//default for yellow key equity and pfd

        //        AddAssetMapping(new string[] {  "Pvt Eqty Fund",
        //                                        "Private Eqty",
        //                                        "Private Comp" }, AssetCategory.PrivateEquity);

        //        #endregion
        //        #region option

        //        AddAssetMapping(new string[] {  "Equity Option",
        //                                        "Equity OTC Option" }, AssetCategory.EquityOption);

        //        AddAssetMapping(new string[] {  "Index Option", 
        //                                        "Index OTC Option",
        //                                        "Financial index option.",
        //                                        "Physical index option."}, AssetCategory.Option);

        //        #endregion

        //        #region futures
        //        AddAssetMapping(new string[] {  "Financial index future.", 
        //                                        "Index Future", 
        //                                        "Physical index future.",
        //                                        "Financial commodity future.",
        //                                        "Physical commodity forward.",
        //                                        "Physical commodity future.",
        //                                        "Financial commodity spot.",
        //                                        "Physical commodity spot.",
        //                                        "CONTRACT FOR DIFFERENCE",
        //                                        "DIVIDEND NEUTRAL STOCK FUTURE",
        //                                        "SINGLE STOCK FORWARD",
        //                                        "SINGLE STOCK FUTURE",
        //                                        "Equity Comdty",
        //                                        "Comdty",
        //                                        "Curncy"}, AssetCategory.Future);//default for Equity Comdty, comdty, curncy

        //        #endregion

        //        AddAssetMapping(new string[] {  "Equity Index",
        //                                        "Spot index.",
        //                                        "Non-Equity Index",
        //                                        "Fixed Income Index",
        //                                        "Commodity Index",
        //                                        "Index"}, AssetCategory.Indices);//default for yellowkey index

        //        AddAssetMapping(new string[] {  "Physical commodity option.",
        //                                        "Calendar Spread Option",
        //                                        "Financial commodity option."}, AssetCategory.FutureOption);

        //        AddAssetMapping(new string[] {  "CROSS", 
        //                                        "SPOT",
        //                                        "Currency spot."}, AssetCategory.FX);

        //        AddAssetMapping(new string[] {  "FORWARD", 
        //                                        "Currency future.",
        //                                        "FUTURE",
        //                                        "FORWARD CROSS"}, AssetCategory.FXForward);

        //        AddAssetMapping(new string[] { "Currency option." }, AssetCategory.FXOption);

        //        AddAssetMapping(new string[] {  "BANK BILL",
        //                                        "COMMERCIAL PAPER",
        //                                        "Bond" }, AssetCategory.FixedIncome);
        //        //default fixed income
        //        AddAssetMapping(SecurityInfo.FixedIncome, AssetCategory.FixedIncome);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// pending initial data requests for which response not received from Bloomberg
        /// </summary>
        ConcurrentDictionary<string, SubmitGetDataRequest> _pendingReqRespForInitSecurityInfo = new ConcurrentDictionary<string, SubmitGetDataRequest>();

        /// <summary>
        /// pending additional data requests for which response not received from Bloomberg
        /// </summary>
        ConcurrentDictionary<string, Tuple<string, SecMasterBaseObj, SubmitGetDataRequest>> _pendingReqRespForAdditionalSymbolData = new ConcurrentDictionary<string, Tuple<string, SecMasterBaseObj, SubmitGetDataRequest>>();

        /// <summary>
        /// pending historical data requests for which response not received from Bloomberg, responseID,Request
        /// </summary>
        ConcurrentDictionary<string, SubmitGetHistoryRequest> _pendingReqRespForHistMarkPrice = new ConcurrentDictionary<string, SubmitGetHistoryRequest>();

        /// <summary>
        /// pending historical data requests for which response not recieved from Bloomberg, responseID,Request
        /// </summary>
        ConcurrentDictionary<string, SubmitGetDataRequest> _pendingReqRespForCurMarkPrice = new ConcurrentDictionary<string, SubmitGetDataRequest>();

        /// <summary>
        /// keeps a mapping of all the requests stored in _pendingReqRespForHistMarkPrice with the original Requests received from the client. Eg. responseID,RequestIDFromClient
        /// </summary>
        ConcurrentDictionary<string, string> _mappingReqIDRespForHistMarkPrice = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// requestid, symbolpks, secondary pricingsource
        /// </summary>
        ConcurrentDictionary<string, Tuple<string[], string>> _mappingRespIDSymbolPkForHistMarkPrice = new ConcurrentDictionary<string, Tuple<string[], string>>();

        /// <summary>
        /// Symbols for which the requests to BB are in process, InstrumentType,Hashset of symbols
        /// </summary>
        ConcurrentDictionary<InstrumentType, HashSet<string>> _symbolsRequestedToBB = new ConcurrentDictionary<InstrumentType, HashSet<string>>();
        /// <summary>
        /// symbol,BBGID pair for requests so that we do not make multiple requests for same symbol
        /// </summary>
        ConcurrentDictionary<String, String> _symbolsRequestedToBBForAdditionalData = new ConcurrentDictionary<string, string>();

        public const int DATA_NOT_AVAILABLE = 100;
        public const int SUCCESS = 0;
        public const int REQUEST_ERROR = 200;
        public const int POLL_INTERVAL = 5000;
        static bool _isResponseThreadForInitSecurityInfoRunning = false;
        static bool _isResponseThreadForAdditionalSymbolDataRunning = false;
        static bool _isResponseThreadForHistoricalMarkPrices = false;
        static bool _isResponseThreadForCurrentMarkPrices = false;

        /// <summary>
        /// TODO: Move the certificate part to appconfig in some way so that we easily specify and change the certificate location
        /// </summary>
        void Connect()
        {
            try
            {
                X509Certificate2 clientCert = null;
                try
                {
                    string certificatePath = ConfigurationManager.AppSettings["BloombergCertificatePath"];
                    if (!string.IsNullOrWhiteSpace(certificatePath))
                    {
                        String path = AppDomain.CurrentDomain.BaseDirectory;
                        certificatePath = path + certificatePath;
                        string certificatePassword = ConfigurationManager.AppSettings["BloombergCertificatePassword"];
                        clientCert = new X509Certificate2(certificatePath, certificatePassword);
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                if (clientCert != null)
                {
                    _perSecurityRef = new PerSecurityWS();
                    _perSecurityRef.ClientCertificates.Add(clientCert);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        ///// <summary>
        ///// Logs the data from Bloomberg in the trace logs. TODO: improve the logging so that it can be clearly inferred what was requested and what was returned
        ///// </summary>
        ///// <param name="fields"></param>
        ///// <param name="instrumentData"></param>
        //private void LogDataFromBB(string[] fields, InstrumentData instrumentData)
        //{
        //    try
        //    {
        //        Logger.Write("Bloomberg Data for :" + instrumentData.instrument.id + "  " + instrumentData.instrument.yellowkey + Environment.NewLine, ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
        //        for (int j = 0; j < instrumentData.data.Length; j++)
        //        {
        //            if (instrumentData.data[j].isArray == true)
        //            {
        //                //In case this is a bulk field request
        //                for (int k = 0; k < instrumentData.data[j].bulkarray.Length; k++)
        //                {
        //                    Logger.Write("-------------------------" + Environment.NewLine, ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
        //                    for (int l = 0; l < instrumentData.data[j].
        //                        bulkarray[k].data.Length; l++)
        //                        Logger.Write(instrumentData.data[j].bulkarray[k].data[l].value + Environment.NewLine, ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
        //                }
        //            }
        //            else
        //                Logger.Write("	" + fields[j] + " : " + instrumentData.data[j].value + Environment.NewLine, ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private void LogDataFromBB(string[] p, HistInstrumentData histInstrumentData)
        //{
        //    try
        //    {
        //        Logger.Write("Bloomberg Data for :" + histInstrumentData.instrument.id + "  " + histInstrumentData.instrument.yellowkey + Environment.NewLine, ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
        //        Logger.Write("Data for :" + histInstrumentData.instrument.id +
        //                    "  " + histInstrumentData.instrument.yellowkey + Environment.NewLine, ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
        //        Logger.Write(histInstrumentData.date.ToString() + Environment.NewLine, ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
        //        for (int j = 0; j < histInstrumentData.data.Length; j++)
        //        {
        //            Logger.Write(p[j] + " : " + histInstrumentData.data[j].value + Environment.NewLine, ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        /// <summary>
        /// requests for additional security data which is dependent on asset type
        /// </summary>
        /// <param name="symbData"></param>
        void SpawnResponseThreadForAdditionalSecurityInfo(SecMasterBaseObj secMasterBaseObj)
        {
            try
            {
                GetDataHeaders getDataHeaders = new GetDataHeaders();
                getDataHeaders.secmaster = true;
                getDataHeaders.secmasterSpecified = true;
                getDataHeaders.closingvalues = true;
                getDataHeaders.closingvaluesSpecified = true;
                getDataHeaders.derived = true;
                getDataHeaders.derivedSpecified = true;
                string bbgid = secMasterBaseObj.BBGID;
                //string ticker = symbData.Symbol;
                InstrumentType instTyp = InstrumentType.BB_GLOBAL;
                Instrument tickerInstrument;
                tickerInstrument = new Instrument();
                tickerInstrument.typeSpecified = true;
                tickerInstrument.type = instTyp;
                //if (instTyp == InstrumentType.TICKER)
                //{
                //    tickerInstrument.yellowkeySpecified = true;
                //    string[] tick = ticker.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //    MarketSector mSec = (MarketSector)Enum.Parse(typeof(MarketSector), tick[tick.Length - 1], true);
                //    tickerInstrument.yellowkey = mSec;
                //    tick = tick.Take(tick.Length - 1).ToArray<string>();
                //    tickerInstrument.id = String.Join(" ", tick);
                //}
                //else
                //{

                // omshiv, june 20,2014, if BBGID is null or empty then we fetch additional data with ticker symbol
                //Ticker symbol is always updated with bloomberg symbol or cusip in case of fixed income.
                if (!string.IsNullOrWhiteSpace(bbgid))
                {
                    tickerInstrument.id = bbgid;
                }
                else
                {
                    tickerInstrument.yellowkeySpecified = true;
                    tickerInstrument.type = InstrumentType.TICKER;
                    string[] tick = secMasterBaseObj.TickerSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    MarketSector mSec = (MarketSector)Enum.Parse(typeof(MarketSector), tick[tick.Length - 1], true);
                    tickerInstrument.yellowkey = mSec;
                    tick = tick.Take(tick.Length - 1).ToArray<string>();
                    tickerInstrument.id = String.Join(" ", tick);
                }




                //}
                SubmitGetDataRequest sbmtGtDtReq = new SubmitGetDataRequest();
                sbmtGtDtReq.headers = getDataHeaders;
                string[] fields = new string[] { "" };
                switch (secMasterBaseObj.AssetCategory)
                {
                    case AssetCategory.EquityOption:
                    case AssetCategory.Option:
                        fields = new string[]{  "OPT_UNDL_TICKER",
                                                "OPT_MULTIPLIER",
                                                "OPT_STRIKE_PX",
                                                "OPT_EXPIRE_DT", 
                                                "OPT_PUT_CALL",
                                                "OPT_CONT_SIZE"
                                               
                                             };
                        break;
                    case AssetCategory.FixedIncome:
                        ///TODO:PKE segregate different fixed incomes and fill the required data.
                        fields = new string[]{  "ISSUE_DT",
                                                "CPN",
                                                "MATURITY",
                                                "DAY_CNT_DES",
                                                "FIRST_CPN_DT",
                                                "ZERO_CPN",
                                                "CPN_FREQ",
                                                "CALC_TYP_DES", //bond Type
                                                "DAYS_TO_SETTLE",
                                                };
                        break;
                    case AssetCategory.Forex:
                    case AssetCategory.FX:
                        fields = new string[] { "BASE_CRNCY" };
                        break;
                    case AssetCategory.Future:
                        fields = new string[] { "LAST_TRADEABLE_DT",
                                                "FUT_CONT_SIZE",
                                                "TRADING_DAY_END_TIME_EOD"};
                        break;
                    case AssetCategory.FutureOption:
                        fields = new string[]{  "OPT_UNDL_TICKER",
                                                "OPT_MULTIPLIER",
                                                "FUT_CONT_SIZE",
                                                "OPT_STRIKE_PX",
                                                "OPT_EXPIRE_DT", 
                                                "OPT_PUT_CALL"};
                        //"PX_SETTLE_LAST_DT"
                        break;
                    case AssetCategory.FXForward: fields = new string[] {   "BASE_CRNCY" 
                                                                            //,"PX_SETTLE_LAST_DT" 
                                                                        };
                        break;
                    case AssetCategory.FXOption:
                        fields = new string[] { "BASE_CRNCY", 
                                                "OPT_UNDL_TICKER",
                                                "OPT_MULTIPLIER",
                                                "OPT_STRIKE_PX",
                                                "OPT_EXPIRE_DT", 
                                                "OPT_PUT_CALL",
                                                "PX_SETTLE_LAST_DT" };
                        break;
                    default:
                        BusinessObjects.Data obj = new BusinessObjects.Data();
                        obj.SecMasterData = secMasterBaseObj;
                        SymbolDataResponse(this, obj);
                        break;
                    //case AssetCategory.Indices:
                    //    break;
                }
                if (fields != null)
                {
                    sbmtGtDtReq.fields = fields;
                }
                Instruments instrs = new Instruments();
                instrs.instrument = new Instrument[] { tickerInstrument };
                sbmtGtDtReq.instruments = instrs;
                SubmitGetDataResponse sbmtGtDtResp = null;
                try
                {
                    sbmtGtDtResp = RetryHelper.Do<SubmitGetDataRequest, SubmitGetDataResponse>(_perSecurityRef.submitGetDataRequest, TimeSpan.FromSeconds(2), sbmtGtDtReq, 2);
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
                if (sbmtGtDtResp == null)
                    return;

                #region Log requested symbols from Bloomberg
                foreach (Instrument inst in sbmtGtDtReq.instruments.instrument)
                {
                    string symbol = inst.id + " " + inst.yellowkey;
                    symbol = symbol.Trim();
                    XElement elem = new XElement("Data",
                    new XElement("InstrumentType", inst.type),
                    new XElement("Fields", sbmtGtDtReq.fields.Select(x => new XElement("Field", x))),
                    new XElement("RequestId", sbmtGtDtResp.requestId),
                    new XElement("ResponseId", sbmtGtDtResp.responseId),
                    new XElement("StatusCode", sbmtGtDtResp.statusCode.code),
                    new XElement("StatusDescription", sbmtGtDtResp.statusCode.description));
                    var reader = elem.CreateReader();
                    reader.MoveToContent();
                    DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Simple Get Data Additional Request", reader.ReadInnerXml());
                    LogRequestResponseDataFromBB(dtLog);
                }
                #endregion
                //SubmitGetDataResponse sbmtGtDtResp = _perSecurityRef.submitGetDataRequest(sbmtGtDtReq);
                _pendingReqRespForAdditionalSymbolData.TryAdd(sbmtGtDtResp.requestId, new Tuple<string, SecMasterBaseObj, SubmitGetDataRequest>(sbmtGtDtResp.responseId, secMasterBaseObj, sbmtGtDtReq));

                BackgroundWorker responseThreadForAddSecurityInfo = new BackgroundWorker();
                responseThreadForAddSecurityInfo.DoWork += responseThreadForAddSecurityInfo_DoWork;
                responseThreadForAddSecurityInfo.RunWorkerCompleted += responseThreadForAddSecurityInfo_RunWorkerCompleted;
                if (!_isResponseThreadForAdditionalSymbolDataRunning)
                {
                    _isResponseThreadForAdditionalSymbolDataRunning = true;
                    responseThreadForAddSecurityInfo.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        /// <summary>
        /// Creates a new derived SymbolData instance depending on asset
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="secType"></param>
        /// <param name="marketSector"></param>
        private static void GetSecmasterBaseObjForSecurityType(out SecMasterBaseObj secMasterBaseObj, string secType, string marketSector)
        {
            try
            {
                AssetCategory info;
                if (!BloombergSecurityTypeMapping.Instance.SecTypeSymbolDataInfoMapping.TryGetValue(secType, out info))
                {
                    BloombergSecurityTypeMapping.Instance.SecTypeSymbolDataInfoMapping.TryGetValue(marketSector, out info);
                }
                secMasterBaseObj = SecurityMasterFactory.GetSecmasterObject(info);
            }
            catch (Exception ex)
            {
                secMasterBaseObj = null;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates and Fills the initial symboldata from the response from Bloomberg
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="instrumentData"></param>
        /// <param name="instrOriginalRequest"></param>
        /// <returns></returns>
        private static SecMasterBaseObj SymbolDataFromResponse(string[] fields, InstrumentData instrumentData, Instrument instrOriginalRequest)
        {
            // SymbolData symbolData = null;
            SecMasterBaseObj secMasterBaseObj = null;
            Dictionary<string, string> returnedFieldsValue = new Dictionary<string, string>();
            try
            {

                for (int j = 0; j < instrumentData.data.Length; j++)
                {
                    ///TODO:PKE handle array type fields, depends on how we are saving information
                    if (!instrumentData.data[j].isArray)
                    {
                        if (String.Compare(instrumentData.data[j].value.Trim(), "N.A.", true) == 0 || String.Compare(instrumentData.data[j].value.Trim(), "N.D.", true) == 0)
                        {
                            returnedFieldsValue.Add(fields[j], "");
                        }
                        else
                        {
                            returnedFieldsValue.Add(fields[j], instrumentData.data[j].value);
                        }
                    }
                    else
                    {
                        ///TODO:PKE Add handling for array type fields for bloomberg
                    }
                }
                //ID_BB_SEC_NUM_DES is being used as ticker as it is the field which is shown on the page bsym.bloomberg.com for ticker column
                if (!String.IsNullOrWhiteSpace(returnedFieldsValue["ID_BB_SEC_NUM_DES"]))
                {
                    #region fill Sec master obj
                    StringBuilder validationComments = new StringBuilder();

                    //modified by omshiv, get sec master base obj
                    GetSecmasterBaseObjForSecurityType(out secMasterBaseObj, returnedFieldsValue["SECURITY_TYP"], returnedFieldsValue["MARKET_SECTOR_DES"]);

                    if (secMasterBaseObj == null)
                    {
                        return null;
                    }
                    secMasterBaseObj.BloombergSymbol = BBSecurityTypeTickerSymbolMapping.Instance.GetBloombergSymbolAccordingToAssetAndSecurityType(returnedFieldsValue["SECURITY_TYP"], returnedFieldsValue["ID_BB_SEC_NUM_DES"], returnedFieldsValue["EXCH_CODE"], returnedFieldsValue["MARKET_SECTOR_DES"]);
                    // unique identifier BBGID
                    if (!string.IsNullOrWhiteSpace(returnedFieldsValue["ID_BB_GLOBAL"]))
                    {
                        secMasterBaseObj.BBGID = returnedFieldsValue["ID_BB_GLOBAL"].Trim();
                    }
                    //ISIN
                    if (!string.IsNullOrWhiteSpace(returnedFieldsValue["ID_ISIN"]))
                    {
                        secMasterBaseObj.ISINSymbol = returnedFieldsValue["ID_ISIN"].Trim();
                    }

                    //Cusip
                    if (!string.IsNullOrWhiteSpace(returnedFieldsValue["ID_CUSIP"]))
                    {
                        secMasterBaseObj.CusipSymbol = returnedFieldsValue["ID_CUSIP"].Trim();
                    }
                    //Sedol
                    if (!string.IsNullOrWhiteSpace(returnedFieldsValue["ID_SEDOL1"]))
                    {
                        secMasterBaseObj.SedolSymbol = returnedFieldsValue["ID_SEDOL1"].Trim();
                    }


                    //_osiOptionSymbol = level1Data.OSIOptionSymbol;
                    //_idcoOptionSymbol = level1Data.IDCOOptionSymbol;
                    //_opraSymbol = level1Data.OpraSymbol;

                    //If Asset class in cusip then we setting ticker to cusip else BB symbol
                    if (secMasterBaseObj.AssetCategory == AssetCategory.FixedIncome)
                    {
                        secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.CusipSymbol);
                        secMasterBaseObj.PrimarySymbology = (int)ApplicationConstants.SymbologyCodes.CUSIPSymbol;
                    }
                    else
                    {
                        secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.BloombergSymbol);
                        secMasterBaseObj.PrimarySymbology = (int)ApplicationConstants.SymbologyCodes.BloombergSymbol;
                    }

                    // need to check for returned symbol - omshiv , currently it will be blank                  
                    secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.ReutersSymbol);

                    secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.ISINSymbol);
                    secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.SedolSymbol);
                    secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.CusipSymbol);
                    secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.BloombergSymbol);
                    secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.OSIOptionSymbol);
                    secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.IDCOOptionSymbol);
                    secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.OpraSymbol);

                    secMasterBaseObj.RequestedSymbology = GetNirvanaSymbologyFromInstrumentType(instrOriginalRequest.type);

                    secMasterBaseObj.UnderLyingSymbol = secMasterBaseObj.TickerSymbol;
                    secMasterBaseObj.Multiplier = 1; // Here setting multiplier to 1 and on addition data request we will update from BB multiplier - omshiv


                    ///TODO: PKE Correct exchange mapping. Handle mapping of bloomberg exchanges and esignal exchanges. Remove hardcoding for nasdaq when the mapping is not found.
                    String ListedExchange = Prana.BlpDLWSAdapter.BusinessObject.Mappings.ExchangeCodeMapping.Instance.GetExchangeCodeForBBExchangeCode(returnedFieldsValue["EXCH_CODE"]);


                    if (String.IsNullOrWhiteSpace(ListedExchange))
                    {
                        ListedExchange = Prana.BlpDLWSAdapter.BusinessObject.Mappings.ExchangeCodeMapping.Instance.GetDefaultExchangeCodeForAsset(secMasterBaseObj.AssetCategory);
                        //Added By Faisal Shah Dated 10/07/14
                        //Need was to show a message on Trade Server if we process a newly Verified Symbol from API with Default AUECID.
                        validationComments.Append(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString());
                        validationComments.Append(",");
                        ExceptionPolicy.HandleException(new Exception(SecMasterConstants.MSG_AUECNotFoundLivefeed + secMasterBaseObj.ToString()), ApplicationConstants.POLICY_LOGANDSHOW);
                    }
                    ///TODO: PKE Move data enriching code from DLWS adapter to common historical feed manager data enricher and remove references for common data cache from the adapters
                    if (!String.IsNullOrWhiteSpace(ListedExchange) && secMasterBaseObj.AssetCategory != AssetCategory.None)
                    {
                        secMasterBaseObj.AUECID = CentralSMDataCache.CentralSMCacheManager.Instance.GetAUECIdByExchangeIdentifier(ListedExchange + "-" + secMasterBaseObj.AssetCategory.ToString());
                    }
                    if (secMasterBaseObj.AUECID == int.MinValue)
                    {
                        ListedExchange = Prana.BlpDLWSAdapter.BusinessObject.Mappings.ExchangeCodeMapping.Instance.GetDefaultExchangeCodeForAsset(secMasterBaseObj.AssetCategory);
                        secMasterBaseObj.AUECID = CentralSMDataCache.CentralSMCacheManager.Instance.GetAUECIdByExchangeIdentifier(ListedExchange + "-" + secMasterBaseObj.AssetCategory.ToString());

                        if (secMasterBaseObj.AUECID == int.MinValue)
                        {
                            secMasterBaseObj.AUECID = 1;
                        }
                    }

                    if (secMasterBaseObj.AssetCategory == AssetCategory.Indices)
                    {
                        secMasterBaseObj.AUECID = CentralSMDataCache.CentralSMCacheManager.Instance.GetAUECIdByExchangeIdentifier(secMasterBaseObj.AssetCategory.ToString() + "-" + secMasterBaseObj.AssetCategory.ToString());
                    }

                    if (secMasterBaseObj.CurrencyID < 0 && secMasterBaseObj.AUECID > 0)
                    {
                        int currencyID = CentralSMDataCache.CentralSMCacheManager.Instance.GetCurrencyIdByAUECID(secMasterBaseObj.AUECID);
                        // string currencyText = CentralSMDataCache.CentralSMCacheManager.Instance.GetCurrencyText(currencyID);
                        secMasterBaseObj.CurrencyID = currencyID; //symbolData.CurencyCode = currencyText;
                    }
                    if (secMasterBaseObj.AUECID != int.MinValue)
                    {
                        int exchangeID = int.MinValue;
                        Underlying underlying = Underlying.None;
                        CentralSMDataCache.CentralSMCacheManager.Instance.GetUnderlyingExchangeIDFromAUECID(secMasterBaseObj.AUECID, ref underlying, ref exchangeID);

                        secMasterBaseObj.UnderLyingID = (int)underlying;
                        secMasterBaseObj.ExchangeID = exchangeID;
                    }
                    if (!String.IsNullOrWhiteSpace(returnedFieldsValue["CRNCY"]))
                        secMasterBaseObj.CurrencyID = CentralSMCacheManager.Instance.GetCurrencyID(returnedFieldsValue["CRNCY"]);

                    if (!String.IsNullOrWhiteSpace(returnedFieldsValue["LONG_COMP_NAME"]))
                        secMasterBaseObj.LongName = returnedFieldsValue["LONG_COMP_NAME"];
                    else
                        secMasterBaseObj.LongName = returnedFieldsValue["NAME"];
                    //OPRA_SYMBOL
                    secMasterBaseObj.CreationDate = DateTime.Now;
                    //secMasterBaseObj.ModifiedDate = DateTime.Now;
                    secMasterBaseObj.CreatedBy = SecMasterConstants.SecMasterSourceOfData.BloombergDLWS.ToString();
                    secMasterBaseObj.SourceOfDataID = (int)SecMasterConstants.SecMasterSourceOfData.BloombergDLWS;
                    secMasterBaseObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.BloombergDLWS;

                    if (secMasterBaseObj.CurrencyID == int.MinValue || secMasterBaseObj.CurrencyID == 0)
                    {
                        string msg = string.Empty;
                        if (secMasterBaseObj.CurrencyID == 0)
                            msg = SecMasterConstants.MSG_CurrencyNotExistInSystem;
                        else
                            msg = SecMasterConstants.MSG_CurrencyNotFoundLivefeed;
                        //Added By Faisal Shah Dated 10/07/14
                        //Need was to show a message on Central Server if we process a newly Verified Symbol from API with Default CurrencyID.
                        secMasterBaseObj.CurrencyID = CentralSMDataCache.CentralSMCacheManager.Instance.GetCurrencyIdByAUECID(secMasterBaseObj.AUECID);
                        validationComments.Append(SecMasterConstants.SecMasterComments.DefaultCurrency.ToString());
                        validationComments.Append(",");
                        ExceptionPolicy.HandleException(new Exception(msg + secMasterBaseObj.ToString()), ApplicationConstants.POLICY_LOGANDSHOW);
                    }

                    //Sedol
                    UDAData udaData = new UDAData();

                    if (!string.IsNullOrWhiteSpace(returnedFieldsValue["MARKET_SECTOR_DES"]))
                    {
                        udaData.UDAAsset = returnedFieldsValue["MARKET_SECTOR_DES"];
                        udaData.AssetID = CentralSMCacheManager.Instance.GetUDAIDFromText(udaData.UDAAsset, SecMasterConstants.CONST_UDAAsset);
                    }
                    if (!string.IsNullOrWhiteSpace(returnedFieldsValue["SECURITY_TYP"]))
                    {
                        udaData.UDASecurityType = returnedFieldsValue["SECURITY_TYP"];
                        udaData.SecurityTypeID = CentralSMCacheManager.Instance.GetUDAIDFromText(udaData.UDASecurityType, SecMasterConstants.CONST_UDASecurityType);
                    }



                    if (!string.IsNullOrWhiteSpace(returnedFieldsValue["INDUSTRY_SECTOR"]))
                    {
                        udaData.UDASector = returnedFieldsValue["INDUSTRY_SECTOR"];

                        udaData.SectorID = CentralSMCacheManager.Instance.GetUDAIDFromText(udaData.UDASector, SecMasterConstants.CONST_UDASector);


                    }
                    if (!string.IsNullOrWhiteSpace(returnedFieldsValue["INDUSTRY_GROUP"]))
                    {
                        udaData.UDASubSector = returnedFieldsValue["INDUSTRY_GROUP"];

                        udaData.SubSectorID = CentralSMCacheManager.Instance.GetUDAIDFromText(udaData.UDASubSector, SecMasterConstants.CONST_UDASubSector);

                    }

                    if (!string.IsNullOrWhiteSpace(returnedFieldsValue["COUNTRY"]))
                    {
                        udaData.UDACountry = returnedFieldsValue["COUNTRY"];

                        udaData.CountryID = CentralSMCacheManager.Instance.GetUDAIDFromText(udaData.UDACountry, SecMasterConstants.CONST_UDACountry);

                    }
                    udaData.Symbol = secMasterBaseObj.TickerSymbol;

                    secMasterBaseObj.SymbolUDAData = udaData;

                    //PKE: Below function has to be modified to suit the central db need

                    //TODO: Handle UDAs in CentralDB using the Bloomberg also update it to secmasterbaseobj
                    //CentralSMDataCache.Instance.UpdateUDADataWithName(secMasterObj);

                    //set comments if default auec
                    secMasterBaseObj.Comments = validationComments.ToString();
                    #endregion

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return secMasterBaseObj;
        }

        /// <summary>
        /// Changes the boolean used to track whether a thread for polling is running or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void responseThreadForAddSecurityInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                _isResponseThreadForAdditionalSymbolDataRunning = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Return the enum equivalent of put call for string from Bloomberg
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Prana.BusinessObjects.AppConstants.OptionType GetOptionType(string value)
        {
            try
            {
                //note: bloomberg has additional types
                //'C' for a call option, 'P' for a put option, 'T' for a touch option, 'F' for a forward or 'M' for a multi-leg option.
                if (value.StartsWith("P"))
                    return Prana.BusinessObjects.AppConstants.OptionType.Put;
                else if (value.StartsWith("C"))
                    return Prana.BusinessObjects.AppConstants.OptionType.Call;
                else
                    return Prana.BusinessObjects.AppConstants.OptionType.None;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return Prana.BusinessObjects.AppConstants.OptionType.None;
            }
        }

        /// <summary>
        /// Fills additional symboldata according to asset from the response received from Bloomberg
        /// </summary>
        /// <param name="symbData"></param>
        /// <param name="fields"></param>
        /// <param name="instrumentData"></param>
        /// <returns></returns>
        static SecMasterBaseObj FillAdditionalSymbolDataFromResponse(SecMasterBaseObj symbData, string[] fields, InstrumentData instrumentData)
        {
            try
            {
                if (ApplicationConstants.LoggingEnabled)
                {
                    LogDataFromBB("BLOOMBERG Additional Data Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(instrumentData));
                }
                Dictionary<string, string> returnedFieldsValue = new Dictionary<string, string>();
                for (int j = 0; j < instrumentData.data.Length; j++)
                {
                    ///TODO:PKE handle array type fields, depends on how we are saving information
                    if (!instrumentData.data[j].isArray)
                    {
                        if (String.Compare(instrumentData.data[j].value.Trim(), "N.A.", true) == 0 || String.Compare(instrumentData.data[j].value.Trim(), "N.D.", true) == 0)
                        {
                            returnedFieldsValue.Add(fields[j], "");
                        }
                        else
                        {
                            returnedFieldsValue.Add(fields[j], instrumentData.data[j].value);
                        }
                    }
                    else
                    {
                        ///TODO:PKE Add handling for array type fields for bloomberg
                    }
                }

                Double tempStrike, coupon;
                DateTime tempExpDate, maturityDate, tempFirstCouponDt, tempIssueDt, tempCutOffTime;
                string[] tick;
                MarketSector mSec;
                switch (symbData.AssetCategory)
                {
                    case AssetCategory.EquityOption:
                    case AssetCategory.Option:
                        SecMasterOptObj optObj = symbData as SecMasterOptObj;
                        if (optObj != null)
                        {
                            tick = symbData.BloombergSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tick.Length > 1)
                            {
                                if (Enum.TryParse<MarketSector>(tick[tick.Length - 1], true, out mSec))
                                {
                                    String underlyingSymbol = (returnedFieldsValue["OPT_UNDL_TICKER"] + " " + mSec.ToString()).ToUpper();
                                    symbData.UnderLyingSymbol = underlyingSymbol;
                                }
                            }
                            if (Double.TryParse(returnedFieldsValue["OPT_STRIKE_PX"], out tempStrike))
                                optObj.StrikePrice = tempStrike;
                            if (DateTime.TryParse(returnedFieldsValue["OPT_EXPIRE_DT"], out tempExpDate))
                                optObj.ExpirationDate = tempExpDate;
                            optObj.PutOrCall = (int)GetOptionType(returnedFieldsValue["OPT_PUT_CALL"]);

                            //if (!String.IsNullOrWhiteSpace(returnedFieldsValue["FUT_CONT_SIZE"]))
                            //    optObj.RoundLot = int.Parse(returnedFieldsValue["FUT_CONT_SIZE"]);

                            if (!String.IsNullOrWhiteSpace(returnedFieldsValue["OPT_MULTIPLIER"]))
                                optObj.Multiplier = double.Parse(returnedFieldsValue["OPT_MULTIPLIER"]);
                        }
                        //"OPT_MULTIPLIER",
                        break;
                    case AssetCategory.FixedIncome:

                        SecMasterFixedIncome secMasterFixedIncome = symbData as SecMasterFixedIncome;
                        if (secMasterFixedIncome != null)
                        {
                            //TODO: PKE Coupon and frequency have been mapped only for the fields valid in our application
                            if (DateTime.TryParse(returnedFieldsValue["ISSUE_DT"], out tempIssueDt))
                                secMasterFixedIncome.IssueDate = tempIssueDt;
                            if (Double.TryParse(returnedFieldsValue["CPN"], out coupon))
                                secMasterFixedIncome.Coupon = coupon;
                            if (DateTime.TryParse(returnedFieldsValue["MATURITY"], out maturityDate))
                                secMasterFixedIncome.MaturityDate = maturityDate;
                            secMasterFixedIncome.AccrualBasis = AccrualBasisMapping.GetAccrualBasisFromBloombergAccruedBasis(returnedFieldsValue["DAY_CNT_DES"]);
                            secMasterFixedIncome.AccrualBasisID = (int)AccrualBasisMapping.GetAccrualBasisFromBloombergAccruedBasis(returnedFieldsValue["DAY_CNT_DES"]);
                            if (DateTime.TryParse(returnedFieldsValue["FIRST_CPN_DT"], out tempFirstCouponDt))
                                secMasterFixedIncome.FirstCouponDate = tempFirstCouponDt;
                            if (returnedFieldsValue["ZERO_CPN"].StartsWith("Y", StringComparison.InvariantCultureIgnoreCase))
                                secMasterFixedIncome.IsZero = true;
                            else
                                secMasterFixedIncome.IsZero = false;

                            //setting days to settlement for fixed income - omshiv
                            int DaysToSettle = 1;
                            int.TryParse(returnedFieldsValue["DAYS_TO_SETTLE"], out DaysToSettle);

                            secMasterFixedIncome.DaysToSettlement = DaysToSettle;
                            //TODO need to analyse bond Types from CALC_TYP_DES
                            //secMasterFixedIncome.BondType = SecurityType.Agency; // returnedFieldsValue["CALC_TYP_DES"];// SecurityType.

                            secMasterFixedIncome.CouponFrequencyID = (int)CouponFrequencyMapping.GetCouponFrequency(returnedFieldsValue["CPN_FREQ"]);
                            secMasterFixedIncome.Frequency = CouponFrequencyMapping.GetCouponFrequency(returnedFieldsValue["CPN_FREQ"]);
                        }
                        break;
                    case AssetCategory.Forex:
                    case AssetCategory.FX:
                        //modified by omshiv, set Lead and vs curency for FX
                        SecMasterFxObj secMasterFx = symbData as SecMasterFxObj;
                        if (secMasterFx != null)
                        {
                            if (!String.IsNullOrWhiteSpace(returnedFieldsValue["BASE_CRNCY"]))
                                secMasterFx.LeadCurrencyID = CentralSMCacheManager.Instance.GetCurrencyID(returnedFieldsValue["BASE_CRNCY"]);
                            secMasterFx.VsCurrencyID = symbData.CurrencyID;
                        }

                        break;
                    case AssetCategory.Future:
                        SecMasterFutObj secMasterFuture = symbData as SecMasterFutObj;
                        if (secMasterFuture != null)
                        {
                            if (DateTime.TryParse(returnedFieldsValue["LAST_TRADEABLE_DT"], out tempExpDate))
                                secMasterFuture.ExpirationDate = tempExpDate;
                            if (DateTime.TryParse(returnedFieldsValue["TRADING_DAY_END_TIME_EOD"], out tempCutOffTime))
                                secMasterFuture.CutOffTime = tempCutOffTime.ToString();
                            if (!String.IsNullOrWhiteSpace(returnedFieldsValue["FUT_CONT_SIZE"]))
                                secMasterFuture.Multiplier = double.Parse(returnedFieldsValue["FUT_CONT_SIZE"]);
                            //fields = new string[] { "PX_SETTLE_LAST_DT" }; FUT_CONTRACT_DT, FUT_CONT_SIZE
                        }
                        break;
                    case AssetCategory.FutureOption:
                        SecMasterOptObj secMasterFutureOpt = symbData as SecMasterOptObj;
                        if (secMasterFutureOpt != null)
                        {
                            tick = symbData.BloombergSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tick.Length > 1)
                            {
                                if (Enum.TryParse<MarketSector>(tick[tick.Length - 1], true, out mSec))
                                {
                                    if (!string.IsNullOrWhiteSpace(returnedFieldsValue["OPT_UNDL_TICKER"]))
                                        secMasterFutureOpt.UnderLyingSymbol = (returnedFieldsValue["OPT_UNDL_TICKER"] + " " + mSec.ToString()).ToUpper();
                                }
                            }
                            if (Double.TryParse(returnedFieldsValue["OPT_STRIKE_PX"], out tempStrike))
                                secMasterFutureOpt.StrikePrice = tempStrike;
                            //"OPT_MULTIPLIER","FUT_CONT_SIZE" add check for string.empty before assigning to multiplier
                            if (DateTime.TryParse(returnedFieldsValue["OPT_EXPIRE_DT"], out tempExpDate))
                                secMasterFutureOpt.ExpirationDate = tempExpDate;
                            secMasterFutureOpt.PutOrCall = (int)GetOptionType(returnedFieldsValue["OPT_PUT_CALL"]);
                            //"PX_SETTLE_LAST_DT"
                            //if (!String.IsNullOrWhiteSpace(returnedFieldsValue["FUT_CONT_SIZE"]))
                            //    secMasterFutureOpt.RoundLot = int.Parse(returnedFieldsValue["FUT_CONT_SIZE"]);

                            if (!String.IsNullOrWhiteSpace(returnedFieldsValue["OPT_MULTIPLIER"]))
                                secMasterFutureOpt.Multiplier = double.Parse(returnedFieldsValue["OPT_MULTIPLIER"]);


                        }
                        break;
                    case AssetCategory.FXForward:
                        SecMasterFXForwardObj secMasterFwdObj = symbData as SecMasterFXForwardObj;
                        if (secMasterFwdObj != null)
                        {
                            if (!String.IsNullOrWhiteSpace(returnedFieldsValue["BASE_CRNCY"]))
                                secMasterFwdObj.LeadCurrencyID = CentralSMCacheManager.Instance.GetCurrencyID(returnedFieldsValue["BASE_CRNCY"]);
                            secMasterFwdObj.VsCurrencyID = symbData.CurrencyID;
                            secMasterFwdObj.CurrencyID = CentralSMCacheManager.Instance.GetCurrencyID("MUL");
                            #region Hardcoded filling of settlement date from the symbol into security master as the settlement date is not received from BB
                            tick = symbData.BloombergSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tick.Length > 1)
                            {
                                string[] tempDate = tick[tick.Length - 2].Split('/');
                                if (tempDate.Length == 3)
                                {
                                    int tempInt;
                                    if (int.TryParse(tempDate[0], out tempInt) && int.TryParse(tempDate[1], out tempInt) && int.TryParse(tempDate[2], out tempInt))
                                        secMasterFwdObj.ExpirationDate = new DateTime(int.Parse(tempDate[2]) + 2000, int.Parse(tempDate[0]), int.Parse(tempDate[1]));
                                }
                            }
                            #endregion
                        }
                        break;
                    case AssetCategory.FXOption:
                        SecMasterOptObj secMasterFXOption = symbData as SecMasterOptObj;
                        if (secMasterFXOption != null)
                        {
                            tick = symbData.BloombergSymbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tick.Length > 1)
                            {
                                if (Enum.TryParse<MarketSector>(tick[tick.Length - 1], true, out mSec))
                                {
                                    secMasterFXOption.UnderLyingSymbol = (returnedFieldsValue["OPT_UNDL_TICKER"] + " " + mSec.ToString()).ToUpper();
                                }
                            }
                            if (Double.TryParse(returnedFieldsValue["OPT_STRIKE_PX"], out tempStrike))
                                secMasterFXOption.StrikePrice = tempStrike;
                            if (DateTime.TryParse(returnedFieldsValue["OPT_EXPIRE_DT"], out tempExpDate))
                                secMasterFXOption.ExpirationDate = tempExpDate;
                            secMasterFXOption.PutOrCall = (int)GetOptionType(returnedFieldsValue["OPT_PUT_CALL"]);



                            if (!String.IsNullOrWhiteSpace(returnedFieldsValue["OPT_MULTIPLIER"]))
                                secMasterFXOption.Multiplier = double.Parse(returnedFieldsValue["OPT_MULTIPLIER"]);

                            //fields = new string[] { "BASE_CRNCY", 
                            //"OPT_MULTIPLIER",
                            //FXOPT_EXPIRY_DATE
                            //"PX_SETTLE_LAST_DT" };
                        }
                        break;
                    default:
                        break;
                }
                return symbData;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return symbData;
            }
        }

        /// <summary>
        /// Polls bloomberg for response for requested additional data. TODO: check that all the possible fields are mapped and received from bloomberg
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void responseThreadForAddSecurityInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (_pendingReqRespForAdditionalSymbolData.Count > 0)
                {
                    Tuple<string, SecMasterBaseObj, SubmitGetDataRequest> tempRespID;
                    foreach (KeyValuePair<string, Tuple<string, SecMasterBaseObj, SubmitGetDataRequest>> requestIDRespIDPair in _pendingReqRespForAdditionalSymbolData)
                    {
                        //retrieve get data request. The response ID sent for the request is the response ID
                        //received from SubmitGetDataRequest()
                        RetrieveGetDataRequest rtrvGtDrReq = new RetrieveGetDataRequest();
                        rtrvGtDrReq.responseId = requestIDRespIDPair.Value.Item1;
                        RetrieveGetDataResponse rtrvGtDrResp = null;
                        try
                        {
                            rtrvGtDrResp = RetryHelper.Do<RetrieveGetDataRequest, RetrieveGetDataResponse>(_perSecurityRef.retrieveGetDataResponse, TimeSpan.FromSeconds(2), rtrvGtDrReq, 2);
                        }
                        catch (Exception ex)
                        {
                            SendInvalidSecurityResponse(requestIDRespIDPair.Value.Item2.RequestedSymbol, GetInstrumentTypeFromSymbologyCode((ApplicationConstants.SymbologyCodes)requestIDRespIDPair.Value.Item2.PrimarySymbology), "Security Not validated, Bloomberg cannot be reached");
                            _pendingReqRespForAdditionalSymbolData.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                            // Invoke our policy that is responsible for making sure no secure information
                            // gets out of our layer.
                            bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                            if (rethrow)
                            {
                                throw;
                            }
                            continue;
                        }
                        //rtrvGtDrResp = _perSecurityRef.retrieveGetDataResponse(rtrvGtDrReq);
                        if (rtrvGtDrResp == null)
                            continue;
                        if (rtrvGtDrResp.statusCode.code == SUCCESS)
                        {
                            //Displaying the RetrieveGetDataResponse
                            for (int i = 0; i < rtrvGtDrResp.instrumentDatas.Length; i++)
                            {
                                #region Log requested symbols from Bloomberg
                                string symbol = _pendingReqRespForAdditionalSymbolData[requestIDRespIDPair.Key].Item3.instruments.instrument[i].id + " " + _pendingReqRespForAdditionalSymbolData[requestIDRespIDPair.Key].Item3.instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data",
                                new XElement("FieldValues", rtrvGtDrResp.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x), new XAttribute("Value", rtrvGtDrResp.instrumentDatas[i].data[index].value)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Simple Get Data Additional Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                                #endregion
                                SecMasterBaseObj secMasterBaseObj = _pendingReqRespForAdditionalSymbolData[requestIDRespIDPair.Key].Item2;
                                secMasterBaseObj = FillAdditionalSymbolDataFromResponse(secMasterBaseObj, rtrvGtDrResp.fields, rtrvGtDrResp.instrumentDatas[i]);

                                if (ApplicationConstants.LoggingEnabled)
                                {
                                    LogDataFromBB("BLOOMBERG Additional Data Response: Symbol:" + secMasterBaseObj.TickerSymbol + " Data:" + Newtonsoft.Json.JsonConvert.SerializeObject(secMasterBaseObj));
                                }

                                BusinessObjects.Data obj = new BusinessObjects.Data();
                                obj.SecMasterData = secMasterBaseObj;
                                SymbolDataResponse(this, obj);
                                if (_symbolsRequestedToBBForAdditionalData.Values.Contains(rtrvGtDrResp.instrumentDatas[i].instrument.id))
                                {
                                    string tempSymbol = _symbolsRequestedToBBForAdditionalData.Where(pair => rtrvGtDrResp.instrumentDatas[i].instrument.id.Equals(pair.Value, StringComparison.InvariantCultureIgnoreCase)).Select(pair => pair.Key).FirstOrDefault();
                                    string tempBBGID;
                                    _symbolsRequestedToBBForAdditionalData.TryRemove(tempSymbol, out tempBBGID);
                                }
                            }
                            _pendingReqRespForAdditionalSymbolData.TryRemove(requestIDRespIDPair.Key, out tempRespID);

                            //DataTable dtLog = CreateDataTableForLogging(rtrvGtDrResp.instrumentDatas.ToString(), System.DateTime.Now, "Response", rtrvGtDrResp.instrumentDatas.ToString());
                            //LogRequestResponseDataFromBB(dtLog);
                        }
                        else if (rtrvGtDrResp.statusCode.code == REQUEST_ERROR)
                        {
                            #region Log requested symbols from Bloomberg
                            SubmitGetDataRequest sbmtGetDataReq = _pendingReqRespForAdditionalSymbolData[requestIDRespIDPair.Key].Item3;
                            for (int i = 0; i < sbmtGetDataReq.instruments.instrument.Length; i++)
                            {
                                string symbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data",
                                new XElement("InstrumentType", sbmtGetDataReq.instruments.instrument[i].type),
                                new XElement("FieldValues", sbmtGetDataReq.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Simple Get Data Additional Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                            }
                            #endregion
                            try
                            {
                                throw new Exception("Bloomberg resulted in request error when it requested data for response id " + requestIDRespIDPair.Key);
                            }
                            catch (Exception ex)
                            {
                                // Invoke our policy that is responsible for making sure no secure information
                                // gets out of our layer.
                                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                                if (rethrow)
                                {
                                    throw;
                                }
                            }
                            _pendingReqRespForAdditionalSymbolData.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                        }
                        else if (rtrvGtDrResp.statusCode.code == DATA_NOT_AVAILABLE)
                        {
                            //TODO
                    }
                        else
                        {
                            #region Log requested symbols from Bloomberg
                            SubmitGetDataRequest sbmtGetDataReq = _pendingReqRespForAdditionalSymbolData[requestIDRespIDPair.Key].Item3;
                            for (int i = 0; i < sbmtGetDataReq.instruments.instrument.Length; i++)
                            {
                                string symbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data",
                                new XElement("InstrumentType", sbmtGetDataReq.instruments.instrument[i].type),
                                new XElement("FieldValues", sbmtGetDataReq.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Simple Get Data Additional Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                            }
                            #endregion
                        }
                    }
                    System.Threading.Thread.Sleep(POLL_INTERVAL);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static void LogDataFromBB(string message)
        {
            try
            {
                if (ApplicationConstants.LoggingEnabled)
                {
                    Logger.Write(message, ApplicationConstants.CATEGORY_FLAT_FILE_TRACING);
                    InformationReporter.GetInstance.Write(message);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets Nirvana Symbology analogous to the instrument type in bloomberg
        /// </summary>
        /// <param name="instrumentType"></param>
        /// <returns></returns>
        private static int GetNirvanaSymbologyFromInstrumentType(InstrumentType instrumentType)
        {
            try
            {
                switch (instrumentType)
                {
                    case InstrumentType.TICKER: return (int)ApplicationConstants.SymbologyCodes.BloombergSymbol;
                    case InstrumentType.CUSIP: return (int)ApplicationConstants.SymbologyCodes.CUSIPSymbol;
                    case InstrumentType.ISIN: return (int)ApplicationConstants.SymbologyCodes.ISINSymbol;
                    case InstrumentType.SEDOL: return (int)ApplicationConstants.SymbologyCodes.SEDOLSymbol;
                    case InstrumentType.BB_GLOBAL: return (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return (int)ApplicationConstants.SymbologyCodes.BloombergSymbol;
        }

        /// <summary>
        /// returns bloomberg InstrumentType analogous to the Nirvana SymbologyCodes
        /// </summary>
        /// <param name="nirvanaSymbology"></param>
        /// <returns></returns>
        static InstrumentType GetInstrumentTypeFromSymbologyCode(ApplicationConstants.SymbologyCodes nirvanaSymbology)
        {
            try
            {
                switch (nirvanaSymbology)
                {
                    case ApplicationConstants.SymbologyCodes.TickerSymbol: return InstrumentType.TICKER;
                    case ApplicationConstants.SymbologyCodes.BloombergSymbol: return InstrumentType.TICKER;
                    case ApplicationConstants.SymbologyCodes.CUSIPSymbol: return InstrumentType.CUSIP;
                    case ApplicationConstants.SymbologyCodes.ISINSymbol: return InstrumentType.ISIN;
                    case ApplicationConstants.SymbologyCodes.SEDOLSymbol: return InstrumentType.SEDOL;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return InstrumentType.AUSTRIAN;
        }

        /// <summary>
        /// Changes the boolean which tracks whether the thread is running or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void responseThreadForInitSecurityInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                _isResponseThreadForInitSecurityInfoRunning = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The thread which polls for data from Bloomberg for symbols which have been initially requested
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void responseThreadForInitSecurityInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (_pendingReqRespForInitSecurityInfo.Count > 0)
                {
                    SubmitGetDataRequest tempRespID;
                    foreach (KeyValuePair<string, SubmitGetDataRequest> requestIDRespIDPair in _pendingReqRespForInitSecurityInfo)
                    {
                        //retrieve get data request. The response ID sent for the request is the response ID
                        //received from SubmitGetDataRequest()
                        RetrieveGetDataRequest rtrvGtDrReq = new RetrieveGetDataRequest();
                        rtrvGtDrReq.responseId = requestIDRespIDPair.Key;
                        RetrieveGetDataResponse rtrvGtDrResp = null;
                        try
                        {
                            rtrvGtDrResp = RetryHelper.Do<RetrieveGetDataRequest, RetrieveGetDataResponse>(_perSecurityRef.retrieveGetDataResponse, TimeSpan.FromSeconds(2), rtrvGtDrReq, 2);
                        }
                        catch (Exception ex)
                        {
                            foreach (Instrument inst in requestIDRespIDPair.Value.instruments.instrument)
                            {
                                SendInvalidSecurityResponse(inst.id, inst.type, "Security Not validated, Bloomberg cannot be reached");
                            }
                            _pendingReqRespForInitSecurityInfo.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                            // Invoke our policy that is responsible for making sure no secure information
                            // gets out of our layer.
                            bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                            if (rethrow)
                            {
                                throw;
                            }
                            continue;
                        }
                        //rtrvGtDrResp = _perSecurityRef.retrieveGetDataResponse(rtrvGtDrReq);
                        if (rtrvGtDrResp == null && rtrvGtDrResp.statusCode == null)
                            _pendingReqRespForInitSecurityInfo.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                        else if (rtrvGtDrResp.statusCode.code == SUCCESS)
                        {
                            SubmitGetDataRequest sbmtGetDataReq = requestIDRespIDPair.Value;
                            //Displaying the RetrieveGetDataResponse
                            for (int i = 0; i < rtrvGtDrResp.instrumentDatas.Length; i++)
                            {
                                #region Log requested symbols from Bloomberg
                                string symbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data",
                                new XElement("InstrumentType", sbmtGetDataReq.instruments.instrument[i].type),
                                new XElement("FieldValues", rtrvGtDrResp.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x), new XAttribute("Value", rtrvGtDrResp.instrumentDatas[i].data[index].value)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Simple Get Data Init Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                                #endregion
                                SecMasterBaseObj secMasterBaseObj = SymbolDataFromResponse(rtrvGtDrResp.fields, rtrvGtDrResp.instrumentDatas[i], sbmtGetDataReq.instruments.instrument[i]);

                                if (secMasterBaseObj == null || String.IsNullOrWhiteSpace(secMasterBaseObj.TickerSymbol))
                                { ///TODO: PKE some code for symbol not validated.
                                    ///
                                    //modified by omshiv, if symbol is invalid the send blank response of sec master object
                                    String requestedSymbol = sbmtGetDataReq.instruments.instrument[i].id;
                                    if (sbmtGetDataReq.instruments.instrument[i].type == InstrumentType.TICKER)
                                    {
                                        requestedSymbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey.ToString().ToUpper();
                                    }
                                    SendInvalidSecurityResponse(requestedSymbol, sbmtGetDataReq.instruments.instrument[i].type, "Invalid Security");
                                    continue;
                                }
                                String symbolNotValidated = CheckRequestSameAsResponse(secMasterBaseObj, sbmtGetDataReq.instruments.instrument[i]);
                                if (!String.IsNullOrWhiteSpace(symbolNotValidated))
                                {
                                    String requestedSymbol = sbmtGetDataReq.instruments.instrument[i].id;
                                    if (sbmtGetDataReq.instruments.instrument[i].type == InstrumentType.TICKER)
                                    {
                                        requestedSymbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey.ToString().ToUpper();
                                    }
                                    SendInvalidSecurityResponse(requestedSymbol, sbmtGetDataReq.instruments.instrument[i].type, "Security format not supported, response from Bloomberg for same symbol : " + symbolNotValidated);
                                    continue;
                                }
                                if (secMasterBaseObj.AssetCategory == AssetCategory.Equity || secMasterBaseObj.AssetCategory == AssetCategory.Indices || secMasterBaseObj.AssetCategory == AssetCategory.PrivateEquity)
                                {
                                    if (ApplicationConstants.LoggingEnabled && (secMasterBaseObj.AUECID > 0))
                                    {
                                        LogDataFromBB("BLOOMBERG Basic Response: Symbol:" + secMasterBaseObj.TickerSymbol + ", Data:" + LoggingHelper.ToLoggerString(secMasterBaseObj));
                                    }
                                    BusinessObjects.Data obj = new BusinessObjects.Data();
                                    obj.SecMasterData = secMasterBaseObj;
                                    SymbolDataResponse(this, obj);
                                }
                                else
                                {
                                    _symbolsRequestedToBBForAdditionalData.TryAdd(sbmtGetDataReq.instruments.instrument[i].id, secMasterBaseObj.BBGID);
                                    // SpawnResponseThreadForAdditionalSecurityInfo(symbData);
                                    SpawnResponseThreadForAdditionalSecurityInfo(secMasterBaseObj);
                                }
                                //need to handle the additional request and maintain cache for symbols. Also need to handle the conflict of symbol sent in response being different from the requested symbol
                                _symbolsRequestedToBB[sbmtGetDataReq.instruments.instrument[i].type].Remove(sbmtGetDataReq.instruments.instrument[i].id);
                            }
                            //Use result in some way. May be we can log for information
                            _pendingReqRespForInitSecurityInfo.TryRemove(requestIDRespIDPair.Key, out tempRespID);

                            // Log response of requested symbols from Bloomberg

                            //DataTable dtLog = CreateDataTableForLogging(rtrvGtDrResp.instrumentDatas.ToString(), System.DateTime.Now, "Response", rtrvGtDrResp.instrumentDatas.ToString());
                            //LogRequestResponseDataFromBB(dtLog);
                        }
                        else if (rtrvGtDrResp.statusCode.code == REQUEST_ERROR)
                        {
                            #region Log requested symbols from Bloomberg
                            SubmitGetDataRequest sbmtGetDataReq = requestIDRespIDPair.Value;
                            for (int i = 0; i < sbmtGetDataReq.instruments.instrument.Length; i++)
                            {
                                string symbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data",
                                new XElement("InstrumentType", sbmtGetDataReq.instruments.instrument[i].type),
                                new XElement("FieldValues", sbmtGetDataReq.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Simple Get Data Init Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                            }
                            #endregion
                            try
                            {
                                throw new Exception("Bloomberg resulted in request error when it requested data for response id " + requestIDRespIDPair.Key);
                            }
                            catch (Exception ex)
                            {
                                // Invoke our policy that is responsible for making sure no secure information
                                // gets out of our layer.
                                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                                if (rethrow)
                                {
                                    throw;
                                }
                            }
                            SubmitGetDataRequest tempSubmitDataRequest;
                            _pendingReqRespForInitSecurityInfo.TryRemove(requestIDRespIDPair.Key, out tempSubmitDataRequest);
                        }
                        else if (rtrvGtDrResp.statusCode.code == DATA_NOT_AVAILABLE)
                        {
                            //TODO
                        }
                        else
                        {
                            #region Log requested symbols from Bloomberg
                            SubmitGetDataRequest sbmtGetDataReq = requestIDRespIDPair.Value;
                            for (int i = 0; i < sbmtGetDataReq.instruments.instrument.Length; i++)
                            {
                                string symbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data",
                                new XElement("InstrumentType", sbmtGetDataReq.instruments.instrument[i].type),
                                new XElement("FieldValues", sbmtGetDataReq.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Simple Get Data Init Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                    }
                            #endregion
                        }
                    }
                    System.Threading.Thread.Sleep(POLL_INTERVAL);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static string CheckRequestSameAsResponse(SecMasterBaseObj secMasterBaseObj, Instrument instrument)
        {
            try
            {
                switch (instrument.type)
                {
                    case InstrumentType.BB_GLOBAL: if (String.Compare(secMasterBaseObj.BBGID, instrument.id, true) == 0)
                            return String.Empty;
                        else
                        {
                            if (String.IsNullOrWhiteSpace(secMasterBaseObj.BBGID))
                                return "BBGID Not returned. BloombergTicker: " + secMasterBaseObj.BloombergSymbol;
                            else
                                return secMasterBaseObj.BBGID;
                        }
                    case InstrumentType.TICKER:
                        if (String.Compare(secMasterBaseObj.RequestedSymbol, instrument.id.Trim() + " " + instrument.yellowkey.ToString(), true) == 0)
                            return String.Empty;
                        else
                        {
                            if (String.IsNullOrWhiteSpace(secMasterBaseObj.RequestedSymbol))
                                return "Bloomberg ticker not returned. BBGID: " + secMasterBaseObj.BBGID;
                            else
                                return secMasterBaseObj.RequestedSymbol;
                        }
                    default: if (String.Compare(secMasterBaseObj.RequestedSymbol, instrument.id, true) == 0)
                            return String.Empty;
                        else
                        {
                            if (String.IsNullOrWhiteSpace(secMasterBaseObj.RequestedSymbol))
                                return instrument.type + " not returned. BBGID: " + secMasterBaseObj.BBGID;
                            else
                                return secMasterBaseObj.RequestedSymbol;
                        }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// Send InvalidSecurity Response in security is invalid
        /// Created by omshiv, Jun 2014
        /// </summary>
        /// <param name="instrOriginalRequest"></param>
        private void SendInvalidSecurityResponse(string symbol, InstrumentType type, string commentToAttachAndShow)
        {
            try
            {
                //Creating a blank sec master object to send as invalid security - omshiv
                //AUEC id =0, means invalid security
                SecMasterBaseObj secMasterBaseObj = new SecMasterEquityObj();
                GetSecmasterBaseObjForSecurityType(out secMasterBaseObj, "Common Stock", "Common Stock");
                secMasterBaseObj.AUECID = 0;

                secMasterBaseObj.RequestedSymbology = GetNirvanaSymbologyFromInstrumentType(type);



                switch (type)
                {

                    case InstrumentType.CUSIP:
                        secMasterBaseObj.CusipSymbol = symbol;
                        break;
                    case InstrumentType.ISIN:
                        secMasterBaseObj.ISINSymbol = symbol;
                        break;
                    case InstrumentType.SEDOL:
                        secMasterBaseObj.SedolSymbol = symbol;
                        break;
                    default:
                        secMasterBaseObj.BloombergSymbol = symbol;
                        break;
                }

                //If Asset class in cusip then we setting ticker to cusip else BB symbol
                if (secMasterBaseObj.AssetCategory == AssetCategory.FixedIncome)
                {
                    secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.CusipSymbol);
                    secMasterBaseObj.PrimarySymbology = (int)ApplicationConstants.SymbologyCodes.CUSIPSymbol;
                }
                else
                {
                    secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.BloombergSymbol);
                    secMasterBaseObj.PrimarySymbology = (int)ApplicationConstants.SymbologyCodes.BloombergSymbol;
                }


                secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.ReutersSymbol);

                secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.ISINSymbol);
                secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.SedolSymbol);
                secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.CusipSymbol);
                secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.BloombergSymbol);
                secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.OSIOptionSymbol);
                secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.IDCOOptionSymbol);
                secMasterBaseObj.SymbologyMapping.Add(secMasterBaseObj.OpraSymbol);
                secMasterBaseObj.Comments = commentToAttachAndShow;// "Invalid Symbol";
                LogDataFromBB(commentToAttachAndShow + " BB Response: Symbol:" + symbol);
                BusinessObjects.Data obj = new BusinessObjects.Data();
                obj.SecMasterData = secMasterBaseObj;
                SymbolDataResponse(this, obj);

                //secMasterBaseObj.
                // return invalidResponse;//Handle the part in which the symbol is not validated by bloomberg itself 
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Thread which send requests for initial security Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RequestInitSecurityInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SubmitGetDataRequest sbmtGtDtReq = e.Argument as SubmitGetDataRequest;
                SubmitGetDataResponse sbmtGtDtResp = null;
                try
                {
                    sbmtGtDtResp = RetryHelper.Do<SubmitGetDataRequest, SubmitGetDataResponse>(_perSecurityRef.submitGetDataRequest, TimeSpan.FromSeconds(2), sbmtGtDtReq, 2);
                }
                catch (Exception ex)
                {
                    foreach (Instrument inst in sbmtGtDtReq.instruments.instrument)
                    {
                        SendInvalidSecurityResponse(inst.id, inst.type, "Security Not validated, Bloomberg cannot be reached");
                    }
                    //SendInvalidSecurityResponse(inst.id, inst.type, "Security Not validated, Bloomberg cannot be reached");
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw;
                    }
                }

                if (sbmtGtDtResp == null)
                    return;
                #region Log requested symbols from Bloomberg
                foreach (Instrument inst in sbmtGtDtReq.instruments.instrument)
                {
                    string symbol = inst.id + " " + inst.yellowkey;
                    symbol = symbol.Trim();
                    XElement elem = new XElement("Data", new XElement("InstrumentType", inst.type),
                        new XElement("Fields", sbmtGtDtReq.fields.Select(x => new XElement("Field", x))),
                        new XElement("RequestId", sbmtGtDtResp.requestId),
                        new XElement("ResponseId", sbmtGtDtResp.responseId),
                        new XElement("StatusCode", sbmtGtDtResp.statusCode.code),
                        new XElement("StatusDescription", sbmtGtDtResp.statusCode.description));
                    var reader = elem.CreateReader();
                    reader.MoveToContent();
                    DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Simple Get Data Init Request", reader.ReadInnerXml());
                    LogRequestResponseDataFromBB(dtLog);
                }
                #endregion
                //SubmitGetDataResponse sbmtGtDtResp = _perSecurityRef.submitGetDataRequest(sbmtGtDtReq);
                _pendingReqRespForInitSecurityInfo.TryAdd(sbmtGtDtResp.responseId, sbmtGtDtReq);
                if (!_isResponseThreadForInitSecurityInfoRunning)
                {
                    BackgroundWorker responseThreadForInitSecurityInfo = new BackgroundWorker();
                    responseThreadForInitSecurityInfo.DoWork += responseThreadForInitSecurityInfo_DoWork;
                    responseThreadForInitSecurityInfo.RunWorkerCompleted += responseThreadForInitSecurityInfo_RunWorkerCompleted;
                    _isResponseThreadForInitSecurityInfoRunning = true;
                    responseThreadForInitSecurityInfo.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region IHistoricalAdapter Members

        public event EventHandler<BusinessObjects.Data> SymbolDataResponse;

        public event EventHandler<ObjectParamEventArg> HistoricalDataResponse;

        /// <summary>
        /// The first function which is called to get data from Bloomberg 
        /// </summary>
        /// <param name="symbols"></param>
        /// <param name="symbologyCode"></param>
        /// public void GetSymbolData(HashSet<string> symbols, ApplicationConstants.SymbologyCodes symbologyCode)
        public void GetSymbolData(List<SymbolDataRow> SymbolDataRowCollection)
        {
            try
            {
                StringBuilder requestedSymbols = new StringBuilder();


                if (_perSecurityRef == null)
                {
                    return;
                }
                GetDataHeaders getDataHeaders = new GetDataHeaders();
                getDataHeaders.secmaster = true;
                getDataHeaders.secmasterSpecified = true;
                getDataHeaders.closingvalues = true;
                getDataHeaders.closingvaluesSpecified = true;
                getDataHeaders.derived = true;
                getDataHeaders.derivedSpecified = true;

                List<Instrument> tickerInstrument = new List<Instrument>();
                foreach (SymbolDataRow symbolRow in SymbolDataRowCollection)
                {
                    InstrumentType instTyp = InstrumentType.TICKER;
                    ///TODO: PKE add generic support for all types of instruments i.e. symbology codes

                    //Modified by omshiv, if BBGID is available then send request only for BBGID 
                    Instrument instrument = new Instrument();

                    String symbol = symbolRow.PrimarySymbol;

                    //Modified by omshiv, if BBGID is available then send request only for BBGID 
                    if (!String.IsNullOrWhiteSpace(symbolRow.BBGID))
                    {
                        symbol = symbolRow.BBGID;
                        instTyp = InstrumentType.BB_GLOBAL;
                    }
                    else
                    {
                        ApplicationConstants.SymbologyCodes symbology = (ApplicationConstants.SymbologyCodes)Enum.ToObject(typeof(ApplicationConstants.SymbologyCodes), symbolRow.PrimarySymbology);
                        instTyp = GetInstrumentTypeFromSymbologyCode(symbology);
                    }

                    instrument.typeSpecified = true;
                    instrument.type = instTyp;
                    if (instTyp == InstrumentType.TICKER)
                    {
                        instrument.yellowkeySpecified = true;
                        string[] tick = symbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        MarketSector mSec;
                        if (tick.Length > 1)
                        {
                            if (!Enum.TryParse<MarketSector>(tick[tick.Length - 1].Trim().ToUpper(), true, out mSec))
                            {
                                if (ApplicationConstants.LoggingEnabled)
                                {
                                    LogDataFromBB("Requesting Data for symbols: " + symbol);
                                }
                                SendInvalidSecurityResponse(symbol, instTyp, "Invalid Security");
                                continue;
                            }
                        }
                        else
                            continue;
                        instrument.yellowkey = mSec;
                        tick = tick.Take(tick.Length - 1).ToArray<string>();
                        instrument.id = String.Join(" ", tick);
                        #region checksAndAddsInCache
                        if (_symbolsRequestedToBB.ContainsKey(instTyp))
                        {
                            if (_symbolsRequestedToBB[instTyp].Contains(instrument.id))
                            {
                                continue;
                            }
                            else
                            {
                                _symbolsRequestedToBB[instTyp].Add(instrument.id);
                            }
                        }
                        else
                        {
                            _symbolsRequestedToBB.TryAdd(instTyp, new HashSet<string>() { instrument.id });
                        }

                        if (_symbolsRequestedToBBForAdditionalData.ContainsKey(instrument.id))
                        {

                            continue;
                        }
                        #endregion
                    }
                    else
                    {
                        instrument.id = symbol;
                        #region checksAndAddsInCache

                        if (_symbolsRequestedToBB.ContainsKey(instTyp))
                        {
                            if (_symbolsRequestedToBB[instTyp].Contains(symbol))
                            {
                                continue;
                            }
                            else
                            {
                                _symbolsRequestedToBB[instTyp].Add(symbol);
                            }
                        }
                        else
                        {
                            _symbolsRequestedToBB.TryAdd(instTyp, new HashSet<string>() { symbol });
                        }
                        if (_symbolsRequestedToBBForAdditionalData.ContainsKey(instrument.id))
                        {
                            continue;
                        }
                        #endregion
                    }
                    requestedSymbols.Append(" Symbology : " + instTyp.ToString() + ", Symbol: " + symbol + " ; ");
                    tickerInstrument.Add(instrument);
                }



                //string[] ticker = symbols.ToArray<string>();
                //  InstrumentType instTyp = InstrumentType.TICKER;
                ///TODO: PKE add generic support for all types of instruments i.e. symbology codes
                // instTyp = GetInstrumentTypeFromSymbologyCode(symbologyCode);
                // List<Instrument> tickerInstrument = new List<Instrument>();
                // for (int i = 0; i < ticker.Length; i++)
                // {

                if (tickerInstrument.Count == 0)
                {
                    return;
                }
                if (ApplicationConstants.LoggingEnabled)
                {
                    LogDataFromBB("Requesting Data for symbols: " + requestedSymbols.ToString());
                }
                SubmitGetDataRequest sbmtGtDtReq = new SubmitGetDataRequest();
                sbmtGtDtReq.headers = getDataHeaders;
                //string[] fields = new string[] { "SECURITY_TYP", "MARKET_SECTOR_DES" };
                string[] fields = new string[]{ "NAME",
                                            "ID_BB_SEC_NUM_DES",
                                            "ID_BB_GLOBAL",
                                            "EXCH_CODE",
                                            "SECURITY_TYP",
                                            "MARKET_SECTOR_DES",
                                            "CRNCY",
                                            "ID_CUSIP",
                                            "LONG_COMP_NAME",
                                            "ID_ISIN",
                                            "ID_SEDOL1",
                                            "INDUSTRY_SECTOR",
                                            "INDUSTRY_GROUP",
                                           // "INDUSTRY_SECTOR_NUM",
                                            //"INDUSTRY_GROUP_NUM",
                                            "COUNTRY"
                                            };
                sbmtGtDtReq.fields = fields;
                Instruments instrs = new Instruments();
                instrs.instrument = tickerInstrument.ToArray();
                sbmtGtDtReq.instruments = instrs;

                //ConsoleWriteLine("Submit Get Data Request");
                ///TODO:PKE Maybe we need to replace it with thread pool if lot of symbols requested in different requests
                BackgroundWorker backGroundWorkerGetInitialSecurityInfo = new BackgroundWorker();
                backGroundWorkerGetInitialSecurityInfo.DoWork += new DoWorkEventHandler(RequestInitSecurityInfo_DoWork);
                backGroundWorkerGetInitialSecurityInfo.RunWorkerAsync(sbmtGtDtReq);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_perSecurityRef != null)
                _perSecurityRef.Dispose();
        }

        #endregion

        /// <summary>
        /// Creates a datatable structure for list of fields and adds symbol and date to it
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        static DataTable GetDataTableFromList(List<string> fields, string tableName)
        {
            DataTable dt = new DataTable(tableName);
            try
            {
                foreach (string field in fields)
                {
                    if (!dt.Columns.Contains(field))
                        dt.Columns.Add(field);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dt;
        }

        private static void HistPricingDataTableFromResp(string symbolPkToFill, string secondaryPricingSource, string[] fieldsRequested, HistInstrumentData histInstrumentDataReturned, Instrument instrumentSent, ref DataTable histPricingTable)
        {
            try
            {
                if (histPricingTable == null)
                {
                    List<string> fieldsList = new List<string>();
                    foreach (string fiel in fieldsRequested)
                    {
                        string temp = HistoricalFields.Instance.GetNirvanaFieldFromBloombergField(fiel);
                        if (temp != PricingDataType.Undefined.ToString())
                            fieldsList.Add(temp);
                    }
                    fieldsList.Add("Symbol");
                    fieldsList.Add("Date");
                    fieldsList.Add("Symbology");
                    fieldsList.Add("SymbolPK");
                    //modified by omshiv, added SecondarySource columns
                    fieldsList.Add("SecondarySource");
                    histPricingTable = GetDataTableFromList(fieldsList, "Historical Prices");
                }
                //if (!String.IsNullOrWhiteSpace(returnedFieldsValue["ID_BB_SEC_NUM_DES"]))
                //{ }
                DataRow dr = histPricingTable.NewRow();
                switch (instrumentSent.type)
                {
                    case InstrumentType.TICKER:
                        //modified by omshiv, removing pricing source from Symbol after getting response from BB if fetching data based on secondary pricing source
                        if (!string.IsNullOrWhiteSpace(secondaryPricingSource) && instrumentSent.id.IndexOf(secondaryPricingSource) > 0)
                        {
                            instrumentSent.id = instrumentSent.id.Replace(secondaryPricingSource, string.Empty);
                        }

                        dr["Symbol"] = instrumentSent.id + " " + instrumentSent.yellowkey;

                        break;
                    default: dr["Symbol"] = instrumentSent.id;
                        break;
                }
                dr["SymbolPK"] = symbolPkToFill;
                dr["Symbology"] = GetNirvanaSymbologyFromInstrumentType(instrumentSent.type).ToString();
                dr["Date"] = histInstrumentDataReturned.date.ToString();
                dr["SecondarySource"] = secondaryPricingSource;
                for (int j = 0; j < histInstrumentDataReturned.data.Length; j++)
                {
                    if (String.Compare(histInstrumentDataReturned.data[j].value.Trim(), "N.A.", true) == 0 || String.Compare(histInstrumentDataReturned.data[j].value.Trim(), "N.D.", true) == 0)
                        dr[HistoricalFields.Instance.GetNirvanaFieldFromBloombergField(fieldsRequested[j]).ToString()] = "";
                    else
                        dr[HistoricalFields.Instance.GetNirvanaFieldFromBloombergField(fieldsRequested[j]).ToString()] = histInstrumentDataReturned.data[j].value;
                }
                //modified by - puneet
                histPricingTable.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void GetHistoricalMarkprice(string requestID, string PricingSource, List<SymbolDataRow> symbolsRowCollection, List<string> fields, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (_perSecurityRef == null)
                {
                    return;
                }
                GetHistoryHeaders getHistHeaders = new GetHistoryHeaders();
                DateRange dtRange = new DateRange();
                dtRange.period = new Period();
                dtRange.period.start = startDate;
                dtRange.period.end = endDate;
                getHistHeaders.daterange = dtRange;
                getHistHeaders.version = Prana.BlpDLWSAdapter.PerSecurityWSDL.Version.@new;

                // Purpose : Handling pricing source other than "BGN"
                if (!string.IsNullOrWhiteSpace(PricingSource))
                {
                    SecondarySource source = (SecondarySource)Enum.Parse(typeof(SecondarySource), PricingSource, true);
                    switch (source)
                    {
                        case SecondarySource.BVAL:
                            getHistHeaders.pricing_source = "BVAL";
                            break;
                    }
                }

                InstrumentType instTyp = InstrumentType.TICKER;
                ///TODO: PKE add generic support for all types of instruments i.e. symbology codes
                List<Instrument> instrumentsToSend = new List<Instrument>();
                List<string> instrumentsSymbolPK = new List<string>();
                foreach (SymbolDataRow symbolDataRow in symbolsRowCollection)
                {
                    string symbol = symbolDataRow.PrimarySymbol;
                    instTyp = GetInstrumentTypeFromSymbologyCode((ApplicationConstants.SymbologyCodes)symbolDataRow.PrimarySymbology);
                    if (instTyp == InstrumentType.AUSTRIAN)
                        continue;
                    Instrument instrument = new Instrument();
                    instrument.typeSpecified = true;
                    instrument.type = instTyp;
                    if (instTyp == InstrumentType.TICKER)
                    {
                        instrument.yellowkeySpecified = true;
                        string[] tick = symbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        MarketSector mSec;
                        if (tick.Length > 1)
                        {
                            if (!Enum.TryParse<MarketSector>(tick[tick.Length - 1], true, out mSec))
                                continue;

                        }
                        else
                            continue;
                        instrument.yellowkey = mSec;
                        //modified by omshiv, added SecondarySource in symbol to fetch data based on secondary source.
                        if (!String.IsNullOrWhiteSpace(PricingSource))
                        {
                            List<string> ticks = new List<string>(tick.Take(tick.Length - 1));
                            ticks.Add(PricingSource);
                            tick = tick = ticks.ToArray();
                        }
                        else
                        {
                            tick = tick.Take(tick.Length - 1).ToArray<string>();
                        }
                        instrument.id = String.Join(" ", tick);
                    }
                    else
                    {
                        instrument.id = symbol;
                    }
                    instrumentsSymbolPK.Add(symbolDataRow.Symbol_PK.ToString());
                    instrumentsToSend.Add(instrument);
                }
                if (instrumentsToSend.Count == 0)
                {
                    return;
                }
                List<string> fieldsList = new List<string>();
                foreach (string fiel in fields)
                {
                    string temp = HistoricalFields.Instance.GetBloombergFieldForNirvanaField(fiel);
                    if (!String.IsNullOrWhiteSpace(temp))
                        fieldsList.Add(temp);
                }

                SubmitGetHistoryRequest sbmtGtHistReq = new SubmitGetHistoryRequest();
                sbmtGtHistReq.headers = getHistHeaders;
                Instruments instrs = new Instruments();
                instrs.instrument = instrumentsToSend.ToArray();
                sbmtGtHistReq.instruments = instrs;
                sbmtGtHistReq.fields = fieldsList.ToArray();

                BackgroundWorker bkGndWrkrGetHistoricalPrices = new BackgroundWorker();
                bkGndWrkrGetHistoricalPrices.DoWork += new DoWorkEventHandler(bkGndWrkrGetHistoricalPrices_DoWork);
                bkGndWrkrGetHistoricalPrices.RunWorkerAsync(new Tuple<SubmitGetHistoryRequest, string, string[], string>(sbmtGtHistReq, requestID, instrumentsSymbolPK.ToArray(), PricingSource));
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets updated data for generic pricing fields from BB for current date
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="PricingSource"></param>
        /// <param name="symbolsRowCollection"></param>
        /// <param name="fields"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void GetCurrentMarkprice(string requestID, string PricingSource, List<SymbolDataRow> symbolsRowCollection, List<string> fields)
        {
            try
            {
                if (_perSecurityRef == null)
                {
                    return;
                }
                GetDataHeaders getDataHeaders = new GetDataHeaders(); ;
                getDataHeaders.version = Prana.BlpDLWSAdapter.PerSecurityWSDL.Version.@new;
                getDataHeaders.secmaster = true;
                getDataHeaders.secmasterSpecified = true;
                getDataHeaders.closingvalues = true;
                getDataHeaders.closingvaluesSpecified = true;
                getDataHeaders.derived = true;
                getDataHeaders.derivedSpecified = true;

                InstrumentType instTyp = InstrumentType.TICKER;
                ///TODO: PKE add generic support for all types of insruments i.e. symbology codes
                List<Instrument> instrumentsToSend = new List<Instrument>();
                List<string> instrumentsSymbolPK = new List<string>();
                foreach (SymbolDataRow symbolDataRow in symbolsRowCollection)
                {
                    string symbol = symbolDataRow.PrimarySymbol;
                    instTyp = GetInstrumentTypeFromSymbologyCode((ApplicationConstants.SymbologyCodes)symbolDataRow.PrimarySymbology);
                    if (instTyp == InstrumentType.AUSTRIAN)
                        continue;
                    Instrument instrument = new Instrument();
                    instrument.typeSpecified = true;
                    instrument.type = instTyp;
                    if (instTyp == InstrumentType.TICKER)
                    {
                        instrument.yellowkeySpecified = true;
                        string[] tick = symbol.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        MarketSector mSec;
                        if (tick.Length > 1)
                        {
                            if (!Enum.TryParse<MarketSector>(tick[tick.Length - 1], true, out mSec))
                                continue;
                        }
                        else
                            continue;
                        instrument.yellowkey = mSec;

                        if (!String.IsNullOrWhiteSpace(PricingSource))
                        {
                            List<string> ticks = new List<string>(tick.Take(tick.Length - 1));
                            ticks.Add(PricingSource);
                            tick = tick = ticks.ToArray();
                        }
                        else
                        {
                            tick = tick.Take(tick.Length - 1).ToArray<string>();
                        }
                        instrument.id = String.Join(" ", tick);
                    }
                    else
                    {
                        instrument.id = symbol;
                    }
                    instrumentsSymbolPK.Add(symbolDataRow.Symbol_PK.ToString());
                    instrumentsToSend.Add(instrument);
                }
                if (instrumentsToSend.Count == 0)
                {
                    return;
                }
                List<string> fieldsList = new List<string>();
                foreach (string fiel in fields)
                {
                    string temp = HistoricalFields.Instance.GetBloombergFieldForNirvanaField(fiel);
                    if (!String.IsNullOrWhiteSpace(temp))
                        fieldsList.Add(temp);
                }

                SubmitGetDataRequest sbmtGtCurrReq = new SubmitGetDataRequest();
                sbmtGtCurrReq.headers = getDataHeaders;
                Instruments instrs = new Instruments();
                instrs.instrument = instrumentsToSend.ToArray();
                sbmtGtCurrReq.instruments = instrs;
                sbmtGtCurrReq.fields = fieldsList.ToArray();

                BackgroundWorker bkGndWrkrGetCurrentPrices = new BackgroundWorker();
                bkGndWrkrGetCurrentPrices.DoWork += new DoWorkEventHandler(bkGndWrkrGetCurrentPrices_DoWork);

                bkGndWrkrGetCurrentPrices.RunWorkerAsync(new Tuple<SubmitGetDataRequest, string, string[], string>(sbmtGtCurrReq, requestID, instrumentsSymbolPK.ToArray(), PricingSource));
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }



        //Handeling request and response  for current date request
        private void bkGndWrkrGetCurrentPrices_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Tuple<SubmitGetDataRequest, string, string[], string> arguments = e.Argument as Tuple<SubmitGetDataRequest, string, string[], string>;
                if (arguments == null)
                    return;
                String reqID = arguments.Item2;
                SubmitGetDataRequest sbmtGtHistReq = arguments.Item1;
                string[] symbolPkArray = arguments.Item3;
                string secondaryPricingSource = arguments.Item4;
                StringBuilder sb = new StringBuilder();
                arguments.Item1.instruments.instrument.Select(x => sb.Append(x.id));
                if (ApplicationConstants.LoggingEnabled)
                {
                    LogDataFromBB("BLOOMBERG Current Data Request:" + Environment.NewLine + "RequestID: " + Newtonsoft.Json.JsonConvert.SerializeObject(arguments.Item2) + Environment.NewLine + " SymbolPKs requested for " + Newtonsoft.Json.JsonConvert.SerializeObject(arguments.Item3) + Environment.NewLine + "Symbols Requested: " + sb.ToString() + Environment.NewLine + " Historical Data Request: " + Newtonsoft.Json.JsonConvert.SerializeObject(arguments.Item1));
                }
                SubmitGetDataResponse sbmtGtCurResp = null;
                try
                {
                    sbmtGtCurResp = RetryHelper.Do<SubmitGetDataRequest, SubmitGetDataResponse>(_perSecurityRef.submitGetDataRequest, TimeSpan.FromSeconds(2), sbmtGtHistReq, 2);
                }
                catch (Exception ex)
                {
                    ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                    histPricingEventarg.Arguments = new Object[] { null, reqID, false, "Bloomberg cannot be reached" };
                    HistoricalDataResponse(this, histPricingEventarg);
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw;
                    }
                    return;
                }
                //SubmitGetHistoryResponse sbmtGtHisResp = _perSecurityRef.submitGetHistoryRequest(sbmtGtHistReq);
                if (sbmtGtCurResp == null)
                {
                    ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                    histPricingEventarg.Arguments = new Object[] { null, reqID, false, "Bloomberg cannot be reached" };
                    HistoricalDataResponse(this, histPricingEventarg);
                    return;
                }
                #region Log requested symbols from Bloomberg
                foreach (Instrument inst in sbmtGtHistReq.instruments.instrument)
                {
                    string symbol = inst.id + " " + inst.yellowkey;
                    symbol = symbol.Trim();
                    XElement elem = new XElement("Data", new XElement("InstrumentType", inst.type),
                                        new XElement("Fields", sbmtGtHistReq.fields.Select(x => new XElement("Field", x))),
                                        new XElement("RequestId", sbmtGtCurResp.requestId),
                                        new XElement("ResponseId", sbmtGtCurResp.responseId),
                                        new XElement("StatusCode", sbmtGtCurResp.statusCode.code),
                                        new XElement("StatusDescription", sbmtGtCurResp.statusCode.description));
                    var reader = elem.CreateReader();
                    reader.MoveToContent();
                    DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Current Pricing Data Request", elem.ToString());
                    LogRequestResponseDataFromBB(dtLog);
                }
                #endregion

                _pendingReqRespForCurMarkPrice.TryAdd(sbmtGtCurResp.responseId, sbmtGtHistReq);
                _mappingReqIDRespForHistMarkPrice.TryAdd(sbmtGtCurResp.responseId, reqID);
                _mappingRespIDSymbolPkForHistMarkPrice.TryAdd(sbmtGtCurResp.responseId, new Tuple<string[], string>(symbolPkArray, secondaryPricingSource));

                if (!_isResponseThreadForCurrentMarkPrices)
                {
                    BackgroundWorker respThreadForCurrMarkPrcInfo = new BackgroundWorker();
                    respThreadForCurrMarkPrcInfo.DoWork += respThreadForCurrMarkPrcInfo_DoWork;
                    respThreadForCurrMarkPrcInfo.RunWorkerCompleted += respThreadForCurrMarkPrcInfo_RunWorkerCompleted;
                    _isResponseThreadForCurrentMarkPrices = true;
                    respThreadForCurrMarkPrcInfo.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void respThreadForCurrMarkPrcInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                _isResponseThreadForCurrentMarkPrices = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkGndWrkrGetHistoricalPrices_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Tuple<SubmitGetHistoryRequest, string, string[], string> arguments = e.Argument as Tuple<SubmitGetHistoryRequest, string, string[], string>;
                if (arguments == null)
                    return;
                String reqID = arguments.Item2;
                SubmitGetHistoryRequest sbmtGtHistReq = arguments.Item1;
                string[] symbolPkArray = arguments.Item3;
                //modified by omshiv, added secondary source in argument 
                string secondaryPricingSource = arguments.Item4;
                StringBuilder sb = new StringBuilder();
                arguments.Item1.instruments.instrument.Select(x => sb.Append(x.id));
                if (ApplicationConstants.LoggingEnabled)
                {
                    LogDataFromBB("BLOOMBERG Historical Data Request:" + Environment.NewLine + "RequestID: " + Newtonsoft.Json.JsonConvert.SerializeObject(arguments.Item2) + Environment.NewLine + " SymbolPKs requested for " + Newtonsoft.Json.JsonConvert.SerializeObject(arguments.Item3) + Environment.NewLine + "Symbols Requested: " + sb.ToString() + Environment.NewLine + " Historical Data Request: " + Newtonsoft.Json.JsonConvert.SerializeObject(arguments.Item1));
                }
                SubmitGetHistoryResponse sbmtGtHisResp = null;
                try
                {
                    sbmtGtHisResp = RetryHelper.Do<SubmitGetHistoryRequest, SubmitGetHistoryResponse>(_perSecurityRef.submitGetHistoryRequest, TimeSpan.FromSeconds(2), sbmtGtHistReq, 2);
                }
                catch (Exception ex)
                {
                    ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                    histPricingEventarg.Arguments = new Object[] { null, reqID, false, "Bloomberg cannot be reached" };
                    HistoricalDataResponse(this, histPricingEventarg);
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                    if (rethrow)
                    {
                        throw;
                    }
                    return;
                }
                //SubmitGetHistoryResponse sbmtGtHisResp = _perSecurityRef.submitGetHistoryRequest(sbmtGtHistReq);
                if (sbmtGtHisResp == null)
                {
                    ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                    histPricingEventarg.Arguments = new Object[] { null, reqID, false, "Bloomberg cannot be reached" };
                    HistoricalDataResponse(this, histPricingEventarg);
                    return;
                }
                #region Log requested symbols from Bloomberg
                foreach (Instrument inst in sbmtGtHistReq.instruments.instrument)
                {
                    string symbol = inst.id + " " + inst.yellowkey;
                    symbol = symbol.Trim();
                    XElement elem = new XElement("Data", new XElement("InstrumentType", inst.type),
                    new XElement("Fields", sbmtGtHistReq.fields.Select(x => new XElement("Field", x))),
                    new XElement("Date", sbmtGtHistReq.headers.daterange.period.start),
                    new XElement("RequestId", sbmtGtHisResp.requestId),
                    new XElement("ResponseId", sbmtGtHisResp.responseId),
                    new XElement("StatusCode", sbmtGtHisResp.statusCode.code),
                    new XElement("StatusDescription", sbmtGtHisResp.statusCode.description));
                    var reader = elem.CreateReader();
                    reader.MoveToContent();
                    DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Historical Pricing Data Request", reader.ReadInnerXml());
                    LogRequestResponseDataFromBB(dtLog);
                }
                #endregion
                _pendingReqRespForHistMarkPrice.TryAdd(sbmtGtHisResp.responseId, sbmtGtHistReq);
                _mappingReqIDRespForHistMarkPrice.TryAdd(sbmtGtHisResp.responseId, reqID);
                _mappingRespIDSymbolPkForHistMarkPrice.TryAdd(sbmtGtHisResp.responseId, new Tuple<string[], string>(symbolPkArray, secondaryPricingSource));

                if (!_isResponseThreadForHistoricalMarkPrices)
                {
                    BackgroundWorker respThreadForHisMarkPrcInfo = new BackgroundWorker();
                    respThreadForHisMarkPrcInfo.DoWork += respThreadForHisMarkPrcInfo_DoWork;
                    respThreadForHisMarkPrcInfo.RunWorkerCompleted += respThreadForHisMarkPrcInfo_RunWorkerCompleted;
                    _isResponseThreadForHistoricalMarkPrices = true;
                    respThreadForHisMarkPrcInfo.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void respThreadForHisMarkPrcInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                _isResponseThreadForHistoricalMarkPrices = false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void respThreadForHisMarkPrcInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (_pendingReqRespForHistMarkPrice.Count > 0)
                {
                    SubmitGetHistoryRequest tempRespID;
                    string tempReqId;
                    //modified by omshiv, Added Secondary source with symbol_PK list
                    Tuple<string[], string> tempSymbolPks;
                    foreach (KeyValuePair<string, SubmitGetHistoryRequest> requestIDRespIDPair in _pendingReqRespForHistMarkPrice)
                    {
                        //retrieve get data request. The response ID sent for the request is the response ID
                        //received from SubmitGetDataRequest()
                        RetrieveGetHistoryRequest rtrvGtDrReq = new RetrieveGetHistoryRequest();
                        rtrvGtDrReq.responseId = requestIDRespIDPair.Key;
                        RetrieveGetHistoryResponse rtrvGtDrResp = null;
                        try
                        {
                            rtrvGtDrResp = RetryHelper.Do<RetrieveGetHistoryRequest, RetrieveGetHistoryResponse>(_perSecurityRef.retrieveGetHistoryResponse, TimeSpan.FromSeconds(2), rtrvGtDrReq, 2);
                        }
                        catch (Exception ex)
                        {
                            _mappingReqIDRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempReqId);
                            _mappingRespIDSymbolPkForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempSymbolPks);
                            bool result = _pendingReqRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                            ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                            histPricingEventarg.Arguments = new Object[] { null, tempReqId, false, "Bloomberg cannot be reached" };
                            HistoricalDataResponse(this, histPricingEventarg);

                            // Invoke our policy that is responsible for making sure no secure information
                            // gets out of our layer.
                            bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                            if (rethrow)
                            {
                                throw;
                            }
                            continue;
                        }
                        //rtrvGtDrResp = _perSecurityRef.retrieveGetHistoryResponse(rtrvGtDrReq);
                        if (rtrvGtDrResp == null && rtrvGtDrResp.statusCode == null)
                        {
                            _mappingReqIDRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempReqId);
                            _pendingReqRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                            _mappingRespIDSymbolPkForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempSymbolPks);
                            ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                            histPricingEventarg.Arguments = new Object[] { null, tempReqId, false, "Bloomberg cannot be reached" };
                            HistoricalDataResponse(this, histPricingEventarg);
                            continue;
                        }
                        else if (rtrvGtDrResp.statusCode.code == SUCCESS)
                        {
                            if (ApplicationConstants.LoggingEnabled)
                            {
                                LogDataFromBB("BLOOMBERG Historical Data Response Fields: " + Newtonsoft.Json.JsonConvert.SerializeObject(rtrvGtDrResp.fields));
                                LogDataFromBB("BLOOMBERG Historical Data Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(rtrvGtDrResp.instrumentDatas));
                            }
                            SubmitGetHistoryRequest sbmtGetDataReq = requestIDRespIDPair.Value;
                            string ReqIDFromCentralSM;
                            Tuple<string[], string> SymbolPkToFill;
                            _mappingReqIDRespForHistMarkPrice.TryGetValue(requestIDRespIDPair.Key, out ReqIDFromCentralSM);
                            _mappingRespIDSymbolPkForHistMarkPrice.TryGetValue(requestIDRespIDPair.Key, out SymbolPkToFill);
                            //Displaying the RetrieveGetDataResponse
                            DataTable histPricingData = null;
                            for (int i = 0; i < rtrvGtDrResp.instrumentDatas.Length; i++)
                            {
                                #region Log requested symbols from Bloomberg
                                string symbol = _pendingReqRespForHistMarkPrice[requestIDRespIDPair.Key].instruments.instrument[i].id + " " + _pendingReqRespForHistMarkPrice[requestIDRespIDPair.Key].instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data", new XElement("FieldValues", rtrvGtDrResp.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x), new XAttribute("Value", rtrvGtDrResp.instrumentDatas[i].data[index].value)))),
                                new XElement("Date", rtrvGtDrResp.headers.daterange.period.start),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Historical Pricing Data Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                                #endregion
                                HistPricingDataTableFromResp(SymbolPkToFill.Item1[i], SymbolPkToFill.Item2, rtrvGtDrResp.fields, rtrvGtDrResp.instrumentDatas[i], sbmtGetDataReq.instruments.instrument[i], ref histPricingData);

                                if (histPricingData == null)
                                { ///TODO: PKE some code for symbol not validated.
                                    continue;
                                }
                            }
                            ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                            histPricingEventarg.Arguments = new Object[] { histPricingData, ReqIDFromCentralSM, true, "Request successfully processed" };
                            HistoricalDataResponse(this, histPricingEventarg);
                            //Use result in some way. May be we can log for information
                            _mappingReqIDRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempReqId);
                            _mappingRespIDSymbolPkForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempSymbolPks);
                            bool result = _pendingReqRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                        }
                        else if (rtrvGtDrResp.statusCode.code == REQUEST_ERROR)
                        {
                            #region Log requested symbols from Bloomberg
                            SubmitGetHistoryRequest sbmtGetDataReq = _pendingReqRespForHistMarkPrice[requestIDRespIDPair.Key];
                            for (int i = 0; i < sbmtGetDataReq.instruments.instrument.Length; i++)
                            {
                                string symbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data", new XElement("InstrumentType", sbmtGetDataReq.instruments.instrument[i].type),
                                new XElement("FieldValues", sbmtGetDataReq.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Historical Pricing Data Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                            }
                            #endregion
                            try
                            {
                                throw new Exception("Bloomberg resulted in request error when it requested historical data for response id " + requestIDRespIDPair.Key);
                            }
                            catch (Exception ex)
                            {
                                // Invoke our policy that is responsible for making sure no secure information
                                // gets out of our layer.
                                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                                if (rethrow)
                                {
                                    throw;
                                }
                            }
                            _mappingReqIDRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempReqId);
                            _mappingRespIDSymbolPkForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempSymbolPks);
                            _pendingReqRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                            ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                            histPricingEventarg.Arguments = new Object[] { null, tempReqId, false, "Request error while requesting Bloomberg for historical data" };
                            HistoricalDataResponse(this, histPricingEventarg);
                            continue;
                        }
                        else if (rtrvGtDrResp.statusCode.code == DATA_NOT_AVAILABLE)
                        {
                            //TODO
                    }
                        else
                        {
                            #region Log requested symbols from Bloomberg
                            SubmitGetHistoryRequest sbmtGetDataReq = _pendingReqRespForHistMarkPrice[requestIDRespIDPair.Key];
                            for (int i = 0; i < sbmtGetDataReq.instruments.instrument.Length; i++)
                            {
                                string symbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data", new XElement("InstrumentType", sbmtGetDataReq.instruments.instrument[i].type),
                                new XElement("FieldValues", sbmtGetDataReq.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Historical Pricing Data Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                            }
                            #endregion
                        }
                    }
                    System.Threading.Thread.Sleep(POLL_INTERVAL);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        //Handle Current data mark price requesting to BB
        private void respThreadForCurrMarkPrcInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (_pendingReqRespForCurMarkPrice.Count > 0)
                {
                    SubmitGetDataRequest tempRespID;
                    string tempReqId;
                    Tuple<string[], string> tempSymbolPks;
                    foreach (KeyValuePair<string, SubmitGetDataRequest> requestIDRespIDPair in _pendingReqRespForCurMarkPrice)
                    {
                        //retrieve get data request. The response ID sent for the request is the response ID
                        //received from SubmitGetDataRequest()
                        RetrieveGetDataRequest rtrvGtDrReq = new RetrieveGetDataRequest();
                        rtrvGtDrReq.responseId = requestIDRespIDPair.Key;
                        RetrieveGetDataResponse rtrvGtDrResp = null;
                        try
                        {
                            rtrvGtDrResp = RetryHelper.Do<RetrieveGetDataRequest, RetrieveGetDataResponse>(_perSecurityRef.retrieveGetDataResponse, TimeSpan.FromSeconds(2), rtrvGtDrReq, 2);
                        }
                        catch (Exception ex)
                        {
                            _mappingReqIDRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempReqId);
                            _mappingRespIDSymbolPkForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempSymbolPks);
                            bool result = _pendingReqRespForCurMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                            ObjectParamEventArg curPricingEventArg = new ObjectParamEventArg();
                            curPricingEventArg.Arguments = new Object[] { null, tempReqId, false, "Bloomberg cannot be reached" };
                            HistoricalDataResponse(this, curPricingEventArg);

                            // Invoke our policy that is responsible for making sure no secure information
                            // gets out of our layer.
                            bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                            if (rethrow)
                            {
                                throw;
                            }
                            continue;
                        }
                        //rtrvGtDrResp = _perSecurityRef.retrieveGetHistoryResponse(rtrvGtDrReq);
                        if (rtrvGtDrResp == null && rtrvGtDrResp.statusCode == null)
                        {
                            _mappingReqIDRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempReqId);
                            _pendingReqRespForCurMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                            _mappingRespIDSymbolPkForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempSymbolPks);
                            ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                            histPricingEventarg.Arguments = new Object[] { null, tempReqId, false, "Bloomberg cannot be reached" };
                            HistoricalDataResponse(this, histPricingEventarg);
                            continue;
                        }
                        else if (rtrvGtDrResp.statusCode.code == SUCCESS)
                        {
                            if (ApplicationConstants.LoggingEnabled)
                            {
                                LogDataFromBB("BLOOMBERG Current Price Data Response Fields: " + Newtonsoft.Json.JsonConvert.SerializeObject(rtrvGtDrResp.fields));
                                LogDataFromBB("BLOOMBERG Current Price Data Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(rtrvGtDrResp.instrumentDatas));
                            }
                            SubmitGetDataRequest sbmtGetDataReq = requestIDRespIDPair.Value;
                            string ReqIDFromCentralSM;
                            Tuple<string[], string> SymbolPkToFill;
                            _mappingReqIDRespForHistMarkPrice.TryGetValue(requestIDRespIDPair.Key, out ReqIDFromCentralSM);
                            _mappingRespIDSymbolPkForHistMarkPrice.TryGetValue(requestIDRespIDPair.Key, out SymbolPkToFill);
                            //Displaying the RetrieveGetDataResponse
                            DataTable histPricingData = null;
                            for (int i = 0; i < rtrvGtDrResp.instrumentDatas.Length; i++)
                            {
                                #region Log requested symbols from Bloomberg
                                string symbol = _pendingReqRespForCurMarkPrice[requestIDRespIDPair.Key].instruments.instrument[i].id + " " + _pendingReqRespForCurMarkPrice[requestIDRespIDPair.Key].instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data", new XElement("FieldValues", rtrvGtDrResp.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x), new XAttribute("Value", rtrvGtDrResp.instrumentDatas[i].data[index].value)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Current Pricing Data Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                                #endregion

                                CurPricingDataTableFromResp(SymbolPkToFill.Item1[i], SymbolPkToFill.Item2, rtrvGtDrResp.fields, rtrvGtDrResp.instrumentDatas[i], sbmtGetDataReq.instruments.instrument[i], ref histPricingData);

                                if (histPricingData == null)
                                { ///TODO: PKE some code for symbol not validated.
                                    continue;
                                }
                            }
                            ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                            histPricingEventarg.Arguments = new Object[] { histPricingData, ReqIDFromCentralSM, true, "Request successfully processed" };
                            HistoricalDataResponse(this, histPricingEventarg);
                            //Use result in some way. May be we can log for information
                            _mappingReqIDRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempReqId);
                            _mappingRespIDSymbolPkForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempSymbolPks);
                            bool result = _pendingReqRespForCurMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                        }
                        else if (rtrvGtDrResp.statusCode.code == REQUEST_ERROR)
                        {
                            #region Log requested symbols from Bloomberg
                            SubmitGetDataRequest sbmtGetDataReq = _pendingReqRespForCurMarkPrice[requestIDRespIDPair.Key];
                            for (int i = 0; i < sbmtGetDataReq.instruments.instrument.Length; i++)
                            {
                                string symbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data",
                                new XElement("InstrumentType", sbmtGetDataReq.instruments.instrument[i].type),
                                new XElement("FieldValues", sbmtGetDataReq.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Current Pricing Data Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                            }
                            #endregion
                            try
                            {
                                throw new Exception("Bloomberg resulted in request error when it requested historical data for response id " + requestIDRespIDPair.Key);
                            }
                            catch (Exception ex)
                            {
                                // Invoke our policy that is responsible for making sure no secure information
                                // gets out of our layer.
                                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                                if (rethrow)
                                {
                                    throw;
                                }
                            }
                            _mappingReqIDRespForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempReqId);
                            _mappingRespIDSymbolPkForHistMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempSymbolPks);
                            _pendingReqRespForCurMarkPrice.TryRemove(requestIDRespIDPair.Key, out tempRespID);
                            ObjectParamEventArg histPricingEventarg = new ObjectParamEventArg();
                            histPricingEventarg.Arguments = new Object[] { null, tempReqId, false, "Request error while requesting Bloomberg for current pricing data" };
                            HistoricalDataResponse(this, histPricingEventarg);
                            continue;
                        }
                        else if (rtrvGtDrResp.statusCode.code == DATA_NOT_AVAILABLE)
                        {
                            //TODO
                    }
                        else
                        {
                            #region Log requested symbols from Bloomberg
                            SubmitGetDataRequest sbmtGetDataReq = _pendingReqRespForCurMarkPrice[requestIDRespIDPair.Key];
                            for (int i = 0; i < sbmtGetDataReq.instruments.instrument.Length; i++)
                            {
                                string symbol = sbmtGetDataReq.instruments.instrument[i].id + " " + sbmtGetDataReq.instruments.instrument[i].yellowkey;
                                symbol = symbol.Trim();
                                XElement elem = new XElement("Data",
                                new XElement("InstrumentType", sbmtGetDataReq.instruments.instrument[i].type),
                                new XElement("FieldValues", sbmtGetDataReq.fields.Select((x, index) => new XElement("FieldValue", new XAttribute("Field", x)))),
                                new XElement("RequestId", rtrvGtDrResp.requestId),
                                new XElement("ResponseId", rtrvGtDrResp.responseId),
                                new XElement("StatusCode", rtrvGtDrResp.statusCode.code),
                                new XElement("StatusDescription", rtrvGtDrResp.statusCode.description));
                                var reader = elem.CreateReader();
                                reader.MoveToContent();
                                DataTable dtLog = CreateDataTableForLogging(symbol, System.DateTime.Now, "Current Pricing Data Response", reader.ReadInnerXml());
                                LogRequestResponseDataFromBB(dtLog);
                            }
                            #endregion
                        }
                    }
                    System.Threading.Thread.Sleep(POLL_INTERVAL);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //Getting Price Data table from BB response
        private static void CurPricingDataTableFromResp(string symbolPkToFill, string secondaryPricingSource, string[] fieldsRequested, InstrumentData histInstrumentDataReturned, Instrument instrumentSent, ref DataTable histPricingTable)
        {
            try
            {
                if (histPricingTable == null)
                {
                    List<string> fieldsList = new List<string>();
                    foreach (string fiel in fieldsRequested)
                    {
                        string temp = HistoricalFields.Instance.GetNirvanaFieldFromBloombergField(fiel);
                        if (temp != PricingDataType.Undefined.ToString())
                            fieldsList.Add(temp);
                    }
                    fieldsList.Add("Symbol");
                    fieldsList.Add("Date");
                    fieldsList.Add("Symbology");
                    fieldsList.Add("SymbolPK");
                    fieldsList.Add("SecondarySource");
                    histPricingTable = GetDataTableFromList(fieldsList, "Historical Prices");
                }
                //if (!String.IsNullOrWhiteSpace(returnedFieldsValue["ID_BB_SEC_NUM_DES"]))
                //{ }
                DataRow dr = histPricingTable.NewRow();





                switch (instrumentSent.type)
                {
                    case InstrumentType.TICKER:

                        if (!string.IsNullOrWhiteSpace(secondaryPricingSource) && instrumentSent.id.IndexOf(secondaryPricingSource) > 0)
                        {
                            instrumentSent.id = instrumentSent.id.Replace(secondaryPricingSource, string.Empty);
                        }

                        dr["Symbol"] = instrumentSent.id.Trim() + " " + instrumentSent.yellowkey;

                        break;
                    default: dr["Symbol"] = instrumentSent.id;
                        break;
                }
                dr["SymbolPK"] = symbolPkToFill;
                dr["Symbology"] = GetNirvanaSymbologyFromInstrumentType(instrumentSent.type).ToString();
                dr["Date"] = DateTime.Now.Date;
                dr["SecondarySource"] = secondaryPricingSource;
                for (int j = 0; j < histInstrumentDataReturned.data.Length; j++)
                {
                    if (String.Compare(histInstrumentDataReturned.data[j].value.Trim(), "N.A.", true) == 0 || String.Compare(histInstrumentDataReturned.data[j].value.Trim(), "N.D.", true) == 0)
                        dr[HistoricalFields.Instance.GetNirvanaFieldFromBloombergField(fieldsRequested[j]).ToString()] = "";
                    else
                        dr[HistoricalFields.Instance.GetNirvanaFieldFromBloombergField(fieldsRequested[j]).ToString()] = histInstrumentDataReturned.data[j].value;
                }
                //modified by - puneet
                histPricingTable.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// add symbol retrieval status to xml log
        /// </summary>
        /// <param name="dtLogging"></param>
        static private void LogRequestResponseDataFromBB(DataTable dtLogging)
        {
            try
            {
                string LogDirectoryPath = Application.StartupPath + @"\SecurityLog";
                string currentDate = DateTime.Today.ToString("dd-MM-yyyy");
                string LogFilePath = LogDirectoryPath + @"\" + currentDate + "_" + "DLWS.xml";

                lock (_logLocker)
                {
                if (!Directory.Exists(LogDirectoryPath))
                {
                    Directory.CreateDirectory(LogDirectoryPath);
                }
                if (File.Exists(LogFilePath))
                {
                    DataSet ds = new DataSet();
                    //ds.ReadXml(LogFilePath);
                    ds = XMLUtilities.ReadXmlUsingBufferedStream(LogFilePath);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dtLogging.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    }
                }
                dtLogging.WriteXml(LogFilePath, XmlWriteMode.IgnoreSchema);
            }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Create datatable for requested symbols
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static DataTable CreateDataTableForLogging(string symbol, DateTime date, string type, string data)
        {
            DataTable dtLogging = new DataTable();
            dtLogging.TableName = "SymbolLogging";

            try
            {
                // Create three columns; Symbol, Date, Type and Data.
                dtLogging.Columns.Add(new DataColumn("Symbol", typeof(System.String)));
                dtLogging.Columns.Add(new DataColumn("Date", typeof(System.DateTime)));
                dtLogging.Columns.Add(new DataColumn("SymbolRequestResponseType", typeof(System.String)));
                dtLogging.Columns.Add(new DataColumn("Data", typeof(System.String)));

                dtLogging.Rows.Add(new object[] { symbol, date, type, data });
                dtLogging.AcceptChanges();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dtLogging;
        }

    }
}
