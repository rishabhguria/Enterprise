using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.RuleEngine.BusinessObjects
{
    public class UserPermission
    {
        private int _userId;

        public int userId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        private string _userRole;

        public string userRole
        {
            get { return _userRole; }
            set { _userRole = value; }
        }
        private String[] _packageList;

        public String[]  packageList
        {
            get { return _packageList; }
            set { _packageList = value; }
        }
	
	
	
    }
}
