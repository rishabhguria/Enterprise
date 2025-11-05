using Prana.ATDLLibrary.Model.Elements;
using Prana.ATDLLibrary.Xml;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Xml.Linq;

namespace Prana.ATDLLibrary.Providers
{
    public class StrategyProvider
    {
        protected readonly StrategiesReader _strategiesReader = new StrategiesReader();
        protected readonly Dictionary<string, Strategies_t> _strategiesDictionary = new Dictionary<string, Strategies_t>();
        private readonly List<string> controlTypesToCheck = new List<string>
        {
            "lay:Slider_t",
            "lay:RadioButton_t",
            "lay:EditableDropDownList_t",
            "lay:HiddenField_t"
        };
        // XML namespaces used in ATDL files
        private readonly XNamespace ns_core = "http://www.fixprotocol.org/FIXatdl-1-1/Core";
        private readonly XNamespace ns_lay = "http://www.fixprotocol.org/FIXatdl-1-1/Layout";
        private readonly XNamespace ns_xsi = "http://www.w3.org/2001/XMLSchema-instance";

        public StrategyProvider()
        {
        }

        public void Load(string providerId, string path)
        {
            try
            {
                Strategies_t strategies = _strategiesReader.Load(path);

                _strategiesDictionary[providerId] = strategies;

                Logger.LogMsg(LoggerLevel.Information, "Laod algo strategies from {path} for providerId {id}", path, providerId);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        public void LogAtdlFile(string providerId, string filePath)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);
                Console.WriteLine($"Analysing ATDL {fileName} for logging: ");
                Logger.LogToFile($"Analysing ATDL {fileName} for logging: ", LoggingConstants.ATDL_LOG_FILE_NAME);

                // Load the ATDL XML file
                XDocument doc = XDocument.Load(filePath);

                // Get all strategies in the file
                var strategies = doc.Descendants(ns_core + "Strategy");

                foreach (var strategy in strategies)
                {
                    string strategyName = strategy.Attribute("name")?.Value ?? "Unknown";
                    Logger.LogToFile($"Validating Strategy: {strategyName}", LoggingConstants.ATDL_LOG_FILE_NAME);

                    // 1. Check for specific control types
                    CheckForSpecificControlTypes(strategy);

                    // 2. Check for controls without parameterRef
                    CheckForControlsWithoutParameterRef(strategy);
                }

                Console.WriteLine($"ATDL {fileName} logging completed. ");
                Logger.LogToFile($"ATDL {fileName} logging completed. ", LoggingConstants.ATDL_LOG_FILE_NAME);
            }
            catch (Exception ex)
            {
                Logger.LogToFile($"Error analysing ATDL file for logging: {ex.Message}", LoggingConstants.ATDL_LOG_FILE_NAME);
            }
        }

        private void CheckForSpecificControlTypes(XElement strategy)
        {
            Logger.LogToFile("1. Following Controls not implemented: ", LoggingConstants.ATDL_LOG_FILE_NAME);

            var allControls = strategy.Descendants(ns_lay + "Control");

            foreach (var control in allControls)
            {
                var typeAttribute = control.Attribute(ns_xsi + "type");
                if (typeAttribute != null && controlTypesToCheck.Contains(typeAttribute.Value))
                {
                    string controlId = control.Attribute("ID")?.Value ?? "Unknown";
                    string controlType = typeAttribute.Value;
                    Logger.LogToFile($"   - Control ID: {controlId}, Type: {controlType}", LoggingConstants.ATDL_LOG_FILE_NAME);
                }
            }
        }

        private void CheckForControlsWithoutParameterRef(XElement strategy)
        {
            Logger.LogToFile("2. Following Controls not assosciated with any Parameter: ", LoggingConstants.ATDL_LOG_FILE_NAME);

            var allControls = strategy.Descendants(ns_lay + "Control");

            foreach (var control in allControls)
            {
                var parameterRefAttr = control.Attribute("parameterRef");
                if (parameterRefAttr == null)
                {
                    string controlId = control.Attribute("ID")?.Value ?? "Unknown";
                    string controlType = control.Attribute(ns_xsi + "type")?.Value ?? "Unknown";
                    Logger.LogToFile($"   - Control ID: {controlId}, Type: {controlType}", LoggingConstants.ATDL_LOG_FILE_NAME);
                }
            }
        }

        public Strategies_t GetStrategiesByProvider(string providerId)
        {
            Strategies_t strategies_t = new Strategies_t();
            try
            {
                strategies_t = _strategiesDictionary.ContainsKey(providerId) ? _strategiesDictionary[providerId] : null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "GetStrategiesByProvider encountered an error for providerId: " + providerId);
            }
            return strategies_t;
        }

        public Strategy_t GetStrategyByName(string providerId, string name)
        {
            try
            {
                Strategies_t strategies = _strategiesDictionary[providerId];

                Strategy_t strategy = strategies[name];

                return strategy;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"GetStrategyByName encountered an error for providerId: {providerId} and algoName:{name}");
            }
            return null;
        }

        /// <summary>
        /// Returns Name wise algo strategy UiRep 
        /// </summary>
        public Dictionary<string, Dictionary<string, dynamic>> GetAllStrategiesAlgoInfo()
        {
            Dictionary<string, Dictionary<string, dynamic>> _allStrategiesInfo = new Dictionary<string, Dictionary<string, dynamic>>();
            try
            {
                foreach (var strategies_t in _strategiesDictionary)
                {
                    // Create or get the inner dictionary for the current key
                    if (!_allStrategiesInfo.ContainsKey(strategies_t.Key))
                    {
                        _allStrategiesInfo[strategies_t.Key] = new Dictionary<string, dynamic>();
                    }

                    foreach (Strategy_t strategy in strategies_t.Value)
                    {
                        dynamic exp = new ExpandoObject();
                        exp.Name = strategy.Name;
                        exp.UiRep = strategy.UiRep;
                        exp.Regions = strategy.Regions;

                        //adding strategy info against strategy name
                        _allStrategiesInfo[strategies_t.Key][strategy.Name] = exp;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"GetAllStrategiesAlgoInfo encountered an error");
            }

            return _allStrategiesInfo;

        }

        /// <summary>
        /// Returns Algo strategies 
        /// </summary>
        public Dictionary<string, string> GetAllStrategiesInfo()
        {
            Dictionary<string, string> _allStrategiesInfo = new Dictionary<string, string>();
            try
            {
                foreach (var entry in _strategiesDictionary)
                {
                    string dictionaryKey = entry.Key;
                    Strategies_t strategies_t = entry.Value;

                    foreach (Strategy_t strategy in strategies_t.Strategies)
                    {
                        string uniqueKey = $"{dictionaryKey}_{strategy.Name}";
                        if (!_allStrategiesInfo.ContainsKey(uniqueKey))
                        {
                            _allStrategiesInfo.Add(uniqueKey, strategy.UiRep);
                        }
                        else
                        {
                            Logger.LoggerWrite($"Key {uniqueKey} already exist in _allStrategiesInfo");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"GetAllStrategiesInfo encountered an error");
            }
            return _allStrategiesInfo;
        }
    }
}
