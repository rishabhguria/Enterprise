using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class DataSourceType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceType"/> class.
        /// </summary>
        public DataSourceType()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceType"/> class.
        /// </summary>
        /// <param name="dataSourceTypeID">The data source type ID.</param>
        /// <param name="dataSourceTypeName">Name of the data source type.</param>
        public DataSourceType(int dataSourceTypeID, string dataSourceTypeName)
        {
            this.DataSourceTypeID = dataSourceTypeID;
            this.DataSourceTypeName = dataSourceTypeName;
        }

        private int _dataSourceTypeID;

        /// <summary>
        /// Gets or sets the data source type ID.
        /// </summary>
        /// <value>The data source type ID.</value>
        public int DataSourceTypeID
        {
            get
            {
                return _dataSourceTypeID;
            }
            set
            {
                _dataSourceTypeID = value;
            }
        }


        private string _dataSourceTypeName;

        /// <summary>
        /// Gets or sets the name of the data source type.
        /// </summary>
        /// <value>The name of the data source type.</value>
        public string DataSourceTypeName
        {
            get
            {
                return _dataSourceTypeName;

            }
            set
            {
                _dataSourceTypeName = value;
            }
        }

        
        


    }
}
