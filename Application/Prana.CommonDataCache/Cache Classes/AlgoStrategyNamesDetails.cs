using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.CommonDataCache
{
    public static class AlgoStrategyNamesDetails
    {
        private static Dictionary<string, string> _algoStrategyNames = new Dictionary<string, string>();

        public static Dictionary<string, string> AlgoStrategyNamesInfo
        {
            get { return _algoStrategyNames; }
            set { _algoStrategyNames = value; }
        }

        /// <summary>
        /// Return algo strategy text based on ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetAlgoStrategyText(string ID, int brokerId)
        {
            try
            {
                var key = brokerId + "_" + ID;
                if (_algoStrategyNames.ContainsKey(key))
                {
                    return _algoStrategyNames[key];
                }
                else
                {
                    return "N.A.";
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return "N.A.";
            }
        }
    }
}
