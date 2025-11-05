using Prana.BusinessObjects;
using Prana.Global;
using Prana.TradeManager.Extension;

namespace Prana.DropCopyClient
{
    /// <summary>
    /// PranaDropCopyClient 
    /// </summary>
    public class PranaDropCopyClient
    {
        //static ICommunicationManager _tradeManager = null;
        private static CompanyUser _loginUser = null;
        //public static void SetUp(CompanyUser loginUser)
        //{
        //    // Don't use it for Now
        //    //_loginUser = loginUser;

        //    //TradeManager.TradeManager.GetInstance().InBoxOrderReceived += new TradeManager.TradeManager.DropCopyOrderRecievedDelegate(_tradeManager_InBoxOrderReceived);
        //    //TradeManager.TradeManager.GetInstance().OutBoxOrderReceived += new TradeManager.TradeManager.DropCopyOrderRecievedDelegate(_tradeManager_OutBoxOrderReceived);
        //    //InBox.getInstance.Init();
        //    //OutBox.getInstance.Init();
        //}
        ///// <summary>
        ///// because iyt waits for caching to complete on client side like  Trading accts informations
        ///// </summary>
        //public static void BindWithCachedData()
        //{
        //    //InBox.getInstance.BindTradingAccount();
        //    //OutBox.getInstance.BindOrderSide();
        //}
        static void _tradeManager_OutBoxOrderReceived(object sender, EventArgs<DropCopyOrder> e)
        {
            e.Value.OrderSide = Prana.CommonDataCache.TagDatabaseManager.GetInstance.GetOrderSideText(e.Value.OrderSideTagValue);
            e.Value.OrderType = Prana.CommonDataCache.TagDatabaseManager.GetInstance.GetOrderTypeText(e.Value.OrderTypeTagValue);
            e.Value.OrderStatus = Prana.CommonDataCache.TagDatabaseManager.GetInstance.GetOrderStatusText(e.Value.OrderStatusTagValue);
            e.Value.SetForUI();
            OutBox.getInstance.AddOrders(e.Value);
        }
        static void _tradeManager_InBoxOrderReceived(object sender, EventArgs<DropCopyOrder> e)
        {

            e.Value.OrderSide = Prana.CommonDataCache.TagDatabaseManager.GetInstance.GetOrderSideText(e.Value.OrderSideTagValue);
            e.Value.OrderType = Prana.CommonDataCache.TagDatabaseManager.GetInstance.GetOrderTypeText(e.Value.OrderTypeTagValue);
            e.Value.SetForUI();
            InBox.getInstance.AddOrders(e.Value);

            //OutBox.getInstance.AddOrders(dropCopyOrder);


        }
        /// <summary>
        /// LoginUser 
        /// </summary>
        public static CompanyUser LoginUser
        {
            get { return _loginUser; }
        }
        /// <summary>
        /// ClearData 
        /// </summary>
        public static void ClearData()
        {
            TradeManagerExtension.GetInstance().InBoxOrderReceived -= new TradeManagerExtension.DropCopyOrderRecievedDelegate(_tradeManager_InBoxOrderReceived);
            TradeManagerExtension.GetInstance().OutBoxOrderReceived -= new TradeManagerExtension.DropCopyOrderRecievedDelegate(_tradeManager_OutBoxOrderReceived);
            //InBox.getInstance.ClearInbox();

            InBox.getInstance.Close();
            OutBox.getInstance.Close();
            //OutBox.getInstance.ClearOutbox();
        }
    }
}
