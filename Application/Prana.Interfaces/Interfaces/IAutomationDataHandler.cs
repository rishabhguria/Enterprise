using Prana.BusinessObjects;
using System.Collections;

namespace Prana.Interfaces
{
    public interface IAutomationDataHandler
    {

        IAllocationServices AllocationServices
        {
            set;
        }
        IPranaPositionServices PositionServices
        {
            set;
        }

        ISecMasterServices SecMasterServices
        {
            set;

        }

        IRiskServices RiskServices
        {
            set;


        }
        void ProcessData(ClientSettings clientSetting, IList data);
        IList RetrieveData(ClientSettings clientSetting);
    }
}
