using Prana.BusinessObjects;
using System.Collections.Generic;
namespace Prana.ClientCommon
{
    public class ClientTradeValidator
    {
        public static List<string> ValidateAUECIDS(OrderCollection orders)
        {
            List<string> errorsymbols = new List<string>();
            foreach (Order order in orders)
            {
                if (order.AUECID == int.MinValue)
                {
                    errorsymbols.Add(order.Symbol.ToUpper());
                }
            }
            return errorsymbols;
        }

    }
}
