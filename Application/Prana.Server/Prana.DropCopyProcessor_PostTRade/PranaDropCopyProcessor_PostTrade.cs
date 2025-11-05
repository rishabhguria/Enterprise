using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.CustomMapper;
using Prana.DataManager;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.QueueManager;
using Prana.ServerCommon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks.Dataflow;

namespace Prana.DropCopyProcessor_PostTrade
{
    public class PranaDropCopyProcessor_PostTrade : IProcessingUnit, IDisposable
    {
        static object locker = new object();
        ITradeQueueProcessor _queueDispatching;
        IQueueProcessor _dbQueue;
        IProcessingUnit _orderProcessor;
        private static IProcessingUnit _pranaDropCopyProcessor = null;
        int _hashCode = 0;
        private MSMQQueueManager _errorQueue;
        BufferBlock<SecMasterBaseObj> _SecMstrDataBuffer = new BufferBlock<SecMasterBaseObj>();
        static PranaDropCopyProcessor_PostTrade()
        {
            _pranaDropCopyProcessor = new PranaDropCopyProcessor_PostTrade();
            DropCopyCacheManager_PostTrade.FillCacheFromDataBase();
        }

        void GetInstance_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                BufferSecMstrData(_SecMstrDataBuffer, e.Value);
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
        void BufferSecMstrData(ITargetBlock<SecMasterBaseObj> target, SecMasterBaseObj SecMstrObj)
        {
            try
            {
                target.SendAsync(SecMstrObj);
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

        async System.Threading.Tasks.Task<SecMasterBaseObj> ConsumeSecMstrDataAsync(IReceivableSourceBlock<SecMasterBaseObj> source)
        {
            try
            {
                // Read from the source buffer until the source buffer has no 
                // available output data.
                while (await source.OutputAvailableAsync())
                {
                    SecMasterBaseObj secMasterObj;
                    while (source.TryReceive(out secMasterObj))
                    {

                        if (secMasterObj != null)
                        {
                            string insymbol = secMasterObj.SymbologyMapping[secMasterObj.RequestedSymbology].ToUpper().Trim();
                            List<PranaMessage> pranaMsgList = PranaMessageCollectionForSymbols.GetAllPranaMsg(insymbol, (ApplicationConstants.SymbologyCodes)secMasterObj.RequestedSymbology);
                            if (Prana.ServerCommon.UserSettingConstants.IsDebugModeEnabled)
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter("Message Count of " + insymbol + " is " + pranaMsgList.Count + ". And TotalBuffered Symbols are " + _SecMstrDataBuffer.Count, LoggingConstants.CATEGORY_WARNING, 1, 1, System.Diagnostics.TraceEventType.Warning);
                            foreach (PranaMessage pranaMsg in pranaMsgList)
                            {
                                ProcessMessageOnSymbolReceived(pranaMsg, secMasterObj);
                            }
                        }
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

            return null;
        }
        public static IProcessingUnit GetInstance
        {
            get { return _pranaDropCopyProcessor; }
        }

        public static PranaDropCopyProcessor_PostTrade GetSameInstance
        {
            get { return (PranaDropCopyProcessor_PostTrade)_pranaDropCopyProcessor; }
        }

        public PranaMessage CreatePranaCXLReqMsgFromExecutionReport(ref PranaMessage pranaMsg)
        {
            try
            {
                string origClOrderID = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                PranaMessage pranaMsgReq = CacheManagerDAL.GetInstance().GetOrderDetailsByOrderID(origClOrderID);

                if (pranaMsgReq != null)
                {
                    string requestClOrderID = UniqueIDGenerator.GetClOrderID();

                    pranaMsgReq.MessageType = FIXConstants.MSGOrderCancelRequest;
                    pranaMsgReq.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = requestClOrderID;
                    pranaMsgReq.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrigClOrdID, origClOrderID);
                    pranaMsgReq.FIXMessage.ExternalInformation.AddField(FIXConstants.TagMsgType, FIXConstants.MSGOrderCancelRequest);

                    if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID))
                    {
                        pranaMsgReq.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrderID, pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value.ToString());
                    }

                    if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecInst))
                    {
                        pranaMsgReq.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecInst, pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagExecInst].Value.ToString());
                    }

