namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for AdvancedOrder.
    /// </summary>
    public class AdvancedOrder
    {
        int _advancedOrdersID = int.MinValue;
        string _advancedOrders = string.Empty;

        public AdvancedOrder()
        {
        }

        public AdvancedOrder(int advancedOrdersID, string advancedOrders)
        {
            _advancedOrdersID = advancedOrdersID;
            _advancedOrders = advancedOrders;
        }

        public int AdvancedOrdersID
        {
            get
            {
                return _advancedOrdersID;
            }

            set
            {
                _advancedOrdersID = value;
            }
        }


        public string AdvancedOrders
        {
            get
            {
                return _advancedOrders;
            }

            set
            {
                _advancedOrders = value;
            }
        }

    }


}
