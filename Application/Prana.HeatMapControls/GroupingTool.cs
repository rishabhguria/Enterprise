using Infragistics.Win.UltraWinGrid;
using Prana.HeatMapControls.Delegates;
using Prana.HeatMapControls.EventArguments;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.HeatMapControls
{
    public partial class GroupingTool : UserControl
    {
        private List<String> _groupings = new List<String>();

        public event GroupingChanged groupingChanged;

        private int ControlHeight = 0;

        public GroupingTool()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the grouping attributes available
        /// </summary>
        /// <param name="attributeList"></param>
        public void SetAttributes(List<String> attributeList)
        {
            try
            {
                foreach (String attribute in attributeList)
                {
                    var column = ultraGrid1.DisplayLayout.Bands[0].Columns.Add(attribute);
                    column.Hidden = true;
                }
                ultraGridColumnChooser1.SourceGrid = ultraGrid1;
                ultraGridColumnChooser1.Style = ColumnChooserStyle.HiddenColumnsOnly;
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
        /// Set up the grouping tool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupingTool_Load(object sender, EventArgs e)
        {
            try
            {
                // disabeling filtering
                this.ultraGrid1.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.Select;
                this.ultraGrid1.DisplayLayout.Override.SelectTypeCol = SelectType.None;

                ultraGrid1.DisplayLayout.Bands[0].ColHeadersVisible = false;
                ultraGrid1.DisplayLayout.Scrollbars = Scrollbars.None;

                //ultraGrid1.DisplayLayout.Appearance.BackColor = System.Drawing.Color.FromArgb(33, 44, 57);
                //ultraGrid1.DisplayLayout.Override.HeaderAppearance.BackColor = System.Drawing.Color.FromArgb(33, 44, 57);
                this.BackColor = System.Drawing.Color.FromArgb(33, 44, 57);

                ControlHeight = this.Height;
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
        /// update the grouping
        /// </summary>
        /// <param name="e"></param>
        private void ultraGrid1_BeforeSortChange(object sender, BeforeSortChangeEventArgs e)
        {
            /// <param name="sender"></param>
            try
            {
                //To check three level grouping 
                if (e.SortedColumns.Count > 3)
                {
                    e.Cancel = true;
                    return;
                }

                //Checking if the change grouping is same as before
                if (e.SortedColumns.Count == _groupings.Count)
                {
                    bool areEqual = true;
                    for (int i = 0; i < e.SortedColumns.Count; i++)
                    {
                        if (!e.SortedColumns.All[i].ToString().Equals(_groupings[i]))
                        {
                            areEqual = false;
                            break;
                        }
                    }
                    if (areEqual)
                        return;
                }

                // hide all columns so that they aer in the column chooser
                foreach (UltraGridColumn column in ultraGrid1.DisplayLayout.Bands[0].Columns.All)
                    column.Hidden = true;

                int oldSize = _groupings.Count;
                _groupings.Clear();
                foreach (UltraGridColumn column in e.SortedColumns)
                {
                    _groupings.Add(column.Key);
                    ultraGrid1.DisplayLayout.Bands[0].Columns[column.Key].Hidden = false;
                    column.Hidden = false;
                }
                if (groupingChanged != null)
                    groupingChanged(this, new GroupingChangedEventArgs() { Grouping = _groupings });
                ultraGrid1.Height = ultraGrid1.Height + (_groupings.Count - oldSize) * (20);
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
        /// show the list og attributes available for grouping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Height >= ControlHeight)
                {
                    this.Height = 30;
                    this.ultraGrid1.Visible = false;
                    this.ultraGridColumnChooser1.Visible = false;
                    this.ultraButton1.Text = "Show Grouping";
                }
                else
                {
                    this.Height = ControlHeight;
                    this.ultraGrid1.Visible = true;
                    this.ultraGridColumnChooser1.Visible = true;
                    this.ultraButton1.Text = "Hide Grouping";
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
        /// reurn the grouping
        /// </summary>
        /// <returns></returns>
        public List<String> GetSortSettings()
        {
            try
            {
                return _groupings;
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
                return null;
            }
        }

        /// <summary>
        /// Allpy a grouping on the UI
        /// </summary>
        /// <param name="keys"></param>
        public void ApplyGroup(List<String> keys)
        {
            try
            {
                foreach (String key in keys)
                {
                    var band = ultraGrid1.DisplayLayout.Bands[0];
                    var sortedColumns = band.SortedColumns;
                    sortedColumns.Add(key, false, true);
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
        /// ultraGrid1_AfterColPosChanged Event to add column in column chooser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_AfterColPosChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                ultraGrid1.DisplayLayout.Bands[0].Columns[e.ColumnHeaders[0].Caption].Hidden = true;
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
