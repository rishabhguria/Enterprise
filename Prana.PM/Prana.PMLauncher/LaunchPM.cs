using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Prana.PM.Admin.UI.Forms;
using Prana.PM.Client.UI.Forms;

namespace Prana.PMLauncher
{
    public partial class LaunchPM : Form
    {
        public LaunchPM()
        {
            InitializeComponent();
        }

        private void btnLaunchPM_Click(object sender, EventArgs e)
        {
            PMMain pmMain = new PMMain();
            pmMain.Show();
        }

        private void btnTestGrid_Click(object sender, EventArgs e)
        {
            GridTestForm gridTestForm = new GridTestForm();
            gridTestForm.Show();

            //SplitterExample frm1 = new SplitterExample(); //BB
            //frm1.Show(); //BB
        }

        private void btnNewPMMain_Click(object sender, EventArgs e)
        {
            Prana.PM.Client.UI.Forms.PM pmNew=  new Prana.PM.Client.UI.Forms.PM();
            Prana.BusinessObjects.CompanyUser loginUser= new Prana.BusinessObjects.CompanyUser();
            loginUser.CompanyID = 1;
            loginUser.CompanyUserID = 1;

            pmNew.LoginUser = loginUser;
            pmNew.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ErrorProviderCheck errorProviderCheck = new ErrorProviderCheck();
            errorProviderCheck.Show();
        }

        private void ultraButton1_Click(object sender, EventArgs e)
        {
            //Drag_CloseColumnsTest frmDrag_CloseColumnsTest = new Drag_CloseColumnsTest();
            //frmDrag_CloseColumnsTest.Show();
        }
    }
}