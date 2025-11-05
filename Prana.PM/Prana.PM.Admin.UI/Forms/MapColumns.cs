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
    public partial class MapColumns : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapColumns"/> class.
        /// </summary>
        public MapColumns()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapColumns"/> class.
        /// </summary>
        /// <param name="dataSourceNameID">The data source name ID.</param>
        public MapColumns(Prana.BusinessObjects.PositionManagement.DataSourceNameID dataSourceNameID)
        {
            InitializeComponent();
            ctrlMapColumns1.InitControl(dataSourceNameID);
        }
    }
}