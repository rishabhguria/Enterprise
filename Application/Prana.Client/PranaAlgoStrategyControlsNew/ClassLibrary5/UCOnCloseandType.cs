using Prana.BusinessObjects;
using Prana.Global.Utilities;
using Prana.Utilities.StringUtilities;
using System;
using System.Collections.Generic;

namespace NirvanaAlgoStrategyControlsUCOnCloseandType
{
    public partial class UCOnCloseandType : AlgoStrategyUserControl
    {
        //AlgoStrategyParameters _parameters = new AlgoStrategyParameters();
        //string[] strArray = null;
        //char seperator = ',';
        //private bool _isReplaceControl = false;

        public UCOnCloseandType()
        {
            InitializeComponent();
            this.radioButton2.Checked = true;
            //string[] strArrayNames = _parameters.Name.Split(seperator);

        }


        #region AlgoStrategyUserControl Members

        public override string ValidateValues()
        {
            if (_parameters.IsRequired)
            {
                return ValidateSetValues();
            }
            else
                return string.Empty;
        }
        private string ValidateSetValues()
        {
            if ((radioButton2.Checked == true) && (comboBox1.SelectedItem == null))
            {
                return "Please select a valid Close % Type";
            }
            else if ((radioButton1.Checked == true) && ((textBox1.Text == null) || (textBox1.Text == string.Empty)))
            {
                return "Please Enter the valid values in the Shares field";
            }
            else if (!RegularExpressionValidation.IsPositiveInteger(textBox1.Text))
            {
                textBox1.Focus();
                return "Please enter valid positive Integer";
            }
            else
                return string.Empty;





            // throw new Exception("The method or operation is not implemented.");
            //if ((radioButton1.Checked == true) && ((textBox1.Text == null) || (textBox1.Text == string.Empty)))
            //{                
            //    return "Please Enter the valid values in the Shares field";
            //}
            //else if (!RegularExpressionValidation.IsPositiveInteger(textBox1.Text))
            //{
            //    textBox1.Focus();
            //    return "Please enter valid positive Integer";
            //}
            //else if ((radioButton2.Checked == true) && (comboBox1.SelectedItem == null))
            //{
            //    return "Please select a valid Close % Type";
            //}
            //else
            //    return string.Empty;
        }

        public override AlgoStrategyParameters GetValue()
        {
            //throw new Exception("The method or operation is not implemented.");
            return _parameters;
        }

        public override void SetValues(AlgoStrategyParameters parameters)
        {
            _parameters = parameters;
            //this.Name = parameters.Name;
            this.SetBounds(parameters.Xpos, parameters.Ypos, this.Width, this.Height);
            //string tempparams = _parameters.ID;

            //strArray = tempparams.Split(seperator);
            this.numericUpDown1.Name = _parameters.IDs[0];
            this.textBox1.Name = _parameters.IDs[2];
            this.comboBox1.Name = _parameters.IDs[1];

            this.radioButton2.Text = _parameters.Names[0];
            this.label1.Text = _parameters.Names[1];
            this.radioButton1.Text = _parameters.Names[2];

            if (_parameters.DefaultValue != null)
            {
                this.textBox1.Text = _parameters.DefaultValue;
            }
        }
        public override Dictionary<string, string> GetFixValue()
        {


            // string strDec = ValidateValues();
            //  if (strDec == string.Empty)
            // {
            Dictionary<string, string> idValuePair = new Dictionary<string, string>();

            if (!this.Enabled || !this.Visible)
            {
                _parameters.IsRequired = false;
                return idValuePair;
            }
            if (!_parameters.IsRequired)
            {
                if (ValidateSetValues() != string.Empty)
                {
                    if (_parameters.DefaultValue != string.Empty)
                    {
                        idValuePair.Add(this.numericUpDown1.Name, _parameters.DefaultValue);
                        return idValuePair;
                    }
                    else
                    {
                        return idValuePair;
                    }
                }

            }
            if (this.radioButton2.Checked == true)
            {
                idValuePair.Add(this.numericUpDown1.Name, this.numericUpDown1.Value.ToString());
                //if (this.comboBox1.SelectedItem != null)
                //{
                //TODO: check for null values before reading the values

                idValuePair.Add(this.comboBox1.Name, this.comboBox1.SelectedItem.ToString());
                //}
            }
            else
                idValuePair.Add(this.textBox1.Name, this.textBox1.Text);

            return idValuePair;
            // }
            //else
            //{
            //    MessageBox.Show(strDec);
            //    return null;
            //}


        }

        public override void SetUserControls(string type)
        {

        }
        public override AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl aslsoStrategy, AlgoStrategyParametersToUpdate parameterToUpdate)
        {
            UCOnCloseandType ucSUCOnCloseandType = (UCOnCloseandType)aslsoStrategy;
            if (parameterToUpdate != null)
            {
                ucSUCOnCloseandType._parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                _parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                ucSUCOnCloseandType.Enabled = Convert.ToBoolean(parameterToUpdate.Enabled);
                ucSUCOnCloseandType.Visible = Convert.ToBoolean(parameterToUpdate.Visibility);
                ucSUCOnCloseandType._parameters.IsRequired = parameterToUpdate.Required;

            }
            return ucSUCOnCloseandType;
        }

        public override AlgoStrategyUserControl GetUserCtrl(string underlyingText)
        {
            UCOnCloseandType uCOnCloseandType = new UCOnCloseandType();
            AlgoStrategyParameters algoStrategyParameters = GetClonedAlgoStrategyParameters(underlyingText);
            uCOnCloseandType.SetValues(algoStrategyParameters);
            uCOnCloseandType.Top = _parameters.Ypos;
            uCOnCloseandType.Left = _parameters.Xpos;
            uCOnCloseandType.Enabled = algoStrategyParameters.IsEnabled;
            return uCOnCloseandType;
        }

        public override void SetFixValues(string tag, string value, OrderSingle order)
        {
            if (this.numericUpDown1.Name == tag)
            {
                this.radioButton2.Checked = true;
                this.numericUpDown1.Value = decimal.Parse(value);
            }
            else if (this.comboBox1.Name == tag)
            {
                this.radioButton2.Checked = true;
                this.comboBox1.SelectedValue = value;
            }
            else if (this.textBox1.Name == tag)
            {
                this.radioButton1.Checked = true;
                this.textBox1.Text = value;
            }
        }


        #endregion

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked == true)
            {
                this.comboBox1.Enabled = false;
                this.numericUpDown1.Enabled = false;
                this.textBox1.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked == true)
            {
                this.comboBox1.Enabled = true;
                this.numericUpDown1.Enabled = true;
                this.textBox1.Enabled = false;
            }
        }


        public override void SetValues(OrderSingle order)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            //if (!RegularExpressionValidation.IsPositiveInteger(textBox1.Text))
            //{
            //    MessageBox.Show("Please enter valid positive Integer");
            //    textBox1.Text = _parameters.DefaultValue;
            //    textBox1.Focus();
            //}
        }

        public override void SetIfReplaceControl(bool isReplaceControl)
        {
            //_isReplaceControl = isReplaceControl;
        }

        //private bool _isVisibleReplaceControl = true;
        public override void SetVisibilityIfReplaceControl(bool isReplaceControl)
        {
            //_isVisibleReplaceControl = isReplaceControl;
        }
    }
}
