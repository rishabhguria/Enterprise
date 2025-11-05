using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;


namespace Prana.AlgoStrategyControls
{
    public partial class GenericControl : AlgoStrategyUserControl
    {
        //string[] strArray = null;
        string[] strcontrolnames = null;
        string[] strcontrolvalues = null;
        // string strcheckBoxvalue = null;
        Dictionary<string, string> ControlValueMapping = new Dictionary<string, string>();

        Dictionary<string, string> ControlNameValueMapping = new Dictionary<string, string>();

        // AlgoStrategyParameters _parameters = new AlgoStrategyParameters();        
        public GenericControl()
        {
            InitializeComponent();
        }

        #region AlgoStrategyUserControl Members

        public override string ValidateValues()
        {
            return string.Empty;
        }

        public override AlgoStrategyParameters GetValue()
        {
            //throw new Exception("The method or operation is not implemented.");
            return _parameters;
        }
        ToolTip toolTip = new ToolTip() { AutoPopDelay = 0, InitialDelay = 0, ReshowDelay = 0, ShowAlways = true, };

        public override void SetValues(AlgoStrategyParameters algoStrategyParameters)
        {
            _parameters = algoStrategyParameters;

            strcontrolnames = algoStrategyParameters.InnerControlNames.ToArray();
            strcontrolvalues = algoStrategyParameters.InnerControlValues.ToArray();


            for (int i = 0; i < strcontrolnames.Length; i++)
            {
                ControlValueMapping.Add(strcontrolnames[i], strcontrolvalues[i]);
            }


            for (int i = 0; i < strcontrolnames.Length; i++)
            {
                ControlNameValueMapping.Add(strcontrolvalues[i], strcontrolnames[i]);
            }

            switch (algoStrategyParameters.ControlType)
            {

                case "System.Windows.Forms.TextBox":
                    TextBox textbx = new TextBox();
                    Label lbl = new Label();
                    if (_parameters.CustomAttributesDict.ContainsKey("LabelWidth"))
                    {
                        int width = 0;
                        if (Int32.TryParse(_parameters.CustomAttributesDict["LabelWidth"], out width))
                            lbl.Width = width;
                    }
                    if (_parameters.CustomAttributesDict.ContainsKey("TextBoxWidth"))
                    {
                        int width = 0;
                        if (Int32.TryParse(_parameters.CustomAttributesDict["TextBoxWidth"], out width))
                            textbx.Width = width;
                    }
                    lbl.Text = _parameters.Names[0] + (_parameters.IsRequired ? "*" : string.Empty);
                    lbl.ForeColor = Color.Black;

                    lbl.SetBounds(0, 0, lbl.Width, 15);
                    textbx.SetBounds(0, 15, textbx.Width, 20);
                    // lbl.SetBounds(0, 3, lbl.Width, lbl.Height);
                    // textbx.SetBounds(lbl.Width, 0, textbx.Width, textbx.Height);
                    textbx.ForeColor = Color.Black;
                    textbx.Text = _parameters.DefaultValue;
                    this.Width = lbl.Width > textbx.Width ? lbl.Width : textbx.Width;
                    this.Controls.Add(lbl);
                    this.Controls.Add(textbx);
                    break;
                case "System.Windows.Forms.Panel":
                    Panel panel1 = new Panel();
                    panel1.Location = new Point(0, 0);
                    int panelWidth = 0;
                    int panelHeight = 0;
                    if (_parameters.CustomAttributesDict.ContainsKey("PanelWidth"))
                    {

                        Int32.TryParse(_parameters.CustomAttributesDict["PanelWidth"], out panelWidth);

                    }
                    if (_parameters.CustomAttributesDict.ContainsKey("PanelHeight"))
                    {

                        Int32.TryParse(_parameters.CustomAttributesDict["PanelHeight"], out panelHeight);

                    }
                    panel1.Size = new Size(panelWidth, panelHeight);

                    panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    this.Controls.Add(panel1);
                    break;
                case "System.Windows.Forms.ListView":
                    ListView listView1 = new ListView();
                    listView1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
                    listView1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
                    listView1.UseCompatibleStateImageBehavior = false;
                    int lineWidthX1Pos = 0;
                    int lineWidthY1Pos = 0;
                    int lineXpos = 0;
                    int lineYPos = 0;

                    if (_parameters.CustomAttributesDict.ContainsKey("Width"))
                    {
                        Int32.TryParse(_parameters.CustomAttributesDict["Width"], out lineWidthX1Pos);
                    }
                    if (_parameters.CustomAttributesDict.ContainsKey("Height"))
                    {
                        Int32.TryParse(_parameters.CustomAttributesDict["Height"], out lineWidthY1Pos);
                    }
                    if (_parameters.CustomAttributesDict.ContainsKey("Xpos"))
                    {
                        Int32.TryParse(_parameters.CustomAttributesDict["Xpos"], out lineXpos);
                    }
                    if (_parameters.CustomAttributesDict.ContainsKey("Ypos"))
                    {
                        Int32.TryParse(_parameters.CustomAttributesDict["Ypos"], out lineYPos);
                    }

                    listView1.MinimumSize = new System.Drawing.Size(1, 1);
                    listView1.Location = new System.Drawing.Point(lineXpos, lineYPos);
                    listView1.Size = new System.Drawing.Size(lineWidthX1Pos, lineWidthY1Pos);
                    foreach (ColumnHeader column in listView1.Columns)
                    {
                        column.Width = -2;
                    }

                    this.Controls.Add(listView1);
                    break;
                case "System.Windows.Forms.Button":
                    Button btn = new Button();
                    btn.Location = new System.Drawing.Point(_parameters.Xpos, _parameters.Ypos);
                    btn.Size = new System.Drawing.Size(119, 29);
                    this.Controls.Add(btn);
                    break;
                case "System.Windows.Forms.NumericUpDown":
                    NumericUpDown numupdown = new NumericUpDown();

                    Label lbl1 = new Label();
                    lbl1.Text = _parameters.Names[0] + (_parameters.IsRequired ? "*" : string.Empty);
                    lbl1.ForeColor = Color.Black;
                    lbl1.SetBounds(0, 3, 110, 23);
                    numupdown.SetBounds(lbl1.Width, 0, 100, numupdown.Height);
                    this.Controls.Add(lbl1);
                    this.Controls.Add(numupdown);
                    this.Width = lbl1.Width + numupdown.Width;
                    this.Height = numupdown.Height;
                    decimal temp = 0;
                    Decimal.TryParse(_parameters.DefaultValue, out temp);

                    numupdown.Value = temp;
                    break;

                case "System.Windows.Forms.RadioButton":
                    for (int j = 0; j < strcontrolnames.Length; j++)
                    {
                        RadioButton radiobtn = new RadioButton();
                        radiobtn.Text = strcontrolnames[j];
                        radiobtn.Top = j * 20;
                        radiobtn.Name = strcontrolnames[j];
                        radiobtn.ForeColor = Color.Black;
                        this.Controls.Add(radiobtn);
                    }
                    break;

                case "System.Windows.Forms.CheckBox":
                    CheckBox checkbx = new CheckBox();
                    checkbx.Text = algoStrategyParameters.Names[0] + (_parameters.IsRequired ? "*" : string.Empty);
                    checkbx.AutoSize = true;
                    checkbx.ForeColor = Color.Black;
                    this.Controls.Add(checkbx);
                    this.Width = checkbx.Width;
                    this.Height = checkbx.Height;
                    bool tempchecked = false;

                    bool.TryParse(_parameters.DefaultValue, out tempchecked);

                    if (_parameters.ControlCategory == "Parent")
                    {
                        checkbx.CheckedChanged += new System.EventHandler(CheckedChanged_SelectedValueChanged);
                        checkbx.Name = _parameters.Names[0];
                    }
                    checkbx.Checked = tempchecked;

                    break;
                case "System.Windows.Forms.ComboBox":
                    if (algoStrategyParameters.InnerControlValues != null)
                        strcontrolvalues = algoStrategyParameters.InnerControlValues.ToArray();
                    if (algoStrategyParameters.InnerControlNames != null)
                        strcontrolnames = algoStrategyParameters.InnerControlNames.ToArray();
                    ComboBox combobx = new ComboBox();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Name");
                    dt.Columns.Add("Value");
                    //object[] rowObj2 = new object[2];
                    //rowObj2[0] = "-Select-";
                    //rowObj2[1] = int.MinValue;
                    //dt.Rows.Add(rowObj2);


                    int i = 0;
                    int defaultSelectIndex = 0;
                    foreach (string value in strcontrolvalues)
                    {

                        object[] rowObj = new object[2];
                        rowObj[0] = strcontrolnames[i];
                        rowObj[1] = value;
                        dt.Rows.Add(rowObj);

                        if (value == _parameters.DefaultValue)
                        {
                            defaultSelectIndex = i;
                        }
                        i++;

                    }
                    combobx.DataSource = dt;
                    combobx.DisplayMember = "Name";
                    combobx.ValueMember = "Value";

                    combobx.Text = ApplicationConstants.C_COMBO_SELECT;

                    Label lbl2 = new Label();
                    if (_parameters.CustomAttributesDict.ContainsKey("LabelWidth"))
                    {
                        int width = 0;
                        if (Int32.TryParse(_parameters.CustomAttributesDict["LabelWidth"], out width))
                            lbl2.Width = width;
                    }
                    if (_parameters.CustomAttributesDict.ContainsKey("ComboBoxWidth"))
                    {
                        int width = 0;
                        if (Int32.TryParse(_parameters.CustomAttributesDict["ComboBoxWidth"], out width))
                            combobx.Width = width;
                    }
                    lbl2.Text = _parameters.Names[0] + (_parameters.IsRequired ? "*" : string.Empty);
                    combobx.Name = "combo_" + lbl2.Text;
                    lbl2.SetBounds(0, 0, lbl2.Width, lbl2.Height);
                    combobx.SetBounds(0, lbl2.Height, combobx.Width, 23);
                    this.Size = new System.Drawing.Size(combobx.Width, 46);
                    if (_parameters.ControlCategory == "Parent")
                    {
                        combobx.SelectedValueChanged += new System.EventHandler(ComboBox_SelectedValueChanged);
                        combobx.Name = _parameters.Names[0];
                    }
                    combobx.DrawMode = DrawMode.OwnerDrawFixed;
                    combobx.DrawItem += combobx_DrawItem;
                    combobx.DropDownClosed += combobx_DropDownClosed;
                    lbl2.TextAlign = ContentAlignment.BottomLeft;
                    combobx.ForeColor = Color.Black;
                    lbl2.ForeColor = Color.Black;

                    combobx.BindingContext = this.BindingContext;
                    if (_parameters.DefaultValue != null)
                        combobx.SelectedIndex = defaultSelectIndex;

                    this.Controls.Add(lbl2);
                    this.Controls.Add(combobx);

                    break;
            }
        }

