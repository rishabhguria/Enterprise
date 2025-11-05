using Infragistics.Windows.DockManager;
using Microsoft.Xaml.Behaviors;
using Prana.Allocation.Client.Constants;
using Prana.Allocation.Client.Helper;
using Prana.LogManager;
using System;
using System.IO;
using System.Windows;

namespace Prana.Allocation.Client.Forms.Behaviours
{
    public class AllocationClientDockManagerBehavior : Behavior<XamDockManager>
    {
        #region Members

        /// <summary>
        /// The save layout
        /// </summary>
        public static readonly DependencyProperty SaveLayout = DependencyProperty.Register("IsSaveLayout", typeof(bool), typeof(AllocationClientDockManagerBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SaveDockManagerLayout));

        /// <summary>
        /// The load layout
        /// </summary>
        public static readonly DependencyProperty LoadLayout = DependencyProperty.Register("IsLoadLayout", typeof(bool), typeof(AllocationClientDockManagerBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, LoadDockManagerLayout));

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is load layout.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is load layout; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoadLayout
        {
            get { return (bool)GetValue(LoadLayout); }
            set { SetValue(LoadLayout, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is save layout.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is save layout; otherwise, <c>false</c>.
        /// </value>
        public bool IsSaveLayout
        {
            get { return (bool)GetValue(SaveLayout); }
            set { SetValue(SaveLayout, value); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads the dock manager layout.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void LoadDockManagerLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                AllocationClientDockManagerBehavior gridExtender = (AllocationClientDockManagerBehavior)d;
                if (gridExtender.AssociatedObject != null)
                {
                    FileStream fs = null;
                    if (Directory.Exists(CommonAllocationMethods.GetDirectoryPath()))
                    {
                        string path = CommonAllocationMethods.GetDirectoryPath() + @"\" + CommonAllocationMethods.GetFileName(AllocationUIConstants.LAYOUT_ALLOCATION_UI) + ".xml";
                        if (File.Exists(path))
                            fs = new FileStream(CommonAllocationMethods.GetDirectoryPath() + @"\" + CommonAllocationMethods.GetFileName(AllocationUIConstants.LAYOUT_ALLOCATION_UI) + ".xml", FileMode.Open, FileAccess.Read, FileShare.Read);

                        if (fs != null)
                        {
                            string xmlString = File.ReadAllText(path);
                            if (xmlString.Contains("Version"))
                            {
                                string layoutVersion = xmlString.Substring(xmlString.IndexOf("<Version>"), xmlString.LastIndexOf("</Version>") + "</Version>".Length);
                                fs.Position = (long)(layoutVersion.Length);
                            }
                            else
                                fs.Position = 0;
                            gridExtender.AssociatedObject.LoadLayout(fs);
                        }
                        gridExtender.IsLoadLayout = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        /// <summary>
        /// Saves the dock manager layout.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void SaveDockManagerLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (!(bool)e.NewValue)
                {
                    return;
                }
                AllocationClientDockManagerBehavior gridExtender = (AllocationClientDockManagerBehavior)d;
                File.WriteAllText(CommonAllocationMethods.GetDirectoryPath() + @"\" + CommonAllocationMethods.GetFileName(AllocationUIConstants.LAYOUT_ALLOCATION_UI) + ".xml", AllocationUIConstants.LAYOUT_VERSION_ALLOCATIONGRID);
                FileStream fs = new FileStream(CommonAllocationMethods.GetDirectoryPath() + @"\" + CommonAllocationMethods.GetFileName(AllocationUIConstants.LAYOUT_ALLOCATION_UI) + ".xml", FileMode.Append, FileAccess.Write, FileShare.None);
                gridExtender.AssociatedObject.SaveLayout(fs);
                fs.Close();
                gridExtender.IsSaveLayout = false;

            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        #endregion Methods
    }
}
