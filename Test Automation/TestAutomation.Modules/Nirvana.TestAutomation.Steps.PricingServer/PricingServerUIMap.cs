using System;
using System.ComponentModel;

using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.PricingServer
{
    [UITestFixture]
    public partial class PricingServerUIMap : UIMap
    {
        public PricingServerUIMap()
        {
            InitializeComponent();
        }

        public PricingServerUIMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
