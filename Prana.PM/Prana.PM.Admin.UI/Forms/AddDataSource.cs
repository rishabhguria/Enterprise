using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Prana.PM.Admin.UI.Forms
{
    public partial class AddDataSource : Form
    {
        public AddDataSource()
        {
            InitializeComponent();
        }

        //private PM.BLL.DataSourceNameIDList dataSourceNameIDList = PM.BLL.DataSourceNameIDList.GetInstance();
        private Prana.BusinessObjects.PositionManagement.DataSourceNameID dataSource = new Prana.BusinessObjects.PositionManagement.DataSourceNameID();
        
        private void AddDataSource_Load(object sender, EventArgs e)
        {           
            string s = string.Empty;

            dataSource.FullName = "";
            dataSource.ShortName = "";
            ctrlAddDataSource1.DataSource = dataSource;
            //ctrlAddDataSource1.DataMember = "DataSourceNameID";
            ctrlAddDataSource1.PopulateDetails();
        }

        
       
    }
}