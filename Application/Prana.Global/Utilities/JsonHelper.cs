using Newtonsoft.Json;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Serialization;

namespace Prana.Global.Utilities
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
        /// Deserialize To List of object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static List<T> DeserializeToList<T>(String jsonString)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<List<T>>(jsonString);
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Deserialize To Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T DeserializeToObject<T>(String jsonString)
        {
            try
            {
                var model = JsonConvert.DeserializeObject<T>(jsonString);
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aList"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(List<T> aList)
        {
            try
            {
                var json = JsonConvert.SerializeObject(aList);
                return json;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Serializing object to JsonOutput
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Json Output</returns>
        public static string SerializeObject(Object obj)
        {
            try
            {
                String jsonOutput = JsonConvert.SerializeObject(obj, settings);
                return jsonOutput;
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
        /// This will deserializes XML data from a byte array into an object type T and then serializes that object to a JSON string.
        /// </summary>
        /// <typeparam name="T">Object type to desialize and serialize </typeparam>
        /// <param name="data">The byte array having  XML data</param>
        /// <returns>A JSON string of the type object T.</returns>
        public static string DeserializeAndSerializeToJson<T>(byte[] data)
        {
            using (var memoryStream = new MemoryStream(data))
            {
                var serializer = new XmlSerializer(typeof(T));
                var deserializedObject = (T)serializer.Deserialize(memoryStream);
                return SerializeObject(deserializedObject);
            }
        }
        
    }
}
