using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CreateCompanyStrategy.
    /// </summary>
    public class CreateCompanyStrategy : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtStrategy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private IContainer components;
        public bool validStrategy = true;

        public CreateCompanyStrategy()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            ApplyTheme();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void ApplyTheme()
        {
            try
            {
                //TODO- based on CH release
                //PranaReleaseViewType pranaReleaseType = CachedDataManager.GetInstance.GetPranaReleaseViewType();
                //if (pranaReleaseType == PranaReleaseViewType.CHMiddleWare)
                //{
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.ForeColor = System.Drawing.Color.White;
                //}
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
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
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (txtStrategy != null)
                {
                    txtStrategy.Dispose();
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
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateCompanyStrategy));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStrategy = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(62, 71);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(142, 71);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtStrategy);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtShortName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(274, 64);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Strategy";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(52, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 10);
            this.label3.TabIndex = 33;
            this.label3.Text = "*";
            // 
            // txtStrategy
            // 
            this.txtStrategy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStrategy.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtStrategy.Location = new System.Drawing.Point(106, 18);
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
            this.label1.Location = new System.Drawing.Point(8, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(8, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "Short Name";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(106, 40);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(148, 21);
            this.txtShortName.TabIndex = 1;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(86, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 8);
            this.label4.TabIndex = 33;
            this.label4.Text = "*";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // CreateCompanyStrategy
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(278, 136);
            this.ControlBox = false;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(276, 138);
            this.Name = "CreateCompanyStrategy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client Strategy";
            this.Load += new System.EventHandler(this.CreateCompanyStrategy_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        Prana.Admin.BLL.Strategy _strategyEdit = null;

        public Prana.Admin.BLL.Strategy StrategyEdit
        {
            get { return _strategyEdit; }
            set { _strategyEdit = value; }
        }

        public void BindForEdit()
        {
            if (_strategyEdit != null)
            {
                txtStrategy.Text = _strategyEdit.StrategyName;
                txtShortName.Text = _strategyEdit.StrategyShortName;
            }
        }
        private int _companyID = int.MinValue;
        //Set to -1 By Faisal Shah
        //Needed as Multiple Logins have a problem in saving concurrently
        //Modification Date 07/07/14
        private int _tmpStrategyID = -1;
        public int CompanyID
        {
            set { _companyID = value; }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            validStrategy = true;
            if (_noData == 1)
            {
                _strategies.Clear();
            }
            if (_strategyEdit != null)
            {
                errorProvider1.SetError(txtStrategy, "");
                errorProvider1.SetError(txtShortName, "");
                if (txtStrategy.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtStrategy, "Please enter Strategy Name!");
                    txtStrategy.Focus();
                }
                else if (txtShortName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                    txtShortName.Focus();
                }
                else
                {
                    foreach (Strategy checkStrategy in _strategies)
                    {
                        if (checkStrategy.StrategyName.Trim().ToUpper() == txtStrategy.Text.Trim().ToUpper() && checkStrategy.StrategyName != _strategyEdit.StrategyName)
                        {
                            validStrategy = false;
                            MessageBox.Show(this, "The edited strategy with the same name already exists.", "Alert", MessageBoxButtons.OK);
                            break;
                        }
                        if (checkStrategy.StrategyShortName.Trim().ToUpper() == txtShortName.Text.Trim().ToUpper() && checkStrategy.StrategyShortName != _strategyEdit.StrategyShortName)
                        {
                            validStrategy = false;
                            MessageBox.Show(this, "The edited strategy with the same short name already exists.", "Alert", MessageBoxButtons.OK);
                            break;
                        }
                    }
                    if (validStrategy == true)
                    {
                        _strategyEdit.StrategyName = txtStrategy.Text.ToString().Trim();
                        _strategyEdit.StrategyShortName = txtShortName.Text.ToString().Trim();

                        //Prana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
                        //Prana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");
                        this.Hide();
                    }
                }
            }
            else
            {
                errorProvider1.SetError(txtStrategy, "");
                errorProvider1.SetError(txtShortName, "");
                if (txtStrategy.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtStrategy, "Please enter Strategy Name!");
                    txtStrategy.Focus();
                }
                else if (txtShortName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                    txtShortName.Focus();
                }
                else
                {
                    foreach (Strategy checkStrategy in _strategies)
                    {
                        if (checkStrategy.StrategyName.Trim().ToUpper() == txtStrategy.Text.Trim().ToUpper())
                        {
                            validStrategy = false;
                            MessageBox.Show(this, "The strategy with the same name already exists.", "Alert", MessageBoxButtons.OK);
                            break;
                        }
                        if (checkStrategy.StrategyShortName.Trim().ToUpper() == txtShortName.Text.Trim().ToUpper())
                        {
                            validStrategy = false;
                            MessageBox.Show(this, "The strategy with the same short name already exists.", "Alert", MessageBoxButtons.OK);
                            break;
                        }
                    }
                    if (validStrategy == true)
                    {
                        //Code Commented By Faisal Shah
                        //Dated 07/07/14
                        #region CommentedCode
                        //modified by: Bharat Raturi, 14-march-14
                        //Purpose: Generate ID for the strategy
                        //foreach (Strategy checkStrategy in _strategies)
                        //{
                        //    if (checkStrategy.StrategyID > _tmpStrategyID)
                        //    {
                        //        _tmpStrategyID = checkStrategy.StrategyID;
                        //    }
                        //}
                        //_tmpStrategyID += 1;
                        #endregion
                        Prana.Admin.BLL.Strategy strategy = new Prana.Admin.BLL.Strategy();

                        strategy.StrategyName = txtStrategy.Text.ToString().Trim();
                        strategy.StrategyShortName = txtShortName.Text.ToString().Trim();
                        //modified by: Bharat Raturi, 14-march-2014
                        //Purpose: Generate ID for the strategy
                        strategy.StrategyID = _tmpStrategyID;
                        _strategies.Add(strategy);
                        _tmpStrategyID--;
                        //Prana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
                        //Prana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");
                        this.Hide();
                    }
                }
            }

        }

        public void Refresh(object sender, System.EventArgs e)
        {
            txtStrategy.Text = "";
            txtShortName.Text = "";
        }

        private void CreateCompanyStrategy_Load(object sender, System.EventArgs e)
        {
            if (_strategyEdit != null)
            {
                BindForEdit();
            }
            else
            {
                Refresh(sender, e);
                //Prana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
            }
        }

        private Prana.Admin.BLL.Strategies _strategies = new Strategies();

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            errorProvider1.SetError(txtStrategy, "");
            errorProvider1.SetError(txtShortName, "");
            this.Close();
        }

        private int _noData = int.MinValue;
        public int NoData
        {
            set
            {
                _noData = value;
            }
        }

        public Strategies CurrentCompanyStrategies
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
    }
}
