using Prana.Auth.Authorization.DAL;
using Prana.BusinessObjects.Authorization;
using Prana.BusinessObjects.Enums;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.Auth.Authorization.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class NirvanaAuthorizationManager
    {
        #region Singleton Instsnce

        /// <summary>
        /// 
        /// </summary>
        static Object _singletonLocker = new object();

        /// <summary>
        /// 
        /// </summary>
        static NirvanaAuthorizationManager _instance;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static NirvanaAuthorizationManager GetInstance()
        {
            lock (_singletonLocker)
            {
                if (_instance == null)
                    _instance = new NirvanaAuthorizationManager();
            }
            return _instance;
        }

        /// <summary>
        /// 
        /// </summary>
        private NirvanaAuthorizationManager()
        {
            LoadAuthorizationCache();
        }


        #endregion

        private void LoadAuthorizationCache()
        {
            lock (cacheLockerObject)
            {
                resourceActionCache = NirvanaAuthorizationDataManager.GetDataFromDatabase();
            }
        }


        private Object cacheLockerObject = new object();
        private Dictionary<NirvanaPrincipalType, Dictionary<int, List<ResourceAction>>> resourceActionCache = new Dictionary<NirvanaPrincipalType, Dictionary<int, List<ResourceAction>>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incomingPrincipal"></param>
        /// <param name="resourceAction"></param>
        /// <returns></returns>
        public bool HasAccess(NirvanaPrincipal incomingPrincipal, ResourceAction resourceAction, NirvanaResourceType resourceType)
        {
            Boolean isAllowed = false;
            try
            {
                if (!incomingPrincipal.Identity.IsAuthenticated)
                    return false;


                switch (resourceType)
                {
                    case NirvanaResourceType.Modules:
                        isAllowed = ValidateResourceActionForRole(incomingPrincipal.Role, resourceAction);
                        break;
                    case NirvanaResourceType.AccountGroup:
                        isAllowed = ValidateResourceActionForUser(incomingPrincipal.UserId, resourceAction);
                        break;


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
            return isAllowed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool ValidateResourceActionForRole(NirvanaRoles role, ResourceAction action)
        {
            int userRole = (int)role;
            try
            {
                lock (cacheLockerObject)
                {
                    if (resourceActionCache.ContainsKey(NirvanaPrincipalType.Role))
                    {
                        Dictionary<int, List<ResourceAction>> resourceActionDict = resourceActionCache[NirvanaPrincipalType.Role];

                        foreach (int roleIdInCache in resourceActionDict.Keys)
                        {
                            //Hierarchical role id check
                            if (roleIdInCache <= userRole)
                            {
                                var resAct = resourceActionDict[roleIdInCache]
                                    .Where(p => p.ResourceId == action.ResourceId && p.ResourceType == action.ResourceType);
                                //    .First<ResourceAction>();
                                // As actions are hierarchical so greater than equal to operater is applied    
                                if (resAct.Count() > 0)
                                {
                                    if (resAct.First().ActionId >= action.ActionId)
                                        return true;
                                }
                            }
                        }

                    }
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
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool ValidateResourceActionForUser(int userId, ResourceAction action)
        {

            try
            {
                lock (cacheLockerObject)
                {
                    if (resourceActionCache.ContainsKey(NirvanaPrincipalType.User))
                    {
                        Dictionary<int, List<ResourceAction>> resourceActionDict = resourceActionCache[NirvanaPrincipalType.User];

                        if (resourceActionDict.ContainsKey(userId))
                        {
                            var resAct = resourceActionDict[userId]
                                    .Where(p => p.ResourceId == action.ResourceId && p.ResourceType == action.ResourceType);
                            //    .First<ResourceAction>();
                            // As actions are hierarchical so greater than equal to operater is applied    
                            if (resAct.Count() > 0)
                            {
                                if (resAct.First().ActionId >= action.ActionId)
                                    return true;
                            }

                        }

                    }
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
            return false;
        }


        /// <summary>
        /// Reload Permissions mapping
        /// Created by omshiv, 14 May 2014
        /// </summary>
        public void RefreshUserPermissions()
        {
            try
            {
                LoadAuthorizationCache();
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