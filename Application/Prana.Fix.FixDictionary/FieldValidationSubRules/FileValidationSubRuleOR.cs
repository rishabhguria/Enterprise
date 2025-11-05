using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Xml;
namespace Prana.Fix.FixDictionary
{
    [Serializable]
    public class FieldValidationSubRuleOR : FieldValidationSubRule
    {
        List<string> tags = new List<string>();
        List<string> values = new List<string>();
        public FieldValidationSubRuleOR(XmlNode ruleNode)
        {

            try
            {
                tags = GeneralUtilities.GetListFromString(ruleNode.SelectSingleNode("RequiredIf").Attributes["Tags"].Value, ',');
                values = GeneralUtilities.GetListFromString(ruleNode.SelectSingleNode("RequiredIf").Attributes["Values"].Value, ',');
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

        public override bool CheckIfRequiredField(PranaMessage pranaMessage)
        {
            bool result = false;
            try
            {

                for (int j = 0; j < tags.Count; j++)
                {
                    if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(tags[j]) && values[j] == pranaMessage.FIXMessage.ExternalInformation[tags[j]].Value)
                    {
                        result = true;
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
