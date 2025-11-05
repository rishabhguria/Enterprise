using Infragistics.Controls.Editors;
using Infragistics.Controls.Editors.Primitives;
using Microsoft.Xaml.Behaviors;
using Prana.LogManager;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Prana.Rebalancer.CommonBehavior
{
    class CommonSymbolBehaviour : Behavior<XamComboEditor>
    {

        /// <summary>
        /// Dependency Property for SymbolList
        /// </summary>
        public readonly static DependencyProperty SymbolListProperty = DependencyProperty.Register("SymbolList", typeof(ObservableCollection<string>), typeof(CommonSymbolBehaviour), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        private bool _isOnDropdown = false;
        /// <summary>
        ///  Binded with SymbolList from view model
        /// </summary>
        public ObservableCollection<string> SymbolList
        {
            get { return (ObservableCollection<string>)GetValue(SymbolListProperty); }
            set { SetValue(SymbolListProperty, value); }
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
                AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// the method deals with preview of key down event and deals here with tab key
        /// </summary>
        /// <param name="sender">xamcomboeditor</param>
        /// <param name="e"></param>
        void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                SpecializedTextBox symboltxt = (SpecializedTextBox)Infragistics.Windows.Utilities.GetDescendantFromType(AssociatedObject, typeof(SpecializedTextBox), false);

                if (e.Key == System.Windows.Input.Key.Tab)
                {
                    if (_isOnDropdown)
                    {
                        string selectedSymbol = symboltxt.Text;

                        if (AssociatedObject.Items != null && AssociatedObject.Items.Count > 0)
                        {
                            ComboEditorItem selectedItem = null;
                            if (AssociatedObject.Items.FirstOrDefault(item => item.Control != null && item.Control.Content.ToString().StartsWith(selectedSymbol)) != null)
                            {
                                selectedItem = AssociatedObject.Items.FirstOrDefault(item => item.IsFocused && !item.IsSelected);
                            }
                            if (selectedItem != null)
                            {
                                if (!selectedItem.IsSelected)
                                    selectedItem.IsSelected = true;
                            }
                        }
                    }
                }
                else if (e.Key == Key.Up || e.Key == Key.Down)
                {
                    _isOnDropdown = true;
                }
                else
                {
                    _isOnDropdown = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
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
                AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
                base.OnDetaching();
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
    }
}
