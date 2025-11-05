using Prana.Admin.BLL;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.ThirdPartyManager.DataAccess;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
//using Prana.ThirdPartyReport;
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
    public partial class frmThirdPartyEditor : Form
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
                    ftp.FtpId = ThirdPartyDataManager.SaveThirdPartyFtp(ftp);

                ThirdPartyGnuPG gnuPG = dataView.Rows[RowIndex].DataBoundItem as ThirdPartyGnuPG;
                if (gnuPG != null)
                {
                    gnuPG.Command = Commands.Decrypt;
                    gnuPG.GnuPGId = ThirdPartyDataManager.SaveThirdPartyGnuPG(gnuPG);
                }

                //ThirdPartyBatch batch = dataView.Rows[RowIndex].DataBoundItem as ThirdPartyBatch;
                //if (batch != null)
                //    batch.ThirdPartyBatchId = ThirdPartyManagerEx.SaveThirdPartyBatch(batch);

                ThirdPartyEmail email = dataView.Rows[RowIndex].DataBoundItem as ThirdPartyEmail;
                if (email != null)
                    email.EmailId = ThirdPartyDataManager.SaveThirdPartyEmail(email);


                dataView.Refresh();
                Console.Beep(1000, 10);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            if (dataView.IsCurrentRowDirty)
            {
                SaveRow(e.RowIndex);
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

            try
            {
                if (dataView.CurrentRow != null)
                {
                    ThirdPartyFtp ftp = dataView.CurrentRow.DataBoundItem as ThirdPartyFtp;
                    if (ftp != null)
                        DeleteRow(dataView, ftp);
                }

                if (dataView.CurrentRow != null)
                {
                    ThirdPartyGnuPG gnuPG = dataView.CurrentRow.DataBoundItem as ThirdPartyGnuPG;
                    if (gnuPG != null)
                        DeleteRow(dataView, gnuPG);
                }

                //ThirdPartyBatch batch = dataView.CurrentRow.DataBoundItem as ThirdPartyBatch;
                //if (batch != null)
                //    DeleteRow(dataView, batch);

                if (dataView.CurrentRow != null)
                {
                    ThirdPartyEmail email = dataView.CurrentRow.DataBoundItem as ThirdPartyEmail;
                    if (email != null)
                        DeleteRow(dataView, email);
                }
            }
            catch (Exception ex)
            {
                //return;
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
        /// Deletes the row.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="batch">The batch.</param>
        /// <remarks></remarks>
        //private void DeleteRow(DataGridView view, ThirdPartyBatch batch)
        //{
        //    bindingSource1.EndEdit();

        //    ThirdPartyManagerEx.DeleteThirdPartyBatch(batch);           
        //    dataView.Rows.Remove(dataView.CurrentRow);
        //}

        /// <summary>
        /// Deletes the row.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="ftp">The FTP.</param>
        /// <remarks></remarks>
        private void DeleteRow(DataGridView view, ThirdPartyFtp ftp)
        {
            try
            {
                bindingSource1.EndEdit();
                if (ThirdPartyDataManager.DeleteThirdPartyFtp(ftp) != 0)
                    MessageBox.Show("FTP is in use. Please delete the import batch associations first and try again.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void DeleteRow(DataGridView view, ThirdPartyGnuPG gnuPG)
        {
            try
            {
                bindingSource1.EndEdit();

                if (ThirdPartyDataManager.DeleteThirdPartyGnuPG(gnuPG) != 0)
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
        private void DeleteRow(DataGridView view, ThirdPartyEmail email)
        {
            try
            {
                bindingSource1.EndEdit();

                if (ThirdPartyDataManager.DeleteThirdPartyEmail(email) != 0)
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
                dataView.DataSource = bindingSource1;

                List<ThirdPartyFtp> items = ThirdPartyDataManager.GetThirdPartyFtps();

                if (items.Count > 0)
                    bindingSource1.DataSource = items;
                else
                    bindingSource1.DataSource = new ThirdPartyFtp();

                CustomizeDataViewForFTPSettings();

                dataView.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Customizes the data view for FTP settings.
        /// </summary>
        private void CustomizeDataViewForFTPSettings()
        {
            try
            {
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
                dataView.DataSource = bindingSource1;

                ThirdPartyGnuPGs items = ThirdPartyDataManager.GetThirdPartyGnuPGForDecryption(-1);

                if (items.Count > 0)
                    bindingSource1.DataSource = items;
                else
                    bindingSource1.DataSource = new ThirdPartyGnuPG();

                dataView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                //Add Column at last index
                dataView.Columns["ExtensionToAdd"].DisplayIndex = dataView.ColumnCount - 1;
                dataView.Columns["GnuPGId"].Visible = false;
                dataView.Columns["Command"].Visible = false;
                if (dataView.Columns.Contains("FtpType2"))
                {
                    dataView.Columns["FtpType2"].Visible = false;
                }
                dataView.Update();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                //bindingSource1.DataSource = null;
                //dataView.DataSource = bindingSource1;

                //ThirdPartyBatches items = ThirdPartyManagerEx.GetThirdPartyBatch();

                //if (items.Count > 0)
                //    bindingSource1.DataSource = items;
                //else
                //    bindingSource1.DataSource = new ThirdPartyBatch();

                //dataView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                //dataView.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                dataView.DataSource = bindingSource1;

                List<ThirdPartyEmail> items = ThirdPartyDataManager.GetThirdPartyEmail();

                if (items.Count > 0)
                    bindingSource1.DataSource = items;
                else
                    bindingSource1.DataSource = new ThirdPartyEmail();

                dataView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataView.Columns["EmailId"].Visible = false;
                dataView.Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                //Setup setup = new Setup();
                //setup.AutoCreateBatchEntries();
                //btnJobSettings_Click(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
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
                //else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "Command")
                //{
                //    comboBox1.Visible = true;
                //    comboBox1.DataSource = new EnumToDataSet<Commands>("CommandId", "Command");
                //    comboBox1.ValueMember = "CommandId";
                //    comboBox1.DisplayMember = "Command";
                //}

                if (dataView.CurrentCell.Value == null)
                    comboBox1.SelectedIndex = -1;
                else
                {
                    comboBox1.SelectedIndex = comboBox1.FindString(dataView.CurrentCell.Value.ToString());

                }
            }
            catch (Exception)
            {

            }

        }
        /// <summary>
        /// Cells the enter.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void CellEnter(ThirdPartyEmail email, DataGridViewCellEventArgs e)
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
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// Cells the enter.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        //private void CellEnter(ThirdPartyBatch batch,DataGridViewCellEventArgs e)
        //{
        //    try
        //    {

        //        Rectangle rect = dataView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex + 1, false);

        //        comboBox1.Top = rect.Top + 2;
        //        comboBox1.Left = rect.Left;
        //        comboBox1.Width = rect.Width;
        //        comboBox1.Height = rect.Height + 1;

        //        if (dataView.CurrentCell.OwningColumn.DataPropertyName == "GnuPGName")
        //        {
        //            comboBox1.Visible = true;
        //            comboBox1.DataSource = GetThirdPartyGnuPGs();
        //            comboBox1.ValueMember = "GnuPGId";
        //            comboBox1.DisplayMember = "GnuPGName";
        //        }
        //        else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "FtpName")
        //        {
        //            comboBox1.Visible = true;
        //            comboBox1.DataSource = GetThirdPartyFtps();
        //            comboBox1.ValueMember = "FtpId";
        //            comboBox1.DisplayMember = "FtpName";
        //        }
        //        else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "ThirdPartyName")
        //        {
        //            comboBox1.Visible = true;
        //            comboBox1.DataSource = batch.GetThirdParties();
        //            comboBox1.ValueMember = "ThirdPartyId";
        //            comboBox1.DisplayMember = "ThirdPartyName";

        //        }
        //        else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "ThirdPartyTypeName")
        //        {
        //            comboBox1.Visible = true;
        //            comboBox1.DataSource = batch.GetThirdPartyTypes();
        //            comboBox1.ValueMember = "ThirdPartyTypeId";
        //            comboBox1.DisplayMember = "ThirdPartyTypeName";

        //        }

        //        else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "FileFormatName")
        //        {
        //            comboBox1.Visible = true;
        //            comboBox1.DataSource = batch.GetThirdPartyFormats();
        //            comboBox1.ValueMember = "FileFormatId";
        //            comboBox1.DisplayMember = "FileFormatName";

        //        }
        //        else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "EmailDataName")
        //        {
        //            comboBox1.Visible = true;
        //            comboBox1.DataSource = GetThirdPartyDataEmail();
        //            comboBox1.ValueMember = "EmailId";
        //            comboBox1.DisplayMember = "EmailName";
        //        }
        //        else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "EmailLogName")
        //        {
        //            comboBox1.Visible = true;
        //            comboBox1.DataSource = GetThirdPartyLogEmail();
        //            comboBox1.ValueMember = "EmailId";
        //            comboBox1.DisplayMember = "EmailName";
        //        }


        //        if (dataView.CurrentCell.Value == null)
        //            comboBox1.SelectedIndex = -1;
        //        else
        //            comboBox1.SelectedIndex = comboBox1.FindString(dataView.CurrentCell.Value.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        /// <summary>
        /// Handles the CellEnter event of the dataView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void dataView_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            comboBox1.Visible = false;

            //ThirdPartyBatch batch = dataView.CurrentRow.DataBoundItem as ThirdPartyBatch;
            //if (batch != null)
            //    CellEnter(batch, e);

            ThirdPartyEmail email = dataView.CurrentRow.DataBoundItem as ThirdPartyEmail;
            if (email != null)
                CellEnter(email, e);

            ThirdPartyGnuPG gnuPG = dataView.CurrentRow.DataBoundItem as ThirdPartyGnuPG;
            if (gnuPG != null)
                CellEnter(gnuPG, e);

        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the comboBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.Print(comboBox1.Text);
        }

        /// <summary>
        /// Selections the change committed.
        /// </summary>
        /// <param name="batch">The batch.</param>
        /// <remarks></remarks>
        //private void SelectionChangeCommitted(ThirdPartyBatch batch)
        //{            
        //    int RowIndex = dataView.CurrentRow.Index;

        //    if (batch == null) return;
        //    int? value = comboBox1.SelectedValue as int?;
        //    if (value == null) return;

        //    if (dataView.CurrentCell.OwningColumn.DataPropertyName == "GnuPGName")
        //    {
        //        int id = batch.GnuPGId;
        //        string sid = batch.GnuPGName;

        //        batch.GnuPGId = (int)comboBox1.SelectedValue;
        //        batch.GnuPGName = comboBox1.Text;
        //        if (SaveRow(RowIndex) == false)
        //        {
        //            comboBox1.Text = sid;
        //            batch.GnuPGId = id;
        //            batch.GnuPGName = sid;
        //        }
        //    }
        //    else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "FtpName")
        //    {
        //        int id = batch.FtpId;
        //        string sid = batch.FtpName;

        //        batch.FtpId = (int)comboBox1.SelectedValue;
        //        batch.FtpName = comboBox1.Text;
        //        if (SaveRow(RowIndex) == false)
        //        {
        //            comboBox1.Text = sid;
        //            batch.FtpId = id;
        //            batch.FtpName = sid;
        //        }
        //    }
        //    else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "ThirdPartyName")
        //    {
        //        int id = batch.ThirdPartyId;
        //        string sid = batch.ThirdPartyName;

        //        batch.ThirdPartyId = (int)comboBox1.SelectedValue;
        //        batch.ThirdPartyName = comboBox1.Text;
        //        if (SaveRow(RowIndex) == false)
        //        {
        //            comboBox1.Text = sid;
        //            batch.ThirdPartyId = id;
        //            batch.ThirdPartyName = sid;
        //        }
        //    }
        //    else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "ThirdPartyTypeName")
        //    {
        //        int id = batch.ThirdPartyTypeId;
        //        string sid = batch.ThirdPartyTypeName;

        //        batch.ThirdPartyTypeId = (int)comboBox1.SelectedValue;
        //        batch.ThirdPartyTypeName = comboBox1.Text;

        //        if (SaveRow(RowIndex) == false)
        //        {
        //            comboBox1.Text = sid;
        //            batch.ThirdPartyTypeId = id;
        //            batch.ThirdPartyTypeName = sid;
        //        }
        //    }

        //    else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "FileFormatName")
        //    {
        //        int id = batch.ThirdPartyFormatId;
        //        string sid = batch.FileFormatName;

        //        batch.ThirdPartyFormatId = (int)comboBox1.SelectedValue;
        //        batch.FileFormatName = comboBox1.Text;

        //        if (SaveRow(RowIndex) == false)
        //        {
        //            comboBox1.Text = sid;
        //            batch.ThirdPartyFormatId = id;
        //            batch.FileFormatName = sid;
        //        }
        //    }

        //    else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "EmailLogName")
        //    {
        //        int id = batch.EmailLogId;
        //        string sid = batch.EmailLogName;

        //        batch.EmailLogId = (int)comboBox1.SelectedValue;
        //        batch.EmailLogName = comboBox1.Text;

        //        if (SaveRow(RowIndex) == false)
        //        {
        //            comboBox1.Text = sid;
        //            batch.EmailLogId = id;
        //            batch.EmailLogName = sid;
        //        }
        //    }

        //    else if (dataView.CurrentCell.OwningColumn.DataPropertyName == "EmailDataName")
        //    {
        //        int id = batch.EmailDataId;
        //        string sid = batch.EmailDataName;

        //        batch.EmailDataId = (int)comboBox1.SelectedValue;
        //        batch.EmailDataName = comboBox1.Text;

        //        if (SaveRow(RowIndex) == false)
        //        {
        //            comboBox1.Text = sid;
        //            batch.EmailDataId = id;
        //            batch.EmailDataName = sid;
        //        }
        //    }
        //}

        /// <summary>
        /// Selections the change committed.
        /// </summary>
        /// <param name="gnuPG">The gnu PG.</param>
        /// <remarks></remarks>
        private void SelectionChangeCommitted(ThirdPartyGnuPG gnuPG)
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
        /// <summary>
        /// Selections the change committed.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <remarks></remarks>
        private void SelectionChangeCommitted(ThirdPartyEmail email)
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

        /// <summary>
        /// Handles the SelectionChangeCommitted event of the comboBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /// <remarks></remarks>
        //private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    ThirdPartyBatch batch = dataView.CurrentRow.DataBoundItem as ThirdPartyBatch;
        //    if (batch != null)
        //        SelectionChangeCommitted(batch);

        //    ThirdPartyEmail email = dataView.CurrentRow.DataBoundItem as ThirdPartyEmail;
        //    if (email != null)                
        //        SelectionChangeCommitted(email);

        //    ThirdPartyGnuPG gnuPG = dataView.CurrentRow.DataBoundItem as ThirdPartyGnuPG;
        //    if (gnuPG != null)
        //        SelectionChangeCommitted(gnuPG);
        //}

        private void dataView_DataSourceChanged(object sender, EventArgs e)
        {

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

        private void dataView_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            //e.Row.DataBoundItem
        }

        private void frmThirdPartyEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            // FileSettingManager.dictFtp = FileSettingDAL.GetFtpFromDB(); 
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //ThirdPartyBatch batch = dataView.CurrentRow.DataBoundItem as ThirdPartyBatch;
            //if (batch != null)
            //    SelectionChangeCommitted(batch);

            ThirdPartyEmail email = dataView.CurrentRow.DataBoundItem as ThirdPartyEmail;
            if (email != null)
                SelectionChangeCommitted(email);

            ThirdPartyGnuPG gnuPG = dataView.CurrentRow.DataBoundItem as ThirdPartyGnuPG;
            if (gnuPG != null)
                SelectionChangeCommitted(gnuPG);
        }

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
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}
