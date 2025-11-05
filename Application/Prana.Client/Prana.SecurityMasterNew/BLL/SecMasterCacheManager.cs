using Prana.BusinessLogic.Symbol;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.NewLiveFeed;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SMObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.SecurityMasterNew.BLL;
using Prana.SecurityMasterNew.DAL;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Prana.SecurityMasterNew
{
    public class SecMasterCacheManager : ILiveFeedCallback, ISecMasterServices, CentralSMService.ICentralSMServiceCallback, IDisposable
    {
        private static object _publishLock = new object();
        public event EventHandler<EventArgs<string, string, string>> StatusOfRequest;
        public event EventHandler<EventArgs<string, object, string, string>> EventSendDataByResKey;
        public event EventHandler<EventArgs<SecMasterBaseObj>> SecMstrDataResponse;

        #region compliance events
        //public event SecurityObjectReceivedHandler SecurityObjectReceived;
        public event EventHandler<EventArgs<SecMasterBaseObj>> SecurityObjectReceived;
        //public event AuecDetailsUpdatedHandler AuecDetailsUpdated;
        public event EventHandler<EventArgs<AuecDetails>> AuecDetailsUpdated;
        //public event UDADataReceivedHandler UDADataReceived;

        /// <summary>
        /// Event to send future root save data response
        /// </summary>
        public event EventHandler<EventArgs<string, string, string>> FutureRootDataSavedResponse;
        #endregion

        PranaBinaryFormatter _binaryFormatter = new PranaBinaryFormatter();

        object lockerDbAccess = new object();
        private int _hashCode = int.MinValue;
        private int _maxSMReqSize = 1000;
        const int MAXSMCHUNKSIZE = 500;
        private bool _isAutoUpdateOptionsUDAWithUnderlyingUpdate = true;

        PranaPricingSource _pricingSource = PranaPricingSource.Esignal;
        public string ServerIdentifier { get; set; }
        public event EventHandler<EventArgs<DataSet, SymbolLookupRequestObject>> SymbolLkUpDataResponse;
        delegate void AsyncInvokeDelegate(Delegate del, params object[] args);
        private SecMasterSymbolCache _symbolCache;

        public SecMasterCacheManager()
        {
            try
            {
                SecMasterDataCache.GetInstance.SetFutureRootData(SecMasterDataManager.GetFutureRootData(_connectionStr));
                SecMasterDataCache.GetInstance.SetPreferences(SecMasterDataManager.GetPreferencesFromDB());

                CreatePricingServiceProxy();
                CreatePublishingProxy();
                SubscribeSecMasterDataCacheEvents();
                UDADataCache.GetInstance.SetCommonUDAData();

                _maxSMReqSize = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("SMUIMaxSizeForHistData").ToString());
                _isAutoUpdateOptionsUDAWithUnderlyingUpdate = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsAutoUpdateOptionsUDAWithUnderlyingUpdate").ToString());

                string histPricing = CachedDataManager.GetInstance.GetPranaPreferenceByKey("PricingSource");
                Enum.TryParse<PranaPricingSource>(histPricing, true, out _pricingSource);
                if (_pricingSource == PranaPricingSource.Bloomberg)
                {
                    string hostName = Dns.GetHostName();
                    IEnumerable<IPAddress> ipAddresses = Dns.GetHostEntry(hostName).AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    ServerIdentifier = "TradeServer" + ipAddresses.Last().ToString() + ":" + ConfigurationHelper.Instance.GetAppSettingValueByKey("OrderRequestPort");
                }

                SecMasterSymbolCache[] secCaches = SecMasterDataManager.getSymbolCache();
                _symbolCache = secCaches[0];
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public SecMasterCacheManager(String connString)
        {
            try
            {
                _connectionStr = connString;
                SecMasterDataCache.GetInstance.SetFutureRootData(SecMasterDataManager.GetFutureRootData(_connectionStr));
                CreatePricingServiceProxy();
                CreatePublishingProxy();
                SubscribeSecMasterDataCacheEvents();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        private void CreatePricingServiceProxy()
        {
            try
            {
                _pricingServicesProxy = new DuplexProxyBase<IPricingService>("PricingServiceEndpointAddress", this);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region SecmasterDatacache events forwading to amqppluginmanager
        /// <summary>
        /// Subscribing to SecMasterDataCache events so that the can be forwarded to esper amqp plugin manager
        /// </summary>
        private void SubscribeSecMasterDataCacheEvents()
        {
            try
            {
                SecMasterDataCache.GetInstance.SendSecurityToCompliance += new EventHandler<EventArgs<SecMasterBaseObj>>(SecMasterDataCache_SendSecurityToCompliance);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Forwarding Security to esper amqp plugin manager
        /// </summary>
        /// <param name="secMaterBaseObj"></param>
        void SecMasterDataCache_SendSecurityToCompliance(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                SecMasterBaseObj secMasterObj = e.Value;
                if (secMasterObj != null)
                    SendSecurityDetailsToCompliance(secMasterObj);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
        #endregion

        static ProxyBase<IPublishing> _proxy;
        private void CreatePublishingProxy()
        {
            try
            {
                _proxy = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
            }
            catch (Exception)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(new Exception("Could not create Pub Proxy"), LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public List<SecMasterBaseObj> GetAllSecMasterData()
        {
            return SecMasterDataCache.GetInstance.GetAllSecMasterData();
        }

        public void Subscribe(int senderHashCode)
        {
            try
            {
                SecMasterDataCache.GetInstance.Subscribe(senderHashCode);
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

        public void UnSubscribe(int senderHashCode)
        {
            try
            {
                SecMasterDataCache.GetInstance.UnSubscribe(senderHashCode);
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
        /// Save Secuirty in DB when data from External Source like e-Signal
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void SaveNewSymbolToSecurityMaster(SecMasterBaseObj secMasterObj, bool saveSymbolInDb = true, string RequestedUsedID = null, string RequestedHashcode = null)
        {
            try
            {
                if (RequestedHashcode != null && RequestedUsedID != null)
                {
                    secMasterObj.RequestedHashcode = RequestedHashcode;
                    secMasterObj.RequestedUserID = RequestedUsedID;
                }
                InvokeSecMstrDataResponse(secMasterObj);

                if (secMasterObj.AUECID != 0)
                {
                    if (secMasterObj is SecMasterOptObj && string.IsNullOrWhiteSpace(secMasterObj.BloombergSymbol) && !string.IsNullOrWhiteSpace(secMasterObj.UnderLyingSymbol))
                    {
                        SecMasterRequestObj secMasterReqObj = new SecMasterRequestObj();
                        secMasterReqObj.AddData(secMasterObj.UnderLyingSymbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                        DateTime date = DateTime.UtcNow;

                        List<SecMasterBaseObj> secMasterData = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterReqObj, date);
                        if (secMasterData.Count > 0)
                        {
                            SecMasterBaseObj underlyingObj = secMasterData[0];
                            SecMasterOptObj secMasterOptObj = (SecMasterOptObj)secMasterObj;
                            OptionDetail optionDetail = new OptionDetail();
                            optionDetail.ExpirationDate = secMasterOptObj.ExpirationDate;
                            optionDetail.StrikePrice = secMasterOptObj.StrikePrice;
                            optionDetail.UnderlyingSymbol = secMasterOptObj.UnderLyingSymbol;
                            optionDetail.Symbology = ApplicationConstants.SymbologyCodes.BloombergSymbol;
                            optionDetail.AssetCategory = underlyingObj.AssetCategory;
                            optionDetail.AUECID = underlyingObj.AUECID;
                            optionDetail.OptionType = (OptionType)secMasterOptObj.PutOrCall;
                            optionDetail.StrikePriceMultiplier = underlyingObj.StrikePriceMultiplier;
                            optionDetail.EsignalOptionRoot = underlyingObj.EsignalOptionRoot;
                            optionDetail.BloombergOptionRoot = underlyingObj.BloombergOptionRoot;
                            OptionSymbolGenerator.GetOptionSymbol(optionDetail);
                            if (!string.IsNullOrEmpty(optionDetail.Symbol))
                            {
                                secMasterObj.BloombergSymbol = optionDetail.Symbol;
                            }
                            if (string.IsNullOrEmpty(secMasterObj.OSIOptionSymbol))
                            {
                                secMasterObj.OSIOptionSymbol = OptionSymbolGenerator.GenerateOSISymbol(optionDetail);
                            }
                        }
                    }
                    if (secMasterObj is SecMasterOptObj && string.IsNullOrEmpty(secMasterObj.OSIOptionSymbol) && !string.IsNullOrWhiteSpace(secMasterObj.UnderLyingSymbol))
                    {
                        SecMasterOptObj secMasterOptObj = (SecMasterOptObj)secMasterObj;
                        OptionDetail optionDetail = new OptionDetail();
                        optionDetail.ExpirationDate = secMasterOptObj.ExpirationDate;
                        optionDetail.StrikePrice = secMasterOptObj.StrikePrice;
                        optionDetail.UnderlyingSymbol = secMasterOptObj.UnderLyingSymbol;
                        optionDetail.OptionType = (OptionType)secMasterOptObj.PutOrCall;
                        secMasterObj.OSIOptionSymbol = OptionSymbolGenerator.GenerateOSISymbol(optionDetail);
                    }

                    // https://jira.nirvanasolutions.com:8443/browse/PRANA-37901
                    if (secMasterObj is SecMasterFixedIncome)
                    {
                        SecMasterFixedIncome secMasterFixedIncomeObj = (SecMasterFixedIncome)secMasterObj;
                        if (secMasterFixedIncomeObj.MaturityDate.Equals(DateTime.MinValue))
                        {
                            secMasterFixedIncomeObj.MaturityDate = DateTimeConstants.MinValue;
                        }
                        if (secMasterFixedIncomeObj.IssueDate.Equals(DateTime.MinValue))
                        {
                            secMasterFixedIncomeObj.IssueDate = DateTimeConstants.MinValue;
                        }
                        if (secMasterFixedIncomeObj.FirstCouponDate.Equals(DateTime.MinValue))
                        {
                            secMasterFixedIncomeObj.FirstCouponDate = DateTimeConstants.MinValue;
                        }
                    }
                    SecMasterBaseObj secMasterObjClone = DeepCopyHelper.Clone(secMasterObj);
                    UpdateRiskCurrency(secMasterObjClone);

                    // saving to DB
                    Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();

                    // Replaces Dynamic UDA XML node
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-9502
                    String XmlString = _xml.WriteString(secMasterObjClone);
                    XmlString = XmlString.Replace("KeyValuePair`2", "KeyValuePair2");
                    XDocument sesMasterDoc = XDocument.Parse(XmlString);
                    XElement secMasterObjXml = sesMasterDoc.Descendants("TickerSymbol").SingleOrDefault();
                    if (secMasterObjXml != null)
                    {
                        XElement oldDynamicUDAXML = secMasterObjXml.Parent.Element("DynamicUDA");

                        XElement newDynamicUDAXML = new XElement("DynamicUDAs");
                        foreach (string key in secMasterObjClone.DynamicUDA.Keys) { newDynamicUDAXML.Add(new XElement(key, secMasterObjClone.DynamicUDA[key].ToString().Trim())); }

                        oldDynamicUDAXML.ReplaceWith(newDynamicUDAXML);
                    }
                    XmlString = sesMasterDoc.ToString(SaveOptions.DisableFormatting);

                    if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: SecMasterCacheManager.SaveNewSymbolToSecurityMaster() > Before SendDataToClient() for Symbol: {0}, Time: {1}", secMasterObj.TickerSymbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                    }

                    SecMasterServerComponent.GetInstance.SendDataToClient(secMasterObj);

                    if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: SecMasterCacheManager.SaveNewSymbolToSecurityMaster() > Before Saving data in DB for Symbol: {0}, Time: {1}", secMasterObj.TickerSymbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                    }

                    // Modified by Bhavana on JULY 2014
                    // Purpose : Datasource field is included in xml parameter
                    if (saveSymbolInDb)
                        SecMasterDataManager.SaveNewSymbolResponsetoSecurityMaster(XmlString, _connectionStr, _isAutoUpdateOptionsUDAWithUnderlyingUpdate);
                    // modified by omshiv, july 2014, Update UDA attributes in Local DB and local cahce and send updated list to client

                    if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                    {
                        LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: SecMasterCacheManager.SaveNewSymbolToSecurityMaster() > After Saving data in DB for Symbol: {0}, Time: {1}", secMasterObj.TickerSymbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                    }

                    if (secMasterObj.SymbolType == (int)SymbolType.New)
                    {
                        UpdateCurUDAListforNewUDA(secMasterObj.SymbolUDAData);
                    }

                }
                //Add to cache of security master
                SecMasterDataCache.GetInstance.AddValues(secMasterObj, DateTime.UtcNow);
                _symbolCache.symbolFillData(secMasterObj);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update issuer for security master object if it is blank
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void UpdateIssuer(SecMasterBaseObj secMasterObj)
        {
            try
            {
                string issuer = GetIssuerForSymbolFromCache(secMasterObj);
                // Update/add issuer field if it is blank
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-9838
                if (secMasterObj.DynamicUDA.ContainsKey("Issuer") && string.IsNullOrWhiteSpace(secMasterObj.DynamicUDA["Issuer"].ToString()))
                {
                    if (!string.IsNullOrWhiteSpace(issuer))
                        secMasterObj.DynamicUDA["Issuer"] = issuer;
                    else
                        secMasterObj.DynamicUDA.Remove("Issuer");
                }
                else if (!secMasterObj.DynamicUDA.ContainsKey("Issuer") && !string.IsNullOrWhiteSpace(issuer))
                    secMasterObj.DynamicUDA.Add("Issuer", issuer);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// remove risk currency field if it is same as automatically calculated value
        /// </summary>
        /// <param name="secMasterObj"></param>
        private static void UpdateRiskCurrency(SecMasterBaseObj secMasterObj)
        {
            try
            {
                // remove risk currency field if it is same as automatically calculated value
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-9768
                string riskCurrency = SecMasterDataManager.GetRiskCurrency(secMasterObj);
                if (secMasterObj.DynamicUDA.ContainsKey("RiskCurrency") && secMasterObj.DynamicUDA["RiskCurrency"].ToString() == riskCurrency)
                    secMasterObj.DynamicUDA.Remove("RiskCurrency");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// get issuer from underlying if it exists in cache
        /// </summary>
        /// <param name="secMasterObj"></param>
        /// <returns></returns>
        private string GetIssuerForSymbolFromCache(SecMasterBaseObj secMasterObj)
        {
            string issuer = string.Empty;
            try
            {
                if (!secMasterObj.TickerSymbol.Equals(secMasterObj.UnderLyingSymbol) && !String.IsNullOrWhiteSpace(secMasterObj.UnderLyingSymbol))
                {
                    // set default issuer for FX and FX Forward symbols same as Underlying symbol, PRANA-10830
                    if (secMasterObj.AssetCategory.Equals(AssetCategory.FXForward) || secMasterObj.AssetCategory.Equals(AssetCategory.FX))
                    {
                        issuer = secMasterObj.UnderLyingSymbol;
                    }
                    else
                    {
                        SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                        secMasterRequestObj.AddData(secMasterObj.UnderLyingSymbol.Trim(),
                            ApplicationConstants.SymbologyCodes.TickerSymbol);

                        List<SecMasterBaseObj> underlyingSymbolList = new List<SecMasterBaseObj>();
                        underlyingSymbolList = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterRequestObj,
                            DateTime.UtcNow);

                        foreach (SecMasterBaseObj secMasterUnderlyingObj in underlyingSymbolList)
                        {
                            if (secMasterUnderlyingObj.TickerSymbol.Equals(secMasterObj.UnderLyingSymbol.Trim()))
                            {
                                issuer = secMasterUnderlyingObj.LongName;
                            }
                        }
                    }
                }
                else if (secMasterObj.TickerSymbol.Equals(secMasterObj.UnderLyingSymbol))
                    issuer = secMasterObj.LongName;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return issuer;
        }

        /// <summary>
        /// Update UDA list for new UDA found on CSM from BB
        /// omshiv, July 2014
        /// </summary>
        /// <param name="uDAData"></param>
        private void UpdateCurUDAListforNewUDA(UDAData uDAData)
        {
            try
            {
                if (uDAData != null)
                {
                    bool isUDAUpdated1 = UDADataCache.GetInstance.AddUDAIfNotExists(uDAData.AssetID, uDAData.UDAAsset, SecMasterConstants.CONST_UDAAsset);
                    bool isUDAUpdated2 = UDADataCache.GetInstance.AddUDAIfNotExists(uDAData.CountryID, uDAData.UDACountry, SecMasterConstants.CONST_UDACountry);
                    bool isUDAUpdated3 = UDADataCache.GetInstance.AddUDAIfNotExists(uDAData.SectorID, uDAData.UDASector, SecMasterConstants.CONST_UDASector);
                    bool isUDAUpdated4 = UDADataCache.GetInstance.AddUDAIfNotExists(uDAData.SubSectorID, uDAData.UDASubSector, SecMasterConstants.CONST_UDASubSector);
                    bool isUDAUpdated5 = UDADataCache.GetInstance.AddUDAIfNotExists(uDAData.SecurityTypeID, uDAData.UDASecurityType, SecMasterConstants.CONST_UDASecurityType);

                    //if update done on cache and DB then send List to client also.
                    if (isUDAUpdated1 || isUDAUpdated2 || isUDAUpdated3 || isUDAUpdated4 || isUDAUpdated5)
                    {
                        SendUDAAttributesToClient();
                    }

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Save security in DB and Add in cache. 
        /// If release type is CH then it also save to Central SM DB.
        /// Created by - Omshiv, May 20, 2014
        /// </summary>
        /// <param name="reqObj"></param>
        public async void SaveSecurityInSecurityMaster(SecMasterbaseList secMasterData)
        {
            try
            {
                if (_pricingSource == PranaPricingSource.Bloomberg)
                {
                    string serializedString = _binaryFormatter.Serialize(secMasterData);
                    try
                    {
                        if (CentralSMCommunicationManager.Instance.IsCSMConnected)
                        {
                            InstanceContext context = new InstanceContext(this);
                            CentralSMService.CentralSMServiceClient centralSMServiceClient = new CentralSMService.CentralSMServiceClient(context, "WSDualHttpBinding_ICentralSMService");
                            await centralSMServiceClient.GenericeSendDataToCSMAsync(CustomFIXConstants.MSG_SECMASTER_SaveREQ, serializedString, ServerIdentifier, null);
                        }
                        else
                        {
                            if (StatusOfRequest != null)
                                StatusOfRequest(this, new EventArgs<string, string, string>("Central SM server not connected!", secMasterData.UserID, secMasterData.RequestID));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                        bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDTHROW);
                        if (rethrow)
                        {
                            throw pricingConnError;
                        }
                    }
                }
                else
                {
                    SaveNewSymbolToSecurityMaster(secMasterData);
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Save updated or new security in DB 
        /// Modified by om, oct,2013
        /// </summary>
        /// <param name="secMasterData"></param>
        public void SaveNewSymbolToSecurityMaster(SecMasterbaseList secMasterData)
        {
            try
            {
                if (secMasterData != null)
                {
                    //if a new future symbol is added from symbol look up, to save the future multiplier as saved in future root data

                    foreach (SecMasterBaseObj secMasterObj in secMasterData)
                    {
                        //if security from symbol look up or from Import UI then validate fields
                        if (secMasterObj.SourceOfData.Equals(SecMasterConstants.SecMasterSourceOfData.SymbolLookup)
                            || secMasterObj.SourceOfData.Equals(SecMasterConstants.SecMasterSourceOfData.ImportData))
                        {
                            //validate security
                            SecMasterValidationManager.ValidateSecurity(secMasterObj);
                            if (!String.IsNullOrEmpty(secMasterObj.ErrorMessage))
                            {
                                if (StatusOfRequest != null)
                                {
                                    StatusOfRequest(this, new EventArgs<string, string, string>("Failed, " + secMasterObj.TickerSymbol + ": " + secMasterObj.ErrorMessage, secMasterData.UserID, secMasterData.RequestID));

                                    //Currently return if validation error, Need Discussion with Dev -om
                                    return;
                                }
                            }
                        }
                    }
                    //Saving with back ground worker 
                    BackgroundWorker bw_SaveSecMasterInDB = new BackgroundWorker();
                    bw_SaveSecMasterInDB.DoWork += new DoWorkEventHandler(bw_SaveSecMasterInDB_DoWork);
                    bw_SaveSecMasterInDB.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_SaveSecMasterInDB_RunWorkerCompleted);
                    ArrayList arguments = new System.Collections.ArrayList();
                    arguments.Add(secMasterData);
                    bw_SaveSecMasterInDB.RunWorkerAsync(arguments);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Save SecMAster to DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_SaveSecMasterInDB_DoWork(object sender, DoWorkEventArgs e)
        {
            ArrayList arguments = (System.Collections.ArrayList)e.Argument;
            SecMasterbaseList secMasterData = (SecMasterbaseList)arguments[0];
            try
            {
                Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();
                String XmlString = _xml.WriteString(secMasterData);
                XmlString = XmlString.Replace("KeyValuePair`2", "KeyValuePair2");
                XDocument sesMasterDoc = XDocument.Parse(XmlString);

                // Replaces Dynamic UDA XML node
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-8952
                foreach (SecMasterBaseObj obj in secMasterData)
                {
                    UpdateIssuer(obj);

                    SecMasterBaseObj secMasterObjClone = DeepCopyHelper.Clone(obj);
                    UpdateRiskCurrency(secMasterObjClone);

                    XElement secMasterObj = sesMasterDoc.Descendants("TickerSymbol")
                                   .Where(x => x.Value.Trim().Equals(secMasterObjClone.TickerSymbol.ToString().Trim()))
                                   .SingleOrDefault();

                    if (secMasterObj != null)
                    {
                        XElement oldDynamicUDAXML = secMasterObj.Parent.Element("DynamicUDA");

                        XElement newDynamicUDAXML = new XElement("DynamicUDAs");
                        foreach (string key in secMasterObjClone.DynamicUDA.Keys) { newDynamicUDAXML.Add(new XElement(key, secMasterObjClone.DynamicUDA[key].ToString().Trim())); }

                        oldDynamicUDAXML.ReplaceWith(newDynamicUDAXML);
                    }

                }
                XmlString = sesMasterDoc.ToString(SaveOptions.DisableFormatting);

                SecMasterDataManager.SaveNewSymbolResponsetoSecurityMaster(XmlString, _connectionStr, _isAutoUpdateOptionsUDAWithUnderlyingUpdate);
                //  }

                e.Result = secMasterData;

            }
            catch (Exception ex)
            {
                e.Cancel = true;
                e.Result = ex.Message;

                if (StatusOfRequest != null)
                {
                    if (ex.Message.Contains("Cannot insert duplicate key row") || ex.Message.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint"))
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>("Not Saved,Symbol already exist", secMasterData.UserID, secMasterData.RequestID));
                    }
                    else
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>(ex.Message, secMasterData.UserID, secMasterData.RequestID));
                    }
                }

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Save SecMAster from symbol look up to DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bw_SaveSecMasterInDB_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!e.Cancelled)
                {
                    SecMasterbaseList secMasterData = e.Result as SecMasterbaseList;
                    SecMasterbaseList updatedSecMasterCacheData = new SecMasterbaseList();
                    if (secMasterData != null)
                    {
                        foreach (SecMasterBaseObj secMasterobj in secMasterData)
                        {
                            //upadate UDA data with name from UDA IDS
                            SecMasterDataCache.GetInstance.UpdateUDADataWithName(secMasterobj);
                            _symbolCache.symbolFillData(secMasterobj);
                            InvokeSecMstrDataResponse(secMasterobj);
                            //add in cache
                            SecMasterDataCache.GetInstance.AddValues(secMasterobj, DateTime.UtcNow);
                            updatedSecMasterCacheData.Add(secMasterobj);
                        }

                        //update derivative securities in cache based on underlying symbol
                        if (_isAutoUpdateOptionsUDAWithUnderlyingUpdate)
                        {
                            SecMasterDataCache.GetInstance.UpdateUDAInSMCacheOfUnderlying(secMasterData, updatedSecMasterCacheData);
                        }
                        if (updatedSecMasterCacheData.Count > 0)
                        {
                            //send Updated Cache to client
                            SecMasterServerComponent.GetInstance.SendDataToClient(updatedSecMasterCacheData);

                            //Publish Security
                            PublishSecurityMasterData(updatedSecMasterCacheData);
                        }
                    }

                    //if no issue then send "Success" to client confirming success on Security saving in DB.
                    if (StatusOfRequest != null)
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>("Success", secMasterData.UserID, secMasterData.RequestID));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Get UDA symbol data based on underlyingsymbol or root symbol
        /// </summary>
        /// <param name="secMasterData">SecMasterBaseObj</param>
        private void GetUDASymbolDataOfUnderlying(SecMasterBaseObj secMasterData, bool isDynamicUDADataRequired = false)
        {
            try
            {
                SecMasterbaseList secMasterBaseList = new SecMasterbaseList();
                secMasterBaseList.Add(secMasterData);
                GetUDASymbolDataOfUnderlying(secMasterBaseList, isDynamicUDADataRequired);
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
        /// Get UDA symbol data based on underlyingsymbol or root symbol
        /// </summary>
        /// <param name="secMasterObj">SecMasterbaseList</param>
        private void GetUDASymbolDataOfUnderlying(SecMasterbaseList secMasterData, bool isDynamicUDADataRequired = false)
        {
            try
            {

                //create a list of underlying symbols
                List<string> underlyingSymbolList = new List<string>();

                SecMasterbaseList secMasterDataWithDefautUDA = new SecMasterbaseList();
                foreach (SecMasterBaseObj secMasterObj in secMasterData)
                {
                    //check if UseUDAFromUnderlyingOrRoot in true, then set UDA from Underlying
                    if (secMasterObj.UseUDAFromUnderlyingOrRoot)
                    {
                        if (!secMasterObj.UnderLyingSymbol.Equals(secMasterObj.TickerSymbol) && !underlyingSymbolList.Contains(secMasterObj.UnderLyingSymbol))
                        {
                            underlyingSymbolList.Add(secMasterObj.UnderLyingSymbol.Trim());
                            secMasterDataWithDefautUDA.Add(secMasterObj);
                        }
                    }
                }

                if (underlyingSymbolList.Count > 0)
                {
                    //get sm data for underlying symbols
                    Dictionary<String, SecMasterBaseObj> foundsymbolsDataDict = GetSecMasterDataDictForListSync(underlyingSymbolList, ApplicationConstants.SymbologyCodes.TickerSymbol);
                    if (foundsymbolsDataDict.Count > 0)
                    {
                        //Merge UDA from found underlying symbols sm data 
                        foreach (SecMasterBaseObj secMasterObj in secMasterDataWithDefautUDA)
                        {
                            String undelyingSymbol = secMasterObj.UnderLyingSymbol.Trim();
                            //Merge UDA from Underlying security 
                            if (foundsymbolsDataDict.ContainsKey(undelyingSymbol))
                            {
                                SecMasterDataCache.SetSymbolUDAData(foundsymbolsDataDict[undelyingSymbol], secMasterObj);

                                if (isDynamicUDADataRequired)
                                    SecMasterDataCache.SetDynamicUDAData(foundsymbolsDataDict[undelyingSymbol], secMasterObj);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// When ever some information is updated on Symbol lookup , it is published to the modules which are currently under pub sub architecture. 
        /// </summary>
        /// <param name="secMasterBaseObjlist"></param>
        private void PublishSecurityMasterData(SecMasterbaseList secMasterBaseObjlist)
        {

            try
            {
                MessageData securityMasterdata = new MessageData();
                securityMasterdata.EventData = secMasterBaseObjlist;
                securityMasterdata.TopicName = Topics.Topic_SecurityMaster;
                CentralizePublish(securityMasterdata);
                #region Compliance Section
                try
                {
                    foreach (SecMasterBaseObj secMasterObj in secMasterBaseObjlist)
                    {
                        SendSecurityDetailsToCompliance(secMasterObj);
                    }
                }
                catch (Exception ex)
                {
                    // Invoke our policy that is responsible for making sure no secure information
                    // gets out of our layer.
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                    if (rethrow)
                    {
                        throw;
                    }
                }
                #endregion
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

        private static void CentralizePublish(MessageData msgData)
        {
            try
            {
                lock (_publishLock)
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        try
                        {
                            _proxy.InnerChannel.Publish(msgData, msgData.TopicName);
                        }
                        catch (Exception ex)
                        {
                            bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                        }
                    });
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

        public async void SaveNewSymbolToSecurityMaster_Import(SecMasterbaseList secMasterData)
        {
            try
            {
                //chunkSize to Import SM data..
                int chunkSize = int.Parse(ConfigurationManager.AppSettings["SMImportChunkSize"]);
                chunkSize = (chunkSize >= MAXSMCHUNKSIZE) ? MAXSMCHUNKSIZE : chunkSize;
                List<List<SecMasterBaseObj>> smChunks = ChunkingManager.CreateChunks<SecMasterBaseObj>(secMasterData, chunkSize);
                int chunksProcessed = 0;

                foreach (List<SecMasterBaseObj> smChunk in smChunks)
                {
                    if (smChunk.Count < chunkSize)
                    {
                        chunkSize = smChunk.Count;
                    }

                    await System.Threading.Tasks.Task.Run(() => { SaveNewSymbolResponsetoSMAsync(smChunk); });
                    chunksProcessed++;

                    foreach (SecMasterBaseObj secMasterobj in smChunk)
                    {
                        InvokeSecMstrDataResponse(secMasterobj);
                    }
                    SecMasterServerComponent.GetInstance.SendDataToClient(smChunk, smChunks.Count, chunksProcessed, 0);
                    SecMasterDataCache.GetInstance.AddValues(smChunk, DateTime.UtcNow);
                }

                if (StatusOfRequest != null)
                {
                    StatusOfRequest(this, new EventArgs<string, string, string>("Success", secMasterData.UserID, secMasterData.RequestID));
                    foreach (SecMasterBaseObj secMasterobj in secMasterData)
                        _symbolCache.symbolFillData(secMasterobj);
                }
            }
            catch (Exception ex)
            {
                if (StatusOfRequest != null)
                {
                    if (ex.Message.Contains("Cannot insert duplicate key row") || ex.Message.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint"))
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>("Not Saved,Symbol already exist", secMasterData.UserID, secMasterData.RequestID));
                    }
                    else
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>(ex.Message, secMasterData.UserID, secMasterData.RequestID));
                    }
                }

                SecMasterServerComponent.GetInstance.SendDataToClient(null, 0, 0, 1);

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void SaveNewSymbolResponsetoSMAsync(List<SecMasterBaseObj> secMasterData)
        {
            try
            {
                foreach (SecMasterBaseObj secMasterobj in secMasterData)
                {
                    if (secMasterobj != null)
                    {
                        UpdateSecMasterObject(secMasterobj);
                        SecMasterDataCache.GetInstance.UpdateUDADataWithName(secMasterobj);
                    }
                }

                Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();

                // Updating Dynamic UDAs XML
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-9541
                String XmlString = _xml.WriteString(secMasterData);
                XmlString = XmlString.Replace("KeyValuePair`2", "KeyValuePair2");
                XmlString = XmlString.Replace("List`1", "List1");
                XDocument sesMasterDoc = XDocument.Parse(XmlString);
                foreach (SecMasterBaseObj obj in secMasterData)
                {
                    UpdateIssuer(obj);

                    SecMasterBaseObj secMasterObjClone = DeepCopyHelper.Clone(obj);
                    UpdateRiskCurrency(secMasterObjClone);

                    XElement secMasterObj = sesMasterDoc.Descendants("TickerSymbol")
                                   .Where(x => x.Value == secMasterObjClone.TickerSymbol.ToString())
                                   .SingleOrDefault();

                    if (secMasterObj != null)
                    {
                        XElement oldDynamicUDAXML = secMasterObj.Parent.Element("DynamicUDA");

                        XElement newDynamicUDAXML = new XElement("DynamicUDAs");
                        foreach (string key in secMasterObjClone.DynamicUDA.Keys) { newDynamicUDAXML.Add(new XElement(key, secMasterObjClone.DynamicUDA[key].ToString().Trim())); }

                        oldDynamicUDAXML.ReplaceWith(newDynamicUDAXML);
                    }

                }
                XmlString = sesMasterDoc.ToString(SaveOptions.DisableFormatting);

                SecMasterDataManager.SaveNewSymbolResponsetoSecurityMaster_Import(XmlString, (int)SecMasterConstants.SecMasterSourceOfData.ImportData);
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

        public void UpdateSymbolToSecurityMaster_Import(SecMasterUpdateDataByImportList secMasterData)
        {
            try
            {

                Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();
                String XmlString = _xml.WriteString(secMasterData);
                XmlString = XmlString.Replace("KeyValuePair`2", "KeyValuePair2");
                XmlString = XmlString.Replace("List`1", "List1");
                XDocument sesMasterDoc = XDocument.Parse(XmlString);
                foreach (SecMasterUpdateDataByImportUI obj in secMasterData)
                {
                    //UpdateIssuer(obj);

                    SecMasterUpdateDataByImportUI secMasterObjClone = DeepCopyHelper.Clone(obj);
                    // UpdateRiskCurrency(secMasterObjClone);

                    XElement secMasterObj = sesMasterDoc.Descendants("TickerSymbol")
                                   .Where(x => x.Value == secMasterObjClone.TickerSymbol.ToString())
                                   .SingleOrDefault();

                    if (secMasterObj != null)
                    {
                        XElement oldDynamicUDAXML = secMasterObj.Parent.Element("DynamicUDA");

                        XElement newDynamicUDAXML = new XElement("DynamicUDAs");
                        foreach (string key in secMasterObjClone.DynamicUDA.Keys) { newDynamicUDAXML.Add(new XElement(key, secMasterObjClone.DynamicUDA[key].ToString().Trim())); }

                        oldDynamicUDAXML.ReplaceWith(newDynamicUDAXML);
                    }

                }
                XmlString = sesMasterDoc.ToString(SaveOptions.DisableFormatting);
                SecMasterDataManager.UpdateSymbolToSecurityMaster_Import(XmlString, (int)SecMasterConstants.SecMasterSourceOfData.ImportData);

                DataTable dtSmEnrichData = null;
                dtSmEnrichData = GeneralUtilities.GetDataTableFromList(secMasterData);
                Dictionary<string, SerializableDictionary<String, Object>> dynamicUDADict = new Dictionary<string, SerializableDictionary<string, object>>();

                foreach (SecMasterUpdateDataByImportUI secMasterUpdateDataByImportUI in secMasterData)
                {
                    if (!dynamicUDADict.ContainsKey(secMasterUpdateDataByImportUI.TickerSymbol))
                    {
                        dynamicUDADict.Add(secMasterUpdateDataByImportUI.TickerSymbol, secMasterUpdateDataByImportUI.DynamicUDA);
                    }
                }

                if (dtSmEnrichData != null && dtSmEnrichData.Rows.Count > 0)
                {
                    SecMasterEnRichData.GetInstance.SetSMEnRichData(dtSmEnrichData);
                    //get symbol list
                    List<string> symbolList = GetSymbolList(secMasterData);
                    //get secmaster objects
                    SecMasterbaseList secmasterList = new SecMasterbaseList();
                    List<SecMasterBaseObj> secMasterObjList = GetSecMasterDataForListSync(symbolList, ApplicationConstants.SymbologyCodes.TickerSymbol, 0);
                    if (secMasterObjList != null && secMasterObjList.Count > 0)
                    {
                        //Enrich sec master object               
                        foreach (SecMasterBaseObj secMasterobj in secMasterObjList)
                        {
                            SecMasterEnRichData.GetInstance.EnRichSecMasterObject(secMasterobj);
                            //Enrich Sec Master Obj with UDAs and Dynamic UDAs.
                            SecMasterEnRichData.GetInstance.EnrichUDAsandDynamicUDAs(secMasterobj, dynamicUDADict[secMasterobj.TickerSymbol]);
                            SecMasterEnRichData.GetInstance.DeleteSMEnRichCachedData(secMasterobj.TickerSymbol);
                        }
                        // update sec master cache
                        SecMasterDataCache.GetInstance.AddValues(secMasterObjList, DateTime.UtcNow);

                        foreach (SecMasterBaseObj secMasterobj in secMasterObjList)
                        {
                            secmasterList.Add(secMasterobj);
                            InvokeSecMstrDataResponse(secMasterobj);
                            SecMasterServerComponent.GetInstance.SendDataToClient(secMasterobj);
                        }
                    }

                    if (StatusOfRequest != null)
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>("Success", secMasterData.UserID, secMasterData.RequestID));
                        foreach (SecMasterBaseObj secMasterobj in secMasterObjList)
                            _symbolCache.symbolFillData(secMasterobj);
                    }

                    PublishSecurityMasterData(secmasterList);
                }
            }
            catch (Exception ex)
            {
                if (StatusOfRequest != null)
                {
                    if (ex.Message.Contains("Cannot insert duplicate key row") || ex.Message.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint"))
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>("Not Saved,Symbol already exist", secMasterData.UserID, secMasterData.RequestID));
                    }
                    else
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>(ex.Message, secMasterData.UserID, secMasterData.RequestID));
                    }
                }
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private List<string> GetSymbolList(SecMasterUpdateDataByImportList secMasterData)
        {
            List<string> symbolList = new List<string>();
            try
            {
                foreach (SecMasterUpdateDataByImportUI secObj in secMasterData)
                {
                    symbolList.Add(secObj.TickerSymbol);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            return symbolList;
        }

        /// <summary>
        /// Search security from DB, when request from symbol lookup UI. 
        /// </summary>
        /// <param name="symbolLookupReqObj"></param>
        public async void GetSymbolLookupRequestedData(SymbolLookupRequestObject symbolLookupReqObj)
        {
            try
            {
                if (symbolLookupReqObj != null)
                {
                    //Modified by omshiv, May 2014, based on release type, we will fetch security from local DB or CSM 

                    //checking request is exact match search or contains etc
                    bool isExactSearch = false;
                    if (_pricingSource == PranaPricingSource.Bloomberg)
                        isExactSearch = CheckExactSearchFromRequest(symbolLookupReqObj);

                    // if not exact search and full scan request then send request to CSM
                    //else if exact seach only then sacn in local DB and if not found then send to CSM
                    //else search only in local DB for Prana release case.
                    if (!isExactSearch && symbolLookupReqObj.IsFullScan)
                    {
                        try
                        {
                            InstanceContext context = new InstanceContext(this);
                            CentralSMService.CentralSMServiceClient centralSMServiceClient = new CentralSMService.CentralSMServiceClient(context, "WSDualHttpBinding_ICentralSMService");
                            await centralSMServiceClient.SearchSecurityAsync(symbolLookupReqObj, null);
                        }
                        catch (Exception ex)
                        {
                            // Invoke our policy that is responsible for making sure no secure information
                            // gets out of our layer.
                            Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                            bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDTHROW);
                            if (rethrow)
                            {
                                throw pricingConnError;
                            }
                        }
                    }
                    else
                    {
                        // get data from db async
                        BackgroundWorker bkWrkSymbolForDbAccess = new BackgroundWorker();
                        bkWrkSymbolForDbAccess.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkWrkSymbolForDbAccess_RunWorkerCompleted);
                        bkWrkSymbolForDbAccess.DoWork += new DoWorkEventHandler(bkWrkSymbolForDbAccess_DoWork);
                        System.Collections.ArrayList arguments = new System.Collections.ArrayList();
                        arguments.Add(symbolLookupReqObj);
                        bkWrkSymbolForDbAccess.RunWorkerAsync(arguments);
                    }
                }


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbolLookupReqObj"></param>
        /// <returns></returns>
        private bool CheckExactSearchFromRequest(SymbolLookupRequestObject symbolLookupReqObj)
        {
            bool isExactSearch = true;

            try
            {
                if (symbolLookupReqObj.BBGID.Contains('%')
                    || (symbolLookupReqObj.BloombergSymbol != null && symbolLookupReqObj.BloombergSymbol.Contains('%'))
                    || (symbolLookupReqObj.CUSIPSymbol != null && symbolLookupReqObj.CUSIPSymbol.Contains('%'))
                    || (symbolLookupReqObj.IDCOOptionSymbol != null && symbolLookupReqObj.IDCOOptionSymbol.Contains('%'))
                    || (symbolLookupReqObj.ISINSymbol != null && symbolLookupReqObj.ISINSymbol.Contains('%'))
                    || (symbolLookupReqObj.Name != null && symbolLookupReqObj.Name.Contains('%'))
                    || (symbolLookupReqObj.OPRAOptionSymbol != null && symbolLookupReqObj.OPRAOptionSymbol.Contains('%'))
                    || (symbolLookupReqObj.OSIOptionSymbol != null && symbolLookupReqObj.OSIOptionSymbol.Contains('%'))
                    || (symbolLookupReqObj.ReutersSymbol != null && symbolLookupReqObj.ReutersSymbol.Contains('%'))
                    || (symbolLookupReqObj.SEDOLSymbol != null && symbolLookupReqObj.SEDOLSymbol.Contains('%'))
                    || (symbolLookupReqObj.TickerSymbol != null && symbolLookupReqObj.TickerSymbol.Contains('%'))
                    || (symbolLookupReqObj.Underlying != null && symbolLookupReqObj.Underlying.Contains('%'))
                    || (symbolLookupReqObj.UDAAsset != null && symbolLookupReqObj.UDAAsset.Contains('%'))
                    || (symbolLookupReqObj.UDACountry != null && symbolLookupReqObj.UDACountry.Contains('%'))
                    || (symbolLookupReqObj.UDASecurityType != null && symbolLookupReqObj.UDASecurityType.Contains('%'))
                    || (symbolLookupReqObj.UDASubSector != null && symbolLookupReqObj.UDASubSector.Contains('%'))
                    || (symbolLookupReqObj.Underlying != null && symbolLookupReqObj.Underlying.Contains('%'))
                    )
                {
                    isExactSearch = false;

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
            return isExactSearch;
        }

        public bool GetSymbolLookupRequestedData(string UnderLyingSymbol, string connString)
        {
            DataSet ds = new DataSet();
            try
            {

                ds = SecMasterDataManager.GetUnderLyingSymbolDetails(UnderLyingSymbol, connString);

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Send requested SM data to symbol lookup UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bkWrkSymbolForDbAccess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }
            object[] result = new object[2];
            result = (object[])e.Result;
            DataSet _ds = (DataSet)result[0];
            SymbolLookupRequestObject symbolLookupReqObj = (SymbolLookupRequestObject)result[1];
            try
            {

                if (!e.Cancelled) // no error
                {
                    if (StatusOfRequest != null)
                    {
                        SymbolLkUpDataResponse(this, new EventArgs<DataSet, SymbolLookupRequestObject>(_ds, symbolLookupReqObj));
                    }
                    if (StatusOfRequest != null && _ds.Tables[0].Rows.Count > 0)
                    {

                        StatusOfRequest(this, new EventArgs<string, string, string>("Success", symbolLookupReqObj.CompanyUserID, symbolLookupReqObj.RequestID));
                    }
                    //modified by omshiv,only for exact search it will goes to CSM if data not found in local db is case of CH
                    else if (_pricingSource == PranaPricingSource.Bloomberg)
                    {
                        bool isExactSearch = CheckExactSearchFromRequest(symbolLookupReqObj);
                        if (isExactSearch)
                            SendReguestToCSMServer(symbolLookupReqObj);
                    }
                    else
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>("No Data found", symbolLookupReqObj.CompanyUserID, symbolLookupReqObj.RequestID));
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                if (StatusOfRequest != null)
                {
                    StatusOfRequest(this, new EventArgs<string, string, string>(ex.Message, symbolLookupReqObj.CompanyUserID, symbolLookupReqObj.RequestID));
                }
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private async void SendReguestToCSMServer(SymbolLookupRequestObject symbolLookupReqObj)
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                CentralSMService.CentralSMServiceClient centralSMServiceClient = new CentralSMService.CentralSMServiceClient(context, "WSDualHttpBinding_ICentralSMService");
                await centralSMServiceClient.SearchSecurityAsync(symbolLookupReqObj, null);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw pricingConnError;
                }
            }
        }

        /// <summary>
        /// Getting SM data from DB when request from Symbol lookup UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bkWrkSymbolForDbAccess_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet _ds = new DataSet();
            System.Collections.ArrayList arguments = (System.Collections.ArrayList)e.Argument;
            SymbolLookupRequestObject symbolLookupReqObj = (SymbolLookupRequestObject)arguments[0];

            object[] result = new object[2];
            try
            {

                Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();
                _ds = SecMasterDataManager.GetSymbolLookupRequestedData(_xml.WriteString(symbolLookupReqObj));

                result[0] = _ds;
                result[1] = symbolLookupReqObj;

                e.Result = result;
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                e.Result = ex.Message;
                if (StatusOfRequest != null)
                {
                    StatusOfRequest(this, new EventArgs<string, string, string>("Problem on Server", symbolLookupReqObj.CompanyUserID, symbolLookupReqObj.RequestID));
                }
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
        }

        private string _connectionStr = "";

        public string ConnectionString
        {
            get { return _connectionStr; }
            set { _connectionStr = value; }
        }

        #region getting data for a symbol
        /// <summary>
        /// async database access
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void bkWrkForDbAccess_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                System.Collections.ArrayList arguments = (System.Collections.ArrayList)e.Argument;
                SecMasterRequestObj secMasterRequestObj = (SecMasterRequestObj)arguments[0];
                DateTime dateTime = (DateTime)arguments[1];
                int senderCode = (int)arguments[2];
                List<SecMasterBaseObj> secMasterObjList = null;
                lock (lockerDbAccess)
                {
                    if (_connectionStr == string.Empty)
                    {
                        secMasterObjList = SecMasterDataManager.GetSecMasterDataFromDB_XML(secMasterRequestObj);
                    }
                    else
                    {
                        secMasterObjList = SecMasterDataManager.GetSecMasterDataFromDB_XML(secMasterRequestObj, _connectionStr);
                    }
                }
                //make call to LiveFeed provider for fetching data Async as data for these symbols was not found in cahce or SecmasterDB...
                //List<SecMasterBaseObj> secMasterObjList = SecMasterDataManager.GetSecMasterDataFromDB_XML_Date(secMasterRequestObj, dateTime);
                if (secMasterObjList.Count > 0)
                {
                    foreach (SecMasterBaseObj secMasterBaseObj in secMasterObjList)
                    {
                        SymbolDataRow datarow = secMasterRequestObj.GetSymbolRow(secMasterBaseObj);
                        if (datarow != null)
                        {
                            secMasterBaseObj.RequestedSymbology = (int)datarow.PrimarySymbology;
                            secMasterBaseObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.Database;
                            secMasterBaseObj.RequestedHashcode = secMasterRequestObj.RequestID;
                            secMasterBaseObj.RequestedUserID = secMasterRequestObj.UserID;

                            InvokeSecMstrDataResponse(secMasterBaseObj);
                            secMasterRequestObj.Remove(datarow);
                            // remove symbols from table if exists in the sec master db
                            if (SecMasterEnRichData.GetInstance.CheckSMEnRichRequires())
                            {
                                SecMasterEnRichData.GetInstance.DeleteSMEnRichCachedData(secMasterBaseObj.TickerSymbol);
                            }
                        }
                        #region Compliance Section
                        SendSecurityDetailsToCompliance(secMasterBaseObj);
                        #endregion
                    }

                    SecMasterDataCache.GetInstance.AddValues(secMasterObjList, dateTime);
                }


                if (secMasterRequestObj.Count > 0)
                {
                    SecMasterRequestObj SecMasterReqOld = (SecMasterRequestObj)secMasterRequestObj.Clone();
                    bool isTransaformationRequired = bool.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsSymbolTransformationRequired"));
                    if (isTransaformationRequired)
                    {
                        SymbolTransformer.TransformSymbol(secMasterRequestObj);
                    }
                    SecMasterDataCache.GetInstance.RequestSymbolData(SecMasterReqOld, secMasterRequestObj);

                    //modified by omshiv,
                    if (_pricingSource == PranaPricingSource.Bloomberg)
                    {
                        CentralSMDataCache.Instance.AddAndModifySymbolRequestsForCentralSM(ref secMasterRequestObj);
                        try
                        {
                            if (secMasterRequestObj.Count > 0 && CentralSMCommunicationManager.Instance.IsCSMConnected)
                            {
                                InstanceContext context = new InstanceContext(this);
                                CentralSMService.CentralSMServiceClient centralSMServiceClient = new CentralSMService.CentralSMServiceClient(context, "WSDualHttpBinding_ICentralSMService");
                                await centralSMServiceClient.GetSecMasterDataCentralSMAsync(secMasterRequestObj, ServerIdentifier, null);
                            }
                            else
                            {
                                InformationReporter.GetInstance.Write("CSM server is not connected. Please contact to Admin " + DateTime.UtcNow);
                                if (secMasterRequestObj.IsSearchInLocalOnly)
                                {
                                    // StatusOfRequest(this, new EventArgs<string, string, string>("Result not found", secMasterRequestObj.UserID, secMasterRequestObj.RequestID));
                                    SymbolLookupRequestObject symbolLookupReqObj = new SymbolLookupRequestObject();
                                    symbolLookupReqObj.RequestID = secMasterRequestObj.RequestID;
                                    symbolLookupReqObj.CompanyUserID = secMasterRequestObj.UserID;
                                    DataSet ds = new DataSet();
                                    SymbolLkUpDataResponse(this, new EventArgs<DataSet, SymbolLookupRequestObject>(ds, symbolLookupReqObj));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Invoke our policy that is responsible for making sure no secure information
                            // gets out of our layer.
                            Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                            bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDSHOW);
                            if (rethrow)
                            {
                                throw pricingConnError;
                            }
                        }
                    }
                    else
                    {
                        if (secMasterRequestObj.UseOptionManualvalidation)
                        {
                            for (int i = 0; i < secMasterRequestObj.SymbolDataRowCollection.Count; i++)
                            {
                                var opt = secMasterRequestObj.SymbolDataRowCollection[i];
                                OptionDetail optionDetail = OptionSymbolGenerator.GetOptionDetailObj(opt.PrimarySymbol, opt.PrimarySymbology, opt.UnderlyingSymbol);
                                if (optionDetail == null || optionDetail.StrikePrice.Equals(0))
                                    continue;

                                //For getting the underlying symbol of the international equity option in the case underlying not present
                                if (string.IsNullOrEmpty(opt.UnderlyingSymbol) && opt.PrimarySymbol.Contains('-'))
                                {
                                    SymbolLookupRequestObject reqObjforUnderlying = new SymbolLookupRequestObject();
                                    reqObjforUnderlying.RequestID = secMasterRequestObj.RequestID;
                                    reqObjforUnderlying.CompanyUserID = secMasterRequestObj.UserID;
                                    reqObjforUnderlying.StartIndex = 1;
                                    reqObjforUnderlying.EndIndex = 2;
                                    reqObjforUnderlying.TickerSymbol = optionDetail.UnderlyingSymbol + "-%";
                                    DataSet ds = new DataSet();
                                    Prana.Utilities.XMLUtilities.CustomXmlSerializer _xml = new Prana.Utilities.XMLUtilities.CustomXmlSerializer();
                                    ds = SecMasterDataManager.GetSymbolLookupRequestedData(_xml.WriteString(reqObjforUnderlying));
                                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                                        optionDetail.UnderlyingSymbol = ds.Tables[0].Rows[0]["UnderlyingSymbol"].ToString();
                                }

                                SecMasterRequestObj underLyingSymbolReqObj = new SecMasterRequestObj();
                                underLyingSymbolReqObj.AddData(optionDetail.UnderlyingSymbol, (ApplicationConstants.SymbologyCodes)opt.PrimarySymbology);
                                underLyingSymbolReqObj.HashCode = GetHashCode();
                                underLyingSymbolReqObj.UserID = secMasterRequestObj.UserID;

                                if (_connectionStr == string.Empty)
                                    secMasterObjList = SecMasterDataManager.GetSecMasterDataFromDB_XML(underLyingSymbolReqObj);
                                else
                                    secMasterObjList = SecMasterDataManager.GetSecMasterDataFromDB_XML(underLyingSymbolReqObj, _connectionStr);

                                if (secMasterObjList.Count > 0)
                                {
                                    SecMasterBaseObj underLyingSecMasterBaseObj = secMasterObjList[0];
                                    OptionSymbolData symbolData = new OptionSymbolData();
                                    symbolData.RequestedSymbology = (ApplicationConstants.SymbologyCodes)opt.PrimarySymbology;
                                    symbolData.CategoryCode = AssetCategory.EquityOption;
                                    symbolData.UnderlyingSymbol = underLyingSecMasterBaseObj.UnderLyingSymbol;
                                    symbolData.UnderlyingCategory = (Underlying)underLyingSecMasterBaseObj.UnderLyingID;
                                    symbolData.CurencyCode = CachedDataManager.GetInstance.GetCurrencyText(underLyingSecMasterBaseObj.CurrencyID);
                                    symbolData.ExpirationDate = optionDetail.ExpirationDate;
                                    symbolData.PutOrCall = optionDetail.OptionType;
                                    symbolData.StrikePrice = optionDetail.StrikePrice;
                                    symbolData.FullCompanyName = symbolData.UnderlyingSymbol + " " + symbolData.ExpirationDate.Date.ToString("MM/dd/yy") + " " + symbolData.StrikePrice + " " + symbolData.PutOrCall;
                                    symbolData.Multiplier = 100;

                                    if (symbolData.RequestedSymbology == ApplicationConstants.SymbologyCodes.TickerSymbol)
                                    {
                                        symbolData.Symbol = opt.PrimarySymbol;
                                        optionDetail.AUECID = underLyingSecMasterBaseObj.AUECID;
                                        optionDetail.AssetCategory = underLyingSecMasterBaseObj.AssetCategory;
                                        optionDetail.Symbology = ApplicationConstants.SymbologyCodes.BloombergSymbol;
                                        OptionSymbolGenerator.GetOptionSymbol(optionDetail);
                                        symbolData.BloombergSymbol = optionDetail.Symbol;
                                    }
                                    else if (symbolData.RequestedSymbology == ApplicationConstants.SymbologyCodes.BloombergSymbol)
                                    {
                                        symbolData.BloombergSymbol = opt.PrimarySymbol;
                                        optionDetail.AUECID = underLyingSecMasterBaseObj.AUECID;
                                        optionDetail.AssetCategory = underLyingSecMasterBaseObj.AssetCategory;
                                        optionDetail.Symbology = (int)ApplicationConstants.SymbologyCodes.TickerSymbol;
                                        OptionSymbolGenerator.GetOptionSymbol(optionDetail);
                                        symbolData.Symbol = optionDetail.Symbol;
                                    }
                                    string[] tickerStrArr = symbolData.Symbol.Split(new char[] { '-' });
                                    if (tickerStrArr.Length == 2)
                                    {
                                        string exchangeIdentifire = tickerStrArr[1] + "-" + symbolData.CategoryCode;
                                        symbolData.AUECID = CachedDataManager.GetInstance.GetAUECIdByExchangeIdentifier(exchangeIdentifire);
                                    }
                                    else
                                    {
                                        symbolData.AUECID = SecMasterConstants.DefaultOptionAUECID;
                                        symbolData.OSIOptionSymbol = OptionSymbolGenerator.GenerateOSISymbol(optionDetail);
                                        symbolData.IDCOOptionSymbol = symbolData.OSIOptionSymbol + "U";
                                    }
                                    symbolData.ExchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(symbolData.AUECID);

                                    MarketDataSymbolResponse marketDataSymbolResponseReq = new MarketDataSymbolResponse();
                                    marketDataSymbolResponseReq.TickerSymbol = symbolData.Symbol;
                                    MarketDataSymbolResponse marketDataSymbolResponse = MarketDataSymbolGenerator.GetMarketDataSymbolFromTickerSymbol(marketDataSymbolResponseReq, CachedDataManager.CompanyMarketDataProvider, SecMasterDataCache.GetInstance.GetFutSymbolRootdata(marketDataSymbolResponseReq.TickerSymbol));

                                    if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.FactSet && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.FactSetSymbol))
                                    {
                                        symbolData.FactSetSymbol = marketDataSymbolResponse.FactSetSymbol;
                                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("FactSet symbol generated from AUEC mapping for Ticker Symbol: " + symbolData.Symbol + ", FactSet Symbol: " + marketDataSymbolResponse.FactSetSymbol, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                    }
                                    else if (CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.ACTIV && !string.IsNullOrWhiteSpace(marketDataSymbolResponse.ActivSymbol))
                                    {
                                        symbolData.ActivSymbol = marketDataSymbolResponse.ActivSymbol;
                                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("ACTIV symbol generated from AUEC mapping for Ticker Symbol: " + symbolData.Symbol + ", ACTIV Symbol: " + marketDataSymbolResponse.ActivSymbol, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                                    }
                                    secMasterRequestObj.Remove(opt);
                                    i--;
                                    PublishLevel1SnapshotResponse(symbolData, true, secMasterRequestObj.UserID, secMasterRequestObj.RequestID);
                                }
                                else
                                {
                                    Logger.HandleException(new Exception(opt.PrimarySymbol + " option, not validate because its underlying symbol(" + optionDetail.UnderlyingSymbol + ") is not present in Security master."), LoggingConstants.POLICY_LOGANDSHOW);
                                }
                            }
                        }
                        // modified by omshiv, request will sent to pricing( e-signal) if IsSearchInLocalOnly is false in request object.
                        // in some case,we just want to check data exist in our system or not. so in such scenario we will not sent request to e-signal.
                        if (!secMasterRequestObj.IsSearchInLocalOnly)
                        {
                            GetSecMasterDataFromLiveFeed(secMasterRequestObj);
                        }
                        else
                        {
                            SymbolLookupRequestObject symbolLookupReqObj = new SymbolLookupRequestObject();
                            symbolLookupReqObj.RequestID = secMasterRequestObj.RequestID;
                            symbolLookupReqObj.CompanyUserID = secMasterRequestObj.UserID;
                            DataSet ds = new DataSet();
                            SymbolLkUpDataResponse(this, new EventArgs<DataSet, SymbolLookupRequestObject>(ds, symbolLookupReqObj));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sends the security details to compliance, PRANA-21715
        /// </summary>
        /// <param name="secMasterBaseObj">The sec master base object.</param>
        private void SendSecurityDetailsToCompliance(SecMasterBaseObj secMasterBaseObj)
        {
            try
            {
                SecMasterBaseObj secMasterObjClone = DeepCopyHelper.Clone(secMasterBaseObj);
                if (secMasterObjClone.RequestedSymbology != Convert.ToInt32(ApplicationConstants.SymbologyCodes.TickerSymbol))
                {
                    //If ticker doesnot exist for security, then don't update requested symbology 
                    if (!string.IsNullOrWhiteSpace(secMasterObjClone.TickerSymbol))
                        secMasterObjClone.RequestedSymbology = Convert.ToInt32(ApplicationConstants.SymbologyCodes.TickerSymbol);
                }

                //If ticker exist for security then only send symbol data to compliance
                if (secMasterObjClone.RequestedSymbology == Convert.ToInt32(ApplicationConstants.SymbologyCodes.TickerSymbol))
                {
                    if (SecurityObjectReceived != null)
                        SecurityObjectReceived(this, new EventArgs<SecMasterBaseObj>(secMasterObjClone));
                    if (AuecDetailsUpdated != null)
                        AuecDetailsUpdated(this, new EventArgs<AuecDetails>(SecMasterDataCache.GetInstance.GetAuecDetails(secMasterObjClone.AUECID)));
                    //Checking if udacollection not already contains same ticker symbol and compliance is enabled
                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-2456
                    //if (UDADataReceived != null && !udaCollection.ContainsKey(secMasterObjClone.TickerSymbol))
                    //    udaCollection.Add(secMasterObjClone.TickerSymbol, CachedDataManager.GetInstance.GetUDAData(secMasterObjClone.TickerSymbol));
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region getting data for list of symbols
        private void GetSecMasterDataFromLiveFeed(SecMasterRequestObj secMasterRequestObj)
        {
            try
            {
                Dictionary<ApplicationConstants.SymbologyCodes, List<string>> symbologySymbolsMapping = new Dictionary<ApplicationConstants.SymbologyCodes, List<string>>();
                foreach (SymbolDataRow symbolDataRow in secMasterRequestObj.SymbolDataRowCollection)
                {
                    switch (symbolDataRow.PrimarySymbology)
                    {
                        case ApplicationConstants.SymbologyCodes.TickerSymbol:
                            if (!String.IsNullOrEmpty(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.TickerSymbol]))
                            {
                                if (symbologySymbolsMapping.ContainsKey(ApplicationConstants.SymbologyCodes.TickerSymbol))
                                {
                                    if (!symbologySymbolsMapping[ApplicationConstants.SymbologyCodes.TickerSymbol].Contains(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.TickerSymbol]))
                                        symbologySymbolsMapping[ApplicationConstants.SymbologyCodes.TickerSymbol].Add(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.TickerSymbol]);
                                }
                                else
                                {
                                    symbologySymbolsMapping.Add(ApplicationConstants.SymbologyCodes.TickerSymbol, new List<string>() { symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.TickerSymbol] });
                                }
                            }
                            break;
                        case ApplicationConstants.SymbologyCodes.FactSetSymbol:
                            if (!String.IsNullOrEmpty(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.FactSetSymbol]))
                            {
                                if (symbologySymbolsMapping.ContainsKey(ApplicationConstants.SymbologyCodes.FactSetSymbol))
                                {
                                    if (!symbologySymbolsMapping[ApplicationConstants.SymbologyCodes.FactSetSymbol].Contains(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.FactSetSymbol]))
                                        symbologySymbolsMapping[ApplicationConstants.SymbologyCodes.FactSetSymbol].Add(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.FactSetSymbol]);
                                }
                                else
                                {
                                    symbologySymbolsMapping.Add(ApplicationConstants.SymbologyCodes.FactSetSymbol, new List<string>() { symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.FactSetSymbol] });
                                }
                            }
                            break;
                        case ApplicationConstants.SymbologyCodes.ActivSymbol:
                            if (!String.IsNullOrEmpty(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.ActivSymbol]))
                            {
                                if (symbologySymbolsMapping.ContainsKey(ApplicationConstants.SymbologyCodes.ActivSymbol))
                                {
                                    if (!symbologySymbolsMapping[ApplicationConstants.SymbologyCodes.ActivSymbol].Contains(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.ActivSymbol]))
                                        symbologySymbolsMapping[ApplicationConstants.SymbologyCodes.ActivSymbol].Add(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.ActivSymbol]);
                                }
                                else
                                {
                                    symbologySymbolsMapping.Add(ApplicationConstants.SymbologyCodes.ActivSymbol, new List<string>() { symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.ActivSymbol] });
                                }
                            }
                            break;
                        case ApplicationConstants.SymbologyCodes.IDCOOptionSymbol:
                            if (!String.IsNullOrEmpty(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol]))
                            {
                                if (symbologySymbolsMapping.ContainsKey(ApplicationConstants.SymbologyCodes.IDCOOptionSymbol))
                                {
                                    if (!symbologySymbolsMapping[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].Contains(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol]))
                                        symbologySymbolsMapping[ApplicationConstants.SymbologyCodes.IDCOOptionSymbol].Add(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol]);
                                }
                                else
                                {
                                    symbologySymbolsMapping.Add(ApplicationConstants.SymbologyCodes.IDCOOptionSymbol, new List<string>() { symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol] });
                                }
                            }
                            break;
                        case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                            if (!String.IsNullOrEmpty(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol]))
                            {
                                if (symbologySymbolsMapping.ContainsKey(ApplicationConstants.SymbologyCodes.BloombergSymbol))
                                {
                                    if (!symbologySymbolsMapping[ApplicationConstants.SymbologyCodes.BloombergSymbol].Contains(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol]))
                                        symbologySymbolsMapping[ApplicationConstants.SymbologyCodes.BloombergSymbol].Add(symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol]);
                                }
                                else
                                {
                                    symbologySymbolsMapping.Add(ApplicationConstants.SymbologyCodes.BloombergSymbol, new List<string>() { symbolDataRow.SymbolData[(int)ApplicationConstants.SymbologyCodes.BloombergSymbol] });
                                }
                            }
                            break;
                    }
                }

                try
                {
                    if (symbologySymbolsMapping.Count > 0)
                    {
                        foreach (KeyValuePair<ApplicationConstants.SymbologyCodes, List<string>> symbols in symbologySymbolsMapping)
                            _pricingServicesProxy.InnerChannel.RequestSMData(symbols.Value, symbols.Key);
                    }
                }
                catch
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("PricingService2 not connected", LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
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

        #endregion

        #region public methods



        /// <summary>
        /// Get SecMaster Data by ticker symbol
        /// </summary>
        /// <param name="Symbol"></param>
        /// <param name="date"></param>
        /// <param name="senderCode"></param>
        public void GetSecMasterData(string Symbol, DateTime date, int senderCode)
        {
            GetSecMasterData(Symbol, date, ApplicationConstants.SymbologyCodes.TickerSymbol, senderCode);
        }

        /// <summary>
        ///  Get SecMaster Data by symbol and symbology
        /// </summary>
        /// <param name="Symbol"></param>
        /// <param name="date"></param>
        /// <param name="symbology"></param>
        /// <param name="senderCode"></param>
        public void GetSecMasterData(string Symbol, DateTime date, ApplicationConstants.SymbologyCodes symbology, int senderCode)
        {
            SecMasterRequestObj secMasterReqObj = new SecMasterRequestObj();

            secMasterReqObj.AddData(Symbol, symbology);
            GetSecMasterData(secMasterReqObj, date, senderCode);
        }

        /// <summary>
        /// Get SecMaster Data by SM request Object
        /// </summary>
        /// <param name="secMasterRequestObj"></param>
        /// <param name="senderCode"></param>
        public void GetSecMasterData(SecMasterRequestObj secMasterRequestObj, int senderCode)
        {

            GetSecMasterData(secMasterRequestObj, DateTime.UtcNow, senderCode);

        }

        /// <summary>
        /// Saves the share out standing in sec master.
        /// </summary>
        /// <param name="shareOutStandingSymbolWise">The share out standing symbol wise.</param>
        /// <param name="userId">The user identifier.</param>
        public void SaveShareOutStandingInSecMaster(string symbol, double sharesOutStanding, string userId)
        {
            try
            {
                SecMasterbaseList lst = new SecMasterbaseList();
                lst.RequestID = System.Guid.NewGuid().ToString();
                lst.UserID = userId.ToString();
                SecMasterBaseObj secMaster = GetSecMasterDataForSymbol(symbol);
                if (secMaster != null)
                {
                    secMaster.SharesOutstanding = sharesOutStanding;
                    secMaster.SymbolType = (int)SymbolType.Updated;
                    secMaster.ModifiedBy = CachedDataManager.GetInstance.GetUserText(Convert.ToInt32(userId)) + "_" + CachedDataManager.GetCompanyText(CachedDataManager.GetInstance.GetCompanyID());
                    secMaster.ModifiedDate = DateTime.Now;
                    lst.Add(secMaster);
                }
                SaveSecurityInSecurityMaster(lst);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get SecMaster Data by SM request Object and specific date
        /// </summary>
        /// <param name="secMasterRequestObj"></param>
        /// <param name="date"></param>
        /// <param name="senderCode"></param>
        public void GetSecMasterData(SecMasterRequestObj secMasterRequestObj, DateTime date, int senderCode)
        {
            try
            {
                secMasterRequestObj.RequestedDate = date;

                //first traverse cache to check if data exist before making a db call..
                List<SecMasterBaseObj> secMasterData = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterRequestObj, date);
                // register these symbols for async return 
                SecMasterDataCache.GetInstance.RequestSymbolData(secMasterRequestObj, null);

                foreach (SecMasterBaseObj secMasterBaseObj in secMasterData)
                {
                    //remove symbol from SM request list if symbol found.. 
                    SymbolDataRow datarow = secMasterRequestObj.GetSymbolRow(secMasterBaseObj);
                    if (datarow != null)
                    {
                        secMasterBaseObj.RequestedSymbology = (int)datarow.PrimarySymbology;
                        secMasterBaseObj.RequestedHashcode = secMasterRequestObj.RequestID;
                        secMasterBaseObj.RequestedUserID = secMasterRequestObj.UserID;

                        InvokeSecMstrDataResponse(secMasterBaseObj);
                        secMasterRequestObj.Remove(datarow);
                        if (SecMasterEnRichData.GetInstance.CheckSMEnRichRequires())
                        {
                            SecMasterEnRichData.GetInstance.DeleteSMEnRichCachedData(secMasterBaseObj.TickerSymbol);
                        }
                    }
                }

                //if some symbols still exist for which data was not found in cache then make a db call for these..
                if (secMasterRequestObj.Count > 0)
                {
                    // get data from db async
                    BackgroundWorker bkWrkForDbAccess = new BackgroundWorker();

                    // if symbol not found in DB then call live feed async
                    bkWrkForDbAccess.DoWork += new DoWorkEventHandler(bkWrkForDbAccess_DoWork);
                    System.Collections.ArrayList arguments = new System.Collections.ArrayList();
                    arguments.Add(secMasterRequestObj);
                    arguments.Add(date);
                    arguments.Add(senderCode);
                    bkWrkForDbAccess.RunWorkerAsync(arguments);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get SecMaster Data by symbols list and symbol code, sync
        /// </summary>
        /// <param name="symbolList"></param>
        /// <param name="symbologyCode"></param>
        /// <param name="senderCode"></param>
        /// <returns></returns>
        public List<SecMasterBaseObj> GetSecMasterDataForListSync(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode, int senderCode)
        {
            SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
            foreach (string symbol in symbolList)
            {
                secMasterRequestObj.AddData(symbol, symbologyCode);
                //secMasterRequestObj.AddNewRow();
            }
            return GetSecMasterDataForListSync(secMasterRequestObj, senderCode);
        }

        /// <summary>
        /// Get SecMaster Data by SM Request object, sync
        /// </summary>
        /// <param name="Symbollist"></param>
        public List<SecMasterBaseObj> GetSecMasterDataForListSync(SecMasterRequestObj secMasterRequestObj, int senderCode)
        {
            List<SecMasterBaseObj> totalData = new List<SecMasterBaseObj>();
            try
            {
                totalData = SecMasterDataCache.GetInstance.GetSecMasterData(secMasterRequestObj, DateTime.UtcNow);

                foreach (SecMasterBaseObj secMasterBaseObj in totalData)
                {
                    SymbolDataRow datarow = secMasterRequestObj.GetSymbolRow(secMasterBaseObj);
                    if (datarow != null)
                    {
                        secMasterRequestObj.Remove(datarow);
                    }
                }
                if (secMasterRequestObj.Count == 0)
                    return totalData;

                //if still data not found in cache and then call to DB
                List<SecMasterBaseObj> dataFoundInDb;
                if (_connectionStr == string.Empty)
                {
                    dataFoundInDb = SecMasterDataManager.GetSecMasterDataFromDB_XML(secMasterRequestObj);
                }
                else
                {
                    dataFoundInDb = SecMasterDataManager.GetSecMasterDataFromDB_XML(secMasterRequestObj, _connectionStr);
                }


                foreach (SecMasterBaseObj secMasterBaseObj in dataFoundInDb)
                {
                    //SourceOfData Was taking default value as None while filling Cache.
                    //So Updated that (Updated by Faisal Gani Shah)
                    secMasterBaseObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.Database;
                    SecMasterDataCache.GetInstance.AddValues(secMasterBaseObj, DateTime.UtcNow);

                    SymbolDataRow datarow = secMasterRequestObj.GetSymbolRow(secMasterBaseObj);
                    if (datarow != null)
                    {
                        secMasterRequestObj.Remove(datarow);
                    }
                    totalData.Add(secMasterBaseObj);
                }
                if (secMasterRequestObj.Count > 0)
                {
                    SecMasterDataCache.GetInstance.RequestSymbolData(secMasterRequestObj, null);

                    if (_pricingSource == PranaPricingSource.Bloomberg)
                    {
                        GetSecMasterDataFromCentralSM(secMasterRequestObj);
                    }
                    else
                    {
                        if (!secMasterRequestObj.IsSearchInLocalOnly)
                        {
                            GetSecMasterDataFromLiveFeed(secMasterRequestObj);
                        }
                    }
                }
                return totalData;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return totalData;

        }

        /// <summary>
        /// Get secMaster data from Bloomberg
        /// </summary>
        /// <param name="secMasterRequestObj"></param>
        async void GetSecMasterDataFromCentralSM(SecMasterRequestObj secMasterRequestObj)
        {
            CentralSMDataCache.Instance.AddAndModifySymbolRequestsForCentralSM(ref secMasterRequestObj);
            try
            {
                if (secMasterRequestObj.Count > 0)
                {
                    InstanceContext context = new InstanceContext(this);
                    CentralSMService.CentralSMServiceClient centralSMServiceClient = new CentralSMService.CentralSMServiceClient(context, "WSDualHttpBinding_ICentralSMService");
                    await centralSMServiceClient.GetSecMasterDataCentralSMAsync(secMasterRequestObj, ServerIdentifier, null);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw pricingConnError;
                }
            }
        }

        /// <summary>
        ///  Get SecMaster Data as dictionary by symbols list and symbol code sync 
        /// </summary>
        /// <param name="symbolList"></param>
        /// <param name="symbologyCode"></param>
        /// <param name="senderCode"></param>
        /// <returns></returns>
        public Dictionary<String, SecMasterBaseObj> GetSecMasterDataDictForListSync(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode) //, int senderCode)
        {

            Dictionary<String, SecMasterBaseObj> requestedSMData = new Dictionary<string, SecMasterBaseObj>();
            try
            {
                List<SecMasterBaseObj> foundData = GetSecMasterDataForListSync(symbolList, symbologyCode, 0);
                foreach (SecMasterBaseObj secMasterBaseObj in foundData)
                {
                    if (!requestedSMData.ContainsKey(secMasterBaseObj.TickerSymbol))
                    {
                        requestedSMData.Add(secMasterBaseObj.TickerSymbol, secMasterBaseObj);
                    }

                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return requestedSMData;
        }

        /// <summary>
        /// Get secMaster data by symbol list 
        /// </summary>
        /// <param name="symbolList"></param>
        /// <param name="symbologyCode"></param>
        /// <param name="senderCode"></param>
        public void GetSecMasterData(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode, int senderCode)
        {
            try
            {
                SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                foreach (string symbol in symbolList)
                {
                    secMasterRequestObj.AddData(symbol, symbologyCode);
                }
                GetSecMasterData(secMasterRequestObj, senderCode);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SetSecuritymasterDetails(PranaMessage pranaMsg)
        {
            try
            {
                //Kuldeep A.: In case of rejecting orders from the Pending Replace or Pending Cancel State, Symbol information (Tag 55) was not there,
                //So applying check here.
                if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                {
                    string symbol = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value;
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                    secMasterRequestObj.AddData(symbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                    secMasterRequestObj.HashCode = _hashCode;
                    List<SecMasterBaseObj> foundsymbolsData = GetSecMasterDataForListSync(secMasterRequestObj, _hashCode);
                    if (foundsymbolsData.Count > 0)
                    {
                        SecMasterBaseObj secMasterObj = foundsymbolsData[0];
                        pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagContractMultiplier, secMasterObj.Multiplier.ToString());
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CompanyName, secMasterObj.LongName);
                        pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagUnderlyingSymbol, secMasterObj.UnderLyingSymbol);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OSIOptionSymbol, secMasterObj.OSIOptionSymbol);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_IDCOOptionSymbol, secMasterObj.IDCOOptionSymbol);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OPRAOptionSymbol, secMasterObj.OpraSymbol);

                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_TickerSymbol, secMasterObj.TickerSymbol);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ReutersSymbol, secMasterObj.RequestedSymbol);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ISINSymbol, secMasterObj.ISINSymbol);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_SEDOLSymbol, secMasterObj.SedolSymbol);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CusipSymbol, secMasterObj.CusipSymbol);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_BloombergSymbol, secMasterObj.BloombergSymbol);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_BloombergSymbolExCode, secMasterObj.BloombergSymbolWithExchangeCode);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Activ_Symbol, secMasterObj.ActivSymbol);
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_FactSet_Symbol, secMasterObj.FactSetSymbol);

                        //updated RoundLot field value from secMasterObject, PRANA-12674
                        pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_RoundLot, secMasterObj.RoundLot.ToString());

                        AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(secMasterObj.AssetCategory);
                        switch (baseAssetCategory)
                        {
                            case AssetCategory.Equity:
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Delta, ((SecMasterEquityObj)secMasterObj).Delta.ToString());
                                break;
                            case AssetCategory.Option:
                                pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagPutOrCall, ((SecMasterOptObj)secMasterObj).PutOrCall.ToString());
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Delta, ((SecMasterOptObj)secMasterObj).Delta.ToString());
                                break;
                            case AssetCategory.Future:

                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_IsCutOffTimeUsed, SecMasterDataCache.GetInstance.SecMasterPreferences.UseCutOffTime.ToString());
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CutOffTime, ((SecMasterFutObj)secMasterObj).CutOffTime);
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Delta, ((SecMasterFutObj)secMasterObj).Delta.ToString());
                                if (secMasterObj.AssetCategory == AssetCategory.FXForward)
                                    pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_SettlementDate, ((SecMasterFXForwardObj)secMasterObj).ExpirationDate.ToString());
                                break;
                            case AssetCategory.FX:
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Delta, ((SecMasterFxObj)secMasterObj).Delta.ToString());
                                break;

                            case AssetCategory.FixedIncome:
                            case AssetCategory.ConvertibleBond:
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Delta, ((SecMasterFixedIncome)secMasterObj).Delta.ToString());
                                //TODO : Check for - Trade, drop copy trades.
                                if (!pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_DaysToSettlementFixedIncome))
                                {
                                    pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_DaysToSettlementFixedIncome, ((SecMasterFixedIncome)secMasterObj).DaysToSettlement.ToString());
                                }
                                pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMaturityDay, ((SecMasterFixedIncome)secMasterObj).MaturityDate.ToString());
                                pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagCouponRate, ((SecMasterFixedIncome)secMasterObj).Coupon.ToString());
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ACCRUAL_BASIS, ((SecMasterFixedIncome)secMasterObj).AccrualBasis.ToString());
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_DATE_ISSUE, ((SecMasterFixedIncome)secMasterObj).IssueDate.ToString());
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_FIRST_COUPON_DATE, ((SecMasterFixedIncome)secMasterObj).FirstCouponDate.ToString());
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_FREQUENCY, ((SecMasterFixedIncome)secMasterObj).Frequency.ToString());
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public SecMasterBaseObj GetSecMasterDataForSymbol(string symbol)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(symbol))
                {
                    SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                    secMasterRequestObj.AddData(symbol.Trim().ToUpper(), ApplicationConstants.SymbologyCodes.TickerSymbol);
                    secMasterRequestObj.HashCode = _hashCode;
                    List<SecMasterBaseObj> foundsymbolsData = GetSecMasterDataForListSync(secMasterRequestObj, _hashCode);
                    if (foundsymbolsData.Count > 0)
                    {
                        return foundsymbolsData[0];
                    }
                }
                else
                {
                    InformationReporter.GetInstance.Write("Symbol is blank in Fetching security info with sync, contact Admin! " + DateTime.UtcNow);
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
            return null;
        }

        public void SetSecuritymasterDetails(PranaBasicMessage taxLotDetail)
        {
            try
            {
                string symbol = taxLotDetail.Symbol;
                SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                secMasterRequestObj.AddData(symbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                secMasterRequestObj.HashCode = _hashCode;
                List<SecMasterBaseObj> foundsymbolsData = GetSecMasterDataForListSync(secMasterRequestObj, _hashCode);
                if (foundsymbolsData.Count > 0)
                {
                    SecMasterBaseObj secMasterObj = foundsymbolsData[0];
                    taxLotDetail.ContractMultiplier = secMasterObj.Multiplier;
                    taxLotDetail.CompanyName = secMasterObj.LongName;
                    taxLotDetail.UnderlyingSymbol = secMasterObj.UnderLyingSymbol;
                    taxLotDetail.RoundLot = secMasterObj.RoundLot;
                    taxLotDetail.SharesOutstanding = secMasterObj.SharesOutstanding;
                    //Added by:[RG 09042012 Details: Sensato UI Improvements]
                    taxLotDetail.BloombergSymbol = secMasterObj.BloombergSymbol;
                    taxLotDetail.BloombergSymbolWithExchangeCode = secMasterObj.BloombergSymbolWithExchangeCode;
                    taxLotDetail.SEDOLSymbol = secMasterObj.SedolSymbol;
                    taxLotDetail.CusipSymbol = secMasterObj.CusipSymbol;
                    taxLotDetail.ISINSymbol = secMasterObj.ISINSymbol;
                    taxLotDetail.OSISymbol = secMasterObj.OSIOptionSymbol;
                    taxLotDetail.IDCOSymbol = secMasterObj.IDCOOptionSymbol;
                    taxLotDetail.ProxySymbol = secMasterObj.ProxySymbol;
                    taxLotDetail.Delta = secMasterObj.Delta;
                    taxLotDetail.ReutersSymbol = secMasterObj.ReutersSymbol;
                    taxLotDetail.FactSetSymbol = secMasterObj.FactSetSymbol;
                    taxLotDetail.ActivSymbol = secMasterObj.ActivSymbol;
                    int currencyID = taxLotDetail.CurrencyID;
                    taxLotDetail.CurrencyID = secMasterObj.CurrencyID;
                    switch (secMasterObj.AssetCategory)
                    {

                        case AssetCategory.FXForward:
                            taxLotDetail.LeadCurrencyID = ((SecMasterFXForwardObj)secMasterObj).LeadCurrencyID;
                            taxLotDetail.VsCurrencyID = ((SecMasterFXForwardObj)secMasterObj).VsCurrencyID;
                            taxLotDetail.ExpirationDate = ((SecMasterFXForwardObj)secMasterObj).ExpirationDate;
                            taxLotDetail.IsNDF = ((SecMasterFXForwardObj)secMasterObj).IsNDF;
                            taxLotDetail.FixingDate = ((SecMasterFXForwardObj)secMasterObj).FixingDate;
                            //PRANA-32706
                            taxLotDetail.UnderlyingDelta = ((SecMasterFXForwardObj)secMasterObj).Delta;
                            if (currencyID == taxLotDetail.LeadCurrencyID || currencyID == taxLotDetail.VsCurrencyID)
                                taxLotDetail.CurrencyID = currencyID;
                            break;

                        default:
                            AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(secMasterObj.AssetCategory);
                            switch (baseAssetCategory)
                            {
                                case AssetCategory.Equity:
                                    taxLotDetail.UnderlyingDelta = ((SecMasterEquityObj)secMasterObj).Delta;
                                    break;
                                case AssetCategory.Option:
                                    taxLotDetail.PutOrCall = ((SecMasterOptObj)secMasterObj).PutOrCall;
                                    taxLotDetail.StrikePrice = ((SecMasterOptObj)secMasterObj).StrikePrice;
                                    taxLotDetail.ExpirationDate = ((SecMasterOptObj)secMasterObj).ExpirationDate;
                                    taxLotDetail.IsCurrencyFuture = ((SecMasterOptObj)secMasterObj).IsCurrencyFuture;
                                    SecMasterBaseObj underlyingdata = GetUnderlyingData(taxLotDetail.UnderlyingSymbol);
                                    if (underlyingdata != null)
                                    {
                                        //As Delta is available in SecMasterEquityObj, thus we are casting it onto SecMasterEquityObj.
                                        //Also, Underlying Delta is used for ETF's which are traded as equity. thus Future options donot have anything to do with underlying delta
                                        //Kuldeep A.: Put extra check for Future class as now underlying delta refers to leveraged factor and that should also update for Future Options from their underlying symbol.
                                        if (underlyingdata.AssetCategory.Equals(AssetCategory.Equity) && !underlyingdata.TickerSymbol.Contains("$"))
                                        {
                                            taxLotDetail.UnderlyingDelta = ((SecMasterEquityObj)underlyingdata).Delta;
                                        }
                                        if (underlyingdata.AssetCategory.Equals(AssetCategory.Future) && !underlyingdata.TickerSymbol.Contains("$"))
                                        {
                                            taxLotDetail.UnderlyingDelta = ((SecMasterFutObj)underlyingdata).Delta;
                                        }
                                    }
                                    break;
                                case AssetCategory.Future:
                                    taxLotDetail.ExpirationDate = ((SecMasterFutObj)secMasterObj).ExpirationDate;
                                    taxLotDetail.IsCurrencyFuture = ((SecMasterFutObj)secMasterObj).IsCurrencyFuture;
                                    //PRANA-32706
                                    taxLotDetail.UnderlyingDelta = ((SecMasterFutObj)secMasterObj).Delta;
                                    break;
                                case AssetCategory.FX:
                                    taxLotDetail.LeadCurrencyID = ((SecMasterFxObj)secMasterObj).LeadCurrencyID;
                                    taxLotDetail.VsCurrencyID = ((SecMasterFxObj)secMasterObj).VsCurrencyID;
                                    taxLotDetail.IsNDF = ((SecMasterFxObj)secMasterObj).IsNDF;
                                    taxLotDetail.FixingDate = ((SecMasterFxObj)secMasterObj).FixingDate;
                                    //PRANA-32706
                                    taxLotDetail.UnderlyingDelta = ((SecMasterFxObj)secMasterObj).Delta;
                                    if (currencyID == taxLotDetail.LeadCurrencyID || currencyID == taxLotDetail.VsCurrencyID)
                                        taxLotDetail.CurrencyID = currencyID;
                                    break;
                                //Kuldeep: Copying Maturity Date to Expiration Date as in case of Fixed Income both are same.
                                case AssetCategory.FixedIncome:
                                case AssetCategory.ConvertibleBond:
                                    taxLotDetail.ExpirationDate = ((SecMasterFixedIncome)secMasterObj).MaturityDate;
                                    taxLotDetail.MaturityDate = ((SecMasterFixedIncome)secMasterObj).MaturityDate;
                                    //PRANA-32732
                                    if (secMasterObj.AssetCategory.Equals(AssetCategory.FixedIncome))
                                        taxLotDetail.UnderlyingDelta = ((SecMasterFixedIncome)secMasterObj).Delta;
                                    SecMasterBaseObj underlyingDataForBonds = GetUnderlyingData(taxLotDetail.UnderlyingSymbol);
                                    if (underlyingDataForBonds != null)
                                    {
                                        //As Delta is available in SecMasterEquityObj, thus we are casting it onto SecMasterEquityObj.
                                        //Also, Underlying Delta is used for ETF's which are traded as equity. thus Future options donot have anything to do with underlying delta
                                        if (underlyingDataForBonds.AssetCategory.Equals(AssetCategory.Equity) && !underlyingDataForBonds.TickerSymbol.Contains("$"))
                                        {
                                            taxLotDetail.UnderlyingDelta = ((SecMasterEquityObj)underlyingDataForBonds).Delta;
                                        }
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        ///merge UDA data to taxlot
        /// </summary>
        /// <param name="taxlot"></param>
        public void SetSecurityUDADetails(TaxLot taxlot)
        {
            try
            {
                string symbol = taxlot.Symbol;
                SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                secMasterRequestObj.AddData(symbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                secMasterRequestObj.HashCode = _hashCode;
                List<SecMasterBaseObj> foundsymbolsData = GetSecMasterDataForListSync(secMasterRequestObj, _hashCode);
                Dictionary<string, DynamicUDA> _dynamicUDACache = GetDynamicUDAList();
                if (foundsymbolsData.Count > 0)
                {
                    SecMasterBaseObj secMasterObj = foundsymbolsData[0];
                    if (secMasterObj != null)
                    {
                        if (secMasterObj.SymbolUDAData != null)
                        {
                            UDAData udaData = secMasterObj.SymbolUDAData;
                            taxlot.UDAAsset = udaData.UDAAsset;
                            taxlot.CountryName = udaData.UDACountry;
                            taxlot.SectorName = udaData.UDASector;
                            taxlot.SecurityTypeName = udaData.UDASecurityType;
                            taxlot.SubSectorName = udaData.UDASubSector;
                        }
                        if (secMasterObj.DynamicUDA != null)
                        {
                            #region Dynamic-UDA
                            SerializableDictionary<String, Object> dynamicUDA = secMasterObj.DynamicUDA;
                            if (dynamicUDA != null)
                            {
                                if (dynamicUDA.ContainsKey("Analyst") && dynamicUDA["Analyst"] != null)
                                    taxlot.Analyst = dynamicUDA["Analyst"].ToString();
                                else if (_dynamicUDACache.ContainsKey("Analyst"))
                                    taxlot.Analyst = _dynamicUDACache["Analyst"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CountryOfRisk") && dynamicUDA["CountryOfRisk"] != null)
                                    taxlot.CountryOfRisk = dynamicUDA["CountryOfRisk"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CountryOfRisk"))
                                    taxlot.CountryOfRisk = _dynamicUDACache["CountryOfRisk"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA1") && dynamicUDA["CustomUDA1"] != null)
                                    taxlot.CustomUDA1 = dynamicUDA["CustomUDA1"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA1"))
                                    taxlot.CustomUDA1 = _dynamicUDACache["CustomUDA1"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA2") && dynamicUDA["CustomUDA2"] != null)
                                    taxlot.CustomUDA2 = dynamicUDA["CustomUDA2"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA2"))
                                    taxlot.CustomUDA2 = _dynamicUDACache["CustomUDA2"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA3") && dynamicUDA["CustomUDA3"] != null)
                                    taxlot.CustomUDA3 = dynamicUDA["CustomUDA3"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA3"))
                                    taxlot.CustomUDA3 = _dynamicUDACache["CustomUDA3"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA4") && dynamicUDA["CustomUDA4"] != null)
                                    taxlot.CustomUDA4 = dynamicUDA["CustomUDA4"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA4"))
                                    taxlot.CustomUDA4 = _dynamicUDACache["CustomUDA4"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA5") && dynamicUDA["CustomUDA5"] != null)
                                    taxlot.CustomUDA5 = dynamicUDA["CustomUDA5"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA5"))
                                    taxlot.CustomUDA5 = _dynamicUDACache["CustomUDA5"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA6") && dynamicUDA["CustomUDA6"] != null)
                                    taxlot.CustomUDA6 = dynamicUDA["CustomUDA6"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA6"))
                                    taxlot.CustomUDA6 = _dynamicUDACache["CustomUDA6"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA7") && dynamicUDA["CustomUDA7"] != null)
                                    taxlot.CustomUDA7 = dynamicUDA["CustomUDA7"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA7"))
                                    taxlot.CustomUDA7 = _dynamicUDACache["CustomUDA7"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA8") && dynamicUDA["CustomUDA8"] != null)
                                    taxlot.CustomUDA8 = dynamicUDA["CustomUDA8"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA8"))
                                    taxlot.CustomUDA8 = _dynamicUDACache["CustomUDA8"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA9") && dynamicUDA["CustomUDA9"] != null)
                                    taxlot.CustomUDA9 = dynamicUDA["CustomUDA9"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA9"))
                                    taxlot.CustomUDA9 = _dynamicUDACache["CustomUDA9"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA10") && dynamicUDA["CustomUDA10"] != null)
                                    taxlot.CustomUDA10 = dynamicUDA["CustomUDA10"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA10"))
                                    taxlot.CustomUDA10 = _dynamicUDACache["CustomUDA10"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA11") && dynamicUDA["CustomUDA11"] != null)
                                    taxlot.CustomUDA11 = dynamicUDA["CustomUDA11"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA11"))
                                    taxlot.CustomUDA11 = _dynamicUDACache["CustomUDA11"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("CustomUDA12") && dynamicUDA["CustomUDA12"] != null)
                                    taxlot.CustomUDA12 = dynamicUDA["CustomUDA12"].ToString();
                                else if (_dynamicUDACache.ContainsKey("CustomUDA12"))
                                    taxlot.CustomUDA12 = _dynamicUDACache["CustomUDA12"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("Issuer") && dynamicUDA["Issuer"] != null)
                                    taxlot.Issuer = dynamicUDA["Issuer"].ToString();
                                else if (_dynamicUDACache.ContainsKey("Issuer"))
                                    taxlot.Issuer = _dynamicUDACache["Issuer"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("LiquidTag") && dynamicUDA["LiquidTag"] != null)
                                    taxlot.LiquidTag = dynamicUDA["LiquidTag"].ToString();
                                else if (_dynamicUDACache.ContainsKey("LiquidTag"))
                                    taxlot.LiquidTag = _dynamicUDACache["LiquidTag"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("MarketCap") && dynamicUDA["MarketCap"] != null)
                                    taxlot.MarketCap = dynamicUDA["MarketCap"].ToString();
                                else if (_dynamicUDACache.ContainsKey("MarketCap"))
                                    taxlot.MarketCap = _dynamicUDACache["MarketCap"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("Region") && dynamicUDA["Region"] != null)
                                    taxlot.Region = dynamicUDA["Region"].ToString();
                                else if (_dynamicUDACache.ContainsKey("Region"))
                                    taxlot.Region = _dynamicUDACache["Region"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("RiskCurrency") && dynamicUDA["RiskCurrency"] != null)
                                    taxlot.RiskCurrency = dynamicUDA["RiskCurrency"].ToString();
                                else if (_dynamicUDACache.ContainsKey("RiskCurrency"))
                                    taxlot.RiskCurrency = _dynamicUDACache["RiskCurrency"].DefaultValue.ToString();

                                if (dynamicUDA.ContainsKey("UCITSEligibleTag") && dynamicUDA["UCITSEligibleTag"] != null)
                                    taxlot.UcitsEligibleTag = dynamicUDA["UCITSEligibleTag"].ToString();
                                else if (_dynamicUDACache.ContainsKey("UCITSEligibleTag"))
                                    taxlot.UcitsEligibleTag = _dynamicUDACache["UCITSEligibleTag"].DefaultValue.ToString();
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private SecMasterBaseObj GetUnderlyingData(string underlyingSymbol)
        {
            SecMasterBaseObj underlyingData = null;
            try
            {
                SecMasterRequestObj secMasterRequestObj = new SecMasterRequestObj();
                secMasterRequestObj.AddData(underlyingSymbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                secMasterRequestObj.HashCode = _hashCode;
                List<SecMasterBaseObj> foundsymbolsData = GetSecMasterDataForListSync(secMasterRequestObj, _hashCode);
                if (foundsymbolsData.Count > 0)
                {
                    underlyingData = foundsymbolsData[0];
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
            return underlyingData;
        }

        public async void GetFutureRootDataAsync()
        {
            try
            {
                InstanceContext context = new InstanceContext(this);
                CentralSMService.CentralSMServiceClient centralSMServiceClient = new CentralSMService.CentralSMServiceClient(context, "WSDualHttpBinding_ICentralSMService");
                await centralSMServiceClient.GetFutureRootDataAsync();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw pricingConnError;
                }
            }
        }

        public DataSet GetFutureRootData()
        {
            try
            {
                if (_pricingSource == PranaPricingSource.Bloomberg)
                {
                    GetFutureRootDataAsync();
                }
                else
                {
                    return SecMasterDataManager.GetFutureRootData(_connectionStr);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public FutureRootData GetFutureRootData(String symbol)
        {
            try
            {
                return SecMasterDataCache.GetInstance.GetFutSymbolRootdata(symbol);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public void SaveFutureRootDataLocal(DataTable dt, string userID, string requestID)
        {
            try
            {
                string result = String.Empty;
                //updating root data in future root cache and update in SM cache - omshiv,nov 2013
                SecMasterbaseList updatedFutureSymbols = SecMasterDataCache.GetInstance.UpdateFutureSymbolsFromRootSymbol(dt);
                if (updatedFutureSymbols.Count > 0)
                {
                    // send back updated symbols to clients for client cache
                    SecMasterServerComponent.GetInstance.SendDataToClient(updatedFutureSymbols);
                    PublishSecurityMasterData(updatedFutureSymbols);
                }
                result = SecMasterDataManager.SaveFutureRootData(dt);

                // send save response to client, PRANA-9815
                if (FutureRootDataSavedResponse != null)
                {
                    FutureRootDataSavedResponse(this, new EventArgs<string, string, string>(result, userID, requestID));
                }

                // send the status back to respective user And Module
                if (StatusOfRequest != null)
                {
                    StatusOfRequest(this, new EventArgs<string, string, string>(result, userID, requestID));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Save future root data Add/edit from root UI 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="userID"></param>
        /// <param name="requestID"></param>
        public async void SaveFutureRootData(DataTable dt, string userID, string requestID)
        {
            try
            {

                if (_pricingSource == PranaPricingSource.Bloomberg)
                {
                    try
                    {
                        InstanceContext context = new InstanceContext(this);
                        CentralSMService.CentralSMServiceClient centralSMServiceClient = new CentralSMService.CentralSMServiceClient(context, "WSDualHttpBinding_ICentralSMService");
                        await centralSMServiceClient.SaveFutureRootDataAsync(dt, null);
                    }
                    catch (Exception ex)
                    {
                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                        bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDTHROW);
                        if (rethrow)
                        {
                            throw pricingConnError;
                        }
                    }
                }
                else
                {
                    SaveFutureRootDataLocal(dt, userID, requestID);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public SecMasterSymbolSearchRes ReqSymbolSearch(SecMasterSymbolSearchReq request)
        {
            IList<string> searchResult = null;
            try
            {
                searchResult = _symbolCache.symbolSearch(request.StartWith, 200, request.Symbology);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return new SecMasterSymbolSearchRes(request.StartWith, request.Symbology, searchResult);
        }
        #endregion

        #region sending response to the client ..... one which generated the request

        #region SecMasterCacheMgr Invoke part
        private Dictionary<int, List<string>> _subscriberSnapShotHash;

        public Dictionary<int, List<string>> SubscriberSnapShotHash
        {
            get { return _subscriberSnapShotHash; }
            set { _subscriberSnapShotHash = value; }
        }

        void InvokeSecMstrDataResponse(SecMasterBaseObj secMasterObj)
        {
            try
            {
                if (SecMstrDataResponse != null)
                {
                    Delegate[] subscriberList = SecMstrDataResponse.GetInvocationList();
                    AsyncInvokeDelegate invoker = new AsyncInvokeDelegate(InvokeDelegate);
                    foreach (Delegate subscriber in subscriberList)
                    {
                        int subscriberHashCode = subscriber.Target.GetHashCode();
                        if (SecMasterDataCache.GetInstance.SubscriberSnapShotHash.ContainsKey(subscriberHashCode))
                        {
                            if (SecMasterDataCache.GetInstance.IsSnapShotRequested(secMasterObj, subscriberHashCode))
                            {
                                //Mukul: update SecMasterBaseObject with required Fields like BB Symbol as they are coming blank..
                                SecMasterDataCache.GetInstance.UpdateSecMasterResponse(secMasterObj, subscriberHashCode);
                                invoker.BeginInvoke(subscriber, new object[1] { new EventArgs<SecMasterBaseObj>(secMasterObj) }, null, null);
                                SecMasterDataCache.GetInstance.RemoveFromSnapShotSubscribers(secMasterObj, subscriberHashCode);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sink"></param>
        /// <param name="args"></param>
        private static void InvokeDelegate(Delegate sink, params object[] args)
        {
            try
            {

                sink.DynamicInvoke(sink, args[0]);

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        #endregion


        #endregion

        /// <summary>
        /// Cleaning  sec master cache
        /// </summary>
        public void Clean()
        {
            try
            {
                SecMasterDataCache.GetInstance.Clean();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region Live feed Data

        /// <summary>
        /// Handeling SM data recieved from Live feed
        /// modified by -om shiv, Nov 2013
        /// </summary>
        /// <param name="level1Data"></param>
        void PublishLevel1SnapshotResponse(SymbolData level1Data, bool saveSymbolInDb = true, string RequestedUserId = null, string RequestedHashcode = null)
        {
            try
            {
                if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: SecMasterCacheManager.PublishLevel1SnapshotResponse() entered for Symbol: {0}, Time: {1}", level1Data.Symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                }

                SecMasterBaseObj secMasterObj = SecMasterDataCache.GetInstance.GetSecMasterObj(level1Data);
                StringBuilder validationComments = new StringBuilder();

                // if asset is not supported it will return null
                if (secMasterObj != null)
                {
                    // fx data should not be saved one that is received from 
                    if (secMasterObj.AssetCategory == AssetCategory.FX)
                    {
                        return;
                    }

                    AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(secMasterObj.AssetCategory);

                    if (baseAssetCategory == AssetCategory.Option)
                    {
                        secMasterObj.RequestedSymbology = (int)level1Data.RequestedSymbology;

                        //Modifed: omshiv, Nov 2013,
                        // First it check underlying symbol available or not, then request for snapshot
                        if (!String.IsNullOrEmpty(secMasterObj.UnderLyingSymbol))
                        {
                            if (!GetSymbolLookupRequestedData(secMasterObj.UnderLyingSymbol, _connectionStr))
                            {
                                List<string> ListOfSymbol = new List<string>();
                                ListOfSymbol.Add(secMasterObj.UnderLyingSymbol);
                                _pricingServicesProxy.InnerChannel.RequestSMData(ListOfSymbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                            }
                        }
                        else
                        {
                            //if underlying symbol not available then set "Undefined", 
                            //so that user can search undefined underlying symbols and correct it.
                            secMasterObj.UnderLyingSymbol = ApplicationConstants.CONST_UNDEFINED;

                        }
                    }

                    secMasterObj.CurrencyID = CachedDataManager.GetInstance.GetCurrencyID(level1Data.CurencyCode);
                    secMasterObj.Symbol_PK = SecurityMasterSymbolIDGenerator.GenerateSymbolPKID();

                    if (secMasterObj.AUECID == int.MinValue || secMasterObj.AUECID == 0)
                    {
                        switch (level1Data.CategoryCode)
                        {
                            case AssetCategory.Equity:
                                secMasterObj.AUECID = SecMasterConstants.DefaultEquityAUECID;
                                break;
                            case AssetCategory.EquityOption:
                                secMasterObj.AUECID = SecMasterConstants.DefaultOptionAUECID;
                                break;

                            case AssetCategory.Future:
                                secMasterObj.AUECID = SecMasterConstants.DefaultFutureAUECID;
                                break;
                            case AssetCategory.FutureOption:
                                secMasterObj.AUECID = SecMasterConstants.DefaultFutureOptionAUECID;

                                break;
                            case AssetCategory.FXForward:
                                secMasterObj.AUECID = SecMasterConstants.DefaultForwardAUECID;
                                break;

                            case AssetCategory.FixedIncome:
                                secMasterObj.AUECID = SecMasterConstants.DefaultFixedIncomeAUECID;
                                break;
                        }
                        validationComments.Append(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString());
                        validationComments.Append(",");

                        secMasterObj.AssetID = CachedDataManager.GetInstance.GetAssetIdByAUECId(secMasterObj.AUECID);
                        secMasterObj.CurrencyID = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(secMasterObj.AUECID);
                        secMasterObj.ExchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(secMasterObj.AUECID);
                        secMasterObj.UnderLyingID = CachedDataManager.GetInstance.GetUnderlyingID(secMasterObj.AUECID);
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(SecMasterConstants.MSG_AUECNotFoundLiveFeed + secMasterObj.ToString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }
                    //Set roundlot value, PRANA-12686
                    secMasterObj.RoundLot = CachedDataManager.GetInstance.GetRoundLotByAUECID(secMasterObj.AUECID);

                    if (secMasterObj.CurrencyID == int.MinValue || secMasterObj.CurrencyID == 0)
                    {
                        secMasterObj.CurrencyID = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(secMasterObj.AUECID);
                        validationComments.Append(SecMasterConstants.SecMasterComments.DefaultCurrency.ToString());
                        LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(SecMasterConstants.MSG_CurrencyNotFoundLiveFeed + secMasterObj.ToString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                    }

                    if (SecMasterEnRichData.GetInstance.CheckSMEnRichRequires())
                    {
                        SecMasterEnRichData.GetInstance.EnRichSecMasterObject(secMasterObj);
                        SecMasterEnRichData.GetInstance.DeleteSMEnRichCachedData(secMasterObj.TickerSymbol);
                    }

                    switch (level1Data.MarketDataProvider)
                    {
                        case MarketDataProvider.Esignal:
                            secMasterObj.SourceOfDataID = (int)SecMasterConstants.SecMasterSourceOfData.ESignal;
                            secMasterObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.ESignal;
                            break;
                        case MarketDataProvider.FactSet:
                            secMasterObj.SourceOfDataID = (int)SecMasterConstants.SecMasterSourceOfData.FactSet;
                            secMasterObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.FactSet;
                            break;
                        case MarketDataProvider.ACTIV:
                            secMasterObj.SourceOfDataID = (int)SecMasterConstants.SecMasterSourceOfData.ACTIV;
                            secMasterObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.ACTIV;
                            break;
                        case MarketDataProvider.SAPI:
                            secMasterObj.SourceOfDataID = (int)SecMasterConstants.SecMasterSourceOfData.SAPI;
                            secMasterObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.SAPI;
                            break;
                        default:
                            secMasterObj.SourceOfDataID = (int)SecMasterConstants.SecMasterSourceOfData.Internal;
                            secMasterObj.SourceOfData = SecMasterConstants.SecMasterSourceOfData.Internal;
                            break;
                    }

                    UpdateSecMasterObject(secMasterObj, true);
                    SecMasterDataCache.GetInstance.UpdateUDADataWithName(secMasterObj);

                    //Update UDA asset as same as its asset class
                    if (secMasterObj.SymbolUDAData != null)
                    {
                        secMasterObj.UseUDAFromUnderlyingOrRoot = false;
                    }

                    // Update issuer in cache based on underlying Symbol
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-9763
                    if (secMasterObj.TickerSymbol.ToUpper().Equals(secMasterObj.UnderLyingSymbol.ToUpper()))
                    {
                        SecMasterDataCache.GetInstance.UpdateIssuerInSMCacheOfUnderlying(secMasterObj);
                        // update issuer in local cache on client, PRANA-9838
                        SecMasterServerComponent.GetInstance.SecMasterUpdateClientCache(secMasterObj);
                    }

                    //set comments if defaut auec
                    secMasterObj.Comments = validationComments.ToString();

                    //Proccess to Save and publish Security
                    SaveNewSymbolToSecurityMaster(secMasterObj, saveSymbolInDb, RequestedUserId, RequestedHashcode);

                    #region Compliance Section
                    try
                    {
                        if (secMasterObj != null && SecurityObjectReceived != null)
                        {
                            if (secMasterObj.AssetCategory == AssetCategory.EquityOption)
                            {
                                secMasterObj.UseUDAFromUnderlyingOrRoot = true;
                                GetUDASymbolDataOfUnderlying(secMasterObj, true);
                                SecMasterbaseList secMasterBaseList = new SecMasterbaseList
                                {
                                    secMasterObj
                                };
                                SaveNewSymbolToSecurityMaster(secMasterBaseList);
                                SecurityObjectReceived(this, new EventArgs<SecMasterBaseObj>(secMasterObj));
                            }
                            else
                            {
                                SecurityObjectReceived(this, new EventArgs<SecMasterBaseObj>(secMasterObj));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                        if (rethrow)
                            throw;
                    }
                    #endregion
                }

                if (CachedDataManager.GetInstance.IsSecurityValidationLoggingEnabled)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.Write(string.Format("SecurityValidationLogging: SecMasterCacheManager.PublishLevel1SnapshotResponse() exited for Symbol: {0}, Time: {1}", level1Data.Symbol, DateTime.UtcNow.ToString("MM-dd-yyyy hh:mm:ss:fff tt")), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Verbose, true);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update Sec master object for e-signal security and SM import security from thier underlying and set prefernces.
        /// </summary>
        /// <param name="secMasterObj"></param>
        private void UpdateSecMasterObject(SecMasterBaseObj secMasterObj, bool isLevel1Snapshot = false)
        {
            try
            {
                //Set Preference of Auto approve security
                SecMasterPreferenceManager.GetInstance.SetPreferences(secMasterObj);

                AssetCategory assetCategory = (AssetCategory)secMasterObj.AssetID;

                if (assetCategory == AssetCategory.Future || assetCategory == AssetCategory.FutureOption)
                {
                    // merge some secMaster details and UDA for futures
                    SecMasterDataCache.GetInstance.SetExtraFromCache(secMasterObj);
                }
                else if (assetCategory == AssetCategory.EquityOption && secMasterObj.Multiplier == 0)
                {
                    secMasterObj.Multiplier = CachedDataManager.GetInstance.GetMultiplierByAUECID(secMasterObj.AUECID);

                }

                //here Security comes from import SM or from E-signal, then merge UDA from their underlying.

                if ((secMasterObj.SymbolUDAData == null)
                    ||
                    (
                    secMasterObj.SymbolUDAData.AssetID == int.MinValue
                    && secMasterObj.SymbolUDAData.SectorID == int.MinValue
                    && secMasterObj.SymbolUDAData.SubSectorID == int.MinValue
                    && secMasterObj.SymbolUDAData.CountryID == int.MinValue
                    && secMasterObj.SymbolUDAData.SecurityTypeID == int.MinValue
                    )
                )
                {
                    //in case if UDA not found, then initialize with default values
                    secMasterObj.SymbolUDAData = new UDAData();
                    secMasterObj.SymbolUDAData.Symbol = secMasterObj.TickerSymbol;

                    //get uda from underlyig symbol if ticker is not same as underlying symbol
                    //om shiv, March 2014
                    //Added the check for empty string as it was requesting empty tickers from LiveFeed
                    if (!secMasterObj.TickerSymbol.Equals(secMasterObj.UnderLyingSymbol) && !String.IsNullOrWhiteSpace(secMasterObj.UnderLyingSymbol))
                    {
                        secMasterObj.UseUDAFromUnderlyingOrRoot = true;
                        GetUDASymbolDataOfUnderlying(secMasterObj);
                    }
                }

                // Update UDA User Asset automatically to Asset Class if it is undefined
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7970
                if (secMasterObj.SymbolUDAData != null && secMasterObj.SymbolUDAData.AssetID == int.MinValue)
                {
                    if (assetCategory == AssetCategory.EquityOption || assetCategory == AssetCategory.FutureOption)
                    {
                        int putOrCall = (secMasterObj as SecMasterOptObj).PutOrCall;
                        secMasterObj.SymbolUDAData.AssetID = UDADataCache.GetInstance.GetUDAAssetFromParameters(secMasterObj.AssetCategory.ToString(), putOrCall);
                    }
                    else
                    {
                        secMasterObj.SymbolUDAData.AssetID = UDADataCache.GetInstance.GetUDAIDFromText(secMasterObj.AssetCategory.ToString(), SecMasterConstants.CONST_UDAAsset);
                    }
                }
                if (isLevel1Snapshot)
                {
                    // Update Risk Currency automatically
                    if (secMasterObj.DynamicUDA.ContainsKey("RiskCurrency"))
                        secMasterObj.DynamicUDA["RiskCurrency"] = SecMasterDataManager.GetRiskCurrency(secMasterObj);
                    else
                        secMasterObj.DynamicUDA.Add("RiskCurrency", SecMasterDataManager.GetRiskCurrency(secMasterObj));

                    //Update Issuer
                    if (secMasterObj.DynamicUDA.ContainsKey("Issuer"))
                        secMasterObj.DynamicUDA["Issuer"] = GetIssuerForSymbol(secMasterObj);
                    else
                        secMasterObj.DynamicUDA.Add("Issuer", GetIssuerForSymbol(secMasterObj));
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

        /// <summary>
        /// gets value of issuer from ticker or underlying
        /// </summary>
        /// <param name="secMasterObj">secMasterObj</param>
        /// <returns>issuer value</returns>
        private string GetIssuerForSymbol(SecMasterBaseObj secMasterObj)
        {
            string issuer = string.Empty;
            try
            {
                if (!secMasterObj.TickerSymbol.Equals(secMasterObj.UnderLyingSymbol) && !String.IsNullOrWhiteSpace(secMasterObj.UnderLyingSymbol))
                {
                    // set default issuer for FX and FX Forward symbols same as Underlying symbol, PRANA-10830
                    if (secMasterObj.AssetCategory.Equals(AssetCategory.FXForward) || secMasterObj.AssetCategory.Equals(AssetCategory.FX))
                    {
                        issuer = secMasterObj.UnderLyingSymbol;
                    }
                    else
                    {
                        if (secMasterObj.AssetCategory.Equals(AssetCategory.EquityOption) && secMasterObj.SourceOfData == SecMasterConstants.SecMasterSourceOfData.Internal)
                            return secMasterObj.LongName;
                        List<string> underlyingSymbolList = new List<string>();
                        underlyingSymbolList.Add(secMasterObj.UnderLyingSymbol.Trim());

                        //get sm data for underlying symbols
                        Dictionary<String, SecMasterBaseObj> foundsymbolsDataDict =
                            GetSecMasterDataDictForListSync(underlyingSymbolList, ApplicationConstants.SymbologyCodes.TickerSymbol);
                        if (foundsymbolsDataDict.Count > 0)
                        {
                            String undelyingSymbol = secMasterObj.UnderLyingSymbol.Trim();
                            //Merge UDA from Underlying security 
                            if (foundsymbolsDataDict.ContainsKey(undelyingSymbol))
                            {
                                issuer = foundsymbolsDataDict[undelyingSymbol].LongName;
                            }
                        }
                    }
                }
                else if (secMasterObj.TickerSymbol.Equals(secMasterObj.UnderLyingSymbol))
                    issuer = secMasterObj.LongName;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return issuer;
        }
        #endregion

        #region ILiveFeedCallback Members
        object _SnapShotlocker = new object();
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            try
            {
                lock (_SnapShotlocker)
                    PublishLevel1SnapshotResponse(data, snapshotResponseData.ShouldSaveInDb);
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

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
        }

        public void LiveFeedDisConnected()
        {
        }
        #endregion

        public string GetOldCompanyNameForNameChange(Guid CAId)
        {
            return SecMasterDataManager.GetOldCompanyNameForNameChange(CAId);
        }

        public SecMasterGlobalPreferences GetSMPreferences()
        {
            SecMasterGlobalPreferences preferences = null;

            try
            {
                if (SecMasterDataCache.GetInstance.SecMasterPreferences != null)
                {
                    preferences = SecMasterDataCache.GetInstance.SecMasterPreferences;
                }
                else
                {
                    preferences = SecMasterDataManager.GetPreferencesFromDB();
                    SecMasterDataCache.GetInstance.SetPreferences(preferences);
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

            return preferences;
        }

        public void SaveSMPreferences(SecMasterGlobalPreferences preferences)
        {
            try
            {
                SecMasterDataManager.SavePreferencesintoDB(preferences);
                SecMasterDataCache.GetInstance.SetPreferences(preferences);
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
        /// Get UDA attributes on client request for first time.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<int, string>> GetUDAAttributes()
        {
            Dictionary<string, Dictionary<int, string>> UDAsDict = new Dictionary<string, Dictionary<int, string>>();
            try
            {

                Dictionary<int, string> dictUDAAssets = UDADataCache.GetInstance.GetAllUDAAssets();
                Dictionary<int, string> dictUDASectors = UDADataCache.GetInstance.GetAllUDASectors();
                Dictionary<int, string> dictUDASubSectors = UDADataCache.GetInstance.GetAllUDASubSectors();
                Dictionary<int, string> dictUDASecurityTypes = UDADataCache.GetInstance.GetAllUDASecurityTypes();
                Dictionary<int, string> dictUDACountries = UDADataCache.GetInstance.GetAllUDACountries();

                if (dictUDAAssets != null && dictUDAAssets.Count > 0)
                    UDAsDict.Add(SecMasterConstants.CONST_UDAAsset, dictUDAAssets);

                if (dictUDACountries != null && dictUDACountries.Count > 0)
                    UDAsDict.Add(SecMasterConstants.CONST_UDACountry, dictUDACountries);

                if (dictUDASectors != null && dictUDASectors.Count > 0)
                    UDAsDict.Add(SecMasterConstants.CONST_UDASector, dictUDASectors);

                if (dictUDASubSectors != null && dictUDASubSectors.Count > 0)
                    UDAsDict.Add(SecMasterConstants.CONST_UDASubSector, dictUDASubSectors);

                if (dictUDASecurityTypes != null && dictUDASecurityTypes.Count > 0)
                    UDAsDict.Add(SecMasterConstants.CONST_UDASecurityType, dictUDASecurityTypes);


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
            return UDAsDict;
        }

        /// <summary>
        /// Get Historical traded symbol or currect symbols SM data 
        /// create by: omshiv, Nov 2013
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RequestID"></param>
        /// <param name="isOpenSymbolsReq"></param>
        public void GetAllHistOrOpenTradedSymbols(string UserID, string RequestID, Boolean isOpenSymbolsReq)
        {
            try
            {
                //get hist/current symbol req obj on symbol list
                SecMasterRequestObj secMasterReqObj = SecMasterDataManager.GetHistOrOpenTradedSymbols(isOpenSymbolsReq);

                //if requested data size is more than allowed data size the prompt to user by below msg
                if (!isOpenSymbolsReq && secMasterReqObj.Count > _maxSMReqSize)
                {
                    if (StatusOfRequest != null)
                    {

                        StatusOfRequest(this, new EventArgs<string, string, string>("The results for your search exceed the maximum number of records allowed for display. Please refine your search.", UserID, RequestID));
                    }
                    return;
                }

                // Gettign data from cache/DB and then send to user in chunks of 50, Hardcode size
                List<SecMasterBaseObj> foundData = GetSecMasterDataForListSync(secMasterReqObj, 0);
                if (foundData.Count > 0)
                {
                    int chunkSize = 50;
                    List<List<SecMasterBaseObj>> dataInChunks = ChunkingManager.CreateChunks(foundData, chunkSize);

                    foreach (List<SecMasterBaseObj> data in dataInChunks)
                    {
                        if (EventSendDataByResKey != null)
                        {
                            EventSendDataByResKey(this, new EventArgs<string, object, string, string>(SecMasterConstants.CONST_TradedSMDataUIRes, data, UserID, RequestID));
                        }

                    }
                    if (StatusOfRequest != null)
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>("Success", UserID, RequestID));
                    }
                }
                else
                {
                    if (StatusOfRequest != null)
                    {

                        StatusOfRequest(this, new EventArgs<string, string, string>("No Record Found.", UserID, RequestID));
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Bharat Jangir (22 September, 2014)
        /// Saving AUEC Mappings for Option & Portfolio Science symbols auto generation
        /// </summary>
        /// <param name="saveDataSetTemp"></param>
        public void SaveAUECMappings(DataSet saveDataSetTemp)
        {
            int rowsAffected = 0;
            try
            {
                rowsAffected = SecMasterDataManager.SaveAUECMappings(saveDataSetTemp);
                if (rowsAffected > 0)
                {
                    //Update PS Symbol Cache
                    PSSymbolGenerator.UpdateDictPSSymbolMapper(saveDataSetTemp);

                    //Update MarketData Symbol Cache
                    MarketDataSymbolGenerator.UpdateDictMarketDataSymbolMapper(saveDataSetTemp);

                    //Update Ticker Symbol Cache
                    TickerSymbolGenerator.UpdateDictMarketDataSymbolMapper(saveDataSetTemp, CachedDataManager.CompanyMarketDataProvider);
                    TickerSymbolGenerator.UpdateDictMarketDataSymbolMapperExchangeIdentifier(saveDataSetTemp);
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

        /// <summary>
        /// Bharat Jangir (22 September, 2014)
        /// Get AUEC Mappings for Option & Portfolio Science symbols auto generation
        /// </summary>
        /// <returns></returns>
        public DataSet GetAUECMappings()
        {
            DataSet dsAUECMapping = new DataSet();
            try
            {
                dsAUECMapping = SecMasterDataManager.GetAUECMappings();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dsAUECMapping;
        }

        /// <summary>
        /// Save UDA attributes based on pricing source Type, if bloomberg then save to CSM also else on local DB only
        /// created by: omshiv, nov 2014
        /// </summary>
        /// <param name="udaDataASBinaryString">udaDataASBinaryString</param>
        public async void SaveUDAAttributesData(String udaDataASBinaryString)
        {
            try
            {
                Dictionary<String, Dictionary<String, object>> udaDataCol = _binaryFormatter.DeSerialize(udaDataASBinaryString) as Dictionary<String, Dictionary<String, object>>;
                if (_pricingSource == PranaPricingSource.Bloomberg)
                {

                    try
                    {
                        InstanceContext context = new InstanceContext(this);
                        CentralSMService.CentralSMServiceClient centralSMServiceClient = new CentralSMService.CentralSMServiceClient(context, "WSDualHttpBinding_ICentralSMService");
                        await centralSMServiceClient.GenericeSendDataToCSMAsync(CustomFIXConstants.MSG_SECMASTER_UDA_Save, udaDataASBinaryString, ServerIdentifier, null);
                    }
                    catch (Exception ex)
                    {
                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                        bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDTHROW);
                        if (rethrow)
                        {
                            throw pricingConnError;
                        }
                    }
                }
                else
                {
                    SaveUDAAttributesData(udaDataCol);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Save UDA attributes in DB and update UDA attributes cache
        /// created by: omshiv, nov 2013
        /// </summary>
        /// <param name="udaDataCol"></param>
        public void SaveUDAAttributesData(Dictionary<String, Dictionary<String, object>> udaDataCol)
        {
            try
            {
                foreach (KeyValuePair<String, Dictionary<String, object>> udaTypeItem in udaDataCol)
                {
                    foreach (KeyValuePair<String, object> udaItem in udaTypeItem.Value)
                    {
                        if (udaItem.Key.Contains("Delete"))
                        {
                            List<int> udaIdsToDelete = udaItem.Value as List<int>;
                            if (udaIdsToDelete != null)
                            {
                                UDADataCache.GetInstance.RemoveDeletedUDAFrmCache(udaTypeItem.Key, udaIdsToDelete);
                                UDADataManager.DeleteInformation(udaItem.Key, udaIdsToDelete);

                            }
                        }
                        else if (udaItem.Key.Contains("Insert"))
                        {
                            UDACollection udaColToInsert = udaItem.Value as UDACollection;
                            if (udaColToInsert != null)
                            {
                                UDADataCache.GetInstance.AddUDAinCache(udaTypeItem.Key, udaColToInsert);
                                UDADataManager.SaveInformation(udaItem.Key, udaColToInsert);

                            }

                        }

                    }


                }
                SendUDAAttributesToClient();
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
        /// Send All UDA attibutes to all clients after updating UDA
        /// </summary>
        private void SendUDAAttributesToClient()
        {
            try
            {
                Dictionary<String, Dictionary<int, string>> UDAsDict = GetUDAAttributes();

                if (EventSendDataByResKey != null)
                {
                    EventSendDataByResKey(this, new EventArgs<string, object, string, string>(SecMasterConstants.Const_UDARes, UDAsDict, "", ""));

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

        /// <summary>
        /// Get Advanced search filetr data from DB
        /// </summary>
        /// <param name="SearchQuery"></param>
        /// <param name="userID"></param>
        /// <param name="requestID"></param>
        public void GetSMUIAdvncdSearchData(string SearchQuery, string userID, String requestID)
        {
            try
            {
                if (!String.IsNullOrEmpty(SearchQuery))
                {

                    BackgroundWorker bkWrkAdvncdSearchfrmDB = new BackgroundWorker();
                    bkWrkAdvncdSearchfrmDB.DoWork += new DoWorkEventHandler(bkWrkAdvncdSearchfrmDB_DoWork);
                    bkWrkAdvncdSearchfrmDB.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkWrkAdvncdSearchfrmDB_RunWorkerCompleted);
                    ArrayList arguments = new System.Collections.ArrayList();
                    arguments.Add(SearchQuery);
                    arguments.Add(userID);
                    arguments.Add(requestID);

                    bkWrkAdvncdSearchfrmDB.RunWorkerAsync(arguments);

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

        /// <summary>
        ///  handle on get Advanced search filter data completed, send to user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bkWrkAdvncdSearchfrmDB_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }
            object[] result = new object[3];
            result = (object[])e.Result;
            DataSet _ds = (DataSet)result[0];
            String userID = result[1].ToString();
            String requestID = result[2].ToString();

            try
            {

                if (!e.Cancelled) // no error
                {
                    if (EventSendDataByResKey != null)
                    {
                        EventSendDataByResKey(this, new EventArgs<string, object, string, string>(CustomFIXConstants.MSG_SECMASTER_SymbolRESPONSE, _ds, userID, requestID));
                    }

                    if (StatusOfRequest != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>("Success", userID, requestID));
                    }
                    else
                    {
                        StatusOfRequest(this, new EventArgs<string, string, string>("No Data found", userID, requestID));
                    }
                }
                else
                {


                    if (StatusOfRequest != null)
                    {

                        StatusOfRequest(this, new EventArgs<string, string, string>("No Data found or Problem on Server", userID, requestID));
                    }

                }

            }
            catch (Exception ex)
            {
                if (StatusOfRequest != null)
                {
                    StatusOfRequest(this, new EventArgs<string, string, string>(ex.Message, userID, requestID));
                }
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gettig Advanced search filter data from DB on backgroud thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bkWrkAdvncdSearchfrmDB_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet _ds = new DataSet();
            System.Collections.ArrayList arguments = (System.Collections.ArrayList)e.Argument;
            string queuryString = arguments[0] as string;
            string userID = arguments[1] as string;
            string requestID = arguments[2] as string;
            object[] result = new object[3];
            try
            {

                if (!string.IsNullOrEmpty(queuryString))
                {
                    String[] strArray = queuryString.Split(Seperators.SEPERATOR_5);
                    if (strArray.Length > 2)
                    {
                        String query = strArray[0];
                        int startIndex = int.Parse(strArray[1]);
                        int endIndex = int.Parse(strArray[2]);
                        _ds = SecMasterDataManager.GetAdvncdSearchDatafrmDB(query, startIndex, endIndex);

                    }
                    else
                    {
                        _ds = SecMasterDataManager.GetAdvncdSearchDatafrmDB(queuryString, 1, 50);
                    }



                    result[0] = _ds;
                    result[1] = userID;
                    result[2] = requestID;


                    e.Result = result;
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                e.Result = ex.Message;
                if (StatusOfRequest != null)
                {
                    StatusOfRequest(this, new EventArgs<string, string, string>("Problem on Server", userID, requestID));
                }
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }

            }
        }

        /// <summary>
        /// Gets the values for the fields requested from bloomberg for specific dates in the form of a datatable
        /// </summary>
        /// <param name="requestID">Generally Guid.NewGuid().ToString()</param>
        /// <param name="fields"></param>
        /// <param name="secMasterReqObj"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public async void GetGenericSMPrice(string requestID, String secondaryPricingSource, ConcurrentBag<string> fields, SecMasterRequestObj secMasterReqObj, DateTime startDate, DateTime endDate, Action<string, DataTable, bool, string> functionForResponse, bool isGetDataFromCacheOrDB)
        {
            try
            {
                SecMasterPricingReqManagerServer.Instance.CreateInitialMappingsForPricingRequestAndProcess(requestID, secondaryPricingSource, fields, secMasterReqObj, startDate, endDate, functionForResponse, isGetDataFromCacheOrDB);
                if (SecMasterPricingReqManagerServer.Instance.PricingRequestInProcessSecmasterCacheManager.ContainsKey(requestID))
                {
                    if (SecMasterPricingReqManagerServer.Instance.PricingRequestInProcessSecmasterCacheManager[requestID].IsFull)
                    {
                        SendHistoricalGenericDataResponseForDataFoundLocally(requestID);
                    }
                    else
                    {
                        if (_pricingSource == PranaPricingSource.Bloomberg)
                        {
                            InstanceContext context = new InstanceContext(this);
                            CentralSMService.CentralSMServiceClient centralSMServiceClient = new CentralSMService.CentralSMServiceClient(context, "WSDualHttpBinding_ICentralSMService");

                            SecMasterPricingReqManagerServer.Instance.GenerateAndSendCentralSMCalls(requestID);
                            foreach (KeyValuePair<string, PricingRequestMappings> bbKvp in SecMasterPricingReqManagerServer.Instance.PricingRequestInProcessSecmasterCacheManager[requestID].BBRequestIds)
                            {
                                try
                                {
                                    if (LoggingConstants.LoggingEnabled)
                                    {
                                        Logger.LoggerWrite("Historical pricing generic request to be sent to central SM. Fields: " + String.Join(",", bbKvp.Value.FieldNames) + Environment.NewLine + "StartDate : " + bbKvp.Value.StartDate.ToString() + " EndDate : " + bbKvp.Value.EndDate.ToString() + Environment.NewLine
                                            + " Requested Symbols : " + String.Join(",", bbKvp.Value.RequestObj.GetPrimarySymbols()), LoggingConstants.CATEGORY_FLAT_FILE_ClientMessages);
                                    }
                                    //await centralSMServiceClient.GetSecMasterDataCentralSMAsync(new SecMasterRequestObj(), null);
                                    await centralSMServiceClient.GetGenericSMPriceAsync(bbKvp.Key, bbKvp.Value.SecondaryPricingSource, bbKvp.Value.FieldNames.ToList(), bbKvp.Value.RequestObj, bbKvp.Value.StartDate, bbKvp.Value.EndDate, ServerIdentifier, null, isGetDataFromCacheOrDB);
                                }
                                catch (Exception ex)
                                {
                                    // Invoke our policy that is responsible for making sure no secure information
                                    // gets out of our layer.
                                    Exception pricingConnError = new Exception("Could not connect to CentralSMServer", ex);
                                    bool rethrow = Logger.HandleException(pricingConnError, LoggingConstants.POLICY_LOGANDSHOW);
                                    if (rethrow)
                                    {
                                        throw pricingConnError;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SendHistoricalGenericDataResponseForDataFoundLocally(string requestIDReturned)
        {
            try
            {
                PricingRequestMappings tempMapping;
                List<KeyValuePair<string, PricingRequestMappings>> requests = SecMasterPricingReqManagerServer.Instance.PricingRequestInProcessSecmasterCacheManager.Where(x => x.Value.RequestID.Equals(requestIDReturned)).ToList();
                IEnumerable<KeyValuePair<string, PricingRequestMappings>> requestsBBIds = SecMasterPricingReqManagerServer.Instance.PricingRequestInProcessSecmasterCacheManager.Where(x => x.Value.BBRequestIds.ContainsKey(requestIDReturned) || x.Value.BBRequestIdsInProcess.ContainsKey(requestIDReturned));
                requests.AddRange(requestsBBIds);
                if (requests.Count() == 0)
                    return;
                foreach (KeyValuePair<string, PricingRequestMappings> reqKvp in requests)
                {
                    if (reqKvp.Value.BBRequestIds.Count == 0 && reqKvp.Value.BBRequestIdsInProcess.Count == 0)
                    {
                        reqKvp.Value.ResponseFunctionDelegate(reqKvp.Value.RequestID, reqKvp.Value.ResponseTable, true, "Pricing Data received from local database");
                        SecMasterPricingReqManagerServer.Instance.PricingRequestInProcessSecmasterCacheManager.TryRemove(reqKvp.Value.RequestID, out tempMapping);
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
        }

        public void CentralSMDisconnected()
        {
            try
            {
                CentralSMDataCache.Instance.CleanCacheForSymbolsRequestedToBB();
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
        /// returns pricing field dictionary
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, StructPricingField> GetPricingFields()
        {
            try
            {
                return SecMasterCommonCache.Instance.PricingField;
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

        #region ICentralSMServiceCallback Members

        public void SecurityValidationResp(SecMasterBaseObj securityData)
        {
            try
            {
                bool sendData = CentralSMDataCache.Instance.RemoveAndCheckRequestedSymbolForCentralSM(securityData);
                if (sendData || securityData.SymbolType == (int)SymbolType.Updated)
                    SaveNewSymbolToSecurityMaster(securityData);
                //Added By Faisal Shah Dated 10/07/14
                //Need was to show a message on Trade Server if we process a newly Verified Symbol from API with Default AUECID.
                if (securityData.Comments.IndexOf(SecMasterConstants.SecMasterComments.DefaultAUECID.ToString()) > -1 && securityData.SymbolType == (int)SymbolType.New)
                {
                    LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(SecMasterConstants.MSG_AUECNotFoundLiveFeed + securityData.ToString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        public void GenricPricingResp(string requestID, System.Data.DataTable pricingTable, bool pricingSuccess, string comment)
        {
            try
            {
                SecMasterPricingReqManagerServer.Instance.MergeResponseFromCentralSMWithRequestsAndCacheAndMoveForward(requestID, pricingTable, pricingSuccess, comment);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SymbolLookUpSearchDataResp(DataSet SymbolsData, SymbolLookupRequestObject symbolLookupRequestObject)
        {
            try
            {
                if (StatusOfRequest != null)
                {
                    SymbolLkUpDataResponse(this, new EventArgs<DataSet, SymbolLookupRequestObject>(SymbolsData, symbolLookupRequestObject));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SaveFutureRootDataLocal(DataTable FutureRootData)
        {
            try
            {
                //TODO set Userid and requestID- osmhiv
                SaveFutureRootDataLocal(FutureRootData, "SM17", "2121");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void FutureRootDataResp(DataSet futureData)
        {
            try
            {
                if (EventSendDataByResKey != null && futureData != null)
                {
                    EventSendDataByResKey(this, new EventArgs<string, object, string, string>(CustomFIXConstants.MSG_SECMASTER_FutureMultiplierREQ, futureData, string.Empty, string.Empty));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void IsAliveResp()
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseType"></param>
        /// <param name="message"></param>
        public void GenericCentralSMResponse(string responseType, object message)
        {
            try
            {
                switch (responseType)
                {
                    case CustomFIXConstants.MSG_SECMASTER_SaveREQ:

                        String messageString = message as String;

                        if (!String.IsNullOrWhiteSpace(messageString) && StatusOfRequest != null)
                        {
                            Tuple<string, string, string> response = (Tuple<string, string, string>)_binaryFormatter.DeSerialize(messageString);
                            //if no issue then send "Success" to client, confirming success on Security saving in DB.
                            StatusOfRequest(this, new EventArgs<string, string, string>(response.Item1, response.Item2, response.Item3));
                        }

                        break;

                    case CustomFIXConstants.MSG_SECMASTER_UDA_Save:

                        String udaDataASBinaryString = message as String;

                        if (!String.IsNullOrWhiteSpace(udaDataASBinaryString))
                        {
                            Dictionary<String, Dictionary<String, object>> udaDataCol = _binaryFormatter.DeSerialize(udaDataASBinaryString) as Dictionary<String, Dictionary<String, object>>;
                            if (udaDataCol != null)
                                SaveUDAAttributesData(udaDataCol);
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion



        #region ISecMasterServices Members

        /// <summary>
        /// get account wise UDA from DB
        /// </summary>
        /// <returns></returns>
        public DataSet GetAccountSymbolUDAData()
        {
            try
            {
                return UDADataManager.GetAccountWiseUDAData();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Saving The Dynamic UDA
        /// </summary>
        /// <param name="accountSymbolUDADataTemp"></param>
        public void SaveAccountWiseUDAData(DataSet accountSymbolUDADataTemp)
        {
            try
            {
                UDADataManager.SaveAccountWiseUDADataInDB(accountSymbolUDADataTemp.GetXml());
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool IsDisposing)
        {
            try
            {
                if (IsDisposing)
                {
                    _pricingServicesProxy.Dispose();
                    _proxy.Dispose();
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

        #endregion


        public void SetHashCode()
        {
            try
            {
                _hashCode = this.GetHashCode();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Getting Dynamic UDA
        /// </summary>
        /// <returns></returns>
        public SerializableDictionary<string, DynamicUDA> GetDynamicUDAList()
        {
            try
            {
                return UDADataCache.GetInstance.GetDynamicUDAList();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Saving Dynamic UDA
        /// </summary>
        /// <returns></returns>
        public bool SaveDynamicUDA(DynamicUDA dynamicUda, string renamedKeys)
        {
            try
            {
                DynamicUDA oldUda = UDADataCache.GetInstance.GetDynamicUDAList(dynamicUda.Tag);

                bool saved = UDADataCache.GetInstance.SaveDynamicUDA(dynamicUda, renamedKeys);
                if (saved)
                {

                    if (oldUda != null && !oldUda.DefaultValue.Trim().Equals(dynamicUda.DefaultValue.Trim()))
                    {
                        //add uda with default value in cache
                        CallUpdateDynamicUDADefaultValueInSMCache(dynamicUda.Tag, oldUda.DefaultValue);
                    }
                    if (oldUda != null && !string.IsNullOrWhiteSpace(renamedKeys))
                    {
                        // update UDA cache for renamed master values in async function, PRANA-9833
                        CallUpdateDynamicUDAMasterValueInSMCache(dynamicUda.Tag, oldUda.MasterValues, dynamicUda.MasterValues);
                    }
                }
                return saved;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Add Tag with value in secMaster Object
        /// </summary>
        /// <param name="tag">UDA tag name</param>
        /// <param name="value">UDA value</param>
        private async void CallUpdateDynamicUDADefaultValueInSMCache(string tag, string defaultValue)
        {
            try
            {
                //wait for updated list of securities
                SecMasterbaseList updatedSecMasterCacheData = await UpdateDynamicUDADefaultValueInSMCacheAsync(tag, defaultValue);

                if (updatedSecMasterCacheData != null && updatedSecMasterCacheData.Count > 0)
                {
                    //send Updated Cache to client
                    SecMasterServerComponent.GetInstance.SendDataToClient(updatedSecMasterCacheData);

                    //Publish Security
                    PublishSecurityMasterData(updatedSecMasterCacheData);
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
        /// Add Tag with value in secMaster Object
        /// </summary>
        /// <param name="tag">UDA tag name</param>
        /// <param name="value">UDA value</param>
        /// <returns>SecMasterBaseObj list</returns>
        private Task<SecMasterbaseList> UpdateDynamicUDADefaultValueInSMCacheAsync(string tag, string defaultValue)
        {
            try
            {
                return System.Threading.Tasks.Task.Run<SecMasterbaseList>(() => SecMasterDataCache.GetInstance.UpdateDynamicUDADefaultValueInSMCache(tag, defaultValue));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return System.Threading.Tasks.Task.FromResult<SecMasterbaseList>(null);
            }
        }

        /// <summary>
        /// check if master value is set for any security
        /// </summary>
        /// <returns></returns>
        public bool CheckMasterValueAssigned(string tag, string value)
        {
            try
            {
                bool result = UDADataCache.GetInstance.CheckMasterValueAssigned(tag, value);
                return result;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// update UDA cache for renamed master values for dynamic udas
        /// </summary>
        /// <param name="udaTag">The dynamic UDA Name</param>
        /// <param name="oldMasterValues">list of old master values</param>
        /// <param name="newMasterValues">list of new master values</param>
        private async void CallUpdateDynamicUDAMasterValueInSMCache(string udaTag, SerializableDictionary<string, string> oldMasterValues, SerializableDictionary<string, string> newMasterValues)
        {
            try
            {
                //wait for updated list of securities
                SecMasterbaseList updatedSecMasterCacheData = await UpdateDynamicUDAMasterValueInSMCacheAsync(udaTag, oldMasterValues, newMasterValues);

                if (updatedSecMasterCacheData != null && updatedSecMasterCacheData.Count > 0)
                {
                    //send Updated Cache to client
                    SecMasterServerComponent.GetInstance.SendDataToClient(updatedSecMasterCacheData);

                    //Publish Security
                    PublishSecurityMasterData(updatedSecMasterCacheData);
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
        /// update UDA cache for renamed master values for dynamic udas
        /// </summary>
        /// <param name="udaTag">The dynamic UDA Name</param>
        /// <param name="oldMasterValues">list of old master values</param>
        /// <param name="newMasterValues">list of new master values</param>
        /// <returns></returns>
        private Task<SecMasterbaseList> UpdateDynamicUDAMasterValueInSMCacheAsync(string udaTag, SerializableDictionary<string, string> oldMasterValues, SerializableDictionary<string, string> newMasterValues)
        {
            try
            {
                return System.Threading.Tasks.Task.Run<SecMasterbaseList>(() => SecMasterDataCache.GetInstance.UpdateDynamicUDAMasterValueInSMCache(udaTag, oldMasterValues, newMasterValues));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return System.Threading.Tasks.Task.FromResult<SecMasterbaseList>(null);
            }
        }
    }

    internal class SubscriberDetails
    {
        private List<string> _symbolList;
        /// <summary>
        /// Contains the Level1 continuous symbol list for each subscriber.
        /// </summary>
        public List<string> SymbolList
        {
            get { return _symbolList; }
            set { _symbolList = value; }
        }
    }
}
