namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ConfirmationPopUp.
    /// </summary>
    public class ConfirmationPopUp
    {
        private bool _iSNewOrder;
        private bool _iSCXL;
        private bool _iSCXLReplace;
        /// <summary>
        /// The is manual order
        /// </summary>
        private bool _isManualOrder = true;
        private int _companyUserID = int.MinValue;



        public ConfirmationPopUp()
        {

        }
        public bool ISNewOrder
        {
            get { return _iSNewOrder; }
            set { _iSNewOrder = value; }
        }
        public bool ISCXL
        {
            get { return _iSCXL; }
            set { _iSCXL = value; }
        }
        public bool ISCXLReplace
        {
            get { return _iSCXLReplace; }
            set { _iSCXLReplace = value; }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is manual order.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is manual order; otherwise, <c>false</c>.
        /// </value>
        public bool IsManualOrder
        {
            get { return _isManualOrder; }
            set { _isManualOrder = value; }
        }

        public int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }
    }
}
