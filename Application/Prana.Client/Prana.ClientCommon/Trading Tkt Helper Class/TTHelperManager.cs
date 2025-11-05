using Infragistics.Win;
using Prana.AlgoStrategyControls;
using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.CommonDataCache;
using Prana.DatabaseManager;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.TradeManager.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Prana.ClientCommon
{
    /// <summary>
    /// Summary description for TTHelperManager.
    /// </summary>
    public class TTHelperManager
    {
        Dictionary<string, TradingTicketValues> tktNames = new Dictionary<string, TradingTicketValues>();

        Dictionary<string, ValueList> counterPartyValueListCollection = new Dictionary<string, ValueList>();
        Dictionary<string, ValueList> venueValueListCollection = new Dictionary<string, ValueList>();
        Dictionary<int, string> dictCounterPartyCollection = new Dictionary<int, string>();
        Dictionary<int, string> dictVenueCollection = new Dictionary<int, string>();
        public IAllocationManager AllocationManager { get; set; }
        int _userID = int.MinValue;
        private TTHelperManager()
        {
        }

        private static readonly TTHelperManager _ttManager = new TTHelperManager();
        public static TTHelperManager GetInstance()
        {
            return _ttManager;
        }

        /// <summary>
        /// Set Helper Collection
        /// </summary>
        /// <param name="CompanyID"></param>
        /// <param name="UserID"></param>
        public void SetHelperCollection(int CompanyID, int UserID)
        {
            if (UserID != _userID)
            {
                TTHelperManagerExtension.GetInstance().GetCVAUECMappings(CompanyID, UserID);
                TTHelperManagerExtension.GetInstance().GetCVAccountMappings(CompanyID);
                TTHelperManagerExtension.GetInstance().GetFundWiseExecutingBrokerMappingFromDB(CompanyID);
                CreateDictionaries();
                LoadAlgoStrategyControls();
                _userID = UserID;
                TTHelperManagerExtension.GetInstance().UserID = _userID;
            }
        }

        /// <summary>
        /// Load Algo Strategy Controls
        /// </summary>
        private void LoadAlgoStrategyControls()
        {
            try
            {
                AlgoControlsDictionary.GetInstance().GetLoadStrategyControls();
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

        /// <summary>
        /// Get Counter parties
        /// </summary>
        /// <param name="assetID"></param>
        /// <param name="underLyingID"></param>
        /// <param name="auecID"></param>
        /// <returns></returns>
        public ValueList GetCounterparties(int assetID, int underLyingID, int auecID)
        {
            ValueList counterparties = new ValueList();
            if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
            {
                counterparties = GetCounterparties(auecID);
            }
            else
            {
                counterparties = GetCounterparties(assetID, underLyingID);
            }
            return counterparties;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetID"></param>
        /// <param name="underLyingID"></param>
        /// <returns></returns>
        public ValueList GetCounterparties(int assetID, int underLyingID)
        {
            var key = TTHelperManagerExtension.GetInstance().AUKey(assetID, underLyingID);

            if (counterPartyValueListCollection.ContainsKey(key))
            {
                return counterPartyValueListCollection[key];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auecID"></param>
        /// <returns></returns>
        public ValueList GetCounterparties(int auecID)
        {
            var key = TTHelperManagerExtension.GetInstance().AUECKey(auecID);
            if (counterPartyValueListCollection.ContainsKey(key))
            {
                return counterPartyValueListCollection[key];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// This method is used in the work area preferences
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetCounterparties()
        {
            return dictCounterPartyCollection;
        }

        /// <summary>
        /// GetVenues By CounterParty
        /// </summary>
        /// <param name="counterPartyID"></param>
        /// <param name="assetID"></param>
        /// <param name="underLyingID"></param>
        /// <param name="auecId"></param>
        /// <returns></returns>
        public ValueList GetVenuesByCounterPartyID(int counterPartyID, int assetID, int underLyingID, int auecId)
        {
            ValueList venueList = new ValueList();
            if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
            {
                venueList = GetVenuesByCounterPartyID(counterPartyID, auecId);
            }
            else
            {
                venueList = GetVenuesByCounterPartyID(counterPartyID, assetID, underLyingID);
            }
            return venueList;
        }


        /// <summary>
        /// Get Venues By CounterParty , asset and underLying
        /// </summary>
        /// <param name="counterPartyID"></param>
        /// <param name="assetID"></param>
        /// <param name="underLyingID"></param>
        /// <returns></returns>
        private ValueList GetVenuesByCounterPartyID(int counterPartyID, int assetID, int underLyingID)
        {
            // If AUC key is not found in venueValueListCollection, then return blank valuelist
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-9512
            var key = TTHelperManagerExtension.GetInstance().AUCKey(assetID, underLyingID, counterPartyID);
            if (venueValueListCollection.ContainsKey(key))
                return venueValueListCollection[key];
            else
                return new ValueList();
        }

        /// <summary>
        /// Get Venues By Counter Party ID and AUEC
        /// </summary>
        /// <param name="counterPartyID"></param>
        /// <param name="auecId"></param>
        /// <returns></returns>
        public ValueList GetVenuesByCounterPartyID(int counterPartyID, int auecId)
        {
            try
            {
                var key = TTHelperManagerExtension.GetInstance().AUECCouterPartyKey(auecId, counterPartyID);
                if (venueValueListCollection.ContainsKey(key))
                    return venueValueListCollection[key];
                else
                    return new ValueList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This method is used in the work area preferences
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetVenues()
        {
            return dictVenueCollection;
        }

        private void CreateDictionaries()
        {
            try
            {
                counterPartyValueListCollection.Clear();
                venueValueListCollection.Clear();
                dictVenueCollection.Clear();
                dictCounterPartyCollection.Clear();
                tktNames.Clear();

                if (CachedDataManager.GetInstance.IsShowMasterFundonTT())
                {
                    #region CounterParties based on AUEC
                    foreach (TTHelper tt in TTHelperManagerExtension.GetInstance().TTCollection)
                    {
                        string key = TTHelperManagerExtension.GetInstance().AUECKey(tt.AuecID);
                        TTHelperManagerExtension.GetInstance().AddCounterparties(key, tt);
                    }
                    foreach (KeyValuePair<string, Dictionary<int, string>> keyvaluepair in TTHelperManagerExtension.GetInstance().CounterPartyCollection)
                    {
                        ValueList vl = new ValueList();
                        foreach (KeyValuePair<int, string> cp in keyvaluepair.Value)
                        {
                            vl.ValueListItems.Add(cp.Key, cp.Value);

                            //Fill all the counter parties in the dictionary
                            if (!dictCounterPartyCollection.ContainsKey(cp.Key))
                            {
                                dictCounterPartyCollection.Add(cp.Key, cp.Value);
                            }
                        }
                        counterPartyValueListCollection.Add(keyvaluepair.Key, vl);
                    }
                    foreach (KeyValuePair<string, Dictionary<int, string>> keyvaluepair in TTHelperManagerExtension.GetInstance().VenueCollection)
                    {
                        ValueList vl = new ValueList();
                        foreach (KeyValuePair<int, string> venue in keyvaluepair.Value)
                        {
                            vl.ValueListItems.Add(venue.Key, venue.Value);
                            //Fill all the venues in the dictionary
                            if (!dictVenueCollection.ContainsKey(venue.Key))
                            {
                                dictVenueCollection.Add(venue.Key, venue.Value);
                            }
                        }
                        venueValueListCollection.Add(keyvaluepair.Key, vl);
                    }
                    #endregion
                }
                else
                {
                    #region CouperParties based on Asset And underlying
                    foreach (TTHelper tt in TTHelperManagerExtension.GetInstance().TTCollection)
                    {
                        string key = TTHelperManagerExtension.GetInstance().AUKey(tt.AssetID, tt.UnderlyingID);
                        if (!tktNames.ContainsKey(key))
                        {
                            TradingTicketValues ttvalues = new TradingTicketValues(tt.UnderlyingName, tt.AssetID, tt.UnderlyingID);
                            tktNames.Add(key, ttvalues);
                        }
                        TTHelperManagerExtension.GetInstance().AddCounterparties(key, tt);

                    }
                    foreach (KeyValuePair<string, Dictionary<int, string>> keyvaluepair in TTHelperManagerExtension.GetInstance().CounterPartyCollection)
                    {
                        ValueList vl = new ValueList();
                        foreach (KeyValuePair<int, string> cp in keyvaluepair.Value)
                        {
                            vl.ValueListItems.Add(cp.Key, cp.Value);

                            //Fill all the counter parties in the dictionary
                            if (!dictCounterPartyCollection.ContainsKey(cp.Key))
                            {
                                dictCounterPartyCollection.Add(cp.Key, cp.Value);
                            }
                        }
                        counterPartyValueListCollection.Add(keyvaluepair.Key, vl);
                    }
                    foreach (KeyValuePair<string, Dictionary<int, string>> keyvaluepair in TTHelperManagerExtension.GetInstance().VenueCollection)
                    {
                        ValueList vl = new ValueList();
                        foreach (KeyValuePair<int, string> venue in keyvaluepair.Value)
                        {
                            vl.ValueListItems.Add(venue.Key, venue.Value);
                            //Fill all the venues in the dictionary
                            if (!dictVenueCollection.ContainsKey(venue.Key))
                            {
                                dictVenueCollection.Add(venue.Key, venue.Value);
                            }
                        }
                        venueValueListCollection.Add(keyvaluepair.Key, vl);
                    }
                    #endregion
                }



                //clearing
                TTHelperManagerExtension.GetInstance().CounterPartyCollection.Clear();
                TTHelperManagerExtension.GetInstance().VenueCollection.Clear();
                TTHelperManagerExtension.GetInstance().TTCollection.Clear();

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

        public Dictionary<string, TradingTicketValues> TicketNames
        {
            get { return tktNames; }
        }

        /// <summary>
        /// Get Counter parties Filter By Account and AUEC
        /// </summary>
        /// <param name="accountIds"></param>
        /// <param name="cpList"></param>
        /// <returns></returns>
        public ValueList GetCounterpartiesFilterByAccount(List<int> accountIds, ValueList cpList)
        {
            ValueList cpListFinal = new ValueList();
            try
            {
                var accountsCPs = TTHelperManagerExtension.GetInstance().CounterPartyAccountCollection.Where(x => accountIds.Contains(x.Key)).SelectMany(x => x.Value).Distinct().ToList();

                foreach (var item in cpList.ValueListItems)
                {
                    if (accountsCPs.Contains((int)item.DataValue))
                    {
                        cpListFinal.ValueListItems.Add((int)item.DataValue, item.DisplayText);

                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return cpListFinal;
        }


        /// <summary>
        /// This Method is for creating allocation operation preference
        /// </summary>
        /// <param name="parms"></param>
        /// <returns>AllocationOperationPreference</returns>
        public string CreateAllocationOperationPreference(List<AccountValue> accountValues, ref AllocationOperationPreference allocationPreference, string prefName, MatchClosingTransactionType transactionType)
        {
            try
            {
                SerializableDictionary<int, AccountValue> targetPercs = new SerializableDictionary<int, AccountValue>();
                foreach (var accountValue in accountValues)
                {
                    if (!targetPercs.ContainsKey(accountValue.AccountId))
                    {
                        // adding strategy with 0 qty as we do not have strategy wise qty here and also mot using it.
                        accountValue.StrategyValueList.Add(new StrategyValue(0, 100, 0));
                        targetPercs.Add(accountValue.AccountId, accountValue);
                    }
                }

                if(allocationPreference == null)
                {
                    if(AllocationManager == null)
                    {
                        return "Allocation template can not be created";
                    }
                    PreferenceUpdateResult preferenceUpdateResult = AllocationManager.AddPreference(prefName, CachedDataManager.GetInstance.GetCompanyID(), AllocationPreferencesType.CalculatedAllocationPreference, false);
                    if (!string.IsNullOrEmpty(preferenceUpdateResult.Error))
                    {
                        return preferenceUpdateResult.Error;
                    }

                    if (preferenceUpdateResult.Preference == null)
                    {
                        return "Allocation template can not be created";
                    }
                    allocationPreference = preferenceUpdateResult.Preference;
                }

                allocationPreference.TryUpdateTargetPercentage(targetPercs);
                AllocationRule defaulfRule = new AllocationRule();
                defaulfRule.BaseType = AllocationBaseType.CumQuantity;
                defaulfRule.RuleType = MatchingRuleType.None;
                defaulfRule.MatchClosingTransaction = transactionType;
                defaulfRule.PreferenceAccountId = -1;
                allocationPreference.TryUpdateDefaultRule(defaulfRule);
                var result = AllocationManager.UpdatePreference(allocationPreference);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    return result.Error;
                }
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
            return string.Empty;
        }

    }
    public class TradingTicketValues
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _assetID;

        public int AssetID
        {
            get { return _assetID; }
            set { _assetID = value; }
        }
        private int _underLyingID;

        public int UnderLyingID
        {
            get { return _underLyingID; }
            set { _underLyingID = value; }
        }

        public TradingTicketValues(string name, int assetID, int underLyingID)
        {
            _name = name;
            _assetID = assetID;
            _underLyingID = underLyingID;
        }

    }
}
