using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.Interfaces;
using Prana.ComplianceEngine.RuleDefinition.DAL;
using Prana.LogManager;
using System;

namespace Prana.ComplianceEngine.RuleDefinition.BLL
{
    /// <summary>
    /// Class which returns instance for all the handlers.
    /// </summary>
    internal static class RuleFactoryHandler
    {

        internal static IRuleHandler GetRuleHandlerFor(RuleCategory category)
        {
            try
            {
                IRuleHandler returnInstance = null;
                switch (category)
                {
                    case RuleCategory.CustomRule:
                        returnInstance = CustomRuleHandler.GetInstance();
                        break;
                    case RuleCategory.UserDefined:
                        returnInstance = UserDefinedRuleHandler.GetInstance();
                        break;
                        //add more cases

                }

                return returnInstance;
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
                return null;
            }

        }

        internal static CommonRuleHandler GetCommonRuleHandler()
        {
            try
            {
                CommonRuleHandler returnInstance = CommonRuleHandler.GetInstance();
                return returnInstance;
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
                return null;
            }
        }




    }
}