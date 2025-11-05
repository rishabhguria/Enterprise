using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Prana.LogManager;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface IPreTradeService : IServiceOnDemandStatus
    {
        event EventHandler<RuleCheckRecievedArguments> RuleCheckReceived;
        // TO-DO : once this becomes a service , RuleCheckReceived should be implemented as a callback method

        /// <summary>
        /// Occurs when [pending approvel request received].
        /// </summary>
        event EventHandler<EventArgs<List<PranaMessage>>> SendPendingApprovelNotificationEvent;

        /// <summary>
        /// Returns all orders present in the pre trade cache
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Dictionary<String, PranaMessage> GetAllCachedMessages();

        /// <summary>
        /// Send an order to pretrade service for validation
        /// </summary>
        /// <param name="pranaMessage"></param>
        [OperationContract]
        void ProcessOrder(PranaMessage pranaMessage);

        /// <summary>
        /// Add or update Cache.
        /// </summary>
        /// <param name="CLOrderID" name="OrderStatus"></param>
        [OperationContract]
        void AddOrUpdateStatusToOrderStatusTrackCache(String CLOrderID, ComplianceOrderStatus OrderStatus);

        /// <summary>
        /// Fetches status from Cache.
        /// </summary>
        /// <param name="ClOrderID"></param>
        [OperationContract]
        ComplianceOrderStatus GetOrderStatusFromOrderStatusTrackCache(string ClOrderID);

        /// <summary>
        /// Updates rule name
        /// </summary>
        /// <param name="pranaMessage"></param>
        [OperationContract]
        void UpdateRenamedRule(String oldRuleName, String newRuleName);

        /// <summary>
        /// Adds new rule
        /// </summary>
        /// <param name="pranaMessage"></param>
        [OperationContract]
        void AddRuleInCache(String addedRuleName);

        /// <summary>
        /// Inform the pretrade service that all orders for a multi trade were received
        /// </summary>
        /// <param name="multiTradeName"></param>
        /// <param name="userId"></param>
        /// <param name="noOfOrders"></param>
        [OperationContract]
        void InformAboutMultiTradeEOM(String multiTradeId, String userId, int noOfOrders);

        /// <summary>
        /// Override a trade on the server, without waiting for validation from compliance
        /// </summary>
        /// <param name="isAllowed"></param>
        /// <param name="orderId"></param>
        [OperationContract]
        void OverideTrade(bool isAllowed, String orderId);

        /// <summary>
        /// Send the simulation basket to esper and wait for the alerts
        /// </summary>
        /// <param name="pranaMessages"></param>
        /// <returns></returns>
        [OperationContract]
        SimulationResult SimulateTrades(List<OrderSingle> orderSingle, PreTradeType preTradeType, int companyUserID, bool isRealTimePositions, bool isComingFromRebalancer);

        /// <summary>
        /// Gets the taxlots for pre order.
        /// </summary>
        /// <param name="orderSingle">The order single.</param>
        /// <returns></returns>
        [OperationContract]
        List<TaxLot> GetTaxlotsForPreOrder(OrderSingle orderSingle, double orderQty);

        /// <summary>
        /// Send Working Qty trade to esper
        /// </summary>
        /// <param name="PranaMessage"></param> 
        [OperationContract]
        void SendInTradeToEsper(List<PranaMessage> list, bool isStartUpData);

        /// <summary>
        /// Synchrousnly return the requested caculateions after fetching the data from esper CEP engine
        /// </summary>
        /// <param name="compression"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable GetCalculationsFromEsper(Compression compression, List<String> fields);

        /// <summary>
        /// Save alerts in the Database and add to alert history grid
        /// </summary>
        /// <param name="alerts"></param>
        [OperationContract]
        void SendToNotificationManager(List<Alert> alerts);

        /// <summary>
        /// Subscribes Allocation service.
        /// </summary>
        void MakeProxy();

        /// <summary>
        /// Returns DataSet for Pending Approval UI
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<PreTradeApprovalInfo> GetPendingApprovalData();

        /// <summary>
        ///Pending Approval Alerts Approve/ Block
        /// </summary>
        /// <param name="alerts"></param>
        [OperationContract]
        PreTradeActionType ApproveBlockBtnClicked(List<Alert> alerts);

        /// <summary>
        /// Get pending approval order cache
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Dictionary<String, PranaMessage> GetPendingApprovalOrderCache();

        /// <summary>
        /// Remove In stage qty from compliance as User has removed order from Blotter UI
        /// </summary>
        /// <param name="listParentClOrderId"></param>
        [OperationContract]
        void HideOrderFromBlotter(List<string> listParentClOrderId);

        /// <summary>
        /// Returns all orders present in the pre trade cache after checking Compliance validation Timeout
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PranaMessage> GetComplianceCachedErrorOrders();

        /// <summary>
        /// Sending CashInFlow to Basket Compliance Service
        /// </summary>
        /// <param name="cashFlow"></param>
        [OperationContract]
        void SendCashInFlowToBasketComplianceService(List<CashFlowToCompliance> cashFlow);

        /// <summary>
        /// Sending updatedalert to trade server
        /// </summary>
        /// <param name="alerts"></param>
        [OperationContract]
        void UpdateAlerts(List<Alert> alerts, string basketId);

        /// <summary>
        /// Cancel an order to pretrade service 
        /// </summary>
        /// <param name="pranaMessage"></param>
        [OperationContract]
        void CancelPendingComplianceApprovalTrades(PranaMessage pranaMessage);

        /// <summary>
        /// Replace an order to pretrade service 
        /// </summary>
        /// <param name="pranaMessage"></param>
        [OperationContract]
        void UpdateReplaceOrderAlerts(PranaMessage pranaMessage);

        /// <summary>
        /// Freeze Unfreeze Pending Compliance Approval Trades
        /// </summary>
        /// <param name="pranaMessage"></param>
        [OperationContract]
        void FreezeUnfreezePendingComplianceApprovalTrades(PranaMessage pranaMessage);

        /// <summary>
        /// Replace multiple Pending Compliance Approval Trades
        /// </summary>
        /// <param name="pranaMessage"></param>
        [OperationContract]
        void UpdateMultipleReplaceOrderAlerts(List<OrderSingle> orders);

        /// <summary>
        /// Adds or Updates the Acknowledged Order Id for the Original Cl Order Id.
        /// </summary>
        /// <param name="clOrderId"></param>
        /// <param name="orgClOrderId"></param>
        [OperationContract]
        void AddOrUpdateAcknowledgeOrderId(string clOrderId, string orgClOrderId);

        /// <summary>
        /// Gets the Acknowledged ClOrder Id.
        /// </summary>
        /// <param name="orgClOrderId"></param>
        /// <returns></returns>
        [OperationContract]
        string GetAcknowledgedClOrderId(string orgClOrderId);
    }
}