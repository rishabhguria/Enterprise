using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.PM.BLL;
//using Prana.PM.Common;
using Prana.PM.Admin.UI.Controls;
using Prana.PM.Admin.UI.Forms;

namespace Prana.PM.Admin.UI.Forms
{
    public partial class SelectColumns : Form
    {
        
        public SelectColumns()
        {
            InitializeComponent();
        }

        public SelectColumns(Prana.BusinessObjects.PositionManagement.DataSourceNameID dataSourceNameID)
        {
            InitializeComponent();
            ctrlSetupColumns1.InitControl(dataSourceNameID);
        }
        
    }
}