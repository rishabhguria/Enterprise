using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Xml;
namespace Prana.Fix.FixDictionary
{
    [Serializable]
    public class FieldValidationSubRuleTAGPRESENT : FieldValidationSubRule
    {
        List<string> tags = new List<string>();

        public FieldValidationSubRuleTAGPRESENT(XmlNode ruleNode)
        {
            try
            {
                tags = GeneralUtilities.GetListFromString(ruleNode.SelectSingleNode("RequiredIf").Attributes["PresentTag"].Value, ',');
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
            bool result = true;

            try
            {
                int i = 0;
                foreach (string tag in tags)
                {
                    if (!(pranaMessage.FIXMessage.ExternalInformation.ContainsKey(tag)))
                    {
                        result = false;
                        break;
                    }
                    i++;
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
