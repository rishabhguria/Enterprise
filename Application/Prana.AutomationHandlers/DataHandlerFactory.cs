using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using Prana.Interfaces;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.AutomationHandlers
{
    public  class DataHandlerFactory
    {
        private  IPranaPositionServices _positionServices;

        public  IPranaPositionServices PositionServices
        {
            set
            {
                _positionServices = value;
            }
            get
            {
               return  _positionServices;
            }
        }
        private  ISecMasterServices _secMasterServices;
        public  ISecMasterServices SecMasterServices
        {
            set { _secMasterServices = value; }

        }
        private  IAllocationServices _allocationServices;
         IRiskServices _riskServices = null;
        public  IRiskServices RiskServices
        {
            set
            {
                _riskServices = value;

            }

        }
        public  IAllocationServices AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
            get
            {
                return _allocationServices ;
            }
        }
        public  IAutomationDataHandler getImportedDataHandler(ClientSettings objClientSettings)
        {
            IAutomationDataHandler _handler = null;
            try
            {               
                switch (objClientSettings.InputDataLocationType)
                {
                    case AutomationEnum.InputOutputType.DB:
                        switch (objClientSettings.ImportType)
                        {
                            //case AutomationEnum.ImportTypeEnum.TaxLot:
                            //    _handler = new TaxLotHandler();
                            //    break;
                            case AutomationEnum.ImportTypeEnum.DailyCash:
                               _handler = new CashHandler(); 
                                break;
                            case AutomationEnum.ImportTypeEnum.MarkPrice:

                                break;
                            case AutomationEnum.ImportTypeEnum.Position:
                                _handler = new TaxLotHandler();
                                break;

                        }
                        break;
                    case AutomationEnum.InputOutputType.FileSystem:
                        _handler = new FileHandler();
                        break;
                }
                AssignServices(_handler);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return _handler;
        }
        public  IAutomationDataHandler getExportDataHandler(ClientSettings objClientSettings)
        {
            IAutomationDataHandler _handler = null;
            try
            {
                switch (objClientSettings.ReportType)
                {
                    case AutomationEnum.ReprotTypeEnum.Internal:
                        switch (objClientSettings.ImportType)
                        {
                            case AutomationEnum.ImportTypeEnum.Position:
                                _handler = new TaxLotHandler();
                                break;
                            case AutomationEnum.ImportTypeEnum.DailyCash:
                                _handler = new CashHandler();
                                break;
                            case AutomationEnum.ImportTypeEnum.MarkPrice:
                                _handler = new MarkPriceHandler();
                                break;
                            case AutomationEnum.ImportTypeEnum.FXRate:
                                _handler = new FXRateHandler();
                                break;

                        }
                        break;
                    case AutomationEnum.ReprotTypeEnum.RiskReport:
                    case AutomationEnum.ReprotTypeEnum.StressTest:
                        _handler = new RiskHandler();
                        break;
                }
                AssignServices(_handler);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return _handler;
        }
        private  void AssignServices(IAutomationDataHandler handler )
        {
            if (handler != null)
            {
                handler.AllocationServices = _allocationServices;
                handler.PositionServices = _positionServices;
                handler.SecMasterServices = _secMasterServices;
                handler.RiskServices = _riskServices;
            }
        }
        public static  Type getImportType(ClientSettings objClientSettings)
        {
            Type _typeToReturn = null;
            try
            {
                switch (objClientSettings.ImportType)
                {
                    //case AutomationEnum.ImportTypeEnum.TaxLot:
                    //    _typeToReturn = typeof(TaxLot);
                    //    break;
                    case AutomationEnum.ImportTypeEnum.DailyCash:
                        _typeToReturn = typeof(CashCurrencyValue);
                        break;
                    case AutomationEnum.ImportTypeEnum.MarkPrice:
                        _typeToReturn = typeof(MarkPriceImport);
                        break;
                    case AutomationEnum.ImportTypeEnum.Position:
                        _typeToReturn = typeof(PositionMaster);
                        break;
                    case AutomationEnum.ImportTypeEnum.FXRate:
                        _typeToReturn = typeof(ForexPriceImport);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
            return _typeToReturn;
        }

    }
}
