using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Xml;
namespace Prana.CustomMapper
{
    class DefaultRule : Rule
    {
        DirectionListOfRules DirectionListOfRules = new DirectionListOfRules();

        public override void LoadRule(System.Xml.XmlNode node)
        {
            try
            {
                XmlNodeList xmlNodeList = node.SelectNodes("Condition");
                foreach (XmlNode xmlNodeItem in xmlNodeList)
                {
                    DefaultRuleParameters defaultParameters = new DefaultRuleParameters();
                    if (defaultParameters.CreateRules(xmlNodeItem))
                    {
                        DirectionListOfRules.Add(defaultParameters);
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
        }
        public override void ApplyRule(PranaMessage pranaMessage, Direction direction)
        {
            try
            {
                DirectionListOfRules.ApplyRule(direction, pranaMessage);
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

        public override void ApplyRule(List<PranaMessage> PranaMsg, Direction direction)
        {
            return;
        }
    }

    class DefaultRuleParameters : RuleParameters
    {
        private string[] toTags;
        private string[] toTagsValues;

        public override void SetValues(PranaMessage PranaMsg)
        {
            try
            {
                string repeatingTag = toTags != null && toTags.Length > 0 ? toTags[0] : string.Empty;              
                
                if (base.IsRuleValid(PranaMsg, repeatingTag))
                {
                    int i = 0;
                    foreach (string toTag in toTags)
                    {
                        if (base.Direction == Direction.Out)
                        {
                            FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(toTag);
                            if (!string.IsNullOrEmpty(RepeatingIdentiferTag) && fixField != null)
                            {
                                UpdatedMsgFieldList(PranaMsg, RepeatingIdentiferTag, string.Empty, toTag, toTagsValues[i]);
                            }
                            else
                            {
                                PranaMsg.FIXMessage.CustomInformation.AddField(toTag, toTagsValues[i]);
                            }
                        }
                        else
                        {
                            FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(toTag);
                            if (fixField != null)
                            {
                                if (!string.IsNullOrEmpty(RepeatingIdentiferTag))
                                {
                                    UpdatedMsgFieldList(PranaMsg, RepeatingIdentiferTag, string.Empty, toTag, toTagsValues[i]);
                                }
                                else if (fixField.IsExternal)
                                {
                                    if (!PranaMsg.FIXMessage.ExternalInformation.ContainsKey(toTag))
                                    {
                                        PranaMsg.FIXMessage.ExternalInformation.AddField(toTag, toTagsValues[i]);
                                    }
                                }
                                else
                                {
                                    if (!PranaMsg.FIXMessage.InternalInformation.ContainsKey(toTag))
                                    {
                                        PranaMsg.FIXMessage.InternalInformation.AddField(toTag, toTagsValues[i]);
                                    }
                                }
                            }
                            else
                            {
                                if (!PranaMsg.FIXMessage.CustomInformation.ContainsKey(toTag))
                                {
                                    PranaMsg.FIXMessage.CustomInformation.AddField(toTag, toTagsValues[i]);
                                }
                            }
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }

        }
        public override bool CreateRules(System.Xml.XmlNode xmlNodeItem)
        {
            try
            {
                toTags = xmlNodeItem.Attributes["ToTags"].Value.Split(',');
                toTagsValues = xmlNodeItem.Attributes["ToTagsValues"].Value.Split(',');

                return base.CreateRules(xmlNodeItem);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return true;
        }
    }
}
