using Infragistics.Documents.Reports.Report;
using Infragistics.Documents.Reports.Report.Section;
using Infragistics.Documents.Reports.Report.Text;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.DocumentExport;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.MiscUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Prana.Utilities.UI.ImportExportUtilities
{
    public class UltraGridFileExporter
    {
        /// <summary>
        /// Input: Ultragrid, Filename, FileFormat
        /// Output: File will be generated at given path
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="FileName"></param>
        /// <param name="FileFormat"></param>
        public static void ExportFile(UltraGrid grid, string FileName, AutomationEnum.FileFormat FileFormat)
        {
            try
            {
                //Create the File Path if does not exist
                if (!Directory.Exists(Path.GetDirectoryName(FileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FileName));
                }
                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();
                List<UltraGrid> grids = new List<UltraGrid>();
                grids.Add(grid);

                if (FileFormat.Equals(AutomationEnum.FileFormat.csv))
                {
                    excelUtils.SetExcelLayoutAndWrite(grids, true, FileName + ".csv");
                }
                else if (FileFormat.Equals(AutomationEnum.FileFormat.xls))
                {
                    UltraGridExcelExporter ultragridExporter = new UltraGridExcelExporter();
                    //ultragridExporter.InitializeColumn += ultragridExporter_InitializeColumn;
                    ultragridExporter.Export(grid, FileName + ".xls");
                }
                else if (FileFormat.Equals(AutomationEnum.FileFormat.pdf))
                {
                    UltraGridDocumentExporter ugde = new UltraGridDocumentExporter();
                    ugde.Export(grid, FileName + ".pdf", GridExportFileFormat.PDF);
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
        /// For Showing Thousand Separator Format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// http://www.infragistics.com/community/forums/t/56160.aspx
        /// //added by amit 09.04.2015
        /// //http://jira.nirvanasolutions.com:8080/browse/PRANA-7135
        //static void ultragridExporter_InitializeColumn(object sender, InitializeColumnEventArgs e)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(e.Column.Format))
        //        {
        //            e.ExcelFormatStr = e.Column.Format;
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        /// <summary>
        /// opens file save dialog to save the grid table in pdf csv or xls format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void LoadFilePathAndExport(UltraGrid grid, IWin32Window owner)
        {
            try
            {
                if (grid.DataSource != null)
                {
                    string pathName = GetFilePath(owner);
                    ExportFileForFileFormat(grid, pathName);
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

        public static string GetFilePath(IWin32Window owner)
        {
            try
            {
                string pathName = null;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                saveFileDialog1.Filter = "Excel WorkBook File (*.xls)|*.xls|CSV File (*.csv)|*.csv|PDF File (*.PDF)|*.pdf";
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog(owner) == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                return pathName;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        public static void LoadFilePathAndExportMultipleGrids(List<Tuple<UltraGrid, int, int>> listExportGrids, IWin32Window owner)
        {
            try
            {
                string pathName = GetFilePath(owner);
                ExportFileForFileFormat(listExportGrids, pathName);
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

        /// <summary>
        /// Input: Ultragrid, Filename, FileFormat
        /// Output: File will be generated at given path
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="FileName"></param>
        /// <param name="FileFormat"></param>
        public static void ExportMultipleGridsInFile(List<Tuple<UltraGrid, int, int>> listExportGrids, string FileName, AutomationEnum.FileFormat FileFormat)
        {
            try
            {
                //Create the File Path if does not exist
                if (!Directory.Exists(Path.GetDirectoryName(FileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(FileName));
                }
                ExcelAndPrintUtilities excelUtils = new ExcelAndPrintUtilities();
                #region Export csv
                if (FileFormat.Equals(AutomationEnum.FileFormat.csv))
                {
                    List<UltraGrid> grids = new List<UltraGrid>();
                    foreach (Tuple<UltraGrid, int, int> tuple in listExportGrids)
                    {
                        grids.Add(tuple.Item1);
                    }
                    excelUtils.SetExcelLayoutAndWrite(grids, true, FileName + ".csv");
                }
                #endregion
                #region Export xls
                else if (FileFormat.Equals(AutomationEnum.FileFormat.xls))
                {
                    Infragistics.Documents.Excel.Workbook workbook = new Infragistics.Documents.Excel.Workbook();
                    Infragistics.Documents.Excel.Worksheet worksheet1 = workbook.Worksheets.Add("Grid");
                    UltraGridExcelExporter ultraGridExcelExporter1 = new UltraGridExcelExporter();
                    foreach (Tuple<UltraGrid, int, int> tuple in listExportGrids)
                    {
                        ultraGridExcelExporter1.Export(tuple.Item1, worksheet1, tuple.Item2, tuple.Item3);
                    }
                    try
                    {
                        workbook.Save(FileName + ".xls");
                        //System.Diagnostics.Process.Start("output.xls");
                    }
                    catch
                    {
                        MessageBox.Show("File already in use.", "Error Saving Workbook", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                #endregion
                #region Export pdf
                else if (FileFormat.Equals(AutomationEnum.FileFormat.pdf))
                {
                    Report report = new Report();
                    ISection section = report.AddSection();

                    UltraGridDocumentExporter ultraGridDocumentExporter1 = new UltraGridDocumentExporter();
                    foreach (Tuple<UltraGrid, int, int> tuple in listExportGrids)
                    {
                        //Add Text/Empty Lines
                        IText bandDividerText = section.AddText();
                        bandDividerText.AddContent(Environment.NewLine);
                        //Add UltraGird
                        ultraGridDocumentExporter1.Export(tuple.Item1, section);
                    }
                    try
                    {
                        report.Publish(FileName + ".pdf", Infragistics.Documents.Reports.Report.FileFormat.PDF);
                        //Process.Start(FileName + ".pdf");
                    }
                    catch
                    {
                        MessageBox.Show("File already in use.", "Error Saving File", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                }
                #endregion
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

        public static void ExportFileForFileFormat(List<Tuple<UltraGrid, int, int>> listExportGrids, string pathName)
        {
            try
            {
                if (!string.IsNullOrEmpty(pathName))
                {
                    string format = Path.GetExtension(pathName);
                    string fileName = Path.GetFileNameWithoutExtension(pathName);
                    pathName = Path.GetDirectoryName(pathName) + "\\" + fileName;
                    if (format == ".xls")
                    {
                        ExportMultipleGridsInFile(listExportGrids, pathName, AutomationEnum.FileFormat.xls);
                    }
                    else if (format == ".csv")
                    {
                        foreach (Tuple<UltraGrid, int, int> tuple in listExportGrids)
                        {
                            //if grid contain group contain grouping then message pop up
                            foreach (UltraGridColumn col in tuple.Item1.DisplayLayout.Bands[0].Columns)
                            {
                                if (col.IsGroupByColumn == true)
                                {
                                    MessageBox.Show("Please remove grouping before writing in csv File", "Exception Report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                        ExportMultipleGridsInFile(listExportGrids, pathName, AutomationEnum.FileFormat.csv);
                    }
                    else
                    {
                        ExportMultipleGridsInFile(listExportGrids, pathName, AutomationEnum.FileFormat.pdf);
                    }
                }
                else
                {
                    MessageBox.Show("Operation Cancelled by user");
                }
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

        public static void ExportFileForFileFormat(UltraGrid grid, string pathName)
        {
            try
            {
                if (!string.IsNullOrEmpty(pathName))
                {
                    string format = Path.GetExtension(pathName);
                    string fileName = Path.GetFileNameWithoutExtension(pathName);
                    pathName = Path.GetDirectoryName(pathName) + "\\" + fileName;
                    if (format == ".xls")
                    {
                        ExportFile(grid, pathName, AutomationEnum.FileFormat.xls);
                    }
                    else if (format == ".csv")
                    {
                        //if grid contain group contain grouping then message pop up
                        foreach (UltraGridColumn col in grid.DisplayLayout.Bands[0].Columns)
                        {
                            if (col.IsGroupByColumn == true)
                            {
                                MessageBox.Show("Please remove grouping before writing in csv File", "Exception Report", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        ExportFile(grid, pathName, AutomationEnum.FileFormat.csv);
                    }
                    else
                    {
                        ExportFile(grid, pathName, AutomationEnum.FileFormat.pdf);
                    }
                }
                else
                {
                    MessageBox.Show("Operation Cancelled by user");
                }
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

        public static void ExportFileForFileFormat(DataSet ds, string pathName)
        {
            try
            {
                UltraGrid grid = new UltraGrid();
                Form UI = new Form();
                UI.Controls.Add(grid);
                grid.DataSource = ds;

                string fileExtension = Path.GetExtension(pathName);
                AutomationEnum.FileFormat fileFormat = new AutomationEnum.FileFormat();
                if (fileExtension.Contains("csv"))
                    fileFormat = AutomationEnum.FileFormat.csv;
                else if (fileExtension.Contains("xls"))
                    fileFormat = AutomationEnum.FileFormat.xls;
                else if (fileExtension.Contains("pdf"))
                    fileFormat = AutomationEnum.FileFormat.pdf;
                else
                    fileFormat = AutomationEnum.FileFormat.txt;

                ExportFile(grid, Path.GetDirectoryName(pathName) + "\\" + Path.GetFileNameWithoutExtension(pathName), fileFormat);
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

        public static void ExportWithoutHeaders(UltraGrid grid, string filePath, AutomationEnum.FileFormat fileFormat)
        {
            try
            {
                switch (fileFormat)
                {
                    case AutomationEnum.FileFormat.xls:
                        UltraGridExcelExporter ultragridExporter = new UltraGridExcelExporter();
                        ultragridExporter.HeaderRowExporting += ultragridExporter_HeaderRowExporting;
                        ultragridExporter.Export(grid, filePath + ".xls");
                        break;

                    case AutomationEnum.FileFormat.pdf:
                        UltraGridDocumentExporter ugde = new UltraGridDocumentExporter();
                        ugde.HeaderRowExporting += ugde_HeaderRowExporting;
                        ugde.Export(grid, filePath + ".pdf", GridExportFileFormat.PDF);
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        static void ugde_HeaderRowExporting(object sender, Infragistics.Win.UltraWinGrid.DocumentExport.HeaderRowExportingEventArgs e)
        {
            e.Cancel = true;
        }

        static void ultragridExporter_HeaderRowExporting(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.HeaderRowExportingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
