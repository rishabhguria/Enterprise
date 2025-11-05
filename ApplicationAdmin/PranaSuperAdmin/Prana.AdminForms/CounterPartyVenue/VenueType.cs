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
    /// Summary description for VenueType.
    /// </summary>
    public class VenueType : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "VenueType : ";
        private const int GRD_VENUETYPE_ID = 0;
        private const int GRD_VENUETYPE = 1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtVenueType;
        private System.Windows.Forms.Label lblVenueType;
        private System.Windows.Forms.GroupBox grpVenueType;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdVenueType;
        private IContainer components;

        public VenueType()
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
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (txtVenueType != null)
                {
                    txtVenueType.Dispose();
                }
                if (lblVenueType != null)
                {
                    lblVenueType.Dispose();
                }
                if (grpVenueType != null)
                {
                    grpVenueType.Dispose();
                }
                if (grdVenueType != null)
                {
                    grdVenueType.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VenueType));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueTypeID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.btnSave = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grpVenueType = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVenueType = new System.Windows.Forms.TextBox();
            this.lblVenueType = new System.Windows.Forms.Label();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grdVenueType = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.grpVenueType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVenueType)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(55, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 67;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grpVenueType
            // 
            this.grpVenueType.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.grpVenueType.Controls.Add(this.label1);
            this.grpVenueType.Controls.Add(this.txtVenueType);
            this.grpVenueType.Controls.Add(this.lblVenueType);
            this.grpVenueType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpVenueType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpVenueType.Location = new System.Drawing.Point(57, 186);
            this.grpVenueType.Name = "grpVenueType";
            this.grpVenueType.Size = new System.Drawing.Size(233, 42);
            this.grpVenueType.TabIndex = 65;
            this.grpVenueType.TabStop = false;
            this.grpVenueType.Text = "Add/Update VenueType";
            this.grpVenueType.Visible = false;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(50, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 8;
            this.label1.Text = "*";
            // 
            // txtVenueType
            // 
            this.txtVenueType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVenueType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtVenueType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtVenueType.Location = new System.Drawing.Point(62, 18);
            this.txtVenueType.MaxLength = 50;
            this.txtVenueType.Name = "txtVenueType";
            this.txtVenueType.Size = new System.Drawing.Size(154, 21);
            this.txtVenueType.TabIndex = 2;
            this.txtVenueType.LostFocus += new System.EventHandler(this.txtVenueType_LostFocus);
            this.txtVenueType.GotFocus += new System.EventHandler(this.txtVenueType_GotFocus);
            // 
            // lblVenueType
            // 
            this.lblVenueType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblVenueType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblVenueType.Location = new System.Drawing.Point(14, 20);
            this.lblVenueType.Name = "lblVenueType";
            this.lblVenueType.Size = new System.Drawing.Size(35, 15);
            this.lblVenueType.TabIndex = 0;
            this.lblVenueType.Text = " Type";
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnEdit.Location = new System.Drawing.Point(97, 160);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 70;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnReset.Location = new System.Drawing.Point(217, 230);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 71;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDelete.Location = new System.Drawing.Point(175, 160);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 69;
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
            this.btnClose.Location = new System.Drawing.Point(136, 230);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 68;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grdVenueType
            // 
            this.grdVenueType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdVenueType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 143;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 313;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdVenueType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdVenueType.DisplayLayout.GroupByBox.Hidden = true;
            this.grdVenueType.DisplayLayout.MaxColScrollRegions = 1;
            this.grdVenueType.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdVenueType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdVenueType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdVenueType.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdVenueType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdVenueType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdVenueType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdVenueType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdVenueType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdVenueType.Location = new System.Drawing.Point(6, 2);
            this.grdVenueType.Name = "grdVenueType";
            this.grdVenueType.Size = new System.Drawing.Size(334, 226);
            this.grdVenueType.TabIndex = 72;
            // 
            // VenueType
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(346, 255);
            this.Controls.Add(this.grdVenueType);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpVenueType);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "VenueType";
            this.Text = "VenueType";
            this.Load += new System.EventHandler(this.VenueType_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.grpVenueType.ResumeLayout(false);
            this.grpVenueType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVenueType)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtVenueType_GotFocus(object sender, System.EventArgs e)
        {
            txtVenueType.BackColor = Color.LemonChiffon;
        }
        private void txtVenueType_LostFocus(object sender, System.EventArgs e)
        {
            txtVenueType.BackColor = Color.White;
        }

        #endregion

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void VenueType_Load(object sender, System.EventArgs e)
        {
            BindVenueTypeGrid();
        }

        private void BindVenueTypeGrid()
        {
            //this.dataGridTableStyle1.MappingName = "venueTypes";

            //Fetching the existing modules from the database and binding it to the grid.
            VenueTypes venueTypes = VenueManager.GetVenueTypes();
            //Assigning the grid's datasource to the VenueTypes object.
            if (venueTypes.Count > 0)
            {
                grdVenueType.DataSource = venueTypes;
            }
            else
            {
                VenueTypes nullVenueTypes = new VenueTypes();
                nullVenueTypes.Add(new Prana.Admin.BLL.VenueType(int.MinValue, ""));
                grdVenueType.DataSource = nullVenueTypes;
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currenly saved id of the VenueType in the database.
                int checkID = SaveVenueType();
                //Binding grid after saving the a VenueType.
                BindVenueTypeGrid();
                if (checkID >= 0)
                {
                    //Calling refresh form after saving a VenueType after checking that whether the 
                    //VenueType is saved successfully or not by checking for the positive value of 
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
        /// This method saves the <see cref="VenueType"/> to the database.
        /// </summary>
        /// <returns>VenueTypeID, saved to the database</returns>
        private int SaveVenueType()
        {
            errorProvider1.SetError(txtVenueType, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.VenueType venueType = new Prana.Admin.BLL.VenueType();

            if (txtVenueType.Tag != null)
            {
                //Update
                venueType.VenueTypeID = int.Parse(txtVenueType.Tag.ToString());
            }
            //Validation to check for the empty value in VenueType textbox
            if (txtVenueType.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtVenueType, "Provide Value!");
                return result;
            }
            else
            {
                venueType.Type = txtVenueType.Text.Trim();
                errorProvider1.SetError(txtVenueType, "");
            }


            //Saving the VenueType data and retrieving the venueTypeid for the newly added VenueType.
            int newVenueTypeID = VenueManager.SaveVenueType(venueType);
            //Showing the message: VenueType already existing by checking the VenueType id value to -1 
            if (newVenueTypeID == -1)
            {
                MessageBox.Show("VenueType with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
            }
            //Showing the message : VenueType data saved
            else
            {
                txtVenueType.Tag = null;
            }
            result = newVenueTypeID;
            //Returning the newly added VenueType id.
            return result;
        }

        //This method blanks the textboxes in the Venue form.
        private void RefreshForm()
        {
            txtVenueType.Text = "";
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtVenueType.Text = "";
            txtVenueType.Tag = null;
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the VenueType if the grid has any VenueType.
                if (grdVenueType.Rows.Count > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the venueType and venueTypeID.
                    txtVenueType.Text = grdVenueType.ActiveRow.Cells["Type"].Text.ToString();
                    txtVenueType.Tag = grdVenueType.ActiveRow.Cells["VenueTypeID"].Text.ToString();
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
                //Check for deleting the venueType if the grid has any venueType.
                if (grdVenueType.Rows.Count > 0)
                {
                    string type = grdVenueType.ActiveRow.Cells["Type"].Text.ToString();
                    if (type != "")
                    {
                        //Asking the user to be sure about deleting the venueType.
                        if (MessageBox.Show(this, "Do you want to delete this VenueType?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the venueTypeid from the currently selected row in the grid.
                            int venueTypeID = int.Parse(grdVenueType.ActiveRow.Cells["VenueTypeID"].Text.ToString());

                            bool chkVarraible = VenueManager.DeleteVenueType(venueTypeID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "VenueType is referenced in Venue.\n Please remove references first to delete it.", "Prana Alert");
                            }
                            else
                            {
                                BindVenueTypeGrid();
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
    }
}
