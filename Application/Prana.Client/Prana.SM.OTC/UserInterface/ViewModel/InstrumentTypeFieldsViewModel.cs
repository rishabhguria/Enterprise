using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.ServiceConnector;
using Prana.SM.OTC.View;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prana.SM.OTC
{
    public class InstrumentTypeFieldsViewModel : BindableBase, IDisposable
    {

        public DelegateCommand AddbtnAddNewCustomFieldCommandCommand { get; set; }
        public DelegateCommand<Window> SaveCustomFieldCommandCommand { get; set; }
        public DelegateCommand<object> ViewButtonClickedCommand { get; set; }
        public DelegateCommand<object> DeleteButtonClickedCommand { get; set; }

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
        /// Occurs when [on form close button event].
        /// </summary>
        internal event EventHandler OnFormCloseButtonEvent;

        /// <summary>
        /// Occurs when [on Add Custom Fields].
        /// </summary>
        internal event EventHandler<List<CustomFieldsModel>> OnAddCustomFieldEvent;

        /// <summary>
        /// Is Show Hide Controls
        /// </summary>
        private Visibility _isShowHideControls = Visibility.Visible;
        public Visibility IsShowHideControls
        {
            get { return _isShowHideControls; }
            set
            {
                _isShowHideControls = value;
                OnPropertyChanged("IsShowHideControls");
            }
        }


        private Visibility isShowHideOnnextButton = Visibility.Visible;
        public Visibility IsShowHideOnnextButton
        {
            get { return isShowHideOnnextButton; }
            set
            {
                isShowHideOnnextButton = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Custom OTC Field Data
        /// </summary>
        private ObservableCollection<CustomFieldsModel> _customOTCFieldData = new ObservableCollection<CustomFieldsModel>();
        public ObservableCollection<CustomFieldsModel> CustomOTCFieldData
        {
            get { return _customOTCFieldData; }
            set
            {
                _customOTCFieldData = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// The bring to front
        /// </summary>
        private WindowState _bringToFront;


        private InstrumentType selectedInstrumentType;
        /// <summary>
        /// 
        /// </summary>
        public InstrumentType SelectedInstrumentType
        {
            get { return selectedInstrumentType; }
            set { selectedInstrumentType = value; OnPropertyChanged(); }
        }

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

        List<int> CustomSelectedIds = new List<int>();

        /// <summary>
        /// InstrumentTypeFieldsViewModel
        /// </summary>
        /// <param name="isShowHide"></param>
        public InstrumentTypeFieldsViewModel(bool isShowHide)
        {
            InitializeMembers();
            GetCustomFieldViewAsync();
        }

        /// <summary>
        /// SetUp already selected custom fields
        /// </summary>
        /// <param name="promotionList"></param>
        /// <param name="instrumentType"></param>
        internal void SetUp(List<int> promotionList, InstrumentType instrumentType)
        {
            try
            {
                this.IsShowHideControls = Visibility.Collapsed;
                this.SelectedInstrumentType = instrumentType;
                CustomSelectedIds = promotionList;
                GetCustomFieldViewAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Instrument Type Fields ViewModel
        /// </summary>
        public InstrumentTypeFieldsViewModel()
        {
            InitializeMembers();
            GetCustomFieldViewAsync();
        }

        /// <summary>
        /// Initialize Members
        /// </summary>
        private void InitializeMembers()
        {
            AddbtnAddNewCustomFieldCommandCommand = new DelegateCommand(() => AddbtnAddNewCustomFieldCommandAction());
            SaveCustomFieldCommandCommand = new DelegateCommand<Window>((windowObj) => SaveCustomFieldCommandCommandAction(windowObj));
            FormCloseButton = new DelegateCommand<object>((parameter) => OnCloseButton(parameter));
            FormClosed = new DelegateCommand<object>((parameter) => OnFormClosed(parameter));
            ViewButtonClickedCommand = new DelegateCommand<object>(obj => ViewButtonClickedCommandAction(obj));
            DeleteButtonClickedCommand = new DelegateCommand<object>(obj => DeleteButtonClickedCommandAction(obj));
        }

        private async void DeleteButtonClickedCommandAction(object obj)
        {
            try
            {
                CustomFieldsModel customFieldsModelData = obj as CustomFieldsModel;
                if (MessageBox.Show("Do you want to Delte?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    var isSuccess = await SecMasterOTCServiceApi.GetInstance().DeleteOTCCustomFieldsAsync(customFieldsModelData.ID);
                    CustomFieldsModel toRemove = (CustomFieldsModel)CustomOTCFieldData.Single(x => x.ID == customFieldsModelData.ID);
                    CustomOTCFieldData.Remove(toRemove);
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        AddCustomFieldViewModel windowAddCustomFieldView = null;

        /// <summary>
        /// View ButtonClicked Command Action
        /// </summary>
        /// <param name="obj"></param>
        private void ViewButtonClickedCommandAction(object obj)
        {
            try
            {
                if (obj != null)
                {
                    if (windowAddCustomFieldView == null)
                    {
                        CustomFieldsModel customFieldsModelData = obj as CustomFieldsModel;
                        DialogService dialogService = DialogService.DialogServiceInstance;
                        ShowAddOTCTemplateView(viewModel => dialogService.Show<AddCustomFieldView>(this, viewModel));
                        windowAddCustomFieldView.SetUp(customFieldsModelData.ID);
                    }
                    else
                    {
                        windowAddCustomFieldView.BringToFront = WindowState.Normal;
                    }
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
        /// Shows the show InstrumentType Fields View UI.
        /// </summary>
        /// <param name="showCalculateProrataUIViewModel">The show calculate prorata UI view model.</param>
        private void ShowAddOTCTemplateView(Action<AddCustomFieldViewModel> showInstrumentTypeFieldsViewModel)
        {
            try
            {
                windowAddCustomFieldView = new AddCustomFieldViewModel();
                windowAddCustomFieldView.OnFormCloseButtonEvent += windowAddOTCTemplateView_Closed;
                showInstrumentTypeFieldsViewModel(windowAddCustomFieldView);
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

        /// <summary>
        /// windowAddOTCTemplateView Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void windowAddOTCTemplateView_Closed(object sender, EventArgs e)
        {

            GetCustomFieldViewAsync();
            if (windowAddCustomFieldView != null)
            {
                windowAddCustomFieldView.OnFormCloseButtonEvent -= windowAddCustomFieldView_Closed;
                windowAddCustomFieldView = null;
            }

        }

        /// <summary>
        /// Save CustomField Command Command Action
        /// </summary>
        /// <param name="window"></param>
        private void SaveCustomFieldCommandCommandAction(Window window)
        {
            try
            {

                if (OnAddCustomFieldEvent != null)
                {
                    var selectedCustomFields = CustomOTCFieldData.Where(x => x.Selected).ToList();
                    OnAddCustomFieldEvent(this, selectedCustomFields);
                    if (window != null)
                    {
                        window.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }

        }


        /// <summary>
        /// Addbtn Add New CustomField CommandAction
        /// </summary>
        private void AddbtnAddNewCustomFieldCommandAction()
        {
            try
            {
                if (windowAddCustomFieldView == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowAddOTCTemplateView(viewModel => dialogService.Show<AddCustomFieldView>(this, viewModel));
                }
                else
                {
                    windowAddCustomFieldView.BringToFront = WindowState.Normal;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void windowAddCustomFieldView_Closed(object sender, EventArgs e)
        {
            try
            {
                GetCustomFieldViewAsync();
                if (windowAddCustomFieldView != null)
                {
                    windowAddCustomFieldView.OnFormCloseButtonEvent -= windowAddCustomFieldView_Closed;
                    windowAddCustomFieldView = null;
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
        /// /
        /// </summary>
        private async void GetCustomFieldViewAsync()
        {
            try
            {
                List<OTCCustomFields> customFields = await SecMasterOTCServiceApi.GetInstance().GetOTCCustomFieldsAsync((int)SelectedInstrumentType, -1);

                CustomOTCFieldData = new ObservableCollection<CustomFieldsModel>();
                foreach (var field in customFields)
                {
                    CustomFieldsModel model = new CustomFieldsModel()
                    {
                        DataType = (DataTypes)Enum.Parse(typeof(DataTypes), field.DataType.ToString()),
                        DefaultValue = field.DefaultValue,
                        DefaultBooleanValue = ((DataTypes)Enum.Parse(typeof(DataTypes), field.DataType.ToString())).Equals(DataTypes.Bool) ? (BooleanValue)Enum.Parse(typeof(BooleanValue), field.DefaultValue.ToString()) : BooleanValue.No,
                        ID = field.ID,
                        InstrumentType = (InstrumentType)Enum.Parse(typeof(InstrumentType), field.InstrumentType.ToString()),
                        Name = field.Name,
                        UIOrder = field.UIOrder,
                        Selected = false
                    };
                    CustomOTCFieldData.Add(model);
                }

                if (CustomSelectedIds != null)
                {
                    foreach (var customID in CustomSelectedIds)
                    {
                        CustomOTCFieldData.Where(w => w.ID == customID).ToList().ForEach(i => i.Selected = true);
                    }
                }
            }

            catch (Exception)
            {

                throw;
            }

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
                System.ComponentModel.CancelEventArgs e = parameter as System.ComponentModel.CancelEventArgs;

                if (windowAddCustomFieldView != null)
                {
                    windowAddCustomFieldView.OnFormCloseButtonEvent -= windowAddCustomFieldView_Closed;
                    windowAddCustomFieldView = null;
                }
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

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    AddbtnAddNewCustomFieldCommandCommand = null;
                    SaveCustomFieldCommandCommand = null;
                    FormCloseButton = null;
                    FormClosed = null;
                    if (windowAddCustomFieldView == null)
                    {
                        windowAddCustomFieldView.Dispose();
                    }
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
