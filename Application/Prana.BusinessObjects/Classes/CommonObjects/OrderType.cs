namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for OrderType.
    /// </summary>
    public class OrderType
    {
        int _orderTypesID = int.MinValue;
        string _orderTypes = string.Empty;
        string _tagValue = string.Empty;

        int _auecID = int.MinValue;
        int _cvAuecID = int.MinValue;

        public OrderType()
        {
        }

        public OrderType(int orderTypesID, string type, string tagValue)
        {
            _orderTypesID = orderTypesID;
            _orderTypes = type;
            _tagValue = tagValue;
        }
        public OrderType(string type, string tagValue)
        {
            _orderTypes = type;
            _tagValue = tagValue;
        }

        public int OrderTypesID
        {
            get
            {
                return _orderTypesID;
            }

            set
            {
                _orderTypesID = value;
            }
        }


        public string Type
        {
            get
            {
                return _orderTypes;
            }

            set
            {
                _orderTypes = value;
            }
        }

        public string TagValue
        {
            get
            {
                return _tagValue;
            }

            set
            {
                _tagValue = value;
            }
        }

        public int AUECID
        {
            get
            {
                return _auecID;
            }

            set
            {
                _auecID = value;
            }
        }

        public int CVAUECID
        {
            get
            {
                return _cvAuecID;
            }

            set
            {
                _cvAuecID = value;
            }
        }

    }


}
