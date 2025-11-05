using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.ServiceConnector;
using Prana.SM.OTC.View;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Prana.SM.OTC
{
    public class OTCTemplateViewModel : BindableBase, IDisposable
    {

        /// <summary>
        /// _InstrumentTypeFieldsUI
        /// </summary>
        private static AddOTCTemplateViewModel _AddOTCTemplateUI = null;

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
        /// Sec Master OTC Data
        /// </summary>
        private ObservableCollection<SecMasterOTCDataModel> _secMasterOTCData = new ObservableCollection<SecMasterOTCDataModel>();
        public ObservableCollection<SecMasterOTCDataModel> SecMasterOTCData
        {
            get { return _secMasterOTCData; }
            set
            {
                _secMasterOTCData = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command executed when View Button on grid clicked
        /// </summary>
        public DelegateCommand<object> ViewButtonClickedCommand { get; set; }

        /// <summary>
        /// Command executed when Delete Button on grid clicked
        /// </summary>
        public DelegateCommand<object> DeleteButtonClickedCommand { get; set; }

        /// <summary>
        /// Command executed when Add OTC Template Button on Window clicked
        /// </summary>
        public DelegateCommand AddOTCTemplateButtonClickedCommand { get; set; }

        /// <summary>
        /// Command executed when Refresh data Button on grid clicked
        /// </summary>
        public DelegateCommand RefreshOTCTemplateButtonClickedCommand { get; set; }

        /// <summary>
        /// OTCTemplateViewModel
        /// </summary>
        public OTCTemplateViewModel()
        {

            try
            {
                AddOTCTemplateButtonClickedCommand = new DelegateCommand(() => AddOTCTemplateButtonClickedCommandAction());
                RefreshOTCTemplateButtonClickedCommand = new DelegateCommand(() => GetOTCTemplatesAsync());
                ViewButtonClickedCommand = new DelegateCommand<object>(obj => ViewButtonClickedCommandAction(obj));
                DeleteButtonClickedCommand = new DelegateCommand<object>(obj => DeleteButtonClickedCommandAction(obj));

                FormCloseButton = new DelegateCommand<object>((parameter) => OnCloseButton(parameter));
                FormClosed = new DelegateCommand<object>((parameter) => OnFormClosed(parameter));
                GetOTCTemplatesAsync();

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
        /// Called when [close button].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnCloseButton(object parameter)
        {
            try
            {
                if (_AddOTCTemplateUI != null)
                {
                    _AddOTCTemplateUI = null;
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        /// <summary>
        /// Get OTC Templates Async
        /// </summary>
        private async void GetOTCTemplatesAsync()
        {
            try
            {
                var secMasterOTCData = await SecMasterOTCServiceApi.GetInstance().GetOTCTemplatesAsync();
                SecMasterOTCData = new ObservableCollection<SecMasterOTCDataModel>();
                foreach (var field in secMasterOTCData)
                {
                    SecMasterOTCDataModel model = new SecMasterOTCDataModel()
                    {
                        Id = field.Id,
                        Name = field.Name,
                        InstrumentType = (InstrumentType)Enum.Parse(typeof(InstrumentType), field.InstrumentType.ToString()),
                        Description = field.Description,
                        CreatedBy = field.CreatedBy == 0 ? "System" : CommonDataCache.CachedDataManager.GetInstance.GetUserText(field.CreatedBy),
                        View = field.View,
                        Delete = null,
                        IsDeleteButtonVisible = field.CreatedBy == 0 ? Visibility.Hidden : Visibility.Visible
                    };
                    SecMasterOTCData.Add(model);
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
        }

        /// <summary>
        /// Open Instrument Types view to add new Field 
        /// </summary>
        private void AddOTCTemplateButtonClickedCommandAction()
        {
            try
            {
                if (_AddOTCTemplateUI == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowAddOTCTemplateView(viewModel => dialogService.Show<AddOTCTemplateView>(this, viewModel));
                    _AddOTCTemplateUI.SetUp();
                }
                else
                    _AddOTCTemplateUI.BringToFront = WindowState.Normal;

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
        /// View Button Clicked Command Action
        /// </summary>
        /// <param name="obj"></param>
        private void ViewButtonClickedCommandAction(object obj)
        {
            try
            {
                if (_AddOTCTemplateUI == null)
                {
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowAddOTCTemplateView(viewModel => dialogService.Show<AddOTCTemplateView>(this, viewModel));
                }
                else
                {
                    _AddOTCTemplateUI.BringToFront = WindowState.Normal;
                }

                SecMasterOTCDataModel secMasterOTCData = obj as SecMasterOTCDataModel;
                _AddOTCTemplateUI.SetUp(secMasterOTCData.Id, secMasterOTCData.InstrumentType);

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
        /// Delete Button Clicked Command Action
        /// </summary>
        /// <param name="obj"></param>
        private async void DeleteButtonClickedCommandAction(object obj)
        {
            try
            {

                SecMasterOTCDataModel secMasterOTCData = obj as SecMasterOTCDataModel;
                if (secMasterOTCData.CreatedBy.Equals("System"))
                {
                    MessageBox.Show("System Generated Template can't be deleted", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (MessageBox.Show("Do you want to delete " + secMasterOTCData.Name + " template?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        var result = await SecMasterOTCServiceApi.GetInstance().DeleteOTCTemplatesAsync(secMasterOTCData.Id);
                        //var secMasterOTCData = await SecMasterOTCServiceApi.GetInstance().GetOTCTemplatesAsync();
                        SecMasterOTCData.Remove(secMasterOTCData);
                    }
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
        }

        /// <summary>
        /// Shows the show InstrumentType Fields View UI.
        /// </summary>
        /// <param name="showCalculateProrataUIViewModel">The show calculate prorata UI view model.</param>
        private void ShowAddOTCTemplateView(Action<AddOTCTemplateViewModel> showInstrumentTypeFieldsViewModel)
        {
            try
            {
                _AddOTCTemplateUI = new AddOTCTemplateViewModel();
                // _AddOTCTemplateUI.OnAddOTCTemplateEvent += _AddOTCTemplateUI_OnAddOTCTemplateEvent;
                _AddOTCTemplateUI.OnFormCloseButtonEvent += windowAddOTCTemplateView_Closed;
                showInstrumentTypeFieldsViewModel(_AddOTCTemplateUI);
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
        /// Window Add OTC TemplateView Closed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void windowAddOTCTemplateView_Closed(object sender, EventArgs e)
        {
            try
            {
                GetOTCTemplatesAsync();
                //_AddOTCTemplateUI.OnAddOTCTemplateEvent += _AddOTCTemplateUI_OnAddOTCTemplateEvent;
                if (_AddOTCTemplateUI != null)
                {
                    _AddOTCTemplateUI.OnFormCloseButtonEvent -= windowAddOTCTemplateView_Closed;
                    _AddOTCTemplateUI = null;
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
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_AddOTCTemplateUI != null)
                {
                    _AddOTCTemplateUI = null;
                }
            }
        }
    }
}
