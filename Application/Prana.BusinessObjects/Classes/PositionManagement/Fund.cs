using Csla;
using System;
using System.Xml.Serialization;

namespace Prana.BusinessObjects.PositionManagement
{
    /// <summary>
    //TODO : We need to use the same object of Prana.BusinessObject, hence need to merge...
    /// Summary description for Account.
    /// </summary>
    [Serializable, System.Runtime.InteropServices.ComVisible(false)]
    public class Account : BusinessBase<Account>
    {

        public Account()
        {
            MarkAsChild();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="accountID">The account ID.</param>
        /// <param name="shortname">The shortname.</param>
        public Account(int accountID, string fullname)
        {
            _id = accountID;
            _fullName = fullname;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="accountID">The account ID.</param>
        /// <param name="fullname">The fullname.</param>
        /// <param name="shortName">Name of the short.</param>
        public Account(int accountID, string fullName, string shortName)
            : this(accountID, fullName)
        {
            _shortName = shortName;
        }


        int _id;
        /// <summary>
        /// Gets or sets the account ID.
        /// </summary>
        /// <value>The account ID.</value>
        [XmlElement("AccountID")]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        string _shortName = string.Empty;
        /// <summary>
        /// Gets or sets short name.
        /// </summary>
        /// <value>The shortname.</value>
        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        string _fullName = string.Empty;
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return FullName;
        }


        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _id;
        }
    }
}
