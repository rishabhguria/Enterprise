#region Using
using Prana.Admin.BLL;
using System.Windows.Forms;
#endregion

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for UploadAUECHolidays.
    /// </summary>
    public class UploadAUECHolidays : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnCopyHolidays;
        private System.Windows.Forms.CheckedListBox checkedlstAUECExchange;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public UploadAUECHolidays()
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
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnCopyHolidays != null)
                {
                    btnCopyHolidays.Dispose();
                }
                if (checkedlstAUECExchange != null)
                {
                    checkedlstAUECExchange.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadAUECHolidays));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCopyHolidays = new System.Windows.Forms.Button();
            this.checkedlstAUECExchange = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Location = new System.Drawing.Point(161, 188);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCopyHolidays
            // 
            this.btnCopyHolidays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnCopyHolidays.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCopyHolidays.BackgroundImage")));
            this.btnCopyHolidays.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCopyHolidays.Location = new System.Drawing.Point(83, 188);
            this.btnCopyHolidays.Name = "btnCopyHolidays";
            this.btnCopyHolidays.Size = new System.Drawing.Size(75, 23);
            this.btnCopyHolidays.TabIndex = 4;
            this.btnCopyHolidays.UseVisualStyleBackColor = false;
            this.btnCopyHolidays.Click += new System.EventHandler(this.btnCopyHolidays_Click);
            // 
            // checkedlstAUECExchange
            // 
            this.checkedlstAUECExchange.CheckOnClick = true;
            this.checkedlstAUECExchange.Location = new System.Drawing.Point(35, 6);
            this.checkedlstAUECExchange.Name = "checkedlstAUECExchange";
            this.checkedlstAUECExchange.Size = new System.Drawing.Size(248, 180);
            this.checkedlstAUECExchange.TabIndex = 3;
            // 
            // UploadAUECHolidays
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(318, 211);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCopyHolidays);
            this.Controls.Add(this.checkedlstAUECExchange);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UploadAUECHolidays";
            this.Text = "UploadHolidays";
            this.ResumeLayout(false);

        }
        #endregion

        public void BindAUECExchangeList()
        {
            checkedlstAUECExchange.Refresh();
            Exchanges auecExchanges = new Exchanges();
            auecExchanges = AUECManager.GetAUECExchanges(_auecID);
            if (auecExchanges.Count > 0)
            {
                checkedlstAUECExchange.DataSource = auecExchanges;
                checkedlstAUECExchange.DisplayMember = "DisplayName";
                checkedlstAUECExchange.ValueMember = "AUECID";

                //checkedlstAUECExchange.SelectedIndex = -1;
            }
            else
            {
                checkedlstAUECExchange.DataSource = null;
            }
        }

        private int _auecID = int.MinValue;

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            if (checkedlstAUECExchange.CheckedItems.Count > 0)
            {
                for (int j = 0; j < checkedlstAUECExchange.Items.Count; j++)
                {
                    checkedlstAUECExchange.SetItemChecked(j, false);
                }
            }
            //this.Close();
            this.Hide();
        }

        private void btnCopyHolidays_Click(object sender, System.EventArgs e)
        {
            bool result = false;
            Exchanges auecExchanges = new Exchanges();
            for (int i = 0, count = checkedlstAUECExchange.Items.Count; i < count; i++)
            {
                if (checkedlstAUECExchange.GetItemChecked(i) == true)
                {
                    checkedlstAUECExchange.SetSelected(i, true);
                    auecExchanges.Add((Exchange)checkedlstAUECExchange.SelectedItem);
                }
            }
            if (auecExchanges.Count > 0)
            {
                result = AUECManager.CopyAUECExchangeHolidays(auecExchanges, _auecID);
                //			if(result == true)
                //			{
                MessageBox.Show("Holidays copied to the selected exchanges");
                //this.Close();w
                //this.Hide();
                //			}
            }
            //int option = int.MinValue;
            if (checkedlstAUECExchange.CheckedItems.Count > 0)
            {
                for (int j = 0; j < checkedlstAUECExchange.Items.Count; j++)
                {
                    checkedlstAUECExchange.SetItemChecked(j, false);
                }
                this.Hide();
            }
            else if (MessageBox.Show("You have not selected any exchange to save. Do you want to try again ?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                this.Hide();
            }
        }

        public int AUECID
        {
            get
            {
                return _auecID;
            }
            set
            {
                if (value != int.MinValue)
                {
                    _auecID = value;
                    BindAUECExchangeList();
                }
            }
        }
    }
}
