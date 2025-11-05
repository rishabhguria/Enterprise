using System;

namespace Prana.Interfaces
{
    /// <summary>
    /// Its a interface for Position Management reports.
    /// </summary>
    public interface IPositionManagementReports
    {
        event EventHandler FormClosedHandler;
        System.Windows.Forms.Form Reference();
        Prana.BusinessObjects.CompanyUser LoginUser
        {
            get;
            set;
        }
        string ServerReportURL
        {
            set;
        }
        string ServerReportName
        {
            set;
        }
    }
}
