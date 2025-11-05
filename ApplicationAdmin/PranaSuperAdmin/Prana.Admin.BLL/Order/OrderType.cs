namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for OrderType.
    /// </summary>
    public class OrderType
    {
        private int _orderTypesID = int.MinValue;
        private string _orderTypes = string.Empty;
        private string _tagValue = string.Empty;

        private int _companyID = int.MinValue;
        private int _cvAuecID = int.MinValue;

        public OrderType()
        {
        }

        public OrderType(int orderTypesID, string type, string tagValue)
        {
            _orderTypesID = orderTypesID;
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

        public int CompanyID
        {
            get
            {
                return _companyID;
            }

            set
            {
                _companyID = value;
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