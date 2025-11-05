using Prana.Admin.BLL;
using System.Drawing;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CreateStrategy.
    /// </summary>
    public class CreateStrategy : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtStrategy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public CreateStrategy()
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
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
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
                if (txtStrategy != null)
                {
                    txtStrategy.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        Prana.Admin.BLL.Strategy _strategyEdit = null;

        public Prana.Admin.BLL.Strategy StrategyEdit
        {
            set { _strategyEdit = value; }
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateStrategy));
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStrategy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(59, 70);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtStrategy);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtShortName);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(266, 66);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Strategy";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(64, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 8);
            this.label3.TabIndex = 35;
            this.label3.Text = "*";
            // 
            // txtStrategy
            // 
            this.txtStrategy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStrategy.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtStrategy.Location = new System.Drawing.Point(96, 20);
            this.txtStrategy.MaxLength = 50;
            this.txtStrategy.Name = "txtStrategy";
            this.txtStrategy.Size = new System.Drawing.Size(148, 21);
            this.txtStrategy.TabIndex = 0;
            this.txtStrategy.GotFocus += new System.EventHandler(this.txtStrategy_GotFocus);
            this.txtStrategy.LostFocus += new System.EventHandler(this.txtStrategy_LostFocus);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(8, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Strategy";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(8, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "Short Name";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(96, 42);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(148, 21);
            this.txtShortName.TabIndex = 1;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(139, 70);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(80, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 8);
            this.label4.TabIndex = 35;
            this.label4.Text = "*";
            // 
            // CreateStrategy
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "CreateStrategy";
            this.Size = new System.Drawing.Size(272, 103);
            this.Load += new System.EventHandler(this.CreateStrategy_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
        #region Focus Colors
        private void txtStrategy_GotFocus(object sender, System.EventArgs e)
        {
            txtStrategy.BackColor = Color.LemonChiffon;
        }
        private void txtStrategy_LostFocus(object sender, System.EventArgs e)
        {
            txtStrategy.BackColor = Color.White;
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

        public void BindForEdit()
        {
            if (_strategyEdit != null)
            {
                txtStrategy.Text = _strategyEdit.StrategyName;
                txtShortName.Text = _strategyEdit.StrategyShortName;
            }
        }
        private int _companyID = int.MinValue;
        public int CompanyID
        {
            set { _companyID = value; }
        }
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (_strategyEdit != null)
            {
                int index = _strategies.IndexOf(_strategyEdit);

                ((Prana.Admin.BLL.Strategy)_strategies[index]).StrategyName = txtStrategy.Text.ToString();
                ((Prana.Admin.BLL.Strategy)_strategies[index]).StrategyShortName = txtShortName.Text.ToString();

                //stbStrategy.Text = "Stored !";

            }
            else
            {
                if (txtStrategy.Text.Trim() == "")
                {
                    //stbStrategy Account.Text = "Please enter Strategy Name!";
                    txtStrategy.Focus();
                }
                if (txtShortName.Text.Trim() == "")
                {
                    //stbStrategy.Text = "Please enter Short Name!";
                    txtShortName.Focus();
                }
                else
                {
                    Prana.Admin.BLL.Strategy strategy = new Prana.Admin.BLL.Strategy();

                    strategy.StrategyName = txtStrategy.Text.ToString();
                    strategy.StrategyShortName = txtShortName.Text.ToString();
                    _strategies.Add(strategy);
                    //stbAccount.Text = "Stored !";
                }
            }
        }

        public void Refresh(object sender, System.EventArgs e)
        {
            txtStrategy.Text = "";
            txtShortName.Text = "";
        }

        private void CreateStrategy_Load(object sender, System.EventArgs e)
        {
            if (_strategyEdit != null)
            {
                BindForEdit();
            }
            else
            {
                Refresh(sender, e);
                //stbAccounts.Text = "";
            }
        }

        private Prana.Admin.BLL.Strategies _strategies = new Strategies();

        private void btnClose_Click(object sender, System.EventArgs e)
        {

        }

        public Strategies CurrentStrategies
        {
            get
            {
                return _strategies;
            }
            set
            {
                if (value != null)
                {
                    _strategies = value;
                }
            }
        }
    }
}
