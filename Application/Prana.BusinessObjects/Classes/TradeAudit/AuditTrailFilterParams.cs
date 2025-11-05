using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class AuditTrailFilterParams
    {
        private DateTime _fromDate = DateTimeConstants.MinValue;

        /// <summary>
        /// From Date
        /// </summary>
        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }

        private DateTime _toDate = DateTimeConstants.MinValue;

        /// <summary>
        /// To Date
        /// </summary>
        public DateTime ToDate
        {
            get { return _toDate; }
            set { _toDate = value; }
        }

        private string _groupIDs;

        /// <summary>
        /// Comma separated  Group IDs
        /// </summary>
        public string GroupIDs
        {
            get { return _groupIDs; }
            set { _groupIDs = value; }
        }

        private string _orderIDs = string.Empty;

        /// <summary>
        /// Comma separated   Order IDs
        /// </summary>
        public string OrderIDs
        {
            get { return _orderIDs; }
            set { _orderIDs = value; }
        }

        private string _accountIDs;

        /// <summary>
        /// Comma separated  Account IDs
        /// </summary>
        public string AccountIDs
        {
            get { return _accountIDs; }
            set { _accountIDs = value; }
        }



        private string _orderSides;

        /// <summary>
        ///Comma separated   Order Sides
        /// </summary>
        public string OrderSides
        {
            get { return _orderSides; }
            set { _orderSides = value; }
        }

        private string _sourceIDs;

        /// <summary>
        /// Comma separated  Source IDs
        /// </summary>
        public string SourceIDs
        {
            get { return _sourceIDs; }
            set { _sourceIDs = value; }
        }

        private string _symbol = string.Empty;

        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private string _ignoredUsers = string.Empty;

        /// <summary>
        /// Comma separated  Ignored Users
        /// </summary>
        public string IgnoredUsers
        {
            get { return _ignoredUsers; }
            set { _ignoredUsers = value; }
        }


    }
}
