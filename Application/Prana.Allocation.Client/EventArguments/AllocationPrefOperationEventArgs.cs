using Prana.Allocation.Client.Enums;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Allocation.Client.EventArguments
{
    public class AllocationPrefOperationEventArgs : EventArgs
    {
        /// <summary>
        /// Opereation that to be performed
        /// </summary>
        public AllocationPrefOperation AllocationPrefOperation { get; set; }

        /// <summary>
        /// Preference name
        /// </summary>
        public String PrefName { get; set; }

        /// <summary>
        /// preference Id.
        /// </summary>
        public int PrefId { get; set; }

        /// <summary>
        /// Preference id of preference to be copied.
        /// min value in case of other operations.
        /// </summary>
        public int CopyPrefId { get; set; }

        /// <summary>
        /// Gets or sets the import export path.
        /// </summary>
        /// <value>
        /// The import export path.
        /// </value>
        public List<string> ImportExportPath { get; set; }

        /// <summary>
        /// Constructor for initializing event args.
        /// </summary>
        public AllocationPrefOperationEventArgs()
        {
            try
            {
                this.PrefId = Int32.MinValue;
                this.PrefName = String.Empty;
                this.AllocationPrefOperation = AllocationPrefOperation.None;
                this.CopyPrefId = Int32.MinValue;
                this.ImportExportPath = new List<string>();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
    }
}
