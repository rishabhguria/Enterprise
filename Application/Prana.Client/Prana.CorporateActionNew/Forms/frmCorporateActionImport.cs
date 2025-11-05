using Infragistics.Win.UltraWinGrid;
using Prana.LogManager;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Xsl;

namespace Prana.CorporateActionNew.Forms
{
    public partial class frmCorporateActionImport : Form
    {
        DataTable dt;
        CheckBoxOnHeader_CreationFilter headerCheckBox = new CheckBoxOnHeader_CreationFilter();

        private DataTable _selectedRowsTable;
        public DataRowCollection SelectedRows
        {
            get
            {
                return _selectedRowsTable.Rows;
            }
        }


        public frmCorporateActionImport()
        {
            try
            {
                InitializeComponent();
                btnImport.Enabled = false;
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

        private void btnXSLTFile_Click(object sender, EventArgs e)
        {
            try
            {
                GetXSLTFileName();
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
        private void btnSourceFile_Click(object sender, EventArgs e)
        {
            try
            {
                GetSourceFileName();
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
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSuccessful = TransformSourceFile();
                btnImport.Enabled = isSuccessful;
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

        private bool TransformSourceFile()
        {
            try
            {
                string xsltFileName = txtbxXSLTFile.Text;
                string sourceFileName = txtbxSourceFile.Text;

                if (!File.Exists(xsltFileName))
                {
                    MessageBox.Show("Incorrect Path or Missing 'XSLT' File !", "Nirvana", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                else if (!File.Exists(sourceFileName))
                {
                    MessageBox.Show("Incorrect Path or Missing 'Source' File !", "Nirvana", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                else
                {
                    dt = GetDataTableFromDifferentFileFormats(sourceFileName);
                    dt.TableName = "Comparision";
                    BindData();

                    string tempPath = Application.StartupPath + "\\xmls\\Transformation\\Temp";
                    string inputCAXML = Application.StartupPath + "\\xmls\\Transformation\\Temp\\InputCAXML.xml";
                    string outputCAXML = Application.StartupPath + "\\xmls\\Transformation\\Temp\\OutPutCAXML.xml";

                    //DirectoryInfo dir = new DirectoryInfo(tempPath);
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }

                    dt.WriteXml(inputCAXML);
                    //XslTransform xslt = new XslTransform();
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(xsltFileName);
                    xslt.Transform(inputCAXML, outputCAXML);
                    DataSet ds = new DataSet();
                    ds.ReadXml(outputCAXML);
                    dt = ds.Tables[0];
                    AddPrimaryKey(dt);
                    BindData();
                    _selectedRowsTable = dt.Clone();
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
                return false;
            }
            return true;
        }
        private static DataTable GetDataTableFromDifferentFileFormats(string fileName)
        {
            DataTable dTable = null;

            try
            {
                string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

                switch (fileFormat.ToUpperInvariant())
                {
                    case "CSV":
                        dTable = FileReaderFactory.Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(fileName);
                        break;
                    case "XLS":
                        dTable = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(fileName);
                        break;
                }
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("File in use! Please close the file and retry.");
                throw;
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
            return dTable;
        }

        private void GetXSLTFileName()
        {
            try
            {
                openFileDialog1.InitialDirectory = "\\\\tsclient\\C";
                openFileDialog1.Title = "Select XSLT File";
                openFileDialog1.Filter = openFileDialog1.Filter = "Data Files (*.xslt)|*.xslt| All Files|*.*";
                openFileDialog1.FileName = String.Empty;

                DialogResult importResult = openFileDialog1.ShowDialog();
                if (importResult == DialogResult.OK)
                {
                    txtbxXSLTFile.Text = openFileDialog1.FileName;
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
        private void GetSourceFileName()
        {
            try
            {
                openFileDialog1.Title = "Select Source File";
                openFileDialog1.Filter = openFileDialog1.Filter = "Data Files (*.xls,*.csv)|*.xls;*.csv| All Files|*.*";
                openFileDialog1.FileName = String.Empty;

                DialogResult importResult = openFileDialog1.ShowDialog();
                if (importResult == DialogResult.OK)
                {
                    txtbxSourceFile.Text = openFileDialog1.FileName;
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
        private void BindData()
        {
            try
            {
                AddBoolColumn(dt);
                grdData.DataSource = dt;
                SetCheckBoxInGrid();
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
        private void SetCheckBoxInGrid()
        {
            try
            {
                grdData.CreationFilter = headerCheckBox;
                UltraGridBand band = grdData.DisplayLayout.Bands[0];
                band.Columns["IsChecked"].CellClickAction = CellClickAction.Edit;
                band.Columns["IsChecked"].AllowRowFiltering = Infragistics.Win.DefaultableBoolean.False;
                band.Columns["IsChecked"].Header.VisiblePosition = 0;
                band.Columns["IsChecked"].Header.Caption = "";
                band.Columns["IsChecked"].SortIndicator = SortIndicator.Disabled;
                band.Columns["IsChecked"].Width = 4;
                band.Columns["IsChecked"].ExcludeFromColumnChooser = ExcludeFromColumnChooser.True;
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
        public static void AddBoolColumn(DataTable dt)
        {
            try
            {
                if (!dt.Columns.Contains("IsChecked"))
                {
                    DataColumn col = new DataColumn("IsChecked", typeof(Boolean));
                    col.DefaultValue = false;
                    dt.Columns.Add(col);
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
        public static void AddPrimaryKey(DataTable dt)
        {
            try
            {
                if (!dt.Columns.Contains("RowID"))
                {
                    dt.Columns.Add("RowID");
                    int rowID = 0;

                    foreach (DataRow row in dt.Rows)
                    {
                        row["RowID"] = rowID;
                        rowID++;
                    }
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["RowID"] };
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


        private void grdData_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "IsChecked")
                {
                    UltraGridRow grdRow = e.Cell.Row;
                    DataRow selectedRow = ((DataRowView)grdRow.ListObject).Row;
                    int rowID = Convert.ToInt32(grdRow.Cells["RowID"].Value);
                    if (selectedRow != null)
                    {
                        string isSelected = e.Cell.Text.ToString().ToUpper();
                        if (isSelected.Equals("TRUE"))
                        {
                            if (!_selectedRowsTable.Rows.Contains(rowID))
                            {
                                DataRow row = _selectedRowsTable.NewRow();
                                row.ItemArray = selectedRow.ItemArray;
                                _selectedRowsTable.Rows.Add(row);

                            }

                            grdRow.Appearance.BackColor = Color.Black;
                            grdRow.Appearance.ForeColor = Color.Orange;
                        }
                        else
                        {
                            if (_selectedRowsTable.Rows.Contains(rowID))
                            {
                                DataRow row = _selectedRowsTable.Rows.Find(rowID);
                                _selectedRowsTable.Rows.Remove(row);
                            }

                            grdRow.Appearance.BackColor = Color.Black;
                            grdRow.Appearance.ForeColor = Color.White;
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
        private void grdData_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = e.Layout.Bands[0];
                if (band.Columns.Exists("RowID"))
                {
                    band.Columns["RowID"].Hidden = true;
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

        private void txtbxXSLTFile_TextChanged(object sender, EventArgs e)
        {
            btnImport.Enabled = false;
        }
        private void txtbxSourceFile_TextChanged(object sender, EventArgs e)
        {
            btnImport.Enabled = false;
        }

        private void frmCorporateActionImport_Load(object sender, EventArgs e)
        {
            try
            {
                SetButtonsColor();
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CORP_ACTION);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void SetButtonsColor()
        {
            try
            {
                btnXSLTFile.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnXSLTFile.ForeColor = System.Drawing.Color.White;
                btnXSLTFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnXSLTFile.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnXSLTFile.UseAppStyling = false;
                btnXSLTFile.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSourceFile.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnSourceFile.ForeColor = System.Drawing.Color.White;
                btnSourceFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSourceFile.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSourceFile.UseAppStyling = false;
                btnSourceFile.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnView.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnView.ForeColor = System.Drawing.Color.White;
                btnView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnView.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnView.UseAppStyling = false;
                btnView.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnImport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnImport.ForeColor = System.Drawing.Color.White;
                btnImport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnImport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnImport.UseAppStyling = false;
                btnImport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

    }
}