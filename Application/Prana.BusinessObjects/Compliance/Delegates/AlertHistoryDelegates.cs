using Prana.BusinessObjects.Compliance.EventArguments;
using System;

namespace Prana.BusinessObjects.Compliance.Delegates
{
    /// <summary>
    /// Delegates for alert history.
    /// Getting alerts for the date range and Update alert grid for current alerts.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void GetAlertHandler(Object sender, GetAlertEventArgs e);
    public delegate void UpdateAlertGrid(Object sender, UpdateAlertGridEventArgs e);
}
