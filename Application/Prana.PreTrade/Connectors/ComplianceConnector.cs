using Prana.AmqpAdapter.Amqp;
using Prana.AmqpAdapter.Delegates;
using Prana.AmqpAdapter.Enums;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.Constants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Messaging;

namespace Prana.PreTrade.Connectors
{
    /// <summary>
    /// The class does the following
    /// 1. Send orders to esper
    /// 2. Get alerts from rule mediator
    /// 3. Get the LiveFeed disconnected alert
    /// </summary>
    internal class ComplianceConnector : IDisposable
    {
        String _otherDataExchange;
        String _alertListener;

        public event EventHandler<EventArgs<Alert>> AlertReceived;

        public event EventHandler<EventArgs<DataSet>> CalculationResponseReceived;

        MessageQueue myQueue = null;
        private bool _enableComplianceLogging = Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("EnableComplianceLogging"));

        /// <summary>
        /// Initialise the connection to esper to send and receive data
        /// </summary>
        internal ComplianceConnector()
        {
            try
            {
                AmqpHelper.Started += new ListenerStarted(AmqpListenerStarted);

                // initialise the taxlot sender
                String orderOutputQueue = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OrderOutputQueue);
                AmqpHelper.InitializeSender("OrderOutput", orderOutputQueue, MediaType.Queue);

                String securityDetailsQueue = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_SecurityDetailsQueue);
                AmqpHelper.InitializeSender("SecurityDetails", securityDetailsQueue, MediaType.Queue);

