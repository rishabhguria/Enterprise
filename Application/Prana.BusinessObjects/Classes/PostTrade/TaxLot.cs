using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class TaxLot : PranaBasicMessage, IKeyable, IFilterable, INotifyPropertyChangedCustom
    {
        private double _taxLotQty;
        private double _taxLotQtyToClose;
        private int _level1ID;
        private int _level2ID = 0;
        private float _percentage = 100;
        private float _l2percentage = 100;
        private string _level1Name = string.Empty;
        private string _level2Name = string.Empty;
        private string _masterFund = string.Empty;
        private string _taxlotID = string.Empty;
        private ApplicationConstants.TaxLotState _taxLotState = ApplicationConstants.TaxLotState.New;
        protected string _underlyingSymbol;
        private ClosingStatus _closingStatus = ClosingStatus.Open;
        private string _navLockStatus = string.Empty;
        private string _tif = string.Empty;
        private int _sideRank = 0;
        // Added _taxlotPk and _parentRowPk to store row Ids to taxlot and its parent taxlot row
        // http: //jira.nirvanasolutions.com:8080/browse/PRANA-7378
        private long _taxlotPk = 0;

        private long _parentRowPk = 0;

        /// <summary>
        /// list of tradeActions performed on the taxlot. Used only for edit type actions. Add only
        /// through the function do not expose.
        /// </summary>
        private List<TradeAuditActionType.ActionType> _tradeActions = new List<TradeAuditActionType.ActionType>();

        #region Dynamic-UDA

        private string _analyst = ApplicationConstants.CONST_UNDEFINED;
        private string _countryOfRisk = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA1 = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA2 = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA3 = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA4 = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA5 = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA6 = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA7 = ApplicationConstants.CONST_UNDEFINED;
        private string _issuer = ApplicationConstants.CONST_UNDEFINED;
        private string _liquidTag = ApplicationConstants.CONST_UNDEFINED;
        private string _marketCap = ApplicationConstants.CONST_UNDEFINED;
        private string _region = ApplicationConstants.CONST_UNDEFINED;
        private string _riskCurrency = ApplicationConstants.CONST_UNDEFINED;
        private string _ucitsEligibleTag = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA8 = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA9 = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA10 = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA11 = ApplicationConstants.CONST_UNDEFINED;
        private string _customUDA12 = ApplicationConstants.CONST_UNDEFINED;
        #endregion Dynamic-UDA

        /// <summary>
        /// Adds the AuditTrailTradeAction to the _tradeActions list in the TaxLot object
        /// </summary>
        /// <param name="action"></param>
        public virtual void AddTradeAction(TradeAuditActionType.ActionType action)
        {
            try
            {
                if (!_tradeActions.Contains(action))
                    _tradeActions.Add(action);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public TaxLot()
        {
            SetBasicDetails();
        }

        public TaxLot(AllocationGroup group)
        {
            SetBasicDetails();
            CopyExternalDetails(group);
            this.CopyBasicDetails(group);
        }

        public TaxLot(DataRow dr)
        {
            //SetBasicDetails();
            _taxLotState = ApplicationConstants.TaxLotState.New;
            //CopyBasicDetails(group);
            if (!string.IsNullOrEmpty(dr["TaxLotID"].ToString()))
                _taxlotID = dr["TaxLotID"].ToString();
            if (!string.IsNullOrEmpty(dr["Level2ID"].ToString()))
                _level2ID = Convert.ToInt32(dr["Level2ID"]);
            if (!string.IsNullOrEmpty(dr["TaxLotQty"].ToString()))
                _taxLotQty = Convert.ToDouble(dr["TaxLotQty"]);
            if (!string.IsNullOrEmpty(dr["L2TradeAttribute1"].ToString()))
                _tradeAttribute1 = dr["L2TradeAttribute1"].ToString();
            if (!string.IsNullOrEmpty(dr["L2TradeAttribute2"].ToString()))
                _tradeAttribute2 = dr["L2TradeAttribute2"].ToString();
            if (!string.IsNullOrEmpty(dr["L2TradeAttribute3"].ToString()))
                _tradeAttribute3 = dr["L2TradeAttribute3"].ToString();
            if (!string.IsNullOrEmpty(dr["L2TradeAttribute4"].ToString()))
                _tradeAttribute4 = dr["L2TradeAttribute4"].ToString();
            if (!string.IsNullOrEmpty(dr["L2TradeAttribute5"].ToString()))
                _tradeAttribute5 = dr["L2TradeAttribute5"].ToString();
            if (!string.IsNullOrEmpty(dr["L2TradeAttribute6"].ToString()))
                _tradeAttribute6 = dr["L2TradeAttribute6"].ToString();
            if (!string.IsNullOrEmpty(dr["L2Commission"].ToString()))
                _commission = Convert.ToDouble(dr["L2Commission"]);
            if (!string.IsNullOrEmpty(dr["L2SoftCommission"].ToString()))
                _softCommission = Convert.ToDouble(dr["L2SoftCommission"]);
            if (!string.IsNullOrEmpty(dr["L2OtherBrokerFees"].ToString()))
                _otherBrokerfees = Convert.ToDouble(dr["L2OtherBrokerFees"]);
            if (!string.IsNullOrEmpty(dr["L2ClearingBrokerFee"].ToString()))
                _clearingBrokerFee = Convert.ToDouble(dr["L2ClearingBrokerFee"]);
            if (!string.IsNullOrEmpty(dr["L2StampDuty"].ToString()))
                _stampDuty = Convert.ToDouble(dr["L2StampDuty"]);
            if (!string.IsNullOrEmpty(dr["L2TransactionLevy"].ToString()))
                _transactionLevy = Convert.ToDouble(dr["L2TransactionLevy"]);
            if (!string.IsNullOrEmpty(dr["L2ClearingFee"].ToString()))
                _clearingFee = Convert.ToDouble(dr["L2ClearingFee"]);
            if (!string.IsNullOrEmpty(dr["L2TaxOnCommissions"].ToString()))
                _taxOnCommissions = Convert.ToDouble(dr["L2TaxOnCommissions"]);
            if (!string.IsNullOrEmpty(dr["L2MiscFees"].ToString()))
                _miscFees = Convert.ToDouble(dr["L2MiscFees"]);
            if (!string.IsNullOrEmpty(dr["L2SecFee"].ToString()))
                _secFee = Convert.ToDouble(dr["L2SecFee"]);
            if (!string.IsNullOrEmpty(dr["L2OccFee"].ToString()))
                _occFee = Convert.ToDouble(dr["L2OccFee"]);
            if (!string.IsNullOrEmpty(dr["L2OrfFee"].ToString()))
                _orfFee = Convert.ToDouble(dr["L2OrfFee"]);
            if (!string.IsNullOrEmpty(dr["L2TaxlotState"].ToString()))
            {
                _taxLotState = (ApplicationConstants.TaxLotState)Enum.Parse(typeof(ApplicationConstants.TaxLotState), dr["L2TaxlotState"].ToString());
            }
            if (!string.IsNullOrEmpty(dr["L2AccruedInterest"].ToString()))
                _accruedInterest = Convert.ToDouble(dr["L2AccruedInterest"]);
            if (!string.IsNullOrEmpty(dr["L2FXRate"].ToString()))
                _avgFXRateForTrade = Convert.ToDouble(dr["L2FXRate"]);
            if (!string.IsNullOrEmpty(dr["L2FXConversionMethodOperator"].ToString()))
                _FXConversionMethodOperator = dr["L2FXConversionMethodOperator"].ToString().Trim();
            if (!string.IsNullOrEmpty(dr["ExternalTransId"].ToString()))
                _externalTransId = dr["ExternalTransId"].ToString();
            if (!string.IsNullOrEmpty(dr["LotId"].ToString()))
                _lotId = dr["LotId"].ToString();
            if (!string.IsNullOrEmpty(dr["TaxLotQty"].ToString()))
                _executedQty = Convert.ToDouble(dr["TaxLotQty"]);
            if (!string.IsNullOrEmpty(dr["Level2Percentage"].ToString()))
                _l2percentage = (float)Convert.ToDouble(dr["Level2Percentage"]);
            if (!string.IsNullOrEmpty(dr["L2AdditionalTradeAttributes"].ToString()))
            {
                base.SetTradeAttribute(dr["L2AdditionalTradeAttributes"].ToString());
            }
        }

        public virtual void SetBasicDetails()
        {
            _taxLotState = ApplicationConstants.TaxLotState.New;
        }

        public virtual void CopyExternalDetails(AllocationGroup group)
        {
            _lotId = group.LotId;
            _externalTransId = group.ExternalTransId;
        }

        public virtual void SetAndCalculateValues(AllocationLevelClass level1, AllocationLevelClass level2, bool isFractionalAllowed)
        {
            SetLevels(level1, ref level2);
            if (level2.Percentage == 0)
            {
                _percentage = level1.Percentage;
            }
            else
            {
                _percentage = level2.Percentage * level1.Percentage / 100;
            }

            _taxLotQty = level2.AllocatedQty;
            _l2percentage = level2.Percentage;
            _executedQty = level2.AllocatedQty; //Set executed quantity same as allocated quantity for trade

            _taxlotID = level1.GroupID + level1.LevelnID.ToString() + level2.LevelnID;
            _groupID = level1.GroupID;
        }

        public virtual void SetLevels(AllocationLevelClass level1, ref AllocationLevelClass level2)
        {
            if (level2 == null)
            {
                level2 = new AllocationLevelClass(level1.GroupID);
                level2.LevelnID = 0;
                level2.Percentage = 0;
                level2.AllocatedQty = level1.AllocatedQty;
            }
            _level1ID = level1.LevelnID;
            _level2ID = level2.LevelnID;
            _l2percentage = level2.Percentage;
            _percentage = level2.Percentage;
        }

        #region Properties

        /// <summary>
        /// Actions for audit trail. Exposed only for enumeration Do not add actions directly use
        /// function instead
        /// </summary>
        [XmlIgnore, Browsable(false)]
        public virtual List<TradeAuditActionType.ActionType> TradeActionsList
        {
            get { return _tradeActions; }
            set { _tradeActions = value; }
        }

        //[XmlIgnore]
        [Browsable(false)]
        public virtual int Level1ID
        {
            set { _level1ID = value; }
            get { return _level1ID; }
        }

        [XmlIgnore]
        public virtual string Level1Name
        {
            get { return _level1Name; }
            set { _level1Name = value; }
        }

        [Browsable(false)]
        public virtual string Level1AllocationID
        {
            get { return _groupID + _level1ID; }
        }

        private string _nLevel1AllocationID;

        [Browsable(false)]
        public virtual string NLevel1AllocationID
        {
            get { return _nLevel1AllocationID; }
            set { _nLevel1AllocationID = value; }
        }

        [Browsable(false)]
        public virtual int Level2ID
        {
            set { _level2ID = value; }
            get
            {
                return _level2ID;
            }
        }

        [XmlIgnore]
        public virtual string Level2Name
        {
            get
            {
                if (_level2Name == string.Empty)
                    return "N/A";
                else
                    return _level2Name;
            }
            set { _level2Name = value; }
        }

        [XmlIgnore]
        public virtual string MasterFund
        {
            get
            {
                //Bharat Kumar Jangir (25 March 2014)
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3430
                //if (_masterFund == string.Empty)
                //{
                //    return _level1Name;
                //}
                return _masterFund;
            }
            set { _masterFund = value; }
        }

        [XmlIgnore]
        public virtual float Percentage
        {
            get
            {
                return _percentage;
            }
            set
            {
                _percentage = value;
            }
        }

        [Browsable(false)]
        public virtual float Level2Percentage
        {
            set { _l2percentage = value; }
            get
            {
                return _l2percentage;
            }
        }

        public virtual string TaxLotID
        {
            get { return _taxlotID; }

            set { _taxlotID = value; }
        }

        public virtual double TaxLotQty
        {
            get { return _taxLotQty; }
            set { _taxLotQty = value; }
        }

        /// <summary>
        /// Narendra Kumar Jangir, May 24 2013 This field specifies quantity to close from
        /// CloseTradeFromAllocation UI. This field will be 0 if closing is done from closing UI.
        /// </summary>
        public virtual double TaxLotQtyToClose
        {
            get { return _taxLotQtyToClose; }
            set { _taxLotQtyToClose = value; }
        }

        private double _commission = 0.0;

        public virtual double Commission
        {
            set
            {
                _commission = value;
            }
            get { return _commission; }
        }

        private double _softCommission = 0.0;

        public virtual double SoftCommission
        {
            set
            {
                _softCommission = value;
            }
            get { return _softCommission; }
        }

        private double _otherBrokerfees = 0.0;

        public virtual double OtherBrokerFees
        {
            set
            {
                _otherBrokerfees = value;
            }
            get { return _otherBrokerfees; }
        }

        private double _clearingBrokerFee = 0.0;

        public virtual double ClearingBrokerFee
        {
            set
            {
                _clearingBrokerFee = value;
            }
            get { return _clearingBrokerFee; }
        }

        public virtual double TotalCommissionandFees
        {
            //Adding option premium
            get { return _taxOnCommissions + _transactionLevy + _miscFees + _stampDuty + _clearingFee + _commission + _softCommission + _otherBrokerfees + _clearingBrokerFee + _secFee + _occFee + _orfFee + _optionPremiumAdjustment; }
        }

        private double _stampDuty = 0.0;

        public virtual double StampDuty
        {
            get { return _stampDuty; }
            set { _stampDuty = value; }
        }

        private double _transactionLevy = 0.0;

        public virtual double TransactionLevy
        {
            get { return _transactionLevy; }
            set { _transactionLevy = value; }
        }

        private double _clearingFee = 0.0;

        public virtual double ClearingFee
        {
            get { return _clearingFee; }
            set { _clearingFee = value; }
        }

        private double _taxOnCommissions = 0.0;

        public virtual double TaxOnCommissions
        {
            get { return _taxOnCommissions; }
            set { _taxOnCommissions = value; }
        }

        private double _miscFees = 0.0;

        public virtual double MiscFees
        {
            get { return _miscFees; }
            set { _miscFees = value; }
        }

        private double _secFee = 0.0;

        public virtual double SecFee
        {
            get { return _secFee; }
            set { _secFee = value; }
        }

        private double _occFee = 0.0;

        public virtual double OccFee
        {
            get { return _occFee; }
            set { _occFee = value; }
        }

        private double _orfFee = 0.0;

        public virtual double OrfFee
        {
            get { return _orfFee; }
            set { _orfFee = value; }
        }

        private double _optionPremiumAdjustment = 0.0;

        public virtual double OptionPremiumAdjustment
        {
            get { return _optionPremiumAdjustment; }
            set { _optionPremiumAdjustment = value; }
        }

        private string _groupID;

        public virtual string GroupID
        {
            get { return _groupID; }
            set { _groupID = value; }
        }

        [Browsable(false)]
        public virtual ApplicationConstants.TaxLotState TaxLotState
        {
            get { return _taxLotState; }
            set { _taxLotState = value; }
        }

        //Added By Faisal Shah
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-1147
        public virtual string NavLockStatus
        {
            get { return _navLockStatus; }
            set { _navLockStatus = value; }
        }

        //[Browsable(false)]
        public virtual ClosingStatus ClosingStatus
        {
            get { return _closingStatus; }
            set { _closingStatus = value; }
        }

        private int _closingAlgo;

        public virtual int ClosingAlgo
        {
            get { return _closingAlgo; }
            set { _closingAlgo = value; }
        }

        private bool _isPreAllocated;

        public virtual bool IsPreAllocated
        {
            get { return _isPreAllocated; }
            set { _isPreAllocated = value; }
        }

        private bool _isSwap;

        public virtual bool ISSwap
        {
            get { return _isSwap; }
            set { _isSwap = value; }
        }

        private OptionMoneyness _itmOtm = OptionMoneyness.NA;
        /// <summary>
        /// Gets or sets the itm otm.
        /// </summary>
        /// <value>
        /// The itm otm.
        /// </value>
        public virtual OptionMoneyness ItmOtm
        {
            get { return _itmOtm; }
            set { _itmOtm = value; }
        }

        private double _percentOfITMOTM = 0.0;
        /// <summary>
        /// Gets or sets the percent of underlying price.
        /// </summary>
        /// <value>
        /// The percent of underlying price.
        /// </value>
        public virtual double PercentOfITMOTM
        {
            get { return _percentOfITMOTM; }
            set { _percentOfITMOTM = value; }
        }

        private decimal _intrinsicValue = 0.0m;
        /// <summary>
        /// Gets or sets the intrinsic value.
        /// </summary>
        /// <value>
        /// The intrinsic value.
        /// </value>
        public virtual decimal IntrinsicValue
        {
            get { return _intrinsicValue; }
            set { _intrinsicValue = value; }
        }

        private int _daysToExpiry = 0;
        /// <summary>
        /// Gets or sets the days to expiry.
        /// </summary>
        /// <value>
        /// The days to expiry.
        /// </value>
        public virtual int DaysToExpiry
        {
            get { return _daysToExpiry; }
            set { _daysToExpiry = value; }
        }

        private double _gainLossIfExerciseAssign = 0.0;
        /// <summary>
        /// Gets or sets the gain loss if exercise assign.
        /// </summary>
        /// <value>
        /// The gain loss if exercise assign.
        /// </value>
        public virtual double GainLossIfExerciseAssign
        {
            get { return _gainLossIfExerciseAssign; }
            set { _gainLossIfExerciseAssign = value; }
        }

        private bool _isCurrencySettle;

        [Browsable(false)]
        [XmlIgnore]
        public virtual bool IsCurrencySettle
        {
            get { return _isCurrencySettle; }
            set { _isCurrencySettle = value; }
        }

        private string _taxLotClosingId;

        public virtual string TaxLotClosingId
        {
            get { return _taxLotClosingId; }
            set { _taxLotClosingId = value; }
        }

        private ClosingMode _closingMode = ClosingMode.None;

        public virtual ClosingMode ClosingMode
        {
            get { return _closingMode; }
            set
            {
                _closingMode = value;
            }
        }

        private DateTime _timeOfSaveUTC = DateTimeConstants.MinValue;

        public virtual DateTime TimeOfSaveUTC
        {
            get { return _timeOfSaveUTC; }
            set { _timeOfSaveUTC = value; }
        }

        private PositionTag _positionTag;

        public virtual PositionTag PositionTag
        {
            get { return _positionTag; }
            set { _positionTag = value; }
        }

        private DateTime _auecModifiedDate = DateTimeConstants.MinValue;

        public virtual DateTime AUECModifiedDate
        {
            get { return _auecModifiedDate; }
            set { _auecModifiedDate = value; }
        }

        private double _openTotalCommissionandFees = 0.0;

        public virtual double OpenTotalCommissionandFees
        {
            get { return _openTotalCommissionandFees; }
            set { _openTotalCommissionandFees = value; }
        }

        private double _closedTotalCommissionandFees = 0.0;

        public virtual double ClosedTotalCommissionandFees
        {
            get { return _closedTotalCommissionandFees; }
            set { _closedTotalCommissionandFees = value; }
        }

        private SwapParameters _swapParameters;

        public virtual SwapParameters SwapParameters
        {
            get { return _swapParameters; }
            set { _swapParameters = value; }
        }

        private OTCTradeData _otcParameters;
        public virtual OTCTradeData OTCParameters
        {
            get { return _otcParameters; }
            set { _otcParameters = value; }
        }

        // Limit and Stop price fields addition in Taxlot for Compliance
        private double _stopPrice = 0.0;
        public virtual double StopPrice
        {
            get { return _stopPrice; }
            set { _stopPrice = value; }
        }

        private double _limitPrice = 0.0;
        public virtual double LimitPrice
        {
            get { return _limitPrice; }
            set { _limitPrice = value; }
        }

        private bool _isWhatIfTradeStreamRequired = true;
        public virtual bool IsWhatIfTradeStreamRequired
        {
            get { return _isWhatIfTradeStreamRequired; }
            set { _isWhatIfTradeStreamRequired = value; }
        }

        #region Dynamic-UDA
        public virtual string Analyst
        {
            get { return _analyst; }
            set { _analyst = value; }
        }

        public virtual string CountryOfRisk
        {
            get { return _countryOfRisk; }
            set { _countryOfRisk = value; }
        }

        public virtual string CustomUDA1
        {
            get { return _customUDA1; }
            set { _customUDA1 = value; }
        }

        public virtual string CustomUDA2
        {
            get { return _customUDA2; }
            set { _customUDA2 = value; }
        }

        public virtual string CustomUDA3
        {
            get { return _customUDA3; }
            set { _customUDA3 = value; }
        }

        public virtual string CustomUDA4
        {
            get { return _customUDA4; }
            set { _customUDA4 = value; }
        }

        public virtual string CustomUDA5
        {
            get { return _customUDA5; }
            set { _customUDA5 = value; }
        }

        public virtual string CustomUDA6
        {
            get { return _customUDA6; }
            set { _customUDA6 = value; }
        }

        public virtual string CustomUDA7
        {
            get { return _customUDA7; }
            set { _customUDA7 = value; }
        }

        public virtual string CustomUDA8
        {
            get { return _customUDA8; }
            set { _customUDA8 = value; }
        }

        public virtual string CustomUDA9
        {
            get { return _customUDA9; }
            set { _customUDA9 = value; }
        }

        public virtual string CustomUDA10
        {
            get { return _customUDA10; }
            set { _customUDA10 = value; }
        }

        public virtual string CustomUDA11
        {
            get { return _customUDA11; }
            set { _customUDA11 = value; }
        }

        public virtual string CustomUDA12
        {
            get { return _customUDA12; }
            set { _customUDA12 = value; }
        }
        public virtual string Issuer
        {
            get { return _issuer; }
            set { _issuer = value; }
        }

        public virtual string LiquidTag
        {
            get { return _liquidTag; }
            set { _liquidTag = value; }
        }

        public virtual string MarketCap
        {
            get { return _marketCap; }
            set { _marketCap = value; }
        }

        public virtual string Region
        {
            get { return _region; }
            set { _region = value; }
        }

        public virtual string RiskCurrency
        {
            get { return _riskCurrency; }
            set { _riskCurrency = value; }
        }

        public virtual string UcitsEligibleTag
        {
            get { return _ucitsEligibleTag; }
            set { _ucitsEligibleTag = value; }
        }

        private DateTime _closingTradeDate = DateTimeConstants.MinValue;

        [Browsable(false)]
        public virtual DateTime ClosingTradeDate
        {
            get { return _closingTradeDate; }
            set { _closingTradeDate = value; }
        }

        private DateTime _closingSettlementDate = DateTimeConstants.MinValue;

        [Browsable(false)]
        public virtual DateTime ClosingSettlementDate
        {
            get { return _closingSettlementDate; }
            set { _closingSettlementDate = value; }
        }

        private double _closedQuantity = 0;

        [Browsable(false)]
        public virtual double ClosedQuantity
        {
            get { return _closedQuantity; }
            set { _closedQuantity = value; }
        }

        #endregion

        public virtual double UnitCost
        {
            get
            {
                if (TaxLotQty == 0)
                {
                    return 0;
                }

                if (OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                {
                    return (_avgPrice * _taxLotQty * _contractMultiplier + _openTotalCommissionandFees) / (_taxLotQty * _contractMultiplier);
                }
                else
                {
                    return (_avgPrice * _taxLotQty * _contractMultiplier - OpenTotalCommissionandFees) / (_taxLotQty * _contractMultiplier);
                }
            }
        }

        public virtual double UnitCostBase
        {
            get
            {
                double value = 0;
                if (UnitCost == 0 || _avgFXRateForTrade == 0)
                {
                    value = 0;
                }
                else if (_FXConversionMethodOperator.Equals(Operator.M.ToString()))
                {
                    value = UnitCost * _avgFXRateForTrade;
                }
                else if (_FXConversionMethodOperator.Equals(Operator.D.ToString()))
                {
                    value = UnitCost / _avgFXRateForTrade;
                }
                value = Math.Round(value, 14);
                return value;
            }
        }

        public virtual double NetNotionalValue
        {
            get
            {
                if (string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy) || string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy_Closed) || string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy_Open) || string.Equals(_orderSideTagValue, FIXConstants.SIDE_Buy_Cover))
                {
                    return (_avgPrice * _taxLotQty * _contractMultiplier) + (_openTotalCommissionandFees);
                }
                else
                    return (_avgPrice * _taxLotQty * _contractMultiplier) - (_openTotalCommissionandFees);
            }
        }

        private double _settledQty = 0;
        /// <summary>
        /// Settled or Expired Qty for Physical or cash Settled
        /// </summary>

        public virtual double SettledQty
        {
            get { return _settledQty; }
            set { _settledQty = value; }
        }

        private bool _isExerciseAtZero = false;

        public virtual bool IsExerciseAtZero
        {
            get { return _isExerciseAtZero; }
            set { _isExerciseAtZero = value; }
        }

        private bool? _isManualyExerciseAssign = null;
        [Browsable(false)]
        public virtual bool? IsManualyExerciseAssign
        {
            get { return _isManualyExerciseAssign; }
            set { _isManualyExerciseAssign = value; }
        }

        private AssetCategory _assetCategoryValue;
        /// <summary>
        /// determines the asset type on the setting of auecid
        /// </summary>

        public virtual AssetCategory AssetCategoryValue
        {
            get { return _assetCategoryValue; }
            set { _assetCategoryValue = value; }
        }

        private double _cashSettledPrice;

        public virtual double CashSettledPrice
        {
            get { return _cashSettledPrice; }
            set
            {
                _cashSettledPrice = value;
            }
        }

        private bool _isPosition;

        public virtual bool IsPosition
        {
            get { return _isPosition; }
            set { _isPosition = value; }
        }

        private string _positionType;

        [XmlIgnore]
        public virtual string PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }

        //initialize UDA fields with default value - omshiv, Nov 2013
        private string _securityTypeName = ApplicationConstants.CONST_UNDEFINED;

        public virtual string SecurityTypeName
        {
            get { return _securityTypeName; }
            set { _securityTypeName = value; }
        }

        private string _sectorName = ApplicationConstants.CONST_UNDEFINED;

        public virtual string SectorName
        {
            get { return _sectorName; }
            set { _sectorName = value; }
        }

        private string _subSectorName = ApplicationConstants.CONST_UNDEFINED;

        public virtual string SubSectorName
        {
            get { return _subSectorName; }
            set { _subSectorName = value; }
        }

        private string _countryName = ApplicationConstants.CONST_UNDEFINED;

        public virtual string CountryName
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        private string _UDAAsset = ApplicationConstants.CONST_UNDEFINED;

        public virtual string UDAAsset
        {
            get { return _UDAAsset; }
            set { _UDAAsset = value; }
        }

        private string _externalTransId = string.Empty;

        /// <summary>
        /// Added By Sandeep as on 08-Feb-2013 this keep the External Transaction ID send by the
        /// client side
        /// </summary>
        public virtual string ExternalTransId
        {
            get { return _externalTransId; }
            set { _externalTransId = (value == null) ? string.Empty : value; }
        }

        private string _lotId = string.Empty;

        /// <summary>
        /// Added By Narendra as on 18-Mar-2013 this keep the lot ID send by the Nirvana client i.e. user
        /// </summary>
        public virtual string LotId
        {
            get { return _lotId; }
            set { _lotId = (value == null) ? string.Empty : value; }
        }

        private double _markPrice = 0.0;

        public virtual double MarkPrice
        {
            set
            {
                _markPrice = value;
            }
            get { return _markPrice; }
        }

        private double _executedQty = 0.0;

        public virtual double ExecutedQty
        {
            set
            {
                _executedQty = value;
            }
            get { return _executedQty; }
        }

        public virtual double UnRealizedPNL
        {
            get
            {
                double openAvgPrice = _avgPrice;
                double markPriceLocal = _markPrice;
                if ((AssetCategoryValue == AssetCategory.FX || AssetCategoryValue == AssetCategory.FXForward) && CurrencyID != LeadCurrencyID)
                {
                    if (openAvgPrice != 0)
                        openAvgPrice = 1 / openAvgPrice;
                    if (markPriceLocal != 0)
                        markPriceLocal = 1 / markPriceLocal;
                }
                if (OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                {
                    return (_taxLotQty * (markPriceLocal - openAvgPrice)) * _contractMultiplier;
                }
                else
                {
                    return (_taxLotQty * (markPriceLocal - openAvgPrice)) * _contractMultiplier * -1;
                }
            }
        }

        private int _sideMultiplier = 1;

        [XmlIgnore]
        public virtual int SideMultiplier
        {
            get { return _sideMultiplier; }
            set { _sideMultiplier = value; }
        }

        public virtual PositionTag LongOrShort
        {
            get
            {
                switch (this.OrderSideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Buy_Open:
                    case FIXConstants.SIDE_Sell_Closed:
                    default:
                        return PositionTag.Long;

                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_Buy_Closed:
                    case FIXConstants.SIDE_Buy_Cover:
                    case FIXConstants.SIDE_BuyMinus:
                    case FIXConstants.SIDE_Sell_Open:
                        return PositionTag.Short;
                }
            }
        }

        // Added TaxlotPk and ParentRowPk to store row Ids to taxlot and its parent taxlot row
        // http: //jira.nirvanasolutions.com:8080/browse/PRANA-7378
        public virtual long TaxlotPk
        {
            get { return _taxlotPk; }
            set { _taxlotPk = value; }
        }

        public virtual long ParentRowPk
        {
            get { return _parentRowPk; }
            set { _parentRowPk = value; }
        }

        [Browsable(false)]
        public virtual string TIF
        {
            get { return _tif; }
            set { _tif = value; }
        }

        // This is for Compliance Order Side rule
        public virtual int SideRank
        {
            get { return _sideRank; }
            set { _sideRank = value; }
        }

        [Browsable(false)]

        public virtual string AdditionalTradeAttributes
        {
            get; set;
        }
        #endregion Properties

        public virtual void CopyBasicDetails(TaxLot taxLot)
        {
            _taxLotQty = taxLot.TaxLotQty;
            _level1ID = taxLot.Level1ID;
            _level2ID = taxLot.Level2ID;
            _percentage = taxLot.Percentage;
            _l2percentage = taxLot.Level2Percentage;
            _level1Name = taxLot.Level1Name;
            _level2Name = taxLot.Level2Name;
            _taxlotID = taxLot.TaxLotID;
            _taxLotState = taxLot.TaxLotState;
            _underlyingSymbol = taxLot.UnderlyingSymbol;
            _groupID = taxLot.GroupID;
            _sideMultiplier = taxLot.SideMultiplier;
            _lotId = taxLot.LotId;
            _externalTransId = taxLot.ExternalTransId;
            _tradeAttribute1 = taxLot.TradeAttribute1;
            _tradeAttribute2 = taxLot.TradeAttribute2;
            _tradeAttribute3 = taxLot.TradeAttribute3;
            _tradeAttribute4 = taxLot.TradeAttribute4;
            _tradeAttribute5 = taxLot.TradeAttribute5;
            _tradeAttribute6 = taxLot.TradeAttribute6;

            #region fill UDA details

            // for updating UDA details on PM -OM Shiv, 2013 Sep

            _UDAAsset = taxLot.UDAAsset;
            _countryName = taxLot.CountryName;
            _sectorName = taxLot.SectorName;
            _securityTypeName = taxLot.SecurityTypeName;
            _subSectorName = taxLot.SubSectorName;

            _positionTag = taxLot.PositionTag;

            #endregion fill UDA details

            if (taxLot.SwapParameters != null)
            {
                if (_swapParameters == null)
                {
                    _swapParameters = new SwapParameters();
                    _swapParameters = taxLot.SwapParameters.Clone();
                }
                else
                {
                    _swapParameters = taxLot.SwapParameters.Clone();
                }
                _isSwap = taxLot.ISSwap;
            }

            if (taxLot.OTCParameters != null)
            {
                _otcParameters = taxLot.OTCParameters;
            }
            _auecModifiedDate = taxLot.AUECModifiedDate;
            #region Dynamic-UDA
            _analyst = taxLot.Analyst;
            _countryOfRisk = taxLot.CountryOfRisk;
            _customUDA1 = taxLot.CustomUDA1;
            _customUDA2 = taxLot.CustomUDA2;
            _customUDA3 = taxLot.CustomUDA3;
            _customUDA4 = taxLot.CustomUDA4;
            _customUDA5 = taxLot.CustomUDA5;
            _customUDA6 = taxLot.CustomUDA6;
            _customUDA7 = taxLot.CustomUDA7;
            _issuer = taxLot.Issuer;
            _liquidTag = taxLot.LiquidTag;
            _marketCap = taxLot.MarketCap;
            _region = taxLot.Region;
            _riskCurrency = taxLot.RiskCurrency;
            _ucitsEligibleTag = taxLot.UcitsEligibleTag;
            _isWhatIfTradeStreamRequired = taxLot.IsWhatIfTradeStreamRequired;
            _customUDA8 = taxLot.CustomUDA8;
            _customUDA9 = taxLot.CustomUDA9;
            _customUDA10 = taxLot.CustomUDA10;
            _customUDA11 = taxLot.CustomUDA11;
            _customUDA12 = taxLot.CustomUDA12;
            #endregion
            base.CopyBasicDetails(taxLot);
        }

        // adding new as that implemented default behavior
        new public virtual void FillData(object[] data, int offset)
        {
            try
            {
                base.FillData(data, 0);
                _executedQty = Convert.ToDouble(data[12]);
                _level1ID = int.Parse(data[16].ToString());
                _level2ID = int.Parse(data[17].ToString());
                _taxLotQty = double.Parse(data[18].ToString());
                _commission = float.Parse(data[19].ToString());
                _otherBrokerfees = float.Parse(data[20].ToString());
                _stampDuty = float.Parse(data[21].ToString());
                _transactionLevy = float.Parse(data[22].ToString());
                _clearingFee = float.Parse(data[23].ToString());
                _taxOnCommissions = float.Parse(data[24].ToString());
                _miscFees = float.Parse(data[25].ToString());
                _secFee = float.Parse(data[31].ToString());
                _occFee = float.Parse(data[32].ToString());
                _orfFee = float.Parse(data[33].ToString());
                _clearingBrokerFee = float.Parse(data[34].ToString());
                _softCommission = float.Parse(data[35].ToString());
                _quantity = Convert.ToDouble(data[36]);
                _settlementCurrencyID = int.Parse(data[37].ToString());
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information gets
                // out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #region IKeyable Members
        public virtual string GetKey()
        {
            return this.TaxLotID;
        }

        public virtual void Update(IKeyable item)
        {
            TaxLot taxlot = (TaxLot)item;
            if (taxlot.TaxLotQty != 0)
            {
                this.OpenTotalCommissionandFees = (taxlot.TotalCommissionandFees * (this.TaxLotQty / taxlot.TaxLotQty));
            }
            this.TaxLotQty = taxlot.TaxLotQty;
            this.ExecutedQty = taxlot.ExecutedQty;
            this.ClosedTotalCommissionandFees = taxlot.ClosedTotalCommissionandFees;
            this.AvgPrice = taxlot.AvgPrice;
            this.OpenTotalCommissionandFees = taxlot.TotalCommissionandFees;
            this.OrderSide = taxlot.OrderSide;
            this.OrderSideTagValue = taxlot.OrderSideTagValue;
            this.SettlementDate = taxlot.SettlementDate;
            this.OriginalPurchaseDate = taxlot.OriginalPurchaseDate;
            this.ProcessDate = taxlot.ProcessDate;
            this.AUECLocalDate = taxlot.AUECLocalDate;
            this.NirvanaProcessDate = taxlot.NirvanaProcessDate;
            this.ClosingDate = taxlot.ClosingDate;
            this.AUECModifiedDate = taxlot.AUECModifiedDate;
            this.FXRate = taxlot.FXRate;
            this.FXConversionMethodOperator = taxlot.FXConversionMethodOperator;
            this.CounterPartyID = taxlot.CounterPartyID;
            this.CounterPartyName = taxlot.CounterPartyName;
            this.ISSwap = taxlot.ISSwap;
            this.TradeAttribute1 = taxlot.TradeAttribute1;
            this.TradeAttribute2 = taxlot.TradeAttribute2;
            this.TradeAttribute3 = taxlot.TradeAttribute3;
            this.TradeAttribute4 = taxlot.TradeAttribute4;
            this.TradeAttribute5 = taxlot.TradeAttribute5;
            this.TradeAttribute6 = taxlot.TradeAttribute6;

            this.TransactionType = taxlot.TransactionType;
            this.SettlementCurrencyID = taxlot.SettlementCurrencyID;

            if (taxlot.ISSwap)
            {
                if (_swapParameters == null)
                {
                    _swapParameters = new SwapParameters();
                    _swapParameters = taxlot.SwapParameters.Clone();
                }
                else
                {
                    _swapParameters = taxlot.SwapParameters.Clone();
                }
            }

            //update commission and fees, PRANA-15997
            this.Commission = taxlot.Commission;
            this.SoftCommission = taxlot.SoftCommission;
            this.ClearingBrokerFee = taxlot.ClearingBrokerFee;
            this.ClearingFee = taxlot.ClearingFee;
            this.MiscFees = taxlot.MiscFees;
            this.OccFee = taxlot.OccFee;
            this.OrfFee = taxlot.OrfFee;
            this.OtherBrokerFees = taxlot.OtherBrokerFees;
            this.SecFee = taxlot.SecFee;
            this.StampDuty = taxlot.StampDuty;
            this.TaxOnCommissions = taxlot.TaxOnCommissions;
            this.TransactionLevy = taxlot.TransactionLevy;
            this.CommissionAmt = taxlot.CommissionAmt;
            this.SoftCommissionAmt = taxlot.SoftCommissionAmt;
            this.CommissionRate = taxlot.CommissionRate;
            this.SoftCommissionCalcBasis = taxlot.SoftCommissionCalcBasis;
            this.SetTradeAttribute(taxlot.GetTradeAttributesAsDict());
        }

        /// <summary>
        /// Sandeep Singh, Feb 4, 2014 This field is used in Corporate Action Spin-off when Notional
        /// value changes
        /// </summary>
        private string _closingWithTaxlotID;

        [Browsable(false)]
        public virtual string ClosingWithTaxlotID
        {
            get { return _closingWithTaxlotID; }
            set { _closingWithTaxlotID = value; }
        }

        #endregion IKeyable Members

        #region IFilterable Members

        public virtual DateTime GetDate()
        {
            return this.ProcessDate;
        }

        public virtual DateTime GetDateModified()
        {
            return this.AUECModifiedDate;
        }

        public virtual string GetSymbol()
        {
            return this.Symbol;
        }

        public virtual int GetAccountID()
        {
            return this.Level1ID;
        }

        #endregion IFilterable Members

        #region INotifyPropertyChangedCustom Members

        [field: NonSerialized]
        public virtual event PropertyChangedEventHandler PropertyChanged;

        public virtual void PropertyHasChanged()
        {
            //passed empty arguments to avoid null reference exception, PRANA-15997
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
            }
        }

        #endregion INotifyPropertyChangedCustom Members


    }
}