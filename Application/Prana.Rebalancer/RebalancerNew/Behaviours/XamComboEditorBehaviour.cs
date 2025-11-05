using Infragistics.Controls.Editors;
using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using Prana.Rebalancer.RebalancerNew.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Prana.BusinessObjects.Enumerators.RebalancerNew;

namespace Prana.Rebalancer.RebalancerNew.Behaviours
{
    class XamComboEditorBehaviour : Behavior<XamComboEditor>
    {
        #region OnAttach Events

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>     
        protected override void OnAttached()
        {
            try
            {
                base.OnAttached();
                AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
                AssociatedObject.Loaded += AssociatedObject_Loaded;
                AssociatedObject.IsEnabledChanged += AssociatedObject_IsEnabledChanged;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Members

        /// <summary>
        /// The selected items property
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>), typeof(XamComboEditorBehaviour), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        /// <value>
        /// The selected items.
        /// </value>
        public ObservableCollection<object> SelectedItems
        {
            get { return GetValue(SelectedItemsProperty) as ObservableCollection<object>; }
            set { SetValue(SelectedItemsProperty, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the Loaded event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                XamComboEditor comboEditor = (sender as XamComboEditor);
                if (SelectedItems != null)
                    comboEditor.SelectedItems = SelectedItems;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
       

        /// <summary>
        /// Handles the SelectionChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                XamComboEditor comboEditor = (sender as XamComboEditor);
                if (comboEditor != null)
                {
                    bool isSellToRaiseCash = false;
                    bool isSetCashTarget = false;
                    bool isNegativeCashAllowed = false;
                    foreach (KeyValueItem item in comboEditor.SelectedItems)
                    {
                        switch (item.Key)
                        {
                            case (int)RebalancerEnums.CashSpecificRules.SetCashTarget:
                                isSetCashTarget = true;
                                break;
                            case (int)RebalancerEnums.CashSpecificRules.SellToRaiseCash:
                                isSellToRaiseCash = true;
                                break;
                            case (int)RebalancerEnums.CashSpecificRules.AllowNegativeCash:
                                isNegativeCashAllowed = true;
                                break;
                        }
                    }
                    if ((isSellToRaiseCash || isSetCashTarget) && isNegativeCashAllowed)
                    {
                        comboEditor.SelectedItems.Remove(e.AddedItems[0]);
                        ShowAlert();
                    }
                    else
                    {
                        SelectedItems = comboEditor.SelectedItems;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        private void ShowAlert()
        {
            try
            {
                MessageBox.Show(RebalancerConstants.MSG_TRADING_RULES_VALIDATION, RebalancerConstants.CAP_NIRVANA_ALERTCAPTION, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the IsEnabledChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                bool isEnabled = (bool)e.NewValue;               
                XamComboEditor comboEditor = (sender as XamComboEditor);
                if (comboEditor != null)
                {                    
                    comboEditor.SelectedItems = isEnabled ? SelectedItems : new ObservableCollection<object>();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Methods

        #region OnDetach Events

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>     
        protected override void OnDetaching()
        {
            try
            {
                AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
                AssociatedObject.Loaded -= AssociatedObject_Loaded;
                AssociatedObject.IsEnabledChanged -= AssociatedObject_IsEnabledChanged;
                base.OnDetaching();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnDetach Events
    }
}
