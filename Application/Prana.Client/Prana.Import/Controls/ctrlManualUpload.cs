using Prana.BusinessObjects;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.TaskManagement.Definition.Definition;
using Prana.TaskManagement.Execution;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace Prana.Import.Controls
{
    public partial class ctrlManualUpload : UserControl
    {
        TaskResult _taskResult = null;
        //added by: Bharat Raturi, 24 jun 2014
        //hold the dashboard file names
        Dictionary<string, List<string>> _ftpFileList = new Dictionary<string, List<string>>();

        private bool _isUploadButtonClicked = false;
        public ctrlManualUpload()
        {
            InitializeComponent();
            BindAccountCombo();
            if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
            {
                SetButtonsColor();
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnGo.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnGo.ForeColor = System.Drawing.Color.White;
                btnGo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnGo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnGo.UseAppStyling = false;
                btnGo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnBrowse.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnBrowse.ForeColor = System.Drawing.Color.White;
                btnBrowse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBrowse.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnBrowse.UseAppStyling = false;
                btnBrowse.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// Browse the file to import from any directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                string fileNameTobeImported = string.Empty;
                Boolean CheckAccessPermission = true;
                fileNameTobeImported = OpenFileDialogHelper.GetFileNameUsingOpenFileDialog(CheckAccessPermission);


                if (!string.IsNullOrWhiteSpace(fileNameTobeImported))
                {
                    if (File.Exists((Application.StartupPath + Path.GetFileName(fileNameTobeImported))))
                    {
                        File.Delete(Application.StartupPath + Path.GetFileName(fileNameTobeImported));
                    }
                    File.Copy(fileNameTobeImported, Application.StartupPath + Path.GetFileName(fileNameTobeImported));
                    txtName.Text = Path.GetFileName(fileNameTobeImported);
                    txtImportFilePath.Text = fileNameTobeImported;
                }
                else
                {
                    //fileNameTobeImported = string.Empty;
                    //MessageBox.Show("Operation canceled by User.", "Exception Report");
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
        /// This method initilizes control
        /// e.g. set values in user combo box
        /// </summary>
        internal void InitializeControl(string formatName, TaskResult taskResult, Dictionary<string, List<string>> fileList)
        {
            try
            {
                _ftpFileList = fileList;
                _taskResult = taskResult;
                if (string.IsNullOrEmpty(formatName))
                {
                    LoadDataInCmbImportFormats();
                    _isUploadButtonClicked = true;
                }
                else
                {
                    Dictionary<int, String> dictImportFormats = new Dictionary<int, string>();
                    dictImportFormats.Add(1, formatName);
                    BindDataToCmbFormat(dictImportFormats);
                    cmbFormat.SetInitialValue(1, formatName);

                    cmbFormat.Enabled = false;
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
        /// The DragDrop event for the control where the drop will occur, retrieve the data being dragged using the GetData.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtImportFilePath_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length != 0)
                {
                    txtImportFilePath.Text = files[0];
                    txtName.Text = Path.GetFileName(files[0]);
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
        /// The DragEnter event for the control where the drop will occur, do the type-checking here to ensure that the data being dragged is of an acceptable type. In this case it is FileDrop, which specifies the Windows file drop format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtImportFilePath_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
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
        /// Loads Data on the cmbThirdPartyFormat from db
        /// </summary>
        private void LoadDataInCmbImportFormats()
        {
            try
            {
                Dictionary<int, String> dictImportFormats = ImportDataManager.GetImportFormatNames();
                dictImportFormats.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                BindDataToCmbFormat(dictImportFormats);
                cmbFormat.Value = int.MinValue;
                //added by: Bharat raturi, 09 jun 2014
                //purpose: adjust the width of the drop down list according to the length of the text
                cmbFormat.DisplayLayout.Bands[0].Columns[1].Width = cmbFormat.DisplayLayout.Bands[0].Columns[1].CalculateAutoResizeWidth(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand, true);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            // return null;
        }

        private void BindDataToCmbFormat(Dictionary<int, String> dictImportFormats)
        {
            try
            {
                cmbFormat.DataSource = null;
                if (dictImportFormats != null)
                {
                    cmbFormat.DataSource = dictImportFormats.ToArray();
                    cmbFormat.DisplayMember = "Value";
                    cmbFormat.ValueMember = "Key";
                    //hides the column value
                    cmbFormat.DisplayLayout.Bands[0].Columns["Key"].Hidden = true;
                    cmbFormat.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    //set column width equal to control
                    cmbFormat.DisplayLayout.Bands[0].Columns["Value"].Width = cmbFormat.Size.Width;

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
        /// Run import process on clicking on this button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbFormat.Value != null && !cmbFormat.Value.Equals(int.MinValue) && !string.IsNullOrEmpty(txtImportFilePath.Text))
                {
                    string fileNameForUpload = Path.GetFileName(txtImportFilePath.Text.Trim());
                    string batchName = cmbFormat.Text;

                    #region For user selected date and Account
                    string userSelectedDate = string.Empty;
                    string userSelectedAccount = string.Empty;
                    if (chkUserSelectedDate.Checked)
                    {
                        userSelectedDate = txtUserSelectedDate.Value.ToString();
                    }
                    if (chkUserSelectedAccount.Checked)
                    {
                        if (cmbAccount.Value.ToString().Equals("-Select-", StringComparison.InvariantCultureIgnoreCase))
                        {
                            userSelectedAccount = int.MinValue.ToString();
                        }
                        else
                        {
                            userSelectedAccount = cmbAccount.Value.ToString();
                        }
                    }
                    #endregion

                    if (!_isUploadButtonClicked)
                    {
                        //check if the selected file name matches the uploaded file for the batch
                        if (_ftpFileList.ContainsKey(batchName) && !_ftpFileList[batchName].Contains(fileNameForUpload) && _ftpFileList[batchName].Count > 0)
                        {
                            //if only one file was uploaded for the batch
                            if (_ftpFileList[batchName].Count == 1)
                            {
                                MessageBox.Show("Only the file with the name '" + _ftpFileList[batchName][0] + "' can be uploaded", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                //error message in case of multiple files
                                MessageBox.Show("File name does not match with uploaded file name for the batch", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            return;
                        }
                    }

                    ExecutionInfo eInfo = DeepCopyHelper.Clone(TaskExecutionCache.Instance.GetExecutionInfo("Import_-1"));
                    //TODO: To prompt for user action we are passing another field seperated with SEPERATOR_6
                    //Here ManualUpload text is passed with input data to identify the source of RunImportProcess
                    //In RunImportProcess method process of ftp download and decryption will be skipped, as we already have 
                    //modified by omshiv, for check XSLT path in case of Auto import, removed release check for it on XSLT manager.
                    eInfo.IsAutoImport = true;
                    eInfo.InputData = cmbFormat.Text + Seperators.SEPERATOR_8 + txtImportFilePath.Text + Seperators.SEPERATOR_6 + true.ToString() + Seperators.SEPERATOR_8 + userSelectedDate + Seperators.SEPERATOR_8 + userSelectedAccount;
                    if (_taskResult != null)
                    {
                        SetDefaultFileSetupForTaskResult(_taskResult);
                    }
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1314
                    // eInfo.InputData = batchName;
                    eInfo.ExecutionName = batchName + Seperators.SEPERATOR_6 + txtName.Text;
                    //modified by: Bharat Raturi, 09 jun 2014
                    //purpose: to close the form after the input from the combobox is stored in the variable
                    ((Form)this.TopLevelControl).Close();

                    //NirvanaTask task = ImportManager.Instance;
                    //task.Initialize(eInfo.TaskInfo);
                    //task.ExecuteTask(eInfo, _taskResult);

                    BackgroundWorker bgwWorkerManualUpload = new BackgroundWorker();
                    bgwWorkerManualUpload.DoWork += bgWorkerManualUpload_DoWork;
                    object[] arguments = new object[2];
                    arguments[0] = eInfo;
                    arguments[1] = _taskResult;
                    bgwWorkerManualUpload.RunWorkerAsync(arguments);


                }
                else
                {
                    MessageBox.Show("Please select format and file or import file missing", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        void bgWorkerManualUpload_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] arguments = e.Argument as object[];
                ExecutionInfo eInfo = arguments[0] as ExecutionInfo;
                TaskResult taskResult = arguments[1] as TaskResult;

                if (eInfo != null)
                {
                    //NirvanaTask task = new ImportManager();
                    NirvanaTask task = ImportManager.Instance;
                    task.Initialize(eInfo.TaskInfo);
                    task.ExecuteTask(eInfo, taskResult);
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
        /// set RetrievalStatus,FileMetaData,FileSize,Comments,SymbolValidation to blank
        /// </summary>
        /// <param name="taskResult"></param>
        private void SetDefaultFileSetupForTaskResult(TaskResult taskResult)
        {
            try
            {
                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("RetrievalStatus", string.Empty, null);
                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FileMetaData", string.Empty, null);
                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("FileSize", string.Empty, null);
                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("Comments", string.Empty, null);
                taskResult.TaskStatistics.TaskSpecificData.AddOrUpdateDataPoint("SymbolValidation", string.Empty, null);
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
        /// bind data on account combo box
        /// </summary>
        private void BindAccountCombo()
        {
            try
            {
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2288
                //AccountCollection accountCollection = CommonDataCache.CachedDataManager.GetInstance.GetUserAccounts();
                Dictionary<int, string> dictAccounts = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict();
                dictAccounts = GeneralUtilities.SortDictionaryByValues<int, string>(dictAccounts);

                cmbAccount.DataSource = null;
                cmbAccount.DataSource = dictAccounts;
                cmbAccount.ValueMember = "Key";
                cmbAccount.DisplayMember = "Value";

                cmbAccount.DataBind();
                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn column in cmbAccount.DisplayLayout.Bands[0].Columns)
                {
                    column.Hidden = true;
                }
                cmbAccount.DisplayLayout.Bands[0].Columns["Value"].Hidden = false;
                cmbAccount.Value = "-Select-";
                cmbAccount.DisplayLayout.Bands[0].ColHeadersVisible = false;
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
        /// To enable user selected date option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkUserSelectedDate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkUserSelectedDate.Checked)
                {
                    txtUserSelectedDate.Enabled = true;
                }
                else
                {
                    txtUserSelectedDate.Enabled = false;
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
        /// To enable user selected Account dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkUserSelectedAccount_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkUserSelectedAccount.Checked)
                {
                    cmbAccount.Enabled = true;
                }
                else
                {
                    cmbAccount.Enabled = false;
                    cmbAccount.Value = "-Select-";
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
        ///Added by sachin  mishra  Purpose: set tool tip for every row   jira-PRANA-6912                                
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFormat_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                if (e.Row.Cells.Exists("Value"))
                {
                    e.Row.ToolTipText = e.Row.Cells["Value"].Text;
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
