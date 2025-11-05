using System;
using System.ComponentModel;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Unknown
{
    [UITestFixture]
    public partial class BlotterUIMap : UIMap
    {
        public BlotterUIMap()
        {
            InitializeComponent();
        }

        public BlotterUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
