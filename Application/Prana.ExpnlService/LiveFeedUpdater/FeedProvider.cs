using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.LiveFeedProvider;
using Prana.CommonDataCache;
using Prana.Interfaces;

namespace Prana.ExposureAndPNLCalculator
{
    public class FeedProvider
    {
        LiveFeedSubscriber _liveFeedSubscriber = null;
        eSignalOptionsManagerNew _eSignalOptionsManagerNewInst = null;
        ForexConverter _forexConverter = null;
        IPricingAnalysis _pricingDataSubscriber = null;
        private int _companyID;

        public int CompanyID
        {            
            set { _companyID = value; }
        }


        internal FeedProvider(IPricingAnalysis pricingDataSubscriber)
        {
            //these are the data input points
            _liveFeedSubscriber = LiveFeedSubscriber.GetInstance(); 
            _eSignalOptionsManagerNewInst = eSignalOptionsManagerNew.GetInstance();
            _forexConverter = ForexConverter.GetInstance(_companyID);
            _pricingDataSubscriber = pricingDataSubscriber;
        }
        
        
        private Dictionary<string, List<string>> _symbolWiseTaxLotIDs = null;
        /// <summary>
        /// SymbolWise TaxLot IDs
        /// </summary>
        public void SetSymbolWiseTaxLotIDs(Dictionary<string, List<string>> symbolWiseTaxLotIDs)
        {
            try
            {
                _symbolWiseTaxLotIDs = symbolWiseTaxLotIDs;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                throw;
            }
        }
        
        
        private Dictionary<string, bool> _symbolWiseCalculationMarking = null;
        /// <summary>
        /// SymbolWise Calculation Marking, true marked if a new order is received or data is refreshed as in that case all the calculations are to be done irrespective of price updates.
        /// </summary>
        public void Set_symbolWiseCalculationMarking(Dictionary<string, bool> symbolWiseCalculationMarking)
        {
            try
            {
                _symbolWiseCalculationMarking = symbolWiseCalculationMarking;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                throw;
            }
        }


        private ExposureAndPnlOrderCollection _taxLotCollection;
        /// <summary>
        /// Set input collection
        /// </summary>
        /// <param name="taxLotCollection"></param>
        public void SetInputTaxLotCollection(ExposureAndPnlOrderCollection taxLotCollection)
        {
            try
            {
                _taxLotCollection = taxLotCollection;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                throw;
            }
        }

        /// <summary>
        /// Calling this method starts filling data for the orders 
        /// </summary>
        public void FillDataAndMarkOrders()
        {
            try
            {
                foreach (EPnlOrder uncalculatedOrder in _taxLotCollection)
                {
                    //if (!_symbolWiseCalculationMarking[uncalculatedOrder.Symbol])
                    //{
                    //    //Fill LiveFeed Data if new data is available

                    //}
                    //else
                    //{
                    //    //Fill LiveFeed Data in any case as the symbol/taxlot is new
                    //    FillPriceInfo(uncalculatedOrder);
                    //}
                    uncalculatedOrder.IsMarkedForCalculation = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                throw;
            }
        }

        /// <summary>
        /// Return data filled orders
        /// </summary>
        /// <returns></returns>
        public ExposureAndPnlOrderCollection GetMarkedOrders()
        {
            try
            {
                return _taxLotCollection;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                throw;
            }
        }


        ///// <summary>
        ///// Called for all the taxlots, check if latest data available and then fill.
        ///// </summary>
        ///// <param name="taxlot"></param>
        //private void FillPriceInfo(EPnlOrder taxlot)
        //{
        //    try
        //    {
        //        switch (taxlot.ClassID)
        //        {
                    
        //            case Prana.BusinessObjects.AppConstants.EPnLClassID.EPnLOrderEquity:
        //            case Prana.BusinessObjects.AppConstants.EPnLClassID.EPnLOrderEquitySwap:

        //                //fill equity
        //                break;
        //            case Prana.BusinessObjects.AppConstants.EPnLClassID.EPnLOrderOption:
        //                // fill option data
        //                break;
        //            case Prana.BusinessObjects.AppConstants.EPnLClassID.EPnLOrderFuture:
        //                // fill EPnLOrderFuture data 
        //                break;
        //            case Prana.BusinessObjects.AppConstants.EPnLClassID.EPnLOrderFX:
                       
        //                break;
        //            case Prana.BusinessObjects.AppConstants.EPnLClassID.EPnLOrderFXForward:
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        throw;
        //    }
        //}





    }
}
