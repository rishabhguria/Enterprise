using Infragistics.Windows.DataPresenter;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using System;
using System.ComponentModel;
using System.Windows;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class TradeListModel : BindableBase
    {

        public TradeListModel(RebalancerModel rebalancerModel)
        {
            AUECID = rebalancerModel.AUECID;
            FXRate = rebalancerModel.FXRate;
            ContractMultiplier = rebalancerModel.Multiplier;
            IsChecked = true;
            AccountId = rebalancerModel.AccountId;
            Symbol = rebalancerModel.Symbol;
            Side = rebalancerModel.Side == PositionType.Long
                        ? rebalancerModel.BuySellQty > 0 ? PTTOrderSide.Buy : PTTOrderSide.Sell
                        : rebalancerModel.BuySellQty > 0 ? PTTOrderSide.SellShort : PTTOrderSide.BuyToClose;
            SideMultiplier = (Side == PTTOrderSide.Buy || Side == PTTOrderSide.BuyToClose) ? 1 : -1;
            Multiplier = rebalancerModel.Multiplier;

            Price = rebalancerModel.Price;

            //Current fields
            CurrentMarketValueBase = Math.Round(rebalancerModel.CurrentMarketValueBase, 2);
            CurrentPercentage = Math.Round(rebalancerModel.CurrentPercentage, 2);
            CurrentPosition = Math.Round(rebalancerModel.Quantity, 0);
            PriceInBaseCurrency = Math.Round((decimal)rebalancerModel.PriceInBaseCurrency, 4);


            //Target fields
            TargetMarketValueBase = Math.Round(rebalancerModel.TargetMarketValueBase, 2);
            TargetPosition = Math.Round(rebalancerModel.TargetPosition, 2);
            TargetPercentage = Math.Round(rebalancerModel.TargetPercentage, 4);

            //Trade value 
            Quantity = Math.Abs(Math.Round(rebalancerModel.BuySellQty, 0));
            TradeValueBase = Math.Round(rebalancerModel.BuySellValue, 2);
            TradeValueLocal = Math.Round((decimal)(rebalancerModel.TargetMarketValueLocal - rebalancerModel.CurrentMarketValueLocal), 2);
            YesterdayMarkPrice = rebalancerModel.YesterdayMarkPrice;
            AccountOrGroupNameToRebalance = rebalancerModel.AccountOrGroupNameToRebalance;

            if (rebalancerModel.YesterdayMarkPrice != 0)
            {
                PercentageChangeFromClosingPrice = Math.Round(((rebalancerModel.Price - rebalancerModel.YesterdayMarkPrice) / rebalancerModel.YesterdayMarkPrice * 100), 2);
            }
            else
            {
                PercentageChangeFromClosingPrice = 100;
            }
            TradeGuid = Guid.NewGuid();
        }

        public TradeListModel()
        {

        }

        [Browsable(false)]
        public int AssetID { get; set; }

        /// <summary>
        /// IsChecked
        /// </summary>
        [Browsable(false)]
        public int AUECID { get; set; }
        /// <summary>
        /// IsChecked
        /// </summary>
        private bool isChecked = true;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// AccountId
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Side
        /// </summary>
        public PTTOrderSide Side { get; set; }

        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        private decimal quantity;
        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged("BuySellValue");
            }
        }

        /// <summary>
        /// Price
        /// </summary>
        private decimal price;
        public decimal Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("BuySellValue");
            }
        }



        private decimal targetMarketValueBase;
        public decimal TargetMarketValueBase
        {
            get { return targetMarketValueBase; }
            set
            {
                targetMarketValueBase = value;
            }
        }

        private decimal targetPosition;
        public decimal TargetPosition
        {
            get { return targetPosition; }
            set
            {
                targetPosition = value;
            }
        }

        private decimal targetPercentage;
        public decimal TargetPercentage
        {
            get { return targetPercentage; }
            set
            {
                targetPercentage = value;
            }
        }
        private decimal currentMarketValueBase;
        public decimal CurrentMarketValueBase
        {
            get { return currentMarketValueBase; }
            set
            {
                currentMarketValueBase = value;
            }
        }
        private decimal currentPercentage;
        public decimal CurrentPercentage
        {
            get { return currentPercentage; }
            set
            {
                currentPercentage = value;
            }
        }

        private decimal currentPosition;
        public decimal CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;
            }
        }

        private decimal priceInBaseCurrency;
        public decimal PriceInBaseCurrency
        {
            get { return priceInBaseCurrency; }
            set
            {
                priceInBaseCurrency = value;
            }
        }


        private decimal tradeValueBase;
        public decimal TradeValueBase
        {
            get { return tradeValueBase; }
            set
            {
                tradeValueBase = value;
            }
        }

        private decimal tradeValueLocal;
        public decimal TradeValueLocal
        {
            get { return tradeValueLocal; }
            set
            {
                tradeValueLocal = value;
            }
        }


        private decimal yesterdayMarkPrice;

        public decimal YesterdayMarkPrice
        {
            get { return yesterdayMarkPrice; }
            set
            {
                yesterdayMarkPrice = value;
            }
        }


        private string accountOrGroupNameToRebalance;
        public string AccountOrGroupNameToRebalance

        {
            get { return accountOrGroupNameToRebalance; }
            set
            {
                accountOrGroupNameToRebalance = value;
            }
        }

        private decimal percentageChangeFromClosingPrice;

        public decimal PercentageChangeFromClosingPrice
        {
            get { return percentageChangeFromClosingPrice; }
            set
            {
                percentageChangeFromClosingPrice = value;
            }
        }

        /// <summary>
        /// Price
        /// </summary>
        [Browsable(false)]
        public decimal BuySellValue
        {

            get
            {
                return Quantity * Price * SideMultiplier * FXRate * Multiplier;
            }
        }

        /// <summary>
        /// Comments
        /// </summary>
        private string comments = string.Empty;
        public string Comments
        {
            get { return comments; }
            set
            {
                comments = value;
                OnPropertyChanged();
            }
        }


        public Decimal FXRate { get; set; }

        [Browsable(false)]
        public decimal ContractMultiplier { get; set; }

        [Browsable(false)]
        public decimal SideMultiplier { get; set; }

        [Browsable(false)]
        public decimal Multiplier { get; set; }

        private bool _isStaged = false;
        [Browsable(false)]
        public bool IsStaged
        {
            get { return _isStaged; }
            set
            {
                _isStaged = value;
                OnPropertyChanged();
            }
        }

        private Guid _tradeGuid = Guid.Empty;
        [Browsable(false)]
        public Guid TradeGuid
        {
            get { return _tradeGuid; }
            set
            {
                _tradeGuid = value;
            }
        }

    }
}
