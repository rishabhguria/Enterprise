using Prana.Rebalancer.RebalancerNew.Models;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public interface IRebalCalculator
    {
        bool CalculateData(SubCalculationModel calculationModel);
    }
}
