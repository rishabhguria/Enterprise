using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.ClientCommon;
using Prana.CommonDataCache;

namespace Prana.BasketTrading
{
    class BasketSummaryView
    {
        private string _basketName = string.Empty;
        private Int64 _numberOfSymbols;
        private double _numberOfShares = 0.0;
        private double _absoluteBasketValue = 0.0;
        private double _absoluteExecutedValue = 0.0;
        private double _numberofSharesCommited=0.0;
        private double _percentageCommited=0.0;
        private double _percentageExecuted=0.0;
        private double _basketPNL = 0.0;
        private double _numberofSharesExecuted = 0.0;
        private BasketSummaryPartsCollection _basketSummaryParts= new BasketSummaryPartsCollection();

        private BasketSummaryParts _basketSummaryPartsLongs = new BasketSummaryParts();
        private BasketSummaryParts _basketSummaryPartsShorts = new BasketSummaryParts();
        public BasketSummaryView()
        {
            _basketSummaryPartsLongs.Side = "Long";
            _basketSummaryPartsShorts.Side = "Short";
            _basketSummaryParts.Add(_basketSummaryPartsShorts);
            _basketSummaryParts.Add(_basketSummaryPartsLongs);
        }
        public string BasketName
        {
            get
            {
                return _basketName;
            }
            set
            {
                _basketName = value;
            }

        }
        public Int64 NumberOfSymbols
        {
            get
            {
                return _numberOfSymbols;
            }
            set
            {
                _numberOfSymbols = value;
            }

        }
        public double AbsoluteBasketValue
        {
            get
            {
                return _absoluteBasketValue;
            }
            set
            {
                _absoluteBasketValue = value;
            }

        }
        public double AbsoluteExecutedValue
        {
            get
            {
                return _absoluteExecutedValue;
            }
            set
            {
                _absoluteExecutedValue = value;
            }

        }
        public double NumberofShares
        {
            get
            {
                return _numberOfShares;
            }
            set
            {
                _numberOfShares = value;
            }

        }
        public double NumberofSharesExecuted
        {
            get
            {
                return _numberofSharesExecuted;
            }
            set
            {
                _numberofSharesExecuted = value;
            }

        }
        public double NumberofSharesCommited
        {
            get
            {
                return _numberofSharesCommited;
            }
            set
            {
                _numberofSharesCommited = value;
            }

        }
        public double PercentageCommited
        {
            get
            {
                return _percentageCommited;
            }
            set
            {
                _percentageCommited = value;
            }

        }
        public double PercentageExecuted
        {
            get
            {
                return _percentageExecuted;
            }
            set
            {
                _percentageExecuted = value;
            }

        }
        public double BasketPNL
        {
            get
            {
                return _basketPNL;
            }
            set
            {
                _basketPNL = value;
            }

        }
        public BasketSummaryPartsCollection BasketSummaryParts
        {
            get
            {
                return _basketSummaryParts;
            }
            set
            {
                _basketSummaryParts = value;
            }

        }

