using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Prana.BusinessObjects;

namespace Prana.AutomationHandlers
{
    interface IExportDataHandler
    {
         void ExportData(IList data,ClientSettings cs);
    }
}
