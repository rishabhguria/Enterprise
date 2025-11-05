using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.AdminTools
{
    public partial class UsrCtrlFileUpLoad : UserControl
    {
        int _fileType = 0;
        List<int> deletedIDS = new List<int>();
        DataTable dt = new DataTable();
        public UsrCtrlFileUpLoad(int fileTypeID)
        {
            _fileType = fileTypeID;
            dt = new DataTable();
            dt.Columns.Add("FileID");
            dt.Columns.Add("FileName");
            dt.Columns.Add("FileType");
            InitializeComponent();

        }
        public void AddRow(DataRow row)
        {
            try
            {
                dt.Rows.Add(row.ItemArray);
            }
            catch (Exception)
            {
                throw;
            }
        }
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

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
            openFileDialog1.InitialDirectory = "DeskTop";
            openFileDialog1.Title = title;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "XSLT Files (*.xslt)|*.xslt|xmls Files (*.xml)|*.xml|XSD Files (*.xsd)|*.xsd";
            string strFileName = string.Empty;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFileName = openFileDialog1.FileName;
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
                        if (fileName != string.Empty)
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdData.ActiveRow != null)
            {
                deletedIDS.Add(int.Parse(grdData.ActiveRow.Cells["FileID"].Value.ToString()));
            }
        }

    }
}
