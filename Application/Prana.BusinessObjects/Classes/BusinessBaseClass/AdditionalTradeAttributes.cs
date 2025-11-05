using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prana.BusinessObjects.Classes.BusinessBaseClass
{
    /// <summary>
    /// Represents a collection of additional trade attribute values
    /// with utility methods to get and set values dynamically.
    /// </summary>
    [Serializable]
    public class AdditionalTradeAttributes
    {
        #region Properties

        public virtual string TradeAttribute7 { get; set; }
        public virtual string TradeAttribute8 { get; set; }
        public virtual string TradeAttribute9 { get; set; }
        public virtual string TradeAttribute10 { get; set; }
        public virtual string TradeAttribute11 { get; set; }
        public virtual string TradeAttribute12 { get; set; }
        public virtual string TradeAttribute13 { get; set; }
        public virtual string TradeAttribute14 { get; set; }
        public virtual string TradeAttribute15 { get; set; }
        public virtual string TradeAttribute16 { get; set; }
        public virtual string TradeAttribute17 { get; set; }
        public virtual string TradeAttribute18 { get; set; }
        public virtual string TradeAttribute19 { get; set; }
        public virtual string TradeAttribute20 { get; set; }
        public virtual string TradeAttribute21 { get; set; }
        public virtual string TradeAttribute22 { get; set; }
        public virtual string TradeAttribute23 { get; set; }
        public virtual string TradeAttribute24 { get; set; }
        public virtual string TradeAttribute25 { get; set; }
        public virtual string TradeAttribute26 { get; set; }
        public virtual string TradeAttribute27 { get; set; }
        public virtual string TradeAttribute28 { get; set; }
        public virtual string TradeAttribute29 { get; set; }
        public virtual string TradeAttribute30 { get; set; }
        public virtual string TradeAttribute31 { get; set; }
        public virtual string TradeAttribute32 { get; set; }
        public virtual string TradeAttribute33 { get; set; }
        public virtual string TradeAttribute34 { get; set; }
        public virtual string TradeAttribute35 { get; set; }
        public virtual string TradeAttribute36 { get; set; }
        public virtual string TradeAttribute37 { get; set; }
        public virtual string TradeAttribute38 { get; set; }
        public virtual string TradeAttribute39 { get; set; }
        public virtual string TradeAttribute40 { get; set; }
        public virtual string TradeAttribute41 { get; set; }
        public virtual string TradeAttribute42 { get; set; }
        public virtual string TradeAttribute43 { get; set; }
        public virtual string TradeAttribute44 { get; set; }
        public virtual string TradeAttribute45 { get; set; }

        #endregion

        #region Private Fields

        private static readonly Dictionary<string, Action<AdditionalTradeAttributes, string>> _attributeSetters =
            new Dictionary<string, Action<AdditionalTradeAttributes, string>>
            {
                { "TradeAttribute7",  (obj, val) => obj.TradeAttribute7 = val },
                { "TradeAttribute8",  (obj, val) => obj.TradeAttribute8 = val },
                { "TradeAttribute9",  (obj, val) => obj.TradeAttribute9 = val },
                { "TradeAttribute10", (obj, val) => obj.TradeAttribute10 = val },
                { "TradeAttribute11", (obj, val) => obj.TradeAttribute11 = val },
                { "TradeAttribute12", (obj, val) => obj.TradeAttribute12 = val },
                { "TradeAttribute13", (obj, val) => obj.TradeAttribute13 = val },
                { "TradeAttribute14", (obj, val) => obj.TradeAttribute14 = val },
                { "TradeAttribute15", (obj, val) => obj.TradeAttribute15 = val },
                { "TradeAttribute16", (obj, val) => obj.TradeAttribute16 = val },
                { "TradeAttribute17", (obj, val) => obj.TradeAttribute17 = val },
                { "TradeAttribute18", (obj, val) => obj.TradeAttribute18 = val },
                { "TradeAttribute19", (obj, val) => obj.TradeAttribute19 = val },
                { "TradeAttribute20", (obj, val) => obj.TradeAttribute20 = val },
                { "TradeAttribute21", (obj, val) => obj.TradeAttribute21 = val },
                { "TradeAttribute22", (obj, val) => obj.TradeAttribute22 = val },
                { "TradeAttribute23", (obj, val) => obj.TradeAttribute23 = val },
                { "TradeAttribute24", (obj, val) => obj.TradeAttribute24 = val },
                { "TradeAttribute25", (obj, val) => obj.TradeAttribute25 = val },
                { "TradeAttribute26", (obj, val) => obj.TradeAttribute26 = val },
                { "TradeAttribute27", (obj, val) => obj.TradeAttribute27 = val },
                { "TradeAttribute28", (obj, val) => obj.TradeAttribute28 = val },
                { "TradeAttribute29", (obj, val) => obj.TradeAttribute29 = val },
                { "TradeAttribute30", (obj, val) => obj.TradeAttribute30 = val },
                { "TradeAttribute31", (obj, val) => obj.TradeAttribute31 = val },
                { "TradeAttribute32", (obj, val) => obj.TradeAttribute32 = val },
                { "TradeAttribute33", (obj, val) => obj.TradeAttribute33 = val },
                { "TradeAttribute34", (obj, val) => obj.TradeAttribute34 = val },
                { "TradeAttribute35", (obj, val) => obj.TradeAttribute35 = val },
                { "TradeAttribute36", (obj, val) => obj.TradeAttribute36 = val },
                { "TradeAttribute37", (obj, val) => obj.TradeAttribute37 = val },
                { "TradeAttribute38", (obj, val) => obj.TradeAttribute38 = val },
                { "TradeAttribute39", (obj, val) => obj.TradeAttribute39 = val },
                { "TradeAttribute40", (obj, val) => obj.TradeAttribute40 = val },
                { "TradeAttribute41", (obj, val) => obj.TradeAttribute41 = val },
                { "TradeAttribute42", (obj, val) => obj.TradeAttribute42 = val },
                { "TradeAttribute43", (obj, val) => obj.TradeAttribute43 = val },
                { "TradeAttribute44", (obj, val) => obj.TradeAttribute44 = val },
                { "TradeAttribute45", (obj, val) => obj.TradeAttribute45 = val },
            };

        private static readonly Dictionary<string, Func<AdditionalTradeAttributes, string>> _attributeGetters =
            new Dictionary<string, Func<AdditionalTradeAttributes, string>>
            {
                { "TradeAttribute7",  (obj) => obj.TradeAttribute7 },
                { "TradeAttribute8",  (obj) => obj.TradeAttribute8 },
                { "TradeAttribute9",  (obj) => obj.TradeAttribute9 },
                { "TradeAttribute10", (obj) => obj.TradeAttribute10 },
                { "TradeAttribute11", (obj) => obj.TradeAttribute11 },
                { "TradeAttribute12", (obj) => obj.TradeAttribute12 },
                { "TradeAttribute13", (obj) => obj.TradeAttribute13 },
                { "TradeAttribute14", (obj) => obj.TradeAttribute14 },
                { "TradeAttribute15", (obj) => obj.TradeAttribute15 },
                { "TradeAttribute16", (obj) => obj.TradeAttribute16 },
                { "TradeAttribute17", (obj) => obj.TradeAttribute17 },
                { "TradeAttribute18", (obj) => obj.TradeAttribute18 },
                { "TradeAttribute19", (obj) => obj.TradeAttribute19 },
                { "TradeAttribute20", (obj) => obj.TradeAttribute20 },
                { "TradeAttribute21", (obj) => obj.TradeAttribute21 },
                { "TradeAttribute22", (obj) => obj.TradeAttribute22 },
                { "TradeAttribute23", (obj) => obj.TradeAttribute23 },
                { "TradeAttribute24", (obj) => obj.TradeAttribute24 },
                { "TradeAttribute25", (obj) => obj.TradeAttribute25 },
                { "TradeAttribute26", (obj) => obj.TradeAttribute26 },
                { "TradeAttribute27", (obj) => obj.TradeAttribute27 },
                { "TradeAttribute28", (obj) => obj.TradeAttribute28 },
                { "TradeAttribute29", (obj) => obj.TradeAttribute29 },
                { "TradeAttribute30", (obj) => obj.TradeAttribute30 },
                { "TradeAttribute31", (obj) => obj.TradeAttribute31 },
                { "TradeAttribute32", (obj) => obj.TradeAttribute32 },
                { "TradeAttribute33", (obj) => obj.TradeAttribute33 },
                { "TradeAttribute34", (obj) => obj.TradeAttribute34 },
                { "TradeAttribute35", (obj) => obj.TradeAttribute35 },
                { "TradeAttribute36", (obj) => obj.TradeAttribute36 },
                { "TradeAttribute37", (obj) => obj.TradeAttribute37 },
                { "TradeAttribute38", (obj) => obj.TradeAttribute38 },
                { "TradeAttribute39", (obj) => obj.TradeAttribute39 },
                { "TradeAttribute40", (obj) => obj.TradeAttribute40 },
                { "TradeAttribute41", (obj) => obj.TradeAttribute41 },
                { "TradeAttribute42", (obj) => obj.TradeAttribute42 },
                { "TradeAttribute43", (obj) => obj.TradeAttribute43 },
                { "TradeAttribute44", (obj) => obj.TradeAttribute44 },
                { "TradeAttribute45", (obj) => obj.TradeAttribute45 },
            };
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes all trade attributes to empty string.
        /// </summary>
        public AdditionalTradeAttributes()
        {
            try
            {
                foreach (var setter in _attributeSetters.Values)
                {
                    setter(this, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion

        #region Getter Methods

        /// <summary>
        /// Returns all non-empty trade attributes as a JSON array.
        /// </summary>
        public virtual string GetTradeAttributesAsJson()
        {
            var attributesArray = new JArray();

            foreach (var entry in _attributeGetters)
            {
                string value = entry.Value(this);
                if (!string.IsNullOrEmpty(value))
                {
                    attributesArray.Add(new JObject
                    {
                        ["Name"] = entry.Key,
                        ["Value"] = value
                    });
                }
            }
            return attributesArray.ToString();
        }

        /// <summary>
        /// Returns all trade attributes as a dictionary.
        /// </summary>
        public virtual Dictionary<string, string> GetTradeAttributesAsDict()
        {
            var attributesArray = new Dictionary<string, string>();

            foreach (var entry in _attributeGetters)
            {
                attributesArray.Add(entry.Key, entry.Value(this));
            }
            return attributesArray;
        }

        /// <summary>
        /// Gets a single trade attribute value by name.
        /// </summary>
        public virtual string GetTradeAttributeValue(string attributeName)
        {
            return _attributeGetters.TryGetValue(attributeName, out var getter) ? getter(this) : null;
        }

        /// <summary>
        /// Returns all trade attribute values as a list.
        /// </summary>
        public virtual List<string> GetTradeAttributesAsList()
        {
            return _attributeGetters.Select(getter => getter.Value(this)).ToList();
        }

        #endregion

        #region Setter Methods

        /// <summary>
        /// Sets trade attribute values from a JSON string.
        /// </summary>
        public virtual void SetTradeAttribute(string json)
        {
            try
            {
                if (!string.IsNullOrEmpty(json) && IsValidJSON(json, out JArray array))
                {
                    foreach (JObject obj in array)
                    {
                        string name = (string)obj["Name"];
                        string value = (string)obj["Value"];
                        if (_attributeSetters.TryGetValue(name, out var setter))
                        {
                            setter(this, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sets a single trade attribute value.
        /// </summary>
        public virtual void SetTradeAttributeValue(string attributeName, string value)
        {
            if (_attributeSetters.TryGetValue(attributeName, out var setter))
            {
                setter(this, value ?? string.Empty);
            }
        }

        /// <summary>
        /// Sets trade attribute values from a dictionary.
        /// </summary>
        public virtual void SetTradeAttribute(Dictionary<string, string> attributeValues)
        {
            try
            {
                foreach(var attributePair in attributeValues)
                {
                    SetTradeAttributeValue(attributePair.Key, attributePair.Value);
                }             
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Sets only the non-empty trade attribute values from the provided dictionary.
        /// Attributes with empty string values will be ignored.
        /// </summary>
        public virtual void SetNonEmptyTradeAttributes(Dictionary<string, string> attributeValues)
        {
            try
            {
                foreach (var attributePair in attributeValues)
                {
                    if (!string.IsNullOrWhiteSpace(attributePair.Value))
                    {
                        SetTradeAttributeValue(attributePair.Key, attributePair.Value);                      
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Resets all trade attribute values.
        /// </summary>
        public virtual void ResetTradeAttributes()
        {
            foreach (var setter in _attributeSetters.Values)
            {
                setter(this, string.Empty);
            }
        }

        /// <summary>
        /// This method checks the correctness of string being in JSON format
        /// </summary>
        /// <param name="json"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool IsValidJSON(string json, out JArray result)
        {
            result = null;
            try
            {
                result = JArray.Parse(json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
