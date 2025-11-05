using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class TradeListSummaryViewModel
    {
        public ObservableCollection<TradeListSummaryModel> TradeListSummaryList { get; set; }
        public ObservableCollection<TradeListOrderSideSummaryModel> TradeListOrderSideSummaryList { get; set; }

        public string CaptionToDisplay { get; set; }
        public bool Yes { get; set; }
        public bool No { get; set; }
        public bool Cancel { get; set; }
        public DelegateCommand YesCommand { get; set; }
        public DelegateCommand NoCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public TradeListSummaryViewModel()
        {
            YesCommand = new DelegateCommand(() => YesCommandAction());
            NoCommand = new DelegateCommand(() => NoCommandAction());
            CancelCommand = new DelegateCommand(() => CancelCommandAction());
        }

        public void SetUp(List<TradeListModel> lstTrades)
        {
            try
            {
                TradeListSummaryList = new ObservableCollection<TradeListSummaryModel>();
                TradeListOrderSideSummaryList = new ObservableCollection<TradeListOrderSideSummaryModel>();
                var BuyRow = new TradeListOrderSideSummaryModel();
                BuyRow.Side = "Buy";
                var SellRow = new TradeListOrderSideSummaryModel();
                SellRow.Side = "Sell";
                var BuyToCloseRow = new TradeListOrderSideSummaryModel();
                BuyToCloseRow.Side = "Buy To Close";
                var SellShortRow = new TradeListOrderSideSummaryModel();
                SellShortRow.Side = "Sell Short";
                TradeListSummaryModel summaryObj = new TradeListSummaryModel();
                summaryObj.TotalSymbols = lstTrades.Where(x => x.IsChecked).GroupBy(x => x.Symbol).Select(x => x.Key).ToList().Count;
                decimal tempBuySellValueTotal = 0M;
                foreach (var trade in lstTrades)
                {
                    switch (trade.Side)
                    {
                        case PTTOrderSide.Buy:
                            BuyRow.Quantity += trade.Quantity;
                            BuyRow.BuySellValue += trade.BuySellValue;
                            break;
                        case PTTOrderSide.Sell:
                            SellRow.Quantity += trade.Quantity * -1;
                            SellRow.BuySellValue += trade.BuySellValue;
                            break;
                        case PTTOrderSide.BuyToClose:
                            BuyToCloseRow.Quantity += trade.Quantity;
                            BuyToCloseRow.BuySellValue += trade.BuySellValue;
                            break;
                        case PTTOrderSide.SellShort:
                            SellShortRow.Quantity += trade.Quantity * -1;
                            SellShortRow.BuySellValue += trade.BuySellValue;
                            break;
                    }
                    summaryObj.TotalQuantity += trade.Quantity;
                    summaryObj.TotalBuySellValue += Math.Abs(trade.BuySellValue);
                    tempBuySellValueTotal += trade.BuySellValue;
                }
                TradeListSummaryList.Add(summaryObj);
                TradeListOrderSideSummaryList.Add(BuyRow);
                TradeListOrderSideSummaryList.Add(SellRow);
                TradeListOrderSideSummaryList.Add(BuyToCloseRow);
                TradeListOrderSideSummaryList.Add(SellShortRow);
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

        public void YesCommandAction()
        {
            try
            {
                Yes = true;
                Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
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

        public void NoCommandAction() 
        {
            try
            {
                No = true;
                Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
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

        public void CancelCommandAction()
        {
            try
            {
                Cancel = true;
                Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)?.Close();
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
    }
}
