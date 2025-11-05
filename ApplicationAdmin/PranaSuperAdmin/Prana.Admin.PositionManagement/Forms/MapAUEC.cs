using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class MapAUEC : Form
    {
        public MapAUEC()
        {
            InitializeComponent();
        }

        public MapAUEC(DataSourceNameID dataSourceNameID)
        {
            InitializeComponent();
            ctrlMapAUEC1.InitControl(dataSourceNameID);
        }
    }
}