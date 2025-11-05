using System;
using System.Collections.Generic;
using System.Text;
using Prana.Interfaces;
using Prana.BusinessObjects;
using System.Collections;
using Prana.CommonDataCache;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.AutomationHandlers
{
    public class FXRateHandler : IAutomationDataHandler
    {
        const string DATEFORMAT = "MM/dd/yyyy";
        const string VALID = "Validated";

        private IPranaPositionServices _positionServices;

        public IPranaPositionServices PositionServices
        {
            set
            {
                _positionServices = value;
            }
        }
        IRiskServices _riskServices = null;
        public IRiskServices RiskServices
        {
            set
            {
                _riskServices = value;

            }
        }
        private ISecMasterServices _secMasterServices;
        public ISecMasterServices SecMasterServices
        {
            set { _secMasterServices = value; }

        }
        private IAllocationServices _allocationServices;

        public IAllocationServices AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
        }

        public void ProcessData(ClientSettings clientSetting, IList data)
        {
            List<ForexPriceImport> fxRateList = ValidateData(clientSetting, data);
            AutomationHandlerDataManager.SaveRunUploadFileDataForForexPrice(fxRateList, clientSetting.DataBaseSettings.ClientDB);
        }

        public List<ForexPriceImport> ValidateData(ClientSettings clientSetting, IList data)
        {
            List<ForexPriceImport> fxRateList = new List<ForexPriceImport>();
            foreach (Object fxRateData in data)
            {
                fxRateList.Add((ForexPriceImport)fxRateData);
            }
            fxRateList = UpdateFXRateCollection(fxRateList, clientSetting);
            return fxRateList;
        }

        private List<ForexPriceImport> UpdateFXRateCollection(List<ForexPriceImport> fxRateList, ClientSettings clientSetting)
        {
            List<ForexPriceImport> forexPriceValidatedDataList = new List<ForexPriceImport>();
            try
            {
                List<ForexPriceImport> forexPriceValueCollection = new List<ForexPriceImport>();

                List<string> currencyStandardPairs = AutomationHandlerDataManager.GetCurrencyStandardPairs();

                List<string> forexUniqueIdsList = new List<string>();                

                foreach (ForexPriceImport forexPriceValue in fxRateList)
                {
                    if (!forexPriceValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(forexPriceValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(forexPriceValue.Date));
                            forexPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(forexPriceValue.Date);
                            forexPriceValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }
                    if (!string.IsNullOrEmpty(forexPriceValue.BaseCurrency))
                    {
                        forexPriceValue.BaseCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(forexPriceValue.BaseCurrency.Trim());
                    }
                    if (!string.IsNullOrEmpty(forexPriceValue.SettlementCurrency))
                    {
                        forexPriceValue.SettlementCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(forexPriceValue.SettlementCurrency.Trim());
                    }

                    if (forexPriceValue.FXConversionMethodOperator.Equals(Prana.BusinessObjects.AppConstants.Operator.D) && forexPriceValue.ForexPrice != 0)
                    {
                        forexPriceValue.ForexPrice = 1 / forexPriceValue.ForexPrice;
                    }

                    if (forexPriceValue.ForexPrice > 0)
                    {
                        string forexUniqueID = forexPriceValue.Date + forexPriceValue.BaseCurrencyID + forexPriceValue.SettlementCurrencyID;
                        if (!forexUniqueIdsList.Contains(forexUniqueID))
                        {
                            forexUniqueIdsList.Add(forexUniqueID);

                            if (forexPriceValue.BaseCurrencyID > 0 && forexPriceValue.SettlementCurrencyID > 0 && forexPriceValue.BaseCurrencyID != forexPriceValue.SettlementCurrencyID)
                            {
                                string uniqueID = forexPriceValue.BaseCurrencyID + Seperators.SEPERATOR_7 + forexPriceValue.SettlementCurrencyID;
                                if (currencyStandardPairs.Contains(uniqueID))
                                {
                                    forexPriceValueCollection.Add(forexPriceValue);
                                }
                                else
                                {
                                    ForexPriceCurrencyStandardPairCheck(currencyStandardPairs, forexPriceValue,forexPriceValueCollection);
                                }
                            }
                            else
                            {
                                forexPriceValueCollection.Add(forexPriceValue);
                                forexPriceValue.Validated = "Standard Currency Pairs does not exists in the application";
                            }
                        }
                    }

                    if (forexPriceValue.Validated.Equals(VALID))
                    {
                        forexPriceValidatedDataList.Add(forexPriceValue);
                    }
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return forexPriceValidatedDataList;
        }

        private void ForexPriceCurrencyStandardPairCheck(List<string> currencyStandardPairs, ForexPriceImport forexPriceValue, List<ForexPriceImport> _forexPriceValueCollection)
        {
            //  to restore the old value,if currency pair does not exits in the application
            double storedForexValue = double.MinValue;

            ///Condition Commented by Ashish. 16th Feb 09
            //if (forexPriceValue.FXConversionMethodOperator.Equals(Prana.BusinessObjects.AppConstants.Operator.M) && forexPriceValue.ForexPrice != 0)
            //{
            //    storedForexValue = forexPriceValue.ForexPrice;
            //    forexPriceValue.ForexPrice = 1 / forexPriceValue.ForexPrice;
            //}
            int baseCurrencyID = forexPriceValue.BaseCurrencyID;
            forexPriceValue.BaseCurrencyID = forexPriceValue.SettlementCurrencyID;
            forexPriceValue.SettlementCurrencyID = baseCurrencyID;

            string baseCurrency = forexPriceValue.BaseCurrency;
            forexPriceValue.BaseCurrency = forexPriceValue.SettlementCurrency;
            forexPriceValue.SettlementCurrency = baseCurrency;

            //Added by Ashish. 
            storedForexValue = forexPriceValue.ForexPrice;
            forexPriceValue.ForexPrice = 1 / storedForexValue;

            // new ID 
            string uniqueID = forexPriceValue.BaseCurrencyID + Seperators.SEPERATOR_7 + forexPriceValue.SettlementCurrencyID;
            if (currencyStandardPairs.Contains(uniqueID))
            {
                _forexPriceValueCollection.Add(forexPriceValue);
            }
            else // if currency standard pair does not exits in the cache, then restore old values and display the same
            {
                int currencyID = forexPriceValue.BaseCurrencyID;
                forexPriceValue.BaseCurrencyID = forexPriceValue.SettlementCurrencyID;
                forexPriceValue.SettlementCurrencyID = currencyID;

                string currency = forexPriceValue.BaseCurrency;
                forexPriceValue.BaseCurrency = forexPriceValue.SettlementCurrency;
                forexPriceValue.SettlementCurrency = currency;

                if (storedForexValue != double.MinValue)
                {
                    forexPriceValue.ForexPrice = storedForexValue;
                }
                _forexPriceValueCollection.Add(forexPriceValue);
                forexPriceValue.Validated = "Standard Currency Pairs does not exists in the application";
            }
        }
        

        public IList RetrieveData(ClientSettings clientSetting)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
