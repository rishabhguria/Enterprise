using Prana.BusinessObjects.PositionManagement;

namespace Prana.PM.BLL
{
    class MapAUEC
    {
        private ThirdPartyNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public ThirdPartyNameID DataSourceNameID
        {
            get { return _dataSourceNameID; }
            set { _dataSourceNameID = value; }
        }

        private MappingItemList _mappingItemList;

        /// <summary>
        /// Gets or sets the mapping item list.
        /// </summary>
        /// <value>The mapping item list.</value>
        public MappingItemList MappingItemList
        {
            get { return _mappingItemList; }
            set { _mappingItemList = value; }
        }
    }
}
