using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class UserCtrlFileUpLoad : UserControl
    {
        int _fileType = 0;
        List<int> deletedIDS = new List<int>();
        DataTable dt = new DataTable();
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="fileTypeID"></param>
        public UserCtrlFileUpLoad(int fileTypeID)
        {
            try
            {
                _fileType = fileTypeID;
                dt = new DataTable();
                dt.Columns.Add("FileID");
                dt.Columns.Add("FileName");
                dt.Columns.Add("FileType");
                InitializeComponent();
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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
                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAddRow.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAddRow.ForeColor = System.Drawing.Color.White;
                btnAddRow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAddRow.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAddRow.UseAppStyling = false;
                btnAddRow.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnDelete.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDelete.ForeColor = System.Drawing.Color.White;
                btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDelete.UseAppStyling = false;
                btnDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="row"></param>
        internal void AddRow(DataRow row)
        {
            try
            {
                if (row != null)
                {
                    dt.Rows.Add(row.ItemArray);
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
        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void BindData()
        {
            try
            {

                grdData.DataSource = dt;
                grdData.DataBind();
                if (!grdData.DisplayLayout.Bands[0].Columns.Exists("SelectFile"))
                {
                    grdData.DisplayLayout.Bands[0].Columns.Add("SelectFile");
                }
                grdData.DisplayLayout.Bands[0].Columns["SelectFile"].Header.VisiblePosition = 2;
                grdData.DisplayLayout.Bands[0].Columns["SelectFile"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                grdData.DisplayLayout.Bands[0].Columns["SelectFile"].Width = 70;
                grdData.DisplayLayout.Bands[0].Columns["SelectFile"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;
                grdData.DisplayLayout.Bands[0].Columns["SelectFile"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                grdData.DisplayLayout.Bands[0].Columns["SelectFile"].Header.Caption = "Select File";
                grdData.DisplayLayout.Bands[0].Columns["SelectFile"].SortIndicator = Infragistics.Win.UltraWinGrid.SortIndicator.Disabled;
                grdData.DisplayLayout.Bands[0].Columns["SelectFile"].NullText = "Select File";
                grdData.DisplayLayout.Bands[0].Columns["SelectFile"].Header.Caption = "";
                grdData.DisplayLayout.Bands[0].Columns["FileID"].Hidden = true;
                grdData.DisplayLayout.Bands[0].Columns["FileType"].Hidden = true;
                grdData.DisplayLayout.Bands[0].Columns["FileName"].Width = 200;
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

        private void grdData_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                if (e.Cell.Column.Key == "SelectFile")
                {
                    string title = "Select File that Maps Prana To Third Party";
                    string shortName = GetFileName(title);
                    if (!String.IsNullOrEmpty(shortName))
                    {
                        grdData.ActiveRow.Cells["FileName"].Value = shortName;
                        deletedIDS.Add(int.Parse(grdData.ActiveRow.Cells["FileID"].Value.ToString()));
                        grdData.ActiveRow.Cells["FileID"].Value = int.MinValue;
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
        private string GetFileName(string title)
        {
            string strFileName = string.Empty;
            try
            {
                openFileDialog1.InitialDirectory = "\\\\tsclient\\C";
                openFileDialog1.Title = title;
                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "XSLT Files (*.xslt)|*.xslt|xmls Files (*.xml)|*.xml|XSD Files (*.xsd)|*.xsd";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    strFileName = openFileDialog1.FileName;
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
            return strFileName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["FileID"].ToString() == int.MinValue.ToString())
                    {
                        string fileName = PranaDataManager.GetFileNameFromPath(row["FileName"].ToString());
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            byte[] data = PranaDataManager.TransformToBinary(row["FileName"].ToString());
                            PranaDataManager.SaveFile(fileName, data, _fileType);
                        }
                        row["FileID"] = -1;
                    }

                }
                foreach (int deletedID in deletedIDS)
                {
                    PranaDataManager.DeleteFile(deletedID);
                    // delete me
                }

                deletedIDS.Clear();
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

        private void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                object[] row = new object[dt.Columns.Count];
                dt.Rows.Add(row);
                DataRow addedRow = dt.Rows[dt.Rows.Count - 1];
                addedRow["FileID"] = int.MinValue;
                addedRow["FileType"] = _fileType;
                BindData();
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdData.ActiveRow != null)
                {
                    deletedIDS.Add(int.Parse(grdData.ActiveRow.Cells["FileID"].Value.ToString()));
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
