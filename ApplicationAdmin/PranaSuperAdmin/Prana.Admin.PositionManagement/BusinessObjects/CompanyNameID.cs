using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class CompanyNameID
    {
        public CompanyNameID()
        {

        }

        public CompanyNameID(int companyID, string fullName)
        {
            this.ID = companyID;
            this.FullName = fullName;
        }

        public CompanyNameID(int companyID, string fullName, string shortName) : this(companyID, fullName)
        {
            this.ShortName = shortName;
        }




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
            }
        }

        private string _fullname = string.Empty;

        /// <summary>
        /// Gets or sets the name.
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
            }
        }

        private string _shortName;

        /// <summary>
        /// Gets or sets the Short Name of the company.
        /// </summary>
        /// <value>Short Name of the company.</value>
        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return _shortName;
        }
        

	
        #endregion
    }
}
