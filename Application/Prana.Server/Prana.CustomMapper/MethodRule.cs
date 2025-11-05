using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
namespace Prana.CustomMapper
{
    class MethodRule : Rule
    {
        DirectionListOfRules DirectionListOfRules = new DirectionListOfRules();

        public override void LoadRule(System.Xml.XmlNode node)
        {
            try
            {
                XmlNodeList xmlNodeList = node.SelectNodes("Condition");
                foreach (XmlNode xmlNodeItem in xmlNodeList)
                {
                    RuleParameters rule1Parameters = new MethodRuleParametersNew();
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
            return;
        }

    }
    class MethodRuleParametersNew : RuleParameters
    {
        MethodInfo methodInfo;
        private object[] parameters;
        private string toTag;
        private string fromTag;
        //private string direction = "out";
        //private List<bool> fromTagsType = new List<bool>();
        //List<AndObject> _andObjectCollection = new List<AndObject>();
        public override void SetValues(PranaMessage PranaMsg)
        {
            try
            {              
                if (base.IsRuleValid(PranaMsg, toTag))
                {
                    string ApplyOnMe = string.Empty;
                    FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(fromTag);
                    
                    if (fixField != null)
                    {                     
                        if (fixField.IsExternal)
                        {
                            if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(fromTag))
                                ApplyOnMe = PranaMsg.FIXMessage.ExternalInformation[fromTag].Value;
                        }
                        else
                        {
                            if (PranaMsg.FIXMessage.InternalInformation.ContainsKey(fromTag))
                                ApplyOnMe = PranaMsg.FIXMessage.InternalInformation[fromTag].Value;
                        }
                    }
                    if (!string.IsNullOrEmpty(RepeatingIdentiferTag))
                    {
                        ApplyOnGroup(ApplyOnMe, PranaMsg);
                    }
                    else if (ApplyOnMe != string.Empty)
                    {
                        string result = methodInfo.Invoke(ApplyOnMe, parameters).ToString();
                        FixFields tofixField = FixDictionaryHelper.GetTagFieldByTagValue(toTag);
                        if (tofixField != null)
                        {
                            if (tofixField.IsExternal)
                            {
                                PranaMsg.FIXMessage.ExternalInformation.AddField(toTag, result);
                            }
                            else
                            {
                                PranaMsg.FIXMessage.InternalInformation.AddField(toTag, result);
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

        private void ApplyOnGroup(string ApplyOnMe, PranaMessage pranaMsg)
        {
            try
            {
                // Get the ordered repeating fields for the specified repeating group
                RepeatingFixField repeatingFieldDictionary = FixMessageValidator.GetOrderedRepeatingGroupFields(pranaMsg.MessageType, RepeatingIdentiferTag);

                if (pranaMsg.FIXMessage.ChildGroups.ContainsKey(RepeatingIdentiferTag))
                {
                    List<RepeatingMessageFieldCollection> updatedMsgFieldList = FindTargetMessageFieldCollection(pranaMsg.FIXMessage.ChildGroups[RepeatingIdentiferTag], RepeatingIdentiferTag, toTag, repeatingFieldDictionary.RepeatingFixFields);

                    if (updatedMsgFieldList != null && updatedMsgFieldList.Count > 0)
                    {
                        RepeatingFixField orderdedRepeatingFields = FixMessageValidator.GetOrderedRepeatingGroupFields(pranaMsg.MessageType, RepeatingIdentiferTag, toTag);

                        if (orderdedRepeatingFields != null)
                        {
                            foreach (RepeatingMessageFieldCollection msgFields in updatedMsgFieldList)
                            {
                                string fieldValue = ApplyOnMe;

                                if (string.IsNullOrEmpty(ApplyOnMe) && !string.IsNullOrEmpty(fromTag) && msgFields.ContainsKey(fromTag))
                                    fieldValue = msgFields[fromTag].Value;

                                if (fieldValue != string.Empty)
                                {
                                    string result = methodInfo.Invoke(fieldValue, parameters).ToString();

                                    // Iterate through fields in the specified order
                                    FixRepeatingFieldHelper.UpdateGroup(msgFields, orderdedRepeatingFields, result, toTag);
                                }
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
        public override bool CreateRules(System.Xml.XmlNode xmlNodeItem)
        {
            try
            {
                toTag = xmlNodeItem.Attributes["ToTag"].Value;
                fromTag = xmlNodeItem.Attributes["FromTag"].Value;
                string methodName = xmlNodeItem.Attributes["MethodName"].Value;

                string[] parametersTypes = xmlNodeItem.Attributes["ParametersTypes"].Value.Split(',');
                Type objectType = typeof(System.String);
                Type[] paramTypes = new Type[parametersTypes.Length];
                for (int i = 0; i < parametersTypes.Length; i++)
                {
                    paramTypes[i] = GetType(parametersTypes[i]);
                }
                parameters = GetObjects(xmlNodeItem.Attributes["Parameters"].Value.Split(','), paramTypes);
                methodInfo = objectType.GetMethod(methodName, paramTypes);
                return base.CreateRules(xmlNodeItem);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return true;
        }
        public Type GetType(string type)
        {
            if (type == "String")
            {
                return typeof(String);
            }
            else if (type == "int")
            {
                return typeof(int);
            }
            else if (type == "char")
            {
                return typeof(char);
            }
            else
            {
                throw new Exception("Undefined Type Found");
            }
        }
        public object[] GetObjects(string[] values, Type[] types)
        {
            object[] objList = new object[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                objList[i] = System.Convert.ChangeType(values[i], types[i]);
            }
            return objList;

        }
    }
}
