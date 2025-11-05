//using Prana.Reconciliation;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Prana.Tools
{
    public partial class ctrlReconXSLTMapping : UserControl
    {
        #region CONSTANTS
        private const string XSLT_MAPPING_FILE = "XSLTFileMapping.xml";
        //private const string COLUMN_SetupName = "SetupName";
        private const string COLUMN_PBName = "PBName";
        private const string COLUMN_ReconType = "ReconType";
        private const string COLUMN_FileName = "FileName";
        private const string COLUMN_XsltPath = "XsltPath";
        private const string COLUMN_SelectFile = "SelectFile";
        private const string COLUMN_SpName = "SP";
        #endregion

        private string XsltPath = string.Empty;
        private bool _isUnSavedChanges = false;
        bool _isControlInitialized = false;
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        public ctrlReconXSLTMapping()
        {
            try
            {
                InitializeComponent();
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



            // InitializeXSLTMappingTab();
        }

        //public void InitializeXSLTMappingTabForSelectedTemplate()
        //{
        //    try
        //    {
        //        ds = new DataSet();
        //        dtXSLTmapping = new DataTable();
        //        dtXSLTmapping.Columns.Add(COLUMN_FileName);
        //        dtXSLTmapping.Columns.Add(COLUMN_XsltPath);
        //        DataRow dr = dtXSLTmapping.NewRow();
        //        dtXSLTmapping.Rows.Add(dr);
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //private string GetFileName(string title)
        //{
        //    string strFileName = string.Empty;
        //    try
        //    {
        //        //browse desktop
        //        openFileDialog1.InitialDirectory = "\\\\tsclient\\C";
        //        openFileDialog1.Title = title;
        //        openFileDialog1.FileName = "";
        //        openFileDialog1.Filter = "XSLT Files (*.xslt)|*.xslt|All Files (*.*)|*.*";

        //        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //        {
        //            strFileName = openFileDialog1.FileName;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //    return strFileName;
        //}
        //get xslt file name
        private string GetXsltFileName(string title)
        {
            string strFileName = string.Empty;
            try
            {
                //if path is not null
                if (!string.IsNullOrWhiteSpace(XsltPath))
                {
                    openFileDialog1.InitialDirectory = XsltPath;
                }
                else
                {
                    //open the appliacation xslt path for the xslt mapping
                    openFileDialog1.InitialDirectory = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString();
                }
                openFileDialog1.Title = title;
                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "XSLT Files (*.xslt)|*.xslt|All Files (*.*)|*.*";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    strFileName = openFileDialog1.FileName;


                    string sourceFile = strFileName;
                    string fileName = Path.GetFileName(strFileName);
                    string destFile = Path.Combine(Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString(), fileName);
                    //check if user copies tries file to same folder
                    if (sourceFile != destFile)
                    {
                        if (!System.IO.Directory.Exists(Path.GetDirectoryName(destFile)))
                        {
                            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                        }
                        System.IO.File.Copy(sourceFile, destFile, true);
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
            return strFileName;
        }



        //private string GetPath(string fileName)
        //{
        //    return Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconRulesFile.ToString() + @"\" + fileName;
        //}

        //public void SaveXSLTMappingXML()
        //{
        //    ds.WriteXml(GetPath(XSLT_MAPPING_FILE));
        //}

        //private ValueList GetValueList(List<GenericNameID> list)
        //{
        //    ValueList coll = new ValueList();
        //    foreach (GenericNameID item in list)
        //    {
        //        if (item.ID != int.MinValue)
        //        {
        //            coll.ValueListItems.Add(item.Name.ToUpper(), item.Name.ToUpper());
        //        }
        //    }
        //    return coll;
        //}
        //private ValueList GetValueList(List<string> array)
        //{
        //    ValueList coll = new ValueList();
        //    coll.ValueListItems.Add(ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT);
        //    foreach (string name in array)
        //    {
        //        coll.ValueListItems.Add(name, name);
        //    }
        //    return coll;
        //}

        internal void LoadXSLTMapping(ReconTemplate template)
        {
            try
            {
                //if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare)
                //{
                //    rbCSVFormat.ForeColor = Color.White;
                //    rbXLSFormat.ForeColor = Color.White;
                //    rbTXTFormat.ForeColor = Color.White;
                //}
                _isControlInitialized = false;
                txtXsltFilePath.Text = Path.GetFileName(template.XsltPath);
                //set empty xslt path so that dont clash with other templates
                XsltPath = string.Empty;
                //check xslt file path is not empty and filepath also exists
                string reconXsltPath = Application.StartupPath + @"\" + ApplicationConstants.MAPPING_FILE_DIRECTORY + @"\" + ApplicationConstants.MappingFileType.ReconXSLT.ToString() + @"\" + template.XsltPath;
                if (!string.IsNullOrWhiteSpace(template.XsltPath) && File.Exists(reconXsltPath))
                {
                    //truncate directory path from where xslt file was picked
                    XsltPath = reconXsltPath.Substring(0, reconXsltPath.LastIndexOf("\\"));
                }
                txtReportSavePath.Text = template.ExceptionReportSavePath;
                txtImportPath.Text = template.ImportFileDetails.ImportFilePath;
                txtNamingConvention.Text = template.ImportFileDetails.NamingConvention;//  txtPassword.Text
                //if sp name is empty then load sp name from matching rules xml.
                if (string.IsNullOrWhiteSpace(template.SpName))
                {
                    DataRow[] result = template.DsMatchingRules.Tables[0].Select("Name='" + template.ReconType + "'");
                    foreach (DataRow row in result)
                    {
                        txtSpName.Text = row[COLUMN_SpName].ToString();
                    }
                }
                else
                {
                    txtSpName.Text = template.SpName;
                }

                switch (template.ImportFileDetails.FileFormat)
                {
                    case AutomationEnum.DataSourceFileFormat.Excel:
                        rbXLSFormat.Checked = true;
                        break;
                    case AutomationEnum.DataSourceFileFormat.Csv:
                        rbCSVFormat.Checked = true;
                        break;
                    case AutomationEnum.DataSourceFileFormat.Text:
                        rbTXTFormat.Checked = true;
                        break;
                    case AutomationEnum.DataSourceFileFormat.FixPlace:
                    case AutomationEnum.DataSourceFileFormat.Default:
                        break;
                    default:
                        break;
                }

                _isControlInitialized = true;

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
        /// <param name="template"></param>
        internal void UpdateXSLTMapping(ReconTemplate template)
        {

            try
            {
                ImportFileDetail fileDetail = new ImportFileDetail();
                //Narendra Jangir 2012/08/20
                //Allow import directory path to be empty
                //if (template.ImportFileDetails.ImportFilePath != txtImportPath.Text)
                //{
                fileDetail.ImportFilePath = txtImportPath.Text;
                //}
                //if (template.ImportFileDetails.NamingConvention != txtNamingConvention.Text)
                //{
                fileDetail.NamingConvention = txtNamingConvention.Text;
                //}
                //added isUseNamingConvention checkbox to follow or not naming convention
                //if (template.ImportFileDetails.IsUseNamingConvention != cbUseNamingConvention.Checked)
                //{
                //}
                //Allow exception directory path to empty, if path is empty than save exception report to default application path
                //if (template.ExceptionReportSavePath != txtReportSavePath.Text)
                //{
                template.ExceptionReportSavePath = txtReportSavePath.Text;
                //}
                //if (template.XsltPath != txtXsltFilePath.Text)
                //{
                template.XsltPath = txtXsltFilePath.Text;
                //}
                //if (template.ExceptionReportPassword != txtPassword.Text)
                //{
                template.ExceptionReportPassword = txtPassword.Text;
                template.SpName = txtSpName.Text;
                //}
                if (rbCSVFormat.Checked)
                {
                    fileDetail.FileFormat = AutomationEnum.DataSourceFileFormat.Csv;
                }
                if (rbXLSFormat.Checked)
                {
                    fileDetail.FileFormat = AutomationEnum.DataSourceFileFormat.Excel;
                }
                if (rbTXTFormat.Checked)
                {
                    fileDetail.FileFormat = AutomationEnum.DataSourceFileFormat.Text;
                }
                template.ImportFileDetails = fileDetail;
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

        private void ctrlReconXSLTMapping_Load(object sender, EventArgs e)
        {

            try
            {
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                btnBrowse.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnBrowse.ForeColor = System.Drawing.Color.White;
                btnBrowse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBrowse.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnBrowse.UseAppStyling = false;
                btnBrowse.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnBrowseExpReport.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnBrowseExpReport.ForeColor = System.Drawing.Color.White;
                btnBrowseExpReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBrowseExpReport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnBrowseExpReport.UseAppStyling = false;
                btnBrowseExpReport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnOpenXsltFile.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnOpenXsltFile.ForeColor = System.Drawing.Color.White;
                btnOpenXsltFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnOpenXsltFile.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnOpenXsltFile.UseAppStyling = false;
                btnOpenXsltFile.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnBrowseXsltFile.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnBrowseXsltFile.ForeColor = System.Drawing.Color.White;
                btnBrowseXsltFile.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnBrowseXsltFile.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnBrowseXsltFile.UseAppStyling = false;
                btnBrowseXsltFile.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
        /// <returns></returns>
        public bool IsUnsavedChanges()
        {
            try
            {
                if (_isUnSavedChanges)
                {
                    _isUnSavedChanges = false;
                    return true;
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
            return false;
        }
        //browse directory for import file
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                Control btn = sender as Control;

                if (folderDialog.ShowDialog(btn.FindForm()) == DialogResult.OK)
                {
                    txtImportPath.Text = folderDialog.SelectedPath;
                    if (_isControlInitialized)
                        _isUnSavedChanges = true;
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
        //browse directory for exception report file
        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseExpReport_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                Control btn = sender as Control;

                if (folderDialog.ShowDialog(btn.FindForm()) == DialogResult.OK)
                {
                    txtReportSavePath.Text = folderDialog.SelectedPath;
                    if (_isControlInitialized)
                        _isUnSavedChanges = true;
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtNamingConvention_ValueChanged(object sender, EventArgs e)
        {
            try
            {

                if (_isControlInitialized)
                    _isUnSavedChanges = true;
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
        private void txtImportPath_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnSavedChanges = true;
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
        private void txtReportSavePath_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnSavedChanges = true;
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

        private void txtPassword_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnSavedChanges = true;
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

        private void rbCSVFormat_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnSavedChanges = true;
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

        private void rbXLSFormat_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnSavedChanges = true;
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

        private void rbTXTFormat_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnSavedChanges = true;
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


        private void txtXsltFilePath_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnSavedChanges = true;
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

        private void btnBrowseXsltFile_Click(object sender, EventArgs e)
        {
            try
            {
                string title = "Select XSLT File for PB and Recon Type";
                string FilePath = GetXsltFileName(title);
                if (!String.IsNullOrEmpty(FilePath))
                {
                    txtXsltFilePath.Text = Path.GetFileName(FilePath);//shortName.Substring(shortName.LastIndexOf('\\') + 1);                   
                    if (_isControlInitialized)
                        _isUnSavedChanges = true;
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

        //private void cbUseNamingConvention_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (_isControlInitialized)
        //            _isUnSavedChanges = true;
        //    }
        //    catch (Exception ex)
        //    {
        // Invoke our policy that is responsible for making sure no secure information
        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void txtSpName_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_isControlInitialized)
                    _isUnSavedChanges = true;
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

        private void btnOpenXsltFile_Click(object sender, EventArgs e)
        {
            try
            {
                string reconXSLTPAth = Application.StartupPath + "\\MappingFiles\\ReconXSLT\\" + txtXsltFilePath.Text;
                if (File.Exists(reconXSLTPAth))
                {
                    //open file to edit or read
                    //System.Diagnostics.Process.Start("notepad.exe", txtXsltFilePath.Text);

                    System.Diagnostics.Process.Start(reconXSLTPAth);
                }
                else
                {
                    MessageBox.Show("Invalid File Path!!!", "File Open Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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


        //private void btnBrowseXsltLocation_Click(object sender, EventArgs e)
        //{
        //    FolderBrowserDialog folderDialog = new FolderBrowserDialog();
        //    Control btn = sender as Control;

        //    if (folderDialog.ShowDialog(btn.FindForm()) == DialogResult.OK)
        //    {
        //        //txtReportSavePath.Text = folderDialog.SelectedPath;
        //        return;
        //    }
        //}

    }


}
