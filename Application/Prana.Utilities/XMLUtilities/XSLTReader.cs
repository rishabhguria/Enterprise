using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Prana.Utilities
{
    public class XSLTReader
    {
        /// <summary>
        /// Returns the node list under a given node in an xslt file
        /// </summary>
        /// <param name="xsltDoc"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static List<string> readNodesUnderGivenNodeName(string xsltDoc, string nodeName)
        {
            List<string> ruleList = new List<string>();
            try
            {
                if (File.Exists(xsltDoc))
                {
                    XmlDocument xslDoc = new XmlDocument();
                    xslDoc.Load(xsltDoc);
                    XmlNodeList elemList = xslDoc.GetElementsByTagName(nodeName);
                    if (elemList.Count > 0)
                    {
                        XmlNode xmlPositonMasterNode = elemList[0];
                        if (xmlPositonMasterNode.HasChildNodes)
                        {
                            XmlNode rule = xmlPositonMasterNode.FirstChild;
                            addAllSubNodeUnderNode(ref ruleList, rule);
                        }
                    }
                    return ruleList;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ruleList;
        }
        /// <summary>
        /// Recursive method to get all nodes and sub nodes of its
        /// </summary>
        /// <param name="ruleList"></param>
        /// <param name="rule"></param>
        private static void addAllSubNodeUnderNode(ref List<string> ruleList, XmlNode rule)
        {
            try
            {
                if (!ruleList.Contains(rule.Name))
                {
                    ruleList.Add(rule.Name);
                }
                if (rule.HasChildNodes)
                {
                    addAllSubNodeUnderNode(ref ruleList, rule.FirstChild);
                }
                while (rule.NextSibling != null)
                {
                    rule = rule.NextSibling;
                    if (!ruleList.Contains(rule.Name))
                    {
                        ruleList.Add(rule.Name);
                    }
                    if (rule.HasChildNodes)
                    {
                        addAllSubNodeUnderNode(ref ruleList, rule.FirstChild);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
