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
    /// Summary description for Side.
    /// </summary>
    public class Side : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "Side : ";
        private const int GRD_SIDE_ID = 0;
        private const int GRD_SIDE = 1;
        private const int GRD_SIDE_ORDERVALUE = 2;

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSide;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblSideTagValue;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdSide;
        private System.Windows.Forms.TextBox txtSide;
        private System.Windows.Forms.TextBox txtSideTagValue;
        private Label label1;
        private IContainer components;

        //private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;

        public Side()
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
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (lblSide != null)
                {
                    lblSide.Dispose();
                }
                if (lblSideTagValue != null)
                {
                    lblSideTagValue.Dispose();
                }
                if (grdSide != null)
                {
                    grdSide.Dispose();
                }
                if (txtSide != null)
                {
                    txtSide.Dispose();
                }
                if (txtSideTagValue != null)
                {
                    txtSideTagValue.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Side));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SideID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderSide", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SideTagValue", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUECID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 5);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TagValue", 6);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSide = new System.Windows.Forms.TextBox();
            this.lblSideTagValue = new System.Windows.Forms.Label();
            this.txtSideTagValue = new System.Windows.Forms.TextBox();
            this.lblSide = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grdSide = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSide)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnReset.Location = new System.Drawing.Point(135, 230);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 15;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(96, 138);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 14;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(174, 138);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 13;
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
            this.btnClose.Location = new System.Drawing.Point(213, 230);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(57, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 11;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSide);
            this.groupBox1.Controls.Add(this.lblSideTagValue);
            this.groupBox1.Controls.Add(this.txtSideTagValue);
            this.groupBox1.Controls.Add(this.lblSide);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(52, 164);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 64);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Update Side";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(62, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 10);
            this.label1.TabIndex = 10;
            this.label1.Text = "*";
            // 
            // txtSide
            // 
            this.txtSide.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSide.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSide.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSide.Location = new System.Drawing.Point(74, 18);
            this.txtSide.MaxLength = 50;
            this.txtSide.Name = "txtSide";
            this.txtSide.Size = new System.Drawing.Size(150, 21);
            this.txtSide.TabIndex = 9;
            this.txtSide.LostFocus += new System.EventHandler(this.txtSide_LostFocus);
            this.txtSide.GotFocus += new System.EventHandler(this.txtSide_GotFocus);
            // 
            // lblSideTagValue
            // 
            this.lblSideTagValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSideTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSideTagValue.Location = new System.Drawing.Point(16, 42);
            this.lblSideTagValue.Name = "lblSideTagValue";
            this.lblSideTagValue.Size = new System.Drawing.Size(52, 15);
            this.lblSideTagValue.TabIndex = 8;
            this.lblSideTagValue.Text = "TagValue";
            // 
            // txtSideTagValue
            // 
            this.txtSideTagValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSideTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSideTagValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSideTagValue.Location = new System.Drawing.Point(74, 40);
            this.txtSideTagValue.MaxLength = 50;
            this.txtSideTagValue.Name = "txtSideTagValue";
            this.txtSideTagValue.Size = new System.Drawing.Size(150, 21);
            this.txtSideTagValue.TabIndex = 2;
            this.txtSideTagValue.LostFocus += new System.EventHandler(this.txtSideTagValue_LostFocus);
            this.txtSideTagValue.GotFocus += new System.EventHandler(this.txtSideTagValue_GotFocus);
            // 
            // lblSide
            // 
            this.lblSide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSide.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSide.Location = new System.Drawing.Point(16, 21);
            this.lblSide.Name = "lblSide";
            this.lblSide.Size = new System.Drawing.Size(36, 15);
            this.lblSide.TabIndex = 0;
            this.lblSide.Text = "Name";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(52, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 8);
            this.label3.TabIndex = 7;
            this.label3.Text = "*";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grdSide
            // 
            this.grdSide.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSide.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn2.Width = 134;
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.Width = 251;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn6.CellAppearance = appearance5;
            appearance6.FontData.BoldAsString = "True";
            appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn6.Header.Appearance = appearance6;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Width = 152;
            appearance7.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn7.CellAppearance = appearance7;
            appearance8.FontData.BoldAsString = "True";
            appearance8.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn7.Header.Appearance = appearance8;
            ultraGridColumn7.Header.VisiblePosition = 6;
            ultraGridColumn7.Width = 153;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdSide.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdSide.DisplayLayout.GroupByBox.Hidden = true;
            this.grdSide.DisplayLayout.MaxColScrollRegions = 1;
            this.grdSide.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdSide.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdSide.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdSide.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdSide.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdSide.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdSide.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdSide.UseFlatMode = DefaultableBoolean.True;
            this.grdSide.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdSide.Location = new System.Drawing.Point(9, 0);
            this.grdSide.Name = "grdSide";
            this.grdSide.Size = new System.Drawing.Size(326, 136);
            this.grdSide.TabIndex = 16;
            // 
            // Side
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(344, 255);
            this.Controls.Add(this.grdSide);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "Side";
            this.Text = "Side";
            this.Load += new System.EventHandler(this.Side_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdSide)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private void BindSideGrid()
        {
            //this.dataGridTableStyle1.MappingName = "sides";

            //Fetching the existing assets from the database and binding it to the grid.
            Sides sides = OrderManager.GetSides();
            //Assigning the grid's datasource to the assets object.
            if (sides.Count > 0)
            {
                grdSide.DataSource = sides;
                //grdSide.Rows.se .Rows[0].Selected = true;
            }
            else
            {
                Sides nullSides = new Sides();
                nullSides.Add(new Prana.Admin.BLL.Side(int.MinValue, "", ""));
                grdSide.DataSource = nullSides;
            }

        }

        private void Side_Load(object sender, System.EventArgs e)
        {
            BindSideGrid();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        //This method blanks the textboxes in the side form.
        private void RefreshForm()
        {
            txtSide.Text = "";
            txtSideTagValue.Text = "";
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currenly saved id of the side in the database.
                int checkID = SaveSide();
                //Binding grid after saving the a side.
                BindSideGrid();
                if (checkID >= 0)
                {
                    //Calling refresh form after saving a side  after checking that whether the side
                    //is saved successfully or not by checking for the positive value of checkID.
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
        /// This method saves the <see cref="Side"/> to the database.
        /// </summary>
        /// <returns>SideID, saved to the database</returns>
        private int SaveSide()
        {
            errorProvider1.SetError(txtSide, "");
            errorProvider1.SetError(txtSideTagValue, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.Side side = new Prana.Admin.BLL.Side();

            if (txtSide.Tag != null)
            {
                //Update
                side.SideID = int.Parse(txtSide.Tag.ToString());
            }
            //Validation to check for the empty value in Side textbox
            if (txtSide.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtSide, "Provide Value!");
                return result;
            }
            if (txtSideTagValue.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtSideTagValue, "Provide Value!");
                return result;
            }
            else
            {
                side.Name = txtSide.Text.Trim();
                errorProvider1.SetError(txtSide, "");
            }
            side.TagValue = txtSideTagValue.Text;

            //Saving the side data and retrieving the sideid for the newly added side.
            int newSideID = OrderManager.SaveSide(side);
            //Showing the message: side already existing by checking the side id value to -1 
            if (newSideID == -1)
            {
                MessageBox.Show("Side with the same name or tag already exists.", "Prana Alert", MessageBoxButtons.OK);
            }
            //Showing the message : Side data saved
            else
            {
                //errorProvider1.SetError(txtSide, "Side Saved");
                txtSide.Tag = null;
            }
            result = newSideID;
            //Returning the newly added side id.
            return result;
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the side if the grid has any side.
                if (grdSide.Rows.Count > 0)
                {
                    if (grdSide.ActiveRow != null)
                    {
                        //Edit: Edit.
                        //Showing the values of the currently selected row to the textboxes in the form by 
                        //the column positions reltative to the side and sideID.
                        txtSide.Text = grdSide.ActiveRow.Cells["Name"].Text.ToString();
                        txtSide.Tag = grdSide.ActiveRow.Cells["SideID"].Value.ToString();
                        txtSideTagValue.Text = grdSide.ActiveRow.Cells["TagValue"].Text.ToString();
                    }
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
                errorProvider1.SetError(btnDelete, "");
                if (grdSide.Rows.Count > 0)
                {
                    string sideName = grdSide.ActiveRow.Cells["Name"].Text.ToString();
                    if (sideName != "")
                    {
                        //Check for deleting the side if the grid has any side.
                        //if(grdSide.VisibleRowCount > 0)
                        //				{
                        //Asking the user to be sure about deleting the side.
                        if (MessageBox.Show(this, "Do you want to delete this Side?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the sideid from the currently selected row in the grid.
                            int sideID = int.Parse(grdSide.ActiveRow.Cells["SideID"].Value.ToString());

                            bool chkVarraible = OrderManager.DeleteSide(sideID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "Side is referenced in AUEC/CounterpartyVenue/Compliance.\n Please remove references first to delete it.", "Prana Alert");
                            }
                            else
                            {
                                BindSideGrid();
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
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, FORM_NAME);
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

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtSide.Text = "";
            txtSide.Tag = null;
            txtSideTagValue.Text = "";
        }

        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void grdSide_Navigate(object sender, System.Windows.Forms.NavigateEventArgs ne)
        {

        }

        #region Focus Colors
        private void txtSide_GotFocus(object sender, System.EventArgs e)
        {
            txtSide.BackColor = Color.LemonChiffon;
        }
        private void txtSide_LostFocus(object sender, System.EventArgs e)
        {
            txtSide.BackColor = Color.White;
        }
        private void txtSideTagValue_GotFocus(object sender, System.EventArgs e)
        {
            txtSideTagValue.BackColor = Color.LemonChiffon;
        }
        private void txtSideTagValue_LostFocus(object sender, System.EventArgs e)
        {
            txtSideTagValue.BackColor = Color.White;
        }

        #endregion
    }
}
