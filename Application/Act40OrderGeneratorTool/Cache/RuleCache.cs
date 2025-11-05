using Prana.LogManager;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Act40OrderGeneratorTool.Cache
{
    class RuleCache
    {
        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static RuleCache _singiltonObject = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private RuleCache()
        {

        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static RuleCache GetInstance()
        {
            lock (_lock)
            {
                if (_singiltonObject == null)
                    _singiltonObject = new RuleCache();
                return _singiltonObject;
            }
        }
        #endregion

        private Rule _longRule = new Rule();
        private Rule _shortRule = new Rule();

        private const String LONG_RULE_FILE = "Long.Rebalancer.Rule";
        private const String SHORT_RULE_FILE = "Short.Rebalancer.Rule";

        /// <summary>
        /// Initialise the rule cache
        /// </summary>
        /// <returns></returns>
        internal Boolean Initialize()
        {
            try
            {
                if (File.Exists(LONG_RULE_FILE) && File.Exists(SHORT_RULE_FILE))
                {
                    Stream stream = File.Open(LONG_RULE_FILE, FileMode.Open);
                    BinaryFormatter bformatter = new BinaryFormatter();
                    _longRule = (Rule)bformatter.Deserialize(stream);
                    stream.Close();

                    stream = File.Open(SHORT_RULE_FILE, FileMode.Open);
                    bformatter = new BinaryFormatter();
                    _shortRule = (Rule)bformatter.Deserialize(stream);
                    stream.Close();

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Save the rule to cache and file
        /// </summary>
        /// <param name="longRule"></param>
        /// <param name="shortRule"></param>
        internal void SaveRule(Rule longRule, Rule shortRule)
        {
            try
            {
                _longRule = longRule;
                _shortRule = shortRule;

                Stream stream = File.Open(LONG_RULE_FILE, FileMode.Create);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, _longRule);
                stream.Close();

                stream = File.Open(SHORT_RULE_FILE, FileMode.Create);
                bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, _shortRule);
                stream.Close();
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

        /// <summary>
        /// Get the long side rule
        /// </summary>
        /// <returns></returns>
        internal Rule GetLongRule()
        {
            return _longRule;
        }

        /// <summary>
        /// Get the short side rule
        /// </summary>
        /// <returns></returns>
        internal Rule GetShortRule()
        {
            return _shortRule;
        }

    }
}
