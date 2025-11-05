using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes;
using Prana.Global;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Prana.CAServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class CAServiceManager : ICAServices, ILiveFeedCallback, IDisposable
    {
        private void SendMessage(QueueMessage msg)
        {
            try
            {
                if (_postTradeServices.IsConnected)
                {
                    msg.HashCode = CommonHelper.REQUESTER_HASHCODE;
                    _postTradeServices.SendMessage(msg);
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

        public void Clear()
        {

        }

        #region ICAServices Members

        private IAllocationServices _allocationServices;
        private IClosingServices _closingServices;
        private ISecMasterServices _secmasterProxy;
        private ICashManagementService _cashManagementService;

        public IAllocationServices AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
        }

        public IClosingServices ClosingServices
        {
            set
            {
                _closingServices = value;
            }
        }

        public ICashManagementService CashManagementService
        {
            set
            {
                _cashManagementService = value;
            }
        }

        private IActivityServices _activityServices;

        public IActivityServices ActivityServices
        {
            set { _activityServices = value; }
        }

        //public event MsgReceivedDelegate CAResponseReceived;
        public event EventHandler<EventArgs<QueueMessage>> CAResponseReceived;
        IPostTradeServices _postTradeServices = null;
        public IPostTradeServices PostTradeServices
        {
            set
            {
                _postTradeServices = value;
                _postTradeServices.MessageReceived += new EventHandler<EventArgs<QueueMessage>>(_postTradeServices_MessageReceived);
                //new MsgReceivedDelegate(_postTradeServices_MessageReceived);
            }
        }

        void _postTradeServices_MessageReceived(object sender, EventArgs<QueueMessage> e)
        {
            if (CAResponseReceived != null)
            {
                CAResponseReceived(sender, e);
            }
        }

        public ISecMasterServices SecmasterProxy
        {
            set
            {
                _secmasterProxy = value;
            }
        }

        public void Initialize(int hashCode)
        {
            CreatePublishingProxy();
            CreatePricingServiceProxy();
            _closingServices.RefreshCAData();
            CAFactory.Initialize(_postTradeServices, hashCode, _allocationServices, _closingServices, _proxyPublishing, _activityServices, _pricingServicesProxy, _secmasterProxy, _cashManagementService);
        }

        ProxyBase<IPublishing> _proxyPublishing;

        private void CreatePublishingProxy()
        {
            _proxyPublishing = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void ValidateCAInfo(CorporateActionType caType, ref DataTable caTable)
        {
            DataRowCollection rows = caTable.Rows;
            ICorporateActionBaseRule corpActionRule = CAFactory.GetCAInstance(caType);
            if (corpActionRule != null)
            {
                corpActionRule.ValidateCAInfo(ref rows);
            }
            else
            {
                throw new Exception("Can not validate the corporate action information");
            }


        }

        public DataSet GetCAsForDateRange(CorporateActionType caType, DateTime fromDate, DateTime toDate, bool isApplied)
        {
            CAOnProcessObjects requestObject = null;
            try
            {
                requestObject = new CAOnProcessObjects();
                requestObject.CAType = caType;
                requestObject.FromDate = fromDate;
                requestObject.ToDate = toDate;
                requestObject.IsApplied = isApplied;
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
            return DBManager.GetAllCAs(requestObject);
        }

        public DataSet GetLatestCorpActionForSymbol(string caSymbols)
        {


            return DBManager.getLatestCorpAction(caSymbols);

        }

        public void SaveCAsOnly(DataRow dataRow)
        {
            try
            {
                DataTable dTable = dataRow.Table.Clone();
                dTable.Rows.Add(dataRow.ItemArray);
                SaveCAsOnly(dTable);
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

        public bool SaveCAsOnly(DataTable dt)
        {
            CAOnProcessObjects caOnProcessObject = null;
            try
            {
                string interCaStr = CommonHelper.GetCorporateActionString(dt);

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(interCaStr);

                string caStr = xmlDoc.SelectSingleNode("DocumentElement").InnerXml;

                caOnProcessObject = new CAOnProcessObjects();
                caOnProcessObject.CorporateActionListString = caStr;

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

            return DBManager.SaveCorporateActionWithoutApplying(caOnProcessObject);
        }


        public bool UpdateCAsOnly(DataTable updateTable)
        {
            CAOnProcessObjects caOnProcessObject = null;
            try
            {
                string caStr = CommonHelper.GetCorporateActionString(updateTable);

                caOnProcessObject = new CAOnProcessObjects();
                caOnProcessObject.CorporateActionListString = caStr;
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
            return DBManager.UpdateCorporateActionWithoutApplying(caOnProcessObject);
        }


        public bool DeleteCAs(string caIDs)
        {
            return DBManager.DeleteCorproateActions(caIDs);
        }

        public CAPreviewResult PreviewCorporateActions(CorporateActionType caType, DataTable caTable, ref TaxlotBaseCollection taxlotList, string commaSeparatedAccountIds, CAPreferences caPref, int brokerId)
        {
            return CAFactory.GetCAInstance(caType).PreviewCorporateActions(caTable.Rows, ref taxlotList, commaSeparatedAccountIds, caPref, brokerId);
        }


        public bool SaveCorporateAction(CorporateActionType caType, string corporateActionListString, TaxlotBaseCollection updatedTaxlots, int userID)
        {
            bool isCAsaved = false;

            try
            {
                isCAsaved = CAFactory.GetCAInstance(caType).SaveCorporateAction(corporateActionListString, updatedTaxlots, userID);
                if (isCAsaved.Equals(true))
                {
                    _closingServices.RefreshCADataonNewThread();
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
            return isCAsaved;
        }


        public string CheckTaxlotsBeforeUndoPreview(CorporateActionType caType, Dictionary<string, DateTime> caWiseDates)
        {
            try
            {
                return CAFactory.GetCAInstance(caType).CheckTaxlotsBeforeUndoPreview(caWiseDates);
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




        public TaxlotBaseCollection PreviewUndoCorporateActions(CorporateActionType caType, string caIDs, DataTable caTable)
        {
            return CAFactory.GetCAInstance(caType).PreviewUndoCorporateActions(caIDs, caTable.Rows);
        }


        public bool UndoCorporateActions(CorporateActionType caType, string caIDs, TaxlotBaseCollection taxlots, bool isSMModificationRequired)
        {
            bool isCAundo = CAFactory.GetCAInstance(caType).UndoCorporateActions(caIDs, taxlots, isSMModificationRequired);
            if (isCAundo.Equals(true))
            {
                _closingServices.RefreshCADataonNewThread();
            }
            return isCAundo;
        }

        public bool UndoCorporateActions(CorporateActionType caType, string caIDs, TaxlotBaseCollection taxlots)
        {
            bool isCAundo = CAFactory.GetCAInstance(caType).UndoCorporateActions(caIDs, taxlots);
            if (isCAundo.Equals(true))
            {
                _closingServices.RefreshCADataonNewThread();
            }
            return isCAundo;
        }


        public void ResetCache()
        {
            CAFactory.ResetCache();
        }

        #endregion

        #region ILiveFeedCallback Members
        public void SnapshotResponse(SymbolData data, [Optional, DefaultParameterValue(null)] SnapshotResponseData snapshotResponseData)
        {
            throw new NotImplementedException();
        }

        public void OptionChainResponse(string symbol, List<OptionStaticData> data)
        {
        }

        public void LiveFeedConnected()
        {
            throw new NotImplementedException();
        }

        public void LiveFeedDisConnected()
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_proxyPublishing != null)
                        _proxyPublishing.Dispose();
                    if (_pricingServicesProxy != null)
                        _pricingServicesProxy.Dispose();
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

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion
    }
}
