using Prana.LogManager;
using System;
using System.Xml;
namespace Prana.Fix.FixDictionary
{
    class FixValidationRuleLoader
    {
        public static FieldValidationRule LoadRule(XmlNode xmlNode)
        {
            FieldValidationRule1 rule = new FieldValidationRule1();
            try
            {
                FieldValidationSubRule subRule = null;

                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    if (node.Attributes != null)
                    {
                        string subruleID = node.Attributes["RuleID"].Value;

                        switch (subruleID)
                        {

                            case "or":
                                subRule = new FieldValidationSubRuleOR(node);
                                rule.AddSubRule(subRule);
                                break;

                            case "and":
                                subRule = new FieldValidationSubRuleAND(node);
                                rule.AddSubRule(subRule);
                                break;

                            case "NotPresentTag":
                                subRule = new FieldValidationSubRuleTAGNOTPRESENT(node);
                                rule.AddSubRule(subRule);
                                break;

                            case "PresentTag":
                                subRule = new FieldValidationSubRuleTAGPRESENT(node);
                                rule.AddSubRule(subRule);
                                break;
                        }
                    }
                }
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
            return rule;
        }
    }
}
