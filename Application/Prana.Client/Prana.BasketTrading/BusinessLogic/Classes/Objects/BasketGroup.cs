using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
namespace Prana.BasketTrading
{
    /// <summary>
    /// For Basket Group Related Works
    /// </summary>
    public class BasketGroup
    {
         string _groupID = string.Empty;
       //string _tradedWaveID = string.Empty;
       string _basketID = string.Empty;
        string _waveID = string.Empty;
       OrderCollection _groupOrders = null;
       OrderCollection _tradedOrders = null;
       OrderCollection _origOrders = null;
        
       public BasketGroup()
       {
           _groupOrders = new OrderCollection();
           _groupID = IDGenerator.GenerateClientGroupID();
       }
      
       public string GroupID
       {
           get { return _groupID; }
           set { _groupID = value; }
       }
      
       public OrderCollection GroupOrders
       {
           get { return _groupOrders; }
           set { _groupOrders = value; }
       }
        public OrderCollection TradedOrders
        {
            get { return _tradedOrders; }
            set { _tradedOrders = value; }
        }
        public OrderCollection OrigOrders
        {
            get { return _origOrders; }
            set { _origOrders = value; }
        }
       public string BasketID
       {
           get { return _basketID; }
           set { _basketID = value; }
       }
        public string WaveID
        {
            get { return _waveID; }
            set { _waveID = value; }
        }
        private BasketTradingConstants.GridType  _parentGridType;

        public BasketTradingConstants.GridType  ParentGridType
        {
            get { return _parentGridType; }
            set { _parentGridType = value; }
        }
	
    }
}
