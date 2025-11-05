using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace Prana.Client.Installer.UpdateService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}