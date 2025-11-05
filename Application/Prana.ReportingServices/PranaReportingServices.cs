using System;
using System.Collections.Generic;
using System.Text;
using Prana.Interfaces;
using Prana.BusinessObjects;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Utilities.MiscUtilities;
using System.Collections;
using Prana.WCFConnectionMgr;
using System.Configuration;
using Prana.AutomationHandlers;
using System.Threading;


namespace Prana.ReportingServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class PranaReportingServices : IReportingServices
    {
        #region Services
        
        public PranaReportingServices()
        {
            
        }
        IPranaPositionServices _positionServices = null;

        public IPranaPositionServices PositionServices
        {
            set
            {
                _positionServices = value;
            }
        }
        IAllocationServices _allocationServices = null;

        public IAllocationServices AllocationServices
        {
            set
            {
                _allocationServices = value;
            }
        }

        IRiskServices _riskService = null;
        public IRiskServices RiskServices
        {
            set
            {
                _riskService = value;
            }
        }
        //private ISecMasterServices _secMasterServices;
        //public ISecMasterServices SecMasterServices
        //{
        //    set { _secMasterServices = value; }

        //}

        #endregion        
        
        public void ProcessRequest(List<ClientSettings> clientSettings)
        {
            try
            {
                DataHandlerFactory handler = new DataHandlerFactory();
                handler.AllocationServices = _allocationServices;
                handler.PositionServices = _positionServices;
                
                handler.RiskServices = _riskService;
                AutomationProcessor automationProcessor = new AutomationProcessor(handler);
                automationProcessor.ProcessData(clientSettings);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);               
            }
        }

        public void CreateStructure(DateTime DateToCreateStructure)
        {
            try
            {

                BaseSettings _baseSettings = ReportingServicesDataManager.BaseSettings;
                ClientSettings _clientSetting = new ClientSettings();
                _clientSetting.BaseSettings = ReportingServicesDataManager.BaseSettings;
                FileMapperComponent.CreateDateStructure(_baseSettings, DateToCreateStructure);

                //AutomationProcessor.ProcessData(clientSettings, _positionServices, _riskService, _allocationServices, _secMasterServices);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);               

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void initializeDictionaries()
        {
            try
            {
                ColumnManager.InitializeDictionaries();
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void Refresh()
        {
            ReportingServicesDataManager.Refresh();
            AutomationHandlerDataManager.Refresh();
            Prana.AutomationHandlers.ClientFunds.Refresh();
            initializeDictionaries();
        }

        #region IReportingServices Members

        public void GetRiskReport(ClientSettings objClientSettings, List<TaxLot> ImportedTaxlots)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion       

       	    
        public void ImportFundsFromDifferentClientsDB()
        {
            try
            {
                //Providing All Clients Details including DBSettings to AutomationHandlers 
                Prana.AutomationHandlers.ClientFunds.DicClientDetails = ReportingServicesDataManager.DicClientDetails;
                Prana.AutomationHandlers.ClientFunds.ImportClientFunds();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
        }

        public void SaveCronExression(string cronExpression)
        {
            try
            {
                ReportingServicesDataManager.SaveCronExpression(cronExpression);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
