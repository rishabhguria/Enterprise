using Prana.Admin.BLL;
using System.Drawing;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CreateClearingFirmsPrimeBrokers.
    /// </summary>
    public class CreateClearingFirmsPrimeBrokers : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtClearingFirmsPrimeBrokers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox grpPrimeBroker;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public CreateClearingFirmsPrimeBrokers()
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
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (txtClearingFirmsPrimeBrokers != null)
                {
                    txtClearingFirmsPrimeBrokers.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (grpPrimeBroker != null)
                {
                    grpPrimeBroker.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateClearingFirmsPrimeBrokers));
            this.btnSave = new System.Windows.Forms.Button();
            this.grpPrimeBroker = new System.Windows.Forms.GroupBox();
            this.txtClearingFirmsPrimeBrokers = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.grpPrimeBroker.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(52, 66);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // grpPrimeBroker
            // 
            this.grpPrimeBroker.Controls.Add(this.txtClearingFirmsPrimeBrokers);
            this.grpPrimeBroker.Controls.Add(this.label1);
            this.grpPrimeBroker.Controls.Add(this.label2);
            this.grpPrimeBroker.Controls.Add(this.txtShortName);
            this.grpPrimeBroker.Controls.Add(this.label5);
            this.grpPrimeBroker.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpPrimeBroker.Location = new System.Drawing.Point(2, 0);
            this.grpPrimeBroker.Name = "grpPrimeBroker";
            this.grpPrimeBroker.Size = new System.Drawing.Size(253, 66);
            this.grpPrimeBroker.TabIndex = 0;
            this.grpPrimeBroker.TabStop = false;
            this.grpPrimeBroker.Text = "Clearing Firms/Prime Brokers";
            this.grpPrimeBroker.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // txtClearingFirmsPrimeBrokers
            // 
            this.txtClearingFirmsPrimeBrokers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtClearingFirmsPrimeBrokers.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtClearingFirmsPrimeBrokers.Location = new System.Drawing.Point(88, 20);
            this.txtClearingFirmsPrimeBrokers.MaxLength = 50;
            this.txtClearingFirmsPrimeBrokers.Name = "txtClearingFirmsPrimeBrokers";
            this.txtClearingFirmsPrimeBrokers.Size = new System.Drawing.Size(148, 21);
            this.txtClearingFirmsPrimeBrokers.TabIndex = 0;
            this.txtClearingFirmsPrimeBrokers.GotFocus += new System.EventHandler(this.txtClearingFirmsPrimeBrokers_GotFocus);
            this.txtClearingFirmsPrimeBrokers.LostFocus += new System.EventHandler(this.txtClearingFirmsPrimeBrokers_LostFocus);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(6, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Short Name";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(88, 42);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(148, 21);
            this.txtShortName.TabIndex = 1;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(40, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 10);
            this.label5.TabIndex = 35;
            this.label5.Text = "*";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(128, 66);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(72, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 8);
            this.label4.TabIndex = 34;
            this.label4.Text = "*";
            // 
            // CreateClearingFirmsPrimeBrokers
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(256, 104);
            this.ControlBox = false;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpPrimeBroker);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MinimumSize = new System.Drawing.Size(256, 112);
            this.Name = "CreateClearingFirmsPrimeBrokers";
            this.grpPrimeBroker.ResumeLayout(false);
            this.grpPrimeBroker.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        ClearingFirmsPrimeBrokers _clearingFirmsPrimeBrokers = null;
        public ClearingFirmsPrimeBrokers CurrentClearingFirmsPrimeBrokers
        {
            get { return _clearingFirmsPrimeBrokers; }
            set { _clearingFirmsPrimeBrokers = value; }
        }

        #region Focus Colors
        private void txtClearingFirmsPrimeBrokers_GotFocus(object sender, System.EventArgs e)
        {
            txtClearingFirmsPrimeBrokers.BackColor = Color.LemonChiffon;
        }
        private void txtClearingFirmsPrimeBrokers_LostFocus(object sender, System.EventArgs e)
        {
            txtClearingFirmsPrimeBrokers.BackColor = Color.White;
        }
        private void txtShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.LemonChiffon;
        }
        private void txtShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.White;
        }
        #endregion

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void statusBar1_PanelClick(object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }
    }
}
