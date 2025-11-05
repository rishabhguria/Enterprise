using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        public User(string id, string name)
        {
            
            this.ID = id;
            this.UserName = name;
        }

        private string _userID;

        public string ID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }


        private string _password;

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public override string ToString()
        {
            return UserName;
        }
    }
}
