
using Csla;
using Prana.BusinessObjects.PositionManagement;


namespace Prana.PM.BLL
{
    public class MapAccounts : BusinessBase<MapAccounts>
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

        private CompanyNameID _companyNameID;

        /// <summary>
        /// Gets or sets the company name ID.
        /// </summary>
        /// <value>The company name ID.</value>
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
            set
            {
                _companyNameID = value;
                PropertyHasChanged();
            }
        }


        private MappingItemList _mappingItemList;


        /// <summary>
        /// Gets or sets the mapping item list.
        /// </summary>
        /// <value>The mapping item list.</value>
        public MappingItemList MappingItems
        {
            get { return _mappingItemList; }
            set
            {
                _mappingItemList = value;
                PropertyHasChanged();
            }
        }

        public override bool IsValid
        {
            get
            {
                if (_mappingItemList == null)
                {
                    return base.IsValid;
                }
                else
                {
                    return base.IsValid && _mappingItemList.IsValid;
                }
            }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        //private int _id;

        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            //return _id;
            return 0;
        }

    }
}
