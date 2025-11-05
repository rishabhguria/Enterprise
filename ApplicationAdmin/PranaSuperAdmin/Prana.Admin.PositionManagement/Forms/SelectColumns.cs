using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Controls;
using Nirvana.Admin.PositionManagement.Forms;
using Nirvana.Admin.PositionManagement.Properties;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class SelectColumns : Form
    {
        
        public SelectColumns()
        {
            InitializeComponent();
        }

        public SelectColumns(DataSourceNameID dataSourceNameID)
        {
            InitializeComponent();
            ctrlSetupColumns1.InitControl(dataSourceNameID);
        }
        
    }
}