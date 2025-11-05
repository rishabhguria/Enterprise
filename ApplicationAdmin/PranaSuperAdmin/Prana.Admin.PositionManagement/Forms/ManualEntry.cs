using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class ManualEntry : Form
    {
        public ManualEntry()
        {
            InitializeComponent();
            ctrlManualEntry1.InitControl();
        }

        private void ctrlManualEntry1_Load(object sender, EventArgs e)
        {
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            //ctrlManualEntry1.ManualEntryClosed += new EventHandler(ctrlManualEntry1_ManualEntryClosed);
        }

        //void ctrlManualEntry1_ManualEntryClosed(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        

    }
}