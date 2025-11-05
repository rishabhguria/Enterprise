using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class DataSourceNameID : INotifyPropertyChanged
    {

        #region contructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceNameID"/> class.
        /// </summary>
        public DataSourceNameID()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceNameID"/> class.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        /// <param name="fullName">The full name.</param>
        public DataSourceNameID(int dataSourceID, string fullName)
        {
            ID = dataSourceID;
            FullName = fullName;
            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceNameID"/> class.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="shortName">Name of the short.</param>
        public DataSourceNameID(int dataSourceID, string fullName, string shortName) : this(dataSourceID, fullName)
        {
            ShortName = shortName;
        }

        #endregion

        #region members and properties

        private int _ID = int.MinValue;

        /// <summary>
        /// Gets or sets the company ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        private string _fullname = string.Empty;

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The name.</value>
        public string FullName
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                OnPropertyChanged("FullName");
            }
        }

        private string _shortname = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string ShortName
        {
            get
            {
                return _shortname;
            }
            set
            {
                _shortname = value;
                OnPropertyChanged("ShortName");
            }
        }
        #endregion

        public override string ToString()
        {
            return _shortname;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
