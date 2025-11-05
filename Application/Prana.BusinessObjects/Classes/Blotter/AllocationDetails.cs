using Prana.BusinessObjects.Classes.BusinessBaseClass;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Classes.Blotter
{
    public class AllocationDetails : AdditionalTradeAttributes
    {
        private string _clOrderID = string.Empty;
        public string ClOrderID
        {
            get { return _clOrderID; }
            set { _clOrderID = value; }
        }

        private PostTradeConstants.ORDERSTATE_ALLOCATION _allocationStatus = PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED;
        public PostTradeConstants.ORDERSTATE_ALLOCATION AllocationStatus
        {
            get { return _allocationStatus; }
            set { _allocationStatus = value; }
        }

        private List<AllocationLevelClass> _level1Allocation = null;
        public List<AllocationLevelClass> Level1Allocation
        {
            get { return _level1Allocation; }
            set { _level1Allocation = value; }
        }

        /// <summary>  
        /// The _allocation scheme name  
        /// </summary>  
        private string _allocationSchemeName = string.Empty;

        /// <summary>  
        /// Gets or sets the name of the allocation scheme.  
        /// </summary>  
        /// <value>  
        /// The name of the allocation scheme.  
        /// </value>  
        public string AllocationSchemeName
        {
            get { return _allocationSchemeName; }
            set { _allocationSchemeName = value; }
        }

        /// <summary>  
        /// Gets or sets the TradeAttribute1.  
        /// </summary>  
        public string TradeAttribute1 { get; set; }

        /// <summary>  
        /// Gets or sets the TradeAttribute2.  
        /// </summary>  
        public string TradeAttribute2 { get; set; }

        /// <summary>  
        /// Gets or sets the TradeAttribute3.  
        /// </summary>  
        public string TradeAttribute3 { get; set; }

        /// <summary>  
        /// Gets or sets the TradeAttribute4.  
        /// </summary>  
        public string TradeAttribute4 { get; set; }

        /// <summary>  
        /// Gets or sets the TradeAttribute5.  
        /// </summary>  
        public string TradeAttribute5 { get; set; }

        /// <summary>  
        /// Gets or sets the TradeAttribute6.  
        /// </summary>  
        public string TradeAttribute6 { get; set; }

       private double _avgPrice;
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }

        private double _fxRate;
        public double FxRate
        {
            get { return _fxRate; }
            set { _fxRate = value; }
        }

        private string _fXConversionMethodOperator;
        public string FXConversionMethodOperator
        {
            get { return _fXConversionMethodOperator; }
            set { _fXConversionMethodOperator = value; }
        }
    }
}
