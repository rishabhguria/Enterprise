using Prana.BusinessObjects;
using System;
using System.Collections.Generic;


namespace NirvanaAlgoStrategyControlsUCExecutionMode
{
    public partial class UCExecutionMode : AlgoStrategyUserControl
    {
        //AlgoStrategyParameters _parameters = new AlgoStrategyParameters();
        //private bool _isReplaceControl = false;

        public UCExecutionMode()
        {
            InitializeComponent();
            this.radioButton1.Checked = true;

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
            this.radioButton1.Name = _parameters.IDs[0];

            this.groupBox1.Text = _parameters.Names[0];
        }
        public override Dictionary<string, string> GetFixValue()
        {
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
                        idValuePair.Add(this.radioButton1.Name, _parameters.DefaultValue);
                        return idValuePair;
                    }
                    else
                    {
                        return idValuePair;
                    }
                }

            }
            if (this.radioButton1.Checked == true)
            {
                idValuePair.Add(this.radioButton1.Name, "P");
            }
            else if (this.radioButton2.Checked == true)
            {
                idValuePair.Add(this.radioButton1.Name, "N");
            }
            else if (this.radioButton3.Checked == true)
            {
                idValuePair.Add(this.radioButton1.Name, "A");
            }
            else
                idValuePair.Add(this.radioButton1.Name, "NotSet");

            return idValuePair;
        }

        public override void SetUserControls(string type)
        {

        }
        public override AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl aslsoStrategy, AlgoStrategyParametersToUpdate parameterToUpdate)
        {
            UCExecutionMode ucExecutionMode = (UCExecutionMode)aslsoStrategy;
            if (parameterToUpdate != null)
            {
                ucExecutionMode._parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                _parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                ucExecutionMode.Enabled = Convert.ToBoolean(parameterToUpdate.Enabled);
                ucExecutionMode.Visible = Convert.ToBoolean(parameterToUpdate.Visibility);
                ucExecutionMode._parameters.IsRequired = parameterToUpdate.Required;
            }
            return ucExecutionMode;

        }
        public override AlgoStrategyUserControl GetUserCtrl(string underlyingText)
        {
            UCExecutionMode uCExecutionMode = new UCExecutionMode();
            AlgoStrategyParameters algoStrategyParameters = GetClonedAlgoStrategyParameters(underlyingText);
            uCExecutionMode.SetValues(algoStrategyParameters);
            uCExecutionMode.Top = _parameters.Ypos;
            uCExecutionMode.Left = _parameters.Xpos;
            uCExecutionMode.Enabled = algoStrategyParameters.IsEnabled;
            return uCExecutionMode;
        }
        public override void SetFixValues(string tag, string value, OrderSingle order)
        {
            if (this.radioButton1.Name == tag)
            {
                switch (value)
                {
                    case "A":
                        this.radioButton3.Checked = true;
                        break;
                    case "P":
                        this.radioButton1.Checked = true;
                        break;
                    case "N":
                        this.radioButton2.Checked = true;
                        break;
                    default:
                        this.radioButton1.Checked = true;
                        this.radioButton2.Checked = true;
                        this.radioButton3.Checked = true;
                        break;
                }
            }
        }


        #endregion

        public override void SetValues(OrderSingle order)
        {

        }

        #region AlgoStrategyUserControl Members


        public override void SetIfReplaceControl(bool isReplaceControl)
        {
            //_isReplaceControl = isReplaceControl;
        }


        //private bool _isVisibleReplaceControl = true;
        public override void SetVisibilityIfReplaceControl(bool isReplaceControl)
        {
            //_isVisibleReplaceControl = isReplaceControl;
        }
        #endregion
    }
}
