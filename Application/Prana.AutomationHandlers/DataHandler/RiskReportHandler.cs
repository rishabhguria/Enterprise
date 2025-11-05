using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Prana.BusinessObjects;

namespace Prana.AutomationHandlers
{
    class RiskReportHandler:IExportDataHandler
    {
        
        #region IExportDataHandler Members

        void IExportDataHandler.ExportData(IList data, ClientSettings cs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
