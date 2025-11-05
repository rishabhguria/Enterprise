using Newtonsoft.Json;
using Prana.Global.Utilities;
using Prana.KafkaWrapper;
using Prana.KafkaWrapper.Extension.Classes;
using Prana.LayoutService.Contracts;
using Prana.LayoutService.Layout_Managers.Modules;
using Prana.LayoutService.Models;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.LayoutService.Layout_Managers
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class ViewLayoutManager : ILayoutManager
    {
        #region variables
        /// <summary>
        /// User Wise Dictionary for managing View Level Layout
        /// </summary>
        private static Dictionary<int, Dictionary<string, ViewInfo>> _userWiseViewLayoutInfo = new Dictionary<int, Dictionary<string, ViewInfo>>();


        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static ViewLayoutManager _viewLayoutManager = null;

        /// <summary>
        /// Instance for DbManager
        /// </summary>
        private static DbManager _dbManager;

        /// <summary>
        /// Instance for Blotter Module Manager
        /// </summary>
        private static BlotterLayoutManager _blotterLayoutManager;

        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <returns>ViewLayoutManager object</returns>
        public static ViewLayoutManager GetInstance()
        {
            lock (_lock)
            {
                if (_viewLayoutManager == null)
                    _viewLayoutManager = new ViewLayoutManager();
                return _viewLayoutManager;
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        private ViewLayoutManager()
        {
            try
            {
                //Getting the instance of dbManager . 
                _dbManager = DbManager.GetInstance();

                //Getting the instance of blotterManager .
                _blotterLayoutManager = new BlotterLayoutManager();

                //methods to get and save view specific data
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveLayoutRequest, KafkaManager_SaveViewLayoutRequest);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_LoadLayoutRequest, KafkaManager_LoadViewLayoutRequest);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_RemoveBlotterCustomTabRequest, KafkaManager_RemoveBlotterCustomTabRequest);
                KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_RenameBlotterCustomTabRequest, KafkaManager_RenameBlotterCustomTabRequest);
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
        #endregion

        /// <summary>
        /// Loading the View's for the userID
        /// </summary>
        /// <param name="companyUserId"></param>
        public void LoadLayoutForLoggedInUser(int companyUserId)
        {
            try
            {
                Dictionary<string, ViewInfo> viewInfoResponse = _dbManager.GetViewInfo(companyUserId);
                if (viewInfoResponse != null)
                {
                    if (_userWiseViewLayoutInfo.ContainsKey(companyUserId))
                    {
                        // Update the existing entry
                        foreach (var kvp in viewInfoResponse)
                        {
                            _userWiseViewLayoutInfo[companyUserId][kvp.Key] = kvp.Value;
                        }
                    }
                    else
                    {
                        // Add a new entry
                        _userWiseViewLayoutInfo[companyUserId] = new Dictionary<string, ViewInfo>(viewInfoResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// remove the view Info from the cache dictionary
        /// </summary>
        /// <param name="companyUserId"></param>
        public void RemoveLayoutForLoggedOutUser(int companyUserId)
        {
            try
            {
                if (_userWiseViewLayoutInfo.ContainsKey(companyUserId))
                {
                    _userWiseViewLayoutInfo[companyUserId].Clear();
                    _userWiseViewLayoutInfo.Remove(companyUserId);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// returns the user view based on module 
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>    
        public async void KafkaManager_LoadViewLayoutRequest(string topic, RequestResponseModel message)
        {
            try
            {
                InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_LoadViewsRequestedBy + message.CompanyUserID);
                Logger.LoggerWrite(LayoutServiceConstants.MSG_LoadViewsRequestedBy + message.CompanyUserID + JsonConvert.SerializeObject(message));

                dynamic info = JsonConvert.DeserializeObject<dynamic>(message.Data);
                string moduleName = info.moduleName;
                int companyUserId = message.CompanyUserID;
                // Getting the users view details.
                if (_userWiseViewLayoutInfo != null && _userWiseViewLayoutInfo.ContainsKey(companyUserId))
                {
                    Dictionary<string, ViewInfo> dictLayoutsForUser = _userWiseViewLayoutInfo[companyUserId];
                    // Creating a list of layouts that match the moduleName
                    List<ViewInfo> filteredLayoutsForUser = new List<ViewInfo>();
                    foreach (var viewInfo in dictLayoutsForUser.Values)
                    {
                        if (viewInfo.moduleName == moduleName)
                        {
                            filteredLayoutsForUser.Add(viewInfo);
                        }
                    }

                    var data = new { layoutData = filteredLayoutsForUser, moduleId = info.moduleId };
                    // Serializing the filtered list to send
                    message.Data = JsonHelper.SerializeObject(data);
                }
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_LoadLayoutResponse, message);
                Logger.LoggerWrite(LayoutServiceConstants.MSG_GetApplicationLayoutProcessed + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_LoadLayoutResponse);
            }

        }

        // <summary>
        /// Loads Views for logged in User from cache . This method will only take companyUser ID and return all the views for the user
        /// </summary>
        /// <param name="companyUserID"></param>
        /// TODO : introduce module name and return data according to module
        public Dictionary<string, ViewInfo> GetUserSpecificViewLayout(int companyUserId)
        {
            Dictionary<string, ViewInfo> userViews = new Dictionary<string, ViewInfo>();
            try
            {
                // Return the user's view layout if it exists, otherwise an empty dictionary
                if (_userWiseViewLayoutInfo.TryGetValue(companyUserId, out userViews))
                {
                    return userViews;
                }

                return new Dictionary<string, ViewInfo>();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return new Dictionary<string, ViewInfo>();
            }
        }


        /// <summary>
        /// Method responsible for Saving View Level layouts
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        public async void KafkaManager_SaveViewLayoutRequest(string topic, RequestResponseModel message)
        {
            try
            {
                InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_SaveViewsRequestedBy + message.CompanyUserID);
                Logger.LoggerWrite(LayoutServiceConstants.MSG_SaveViewsRequestedBy + message.CompanyUserID + JsonConvert.SerializeObject(message));

                string errorMessage = string.Empty;
                string menuItem = string.Empty;
                string viewLayout = string.Empty;
                string viewId = string.Empty;
                try
                {
                    String updatedViewLayout = String.Empty;
                    ViewInfo viewInfo = JsonConvert.DeserializeObject<ViewInfo>(message.Data);
                    menuItem = viewInfo.menuItem;
                    viewId = viewInfo.viewId;

                    if (menuItem == LayoutServiceConstants.CONST_CUSTOM_TAB_CREATED)
                    {
                        viewLayout = viewInfo.viewLayout;
                    }

                    if (viewInfo != null)
                    {
                        //Case when when Module is Rtpnl to save Grouping Widget Arranged and Favorite Chips List
                        if (viewInfo.moduleName.Equals(LayoutServiceConstants.CONST_MODULE_RTPNL, StringComparison.InvariantCultureIgnoreCase))
                        {
                            SaveUserSpecificViewLayout(message.CompanyUserID, new List<ViewInfo> { viewInfo });
                            viewLayout = viewInfo.viewLayout;
                        }

                        // Case when Module is Blotter
                        if (viewInfo.moduleName.Equals(LayoutServiceConstants.CONST_MODULE_Blotter, StringComparison.InvariantCultureIgnoreCase))
                        {
                            String centralBlotterLayout = String.Empty;
                            // Blotter view needs to be saved , so calling blotter layout manager for updated layout
                            if (_userWiseViewLayoutInfo.ContainsKey(message.CompanyUserID))
                            {
                                if (_userWiseViewLayoutInfo[message.CompanyUserID].ContainsKey(viewInfo.viewId))
                                {
                                    //blotter already has some layout saved
                                    centralBlotterLayout = _userWiseViewLayoutInfo[message.CompanyUserID][viewInfo.viewId].viewLayout;
                                }
                            }

                            updatedViewLayout = _blotterLayoutManager.UpdateBlotterLayout(centralBlotterLayout, viewInfo.viewLayout);

                            if (_userWiseViewLayoutInfo.ContainsKey(message.CompanyUserID))
                            {
                                if (_userWiseViewLayoutInfo[message.CompanyUserID].ContainsKey(viewInfo.viewId))
                                {                                    
                                    viewInfo.viewLayout = updatedViewLayout;
                                    
                                    
                                }
                            }
                            //Saving the updated viewInfo in database and internal cache
                            SaveUserSpecificViewLayout(message.CompanyUserID, new List<ViewInfo> { viewInfo });
                        }                  
                    }
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }

                var response = new { errorMessage = errorMessage, menuItem = menuItem, viewLayout = viewLayout, viewId = viewId };
                message.Data = JsonConvert.SerializeObject(response);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveLayoutResponse, message);
                Logger.LoggerWrite(LayoutServiceConstants.MSG_SaveViewsResponseFor + message.CompanyUserID + JsonConvert.SerializeObject(message));
                InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_SaveViewsResponseFor + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_SaveLayoutResponse);
            }
        }


        /// <summary>
        /// Saves the views in database as well as the cache . This method will take the userId and viewInfo and save the data 
        /// </summary>
        /// <param name="companyUserId"></param>
        /// <param name="viewInfo"></param>
        /// <returns></returns>
        public string SaveUserSpecificViewLayout(int companyUserId, List<ViewInfo> viewInfoList)
        {
            string errorMessage = String.Empty;
            //TODO : Improve this section of code .
            try
            {
                if (viewInfoList != null && viewInfoList.Count > 0)
                {
                    foreach (var viewInfo in viewInfoList)
                    {
                        if (viewInfo != null)
                        {
                            string viewId = viewInfo.viewId ?? string.Empty;
                            string viewName = viewInfo.viewName ?? string.Empty;
                            string viewLayout = viewInfo.viewLayout ?? string.Empty;
                            string description = viewInfo.description ?? string.Empty;
                            string moduleName = viewInfo.moduleName ?? string.Empty;


                            if (string.IsNullOrEmpty(viewName) || string.IsNullOrEmpty(viewId))
                                errorMessage = LayoutServiceConstants.MSG_ErrorForSaveLayout;

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                try
                                {
                                    int rowsAffected = _dbManager.SaveUpdateViewInfo(companyUserId, viewInfo);
                                    if (rowsAffected > 0)
                                    {
                                        //In case this is the first saved page for the user , so creating a blank dictionary for this userId
                                        if (!_userWiseViewLayoutInfo.ContainsKey(companyUserId))
                                            _userWiseViewLayoutInfo.Add(companyUserId, new Dictionary<string, ViewInfo>());

                                        if (_userWiseViewLayoutInfo != null && _userWiseViewLayoutInfo.ContainsKey(companyUserId))
                                        {
                                            //taking internal dictionary for the userID and now either saving the view or updating the view .
                                            Dictionary<string, ViewInfo> userViews = _userWiseViewLayoutInfo[companyUserId];

                                            if (userViews.ContainsKey(viewId))
                                            {
                                                ViewInfo existingViewInfo = userViews[viewId];
                                                existingViewInfo.viewName = viewInfo.viewName;
                                                existingViewInfo.description = viewInfo.description;
                                                existingViewInfo.viewLayout = viewInfo.viewLayout;
                                                existingViewInfo.moduleName = viewInfo.moduleName;
                                            }
                                            else
                                            {
                                                //Adding a new view info in the dictionary cache
                                                userViews[viewInfo.viewId] = viewInfo;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errorMessage = ex.Message;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return errorMessage;
        }

        public void DeleteUserSpecificViewLayout(int companyUserId, List<string> viewIds)
        {
            try
            {
                // Check if the user exists in the dictionary
                if (_userWiseViewLayoutInfo.TryGetValue(companyUserId, out var userViewLayouts))
                {
                    // Iterate over the list of View IDs
                    foreach (var viewId in viewIds)
                    {
                        // Check if the ViewId exists for the user and remove it
                        if (userViewLayouts.ContainsKey(viewId))
                        {
                            userViewLayouts.Remove(viewId);
                        }
                    }

                    // Optionally, if the user's page layout dictionary is empty, remove the user entry
                    if (userViewLayouts.Count == 0)
                    {
                        _userWiseViewLayoutInfo.Remove(companyUserId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public async void KafkaManager_RemoveBlotterCustomTabRequest(string topic, RequestResponseModel message)
        {
            InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_RemoveCustomTabRequestedBy + message.CompanyUserID);
            string errorMessage = string.Empty;
            try
            {
                //string tabNameToRemove = message.Data;
                BlotterTabInfo blotterTabInfo = JsonConvert.DeserializeObject<BlotterTabInfo>(message.Data);
                String layout = String.Empty;
                ViewInfo viewInfo = null;
                try
                {
                    if (_userWiseViewLayoutInfo.ContainsKey(message.CompanyUserID) && _userWiseViewLayoutInfo[message.CompanyUserID].ContainsKey(blotterTabInfo.viewId))
                    {
                        viewInfo = _userWiseViewLayoutInfo[message.CompanyUserID][blotterTabInfo.viewId];
                        layout = viewInfo.viewLayout;
                    }
                    if (!string.IsNullOrEmpty(layout))
                    {
                        string updatedLayout = _blotterLayoutManager.RemoveBlotterCustomTab(blotterTabInfo.tabName, layout);
                        viewInfo.viewLayout = updatedLayout;

                        //Saving the updated viewInfo in database and internal cache
                        SaveUserSpecificViewLayout(message.CompanyUserID, new List<ViewInfo> { viewInfo });
                    }
                }
                catch (Exception)
                {
                    errorMessage = LayoutServiceConstants.MSG_ErrorFailedToRemoveTab;
                }
                var response = new { errorMessage = errorMessage };
                message.Data = JsonConvert.SerializeObject(response);
                
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse, message);
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_RemoveBlotterCustomTabResponse);
            }
            InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_RemoveCustomTabResponseFor + message.CompanyUserID);
        }

        

        public async void KafkaManager_RenameBlotterCustomTabRequest(string topic, RequestResponseModel message)
        {
            InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_RenameCustomTabRequestedBy + message.CompanyUserID);
            string errorMessage = string.Empty;
            try
            {
                //string tabNameToRemove = message.Data;
                BlotterTabRenameInfo blotterTabRenameInfo = JsonConvert.DeserializeObject<BlotterTabRenameInfo>(message.Data);
                String layout = String.Empty;

                // Access the deserialized data
                string viewId = blotterTabRenameInfo.viewId;
                Dictionary<string, string> tabRenameDetails = blotterTabRenameInfo.customTabsDetails.tabtoRename;

                ViewInfo viewInfo = null;
                try
                {
                    if (_userWiseViewLayoutInfo.ContainsKey(message.CompanyUserID) && _userWiseViewLayoutInfo[message.CompanyUserID].ContainsKey(blotterTabRenameInfo.viewId))
                    {
                        viewInfo = _userWiseViewLayoutInfo[message.CompanyUserID][blotterTabRenameInfo.viewId];
                        layout = viewInfo.viewLayout;
                    }
                    if (!string.IsNullOrEmpty(layout))
                    {
                        string updatedLayout = _blotterLayoutManager.RenameBlotterCustomTab(layout, tabRenameDetails);
                        viewInfo.viewLayout = updatedLayout;

                        //Saving the updated viewInfo in database and internal cache
                        SaveUserSpecificViewLayout(message.CompanyUserID, new List<ViewInfo> { viewInfo });
                    }
                }
                catch (Exception)
                {
                    errorMessage = LayoutServiceConstants.MSG_ErrorFailedToRemoveTab;
                }

                var response = new { errorMessage = errorMessage, tabDetail = tabRenameDetails };
                message.Data = JsonConvert.SerializeObject(response);

                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_RenameBlotterCustomTabResponse, message);
            }
            catch (Exception ex)
            {
                await ProduceTopicNHandleException(message, ex, KafkaConstants.TOPIC_RenameBlotterCustomTabResponse);
            }
            InformationReporter.GetInstance.Write(LayoutServiceConstants.MSG_RenameCustomTabResponseFor + message.CompanyUserID);
        }

        private static async System.Threading.Tasks.Task ProduceTopicNHandleException(
            RequestResponseModel message,
            Exception ex,
            string topicName)
        {
            try
            {
                message.Data = null;
                message.ErrorMsg = $"Error while producing to topic {topicName}, err msg:{ex.Message}";
                await KafkaManager.Instance.Produce(topicName, message);
                Logger.LogError(ex, $"Error while producing to topic {topicName}");
            }
            catch (Exception ex2)
            {
                Logger.LogError(ex2, $"ProduceTopicNHandleException encountered an error,  message might not have been published to event {topicName}");
            }
        }
    }
}
