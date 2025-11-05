using System.Collections.Generic;

namespace Prana.BusinessObjects.Classes.Allocation
{
    public class AllocationPrefetchFilter
    {
        /// <summary>
        /// The _allocated
        /// </summary>
        private Dictionary<string, string> _allocated = new Dictionary<string, string>();

        /// <summary>
        /// The _unallocated
        /// </summary>
        private Dictionary<string, string> _unallocated = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the allocated.
        /// </summary>
        /// <value>
        /// The allocated.
        /// </value>
        public Dictionary<string, string> Allocated
        {
            get { return _allocated; }
            set { _allocated = value; }
        }

        /// <summary>
        /// Gets or sets the unallocated.
        /// </summary>
        /// <value>
        /// The unallocated.
        /// </value>
        public Dictionary<string, string> Unallocated
        {
            get { return _unallocated; }
            set { _unallocated = value; }
        }
    }
}
