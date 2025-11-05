using Prana.LogManager;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Prana.Utilities.XMLUtilities
{
    public class XMLUtilities
    {

        private static object lockObject = new object();
        /// <summary>
        /// Takes a serializable object and serializes it to its XML string representation.
        /// </summary>
        /// <param name="objToSerialize">The object to be serialized.  This object must be serializable or the method will fail.</param>
        /// <returns>A string of XML representing the serialized version of the object.</returns>
        public static string SerializeToXML(Object objToSerialize)
        {
            if (objToSerialize == null)
            {
                objToSerialize = new object();
            }
            System.Xml.Serialization.XmlSerializer mySerializer = null;
            MemoryStream msSerializedXML = null;
            try
            {
                // Instantiate the Serializer with the type of object that is being deserialized. 
                mySerializer = new System.Xml.Serialization.XmlSerializer(objToSerialize.GetType());

                // Serialize the object to xml 
                msSerializedXML = new MemoryStream();
                mySerializer.Serialize(msSerializedXML, objToSerialize);

                // Get XML as string 
                string xmlStr = System.Text.ASCIIEncoding.ASCII.GetString(msSerializedXML.ToArray());
                //msSerializedXML.Close();

                if (xmlStr == null || xmlStr.Length == 0)
                {
                    return String.Empty;
                }

                return xmlStr;
            }
            catch //(Exception ex)
            {
                throw;
            }
            finally
            {
                if (msSerializedXML != null) { msSerializedXML.Close(); }
            }
        }
        /// <summary>
        /// The method is used to perform the xml to xml mapping using XslCompiledTransform .
        /// This maps the ClientThirdPartyFormat to PranaThirdPartyDetails .
        /// </summary>
        /// <param name="str"></param>
        /// <param name="mappedFilePath"></param>
        /// <param name="xsltPath"></param>
        /// <returns></returns>
        public static string GetTransformed(string str, string mappedFilePath, string xsltPath)
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

            XslCompiledTransform xslt = new XslCompiledTransform(true);
            XsltSettings xsltsettings = new XsltSettings(true, true);
            xslt.Load(xsltPath, xsltsettings, null);
            try
            {
                xslt.Transform(str, xw);
            }
            //catch (XmlException e)
            //{
            //    throw e;
            //}
            catch (Exception ex)
            {
                if (ex is XmlException)
                {
                    throw;
                }
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
        /// The method is used to perform the xml to xml mapping using xslt transformation .
        /// </summary>
        /// <param name="str"></param>
        /// <param name="mappedFilePath"></param>
        /// <param name="xsltPath"></param>
        /// <returns></returns>
        public static void GetTransformedXML(string inputXML, string outputXML, string xsltPath)
        {
            System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
            // System.Xml.Xsl.XslTransform xslt = new System.Xml.Xsl.XslTransform();
            XsltSettings set = new XsltSettings(true, true);
            xslt.Load(xsltPath, set, null);
            try
            {

                //xslt.OutputSettings = set;
                xslt.Transform(inputXML, outputXML);
            }
            //catch (XmlException e)
            //{
            //    throw e;
            //}
            catch (Exception ex)
            {
                if (ex is XmlException)
                {
                    throw;
                }
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        public static void Transform(string sXmlPath, string sXslPath, string outputXmlPath)
        {
            try
            {
                //load the Xml doc
                XPathDocument myXPathDoc = new XPathDocument(sXmlPath);

                XslCompiledTransform myXslTrans = new XslCompiledTransform();
                //XslTransform myXslTrans = new XslTransform();

                //load the Xsl 
                myXslTrans.Load(sXslPath);

                //create the output stream
                XmlTextWriter myWriter = new XmlTextWriter
                    (outputXmlPath, null);

                //do the actual transform of Xml
                myXslTrans.Transform(myXPathDoc, null, myWriter);

                myWriter.Close();
            }
            catch (Exception e)
            {
                Logger.LoggerWrite("Exception: {0}", e.ToString());
            }

        }
        public static string GetTransformed(XmlReader xmlreader, string mappedFilePath, string xsltPath)
        {
            string strFilePathNew = mappedFilePath;
            XmlWriter xw = new XmlTextWriter(mappedFilePath, Encoding.UTF8);
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
                xslt.Transform(xmlreader, null, xw);
                //xslt.Transform(
            }
            //catch (XmlException e)
            //{
            //    throw e;
            //}
            catch (Exception ex)
            {
                if (ex is XmlException)
                {
                    throw;
                }
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
        /// Takes a serializable object and serializes it to its XML string representation.
        /// </summary>
        /// <param name="objToSerialize">The object to be serialized.  This object must be serializable or the method will fail.</param>
        /// <returns>A string of XML representing the serialized version of the object.</returns>
        public static string SerializeUsingBinaryFormatter(Object objToSerialize)
        {
            //System.Xml.Serialization.XmlSerializer mySerializer = null;
            MemoryStream msSerializedXML = null;
            try
            {
                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();
                msSerializedXML = new MemoryStream();
                try
                {
                    formatter.Serialize(msSerializedXML, objToSerialize);
                }
                catch (SerializationException e)
                {
                    Logger.LoggerWrite("Failed to serialize. Reason: " + e.Message);
                    throw;
                }

                // Instantiate the Serializer with the type of object that is being deserialized. 
                //mySerializer = new System.Xml.Serialization.XmlSerializer(objToSerialize.GetType());

                // Serialize the object to xml 
                //msSerializedXML = new MemoryStream();
                //mySerializer.Serialize(msSerializedXML, objToSerialize);

                // Get XML as string 
                string xmlStr = System.Text.ASCIIEncoding.ASCII.GetString(msSerializedXML.ToArray());
                //CA2202	Do not dispose objects multiple times	Object 'msSerializedXML' can be disposed more than once in method 'XMLUtilities.SerializeUsingBinaryFormatter(object)'. To avoid generating a System.ObjectDisposedException you should not call Dispose more than one time on an object.: Lines: 351	Prana.Utilities	XMLUtilities.cs	351
                //msSerializedXML.Close();

                if (xmlStr == null || xmlStr.Length == 0)
                {
                    return String.Empty;
                }

                return xmlStr;
            }
            catch //(Exception ex)
            {
                throw;
            }
            finally
            {
                if (msSerializedXML != null) { msSerializedXML.Close(); }
            }
        }

        public static string SerializeWithXmlWriter(Object objToSerialize)
        {
            MemoryStream msSerializedXML = null;
            try
            {
                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();
                msSerializedXML = new MemoryStream();
                try
                {
                    formatter.Serialize(msSerializedXML, objToSerialize);
                }
                catch (SerializationException e)
                {
                    Logger.LoggerWrite("Failed to serialize. Reason: " + e.Message);
                    throw;
                }

                // Instantiate the Serializer with the type of object that is being deserialized. 
                //mySerializer = new System.Xml.Serialization.XmlSerializer(objToSerialize.GetType());

                // Serialize the object to xml 
                //msSerializedXML = new MemoryStream();
                //mySerializer.Serialize(msSerializedXML, objToSerialize);

                // Get XML as string 
                string xmlStr = System.Text.ASCIIEncoding.ASCII.GetString(msSerializedXML.ToArray());
                //msSerializedXML.Close();

                if (xmlStr == null || xmlStr.Length == 0)
                {
                    return String.Empty;
                }

                return xmlStr;
            }
            catch //(Exception ex)
            {
                throw;
            }
            finally
            {
                if (msSerializedXML != null) { msSerializedXML.Close(); }
            }
        }

        public static void WriteXml(string xml, string filePath)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + filePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + filePath));
                }
                using (FileStream fileStream = File.Open(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    lock (lockObject)
                    {
                        StreamWriter writer = new StreamWriter(fileStream, Encoding.GetEncoding("ISO-8859-1"));
                        fileStream.Position = 0;
                        writer.Write(xml);
                        writer.Flush();
                        fileStream.Flush();
                        //fileStream.Close();
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

        /// <summary>
        /// Reads Xml Using Buffered Stream
        /// Prevent from out of memory exception
        /// </summary>
        /// <param name="pathOfXMLFile"></param>
        /// <returns></returns>
        public static DataSet ReadXmlUsingBufferedStream(string pathOfXMLFile)
        {
            DataSet ds = new DataSet();
            ds.EnforceConstraints = false;
            try
            {
                if (File.Exists(pathOfXMLFile))
                {
                    //Modified by Pranay Deep.
                    //using BufferedStream in the place of ReadXml
                    using (FileStream filestream = File.OpenRead(pathOfXMLFile))
                    {
                        BufferedStream buffered = new BufferedStream(filestream);
                        ds.ReadXml(buffered);
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
            ds.EnforceConstraints = true;
            return ds;
        }

        /// <summary>
        /// DataSet Write XML using XmlTextWriter
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="filePath"></param>
        public static void WriteXML(DataSet ds, string filePath)
        {
            try
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(filePath, Encoding.UTF8))
                {
                    ds.WriteXml(xmlWriter);
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

        /// <summary>
        /// DataTable Write XML using XmlTextWriter
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public static void WriteXML(DataTable dt, string filePath)
        {
            try
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(filePath, Encoding.UTF8))
                {
                    //modified by amit 02.04.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3249                    
                    //dt.WriteXml(xmlWriter, XmlWriteMode.WriteSchema);
                    //Created different method for reading and writing schema
                    dt.WriteXml(xmlWriter);
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

        /// <summary>
        /// DataTable Write XML using XmlTextWriter With Schema
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        public static void WriteXMLWithSchema(DataTable dt, string filePath)
        {
            try
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(filePath, Encoding.UTF8))
                {
                    //modified by amit 02.04.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3249                    
                    dt.WriteXml(xmlWriter, XmlWriteMode.WriteSchema);
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
        /// <summary>
        /// DataSet Write XML using XmlTextWriter With Schema
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="filePath"></param>
        public static void WriteXMLWithSchema(DataSet ds, string filePath)
        {
            try
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(filePath, Encoding.UTF8))
                {
                    //modified by amit 02.04.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3249                    
                    ds.WriteXml(xmlWriter, XmlWriteMode.WriteSchema);
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

        #region column Serialization And Deserialization to xmlFile

        public static void SerializeToXMLFile<T>(T serializableObject, string pathOfXMLFile)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                if (!Directory.Exists(Path.GetDirectoryName(pathOfXMLFile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(pathOfXMLFile));

                TextWriter textWriter = new StreamWriter(pathOfXMLFile);
                serializer.Serialize(textWriter, serializableObject);
                textWriter.Close();
            }
            catch //(Exception ex)
            {
                throw;
            }
        }

        public static T DeserializeFromXMLFile<T>(string pathOfXMLFile)
        {
            TextReader textReader = null;
            T serializableObject;
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                if (!File.Exists(pathOfXMLFile))
                    throw new Exception(pathOfXMLFile + " doesn't Exist");
                textReader = new StreamReader(pathOfXMLFile);
                serializableObject = (T)deserializer.Deserialize(textReader);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (textReader != null)
                {
                    textReader.Close();
                }
            }
            return serializableObject;
        }

        #endregion
    }
}
