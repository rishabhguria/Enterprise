using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;

namespace NirvanaAlgoStrategyControlsUCEndTimeandDuration
{
    public partial class UCEndTimeandDuration : AlgoStrategyUserControl
    {
        //AlgoStrategyParameters _parameters = new AlgoStrategyParameters();
        //string[] strArray = null;
        OrderSingle _order = null;
        //private bool _isReplaceControl = false;

        public UCEndTimeandDuration()
        {
            InitializeComponent();
            //this.numericUpDown1.Enabled = false;
            this.radioButtonEndTime.Checked = true;
            endTimeDateTime.Value = DateTime.Now.ToString("HH:mm");

        }

        private void radioButtonEndTime_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonEndTime.Checked == true)
            {
                this.durationUpDown.Enabled = false;
                this.endTimeDateTime.Enabled = true;
                this.BtnNow.Enabled = true;
            }
        }

        private void radioButtonDuration_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonDuration.Checked == true)
            {
                this.endTimeDateTime.Enabled = false;
                this.BtnNow.Enabled = false;
                this.durationUpDown.Enabled = true;
            }
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
            if (radioButtonEndTime.Checked == true)
            {
                DateTime selectedTime = Convert.ToDateTime(endTimeDateTime.Value);
                DateTime currentTime = DateTime.Now.ToUniversalTime();
                if (_order != null)
                {
                    selectedTime = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(selectedTime, CachedDataManager.GetInstance.GetAUECTimeZone(_order.AUECID));

                }
                else
                {
                    selectedTime = selectedTime.ToUniversalTime();
                }


                if (currentTime >= selectedTime)
                {
                    return "End Time should be greater than Current Time";
                }
                else
                    return string.Empty;
            }
            else
            {
                int duration = int.Parse(durationUpDown.Value.ToString());
                if (duration <= 0)
                {
                    return "Duration should be greater than 0";
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public override AlgoStrategyParameters GetValue()
        {
            //throw new Exception("The method or operation is not implemented.");
            return _parameters;
        }

        public override void SetValues(AlgoStrategyParameters parameters)
        {
            _parameters = parameters;
            if (_parameters.Format == string.Empty)
            {
                _parameters.Format = DateTimeConstants.NirvanaDateTimeFormat;
            }
            //this.Name = parameters.Name;
            this.SetBounds(parameters.Xpos, parameters.Ypos, this.Width, this.Height);
            //string tempparams = _parameters.ID;
            //char seperator = ',';
            //strArray = tempparams.Split(seperator);

            endTimeDateTime.Name = _parameters.IDs[0];
            durationUpDown.Name = _parameters.IDs[1];

            this.radioButtonEndTime.Text = _parameters.Names[0];
            this.radioButtonDuration.Text = _parameters.Names[1];
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
                        if (this.radioButtonDuration.Checked == true)
                        {

                        }
                        else
                        {
                            idValuePair.Add(this.endTimeDateTime.Name, _parameters.DefaultValue);
                            return idValuePair;
                        }

                    }
                    else
                    {
                        return idValuePair;
                    }
                }
            }
            DateTime selectedTimeinUTC = Convert.ToDateTime(endTimeDateTime.Value).ToUniversalTime();
            if (_order != null)
            {
                selectedTimeinUTC = Prana.BusinessObjects.TimeZoneInfo.ConvertLocalTimeToUTC(Convert.ToDateTime(endTimeDateTime.Value), CachedDataManager.GetInstance.GetAUECTimeZone(_order.AUECID));
            }

            if (this.radioButtonDuration.Checked == true)
            {
                //tempID = strArray[1];
                idValuePair.Add(this.durationUpDown.Name, this.durationUpDown.Value.ToString());
            }
            else
            {
                //tempID = strArray[0];
                idValuePair.Add(this.endTimeDateTime.Name, selectedTimeinUTC.ToString(_parameters.Format));
            }
            return idValuePair;
            //  }
            // else
            //  {
            //  MessageBox.Show(strDec);
            // return null;
            //}
        }


        public override void SetUserControls(string type)
        {

        }
        public override AlgoStrategyUserControl UpdateParameterValues(AlgoStrategyUserControl aslsoStrategy, AlgoStrategyParametersToUpdate parameterToUpdate)
        {
            UCEndTimeandDuration ucEndTimeandDuration = (UCEndTimeandDuration)aslsoStrategy;
            if (parameterToUpdate != null)
            {
                ucEndTimeandDuration._parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                _parameters.ReuiredGroupName = parameterToUpdate.ReuiredGroupName;
                ucEndTimeandDuration.Enabled = Convert.ToBoolean(parameterToUpdate.Enabled);
                ucEndTimeandDuration.Visible = Convert.ToBoolean(parameterToUpdate.Visibility);
                ucEndTimeandDuration._parameters.IsRequired = parameterToUpdate.Required;
                if (_parameters.ReplaceVisible.Equals("no", StringComparison.OrdinalIgnoreCase) && !_isVisibleReplaceControl)
                {
                    ucEndTimeandDuration.Enabled = _isVisibleReplaceControl;
                }
            }
            return ucEndTimeandDuration;

        }
        public override AlgoStrategyUserControl GetUserCtrl(string underlyingText)
        {
            UCEndTimeandDuration uCEndTimeandDuration = new UCEndTimeandDuration();
            AlgoStrategyParameters algoStrategyParameters = GetClonedAlgoStrategyParameters(underlyingText);
            uCEndTimeandDuration.SetValues(algoStrategyParameters);
            uCEndTimeandDuration.Top = _parameters.Ypos;
            uCEndTimeandDuration.Left = _parameters.Xpos;
            uCEndTimeandDuration.Enabled = algoStrategyParameters.IsEnabled;
            return uCEndTimeandDuration;
        }

        public override void SetFixValues(string tag, string value, OrderSingle order)
        {
            if (this.durationUpDown.Name == tag)
            {
                this.radioButtonDuration.Checked = true;
                this.durationUpDown.Value = decimal.Parse(value);
            }
            else if (this.endTimeDateTime.Name == tag)
            {
                this.radioButtonEndTime.Checked = true;
                //this.endTimeDateTime.Value = value;
                if (value != _parameters.DefaultValue)
                {
                    if (_parameters.Format == string.Empty)
                    {
                        _parameters.Format = DateTimeConstants.NirvanaDateTimeFormat;
                    }

                    endTimeDateTime.Value = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.ParseExact(value, _parameters.Format, null), CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));
                }
                else
                {
                    endTimeDateTime.Value = order.TransactionTime.ToString(DateTimeConstants.NirvanaDateTimeFormat);
                    //endTimeDateTime.Value = TimeZoneHelper.GetAUECTimeFromUTC(order.AUECID, DateTime.ParseExact(order.TransactionTime, DateTimeConstants.NirvanaDateTimeFormat, null));
                }
            }
        }


        #endregion
        public override string Validate(OrderSingle order, Dictionary<string, string> tagValueDictionary)
        {
            try
            {
                string error = base.Validate(order, tagValueDictionary);

                if (!string.IsNullOrEmpty(error))
                    return error;
                Dictionary<string, string> algoPropertiesDictionary = new Dictionary<string, string>();
                if (tagValueDictionary == null)
                {
                    algoPropertiesDictionary = order.AlgoProperties.TagValueDictionary;
                }
                else
                {
                    algoPropertiesDictionary = tagValueDictionary;
                }
                if (radioButtonEndTime.Checked == true)
                {
                    if (_parameters.ValidateWith.Count > 0 && algoPropertiesDictionary.ContainsKey(_parameters.ValidateWith[0]))
                    {
                        string startTimetemp = algoPropertiesDictionary[_parameters.ValidateWith[0]];

                        if (endTimeDateTime.Value.ToString() != _parameters.DefaultValue)
                        {
                            DateTime endtimeDate = Convert.ToDateTime(endTimeDateTime.Value);

                            DateTime startTimeDate = DateTimeConstants.MinValue;
                            DateTime.TryParse(startTimetemp, out startTimeDate);
                            if (startTimeDate == DateTimeConstants.MinValue)
                            {
                                if (endtimeDate > startTimeDate)
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
                    else
                        return string.Empty;
                    // endtime < auecendtime

                }
                else
                {
                    //duration + starttime should be less than AUEC endtime
                    return string.Empty;

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
            return string.Empty;
        }
        public override void SetValues(OrderSingle order)
        {
            //Dileep: To check the refilling of the default values in the End Time on the Replace / Cancel New
            if (_order == null)
            {
                _order = order;
                endTimeDateTime.Value = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(order.AUECID));

            }
        }

        private void BtnNow_Click(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            if (_order != null)
            {
                currentTime = Prana.ClientCommon.MarketStartEndClearanceTimes.GetInstance().GetAUECMarketEndTime(_order.AUECID);//TimeZoneHelper.GetAUECTimeFromUTC(_order.AUECID, DateTime.Now.ToUniversalTime());
            }
            //this.endTimeDateTime.Value = currentTime.AddHours(7.0).ToString("HH:mm");
            this.endTimeDateTime.Value = currentTime.ToString("HH:mm");

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
