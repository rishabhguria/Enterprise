using Prana.LogManager;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Prana.Rebalancer.PercentTradingTool.CustomControls
{
    /// <summary>
    /// Interaction logic for ConnectionIndicator.xaml
    /// </summary>
    public partial class ConnectionIndicator : UserControl
    {
        /// <summary>
        /// The is active property
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty =
        DependencyProperty.Register("IsActive", typeof(bool?), typeof(ConnectionIndicator),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IsActivePropertyChanced));

        /// <summary>
        /// The color on property
        /// </summary>
        public static readonly DependencyProperty ColorOnProperty =
        DependencyProperty.Register("ColorOn", typeof(Color), typeof(ConnectionIndicator),
        new PropertyMetadata(Colors.Green, OnColorOnPropertyChanged));

        /// <summary>
        /// The color off property
        /// </summary>
        public static readonly DependencyProperty ColorOffProperty =
        DependencyProperty.Register("ColorOff", typeof(Color), typeof(ConnectionIndicator),
        new PropertyMetadata(Colors.Red, OnColorOffPropertyChanged));

        /// <summary>
        /// The color null property
        /// </summary>
        public static readonly DependencyProperty ColorNullProperty =
        DependencyProperty.Register("ColorNull", typeof(Color), typeof(ConnectionIndicator),
        new PropertyMetadata(Colors.Gray, OnColorNullPropertyChanged));

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>
        /// The is active.
        /// </value>
        public bool? IsActive
        {
            get { return (bool?)GetValue(IsActiveProperty); }
            set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color on.
        /// </summary>
        /// <value>
        /// The color on.
        /// </value>
        public Color ColorOn
        {
            get
            {
                return (Color)GetValue(ColorOnProperty);
            }
            set
            {
                SetValue(ColorOnProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color off.
        /// </summary>
        /// <value>
        /// The color off.
        /// </value>
        public Color ColorOff
        {
            get
            {
                return (Color)GetValue(ColorOffProperty);
            }
            set
            {
                SetValue(ColorOffProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the color null.
        /// </summary>
        /// <value>
        /// The color null.
        /// </value>
        public Color ColorNull
        {
            get
            {
                return (Color)GetValue(ColorNullProperty);
            }
            set
            {
                SetValue(ColorNullProperty, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionIndicator"/> class.
        /// </summary>
        public ConnectionIndicator()
        {
            try
            {
                InitializeComponent();
                if (IsActive == true)
                    backgroundColor.Color = ColorOn;
                else if (IsActive == false)
                    backgroundColor.Color = ColorOff;
                else
                    backgroundColor.Color = ColorNull;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether [is active property chanced] [the specified d].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void IsActivePropertyChanced(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ConnectionIndicator connectionIndicator = (ConnectionIndicator)d;
                if (connectionIndicator.IsActive == null)
                    connectionIndicator.backgroundColor.Color = connectionIndicator.ColorNull;
                else if (connectionIndicator.IsActive == true)
                    connectionIndicator.backgroundColor.Color = connectionIndicator.ColorOn;
                else
                    connectionIndicator.backgroundColor.Color = connectionIndicator.ColorOff;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Called when [color on property changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnColorOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ConnectionIndicator connectionIndicator = (ConnectionIndicator)d;
            connectionIndicator.ColorOn = (Color)e.NewValue;
            if (connectionIndicator.IsActive == true)
                connectionIndicator.backgroundColor.Color = connectionIndicator.ColorOn;
        }

        /// <summary>
        /// Called when [color off property changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnColorOffPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ConnectionIndicator connectionIndicator = (ConnectionIndicator)d;
                connectionIndicator.ColorOff = (Color)e.NewValue;
                if (connectionIndicator.IsActive == false)
                    connectionIndicator.backgroundColor.Color = connectionIndicator.ColorOff;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Called when [color null property changed].
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnColorNullPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                ConnectionIndicator connectionIndicator = (ConnectionIndicator)d;
                connectionIndicator.ColorOff = (Color)e.NewValue;
                if (connectionIndicator.IsActive == null)
                    connectionIndicator.backgroundColor.Color = connectionIndicator.ColorNull;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
