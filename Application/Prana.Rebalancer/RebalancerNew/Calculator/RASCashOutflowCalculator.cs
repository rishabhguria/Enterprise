using Prana.BusinessObjects.AppConstants;
using Prana.Rebalancer.RebalancerNew.Classes;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Rebalancer.RebalancerNew.Calculator
{
    public class RASCashOutflowCalculator : RASCashflowCalculator
    {
        public override void CalculateCashFlow(SubCalculationModel subCalculationModel, List<RebalancerModel> rebalModels)
        {
            //Use unlocked cash of account to fulfill the cash need.
            if (subCalculationModel.CashLongRebalanceModel != null && !subCalculationModel.CashLongRebalanceModel.IsLock)
            {
                //if cash is long enough then first use cash.
                if (subCalculationModel.CashLongRebalanceModel.TargetPosition >= Math.Abs(subCalculationModel.AccountWiseNAV.CashFlow))
                    subCalculationModel.CashLongRebalanceModel.TargetPosition += subCalculationModel.AccountWiseNAV.CashFlow;
                else
                {
                    //First use complete cash of account
                    subCalculationModel.CashLongRebalanceModel.TargetPosition = 0;
                }
            }
            if (Math.Abs(subCalculationModel.AccountWiseNAV.CashFlow) > 0 && RebalancerCache.Instance.GetTradingRules().IsNegativeCashAllowed)
            {
                if (subCalculationModel.CashShortRebalanceModel != null && !subCalculationModel.CashShortRebalanceModel.IsLock)
                {
                    subCalculationModel.CashShortRebalanceModel.TargetPosition += Math.Abs(subCalculationModel.AccountWiseNAV.CashFlow);
                }
                else
                {
                    //Add short cash rebal model for the required cash
                    RebalancerModel cashRebalanceModel = RebalancerMapper.Instance.CreateCustomRebalModel(RebalancerConstants.CONST_CASH, 0, subCalculationModel.AccountWiseNAV);
                    cashRebalanceModel.IsNewlyAdded = true;
                    cashRebalanceModel.Side = subCalculationModel.AccountWiseNAV.CashFlow > 0 ? BusinessObjects.AppConstants.PositionType.Long : BusinessObjects.AppConstants.PositionType.Short;
                    cashRebalanceModel.TargetPosition = Math.Abs(subCalculationModel.AccountWiseNAV.CashFlow);
                    subCalculationModel.RebalancerModels.Add(cashRebalanceModel);
                }
            }
            //Sell securities if sufficient cash is not available in the account or more cash needed to set the target% or cash is locked.
            else if (subCalculationModel.AccountWiseNAV.CashFlow < 0 && RebalancerCache.Instance.GetTradingRules().IsSellToRaiseCash)
            {
                //Use free cash among rasNonEligibleTrades securities
                List<RebalancerModel> rebalModelsForCashFlow = rebalModels.Where(x => x.IsLock == false && x.Side == PositionType.Long && x.Symbol != RebalancerConstants.CONST_CASH).ToList();
                SubCalculationModel subCashFlowCalcModel = new SubCalculationModel(new CalculationModel()
                {
                    //Excess cash need to be invested for 
                    RebalancerModels = rebalModelsForCashFlow,
                    RoundingTypes = subCalculationModel.RoundingTypes
                },
                subCalculationModel.AccountWiseNAV);
                subCashFlowCalcModel.CashFlow = subCalculationModel.AccountWiseNAV.CashFlow;
                CashFlowCalculatorInstance.CalculateData(subCashFlowCalcModel);
                List<RebalancerModel> newlyGeneratedRebalModels = subCashFlowCalcModel.RebalancerModels.Where(x => x.IsNewlyAdded &&
                    subCalculationModel.RebalancerModels.Where(y => y.AccountId == x.AccountId && y.Symbol == x.Symbol && y.Side == x.Side).Count() == 0).ToList();//filter the trades if added in previous calculation in the case of Modify Rebalance
                subCalculationModel.RebalancerModels.AddRange(newlyGeneratedRebalModels);
            }
        }
    }
}
