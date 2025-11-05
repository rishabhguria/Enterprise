using Infragistics.Windows.Editors;
using Microsoft.Xaml.Behaviors;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Prana.Allocation.Client.Controls.Allocation.Behaviours
{
    class EditTradeNumericEditorBehavior : Behavior<XamNumericEditor>
    {
        #region OnAttach Events

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached()
        {
            try
            {
                base.OnAttached();
                AssociatedObject.TextChanged += AssociatedObject_TextChanged;
                AssociatedObject.EditModeStarted += AssociatedObject_EditModeStarted;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Members

        /// <summary>
        /// The modified group property
        /// </summary>
        public static readonly DependencyProperty ModifiedGroupProperty = DependencyProperty.Register("ModifiedGroup", typeof(AllocationGroup), typeof(EditTradeNumericEditorBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });
        /// <summary>
        /// The total commission and fees property
        /// </summary>
        public static readonly DependencyProperty TotalCommissionAndFeesProperty = DependencyProperty.Register("TotalCommissionAndFees", typeof(string), typeof(EditTradeNumericEditorBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the modified group.
        /// </summary>
        /// <value>
        /// The modified group.
        /// </value>
        public AllocationGroup ModifiedGroup
        {
            get { return GetValue(ModifiedGroupProperty) as AllocationGroup; }
            set { SetValue(ModifiedGroupProperty, value); }
        }

        /// <summary>
        /// Gets or sets the total commission and fees.
        /// </summary>
        /// <value>
        /// The total commission and fees.
        /// </value>
        public string TotalCommissionAndFees
        {
            get { return GetValue(TotalCommissionAndFeesProperty).ToString(); }
            set { SetValue(TotalCommissionAndFeesProperty, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the EditModeStarted event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Infragistics.Windows.Editors.Events.EditModeStartedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_EditModeStarted(object sender, Infragistics.Windows.Editors.Events.EditModeStartedEventArgs e)
        {
            try
            {
                AssociatedObject.SelectAll();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the TextChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        private void AssociatedObject_TextChanged(object sender, RoutedPropertyChangedEventArgs<string> e)
        {
            try
            {
                XamNumericEditor selectedTextBox = (XamNumericEditor)e.Source;
                if (!selectedTextBox.Text.Any())
                    selectedTextBox.Text = "0";

                //Update Total Commission text box value 
                if (ModifiedGroup != null)
                    TotalCommissionAndFees = ModifiedGroup.TotalCommissionandFees.ToString();
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
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching()
        {
            try
            {
                base.OnAttached();
                AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
                AssociatedObject.EditModeStarted -= AssociatedObject_EditModeStarted;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnDetach Events
    }
}
