using Prana.AuditManager.Definitions.Enum;
using System;

namespace Prana.AuditManager.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AuditAttribute : Attribute
    {
        public AuditAction AuditAction { get; set; }

        public bool ShowAuditUI { get; set; }

        public AuditAttribute(AuditAction action)
        {
            this.AuditAction = action;
        }
    }
}