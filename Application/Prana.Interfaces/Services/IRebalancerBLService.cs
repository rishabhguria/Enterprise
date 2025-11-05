using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface IRebalancerBLService : IServiceOnDemandStatus
    {
        [OperationContract]
        List<ModelPortfolioDto> GetModelPortfolios();

        [OperationContract]
        Dictionary<int, List<int>> GetAllCustomFundGroupsMapping();

        [OperationContract]
        Dictionary<int, string> GetAllCustomFundGroups();

        [OperationContract]
        bool SaveEditModelPortfolio(ModelPortfolioDto modelPortfolioDto, bool isEdit);

        [OperationContract]
        bool DeleteModelPortfolio(int modelPortfolioId);

        [OperationContract]
        bool SaveCustomGroupMapping(CustomGroupDto customGroupDto);

        [OperationContract]
        bool UpdateRebalPreferences(RebalPreferencesDto rebalPreferences);

        [OperationContract]
        Dictionary<Tuple<int, string>, string> GetRebalPreferences();

        [OperationContract]
        bool SaveRebalancerTradeList(DateTime SelectedDate, string SmartName, int id, string TradeList);

        [OperationContract]
        Dictionary<string, int> GetRebalancerTradeListNames(DateTime selectedDate);

        [OperationContract]
        string GetTradeList(int tradeListId);

        [OperationContract]
        string GetSmartName();

        [OperationContract]
        bool DeleteCustomGroupMapping(int customGroupId);

        [OperationContract]
        bool UpdateRebalPreferencesForAllAccounts(string preferenceKey, Dictionary<int, string> preferenceDictionary);
    }
}
