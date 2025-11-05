using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.MvvmDialogs;
using Prana.SM.OTC.View;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Prana.SM.OTC
{
    public class ShowCustomTemplateViewModel : BindableBase, IDisposable
    {
        /// <summary>
        /// _InstrumentTypeFieldsUI
        /// </summary>
        private static InstrumentTypeFieldsViewModel _InstrumentTypeFieldsUI = null;

        /// <summary>
        /// btn Add Additional Fields Command
        /// </summary>
        public DelegateCommand btnAddAdditionalFieldsCommand { get; set; }
        public DelegateCommand btnDeleteCustomFieldsCommand { get; set; }

        /// <summary>
        /// _customFields
        /// </summary>
        private ObservableCollection<CustomFieldsModel> _customFields = new ObservableCollection<CustomFieldsModel>();

        private ObservableCollection<CustomFieldsModel> _customFieldsTemp = new ObservableCollection<CustomFieldsModel>();

        private InstrumentType selectedInstrumentType = InstrumentType.EquitySwap;

        public InstrumentType SelectedInstrumentType
        {
            get { return selectedInstrumentType; }
            set { selectedInstrumentType = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// Custom Fields
        /// </summary>
        public ObservableCollection<CustomFieldsModel> CustomFields
        {
            get { return _customFields; }
            set
            {
                _customFields = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// ShowCustomTemplateViewModel
        /// </summary>
        public ShowCustomTemplateViewModel()
        {
            btnAddAdditionalFieldsCommand = new DelegateCommand(() => InstrumentTypesFieldsAction());
            btnDeleteCustomFieldsCommand = new DelegateCommand(() => DeleteCustomFieldsAction());
        }

        private void DeleteCustomFieldsAction()
        {
            try
            {
                if (CustomFields.Count > 0)
                {
                    if (MessageBox.Show("Do you want to delete selected fields?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        var toRemove = CustomFields.Where(x => x.Selected == true).ToList();
                        foreach (var item in toRemove)
                            CustomFields.Remove(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Open Instrument Types view to add new Field 
        /// </summary>
        private void InstrumentTypesFieldsAction()
        {

            try
            {
                if (_InstrumentTypeFieldsUI == null)
                {
                    var savedCustomFields = CustomFields.Select(x => x.ID).ToList();
                    DialogService dialogService = DialogService.DialogServiceInstance;
                    ShowInstrumentTypesFieldsView(viewModel => dialogService.Show<InstrumentTypesFieldsView>(this, viewModel));
                    _InstrumentTypeFieldsUI.SetUp(savedCustomFields, SelectedInstrumentType);
                }
                else
                    _InstrumentTypeFieldsUI.BringToFront = WindowState.Normal;
                //MessageBox.Show("Prorata Form is Already Opened", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }

        }

        /// <summary>
        /// Shows the show InstrumentType Fields View UI.
        /// </summary>
        /// <param name="showCalculateProrataUIViewModel">The show calculate prorata UI view model.</param>
        private void ShowInstrumentTypesFieldsView(Action<InstrumentTypeFieldsViewModel> showInstrumentTypeFieldsViewModel)
        {
            try
            {

                _InstrumentTypeFieldsUI = new InstrumentTypeFieldsViewModel();
                _InstrumentTypeFieldsUI.OnAddCustomFieldEvent += InstrumentTypeFieldsUI_OnAddCustomFieldEvent;
                _InstrumentTypeFieldsUI.OnFormCloseButtonEvent += InstrumentTypeFieldsUI_OnFormCloseButtonEvent;
                showInstrumentTypeFieldsViewModel(_InstrumentTypeFieldsUI);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// On Add Custom Field Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InstrumentTypeFieldsUI_OnAddCustomFieldEvent(object sender, List<CustomFieldsModel> e)
        {
            try
            {
                _customFieldsTemp = new ObservableCollection<CustomFieldsModel>(CustomFields);
                CustomFields = new ObservableCollection<CustomFieldsModel>();

                var newCustomFields = e.Where(x => !_customFieldsTemp.Select(y => y.ID).Contains(x.ID)).ToList();
                _customFieldsTemp.AddRange(newCustomFields);
                var toRemoveCustomFields = _customFieldsTemp.Where(x => !e.Select(y => y.ID).Contains(x.ID)).ToList();
                foreach (var item in toRemoveCustomFields)
                {
                    _customFieldsTemp.Remove(item);
                }

                _customFieldsTemp.Join(e, (customFields) => customFields.ID, (ToUpdateUIorder) => ToUpdateUIorder.ID, (customFields, ToUpdateUIorder) =>
                    {
                        customFields.UIOrder = ToUpdateUIorder.UIOrder;
                        return customFields;
                    }).ToList();

                CustomFields = new ObservableCollection<CustomFieldsModel>(_customFieldsTemp.OrderBy(i => i.UIOrder));
                _customFieldsTemp = null;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the OnFormCloseButtonEvent event of the editAllocPrefUI control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void InstrumentTypeFieldsUI_OnFormCloseButtonEvent(object sender, EventArgs e)
        {
            try
            {
                if (_InstrumentTypeFieldsUI != null)
                {
                    _InstrumentTypeFieldsUI.OnAddCustomFieldEvent -= InstrumentTypeFieldsUI_OnAddCustomFieldEvent;
                    _InstrumentTypeFieldsUI.OnFormCloseButtonEvent -= InstrumentTypeFieldsUI_OnFormCloseButtonEvent;

                    _InstrumentTypeFieldsUI = null;
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_InstrumentTypeFieldsUI != null)
                    {
                        _InstrumentTypeFieldsUI.OnAddCustomFieldEvent -= InstrumentTypeFieldsUI_OnAddCustomFieldEvent;
                        _InstrumentTypeFieldsUI.OnFormCloseButtonEvent -= InstrumentTypeFieldsUI_OnFormCloseButtonEvent;
                        _InstrumentTypeFieldsUI = null;

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }

        }

        /// <summary>
        /// Set Data for Custom Fields
        /// </summary>
        /// <param name="list"></param>
        internal void SetData(List<OTCCustomFields> list)
        {
            try
            {
                ObservableCollection<CustomFieldsModel> customFieldsData = new ObservableCollection<CustomFieldsModel>();
                if (list != null)
                {
                    foreach (var field in list)
                    {

                        CustomFieldsModel model = new CustomFieldsModel()
                        {
                            DataType = (DataTypes)Enum.Parse(typeof(DataTypes), field.DataType.ToString()),
                            DefaultValue = field.DefaultValue,
                            DefaultBooleanValue = ((DataTypes)Enum.Parse(typeof(DataTypes), field.DataType.ToString())).Equals(DataTypes.Bool) ? (BooleanValue)Enum.Parse(typeof(BooleanValue), field.DefaultValue.ToString()) : BooleanValue.No,
                            ID = field.ID,
                            InstrumentType = (InstrumentType)Enum.Parse(typeof(InstrumentType), field.InstrumentType.ToString()),
                            Name = field.Name,
                            UIOrder = field.UIOrder
                        };
                        customFieldsData.Add(model);
                    }
                    CustomFields = customFieldsData;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
