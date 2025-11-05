using Prana.BusinessObjects;
using Prana.CommonDataCache;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace NirvanaAlgoStrategyControlsUCStartTime
{
    public partial class UCStartTime : AlgoStrategyUserControl
    {
        OrderSingle _order = null;
        //AlgoStrategyParameters _parameters = new AlgoStrategyParameters();
        private bool _isReplaceControl = false;

        private bool _isBlankAllowed = false;
        public bool IsBlankAllowed
        {
            get { return _isBlankAllowed; }
            set
            {
                _isBlankAllowed = value;
            }
        }
        public UCStartTime()
        {
            InitializeComponent();
            ultraDateTimeEditor1.Value = DateTime.Now.ToString("HH:mm");

        }

        private void BtnNow_Click(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;

            if (_order != null)
            {
                currentTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(_order.AUECID));
            }
            this.ultraDateTimeEditor1.Value = currentTime.ToString("HH:mm");
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
            DateTime currentTime = DateTime.Now;
            if (_isReplaceControl)
            {
                _parameters.IsRequired = false;
                return string.Empty;
            }
            if (_order != null)
            {
                currentTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(_order.AUECID));
            }
            //return "Start Time should be greater than Current Time";
            if ((Convert.ToDateTime(ultraDateTimeEditor1.Value)).TimeOfDay < currentTime.TimeOfDay)
            {
                return "Start Time should be greater than Current Time";
            }
            else
                return string.Empty;



        }
        public override AlgoStrategyParameters GetValue()
        {
            // return null;
            //AlgoStrategyParameters Algo = new AlgoStrategyParameters();
            return _parameters;
        }

        public override void SetValues(AlgoStrategyParameters parameters)
        {
            _parameters = parameters;
            if (_parameters.Format == string.Empty)
            {
                _parameters.Format = DateTimeConstants.NirvanaDateTimeFormat;
            }
            if (_parameters.CustomAttributesDict.ContainsKey("LabelWidth"))
            {
                int width = 0;
                if (Int32.TryParse(_parameters.CustomAttributesDict["LabelWidth"], out width))
                    this.label1.Width = width;
            }
            if (_parameters.CustomAttributesDict.ContainsKey("ButtonWidth"))
            {
                int width = 0;
                if (Int32.TryParse(_parameters.CustomAttributesDict["ButtonWidth"], out width))
                    this.BtnNow.Width = width;
            }
            if (_parameters.CustomAttributesDict.ContainsKey("TimeControlWidth"))
            {
                int width = 0;
                if (Int32.TryParse(_parameters.CustomAttributesDict["TimeControlWidth"], out width))
                    this.ultraDateTimeEditor1.Width = width;
            }
            if (_parameters.CustomAttributesDict.ContainsKey("IsBlankAllowed"))
            {
                bool isBlankAllowed = false;
                if (Boolean.TryParse(_parameters.CustomAttributesDict["IsBlankAllowed"], out isBlankAllowed))
                    this._isBlankAllowed = isBlankAllowed;
            }
            this.ultraDateTimeEditor1.Location = new System.Drawing.Point(this.label1.Width, 0);
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.BtnNow.Location = new System.Drawing.Point(this.label1.Width + this.ultraDateTimeEditor1.Width, 0);
            this.Width = label1.Width + ultraDateTimeEditor1.Width + BtnNow.Width;

            //this.Name = parameters.Name;
            this.SetBounds(parameters.Xpos, parameters.Ypos, this.Width, this.Height);
            ultraDateTimeEditor1.Name = _parameters.IDs[0];
            this.label1.Text = _parameters.Names[0] + (_parameters.IsRequired ? "*" : string.Empty);
            if (IsBlankAllowed)
            {
                Nullable<DateTime> myDateTime = null;
                this.ultraDateTimeEditor1.Value = myDateTime;

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
            else if ((this.Enabled || this.Visible) && label1.Text.Contains("*"))
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


            // if field is optional than if it is not valid send blank dictionary, send default value if it is there
            if (!_parameters.IsRequired)
            {

                if (ValidateSetValues() != string.Empty)
                {
                    if (_parameters.DefaultValue != null)
                    {
                        if (_parameters.DefaultValue != string.Empty)
                        {
                            idValuePair.Add(this.ultraDateTimeEditor1.Name, _parameters.DefaultValue);
                            return idValuePair;
                        }
                    }
                    else
                    {
                        return idValuePair;
                    }
                }
            }

            DateTime selectedTimeinUTC = Convert.ToDateTime(ultraDateTimeEditor1.Value).ToUniversalTime();
            if (_order != null)
            {
                selectedTimeinUTC = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(Convert.ToDateTime(ultraDateTimeEditor1.Value), CachedDataManager.GetInstance.GetAUECTimeZone(_order.AUECID));
            }

            if (IsBlankAllowed && (ultraDateTimeEditor1.Value == null))
            {
                idValuePair.Add(ultraDateTimeEditor1.Name, "");
            }
            else
            {
                idValuePair.Add(this.ultraDateTimeEditor1.Name, selectedTimeinUTC.ToString(_parameters.Format));
            }
            return idValuePair;

        }


        public override void SetUserControls(string type)
        {

        }
        public override AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl aslsoStrategy, AlgoStrategyParametersToUpdate parameterToUpdate)
        {
            UCStartTime ucStartTime = (UCStartTime)aslsoStrategy;
            if (parameterToUpdate != null)
            {
                ucStartTime.Enabled = parameterToUpdate.Enabled;
                ucStartTime.Visible = parameterToUpdate.Visibility;
                ucStartTime.label1.Enabled = parameterToUpdate.Enabled;
                ucStartTime.label1.Visible = parameterToUpdate.Visibility;
                ucStartTime.BtnNow.Enabled = parameterToUpdate.Enabled;
                ucStartTime.BtnNow.Visible = parameterToUpdate.Visibility;
                ucStartTime._parameters.IsRequired = parameterToUpdate.Required;
                label1.Text = label1.Text.Replace("*", "") + (ucStartTime._parameters.IsRequired ? "*" : string.Empty);
                if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                {
                    ucStartTime.Enabled = _isVisibleReplaceControl;
                }
            }
            return ucStartTime;
        }


        public override AlgoStrategyUserControl GetUserCtrl(string underlyingText)
        {
            UCStartTime uCStartTime = new UCStartTime();
            AlgoStrategyParameters algoStrategyParameters = GetClonedAlgoStrategyParameters(underlyingText);
            uCStartTime.SetValues(algoStrategyParameters);
            uCStartTime.Top = _parameters.Ypos;
            uCStartTime.Left = _parameters.Xpos;
            uCStartTime.Enabled = algoStrategyParameters.IsEnabled;
            return uCStartTime;
        }

        public override void SetFixValues(string fixTag, string value, OrderSingle order)
        {
            //_isReplaceControl = isReplaceControl;
            _order = order;
            if (ultraDateTimeEditor1.Name == fixTag)
            {

                //ultraDateTimeEditor1.Value = value;
                if (value != _parameters.DefaultValue)
                {
                    if (_parameters.Format == string.Empty)
                    {
                        _parameters.Format = DateTimeConstants.NirvanaDateTimeFormat;
                    }
                    ultraDateTimeEditor1.Value = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.ParseExact(value, _parameters.Format, null), CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                }
                else
                {
                    ultraDateTimeEditor1.Value = order.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);

                    //ultraDateTimeEditor1.Value = TimeZoneHelper.GetAUECTimeFromUTC(order.AUECID, DateTime.ParseExact(order.TransactionTime,DateTimeConstants.NirvanaDateTimeFormat,null));
                }

            }


        }

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


        public override void SetValues(OrderSingle order)
        {
            //Dileep: To check the refilling of the default values in the Start Time on the Replace / Cancel New
            if (_order == null)
            {
                _order = order;
                if (IsBlankAllowed)
                {
                    Nullable<DateTime> myDateTime = null;
                    this.ultraDateTimeEditor1.Value = myDateTime;
                }
                else
                {
                    ultraDateTimeEditor1.Value = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                }
            }
        }

    }
}
