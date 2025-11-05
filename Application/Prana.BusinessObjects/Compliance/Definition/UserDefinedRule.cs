using Prana.BusinessObjects.Compliance.Enums;
using System;
using System.Data;

namespace Prana.BusinessObjects.Compliance.Definition
{
    /// <summary>
    /// Derive Abstract class Rule Base
    /// UUID- Guvnor generated rule Id
    /// RuleBRL- Url for rule source.
    /// </summary>
    public class UserDefinedRule : RuleBase
    {
        public String Uuid { get; set; }
        public String RuleBrl { get; set; }

        public UserDefinedRule(DataRow drRecieved)
            : base(drRecieved, RuleCategory.UserDefined)
        {
            this.Uuid = drRecieved["uuid"].ToString();
            this.RuleBrl = drRecieved["ruleUrl"].ToString();

        }

        public UserDefinedRule(RuleBase rule)
            : base(rule)
        {
            // TODO: Complete member initialization

            if (rule is UserDefinedRule)
            {
                UserDefinedRule temp = (UserDefinedRule)rule;
                this.Uuid = temp.Uuid;
                this.RuleBrl = temp.RuleBrl;
            }


        }

        public UserDefinedRule()
            : base()
        {

        }


        public override RuleBase DeepClone()
        {
            return new UserDefinedRule(this);
        }
    }
}
