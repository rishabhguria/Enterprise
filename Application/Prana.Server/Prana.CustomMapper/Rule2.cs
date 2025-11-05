using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Xml;
namespace Prana.CustomMapper
{
    class Rule2 : Rule
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
                    RuleParameters rule2Parameters = new Rule2ParametersNew();
                    if (rule2Parameters.CreateRules(xmlNodeItem))
                    {
                        DirectionListOfRules.Add(rule2Parameters);
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
    //struct Rule3Parameters
    //{
    //    public string[] tags ;
    //    public string[] types ;
    //    public string[] values ;
    //    public string tag ;
    //    public string value ;
    //}
    class Rule2ParametersNew : RuleParameters
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
                                    PranaMsg.FIXMessage.ExternalInformation.AddField(toTag, toTagsValues[i]);
                                }
                                else
                                {
                                    PranaMsg.FIXMessage.InternalInformation.AddField(toTag, toTagsValues[i]);
                                }
                            }
                            else
                            {
                                PranaMsg.FIXMessage.CustomInformation.AddField(toTag, toTagsValues[i]);
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
                //direction = xmlNodeItem.Attributes["Direction"].Value;
                //string conditionString = xmlNodeItem.Attributes["ConditionString"].Value.Trim();
                //_andObjectCollection = ConditionValidator.LoadRule(conditionString);
                //if (_andObjectCollection == null)
                //  return false;
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
