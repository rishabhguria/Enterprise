using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.PranaClient
{
    [UITestFixture]
    public partial class PranaClientUIMap : UIMap
    {
        public PranaClientUIMap()
        {
            InitializeComponent();
        }

        public PranaClientUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
