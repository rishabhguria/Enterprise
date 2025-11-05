using Prana.BusinessObjects.FIX;
using Prana.Fix.FixDictionary;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Prana.CounterPartyRules
{
    public class RegularExpressionRule : DllBaseRule
    {
        string _regularExpression = "([ |\n|\t|\f][ |\n|\t|\f]+)|([\n]{1})";
        string _replaceWith = " ";
        string _tag = "";
        string _tagValue = "";
        Direction _direction;

        public override void ApplyRule(Prana.BusinessObjects.FIX.PranaMessage msg)
        {
            try
            {
                FixFields fixfield = FixDictionaryHelper.GetTagFieldByTagValue(_tag);
                if (fixfield != null)
                {
                    if (_direction == Direction.Out)
                    {
                        if (msg.FIXMessage.CustomInformation.ContainsKey(_tag))
                        {
                            _tagValue = Regex.Replace(msg.FIXMessage.CustomInformation[_tag].Value, _regularExpression, _replaceWith);
                        }
                        else if (fixfield.IsExternal)
                        {
                            if (msg.FIXMessage.ExternalInformation.ContainsKey(_tag))
                            {
                                _tagValue = Regex.Replace(msg.FIXMessage.ExternalInformation[_tag].Value, _regularExpression, _replaceWith);
                            }
                        }
                        else
                        {
                            if (msg.FIXMessage.InternalInformation.ContainsKey(_tag))
                                _tagValue = Regex.Replace(msg.FIXMessage.InternalInformation[_tag].Value, _regularExpression, _replaceWith);
                        }

                        msg.FIXMessage.CustomInformation.AddField(_tag, _tagValue);
                    }
                    else
                    {
                        if (fixfield.IsExternal)
                        {
                            if (msg.FIXMessage.ExternalInformation.ContainsKey(_tag))
                            {
                                msg.FIXMessage.ExternalInformation[_tag].Value = Regex.Replace(msg.FIXMessage.ExternalInformation[_tag].Value, _regularExpression, _replaceWith);
                            }
                        }
                        else
                        {
                            if (msg.FIXMessage.InternalInformation.ContainsKey(_tag))
                                msg.FIXMessage.InternalInformation[_tag].Value = Regex.Replace(msg.FIXMessage.InternalInformation[_tag].Value, _regularExpression, _replaceWith);
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
                {
                    throw;
                }
            }
        }

        public override void ApplyRule(PranaMessage msg1, List<RepeatingMessageFieldCollection> msg, string repeatingTag)
        {
            try
            {
                foreach (RepeatingMessageFieldCollection msgFields in msg)
                {
                    if (msgFields.ContainsKey(_tag))
                    {
                        msgFields[_tag].Value = Regex.Replace(msgFields[_tag].Value, _regularExpression, _replaceWith);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public override bool CreateRules(System.Xml.XmlNode xmlNodeItem)
        {
            _regularExpression = xmlNodeItem.Attributes["RegularExpression"].Value;
            _replaceWith = xmlNodeItem.Attributes["ReplaceWith"].Value;
            _tag = xmlNodeItem.Attributes["Tag"].Value;
            _direction = (Direction)int.Parse(xmlNodeItem.Attributes["Direction"].Value);
            return true;
        }
    }
}
