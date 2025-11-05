using Act40OrderGeneratorTool.Cache;
using Act40OrderGeneratorTool.Classes;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Act40OrderGeneratorTool
{
    internal class Manager
    {
        #region SingiltonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static Object _lock = new Object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static Manager _singiltonObject = null;

        /// <summary>
        /// private cunstructor
        /// </summary>
        private Manager()
        {

        }

        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        internal static Manager GetInstance()
        {
            lock (_lock)
            {
                if (_singiltonObject == null)
                    _singiltonObject = new Manager();
                return _singiltonObject;
            }
        }
        #endregion

        /// <summary>
        /// Initialises the account and position cache
        /// </summary>
        /// <returns></returns>
        internal Boolean LoadData()
        {
            ReplacementMatrix.GetInstance();
            if (Account.GetInstance().Initialize() && MasterFund.GetInstance().Initialize() && PositionCache.GetInstance().Initialize(Preference.GetInstance().ModelPrefrence, Preference.GetInstance().CalculationPreference))
                return true;
            return false;
        }

        /// <summary>
        /// Initialises the rules cacehe
        /// </summary>
        /// <returns></returns>
        internal Boolean LoadRules()
        {
            if (RuleCache.GetInstance().Initialize())
                return true;
            return false;
        }

        /// <summary>
        /// returns the model account with the rules applied
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        internal List<Position> GetModelAccount(String account)
        {
            try
            {
                List<Position> model = new List<Position>();
                // get long securities
                model.AddRange(PositionCache.GetInstance().GetUnderlyingPositionsFilteredByRule(account, RuleCache.GetInstance().GetLongRule(), Side.Long, ReplacementMatrix.GetInstance().Get(), !Preference.GetInstance().ExcludeNakedSecurities));

                // get short securitis
                model.AddRange(PositionCache.GetInstance().GetUnderlyingPositionsFilteredByRule(account, RuleCache.GetInstance().GetShortRule(), Side.Short, ReplacementMatrix.GetInstance().Get(), !Preference.GetInstance().ExcludeNakedSecurities));

                return model;
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
            return null;
        }

        /// <summary>
        /// returns the positions in a account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        internal List<Position> GetAccountPositions(String account)
        {
            try
            {
                return PositionCache.GetInstance().GetUnderlyingPositionsForAccount(account);
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
            return null;
        }

        /// <summary>
        /// Returns orders that will rebalance the account with the new NAV
        /// </summary>
        /// <param name="modelAccount"></param>
        /// <param name="destAccount"></param>
        /// <param name="newNav"></param>
        /// <returns></returns>
        internal List<Trade> Rebalance(String modelAccount, String destAccount, Double modelNav, Double newNav, Double oldNav)
        {
            try
            {
                Rule longRule = RuleCache.GetInstance().GetLongRule();
                Rule shortRule = RuleCache.GetInstance().GetShortRule();

                Double longFactor = longRule.BookSizeFactor;
                Double shortFactor = shortRule.BookSizeFactor;

                Double longlimit = !longRule.LimitDestination || longRule.DestinationLimit == 0 ? Double.MaxValue : longRule.DestinationLimit;
                Double shortLimit = !shortRule.LimitDestination || shortRule.DestinationLimit == 0 ? Double.MaxValue : shortRule.DestinationLimit;

                return Calculator.Rebalance(GetModelAccount(modelAccount), PositionCache.GetInstance().GetUnderlyingPositionsForAccount(destAccount), modelNav, oldNav, newNav, longFactor, shortFactor, longlimit, shortLimit);
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
            return null;
        }

        /// <summary>
        /// Gets the Nav for the account and master fund as per the prefrence
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        internal Double GetNav(String account)
        {
            if (Preference.GetInstance().ModelPrefrence == ModelPrefrence.Account)
                return Account.GetInstance().GetNAV(account);
            else
                return MasterFund.GetInstance().GetNAV(account);
        }
    }
}
