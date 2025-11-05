using Prana.Interfaces;
using System.Collections.Generic;

namespace Prana.MarketDataPermissionService
{
    internal class MarketDataPermissionDetail
    {
        private int _companyUserID;
        internal int CompanyUserID
        {
            get { return _companyUserID; }
            set { _companyUserID = value; }
        }

        private bool _hasPermission;
        internal bool HasPermission
        {
            get { return _hasPermission; }
            set { _hasPermission = value; }
        }

        private Dictionary<string, IMarketDataPermissionServiceCallback> _callback = new Dictionary<string, IMarketDataPermissionServiceCallback>();
        internal Dictionary<string, IMarketDataPermissionServiceCallback> Callback
        {
            get { return _callback; }
        }
    }
}
