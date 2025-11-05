using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.CounterPartyRules;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
namespace Prana.CustomMapper
{
    class DllRule : Rule
    {
        DirectionListOfRules DirectionListOfRules = new DirectionListOfRules();
        public override void LoadRule(System.Xml.XmlNode node)
        {
            try
            {
                XmlNodeList xmlNodeList = node.SelectNodes("Condition");
                foreach (XmlNode xmlNodeItem in xmlNodeList)
                {
                    RuleParameters rule1Parameters = new DllRuleParameters();
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

        public override void ApplyRule(List<PranaMessage> PranaMsg, Direction direction)
        {
            try
            {
                DirectionListOfRules.ApplyRule(direction, PranaMsg);
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
    class DllRuleParameters : RuleParameters
    {
        DllBaseRule dllRule = null;
        string toTag = string.Empty;
        public override void SetValues(PranaMessage PranaMsg)
        {          
            if (base.IsRuleValid(PranaMsg, toTag))
            {
                if (dllRule != null)
                {
                    if (!string.IsNullOrEmpty(RepeatingIdentiferTag) && !string.IsNullOrEmpty(toTag))
                    {
                        // Get the ordered repeating fields for the specified repeating group
                        RepeatingFixField repeatingFieldDictionary = FixMessageValidator.GetOrderedRepeatingGroupFields(PranaMsg.MessageType, RepeatingIdentiferTag);
                        if (PranaMsg.FIXMessage.ChildGroups.ContainsKey(RepeatingIdentiferTag))
                        {
                            List<RepeatingMessageFieldCollection> updatedMsgFieldList = FindTargetMessageFieldCollection(PranaMsg.FIXMessage.ChildGroups[RepeatingIdentiferTag], RepeatingIdentiferTag, toTag, repeatingFieldDictionary.RepeatingFixFields);
                            if (updatedMsgFieldList != null)
                            {
                                dllRule.ApplyRule(PranaMsg, updatedMsgFieldList, RepeatingIdentiferTag);
                            }
                        }
                    }
                    else
                    {
                        dllRule.ApplyRule(PranaMsg);
                    }
                }
            }

        }

        public override void SetValues(List<PranaMessage> PranaMsgList)
        {
            foreach (PranaMessage pranaMsg in PranaMsgList)
            {
                if (!base.IsRuleValid(pranaMsg))
                    return;
            }

            if (dllRule != null)
            {
                dllRule.ApplyRule(PranaMsgList);
            }
        }
        public override bool CreateRules(System.Xml.XmlNode xmlNodeItem)
        {
            try
            {
                string dllFullPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + xmlNodeItem.Attributes["DllFullName"].Value;
                if (File.Exists(dllFullPath))
                {
                    SetToTag(xmlNodeItem);
                    string className = xmlNodeItem.Attributes["ClassName"].Value;
                    Assembly assembly = Assembly.LoadFrom(dllFullPath);
                    Type type = assembly.GetType(className);
                    dllRule = (Prana.CounterPartyRules.DllBaseRule)Activator.CreateInstance(type);
                    dllRule.CreateRules(xmlNodeItem);
                    return base.CreateRules(xmlNodeItem);
                }
                return false;
            }

            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return true;
        }

        private void SetToTag(System.Xml.XmlNode xmlNodeItem)
        {
            try
            {
                if (xmlNodeItem.Attributes["Tag"] != null)
                {
                    toTag = xmlNodeItem.Attributes["Tag"].Value;
                }
                else if (xmlNodeItem.Attributes["ToTag"] != null)
                {
                    toTag = xmlNodeItem.Attributes["ToTag"].Value;
                }
                else if(xmlNodeItem.Attributes["ConditionToApply"] != null)
                {
                    string RuleToApply = xmlNodeItem.Attributes["ConditionToApply"].Value.Trim();
                    string[] data = RuleToApply.Split('=');
                    toTag = data[0];
                }                              
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }
    }
}
