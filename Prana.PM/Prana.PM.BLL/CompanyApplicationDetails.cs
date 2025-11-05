//
using Prana.BusinessObjects.PositionManagement;

namespace Prana.PM.BLL
{
    /// <summary>
    /// Responsible for Company Application Details
    /// </summary>
    public class CompanyApplicationDetails
    {

        private CompanyNameID _companyNameID;

        /// <summary>
        /// Gets or sets the companyname and ID.
        /// </summary>
        /// <value>The companyname and ID.</value>
        public CompanyNameID CompanyNameID
        {
            get
            {
                if (_companyNameID == null)
                {
                    _companyNameID = new CompanyNameID();
                }

                return _companyNameID;
            }
            set { _companyNameID = value; }
        }


        private PricingModel _pricingModel;

        /// <summary>
        /// Gets or sets the pricing model.
        /// </summary>
        /// <value>The pricing model.</value>
        public PricingModel PricingModel
        {
            get
            {
                if (_pricingModel == null)
                {
                    _pricingModel = new PricingModel();
                }

                return _pricingModel;
            }
            set { _pricingModel = value; }
        }

        private bool _allowDailyImport;

        /// <summary>
        /// Gets or sets a value indicating whether [allow daily import].
        /// </summary>
        /// <value><c>true</c> if [allow daily import]; otherwise, <c>false</c>.</value>
        public bool AllowDailyImport
        {
            get { return _allowDailyImport; }
            set { _allowDailyImport = value; }
        }

        //private int _minimumRefreshRate;

        ///// <summary>
        ///// Gets or sets the minimum refresh rate.
        ///// </summary>
        ///// <value>The minimum refresh rate.</value>
        //public int MinimumRefreshRate
        //{
        //    get { return _minimumRefreshRate; }
        //    set { _minimumRefreshRate = value; }
        //}

        //private SortableSearchableList<DataSourceNameID> _dataSourceNameIDList;

        ///// <summary>
        ///// Gets or sets the data source name ID.
        ///// </summary>
        ///// <value>The data source name ID.</value>        
        //public SortableSearchableList<DataSourceNameID> DataSourceNameIDList
        //{
        //    get { return _dataSourceNameIDList; }
        //    set { _dataSourceNameIDList = value; }
        //}


        //private bool _allowDataMapping;
        ///// <summary>
        ///// Gets or sets a value indicating whether [allow data mapping].
        ///// </summary>
        ///// <value><c>true</c> if [allow data mapping]; otherwise, <c>false</c>.</value>
        //public bool AllowDataMapping
        //{
        //    get { return _allowDataMapping; }
        //    set { _allowDataMapping = value; }
        //}



    }
}
