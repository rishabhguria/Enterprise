using Prana.LogManager;
using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Prana.Utilities
{
    public class XslTransformer
    {
        XslCompiledTransform _xslt = null;

        private static XslTransformer _singleton = null;
        private static readonly object _locker = new object();

        public static XslTransformer GetInstance(string xsltPath)
        {
            if (_singleton == null)
            {
                lock (_locker)
                {
                    if (_singleton == null)
                    {
                        _singleton = new XslTransformer(xsltPath);
                    }
                }
            }
            return _singleton;
        }



        public XslTransformer(string xsltPath)
        {
            LoadXslt(xsltPath);
        }

        public void LoadXslt(string xsltPath)
        {
            lock (_locker)
            {
                _xslt = new XslCompiledTransform(true);
                XsltSettings xsltsettings = new XsltSettings(true, true);
                _xslt.Load(xsltPath, xsltsettings, null);
            }
        }

        public XslTransformer()
        {

        }

        public string TransformXmlInMemory(string xmlString)
        {
            string outputxml = string.Empty;
            StreamReader sr = null;
            try
            {
                lock (_locker)
                {
                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(xmlString);

                    //Mukul: Write the output data to memoryStream...
                    MemoryStream stm = new MemoryStream();
                    _xslt.Transform(xd, null, stm);
                    stm.Position = 0;
                    sr = new StreamReader(stm);
                    outputxml = sr.ReadToEnd();
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

            finally
            {
                sr.Close();
            }
            return outputxml;
        }
    }
}
