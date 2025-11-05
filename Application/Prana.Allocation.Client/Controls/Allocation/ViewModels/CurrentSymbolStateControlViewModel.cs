using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects.Classes.Allocation;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    public class CurrentSymbolStateControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [update current state for symbols].
        /// </summary>
        public event EventHandler<EventArgs<List<string>>> UpdateCurrentStateForSymbols;

        #endregion Events

        #region Members

        /// <summary>
        /// The _current state dictionary
        /// </summary>
        private ObservableCollection<CurrentSymbolState> _currentSymbolStateCollection;

        /// <summary>
        /// The _symbol with no state collection
        /// </summary>
        private ObservableCollection<CurrentSymbolState> _symbolWithNoStateCollection;

        /// <summary>
        /// The _precision format
        /// </summary>
        private string _precisionFormat;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the current state dictionary.
        /// </summary>
        /// <value>
        /// The current state dictionary.
        /// </value>
        public ObservableCollection<CurrentSymbolState> CurrentSymbolStateCollection
        {
            get { return _currentSymbolStateCollection; }
            set
            {
                _currentSymbolStateCollection = value;
                RaisePropertyChangedEvent("CurrentSymbolStateCollection");
            }
        }

        /// <summary>
        /// Gets or sets the precision format.
        /// </summary>
        /// <value>
        /// The precision format.
        /// </value>
        public string PrecisionFormat
        {
            get { return _precisionFormat; }
            set
            {
                _precisionFormat = value;
                RaisePropertyChangedEvent("PrecisionFormat");
            }
        }

        /// <summary>
        /// Gets or sets the symbol with no state collection.
        /// </summary>
        /// <value>
        /// The symbol with no state collection.
        /// </value>
        public ObservableCollection<CurrentSymbolState> SymbolWithNoStateCollection
        {
            get { return _symbolWithNoStateCollection; }
            set
            {
                _symbolWithNoStateCollection = value;
                RaisePropertyChangedEvent("SymbolWithNoStateCollection");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the start calculate prorata.
        /// </summary>
        /// <value>
        /// The start calculate prorata.
        /// </value>
        public RelayCommand<object> GetStateCommand { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSymbolStateControlViewModel"/> class.
        /// </summary>
        public CurrentSymbolStateControlViewModel()
        {
            try
            {
                GetStateCommand = new RelayCommand<object>((parameter) => GetState(parameter));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the state.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object GetState(object parameter)
        {
            try
            {
                List<string> uniqueSymbols = new List<string>();
                if (UpdateCurrentStateForSymbols != null)
                    UpdateCurrentStateForSymbols(this, new EventArgs<List<string>>(uniqueSymbols));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [load current symbol state control].
        /// </summary>
        internal void OnLoadCurrentSymbolStateControl()
        {
            try
            {
                CurrentSymbolStateCollection = new ObservableCollection<CurrentSymbolState>();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the preferences.
        /// </summary>
        /// <param name="precisionDigit">The precision digit.</param>
        internal void SetPreferences(int precisionDigit)
        {
            try
            {
                //set precision format
                PrecisionFormat = CommonAllocationMethods.SetPrecisionFormat(precisionDigit);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Updates the state of the current.
        /// </summary>
        /// <param name="currentStateDictionary">The current state dictionary.</param>
        internal void UpdateCurrentState(List<CurrentSymbolState> currentStateCollection)
        {
            try
            {
                Dispatcher.CurrentDispatcher.Invoke(new Action(() =>
                {
                    if (CurrentSymbolStateCollection != null)
                        CurrentSymbolStateCollection.Clear();
                    if (SymbolWithNoStateCollection != null)
                        SymbolWithNoStateCollection.Clear();
                    if (currentStateCollection != null && currentStateCollection.Count > 0)
                    {
                        CurrentSymbolStateCollection = new ObservableCollection<CurrentSymbolState>(currentStateCollection.Where(x => x.Notional != 0 || x.Quantity != 0));
                        SymbolWithNoStateCollection = new ObservableCollection<CurrentSymbolState>(currentStateCollection.Where(x => x.Notional == 0 && x.Quantity == 0));
                    }
                }));
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Methods
    }
}