                // initialise the Basket Compliance sender
                String basketComplianceQueueName = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_BasketComplianceQueue);
                AmqpHelper.InitializeSender("BasketComplianceQueue", basketComplianceQueueName, MediaType.Queue);

                /// listener for LiveFeed disconnected alert and calculation response
                _otherDataExchange = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_OtherDataExchange);
                AmqpHelper.InitializeListenerForExchange(_otherDataExchange, MediaType.Exchange_Direct, new List<String>() { "TradeCancelled" });
                AmqpHelper.InitializeListenerForExchange(_otherDataExchange, MediaType.Exchange_Direct, new List<String>() { "CalculationResponse" });

                AmqpHelper.InitializeSender("InTrade", _otherDataExchange, MediaType.Exchange_Direct);

                AmqpHelper.InitializeSender("CalculationRequest", _otherDataExchange, MediaType.Exchange_Direct);

                //Sending CashFlow from Rebalancer to BasketComplianceService
                AmqpHelper.InitializeSender("CashFlow", _otherDataExchange, MediaType.Exchange_Direct);

                //Sending Simulation preference to BasketComplianceService
                AmqpHelper.InitializeSender("SimulationPreference", _otherDataExchange, MediaType.Exchange_Direct);

                /// listener for violated alerts
                _alertListener = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_ComplianceSettings, ConfigurationHelper.CONFIGKEY_RuleResponseUserDefined);
                AmqpHelper.InitializeListenerForQueue(_alertListener);
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

        private void AmqpListenerStarted(object sender, AmqpAdapter.EventArguments.ListenerStartedEventArguments e)
        {
            try
            {
                if (e.AmqpReceiver.MediaName == _otherDataExchange || e.AmqpReceiver.MediaName == _alertListener)
                    e.AmqpReceiver.AmqpDataReceived += new DataReceived(AmqpDataReceived);
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

        private void AmqpDataReceived(object sender, AmqpAdapter.EventArguments.DataReceivedEventArguments e)
        {
            try
            {

                // A different listener for HEARTBEAT is present
                if (!e.RoutingKey.Contains("HEARTBEAT"))
                {
                    if (e.RoutingKey.Contains("CalculationResponse"))
                        CalculationsReceived(e.DsReceived);
                    else
                        ValidationRecievedAction(e.DsReceived);
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

        private void CalculationsReceived(DataSet dataSet)
        {
            if (CalculationResponseReceived != null)
                CalculationResponseReceived(this, new EventArgs<DataSet>(dataSet));
        }

        /// <summary>
        /// Handle the received alert
        /// </summary>
        /// <param name="dataSet"></param>
        private void ValidationRecievedAction(DataSet dataSet)
        {
            try
            {
                Alert alert = Alert.GetAlertObject(dataSet);
                if (alert != null)
                    AlertReceived(this, new EventArgs<Alert>(alert));
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
        /// Send the taxlots to compliance engine for validation
        /// </summary>
        /// <param name="taxlots"></param>
        internal void SendTaxlotsToEsper(List<TaxLot> taxlots, bool isReplacedOrder = false)
        {
            try
            {
                if (taxlots == null || taxlots.Count == 0)
                    return;

                int deletedTaxlotsCount = 0;
                int finalTaxlotCount = 0;
                if (taxlots.Count == 1 && taxlots[0].TaxLotState.Equals(ApplicationConstants.TaxLotState.Deleted))
                {
                    AmqpHelper.SendObject(taxlots[0], "OrderOutput", null);
                    finalTaxlotCount = taxlots.Count;
                }
                else
                {
                    foreach (TaxLot taxlot in taxlots)
                    {
                        if (taxlot.TaxLotState.Equals(ApplicationConstants.TaxLotState.Deleted))
                            deletedTaxlotsCount++;
                        else
                            AmqpHelper.SendObject(taxlot, "OrderOutput", null);
                    }
                    finalTaxlotCount = (taxlots.Count - deletedTaxlotsCount);

                    #region In case of Replace order, if no taxlot is sent to Esper then added handling
                    if (taxlots.Count == deletedTaxlotsCount && isReplacedOrder)
                    {
                        int sentTaxlotCount = 0;
                        foreach (TaxLot taxlot in taxlots)
                        {
                            if (taxlot.CumQty != 0)
                            {
                                sentTaxlotCount++;
                                AmqpHelper.SendObject(taxlot, "OrderOutput", null);
                            }
                        }
                        if(sentTaxlotCount != 0)
                            finalTaxlotCount = sentTaxlotCount;
                    }
                    #endregion
                }

                // Send an EOM
                Object eom = new { BasketId = taxlots[0].GroupID, Count = finalTaxlotCount, IsEom = true, IsPricingAvailable = taxlots[0].IsPricingAvailable };
                AmqpHelper.SendObject(eom, "OrderOutput", null);
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

        internal void SendTaxlotsToBasketCompliance(List<TaxLot> taxlots)
        {
            try
            {
                if (taxlots.Count == 0)
                    return;
                //foreach (TaxLot taxlot in taxlots)
                AmqpHelper.SendObject(taxlots, "BasketComplianceQueue", null);

                // Send an EOM
                List<Object> eomObject = new List<Object>();
                eomObject.Add(new { BasketId = taxlots[0].GroupID, Count = taxlots.Count, IsEom = true });
                AmqpHelper.SendObject(eomObject, "BasketComplianceQueue", null);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sending CashFlow to Basket Compliance Service
        /// </summary>
        internal void SendCashInFlowToBasketComplianceService(List<CashFlowToCompliance> cashFlow)
        {
            try
            {
                foreach (CashFlowToCompliance cashFlowToCompliance in cashFlow)
                {
                    AmqpHelper.SendObject(cashFlowToCompliance, "CashFlow", "CashFlow");
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sending simulation preference to Basket Compliance Service
        /// </summary>
        internal void SendSimulationPreferenceToBasketCompliance(bool isRealTimePositions, bool isComingFromRebalancer)
        {
            try
            {
                Dictionary<String, Object> rebalancePref = new Dictionary<String, Object>();
                rebalancePref.Add("IsRealTimePositions", isRealTimePositions);
                rebalancePref.Add("IsComingFromRebalancer", isComingFromRebalancer);
                AmqpHelper.SendObject(rebalancePref, "SimulationPreference", "SimulationPreference");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sends Intrade through the key provided.
        /// </summary>
        /// <param name="taxlotList"></param>
        /// <param name="key"></param>
        internal void SendInTradesToEsper(List<TaxLot> taxlotList, String key)
        {
            try
            {
                if (_enableComplianceLogging)
                {
                    if (myQueue == null)
                    {
                        myQueue = new MessageQueue(".\\Private$\\ComplianceLoggingQueue");
                    }
                    if (!MessageQueue.Exists(".\\Private$\\ComplianceLoggingQueue"))
                    {
                        myQueue = MessageQueue.Create(".\\Private$\\ComplianceLoggingQueue");
                    }
                }
                foreach (TaxLot taxlot in taxlotList)
                {
                    if (myQueue != null)
                    {
                        myQueue.Send(taxlot, "Compliance Logging");
                    }
                    if (key.Equals("InTradeStage") && taxlot.TaxLotQty < 0 && taxlot.TaxLotState.Equals(ApplicationConstants.TaxLotState.Updated))
                    {
                        taxlot.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                    }
                    AmqpHelper.SendObject(taxlot, "InTrade", key);
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
        /// Send the calculation request to esper
        /// </summary>
        /// <param name="compression"></param>
        /// <param name="fieldsList"></param>
        internal void SendCalculationRequestToEsper(String requestId, String compression, String fieldsList)
        {
            try
            {
                Object request = new { RequestId = requestId, Compression = compression, FieldsList = fieldsList };
                AmqpHelper.SendObject(request, "CalculationRequest", "CalculationRequest");
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

        #region Dispose
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
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
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (myQueue != null)
                    {
                        myQueue.Dispose();
                        myQueue = null;
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
        #endregion
    }
}
