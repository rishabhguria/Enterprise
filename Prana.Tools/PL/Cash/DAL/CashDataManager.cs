using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Data;
using System.Data.SqlClient;
using Prana.Global;
using System.Data.Common;
using Prana.BusinessObjects;
using Prana.Interfaces;

namespace Prana.Tools
{
    class CashDataManager
    {
        ICashManagementService _CashManagementServices = null;
        
        public ICashManagementService CashManagementServices
        {
            set
            {
                _CashManagementServices = value;

            }
        }

        private static CashDataManager _singleton = null;
        private static object _locker = new object();

        public static CashDataManager GetInstance()
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new CashDataManager();
                    }
                }
            }
            return _singleton;
        }

    }
}
