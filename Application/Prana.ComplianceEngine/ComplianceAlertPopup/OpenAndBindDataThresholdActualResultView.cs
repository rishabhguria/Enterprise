using Infragistics.Win.Misc;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinGrid.ExcelExport;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI.ImportExportUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.ComplianceAlertPopup
{
    public partial class OpenAndBindDataThresholdActualResultView
    {

        /// <summary>
        /// Binds threshold and actual result's data
        /// </summary>
        public void OpenAndBindDataThresholdActualResultView1(string constraintFields, string threshold, string actualResult)
        {
            try
            {
                List<string> constraintFieldsList = new List<string>();
                List<string> thresholdList = new List<string>();
                List<string> actualResultList = new List<string>();

                constraintFieldsList = constraintFields.Split(ComplainceConstants.CONST_SEPARATOR_CHAR.ToCharArray()).ToList();
                thresholdList = threshold.Split(ComplainceConstants.CONST_SEPARATOR_CHAR.ToCharArray()).ToList();
                actualResultList = actualResult.Split(ComplainceConstants.CONST_SEPARATOR_CHAR.ToCharArray()).ToList();

                DataTable resultTable = CreateTable(constraintFieldsList, thresholdList, actualResultList);

                ThresholdActualResultDetails alertThresholdActualResultViewControl;
                Form formThresholdActualResultView;

                alertThresholdActualResultViewControl = new ThresholdActualResultDetails();
                formThresholdActualResultView = new Form();
                alertThresholdActualResultViewControl.BindData(resultTable);

                UltraPanel ultraPanel1 = new UltraPanel();
                ultraPanel1.ClientArea.SuspendLayout();
                ultraPanel1.SuspendLayout();
                ultraPanel1.Dock = DockStyle.Fill;
                ultraPanel1.Name = "ultraPanel1";
                ultraPanel1.ClientArea.Controls.Add(alertThresholdActualResultViewControl);
                alertThresholdActualResultViewControl.Dock = DockStyle.Fill;

                formThresholdActualResultView.Controls.Add(ultraPanel1);
                formThresholdActualResultView.ShowIcon = false;
                formThresholdActualResultView.Text = ComplainceConstants.CONST_Threshold_And_Actual_Result;
                formThresholdActualResultView.Size = new System.Drawing.Size(500, 250);
                formThresholdActualResultView.StartPosition = FormStartPosition.CenterParent;
                formThresholdActualResultView.MaximumSize = formThresholdActualResultView.MinimumSize = new System.Drawing.Size(500, 250);
                formThresholdActualResultView.MinimumSize = formThresholdActualResultView.MinimumSize = new System.Drawing.Size(500, 250);
                formThresholdActualResultView.MaximizeBox = false;
                formThresholdActualResultView.MinimizeBox = false;

                CustomThemeHelper.AddUltraFormManagerToDynamicForm(formThresholdActualResultView);
                CustomThemeHelper.SetThemeProperties(formThresholdActualResultView, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_COMPLIANCE_ENGINE);

                formThresholdActualResultView.ShowInTaskbar = false;
                ultraPanel1.ClientArea.ResumeLayout(false);
                ultraPanel1.ClientArea.PerformLayout();
                ultraPanel1.ResumeLayout(false);
                formThresholdActualResultView.ShowDialog();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Creates table for threshold and actual result
        /// </summary>
        private DataTable CreateTable(List<string> constraintFieldsList, List<string> thresholdList, List<string> actualResultList)
        {
            DataTable result = new DataTable();
            try
            {
                result.Columns.Add(ComplainceConstants.CONST_FIELD_NAME);
                result.Columns.Add(ComplainceConstants.CONST_THRESHOLD);
                result.Columns.Add(ComplainceConstants.CONST_CAPTION_ACTUAL_RESULT);
                for (int i = 0; i < thresholdList.Count; i++)
                {
                    DataRow row = result.NewRow();

                    List<string> ListOfFieldData = new List<string>(ComplainceConstants.CONST_FieldDataStr.Split(','));
                    List<string> fieldsList = new List<string>();
                    foreach (var field in ListOfFieldData)
                    {
                        fieldsList.Add(field.ToLower());
                    }
                    if ((!fieldsList.Contains((constraintFieldsList[i].ToLower()))) && CachedDataManager.IsMarketDataBlocked && CachedDataManager.CompanyMarketDataProvider == MarketDataProvider.SAPI
                          && (!CachedDataManager.GetInstance.IsMarketDataPermissionEnabled))
                    {
                        row[ComplainceConstants.CONST_FIELD_NAME] = SplitCamelCase(constraintFieldsList[i]);
                        row[ComplainceConstants.CONST_THRESHOLD] = ComplainceConstants.CONST_CensorValue;
                        row[ComplainceConstants.CONST_CAPTION_ACTUAL_RESULT] = ComplainceConstants.CONST_CensorValue;
                    }
                    else
                    {
                        row[ComplainceConstants.CONST_FIELD_NAME] = SplitCamelCase(constraintFieldsList[i]);
                        row[ComplainceConstants.CONST_THRESHOLD] = SetPricisionForCells(thresholdList[i]);
                        row[ComplainceConstants.CONST_CAPTION_ACTUAL_RESULT] = SetPricisionForCells(actualResultList[i]);
                    }
                    result.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

            return result;
        }

        /// <summary>
        /// To set precision of thresholad anc actual cells if cells have numeric value.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string SetPricisionForCells(string list)
        {
            try
            {
                double numericValue;
                bool isNumeric = double.TryParse(list, out numericValue);
                if (isNumeric)
                    return string.Format(ComplainceConstants.CONST_PRECISION_FORMAT, numericValue);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return list;
        }
        /// <summary>
        /// Splits string in camel case.
        /// </summary>
        /// <param name="p">String</param>
        /// <returns>Split String by Camel case</returns>
        private string SplitCamelCase(string input)
        {
            try
            {
                return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return string.Empty;
            }
        }

        /// <summary>
        /// Export To Excel
        /// </summary>
        /// <param name="gridToExport"></param>
        /// <param name="ultraGridExcelExporter"></param>
        /// <param name="name"></param>
        public void ExportToExcel(UltraGrid gridToExport, UltraGridExcelExporter ultraGridExcelExporter, string name)
        {
            try
            {
                if (gridToExport.Rows.Count > 0)
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    string folderPath = null;
                    saveFileDialog1 = new SaveFileDialog
                    {
                        InitialDirectory = Application.StartupPath,
                        Filter = "Excel WorkBook File (*.xls)|*.xls|CSV File (*.csv)|*.csv",
                        RestoreDirectory = true
                    };
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        folderPath = saveFileDialog1.FileName;

                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        if (folderPath.ToLower().EndsWith(".xls"))
                        {
                            string workbookName = name + ComplainceConstants.CONST_LIST;
                            Infragistics.Documents.Excel.Workbook workBook = new Infragistics.Documents.Excel.Workbook();
                            workBook.Worksheets.Add(workbookName);
                            workBook.WindowOptions.SelectedWorksheet = workBook.Worksheets[workbookName];
                            workBook = ultraGridExcelExporter.Export(gridToExport, workBook.Worksheets[workbookName]);
                            workBook.Save(folderPath);
                        }
                        else
                        {
                            UltraGridFileExporter.ExportFile(gridToExport, Path.ChangeExtension(folderPath, null), AutomationEnum.FileFormat.csv);
                        }
                        InformationMessageBox.Display(ComplainceConstants.MSG_FILE_EXPORTED_SUCCESSFULLY, name + ComplainceConstants.CONST_DATA_EXPORT);
                    }
                }
                else
                    InformationMessageBox.Display(ComplainceConstants.MSG_NOTHING_TO_EXPORT, name + ComplainceConstants.CONST_DATA_EXPORT);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                MessageBox.Show(ComplainceConstants.MSG_EXPORT_FAILED, name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (rethrow)
                    throw;
            }
        }
    }
}