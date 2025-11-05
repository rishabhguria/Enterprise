using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Xml;
namespace Prana.CustomMapper
{

    class Rule1 : Rule
    {

        DirectionListOfRules DirectionListOfRules = new DirectionListOfRules();

        public override void ApplyRule(List<PranaMessage> PranaMsg, Direction direction)
        {
            return;
        }

        public override void LoadRule(System.Xml.XmlNode node)
        {
            try
            {
                XmlNodeList xmlNodeList = node.SelectNodes("Condition");
                foreach (XmlNode xmlNodeItem in xmlNodeList)
                {
                    RuleParameters rule1Parameters = new Rule1ParametersNew();
                    if (rule1Parameters.CreateRules(xmlNodeItem))
                    {
                        DirectionListOfRules.Add(rule1Parameters);
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
    }

    class Rule1ParametersNew : RuleParameters
    {
        private string[] toTags;
        private string[] FromTags;
        //private string direction = "out";
        private List<bool> fromTagsType = new List<bool>();
        //List<AndObject> _andObjectCollection = new List<AndObject>();
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
                        string fromTag = FromTags[i];
                        string value = string.Empty;
                        if (fromTagsType[i])
                        {
                            if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(fromTag))
                            {
                                value = PranaMsg.FIXMessage.ExternalInformation[fromTag].Value;
                            }
                            else if (PranaMsg.FIXMessage.CustomInformation.ContainsKey(fromTag))
                            {
                                value = PranaMsg.FIXMessage.CustomInformation[fromTag].Value;
                            }
                        }
                        else
                        {
                            if (PranaMsg.FIXMessage.InternalInformation.ContainsKey(fromTag))
                            {
                                value = PranaMsg.FIXMessage.InternalInformation[fromTag].Value;
                            }
                        }
                        if (value != string.Empty || !string.IsNullOrEmpty(RepeatingIdentiferTag))
                        {
                            if (base.Direction == Direction.Out)
                            {
                                FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(toTag);
                                if (!string.IsNullOrEmpty(RepeatingIdentiferTag) && fixField != null)
                                {
                                    UpdatedMsgFieldList(PranaMsg, RepeatingIdentiferTag, fromTag, toTag, value);
                                }
                                else
                                {
                                    PranaMsg.FIXMessage.CustomInformation.AddField(toTag, value);
                                }
                            }
                            else
                            {
                                FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(toTag);
                                if (fixField != null)
                                {
                                    if (!string.IsNullOrEmpty(RepeatingIdentiferTag))
                                    {
                                        UpdatedMsgFieldList(PranaMsg, RepeatingIdentiferTag, fromTag, toTag, value);
                                    }
                                    if (fixField.IsExternal)
                                    {
                                        PranaMsg.FIXMessage.ExternalInformation.AddField(toTag, value);
                                    }
                                    else
                                    {
                                        PranaMsg.FIXMessage.InternalInformation.AddField(toTag, value);
                                    }
                                }
                                else
                                {
                                    PranaMsg.FIXMessage.CustomInformation.AddField(toTag, value);
                                }
                            }
                            i++;
                        }
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
                FromTags = xmlNodeItem.Attributes["FromTags"].Value.Split(',');
                int j = 0;
                foreach (string FromTag in FromTags)
                {
                    FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(FromTag);
                    if (fixField != null)
                    {
                        fromTagsType.Add(fixField.IsExternal);
                    }
                    else
                    {
                        fromTagsType.Add(false);
                    }
                    j++;

                }
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
