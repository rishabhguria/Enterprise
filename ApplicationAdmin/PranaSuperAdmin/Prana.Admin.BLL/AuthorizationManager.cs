using Prana.Auth.Authorization.BLL;
using Prana.BusinessObjects.Authorization;
using Prana.BusinessObjects.Enums;
using Prana.LogManager;
using System;

namespace Prana.Admin.BLL
{
    public class AuthorizationManager
    {
        #region Singleton Instance

        /// <summary>
        /// Locker object for singleton instance of the class
        /// </summary>
        private static Object _singletonLocker = new object();

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static AuthorizationManager _instance;

        /// <summary>
        /// Method to get singleton instance of the AuthorizationManager
        /// </summary>
        /// <returns>singleton instance of the AuthorizationManager</returns>
        public static AuthorizationManager GetInstance()
        {
            lock (_singletonLocker)
            {
                if (_instance == null)
                    _instance = new AuthorizationManager();
            }
            return _instance;
        }

        /// <summary>
        /// Private constructor of the AuthorizationManager to implement singleton instance
        /// </summary>
        private AuthorizationManager() { }

        #endregion

        /// <summary>
        /// authorizedPrincipal of current logged-in user
        /// </summary>
        public NirvanaPrincipal _authorizedPrincipal { get; set; }

        /// <summary>
        /// Check Accessibility For Module
        /// </summary>
        /// <param name="module"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public Boolean CheckAccesibilityForMoldule(ModuleResources module, AuthAction Action)
        {
            try
            {
                //ModuleResources currentModule = ModuleResources.COMPANY;
                int resourceId = (int)module;
                ResourceAction action = new ResourceAction(resourceId, NirvanaResourceType.Modules, Action);
                var hasAccess = NirvanaAuthorizationManager.GetInstance().HasAccess(_authorizedPrincipal, action, NirvanaResourceType.Modules);
                return hasAccess;
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
                return false;
            }
        }

        /// <summary>
        /// Check Accesibility For AccountGroup
        /// </summary>
        /// <param name="accountGroup"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public Boolean CheckAccesibilityForAccountGroup(int accountGroup, AuthAction Action)
        {
            try
            {
                //ModuleResources currentModule = ModuleResources.COMPANY;
                int resourceId = accountGroup;
                ResourceAction action = new ResourceAction(resourceId, NirvanaResourceType.Modules, Action);
                var hasAccess = NirvanaAuthorizationManager.GetInstance().HasAccess(_authorizedPrincipal, action, NirvanaResourceType.Modules);
                return hasAccess;
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
                return false;
            }
        }

        /// <summary>
        /// Refresh User Permissions, no need to restart Client.
        /// </summary>
        public void RefreshUserPermissions()
        {
            try
            {
                NirvanaAuthorizationManager.GetInstance().RefreshUserPermissions();
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
