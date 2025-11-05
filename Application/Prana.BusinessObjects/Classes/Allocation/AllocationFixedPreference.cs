using Prana.LogManager;
using System;
using System.Data;

namespace Prana.BusinessObjects.Classes.Allocation
{
    [Serializable]
    public class AllocationFixedPreference
    {
        #region members

        /// <summary>
        /// The _scheme identifier
        /// </summary>
        private int _schemeID = int.MinValue;

        /// <summary>
        /// The _scheme name
        /// </summary>
        private string _schemeName = string.Empty;

        /// <summary>
        /// The _scheme
        /// </summary>
        private string _scheme = string.Empty;

        /// <summary>
        /// The _is preference visible
        /// </summary>
        private bool _isPrefVisible = false;

        /// <summary>
        /// The _ date
        /// </summary>
        private DateTime _date = DateTime.MinValue;

        /// <summary>
        /// The _creation source
        /// </summary>
        private FixedPreferenceCreationSource _creationSource;

        #endregion member

        #region properties

        /// <summary>
        /// Gets or sets the scheme identifier.
        /// </summary>
        /// <value>
        /// The scheme identifier.
        /// </value>
        public int SchemeID
        {
            get { return _schemeID; }
            set { _schemeID = value; }
        }

        /// <summary>
        /// Gets or sets the name of the scheme.
        /// </summary>
        /// <value>
        /// The name of the scheme.
        /// </value>
        public string SchemeName
        {
            get { return _schemeName; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is preference visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is preference visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrefVisible
        {
            get { return _isPrefVisible; }
            set { _isPrefVisible = value; }
        }

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        /// <value>
        /// The scheme.
        /// </value>
        public string Scheme
        {
            get { return _scheme; }
            set { _scheme = value; }
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// Gets or sets the creation source.
        /// </summary>
        /// <value>
        /// The creation source.
        /// </value>
        public FixedPreferenceCreationSource CreationSource
        {
            get { return _creationSource; }
            set { _creationSource = value; }
        }

        #endregion properties

        #region constructors

        public AllocationFixedPreference()
            : this(int.MinValue, string.Empty, string.Empty, DateTime.MinValue, true, 0)
        {

        }

        public AllocationFixedPreference(int schemeID, string schemeName, string scheme, DateTime date, bool isPrefVisible, FixedPreferenceCreationSource creationSource)
        {
            try
            {
                this._schemeID = schemeID;
                this._schemeName = schemeName;
                this._scheme = scheme;
                this._date = date;
                this._isPrefVisible = isPrefVisible;
                this._creationSource = creationSource;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Initializes a new instance of the AllocationFixedPreference class from a datarow.
        /// </summary>
        /// <param name="row">The row.</param>
        public AllocationFixedPreference(DataRow row, string scheme)
        {
            try
            {
                this._schemeID = Int32.Parse(row["AllocationSchemeID"].ToString());
                this._schemeName = row["AllocationSchemeName"].ToString();
                //Scheme Name not to be fetched for cache thereforre assigning empty string
                this._scheme = scheme;
                this._date = Convert.ToDateTime(row["Date"].ToString());
                this._isPrefVisible = Convert.ToBoolean(row["IsPrefVisible"].ToString());
                this._creationSource = (FixedPreferenceCreationSource)Int32.Parse(row["CreationSource"].ToString());
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }


        #endregion constructors

        #region methods



        #endregion methods

    }
}