                    if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTimeInForce))
                    {
                        pranaMsgReq.FIXMessage.ExternalInformation.AddField(FIXConstants.TagTimeInForce, pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagTimeInForce].Value.ToString());
                    }

                    if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagHandlInst))
                    {
                        pranaMsgReq.FIXMessage.ExternalInformation.AddField(FIXConstants.TagHandlInst, pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagHandlInst].Value.ToString());
                    }

                    if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagUnderlyingSymbol))
                    {
                        pranaMsgReq.FIXMessage.ExternalInformation.AddField(FIXConstants.TagUnderlyingSymbol, pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagUnderlyingSymbol].Value.ToString());
                    }
                    else
                    {
                        pranaMsgReq.FIXMessage.ExternalInformation.AddField(FIXConstants.TagUnderlyingSymbol, pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value.ToString());
                    }

                    if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTransactTime))
                    {
                        pranaMsgReq.FIXMessage.ExternalInformation.AddField(FIXConstants.TagTransactTime, pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagTransactTime].Value.ToString());
                    }

                    pranaMsgReq.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PNP, "0");

                    pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagClOrdID].Value = requestClOrderID;

                    return pranaMsgReq;
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
            return null;
        }

        #region IProcessingUnit Members

        public void Initlise(ITradeQueueProcessor queueDispatching, IQueueProcessor dbQueue, ISecMasterServices secmasterServices, IAllocationServices allocationServices, IProcessingUnit orderProcessor)
        {
            try
            {
                _secMasterServices = secmasterServices;
                PranaMessageCollectionForSymbols.SecMasterServices = _secMasterServices;
                _queueDispatching = queueDispatching;
                _orderProcessor = orderProcessor;
                _dbQueue = dbQueue;
                _hashCode = this.GetHashCode();
                _secMasterServices.Subscribe(_hashCode);
                _secMasterServices.SecMstrDataResponse += new EventHandler<EventArgs<SecMasterBaseObj>>(GetInstance_SecMstrDataResponse);
                _errorQueue = new MSMQQueueManager(ConfigurationManager.AppSettings[PranaServerConstants.DRP_CPY_ERROR_MSGS_PATH].ToString() + "_" + Prana.CommonDataCache.CachedDataManager.GetInstance.GetCompany().Rows[0]["CompanyID"].ToString());
                System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeSecMstrDataAsync(_SecMstrDataBuffer)).ConfigureAwait(false); ;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public void ProcessMessage(PranaMessage pranaMsg)
        {
            try
            {
                switch (pranaMsg.MessageType)
                {
                    case CustomFIXConstants.MsgDropCopyReceived:
                        if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSettlCurrency))
                        {
                            pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_SettlementCurrencyName, pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagSettlCurrency].Value);
                        }
                        PranaCustomMapper.ApplyRules(pranaMsg, Direction.In);
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TradeBlocked) && pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TradeBlocked].Value == "1")
                        {
                            return;
                        }
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AUECID))
                        {
                            int auecID = int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECID].Value);
                            int AssetID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetAssetIdByAUECId(auecID);
                            int CurrencyID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetCurrencyIdByAUECID(auecID);
                            int underLyingID = CachedDataManager.GetInstance.GetUnderlyingID(auecID);
                            if (!pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ExchangeID))
                            {
                                int ExchangeID = Prana.CommonDataCache.CachedDataManager.GetInstance.GetExchangeIdFromAUECId(auecID);
                                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ExchangeID, ExchangeID.ToString());
                            }
                            pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_AssetID, AssetID.ToString());
                            pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_UnderlyingID, underLyingID.ToString());
                            pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CurrencyID, CurrencyID.ToString());
                            ProcessMessageAfterSettingAUEC(pranaMsg);
                        }
                        else
                        {
                            RequestForAllSymbology(pranaMsg);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                Error(this, new EventArgs<string, PranaMessage>("Prolem at PranaDropOrderProcessor_PM", pranaMsg));
            }
        }

        private void ProcessMessageAfterSettingAUEC(PranaMessage pranaMsg)
        {
            ServerCommonBusinessLogic.SetDateDetails(pranaMsg);
            bool isAUECDateAppendRequired = true;
            if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsAUECDateAppendRequired) && !string.IsNullOrEmpty(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsAUECDateAppendRequired].Value))
            {
                isAUECDateAppendRequired = bool.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsAUECDateAppendRequired].Value);
            }

            if (CheckMultiDayHistoryForDC(pranaMsg))
            {
                isAUECDateAppendRequired = false;
            }
            if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_AUECLocalDate) && !string.IsNullOrEmpty(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECLocalDate].Value) && isAUECDateAppendRequired)
            {
                if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID) && !string.IsNullOrEmpty(pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value))
                {
                    string orderID = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value;
                    orderID = orderID + "_" + Convert.ToDateTime(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECLocalDate].Value).ToString("yyyy/MM/dd").Replace("/", "");

                    pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrderID, orderID);
                }

                if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID) && !string.IsNullOrEmpty(pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value))
                {
                    string origClOrdID = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                    origClOrdID = origClOrdID + "_" + Convert.ToDateTime(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECLocalDate].Value).ToString("yyyy/MM/dd").Replace("/", "");

                    pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrigClOrdID, origClOrdID);
                }
            }
            _orderProcessor.ProcessMessage(pranaMsg);
        }

        private bool CheckMultiDayHistoryForDC(PranaMessage pranaMsg)
        {
            try
            {
                if (OrderInformation.IsMultiDayOrder(pranaMsg))
                {
                    return true;
                }
                if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrderID))
                {
                    string orderid = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrderID].Value.ToString();
                    string clOrderID = DropCopyCacheManager_PostTrade.GetClOrderID(orderid);
                    if (OrderCacheManager.StagedSubsCollection.ContainsKey(clOrderID))
                        return true;
                }
                if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOrigClOrdID))
                {
                    string orderid = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value.ToString();
                    string clOrderID = DropCopyCacheManager_PostTrade.GetClOrderID(orderid);
                    if (OrderCacheManager.StagedSubsCollection.ContainsKey(clOrderID))
                        return true;
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
            return false;
        }

        private void ProcessMessageOnSymbolReceived(PranaMessage pranaMsg, SecMasterBaseObj secMstrData)
        {
            try
            {
                pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value = secMstrData.SymbologyMapping[(int)ApplicationConstants.PranaSymbology];
                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_AUECID, secMstrData.AUECID.ToString());
                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_AssetID, secMstrData.AssetID.ToString());
                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_UnderlyingID, secMstrData.UnderLyingID.ToString());
                if (!pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ExchangeID))
                    pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ExchangeID, secMstrData.ExchangeID.ToString());
                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_CurrencyID, secMstrData.CurrencyID.ToString());
                ProcessMessageAfterSettingAUEC(pranaMsg);
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
        public PranaMessage CreatePranaReqMsgFromExecutionReport(PranaMessage pranaMsg)
        {
            try
            {
                PranaMessage pranaMsgReq = pranaMsg.Clone();

                pranaMsgReq.MessageType = FIXConstants.MSGOrder;
                pranaMsgReq.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGOrder;

                pranaMsgReq.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.MsgDropCopy_PM).ToString());

                return pranaMsgReq;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }
        public PranaMessage CreatePranaCXLReplaceMsgFromExecutionReport(PranaMessage pranaMsg)
        {
            try
            {
                PranaMessage pranaMsgReq = pranaMsg.Clone();
                pranaMsgReq.MessageType = FIXConstants.MSGOrderCancelReplaceRequest;

                pranaMsgReq.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value = FIXConstants.MSGOrderCancelReplaceRequest;
                string origClOrderID = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagOrigClOrdID].Value;
                string origClOrderIDPrana = DropCopyCacheManager_PostTrade.GetClOrderID(origClOrderID);
                string parentclOrderID = DropCopyCacheManager_PostTrade.GetParentClOrderID(origClOrderID);

                pranaMsgReq.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ParentClOrderID, parentclOrderID);
                pranaMsgReq.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrigClOrdID, origClOrderIDPrana);
                pranaMsgReq.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_PranaMsgType, ((int)OrderFields.PranaMsgTypes.MsgDropCopy_PM).ToString());
                return pranaMsgReq;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        private void RequestForAllSymbology(PranaMessage pranaMsg)
        {
            try
            {
                // main symbol and main symboilogy
                string inSymbol = string.Empty;
                ApplicationConstants.SymbologyCodes _symbologyCode = (ApplicationConstants.SymbologyCodes)int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Symbology].Value);
                switch (_symbologyCode)
                {
                    case ApplicationConstants.SymbologyCodes.TickerSymbol:
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TickerSymbol))
                        {
                            inSymbol = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TickerSymbol].Value.ToUpper().Trim();
                        }
                        break;
                    case ApplicationConstants.SymbologyCodes.ReutersSymbol:
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ReutersSymbol))
                        {
                            inSymbol = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ReutersSymbol].Value.ToUpper().Trim();
                        }
                        break;
                    case ApplicationConstants.SymbologyCodes.ISINSymbol:
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_ISINSymbol))
                        {
                            inSymbol = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_ISINSymbol].Value.ToUpper().Trim();
                        }
                        break;
                    case ApplicationConstants.SymbologyCodes.SEDOLSymbol:
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_SEDOLSymbol))
                        {
                            inSymbol = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_SEDOLSymbol].Value.ToUpper().Trim();
                        }
                        break;
                    case ApplicationConstants.SymbologyCodes.CUSIPSymbol:
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CusipSymbol))
                        {
                            inSymbol = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CusipSymbol].Value.ToUpper().Trim();
                        }
                        break;
                    case ApplicationConstants.SymbologyCodes.BloombergSymbol:

                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_BloombergSymbol))
                        {
                            inSymbol = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_BloombergSymbol].Value.ToUpper().Trim();
                        }
                        break;
                    case ApplicationConstants.SymbologyCodes.OSIOptionSymbol:
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OSIOptionSymbol))
                        {
                            inSymbol = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OSIOptionSymbol].Value.ToUpper().Trim();
                        }
                        break;
                    case ApplicationConstants.SymbologyCodes.IDCOOptionSymbol:
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IDCOOptionSymbol))
                        {
                            inSymbol = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IDCOOptionSymbol].Value.ToUpper().Trim();
                        }
                        break;
                    case ApplicationConstants.SymbologyCodes.OPRAOptionSymbol:
                        if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OPRAOptionSymbol))
                        {
                            inSymbol = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OPRAOptionSymbol].Value.ToUpper().Trim();
                        }
                        break;
                    default:
                        break;
                }
                string underlyingSymbol = string.Empty;
                if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagUnderlyingSymbol))
                    underlyingSymbol = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagUnderlyingSymbol].Value.ToUpper().Trim();

                if (string.IsNullOrEmpty(inSymbol))
                {
                    inSymbol = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value.ToUpper().Trim();
                    _symbologyCode = ApplicationConstants.SymbologyCodes.TickerSymbol;
                }

                SecMasterRequestObj secMasterReqObj = new SecMasterRequestObj();
                secMasterReqObj.AddData(inSymbol, _symbologyCode);
                secMasterReqObj.SymbolDataRowCollection[0].UnderlyingSymbol = underlyingSymbol;
                secMasterReqObj.HashCode = _hashCode;
                PranaMessageCollectionForSymbols.Add(secMasterReqObj, pranaMsg, _hashCode);
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

        public event EventHandler<EventArgs<string, PranaMessage>> Error;
        public void SaveCachedErrorOrders()
        {
            List<PranaMessage> pranaMssgs = PranaMessageCollectionForSymbols.GetAllPranaMsg();
            foreach (PranaMessage pranaMsg in pranaMssgs)
            {
                QueueMessage qMsg = new QueueMessage(CustomFIXConstants.MSG_Trade, "", "", pranaMsg);
                _errorQueue.SendMessage(qMsg);
            }
        }
        public List<PranaMessage> GetCachedErrorOrders()
        {
            return PranaMessageCollectionForSymbols.GetAllPranaMsg();

        }
        private string _name = "DropCopy_PostTrade";

        public string Name
        {
            get { return _name; }
        }
        public List<PranaMessage> GetAllCachedMessages()
        {
            return DropCopyCacheManager_PostTrade.GetCachedMessages();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
                _secMasterServices.SecMstrDataResponse -= new EventHandler<EventArgs<SecMasterBaseObj>>(GetInstance_SecMstrDataResponse);
            if (_errorQueue != null)
            {
                _errorQueue.Dispose();
                _errorQueue = null;
            }
        }


        #endregion

        private ISecMasterServices _secMasterServices;

        #region IProcessingUnit Members

        public Dictionary<string, List<PranaMessage>> GetAndClearPranaMessages()
        {
            return PranaMessageCollectionForSymbols.GetAndClearPranaMessages();
        }

        #endregion
    }
}
