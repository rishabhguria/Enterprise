using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.AutomationHandlers
{
    public class OrderIDGenerator
    {

        private static Object thisLockExternalOrderID = new Object();

        private static Int64 _externalOrderID = 0;
        static OrderIDGenerator()
        {


            _externalOrderID = Int64.Parse(DateTime.Now.ToString("yyMMddHHmmss"));

        }

        /// <summary>
        /// used from server when with savedFromServer  set to true
        /// </summary>
        /// <param name="savedFromServer"></param>
        /// <returns></returns>
        public static string GenerateExternalOrderID()
        {
            lock (thisLockExternalOrderID)
            {
                _externalOrderID++;
            }
            return _externalOrderID.ToString();

        }
    }
}
