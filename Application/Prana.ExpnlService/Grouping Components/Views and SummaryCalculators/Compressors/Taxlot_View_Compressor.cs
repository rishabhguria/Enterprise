using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.ExpnlService.Grouping_Components.Views_and_SummaryCalculators.Compressors
{
    public class Taxlot_View_Compressor : GenericCompressor, ICompressor
    {
        private Dictionary<int, ExposureAndPnlOrderCollection> _inputOrderCollection;
        private ExposurePnlCacheItemList listofUIObjects;

        public CompressedDataDictionaries GetData(Dictionary<int, ExposureAndPnlOrderCollection> calculatedTaxLots, ExposureAndPnlOrderCollection markedCollection, Dictionary<int, DistinctAccountSetWiseSummaryCollection> CompressedDistinctAccountSetWiseSummaryCollection, Dictionary<int, ExposureAndPnlOrderSummary> CompressedAccountSummaries)
        {
            CompressedDataDictionaries dictToReturn = new CompressedDataDictionaries();
            try
            {
                _inputOrderCollection = calculatedTaxLots;

                ExposurePnlCacheItem itemToReturn;
                foreach (KeyValuePair<int, ExposureAndPnlOrderCollection> temp in _inputOrderCollection)
                {
                    listofUIObjects = new ExposurePnlCacheItemList();
                    if (!dictToReturn.OutputCompressedData.ContainsKey(temp.Key))
                    {
                        dictToReturn.OutputCompressedData.Add(temp.Key, listofUIObjects);
                    }
                    foreach (EPnlOrder calculatedTaxLot in temp.Value)
                    {
                        itemToReturn = new ExposurePnlCacheItem();
                        switch (calculatedTaxLot.ClassID)
                        {
                            case EPnLClassID.EPnlOrder:
                                calculatedTaxLot.GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderEquity:
                                ((EPnLOrderEquity)calculatedTaxLot).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderOption:
                                ((EPnLOrderOption)calculatedTaxLot).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderFuture:
                                ((EPnLOrderFuture)calculatedTaxLot).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderFX:
                                ((EPnLOrderFX)calculatedTaxLot).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderEquitySwap:
                                ((EPnLOrderEquitySwap)calculatedTaxLot).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderFXForward:
                                ((EPnLOrderFXForward)calculatedTaxLot).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderFixedIncome:
                                ((EPnLOrderFixedIncome)calculatedTaxLot).GetBindableObject(itemToReturn);
                                break;

                            default:
                                calculatedTaxLot.GetBindableObject(itemToReturn);
                                break;
                        }
                        dictToReturn.OutputCompressedData[temp.Key].Add(itemToReturn);
                        CalculateAbsoluteValues(itemToReturn);
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
            return dictToReturn;
        }

        #region ICompressor Members
        public ExposurePnlCacheItemList GetContainingTaxlots(string compressedRowID, int accountID, DistinctAccountSetWiseSummaryCollection outputAccountSetWiseConsolidatedSummary)
        {
            EPnlOrder orderToReturn = null;
            ExposurePnlCacheItemList listToReturn = new ExposurePnlCacheItemList();
            try
            {
                if (_inputOrderCollection.ContainsKey(accountID))
                {
                    ExposureAndPnlOrderCollection accountWiseCollection = _inputOrderCollection[accountID];
                    if (accountWiseCollection != null && accountWiseCollection.Contains(compressedRowID))
                    {
                        orderToReturn = accountWiseCollection[compressedRowID];
                        ExposurePnlCacheItem itemToReturn = new ExposurePnlCacheItem();

                        switch (orderToReturn.ClassID)
                        {
                            case EPnLClassID.EPnlOrder:
                                orderToReturn.GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderEquity:
                                ((EPnLOrderEquity)orderToReturn).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderOption:
                                ((EPnLOrderOption)orderToReturn).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderFuture:
                                ((EPnLOrderFuture)orderToReturn).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderFX:
                                ((EPnLOrderFX)orderToReturn).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderEquitySwap:
                                ((EPnLOrderEquitySwap)orderToReturn).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderFXForward:
                                ((EPnLOrderFXForward)orderToReturn).GetBindableObject(itemToReturn);
                                break;

                            case EPnLClassID.EPnLOrderFixedIncome:
                                ((EPnLOrderFixedIncome)orderToReturn).GetBindableObject(itemToReturn);
                                break;

                            default:
                                orderToReturn.GetBindableObject(itemToReturn);
                                break;
                        }
                        FillOrderWithSummaryValues(outputAccountSetWiseConsolidatedSummary, itemToReturn);
                        listToReturn = new ExposurePnlCacheItemList();
                        listToReturn.Add(itemToReturn);
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
            return listToReturn;
        }

        #endregion ICompressor Members

        protected override void CalculateSpecificDetails(ExposurePnlCacheItem uiObject, EPnlOrder taxlot)
        {
        }

        protected override string getKey(EPnlOrder taxlot)
        {
            return String.Empty;
        }
    }
}