using Bloomberglp.Blpapi;
using System;
using System.Collections.Generic;

namespace Bloomberg.Library
{
    /// <summary>
    /// Security Data
    /// </summary>
    /// <remarks></remarks> 
    [Serializable]
    public class SecurityData
    {
        /// <summary>
        /// Gets or sets the security.
        /// </summary>
        /// <value>The security.</value>
        /// <remarks></remarks>
        public string Security;
        /// <summary>
        /// Gets or sets the eid data.
        /// </summary>
        /// <value>The eid data.</value>
        /// <remarks></remarks>
        public List<Entitlement> Entitlements;
        /// <summary>
        /// Gets or sets the security error.
        /// </summary>
        /// <value>The security error.</value>
        /// <remarks></remarks>
        public SecurityError SecurityError;
        /// <summary>
        /// Gets or sets the field exceptions.
        /// </summary>
        /// <value>The field exceptions.</value>
        /// <remarks></remarks>
        public List<FieldException> FieldExceptions;
        /// <summary>
        /// Gets or sets the field data.
        /// </summary>
        /// <value>The field data.</value>
        /// <remarks></remarks>
        public List<Fields> Fields;
        /// <summary>
        /// Gets or sets the sequence number.
        /// </summary>
        /// <value>The sequence number.</value>
        /// <remarks></remarks>
        public int SequenceNumber;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public SecurityData()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityData"/> class.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <remarks></remarks>
        public SecurityData(Element security)
        {
            ProcessEntitlements(security);
            ProcessFieldExceptions(security);
            ProcessFieldData(security);

            if (security.HasElement(new Name("sequenceNumber")))
            {
                SequenceNumber = security.GetElementAsInt32(new Name("sequenceNumber"));
            }

            if (security.HasElement(new Name("security")))
            {
                Security = security.GetElementAsString(new Name("security"));
            }

            if (security.HasElement(new Name("securityError")))
            {
                SecurityError = new SecurityError(security.GetElement(new Name("securityError")));
            }
        }

        /// <summary>
        /// Get and Convert Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topic"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public T GetValue<T>(string key, T def)
        {
            if (Fields[0].ContainsKey(key) == false) return def;

            object value = Fields[0][key].Value;
            object tvalue = Convert.ChangeType(value == null ? def : value, typeof(T));
            return (value == null) ? def : (T)tvalue;
        }
        /// <summary>
        /// Get and Convert Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topic"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public T GetValue<T>(int index, string key, T def)
        {
            if (Fields[index].ContainsKey(key) == false) return def;

            object value = Fields[index][key].Value;
            object tvalue = Convert.ChangeType(value == null ? def : value, typeof(T));
            return (value == null) ? def : (T)tvalue;
        }

        public bool HasField(string field)
        {
            return Fields[0].ContainsKey(field);
        }

        /// <summary>
        /// Processes the field data.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <remarks></remarks>
        private void ProcessFieldData(Element security)
        {
            Fields = new List<Fields>();
            if (security.HasElement(new Name("fieldData")))
            {
                Element fieldData = security.GetElement(new Name("fieldData"));
                Element fields = fieldData;

                if (fieldData.IsArray)
                {
                    int size = fieldData.NumValues;
                    for (int index = 0; index < size; index++)
                    {
                        fields = fieldData.GetValueAsElement(index);
                        Fields Data = new Fields(fields);

                        Fields.Add(Data);
                    }
                }
                else
                {
                    Fields Data = new Fields(fields);
                    Fields.Add(Data);
                }
            }
        }
        /// <summary>
        /// Processes the eie data.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <remarks></remarks>
        private void ProcessEntitlements(Element security)
        {
            Entitlements = new List<Entitlement>();
            if (security.HasElement(new Name("eidData")))
            {
                Entitlements.Add(new Entitlement(security));
            }
        }
        /// <summary>
        /// Processes the field exceptions.
        /// </summary>
        /// <param name="security">The security.</param>
        /// <remarks></remarks>
        private void ProcessFieldExceptions(Element security)
        {
            FieldExceptions = new List<FieldException>();
            if (security.HasElement(new Name("fieldExceptions")))
            {
                Element exceptions = security.GetElement(new Name("fieldExceptions"));
                int size = exceptions.NumValues;

                for (int i = 0; i < size; i++)
                {
                    FieldExceptions.Add(new FieldException(exceptions.GetValueAsElement(i)));
                }
            }
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Security;
        }
    }
}
