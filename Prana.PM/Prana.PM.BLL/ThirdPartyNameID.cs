using System;
using System.Collections.Generic;
using System.Text;

using Prana.PM.BLL;

using Prana.BusinessObjects;
using Prana.PM.DAL;
using Prana.Global;
namespace Prana.PM.BLL
{
    public class ThirdPartyNameID
    {
          #region contructor


        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyNameID"/> class.
        /// </summary>
        public ThirdPartyNameID()
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyNameID"/> class.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <param name="fullName">The full name.</param>
        public ThirdPartyNameID(int ID, string fullName)
        {
            this.ID = ID;
            this.FullName = fullName;
            
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyNameID"/> class.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="shortName">Name of the short.</param>
        public ThirdPartyNameID(int ID, string fullName, string shortName) : this(ID, fullName)
        {
            this.ShortName = shortName;
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
                //FirePropertyChanged("ID");
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
                //FirePropertyChanged("FullName");
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
                //FirePropertyChanged("ShortName");
            }
        }
        #endregion

        public override string ToString()
        {
            return _shortname;
        }

        public static SortableSearchableList<ThirdPartyNameID> RetrieveList()
        {
            SortableSearchableList<ThirdPartyNameID> thirdPartyNameID = DataSourceManager.GetAllThirdPartyNames();
            thirdPartyNameID.Insert(0, new ThirdPartyNameID(-1, ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
            return thirdPartyNameID;
        }

    }
}
