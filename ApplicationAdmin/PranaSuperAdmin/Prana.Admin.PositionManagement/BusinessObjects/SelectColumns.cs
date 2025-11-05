using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    /// <summary>
    /// Responsible for setting up Source Columns!
    /// </summary>
    class SelectColumns
    {
        #region Constructors

        public SelectColumns()
        { 
        }

        #endregion

        private DataSourceNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public DataSourceNameID DataSourceNameID
        {
            get { return _dataSourceNameID; }
            set { _dataSourceNameID = value; }
        }

        private SortableSearchableList<SelectColumnsItem> _selectColumnItems;

        /// <summary>
        /// Gets or sets the select column items.
        /// </summary>
        /// <value>The select column items.</value>
        public SortableSearchableList<SelectColumnsItem> SelectColumnItems
        {
            get { return _selectColumnItems; }
            set { _selectColumnItems = value; }
        }
	
	
	
    }
}
