using Prana.AmqpAdapter.Amqp;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.ServiceConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.ComplianceAlertPopup
{
    public class ComplianceCommon
    {
        public static bool IsDisableRequireOnTT { get; set; }

        /// <summary>
        /// Uses pretrade service and user input tp validate a staged order
        /// Vadidates order before staging
        /// </summary>
        /// <param name="os">The os.</param>
        /// <returns></returns>
        public static bool ValidateOrderInCompliance_New(List<OrderSingle> orders, Form form, int companyUserID, bool isOnlyHardAlerts = false, bool isRealTimePositions = true, bool isComingFromRebalancer = false)
        {
            try
            {
                bool validation = false;
                SimulationResult result = ComplianceServiceConnector.GetInstance().SimulateTrade(orders, PreTradeType.Stage, companyUserID, isRealTimePositions, isComingFromRebalancer);
                List<Alert> alerts = result.Alerts;
                ComplianceAlertPopUp popUp = new ComplianceAlertPopUp();
                if (result.Allowed)
                {
                    if (alerts.Count == 0)
                    {
                        validation = true;
                        alerts.ForEach(x => x.Status = ComplainceConstants.MSG_STAGING_ALLOWED);
                        var response = new { Response = new { IsAllowed = false, OrderId = result.SimulationId, UserId = orders[0].CompanyUserID, isApprovalRequired = false } };
                        AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                    }
                    else
                    {
                        if (result.OverrideType == RuleOverrideType.Soft)
                        {
                            popUp.BindingComplianceAlertData(AlertPopUpType.Override, alerts);
                            popUp.ShowDialog(form ?? null);
                            popUp.Activate();
                            if (popUp.IsTradeAllowed)
                                alerts.ForEach(x => x.Status = ComplainceConstants.MSG_STAGING_ALLOWED_RULE_OVERRIDEN);
                            else
                                alerts.ForEach(x => x.Status = ComplainceConstants.MSG_STAGING_BLOCKED);
                            validation = popUp.IsTradeAllowed;
                            ComplianceServiceConnector.GetInstance().UpdateAlerts(popUp.GetUpdatedAlerts(), alerts[0].OrderId);
                            var response = new { Response = new { IsAllowed = popUp.IsTradeAllowed, OrderId = alerts[0].OrderId, UserId = orders[0].CompanyUserID, isApprovalRequired = false, popUpType = AlertPopUpType.Override } };
                            AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                        }
                        else if (result.OverrideType == RuleOverrideType.RequiresApproval)
                        {
                            popUp.BindingComplianceAlertData(AlertPopUpType.PendingApproval, alerts);
                            popUp.ShowDialog(form ?? null);
                            popUp.Activate();
                            validation = false;
                            ComplianceServiceConnector.GetInstance().UpdateAlerts(popUp.GetUpdatedAlerts(), alerts[0].OrderId);
                            var response = new { Response = new { IsAllowed = popUp.IsTradeAllowed, OrderId = alerts[0].OrderId, UserId = orders[0].CompanyUserID, isApprovalRequired = popUp.IsTradeAllowed, popUpType = AlertPopUpType.PendingApproval } };

                            if (popUp.IsTradeAllowed)
                                IsDisableRequireOnTT = true;

                            AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                        }
                    }
                }
                else
                {
                    if (result.Alerts.Count == 1 && result.Alerts[0].AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID))
                    {
                        if (orders.Count == 1)
                            result.Alerts[0].Description = result.Alerts[0].Description + TagDatabaseManager.GetInstance.GetOrderSideText(orders[0].OrderSideTagValue) + " " + orders[0].Symbol + " " + orders[0].Quantity + " @" + orders[0].AvgPriceForCompliance + " for " + TagDatabaseManager.GetInstance.GetOrderTypeText(orders[0].OrderTypeTagValue) + " Order";
                        else
                            result.Alerts[0].Description = result.Alerts[0].Description + ComplainceConstants.CONST_MULTIPLE_ORDER;
                        if (result.OverrideType == RuleOverrideType.Hard)
                        {
                            validation = false;
                            popUp.BindingComplianceAlertData(AlertPopUpType.Inform, result.Alerts);
                            popUp.ShowDialog(form);
                            popUp.Activate();
                        }
                        else if (result.OverrideType == RuleOverrideType.Soft)
                        {
                            popUp.BindingComplianceAlertData(AlertPopUpType.Override, result.Alerts);
                            popUp.ShowDialog(form);
                            popUp.Activate();
                            validation = popUp.IsTradeAllowed;
                        }
                        var response = new { Response = new { IsAllowed = validation, OrderId = alerts[0].OrderId, UserId = orders[0].CompanyUserID, isApprovalRequired = false, popUpType = AlertPopUpType.Inform } };
                        AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                    }
                    else
                    {
                        if (result.OverrideType == RuleOverrideType.Hard)
                        {
                            popUp.BindingComplianceAlertData(AlertPopUpType.Inform, alerts.Where(x => x.Blocked).ToList(), isOnlyHardAlerts);
                            popUp.ShowDialog(form);
                            popUp.Activate();
                        }
                        validation = false;
                        alerts.ForEach(x => x.Status = ComplainceConstants.MSG_STAGING_BLOCKED);
                        ComplianceServiceConnector.GetInstance().UpdateAlerts(popUp.GetUpdatedAlerts(), alerts[0].OrderId);
                        var response = new { Response = new { IsAllowed = validation, OrderId = alerts[0].OrderId, UserId = orders[0].CompanyUserID, isApprovalRequired = false, popUpType = AlertPopUpType.Inform } };
                        AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                    }
                }
                popUp.Dispose();
                //ComplianceServiceConnector.GetInstance().SaveAlertsToHistory(alerts);
                return validation;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        /// <summary>
        /// Sends the List of orders for compliance check.
        /// </summary>
        /// <param name="orderList"></param>
        /// <param name="companyUserID"></param>
        /// <returns></returns>
        public static dynamic CheckCompliance(List<OrderSingle> orderList, int companyUserID, bool isRealTimePositions = true, bool isComingFromRebalancer = false)
        {
            dynamic complianceCheckResult = true;
            try
            {
                SimulationResult result = ComplianceServiceConnector.GetInstance().SimulateTrade(orderList, PreTradeType.ComplianceCheck, companyUserID, isRealTimePositions, isComingFromRebalancer);
                if (result != null)
                {
                    List<Alert> alerts = result.Alerts;
                    if (alerts.Count == 0)
                        complianceCheckResult = false;
                    else
                    {
                        if (result.Alerts.Count == 1 && result.Alerts[0].AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID))
                        {
                            if (orderList.Count == 1)
                                result.Alerts[0].Description = result.Alerts[0].Description + TagDatabaseManager.GetInstance.GetOrderSideText(orderList[0].OrderSideTagValue) + " " + orderList[0].Symbol + " " + orderList[0].Quantity + " @" + orderList[0].AvgPriceForCompliance + " for " + TagDatabaseManager.GetInstance.GetOrderTypeText(orderList[0].OrderTypeTagValue) + " Order";
                            else
                                result.Alerts[0].Description = result.Alerts[0].Description + ComplainceConstants.CONST_MULTIPLE_ORDER;
                        }
                        ComplianceAlertPopUp popUp = new ComplianceAlertPopUp();
                        popUp.BindingComplianceAlertData(AlertPopUpType.ComplianceCheck, alerts);
                        popUp.ShowDialog();
                        popUp.Activate();
                        popUp.Dispose();

                        if (result.Alerts[0].AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID))
                            complianceCheckResult = PreTradeConstants.CONST_FAILED_ALERT_ID;
                    }
                    var response = new { Response = new { IsAllowed = false, OrderId = result.SimulationId, UserId = orderList[0].CompanyUserID, isApprovalRequired = false, popUpType = AlertPopUpType.ComplianceCheck } };
                    AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                }
                else
                {
                    complianceCheckResult = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return complianceCheckResult;
        }

        /// <summary>
        /// Sends the List of orders for compliance check.
        /// </summary>
        /// <param name="orderList"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static dynamic CheckComplianceForRebalancer(List<OrderSingle> orderList, SimulationResult result)
        {
            dynamic complianceCheckResult = true;
            try
            {
                if (result != null)
                {
                    List<Alert> alerts = result.Alerts;
                    if (alerts.Count == 0)
                        complianceCheckResult = false;
                    else
                    {
                        if (result.Alerts.Count == 1 && result.Alerts[0].AlertId.Equals(PreTradeConstants.CONST_FAILED_ALERT_ID))
                        {
                            if (orderList.Count == 1)
                                result.Alerts[0].Description = result.Alerts[0].Description + TagDatabaseManager.GetInstance.GetOrderSideText(orderList[0].OrderSideTagValue) + " " + orderList[0].Symbol + " " + orderList[0].Quantity + " @" + orderList[0].AvgPriceForCompliance + " for " + TagDatabaseManager.GetInstance.GetOrderTypeText(orderList[0].OrderTypeTagValue) + " Order";
                            else
                                result.Alerts[0].Description = result.Alerts[0].Description + ComplainceConstants.CONST_MULTIPLE_ORDER;
                        }
                    }
                    var response = new { Response = new { IsAllowed = false, OrderId = result.SimulationId, UserId = orderList[0].CompanyUserID, isApprovalRequired = false, popUpType = AlertPopUpType.None } };
                    Logger.LoggerWrite(string.Format("Response: {0} - Alerts count: {1}", response, result.Alerts.Count), LoggingConstants.CATEGORY_GENERAL_COMPLIANCE);
                    AmqpHelper.SendObject(response, PreTradeConstants.Const_OverrideResponse, null);
                }
                else
                {
                    complianceCheckResult = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return complianceCheckResult;
        }

        /// <summary>
        /// Send account wise cash amount for Rebalancer
        /// </summary>
        /// <param name="accountIds"></param>
        public static void SendCashAmountForAccountsFromRebalancer(List<CashFlowToCompliance> cashFlow)
        {
            try
            {
                ComplianceServiceConnector.GetInstance().SendCashInFlowToBasketComplianceService(cashFlow);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Update Multiple Replace Order Alerts 
        /// </summary>
        /// <param name="orders"></param>
        public static void UpdateMultipleReplaceOrderAlerts(List<OrderSingle> orders)
        {
            try
            {
                ComplianceServiceConnector.GetInstance().UpdateMultipleReplaceOrderAlerts(orders);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}
