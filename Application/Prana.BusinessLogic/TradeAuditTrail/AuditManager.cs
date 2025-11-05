using Prana.BusinessLogic.TradeAuditTrail;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.TradeAudit;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessLogic
{
    /// <summary>
    /// The MAnager class used for all the Audit Trail related function
    /// </summary>
    public sealed class AuditManager
    {
        private static volatile AuditManager instance;
        /// <summary>
        /// lock is applied to this variable to ensure singleton behaviour while using multiple threads
        /// </summary>
        private static readonly object syncRoot = new Object();

        List<TradeAuditEntry> _tradeAuditCollection = new List<TradeAuditEntry>();

        /// <summary>
        /// Stores all the unallocated groups which have been deleted
        /// </summary>
        private Dictionary<string, T_Group_DeletedAudit> _deletedGroups = new Dictionary<string, T_Group_DeletedAudit>();

        /// <summary>
        /// Stores all the groups whose all the taxlots have been unallocated or deleted
        /// </summary>
        private Dictionary<string, PM_Taxlots_DeletedAudit> _deletedTaxlots = new Dictionary<string, PM_Taxlots_DeletedAudit>();

        /// <summary>
        /// Stores all the taxlots which are of type swap
        /// </summary>
        private Dictionary<string, SwapParameters> _deletedSwaps = new Dictionary<string, SwapParameters>();

        /// <summary>
        /// Constructor for the singleton Audit Trail class. Makes a Proxy for the AuditTrailService
        /// </summary>
        private AuditManager()
        {
            try
            {
                _auditDataManager = AuditDataManager.Instance;
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
        /// returns the instance, if null creates one
        /// </summary>
        public static AuditManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AuditManager();
                    }
                }
                return instance;
            }
        }

        private AuditDataManager _auditDataManager = null;

        readonly object locker = new object();
        readonly object lockerr = new object();
        /// <summary>
        /// clears the audit List which stores the entries to be saved by the user. Is saved on the click of save button on allocation.
        /// </summary>
        /// <returns></returns>
        public bool clearAuditListAndDeletedList()
        {
            try
            {
                lock (locker)
                {
                    _tradeAuditCollection.Clear();
                    _deletedGroups.Clear();
                    _deletedTaxlots.Clear();
                    _deletedSwaps.Clear();
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
            return true;
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
        /// Adds the Allocation group to the deleted Groups Collection
        /// </summary>
        /// <param name="group"></param>
        private void AddGroupToDeletedCollection(AllocationGroup group)
        {
            try
            {
                lock (locker)
                {
                    if (!_deletedGroups.ContainsKey(group.GroupID))
                    {
                        _deletedGroups.Add(group.GroupID, new T_Group_DeletedAudit(group));
                    }
                    if (group.IsSwapped)
                    {
                        if (!_deletedSwaps.ContainsKey(group.GroupID))
                        {
                            SwapParameters swapClone = group.SwapParameters.Clone();
                            _deletedSwaps.Add(group.GroupID, swapClone);
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
        /// Adds only the group's entry to the Audit List, no taxlot entries are made
        /// </summary>
        /// <param name="group">Not Null, the group from which the data has to be extracted</param>
        /// <param name="auecLocalDate">Not Null, Date time to be used while saving the entry</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="originalValue">the value in case of edited trades</param>
        /// <param name="comment">Not Null, comment of the change by the user</param>
        /// <returns></returns>
        public bool AddGroupToAuditEntry(AllocationGroup group, bool addGroupToDeletedAuditList, Prana.BusinessObjects.TradeAuditActionType.ActionType action, string originalValue, string newValue, string comment, int currentUserID)
        {
            TradeAuditEntry newEntry = new TradeAuditEntry();
            try
            {
                if (originalValue != null && comment != null && group != null)
                {
                    newEntry.Action = action;
                    newEntry.OriginalValue = originalValue;
                    newEntry.NewValue = newValue;
                    newEntry.AUECLocalDate = DateTime.Now;
                    newEntry.Comment = GetCommentForGroups(comment);
                    newEntry.CompanyUserId = currentUserID;
                    newEntry.GroupID = group.GroupID;
                    newEntry.TaxLotClosingId = "";
                    newEntry.TaxLotID = "";
                    newEntry.Symbol = group.Symbol;
                    newEntry.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Allocation;
                    if (group.TaxLots.Count == 0 || (action.Equals(TradeAuditActionType.ActionType.UNALLOCATE) && comment.Equals(TradeAuditActionType.AllocationAuditComments.GroupCreated.ToString())))
                        newEntry.Level1ID = int.MinValue;
                    else
                        newEntry.Level1ID = group.TaxLots.Count == 1 ? group.TaxLots[0].Level1ID : int.MaxValue;
                    newEntry.OrderSideTagValue = group.OrderSideTagValue;
                    if (action == Prana.BusinessObjects.TradeAuditActionType.ActionType.TradeDate_Changed)
                    {
                        newEntry.OriginalDate = Convert.ToDateTime(originalValue);
                    }
                    else
                    {
                        newEntry.OriginalDate = group.AUECLocalDate;
                    }
                    lock (locker)
                    {
                        _tradeAuditCollection.Add(newEntry);
                    }
                    if (addGroupToDeletedAuditList)
                    {
                        AddGroupToDeletedCollection(group);
                    }
                }
                else
                    throw new NullReferenceException("The group object to add in audit dictionary is null");
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
            return true;
        }

        private static string GetCommentForGroups(string comment)
        {
            try
            {
                if (comment.Equals(TradeAuditActionType.AllocationAuditComments.UnallocatedAutomated.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.UnallocatedAutomated);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.GroupCreatedForTrade.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupCreatedForTrade);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.GroupCreated.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupCreated);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.GroupCreatedAfterOptionExercise.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupCreatedAfterOptionExercise);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.GroupCreatedAfterOptionExpire.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupCreatedAfterOptionExpire);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.GroupDeletedAfterUnwindingOptionExercise.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupDeletedAfterUnwindingOptionExercise);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.GroupDeletedAfterUnwindingOptionExpire.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupDeletedAfterUnwindingOptionExpire);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.GroupDeleted.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupDeleted);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TradesGroupedCreated.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TradesGroupedCreated);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TradesGroupedDeleted.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TradesGroupedDeleted);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.UngroupedGroupsCreated.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.UngroupedGroupsCreated);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.GroupsUngroupedDeleted.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupsUngroupedDeleted);

                else
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.GroupModified);
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

        private static string GetCommentForTaxlots(string comment)
        {
            try
            {
                if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotCreatedForTrade.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TaxlotCreatedForTrade);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotCreated.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TaxlotCreated);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotCreatedAfterOptionExercise.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TaxlotCreatedAfterOptionExercise);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotCreatedAfterOptionExpire.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TaxlotCreatedAfterOptionExpire);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotDeletedAfterUnwindingOptionExpire.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TaxlotDeletedAfterUnwindingOptionExpire);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotDeletedAfterUnwindingOptionExercise.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TaxlotDeletedAfterUnwindingOptionExercise);
                else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotDeleted.ToString()))
                    return EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TaxlotDeleted);
                else
                    return comment;
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


        /// <summary>
        /// Adds the entry for all taxlots in the group to the Audit Entry list. The group is not created as an entry
        /// </summary>
        /// <param name="group">the Allocation group from which the taxlots have to be extracted</param>
        /// <param name="addTaxlotsToDeletedAuditList">whether to add the specified talot to deleted list</param>
        /// <param name="auecLocalDate">The Date and Time which have to be used as the time of change</param>
        /// <param name="action">The TradeAuditActionType performed by the user</param>
        /// <param name="originalValue">The value of the change in case of edit action</param>
        /// <param name="comment">The comment by the user for the change performed</param>
        /// <returns></returns>
        public bool AddTaxlotsFromGroupToAuditEntry(AllocationGroup group, bool addTaxlotsToDeletedAuditList, Prana.BusinessObjects.TradeAuditActionType.ActionType action, string originalValue, string newValue, string comment, int currentUserID)
        {
            try
            {
                if (originalValue != null && comment != null && group != null)
                {
                    lock (locker)
                    {
                        foreach (TaxLot taxlot in group.TaxLots)
                        {
                            TradeAuditEntry newEntryTaxlot = new TradeAuditEntry();
                            newEntryTaxlot.Action = action;
                            newEntryTaxlot.OriginalValue = originalValue;
                            newEntryTaxlot.NewValue = newValue;
                            newEntryTaxlot.AUECLocalDate = DateTime.Now;
                            newEntryTaxlot.Comment = GetCommentForTaxlots(comment);
                            newEntryTaxlot.CompanyUserId = currentUserID;
                            newEntryTaxlot.GroupID = group.GroupID;
                            newEntryTaxlot.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Allocation;
                            if (taxlot.TaxLotClosingId != null)
                                newEntryTaxlot.TaxLotClosingId = taxlot.TaxLotClosingId;
                            newEntryTaxlot.TaxLotID = taxlot.TaxLotID;
                            newEntryTaxlot.Level1AllocationID = taxlot.Level1AllocationID;
                            newEntryTaxlot.Symbol = taxlot.Symbol;
                            newEntryTaxlot.Level1ID = taxlot.Level1ID;
                            newEntryTaxlot.OrderSideTagValue = taxlot.OrderSideTagValue;
                            if (action == Prana.BusinessObjects.TradeAuditActionType.ActionType.TradeDate_Changed)
                            {
                                newEntryTaxlot.OriginalDate = Convert.ToDateTime(originalValue);
                            }
                            else
                            {
                                newEntryTaxlot.OriginalDate = group.AUECLocalDate;
                            }

                            if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotCreatedForTrade.ToString())
                                || comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotCreated.ToString())
                                || comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotCreatedAfterOptionExercise.ToString())
                                || comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotCreatedAfterOptionExpire.ToString()))
                            {
                                newEntryTaxlot.OriginalValue = "0";
                                if (group.AllocationSchemeName.Equals("Manual"))
                                    newEntryTaxlot.NewValue = getTaxlotQuantity(taxlot);
                                else
                                    newEntryTaxlot.NewValue = "Quantity " + getTaxlotQuantity(taxlot) + " Allocated using custom preference " + group.AllocationSchemeName;
                            }
                            else if (comment.Equals(TradeAuditActionType.AllocationAuditComments.TaxlotDeleted.ToString()))
                            {
                                newEntryTaxlot.OriginalValue = taxlot.TaxLotQty.ToString(); ;
                            }
                            _tradeAuditCollection.Add(newEntryTaxlot);
                            if (addTaxlotsToDeletedAuditList)
                            {
                                AddTaxlotToDeletedCollection(taxlot);
                            }
                        }
                    }
                }
                else
                    throw new NullReferenceException("The group object to add in audit dictionary is null");
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
            return true;
        }

        private static string getTaxlotQuantity(TaxLot taxlot)
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

        /// <summary>
        /// Add Deleted Taxlots From Group To AuditEntry
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="auecLocalDate"></param>
        /// <param name="taxlot"></param>
        /// <param name="originalValue"></param>
        /// <param name="comment"></param>
        /// <param name="_userId"></param>
        /// <returns></returns>
        public bool AddDeletedTaxlotsFromGroupToAuditEntry(string groupId, TaxLot taxlot, string originalValue, string newValue, string comment, int _userId)
        {
            try
            {
                if (originalValue != null && comment != null)
                {
                    lock (locker)
                    {

                        TradeAuditEntry newEntryTaxlot = new TradeAuditEntry();
                        newEntryTaxlot.Action = TradeAuditActionType.ActionType.UNALLOCATE;
                        newEntryTaxlot.OriginalValue = originalValue;
                        newEntryTaxlot.NewValue = newValue;
                        newEntryTaxlot.AUECLocalDate = DateTime.Now;
                        newEntryTaxlot.Comment = comment;
                        newEntryTaxlot.CompanyUserId = _userId;
                        newEntryTaxlot.GroupID = groupId;
                        newEntryTaxlot.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Allocation;
                        if (taxlot.TaxLotClosingId != null)
                            newEntryTaxlot.TaxLotClosingId = taxlot.TaxLotClosingId;
                        newEntryTaxlot.TaxLotID = taxlot.TaxLotID;
                        newEntryTaxlot.Level1AllocationID = taxlot.Level1AllocationID;
                        newEntryTaxlot.Symbol = taxlot.Symbol;
                        newEntryTaxlot.Level1ID = taxlot.Level1ID;
                        newEntryTaxlot.OrderSideTagValue = taxlot.OrderSideTagValue;

                        newEntryTaxlot.OriginalDate = taxlot.AUECLocalDate;

                        _tradeAuditCollection.Add(newEntryTaxlot);

                        AddTaxlotToDeletedCollection(taxlot);

                    }

                }
                else
                    throw new NullReferenceException("The group object to add in audit dictionary is null");
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
            return true;
        }

        /// <summary>
        /// Adds a row to the audit List with the specified values
        /// </summary>
        /// <param name="taxlot">the Taxlot from which the data have to be extracted</param>
        /// <param name="auecLocalDate">The date time at which the change was performed</param>
        /// <param name="groupID">The group id of the trade</param>
        /// <param name="taxlotID">The taxlot id of the trade if allocated</param>
        /// <param name="taxLotClosingId">The taxlot closing id of the trade if closed</param>
        /// <param name="action">The TradeAuditActionType performed by the user</param>
        /// <param name="originalValue">The old value of the change when action is edited</param>
        /// <param name="comment">The comment by the user for the change</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        public bool AddRowTradeAuditEntry(TaxLot taxlot, string groupID, Prana.BusinessObjects.TradeAuditActionType.ActionType action, string originalValue, string newValue, int companyUserId)
        {
            TradeAuditEntry newEntry = new TradeAuditEntry();
            try
            {
                if (groupID != null)
                {
                    newEntry.Action = action;
                    newEntry.OriginalValue = originalValue;
                    newEntry.NewValue = newValue;
                    newEntry.AUECLocalDate = DateTime.Now;
                    //newEntry.Comment = action.ToString();
                    newEntry.Comment = EnumHelper.GetDescription(TradeAuditActionType.AllocationAuditComments.TaxlotModified);
                    newEntry.CompanyUserId = companyUserId;
                    newEntry.GroupID = groupID;
                    newEntry.TaxLotClosingId = taxlot.TaxLotClosingId;
                    newEntry.TaxLotID = taxlot.TaxLotID;
                    newEntry.Level1AllocationID = taxlot.Level1AllocationID;
                    newEntry.Symbol = taxlot.Symbol;
                    newEntry.Level1ID = taxlot.Level1ID;
                    newEntry.OrderSideTagValue = taxlot.OrderSideTagValue;
                    newEntry.Source = TradeAuditActionType.ActionSource.Allocation;
                    if (action == Prana.BusinessObjects.TradeAuditActionType.ActionType.TradeDate_Changed)
                    {
                        newEntry.OriginalDate = Convert.ToDateTime(originalValue);
                    }
                    else
                    {
                        newEntry.OriginalDate = taxlot.AUECLocalDate;
                    }
                    lock (locker)
                    {
                        _tradeAuditCollection.Add(newEntry);
                    }
                }
                else
                    throw new NullReferenceException("The group object to add in audit dictionary is null");
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
            return true;
        }

        /// <summary>
        /// Adds entry to the Audit List for the Allocated data Traded in some back date
        /// </summary>
        /// <param name="order">Not Null, Order from which the data has to be extracted</param>
        /// <param name="action">TradeAuditActionType </param>
        /// <param name="comment">Not Null, comment of the action by the user</param>
        /// <param name="companyUserId">The company user id of the user doing the changes</param>
        /// <returns></returns>
        public bool AddAllocatedDataAuditEntry(AllocationGroup group, TradeAuditActionType.ActionType action, string comment)
        {
            TradeAuditEntry newEntry = new TradeAuditEntry();
            try
            {
                if (group != null && comment != null)
                {
                    //  newEntry.AuditId = GetAuditId();
                    newEntry.Action = action;
                    newEntry.AUECLocalDate = DateTime.Now;
                    newEntry.OriginalDate = DateTime.Parse(group.OriginalPurchaseDate.ToString());
                    newEntry.Comment = comment;
                    newEntry.CompanyUserId = group.CompanyUserID;
                    newEntry.Symbol = group.Symbol;
                    newEntry.Level1ID = group.AccountID;
                    newEntry.GroupID = group.GroupID;
                    newEntry.TaxLotClosingId = group.TaxLotClosingId;
                    newEntry.TaxLotID = group.TaxLotIdsWithAttributes;
                    newEntry.OrderSideTagValue = group.OrderSideTagValue;
                    newEntry.OriginalValue = "";
                    newEntry.NewValue = "";
                    newEntry.Source = Prana.BusinessObjects.TradeAuditActionType.ActionSource.Allocation;
                    lock (locker)
                    {
                        _tradeAuditCollection.Add(newEntry);
                    }
                }
                else
                    throw new NullReferenceException("The Data Table to add in audit dictionary is null");
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
            return true;
        }

        /// <summary>
        /// Gets ignored users for audit trail from db for the current user
        /// </summary>
        /// <param name="companyId">company Id of current user</param>
        /// <param name="companyUserId">company userid of the current user</param>
        /// <returns></returns>
        public string GetIgnoredUserIds(int companyId, int companyUserId)
        {
            string ignored = "";
            try
            {
                ignored = _auditDataManager.GetIgnoredUserIds(companyId, companyUserId);
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
            return ignored;
        }

        /// <summary>
        /// generic function to convert a values collection of dictionary in to a list of the value collection
        /// </summary>
        /// <typeparam name="K">the key type of the dictionary</typeparam>
        /// <typeparam name="V">the value type of the dictionary</typeparam>
        /// <param name="valueCollection">the value collection of the dictionary</param>
        /// <returns>list of values of the dictionary</returns>
        private List<V> DictionariesToList<K, V>(Dictionary<K, V>.ValueCollection valueCollection)
        {
            List<V> list = new List<V>();
            try
            {
                list.AddRange(valueCollection);
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
            return list;
        }

        /// <summary>
        /// the function to save the audit list data to the DB, clears the lists after that
        /// </summary>
        /// <returns></returns>
        public bool SaveAuditList()
        {
            try
            {
                lock (locker)
                {
                    if (_tradeAuditCollection.Count > 0)
                    {
                        _auditDataManager.SaveAuditList(_tradeAuditCollection);
                    }
                    if (_deletedTaxlots.Count > 0)
                    {
                        _auditDataManager.SaveAuditDeletedTaxlots(DictionariesToList<string, PM_Taxlots_DeletedAudit>(_deletedTaxlots.Values));
                    }
                    if (_deletedGroups.Count > 0)
                    {
                        _auditDataManager.SaveAuditDeletedGroups(DictionariesToList<string, T_Group_DeletedAudit>(_deletedGroups.Values));
                    }
                    if (_deletedSwaps.Count > 0)
                    {
                        _auditDataManager.SaveAuditDeletedSwap(DictionariesToList<string, SwapParameters>(_deletedSwaps.Values));
                    }
                }
                clearAuditListAndDeletedList();
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
            return true;
        }

        /// <summary>
        /// the function to save the audit list data to the DB, clears the lists after that
        /// </summary>
        /// <returns></returns>
        public bool SaveAuditList(List<TradeAuditEntry> tradeAuditCollection)
        {
            try
            {
                lock (lockerr)
                {
                    if (tradeAuditCollection.Count > 0)
                    {
                        _auditDataManager.SaveAuditList(tradeAuditCollection);
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
            return true;
        }

        /// <summary>
        /// the function to save the audit list data to the DB, clears the lists after that
        /// </summary>
        /// <returns></returns>
        public bool SaveAuditDeletedTaxlots(Dictionary<string, PM_Taxlots_DeletedAudit> deletedTaxlots)
        {
            try
            {
                lock (lockerr)
                {
                    if (deletedTaxlots.Count > 0)
                    {
                        _auditDataManager.SaveAuditDeletedTaxlots(DictionariesToList<string, PM_Taxlots_DeletedAudit>(deletedTaxlots.Values));
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
            return true;
        }

        /// <summary>
        /// the function to save the audit list data to the DB specifically for Daily
        /// Valuation Data (i.e. MarkPrice and Fx-Rate), clears the lists after that
        /// </summary>
        /// <returns></returns>
        public bool SaveAuditListForDailyValuation(List<TradeAuditEntry> tradeAuditCollection)
        {
            try
            {
                lock (lockerr)
                {
                    if (tradeAuditCollection.Count > 0)
                    {
                        _auditDataManager.SaveAuditListForDailyValuation(tradeAuditCollection);
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
            return true;
        }

        /// <summary>
        /// the function to save the audit list data to the DB specifically for Daily
        /// Valuation Data (i.e. MarkPrice and Fx-Rate), clears the lists after that
        /// </summary>
        /// <returns></returns>
        public bool SaveAuditListForCashJournal(List<CashJournalAuditEntry> cashAuditCollection)
        {
            try
            {
                lock (lockerr)
                {
                    if (cashAuditCollection.Count > 0)
                    {
                        _auditDataManager.SaveAuditListForCashJournal(cashAuditCollection);
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
            return true;
        }

        /// <summary>
        /// Adds the specified action to all the taxlots and the group itself in the tradeactions list
        /// </summary>
        /// <param name="group">allocation group</param>
        /// <param name="action">action of type TradeAuditActionType.ActionType</param>
        public void AddActionToAllGroupAndTaxlots(AllocationGroup group, TradeAuditActionType.ActionType action)
        {
            try
            {
                group.AddTradeAction(action);
                foreach (TaxLot tax in group.TaxLots)
                {
                    tax.AddTradeAction(action);
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
        /// Removes all the Audit entries from the list where action is deleted
        /// </summary>
        public void RemoveAllDeletedAuditTrail()
        {
            try
            {
                _tradeAuditCollection.RemoveAll(TradeAuditEntry.IsActionDeleted);
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
        /// Removes all the Audit Entries from the Audit List where action is edited
        /// </summary>
        public void RemoveAllEdited()
        {
            try
            {
                lock (locker)
                {
                    _tradeAuditCollection.RemoveAll(TradeAuditEntry.IsActionEdited);
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
        /// Gets the audit datatable using audit trail service
        /// </summary>
        /// <param name="groupIds"></param>
        /// <param name="ignoredUser"></param>
        /// <param name="accountIdsCommaSeparated"></param>
        /// <returns></returns>
        public DataTable GetAuditDataByGroupIds(List<string> groupIds, string ignoredUser, string accountIdsCommaSeparated)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = _auditDataManager.GetAuditDataByGroupIds(groupIds, ignoredUser, accountIdsCommaSeparated);
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
            return dt;
        }

        /// <summary>
        /// gets the audit data from db using audit service for filters on auditUI
        /// </summary>
        /// <param name="from">fram date</param>
        /// <param name="till">till date</param>
        /// <param name="symbol">symbols comma separated</param>
        /// <param name="accountIds">accountids comma separated</param>
        /// <param name="orderSideTagValues">comma separated</param>
        /// <returns></returns>
        public DataTable GetAuditUIDataByDate(AuditTrailFilterParams auditTrailFilterParams)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = _auditDataManager.GetAuditUIDataByDate(auditTrailFilterParams);
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
            return dt;
        }

        public DataTable GetGroupTaxlotForIds(string groupId, string taxlotId)
        {
            return _auditDataManager.GetGroupTaxlotForIds(groupId, taxlotId);
        }

        public DataTable GetOrderDetailsByIds(string parentOrderID, string clOrderId)
        {
            return _auditDataManager.GetOrderDetailsByIds(parentOrderID, clOrderId);
        }
    }
}
