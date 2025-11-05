using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace Prana.AlgoStrategyControls
{
    public class AlgoStrategy
    {
        private string ID;
        private List<string> _availableInRegions;

        public List<string> AvailableInRegions
        {
            get { return _availableInRegions; }
            set { _availableInRegions = value; }
        }
        string _customMessage = string.Empty;

        public string CustomMessage
        {
            get { return _customMessage; }
            set { _customMessage = value; }
        }
        public string StrategyID
        {
            get { return ID; }
            set { ID = value; }
        }

        private string _name;
        private bool _IsSyntheticReplace = false;
        public bool IsSyntheticReplace
        {
            get { return _IsSyntheticReplace; }
            set { _IsSyntheticReplace = value; }
        }
        public string StrategyName
        {
            get { return _name; }
            set { _name = value; }
        }

        private Dictionary<string, string> _algoStrategyTagValues = new Dictionary<string, string>();

        private List<AlgoStrategyParameters> _AlgoparametersList = new List<AlgoStrategyParameters>();

        public List<AlgoStrategyParameters> AlgoparametersList
        {
            get { return _AlgoparametersList; }
            set { _AlgoparametersList = value; }
        }

        Dictionary<string, AlgoStrategyUserControl> _algoStrategyCtrlDict = new Dictionary<string, AlgoStrategyUserControl>();
        public Dictionary<string, AlgoStrategyUserControl> AlgoStrategyCtrlDict
        {
            get
            {

                return _algoStrategyCtrlDict;
            }
        }
        public Dictionary<string, AlgoStrategyUserControl> GetClonedCtrls(string underlyingText)
        {

            Dictionary<string, AlgoStrategyUserControl> newInstAlgoStrategyCtrlDict = new Dictionary<string, AlgoStrategyUserControl>();
            foreach (KeyValuePair<string, AlgoStrategyUserControl> ctrl in _algoStrategyCtrlDict)
            {
                try
                {
                    if (ctrl.Value.IsEnableInRegionsParamDefined())
                    {
                        var strategyctrl = ctrl.Value.GetUserCtrl(underlyingText);
                        if (!ctrl.Value.IsEnableInRegions(underlyingText))
                        {
                            strategyctrl.Enabled = false;
                        }
                        newInstAlgoStrategyCtrlDict.Add(ctrl.Key, strategyctrl);
                    }
                    else if (ctrl.Value.IsAvailableInRegions(underlyingText))
                    {
                        newInstAlgoStrategyCtrlDict.Add(ctrl.Key, ctrl.Value.GetUserCtrl(underlyingText));

                    }
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                    if (rethrow)
                    {
                        throw new Exception("Error in setting Strategy " + StrategyName + ctrl.Value._parameters.Names + " with IDs " + ctrl.Value._parameters.IDs + " : " + ex.Message);
                    }
                }

            }
            return newInstAlgoStrategyCtrlDict;

        }



        /// <summary>
        /// 
        /// </summary>
        public void AddTagValues(string commaSeptags, string commaSepvalues)
        {
            string[] tags = commaSeptags.Split(',');
            string[] values = commaSepvalues.Split(',');
            int i = 0;
            foreach (string tag in tags)
            {
                _algoStrategyTagValues.Add(tag, values[i]);
                i++;
            }
        }
        public Dictionary<string, string> StrategyTagValues
        {
            get { return _algoStrategyTagValues; }
        }

    }
}
