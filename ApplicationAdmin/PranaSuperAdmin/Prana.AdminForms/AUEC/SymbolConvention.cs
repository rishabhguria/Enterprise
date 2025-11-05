#region Using
using Infragistics.Win;
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
    /// Summary description for SymbolConvention.
    /// </summary>
    public class SymbolConvention : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "SymbolConvention : ";
        private const int GRD_SYMBOLCONVENTION_ID = 0;
        private const int GRD_SYMBOLCONVENTION = 1;
        private const int GRD_SYMBOLCONVENTION_SHORTNAME = 2;

        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSymbolConventionShortName;
        private System.Windows.Forms.Label lblSymbolConventionShortName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSymbolConvention;
        private System.Windows.Forms.Label lblSymbolConventionName;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSymbolConvention;
        private Label label2;
        private IContainer components;

        public SymbolConvention()
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
                if (txtSymbolConventionShortName != null)
                {
                    txtSymbolConventionShortName.Dispose();
                }
                if (lblSymbolConventionShortName != null)
                {
                    lblSymbolConventionShortName.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (txtSymbolConvention != null)
                {
                    txtSymbolConvention.Dispose();
                }
                if (lblSymbolConventionName != null)
                {
                    lblSymbolConventionName.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (grdSymbolConvention != null)
                {
                    grdSymbolConvention.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolConvention));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SymbolConventionID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SymbolConventionName", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 2);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 3);
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSymbolConventionShortName = new System.Windows.Forms.TextBox();
            this.lblSymbolConventionShortName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSymbolConvention = new System.Windows.Forms.TextBox();
            this.lblSymbolConventionName = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grdSymbolConvention = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSymbolConvention)).BeginInit();
            this.SuspendLayout();
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(208, 215);
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
            this.btnReset.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnReset.Location = new System.Drawing.Point(247, 309);
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
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(286, 215);
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
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(327, 309);
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
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(167, 309);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 88;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSymbolConventionShortName);
            this.groupBox1.Controls.Add(this.lblSymbolConventionShortName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSymbolConvention);
            this.groupBox1.Controls.Add(this.lblSymbolConventionName);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(118, 243);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 64);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Update SymbolConvention";
            // 
            // txtSymbolConventionShortName
            // 
            this.txtSymbolConventionShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSymbolConventionShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSymbolConventionShortName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSymbolConventionShortName.Location = new System.Drawing.Point(158, 42);
            this.txtSymbolConventionShortName.MaxLength = 20;
            this.txtSymbolConventionShortName.Name = "txtSymbolConventionShortName";
            this.txtSymbolConventionShortName.Size = new System.Drawing.Size(154, 21);
            this.txtSymbolConventionShortName.TabIndex = 10;
            this.txtSymbolConventionShortName.LostFocus += new System.EventHandler(this.txtSymbolConventionShortName_LostFocus);
            this.txtSymbolConventionShortName.GotFocus += new System.EventHandler(this.txtSymbolConventionShortName_GotFocus);
            // 
            // lblSymbolConventionShortName
            // 
            this.lblSymbolConventionShortName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblSymbolConventionShortName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSymbolConventionShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSymbolConventionShortName.Location = new System.Drawing.Point(14, 45);
            this.lblSymbolConventionShortName.Name = "lblSymbolConventionShortName";
            this.lblSymbolConventionShortName.Size = new System.Drawing.Size(62, 15);
            this.lblSymbolConventionShortName.TabIndex = 9;
            this.lblSymbolConventionShortName.Text = "ShortName";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(144, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 8;
            this.label1.Text = "*";
            // 
            // txtSymbolConvention
            // 
            this.txtSymbolConvention.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSymbolConvention.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSymbolConvention.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSymbolConvention.Location = new System.Drawing.Point(158, 20);
            this.txtSymbolConvention.MaxLength = 100;
            this.txtSymbolConvention.Name = "txtSymbolConvention";
            this.txtSymbolConvention.Size = new System.Drawing.Size(154, 21);
            this.txtSymbolConvention.TabIndex = 2;
            this.txtSymbolConvention.LostFocus += new System.EventHandler(this.txtSymbolConvention_LostFocus);
            this.txtSymbolConvention.GotFocus += new System.EventHandler(this.txtSymbolConvention_GotFocus);
            // 
            // lblSymbolConventionName
            // 
            this.lblSymbolConventionName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.lblSymbolConventionName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSymbolConventionName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSymbolConventionName.Location = new System.Drawing.Point(14, 23);
            this.lblSymbolConventionName.Name = "lblSymbolConventionName";
            this.lblSymbolConventionName.Size = new System.Drawing.Size(134, 15);
            this.lblSymbolConventionName.TabIndex = 0;
            this.lblSymbolConventionName.Text = "Symbol Convention Name";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grdSymbolConvention
            // 
            this.grdSymbolConvention.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSymbolConvention.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 142;
            appearance1.FontData.BoldAsString = "True";
            ultraGridColumn2.Header.Appearance = appearance1;
            ultraGridColumn2.Header.Caption = "Name";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 350;
            appearance2.FontData.BoldAsString = "True";
            ultraGridColumn3.Header.Appearance = appearance2;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 194;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 76;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSymbolConvention.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSymbolConvention.DisplayLayout.GroupByBox.Hidden = true;
            this.grdSymbolConvention.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSymbolConvention.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdSymbolConvention.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdSymbolConvention.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdSymbolConvention.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdSymbolConvention.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdSymbolConvention.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdSymbolConvention.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdSymbolConvention.UseFlatMode = DefaultableBoolean.True;
            this.grdSymbolConvention.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdSymbolConvention.Location = new System.Drawing.Point(2, 0);
            this.grdSymbolConvention.Name = "grdSymbolConvention";
            this.grdSymbolConvention.Size = new System.Drawing.Size(565, 213);
            this.grdSymbolConvention.TabIndex = 93;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(144, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 8);
            this.label2.TabIndex = 11;
            this.label2.Text = "*";
            // 
            // SymbolConvention
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(569, 334);
            this.Controls.Add(this.grdSymbolConvention);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(575, 366);
            this.Name = "SymbolConvention";
            this.Text = "Symbol Convention";
            this.Load += new System.EventHandler(this.SymbolConvention_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSymbolConvention)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtSymbolConventionShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtSymbolConventionShortName.BackColor = Color.LemonChiffon;
        }
        private void txtSymbolConventionShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtSymbolConventionShortName.BackColor = Color.White;
        }
        private void txtSymbolConvention_GotFocus(object sender, System.EventArgs e)
        {
            txtSymbolConvention.BackColor = Color.LemonChiffon;
        }
        private void txtSymbolConvention_LostFocus(object sender, System.EventArgs e)
        {
            txtSymbolConvention.BackColor = Color.White;
        }
        #endregion

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void SymbolConvention_Load(object sender, System.EventArgs e)
        {
            BindSymbolConventionGrid();
        }

        private void BindSymbolConventionGrid()
        {
            //this.dataGridTableStyle1.MappingName = "symbolConventions";

            //Fetching the existing symbolConventions from the database and binding it to the grid.
            SymbolConventions symbolConventions = SymbolManager.GetSymbolConventions();
            //Assigning the grid's datasource to the symbolConventions object.
            if (symbolConventions.Count > 0)
            {
                grdSymbolConvention.DataSource = symbolConventions;
            }
            else
            {
                SymbolConventions nullSymbolConventions = new SymbolConventions();
                nullSymbolConventions.Add(new Prana.Admin.BLL.SymbolConvention(int.MinValue, "", ""));
                grdSymbolConvention.DataSource = nullSymbolConventions;
            }

        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            //The checkID is used to store the currenly saved id of the SymbolConvention in the database.
            int checkID = SaveSymbolConvention();
            //Binding grid after saving a SymbolConvention.
            BindSymbolConventionGrid();
            if (checkID >= 0)
            {
                //Calling refresh form after saving a SymbolConvention after checking that whether the 
                //SymbolConvention is saved successfully or not by checking for the positive value of 
                //checkID.
                RefreshForm();
            }
        }

        /// <summary>
        /// This method saves the <see cref="SymbolConvention"/> to the database.
        /// </summary>
        /// <returns>SymbolConvention, saved to the database</returns>
        private int SaveSymbolConvention()
        {
            errorProvider1.SetError(txtSymbolConvention, "");
            errorProvider1.SetError(txtSymbolConventionShortName, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.SymbolConvention symbolConvention = new Prana.Admin.BLL.SymbolConvention();

            if (txtSymbolConvention.Tag != null)
            {
                //Update
                symbolConvention.SymbolConventionID = int.Parse(txtSymbolConvention.Tag.ToString());
            }
            //Validation to check for the empty value in SymbolConvention textbox
            if (txtSymbolConvention.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtSymbolConvention, "Provide Value!");
                return result;
            }
            else if (txtSymbolConventionShortName.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtSymbolConventionShortName, "Provide Value Symbol Convention ShortName!");
                txtSymbolConventionShortName.Focus();
                return result;
            }
            else
            {
                symbolConvention.SymbolConventionName = txtSymbolConvention.Text.Trim();
                errorProvider1.SetError(txtSymbolConvention, "");
            }
            symbolConvention.ShortName = txtSymbolConventionShortName.Text.Trim();

            //Saving the SymbolConvention data and retrieving the SymbolConvention for the newly added SymbolConvention.
            int newSymbolConventionID = SymbolManager.SaveSymbolConventions(symbolConvention);
            //Showing the message: SymbolConvention already existing by checking the SymbolConvention id value to -1 
            if (newSymbolConventionID == -1)
            {
                MessageBox.Show("Symbol Convention with the same name or short name already exists.", "Prana Alert", MessageBoxButtons.OK);
            }
            //Showing the message : SymbolConvention data saved
            else
            {
                txtSymbolConvention.Tag = null;
            }
            result = newSymbolConventionID;
            //Returning the newly added SymbolConvention id.
            return result;
        }
        //This method blanks the textboxes in the SymbolConvention form.
        private void RefreshForm()
        {
            txtSymbolConvention.Text = "";
            txtSymbolConventionShortName.Text = "";
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtSymbolConvention.Text = "";
            txtSymbolConvention.Tag = null;
            txtSymbolConventionShortName.Text = "";
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the SymbolConvention if the grid has any SymbolConvention.
                if (grdSymbolConvention.Rows.Count > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the symbolConvention and symbolConventionID.
                    txtSymbolConvention.Text = grdSymbolConvention.ActiveRow.Cells["SymbolConventionName"].Text.ToString();
                    txtSymbolConvention.Tag = grdSymbolConvention.ActiveRow.Cells["SymbolConventionID"].Text.ToString();
                    txtSymbolConventionShortName.Text = grdSymbolConvention.ActiveRow.Cells["ShortName"].Text.ToString();
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
                //Check for deleting the symbolConvention if the grid has any symbolConvention.
                if (grdSymbolConvention.Rows.Count > 0)
                {
                    string scName = grdSymbolConvention.ActiveRow.Cells["SymbolConventionName"].Text.ToString();
                    if (scName != "")
                    {
                        //Asking the user to be sure about deleting the symbolConvention.
                        if (MessageBox.Show(this, "Do you want to delete this SymbolConvention?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the symbolConventionid from the currently selected row in the grid.
                            int symbolConventionID = int.Parse(grdSymbolConvention.ActiveRow.Cells["SymbolConventionID"].Text.ToString());

                            bool chkVarraible = SymbolManager.DeleteSymbolConvention(symbolConventionID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "SymbolConventionID is referenced in AUEC/CounterPartyVenue.\n Please remove references first to delete it.", "Prana Alert");
                            }
                            else
                            {
                                BindSymbolConventionGrid();
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
