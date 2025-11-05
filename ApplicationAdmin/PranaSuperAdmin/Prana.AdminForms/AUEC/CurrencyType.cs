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
    /// Summary description for CurrencyType.
    /// </summary>
    public class CurrencyType : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "CurrencyType : ";
        private const int GRD_CURRENCY_TYPE_ID = 0;
        private const int GRD_CURRENCY_TYPE = 1;

        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCurrencyType;
        private System.Windows.Forms.Label lblCurrencyType;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCurrencyType;
        private IContainer components;

        public CurrencyType()
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
                if (txtCurrencyType != null)
                {
                    txtCurrencyType.Dispose();
                }
                if (lblCurrencyType != null)
                {
                    lblCurrencyType.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (grdCurrencyType != null)
                {
                    grdCurrencyType.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurrencyType));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyTypeID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyTypeName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 2);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCurrencyType = new System.Windows.Forms.TextBox();
            this.lblCurrencyType = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grdCurrencyType = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCurrencyType)).BeginInit();
            this.SuspendLayout();
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnEdit.Location = new System.Drawing.Point(96, 180);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 91;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnReset.Location = new System.Drawing.Point(135, 254);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 92;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDelete.Location = new System.Drawing.Point(174, 180);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 90;
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
            this.btnClose.Location = new System.Drawing.Point(213, 254);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 89;
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
            this.btnSave.Location = new System.Drawing.Point(57, 254);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 88;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCurrencyType);
            this.groupBox1.Controls.Add(this.lblCurrencyType);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(63, 208);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 44);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Update CurrencyType";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(42, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 8;
            this.label1.Text = "*";
            // 
            // txtCurrencyType
            // 
            this.txtCurrencyType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCurrencyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCurrencyType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCurrencyType.Location = new System.Drawing.Point(54, 20);
            this.txtCurrencyType.MaxLength = 50;
            this.txtCurrencyType.Name = "txtCurrencyType";
            this.txtCurrencyType.Size = new System.Drawing.Size(148, 21);
            this.txtCurrencyType.TabIndex = 2;
            this.txtCurrencyType.LostFocus += new System.EventHandler(this.txtCurrencyType_LostFocus);
            this.txtCurrencyType.GotFocus += new System.EventHandler(this.txtCurrencyType_GotFocus);
            this.txtCurrencyType.TextChanged += new System.EventHandler(this.txtCurrencyType_TextChanged);
            // 
            // lblCurrencyType
            // 
            this.lblCurrencyType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCurrencyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCurrencyType.Location = new System.Drawing.Point(14, 22);
            this.lblCurrencyType.Name = "lblCurrencyType";
            this.lblCurrencyType.Size = new System.Drawing.Size(34, 15);
            this.lblCurrencyType.TabIndex = 0;
            this.lblCurrencyType.Text = "Type";
            this.lblCurrencyType.Click += new System.EventHandler(this.lblCurrencyType_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grdCurrencyType
            // 
            this.grdCurrencyType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCurrencyType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 273;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn2.Width = 275;
            appearance1.FontData.BoldAsString = "True";
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.Header.Appearance = appearance1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 305;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCurrencyType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCurrencyType.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCurrencyType.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCurrencyType.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCurrencyType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCurrencyType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCurrencyType.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCurrencyType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCurrencyType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCurrencyType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCurrencyType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCurrencyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdCurrencyType.Location = new System.Drawing.Point(9, 2);
            this.grdCurrencyType.Name = "grdCurrencyType";
            this.grdCurrencyType.Size = new System.Drawing.Size(326, 177);
            this.grdCurrencyType.TabIndex = 93;
            // 
            // CurrencyType
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(344, 277);
            this.Controls.Add(this.grdCurrencyType);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "CurrencyType";
            this.Text = "CurrencyType";
            this.Load += new System.EventHandler(this.CurrencyType_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCurrencyType)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        #region Focus Colors
        private void txtCurrencyType_GotFocus(object sender, System.EventArgs e)
        {
            txtCurrencyType.BackColor = Color.LemonChiffon;
        }
        private void txtCurrencyType_LostFocus(object sender, System.EventArgs e)
        {
            txtCurrencyType.BackColor = Color.White;
        }
        #endregion

        /// <summary>
        /// Binds the grid with all the Currency Types in the database.
        /// </summary>
        private void BindCurrencyTypeGrid()
        {
            //this.dataGridTableStyle1.MappingName = "currencyTypes";

            //Fetching the existing currencyTypes from the database and binding it to the grid.
            CurrencyTypes currencyTypes = AUECManager.GetCurrencyTypes();
            //Assigning the grid's datasource to the currencyTypes object.

            if (currencyTypes.Count <= 0)
            {
                currencyTypes.Add(new Prana.Admin.BLL.CurrencyType(int.MinValue, ""));
            }
            grdCurrencyType.DataSource = currencyTypes;
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currently saved id of the CurrencyType in the database.
                int checkID = SaveCurrencyType();
                //Binding grid after saving a CurrencyType.
                BindCurrencyTypeGrid();
                if (checkID >= 0)
                {
                    //Calling refresh form after saving a CurrencyType after checking that whether the 
                    //CurrencyType is saved successfully or not by checking for the positive value of 
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
        /// This method saves the <see cref="CurrencyType"/> to the database.
        /// </summary>
        /// <returns>CurrencyType, saved to the database</returns>
        private int SaveCurrencyType()
        {
            errorProvider1.SetError(txtCurrencyType, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.CurrencyType currencyType = new Prana.Admin.BLL.CurrencyType();

            if (txtCurrencyType.Tag != null)
            {
                //Update
                currencyType.CurrencyTypeID = int.Parse(txtCurrencyType.Tag.ToString());
            }
            //Validation to check for the empty value in CurrencyType textbox
            if (txtCurrencyType.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtCurrencyType, "Provide Value!");
                return result;
            }
            else
            {
                currencyType.Type = txtCurrencyType.Text.Trim();
                errorProvider1.SetError(txtCurrencyType, "");
            }

            //Saving the CurrencyType data and retrieving the CurrencyType for the newly added CurrencyType.
            int newCurrencyTypeID = AUECManager.SaveCurrencyType(currencyType);
            //Showing the message: CurencyType already existing by checking the CurencyType id value to -1 
            if (newCurrencyTypeID == -1)
            {
                MessageBox.Show("CurrencyType with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
            }
            //Showing the message : CurrencyType data saved
            else
            {
                txtCurrencyType.Tag = null;
            }
            result = newCurrencyTypeID;
            //Returning the newly added CurrencyType id.
            return result;
        }

        private void CurrencyType_Load(object sender, System.EventArgs e)
        {
            BindCurrencyTypeGrid();
        }

        //This method blanks the textboxes in the CurrencyType form.
        private void RefreshForm()
        {
            txtCurrencyType.Text = "";
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtCurrencyType.Text = "";
            txtCurrencyType.Tag = null;
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the CurrencyType if the grid has any CurrencyType.
                if (grdCurrencyType.Rows.Count > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the currencyType and currencyTypID.
                    txtCurrencyType.Text = grdCurrencyType.ActiveRow.Cells["Type"].Text.ToString();

                    txtCurrencyType.Tag = grdCurrencyType.ActiveRow.Cells["CurrencyTypeID"].Text.ToString();
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
                //Check for deleting the CurrencyType if the grid has any currencyType.
                if (grdCurrencyType.Rows.Count > 0)
                {
                    string currencyType = grdCurrencyType.ActiveRow.Cells["Type"].Text.ToString();
                    if (currencyType != "")
                    {
                        //Asking the user to be sure about deleting the currencyType.
                        if (MessageBox.Show(this, "Do you want to delete this CurrencyType?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the currencyTypeid from the currently selected row in the grid.
                            int currencyTypeID = int.Parse(grdCurrencyType.ActiveRow.Cells["CurrencyTypeID"].Text.ToString());

                            bool chkVarraible = AUECManager.DeleteCurrencyType(currencyTypeID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "CurrencyTypeID is referenced in Currency/CounterPartyVenue.\n Please remove references first to delete it.", "Prana Alert");
                            }
                            else
                            {
                                BindCurrencyTypeGrid();
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

        private void txtCurrencyType_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }

        private void lblCurrencyType_Click(object sender, System.EventArgs e)
        {

        }
    }

}
