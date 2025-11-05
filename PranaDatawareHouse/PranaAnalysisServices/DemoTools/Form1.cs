using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Runtime;

namespace DemoTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {

            Microsoft.SqlServer.Dts.Runtime.Application app = new Microsoft.SqlServer.Dts.Runtime.Application();
            Package package = app.LoadPackage("G:\\NirvanaSourceCode\\SourceCode\\Dev\\Prana_CA\\PranaDatawareHouse\\PranaAnalysisServices\\Integration Services Project\\bin\\Package.dtsx", null);
           // package.ImportConfigurationFile("c:\\ExamplePackage.dtsConfig");
           // Variables vars = package.Variables;
           // vars["MyVariable"].Value = "value from c#";

            DTSExecResult result = package.Execute();

            MessageBox.Show("Package Execution results: {0}"+ result.ToString());

        }
    }
}