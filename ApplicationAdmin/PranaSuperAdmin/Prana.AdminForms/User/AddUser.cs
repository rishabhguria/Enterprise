using Prana.Admin.BLL;
using System.Drawing;

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for AddUser.
    /// </summary>
    public class AddUser : System.Windows.Forms.Form
    {
        private System.Windows.Forms.StatusBar stbAddUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAddUser;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox grpboxAddUser;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusBarPanel statusBarPanel1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public AddUser()
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
                if (stbAddUser != null)
                {
                    stbAddUser.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (txtAddUser != null)
                {
                    txtAddUser.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (grpboxAddUser != null)
                {
                    grpboxAddUser.Dispose();
                }
                if (statusBarPanel1 != null)
                {
                    statusBarPanel1.Dispose();
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
            this.stbAddUser = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.grpboxAddUser = new System.Windows.Forms.GroupBox();
            this.txtAddUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            this.grpboxAddUser.SuspendLayout();
            this.SuspendLayout();
            // 
            // stbAddUser
            // 
            this.stbAddUser.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.stbAddUser.Location = new System.Drawing.Point(0, 73);
            this.stbAddUser.Name = "stbAddUser";
            this.stbAddUser.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1});
            this.stbAddUser.Size = new System.Drawing.Size(262, 22);
            this.stbAddUser.TabIndex = 0;
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
            this.statusBarPanel1.Text = "statusBarPanel1";
            // 
            // grpboxAddUser
            // 
            this.grpboxAddUser.Controls.Add(this.txtAddUser);
            this.grpboxAddUser.Controls.Add(this.label1);
            this.grpboxAddUser.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.grpboxAddUser.Location = new System.Drawing.Point(5, 2);
            this.grpboxAddUser.Name = "grpboxAddUser";
            this.grpboxAddUser.Size = new System.Drawing.Size(250, 46);
            this.grpboxAddUser.TabIndex = 1;
            this.grpboxAddUser.TabStop = false;
            // 
            // txtAddUser
            // 
            this.txtAddUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddUser.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.txtAddUser.Location = new System.Drawing.Point(120, 20);
            this.txtAddUser.MaxLength = 50;
            this.txtAddUser.Name = "txtAddUser";
            this.txtAddUser.Size = new System.Drawing.Size(104, 21);
            this.txtAddUser.TabIndex = 1;
            this.txtAddUser.GotFocus += new System.EventHandler(this.txtAddUser_GotFocus);
            this.txtAddUser.LostFocus += new System.EventHandler(this.txtAddUser_LostFocus);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.label1.Location = new System.Drawing.Point(24, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "ADD User";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(60, 50);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 24);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(125, 50);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 24);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddUser
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(262, 95);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpboxAddUser);
            this.Controls.Add(this.stbAddUser);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User - Add";
            this.Load += new System.EventHandler(this.AddUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            this.grpboxAddUser.ResumeLayout(false);
            this.grpboxAddUser.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            //TODO: Add User

            Prana.Admin.BLL.User user = new Prana.Admin.BLL.User();
            user.FirstName = txtAddUser.Text.Trim();

            UserManager.AddUser(user);

            //Prana.Admin.Utility.Common.ResetStatusPanel(stbAddUser);
            //Prana.Admin.Utility.Common.SetStatusPanel(stbAddUser, "User Added!");

        }

        private void AddUser_Load(object sender, System.EventArgs e)
        {

        }

        private void txtAddUser_GotFocus(object sender, System.EventArgs e)
        {
            txtAddUser.BackColor = Color.LemonChiffon;
        }
        private void txtAddUser_LostFocus(object sender, System.EventArgs e)
        {
            txtAddUser.BackColor = Color.White;
        }

    }
}
