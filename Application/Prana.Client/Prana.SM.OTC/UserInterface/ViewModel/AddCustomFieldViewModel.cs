using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.ServiceConnector;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Prana.SM.OTC
{

    public enum DataTypes
    {
        String = 1,
        Numeric = 2,
        Date = 3,
        Bool = 4
    }

    public class AddCustomFieldViewModel : BindableBase
    {
        /// <summary>
        /// Save Custom Fields Command
        /// </summary>
        public DelegateCommand<Window> SaveCustomFieldsCommand { get; set; }

        /// <summary>
        /// Gets or sets the form close button.
        /// </summary>
        /// <value>
        /// The form close button.
        /// </value>
        public DelegateCommand<object> FormCloseButton { get; set; }

        /// <summary>
        /// Gets or sets the form closed button.
        /// </summary>
        /// <value>
        /// The form closed button.
        /// </value>
        public DelegateCommand<object> FormClosed { get; set; }

        /// <summary>
        /// OnForm Close Button Event
        /// </summary>
        public event EventHandler OnFormCloseButtonEvent;

        /// <summary>
        /// _customOTCFieldData 
        /// </summary>
        private CustomFieldsModel _customOTCFieldData = new CustomFieldsModel();

        /// <summary>
        /// Custom OTC Field Data
        /// </summary>
        public CustomFieldsModel CustomOTCFieldData
        {
            get { return _customOTCFieldData; }
            set
            {
                _customOTCFieldData = value;
                OnPropertyChanged();
            }
        }

        Visibility isVisibleLblNotificationBar = Visibility.Hidden;
        public Visibility IsVisibleLblNotificationBar
        {
            get { return isVisibleLblNotificationBar; }
            set { isVisibleLblNotificationBar = value; OnPropertyChanged(); }
        }

        Color notificationBarColor = Color.Green;
        public Color NotificationBarColor
        {
            get { return notificationBarColor; }
            set { notificationBarColor = value; OnPropertyChanged(); }
        }

        private string notificationBarContent;
        public string NotificationBarContent
        {
            get { return notificationBarContent; }
            set
            {
                notificationBarContent = value;
                OnPropertyChanged();
            }
        }


        private int customTemplateID;
        public int CustomTemplateID
        {
            get { return customTemplateID; }
            set
            {
                customTemplateID = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The bring to front
        /// </summary>
        private WindowState _bringToFront;

        /// <summary>
        /// Gets or sets the bring to front.
        /// </summary>
        /// <value>
        /// The bring to front.
        /// </value>
        public WindowState BringToFront
        {
            get { return _bringToFront; }
            set
            {
                if (_bringToFront == WindowState.Minimized)
                    _bringToFront = value;
                else
                {
                    if (value == WindowState.Minimized)
                        _bringToFront = value;
                    else
                    {
                        WindowState currentState = _bringToFront;
                        _bringToFront = WindowState.Minimized;
                        RaisePropertyChangedEvent("BringToFront");
                        _bringToFront = currentState;
                    }
                }
                RaisePropertyChangedEvent("BringToFront");
            }
        }

        //string _precisionFormat;
        //public string PrecisionFormat
        //{
        //    get { return _precisionFormat; }
        //    set
        //    {
        //        _precisionFormat = value;
        //        RaisePropertyChangedEvent("PrecisionFormat");
        //    }
        //}
        //double _notionalValueEditor = 4.36982155;
        //public double NotionalValueEditor
        //{
        //    get { return _notionalValueEditor; }
        //    set
        //    {
        //        _notionalValueEditor = value;
        //        RaisePropertyChangedEvent("NotionalValueEditor");
        //    }
        //}

        //public int PrecisionDigit { get; set; }

        /// <summary>
        /// Add Custom Field View Model
        /// </summary>
        public AddCustomFieldViewModel()
        {
            InitalizeMembers();
            //--Todo to set Precision Digit from preferences
            //PrecisionDigit = 4;
            //PrecisionFormat = CommonMethods.SetPrecisionStringFormat(PrecisionDigit);
        }

        private void InitalizeMembers()
        {
            SaveCustomFieldsCommand = new DelegateCommand<Window>((windowObj) => SaveCustomFieldsCommandAction(windowObj));
            FormCloseButton = new DelegateCommand<object>((parameter) => OnCloseButton(parameter));
            FormClosed = new DelegateCommand<object>((parameter) => OnFormClosed(parameter));

        }

        private async void GetCustomFieldsData(int customTempID)
        {
            try
            {
                List<OTCCustomFields> CustomFieldTemplates = await SecMasterOTCServiceApi.GetInstance().GetOTCCustomFieldsAsync(-1, customTempID);
                OTCCustomFields CustomFieldTemplate = CustomFieldTemplates.FirstOrDefault(x => x.ID == customTempID);
                CustomOTCFieldData = new CustomFieldsModel();
                if (CustomFieldTemplate != null)
                {
                    CustomFieldsModel model = new CustomFieldsModel()
                    {
                        DataType = (DataTypes)Enum.Parse(typeof(DataTypes), CustomFieldTemplate.DataType.ToString()),
                        DefaultValue = CustomFieldTemplate.DefaultValue,
                        DefaultBooleanValue = ((DataTypes)Enum.Parse(typeof(DataTypes), CustomFieldTemplate.DataType.ToString())).Equals(DataTypes.Bool) ? (BooleanValue)Enum.Parse(typeof(BooleanValue), CustomFieldTemplate.DefaultValue.ToString()) : BooleanValue.No,
                        ID = CustomFieldTemplate.ID,
                        InstrumentType = (InstrumentType)Enum.Parse(typeof(InstrumentType), CustomFieldTemplate.InstrumentType.ToString()),
                        Name = CustomFieldTemplate.Name,
                        UIOrder = CustomFieldTemplate.UIOrder,
                        IsControlsEditable = false
                    };
                    CustomOTCFieldData = model;
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private async void SaveCustomFieldsCommandAction(Window window)
        {
            try
            {
                int isSuccess = -1;
                if (CustomOTCFieldData != null && CustomOTCFieldData.Name != null && CustomOTCFieldData.DefaultValue != null && CustomOTCFieldData.Name != "")
                {
                    OTCCustomFields _OTCCustomFields = new OTCCustomFields();
                    _OTCCustomFields.ID = CustomOTCFieldData.ID;
                    _OTCCustomFields.DataType = Convert.ToString((int)(CustomOTCFieldData.DataType));
                    _OTCCustomFields.DefaultValue = CustomOTCFieldData.DataType == DataTypes.Bool ? CustomOTCFieldData.DefaultBooleanValue.ToString() : CustomOTCFieldData.DefaultValue;
                    _OTCCustomFields.InstrumentType = Convert.ToString((int)(CustomOTCFieldData.InstrumentType));
                    _OTCCustomFields.UIOrder = CustomOTCFieldData.UIOrder;
                    _OTCCustomFields.Name = CustomOTCFieldData.Name;

                    isSuccess = await SecMasterOTCServiceApi.GetInstance().SaveOTCCustomFieldsAsync(_OTCCustomFields);


                    if (isSuccess == 0)
                    {
                        TimeTickerIndicator();
                        IsVisibleLblNotificationBar = Visibility.Visible;
                        NotificationBarContent = "Data Saved.";
                    }
                    else if (isSuccess == 1)
                    {
                        TimeTickerIndicator();
                        IsVisibleLblNotificationBar = Visibility.Visible;
                        NotificationBarContent = "Failed!";
                    }



                }
                else
                {
                    TimeTickerIndicator();
                    IsVisibleLblNotificationBar = Visibility.Visible;
                    NotificationBarContent = "Name and Default Value cann't be null.";
                }

                if (OnFormCloseButtonEvent != null)
                    OnFormCloseButtonEvent(this, EventArgs.Empty);

                if (window != null && isSuccess != -1)
                {
                    window.Close();
                }


            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

        }

        /// <summary>
        /// Time Ticker Indicator
        /// </summary>
        private void TimeTickerIndicator()
        {
            var timeTick = new Timer();
            timeTick.Interval = 10000; // it will Tick in 10 seconds
            timeTick.Tick += (s, e) =>
            {
                IsVisibleLblNotificationBar = Visibility.Hidden;
                timeTick.Stop();
            };
            timeTick.Start();
        }

        internal void SetUp(int customTemplateID)
        {
            this.CustomTemplateID = customTemplateID;
            GetCustomFieldsData(customTemplateID);
        }
        /// <summary>
        /// Called when [close button].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnCloseButton(object parameter)
        {
            try
            {
                if (OnFormCloseButtonEvent != null)
                    OnFormCloseButtonEvent(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [form closed].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnFormClosed(object parameter)
        {
            try
            {
                Dispose();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>

        private void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    SaveCustomFieldsCommand = null;
                    FormCloseButton = null;
                    FormClosed = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}

