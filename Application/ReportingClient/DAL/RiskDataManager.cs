using System;
using System.Collections.Generic;
using System.Text;

namespace ReportingClient
{
    public class RiskDataManager
    {
        private static RiskDataManager _singleton = null;
        private static object _locker = new object();

        public static RiskDataManager getInstance()
        {
            if (_singleton == null)
                lock (_locker)
                    if (_singleton == null)
                        _singleton = new RiskDataManager();
            return _singleton;
        }

        
    }
}
