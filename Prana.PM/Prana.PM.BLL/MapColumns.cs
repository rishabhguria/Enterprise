using Csla;
using Prana.BusinessObjects.PositionManagement;
using System;

//


namespace Prana.PM.BLL
{
    /// <summary>
    /// Responsible for Mapping Application Columns with Source Columns!
    /// </summary>
    [Serializable()]
    public class MapColumns : BusinessBase<MapColumns>
    {
        private ThirdPartyNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public ThirdPartyNameID DataSourceNameID
        {
            get
            {
                if (_dataSourceNameID == null)
                {
                    _dataSourceNameID = new ThirdPartyNameID();
                }
                return _dataSourceNameID;
            }
            set
            {
                _dataSourceNameID = value;
                PropertyHasChanged();
            }
        }

        private MappingItemList _mappingItems;

        /// <summary>
        /// Gets or sets the mapping item list.
        /// </summary>
        /// <value>The mapping item list.</value>
        public MappingItemList MappingItems
        {
            get { return _mappingItems; }
            set
            {
                _mappingItems = value;
                PropertyHasChanged();
            }
        }

        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        // private int _id;

        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return 0;
        }

    }
}
