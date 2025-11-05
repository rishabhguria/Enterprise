using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Drawing;




namespace NirvanaAlgoStrategyControlsUCEndTime
{
    public partial class UCEndTime : AlgoStrategyUserControl
    {
        // AlgoStrategyParameters _parameters = new AlgoStrategyParameters();
        OrderSingle _order = null;
        private bool _isBlankAllowed = false;
        public bool IsBlankAllowed
        {
            get { return _isBlankAllowed; }
            set
            {
                _isBlankAllowed = value;
            }
        }
        public UCEndTime()
        {
            try
            {
                InitializeComponent();
                ultraDateTimeEditor1.Value = DateTime.Now.ToString("HH:mm");
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

        #region AlgoStrategyUserControl Members

        public override string ValidateValues()
        {
            //throw new Exception("The method or operation is not implemented.");
            try
            {
                if (_parameters.IsRequired)
                {
                    return ValidateSetValue();
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }

        }
        private string ValidateSetValue()
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                if (_isReplaceControl)
                {
                    _parameters.IsRequired = false;
                    return string.Empty;
                }
                if (_order != null && checkBox1.Checked)
                {
                    currentTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(_order.AUECID));
                }
                //return "Start Time should be greater than Current Time";
                if ((Convert.ToDateTime(ultraDateTimeEditor1.Value)).TimeOfDay < currentTime.TimeOfDay)
                {
                    return "End Time should be greater than Current Time";
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }



        }
        public override AlgoStrategyParameters GetValue()
        {
            //throw new Exception("The method or operation is not implemented.");
            return _parameters;
        }

        public override void SetValues(AlgoStrategyParameters parameters)
        {
            try
            {
                _parameters = parameters;
                if (_parameters.Format == string.Empty)
                {
                    _parameters.Format = DateTimeConstants.NirvanaDateTimeFormat;
                }
                this.Name = parameters.Names[0];

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
                this.checkBox1.CheckAlign = ContentAlignment.MiddleLeft;
                this.label1.TextAlign = ContentAlignment.MiddleLeft;
                this.ultraDateTimeEditor1.Location = new System.Drawing.Point(this.checkBox1.Width + label1.Width, 0);
                this.BtnNow.Location = new System.Drawing.Point(this.checkBox1.Width + this.ultraDateTimeEditor1.Width + label1.Width, 0);
                this.Width = this.checkBox1.Width + label1.Width + ultraDateTimeEditor1.Width + BtnNow.Width;

                this.SetBounds(parameters.Xpos, parameters.Ypos, this.Width, this.Height);
                ultraDateTimeEditor1.Name = _parameters.IDs[0];
                label1.Text = _parameters.Names[0] + (_parameters.IsRequired ? "*" : string.Empty);
                if (IsBlankAllowed)
                {
                    Nullable<DateTime> myDateTime = null;
                    this.ultraDateTimeEditor1.Value = myDateTime;

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


        public override Dictionary<string, string> GetFixValue()
        {

            try
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
                if (!this.checkBox1.Checked)
                    return idValuePair;

                if (!_parameters.IsRequired)
                {
                    if (ValidateSetValue() != string.Empty)
                    {
                        if (_parameters.DefaultValue != string.Empty)
                        {
                            idValuePair.Add(this.ultraDateTimeEditor1.Name, _parameters.DefaultValue);
                            return idValuePair;
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
                    idValuePair.Add(ultraDateTimeEditor1.Name, selectedTimeinUTC.ToString(_parameters.Format));
                }
                return idValuePair;


            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;

                }
                return null;
            }
        }

        public override void SetUserControls(string type)
        {

        }
        public override void SetFixValues(string tag, string value, OrderSingle order)
        {
            try
            {
                if (order != null)
                {
                    if (this.ultraDateTimeEditor1.Name == tag)
                    {
                        this.checkBox1.Checked = true;
                        // this.ultraDateTimeEditor1.Value = value;
                        if (value != _parameters.DefaultValue)
                        {
                            if (_parameters.Format == string.Empty)
                            {
                                _parameters.Format = DateTimeConstants.NirvanaDateTimeFormat;
                            }
                            if (value != string.Empty)
                            {
                                ultraDateTimeEditor1.Value = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.ParseExact(value, _parameters.Format, null), CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                            }
                        }
                        else
                        {
                            ultraDateTimeEditor1.Value = order.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                            //ultraDateTimeEditor1.Value = TimeZoneHelper.GetAUECTimeFromUTC(order.AUECID, DateTime.ParseExact(order.TransactionTime, DateTimeConstants.NirvanaDateTimeFormat, null));
                        }

                    }
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
        #endregion AlgoStrategyUserControl Members

        #region AlgoStrategyUserControl Members



        public override AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl aslsoStrategy, AlgoStrategyParametersToUpdate parameterToUpdate)
        {
            UCEndTime ucEndTime = (UCEndTime)aslsoStrategy;
            if (parameterToUpdate != null)
            {
                ucEndTime._parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                _parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                //ucEndTime.Enabled = parameterToUpdate.Enabled;
                ucEndTime.Visible = parameterToUpdate.Visibility;
                //ucEndTime.checkBox1.Enabled = parameterToUpdate.Enabled;
                ucEndTime.checkBox1.Visible = parameterToUpdate.Visibility;
                //ucEndTime.BtnNow.Enabled = parameterToUpdate.Enabled;
                ucEndTime.BtnNow.Visible = parameterToUpdate.Visibility;
                ucEndTime.Enabled = parameterToUpdate.Enabled;
                ucEndTime._parameters.IsRequired = parameterToUpdate.Required;
                label1.Text = label1.Text.Replace("*", "") + (ucEndTime._parameters.IsRequired ? "*" : string.Empty);

                if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                {
                    ucEndTime.Enabled = _isVisibleReplaceControl;
                }
            }
            return ucEndTime;
        }

        public override AlgoStrategyUserControl GetUserCtrl(string underlyingText)
        {
            UCEndTime ucEndTime = new UCEndTime();
            try
            {
                AlgoStrategyParameters algoStrategyParameters = GetClonedAlgoStrategyParameters(underlyingText);
                ucEndTime.SetValues(algoStrategyParameters);
                ucEndTime.Top = _parameters.Ypos;
                ucEndTime.Left = _parameters.Xpos;
                ucEndTime.Enabled = algoStrategyParameters.IsEnabled;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return ucEndTime;
        }

        public override string Validate(OrderSingle order, Dictionary<string, string> tagValueDictionary)
        {
            try
            {
                string error = base.Validate(order, tagValueDictionary);

                Dictionary<string, string> algoPropertiesDictionary = new Dictionary<string, string>();
                if (tagValueDictionary == null)
                {
                    algoPropertiesDictionary = order.AlgoProperties.TagValueDictionary;
                }
                else
                {
                    algoPropertiesDictionary = tagValueDictionary;
                }

                if (!string.IsNullOrEmpty(error))
                    return error;

                if (_parameters.ValidateWith.Count > 0 && algoPropertiesDictionary.ContainsKey(_parameters.ValidateWith[0]))
                {
                    if (ultraDateTimeEditor1.Value != null && (ultraDateTimeEditor1.Value.ToString() != _parameters.DefaultValue))
                    {
                        DateTime endtimeDate = Convert.ToDateTime(ultraDateTimeEditor1.Value).ToUniversalTime();


                        string startTimetemp = algoPropertiesDictionary[_parameters.ValidateWith[0]];

                        int year = Convert.ToInt16(startTimetemp.Substring(0, 4));
                        var mm = Convert.ToInt16(startTimetemp.Substring(4, 2));
                        int dd = Convert.ToInt16(startTimetemp.Substring(6, 2));
                        int hh = Convert.ToInt16(startTimetemp.Substring(9, 2));
                        int min = Convert.ToInt16(startTimetemp.Substring(12, 2));
                        int ss = Convert.ToInt16(startTimetemp.Substring(15, 2));
                        DateTime startDt = new DateTime(year, mm, dd, hh, min, ss);

                        if (startDt != DateTimeConstants.MinValue)
                        {
                            if (endtimeDate >= startDt)
                                return string.Empty;
                            else
                                return "End Time should be greater than Start Time";
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
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
            return string.Empty;

        }
        public override void SetValues(OrderSingle order)
        {
            //Dileep: To check the refilling of the default values in the End Time on the Replace / Cancel New
            try
            {
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
                        DateTime currentTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                        if (_order != null)
                        {
                            currentTime = Prana.ClientCommon.MarketStartEndClearanceTimes.GetInstance().GetAUECMarketEndTime(_order.AUECID);
                        }
                        this.ultraDateTimeEditor1.Value = currentTime.ToString("HH:mm");
                    }
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
        #endregion

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

        private void BtnNow_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked == true)
                {
                    DateTime currentTime = DateTime.Now;

                    if (_order != null)
                    {
                        currentTime = Prana.ClientCommon.MarketStartEndClearanceTimes.GetInstance().GetAUECMarketEndTime(_order.AUECID);//TimeZoneHelper.GetAUECTimeFromUTC(_order.AUECID, DateTime.Now.ToUniversalTime());
                    }
                    //this.ultraDateTimeEditor1.Value = currentTime.AddHours(7.0).ToString("HH:mm");
                    this.ultraDateTimeEditor1.Value = currentTime.ToString("HH:mm");
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


    }
}
