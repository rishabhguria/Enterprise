using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Prana.TradeManager.Extension
{
    public class BlotterAuditTrailManager
    {
        List<TradeAuditEntry> _auditCollection = new List<TradeAuditEntry>();
        private Dictionary<string, PM_Taxlots_DeletedAudit> _deletedTaxlots = new Dictionary<string, PM_Taxlots_DeletedAudit>();

        object locker = new object();

        private static BlotterAuditTrailManager _instance = new BlotterAuditTrailManager();

        private BlotterAuditTrailManager()
        {
        }

        public static BlotterAuditTrailManager GetInstance()
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
                AuditManager.Instance.SaveAuditDeletedTaxlots(_deletedTaxlots);
                _deletedTaxlots.Clear();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the deleted taxlots from group to audit entry.
        /// </summary>
        /// <param name="taxlot">The taxlot.</param>
        /// <param name="orRequest">The or request.</param>
        /// <param name="userId">The user identifier.</param>
        public void AddDeletedTaxlotsFromGroupToAuditEntry(TaxLot taxlot, OrderSingle orRequest, int userId)
        {
            try
            {
                lock (locker)
                {
                    TradeAuditEntry newEntryTaxlot = new TradeAuditEntry();
                    newEntryTaxlot.Action = TradeAuditActionType.ActionType.REALLOCATE;
                    newEntryTaxlot.OriginalValue = taxlot.TaxLotQty.ToString();
                    newEntryTaxlot.NewValue = "0";
                    newEntryTaxlot.AUECLocalDate = DateTime.Now;
                    newEntryTaxlot.Comment = TradeAuditActionType.AllocationAuditComments.TaxlotDeleted.ToString();
                    newEntryTaxlot.CompanyUserId = userId;
                    newEntryTaxlot.GroupID = taxlot.GroupID;
                    newEntryTaxlot.ParentClOrderID = orRequest.ParentClOrderID;
                    newEntryTaxlot.ClOrderID = orRequest.ClOrderID;
                    newEntryTaxlot.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Blotter;
                    if (taxlot.TaxLotClosingId != null)
                        newEntryTaxlot.TaxLotClosingId = taxlot.TaxLotClosingId;
                    newEntryTaxlot.TaxLotID = taxlot.TaxLotID;
                    newEntryTaxlot.Level1AllocationID = taxlot.Level1AllocationID;
                    newEntryTaxlot.Symbol = taxlot.Symbol;
                    newEntryTaxlot.Level1ID = taxlot.Level1ID;
                    newEntryTaxlot.OrderSideTagValue = taxlot.OrderSideTagValue;

                    newEntryTaxlot.OriginalDate = taxlot.AUECLocalDate;

                    _auditCollection.Add(newEntryTaxlot);

                    AddTaxlotToDeletedCollection(taxlot);

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
        /// Adds all the taxlots in the group to the deleted Taxlots List
        /// </summary>
        /// <param name="tax">The taxlot class object</param>
        /// <param name="auditId">the auditID associated with deletion of taxlot</param>
        private void AddTaxlotToDeletedCollection(TaxLot tax)
        {
            try
            {
                if (!_deletedTaxlots.ContainsKey(tax.TaxLotID))
                {
                    _deletedTaxlots.Add(tax.TaxLotID, new PM_Taxlots_DeletedAudit(tax));
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
        /// Adds the taxlots from group to audit entry.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="orRequest">The or request.</param>
        /// <param name="userId">The user identifier.</param>
        /// <exception cref="System.NullReferenceException">The group object to add in audit dictionary is null</exception>
        public void AddTaxlotsFromGroupToAuditEntry(List<AllocationGroup> groups, OrderSingle orRequest, int userId, bool isGroupOrderReallocated = false)
        {
            try
            {
                if (groups == null)
                    throw new ArgumentNullException("The group object to add in audit dictionary is null");
                lock (locker)
                {
                    foreach (AllocationGroup group in groups)
                    {
                        foreach (TaxLot taxlot in group.TaxLots)
                        {
                            TradeAuditEntry newEntryTaxlot = new TradeAuditEntry();
                            newEntryTaxlot.Action = TradeAuditActionType.ActionType.REALLOCATE;
                            newEntryTaxlot.OriginalValue = "0";
                            newEntryTaxlot.NewValue = group.Quantity.ToString();
                            newEntryTaxlot.AUECLocalDate = DateTime.Now;
                            if (isGroupOrderReallocated)
                                newEntryTaxlot.Comment = EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupedOrderAllocatedFromBlotter);
                            else
                                newEntryTaxlot.Comment = EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TaxlotCreated);
                            newEntryTaxlot.CompanyUserId = userId;
                            newEntryTaxlot.GroupID = group.GroupID;
                            newEntryTaxlot.ParentClOrderID = orRequest.ParentClOrderID;
                            newEntryTaxlot.ClOrderID = orRequest.ClOrderID;
                            newEntryTaxlot.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Blotter;
                            if (taxlot.TaxLotClosingId != null)
                                newEntryTaxlot.TaxLotClosingId = taxlot.TaxLotClosingId;
                            newEntryTaxlot.TaxLotID = taxlot.TaxLotID;
                            newEntryTaxlot.Level1AllocationID = taxlot.Level1AllocationID;
                            newEntryTaxlot.Symbol = taxlot.Symbol;
                            newEntryTaxlot.Level1ID = taxlot.Level1ID;
                            newEntryTaxlot.OrderSideTagValue = taxlot.OrderSideTagValue;
                            newEntryTaxlot.OriginalDate = group.AUECLocalDate;
                            newEntryTaxlot.OriginalValue = "0";
                            if (group.AllocationSchemeName.Equals("Manual"))
                                newEntryTaxlot.NewValue = getTaxlotQuantity(taxlot);
                            else
                                newEntryTaxlot.NewValue = "Quantity " + getTaxlotQuantity(taxlot) + " Allocated using custom preference " + group.AllocationSchemeName;

                            _auditCollection.Add(newEntryTaxlot);
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
        }

        /// <summary>
        /// Gets the taxlot quantity.
        /// </summary>
        /// <param name="taxlot">The taxlot.</param>
        /// <returns></returns>
        private string getTaxlotQuantity(TaxLot taxlot)
        {
            try
            {
                double qty = taxlot.Quantity * taxlot.Percentage / 100;
                return qty.ToString();
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
            return string.Empty;
        }

    }
}
