using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for AllocationGroup.
    /// </summary>
    [Serializable]
    public class AllocationGroup : PranaBasicMessage, IKeyable, INotifyPropertyChangedCustom, IFilterable
    {

        #region Private Members

        bool _isPreAllocated = false;
        private string _groupID;
        //To be used only for Virtual Groups generated MasterFund Allocation 
        private List<string> _originalGroupIDs = null;
        private string _listID = string.Empty;
        private bool _autoGrouped = false;
        private bool _updated = false;
        private bool _isProrataActive = false;
        private AllocationLevelList _level1Collection = new AllocationLevelList();
        private double _allocatedQty = 0.0;
        private PostTradeConstants.ORDERSTATE_ALLOCATION _state = PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED;
        private bool _isManualGroup = false;
        private bool _isOverbuyOversellAccepted = false;
        private ApplicationConstants.PersistenceStatus _persistenceStatus = ApplicationConstants.PersistenceStatus.New;
        private DateTime _allocationDate = DateTimeConstants.MinValue;

        private double _commission = 0;
        private double _softCommission = 0;
        private double _otherBrokerfees = 0;
        private double _clearingBrokerFee = 0;

        [XmlIgnore]
        private bool _commissionCalculationTime = false;
        private bool _isCommissionCalculated = false;
        private string _commissionText = "Calculated";
        private bool _isCommissionChanged = false;

        private bool _isSoftCommissionChanged = false;
        [Browsable(false)]
        public virtual bool IsCommissionChanged
        {
            get { return _isCommissionChanged; }
            set { _isCommissionChanged = value; }
        }


        /// <summary>
        /// The is manually ungrouped from Allocation UI
        /// </summary>
        private bool _isManuallyModified = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is manually ungrouped from Allocation UI.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is manually ungrouped from Allocation UI; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        public virtual bool IsManuallyModified
        {
            get { return _isManuallyModified; }
            set { _isManuallyModified = value; }
        }

        [Browsable(false)]
        public virtual bool IsSoftCommissionChanged
        {
            get { return _isSoftCommissionChanged; }
            set { _isSoftCommissionChanged = value; }
        }

        private bool _isRecalculateCommission = false;
        [Browsable(false)]
        public virtual bool IsRecalculateCommission
        {
            get { return _isRecalculateCommission; }
            set { _isRecalculateCommission = value; }
        }

        /// <summary>
        /// TradeAction for Audit Log. Only used to save the action related to editing of trade
        /// </summary>
        private List<TradeAuditActionType.ActionType> _tradeActions = new List<TradeAuditActionType.ActionType>();

        [Browsable(false)]
        private SwapParameters _swapParameters;

        [Browsable(false)]
        private OTCTradeData _otcParameters;  

        Dictionary<string, TaxLot> _updateDeleteTaxlotDict = null;

        #endregion

        public AllocationGroup(DateTime date)
        {
            // when a new group is created assign a CreationDate to it
            _aUECLocalDate = date;
        }
        public AllocationGroup()
        {
            _updateDeleteTaxlotDict = new Dictionary<string, TaxLot>();
        }
        public AllocationGroup(DataRow dr)
        {
            _updateDeleteTaxlotDict = new Dictionary<string, TaxLot>();
            _groupID = dr["GroupID"].ToString();
            if (!string.IsNullOrEmpty(dr["OrderSideTagValue"].ToString()))
                _orderSideTagValue = dr["OrderSideTagValue"].ToString();
            _symbol = dr["Symbol"].ToString();
            if (!string.IsNullOrEmpty(dr["OrderTypeTagValue"].ToString()))
                _orderTypeTagValue = dr["OrderTypeTagValue"].ToString();
            if (!string.IsNullOrEmpty(dr["CounterPartyID"].ToString()))
                _counterPartyID = Convert.ToInt32(dr["CounterPartyID"]);
            if (!string.IsNullOrEmpty(dr["TradingAccountID"].ToString()))
                _tradingAccountID = Convert.ToInt32(dr["TradingAccountID"]);
            if (!string.IsNullOrEmpty(dr["VenueID"].ToString()))
                _venueID = Convert.ToInt32(dr["VenueID"]);
            if (!string.IsNullOrEmpty(dr["AUECID"].ToString()))
                _auecID = Convert.ToInt32(dr["AUECID"]);
            if (!string.IsNullOrEmpty(dr["CumQty"].ToString()))
                _cumQty = Convert.ToDouble(dr["CumQty"]);
            if (!string.IsNullOrEmpty(dr["CumQty"].ToString()))
                _originalCumQty = Convert.ToDouble(dr["CumQty"]);
            if (!string.IsNullOrEmpty(dr["AllocatedQty"].ToString()))
                _allocatedQty = Convert.ToDouble(dr["AllocatedQty"]);
            if (!string.IsNullOrEmpty(dr["Quantity"].ToString()))
                _quantity = Convert.ToDouble(dr["Quantity"]);
            if (!string.IsNullOrEmpty(dr["AvgPrice"].ToString()))
                _avgPrice = Convert.ToDouble(dr["AvgPrice"]);
            if (!string.IsNullOrEmpty(dr["IsPreAllocated"].ToString()))
                _isPreAllocated = Convert.ToBoolean(dr["IsPreAllocated"]);
            if (!string.IsNullOrEmpty(dr["ListID"].ToString()))
                _listID = dr["ListID"].ToString();
            if (!string.IsNullOrEmpty(dr["UserID"].ToString()))
                _UserID = Convert.ToInt32(dr["UserID"]);
            if (!string.IsNullOrEmpty(dr["ISProrataActive"].ToString()))
                _isProrataActive = Convert.ToBoolean(dr["ISProrataActive"]);
            if (!string.IsNullOrEmpty(dr["AutoGrouped"].ToString()))
                _autoGrouped = Convert.ToBoolean(dr["AutoGrouped"]);
            if (!string.IsNullOrEmpty(dr["IsManualGroup"].ToString()))
                _isManualGroup = Convert.ToBoolean(dr["IsManualGroup"]);
            if (!string.IsNullOrEmpty(dr["StateID"].ToString()))
                _state = (PostTradeConstants.ORDERSTATE_ALLOCATION)Enum.Parse(typeof(PostTradeConstants.ORDERSTATE_ALLOCATION), dr["StateID"].ToString());
            if (!string.IsNullOrEmpty(dr["AllocationDate"].ToString()))
                _allocationDate = Convert.ToDateTime(dr["AllocationDate"]);
            if (!string.IsNullOrEmpty(dr["SettlementDate"].ToString()))
                _settlementDate = Convert.ToDateTime(dr["SettlementDate"]);
            if (!string.IsNullOrEmpty(dr["AssetID"].ToString()))
                _assetID = Convert.ToInt32(dr["AssetID"]);
            if (!string.IsNullOrEmpty(dr["UnderlyingID"].ToString()))
                _underlyingID = Convert.ToInt32(dr["UnderlyingID"]);
            if (!string.IsNullOrEmpty(dr["ExchangeID"].ToString()))
                _exchangeID = Convert.ToInt32(dr["ExchangeID"]);
            if (!string.IsNullOrEmpty(dr["CurrencyID"].ToString()))
                _currencyID = Convert.ToInt32(dr["CurrencyID"]);
            if (!string.IsNullOrEmpty(dr["Description"].ToString()))
                _description = dr["Description"].ToString();
            if (!string.IsNullOrEmpty(dr["AUECLocalDate"].ToString()))
                _aUECLocalDate = Convert.ToDateTime(dr["AUECLocalDate"]);
            if (!string.IsNullOrEmpty(dr["IsSwapped"].ToString()))
                _isSwapped = Convert.ToBoolean(dr["IsSwapped"]);
            if (!string.IsNullOrEmpty(dr["FXRate"].ToString()))
                _avgFXRateForTrade = Convert.ToDouble(dr["FXRate"]);
            if (!string.IsNullOrEmpty(dr["FXConversionMethodOperator"].ToString()))
                _FXConversionMethodOperator = dr["FXConversionMethodOperator"].ToString().Trim();
            _taxLotClosingId = string.IsNullOrEmpty(dr["TaxLotClosingId_FK"].ToString()) ? string.Empty : dr["TaxLotClosingId_FK"].ToString();
            if (!string.IsNullOrEmpty(dr["Commission"].ToString()))
                _commission = Convert.ToDouble(dr["Commission"]);
            if (!string.IsNullOrEmpty(dr["SoftCommission"].ToString()))
                _softCommission = Convert.ToDouble(dr["SoftCommission"]);
            if (!string.IsNullOrEmpty(dr["OtherBrokerFees"].ToString()))
                _otherBrokerfees = Convert.ToDouble(dr["OtherBrokerFees"]);
            if (!string.IsNullOrEmpty(dr["ClearingBrokerFee"].ToString()))
                _clearingBrokerFee = Convert.ToDouble(dr["ClearingBrokerFee"]);
            if (!string.IsNullOrEmpty(dr["StampDuty"].ToString()))
                _stampDuty = Convert.ToDouble(dr["StampDuty"]);
            if (!string.IsNullOrEmpty(dr["TransactionLevy"].ToString()))
                _transactionLevy = Convert.ToDouble(dr["TransactionLevy"]);
            if (!string.IsNullOrEmpty(dr["ClearingFee"].ToString()))
                _clearingFee = Convert.ToDouble(dr["ClearingFee"]);
            if (!string.IsNullOrEmpty(dr["TaxOnCommissions"].ToString()))
                _taxOnCommissions = Convert.ToDouble(dr["TaxOnCommissions"]);
            if (!string.IsNullOrEmpty(dr["MiscFees"].ToString()))
                _miscFees = Convert.ToDouble(dr["MiscFees"]);
            if (!string.IsNullOrEmpty(dr["SecFee"].ToString()))
                _secFee = Convert.ToDouble(dr["SecFee"]);
            if (!string.IsNullOrEmpty(dr["OccFee"].ToString()))
                _occFee = Convert.ToDouble(dr["OccFee"]);
            if (!string.IsNullOrEmpty(dr["OrfFee"].ToString()))
                _orfFee = Convert.ToDouble(dr["OrfFee"]);
            if (!string.IsNullOrEmpty(dr["AccruedInterest"].ToString()))
                _accruedInterest = Convert.ToDouble(dr["AccruedInterest"]);
            if (!string.IsNullOrEmpty(dr["ProcessDate"].ToString()))
                _processDate = Convert.ToDateTime(dr["ProcessDate"]);
            if (!string.IsNullOrEmpty(dr["OriginalPurchaseDate"].ToString()))
                _originalPurchaseDate = Convert.ToDateTime(dr["OriginalPurchaseDate"]);
            if (!string.IsNullOrEmpty(dr["CommissionSource"].ToString()))
                _commissionSource = Convert.ToInt32(dr["CommissionSource"]);
            if (!string.IsNullOrEmpty(dr["IsModified"].ToString()))
                _isModified = Convert.ToBoolean(dr["IsModified"]);
            if (!string.IsNullOrEmpty(dr["AllocationSchemeID"].ToString()))
                _allocationSchemeID = Convert.ToInt32(dr["AllocationSchemeID"]);
            if (!string.IsNullOrEmpty(dr["TaxLotIdsWithAttributes"].ToString()))
                _taxLotIdsWithAttributes = dr["TaxLotIdsWithAttributes"].ToString();
            if (!string.IsNullOrEmpty(dr["TradeAttribute1"].ToString()))
                _tradeAttribute1 = dr["TradeAttribute1"].ToString();
            if (!string.IsNullOrEmpty(dr["TradeAttribute2"].ToString()))
                _tradeAttribute2 = dr["TradeAttribute2"].ToString();
            if (!string.IsNullOrEmpty(dr["TradeAttribute3"].ToString()))
                _tradeAttribute3 = dr["TradeAttribute3"].ToString();
            if (!string.IsNullOrEmpty(dr["TradeAttribute4"].ToString()))
                _tradeAttribute4 = dr["TradeAttribute4"].ToString();
            if (!string.IsNullOrEmpty(dr["TradeAttribute5"].ToString()))
                _tradeAttribute5 = dr["TradeAttribute5"].ToString();
            if (!string.IsNullOrEmpty(dr["TradeAttribute6"].ToString()))
                _tradeAttribute6 = dr["TradeAttribute6"].ToString();
            if (!string.IsNullOrEmpty(dr["TransactionType"].ToString()))
                _transactionType = dr["TransactionType"].ToString();
            if (!string.IsNullOrEmpty(dr["NirvanaProcessDate"].ToString()))
                _nirvanaProcessDate = Convert.ToDateTime(dr["NirvanaProcessDate"]);
            if (!string.IsNullOrEmpty(dr["TransactionSource"].ToString()))
            {
                _transactionSource = (TransactionSource)Enum.Parse(typeof(TransactionSource), dr["TransactionSource"].ToString());
                _transactionSourceTag = Convert.ToInt32(dr["TransactionSource"].ToString());
            }
            if (!string.IsNullOrEmpty(dr["InternalComments"].ToString()))
                _internalComments = dr["InternalComments"].ToString();
            if (!string.IsNullOrEmpty(dr["SettlCurrency"].ToString()))
                _settlementCurrencyID = Convert.ToInt32(dr["SettlCurrency"]);
            if (!string.IsNullOrEmpty(dr["OptionPremiumAdjustment"].ToString()))
                _optionPremiumAdjustment = Convert.ToDouble(dr["OptionPremiumAdjustment"]);
            if (!string.IsNullOrEmpty(dr["ChangeType"].ToString()))
                _changeType = Convert.ToInt32(dr["ChangeType"]);
            if (!string.IsNullOrEmpty(dr["IsCommissionChanged"].ToString()))
                _isCommissionChanged = Convert.ToBoolean(dr["IsCommissionChanged"]);
            if (!string.IsNullOrEmpty(dr["IsSoftCommissionChanged"].ToString()))
                _isSoftCommissionChanged = Convert.ToBoolean(dr["IsSoftCommissionChanged"]);
            if (!string.IsNullOrEmpty(dr["OriginalAllocationPreferenceID"].ToString()))
                _originalAllocationPreferenceID = Convert.ToInt32(dr["OriginalAllocationPreferenceID"]);
            if (!string.IsNullOrEmpty(dr["UserID"].ToString()))
                _userID = Convert.ToInt32(dr["UserID"]);
            if (!string.IsNullOrEmpty(dr["IsManuallyModified"].ToString()))
                _isManuallyModified = Convert.ToBoolean(dr["IsManuallyModified"]);
            if (!string.IsNullOrEmpty(dr["BorrowerID"].ToString()))
                _borrowerID = dr["BorrowerID"].ToString();
            if (!string.IsNullOrEmpty(dr["BorrowBroker"].ToString()))
                _borrowerBroker = dr["BorrowBroker"].ToString();
            if (!string.IsNullOrEmpty(dr["ShortRebate"].ToString()))
                _shortRebate = Convert.ToDouble(dr["ShortRebate"]);
            if (_isSwapped)
            {
                _swapParameters = new SwapParameters(dr);
            }
            if (!string.IsNullOrEmpty(dr["AdditionalTradeAttributes"].ToString()))
            {
                base.SetTradeAttribute(dr["AdditionalTradeAttributes"].ToString());
            }
        }

        public virtual void SetGroupDetailsFromOrder(AllocationOrder order)
        {
            try
            {
                if (_cumQty == 0)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("For Groupid : ");
                    sb.Append(GroupID);
                    sb.Append(" ,Quantity is zero hence problem in calculating prorata commission and fees.");
                    throw new Exception(sb.ToString());
                }
                _orders.Add(order);
                Commission = _commission * (order.CumQty / _cumQty);
                AccruedInterest = _accruedInterest * (order.CumQty / _cumQty);
                SoftCommission = _softCommission * (order.CumQty / _cumQty);
                _otherBrokerfees = _otherBrokerfees * (order.CumQty / _cumQty);
                _clearingBrokerFee = _clearingBrokerFee * (order.CumQty / _cumQty);
                _stampDuty = _stampDuty * (order.CumQty / _cumQty);
                _transactionLevy = _transactionLevy * (order.CumQty / _cumQty);
                _clearingFee = _clearingFee * (order.CumQty / _cumQty);
                _taxOnCommissions = _taxOnCommissions * (order.CumQty / _cumQty);
                _miscFees = _miscFees * (order.CumQty / _cumQty);
                _secFee = _secFee * (order.CumQty / _cumQty);
                _occFee = _occFee * (order.CumQty / _cumQty);
                _orfFee = _orfFee * (order.CumQty / _cumQty);

                _cumQty = order.CumQty;
                _quantity = order.Quantity;
                _avgPrice = order.AvgPrice;
                _avgFXRateForTrade = order.FXRate;
                _FXConversionMethodOperator = order.FXConversionMethodOperator.ToString();
                _settlementCurrencyID = order.SettlementCurrencyID;

                _orderCount = _orders.Count;

                //Adding more fields as they were missing.
                //Problem faced when reverting order values from a grouped data while ungrouping, 
                //as List of orders in allocation group object does not contain these values.
                _originalPurchaseDate = order.OriginalPurchaseDate;
                _aUECLocalDate = order.AUECLocalDate;
                _processDate = order.ProcessDate;
                _settlementDate = order.SettlementDate;
                _orderSideTagValue = order.OrderSideTagValue;
                _venueID = order.VenueID;
                _counterPartyID = order.CounterPartyID;
                _tradingAccountID = order.TradingAccountID;
                //Updating trade attributes in group from order.
                _tradeAttribute1 = order.TradeAttribute1;
                _tradeAttribute2 = order.TradeAttribute2;
                _tradeAttribute3 = order.TradeAttribute3;
                _tradeAttribute4 = order.TradeAttribute4;
                _tradeAttribute5 = order.TradeAttribute5;
                _tradeAttribute6 = order.TradeAttribute6;
                SetTradeAttribute(order.GetTradeAttributesAsDict());
                CompanyUserID = order.CompanyUserID;
                _description = order.Description;
                _internalComments = order.InternalComments;
                _changeType = order.ChangeType;
                _originalAllocationPreferenceID = order.OriginalAllocationPreferenceID;
                _transactionSourceTag = order.TransactionSourceTag;
                _borrowerID = order.BorrowerID;
                _borrowerBroker = order.BorrowerBroker;
                _shortRebate = order.ShortRebate;
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

        #region public Methods

        /// <summary>
        /// Adds the action to the allocationGroup _tradeActions list
        /// </summary>
        /// <param name="action">action of the type TradeAuditActionType.ActionType performed on the group</param>
        public virtual void AddTradeAction(TradeAuditActionType.ActionType action)
        {
            try
            {
                if (!_tradeActions.Contains(action))
                    _tradeActions.Add(action);
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

        public virtual int CountTradeActions()
        {
            return _tradeActions.Count;
        }

        public virtual void UpdateOrder(Order updatedorder)
        {
            AllocationOrder deletedOrder = null;
            AllocationOrder allocOrder = new AllocationOrder();
            allocOrder.ClOrderID = updatedorder.ClOrderID;
            allocOrder.GroupID = _groupID;
            allocOrder.CumQty = updatedorder.CumQty;
            allocOrder.AvgPrice = updatedorder.AvgPrice;
            allocOrder.FXRate = updatedorder.FXRate;
            allocOrder.Quantity = updatedorder.Quantity;
            allocOrder.Description = updatedorder.Text;
            allocOrder.InternalComments = updatedorder.InternalComments;
            allocOrder.ParentClOrderID = updatedorder.ParentClOrderID;

            // Updating allocation order details from Order
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7750
            allocOrder.CompanyUserID = updatedorder.CompanyUserID;
            allocOrder.CounterPartyID = updatedorder.CounterPartyID;
            allocOrder.OrderSideTagValue = updatedorder.OrderSideTagValue;
            allocOrder.VenueID = updatedorder.VenueID;
            allocOrder.TradingAccountID = updatedorder.TradingAccountID;
            allocOrder.FXConversionMethodOperator = updatedorder.FXConversionMethodOperator;
            // Updating order dates
            allocOrder.AUECLocalDate = updatedorder.AUECLocalDate;
            allocOrder.OriginalPurchaseDate = updatedorder.OriginalPurchaseDate;
            allocOrder.ProcessDate = updatedorder.ProcessDate;
            allocOrder.SettlementDate = updatedorder.SettlementDate;
            allocOrder.SettlementCurrencyID = updatedorder.SettlementCurrencyID;
            double tempCumQty = 0;
            double tempQuantity = 0;
            double tempTotalExec = 0;
            foreach (AllocationOrder order in _orders)
            {
                if (updatedorder.ParentClOrderID == order.ParentClOrderID)
                {
                    deletedOrder = order;
                    break;
                }
            }
            if (deletedOrder != null)
            {
                allocOrder.TransactionSourceTag = deletedOrder.TransactionSourceTag;
                allocOrder.TransactionSource = deletedOrder.TransactionSource;
                _orders.Remove(deletedOrder);
                _orders.Add(allocOrder);
            }
            foreach (AllocationOrder order in _orders)
            {
                tempCumQty += order.CumQty;
                tempQuantity += order.Quantity;
                tempTotalExec += order.CumQty * order.AvgPrice;
            }
            _cumQty = tempCumQty;
            _quantity = tempQuantity;
            // Sandeep Singh July 2, 2013: check should be on tempCumQty, it is dividing tempTotalExec to calculate weighted avarage price
            if (tempCumQty != 0)
            {
                _avgPrice = tempTotalExec / tempCumQty;
            }
            updateInternalComments();
        }

        /// <summary>
        /// just adds the order in collection
        /// </summary>
        /// <param name="order"></param>
        public virtual void AddOrder(AllocationOrder order)
        {
            try
            {
                _orders.Add(order);
                updateInternalComments();
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

        public virtual void AddAccounts(AllocationLevelList allocationAccounts)
        {
            try
            {
                _level1Collection.Add(allocationAccounts);
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
        /// to add taxlots 
        /// </summary>
        /// <param name="taxlots"></param>
        public virtual void Addtaxlots(TaxLot taxlot)
        {
            try
            {
                if (_taxLots == null)
                {
                    _taxLots = new List<TaxLot>();
                }
                _taxLots.Add(taxlot);
                CalculateTotalCommission();
                CalculateTotalFees();
                CalculateTotalOtherFees();
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

        public virtual void CalculateTotalOtherFees()
        {
            if (_level1Collection != null)
            {
                _stampDuty = 0.0;
                _transactionLevy = 0.0;
                _clearingFee = 0.0;
                _taxOnCommissions = 0.0;
                _miscFees = 0.0;
                _secFee = 0.0;
                _occFee = 0.0;
                _orfFee = 0.0;
                if (_taxLots != null)
                {
                    foreach (TaxLot taxLot in _taxLots)
                    {
                        _stampDuty += taxLot.StampDuty;
                        _transactionLevy += taxLot.TransactionLevy;
                        _clearingFee += taxLot.ClearingFee;
                        _taxOnCommissions += taxLot.TaxOnCommissions;
                        _miscFees += taxLot.MiscFees;
                        _secFee += taxLot.SecFee;
                        _occFee += taxLot.OccFee;
                        _orfFee += taxLot.OrfFee;
                    }
                }
                _isCommissionCalculated = false;
            }
        }
        public virtual void ClearAccounts()
        {
            try
            {
                _level1Collection = new AllocationLevelList();
                CalculateTaxLots();
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

        public virtual void UnAllocate()
        {
            _state = PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED;
            if (_persistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
            {
                _persistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
            }
            ClearAccounts();
            _allocationDate = DateTimeConstants.MinValue;
            _allocatedQty = 0.0;
            //reset allocation scheme id and name while unallocating a trade
            _allocationSchemeID = 0;
            _allocationSchemeName = string.Empty;
        }

        public virtual void Allocate(AllocationLevelList allocationLevelList)
        {
            decimal allocatedQty = 0;
            foreach (AllocationLevelClass account in allocationLevelList.Collection)
            {
                allocatedQty += Convert.ToDecimal(account.AllocatedQty);
            }
            _allocatedQty = Convert.ToDouble(allocatedQty);
            _state = PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED;
            if (_persistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
            {
                _persistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
            }
            AddAccounts(allocationLevelList);
            CalculateTaxLots();
            _allocationDate = _processDate;
            if (_taxLots != null && _taxLots.Count > 0)
            {
                DistributeCommisionInTaxLot(true, true);
                DistributeFeesInTaxLot();
                DistributeOtherFeesInTaxLot();
            }
        }

        public virtual void ProrataAllocate()
        {
            if (_cumQty == 0)
            {
                return;
            }
            if (_state == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
            {
                _level1Collection = new AllocationLevelList();
                return;
            }
            AllocationLevelList allocationLevelList = new AllocationLevelList();
            _allocatedQty = _cumQty;
            foreach (AllocationLevelClass account in _level1Collection.Collection)
            {
                AllocationLevelClass level1 = new AllocationLevelClass(_groupID);
                level1.Percentage = account.Percentage;
                level1.AllocatedQty = PostTradeHelper.GetAllocatedQty(_assetID, level1.Percentage, _cumQty);
                level1.LevelnID = account.LevelnID;
                allocationLevelList.Add(level1);
                if (account.Childs != null)
                {
                    foreach (AllocationLevelClass strategy in account.Childs.Collection)
                    {
                        AllocationLevelClass level2 = new AllocationLevelClass(_groupID);
                        level2.Percentage = strategy.Percentage;
                        level2.AllocatedQty = PostTradeHelper.GetAllocatedQty(_assetID, level2.Percentage, level1.AllocatedQty);
                        level2.LevelnID = strategy.LevelnID;
                        level1.AddChilds(level2);
                    }
                }
            }
            _allocatedQty = _cumQty;
            string isvalid = ValidateAllocationAccounts(this, ref allocationLevelList);
            if (isvalid == string.Empty)
            {
                _state = PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED;
                if (_persistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
                {
                    _persistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
                }
                _level1Collection = new AllocationLevelList();
                AddAccounts(allocationLevelList);
                CalculateTaxLots();
                _allocationDate = _aUECLocalDate;
                ResetTaxlotDictionary(_taxLots);
                if (_taxLots != null && _taxLots.Count > 0)
                {
                    DistributeCommisionInTaxLot(true, true);
                    DistributeFeesInTaxLot();
                    DistributeOtherFeesInTaxLot();
                }
            }
            else
            {
                throw new Exception(isvalid);
            }
        }
        public virtual void DistributeFeesInTaxLot()
        {
            foreach (TaxLot taxlot in _taxLots)
            {
                //Recalculated taxlot percentage to exact percision digit
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8160
                decimal taxlotPercentage = this.CumQty == 0.0 ? (decimal)taxlot.Percentage : CalculateTaxlotPercentage(taxlot.TaxLotQty);
                taxlot.OtherBrokerFees = (_otherBrokerfees * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
                taxlot.ClearingBrokerFee = (_clearingBrokerFee * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
            }
        }
        public virtual void DistributeCommisionInTaxLot(bool distributeCommission, bool distributeSoftCommission)
        {
            foreach (TaxLot taxlot in _taxLots)
            {
                //Recalculated taxlot percentage to exact percision digit
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8160
                decimal taxlotPercentage = this.CumQty == 0.0 ? (decimal)taxlot.Percentage : CalculateTaxlotPercentage(taxlot.TaxLotQty);
                if (distributeCommission)
                    taxlot.Commission = (_commission * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
                if (distributeSoftCommission)
                    taxlot.SoftCommission = (_softCommission * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
            }
        }
        public virtual void DistributeOtherFeesInTaxLot()
        {
            foreach (TaxLot taxlot in _taxLots)
            {
                //Recalculated taxlot percentage to exact percision digit
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-8160
                decimal taxlotPercentage = this.CumQty == 0.0 ? (decimal)taxlot.Percentage : CalculateTaxlotPercentage(taxlot.TaxLotQty);
                taxlot.TaxOnCommissions = (_taxOnCommissions * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
                taxlot.ClearingFee = (_clearingFee * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
                taxlot.StampDuty = (_stampDuty * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
                taxlot.MiscFees = (_miscFees * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
                taxlot.TransactionLevy = (_transactionLevy * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
                taxlot.SecFee = (_secFee * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
                taxlot.OccFee = (_occFee * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
                taxlot.OrfFee = (_orfFee * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;
                taxlot.OptionPremiumAdjustment = (_optionPremiumAdjustment * NumberPrecisionConstants.ToDoublePrecise(taxlotPercentage)) / 100;//Distrtibuting option premium based on taxlot QTY
            }
        }

        /// <summary>
        /// Adds account in group , adds taxlots and calculates taxlots commision from
        /// groups commios
        /// </summary>
        /// <param name="account"></param>
        public virtual void AddAccount(AllocationLevelClass account)
        {
            _level1Collection.Add(account);
            CalculateTaxLots();
        }

        public virtual void UpdateAccounts(AllocationLevelClass newaccount)
        {
            try
            {
                bool alreadyPresent = false;
                foreach (AllocationLevelClass account in _level1Collection.Collection)
                {
                    if (account.LevelnID == newaccount.LevelnID)
                    {
                        account.AllocatedQty += newaccount.AllocatedQty;
                        alreadyPresent = true;
                        break;
                    }
                }
                if (!alreadyPresent)
                {
                    _level1Collection.Add(newaccount);
                }
                CalculateTaxLots();
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

        private int _orderCount = 1;
        [Browsable(false)]
        public virtual int OrderCount
        {
            get { return _orderCount; }
            set { _orderCount = value; }
        }

        /// <summary>
        /// adds a group in this group
        /// </summary>
        /// <param name="group"></param>
        public virtual void AddGroup(AllocationGroup group)
        {
            double notional = 0D;
            double cumQty = 0D;
            foreach (AllocationOrder order in _orders)
            {
                notional += (order.CumQty * order.AvgPrice);
                cumQty += order.CumQty;
            }
            _avgPrice = cumQty != 0D ? notional / cumQty : 0D;
            notional = 0D;
            cumQty = 0D;
            foreach (AllocationOrder order in group.Orders)
            {
                notional += (order.CumQty * order.AvgPrice);
                cumQty += order.CumQty;
            }
            group.AvgPrice = cumQty != 0D ? notional / cumQty : 0D;

            foreach (AllocationOrder order in group.Orders)
            {
                AddOrder(order);
            }
            if (!_FXConversionMethodOperator.Equals(group.FXConversionMethodOperator))
            {
                switch (_currencyName)
                {
                    case "EUR":
                    case "AUD":
                    case "NZD":
                    case "GBP":
                        {
                            if (_FXConversionMethodOperator.Equals(Operator.D.ToString()))
                            {
                                _FXConversionMethodOperator = Operator.M.ToString();
                                _avgFXRateForTrade = _avgFXRateForTrade == 0 ? 0 : 1 / _avgFXRateForTrade;
                            }
                            else
                            {
                                group.FXConversionMethodOperator = Operator.M.ToString();
                                group.FXRate = group.FXRate == 0 ? 0 : 1 / group.FXRate;
                            }
                        }
                        break;
                    default:
                        {
                            if (_FXConversionMethodOperator.Equals(Operator.M.ToString()))
                            {
                                _FXConversionMethodOperator = Operator.D.ToString();
                                _avgFXRateForTrade = _avgFXRateForTrade == 0 ? 0 : 1 / _avgFXRateForTrade;
                            }
                            else
                            {
                                group.FXConversionMethodOperator = Operator.D.ToString();
                                group.FXRate = group.FXRate == 0 ? 0 : 1 / group.FXRate;
                            }
                        }
                        break;
                }
            }
            if ((_cumQty * _avgPrice + group.CumQty * group.AvgPrice) != 0)
            {
                _avgFXRateForTrade = ((group.FXRate * group.CumQty * group.AvgPrice + _cumQty * _avgPrice * _avgFXRateForTrade) / (_cumQty * _avgPrice + group.CumQty * group.AvgPrice));
            }
            _avgPrice = Convert.ToDouble((group.CumQty * group.AvgPrice + _cumQty * _avgPrice) / (_cumQty + group.CumQty));
            _cumQty += group.CumQty;
            _quantity += group.Quantity;
            Commission += group.Commission;
            SoftCommission += group.SoftCommission;
            _otherBrokerfees += group.OtherBrokerFees;
            _clearingBrokerFee += group.ClearingBrokerFee;
            _stampDuty += group.StampDuty;
            _transactionLevy += group.TransactionLevy;
            _clearingFee += group.ClearingFee;
            _taxOnCommissions += group.TaxOnCommissions;
            _miscFees += group.MiscFees;
            _secFee += group.SecFee;
            _occFee += group.OccFee;
            _orfFee += group.OrfFee;
            _orderCount = _orders.Count;
            AccruedInterest += group.AccruedInterest;
        }

        public virtual void UpdateGroupCommissionAndFees(CommissionFields commissionFields)
        {
            double updatedGroupCommissionFee = 0.0;
            if (commissionFields.Commission != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.Commission;
                }
                Commission = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.SoftCommission != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.SoftCommission;
                }
                SoftCommission = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.OtherBrokerFees != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.OtherBrokerFees;
                }
                _otherBrokerfees = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.ClearingBrokerFee != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.ClearingBrokerFee;
                }
                _clearingBrokerFee = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.ClearingFee != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.ClearingFee;
                }
                _clearingFee = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.MiscFees != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.MiscFees;
                }
                _miscFees = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.StampDuty != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.StampDuty;
                }
                _stampDuty = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.TaxOnCommissions != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.TaxOnCommissions;
                }
                _taxOnCommissions = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.TransactionLevy != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.TransactionLevy;
                }
                _transactionLevy = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.SecFee != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.SecFee;
                }
                _secFee = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.OccFee != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.OccFee;
                }
                _occFee = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.OrfFee != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.OrfFee;
                }
                _orfFee = updatedGroupCommissionFee;
            }

            updatedGroupCommissionFee = 0.0;
            if (commissionFields.OptionPremiumAdjustment != double.MinValue)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    updatedGroupCommissionFee += childTaxlot.OptionPremiumAdjustment;
                }
                _optionPremiumAdjustment = updatedGroupCommissionFee;
            }

        }

        public virtual void UpdateTaxlotCommissionAndFees(CommissionFields commissionFields)
        {
            try
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    if (commissionFields.Commission != double.MinValue)
                        childTaxlot.Commission = _commission * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.SoftCommission != double.MinValue)
                        childTaxlot.SoftCommission = _softCommission * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.OtherBrokerFees != double.MinValue)
                        childTaxlot.OtherBrokerFees = _otherBrokerfees * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.ClearingBrokerFee != double.MinValue)
                        childTaxlot.ClearingBrokerFee = _clearingBrokerFee * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.ClearingFee != double.MinValue)
                        childTaxlot.ClearingFee = _clearingFee * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.MiscFees != double.MinValue)
                        childTaxlot.MiscFees = _miscFees * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.StampDuty != double.MinValue)
                        childTaxlot.StampDuty = _stampDuty * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.TaxOnCommissions != double.MinValue)
                        childTaxlot.TaxOnCommissions = _taxOnCommissions * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.TransactionLevy != double.MinValue)
                        childTaxlot.TransactionLevy = _transactionLevy * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.SecFee != double.MinValue)
                        childTaxlot.SecFee = _secFee * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.OccFee != double.MinValue)
                        childTaxlot.OccFee = _occFee * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.OrfFee != double.MinValue)
                        childTaxlot.OrfFee = _orfFee * childTaxlot.TaxLotQty / _allocatedQty;

                    if (commissionFields.OptionPremiumAdjustment != double.MinValue)
                        childTaxlot.OptionPremiumAdjustment = _optionPremiumAdjustment * childTaxlot.TaxLotQty / _allocatedQty;

                    UpdateTaxlotState(childTaxlot);
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
        public virtual void UpdateTaxlotState()
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                UpdateTaxlotState(childTaxlot);
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// list to stroe the actions performed on the group. Used in edit/trade only for edited trades
        /// </summary>
        [XmlIgnore, Browsable(false)]
        public virtual List<TradeAuditActionType.ActionType> TradeActionsList
        {
            get { return _tradeActions; }
            set { _tradeActions = value; }
        }

        public virtual string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        /// <summary>
        /// Gets or sets the original group i ds.(To be used only for Virtual Groups generated MasterFund Allocation )
        /// </summary>
        /// <value>
        /// The original group i ds.
        /// </value>
        [XmlIgnore, Browsable(false)]
        public virtual List<string> OriginalGroupIDs
        {
            get
            {
                if (_originalGroupIDs == null || _originalGroupIDs.Count == 0)
                    return new List<string>() { _groupID };
                else
                    return _originalGroupIDs;
            }
            set { _originalGroupIDs = value; }
        }

        public virtual double AllocatedQty
        {
            set { _allocatedQty = value; }
            get { return _allocatedQty; }
        }

        //[Browsable(false)]
        public virtual bool IsPreAllocated
        {
            set { _isPreAllocated = value; }
            get { return _isPreAllocated; }
        }

        private int _UserID;
        public virtual int UserID
        {
            set { _UserID = value; }
            get { return _UserID; }
        }

        //[Browsable(false)]
        public virtual bool ISProrataActive
        {
            set { _isProrataActive = value; }
            get { return _isProrataActive; }
        }

        public virtual bool AutoGrouped
        {
            set { _autoGrouped = value; }
            get { return _autoGrouped; }
        }

        public virtual int StateID
        {
            set { _state = (PostTradeConstants.ORDERSTATE_ALLOCATION)value; }
            get { return (int)_state; }
        }

        //[Browsable(false)]
        public virtual bool IsManualGroup
        {
            get { return _isManualGroup; }
            set { _isManualGroup = value; }
        }

        public virtual bool IsOverbuyOversellAccepted
        {
            get { return _isOverbuyOversellAccepted; }
            set { _isOverbuyOversellAccepted = value; }
        }

        [Browsable(false)]
        public virtual DateTime AllocationDate
        {
            get { return _allocationDate; }
            set { _allocationDate = value; }
        }

        private bool _isSwapped = false;
        //[Browsable(false)]
        public virtual bool IsSwapped
        {
            get { return _isSwapped; }
            set { _isSwapped = value; }
        }

        private List<TaxLot> _taxLots = new List<TaxLot>();
        public virtual List<TaxLot> TaxLots
        {
            get { return _taxLots; }
            set { _taxLots = value; }
        }

        //NeverUsed
        public virtual void AddTaxlots(TaxLot taxlot1)
        {
            _taxLots.Add(taxlot1);
        }

        private IList<Level1Allocation> level1AllocationList = new List<Level1Allocation>();
        [XmlIgnore]
        [Browsable(false)]
        public virtual IList<Level1Allocation> Level1AllocationList
        {
            get { return level1AllocationList; }
            set { level1AllocationList = value; }
        }

        private IList<AllocationOrder> _ordersH = new List<AllocationOrder>();
        [XmlIgnore]
        [Browsable(false)]
        public virtual IList<AllocationOrder> OrdersH
        {
            get { return _ordersH; }
            set { _ordersH = value; }
        }

        private List<AllocationOrder> _orders = new List<AllocationOrder>();
        public virtual List<AllocationOrder> Orders
        {
            get { return _orders; }
            set { _orders = value; }
        }

        public virtual string CommissionText
        {
            get { return _commissionText; }
            set { _commissionText = value; }
        }

        public virtual SwapParameters SwapParameters
        {
            get { return _swapParameters; }
            set { _swapParameters = value; }
        }

        public virtual OTCTradeData OTCParameters
        {
            get { return _otcParameters; }
            set { _otcParameters = value; }
        }


        [Browsable(false)]
        public virtual AllocationLevelList Allocations
        {
            get { return _level1Collection; }
        }

        private IList<SwapParameters> _swapParametersH = new List<SwapParameters>();

        [XmlIgnore]
        [Browsable(false)]
        public virtual IList<SwapParameters> SwapParametersH
        {
            get { return _swapParametersH; }
            set { _swapParametersH = value; }
        }
        public virtual bool FillAdditionalParameters()
        {
            try
            {
                _orders.Clear();
                foreach (AllocationOrder o in _ordersH)
                {
                    /* This change is done for managing trade attributes while grouping and ungrouping
                     * As there is no columns for trade attributes in T_TradedOrders, so currently using values from group
                     * When these columns get updated in table then these line must get commented and should be entered in hbm.xml file
                     * So steps required will be as
                     * 1. Add columns in T_TradedOrders
                     * 2. Update hbm.xml file to include those columns
                     * 3. Comment following line to avoid override values
                     * 4. Update SPs which updates data in T_TradedOrders such as P_UpdateGroupXML etc
                     * http://jira.nirvanasolutions.com:8080/browse/PRANA-6038
                     */
                    //o.TradeAttribute1 = _tradeAttribute1;
                    //o.TradeAttribute2 = _tradeAttribute2;
                    //o.TradeAttribute3 = _tradeAttribute3;
                    //o.TradeAttribute4 = _tradeAttribute4;
                    //o.TradeAttribute5 = _tradeAttribute5;
                    //o.TradeAttribute6 = _tradeAttribute6;

                    _orders.Add(o);
                }

                if (SwapParametersH.Count > 0)
                {
                    _swapParameters = _swapParametersH[0];
                }
                return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        [XmlIgnore]
        public virtual bool Updated
        {
            set { _updated = value; }
            get { return _updated; }
        }

        [Browsable(false)]
        public virtual string ListID
        {
            get { return _listID; }
            set { _listID = value; }
        }

        [XmlIgnore]
        public virtual double UnAllocatedQty
        {
            get { return _cumQty - _allocatedQty; }
        }

        [XmlIgnore]
        public virtual bool NotAllExecuted
        {
            get
            {
                if (_cumQty == _quantity)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [XmlIgnore]
        public virtual bool AllocatedEqualTotalQty
        {
            get
            {
                if (_allocatedQty == _quantity)
                    return true;
                else return false;
            }
        }

        [XmlIgnore]
        [Browsable(false)]
        public virtual PostTradeConstants.ORDERSTATE_ALLOCATION State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (value == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                {
                    _allocationSchemeID = 0;
                    _allocationSchemeName = string.Empty;
                }
            }
        }

        public virtual double Commission
        {
            get
            {
                return Double.IsNaN(_commission) ? 0.0 : _commission;
            }
            set { _commission = value; }
        }

        public virtual double SoftCommission
        {
            get
            {
                return Double.IsNaN(_softCommission) ? 0.0 : _softCommission;
            }
            set { _softCommission = value; }
        }

        public virtual double OtherBrokerFees
        {
            get
            {
                return Double.IsNaN(_otherBrokerfees) ? 0.0 : _otherBrokerfees;
            }
            set { _otherBrokerfees = value; }
        }

        public virtual double ClearingBrokerFee
        {
            get
            {
                return Double.IsNaN(_clearingBrokerFee) ? 0.0 : _clearingBrokerFee;
            }
            set { _clearingBrokerFee = value; }
        }

        public virtual double TotalCommissionandFees
        {
            //Addign option premium to total commission and fees.
            get { return StampDuty + ClearingFee + MiscFees + TaxOnCommissions + TransactionLevy + Commission + SoftCommission + OtherBrokerFees + ClearingBrokerFee + SecFee + OccFee + OrfFee + OptionPremiumAdjustment; }
        }

        private double _stampDuty;
        public virtual double StampDuty
        {
            get
            {
                return Double.IsNaN(_stampDuty) ? 0.0 : _stampDuty;
            }
            set { _stampDuty = value; }
        }

        private double _transactionLevy;
        public virtual double TransactionLevy
        {
            get
            {
                return Double.IsNaN(_transactionLevy) ? 0.0 : _transactionLevy;
            }
            set { _transactionLevy = value; }
        }

        private double _clearingFee;
        public virtual double ClearingFee
        {
            get
            {
                return Double.IsNaN(_clearingFee) ? 0.0 : _clearingFee;
            }
            set { _clearingFee = value; }
        }

        private double _taxOnCommissions;
        public virtual double TaxOnCommissions
        {
            get
            {
                return Double.IsNaN(_taxOnCommissions) ? 0.0 : _taxOnCommissions;
            }
            set { _taxOnCommissions = value; }
        }

        private double _miscFees;
        public virtual double MiscFees
        {
            get
            {
                return Double.IsNaN(_miscFees) ? 0.0 : _miscFees;
            }
            set { _miscFees = value; }
        }

        private double _secFee;
        public virtual double SecFee
        {
            get
            {
                return Double.IsNaN(_secFee) ? 0.0 : _secFee;
            }
            set { _secFee = value; }
        }

        private double _occFee;
        public virtual double OccFee
        {
            get
            {
                return Double.IsNaN(_occFee) ? 0.0 : _occFee;
            }
            set { _occFee = value; }
        }

        private double _orfFee;
        public virtual double OrfFee
        {
            get
            {
                return Double.IsNaN(_orfFee) ? 0.0 : _orfFee;
            }
            set { _orfFee = value; }
        }

        private double _optionPremiumAdjustment;
        public virtual double OptionPremiumAdjustment
        {
            get
            {
                return Double.IsNaN(_optionPremiumAdjustment) ? 0.0 : _optionPremiumAdjustment;
            }
            set { _optionPremiumAdjustment = value; }
        }

        private CommisionSource _commSource = CommisionSource.Auto;
        public virtual CommisionSource CommSource
        {
            get { return _commSource; }
            set
            {
                _commSource = value;
                _commissionSource = Convert.ToInt32(_commSource);
            }
        }

        private CommisionSource _softCommSource = CommisionSource.Auto;
        public virtual CommisionSource SoftCommSource
        {
            get { return _softCommSource; }
            set
            {
                _softCommSource = value;
                _softCommissionSource = Convert.ToInt32(_softCommSource);
            }
        }

        private int _commissionSource = 1;
        public virtual int CommissionSource
        {
            get { return _commissionSource; }
            set
            {
                _commissionSource = value;
                _commSource = (CommisionSource)value;
            }
        }

        private int _softCommissionSource = 1;
        public virtual int SoftCommissionSource
        {
            get { return _softCommissionSource; }
            set
            {
                _softCommissionSource = value;
                _softCommSource = (CommisionSource)value;
            }
        }

        [Browsable(false)]
        public virtual ApplicationConstants.PersistenceStatus PersistenceStatus
        {
            get { return _persistenceStatus; }
            set
            {
                _persistenceStatus = value;
                if (_persistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
                {
                    foreach (TaxLot taxlot in _taxLots)
                    {
                        taxlot.TaxLotState = ApplicationConstants.TaxLotState.NotChanged;
                    }
                }
                else if (_persistenceStatus == ApplicationConstants.PersistenceStatus.ReAllocated)
                {
                    foreach (TaxLot taxlot in _taxLots)
                    {
                        taxlot.TaxLotState = ApplicationConstants.TaxLotState.Updated;
                    }
                }
            }
        }

        [Browsable(false)]
        public virtual bool CommissionCalculationTime
        {
            get { return _commissionCalculationTime; }
            set { _commissionCalculationTime = value; }
        }

        // ading new keyword as derived class field has [Browsable(false)] attribute
        new double _strikePrice = 0;
        [Browsable(false)]
        new public virtual double StrikePrice
        {
            get { return _strikePrice; }
            set { _strikePrice = value; }
        }

        // adding new as derived has an initial value
        new int _putOrCall = int.MinValue;
        [Browsable(false)]
        public virtual int PutCall
        {
            get { return _putOrCall; }
            set { _putOrCall = value; }
        }

        private OrderFields.PranaMsgTypes _pranaMsgType;
        [XmlIgnore]
        public virtual OrderFields.PranaMsgTypes PranaMsgType
        {
            get { return _pranaMsgType; }
            set
            {
                if (_intPranaMsgType != int.MinValue)
                    _pranaMsgType = value;
                _intPranaMsgType = (int)_pranaMsgType;
            }
        
        }

        private int _intPranaMsgType = int.MinValue;
        [Browsable(false)]
        public virtual int IntPranaMsgType
        {
            get { return _intPranaMsgType; }
            set
            {
                _intPranaMsgType = value;
                if(_intPranaMsgType != int.MinValue)
                _pranaMsgType = (OrderFields.PranaMsgTypes)_intPranaMsgType;
            }
        }

        private string _corpActionId = Guid.Empty.ToString();
        [Browsable(false)]
        public virtual string CorpActionID
        {
            get { return _corpActionId; }
            set { _corpActionId = value; }
        }

        private int _positionTagValue;
        [Browsable(false)]
        public virtual int PositionTagValue
        {
            get { return _positionTagValue; }
            set { _positionTagValue = value; }
        }

        private string _taxLotClosingId;
        public virtual string TaxLotClosingId
        {
            get { return _taxLotClosingId; }
            set { _taxLotClosingId = value; }
        }

        private long _parentTaxlot_PK;
        /// <summary>
        /// The property has been put for corporate action child rows. If these are newly generated rows, these must
        /// recognize their parents.
        /// </summary>
        [Browsable(false)]
        public virtual long ParentTaxlot_PK
        {
            get { return _parentTaxlot_PK; }
            set { _parentTaxlot_PK = value; }
        }

        private string _lotId = string.Empty;
        /// <summary>
        /// Added By Sandeep as on 08-Feb-2013
        /// this keep the lot ID send by the Nirvana client i.e. user
        /// </summary>    
        [Browsable(false)]
        public virtual string LotId
        {
            get { return _lotId; }
            set { _lotId = (value == null) ? string.Empty : value; }
        }

        private string _externalTransId = string.Empty;
        /// <summary>
        /// Added By Sandeep as on 25-Feb-2013
        /// this keep the External Transaction ID send by the Nirvana client side i.e. user
        /// </summary>   
        [Browsable(false)]
        public virtual string ExternalTransId
        {
            get { return _externalTransId; }
            set { _externalTransId = (value == null) ? string.Empty : value; }
        }

        private string _taxLotIdsWithAttributes = string.Empty;
        /// <summary>
        /// Added By Narendra as on 18-Mar-2013
        /// 
        /// </summary>   
        [Browsable(false)]
        public virtual string TaxLotIdsWithAttributes
        {
            get { return _taxLotIdsWithAttributes; }
            set { _taxLotIdsWithAttributes = value; }
        }
        #endregion

        #region commision fees calculation
        public virtual void CalculateTaxLots()
        {
            _taxLots = new List<TaxLot>();
            try
            {
                _taxLots = CreateTaxlots();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public virtual List<TaxLot> CreateTaxlots()
        {
            bool isFractionalAllowed = PostTradeHelper.ISFractionalAllocationAllowed(_assetID);
            List<TaxLot> taxlots = new List<TaxLot>();
            try
            {
                foreach (AllocationLevelClass level1Obj in _level1Collection.Collection)
                {
                    if (level1Obj.Childs != null)
                    {
                        foreach (AllocationLevelClass strategy in level1Obj.Childs.Collection)
                        {
                            if (strategy.Percentage != 0)
                            {
                                TaxLot taxLot = new TaxLot(this);
                                taxLot.SetAndCalculateValues(level1Obj, strategy, isFractionalAllowed);

                                if (taxLot.AssetID.Equals(this.AssetID))
                                {
                                    SetAndCalculateAccruedInterest(taxLot);
                                }
                                taxlots.Add(taxLot);
                            }
                        }
                    }
                    else
                    {
                        if (level1Obj.Percentage != 0)
                        {
                            TaxLot taxLot = new TaxLot(this);
                            taxLot.SetAndCalculateValues(level1Obj, null, isFractionalAllowed);
                            if (taxLot.AssetID.Equals(this.AssetID))
                            {
                                SetAndCalculateAccruedInterest(taxLot);
                            }
                            taxlots.Add(taxLot);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }
            return taxlots;
        }

        public virtual List<TaxLot> GetAllTaxlots()
        {
            List<TaxLot> taxlots = new List<TaxLot>();
            foreach (KeyValuePair<string, TaxLot> keyvalpair in _updateDeleteTaxlotDict)
            {
                taxlots.Add(keyvalpair.Value);
            }
            return taxlots;
        }

        /// <summary>
        /// to get list of taxlotids from cache _updateDeleteTaxlotDict for each 
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-2238
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetAllTaxlotIDs()
        {
            List<string> taxlotIDs = new List<string>();
            foreach (KeyValuePair<string, TaxLot> keyvalpair in _updateDeleteTaxlotDict)
            {
                taxlotIDs.Add(keyvalpair.Key);
            }
            return taxlotIDs;
        }

        public virtual void SetAndCalculateAccruedInterest(TaxLot taxlot)
        {
            taxlot.AccruedInterest = (AccruedInterest * taxlot.Percentage) / 100;
        }

        public virtual void RemoveDeletedTaxlotsFromResetDictionary()
        {
            List<string> deletedTaxlots = new List<string>();
            foreach (KeyValuePair<string, TaxLot> keyValPair in _updateDeleteTaxlotDict)
            {
                if (keyValPair.Value.TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                {
                    deletedTaxlots.Add(keyValPair.Key);
                }
            }
            foreach (string taxlotID in deletedTaxlots)
            {
                _updateDeleteTaxlotDict.Remove(taxlotID);
            }
        }

        public virtual void ResetTheResetDictionary(TaxLot taxlot)
        {
            if (_updateDeleteTaxlotDict.ContainsKey(taxlot.TaxLotID))
            {
                if (_updateDeleteTaxlotDict[taxlot.TaxLotID].TaxLotState == ApplicationConstants.TaxLotState.Deleted)
                {
                    _updateDeleteTaxlotDict[taxlot.TaxLotID].TaxLotState = ApplicationConstants.TaxLotState.NotChanged;
                }
            }
            else
            {
                _updateDeleteTaxlotDict.Add(taxlot.TaxLotID, taxlot);
            }
        }

        public virtual void UpdateResetDictionaryWithDeletedState()
        {
            if (_updateDeleteTaxlotDict.ContainsKey(_groupID))
            {
                if (_updateDeleteTaxlotDict[_groupID].TaxLotState == ApplicationConstants.TaxLotState.New)
                {
                    _updateDeleteTaxlotDict.Remove(_groupID);
                }
                else
                {
                    _updateDeleteTaxlotDict[_groupID].TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                }
            }
        }
        public virtual void ResetTaxlotDictionaryState(ApplicationConstants.TaxLotState taxlotstate)
        {
            foreach (KeyValuePair<string, TaxLot> keyvalpair in _updateDeleteTaxlotDict)
            {
                keyvalpair.Value.TaxLotState = taxlotstate;
            }
        }

        [Browsable(false)]
        public virtual void ResetTaxlotDictionary(List<TaxLot> paramTaxlots, bool isReallocatedFromBlotter = false)
        {
            // if grouped in deleted(Ungrouped)  then set all taxlots to Deleted
            if (_persistenceStatus == ApplicationConstants.PersistenceStatus.UnGrouped)
            {
                // if unallocated then create a taxlot and set it's taxlot to be deleted
                if (_state == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED)
                {
                    if (_updateDeleteTaxlotDict.Count == 0)
                    {
                        foreach (TaxLot taxlot in paramTaxlots)
                        {
                            _updateDeleteTaxlotDict.Add(taxlot.TaxLotID, taxlot);
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, TaxLot> keyvalpair in _updateDeleteTaxlotDict)
                        {
                            keyvalpair.Value.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                        }
                    }
                }
                else
                {
                    foreach (KeyValuePair<string, TaxLot> keyvalpair in _updateDeleteTaxlotDict)
                    {
                        keyvalpair.Value.TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                    }
                }
                return;
            }

            ICollection<String> orig = new List<string>();
            foreach (KeyValuePair<string, TaxLot> keyvalpair in _updateDeleteTaxlotDict)
            {
                orig.Add(keyvalpair.Key);
            }

            foreach (TaxLot taxlot in paramTaxlots)
            {
                if (!_updateDeleteTaxlotDict.ContainsKey(taxlot.TaxLotID))
                {
                    _updateDeleteTaxlotDict.Add(taxlot.TaxLotID, taxlot);
                }
                else
                {
                    TaxLot updatedTaxlot = _updateDeleteTaxlotDict[taxlot.TaxLotID];
                    ApplicationConstants.TaxLotState taxlotExistingState = updatedTaxlot.TaxLotState;
                    _updateDeleteTaxlotDict[taxlot.TaxLotID] = taxlot;
                    if (taxlotExistingState != ApplicationConstants.TaxLotState.Deleted && updatedTaxlot.TaxLotState != taxlot.TaxLotState)
                    {
                        taxlot.TaxLotState = taxlotExistingState;
                    }
                    if (updatedTaxlot.TaxLotState != ApplicationConstants.TaxLotState.New)
                    {
                        taxlot.TaxLotState = ApplicationConstants.TaxLotState.Updated;
                    }
                }
                if (orig.Contains(taxlot.TaxLotID))
                {
                    orig.Remove(taxlot.TaxLotID);
                }
            }
            foreach (String taxlotID in orig)
            {
                if (_updateDeleteTaxlotDict.ContainsKey(taxlotID))
                {
                    if (!isReallocatedFromBlotter && _updateDeleteTaxlotDict[taxlotID].TaxLotState == ApplicationConstants.TaxLotState.New)
                    {
                        _updateDeleteTaxlotDict.Remove(taxlotID);
                    }
                    else
                    {
                        _updateDeleteTaxlotDict[taxlotID].TaxLotState = ApplicationConstants.TaxLotState.Deleted;
                    }
                }
            }
        }

        protected virtual void CalculateTotalFees()
        {
            if (_level1Collection != null)
            {
                _otherBrokerfees = 0.0;
                _clearingBrokerFee = 0.0;
                if (_taxLots != null)
                {
                    foreach (TaxLot taxLot in _taxLots)
                    {
                        _otherBrokerfees += taxLot.OtherBrokerFees;
                        _clearingBrokerFee += taxLot.ClearingBrokerFee;
                    }
                }
                _isCommissionCalculated = false;
            }
        }

        public virtual void RecalculateGroupCommissionFromTaxlotComm()
        {
            if (_commissionCalculationTime.Equals(true))
            {
                double new_commission = 0;
                double new_SoftCommission = 0;
                foreach (TaxLot taxLot in _taxLots)
                {
                    new_commission += taxLot.Commission;
                    new_SoftCommission += taxLot.SoftCommission;
                }
                Commission = new_commission;
                SoftCommission = new_SoftCommission;
                _isCommissionCalculated = false;
            }
        }

        public virtual void RecalculateGroupFeesFromTaxlotComm()
        {
            if (_commissionCalculationTime.Equals(true))
            {
                double new_Fees = 0;
                double new_Fees_for_clearingBrokerFee = 0;
                foreach (TaxLot taxLot in _taxLots)
                {
                    new_Fees += taxLot.OtherBrokerFees;
                    new_Fees_for_clearingBrokerFee += taxLot.ClearingBrokerFee;
                }
                _otherBrokerfees = new_Fees;
                _clearingBrokerFee = new_Fees_for_clearingBrokerFee;
                _isCommissionCalculated = false;
            }
        }
        protected virtual void CalculateTotalCommission()
        {
            Commission = 0.0;
            SoftCommission = 0.0;
            if (_taxLots != null)
            {
                foreach (TaxLot taxLot in _taxLots)
                {
                    Commission += taxLot.Commission;
                    SoftCommission += taxLot.SoftCommission;
                }
                _isCommissionCalculated = false;
            }
        }

        public virtual bool IsCommissionCalculated
        {
            get { return _isCommissionCalculated; }
            set { _isCommissionCalculated = value; }
        }
        #endregion

        public virtual string ValidateAllocationAccounts(AllocationGroup allocationGroup, ref AllocationLevelList newaccounts)
        {
            try
            {
                bool _shouldAllowFractionalValues = PostTradeHelper.ISFractionalAllocationAllowed(allocationGroup.AssetID);

                if (allocationGroup.State == PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED && allocationGroup.AllocationDate != Prana.BusinessObjects.DateTimeConstants.MinValue)
                {
                    return "This Group is already Allocated!";
                }

                double allowedqty = allocationGroup.CumQty;
                double accountsQty = 0.0;
                double accountsPercentage = 0.0;
                double remainingQty = 0;
                List<AllocationLevelClass> accountsToRemove = new List<AllocationLevelClass>();
                foreach (AllocationLevelClass account in newaccounts.Collection)
                {
                    accountsQty += account.AllocatedQty;
                    accountsPercentage += account.Percentage;
                }

                accountsPercentage = Convert.ToSingle(Math.Round(accountsPercentage, 0));
                if (accountsPercentage != 100.0)
                {
                    return "Sum of Accounts percentage should be 100";
                }
                if (accountsQty != 0 && accountsQty != allowedqty)
                {
                    AllocationLevelClass accountToAdj = newaccounts.Collection[0];
                    remainingQty = allowedqty - accountsQty;
                    if (remainingQty > 0)
                    {
                        accountToAdj.AllocatedQty = accountToAdj.AllocatedQty + remainingQty;
                        remainingQty = 0;
                        accountToAdj.Percentage = Convert.ToSingle((accountToAdj.AllocatedQty * 100.0) / allowedqty);
                        if (accountToAdj.Childs != null)
                        {
                            foreach (AllocationLevelClass strategy in accountToAdj.Childs.Collection)
                            {
                                strategy.AllocatedQty = (strategy.Percentage * accountToAdj.AllocatedQty) / 100.0;
                                if (!_shouldAllowFractionalValues)
                                {
                                    strategy.AllocatedQty = Convert.ToInt64(strategy.AllocatedQty);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = newaccounts.Collection.Count - 1; i >= 0; i--)
                        {
                            AllocationLevelClass accountToAdjNew = newaccounts.Collection[i];
                            if (remainingQty == 0)
                            {
                                break;
                            }
                            else
                            {
                                accountToAdjNew.AllocatedQty--;
                                remainingQty++;
                            }
                        }
                    }
                }
                foreach (AllocationLevelClass account in newaccounts.Collection)
                {
                    if (account.AllocatedQty < 0)
                    {
                        accountsToRemove.Add(account);
                    }
                }

                // remove accounts having zero qty
                foreach (AllocationLevelClass account in accountsToRemove)
                {
                    newaccounts.Collection.Remove(account);
                }
                foreach (AllocationLevelClass account in newaccounts.Collection)
                {
                    string strategyResult = CheckStrategyQtyPerInAccounts(account);
                    if (strategyResult != string.Empty)
                    {
                        return strategyResult;
                    }
                }
                if (remainingQty != 0)
                {
                    return "Can not allocate more than cum qty";
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
            return string.Empty;
        }
        private string CheckStrategyQtyPerInAccounts(AllocationLevelClass account)
        {
            double strategyQty = 0.0;
            if (account.Childs != null)
            {
                float strategyPercentage = 0;
                foreach (AllocationLevelClass strategy in account.Childs.Collection)
                {
                    strategyQty += strategy.AllocatedQty;
                    strategyPercentage += strategy.Percentage;
                }
                strategyPercentage = Convert.ToSingle(Math.Round(strategyPercentage, 0));
                if (strategyPercentage != 100.0)
                {
                    return "Sum of Strategy percentage should be 100";
                }

                if (strategyQty != account.AllocatedQty)
                {
                    AllocationLevelClass strategyAdj = account.Childs.Collection[0];
                    strategyAdj.AllocatedQty = strategyAdj.AllocatedQty + account.AllocatedQty - strategyQty;
                    strategyAdj.Percentage = Convert.ToSingle((strategyAdj.AllocatedQty * 100.0) / account.AllocatedQty);
                }
                // delete zero qty strategies
                List<AllocationLevelClass> deletedStrategies = new List<AllocationLevelClass>();
                foreach (AllocationLevelClass strategy in account.Childs.Collection)
                {
                    if (strategy.AllocatedQty == 0)
                    {
                        deletedStrategies.Add(strategy);
                    }
                }
                foreach (AllocationLevelClass strategy in deletedStrategies)
                {
                    account.Childs.Collection.Remove(strategy);
                }
                foreach (AllocationLevelClass strategy in account.Childs.Collection)
                {
                    if (account.AllocatedQty > 0)
                    {
                        strategy.Percentage = Convert.ToSingle(Math.Round(100 * (strategy.AllocatedQty) / account.AllocatedQty, 0));
                    }
                }
            }
            return string.Empty;
        }

        public virtual AllocationGroup CloneDates()
        {
            AllocationGroup clone = new AllocationGroup();
            clone.AUECLocalDate = _aUECLocalDate;
            clone.AllocationDate = _allocationDate;
            clone.ProcessDate = _processDate;
            clone.OriginalPurchaseDate = _originalPurchaseDate;
            return clone;
        }

        public virtual void UpdateGroupPersistenceStatus()
        {
            if (_persistenceStatus == ApplicationConstants.PersistenceStatus.NotChanged)
            {
                _persistenceStatus = ApplicationConstants.PersistenceStatus.ReAllocated;
            }
        }

        /// <summary>
        /// Adds the action to the taxlots present in _updateDeleteTaxlotDict
        /// </summary>
        /// <param name="action">action performed of type TradeAuditActionType.ActionType</param>
        public virtual void AddTradeAuditActionToUpdateDeleteTaxlots(TradeAuditActionType.ActionType action)
        {
            try
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    if (_updateDeleteTaxlotDict.ContainsKey(childTaxlot.TaxLotID))
                    {
                        childTaxlot.AddTradeAction(action);
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
        /// Adds the action to all the taxlots 
        /// </summary>
        /// <param name="action">action performed of type TradeAuditActionType.ActionType</param>
        public virtual void AddTradeAuditActionToAllTaxlots(TradeAuditActionType.ActionType action)
        {
            try
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    childTaxlot.AddTradeAction(action);
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

        public virtual void UpdateGroupTaxlots(string conversionOperator, string conversionOperatorValue)
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                if (_updateDeleteTaxlotDict.ContainsKey(childTaxlot.TaxLotID))
                {
                    childTaxlot.AvgPrice = _avgPrice;
                    childTaxlot.CounterPartyID = _counterPartyID;
                    childTaxlot.CounterPartyName = _counterPartyName;
                    childTaxlot.VenueID = _venueID;
                    childTaxlot.AUECLocalDate = _aUECLocalDate;
                    childTaxlot.ProcessDate = _processDate;
                    childTaxlot.OriginalPurchaseDate = _originalPurchaseDate;
                    childTaxlot.SettlementDate = _settlementDate;
                    childTaxlot.FXRate = _avgFXRateForTrade;
                    childTaxlot.Description = _description;
                    childTaxlot.InternalComments = _internalComments;
                    childTaxlot.TransactionType = _transactionType;
                    childTaxlot.SettlementCurrencyID = _settlementCurrencyID;

                    if (conversionOperator.Equals("FXConversionMethodOperator"))
                    {
                        childTaxlot.FXConversionMethodOperator = conversionOperatorValue;
                    }
                    TaxLot innerTaxlot = _updateDeleteTaxlotDict[childTaxlot.TaxLotID];
                    ApplicationConstants.TaxLotState preTaxlotState = innerTaxlot.TaxLotState;
                    innerTaxlot = childTaxlot;
                    if (preTaxlotState != ApplicationConstants.TaxLotState.New)
                    {
                        innerTaxlot.TaxLotState = ApplicationConstants.TaxLotState.Updated;
                    }
                    _updateDeleteTaxlotDict[childTaxlot.TaxLotID] = innerTaxlot;
                }
            }
        }

        public virtual void UpdateTaxlotFXRate()
        {

            foreach (TaxLot childTaxlot in _taxLots)
            {
                if (_updateDeleteTaxlotDict.ContainsKey(childTaxlot.TaxLotID))
                {
                    _updateDeleteTaxlotDict[childTaxlot.TaxLotID].FXRate = _avgFXRateForTrade;
                    childTaxlot.FXRate = _avgFXRateForTrade;

                    ApplicationConstants.TaxLotState preTaxlotState = _updateDeleteTaxlotDict[childTaxlot.TaxLotID].TaxLotState;
                    if (preTaxlotState != ApplicationConstants.TaxLotState.New)
                    {
                        _updateDeleteTaxlotDict[childTaxlot.TaxLotID].TaxLotState = ApplicationConstants.TaxLotState.Updated;
                    }
                }
            }
        }

        /// <summary>
        /// Update Taxlot Settlement Currency 
        /// </summary>
        public virtual void UpdateTaxlotSettlCurrency()
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                if (_updateDeleteTaxlotDict.ContainsKey(childTaxlot.TaxLotID))
                {
                    _updateDeleteTaxlotDict[childTaxlot.TaxLotID].SettlementCurrencyID = _settlementCurrencyID;
                    childTaxlot.SettlementCurrencyID = _settlementCurrencyID;

                    ApplicationConstants.TaxLotState preTaxlotState = _updateDeleteTaxlotDict[childTaxlot.TaxLotID].TaxLotState;
                    if (preTaxlotState != ApplicationConstants.TaxLotState.New)
                    {
                        _updateDeleteTaxlotDict[childTaxlot.TaxLotID].TaxLotState = ApplicationConstants.TaxLotState.Updated;
                    }
                }
            }
        }
        /// <summary>
        /// this function is used to update conversionOperator for all the taxlots
        /// </summary>
        /// <param name="conversionOperator"></param>
        public virtual void UpdateTaxlotFXConversionMethodOperator(string conversionOperator)
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                if (_updateDeleteTaxlotDict.ContainsKey(childTaxlot.TaxLotID))
                {
                    _updateDeleteTaxlotDict[childTaxlot.TaxLotID].FXConversionMethodOperator = conversionOperator;
                    childTaxlot.FXConversionMethodOperator = conversionOperator;

                    ApplicationConstants.TaxLotState preTaxlotState = _updateDeleteTaxlotDict[childTaxlot.TaxLotID].TaxLotState;
                    if (preTaxlotState != ApplicationConstants.TaxLotState.New)
                    {
                        _updateDeleteTaxlotDict[childTaxlot.TaxLotID].TaxLotState = ApplicationConstants.TaxLotState.Updated;
                    }
                }
            }
        }

        /// <summary>
        /// this method is used to update FX Rate and ConversionOperator for selected taxlot
        /// </summary>
        /// <param name="FXRate"></param>
        /// <param name="conversionOperator"></param>
        /// <param name="taxlotID"></param>
        public virtual void UpdateTaxlotFXRateAndOperator(double FXRate, string conversionOperator, string taxlotID)
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                if (_updateDeleteTaxlotDict.ContainsKey(taxlotID) && childTaxlot.TaxLotID.Equals(taxlotID))
                {
                    _updateDeleteTaxlotDict[taxlotID].FXRate = FXRate;
                    childTaxlot.FXRate = FXRate;
                    _updateDeleteTaxlotDict[taxlotID].FXConversionMethodOperator = conversionOperator;
                    childTaxlot.FXConversionMethodOperator = conversionOperator;

                    ApplicationConstants.TaxLotState preTaxlotState = _updateDeleteTaxlotDict[childTaxlot.TaxLotID].TaxLotState;
                    if (preTaxlotState != ApplicationConstants.TaxLotState.New)
                    {
                        _updateDeleteTaxlotDict[childTaxlot.TaxLotID].TaxLotState = ApplicationConstants.TaxLotState.Updated;
                    }
                }
            }
        }

        public virtual void UpdateTaxlotTradeAttributes(TradeAttributes attributes, string taxlotID)
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                if (_updateDeleteTaxlotDict.ContainsKey(taxlotID) && childTaxlot.TaxLotID.Equals(taxlotID))
                {
                    if (attributes.TradeAttribute1 != string.Empty)
                    {
                        _updateDeleteTaxlotDict[taxlotID].TradeAttribute1 = attributes.TradeAttribute1;
                        childTaxlot.TradeAttribute1 = attributes.TradeAttribute1;
                    }
                    if (attributes.TradeAttribute2 != string.Empty)
                    {
                        _updateDeleteTaxlotDict[taxlotID].TradeAttribute2 = attributes.TradeAttribute2;
                        childTaxlot.TradeAttribute2 = attributes.TradeAttribute2;
                    }
                    if (attributes.TradeAttribute3 != string.Empty)
                    {
                        _updateDeleteTaxlotDict[taxlotID].TradeAttribute3 = attributes.TradeAttribute3;
                        childTaxlot.TradeAttribute3 = attributes.TradeAttribute3;
                    }
                    if (attributes.TradeAttribute4 != string.Empty)
                    {
                        _updateDeleteTaxlotDict[taxlotID].TradeAttribute4 = attributes.TradeAttribute4;
                        childTaxlot.TradeAttribute4 = attributes.TradeAttribute4;
                    }
                    if (attributes.TradeAttribute5 != string.Empty)
                    {
                        _updateDeleteTaxlotDict[taxlotID].TradeAttribute5 = attributes.TradeAttribute5;
                        childTaxlot.TradeAttribute5 = attributes.TradeAttribute5;
                    }
                    if (attributes.TradeAttribute6 != string.Empty)
                    {
                        _updateDeleteTaxlotDict[taxlotID].TradeAttribute6 = attributes.TradeAttribute6;
                        childTaxlot.TradeAttribute6 = attributes.TradeAttribute6;
                    }

                    _updateDeleteTaxlotDict[taxlotID].SetNonEmptyTradeAttributes(attributes.GetTradeAttributesAsDict());
                    childTaxlot.SetNonEmptyTradeAttributes(attributes.GetTradeAttributesAsDict());
                  
                    ApplicationConstants.TaxLotState preTaxlotState = _updateDeleteTaxlotDict[childTaxlot.TaxLotID].TaxLotState;
                    if (preTaxlotState != ApplicationConstants.TaxLotState.New)
                    {
                        _updateDeleteTaxlotDict[childTaxlot.TaxLotID].TaxLotState = ApplicationConstants.TaxLotState.Updated;
                    }
                }
            }
        }

        public virtual void UpdateTaxlotTradeAttributes(TradeAttributes attributes)
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                childTaxlot.TradeAttribute1 = attributes.TradeAttribute1;
                childTaxlot.TradeAttribute2 = attributes.TradeAttribute2;
                childTaxlot.TradeAttribute3 = attributes.TradeAttribute3;
                childTaxlot.TradeAttribute4 = attributes.TradeAttribute4;
                childTaxlot.TradeAttribute5 = attributes.TradeAttribute5;
                childTaxlot.TradeAttribute6 = attributes.TradeAttribute6;
                childTaxlot.SetTradeAttribute(attributes.GetTradeAttributesAsDict());
                UpdateTaxlotState(childTaxlot);
            }
        }

        public virtual void UpdateTaxlotTransactionType(string transactionType)
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                childTaxlot.TransactionType = transactionType;
                UpdateTaxlotState(childTaxlot);
            }
        }

        public virtual void UpdateTaxlotState(TaxLot taxlot)
        {
            if (_updateDeleteTaxlotDict.ContainsKey(taxlot.TaxLotID))
            {
                TaxLot innerTaxlot = _updateDeleteTaxlotDict[taxlot.TaxLotID];
                ApplicationConstants.TaxLotState preTaxlotState = innerTaxlot.TaxLotState;
                innerTaxlot = Prana.Global.Utilities.DeepCopyHelper.Clone(taxlot);
                if (preTaxlotState != ApplicationConstants.TaxLotState.New)
                {
                    innerTaxlot.TaxLotState = ApplicationConstants.TaxLotState.Updated;
                }
                _updateDeleteTaxlotDict[taxlot.TaxLotID] = innerTaxlot;
            }
        }

        /// <summary>
        /// Updates the group order with the values from the group.
        /// </summary>
        /// <param name="group"></param>
        public virtual void UpdateGroupOrder(AllocationGroup group)
        {
            try
            {
                if (group.Orders.Count == 1)
                {
                    group.Orders[0].IsModified = true;
                    group.Orders[0].AvgPrice = group.AvgPrice;
                    group.Orders[0].CumQty = group.CumQty;
                    group.Orders[0].Description = group.Description;
                    group.Orders[0].InternalComments = group.InternalComments;
                    group.Orders[0].AUECLocalDate = group.AUECLocalDate;
                    group.Orders[0].OriginalPurchaseDate = group.OriginalPurchaseDate;
                    group.Orders[0].ProcessDate = group.ProcessDate;
                    group.Orders[0].SettlementDate = group.SettlementDate;
                    group.Orders[0].Venue = group.Venue;
                    group.Orders[0].VenueID = group.VenueID;
                    group.Orders[0].CounterPartyID = group.CounterPartyID;
                    group.Orders[0].CounterPartyName = group.CounterPartyName;
                    group.Orders[0].OrderSideTagValue = group.OrderSideTagValue;
                    group.Orders[0].OrderSide = group.OrderSide;
                    group.Orders[0].FXRate = group.FXRate;
                    group.Orders[0].FXConversionMethodOperator = group.FXConversionMethodOperator;
                    group.Orders[0].TradeAttribute1 = group.TradeAttribute1;
                    group.Orders[0].TradeAttribute2 = group.TradeAttribute2;
                    group.Orders[0].TradeAttribute3 = group.TradeAttribute3;
                    group.Orders[0].TradeAttribute4 = group.TradeAttribute4;
                    group.Orders[0].TradeAttribute5 = group.TradeAttribute5;
                    group.Orders[0].TradeAttribute6 = group.TradeAttribute6;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public virtual void UpdateTaxlotAvgPrice()
        {
            if (_persistenceStatus == ApplicationConstants.PersistenceStatus.ReAllocated)
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    if (_updateDeleteTaxlotDict.ContainsKey(childTaxlot.TaxLotID))
                    {
                        _updateDeleteTaxlotDict[childTaxlot.TaxLotID].AvgPrice = _avgPrice;
                        childTaxlot.AvgPrice = _avgPrice;
                        UpdateTaxlotState(childTaxlot);
                    }
                }
            }
        }

        public virtual void UpdateTaxlotAccruedInterest()
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                if (_updateDeleteTaxlotDict.ContainsKey(childTaxlot.TaxLotID))
                {
                    _updateDeleteTaxlotDict[childTaxlot.TaxLotID].AccruedInterest = _accruedInterest;
                    if (_allocatedQty != 0)
                    {
                        childTaxlot.AccruedInterest = _accruedInterest * childTaxlot.TaxLotQty / _allocatedQty;
                    }
                    UpdateTaxlotState(childTaxlot);
                }
            }
        }

        public virtual void UpdateTaxlotDescription()
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                if (_updateDeleteTaxlotDict.ContainsKey(childTaxlot.TaxLotID))
                {
                    _updateDeleteTaxlotDict[childTaxlot.TaxLotID].Description = _description;
                    childTaxlot.Description = _description;
                    UpdateTaxlotState(childTaxlot);
                }
            }
        }

        public virtual void UpdateTaxlotIneternalComments()
        {
            foreach (TaxLot childTaxlot in _taxLots)
            {
                if (_updateDeleteTaxlotDict.ContainsKey(childTaxlot.TaxLotID))
                {
                    _updateDeleteTaxlotDict[childTaxlot.TaxLotID].InternalComments = _internalComments;
                    childTaxlot.InternalComments = _internalComments;
                    UpdateTaxlotState(childTaxlot);
                }
            }
        }

        public virtual void ClearTaxlotDictionary()
        {
            _updateDeleteTaxlotDict.Clear();
        }

        string _errorMessage = string.Empty;
        public virtual string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public virtual double TotalCommission
        {
            get
            {
                return _commission + _softCommission;
            }
        }

        //Bharat Kumar Jangir (23 August 2013)
        // Note : Calculates hard commission per share
        //http://jira.nirvanasolutions.com:8080/browse/IGUANA-16
        public virtual double CommissionPerShare
        {
            get
            {
                if (_cumQty != 0)
                {
                    return Commission / _cumQty;
                }
                return 0;
            }
        }

        // Calculates soft commission per share
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-10587
        public virtual double SoftCommissionPerShare
        {
            get
            {
                if (_cumQty != 0)
                {
                    return SoftCommission / _cumQty;
                }
                return 0;
            }
        }
        public virtual double TotalCommissionPerShare
        {
            get
            {
                if (_cumQty != 0)
                {
                    return (_commission + _softCommission) / _cumQty;
                }
                return 0;
            }
        }

        //This property is added to change quantity of allocated taxlot,
        //If group have more than one taxlot then we cant update quantity of allocated trade
        [Browsable(false)]
        public virtual bool IsGroupAllocatedToOneTaxLot
        {
            get
            {
                if (_taxLots.Count == 1 && _taxLots[0].TaxLotQty == _cumQty)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        [Browsable(false)]
        bool _isAnotherTaxlotAttributesUpdated = false;
        public virtual bool IsAnotherTaxlotAttributesUpdated
        {
            get { return _isAnotherTaxlotAttributesUpdated; }
            set
            {
                _isAnotherTaxlotAttributesUpdated = value;
            }
        }

        private GroupAllocationStatus _groupAllocationStatus = GroupAllocationStatus.UnAllocated;
        /// <summary>
        /// This property is used when we allocate a group based on latest positions and check side
        /// </summary>
        [XmlIgnore]
        [Browsable(false)]
        public virtual GroupAllocationStatus GroupAllocationStatus
        {
            get { return _groupAllocationStatus; }
            set { _groupAllocationStatus = value; }
        }

        private int _accountID = 0;
        [XmlIgnore]
        [Browsable(false)]
        public virtual int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private int _strategyID = 0;
        [XmlIgnore]
        [Browsable(false)]
        public virtual int StrategyID
        {
            get { return _strategyID; }
            set { _strategyID = value; }
        }

        private ClosingStatus _closingStatus = ClosingStatus.Open;
        public virtual ClosingStatus ClosingStatus
        {
            get { return _closingStatus; }
            set { _closingStatus = value; }
        }

        private String _closingAlgoText = PostTradeEnums.CloseTradeAlogrithm.NONE.ToString();
        public virtual String ClosingAlgoText
        {
            get { return _closingAlgoText; }
            set { _closingAlgoText = value; }
        }

        private PostTradeEnums.Status _groupStatus = PostTradeEnums.Status.None;
        public virtual PostTradeEnums.Status GroupStatus
        {
            get { return _groupStatus; }
            set { _groupStatus = value; }
        }

        #region IKeyable Members
        public virtual string GetKey()
        {
            return _groupID;
        }

        public virtual void Update(IKeyable item)
        {
            AllocationGroup newGrp = (AllocationGroup)item;

            //Update basic group details
            _quantity = newGrp.Quantity;
            _cumQty = newGrp.CumQty;
            _avgPrice = newGrp.AvgPrice;
            _orderSide = newGrp._orderSide;
            _aUECLocalDate = newGrp._aUECLocalDate;
            _counterPartyName = newGrp.CounterPartyName;
            _settlementDate = newGrp._settlementDate;
            _otherBrokerfees = newGrp._otherBrokerfees;
            _venue = newGrp.Venue;
            _processDate = newGrp._processDate;
            _originalPurchaseDate = newGrp._originalPurchaseDate;
            _description = newGrp._description;
            _internalComments = newGrp._internalComments;
            _accruedInterest = newGrp._accruedInterest;
            _changeType = newGrp.ChangeType;

            //Update FX and Settlement currency fields
            _avgFXRateForTrade = newGrp._avgFXRateForTrade;
            FXConversionMethodOperator = newGrp.FXConversionMethodOperator;
            _settlementCurrencyID = newGrp._settlementCurrencyID;

            //Update commission and fees
            UpdateCommissionAndFees(newGrp);

            //Update Trade Attributes
            UpdateTradeAttributes(newGrp);

            //Update Edit Trade fields, PRANA-15942
            _persistenceStatus = newGrp.PersistenceStatus;
            _isAnotherTaxlotAttributesUpdated = newGrp.IsAnotherTaxlotAttributesUpdated;
            _userID = newGrp.CompanyUserID;
            _isModified = newGrp.IsModified;
        }

        public virtual void UpdateTradeAttributes(AllocationGroup newGrp)
        {
            //added to update trade attributes while updating a group, PRANA-15709
            #region TradeAttributes
            _tradeAttribute1 = newGrp.TradeAttribute1;
            _tradeAttribute2 = newGrp.TradeAttribute2;
            _tradeAttribute3 = newGrp.TradeAttribute3;
            _tradeAttribute4 = newGrp.TradeAttribute4;
            _tradeAttribute5 = newGrp.TradeAttribute5;
            _tradeAttribute6 = newGrp.TradeAttribute6;
            SetTradeAttribute(newGrp.GetTradeAttributesAsDict());
            #endregion
        }

        public virtual void UpdateCommissionAndFees(AllocationGroup newGrp)
        {
            Commission = newGrp.Commission;
            SoftCommission = newGrp.SoftCommission;
            _otherBrokerfees = newGrp.OtherBrokerFees;
            _clearingBrokerFee = newGrp.ClearingBrokerFee;
            _stampDuty = newGrp.StampDuty;
            _miscFees = newGrp.MiscFees;
            _taxOnCommissions = newGrp.TaxOnCommissions;
            _transactionLevy = newGrp.TransactionLevy;
            _clearingFee = newGrp.ClearingFee;
            _secFee = newGrp.SecFee;
            _occFee = newGrp.OccFee;
            _orfFee = newGrp.OrfFee;
            _commSource = newGrp.CommSource;
            _softCommSource = newGrp.SoftCommSource;
            _commissionSource = newGrp.CommissionSource;
            _softCommissionSource = newGrp.SoftCommissionSource;
            _isRecalculateCommission = newGrp.IsRecalculateCommission;
        }

        public virtual void UpdateSecFee(AllocationGroup newGrp)
        {
            try
            {
                _secFee = newGrp.SecFee;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public virtual void UpdateSecFeeAtTaxlotLevel(AllocationGroup allocationGroup)
        {
            try
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    TaxLot newTaxlot = allocationGroup.TaxLots.Find(x => x.TaxLotID == childTaxlot.TaxLotID);
                    childTaxlot.SecFee = newTaxlot.SecFee;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region INotifyPropertyChangedCustom Members
        [field: NonSerialized]
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public virtual void PropertyHasChanged()
        {
            if (PropertyChanged != null)
            {
                // Passed new PropertyChangedEventArgs instead of null to avoid object reference null exception
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-8204
                PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
            }
            TaxLots.ForEach(taxlot => taxlot.PropertyHasChanged());
            Orders.ForEach(order => order.PropertyHasChanged());
        }
        #endregion

        #region Allocation Scheme members


        private int _allocationSchemeID = 0;
        [Browsable(false)]
        public virtual int AllocationSchemeID
        {
            set
            { _allocationSchemeID = value; }
            get { return _allocationSchemeID; }
        }

        private string _allocationSchemeName = string.Empty;
        public virtual string AllocationSchemeName
        {
            set { _allocationSchemeName = value; }
            get { return _allocationSchemeName; }
        }
        #endregion

        public virtual void UpdateTaxLotDataforCumQty(AllocationGroup gParent)
        {
            //Here we are assuming that one group will belong to one taxlot
            foreach (TaxLot childTaxlot in _taxLots)
            {
                childTaxlot.TaxLotQty = gParent.CumQty;
                childTaxlot.ExecutedQty = gParent.CumQty;
                UpdateTaxlotState(childTaxlot);
            }
        }

        public virtual void UpdateTaxLotDataforDate(AllocationGroup gParent)
        {
            //Here we are assuming that one group will belong to one taxlot
            foreach (TaxLot childTaxlot in _taxLots)
            {
                childTaxlot.AUECModifiedDate = gParent.AUECLocalDate;
                childTaxlot.NirvanaProcessDate = gParent.NirvanaProcessDate;
                UpdateTaxlotState(childTaxlot);
            }
        }

        //Added By Faisal Shah 04/09/14
        private string _navLockStatus = string.Empty;
        public virtual string NavLockStatus
        {
            get { return _navLockStatus; }
            set { _navLockStatus = value; }
        }

        protected internal virtual void updateInternalComments()
        {
            List<string> comments = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (AllocationOrder aOrder in _orders)
            {
                string comment = aOrder.InternalComments;
                if (comment != null && comment != string.Empty && !comments.Contains(comment))
                {
                    comments.Add(comment);
                    if (sb.Length > 0)
                    {
                        sb.Append(" * ");
                    }
                    sb.Append(comment);
                }
            }
            _internalComments = sb.ToString();
        }

        /// <summary>
        /// Adds a new tax lot or updates the existing one in the update/delete dictionary.
        /// </summary>
        /// <param name="key">The unique identifier for the tax lot.</param>
        /// <param name="taxLot">The tax lot object to add or update.</param>
        public virtual void AddOrUpdateTaxLot(string key, TaxLot taxLot)
        {
            if (_updateDeleteTaxlotDict == null)
                _updateDeleteTaxlotDict = new Dictionary<string, TaxLot>();

            _updateDeleteTaxlotDict[key] = taxLot;  // Add if not present, else update
        }

        /// <summary>
        /// Calculates taxlot percentage to exact percision digit
        /// </summary>
        /// <param name="taxLotQty">Taxlot qty</param>
        /// <returns>taxlot percentage to exact percision digit</returns>
        private decimal CalculateTaxlotPercentage(double taxLotQty)
        {
            try
            {
                return ((decimal)taxLotQty / (decimal)this.CumQty) * 100;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return 0.0M;
            }
        }
        /// <summary>
        /// Update SettlementCurrency In Taxlots for third party amendmaned
        /// </summary>
        /// <param name="gParent"></param>
        public virtual void UpdateSettlementCurrencyInTaxlots(AllocationGroup gParent)
        {
            try
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    childTaxlot.SettlementCurrencyID = gParent.SettlementCurrencyID;
                    UpdateTaxlotState(childTaxlot);
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
        /// Updates the commission and fees at taxlot level.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        /// TODO: Need to move this code in Taxlot class
        public virtual void UpdateCommissionAndFeesAtTaxlotLevel(AllocationGroup allocationGroup)
        {
            try
            {
                foreach (TaxLot childTaxlot in _taxLots)
                {
                    TaxLot newTaxlot = allocationGroup.TaxLots.Find(x => x.TaxLotID == childTaxlot.TaxLotID);

                    childTaxlot.Commission = newTaxlot.Commission;
                    childTaxlot.SoftCommission = newTaxlot.SoftCommission;
                    childTaxlot.OtherBrokerFees = newTaxlot.OtherBrokerFees;
                    childTaxlot.ClearingBrokerFee = newTaxlot.ClearingBrokerFee;
                    childTaxlot.StampDuty = newTaxlot.StampDuty;
                    childTaxlot.MiscFees = newTaxlot.MiscFees;
                    childTaxlot.TaxOnCommissions = newTaxlot.TaxOnCommissions;
                    childTaxlot.TransactionLevy = newTaxlot.TransactionLevy;
                    childTaxlot.ClearingFee = newTaxlot.ClearingFee;
                    childTaxlot.SecFee = newTaxlot.SecFee;
                    childTaxlot.OccFee = newTaxlot.OccFee;
                    childTaxlot.OrfFee = newTaxlot.OrfFee;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #region IsSelected property for maintaining selection in WPF, PRANA-15780

        /// <summary>
        /// The _isSelected property for maintaining selection in WPF
        /// </summary>
        private bool _isSelected = false;

        /// <summary>
        /// Gets or sets a value indicating whether the group is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if group is selected by user; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
        }
        #endregion

        #region IFilterable Members

        public virtual DateTime GetDate()
        {
            return this.ProcessDate;
        }

        public virtual DateTime GetDateModified()
        {
            return this.AUECLocalDate;
        }

        public virtual string GetSymbol()
        {
            return this.Symbol;
        }

        public virtual int GetAccountID()
        {
            return this.AccountID;
        }

        #endregion

        public virtual bool IsTaxlotDictChanged()
        {
            return _updateDeleteTaxlotDict != null && _updateDeleteTaxlotDict.Any(x =>
                x.Value.TaxLotState == ApplicationConstants.TaxLotState.New || x.Value.TaxLotState == ApplicationConstants.TaxLotState.Deleted);
        }
    }
}