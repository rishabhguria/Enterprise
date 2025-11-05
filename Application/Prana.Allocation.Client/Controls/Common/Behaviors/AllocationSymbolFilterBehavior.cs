using Infragistics.Controls.Editors;
using Infragistics.Controls.Editors.Primitives;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.ClientLibrary.DataAccess;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Prana.Allocation.Client.Controls.Common.Behaviors
{
    class AllocationSymbolFilterBehavior : Behavior<XamComboEditor>
    {
        #region Members

        /// <summary>
        /// The is focused
        /// </summary>
        private bool _hasLogicalFocus = false;

        /// <summary>
        /// Dependency Property for SymbolList
        /// </summary>
        public readonly static DependencyProperty SymbolListProperty = DependencyProperty.Register("SymbolList", typeof(ObservableCollection<string>), typeof(AllocationSymbolFilterBehavior), new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, });

        /// <summary>
        /// Dependency Property for SelectedSymbol
        /// </summary>
        public readonly static DependencyProperty SelectedSymbolProperty = DependencyProperty.Register("SelectedSymbol", typeof(object[]), typeof(AllocationSymbolFilterBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedSymbolChanged));

        /// <summary>
        /// Binded with SelectedSymbol from view model
        /// </summary>
        public object[] SelectedSymbol
        {
            get { return GetValue(SelectedSymbolProperty) as object[]; }
            set { SetValue(SelectedSymbolProperty, value); }
        }

        /// <summary>
        /// Occurs when selected symbol is changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSelectedSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                AllocationSymbolFilterBehavior behaviour = (AllocationSymbolFilterBehavior)d;
                if (e.NewValue == null && behaviour != null)
                {
                    behaviour.SymbolList.Clear();
                    behaviour.AssociatedObject.SelectedItems.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }
        /// <summary>
        ///  Binded with SymbolList from view model
        /// </summary>
        public ObservableCollection<string> SymbolList
        {
            get { return (ObservableCollection<string>)GetValue(SymbolListProperty); }
            set { SetValue(SymbolListProperty, value); }
        }

        #endregion members

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
                AssociatedObject.Loaded += AssociatedObject_Loaded;
                AssociatedObject.KeyUp += AssociatedObject_KeyUp;
                AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
                AssociatedObject.GotFocus += AssociatedObject_GotFocus;
                AssociatedObject.LostFocus += AssociatedObject_LostFocus;
                AssociatedObject.LostKeyboardFocus += AssociatedObject_LostKeyboardFocus;
                AssociatedObject.IsMouseDirectlyOverChanged += AssociatedObject_IsMouseDirectlyOverChanged;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// the method deals with the event when the element is laid out, rendered, and ready for interaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SpecializedTextBox symboltxt = (SpecializedTextBox)Infragistics.Windows.Utilities.GetDescendantFromType(AssociatedObject, typeof(SpecializedTextBox), false);
                if (symboltxt != null)
                {
                    symboltxt.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, AssociatedObject_Paste));
                }

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// This Method deals with Application Command Paste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion  OnAttach Events

        #region Methods
        /// <summary>
        /// the method deals with the event when the key is released and here deals with processing of input so that symbol is entered
        /// </summary>
        /// <param name="sender">xamcomboeditor</param>
        /// <param name="e"></param>
        void AssociatedObject_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                AssociatedObject.CustomValueEnteredAction = CustomValueEnteredActions.Allow;
                switch (e.Key)
                {
                    case Key.Enter:
                    case Key.Tab:
                    case Key.OemComma:
                        AssociatedObject.IsDropDownOpen = false;
                        break;
                }
                string enteredsymbol;
                SpecializedTextBox symboltxt = (SpecializedTextBox)Infragistics.Windows.Utilities.GetDescendantFromType(AssociatedObject, typeof(SpecializedTextBox), false);
                //this part deals with the logic that unnecessary buttons like down button don't result in the call procedure again
                if (((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Left || e.Key == Key.Right) == false)
                {
                    //AssociatedObject.IsDropDownOpen = false;
                    return;
                }
                //this part deals with logic of fetching the correct symbol to be searched      
                if (!symboltxt.Text.Contains(','))
                {
                    enteredsymbol = symboltxt.Text.Trim();
                }
                else
                {
                    int index = symboltxt.Text.LastIndexOf(',');
                    enteredsymbol = symboltxt.Text.Substring(index + 1).Trim();
                }

                // here is the logic for fetching the symbol list 
                if (enteredsymbol.Length == 1 || (!string.IsNullOrWhiteSpace(enteredsymbol) && (SymbolList == null || SymbolList.Count >= 200 || !SymbolList.Any(symbol => symbol.StartsWith(enteredsymbol)))))
                {
                    SecMasterSymbolSearchReq request = new SecMasterSymbolSearchReq(enteredsymbol, ApplicationConstants.SymbologyCodes.TickerSymbol);
                    SecMasterSymbolSearchRes symbolList = AllocationClientServiceConnector.SecurityMasterServices.InnerChannel.SearchSymbols(request);
                    if (symbolList != null)
                    {
                        HashSet<string> symbolListHashSet = new HashSet<string>();
                        List<string> selectedSymbols = new List<string>();

                        if (AssociatedObject.SelectedValues != null)
                        {
                            selectedSymbols = AssociatedObject.SelectedValues.Select(o => o.ToString()).ToList();
                            symbolListHashSet.UnionWith(selectedSymbols);
                        }
                        if (symbolList.Result != null)
                            symbolListHashSet.UnionWith(symbolList.Result);

                        SymbolList = new ObservableCollection<string>(symbolListHashSet);
                        if (selectedSymbols.Count != 0)
                        {
                            for (int i = 0, j = 0; i < AssociatedObject.Items.Count && j < selectedSymbols.Count; i++)
                            {
                                if (selectedSymbols.Contains(AssociatedObject.Items[i].Data))
                                {
                                    AssociatedObject.Items[i].IsSelected = true;
                                    j++;
                                }
                            }
                            symboltxt.Text = string.Format("{0},{1}", symboltxt.Text, enteredsymbol);
                        }

                        if (AssociatedObject.Items != null)
                        {
                            ComboEditorItem activeItem = AssociatedObject.Items.FirstOrDefault(item => item.Data.ToString().StartsWith(enteredsymbol));
                            if (activeItem != null)
                            {
                                activeItem.IsFocused = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// the method deals with preview of key down event and deals here with backspace and tab and spacekeys
        /// </summary>
        /// <param name="sender">xamcomboeditor</param>
        /// <param name="e"></param>        
        void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                SpecializedTextBox symboltxt = (SpecializedTextBox)Infragistics.Windows.Utilities.GetDescendantFromType(AssociatedObject, typeof(SpecializedTextBox), false);
                string enteredsymbol = symboltxt.Text;
                symboltxt.CaretIndex = enteredsymbol.Length;
                switch (e.Key)
                {
                    case Key.Space:
                    case Key.Down:
                    case Key.Up:
                    case Key.Right:
                    case Key.Left:
                        if (string.IsNullOrWhiteSpace(symboltxt.Text) || (symboltxt.CaretIndex < 1 || enteredsymbol[symboltxt.CaretIndex - 1] == ','))
                        {
                            e.Handled = true;
                        }
                        break;
                    case Key.Enter:
                    case Key.Tab:
                    case Key.OemComma:
                        string[] selectedList = symboltxt.Text.Split(',');
                        string selectedSymbol = selectedList[selectedList.Length - 1];

                        if (AssociatedObject.Items != null && AssociatedObject.Items.Count > 0)
                        {
                            ComboEditorItem selectedItem = null;
                            if (e.Key.Equals(Key.OemComma))
                            {
                                selectedItem = AssociatedObject.Items.FirstOrDefault(item => item.Data.ToString().Equals(selectedSymbol));
                            }
                            else if (AssociatedObject.Items.FirstOrDefault(item => item.Data.ToString().StartsWith(selectedSymbol)) != null)
                            {
                                selectedItem = AssociatedObject.Items.FirstOrDefault(item => item.IsFocused && !item.IsSelected);
                            }
                            if (selectedItem != null)
                            {
                                if (!selectedItem.IsSelected)
                                    selectedItem.IsSelected = true;
                                selectedSymbol = selectedItem.Data.ToString();

                                selectedList[selectedList.Length - 1] = selectedSymbol;
                                selectedList = selectedList.Distinct().ToArray();
                                symboltxt.Text = string.Join(",", selectedList);
                                if (!symboltxt.Text[symboltxt.Text.Length - 1].Equals(','))
                                    symboltxt.Text += ',';
                                symboltxt.CaretIndex = symboltxt.Text.Length;
                            }
                        }
                        e.Handled = true;
                        AssociatedObject.CustomValueEnteredAction = CustomValueEnteredActions.Ignore;
                        break;
                }
                if (string.IsNullOrWhiteSpace(enteredsymbol) || e.Key == Key.Back)
                {
                    AssociatedObject.IsDropDownOpen = false;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the LostFocus event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void AssociatedObject_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                _hasLogicalFocus = false;
                LogicalPhysicalFocusLost();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the LostKeyboardFocus event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyboardFocusChangedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                if (_hasLogicalFocus)
                    LogicalPhysicalFocusLost();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the GotFocus event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                _hasLogicalFocus = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Logical or physical focus lost.
        /// </summary>
        private void LogicalPhysicalFocusLost()
        {

            try
            {
                SpecializedTextBox symboltxt = (SpecializedTextBox)Infragistics.Windows.Utilities.GetDescendantFromType(AssociatedObject, typeof(SpecializedTextBox), false);
                string enteredsymbol = symboltxt.Text;
                string[] selectedList = symboltxt.Text.Split(',');
                string selectedSymbol = selectedList[selectedList.Length - 1];

                if (AssociatedObject.Items != null && AssociatedObject.Items.Count > 0)
                {
                    ComboEditorItem selectedItem = null;
                    selectedItem = AssociatedObject.Items.FirstOrDefault(item => item.Data.ToString().Equals(selectedSymbol));
                    if (selectedItem != null)
                    {
                        if (!selectedItem.IsSelected)
                            selectedItem.IsSelected = true;
                    }
                    else
                    {
                        int lastComma = enteredsymbol.LastIndexOf(',');
                        symboltxt.Text = enteredsymbol.Substring(0, lastComma + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the IsMouseDirectlyOverChanged event of the AssociatedObject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        void AssociatedObject_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(e.NewValue))
                {
                    (sender as XamComboEditor).ReleaseMouseCapture();
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion methods

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
                AssociatedObject.KeyUp -= AssociatedObject_KeyUp;
                AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
                AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
                AssociatedObject.LostKeyboardFocus -= AssociatedObject_LostKeyboardFocus;
                AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
                AssociatedObject.IsMouseDirectlyOverChanged -= AssociatedObject_IsMouseDirectlyOverChanged;
                AssociatedObject.Loaded -= AssociatedObject_Loaded;
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
