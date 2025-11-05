using Prana.BusinessObjects.Enumerators.RebalancerNew;

namespace Prana.Rebalancer.RebalancerNew.BussinessLogic.Interfaces
{
    public interface IImportFactory
    {
        IImport CreateObject(RebalancerEnums.ImportType type, ref string error);
    }
}
