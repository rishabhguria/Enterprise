using Prana.LayoutService.Contracts;
using Prana.LayoutService.Models;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace Prana.LayoutService.Layout_Managers
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class PageLayoutManager : ILayoutManager
    {
        #region variables
        /// <summary>
        /// User Wise Dictionary for managing Page Level Layout
        /// </summary>
        private static Dictionary<int, Dictionary<string, PageInfo>> _userWisePageLayoutInfo = new Dictionary<int, Dictionary<string, PageInfo>>();


        /// <summary>
        /// Locker object
        /// </summary>
        private static readonly Object _lock = new Object();

        /// <summary>
        /// The singleton instance
        /// </summary>
        private static PageLayoutManager _pageLayoutManager = null;

        /// <summary>
        /// Instance for DbManager
        /// </summary>
        private static DbManager _dbManager;

        /// <summary>
        /// Singleton instance
        /// </summary>
        /// <returns>PageLayoutManager object</returns>
        public static PageLayoutManager GetInstance()
        {
            lock (_lock)
            {
                if (_pageLayoutManager == null)
                    _pageLayoutManager = new PageLayoutManager();
                return _pageLayoutManager;
            }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        private PageLayoutManager()
        {
            try
            {
                //Getting the instance of dbManager . 
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
        #endregion


        /// <summary>
        /// Method to delete the page info from the cache dictionary
        /// </summary>
        /// <param name="companyUserId"></param>
        /// <param name="pageId"></param>
        public void DeleteUserSpecificPageLayout(int companyUserId , string pageId)
        {
            try
            {
                // Check if the user exists in the dictionary
                if (_userWisePageLayoutInfo.TryGetValue(companyUserId, out var userPageLayouts))
                {
                    // Check if the pageId exists for the user and removing the same .
                    if (userPageLayouts.ContainsKey(pageId))
                    {
                        userPageLayouts.Remove(pageId);

                        // Optionally, if the user's page layout dictionary is empty, remove the user entry
                        if (userPageLayouts.Count == 0)
                        {
                            _userWisePageLayoutInfo.Remove(companyUserId);
                        }
                    }                    
                }                
            }
            catch(Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        // <summary>
        /// Loads Pages for logged in User from cache . This method will only take companyUser ID and return all the pages for the user
        /// </summary>
        /// <param name="companyUserID"></param>
        public Dictionary<string, PageInfo> GetUserSpecificPageLayout(int companyUserId)
        {
            Dictionary<string, PageInfo> userPages = new Dictionary<string, PageInfo>();
            try
            {                                
                // Return the user's page layout if it exists, otherwise an empty dictionary
                if (_userWisePageLayoutInfo.TryGetValue(companyUserId, out userPages))
                {
                    return userPages;
                }

                return new Dictionary<string, PageInfo>();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return new Dictionary<string, PageInfo>();
            }
        }

        /// <summary>
        /// Saves the page in database as well as the cache . This method will take the userId and pageInfo and save the data 
        /// </summary>
        /// <param name="companyUserId"></param>
        /// <param name="pageInfo"></param>
        /// <returns></returns>
        public string SaveUserSpecificPageLayout(int companyUserId, PageInfo pageInfo)
        {
            string errorMessage = String.Empty;

            try
            {
                if(pageInfo != null)
                {
                    string pageName = pageInfo.pageName ?? string.Empty;
                    string oldPageName = pageInfo.oldPageName ?? string.Empty;
                    string pageTag = pageInfo.pageTag ?? string.Empty;
                    string pageLayout = pageInfo.pageLayout ?? string.Empty;
                    string pageId = pageInfo.pageId ?? string.Empty;

                    if (string.IsNullOrEmpty(pageName) || string.IsNullOrEmpty(pageId))
                        errorMessage = LayoutServiceConstants.MSG_ErrorForSaveLayout;

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        try
                        {
                            int rowsAffected = _dbManager.SaveUpdatePageInfo(companyUserId, pageInfo);
                            if (rowsAffected > 0)
                            {
                                //In case this is the first saved page for the user , so creating a blank dictionary for this userId
                                if (!_userWisePageLayoutInfo.ContainsKey(companyUserId))
                                    _userWisePageLayoutInfo.Add(companyUserId, new Dictionary<string, PageInfo>());

                                if (_userWisePageLayoutInfo != null && _userWisePageLayoutInfo.ContainsKey(companyUserId))
                                {
                                    //taking internal dictionary for the userID and now either saving the page or updating the page .
                                    Dictionary<string,PageInfo> userPages = _userWisePageLayoutInfo[companyUserId];

                                    if (userPages.ContainsKey(pageId))
                                    {
                                        PageInfo existingPageInfo = userPages[pageId];
                                        existingPageInfo.pageName = pageInfo.pageName;
                                        existingPageInfo.pageLayout = pageInfo.pageLayout;
                                        existingPageInfo.pageTag = pageInfo.pageTag;
                                        existingPageInfo.pageId = pageInfo.pageId;
                                    }
                                    else
                                    {
                                        //Adding a new page info in the dictionary cache
                                        userPages[pageInfo.pageId] = pageInfo;
                                    }                                    
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            errorMessage = ex.Message;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return errorMessage;
        }

        // <summary>
        /// /// Loads RTPNL views for logged in User from database and adds in local cache
        /// </summary>
        /// <param name="companyUserID"></param>
        public void LoadLayoutForLoggedInUser(int companyUserId)
        {
           try
           {
                Dictionary<string,PageInfo> pageInfoResponse = _dbManager.GetPageInfo(companyUserId);
                if (pageInfoResponse != null)
                {
                    if (_userWisePageLayoutInfo.ContainsKey(companyUserId))
                    {
                        // Update the existing entry
                        foreach (var kvp in pageInfoResponse)
                        {
                            _userWisePageLayoutInfo[companyUserId][kvp.Key] = kvp.Value;
                        }
                    }
                    else
                    {
                        // Add a new entry
                        _userWisePageLayoutInfo[companyUserId] = new Dictionary<string, PageInfo>(pageInfoResponse);
                    }
                }
            }
           catch(Exception ex)
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
        public void RemoveLayoutForLoggedOutUser(int companyUserId)
        {
            try
            {
                if (_userWisePageLayoutInfo.ContainsKey(companyUserId))
                {
                    _userWisePageLayoutInfo[companyUserId].Clear();
                    _userWisePageLayoutInfo.Remove(companyUserId);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
