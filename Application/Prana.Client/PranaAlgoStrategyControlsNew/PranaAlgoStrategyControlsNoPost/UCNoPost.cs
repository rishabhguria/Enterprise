using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PranaAlgoStrategyControlsNoPost
{
    public partial class UCNoPost : AlgoStrategyUserControl
    {
        //AlgoStrategyParameters _parameters = new AlgoStrategyParameters();
        //private bool _isReplaceControl = false;

        public UCNoPost()
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

            this.SetBounds(parameters.Xpos, parameters.Ypos, this.Width, this.Height);
            chkbxNoPost.Name = _parameters.IDs[0];
            spinner.Name = _parameters.IDs[0];
            chkbxNoPost.ForeColor = Color.Black;
            chkbxNoPost.Text = _parameters.Names[0];
            lblName.Text = _parameters.Names[1];


            string[] strcontrolvalues = _parameters.InnerControlValues.ToArray();
            spinner.Value = double.Parse(strcontrolvalues[0]);
            spinner.MinValue = double.Parse(strcontrolvalues[1]);
            spinner.MaxValue = double.Parse(strcontrolvalues[2]);
            spinner.Increment = double.Parse(strcontrolvalues[3]);
            double temp = 0.0;
            double.TryParse(_parameters.DefaultValue, out temp);
            spinner.Value = temp;






        }


        public override Dictionary<string, string> GetFixValue()
        {

            Dictionary<string, string> idValuePair = new Dictionary<string, string>();
            //if (!_parameters.IsRequired)
            //{
            //    if (ValidateSetValue() != string.Empty)
            //    {
            //        if (_parameters.DefaultValue != string.Empty)
            //        {
            //            idValuePair.Add(this.spinner.Name, _parameters.DefaultValue);
            //            return idValuePair;
            //        }
            //        else
            //        {
            //            return idValuePair;
            //        }
            //    }
            //}
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

            if (chkbxNoPost.Checked)
            {
                idValuePair.Add(chkbxNoPost.Name, "0");
            }
            else
            {
                idValuePair.Add(spinner.Name, spinner.Value.ToString());
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
                this.spinner.Value = double.Parse(value);
            }
        }



        public override AlgoStrategyUserControl GetUserCtrl(string underlyingText)
        {
            UCNoPost ucNoPost = new UCNoPost();
            AlgoStrategyParameters algoStrategyParameters = GetClonedAlgoStrategyParameters(underlyingText);
            ucNoPost.SetValues(algoStrategyParameters);
            ucNoPost.Top = _parameters.Ypos;
            ucNoPost.Left = _parameters.Xpos;
            ucNoPost.Enabled = algoStrategyParameters.IsEnabled;
            return ucNoPost;
        }
        public override AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl aslsoStrategy, AlgoStrategyParametersToUpdate parameterToUpdate)
        {
            UCNoPost ucNoPost = (UCNoPost)aslsoStrategy;
            if (parameterToUpdate != null)
            {
                ucNoPost._parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                _parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                ucNoPost.Enabled = Convert.ToBoolean(parameterToUpdate.Enabled);
                ucNoPost.Visible = Convert.ToBoolean(parameterToUpdate.Visibility);
                ucNoPost._parameters.IsRequired = parameterToUpdate.Required;
                lblName.Text = lblName.Text.Replace("*", "") + (ucNoPost._parameters.IsRequired ? "*" : string.Empty);
                if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                {
                    ucNoPost.Enabled = _isVisibleReplaceControl;
                }

            }
            return ucNoPost;

        }



        public override void SetValues(OrderSingle order)
        {
        }
        #endregion

        private void chkbxNoPost_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkbxNoPost.Checked)
            {
                spinner.Enabled = false;
            }
            else
            {
                spinner.Enabled = true;
            }
        }

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
