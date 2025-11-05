namespace Prana.BusinessObjects
{
    public class RiskConstants
    {
        // LAMBDA - Decay
        public const double CONST_LAMBDA = 0.94;
        public const double CONST_INCREMENT = 0.01;
        public enum VolType
        {
            Normal = 1,
            EWMA = 2,
            GARCH = 3
        }

        public enum Method
        {
            VolatilityBased = 1,
            HistoricalSimulation = 2,
            VarianceCovariance = 3,
            HistoricalSimulationPercentReturns = 4,
            HistoricalSimulationLognormalReturns = 5,
            HistoricalSimulationLossesIncludedAsNegatives = 6,
            HistoricalSimulationPercentReturnsLossesIncludedAsNegatives = 7,
            HistoricalSimulationLognormalReturnsLossesIncludedAsNegatives = 8,
            FullHistoricalSimulation = 9,
            FullHistoricalSimulationPercentReturns = 10,
            FullHistoricalSimulationLognormalReturns = 11,
            DecayedHistoricalSimulation = 12,
            DecayedHistoricalSimulationPercentReturns = 13,
            DecayedHistoricalSimulationLognormalReturns = 14,
            MonteCarlo = 15
        }

        public enum RiskCalculationBasedOn
        {
            Quantity = 1,
            DeltaAdjPosition = 2
        }
    }
}
