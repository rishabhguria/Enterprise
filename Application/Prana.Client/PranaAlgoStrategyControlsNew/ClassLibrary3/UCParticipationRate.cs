using Prana.BusinessObjects;
using System;
using System.Collections.Generic;

namespace NirvanaAlgoStrategyControlsUCParticipationRate
{
    public partial class UCParticipationRate : AlgoStrategyUserControl
    {
        //AlgoStrategyParameters _parameters = new AlgoStrategyParameters();

        //private bool _isReplaceControl = false;

        public UCParticipationRate()
        {
            InitializeComponent();
            this.radioButton2.Checked = true;

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


        public string ValidateSetValues()
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
            this.numericUpDown1.Name = _parameters.IDs[0];
            this.radioButton2.Text = _parameters.Names[0];

        }
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
            idValuePair.Add(this.numericUpDown1.Name, this.numericUpDown1.Value.ToString());
            return idValuePair;
        }


        public override void SetUserControls(string type)
        {

        }
        public override AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl aslsoStrategy, AlgoStrategyParametersToUpdate parameterToUpdate)
        {
            UCParticipationRate ucParticipationRate = (UCParticipationRate)aslsoStrategy;
            if (parameterToUpdate != null)
            {
                ucParticipationRate._parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                _parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                ucParticipationRate.Enabled = Convert.ToBoolean(parameterToUpdate.Enabled);
                ucParticipationRate.Visible = Convert.ToBoolean(parameterToUpdate.Visibility);
                ucParticipationRate._parameters.IsRequired = parameterToUpdate.Required;
                if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                {
                    ucParticipationRate.Enabled = _isVisibleReplaceControl;
                }
            }
            return ucParticipationRate;
        }
        public override AlgoStrategyUserControl GetUserCtrl(string underlyingText)
        {
            UCParticipationRate uCParticipationRate = new UCParticipationRate();
            AlgoStrategyParameters algoStrategyParameters = GetClonedAlgoStrategyParameters(underlyingText);
            uCParticipationRate.SetValues(algoStrategyParameters);
            uCParticipationRate.Top = _parameters.Ypos;
            uCParticipationRate.Left = _parameters.Xpos;
            uCParticipationRate.Enabled = algoStrategyParameters.IsEnabled;
            return uCParticipationRate;
        }


        #endregion

        #region AlgoStrategyUserControl Members


        public override void SetFixValues(string tag, string value, OrderSingle order)
        {
            if (this.numericUpDown1.Name == tag)
            {
                this.radioButton2.Checked = true;
                this.numericUpDown1.Value = decimal.Parse(value);
            }
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
    }
}
