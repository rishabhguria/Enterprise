using Csla;
using System;

namespace Prana.PM.BLL
{
    [Serializable()]
    public class DataSourceTable : BusinessBase<DataSourceTable>
    {
        public DataSourceTable()
        {
            MarkAsChild();
        }

        private int _thirdPartyID;

        /// <summary>
        /// Gets or sets the data source ID.
        /// </summary>
        /// <value>The data data source ID.</value>
        public int ThirdPartyID
        {
            get
            {
                return _thirdPartyID;
            }
            set
            {
                _thirdPartyID = value;
                PropertyHasChanged();
            }
        }

        private int _dataSourceTableTypeID;
        /// <summary>
        /// Gets or sets the data source table type ID.
        /// </summary>
        /// <value>The data source table type ID.</value>
        public int DataSourceTableTypeID
        {
            get
            {
                return _dataSourceTableTypeID;
            }
            set
            {
                _dataSourceTableTypeID = value;
                PropertyHasChanged();
            }
        }


        private string _tableName;

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName
        {
            get
            {
                return _tableName;

            }
            set
            {
                _tableName = value;
                PropertyHasChanged();
            }
        }

        private int _tableTypeID;
        /// <summary>
        /// Gets or sets the table type ID.
        /// </summary>
        /// <value>The data table type ID.</value>
        public int TableTypeID
        {
            get
            {
                return _tableTypeID;
            }
            set
            {
                _tableTypeID = value;
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
