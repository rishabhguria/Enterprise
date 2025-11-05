using System;
using System.Collections.Generic;
using System.Text;
using Prana.Interfaces;
using System.Collections;
using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.BusinessObjects.AppConstants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prana.AutomationHandlers
{
    public class CashHandler : IAutomationDataHandler
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
        #region IAutomationDataHandler Members

        public void ProcessData(ClientSettings clientSetting, IList data)
        {
            List<CashCurrencyValue> cashDataImportList = ValidateData(clientSetting, data);
            AutomationHandlerDataManager.SaveRunUploadFileDataForCash(cashDataImportList, clientSetting.DataBaseSettings.ClientDB);
        }

        public List<CashCurrencyValue> ValidateData(ClientSettings clientSetting, IList data)
        {
            List<CashCurrencyValue> cashDataList = new List<CashCurrencyValue>();
            foreach (Object cashData in data)
            {
                cashDataList.Add((CashCurrencyValue)cashData);
            }
            cashDataList = UpdateCashImportDataCollection(cashDataList, clientSetting);
            return cashDataList;
        }

        private List<CashCurrencyValue> UpdateCashImportDataCollection(List<CashCurrencyValue> cashDataList, ClientSettings clientSetting)
        {
            List<CashCurrencyValue> cashCurrencyValueCollection = new List<CashCurrencyValue>();
            try
            {
                foreach (CashCurrencyValue cashCurrencyValue in cashDataList)
                {
                    if (!cashCurrencyValue.Date.Equals(string.Empty))
                    {
                        bool isParsed = false;
                        double outResult;
                        isParsed = double.TryParse(cashCurrencyValue.Date, out outResult);
                        if (isParsed)
                        {
                            DateTime dtn = DateTime.FromOADate(Convert.ToDouble(cashCurrencyValue.Date));
                            cashCurrencyValue.Date = dtn.ToString(DATEFORMAT);
                        }
                        else
                        {
                            DateTime dtn = Convert.ToDateTime(cashCurrencyValue.Date);
                            cashCurrencyValue.Date = dtn.ToString(DATEFORMAT);
                        }
                    }

                    if (!string.IsNullOrEmpty(cashCurrencyValue.FundName))
                    {
                        cashCurrencyValue.FundID = CachedDataManager.GetInstance.GetFundID(cashCurrencyValue.FundName.Trim());
                    }

                    if (!string.IsNullOrEmpty(cashCurrencyValue.BaseCurrency))
                    {
                        cashCurrencyValue.BaseCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(cashCurrencyValue.BaseCurrency.Trim());
                    }
                    if (!string.IsNullOrEmpty(cashCurrencyValue.LocalCurrency))
                    {
                        cashCurrencyValue.LocalCurrencyID = CachedDataManager.GetInstance.GetCurrencyID(cashCurrencyValue.LocalCurrency.Trim());
                    }

                    if (cashCurrencyValue.CashValueBase == 0 && cashCurrencyValue.CashValueLocal != 0)
                    {
                        if (cashCurrencyValue.LocalCurrencyID > 0 && cashCurrencyValue.BaseCurrencyID > 0 && cashCurrencyValue.LocalCurrencyID.Equals(cashCurrencyValue.BaseCurrencyID))
                        {
                            cashCurrencyValue.CashValueBase = cashCurrencyValue.CashValueLocal;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(cashCurrencyValue.Date))
                            {
                                ForexConverter.GetInstance(clientSetting.ClientInformation.CompanyID, Convert.ToDateTime(cashCurrencyValue.Date)).GetForexData(Convert.ToDateTime(cashCurrencyValue.Date));
                                ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(clientSetting.ClientInformation.CompanyID).GetConversionRateFromCurrenciesForGivenDate(cashCurrencyValue.LocalCurrencyID, cashCurrencyValue.BaseCurrencyID, Convert.ToDateTime(cashCurrencyValue.Date));
                                if (conversionRate != null)
                                {
                                    cashCurrencyValue.ForexConversionRate = conversionRate.RateValue;
                                    if (conversionRate.ConversionMethod == Operator.D)
                                    {
                                        if (conversionRate.RateValue != 0)
                                            cashCurrencyValue.CashValueBase = cashCurrencyValue.CashValueLocal / conversionRate.RateValue;
                                    }
                                    else
                                    {
                                        cashCurrencyValue.CashValueBase = cashCurrencyValue.CashValueLocal * conversionRate.RateValue;
                                    }
                                }
                            }
                        }
                    }
                    if (cashCurrencyValue.CashValueLocal == 0 && cashCurrencyValue.CashValueBase != 0)
                    {
                        if (cashCurrencyValue.LocalCurrencyID > 0 && cashCurrencyValue.BaseCurrencyID > 0 && cashCurrencyValue.LocalCurrencyID.Equals(cashCurrencyValue.BaseCurrencyID))
                        {
                            cashCurrencyValue.CashValueLocal = cashCurrencyValue.CashValueBase;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(cashCurrencyValue.Date))
                            {
                                ForexConverter.GetInstance(clientSetting.ClientInformation.CompanyID, Convert.ToDateTime(cashCurrencyValue.Date)).GetForexData(Convert.ToDateTime(cashCurrencyValue.Date));
                                ConversionRate conversionRate = Prana.CommonDataCache.ForexConverter.GetInstance(clientSetting.ClientInformation.CompanyID).GetConversionRateFromCurrenciesForGivenDate(cashCurrencyValue.LocalCurrencyID, cashCurrencyValue.BaseCurrencyID, Convert.ToDateTime(cashCurrencyValue.Date));
                                if (conversionRate != null)
                                {
                                    cashCurrencyValue.ForexConversionRate = conversionRate.RateValue;
                                    if (conversionRate.ConversionMethod == Operator.D)
                                    {
                                        if (conversionRate.RateValue != 0)
                                            cashCurrencyValue.CashValueLocal = cashCurrencyValue.CashValueBase / conversionRate.RateValue;
                                    }
                                    else
                                    {
                                        cashCurrencyValue.CashValueLocal = cashCurrencyValue.CashValueBase * conversionRate.RateValue;
                                    }
                                }
                            }
                        }
                    }

                    cashCurrencyValueCollection.Add(cashCurrencyValue);
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
            return cashCurrencyValueCollection;
        }

        public IList RetrieveData(ClientSettings clientSetting)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
