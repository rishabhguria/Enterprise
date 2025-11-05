#region Using
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for Identifier.
    /// </summary>
    public class Identifier : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "Identifier : ";
        private const int GRD_IDENTIFIER_ID = 0;
        private const int GRD_IDENTIFIER = 1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtIdenifier;
        private System.Windows.Forms.GroupBox grpIdentifier;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdIdentifier;
        private IContainer components;

        public Identifier()
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
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (lblUnit != null)
                {
                    lblUnit.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (grpIdentifier != null)
                {
                    grpIdentifier.Dispose();
                }
                if (txtIdenifier != null)
                {
                    txtIdenifier.Dispose();
                }
                if (grdIdentifier != null)
                {
                    grdIdentifier.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Identifier));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierName", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClientIdentifierName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryKey", 3);
            this.btnReset = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpIdentifier = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIdenifier = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grdIdentifier = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpIdentifier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIdentifier)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnReset.Location = new System.Drawing.Point(232, 241);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 43;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnEdit.Location = new System.Drawing.Point(97, 161);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 42;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDelete.Location = new System.Drawing.Point(175, 161);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 41;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(151, 241);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 40;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(70, 241);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 39;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpIdentifier
            // 
            this.grpIdentifier.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.grpIdentifier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grpIdentifier.Controls.Add(this.label1);
            this.grpIdentifier.Controls.Add(this.txtIdenifier);
            this.grpIdentifier.Controls.Add(this.lblUnit);
            this.grpIdentifier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpIdentifier.Location = new System.Drawing.Point(47, 189);
            this.grpIdentifier.Name = "grpIdentifier";
            this.grpIdentifier.Size = new System.Drawing.Size(252, 48);
            this.grpIdentifier.TabIndex = 37;
            this.grpIdentifier.TabStop = false;
            this.grpIdentifier.Text = "Add/Update Identifier";
            this.grpIdentifier.Visible = false;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(90, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 8;
            this.label1.Text = "*";
            // 
            // txtIdenifier
            // 
            this.txtIdenifier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIdenifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtIdenifier.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtIdenifier.Location = new System.Drawing.Point(104, 22);
            this.txtIdenifier.MaxLength = 50;
            this.txtIdenifier.Name = "txtIdenifier";
            this.txtIdenifier.Size = new System.Drawing.Size(132, 21);
            this.txtIdenifier.TabIndex = 2;
            this.txtIdenifier.LostFocus += new System.EventHandler(this.txtIdenifier_LostFocus);
            this.txtIdenifier.GotFocus += new System.EventHandler(this.txtIdenifier_GotFocus);
            this.txtIdenifier.TextChanged += new System.EventHandler(this.txtIdenifier_TextChanged);
            // 
            // lblUnit
            // 
            this.lblUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblUnit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblUnit.Location = new System.Drawing.Point(14, 25);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(78, 15);
            this.lblUnit.TabIndex = 0;
            this.lblUnit.Text = "Idenifier Name";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grdIdentifier
            // 
            this.grdIdentifier.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdIdentifier.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 211;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 311;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.Width = 75;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 74;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdIdentifier.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdIdentifier.DisplayLayout.GroupByBox.Hidden = true;
            this.grdIdentifier.DisplayLayout.MaxColScrollRegions = 1;
            this.grdIdentifier.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdIdentifier.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdIdentifier.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdIdentifier.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdIdentifier.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdIdentifier.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdIdentifier.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdIdentifier.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdIdentifier.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdIdentifier.Location = new System.Drawing.Point(7, 2);
            this.grdIdentifier.Name = "grdIdentifier";
            this.grdIdentifier.Size = new System.Drawing.Size(332, 235);
            this.grdIdentifier.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.grdIdentifier.TabIndex = 54;
            // 
            // Identifier
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(346, 268);
            this.Controls.Add(this.grdIdentifier);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpIdentifier);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "Identifier";
            this.Text = "Identifier";
            this.Load += new System.EventHandler(this.Identifier_Load);
            this.grpIdentifier.ResumeLayout(false);
            this.grpIdentifier.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdIdentifier)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtIdenifier_GotFocus(object sender, System.EventArgs e)
        {
            txtIdenifier.BackColor = Color.LemonChiffon;
        }
        private void txtIdenifier_LostFocus(object sender, System.EventArgs e)
        {
            txtIdenifier.BackColor = Color.White;
        }

        #endregion

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void Identifier_Load(object sender, System.EventArgs e)
        {
            BindIdentifierGrid();
        }

        private void BindIdentifierGrid()
        {
            //this.dataGridTableStyle1.MappingName = "identifiers";

            //Fetching the existing identifiers from the database and binding it to the grid.
            Identifiers identifiers = AUECManager.GetIdentifiers();
            //Assigning the grid's datasource to the identifiers object.
            if (identifiers.Count > 0)
            {
                grdIdentifier.DataSource = identifiers;
            }
            else
            {
                Identifiers nullIdentifiers = new Identifiers();
                nullIdentifiers.Add(new Prana.Admin.BLL.Identifier(int.MinValue, "", ""));
                grdIdentifier.DataSource = nullIdentifiers;
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currenly saved id of the Identifier in the database.
                int checkID = SaveIdentifier();
                //Binding grid after saving the a Identifier.
                BindIdentifierGrid();
                if (checkID >= 0)
                {
                    //Calling refresh form after saving a Identifier after checking that whether the 
                    //Identifier is saved successfully or not by checking for the positive value of 
                    //checkID.
                    RefreshForm();
                }
            }

            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
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
        /// This method saves the <see cref="Idnentifier"/> to the database.
        /// </summary>
        /// <returns>IdentiferID, saved to the database</returns>
        private int SaveIdentifier()
        {
            errorProvider1.SetError(txtIdenifier, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.Identifier identifer = new Prana.Admin.BLL.Identifier();

            if (txtIdenifier.Tag != null)
            {
                //Update
                identifer.IdentifierID = int.Parse(txtIdenifier.Tag.ToString());
            }
            //Validation to check for the empty value in Identifer textbox
            if (txtIdenifier.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtIdenifier, "Provide Value!");
                return result;
            }
            else
            {
                identifer.IdentifierName = txtIdenifier.Text.Trim();
                errorProvider1.SetError(txtIdenifier, "");
            }


            //Saving the Identifer data and retrieving the identiferid for the newly added identifier.
            int newIdentiferID = AUECManager.SaveIdentifier(identifer);
            //Showing the message: Identifier already existing by checking the identifer id value to -1 
            if (newIdentiferID == -1)
            {
                MessageBox.Show("Identifier with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
            }
            //Showing the message : Identifer data saved
            else
            {
                txtIdenifier.Tag = null;
            }
            result = newIdentiferID;
            //Returning the newly added identifer id.
            return result;
        }
        //This method blanks the textboxes in the Identifier form.
        private void RefreshForm()
        {
            txtIdenifier.Text = "";
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtIdenifier.Text = "";
            txtIdenifier.Tag = null;
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the Identifier if the grid has any Identifier.
                if (grdIdentifier.Rows.Count > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the identifier and identifierID.
                    txtIdenifier.Text = grdIdentifier.ActiveRow.Cells["IdentifierName"].Text.ToString();
                    txtIdenifier.Tag = grdIdentifier.ActiveRow.Cells["IdentifierID"].Text.ToString();
                }
                else
                {
                    //Showing the message: No Data Available.
                    errorProvider1.SetError(btnEdit, "No Data Available to edit.");
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
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
                //Check for deleting the identifier if the grid has any identifier.
                if (grdIdentifier.Rows.Count > 0)
                {
                    string identifierName = grdIdentifier.ActiveRow.Cells["IdentifierName"].Text.ToString();
                    if (identifierName != "")
                    {
                        //Asking the user to be sure about deleting the identifier.
                        if (MessageBox.Show(this, "Do you want to delete this Identifier?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the identiiferid from the currently selected row in the grid.
                            int identifierID = int.Parse(grdIdentifier.ActiveRow.Cells["IdentifierID"].Text.ToString());

                            bool chkVarraible = AUECManager.DeleteIdentifier(identifierID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "identifierID is referenced in ClientBrokerVenueDetails/AUEC Compliance Client.\n Please remove references first to delete it.", "Prana Alert");
                            }
                            else
                            {
                                BindIdentifierGrid();
                            }

                        }
                    }
                }
                else
                {
                    //Showing the message: No Data Available.
                    errorProvider1.SetError(btnDelete, "No Data Available to delete.");
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnDelete_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);


                #endregion
            }
        }

        private void grdIdentifier_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
        {
        }

        private void txtIdenifier_TextChanged(object sender, System.EventArgs e)
        {

        }
    }
}
