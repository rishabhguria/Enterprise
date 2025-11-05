using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;

namespace Prana.Fix.FixDictionary
{
    public class FixDictionaryHelper
    {
        static readonly object _locker = new object();
        static Dictionary<string, FixFields> _fixmasterDictionary = null;
        static Dictionary<string, FixFields> _fixNameDictionary = null;

        public static Dictionary<string, Dictionary<string, RepeatingFixField>> RepeatingGroupDictionary
        {
            get
            {
                return FixMessageValidator.RepeatingGroupDictionary;
            }
        }

        public static Dictionary<string, Dictionary<string, string>> RepeatingFieldIdentifierTagMappings
        {
            get
            {
                return FixMessageValidator.RepeatingFieldIdentifierTagMappings;
            }
        }

        public static FixFields GetTagFieldByTagValue(string key)
        {
            lock (_locker)
            {
                if (_fixmasterDictionary.ContainsKey(key))
                    return _fixmasterDictionary[key];
                else
                    return null;
            }
        }
        public static FixFields GetTagFieldByTagName(string fieldName)
        {
            lock (_locker)
            {
                if (_fixNameDictionary.ContainsKey(fieldName))
                    return _fixNameDictionary[fieldName];
                else
                    return null;
            }
        }
        public FixDictionaryHelper()
        {

        }
        public static void LoadFixDictionary()
        {
            try
            {
                lock (_locker)
                {
                    _fixmasterDictionary = new Dictionary<string, FixFields>();
                    _fixNameDictionary = new Dictionary<string, FixFields>();
                    string xmlpath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\xmls";
                    FillFromNewFixFile(xmlpath + "\\Fields.xml", true);
                    FillFromNewFixFile(xmlpath + "\\PranaInternalFixTags.xml", false);
                    FillSwapAccountInternalTags();
                    FixMessageValidator.LoadMessageTypes(xmlpath);
                    FixMessageValidator.LoadRepeatingGroupFields(xmlpath);
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



        static void FillFromNewFixFile(string path, bool isExternal)
        {
            try
            {
                XmlTextReader myxmlreader = new XmlTextReader(path);
                FixFields newfixparams = null;
                while (myxmlreader.Read())
                {
                    myxmlreader.MoveToContent();

                    if (myxmlreader.NodeType == XmlNodeType.EndElement)
                    {
                        if (myxmlreader.Name == "Fields")
                        {
                            _fixmasterDictionary.Add(newfixparams.Tag, newfixparams);
                            if (_fixNameDictionary.ContainsKey(newfixparams.FieldName))
                            {
                                _fixNameDictionary.Remove(newfixparams.FieldName);
                            }
                            _fixNameDictionary.Add(newfixparams.FieldName, newfixparams);
                        }
                    }

                    if (myxmlreader.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        switch (myxmlreader.Name)
                        {
                            case "Fields":
                                newfixparams = new FixFields();
                                newfixparams.IsExternal = isExternal;
                                break;
                            case "Tag":
                                newfixparams.Tag = myxmlreader.ReadString();
                                break;
                            case "FieldName":
                                newfixparams.FieldName = myxmlreader.ReadString();
                                break;
                            case "Type":
                                newfixparams.Type = myxmlreader.ReadString();
                                break;
                            case "LenRefers":
                                newfixparams.LenRefers = myxmlreader.ReadString();
                                break;
                            case "RuleList":

                                XmlDocument xmldoc = new XmlDocument();
                                xmldoc.LoadXml(myxmlreader.ReadOuterXml());
                                newfixparams.ValidationRule = FixValidationRuleLoader.LoadRule(xmldoc.SelectSingleNode("RuleList"));
                                break;
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
        /// Fills the swap account internal tags.
        /// </summary>
        private static void FillSwapAccountInternalTags()
        {
            try
            {
                List<int> swapAccounts = CachedDataManager.GetSwapAccounts();
                FixFields newfixparams = null;
                foreach (int accountId in swapAccounts)
                {
                    newfixparams = new FixFields();
                    newfixparams.IsExternal = false;
                    newfixparams.Tag = "Account_" + accountId;
                    newfixparams.FieldName = "Account_" + accountId;
                    newfixparams.Type = "string";

                    _fixmasterDictionary.Add(newfixparams.Tag, newfixparams);
                    if (_fixNameDictionary.ContainsKey(newfixparams.FieldName))
                    {
                        _fixNameDictionary.Remove(newfixparams.FieldName);
                    }
                    _fixNameDictionary.Add(newfixparams.FieldName, newfixparams);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }



        public static DataTable GetAllNames()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Tag");
            foreach (KeyValuePair<string, FixFields> item in _fixmasterDictionary)
            {
                dt.Rows.Add(new object[] { item.Value.FieldName, item.Key });
            }
            return dt;
        }
    }
}


