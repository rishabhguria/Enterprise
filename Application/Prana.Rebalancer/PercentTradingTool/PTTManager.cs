using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Rebalancer.PercentTradingTool.BusinessLogic;
using Prana.Rebalancer.PercentTradingTool.DAL;
using Prana.Rebalancer.PercentTradingTool.Preferences;
using Prana.ServiceConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using Task = System.Threading.Tasks.Task;

namespace Prana.Rebalancer.PercentTradingTool
{
    /// <summary>
    /// The manager class for connecting various components
    /// </summary>
    internal class PTTManager
    {
        #region members
        private static EXPNLConnector _expnlConnector = null;

        private static List<BindingList<PTTMFAccountPref>> _mfAccountPrefBindList;

        /// <summary>
        /// The permitted order sides for asset
        /// </summary>
        private static Dictionary<string, string> _permittedOrderSidesForAsset;

        /// <summary>
        /// The dictionary orders and response
        /// </summary>
        private static Dictionary<string, List<PTTResponseObject>> _dictOrdersAndResponse;

        #endregion

        #region Propeties
        /// <summary>
        /// Gets or sets the masterfund account preference bindinglist.
        /// </summary>
        /// <value>
        /// The Gets or sets the masterfund account preference bindinglist.
        /// </value>
        public static List<BindingList<PTTMFAccountPref>> MfAccountPrefBindList
        {
            get { return _mfAccountPrefBindList; }
            set { _mfAccountPrefBindList = value; }
        }
        /// <summary>
        /// Gets or sets the allocation preference identifier.
        /// </summary>
        /// <value>
        /// The allocation preference identifier.
        /// </value>
        public static int AllocationPrefID;

        /// <summary>
        /// Gets or sets the dictionary orders and response.
        /// </summary>
        /// <value>
        /// The dictionary orders and response.
        /// </value>
        public static Dictionary<string, List<PTTResponseObject>> DictOrdersAndResponse
        {
            get { return _dictOrdersAndResponse; }
            set { _dictOrdersAndResponse = value; }
        }
        /// <summary>
        /// Gets or sets the permitted order sides for asset.
        /// </summary>
        /// <value>
        /// The permitted order sides for asset.
        /// </value>
        public static Dictionary<string, string> PermittedOrderSidesForAsset
        {
            get { return _permittedOrderSidesForAsset; }
            set { _permittedOrderSidesForAsset = value; }
        }
        #endregion

