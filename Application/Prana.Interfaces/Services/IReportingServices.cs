using Prana.BusinessObjects;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.Interfaces
{
    [ServiceContract]//(CallbackContract = typeof(IRiskReportingCallback))]
    public interface IReportingServices
    {
        [OperationContract]
        void GetRiskReport(ClientSettings objClientSettings, List<TaxLot> ImportedTaxlots);
    }
}
