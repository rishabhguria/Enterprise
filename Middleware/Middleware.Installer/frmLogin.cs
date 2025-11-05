using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
namespace Middleware.Installer
{
    public partial class frmLogin : Form
    {
        public string ConnectString { get; set; }

        const string dsn1 = "Data Source={0};Initial Catalog={1};Integrated Security=True;";

        const string dsn2 = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}";
        
        public frmLogin()
        {
            InitializeComponent();
        }

     

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            cbServer.SelectedIndex = 0;
            cbAuth.SelectedIndex = 1;

            if (File.Exists(".\\Servers.txt"))
            {
                string[] servers = File.ReadAllLines(".\\Servers.txt");
                cbServer.Items.AddRange(servers);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectString = GetString(cbServer.Text, cbDatabase.Text, txtUserName.Text, txtPassword.Text);
        }

        public string GetString(string server, string database, string user, string password)
        {
            if (cbAuth.SelectedIndex == 1)
            {
                return  string.Format(dsn2, server, database, user, password);
            }
            else
            {
                return string.Format(dsn1, server, database);
            }
        }
        private void cbDatabase_DropDown(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(GetString(cbServer.Text, "master", txtUserName.Text, txtPassword.Text)))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("Exec sp_databases", cn);
                    var reader = cmd.ExecuteReader();
                                        
                    cbDatabase.Items.Clear();

                    while (reader.Read())
                        cbDatabase.Items.Add(reader.GetString(0));
                }

                ConnectString = GetString(cbServer.Text, cbDatabase.Text, txtUserName.Text, txtPassword.Text);
            }
            catch (Exception)
            {
                return;
            }

        }
    }
}
