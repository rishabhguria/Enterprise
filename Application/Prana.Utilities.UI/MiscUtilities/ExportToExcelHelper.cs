using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.LogManager;
using System;
using System.IO;
using System.Windows.Forms;

namespace Prana.Utilities.UI.MiscUtilities
{
    public class ExportToExcelHelper
    {

        public static bool ExportToExcel(UltraGrid grd, UltraGridExcelExporter ultraGridExcelExporter = null)
        {
            bool result = false;
            try
            {
                Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                string pathName = null;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                saveFileDialog1.Filter = "Excel WorkBook Files (*.xls)|*.xls|All Files (*.*)|*.*";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    pathName = saveFileDialog1.FileName;
                }
                else
                {
                    return result;

                }
                string workbookName = "Report" + DateTime.Now.Date.ToString("yyyyMMdd");
                workBook.Worksheets.Add(workbookName);

                workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];
                if (ultraGridExcelExporter == null)
                {
                    ultraGridExcelExporter = new UltraGridExcelExporter();
                }
                workBook = ultraGridExcelExporter.Export(grd, workBook.Worksheets[workbookName]);
                workBook.Save(pathName);
                result = true;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return result;
        }

    }
}
