using Csla;
using Prana.BusinessObjects.PositionManagement;
using System;

namespace Prana.PM.BLL
{
    /// <summary>
    /// Responsible for setting up Source Columns!
    /// </summary>
    [Serializable()]
    public class SelectColumns : BusinessBase<SelectColumns>
    {
        #region Constructors

        public SelectColumns()
        {
            MarkAsChild();
        }

        #endregion

        #region Public properties and methods
        private ThirdPartyNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public ThirdPartyNameID DataSourceNameIDValue
        {
            get { return _dataSourceNameID; }
            set
            {
                _dataSourceNameID = value;
                PropertyHasChanged();
            }
        }

        private SelectColumnsItemList _selectColumnItems;

        /// <summary>
        /// Gets or sets the select column items.
        /// </summary>
        /// <value>The select column items.</value>
        public SelectColumnsItemList SelectColumnItems
        {
            get { return _selectColumnItems; }
            set
            {
                _selectColumnItems = value;
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
        #endregion



    }



}