        static PTTManager()
        {
            try
            {
                _expnlConnector = new EXPNLConnector();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets Data and calculates from calculator.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public static List<PTTResponseObject> GetDataAndCalculate(PTTRequestObject requestObject, ref StringBuilder errorMessage)
        {
            try
            {
                List<PTTResponseObject> responseList = _expnlConnector.GetData(requestObject, ref errorMessage);
                bool removeAccountsWithZeroNav = PTTPrefDataManager.GetInstance.GetPTTPreferences(CachedDataManager.GetInstance.LoggedInUser.CompanyUserID).RemoveAccountsWithZeroNAV;
                PTTManager.DictOrdersAndResponse = new Dictionary<string, List<PTTResponseObject>>();
                if (responseList != null)
                {
                    if (removeAccountsWithZeroNav)
                    {
                        if (responseList.Count > 0)
                        {
                            responseList.RemoveAll(x => x.AccountNAV == 0 && x.StartingPosition == 0);
                            errorMessage.Clear();
                        }
                    }
                    if (responseList.Count > 0)
                    {
                        //If 1 then Zero, Current Positions treated as Positive Positions
                        //If -1 then Zero, Current Positions treated as Negative Positions
                        int useZeroPositionsAsPositiveOrNegative = 0;
                        if (CheckAllCurrentSidesAreSame(responseList, ref useZeroPositionsAsPositiveOrNegative))
                        {
                            if ((PTTMasterFundOrAccount)requestObject.MasterFundOrAccount.Value == PTTMasterFundOrAccount.MasterFund)
                            {
                                foreach (PTTResponseObject response in responseList)
                                {
                                    if (!CheckAllCurrentSidesAreSame(response.ChildResponseObjectList, ref useZeroPositionsAsPositiveOrNegative))
                                    {
                                        errorMessage.Clear();
                                        errorMessage.Append(PTTConstants.MSG_CONFLICTING_POSITIONS_FOR_CHILD_ACCOUNTS);
                                        return responseList;
                                    }
                                }
                            }
                            if (useZeroPositionsAsPositiveOrNegative == 0 && ((PTTChangeType)requestObject.AddOrSet.Value == PTTChangeType.Increase || (PTTChangeType)requestObject.AddOrSet.Value == PTTChangeType.Set))
                            {
                                //Incase when all the selected accounts contains zero position
                                useZeroPositionsAsPositiveOrNegative = 1;
                            }
                            return PTTCalculator.Calculate(requestObject, responseList, useZeroPositionsAsPositiveOrNegative, ref errorMessage);
                        }
                        errorMessage.Clear();
                        errorMessage.Append(PTTConstants.MSG_CONFLICTING_POSITIONS);
                    }
                    else if (removeAccountsWithZeroNav)
                    {
                        errorMessage.Clear();
                        errorMessage.Append(PTTConstants.MSG_NO_CALCULATED_DATA);
                    }
                }
                return responseList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Creates the allocation preferecne.
        /// </summary>
        /// <param name="pttResponseObjectsList">The PTT response objects list.</param>
        /// <returns></returns>
        public static CheckListWisePreference CreateCheckListWisePreference(List<PTTResponseObject> pttResponseObjectsList, int orderNo)
        {
            CheckListWisePreference checkListWisePref = null;
            try
            {
                decimal totalQty = pttResponseObjectsList.Sum(x => x.TradeQuantity);
                if (totalQty > 0)
                {
                    SerializableDictionary<int, AccountValue> targetPercs = new SerializableDictionary<int, AccountValue>();
                    foreach (PTTResponseObject item in pttResponseObjectsList)
                    {
                        if (!targetPercs.ContainsKey(item.AccountId))
                        {
                            AccountValue fv = new AccountValue(item.AccountId, item.PercentageAllocation);
                            // adding strategy with 0 qty as we do not have strategy wise qty here and also mot using it.
                            fv.StrategyValueList.Add(new StrategyValue(0, 100, 0));
                            targetPercs.Add(item.AccountId, fv);
                        }
                    }
                    checkListWisePref = new CheckListWisePreference();
                    checkListWisePref.ChecklistId = orderNo;
                    checkListWisePref.OrderSideOperator = CustomOperator.Include;
                    checkListWisePref.OrderSideList.Add(pttResponseObjectsList.FirstOrDefault(x => x.OrderSide != null).OrderSide);
                    checkListWisePref.TargetPercentage = targetPercs;
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
            return checkListWisePref;
        }

        public static AllocationOperationPreference CreateAllocationPreference(PTTRequestObject pttRequestObject, List<PTTResponseObject> pttResponseObjectsList)
        {
            AllocationOperationPreference allocationOperationPreference = null;
            try
            {
                List<AccountValue> accountValues = new List<AccountValue>();
                foreach (PTTResponseObject item in pttResponseObjectsList)
                {
                    accountValues.Add(new AccountValue(item.AccountId, item.PercentageAllocation));
                }
                string prefName = "*PTT#_" + pttRequestObject.TickerSymbol + "_" + "_" + DateTime.Now.ToString("yyMMddHHmmssff");

                AllocationOperationPreference tempAllocationOperationPreference = ServiceManager.Instance.AllocationManager.InnerChannel.GetPreferenceById(AllocationPrefID);
                if (tempAllocationOperationPreference != null)
                {
                    ServiceManager.Instance.AllocationManager.InnerChannel.DeletePreference(AllocationPrefID, AllocationPreferencesType.CalculatedAllocationPreference);
                }

                var transactionType = Convert.ToInt32(pttRequestObject.AddOrSet.Value) == ((int)PTTChangeType.Set) && pttRequestObject.Target == 0 ? MatchClosingTransactionType.SelectedAccounts : MatchClosingTransactionType.None;
                TTHelperManager.GetInstance().AllocationManager = ServiceManager.Instance.AllocationManager.InnerChannel;
                var error = TTHelperManager.GetInstance().CreateAllocationOperationPreference(accountValues, ref allocationOperationPreference, prefName, transactionType);
                TTHelperManager.GetInstance().AllocationManager = null;
                if(string.IsNullOrEmpty(error))
                    AllocationPrefID = allocationOperationPreference.OperationPreferenceId;
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
            return allocationOperationPreference;
        }

        /// <summary>
        /// PTTs the preference details.
        /// </summary>
        /// <param name="pttRequestObject">The p st request object.</param>
        /// <param name="pttResponseObjects">The PTT response objects.</param>
        public static void SavePTTPreferenceDetails(PTTRequestObject pttRequestObject, List<PTTResponseObject> pttResponseObjects)
        {
            try
            {
                PTTDataManager.SavePTTPreferenceDetails(pttRequestObject, pttResponseObjects, AllocationPrefID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Gets the order side dictionary.
        /// </summary>
        /// <param name="auecID">The auec identifier.</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetOrderSideDictionary(int auecID)
        {
            Dictionary<string, string> dictlstSides = new Dictionary<string, string>();
            try
            {
                dictlstSides = PTTDataManager.GetOrderSideDataTable(auecID).AsEnumerable().ToDictionary<DataRow, string, string>(row => row[0].ToString(), row => row[1].ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dictlstSides;
        }

        /// <summary>
        /// PTTs the get allocation details.
        /// </summary>
        /// <param name="pttRequestObject">The PTT request object.</param>
        /// <param name="pttResponseObjectList">The PTT response object list.</param>
        /// <param name="allocationPrefID">The unique allocationPrefID.</param>
        /// <param name="errorMessage">errorMessage.</param>
        public static void PTTGetAllocationDetails(PTTAllocDetailsRequest pttRequestObject, List<PTTResponseObject> pttResponseObjectList, int allocationPrefID, string symbol, string OrderSideId, StringBuilder errorMessage)
        {
            try
            {
                Task.Factory.StartNew(() => PTTDataManager.GetPTTPreferenceDetails(pttRequestObject, pttResponseObjectList, allocationPrefID, symbol, OrderSideId, errorMessage)).Wait();
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
        /// Gets the selected feed for request object.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <param name="errorMessage"></param>
        public static void GetSelectedFeedForRequestObject(PTTRequestObject requestObject, ref StringBuilder errorMessage)
        {
            try
            {
                if (_expnlConnector != null)
                    _expnlConnector.GetSelectedFeedForRequestObject(requestObject, ref errorMessage);
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
        /// Creates the check list wise preference and order.
        /// </summary>
        /// <param name="pttRequestObject">The PTT request object.</param>
        /// <param name="pttResponseObjectList">The PTT response object list.</param>
        /// <param name="orderNo">The order no.</param>
        /// <param name="allocOperation">The alloc operation.</param>
        /// <returns></returns>
        public static OrderSingle CreateCheckListWisePrefAndOrder(PTTRequestObject pttRequestObject, List<PTTResponseObject> pttResponseObjectList, int orderNo, AllocationOperationPreference allocOperation)
        {
            try
            {
                CheckListWisePreference chklist = CreateCheckListWisePreference(pttResponseObjectList, orderNo);
                allocOperation.TryUpdateCheckList(chklist);
                PreferenceUpdateResult pref = ServiceManager.Instance.AllocationManager.InnerChannel.UpdatePreference(allocOperation);
                allocOperation = pref.Preference;
                var order = CreateOrderSingle(pttRequestObject, pttResponseObjectList, allocOperation.OperationPreferenceId);
                return order;
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
            return null;
        }

        /// <summary>
        /// Creates the order single.
        /// </summary>
        /// <param name="pttResponseObjectList"></param>
        /// <param name="pttRequestObject"></param>
        /// <returns></returns>
        public static OrderSingle CreateOrderSingle(PTTRequestObject pttRequestObject, List<PTTResponseObject> pttResponseObjectList, int allocationPrefID)
        {
            var order = new OrderSingle();
            try
            {
                if (pttRequestObject != null)
                {
                    TradingTicketUIPrefs userTradingTicketUiPrefs = TradingTktPrefs.UserTradingTicketUiPrefs;
                    TradingTicketUIPrefs companyTradingTicketUiPrefs = TradingTktPrefs.CompanyTradingTicketUiPrefs;
                    CounterPartyWiseCommissionBasis CommisionUserTTUiPrefs = TradingTktPrefs.CpwiseCommissionBasis;

                    if (userTradingTicketUiPrefs != null && companyTradingTicketUiPrefs != null)
                    {
                        if (pttRequestObject.IsUseCustodianBroker)
                        {
                            order.VenueID = 1;
                        }
                        else if (userTradingTicketUiPrefs != null && userTradingTicketUiPrefs.Broker.HasValue && CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue.ContainsKey(userTradingTicketUiPrefs.Broker.Value))
                        {
                            order.VenueID = CommisionUserTTUiPrefs.DictCounterPartyWiseExecutionVenue[userTradingTicketUiPrefs.Broker.Value];
                        }
                        else if (companyTradingTicketUiPrefs.Venue.HasValue)
                        {
                            order.VenueID = companyTradingTicketUiPrefs.Venue.Value;
                        }

                        if (userTradingTicketUiPrefs.Broker.HasValue)
                        {
                            order.CounterPartyID = userTradingTicketUiPrefs.Broker.Value;
                        }
                        else if (companyTradingTicketUiPrefs.Broker.HasValue)
                        {
                            order.CounterPartyID = companyTradingTicketUiPrefs.Broker.Value;
                        }

                        if (userTradingTicketUiPrefs.TimeInForce.HasValue)
                        {
                            order.TIF = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(userTradingTicketUiPrefs.TimeInForce.Value.ToString());
                        }
                        else if (companyTradingTicketUiPrefs.TimeInForce.HasValue)
                        {
                            order.TIF = TagDatabaseManager.GetInstance.GetTIFValueBasedOnID(companyTradingTicketUiPrefs.TimeInForce.Value.ToString());
                        }

                        if (userTradingTicketUiPrefs.HandlingInstruction.HasValue)
                        {
                            order.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(userTradingTicketUiPrefs.HandlingInstruction.Value.ToString());
                        }
                        else if (companyTradingTicketUiPrefs.HandlingInstruction.HasValue)
                        {
                            order.HandlingInstruction = TagDatabaseManager.GetInstance.GetHandlingInstructionValueBasedOnId(companyTradingTicketUiPrefs.HandlingInstruction.Value.ToString());
                        }

                        if (userTradingTicketUiPrefs.TradingAccount.HasValue)
                        {
                            order.TradingAccountID = userTradingTicketUiPrefs.TradingAccount.Value;
                        }
                        else if (companyTradingTicketUiPrefs.TradingAccount.HasValue)
                        {
                            order.TradingAccountID = companyTradingTicketUiPrefs.TradingAccount.Value;
                        }

                        if (userTradingTicketUiPrefs.Broker.HasValue)
                        {
                            if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(Convert.ToInt32(userTradingTicketUiPrefs.Broker)))
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(userTradingTicketUiPrefs.Broker)].ToString());
                            }
                            else
                            {
                                if (userTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(userTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }
                                else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(companyTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }

                            }
                        }
                        else if (companyTradingTicketUiPrefs.Broker.HasValue)
                        {
                            if (TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions.ContainsKey(Convert.ToInt32(userTradingTicketUiPrefs.Broker)))
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(TradingTktPrefs.CpwiseCommissionBasis.DictCounterPartyWiseExecutionInstructions[Convert.ToInt32(userTradingTicketUiPrefs.Broker)].ToString());
                            }
                            else
                            {
                                if (userTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(userTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }
                                else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                                {
                                    order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(companyTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                                }
                            }
                        }
                        else
                        {
                            if (userTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(userTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                            }
                            else if (companyTradingTicketUiPrefs.ExecutionInstruction.HasValue)
                            {
                                order.ExecutionInstruction = TagDatabaseManager.GetInstance.GetExecutionInstructionValueBasedOnID(companyTradingTicketUiPrefs.ExecutionInstruction.Value.ToString());
                            }
                        }

                        if (userTradingTicketUiPrefs.Strategy.HasValue)
                        {
                            order.Level2ID = int.Parse(userTradingTicketUiPrefs.Strategy.ToString());
                        }
                        else if (companyTradingTicketUiPrefs.Strategy.HasValue)
                        {
                            order.Level2ID = int.Parse(companyTradingTicketUiPrefs.Strategy.ToString());
                        }

                        if (userTradingTicketUiPrefs.OrderType.HasValue)
                        {
                            order.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValueBasedOnID(userTradingTicketUiPrefs.OrderType.Value.ToString());
                        }
                        else if (companyTradingTicketUiPrefs.OrderType.HasValue)
                        {
                            order.OrderTypeTagValue = TagDatabaseManager.GetInstance.GetOrderTypeValueBasedOnID(companyTradingTicketUiPrefs.OrderType.Value.ToString());
                        }

                        if (userTradingTicketUiPrefs.IsSettlementCurrencyBase.HasValue)
                        {
                            if (userTradingTicketUiPrefs.IsSettlementCurrencyBase.Value)
                            {
                                order.SettlementCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                            }
                            else
                            {
                                order.SettlementCurrencyID = pttRequestObject.SecMasterBaseObj.CurrencyID;
                            }
                        }
                        else if (companyTradingTicketUiPrefs.IsSettlementCurrencyBase.HasValue)
                        {
                            if (companyTradingTicketUiPrefs.IsSettlementCurrencyBase.Value)
                            {
                                order.SettlementCurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                            }
                            else
                            {
                                order.SettlementCurrencyID = pttRequestObject.SecMasterBaseObj.CurrencyID;
                            }
                        }
                    }
                    order.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(order.CounterPartyID);
                    order.Venue = CachedDataManager.GetInstance.GetVenueText(order.VenueID);
                    order.TradingAccountName = CachedDataManager.GetInstance.GetTradingAccountText(order.TradingAccountID);
                    string orderSideId = pttResponseObjectList.FirstOrDefault(x => x.OrderSide != null).OrderSide;
                    order.OrderSideTagValue = orderSideId;
                    order.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(orderSideId);
                    order.Price = Convert.ToDouble(pttRequestObject.SelectedFeedPrice);
                    order.Quantity = Math.Abs(pttResponseObjectList.Sum(pstResponseObject => Convert.ToDouble(pstResponseObject.TradeQuantity)));
                    order.Symbol = pttRequestObject.TickerSymbol;
                    order.FXRate = Convert.ToDouble(pttResponseObjectList.FirstOrDefault(x => x.FxRate != 0).FxRate);
                    order.BloombergSymbol = pttRequestObject.SecMasterBaseObj.BloombergSymbol;
                    order.FactSetSymbol = pttRequestObject.SecMasterBaseObj.FactSetSymbol;
                    order.ActivSymbol = pttRequestObject.SecMasterBaseObj.ActivSymbol;
                    if (CachedDataManager.GetInstance.IsBreakOrderPreference())
                        order.Level1ID = pttResponseObjectList.FirstOrDefault(x => x.AccountId != int.MinValue).AccountId;
                    else
                        order.Level1ID = allocationPrefID;
                    order.Level2ID = int.MinValue;
                    order.AUECID = pttRequestObject.SecMasterBaseObj.AUECID;
                    int assetID = Convert.ToInt32(CachedDataManager.GetInstance.GetAssetIdByAUECId(order.AUECID));
                    int underlyingID = Convert.ToInt32(CachedDataManager.GetInstance.GetUnderlyingID(order.AUECID));
                    int exchangeID = CachedDataManager.GetInstance.GetExchangeIdFromAUECId(order.AUECID);
                    order.AssetID = assetID;
                    order.UnderlyingID = underlyingID;
                    order.ExchangeID = exchangeID;
                    order.CurrencyID = pttRequestObject.SecMasterBaseObj.CurrencyID;
                    order.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(order.CurrencyID);
                    order.ContractMultiplier = pttRequestObject.SecMasterBaseObj.Multiplier;
                    //switch (order.CurrencyName)
                    //{
                    //    case "EUR":
                    //    case "GBP":
                    //    case "NZD":
                    //    case "AUD":
                    //        order.FXConversionMethodOperator = "M";
                    //        break;

                    //    default:
                    //        order.FXConversionMethodOperator = "D";
                    //        break;
                    //}
                    //if (order.CurrencyID == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                    //    order.FXConversionMethodOperator = "M";

                    order.FXConversionMethodOperator = Operator.M.ToString();

                    order.Strategy = "-";
                    order.TransactionSource = TransactionSource.PST;
                    order.TransactionSourceTag = (int)TransactionSource.PST;
                    order.OriginalAllocationPreferenceID = order.Level1ID;
                    order.IsUseCustodianBroker = pttRequestObject.IsUseCustodianBroker;
                    return order;
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
            return new OrderSingle();
        }

        /// <summary>
        /// Check if all Response Object positions in all accounts same either long or short
        /// </summary>
        /// <param name="responseList">The PTT response objects.</param>
        /// <param name="useZeroPositionsAsPositiveOrNegative">
        /// If 1 then Zero, Current Positions treated as Positive Positions
        /// If -1 then Zero, Current Positions treated as Negative Positions
        /// </param>
        /// <returns></returns>
        private static bool CheckAllCurrentSidesAreSame(List<PTTResponseObject> responseList, ref int useZeroPositionsAsPositiveOrNegative)
        {
            try
            {
                bool isPositive = false, isNegative = false;
                if (responseList.All((pttResponseObject) => pttResponseObject.StartingPosition == 0))
                    return true;

                foreach (var pttResponseObject in responseList)
                {
                    if (pttResponseObject.StartingPosition > 0)
                    {
                        isPositive = true;
                    }
                    else if (pttResponseObject.StartingPosition < 0)
                    {
                        isNegative = true;
                    }
                    if (isPositive && isNegative)
                    {
                        return false;
                    }
                }

                if (isPositive)
                {
                    useZeroPositionsAsPositiveOrNegative = 1;
                }
                else if (isNegative)
                {
                    useZeroPositionsAsPositiveOrNegative = -1;
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
            return true;
        }

        public static bool DoesSymbolExistInPortfolio(PTTRequestObject reqObj, ref StringBuilder errorMssg)
        {
            try
            {
                var userAccountsIds = CachedDataManager.GetInstance.GetUserAccounts().Cast<Account>().Select(x => x.AccountID).ToList();
                Dictionary<int, decimal> accountWiseStartingPositions = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(reqObj.TickerSymbol, userAccountsIds, ref errorMssg);
                if (accountWiseStartingPositions != null)
                {
                    foreach (var acc in accountWiseStartingPositions)
                    {
                        if (acc.Value != 0)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }
    }
}
