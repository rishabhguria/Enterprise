using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Collections;
using Prana.Interfaces;
using Prana.AutomationHandlers;

namespace Prana.ReportingServices
{
    class AutomationDataProcessing
    {
         DataHandlerFactory _handler;


        public AutomationDataProcessing(DataHandlerFactory handler)
        {
            _handler = handler;
        }
        public  void Process(ClientSettings clientSetting,IList data)
        {
            IAutomationDataHandler datahandler = _handler.getExportDataHandler(clientSetting);
            datahandler.    ProcessData(clientSetting, data);            
        }
    }
}
