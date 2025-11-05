using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using static Prana.BusinessObjects.PranaServerConstants;

namespace Prana.CustomMapper
{
    public static class PranaCustomMapper
    {
        static Dictionary<OriginatorTypeCategory, Dictionary<string, List<Rule>>> _dictCounterPartyRules;

        public static void LoadDictionary()
        {
            try
            {
                #region Load AppSettings for adding dynamic rule
                string xmlFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\AppSettings.xml";

                Dictionary<int, int> counterPartyInfo = new Dictionary<int, int>();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFilePath);

                XmlNodeList counterParties = xmlDocument.SelectNodes("//CounterParty[@OriginatorType='4']");

                foreach (XmlNode counterParty in counterParties)
                {
                    int counterPartyID = int.Parse(counterParty.Attributes["CounterPartyID"].Value);
                    int brokerConnectionType = int.Parse(counterParty.Attributes["BrokerConnectionType"].Value);

                    counterPartyInfo.Add(counterPartyID, brokerConnectionType);
                }
                #endregion

                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\xmls\\CounterPartyCustomMapping.xml";
                _dictCounterPartyRules = new Dictionary<OriginatorTypeCategory, Dictionary<string, List<Rule>>>();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNodeList xmlCounterPartyNodeList = xmlDoc.SelectNodes("Counterparties/CounterParty");

                foreach (XmlNode xmlNode in xmlCounterPartyNodeList)
                {
                    List<Rule> myRulesList = new List<Rule>();
                    XmlNode xmlRule1Node = xmlNode.SelectSingleNode("Rule1");
                    XmlNode xmlRule2Node = xmlNode.SelectSingleNode("Rule2");
                    XmlNode xmlRule3Node = xmlNode.SelectSingleNode("Rule3");
                    XmlNode xmlRule4Node = xmlNode.SelectSingleNode("DefaultRule");
                    XmlNode xmlRuleMethod = xmlNode.SelectSingleNode("MethodRule");
                    XmlNode xmlRule5Node = xmlNode.SelectSingleNode("DllRule");

                    if (xmlRule1Node != null)
                    {
                        Rule rule1 = new Rule1();
                        rule1.LoadRule(xmlRule1Node);
                        myRulesList.Add(rule1);
                    }
                    if (xmlRule2Node != null)
                    {
                        Rule rule2 = new Rule2();
                        rule2.LoadRule(xmlRule2Node);
                        myRulesList.Add(rule2);
                    }
                    if (xmlRule3Node != null)
                    {
                        Rule rule3 = new Rule3();
                        rule3.LoadRule(xmlRule3Node);
                        myRulesList.Add(rule3);
                    }
                    if (xmlRule4Node != null)
                    {
                        DefaultRule rule4 = new DefaultRule();
                        rule4.LoadRule(xmlRule4Node);
                        myRulesList.Add(rule4);
                    }
                    if (xmlRuleMethod != null)
                    {
                        MethodRule methodrule = new MethodRule();
                        methodrule.LoadRule(xmlRuleMethod);
                        myRulesList.Add(methodrule);
                    }
                    if (xmlRule5Node != null)
                    {
                        DllRule dllRule = new DllRule();
                        dllRule.LoadRule(xmlRule5Node);
                        myRulesList.Add(dllRule);
                    }

                    OriginatorTypeCategory originatorTypeCategory = OriginatorTypeCategory.OMS;

                    if (xmlNode.Attributes["OriginatorType"] != null && xmlNode.Attributes["OriginatorType"].Value != null && (OriginatorType)int.Parse(xmlNode.Attributes["OriginatorType"].Value.Trim()) == OriginatorType.Allocation)
                    {
                        originatorTypeCategory = OriginatorTypeCategory.EOD;

                        int counterPartyID = int.Parse(xmlNode.Attributes["ID"].Value);
                        if (counterPartyInfo.ContainsKey(counterPartyID))
                        {
                            XmlDocument tempXmlDoc = new XmlDocument();
                            XmlNode defaultRuleNode = tempXmlDoc.CreateElement("DefaultRule");
                            XmlNode conditionNode = tempXmlDoc.CreateElement("Condition");

                            conditionNode.Attributes.Append(tempXmlDoc.CreateAttribute("Direction")).Value = "0";
                            conditionNode.Attributes.Append(tempXmlDoc.CreateAttribute("ConditionString")).Value = "";
                            conditionNode.Attributes.Append(tempXmlDoc.CreateAttribute("ToTags")).Value = CustomFIXConstants.CUST_TAG_BrokerConnectionType;
                            conditionNode.Attributes.Append(tempXmlDoc.CreateAttribute("ToTagsValues")).Value = counterPartyInfo[counterPartyID].ToString();
                            conditionNode.Attributes.Append(tempXmlDoc.CreateAttribute("Name")).Value = "BrokerConnectionType";

                            defaultRuleNode.AppendChild(conditionNode);
                            tempXmlDoc.AppendChild(defaultRuleNode);

                            DefaultRule defaultRule = new DefaultRule();
                            defaultRule.LoadRule(tempXmlDoc.DocumentElement);
                            myRulesList.Add(defaultRule);
                        }
                    }

                    if (!_dictCounterPartyRules.ContainsKey(originatorTypeCategory))
                    {
                        _dictCounterPartyRules.Add(originatorTypeCategory, new Dictionary<string, List<Rule>>());
                    }

                    if (!_dictCounterPartyRules[originatorTypeCategory].ContainsKey(xmlNode.Attributes["ID"].Value))
                    {
                        _dictCounterPartyRules[originatorTypeCategory].Add(xmlNode.Attributes["ID"].Value, myRulesList);
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

        public static void ApplyRules(PranaMessage pranaMessage, Direction direction, OriginatorTypeCategory originatorTypeCategory = OriginatorTypeCategory.OMS)
        {
            try
            {
                string cpID = string.Empty;
                if (pranaMessage.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CounterPartyID))
                {
                    cpID = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CounterPartyID].Value;
                }
                else
                {
                    cpID = pranaMessage.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_OrigCounterPartyID].Value;
                    direction = Direction.In;
                }

                if (_dictCounterPartyRules.ContainsKey(originatorTypeCategory) && _dictCounterPartyRules[originatorTypeCategory].ContainsKey(cpID))
                {
                    List<Rule> cpRules = _dictCounterPartyRules[originatorTypeCategory][cpID];
                    foreach (Rule rule in cpRules)
                    {
                        rule.ApplyRule(pranaMessage, direction);
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
        /// Applies the rules for manual order send.
        /// </summary>
        /// <param name="pranaMessage">The prana message.</param>
        public static void ApplyRulesForManualOrderSend(PranaMessage pranaMessage, Direction direction = Direction.Out, OriginatorTypeCategory originatorTypeCategory = OriginatorTypeCategory.OMS)
        {
            try
            {
                string cpID = "-1";
                if (_dictCounterPartyRules.ContainsKey(originatorTypeCategory) && _dictCounterPartyRules[originatorTypeCategory].ContainsKey(cpID))
                {
                    List<Rule> cpRules = _dictCounterPartyRules[originatorTypeCategory][cpID];
                    foreach (Rule rule in cpRules)
                    {
                        rule.ApplyRule(pranaMessage, direction);
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
    }
}