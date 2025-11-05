using System;
using System.Collections.Generic;

namespace Prana.HeatMapControls.EventArguments
{
    /// <summary>
    /// Event Arguments for when grouping is changed
    /// </summary>
    public class GroupingChangedEventArgs : EventArgs
    {
        public List<String> Grouping { get; set; }
    }
}
