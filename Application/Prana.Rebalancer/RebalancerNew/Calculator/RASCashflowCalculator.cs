using Prana.Rebalancer.RebalancerNew.Models;
using System.Collections.Generic;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public abstract class RASCashflowCalculator
    {
        public CashFlowCalculator CashFlowCalculatorInstance { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RASCashflowCalculator()
        {
            CashFlowCalculatorInstance = new CashFlowCalculator();
        }

        /// <summary>
        /// abstract method to calculate cash inflow and cash outflow in the rebalance across securities calculation.
        /// </summary>
        /// <param name="subCalculationModel"></param>
        /// <param name="rebalModels"></param>
        /// <returns>return newly generated short trades if any</returns>
        public abstract void CalculateCashFlow(SubCalculationModel subCalculationModel, List<RebalancerModel> rebalModels);
    }
}
