using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using System;
using System.Data;

namespace Prana.BusinessObjects.Compliance.Definition
{
    /// <summary>
    /// Derives Rule base class. 
    /// Description:- Description of custom rules which we display on UI
    /// returns cloned copy of rule.
    /// </summary>
    public class CustomRuleDefinition : RuleBase
    {
        public string Description { get; set; }
        public string ConstantsDefinationAsJSon { get; set; }


        public CustomRuleDefinition()
        { }
        public CustomRuleDefinition(DataRow drRecieved)
            : base(drRecieved, RuleCategory.CustomRule)
        {
            //this.RuleId = key;

            this.Description = drRecieved["description"].ToString();
            this.ConstantsDefinationAsJSon = drRecieved["constants"].ToString();

            //this.RuleURL = drRecieved["htmlPath"].ToString();
            //this.Name = drRecieved["name"].ToString();
            //this.RuleType = drRecieved["ruleType"].ToString();

        }


        public CustomRuleDefinition(RuleBase rule)
            : base(rule)
        {
            // TODO: Complete member initialization

            try
            {
                if (rule is CustomRuleDefinition)
                {
                    CustomRuleDefinition temp = (CustomRuleDefinition)rule;
                    this.Description = temp.Description;
                    this.ConstantsDefinationAsJSon = temp.ConstantsDefinationAsJSon;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        public override RuleBase DeepClone()
        {
            return new CustomRuleDefinition(this);
        }
    }
}
