using Prana.MvvmDialogs.Properties;
using Prana.MvvmDialogs.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace Prana.MvvmDialogs
{
    /// <summary>
    /// Class containing means to register a <see cref="FrameworkElement"/> as a view for a view
    /// model when using the MVVM pattern. The view will then be used by the 
    /// <see cref="DialogService"/> when opening dialogs.
    /// </summary>
    public static class DialogServiceViews
    {
        /// <summary>
        /// The registered views.
        /// </summary>
        private static readonly List<IView> InternalViews = new List<IView>();

        #region Attached properties

        /// <summary>
        /// Attached property describing whether a <see cref="FrameworkElement"/> is acting as a
        /// view for a view model when using the MVVM pattern.
        /// </summary>
        public static readonly DependencyProperty IsRegisteredProperty =
            DependencyProperty.RegisterAttached(
                "IsRegistered",
                typeof(bool),
                typeof(DialogServiceViews),
                new PropertyMetadata(IsRegisteredChanged));

        /// <summary>
        /// Gets value describing whether <see cref="DependencyObject"/> is acting as a view for a
        /// view model when using the MVVM pattern
        /// </summary>
        public static bool GetIsRegistered(DependencyObject target)
        {
            return (bool)target.GetValue(IsRegisteredProperty);
        }

        /// <summary>
        /// Sets value describing whether <see cref="DependencyObject"/> is acting as a view for a
        /// view model when using the MVVM pattern
        /// </summary>
        public static void SetIsRegistered(DependencyObject target, bool value)
        {
            target.SetValue(IsRegisteredProperty, value);
        }

        /// <summary>
        /// Is responsible for handling <see cref="IsRegisteredProperty"/> changes, i.e.
        /// whether <see cref="FrameworkElement"/> is acting as a view for a view model when using
        /// the MVVM pattern.
        /// </summary>
        private static void IsRegisteredChanged(
            DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            // The Visual Studio Designer or Blend will run this code when setting the attached
            // property in XAML, however we wish to abort the execution since the behavior adds
            // nothing to a designer.
            if (DesignerProperties.GetIsInDesignMode(target))
                return;

            var view = target as FrameworkElement;
            if (view != null)
            {
                if ((bool)e.NewValue)
                {
                    Register(new ViewWrapper(view));
                }
                else
                {
                    Unregister(new ViewWrapper(view));
                }
            }
        }

        #endregion

        /// <summary>
        /// Gets the registered views.
        /// </summary>
        internal static IEnumerable<IView> Views
        {
            get
            {
                return InternalViews
                    .Where(view => view.IsAlive)
                    .ToArray();
            }
        }

        /// <summary>
        /// Registers specified view.
        /// </summary>
        /// <param name="view">The view to register.</param>
        /// <param name="isLateRegister">checks if this is a late register request</param>
        internal static void Register(IView view, [Optional, DefaultParameterValue(false)] bool isLateRegister)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            if (view.Source is IViewWinForms)
            {
                Form winOwner = ((IViewWinForms)view.Source).ParentWinformControl;
                if (winOwner == null)
                {
                    if (isLateRegister)
                    {
                        SpawnWindowControl(view);
                    }
                    else
                    {
                        view.Loaded += LateRegister;
                        return;
                    }
                }
                if (((IViewWinForms)view.Source).IsChildOfWinformControl)
                {
                    if (winOwner != null)
                        winOwner.Closed += winOwner_Closed;
                }
            }
            else
            {
                SpawnWindowControl(view);
            }


            PruneInternalViews();
            //If view already contains, no need to add again : Reviwed by Ishan Gandhi
            if (!InternalViews.Contains(view))
            {
                InternalViews.Add(view);
            }
        }

        private static void SpawnWindowControl(IView view)
        {
            // Get owner window
            Window wpfOwner = view.GetOwner();
            if (wpfOwner == null)
            {
                // Perform a late register when the view hasn't been loaded yet.
                // This will happen if e.g. the view is contained in a Frame.
                view.Loaded += LateRegister;
                return;
            }

            // Register for owner window closing, since we then should unregister view reference
            wpfOwner.Closed += OwnerClosed;
        }

        static void winOwner_Closed(object sender, EventArgs e)
        {
            var owner = sender as Form;
            if (owner != null)
            {
                // Find views acting within closed window
                IView[] windowViews = Views
                    .Where(view => view.Source is IViewWinForms && ReferenceEquals(((IViewWinForms)view.Source).ParentWinformControl, owner))
                    .ToArray();

                // Unregister Views in window
                foreach (IView windowView in windowViews)
                {
                    Unregister(windowView);
                }
            }
        }

        /// <summary>
        /// Clears the registered views.
        /// </summary>
        internal static void Clear()
        {
            InternalViews.Clear();
        }

        /// <summary>
        /// Unregisters specified view.
        /// </summary>
        /// <param name="view">The view to unregister.</param>
        private static void Unregister(IView view)
        {
            if (view == null)
                throw new ArgumentNullException("view");
            if (!InternalViews.Any(registeredView => ReferenceEquals(registeredView.Source, view.Source)))
                throw new ArgumentException(Resources.ViewNotRegistered.CurrentFormat(view.GetType()), "view");

            InternalViews.RemoveAll(registeredView => ReferenceEquals(registeredView.Source, view.Source));
        }

        /// <summary>
        /// Callback for late view register. It wasn't possible to do a instant register since the
        /// view wasn't at that point part of the logical nor visual tree.
        /// </summary>
        private static void LateRegister(object sender, RoutedEventArgs e)
        {
            var frameworkElement = e.Source as FrameworkElement;
            if (frameworkElement != null)
            {
                // Unregister loaded event
                frameworkElement.Loaded -= LateRegister;

                // Register the view
                var view = frameworkElement as IView;
                if (view != null)
                {
                    Register(view, true);
                }
                else
                {
                    Register(new ViewWrapper(frameworkElement), true);
                }
            }
        }

        /// <summary>
        /// Handles owner window closed. All views acting within the closed window should be
        /// unregistered.
        /// </summary>
        private static void OwnerClosed(object sender, EventArgs e)
        {
            var owner = sender as Window;
            if (owner != null)
            {
                // Find views acting within closed window
                IView[] windowViews = Views
                    .Where(view => ReferenceEquals(view.GetOwner(), owner))
                    .ToArray();

                // Unregister Views in window
                foreach (IView windowView in windowViews)
                {
                    Unregister(windowView);
                }
            }
        }

        private static void PruneInternalViews()
        {
            InternalViews.RemoveAll(reference => !reference.IsAlive);
        }
    }
}