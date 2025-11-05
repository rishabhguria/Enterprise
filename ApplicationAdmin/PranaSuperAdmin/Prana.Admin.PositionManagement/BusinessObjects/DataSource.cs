using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{

    public class DataSource : BusinessBase<DataSource>
    {

        #region Private members And Public Properties

        private DataSourceNameID _dataSourceNameID;

        /// <summary>
        /// Gets or sets the data source name ID.
        /// </summary>
        /// <value>The data source name ID.</value>
        public DataSourceNameID DataSourceNameID
        {
            get 
                {
                    if (_dataSourceNameID == null )
                    {
                        _dataSourceNameID = new DataSourceNameID();
                    }
                    return _dataSourceNameID; 
                }
            set 
                { 
                    _dataSourceNameID = value; 
                }     
        }

        private int _status;

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public int StatusID
        {
            get { return _status; }
            set { _status = value; }
        }
	

       

        private int _typeID = 0;

        /// <summary>
        /// Gets or sets the type ID.
        /// </summary>
        /// <value>The type ID.</value>
        public int TypeID
        {
            get
            {
                return _typeID;
            }
            set
            {
               // if (int.Equals(value, 1))
              //  {
              //      errorMessage = "There is(are) error(s) in DataSourceDetails.";
              //      propErrors["TypeID"] = "Type ID is 1.";
                    FirePropertyChanged("TypeID");                    
               // }
             //   else
              //  {
                    _typeID = value;
             //   }
            }
        }

        private string _type = string.Empty;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public string Type
        {
            get
            {
                return _type;

            }
            set
            {
                _type = value;
            }
        }

        #region Moved to Address Details
        //private string _address1 = string.Empty;

        ///// <summary>
        ///// Gets or sets the address1.
        ///// </summary>
        ///// <value>The address1.</value>
        //public string Address1
        //{
        //    get
        //    {
        //        return _address1;

        //    }
        //    set
        //    {
        //        _address1 = value;
        //    }
        //}
        //private string _address2 = string.Empty;

        ///// <summary>
        ///// Gets or sets the address2.
        ///// </summary>
        ///// <value>The address2.</value>
        //public string Address2
        //{
        //    get
        //    {
        //        return _address2;

        //    }
        //    set
        //    {
        //        _address2 = value;
        //    }
        //}
        //private string _state = string.Empty;

        ///// <summary>
        ///// Gets or sets the state.
        ///// </summary>
        ///// <value>The state.</value>
        //public string State
        //{
        //    get
        //    {
        //        return _state;

        //    }
        //    set
        //    {
        //        _state = value;
        //    }
        //}
        //private int _stateID = 0;

        ///// <summary>
        ///// Gets or sets the state ID.
        ///// </summary>
        ///// <value>The state ID.</value>
        //public int StateID
        //{
        //    get
        //    {
        //        return _stateID;

        //    }
        //    set
        //    {
        //        _stateID = value;
        //    }
        //}
        //private string _country = string.Empty;

        ///// <summary>
        ///// Gets or sets the country.
        ///// </summary>
        ///// <value>The country.</value>
        //public string Country
        //{
        //    get
        //    {
        //        return _country;

        //    }
        //    set
        //    {
        //        _country = value;
        //    }
        //}

        //private int _countryID = 0;
        //public int CountryID
        //{
        //    get
        //    {
        //        return _countryID;

        //    }
        //    set
        //    {
        //        _countryID = value;
        //    }
        //}
        
        //private string _zip = string.Empty;

        ///// <summary>
        ///// Gets or sets the zip.
        ///// </summary>
        ///// <value>The zip.</value>
        //public string Zip
        //{
        //    get
        //    {
        //        return _zip;

        //    }
        //    set
        //    {
        //        _zip = value;
        //    }
        //}
        //private string _EMail = string.Empty;

        ///// <summary>
        ///// Gets or sets the E mail.
        ///// </summary>
        ///// <value>The E mail.</value>
        //public string EMail
        //{
        //    get
        //    {
        //        return _EMail;

        //    }
        //    set
        //    {
        //        _EMail = value;
        //    }
        //}

        //private string _workNumber = string.Empty;

        ///// <summary>
        ///// Gets or sets the work number.
        ///// </summary>
        ///// <value>The work number.</value>
        //public string WorkNumber
        //{
        //    get
        //    {
        //        return _workNumber;

        //    }
        //    set
        //    {                
        //        _workNumber = value;

        //    }
        //}        

        //private string _faxNumber = string.Empty;

        ///// <summary>
        ///// Gets or sets the fax number.
        ///// </summary>
        ///// <value>The fax number.</value>
        //public string FaxNumber
        //{
        //    get
        //    {
        //        return _faxNumber;

        //    }
        //    set
        //    {
        //        _faxNumber = value;
        //    }
        //}

        #endregion

        private AddressDetails _addressDetails;

        public AddressDetails DataSourceAddressDetails
        {
            get 
            {
                if (_addressDetails == null)
                {
                    _addressDetails = new AddressDetails();
                }
                return _addressDetails; 
            }
            set { _addressDetails = value; }
        }


        private DataSourcePrimaryContact _primaryContact;

        /// <summary>
        /// Gets or sets the primary contact.
        /// </summary>
        /// <value>The primary contact.</value>
        public DataSourcePrimaryContact PrimaryContact
        {
            get
            {
                if (_primaryContact == null)
                {
                    _primaryContact = new DataSourcePrimaryContact();                    
                }
                return _primaryContact;
            }
            set
            {
                _primaryContact = value;
            }
        }

       
        //public SortableSearchableList<DataSourceType> DataSourceTypeList
        //{
        //    get { return RetrieveDataSourceTypeList(); }           
        //}

        //public SortableSearchableList<Country> CountryList
        //{
        //    get { return RetrieveCountryList(); }           
        //}

        //public SortableSearchableList<State> StateList
        //{
        //    get { return RetrieveStateList(); }
        //}


        #region  Retrieve
        ///// <summary>
        ///// Retrieves the colection implementing inheriting BindingList Generic Type
        ///// using the appropriate mechanism
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>In a real application, this would call a data access component,
        ///// access a typed dataset or TableAdapter, or call a Web service.</remarks>
        //private SortableSearchableList<DataSourceType> RetrieveDataSourceTypeList()
        //{
        //    SortableSearchableList<DataSourceType> dataSourceTypeList = DataSourceManager.GetDataSourceTypes();
        //    dataSourceTypeList.Insert(0, new DataSourceType(0, "--Select--"));
        //    return dataSourceTypeList;
        //}

        ///// <summary>
        ///// Retrieves the country list.
        ///// </summary>
        ///// <returns></returns>
        //private SortableSearchableList<Country> RetrieveCountryList()
        //{
        //    SortableSearchableList<Country> CountryList = DataSourceManager.GetCountryList();
        //    CountryList.Insert(0, new Country(0, "--Select--"));
        //    return CountryList;            
        //}

        //private SortableSearchableList<State> RetrieveStateList()
        //{
        //    SortableSearchableList<State> stateList = DataSourceManager.GetStateList();
        //    stateList.Insert(0, new State(0, "--Select--", 0));
        //    return stateList;
        //}

        #endregion         

        #endregion

    }

   
}
