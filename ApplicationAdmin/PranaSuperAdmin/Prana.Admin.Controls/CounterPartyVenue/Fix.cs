//
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using System.ComponentModel;
using System.Drawing;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for Fix.
    /// </summary>
    public class Fix : System.Windows.Forms.UserControl
    {
        const string C_COMBO_SELECT = "- Select -";

        private System.Windows.Forms.GroupBox grpFixDetails;
        private System.Windows.Forms.Label lblAcronymn;
        private System.Windows.Forms.TextBox txtAcronymn;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label lblFixVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetCompID;
        private System.Windows.Forms.Label lblTargetCompID;
        private System.Windows.Forms.TextBox txtDeliverToCompID;
        private System.Windows.Forms.Label lblDeliverToCompID;
        private System.Windows.Forms.TextBox txtDeliverToSubID;
        private System.Windows.Forms.Label lblDeliverToSubID;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbFixVersion;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private IContainer components;

        public Fix()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            //BindFix();

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
                if (grpFixDetails != null)
                {
                    grpFixDetails.Dispose();
                }
                if (lblAcronymn != null)
                {
                    lblAcronymn.Dispose();
                }
                if (txtAcronymn != null)
                {
                    txtAcronymn.Dispose();
                }
                if (label50 != null)
                {
                    label50.Dispose();
                }
                if (lblFixVersion != null)
                {
                    lblFixVersion.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (txtTargetCompID != null)
                {
                    txtTargetCompID.Dispose();
                }
                if (lblTargetCompID != null)
                {
                    lblTargetCompID.Dispose();
                }
                if (txtDeliverToCompID != null)
                {
                    txtDeliverToCompID.Dispose();
                }
                if (lblDeliverToCompID != null)
                {
                    lblDeliverToCompID.Dispose();
                }
                if (txtDeliverToSubID != null)
                {
                    txtDeliverToSubID.Dispose();
                }
                if (lblDeliverToSubID != null)
                {
                    lblDeliverToSubID.Dispose();
                }
                if (cmbFixVersion != null)
                {
                    cmbFixVersion.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixVersion", 1);
            this.grpFixDetails = new System.Windows.Forms.GroupBox();
            this.txtDeliverToSubID = new System.Windows.Forms.TextBox();
            this.lblDeliverToSubID = new System.Windows.Forms.Label();
            this.txtDeliverToCompID = new System.Windows.Forms.TextBox();
            this.lblDeliverToCompID = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTargetCompID = new System.Windows.Forms.TextBox();
            this.lblTargetCompID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.cmbFixVersion = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblFixVersion = new System.Windows.Forms.Label();
            this.txtAcronymn = new System.Windows.Forms.TextBox();
            this.lblAcronymn = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.grpFixDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFixVersion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpFixDetails
            // 
            this.grpFixDetails.Controls.Add(this.txtDeliverToSubID);
            this.grpFixDetails.Controls.Add(this.lblDeliverToSubID);
            this.grpFixDetails.Controls.Add(this.txtDeliverToCompID);
            this.grpFixDetails.Controls.Add(this.lblDeliverToCompID);
            this.grpFixDetails.Controls.Add(this.label2);
            this.grpFixDetails.Controls.Add(this.txtTargetCompID);
            this.grpFixDetails.Controls.Add(this.lblTargetCompID);
            this.grpFixDetails.Controls.Add(this.label1);
            this.grpFixDetails.Controls.Add(this.label50);
            this.grpFixDetails.Controls.Add(this.cmbFixVersion);
            this.grpFixDetails.Controls.Add(this.lblFixVersion);
            this.grpFixDetails.Controls.Add(this.txtAcronymn);
            this.grpFixDetails.Controls.Add(this.lblAcronymn);
            this.grpFixDetails.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpFixDetails.Location = new System.Drawing.Point(4, 6);
            this.grpFixDetails.Name = "grpFixDetails";
            this.grpFixDetails.Size = new System.Drawing.Size(290, 132);
            this.grpFixDetails.TabIndex = 0;
            this.grpFixDetails.TabStop = false;
            this.grpFixDetails.Text = "Details";
            this.grpFixDetails.Enter += new System.EventHandler(this.grpFixDetails_Enter);
            // 
            // txtDeliverToSubID
            // 
            this.txtDeliverToSubID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeliverToSubID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDeliverToSubID.Location = new System.Drawing.Point(116, 106);
            this.txtDeliverToSubID.MaxLength = 50;
            this.txtDeliverToSubID.Name = "txtDeliverToSubID";
            this.txtDeliverToSubID.Size = new System.Drawing.Size(154, 21);
            this.txtDeliverToSubID.TabIndex = 93;
            this.txtDeliverToSubID.GotFocus += new System.EventHandler(this.txtDeliverToSubID_GotFocus);
            this.txtDeliverToSubID.LostFocus += new System.EventHandler(this.txtDeliverToSubID_LostFocus);
            // 
            // lblDeliverToSubID
            // 
            this.lblDeliverToSubID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblDeliverToSubID.Location = new System.Drawing.Point(8, 108);
            this.lblDeliverToSubID.Name = "lblDeliverToSubID";
            this.lblDeliverToSubID.Size = new System.Drawing.Size(84, 16);
            this.lblDeliverToSubID.TabIndex = 92;
            this.lblDeliverToSubID.Text = "DeliverToSubID";
            // 
            // txtDeliverToCompID
            // 
            this.txtDeliverToCompID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeliverToCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDeliverToCompID.Location = new System.Drawing.Point(116, 84);
            this.txtDeliverToCompID.MaxLength = 50;
            this.txtDeliverToCompID.Name = "txtDeliverToCompID";
            this.txtDeliverToCompID.Size = new System.Drawing.Size(154, 21);
            this.txtDeliverToCompID.TabIndex = 90;
            this.txtDeliverToCompID.GotFocus += new System.EventHandler(this.txtDeliverToCompID_GotFocus);
            this.txtDeliverToCompID.LostFocus += new System.EventHandler(this.txtDeliverToCompID_LostFocus);
            // 
            // lblDeliverToCompID
            // 
            this.lblDeliverToCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblDeliverToCompID.Location = new System.Drawing.Point(8, 86);
            this.lblDeliverToCompID.Name = "lblDeliverToCompID";
            this.lblDeliverToCompID.Size = new System.Drawing.Size(92, 16);
            this.lblDeliverToCompID.TabIndex = 89;
            this.lblDeliverToCompID.Text = "DeliverToCompID";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(88, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 8);
            this.label2.TabIndex = 88;
            this.label2.Text = "*";
            // 
            // txtTargetCompID
            // 
            this.txtTargetCompID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTargetCompID.Location = new System.Drawing.Point(116, 62);
            this.txtTargetCompID.MaxLength = 50;
            this.txtTargetCompID.Name = "txtTargetCompID";
            this.txtTargetCompID.Size = new System.Drawing.Size(154, 21);
            this.txtTargetCompID.TabIndex = 87;
            this.txtTargetCompID.GotFocus += new System.EventHandler(this.txtTargetCompID_GotFocus);
            this.txtTargetCompID.LostFocus += new System.EventHandler(this.txtTargetCompID_LostFocus);
            // 
            // lblTargetCompID
            // 
            this.lblTargetCompID.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTargetCompID.Location = new System.Drawing.Point(8, 64);
            this.lblTargetCompID.Name = "lblTargetCompID";
            this.lblTargetCompID.Size = new System.Drawing.Size(80, 16);
            this.lblTargetCompID.TabIndex = 86;
            this.lblTargetCompID.Text = "TargetCompID";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(70, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 85;
            this.label1.Text = "*";
            // 
            // label50
            // 
            this.label50.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label50.ForeColor = System.Drawing.Color.Red;
            this.label50.Location = new System.Drawing.Point(66, 42);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(8, 8);
            this.label50.TabIndex = 84;
            this.label50.Text = "*";
            // 
            // cmbFixVersion
            // 
            this.cmbFixVersion.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cmbFixVersion.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            this.cmbFixVersion.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbFixVersion.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbFixVersion.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.cmbFixVersion.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbFixVersion.DropDownWidth = 0;
            this.cmbFixVersion.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbFixVersion.Location = new System.Drawing.Point(116, 40);
            this.cmbFixVersion.MaxDropDownItems = 12;
            this.cmbFixVersion.MaxLength = 50;
            this.cmbFixVersion.Name = "cmbFixVersion";
            this.cmbFixVersion.Size = new System.Drawing.Size(154, 21);
            this.cmbFixVersion.TabIndex = 83;
            this.cmbFixVersion.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbFixVersion.GotFocus += new System.EventHandler(this.cmbFixVersion_GotFocus);
            this.cmbFixVersion.LostFocus += new System.EventHandler(this.cmbFixVersion_LostFocus);
            // 
            // lblFixVersion
            // 
            this.lblFixVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFixVersion.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblFixVersion.Location = new System.Drawing.Point(8, 42);
            this.lblFixVersion.Name = "lblFixVersion";
            this.lblFixVersion.Size = new System.Drawing.Size(58, 16);
            this.lblFixVersion.TabIndex = 82;
            this.lblFixVersion.Text = "FixVersion";
            // 
            // txtAcronymn
            // 
            this.txtAcronymn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAcronymn.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAcronymn.Location = new System.Drawing.Point(116, 18);
            this.txtAcronymn.MaxLength = 10;
            this.txtAcronymn.Name = "txtAcronymn";
            this.txtAcronymn.Size = new System.Drawing.Size(154, 21);
            this.txtAcronymn.TabIndex = 81;
            this.txtAcronymn.GotFocus += new System.EventHandler(this.txtAcronymn_GotFocus);
            this.txtAcronymn.LostFocus += new System.EventHandler(this.txtAcronymn_LostFocus);
            // 
            // lblAcronymn
            // 
            this.lblAcronymn.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAcronymn.Location = new System.Drawing.Point(8, 20);
            this.lblAcronymn.Name = "lblAcronymn";
            this.lblAcronymn.Size = new System.Drawing.Size(62, 16);
            this.lblAcronymn.TabIndex = 15;
            this.lblAcronymn.Text = "Acronymn";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(6, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 16);
            this.label3.TabIndex = 35;
            this.label3.Text = "* Required Field";
            // 
            // Fix
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.label3);
            this.Controls.Add(this.grpFixDetails);
            this.Name = "Fix";
            this.Size = new System.Drawing.Size(296, 158);
            this.grpFixDetails.ResumeLayout(false);
            this.grpFixDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFixVersion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtAcronymn_GotFocus(object sender, System.EventArgs e)
        {
            txtAcronymn.BackColor = Color.LemonChiffon;
        }
        private void txtAcronymn_LostFocus(object sender, System.EventArgs e)
        {
            txtAcronymn.BackColor = Color.White;
        }
        private void txtTargetCompID_GotFocus(object sender, System.EventArgs e)
        {
            txtTargetCompID.BackColor = Color.LemonChiffon;
        }
        private void txtTargetCompID_LostFocus(object sender, System.EventArgs e)
        {
            txtTargetCompID.BackColor = Color.White;
        }
        private void txtDeliverToCompID_GotFocus(object sender, System.EventArgs e)
        {
            txtDeliverToCompID.BackColor = Color.LemonChiffon;
        }
        private void txtDeliverToCompID_LostFocus(object sender, System.EventArgs e)
        {
            txtDeliverToCompID.BackColor = Color.White;
        }
        private void txtDeliverToSubID_GotFocus(object sender, System.EventArgs e)
        {
            txtDeliverToSubID.BackColor = Color.LemonChiffon;
        }
        private void txtDeliverToSubID_LostFocus(object sender, System.EventArgs e)
        {
            txtDeliverToSubID.BackColor = Color.White;
        }

        private void cmbFixVersion_GotFocus(object sender, System.EventArgs e)
        {
            cmbFixVersion.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbFixVersion_LostFocus(object sender, System.EventArgs e)
        {
            cmbFixVersion.Appearance.BackColor = Color.White;
        }
        #endregion


        public CounterPartyVenue CounterPartyProperty
        {
            get
            {
                CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
                GetCounterPartyVenueFix(counterPartyVenue);
                return counterPartyVenue;
            }
            set
            {
                //SetCounterPartyVenueFix(value);
            }
        }

        private int _counterPartyVenueID = int.MinValue;
        public int CounterPartyVenueID
        {
            get { return _counterPartyVenueID; }
            set
            {
                //_counterPartyVenueID = value;
                //CounterPartyVenue counterPartyVenue = CounterPartyManager.GetCVFIX(_counterPartyVenueID);
                //SetCounterPartyVenueFix(counterPartyVenue);
            }
        }

        public void SetupControl(int counterPartyVenueID)
        {
            BindFix();
            _counterPartyVenueID = counterPartyVenueID;
            CounterPartyVenue counterPartyVenue = CounterPartyManager.GetCVFIX(_counterPartyVenueID);
            SetCounterPartyVenueFix(counterPartyVenue);
        }

        /// <summary>
        /// This method binds the existing <see cref="Fixs"/> in the ComboBox control by assigning the 
        /// fixs object to its datasource property.
        /// Binds Fixs combo.
        /// </summary>
        private void BindFix()
        {
            Fixs fixs = Fixmanager.GetFixs();
            fixs.Insert(0, new Prana.Admin.BLL.Fix(int.MinValue, C_COMBO_SELECT));
            cmbFixVersion.DataSource = null;
            cmbFixVersion.DataSource = fixs;
            cmbFixVersion.DisplayMember = "FixVersion";
            cmbFixVersion.ValueMember = "FixID";
        }

        private void grpFixDetails_Enter(object sender, System.EventArgs e)
        {

        }

        public int GetCounterPartyVenueFix(CounterPartyVenue counterPartyVenue)
        {
            int result = int.MinValue;

            errorProvider1.SetError(txtAcronymn, "");
            errorProvider1.SetError(cmbFixVersion, "");
            //			errorProvider1.SetError(txtDeliverToCompID, "");
            //			errorProvider1.SetError(txtDeliverToSubID, "");
            errorProvider1.SetError(txtTargetCompID, "");

            #region Check if the fields have atleast one item selected
            if (txtAcronymn.Text.Trim() == "")
            {
                errorProvider1.SetError(txtAcronymn, "Please enter Acronymn!");
                txtAcronymn.Focus();
            }
            else if (int.Parse(cmbFixVersion.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbFixVersion, "Please select Fix Version!");
                cmbFixVersion.Focus();
            }
            else if (txtTargetCompID.Text.Trim() == "")
            {
                errorProvider1.SetError(txtTargetCompID, "Please enter TargetCompID!");
                txtTargetCompID.Focus();
            }
            //			else if(txtDeliverToCompID.Text.Trim() == "")
            //			{
            //				errorProvider1.SetError(txtDeliverToCompID, "Please enter DeliverToCompID!");
            //				txtDeliverToCompID.Focus();
            //			}
            //			else if(txtDeliverToSubID.Text.Trim() == "")
            //			{
            //				errorProvider1.SetError(txtDeliverToSubID, "Please enter DeliverToSubID!");
            //				txtDeliverToSubID.Focus();
            //			}
            #endregion
            else
            {
                counterPartyVenue.Acronym = txtAcronymn.Text.ToString();
                counterPartyVenue.FixVersionID = int.Parse(cmbFixVersion.Value.ToString());
                counterPartyVenue.TargetCompID = txtTargetCompID.Text.ToString();
                counterPartyVenue.DeliverToCompID = txtDeliverToCompID.Text.ToString();
                counterPartyVenue.DeliverToSubID = txtDeliverToSubID.Text.ToString();
                result = 1;
            }
            return result;
        }

        public void SetCounterPartyVenueFix(CounterPartyVenue counterPartyVenue)
        {
            txtAcronymn.Text = counterPartyVenue.Acronym.ToString();
            cmbFixVersion.Value = int.Parse(counterPartyVenue.FixVersionID.ToString());
            txtTargetCompID.Text = counterPartyVenue.TargetCompID.ToString();
            txtDeliverToCompID.Text = counterPartyVenue.DeliverToCompID.ToString();
            txtDeliverToSubID.Text = counterPartyVenue.DeliverToSubID.ToString();

            ColumnsCollection columns = cmbFixVersion.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != "FixVersion")
                {
                    column.Hidden = true;
                }
            }
        }
    }
}
