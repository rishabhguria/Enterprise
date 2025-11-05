using Prana.ActivityHandler.BusinessObjects;
using Prana.ActivityHandler.DAL;
using Prana.ActivityHandler.Factories;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Prana.ActivityHandler
{
    public class ActivityHelperClass
    {

        #region Methods
        /// <summary>
        /// Creates the cash activity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="transactionSource">The transaction source.</param>
        /// <returns></returns>
        internal static List<CashActivity> CreateCashActivity<T>(T data, CashTransactionType transactionSource)
        {
            List<CashActivity> lsCashActivity = new List<CashActivity>();
            if (CommonDataCache.CachedDataManager.GetInstance.CompanyModules.ContainsValue(PranaModules.GENERAL_LEDGER_MODULE))
            {
                try
                {
                    switch (transactionSource)
                    {
                        case CashTransactionType.Trading:
                        case CashTransactionType.TradeImport:
                        case CashTransactionType.DailyCalculation:
                            List<TaxLot> lsTaxlots = data as List<TaxLot>;
                            if (lsTaxlots != null)
                            {
                                Parallel.ForEach(lsTaxlots, t =>
                                {
                                    IActivityGenerator tradActGen = ActivityGeneratorFactory.GetActivityGenerator((AssetCategory)t.AssetID);
                                    if (tradActGen != null)
                                    {
                                        List<CashActivity> temp = tradActGen.CreateCashActivity<TaxLot>(t, transactionSource);
                                        lock (lsCashActivity)
                                        {
                                            lsCashActivity.AddRange(temp);
                                        }
                                    }
                                });
                            }
                            break;

                        case CashTransactionType.CashTransaction:
                        case CashTransactionType.CorpAction:
                        case CashTransactionType.ImportedEditableData:
                            IActivityGenerator cashActGen = ActivityGeneratorFactory.GetActivityGenerator(AssetCategory.None);
                            if (cashActGen != null)
                                lsCashActivity.AddRange(cashActGen.CreateCashActivity<T>(data, transactionSource));
                            break;

                        case CashTransactionType.Closing:
                            List<Position> lsPosition = data as List<Position>;
                            if (lsPosition != null)
                            {
                                Parallel.ForEach(lsPosition, position =>
                                {
                                    IActivityGenerator tradActGen = ActivityGeneratorFactory.GetActivityGenerator(position.AssetCategoryValue);
                                    if (tradActGen != null)
                                    {
                                        List<CashActivity> temp = tradActGen.CreateCashActivity<Position>(position, transactionSource);
                                        lock (lsCashActivity)
                                        {
                                            lsCashActivity.AddRange(temp);
                                        }
                                    }
                                });
                                UpdateRevaluationDate<List<Position>>(lsPosition);
                            }
                            break;

                        case CashTransactionType.Unwinding:

                            //TODO:following section can be removed, because pnl entries for all the asset classes are handled through revaluation batch
                            CreateCashActivityUnwinding<T>(data);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                        throw;
                }
            }
            return lsCashActivity;
        }

        /// <summary>
        /// Updates revaluation date for position
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        private static void UpdateRevaluationDate<T>(T data)
        {
            try
            {
                List<Position> lsPosition = data as List<Position>;
                Dictionary<int, RevaluationUpdateDetail> revalDates = CachedDataManager.GetLastRevaluationCalculationDate();
                Dictionary<int, CashPreferences> cashPreferences = ServiceProxyConnector.CashManagementServices.GetCashPreferences();
                Dictionary<int, DateTime> accountsTobeUpdated = new Dictionary<int, DateTime>();
                foreach (Position position in lsPosition)
                {
                    int accountID = 0;
                    if (!string.IsNullOrWhiteSpace(position.AccountID.ToString()) && position.AccountID > 0)
                    {
                        accountID = position.AccountID;
                    }
                    else
                    {
                        accountID = position.AccountValue.ID;
                    }

                    //http://jira.nirvanasolutions.com:8080/browse/PRANA-6561
                    if (revalDates.ContainsKey(accountID) && DateTime.Compare(position.ClosingTradeDate.Date, revalDates[accountID].LastRevaluationDate) <= 0
                        && DateTime.Compare(cashPreferences[accountID].CashMgmtStartDate, position.ClosingTradeDate.Date) <= 0)
                    {
                        revalDates[accountID].LastRevaluationDate = position.ClosingTradeDate.Date;
                        if (!accountsTobeUpdated.ContainsKey(accountID))
                        {
                            accountsTobeUpdated.Add(accountID, position.ClosingTradeDate.Date);
                        }
                        else
                        {
                            accountsTobeUpdated[accountID] = position.ClosingTradeDate.Date;
                        }
                    }
                }
                if (accountsTobeUpdated != null && accountsTobeUpdated.Count > 0)
                {
                    foreach (KeyValuePair<int, DateTime> kvp in accountsTobeUpdated)
                    {
                        ServiceProxyConnector.CashManagementServices.UpdateLastRevaluationCalculatedDateToGivenDate(kvp.Value, kvp.Key.ToString(), false);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Creates the cash activity unwinding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        private static void CreateCashActivityUnwinding<T>(T data)
        {
            try
            {
                List<string> lsClosingDates = data as List<string>;
                Dictionary<int, DateTime> accountsTobeUpdate = new Dictionary<int, DateTime>();
                Dictionary<int, RevaluationUpdateDetail> revalDate = CachedDataManager.GetLastRevaluationCalculationDate();
                Dictionary<int, CashPreferences> cashPreference = ServiceProxyConnector.CashManagementServices.GetCashPreferences();
                //update reval date for unwinded taxlots
                if (lsClosingDates != null && lsClosingDates.Count > 0)
                {
                    DateTime closingDate = DateTime.MinValue;
                    string accountID = string.Empty;
                    foreach (string closingDt in lsClosingDates)
                    {
                        string[] lsDateAccount = closingDt.Split('_');
                        if (lsDateAccount.Length > 1 && !string.IsNullOrEmpty(lsDateAccount[0]) && !string.IsNullOrEmpty(lsDateAccount[1]))
                        {
                            closingDate = Convert.ToDateTime(lsDateAccount[0]).Date;
                            accountID = lsDateAccount[1].ToString();
                        }
                        if (!string.IsNullOrEmpty(closingDate.ToString()) && !string.IsNullOrEmpty(accountID) && revalDate.ContainsKey(Convert.ToInt32(accountID)) && DateTime.Compare(closingDate.Date, revalDate[Convert.ToInt32(accountID)].LastRevaluationDate) <= 0
                            && DateTime.Compare(cashPreference[Convert.ToInt32(accountID)].CashMgmtStartDate, closingDate.Date) <= 0)
                        {
                            revalDate[Convert.ToInt32(accountID)].LastRevaluationDate = closingDate;
                            if (!accountsTobeUpdate.ContainsKey(Convert.ToInt32(accountID)))
                            {
                                accountsTobeUpdate.Add(Convert.ToInt32(accountID), closingDate);
                            }
                            else
                            {
                                accountsTobeUpdate[Convert.ToInt32(accountID)] = closingDate;
                            }
                        }
                    }
                    if (accountsTobeUpdate != null && accountsTobeUpdate.Count > 0)
                    {
                        foreach (KeyValuePair<int, DateTime> kvp in accountsTobeUpdate)
                        {
                            ServiceProxyConnector.CashManagementServices.UpdateLastRevaluationCalculatedDateToGivenDate(kvp.Value, kvp.Key.ToString().TrimEnd(','), false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the state of the activiy.
        /// </summary>
        /// <param name="lsActivity">The ls activity.</param>
        /// <param name="activityState">State of the activity.</param>
        public static void SetActiviyState(List<CashActivity> lsActivity, ApplicationConstants.TaxLotState activityState)
        {
            try
            {
                if (lsActivity != null)
                {
                    foreach (CashActivity data in lsActivity)
                    {
                        data.ActivityState = activityState;
                    }
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
        }

        /// <summary>
        /// Sets the state of the tax lot.
        /// </summary>
        /// <param name="lsTaxLots">The ls tax lots.</param>
        /// <param name="taxLotState">State of the tax lot.</param>
        public static void SetTaxLotState(List<TaxLot> lsTaxLots, ApplicationConstants.TaxLotState taxLotState)
        {
            try
            {
                if (lsTaxLots != null)
                {
                    foreach (TaxLot data in lsTaxLots)
                    {
                        data.TaxLotState = taxLotState;
                    }
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
        }

        /// <summary>
        /// Creates the cash activity datatable.
        /// New Parameter UserId, PRANA-9776
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        internal static DataSet CreateCashActivityDatatable<T>(T data, int userId, bool isSymbolLevelAccruals)
        {
            DataSet ActivitiesDataSet = data as DataSet;
            int index = isSymbolLevelAccruals ? 1 : 0;
            try
            {
                if (ActivitiesDataSet != null && ActivitiesDataSet.Tables.Count > index)
                {
                    if (ActivitiesDataSet.Tables[index].Rows.Count > 0)
                    {
                        ActivitiesDataSet.Tables[index].Columns.Add("ActivityID", typeof(System.String));
                        ActivitiesDataSet.Tables[index].Columns.Add("UniqueKey", typeof(System.String));
                        ActivitiesDataSet.Tables[index].Columns.Add("ActivityType", typeof(System.String));
                        ActivitiesDataSet.Tables[index].Columns.Add("BalanceType", typeof(System.Int32));
                        ActivitiesDataSet.Tables[index].Columns.Add("ModifyDate", typeof(System.DateTime)); //PRANA-9777
                        ActivitiesDataSet.Tables[index].Columns.Add("EntryDate", typeof(System.DateTime)); //PRANA-9777
                        ActivitiesDataSet.Tables[index].Columns.Add("UserId", typeof(System.Int32)); //PRANA-9776

                        foreach (DataRow dr in ActivitiesDataSet.Tables[index].Rows)
                        {
                            if (Convert.ToString(dr["ActivityState"]) == Convert.ToString(ApplicationConstants.TaxLotState.New))
                                dr["ActivityID"] = uIDGenerator.GenerateID();

                            string FKID = string.Empty;
                            if (string.IsNullOrEmpty(Convert.ToString(dr["FKID"])))
                            {
                                FKID = uIDGenerator.GenerateID();
                                dr["FKID"] = FKID;
                            }
                            dr["UniqueKey"] = dr["FKID"].ToString() + Convert.ToDateTime(dr["TradeDate"]).ToShortDateString() + Convert.ToInt32(dr["TransactionSource"]).ToString() + Convert.ToString(dr["ActivityNumber"]);
                            dr["ActivityType"] = CachedDataManager.GetActivityText(Convert.ToInt32(dr["ActivityTypeId_FK"]));
                            dr["BalanceType"] = Convert.ToInt32(BalanceType.Cash);
                            //PRANA-9777
                            dr["ModifyDate"] = DateTime.Now;
                            dr["EntryDate"] = DateTime.Now;
                            dr["UserId"] = userId;
                        }
                    }
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
            return ActivitiesDataSet;
        }

        #endregion
    }
}
