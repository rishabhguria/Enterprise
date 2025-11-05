using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.ServiceModel;
using System.Text;

namespace Prana.ExpnlService
{
    /// <summary>
    /// The order Fill Manager
    /// </summary>
    /// <seealso cref="IPublishing" />
    /// <seealso cref="System.IDisposable" />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    internal class OrderFillManager : IPublishing, IDisposable
    {
        /// <summary>
        /// Occurs when [order fill received].
        /// </summary>
        public event ParameterizedMethodHandler OrderFillReceived;

        /// <summary>
        /// The auec wise local dates
        /// </summary>
        ConcurrentDictionary<int, DateTime> _auecWiseLocalDates = null;
        /// <summary>
        /// Sets the auec wise local dates.
        /// </summary>
        /// <value>
        /// The auec wise local dates.
        /// </value>
        public ConcurrentDictionary<int, DateTime> AuecWiseLocalDates
        {
            set { _auecWiseLocalDates = value; }
        }

        /// <summary>
        /// The order fill manager
        /// </summary>
        private static OrderFillManager _orderFillManager = null;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        internal static OrderFillManager GetInstance()
        {
            if (_orderFillManager == null)
            {
                _orderFillManager = new OrderFillManager();
            }
            return _orderFillManager;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="OrderFillManager"/> class from being created.
        /// </summary>
        private OrderFillManager()
        {
            MakeProxy();
        }

        /// <summary>
        /// The proxy
        /// </summary>
        DuplexProxyBase<ISubscription> _proxy;
        /// <summary>
        /// Makes the proxy.
        /// </summary>
        private void MakeProxy()
        {
            try
            {
                _proxy = new DuplexProxyBase<ISubscription>("TradeSubscriptionEndpointAddress", this);

                _proxy.Subscribe(Topics.Topic_Allocation, null);
                //FilterDataByToDate filterdata = new FilterDataByToDate();
                //filterdata.ToDate = DateTime.Today.Date.AddDays(-1);

                //Bharat Kumar Jangir (25 Nov 2014)
                //Special Handing for same day Corporate Action - Allowing same day closing data for updation of Costbasis values
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5357
                //FilterDataForLastDateModified filterDataDateModified = new FilterDataForLastDateModified();
                //filterDataDateModified.TillDate = DateTime.Today.Date.AddDays(-1);
                //ExposurePnLScheduler.GetInstance().GetMostLaggingDate().AddDays(-1);

                //List<FilterData> filters = new List<FilterData>();
                //filters.Add(filterdata);
                //filters.Add(filterDataDateModified);
                //_proxy.Subscribe(Topics.Topic_Closing, filters);

                _proxy.Subscribe(Topics.Topic_Closing, null);
                //_proxy.InnerChannel.Subscribe(Topics.Topic_CreateGroup);
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

        #region IPublishing Members

        /// <summary>
        /// Publishes the specified MessageData.
        /// </summary>
        /// <param name="e">The MessageData.</param>
        /// <param name="topicName">Name of the topic.</param>
        public void Publish(MessageData e, string topicName)
        {
            try
            {
                int IsTracingEnabledForPublishedTaxlots = Convert.ToInt32(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsTracingEnabledForPublishedTaxlots").ToString());

                if (topicName.Equals(string.Empty))
                {
                    topicName = e.TopicName;
                }

                if (topicName == Topics.Topic_Allocation || topicName == Topics.Topic_Closing)
                {
                    System.Object[] dataList = (System.Object[])e.EventData;
                    foreach (Object obj in dataList)
                    {
                        TaxLot taxlot = (TaxLot)obj;

                        if (taxlot.AssetID == (int)AssetCategory.FX)
                            continue;

                        if (IsTracingEnabledForPublishedTaxlots > 0)
                        {
                            StringBuilder logMessage = new StringBuilder();
                            logMessage.Append("Published taxlot received with details.");
                            logMessage.Append(" Symbol = " + taxlot.Symbol);
                            logMessage.Append(", Taxlot ID = " + taxlot.TaxLotID);
                            logMessage.Append(", AUEC ID = " + taxlot.AUECID);
                            logMessage.Append(", Asset ID = " + taxlot.AssetID);
                            logMessage.Append(", Underlying ID = " + taxlot.UnderlyingID);
                            logMessage.Append(", Exchange = " + taxlot.ExchangeID);
                            logMessage.Append(", Currency ID = " + taxlot.CurrencyID);
                            logMessage.Append(", Order Side = " + taxlot.OrderSideTagValue);
                            logMessage.Append(", Account ID = " + taxlot.Level1ID);
                            logMessage.Append(", Strategy ID = " + taxlot.Level2ID);
                            logMessage.Append(", Cum Quantity = " + taxlot.CumQty);
                            logMessage.Append(", Open Quantity = " + taxlot.TaxLotQty);
                            logMessage.Append(", AUEC Local Date = " + taxlot.AUECLocalDate);
                            logMessage.Append(", AUEC Modified Date = " + taxlot.AUECModifiedDate);
                            logMessage.Append(", Process Date = " + taxlot.ProcessDate);
                            logMessage.Append(", Original Purchase Date = " + taxlot.OriginalPurchaseDate);
                            logMessage.Append(", Closing Date = " + taxlot.ClosingDate);
                            logMessage.Append(", Settlement Date = " + taxlot.SettlementDate);
                            logMessage.Append(", Published From = " + topicName.ToString());
                            logMessage.Append(", Closing Status = " + taxlot.ClosingStatus);
                            logMessage.Append(", Closing Mode = " + taxlot.ClosingMode);
                            logMessage.Append(", Position Tag = " + taxlot.PositionTag);
                            logMessage.Append(", TaxLot State = " + taxlot.TaxLotState);
                            logMessage.Append(", Username = " + CachedDataManager.GetInstance.GetUserText(taxlot.CompanyUserID));
                            logMessage.Append(", Taxlot Received at Expnl = " + DateTime.Now);
                            logMessage.Append(", Taxlot Received at Expnl (UTC) = " + DateTime.UtcNow);

                            if (IsTracingEnabledForPublishedTaxlots == 1)
                                LogAndDisplayOnInformationReporter.GetInstance.Write(logMessage.ToString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);

                            else if (IsTracingEnabledForPublishedTaxlots == 2)
                                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(logMessage.ToString(), LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                        }

                        if (String.IsNullOrEmpty(taxlot.Symbol) || String.IsNullOrEmpty(taxlot.OrderSideTagValue) || taxlot.CurrencyID <= 0)
                        {
                            string logMessage = "Data missing for published taxlot. Symbol = " + taxlot.Symbol + ", Taxlot ID = " + taxlot.TaxLotID + ", Order Side = " + taxlot.OrderSideTagValue + ", Account ID = " + taxlot.Level1ID + ", AUEC ID = " + taxlot.AUECID + ", Exchange ID = " + taxlot.ExchangeID + ", Currency ID = " + taxlot.CurrencyID;
                            LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(logMessage, LoggingConstants.CATEGORY_INFORMATION, 1, 1, TraceEventType.Information);
                        }

                        //Gaurav:To remove back date closing data from PM/EXPNL server. 
                        //Closing module is publishing only back date closing and not current date closing. So this condition will not delete current date trades
                        DateTime todayAUECLocalDate = DateTimeConstants.MinValue;
                        if (_auecWiseLocalDates != null && !_auecWiseLocalDates.TryGetValue((int)taxlot.AUECID, out todayAUECLocalDate))
                        {
                            todayAUECLocalDate = DateTimeConstants.MinValue;
                        }

                        // Removing closed taxlots from PM - Added Closing mode check to remove closed taxlots if they are closed through cost adjustment
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7195
                        if (topicName == Topics.Topic_Closing && taxlot != null && taxlot.ClosingStatus == ClosingStatus.Closed && (taxlot.AUECModifiedDate.Date < todayAUECLocalDate.Date || taxlot.ClosingMode == ClosingMode.CostBasisAdjustment) && taxlot.TaxLotQty == 0)
                        {
                            if (OrderFillReceived != null)
                            {
                                OrderFillReceived(taxlot, ApplicationConstants.TaxLotState.Deleted, topicName);
                            }
                        }
                        else
                        {
                            //Ignoring the taxlots which have been fully closed but if current day closing, then the taxlots have to be updated even if fully closed.
                            if (taxlot != null && (topicName == Topics.Topic_Allocation && taxlot.ClosingStatus == ClosingStatus.Closed && taxlot.TaxLotState != ApplicationConstants.TaxLotState.Deleted && taxlot.AUECModifiedDate.Date < todayAUECLocalDate.Date))
                            {
                                continue;
                            }

                            // Blocking taxlots closed by cost adjustment
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-6867
                            if (topicName == Topics.Topic_Closing && taxlot != null && taxlot.ClosingStatus == ClosingStatus.Closed && taxlot.ClosingMode == ClosingMode.CostBasisAdjustment)
                            {
                                continue;
                            }

                            if (topicName == Topics.Topic_Closing && taxlot != null && (taxlot.ClosingStatus == ClosingStatus.PartiallyClosed || taxlot.ClosingStatus == ClosingStatus.Closed) && taxlot.AUECModifiedDate.Date >= todayAUECLocalDate.Date)
                            {
                                continue;
                            }

                            if (taxlot != null)
                            {
                                if (OrderFillReceived != null)
                                {
                                    NameValueFiller.FillNameDetailsOfMessage(taxlot);
                                    OrderFillReceived(taxlot, taxlot.TaxLotState, topicName);
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

        /// <summary>
        /// Gets the name of the receiver unique.
        /// </summary>
        /// <returns></returns>
        public string getReceiverUniqueName()
        {
            return "ExpnlServer";
        }
        #endregion

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (_proxy != null)
                {
                    try
                    {
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_Allocation);
                        _proxy.InnerChannel.UnSubscribe(Topics.Topic_Closing);
                    }
                    catch
                    {
                    }

                    _proxy.Dispose();
                    _proxy = null;
                }
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
        #endregion
    }
}
