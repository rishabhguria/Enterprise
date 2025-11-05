using Prana.LogManager;
using Prana.BusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Prana.Rebalancer.PercentTradingTool.Behavior
{
    public class DynamicStackPanelBehavior : Behavior<StackPanel>
    {
        public ObservableDictionary<string, string> Alerts
        {
            get { return (ObservableDictionary<string, string>)GetValue(AlertsProperty); }
            set { SetValue(AlertsProperty, value); }
        }

        public bool CanBindAlerts
        {
            get { return (bool)GetValue(CanBindAlertsProperty); }
            set { SetValue(CanBindAlertsProperty, value); }
        }

        public static readonly DependencyProperty AlertsProperty = DependencyProperty.Register("Alerts", typeof(ObservableDictionary<string, string>), typeof(DynamicStackPanelBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty CanBindAlertsProperty = DependencyProperty.Register("CanBindAlerts", typeof(bool), typeof(DynamicStackPanelBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCanBindAlertsChanged));

        private static void OnCanBindAlertsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                DynamicStackPanelBehavior alertsPanelBehavior = (DynamicStackPanelBehavior)d;
                StackPanel alertsPanel = alertsPanelBehavior.AssociatedObject;
                if (alertsPanel != null)
                {
                    alertsPanel.Orientation = Orientation.Vertical;
                    if (alertsPanelBehavior.Alerts != null && alertsPanelBehavior.Alerts.Count > 0)
                    {
                        foreach (KeyValuePair<String, String> alert in alertsPanelBehavior.Alerts)
                        {
                            GroupBox gbx = new GroupBox();
                            gbx.Header = alert.Key;
                            Expander expander = new Expander();
                            TextBlock textBlock = new TextBlock { Text = alert.Value, TextWrapping = TextWrapping.WrapWithOverflow };
                            expander.Content = textBlock;
                            if (alertsPanel.Children.Count == alertsPanelBehavior.Alerts.Count - 1)
                            {
                                expander.IsExpanded = true;
                            }
                            gbx.Content = expander;
                            alertsPanel.Children.Add(gbx);
                        }
                    }
                }
                alertsPanelBehavior.CanBindAlerts = false;
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
