using Prana.AuditManager.Definitions.Enum;
using System;
using System.Xml.Serialization;

namespace Prana.AuditManager.Definitions.Data
{
    /// <summary>
    /// Class to define audit properties
    /// </summary>
    [Serializable]
    [XmlRoot("AuditDataList")]
    public class AuditDataDefinition
    {
        [XmlElement("Action")]
        public AuditAction Action { get; set; }

        [XmlElement("UserId")]
        public int UserId { get; set; }

        [XmlElement("CompanyId")]
        public int CompanyId { get; set; }

        [XmlElement("CompanyFundId")]
        public int CompanyAccountId { get; set; }

        [XmlElement("AppliedAuditTime")]
        public DateTime AppliedAuditTime { get; set; }

        [XmlElement("Comment")]
        public String Comment { get; set; }

        [XmlElement("StatusId")]
        public int StatusId { get; set; }

        [XmlElement("ModuleId")]
        public int ModuleId { get; set; }

        [XmlElement("ActualAuditTime")]
        public DateTime ActualAuditTime { get; set; }

        [XmlElement("AuditDimensionValue")]
        public int AuditDimensionValue { get; set; }

        [XmlElement("IsActive")]
        public bool IsActive { get; set; }

    }
}
