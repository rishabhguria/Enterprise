using System;

namespace Prana.HeatMapControls.EventArguments
{
    /// <summary>
    /// Event Arguments for drill down in HeatMaps
    /// </summary>
    public class DrillDownEventArgs : EventArgs
    {
        public string Value { get; set; }
    }
}
