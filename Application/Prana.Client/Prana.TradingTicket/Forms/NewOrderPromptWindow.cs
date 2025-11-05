namespace Prana.TradingTicket
{
    /// <summary>
    /// Summary description for NewOrderPrompt.
    /// </summary>
    public class PromptWindow : System.Windows.Forms.Form
    {
        private bool _bTrade = false;
        private System.Windows.Forms.Button btnPlace;
        private System.Windows.Forms.Button btnDontPlace;
        private System.Windows.Forms.TextBox txtMsg;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public PromptWindow(string msg, string windowName)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            txtMsg.Text = msg;
            this.Text = windowName;
            //this.Parent=(System.Windows.Forms.Control )parent;

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
                if (txtMsg != null)
                {
                    txtMsg.Dispose();
                }
                if (btnDontPlace != null)
                {
                    btnDontPlace.Dispose();
                }
                if (btnPlace != null)
                {
                    btnPlace.Dispose();
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
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.btnPlace = new System.Windows.Forms.Button();
            this.btnDontPlace = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMsg
            // 
            this.txtMsg.Enabled = false;
            this.txtMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.txtMsg.Location = new System.Drawing.Point(80, 24);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(272, 56);
            this.txtMsg.TabIndex = 0;
            this.txtMsg.Text = "";
            // 
            // btnPlace
            // 
            this.btnPlace.Location = new System.Drawing.Point(120, 96);
            this.btnPlace.Name = "btnPlace";
            this.btnPlace.TabIndex = 1;
            this.btnPlace.Text = "Continue";
            this.btnPlace.Click += new System.EventHandler(this.btnPlace_Click);
            // 
            // btnDontPlace
            // 
            this.btnDontPlace.Location = new System.Drawing.Point(228, 96);
            this.btnDontPlace.Name = "btnDontPlace";
            this.btnDontPlace.TabIndex = 2;
            this.btnDontPlace.Text = "Edit Order";
            this.btnDontPlace.Click += new System.EventHandler(this.btnDontPlace_Click);
            // 
            // NewOrderPromptWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.ClientSize = new System.Drawing.Size(432, 133);
            this.Controls.Add(this.btnDontPlace);
            this.Controls.Add(this.btnPlace);
            this.Controls.Add(this.txtMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewOrderPromptWindow";
            this.Text = "NewOrderPrompt";
            this.ResumeLayout(false);

        }
        #endregion

        private void btnPlace_Click(object sender, System.EventArgs e)
        {
            _bTrade = true;
            this.Hide();
        }

        private void btnDontPlace_Click(object sender, System.EventArgs e)
        {
            _bTrade = false;
            this.Hide();

        }
        public bool ShouldTrade
        {
            get { return _bTrade; }
        }
    }
}
