using Prana.BusinessObjects.Enumerators.RebalancerNew;
using System.Collections.Generic;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class CalculationModel
    {
        private List<RebalancerModel> rebalancerModels = new List<RebalancerModel>();

        public List<RebalancerModel> RebalancerModels
        {
            get { return rebalancerModels; }
            set
            {
                rebalancerModels = value;
            }
        }

        private decimal cashFlow;

        public decimal CashFlow
        {
            get { return cashFlow; }
            set { cashFlow = value; }
        }

        private int modelPortfolioId;

        public int ModelPortfolioId
        {
            get { return modelPortfolioId; }
            set { modelPortfolioId = value; }
        }

        private RebalancerEnums.RebalancerPositionsType rebalPositionType;

        public RebalancerEnums.RebalancerPositionsType RebalPositionType
        {
            get { return rebalPositionType; }
            set { rebalPositionType = value; }
        }

        private RebalancerEnums.RoundingTypes roundingTypes;

        public RebalancerEnums.RoundingTypes RoundingTypes
        {
            get { return roundingTypes; }
            set { roundingTypes = value; }
        }

        public Dictionary<int, Dictionary<string, SecurityDataGridModel>> AccountWiseSecurityDataGridDict { get; set; }
    }
}
