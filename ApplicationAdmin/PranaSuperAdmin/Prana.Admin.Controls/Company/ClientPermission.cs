using Prana.Admin.BLL;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for ClientPermission.
    /// </summary>
    public class ClientPermission : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.GroupBox grpbxAUEC;
        #region Private Properties
        private int _companyID;
        private int _clientID;
        private AUECs _companyauecs;
        private AUECs _clientAUECS;
        private Prana.Admin.BLL.Company _companyVenue;
        #endregion
        private System.Windows.Forms.GroupBox grpcompanyvenue;
        private System.Windows.Forms.CheckedListBox checkedlstCopmanyVenue;
        private System.Windows.Forms.CheckedListBox checkedlstAUEC;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;


        public ClientPermission()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

        }
        public void Setup(int companyID, int companyClientID)
        {
            if (!(companyClientID == 0 && companyID == 0))
            {
                _companyID = companyID;
                _clientID = companyClientID;
                //_clientAUECS = new AUECs();
                _companyauecs = AUECManager.GetCompanyAUEC(_companyID);
                _clientAUECS = AUECManager.GetClientAUEC(_clientID);
                BindAUECS();
            }
            else
                BindAUECS();




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
                if (grpbxAUEC != null)
                {
                    grpbxAUEC.Dispose();
                }
                if (grpcompanyvenue != null)
                {
                    grpcompanyvenue.Dispose();
                }
                if (checkedlstAUEC != null)
                {
                    checkedlstAUEC.Dispose();
                }
                if (checkedlstCopmanyVenue != null)
                {
                    checkedlstCopmanyVenue.Dispose();
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
            this.grpcompanyvenue = new System.Windows.Forms.GroupBox();
            this.checkedlstCopmanyVenue = new System.Windows.Forms.CheckedListBox();
            this.grpbxAUEC = new System.Windows.Forms.GroupBox();
            this.checkedlstAUEC = new System.Windows.Forms.CheckedListBox();
            this.grpcompanyvenue.SuspendLayout();
            this.grpbxAUEC.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpcompanyvenue
            // 
            this.grpcompanyvenue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grpcompanyvenue.Controls.Add(this.checkedlstCopmanyVenue);
            this.grpcompanyvenue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpcompanyvenue.Location = new System.Drawing.Point(6, 8);
            this.grpcompanyvenue.Name = "grpcompanyvenue";
            this.grpcompanyvenue.Size = new System.Drawing.Size(294, 266);
            this.grpcompanyvenue.TabIndex = 2;
            this.grpcompanyvenue.TabStop = false;
            this.grpcompanyvenue.Text = "Client Venue";
            // 
            // checkedlstCopmanyVenue
            // 
            this.checkedlstCopmanyVenue.CheckOnClick = true;
            this.checkedlstCopmanyVenue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstCopmanyVenue.Location = new System.Drawing.Point(9, 26);
            this.checkedlstCopmanyVenue.Name = "checkedlstCopmanyVenue";
            this.checkedlstCopmanyVenue.Size = new System.Drawing.Size(276, 228);
            this.checkedlstCopmanyVenue.TabIndex = 4;
            // 
            // grpbxAUEC
            // 
            this.grpbxAUEC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.grpbxAUEC.Controls.Add(this.checkedlstAUEC);
            this.grpbxAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpbxAUEC.Location = new System.Drawing.Point(316, 8);
            this.grpbxAUEC.Name = "grpbxAUEC";
            this.grpbxAUEC.Size = new System.Drawing.Size(294, 266);
            this.grpbxAUEC.TabIndex = 3;
            this.grpbxAUEC.TabStop = false;
            this.grpbxAUEC.Text = "AUEC";
            // 
            // checkedlstAUEC
            // 
            this.checkedlstAUEC.CheckOnClick = true;
            this.checkedlstAUEC.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstAUEC.Location = new System.Drawing.Point(9, 26);
            this.checkedlstAUEC.Name = "checkedlstAUEC";
            this.checkedlstAUEC.Size = new System.Drawing.Size(276, 228);
            this.checkedlstAUEC.TabIndex = 5;
            // 
            // ClientPermission
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.grpbxAUEC);
            this.Controls.Add(this.grpcompanyvenue);
            this.Name = "ClientPermission";
            this.Size = new System.Drawing.Size(616, 278);
            this.Load += new System.EventHandler(this.ClientPermission_Load);
            this.grpcompanyvenue.ResumeLayout(false);
            this.grpbxAUEC.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private void BindAUECS()
        {


            System.Data.DataTable dtauec = new System.Data.DataTable();
            dtauec.Columns.Add("Data");
            dtauec.Columns.Add("Value");
            object[] rowAUEC = new object[2];
            if (_companyauecs.Count > 0)
            {
                foreach (Prana.Admin.BLL.AUEC auec in _companyauecs)
                {
                    Currency currency = new Currency();
                    //currency = AUECManager.GetCurrency(auec.Exchange.Currency);
                    //string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.DisplayName.ToString() + "/" + currency.CurrencySymbol.ToString();
                    string Data = auec.AUECString;
                    int Value = auec.AUECID;

                    rowAUEC[0] = Data;
                    rowAUEC[1] = Value;
                    dtauec.Rows.Add(rowAUEC);
                }
                checkedlstAUEC.DataSource = dtauec;
                checkedlstAUEC.DisplayMember = "Data";
                checkedlstAUEC.ValueMember = "Value";
            }
            if (_companyauecs.Count > 0)
            {
                for (int j = 0; j < checkedlstAUEC.Items.Count; j++)
                {
                    checkedlstAUEC.SetItemChecked(j, false);
                }
            }

            checkedlstAUEC.SelectedIndex = -1;
            checkedlstAUEC.ClearSelected();
            foreach (Prana.Admin.BLL.AUEC clientAUEC in _clientAUECS)
            {
                checkedlstAUEC.SelectedValue = clientAUEC.AUECID;
                for (int j = 0; j < checkedlstAUEC.Items.Count; j++)
                {
                    if (int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstAUEC.Items[j]))).Row)).ItemArray[1].ToString()) == int.Parse(clientAUEC.AUECID.ToString()))
                    {
                        checkedlstAUEC.SetItemChecked(j, true);
                    }
                }
            }
            checkedlstAUEC.Refresh();

        }
        private void BindVenues()
        {
            Prana.Admin.BLL.Company companyVenue = CompanyManager.GetCompanyVenueDetails(_companyID);
        }

        public AUECs getClientAUECS()
        {
            int auecID;
            _clientAUECS = new AUECs();
            AUEC aUEC = new AUEC();
            for (int i = 0, count = checkedlstAUEC.CheckedItems.Count; i < count; i++)
            {
                auecID = int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstAUEC.CheckedItems[i]))).Row)).ItemArray[1].ToString());
                _clientAUECS.Add(new Prana.Admin.BLL.AUEC(auecID));
            }

            return _clientAUECS;
        }

        private void ClientPermission_Load(object sender, System.EventArgs e)
        {

        }

        #region Get Set Properties 
        public int CompanyID
        {
            get { return _companyID; }
            //			set
            //			{
            //				
            //				_companyID=value;
            //			
            //			}
        }
        public int ClientID
        {
            get { return _clientID; }
            //			set
            //			{
            //				
            //				_clientID=value;
            //				
            //			
            //			}
        }
        public AUECs ClientAUECS
        {
            get
            {
                getClientAUECS();
                return _clientAUECS;
            }
            set
            {

                _clientAUECS = value;

            }
        }

        public Prana.Admin.BLL.Company CompanyVenue
        {
            set { _companyVenue = value; }
            get { return _companyVenue; }

        }
        #endregion
    }
}
