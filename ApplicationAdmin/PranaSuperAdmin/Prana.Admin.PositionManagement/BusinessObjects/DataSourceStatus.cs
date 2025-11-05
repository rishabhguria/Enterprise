using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class DataSourceStatus
    {
        #region Private properties
        private int _statusID = int.MinValue;
        private string _name = string.Empty; 
        #endregion

        #region Public Properties
        public int StatusId
        {
            get { return _statusID; }
            set { _statusID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        } 
        #endregion

        #region Constructor 

        public DataSourceStatus()
		{
		}

        public DataSourceStatus(int statusID, string name)
		{
            StatusId = statusID;
            Name = name;
		}
        #endregion

    }
}
