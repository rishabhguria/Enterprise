using Infragistics.Win;
using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolTip;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Tools.PL.Controls
{
    public partial class DynamicUserControl : UserControl
    {
        /// <summary>
        /// Event for Save Dynamic UDA
        /// </summary>
        public event EventHandler<EventArgs<DynamicUDA, string>> SaveDynamicUDA;

        /// <summary>
        /// Event to check master value is used
        /// </summary>
        public event EventHandler<EventArgs<string, string>> CheckMasterValueAssigned;

        /// <summary>
        /// List of existing colums of other UIs, e.g. symbollookup
        /// </summary>
        private List<string> _otherExistingColumns;

        /// <summary>
        /// Constructor to Initialize Component, Loading Data Table and Changing MasterValue column Style dropdown
        /// </summary>
        public DynamicUserControl()
        {
            try
            {
                InitializeComponent();
                LoadDataTable();
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
        /// DataTable for Dynamic UDA
        /// </summary>
        DataTable _dynamicUDA = new DataTable();

        /// <summary>
        /// Load Data Table for Dynamic UDA to UltraGrid
        /// </summary>
        private void LoadDataTable()
        {
            try
            {
                DataColumn tagColumn = new DataColumn();
                tagColumn.ColumnName = "Tag";
                tagColumn.Caption = "Tag";
                tagColumn.DataType = typeof(String);
                _dynamicUDA.Columns.Add(tagColumn);

                DataColumn headerCaptionColumn = new DataColumn();
                headerCaptionColumn.ColumnName = "HeaderCaption";
                headerCaptionColumn.Caption = "Header Caption";
                headerCaptionColumn.DataType = typeof(String);
                _dynamicUDA.Columns.Add(headerCaptionColumn);

                DataColumn defaultValueColumn = new DataColumn();
                defaultValueColumn.ColumnName = "DefaultValue";
                defaultValueColumn.Caption = "Default Value";
                defaultValueColumn.DataType = typeof(String);
                _dynamicUDA.Columns.Add(defaultValueColumn);

                DataColumn masterValuesColumn = new DataColumn();
                masterValuesColumn.ColumnName = "MasterValues";
                masterValuesColumn.Caption = "Master Values";
                _dynamicUDA.Columns.Add(masterValuesColumn);

                ultraGrid1.DataSource = _dynamicUDA;
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
        /// Bind UDA Data to Grid
        /// </summary>
        /// <param name="cache">Cache of dynamic UDA names</param>
        /// <param name="otherExistingColumns">List of symbol lookup column names</param>
        internal void BindGrid(Dictionary<string, DynamicUDA> cache, List<string> otherExistingColumns)
        {
            try
            {
                _otherExistingColumns = otherExistingColumns;
                List<string> sortedKeys = cache.Keys.ToList();
                sortedKeys.Sort((x, y) =>
                {
                    if (x.Contains("CustomUDA") && y.Contains("CustomUDA"))
                    {
                        int xNum = int.Parse(Regex.Match(x, @"\d+").Value);
                        int yNum = int.Parse(Regex.Match(y, @"\d+").Value);
                        return xNum.CompareTo(yNum);
                    }
                    else
                    {
                        return x.CompareTo(y);
                    }
                });
                foreach (string key in sortedKeys)
                {
                    _dynamicUDA.Rows.Add(key, cache[key].HeaderCaption, cache[key].DefaultValue, cache[key].SerializeToXML(cache[key].MasterValues));
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
        /// Ultra Grid Initialize Layout Event to set property related to ultragrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                if (e.Layout.Bands[0].Columns.Exists("Tag"))
                {
                    this.ultraGrid1.DisplayLayout.Bands[0].Columns["Tag"].CellActivation = Activation.NoEdit;
                }
                if (e.Layout.Bands[0].Columns.Exists("DefaultValue"))
                {
                    this.ultraGrid1.DisplayLayout.Bands[0].Columns["DefaultValue"].CellActivation = Activation.NoEdit;
                }
                if (e.Layout.Bands[0].Columns.Exists("HeaderCaption"))
                {
                    this.ultraGrid1.DisplayLayout.Bands[0].Columns["HeaderCaption"].CellActivation = Activation.NoEdit;
                }
                if (e.Layout.Bands[0].Columns.Exists("MasterValues"))
                {
                    this.ultraGrid1.DisplayLayout.Bands[0].Columns["MasterValues"].Hidden = true;
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
        /// Returns true if number of row in grid is greater than 0
        /// else reutrn false as there is no row to export.
        /// </summary>
        /// <returns></returns>
        internal bool CanExport()
        {
            try
            {
                if (ultraGrid1.Rows.Count > 0)
                    return true;
                else
                    return false;
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
                return false;
            }
        }

        /// <summary>
        /// Exports Dynamic UDA in grid to the folder path selected by user.
        /// </summary>
        /// <param name="folderPath"></param>
        private void btnDynamicUDAExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (CanExport())
                {
                    String folderPath = "";
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "Excel Worksheets|*.xls;|All Files|*.*";
                    dialog.Title = "Save an Excel File";

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        folderPath = dialog.FileName;
                    }
                    if (String.IsNullOrEmpty(folderPath))
                        MessageBox.Show(this, "No path selected", "Nirvana Dynamic UDA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        ultraGridDynamicUDAExcelExporter.Export(ultraGrid1, folderPath);
                        MessageBox.Show(this, "UDA Data Exported", "Nirvana Dynamic UDA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                    MessageBox.Show(this, "There is no row to export.", "Nirvana Dynamic UDA", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Show Tool Tip on Ultragrid Row for Dynamic UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_MouseEnter(object sender, UIElementEventArgs e)
        {
            try
            {
                UltraToolTipInfo infoRow;
                RowUIElement currentRow;
                StringBuilder builder = new StringBuilder();
                if (e.Element.GetAncestor(typeof(RowUIElement)) != null)
                {
                    currentRow = (RowUIElement)e.Element.GetAncestor(typeof(RowUIElement)) as RowUIElement;

                    String title = currentRow.Row.Cells["Tag"].Value.ToString();

                    builder.Append("Default Value: ");
                    if (!String.IsNullOrEmpty(currentRow.Row.Cells["DefaultValue"].Value.ToString()))
                        builder.AppendLine(currentRow.Row.Cells["DefaultValue"].Value.ToString());
                    else
                        builder.AppendLine("Undefined");

                    builder.Append("Header Caption: ");
                    builder.AppendLine(currentRow.Row.Cells["HeaderCaption"].Value.ToString());

                    if (currentRow != null)
                    {
                        infoRow = new UltraToolTipInfo(builder.ToString(), Infragistics.Win.ToolTipImage.Info, title, Infragistics.Win.DefaultableBoolean.True);
                        ultraToolTipManager1.SetUltraToolTip(ultraGrid1, infoRow);
                        ultraToolTipManager1.ShowToolTip(ultraGrid1);
                    }
                }
                else
                {
                    ultraToolTipManager1.HideToolTip();
                    ultraToolTipManager1.SetUltraToolTip(ultraGrid1, null);

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
        ///  Sets context menu on mouse uo event.
        /// If row selected then show Update Master Values
        /// else Add New UDA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                UIElement element = ultraGrid1.DisplayLayout.UIElement.ElementFromPoint(e.Location);
                if (element != null)
                {
                    UltraGridRow row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;

                    if (row == null && e.Button == MouseButtons.Right)
                    {
                        cntxtMnu.Visible = true;
                        addNewUDAToolStripMenuItem.Visible = true;
                        toolStripUpdateMasterValueMenuItem.Visible = false;
                        ultraGrid1.PerformAction(UltraGridAction.ExitEditMode);
                    }
                    else if (row != null && e.Button == MouseButtons.Right)
                    {
                        row.Activated = true;
                        addNewUDAToolStripMenuItem.Visible = true;
                        toolStripUpdateMasterValueMenuItem.Visible = true;
                        ultraGrid1.Visible = true;
                    }
                    //else if (row != null && e.Button == MouseButtons.Left)
                    //{
                    //    row.Selected = true;
                    //}
                    //else if (row == null && e.Button == MouseButtons.Left)
                    //{
                    //    ultraGrid1.PerformAction(UltraGridAction.ExitEditMode);
                    //}
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
        /// Dynamic UDA Add Control
        /// </summary>
        DynamicUDAAddControl _dynamicUDAAdControl;

        /// <summary>
        /// Context Menu to add the new row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cntxtMnu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                if (e.ClickedItem.Name.ToString().Equals("addNewUDAToolStripMenuItem"))
                {
                    Dictionary<string, string> existingCache = _dynamicUDA.AsEnumerable().ToDictionary<DataRow, string, string>(r => r["Tag"].ToString().ToLower(), r => r["HeaderCaption"].ToString().ToLower());
                    _dynamicUDAAdControl = new DynamicUDAAddControl(existingCache, _otherExistingColumns);
                    DynamicUDAAddFormOpen();
                }
                else if (e.ClickedItem.Name.ToString().Equals("toolStripUpdateMasterValueMenuItem"))
                {
                    UltraGridRow row = ultraGrid1.ActiveRow;
                    if (row != null)
                    {
                        DynamicUDA dynamicUDA = new DynamicUDA(row.Cells["Tag"].Value.ToString(), row.Cells["HeaderCaption"].Value.ToString(), row.Cells["DefaultValue"].Value.ToString(), row.Cells["MasterValues"].Value.ToString());
                        Dictionary<string, string> existingCache = _dynamicUDA.AsEnumerable().ToDictionary<DataRow, string, string>(r => r["Tag"].ToString().ToLower(), r => r["HeaderCaption"].ToString().ToLower());
                        existingCache.Remove(row.Cells["Tag"].Value.ToString().ToLower());
                        _dynamicUDAAdControl = new DynamicUDAAddControl(dynamicUDA, existingCache, _otherExistingColumns);
                        DynamicUDAAddFormOpen();
                    }
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
        /// Dynamic UDA Add Form Open
        /// </summary>
        /// <param name="frmDynamicUDAAddControl"></param>
        private void DynamicUDAAddFormOpen()
        {
            try
            {
                Form frmDynamicUDAAddControl = new Form();
                UltraPanel ultraPanel1 = new UltraPanel();
                ultraPanel1.ClientArea.SuspendLayout();
                ultraPanel1.SuspendLayout();
                ultraPanel1.Dock = DockStyle.Fill;
                ultraPanel1.Name = "ultraPanel1";
                frmDynamicUDAAddControl.Controls.Add(ultraPanel1);
                _dynamicUDAAdControl.Dock = DockStyle.Fill;
                ultraPanel1.ClientArea.Controls.Add(_dynamicUDAAdControl);
                frmDynamicUDAAddControl.ShowIcon = false;
                frmDynamicUDAAddControl.Text = "Dynamic UDA";
                frmDynamicUDAAddControl.Size = new System.Drawing.Size(330, 380);
                frmDynamicUDAAddControl.StartPosition = FormStartPosition.CenterParent;
                frmDynamicUDAAddControl.MaximumSize = frmDynamicUDAAddControl.MinimumSize = new System.Drawing.Size(330, 380);
                CustomThemeHelper.AddUltraFormManagerToDynamicForm(frmDynamicUDAAddControl);
                frmDynamicUDAAddControl.MaximizeBox = false;
                frmDynamicUDAAddControl.Load += new System.EventHandler(_frmDynamicUDAAddControl_Load);
                frmDynamicUDAAddControl.ShowInTaskbar = false;
                ultraPanel1.ClientArea.ResumeLayout(false);
                ultraPanel1.ClientArea.PerformLayout();
                ultraPanel1.ResumeLayout(false);
                _dynamicUDAAdControl.SaveDynamicUDA += _dynamicUDAAdControl_SaveDynamicUDA;
                _dynamicUDAAdControl.CheckMasterValueAssigned += _dynamicUDAAdControl_CheckMasterValueAssigned;
                frmDynamicUDAAddControl.ShowDialog(this.FindForm());
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
        /// Check Master Value is used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _dynamicUDAAdControl_CheckMasterValueAssigned(object sender, EventArgs<string, string> e)
        {
            try
            {
                if (CheckMasterValueAssigned != null)
                    CheckMasterValueAssigned(this, e);
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
        /// Event Raise to save the Dynamic UDA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _dynamicUDAAdControl_SaveDynamicUDA(object sender, EventArgs<DynamicUDA, string> e)
        {
            try
            {
                if (SaveDynamicUDA != null)
                    SaveDynamicUDA(this, e);
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
        /// Form Load to Apply the Theme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _frmDynamicUDAAddControl_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_SYMBOL_LOOKUP);
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
        /// Update Dynamic UDA Grid
        /// </summary>
        /// <param name="dynamicUDA"></param>
        internal void UpdateDynamicUDAGrid(DynamicUDA dynamicUDA)
        {
            try
            {
                List<DataRow> row = _dynamicUDA.AsEnumerable().Where(x => dynamicUDA.Tag == x.Field<String>("Tag")).ToList();

                if (row != null && row.Count == 1)
                {
                    row[0]["Tag"] = dynamicUDA.Tag;
                    row[0]["HeaderCaption"] = dynamicUDA.HeaderCaption;
                    row[0]["DefaultValue"] = string.IsNullOrWhiteSpace(dynamicUDA.DefaultValue) ? "Undefined" : dynamicUDA.DefaultValue;
                    row[0]["MasterValues"] = dynamicUDA.SerializeToXML(dynamicUDA.MasterValues);
                }
                else
                    _dynamicUDA.Rows.Add(dynamicUDA.Tag, dynamicUDA.HeaderCaption, dynamicUDA.DefaultValue, dynamicUDA.SerializeToXML(dynamicUDA.MasterValues));
                MessageBox.Show(this, "Dynamic UDA Saved", "Nirvana", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Delete Master Value user choice
        /// </summary>
        internal void DeleteListViewMasterValue(bool result)
        {
            try
            {
                _dynamicUDAAdControl.DeleteListViewMasterValue(result);
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
        /// Doublick Dynamic UDA Add form Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraGrid1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                UIElement element = ultraGrid1.DisplayLayout.UIElement.ElementFromPoint(e.Location);
                if (element != null)
                {
                    UltraGridRow row = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;

                    if (row != null)
                    {
                        DynamicUDA dynamicUDA = new DynamicUDA(row.Cells["Tag"].Value.ToString(), row.Cells["HeaderCaption"].Value.ToString(), row.Cells["DefaultValue"].Value.ToString(), row.Cells["MasterValues"].Value.ToString());
                        Dictionary<string, string> existingCache = _dynamicUDA.AsEnumerable().ToDictionary<DataRow, string, string>(r => r["Tag"].ToString().ToLower(), r => r["HeaderCaption"].ToString().ToLower());
                        existingCache.Remove(row.Cells["Tag"].Value.ToString().ToLower());
                        _dynamicUDAAdControl = new DynamicUDAAddControl(dynamicUDA, existingCache, _otherExistingColumns);
                        DynamicUDAAddFormOpen();
                    }
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
    }
}