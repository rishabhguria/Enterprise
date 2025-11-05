using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Global.Utilities;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Prana.Fix.FixDictionary
{
    public class FixMessageValidator
    {
        static Dictionary<string, MessgeType> _dictMessageTypes = new Dictionary<string, MessgeType>();
        static Dictionary<string, string> _msgTypeIDMapping = new Dictionary<string, string>();
        //Dictionary<string, MessageContentList> _dictMessageContent = new Dictionary<string, MessageContentList>();
        static Dictionary<string, Dictionary<string, RepeatingFixField>> _dictMsgOrderedFixField = new Dictionary<string, Dictionary<string, RepeatingFixField>>();
        static Dictionary<string, Dictionary<string, string>> _repeatingFieldIdentifierTagMappings = new Dictionary<string, Dictionary<string, string>>();

        internal static Dictionary<string, Dictionary<string, RepeatingFixField>> RepeatingGroupDictionary
        {
            get
            {
                return _dictMsgOrderedFixField;
            }
        }

        internal static Dictionary<string, Dictionary<string, string>> RepeatingFieldIdentifierTagMappings
        {
            get
            {
                return _repeatingFieldIdentifierTagMappings;
            }
        }

        public static void LoadMessageTypes(string path)
        {

            try
            {
                _dictMessageTypes = new Dictionary<string, MessgeType>();
                _msgTypeIDMapping = new Dictionary<string, string>();
                string pathMsgTypes = path + "\\MsgType.xml";
                string pathMsgContents = path + "\\MsgContents.xml";
                XmlDocument xmlDocMsgTypes = new XmlDocument();
                xmlDocMsgTypes.Load(pathMsgTypes);
                XmlNodeList xmlNodes = xmlDocMsgTypes.SelectNodes("dataroot/MsgType");

                foreach (XmlNode xmlNode in xmlNodes)
                {
                    MessgeType msgType = new MessgeType();
                    msgType.MsgType = xmlNode.SelectSingleNode("MsgType").InnerText.Trim();
                    msgType.MsgID = xmlNode.SelectSingleNode("MsgID").InnerText.Trim();
                    _dictMessageTypes.Add(msgType.MsgType, msgType);
                    _msgTypeIDMapping.Add(msgType.MsgID, msgType.MsgType);
                }

                XmlDocument xmlDocMsgContents = new XmlDocument();
                xmlDocMsgContents.Load(pathMsgContents);
                XmlNodeList xmlNodesMsgContents = xmlDocMsgContents.SelectNodes("dataroot/MsgContents");

                foreach (XmlNode xmlNode in xmlNodesMsgContents)
                {
                    MessageContent messageContent = new MessageContent();
                    messageContent.Tag = xmlNode.SelectSingleNode("TagText").InnerText.Trim();
                    messageContent.Reqd = Convert.ToBoolean(int.Parse(xmlNode.SelectSingleNode("Reqd").InnerText.Trim()));
                    // Position tag was already there so loaded in memory to use sorting of field based on this
                    messageContent.Position = Convert.ToInt32(xmlNode.SelectSingleNode("Position").InnerText.Trim());
                    string msgID = xmlNode.SelectSingleNode("MsgID").InnerText.Trim();

                    if (_msgTypeIDMapping.ContainsKey(msgID))
                    {
                        string msgType = _msgTypeIDMapping[msgID];
                        // As there could be different position for each tag in different messagetype so cloned the object for proper information
                        // Also this required to convert all underlying object definition to be [serializable]
                        FixFields fixField = DeepCopyHelper.Clone(FixDictionaryHelper.GetTagFieldByTagValue(messageContent.Tag));
                        if (fixField != null)
                        {
                            fixField.Position = messageContent.Position;
                            if (messageContent.Reqd)
                            {
                                _dictMessageTypes[msgType].MsgContentList.AddReqFields(fixField);
                            }
                            else
                            {
                                _dictMessageTypes[msgType].MsgContentList.AddOptFields(fixField);
                            }
                        }


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

        /// <summary>
        /// Loads repeating group fields from an XML file.
        /// </summary>
        /// <param name="path">The path to the directory containing MsgContents.xml.</param>
        public static void LoadRepeatingGroupFields(string path)
        {
            try
            {
                _dictMsgOrderedFixField = new Dictionary<string, Dictionary<string, RepeatingFixField>>();

                string pathMsgContents = path + "\\MsgContents.xml";
                XmlDocument xmlDocMsgContents = new XmlDocument();
                xmlDocMsgContents.Load(pathMsgContents);
                XmlNodeList xmlNodesMsgContents = xmlDocMsgContents.SelectNodes("dataroot/MsgContents");

                int i = 0;
                RepeatingFixField prevRepeatingFixField = new RepeatingFixField();
                while (i < xmlNodesMsgContents.Count)
                {
                    XmlNode xmlNode = xmlNodesMsgContents[i];
                    RepeatingFixField repeatingFixField = new RepeatingFixField();

                    // Extract the tag, message ID, and indent from the XML node
                    repeatingFixField.Tag = xmlNode.SelectSingleNode("TagText").InnerText.Trim();
                    string msgID = xmlNode.SelectSingleNode("MsgID").InnerText.Trim();
                    int indent = Convert.ToInt32(xmlNode.SelectSingleNode("Indent").InnerText.Trim());

                    if (_msgTypeIDMapping.ContainsKey(msgID))
                    {
                        string msgType = _msgTypeIDMapping[msgID];

                        if (!_dictMsgOrderedFixField.ContainsKey(msgType))
                        {
                            _dictMsgOrderedFixField[msgType] = new Dictionary<string, RepeatingFixField>();
                        }
                        else
                        {
                            FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(repeatingFixField.Tag);
                            if (fixField != null)
                            {
                                // Handle repeating groups
                                if (indent > 0)
                                {
                                    prevRepeatingFixField.IsRepeatingTag = true;
                                    i = AddRepeatingGroup(i, xmlNodesMsgContents, prevRepeatingFixField, msgID, indent);
                                    _dictMsgOrderedFixField[msgType].Add(prevRepeatingFixField.Tag, prevRepeatingFixField);
                                    continue;
                                }
                            }
                        }
                    }
                    i++;
                    prevRepeatingFixField = repeatingFixField;
                }
                CreateRepeatingFieldIdentifierTagMapping();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            finally
            {
                // not required 
                _msgTypeIDMapping = null;
            }
        }

        /// <summary>
        /// Recursively adds FIX fields to a repeating group based on XML nodes.
        /// </summary>
        /// <param name="i">Current index in the XML node list.</param>
        /// <param name="xmlNodesMsgContents">List of XML nodes representing FIX fields.</param>
        /// <param name="parentField">Repeating group to populate.</param>
        /// <param name="parentMsgID">Parent message ID.</param>
        /// <param name="parentIndent">Parent indent level for grouping.</param>
        /// <returns>The updated index after processing.</returns>
        private static int AddRepeatingGroup(int i, XmlNodeList xmlNodesMsgContents, RepeatingFixField parentField, string parentMsgID, int parentIndent)
        {
            try
            {
                RepeatingFixField prevRepeatingFixField = new RepeatingFixField();
                while (i < xmlNodesMsgContents.Count)
                {
                    XmlNode xmlNode = xmlNodesMsgContents[i];
                    RepeatingFixField repeatingFixField = new RepeatingFixField();

                    // Extract the tag, message ID, and indent from the XML node
                    repeatingFixField.Tag = xmlNode.SelectSingleNode("TagText").InnerText.Trim();
                    string msgID = xmlNode.SelectSingleNode("MsgID").InnerText.Trim();
                    int indent = Convert.ToInt32(xmlNode.SelectSingleNode("Indent").InnerText.Trim());

                    // Check if the current node belongs to the same repeating group
                    if (parentMsgID != msgID || parentIndent > indent)
                        return i;

                    FixFields fixField = FixDictionaryHelper.GetTagFieldByTagValue(repeatingFixField.Tag);
                    if (fixField != null)
                    {
                        // Handle nested repeating groups
                        if (indent > parentIndent)
                        {
                            prevRepeatingFixField.IsRepeatingTag = true;
                            i = AddRepeatingGroup(i, xmlNodesMsgContents, prevRepeatingFixField, msgID, indent);
                            continue;
                        }
                        else
                        {
                            // Add the field detail to the repeating group
                            parentField.RepeatingFixFields.Add(repeatingFixField.Tag, repeatingFixField);
                        }
                    }
                    i++;
                    prevRepeatingFixField = repeatingFixField;
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
            return xmlNodesMsgContents.Count;
        }

        public static string ValidateMessage(PranaMessage pranaMessage)
        {

            try
            {
                string msgType = pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagMsgType].Value;
                if (_dictMessageTypes.ContainsKey(msgType))
                {
                    return _dictMessageTypes[msgType].MsgContentList.ValidateMessage(pranaMessage);
                }
                else
                {
                    throw new Exception("Message type Not Supported");
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
            return string.Empty;
        }

        public static List<FixFields> GetRequiredFields(string msgType)
        {
            if (_dictMessageTypes.ContainsKey(msgType))
            {
                return _dictMessageTypes[msgType].MsgContentList.RequiredFields;
            }
            else
                return null;
        }

        /// <summary>
        /// This method returns the sorted data based on provided comparer
        /// </summary>
        /// <param name="msgType">MessageType for which data is required</param>
        /// <param name="fixFieldComparer">Comparer to be used while sorting</param>
        /// <returns>List of MessageFields in sorted manner</returns>
        internal static List<FixFields> GetSortedFields(string msgType, IComparer<FixFields> fixFieldComparer)
        {
            if (_dictMessageTypes.ContainsKey(msgType))
            {
                List<FixFields> fixFields = new List<FixFields>();
                fixFields.AddRange(_dictMessageTypes[msgType].MsgContentList.RequiredFields);
                fixFields.AddRange(_dictMessageTypes[msgType].MsgContentList.OptionalFields);

                fixFields.Sort(fixFieldComparer);
                return fixFields;

            }
            else
                return null;
        }

        /// <summary>
        /// Retrieves the ordered list of fields for a specific repeating group.
        /// </summary>
        /// <param name="RepeatingIdentifierTag"></param>
        /// <returns>List of field names in the repeating group, or null if not found.</returns>
        public static RepeatingFixField GetOrderedRepeatingGroupFields(string msgType, string repeatingIdentifierTag)
        {
            RepeatingFixField repeatingFixField = null;
            try
            {
                if (_dictMsgOrderedFixField.ContainsKey(msgType))
                {
                    if (_dictMsgOrderedFixField[msgType].ContainsKey(repeatingIdentifierTag))
                    {
                        repeatingFixField = _dictMsgOrderedFixField[msgType][repeatingIdentifierTag];
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return repeatingFixField;
        }

        /// <summary>
        /// Retrieves the ordered list of fields for a specific repeating group.
        /// </summary>
        /// <param name="RepeatingIdentifierTag"></param>
        /// <returns>List of field names in the repeating group, or null if not found.</returns>
        public static RepeatingFixField GetOrderedRepeatingGroupFields(string msgType, string repeatingIdentifierTag, string childTag)
        {
            RepeatingFixField repeatingFixField = null;
            try
            {
                RepeatingFixField parentRepeatingFields = GetOrderedRepeatingGroupFields(msgType, repeatingIdentifierTag);
                if(parentRepeatingFields != null)
                    repeatingFixField = FindRepeatingGroup(parentRepeatingFields, childTag);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return repeatingFixField;
        }

        /// <summary>
        /// Recursively finds the repeating group by its tag within the parent group.
        /// </summary>     
        private static RepeatingFixField FindRepeatingGroup(RepeatingFixField repeatingFixField, string childTag)
        {
            try
            {
                if (repeatingFixField.RepeatingFixFields.ContainsKey(childTag))
                {
                    return repeatingFixField;
                }
                else
                {
                    foreach (var field in repeatingFixField.RepeatingFixFields)
                    {
                        if (field.Value.IsRepeatingTag)
                        {
                            RepeatingFixField result = FindRepeatingGroup(field.Value, childTag);
                            if (result != null) 
                                return result;
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
            return null;
        }

        /// <summary>
        /// This method is to create reverse mapping of fields with its base parent identifier tag
        /// </summary>
        private static void CreateRepeatingFieldIdentifierTagMapping()
        {
            try
            {
                foreach(var msgType in _dictMsgOrderedFixField.Keys)
                {
                    if (!_repeatingFieldIdentifierTagMappings.ContainsKey(msgType))
                    {
                        _repeatingFieldIdentifierTagMappings.Add(msgType, new Dictionary<string, string>());
                    }
                    foreach(var repeatingIdentiferTag in _dictMsgOrderedFixField[msgType].Keys)
                    {
                        Queue<RepeatingFixField> queue = new Queue<RepeatingFixField>();   
                        queue.Enqueue(_dictMsgOrderedFixField[msgType][repeatingIdentiferTag]);
                        while(queue.Count > 0)
                        {
                            var repeatingFixField = queue.Dequeue();
                            foreach (var field in repeatingFixField.RepeatingFixFields.Values)
                            {
                                if (field.IsRepeatingTag)
                                {
                                    queue.Enqueue(field);
                                }
                                else
                                {
                                    if(!_repeatingFieldIdentifierTagMappings[msgType].ContainsKey(field.Tag))
                                        _repeatingFieldIdentifierTagMappings[msgType].Add(field.Tag, repeatingIdentiferTag);
                                }
                            }
                        }
                    }
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
