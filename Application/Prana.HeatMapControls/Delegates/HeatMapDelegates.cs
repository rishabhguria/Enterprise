using Prana.HeatMapControls.EventArguments;
using System;

namespace Prana.HeatMapControls.Delegates
{
    /// <summary>
    /// Delegate for drilldown on HeatMap
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void DrilledDown(Object sender, DrillDownEventArgs e);

    /// <summary>
    /// Delegate for drillup on HeatMap
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void DrilledUp(Object sender, DrillUpEventArgs e);
}
