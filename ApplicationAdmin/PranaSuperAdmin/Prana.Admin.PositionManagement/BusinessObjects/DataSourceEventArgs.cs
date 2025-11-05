using System;
using System.Collections.Generic;
using System.Text;
using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public class DataSourceEventArgs : EventArgs
    {
        private DataSourceNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the DataSourceNameID Business Object.
        /// </summary>
        /// <value>The DataSourceNameID.</value>
        public DataSourceNameID DataSourceNameID
        {
            get { return _dataSourceNameID; }
            set { _dataSourceNameID = value; }
        }
	
        //private string _fullName;

        ///// <summary>
        ///// Gets or sets the full name.
        ///// </summary>
        ///// <value>The full name.</value>
        //public string FullName
        //{
        //    get { return _fullName; }
        //    set { _fullName = value; }
        //}

        //private string _id;

        ///// <summary>
        ///// Gets or sets the ID.
        ///// </summary>
        ///// <value>The ID.</value>
        //public string ID
        //{
        //    get { return _id; }
        //    set { _id = value; }
        //}

        //private string _shortName;

        ///// <summary>
        ///// Gets or sets the name of the short.
        ///// </summary>
        ///// <value>The name of the short.</value>
        //public string ShortName
        //{
        //    get { return _shortName; }
        //    set { _shortName = value; }
        //}
    }
}
