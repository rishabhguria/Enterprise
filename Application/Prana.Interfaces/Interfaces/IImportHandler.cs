using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using System.Data;

namespace Prana.Interfaces
{
    public interface IImportHandler
    {
        void ProcessRequest(DataSet ds, RunUpload runUpload, object taskResult, bool isSaveDataInApplication);
        void UpdateCollection(SecMasterBaseObj secMasterObj, string collectionKey);
        string GetXSDName();
        DataTable ValidatePriceTolerance(DataSet ds);
    }
}
