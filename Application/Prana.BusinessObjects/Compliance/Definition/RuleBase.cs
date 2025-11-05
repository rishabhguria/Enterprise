using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.BusinessObjects.Compliance.Definition
{
    /// <summary>
    /// Abstract class
    /// Contains all fields for Rule which are common for user defined and custom rules.
    /// Rule Id of rule. UUId for user defined rules.
    /// Rule name
    /// Category- From Enum RuleCategory
    /// Package- From Enum RulePackage
    /// Notification of type NotificationSetting
    /// Enabled- Rule is Enabled or Disabled.
    /// RuleURl- Url from where to load rules.
    /// </summary>
    public abstract class RuleBase
    {

        public String RuleId { get; set; }
        public String RuleName { get; set; }


        public RuleCategory Category { get; set; }
        public RulePackage Package { get; set; }

        public NotificationSetting Notification { get; set; }
        public bool Enabled { get; set; }
        public string RuleURL { get; set; }

        public String GroupId { get; set; }

        public RuleBase()
        {
            this.Notification = new NotificationSetting();
        }


        public RuleBase(DataRow dr, RuleCategory category)
        {
            // TODO: Complete member initialization
            this.RuleId = dr["ruleId"].ToString();
            this.Category = category;
            this.Package = (RulePackage)Enum.Parse(typeof(RulePackage), dr["ruleType"].ToString(), true);
            this.RuleName = dr["ruleName"].ToString();
            this.Enabled = Boolean.Parse(dr["enabled"].ToString());
            this.GroupId = "-1";
        }

        public RuleBase(RuleBase rule)
        {
            // TODO: Complete member initialization
            this.Category = rule.Category;
            this.Enabled = rule.Enabled;
            this.Package = rule.Package;
            this.RuleId = rule.RuleId;
            this.RuleName = rule.RuleName;
            this.RuleURL = rule.RuleURL;
            this.GroupId = rule.GroupId;

            if (rule.Notification != null)
                this.Notification = rule.Notification.DeepClone();

        }
        public abstract RuleBase DeepClone();





        public override bool Equals(object obj)
        {
            try
            {
                if (obj is RuleBase)
                {
                    RuleBase baseOther = obj as RuleBase;
                    if (this.Package == baseOther.Package && this.RuleName == baseOther.RuleName)
                        return true;
                    else return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        public override int GetHashCode()
        {
            try
            {
                return base.GetHashCode();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return Int32.MinValue;
            }
        }

    }
}
