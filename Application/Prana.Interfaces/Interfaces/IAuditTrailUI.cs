using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Interfaces
{
    /// <summary>
    /// Interface for AuditTrailUI
    /// </summary>
    public interface IAuditTrailUI
    {
        System.Windows.Forms.Form Reference();
        event EventHandler FormClosed;

        Prana.BusinessObjects.CompanyUser LoginUser
        {
            set;
        }
        void BindNewTableToTradeAudit(DataTable entries);
        void GetAndBindAuditUIDataForFilters(AuditTrailFilterParams auditTrailFilterParams);
        void GetAndBindAuditUIDataForGroupIds(List<string> groups);
    }
}
