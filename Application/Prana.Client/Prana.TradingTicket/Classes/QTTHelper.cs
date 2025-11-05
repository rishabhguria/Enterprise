using Prana.LogManager;
using System;

namespace Prana.TradingTicket.Classes
{
    internal static class QTTHelper
    {/// <summary>
     /// Gets the label from quantity.
     /// </summary>
     /// <param name="qty">The qty.</param>
     /// <returns></returns>
        internal static string GetLabelFromQuantity(int qty)
        {
            string lblText = string.Empty;
            try
            {
                if (qty >= 100000000)
                {
                    lblText = string.Format("{0:0.##}", (qty / 1000000000m)) + "B";
                }
                else if (qty >= 1000000)
                {
                    lblText = string.Format("{0:0.#}", (qty / 1000000m)) + "M";
                }
                else if (qty >= 100000)
                {
                    lblText = string.Format("{0:0.##}", (qty / 1000000m)) + "M";
                }
                else if (qty >= 1000)
                {
                    lblText = string.Format("{0:0.#}", (qty / 1000m)) + "K";
                }
                else
                {
                    lblText = qty.ToString();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return lblText;
        }

    }
}
