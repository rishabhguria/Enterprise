using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Prana.CustomMapper
{
    public abstract class Rule
    {
        public abstract void ApplyRule(PranaMessage PranaMsg, Direction direction);
        public abstract void LoadRule(XmlNode node);
        public abstract void ApplyRule(List<PranaMessage> PranaMsg, Direction direction);
    }
    public class RuleParameters
    {
        private Direction direction = Direction.In;
        List<AndObject> _andObjectCollection = new List<AndObject>();
        private Dictionary<string, List<AndObject>> _identifierTagWiseRules = new Dictionary<string, List<AndObject>>();
        private int executeOnReloadRule = 1;

        protected string RepeatingIdentiferTag { get; set; }

        public int ExecuteOnReloadRule
        {
            get { return executeOnReloadRule; }
        }

        public Direction Direction
        {
            get { return direction; }
        }

        public virtual void SetValues(PranaMessage PranaMsg)
        {
        }

        public virtual void SetValues(List<PranaMessage> PranaMsgList)
        {
        }

        public bool IsRuleValid(PranaMessage pranaMsg, string repeatingTag = "")
        {
            //Assign RepeatingIdentiferTag value based on repeatingTag
            if (!string.IsNullOrEmpty(repeatingTag) && FixDictionaryHelper.RepeatingFieldIdentifierTagMappings.ContainsKey(pranaMsg.MessageType))
            {               
                RepeatingIdentiferTag = FixDictionaryHelper.RepeatingFieldIdentifierTagMappings[pranaMsg.MessageType].ContainsKey(repeatingTag) ?
                                        FixDictionaryHelper.RepeatingFieldIdentifierTagMappings[pranaMsg.MessageType][repeatingTag] :  string.Empty;
            }

            if (string.IsNullOrEmpty(RepeatingIdentiferTag))
            {
                return ConditionValidator.ValidateRule(pranaMsg, _andObjectCollection);
            }
            else
            {
                _identifierTagWiseRules = CreateRuleDictionaryIdentifierWise(RepeatingIdentiferTag, pranaMsg.MessageType);
                bool result = _identifierTagWiseRules.ContainsKey("EI") ? ConditionValidator.ValidateRule(pranaMsg, _identifierTagWiseRules["EI"]) : true;
                return result;
            }
        }

        public virtual bool CreateRules(XmlNode xmlNodeItem)
        {
            try
            {
                direction = (Direction)int.Parse(xmlNodeItem.Attributes["Direction"].Value);
                if (null != xmlNodeItem.Attributes["ExecuteOnReloadRule"])
                {
                    executeOnReloadRule = int.Parse(xmlNodeItem.Attributes["ExecuteOnReloadRule"].Value);
                }
                string conditionString = xmlNodeItem.Attributes["ConditionString"].Value.Trim();
                _andObjectCollection = ConditionValidator.LoadRule(conditionString);
                if (_andObjectCollection == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Updates a repeating group within a FIX message.
        /// </summary>
        /// <param name="pranaMsg">The PranaMessage containing the FIX message.</param>
        /// <param name="repeatingGroupTag">The tag of the repeating group.</param>
        /// <param name="fromTag">The source tag for the value to update.</param>
        /// <param name="toTag">The target tag for the update.</param>
        /// <param name="value">The new value to set.</param>
        public void UpdatedMsgFieldList(PranaMessage pranaMsg, string repeatingGroupTag, string fromTag ,string toTag, string value)
        {
            try
            {
                // Get the ordered repeating fields for the specified repeating group
                RepeatingFixField repeatingFieldDictionary = FixMessageValidator.GetOrderedRepeatingGroupFields(pranaMsg.MessageType, RepeatingIdentiferTag);

                // Find the target message field collection
                List<RepeatingMessageFieldCollection> updatedMsgFieldList = new List<RepeatingMessageFieldCollection>();
                if (repeatingFieldDictionary != null && pranaMsg.FIXMessage.ChildGroups.ContainsKey(repeatingGroupTag))
                    updatedMsgFieldList = FindTargetMessageFieldCollection(pranaMsg.FIXMessage.ChildGroups[repeatingGroupTag], repeatingGroupTag, toTag, repeatingFieldDictionary.RepeatingFixFields);

                if (updatedMsgFieldList != null && updatedMsgFieldList.Count > 0)
                {
                    // Get the ordered repeating fields for the target tag
                    RepeatingFixField orderdedRepeatingFields = FixMessageValidator.GetOrderedRepeatingGroupFields(pranaMsg.MessageType, repeatingGroupTag, toTag);

                    if (orderdedRepeatingFields != null)
                    {
                        foreach (RepeatingMessageFieldCollection msgFields in updatedMsgFieldList)
                        {
                            List<MessageField> updatedMsgFields = msgFields.MessageFields;
                            string fieldValue = value;
                            // If value is empty, use the value from the source tag
                            if (string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(fromTag) && msgFields.ContainsKey(fromTag))
                                fieldValue = msgFields[fromTag].Value;

                            if (!string.IsNullOrEmpty(fieldValue))
                            {
                                // Update the group with the new value
                                FixRepeatingFieldHelper.UpdateGroup(msgFields, orderdedRepeatingFields, fieldValue, toTag);
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

        /// <summary>
        /// This method is to get the filtered MessageFieldCollections after apply condtion string from rules
        /// </summary>
        /// <param name="repeatingGroup"></param>
        /// <param name="conditionList"></param>
        /// <returns></returns>
        private List<RepeatingMessageFieldCollection> GetFilteredMessageFieldCollection(RepeatingGroup repeatingGroup, List<AndObject> conditionList)
        {
            try
            {
                if(conditionList == null)
                {
                    return repeatingGroup.MessageFields;
                }
                List<RepeatingMessageFieldCollection> filteredMessgeFieldCollection = new List<RepeatingMessageFieldCollection>();
                foreach (var messageFieldCollection in repeatingGroup.MessageFields)
                {
                    if(ConditionValidator.ValidateRule(new PranaMessage(), messageFieldCollection, conditionList, repeatingGroup.CountField))
                    {
                        filteredMessgeFieldCollection.Add(messageFieldCollection);
                    }
                }
                return filteredMessgeFieldCollection;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// This method is to find the target MessageFieldCollection where we need to apply the rule
        /// </summary>
        /// <param name="repeatingGroup"></param>
        /// <param name="repeatingIdentifierTag"></param>
        /// <param name="targetTag"></param>
        /// <param name="repeatingFieldDictionary"></param>
        /// <returns></returns>
        public List<RepeatingMessageFieldCollection> FindTargetMessageFieldCollection(RepeatingGroup repeatingGroup, string repeatingIdentifierTag, string targetTag, Dictionary<string, RepeatingFixField> repeatingFieldDictionary)
        {
            try
            {
                List<RepeatingMessageFieldCollection> filteredMessageFieldCollection = _identifierTagWiseRules.ContainsKey(repeatingIdentifierTag) ?
                    GetFilteredMessageFieldCollection(repeatingGroup, _identifierTagWiseRules[repeatingIdentifierTag]) : repeatingGroup.MessageFields;

                if(filteredMessageFieldCollection != null && filteredMessageFieldCollection.Count > 0)
                {
                    if (repeatingFieldDictionary.ContainsKey(targetTag))
                    {
                        return filteredMessageFieldCollection;
                    }

                    Dictionary<string, Dictionary<string, RepeatingFixField>> childFixFieldDictionary = new Dictionary<string, Dictionary<string, RepeatingFixField>>();
                    foreach (var repeatingField in repeatingFieldDictionary.Values)
                    {
                        if (repeatingField.IsRepeatingTag)
                        {
                            childFixFieldDictionary.Add(repeatingField.Tag, repeatingField.RepeatingFixFields);
                        }
                    }

                    List<SerializableDictionary<string, RepeatingGroup>> childRepeatingGroupCollection = new List<SerializableDictionary<string, RepeatingGroup>>();
                    foreach(var messageFieldCollection in filteredMessageFieldCollection)
                    {
                        if (repeatingGroup.ChildGroups.ContainsKey(messageFieldCollection.ID.ToString()))
                        {
                            childRepeatingGroupCollection.Add(repeatingGroup.ChildGroups[messageFieldCollection.ID.ToString()]);
                        }
                    }

                    List<RepeatingMessageFieldCollection> targetMessageFieldCollections = new List<RepeatingMessageFieldCollection>();
                    foreach(var childRepeatingGroup in childRepeatingGroupCollection)
                    {
                        foreach(var childGroupIdentifier in childRepeatingGroup.Keys)
                        {
                            var targetMessageFields = FindTargetMessageFieldCollection(childRepeatingGroup[childGroupIdentifier],
                                childGroupIdentifier, targetTag, childFixFieldDictionary[childGroupIdentifier]);
                            if (targetMessageFields != null)
                            {
                                targetMessageFieldCollections.AddRange(targetMessageFields);
                            }
                        }
                    }
                    return targetMessageFieldCollections;
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
            return null;
        }

        /// <summary>
        /// This method is to divide the _andObjectCollection into a dictionary where key will be its parent identifier tag
        /// </summary>
        /// <param name="repeatingIdentifierTag"></param>
        /// <returns></returns>
        private Dictionary<string, List<AndObject>> CreateRuleDictionaryIdentifierWise(string repeatingIdentifierTag, string msgType)
        {
            Dictionary<string, List<AndObject>> ruleDictionary = new Dictionary<string, List<AndObject>>();
            try
            {
                Dictionary<string, string> repeatingIdentifierChildWise = new Dictionary<string, string>();

                RepeatingFixField orderedRepeatingFields = FixMessageValidator.GetOrderedRepeatingGroupFields(msgType, repeatingIdentifierTag);

                if (orderedRepeatingFields != null)
                {
                    GetRepeatingIdentifierTags(orderedRepeatingFields.RepeatingFixFields, repeatingIdentifierTag, repeatingIdentifierChildWise);
                }
                foreach (var andObject in _andObjectCollection)
                {
                    string identifierTag = string.Empty;

                    foreach (var orObject in andObject.OrCondictions)
                    {
                        if (repeatingIdentifierChildWise.ContainsKey(orObject.Tag))
                        {
                            identifierTag = repeatingIdentifierChildWise[orObject.Tag];
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(identifierTag))
                    {
                        if (!ruleDictionary.ContainsKey("EI"))
                        {
                            ruleDictionary.Add("EI", new List<AndObject>());
                        }
                        ruleDictionary["EI"].Add(andObject);
                    }
                    else
                    {
                        if (!ruleDictionary.ContainsKey(identifierTag))
                        {
                            ruleDictionary.Add(identifierTag, new List<AndObject>());
                        }
                        ruleDictionary[identifierTag].Add(andObject);
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
            return ruleDictionary;
        }

        /// <summary>
        /// This method is to create a mapped dictionary of repeating identifier tag and its child tags
        /// </summary>
        /// <param name="repeatingFixFieldDictionary"></param>
        /// <param name="repeatingIdentifierTag"></param>
        /// <param name="repeatingIdentifierChildWise"></param>
        private void GetRepeatingIdentifierTags(Dictionary<string, RepeatingFixField> repeatingFixFieldDictionary, string repeatingIdentifierTag, Dictionary<string, string> repeatingIdentifierChildWise)
        {
            try
            {
                foreach(var repeatingFixField in repeatingFixFieldDictionary.Values)
                {
                    if (!repeatingIdentifierChildWise.ContainsKey(repeatingIdentifierTag))
                    {
                        repeatingIdentifierChildWise.Add(repeatingFixField.Tag, repeatingIdentifierTag);
                    }
                    if (repeatingFixField.IsRepeatingTag)
                    {
                        GetRepeatingIdentifierTags(repeatingFixField.RepeatingFixFields, repeatingFixField.Tag, repeatingIdentifierChildWise);
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
    }

    public enum Direction
    {
        In = 0,
        Out = 1
    }
}
