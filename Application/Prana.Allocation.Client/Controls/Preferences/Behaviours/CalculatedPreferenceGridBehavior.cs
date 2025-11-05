using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Definitions;
using Prana.LogManager;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Prana.Allocation.Client.Controls.Preferences.Behaviours
{
    public class CalculatedPreferenceGridBehavior : Behavior<XamDataGrid>
    {
        #region OnAttach Events

        /// <summary>
        /// Called when [attached].
        /// </summary>
        protected override void OnAttached()
        {
            try
            {
                base.OnAttached();
                AssociatedObject.EditModeStarted += AssociatedObject_EditModeStarted;
                AssociatedObject.MouseRightButtonDown += AssociatedObject_MouseRightButtonDown;
                AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion OnAttach Events

        #region Members

        /// <summary>
        /// The item enter edit mode property
        /// </summary>
        public static readonly DependencyProperty ItemEnterEditModeProperty = DependencyProperty.Register("ItemEnterEditMode", typeof(bool), typeof(CalculatedPreferenceGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EnterEditMode));

        /// <summary>
        /// The end edit mode property
        /// </summary>
        public static readonly DependencyProperty EndEditModeProperty = DependencyProperty.Register("EndEditMode", typeof(bool), typeof(CalculatedPreferenceGridBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, EndEditModeGrid));

        /// <summary>
        /// The selected allocation preferences property
        /// </summary>
        public static readonly DependencyProperty SelectedAllocationPreferencesProperty = DependencyProperty.Register("SelectedAllocationPreferences", typeof(object), typeof(CalculatedPreferenceGridBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [item enter edit mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [item enter edit mode]; otherwise, <c>false</c>.
        /// </value>
        public bool ItemEnterEditMode
        {
            get { return (bool)GetValue(ItemEnterEditModeProperty); }
            set { SetValue(ItemEnterEditModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [end edit mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [end edit mode]; otherwise, <c>false</c>.
        /// </value>
        public bool EndEditMode
        {
            get { return (bool)GetValue(EndEditModeProperty); }
            set { SetValue(EndEditModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected allocation preferences.
        /// </summary>
        /// <value>
        /// The selected allocation preferences.
        /// </value>
        public object SelectedAllocationPreferences
        {
            get { return GetValue(SelectedAllocationPreferencesProperty); }
            set { SetValue(SelectedAllocationPreferencesProperty, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handles the EditModeStarted event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EditModeStartedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_EditModeStarted(object sender, EditModeStartedEventArgs e)
        {
            try
            {
                CellValuePresenter cvp = CellValuePresenter.FromCell(e.Cell);
                cvp.Editor.StartEditMode();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the MouseRightButtonDown event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AssociatedObject.ExecuteCommand(DataPresenterCommands.EndEditModeAndCommitRecord);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the MouseLeftButtonDown event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                AssociatedObject.ExecuteCommand(DataPresenterCommands.EndEditModeAndCommitRecord);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Enters the edit mode.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void EnterEditMode(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            try
            {
                if (!(bool)e.NewValue)
                    return;

                CalculatedPreferenceGridBehavior gridBehavior = (CalculatedPreferenceGridBehavior)d;
                XamDataGrid preferencesGrid = gridBehavior.AssociatedObject;
                preferencesGrid.Records.ToList().ForEach(record =>
                {
                    if (record.IsDataRecord && ((DictionaryImpersonation<int, string>)((DataRecord)record).DataItem).Key.Equals(((DictionaryImpersonation<int, string>)gridBehavior.SelectedAllocationPreferences).Key))
                    {
                        record.IsActive = true;
                        preferencesGrid.ActiveDataItem = ((DataRecord)record).DataItem;
                        ((DataRecord)record).Cells["Value"].IsActive = true;
                    }

                });
                preferencesGrid.FieldSettings.AllowEdit = (bool)e.NewValue;
                preferencesGrid.BringCellIntoView(preferencesGrid.ActiveCell);
                preferencesGrid.ExecuteCommand(DataPresenterCommands.StartEditMode);
                gridBehavior.ItemEnterEditMode = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Ends the edit mode.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void EndEditModeGrid(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                    return;

                CalculatedPreferenceGridBehavior gridBehavior = (CalculatedPreferenceGridBehavior)d;
                gridBehavior.AssociatedObject.ExecuteCommand(DataPresenterCommands.EndEditModeAndCommitRecord);
                gridBehavior.AssociatedObject.FieldSettings.AllowEdit = false;
                gridBehavior.EndEditMode = false;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Methods

        #region OnDetach Events

        /// <summary>
        /// Called when [detaching].
        /// </summary>
        protected override void OnDetaching()
        {
            try
            {
                AssociatedObject.EditModeStarted -= AssociatedObject_EditModeStarted;
                AssociatedObject.MouseRightButtonDown -= AssociatedObject_MouseRightButtonDown;
                AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
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
