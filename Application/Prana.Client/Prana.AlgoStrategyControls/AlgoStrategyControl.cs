using Infragistics.Win;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Prana.AlgoStrategyControls
{
    public partial class AlgoStrategyControl : UserControl
    {
        #region Private Members

        //TableLayoutPanel tablelayout;
        Dictionary<string, AlgoStrategyUserControl> selectedStrategyCtrls;
        AlgoStrategy selectedAlgostrat = null;
        #endregion
        public delegate void SendButtonClicked(Object sender, EventArgs<String> e);
        public event SendButtonClicked ClickedSendButtonEvent;

        string _customMessage = string.Empty;
        public string strategyId = string.Empty;
        public string strategyName = string.Empty;
        public string CustomMessage
        {
            get { return _customMessage; }
            set { _customMessage = value; }
        }

        int _maxPanelHeight = 120;

        public int MaxPanelHeight
        {
            get { return _maxPanelHeight; }
            set { _maxPanelHeight = value; }
        }

        int _maxPanelWidth = 400;
        public int MaxPanelWidth
        {
            get { return _maxPanelWidth; }
            set { _maxPanelWidth = value; }
        }
        public AlgoStrategyControl()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(209, 210, 212);
            this.panel1.BackColor = Color.FromArgb(209, 210, 212);
            this.Disposed += AlgoStrategyControl_Disposed;
        }

        void AlgoStrategyControl_Disposed(object sender, EventArgs e)
        {
            try
            {
                base.Dispose();
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
        /// <summary>
        /// Set strategy name and id
        /// </summary>
        /// <param name="sId"></param>
        /// <param name="sName"></param>
        public void SetStrategyNameAndId(string sId, string sName)
        {
            strategyId = sId;
            strategyName = sName;
        }
        /// <summary>
        /// Create Algo Controls
        /// </summary>
        /// <param name="selectedCounterPartyID"></param>
        /// <param name="selectedStrategyID"></param>
        /// <param name="underlyingText"></param>
        public void CreateAlgoControls(string selectedCounterPartyID, string selectedStrategyID, string underlyingText, bool sendButtonVisibility, bool isAlgoControlPopUp)
        {
            int sndBtnXpos = 0, sndBtnYpos = 0;
            selectedStrategyCtrls = AlgoStrategyControls.AlgoControlsDictionary.GetInstance().GetSelectedStrategyControls(selectedCounterPartyID, selectedStrategyID, underlyingText);
            selectedAlgostrat = AlgoControlsDictionary.GetInstance().GetAlgoStrategyDatils(selectedCounterPartyID, selectedStrategyID);
            for (int i = 0; i < this.panel1.Controls.Count; i++)
            {
                if (!(this.panel1.Controls[i] is Infragistics.Win.Misc.UltraButton))
                    this.panel1.Controls[i].Dispose();
            }
            this.panel1.Controls.Clear();
            if (selectedStrategyCtrls != null)
                foreach (KeyValuePair<string, AlgoStrategyUserControl> selectedStrategyCtrl in selectedStrategyCtrls)
                {
                    AlgoStrategyUserControl ctrl = (AlgoStrategyUserControl)selectedStrategyCtrl.Value;
                    if (ctrl._parameters.Ypos + 30 > _maxPanelHeight)
                    {
                        _maxPanelHeight = ctrl._parameters.Ypos + 30;
                    }
                    if (ctrl._parameters.Xpos + 220 > _maxPanelWidth && isAlgoControlPopUp)
                    {
                        _maxPanelWidth = ctrl._parameters.Xpos + 220;
                    }
                    if (ctrl._parameters.Names[0].Equals("SendButton"))
                    {
                        sndBtnXpos = ctrl._parameters.Xpos;
                        sndBtnYpos = ctrl._parameters.Ypos;
                    }
                    else
                        this.panel1.Controls.Add((UserControl)selectedStrategyCtrl.Value);
                }
            if (this.panel1.MinimumSize.Height < _maxPanelHeight)
                this.panel1.Height = _maxPanelHeight;
            else
                this.panel1.Height = 120;

            if (selectedAlgostrat != null && selectedAlgostrat.CustomMessage != string.Empty)
            {
                _customMessage = selectedAlgostrat.CustomMessage;
            }
            else
            {
                _customMessage = string.Empty;
            }
            if (sndBtnXpos != 0 && sndBtnYpos != 0)
            {
                this.btnSend.Location = new Point(sndBtnXpos, sndBtnYpos + 23);
            }
            else
            {
                this.btnSend.Location = new Point((this.panel1.Size.Width - btnSend.Size.Width) - 15, (_maxPanelHeight - btnSend.Size.Height + 23) - 15);
            }
            ChangeButtonTheme();
            if (isAlgoControlPopUp == true)
            {
                this.btnSend.Text = "OK";
                this.btnSend.BackColor = Color.FromArgb(55, 67, 85);
                this.btnSend.Size = new Size(70, 23);
                this.btnSend.Location = new Point((_maxPanelWidth - btnSend.Size.Width) - 30, (_maxPanelHeight - btnSend.Size.Height + 23) - 15);
            }
            if (sendButtonVisibility)
            {
                this.panel1.Controls.Add(this.btnSend);
            }
            this.Update();
        }
        /// <summary>
        /// Change Button Theme
        /// </summary>
        private void ChangeButtonTheme()
        {
            this.btnSend.ButtonStyle = UIElementButtonStyle.Button3D;
            this.btnSend.BackColor = Color.FromArgb(104, 156, 46);
            this.btnSend.ForeColor = Color.White;
            this.btnSend.UseAppStyling = false;
            this.btnSend.UseOsThemes = DefaultableBoolean.False;
        }
        public int GetAlgoPanelHeight()
        {
            return _maxPanelHeight;
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClickedSendButtonEvent != null)
                {
                    ClickedSendButtonEvent(this, new EventArgs<string>(btnSend.Name));
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
        public DataTable GetAllowedStrategies(string cpID, string underlyingText)
        {
            return AlgoStrategyControls.AlgoControlsDictionary.GetInstance().GetAllowedStrategies(cpID, underlyingText);
        }
        public string ValidateValues()
        {
            string result = string.Empty;
            if (selectedStrategyCtrls != null)
            {
                foreach (KeyValuePair<string, AlgoStrategyUserControl> algoFixDictCtrlItem in selectedStrategyCtrls)
                {
                    result = algoFixDictCtrlItem.Value.ValidateValues();
                    if (result != string.Empty)
                    {
                        break;
                    }
                }
            }
            else
            {
                result = "Please select a strategy.";
            }
            return result;
        }
        public Dictionary<string, string> GetSelectedStrategyFixTagValues()
        {
            Dictionary<string, string> completeFixDict = new Dictionary<string, string>();
            if (selectedStrategyCtrls != null)
            {
                foreach (KeyValuePair<string, AlgoStrategyUserControl> algoFixDictCtrlItem in selectedStrategyCtrls)
                {
                    Dictionary<string, string> ctrlFixDict = algoFixDictCtrlItem.Value.GetFixValue();

                    if (ctrlFixDict != null)
                    {
                        foreach (KeyValuePair<string, string> pair in ctrlFixDict)
                        {
                            if (!string.IsNullOrEmpty(pair.Value))
                                completeFixDict.Add(pair.Key, pair.Value);
                        }
                    }
                }
                if (selectedAlgostrat != null)
                {
                    foreach (KeyValuePair<string, string> pair in selectedAlgostrat.StrategyTagValues)
                    {
                        completeFixDict.Add(pair.Key, pair.Value);
                    }
                }

            }
            return completeFixDict;
        }

        public void SetUp()
        {
            //_startPath = path;
            //AlgoStrategyControls.AlgoControlsDictionary.GetInstance(_startPath);            
        }

        public void SetSelectedStrategyFixTagValues(OrderSingle order, Dictionary<string, string> algoPropertiesDictionary, int AuecID = int.MinValue)
        {
            try
            {
                if (selectedStrategyCtrls != null)
                {
                    foreach (KeyValuePair<string, AlgoStrategyUserControl> algoFixDictCtrlItem in selectedStrategyCtrls)
                    {
                        if (algoFixDictCtrlItem.Value._parameters.ControlType.Equals("System.Windows.Forms.Button") || algoFixDictCtrlItem.Value._parameters.ControlType.Equals("System.Windows.Forms.ListView"))
                        {
                            continue;
                        }
                        string tagvalue = string.Empty;
                        if (order != null)
                        {
                            if (order.AlgoProperties.TagValueDictionary.ContainsKey(algoFixDictCtrlItem.Key))
                            {
                                tagvalue = order.AlgoProperties.TagValueDictionary[algoFixDictCtrlItem.Key];
                            }
                        }
                        else
                        {
                            if (algoPropertiesDictionary.ContainsKey(algoFixDictCtrlItem.Key))
                            {
                                tagvalue = algoPropertiesDictionary[algoFixDictCtrlItem.Key];
                            }
                        }
                        if (order != null)
                        {
                            AlgoStrategyParameters parameters = algoFixDictCtrlItem.Value.GetValue();
                            if (!(string.IsNullOrEmpty(order.AlgoStrategyID)) && order.AlgoStrategyID != null)
                                if (!AlgoControlsDictionary.GetInstance().GetAlgoStrategyDatils(order.CounterPartyID.ToString(), order.AlgoStrategyID).IsSyntheticReplace)
                                {
                                    foreach (AlgoStrategyUserControl control in this.panel1.Controls)
                                    {
                                        if (control._parameters.ControlType.Equals("System.Windows.Forms.Button") || control._parameters.ControlType.Equals("System.Windows.Forms.ListViews"))
                                        {
                                            continue;
                                        }
                                        if (control._parameters.IDs[0].Equals(algoFixDictCtrlItem.Value._parameters.IDs[0]) && control._parameters.IDs[0].Equals(parameters.IDs[0]))
                                        {
                                            if (control._parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase))
                                            {
                                                algoFixDictCtrlItem.Value.SetVisibilityIfReplaceControl(false);
                                                control.Enabled = false;
                                            }
                                            else
                                            {
                                                control.Enabled = true;
                                            }
                                            if (!parameters.SendOnReplace)
                                            {
                                                algoFixDictCtrlItem.Value.SetIfReplaceControl(true);
                                            }
                                            break;
                                        }
                                    }
                                    panel1.Refresh();

                                }
                        }
                        if (!string.IsNullOrEmpty(tagvalue))
                        {
                            OrderSingle orderAlgoDetails = new OrderSingle { AUECID = AuecID };
                            if (AuecID != int.MinValue)
                                algoFixDictCtrlItem.Value.SetFixValues(algoFixDictCtrlItem.Key, tagvalue, orderAlgoDetails);
                            else
                                algoFixDictCtrlItem.Value.SetFixValues(algoFixDictCtrlItem.Key, tagvalue, order);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public string ValidateAlgoStrategy(OrderSingle order, Dictionary<string, string> tagValueDictionary)
        {
            try
            {
                string error = string.Empty;
                if (selectedStrategyCtrls != null)
                {
                    //Dictionary<string, Dictionary<string, List<string>>> controls = DeepCopyHelper.Clone(AlgoControlsDictionary.GetInstance().ReuiredGroupNames);
                    Dictionary<string, Dictionary<string, List<string>>> controls = new Dictionary<string, Dictionary<string, List<string>>>();
                    foreach (KeyValuePair<string, AlgoStrategyUserControl> algoFixDictCtrlItem in selectedStrategyCtrls)
                    {
                        SetGroupingsOnValidation(controls, algoFixDictCtrlItem);

                        string tempError = algoFixDictCtrlItem.Value.Validate(order, tagValueDictionary);
                        if (tempError != string.Empty)
                        {
                            error += "\u2022 " + tempError + "\n";
                        }
                    }
                    Dictionary<string, string> algoPropertiesDictionary = new Dictionary<string, string>();
                    if (tagValueDictionary == null)
                    {
                        algoPropertiesDictionary = order.AlgoProperties.TagValueDictionary;
                    }
                    else
                    {
                        algoPropertiesDictionary = tagValueDictionary;
                    }
                    var strategyNames = selectedAlgostrat.AlgoparametersList;

                    string errorMessage = string.Empty;
                    if (controls.ContainsKey(selectedAlgostrat.StrategyID.ToString()))
                    {
                        var groupsWiseParameters = controls[selectedAlgostrat.StrategyID.ToString()];
                        foreach (var item in groupsWiseParameters)
                        {
                            bool isParameterHasValue = false;
                            var lstParameters = item.Value;
                            errorMessage = string.Empty;
                            foreach (var parms in lstParameters)
                            {
                                var listItems = strategyNames.Where(x => x.IDtoNameMapping.ContainsKey(parms)).Select(y => y.IDtoNameMapping[parms]).FirstOrDefault();
                                if (listItems != null)
                                    errorMessage += ", " + listItems;
                                if (algoPropertiesDictionary.ContainsKey(parms) && !string.IsNullOrEmpty(algoPropertiesDictionary[parms]))
                                {
                                    isParameterHasValue = true;
                                    break;
                                }

                            }

                            if (!isParameterHasValue)
                            {
                                errorMessage = errorMessage.Remove(0, 1);
                                error += "\u2022 Please select at-least one parameter value for " + errorMessage + ".\n";
                            }

                        }



                    }
                }
                else
                {
                    error = "Please select a strategy.";
                }
                if (!string.IsNullOrEmpty(error))
                    error = "There is some error in Algo Fields:\n\n" + error;
                return error;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        private void SetGroupingsOnValidation(Dictionary<string, Dictionary<string, List<string>>> controls, KeyValuePair<string, AlgoStrategyUserControl> algoFixDictCtrlItem)
        {
            if (controls.ContainsKey(selectedAlgostrat.StrategyID) && !string.IsNullOrEmpty(algoFixDictCtrlItem.Value._parameters.ReuiredGroupName))
            {
                if (controls[selectedAlgostrat.StrategyID].ContainsKey(algoFixDictCtrlItem.Value._parameters.ReuiredGroupName))
                {
                    controls[selectedAlgostrat.StrategyID][algoFixDictCtrlItem.Value._parameters.ReuiredGroupName].Add(algoFixDictCtrlItem.Value._parameters.IDs[0]);
                }
            }
            else if (!controls.ContainsKey(selectedAlgostrat.StrategyID) && !string.IsNullOrEmpty(algoFixDictCtrlItem.Value._parameters.ReuiredGroupName))
            {
                Dictionary<string, List<string>> groupChildParams = new Dictionary<string, List<string>>();
                groupChildParams.Add(algoFixDictCtrlItem.Value._parameters.ReuiredGroupName, new List<string> { algoFixDictCtrlItem.Value._parameters.IDs[0].ToString() });
                controls.Add(selectedAlgostrat.StrategyID, groupChildParams);
            }
        }
        public void SetAlgoDetails(OrderSingle order)
        {
            if (selectedStrategyCtrls != null)
            {
                foreach (KeyValuePair<string, AlgoStrategyUserControl> algoFixDictCtrlItem in selectedStrategyCtrls)
                {
                    algoFixDictCtrlItem.Value.SetValues(order);
                }
            }
        }

        public void EnableAlgoControlsBasedTTFields(string ordeSide, string sourceValue)
        {
            if (selectedStrategyCtrls != null)
                foreach (KeyValuePair<string, AlgoStrategyUserControl> selectedStrategyCtrl in selectedStrategyCtrls)
                {
                    AlgoStrategyUserControl ctrl = (AlgoStrategyUserControl)selectedStrategyCtrl.Value;

                    if (ctrl._parameters.ValidateWithOrderProperty.Count > 0)
                    {

                        char seperatorSemiColon = ';';
                        char seperatorComma = ',';
                        char seperatorCap = '^';
                        char seperatorAnd = '&';
                        List<string> validateWithOrder = ctrl._parameters.ValidateWithOrderProperty[0].Split(seperatorCap).ToList();

                        foreach (string fieldsConditions in validateWithOrder)
                        {
                            if (!fieldsConditions.ToLower().Contains(sourceValue.ToLower()))
                            {
                                continue;
                            }
                            string[] dataOperations = fieldsConditions.Replace("And", "&").Split(seperatorAnd);

                            List<Tuple<string, string, string>> dictFieldsCollection = new List<Tuple<string, string, string>>();
                            foreach (var dataitems in dataOperations)
                            {
                                string[] data = dataitems.Split(seperatorSemiColon);
                                dictFieldsCollection.Add(Tuple.Create(data[0], data[1], data[2]));
                            }
                            bool isallconditions = true;
                            bool isEnabled = ctrl._parameters.IsEnabled;
                            bool isVisible = true;
                            foreach (Tuple<string, string, string> item in dictFieldsCollection)
                            {
                                if (isallconditions && item.Item1.ToLower().Contains("this") && item.Item2.ToLower().Contains("enabled"))
                                {
                                    if (bool.TryParse(item.Item3, out isEnabled))
                                    {
                                        ctrl.Enabled = isEnabled;
                                    }
                                }
                                else if (isallconditions && item.Item1.ToLower().Contains("this") && item.Item2.ToLower().Contains("visible"))
                                {
                                    if (bool.TryParse(item.Item3, out isVisible))
                                    {
                                        ctrl.Visible = isVisible;
                                    }
                                }
                                else if (item.Item2.ToLower().Contains("enabled") || item.Item2.ToLower().Contains("visible"))
                                {
                                    ctrl.Enabled = isEnabled;
                                    ctrl.Visible = isVisible;
                                }

                                if (item.Item3.Split(seperatorComma).ToList().Contains(ordeSide))
                                {
                                    isallconditions = true;
                                }
                                else { isallconditions = false; }

                            }
                        }
                    }
                }

        }
    }
}

