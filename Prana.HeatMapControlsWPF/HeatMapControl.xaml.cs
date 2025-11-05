using Prana.Global;
using Prana.LogManager;
using System;
using System.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Prana.HeatMapControlsWPF
{
    /// <summary>
    /// Interaction logic for HeatMapControl.xaml
    /// </summary>
    public partial class HeatMapControl : UserControl
    {
        public event EventHandler<EventArgs<String>> DrillDown;
        public event EventHandler DrillUp;

        public HeatMapControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bind the data to the control
        /// </summary>
        /// <param name="dt"></param>
        public void SetData(DataTable dt)
        {
            try
            {
                //Sorting Data in descending order
                if (dt.Columns.Count != 0)
                {
                    dt.DefaultView.Sort = dt.Columns[2].ColumnName + " DESC";
                    dt = dt.DefaultView.ToTable();
                }
                if (cntrl.Dispatcher.CheckAccess())
                {
                    cntrl.ItemsSource = dt.Rows;
                }
                else
                {
                    cntrl.Dispatcher.Invoke(() => { SetData(dt); });
                }
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

        /// <summary>
        /// highlight the tile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseEnter_1(object sender, MouseEventArgs e)
        {
            try
            {
                (sender as Border).BorderBrush = Brushes.Black;
                //Added Tooltip for grid tile
                DataRow dr = (DataRow)(sender as Border).DataContext;
                ToolTip tooltip1 = new ToolTip();
                tooltip1.Content = dr.ItemArray[0].ToString() + Environment.NewLine + dr.ItemArray[1].ToString();
                (sender as Border).ToolTip = tooltip1;
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

        /// <summary>
        /// un-highlight the tile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeave_1(object sender, MouseEventArgs e)
        {
            try
            {
                (sender as Border).BorderBrush = Brushes.White;
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

        /// <summary>
        /// Drill down on double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 2)
                {
                    String tag = (sender as Grid).Tag.ToString();
                    if (DrillDown != null)
                        DrillDown(this, new EventArgs<String>(tag));
                }
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

        /// <summary>
        /// Drill up on right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseRightButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DrillUp != null)
                    DrillUp(this, new EventArgs());
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
