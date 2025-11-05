#region Using
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

#endregion Using

namespace Prana.ThirdPartyReport
{
    public class XMLHelper
    {
        public XMLHelper()
        {

        }

        /// <summary>
        /// The method is used to serialize the thirdparty details collection into an xml format.
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        private static string getxml(string filePath, ThirdPartyFlatFileDetailCollection coll)
        {
            string str = filePath;
            StreamWriter newXMLDoc = new StreamWriter(str);
            XmlSerializer isinXML = new XmlSerializer(typeof(ThirdPartyFlatFileDetailCollection));
            try
            {
                isinXML.Serialize(newXMLDoc, coll);
            }
            catch (XmlException ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
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
            finally
            {
                if (newXMLDoc != null)
                {
                    newXMLDoc.Close();
                }
                else
                {
                    str = string.Empty;
                }
            }
            return str;
        }

        /// <summary>
        /// The method is used to perform the xml to xml mapping using xslt tranformation .
        /// This maps the PranaThirdPartyDetails to ClientThirdPartyFormat.
        /// </summary>
        /// <param name="strXsltFilePath"></param>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static string GetTransformed(string serializeFilepath, string mappedFilePath, string strXsltFilePath, ThirdPartyFlatFileDetailCollection coll)
        {

            string strFilePathNew = mappedFilePath;
            string str = getxml(serializeFilepath, coll);
            if (str.Equals(""))
            {
                strFilePathNew = string.Empty;
                return strFilePathNew;
            }

            XmlWriter xw = new XmlTextWriter(strFilePathNew, Encoding.UTF8);
            XmlWriterSettings settings = new XmlWriterSettings();
            //settings.CheckCharacters = true;
            settings.CloseOutput = false;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.IndentChars = "";
            settings.NewLineOnAttributes = true;
            settings.OmitXmlDeclaration = false;
            settings.ConformanceLevel = ConformanceLevel.Auto;

            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;

            XsltSettings xsltsetting = new XsltSettings(true, true);
            // Set doDebug = true for debugging and false for performance
            // By disabling debug mode transformation can be faster. To make it more faster XPathDocument is used.
            // XPathDocument does not have line info so not suitable for debugging
            // https://msdn.microsoft.com/en-us/library/ms163418(v=vs.110).aspx
            bool doDebug = true;
            doDebug = Boolean.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("DebugXSLT"));
            XslCompiledTransform xslt = new XslCompiledTransform(doDebug);
            xslt.Load(strXsltFilePath, xsltsetting, null);
            try
            {
                if (doDebug)
                {
                    xslt.Transform(str, xw);
                }
                else
                {
                    XPathDocument xPathDoc = new XPathDocument(str);
                    xslt.Transform(xPathDoc, xw);
                }
            }
            catch (XmlException ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
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
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }
                else
                {
                    strFilePathNew = string.Empty;
                }
            }
            //Bool is returned to ensure that the xsl transformation has been executed successfully.
            //return new XmlTextReader(reader);
            return strFilePathNew;
        }

        /// <summary>
        /// The method is used to perform the xml to xml mapping using xslt tranformation .
        /// This maps the ClientThirdPartyFormat to PranaThirdPartyDetails .
        /// </summary>
        /// <param name="str"></param>
        /// <param name="mappedFilePath"></param>
        /// <param name="xsltPath"></param>
        /// <returns></returns>
        public static string GetTransformedtoPrana(string str, string mappedFilePath, string xsltPath)
        {
            string strFilePathNew = mappedFilePath;

            XmlWriter xw = new XmlTextWriter(strFilePathNew, Encoding.UTF8);
            XmlWriterSettings settings = new XmlWriterSettings();
            //settings.CheckCharacters = true;
            settings.CloseOutput = false;
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;
            settings.IndentChars = "";
            settings.NewLineOnAttributes = true;
            settings.OmitXmlDeclaration = false;
            settings.ConformanceLevel = ConformanceLevel.Auto;

            settings.NewLineChars = "\r\n";
            settings.NewLineHandling = NewLineHandling.Replace;
            XsltSettings xsltsetting = new XsltSettings(true, true);

            XslCompiledTransform xslt = new XslCompiledTransform(true);

            xslt.Load(xsltPath, xsltsetting, null);
            try
            {
                xslt.Transform(str, xw);
            }
            catch (XmlException ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
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
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }
                else
                {
                    strFilePathNew = string.Empty;
                }
            }
            //Bool is returned to ensure that the xsl transformation has been executed successfully.
            //return new XmlTextReader(reader);
            return strFilePathNew;
        }
    }

}
