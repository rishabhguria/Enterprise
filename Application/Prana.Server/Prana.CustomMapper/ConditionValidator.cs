using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Prana.CustomMapper
{
    class ConditionValidator
    {
        public static List<AndObject> LoadRule(string conditionString)
        {
            List<AndObject> _andObjectCollection = new List<AndObject>();

            if (conditionString == string.Empty)
            {
                return _andObjectCollection;
            }
            else
            {
                conditionString = conditionString.Replace(" AND ", "+");
                conditionString = conditionString.Replace("!=", "!");
                conditionString = conditionString.Replace("?", "? ");
                conditionString = conditionString.Replace("!?", "~ ");
                conditionString = conditionString.Replace("==", "#");
                string[] andStringArray = conditionString.Split('+');
                foreach (string andString in andStringArray)
                {
                    AndObject andObj = new AndObject();
                    string andTrimmedString = andString.Trim();
                    string[] orConditions = null;
                    char seperator = '=';
                    if (andTrimmedString.Contains("!"))
                    {
                        seperator = '!';
                    }
                    else if (andTrimmedString.Contains("="))
                    {
                        seperator = '=';
                    }
                    else if (andTrimmedString.Contains("?"))
                    {
                        seperator = '?';
                    }
                    else if (andTrimmedString.Contains("~"))
                    {
                        seperator = '~';
                    }
                    else if (andTrimmedString.Contains("#"))
                    {
                        seperator = '#';
                    }

                    orConditions = andTrimmedString.Split(seperator);

                    string[] orStringTags = orConditions[0].Split(',');
                    string[] orStringValues = orConditions[1].Split(',');
                    for (int i = 0; i < orStringTags.Length; i++)
                    {
                        OrObject or = new OrObject();
                        if (or.LoadOrObject(orStringTags[i].Trim(), orStringValues[i].Trim(), seperator))
                        {
                            andObj.OrCondictions.Add(or);
                        }
                        else
                        {
                            return null;
                        }

                    }
                    _andObjectCollection.Add(andObj);
                }
                return _andObjectCollection;
            }
        }
        public static bool ValidateRule(PranaMessage PranaMsg, List<AndObject> andRuleCollection)
        {
            bool result = true;
            foreach (AndObject andObj in andRuleCollection)
            {
                if (!andObj.EqualRuleValid(PranaMsg))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Validates a collection of message fields against a list of AND rules.
        /// </summary>
        public static bool ValidateRule(PranaMessage pranaMsg, MessageFieldCollection msgFields, List<AndObject> andRuleCollection, MessageField repeatingGroupField)
        {
            try
            {
                foreach (AndObject andObj in andRuleCollection)
                {
                    if (!andObj.EqualRuleValid(pranaMsg, msgFields, repeatingGroupField))
                        return false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return true;
        }
    }
    class AndObject
    {
        public List<OrObject> OrCondictions = new List<OrObject>();
        public bool EqualRuleValid(PranaMessage PranaMsg)
        {
            bool result = false;
            foreach (OrObject orObj in OrCondictions)
            {
                if (orObj.Validate(PranaMsg))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Validates a rule based on the provided message fields using OR conditions.
        /// </summary>
        public bool EqualRuleValid(PranaMessage pranaMsg, MessageFieldCollection msgFields, MessageField repeatingGroupField)
        {
            try
            {
                foreach (OrObject orObj in OrCondictions)
                {
                    if (orObj.Validate(pranaMsg, msgFields, repeatingGroupField))
                        return true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }
    }
    class OrObject
    {
        string _tag = string.Empty;
        string _requiredValue = string.Empty;
        //bool _equalCheck = true;
        char _seperator = '=';
        bool _isExternal = true;

        public string Tag
        {
            get { return _tag; }
        }

        public bool LoadOrObject(string tag, string value, char seperator)
        {
            bool shouldLoad = false;
            try
            {
                _tag = tag;
                _requiredValue = value;
                _seperator = seperator;
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagValue(_tag);
                if (fixfield != null)
                {                 
                    _isExternal = fixfield.IsExternal;
                    shouldLoad = true;
                }
                else
                {
                    //throw new Exception("Tag="+_tag +" is not  a valid tag");
                    shouldLoad = true;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return shouldLoad;
        }
        public bool Validate(PranaMessage PranaMsg)
        {
            try
            {
                bool result = false;
                switch (_seperator)
                {
                    case '=':
                        result = EqualRuleValid(GetValue(PranaMsg));
                        break;
                    case '#':
                        result = ExactEqualRuleValid(GetValue(PranaMsg));
                        break;
                    case '!':
                        result = NotEqualRuleValid(GetValue(PranaMsg));
                        break;
                    case '?':
                        result = DoesTagExist(PranaMsg);
                        break;
                    case '~':
                        result = !DoesTagExist(PranaMsg);
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                return false;
            }
        }

        /// <summary>
        /// Validates a rule based on the provided message fields.
        /// </summary>
        public bool Validate(PranaMessage pranaMsg, MessageFieldCollection msgFields, MessageField repeatingGroupField)
        {
            bool result = false;
            try
            {               
                switch (_seperator)
                {
                    case '=':
                        result = EqualRuleValid(GetValue(msgFields, repeatingGroupField));
                        break;
                    case '#':
                        result = ExactEqualRuleValid(GetValue(msgFields, repeatingGroupField));
                        break;
                    case '!':
                        result = NotEqualRuleValid(GetValue(msgFields, repeatingGroupField));
                        break;
                    case '?':
                        result = DoesTagExist(msgFields, repeatingGroupField);
                        break;
                    case '~':
                        result = !DoesTagExist(msgFields, repeatingGroupField);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return result;
        }

        private string GetValue(PranaMessage PranaMsg)
        {
            string value = "";
            if (_isExternal)
            {
                if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(_tag))
                    value = PranaMsg.FIXMessage.ExternalInformation[_tag].Value;
            }
            else
            {
                if (PranaMsg.FIXMessage.InternalInformation.ContainsKey(_tag))
                    value = PranaMsg.FIXMessage.InternalInformation[_tag].Value;
            }
            return value;
        }

        /// <summary>
        /// Retrieves the value associated with a specific tag from the message fields.
        /// </summary>
        private string GetValue(MessageFieldCollection msgFields, MessageField repeatingGroupField)
        {
            try
            {             
                if(repeatingGroupField.Tag == _tag) 
                    return repeatingGroupField.Value;
                else if (msgFields.ContainsKey(_tag))
                    return msgFields[_tag].Value;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return string.Empty;
        }

        private bool EqualRuleValid(string value)
        {

            if (Regex.IsMatch(value, _requiredValue, RegexOptions.IgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ExactEqualRuleValid(string value)
        {
            if (value.Equals(_requiredValue.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool DoesTagExist(PranaMessage PranaMsg)
        {

            if (_isExternal)
            {
                if (PranaMsg.FIXMessage.ExternalInformation.ContainsKey(_tag))
                    return true;
            }
            else
            {
                if (PranaMsg.FIXMessage.InternalInformation.ContainsKey(_tag))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the specified tag exists in the message fields.
        /// </summary>
        private bool DoesTagExist(MessageFieldCollection msgFields, MessageField repeatingGroupField)
        {
            try
            {
                return repeatingGroupField.Tag == _tag || msgFields.ContainsKey(_tag);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return false;
        }

        private bool NotEqualRuleValid(string value)
        {
            if (_requiredValue != value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
