using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.BussinessLogic;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Rebalancer.RebalancerNew.Classes
{
    internal class RebalancerMapper : IDisposable
    {
        #region singleton
        private static volatile RebalancerMapper instance;
        private static object syncRoot = new Object();
        internal IRebalancerHelper RebalancerHelperInstance { get; set; }
        private RebalancerMapper()
        {
            RebalancerHelperInstance = new RebalancerHelper();
        }

        public static RebalancerMapper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new RebalancerMapper();
                        }
                    }
                }

                return instance;
            }
        }
        #endregion

        public List<RebalancerModel> MapRebalancerDtosToRebalancerModel(List<RebalancerDto> rebalancerDtos, List<AdjustedAccountLevelNAV> accountWiseNAVDtos)
        {
            Dictionary<int, AdjustedAccountLevelNAV> dictAccountLevelNAV = new Dictionary<int, AdjustedAccountLevelNAV>();
            //Swaps market value calculation is bit differet so we need to calculate market value for all securities using Formula.dll
            Dictionary<int, decimal> dictAccountLevelSecuritiesMV = new Dictionary<int, decimal>();
            Dictionary<int, List<RebalancerModel>> dictAccountLevelCustomRebalModels = new Dictionary<int, List<RebalancerModel>>();
            AccountGroupNAV accountGroupLevelNAV = new AccountGroupNAV { AccountsNAV = accountWiseNAVDtos };
            RebalancerCache.Instance.SetAccountGroupLevelNAV(accountGroupLevelNAV);
            foreach (AdjustedAccountLevelNAV accountLevelNAVItem in accountWiseNAVDtos)
            {
                dictAccountLevelNAV.Add(accountLevelNAVItem.AccountId, accountLevelNAVItem);
                dictAccountLevelSecuritiesMV.Add(accountLevelNAVItem.AccountId, 0);
                List<RebalancerModel> customRebalModels = GetCustomRebalModels(accountLevelNAVItem, true);
                if (customRebalModels.Count > 0)
                {
                    dictAccountLevelCustomRebalModels.Add(accountLevelNAVItem.AccountId, customRebalModels);
                }
            }
            List<RebalancerModel> rebalancerModels = new List<RebalancerModel>();
            foreach (RebalancerDto rebalancerDto in rebalancerDtos)
            {
                RebalancerModel rebalancerModel = new RebalancerModel(rebalancerDto, dictAccountLevelNAV[rebalancerDto.AccountId]);
                dictAccountLevelSecuritiesMV[rebalancerDto.AccountId] += rebalancerModel.CurrentMarketValueBase;
                rebalancerModel.AccountLevelNAV = dictAccountLevelNAV[rebalancerDto.AccountId];
                if (rebalancerModel.Asset == "EquitySwap")
                {
                    rebalancerModel.AccountLevelNAV.UnRealizedPnlOfSwaps -= rebalancerModel.CurrentMarketValueBase;
                }
                rebalancerModels.Add(rebalancerModel);
            }
            //Set securities market value accout wise and also add custom securities market value in it.
            foreach (KeyValuePair<int, decimal> kvp in dictAccountLevelSecuritiesMV)
            {
                decimal securitiesMV;
                securitiesMV = kvp.Value;
                dictAccountLevelNAV[kvp.Key].SecuritiesMarketValue = securitiesMV;
                if (dictAccountLevelCustomRebalModels.ContainsKey(kvp.Key))
                {
                    //Update value for Swaps Unrealized PNL
                    RebalancerModel rebalancerModelSwapUnrealizedPNL = dictAccountLevelCustomRebalModels[kvp.Key].FirstOrDefault(x => x.Symbol == RebalancerConstants.CONST_SWAP_UNREALIZED_PNL);
                    if (rebalancerModelSwapUnrealizedPNL != null)
                    {
                        rebalancerModelSwapUnrealizedPNL.TargetPosition = rebalancerModelSwapUnrealizedPNL.Quantity = Math.Abs(dictAccountLevelNAV[kvp.Key].UnRealizedPnlOfSwaps);
                        rebalancerModelSwapUnrealizedPNL.Side = dictAccountLevelNAV[kvp.Key].UnRealizedPnlOfSwaps >= 0 ? BusinessObjects.AppConstants.PositionType.Long : BusinessObjects.AppConstants.PositionType.Short;
                    }
                    rebalancerModels.AddRange(dictAccountLevelCustomRebalModels[kvp.Key]);
                    securitiesMV += dictAccountLevelCustomRebalModels[kvp.Key].Sum(x => x.CurrentMarketValueBase);
                }
                dictAccountLevelNAV[kvp.Key].CurrentSecuritiesMarketValue = securitiesMV;
                dictAccountLevelNAV[kvp.Key].TargetSecuritiesMarketValue = securitiesMV;
                dictAccountLevelNAV[kvp.Key].MarketValueForCalculation = securitiesMV;
                dictAccountLevelNAV[kvp.Key].CashFlow = 0;
            }
            return rebalancerModels;
        }

        internal List<RebalancerModel> GetCustomRebalModels(AdjustedAccountLevelNAV adjustedAccountLevelNAV, bool isFirstTime)
        {
            List<RebalancerModel> customRebalModels = new List<RebalancerModel>();
            decimal totalMV = adjustedAccountLevelNAV.CurrentSecuritiesMarketValue;
            if (adjustedAccountLevelNAV.IsIncludeCashInBaseCurrency)
            {
                RebalancerModel rebalancerModel = CreateCustomRebalModel(RebalancerConstants.CONST_CASH, adjustedAccountLevelNAV.CashInBaseCurrency, adjustedAccountLevelNAV);
                //Cash should be by default unlocked.                
                rebalancerModel.IsLock = false;
                totalMV += adjustedAccountLevelNAV.CashInBaseCurrency;
                customRebalModels.Add(rebalancerModel);
            }
            if (adjustedAccountLevelNAV.IsIncludeAccrualsInBaseCurrency)
            {
                RebalancerModel rebalancerModel = CreateCustomRebalModel(RebalancerConstants.CONST_ACCRUALS, adjustedAccountLevelNAV.AccrualsInBaseCurrency, adjustedAccountLevelNAV);
                totalMV += adjustedAccountLevelNAV.AccrualsInBaseCurrency;
                customRebalModels.Add(rebalancerModel);
            }
            if (adjustedAccountLevelNAV.IsIncludeOtherAssetsNAV)
            {
                RebalancerModel rebalancerModel = CreateCustomRebalModel(RebalancerConstants.CONST_OTHER_ASSETS_MARKET_VALUE, adjustedAccountLevelNAV.OtherAssetsMarketValue, adjustedAccountLevelNAV);
                totalMV += adjustedAccountLevelNAV.OtherAssetsMarketValue;
                customRebalModels.Add(rebalancerModel);
            }
            if (adjustedAccountLevelNAV.IsIncludeSwapNavAdjustement)
            {
                RebalancerModel rebalancerModelSwapNavAdjustement = CreateCustomRebalModel(RebalancerConstants.CONST_SWAP_NAV_ADJUSTMENT, adjustedAccountLevelNAV.SwapNavAdjustment, adjustedAccountLevelNAV);
                totalMV += adjustedAccountLevelNAV.SwapNavAdjustment;
                customRebalModels.Add(rebalancerModelSwapNavAdjustement);
            }
            if (adjustedAccountLevelNAV.IsIncludeUnrealizedPNLOfSwaps)
            {
                RebalancerModel rebalancerModelSwapUnrealizedPnl = CreateCustomRebalModel(RebalancerConstants.CONST_SWAP_UNREALIZED_PNL, adjustedAccountLevelNAV.UnRealizedPnlOfSwaps, adjustedAccountLevelNAV);
                totalMV += adjustedAccountLevelNAV.UnRealizedPnlOfSwaps;
                customRebalModels.Add(rebalancerModelSwapUnrealizedPnl);
            }
            if (adjustedAccountLevelNAV.IsIncludeUserAdjustedNAV)
            {
                decimal navAdjustmentFactor = isFirstTime ? adjustedAccountLevelNAV.UserAdjustedNAV - adjustedAccountLevelNAV.CurrentSecuritiesMarketValue : adjustedAccountLevelNAV.UserAdjustedNAV - totalMV;
                RebalancerModel rebalancerModel = CreateCustomRebalModel(RebalancerConstants.CONST_NAV_ADJUSTEMENT_Factor, navAdjustmentFactor, adjustedAccountLevelNAV);
                customRebalModels.Add(rebalancerModel);
            }
            return customRebalModels;
        }

        internal RebalancerModel CreateCustomRebalModel(string field, decimal value, AdjustedAccountLevelNAV adjustedAccountLevelNAV)
        {
            RebalancerModel rebalancerModel = new RebalancerModel(new RebalancerDto
            {
                Symbol = field,
                Price = 1,
                Quantity = Math.Abs(value),
                Multiplier = 1,
                FXRate = 1,
                AccountId = adjustedAccountLevelNAV.AccountId,
                Sector = field,
                Asset = field,
                Side = value >= 0 ? BusinessObjects.AppConstants.PositionType.Long : BusinessObjects.AppConstants.PositionType.Short
            },
            adjustedAccountLevelNAV)
            {
                IsLock = (field == RebalancerConstants.CONST_CASH ? false : true),
                IsCustomModel = true
            };
            return rebalancerModel;
        }

        public List<AdjustedAccountLevelNAV> MapAccountLevelNAVDtosToAdjustedAccountLevelNAVDtos(List<AccountLevelNAV> accountLevelNAVDtos)
        {
            List<AdjustedAccountLevelNAV> adjustedAccountLevelNAVDtos = new List<AdjustedAccountLevelNAV>();
            foreach (AccountLevelNAV accountLevelNAVDto in accountLevelNAVDtos)
            {
                AdjustedAccountLevelNAV adjustedAccountLevelNAVDto = new AdjustedAccountLevelNAV(accountLevelNAVDto);
                adjustedAccountLevelNAVDtos.Add(adjustedAccountLevelNAVDto);
            }
            return adjustedAccountLevelNAVDtos;
        }

        internal SecurityDataGridModel GetSecurityDataFromRebalancerModel(RebalancerModel rebalModel)
        {
            SecurityDataGridModel securityDataGridModel = new SecurityDataGridModel()
            {
                Symbol = rebalModel.Symbol,
                FactSetSymbol = rebalModel.FactSetSymbol,
                ActivSymbol = rebalModel.ActivSymbol,
                BloombergSymbol = rebalModel.BloombergSymbol,
                AUECID = rebalModel.AUECID,
                RoundLot = rebalModel.RoundLot,
                IncreaseDecreaseOrSet = RebalancerEnums.RASIncreaseDecreaseOrSet.Set.ToString(),
                TargetPercentage = rebalModel.TargetPercentage,
                Price = rebalModel.Price,
                AccountOrGroupId = rebalModel.AccountId,
                AccountOrGroupName = rebalModel.AccountLevelNAV.AccountName,
                Asset = rebalModel.Asset,
                FXRate = rebalModel.FXRate,
                Multiplier = rebalModel.Multiplier,
                Sector = rebalModel.Sector,
                Delta = rebalModel.Delta,
                LeveragedFactor = rebalModel.LeveragedFactor,
                BPSOrPercentage = RebalancerEnums.BPSOrPercentage.Percentage.ToString(),
                BloombergSymbolWithExchangeCode = rebalModel.BloombergSymbolWithExchangeCode
            };
            return securityDataGridModel;
        }

        #region IDisposable
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (instance != null)
                    instance = null;
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion
    }
}
