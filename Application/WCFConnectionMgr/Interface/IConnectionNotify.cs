using Prana.BusinessObjects;

namespace Prana.WCFConnectionMgr
{
    public interface IConnectionNotify
    {
        void Notify(PranaInternalConstants.ConnectionStatus status);
    }
}
