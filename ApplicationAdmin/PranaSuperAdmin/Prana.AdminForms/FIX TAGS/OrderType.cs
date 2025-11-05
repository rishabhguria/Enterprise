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
    /// Summary description for OrderType.
    /// </summary>
    public class OrderType : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "OrderType : ";
        private const int GRD_ORDERTYPE_ID = 0;
        private const int GRD_ORDERTYPE = 1;
        private const int GRD_ORDER_VALUETAG = 2;

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOrderType;
        private System.Windows.Forms.TextBox txtOrderTypeTagValue;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblOrderTypeTagValue;
        private System.Windows.Forms.Label lblOrderType;
        private System.Windows.Forms.GroupBox grpside;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdOrderTypes;
        private Label label1;
        private IContainer components;

        public OrderType()
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
                if (txtOrderType != null)
                {
                    txtOrderType.Dispose();
                }
                if (txtOrderTypeTagValue != null)
                {
                    txtOrderTypeTagValue.Dispose();
                }
                if (lblOrderType != null)
                {
                    lblOrderType.Dispose();
                }
                if (lblOrderTypeTagValue != null)
                {
                    lblOrderTypeTagValue.Dispose();
                }
                if (grpside != null)
                {
                    grpside.Dispose();
                }
                if (grdOrderTypes != null)
                {
                    grdOrderTypes.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderType));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderTypesID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderTypesTagValue", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUECID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TagValue", 5);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpside = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOrderTypeTagValue = new System.Windows.Forms.TextBox();
            this.lblOrderTypeTagValue = new System.Windows.Forms.Label();
            this.txtOrderType = new System.Windows.Forms.TextBox();
            this.lblOrderType = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grdOrderTypes = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpside.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOrderTypes)).BeginInit();
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
            this.btnReset.TabIndex = 22;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnEdit.Location = new System.Drawing.Point(91, 142);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 21;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(169, 142);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 20;
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
            this.btnClose.Location = new System.Drawing.Point(212, 230);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 19;
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
            this.btnSave.Location = new System.Drawing.Point(58, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 18;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpside
            // 
            this.grpside.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.grpside.Controls.Add(this.label1);
            this.grpside.Controls.Add(this.txtOrderTypeTagValue);
            this.grpside.Controls.Add(this.lblOrderTypeTagValue);
            this.grpside.Controls.Add(this.txtOrderType);
            this.grpside.Controls.Add(this.lblOrderType);
            this.grpside.Controls.Add(this.label3);
            this.grpside.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpside.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpside.Location = new System.Drawing.Point(44, 168);
            this.grpside.Name = "grpside";
            this.grpside.Size = new System.Drawing.Size(256, 62);
            this.grpside.TabIndex = 16;
            this.grpside.TabStop = false;
            this.grpside.Text = "Add/Update OrderType";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(64, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 10);
            this.label1.TabIndex = 10;
            this.label1.Text = "*";
            // 
            // txtOrderTypeTagValue
            // 
            this.txtOrderTypeTagValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrderTypeTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtOrderTypeTagValue.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOrderTypeTagValue.Location = new System.Drawing.Point(76, 38);
            this.txtOrderTypeTagValue.MaxLength = 50;
            this.txtOrderTypeTagValue.Name = "txtOrderTypeTagValue";
            this.txtOrderTypeTagValue.Size = new System.Drawing.Size(164, 21);
            this.txtOrderTypeTagValue.TabIndex = 9;
            this.txtOrderTypeTagValue.LostFocus += new System.EventHandler(this.txtOrderTypeTagValue_LostFocus);
            this.txtOrderTypeTagValue.GotFocus += new System.EventHandler(this.txtOrderTypeTagValue_GotFocus);
            // 
            // lblOrderTypeTagValue
            // 
            this.lblOrderTypeTagValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblOrderTypeTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblOrderTypeTagValue.Location = new System.Drawing.Point(16, 40);
            this.lblOrderTypeTagValue.Name = "lblOrderTypeTagValue";
            this.lblOrderTypeTagValue.Size = new System.Drawing.Size(54, 15);
            this.lblOrderTypeTagValue.TabIndex = 8;
            this.lblOrderTypeTagValue.Text = "TagValue";
            // 
            // txtOrderType
            // 
            this.txtOrderType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrderType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtOrderType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtOrderType.Location = new System.Drawing.Point(76, 16);
            this.txtOrderType.MaxLength = 50;
            this.txtOrderType.Name = "txtOrderType";
            this.txtOrderType.Size = new System.Drawing.Size(164, 21);
            this.txtOrderType.TabIndex = 2;
            this.txtOrderType.LostFocus += new System.EventHandler(this.txtOrderType_LostFocus);
            this.txtOrderType.GotFocus += new System.EventHandler(this.txtOrderType_GotFocus);
            // 
            // lblOrderType
            // 
            this.lblOrderType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblOrderType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblOrderType.Location = new System.Drawing.Point(14, 19);
            this.lblOrderType.Name = "lblOrderType";
            this.lblOrderType.Size = new System.Drawing.Size(33, 15);
            this.lblOrderType.TabIndex = 0;
            this.lblOrderType.Text = "Type";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(46, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 8);
            this.label3.TabIndex = 7;
            this.label3.Text = "*";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grdOrderTypes
            // 
            this.grdOrderTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdOrderTypes.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 149;
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn3.Width = 135;
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
            ultraGridColumn6.Width = 164;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdOrderTypes.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdOrderTypes.DisplayLayout.GroupByBox.Hidden = true;
            this.grdOrderTypes.DisplayLayout.MaxColScrollRegions = 1;
            this.grdOrderTypes.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdOrderTypes.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdOrderTypes.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdOrderTypes.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdOrderTypes.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdOrderTypes.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdOrderTypes.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdOrderTypes.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdOrderTypes.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdOrderTypes.Location = new System.Drawing.Point(5, 1);
            this.grdOrderTypes.Name = "grdOrderTypes";
            this.grdOrderTypes.Size = new System.Drawing.Size(334, 139);
            this.grdOrderTypes.TabIndex = 24;
            this.grdOrderTypes.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdOrderTypes1_InitializeLayout);
            // 
            // OrderType
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(344, 255);
            this.Controls.Add(this.grdOrderTypes);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpside);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "OrderType";
            this.Text = "OrderType";
            this.Load += new System.EventHandler(this.OrderType_Load);
            this.grpside.ResumeLayout(false);
            this.grpside.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdOrderTypes)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtOrderType_GotFocus(object sender, System.EventArgs e)
        {
            txtOrderType.BackColor = Color.LemonChiffon;
        }
        private void txtOrderType_LostFocus(object sender, System.EventArgs e)
        {
            txtOrderType.BackColor = Color.White;
        }
        private void txtOrderTypeTagValue_GotFocus(object sender, System.EventArgs e)
        {
            txtOrderTypeTagValue.BackColor = Color.LemonChiffon;
        }
        private void txtOrderTypeTagValue_LostFocus(object sender, System.EventArgs e)
        {
            txtOrderTypeTagValue.BackColor = Color.White;
        }

        #endregion

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void OrderType_Load(object sender, System.EventArgs e)
        {
            BindOrderTypesGrid();
        }

        private void BindOrderTypesGrid()
        {
            //this.dataGridTableStyle1.MappingName = "orderTypes";

            //Fetching the existing orderTypes from the database and binding it to the grid.
            OrderTypes orderTypes = OrderManager.GetOrderTypes();
            //Assigning the grid's datasource to the orderTypes object.
            if (orderTypes.Count > 0)
            {
                grdOrderTypes.DataSource = orderTypes;
            }
            else
            {
                OrderTypes nullOrderTypes = new OrderTypes();
                nullOrderTypes.Add(new Prana.Admin.BLL.OrderType(int.MinValue, "", ""));
                grdOrderTypes.DataSource = nullOrderTypes;
            }
        }

        //This method blanks the textboxes in the side form.
        private void RefreshForm()
        {
            txtOrderType.Text = "";
            txtOrderTypeTagValue.Text = "";
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currenly saved id of the OrderType in the database.
                int checkID = SaveOrderType();
                //Binding grid after saving the a side.
                BindOrderTypesGrid();
                if (checkID >= 0)
                {
                    //Calling refresh form after saving a OrderType after checking that whether the OrderType
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
        /// This method saves the <see cref="OrderType"/> to the database.
        /// </summary>
        /// <returns>OrderID, saved to the database</returns>
        private int SaveOrderType()
        {
            errorProvider1.SetError(txtOrderType, "");
            errorProvider1.SetError(txtOrderTypeTagValue, "");
            errorProvider1.SetError(btnSave, "");

            int result = int.MinValue;

            Prana.Admin.BLL.OrderType orderType = new Prana.Admin.BLL.OrderType();

            if (txtOrderType.Tag != null)
            {
                //Update
                orderType.OrderTypesID = int.Parse(txtOrderType.Tag.ToString());
            }
            //Validation to check for the empty value in OrderType textbox
            if (txtOrderType.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtOrderType, "Provide Value!");
                return result;
            }
            if (txtOrderTypeTagValue.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtOrderTypeTagValue, "Provide Value!");
                return result;
            }
            else
            {
                orderType.Type = txtOrderType.Text.Trim();
                errorProvider1.SetError(txtOrderType, "");
            }
            orderType.TagValue = txtOrderTypeTagValue.Text;

            //Saving the side data and retrieving the sideid for the newly added orderType.
            int newOrderTypeID = OrderManager.SaveOrderType(orderType);
            //Showing the message: side already existing by checking the side id value to -1 
            if (newOrderTypeID == -1)
            {
                MessageBox.Show("Order Type with the same name or tag already exists.", "Prana Alert", MessageBoxButtons.OK);
            }
            //Showing the message : OrderType data saved
            else
            {
                txtOrderType.Tag = null;
            }
            result = newOrderTypeID;
            //Returning the newly added orderType id.
            return result;
        }

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtOrderType.Text = "";
            txtOrderType.Tag = null;
            txtOrderTypeTagValue.Text = "";
        }

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            try
            {
                errorProvider1.SetError(btnEdit, "");
                //Check for editing the order if the grid has any side.
                if (grdOrderTypes.Rows.Count > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the order and orderID.
                    txtOrderType.Text = grdOrderTypes.ActiveRow.Cells["Type"].Text.ToString();
                    txtOrderType.Tag = grdOrderTypes.ActiveRow.Cells["OrderTypesID"].Text.ToString();
                    txtOrderTypeTagValue.Text = grdOrderTypes.ActiveRow.Cells["TagValue"].Text.ToString();
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
                //Check for deleting the order if the grid has any orderType.
                if (grdOrderTypes.Rows.Count > 0)
                {
                    string otName = grdOrderTypes.ActiveRow.Cells["Type"].Text.ToString();
                    if (otName != "")
                    {
                        //Asking the user to be sure about deleting the orderType.
                        if (MessageBox.Show(this, "Do you want to delete this OrderType?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the orderTypeid from the currently selected row in the grid.
                            int orderTypeID = int.Parse(grdOrderTypes.ActiveRow.Cells["OrderTypesID"].Value.ToString());
                            bool chkVarraible = OrderManager.DeleteOrderType(orderTypeID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "OrderType is referenced in AUEC/CounterpartyVenue/Compliance.\n Please remove references first to delete it.", "Prana Alert");
                            }
                            else
                            {
                                BindOrderTypesGrid();
                            }
                        }
                    }

                    //Binding the orderType grid.
                    BindOrderTypesGrid();
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

        private void grdOrderTypes1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }
    }
}
