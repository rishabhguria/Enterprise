using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.AllocationNew.Allocation.BusinessObjects
{
    /// <summary>
    /// delegate for Operation on preferences from Preference List Control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AllocationPrefOperationHandler(Object sender, AllocationPrefOperationEventArgs e);

    /// <summary>
    /// delegate for applying bulk changes on general rule grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ApplyBulkChangeHandler(Object sender, ApplyBulkChangeEventArgs e);

}
