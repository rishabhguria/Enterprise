using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities;
using System;
using System.IO;
using System.Xml;

namespace Prana.SecurityMasterNew
{
    public static class SymbolTransformer
    {
        public static void TransformSymbol(SecMasterRequestObj secMasterRequestObj)
        {
            try
            {

                string transformedXml = XslTransformer.GetInstance(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\xslts\SymbolTransformer.xslt").TransformXmlInMemory(secMasterRequestObj.CreateXml());

                XmlDocument xd = new XmlDocument();
                xd.LoadXml(transformedXml);

                XmlNodeList xmlNodesDetails = xd.SelectNodes("DocumentElement/SecMasterRequest");
                string convertedSymbol = string.Empty;
                int convertedSymbology = int.MinValue;
                DateTime expirationDate = DateTime.MinValue;
                string requestedSymbol = string.Empty;
                string underlyingSymbol = string.Empty;
                // string bbSymbol = string.Empty;
                int i = 0;
                foreach (SymbolDataRow row in secMasterRequestObj.SymbolDataRowCollection)
                {
                    convertedSymbol = string.Empty;
                    convertedSymbology = int.MinValue;
                    requestedSymbol = string.Empty;
                    expirationDate = DateTime.MinValue;
                    underlyingSymbol = string.Empty;
                    //   bbSymbol = string.Empty;
                    if (xmlNodesDetails.Count > 0)
                    {
                        foreach (XmlNode node in xmlNodesDetails[i].ChildNodes)
                        {
                            if (node.Name.Equals("RequestedSymbol"))
                            {
                                requestedSymbol = node.InnerText;
                            }
                            if (node.Name.Equals("ConvertedSymbol"))
                            {
                                convertedSymbol = node.InnerText;
                            }
                            if (node.Name.Equals("ConvertedSymbology"))
                            {
                                convertedSymbology = int.Parse(node.InnerText.ToString());
                            }
                            if (node.Name.Equals("ExpirationDate"))
                            {
                                expirationDate = DateTime.Parse(node.InnerText);
                            }
                            if (node.Name.Equals("UnderlyingSymbol"))
                            {
                                underlyingSymbol = node.InnerText;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(requestedSymbol) && !requestedSymbol.Equals(row.PrimarySymbol))
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(convertedSymbol))
                    {
                        if (row.SymbolData.Count > 0)
                        {
                            row.SymbolData[convertedSymbology] = convertedSymbol;
                            row.ExpirationDate = expirationDate;
                            row.UnderlyingSymbol = underlyingSymbol;

                        }
                    }
                    if (convertedSymbology != int.MinValue)
                    {
                        row.PrimarySymbology = (ApplicationConstants.SymbologyCodes)convertedSymbology;
                    }
                    i++;
                    if (i > xmlNodesDetails.Count - 1)
                    {
                        break;
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
