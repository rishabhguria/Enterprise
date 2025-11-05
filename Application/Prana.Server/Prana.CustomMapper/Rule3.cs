using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Xml;
namespace Prana.CustomMapper
{
    class Rule3 : Rule
    {


        DirectionListOfRules DirectionListOfRules = new DirectionListOfRules();
        //List<Rule3ParametersNew> _ruleParameters = new List<Rule3ParametersNew>();

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
                    RuleParameters rule3Parameters = new Rule3ParametersNew();
                    if (rule3Parameters.CreateRules(xmlNodeItem))
                    {
                        DirectionListOfRules.Add(rule3Parameters);
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

    class Rule3ParametersNew : RuleParameters
    {
        private string[] deleteTags;

        public override void SetValues(PranaMessage PranaMsg)
        {
            try
            {
                string repeatingTag = deleteTags != null && deleteTags.Length > 0 ? deleteTags[0] : string.Empty;
               
                if (base.IsRuleValid(PranaMsg, repeatingTag))
                {
                    foreach (string toTag in deleteTags)
                    {
                        if (PranaMsg.FIXMessage.RequiredFixFields.Contains(toTag))
                        {
                            PranaMsg.FIXMessage.RequiredFixFields.Remove(toTag);
                        }
                        if (!string.IsNullOrEmpty(RepeatingIdentiferTag))
                        {
                            RemoveTagFromGroup(PranaMsg, toTag, RepeatingIdentiferTag);
                        }
                        if (base.Direction == Direction.Out)
                        {
                            if (PranaMsg.FIXMessage.CustomInformation.ContainsKey(toTag))
                            {
                                PranaMsg.FIXMessage.CustomInformation.RemoveField(toTag);
                            }
                            if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(toTag))
                            {
                                PranaMsg.FIXMessage.ExternalInformation.RemoveField(toTag);
                            }
                            
                        }
                        else
                        {
                            FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(toTag);
                            if (fixField != null)
                            {
                                if (fixField.IsExternal)
                                {
                                    if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(toTag))
                                        PranaMsg.FIXMessage.ExternalInformation.RemoveField(toTag);
                                }
                                else
                                {
                                    if (PranaMsg.FIXMessage.InternalInformation.ContainsKey(toTag))
                                        PranaMsg.FIXMessage.InternalInformation.RemoveField(toTag);
                                }
                            }
                            else
                            {
                                if (PranaMsg.FIXMessage.CustomInformation.ContainsKey(toTag))
                                    PranaMsg.FIXMessage.CustomInformation.RemoveField(toTag);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Removes a specific tag from a repeating group within a PranaMessage.
        /// </summary>
        /// <param name="pranaMsg">The PranaMessage containing the FIX message.</param>
        /// <param name="toTag">The tag to remove from the repeating group.</param>
        /// <param name="repeatingTag">The repeating group tag.</param>
        private void RemoveTagFromGroup(PranaMessage pranaMsg, string toTag, string repeatingTag)
        {
            try
            {
                FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(toTag);
                if (fixField != null)
                {
                    // Get the ordered repeating fields for the specified repeating group
                    RepeatingFixField repeatingFieldDictionary = FixMessageValidator.GetOrderedRepeatingGroupFields(pranaMsg.MessageType, RepeatingIdentiferTag);

                    if (pranaMsg.FIXMessage.ChildGroups.ContainsKey(repeatingTag))
                    {
                        List<RepeatingMessageFieldCollection> updatedMsgFieldList = FindTargetMessageFieldCollection(pranaMsg.FIXMessage.ChildGroups[repeatingTag], repeatingTag, toTag, repeatingFieldDictionary.RepeatingFixFields);

                        if (updatedMsgFieldList != null)
                        {
                            foreach (RepeatingMessageFieldCollection msgFields in updatedMsgFieldList)
                            {
                                if (msgFields.ContainsKey(toTag))
                                    msgFields.RemoveField(toTag);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public override bool CreateRules(System.Xml.XmlNode xmlNodeItem)
        {
            try
            {
                deleteTags = xmlNodeItem.Attributes["DeleteTags"].Value.Split(',');
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
