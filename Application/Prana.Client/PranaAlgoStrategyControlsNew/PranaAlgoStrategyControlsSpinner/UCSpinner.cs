using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PranaAlgoStrategyControlsSpinner
{
    public partial class UCSpinner : AlgoStrategyUserControl
    {
        // AlgoStrategyParameters _parameters = new AlgoStrategyParameters();
        //OrderSingle _order = null;
        //private bool _isReplaceControl = false;

        public UCSpinner()
        {
            InitializeComponent();
        }

        #region AlgoStrategyUserControl Members

        public override string ValidateValues()
        {
            //throw new Exception("The method or operation is not implemented.");
            if (_parameters.IsRequired)
            {
                return ValidateSetValue();
            }
            else
                return string.Empty;
        }
        private string ValidateSetValue()
        {

            return string.Empty;


        }
        public override AlgoStrategyParameters GetValue()
        {
            //throw new Exception("The method or operation is not implemented.");
            return _parameters;
        }

        public override void SetValues(AlgoStrategyParameters parameters)
        {
            _parameters = parameters;
            this.Name = parameters.Names[0];

            if (_parameters.CustomAttributesDict.ContainsKey("LabelWidth"))
            {
                int width = 0;
                if (Int32.TryParse(_parameters.CustomAttributesDict["LabelWidth"], out width))
                {
                    lblName.Width = width;

                }
            }
            if (_parameters.CustomAttributesDict.ContainsKey("SpinnerWidth"))
            {
                int width = 0;
                if (Int32.TryParse(_parameters.CustomAttributesDict["SpinnerWidth"], out width))
                    spinner.Width = width;


            }


            // this.spinner.SetBounds(lblName.Width + 5, 7, spinner.Width, spinner.Height);
            this.spinner.Location = new System.Drawing.Point(0, lblName.Height);
            lblName.TextAlign = ContentAlignment.BottomLeft;
            this.Width = lblName.Width;
            this.Height = lblName.Height + spinner.Height;

            //this.spinner.Location = new System.Drawing.Point(lblName.Width + 5, 7);
            //this.Width = lblName.Width + spinner.Width + 10;
            // this.SetBounds(parameters.Xpos, parameters.Ypos, this.Width, this.Height);
            // this.spinner.BringToFront();




            // this.PerformLayout();
            spinner.Name = _parameters.IDs[0];
            lblName.Text = _parameters.Names[0] + (_parameters.IsRequired ? "*" : string.Empty);

            string[] strcontrolvalues = _parameters.InnerControlValues.ToArray();

            spinner.MinValue = double.Parse(strcontrolvalues[1]);
            spinner.MaxValue = double.Parse(strcontrolvalues[2]);
            spinner.Increment = double.Parse(strcontrolvalues[3]);

            spinner.IsCustomValidation = true;
            if (Array.Exists(strcontrolvalues, element => element.ToLower() == "true"))
                spinner.IsBlankAllowed = Boolean.Parse(strcontrolvalues[4]);
            if ((strcontrolvalues[0].Trim()).Equals("") && spinner.IsBlankAllowed)
            {
                spinner.IsBlankValue = true;
                spinner.Value = 0;
            }
            else
            {
                spinner.Value = double.Parse(strcontrolvalues[0]);
            }
            if (_parameters.ControlCategory == "Parent")
            {
                this.spinner.ValueChanged += spinner_ValueChanged;
            }

        }


        public override Dictionary<string, string> GetFixValue()
        {
            Dictionary<string, string> idValuePair = new Dictionary<string, string>();

            if (_isReplaceControl && !_parameters.SendOnReplace)
            {
                _parameters.IsRequired = false;
                return idValuePair;
            }
            else if ((this.Enabled || this.Visible) && lblName.Text.Contains("*"))
            {
                _parameters.IsRequired = true;
            }
            else if ((this.Enabled || this.Visible) && lblName.Text.Contains("*"))
            {
                _parameters.IsRequired = true;
            }
            else if ((this.Enabled || this.Visible) && lblName.Text.Contains("*"))
            {
                _parameters.IsRequired = true;
            }
            if (!this.Enabled || !this.Visible)
            {
                _parameters.IsRequired = false;
                if (_parameters.SendOnReplace && !_isReplaceControl && !_isVisibleReplaceControl)
                { }
                else
                    return idValuePair;
            }


            if (!_parameters.IsRequired)
            {
                string val = spinner.Value.ToString();
                if (val.Equals("0") && spinner.IsBlankValue && spinner.IsBlankAllowed)
                {
                    idValuePair.Add(spinner.Name, "");
                }
                else if (_parameters.DefaultValue != string.Empty)
                {
                    idValuePair.Add(this.spinner.Name, spinner.Value.ToString());
                    return idValuePair;
                }
                else
                    return idValuePair;
                //}
            }
            else
            {
                string val = spinner.Value.ToString();
                if (val.Equals("0") && spinner.IsBlankValue && spinner.IsBlankAllowed)
                {
                    idValuePair.Add(spinner.Name, "");
                }
                else
                {
                    idValuePair.Add(spinner.Name, spinner.Value.ToString());
                }

            }

            return idValuePair;


        }

        public override void SetUserControls(string type)
        {

        }
        public override void SetFixValues(string tag, string value, OrderSingle order)
        {
            if (this.spinner.Name == tag)
            {
                if (Array.Exists(_parameters.InnerControlValues.ToArray(), element => element.ToLower() == "true"))
                    this.spinner.IsBlankAllowed = Boolean.Parse(_parameters.InnerControlValues.ToArray()[4]);
                if ((value.Trim()).Equals("") && spinner.IsBlankAllowed)
                {
                    this.spinner.IsBlankValue = true;
                    this.spinner.Value = 0;
                }
                else
                {
                    this.spinner.IsBlankValue = false;
                    this.spinner.Value = double.Parse(value);
                }
            }
        }

        public override AlgoStrategyUserControl GetUserCtrl(string underlyingText)
        {
            UCSpinner ucSpinner = new UCSpinner();
            AlgoStrategyParameters algoStrategyParameters = GetClonedAlgoStrategyParameters(underlyingText);
            ucSpinner.SetValues(algoStrategyParameters);
            ucSpinner.Top = _parameters.Ypos;
            ucSpinner.Left = _parameters.Xpos;
            ucSpinner.Enabled = algoStrategyParameters.IsEnabled;
            return ucSpinner;
        }

        private void spinner_ValueChanged(object sender, EventArgs e)
        {
            string parameterName = _parameters.Names[0];
            UpdateDependentParamenters(sender.ToString(), parameterName);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedItem"></param>
        /// <param name="parameterName"></param>
        private void UpdateDependentParamenters(string selectedItem, string parameterName)
        {
            try
            {
                Panel parentPanel = this.Parent as Panel;
                if (parentPanel != null)
                    foreach (var control in parentPanel.Controls)
                    {
                        if (!(control is Infragistics.Win.Misc.UltraButton))
                        {
                            AlgoStrategyUserControl algoStrategy = (AlgoStrategyUserControl)(control);

                            if (!(string.IsNullOrEmpty(selectedItem) || (selectedItem.Equals("0")) || (selectedItem.Equals("1"))))
                            {
                                var genericValue = algoStrategy._parameters._algoStrategyParametersToUpdate.Select(x => x.Key).ToList();
                                if (genericValue != null)
                                {
                                    foreach (var item in genericValue)
                                    {
                                        if (item.Equals("") || item.Equals("1") || item.Equals("0"))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            selectedItem = item;
                                        }
                                    }
                                }

                            }

                            if (algoStrategy._parameters.ControlCategory == "Parent" && algoStrategy._parameters.Names[0].Equals(parameterName))
                            {

                                if (algoStrategy._parameters._algoStrategyParametersToUpdate.ContainsKey(selectedItem))
                                {
                                    foreach (var item in algoStrategy._parameters._algoStrategyParametersToUpdate[selectedItem])
                                    {
                                        foreach (var ctrl in parentPanel.Controls)
                                        {
                                            if (!(ctrl is Infragistics.Win.Misc.UltraButton))
                                            {
                                                AlgoStrategyUserControl alsoStrategyCtrl = (AlgoStrategyUserControl)(ctrl);
                                                if (alsoStrategyCtrl._parameters.IDs[0].Equals(item.Key))
                                                {
                                                    alsoStrategyCtrl.UpdateParameterValues(alsoStrategyCtrl, algoStrategy._parameters._algoStrategyParametersToUpdate[selectedItem][item.Key]);
                                                }
                                            }
                                        }
                                    }
                                }
                                parentPanel.Refresh();
                            }
                        }
                    }
                parentPanel.Update();
            }
            catch (Exception)
            {
                //bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                //if (rethrow)
                //{
                throw;
                //}
            }
        }

        public override AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl aslsoStrategy, AlgoStrategyParametersToUpdate parameterToUpdate)
        {
            UCSpinner ucSpinner = (UCSpinner)aslsoStrategy;
            if (parameterToUpdate != null)
            {

                ucSpinner.spinner.Visible = Convert.ToBoolean(parameterToUpdate.Visibility);
                ucSpinner.lblName.Visible = Convert.ToBoolean(parameterToUpdate.Visibility);
                ucSpinner.Enabled = Convert.ToBoolean(parameterToUpdate.Enabled);
                ucSpinner._parameters.IsRequired = Convert.ToBoolean(parameterToUpdate.Required);
                ucSpinner._parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                _parameters.IsRequired = Convert.ToBoolean(parameterToUpdate.Required);
                _parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                if (parameterToUpdate.MaxValue != null && parameterToUpdate.MinValue != null)
                {
                    ucSpinner.spinner.MinValue = 0;
                    ucSpinner.spinner.MaxValue = 0;
                    ucSpinner.spinner.MaxValue = Convert.ToDouble(parameterToUpdate.MaxValue);
                    ucSpinner.spinner.MinValue = Convert.ToDouble(parameterToUpdate.MinValue);
                }

                if (parameterToUpdate.Default != null)
                    ucSpinner.spinner.Value = Convert.ToDouble(parameterToUpdate.Default);
                if (parameterToUpdate.Increment != null)
                    ucSpinner.spinner.Increment = Convert.ToDouble(parameterToUpdate.Increment);
                if (parameterToUpdate.ValidateWithOrderProperty != null)
                    _parameters.ValidateWithOrderProperty = parameterToUpdate.ValidateWithOrderProperty;
                ucSpinner.lblName.Text = ucSpinner.lblName.Text.Replace("*", "") + (ucSpinner._parameters.IsRequired ? "*" : string.Empty);
                if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                {
                    ucSpinner.Enabled = _isVisibleReplaceControl;
                }
                ucSpinner.spinner.Update();
            }
            return ucSpinner;
        }

        /// <summary>
        /// Validate 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override string Validate(OrderSingle order, Dictionary<string, string> tagValueDictionary)
        {
            try
            {
                string error = base.Validate(order, tagValueDictionary);
                if (!string.IsNullOrEmpty(error))
                    return error;


                error = string.Empty;
                Dictionary<string, string> algoPropertiesDictionary = new Dictionary<string, string>();
                if (tagValueDictionary == null)
                {
                    algoPropertiesDictionary = order.AlgoProperties.TagValueDictionary;
                }
                else
                {
                    algoPropertiesDictionary = tagValueDictionary;
                }

                if (_parameters.ValidateWith.Count > 0)
                {


                    for (int incrementVal = 0; incrementVal < _parameters.ValidateWith.Count; incrementVal++)
                    {
                        string val = spinner.Value.ToString();
                        Tuple<string, string, string> rowValues;
                        if (val.Equals("0") && spinner.IsBlankValue && spinner.IsBlankAllowed)
                        {
                            incrementVal = incrementVal + 2;
                            continue;
                        }
                        string itemErrorMessge = "";
                        if (_parameters.ValidateWith.Count == 1 && algoPropertiesDictionary.ContainsKey(_parameters.ValidateWith[0]))
                        {
                            itemErrorMessge = _parameters.Names[incrementVal] + " should be greater than Min field.\n";
                            rowValues = new Tuple<string, string, string>(_parameters.ValidateWith[incrementVal], itemErrorMessge, ">=");
                        }
                        else if (((_parameters.ValidateWith.Count) == 2 || _parameters.ValidateWith.Count == 4) && algoPropertiesDictionary.ContainsKey(_parameters.ValidateWith[incrementVal]))
                        {
                            itemErrorMessge = _parameters.ValidateWith[incrementVal + 1];
                            rowValues = new Tuple<string, string, string>(_parameters.ValidateWith[incrementVal], itemErrorMessge, ">=");
                            incrementVal += 1;
                        }
                        else if ((_parameters.ValidateWith.Count) >= 3 && algoPropertiesDictionary.ContainsKey(_parameters.ValidateWith[incrementVal]))
                        {
                            rowValues = new Tuple<string, string, string>(_parameters.ValidateWith[incrementVal], _parameters.ValidateWith[incrementVal + 1], _parameters.ValidateWith[incrementVal + 2]);
                            incrementVal += 2;
                        }
                        else
                        {
                            rowValues = null;
                        }

                        double maxParamValue = this.spinner.Value;
                        double minParamvalue = 0;
                        if (rowValues != null)
                            if (algoPropertiesDictionary.ContainsKey(rowValues.Item1) && double.TryParse(algoPropertiesDictionary[rowValues.Item1], out minParamvalue))
                            {

                                if (!(
                                    ((maxParamValue >= minParamvalue) && (rowValues.Item3.Equals(">=") || rowValues.Item3.Equals("&gt;="))) ||
                                    ((maxParamValue > minParamvalue) && (rowValues.Item3.Equals(">") || rowValues.Item3.Equals("&gt;"))) ||
                                    ((maxParamValue == minParamvalue) && (rowValues.Item3.Equals("=")))))
                                {

                                    if (!string.IsNullOrEmpty(error))
                                        error += "\u2022 " + rowValues.Item2 + ".\n";
                                    else
                                        error = rowValues.Item2 + ".\n";
                                }


                            }
                    }
                }
                else
                {
                    error = string.Empty;
                }

                if (_parameters.ValidateWithOrderProperty.Count > 0 && order != null)
                {
                    char seperatorSemiColon = ';';
                    char seperatorComma = ',';
                    char seperatorCap = '^';
                    char seperatorAnd = '&';
                    double alogFieldValue = this.spinner.Value;
                    List<string> validateWithOrder = _parameters.ValidateWithOrderProperty[0].Split(seperatorCap).ToList();

                    foreach (string fieldsConditions in validateWithOrder)
                    {
                        string[] dataOperations = fieldsConditions.Replace("And", "&").Split(seperatorAnd);

                        List<Tuple<string, string, string>> dictFieldsCollection = new List<Tuple<string, string, string>>();
                        foreach (var dataitems in dataOperations)
                        {
                            string[] data = dataitems.Split(seperatorSemiColon);
                            dictFieldsCollection.Add(Tuple.Create(data[0], data[1], data[2]));
                        }
                        foreach (Tuple<string, string, string> item in dictFieldsCollection)
                        {

                            var allOrderFields = order.GetType().BaseType.GetFields(BindingFlags.Public
                                                                                                     | BindingFlags.Instance
                                                                                                     | BindingFlags.NonPublic
                                                                                                     | BindingFlags.Static);
                            var orderField1 = allOrderFields.SingleOrDefault(a => a.Name.ToLower().Equals("_" + item.Item1.ToLower()));
                            if (orderField1 == null)
                            {
                                var allOrderFieldsMain = order.GetType().GetFields(BindingFlags.Public
                                                                                                     | BindingFlags.Instance
                                                                                                     | BindingFlags.NonPublic
                                                                                                     | BindingFlags.Static);
                                orderField1 = allOrderFieldsMain.SingleOrDefault(a => a.Name.ToLower().Equals("_" + item.Item1.ToLower()));
                            }
                            var orderField2 = allOrderFields.SingleOrDefault(a => a.Name.ToLower().Equals("_" + item.Item3.ToLower()));
                            if (orderField2 == null)
                            {
                                var allOrderFieldsMain2 = order.GetType().GetFields(BindingFlags.Public
                                                                                                        | BindingFlags.Instance
                                                                                                        | BindingFlags.NonPublic
                                                                                                        | BindingFlags.Static);
                                orderField2 = allOrderFieldsMain2.SingleOrDefault(a => a.Name.ToLower().Equals("_" + item.Item3.ToLower()));
                            }
                            double _OrderField1DoubleValue = 0;
                            if (orderField1 != null && item.Item3.ToLower().Contains("this") && (item.Item2.ToLower().Contains(">") || item.Item2.ToLower().Contains("<") || item.Item2.ToLower().Contains("=")) && Double.TryParse((orderField1.GetValue(order)).ToString(), out _OrderField1DoubleValue))
                            {
                                error += ValidateOperatorWiseTTAlgoFields(alogFieldValue, item, _OrderField1DoubleValue);
                            }
                            else if (orderField2 == null && !item.Item3.ToLower().Equals("all") && orderField1 != null && Double.TryParse((orderField1.GetValue(order)).ToString(), out _OrderField1DoubleValue))
                            {
                                if (!(item.Item3.Split(seperatorComma).ToList().Contains(Convert.ToInt16(_OrderField1DoubleValue).ToString())))
                                {
                                    if ((item.Item1.ToLower().Equals("ordersidetagvalue") || item.Item1.ToLower().Equals("ordertypetagvalue")) && dictFieldsCollection.Count > 1)
                                    {
                                        break;
                                    }
                                    error += "\u2022 " + "Please select valid " + Regex.Replace(item.Item1, "(\\B[A-Z])", " $1") + ". \n";
                                }

                            }
                            else if (orderField1 != null && orderField2 != null && !item.Item3.ToLower().Contains("this"))
                            {
                                double _OrderField2DoubleValue = 0;
                                if (Double.TryParse((orderField2.GetValue(order)).ToString(), out _OrderField2DoubleValue))
                                {
                                    alogFieldValue = _OrderField1DoubleValue;
                                    _OrderField1DoubleValue = _OrderField2DoubleValue;
                                    error += ValidateOperatorWiseTTAlgoFields(alogFieldValue, item, _OrderField1DoubleValue);
                                }
                            }
                            //else if (item.Item1.ToLower().Contains("this") && item.Item2.ToLower().Contains("enabled"))
                            //{
                            //    bool iBoolVal = true;
                            //    if (bool.TryParse(item.Item3, out iBoolVal))
                            //    {
                            //        this.Enabled = iBoolVal;
                            //    }
                            //}
                        }
                    }
                }
                if (error != string.Empty && error.StartsWith("\u2022"))
                    return error.Remove(0, 1);
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

        private string ValidateOperatorWiseTTAlgoFields(double alogFieldValue, Tuple<string, string, string> item, double _OrderField1DoubleValue)
        {
            string error = string.Empty;
            string fieldName = item.Item1;
            if (item.Item1.ToLower().Contains("tradeattribute"))
            {
                fieldName = "Avg Price";
            }
            switch (item.Item2)
            {
                case ">":
                    if (alogFieldValue > _OrderField1DoubleValue)
                    {
                        error += "\u2022 " + "Algo field " + this.Name + " should not be greater than Order " + fieldName + " field. ";
                    }
                    break;
                case ">=":
                    if (alogFieldValue >= _OrderField1DoubleValue)
                    {
                        error += "\u2022 " + "Algo field " + this.Name + " should not be greater than equal to Order " + fieldName + " field.";
                    }
                    break;
                case "<":
                    if (alogFieldValue < _OrderField1DoubleValue)
                    {
                        error += "\u2022 " + "Algo field " + this.Name + " should not be less than to Order " + fieldName + " field.";
                    }
                    break;
                case "<=":
                    if (alogFieldValue <= _OrderField1DoubleValue)
                    {
                        error += "\u2022 " + "Algo field " + this.Name + " should not be less than equal to Order " + fieldName + " field.";
                    }
                    break;
                case "=":
                case "==":
                    if (!(alogFieldValue == _OrderField1DoubleValue))
                    {
                        error += "\u2022 " + "Algo field " + this.Name + " should be equal to Order " + fieldName + " field. ";
                    }
                    break;
            }
            return error;
        }

        public override void SetValues(OrderSingle order)
        {
        }
        #endregion

        #region AlgoStrategyUserControl Members

        private bool _isReplaceControl = false;
        public override void SetIfReplaceControl(bool isReplaceControl)
        {
            _isReplaceControl = isReplaceControl;
        }

        private bool _isVisibleReplaceControl = true;
        public override void SetVisibilityIfReplaceControl(bool isReplaceControl)
        {
            _isVisibleReplaceControl = isReplaceControl;
        }

        #endregion
    }
}
