using Newtonsoft.Json;
using Prana.BusinessLogic;
using Prana.CalculationService.CacheStore;
using Prana.CalculationService.Constants;
using Prana.CalculationService.Models;
using Prana.KafkaWrapper;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Prana.Global.Utilities;
using System.Data;
using Prana.BusinessObjects;
using Task = System.Threading.Tasks.Task;
using Prana.KafkaWrapper.Extension.Classes;

namespace Prana.CalculationService
{
    public class RtpnlLayoutManager
    {
        #region Constants
        public const string CONST_SAMSARA_PREFERENCES = @"SamsaraPreferences\";
        public const string CONST_RTPNL_VIEW_LAYOUT = "RtpnlViewLayout";
        public const string CONST_TXT = ".txt";
        public const string CONST_PATH_SEPARATOR = "\\";
        public const string CONST_ALL = "*";
        public const string CONST_BLANK = "";
        public const string CONST_RTPNL = "RTPNL";
        public const string CONST_VIEWS_WITH_DESCRIPTION = "ViewsWithDescription";
        public const string CONST_VIEWS_WITH_LAYOUT = "ViewsWithLayout";
        public const string CONST_TITLE = "title";
        public const string CONST_OLD_TITLE = "oldTitle";
        public const string CONST_DESCRIPTION = "description";
        public const string CONST_LAYOUT = "layout";
        public const string CONST_PAGEID = "pageId";
        public const string CONST_PAGENAME = "pageName";
        public const string CONST_OLDVIEWNAME = "oldViewName";
        public const string CONST_OLDPAGENAME = "oldPageName";
        public const string CONST_PAGE_LAYOUT = "pageLayout";
        public const string CONST_PAGE_TAG = "pageTag";
        public const string CONST_RTPNL_PAGE_LAYOUT = "RtpnlPageLayout";
        #endregion

        #region Private variables

        Dictionary<int, List<InternalPageInfo>> _userWiseViewsInfo = new Dictionary<int, List<InternalPageInfo>>();
        Dictionary<int, List<OpenFinPageInfo>> _userWisePageInfo = new Dictionary<int, List<OpenFinPageInfo>>();
        #endregion

        #region Public Properties
        public Dictionary<int, List<InternalPageInfo>> UserWiseViewsInfo
        {
            get { return _userWiseViewsInfo; }
        }

