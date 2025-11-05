using Prana.LogManager;
using System;

namespace Prana.SecurityMasterNew.BLL
{
    public class SecurityMasterSymbolIDGenerator
    {
        private static long _symbolPKID = 0;

        private static readonly Object _syncroot = 0;

        static SecurityMasterSymbolIDGenerator()
        {
            try
            {
                lock (_syncroot)
                {
                    _symbolPKID = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssFFF"));
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

        public static long GenerateSymbolPKID()
        {
            //TO DO need to handle teh case if _ID > Int64.MaxValue. Consider the case, when multiple client db's are merged together
            // and there are many ids generated.
            try
            {
                lock (_syncroot)
                {
                    _symbolPKID++;
                    return _symbolPKID;
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
            return long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssFFF"));
        }
        public static void SetMaxGeneratedIDFromDB(Int64 maxIDFromDB)
        {
            try
            {
                lock (_syncroot)
                {
                    if (_symbolPKID < maxIDFromDB)
                        _symbolPKID = maxIDFromDB;
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
