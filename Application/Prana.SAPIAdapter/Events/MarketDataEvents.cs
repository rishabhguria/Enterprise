// ***********************************************************************
// Assembly         : Bloomberg.Library
// Author           : Administrator
// Created          : 05-21-2013
//
// Last Modified By : Administrator
// Last Modified On : 05-23-2013
// ***********************************************************************
// <copyright file="MarketDataEvents.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Bloomberg.Library
{
    /// <summary>
    /// Class MarketDataEvents
    /// </summary>
    [Serializable]
    public class MarketDataEvents
    {
        public string TopicName;
        public string Type;
        public string SubType;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketDataEvents" /> class.
        /// </summary>
        public MarketDataEvents() { }

        /// <summary>
        /// The fields
        /// </summary>
        public Dictionary<string, Fields> fields = new Dictionary<string, Fields>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MarketDataEvents" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MarketDataEvents(Bloomberglp.Blpapi.Message message)
        {
            try
            {
                TopicName = message.TopicName.ToString();
                Fields row;
                if (fields.TryGetValue(message.TopicName, out row) == false)
                {
                    row = new Fields(message.AsElement);
                    fields.Add(message.TopicName, row);
                }
                else
                {
                    fields[message.TopicName].Update(message.AsElement);
                }
                Type = (string)fields[TopicName]["MKTDATA_EVENT_TYPE"].Value.ToString();
                SubType = (string)fields[TopicName]["MKTDATA_EVENT_SUBTYPE"].Value.ToString();

                row.Print();

            }
            catch (Exception ex)
            {
                Logger.LoggerWrite(ex.Message);
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
        public T GetValue<T>(string topic, string key, T def)
        {

            if (fields.ContainsKey(topic) == false) return def;
            if (fields[topic].ContainsKey(key) == false) return def;
            object value = fields[topic][key].Value;
            object tvalue = Convert.ChangeType(value == null ? def : value, typeof(T));
            return (value == null) ? def : (T)tvalue;
        }

        public DateTime GetValue(string topic, string key, DateTime def)
        {

            if (fields.ContainsKey(topic) == false) return def;
            if (fields[topic].ContainsKey(key) == false) return def;
            DateTime value = DateTimeOffset.Parse(fields[topic][key].Value == null ? def.ToString() : fields[topic][key].Value.ToString()).UtcDateTime;
            return value;
        }

    }
}
