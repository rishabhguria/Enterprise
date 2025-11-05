using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Compliance.Definition
{
    public class GroupBase
    {
        public String GroupId { get; set; }
        public String GroupName { get; set; }
        //public String LastTriggeredRuleId { get; set; }
        //public String CurrentTriggeredRuleId { get; set; }
        //public String Dimension { get; set; }
        public NotificationSetting Notification { get; set; }
        public List<RuleBase> RuleList { get; set; }

        public GroupBase DeepClone()
        {
            //TODO: DeepCopyHelher
            GroupBase group = new GroupBase(this);
            return group;
        }

        public GroupBase()
        {
            try
            {
                this.GroupId = String.Empty;
                this.GroupName = String.Empty;
                //this.LastTriggeredRuleId = String.Empty;
                //this.CurrentTriggeredRuleId = String.Empty;
                //this.Dimension = String.Empty;
                this.RuleList = new List<RuleBase>();
                this.Notification = new NotificationSetting();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public GroupBase(GroupBase groupBase)
        {
            try
            {
                this.GroupId = groupBase.GroupId;
                this.GroupName = groupBase.GroupName;
                this.Notification = groupBase.Notification;
                this.RuleList = groupBase.RuleList;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