        #endregion
        #region SingletonInstance

        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// The singilton instance
        /// </summary>
        private static RtpnlLayoutManager _rtpnlLayoutManager = null;
        /// <summary>
        /// Singilton instance
        /// </summary>
        /// <returns></returns>
        public static RtpnlLayoutManager GetInstance()
        {
            lock (_lock)
            {
                if (_rtpnlLayoutManager == null)
                    _rtpnlLayoutManager = new RtpnlLayoutManager();
                return _rtpnlLayoutManager;
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        private RtpnlLayoutManager()
        {
            try
            {
                _dbManager = DbManager.GetInstance();
                #region SubscribeAndConsume
                //KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_LoadViewsRequest, KafkaManager_LoadRtpnlViewsRequest);
                //KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_SaveOrUpdateViewsRequest, KafkaManager_SaveOrUpdateRtpnlViewsRequest);
                //KafkaManager.Instance.SubscribeAndConsume(KafkaConstants.TOPIC_DeleteOpenfinPageRequest, KafkaManager_DeletertpnlPageInformation);
                #endregion
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
        /// /// Loads RTPNL views for logged in User from database and adds in local cache
        /// </summary>
        /// <param name="companyUserID"></param>
        internal void LoadRtpnlViewsForLoggedInUser(int companyUserID)
        {
            try
            {
                Dictionary<string, string> viewsInfo = FileAndDbSyncManager.SyncFileWithDataBase(CONST_SAMSARA_PREFERENCES, companyUserID, true, CONST_RTPNL);

                List<InternalPageInfo> internalPageInfoResponse = _dbManager.GetInternalPageInfo(companyUserID, Seperators.SEPERATOR_5, Seperators.SEPERATOR_6, "RTPNL");
                if (internalPageInfoResponse != null && _userWiseViewsInfo != null && !_userWiseViewsInfo.ContainsKey(companyUserID))
                    _userWiseViewsInfo.Add(companyUserID, internalPageInfoResponse);

                List<OpenFinPageInfo> openFinPageInfoResponse = _dbManager.GetOpenfinPageInfo(companyUserID);
                if (openFinPageInfoResponse != null && _userWisePageInfo != null && !_userWisePageInfo.ContainsKey(companyUserID))
                    _userWisePageInfo.Add(companyUserID, openFinPageInfoResponse);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// /// Remove RTPNL views for logged out User from local cache
        /// </summary>
        /// <param name="companyUserID"></param>
        internal void RemoveRtpnlViewsForLoggedInUser(int companyUserID)
        {
            try
            {
                if (_userWiseViewsInfo.ContainsKey(companyUserID))
                {
                    _userWisePageInfo[companyUserID].Clear();
                    _userWiseViewsInfo.Remove(companyUserID);
                }
                if (_userWisePageInfo.ContainsKey(companyUserID))
                {
                    _userWisePageInfo[companyUserID].Clear();
                    _userWisePageInfo.Remove(companyUserID);
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
        /// KafkaManager_LoadRtpnlViewsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_LoadRtpnlViewsRequest(string topic, RequestResponseModel message)
        {
            try
            {
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_LoadViewsRequestedBy + message.CompanyUserID);
                Logger.LoggerWrite(RtpnlConstants.MSG_LoadViewsRequestedBy + message.CompanyUserID + JsonConvert.SerializeObject(message));
                int companyUserId = message.CompanyUserID;
                LoadRtpnlLayout loadRtpnlLayout = JsonConvert.DeserializeObject<LoadRtpnlLayout>(message.Data);
                Dictionary<string, string> viewLayouts = new Dictionary<string, string>();
                string rtpnlPreferencePath = AppContext.BaseDirectory + CONST_SAMSARA_PREFERENCES + companyUserId;
                if (Directory.Exists(rtpnlPreferencePath))
                {
                    DirectoryInfo info = new DirectoryInfo(rtpnlPreferencePath);
                    FileInfo[] files = info.GetFiles(CONST_ALL + CONST_RTPNL_VIEW_LAYOUT + CONST_TXT).OrderBy(p => p.CreationTime).ToArray();
                    foreach (FileInfo file in files)
                    {
                        string tabKey = Path.GetFileName(file.Name).Replace(CONST_RTPNL_VIEW_LAYOUT + CONST_TXT, CONST_BLANK);
                        string gridLayout = File.ReadAllText(file.FullName);
                        if (gridLayout != null)
                            viewLayouts.Add(tabKey, gridLayout);
                    }
                }

                if ((string.IsNullOrEmpty(loadRtpnlLayout.pageId) && string.IsNullOrEmpty(loadRtpnlLayout.viewName)) || !string.IsNullOrEmpty(loadRtpnlLayout.pageId))
                {
                    List<RtpnlLayoutInfo> RtpnlLayoutInfoResponse = new List<RtpnlLayoutInfo>();

                    if (_userWisePageInfo.TryGetValue(message.CompanyUserID, out var userPageInfo))
                    {
                        foreach (var openFinPage in userPageInfo)
                        {
                            var rtpnlLayoutInfo = new RtpnlLayoutInfo
                            {
                                pageInfo = openFinPage,
                                internalPageInfo = new List<InternalPageInfo>()
                            };

                            if (_userWiseViewsInfo.TryGetValue(message.CompanyUserID, out var userViewsInfo))
                            {
                                rtpnlLayoutInfo.internalPageInfo.AddRange(userViewsInfo.Where(internalPage => internalPage.pageId == openFinPage.pageId));
                            }

                            RtpnlLayoutInfoResponse.Add(rtpnlLayoutInfo);
                        }
                    }

                    if (string.IsNullOrEmpty(loadRtpnlLayout.pageId) && string.IsNullOrEmpty(loadRtpnlLayout.viewName)) message.Data = JsonConvert.SerializeObject(RtpnlLayoutInfoResponse);
                    else
                    {
                        List<RtpnlLayoutInfo> filteredResponse = new List<RtpnlLayoutInfo>();
                        if (!string.IsNullOrEmpty(loadRtpnlLayout.pageId))
                        {
                            filteredResponse = RtpnlLayoutInfoResponse.Where(info => info.pageInfo.pageId == loadRtpnlLayout.pageId).ToList();
                            if (!string.IsNullOrEmpty(loadRtpnlLayout.viewName))
                            {
                                foreach (var response in filteredResponse)
                                {
                                    response.internalPageInfo = response.internalPageInfo.Where(internalInfo => internalInfo.title == loadRtpnlLayout.viewName).ToList();
                                }
                            }
                        }
                        message.Data = JsonConvert.SerializeObject(filteredResponse);
                    }
                }
                else if (!string.IsNullOrEmpty(loadRtpnlLayout.viewName))
                {
                    InternalPageInfo filteredView = _userWiseViewsInfo.Values.SelectMany(list => list.Where(info => info.title == loadRtpnlLayout.viewName)).FirstOrDefault();
                    message.Data = JsonConvert.SerializeObject(filteredView);
                }
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_LoadViewsResponse, message);

                Logger.LoggerWrite(RtpnlConstants.MSG_LoadViewsResponseFor + message.CompanyUserID + JsonConvert.SerializeObject(message));
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_LoadViewsResponseFor + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// KafkaManager_SaveOrUpdateRtpnlViewsRequest
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_SaveOrUpdateRtpnlViewsRequest(string topic, RequestResponseModel message)
        {
            InformationReporter.GetInstance.Write(RtpnlConstants.MSG_SaveViewsRequestedBy + message.CompanyUserID);
            Logger.LoggerWrite(RtpnlConstants.MSG_SaveViewsRequestedBy + message.CompanyUserID);
            string errorMessage = string.Empty;
            string pageName = string.Empty;
            string oldLayoutFile = string.Empty;
            string oldFileName = string.Empty;
            try
            {
                dynamic layoutDetails = JsonConvert.DeserializeObject<RtpnlLayoutInfo>(message.Data);
                if (layoutDetails != null)
                {
                    var pageInfo = layoutDetails.pageInfo;
                    var internalPageInfoList = layoutDetails.internalPageInfo;

                    if (pageInfo != null)
                    {
                        pageName = pageInfo.pageName ?? string.Empty;
                        string oldPageName = pageInfo.oldPageName ?? string.Empty;
                        string pageTag = pageInfo.pageTag ?? string.Empty;
                        string pageLayout = pageInfo.pageLayout ?? string.Empty;
                        string pageId = pageInfo.pageId ?? string.Empty;
                        int companyUserId = message.CompanyUserID;

                        if (string.IsNullOrEmpty(pageName) || string.IsNullOrEmpty(pageId))
                            errorMessage = RtpnlConstants.MSG_ErrorForSaveLayout;

                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            string startupPath = AppContext.BaseDirectory + CONST_SAMSARA_PREFERENCES + companyUserId;
                            if (!Directory.Exists(startupPath))
                            {
                                Directory.CreateDirectory(startupPath);
                            }
                            try
                            {
                                int rowsAffected = _dbManager.SaveUpdateOpenfinPageInfo(companyUserId, pageInfo);
                                if (rowsAffected > 0)
                                {
                                    if (!_userWisePageInfo.ContainsKey(companyUserId))
                                        _userWisePageInfo.Add(companyUserId, new List<OpenFinPageInfo>());

                                    if (_userWisePageInfo != null && _userWisePageInfo.ContainsKey(companyUserId))
                                    {
                                        List<OpenFinPageInfo> openFinPageInfo = _userWisePageInfo[companyUserId].ToList();
                                        var existingResponse = openFinPageInfo.FirstOrDefault(r => r.pageName.Equals(pageInfo.oldPageName));
                                        if (existingResponse != null)
                                        {
                                            existingResponse.pageName = pageInfo.pageName;
                                            existingResponse.pageLayout = pageInfo.pageLayout;
                                            existingResponse.pageTag = pageInfo.pageTag;
                                            existingResponse.pageId = pageInfo.pageId;
                                        }
                                        else
                                        {
                                            var newResponse = new OpenFinPageInfo
                                            {
                                                pageName = pageInfo.pageName,
                                                pageLayout = pageInfo.pageLayout,
                                                pageTag = pageInfo.pageTag,
                                                pageId = pageInfo.pageId,
                                            };

                                            openFinPageInfo.Add(newResponse);
                                        }
                                        _userWisePageInfo[companyUserId] = openFinPageInfo;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                errorMessage = ex.Message;
                            }

                        }
                    }
                    if (internalPageInfoList != null)
                    {
                        foreach (var internalPageInfo in internalPageInfoList)
                        {
                            string title = internalPageInfo.title ?? string.Empty;
                            string oldTitle = internalPageInfo.oldTitle ?? string.Empty;
                            string description = internalPageInfo.description ?? string.Empty;
                            string layout = internalPageInfo.layout ?? string.Empty;
                            string oldViewName = internalPageInfo.oldViewName ?? string.Empty;
                            string pageId = internalPageInfo.pageId ?? string.Empty;
                            int companyUserId = message.CompanyUserID;
                            string viewId = internalPageInfo.viewId ?? string.Empty;

                            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(pageId))
                                errorMessage = RtpnlConstants.MSG_ErrorForSaveLayout;

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                string startupPath = AppContext.BaseDirectory + CONST_SAMSARA_PREFERENCES + companyUserId;
                                if (!Directory.Exists(startupPath))
                                {
                                    Directory.CreateDirectory(startupPath);
                                }
                                try
                                {
                                    if (!string.IsNullOrEmpty(oldTitle) || !string.IsNullOrEmpty(pageInfo.oldPageName))
                                    {
                                        if (string.IsNullOrEmpty(oldTitle))
                                        {
                                            oldFileName = title + CONST_RTPNL_VIEW_LAYOUT + pageInfo.oldPageName + CONST_TXT;
                                            oldLayoutFile = startupPath + CONST_PATH_SEPARATOR + title + CONST_RTPNL_VIEW_LAYOUT + pageInfo.oldPageName + CONST_TXT;
                                        }
                                        else
                                        {
                                            oldFileName = oldTitle + CONST_RTPNL_VIEW_LAYOUT + pageInfo.oldPageName + CONST_TXT;
                                            oldLayoutFile = startupPath + CONST_PATH_SEPARATOR + oldTitle + CONST_RTPNL_VIEW_LAYOUT + pageInfo.oldPageName + CONST_TXT;
                                        }
                                        FileAndDbSyncManager.DeleteCompanyUserLayout(companyUserId, oldFileName);
                                        if (File.Exists(oldLayoutFile))
                                            File.Delete(oldLayoutFile);
                                    }

                                    string fileName = title + CONST_RTPNL_VIEW_LAYOUT + pageName + CONST_TXT;
                                    string file = startupPath + CONST_PATH_SEPARATOR + fileName;
                                    TextWriter txt = new StreamWriter(file);
                                    txt.Write(layout);
                                    txt.Close();
                                    FileAndDbSyncManager.SyncDataBaseWithFile(CONST_SAMSARA_PREFERENCES, companyUserId, true, CONST_RTPNL, description, fileName, pageId, viewId, title);

                                    if (_userWiseViewsInfo.TryGetValue(companyUserId, out var userViews))
                                    {
                                        if (!string.IsNullOrEmpty(title))
                                        {
                                            var viewTitle = title;
                                            if (!string.IsNullOrEmpty(oldTitle))
                                                viewTitle = oldTitle;
                                            var oldView = userViews.FirstOrDefault(v => v.title == viewTitle && v.pageId == pageId);

                                            if (oldView != null)
                                            {
                                                userViews.Remove(oldView);
                                            }
                                        }
                                        userViews.Add(internalPageInfo);
                                    }
                                    else
                                    {
                                        var newUserViews = new List<InternalPageInfo>();
                                        newUserViews.Add(internalPageInfo);
                                        _userWiseViewsInfo.Add(companyUserId, newUserViews);
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
                message.Data = JsonConvert.SerializeObject(errorMessage);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_SaveOrUpdateViewsResponse, message);
                Logger.LoggerWrite(RtpnlConstants.MSG_SaveViewsResponseFor + message.CompanyUserID + JsonConvert.SerializeObject(message));
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_SaveViewsResponseFor + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Remove layouts for logged out user 
        /// </summary>
        /// <param name="companyUserID"></param>
        internal void RemoveLayoutsForLoggedOutUser(int companyUserID)
        {
            try
            {
                FileAndDbSyncManager.RemoveUserLayoutsFromPrefFolder(CONST_SAMSARA_PREFERENCES, companyUserID);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Delete rtpnl page information  
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private async void KafkaManager_DeletertpnlPageInformation(string topic, RequestResponseModel message)
        {
            try
            {
                Logger.LoggerWrite(RtpnlConstants.MSG_DeleteOpenfinPageInformationReceived + message.CompanyUserID + JsonConvert.SerializeObject(message));
                string errorMessage = string.Empty;
                dynamic jsonDataObject = JsonConvert.DeserializeObject<dynamic>(message.Data);
                try
                {
                    object[] parameters = new object[2];
                    parameters[0] = message.CompanyUserID;
                    parameters[1] = jsonDataObject.pageId;

                    List<OpenFinPageInfo> OpenfinPageInfoList = _userWisePageInfo[message.CompanyUserID];
                    // Deleting the Page information from DB
                    DatabaseManager.DatabaseManager.ExecuteNonQuery(RtpnlConstants.CONST_P_Samsara_DeleteOpenfinPage, parameters);
                    Logger.LoggerWrite(RtpnlConstants.MSG_DeleteOpenfinPageInformationReceived + message.CompanyUserID + "::" + jsonDataObject.pageId);
                    if (_userWisePageInfo.ContainsKey(message.CompanyUserID))
                    {
                        // Deleting the Page information from cache
                        foreach (var key in _userWisePageInfo.ToList())
                        {
                            string jsonDataPageId = (string)jsonDataObject.pageId;
                            var pageInfoList = key.Value;
                            pageInfoList.RemoveAll(pageInfo => pageInfo.pageId == jsonDataPageId);

                            if (pageInfoList.Count == 0)
                                _userWisePageInfo.Remove(key.Key);
                        }
                    }
                    if (_userWiseViewsInfo.ContainsKey(message.CompanyUserID))
                    {
                        // Deleting the Views information from cache
                        foreach (var key in _userWiseViewsInfo.ToList())
                        {
                            string jsonDataPageId = (string)jsonDataObject.pageId;
                            var pageInfoList = key.Value;
                            pageInfoList.RemoveAll(pageInfo => pageInfo.pageId == jsonDataPageId);

                            if (pageInfoList.Count == 0)
                                _userWiseViewsInfo.Remove(key.Key);
                        }
                    }
                    Logger.LoggerWrite(RtpnlConstants.MSG_CacheUpdatedWithData + message.CompanyUserID + JsonConvert.SerializeObject(message));

                }
                catch (Exception ex1)
                {
                    errorMessage = ex1.Message;
                    Logger.LoggerWrite(RtpnlConstants.MSG_ExceptionThrown + JsonConvert.SerializeObject(ex1));
                }
                message.Data = JsonHelper.SerializeObject(errorMessage);
                await KafkaManager.Instance.Produce(KafkaConstants.TOPIC_DeleteOpenfinPageResponse, message);
                InformationReporter.GetInstance.Write(RtpnlConstants.MSG_DeleteOpenfinPageInformationProcessed + message.CompanyUserID);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
    }
}
