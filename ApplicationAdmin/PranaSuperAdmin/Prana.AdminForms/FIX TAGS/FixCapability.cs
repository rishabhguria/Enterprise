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
    /// Summary description for FixCapability.
    /// </summary>
    public class FixCapability : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "FixCapability : ";
        private const int GRD_FIXCAPABILITY_ID = 0;
        private const int GRD_FIXCAPABILITY = 1;

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFixCapability;
        private System.Windows.Forms.Label lblFixCapability;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdFixCapability;
        private IContainer components;

        public FixCapability()
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
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (txtFixCapability != null)
                {
                    txtFixCapability.Dispose();
                }
                if (lblFixCapability != null)
                {
                    lblFixCapability.Dispose();
                }
                if (grdFixCapability != null)
                {
                    grdFixCapability.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FixCapability));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixCapabilityID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFixCapability = new System.Windows.Forms.TextBox();
            this.lblFixCapability = new System.Windows.Forms.Label();
            this.grdFixCapability = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFixCapability)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnReset.Location = new System.Drawing.Point(135, 232);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 57;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnEdit.Location = new System.Drawing.Point(94, 158);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 56;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDelete.Location = new System.Drawing.Point(172, 158);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 55;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(213, 232);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 54;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(57, 232);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 53;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtFixCapability);
            this.groupBox1.Controls.Add(this.lblFixCapability);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(60, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 46);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Update FixCapability";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(48, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 8;
            this.label1.Text = "*";
            // 
            // txtFixCapability
            // 
            this.txtFixCapability.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFixCapability.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFixCapability.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFixCapability.Location = new System.Drawing.Point(62, 20);
            this.txtFixCapability.MaxLength = 50;
            this.txtFixCapability.Name = "txtFixCapability";
            this.txtFixCapability.Size = new System.Drawing.Size(146, 21);
            this.txtFixCapability.TabIndex = 2;
            this.txtFixCapability.LostFocus += new System.EventHandler(this.txtFixCapability_LostFocus);
            this.txtFixCapability.GotFocus += new System.EventHandler(this.txtFixCapability_GotFocus);
            // 
            // lblFixCapability
            // 
            this.lblFixCapability.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFixCapability.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFixCapability.Location = new System.Drawing.Point(14, 23);
            this.lblFixCapability.Name = "lblFixCapability";
            this.lblFixCapability.Size = new System.Drawing.Size(35, 15);
            this.lblFixCapability.TabIndex = 0;
            this.lblFixCapability.Text = "Name";
            // 
            // grdFixCapability
            // 
            this.grdFixCapability.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFixCapability.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 122;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn2.Width = 239;
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 311;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdFixCapability.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdFixCapability.DisplayLayout.GroupByBox.Hidden = true;
            this.grdFixCapability.DisplayLayout.MaxColScrollRegions = 1;
            this.grdFixCapability.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdFixCapability.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdFixCapability.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdFixCapability.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdFixCapability.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdFixCapability.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdFixCapability.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdFixCapability.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdFixCapability.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdFixCapability.Location = new System.Drawing.Point(6, 0);
            this.grdFixCapability.Name = "grdFixCapability";
            this.grdFixCapability.Size = new System.Drawing.Size(332, 156);
            this.grdFixCapability.TabIndex = 58;
            this.grdFixCapability.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdFixCapability1_InitializeLayout);
            // 
            // FixCapability
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(344, 255);
            this.Controls.Add(this.grdFixCapability);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnReset);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "FixCapability";
            this.Text = "FixCapability";
            this.Load += new System.EventHandler(this.FixCapability_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFixCapability)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtFixCapability_GotFocus(object sender, System.EventArgs e)
        {
            txtFixCapability.BackColor = Color.LemonChiffon;
        }
        private void txtFixCapability_LostFocus(object sender, System.EventArgs e)
        {
            txtFixCapability.BackColor = Color.White;
        }

        #endregion


        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void FixCapability_Load(object sender, System.EventArgs e)
        {
            BindFixCapabilityGrid();
        }

        private void BindFixCapabilityGrid()
        {
            //this.dataGridTableStyle1.MappingName = "fixCapabilities";

            //Fetching the existing fixCapabilities from the database and binding it to the grid.
            FixCapabilities fixCapabilities = Fixmanager.GetFixCapabilities();
            //Assigning the grid's datasource to the fixCapabilities object.
            if (fixCapabilities.Count > 0)
            {
                grdFixCapability.DataSource = fixCapabilities;
            }
            else
            {
                FixCapabilities nullFixCapabilities = new FixCapabilities();
                nullFixCapabilities.Add(new Prana.Admin.BLL.FixCapability(int.MinValue, ""));
                grdFixCapability.DataSource = nullFixCapabilities;
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currenly saved id of the FixCapability in the database.
                int checkID = SaveFixCapability();
                //Binding grid after saving the a FixCapability.
                BindFixCapabilityGrid();
                if (checkID >= 0)
                {
                    //Calling refresh form after saving a FixCapability after checking that whether the 
                    //FixCapability is saved successfully or not by checking for the positive value of 
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
        /// This method saves the <see cref="FixCapability"/> to the database.
        /// </summary>
        /// <returns>FixCapabilityID, saved to the database</returns>
        private int SaveFixCapability()
        {
            errorProvider1.SetError(txtFixCapability, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.FixCapability fixCapability = new Prana.Admin.BLL.FixCapability();

            if (txtFixCapability.Tag != null)
            {
                //Update
                fixCapability.FixCapabilityID = int.Parse(txtFixCapability.Tag.ToString());
            }
            //Validation to check for the empty value in FixCapability textbox
            if (txtFixCapability.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtFixCapability, "Provide Value!");
                return result;
            }
            else
            {
                fixCapability.Name = txtFixCapability.Text.Trim();
                errorProvider1.SetError(txtFixCapability, "");
            }


            //Saving the FixCapability data and retrieving the fixCapabilityid for the newly added FixCapability.
            int newFixCapabilityID = Fixmanager.SaveFixCapability(fixCapability);
            //Showing the message: FixCapability already existing by checking the fixCapability id value to -1 
            if (newFixCapabilityID == -1)
            {
                MessageBox.Show("Fix Capability with the same name or tag already exists.", "Prana Alert", MessageBoxButtons.OK);
            }
            //Showing the message : FixCapability data saved
            else
            {
                txtFixCapability.Tag = null;
            }
            result = newFixCapabilityID;
            //Returning the newly added FixCapability id.
            return result;
        }

        //This method blanks the textboxes in the FixCapability form.
        private void RefreshForm()
        {
            txtFixCapability.Text = "";
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtFixCapability.Text = "";
            txtFixCapability.Tag = null;
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the FixCapability if the grid has any FixCapability.
                if (grdFixCapability.Rows.Count > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the fixCapability and fixCapabilityID.
                    txtFixCapability.Text = grdFixCapability.ActiveRow.Cells["Name"].Text.ToString();
                    txtFixCapability.Tag = grdFixCapability.ActiveRow.Cells["FixCapabilityID"].Text.ToString();
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
                //Check for deleting the fixCapability if the grid has any fixCapability.
                if (grdFixCapability.Rows.Count > 0)
                {
                    string fcName = grdFixCapability.ActiveRow.Cells["Name"].Text.ToString();
                    if (fcName != "")
                    {
                        //Asking the user to be sure about deleting the fixCapability.
                        if (MessageBox.Show(this, "Do you want to delete this FixCapability Type?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the fixCapabilityid from the currently selected row in the grid.
                            int fixCapabilityID = int.Parse(grdFixCapability.ActiveRow.Cells["FixCapabilityID"].Text.ToString());

                            bool chkVarraible = Fixmanager.DeleteFixCapability(fixCapabilityID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "FixCapability Type is referenced in Client.\n Please remove references first to delete it.", "Prana Alert");
                            }
                            else
                            {
                                BindFixCapabilityGrid();
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

        private void grdFixCapability1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }
    }
}
