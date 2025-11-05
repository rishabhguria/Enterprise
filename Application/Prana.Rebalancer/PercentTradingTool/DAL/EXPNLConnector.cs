using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.ServiceConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prana.Rebalancer.PercentTradingTool.DAL
{
    /// <summary>
    /// Connects to Expnl Service to bring data
    /// </summary>
    internal class EXPNLConnector
    {
        /// <summary>
        /// Fetching data from Expnl
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public List<PTTResponseObject> GetData(PTTRequestObject requestObject, ref StringBuilder errorMessage)
        {
            List<PTTResponseObject> responseList = new List<PTTResponseObject>();

            try
            {
                Dictionary<int, List<int>> dictMasterFundAccountAssociationUpdated = new Dictionary<int, List<int>>();
                Dictionary<int, decimal> currentAccountStartingValue = new Dictionary<int, decimal>();
                Dictionary<int, decimal> accountWiseStartingPositions = new Dictionary<int, decimal>();
                Dictionary<int, decimal> currentAccountNAV = new Dictionary<int, decimal>();
                Dictionary<int, decimal> currentAccountFxRateValue = new Dictionary<int, decimal>();
                Dictionary<int, List<int>> dictMasterFundAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                AccountCollection userAccounts = CachedDataManager.GetInstance.GetUserAccounts();
                List<int> accountIDs = requestObject.Account.Cast<Account>().Select(account => account.AccountID).ToList();
                if (!string.IsNullOrEmpty(requestObject.TickerSymbol) && accountIDs.Count > 0)
                {
                    switch ((PTTMasterFundOrAccount)requestObject.MasterFundOrAccount.Value)
                    {
                        case PTTMasterFundOrAccount.Account:
                            currentAccountStartingValue = ExpnlServiceConnector.GetInstance().GetGrossExposureForSymbolAndAccounts(requestObject.TickerSymbol, accountIDs, ref errorMessage);
                            currentAccountNAV = ExpnlServiceConnector.GetInstance().GetAccountNAV(accountIDs, ref errorMessage);
                            accountWiseStartingPositions = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(requestObject.TickerSymbol, accountIDs, ref errorMessage);
                            currentAccountFxRateValue = ExpnlServiceConnector.GetInstance().GetFxRateForSymbolAndAccounts(requestObject.TickerSymbol, accountIDs, requestObject.SecMasterBaseObj.AUECID, requestObject.SecMasterBaseObj.CurrencyID, ref errorMessage);

                            responseList.AddRange(accountIDs.Select(accountId => new PTTResponseObject
                            {
                                AccountId = accountId,
                                StartingPosition = accountWiseStartingPositions[accountId],
                                StartingValue = currentAccountStartingValue[accountId] > 0 ? currentAccountStartingValue[accountId] : 0,
                                AccountNAV = currentAccountNAV[accountId] > 0 ? currentAccountNAV[accountId] : 0,
                                FxRate = currentAccountFxRateValue[accountId] > 0 ? currentAccountFxRateValue[accountId] : (decimal)1
                            }));

                            if (currentAccountNAV.Any(x => x.Value < 0))
                            {
                                errorMessage.Append(PTTConstants.MSG_NEGATIVE_NAV_FOR_ACCOUNTS);
                            }
                            break;

                        case PTTMasterFundOrAccount.MasterFund:
                            foreach (KeyValuePair<int, string> kvp in CachedDataManager.GetInstance.GetAllMasterFunds())
                            {
                                if (!dictMasterFundAccountAssociation.ContainsKey(kvp.Key)) continue;
                                var accountList = new List<int>();
                                foreach (KeyValuePair<int, string> accountKvp in CachedDataManager.GetInstance.GetAccounts().OrderBy(account => account.Value))
                                {
                                    if (dictMasterFundAccountAssociation[kvp.Key].Any(a => a == accountKvp.Key))
                                        accountList.Add(accountKvp.Key);
                                }

                                dictMasterFundAccountAssociationUpdated.Add(kvp.Key, accountList);
                            }
                            foreach (int currMasterFund in accountIDs)
                            {
                                List<int> accountsOfMasterFundList = dictMasterFundAccountAssociationUpdated[currMasterFund];
                                /*foreach (int accid in dictMasterFundAccountAssociationUpdated[currMasterFund])
                                {
                                    if (!userAccounts.Contains(accid))
                                    {
                                        accountsOfMasterFundList.Remove(accid);
                                    }
                                }*/
                                // PRANA-29300
                                int accid;
                                for (int i = 0; i < dictMasterFundAccountAssociationUpdated[currMasterFund].Count; i++)
                                {
                                    accid = dictMasterFundAccountAssociationUpdated[currMasterFund][i];
                                    if (!userAccounts.Contains(accid))
                                    {
                                        accountsOfMasterFundList.Remove(accid);
                                    }
                                }

                                currentAccountStartingValue = ExpnlServiceConnector.GetInstance().GetGrossExposureForSymbolAndAccounts(requestObject.TickerSymbol, accountsOfMasterFundList, ref errorMessage);
                                currentAccountNAV = ExpnlServiceConnector.GetInstance().GetAccountNAV(accountsOfMasterFundList, ref errorMessage);
                                accountWiseStartingPositions = ExpnlServiceConnector.GetInstance().GetPositionForSymbolAndAccounts(requestObject.TickerSymbol, accountsOfMasterFundList, ref errorMessage);
                                currentAccountFxRateValue = ExpnlServiceConnector.GetInstance().GetFxRateForSymbolAndAccounts(requestObject.TickerSymbol, accountsOfMasterFundList, requestObject.SecMasterBaseObj.AUECID, requestObject.SecMasterBaseObj.CurrencyID, ref errorMessage);

                                PTTResponseObject curResponseObject = new PTTResponseObject();
                                curResponseObject.ChildResponseObjectList = new List<PTTResponseObject>();
                                curResponseObject.ChildResponseObjectList.AddRange(accountsOfMasterFundList.Select(accountId => new PTTResponseObject
                                {
                                    AccountId = accountId,
                                    StartingPosition = accountWiseStartingPositions[accountId],
                                    StartingValue = currentAccountStartingValue[accountId],
                                    AccountNAV = currentAccountNAV[accountId],
                                    FxRate = currentAccountFxRateValue[accountId],
                                    ChildResponseObjectList = null
                                }
                                      ));

                                decimal masterFundNav = curResponseObject.ChildResponseObjectList.Sum(x => x.AccountNAV);
                                curResponseObject.AccountId = currMasterFund;
                                curResponseObject.StartingPosition = curResponseObject.ChildResponseObjectList.Sum(x => x.StartingPosition);
                                curResponseObject.StartingValue = curResponseObject.ChildResponseObjectList.Sum(x => x.StartingValue);
                                curResponseObject.AccountNAV = masterFundNav > 0 ? masterFundNav : 0;
                                curResponseObject.FxRate = curResponseObject.ChildResponseObjectList.Count > 0 ? curResponseObject.ChildResponseObjectList.FirstOrDefault().FxRate : 0;
                                responseList.Add(curResponseObject);
                            }

                            break;

                        default:
                            break;
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
                return null;
            }
            return responseList;
        }

        /// <summary>
        /// Gets the selected feed for request object.
        /// </summary>
        /// <param name="requestObject">The request object.</param>
        /// <param name="errorMessage"></param>
        public void GetSelectedFeedForRequestObject(PTTRequestObject requestObject, ref StringBuilder errorMessage)
        {
            try
            {
                requestObject.SelectedFeedPrice = ExpnlServiceConnector.GetInstance().GetPXSelectedFeedForSymbol(requestObject.TickerSymbol, ref errorMessage);
                requestObject.SelectedFeedPriceInBaseCurrency = ExpnlServiceConnector.GetInstance().GetPXSelectedFeedBaseForSymbol(requestObject.TickerSymbol, ref errorMessage);
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
    }
}