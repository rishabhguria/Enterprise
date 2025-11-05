using System.ComponentModel;

namespace Prana.BusinessObjects.Enumerators.RebalancerNew
{
    public class RebalancerEnums
    {
        public enum RebalancerPositionsType
        {
            [Description("Real-Time Positions")]
            RealTimePositions = 1,
            [Description("Previous Day's Positions")]
            PreviousDaysPositons = 2
        }

        public enum ModelPortfolioType
        {
            [Description("Master Fund")]
            MasterFund = 1,
            [Description("Account")]
            Account = 2,
            [Description("Model Portfolio")]
            ModelPortfolio = 3,
            [Description("Custom Group")]
            CustomGroup = 4
        }

        public enum RefreshTypes
        {
            [Description("Positions")]
            Positions = 1,
            [Description("Prices")]
            Prices = 2,
            [Description("Positions and Prices")]
            Both = 3
        }

        public enum RASIncreaseDecreaseOrSet
        {
            Increase,
            Decrease,
            Set
        }

        public enum BPSOrPercentage
        {
            BPS,
            Percentage
        }

        public enum UseTolerance
        {
            [Description("No")]
            No = 1,
            [Description("Yes")]
            Yes = 2
        }

        public enum ToleranceFactor
        {
            [Description("In Percentage")]
            InPercentage = 1,
            [Description("In BPS")]
            InBPS = 2
        }

        public enum TargetPercentType
        {
            [Description("Model Target %")]
            ModelTargetPercent = 1,
            [Description("Boundary Level")]
            BoundaryLevel = 2
        }

        public enum ActionsOnCash
        {
            [Description("No Action")]
            NoAction = 1,
            [Description("Raise Cash For Buys")]
            RaiseCashForBuys = 2,
            [Description("Reinvest Cash From Sale")]
            ReinvestCashFromSale = 3
        }

        public enum AccountTypes
        {
            [Description("MasterFund")]
            MasterFund = 1,
            [Description("Account")]
            Account = 2,
            [Description("CustomGroup")]
            CustomGroup = 3
        }

        public enum RoundingTypes
        {
            [Description("Round Down")]
            RoundDown = 1,
            [Description("Round Up")]
            RoundUp = 2,
            [Description("Nearest Round Off")]
            NearestRoundOff = 3
        }

        public enum AssetClass
        {
            [Description("Global Equities")]
            GlobalEquities = 1,
            [Description("Fixed Income")]
            FixedIncome = 2,
            [Description("Private Equity")]
            PrivateEquity = 3,
            [Description("Swaps<Notional Value>")]
            SwapsNotionalValue = 4,
            [Description("Futures")]
            Futures = 5,
            [Description("Options")]
            Options = 6,
            [Description("Others")]
            Others = 7
        }

        public enum ItemsImpactingNav
        {
            [Description("Cash")]
            Cash = 1,
            [Description("Accruals")]
            Accruals = 2,
            [Description("Other Assets Market Value")]
            OtherAssetsMarketValue = 3,
            [Description("Swap NAV Adjustment")]
            SwapNavAdjustment = 4,
            [Description("Unrealized P&L of Swaps")]
            UnrealizedPandLofSwaps = 5
        }

        public enum CalculationLevel
        {
            [Description("Account")]
            Account = 1,
            [Description("Master Fund")]
            MasterFund = 2
        }

        public enum CashFlowImpactOnNAV
        {
            [Description("Impact NAV")]
            ImpactNAV = 1,
            [Description("No Impact")]
            NoImpact = 2
        }

        public enum ImportType
        {
            CustomGroupsImport,
            CashFlowImport,
            ModelPortfolioImport
        }

        public enum ModelType
        {
            [Description("Target Security")]
            TargetSecurity = 1,
            [Description("Target Cash")]
            TargetCash = 2
        }

        public enum CashSpecificRules
        {
            [Description("Allow negative cash")]
            AllowNegativeCash = 1,
            [Description("Set cash target")]
            SetCashTarget = 2,
            [Description("Sell to raise cash")]
            SellToRaiseCash = 3,
        }

        public enum ImportSymbology
        {
            Ticker = 0,
            Sedol = 1,
        }
    }
}
