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
    /// Summary description for HandlingInstruction.
    /// </summary>
    public class HandlingInstruction : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "HandlingInstruction : ";
        private const int GRD_HANDLINGINSTRUCTION_ID = 0;
        private const int GRD_HANDLINGINSTRUCTION = 1;
        private const int GRD_HANDLINGINSTRUCTION_VALUETAG = 2;

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtHandlingInstructionTagValue;
        private System.Windows.Forms.TextBox txtHandlingInstruction;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblHandlingInstructionTagValue;
        private System.Windows.Forms.Label lblHandlingInstruction;
        private System.Windows.Forms.GroupBox grpHandlingInstruction;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdHandlingInstructions;
        private Label label1;
        private IContainer components;

        public HandlingInstruction()
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
                if (txtHandlingInstruction != null)
                {
                    txtHandlingInstruction.Dispose();
                }
                if (txtHandlingInstructionTagValue != null)
                {
                    txtHandlingInstructionTagValue.Dispose();
                }
                if (lblHandlingInstruction != null)
                {
                    lblHandlingInstruction.Dispose();
                }
                if (lblHandlingInstructionTagValue != null)
                {
                    lblHandlingInstructionTagValue.Dispose();
                }
                if (grpHandlingInstruction != null)
                {
                    grpHandlingInstruction.Dispose();
                }
                if (grdHandlingInstructions != null)
                {
                    grdHandlingInstructions.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HandlingInstruction));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HandlingInstructionID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderHandlingInstruction", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HandlingInstructionsTagValue", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 4);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TagValue", 5);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpHandlingInstruction = new System.Windows.Forms.GroupBox();
            this.txtHandlingInstructionTagValue = new System.Windows.Forms.TextBox();
            this.lblHandlingInstructionTagValue = new System.Windows.Forms.Label();
            this.txtHandlingInstruction = new System.Windows.Forms.TextBox();
            this.lblHandlingInstruction = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grdHandlingInstructions = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.grpHandlingInstruction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHandlingInstructions)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnReset.Location = new System.Drawing.Point(135, 230);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 36;
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
            this.btnEdit.Location = new System.Drawing.Point(96, 138);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 35;
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
            this.btnDelete.Location = new System.Drawing.Point(174, 138);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 34;
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
            this.btnClose.Location = new System.Drawing.Point(213, 230);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 33;
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
            this.btnSave.Location = new System.Drawing.Point(57, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 32;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpHandlingInstruction
            // 
            this.grpHandlingInstruction.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.grpHandlingInstruction.Controls.Add(this.label1);
            this.grpHandlingInstruction.Controls.Add(this.txtHandlingInstructionTagValue);
            this.grpHandlingInstruction.Controls.Add(this.lblHandlingInstructionTagValue);
            this.grpHandlingInstruction.Controls.Add(this.txtHandlingInstruction);
            this.grpHandlingInstruction.Controls.Add(this.lblHandlingInstruction);
            this.grpHandlingInstruction.Controls.Add(this.label3);
            this.grpHandlingInstruction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpHandlingInstruction.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpHandlingInstruction.Location = new System.Drawing.Point(42, 164);
            this.grpHandlingInstruction.Name = "grpHandlingInstruction";
            this.grpHandlingInstruction.Size = new System.Drawing.Size(261, 66);
            this.grpHandlingInstruction.TabIndex = 30;
            this.grpHandlingInstruction.TabStop = false;
            this.grpHandlingInstruction.Text = "Add/Update Handling Instruction";
            // 
            // txtHandlingInstructionTagValue
            // 
            this.txtHandlingInstructionTagValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHandlingInstructionTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtHandlingInstructionTagValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtHandlingInstructionTagValue.Location = new System.Drawing.Point(78, 42);
            this.txtHandlingInstructionTagValue.MaxLength = 50;
            this.txtHandlingInstructionTagValue.Name = "txtHandlingInstructionTagValue";
            this.txtHandlingInstructionTagValue.Size = new System.Drawing.Size(164, 21);
            this.txtHandlingInstructionTagValue.TabIndex = 9;
            this.txtHandlingInstructionTagValue.LostFocus += new System.EventHandler(this.txtHandlingInstructionTagValue_LostFocus);
            this.txtHandlingInstructionTagValue.GotFocus += new System.EventHandler(this.txtHandlingInstructionTagValue_GotFocus);
            // 
            // lblHandlingInstructionTagValue
            // 
            this.lblHandlingInstructionTagValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblHandlingInstructionTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHandlingInstructionTagValue.Location = new System.Drawing.Point(16, 44);
            this.lblHandlingInstructionTagValue.Name = "lblHandlingInstructionTagValue";
            this.lblHandlingInstructionTagValue.Size = new System.Drawing.Size(58, 15);
            this.lblHandlingInstructionTagValue.TabIndex = 8;
            this.lblHandlingInstructionTagValue.Text = "TagValue";
            // 
            // txtHandlingInstruction
            // 
            this.txtHandlingInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHandlingInstruction.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtHandlingInstruction.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtHandlingInstruction.Location = new System.Drawing.Point(78, 20);
            this.txtHandlingInstruction.MaxLength = 50;
            this.txtHandlingInstruction.Name = "txtHandlingInstruction";
            this.txtHandlingInstruction.Size = new System.Drawing.Size(164, 21);
            this.txtHandlingInstruction.TabIndex = 2;
            this.txtHandlingInstruction.LostFocus += new System.EventHandler(this.txtHandlingInstruction_LostFocus);
            this.txtHandlingInstruction.GotFocus += new System.EventHandler(this.txtHandlingInstruction_GotFocus);
            // 
            // lblHandlingInstruction
            // 
            this.lblHandlingInstruction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblHandlingInstruction.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHandlingInstruction.Location = new System.Drawing.Point(14, 22);
            this.lblHandlingInstruction.Name = "lblHandlingInstruction";
            this.lblHandlingInstruction.Size = new System.Drawing.Size(37, 15);
            this.lblHandlingInstruction.TabIndex = 0;
            this.lblHandlingInstruction.Text = "Name";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(50, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 8);
            this.label3.TabIndex = 7;
            this.label3.Text = "*";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grdHandlingInstructions
            // 
            this.grdHandlingInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdHandlingInstructions.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 195;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn2.Width = 361;
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.Width = 198;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 76;
            appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn5.CellAppearance = appearance5;
            appearance6.FontData.BoldAsString = "True";
            appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn5.Header.Appearance = appearance6;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Width = 157;
            appearance7.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn6.CellAppearance = appearance7;
            appearance8.FontData.BoldAsString = "True";
            appearance8.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn6.Header.Appearance = appearance8;
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Width = 154;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdHandlingInstructions.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdHandlingInstructions.DisplayLayout.GroupByBox.Hidden = true;
            this.grdHandlingInstructions.DisplayLayout.MaxColScrollRegions = 1;
            this.grdHandlingInstructions.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdHandlingInstructions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdHandlingInstructions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdHandlingInstructions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdHandlingInstructions.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdHandlingInstructions.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdHandlingInstructions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdHandlingInstructions.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdHandlingInstructions.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdHandlingInstructions.Location = new System.Drawing.Point(6, 1);
            this.grdHandlingInstructions.Name = "grdHandlingInstructions";
            this.grdHandlingInstructions.Size = new System.Drawing.Size(332, 136);
            this.grdHandlingInstructions.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(66, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 10);
            this.label1.TabIndex = 10;
            this.label1.Text = "*";
            // 
            // HandlingInstruction
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(344, 255);
            this.Controls.Add(this.grdHandlingInstructions);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpHandlingInstruction);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "HandlingInstruction";
            this.Text = "HandlingInstruction";
            this.Load += new System.EventHandler(this.HandlingInstruction_Load);
            this.grpHandlingInstruction.ResumeLayout(false);
            this.grpHandlingInstruction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHandlingInstructions)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtHandlingInstructionTagValue_GotFocus(object sender, System.EventArgs e)
        {
            txtHandlingInstructionTagValue.BackColor = Color.LemonChiffon;
        }
        private void txtHandlingInstructionTagValue_LostFocus(object sender, System.EventArgs e)
        {
            txtHandlingInstructionTagValue.BackColor = Color.White;
        }
        private void txtHandlingInstruction_GotFocus(object sender, System.EventArgs e)
        {
            txtHandlingInstruction.BackColor = Color.LemonChiffon;
        }
        private void txtHandlingInstruction_LostFocus(object sender, System.EventArgs e)
        {
            txtHandlingInstruction.BackColor = Color.White;
        }


        #endregion
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void HandlingInstruction_Load(object sender, System.EventArgs e)
        {
            BindHandlingInstructionsGrid();
        }

        private void BindHandlingInstructionsGrid()
        {
            //this.dataGridTableStyle1.MappingName = "handlingInstructions";

            //Fetching the existing handlingInstructions from the database and binding it to the grid.
            HandlingInstructions handlingInstructions = OrderManager.GetHandlingInstructions();
            //Assigning the grid's datasource to the handlingInstructions object.
            if (handlingInstructions.Count > 0)
            {
                grdHandlingInstructions.DataSource = handlingInstructions;
            }
            else
            {
                HandlingInstructions nullHandlingInstructions = new HandlingInstructions();
                nullHandlingInstructions.Add(new Prana.Admin.BLL.HandlingInstruction(int.MinValue, "", ""));
                grdHandlingInstructions.DataSource = nullHandlingInstructions;
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currenly saved id of the HandlingInstruction in the database.
                int checkID = SaveHandlingInstruction();
                //Binding grid after saving the a HandlingInstruction.
                BindHandlingInstructionsGrid();
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

        //This method blanks the textboxes in the HandlingInstruction form.
        private void RefreshForm()
        {
            txtHandlingInstruction.Text = "";
            txtHandlingInstructionTagValue.Text = "";
        }

        /// <summary>
        /// This method saves the <see cref="HandlingInstruction"/> to the database.
        /// </summary>
        /// <returns>HandlingInstructionID, saved to the database</returns>
        private int SaveHandlingInstruction()
        {
            errorProvider1.SetError(txtHandlingInstruction, "");
            errorProvider1.SetError(txtHandlingInstructionTagValue, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.HandlingInstruction handlingInstruction = new Prana.Admin.BLL.HandlingInstruction();

            if (txtHandlingInstruction.Tag != null)
            {
                //Update
                handlingInstruction.HandlingInstructionID = int.Parse(txtHandlingInstruction.Tag.ToString());
            }
            //Validation to check for the empty value in HandlingInstruction textbox
            if (txtHandlingInstruction.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtHandlingInstruction, "Provide Value!");
                return result;
            }
            if (txtHandlingInstructionTagValue.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtHandlingInstructionTagValue, "Provide Value!");
                return result;
            }
            else
            {
                handlingInstruction.Name = txtHandlingInstruction.Text.Trim();
                errorProvider1.SetError(txtHandlingInstruction, "");
            }
            handlingInstruction.TagValue = txtHandlingInstructionTagValue.Text;

            //Saving the handlingInstruction data and retrieving the handlingInstructionid for the newly added handlingInstruction.
            int newHandlingInstructionID = OrderManager.SaveHandlingInstruction(handlingInstruction);
            //Showing the message: HandlingInstruction already existing by checking the handlingInstruction id value to -1 
            if (newHandlingInstructionID == -1)
            {
                MessageBox.Show("Handling Instructions with the same name or tag already exists.", "Prana Alert", MessageBoxButtons.OK);
            }
            //Showing the message : HandlingInstruction data saved
            else
            {
                //errorProvider1.SetError(txtHandlingInstruction, "HandlingInstruction Saved");
                txtHandlingInstruction.Tag = null;
            }
            result = newHandlingInstructionID;
            //Returning the newly added handlingInstruction id.
            return result;
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtHandlingInstruction.Text = "";
            txtHandlingInstruction.Tag = null;
            txtHandlingInstructionTagValue.Text = "";
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the Handling Instruction if the grid has any Handling Instruction.
                if (grdHandlingInstructions.Rows.Count > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the handlingInstruction and handlingInstructionID.
                    txtHandlingInstruction.Text = grdHandlingInstructions.ActiveRow.Cells["Name"].Text.ToString();
                    txtHandlingInstruction.Tag = grdHandlingInstructions.ActiveRow.Cells["HandlingInstructionID"].Text.ToString();
                    txtHandlingInstructionTagValue.Text = grdHandlingInstructions.ActiveRow.Cells["TagValue"].Text.ToString();
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
                //Check for deleting the handlingInstruction if the grid has any executionInstruction.
                if (grdHandlingInstructions.Rows.Count > 0)
                {
                    string hiName = grdHandlingInstructions.ActiveRow.Cells["Name"].Text.ToString();
                    if (hiName != "")
                    {
                        //Asking the user to be sure about deleting the handlingInstruction.
                        if (MessageBox.Show(this, "Do you want to delete this HandlingInstruction?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the handlingInstructionid from the currently selected row in the grid.
                            int handlingInstructionID = int.Parse(grdHandlingInstructions.ActiveRow.Cells["HandlingInstructionID"].Value.ToString());

                            bool chkVarraible = OrderManager.DeleteHandlingInstruction(handlingInstructionID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "HandlingInstruction is referenced in CounterpartyVenue.\n Please remove references first to delete it.", "Prana Alert");
                            }
                            else
                            {
                                BindHandlingInstructionsGrid();
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

        private void grdHandlingInstructions1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }
    }
}
