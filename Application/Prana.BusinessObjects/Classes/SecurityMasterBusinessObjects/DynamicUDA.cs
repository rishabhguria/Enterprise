using Prana.LogManager;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects
{
    /// <summary>
    /// The DynamicUDA class
    /// </summary>
    [Serializable]
    public class DynamicUDA
    {
        /// <summary>
        /// Tag of Dynamic UDA
        /// </summary>
        private string _tag = String.Empty;

        /// <summary>
        /// Dynamic UDA Tag
        /// </summary>
        public string Tag
        {
            get { return _tag; }
            //TODO: Remove this setter property
            set { _tag = value; }
        }

        /// <summary>
        /// Header Caption of Dynamic UDA
        /// </summary>
        private string _headerCaption;

        /// <summary>
        /// Dynamic UDA Header Caption
        /// </summary>
        public string HeaderCaption
        {
            get { return _headerCaption; }
            set { _headerCaption = value; }
        }

        /// <summary>
        /// Default Value of Dynamic UDA
        /// </summary>
        private string _defaultValue;

        /// <summary>
        /// Dynamic UDA Default Value
        /// </summary>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        /// <summary>
        /// Master Values of Dynamic UDA
        /// </summary>
       // private List<string> _masterValues = new List<string>();
        private SerializableDictionary<string, string> _masterValues = new SerializableDictionary<string, string>();

        /// <summary>
        /// Master Values of Dynamic UDA
        /// </summary>
        public SerializableDictionary<string, string> MasterValues
        {
            get { return _masterValues; }
            //TODO: Remove this setter property
            set { _masterValues = value; }
        }

        /// <summary>
        /// Parameterized Constructor that is assigning Dynamic UDA Tag Value
        /// </summary>
        /// <param name="tag">Tag</param>
        public DynamicUDA(string tag)
        {
            this._tag = tag;
            this._masterValues = new SerializableDictionary<string, string>();
        }

        /// <summary>
        ///  Parameterized Constructor that is assigning Dynamic UDA Tag Value, Header Caption, Default Value and Master Values(in form of XML)
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <param name="caption">Header Caption</param>
        /// <param name="defaultValue">Default Value</param>
        /// <param name="masterValue">Master Values</param>
        public DynamicUDA(string tag, string caption, string defaultValue, string masterValues)
        {
            try
            {
                this._tag = tag;
                this._headerCaption = caption;
                this._defaultValue = defaultValue;
                this._masterValues = DeserializeXML(masterValues);
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
        ///  Parameterized Constructor that is assigning Dynamic UDA Tag Value, Header Caption, Default Value and Master Values (in form of List)
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <param name="caption">Header Caption</param>
        /// <param name="defaultValue">Default Value</param>
        /// <param name="masterValue">Master Values</param>
        public DynamicUDA(string tag, string caption, string defaultValue, SerializableDictionary<string, string> masterValues)
        {
            try
            {
                this._tag = tag;
                this._headerCaption = caption;
                this._defaultValue = defaultValue;
                foreach (string key in masterValues.Keys)
                {
                    if (!_masterValues.ContainsKey(key))
                        _masterValues.Add(key, masterValues[key]);
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
        /// Default Constructor
        /// </summary>
        public DynamicUDA()
        {
            // TODO: Complete member initialization
        }

        /// <summary>
        /// Deserialize XML to List
        /// </summary>
        /// <param name="masterValues"></param>
        /// <returns></returns>
        private SerializableDictionary<string, string> DeserializeXML(string masterValues)
        {
            try
            {
                SerializableDictionary<string, string> dic = new SerializableDictionary<string, string>();
                if (!String.IsNullOrWhiteSpace(masterValues))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(masterValues);
                    XmlNodeList resources = doc.SelectNodes("MasterUDAValue/Value");

                    foreach (XmlNode node in resources)
                    {
                        dic.Add(node.Attributes["key"].Value, node.InnerText);
                    }
                }
                return dic;
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
                return null;
            }
        }

        /// <summary>
        /// Serialize XML to String
        /// </summary>
        /// <param name="masterValues"></param>
        /// <returns></returns>
        public string SerializeToXML(SerializableDictionary<string, string> masterValues)
        {
            try
            {
                if (masterValues != null && masterValues.Count() > 0)
                {
                    XElement masterValue = new XElement("MasterUDAValue");

                    foreach (string key in masterValues.Keys)
                    {
                        XElement value = new XElement("Value", new XAttribute("key", key));
                        value.Value = masterValues[key];
                        masterValue.Add(value);
                    }
                    return masterValue.ToString();
                }
                else
                    return null;
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
                return null;
            }
        }
    }
}
