using Prana.ActivityHandler.BusinessObjects;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ActivityHandler.Generators
{
    internal class CashTransactionActivityGenerator : IActivityGenerator
    {
        /// <summary>
        /// Creates the activities from cash transactions.
        /// </summary>
        /// <param name="data">The data set.</param>
        /// <param name="transactionSource">The transaction source.</param>
        /// <returns>The cash activity list</returns>
        public List<CashActivity> CreateCashActivity<T>(T data, CashTransactionType transactionSource)
        {
            List<CashActivity> lsCashActivity = new List<CashActivity>();
            try
            {
                //Narendra Kumar Jangir
                //Date: Oct 18 2013
                //Desc: Handling of activity generation 
                //DataTable dtCashTRNActivityMapping = CommonDataCache.CachedData.GetInstance().CashTRNActivityMapping;

                DataSet ds = data as DataSet;
                if (ds != null)
                {
                    int activityTypeId = int.MinValue;

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int i = 1;
                        if (dr.RowState == DataRowState.Deleted)
                        {
                            activityTypeId = Convert.ToInt32(dr[CashManagementConstants.COLUMN_ACTIVITYTYPEID, DataRowVersion.Original]);
                        }
                        else
                            activityTypeId = Convert.ToInt32(dr[CashManagementConstants.COLUMN_ACTIVITYTYPEID]);

                        CashActivity cashActivity = new CashActivity();
                        cashActivity.TransactionSource = transactionSource;
                        //cash dividend is not an trade activity, it is an cash activity
                        cashActivity.ActivitySource = CachedDataManager.GetActivitySource(activityTypeId);
                        cashActivity.ActivityTypeId = activityTypeId;
                        cashActivity.ActivityType = CachedDataManager.GetActivityText(cashActivity.ActivityTypeId);
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-6490
                        cashActivity.BalanceType = (BalanceType)CachedDataManager.GetBalanceTypeID(cashActivity.ActivityType);
                        cashActivity.ActivityNumber = i;

                        if (dr.RowState != DataRowState.Deleted && !string.IsNullOrEmpty(dr[CashManagementConstants.COLUMN_USERID].ToString()))
                            cashActivity.UserId = Convert.ToInt32(dr[CashManagementConstants.COLUMN_USERID]);

                        //Unique Key is created using CashTransactionId + Date + ActivitySource + ActivityNumber
                        //DataRowVersion.Original is not neccessary for the rowstatetype column becuase it is creating a problem when we add new row in the table 
                        if (dr.Table.Columns.Contains("RowStateType") && dr.Table.Columns["RowStateType"] != null && ((dr.RowState == DataRowState.Deleted || dr.RowState == DataRowState.Modified) ? (Convert.ToString(dr["RowStateType", DataRowVersion.Original].ToString()) == "Added") : (Convert.ToString(dr["RowStateType"].ToString()) == "Added")))
                            cashActivity.ActivityState = ApplicationConstants.TaxLotState.New;
                        else if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Unchanged)
                            cashActivity.ActivityState = ApplicationConstants.TaxLotState.New;
                        else if (dr.RowState == DataRowState.Modified)
                            cashActivity.ActivityState = ApplicationConstants.TaxLotState.Updated;
                        else if (dr.RowState == DataRowState.Deleted)
                        {
                            cashActivity.ActivityState = ApplicationConstants.TaxLotState.Deleted;
                            cashActivity.FKID = dr[CashManagementConstants.COLUMN_CASHTRANSACTIONID, DataRowVersion.Original].ToString();
                            cashActivity.Amount = Convert.ToDecimal(dr[CashManagementConstants.COLUMN_AMOUNT, DataRowVersion.Original]);
                            cashActivity.Date = Convert.ToDateTime(dr[CashManagementConstants.COLUMN_EXDATE, DataRowVersion.Original].ToString());

                            DateTime settleDate;
                            DateTime.TryParse(dr[CashManagementConstants.COLUMN_PAYOUTDATE, DataRowVersion.Original].ToString(), out settleDate);
                            if (!settleDate.Equals(DateTime.MinValue))
                                cashActivity.SettlementDate = Convert.ToDateTime(dr[CashManagementConstants.COLUMN_PAYOUTDATE, DataRowVersion.Original].ToString());
                            else
                                cashActivity.SettlementDate = null;

                            cashActivity.AccountID = Convert.ToInt32(dr[CashManagementConstants.COLUMN_ACCOUNTID, DataRowVersion.Original]);
                            cashActivity.Symbol = dr[CashManagementConstants.COLUMN_SYMBOL, DataRowVersion.Original].ToString();
                            if (ds.Tables[0].Columns.Contains(CashManagementConstants.COLUMN_CURRENCYID))
                                cashActivity.CurrencyID = Convert.ToInt32(dr[CashManagementConstants.COLUMN_CURRENCYID, DataRowVersion.Original]);
                            else
                                cashActivity.CurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                            cashActivity.UniqueKey = cashActivity.GetKey();
                            cashActivity.TaxLotId = dr[CashManagementConstants.COLUMN_TAXLOTID, DataRowVersion.Original].ToString();
                        }
                        if (dr.RowState != DataRowState.Deleted)
                        {
                            cashActivity.Amount = Convert.ToDecimal(dr[CashManagementConstants.COLUMN_AMOUNT]);
                            if (cashActivity.Amount > 0)
                                cashActivity.SideMultiplier = 1;
                            else
                                cashActivity.SideMultiplier = -1;
                            cashActivity.Date = Convert.ToDateTime(dr[CashManagementConstants.COLUMN_EXDATE].ToString());

                            DateTime settleDate;
                            DateTime.TryParse(dr[CashManagementConstants.COLUMN_PAYOUTDATE].ToString(), out settleDate);
                            if (!settleDate.Equals(DateTime.MinValue))
                                cashActivity.SettlementDate = Convert.ToDateTime(dr[CashManagementConstants.COLUMN_PAYOUTDATE].ToString());
                            else
                                cashActivity.SettlementDate = null;

                            cashActivity.AccountID = Convert.ToInt32(dr[CashManagementConstants.COLUMN_ACCOUNTID]);
                            cashActivity.Symbol = dr[CashManagementConstants.COLUMN_SYMBOL].ToString();
                            cashActivity.FKID = dr[CashManagementConstants.COLUMN_CASHTRANSACTIONID].ToString();
                            if (ds.Tables[0].Columns.Contains(CashManagementConstants.COLUMN_CURRENCYID))
                                cashActivity.CurrencyID = Convert.ToInt32(dr[CashManagementConstants.COLUMN_CURRENCYID]);
                            else
                                cashActivity.CurrencyID = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();

                            //PRANA-9777
                            if (ds.Tables[0].Columns.Contains(CashManagementConstants.COLUMN_ENTRYDATE) && !string.IsNullOrEmpty(dr[CashManagementConstants.COLUMN_ENTRYDATE].ToString()))
                            {
                                cashActivity.EntryDate = Convert.ToDateTime(dr[CashManagementConstants.COLUMN_ENTRYDATE]);
                            }


                            if (ds.Tables[0].Columns.Contains(CashManagementConstants.COLUMN_OTHERCURRENCYID) && !string.IsNullOrEmpty(dr[CashManagementConstants.COLUMN_OTHERCURRENCYID].ToString()))
                            {
                                cashActivity.LeadCurrencyID = Convert.ToInt32(dr[CashManagementConstants.COLUMN_CURRENCYID]);
                                cashActivity.VsCurrencyID = Convert.ToInt32(dr[CashManagementConstants.COLUMN_OTHERCURRENCYID]);
                                if (cashActivity.ActivityType.Equals(CachedDataManager.GetActivityTypeWithAcronym(Activities.CashTransfer.ToString())))
                                {
                                    cashActivity.CurrencyID = int.MinValue;
                                }
                            }
                            if (ds.Tables[0].Columns.Contains(CashManagementConstants.COLUMN_FXRATE) && !string.IsNullOrEmpty(dr[CashManagementConstants.COLUMN_FXRATE].ToString()))
                            {
                                cashActivity.FXRate = Convert.ToDouble(dr[CashManagementConstants.COLUMN_FXRATE]);
                            }
                            if (ds.Tables[0].Columns.Contains(OrderFields.PROPERTY_SETTLEMENTCURRENCYID) && !string.IsNullOrEmpty(dr[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].ToString()))
                            {
                                if (!dr[OrderFields.PROPERTY_SETTLEMENTCURRENCY].Equals(DBNull.Value))
                                {
                                    int value;
                                    Int32.TryParse(dr[OrderFields.PROPERTY_SETTLEMENTCURRENCYID].ToString(), out value);
                                    cashActivity.SettlCurrencyID = value;
                                }
                            }

                            if (ds.Tables[0].Columns.Contains(CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR) && !string.IsNullOrEmpty(dr[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR].ToString()))
                            {
                                cashActivity.FXConversionMethodOperator = Convert.ToString(dr[CashManagementConstants.COLUMN_FXCONVERSIONMETHODOPERATOR]);
                            }
                            if (cashActivity.ActivityState == ApplicationConstants.TaxLotState.New || cashActivity.ActivityState == ApplicationConstants.TaxLotState.Updated)
                            {
                                cashActivity.ActivityId = uIDGenerator.GenerateID();
                            }
                            cashActivity.TransactionSource = transactionSource;
                            if (dr.Table.Columns.Contains(CashManagementConstants.COLUMN_DESCRIPTION))
                                cashActivity.Description = dr[CashManagementConstants.COLUMN_DESCRIPTION].ToString();
                            cashActivity.UniqueKey = cashActivity.GetKey();
                            cashActivity.TaxLotId = dr[CashManagementConstants.COLUMN_TAXLOTID].ToString();
                        }

                        cashActivity.TransactionSource = transactionSource;

                        // Check introduced  for overriding if any activity have taxlotid null then it is sure to have its TransactionSource='CashTransaction'. otherwise Corporate Action
                        if (String.IsNullOrEmpty(cashActivity.TaxLotId))
                        {
                            cashActivity.TransactionSource = CashTransactionType.CashTransaction;
                            cashActivity.UniqueKey = cashActivity.GetKey();
                        }

                        if (cashActivity.ActivityTypeId != int.MinValue)
                            lsCashActivity.Add(cashActivity);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return lsCashActivity;
        }
    }
}
