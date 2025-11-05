using Prana.PM.BLL;

namespace Prana.PM.DAL
{
    public class DeskTradeManager
    {
        /// <summary>
        /// Gets the eligible sides.
        /// </summary>
        /// <returns></returns>
        public static OrderSides GetEligibleSides()
        {
            return OrderSides.GetList();
        }
    }
}
