using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ExportGridsData
{
    [ServiceContract]
    public interface IExportGridData
    {
        [OperationContract]
        void ExportData(string gridName, string WindowName, string tabName, string filePath);
    }
}
