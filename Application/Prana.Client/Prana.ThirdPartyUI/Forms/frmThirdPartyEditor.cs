using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.ThirdParty;
using Prana.BusinessObjects.Enumerators;
using Prana.ClientCommon;
using Prana.LogManager;
using Prana.ThirdPartyUI.Forms;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mail;
using System.Windows.Forms;

namespace Prana.ThirdPartyUI
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public partial class frmThirdPartyEditor : Form, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Forms.Form"/> class.
        /// </summary>
        /// <remarks></remarks>
        public frmThirdPartyEditor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the frmThirdPartyEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void frmThirdPartyEditor_Load(object sender, EventArgs e)
        {
            try
            {
                toleranceProfileAddNewItem.Visible = false;
                if (!CustomThemeHelper.ApplyTheme)
                {
                    SetAppearanceWithoutTheme();
                }
                else
                {
                    SetAppearanceWithTheme();
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                    CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLE_NAME_THIRD_PARTY);
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

        private void SetAppearanceWithTheme()
        {
            try
            {
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();

                this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
                this.bindingNavigator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
                this.bindingNavigatorPositionItem.BackColor = System.Drawing.Color.WhiteSmoke;
                this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.btnEmailSettings.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.btnFtpSettings.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.btnGnuPGSettings.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.btnJobSettings.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);
                this.btnToleranceProfileSettings.ImageTransparentColor = System.Drawing.Color.FromArgb(209, 210, 212);

                this.dataView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));

                this.dataView.GridColor = System.Drawing.Color.FromArgb(88, 88, 90);

                dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
                dataGridViewCellStyle1.ForeColor = System.Drawing.Color.WhiteSmoke;
                this.dataView.RowsDefaultCellStyle = dataGridViewCellStyle1;
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

        private void SetAppearanceWithoutTheme()
        {
            try
            {
                System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();

                this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                this.bindingNavigator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                this.bindingNavigatorPositionItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
                this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.btnEmailSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.btnFtpSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.btnGnuPGSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.btnJobSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.btnToleranceProfileSettings.ImageTransparentColor = System.Drawing.Color.Magenta;

                this.dataView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

                this.dataView.GridColor = System.Drawing.Color.Gray;

                dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
                this.dataView.RowsDefaultCellStyle = dataGridViewCellStyle1;
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
        /// Method to check if all details in ftp is valid or not.
        /// </summary>
        /// <param name="ftp"></param>
        /// <returns></returns>
        private bool IsValidFTP(ThirdPartyFtp ftp, out string message)
        {
            bool isValid = false;
            message = "";
            try
            {
                if (string.IsNullOrEmpty(ftp.FtpName) || string.IsNullOrWhiteSpace(ftp.FtpName))
                {
                    message += " FtpName,";
                }
                if (string.IsNullOrEmpty(ftp.FtpType) || string.IsNullOrWhiteSpace(ftp.FtpType))
                {
                    message += " FtpType,";
                }
                else
                {
                    if (!string.IsNullOrEmpty(ftp.FtpName) && !string.IsNullOrWhiteSpace(ftp.FtpName))
                    {
                        switch (ftp.FtpType)
                        {
                            case "FTP":
                                if (string.IsNullOrEmpty(ftp.Host) || string.IsNullOrWhiteSpace(ftp.Host))
                                {
                                    message += " Host,";
                                }
                                if (string.IsNullOrEmpty(ftp.UserName) || string.IsNullOrWhiteSpace(ftp.UserName))
                                {
                                    message += " UserName,";
                                }
                                if (string.IsNullOrEmpty(ftp.Password) || string.IsNullOrWhiteSpace(ftp.Password))
                                {
                                    message += " Password,";
                                }
                                else if (!string.IsNullOrEmpty(ftp.Host) && !string.IsNullOrWhiteSpace(ftp.Host) && !string.IsNullOrEmpty(ftp.UserName) && !string.IsNullOrWhiteSpace(ftp.UserName))
                                {
                                    isValid = true;
                                }
                                break;
                            case "SCP":
                                if (string.IsNullOrEmpty(ftp.Host) || string.IsNullOrWhiteSpace(ftp.Host))
                                {
                                    message += " Host,";
                                }
                                if (string.IsNullOrEmpty(ftp.KeyFingerPrint) || string.IsNullOrWhiteSpace(ftp.KeyFingerPrint))
                                {
                                    message += " KeyFingerPrint,";
                                }
                                else if (!string.IsNullOrEmpty(ftp.Host) && !string.IsNullOrWhiteSpace(ftp.Host))
                                {
                                    isValid = true;
                                }
                                break;
                            case "SFTP":
                            case "SFTPPasswordLess":
                                if (string.IsNullOrEmpty(ftp.KeyFingerPrint) || string.IsNullOrWhiteSpace(ftp.KeyFingerPrint))
                                {
                                    message += " KeyFingerPrint,";
                                }
                                else if (!string.IsNullOrEmpty(ftp.Host) && !string.IsNullOrWhiteSpace(ftp.Host))
                                {
                                    isValid = true;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return isValid;
        }

        /// <summary>
        /// Saves the row.
        /// </summary>
        /// <param name="RowIndex">Index of the row.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool SaveRow(int RowIndex)
        {
            try
            {
                bindingSource1.EndEdit();

                ThirdPartyFtp ftp = dataView.Rows[RowIndex].DataBoundItem as ThirdPartyFtp;
                if (ftp != null)
                {
                    string message;
                    if (!IsValidFTP(ftp, out message))
                        MessageBox.Show("Please set" + message.Trim(','));
                    else
                        ftp.FtpId = ThirdPartyClientManager.ServiceInnerChannel.SaveThirdPartyFtp(ftp);
                }

                ThirdPartyGnuPG gnuPG = dataView.Rows[RowIndex].DataBoundItem as ThirdPartyGnuPG;
                if (gnuPG != null)
                {
                    if (!string.IsNullOrEmpty(gnuPG.GnuPGName) && !string.IsNullOrWhiteSpace(gnuPG.GnuPGName))
                        gnuPG.GnuPGId = ThirdPartyClientManager.ServiceInnerChannel.SaveThirdPartyGnuPG(gnuPG);
                    else
                    {
                        MessageBox.Show("Please enter valid GnuPG Name!");
                        return false;
                    }
                }

                ThirdPartyBatch batch = dataView.Rows[RowIndex].DataBoundItem as ThirdPartyBatch;
                if (batch != null)
                {
                    if (!string.IsNullOrEmpty(batch.Description) && !string.IsNullOrWhiteSpace(batch.Description))
                    {
                        if (ThirdPartyClientManager.ServiceInnerChannel.CheckDuplicateBatch(batch))
                        {
                            batch.ThirdPartyBatchId = ThirdPartyClientManager.ServiceInnerChannel.SaveThirdPartyBatch(batch);
                        }
                        else
                        {
                            MessageBox.Show("Can not insert duplicate job. Another job with same parameters already exists.");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter Description!");
                        return false;
                    }
                }

                ThirdPartyEmail email = dataView.Rows[RowIndex].DataBoundItem as ThirdPartyEmail;
                if (email != null)
                {
                    if (!string.IsNullOrEmpty(email.EmailName) && !string.IsNullOrWhiteSpace(email.EmailName)
                        && !string.IsNullOrEmpty(email.MailFrom) && !string.IsNullOrWhiteSpace(email.MailFrom)
                         && !string.IsNullOrEmpty(email.MailTo) && !string.IsNullOrWhiteSpace(email.MailTo)
                         && !string.IsNullOrEmpty(email.Smtp) && !string.IsNullOrWhiteSpace(email.Smtp)
                        && !string.IsNullOrEmpty(Convert.ToString(email.Port)) && !string.IsNullOrWhiteSpace(Convert.ToString(email.Port)))
                    {
                        email.EmailId = ThirdPartyClientManager.ServiceInnerChannel.SaveThirdPartyEmail(email);
                    }
                    else
                    {
                        MessageBox.Show("Please check following fields!"
                                            + Environment.NewLine + "1.EmailName"
                                            + Environment.NewLine + "2.MailFrom"
                                            + Environment.NewLine + "3.MailTo"
                                            + Environment.NewLine + "4.SMTP"
                                            + Environment.NewLine + "5.Port!!!");
                        return false;
                    }
                }


                dataView.Refresh();
                Console.Beep(1000, 10);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }
        /// <summary>
        /// Handles the RowValidating event of the dataGridView1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellCancelEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void dataGridView1_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (dataView.IsCurrentRowDirty)
                {
                    SaveRow(e.RowIndex);
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

        /// <summary>
        /// Handles the Click event of the bindingNavigatorDeleteItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            if (dataView.Rows.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to delete the selected row?", "Confirm Delete",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No) return;

            int rowIndex = dataView.CurrentCell.RowIndex;

            try
            {
                ThirdPartyToleranceProfile toleranceProfile = dataView.Rows[rowIndex].DataBoundItem as ThirdPartyToleranceProfile;
                if (toleranceProfile != null)
                { 
                    DeleteRow(toleranceProfile.ToleranceProfileId); 
                    return; 
                }

                ThirdPartyFtp ftp = dataView.Rows[rowIndex].DataBoundItem as ThirdPartyFtp;
                if (ftp != null)
                { 
                    DeleteRow(ftp);
                    return;
                }

                ThirdPartyGnuPG gnuPG = dataView.Rows[rowIndex].DataBoundItem as ThirdPartyGnuPG;
                if (gnuPG != null)
                { 
                    DeleteRow(gnuPG);
                    return;
                }

                ThirdPartyBatch batch = dataView.Rows[rowIndex].DataBoundItem as ThirdPartyBatch;
                if (batch != null) 
                { 
                    DeleteRow(batch);
                    return;
                }

                ThirdPartyEmail email = dataView.Rows[rowIndex].DataBoundItem as ThirdPartyEmail;
                if (email != null) 
                { 
                    DeleteRow(email);
                    return;
                }
            }
            #region Catch
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
            #endregion

        }

        /// <summary>
        /// Deletes the row.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="batch">The batch.</param>
        /// <remarks></remarks>
        private void DeleteRow(ThirdPartyBatch batch)
        {
            try
            {
                bindingSource1.EndEdit();

                ThirdPartyClientManager.ServiceInnerChannel.DeleteThirdPartyBatch(batch);
                dataView.Rows.Remove(dataView.CurrentRow);
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
        /// Delete the Tolerance Profile row
        /// </summary>
        /// <param name="toleranceProfileId"></param>
        private void DeleteRow(int toleranceProfileId)
        {
            try
            {
                bindingSource1.EndEdit();

                ThirdPartyClientManager.ServiceInnerChannel.DeleteThirdPartyToleranceProfile(toleranceProfileId);
                dataView.Rows.Remove(dataView.CurrentRow);

                // Set the current cell to the cell in column 2, Row 0. Column 1 is  hidden so can't set to that.
                if (dataView.Rows.Count > 0) dataView.CurrentCell = dataView[2, 0];
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
        /// Deletes the row.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="ftp">The FTP.</param>
        /// <remarks></remarks>
        private void DeleteRow(ThirdPartyFtp ftp)
        {
            try
            {
                bindingSource1.EndEdit();

                if (ThirdPartyClientManager.ServiceInnerChannel.DeleteThirdPartyFtp(ftp) != 0)
                    MessageBox.Show("Can't delete Item while item is in use by ThirdPartyBatch");
                else
                    dataView.Rows.Remove(dataView.CurrentRow);
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
        /// Deletes the row.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="gnuPG">The gnu PG.</param>
        /// <remarks></remarks>
        private void DeleteRow(ThirdPartyGnuPG gnuPG)
        {
            try
            {
                bindingSource1.EndEdit();

                if (ThirdPartyClientManager.ServiceInnerChannel.DeleteThirdPartyGnuPG(gnuPG) != 0)
                    MessageBox.Show("Can't delete Item while item is in use by ThirdPartyBatch");
                else
                    dataView.Rows.Remove(dataView.CurrentRow);
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
        /// Deletes the row.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="email">The email.</param>
        /// <remarks></remarks>
        private void DeleteRow(ThirdPartyEmail email)
        {
            try
            {
                bindingSource1.EndEdit();

                if (ThirdPartyClientManager.ServiceInnerChannel.DeleteThirdPartyEmail(email) != 0)
                    MessageBox.Show("Can't delete Item while item is in use by ThirdPartyBatch");
                else
                    dataView.Rows.Remove(dataView.CurrentRow);
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

        /// <summary>
        /// Handles the Click event of the btnFtp control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnFtpSettings_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.DataSource = null;
                dataView.AllowUserToAddRows = true;
                dataView.DataSource = bindingSource1;
                dataView.Columns.Clear();
                List<ThirdPartyFtp> items = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyFtps();

                if (items.Count > 0)
                    bindingSource1.DataSource = items;
                else
                    bindingSource1.DataSource = new ThirdPartyFtp();

                CustomizeDataViewForFTPSettings();

                dataView.Update();
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

        private void DataView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && (e.KeyCode == Keys.C))
                {
                    foreach (DataGridViewCell cell in ((DataGridView)(sender)).SelectedCells)
                    {
                        if (cell.OwningColumn.Name.Contains("Pass"))
                        {
                            e.SuppressKeyPress = true;
                            break;
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
        /// Customizes the data view for FTP settings.
        /// </summary>
        private void CustomizeDataViewForFTPSettings()
        {
            try
            {
                bindingNavigatorAddNewItem.Visible = true;
                toleranceProfileAddNewItem.Visible = false;

                dataView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataView.Columns["FtpId"].Visible = false;

                if (dataView.Columns.Contains("SshPrivateKeyPath"))
                    dataView.Columns["SshPrivateKeyPath"].HeaderText = "PrivateKeyPath";

                if (dataView.Columns.Contains("FtpType"))
                    dataView.Columns["FtpType"].Visible = false;

                if (!dataView.Columns.Contains("FtpType2"))
                {
                    DataGridViewComboBoxColumn cmbCol = new DataGridViewComboBoxColumn();
                    cmbCol.HeaderText = "FtpType";
                    cmbCol.Name = "FtpType2";
                    cmbCol.Items.Add("True");
                    cmbCol.DataSource = GetFtpTypeList();
                    dataView.Columns.Add(cmbCol);
                }

                if (dataView.Columns.Contains("FtpType2"))
                {
                    dataView.Columns["FtpType2"].DisplayIndex = 8;

                    foreach (DataGridViewRow row in dataView.Rows)
                    {
                        row.Cells["FtpType2"].Value = row.Cells["FtpType"].Value;
                    }
                }

                if (dataView.Columns.Contains("Encryption"))
                    dataView.Columns["Encryption"].Visible = false;
                if (!dataView.Columns.Contains("Encryption2"))
                {
                    DataGridViewComboBoxColumn cmbCol = new DataGridViewComboBoxColumn();
                    cmbCol.HeaderText = "Encryption";
                    cmbCol.Name = "Encryption2";
                    cmbCol.Items.Add("True");
                    cmbCol.DataSource = GetEncryptionTypeList();
                    dataView.Columns.Add(cmbCol);
                }
                if (dataView.Columns.Contains("Encryption2"))
                {
                    dataView.Columns["Encryption2"].DisplayIndex = 5;
                    foreach (DataGridViewRow row in dataView.Rows)
                    {
                        row.Cells["Encryption2"].Value = row.Cells["Encryption"].Value;
                        if (row.Cells["FtpType2"].Value == null || !row.Cells["FtpType2"].Value.ToString().Equals("FTP", StringComparison.OrdinalIgnoreCase))
                        {
                            row.Cells["Encryption2"].ReadOnly = true;
                            row.Cells["Encryption2"].Value = "";

                        }
                        else
                            row.Cells["Encryption2"].ReadOnly = false;
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnGnuPG control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnGnuPGSettings_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.DataSource = null;
                dataView.AllowUserToAddRows = true;
                dataView.DataSource = bindingSource1;
                dataView.Columns.Clear();
                List<ThirdPartyGnuPG> items = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyGnuPG();

                if (items.Count > 0)
                    bindingSource1.DataSource = items;
                else
                    bindingSource1.DataSource = new ThirdPartyGnuPG();

                bindingNavigatorAddNewItem.Visible = true;
                toleranceProfileAddNewItem.Visible = false;
                dataView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataView.Update();
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

        /// <summary>
        /// Handles the Click event of the btnBatch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnJobSettings_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.DataSource = null;
                dataView.AllowUserToAddRows = true;
                dataView.DataSource = bindingSource1;
                dataView.Columns.Clear();

                var items = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyBatch();

                if (items.Count > 0)
                    bindingSource1.DataSource = items;
                else
                    bindingSource1.DataSource = new ThirdPartyBatch();

                CustomizeDataViewForJobSettings();
                dataView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataView.Update();
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
        /// <summary>
        /// Handles the Click event of the Tolerance Profile Add control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toleranceProfileAddNewItem_Click(object sender, EventArgs e)
        {
            try
            {
                ThirdPartyToleranceProfile thirdPartyToleranceProfile = new ThirdPartyToleranceProfile();
                ToleranceProfile toleranceProfile = new ToleranceProfile();
                toleranceProfile.InitializeDataSource(thirdPartyToleranceProfile, true);
                toleranceProfile.updateToleranceProfileGrid += btnToleranceProfileSettings_Click;
                toleranceProfile.ShowDialog();
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

        /// <summary>
        /// Handles the Click event of the Tolerance Profile control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnToleranceProfileSettings_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.DataSource = null;
                dataView.AllowUserToAddRows = false;
                dataView.DataSource = bindingSource1;
                dataView.Columns.Clear();

                var items = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyToleranceProfiles();

                if (items.Count > 0)
                    bindingSource1.DataSource = items;
                else
                {
                    bindingSource1.DataSource = new ThirdPartyToleranceProfile();
                    dataView.Rows.Remove(dataView.CurrentRow);
                }

                CustomizeDataViewForToleranceProfile();
                dataView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataView.Update();
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

        /// <summary>
        /// This Method is to customize data view for Tolerance Profile
        /// </summary>
        private void CustomizeDataViewForToleranceProfile()
        {
            try
            {
                bindingNavigatorAddNewItem.Visible = false;
                toleranceProfileAddNewItem.Visible = true;

                //Hide the columns which are not required to Show but need to pass on Tolerance Profile
                dataView.Columns["ToleranceProfileId"].Visible = false;
                dataView.Columns["ThirdPartyBatchId"].Visible = false;

                //Set Order of Columns for dataGridView
                dataView.Columns["JobName"].DisplayIndex = 0; 
                dataView.Columns["ExecutingBroker"].DisplayIndex = 1;
                dataView.Columns["ThirdPartyName"].DisplayIndex = 2;
                dataView.Columns["LastModified"].DisplayIndex = 3;
                dataView.Columns["MatchingField"].DisplayIndex = 4;
                dataView.Columns["AvgPrice"].DisplayIndex = 5;
                dataView.Columns["NetMoney"].DisplayIndex = 6;
                dataView.Columns["Commission"].DisplayIndex = 7;
                dataView.Columns["MiscFees"].DisplayIndex = 8;

                //make all rows apart from edit button row Non editable
                for (int i = 0; i < dataView.Rows.Count; i++)
                    dataView.Rows[i].ReadOnly = true;

                // Add a button column to the DataGridView.
                DataGridViewButtonColumn button = new DataGridViewButtonColumn();
                {
                    button.Name = "EditTolerance";
                    button.HeaderText = "EditTolerance";
                    button.Text = "Edit";
                    button.UseColumnTextForButtonValue = true;
                    this.dataView.Columns.Add(button);
                }

                // Set the current cell to the cell in column 2, Row 0. Column 1 is  hidden so can't set to that.
                if(dataView.Rows.Count > 0) dataView.CurrentCell = dataView[2, 0];
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// This Method is to customize data view for job settings
        /// </summary>
        private void CustomizeDataViewForJobSettings()
        {
            try
            {
                bindingNavigatorAddNewItem.Visible = true;
                toleranceProfileAddNewItem.Visible = false;

                dataView.Columns["TRANSMISSIONTYPE"].Visible = false;
                dataView.Columns["FIXCONNECTIONSTATUS"].Visible = false;
                dataView.Columns["BROKERCONNECTIONTYPE"].Visible = false;
                dataView.Columns["ALLOCATIONMATCHSTATUS"].Visible = false;

                foreach (DataGridViewRow row in dataView.Rows)
                {
                    var allowedFixTransmission = row.Cells["AllowedFixTransmission"].Value;
                    var checkBox = new DataGridViewCheckBoxCell();
                    row.Cells["AllowedFixTransmission"] = checkBox;
                    if (allowedFixTransmission == null)
                    {
                        checkBox.FlatStyle = FlatStyle.Flat;
                        checkBox.Style.ForeColor = Color.DarkGray;
                        checkBox.ReadOnly = true;
                    }
                    else
                    {
                        checkBox.Value = (bool)allowedFixTransmission;
                    }
                }

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }


        /// <summary>
        /// Handles the Click event of the btnEmailSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnEmailSettings_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.DataSource = null;
                dataView.AllowUserToAddRows = true;
                dataView.DataSource = bindingSource1;
                dataView.Columns.Clear();

                List<ThirdPartyEmail> items = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyEmails();

                if (items.Count > 0)
                    bindingSource1.DataSource = items;
                else
                    bindingSource1.DataSource = new ThirdPartyEmail();

                bindingNavigatorAddNewItem.Visible = true;
                toleranceProfileAddNewItem.Visible = false;
                dataView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                dataView.Update();
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

        /// <summary>
        /// Handles the Click event of the toolStripButton1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void btnAutoCreate(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to auto generate records?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No) return;

            try
            {
                ThirdPartyClientManager.ServiceInnerChannel.AutoCreateBatchEntries();
                btnJobSettings_Click(this, EventArgs.Empty);
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



        /// <summary>
        /// Handles the DataError event of the dataView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewDataErrorEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void dataView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void CellEnter(ThirdPartyGnuPG gnuPG, DataGridViewCellEventArgs e)
        {
            try
            {
                Rectangle rect = dataView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex + 1, false);

                comboBox1.Top = rect.Top + 2;
                comboBox1.Left = rect.Left;
                comboBox1.Width = rect.Width;
                comboBox1.Height = rect.Height + 1;

                if (dataView.CurrentCell.OwningColumn.DataPropertyName == "VerboseLevel")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = new EnumToDataSet<VerboseLevel>("VerboseId", "Verbose");
                    comboBox1.ValueMember = "VerboseId";
                    comboBox1.DisplayMember = "Verbose";
                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "Command")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = new EnumToDataSet<Commands>("CommandId", "Command");
                    comboBox1.ValueMember = "CommandId";
                    comboBox1.DisplayMember = "Command";
                }

                if (dataView.CurrentCell.Value == null)
                    comboBox1.SelectedIndex = -1;
                else
                {
                    comboBox1.SelectedIndex = comboBox1.FindString(dataView.CurrentCell.Value.ToString());

                }
            }
            #region Catch
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
            #endregion


        }
        /// <summary>
        /// Cells the enter.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void CellEnter(DataGridViewCellEventArgs e)
        {
            try
            {
                Rectangle rect = dataView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex + 1, false);

                comboBox1.Top = rect.Top + 2;
                comboBox1.Left = rect.Left;
                comboBox1.Width = rect.Width;
                comboBox1.Height = rect.Height + 1;

                if (dataView.CurrentCell.OwningColumn.DataPropertyName == "Priority")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = new EnumToDataSet<MailPriority>("PriorityId", "Priority");
                    comboBox1.ValueMember = "PriorityId";
                    comboBox1.DisplayMember = "Priority";
                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "MailType")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = new EnumToDataSet<MailType>("MailTypeId", "MailType");
                    comboBox1.ValueMember = "MailTypeId";
                    comboBox1.DisplayMember = "MailType";
                }

                if (dataView.CurrentCell.Value == null)
                    comboBox1.SelectedIndex = -1;
                else
                {
                    comboBox1.SelectedIndex = comboBox1.FindString(dataView.CurrentCell.Value.ToString());

                }
            }
            #region Catch
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
            #endregion
        }
        /// <summary>
        /// Cells the enter.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void CellEnter(ThirdPartyBatch batch, DataGridViewCellEventArgs e)
        {
            try
            {

                Rectangle rect = dataView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex + 1, false);

                comboBox1.Top = rect.Top + 2;
                comboBox1.Left = rect.Left;
                comboBox1.Width = rect.Width;
                comboBox1.Height = rect.Height + 1;

                if (dataView.CurrentCell.OwningColumn.DataPropertyName == "GnuPGName")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = GetThirdPartyGnuPGs();
                    comboBox1.ValueMember = "GnuPGId";
                    comboBox1.DisplayMember = "GnuPGName";
                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "FtpName")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = GetThirdPartyFtps();
                    comboBox1.ValueMember = "FtpId";
                    comboBox1.DisplayMember = "FtpName";
                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "ThirdPartyName")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = ThirdPartyClientManager.ServiceInnerChannel.GetThirdParties(batch);
                    comboBox1.ValueMember = "ThirdPartyId";
                    comboBox1.DisplayMember = "ThirdPartyName";

                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "ThirdPartyTypeName")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyTypes();
                    comboBox1.ValueMember = "ThirdPartyTypeId";
                    comboBox1.DisplayMember = "ThirdPartyTypeName";

                }

                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "FileFormatName")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyFormats(batch);
                    comboBox1.ValueMember = "FileFormatId";
                    comboBox1.DisplayMember = "FileFormatName";

                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "EmailDataName")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = GetThirdPartyDataEmail();
                    comboBox1.ValueMember = "EmailId";
                    comboBox1.DisplayMember = "EmailName";
                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "EmailLogName")
                {
                    comboBox1.Visible = true;
                    comboBox1.DataSource = GetThirdPartyLogEmail();
                    comboBox1.ValueMember = "EmailId";
                    comboBox1.DisplayMember = "EmailName";
                }

                if (dataView.CurrentCell.Value == null)
                    comboBox1.SelectedIndex = -1;
                else
                    comboBox1.SelectedIndex = comboBox1.FindString(dataView.CurrentCell.Value.ToString());
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
        /// Handles the CellEnter event of the dataView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void dataView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                comboBox1.Visible = false;

                ThirdPartyBatch batch = dataView.CurrentRow.DataBoundItem as ThirdPartyBatch;
                if (batch != null)
                    CellEnter(batch, e);

                ThirdPartyEmail email = dataView.CurrentRow.DataBoundItem as ThirdPartyEmail;
                if (email != null)
                    CellEnter(e);

                ThirdPartyGnuPG gnuPG = dataView.CurrentRow.DataBoundItem as ThirdPartyGnuPG;
                if (gnuPG != null)
                    CellEnter(gnuPG, e);
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

        /// <summary>
        /// Handles the CellFormatting event of the dataView control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dataView.Columns[e.ColumnIndex].Name == "MatchingField")
                {
                    if (e.GetType() != null)
                    {
                        e.Value = (int)e.Value == (int)MatchingField.ToleranceInValue ? "Tolerance in Value" : "Tolerance in Percentage";
                    }
                }
                if (dataView.Columns[e.ColumnIndex].Name == "LastModified")
                {
                    if (e.GetType() != null)
                    {
                        e.Value = e.Value.ToString();
                    }
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

        /// <summary>
        /// Gets the third party gnu P gs.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private ThirdPartyGnuPGs GetThirdPartyGnuPGs()
        {

            ThirdPartyGnuPGs items = new ThirdPartyGnuPGs();
            try
            {
                List<ThirdPartyGnuPG> gnupgs = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyGnuPG();
                items.Add(new ThirdPartyGnuPG());
                foreach (ThirdPartyGnuPG gnupg in gnupgs)
                {
                    items.Add(gnupg);
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
            return items;
        }
        /// <summary>
        /// Gets the third party FTPS.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private ThirdPartyFtps GetThirdPartyFtps()
        {
            ThirdPartyFtps items = new ThirdPartyFtps();
            try
            {
                List<ThirdPartyFtp> ftps = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyFtps();

                items.Add(new ThirdPartyFtp());
                foreach (ThirdPartyFtp ftp in ftps)
                {
                    items.Add(ftp);
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
            return items;
        }
        /// <summary>
        /// Gets the third party data email.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private ThirdPartyEmails GetThirdPartyDataEmail()
        {
            ThirdPartyEmails items = new ThirdPartyEmails();
            try
            {
                List<ThirdPartyEmail> thirdPartyEmails = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyDataEmail();

                items.Add(new ThirdPartyEmail());
                foreach (ThirdPartyEmail email in thirdPartyEmails)
                {
                    items.Add(email);
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
            return items;
        }

        /// <summary>
        /// Gets the third party log email.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        private ThirdPartyEmails GetThirdPartyLogEmail()
        {
            ThirdPartyEmails items = new ThirdPartyEmails();
            try
            {
                List<ThirdPartyEmail> thirdPartyEmails = ThirdPartyClientManager.ServiceInnerChannel.GetThirdPartyLogEmail();

                items.Add(new ThirdPartyEmail());
                foreach (ThirdPartyEmail email in thirdPartyEmails)
                {
                    items.Add(email);
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
            return items;
        }


        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Debug.Print(comboBox1.Text);
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

        /// <summary>
        /// Selections the change committed.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <remarks></remarks>
        private void SelectionChangeCommitted(ThirdPartyBatch batch)
        {
            try
            {
                int RowIndex = dataView.CurrentRow.Index;

                if (batch == null) return;
                int? value = comboBox1.SelectedValue as int?;
                if (value == null) return;

                if (dataView.CurrentCell.OwningColumn.DataPropertyName == "GnuPGName")
                {
                    int id = batch.GnuPGId;
                    string sid = batch.GnuPGName;

                    batch.GnuPGId = (int)comboBox1.SelectedValue;
                    batch.GnuPGName = comboBox1.Text;
                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        batch.GnuPGId = id;
                        batch.GnuPGName = sid;
                    }
                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "FtpName")
                {
                    int id = batch.FtpId;
                    string sid = batch.FtpName;

                    batch.FtpId = (int)comboBox1.SelectedValue;
                    batch.FtpName = comboBox1.Text;
                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        batch.FtpId = id;
                        batch.FtpName = sid;
                    }
                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "ThirdPartyName")
                {
                    int id = batch.ThirdPartyId;
                    string sid = batch.ThirdPartyName;

                    batch.ThirdPartyId = (int)comboBox1.SelectedValue;
                    batch.ThirdPartyName = comboBox1.Text;
                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        batch.ThirdPartyId = id;
                        batch.ThirdPartyName = sid;
                    }
                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "ThirdPartyTypeName")
                {
                    int id = batch.ThirdPartyTypeId;
                    string sid = batch.ThirdPartyTypeName;

                    batch.ThirdPartyTypeId = (int)comboBox1.SelectedValue;
                    batch.ThirdPartyTypeName = comboBox1.Text;

                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        batch.ThirdPartyTypeId = id;
                        batch.ThirdPartyTypeName = sid;
                    }
                }

                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "FileFormatName")
                {
                    int id = batch.ThirdPartyFormatId;
                    string sid = batch.FileFormatName;

                    batch.ThirdPartyFormatId = (int)comboBox1.SelectedValue;
                    batch.FileFormatName = comboBox1.Text;

                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        batch.ThirdPartyFormatId = id;
                        batch.FileFormatName = sid;
                    }
                }

                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "EmailLogName")
                {
                    int id = batch.EmailLogId;
                    string sid = batch.EmailLogName;

                    batch.EmailLogId = (int)comboBox1.SelectedValue;
                    batch.EmailLogName = comboBox1.Text;

                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        batch.EmailLogId = id;
                        batch.EmailLogName = sid;
                    }
                }

                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "EmailDataName")
                {
                    int id = batch.EmailDataId;
                    string sid = batch.EmailDataName;

                    batch.EmailDataId = (int)comboBox1.SelectedValue;
                    batch.EmailDataName = comboBox1.Text;

                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        batch.EmailDataId = id;
                        batch.EmailDataName = sid;
                    }
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

        /// <summary>
        /// Selections the change committed.
        /// </summary>
        /// <param name="gnuPG">The gnu PG.</param>
        /// <remarks></remarks>
        private void SelectionChangeCommitted(ThirdPartyGnuPG gnuPG)
        {
            try
            {
                int RowIndex = dataView.CurrentRow.Index;

                if (gnuPG == null) return;
                int? value = comboBox1.SelectedValue as int?;
                if (value == null) return;

                if (dataView.CurrentCell.OwningColumn.DataPropertyName == "VerboseLevel")
                {
                    VerboseLevel id = gnuPG.VerboseLevel;
                    string sid = Enum.GetName(typeof(VerboseLevel), id);

                    gnuPG.VerboseLevel = (VerboseLevel)(int)comboBox1.SelectedValue;

                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        gnuPG.VerboseLevel = id;
                    }
                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "Command")
                {
                    Commands id = gnuPG.Command;
                    string sid = Enum.GetName(typeof(MailPriority), id);

                    gnuPG.Command = (Commands)(int)comboBox1.SelectedValue;

                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        gnuPG.Command = id;
                    }
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
        /// <summary>
        /// Selections the change committed.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <remarks></remarks>
        private void SelectionChangeCommitted(ThirdPartyEmail email)
        {
            try
            {
                int RowIndex = dataView.CurrentRow.Index;

                if (email == null) return;
                int? value = comboBox1.SelectedValue as int?;
                if (value == null) return;

                if (dataView.CurrentCell.OwningColumn.DataPropertyName == "MailType")
                {
                    MailType id = email.MailType;
                    string sid = Enum.GetName(typeof(MailType), id);

                    email.MailType = (MailType)(int)comboBox1.SelectedValue;

                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        email.MailType = id;
                    }
                }
                else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "Priority")
                {
                    MailPriority id = email.Priority;
                    string sid = Enum.GetName(typeof(MailPriority), id);

                    email.Priority = (MailPriority)(int)comboBox1.SelectedValue;

                    if (SaveRow(RowIndex) == false)
                    {
                        comboBox1.Text = sid;
                        email.Priority = id;
                    }
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

        /// <summary>
        /// Handles the SelectionChangeCommitted event of the comboBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                ThirdPartyBatch batch = dataView.CurrentRow.DataBoundItem as ThirdPartyBatch;
                if (batch != null)
                    SelectionChangeCommitted(batch);

                ThirdPartyEmail email = dataView.CurrentRow.DataBoundItem as ThirdPartyEmail;
                if (email != null)
                    SelectionChangeCommitted(email);

                ThirdPartyGnuPG gnuPG = dataView.CurrentRow.DataBoundItem as ThirdPartyGnuPG;
                if (gnuPG != null)
                    SelectionChangeCommitted(gnuPG);
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

        private void dataView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;

            try
            {
                if (tb is TextBox)
                {
                    if (tb != null && dataView.Columns[dataView.CurrentCell.ColumnIndex].DataPropertyName.StartsWith("Pass"))
                    {

                        tb.UseSystemPasswordChar = true;
                    }
                    else
                        tb.UseSystemPasswordChar = false;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Handles the CellPainting event of the dataView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellPaintingEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void dataView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.Value == null) return;

            try
            {
                if (dataView.Columns[e.ColumnIndex].DataPropertyName.StartsWith("Pass"))
                {

                    string value = new String('*', e.Value.ToString().Length);
                    Rectangle newRect = new Rectangle(e.CellBounds.X + 1,
                                             e.CellBounds.Y + 1, e.CellBounds.Width - 4,
                                             e.CellBounds.Height - 4);


                    using (Brush gridBrush = new SolidBrush(dataView.GridColor), backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                    {
                        using (Pen gridLinePen = new Pen(gridBrush))
                        {
                            // Erase the cell.
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                            // Draw the grid lines (only the right and bottom lines; 
                            // DataGridView takes care of the others).
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                                e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                                e.CellBounds.Bottom - 1);
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                                e.CellBounds.Top, e.CellBounds.Right - 1,
                                e.CellBounds.Bottom);

                            // Draw the inset highlight box.
                            e.Graphics.DrawRectangle(Pens.Blue, newRect);

                            // Draw the text content of the cell, ignoring alignment. 
                            if (e.Value != null)
                            {

                                e.Graphics.DrawString(value, e.CellStyle.Font,
                                    Brushes.Crimson, e.CellBounds.X + 2,
                                    e.CellBounds.Y + 8, StringFormat.GenericTypographic);
                            }
                            e.Handled = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }

        }

        //Added By : Manvendra Prajapati
        //http://jira.nirvanasolutions.com:8080/browse/PRANA-5268
        //Purpose : Disable copy of password column in third party editor                    

        /// <summary>
        /// Gets the FTP type list.
        /// </summary>
        /// <returns></returns>
        private object GetFtpTypeList()
        {
            List<string> _ftpTypeList = new List<string>();
            try
            {
                _ftpTypeList.Add("FTP");
                _ftpTypeList.Add("SCP");
                _ftpTypeList.Add("SFTP");
                _ftpTypeList.Add("SFTPPasswordLess");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return _ftpTypeList;
        }

        /// <summary>
        /// Gets the Encryption type list.
        /// </summary>
        /// <returns></returns>
        private object GetEncryptionTypeList()
        {
            List<string> _encryptionTypeList = new List<string>();
            try
            {
                _encryptionTypeList.Add(ThirdPartyConstants.FTP_ENCRYPTION_NONE);
                _encryptionTypeList.Add(ThirdPartyConstants.FTP_ENCRYPTION_IMPLICIT);
                _encryptionTypeList.Add(ThirdPartyConstants.FTP_ENCRYPTION_EXPLICIT);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return _encryptionTypeList;
        }

        /// <summary>
        /// Handles the CellValueChanged event of the dataView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //If e.RowIndex is less than 0, then no need to update value
                if (e.RowIndex < 0)
                    return;
                if (dataView.Columns.Contains("FtpType") && dataView.Columns.Contains("FtpType2"))
                    dataView.Rows[e.RowIndex].Cells["FtpType"].Value = dataView.Rows[e.RowIndex].Cells["FtpType2"].Value;

                if (dataView.Columns.Contains("Encryption2"))
                {
                    if (dataView.Rows[e.RowIndex].Cells["FtpType2"].Value == null || !dataView.Rows[e.RowIndex].Cells["FtpType2"].Value.ToString().Equals("FTP", StringComparison.OrdinalIgnoreCase))
                    {
                        dataView.Rows[e.RowIndex].Cells["Encryption2"].ReadOnly = true;
                        dataView.Rows[e.RowIndex].Cells["Encryption2"].Value = "";

                    }
                    else
                        dataView.Rows[e.RowIndex].Cells["Encryption2"].ReadOnly = false;
                }
                if (dataView.Columns.Contains("Encryption") && dataView.Columns.Contains("Encryption2"))
                    dataView.Rows[e.RowIndex].Cells["Encryption"].Value = dataView.Rows[e.RowIndex].Cells["Encryption2"].Value;


            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the CellClick event of the dataView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void dataView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataView.Columns.Contains("EditTolerance") && e.ColumnIndex == dataView.Columns["EditTolerance"].Index)
                {
                    int rowIndex = dataView.CurrentCell.RowIndex;
                    ThirdPartyToleranceProfile ttp = dataView.Rows[rowIndex].DataBoundItem as ThirdPartyToleranceProfile;
                    if (ttp != null)
                    {
                        ToleranceProfile toleranceProfile = new ToleranceProfile();
                        toleranceProfile.InitializeDataSource(ttp);
                        toleranceProfile.updateToleranceProfileGrid += btnToleranceProfileSettings_Click;
                        toleranceProfile.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Handles the FormClosing event of the frmThirdParty control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosingEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void frmThirdPartyEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        #region IDisposable members
        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    ThirdPartyClientManager.Dispose();
                    if (components != null)
                    {
                        components.Dispose();
                    }

                    base.Dispose(disposing);
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
        #endregion

    }


}
