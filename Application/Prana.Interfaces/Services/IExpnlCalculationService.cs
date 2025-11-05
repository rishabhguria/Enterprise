using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface IExpnlCalculationService : IServiceOnDemandStatus
    {
        [OperationContract]
        Dictionary<int, decimal> GetDayPNLForSymbolAndAccounts(string symbol, List<int> accountIds, ref StringBuilder errorStringBuilder);
        [OperationContract]
        Dictionary<int, decimal> GetGrossExposureForSymbolAndAccounts(string symbol, List<int> accountIds, ref StringBuilder errorStringBuilder);

        [OperationContract]
        Dictionary<int, decimal> GetAccountNAV(List<int> accountIds, ref StringBuilder errorStringBuilder);

        [OperationContract]
        Dictionary<int, decimal> GetPositionForSymbolAndAccounts(string symbol, List<int> accountIds, ref StringBuilder errorStringBuilder, bool isAddingSwap = true, bool isInMarketIncluded = false);

        /// <summary>
        /// Gets the symbol share out standing.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns>decimal</returns>
        [OperationContract]
        decimal GetSymbolShareOutStanding(string symbol);

        /// <summary>
        /// Gets the position with side for symbol and accounts.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="accountIds">The account ids.</param>
        /// <param name="orderSideTagValue">The order side tag value.</param>
        /// <param name="errorStringBuilder">The error string builder.</param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<int, double> GetPositionWithSideForSymbolAndAccounts(string symbol, List<int> accountIds, bool isInMarketIncluded, string orderSideTagValue);

        [OperationContract]
        decimal GetPXSelectedFeedForSymbol(string symbol, ref StringBuilder errorStringBuilder);

        [OperationContract]
        decimal GetPXSelectedFeedBaseForSymbol(string symbol, ref StringBuilder errorStringBuilder);

        [OperationContract]
        Dictionary<int, decimal> GetFxRateForSymbolAndAccounts(string symbol, List<int> accountIds, int auecId, int currencyID, ref StringBuilder errorStringBuilder);

        [OperationContract]
        Dictionary<int, decimal> GetAccountsStartOfDayNAV(List<int> accountIds, ref StringBuilder errorStringBuilder);

        [OperationContract]
        StringBuilder GetValuesForLeveling(List<TaxLot> taxlotList, List<int> fundList, ref Dictionary<int, decimal> accountWiseNav, ref Dictionary<string, double> groupWiseMarketValue, ref Dictionary<string, Dictionary<int, double>> symbolAccountWiseMarketValue);

        [OperationContract]
        RebalancerData GetRebalancerData(List<int> accountIds, RebalancerEnums.RebalancerPositionsType RebalPositionType, ref StringBuilder errorStringBuilder);

        [OperationContract]
        Dictionary<string, ModelPortfolioSecurityDto> GetModelPortfolioData(List<int> accountIds, RebalancerEnums.RebalancerPositionsType RebalPositionType, ref StringBuilder errorStringBuilder, Dictionary<string, decimal> dictSymbolTolerancePercentage);

        [OperationContract]
        Dictionary<string, decimal> RefreshPrices(List<string> symbolList, ref StringBuilder errorStringBuilder);

        [OperationContract]
        Dictionary<string, Dictionary<int, Tuple<decimal, decimal>>> RefreshPositions(Dictionary<string, List<int>> symbolandAccountIdsInformation, ref StringBuilder errorStringBuilder);

        [OperationContract]
        Dictionary<string, ModelPortfolioSecurityDto> GetModelPortfolios(Dictionary<string, decimal> dictSymbolTargetPercentage, RebalancerEnums.RebalancerPositionsType RebalPositionType, ref StringBuilder errorStringBuilder, Dictionary<string, decimal> dictSymbolTolerancePercentage);

        [OperationContract(IsOneWay = true)]
        void UpdateInMarketTaxlots(List<TaxLot> taxlotList, bool isStartUpData);
    }
}
