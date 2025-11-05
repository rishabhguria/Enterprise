using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.AllocationNew.Allocation.BusinessObjects
{
    /// <summary>
    /// Enum for Preference operation
    /// </summary>
    public enum AllocationPrefOperation
    {
        /// <summary>
        /// Add new preference
        /// </summary>
        Add,
        /// <summary>
        /// Copy preference
        /// </summary>
        Copy,
        /// <summary>
        /// Delete preference
        /// </summary>
        Delete,
        /// <summary>
        /// Open preference
        /// </summary>
        Open,
        /// <summary>
        /// Rename Preference
        /// </summary>
        Rename,
        /// <summary>
        /// Export Preference
        /// </summary>
        Export,
        /// <summary>
        /// ExportAll Preference
        /// </summary>
		ExportAll,
        /// <summary>
        /// Import Preference
        /// </summary>
        Import,
        None

    }
}
