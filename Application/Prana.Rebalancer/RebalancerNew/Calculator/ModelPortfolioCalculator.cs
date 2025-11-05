using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public class ModelPortfolioCalculator : IRebalCalculator
    {
        public ModelPortfolioTypeCalculator ModelPortfolioTargetCashCalculatorInstance { get; set; }

        public ModelPortfolioTypeCalculator ModelPortfolioTargetSecurityCalculatorInstance { get; set; }

        public ModelPortfolioCalculator()
        {
            ModelPortfolioTargetCashCalculatorInstance = new ModelPortfolioTargetCashCalculator();
            ModelPortfolioTargetSecurityCalculatorInstance = new ModelPortfolioTargetSecurityCalculator();
        }

        public bool CalculateData(SubCalculationModel calculationModel)
        {
            StringBuilder errorMessage = new StringBuilder();
            ModelPortfolioDto modelPortfolioDto = RebalancerCache.Instance.GetModelPortfolio(calculationModel.ModelPortfolioId);
            if (modelPortfolioDto != null)
            {
                //Get model portfolio for the selected model portfolio id.
                List<ModelPortfolioSecurityDto> portfolioDtos = RebalancerCommon.Instance.GetModelProtfolioData(modelPortfolioDto, calculationModel.RebalPositionType, ref errorMessage);
                Dictionary<string, ModelPortfolioSecurityDto> dictPortfolioDtos = new Dictionary<string, ModelPortfolioSecurityDto>(StringComparer.InvariantCultureIgnoreCase);
                portfolioDtos.ForEach(p => { 
                    if (!dictPortfolioDtos.ContainsKey(p.Symbol)) 
                    { 
                        dictPortfolioDtos.Add(p.Symbol, p);
                    } 
                });
                if (RebalancerCache.Instance.GetCalculationLevel()
                    .Equals(RebalancerEnums.CalculationLevel.MasterFund))
                {
                    decimal multiplier = (calculationModel.AccountWiseNAV.CurrentSecuritiesMarketValue /
                                         RebalancerCache.Instance.GetAccountGroupLevelNAV()
                                             .CurrentTotalNAV);
                    foreach (KeyValuePair<string, ModelPortfolioSecurityDto> dictPortfolioInstance in dictPortfolioDtos)
                    {
                        dictPortfolioInstance.Value.TargetPercentage *= multiplier;
                    }
                }

                if (modelPortfolioDto.ReferenceId == (int)RebalancerEnums.ModelType.TargetCash)
                {
                    ModelPortfolioTargetCashCalculatorInstance.CalculateData(calculationModel, dictPortfolioDtos, modelPortfolioDto);
                }
                else
                {
                    ModelPortfolioTargetSecurityCalculatorInstance.CalculateData(calculationModel, dictPortfolioDtos, modelPortfolioDto);
                }
            }

            calculationModel.CashFlow = calculationModel.AccountWiseNAV.CashFlow;
            return true;
        }

    }
}
