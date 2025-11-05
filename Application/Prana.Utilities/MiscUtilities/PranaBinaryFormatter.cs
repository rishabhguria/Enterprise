using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Prana.Utilities.MiscUtilities
{
    public class PranaBinaryFormatter
    {
        public string Serialize(object targetObject)
        {
            try
            {
                if (targetObject == null)
                {
                    return null;
                }
                MemoryStream memoryStream = new MemoryStream();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, targetObject);
                string str = System.Convert.ToBase64String(memoryStream.ToArray());
                return str;
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

        public object DeSerialize(string data)
        {

            
            try
            {
                if (string.IsNullOrEmpty(data))
                {
                    return null;
                }

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                byte[] bytesData = System.Convert.FromBase64String(data);
                using (MemoryStream memoryStream = new MemoryStream(bytesData))
                {
                    object targetObject = binaryFormatter.Deserialize(memoryStream);
                    return targetObject;
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

                return null;
            }
        }

        /// <summary>
        /// Can be used to deserialize the string serialized from multiple objects
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<object> DeSerializeParams(string s)
        {
            bool escaped = false;
            List<object> listOfObj = new List<object>();
            StringBuilder sb = new StringBuilder();
            try
            {
                if (string.IsNullOrEmpty(s))
                {
                    return null;
                }
                foreach (char c in s)
                {
                    if ((c == '\\') && !escaped)
                    {
                        escaped = true;
                    }
                    else if ((c == ',') && !escaped)
                    {
                        listOfObj.Add(DeSerialize(sb.ToString()));
                        sb.Length = 0;
                    }
                    else
                    {
                        sb.Append(c);
                        escaped = false;
                    }
                }
                listOfObj.Add(DeSerialize(sb.ToString()));
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
            return listOfObj;
        }

        /// <summary>
        /// Can be used to serialize multiple objects passed as params. Delimiter used is a comma. Existing commas are escaped by \,
        /// Use DeSerializeParams function to deserialize objects serialized using this function.
        /// </summary>
        /// <param name="listOfObjectToSerialize"></param>
        /// <returns></returns>
        public string Serialize(params object[] listOfObjectToSerialize)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (listOfObjectToSerialize == null)
                {
                    return null;
                }
                foreach (object item in listOfObjectToSerialize)
                {
                    sb.Append(Serialize(item).Replace("\\", "\\\\").Replace(",", "\\,"));
                    sb.Append(",");
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
            return (sb.Length > 0) ? sb.ToString(0, sb.Length - 1) : string.Empty;
        }
    }
}
