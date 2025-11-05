using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.Fix.FixDictionary
{
    [Serializable]
    public class FieldValidationRule1 : FieldValidationRule
    {

        private List<FieldValidationSubRule> _subRules = new List<FieldValidationSubRule>();

        public void AddSubRule(FieldValidationSubRule subRule)
        {
            _subRules.Add(subRule);

        }

        public FieldValidationRule1()
        {

        }

        public override bool CheckIfRequiredField(PranaMessage pranaMessage)
        {
            bool result = true;
            try
            {

                foreach (FieldValidationSubRule subrule in _subRules)
                {
                    if (!subrule.CheckIfRequiredField(pranaMessage))
                    {
                        result = false;
                        break;
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
            return result;
        }
    }
}
