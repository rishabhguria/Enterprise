using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.PM.BLL;

namespace Prana.PM.Admin.UI.Forms
{
    public partial class MapFunds : Form
    {
        public MapFunds()
        {
            InitializeComponent();
        }

        public MapFunds(Prana.BusinessObjects.PositionManagement.DataSourceNameID dataSourceNameID)
        {
            InitializeComponent();
           // ctrlMapFunds1.InitControl(dataSourceNameID);
        }
    }
}