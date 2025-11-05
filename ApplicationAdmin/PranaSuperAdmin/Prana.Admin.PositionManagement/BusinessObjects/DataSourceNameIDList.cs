using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    class DataSourceNameIDList : SortableSearchableList<DataSourceNameID>
    {
        /// <summary>
        /// Initializes the <see cref="DataSourceNameIDList"/> class.
        /// </summary>
        private DataSourceNameIDList()
        {

        }

        private static DataSourceNameIDList _dataSourceNameIDList = null;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static DataSourceNameIDList GetInstance()
        {
            if (_dataSourceNameIDList == null)
            {
                _dataSourceNameIDList = new DataSourceNameIDList();
            }
            return _dataSourceNameIDList;
        }

        private bool _isAllDataSourceAvailable = false;

        /// <summary>
        /// Sets a value indicating whether All data source item to be added in 
        /// combo or not.
        /// </summary>
        /// <value><c>true</c> if [add data source all]; otherwise, <c>false</c>.</value>
        public bool IsAllDataSourceAvailable
        {
            // private as this will be used only inside the class and not outside this class. 
            private get
            {
                return _isAllDataSourceAvailable;
            }
            set { _isAllDataSourceAvailable = value; }
        }


        private bool _isSelectItemRequired = true;
        /// <summary>
        /// Sets a value indicating whether [select item required].
        /// </summary>
        /// <value><c>true</c> if [select item required]; otherwise, <c>false</c>.</value>
        public bool SelectItemRequired
        {
            // private as this will be used only inside the class and not outside this class. 
            private get
            {
                return _isSelectItemRequired;
            }
            set { _isSelectItemRequired = value; }
        }
	

        /// <summary>
        /// Gets the retrieve.
        /// </summary>
        /// <value>The retrieve.</value>
        public SortableSearchableList<DataSourceNameID> Retrieve
        {
            get { return GetAllDataSourceNames(); }
        }

        /// <summary>
        /// Retrieves the DataSourcesList list.
        /// </summary>
        /// <returns></returns>
        private SortableSearchableList<DataSourceNameID> GetAllDataSourceNames()
        {
            SortableSearchableList<DataSourceNameID> dataSourceNameIDList = DataSourceManager.GetAllDataSourceNames();
            if (SelectItemRequired)
            {
                dataSourceNameIDList.Insert(0, new DataSourceNameID(-1, Constants.C_COMBO_SELECT,Constants.C_COMBO_SELECT));
            }
            if (IsAllDataSourceAvailable)
            {
                dataSourceNameIDList.Insert(0, new DataSourceNameID(0, Constants.C_COMBO_ALL));
            }

            return dataSourceNameIDList;
        }
    }
}
