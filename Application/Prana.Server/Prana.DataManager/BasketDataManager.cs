using Prana.BusinessObjects;
using Prana.LogManager;
using System;

namespace Prana.DataManager
{
    public class BasketDataManager
    {
        private static readonly BasketDataManager instance = null;

        static BasketDataManager()
        {
            instance = new BasketDataManager();
        }
        public static BasketDataManager GetInstance()
        {
            return instance;
        }
        public bool SaveBasketRequest(BasketDetail basket)
        {
            bool result = false;
            object[] parameter = new object[5];
            try
            {
                parameter[0] = basket.BasketID;
                /// Now save the order id's as integers!!
                parameter[1] = basket.TradedBasketID;

                parameter[2] = basket.UserID;

                parameter[3] = DateTime.Now.ToUniversalTime();
                parameter[4] = basket.TradingAccountID;

                //TODO: write SP P_SaveOrderResponse
                if (DatabaseManager.DatabaseManager.ExecuteNonQuery("P_BTSaveTradedBasketDetails", parameter) > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            #endregion
            return result;
        }
    }
}

