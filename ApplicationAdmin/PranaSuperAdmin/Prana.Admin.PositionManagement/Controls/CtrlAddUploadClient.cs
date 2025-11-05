using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;


namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlAddUploadClient : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlAddUploadClient"/> class.
        /// </summary>
        public CtrlAddUploadClient()
        {
            InitializeComponent();
        }



        /// <summary>
        /// Populates the control.
        /// sets the list of companies as the datasource for the bindingSource in the control, which in tunr is the 
        /// DataSource for the combo containing list of companies.
        /// </summary>
        public void PopulateControl()
        {
            bindingSourceCompanyList.DataSource = CompanyNameIDList.Retrieve();
            
        }


        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //TOdo--- code to add new upload client into DataBase.
        }

        
    }
}
