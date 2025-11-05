using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.AllocationNew.Allocation.BusinessObjects
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
        /// Path to impot/export files
        /// </summary>
       public String ImportExportPath { get; set; } 

        /// <summary>
        /// Constructor for initializing event args.
        /// </summary>
       public AllocationPrefOperationEventArgs()
       {
           this.PrefId = Int32.MinValue;
           this.PrefName = String.Empty;
           this.AllocationPrefOperation = new AllocationPrefOperation();
           this.CopyPrefId = Int32.MinValue;
           this.ImportExportPath = String.Empty;
       }
    }
}
