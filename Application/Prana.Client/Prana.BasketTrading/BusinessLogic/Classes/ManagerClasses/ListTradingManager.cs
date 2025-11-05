using System;
using System.Collections.Generic;
using System.Text;
using Prana.Interfaces ;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Utilities.DateTimeUtilities;
using Prana.Fix.FixDictionary;
using Prana.CommonDataCache;

namespace Prana.BasketTrading
{
    /// <summary>
    /// This Class  is Interface between 
    /// </summary>
    public class ListTradingManager
    {
       
        //private Prana.BusinessObjects.CompanyUser _loginUser;
        /// <summary>
        /// For Basket Received Events
        /// </summary>
        /// <param name="basket"></param>
        //public delegate void BasketEventHandler(BasketDetail basket);
        /// <summary>
        ///  For Basket Order Received Events
        /// </summary>
        /// <param name="order"></param>
        //public delegate void BasketOrderFillEventHandler(Order order);

        //public event BasketEventHandler BasketReceivedEvent;
        public event EventHandler<EventArgs<BasketDetail>> BasketReceivedEvent;

        public event EventHandler<EventArgs<Order>> BasketOrderFillReceivedEvent;

        //public event BasketOrderFillEventHandler BasketOrderFillReceivedEvent;
        private static int OPTIMUM_SIXE_OF_BASKET = 25;
        /// <summary>
        /// For Sending Messages to Server
        /// </summary>
       // private ICommunicationManager _communicationManager;
        /// <summary>
        /// For making this Class Singleton
        /// </summary>
        private static ListTradingManager _listTradingManager = null;
       

        private ListTradingManager()
        {
            //int.TryParse(System.Configuration.ConfigurationManager.AppSettings["OPTIMUM_SIXE_OF_BASKET"], out OPTIMUM_SIXE_OF_BASKET);
            int.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey("OPTIMUM_SIXE_OF_BASKET"), out OPTIMUM_SIXE_OF_BASKET);
            TradeManager.TradeManager.GetInstance().BasketOrderReceived += new Prana.TradeManager.TradeManager.BasketOrderMessageDelegate(_communicationManager_BasketOrderReceived);
            TradeManager.TradeManager.GetInstance().BasketReceived += new Prana.TradeManager.TradeManager.BasketMessageDelegate(_communicationManager_BasketReceived);
        }

        /// <summary>
        /// It sets Com Manager Instance for This class
        /// </summary>
        /// <param name="communicationManager"></param>



        void _communicationManager_BasketReceived(object sender, EventArgs<BasketDetail> e)
        {
            BasketReceivedEvent(this, e);
        }

        void _communicationManager_BasketOrderReceived(object sender, EventArgs<Order> e)
        {
            e.Value.SetForUI();
            BasketOrderFillReceivedEvent(this, e);
        }

        /// <summary>
        /// Message Received From Communication Manager are received here
        /// Basket or Order Related Objects are sent to Respective 
        /// Listener
        /// </summary>
        /// <param name="message"></param>

        public static ListTradingManager GetInstance
        {
            get
            {
                if (_listTradingManager == null)
                {
                    _listTradingManager = new ListTradingManager();
                }

                return _listTradingManager;
            }

        }
        /// <summary>
        /// For Trading a Basket send Selected Oredrs to Trade and Basket Details for Sending to Server 
        /// This Method returns List of Errors of these Orders if return string is empty it means all Orders
        /// sre tradable and they are sent to Server
        /// 
        /// Based on User Action Wave is traded , Canceled , Replaced
        /// </summary>
        /// <param name="basketdetails"></param>
        /// <param name="selectedOrdersForTrade"></param>
        /// <returns></returns>
        public bool  TradeList(BasketDetail basketdetails, OrderCollection tradableOrders,UserAction action)
        {
            
            bool sucess = false;
            try
            {
                int loopCount = 0;
                int OPTIMUM_SIXE_OF_BASKET = 25;
                List<OrderCollection> lstOrderColl = new List<OrderCollection>();

                foreach (Order order in tradableOrders)
                {
                    
                    if (action == UserAction.Trade)
                    {
                        order.MsgType = FIXConstants.MSGOrderSingle;
                        //order.Text = "New Order Sent";
                        order.SendQty = order.Quantity;
                    }
                    else if (action == UserAction.Replace)
                    {
                        order.MsgType = FIXConstants.MSGOrderCancelReplaceRequest;
                        order.Text = "Replace Request Sent";  
                    }
                    else // Cancel
                    {
                        order.MsgType = FIXConstants.MSGOrderCancelRequest;
                        order.Text = "Cancel Request Sent";                            
                    }
                    order.TradingAccountID = basketdetails.TradingAccountID;
                }

                if (tradableOrders.Count > OPTIMUM_SIXE_OF_BASKET)
                {
                    foreach (Order order in tradableOrders)
                    {
                        if (loopCount == OPTIMUM_SIXE_OF_BASKET)
                        {
                            loopCount = 0;
                        }
                        if (loopCount == 0)
                        {
                            OrderCollection orderCollection = new OrderCollection();
                            lstOrderColl.Add(orderCollection);
                        }
                        lstOrderColl[lstOrderColl.Count-1].Add(order);
                        loopCount++;
                    }
                    foreach (OrderCollection orderCollection in lstOrderColl)
                    {
                        if (TradeListInChunks(basketdetails, orderCollection, action))
                        {
                            sucess = false;
                        }
                    }
                }
                else
                {
                    TradeListInChunks(basketdetails, tradableOrders, action);
                }
                //update details of Trading
                //FillManager.GetInstance.SetTradingDetails(basketdetails, tradableOrders, action);
                   
               
            }
            catch (Exception ex)
            {

                sucess = false;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return sucess;
        }


        private bool TradeListInChunks(BasketDetail basketdetails, OrderCollection tradableOrders, UserAction action)
        {
             bool sucess = false;
            try
            {

                string msgType;
                if (action == UserAction.Trade)
                {
                    msgType = FIXConstants.MSGOrderList;
                }
                else if (action == UserAction.Replace)
                {
                    msgType = FIXConstants.MSGListReplace;
                }                 
                else // Cancel
                {
                    msgType = FIXConstants.MSGListCancelRequest;
                }
                PranaMessage PranaMessage = Transformer.CreatePranaMessageThroughReflection(basketdetails, tradableOrders, msgType);
                // Send to Communication Manager
                
                TradeManager.TradeManager.GetInstance().SendMessage(PranaMessage);
                sucess = true;

              
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
                sucess = false;
                
            }
            return sucess;
        }

        /// <summary>
        /// For Cancelling a Wave
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="selectedOrders"></param>
        /// <returns></returns>
        

        /// <summary>
        /// For Checking whether Server is Connected or not
        /// </summary>
        public PranaInternalConstants.ConnectionStatus ConnectionStatus
        {
            get { return TradeManager.TradeManager.GetInstance().ConnectionStatus; }
        }
        /// <summary>
        /// Cleaning
        /// </summary>
        public void Dispose()
        {
            TradeManager.TradeManager.GetInstance().BasketOrderReceived -= new Prana.TradeManager.TradeManager.BasketOrderMessageDelegate(_communicationManager_BasketOrderReceived);
            TradeManager.TradeManager.GetInstance().BasketReceived -= new Prana.TradeManager.TradeManager.BasketMessageDelegate(_communicationManager_BasketReceived);
            _listTradingManager = null;
        }

    }
    public enum UserAction
    { 
        Trade,
        Cancel,
        Replace
    }

    
}
