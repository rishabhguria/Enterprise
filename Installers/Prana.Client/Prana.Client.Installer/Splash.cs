using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Prana.Client.Installer.Utilities;

namespace Prana.Client.Installer
{
    public partial class Splash : Form
    {

        public Splash()
        {
            InitializeComponent();
            BackgroundImage = WhiteLabelHelper.InstallerSplash;
        }
    }
}