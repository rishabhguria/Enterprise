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
    /// Summary description for CounterPartyType.
    /// </summary>
    public class CounterPartyType : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "BrokerType : ";
        private const int GRD_COUNTERPARTYTYPE_ID = 0;
        private const int GRD_COUNTERPARTYTYPE = 1;

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtCounterPartyType;
        private System.Windows.Forms.Label lblCounterPartyType;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCounterPartyType;
        private IContainer components;

        public CounterPartyType()
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
                if (txtCounterPartyType != null)
                {
                    txtCounterPartyType.Dispose();
                }
                if (lblCounterPartyType != null)
                {
                    lblCounterPartyType.Dispose();
                }
                if (grdCounterPartyType != null)
                {
                    grdCounterPartyType.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CounterPartyType));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyTypeID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCounterPartyType = new System.Windows.Forms.TextBox();
            this.lblCounterPartyType = new System.Windows.Forms.Label();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.grdCounterPartyType = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCounterPartyType)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCounterPartyType);
            this.groupBox1.Controls.Add(this.lblCounterPartyType);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(59, 177);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 46);
            this.groupBox1.TabIndex = 72;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Update BrokerType";
            this.groupBox1.Visible = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(44, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 8;
            this.label1.Text = "*";
            // 
            // txtCounterPartyType
            // 
            this.txtCounterPartyType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCounterPartyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCounterPartyType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCounterPartyType.Location = new System.Drawing.Point(58, 22);
            this.txtCounterPartyType.MaxLength = 50;
            this.txtCounterPartyType.Name = "txtCounterPartyType";
            this.txtCounterPartyType.Size = new System.Drawing.Size(154, 21);
            this.txtCounterPartyType.TabIndex = 2;
            this.txtCounterPartyType.LostFocus += new System.EventHandler(this.txtCounterPartyType_LostFocus);
            this.txtCounterPartyType.GotFocus += new System.EventHandler(this.txtCounterPartyType_GotFocus);
            this.txtCounterPartyType.TextChanged += new System.EventHandler(this.txtCounterPartyType_TextChanged);
            // 
            // lblCounterPartyType
            // 
            this.lblCounterPartyType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCounterPartyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblCounterPartyType.Location = new System.Drawing.Point(14, 24);
            this.lblCounterPartyType.Name = "lblCounterPartyType";
            this.lblCounterPartyType.Size = new System.Drawing.Size(33, 15);
            this.lblCounterPartyType.TabIndex = 0;
            this.lblCounterPartyType.Text = "Type";
            this.lblCounterPartyType.Click += new System.EventHandler(this.lblCounterPartyType_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnEdit.Location = new System.Drawing.Point(98, 151);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 77;
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
            this.btnReset.Location = new System.Drawing.Point(222, 225);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 78;
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
            this.btnDelete.Location = new System.Drawing.Point(174, 151);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 76;
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
            this.btnClose.Location = new System.Drawing.Point(141, 225);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 75;
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
            this.btnSave.Location = new System.Drawing.Point(60, 225);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 74;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grdCounterPartyType
            // 
            this.grdCounterPartyType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdCounterPartyType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 273;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 308;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCounterPartyType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCounterPartyType.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCounterPartyType.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCounterPartyType.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdCounterPartyType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCounterPartyType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCounterPartyType.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCounterPartyType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCounterPartyType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCounterPartyType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCounterPartyType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdCounterPartyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdCounterPartyType.Location = new System.Drawing.Point(9, 0);
            this.grdCounterPartyType.Name = "grdCounterPartyType";
            this.grdCounterPartyType.Size = new System.Drawing.Size(329, 223);
            this.grdCounterPartyType.TabIndex = 79;
            this.grdCounterPartyType.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdCounterPartyType_InitializeLayout);
            // 
            // CounterPartyType
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(346, 250);
            this.Controls.Add(this.grdCounterPartyType);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "CounterPartyType";
            this.Text = "Broker Type";
            this.Load += new System.EventHandler(this.CounterPartyType_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdCounterPartyType)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtCounterPartyType_GotFocus(object sender, System.EventArgs e)
        {
            txtCounterPartyType.BackColor = Color.LemonChiffon;
        }
        private void txtCounterPartyType_LostFocus(object sender, System.EventArgs e)
        {
            txtCounterPartyType.BackColor = Color.White;
        }

        #endregion
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void BindCounterPartyTypeGrid()
        {
            //this.dataGridTableStyle1.MappingName = "counterPartyTypes";

            //Fetching the existing counterPartyTypes from the database and binding it to the grid.
            CounterPartyTypes counterPartyTypes = CounterPartyManager.GetCounterPartyTypes();
            //Assigning the grid's datasource to the counterPartyTypes object.
            if (counterPartyTypes.Count > 0)
            {
                grdCounterPartyType.DataSource = counterPartyTypes;
            }
            else
            {
                CounterPartyTypes nullCounterPartyTypes = new CounterPartyTypes();
                nullCounterPartyTypes.Add(new Prana.Admin.BLL.CounterPartyType(int.MinValue, ""));
                grdCounterPartyType.DataSource = nullCounterPartyTypes;
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currenly saved id of the CounterPartyType in the database.
                int checkID = SaveCounterPartyType();
                //Binding grid after saving the a CounterPartyType.
                BindCounterPartyTypeGrid();
                if (checkID >= 0)
                {
                    //Calling refresh form after saving a CounterPartyType after checking that whether the 
                    //CounterPartyType is saved successfully or not by checking for the positive value of 
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
        /// This method saves the <see cref="CounterPartyType"/> to the database.
        /// </summary>
        /// <returns>CounterPartyType, saved to the database</returns>
        private int SaveCounterPartyType()
        {
            errorProvider1.SetError(txtCounterPartyType, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.CounterPartyType counterPartyType = new Prana.Admin.BLL.CounterPartyType();

            if (txtCounterPartyType.Tag != null)
            {
                //Update
                counterPartyType.CounterPartyTypeID = int.Parse(txtCounterPartyType.Tag.ToString());
            }
            //Validation to check for the empty value in CounterPartyType textbox
            if (txtCounterPartyType.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtCounterPartyType, "Provide Value!");
                return result;
            }
            else
            {
                counterPartyType.Type = txtCounterPartyType.Text.Trim();
                errorProvider1.SetError(txtCounterPartyType, "");
            }


            //Saving the CounterPartyType data and retrieving the CounterPartyTypeid for the newly added CounterPartyType.
            int newCounterPartyTypeID = CounterPartyManager.SaveCounterPartyType(counterPartyType);
            //Showing the message: CounterPartyType already existing by checking the CounterPartyType id value to -1 
            if (newCounterPartyTypeID == -1)
            {
                MessageBox.Show("BrokerType with the same name already exists.", "Prana Alert", MessageBoxButtons.OK);
            }
            //Showing the message : CounterPartyType data saved
            else
            {
                txtCounterPartyType.Tag = null;
            }
            result = newCounterPartyTypeID;
            //Returning the newly added CounterPartyType id.
            return result;
        }

        //This method blanks the textboxes in the CounterPartyType form.
        private void RefreshForm()
        {
            txtCounterPartyType.Text = "";
        }

        private void CounterPartyType_Load(object sender, System.EventArgs e)
        {
            BindCounterPartyTypeGrid();
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtCounterPartyType.Text = "";
            txtCounterPartyType.Tag = null;
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the CounterPartyType if the grid has any CounterPartyType.
                if (grdCounterPartyType.Rows.Count > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the counterPartyType and counterPartyTypeID.
                    txtCounterPartyType.Text = grdCounterPartyType.ActiveRow.Cells["Type"].Text.ToString();
                    txtCounterPartyType.Tag = grdCounterPartyType.ActiveRow.Cells["CounterPartyTypeID"].Text.ToString();
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
                //Check for deleting the counterPartyType if the grid has any counterPartyType.
                if (grdCounterPartyType.Rows.Count > 0)
                {
                    string type = grdCounterPartyType.ActiveRow.Cells["Type"].Text.ToString();
                    if (type != "")
                    {
                        //Asking the user to be sure about deleting the counterPartyType.
                        if (MessageBox.Show(this, "Do you want to delete this Broker Type?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the counterPartyTypeid from the currently selected row in the grid.
                            int counterPartyTypeID = int.Parse(grdCounterPartyType.ActiveRow.Cells["CounterPartyTypeID"].Value.ToString());

                            bool chkVarraible = CounterPartyManager.DeleteCounterPartyType(counterPartyTypeID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "Broker Type is referenced in CounterParty.\n Please remove references first to delete it.", "Prana Alert");
                            }
                            else
                            {
                                BindCounterPartyTypeGrid();
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

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }

        private void txtCounterPartyType_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void lblCounterPartyType_Click(object sender, System.EventArgs e)
        {

        }

        private void grdCounterPartyType_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }
    }
}
