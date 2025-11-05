using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Prana.Utilities.UI.UIUtilities
{
    public partial class CustomizableCardGrid : UserControl
    {
        #region private-variables
        private DataTable _bindedTable;
        private DataTable _dtSummPrint;
        private CustomColumnChooser _customColumnChooserDialog = new CustomColumnChooser();
        private int NO_OF_COLS;
        #endregion

        #region constructor & initialization
        public CustomizableCardGrid()
        {
            InitializeComponent();
            ShowCustomColumnChooserDialog(true);
            btnColumnChooser.Enabled = true;
            ultraGrid1.DisplayLayout.Override.AllowColMoving = AllowColMoving.WithinBand;
            ultraGrid1.DisplayLayout.Override.HeaderAppearance.Cursor = Cursors.Arrow;
            ultraGrid1.DisplayLayout.Override.RowSizing = RowSizing.Fixed;
            ultraGrid1.DisplayLayout.Override.AllowColSizing = AllowColSizing.None;
        }

        public void Setup()
        {
            try
            {
                _dtSummPrint = GetSummaryPrintDataTable();
                NO_OF_COLS = int.Parse(ConfigurationHelper.Instance.GetAppSettingValueByKey("MaxNumberOfColumnsPMDashboard"));
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region public-methods

        public void AllowEdit()
        {
            try
            {
                ShowCustomColumnChooserDialog(true);
                btnColumnChooser.Enabled = true;
                ultraGrid1.DisplayLayout.Override.AllowColMoving = AllowColMoving.WithinBand;
                ultraGrid1.DisplayLayout.Override.HeaderAppearance.Cursor = Cursors.SizeAll;
                ultraGrid1.DisplayLayout.Override.RowSizing = RowSizing.Free;
                ultraGrid1.DisplayLayout.Override.AllowColSizing = AllowColSizing.Free;
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

        public void AssignData(DataTable value)
        {
            try
            {
                if (_bindedTable != null && _bindedTable.Rows.Count > 0 && value != null && value.Rows.Count > 0)
                {
                    ultraGrid1.BeginUpdate();
                    ultraGrid1.SuspendRowSynchronization();
                    foreach (DataColumn col in value.Columns)
                    {
                        _bindedTable.Rows[0][col.ColumnName] = value.Rows[0][col.ColumnName];
                    }
                }
            }
            catch (Exception ex)
            {
                // not throwing error as continuous data pouring in from EPNL. If there is one column that does not get updated, we do not want others to
                //stop updating too. Also we do not want exceptions to be thrown every 5 sec on UI.
                //Here we ensure at lease other columns are being updated.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                ultraGrid1.ResumeRowSynchronization();
                ultraGrid1.EndUpdate();
            }
        }

        public void BindDataSource(DataTable value, Type type, string prefFilePath, bool isControlVisible)
        {
            try
            {
                if (UIValidation.GetInstance().validate(this))
                {
                    if (InvokeRequired)
                    {
                        if (IsDisposed && Disposing)
                            return;

                        MethodInvoker del =
                        delegate
                        {
                            BindDataSource(value, type, prefFilePath, isControlVisible);
                        };

                        try
                        {
                            BeginInvoke(del);
                        }
                        catch (ObjectDisposedException)
                        {
                            //http://jira.nirvanasolutions.com:8080/browse/PRANA-6350
                            //Do Nothing as here is thread race condition and in some cases it will throgh error while closing the form.
                        }
                    }
                    else
                    {
                        _bindedTable = value;

                        ultraGrid1.DataSource = value; // GetItemList(value);
                        grdPrint.DataSource = _dtSummPrint;
                        LoadLayout(prefFilePath);
                        this.Visible = isControlVisible;
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

        public void CloseColumnChooser()
        {
            if (_customColumnChooserDialog != null)
            {
                _customColumnChooserDialog.Close();
            }
        }

        public void DenyEdit()
        {
            try
            {
                btnColumnChooser.Enabled = false;
                ultraGrid1.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;
                ultraGrid1.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False;
                ultraGrid1.DisplayLayout.Override.HeaderAppearance.Cursor = Cursors.Arrow;
                ultraGrid1.DisplayLayout.Override.RowSizing = RowSizing.Fixed;
                ultraGrid1.DisplayLayout.Override.AllowColSizing = AllowColSizing.None;
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

        public void LoadLayout(string filePath)
        {
            try
            {
                //Keep the column info before loading the preferences as it might alter the datasource.
                //DataTable dt = ultraGrid1.DataSource as DataTable;

                if (File.Exists(filePath))
                {
                    //ultraGrid1.DisplayLayout.LoadFromXml(filePath);

                    UltraGridLayout savedLayout = new UltraGridLayout();

                    savedLayout.LoadFromXml(filePath);

                    Dictionary<string, List<string>> newColumns = new Dictionary<string, List<string>>();

                    foreach (UltraGridColumn existingCol in ultraGrid1.DisplayLayout.Bands[0].Columns)
                    {
                        if (!(savedLayout.Bands[0].Columns.Exists(existingCol.Key)))
                        {
                            // => new columns , SO save all the previous properties
                            var colProperties = new List<string>
                            {
                                existingCol.Format,
                                existingCol.CellActivation.ToString(),
                                existingCol.NullText,
                                existingCol.Hidden.ToString(),
                                existingCol.ExcludeFromColumnChooser.ToString()
                            };
                            newColumns.Add(existingCol.Key, colProperties);
                        }
                    }
                    //ultraGrid1.DisplayLayout.LoadFromXml(filePath);
                    ultraGrid1.DisplayLayout.Load(savedLayout, PropertyCategories.All);
                    foreach (UltraGridColumn existingCol in ultraGrid1.DisplayLayout.Bands[0].Columns)
                    {
                        if (!(savedLayout.Bands[0].Columns.Exists(existingCol.Key)))
                        {
                            // => these were lost as a result of load layout, restore the desired properties
                            existingCol.Format = newColumns[existingCol.Key][0];
                            existingCol.CellActivation = (Activation)Enum.Parse(typeof(Activation), newColumns[existingCol.Key][1], false);
                            existingCol.NullText = newColumns[existingCol.Key][2];
                            existingCol.Hidden = Convert.ToBoolean(newColumns[existingCol.Key][3]);
                            existingCol.ExcludeFromColumnChooser = (ExcludeFromColumnChooser)Enum.Parse(typeof(ExcludeFromColumnChooser), newColumns[existingCol.Key][4], false);
                        }
                    }
                }
                else
                {
                    SetCardDefaultLayout();
                    //Save the default layout in the file so that it get written in the db.
                    SaveLayout(filePath);
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

        public void RestoreDefaultDashBoard(DataTable value, string filePath)
        {
            try
            {
                _bindedTable = value;
                ultraGrid1.DataSource = value;
                SetCardDefaultLayout();
                SaveLayout(filePath);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void SaveLayout(string filePath)
        {
            try
            {
                if (ultraGrid1.DataSource != null)
                {
                    ultraGrid1.DisplayLayout.SaveAsXml(filePath);
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

        public void SetGridFontSize(decimal fontsize)
        {
            try
            {
                float fontSize = Convert.ToSingle(fontsize);
                Font oldFont = ultraGrid1.Font;
                Font newFont = new Font(oldFont.FontFamily, fontSize, oldFont.Style, oldFont.Unit, oldFont.GdiCharSet, oldFont.GdiVerticalFont);
                ultraGrid1.Font = newFont;
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

        #endregion

        #region private-methods

        private void ColorCell(UltraGridCell cell)
        {
            try
            {
                if (cell.Value == null)
                {
                    return;
                }
                string s = cell.Value.ToString();
                if (s == string.Empty)
                {
                    return;
                }
                double value = double.Parse(s);
                if (value > 0)
                {
                    cell.Appearance.ForeColor = CustomThemeHelper.ApplyTheme ? Color.FromArgb(23, 160, 94) : Color.FromArgb(177, 216, 64);
                }
                else if (value < 0)
                {
                    cell.Appearance.ForeColor = CustomThemeHelper.ApplyTheme ? Color.FromArgb(192, 57, 43) : Color.FromArgb(255, 91, 71);
                }
                else
                {
                    cell.Appearance.ForeColor = CustomThemeHelper.ApplyTheme ? Color.FromArgb(127, 140, 141) : Color.White;
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

        private DataTable GetSummaryPrintDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            return dt;
        }

        private void SetCardDefaultLayout()
        {
            try
            {
                //this.ultraGrid1.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.Select;
                ultraGrid1.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Appearance.BorderColor = Color.Transparent;
                ultraGrid1.DisplayLayout.BorderStyle = UIElementBorderStyle.None;
                //this.ultraGrid1.DisplayLayout.Override.HeaderAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                ultraGrid1.DisplayLayout.Override.HeaderAppearance.TextHAlign = HAlign.Left;
                ultraGrid1.DisplayLayout.Override.HeaderAppearance.TextVAlign = VAlign.Middle;

                UltraGridBand band = ultraGrid1.DisplayLayout.Bands[0];
                band.CardSettings.CardScrollbars = CardScrollbars.None;
                band.CardSettings.ShowCaption = false;
                //band.CardSettings.AllowSizing = true;
                //band.Override.RowSpacingAfter = 7;
                //band.Override.CellSpacing = 7;
                // Turn on the row layout functionality for Table1 band.
                //band.UseRowLayout = true;
                band.RowLayouts.Clear();
                // Create a new row layout with "RowLayout1" as the key.
                RowLayout rowLayout1 = band.RowLayouts.Add("RowLayout1");
                //band.RowLayoutStyle = RowLayoutStyle.ColumnLayout;
                rowLayout1.CardView = true;
                rowLayout1.CardViewStyle = CardStyle.StandardLabels;
                //rowLayout1.RowLayoutLabelStyle = RowLayoutLabelStyle.Separate;
                rowLayout1.RowLayoutLabelPosition = LabelPosition.Left;
                SetDefaultColumnPositions(rowLayout1.ColumnInfos);

                rowLayout1.Apply();
                // This is workarround of the issue where columns were not displayed on proper positions.
                //http://www.infragistics.com/membership/mysupport.aspx?CaseNumber=CAS-30101-VAKKAJ
                //band.UseRowLayout = true;
                band.RowLayoutStyle = RowLayoutStyle.ColumnLayout;
                band.CardSettings.MaxCardAreaCols = 5;
                //ultraGrid1.DisplayLayout.Override.SelectedAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;
                //ultraGrid1.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;
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

        private void SetDefaultColumnPositions(RowLayoutColumnInfosCollection colInfos)
        {
            int NO_OF_ROWS = Convert.ToInt32(Math.Ceiling(colInfos.Count * 1.0 / NO_OF_COLS));
            int columnCount = 0;

            try
            {
                if (columnCount == colInfos.Count)
                {
                    return;
                }

                for (int i = 1; i <= NO_OF_ROWS; i++)
                {
                    for (int j = 0; j < NO_OF_COLS; j++)
                    {
                        if (columnCount >= colInfos.Count)
                        {
                            return;
                        }

                        while (colInfos[columnCount].Column.Hidden)
                        {
                            ++columnCount;

                            if (columnCount >= colInfos.Count)
                            {
                                return;
                            }
                        }

                        if (columnCount < colInfos.Count)
                        {
                            colInfos[columnCount].Initialize(j * 2, i * 2);
                            ++columnCount;
                            //continue;
                        }

                        if (columnCount >= colInfos.Count)
                        {
                            break;
                        }
                    }

                    if (columnCount >= colInfos.Count)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ShowCustomColumnChooserDialog(bool isVisible)
        {
            if (!isVisible)
            {
                (FindForm()).AddCustomColumnChooser(ultraGrid1);
            }
            else
            {
                ultraGrid1.DisplayLayout.ColumnChooserEnabled = DefaultableBoolean.True;
            }
        }

        #endregion

        #region events

        private void ultraGrid1_ShowCustomColumnChooserDialog()
        {
            try
            {
                if (_customColumnChooserDialog == null || _customColumnChooserDialog.IsDisposed)
                {
                    _customColumnChooserDialog = new CustomColumnChooser();
                }
                if (_customColumnChooserDialog.Owner == null)
                {
                    _customColumnChooserDialog.Owner = FindForm();
                }
                if (_customColumnChooserDialog.Grid == null)
                {
                    _customColumnChooserDialog.Grid = ultraGrid1;
                }
                _customColumnChooserDialog.Show();
                _customColumnChooserDialog.DesktopLocation = _customColumnChooserDialog.Owner.DesktopLocation;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ultraGrid1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                ultraGrid1.DisplayLayout.Override.SelectedAppearancesEnabled = DefaultableBoolean.False;
                ultraGrid1.DisplayLayout.Override.ActiveAppearancesEnabled = DefaultableBoolean.False;
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

        private void btnColumnChooser_Click(object sender, EventArgs e)
        {
            try
            {
                ShowCustomColumnChooserDialog(false);
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

        private void grdPrint_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand band = grdPrint.DisplayLayout.Bands[0];

            UltraGridColumn colName = band.Columns["Name"];
            UltraGridColumn colValue = band.Columns["Value"];
            colName.Width = 150;
            colName.Header.Caption = "";
            colValue.Header.Caption = "";
            colValue.Format = @"#,0";
            colValue.CellAppearance.TextHAlign = HAlign.Right;
        }


        private void ultraGrid1_BeforeColumnChooserDisplayed(object sender, BeforeColumnChooserDisplayedEventArgs e)
        {
            try
            {
                e.Cancel = true;
                ultraGrid1_ShowCustomColumnChooserDialog();
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

        private void ultraGrid1_BeforeCustomRowFilterDialog(object sender, BeforeCustomRowFilterDialogEventArgs e)
        {
            e.CustomRowFiltersDialog.PaintDynamicForm();
        }

        private void ultraGrid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = ultraGrid1.DisplayLayout.Bands[0];
                band.Override.CellAppearance.FontData.Bold = DefaultableBoolean.True;
                band.Override.CellAppearance.TextHAlign = HAlign.Left;
                band.Override.CellAppearance.TextVAlign = VAlign.Middle;

                foreach (DataColumn col in _bindedTable.Columns)
                {
                    if (band.Columns.Exists(col.ColumnName))
                    {
                        band.Columns[col.ColumnName].CellActivation = Activation.NoEdit;
                        band.Columns[col.ColumnName].NullText = @"N/A";

                        object format = col.ExtendedProperties["Format"];
                        if (format != null)
                        {
                            band.Columns[col.ColumnName].Format = format.ToString();
                        }

                        object hidden = col.ExtendedProperties["Hidden"];
                        if (hidden != null)
                        {
                            bool isHidden = Convert.ToBoolean(hidden);
                            band.Columns[col.ColumnName].Hidden = isHidden;
                            if (isHidden)
                            {
                                band.Columns[col.ColumnName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
                            }
                            else
                            {
                                //Some of the columns are visible in column chooser only but not in dashboard
                                object isVisibleInColChooserOnly = col.ExtendedProperties["OnlyInColChooser"];
                                if (isVisibleInColChooserOnly != null && Convert.ToBoolean(isVisibleInColChooserOnly))
                                {
                                    band.Columns[col.ColumnName].Hidden = true;
                                }
                                else
                                {
                                    band.Columns[col.ColumnName].Hidden = false;
                                }
                                band.Columns[col.ColumnName].ExcludeFromColumnChooser = ExcludeFromColumnChooser.False;
                            }
                        }
                        else
                        {
                            // is  a new column
                        }
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

        private void ultraGrid1_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                CellsCollection cells = e.Row.Cells;

                foreach (UltraGridCell cell in cells)
                {
                    if (cell.Column.DataType != typeof(Double))
                    {
                        continue;
                    }
                    if (!cell.Column.Hidden)
                    {
                        ColorCell(cell);
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

        #endregion
    }
}
