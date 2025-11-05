using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Collections;
using Prana.BusinessObjects.LiveFeed;
using Prana.ClientCommon;
namespace Prana.BasketTrading
{
    public class PreTradeManager
    {
        private Dictionary<string,ErrorDetails>  _basketErrorCollection = null;
        private Dictionary<string, BasketFilterCollection> _basketFilterCollection = null;
        private string  _errorOrderSeqNumbers="" ;
        private string errorTypes = "";
        private static PreTradeManager _preTradeManager=null ;
        
        private PreTradeManager()
        {
            _basketErrorCollection = new Dictionary<string, ErrorDetails>();
            _basketFilterCollection = new Dictionary<string, BasketFilterCollection>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PreTradeManager GetInstance()
        {
            if (_preTradeManager == null)
            {
                _preTradeManager = new PreTradeManager();
            }
           
            return _preTradeManager;

        }
        public void ApplyFilters(BasketDetail basket, BasketFilterCollection filters)
        {
            //_errorOrderSeqNumbers = string.Empty;
            //if (!_basketFilterCollection.ContainsKey(basket.BasketID))
            //{
            //    _basketFilterCollection.Add(basket.BasketID, filters);
            //}
            //ErrorDetails errorDetails = new ErrorDetails();
            List<string> errorsymbolList = new List<string>();
            Dictionary<string, Level1Data> priceData = LiveFeedDataSubscriberClient.GetInstance().GetLiveFeedDataSnapShot(basket.BasketOrders.SymbolList, errorsymbolList);
            // Value Filter
            BasketFilter valuefilter = FilterDataManager.GetFilter(filters, 2);
            if (valuefilter != null)
            {
                ApplyValueFilter(basket, valuefilter);
            }
            // Bid Offer spread Filter
            BasketFilter spreadfilter = FilterDataManager.GetFilter(filters, 3);
            if (spreadfilter != null && errorsymbolList.Count==0)
            {
                ApplySpreadFilter(basket, spreadfilter, priceData);
            }

            BasketFilter volumeFilter = FilterDataManager.GetFilter(filters, 1);
            if (volumeFilter != null && errorsymbolList.Count==0)
            {
                ApplyVolumeFilter(basket, volumeFilter, priceData);
            }
                //if (_errorOrderSeqNumbers != string.Empty)
                //{
                //    _errorOrderSeqNumbers = _errorOrderSeqNumbers.Substring(1, _errorOrderSeqNumbers.Length - 1);
                //    errorTypes = errorTypes.Substring(1, errorTypes.Length - 1);
                //}
                //    errorDetails.ErrorOrderSeqNumbers = _errorOrderSeqNumbers;
                //    errorDetails.ErrorTypes = errorTypes;
                //    if (_basketErrorCollection.ContainsKey(basket.BasketID))
                //    {
                //        _basketErrorCollection.Remove(basket.BasketID);
                //    }
                //    _basketErrorCollection.Add(basket.BasketID, errorDetails);
               
            
        }
        public bool  ApplyFilters(BasketDetail basket)
        {
           // ErrorDetails errorDetails = new ErrorDetails();
            BasketFilterCollection filters = _basketFilterCollection[basket.BasketID];
            _basketErrorCollection[basket.BasketID] = new ErrorDetails();
            //_errorOrderSeqNumbers = "";
            ApplyFilters(basket, filters);
            
            ErrorDetails errorDetails = _basketErrorCollection[basket.BasketID];
            //if (filter != null)
            //{
            //    _errorOrderSeqNumbers = "";
            //    errorTypes = "";

            //    ApplyValueFilter(basket, filter);

            //    if (_errorOrderSeqNumbers != string.Empty)
            //    {
            //        _errorOrderSeqNumbers = _errorOrderSeqNumbers.Substring(1, _errorOrderSeqNumbers.Length - 1);
            //        errorTypes = errorTypes.Substring(1, errorTypes.Length - 1);
            //    }
            //    errorDetails.ErrorOrderSeqNumbers = _errorOrderSeqNumbers;
            //    errorDetails.ErrorTypes = errorTypes;
            //    if (_basketErrorCollection.ContainsKey(basket.BasketID))
            //    {
            //        _basketErrorCollection.Remove(basket.BasketID);
            //    }
            //    _basketErrorCollection.Add(basket.BasketID, errorDetails);
                

            //}

            if (errorDetails.ErrorOrderSeqNumbers == string.Empty)
                return true;
            else
                return false;
        }

        public void ApplyValueFilter(BasketDetail basket , BasketFilter filter)
        {
            double cValue = (GetValueBenchMark(basket)*(filter.Percentage))/100;
            foreach (Order o in basket.BasketOrders)
            {
                if (!ApplyOperator(cValue, o.Quantity * o.Price, filter.OperatorID))
                {
                    
                    _errorOrderSeqNumbers = _errorOrderSeqNumbers + "," + o.BasketSequenceNumber.ToString();
                    errorTypes = errorTypes + ","+filter.FilterTypeID;
                   
                }
            }
           

        }
        private void ApplySpreadFilter(BasketDetail basket, BasketFilter filter, Dictionary<string, Level1Data> priceData)
        { 
           
           
           
                
                foreach (Order o in basket.BasketOrders)
                {
                    string upperSymbol=o.Symbol.Trim().ToUpper();
                    if (priceData[upperSymbol].Bid > 0.0)
                    {
                        double cValue = ((priceData[upperSymbol].Ask - priceData[upperSymbol].Bid )*100)/ priceData[upperSymbol].Bid;
                        if (!ApplyOperator(filter.Percentage, cValue, filter.OperatorID))
                        {

                            _errorOrderSeqNumbers = _errorOrderSeqNumbers + "," + o.BasketSequenceNumber.ToString();
                            errorTypes = errorTypes + "," + filter.FilterTypeID;

                        }
                    }
                } 
            
        }
        private void ApplyVolumeFilter(BasketDetail basket, BasketFilter filter, Dictionary<string, Level1Data> priceData)
        {
            foreach (Order o in basket.BasketOrders)
            {
                string upperSymbol = o.Symbol.Trim().ToUpper();
                if (priceData[upperSymbol].Volume > 0.0)
                {
                    double cValue = (o.Quantity * 100.0) / priceData[upperSymbol].Volume;
                    if (!ApplyOperator(cValue, filter.Percentage, filter.OperatorID))
                    {

                        _errorOrderSeqNumbers = _errorOrderSeqNumbers + "," + o.BasketSequenceNumber.ToString();
                        errorTypes = errorTypes + "," + filter.FilterTypeID;

                    }
                }
            }

        }
        private double GetValueBenchMark(BasketDetail basket )
        {
            return basket.BasketValue; 
        }
        private bool ApplyOperator(double cValue,double origValue,int operatorType)
        {
            bool result = false  ;
            switch (operatorType)
            {
                    case 1: 
                        if (origValue > cValue)
                        {
                            result = true;
                        }
                        break;
                    case 2:
                        if (origValue >= cValue)
                        {
                            result = true;
                        }
                        break;
                    case 3:
                        if (origValue < cValue)
                        {
                            result = true ;
                        }
                        break;
                    case 4:
                        if (origValue >= cValue)
                        {
                            result = true;
                        }
                        break;
           }
            return result;

        }
       
        public void ClearAllFilterErrors()
        {
            _errorOrderSeqNumbers = "";
            errorTypes = "";
        }
        public ErrorDetails GetErrorDetals(string basketID)
        {
                if (_basketErrorCollection.ContainsKey(basketID))
                    return _basketErrorCollection[basketID];
                else
                    return null;
        }
        public bool ISPreTradeAnalysisDone(string basketID)
        { 
            if(_basketErrorCollection.ContainsKey(basketID))
                return true;
            else
                return false;
        }
        public bool IsPreTradeAnalysisPassed(string basketID)
        {
            ErrorDetails errorDetails=_basketErrorCollection[basketID];
            if (errorDetails.ErrorOrderSeqNumbers == string.Empty)
                return true;
            else
                return false;
        }
    }
    
}
