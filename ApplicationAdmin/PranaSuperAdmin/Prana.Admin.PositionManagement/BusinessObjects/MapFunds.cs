using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class MapFunds
    {
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

        private SortableSearchableList<MappingItem> _mappingItemList;

        /// <summary>
        /// Gets or sets the mapping item list.
        /// </summary>
        /// <value>The mapping item list.</value>
        public SortableSearchableList<MappingItem> MappingItemList
        {
            get { return _mappingItemList; }
            set { _mappingItemList = value; }
        }

    }
}
