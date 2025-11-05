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
        public MapColumns(DataSourceNameID dataSourceNameID)
        {
            InitializeComponent();
            ctrlMapColumns1.InitControl(dataSourceNameID);
        }
    }
}