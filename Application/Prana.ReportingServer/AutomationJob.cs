using System;
using System.Collections.Generic;
using System.Text;
using Quartz;
using Prana.BusinessObjects;
using Prana.ReportingServices;

namespace Prana.ReportingServer
{
    class AutomationJob:IJob
    {
        #region IJob Members
       
       
        public void Execute(JobExecutionContext context)
        {
             ClientSettingsPref ObjectDeSerialize = ReportingServicesDataManager.ImportXMLSettings;

            // foreach (ClientSettings _clientSettings in ObjectDeSerialize.ClientSettingsList)
           //  {
             ReportingServer.PranaReportingServices.ProcessRequest(ObjectDeSerialize.getClientSettings());
            // }

           
            
        }

        #endregion
    }
}
