using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for GridClearingFirmsPrimeBrokers.
    /// </summary>
    public class GridClearingFirmsPrimeBrokers : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "GridClearingFirmsPrimeBrokers : ";
        private Infragistics.Win.UltraWinGrid.UltraGrid grdClearingFirmsPrimeBrokers;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private IContainer components;

        public GridClearingFirmsPrimeBrokers()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

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
                if (grdClearingFirmsPrimeBrokers != null)
                {
                    grdClearingFirmsPrimeBrokers.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (btnCreate != null)
                {
                    btnCreate.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (createCompanyClearingFirmsPrimeBrokers != null)
                {
                    createCompanyClearingFirmsPrimeBrokers.Dispose();
                }
                if (createCompanyClearingFirmsPrimeBrokersEdit != null)
                {
                    createCompanyClearingFirmsPrimeBrokersEdit.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClearingFirmsPrimeBrokersID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClearingFirmsPrimeBrokersName", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClearingFirmsPrimeBrokersShortName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridClearingFirmsPrimeBrokers));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grdClearingFirmsPrimeBrokers = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdClearingFirmsPrimeBrokers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.grdClearingFirmsPrimeBrokers);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(0, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 158);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Clearing Firms/Prime Brokers";
            // 
            // grdClearingFirmsPrimeBrokers
            // 
            this.grdClearingFirmsPrimeBrokers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 221;
            ultraGridColumn2.Header.Caption = "Name";
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 110;
            ultraGridColumn3.Header.Caption = "ShortName";
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 163;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 69;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.GroupByBox.Hidden = true;
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.MaxColScrollRegions = 1;
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdClearingFirmsPrimeBrokers.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdClearingFirmsPrimeBrokers.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdClearingFirmsPrimeBrokers.Location = new System.Drawing.Point(5, 22);
            this.grdClearingFirmsPrimeBrokers.Name = "grdClearingFirmsPrimeBrokers";
            this.grdClearingFirmsPrimeBrokers.Size = new System.Drawing.Size(294, 132);
            this.grdClearingFirmsPrimeBrokers.TabIndex = 0;
            this.grdClearingFirmsPrimeBrokers.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnCreate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCreate.BackgroundImage")));
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreate.Location = new System.Drawing.Point(41, 158);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Location = new System.Drawing.Point(197, 158);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Location = new System.Drawing.Point(119, 158);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // GridClearingFirmsPrimeBrokers
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "GridClearingFirmsPrimeBrokers";
            this.Size = new System.Drawing.Size(312, 198);
            this.Load += new System.EventHandler(this.GridClearingFirmsPrimeBrokers_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdClearingFirmsPrimeBrokers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;

        private CreateCompanyClearingFirmsPrimeBrokers createCompanyClearingFirmsPrimeBrokers = null;
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            if (createCompanyClearingFirmsPrimeBrokers == null)
            {
                createCompanyClearingFirmsPrimeBrokers = new CreateCompanyClearingFirmsPrimeBrokers();
            }
            Prana.Admin.BLL.ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();
            createCompanyClearingFirmsPrimeBrokers.CurrentClearingFirmsPrimeBrokers = (ClearingFirmsPrimeBrokers)grdClearingFirmsPrimeBrokers.DataSource;
            foreach (ClearingFirmPrimeBroker clearingFirmPrimeBroker in createCompanyClearingFirmsPrimeBrokers.CurrentClearingFirmsPrimeBrokers)
            {
                if (clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName.ToString() == "")
                {
                    createCompanyClearingFirmsPrimeBrokers.NoData = 1;
                }
                else
                {
                    createCompanyClearingFirmsPrimeBrokers.NoData = 0;
                    break;
                }
            }
            createCompanyClearingFirmsPrimeBrokers.ShowDialog(this.ParentForm);
            grdClearingFirmsPrimeBrokers.Refresh();
            clearingFirmsPrimeBrokers = createCompanyClearingFirmsPrimeBrokers.CurrentClearingFirmsPrimeBrokers;
            //grdClearingFirmsPrimeBrokers.DataSource = createCompanyClearingFirmsPrimeBrokers.CurrentClearingFirmsPrimeBrokers;

            if (clearingFirmsPrimeBrokers.Count > 0)
            {
                grdClearingFirmsPrimeBrokers.DataSource = clearingFirmsPrimeBrokers;
                grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["ClearingFirmsPrimeBrokersID"].Hidden = true;
                grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;
            }
            else
            {
                ClearingFirmPrimeBroker nullClearingFirmPrimeBroker = new ClearingFirmPrimeBroker(int.MinValue, "");
                clearingFirmsPrimeBrokers.Add(nullClearingFirmPrimeBroker);
                grdClearingFirmsPrimeBrokers.DataSource = clearingFirmsPrimeBrokers;
                RefreshGrid();
            }

            //if(createCompanyClearingFirmsPrimeBrokers.CurrentClearingFirmsPrimeBrokers.Count > 0 )
            //{
            //    grdClearingFirmsPrimeBrokers.Rows[0].Selected = true;
            //    _nullRow = false;
            //}
            foreach (ClearingFirmPrimeBroker clearingFirmPrimeBroker in createCompanyClearingFirmsPrimeBrokers.CurrentClearingFirmsPrimeBrokers)
            {
                if (clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName.ToString() != "")
                {
                    _nullRow = false;
                    break;
                }
            }
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["ClearingFirmsPrimeBrokersID"].Hidden = true;
            grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;
            grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["ClearingFirmsPrimeBrokersName"].Header.VisiblePosition = 0;
            grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["ClearingFirmsPrimeBrokersShortName"].Header.VisiblePosition = 1;

            grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["ClearingFirmsPrimeBrokersName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;
            grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["ClearingFirmsPrimeBrokersShortName"].Header.Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.False;

            grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["ClearingFirmsPrimeBrokersName"].Header.Caption = "Name";
            grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["ClearingFirmsPrimeBrokersShortName"].Header.Caption = "ShortName";
        }
        public Prana.Admin.BLL.ClearingFirmsPrimeBrokers CurrentClearingFirmsPrimeBrokers
        {
            get
            {
                foreach (ClearingFirmPrimeBroker clearingFirmPrimeBroker in (ClearingFirmsPrimeBrokers)grdClearingFirmsPrimeBrokers.DataSource)
                {
                    if (clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName.ToString() == "")
                    {
                        _nullRow = true;
                        //						ClearingFirmsPrimeBrokers nullClearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();
                        //						grdClearingFirmsPrimeBrokers.DataSource = nullClearingFirmsPrimeBrokers;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                //return (Prana.Admin.BLL.ClearingFirmsPrimeBrokers) grdClearingFirmsPrimeBrokers.DataSource; 
                ClearingFirmsPrimeBrokers currentClearingFirmsPrimeBrokers = (Prana.Admin.BLL.ClearingFirmsPrimeBrokers)grdClearingFirmsPrimeBrokers.DataSource;
                if (currentClearingFirmsPrimeBrokers.Count > 0)
                {
                    ClearingFirmsPrimeBrokers saveClearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();
                    ClearingFirmsPrimeBrokers companyClearingFirmsPrimeBrokers = (Prana.Admin.BLL.ClearingFirmsPrimeBrokers)grdClearingFirmsPrimeBrokers.DataSource;
                    foreach (ClearingFirmPrimeBroker companyClearingFirmsPrimeBroker in companyClearingFirmsPrimeBrokers)
                    {
                        if (companyClearingFirmsPrimeBroker.ClearingFirmsPrimeBrokersName != "")
                        {
                            saveClearingFirmsPrimeBrokers.Add(companyClearingFirmsPrimeBroker);
                        }
                    }

                    return saveClearingFirmsPrimeBrokers;
                    //return (Prana.Admin.BLL.ClientAccounts) grdClientAccounts.DataSource; 
                }
                else
                {
                    currentClearingFirmsPrimeBrokers = null;
                    return currentClearingFirmsPrimeBrokers;
                }
            }
        }

        private int _companyID = int.MinValue;
        public int CompanyID
        {
            get { return _companyID; }
            //			set
            //			{
            //				_companyID = value;
            //				BindDataGrid();
            //			}
        }

        private int _clearingFirmsPrimeBrokersID = int.MinValue;
        public int ClearingFirmsPrimeBrokersID
        {
            get { return _clearingFirmsPrimeBrokersID; }
            //			set
            //			{
            //				_clearingFirmsPrimeBrokersID = value;
            //				BindDataGrid();
            //			}
        }

        public ClearingFirmPrimeBroker ClearingFirmPrimeBrokerProperty
        {
            get
            {
                ClearingFirmPrimeBroker clearingFirmPrimeBroker = new ClearingFirmPrimeBroker();
                GetClearingFirmPrimeBroker(clearingFirmPrimeBroker);
                return clearingFirmPrimeBroker;
            }
            //			set 
            //			{
            //				SetClearingFirmPrimeBroker(value);
            //					
            //			}
        }

        public void SetupControl(int companyID)
        {
            _companyID = companyID;
            BindDataGrid();
        }

        public void GetClearingFirmPrimeBroker(ClearingFirmPrimeBroker clearingFirmPrimeBroker)
        {
            clearingFirmPrimeBroker = (ClearingFirmPrimeBroker)grdClearingFirmsPrimeBrokers.DataSource;
        }

        public void SetClearingFirmPrimeBroker(ClearingFirmPrimeBroker clearingFirmPrimeBroker)
        {
            grdClearingFirmsPrimeBrokers.DataSource = null;
            grdClearingFirmsPrimeBrokers.DataSource = clearingFirmPrimeBroker;
        }

        private void BindDataGrid()
        {
            try
            {
                ClearingFirmPrimeBroker nullClearingFirmPrimeBroker = new ClearingFirmPrimeBroker(int.MinValue, "");
                Prana.Admin.BLL.ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers = CompanyManager.GetClearingFirmPrimeBroker(_companyID);
                if (clearingFirmsPrimeBrokers.Count <= 0)
                {
                    clearingFirmsPrimeBrokers.Add(nullClearingFirmPrimeBroker);
                    _nullRow = true;
                }
                else
                {
                    _nullRow = false;
                }
                grdClearingFirmsPrimeBrokers.DataSource = clearingFirmsPrimeBrokers;
                grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["ClearingFirmsPrimeBrokersID"].Hidden = true;
                grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns["CompanyID"].Hidden = true;
                ColumnsCollection columns = grdClearingFirmsPrimeBrokers.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key == "ClearingFirmsPrimeBrokersID" && column.Key == "CompanyID")
                    {
                        column.Hidden = true;
                    }
                }
                RefreshGrid();
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                #region LogEntry
                Logger.LoggerWrite("btnLogin_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnLogin_Click", null);
                #endregion
            }
        }

        private bool _nullRow = false;
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            bool result = false;
            errorProvider1.SetError(btnDelete, "");
            CreateCompanyClearingFirmsPrimeBrokers createCompanyClearingFirmsPrimeBrokers = new CreateCompanyClearingFirmsPrimeBrokers();
            if (grdClearingFirmsPrimeBrokers.Rows.Count > 0)
            {
                string clearingFirmsPrimeBrokersName = grdClearingFirmsPrimeBrokers.ActiveRow.Cells["ClearingFirmsPrimeBrokersName"].Text.ToString();
                if (clearingFirmsPrimeBrokersName != "")
                {
                    if (MessageBox.Show(this, "Do you want to delete this ClearingFirmPrimeBroker?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int clearingFirmPrimeBrokerID = int.Parse(grdClearingFirmsPrimeBrokers.ActiveRow.Cells["ClearingFirmsPrimeBrokersID"].Text.ToString());

                        Prana.Admin.BLL.ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers = (ClearingFirmsPrimeBrokers)grdClearingFirmsPrimeBrokers.DataSource;
                        Prana.Admin.BLL.ClearingFirmPrimeBroker clearingFirmPrimeBroker = new Prana.Admin.BLL.ClearingFirmPrimeBroker();


                        clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersID = int.Parse(grdClearingFirmsPrimeBrokers.ActiveRow.Cells["ClearingFirmsPrimeBrokersID"].Text.ToString());
                        clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName = grdClearingFirmsPrimeBrokers.ActiveRow.Cells["ClearingFirmsPrimeBrokersName"].Text.ToString();
                        clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName = grdClearingFirmsPrimeBrokers.ActiveRow.Cells["ClearingFirmsPrimeBrokersShortName"].Text.ToString();
                        clearingFirmPrimeBroker.CompanyID = int.Parse(grdClearingFirmsPrimeBrokers.ActiveRow.Cells["CompanyID"].Text.ToString());
                        //clearingFirmsPrimeBrokers.RemoveAt(clearingFirmsPrimeBrokers.IndexOf(clearingFirmPrimeBroker));
                        if (clearingFirmPrimeBrokerID != int.MinValue)
                        {
                            result = CompanyManager.DeleteClearingFirmPrimeBroker(clearingFirmPrimeBrokerID);
                            if (result == false)
                            {
                                errorProvider1.SetError(btnDelete, "The selected broker is referred by some Client Broker Venue. Please unallocate that first before deleting it !");
                            }
                            else
                            {
                                clearingFirmsPrimeBrokers.RemoveAt(clearingFirmsPrimeBrokers.IndexOf(clearingFirmPrimeBroker));
                            }
                        }
                        else
                        {
                            clearingFirmsPrimeBrokers.RemoveAt(clearingFirmsPrimeBrokers.IndexOf(clearingFirmPrimeBroker));
                            //errorProvider1.SetError(btnDelete, "There is nothing do delete in the Grid!");
                        }

                        ClearingFirmsPrimeBrokers newClearingFirmsPrimeBrokers = new ClearingFirmsPrimeBrokers();
                        foreach (ClearingFirmPrimeBroker tempClearingFirmPrimeBroker in clearingFirmsPrimeBrokers)
                        {
                            newClearingFirmsPrimeBrokers.Add(tempClearingFirmPrimeBroker);
                        }
                        if (clearingFirmsPrimeBrokers.Count <= 0)
                        {
                            newClearingFirmsPrimeBrokers.Add(new ClearingFirmPrimeBroker(int.MinValue, ""));
                            createCompanyClearingFirmsPrimeBrokers.NoData = 1;
                            _nullRow = true;
                        }
                        else
                        {
                            createCompanyClearingFirmsPrimeBrokers.NoData = 0;
                            _nullRow = false;
                        }
                        grdClearingFirmsPrimeBrokers.DataSource = newClearingFirmsPrimeBrokers;
                    }
                }
            }
        }

        private CreateCompanyClearingFirmsPrimeBrokers createCompanyClearingFirmsPrimeBrokersEdit = null;

        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            if (createCompanyClearingFirmsPrimeBrokersEdit == null)
            {
                if ((grdClearingFirmsPrimeBrokers.Rows.Count > 0) && (_nullRow == false))
                {
                    createCompanyClearingFirmsPrimeBrokersEdit = new CreateCompanyClearingFirmsPrimeBrokers();
                    //					Prana.Admin.BLL.ClearingFirmPrimeBroker clearingFirmPrimeBrokerEdit = new Prana.Admin.BLL.ClearingFirmPrimeBroker();
                    //					clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersID = int.Parse(grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 0].ToString());
                    //					clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersName = grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 1].ToString();
                    //					clearingFirmPrimeBrokerEdit.ClearingFirmsPrimeBrokersShortName = grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 3].ToString();
                    //					clearingFirmPrimeBrokerEdit.CompanyID = int.Parse(grdClearingFirmsPrimeBrokers[grdClearingFirmsPrimeBrokers.CurrentCell.RowNumber, 2].ToString());
                    //					createCompanyClearingFirmsPrimeBrokersEdit.ClearingFirmPrimeBrokerEdit =  clearingFirmPrimeBrokerEdit;

                    createCompanyClearingFirmsPrimeBrokersEdit.ClearingFirmPrimeBrokerEdit = (ClearingFirmPrimeBroker)((Prana.Admin.BLL.ClearingFirmsPrimeBrokers)grdClearingFirmsPrimeBrokers.DataSource)[grdClearingFirmsPrimeBrokers.ActiveRow.Index];
                    //createCompanyClearingFirmsPrimeBrokersEdit.ClearingFirmPrimeBrokerEdit = (ClearingFirmPrimeBroker)((Prana.Admin.BLL.ClearingFirmsPrimeBrokers)grdClearingFirmsPrimeBrokers.DataSource)[grdClearingFirmsPrimeBrokers.ActiveCell.Row.Index]; 

                    createCompanyClearingFirmsPrimeBrokersEdit.CurrentClearingFirmsPrimeBrokers = (Prana.Admin.BLL.ClearingFirmsPrimeBrokers)grdClearingFirmsPrimeBrokers.DataSource;
                    createCompanyClearingFirmsPrimeBrokersEdit.ShowDialog(this.Parent);
                    createCompanyClearingFirmsPrimeBrokersEdit = null;
                }
            }
        }


        private void GridClearingFirmsPrimeBrokers_Load(object sender, System.EventArgs e)
        {

        }
    }
}
