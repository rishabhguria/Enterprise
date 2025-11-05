//using Infragistics.Win.UltraWinGrid;
//using Prana.Utilities.ImportExportUtilities;
//using System;
//using System.Data;

//namespace Prana.Utilities.UI.ImportExportUtilities.Excel
//{
//    class ImportExportManager
//    {
//        // to get the datat able in desire file format xls,csv,txt
//        public DataTable GetDataTableFromDifferentFileFormats(string fileName)
//        {
//            DataTable datasourceData = null;

//            string fileFormat = fileName.Substring(fileName.LastIndexOf(".") + 1);

//            string completeFileName = fileName;//startupPath + intermediateFileName;

//            switch (fileFormat.ToUpperInvariant())
//            {
//                case "CSV":
//                    datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Csv).GetDataTableFromUploadedDataFile(completeFileName);
//                    break;
//                case "XLS":
//                    datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Excel).GetDataTableFromUploadedDataFile(completeFileName);
//                    break;
//                case "TXT":
//                    datasourceData = FileReaderFactory.Create(DataSourceFileFormat.Text).GetDataTableFromUploadedDataFile(completeFileName);
//                    break;
//                default:
//                    break;
//            }


//            return datasourceData;
//        }
//        // validation check that file and xslt are same
//        public bool ValidationCheck()
//        {

//            UltraGrid gridRunUpload = new UltraGrid();
//            bool iScheck = false;
//            foreach (UltraGridRow gridrow in gridRunUpload.Rows)
//            {
//                bool isRowSelected = Convert.ToBoolean(gridrow.Cells["Select"].Value);
//                if (isRowSelected)
//                {
//                    string fileWithPath = Convert.ToString(gridrow.Cells["FilePath"].Value);
//                    string strXSLTPath = Convert.ToString(gridrow.Cells["XSLTFile"].Value);
//                    if (String.IsNullOrEmpty(fileWithPath) || String.IsNullOrEmpty(strXSLTPath))
//                    {
//                        iScheck = false;
//                        break;
//                    }
//                    else
//                    {
//                        iScheck = true;
//                    }
//                }
//            }
//            return iScheck;
//        }
//    }
//}
