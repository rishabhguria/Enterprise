using Prana.KafkaWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.LayoutService.Contracts
{
    /// <summary>
    /// Defines the contract for every layout manager.
    /// </summary>
    public interface ILayoutManager
    {

        /// <summary>
        /// Loads the layout for a logged-in user based on their company user Id.
        /// </summary>
        /// <param name="companyUserID">The Id of the company user.</param>
        void LoadLayoutForLoggedInUser(int companyUserId);

        /// <summary>
        /// Removes the layout from the cache for a logged-in user based on their company user Id.
        /// </summary>
        /// <param name="companyUserID">The Id of the company user.</param>
        void RemoveLayoutForLoggedOutUser(int companyUserId);

        /// <summary>
        /// Saves the user specific layout in the cache.
        /// </summary>
        /// <param name="topic">The topic of the kafka request.</param>
        /// <param name="message">Object contains details related to this request (RequestID ,CompanyUserId ,Data). </param>
        //void KafkaManager_SaveUserSpecificLayout(string topic, RequestResponseModel message);

        /// <summary>
        /// Deletes the user specific layout in the cache.
        /// </summary>
        /// <param name="topic">The topic of the kafka request.</param>
        /// <param name="message">Object contains details related to this request (RequestID ,CompanyUserId ,Data). </param>
        //void KafkaManager_DeleteUserSpecificLayout(string topic, RequestResponseModel message);

        /// <summary>
        /// Gets the user specific layout in the cache.
        /// </summary>
        /// <param name="topic">The topic of the kafka request.</param>
        /// <param name="message">Object contains details related to this request (RequestID ,CompanyUserId ,Data). </param>
        //void KafkaManager_GetUserSpecificLayout(string topic, RequestResponseModel message);
    }
}
