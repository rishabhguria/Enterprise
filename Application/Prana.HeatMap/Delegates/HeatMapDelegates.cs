using Prana.HeatMap.EventArguments;
using System;

namespace Prana.HeatMap.Delegates
{
    /// <summary>
    /// Delegate for when data is received from Esper
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void EsperDataReceived(Object sender, EsperDataReceivedEventArgs e);

    /// <summary>
    /// Update data on the actual heat map
    /// </summary>
    public delegate void UpdateData(Object sender, UpdateDataEventArgs e);
}
