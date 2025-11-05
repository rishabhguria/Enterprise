using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Prana.Client.Installer.UpdateService
{
    public partial class UpdatesDialog : Form
    {
        public UpdatesDialog()
        {
            InitializeComponent();
        }


        public UpdatesDialog(string latestVersion)
        {
            InitializeComponent();
            this.lblLatestPatchUpdate.Text = latestVersion;
        }
    }
}