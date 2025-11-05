using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class AddUploadClient : Form
    {
        public AddUploadClient()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Populates the child controls.
        /// </summary>
        public void PopulateChildControls()
        {
            this.ctrlAddUploadClient1.PopulateControl();
        }
    }
}