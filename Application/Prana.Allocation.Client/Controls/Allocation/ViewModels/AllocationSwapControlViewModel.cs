using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Helper;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    public class AllocationSwapControlViewModel : ViewModelBase
    {
        #region Events

        /// <summary>
        /// Occurs when [swap update click].
        /// </summary>
        public event EventHandler<EventArgs<SwapParameters>> SwapUpdateClick;

        #endregion Events

        #region Members

        /// <summary>
        /// The _notional value editor
        /// </summary>
        private double _notionalValueEditor;

        /// <summary>
        /// The _interest rate editor
        /// </summary>
        private double _interestRateEditor;

        /// <summary>
        /// The _spread editor
        /// </summary>
        private int _spreadEditor;

        /// <summary>
        /// The _first reset date editor
        /// </summary>
        private DateTime _firstResetDateEditor;

        /// <summary>
        /// The _day count editor
        /// </summary>
        private int _dayCountEditor;

        /// <summary>
        /// The _original date editor
        /// </summary>
        private DateTime _originalDateEditor;

        /// <summary>
        /// The _original cost editor
        /// </summary>
        private double _originalCostEditor;

        /// <summary>
        /// The _swap description
        /// </summary>
        private string _swapDescription;

        /// <summary>
        /// The _is swap update button
        /// </summary>
        private Visibility _isSwapUpdateButton;

        /// <summary>
        /// The _is read only
        /// </summary>
        private bool _isReadOnly;

        /// <summary>
        /// The _precision format
        /// </summary>
        private string _precisionFormat;

        /// <summary>
        /// The _enable control
        /// </summary>
        private bool _enableControl;

        /// <summary>
        /// The _button text
        /// </summary>
        private string _buttonText;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the button text.
        /// </summary>
        /// <value>
        /// The button text.
        /// </value>
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                RaisePropertyChangedEvent("ButtonText");
            }
        }

        /// <summary>
        /// Gets or sets the day count editor.
        /// </summary>
        /// <value>
        /// The day count editor.
        /// </value>
        public int DayCountEditor
        {
            get { return _dayCountEditor; }
            set
            {
                _dayCountEditor = value;
                RaisePropertyChangedEvent("DayCountEditor");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable control].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable control]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableControl
        {
            get { return _enableControl; }
            set
            {
                _enableControl = value;
                RaisePropertyChangedEvent("EnableControl");
            }
        }

        /// <summary>
        /// Gets or sets the first reset date editor.
        /// </summary>
        /// <value>
        /// The first reset date editor.
        /// </value>
        public DateTime FirstResetDateEditor
        {
            get { return _firstResetDateEditor; }
            set
            {
                _firstResetDateEditor = value;
                RaisePropertyChangedEvent("FirstResetDateEditor");
            }
        }

        /// <summary>
        /// Gets or sets the interest rate editor.
        /// </summary>
        /// <value>
        /// The interest rate editor.
        /// </value>
        public double InterestRateEditor
        {
            get { return _interestRateEditor; }
            set
            {
                _interestRateEditor = value;
                RaisePropertyChangedEvent("InterestRateEditor");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                RaisePropertyChangedEvent("IsReadOnly");
            }
        }

        /// <summary>
        /// Gets or sets the is swap update button.
        /// </summary>
        /// <value>
        /// The is swap update button.
        /// </value>
        public Visibility IsSwapUpdateButton
        {
            get { return _isSwapUpdateButton; }
            set
            {
                _isSwapUpdateButton = value;
                RaisePropertyChangedEvent("IsSwapUpdateButton");
            }
        }

        /// <summary>
        /// Gets or sets the notional value editor.
        /// </summary>
        /// <value>
        /// The notional value editor.
        /// </value>
        public double NotionalValueEditor
        {
            get { return _notionalValueEditor; }
            set
            {
                _notionalValueEditor = value;
                RaisePropertyChangedEvent("NotionalValueEditor");
            }
        }

        /// <summary>
        /// Gets or sets the original cost editor.
        /// </summary>
        /// <value>
        /// The original cost editor.
        /// </value>
        public double OriginalCostEditor
        {
            get { return _originalCostEditor; }
            set
            {
                _originalCostEditor = value;
                RaisePropertyChangedEvent("OriginalCostEditor");
            }
        }

        /// <summary>
        /// Gets or sets the original date editor.
        /// </summary>
        /// <value>
        /// The original date editor.
        /// </value>
        public DateTime OriginalDateEditor
        {
            get { return _originalDateEditor; }
            set
            {
                _originalDateEditor = value;
                RaisePropertyChangedEvent("OriginalDateEditor");
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
        /// Gets or sets the spread editor.
        /// </summary>
        /// <value>
        /// The spread editor.
        /// </value>
        public int SpreadEditor
        {
            get { return _spreadEditor; }
            set
            {
                _spreadEditor = value;
                RaisePropertyChangedEvent("SpreadEditor");
            }
        }

        /// <summary>
        /// Gets or sets the swap description.
        /// </summary>
        /// <value>
        /// The swap description.
        /// </value>
        public string SwapDescription
        {
            get { return _swapDescription; }
            set
            {
                _swapDescription = value;
                RaisePropertyChangedEvent("SwapDescription");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the clear button.
        /// </summary>
        /// <value>
        /// The clear button.
        /// </value>
        public RelayCommand<object> ClearButton { get; set; }

        /// <summary>
        /// Gets or sets the swap update.
        /// </summary>
        /// <value>
        /// The swap update.
        /// </value>
        public RelayCommand<object> SwapUpdate { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationSwapControlViewModel"/> class.
        /// </summary>
        public AllocationSwapControlViewModel()
        {
            try
            {
                SwapUpdate = new RelayCommand<object>((parameter) => OnSwapUpdate(parameter));
                ClearButton = new RelayCommand<object>((parameter) => OnClearButton(parameter));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the swap parameters.
        /// </summary>
        /// <returns></returns>
        internal SwapParameters GetSwapParameters(SwapValidate swapValidate)
        {
            SwapParameters targetSwapParams = new SwapParameters();
            try
            {
                if (ValidateValues(swapValidate))
                {
                    targetSwapParams.NotionalValue = NotionalValueEditor;
                    targetSwapParams.OrigCostBasis = OriginalCostEditor;
                    targetSwapParams.FirstResetDate = FirstResetDateEditor;
                    targetSwapParams.OrigTransDate = OriginalDateEditor;
                    targetSwapParams.DayCount = DayCountEditor;
                    targetSwapParams.SwapDescription = SwapDescription;
                    targetSwapParams.BenchMarkRate = InterestRateEditor;
                    targetSwapParams.Differential = SpreadEditor;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return targetSwapParams; ;
        }

        /// <summary>
        /// Called when [clear button].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnClearButton(object parameter)
        {
            try
            {
                SetDefaultValues();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [load swap control].
        /// </summary>
        internal void OnLoadSwapControl()
        {
            try
            {
                SetDefaultValues();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Called when [swap update].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnSwapUpdate(object parameter)
        {
            try
            {

                SwapParameters swapParams = GetSwapParameters(SwapValidate.Allocate);
                if (SwapUpdateClick != null)
                    SwapUpdateClick(this, new EventArgs<SwapParameters>(swapParams));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Sets the default values.
        /// </summary>
        internal void SetDefaultValues()
        {
            try
            {
                OriginalCostEditor = 0.0;
                OriginalDateEditor = DateTime.Now.Date;
                NotionalValueEditor = 0.0;
                DayCountEditor = 365;
                SwapDescription = String.Empty;
                InterestRateEditor = 0.0;
                SpreadEditor = 0;
                FirstResetDateEditor = DateTime.Now.Date.AddDays(1);
                ButtonText = "Book as Swap";
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
                PrecisionFormat = CommonAllocationMethods.SetPrecisionStringFormat(precisionDigit);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Sets the preview to swap UI.
        /// </summary>
        /// <param name="group">The group.</param>
        internal void SetPreviewToSwapUI(AllocationGroup group)
        {
            try
            {
                SwapParameters swapParams = new SwapParameters();
                swapParams.OrigCostBasis = group.AvgPrice;
                swapParams.OrigTransDate = group.AUECLocalDate;
                swapParams.NotionalValue = group.AvgPrice * group.Quantity;
                if (!group.IsSwapped)
                    SetUp(swapParams, false);
                else
                    SetUp(group.SwapParameters, true);
                ButtonText = @group.IsSwapped ? "Swap Update" : " Book as Swap";
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// SetUp Swap Parameter
        /// </summary>
        /// <param name="swapParams"></param>
        /// <param name="isSwap"></param>
        private void SetUp(SwapParameters swapParams, bool isSwap)
        {
            try
            {
                if (isSwap)
                {
                    DayCountEditor = swapParams.DayCount;
                    FirstResetDateEditor = swapParams.FirstResetDate;
                    OriginalDateEditor = swapParams.OrigTransDate;
                }
                else
                {
                    OriginalDateEditor = DateTime.Now.Date;
                    FirstResetDateEditor = DateTime.Now.Date.AddDays(1);
                    DayCountEditor = 365;
                }

                OriginalCostEditor = swapParams.OrigCostBasis;
                NotionalValueEditor = swapParams.NotionalValue;
                SwapDescription = swapParams.SwapDescription;
                InterestRateEditor = swapParams.BenchMarkRate;
                SpreadEditor = Convert.ToInt32(swapParams.Differential);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Validates the values.
        /// </summary>
        /// <param name="swapValidate">The swap validate.</param>
        /// <returns></returns>
        private bool ValidateValues(SwapValidate swapValidate)
        {
            bool isValidated = false;
            try
            {

                switch (swapValidate)
                {
                    case SwapValidate.Trade:
                        isValidated = ValidateValuesForTrade();
                        break;
                    case SwapValidate.Allocate:
                        isValidated = ValidateValuesForTrade();
                        break;
                    case SwapValidate.Expire:
                        isValidated = ValidateValuesForExpire();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return isValidated;
        }

        /// <summary>
        /// Validates the values for expire.
        /// </summary>
        /// <returns></returns>
        private bool ValidateValuesForExpire()
        {
            try
            {
                if (DayCountEditor < 1)
                {
                    MessageBox.Show("Enter valid Swap DayCount Convention", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION);
                    return false;
                }
                if (OriginalDateEditor == DateTime.MinValue)
                {
                    MessageBox.Show("Enter valid Swap Transaction Date", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION);
                    return false;
                }
                if (FirstResetDateEditor == DateTime.MinValue || DateTime.Parse(FirstResetDateEditor.ToString()) <= DateTime.Parse(OriginalDateEditor.ToString()))
                {
                    MessageBox.Show("Enter valid Swap Reset Date", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION);
                    return false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return true;
        }

        /// <summary>
        /// Validates the values for trade.
        /// </summary>
        /// <returns></returns>
        private bool ValidateValuesForTrade()
        {
            try
            {
                if (NotionalValueEditor < 0)
                {
                    MessageBox.Show("Enter valid Swap Notional", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION);
                    return false;
                }
                if (DayCountEditor < 1)
                {
                    MessageBox.Show("Enter valid Swap DayCount Convention", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION);
                    return false;
                }
                if (OriginalDateEditor == DateTime.MinValue)
                {
                    MessageBox.Show("Enter valid Swap Transaction Date", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION);
                    return false;
                }
                if (FirstResetDateEditor == DateTime.MinValue || DateTime.Parse(FirstResetDateEditor.ToString()) <= DateTime.Parse(OriginalDateEditor.ToString()))
                {
                    MessageBox.Show("Enter valid Swap Reset Date", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION);
                    return false;
                }
                if (OriginalCostEditor < 0)
                {
                    MessageBox.Show("Enter valid Swap CostBasis", AllocationClientConstants.ALLOCATION_MESSAGEBOX_CAPTION);
                    return false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return true;
        }

        #endregion Methods
    }
}
