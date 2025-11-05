using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class DataSourcePrimaryContact
    {

        #region Constructor/s
        public DataSourcePrimaryContact()
        {
            //Initialize the Primary contact fields with empty strings;
            FirstName = string.Empty;
            LastName = string.Empty;
            Title = string.Empty;
            EMail = string.Empty;
            WorkNumber = string.Empty;
            CellNumber = string.Empty;

        }
        #endregion 

        #region Private fields and Public Properties
        private string _FirstName = string.Empty;
        /// <summary>
        /// Gets or sets the name of the first.
        /// </summary>
        /// <value>The name of the first.</value>
        public string FirstName
        {
            get
            {
                return _FirstName;

            }
            set
            {
                _FirstName = value;
            }
        }
        private string _LastName = string.Empty;
        /// <summary>
        /// Gets or sets the name of the last.
        /// </summary>
        /// <value>The name of the last.</value>
        public string LastName
        {
            get
            {
                return _LastName;

            }
            set
            {
                _LastName = value;
            }
        }
        private string _Title = string.Empty;
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get
            {
                return _Title;

            }
            set
            {
                _Title = value;
            }
        }
        private string _EMail = string.Empty;
        /// <summary>
        /// Gets or sets the E mail.
        /// </summary>
        /// <value>The E mail.</value>
        public string EMail
        {
            get
            {
                return _EMail;

            }
            set
            {
                _EMail = value;
            }
        }
        private string _workNumber = string.Empty;
        /// <summary>
        /// Gets or sets the work number.
        /// </summary>
        /// <value>The work number.</value>
        public string WorkNumber
        {
            get
            {
                return _workNumber;

            }
            set
            {
                _workNumber = value;
            }
        }
        private string _CellNumber = string.Empty;
        /// <summary>
        /// Gets or sets the cell number.
        /// </summary>
        /// <value>The cell number.</value>
        public string CellNumber
        {
            get
            {
                return _CellNumber;

            }
            set
            {
                _CellNumber = value;
            }
        }
        #endregion

    }
}
