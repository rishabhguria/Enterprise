#region Using
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Windows.Forms;
#endregion

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for SymbolIdentifier.
    /// </summary>
    public class SymbolIdentifier : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "SymbolIdentifier : ";
        private const int GRD_SYMBOLIDENTIFIER_ID = 0;
        private const int GRD_SYMBOLIDENTIFIER = 1;
        private const int GRD_SYMBOLIDENTIFIER_SHORTNAME = 2;

        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGrid grdSymbolIdentifier;
        private System.Windows.Forms.TextBox txtSymbolIdentifier;
        private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
        private System.Windows.Forms.DataGridTextBoxColumn colSymbolIdentifierID;
        private System.Windows.Forms.DataGridTextBoxColumn colSymbolIdentifierName;
        private System.Windows.Forms.DataGridTextBoxColumn colSymbolIdentifierShortName;
        private System.Windows.Forms.Label lblSymbolIdentifierName;
        private System.Windows.Forms.TextBox txtSymbolIdentifierShortName;
        private System.Windows.Forms.Label lblSymbolIdentifierShortName;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public SymbolIdentifier()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (btnReset != null)
                {
                    btnReset.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (grdSymbolIdentifier != null)
                {
                    grdSymbolIdentifier.Dispose();
                }
                if (txtSymbolIdentifier != null)
                {
                    txtSymbolIdentifier.Dispose();
                }
                if (dataGridTableStyle1 != null)
                {
                    dataGridTableStyle1.Dispose();
                }
                if (colSymbolIdentifierID != null)
                {
                    colSymbolIdentifierID.Dispose();
                }
                if (colSymbolIdentifierName != null)
                {
                    colSymbolIdentifierName.Dispose();
                }
                if (colSymbolIdentifierShortName != null)
                {
                    colSymbolIdentifierShortName.Dispose();
                }
                if (lblSymbolIdentifierName != null)
                {
                    lblSymbolIdentifierName.Dispose();
                }
                if (txtSymbolIdentifierShortName != null)
                {
                    txtSymbolIdentifierShortName.Dispose();
                }
                if (lblSymbolIdentifierShortName != null)
                {
                    lblSymbolIdentifierShortName.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grdSymbolIdentifier = new System.Windows.Forms.DataGrid();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSymbolIdentifier = new System.Windows.Forms.TextBox();
            this.lblSymbolIdentifierName = new System.Windows.Forms.Label();
            this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
            this.colSymbolIdentifierID = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colSymbolIdentifierName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.colSymbolIdentifierShortName = new System.Windows.Forms.DataGridTextBoxColumn();
            this.txtSymbolIdentifierShortName = new System.Windows.Forms.TextBox();
            this.lblSymbolIdentifierShortName = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this.grdSymbolIdentifier)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(107, 174);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.TabIndex = 84;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(240)), ((System.Byte)(150)));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnReset.Location = new System.Drawing.Point(146, 272);
            this.btnReset.Name = "btnReset";
            this.btnReset.TabIndex = 85;
            this.btnReset.Text = "&Reset";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(185, 174);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.TabIndex = 83;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(226, 272);
            this.btnClose.Name = "btnClose";
            this.btnClose.TabIndex = 82;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdSymbolIdentifier
            // 
            this.grdSymbolIdentifier.AllowNavigation = false;
            this.grdSymbolIdentifier.AlternatingBackColor = System.Drawing.Color.WhiteSmoke;
            this.grdSymbolIdentifier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grdSymbolIdentifier.CaptionVisible = false;
            this.grdSymbolIdentifier.DataMember = "";
            //this.grdSymbolIdentifier.UseFlatMode = DefaultableBoolean.True;
            this.grdSymbolIdentifier.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.grdSymbolIdentifier.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.grdSymbolIdentifier.Location = new System.Drawing.Point(4, 4);
            this.grdSymbolIdentifier.Name = "grdSymbolIdentifier";
            this.grdSymbolIdentifier.PreferredColumnWidth = 70;
            this.grdSymbolIdentifier.ReadOnly = true;
            this.grdSymbolIdentifier.RowHeadersVisible = false;
            this.grdSymbolIdentifier.RowHeaderWidth = 5;
            this.grdSymbolIdentifier.Size = new System.Drawing.Size(352, 168);
            this.grdSymbolIdentifier.TabIndex = 80;
            this.grdSymbolIdentifier.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
                                                                                                            this.dataGridTableStyle1});
            this.grdSymbolIdentifier.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(66, 272);
            this.btnSave.Name = "btnSave";
            this.btnSave.TabIndex = 81;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSymbolIdentifierShortName);
            this.groupBox1.Controls.Add(this.lblSymbolIdentifierShortName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSymbolIdentifier);
            this.groupBox1.Controls.Add(this.lblSymbolIdentifierName);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(2, 200);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 66);
            this.groupBox1.TabIndex = 79;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Update SymbolIdentifier";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(190, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 8;
            this.label1.Text = "*";
            // 
            // txtSymbolIdentifier
            // 
            this.txtSymbolIdentifier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSymbolIdentifier.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSymbolIdentifier.Location = new System.Drawing.Point(202, 13);
            this.txtSymbolIdentifier.MaxLength = 50;
            this.txtSymbolIdentifier.Name = "txtSymbolIdentifier";
            this.txtSymbolIdentifier.Size = new System.Drawing.Size(154, 21);
            this.txtSymbolIdentifier.TabIndex = 2;
            this.txtSymbolIdentifier.Text = "";
            // 
            // lblSymbolIdentifierName
            // 
            this.lblSymbolIdentifierName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSymbolIdentifierName.Location = new System.Drawing.Point(5, 19);
            this.lblSymbolIdentifierName.Name = "lblSymbolIdentifierName";
            this.lblSymbolIdentifierName.Size = new System.Drawing.Size(141, 15);
            this.lblSymbolIdentifierName.TabIndex = 0;
            this.lblSymbolIdentifierName.Text = "Symbol Identifier Name";
            // 
            // dataGridTableStyle1
            // 
            this.dataGridTableStyle1.DataGrid = this.grdSymbolIdentifier;
            this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
                                                                                                                  this.colSymbolIdentifierID,
                                                                                                                  this.colSymbolIdentifierName,
                                                                                                                  this.colSymbolIdentifierShortName});
            this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGridTableStyle1.MappingName = "symbolIdentifiers";
            // 
            // colSymbolIdentifierID
            // 
            this.colSymbolIdentifierID.Format = "";
            this.colSymbolIdentifierID.FormatInfo = null;
            this.colSymbolIdentifierID.MappingName = "SymbolIdentifierID";
            this.colSymbolIdentifierID.Width = 0;
            // 
            // colSymbolIdentifierName
            // 
            this.colSymbolIdentifierName.Format = "";
            this.colSymbolIdentifierName.FormatInfo = null;
            this.colSymbolIdentifierName.HeaderText = "SymbolIdentifier Name";
            this.colSymbolIdentifierName.MappingName = "SymbolIdentifierName";
            // 
            // colSymbolIdentifierShortName
            // 
            this.colSymbolIdentifierShortName.Format = "";
            this.colSymbolIdentifierShortName.FormatInfo = null;
            this.colSymbolIdentifierShortName.HeaderText = "SymbolIdentifier ShortName";
            this.colSymbolIdentifierShortName.MappingName = "SymbolIdentifierShortName";
            // 
            // txtSymbolIdentifierShortName
            // 
            this.txtSymbolIdentifierShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSymbolIdentifierShortName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSymbolIdentifierShortName.Location = new System.Drawing.Point(202, 36);
            this.txtSymbolIdentifierShortName.MaxLength = 50;
            this.txtSymbolIdentifierShortName.Name = "txtSymbolIdentifierShortName";
            this.txtSymbolIdentifierShortName.Size = new System.Drawing.Size(154, 21);
            this.txtSymbolIdentifierShortName.TabIndex = 10;
            this.txtSymbolIdentifierShortName.Text = "";
            // 
            // lblSymbolIdentifierShortName
            // 
            this.lblSymbolIdentifierShortName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSymbolIdentifierShortName.Location = new System.Drawing.Point(4, 42);
            this.lblSymbolIdentifierShortName.Name = "lblSymbolIdentifierShortName";
            this.lblSymbolIdentifierShortName.Size = new System.Drawing.Size(178, 15);
            this.lblSymbolIdentifierShortName.TabIndex = 9;
            this.lblSymbolIdentifierShortName.Text = "Symbol Identifier Short Name";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // SymbolIdentifier
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(366, 303);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grdSymbolIdentifier);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "SymbolIdentifier";
            this.Text = "SymbolIdentifier";
            this.Load += new System.EventHandler(this.SymbolIdentifier_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdSymbolIdentifier)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void SymbolIdentifier_Load(object sender, System.EventArgs e)
        {
            BindSymbolIdentifierGrid();
        }

        private void BindSymbolIdentifierGrid()
        {
            this.dataGridTableStyle1.MappingName = "symbolIdentifiers";

            //Fetching the existing symbolIdentifiers from the database and binding it to the grid.
            SymbolIdentifiers symbolIdentifiers = SymbolManager.GetSymbolIdentifiers();
            //Assigning the grid's datasource to the symbolIdentifiers object.
            grdSymbolIdentifier.DataSource = symbolIdentifiers;
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currenly saved id of the SymbolIdentifier in the database.
                int checkID = SaveSymbolIdentifier();
                //Binding grid after saving a SymbolIdentifier.
                BindSymbolIdentifierGrid();
                if (checkID >= 0)
                {
                    //Calling refresh form after saving a SymbolIdentifier after checking that whether the 
                    //SymbolIdentifier is saved successfully or not by checking for the positive value of 
                    //checkID.
                    RefreshForm();
                }
            }
            catch (Exception ex)
            {
                #region Catch
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
                #endregion
            }
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);


                #endregion
            }
        }

        /// <summary>
        /// This method saves the <see cref="SymbolIDentifier"/> to the database.
        /// </summary>
        /// <returns>SymbolIDentifier, saved to the database</returns>
        private int SaveSymbolIdentifier()
        {
            errorProvider1.SetError(txtSymbolIdentifier, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.SymbolIdentifier symbolIDentifier = new Prana.Admin.BLL.SymbolIdentifier();

            if (txtSymbolIdentifier.Tag != null)
            {
                //Update
                symbolIDentifier.SymbolIdentifierID = int.Parse(txtSymbolIdentifier.Tag.ToString());
            }
            //Validation to check for the empty value in SymbolIDentifier textbox
            if (txtSymbolIdentifier.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtSymbolIdentifier, "Provide Value!");
                return result;
            }
            else
            {
                symbolIDentifier.SymbolIdentifierName = txtSymbolIdentifier.Text.Trim();
                errorProvider1.SetError(txtSymbolIdentifier, "");
            }
            symbolIDentifier.ShortName = txtSymbolIdentifierShortName.Text.Trim();

            //Saving the SymbolIdentifer data and retrieving the SymbolIdentifer for the newly added SymbolIdentifer.
            int newSymbolIdentiferID = SymbolManager.SaveSymbolIdentifiers(symbolIDentifier);
            //Showing the message: SymbolIdentifer already existing by checking the SymbolIdentifer id value to -1 
            if (newSymbolIdentiferID == -1)
            {
                errorProvider1.SetError(btnSave, "SymbolIdentifer Already Exists");
            }
            //Showing the message : SymbolIdentifer data saved
            else
            {
                txtSymbolIdentifier.Tag = null;
            }
            result = newSymbolIdentiferID;
            //Returning the newly added SymbolIdentifer id.
            return result;
        }

        //This method blanks the textboxes in the SymbolIdentifer form.
        private void RefreshForm()
        {
            txtSymbolIdentifier.Text = "";
            txtSymbolIdentifierShortName.Text = "";
        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtSymbolIdentifier.Text = "";
            txtSymbolIdentifier.Tag = null;
            txtSymbolIdentifierShortName.Text = "";
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the SymbolIdentifer if the grid has any SymbolIdentifer.
                if (grdSymbolIdentifier.VisibleRowCount > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the symbolIdentifer and symbolIdentiferID.
                    txtSymbolIdentifier.Text = grdSymbolIdentifier[grdSymbolIdentifier.CurrentRowIndex, GRD_SYMBOLIDENTIFIER].ToString();
                    txtSymbolIdentifier.Tag = grdSymbolIdentifier[grdSymbolIdentifier.CurrentRowIndex, GRD_SYMBOLIDENTIFIER_ID].ToString();
                    txtSymbolIdentifierShortName.Text = grdSymbolIdentifier[grdSymbolIdentifier.CurrentRowIndex, GRD_SYMBOLIDENTIFIER_SHORTNAME].ToString();
                }
                else
                {
                    //Showing the message: No Data Available.
                    errorProvider1.SetError(btnEdit, "No Data Available to edit.");
                }
            }
            catch (Exception ex)
            {
                #region Catch
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
                #endregion
            }
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnEdit_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnEdit_Click", null);


                #endregion
            }
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Check for deleting the symbolIdentifier if the grid has any symbolIdentifier.
                if (grdSymbolIdentifier.VisibleRowCount > 0)
                {
                    //Asking the user to be sure about deleting the symbolIdentifier.
                    if (MessageBox.Show(this, "Do you want to delete this SymbolIdentifier?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //Getting the symbolIdentifierid from the currently selected row in the grid.
                        int symbolIdentifierID = int.Parse(grdSymbolIdentifier[grdSymbolIdentifier.CurrentRowIndex, GRD_SYMBOLIDENTIFIER_ID].ToString());

                        bool chkVarraible = SymbolManager.DeleteSymbolIdentifier(symbolIdentifierID, false);
                        if (!(chkVarraible))
                        {
                            MessageBox.Show(this, "SymbolIdentifierID is referenced in Symbol Mapping.\n You can not delete it.", "Prana Alert");
                        }
                        else
                        {
                            BindSymbolIdentifierGrid();
                        }

                    }

                }
                else
                {
                    //Showing the message: No Data Available.
                    errorProvider1.SetError(btnDelete, "No Data Available to delete.");
                }
            }
            catch (Exception ex)
            {
                #region Catch
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
                #endregion
            }
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnDelete_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);


                #endregion
            }
        }

    }
}
