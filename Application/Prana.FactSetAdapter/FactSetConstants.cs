using System.ComponentModel;

namespace Prana.FactSetAdapter
{
    public class FactSetConstants
    {
        public enum AccessType
        {
            Delayed = 0,
            Realtime = 1,
            Denied = 2
        }

        //3.2.4 Table 4: Security Type (FactSet Data Service Manual - Version 2.0J)
        public enum FactSetSecurityType
        {
            None = 0,
            Equity = 1,
            USSecurities = 3,
            OpenAndClosedEndFunds = 4,
            Future = 5,
            USInvestmentTrustsDebt = 6,
            Option = 7,
            Indices = 8,
            USMoneyMarketFunds = 9,
            USInvestmentTrusts = 10,
            CorporateBonds = 12,
            GovernmentTreasuryBondPrices = 13,
            USOTC = 14,
            GlobalMarketStatistics = 15,
            GlobalMarketMovers = 16,
            Cash = 18,
            FX = 19,
            MMAMuniBonds = 21,
            IMMForwardRates = 22,
            MMAMunicipalBonds = 23,
            GovernmentTreasuryBondYields = 24,
            GlobalCashDeposits = 25,
            ShortTermGovernmentSecurities = 27,
            MediumTermGovernmentSecurities = 28,
            LongTermGovernmentSecurities = 29,
            GovernmentStrips = 30,
            USGovernmentAgencyBonds = 31,
            USMortgageBackedSecurities = 32,
            GlobalInflationProtectedSecurities = 34,
            GlobalConsumerPriceIndex = 35,
            USOpenEndMutualFunds = 37,
            USClosedEndFunds = 38,
            USAnnuities = 39,
            USStructuredProducts = 40,
            GlobalWarrants = 41,
            EquityLinkedSecurities = 42,
            MunicipalBonds = 44,
            LoanCertificates = 45,
            NonUSMortgageBonds = 46,
            MunicipalBond = 47,
            SpotPrice = 49,
            EODBenchmarkBonds = 50,
            AlternativeInvestmentProducts = 54,
            AssetSpreads = 57,
            FinraTraceBonds = 58,
            EvaluatedBonds = 59,
            FutureOption = 60,
            ExchangeTradedManagedFunds = 61,
            SBABackedSecuritiesToBeAnnounced = 64,
            SBABackedSecuritiesTradedInPool = 66,
            AssetBackedSecurities = 65,
            CollateralizedMortgageObligations = 67,
            ETFStatistics = 68,
            Metals = 71,
            InvestmentCertificates = 72,
            MutualFundsCollectiveInterestTrust = 87,
            MutualFundSeparatelyManagedAccount = 88,
            MutualFundUnifiedManagedAccount = 89,
            MutualFundSeparateAccounts = 90,
            FXOptions = 91,
            USFlexOptions = 92
        }

        public enum FactSetOptionType
        {
            None = 0,
            Equity = 1,
            Index = 2,
            OOF = 60,
            ETF = 99
        }

        public enum FactSetTickDirection
        {
            [Description("No Tick")]
            NO_TICK = 0,
            [Description("Up Tick")]
            UP_TICK = 1,
            [Description("Down Tick")]
            DOWN_TICK = 2,
            [Description("Up Unchanged")]
            UP_UNCHANGED = 3,
            [Description("Down Unchanged")]
            DOWN_UNCHANGED = 4,
            [Description("End of the day")]
            EOD = 9,
        }

        public const string SAMSARA = "Samsara";
        public const string CLIENT = "Client";
    }
}
