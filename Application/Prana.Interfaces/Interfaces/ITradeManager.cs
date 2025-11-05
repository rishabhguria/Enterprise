using Prana.BusinessObjects;

namespace Prana.Interfaces
{
    public interface ITradeManager
    {
        //static bool ValidateAUECandCV(Order order); 
        void SendTradeToServer(OrderSingle order);
        bool IsWithinLimits(OrderSingle orderRequest, double marketPrice);
    }
}
