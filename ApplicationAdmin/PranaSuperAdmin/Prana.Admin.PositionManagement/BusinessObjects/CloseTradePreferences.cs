using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class CloseTradePreferences
    {
        private Asset _asset;

        public Asset Asset
        {
            get 
            {
                if (_asset == null)
                {
                    _asset = new Asset();
                }
                return _asset; 
            }
            set { _asset = value; }
        }

        private SortableSearchableList<Underlying> _underlyings;

        /// <summary>
        /// Gets or sets the underlyings.
        /// </summary>
        /// <value>The underlyings.</value>
        public SortableSearchableList<Underlying> Underlyings
        {
            get {
                if (_underlyings==null)
                {
                    _underlyings = new SortableSearchableList<Underlying>();
                }
                   return _underlyings;
                }
            set { _underlyings = value; }
        }

        private SortableSearchableList<Exchange> _exchanges;

        /// <summary>
        /// Gets or sets the exchanges.
        /// </summary>
        /// <value>The exchanges.</value>
        public SortableSearchableList<Exchange> Exchanges
        {
            get 
            {
                if (_exchanges==null)
                {
                    _exchanges = new SortableSearchableList<Exchange>();
                }
                return _exchanges; 
            }
            set { _exchanges = value; }
        }

        private SortableSearchableList<Fund> _funds;

        /// <summary>
        /// Gets or sets the funds.
        /// </summary>
        /// <value>The funds.</value>
        public SortableSearchableList<Fund> Funds
        {
            get 
            {
                if (_funds== null)
                {
                    _funds = new SortableSearchableList<Fund>();
                }
                return _funds; 
            }
            set { _funds = value; }
        }

        private DataSourceNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public DataSourceNameID DataSourceNameID
        {
            get 
            {
                if (_dataSourceNameID ==null)
                {
                    _dataSourceNameID = new DataSourceNameID();
                }  
                return _dataSourceNameID; 
            }
            set { _dataSourceNameID = value; }
        }


        private CloseTradeMethodology _defaultMethodology;

        /// <summary>
        /// Gets or sets the default methodology.
        /// </summary>
        /// <value>The default methodology.</value>
        public CloseTradeMethodology DefaultMethodology
        {
            get { return _defaultMethodology; }
            set { _defaultMethodology = value; }
        }

        private CloseTradeAlogrithm _algorithm;

        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        /// <value>The algorithm.</value>
        public CloseTradeAlogrithm Algorithm
        {
            get { return _algorithm; }
            set { _algorithm = value; }
        }
	
    }
}
