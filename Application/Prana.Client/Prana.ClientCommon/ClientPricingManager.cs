using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.LiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.DateTimeUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Prana.ClientCommon
{
    public class ClientPricingManager : ILiveFeedCallback, IDisposable
    {
        private static object _lockerObject = new object();
        static ClientPricingManager _clientPricingManager = null;
        static List<PricingRule> _priceRuleList = new List<PricingRule>();
        PranaBinaryFormatter binaryFormatter = new PranaBinaryFormatter();
        static List<PricePolicyDetails> _pricingPolicyDetailList = new List<PricePolicyDetails>();
        //static List<PricingPolicyDetailsFromSP> _pricingPolicyDetailFromSP = new List<PricingPolicyDetailsFromSP>();
        //Dictionary<int, Dictionary<string, List<PricingPolicyDetailsFromSP>>> _pricingPolicyDetailsCache = new Dictionary<int, Dictionary<string, List<PricingPolicyDetailsFromSP>>>();

        //  public delegate void DelegatePriceResponse(DataTable dt);
        //  public event DelegatePriceResponse PriceResponseEventHandler;
        public event EventHandler<EventArgs<DataTable>> PriceResponseEventHandler;

        //public delegate void DelegatePriceResponsefromfile(DataTable dt);
        // public event DelegatePriceResponsefromfile PriceResponseEventHandlerfromfile;
        public event EventHandler<EventArgs<DataTable>> PriceResponseEventHandlerfromfile;

        string _yesterdayDateTimeString;

        private DuplexProxyBase<IPricingService> _pricingServiceProxy = null;
        public DuplexProxyBase<IPricingService> PricingServiceProxy
        {
            set
            {
                _pricingServiceProxy = value;
                _pricingServiceProxy.ConnectedEvent += new Proxy<IPricingService>.ConnectionEventHandler(_pricingServiceProxy_ConnectedEvent);
            }
        }

        private Dictionary<int, DateTime> _currentOffsetAdjustedAUECDates;

        public Dictionary<int, DateTime> CurrentOffsetAdjustedAUECDates
        {
            get { return _currentOffsetAdjustedAUECDates; }
            set { _currentOffsetAdjustedAUECDates = value; }
        }

        ISecurityMasterServices _securityMasterServices = null;
        public ISecurityMasterServices SecurityMasterServices
        {
            set
            {
                _securityMasterServices = value;

                if (_securityMasterServices != null)
                {
                    //_securityMasterServices.SMGenericPriceResponse += _securityMasterServices_SMGenericPriceResponse;
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static ClientPricingManager GetInstance
        {
            get
            {

                lock (_lockerObject)
                {
                    if (_clientPricingManager == null)
                    {
                        _clientPricingManager = new ClientPricingManager();

                    }
                    return _clientPricingManager;
                }
            }
        }

        ClientPricingManager()
        {

            try
            {
                if (!CustomThemeHelper.IsDesignMode())
                {
                    //Get pricing rules
                    GetPricingRules();
                    GetPricingPolicyDetail();

                    //Create Pricin gService Proxy
                    CreatePricingServiceProxy();

                    //get yester day date time string to initialize mark cache
                    _yesterdayDateTimeString = GetAUECOffsetBusinessAdjustedYesterdayDateTimeString();

                    // Initilize Mark Price Cache on pricing server
                    InitilizeMarkPriceCache();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServiceProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        void _pricingServiceProxy_ConnectedEvent(object sender, EventArgs e)
        {
            try
            {
                InitilizeMarkPriceCache();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        private void InitilizeMarkPriceCache()
        {
            try
            {
                if (_pricingServiceProxy != null && _yesterdayDateTimeString != null)
                {
                    BackgroundWorker InitializeMarkPricesCacheAsync = new BackgroundWorker();
                    InitializeMarkPricesCacheAsync.DoWork += new DoWorkEventHandler(InitializeMarkPricesCacheAsync_DoWork);
                    InitializeMarkPricesCacheAsync.RunWorkerCompleted += new RunWorkerCompletedEventHandler(InitializeMarkPricesCacheAsync_RunWorkerCompleted);
                    InitializeMarkPricesCacheAsync.RunWorkerAsync();
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

        }

        void InitializeMarkPricesCacheAsync_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            try
            {
                //need to check here for below function from expnl - omshiv
                // AdjustMarkPriceByTodaysSplitFactor(false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        void InitializeMarkPricesCacheAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_pricingServiceProxy != null && _yesterdayDateTimeString != null)
                {
                    _pricingServiceProxy.InnerChannel.InitializeMarkPricesCache(_yesterdayDateTimeString);
                }

            }
            catch (Exception ex)
            {

                Exception pricingConnError = new Exception("Pricing Server is down.", ex);
                bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw pricingConnError;
                }
            }
        }


        void _securityMasterServices_SMGenericPriceResponse(QueueMessage qMsg, Guid requestId)
        {
            try
            {

                List<object> listDatareturned = binaryFormatter.DeSerializeParams(qMsg.Message.ToString());
                DataTable priceData = listDatareturned[0] as DataTable;
                if (PriceResponseEventHandler != null)
                {
                    PriceResponseEventHandler(this, new EventArgs<DataTable>(priceData));
                }


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public static void GetPricingRules()
        {

            try
            {
                _priceRuleList = WindsorContainerManager.GetPricingRules();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Modified by sachin mishra purpose -CHMW-2905 Pricing Policy Implementation 
        /// </summary>
        public static void GetPricingPolicyDetail()
        {

            try
            {
                _pricingPolicyDetailList = WindsorContainerManager.GetPriceRuleDetailFromDB();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Modified by sachin mishra purpose -CHMW-2905 Pricing Policy Implementation 
        /// </summary>
        // modified by omshiv, now we are requesting for price based on secondary pricing source.
        public int GetPrices_New(Dictionary<int, Dictionary<string, SymbolPriceRequest>> requestedSymbolsDict, DateTime startDate, DateTime endDate, bool isForex)
        {
            try
            {
                if (_priceRuleList != null && _priceRuleList.Count > 0)
                {
                    int countOfRequestedSymbols = 0;
                    bool proceed = true;
                    StringBuilder invalidAccountSymbolCombination = new StringBuilder();
                    StringBuilder invalidPricingField = new StringBuilder();
                    //secondary soure wise field wise secmasterRequestObject dictionary
                    Dictionary<String, Dictionary<PricingDataType, SecMasterRequestObj>> symbolPriceRequestCol = new Dictionary<String, Dictionary<PricingDataType, SecMasterRequestObj>>();
                    foreach (int accountId in requestedSymbolsDict.Keys)
                    {
                        DataSet pricingPolicyDetailFromfile = new DataSet();
                        PricingRule priceRuleForAccount = GetPriceRuleForAccount(accountId);

                        if (priceRuleForAccount != null && !isForex)
                        {
                            string spName = string.Empty;
                            PricePolicyDetails pricePolicyDetail = GetPriceRuleDetails(priceRuleForAccount.PricingPolicyID);
                            if (pricePolicyDetail != null && priceRuleForAccount.IsPricingPolicy && pricePolicyDetail.IsFileAvailable)
                            {
                                spName = pricePolicyDetail.SPName;
                                pricingPolicyDetailFromfile = GetPricingPolicyDetailsFromFile(pricePolicyDetail, priceRuleForAccount, startDate, endDate);
                                DataTable detailFromFile = pricingPolicyDetailFromfile.Tables[0];
                                if (detailFromFile.Rows.Count > 0)
                                {
                                    if (PriceResponseEventHandlerfromfile != null)
                                        PriceResponseEventHandlerfromfile(this, new EventArgs<DataTable>(detailFromFile));
                                }

                                //TDOD returen data
                            }
                            else if (priceRuleForAccount.IsPricingPolicy && pricePolicyDetail != null)
                            {
                                spName = pricePolicyDetail.SPName;
                                //if (priceRule.IsPricingPolicy)
                                //{
                                List<PricingPolicyDetailsFromSP> pricingPolicyDetailFromsp = GetPricingPolicyDetailsBySP(spName, priceRuleForAccount.AccountID, startDate);

                                foreach (String symbol in requestedSymbolsDict[accountId].Keys)
                                {
                                    String secondaryPricingSource = string.Empty;
                                    SymbolPriceRequest request = GetPriceRquestForSymbol(pricingPolicyDetailFromsp, symbol, priceRuleForAccount);
                                    if (request != null && request.PriceFieldType != PricingDataType.Undefined)
                                    {
                                        secondaryPricingSource = request.secondaryPricingSource;
                                        if (secondaryPricingSource != "BGN")
                                        {
                                            secondaryPricingSource = request.secondaryPricingSource;
                                        }
                                        else
                                        {
                                            secondaryPricingSource = string.Empty;
                                        }
                                        requestedSymbolsDict[accountId][symbol].secondaryPricingSource = secondaryPricingSource;
                                        requestedSymbolsDict[accountId][symbol].PriceFieldType = request.PriceFieldType;
                                        countOfRequestedSymbols++;
                                        AddRequest(symbolPriceRequestCol, request);
                                    }
                                    else
                                    {
                                        if (request == null)
                                        {
                                            invalidAccountSymbolCombination.Append(CachedDataManager.GetInstance.GetAccountText(accountId) + "-" + symbol + ", ");
                                        }
                                        else
                                        {
                                            invalidPricingField.Append(CachedDataManager.GetInstance.GetAccountText(accountId) + "-" + symbol + ", ");
                                        }
                                        continue;
                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (String symbol in requestedSymbolsDict[accountId].Keys)
                            {

                                SymbolPriceRequest symbolRquest = requestedSymbolsDict[accountId][symbol];

                                PricingDataType field = PricingDataType.Close;
                                String secondaryPricingSource = string.Empty;
                                PricingRule priceRuleForAccountSymbol = GetPriceRuleForSymbol(symbolRquest);
                                countOfRequestedSymbols++;

                                if (priceRuleForAccountSymbol != null)
                                {
                                    field = (PricingDataType)Enum.ToObject(typeof(PricingDataType), priceRuleForAccountSymbol.PricingDataTypeID);
                                    if (field != PricingDataType.Undefined)
                                    {
                                        secondaryPricingSource = priceRuleForAccountSymbol.SecondarySource;
                                        symbolRquest.PriceFieldType = field;
                                        //todo - remove this condition after fixing default source issue - omshiv
                                        if (secondaryPricingSource != "BGN")
                                            symbolRquest.secondaryPricingSource = secondaryPricingSource;
                                        else
                                        {
                                            symbolRquest.secondaryPricingSource = string.Empty;
                                        }

                                        AddRequest(symbolPriceRequestCol, symbolRquest);
                                    }
                                    else
                                    {
                                        invalidAccountSymbolCombination.Append(CachedDataManager.GetInstance.GetAccountText(symbolRquest.accountId) + "-" + symbolRquest.Symbol + ", ");
                                        continue;
                                    }
                                }
                                else
                                {
                                    invalidAccountSymbolCombination.Append(CachedDataManager.GetInstance.GetAccountText(symbolRquest.accountId) + "-" + symbolRquest.Symbol + ", ");
                                    continue;
                                }
                            }

                        }
                    }

                    if (invalidAccountSymbolCombination.Length > 0)
                    {
                        if (isForex)
                        {
                            if (MessageBox.Show("Pricing Asset class rules not defined for some of the selected account-symbol Combination.\n Do you want to request data for rest? ", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                proceed = false;
                                return -1;
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Pricing rules not defined for some of the selected account-symbol Combination.\n Do you want to request data for rest? ", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                proceed = false;
                                return -1;
                            }
                        }
                    }
                    if (invalidPricingField.Length > 0)
                    {
                        if (MessageBox.Show("Invalid BloomBerg Symbols." + invalidPricingField + "\n Do you want to request data for rest?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            proceed = false;
                            return -1;
                        }
                    }

                    if (proceed)
                    {
                        foreach (String pricingSource in symbolPriceRequestCol.Keys)
                        {
                            Dictionary<PricingDataType, SecMasterRequestObj> fieldWiseSymbols = symbolPriceRequestCol[pricingSource];
                            foreach (PricingDataType field in fieldWiseSymbols.Keys)
                            {
                                //TODO get pricingSorce from Rule -omshiv
                                String secondarySource = pricingSource;
                                if (pricingSource == "BGN")
                                {
                                    secondarySource = string.Empty;
                                }
                                SecMasterRequestObj reqObj = fieldWiseSymbols[field];
                                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                                reqObj.HashCode = this.GetHashCode();

                                ConcurrentBag<string> fieldList = new ConcurrentBag<string>();
                                if (field == PricingDataType.Avg_AskBid)
                                {
                                    if (!fieldList.Contains(PricingDataType.Ask.ToString()))
                                    {
                                        fieldList.Add(PricingDataType.Ask.ToString());
                                    }
                                    if (!fieldList.Contains(PricingDataType.Bid.ToString()))
                                    {
                                        fieldList.Add(PricingDataType.Bid.ToString());
                                    }
                                }
                                else
                                {
                                    if (!fieldList.Contains(field.ToString()))
                                    {
                                        fieldList.Add(field.ToString());
                                    }
                                }
                                _securityMasterServices.GetMarkPricesForSymbolAndDate(secondarySource, fieldList.ToList(), reqObj, startDate, endDate, Guid.NewGuid(), _securityMasterServices_SMGenericPriceResponse, true);
                            }
                        }
                    }
                    string[] invalidCombinationCount = invalidAccountSymbolCombination.ToString().Trim().Split(',');
                    if ((invalidCombinationCount.Length - 1) < countOfRequestedSymbols)
                        return 1;
                }
                else
                {
                    MessageBox.Show("Pricing rules not defined for some of the selected  account-symbol Combination. ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return 0;
            }
            return -1;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolPriceRequest"></param>
        /// <param name="request"></param>
        private void AddRequest(Dictionary<string, Dictionary<PricingDataType, SecMasterRequestObj>> symbolPriceRequest, SymbolPriceRequest request)
        {
            try
            {
                if (symbolPriceRequest.ContainsKey(request.secondaryPricingSource))
                {
                    Dictionary<PricingDataType, SecMasterRequestObj> fieldWiseSymbols = symbolPriceRequest[request.secondaryPricingSource];

                    if (fieldWiseSymbols.ContainsKey(request.PriceFieldType))
                    {
                        fieldWiseSymbols[request.PriceFieldType].AddData(request.Symbol, ApplicationConstants.SymbologyCodes.BloombergSymbol);
                    }
                    else
                    {
                        SecMasterRequestObj req = new SecMasterRequestObj();
                        req.AddData(request.Symbol, ApplicationConstants.SymbologyCodes.BloombergSymbol);
                        fieldWiseSymbols.Add(request.PriceFieldType, req);
                    }


                }
                else
                {
                    Dictionary<PricingDataType, SecMasterRequestObj> fieldWiseSymbols = new Dictionary<PricingDataType, SecMasterRequestObj>();
                    SecMasterRequestObj req = new SecMasterRequestObj();
                    req.AddData(request.Symbol, ApplicationConstants.SymbologyCodes.BloombergSymbol);

                    fieldWiseSymbols.Add(request.PriceFieldType, req);
                    symbolPriceRequest.Add(request.secondaryPricingSource, fieldWiseSymbols);
                }
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
        /// 
        /// </summary>
        /// <param name="pricingPolicyDetailFromsp"></param>
        /// <param name="symbol"></param>
        /// <param name="priceRule"></param>
        /// <returns></returns>
        private SymbolPriceRequest GetPriceRquestForSymbol(List<PricingPolicyDetailsFromSP> pricingPolicyDetailFromsp, string symbol, PricingRule priceRule)
        {
            SymbolPriceRequest req = new SymbolPriceRequest();
            try
            {
                PricingPolicyDetailsFromSP pricing = null;
                var rule =
                from pricingrule in pricingPolicyDetailFromsp
                where pricingrule.AccountID == priceRule.AccountID && pricingrule.Symbol == symbol
                select pricingrule;


                if (rule.Count() > 0)
                {
                    pricing = rule.First() as PricingPolicyDetailsFromSP;
                }
                PricingDataType fieldValue = PricingDataType.Undefined;
                if (pricing != null)
                {
                    string PricingFieldValue = pricing.PricingField;
                    Enum.TryParse(PricingFieldValue, out fieldValue);
                }
                req.PriceFieldType = fieldValue;
                req.Symbol = symbol;
                req.accountId = priceRule.AccountID;
                req.secondaryPricingSource = priceRule.SecondarySource;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return req;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private PricingRule GetPriceRuleForAccount(int accountId)
        {
            PricingRule pricing = null;
            try
            {
                var rule =
                from pricingrule in _priceRuleList
                where pricingrule.AccountID == accountId && pricingrule.IsPricingPolicy == true
                select pricingrule;


                if (rule.Count() > 0)
                {
                    pricing = rule.First() as PricingRule;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            return pricing;

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pricePolicyDetail"></param>
        /// <param name="priceRule"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        private DataSet GetPricingPolicyDetailsFromFile(PricePolicyDetails pricePolicyDetail, PricingRule priceRule, DateTime startDate, DateTime endDate)
        {
            DataSet pricingPolicyDetailFromFile = new DataSet();
            try
            {
                pricingPolicyDetailFromFile = WindsorContainerManager.GetPricePolicyDetailSPFromDB(pricePolicyDetail.SPName, priceRule.AccountID, pricePolicyDetail.FilePath, pricePolicyDetail.FolderPath, startDate, endDate);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }


            return pricingPolicyDetailFromFile;
        }


        /// <summary>
        /// Modified by sachin mishra purpose -CHMW-2905 Pricing Policy Implementation
        /// </summary>
        /// <param name="SPName"></param>
        /// <param name="accountID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private List<PricingPolicyDetailsFromSP> GetPricingPolicyDetailsBySP(string SPName, int accountID, DateTime date)
        {
            try
            {
                List<PricingPolicyDetailsFromSP> pricingPolicyDetailFromSP = new List<PricingPolicyDetailsFromSP>();
                pricingPolicyDetailFromSP = WindsorContainerManager.GetPricePolicyDetailSPFromDB(accountID, date, SPName); // filtered date
                return pricingPolicyDetailFromSP;

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return null;

        }





        //TODO - Need to comment/remove this function as we are using new for it.
        // modified by omshiv, now we are requesting for price based on secondary pricing source.
        public int GetPrices(Dictionary<string, List<SymbolPriceRequest>> requestedSymbolsDict, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (_priceRuleList != null && _priceRuleList.Count > 0)
                {
                    int countOfRequestedSymbols = 0;
                    StringBuilder invalidAccountSymbolCombination = new StringBuilder();
                    //secondary soure wise field wise secmasterRequestObject dictionary
                    Dictionary<String, Dictionary<PricingDataType, SecMasterRequestObj>> symbolPriceRequest = new Dictionary<String, Dictionary<PricingDataType, SecMasterRequestObj>>();
                    foreach (String symbol in requestedSymbolsDict.Keys)
                    {
                        ApplicationConstants.SymbologyCodes symbology = ApplicationConstants.SymbologyCodes.BloombergSymbol;
                        foreach (SymbolPriceRequest symbolRquest in requestedSymbolsDict[symbol])
                        {
                            //PricingProviderType
                            //PricingDataType
                            PricingRule priceRule = GetPriceRuleForSymbol(symbolRquest);
                            PricingDataType field = PricingDataType.Close;
                            String secondaryPricingSource = string.Empty;
                            countOfRequestedSymbols++;
                            if (priceRule != null)
                            {
                                field = (PricingDataType)Enum.ToObject(typeof(PricingDataType), priceRule.PricingDataTypeID);

                                secondaryPricingSource = priceRule.SecondarySource;
                            }
                            else
                            {
                                invalidAccountSymbolCombination.Append(CachedDataManager.GetInstance.GetAccountText(symbolRquest.accountId) + "-" + symbolRquest.Symbol + ", ");
                                continue;
                                //return -1;
                            }
                            symbolRquest.PriceFieldType = field;
                            //todo - remove this condition after fixing default source issue - omshiv
                            if (secondaryPricingSource != "BGN")
                                symbolRquest.secondaryPricingSource = secondaryPricingSource;
                            else
                            {
                                symbolRquest.secondaryPricingSource = string.Empty;
                            }
                            if (symbolPriceRequest.ContainsKey(secondaryPricingSource))
                            {
                                Dictionary<PricingDataType, SecMasterRequestObj> fieldWiseSymbols = symbolPriceRequest[secondaryPricingSource];
                                if (fieldWiseSymbols.ContainsKey(field))
                                {
                                    fieldWiseSymbols[field].AddData(symbol, symbology);
                                }
                                else
                                {
                                    SecMasterRequestObj req = new SecMasterRequestObj();
                                    req.AddData(symbol, symbology);
                                    fieldWiseSymbols.Add(field, req);
                                }
                            }
                            else
                            {
                                Dictionary<PricingDataType, SecMasterRequestObj> fieldWiseSymbols = new Dictionary<PricingDataType, SecMasterRequestObj>();
                                SecMasterRequestObj req = new SecMasterRequestObj();
                                req.AddData(symbol, symbology);
                                fieldWiseSymbols.Add(field, req);
                                symbolPriceRequest.Add(secondaryPricingSource, fieldWiseSymbols);
                            }
                        }
                    }
                    bool proceed = true; ;
                    if (invalidAccountSymbolCombination.Length > 0)
                    {
                        if (MessageBox.Show("Pricing rules not defined for some of the selected  account-symbol Combination.\n Do you want to request data for rest? ", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            proceed = false;
                            return -1;
                        }
                    }
                    if (proceed)
                    {
                        foreach (String pricingSource in symbolPriceRequest.Keys)
                        {
                            Dictionary<PricingDataType, SecMasterRequestObj> fieldWiseSymbols = symbolPriceRequest[pricingSource];
                            foreach (PricingDataType field in fieldWiseSymbols.Keys)
                            {
                                //TODO get pricingSorce from Rule -omshiv
                                String secondarySource = pricingSource;
                                if (pricingSource == "BGN")
                                {
                                    secondarySource = string.Empty;
                                }
                                SecMasterRequestObj reqObj = fieldWiseSymbols[field];
                                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                                reqObj.HashCode = this.GetHashCode();
                                _securityMasterServices.GetMarkPricesForSymbolAndDate(secondarySource, new List<string> { field.ToString() }, reqObj, startDate, endDate, Guid.NewGuid(), _securityMasterServices_SMGenericPriceResponse, true);
                            }
                        }
                    }
                    string[] invalidCombinationCount = invalidAccountSymbolCombination.ToString().Trim().Split(',');
                    if ((invalidCombinationCount.Length - 1) < countOfRequestedSymbols)
                        return 1;
                }
                else
                {
                    MessageBox.Show("Pricing rules not defined for some of the selected  account-symbol Combination. ", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return 0;
            }
            return -1;
        }



        /// <summary>
        /// Modified by sachin mishra purpose -CHMW-2905 Pricing Policy Implementation 
        /// </summary>


        private PricePolicyDetails GetPriceRuleDetails(int pricingPolicyID)
        {
            PricePolicyDetails pricingDetails = null;
            try
            {
                var rule =
                from pricingDetail in _pricingPolicyDetailList
                where pricingDetail.PricingID == pricingPolicyID
                select pricingDetail;
                if (rule.Count() > 0)
                {
                    pricingDetails = rule.First() as PricePolicyDetails;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            return pricingDetails;
        }





        private static PricingRule GetPriceRuleForSymbol(SymbolPriceRequest symbol)
        {
            PricingRule pricing = null;
            PricingDataType field;
            try
            {
                var rule =
                from pricingrule in _priceRuleList
                where pricingrule.AccountID == symbol.accountId
                select pricingrule;


                if (rule.Count() > 0)
                {
                    pricing = rule.First() as PricingRule;
                    field = (PricingDataType)Enum.ToObject(typeof(PricingDataType), pricing.PricingDataTypeID);
                    if (field == PricingDataType.Undefined)
                    {
                        pricing = rule.Last() as PricingRule;
                    }
                    field = (PricingDataType)Enum.ToObject(typeof(PricingDataType), pricing.PricingDataTypeID);
                    if (Convert.ToInt16(field) == -1)
                    {
                        pricing.PricingDataTypeID = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            return pricing;
        }

        public void RefreshPricingRulesCache()
        {
            try
            {
                GetPricingRules();
                GetPricingPolicyDetail();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        internal string GetAUECOffsetBusinessAdjustedYesterdayDateTimeString()
        {
            StringBuilder allAUECLocalDatesFromUTC = new StringBuilder();
            try
            {
                allAUECLocalDatesFromUTC.Append(0);
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                allAUECLocalDatesFromUTC.Append(DateTime.Now.ToUniversalTime());
                allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                _currentOffsetAdjustedAUECDates = new Dictionary<int, DateTime>(CachedDataManager.GetInstance.GetAUECCount() + 1);
                List<int> InUseAUECIDs = WindsorContainerManager.GetInUseAUECIDs();
                //todo ADD AUec WISE _currentOffsetAdjustedAUECDates - OMSHIV
                //foreach (KeyValuePair<int, DateTime> AUECOffsetAdjustedTime in _currentOffsetAdjustedAUECDates)
                //{
                foreach (int AuecID in InUseAUECIDs)
                {

                    //  int AuecID = AUECOffsetAdjustedTime.Key;
                    //if (_inUseAUECIDs.Contains(AuecID))
                    //{
                    allAUECLocalDatesFromUTC.Append(AuecID);
                    allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_5);
                    DateTime yesterdayBusinessAdjustedDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(DateTime.Now.ToUniversalTime(), -1, AuecID).Date;
                    allAUECLocalDatesFromUTC.Append(yesterdayBusinessAdjustedDate);
                    allAUECLocalDatesFromUTC.Append(Prana.BusinessObjects.Seperators.SEPERATOR_6);
                    // }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return allAUECLocalDatesFromUTC.ToString();
        }

        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            //throw new NotImplementedException();
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            //throw new NotImplementedException();
        }

        public void LiveFeedDisConnected()
        {
            //throw new NotImplementedException();
        }

        public void SaveDailyValuationData(SMBatchType sMBatchType, DataTable dt)
        {
            try
            {
                switch (sMBatchType)
                {

                    case SMBatchType.Beta:
                        _pricingServiceProxy.InnerChannel.SaveBeta(dt);
                        break;

                    case SMBatchType.DailyVolume:
                        break;

                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    if (_pricingServiceProxy != null)
                        _pricingServiceProxy.Dispose();
                }

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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
