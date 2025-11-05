using Prana.ActivityHandler.BusinessObjects;
using Prana.ActivityHandler.Helpers;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ActivityHandler.Generators
{
    internal abstract class TradingActivityGenerator : IActivityGenerator
    {
        /// <summary>
        /// The cash transaction source
        /// </summary>
        CashTransactionType _cashTransactionSource = CashTransactionType.Trading;

        /// <summary>
        /// Gets or sets the cash transaction source.
        /// </summary>
        /// <value>
        /// The cash transaction source.
        /// </value>
        internal CashTransactionType CashTransactionSource
        {
            get { return _cashTransactionSource; }
            set { _cashTransactionSource = value; }
        }

        /// <summary>
        /// Creates the cash activity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="transactionSource">The transaction source.</param>
        /// <returns></returns>
        public List<CashActivity> CreateCashActivity<T>(T data, CashTransactionType transactionSource)
        {
            List<CashActivity> lsCashActivity = new List<CashActivity>();

            try
            {
                CashTransactionSource = transactionSource;
                switch (CashTransactionSource)
                {
                    case CashTransactionType.Trading:
                    case CashTransactionType.TradeImport:
                    case CashTransactionType.DailyCalculation:
                        TaxLot t = data as TaxLot;
                        if (t != null && t.GroupID != t.TaxLotID && t.TaxLotState != ApplicationConstants.TaxLotState.NotChanged)
                        {
                            int sideMultiplier = t.SideMultiplier;
                            decimal notionalAmount;
                            CashActivity cashActivity = new CashActivity();
                            cashActivity.ActivityId = uIDGenerator.GenerateID();
                            cashActivity.TransactionSource = (t.TransactionSource == TransactionSource.TradeImport) ? CashTransactionType.TradeImport : CashTransactionSource;
                            cashActivity.ActivityState = t.TaxLotState;
                            //PRANA-9777
                            cashActivity.EntryDate = t.AUECLocalDate;

                            //For fixed income netnotionalvalue is already divided by 100 in taxlot
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-2707
                            //When fixed income is traded then AssetCategoryValue is none and 
                            //when getting journal exceptions AssetCategoryValue is FixedIncome
                            t.AssetCategoryValue = (AssetCategory)(t.AssetID);
                            //Since we are calculating commissions when journal entries fetched from journal exceptions
                            //commission should be excluded from amount
                            //commission will be handled further in commission region
                            if (string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_Buy) || string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_Buy_Closed) || string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_Buy_Open) || string.Equals(t.OrderSideTagValue, FIXConstants.SIDE_Buy_Cover))
                            {
                                //Notional amount would be negative if the trade is buy and vice versa
                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-5321
                                notionalAmount = (Convert.ToDecimal(t.NetNotionalValue - t.OpenTotalCommissionandFees)) * (-1);
                            }
                            else
                            {
                                notionalAmount = (Convert.ToDecimal(t.NetNotionalValue + t.OpenTotalCommissionandFees));
                            }
                            cashActivity.SideMultiplier = sideMultiplier;
                            cashActivity.Amount = notionalAmount;
                            cashActivity.CurrencyID = t.CurrencyID;
                            cashActivity.Date = t.AUECLocalDate;
                            cashActivity.Description = t.Description;
                            cashActivity.AccountID = Convert.ToInt32(t.Level1ID);
                            cashActivity.FKID = t.TaxLotID;
                            cashActivity.LeadCurrencyID = t.LeadCurrencyID;
                            cashActivity.Symbol = t.Symbol;
                            cashActivity.VsCurrencyID = t.VsCurrencyID;
                            cashActivity.UniqueKey = cashActivity.GetKey();
                            cashActivity.FXRate = t.FXRate;
                            cashActivity.FXConversionMethodOperator = t.FXConversionMethodOperator;
                            //Showing correct quantity for the activity
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-5650
                            cashActivity.ClosedQty = Convert.ToDecimal(t.TaxLotQty);
                            cashActivity.SettlementDate = Convert.ToDateTime(t.SettlementDate);
                            cashActivity.SettlCurrencyID = t.SettlementCurrencyID;
                            if (CashTransactionSource != CashTransactionType.DailyCalculation)
                                ActivityGeneratorHelper.UpdateCommissionAndFees(cashActivity, t);

                            lsCashActivity = UpdateActivityAssetWise(t, cashActivity);
                        }
                        break;

                    case CashTransactionType.Closing:
                        Position position = data as Position;
                        if ((position.ClosingMode != ClosingMode.None))
                        {
                            lsCashActivity = UpdateActivityAssetWise<Position>(position, null);
                        }
                        break;
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

        /// <summary>
        /// Updates the activity asset wise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="cashActivity">The cash activity.</param>
        /// <returns></returns>
        internal abstract List<CashActivity> UpdateActivityAssetWise<T>(T data, CashActivity cashActivity);


    }
}
