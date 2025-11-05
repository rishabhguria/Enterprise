using Prana.BusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Interfaces;
using Prana.LiveFeedProvider;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Prana.Rebalancer.PercentTradingTool.CustomControls
{

    /// <summary>
    /// Validates and brings the prices of an equity symbol from esignal
    /// </summary>
    [TemplatePart(Name = "PART_SymbolTextBox", Type = typeof(TextBox))]
    public class PranaSymbolWPFControl : Control, IDisposable
    {
        #region Properties

        #region Security Master

        /// <summary>
        /// The security master property
        /// </summary>
        public static readonly DependencyProperty SecurityMasterProperty =
        DependencyProperty.Register("SecurityMaster", typeof(ISecurityMasterServices), typeof(PranaSymbolWPFControl),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSecurityMasterChanged));

        /// <summary>
        /// Called when [security master changed].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnSecurityMasterChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            var control = (PranaSymbolWPFControl)element;

            if (control != null)
            {
                if (!String.IsNullOrEmpty(control.Value))
                {
                    control.SendRequestForValidation();
                }
            }
        }

        /// <summary>
        /// Gets or sets the security master.
        /// </summary>
        /// <value>
        /// The security master.
        /// </value>
        public ISecurityMasterServices SecurityMaster
        {
            get { return (ISecurityMasterServices)GetValue(SecurityMasterProperty); }
            set { SetValue(SecurityMasterProperty, value); }
        }

        #endregion

        #region IsTickerSymbology

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty IsTickerSymbologyProperty =
        DependencyProperty.Register("IsTickerSymbology", typeof(bool), typeof(PranaSymbolWPFControl),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSymbologyChanged));

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public bool IsTickerSymbology
        {
            get { return (bool)GetValue(IsTickerSymbologyProperty); }
            set
            {
                SetValue(IsTickerSymbologyProperty, value);
            }
        }

        /// <summary>
        /// Called when [value changed].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnSymbologyChanged(DependencyObject element,
        DependencyPropertyChangedEventArgs e)
        {
            var control = (PranaSymbolWPFControl)element;

            if (control != null)
            {
                control.Value = string.Empty;
            }
        }

        #endregion

        #region Value

        /// <summary>
        /// The value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(String), typeof(PranaSymbolWPFControl),
        new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, CoerceValue));

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public String Value
        {
            get { return (String)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        /// <summary>
        /// The _is symbol advised
        /// </summary>
        private bool _isSymbolAdvised;
        /// <summary>
        /// Called when [value changed].
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnValueChanged(DependencyObject element,
        DependencyPropertyChangedEventArgs e)
        {
            var control = (PranaSymbolWPFControl)element;

            if (control.TextBox != null)
            {
                control.TextBox.UndoLimit = 0;
                control.TextBox.UndoLimit = 1;
            }
            control.SendRequestForValidation();

        }

        /// <summary>
        /// Coerces the value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="baseValue">The base value.</param>
        /// <returns></returns>
        private static object CoerceValue(DependencyObject element, object baseValue)
        {
            if (baseValue != null)
            {
                var control = (PranaSymbolWPFControl)element;
                var value = baseValue.ToString();
                if (control != null && control.TextBox != null)
                {
                    control.TextBox.Text = value;
                }
                return value;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion

        #region Symbol Validated

        /// <summary>
        /// The is symbol validated property
        /// </summary>
        public static DependencyProperty IsSymbolValidatedProperty = DependencyProperty.Register("IsSymbolValidated",
        typeof(bool), typeof(PranaSymbolWPFControl),
        new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is symbol validated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is symbol validated; otherwise, <c>false</c>.
        /// </value>
        public bool IsSymbolValidated
        {
            get { return (bool)GetValue(IsSymbolValidatedProperty); }
            set { SetValue(IsSymbolValidatedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is symbol advised.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is symbol advised; otherwise, <c>false</c>.
        /// </value>
        public bool IsSymbolAdvised
        {
            get { return _isSymbolAdvised; }
            set { _isSymbolAdvised = value; }
        }

        /// <summary>
        /// The ticker symbol
        /// </summary>
        private string _tickerSymbol = string.Empty;

        #endregion

        #region Fetch Live Prices

        /// <summary>
        /// The is live feed allowed property
        /// </summary>
        public static DependencyProperty IsLiveFeedAllowedProperty = DependencyProperty.Register("IsLiveFeedAllowed",
        typeof(bool), typeof(PranaSymbolWPFControl),
        new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is live feed allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is live feed allowed; otherwise, <c>false</c>.
        /// </value>
        public bool IsLiveFeedAllowed
        {
            get { return (bool)GetValue(IsLiveFeedAllowedProperty); }
            set { SetValue(IsLiveFeedAllowedProperty, value); }
        }

        #endregion

        #endregion

        #region Fields

        private MarketDataHelper _marketDataHelperInstance = MarketDataHelper.GetInstance();

        /// <summary>
        /// The culture
        /// </summary>
        protected readonly CultureInfo Culture;

        /// <summary>
        /// The text box
        /// </summary>
        protected TextBox TextBox;

        #endregion

        #region commands

        /// <summary>
        /// The _update value string on enter command
        /// </summary>
        private readonly RoutedUICommand _updateValueStringOnEnterCommand =
        new RoutedUICommand("UpdateValueStringOnEnter", "UpdateValueStringOnEnter", typeof(PranaSymbolWPFControl));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="PranaSymbolWPFControl"/> class.
        /// </summary>
        static PranaSymbolWPFControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PranaSymbolWPFControl),
            new FrameworkPropertyMetadata(
            typeof(PranaSymbolWPFControl)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PranaSymbolWPFControl"/> class.
        /// </summary>
        public PranaSymbolWPFControl()
        {
            Culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();

            Loaded += OnLoaded;

        }

        #endregion

        #region event handlers

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            try
            {
                base.OnApplyTemplate();
                AttachToVisualTree();
                AttachCommands();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
            }
        }

        /// <summary>
        /// Texts the box on lost focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void TextBoxOnLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdateValue();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="routedEventArgs">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                InvalidateProperty(SecurityMasterProperty);
                InvalidateProperty(IsSymbolValidatedProperty);
                InvalidateProperty(ValueProperty);
                InvalidateProperty(IsTickerSymbologyProperty);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the SecMstrDataResponse event of the _secMasterClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs{ISecMasterBase}"/> instance containing the event data.</param>
        private void _secMasterClient_SecMstrDataResponse(object sender, EventArgs<SecMasterBaseObj> e)
        {
            try
            {
                if (Dispatcher.CheckAccess())
                {
                    if (e.Value != null)
                    {

                        SecMasterBaseObj iSecMasterBaseObj = e.Value;
                        IsSymbolValidated = true;
                        if (IsLiveFeedAllowed && _marketDataHelperInstance != null)
                        {
                            if (!_marketDataHelperInstance.IsDataManagerConnected())
                            {
                                RaiseErrorOccuredEvent(PTTConstants.MSG_LIVE_FEED_DISCONNECTED);
                            }
                            else
                            {
                                if (CachedDataManager.GetInstance.IsMarketDataPermissionEnabled)
                                {
                                    _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                                    _marketDataHelperInstance.OnResponse += new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                                    _marketDataHelperInstance.RequestSingleSymbol(((SecMasterBaseObj)e.Value).TickerSymbol, true);
                                }
                            }
                        }
                        _tickerSymbol = e.Value.TickerSymbol;
                        RaiseSymbolValidatedEvent(e.Value);
                    }
                }
                else
                {
                    Dispatcher.Invoke(() => _secMasterClient_SecMstrDataResponse(sender, e));
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

        /// <summary>
        /// Handles the OnResponse event of the LOne control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="arg">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void LOne_OnResponse(object sender, EventArgs<SymbolData> args)
        {
            try
            {
                if (Dispatcher.CheckAccess())
                {
                    if (args != null)
                    {
                        onL1Response(args.Value);
                    }
                }
                else
                {
                    Dispatcher.Invoke(() => LOne_OnResponse(sender, args));
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Ons the l1 response.
        /// </summary>
        /// <param name="l1Data">The l1 data.</param>
        private void onL1Response(SymbolData l1Data)
        {
            try
            {
                if (!IsSymbolAdvised && _marketDataHelperInstance != null && l1Data.Symbol == _tickerSymbol)
                {
                    IsSymbolAdvised = true;
                    Dictionary<string, string> l1DataDictionary = new Dictionary<string, string>();
                    l1DataDictionary.Add(PTTConstants.LIT_SYMBOL, l1Data.Symbol);
                    l1DataDictionary.Add(PTTConstants.LIT_LAST, Math.Round(l1Data.LastPrice, 4).ToString());
                    l1DataDictionary.Add(PTTConstants.LIT_ASK, Math.Round(l1Data.Ask, 4).ToString());
                    l1DataDictionary.Add(PTTConstants.LIT_BID, Math.Round(l1Data.Bid, 4).ToString());
                    l1DataDictionary.Add(PTTConstants.LIT_VWAP, Math.Round(l1Data.VWAP, 4).ToString());
                    RaiseSnapshotResponseEvent(l1DataDictionary);
                    _marketDataHelperInstance.OnResponse -= new EventHandler<EventArgs<SymbolData>>(LOne_OnResponse);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (SecurityMaster != null)
                {
                    SecurityMaster.SecMstrDataResponse -= _secMasterClient_SecMstrDataResponse;
                    //http://sharpfellows.com/post/Memory-Leaks-and-Dependency-Properties                   
                    SecurityMaster = null;
                }
                if (_marketDataHelperInstance != null)
                {
                    _marketDataHelperInstance.OnResponse -= LOne_OnResponse;
                    _marketDataHelperInstance.Dispose();
                    _marketDataHelperInstance = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Attaches to visual tree.
        /// </summary>
        private void AttachToVisualTree()
        {
            try
            {
                AttachTextBox();
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

        /// <summary>
        /// Attaches the text box.
        /// </summary>
        private void AttachTextBox()
        {
            try
            {
                var textBox = GetTemplateChild("PART_SymbolTextBox") as TextBox;

                if (textBox == null) return;
                TextBox = textBox;
                TextBox.LostFocus += TextBoxOnLostFocus;

                TextBox.UndoLimit = 1;
                TextBox.IsUndoEnabled = true;
                TextBox.CharacterCasing = CharacterCasing.Upper;
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

        /// <summary>
        /// Attaches the commands.
        /// </summary>
        private void AttachCommands()
        {

            try
            {
                CommandBindings.Add(new CommandBinding(_updateValueStringOnEnterCommand, (a, b) => UpdateValue()));

                CommandManager.RegisterClassInputBinding(typeof(TextBox),
                new KeyBinding(_updateValueStringOnEnterCommand, new KeyGesture(Key.Enter)));
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

        #region methods

        /// <summary>
        /// Updates the value.
        /// </summary>
        private void UpdateValue()
        {
            try
            {
                if (Value != TextBox.Text)
                {
                    Value = TextBox.Text;
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

        /// <summary>
        /// Wires the events.
        /// </summary>
        internal void WireEvents()
        {
            try
            {
                if (SecurityMaster != null)
                {
                    SecurityMaster.SecMstrDataResponse -= _secMasterClient_SecMstrDataResponse;
                    SecurityMaster.SecMstrDataResponse += _secMasterClient_SecMstrDataResponse;
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

        /// <summary>
        /// Sends the request for validation.
        /// </summary>
        private void SendRequestForValidation()
        {
            try
            {
                IsSymbolAdvised = false;
                SecMasterRequestObj reqObj = new SecMasterRequestObj();

                reqObj.AddData(Value, IsTickerSymbology ? ApplicationConstants.SymbologyCodes.TickerSymbol : ApplicationConstants.SymbologyCodes.BloombergSymbol);
                reqObj.IsSearchInLocalOnly = !CachedDataManager.GetInstance.IsMarketDataPermissionEnabled;
                reqObj.HashCode = GetHashCode();
                if (SecurityMaster != null)
                {
                    IsSymbolValidated = false;
                    RaiseClearDetailsEvent();
                    WireEvents();
                    SecurityMaster.SendRequest(reqObj);
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

        #region CustomRoutedEvents
        /// <summary>
        /// The symbol validated event
        /// </summary>
        public static readonly RoutedEvent SymbolValidatedEvent = EventManager.RegisterRoutedEvent(
                "SymbolValidated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PranaSymbolWPFControl));

        /// <summary>
        /// Occurs when [symbol validated].
        /// </summary>
        public event RoutedEventHandler SymbolValidated
        {
            add { AddHandler(SymbolValidatedEvent, value); }
            remove { RemoveHandler(SymbolValidatedEvent, value); }
        }

        /// <summary>
        /// Raises the symbol validated event.
        /// </summary>
        /// <param name="iSecMasterBaseObj">The i sec master base object.</param>
        void RaiseSymbolValidatedEvent(SecMasterBaseObj iSecMasterBaseObj)
        {
            SymbolValidateEventArgs args = new SymbolValidateEventArgs(SymbolValidatedEvent, iSecMasterBaseObj);
            RaiseEvent(args);
        }

        /// <summary>
        /// The clear details event
        /// </summary>
        public static readonly RoutedEvent ClearDetailsEvent = EventManager.RegisterRoutedEvent(
                "ClearDetails", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PranaSymbolWPFControl));

        /// <summary>
        /// Occurs when [clear details].
        /// </summary>
        public event RoutedEventHandler ClearDetails
        {
            add { AddHandler(ClearDetailsEvent, value); }
            remove { RemoveHandler(ClearDetailsEvent, value); }
        }

        /// <summary>
        /// Raises the clear details event.
        /// </summary>
        void RaiseClearDetailsEvent()
        {
            RoutedEventArgs args = new RoutedEventArgs(ClearDetailsEvent);
            RaiseEvent(args);
        }

        /// <summary>
        /// The snapshot response event
        /// </summary>
        public static readonly RoutedEvent SnapshotResponseEvent = EventManager.RegisterRoutedEvent(
                "SnapshotResponse", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PranaSymbolWPFControl));

        /// <summary>
        /// Occurs when [snapshot response].
        /// </summary>
        public event RoutedEventHandler SnapshotResponse
        {
            add { AddHandler(SnapshotResponseEvent, value); }
            remove { RemoveHandler(SnapshotResponseEvent, value); }
        }

        /// <summary>
        /// Raises the snapshot response event.
        /// </summary>
        /// <param name="l1DataDictionary">The l1 data dictionary.</param>
        void RaiseSnapshotResponseEvent(Dictionary<string, string> l1DataDictionary)
        {
            SanpshotResponseEventArgs args = new SanpshotResponseEventArgs(SnapshotResponseEvent, l1DataDictionary);
            RaiseEvent(args);
        }

        public static readonly RoutedEvent ErrorOccuredEvent = EventManager.RegisterRoutedEvent("ErrorOccured", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PranaSymbolWPFControl));

        public event RoutedEventHandler ErrorOccured
        {
            add { AddHandler(ErrorOccuredEvent, value); }
            remove { RemoveHandler(ErrorOccuredEvent, value); }
        }

        void RaiseErrorOccuredEvent(string errorMessage)
        {
            PranaSymbolErrorMessageWpfControlEventArgs args = new PranaSymbolErrorMessageWpfControlEventArgs(ErrorOccuredEvent, errorMessage);
            RaiseEvent(args);
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class SymbolValidateEventArgs : RoutedEventArgs
    {
        private readonly SecMasterBaseObj _isecMasterBase;
        public SecMasterBaseObj ISecMasterBaseObj
        {
            get { return _isecMasterBase; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolValidateEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event.</param>
        /// <param name="isecMasterBase">The isec master base.</param>
        public SymbolValidateEventArgs(RoutedEvent routedEvent, SecMasterBaseObj isecMasterBase)
            : base(routedEvent)
        {
            this._isecMasterBase = isecMasterBase;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SanpshotResponseEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The _L1 data dictionary
        /// </summary>
        private readonly Dictionary<string, string> _l1DataDictionary;
        /// <summary>
        /// Gets the l1 data dictionary.
        /// </summary>
        /// <value>
        /// The l1 data dictionary.
        /// </value>
        public Dictionary<string, string> L1DataDictionary
        {
            get { return _l1DataDictionary; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SanpshotResponseEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event.</param>
        /// <param name="l1DataDictionary">The l1 data dictionary.</param>
        public SanpshotResponseEventArgs(RoutedEvent routedEvent, Dictionary<string, string> l1DataDictionary)
            : base(routedEvent)
        {
            this._l1DataDictionary = l1DataDictionary;
        }
    }

    /// <summary>
    /// Send errors to view model
    /// </summary>
    public class PranaSymbolErrorMessageWpfControlEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// The _error message
        /// </summary>
        private string _errorMessage;
        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PranaSymbolErrorMessageWpfControlEventArgs"/> class.
        /// </summary>
        /// <param name="routedEvent">The routed event.</param>
        /// <param name="errorMessage">The error message.</param>
        public PranaSymbolErrorMessageWpfControlEventArgs(RoutedEvent routedEvent, string errorMessage)
            : base(routedEvent)
        {
            this._errorMessage = errorMessage;
        }
    }
}

