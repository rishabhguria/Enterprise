using Prana.CalculationService.Constants;
using Prana.CalculationService.Models;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.CalculationService.CacheStore
{
    internal class ConfigurationDetails
    {
        /// <summary>
        /// Cache for Users configuration details
        /// </summary>
        private static Dictionary<int, List<RtpnlResponse>> _usersConfigurationDetails = new Dictionary<int, List<RtpnlResponse>>();
        public Dictionary<int, List<RtpnlResponse>> UsersConfigurationDetails
        {
            get { return _usersConfigurationDetails; }
        }

        /// <summary>
        /// Cache for Users widget counts with type
        /// </summary>
        Dictionary<string, int> widgetCountsWithType = new Dictionary<string, int>();

        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static ConfigurationDetails _configurationDetails = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static ConfigurationDetails GetInstance()
        {
            lock (_lock)
            {
                if (_configurationDetails == null)
                    _configurationDetails = new ConfigurationDetails();
                return _configurationDetails;
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        private ConfigurationDetails()
        {
            try
            {
                _dbManager = DbManager.GetInstance();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Instance for DbManager
        /// </summary>
        private static DbManager _dbManager;


        /// <summary>
        /// Loads the configuration details of given UserID
        /// </summary>
        /// <param name="UserId"></param>
        internal void LoadConfigurationDetailsForUser(int UserId)
        {
            try
            {
                List<RtpnlResponse> rtpnlResponse = _dbManager.GetWidgetData(UserId);
                if (rtpnlResponse != null && UsersConfigurationDetails != null && !UsersConfigurationDetails.ContainsKey(UserId))
                    UsersConfigurationDetails.Add(UserId, rtpnlResponse);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Removes the configuration details of given UserID
        /// </summary>
        /// <param name="UserId"></param>
        internal void RemoveConfigurationDetailsForUser(int UserId)
        {
            try
            {
                if (UsersConfigurationDetails != null && UsersConfigurationDetails.ContainsKey(UserId))
                {
                    UsersConfigurationDetails[UserId].Clear();
                    UsersConfigurationDetails.Remove(UserId);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the configuration details for given UserID and WidgetType
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="WidgetType"></param>
        internal List<RtpnlResponse> GetConfigDetailsForUserAndWidgetType(int UserId, string WidgetType)
        {
            List<RtpnlResponse> response = null;
            try
            {
                if (UsersConfigurationDetails != null && UsersConfigurationDetails.ContainsKey(UserId))
                    response = UsersConfigurationDetails[UserId].Where(d => d.widgetType.Equals(WidgetType)).ToList();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return response;
        }
        /// <summary>
        /// Save and Update the configuration details of given UserID
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="data"></param>
        internal void SaveUpdateConfigurationDetailsForUser(int UserId, WidgetData data, List<string> oldWidgetIds = null )
        {
            try
            {
                if (oldWidgetIds != null)
                {
                    // This block is for Save Page As operation
                    List<RtpnlResponse> configurationDetails = UsersConfigurationDetails[UserId].ToList();
                    List<RtpnlResponse> allOldWidgets = configurationDetails.Where(detail => oldWidgetIds.Contains(detail.widgetId)).ToList();
                    List<RtpnlResponse> oldWidgetsInView = allOldWidgets.Where(r => r.viewName == data.viewName).ToList();

                    RtpnlResponse widget = oldWidgetsInView.FirstOrDefault(r => r.widgetName == data.widgetName);

                    if (data != null && widget != null)
                    {
                        data.defaultColumns = widget.defaultColumns;
                        data.primaryMetric = widget.primaryMetric;
                        data.linkedWidget = widget.linkedWidget;
                        data.channelDetail = widget.channelDetail;
                        data.isFlashColorEnabled = widget.isFlashColorEnabled;
                        data.graphType = widget.graphType;
                        data.coloredColumns = widget.coloredColumns;
                    }
                }
                int rowsAffected = _dbManager.SaveUpdateWidgetData(UserId, data);
                if (rowsAffected > 0)
                {
                    if (UsersConfigurationDetails != null && UsersConfigurationDetails.ContainsKey(UserId))
                    {
                        List<RtpnlResponse> configurationDetails = UsersConfigurationDetails[UserId].ToList();
                        var existingResponse = configurationDetails.FirstOrDefault(r => r.widgetId.Equals(data.widgetId));
                        if (existingResponse != null)
                        {
                            existingResponse.pageId = data.pageId;
                            existingResponse.viewName = data.viewName;
                            existingResponse.widgetName = data.widgetName;
                            existingResponse.widgetType = data.widgetType;
                            existingResponse.defaultColumns = data.defaultColumns;
                            existingResponse.coloredColumns = data.coloredColumns;
                            existingResponse.graphType = data.graphType;
                            existingResponse.isFlashColorEnabled = data.isFlashColorEnabled;
                            existingResponse.channelDetail = data.channelDetail;
                            existingResponse.linkedWidget = data.linkedWidget;
                            existingResponse.widgetId = data.widgetId;
                            existingResponse.primaryMetric = data.primaryMetric;
                        }
                        else
                        {
                            var newResponse = new RtpnlResponse
                            {
                                pageId = data.pageId,
                                viewName = data.viewName,
                                widgetName = data.widgetName,
                                widgetType = data.widgetType,
                                defaultColumns = data.defaultColumns,
                                coloredColumns = data.coloredColumns,
                                graphType = data.graphType,
                                isFlashColorEnabled = data.isFlashColorEnabled,
                                channelDetail = data.channelDetail,
                                linkedWidget = data.linkedWidget,
                                widgetId = data.widgetId,
                                primaryMetric = data.primaryMetric
                            };

                            configurationDetails.Add(newResponse);
                        }
                        UsersConfigurationDetails[UserId] = configurationDetails;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets widget count for given UserID
        /// </summary>
        /// <param name="UserId"></param>
        internal Dictionary<string, int> GetWidgetCountForUser(int UserId)
        {
            try
            {
                if (UsersConfigurationDetails != null && UsersConfigurationDetails.ContainsKey(UserId))
                {
                    widgetCountsWithType.Clear();
                    foreach (var configDetail in UsersConfigurationDetails[UserId])
                    {
                        if (configDetail != null)
                        {
                            string widgetType = configDetail.widgetType;

                            if (!widgetCountsWithType.ContainsKey(widgetType))
                                widgetCountsWithType[widgetType] = 1;
                            else
                                widgetCountsWithType[widgetType]++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return widgetCountsWithType;
        }

        /// <summary>
        /// Gets the configuration details for given UserID 
        /// </summary>
        /// <param name="UserId"></param>
        internal List<RtpnlResponse> GetConfigDetailsForUser(int UserId, string PageId)
        {
            List<RtpnlResponse> response = null;
            try
            {
                if (UsersConfigurationDetails != null && UsersConfigurationDetails.ContainsKey(UserId))
                {
                    response = UsersConfigurationDetails[UserId]
                        .Where(config => string.IsNullOrEmpty(PageId) || config.pageId == PageId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return response;
        }

        /// <summary>
        /// Check Duplicate widget name
        /// </summary>
        /// <param name="name"></param>
        public bool IsWidgetNameDuplicate(int userId, WidgetData data)
        {
            try
            {
                if (_usersConfigurationDetails != null && _usersConfigurationDetails.ContainsKey(userId))
                {
                    List<RtpnlResponse> configurationDetails = _usersConfigurationDetails[userId];

                    foreach (var widget in configurationDetails)
                    {
                        if (!widget.widgetId.Equals(data.widgetId))
                        {
                            if (widget.widgetName.Equals(data.widgetName, StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }
    }
}