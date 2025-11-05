using Infragistics.Win.UltraWinGrid;
using Microsoft.VisualBasic.FileIO;
using Prana.BusinessObjects;
using Prana.Utilities.ImportExportUtilities;
using Prana.Utilities.UI.ImportExportUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Prana.ThirdPartyUI.Forms
{
    public partial class frmThirdPartyEditDetails : Form
    {
        private static frmThirdPartyEditDetails _frmThirdPartyEditDetailsForm = null;
        static object _locker = new object();

        private bool _isDirty = false;

        public frmThirdPartyEditDetails()
        {
            InitializeComponent();
        }


        private string _filePath = string.Empty;
        private string _delimiter = string.Empty;

        public frmThirdPartyEditDetails(string filePath, string delimiter)
        {
            InitializeComponent();
            _filePath = filePath;
            _delimiter = delimiter;
            DataTable dt = new DataTable();

            if (Path.GetExtension(_filePath).ToLower().Equals(".xml"))
                dt = GetDataTableFromXMLFile(_filePath);
            else
                dt = FileReaderFactory.GetDataTableFromDifferentFileFormats(filePath);

            ultraGrid1.DataSource = dt;
        }

        public static frmThirdPartyEditDetails GetInstance(string _outputFilePath, string delimiter)
        {
            if (_frmThirdPartyEditDetailsForm == null)
            {
                lock (_locker)
                {
                    if (_frmThirdPartyEditDetailsForm == null)
                    {
                        _frmThirdPartyEditDetailsForm = new frmThirdPartyEditDetails(_outputFilePath, delimiter);
                    }
                }
            }
            return _frmThirdPartyEditDetailsForm;
        }

        private static DataTable GetDataTableFromXMLFile(string xml_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(xml_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return csvData;
        }

        private void frmThirdPartyEditDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isDirty)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes to Generated File?", "Save?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result.Equals(DialogResult.Yes))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
                    }
                    string fileExtension = Path.GetExtension(_filePath).ToLower();
                    if (fileExtension.Equals(".txt") || fileExtension.Equals(".xml"))
                        SaveOtherFormats();
                    else if (fileExtension.Equals(".csv"))
                    {
                        DataTable dt = (DataTable)ultraGrid1.DataSource;
                        StringBuilder sb = new StringBuilder();
                        foreach (DataRow row in dt.Rows)
                        {
                            IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                            sb.AppendLine(string.Join(",", fields));
                        }

                        File.WriteAllText(_filePath, sb.ToString());
                    }
                    else
                    {

                        DataTable dt = (DataTable)ultraGrid1.DataSource;
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dt.Copy());
                        UltraGrid grid = new UltraGrid();
                        grid.DataSource = ds;
                        AutomationEnum.FileFormat fileFormat = AutomationEnum.FileFormat.xls;
                        if (fileExtension.Equals(".xls"))
                        {
                            fileFormat = AutomationEnum.FileFormat.xls;
                        }
                        else if (fileExtension.Equals(".pdf"))
                        {
                            fileFormat = AutomationEnum.FileFormat.pdf;
                        }
                        UltraGridFileExporter.ExportWithoutHeaders(grid, _filePath, fileFormat);
                    }
                }
                else if (result.Equals(DialogResult.Cancel))
                {
                    e.Cancel = true;
                    return;
                }
            }
            _frmThirdPartyEditDetailsForm = null;
        }

        private void SaveOtherFormats()
        {
            string delimiter = _delimiter;

            using (StreamWriter sw = new StreamWriter(_filePath))
            {
                // First we will write the headers.

                DataTable dt = (DataTable)ultraGrid1.DataSource;

                if (!Path.GetExtension(_filePath).ToLower().Equals(".xml"))
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        string cName = dt.Rows[0][column.ColumnName].ToString();
                        if (!dt.Columns.Contains(cName) && cName != "")
                        {
                            column.ColumnName = cName;
                        }

                    }

                    dt.Rows[0].Delete();
                }

                int iColCount = dt.Columns.Count;
                for (int i = 0; i < iColCount; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < iColCount - 1)
                    {
                        sw.Write(delimiter);
                    }
                }
                sw.Write(sw.NewLine);

                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write(dr[i].ToString());
                        }
                        if (i < iColCount - 1)
                        {
                            sw.Write(System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator);
                        }
                    }
                    sw.Write(sw.NewLine);
                }
            }
        }


        private void ultraGrid1_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            _isDirty = true;
            Infragistics.Win.UltraWinGrid.UltraGrid ug = (Infragistics.Win.UltraWinGrid.UltraGrid)sender;
            ((DataTable)ug.DataSource).Rows[e.Cell.Row.Index][e.Cell.Column.Index] = e.Cell.Text;
        }

    }
}