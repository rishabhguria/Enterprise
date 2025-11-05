using Prana.BusinessObjects;
using Prana.Global.Utilities;
using Prana.Utilities.StringUtilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NirvanaAlgoStrategyControlsUCOnClose
{
    public partial class UCOnClose : AlgoStrategyUserControl
    {
        //AlgoStrategyParameters _parameters = new AlgoStrategyParameters();
        //string[] strArray = null;
        //char seperator = ',';
        //private bool _isReplaceControl = false;

        public UCOnClose()
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
            if (_isReplaceControl)
            {
                _parameters.IsRequired = false;
                return string.Empty;
            }
            //throw new Exception("The method or operation is not implemented.");
            if ((radioButton1.Checked == true) && (textBox1.Text == null))
            {
                return "Please Enter the valid values in the Shares field";
            }
            else
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
            //this.Name = parameters.Name;
            this.SetBounds(parameters.Xpos, parameters.Ypos, this.Width, this.Height);
            //string tempparams = _parameters.ID;

            this.numericUpDown1.Name = _parameters.IDs[0];//strArray[0];
            this.textBox1.Name = _parameters.IDs[1]; //strArray[1];

            this.radioButton2.Text = _parameters.Names[0];// strArrayNames[0];
            this.radioButton1.Text = _parameters.Names[1];//strArrayNames[1];
            if (_parameters.DefaultValue != null)
            {
                this.textBox1.Text = _parameters.DefaultValue;
            }
        }



        public override Dictionary<string, string> GetFixValue()
        {

            //  string strDec = ValidateValues();
            //if (strDec == string.Empty)
            // {
            //string tempID;
            Dictionary<string, string> idValuePair = new Dictionary<string, string>();

            if (_isReplaceControl && !_parameters.SendOnReplace)
            {
                _parameters.IsRequired = false;
                return idValuePair;
            }
            if (!this.Enabled || !this.Visible)
            {
                _parameters.IsRequired = false;
                _parameters.ReuiredGroupName = string.Empty;
                if (_parameters.SendOnReplace && !_isReplaceControl && !_isVisibleReplaceControl)
                { }
                else
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
                //tempID = strArray[0];
                idValuePair.Add(this.numericUpDown1.Name, this.numericUpDown1.Value.ToString());
            }
            else
            {
                //tempID = strArray[1];
                idValuePair.Add(this.textBox1.Name, this.textBox1.Text);
            }
            return idValuePair;
            // }
            // else
            // {
            //MessageBox.Show(strDec);
            //return null;
            //

        }

        public override void SetUserControls(string type)
        {


        }
        public override AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl aslsoStrategy, AlgoStrategyParametersToUpdate parameterToUpdate)
        {
            UCOnClose ucOnClose = (UCOnClose)aslsoStrategy;
            if (parameterToUpdate != null)
            {
                ucOnClose._parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                _parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                ucOnClose.Enabled = Convert.ToBoolean(parameterToUpdate.Enabled);
                ucOnClose.Visible = Convert.ToBoolean(parameterToUpdate.Visibility);
                ucOnClose._parameters.IsRequired = parameterToUpdate.Required;
                if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                {
                    ucOnClose.Enabled = _isVisibleReplaceControl;
                }
            }
            return ucOnClose;
        }

        public override AlgoStrategyUserControl GetUserCtrl(string underlyingText)
        {
            UCOnClose uCOnClose = new UCOnClose();
            AlgoStrategyParameters algoStrategyParameters = GetClonedAlgoStrategyParameters(underlyingText);
            uCOnClose.SetValues(algoStrategyParameters);
            uCOnClose.Top = _parameters.Ypos;
            uCOnClose.Left = _parameters.Xpos;
            uCOnClose.Enabled = algoStrategyParameters.IsEnabled;
            return uCOnClose;
        }
        public override void SetFixValues(string tag, string value, OrderSingle order)
        {
            if (this.numericUpDown1.Name == tag)
            {
                this.radioButton2.Checked = true;
                this.numericUpDown1.Value = decimal.Parse(value);
            }
            else if (this.textBox1.Name == tag)
            {
                this.radioButton1.Checked = true;
                this.textBox1.Text = value;
            }
        }


        #endregion

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton2.Checked == true)
            {
                this.numericUpDown1.Enabled = true;
                this.textBox1.Enabled = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked == true)
            {
                this.numericUpDown1.Enabled = false;
                this.textBox1.Enabled = true;
            }
        }
        public override void SetValues(OrderSingle order)
        {

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (!RegularExpressionValidation.IsPositiveInteger(textBox1.Text))
            {
                MessageBox.Show("Please enter valid positive Integer");
                textBox1.Text = _parameters.DefaultValue;
                textBox1.Focus();
            }
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
    }
}
