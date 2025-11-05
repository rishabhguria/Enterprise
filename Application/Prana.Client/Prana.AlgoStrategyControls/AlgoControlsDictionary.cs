using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Prana.AlgoStrategyControls
{
    public class AlgoControlsDictionary
    {
        private AlgoControlsDictionary _algoControlClass = null;

        private Dictionary<string, Dictionary<string, AlgoStrategy>> _dictCompleteDictOfCtrls = null;

        private Dictionary<string, string> _algoStrategyNames = new Dictionary<string, string>();
        private Dictionary<string, string> _dictcounterPartyLogoName = new Dictionary<string, string>();

        private static readonly Lazy<AlgoControlsDictionary> lazy = new Lazy<AlgoControlsDictionary>(() => new AlgoControlsDictionary());
        public static AlgoControlsDictionary GetInstance()
        {

            return lazy.Value;

        }

        private AlgoControlsDictionary()
        {
        }

        public Dictionary<string, string> AlgoStrategyNames
        {
            get { return _algoStrategyNames; }
            set { _algoStrategyNames = value; }
        }

        /// <summary>
        /// Get Load Strategy Controls
        /// </summary>
        public void GetLoadStrategyControls()
        {
            try
            {
                if (_algoControlClass == null)
                {
                    _dictCompleteDictOfCtrls = new Dictionary<string, Dictionary<string, AlgoStrategy>>();
                    _algoControlClass = new AlgoControlsDictionary();
                    _algoStrategyNames = new Dictionary<string, string>();
                    _algoStrategyNames.Add(int.MinValue.ToString(), "Algo Type");
                    AlgoStrategyNamesDetails.AlgoStrategyNamesInfo.Add(int.MinValue.ToString(), "Algo Type");
                    _dictcounterPartyLogoName = new Dictionary<string, string>();
                    string path = Application.StartupPath + "\\xmls\\MYControls.xml";
                    SetUp(path);
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


        private void SetUp(string path)
        {
            int count = 0;
            try
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(path);
                XmlNodeList xmlNodesCounterparty = xmldoc.SelectNodes("CounterParties / CounterParty");
                foreach (XmlNode xmlNodeCp in xmlNodesCounterparty)
                {
                    Dictionary<string, AlgoStrategy> dictAlgoStrat = new Dictionary<string, AlgoStrategy>();
                    _dictCompleteDictOfCtrls.Add(xmlNodeCp.Attributes["ID"].Value, dictAlgoStrat);

                    if (xmlNodeCp.Attributes["CounterPartyLogoName"] != null)
                    {
                        _dictcounterPartyLogoName.Add(xmlNodeCp.Attributes["ID"].Value, xmlNodeCp.Attributes["CounterPartyLogoName"].Value);
                    }
                    //Dictionary<string, AlgoStrategyUserControl> dictAlgoStratUC = new Dictionary<string, AlgoStrategyUserControl>();

                    XmlNodeList xmlNodeAlgoStrategyList = xmlNodeCp.SelectNodes("Strategies/AlgoStrategy");
                    foreach (XmlNode xmlNodeAlgoStrategy in xmlNodeAlgoStrategyList)
                    {
                        string strategyID = xmlNodeAlgoStrategy.Attributes["ID"].Value;
                        AlgoStrategy algoStrategy = new AlgoStrategy();
                        algoStrategy.StrategyID = strategyID;
                        if (xmlNodeCp.Attributes["StrategyTags"] != null && xmlNodeAlgoStrategy.Attributes["StrategyValues"] != null)
                        {
                            string StrategyTags = xmlNodeCp.Attributes["StrategyTags"].Value;
                            string StrategyValues = xmlNodeAlgoStrategy.Attributes["StrategyValues"].Value;
                            algoStrategy.AddTagValues(StrategyTags, StrategyValues);
                        }
                        if (xmlNodeAlgoStrategy.Attributes["AvailableInRegions"] != null)
                        {
                            algoStrategy.AvailableInRegions = ConvertToList(xmlNodeAlgoStrategy.Attributes["AvailableInRegions"].Value);
                        }
                        algoStrategy.StrategyName = xmlNodeAlgoStrategy.Attributes["Name"].Value;
                        if (xmlNodeAlgoStrategy.Attributes["IsSyntheticReplace"] != null)
                        {
                            algoStrategy.IsSyntheticReplace = Convert.ToBoolean(xmlNodeAlgoStrategy.Attributes["IsSyntheticReplace"].Value);
                        }
                        if (xmlNodeAlgoStrategy.Attributes["CustomMessage"] != null)
                        {
                            algoStrategy.CustomMessage = (xmlNodeAlgoStrategy.Attributes["CustomMessage"].Value);
                        }
                        dictAlgoStrat.Add(strategyID, algoStrategy);
                        _algoStrategyNames.Add(algoStrategy.StrategyID, algoStrategy.StrategyName);
                        AlgoStrategyNamesDetails.AlgoStrategyNamesInfo.Add(xmlNodeCp.Attributes["ID"].Value + "_" + strategyID, algoStrategy.StrategyName);

                        XmlNodeList xmlNodeParameterList = xmlNodeAlgoStrategy.SelectNodes("Parameters/Parameter");
                        count++;
                        //AlgoStrategyParametersToUpdate algoStrategyParametersToUpdateValues = new AlgoStrategyParametersToUpdate();

                        foreach (XmlNode xmlNodeParameter in xmlNodeParameterList)
                        {
                            AlgoStrategyParameters algoStrategyParameters = new AlgoStrategyParameters();
                            Dictionary<string, Dictionary<string, AlgoStrategyParametersToUpdate>> ParameterDictToUpdate = new Dictionary<string, Dictionary<string, AlgoStrategyParametersToUpdate>>();
                            try
                            {

                                XmlNodeList xmlNodesParametersToUpdate = xmlNodeParameter.SelectNodes("Trigger / Condition ");
                                foreach (XmlNode xmlNodeParameters in xmlNodesParametersToUpdate)
                                {
                                    Dictionary<string, AlgoStrategyParametersToUpdate> dictChildParamWiseUpdateParams = new Dictionary<string, AlgoStrategyParametersToUpdate>();
                                    XmlNodeList xmlNodesParametersSelect = xmlNodeParameters.SelectNodes("Actions / Action");
                                    string SelectdValue = xmlNodeParameters.Attributes[0].Value;
                                    foreach (XmlNode xmlNodeParamSelect in xmlNodesParametersSelect)
                                    {

                                        List<string> childparams = ConvertToList(xmlNodeParamSelect.Attributes[3].Value);
                                        foreach (string item in childparams)
                                        {
                                            AlgoStrategyParametersToUpdate algoStrategyParametersToUpdate = new AlgoStrategyParametersToUpdate();
                                            algoStrategyParametersToUpdate.ValidateWithOrderProperty = new List<string>();
                                            if (dictChildParamWiseUpdateParams.ContainsKey(item))
                                            {
                                                AlgoStrategyParametersToUpdate param = dictChildParamWiseUpdateParams[item];

                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "MinValue")
                                                    param.MinValue = xmlNodeParamSelect.Attributes[2].Value;
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "MaxValue")
                                                    param.MaxValue = xmlNodeParamSelect.Attributes[2].Value;
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Enabled")
                                                    param.Enabled = Convert.ToBoolean(xmlNodeParamSelect.Attributes[2].Value);
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Visibility")
                                                    param.Visibility = Convert.ToBoolean(xmlNodeParamSelect.Attributes[2].Value);
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Default")
                                                    param.Default = xmlNodeParamSelect.Attributes[2].Value;
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Increment")
                                                    param.Increment = xmlNodeParamSelect.Attributes[2].Value;
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "SelectedValue")
                                                    param.SelectedValue = xmlNodeParamSelect.Attributes[2].Value;
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Checked")
                                                    param.Checked = Convert.ToBoolean(xmlNodeParamSelect.Attributes[2].Value);
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Required")
                                                    param.Required = Convert.ToBoolean(xmlNodeParamSelect.Attributes[2].Value);
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "ReuiredGroupName")
                                                    param.ReuiredGroupName = (xmlNodeParamSelect.Attributes[2].Value);
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "ValidateWithOrderProperty")
                                                {
                                                    param.ValidateWithOrderProperty.Add(xmlNodeParamSelect.Attributes[2].Value);
                                                }

                                            }
                                            else
                                            {
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "MinValue")
                                                    algoStrategyParametersToUpdate.MinValue = xmlNodeParamSelect.Attributes[2].Value;
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "MaxValue")
                                                    algoStrategyParametersToUpdate.MaxValue = xmlNodeParamSelect.Attributes[2].Value;
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Enabled")
                                                    algoStrategyParametersToUpdate.Enabled = Convert.ToBoolean(xmlNodeParamSelect.Attributes[2].Value);
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Visibility")
                                                    algoStrategyParametersToUpdate.Visibility = Convert.ToBoolean(xmlNodeParamSelect.Attributes[2].Value);
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Default")
                                                    algoStrategyParametersToUpdate.Default = xmlNodeParamSelect.Attributes[2].Value;
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Increment")
                                                    algoStrategyParametersToUpdate.Increment = xmlNodeParamSelect.Attributes[2].Value;
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "SelectedValue")
                                                    algoStrategyParametersToUpdate.SelectedValue = xmlNodeParamSelect.Attributes[2].Value;
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Checked")
                                                    algoStrategyParametersToUpdate.Checked = Convert.ToBoolean(xmlNodeParamSelect.Attributes[2].Value);
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "Required")
                                                    algoStrategyParametersToUpdate.Required = Convert.ToBoolean(xmlNodeParamSelect.Attributes[2].Value);
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "ReuiredGroupName")
                                                    algoStrategyParametersToUpdate.ReuiredGroupName = (xmlNodeParamSelect.Attributes[2].Value);
                                                if (xmlNodeParamSelect.Attributes["name"] != null && xmlNodeParamSelect.Attributes["name"].Value == "ValidateWithOrderProperty")
                                                {
                                                    algoStrategyParametersToUpdate.ValidateWithOrderProperty.Add(xmlNodeParamSelect.Attributes[2].Value);
                                                }

                                                dictChildParamWiseUpdateParams.Add(item, algoStrategyParametersToUpdate);
                                            }


                                        }


                                        if (xmlNodeParameter.Attributes["ChildParameterID"] != null)
                                        {
                                            algoStrategyParameters.ChildParameterID = xmlNodeParameter.Attributes["ChildParameterID"].Value;
                                        }

                                    }
                                    ParameterDictToUpdate.Add(SelectdValue, dictChildParamWiseUpdateParams);
                                }

                                algoStrategyParameters.ValidateWith = new List<string>();
                                algoStrategyParameters.ValidateWithOrderProperty = new List<string>();

                                if (xmlNodeParameter.Attributes["ID"] != null)
                                {
                                    algoStrategyParameters.IDs = ConvertToList(xmlNodeParameter.Attributes["ID"].Value);
                                }
                                if (xmlNodeParameter.Attributes["Name"] != null)
                                {
                                    algoStrategyParameters.Names = ConvertToList(xmlNodeParameter.Attributes["Name"].Value);
                                }
                                if (xmlNodeParameter.Attributes["ControlType"] != null)
                                {
                                    algoStrategyParameters.ControlType = xmlNodeParameter.Attributes["ControlType"].Value;
                                }
                                if (xmlNodeParameter.Attributes["InnerControlNames"] != null)
                                {
                                    algoStrategyParameters.InnerControlNames = ConvertToList(xmlNodeParameter.Attributes["InnerControlNames"].Value);
                                }
                                if (xmlNodeParameter.Attributes["InnerControlValues"] != null)
                                {
                                    algoStrategyParameters.InnerControlValues = ConvertToList(xmlNodeParameter.Attributes["InnerControlValues"].Value);
                                }
                                if (xmlNodeParameter.Attributes["XPos"] != null)
                                {
                                    algoStrategyParameters.Xpos = Convert.ToInt32(xmlNodeParameter.Attributes["XPos"].Value);
                                }
                                if (xmlNodeParameter.Attributes["YPos"] != null)
                                {
                                    algoStrategyParameters.Ypos = Convert.ToInt32(xmlNodeParameter.Attributes["YPos"].Value);
                                }
                                if (xmlNodeParameter.Attributes["Type"] != null)
                                {
                                    algoStrategyParameters.Type = xmlNodeParameter.Attributes["Type"].Value;
                                }
                                if (xmlNodeParameter.Attributes["ReplaceVisible"] != null)
                                {
                                    algoStrategyParameters.ReplaceVisible = xmlNodeParameter.Attributes["ReplaceVisible"].Value;
                                }

                                algoStrategyParameters.IDtoNameMapping = GetIDstoNamesMapping(algoStrategyParameters.IDs, algoStrategyParameters.Names);


                                if (xmlNodeParameter.Attributes["ControlCategory"] != null)
                                {
                                    algoStrategyParameters.ControlCategory = xmlNodeParameter.Attributes["ControlCategory"].Value;
                                }

                                if (xmlNodeParameter.Attributes["ChildParameterID"] != null)
                                {
                                    algoStrategyParameters.ChildParameterID = xmlNodeParameter.Attributes["ChildParameterID"].Value;
                                }
                                //Initializing the InnerControlList
                                string regionWiseValuesAvailable = "";
                                if (xmlNodeParameter.Attributes["RegionWiseValueAvailable"] != null)
                                {
                                    regionWiseValuesAvailable = xmlNodeParameter.Attributes["RegionWiseValueAvailable"].Value;
                                }
                                if (algoStrategyParameters.InnerControlValues != null && !string.IsNullOrEmpty(regionWiseValuesAvailable))
                                    algoStrategyParameters.ValueWiseRegionAvailabilty = GetInnerControlList(algoStrategyParameters.InnerControlValues, regionWiseValuesAvailable.Split(';').ToList());


                                if (xmlNodeParameter.Attributes["ValidationWith"] != null)
                                {
                                    algoStrategyParameters.ValidateWith = ((xmlNodeParameter.Attributes["ValidationWith"].Value).Split(',').ToList());
                                }

                                if (xmlNodeParameter.Attributes["IsReuiredGroupName"] != null)
                                {
                                    string key = (xmlNodeParameter.Attributes["IsReuiredGroupName"].Value);
                                    if (!string.IsNullOrEmpty(key))
                                        algoStrategyParameters.ReuiredGroupName = key;

                                }


                                if (xmlNodeParameter.Attributes["ValidateWithOrderProperty"] != null)
                                {
                                    algoStrategyParameters.ValidateWithOrderProperty.Add(xmlNodeParameter.Attributes["ValidateWithOrderProperty"].Value);
                                }
                                if (xmlNodeParameter.Attributes["RequiredInRegions"] != null)
                                {
                                    algoStrategyParameters.RequiredInRegions = ConvertToList(xmlNodeParameter.Attributes["RequiredInRegions"].Value);
                                }

                                if (xmlNodeParameter.Attributes["Format"] != null)
                                {
                                    algoStrategyParameters.Format = xmlNodeParameter.Attributes["Format"].Value;
                                }
                                if (xmlNodeParameter.Attributes["SendOnReplace"] != null)
                                {
                                    algoStrategyParameters.SendOnReplace = Convert.ToBoolean(xmlNodeParameter.Attributes["SendOnReplace"].Value);
                                }
                                if (xmlNodeParameter.Attributes["AvailableInRegions"] != null)
                                {
                                    algoStrategyParameters.AvailableInRegions = ConvertToList(xmlNodeParameter.Attributes["AvailableInRegions"].Value);
                                }
                                if (xmlNodeParameter.Attributes["EnableInRegions"] != null)
                                {
                                    algoStrategyParameters.EnableInRegions = ConvertToList(xmlNodeParameter.Attributes["EnableInRegions"].Value);
                                }

                                if (xmlNodeParameter.Attributes["IsEnabled"] != null)
                                {
                                    algoStrategyParameters.IsEnabled = Convert.ToBoolean(xmlNodeParameter.Attributes["IsEnabled"].Value);
                                }
                                if (xmlNodeParameter.Attributes["DefaultRegionKey"] != null && xmlNodeParameter.Attributes["DefaultRegionValue"] != null)
                                {
                                    List<string> defaultregion = ConvertToList(xmlNodeParameter.Attributes["DefaultRegionKey"].Value);
                                    List<string> defaultregionvalues = ConvertToList(xmlNodeParameter.Attributes["DefaultRegionValue"].Value);
                                    algoStrategyParameters.RegionWiseDefaultValues = GetIDstoNamesMapping(defaultregion, defaultregionvalues);
                                }
                                if (xmlNodeParameter.Attributes["CustomAttributesKey"] != null && xmlNodeParameter.Attributes["CustomAttributesValue"] != null)
                                {
                                    List<string> customAttributeNames = ConvertToList(xmlNodeParameter.Attributes["CustomAttributesKey"].Value);
                                    List<string> customAttributeValues = ConvertToList(xmlNodeParameter.Attributes["CustomAttributesValue"].Value);
                                    algoStrategyParameters.CustomAttributesDict = GetIDstoNamesMapping(customAttributeNames, customAttributeValues);
                                }

                                algoStrategy.AlgoparametersList.Add(algoStrategyParameters);
                                //AlgoStrategyUserControl algoStrategyUserControl = GetControl(algoStrategyParameters.GUIControl);
                                AlgoStrategyUserControl algoStrategyUserControl = _algoControlClass.GetControl(algoStrategyParameters);
                                if (algoStrategyUserControl != null)
                                {

                                    algoStrategyParameters._algoStrategyParametersToUpdate = ParameterDictToUpdate;

                                    algoStrategyUserControl.SetAlgoStrategyParameters(algoStrategyParameters);
                                }
                                algoStrategy.AlgoStrategyCtrlDict.Add(GetStringFromLst(algoStrategyParameters.IDs), algoStrategyUserControl);
                            }
                            catch (Exception ex)
                            {
                                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                                if (rethrow)
                                {
                                    throw new Exception("Error in initializing " + GetStringFromLst(algoStrategyParameters.Names) + " for strategy " + algoStrategy.StrategyName + ":" + ex.Message);
                                }
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private Dictionary<string, List<string>> GetInnerControlList(List<string> innerControlValues, List<string> regionWiseValuesAvailable)
        {
            Dictionary<string, List<string>> regionWiseValuesAvaialability = new Dictionary<string, List<string>>();

            int i = 0;
            foreach (string innerControlValue in innerControlValues)
            {
                if (!(regionWiseValuesAvailable[i]).Contains("All"))
                {
                    regionWiseValuesAvaialability.Add(innerControlValue, regionWiseValuesAvailable[i].Split(',').ToList());
                }
                i++;
            }


            return regionWiseValuesAvaialability;
        }


        private Dictionary<string, string> GetIDstoNamesMapping(List<string> IDList, List<string> NamesList)
        {
            Dictionary<string, string> IDstoNamesMapping = new Dictionary<string, string>();

            int i = 0;
            foreach (string ID in IDList)
            {
                IDstoNamesMapping.Add(ID, NamesList[i]);
                i++;
            }
            return IDstoNamesMapping;
        }



        private List<string> ConvertToList(string input)
        {
            List<string> stringList = new List<string>();
            if (string.IsNullOrEmpty(input))
                return stringList;
            char seperator = ',';
            string[] strArray = input.Split(seperator);


            foreach (string str in strArray)
            {
                stringList.Add(str);
            }
            return stringList;

        }





        public AlgoStrategyUserControl GetControl(AlgoStrategyParameters algoStrategyParameters)
        {
            AlgoStrategyUserControl algoStrategyUserControl = null;

            try
            {
                if (algoStrategyParameters.Type == "PranaUC")
                {
                    Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.StartupPath + "//" + "AlgoStrategyControls" + "//" + algoStrategyParameters.ControlType);
                    //Gets the first type in the assembly which the UserControl
                    Type typeToLoad = assembly.GetTypes()[0];
                    //Type typeToLoad = assembly.GetType(algoStrategyParameters.ControlType);
                    if (typeToLoad != null)
                    {
                        algoStrategyUserControl = (AlgoStrategyUserControl)Activator.CreateInstance(typeToLoad);
                    }
                }
                else if (algoStrategyParameters.Type == "Generic")
                {
                    algoStrategyUserControl = new GenericControl();
                }
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {

                    throw;

                }
            }
            return algoStrategyUserControl;
        }


        public string GetCounterPartyLogoName(string cpID)
        {
            if (_dictcounterPartyLogoName.ContainsKey(cpID))
            {
                return _dictcounterPartyLogoName[cpID];
            }
            return null;
        }
        public DataTable GetAllowedStrategies(string cpID, string underlyingText)
        {
            DataTable dt = null;
            dt = new DataTable();
            dt.Columns.Add("StrategyID");
            dt.Columns.Add("StrategyName");
            object[] row = new object[2];
            row[0] = int.MinValue;
            row[1] = "Algo Type";
            dt.Rows.Add(row);
            if (_dictCompleteDictOfCtrls.ContainsKey(cpID))
            {
                Dictionary<string, AlgoStrategy> strategies = _dictCompleteDictOfCtrls[cpID];
                foreach (KeyValuePair<string, AlgoStrategy> strategy in strategies)
                {
                    if (strategy.Value.AvailableInRegions == null || strategy.Value.AvailableInRegions.Contains(underlyingText))
                    {
                        object[] row1 = new object[2];
                        row1[0] = strategy.Value.StrategyID;
                        row1[1] = strategy.Value.StrategyName;
                        dt.Rows.Add(row1);
                    }
                }
            }
            return dt;
        }


        public Dictionary<string, string> GetCPWiseAllStrategies(string cpID)
        {

            Dictionary<string, string> dictCPWiseALgoType = new Dictionary<string, string>();
            dictCPWiseALgoType.Add(int.MinValue.ToString(), "Algo Type");
            if (_dictCompleteDictOfCtrls.ContainsKey(cpID))
            {
                Dictionary<string, AlgoStrategy> strategies = _dictCompleteDictOfCtrls[cpID];
                foreach (KeyValuePair<string, AlgoStrategy> strategy in strategies)
                {
                    if (!dictCPWiseALgoType.ContainsKey((strategy.Value.StrategyID)))
                        dictCPWiseALgoType.Add((strategy.Value.StrategyID), strategy.Value.StrategyName);
                }
            }
            return dictCPWiseALgoType;
        }


        public Dictionary<string, AlgoStrategyUserControl> GetSelectedStrategyControls(string counterPartyID, string strategyID, string underlyingText)
        {
            if (_dictCompleteDictOfCtrls.ContainsKey(counterPartyID))
            {
                if (_dictCompleteDictOfCtrls[counterPartyID].ContainsKey(strategyID))
                {
                    return _dictCompleteDictOfCtrls[counterPartyID][strategyID].GetClonedCtrls(underlyingText);
                }
            }
            return null;
        }

        public AlgoStrategy GetAlgoStrategyDatils(string cpID, string algoStrategyID)
        {
            if (_dictCompleteDictOfCtrls.ContainsKey(cpID))
            {
                if (_dictCompleteDictOfCtrls[cpID].ContainsKey(algoStrategyID))
                {
                    return _dictCompleteDictOfCtrls[cpID][algoStrategyID];
                }
            }
            return null;
        }
        // static Dictionary<string, string> dicStrategiID_Name = null;


        public string GetAlgoStrategyText(string ID)
        {
            //if (dicStrategiID_Name == null)
            //{
            //    string path = Application.StartupPath + "\\xmls\\MYControls.xml";
            //    dicStrategiID_Name = new Dictionary<string, string>();
            //    XmlDocument xmldoc = new XmlDocument();
            //    xmldoc.Load(path);
            //    XmlNodeList xmlNodesCounterparty = xmldoc.SelectNodes("CounterParties / CounterParty");
            //    foreach (XmlNode xmlNodeCp in xmlNodesCounterparty)
            //    {
            //        XmlNodeList xmlNodeAlgoStrategyList = xmlNodeCp.SelectNodes("Strategies/AlgoStrategy");
            //        foreach (XmlNode xmlNodeAlgoStrategy in xmlNodeAlgoStrategyList)
            //        {
            //            string strategyID = xmlNodeAlgoStrategy.Attributes["ID"].Value;
            //            string StrategyName = xmlNodeAlgoStrategy.Attributes["Name"].Value;
            //            if (!dicStrategiID_Name.ContainsKey(strategyID))
            //            {
            //                dicStrategiID_Name.Add(strategyID, StrategyName);
            //            }
            //        }
            //    }
            //}


            if (_algoStrategyNames.ContainsKey(ID))
            {
                return _algoStrategyNames[ID];
            }
            else
            {
                return "N.A.";
            }
        }

        //TDDO shift to helper class
        private static string GetStringFromLst(List<string> list)
        {
            if (list.Count > 0)
            {
                StringBuilder sBuilder = new StringBuilder();
                foreach (string var in list)
                {
                    sBuilder.Append(var);
                }
                return sBuilder.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
