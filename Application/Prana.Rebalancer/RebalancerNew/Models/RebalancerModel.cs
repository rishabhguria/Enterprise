using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.Classes;
using System;
using System.ComponentModel;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    /// <summary>
    /// Contains all the fields that needs to be calculated
    /// </summary>
    /// <seealso cref="Prana.BusinessObjects.Classes.RebalancerNew.RebalancerDto" />
    public class RebalancerModel : BindableBase, IRebalancerDto
    {
        #region IRebalancerDto
        private string symbol;

        /// <summary>
        /// Gets or sets the security.
        /// </summary>
        /// <value>
        /// The security.
        /// </value>
        public string Symbol
        {
            get { return symbol; }
            set
            {
                symbol = value;
                OnPropertyChanged();
            }
        }

        private string bloombergSymbol;

        /// <summary>
        /// Gets or sets the security.
        /// </summary>
        /// <value>
        /// The security.
        /// </value>
        public string BloombergSymbol
        {
            get { return bloombergSymbol; }
            set
            {
                bloombergSymbol = value;
                OnPropertyChanged();
            }
        }

        private string bloombergSymbolWithExchangeCode;
        /// <summary>
        /// Gets or sets the security.
        /// </summary>
        /// <value>
        /// The security.
        /// </value>
        public string BloombergSymbolWithExchangeCode
        {
            get { return bloombergSymbolWithExchangeCode; }
            set
            {
                bloombergSymbolWithExchangeCode = value;
                OnPropertyChanged();
            }
        }

        private string factSetSymbol;

        /// <summary>
        /// Gets or sets the security.
        /// </summary>
        /// <value>
        /// The security.
        /// </value>
        public string FactSetSymbol
        {
            get { return factSetSymbol; }
            set
            {
                factSetSymbol = value;
                OnPropertyChanged();
            }
        }

        private string activSymbol;

        /// <summary>
        /// Gets or sets the security.
        /// </summary>
        /// <value>
        /// The security.
        /// </value>
        public string ActivSymbol
        {
            get { return activSymbol; }
            set
            {
                activSymbol = value;
                OnPropertyChanged();
            }
        }

        private PositionType side;
        /// <summary>
        /// Side Long or Short.
        /// </summary>
        /// <value>
        /// Side Long or Short.
        /// </value>
        public PositionType Side
        {
            get { return side; }
            set
            {
                side = value;
                OnPropertyChanged();
            }
        }

        private int accountId;
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public int AccountId
        {
            get { return accountId; }
            set
            {
                accountId = value;
                OnPropertyChanged();
            }
        }

        private string tolerancePercentage = RebalancerConstants.CONST_NotApplicable;
        /// <summary>
        /// Gets or Sets Tolerance Percentage
        /// </summary>
        public string TolerancePercentage
        {
            get { return tolerancePercentage; }
            set
            {
                tolerancePercentage = value;
                OnPropertyChanged();
            }
        }

        private string modelPercentage = RebalancerConstants.CONST_NotApplicable;
        /// <summary>
        /// Gets or Sets Model Percentage
        /// </summary>
        public string ModelPercentage
        {
            get { return modelPercentage; }
            set
            {
                modelPercentage = value;
                OnPropertyChanged();
            }
        }

        private decimal price;
        /// <summary>
        /// Gets or sets the current price / Mark Price.
        /// </summary>
        /// <value>
        /// The current price / mark price.
        /// </value>
        public decimal Price
        {
            get { return price; }
            set
            {
                //If price is changed then we also need to update securities market value, current percentage and target percentage.
                decimal currentMVBase = CurrentMarketValueBase;
                decimal targetMVBase = TargetMarketValueBase;
                price = value;
                decimal updatedCurrentMVBase = CurrentMarketValueBase;
                decimal updatedTargetMVBase = TargetMarketValueBase;
                AccountLevelNAV.CurrentSecuritiesMarketValue += (updatedCurrentMVBase - currentMVBase);
                AccountLevelNAV.TargetSecuritiesMarketValue += (updatedTargetMVBase - targetMVBase);
                AccountLevelNAV.MarketValueForCalculation += (updatedTargetMVBase - targetMVBase);
                //AccountLevelNAV.CashFlow += (targetMVBase - updatedTargetMVBase);

                OnPropertyChanged();
                OnPropertyChanged("CurrentMarketValueLocal");
                OnPropertyChanged("CurrentMarketValueBase");
                OnPropertyChanged("CurrentPercentage");
                OnPropertyChanged("TargetMarketValueLocal");
                OnPropertyChanged("TargetMarketValueBase");
                OnPropertyChanged("TargetPercentage");
                OnPropertyChanged("ChangePercentage");
                OnPropertyChanged("BuySellValue");
                OnPropertyChanged("PriceInBaseCurrency");
            }
        }

        private decimal fXRate;
        /// <summary>
        /// Gets or sets the fx rate.
        /// </summary>
        /// <value>
        /// The fx rate.
        /// </value>
        public decimal FXRate
        {
            get { return fXRate; }
            set
            {
                //If fXRate is changed then we also need to update securities market value, current percentage and target percentage.
                decimal currentMVBase = CurrentMarketValueBase;
                decimal targetMVBase = TargetMarketValueBase;
                fXRate = value;
                decimal updatedCurrentMVBase = CurrentMarketValueBase;
                decimal updatedTargetMVBase = TargetMarketValueBase;
                AccountLevelNAV.CurrentSecuritiesMarketValue += (updatedCurrentMVBase - currentMVBase);
                AccountLevelNAV.TargetSecuritiesMarketValue += (updatedTargetMVBase - targetMVBase);
                AccountLevelNAV.MarketValueForCalculation += (updatedTargetMVBase - targetMVBase);
                //AccountLevelNAV.CashFlow += (targetMVBase - updatedTargetMVBase);

                OnPropertyChanged();
                OnPropertyChanged("CurrentMarketValueLocal");
                OnPropertyChanged("CurrentMarketValueBase");
                OnPropertyChanged("CurrentPercentage");
                OnPropertyChanged("TargetMarketValueLocal");
                OnPropertyChanged("TargetMarketValueBase");
                OnPropertyChanged("TargetPercentage");
                OnPropertyChanged("ChangePercentage");
                OnPropertyChanged("BuySellValue");
            }
        }

        private decimal quantity;
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public decimal Quantity
        {
            get { return quantity; }
            set
            {

                decimal quantityToSet = CalculateQuantity(value);
                targetPosition = value;
                //If quantity is changed then we also need to update securities market value and current percentage.
                decimal currentMVBase = CurrentMarketValueBase;
                quantity = quantityToSet;
                decimal updatedCurrentMVBase = CurrentMarketValueBase;
                AccountLevelNAV.CurrentSecuritiesMarketValue += (updatedCurrentMVBase - currentMVBase);

                OnPropertyChanged();
                OnPropertyChanged("BuySellQty");
                OnPropertyChanged("BuySellValue");
            }
        }

        private decimal CalculateQuantity(decimal value)
        {
            decimal quantityToSet = 0;
            if (RebalancerCache.Instance.GetRoundingType() == RebalancerEnums.RoundingTypes.RoundDown)
            {
                quantityToSet = value > 0 ? Math.Floor(value) : Math.Ceiling(value);
            }
            else if (RebalancerCache.Instance.GetRoundingType() == RebalancerEnums.RoundingTypes.RoundUp)
            {
                quantityToSet = value > 0 ? Math.Ceiling(value) : Math.Floor(value);
            }
            else
            {
                quantityToSet = Math.Round(value);
            }

            return quantityToSet;
        }

        internal void RoundOffTargetPosition()
        {
            if (RebalancerCache.Instance.IsUseRoundLot && roundLot > 0)
            {
                decimal buySellQty = (TargetPosition - quantity) / roundLot;

                switch (RebalancerCache.Instance.GetRoundingType())
                {
                    case RebalancerEnums.RoundingTypes.RoundDown:
                        buySellQty = buySellQty > 0 ? Math.Floor(buySellQty) : Math.Ceiling(buySellQty);
                        break;
                    case RebalancerEnums.RoundingTypes.RoundUp:
                        buySellQty = buySellQty > 0 ? Math.Ceiling(buySellQty) : Math.Floor(buySellQty);
                        break;
                    default:
                        buySellQty = Math.Round(buySellQty);
                        break;
                }
                TargetPosition = quantity + (buySellQty * roundLot);

            }
        }

        private decimal roundLot = 1;
        /// <summary>
        /// Gets or sets the roundLot.
        /// </summary>
        /// <value>
        /// The roundLot.
        /// </value>

        public decimal RoundLot
        {
            get { return roundLot; }
            set
            {
                if (value > 0)
                {
                    roundLot = value;
                    OnPropertyChanged();
                }
            }
        }


        private decimal multiplier;
        /// <summary>
        /// Gets or sets the multiplier.
        /// </summary>
        /// <value>
        /// The multiplier.
        /// </value>
        [Browsable(false)]
        public decimal Multiplier
        {
            get { return multiplier; }
            set
            {
                multiplier = value;
                OnPropertyChanged();
            }
        }

        private string accountOrGroupNameToRebalance;
        /// <summary>
        /// AccountOrGroup Name
        /// </summary>
        [Browsable(false)]
        public string AccountOrGroupNameToRebalance
        {
            get { return accountOrGroupNameToRebalance; }
            set
            {
                accountOrGroupNameToRebalance = value;
            }
        }

        private string sector;
        /// <summary>
        /// Gets or sets the sector of the security.
        /// </summary>
        /// <value>
        /// The sector of the security.
        /// </value>
        public string Sector
        {
            get { return sector; }
            set
            {
                sector = value;
                OnPropertyChanged();
            }
        }

        private string asset;
        /// <summary>
        /// Gets or sets the asset of the security.
        /// </summary>
        /// <value>
        /// The asset of the security.
        /// </value>
        public string Asset
        {
            get { return asset; }
            set
            {
                asset = value;
                OnPropertyChanged();
            }
        }


        private decimal delta;
        /// <summary>
        /// Gets or sets the delta.
        /// </summary>
        /// <value>
        /// The delta.
        /// </value>
        [Browsable(false)]
        public decimal Delta
        {
            get { return delta; }
            set
            {
                delta = value;
                OnPropertyChanged();
            }
        }

        private decimal leveragedFactor;
        /// <summary>
        /// Gets or sets the leveraged factor.
        /// </summary>
        /// <value>
        /// The leveraged factor.
        /// </value>
        [Browsable(false)]
        public decimal LeveragedFactor
        {
            get { return leveragedFactor; }
            set
            {
                leveragedFactor = value;
                OnPropertyChanged();
            }
        }

        private bool isStaleClosingMark;
        /// <summary>
        /// Gets or sets the IsStaleClosingMark.
        /// </summary>
        /// <value>
        /// The IsStaleClosingMark.
        /// </value>
        [Browsable(false)]
        public bool IsStaleClosingMark
        {
            get { return isStaleClosingMark; }
            set
            {
                isStaleClosingMark = value;
                OnPropertyChanged();
            }
        }

        private bool isStaleFxRate;
        /// <summary>
        /// Gets or sets the IsStaleFxRate.
        /// </summary>
        /// <value>
        /// The IsStaleFxRate
        /// </value>
        [Browsable(false)]
        public bool IsStaleFxRate
        {
            get { return isStaleFxRate; }
            set
            {
                isStaleFxRate = value;
                OnPropertyChanged();
            }
        }

        private decimal yesterdayMarkPrice;
        /// <summary>
        /// Gets or sets the yesterdayMarkPrice.
        /// </summary>
        /// <value>
        /// The IsStaleFxRate
        /// </value>
        [Browsable(false)]
        public decimal YesterdayMarkPrice
        {
            get { return yesterdayMarkPrice; }
            set
            {
                yesterdayMarkPrice = value;
                OnPropertyChanged();
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RebalancerModel"/> class.
        /// </summary>
        /// <param name="rebalDto">The rebal dto.</param>
        /// <param name="accountWiseNAV"></param>
        public RebalancerModel(RebalancerDto rebalDto, AdjustedAccountLevelNAV accountWiseNAV)
        {
            AUECID = rebalDto.AUECID;
            AccountLevelNAV = accountWiseNAV;
            symbol = rebalDto.Symbol;
            bloombergSymbol = rebalDto.BloombergSymbol;
            factSetSymbol = rebalDto.FactSetSymbol;
            activSymbol = rebalDto.ActivSymbol;
            side = rebalDto.Side;
            accountId = rebalDto.AccountId;
            price = rebalDto.Price;
            fXRate = rebalDto.FXRate;
            targetPosition = quantity = rebalDto.Quantity;
            multiplier = rebalDto.Multiplier;
            RoundLot = rebalDto.RoundLot;
            delta = rebalDto.Delta;
            leveragedFactor = rebalDto.LeveragedFactor;
            sector = rebalDto.Sector;
            asset = rebalDto.Asset;
            isStaleClosingMark = rebalDto.IsStaleClosingMark;
            isStaleFxRate = rebalDto.IsStaleFxRate;
            if (rebalDto.Asset == "EquitySwap")
            {
                if (Quantity != 0)
                    AvgPrice = Price - (accountWiseNAV.SwapNavAdjustment / Quantity);
            }
            bloombergSymbolWithExchangeCode = rebalDto.BloombergSymbolWithExchangeCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RebalancerModel"/> using <see cref="RebalancerModel"/>.
        /// </summary>
        /// <param name="modelPortfolioSecurityDto"></param>
        /// <param name="accountWiseNAV"></param>
        public RebalancerModel(ModelPortfolioSecurityDto modelPortfolioSecurityDto, AdjustedAccountLevelNAV accountWiseNAV)
        {
            AUECID = modelPortfolioSecurityDto.AUECID;
            symbol = modelPortfolioSecurityDto.Symbol;
            BloombergSymbol = modelPortfolioSecurityDto.BloombergSymbol;
            FactSetSymbol = modelPortfolioSecurityDto.FactSetSymbol;
            ActivSymbol = modelPortfolioSecurityDto.ActivSymbol;
            accountId = accountWiseNAV.AccountId;
            price = modelPortfolioSecurityDto.Price;
            fXRate = modelPortfolioSecurityDto.FXRate;
            multiplier = modelPortfolioSecurityDto.Multiplier;
            RoundLot = modelPortfolioSecurityDto.RoundLot;
            delta = modelPortfolioSecurityDto.Delta;
            leveragedFactor = modelPortfolioSecurityDto.LeveragedFactor;
            sector = modelPortfolioSecurityDto.Sector;
            asset = modelPortfolioSecurityDto.Asset;
            AccountLevelNAV = accountWiseNAV;
            BloombergSymbolWithExchangeCode = modelPortfolioSecurityDto.BloombergSymbolWithExchangeCode;
            if (modelPortfolioSecurityDto.ModelType == (int)Prana.BusinessObjects.Enumerators.RebalancerNew.RebalancerEnums.ModelType.TargetCash)
            {
                TargetPercentage = 0;
            }
            else
            {
                Side = AccountLevelNAV.CurrentTotalNAV + AccountLevelNAV.CashFlow > 0
                  ? modelPortfolioSecurityDto.TargetPercentage > 0 ? BusinessObjects.AppConstants.PositionType.Long : BusinessObjects.AppConstants.PositionType.Short
                  : modelPortfolioSecurityDto.TargetPercentage > 0 ? BusinessObjects.AppConstants.PositionType.Short : BusinessObjects.AppConstants.PositionType.Long;
                TargetPercentage = modelPortfolioSecurityDto.TargetPercentage;
            }
            IsNewlyAdded = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityDataGridModel"></param>
        /// <param name="adjustedAccountLevelNAV"></param>
        public RebalancerModel(SecurityDataGridModel securityDataGridModel, AdjustedAccountLevelNAV adjustedAccountLevelNAV)
        {
            AUECID = securityDataGridModel.AUECID;
            RoundLot = securityDataGridModel.RoundLot;
            symbol = securityDataGridModel.Symbol;
            FactSetSymbol = securityDataGridModel.FactSetSymbol;
            ActivSymbol = securityDataGridModel.ActivSymbol;
            BloombergSymbol = securityDataGridModel.BloombergSymbol;
            accountId = adjustedAccountLevelNAV.AccountId;
            price = securityDataGridModel.Price;
            fXRate = securityDataGridModel.FXRate;
            multiplier = securityDataGridModel.Multiplier;
            delta = securityDataGridModel.Delta;
            leveragedFactor = securityDataGridModel.LeveragedFactor;
            sector = securityDataGridModel.Sector;
            asset = securityDataGridModel.Asset;
            AccountLevelNAV = adjustedAccountLevelNAV;
            BloombergSymbolWithExchangeCode = securityDataGridModel.BloombergSymbolWithExchangeCode;
            if (securityDataGridModel.IncreaseDecreaseOrSet == RebalancerEnums.RASIncreaseDecreaseOrSet.Set.ToString())
            {
                Side = securityDataGridModel.TargetPercentage > 0 ? PositionType.Long : PositionType.Short;
            }
            else
            {
                Side = securityDataGridModel.IncreaseDecreaseOrSet == RebalancerEnums.RASIncreaseDecreaseOrSet.Increase.ToString() ? PositionType.Long : PositionType.Short;
            }
            TargetPercentage = securityDataGridModel.TargetPercentage;
            IsNewlyAdded = true;
        }

        //Added this field for Swap MV calculation
        [Browsable(false)]
        public decimal AvgPrice { get; set; }

        //Added this field for Swap MV calculation
        [Browsable(false)]
        public decimal CostBasisPNL
        {
            get
            {
                return (Price - AvgPrice) * Quantity * FXRate;
            }
        }

        [Browsable(false)]
        public int AUECID { get; set; }

        /// <summary>
        /// True if target position is changed by any of rebalance methods.
        /// False if did not take part in rebalance calculations.
        /// </summary>
        private bool _isCalculatedModel = false;
        [Browsable(false)]
        public bool IsCalculatedModel
        {
            get { return _isCalculatedModel; }
            set
            {
                _isCalculatedModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// True if modified by user from rebalancer grid.
        /// False if not modified by user.
        /// </summary>
        [Browsable(false)]
        public bool IsModified
        {
            get
            {
                if (IsCalculatedModel)
                    return false;
                else
                {
                    if (Quantity != TargetPosition)
                        return true;
                    else
                        return false;
                }
            }
        }

        /// <summary>
        /// True of NAV impacting components.
        /// False for rest rebalance models
        /// </summary>
        //[Browsable(false)]
        public bool IsCustomModel { get; set; }

        /// <summary>
        /// Gets or sets the account Level NAV.
        /// </summary>
        /// <value>
        /// The account Level NAV.
        /// </value>
        [Browsable(false)]
        public AdjustedAccountLevelNAV AccountLevelNAV { get; set; }

        /// <summary>
        /// True if symbol does not exist in portfolio and added newly while rebalance calculation.
        /// </summary>
        [Browsable(false)]
        public bool IsNewlyAdded { get; set; }

        public double PriceInBaseCurrency
        {
            get
            {
                return (double)(Price * FXRate);
            }
        }


        /// <summary>
        /// Gets or sets the current market value.
        /// </summary>
        /// <value>
        /// The current market value.
        /// </value>
        public decimal CurrentMarketValueBase
        {
            get
            {
                return ((decimal)CurrentMarketValueLocal * FXRate);
            }
        }

        /// <summary>
        /// Gets or sets the current market value local.
        /// </summary>
        /// <value>
        /// The current market value local.
        /// </value>
        public double CurrentMarketValueLocal
        {
            get
            {
                return BusinessLogic.Calculations.GetMarketValue((double)Quantity, (double)Price, (double)Multiplier, Side == (PositionType.Long) ? 1 : -1);
            }
        }

        /// <summary>
        /// Gets or sets the Calculation Level.
        /// </summary>
        /// <value>
        /// The Calculation Level.
        /// </value>
        public RebalancerEnums.CalculationLevel RebalCalculationLevel
        {
            get
            {
                return RebalancerCache.Instance.GetCalculationLevel();
            }
        }

        /// <summary>
        /// Gets or sets the current percentage.
        /// </summary>
        /// <value>
        /// The current percentage.
        /// </value>
        public decimal CurrentPercentage
        {
            get
            {
                if (RebalCalculationLevel == RebalancerEnums.CalculationLevel.Account)
                    return CurrentAccountLevelPercentage;
                return CurrentAccountGroupLevelPercentage;
            }
        }

        /// <summary>
        /// Gets the change percentage.
        /// </summary>
        /// <value>
        /// The change percentage.
        /// </value>
        public decimal ChangePercentage
        {
            get
            {
                return TargetPercentage - CurrentPercentage;
            }
        }

        /// <summary>
        /// Gets or sets the current percentage for account.
        /// </summary>
        /// <value>
        /// The current percentage for account.
        /// </value>
        [Browsable(false)]
        public decimal CurrentAccountLevelPercentage
        {
            get
            {
                if (AccountLevelNAV.CurrentTotalNAV != 0)
                    return (CurrentMarketValueBase / AccountLevelNAV.CurrentTotalNAV) * 100;
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the current percentage for account group/MF.
        /// </summary>
        /// <value>
        /// The current percentage for account group/MF.
        /// </value>
        [Browsable(false)]
        public decimal CurrentAccountGroupLevelPercentage
        {
            get
            {
                if (RebalancerCache.Instance.GetAccountGroupLevelNAV().CurrentTotalNAV != 0)
                    return (CurrentMarketValueBase / RebalancerCache.Instance.GetAccountGroupLevelNAV().CurrentTotalNAV) * 100;
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the target percentage.
        /// </summary>
        /// <value>
        /// The target percentage.
        /// </value>
        public decimal TargetPercentage
        {
            get
            {
                if (RebalCalculationLevel == RebalancerEnums.CalculationLevel.Account)
                    return TargetAccountLevelPercentage;
                return TargetAccountGroupLevelPercentage;
            }
            set
            {
                decimal currentTargetPercentage, multiplier;
                currentTargetPercentage = TargetPercentage;
                decimal targetPercentage = value;
                decimal totalNAV = RebalCalculationLevel == RebalancerEnums.CalculationLevel.Account ? AccountLevelNAV.TargetTotalNAV : RebalancerCache.Instance.GetAccountGroupLevelNAV().TargetTotalNAV;
                if (currentTargetPercentage != 0)
                {
                    multiplier = targetPercentage / currentTargetPercentage;
                    TargetPosition = TargetPosition * multiplier;
                }
                else
                {
                    if (price != 0 && fXRate != 0)
                        TargetPosition = (totalNAV * targetPercentage / (price * fXRate)) / 100;

                    else
                        TargetPosition = 0;
                }
                OnPropertyChanged("ChangePercentage");
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the target percentage for account.
        /// </summary>
        /// <value>
        /// The target percentage for account.
        /// </value>
        [Browsable(false)]
        public decimal TargetAccountLevelPercentage
        {
            get
            {
                if (AccountLevelNAV.TargetTotalNAV != 0)
                    return (TargetMarketValueBase / AccountLevelNAV.TargetTotalNAV) * 100;
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the target percentage for account group/MF.
        /// </summary>
        /// <value>
        /// The target percentage for account group/MF.
        /// </value>
        [Browsable(false)]
        public decimal TargetAccountGroupLevelPercentage
        {
            get
            {
                if (RebalancerCache.Instance.GetAccountGroupLevelNAV().TargetTotalNAV != 0)
                    return (TargetMarketValueBase / RebalancerCache.Instance.GetAccountGroupLevelNAV().TargetTotalNAV) * 100;
                return 0;
            }
        }

        private decimal targetPosition;
        /// <summary>
        /// Gets or sets the target position.
        /// </summary>
        /// <value>
        /// The target position.
        /// </value>
        public decimal TargetPosition
        {
            get { return targetPosition; }
            set
            {
                decimal targetPositionToSet;
                if (value != Quantity)
                    targetPositionToSet = CalculateQuantity(value);
                else
                    targetPositionToSet = value;

                //If Target Position is changed then we also need to update securities market value and target percentage.
                decimal targetMVBase = TargetMarketValueBase;
                targetPosition = Math.Abs(targetPositionToSet);
                decimal updatedTargetMVBase = TargetMarketValueBase;
                AccountLevelNAV.TargetSecuritiesMarketValue += (updatedTargetMVBase - targetMVBase);
                AccountLevelNAV.MarketValueForCalculation += (updatedTargetMVBase - targetMVBase);
                AccountLevelNAV.CashFlow += (targetMVBase - updatedTargetMVBase);
                OnPropertyChanged();
                OnPropertyChanged("TargetMarketValueLocal");
                OnPropertyChanged("TargetMarketValueBase");
                OnPropertyChanged("TargetPercentage");
                OnPropertyChanged("ChangePercentage");
                OnPropertyChanged("BuySellValue");
                OnPropertyChanged("BuySellQty");
            }
        }

        /// <summary>
        /// Gets or sets the target market value base.
        /// </summary>
        /// <value>
        /// The target market value base.
        /// </value>
        public decimal TargetMarketValueBase
        {
            get
            {
                return ((decimal)TargetMarketValueLocal * FXRate);
            }
        }

        /// <summary>
        /// Gets or sets the target market value local.
        /// </summary>
        /// <value>
        /// The target market value local.
        /// </value>
        public double TargetMarketValueLocal
        {
            get
            {
                return BusinessLogic.Calculations.GetMarketValue((double)TargetPosition, (double)Price, (double)Multiplier, Side == (PositionType.Long) ? 1 : -1);
            }
        }

        /// <summary>
        /// Gets or sets the buy sell value.
        /// </summary>
        /// <value>
        /// The buy sell value.
        /// </value>
        public decimal BuySellValue
        {
            get
            {
                return TargetMarketValueBase - CurrentMarketValueBase;
            }
        }

        /// <summary>
        /// Gets or sets the buy sell quantity.
        /// </summary>
        /// <value>
        /// The buy sell quantity.
        /// </value>
        public decimal BuySellQty
        {
            get
            {
                return TargetPosition - Quantity;
            }
        }

        private bool _isLock = false;

        public bool IsLock
        {
            get { return _isLock; }
            set
            {
                _isLock = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the commission.
        /// </summary>
        /// <value>
        /// The commission.
        /// </value>
        [Browsable(false)]
        public double Commission { get; set; }

        internal void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        internal RebalancerModel Clone()
        {
            RebalancerModel rebalModel = new RebalancerModel(
                new RebalancerDto
                {
                    AccountId = accountId,
                    AUECID = AUECID,
                    RoundLot = RoundLot,
                    Asset = asset,
                    Delta = delta,
                    FXRate = fXRate,
                    IsStaleClosingMark = isStaleClosingMark,
                    IsStaleFxRate = isStaleFxRate,
                    LeveragedFactor = leveragedFactor,
                    Multiplier = multiplier,
                    Price = price,
                    Sector = sector,
                    Side = side,
                    Symbol = symbol,
                    FactSetSymbol = FactSetSymbol,
                    ActivSymbol = ActivSymbol,
                    BloombergSymbol = BloombergSymbol,
                    BloombergSymbolWithExchangeCode = BloombergSymbolWithExchangeCode
                    //No need to set quantity here as clone is used to generate opposite side trade, which have quantity 0;
                },
                AccountLevelNAV)
            { IsNewlyAdded = true, IsCustomModel = IsCustomModel };
            return rebalModel;
        }
    }
}
