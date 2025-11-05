using Prana.Admin.BLL;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CreateClientTrader.
    /// </summary>
    public class CreateClientTrader : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtEMail;
        private System.Windows.Forms.TextBox txtTelephoneWork;
        private System.Windows.Forms.TextBox txtTelephoneHome;
        private System.Windows.Forms.TextBox txtTelephoneCell;
        private System.Windows.Forms.TextBox txtPager;
        private System.Windows.Forms.TextBox txtFax;
        private IContainer components;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private Label label14;

        Prana.Admin.BLL.Trader _traderEdit = null;
        public Prana.Admin.BLL.Trader TraderEdit
        {
            set { _traderEdit = value; }
            get { return _traderEdit; }
        }

        public CreateClientTrader()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            BindForEdit();
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
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (label7 != null)
                {
                    label7.Dispose();
                }
                if (label8 != null)
                {
                    label8.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (label10 != null)
                {
                    label10.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (txtFirstName != null)
                {
                    txtFirstName.Dispose();
                }
                if (txtLastName != null)
                {
                    txtLastName.Dispose();
                }
                if (txtShortName != null)
                {
                    txtShortName.Dispose();
                }
                if (txtTitle != null)
                {
                    txtTitle.Dispose();
                }
                if (txtEMail != null)
                {
                    txtEMail.Dispose();
                }
                if (txtTelephoneWork != null)
                {
                    txtTelephoneWork.Dispose();
                }
                if (txtTelephoneHome != null)
                {
                    txtTelephoneHome.Dispose();
                }
                if (txtTelephoneCell != null)
                {
                    txtTelephoneCell.Dispose();
                }
                if (txtPager != null)
                {
                    txtPager.Dispose();
                }
                if (txtFax != null)
                {
                    txtFax.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label22 != null)
                {
                    label22.Dispose();
                }
                if (label11 != null)
                {
                    label11.Dispose();
                }
                if (label12 != null)
                {
                    label12.Dispose();
                }
                if (label13 != null)
                {
                    label13.Dispose();
                }
                if (label17 != null)
                {
                    label17.Dispose();
                }
                if (label19 != null)
                {
                    label19.Dispose();
                }
                if (label14 != null)
                {
                    label14.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateClientTrader));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtEMail = new System.Windows.Forms.TextBox();
            this.txtTelephoneWork = new System.Windows.Forms.TextBox();
            this.txtTelephoneHome = new System.Windows.Forms.TextBox();
            this.txtTelephoneCell = new System.Windows.Forms.TextBox();
            this.txtPager = new System.Windows.Forms.TextBox();
            this.txtFax = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label13 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtFirstName);
            this.groupBox1.Controls.Add(this.txtLastName);
            this.groupBox1.Controls.Add(this.txtShortName);
            this.groupBox1.Controls.Add(this.txtTitle);
            this.groupBox1.Controls.Add(this.txtEMail);
            this.groupBox1.Controls.Add(this.txtTelephoneWork);
            this.groupBox1.Controls.Add(this.txtTelephoneHome);
            this.groupBox1.Controls.Add(this.txtTelephoneCell);
            this.groupBox1.Controls.Add(this.txtPager);
            this.groupBox1.Controls.Add(this.txtFax);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(2, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 256);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Client Trader";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label19.Location = new System.Drawing.Point(8, 148);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(98, 16);
            this.label19.TabIndex = 59;
            this.label19.Text = "(1-111-111111)";
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(46, 106);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(12, 10);
            this.label12.TabIndex = 58;
            this.label12.Text = "*";
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(82, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(12, 8);
            this.label11.TabIndex = 57;
            this.label11.Text = "*";
            // 
            // label22
            // 
            this.label22.ForeColor = System.Drawing.Color.Red;
            this.label22.Location = new System.Drawing.Point(82, 20);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(12, 8);
            this.label22.TabIndex = 56;
            this.label22.Text = "*";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Name";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(8, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Last Name";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(8, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 18);
            this.label3.TabIndex = 0;
            this.label3.Text = "Short Name";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(8, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 16);
            this.label4.TabIndex = 0;
            this.label4.Text = "Title";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(8, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "E-Mail";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(8, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "Work #";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(8, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 18);
            this.label7.TabIndex = 0;
            this.label7.Text = "Home #";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(8, 165);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 18);
            this.label8.TabIndex = 0;
            this.label8.Text = "Cell #";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.Location = new System.Drawing.Point(8, 187);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 18);
            this.label9.TabIndex = 0;
            this.label9.Text = "Pager #";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label10.Location = new System.Drawing.Point(8, 230);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 18);
            this.label10.TabIndex = 0;
            this.label10.Text = "Fax";
            // 
            // txtFirstName
            // 
            this.txtFirstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFirstName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFirstName.Location = new System.Drawing.Point(116, 17);
            this.txtFirstName.MaxLength = 50;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(100, 21);
            this.txtFirstName.TabIndex = 1;
            this.txtFirstName.GotFocus += new System.EventHandler(this.txtFirstName_GotFocus);
            this.txtFirstName.LostFocus += new System.EventHandler(this.txtFirstName_LostFocus);
            // 
            // txtLastName
            // 
            this.txtLastName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtLastName.Location = new System.Drawing.Point(116, 39);
            this.txtLastName.MaxLength = 50;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(100, 21);
            this.txtLastName.TabIndex = 2;
            this.txtLastName.GotFocus += new System.EventHandler(this.txtLastName_GotFocus);
            this.txtLastName.LostFocus += new System.EventHandler(this.txtLastName_LostFocus);
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(116, 61);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(100, 21);
            this.txtShortName.TabIndex = 3;
            this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
            this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
            // 
            // txtTitle
            // 
            this.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTitle.Location = new System.Drawing.Point(116, 83);
            this.txtTitle.MaxLength = 50;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(100, 21);
            this.txtTitle.TabIndex = 4;
            this.txtTitle.GotFocus += new System.EventHandler(this.txtTitle_GotFocus);
            this.txtTitle.LostFocus += new System.EventHandler(this.txtTitle_LostFocus);
            // 
            // txtEMail
            // 
            this.txtEMail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEMail.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtEMail.Location = new System.Drawing.Point(116, 105);
            this.txtEMail.MaxLength = 50;
            this.txtEMail.Name = "txtEMail";
            this.txtEMail.Size = new System.Drawing.Size(100, 21);
            this.txtEMail.TabIndex = 5;
            this.txtEMail.GotFocus += new System.EventHandler(this.txtEMail_GotFocus);
            this.txtEMail.LostFocus += new System.EventHandler(this.txtEMail_LostFocus);
            // 
            // txtTelephoneWork
            // 
            this.txtTelephoneWork.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelephoneWork.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTelephoneWork.Location = new System.Drawing.Point(116, 128);
            this.txtTelephoneWork.MaxLength = 50;
            this.txtTelephoneWork.Name = "txtTelephoneWork";
            this.txtTelephoneWork.Size = new System.Drawing.Size(100, 21);
            this.txtTelephoneWork.TabIndex = 6;
            this.txtTelephoneWork.GotFocus += new System.EventHandler(this.txtTelephoneWork_GotFocus);
            this.txtTelephoneWork.LostFocus += new System.EventHandler(this.txtTelephoneWork_LostFocus);
            // 
            // txtTelephoneHome
            // 
            this.txtTelephoneHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelephoneHome.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTelephoneHome.Location = new System.Drawing.Point(116, 208);
            this.txtTelephoneHome.MaxLength = 50;
            this.txtTelephoneHome.Name = "txtTelephoneHome";
            this.txtTelephoneHome.Size = new System.Drawing.Size(100, 21);
            this.txtTelephoneHome.TabIndex = 9;
            this.txtTelephoneHome.GotFocus += new System.EventHandler(this.txtTelephoneHome_GotFocus);
            this.txtTelephoneHome.LostFocus += new System.EventHandler(this.txtTelephoneHome_LostFocus);
            // 
            // txtTelephoneCell
            // 
            this.txtTelephoneCell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTelephoneCell.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTelephoneCell.Location = new System.Drawing.Point(116, 164);
            this.txtTelephoneCell.MaxLength = 50;
            this.txtTelephoneCell.Name = "txtTelephoneCell";
            this.txtTelephoneCell.Size = new System.Drawing.Size(100, 21);
            this.txtTelephoneCell.TabIndex = 7;
            this.txtTelephoneCell.GotFocus += new System.EventHandler(this.txtTelephoneCell_GotFocus);
            this.txtTelephoneCell.LostFocus += new System.EventHandler(this.txtTelephoneCell_LostFocus);
            // 
            // txtPager
            // 
            this.txtPager.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPager.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPager.Location = new System.Drawing.Point(116, 186);
            this.txtPager.MaxLength = 50;
            this.txtPager.Name = "txtPager";
            this.txtPager.Size = new System.Drawing.Size(100, 21);
            this.txtPager.TabIndex = 8;
            this.txtPager.GotFocus += new System.EventHandler(this.txtPager_GotFocus);
            this.txtPager.LostFocus += new System.EventHandler(this.txtPager_LostFocus);
            // 
            // txtFax
            // 
            this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFax.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFax.Location = new System.Drawing.Point(116, 230);
            this.txtFax.MaxLength = 50;
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(100, 21);
            this.txtFax.TabIndex = 10;
            this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
            this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Location = new System.Drawing.Point(38, 278);
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
            this.btnClose.Location = new System.Drawing.Point(116, 278);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label13
            // 
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(64, 128);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(12, 8);
            this.label13.TabIndex = 56;
            this.label13.Text = "*";
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(4, 260);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(98, 16);
            this.label17.TabIndex = 179;
            this.label17.Text = "* Required Field";
            // 
            // label14
            // 
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(51, 164);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(12, 8);
            this.label14.TabIndex = 60;
            this.label14.Text = "*";
            // 
            // CreateClientTrader
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(241, 324);
            this.ControlBox = false;
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MinimumSize = new System.Drawing.Size(234, 340);
            this.Name = "CreateClientTrader";
            this.Text = " Client Trader";
            this.Load += new System.EventHandler(this.CreateClientTrader_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private int _companyID = int.MinValue;
        public int CompanyID
        {
            set { _companyID = value; }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public void BindForEdit()
        {
            if (_traderEdit != null)
            {
                txtEMail.Text = _traderEdit.EMail;
                txtFax.Text = _traderEdit.Fax;
                txtFirstName.Text = _traderEdit.FirstName;
                txtLastName.Text = _traderEdit.LastName;
                txtPager.Text = _traderEdit.Pager;
                txtShortName.Text = _traderEdit.ShortName;
                txtTelephoneCell.Text = _traderEdit.TelephoneCell;
                txtTelephoneHome.Text = _traderEdit.TelephoneHome;
                txtTelephoneWork.Text = _traderEdit.TelephoneWork;
                txtTitle.Text = _traderEdit.Title;
            }

        }

        public void ResetErrorMsg()
        {
            errorProvider1.SetError(txtFirstName, "");
            errorProvider1.SetError(txtShortName, "");
            errorProvider1.SetError(txtEMail, "");
            errorProvider1.SetError(txtTelephoneWork, "");
            errorProvider1.SetError(txtTelephoneCell, "");
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            //Regex emailRegex = new Regex("(?<user>[^@]+)@(?<host>.+)");
            string emailCheck = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex emailRegex = new Regex(emailCheck);
            Match emailMatch = emailRegex.Match(txtEMail.Text.ToString());
            //Editing  Trading Account
            if (_traderEdit != null)
            {
                errorProvider1.SetError(txtFirstName, "");
                errorProvider1.SetError(txtShortName, "");
                errorProvider1.SetError(txtEMail, "");
                errorProvider1.SetError(txtTelephoneWork, "");
                if (txtFirstName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtFirstName, "Please enter Trader First Name!");
                    txtFirstName.Focus();
                }
                else if (txtShortName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                    txtShortName.Focus();
                }
                //				else if(txtEMail.Text.Trim() == "")
                //				{
                //					errorProvider1.SetError(txtEMail, "Please enter Short Name!");
                //					txtEMail.Focus();
                //				}
                else if (!emailMatch.Success)
                {
                    errorProvider1.SetError(txtEMail, "Please enter valid Email address!");
                    txtEMail.Focus();
                }
                else if (txtTelephoneWork.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtTelephoneWork, "Please enter Work telephone!");
                    txtTelephoneWork.Focus();
                }
                else if (txtTelephoneCell.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtTelephoneCell, "Please enter Cell telephone!");
                    txtTelephoneCell.Focus();
                }
                else
                {
                    if (_traders.ContainsShortName(txtShortName.Text.ToString().Trim().ToUpper()))
                    {
                        if (!(_traderEdit.ShortName.ToUpper() == txtShortName.Text.ToString().Trim().ToUpper()))
                        {
                            MessageBox.Show("Already Contains this Short Name");
                            return;
                        }

                    }
                    _traderEdit.EMail = txtEMail.Text.ToString().Trim();
                    _traderEdit.Fax = txtFax.Text.ToString();
                    _traderEdit.FirstName = txtFirstName.Text.ToString().Trim();
                    _traderEdit.LastName = txtLastName.Text.ToString().Trim();
                    _traderEdit.Pager = txtPager.Text.ToString().Trim();
                    _traderEdit.ShortName = txtShortName.Text.ToString().Trim();
                    _traderEdit.TelephoneCell = txtTelephoneCell.Text.ToString().Trim();
                    _traderEdit.TelephoneHome = txtTelephoneHome.Text.ToString().Trim();
                    _traderEdit.TelephoneWork = txtTelephoneWork.Text.ToString().Trim();
                    _traderEdit.Title = txtTitle.Text.ToString().Trim();


                    //Prana.Admin.Utility.Common.ResetStatusPanel(stbClientTrader);
                    //Prana.Admin.Utility.Common.SetStatusPanel(stbClientTrader, "Stored!");


                    this.Hide();
                }

            }
            //Creating New Trading Account
            else
            {

                errorProvider1.SetError(txtFirstName, "");
                errorProvider1.SetError(txtShortName, "");
                errorProvider1.SetError(txtEMail, "");
                errorProvider1.SetError(txtTelephoneWork, "");
                errorProvider1.SetError(txtTelephoneCell, "");
                if (txtFirstName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtFirstName, "Please enter Trader First Name!");
                    txtFirstName.Focus();
                }
                else if (txtShortName.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtShortName, "Please enter Short Name!");
                    txtShortName.Focus();
                }
                else if (txtEMail.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtEMail, "Please enter valid Email address!");
                    txtEMail.Focus();
                }
                //				else if(txtTelephoneWork.Text.Trim() == "")
                //				{
                //					errorProvider1.SetError(txtTelephoneWork, "Please enter Short Name!");
                //					txtTelephoneWork.Focus();
                //				}
                else if (!emailMatch.Success)
                {
                    errorProvider1.SetError(txtEMail, "Please enter valid Email address!");
                    txtEMail.Focus();
                }
                else if (txtTelephoneWork.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtTelephoneWork, "Please enter Work telephone!");
                    txtTelephoneWork.Focus();
                }
                else if (txtTelephoneCell.Text.Trim() == "")
                {
                    errorProvider1.SetError(txtTelephoneCell, "Please enter Cell telephone!");
                    txtTelephoneCell.Focus();
                }
                else
                {
                    Trader trader = new Trader();

                    trader.EMail = txtEMail.Text.ToString().Trim();
                    trader.Fax = txtFax.Text.ToString().Trim();
                    trader.FirstName = txtFirstName.Text.ToString().Trim();
                    trader.LastName = txtLastName.Text.ToString().Trim();
                    trader.Pager = txtPager.Text.ToString().Trim();
                    trader.ShortName = txtShortName.Text.ToString().Trim();
                    trader.TelephoneCell = txtTelephoneCell.Text.ToString().Trim();
                    trader.TelephoneHome = txtTelephoneHome.Text.ToString().Trim();
                    trader.TelephoneWork = txtTelephoneWork.Text.ToString().Trim();
                    trader.Title = txtTitle.Text.ToString();
                    if (_traders.ContainsShortName(trader.ShortName))
                    {
                        MessageBox.Show("Already Contains this Short Name");
                        return;
                    }
                    _traders.Add(trader);
                    //Prana.Admin.Utility.Common.ResetStatusPanel(stbClientTrader);
                    //Prana.Admin.Utility.Common.SetStatusPanel(stbClientTrader, "Stored!");
                    ResetErrorMsg();
                    this.Hide();
                }
            }

        }

        public void Refresh(object sender, System.EventArgs e)
        {
            txtEMail.Text = "";
            txtFax.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPager.Text = "";
            txtShortName.Text = "";
            txtTelephoneCell.Text = "";
            txtTelephoneHome.Text = "";
            txtTelephoneWork.Text = "";
            txtTitle.Text = "";
        }

        private void CreateClientTrader_Load(object sender, System.EventArgs e)
        {
            if (_traderEdit != null)
            {
                BindForEdit();
            }
            else
            {
                Refresh(sender, e);
                //Prana.Admin.Utility.Common.ResetStatusPanel(stbClientTrader);
            }
        }

        private int _noData = int.MinValue;
        public int NoData
        {
            set
            {
                _noData = value;
            }
        }

        private Prana.Admin.BLL.Traders _traders = new Traders();
        public Traders Traders
        {
            get
            {
                return _traders;
            }
            set
            {
                if (value != null)
                {
                    _traders = value;
                }
            }
        }
        #region Focus Colors
        private void txtFirstName_GotFocus(object sender, System.EventArgs e)
        {
            txtFirstName.BackColor = Color.LemonChiffon;
        }
        private void txtFirstName_LostFocus(object sender, System.EventArgs e)
        {
            txtFirstName.BackColor = Color.White;
        }
        private void txtLastName_GotFocus(object sender, System.EventArgs e)
        {
            txtLastName.BackColor = Color.LemonChiffon;
        }
        private void txtLastName_LostFocus(object sender, System.EventArgs e)
        {
            txtLastName.BackColor = Color.White;
        }
        private void txtShortName_GotFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.LemonChiffon;
        }
        private void txtShortName_LostFocus(object sender, System.EventArgs e)
        {
            txtShortName.BackColor = Color.White;
        }
        private void txtTitle_GotFocus(object sender, System.EventArgs e)
        {
            txtTitle.BackColor = Color.LemonChiffon;
        }
        private void txtTitle_LostFocus(object sender, System.EventArgs e)
        {
            txtTitle.BackColor = Color.White;
        }
        private void txtEMail_GotFocus(object sender, System.EventArgs e)
        {
            txtEMail.BackColor = Color.LemonChiffon;
        }
        private void txtEMail_LostFocus(object sender, System.EventArgs e)
        {
            txtEMail.BackColor = Color.White;
        }
        private void txtTelephoneWork_GotFocus(object sender, System.EventArgs e)
        {
            txtTelephoneWork.BackColor = Color.LemonChiffon;
        }
        private void txtTelephoneWork_LostFocus(object sender, System.EventArgs e)
        {
            txtTelephoneWork.BackColor = Color.White;
        }
        private void txtTelephoneHome_GotFocus(object sender, System.EventArgs e)
        {
            txtTelephoneHome.BackColor = Color.LemonChiffon;
        }
        private void txtTelephoneHome_LostFocus(object sender, System.EventArgs e)
        {
            txtTelephoneHome.BackColor = Color.White;
        }
        private void txtTelephoneCell_GotFocus(object sender, System.EventArgs e)
        {
            txtTelephoneCell.BackColor = Color.LemonChiffon;
        }
        private void txtTelephoneCell_LostFocus(object sender, System.EventArgs e)
        {
            txtTelephoneCell.BackColor = Color.White;
        }
        private void txtPager_GotFocus(object sender, System.EventArgs e)
        {
            txtPager.BackColor = Color.LemonChiffon;
        }
        private void txtPager_LostFocus(object sender, System.EventArgs e)
        {
            txtPager.BackColor = Color.White;
        }
        private void txtFax_GotFocus(object sender, System.EventArgs e)
        {
            txtFax.BackColor = Color.LemonChiffon;
        }
        private void txtFax_LostFocus(object sender, System.EventArgs e)
        {
            txtFax.BackColor = Color.White;
        }
        #endregion

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }
    }
}
