using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Collections;
using Prana.Interfaces;
using Prana.Utilities.MiscUtilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;

namespace Prana.AutomationHandlers
{
    class FileHandler : IAutomationDataHandler
    {
        private IPranaPositionServices _positionServices;
        private ISecMasterServices _secMasterServices;
        public ISecMasterServices SecMasterServices
        {
            set { _secMasterServices = value; }

        }
        IRiskServices _riskServices = null;
        public IRiskServices RiskServices
        {
            set
            {
                _riskServices = value;

            }

        }
        public IPranaPositionServices PositionServices
        {
            set
            {
                _positionServices = value;
            }
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

            FileFormatterFactory.GetInstance().GetFormatterClass(clientSetting.FileFormatter).CreateFile(data,null, clientSetting.ReportFileName , clientSetting.DicColumns);
        }
        public IList RetrieveData(ClientSettings clientSetting)
        {
            IList list = null;
            try
            {
                
                int count = 0;
                foreach (FileSetting fileSetting in clientSetting.FileSettings)
                {
                    //Here Wil Be A factory Which Will return the type From:--- clientsetting
                    Type _ImportType = DataHandlerFactory.getImportType(clientSetting);
                    IList retrieved = FileMapperComponent.GetTransformedData(_ImportType, fileSetting);
                    if (count == 0)
                    {
                        list = retrieved;
                    }
                    else
                    {
                        foreach (Object data in retrieved)
                        {
                            list.Add(data);
                        }
                    }
                    count++;
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
            return list;
        }
       
    }
}
