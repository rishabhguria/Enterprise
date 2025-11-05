using System;

namespace Prana.BusinessObjects.Compliance.Definition
{
    public class GroupNotificationDetails
    {
        public String LastTriggeredRuleId { get; set; }
        public String CurrentTriggeredRuleId { get; set; }
        public String Dimension { get; set; }
        public DateTime LastValidationTime { get; set; }

        public GroupNotificationDetails()
        {
            this.LastTriggeredRuleId = String.Empty;
            this.CurrentTriggeredRuleId = String.Empty;
            this.Dimension = String.Empty;
            this.LastValidationTime = DateTime.Now;
        }

    }
}
