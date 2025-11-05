using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

namespace Prana.Utilities.UI.XMLUtilities
{
    public class XMLUtilities
    {
        /// <summary>
        /// this method is used to validate the XML with the help of XSD
        /// Changes done as per link http://www.nullskull.com/q/10002158/compilation-errors.aspx to remove warnings 
        /// </summary>
        /// <param name="strXML"></param>
        /// <param name="strXSD"></param>
        /// <param name="strXSDNS"></param>
        /// <returns></returns>

        public static bool ValidateXML(string strXML, string strXSD, string strXSDNS, out string errorMsg, bool showError = true)
        {
            bool isValidated = false;
            errorMsg = string.Empty;
            // Create a XmlValidatingReader, XmlSchemaCollection and ValidationEventHandler objects to be used to
            //validate the XML against an XSD file
            //XmlValidatingReader reader = null;
            XmlTextReader tr = null;
            XmlReader reader = null;
            XmlReaderSettings xrs = null;
            XmlSchemaSet myschema = new XmlSchemaSet();
            //XmlSchemaCollection myschema = new XmlSchemaCollection();
            //ValidationEventHandler eventHandler = new ValidationEventHandler(XMLUtility.ShowCompileErrors);
            try
            {
                //Create the XML fragment to be parsed.
                XmlDocument doc = new XmlDocument();
                doc.Load(strXML);
                string xmlFrag = doc.InnerXml;
                //Create an XmlParserContext object for use with the XMLValidatingReader
                //Create the XmlParserContext
                XmlParserContext context = new XmlParserContext(null, null, "", XmlSpace.None);
                //Implement the reader
                tr = new XmlTextReader(xmlFrag, XmlNodeType.Element, context);
                //new XmlReader(xmlFrag, XmlNodeType.Element, context); //XmlValidatingReader

                //Add the relevant schema files (.XSD) to the name space we are checking against and repeat
                //until all the schema's have an associated XSD file with them
                //Add the schema.
                myschema.Add(strXSDNS, strXSD);
                //Set the schema type and add the schema to the reader.
                xrs = new XmlReaderSettings();
                xrs.ConformanceLevel = ConformanceLevel.Auto;
                xrs.Schemas.Add(myschema);
                // Add validation event handler
                xrs.ValidationType = ValidationType.Schema;
                //reader.ValidationType = ValidationType.Schema;
                reader = XmlReader.Create(tr, xrs);
                //reader.Schemas.Add(myschema);
                //Read in the XML Data: 

                while (reader.Read())
                {
                }
                isValidated = true;
            }
            catch (XmlException XMlExp)
            {
                if (showError)
                    MessageBox.Show(XMlExp.Message, "XMLValidation", MessageBoxButtons.OK);
                isValidated = false;
                errorMsg = XMlExp.Message;
            }
            catch (Exception GenExp)
            {
                if (showError)
                    MessageBox.Show(GenExp.Message, "XMLValidation", MessageBoxButtons.OK);
                isValidated = false;
                errorMsg = GenExp.Message;
            }
            return isValidated;
        }
    }
}
