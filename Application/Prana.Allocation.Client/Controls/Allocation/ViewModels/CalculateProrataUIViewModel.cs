using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Helper;
using Prana.BusinessLogic;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.LogManager;
using Prana.Utilities.DateTimeUtilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    class CalculateProrataUIViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [on form close button event].
        /// </summary>
        internal event EventHandler OnFormCloseButtonEvent;

        #endregion Events

        #region Members

        /// <summary>
        /// The _selected date
        /// </summary>
        private DateTime _selectedDate;

        /// <summary>
        /// The bring to front
        /// </summary>
        private WindowState _bringToFront;

        /// <summary>
        /// The _scheme attributes visibility
        /// </summary>
        private Visibility _schemeAttributesVisibility;

        /// <summary>
        /// The _prorata scheme name collection
        /// </summary>
        private ObservableCollection<string> _schemeNameCollection;

        /// <summary>
        /// The _prorata scheme basis collection
        /// </summary>
        private ObservableCollection<string> _schemeBasisCollection;


        /// <summary>
        /// The _selected prorata scheme name
        /// </summary>
        private string _selectedSchemeName;

        /// <summary>
        /// The _selected prorata scheme basis
        /// </summary>
        private string _selectedSchemeBasis;

        /// <summary>
        /// The _window height
        /// </summary>
        private int _windowHeight;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the selected date.
        /// </summary>
        /// <value>
        /// The selected date.
        /// </value>
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value.Date;
                RaisePropertyChangedEvent("SelectedDate");
            }
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

        /// <summary>
        /// Gets or sets the scheme attributes visibility.
        /// </summary>
        /// <value>
        /// The scheme attributes visibility.
        /// </value>
        public Visibility SchemeAttributesVisibility
        {
            get { return _schemeAttributesVisibility; }
            set
            {
                _schemeAttributesVisibility = value;
                RaisePropertyChangedEvent("SchemeAttributesVisibility");
            }
        }


        /// <summary>
        /// Gets or sets the prorata scheme name collection.
        /// </summary>
        /// <value>
        /// The prorata scheme name collection.
        /// </value>
        public ObservableCollection<string> SchemeNameCollection
        {
            get { return _schemeNameCollection; }
            set
            {
                _schemeNameCollection = value;
                RaisePropertyChangedEvent("SchemeNameCollection");
            }
        }

        /// <summary>
        /// Gets or sets the prorata scheme basis collection.
        /// </summary>
        /// <value>
        /// The prorata scheme basis collection.
        /// </value>
        public ObservableCollection<string> SchemeBasisCollection
        {
            get { return _schemeBasisCollection; }
            set
            {
                _schemeBasisCollection = value;
                RaisePropertyChangedEvent("SchemeBasisCollection");
            }
        }


        /// <summary>
        /// Gets or sets the name of the selected prorata scheme.
        /// </summary>
        /// <value>
        /// The name of the selected prorata scheme.
        /// </value>
        public string SelectedSchemeName
        {
            get { return _selectedSchemeName; }
            set
            {
                _selectedSchemeName = value;
                RaisePropertyChangedEvent("SelectedSchemeName");
            }

        }

        /// <summary>
        /// Gets or sets the selected prorata scheme basis.
        /// </summary>
        /// <value>
        /// The selected prorata scheme basis.
        /// </value>
        public string SelectedSchemeBasis
        {
            get { return _selectedSchemeBasis; }
            set
            {
                _selectedSchemeBasis = value;
                RaisePropertyChangedEvent("SelectedSchemeBasis");
            }
        }

        /// <summary>
        /// Gets or sets the height of the window.
        /// </summary>
        /// <value>
        /// The height of the window.
        /// </value>
        public int WindowHeight
        {
            get { return _windowHeight; }
            set
            {
                _windowHeight = value;
                RaisePropertyChangedEvent("WindowHeight");
            }
        }


        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the calculated prorata UI loaded.
        /// </summary>
        /// <value>
        /// The calculated prorata UI loaded.
        /// </value>
        public RelayCommand<object> CalculatedProrataUILoaded { get; set; }

        /// <summary>
        /// Gets or sets the form close button.
        /// </summary>
        /// <value>
        /// The form close button.
        /// </value>
        public RelayCommand<object> FormCloseButton { get; set; }

        /// <summary>
        /// Gets or sets the start calculate prorata.
        /// </summary>
        /// <value>
        /// The start calculate prorata.
        /// </value>
        public RelayCommand<object> StartCalculateProrata { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculateProrataUIViewModel"/> class.
        /// </summary>
        public CalculateProrataUIViewModel()
        {
            try
            {
                CalculatedProrataUILoaded = new RelayCommand<object>((parameter) => OnLoadProrataUI(parameter));
                StartCalculateProrata = new RelayCommand<object>((parameter) => StartProrataCalculation(parameter));
                FormCloseButton = new RelayCommand<object>((parameter) => OnCloseButton(parameter));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

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
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Called when [load prorata UI].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnLoadProrataUI(object parameter)
        {
            try
            {
                SelectedDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(DateTime.Now, -1, 1).Date;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Starts the prorata calculation.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object StartProrataCalculation(object parameter)
        {
            try
            {
                if (SelectedSchemeName == null || SelectedSchemeName.Trim().Equals(string.Empty))
                    MessageBox.Show(AllocationStringConstants.PRORATA_SCHEME_NAME_NOT_BLANK.Trim(), AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    AllocationClientManager.GetInstance().SaveAllocationScheme(SelectedSchemeName, SelectedDate, SelectedSchemeBasis);
                    Window prorataWindow = parameter as Window;
                    if (prorataWindow != null)
                        prorataWindow.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Sets the preferences.
        /// </summary>
        /// <param name="_allocationCompanyWisePref">The _allocation company wise preference.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void SetPreferences(AllocationCompanyWisePref allocationCompanyWisePref)
        {
            try
            {
                SchemeNameCollection = new ObservableCollection<string>(AllocationClientPreferenceManager.GetInstance.GetProrataFixedPreferenceNames());
                SchemeBasisCollection = new ObservableCollection<string>(CommonAllocationMethods.GetAllocationSchemeKeys());
                if (allocationCompanyWisePref != null)
                {
                    if (SchemeNameCollection != null && !SchemeNameCollection.Contains(allocationCompanyWisePref.SelectedProrataSchemeName))
                    {
                        SchemeNameCollection.Add(allocationCompanyWisePref.SelectedProrataSchemeName);
                    }
                    SelectedSchemeName = allocationCompanyWisePref.SelectedProrataSchemeName;
                    SelectedSchemeBasis = allocationCompanyWisePref.SelectedProrataSchemeBasis;
                    SchemeAttributesVisibility = allocationCompanyWisePref.SetSchemeGenerationAttributesProrata ? Visibility.Visible : Visibility.Collapsed;
                    WindowHeight = allocationCompanyWisePref.SetSchemeGenerationAttributesProrata ? 173 : 109;
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
        #endregion Methods
    }
}
