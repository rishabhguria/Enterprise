using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Nirvana.TestAutomation.UIAutomation
{

    [ServiceContract]
    public interface IExportGridData
    {
        [OperationContract]
        void ExportData(string gridName, string WindowName, string tabName, string filePath);

        void GetSelectedRowData(string gridName, string WindowName, string tabName, string filePath);
    }
}
