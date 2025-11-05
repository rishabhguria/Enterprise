using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Import
{
    [UITestFixture]
    public partial class ImportUIMap : UIMap
    {
        public ImportUIMap()
        {
            InitializeComponent();
        }

        public ImportUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        public  void TradesUpload()
        {
            try
            {

                bool isWindowVisible = ExtentionMethods.ClickWindowAndWaitForExpectedWindow(ref BtnUpload, ref UltraPanel1ClientArea3, TimeSpan.FromSeconds(6));
                Console.WriteLine(ExtentionMethods.GetActiveWindowTitle());

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
