using Prana.BusinessObjects.Enums;
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects.Authorization
{
    /// <summary>
    /// Resource and action are encapsulated in a single class to handle authorization
    /// </summary>
    public class ResourceAction
    {
        /// <summary>
        /// Id of resource
        /// </summary>
        public int ResourceId { get; set; }

        /// <summary>
        /// Type of the resource
        /// </summary>
        public NirvanaResourceType ResourceType { get; set; }

        /// <summary>
        /// Action allowed on that resource
        /// </summary>
        public AuthAction ActionId { get; set; }



        public ResourceAction(int resourceId, NirvanaResourceType resourceType, AuthAction actionId)
        {
            try
            {
                this.ActionId = actionId;
                this.ResourceId = resourceId;
                this.ResourceType = resourceType;
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

        public ResourceAction(int resourceId, int resourceType, int actionId)
        {

            try
            {
                this.ResourceType = (NirvanaResourceType)resourceType;
                this.ActionId = (AuthAction)actionId;
                this.ResourceId = resourceId;
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