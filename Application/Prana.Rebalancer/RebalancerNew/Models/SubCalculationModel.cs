using Newtonsoft.Json;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.Rebalancer.RebalancerNew.Classes;
using System.Collections.Generic;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class SubCalculationModel : CalculationModel
    {
        public SubCalculationModel(CalculationModel calculationModel, AdjustedAccountLevelNAV accountWiseNAV)
        {
            AccountWiseNAV = accountWiseNAV;
            //Send only unlocked positions of account for rebalancing.
            RebalancerModels = calculationModel.RebalancerModels;
            AccountWiseNAV.MarketValueForCalculation = 0;
            foreach (RebalancerModel rebalancerModel in calculationModel.RebalancerModels)
            {
                if (!rebalancerModel.IsLock)
                {
                    UnLockedRebalModels.Add(rebalancerModel);
                    AccountWiseNAV.MarketValueForCalculation += rebalancerModel.TargetMarketValueBase;
                }
                if (rebalancerModel.Symbol == RebalancerConstants.CONST_CASH)
                {
                    if (rebalancerModel.Side == BusinessObjects.AppConstants.PositionType.Long)
                        CashLongRebalanceModel = rebalancerModel;
                    else if (rebalancerModel.Side == BusinessObjects.AppConstants.PositionType.Short)
                        CashShortRebalanceModel = rebalancerModel;
                }
                if (rebalancerModel.IsModified)
                    IsUserModifiedRebalModelsAvailable = true;
            }
            ModelPortfolioId = calculationModel.ModelPortfolioId;
            RebalPositionType = calculationModel.RebalPositionType;
            RoundingTypes = calculationModel.RoundingTypes;
            if (calculationModel.AccountWiseSecurityDataGridDict != null)
            {
                Dictionary<string, SecurityDataGridModel> securityDataGridTemp = new Dictionary<string, SecurityDataGridModel>();
                if (calculationModel.AccountWiseSecurityDataGridDict.ContainsKey(accountWiseNAV.AccountId))
                    securityDataGridTemp = calculationModel.AccountWiseSecurityDataGridDict[accountWiseNAV.AccountId];
                /*
                 Data from the security data grid is updated for each account . The target percentage for securities is update
                 according to the the NAV contribution of each account in master fund
                 */
                string dictionaryJson = JsonConvert.SerializeObject(securityDataGridTemp);
                SecurityDataGridDict =
                    JsonConvert.DeserializeObject<Dictionary<string, SecurityDataGridModel>>(dictionaryJson);
                if (RebalancerCache.Instance.GetCalculationLevel()
                    .Equals(RebalancerEnums.CalculationLevel.MasterFund))
                {
                    decimal multiplier = (accountWiseNAV.CurrentSecuritiesMarketValue /
                                         RebalancerCache.Instance.GetAccountGroupLevelNAV()
                                             .CurrentTotalNAV);
                    foreach (KeyValuePair<string, SecurityDataGridModel> securityInfo in
                        SecurityDataGridDict)
                    {
                        securityInfo.Value.TargetPercentage *= (multiplier);
                    }
                }
            }
            //Set account cash flow in calculation model cash flow.
            CashFlow = accountWiseNAV.CashFlow;
        }

        public AdjustedAccountLevelNAV AccountWiseNAV { get; set; }

        private List<RebalancerModel> unLockedRebalModels = new List<RebalancerModel>();

        public List<RebalancerModel> UnLockedRebalModels
        {
            get { return unLockedRebalModels; }
            set
            {
                unLockedRebalModels = value;
            }
        }

        private RebalancerModel cashLongRebalanceModel;

        public RebalancerModel CashLongRebalanceModel
        {
            get { return cashLongRebalanceModel; }
            set
            {
                cashLongRebalanceModel = value;
            }
        }

        private RebalancerModel cashShortRebalanceModel;

        public RebalancerModel CashShortRebalanceModel
        {
            get { return cashShortRebalanceModel; }
            set
            {
                cashShortRebalanceModel = value;
            }
        }

        private Dictionary<string, SecurityDataGridModel> securityDataGridDict = new Dictionary<string, SecurityDataGridModel>();

        public Dictionary<string, SecurityDataGridModel> SecurityDataGridDict
        {
            get { return securityDataGridDict; }
            set
            {
                securityDataGridDict = value;
            }
        }

        public bool IsUserModifiedRebalModelsAvailable { get; set; }
    }
}