        public void SetDetails(BasketDetail basket)
        {
            try
            {
                _numberOfSymbols = 0;
                _absoluteBasketValue = 0.0;
                _absoluteExecutedValue = 0.0;
                _numberofSharesCommited = 0.0;
                _percentageCommited = 0.0;
                _percentageExecuted = 0.0;
                _basketPNL = 0.0;

                _basketSummaryPartsLongs.NumberOfShares = 0;
                _basketSummaryPartsLongs.NumberofSharesCommited = 0;
                _basketSummaryPartsLongs.NumberofSharesExecuted = 0;
                _basketSummaryPartsLongs.AbsoluteExecutedValue = 0;
                _basketSummaryPartsLongs.AbsoluteBasketValue = 0;


                _basketSummaryPartsShorts.NumberOfShares = 0;
                _basketSummaryPartsShorts.NumberofSharesCommited = 0;
                _basketSummaryPartsShorts.NumberofSharesExecuted = 0;
                _basketSummaryPartsShorts.AbsoluteExecutedValue = 0;
                _basketSummaryPartsShorts.AbsoluteBasketValue = 0;                //}





                _basketName = basket.BasketName;
                _numberOfShares = basket.BasketOrders.Quantity;

                _numberofSharesExecuted = basket.BasketOrders.CumQty;

                List<string> listOfSymbols = new List<string>();
                List<string> listOfSymbolsLongs = new List<string>();
                List<string> listOfSymbolsShort = new List<string>();
                List<string> errorList = new List<string>();
                Dictionary<string, Prana.BusinessObjects.LiveFeed.Level1Data> basketLiveFeedData = LiveFeedDataSubscriberClient.GetInstance().GetLiveFeedDataContiniousData(basket.BasketOrders.SymbolList, errorList);
                double shortPNL = 0.0;
                double longPNL = 0.0;

                foreach (Order order in basket.BasketOrders)
                {
                    if (!listOfSymbols.Contains(order.Symbol))
                    {
                        listOfSymbols.Add(order.Symbol);
                    }
                    _numberofSharesCommited += order.Quantity - order.UnsentQty - order.SendQty;
                    _absoluteExecutedValue += (order.CumQty) * (order.AvgPrice);
                    //if (basketLiveFeedData.ContainsKey(order.Symbol.ToUpper()))
                    // {
                    order.LastPrice = basketLiveFeedData[order.Symbol.ToUpper()].Last;
                    _absoluteBasketValue += (order.Quantity) * (order.LastPrice);
                    // }
                    if (NameValueFiller.IsLongSide(order.OrderSideTagValue))//Buy
                    {
                        longPNL += BasketManager.GetOrderUnrealisedPNL(order);
                        if (!listOfSymbolsLongs.Contains(order.Symbol))
                        {
                            listOfSymbolsLongs.Add(order.Symbol);
                        }
                        _basketSummaryPartsLongs.NumberofSharesCommited += order.Quantity - order.UnsentQty - order.SendQty;
                        _basketSummaryPartsLongs.NumberofSharesExecuted += order.CumQty;

                        _basketSummaryPartsLongs.AbsoluteExecutedValue += (order.CumQty) * (order.AvgPrice);
                        // if (basketLiveFeedData.ContainsKey(order.Symbol.ToUpper()))
                        //{
                        _basketSummaryPartsLongs.AbsoluteBasketValue += (order.Quantity) * (basketLiveFeedData[order.Symbol.ToUpper()].Last);
                        // }
                        _basketSummaryPartsLongs.NumberOfShares += order.Quantity;
                    }
                    else
                    {
                        shortPNL += BasketManager.GetOrderUnrealisedPNL(order);
                        if (!listOfSymbolsShort.Contains(order.Symbol))
                        {
                            listOfSymbolsShort.Add(order.Symbol);
                        }
                        _basketSummaryPartsShorts.AbsoluteExecutedValue += (order.CumQty) * (order.AvgPrice);

                        //if (basketLiveFeedData.ContainsKey(order.Symbol.ToUpper()))
                        //{
                        _basketSummaryPartsShorts.AbsoluteBasketValue += (order.Quantity) * (order.LastPrice);
                        //}
                        _basketSummaryPartsShorts.NumberofSharesCommited += order.Quantity - order.UnsentQty - order.SendQty;
                        _basketSummaryPartsShorts.NumberofSharesExecuted += order.CumQty;
                        _basketSummaryPartsShorts.NumberOfShares += order.Quantity;
                    }
                }
                _basketSummaryPartsLongs.NumberOfSymbols = listOfSymbolsLongs.Count;
                //divide by Zero Check
                if (_basketSummaryPartsLongs.NumberOfShares != 0)
                {
                    _basketSummaryPartsLongs.PercentageExecuted = (_basketSummaryPartsLongs.NumberofSharesExecuted * 100) / _basketSummaryPartsLongs.NumberOfShares;
                    _basketSummaryPartsLongs.PercentageCommited = (_basketSummaryPartsLongs.NumberofSharesCommited * 100) / _basketSummaryPartsLongs.NumberOfShares;
                }
                _basketSummaryPartsLongs.BasketPNL = longPNL;
                //divide by Zero Check
                if (_basketSummaryPartsShorts.NumberOfShares != 0)
                {
                    _basketSummaryPartsShorts.PercentageCommited = (_basketSummaryPartsShorts.NumberofSharesCommited * 100) / _basketSummaryPartsShorts.NumberOfShares;
                    _basketSummaryPartsShorts.PercentageExecuted = (_basketSummaryPartsShorts.NumberofSharesExecuted * 100) / _basketSummaryPartsShorts.NumberOfShares;

                }
                _basketSummaryPartsShorts.NumberOfSymbols = listOfSymbolsShort.Count;
                //divide by Zero Check
                if (_numberOfShares != 0)
                {
                    _percentageExecuted = (_numberofSharesExecuted * 100) / _numberOfShares;
                }
                _basketSummaryPartsShorts.BasketPNL = shortPNL;

                _basketPNL = longPNL + shortPNL;
                //divide by Zero Check
                if (basket.BasketOrders.Quantity != 0)
                {
                    _percentageCommited = (_numberofSharesCommited * 100) / basket.BasketOrders.Quantity;
                }
                _numberOfSymbols = listOfSymbols.Count;
            }
            catch (Exception )
            {

                
            }
        }
    }
}
