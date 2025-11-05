using System.Collections.Generic;

namespace Prana.ExpnlService
{
    public class UserWisePermissions
    {
        private List<int> _allowedAccounts = new List<int>();
        public List<int> AllowedAccounts
        {
            get { return _allowedAccounts; }
            set { _allowedAccounts = value; }
        }

        private bool _isMarketDataPermissionEnabled = false;
        public bool IsMarketDataPermissionEnabled
        {
            get { return _isMarketDataPermissionEnabled; }
            set { _isMarketDataPermissionEnabled = value; }
        }
    }
}