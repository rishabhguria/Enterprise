using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Admin.Controls.ThirdParty
{
    public partial class FileSettingSetup : UserControl
    {
        #region GlobalVariables

        /// <summary>
        /// ID of the third party
        /// </summary>
        int _thirdPartyID = 0;

        /// <summary>
        /// Name of the third party
        /// </summary>
        string _thirdPartyName = string.Empty;
        /// <summary>
        /// Flag Variable to check if the data is to be saved
        /// </summary>
        public bool _isSaveRequired = false;

        /// <summary>
        /// Flag variable to check if there are duplicate rules in the grid  
        /// </summary>
        public bool _isValidData = true;

        int _accountsCount = -1;
        Dictionary<string, string> _accountsBeforeCellUpdate = new Dictionary<string, string>();

        #endregion

        #region ValueLists
        /// <summary>
        /// Value List for Import types
        /// </summary>
        ValueList _vlFormatType = new ValueList();

        /// <summary>
        /// Value List for Import types
        /// </summary>
        ValueList _vlImportType = new ValueList();

        /// <summary>
        /// Value List for Import names to be mapped with recon
        /// </summary>
        ValueList _vlImportFormatName = new ValueList();

        /// <summary>
        /// Value list for release Types
        /// </summary>
        ValueList _vlReleaseType = new ValueList();

        /// <summary>
        /// Value list for FTPs
        /// </summary>
        ValueList _vlFtp = new ValueList();

        /// <summary>
        /// valuelist for the mail Addresses
        /// </summary>
        ValueList _vlSendMail = new ValueList();

        /// <summary>
        /// Valuelist of mails that will be sent the logs
        /// </summary>
        ValueList _vlLogMail = new ValueList();

        /// <summary>
        /// Valuelist for Decryptions
        /// </summary>
        ValueList _vlDecryption = new ValueList();
        #endregion

        public FileSettingSetup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the control when it is loaded
        /// </summary>
        /// <param name="thirdPartyID">ID of the third party</param>
        public void InitializeControl(int thirdPartyID, string thirdPartyName)
        {
            try
            {
                if (thirdPartyID > 0)
                {
                    _thirdPartyID = thirdPartyID;
                    _thirdPartyName = thirdPartyName;
                    FileSettingManager.InitializeData();
                    //cmbAccount.DataSource = FileSettingManager.GetCurrentThirdPartyAccounts(thirdPartyID);
                    grdFileSetting.DataSource = FileSettingManager.GetFileSettingDetails(thirdPartyID);
                    SetGridReadOnlyRows();
                    SetAccountsEditorControl();
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

        private void SetAccountsEditorControl()
        {
            try
            {
                _accountsBeforeCellUpdate.Clear();

                foreach (UltraGridRow row in grdFileSetting.Rows)
                {
                    // Added by Bhavana for multiple clients
                    if (!string.IsNullOrWhiteSpace(row.Cells["ReleaseID"].Value.ToString()))
                    {
                        int releaseID = Convert.ToInt32(row.Cells["ReleaseID"].Value.ToString());
                        row.Cells["ClientID"].EditorComponent = EditorComponentForClientCell(releaseID);
                    }
                    if (!string.IsNullOrWhiteSpace(row.Cells["ReleaseID"].Value.ToString()) && !string.IsNullOrWhiteSpace(row.Cells["ClientID"].Value.ToString()))
                    {
                        List<int> clients = new List<int>();
                        List<object> listClients = (List<object>)row.Cells["ClientID"].Value;
                        foreach (int clientID in listClients)
                        {
                            if (!clients.Contains(clientID))
                            {
                                clients.Add(clientID);
                            }
                        }
                        int releaseID = Convert.ToInt32(row.Cells["ReleaseID"].Value.ToString());
                        row.Cells["AccountID"].EditorComponent = EditorComponentForCell(clients, releaseID);
                        //Modified By Faisal Shah 28/08/14
                        //Filling the dictionary with Accounts that are preselected for a Format Name
                        DataRow dr = (row.ListObject as DataRowView).Row;
                        if (FileSettingManager.IsBatchExecuted(dr))
                        {
                            string AccountsBeforeCellUpdate = string.Empty;
                            if (!string.IsNullOrEmpty(row.Cells["AccountID"].ToString()))
                                AccountsBeforeCellUpdate = row.Cells["AccountID"].Text.ToString();

                            if (!_accountsBeforeCellUpdate.ContainsKey(row.Cells["Formatname"].Text.ToString()))
                                _accountsBeforeCellUpdate.Add(row.Cells["Formatname"].Text.ToString(), AccountsBeforeCellUpdate);
                            else
                                _accountsBeforeCellUpdate[row.Cells["Formatname"].Text.ToString()] = AccountsBeforeCellUpdate;
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

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.ForeColor = System.Drawing.Color.White;
                this.grdFileSetting.DisplayLayout.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.grdFileSetting.DisplayLayout.Appearance.BorderColor = System.Drawing.SystemColors.InactiveCaption;
                this.grdFileSetting.DisplayLayout.Override.HeaderAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.grdFileSetting.DisplayLayout.Override.HeaderAppearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.grdFileSetting.DisplayLayout.Override.HeaderAppearance.ForeColor = System.Drawing.Color.White;
                this.grdFileSetting.DisplayLayout.Override.RowAppearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.grdFileSetting.DisplayLayout.Override.RowAppearance.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.grdFileSetting.DisplayLayout.Override.RowAppearance.ForeColor = System.Drawing.Color.White;
                this.grdFileSetting.DisplayLayout.Override.ActiveRowAppearance.BackColor = System.Drawing.Color.White;
                this.grdFileSetting.DisplayLayout.Override.ActiveRowAppearance.BackColor2 = System.Drawing.Color.White;
                this.grdFileSetting.DisplayLayout.Override.ActiveRowAppearance.ForeColor = System.Drawing.Color.Black;
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
        /// Create the Value list for the Import types
        /// </summary>
        private void MakeValueLists()
        {
            try
            {
                _vlFormatType.ValueListItems.Clear();
                _vlImportType.ValueListItems.Clear();
                _vlImportFormatName.ValueListItems.Clear();
                _vlReleaseType.ValueListItems.Clear();
                _vlFtp.ValueListItems.Clear();
                _vlSendMail.ValueListItems.Clear();
                _vlLogMail.ValueListItems.Clear();
                _vlDecryption.ValueListItems.Clear();

                _vlFormatType.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int formatType in FileSettingManager.dictFormatType.Keys)
                {
                    _vlFormatType.ValueListItems.Add(formatType, FileSettingManager.dictFormatType[formatType]);
                }

                _vlImportType.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int formatID in FileSettingManager.dictImport.Keys)
                {
                    _vlImportType.ValueListItems.Add(formatID, FileSettingManager.dictImport[formatID]);
                }

                _vlImportFormatName.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int fileSettingID in FileSettingManager.dictFileSetting.Keys)
                {
                    Dictionary<int, FileSettingItem> dictFileSetting = FileSettingManager.dictFileSetting;
                    if (FileSettingManager.dictFileSetting[fileSettingID].FormatType == "0" && FileSettingManager.dictFileSetting[fileSettingID].ImportTypeID == 1)
                    {
                        _vlImportFormatName.ValueListItems.Add(fileSettingID, FileSettingManager.dictFileSetting[fileSettingID].FormatName);
                    }
                }

                _vlReleaseType.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int releaseID in FileSettingManager.dictRelease.Keys)
                {
                    _vlReleaseType.ValueListItems.Add(releaseID, FileSettingManager.dictRelease[releaseID]);
                }

                _vlFtp.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int ftpID in FileSettingManager.dictFtp.Keys)
                {
                    _vlFtp.ValueListItems.Add(ftpID, FileSettingManager.dictFtp[ftpID]);
                }

                _vlSendMail.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int mailID in FileSettingManager.dictSendMail.Keys)
                {
                    _vlSendMail.ValueListItems.Add(mailID, FileSettingManager.dictSendMail[mailID]);
                }

                _vlLogMail.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int mailID in FileSettingManager.dictLogMail.Keys)
                {
                    _vlLogMail.ValueListItems.Add(mailID, FileSettingManager.dictLogMail[mailID]);
                }

                _vlDecryption.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int decryptionID in FileSettingManager.dictDecryption.Keys)
                {
                    _vlDecryption.ValueListItems.Add(decryptionID, FileSettingManager.dictDecryption[decryptionID]);
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
        /// Add new row to the Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //if (cmbAccount.DataSource != null)
                //{
                //if (!_isSaveRequired)
                //    _isSaveRequired = true;
                grdFileSetting.DisplayLayout.Bands[0].AddNew();
                grdFileSetting.ActiveRow.Cells["FormatType"].Value = int.MinValue;
                grdFileSetting.ActiveRow.Cells["ImportTypeID"].Value = int.MinValue;
                grdFileSetting.ActiveRow.Cells["ReleaseID"].Value = int.MinValue;
                //modified by amit on 28/04/2015
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3504
                grdFileSetting.ActiveRow.Cells["ClientID"].Activation = Activation.NoEdit;
                grdFileSetting.ActiveRow.Cells["AccountID"].Activation = Activation.NoEdit;
                //}
                //else
                //{
                //    MessageBox.Show("This Third Party does not have any permitted accounts","Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
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
        /// Delete the selected row from the grid 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdFileSetting.ActiveRow != null && grdFileSetting.ActiveRow.Cells["FormatName"].Activation == Activation.NoEdit)
                {
                    MessageBox.Show("The file setting cannot be deleted because\nrespective batch has already been executed.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (grdFileSetting.DataSource != null && grdFileSetting.ActiveRow != null)
                {
                    UltraGridRow row = grdFileSetting.ActiveRow;
                    if (!row.Cells["FormatName"].Text.Equals(string.Empty) || !row.Cells["ImportTypeID"].Text.Equals(ApplicationConstants.C_COMBO_SELECT) ||
                    !string.IsNullOrEmpty(row.Cells["AccountID"].Text))
                    {
                        DialogResult dr = MessageBox.Show("Do you want to delete the selected file setting?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                        {
                            return;
                        }
                        if (!_isSaveRequired)
                        {
                            _isSaveRequired = true;
                        }
                    }
                    grdFileSetting.ActiveRow.Delete(false);
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

        private void btnEmailSetting_Click(object sender, EventArgs e)
        {

        }

        private void btnFtpSetting_Click(object sender, EventArgs e)
        {
            //this.SaveFileSetting();// SaveFileSetting();
        }

        private void btnDecryptSetting_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Initialize the layout of the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFileSetting_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //define the multi select combo properties
            try
            {
                MakeValueLists();

                //if (cmbAccount.DataSource != null)
                //{
                //    if (!cmbAccount.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                //    {
                //        UltraGridColumn colSelectAccount = cmbAccount.DisplayLayout.Bands[0].Columns.Add();
                //        colSelectAccount.Key = "Selected";
                //        colSelectAccount.Header.Caption = string.Empty;
                //        colSelectAccount.Width = 25;
                //        colSelectAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                //        colSelectAccount.DataType = typeof(bool);
                //        colSelectAccount.Header.VisiblePosition = 1;
                //    }
                //    cmbAccount.CheckedListSettings.CheckStateMember = "Selected";
                //    cmbAccount.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                //    cmbAccount.CheckedListSettings.ListSeparator = " , ";
                //    cmbAccount.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                //    cmbAccount.DisplayMember = "AccountName";
                //    cmbAccount.ValueMember = "AccountID";
                //    cmbAccount.NullText = ApplicationConstants.C_COMBO_SELECT;
                //    cmbAccount.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                //    cmbAccount.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                //}

                //Grid layout 
                UltraGridBand band = e.Layout.Bands[0];
                band.Override.AllowRowFiltering = DefaultableBoolean.True;

                foreach (UltraGridColumn column in band.Columns)
                {
                    //following line auto adjust width of columns of ultragrid accocrding to text size of header.
                    column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                }

                int i = 1;
                UltraGridColumn colIsActive = band.Columns["IsActive"];
                colIsActive.Header.Caption = "Active";
                colIsActive.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                colIsActive.DefaultCellValue = true;
                colIsActive.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colIsActive.Header.VisiblePosition = i++;

                if (band.Columns.Exists("BatchStartDate"))
                {
                    UltraGridColumn colBatchDate = band.Columns["BatchStartDate"];
                    colBatchDate.Header.Caption = "Batch Start Date";
                    colBatchDate.Header.VisiblePosition = i++;
                    colBatchDate.Nullable = Infragistics.Win.UltraWinGrid.Nullable.Null;
                }
                UltraGridColumn colFormatName = band.Columns["FormatName"];
                colFormatName.Header.Caption = "Format Name";
                colFormatName.Header.VisiblePosition = i++;

                UltraGridColumn colFormatType = band.Columns["FormatType"];
                colFormatType.Header.Caption = "Format Type";
                colFormatType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colFormatType.ValueList = _vlFormatType;
                colFormatType.Header.VisiblePosition = i++;

                UltraGridColumn colImportFormatName = band.Columns["ImportFormatID"];
                colImportFormatName.Header.Caption = "Reference Format Name";
                colImportFormatName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colImportFormatName.ValueList = _vlImportFormatName;
                colImportFormatName.Header.VisiblePosition = i++;

                UltraGridColumn colImportType = band.Columns["ImportTypeID"];
                colImportType.Header.Caption = "Import Type";
                colImportType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colImportType.ValueList = _vlImportType;
                colImportType.Header.VisiblePosition = i++;

                UltraGridColumn colReleaseName = band.Columns["ReleaseID"];
                colReleaseName.Header.Caption = "Release Name";
                colReleaseName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colReleaseName.ValueList = _vlReleaseType;
                colReleaseName.Header.VisiblePosition = i++;

                UltraGridColumn colClientList = band.Columns["ClientID"];
                colClientList.Header.Caption = "Client Name";
                //colClientList.CellActivation = Activation.NoEdit;
                colClientList.Header.VisiblePosition = i++;
                //colClientList.Hidden = true;

                UltraGridColumn colAccount = band.Columns["AccountID"];
                colAccount.Header.Caption = "Account Name";
                //colAccount.EditorComponent = cmbAccount;
                colAccount.Header.VisiblePosition = i++;

                UltraGridColumn colXslt = band.Columns["XSLTPath"];
                colXslt.Header.Caption = "XSLT Name";
                colXslt.Header.VisiblePosition = i++;
                colXslt.CellActivation = Activation.NoEdit;

                if (!band.Columns.Exists("SelectXslt"))
                {
                    band.Columns.Add("SelectXslt");
                }
                UltraGridColumn colBtnXslt = band.Columns["SelectXslt"];
                colBtnXslt.Header.Caption = "Select Xslt";
                colBtnXslt.NullText = "Select";
                colBtnXslt.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colBtnXslt.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colBtnXslt.Header.VisiblePosition = i++;


                UltraGridColumn colXsd = band.Columns["XSDPath"];
                colXsd.Header.Caption = "XSD Name";
                colXsd.Header.VisiblePosition = i++;
                colXsd.CellActivation = Activation.NoEdit;

                if (!band.Columns.Exists("SelectXsd"))
                {
                    band.Columns.Add("SelectXsd");
                }
                UltraGridColumn colBtnXsd = band.Columns["SelectXsd"];
                colBtnXsd.Header.Caption = "Select Xsd";
                colBtnXsd.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                colBtnXsd.NullText = "Select";
                colBtnXsd.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                colBtnXsd.Header.VisiblePosition = i++;


                UltraGridColumn colImportSp = band.Columns["ImportSPName"];
                colImportSp.Header.Caption = "Import SP Name";
                colImportSp.Header.VisiblePosition = i++;

                UltraGridColumn colFTPFolder = band.Columns["FTPFolderPath"];
                colFTPFolder.Header.Caption = "FTP File Path";
                colFTPFolder.Header.VisiblePosition = i++;

                UltraGridColumn colLocalFolder = band.Columns["LocalFolderPath"];
                colLocalFolder.Header.Caption = "Local Folder Path";
                colLocalFolder.CellActivation = Activation.NoEdit;
                colLocalFolder.Header.VisiblePosition = i++;

                //UltraGridColumn colImportFile = grdBand.Columns["ImportFileName"];
                //colImportFile.Header.Caption = "File Name";
                //colImportFile.Header.VisiblePosition = i++;

                UltraGridColumn colFtp = band.Columns["FtpID"];
                colFtp.Header.Caption = "FTP";
                colFtp.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colFtp.ValueList = _vlFtp;
                colFtp.DefaultCellValue = int.MinValue;
                colFtp.Header.VisiblePosition = i++;

                UltraGridColumn colEmailName = band.Columns["EMailID"];
                colEmailName.Header.Caption = "Email Data Name";
                colEmailName.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colEmailName.ValueList = _vlSendMail;
                colEmailName.DefaultCellValue = int.MinValue;
                colEmailName.Header.VisiblePosition = i++;

                UltraGridColumn colEmailLog = band.Columns["EMailLogID"];
                colEmailLog.Header.Caption = "Email Log name";
                colEmailLog.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colEmailLog.DefaultCellValue = int.MinValue;
                colEmailLog.ValueList = _vlLogMail;
                colEmailLog.Header.VisiblePosition = i++;

                UltraGridColumn colDecryption = band.Columns["DecryptionID"];
                colDecryption.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colDecryption.ValueList = _vlDecryption;
                colDecryption.DefaultCellValue = int.MinValue;
                colDecryption.Header.Caption = "Decryption";
                colDecryption.Header.VisiblePosition = i++;

                if (band.Columns.Exists("PriceToleranceColumns"))
                {
                    UltraGridColumn colPriceToleranceColumn = band.Columns["PriceToleranceColumns"];
                    colPriceToleranceColumn.Header.Caption = "Price Tolerance Columns";
                    //modified By: Bharat raturi, 05-may-2014
                    //purpose: hide the column price tolerance columns for time being
                    //colPriceToleranceColumn.Header.VisiblePosition = i++;
                    colPriceToleranceColumn.Hidden = true;
                }
                band.Columns["SettingID"].Hidden = true;
                band.Columns["ThirdPartyID"].Hidden = true;
                band.Columns["ThirdPartyID"].NullText = _thirdPartyID.ToString();
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
        /// added by: Bharat Raturi, 16 jun 2014
        /// make the grid rows read only if the respective batched have been executed at least once
        /// </summary>
        private void SetGridReadOnlyRows()
        {
            foreach (UltraGridRow row in grdFileSetting.Rows)
            {

                DataRow dr = (row.ListObject as DataRowView).Row;
                if (FileSettingManager.IsBatchExecuted(dr))
                {
                    foreach (UltraGridCell cell in row.Cells)
                    {
                        if (!cell.Column.Key.Equals("EmailID") && !cell.Column.Key.Equals("EmailLogID") && !cell.Column.Key.Equals("DecryptionID") && !cell.Column.Key.Equals("AccountID") && !cell.Column.Key.Equals("FTPFolderPath"))
                        {
                            cell.Activation = Activation.NoEdit;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the file from the system when user clicks the cell button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFileSetting_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                if (string.Equals(e.Cell.Column.Key, "SelectXslt"))
                {
                    string title = "Select XSLT ";
                    string buttonKey = "SelectXslt";
                    string fileWithPath = GetFileName(title, buttonKey);
                    if (!String.IsNullOrEmpty(fileWithPath))
                    {
                        if (!_isSaveRequired)
                        {
                            _isSaveRequired = true;
                        }
                        string releaseName = grdFileSetting.ActiveRow.Cells["ReleaseID"].Text;
                        if (FileSettingManager.DictCommanyDetails.Keys.Contains(releaseName))
                        {
                            string targetPath = string.Empty;

                            if (e.Cell.Row.Cells["FormatType"].Text == FormatType.Recon.ToString())
                            {
                                targetPath = FileSettingManager.DictCommanyDetails[releaseName].ReleasePath + "\\MappingFiles\\ReconXSLT";
                            }
                            else
                            {
                                targetPath = FileSettingManager.DictCommanyDetails[releaseName].ReleasePath + "\\Incoming\\" + _thirdPartyName + "\\TransformationFiles";
                            }
                            //copy xslt file from the openfile directory to location fetched from db t_CompanyReleaseDetails
                            string sourceFile = fileWithPath;
                            string fileName = System.IO.Path.GetFileName(fileWithPath);
                            string destFile = System.IO.Path.Combine(targetPath, fileName);
                            //check if user copies tries file to same folder
                            if (sourceFile != destFile)
                            {
                                if (!System.IO.Directory.Exists(targetPath))
                                {
                                    System.IO.Directory.CreateDirectory(targetPath);
                                }
                                System.IO.File.Copy(sourceFile, destFile, true);
                            }
                            grdFileSetting.ActiveRow.Cells["XSLTPath"].Value = fileName;
                        }
                    }
                }
                else if (string.Equals(e.Cell.Column.Key, "SelectXsd"))
                {
                    string title = "Select XSD ";
                    string buttonKey = "SelectXsd";
                    string fileWithPath = GetFileName(title, buttonKey);
                    if (!String.IsNullOrEmpty(fileWithPath))
                    {
                        if (!_isSaveRequired)
                        {
                            _isSaveRequired = true;
                        }
                        //copy xslt file from the openfile directory to location fetched from db t_CompanyReleaseDetails    
                        string releaseName = grdFileSetting.ActiveRow.Cells["ReleaseID"].Text;
                        if (FileSettingManager.DictCommanyDetails.Keys.Contains(releaseName))
                        {
                            string targetPath = FileSettingManager.DictCommanyDetails[releaseName].ReleasePath + "\\Incoming\\" + _thirdPartyName + "\\ValidationFiles";
                            string sourceFile = fileWithPath;
                            string fileName = System.IO.Path.GetFileName(fileWithPath);
                            string destFile = System.IO.Path.Combine(targetPath, fileName);
                            //check if user copies tries file to same folder
                            if (sourceFile != destFile)
                            {
                                if (!System.IO.Directory.Exists(targetPath))
                                {
                                    System.IO.Directory.CreateDirectory(targetPath);
                                }
                                System.IO.File.Copy(sourceFile, destFile, true);
                            }
                            grdFileSetting.ActiveRow.Cells["XSDPath"].Value = fileName;
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

        /// <summary>
        /// Get the name of the file from the open file dialog button
        /// </summary>
        /// <param name="title">The caption of the button</param>
        /// <param name="buttonKey">The key of the button</param>
        /// <returns>The file name of the selected file</returns>
        private string GetFileName(string title, string buttonKey)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.InitialDirectory = "DeskTop";
                openFileDialog1.Title = title;
                if (buttonKey == "SelectXslt")
                {
                    openFileDialog1.Filter = "XSLT Files (*.xslt)|*.xslt";
                }
                else if (buttonKey == "SelectXsd")
                {
                    openFileDialog1.Filter = "XSD Files (*.xsd)|*.xsd";
                }
                string strFileName = string.Empty;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    strFileName = openFileDialog1.FileName;
                }
                return strFileName;
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
            return string.Empty;
        }

        /// <summary>
        /// Populate the clients for the selected release
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFileSetting_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (!_isSaveRequired)
                {
                    _isSaveRequired = true;
                }
                if (e.Cell.Column.Key == "ReleaseID")
                {
                    UltraGridColumn gridColumn = e.Cell.Column;
                    String ColumnText = e.Cell.Text;
                    EmbeddableEditorBase editor = e.Cell.EditorResolved;
                    object changedValue = editor.IsValid ? editor.Value : editor.CurrentEditText;
                    UltraGridRow actRow = grdFileSetting.ActiveRow;
                    int i = Convert.ToInt32(changedValue);
                    if (i != int.MinValue)
                    {
                        //modified by amit on 28/04/2015
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3504
                        grdFileSetting.ActiveRow.Cells["ClientID"].Activation = Activation.AllowEdit;
                        grdFileSetting.ActiveRow.Cells["AccountID"].Activation = Activation.AllowEdit;
                        grdFileSetting.ActiveRow.Cells["ClientID"].Value = FileSettingManager.GetClientsForRelease(i);
                    }
                    grdFileSetting.ActiveRow.Cells["ClientID"].EditorComponent = EditorComponentForClientCell(i);
                    grdFileSetting.ActiveRow.Cells["AccountID"].EditorComponent = EditorComponentForCell(null, i);
                }
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-2380
                // the code was commented, as it was forcing the client drop down to exit the edit mode, and due to this, 
                // user was able to select only one client.
                //else if (e.Cell.Column.Key == "ClientID")
                //{
                //    grdFileSetting.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.ExitEditMode);
                //}

                //disable the import type if formattype for the cell is recon
                else if (e.Cell.Column.Key == "FormatType")
                {
                    if (e.Cell.Text == Prana.BusinessObjects.AppConstants.FormatType.Recon.ToString())
                    {
                        string releaseName = e.Cell.Row.Cells["ReleaseID"].Text;
                        if (FileSettingManager.DictCommanyDetails.Keys.Contains(releaseName))
                        {
                            e.Cell.Row.Cells["LocalFolderPath"].Value = FileSettingManager.DictCommanyDetails[releaseName].ReleasePath + "\\Incoming\\" + _thirdPartyName + "\\DataFiles\\Recon\\";
                        }
                        e.Cell.Row.Cells["ImportTypeID"].Value = int.MinValue;
                        e.Cell.Row.Cells["ImportTypeID"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                    }
                    else
                    {
                        e.Cell.Row.Cells["ImportFormatID"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                        e.Cell.Row.Cells["ImportTypeID"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                    }
                    e.Cell.Row.Cells["ImportFormatID"].Value = int.MinValue;
                }
                //Added By Faisal Shah 28/08/14
                // Need to Make Sure that Accounts associated with Batch cannot be unselected. However New Accounts can be Selected
                else if (e.Cell.Column.Key == "AccountID")
                {
                    if (_accountsBeforeCellUpdate.ContainsKey(e.Cell.Row.Cells["FormatName"].Text.ToString()))
                    {
                        string AccountsAfterCellUpdate = e.Cell.Text;
                        string AccountsBeforeCellUpdate = _accountsBeforeCellUpdate[e.Cell.Row.Cells["FormatName"].Text.ToString()];
                        if (!AccountsAfterCellUpdate.Contains(AccountsBeforeCellUpdate))
                        {
                            e.Cell.CancelUpdate();
                            MessageBox.Show("You can not remove accounts associated with an executed batch.\n However you can add new accounts", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                }
                else if (e.Cell.Column.Key == "ImportFormatID")
                {
                    if (e.Cell.Text.Equals(ApplicationConstants.C_COMBO_SELECT))
                    {
                        foreach (UltraGridCell cell in e.Cell.Row.Cells)
                        {
                            if (cell.Column.Key == "ClientID" || cell.Column.Key == "ReleaseID" || cell.Column.Key == "AccountID" || cell.Column.Key == "FTPFolderPath" || cell.Column.Key == "FtpID" || cell.Column.Key == "EmailID" || cell.Column.Key == "EmailLogID" || cell.Column.Key == "DecryptionID" || cell.Column.Key == "ThirdPartyID" || cell.Column.Key == "BatchStartDate")
                            {
                                cell.Activation = Activation.AllowEdit;
                            }
                        }
                    }
                    else
                    {
                        //copy the values of import settingd to recon settings
                        foreach (UltraGridRow row in grdFileSetting.Rows)
                        {

                            if (row.Cells.Exists("FormatName") && row.Cells["FormatName"].Value != null
                                && row.Cells["FormatName"].Value.ToString().Equals(e.Cell.Text, StringComparison.InvariantCultureIgnoreCase))
                            {
                                CopyUltraGridCellValue(e.Cell.Row, row, "ReleaseID", false);
                                CopyUltraGridCellValue(e.Cell.Row, row, "ThirdPartyID", false);
                                CopyUltraGridCellValue(e.Cell.Row, row, "BatchStartDate", false);
                                if (CheckValidCellExist(e.Cell.Row, row, "ReleaseID"))
                                {
                                    int i = Convert.ToInt32(row.Cells["ReleaseID"].Value.ToString());
                                    if (i != int.MinValue)
                                    {
                                        e.Cell.Row.Cells["ClientID"].Value = FileSettingManager.GetClientsForRelease(i);
                                    }
                                    e.Cell.Row.Cells["ClientID"].EditorComponent = EditorComponentForClientCell(i);
                                    e.Cell.Row.Cells["AccountID"].EditorComponent = EditorComponentForCell(null, i);
                                }
                                CopyUltraGridCellValue(e.Cell.Row, row, "ClientID", true);
                                CopyUltraGridCellValue(e.Cell.Row, row, "AccountID", true);
                                if (CheckValidCellExist(e.Cell.Row, row, "FTPFolderPath") && CheckValidCellExist(e.Cell.Row, row, "LocalFolderPath"))
                                {
                                    e.Cell.Row.Cells["FTPFolderPath"].Value = Path.Combine(row.Cells["LocalFolderPath"].Value.ToString(),
                                        Path.Combine("ProcessedData", Path.GetFileName(row.Cells["FTPFolderPath"].Value.ToString())));
                                    e.Cell.Row.Cells["FTPFolderPath"].Activation = Activation.NoEdit;
                                }
                                UpdateUltraGridCellValueToNull(e.Cell.Row, "FtpID");
                                UpdateUltraGridCellValueToNull(e.Cell.Row, "EmailID");
                                UpdateUltraGridCellValueToNull(e.Cell.Row, "EmailLogID");
                                UpdateUltraGridCellValueToNull(e.Cell.Row, "DecryptionID");

                                break;
                            }
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

        private bool CheckValidCellExist(UltraGridRow row1, UltraGridRow row2, string p)
        {
            try
            {
                if (row1.Cells.Exists("ReleaseID") && row1.Cells["ReleaseID"].Value != null
                                            && row2.Cells.Exists("ReleaseID") && row2.Cells["ReleaseID"].Value != null)
                {
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



        /// <summary>
        /// update the grid cell value to another cell value of column
        /// </summary>
        /// <param name="row"></param>
        /// <param name="rowToBeCopied"></param>
        /// <param name="columnName"></param>
        /// <param name="isSetNull"></param>
        private void CopyUltraGridCellValue(UltraGridRow row, UltraGridRow rowToBeCopied, string columnName, bool _isEditorComponentToBeCopied)
        {
            try
            {
                if (CheckValidCellExist(row, rowToBeCopied, columnName))
                {
                    if (_isEditorComponentToBeCopied)
                    {
                        row.Cells[columnName].EditorComponent = rowToBeCopied.Cells[columnName].EditorComponent;
                    }
                    row.Cells[columnName].Value = rowToBeCopied.Cells[columnName].Value;
                    row.Cells[columnName].Activation = Activation.NoEdit;
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
        ///  update the grid cell value to null
        /// </summary>
        /// <param name="row"></param>
        /// <param name="rowToBeCopied"></param>
        /// <param name="columnName"></param>
        /// <param name="isSetNull"></param>
        private void UpdateUltraGridCellValueToNull(UltraGridRow row, string columnName)
        {
            try
            {
                if (row.Cells.Exists(columnName) && row.Cells[columnName].Value != null)
                {
                    row.Cells[columnName].Value = int.MinValue;
                    row.Cells[columnName].Activation = Activation.NoEdit;
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

        private UltraCombo EditorComponentForCell(List<int> clients, int releaseID)
        {
            UltraCombo cmbCell = new UltraCombo();
            try
            {
                DataTable accounts = FileSettingManager.GetCurrentThirdPartyClientAccounts(_thirdPartyID, clients, releaseID);
                cmbCell.DataSource = accounts;
                _accountsCount = accounts.Rows.Count;
                if (cmbCell.DataSource != null)
                {
                    if (!cmbCell.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn colSelectAccount = cmbCell.DisplayLayout.Bands[0].Columns.Add();
                        colSelectAccount.Key = "Selected";
                        colSelectAccount.Header.Caption = string.Empty;
                        colSelectAccount.Width = 25;
                        colSelectAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        colSelectAccount.DataType = typeof(bool);
                        colSelectAccount.Header.VisiblePosition = 1;
                    }
                    cmbCell.CheckedListSettings.CheckStateMember = "Selected";
                    cmbCell.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbCell.CheckedListSettings.ListSeparator = " , ";
                    cmbCell.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    cmbCell.DisplayMember = "AccountName";
                    cmbCell.ValueMember = "AccountID";
                    cmbCell.NullText = ApplicationConstants.C_COMBO_SELECT;
                    cmbCell.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                    cmbCell.DisplayLayout.Bands[0].Columns[0].Hidden = true;
                    cmbCell.AfterDropDown += new System.EventHandler(cmbAccount_AfterDropDown);
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
            return cmbCell;
        }

        private UltraCombo EditorComponentForClientCell(int releaseID)
        {
            UltraCombo cmbCell = new UltraCombo();
            try
            {
                cmbCell.DataSource = FileSettingManager.GetClientsForRelease(releaseID);
                if (cmbCell.DataSource != null)
                {
                    if (!cmbCell.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn colSelectClient = cmbCell.DisplayLayout.Bands[0].Columns.Add();
                        colSelectClient.Key = "Selected";
                        colSelectClient.Header.Caption = string.Empty;
                        colSelectClient.Width = 25;
                        colSelectClient.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        colSelectClient.DataType = typeof(bool);
                        colSelectClient.Header.VisiblePosition = 1;
                    }
                    cmbCell.CheckedListSettings.CheckStateMember = "Selected";
                    cmbCell.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbCell.CheckedListSettings.ListSeparator = " , ";
                    cmbCell.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    cmbCell.DisplayMember = "CompanyName";
                    cmbCell.ValueMember = "CompanyID";
                    cmbCell.NullText = ApplicationConstants.C_COMBO_SELECT;
                    cmbCell.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                    cmbCell.DisplayLayout.Bands[0].Columns[0].Hidden = true;
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
            return cmbCell;
        }

        /// <summary>
        /// Save the details of the setting inthe database
        /// </summary>
        /// <returns>Third party ID if the records are saved</returns>
        public int SaveFileSetting()
        {
            try
            {
                //if (_isSaveRequired)
                //{
                //    _isSaveRequired = false;
                //}
                //else
                if (grdFileSetting.DisplayLayout.Bands[0].Override.AllowUpdate == DefaultableBoolean.False)
                {
                    return _thirdPartyID;
                }
                if (!_isSaveRequired)
                {
                    return _thirdPartyID;
                }
                if (grdFileSetting.DataSource != null)
                {
                    DataTable dt = (DataTable)grdFileSetting.DataSource;
                    if (FileSettingManager.HasEmpty(dt, _thirdPartyID))
                    {
                        MessageBox.Show("Blank details for file setting cannot be inserted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _isValidData = false;
                        return -1;
                    }

                    foreach (UltraGridRow Row in grdFileSetting.Rows)
                    {

                        if (string.IsNullOrEmpty(Row.Cells["ClientID"].Text) || string.IsNullOrEmpty(Row.Cells["AccountID"].Text))
                        {
                            MessageBox.Show("Blank details for file setting cannot be inserted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            _isValidData = false;
                            return -1;
                        }
                        //Modified by sachin mishra Jira-CHMW-2818
                        string Accounts = string.Empty;
                        Accounts = Row.Cells["AccountID"].Text.ToString();

                        string[] accountcheck = Accounts.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (accountcheck.Count() >= 0)
                        {
                            if (accountcheck.Count() == 0)
                            {
                                MessageBox.Show("Blank details for file setting cannot be inserted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                _isValidData = false;
                                return -1;
                            }
                            else
                            {
                                int count = 0;
                                foreach (string account in accountcheck)
                                {
                                    if (string.IsNullOrWhiteSpace(account))
                                    {
                                        count++;
                                    }

                                }
                                if (accountcheck.Count() == count)
                                {
                                    MessageBox.Show("Blank details for file setting cannot be inserted.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    _isValidData = false;
                                    return -1;
                                }
                            }
                        }


                    }
                    // Added By: Bharat Raturi, 22 may 2014
                    // purpose: check for the invalid format names
                    if (InvalidFormatName())
                    {
                        MessageBox.Show("Format Name can only have letters, numbers and underscores.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _isValidData = false;
                        return -1;
                    }
                    int i = FileSettingManager.SaveFileSetting(dt, _thirdPartyID);
                    if (i > 0)
                    {
                        _isSaveRequired = false;
                        _isValidData = true;
                        return _thirdPartyID;
                    }
                    else if (i == -1)
                    {
                        MessageBox.Show("Duplicate Settings for file cannot be saved.\nMake sure that two different settings do not have same combination of third party, account name and import type", "Duplicate File Setting Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _isValidData = false;
                        return -1;// _thirdPartyID;
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
            return 0;
        }

        /// <summary>
        /// Added By: Bharat Raturi, 22 may 2014
        /// purpose: check for the invalid format names
        /// </summary>
        /// <returns></returns>
        private bool InvalidFormatName()
        {
            foreach (UltraGridRow row in grdFileSetting.Rows)
            {
                if (!string.IsNullOrWhiteSpace(row.Cells["FormatName"].Text.ToString()))
                {
                    if (!Regex.IsMatch(row.Cells["FormatName"].Text.ToString(), "^[a-zA-Z0-9_]+$"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// if the user does not want to save the changes, refresh the grid from the database
        /// </summary>
        public void RollBackChanges()
        {
            try
            {
                _isSaveRequired = false;
                this.InitializeControl(_thirdPartyID, _thirdPartyName);
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

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Refersh the value lists after the advance setting are updated
        /// </summary>
        internal void RefreshAdvanceSettings()
        {
            try
            {
                FileSettingManager.RefreshAdvanceDetails();
                _vlFtp.ValueListItems.Clear();
                _vlSendMail.ValueListItems.Clear();
                _vlLogMail.ValueListItems.Clear();
                _vlDecryption.ValueListItems.Clear();

                _vlFtp.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int ftpID in FileSettingManager.dictFtp.Keys)
                {
                    _vlFtp.ValueListItems.Add(ftpID, FileSettingManager.dictFtp[ftpID]);
                }

                _vlSendMail.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int mailID in FileSettingManager.dictSendMail.Keys)
                {
                    _vlSendMail.ValueListItems.Add(mailID, FileSettingManager.dictSendMail[mailID]);
                }

                _vlLogMail.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int mailID in FileSettingManager.dictLogMail.Keys)
                {
                    _vlLogMail.ValueListItems.Add(mailID, FileSettingManager.dictLogMail[mailID]);
                }

                _vlDecryption.ValueListItems.Add(int.MinValue, ApplicationConstants.C_COMBO_SELECT);
                foreach (int decryptionID in FileSettingManager.dictDecryption.Keys)
                {
                    _vlDecryption.ValueListItems.Add(decryptionID, FileSettingManager.dictDecryption[decryptionID]);
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
        /// changes the local folder Path column and sets the ImportTypeID column to no edit mode if the format Type is Recon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFileSetting_InitializeRow(object sender, InitializeRowEventArgs e)
        {

            string releaseName = e.Row.Cells["ReleaseID"].Text;

            //added by: Bharat raturi, 08-may-2014
            //purpose: to provide variables for storing the values of local folder paths 
            string currentLocalPath = string.Empty;
            string newLocalPath = string.Empty;

            //disable the import type if formattype for the cell is recon
            if (e.Row.Cells["FormatType"].Text == Prana.BusinessObjects.AppConstants.FormatType.Recon.ToString())
            {

                //changes the LocalFolderPath to Release path from DictCommanyDetails 
                if (FileSettingManager.DictCommanyDetails.Keys.Contains(releaseName))
                {
                    //modified by: Bharat Raturi, 08-may-2014
                    //purpose: to match the local folder paths 
                    if (!string.IsNullOrEmpty(e.Row.Cells["LocalFolderPath"].Value.ToString()))
                    {
                        currentLocalPath = e.Row.Cells["LocalFolderPath"].Value.ToString();
                    }
                    newLocalPath = FileSettingManager.DictCommanyDetails[releaseName].ReleasePath + "\\Incoming\\" + _thirdPartyName + "\\DataFiles\\Recon\\";
                    if (!currentLocalPath.Equals(newLocalPath))
                    {
                        _isSaveRequired = true;
                        e.Row.Cells["LocalFolderPath"].Value = FileSettingManager.DictCommanyDetails[releaseName].ReleasePath + "\\Incoming\\" + _thirdPartyName + "\\DataFiles\\Recon\\";
                    }
                }
                e.Row.Cells["ImportTypeID"].Value = int.MinValue;
                e.Row.Cells["ImportTypeID"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

                e.Row.Cells["ImportFormatID"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
            }
            else
            {
                //changes the LocalFolderPath to Release path from DictCommanyDetails 
                if (FileSettingManager.DictCommanyDetails.Keys.Contains(releaseName))
                {
                    //copy xslt file from the openfile directory to location fetched from db t_CompanyReleaseDetails
                    if (FileSettingManager.DictImportAcronym.Keys.Contains(Convert.ToInt32(e.Row.Cells["ImportTypeID"].Value)))
                    {
                        string importAcronym = FileSettingManager.DictImportAcronym[Convert.ToInt32(e.Row.Cells["ImportTypeID"].Value)];
                        currentLocalPath = e.Row.Cells["LocalFolderPath"].Value.ToString();
                        newLocalPath = FileSettingManager.DictCommanyDetails[releaseName].ReleasePath + "\\Incoming\\" + _thirdPartyName + "\\DataFiles\\" + importAcronym + "\\";
                        if (!currentLocalPath.Equals(newLocalPath))
                        {
                            _isSaveRequired = true;
                            e.Row.Cells["LocalFolderPath"].Value = FileSettingManager.DictCommanyDetails[releaseName].ReleasePath + "\\Incoming\\" + _thirdPartyName + "\\DataFiles\\" + importAcronym + "\\";
                        }
                    }
                }
                e.Row.Cells["ImportTypeID"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;

                e.Row.Cells["ImportFormatID"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
            }
        }

        /// <summary>
        /// Event fired when cell value change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdFileSetting_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            {
                //Added by sachin mishra Purpose: http://jira.nirvanasolutions.com:8080/browse/PRANA-8326
                if (e.Cell.Row.Cells["ImportFormatID"].Activation == Activation.Disabled)
                {
                    e.Cell.Row.Cells["ClientID"].Activation = Activation.AllowEdit;
                    e.Cell.Row.Cells["AccountID"].Activation = Activation.AllowEdit;
                    e.Cell.Row.Cells["ReleaseID"].Activation = Activation.AllowEdit;
                    e.Cell.Row.Cells["ThirdPartyID"].Activation = Activation.AllowEdit;
                    e.Cell.Row.Cells["BatchStartDate"].Activation = Activation.AllowEdit;
                    e.Cell.Row.Cells["FtpID"].Activation = Activation.AllowEdit;
                    e.Cell.Row.Cells["EmailID"].Activation = Activation.AllowEdit;
                    e.Cell.Row.Cells["EmailLogID"].Activation = Activation.AllowEdit;
                    e.Cell.Row.Cells["DecryptionID"].Activation = Activation.AllowEdit;
                }
                if (e.Cell.Column.Key == "ClientID" && e.Cell.IsActiveCell)
                {
                    UltraGridRow row = e.Cell.Row;
                    if (row.Cells["ClientID"].Value != null)
                    {
                        List<object> listClient = (List<object>)row.Cells["ClientID"].Value;
                        int releaseID = Convert.ToInt32(row.Cells["ReleaseID"].Value.ToString());
                        List<int> clients = new List<int>();
                        foreach (int clientID in listClient)
                        {
                            if (!clients.Contains(clientID))
                            {
                                clients.Add(clientID);
                            }
                        }
                        row.Cells["AccountID"].EditorComponent = EditorComponentForCell(clients, releaseID);
                    }
                }
                //Added By Faisal Shah Dated 06/08/14
                //Needed to restrict same Format Names
                else if (e.Cell.Column.Key == "FormatName" && e.Cell.IsActiveCell)
                {
                    string formatName = e.Cell.Row.Cells["FormatName"].Text.ToString();
                    //FormatType formatType;
                    //Enum.TryParse(e.Cell.Row.Cells["FormatType"].Text.ToString(), out formatType);

                    foreach (UltraGridRow row in grdFileSetting.Rows)
                    {
                        //Skipping the check for Same row
                        if (e.Cell.Row.Index != row.Index)
                        {
                            //FormatType rowFormatType;
                            //Enum.TryParse(row.Cells["FormatType"].Text, out rowFormatType);
                            ////Skipping Check if we do not have a Format Type Selected
                            //if (e.Cell.Row.Cells["FormatType"].Text.ToString() != ApplicationConstants.C_COMBO_SELECT)
                            //{
                            //Checking For Duplicacy
                            if (!String.IsNullOrEmpty(formatName) && row.Cells["FormatName"].Text.ToString().Equals(formatName))
                            {
                                MessageBox.Show("Format Name  cannot be same for two batches", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                e.Cell.Row.Cells["FormatName"].Value = string.Empty;
                                break;
                            }
                            // }
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

        /// <summary>
        /// added by: Bharat Raturi, 16 jun 2014
        /// make the controls read only if the user does not have write permission
        /// </summary>
        /// <param name="isActive"></param>
        public void SetGridAccess(bool isActive)
        {
            try
            {
                if (!isActive)
                {
                    grdFileSetting.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.False;
                    //grdPermissions.DisplayLayout.Bands[0].Override.AllowAddNew = DefaultableBoolean.False;
                    //grdPermissions.DisplayLayout.Bands[0].Override.AllowDelete = DefaultableBoolean.False;
                    btnAdd.Enabled = btnDelete.Enabled = false;
                }
                else
                {
                    grdFileSetting.DisplayLayout.Bands[0].Override.AllowUpdate = DefaultableBoolean.True;
                    btnAdd.Enabled = btnDelete.Enabled = true;
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

        private void cmbAccount_AfterDropDown(object sender, EventArgs e)
        {
            try
            {
                if (_accountsCount == 0)
                {
                    MessageBox.Show("No accounts are associated with the selected third party.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}