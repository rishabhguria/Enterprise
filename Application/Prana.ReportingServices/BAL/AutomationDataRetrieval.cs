using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Collections;
using Prana.Interfaces;
using Prana.AutomationHandlers;

namespace Prana.ReportingServices
{
    class AutomationDataRetrieval
    {
        DataHandlerFactory _handler;
        private IPranaPositionServices _positionServices;

        public AutomationDataRetrieval(DataHandlerFactory handler)
        {
            _handler = handler;
        }
        
        internal  IList RetrieveData(ClientSettings  clientSetting)
        {
            IAutomationDataHandler dataretrievalComponent = _handler.getImportedDataHandler(clientSetting);
            IList list = dataretrievalComponent.RetrieveData(clientSetting);
            return list;
        }
    }
}
