using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Nirvana.Admin.PositionManagement.Forms
{
    public partial class ManualEntryPassword : Form
    {
        public ManualEntryPassword()
        {
            InitializeComponent();
        }
        public ManualEntryPassword(string screenName)
        {
            InitializeComponent();
            this.ctrlManualEntryPassword1.FormType = screenName;
        }
    }
}