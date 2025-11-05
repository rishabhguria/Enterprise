using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for Account.
    /// </summary>
    [Serializable]
    public class Account
    {
        int _accountID = int.MinValue;
        string _name = string.Empty;

        public Account()
        {
        }

        public Account(int accountID, string name)
        {
            _accountID = accountID;
            _name = name;
        }

        public Account(int accountID, string name, string fullName)
        {
            _accountID = accountID;
            _name = name;
            _fullName = fullName;
        }

        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        /// <summary>
        /// this should be short name !! - Ram
        /// modified on 23rd Apr 2007
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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

        public override string ToString()
        {
            return FullName;
        }

        /// <summary>
        /// The is swap account
        /// </summary>
        bool _isSwapAccount = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is swap account.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is swap account; otherwise, <c>false</c>.
        /// </value>
        public bool IsSwapAccount
        {
            get { return _isSwapAccount; }
            set { _isSwapAccount = value; }
        }
    }
}
