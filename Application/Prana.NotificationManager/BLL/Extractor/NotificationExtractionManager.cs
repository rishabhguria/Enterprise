using Prana.BusinessObjects.Compliance.Alerting;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.NotificationManager.BLL.Extractor
{
    internal class NotificationExtractionManager
    {

        #region singletonInstance
        private static NotificationExtractionManager _notificationExtractionManager;
        private static object _notificationExtractionManagerLocker = new Object();

        internal static NotificationExtractionManager GetInstance()
        {
            lock (_notificationExtractionManagerLocker)
            {
                if (_notificationExtractionManager == null)
                    _notificationExtractionManager = new NotificationExtractionManager();
                return _notificationExtractionManager;
            }
        }

        private NotificationExtractionManager()
        {

        }


        #endregion

        /// <summary>
        /// Initialize extractor.
        /// </summary>
        internal void Initialize()
        {
            try
            {
                LoadExtractorCache();
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
        /// Loads instance and initializes cache.
        /// </summary>
        private void LoadExtractorCache()
        {
            try
            {
                lock (_extractorCache)
                {
                    if (!_extractorCache.ContainsKey(StrategyBehavior.Default))
                        _extractorCache.Add(StrategyBehavior.Default, new DefaultExtractor());
                    if (!_extractorCache.ContainsKey(StrategyBehavior.Group))
                        _extractorCache.Add(StrategyBehavior.Group, new GroupNotificationExtractor());
                }
                GetHandlerForExtractor(StrategyBehavior.Default).InitializeCache();
                GetHandlerForExtractor(StrategyBehavior.Group).InitializeCache();
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
        /// If rule triggered is in grouop then send to group extractor else to default.
        /// </summary>
        /// <param name="alert"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        internal NotificationSetting Extract(Alert alert, RuleBase rule)
        {
            try
            {
                if (rule.GroupId == "-1")
                {
                    return GetHandlerForExtractor(StrategyBehavior.Default).Extract(alert, rule);
                }
                else
                {
                    return GetHandlerForExtractor(StrategyBehavior.Group).Extract(alert, rule);
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
                return null;
            }
            //NotificationSetting setting;
            //bool isExists = GetHandlerForExtractor(StrategyBehavior.Default).Extract(rule, out setting);
            //if (!isExists)
            //    GetHandlerForExtractor(StrategyBehavior.Group).Extract(rule, out setting);

            //return setting;
        }



        Dictionary<StrategyBehavior, INotificationExtractor> _extractorCache = new Dictionary<StrategyBehavior, INotificationExtractor>();

        /// <summary>
        /// returns extractor for strategy.
        /// </summary>
        /// <param name="behavior"></param>
        /// <returns></returns>
        private INotificationExtractor GetHandlerForExtractor(StrategyBehavior behavior)
        {
            try
            {
                lock (_extractorCache)
                {
                    if (_extractorCache.ContainsKey(behavior))
                        return _extractorCache[behavior];
                    else
                        throw new KeyNotFoundException("Behavior key not found");
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
                return null;
            }
        }


        /// <summary>
        /// Dispioses extractor
        /// </summary>
        internal void Dispose()
        {
            try
            {
                GetHandlerForExtractor(StrategyBehavior.Default).CallDispose();
                GetHandlerForExtractor(StrategyBehavior.Group).CallDispose();
                _extractorCache = null;
                _notificationExtractionManager = null;
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
