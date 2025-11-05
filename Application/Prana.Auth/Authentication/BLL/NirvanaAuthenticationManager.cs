using Prana.Auth.Authentication.DAL;
using Prana.BusinessObjects.Authorization;
using Prana.LogManager;
using System;

namespace Prana.Auth.Authentication.BLL
{
    public class NirvanaAuthenticationManager
    {
        #region Singleton Instance

        /// <summary>
        /// Locker object for singleton instance of the class
        /// </summary>
        private static Object _singletonLocker = new object();

        /// <summary>
        /// Singleton instance of the class
        /// </summary>
        private static NirvanaAuthenticationManager _instance;

        /// <summary>
        /// Method to get singleton instance of the NirvanaAuthenticationManager
        /// </summary>
        /// <returns>singleton instance of the NirvanaAuthenticationManager</returns>
        public static NirvanaAuthenticationManager GetInstance()
        {
            lock (_singletonLocker)
            {
                if (_instance == null)
                    _instance = new NirvanaAuthenticationManager();
            }
            return _instance;
        }

        /// <summary>
        /// Private constructor of the NirvanaAuthenticationManager to implement singleton instance
        /// </summary>
        private NirvanaAuthenticationManager() { }

        #endregion






        /// <summary>
        /// Validates user and returns authorized principal
        /// </summary>
        /// <param name="loginId">Login id of the user</param>
        /// <param name="password">Password to be validated</param>
        /// <returns>Authorized ClaimsPrincipal</returns>
        public NirvanaPrincipal ValidateUser(String loginId, String password)
        {
            NirvanaPrincipal authorizedPrincipal = null;
            try
            {
                //Getting nirvana identity auromatically validates the user
                //NirvanaIdentity nirvanaIdentity = new NirvanaIdentity(loginId, password);

                NirvanaIdentity nirvanaIdentity = ValidateAndUpdateClaims(loginId, password);

                //Getting Authorized principal based on authenticated ClaimsIdentity
                authorizedPrincipal = GetPrincipalFromAuthenticatedClaimsIdentity(nirvanaIdentity);


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
            return authorizedPrincipal;

        }

        private NirvanaIdentity ValidateAndUpdateClaims(string loginId, string password)
        {

            NirvanaIdentity identity = null;// = new NirvanaIdentity();

            try
            {
                NirvanaAuthenticationDataManager.ValidateUser(loginId, password, out identity);
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

            return identity;

        }





        /// <summary>
        /// Encapsulate identity into pricipal
        /// </summary>
        /// <param name="nirvanaIdentity"></param>
        /// <returns></returns>
        private NirvanaPrincipal GetPrincipalFromAuthenticatedClaimsIdentity(NirvanaIdentity nirvanaIdentity)
        {
            NirvanaPrincipal returnPrincipal = new NirvanaPrincipal(nirvanaIdentity);
            return returnPrincipal;
        }




    }
}
