using Infragistics.Win.FormattedLinkLabel;
using Prana.HeatMapControls.Delegates;
using Prana.HeatMapControls.EventArguments;
using Prana.LogManager;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.HeatMapControls
{
    public partial class HeatMapControl : UserControl
    {
        public event DrilledDown drillDown;
        public event DrilledUp drillUp;

        public HeatMapControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// render the data on heat map
        /// </summary>
        /// <param name="dt"></param>
        public void SetData(DataTable dt)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker del = delegate { SetData(dt); };
                    this.BeginInvoke(del);
                }
                else
                {
                    int size = GetSquareSize(dt.Rows.Count);
                    Size preferredSize = new System.Drawing.Size(size, size);

                    Double maxval = Double.MinValue;
                    foreach (DataRow dr in dt.Rows)
                    {
                        Double val = Math.Abs(Convert.ToDouble(dr[1]));
                        maxval = Math.Max(maxval, val);
                    }

                    this.ultraPanel.ClientArea.Controls.Clear();
                    this.ultraPanel.ClientArea.SuspendLayout();

                    UltraFormattedLinkLabel tile;
                    foreach (DataRow row in dt.Rows)
                    {

                        Infragistics.Win.Misc.UltraPanel whitetile = new Infragistics.Win.Misc.UltraPanel();
                        whitetile.Appearance.BackColor = Color.White;
                        whitetile.Size = preferredSize;

                        tile = new UltraFormattedLinkLabel();
                        tile.Text = String.Format("\n{0}\n\n {1}", row[0], String.Format("{0:0.00}", row[1]));
                        tile.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
                        tile.Appearance.BorderColor = Color.LightGray;
                        tile.Tag = row[0];
                        tile.Appearance.BackColor = BackColorFromValue(Convert.ToDouble(row[1]) / maxval);

                        this.ultraPanel.ClientArea.Controls.Add(whitetile);
                        //this.ultraFlowLayoutManager1.SetPreferredSize(tile, preferredSize);
                        tile.Size = preferredSize;
                        tile.MouseEnter += tile_MouseEnter;
                        tile.MouseLeave += tile_MouseLeave;
                        tile.DoubleClick += tile_DoubleClick;
                        tile.Click += tile_Click;

                        whitetile.ClientArea.Controls.Add(tile);
                    }
                    this.ultraPanel.ClientArea.ResumeLayout();
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
        /// drill out on right click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tile_Click(object sender, EventArgs e)
        {
            try
            {
                if ((e as MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Right)
                    drillUp(this, new DrillUpEventArgs());
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
        /// raise drill down event on dbl click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tile_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                String tag = (sender as UltraFormattedLinkLabel).Tag.ToString();
                if (drillDown != null)
                    drillDown(this, new DrillDownEventArgs() { Value = tag });
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
        /// Un-highighlight the tile on mouse out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tile_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                UltraFormattedLinkLabel c = (UltraFormattedLinkLabel)sender;
                c.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
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
        /// Highlight the tile on mouse over
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tile_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                UltraFormattedLinkLabel c = (UltraFormattedLinkLabel)sender;
                c.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
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
        /// Returns the size of the tile based on the size of the Heat Map and the data. A value betvwwn 80-100
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private int GetSquareSize(int n)
        {
            try
            {
                if (n == 0)
                    return 0;
                double ratio = this.ultraPanel.Width / this.ultraPanel.Height;

                double noOfTiles = Math.Sqrt(ratio * n);

                int a = Convert.ToInt32(this.ultraPanel.Width / Math.Ceiling(noOfTiles) * 0.9);

                //if (a > 100)
                //    return 100;
                //else if (a < 80)
                //    return 80;
                return a;
            }
            catch
            {
                return 100;
            }

            //double noOfTilesWidthWise = Math.Floor(n * ratio);
            //return Convert.ToInt32(this.ultraPanel.Width / noOfTilesWidthWise);
            //double areaOfTile = area / n;
            //double sizeOfTile = Math.Floor(Math.Sqrt(areaOfTile) * 0.95);

            //return Convert.ToInt32(sizeOfTile);
        }

        /// <summary>
        /// Returns the color of the tile based on the input value
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private Color BackColorFromValue(double val)
        {
            try
            {
                int alphaAdjusted = (int)(255 * Math.Abs(val));
                if (val > 0d)
                {
                    return Color.FromArgb(alphaAdjusted, 39, 174, 96);
                }
                if (val < 0d)
                {
                    return Color.FromArgb(alphaAdjusted, 231, 76, 60);
                }

                return Color.White;
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
                return Color.White;
            }
        }

        /// <summary>
        /// Redraw the map
        /// </summary>
        public void Redraw()
        {
            //int size = GetSquareSize(this.ultraPanel.ClientArea.Controls.Count);
            //Size preferredSize = new System.Drawing.Size(size, size);
            //this.ultraPanel.ClientArea.SuspendLayout();
            //foreach (Control x in this.ultraPanel.ClientArea.Controls)
            //{
            //    this.ultraFlowLayoutManager1.SetPreferredSize((Infragistics.Win.Misc.UltraPanel)x, preferredSize);
            //}
            //this.ultraPanel.ClientArea.ResumeLayout();
        }
    }
}
