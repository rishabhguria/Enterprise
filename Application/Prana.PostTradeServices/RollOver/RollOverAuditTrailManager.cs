using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.PostTradeServices.RollOver
{
    public class RollOverAuditTrailManager
    {
        List<TradeAuditEntry> _auditCollection = new List<TradeAuditEntry>();

        private static RollOverAuditTrailManager _instance = new RollOverAuditTrailManager();

        private RollOverAuditTrailManager()
        {
        }

        public static RollOverAuditTrailManager GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Add Audit Trail Collection
        /// </summary>
        /// <param name="orRequest"></param>
        /// <param name="action"></param>
        public void AddAuditTrailCollection(OrderSingle orRequest, TradeAuditActionType.ActionType action, int userId, string comment = null, string groupID = null, string taxlotId = null)
        {
            try
            {
                TradeAuditEntry audit = new TradeAuditEntry();
                audit.Action = action;
                audit.AUECLocalDate = DateTime.Now;
                audit.OriginalDate = orRequest.AUECLocalDate;
                audit.CompanyUserId = userId;
                audit.GroupID = string.Empty;
                audit.TaxLotID = string.Empty;
                audit.ParentClOrderID = orRequest.ParentClOrderID;
                audit.ClOrderID = orRequest.ClOrderID;
                audit.Symbol = orRequest.Symbol;
                audit.Level1ID = orRequest.Level1ID;
                audit.OrderSideTagValue = orRequest.OrderSideTagValue;

                if (!string.IsNullOrWhiteSpace(comment))
                {
                    audit.Comment = comment;
                }
                else
                {
                    TradeAuditActionTypeConverter ac = TypeDescriptor.GetConverter(typeof(TradeAuditActionType.ActionType)) as TradeAuditActionTypeConverter;
                    audit.Comment = (string)ac.ConvertTo(null, System.Globalization.CultureInfo.CurrentCulture, action, typeof(string));
                }

                audit.Source = TradeAuditActionType.ActionSource.Blotter;
                _auditCollection.Add(audit);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Save Audit Trail Data
        /// </summary>
        public void SaveAuditTrailData()
        {
            try
            {
                var IsAuditSaved = AuditManager.Instance.SaveAuditList(_auditCollection);
                if (IsAuditSaved)
                {
                    _auditCollection.Clear();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
