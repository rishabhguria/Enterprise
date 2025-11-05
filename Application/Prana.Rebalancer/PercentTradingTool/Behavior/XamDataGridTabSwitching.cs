using Infragistics.Controls.Editors;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.DataPresenter.Events;
using Microsoft.Xaml.Behaviors;
using Prana.BusinessObjects;
using Prana.LogManager;
using System;
using System.Windows;
using System.Windows.Data;

namespace Prana.Rebalancer.PercentTradingTool.Behavior
{
    /// <summary>
    /// Allows switching in cells using [TAB] key
    /// </summary>
    class XamDataGridTabSwitching : Behavior<XamDataGrid>
    {
        /// <summary>
        /// The cell type collection property
        /// </summary>
        public readonly static DependencyProperty CellTypeCollectionProperty = DependencyProperty.Register("CellTypeCollection", typeof(ObservableDictionary<string, string>), typeof(XamDataGridTabSwitching), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });
        /// <summary>
        /// Gets or sets the cell type collection.
        /// </summary>
        /// <value>
        /// The cell type collection.
        /// </value>
        public ObservableDictionary<string, string> CellTypeCollection
        {
            get { return GetValue(CellTypeCollectionProperty) as ObservableDictionary<string, string>; }
            set { SetValue(CellTypeCollectionProperty, value); }
        }

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
                AssociatedObject.CellActivating += AssociatedObject_CellActivating;
                AssociatedObject.Loaded += AssociatedObject_Loaded;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Cell cell = ((DataRecord)AssociatedObject.Records[0]).Cells[0];
                    CellValuePresenter cvp = CellValuePresenter.FromCell(cell);
                    cvp.StartEditMode();
                    XamComboEditor cb = Infragistics.Windows.Utilities.GetDescendantFromType(cvp, typeof(XamComboEditor), true) as XamComboEditor;
                    if (cb != null)
                    {
                        cb.Focus();
                    }
                }));
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Handles the CellActivating event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CellActivatingEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_CellActivating(object sender, CellActivatingEventArgs e)
        {
            try
            {
                if (this.CellTypeCollection.ContainsKey(e.Cell.Field.Name))
                {
                    string type = CellTypeCollection[e.Cell.Field.Name];
                    switch (type)
                    {
                        case "XamComboEditor":
                            CellValuePresenter cvp = CellValuePresenter.FromCell(e.Cell);
                            cvp.StartEditMode();
                            XamComboEditor cb = Infragistics.Windows.Utilities.GetDescendantFromType(cvp, typeof(XamComboEditor), true) as XamComboEditor;
                            if (cb != null)
                                cb.Focus();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

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
                AssociatedObject.CellActivating -= AssociatedObject_CellActivating;
                AssociatedObject.Loaded -= AssociatedObject_Loaded;
                base.OnDetaching();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
    }
}
