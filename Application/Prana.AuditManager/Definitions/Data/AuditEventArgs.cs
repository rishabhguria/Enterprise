using System;
using System.Collections.Generic;

namespace Prana.AuditManager.Definitions.Data
{

    /// <summary>
    /// Class consists of AuditData property of type AuditDataDefinition
    /// </summary>
    public class AuditEventArgs : EventArgs
    {
        // property to describe Audit data details
        public List<AuditDataDefinition> AuditData { get; set; }
    }
}
