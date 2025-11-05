using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;
using Infragistics.Controls.Editors;
using Nirvana.TestAutomation.Utilities;
using System.Collections;

namespace Nirvana.TestAutomation.TestExecutor
{
    class XamComboEditorSelectAllBehavior : Behavior<XamComboEditor>
    {
     
        static bool _check = false;
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (AssociatedObject.Items[0].IsSelected )
                {
                    AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;

                    if (_check == false)
                    {
                        if (AssociatedObject.Items[0].Control != null)
                            AssociatedObject.Items[0].Control.Content = CommandLineConstants.CONST_UNSELECT_ALL;
                        _check = true;
                        for (int i = 1; i < AssociatedObject.Items.Count; i++)
                        {
                            AssociatedObject.Items[i].IsSelected = true;
                        }
                    }
                    else
                    {
                        if (AssociatedObject.Items[0].Control != null)
                            AssociatedObject.Items[0].Control.Content = CommandLineConstants.CONST_SELECT_ALL;
                        for (int i = 1; i < AssociatedObject.Items.Count; i++)
                        {
                            AssociatedObject.Items[i].IsSelected = false;
                        }
                        _check = false;
                    }
                  
                    AssociatedObject.Items[0].IsSelected = false;
                    UpdateBindingSources(AssociatedObject);
                    AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
                }
            }
            catch
            {
                throw;
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
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
            base.OnDetaching();
        }

        /// <summary>
        /// Updates the binding sources.
        /// </summary>
        /// <param name="obj">The object.</param>
        private void UpdateBindingSources(DependencyObject obj)
        {
            try
            {
                foreach (DependencyProperty depProperty in EnumerateDependencyProperties(obj))
                {
                    //check whether the submitted object provides a bound property
                    //that matches the property parameters
                    BindingExpression be =
                      BindingOperations.GetBindingExpression(obj, depProperty);
                    if (be != null) be.UpdateSource();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Enumerates the dependency properties.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static IEnumerable EnumerateDependencyProperties(DependencyObject element)
        {
            LocalValueEnumerator lve = element.GetLocalValueEnumerator();

            while (lve.MoveNext())
            {
                LocalValueEntry entry = lve.Current;
                if (BindingOperations.IsDataBound(element, entry.Property))
                {
                    yield return entry.Property;
                }
            }
        }

    }
}
