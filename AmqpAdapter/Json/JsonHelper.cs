using Newtonsoft.Json;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace Prana.AmqpAdapter.Json
{
    public class JsonHelper
    {
        static JsonSerializerSettings settings;

        static JsonHelper()
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter conv = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            conv.DateTimeFormat = "MM/dd/yyyy HH:mm:ss zzz";

            settings = new JsonSerializerSettings
            {
                ContractResolver = new AllDataContractResolver(),
                Converters = { conv },
            };
        }

        /// <summary>
        /// Serializing object to JsonOutput
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Json Output</returns>
        public static byte[] Serialize(Object obj)
        {
            try
            {
                String jsonOutput = JsonConvert.SerializeObject(obj, settings);
                return System.Text.Encoding.UTF8.GetBytes(jsonOutput);
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
        /// Serializing DataSet Object to JsonOutput
        /// </summary>
        /// <param name="obj"> Dataset Object</param>
        /// <returns>Json Output</returns>
        public static byte[] SerializeDataSet(Object obj)
        {
            try
            {
                String jsonOutput = JsonConvert.SerializeObject(obj, settings);
                return System.Text.Encoding.UTF8.GetBytes(jsonOutput.Replace('[', ' ').Replace(']', ' '));
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
        /// Serializing Data Set as Row to Json Output
        /// </summary>
        /// <param name="obj">DataSet Object</param>
        /// <returns> Json Output</returns>
        public static List<byte[]> SerializeDataSetAsRow(DataSet obj)
        {
            List<byte[]> tempList = new List<byte[]>();

            try
            {
                DataTable dtTemp = obj.Tables[0].Clone();
                DataSet dsTemp = new DataSet();
                dsTemp.Tables.Add(dtTemp);

                foreach (DataRow dr in obj.Tables[0].Rows)
                {
                    dtTemp.Clear();
                    //DataTable dtTemp = new DataTable(obj.Tables[0].TableName);
                    dtTemp.Rows.Add(dr.ItemArray);
                    String jsonOutput = JsonConvert.SerializeObject(dsTemp, settings);
                    tempList.Add(System.Text.Encoding.UTF8.GetBytes(jsonOutput.Replace('[', ' ').Replace(']', ' ')));
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
            return tempList;
        }

        /// <summary>
        /// Deserialization
        /// </summary>
        /// <param name="jsonToConvert"> json To Convert as a String</param>
        /// <returns>Static Dataset</returns>
        public static DataSet Deserialize(String jsonToConvert)
        {
            DataSet ds = null;
            try
            {
                //Newtonsoft.Json.Converters.DataSetConverter dsConverter = new Newtonsoft.Json.Converters.DataSetConverter();
                XmlDocument xd = new XmlDocument();
                xd = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonToConvert);
                ds = new DataSet();
                ds.ReadXml(new XmlNodeReader(xd));
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
            return ds;
        }
    }
}
