using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Classes.Allocation
{
    [Serializable]
    public class AllocationResponse
    {

        /// <summary>
        /// The response locker object
        /// </summary>
        readonly object responseLockerObject = new object();

        /// <summary>
        /// List of groups in response
        /// </summary>
        public List<AllocationGroup> GroupList { get; set; }

        /// <summary>
        /// List of groups in response
        /// </summary>
        public List<AllocationGroup> OldAllocationGroups { get; set; }

        /// <summary>
        /// Contains error if any
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// Inititalizes group list
        /// </summary>
        public AllocationResponse()
        {
            this.Response = string.Empty;
            this.GroupList = new List<AllocationGroup>();
            this.OldAllocationGroups = new List<AllocationGroup>();
        }

        /// <summary>
        /// Sets the allocation response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="groupList">The group list.</param>
        public void SetAllocationResponse(string response, List<AllocationGroup> groupList)
        {
            try
            {
                this.Response = response;
                this.GroupList = groupList;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Adds the allocation response.
        /// </summary>
        /// <param name="resIntermediate">The resource intermediate.</param>
        public void AddAllocationResponse(AllocationResponse resIntermediate)
        {
            try
            {
                if (resIntermediate != null)
                {
                    lock (responseLockerObject)
                    {
                        if (this.Response == null || !this.Response.Contains(resIntermediate.Response.Trim()))
                            this.Response += (!string.IsNullOrWhiteSpace(this.Response)) ? "\n" + resIntermediate.Response.Trim() : resIntermediate.Response.Trim();
                        if (resIntermediate.GroupList != null)
                            this.GroupList.AddRange(resIntermediate.GroupList);
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
        /// Adds the allocation response.
        /// </summary>
        /// <param name="response">The error message.</param>
        /// <param name="allocatedGroups">The allocated groups.</param>
        public void AddAllocationResponse(string response, List<AllocationGroup> allocatedGroups)
        {
            try
            {
                lock (responseLockerObject)
                {
                    if (!string.IsNullOrWhiteSpace(response) && !this.Response.Contains(response.Trim()))
                        this.Response += (!string.IsNullOrWhiteSpace(this.Response)) ? "\n" + response.Trim() : response.Trim();
                    if (allocatedGroups != null)
                        this.GroupList.AddRange(allocatedGroups);
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