        void combobx_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            e.DrawBackground();
            string text = comboBox.GetItemText(comboBox.Items[e.Index]);
            using (SolidBrush br = new SolidBrush(e.ForeColor))
                e.Graphics.DrawString(text, e.Font, br, e.Bounds);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected && comboBox.DroppedDown)
                toolTip.Show(text, comboBox, e.Bounds.Right, e.Bounds.Bottom + 4);
            e.DrawFocusRectangle();
        }

        void combobx_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            toolTip.Hide(comboBox);
        }

        private void CheckedChanged_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {

                CheckBox chkBox = (CheckBox)sender;

                bool isCheck = chkBox.Checked;
                string parameterName = (string)chkBox.Name;
                string selectedItem = "True";
                if (!isCheck)
                {
                    selectedItem = "False";
                }
                UpdateDependentParamenters(selectedItem, parameterName);
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

        void ComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox comboBox = (ComboBox)sender;
                string selectedItem = (string)comboBox.SelectedValue;
                string parameterName = (string)comboBox.Name;
                if (selectedItem == null)
                    selectedItem = "";
                UpdateDependentParamenters(selectedItem, parameterName);
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

        private void UpdateDependentParamenters(string selectedItem, string parameterName)
        {
            try
            {
                Panel parentPanel = new Panel();
                if (this.Parent != null)
                {
                    parentPanel = (this.Parent as Panel);
                    if (parentPanel != null)
                        foreach (var control in parentPanel.Controls)
                        {
                            if (!(control is Infragistics.Win.Misc.UltraButton))
                            {
                                AlgoStrategyUserControl aslsoStrategy = (AlgoStrategyUserControl)(control);
                                if (aslsoStrategy._parameters.ControlCategory == "Parent" && aslsoStrategy._parameters.Names[0].Equals(parameterName))
                                {

                                    if (aslsoStrategy._parameters._algoStrategyParametersToUpdate.ContainsKey(selectedItem))
                                    {
                                        foreach (var item in aslsoStrategy._parameters._algoStrategyParametersToUpdate[selectedItem])
                                        {
                                            foreach (var ctrl in parentPanel.Controls)
                                            {
                                                if (!(ctrl is Infragistics.Win.Misc.UltraButton))
                                                {
                                                    AlgoStrategyUserControl alsoStrategyCtrl = (AlgoStrategyUserControl)(ctrl);
                                                    if (alsoStrategyCtrl._parameters.IDs[0].Equals(item.Key))
                                                    {
                                                        alsoStrategyCtrl.UpdateParameterValues(alsoStrategyCtrl, aslsoStrategy._parameters._algoStrategyParametersToUpdate[selectedItem][item.Key]);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    parentPanel.Refresh();
                                }
                            }
                        }
                }
                parentPanel.Update();
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
        /// Get Fix Value
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> GetFixValue()
        {

            Dictionary<string, string> idValuePair = new Dictionary<string, string>();

            if (_isReplaceControl && !_parameters.SendOnReplace)
            {
                _parameters.IsRequired = false;
                return idValuePair;
            }
            if (!this.Enabled || !this.Visible)
            {
                _parameters.IsRequired = false;
                if (_parameters.SendOnReplace && !_isReplaceControl && !_isVisibleReplaceControl)
                { }
                else
                    return idValuePair;
            }



            //throw new Exception("The method or operation is not implemented.");
            foreach (Control varControl in this.Controls)
            {
                switch (varControl.GetType().Name)
                {
                    case "CheckBox":
                        string key = (varControl as CheckBox).Checked.ToString();
                        if (ControlValueMapping.ContainsKey(key))
                        {
                            idValuePair.Add(_parameters.IDs[0], ControlValueMapping[key]);
                        }
                        //Handling for lower case true-false
                        else if (ControlValueMapping.ContainsKey(key.ToLower()))
                        {
                            idValuePair.Add(_parameters.IDs[0], ControlValueMapping[key.ToLower()]);
                        }
                        else
                        {
                            idValuePair.Add(_parameters.IDs[0], key);
                        }

                        break;
                    case "RadioButton":
                        if ((varControl as RadioButton).Checked == true)
                        {
                            idValuePair.Add(_parameters.IDs[0], ControlValueMapping[varControl.Name]);
                        }
                        break;
                    case "NumericUpDown":
                        idValuePair.Add(_parameters.IDs[0], (varControl as NumericUpDown).Value.ToString());
                        break;
                    case "TextBox":
                        idValuePair.Add(_parameters.IDs[0], (varControl as TextBox).Text);
                        break;
                    case "ComboBox":
                        if ((varControl as ComboBox).SelectedItem != null && !(varControl as ComboBox).SelectedValue.ToString().Equals(int.MinValue.ToString()))
                            idValuePair.Add(_parameters.IDs[0], (varControl as ComboBox).SelectedValue.ToString());
                        else
                            idValuePair.Add(_parameters.IDs[0], string.Empty);
                        break;
                }
            }
            //Dictionary<string, string> idValuePair = new Dictionary<string, string>();
            //idValuePair.Add(tempID, this.ultraDateTimeEditor1.Value.ToString());
            return idValuePair;
        }
        public override void SetUserControls(string type)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public override AlgoStrategyUserControl GetUserCtrl(string underlyingText)
        {
            GenericControl genericControl = new GenericControl();

            AlgoStrategyParameters algoStrategyParameters = GetClonedAlgoStrategyParameters(underlyingText);
            genericControl.SetValues(algoStrategyParameters);
            genericControl.Top = _parameters.Ypos;
            genericControl.Left = _parameters.Xpos;
            genericControl.Enabled = algoStrategyParameters.IsEnabled;
            return genericControl;
        }

        public override AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl alsoStart, AlgoStrategyParametersToUpdate parameterToUpdate)
        {
            GenericControl genericControl = (GenericControl)alsoStart;
            genericControl.Enabled = parameterToUpdate.Enabled;
            genericControl.Visible = parameterToUpdate.Visibility;

            alsoStart._parameters.IsRequired = parameterToUpdate.Required;
            genericControl._parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
            _parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
            foreach (var ctrl in genericControl.Controls)
            {

                if (ctrl is ComboBox)
                {
                    ComboBox cmb = (ComboBox)ctrl;
                    if (parameterToUpdate.SelectedValue != null)
                    {
                        cmb.SelectedValue = parameterToUpdate.SelectedValue;
                    }

                    cmb.Visible = parameterToUpdate.Visibility;
                    cmb.Enabled = parameterToUpdate.Enabled;
                    if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                    {
                        cmb.Enabled = _isVisibleReplaceControl;
                    }
                }
                else if (ctrl is CheckBox)
                {
                    CheckBox chkBox = (CheckBox)ctrl;
                    chkBox.Checked = parameterToUpdate.Checked;
                    chkBox.Visible = parameterToUpdate.Visibility;
                    chkBox.Enabled = parameterToUpdate.Enabled;
                    if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                    {
                        chkBox.Enabled = _isVisibleReplaceControl;
                    }

                }
                else if (ctrl is Label)
                {
                    Label label = (Label)ctrl;
                    label.Visible = parameterToUpdate.Visibility;
                    label.Enabled = parameterToUpdate.Enabled;
                    label.Text = label.Text.Replace("*", "") + (parameterToUpdate.Required ? "*" : string.Empty);
                    if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                    {
                        label.Enabled = _isVisibleReplaceControl;
                    }
                }
                else if (ctrl is ListView)
                {
                    ListView lstView = (ListView)ctrl;
                    lstView.Visible = parameterToUpdate.Visibility;
                    lstView.Enabled = parameterToUpdate.Enabled;
                    if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                    {
                        lstView.Enabled = _isVisibleReplaceControl;
                    }

                }
                else if (ctrl is RadioButton)
                {
                    RadioButton radioButton = (RadioButton)ctrl;
                    radioButton.Checked = parameterToUpdate.Checked;
                    radioButton.Visible = parameterToUpdate.Visibility;
                    radioButton.Enabled = parameterToUpdate.Enabled;
                    if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                    {
                        radioButton.Enabled = _isVisibleReplaceControl;
                    }

                }

            }

            return genericControl;
        }



        #endregion

        #region AlgoStrategyUserControl Members


        public override void SetFixValues(string tag, string value, OrderSingle order)
        {
            foreach (Control varControl in this.Controls)
            {
                switch (varControl.GetType().Name)
                {
                    case "CheckBox":
                        //string key = (varControl as CheckBox).Checked.ToString();
                        string checkvalue = string.Empty;
                        if (ControlNameValueMapping.ContainsKey(value))
                            checkvalue = ControlNameValueMapping[value];
                        if ((checkvalue == "True") || (checkvalue == "true"))
                        {
                            (varControl as CheckBox).Checked = true;
                        }
                        else
                        {
                            (varControl as CheckBox).Checked = false;
                        }

                        break;
                    case "RadioButton":

                        if ((varControl as RadioButton).Checked == true)
                        {
                            //idValuePair.Add(_parameters.IDs[0], ControlValueMapping[varControl.Name]);
                        }
                        break;
                    case "NumericUpDown":
                        break;
                    case "TextBox":
                        (varControl as TextBox).Text = value;
                        break;
                    case "ComboBox":
                        System.Windows.Forms.ComboBox c = ((System.Windows.Forms.ComboBox)varControl);
                        if (string.IsNullOrEmpty(value))
                            value = int.MinValue.ToString();
                        c.SelectedValue = value;


                        break;
                }
            }



            //foreach (Control var in this.Controls)
            //{
            //    if (var.Name == tag)
            //    {
            //        Type type = var.GetType();

            //    }


            //}

            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        public override void SetValues(OrderSingle order)
        {

        }

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

                if (_parameters.ValidateWithOrderProperty.Count > 0 && order != null)
                {
                    Dictionary<string, string> dictalogFieldValue = GetFixValue();
                    double alogFieldValue = 0.0;
                    string alogFieldValueGeneric = string.Empty;
                    if (dictalogFieldValue != null && dictalogFieldValue.Count > 0)
                    {
                        foreach (var item in dictalogFieldValue)
                        {
                            double doubleValue = 0;

                            if (double.TryParse(dictalogFieldValue[item.Key], out doubleValue))
                            {
                                alogFieldValue = doubleValue;
                            }
                            else
                            {
                                alogFieldValueGeneric = dictalogFieldValue[item.Key];
                            }
                        }
                    }


                    char seperatorSemiColon = ';';
                    char seperatorComma = ',';
                    char seperatorCap = '^';
                    char seperatorAnd = '&';
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
                                error += ValidateOperatorWiseTTAlgoFields(alogFieldValue, alogFieldValueGeneric, item, _OrderField1DoubleValue, string.Empty);
                            }
                            else if (orderField1 != null && item.Item3.ToLower().Contains("this") && (item.Item2.ToLower().Contains(">") || item.Item2.ToLower().Contains("<") || item.Item2.ToLower().Contains("=")) && !Double.TryParse((orderField1.GetValue(order)).ToString(), out _OrderField1DoubleValue))
                            {
                                error += ValidateOperatorWiseTTAlgoFields(alogFieldValue, alogFieldValueGeneric, item, 0, (orderField1.GetValue(order)).ToString());
                            }
                            else if (orderField2 == null && !item.Item3.ToLower().Equals("all") && orderField1 != null && Double.TryParse((orderField1.GetValue(order)).ToString(), out _OrderField1DoubleValue))
                            {

                                if (!(item.Item3.Split(seperatorComma).ToList().Contains(_OrderField1DoubleValue.ToString())))
                                {
                                    if ((item.Item1.ToLower().Equals("ordersidetagvalue") || item.Item1.ToLower().Equals("ordertypetagvalue")) && dictFieldsCollection.Count > 1)
                                    {
                                        break;
                                    }
                                    error += "\u2022 " + "Please select valid " + item.Item1 + ".\n";
                                }

                            }
                            else if (orderField1 != null && orderField2 != null && !item.Item3.ToLower().Contains("this"))
                            {
                                double _OrderField2DoubleValue = 0;
                                if (Double.TryParse((orderField2.GetValue(order)).ToString(), out _OrderField2DoubleValue))
                                {
                                    alogFieldValue = _OrderField1DoubleValue;
                                    _OrderField1DoubleValue = _OrderField2DoubleValue;
                                    error = ValidateOperatorWiseTTAlgoFields(alogFieldValue, alogFieldValueGeneric, item, _OrderField1DoubleValue, string.Empty);
                                }
                                else
                                {
                                    error = ValidateOperatorWiseTTAlgoFields(alogFieldValue, alogFieldValueGeneric, item, 0, (orderField2.GetValue(order)).ToString());
                                }
                            }
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


        private string ValidateOperatorWiseTTAlgoFields(double alogFieldValue, string alogFieldValueGeneric, Tuple<string, string, string> item, double _OrderField1DoubleValue, string _OrderField1Generic)
        {
            string error = string.Empty;
            switch (item.Item2)
            {
                case "=":
                case "==":
                    if (!(alogFieldValue == _OrderField1DoubleValue))
                    {
                        error += "\u2022 " + "Algo field " + this._parameters.Names[0] + " should be equal to Order " + item.Item1 + " field.\n ";
                    }
                    else if (!(alogFieldValueGeneric.Equals(_OrderField1Generic)))
                    {
                        error += "\u2022 " + "Algo field " + this._parameters.Names[0] + " should be equal to Order " + item.Item1 + " field.\n ";
                    }
                    break;
            }
            return error;
        }
    }
}
