
using Prana.BusinessObjects.Enums;
using System;
using System.Security.Principal;

namespace Prana.BusinessObjects.Authorization
{
    /// <summary>
    /// Nirvana implementation of IIdentity
    /// </summary>
    [Serializable]
    public class NirvanaIdentity : IIdentity
    {
        /// <summary>
        /// 
        /// </summary>
        private bool isAuthenticated = false;

        /// <summary>
        /// 
        /// </summary>
        private NirvanaRoles nirvanaRole;

        /// <summary>
        /// Name of the identity
        /// </summary>
        private String name;

        /// <summary>
        /// UserId of the identity
        /// </summary>
        private int userId;

        private int companyID;


        /// <summary>
        /// 
        /// </summary>
        public NirvanaRoles Role
        {
            get { return nirvanaRole; }
            set { this.nirvanaRole = value; }
        }

        #region IIdentity Members

        /// <summary>
        /// 
        /// </summary>
        public string AuthenticationType
        {
            get { return "NirvanaAuthentication"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set { this.isAuthenticated = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        #endregion

    }
}
