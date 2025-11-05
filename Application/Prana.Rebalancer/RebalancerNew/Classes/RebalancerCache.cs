using Prana.BusinessObjects.Classes.RebalancerNew;
using Prana.BusinessObjects.Enumerators.RebalancerNew;
using Prana.CommonDataCache;
using Prana.Rebalancer.RebalancerNew.BussinessLogic;
using Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces;
using Prana.Rebalancer.RebalancerNew.Models;
using Prana.ServiceConnector;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Rebalancer.RebalancerNew.Classes
{
    internal class RebalancerCache
    {
        #region singleton
        private static volatile RebalancerCache instance;
        private static object syncRoot = new Object();
        internal IRebalancerHelper RebalancerHelperInstance { get; set; }
        private RebalancerCache()
        {
            RebalancerHelperInstance = new RebalancerHelper();
        }

        public static RebalancerCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new RebalancerCache();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion

        internal event EventHandler<EventArgs> UpdateModelPortfolios;
        internal event EventHandler<EventArgs> UpdateCustomGroups;
        internal event EventHandler<EventArgs> UpdateDataPreferencsOnRebalancerView;
        internal string SelectedTab { get; set; }
        internal void FillRebalancerCache()
        {
            Parallel.Invoke(() => { SetModelPortfolios(RebalancerServiceApi.GetInstance().GetModelPortfolios()); }
                , () => { SetCustomGroupAndAccountsAssociation(RebalancerServiceApi.GetInstance().GetAllCustomFundGroupsMapping()); },
                () => { SetCustomGroupsDictionary(RebalancerServiceApi.GetInstance().GetAllCustomFundGroups()); }
                , () => { SetRebalPreferencesDictionary(RebalancerServiceApi.GetInstance().GetRebalPreferences()); });
        }

        internal void ClearCache()
        {
            symbolWisePriceAndFx = null;
            _modelPortfolios = null;
            _customGroupAndAccountsAssociation = null;
            _customGroupsDictionary = null;
            _rebalPreferences = null;
            accountGroupLevelNAV = null;
            securitiesTradingRules = null;
            instance = null;
        }

        #region symbol wise price and fx

        private Dictionary<string, PriceAndFx> symbolWisePriceAndFx = new Dictionary<string, PriceAndFx>();

        private Dictionary<string, PriceAndFx> SymbolWisePriceAndFx
        {
            get { return symbolWisePriceAndFx; }
            set { symbolWisePriceAndFx = value; }
        }

        internal void SetSymbolWisePriceAndFx(Dictionary<string, PriceAndFx> symbolWisePriceAndFx)
        {
            SymbolWisePriceAndFx = symbolWisePriceAndFx;
        }

        /// <summary>
        /// Get Symbol Wise Price
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal decimal GetSymbolPrice(string symbol)
        {
            decimal price = 0;
            if (SymbolWisePriceAndFx.ContainsKey(symbol))
            {
                return SymbolWisePriceAndFx[symbol].price;
            }
            return price;
        }

        /// <summary>
        /// Get Symbol Wise Price
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal decimal GetSymbolFx(string symbol)
        {
            decimal fx = 1;
            if (SymbolWisePriceAndFx.ContainsKey(symbol))
            {
                return SymbolWisePriceAndFx[symbol].fx;
            }
            return fx;
        }

        internal void AddOrUpdateSymbolWisePriceAndFx(string symbol, decimal price, decimal fx)
        {
            PriceAndFx priceAndFx = new PriceAndFx();
            priceAndFx.price = price;
            priceAndFx.fx = fx;
            if (SymbolWisePriceAndFx.ContainsKey(symbol))
            {
                SymbolWisePriceAndFx[symbol] = priceAndFx;
            }
            else
            {
                SymbolWisePriceAndFx.Add(symbol, priceAndFx);
            }
        }

        /// <summary>
        /// Chech Symbol Exist in Grid
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        internal bool IsSymbolExistInGrid(string symbol)
        {
            if (SymbolWisePriceAndFx.ContainsKey(symbol))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Model Portfolios dictionary key = ModelPOrtfolioId,Value = Model POrtfolio Data

        private ConcurrentDictionary<int, ModelPortfolioDto> _modelPortfolios = new ConcurrentDictionary<int, ModelPortfolioDto>();

        internal bool AddOrUpdateModelPortfolio(ModelPortfolioDto modelPortfolioDto)
        {
            _modelPortfolios.AddOrUpdate(modelPortfolioDto.Id, modelPortfolioDto, (oldkey, oldvalue) => modelPortfolioDto);
            if (UpdateModelPortfolios != null)
                UpdateModelPortfolios(null, null);
            return true;
        }

        internal bool DeleteModelPortfolio(int modelPortfolioId)
        {
            ModelPortfolioDto modelPortfolioDto;
            _modelPortfolios.TryRemove(modelPortfolioId, out modelPortfolioDto);
            if (UpdateModelPortfolios != null)
                UpdateModelPortfolios(null, null);
            return true;
        }

        internal bool SetModelPortfolios(List<ModelPortfolioDto> modelPortfolioDtos)
        {
            _modelPortfolios = new ConcurrentDictionary<int, ModelPortfolioDto>();
            if (modelPortfolioDtos != null)
            {
                foreach (ModelPortfolioDto modelPortfolioDto in modelPortfolioDtos)
                {
                    _modelPortfolios.TryAdd(modelPortfolioDto.Id, modelPortfolioDto);
                }
            }
            if (UpdateModelPortfolios != null)
                UpdateModelPortfolios(null, null);
            return true;
        }

        internal ModelPortfolioDto GetModelPortfolio(int modelPortfolioId)
        {
            ModelPortfolioDto modelPortfolioDto;
            _modelPortfolios.TryGetValue(modelPortfolioId, out modelPortfolioDto);
            return modelPortfolioDto;
        }

        internal ObservableCollection<KeyValueItem> GetAllModelPortfolioNames()
        {
            return new ObservableCollection<KeyValueItem>(_modelPortfolios
                .Values.Select(p => new KeyValueItem { ItemValue = p.ModelPortfolioName, Key = p.Id }).ToList());
        }

        #endregion

        #region All Custom Groups Dictionary

        private ConcurrentDictionary<int, string> _customGroupsDictionary = new ConcurrentDictionary<int, string>();

        internal ConcurrentDictionary<int, string> GetCustomGroupsDictionary()
        {
            return _customGroupsDictionary;
        }

        internal ConcurrentDictionary<int, string> GetCustomGroupsDictionaryOnTheBasisOfPermittedFund()
        {
            ConcurrentDictionary<int, string> dictCustomGroups = new ConcurrentDictionary<int, string>();

            foreach (KeyValuePair<int, string> kvp in _customGroupsDictionary)
            {
                if (_customGroupAndAccountsAssociation.ContainsKey(kvp.Key) && _customGroupAndAccountsAssociation[kvp.Key][true].Count > 0)
                {
                    dictCustomGroups[kvp.Key] = _customGroupsDictionary[kvp.Key];
                }
            }
            return dictCustomGroups;
        }

        internal void SetCustomGroupsDictionary(Dictionary<int, string> customGroupsDictionary)
        {
            if (customGroupsDictionary != null)
                _customGroupsDictionary = new ConcurrentDictionary<int, string>(customGroupsDictionary);
            if (UpdateCustomGroups != null)
                UpdateCustomGroups(null, null);
        }

        internal void AddOrUpdateCustomGroupsDictionary(int groupId, string groupName)
        {
            if (_customGroupsDictionary.ContainsKey(groupId) && _customGroupsDictionary[groupId] != groupName)
            {
                _customGroupsDictionary[groupId] = groupName;
                if (UpdateCustomGroups != null)
                    UpdateCustomGroups(null, null);
            }
            else if (!_customGroupsDictionary.ContainsKey(groupId))
            {
                _customGroupsDictionary.AddOrUpdate(groupId, groupName, (oldkey, oldvalue) => groupName);
                if (UpdateCustomGroups != null)
                    UpdateCustomGroups(null, null);
            }
        }

        internal void DeleteCustomGroupsDictionary(int groupId)
        {
            string deletedCustomGroup;
            _customGroupsDictionary.TryRemove(groupId, out deletedCustomGroup);
        }

        internal string GetCustomGroupName(int groupId)
        {
            string groupName = string.Empty;
            if (_customGroupsDictionary.ContainsKey(groupId))
                groupName = _customGroupsDictionary[groupId];
            return groupName;
        }

        internal void DeleteCustomGroup(int customGroupId)
        {
            DeleteCustomGroupAssociatedAccounts(customGroupId);
            DeleteCustomGroupsDictionary(customGroupId);
            if (UpdateCustomGroups != null)
                UpdateCustomGroups(null, null);
        }

        #endregion

        #region Custom Groups and assosciated accounts dictionary

        /// <summary>
        /// Dictionary with custom group id as key and list of associated accounts as value
        /// </summary>
        private ConcurrentDictionary<int, ConcurrentDictionary<bool, List<int>>> _customGroupAndAccountsAssociation = new ConcurrentDictionary<int, ConcurrentDictionary<bool, List<int>>>();

        internal void SetCustomGroupAndAccountsAssociation(Dictionary<int, List<int>> customGroupAndAccountsAssociation)
        {
            if (customGroupAndAccountsAssociation != null)
            {
                Dictionary<int, string> permittedAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
                _customGroupAndAccountsAssociation = new ConcurrentDictionary<int, ConcurrentDictionary<bool, List<int>>>();
                foreach (KeyValuePair<int, List<int>> kvp in customGroupAndAccountsAssociation)
                {
                    AddNewCustomGroup(permittedAccounts, kvp.Key, kvp.Value);
                }
            }
        }

        private void AddNewCustomGroup(Dictionary<int, string> permittedAccounts, int groupId, List<int> accountsMapping)
        {
            _customGroupAndAccountsAssociation.TryAdd(groupId, new ConcurrentDictionary<bool, List<int>>());
            _customGroupAndAccountsAssociation[groupId].TryAdd(true, new List<int>());
            _customGroupAndAccountsAssociation[groupId].TryAdd(false, new List<int>());
            foreach (int cgAccountId in accountsMapping)
            {
                if (permittedAccounts.ContainsKey(cgAccountId))
                {
                    _customGroupAndAccountsAssociation[groupId][true].Add(cgAccountId);
                }
                else
                {
                    _customGroupAndAccountsAssociation[groupId][false].Add(cgAccountId);
                }
            }
        }

        internal ConcurrentDictionary<int, List<int>> GetCustomGroupAndAccountsAssociation()
        {
            ConcurrentDictionary<int, List<int>> dictCustomGroupAndAccountsAssociation = new ConcurrentDictionary<int, List<int>>();
            foreach (KeyValuePair<int, ConcurrentDictionary<bool, List<int>>> kvp in _customGroupAndAccountsAssociation)
            {
                if (_customGroupAndAccountsAssociation[kvp.Key][true].Count > 0)
                    dictCustomGroupAndAccountsAssociation.TryAdd(kvp.Key, _customGroupAndAccountsAssociation[kvp.Key][true]);
            }
            return dictCustomGroupAndAccountsAssociation;
        }

        internal List<int> GetCustomGroupAssociatedAccounts(int groupId)
        {
            if (_customGroupAndAccountsAssociation.ContainsKey(groupId))
                return _customGroupAndAccountsAssociation[groupId][true];
            return new List<int>();
        }

        internal List<int> GetCustomGroupNonPermittedAccounts(int groupId)
        {
            if (_customGroupAndAccountsAssociation.ContainsKey(groupId))
                return _customGroupAndAccountsAssociation[groupId][false];
            return new List<int>();
        }

        internal void AddOrUpdateCustomGroupAssociatedAccounts(int groupId, List<int> accountsMapping)
        {
            Dictionary<int, string> permittedAccounts = CachedDataManager.GetInstance.GetUserAccountsAsDict();
            if (_customGroupAndAccountsAssociation.ContainsKey(groupId))
            {
                _customGroupAndAccountsAssociation[groupId][true].Clear();
                _customGroupAndAccountsAssociation[groupId][false].Clear();
                foreach (int cgAccountId in accountsMapping)
                {
                    if (permittedAccounts.ContainsKey(cgAccountId))
                    {
                        _customGroupAndAccountsAssociation[groupId][true].Add(cgAccountId);
                    }
                    else
                    {
                        _customGroupAndAccountsAssociation[groupId][false].Add(cgAccountId);
                    }
                }
            }
            else
            {
                AddNewCustomGroup(permittedAccounts, groupId, accountsMapping);
            }
            if (UpdateCustomGroups != null)
                UpdateCustomGroups(null, null);
        }

        internal void DeleteCustomGroupAssociatedAccounts(int groupId)
        {
            List<int> deletedMappedAccounts;
            ConcurrentDictionary<bool, List<int>> deletedCustomGroup;
            _customGroupAndAccountsAssociation[groupId].TryRemove(true, out deletedMappedAccounts);
            _customGroupAndAccountsAssociation.TryRemove(groupId, out deletedCustomGroup);
        }

        internal bool IsCanDeleteCustomGroup(int groupId, StringBuilder accountName)
        {
            if (_customGroupAndAccountsAssociation.ContainsKey(groupId) && _customGroupAndAccountsAssociation[groupId].ContainsKey(false))
            {
                if (_customGroupAndAccountsAssociation[groupId][false].Count > 0)
                {
                    int i = 0;
                    for (; i < _customGroupAndAccountsAssociation[groupId][false].Count; i++)
                    {
                        if (i <= 4)
                        {
                            accountName.Append(
                                CachedDataManager.GetInstance.GetAccountText(
                                    _customGroupAndAccountsAssociation[groupId][false][i]));
                        }
                        else
                        {
                            accountName.Remove(accountName.Length - 2, 2);
                            accountName.Append(" and ");
                            accountName.Append(_customGroupAndAccountsAssociation[groupId][false].Count - i);
                            accountName.Append(" more accounts");
                            break;
                        }
                        accountName.Append(", ");
                    }
                    if (i <= 4)
                        accountName.Remove(accountName.Length - 2, 2);

                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Rebalancer Preferences

        private ConcurrentDictionary<Tuple<int, string>, string> _rebalPreferences = new ConcurrentDictionary<Tuple<int, string>, string>();

        internal ConcurrentDictionary<Tuple<int, string>, string> GetRebalPreferencesDictionary()
        {
            return _rebalPreferences;
        }

        internal void SetRebalPreferencesDictionary(Dictionary<Tuple<int, string>, string> rebalPreferences)
        {
            if (rebalPreferences != null)
                _rebalPreferences = new ConcurrentDictionary<Tuple<int, string>, string>(rebalPreferences);
        }

        internal string GetRebalPreference(string preferenceKey, int accountId)
        {
            string preferenceValue = string.Empty;
            var key = Tuple.Create(accountId, preferenceKey);
            if (_rebalPreferences.ContainsKey(key))
                preferenceValue = _rebalPreferences[key];
            else
            {
                key = Tuple.Create(0, preferenceKey);
                if (_rebalPreferences.ContainsKey(key))
                    preferenceValue = _rebalPreferences[key];
            }
            return preferenceValue;
        }

        internal void AddOrUpdateRebalPreference(int accountId, string preferenceKey, string preferenceValue)
        {
            var key = Tuple.Create(accountId, preferenceKey);
            _rebalPreferences.AddOrUpdate(key, preferenceValue, (oldkey, oldvalue) => preferenceValue);
            if (UpdateDataPreferencsOnRebalancerView != null)
                UpdateDataPreferencsOnRebalancerView(null, null);
        }

        public void AddOrUpdateRebalPreferenceForAllAccounts(string preferenceKey, Dictionary<int, string> navImpactingPreferenceDictionary)
        {
            foreach (KeyValuePair<int, string> memberKeyValuePair in navImpactingPreferenceDictionary)
            {
                AddOrUpdateRebalPreference(memberKeyValuePair.Key, preferenceKey, memberKeyValuePair.Value);
            }
        }

        #endregion

        #region rounding type

        private RebalancerEnums.RoundingTypes selectedRoundingType = RebalancerEnums.RoundingTypes.RoundDown;

        private RebalancerEnums.RoundingTypes SelectedRoundingType
        {
            get { return selectedRoundingType; }
            set { selectedRoundingType = value; }
        }


        internal void SetRoundingType(int roundingType)
        {
            SelectedRoundingType = (RebalancerEnums.RoundingTypes)roundingType;
        }

        internal RebalancerEnums.RoundingTypes GetRoundingType()
        {
            return SelectedRoundingType;
        }

        #endregion

        private bool isUseRoundLot;

        internal bool IsUseRoundLot
        {
            get { return isUseRoundLot; }
            set { isUseRoundLot = value; }
        }

        #region calculation level

        private RebalancerEnums.CalculationLevel selectedCalculationLevel = RebalancerEnums.CalculationLevel.Account;

        private RebalancerEnums.CalculationLevel SelectedCalculationLevel
        {
            get { return selectedCalculationLevel; }
            set { selectedCalculationLevel = value; }
        }


        internal void SetCalculationLevel(int calculationLevel)
        {
            SelectedCalculationLevel = (RebalancerEnums.CalculationLevel)calculationLevel;
        }

        internal RebalancerEnums.CalculationLevel GetCalculationLevel()
        {
            return SelectedCalculationLevel;
        }

        #endregion

        #region Account Group

        private AccountGroupNAV accountGroupLevelNAV = new AccountGroupNAV();

        private AccountGroupNAV AccountGroupLevelNAV
        {
            get { return accountGroupLevelNAV; }
            set { accountGroupLevelNAV = value; }
        }


        internal void SetAccountGroupLevelNAV(AccountGroupNAV accountGroupNAV)
        {
            AccountGroupLevelNAV = accountGroupNAV;
        }

        internal AccountGroupNAV GetAccountGroupLevelNAV()
        {
            return AccountGroupLevelNAV;
        }

        #endregion

        #region Cash Flow Impact

        private RebalancerEnums.CashFlowImpactOnNAV selectedCashFlowImpact = RebalancerEnums.CashFlowImpactOnNAV.ImpactNAV;

        private RebalancerEnums.CashFlowImpactOnNAV SelectedCashFlowImpact
        {
            get { return selectedCashFlowImpact; }
            set { selectedCashFlowImpact = value; }
        }


        internal void SetCashFlowImpact(int cashFlowImpact)
        {
            SelectedCashFlowImpact = (RebalancerEnums.CashFlowImpactOnNAV)cashFlowImpact;
        }

        internal RebalancerEnums.CashFlowImpactOnNAV GetCashFlowImpact()
        {
            return SelectedCashFlowImpact;
        }

        #endregion

        #region Trading Rules

        private TradingRules securitiesTradingRules = null;

        private TradingRules SecuritiesTradingRules
        {
            get { return securitiesTradingRules; }
            set { securitiesTradingRules = value; }
        }


        internal void SetTradingRules(TradingRules tradingRules)
        {
            SecuritiesTradingRules = tradingRules;
        }

        internal TradingRules GetTradingRules()
        {
            return SecuritiesTradingRules;
        }

        #endregion

        private Dictionary<int, string> accountsOrGroupsList = new Dictionary<int, string>();
        internal Dictionary<int, string> AccountsOrGroupsList
        {
            get { return accountsOrGroupsList; }
            set { accountsOrGroupsList = value; }
        }
    }
}
