using Prana.LogManager;
using System;
using System.Configuration;

namespace Prana.Global
{
    public class uIDGenerator
    {
        private static Int64 _ID = 0;
        private static int _IDint = 0;
        private static readonly Object thisLockID = new Object();
        private static readonly Object thisLockID_int = new Object();
        private static bool _addZeroPrefixToInIDs = false;

        static uIDGenerator()
        {
            try
            {
                lock (thisLockID)
                {
                    _addZeroPrefixToInIDs = Convert.ToBoolean(ConfigurationManager.AppSettings["AddZeroPrefixToInIDs"]);
                    _ID = Int64.Parse(DateTime.Now.ToString("yyMMddHHmmss"));
                    _IDint = int.Parse(DateTime.Now.ToString("mmss"));
                }
            }
            catch (Exception ex)
            {
                //ex.Message += "  Error while initializing id.";
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static string GenerateID()
        {
            //TO DO need to handle teh case if _ID > Int64.MaxValue. Consider the case, when multiple client db's are merged together
            // and there are many ids generated.
            try
            {
                lock (thisLockID)
                {
                    _ID++;
                    return _addZeroPrefixToInIDs ? "0" + _ID.ToString() : _ID.ToString();
                }
            }
            catch (Exception ex)
            {
                //ex.Message += "  Error while generating id in IDGenerator.GenerateID()";
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public static string GenerateIDLong()
        {
            try
            {
                lock (thisLockID_int)
                {
                    _IDint++;
                    return _addZeroPrefixToInIDs ? "0" + _IDint.ToString() : _IDint.ToString();
                }
            }
            catch (Exception ex)
            {
                //ex.Message += "  Error while generating id in IDGenerator.GenerateIDLong()";
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gaurav: To avoid duplicate key generation . maxIDFromDB is the max id saved in db.
        /// Currently this class is being used by Cash management only so we are passing maxid in T_Journal only       
        /// </summary>
        /// <param name="maxIDFromDB"></param>
        public static void SetMaxGeneratedIDFromDB(Int64 maxIDFromDB)
        {
            try
            {
                lock (thisLockID)
                {
                    if (_ID < maxIDFromDB)
                        _ID = maxIDFromDB;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
