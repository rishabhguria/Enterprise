

using Prana.BusinessObjects.Enums;
using System;
using System.Security.Principal;

namespace Prana.BusinessObjects.Authorization
{
    /// <summary>
    /// Nirvana implementation of IPrincipal
    /// </summary>
    public class NirvanaPrincipal : IPrincipal
    {
        /// <summary>
        /// A single identity is associated with Principal
        /// </summary>
        NirvanaIdentity identity;



        #region IPrincipal Members

        /// <summary>
        /// Nirvana implementation of IPrincipal
        /// It return instance of NirvanaIdentity
        /// </summary>
        public IIdentity Identity
        {
            get { return identity; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {


            NirvanaRoles nirvanaRole;
            bool isTypeOFNirvanaRole = Enum.TryParse<NirvanaRoles>(role, true, out nirvanaRole);
            if (!isTypeOFNirvanaRole)
                throw new InvalidCastException(role + " is not identified as NirvanRoles");

            if (nirvanaRole == identity.Role)
                return true;
            else
                return false;

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        public NirvanaPrincipal(NirvanaIdentity identity)
        {
            this.identity = identity;
        }

        /// <summary>
        /// 
        /// </summary>
        public NirvanaRoles Role
        {
            get { return identity.Role; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int UserId
        {
            get { return identity.UserId; }
        }

        public int CompanyID
        {
            get { return identity.CompanyID; }
        }

    }
}
