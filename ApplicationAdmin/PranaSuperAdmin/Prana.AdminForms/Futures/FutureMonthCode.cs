#region Using
using Infragistics.Win;
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for FutureMonthCode.
    /// </summary>
    public class FutureMonthCode : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "FutureMonthCode : ";
        private const int GRD_FUTUREMONTH_ID = 0;
        private const int GRD_FUTUREMONTH = 1;
        private const int GRD_ABBREVIATION = 2;

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAbbreviation;
        private System.Windows.Forms.TextBox txtFutureMonth;
        private System.Windows.Forms.Label lblFutureMonth;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtAbbreviation;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdFutureMonth;
        private System.Windows.Forms.GroupBox grpFutureMonths;
        private System.Windows.Forms.Label label1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public FutureMonthCode()
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
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (lblAbbreviation != null)
                {
                    lblAbbreviation.Dispose();
                }
                if (txtFutureMonth != null)
                {
                    txtFutureMonth.Dispose();
                }
                if (lblFutureMonth != null)
                {
                    lblFutureMonth.Dispose();
                }
                if (txtAbbreviation != null)
                {
                    txtAbbreviation.Dispose();
                }
                if (grdFutureMonth != null)
                {
                    grdFutureMonth.Dispose();
                }
                if (grpFutureMonths != null)
                {
                    grpFutureMonths.Dispose();
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FutureMonthCode));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FutureMonthCodeID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FutureMonth", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Abbreviation", 2, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpFutureMonths = new System.Windows.Forms.GroupBox();
            this.txtAbbreviation = new System.Windows.Forms.TextBox();
            this.lblAbbreviation = new System.Windows.Forms.Label();
            this.txtFutureMonth = new System.Windows.Forms.TextBox();
            this.lblFutureMonth = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
            this.grdFutureMonth = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.grpFutureMonths.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdFutureMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(240)), ((System.Byte)(150)));
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnReset.Location = new System.Drawing.Point(134, 232);
            this.btnReset.Name = "btnReset";
            this.btnReset.TabIndex = 43;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(96, 138);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.TabIndex = 42;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(174, 138);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.TabIndex = 41;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(214, 232);
            this.btnClose.Name = "btnClose";
            this.btnClose.TabIndex = 40;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(56, 232);
            this.btnSave.Name = "btnSave";
            this.btnSave.TabIndex = 39;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpFutureMonths
            // 
            this.grpFutureMonths.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.grpFutureMonths.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.grpFutureMonths.Controls.Add(this.label1);
            this.grpFutureMonths.Controls.Add(this.txtAbbreviation);
            this.grpFutureMonths.Controls.Add(this.lblAbbreviation);
            this.grpFutureMonths.Controls.Add(this.txtFutureMonth);
            this.grpFutureMonths.Controls.Add(this.lblFutureMonth);
            this.grpFutureMonths.Controls.Add(this.label3);
            this.grpFutureMonths.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpFutureMonths.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpFutureMonths.Location = new System.Drawing.Point(31, 164);
            this.grpFutureMonths.Name = "grpFutureMonths";
            this.grpFutureMonths.Size = new System.Drawing.Size(283, 66);
            this.grpFutureMonths.TabIndex = 37;
            this.grpFutureMonths.TabStop = false;
            this.grpFutureMonths.Text = "Add/Update Future Month";
            // 
            // txtAbbreviation
            // 
            this.txtAbbreviation.BackColor = System.Drawing.Color.White;
            this.txtAbbreviation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAbbreviation.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAbbreviation.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtAbbreviation.Location = new System.Drawing.Point(102, 40);
            this.txtAbbreviation.MaxLength = 10;
            this.txtAbbreviation.Name = "txtAbbreviation";
            this.txtAbbreviation.Size = new System.Drawing.Size(164, 21);
            this.txtAbbreviation.TabIndex = 9;
            this.txtAbbreviation.Text = "";
            this.txtAbbreviation.LostFocus += new System.EventHandler(this.txtAbbreviation_LostFocus);
            this.txtAbbreviation.GotFocus += new System.EventHandler(this.txtAbbreviation_GotFocus);
            // 
            // lblAbbreviation
            // 
            this.lblAbbreviation.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.lblAbbreviation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAbbreviation.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAbbreviation.Location = new System.Drawing.Point(16, 42);
            this.lblAbbreviation.Name = "lblAbbreviation";
            this.lblAbbreviation.Size = new System.Drawing.Size(70, 15);
            this.lblAbbreviation.TabIndex = 8;
            this.lblAbbreviation.Text = "Abbreviation";
            // 
            // txtFutureMonth
            // 
            this.txtFutureMonth.BackColor = System.Drawing.Color.White;
            this.txtFutureMonth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFutureMonth.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFutureMonth.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFutureMonth.Location = new System.Drawing.Point(102, 18);
            this.txtFutureMonth.MaxLength = 50;
            this.txtFutureMonth.Name = "txtFutureMonth";
            this.txtFutureMonth.Size = new System.Drawing.Size(164, 21);
            this.txtFutureMonth.TabIndex = 2;
            this.txtFutureMonth.Text = "";
            this.txtFutureMonth.LostFocus += new System.EventHandler(this.txtFutureMonth_LostFocus);
            this.txtFutureMonth.GotFocus += new System.EventHandler(this.txtFutureMonth_GotFocus);
            // 
            // lblFutureMonth
            // 
            this.lblFutureMonth.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.lblFutureMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFutureMonth.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFutureMonth.Location = new System.Drawing.Point(14, 22);
            this.lblFutureMonth.Name = "lblFutureMonth";
            this.lblFutureMonth.Size = new System.Drawing.Size(71, 15);
            this.lblFutureMonth.TabIndex = 0;
            this.lblFutureMonth.Text = "Future Month";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(86, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 8);
            this.label3.TabIndex = 7;
            this.label3.Text = "*";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grdFutureMonth
            // 
            this.grdFutureMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.grdFutureMonth.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 293;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 143;
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 172;
            ultraGridBand1.Columns.AddRange(new object[] {
                                                             ultraGridColumn1,
                                                             ultraGridColumn2,
                                                             ultraGridColumn3});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdFutureMonth.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdFutureMonth.DisplayLayout.GroupByBox.Hidden = true;
            this.grdFutureMonth.DisplayLayout.MaxColScrollRegions = 1;
            this.grdFutureMonth.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdFutureMonth.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdFutureMonth.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdFutureMonth.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdFutureMonth.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdFutureMonth.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdFutureMonth.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdFutureMonth.UseFlatMode = DefaultableBoolean.True;
            this.grdFutureMonth.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdFutureMonth.Location = new System.Drawing.Point(4, 0);
            this.grdFutureMonth.Name = "grdFutureMonth";
            this.grdFutureMonth.Size = new System.Drawing.Size(336, 136);
            this.grdFutureMonth.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(86, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 10;
            this.label1.Text = "*";
            // 
            // FutureMonthCode
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ClientSize = new System.Drawing.Size(344, 255);
            this.Controls.Add(this.grdFutureMonth);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpFutureMonths);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "FutureMonthCode";
            this.Text = "FutureMonthCode";
            this.Load += new System.EventHandler(this.FutureMonthCode_Load);
            this.grpFutureMonths.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdFutureMonth)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtFutureMonth_GotFocus(object sender, System.EventArgs e)
        {
            txtFutureMonth.BackColor = Color.LemonChiffon;
        }
        private void txtFutureMonth_LostFocus(object sender, System.EventArgs e)
        {
            txtFutureMonth.BackColor = Color.White;
        }
        private void txtAbbreviation_GotFocus(object sender, System.EventArgs e)
        {
            txtAbbreviation.BackColor = Color.LemonChiffon;
        }
        private void txtAbbreviation_LostFocus(object sender, System.EventArgs e)
        {
            txtAbbreviation.BackColor = Color.White;
        }

        #endregion

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void BindFutureMonthGrid()
        {
            //this.dataGridTableStyle1.MappingName = "futureMonthCodes";

            //Fetching the existing futureMonthCodes from the database and binding it to the grid.
            FutureMonthCodes futureMonthCodes = FutureManager.GetFutureMonthCodes();
            //Assigning the grid's datasource to the futureMonthCodes object.
            grdFutureMonth.DataSource = futureMonthCodes;
        }

        private void FutureMonthCode_Load(object sender, System.EventArgs e)
        {
            BindFutureMonthGrid();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currenly saved id of the HandlingInstruction in the database.
                int checkID = SaveHandlingInstruction();
                //Binding grid after saving the a HandlingInstruction.
                BindFutureMonthGrid();
                if (checkID >= 0)
                {
                    //Calling refresh form after saving a HandlingInstruction after checking that whether the 
                    //HandlingInstruction is saved successfully or not by checking for the positive value of 
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
        /// This method saves the <see cref="FutureMonthCode"/> to the database.
        /// </summary>
        /// <returns>FutureMonthCodeID, saved to the database</returns>
        private int SaveHandlingInstruction()
        {
            errorProvider1.SetError(txtFutureMonth, "");
            errorProvider1.SetError(txtAbbreviation, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.FutureMonthCode futureMonthCode = new Prana.Admin.BLL.FutureMonthCode();

            if (txtFutureMonth.Tag != null)
            {
                //Update
                futureMonthCode.FutureMonthCodeID = int.Parse(txtFutureMonth.Tag.ToString());
            }
            //Validation to check for the empty value in FutureMonth textbox
            if (txtFutureMonth.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtFutureMonth, "Provide Value!");
                return result;
            }
            else
            {
                futureMonthCode.FutureMonth = txtFutureMonth.Text.Trim();
                errorProvider1.SetError(txtFutureMonth, "");
            }
            if (txtAbbreviation.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtAbbreviation, "Provide Value!");
                return result;
            }
            else
            {
                futureMonthCode.Abbreviation = txtAbbreviation.Text.Trim();
                errorProvider1.SetError(txtAbbreviation, "");
            }

            //Saving the futureMonth data and retrieving the futureMonthid for the newly added futureMonth.
            int newFutureMonthID = FutureManager.SaveFutureMonthCode(futureMonthCode);
            //Showing the message: FutureMonth already existing by checking the FutureMonth id value to -1 
            if (newFutureMonthID == -1)
            {
                errorProvider1.SetError(btnSave, "futureMonth Already Exists");
            }
            //Showing the message : futureMonth data saved
            else
            {
                //errorProvider1.SetError(txtFutureMonth, "FutureMonth Saved");
                txtFutureMonth.Tag = null;
            }
            result = newFutureMonthID;
            //Returning the newly added futureMonth id.
            return result;
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the Future Month if the grid has any FutureMonth.
                if (grdFutureMonth.Rows.Count > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the Future Month and futureMonthID.
                    txtFutureMonth.Text = grdFutureMonth.ActiveRow.Cells["FutureMonth"].Text.ToString();
                    txtFutureMonth.Tag = grdFutureMonth.ActiveRow.Cells["FutureMonthCodeID"].Text.ToString();
                    txtAbbreviation.Text = grdFutureMonth.ActiveRow.Cells["Abbreviation"].Text.ToString();
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
                //Check for deleting the futureCode if the grid has any futureCode.
                if (grdFutureMonth.Rows.Count > 0)
                {
                    //Asking the user to be sure about deleting the futureCode.
                    if (MessageBox.Show(this, "Do you want to delete this FutureMonth?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //Getting the futureMonthid from the currently selected row in the grid.
                        int futureMonthID = int.Parse(grdFutureMonth.ActiveRow.Cells["FutureMonthCodeID"].Text.ToString());

                        bool chkVarraible = FutureManager.DeleteFutureMonthCode(futureMonthID, false);
                        if (!(chkVarraible))
                        {
                            MessageBox.Show(this, "FutureMonthis referenced in CounterpartyVenue.\n Please remove references first to delete it.", "Prana Alert");
                        }
                        else
                        {
                            BindFutureMonthGrid();
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

        //This method blanks the textboxes in the FutureMonth form.
        private void RefreshForm()
        {
            txtFutureMonth.Text = "";
            txtAbbreviation.Text = "";
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtFutureMonth.Text = "";
            txtFutureMonth.Tag = null;
            txtAbbreviation.Text = "";
        }
    }
}
