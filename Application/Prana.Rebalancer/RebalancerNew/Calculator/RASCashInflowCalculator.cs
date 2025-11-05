using Prana.BusinessObjects.AppConstants;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public class RASCashInflowCalculator : RASCashflowCalculator
    {
        public override void CalculateCashFlow(SubCalculationModel subCalculationModel, List<RebalancerModel> rebalModels)
        {
            //Use free cash among rasNonEligibleTrades securities
            List<RebalancerModel> rebalModelsForCashFlow = new List<RebalancerModel>();
            //As per the requirement spec cash should be invested across unlocked and long positions only.
            rebalModelsForCashFlow = rebalModels.Where(x => x.IsLock == false && x.Side == PositionType.Long).ToList();
            if (RebalancerCache.Instance.GetTradingRules().IsReInvestCash || RebalancerCache.Instance.GetTradingRules().IsSetCashTarget)
            {
                SubCalculationModel subCashFlowCalcModel = new SubCalculationModel(new CalculationModel()
                {
                    //Excess cash need to be invested for 
                    RebalancerModels = rebalModelsForCashFlow,
                    RoundingTypes = subCalculationModel.RoundingTypes
                },
                subCalculationModel.AccountWiseNAV);
                subCashFlowCalcModel.CashFlow = subCalculationModel.AccountWiseNAV.CashFlow;
                CashFlowCalculatorInstance.CalculateData(subCashFlowCalcModel);
            }
            else if (RebalancerCache.Instance.GetTradingRules().IsIncreaseCashPosition)
            {
                //If there is cash in flow then first increase cash for short cash security
                if (subCalculationModel.CashShortRebalanceModel != null)
                {
                    if (subCalculationModel.CashShortRebalanceModel.TargetPosition <= subCalculationModel.AccountWiseNAV.CashFlow)
                        subCalculationModel.CashShortRebalanceModel.TargetPosition = 0;
                    else
                    {
                        subCalculationModel.CashShortRebalanceModel.TargetPosition -= subCalculationModel.AccountWiseNAV.CashFlow;
                    }
                }
                if (subCalculationModel.AccountWiseNAV.CashFlow > 0)
                {
                    if (subCalculationModel.CashLongRebalanceModel != null)
                    {
                        subCalculationModel.CashLongRebalanceModel.TargetPosition += subCalculationModel.AccountWiseNAV.CashFlow;
                    }
                    else
                    {
                        RebalancerModel cashRebalanceModel = RebalancerMapper.Instance.CreateCustomRebalModel(RebalancerConstants.CONST_CASH, 0, subCalculationModel.AccountWiseNAV);
                        cashRebalanceModel.IsNewlyAdded = true;
                        cashRebalanceModel.TargetPosition = subCalculationModel.AccountWiseNAV.CashFlow;
                        subCalculationModel.RebalancerModels.Add(cashRebalanceModel);
                        subCalculationModel.CashLongRebalanceModel = cashRebalanceModel;
                    }
                }
            }
        }
    }
}
